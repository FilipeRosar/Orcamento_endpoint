namespace Orcamento_Endpoint.Entities
{
    public class OrcamentoItem
    {
        public int Id { get; set; }
        public int OrcamentoId { get; set; }
        public required string Descricao { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal { get; set; }

        public Orcamento? Orcamento { get; set; }
    }
}
