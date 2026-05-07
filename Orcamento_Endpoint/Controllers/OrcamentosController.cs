using Microsoft.AspNetCore.Mvc;
using Orcamento_Endpoint.Entities.DTOs;
using Orcamento_Endpoint.Interface;


namespace Orcamento_Endpoint.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrcamentosController : ControllerBase
    {
        private readonly IOrcamentoService _service;

        public OrcamentosController(IOrcamentoService service)
        {
            _service = service;
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrcamento([FromBody] CreateOrcamentoDto dto)
        {
            var result = await _service.CriarOrcamentoAsync(dto);

            if (!result.Sucesso)
            {
                return BadRequest(result.Mensage);
            }
            return CreatedAtAction(nameof(FindById), new { id = result.id }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FindById(int id)
        {
            var orcamento = await _service.ObterOrcamentoPorIdAsync(id);
            if (orcamento == null)
            {
                return NotFound();
            }
            return Ok(orcamento);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusDto dto)
        {
            var result = await _service.AtualizarStatusAsync(id, dto.NovoStatus);
            
            if (!result.Sucesso)
            {
                return BadRequest(result.Mensage);
            }
            return Ok(new { mensagem = result.Mensage });
        }
    }
}
