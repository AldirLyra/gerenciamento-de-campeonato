using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
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
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        public Cargo Cargo { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        public DateTime DataNascimento { get; set; }

        [Display(Name = "Time")]
        public int TimeId { get; set; }

        public virtual Time Time { get; set; }
    }
}