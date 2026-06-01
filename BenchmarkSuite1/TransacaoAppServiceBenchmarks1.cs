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
        private TransacaoAppService _service !;
        private const string Phone = "+5511999999999";
        [GlobalSetup]
        public void Setup()
        {
            var usuarioRepo = new InMemoryUsuarioRepository();
            var transacaoRepo = new InMemoryTransacaoRepository();
            var usuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = "Usuário Benchmark",
                Telefone = Phone,
                Email = "bench@local"
            };
            usuarioRepo.Add(usuario);
            var now = DateTime.UtcNow;
            transacaoRepo.Add(new Transacao { Id = Guid.NewGuid(), UsuarioId = usuario.Id, Valor = 100m, Tipo = TipoTransacao.Receita, Categoria = "Salario", DataCriacao = now });
            transacaoRepo.Add(new Transacao { Id = Guid.NewGuid(), UsuarioId = usuario.Id, Valor = 50m, Tipo = TipoTransacao.Despesa, Categoria = "Alimentacao", DataCriacao = now });
            transacaoRepo.Add(new Transacao { Id = Guid.NewGuid(), UsuarioId = usuario.Id, Valor = 20m, Tipo = TipoTransacao.Despesa, Categoria = "Transporte", DataCriacao = now });
            _service = new TransacaoAppService(transacaoRepo, usuarioRepo);
        }

        [Benchmark]
        public async Task ProcessarMensagem_Gastei()
        {
            await _service.ProcessarMensagemAsync(Phone, "gastei 50 ifood");
        }

        [Benchmark]
        public async Task ProcessarMensagem_Relatorio()
        {
            await _service.ProcessarMensagemAsync(Phone, "relatorio");
        }

        // Repositórios em memória simples para uso no benchmark
        private class InMemoryUsuarioRepository : IUsuarioRepository
        {
            private readonly List<Usuario> _usuarios = new();
            private readonly object _lock = new();
            public Task AdicionarAsync(Usuario usuario)
            {
                lock (_lock)
                {
                    _usuarios.Add(usuario);
                }

                return Task.CompletedTask;
            }

            public void Add(Usuario usuario)
            {
                lock (_lock)
                {
                    _usuarios.Add(usuario);
                }
            }

            public Task<Usuario?> ObterPorTelefoneAsync(string telefone)
            {
                lock (_lock)
                {
                    return Task.FromResult(_usuarios.FirstOrDefault(u => u.Telefone == telefone));
                }
            }

            public Task<bool> ExisteEmailAsync(string email)
            {
                lock (_lock)
                {
                    return Task.FromResult(_usuarios.Any(u => u.Email == email));
                }
            }
        }

        private class InMemoryTransacaoRepository : ITransacaoRepository
        {
            private readonly List<Transacao> _transacoes = new();
            private readonly object _lock = new();
            public Task AdicionarAsync(Transacao transacao)
            {
                lock (_lock)
                {
                    _transacoes.Add(transacao);
                }

                return Task.CompletedTask;
            }

            public void Add(Transacao t)
            {
                lock (_lock)
                {
                    _transacoes.Add(t);
                }
            }

            public Task<decimal> CalcularSaldoAsync(Guid usuarioId)
            {
                decimal acc = 0m;
                lock (_lock)
                {
                    for (int i = 0; i < _transacoes.Count; i++)
                    {
                        var t = _transacoes[i];
                        if (t.UsuarioId == usuarioId)
                            acc += (t.Tipo == TipoTransacao.Receita) ? t.Valor : -t.Valor;
                    }
                }

                return Task.FromResult(acc);
            }

            public Task<IEnumerable<object>> ObterPorUsuarioIdAsync(Guid usuarioId)
            {
                object[] snapshot;
                lock (_lock)
                {
                    var list = _transacoes.Where(t => t.UsuarioId == usuarioId).Cast<object>().ToArray();
                    snapshot = list;
                }

                return Task.FromResult<IEnumerable<object>>(snapshot);
            }

            // Implementações compatíveis para evitar NotImplementedException
            public Task<Usuario?> ObterUsuarioPorTelefoneAsync(string telefone) => Task.FromResult<Usuario?>(null);
            public Task<string> ProcesssarMensagemAsync(string telefone, string mensagemRecebida) => Task.FromResult(string.Empty);
        }
    }
}