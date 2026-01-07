using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;

namespace API.Repositories;

/// <summary>
/// Implementação do repositório de Produto
/// </summary>
public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Produto>> GetAllAsync()
    {
        return await _context.Produtos.ToListAsync();
    }

    public async Task<Produto?> GetByIdAsync(Guid id)
    {
        return await _context.Produtos.FindAsync(id);
    }

    public async Task AddAsync(Produto produto)
    {
        await _context.Produtos.AddAsync(produto);
    }

    public async Task UpdateAsync(Produto produto)
    {
        _context.Produtos.Update(produto);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto != null)
        {
            _context.Produtos.Remove(produto);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
