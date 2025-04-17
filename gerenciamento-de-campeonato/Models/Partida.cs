using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace gerenciamento_de_campeonato.Models
{
	public class Partida
	{
        public int Id { get; set; }
        public int TimeCasaId { get; set; }
        public int TimeVisitanteId { get; set; }
        public virtual Time TimeCasa { get; set; }
        public virtual Time TimeVisitante { get; set; }
        [DataType(DataType.Date)]
        public DateTime DataPartida { get; set; }
        public int GolsTimeCasa { get; set; }
        public int GolsTimeVisitante { get; set; }
        public int Rodada { get; set; }
        public int LigaId { get; set; }
        public virtual Liga Liga { get; set; }
        public virtual ICollection<Gol> Gols { get; set; }
    }
}