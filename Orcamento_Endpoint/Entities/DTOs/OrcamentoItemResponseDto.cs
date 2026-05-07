namespace Orcamento_Endpoint.Entities.DTOs
{
    public class OrcamentoItemResponseDto
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
