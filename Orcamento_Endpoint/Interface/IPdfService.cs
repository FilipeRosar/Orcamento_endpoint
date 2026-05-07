using Orcamento_Endpoint.Entities;
using Orcamento_Endpoint.Entities.DTOs;

namespace Orcamento_Endpoint.Interface
{
    public interface IPdfService
    {
        byte[] GerarOrcamentoPdf(OrcamentoResponseDto orcamento);
    }
}
