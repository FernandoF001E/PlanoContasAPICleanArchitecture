using Microsoft.EntityFrameworkCore;
using PlanoContasApi.Domain.Entities;
using PlanoContasApi.Domain.Interfaces;
using PlanoContasApi.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanoContasApi.Infrastructure.Repositories
{
	public class ContaRepository : IContaRepository
	{
		private readonly AppDbContext _context;

		public ContaRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<List<Conta>> ObterTodosAsync()
		{
			return await _context.Contas.ToListAsync();
		}

		public async Task<Conta?> ObterPorIdAsync(int id)
		{
			return await _context.Contas.FindAsync(id);
		}

		public async Task AdicionarAsync(Conta conta)
		{
			_context.Contas.Add(conta);
			await _context.SaveChangesAsync();
		}

		public async Task AtualizarAsync(Conta conta)
		{
			_context.Contas.Update(conta);
			await _context.SaveChangesAsync();
		}

		public async Task RemoverAsync(int id)
		{
			var conta = await _context.Contas.FindAsync(id);
			if (conta != null)
			{
				_context.Contas.Remove(conta);
				await _context.SaveChangesAsync();
			}
		}
	}
}
