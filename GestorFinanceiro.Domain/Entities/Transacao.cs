using System;
using GestorFinanceiro.Domain.Enums;

namespace GestorFinanceiro.Domain.Entities;

public class Transacao {

    public Guid Id { get; private set; }
    public Guid UsuarioId { get; set; }
    public decimal Valor { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public TipoTransacao Tipo { get; set; }
    public string Categoria { get; set; } = string.Empty;


    // O construtor vazio é necessário para o Entity Framework, mas ele não deve ser usado diretamente. Ele é a porta dos fundos do castelo, só para o mordomo (EF) entrar e fazer seu trabalho de magia.
    public Transacao()
    {
        Id = Guid.NewGuid();
        DataCriacao = DateTime.Now;
    }

    // O seu construtor principal com as regras de negócio
    public Transacao(Guid usuarioId, decimal valor, string descricao, DateTime data, TipoTransacao tipo) {
        if (valor <= 0) throw new Exception("O valor deve ser maior que zero.");

        Id = Guid.NewGuid();
        UsuarioId = usuarioId;
        Valor = valor;
        DataCriacao = DateTime.Now;
        Tipo = tipo;
        
       

        // A BLINDAGEM MÁGICA: Se o texto vier vazio, ele salva "Não especificada"
        Descricao = string.IsNullOrWhiteSpace(descricao) ? "Não especificada" : descricao.Trim();
    }
}
