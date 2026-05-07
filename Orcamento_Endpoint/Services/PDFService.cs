using Orcamento_Endpoint.Entities;
using Orcamento_Endpoint.Entities.DTOs;
using Orcamento_Endpoint.Interface;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;


namespace Orcamento_Endpoint.Services
{
    public class PDFService : IPdfService
    {
        public byte[] GerarOrcamentoPdf(OrcamentoResponseDto orcamento)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header()
                    .Text("ORÇAMENTO MECÂNICO")
                    .FontSize(20)
                    .Bold();

                    page.Content().Column(column =>
                    {
                        column.Spacing(30);

                        column.Item().Text($"Orçamento ID: {orcamento.Id}");
                        column.Item().Text($"Cliente ID: {orcamento.ClienteId}");
                        column.Item().Text($"Veículo ID: {orcamento.VeiculoId}");
                        column.Item().Text($"Status: {orcamento.Status}");
                        column.Item().Text($"Data de Criação: {orcamento.DataCriacao:dd/MM/yyyy}");

                        column.Item().PaddingTop(15).Text("Itens do orçamento:").FontSize(16).Bold();

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(4);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                            });
                            table.Header(header =>
                            {
                                header.Cell().Text("Descrição").FontSize(14).Bold();
                                header.Cell().Text("Quantidade").FontSize(14).Bold();
                                header.Cell().Text("Preço Unitário").FontSize(14).Bold();
                                header.Cell().Text("Valor Total").FontSize(14).Bold();
                            });
                            foreach (var item in orcamento.Itens)
                            {
                                table.Cell().Text(item.Descricao);
                                table.Cell().Text(item.Quantidade.ToString());
                                table.Cell().Text(item.ValorUnitario.ToString("C"));
                                table.Cell().Text(item.ValorTotal.ToString("C"));
                            }
                        });
                        column.Item()
                        .PaddingTop(20)
                        .Text($"Valor Total: R$ {orcamento.ValorTotal:F2}")
                        .Bold()
                        .FontSize(16);
                    });
                });
            }).GeneratePdf();
        }
    }
}
