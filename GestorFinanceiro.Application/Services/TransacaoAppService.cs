using GestorFinanceiro.Domain.Entities;
using GestorFinanceiro.Domain.Enums;
using GestorFinanceiro.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace GestorFinanceiro.Application.Services;

public class TransacaoAppService {
    private readonly ITransacaoRepository _repository;

    // Injeção de Dependência: O serviço pede o repositório, mas não sabe qual banco é.
    public TransacaoAppService(ITransacaoRepository repository) {
        _repository = repository;
    }

    public async Task<string> ProcessarMensagemAsync(string telefone, string mensagemTexto) {
        //Verificar se o usuário dono deste telefone existe no banco
        var usuario = await _repository.ObterUsuarioPorTelefoneAsync(telefone);
        if (usuario == null) {
            return "Usuário não encontrado. Por favor, cadastre seu número no portal.";
        }

        //Interpretar a mensagem 
        var mensagem = mensagemTexto.ToLower();
        TipoTransacao tipo;

        if (mensagemTexto.Trim().Equals("saldo", StringComparison.OrdinalIgnoreCase)) {
            var saldoFinal = await _repository.CalcularSaldoAsync(usuario.Id);
            return $"💰 O seu saldo atual é de R$ {saldoFinal:N2}";
        }

        // palavras-chave
        if (mensagem.Contains("ganhei") || mensagem.Contains("recebi") || mensagem.Contains("lucro")) {
            tipo = TipoTransacao.Receita;
        }
        else if (mensagem.Contains("gastei") || mensagem.Contains("perdi") || mensagem.Contains("paguei")) {
            tipo = TipoTransacao.Despesa;
        }
        else {
            return "🤖 Não entendi. Tente começar a frase com 'ganhei', 'recebi', 'gastei' ou 'perdi'.";


        }

        //Extrair o Valor usando Regex (Procura qualquer sequência de números, com ou sem vírgula/ponto)
        var matchValor = Regex.Match(mensagem, @"\d+([.,]\d+)?");
        if (!matchValor.Success) {
            return "🤖 Não consegui identificar o valor. Tente algo como 'gastei 50 com almoço'.";
        }

        // Converte o texto encontrado para decimal (garantindo que entenda ponto ou vírgula)
        decimal valor = decimal.Parse(matchValor.Value.Replace(".", ","));

        //A descrição será a própria mensagem original para não perdermos o contexto
        string descricao = mensagemTexto;

        // Criar a entidade 
        var novaTransacao = new Transacao(usuario.Id, valor, descricao, DateTime.Now, tipo);

        // Salvar no banco
        await _repository.AdicionarAsync(novaTransacao);

        // 7. Retornar a resposta que o Twilio enviará de volta pro WhatsApp
        string tipoTexto = tipo == TipoTransacao.Receita ? "Receita 📈" : "Despesa 📉";
        return $"✅ {tipoTexto} de R$ {valor} registrada com sucesso!";
    }
}