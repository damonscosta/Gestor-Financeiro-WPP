using System.Threading.Tasks;

namespace GestorFinanceiro.Domain.Interfaces 
{
    public interface ITransacaoAppService {
        Task<string> ProcessarMensagemAsync(string telefone, string mensagemRecebida);
    }
}