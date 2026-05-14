using GestorFinanceiro.Domain.Entities;
using GestorFinanceiro.Domain.Interfaces;
using GestorFinanceiro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestorFinanceiro.Infrastructure.Repositories;

// Veja que colocamos o ": ITransacaoRepository", assumindo o compromisso do contrato
public class TransacaoRepository : ITransacaoRepository {
    private readonly AppDbContext _context;

    public TransacaoRepository(AppDbContext context) {
        _context = context;
    }

    public async Task AdicionarAsync(Transacao transacao) {
        await _context.Transacoes.AddAsync(transacao);
        await _context.SaveChangesAsync(); // É isso que de fato dá o "Commit" no banco
    }

    public async Task<Usuario> ObterUsuarioPorTelefoneAsync(string telefone) {
        // Vai no banco, procura na tabela de usuários onde o telefone bate com o do WhatsApp
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.TelefoneWhatsapp == telefone);
    }


    public async Task<decimal> CalcularSaldoAsync(Guid usuarioId) {
        // O Entity Framework faz a conta direto no banco de dados!
        var receitas = await _context.Transacoes
            .Where(t => t.UsuarioId == usuarioId && t.Tipo == GestorFinanceiro.Domain.Enums.TipoTransacao.Receita)
            .SumAsync(t => t.Valor);

        var despesas = await _context.Transacoes
            .Where(t => t.UsuarioId == usuarioId && t.Tipo == GestorFinanceiro.Domain.Enums.TipoTransacao.Despesa)
            .SumAsync(t => t.Valor);

        return receitas - despesas;
    }
}