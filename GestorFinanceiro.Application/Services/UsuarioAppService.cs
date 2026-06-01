using GestorFinanceiro.Domain.Entities;
using GestorFinanceiro.Domain.Interfaces;
using System.Security.Cryptography.X509Certificates;

namespace GestorFinanceiro.Application.Services;

public class UsuarioAppService {
    private readonly IUsuarioRepository _usuarioRepository;
    public UsuarioAppService(IUsuarioRepository usuarioRepository) {
        _usuarioRepository = usuarioRepository;
    }

    // Método para verificar se um usuário já existe pelo email
    // === Aplicado no IUsuarioRepository e no UsuarioRepository ===
    public async Task<bool> VerificarSeUsuarioExisteAsync(string email) {
               return await _usuarioRepository.ExisteEmailAsync(email);
    }

    public async Task<string> CadastrarUsuarioAsync(string nome, string cpf, string email, string telefone) {

        {
            // Verificar se ja existe numero cadastrado
            // === Aplicado no IUsuarioRepository e no UsuarioRepository ===
            var usuarioExistente = await _usuarioRepository.ObterPorTelefoneAsync(telefone);
            if (usuarioExistente != null)
                return "Erro, Este numero de WhatsApp ja esta cadastrado em nosso sistema.";

            // criar a entidade respeitando o Dominio
            var novoUsuario = new Usuario(nome, cpf, email, telefone);

            await _usuarioRepository.AdicionarAsync(novoUsuario);
            return $"Sucesso! Usuario {nome} cadastrado. Voce já pode enviar suas despesas pelo WhatsApp";

            
        }
    }
}
