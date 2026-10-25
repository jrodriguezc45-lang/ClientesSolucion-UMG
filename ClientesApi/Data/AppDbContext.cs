using ClientesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientesApi.Data;

/// <summary>
/// Contexto de Entity Framework Core: es el "puente" entre C# y Azure SQL.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Cliente> Clientes => Set<Cliente>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // El CUI no se puede repetir (igual que la restricción UNIQUE del script SQL)
        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.CUI)
            .IsUnique();
    }
}
