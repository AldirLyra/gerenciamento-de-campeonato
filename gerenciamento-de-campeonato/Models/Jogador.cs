using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gerenciamento_de_campeonato.Models
{
    public enum Posicao
    {
        Goleiro,
        Zagueiro,
        Lateral,
        Volante,
        Meia,
        Atacante
    }

    public enum PePreferido
    {
        Direito,
        Esquerdo,
        Ambidestro
    }

    public class Jogador
	{
		public int ID { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Nacionalidade { get; set; }
        public virtual Posicao Posicao { get; set; }
        public int NumeroCamisa { get; set; }
        public string Peso { get; set; }
        public string Altura { get; set; }
        public virtual PePreferido PePreferido { get; set; }
        public virtual ICollection<Time> Time { get; set; }
    }
}