using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using GestorFinanceiro.Application.Services;
using GestorFinanceiro.Domain.Entities;
using GestorFinanceiro.Domain.Interfaces;
using GestorFinanceiro.Domain.Enums;
using Microsoft.VSDiagnostics;

namespace GestorFinanceiro.Benchmarks
{
    [CPUUsageDiagnoser]
    public class TransacaoAppServiceBenchmarks
    {
        private TransacaoAppService _service;
        [GlobalSetup]
        public void Setup()
        {
            var usuarioRepo = new InMemoryUsuarioRepository();
            var transacaoRepo = new InMemoryTransacaoRepository();
            var usuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = "Usuário Benchmark",
                Telefone = "+5511999999999",
                Email = "bench@local"
            };
            usuarioRepo.Add(usuario);
            // Preenche com algumas transações para o relatório
            transacaoRepo.Add(new Transacao { Id = Guid.NewGuid(), UsuarioId = usuario.Id, Valor = 100, Tipo = TipoTransacao.Receita, Categoria = "Salario", DataCriacao = DateTime.Now });
            transacaoRepo.Add(new Transacao { Id = Guid.NewGuid(), UsuarioId = usuario.Id, Valor = 50, Tipo = TipoTransacao.Despesa, Categoria = "Alimentacao", DataCriacao = DateTime.Now });
            transacaoRepo.Add(new Transacao { Id = Guid.NewGuid(), UsuarioId = usuario.Id, Valor = 20, Tipo = TipoTransacao.Despesa, Categoria = "Transporte", DataCriacao = DateTime.Now });
            _service = new TransacaoAppService(transacaoRepo, usuarioRepo);
        }

        [Benchmark]
        public async Task ProcessarMensagem_Gastei()
        {
            await _service.ProcessarMensagemAsync("+5511999999999", "gastei 50 ifood");
        }

        [Benchmark]
        public async Task ProcessarMensagem_Relatorio()
        {
            await _service.ProcessarMensagemAsync("+5511999999999", "relatorio");
        }

        // Repositórios em memória simples para uso no benchmark
        private class InMemoryUsuarioRepository : IUsuarioRepository
        {
            private readonly List<Usuario> _usuarios = new();
            public Task AdicionarAsync(Usuario usuario)
            {
                _usuarios.Add(usuario);
                return Task.CompletedTask;
            }

            public void Add(Usuario usuario) => _usuarios.Add(usuario);
            public Task<Usuario?> ObterPorTelefoneAsync(string telefone) => Task.FromResult(_usuarios.FirstOrDefault(u => u.Telefone == telefone));
            public Task<bool> ExisteEmailAsync(string email) => Task.FromResult(_usuarios.Any(u => u.Email == email));
        }

        private class InMemoryTransacaoRepository : ITransacaoRepository
        {
            private readonly List<Transacao> _transacoes = new();
            public Task AdicionarAsync(Transacao transacao)
            {
                _transacoes.Add(transacao);
                return Task.CompletedTask;
            }

            public void Add(Transacao t) => _transacoes.Add(t);
            public Task<Usuario> ObterUsuarioPorTelefoneAsync(string telefone) => throw new NotImplementedException();
            public Task<decimal> CalcularSaldoAsync(Guid usuarioId) => Task.FromResult(_transacoes.Where(t => t.UsuarioId == usuarioId).Sum(t => t.Tipo == TipoTransacao.Receita ? t.Valor : -t.Valor));
            public Task<string> ProcesssarMensagemAsync(string telefone, string mensagemRecebida) => throw new NotImplementedException();
            public Task<IEnumerable<object>> ObterPorUsuarioIdAsync(Guid usuarioId) => Task.FromResult(_transacoes.Where(t => t.UsuarioId == usuarioId).Cast<object>());
        }
    }
}