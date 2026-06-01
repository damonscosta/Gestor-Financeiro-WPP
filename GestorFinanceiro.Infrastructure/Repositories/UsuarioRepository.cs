using GestorFinanceiro.Domain.Entities;
using GestorFinanceiro.Domain.Interfaces;
using GestorFinanceiro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestorFinanceiro.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository {
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context) {
        _context = context;
    }

    public async Task AdicionarAsync(Usuario usuario) {
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task<Usuario?> ObterPorTelefoneAsync(string telefone) {
        // Busca no banco se o número do WhatsApp já existe
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.TelefoneWhatsapp == telefone);
    }
    // Implementação do método para verificar se um email já existe
    // === Aplicado no IUsuarioRepository e no UsuarioAppService ===
    public async Task<bool> ExisteEmailAsync(string email) {
        return await _context.Usuarios.AnyAsync(u => u.Email == email);
    }
}

