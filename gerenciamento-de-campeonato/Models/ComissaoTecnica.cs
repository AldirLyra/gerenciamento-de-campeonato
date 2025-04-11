using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gerenciamento_de_campeonato.Models
{
	public enum Cargo
	{
		TREINADOR, 
		AUXILIAR, 
		PREPARADOR_FISICO, 
		FISIOLOGISTA, 
		TREINADOR_DE_GOLEIROS, 
		FISIOTERAPEUTA
    }

	public class ComissaoTecnica
	{
		public int ID { get; set; }
        public string Nome { get; set; }
		public virtual Cargo Cargo { get; set; }
        public DateTime DataNascimento { get; set; }
        public virtual ICollection<Time> Time { get; set; }

    }
}