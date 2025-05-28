using PlanoContasApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanoContasApi.Domain.Interfaces
{
	public interface IContaRepository
	{
		Task<List<Conta>> ObterTodosAsync();
		Task<Conta?> ObterPorIdAsync(int id);
		Task AdicionarAsync(Conta conta);
		Task AtualizarAsync(Conta conta);
		Task RemoverAsync(int id);
	}
}
