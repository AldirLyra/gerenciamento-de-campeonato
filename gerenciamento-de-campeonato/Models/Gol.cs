using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gerenciamento_de_campeonato.Models
{
	public class Gol
	{
        public int Id { get; set; }

        [Display(Name = "Partida")]
        public int PartidaId { get; set; }

        [Display(Name = "Jogador")]
        public int JogadorId { get; set; }

        public int Minuto { get; set; }
        public virtual Partida Partida { get; set; }
        public virtual Jogador Jogador { get; set; }
    }
}