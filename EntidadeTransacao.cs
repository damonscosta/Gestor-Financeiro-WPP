namespace SeuProjeto.Domain.Entities;

public class Transacao {

    public Guid Id { get; private set; }
    public Guid UsuarioId { get; private set; }
    public decimal Valor { get; private set; }
    public string Descricao { get; private set; }
    public DateTime Data { get; private set; }
    public TipoTransacao Tipo { get; private set; }

    public Transacao(Guid usuarioId, decimal valor, string descricao, DateTime data, TipoTransacao tipo)
    {
        Id = Guid.NewGuid();
        UsuarioId = usuarioId;
        Valor = valor;
        Descricao = descricao;
        Data = data;
        Tipo = tipo;
    }
}