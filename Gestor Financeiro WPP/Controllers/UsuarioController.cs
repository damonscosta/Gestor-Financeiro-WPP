using GestorFinanceiro.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestorFinanceiro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase {
    private readonly UsuarioAppService _appService;
    public UsuarioController(UsuarioAppService appService) {
        _appService = appService;

    }

    [HttpPost("cadastrar")]
    public async Task<IActionResult> Cadastrar([FromBody] NovoUsuarioDTO request) 
        {
        var resultado = await _appService.CadastrarUsuarioAsync(
                    request.Nome, request.Cpf, request.Email, request.Telefone);

        return Ok(resultado);
    }
}

public class NovoUsuarioDTO {
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
}

