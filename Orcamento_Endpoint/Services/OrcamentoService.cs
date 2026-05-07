using Microsoft.AspNetCore.Http.HttpResults;
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
            
            foreach (var item in dto.Itens)
            {
                if (item.Quantidade <= 0)
                {
                    return (false, "A quantidade deve ser maior que zero", 0);
                }
                if (item.ValorUnitario <= 0)
                {
                    return (false, "O valor unitário deve ser maior que zero", 0);
                }

            }
            var itens = dto.Itens
                .Select(itemDto =>
            {
                var valorTotal = itemDto.Quantidade * itemDto.ValorUnitario;

                return new OrcamentoItem
                {
                    Descricao = itemDto.Descricao,
                    Quantidade = itemDto.Quantidade,
                    ValorUnitario = itemDto.ValorUnitario,
                    ValorTotal = valorTotal
                };
            }).ToList();
            var valorTotalOrcamento = itens.Sum(i => i.ValorTotal);

            var orcamento = new Orcamento
            {
                ClienteId = dto.ClienteId,
                VeiculoId = dto.VeiculoId,
                Status = "Aberto",
                DataCriacao = DateTime.UtcNow,
                ValorTotal = valorTotalOrcamento,
                Itens = itens
            };
            await _repository.AddAsync(orcamento);
            return (true, "Orçamento criado com sucesso", orcamento.Id);
        }

        public async Task<OrcamentoResponseDto?> ObterOrcamentoPorIdAsync(int id)
        {
            var orcamento = await _repository.GetByIdAsync(id);

            if (orcamento == null)
            {
                return null;
            }
            return new OrcamentoResponseDto
            {
                Id = orcamento.Id,
                ClienteId = orcamento.ClienteId,
                VeiculoId = orcamento.VeiculoId,
                Status = orcamento.Status,
                ValorTotal = orcamento.ValorTotal,
                DataCriacao = orcamento.DataCriacao,


                Itens = orcamento.Itens.Select(i => new OrcamentoItemResponseDto
                {
                    Id = i.Id,
                    Descricao = i.Descricao,
                    Quantidade = i.Quantidade,
                    ValorUnitario = i.ValorUnitario,
                    ValorTotal = i.ValorTotal
                }).ToList()
            };
        }

        public async Task<(bool Sucesso, string Mensage)> AtualizarStatusAsync(int id, string novoStatus)
        {
            var statusesValidos = new[] { "Aberto", "Aceito", "Rejeitado", "Cancelado" };
            
            if (!statusesValidos.Contains(novoStatus))
            {
                return (false, "Status inválido. Use: Aberto, Aceito, Rejeitado ou Cancelado");
            }

            var orcamento = await _repository.GetByIdAsync(id);
            if (orcamento == null)
            {
                return (false, "Orçamento não encontrado.");
            }

            orcamento.Status = novoStatus;
            await _repository.UpdateAsync(orcamento);

            return (true, $"Status atualizado para {novoStatus} com sucesso.");
        }
    }
}
