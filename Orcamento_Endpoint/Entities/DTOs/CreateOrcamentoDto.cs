namespace Orcamento_Endpoint.Entities.DTOs
{
    public record CreateOrcamentoDto
    (
        int ClienteId,
        int VeiculoId,
        List<OrcamentoItemDto> Itens
    );
}
