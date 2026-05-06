namespace Orcamento_Endpoint.Entities
{
    public class Orcamento
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int VeiculoId { get; set; }
        public string Status { get; set; } = "Aberto";
        public decimal ValorTotal { get; set; }
        public DateTime DataCriacao { get; set; }
        public List<OrcamentoItem> Itens { get; set; } = new();
    }
}
