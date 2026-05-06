using Orcamento_Endpoint.Entities;

namespace Orcamento_Endpoint.Repository.RepositoryInterfaces
{
    public interface IOrcamentoRepository
    {
        Task AddAsync(Orcamento orcamento);
        Task<Orcamento?> GetByIdAsync(int id);
    }
}
