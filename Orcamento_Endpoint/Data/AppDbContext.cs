using Microsoft.EntityFrameworkCore;
using Orcamento_Endpoint.Entities;

namespace Orcamento_Endpoint.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Orcamento> Orcamentos { get; set; }
        public DbSet<OrcamentoItem> OrcamentoItens { get; set; }
    }
}
