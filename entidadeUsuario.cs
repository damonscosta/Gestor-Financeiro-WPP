namespace SeuProjeto.Domain.Entities;

// Entidade que representa um usuário do sistema
public class Usuario
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string CPF { get; private set; }
    public string Email { get; private set; }
    public string TelefoneWhatsapp { get; private set; }
} 

// Construtor para criar um novo usuário
public Usuario(string nome, string cpf, string email, string telefone)
{

    Id = Guid.NewGuid();
    Nome = nome;
    CPF = cpf;
    Email = email;
    TelefoneWhatsapp = telefone;
}

    // Metodo para atualizar as numero de telefone do usuário
    public void AtualizarInformacoes(string novoTelefone)
    {
       if (!string.IsNullOrEmpty(novoTelefone))
            throw new Exception("Telefone invalido.");
        
           TelefoneWhatsapp = novoTelefone;
        
    }