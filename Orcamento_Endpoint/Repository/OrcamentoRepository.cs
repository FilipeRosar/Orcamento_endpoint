using Microsoft.EntityFrameworkCore;
using Orcamento_Endpoint.Data;
using Orcamento_Endpoint.Entities;
using Orcamento_Endpoint.Repository.RepositoryInterfaces;

namespace Orcamento_Endpoint.Repository
{
    public class OrcamentoRepository : IOrcamentoRepository
    {
        private readonly AppDbContext _context;

        public OrcamentoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Orcamento orcamento)
        {
            _context.Orcamentos.Add(orcamento);
            await _context.SaveChangesAsync();
        }

        public async Task<Orcamento?> GetByIdAsync(int id)
        {
            return await _context.Orcamentos
                .Include(o => o.Itens)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task UpdateAsync(Orcamento orcamento)
        {
            _context.Orcamentos.Update(orcamento);
            await _context.SaveChangesAsync();
        }
    }
}
