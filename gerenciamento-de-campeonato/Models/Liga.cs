using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gerenciamento_de_campeonato.Models
{
	public class Liga
	{
		public int ID { get; set; }
        public string Nome { get; set; }
		public ICollection<Time> Times { get; set; } = new List<Time>();
    }
}