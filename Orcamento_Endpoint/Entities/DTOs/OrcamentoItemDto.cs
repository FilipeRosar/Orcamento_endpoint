namespace Orcamento_Endpoint.Entities.DTOs
{
    public record OrcamentoItemDto(
        string Descricao,
        int Quantidade,
        decimal ValorUnitario
        );
    
}
