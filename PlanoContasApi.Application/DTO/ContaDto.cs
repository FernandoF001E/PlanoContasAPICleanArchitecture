using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanoContasApi.Application.DTO
{
	public class ContaDto
	{
		public int Id { get; set; }
		public string? Codigo { get; set; }
		public string? Nome { get; set; }
		public string? Tipo { get; set; }
		public bool AceitaLancamentos { get; set; }
	}
}
