using Microsoft.AspNetCore.Mvc;
using PlanoContasApi.Application.DTO;
using PlanoContasApi.Application.Interfaces;
using PlanoContasApi.Domain.Entities;

namespace PlanoContasApi.Web.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PlanoContasController(IContaService contaService) : ControllerBase
	{
		private readonly IContaService _contaService = contaService;

		[HttpGet("sugerir-proximo-codigo/{codigoPai}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		public async Task<ActionResult> SugerirProximoCodigo(string codigoPai)
		{
			var resultado = await _contaService.ExecutarAsync(codigoPai);
			if (resultado == null)
				return BadRequest("Limite máximo de códigos atingido.");
			return Ok(resultado);
		}

		[HttpGet]
		[ProducesResponseType(typeof(Conta), 200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> Get()
		{
			var contas = await _contaService.ObterTodosAsync();
			return Ok(contas);
		}

		[HttpGet("{id}")]
		[ProducesResponseType(typeof(Conta), 200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> GetById(int id)
		{
			var conta = await _contaService.ObterPorIdAsync(id);
			if (conta == null)
				return NotFound();
			return Ok(conta);
		}

		[HttpPost]
		[ProducesResponseType(typeof(Conta), 200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> Post([FromBody] ContaDto conta)
		{
			await _contaService.AdicionarAsync(conta);
			return CreatedAtAction(nameof(GetById), new { id = conta.Id }, conta);
		}

		[HttpPut("{id}")]
		[ProducesResponseType(typeof(Conta), 200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> Put(int id, [FromBody] ContaDto conta)
		{
			if (id != conta.Id)
				return BadRequest("ID mismatch.");

			await _contaService.AtualizarAsync(conta);
			return NoContent();
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(Conta), 200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> Delete(int id)
		{
			await _contaService.RemoverAsync(id);
			return NoContent();
		}
	}
}
