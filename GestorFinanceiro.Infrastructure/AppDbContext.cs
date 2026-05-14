using GestorFinanceiro.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestorFinanceiro.Infrastructure.Data;

public class AppDbContext : DbContext {
    // O construtor recebe as configurações (como a senha do banco) que virão da API depois
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Estas propriedades virarão as tabelas reais no SQL Server
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Transacao> Transacoes { get; set; }

    // Aqui podemos configurar detalhes das colunas (ex: tamanho máximo de texto)
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Usuario>().HasKey(u => u.Id);
        modelBuilder.Entity<Transacao>().HasKey(t => t.Id);
    }
}