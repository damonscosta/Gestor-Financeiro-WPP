using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth;
using GestorFinanceiro.Application.Services;

namespace Gestor_Financeiro_WPP;

[ApiController]
[Route("api/[controller]")]
public class AuthController(UsuarioAppService usuarioService) : ControllerBase
{
    private readonly UsuarioAppService _usuarioService = usuarioService;

    // O endpoint de login do Google. Ele recebe o token do Google e o número de WhatsApp, e faz toda a mágica de validação e cadastro.
    [HttpPost("login")]
    public async Task<IActionResult> LoginGoogle([FromBody] LoginRequest request)
    {
        try
        {
            //O Segurança do C# verificando o passe VIP do Google
            var configuracoes = new GoogleJsonWebSignature.ValidationSettings()
            {
                // O Google emite tokens para várias aplicações. Aqui, a gente diz que só aceita o nosso app específico.
                Audience = ["647379081050-rkudk52lcars6b2najlan3h25n4ss9o4.apps.googleusercontent.com"]
            };

            // Se o token for inválido, a linha de baixo vai lançar uma InvalidJwtException e a gente já sabe que é golpe.
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.GoogleToken, configuracoes);

            // Se chegou aqui, o token é legítimo e o Google confirmou a identidade do usuário. Podemos confiar nas informações do payload.
            Console.WriteLine($"\n Google confirmou a identidade!");
            Console.WriteLine($" Email: {payload.Email}");
            Console.WriteLine($" Nome: {payload.Name}");
            Console.WriteLine($" WhatsApp: {request.TelefoneWhatsapp}\n");

            //O Maestro de Usuários do nosso sistema, recebendo as informações do Google e do WhatsApp, e fazendo a mágica de cadastrar ou atualizar o usuário.
            var resultado = await _usuarioService.CadastrarUsuarioAsync(
                payload.Name, "00000000000", payload.Email, request.TelefoneWhatsapp);

            return Ok(new { Mensagem = "Login validado com sucesso! WhatsApp vinculado.", Detalhe = resultado });
        }
        catch (InvalidJwtException)
        {
            // Se o token for inválido, a gente cai aqui. Pode ser um golpe, alguém tentando se passar por um usuário legítimo.
            return Unauthorized(new { Mensagem = "Token do Google inválido. Tentando me enganar?" });
        }
    }
}

public class LoginRequest
{
    public string GoogleToken { get; set; } = string.Empty;
    public string TelefoneWhatsapp { get; set; } = string.Empty;
}