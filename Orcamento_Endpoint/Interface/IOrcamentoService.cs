using Orcamento_Endpoint.Entities;
using Orcamento_Endpoint.Entities.DTOs;

namespace Orcamento_Endpoint.Interface
{
    public interface IOrcamentoService
    {
        Task<(bool Sucesso, string Mensage, int id)> CriarOrcamentoAsync(CreateOrcamentoDto dto);
        Task<OrcamentoResponseDto?> ObterOrcamentoPorIdAsync(int id);
        Task<(bool Sucesso, string Mensage)> AtualizarStatusAsync(int id, string novoStatus);
    }
}
