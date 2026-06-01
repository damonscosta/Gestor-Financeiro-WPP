namespace GestorFinanceiro.Domain.Entities;

// Entidade de usuário para o sistema de gestão financeira
public class Usuario {
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Cpf { get; private set; }
    public string Email { get; private set; }
    public string TelefoneWhatsapp { get; private set; }

    // Inicializa propriedades não anuláveis com valores padrão para evitar CS8618
    protected Usuario() {
        Nome = string.Empty;
        Cpf = string.Empty;
        Email = string.Empty;
        TelefoneWhatsapp = string.Empty;
    }
    public Usuario(string nome, string cpf, string email, string telefone) {
        Id = Guid.NewGuid();
        Nome = nome;
        Cpf = cpf;
        Email = email;
        TelefoneWhatsapp = telefone;
    }

    // Método para permitir a troca de número mantendo o vínculo com CPF/Email
    public void AtualizarTelefone(string novoTelefone) {
        if (string.IsNullOrWhiteSpace(novoTelefone))
            throw new Exception("Telefone inválido.");

        TelefoneWhatsapp = novoTelefone;
    }
}