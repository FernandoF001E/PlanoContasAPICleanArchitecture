using PlanoContasApi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanoContasApi.Application.UserResult
{
	public class SugerirCodigoResult
	{
		public string? NovoPai { get; set; }
		public string CodigoSugerido { get; set; } = string.Empty;
	}
}
