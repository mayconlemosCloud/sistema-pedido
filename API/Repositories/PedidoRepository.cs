using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;

namespace API.Repositories;

/// <summary>
/// Implementação do repositório de Pedido
/// </summary>
public class PedidoRepository : IPedidoRepository
{
    private readonly AppDbContext _context;

    public PedidoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Pedido>> GetAllAsync()
    {
        return await _context.Pedidos
            .Include(p => p.Itens)
            .ToListAsync();
    }

    public async Task<Pedido?> GetByIdAsync(Guid id)
    {
        return await _context.Pedidos
            .Include(p => p.Itens)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Pedido>> GetByClienteIdAsync(Guid clienteId)
    {
        return await _context.Pedidos
            .Include(p => p.Itens)
            .Where(p => p.ClienteId == clienteId)
            .ToListAsync();
    }

    public async Task AddAsync(Pedido pedido)
    {
        await _context.Pedidos.AddAsync(pedido);
    }

    public async Task UpdateAsync(Pedido pedido)
    {
        _context.Pedidos.Update(pedido);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        var pedido = await _context.Pedidos.FindAsync(id);
        if (pedido != null)
        {
            _context.Pedidos.Remove(pedido);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
