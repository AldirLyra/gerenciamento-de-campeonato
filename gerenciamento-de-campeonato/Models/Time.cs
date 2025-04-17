using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace gerenciamento_de_campeonato.Models
{
	public class Time
	{
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nome do Time")]
        public string Nome { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        [Display(Name = "Ano de Fundação")]
        public int AnoFundacao { get; set; }

        public string Estadio { get; set; }

        [Display(Name = "Capacidade do Estádio")]
        public int CapacidadeEstadio { get; set; }

        [Display(Name = "Cor Primária")]
        public string CorPrimaria { get; set; }

        [Display(Name = "Cor Secundária")]
        public string CorSecundaria { get; set; }

        public bool Ativo { get; set; }

        [Display(Name = "Liga")]
        public int LigaId { get; set; }

        [Display(Name = "Liga")]
        public virtual Liga Liga { get; set; }
        public virtual ICollection<Jogador> Jogadores { get; set; }
        public virtual ICollection<ComissaoTecnica> ComissaoTecnicas { get; set; }
    }

}