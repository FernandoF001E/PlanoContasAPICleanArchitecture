using PlanoContasApi.Application.DTO;
using PlanoContasApi.Application.UserResult;

namespace PlanoContasApi.Application.Interfaces
{
	public interface IContaService
	{
		Task<List<ContaDto>> ObterTodosAsync();
		Task<SugerirCodigoResult> ExecutarAsync(string codigo);
		Task<ContaDto?> ObterPorIdAsync(int id);
		Task AdicionarAsync(ContaDto contaDto);
		Task AtualizarAsync(ContaDto contaDto);
		Task RemoverAsync(int id);
	}
}
