using PlanoContasApi.Application.DTO;
using PlanoContasApi.Application.Interfaces;
using PlanoContasApi.Application.UserResult;
using PlanoContasApi.Domain.Entities;
using PlanoContasApi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PlanoContasApi.Application.Service
{
	public class ContaService(IContaRepository contaRepository) : IContaService
	{
		private readonly IContaRepository _contaRepository = contaRepository;
		private const int LIMITE_MAXIMO = 999;

		public async Task<SugerirCodigoResult?> ExecutarAsync(string codigoPai)
		{
			var todasContas = await _contaRepository.ObterTodosAsync();

			var filhosDiretos = todasContas
				.Where(c => ObterCodigoPai(c.Codigo) == codigoPai)
				.ToList();

			var ultimosNumeros = filhosDiretos
				.Select(c =>
				{
					var partes = c.Codigo.Split('.');
					return int.TryParse(partes.Last(), out var n) ? n : -1;
				})
				.Where(n => n > 0)
				.ToList();

			var proximoNumero = (ultimosNumeros.Count > 0 ? ultimosNumeros.Max() + 1 : 1);

			if (proximoNumero > LIMITE_MAXIMO)
			{
				var novoPai = ObterCodigoPai(codigoPai);
				if (string.IsNullOrEmpty(novoPai))
					return null;

				var filhosNovoPai = todasContas
					.Where(c => ObterCodigoPai(c.Codigo) == novoPai)
					.ToList();

				var ultimosNovoPai = filhosNovoPai
					.Select(c =>
					{
						var partes = c.Codigo.Split('.');
						return int.TryParse(partes.Last(), out var n) ? n : -1;
					})
					.Where(n => n > 0)
					.ToList();

				var novoNumero = (ultimosNovoPai.Count > 0 ? ultimosNovoPai.Max() + 1 : 1);

				if (novoNumero > LIMITE_MAXIMO)
					return null;

				return new SugerirCodigoResult
				{
					NovoPai = novoPai,
					CodigoSugerido = $"{novoPai}.{novoNumero}"
				};
			}

			return new SugerirCodigoResult
			{
				CodigoSugerido = $"{codigoPai}.{proximoNumero}"
			};
		}

		public async Task<List<ContaDto>> ObterTodosAsync()
		{
			var contas = await _contaRepository.ObterTodosAsync();
			return [.. contas.Select(c => new ContaDto
			{
				Id = c.Id,
				Codigo = c.Codigo,
				Nome = c.Nome,
				Tipo = c.Tipo,
				AceitaLancamentos = c.AceitaLancamentos
			})];
		}

		public async Task<ContaDto?> ObterPorIdAsync(int id)
		{
			var c = await _contaRepository.ObterPorIdAsync(id);
			if (c == null) return null;

			return new ContaDto
			{
				Id = c.Id,
				Codigo = c.Codigo,
				Nome = c.Nome,
				Tipo = c.Tipo,
				AceitaLancamentos = c.AceitaLancamentos
			};
		}

		public async Task AdicionarAsync(ContaDto dto)
		{
			var todasContas = await _contaRepository.ObterTodosAsync();

			if (todasContas.Any(c => c.Codigo == dto.Codigo))
				throw new InvalidOperationException("Já existe uma conta com este código.");

			Conta? contaPai = null;
			if (!string.IsNullOrEmpty(dto.Codigo))
			{
				var pai = ObterCodigoPai(dto.Codigo);
				if (pai != null)
				{
					contaPai = todasContas.FirstOrDefault(c => c.Codigo == pai);
					if (contaPai == null)
						throw new InvalidOperationException("Conta pai não encontrada.");

					if (contaPai.Tipo != dto.Tipo)
						throw new InvalidOperationException("A conta e seu pai devem ser do mesmo tipo.");

					if (contaPai.AceitaLancamentos)
						throw new InvalidOperationException("Não é possível adicionar uma conta filha a uma conta que aceita lançamentos.");
				}
			}

			var filhosDessaConta = todasContas.Where(c => c.Codigo == dto.Codigo).ToList();
			if (dto.AceitaLancamentos && filhosDessaConta.Count != 0)
				throw new InvalidOperationException("Contas que aceitam lançamentos não podem possuir contas filhas.");

			var conta = new Conta
			{
				Codigo = dto.Codigo,
				Nome = dto.Nome,
				Tipo = dto.Tipo,
				AceitaLancamentos = dto.AceitaLancamentos
			};

			await _contaRepository.AdicionarAsync(conta);
		}

		public async Task AtualizarAsync(ContaDto dto)
		{
			var contaExistente = await _contaRepository.ObterPorIdAsync(dto.Id) ?? throw new InvalidOperationException("Conta não encontrada.");

			if (contaExistente.Codigo != dto.Codigo)
				throw new InvalidOperationException("Já existe uma conta com este código.");

			if (contaExistente.Tipo != dto.Tipo)
				throw new InvalidOperationException("A conta e seu pai devem ser do mesmo tipo.");

			contaExistente.Codigo = dto.Codigo;
			contaExistente.Nome = dto.Nome;
			contaExistente.Tipo = dto.Tipo;
			contaExistente.AceitaLancamentos = dto.AceitaLancamentos;

			await _contaRepository.AtualizarAsync(contaExistente);
		}

		public async Task RemoverAsync(int id)
		{
			await _contaRepository.RemoverAsync(id);
		}

		private static string? ObterCodigoPai(string codigo)
		{
			var partes = codigo.Split('.');
			if (partes.Length <= 1) return null;
			return string.Join('.', partes.Take(partes.Length - 1));
		}
	}
}
