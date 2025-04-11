using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gerenciamento_de_campeonato.Models
{
	public enum CoresUniforme
	{
		COR_PRIMARIA, COR_SECUNDARIA
	}

	public class Time
	{
		public int ID { get; set; }
		public string Nome { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
		public int AnoFundacao { get; set; }
		public string Estadio { get; set; }
		public string CapacidadeEstadio { get; set; }
		public virtual CoresUniforme CoresUniforme { get; set; }
		public bool Status { get; set; }
    }
}