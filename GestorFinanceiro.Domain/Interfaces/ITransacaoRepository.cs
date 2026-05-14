using GestorFinanceiro.Domain.Entities;

namespace GestorFinanceiro.Domain.Interfaces;

public interface ITransacaoRepository {
    Task AdicionarAsync(Transacao transacao);
    Task<Usuario> ObterUsuarioPorTelefoneAsync(string telefone);
    // O método mágico que calcula o saldo do usuário, somando as receitas e subtraindo as despesas. Ele é assíncrono porque pode envolver consultas ao banco de dados.
    Task<decimal> CalcularSaldoAsync(Guid usuarioId);
}