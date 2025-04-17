using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace gerenciamento_de_campeonato.Models
{
    public enum Posicao
    {
        GOLEIRO,
        ZAGUEIRO,
        LATERAL,
        VOLANTE,
        MEIA,
        ATACANTE
    }

    public enum PePreferido
    {
        DIREITO,
        ESQUERDO,
        AMBIDESTRO
    }

    public class Jogador
	{
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        public DateTime DataNascimento { get; set; }
        public string Nacionalidade { get; set; }
        public Posicao Posicao { get; set; }

        [Display(Name = "Número da Camisa")]
        public int NumeroCamisa { get; set; }
        public double Altura { get; set; }
        public double Peso { get; set; }

        [Display(Name = "Pé Preferido")]
        public PePreferido PePreferido { get; set; }

        [Display(Name = "Time")]
        public int TimeId { get; set; }
        public virtual Time Time { get; set; }
        public virtual ICollection<Gol> Gols { get; set; }
    }
}