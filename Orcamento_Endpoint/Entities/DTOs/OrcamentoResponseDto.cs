namespace Orcamento_Endpoint.Entities.DTOs
{
    public class OrcamentoResponseDto
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int VeiculoId { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; }
        public DateTime DataCriacao { get; set; }

        public List<OrcamentoItemResponseDto> Itens { get; set; } = new();
    }
}
