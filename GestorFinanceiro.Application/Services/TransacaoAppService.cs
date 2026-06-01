using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GestorFinanceiro.Domain.Entities;
using GestorFinanceiro.Domain.Interfaces;
using GestorFinanceiro.Domain.Enums;

namespace GestorFinanceiro.Application.Services {
    public class TransacaoAppService : ITransacaoAppService {
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        // Construtor com Injeção de Dependências
        public TransacaoAppService(ITransacaoRepository transacaoRepository, IUsuarioRepository usuarioRepository) {
            _transacaoRepository = transacaoRepository;
            _usuarioRepository = usuarioRepository;
        }

        // 1. MÉTODO QUE LÊ A MENSAGEM, EXTRAI O VALOR E INVENTA A CATEGORIA
        public async Task<string> ProcessarMensagemAsync(string telefone, string mensagemRecebida) {
            var usuario = await _usuarioRepository.ObterPorTelefoneAsync(telefone);
            if (usuario == null) return "Usuário não encontrado. Por favor, cadastre seu número no portal.";

            string mensagemWpp = mensagemRecebida.ToLower().Trim();

            // Verifica se ele quer o relatório
            if (mensagemWpp == "relatorio" || mensagemWpp == "relatório") {
                return await GerarRelatorioAsync(usuario.Id);
            }

            TipoTransacao? tipoTransacao = null;
            string tipoDescricao = "";
            if (mensagemWpp.StartsWith("gastei, gasto, despesa")) {
                tipoTransacao = TipoTransacao.Despesa;
                tipoDescricao = "Gasto";
            }
            else if (mensagemWpp.StartsWith("ganhei, ganho, lucro")) {
                tipoTransacao = TipoTransacao.Receita;
                tipoDescricao = "Ganho";
            }

            if (tipoTransacao.HasValue) {
                // Regex "pesca" o número
                Match matchValor = Regex.Match(mensagemWpp, @"\d+(,\d+)?");
                if (!matchValor.Success) return "Não consegui identificar o valor. Tente algo como 'gastei 50 ifood'.";

                decimal valor = Convert.ToDecimal(matchValor.Value);

                // A Categoria será TUDO o que sobrar na frase!
                string categoria = mensagemWpp
                    .Replace("ganhei, ganho, lucro", "")
                    .Replace("ganhei, ganho, lucro", "")
                    .Replace(matchValor.Value, "")
                    .Replace("$", "")
                    .Replace(" em ", " ")
                    .Trim();

                if (string.IsNullOrWhiteSpace(categoria)) {
                    categoria = "Geral";
                }
                else {
                    categoria = char.ToUpper(categoria[0]) + categoria.Substring(1);
                }

                // Cria e guarda a transação no banco
                var novaTransacao = new Transacao {
                    UsuarioId = usuario.Id,
                    Valor = valor,
                    Tipo = tipoTransacao.Value,
                    Categoria = categoria,
                    DataCriacao = DateTime.Now
                };

                await _transacaoRepository.AdicionarAsync(novaTransacao);

                return $"✅ Registado com sucesso!\nTipo: {tipoDescricao}\nValor: R$ {valor}\nCategoria: {categoria}";
            }

            return "Comando não reconhecido. Diga 'gastei [valor] [categoria]', 'ganhei [valor] [categoria]' ou 'relatorio'.";
        }

        // 2. MÉTODO QUE FAZ A MATEMÁTICA E AGRUPA TUDO (O RELATÓRIO)
        private async Task<string> GerarRelatorioAsync(Guid usuarioId) {
            var transacoes = await _transacaoRepository.ObterPorUsuarioIdAsync(usuarioId);

            if (!transacoes.Any()) return "Você ainda não possui nenhuma transação registada.";

            var gastos = transacoes.Cast<Transacao>().Where(static t => t.Tipo == TipoTransacao.Despesa).ToList();
            var ganhos = transacoes.Cast<Transacao>().Where(t => t.Tipo == TipoTransacao.Receita).ToList();

            var totalGasto = gastos.Sum(t => t.Valor);
            var totalGanho = ganhos.Sum(t => t.Valor);

            string resposta = " *SEU RELATÓRIO FINANCEIRO* \n\n";

            resposta += $" *GASTOS TOTAIS: R$ {totalGasto}*\n";
            var gastosAgrupados = gastos.GroupBy(t => t.Categoria);
            foreach (var grupo in gastosAgrupados) {
                resposta += $" ➖ {grupo.Key}: R$ {grupo.Sum(t => t.Valor)}\n";
            }

            resposta += $"\n *GANHOS TOTAIS: R$ {totalGanho}*\n";
            var ganhosAgrupados = ganhos.GroupBy(t => t.Categoria);
            foreach (var grupo in ganhosAgrupados) {
                resposta += $" ➕ {grupo.Key}: R$ {grupo.Sum(t => t.Valor)}\n";
            }

            resposta += $"\n *SALDO FINAL: R$ {totalGanho - totalGasto}*";

            return resposta;
        }
    }
}