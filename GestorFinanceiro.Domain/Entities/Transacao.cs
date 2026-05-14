using GestorFinanceiro.Domain.Enums;

namespace GestorFinanceiro.Domain.Entities;

public class Transacao {

    public Guid Id { get; private set; }
    public Guid UsuarioId { get; private set; }
    public decimal Valor { get; private set; }
    public string Descricao { get; private set; }
    public DateTime Data { get; private set; }
    public TipoTransacao Tipo { get; private set; }


    protected Transacao() { }
    // O seu construtor principal com as regras de negócio
    public Transacao(Guid usuarioId, decimal valor, string descricao, DateTime data, TipoTransacao tipo) {
        if (valor <= 0) throw new Exception("O valor deve ser maior que zero.");

        Id = Guid.NewGuid();
        UsuarioId = usuarioId;
        Valor = valor;
        Data = data;
        Tipo = tipo;

        // A BLINDAGEM MÁGICA: Se o texto vier vazio, ele salva "Não especificada"
        Descricao = string.IsNullOrWhiteSpace(descricao) ? "Não especificada" : descricao.Trim();
    }
}
