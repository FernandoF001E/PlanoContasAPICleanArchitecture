using Microsoft.EntityFrameworkCore;
using PlanoContasApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanoContasApi.Infrastructure.Persistence
{
	/// <summary>
	/// AppContext
	/// </summary>
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
		/// <summary>
		/// Tabela Contas do banco de dados
		/// </summary>
		public DbSet<Conta> Contas { get; set; }
	}
}
