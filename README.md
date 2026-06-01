
# 💰 Gestor Financeiro WPP

Um **assistente financeiro inteligente via WhatsApp** que permite registrar gastos e receitas através de mensagens simples, com relatórios automáticos de suas finanças.

## ✨ Funcionalidades

- 📱 **Interface via WhatsApp** - Comunique-se naturalmente com o bot
- 💸 **Registre Transações** - Diga `"gastei 50 ifood"` ou `"ganhei 100 em Trade"`
- 📊 **Relatórios Automáticos** - Envie `"relatorio"` para ver resumo de gastos e ganhos
- 🔐 **Login com Google** - Autenticação segura via Google OAuth
- 🗂️ **Categorização Automática** - As categorias são extraídas automaticamente das mensagens
- 📈 **Histórico Completo** - Todas as transações são armazenadas no banco de dados

---

##  Arquitetura

O projeto segue a arquitetura **DDD (Domain-Driven Design)** com camadas bem definidas:

Gestor Financeiro WPP/
├── GestorFinanceiro.Domain/          # Camada de Domínio (Entidades)
├── GestorFinanceiro.Application/     # Camada de Aplicação (Serviços)
├── GestorFinanceiro.Infrastructure/  # Camada de Infraestrutura (Banco, Repos)
└── Gestor Financeiro WPP/            # API ASP.NET Core

---

## 🚀 Tecnologias Utilizadas

- **Framework**: ASP.NET Core 9.0
- **Banco de Dados**: SQL Server (Azure SQL Database)
- **ORM**: Entity Framework Core 9.0
- **Autenticação**: Google OAuth 2.0
- **Bot**: Twilio WhatsApp API
- **Cloud**: Microsoft Azure
- **Linguagem**: C# 13.0

---

## 📋 Pré-requisitos

- .NET 9.0 SDK
- SQL Server (local ou Azure)
- Conta no Twilio (para integração com WhatsApp)
- Conta Google Cloud (para OAuth)
- Conta Microsoft Azure (para hosting)

---

## 🔧 Instalação e Configuração

### 1. Clone o repositório

git clone https://github.com/damonscosta/Gestor-Financeiro-WPP.git
cd "Gestor Financeiro WPP"

### 2. Configure o banco de dados

Edite `appsettings.json` com sua string de conexão:

json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=seu-servidor;Database=GestorFinanceiro;User Id=sa;Password=sua-senha;"
  }
}

### 3. Aplique as migrations

# Via Package Manager Console
Add-Migration Initial
Update-Database

# Ou via CLI
dotnet ef migrations add Initial
dotnet ef database update

### 4. Configure variáveis de ambiente

Crie um arquivo `user-secrets.json` ou defina as variáveis:

{
  "Google:ClientId": "seu-google-client-id",
  "Twilio:AccountSid": "seu-twilio-sid",
  "Twilio:AuthToken": "seu-twilio-token"
}

### 5. Execute o projeto

dotnet run

A API estará disponível em: `https://localhost:5001`

---

## 💬 Como Usar

### 1. **Fazer Login**

Acesse a aplicação e faça login com sua conta Google, vinculando seu número de WhatsApp.

### 2. **Registrar uma Despesa**

Envie uma mensagem no WhatsApp:
gastei 50 ifood
ganhei 200 freelance
gastei 15,50 combustível

Resposta esperada:
✅ Registado com sucesso!
Tipo: Gasto
Valor: R$ 50
Categoria: Ifood

### 3. **Ver Relatório**

Envie:
relatorio


Resposta:
*SEU RELATÓRIO FINANCEIRO*

*GASTOS TOTAIS: R$ 235,50*
➖ Ifood: R$ 50
➖ Combustível: R$ 15,50

*GANHOS TOTAIS: R$ 200*
➕ Freelance: R$ 200

*SALDO FINAL: R$ -35,50*

---

## 📂 Estrutura de Diretórios

GestorFinanceiro.Domain/
├── Entities/           # Entidades (Transacao, Usuario)
├── Enums/             # Enumerações (TipoTransacao)
└── Interfaces/        # Contratos de repositórios

GestorFinanceiro.Application/
├── Services/          # Lógica de negócio (TransacaoAppService)
└── DTOs/             # Objetos de transferência

GestorFinanceiro.Infrastructure/
├── Data/             # Contexto do banco
├── Repositories/     # Implementações dos repositórios
└── Migrations/       # Migrations do Entity Framework

Gestor Financeiro WPP/
├── Controllers/      # Endpoints da API
├── wwwroot/         # Arquivos estáticos
└── Program.cs       # Configuração da aplicação

---

## 🔌 Endpoints da API

### POST `/api/auth/login`
Realizar login com Google e vincular WhatsApp

**Request:**
{
  "googleToken": "token_do_google",
  "telefoneWhatsapp": "+5511999999999"
}

### POST `/api/whatsapp`
Recebe mensagens do Twilio (webhook)

**Request:**
From=whatsapp:+5511999999999
Body=gastei 50 ifood

---

## 🗄️ Modelo de Dados

### Usuario
public class Usuario {
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public string Email { get; set; }
    public string TelefoneWhatsapp { get; set; }
}

### Transacao
public class Transacao {
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public decimal Valor { get; set; }
    public string Categoria { get; set; }
    public DateTime DataCriacao { get; set; }
    public TipoTransacao Tipo { get; set; }
}

public enum TipoTransacao {
    Receita = 0,
    Despesa = 1
}

---

## 🚀 Deploy no Azure

### 1. Publicar via Visual Studio

Clique com botão direito no projeto > Publish > Azure

### 2. Configurar banco de dados

Use Azure SQL Database para maior confiabilidade.

### 3. Configurar variáveis de ambiente

No Azure App Service:
- `Google:ClientId`
- `Twilio:AccountSid`
- `Twilio:AuthToken`

---

## 🐛 Troubleshooting

### ❌ Erro: "Usuário não encontrado"
- Certifique-se de fazer login via Google primeiro
- Verifique se o número do WhatsApp está correto

### ❌ Erro: "Não consegui identificar o valor"
- Use o formato: `gastei 50` (com número)
- Exemplos válidos: `gastei 50`, `ganhei 100,50`, `gastei 15.99`

### ❌ Erro de conexão com o banco
- Verifique a string de conexão em `appsettings.json`
- Confirme se o servidor SQL está rodando
- Verifique credenciais de acesso

---

## 📝 Melhorias Futuras

- [ ] Autenticação com WhatsApp nativa
- [ ] Dashboard web para visualizar transações
- [ ] Orçamentos e alertas de limite
- [ ] Gráficos de despesas por categoria
- [ ] Exportar relatórios em PDF
- [ ] Integração com banco (API Open Banking)
- [ ] Suporte a múltiplas moedas

---

## 🤝 Contribuindo

Contribuições são bem-vindas! Por favor:

1. Faça um **Fork** do projeto
2. Crie uma **branch** para sua feature (`git checkout -b feature/AmazingFeature`)
3. **Commit** suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. **Push** para a branch (`git push origin feature/AmazingFeature`)
5. Abra um **Pull Request**

---

## 📄 Licença

Este projeto está licenciado sob a Licença MIT - veja o arquivo [LICENSE](LICENSE) para detalhes.

---

## 👨‍💻 Autor

**Damon da Costa**

- GitHub: [@damonscosta](https://github.com/damonscosta)
- LinkedIn: [Damon da Costa](https://linkedin.com/in/damonscosta)

---


**Desenvolvido com ❤️ em C# .NET**

