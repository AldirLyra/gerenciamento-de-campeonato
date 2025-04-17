using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace gerenciamento_de_campeonato.Models
{
	public class Liga
	{
        public int Id { get; set; }
        [Display(Name = "Nome da Liga")]
        public string Nome { get; set; }
        public bool Ativa { get; set; }
        public virtual ICollection<Time> Times { get; set; }
        public virtual ICollection<Partida> Partidas { get; set; }
        public virtual ICollection<Tabela> Tabela { get; set; }
    }
}