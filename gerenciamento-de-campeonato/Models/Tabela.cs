using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace gerenciamento_de_campeonato.Models
{
	public class Tabela
	{
        public int Id { get; set; }

        [Display(Name = "Time")]
        public int TimeId { get; set; }

        [Display(Name = "Liga")]
        public int LigaId { get; set; }
        public int Pontos { get; set; }
        public int Jogos { get; set; }
        public int Vitorias { get; set; }
        public int Empates { get; set; }
        public int Derrotas { get; set; }
        public int GolsPro { get; set; }
        public int GolsContra { get; set; }
        public int SaldoGols => GolsPro - GolsContra;
        public virtual Time Time { get; set; }
        public virtual Liga Liga { get; set; }
    }

}