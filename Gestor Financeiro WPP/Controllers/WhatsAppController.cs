using GestorFinanceiro.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestorFinanceiro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WhatsAppController : ControllerBase {
    private readonly TransacaoAppService _appService;

    // Injeção do nosso Serviço de Aplicação (O Maestro)
    public WhatsAppController(TransacaoAppService appService) {
        _appService = appService;
    }
    // O endpoint que o Twilio vai chamar toda vez que receber uma mensagem no WhatsApp. Ele é um POST porque o Twilio envia os dados da mensagem no corpo da requisição.
    [HttpPost]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<IActionResult> ReceberMensagem([FromForm] string From, [FromForm] string Body) {
        //O Twilio nos manda o número do remetente (From) e o texto da mensagem (Body). O número vem no formato "whatsapp:+5511999999999", então a gente limpa isso para ficar só o número.
        var telefoneUsuario = From.Replace("whatsapp:", "");
        var textoMensagem = Body;

        //Passamos a bola para a camada de Application resolver tudo
        var respostaTexto = await _appService.ProcessarMensagemAsync(telefoneUsuario, textoMensagem);

        //O Twilio espera uma resposta em XML para enviar a mensagem de volta
        var xmlTwilio = $"<Response><Message>{respostaTexto}</Message></Response>";

        //Devolvemos a mensagem para o WhatsApp da pessoa
        return Content(xmlTwilio, "application/xml");
    }
}