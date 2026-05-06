using Orcamento_Endpoint.Entities;
using Orcamento_Endpoint.Entities.DTOs;
using Orcamento_Endpoint.Interface;
using Orcamento_Endpoint.Repository.RepositoryInterfaces;

namespace Orcamento_Endpoint.Services
{
    public class OrcamentoService : IOrcamentoService
    {
        private readonly IOrcamentoRepository _repository;
        public OrcamentoService(IOrcamentoRepository repository)
        {
            _repository = repository;
        }
        public async Task<(bool Sucesso, string Mensage, int id)> CriarOrcamentoAsync(CreateOrcamentoDto dto)
        {
            if (dto.Itens == null || !dto.Itens.Any())
            {
                return (false, "O orçamento deve conter pelo menos um item.", 0);
            }
            var orcamento = new Orcamento
            {
                ClienteId = dto.ClienteId,
                VeiculoId = dto.VeiculoId,
                Status = "Aberto",
                DataCriacao = DateTime.UtcNow
            };
            decimal total = 0;

            foreach (var itemDto in dto.Itens)
            {
                var valorTotal = itemDto.Quantidade * itemDto.ValorUnitario;

                orcamento.Itens.Add(new OrcamentoItem
                {
                    Descricao = itemDto.Descricao,
                    Quantidade = itemDto.Quantidade,
                    ValorUnitario = itemDto.ValorUnitario,
                    ValorTotal = valorTotal
                });
                total += valorTotal;
            }
            orcamento.ValorTotal = total;
            await _repository.AddAsync(orcamento);

            return (true, "Orçamento criado com sucesso.", orcamento.Id);
        }

        public async Task<Orcamento?> ObterOrcamentoPorIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
