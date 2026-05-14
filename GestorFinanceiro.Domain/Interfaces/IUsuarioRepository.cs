using GestorFinanceiro.Domain.Entities;

namespace GestorFinanceiro.Domain.Interfaces;

public interface IUsuarioRepository {
    Task AdicionarAsync(Usuario usuario);
    Task<Usuario?> ObterPorTelefoneAsync(string telefone);
}