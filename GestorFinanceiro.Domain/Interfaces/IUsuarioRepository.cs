using GestorFinanceiro.Domain.Entities;

namespace GestorFinanceiro.Domain.Interfaces;

public interface IUsuarioRepository {
    Task AdicionarAsync(Usuario usuario);
    Task<Usuario?> ObterPorTelefoneAsync(string telefone);

    // Método adicional para verificar se um email já existe, caso queira implementar essa validação no futuro
    Task<bool> ExisteEmailAsync(string email);

}