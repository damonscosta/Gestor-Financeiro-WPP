using GestorFinanceiro.Domain.Entities;
using GestorFinanceiro.Domain.Interfaces;

namespace GestorFinanceiro.Application.Services;

public class UsuarioAppService {
    private readonly IUsuarioRepository _usuarioRepository;
    public UsuarioAppService(IUsuarioRepository usuarioRepository) {
        _usuarioRepository = usuarioRepository;
    }



    public async Task<string> CadastrarUsuarioAsync(string nome, string cpf, string email, string telefone) {

        {
            // Verificar se ja existe numero cadastrado
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