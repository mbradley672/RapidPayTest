using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class RapidPayDbContext : DbContext
{
    public RapidPayDbContext(DbContextOptions options) : base(options)
    {
        
    }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Card> Cards { get; set; }
}