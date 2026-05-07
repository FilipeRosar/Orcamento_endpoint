using Xunit;
using Moq;
using FluentAssertions;
using Orcamento_Endpoint.Services;
using Orcamento_Endpoint.Repository.RepositoryInterfaces;
using Orcamento_Endpoint.Interface;
using Orcamento_Endpoint.Entities.DTOs;
using Orcamento_Endpoint.Entities;

namespace Orcamento_Endpoint.Tests;

public class OrcamentoServiceTests
{
    private readonly Mock<IOrcamentoRepository> _repositoryMock;
    private readonly OrcamentoService _service;

    public OrcamentoServiceTests()
    {
        _repositoryMock = new Mock<IOrcamentoRepository>();
        _service = new OrcamentoService(_repositoryMock.Object);
    }
    [Fact]
    public async Task Deve_Criar_Orcamento_Com_Sucesso()
    {
        var dto = new CreateOrcamentoDto(
            1,
            1,
            new List<OrcamentoItemDto>()
            {
                new("Troca de óleo", 1, 100.0m)
            }
        );

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Orcamento>()))
            .Callback<Orcamento>(o => o.Id = 1)
            .Returns(Task.CompletedTask);

        var result = await _service.CriarOrcamentoAsync(dto);

        result.Sucesso.Should().BeTrue();
        result.id.Should().BeGreaterThan(0);

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Orcamento>()), Times.Once);
    }

    [Fact]
    public async Task Deve_Falhar_Quando_Nao_Tem_Itens()
    {
        var dto = new CreateOrcamentoDto(
            1,
            1,
            new List<OrcamentoItemDto>()
        );

        var result = await _service.CriarOrcamentoAsync(dto);

        result.Sucesso.Should().BeFalse();
        result.Mensage.Should().Contain("pelo menos um item");
        result.id.Should().Be(0);

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Orcamento>()), Times.Never);
    }

    [Fact]
    public async Task Deve_Obter_Orcamento_Por_Id()
    {
        var orcamento = new Orcamento
        {
            Id = 1,
            ClienteId = 1,
            VeiculoId = 1,
            Status = "Aberto",
            ValorTotal = 100.0m,
            DataCriacao = DateTime.UtcNow,
            Itens = new List<OrcamentoItem>
            {
                new()
                {
                    Id = 1,
                    OrcamentoId = 1,
                    Descricao = "Troca de óleo",
                    Quantidade = 1,
                    ValorUnitario = 100.0m,
                    ValorTotal = 100.0m
                }
            }
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(orcamento);

        var result = await _service.ObterOrcamentoPorIdAsync(1);

        result.Should().NotBeNull();
        result?.Id.Should().Be(1);
        result?.ValorTotal.Should().Be(100.0m);
        result?.Itens.Should().HaveCount(1);
    }

    [Fact]
    public async Task Deve_Retornar_Null_Quando_Orcamento_Nao_Existe()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Orcamento?)null);

        var result = await _service.ObterOrcamentoPorIdAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Deve_Atualizar_Status_Com_Sucesso()
    {
        var orcamento = new Orcamento { Id = 1, Status = "Aberto" };

        _repositoryMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(orcamento);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Orcamento>()))
            .Returns(Task.CompletedTask);

        var result = await _service.AtualizarStatusAsync(1, "Aceito");

        result.Sucesso.Should().BeTrue();
        result.Mensage.Should().Contain("Aceito");
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Orcamento>()), Times.Once);
    }

    [Fact]
    public async Task Deve_Falhar_Com_Status_Invalido()
    {
        var result = await _service.AtualizarStatusAsync(1, "StatusInvalido");

        result.Sucesso.Should().BeFalse();
        result.Mensage.Should().Contain("Status inválido");
    }

    [Fact]
    public async Task Deve_Falhar_Ao_Atualizar_Orcamento_Inexistente()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Orcamento?)null);

        var result = await _service.AtualizarStatusAsync(999, "Aceito");

        result.Sucesso.Should().BeFalse();
        result.Mensage.Should().Contain("não encontrado");
    }
}
