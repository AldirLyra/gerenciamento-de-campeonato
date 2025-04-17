namespace gerenciamento_de_campeonato.Migrations
{
    using gerenciamento_de_campeonato.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<gerenciamento_de_campeonato.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var random = new Random();

            var coresUniformes = new[]
            {
                new { Primaria = "Vermelho", Secundaria = "Branco" },
                new { Primaria = "Azul", Secundaria = "Amarelo" },
                new { Primaria = "Verde", Secundaria = "Branco" },
                new { Primaria = "Preto", Secundaria = "Vermelho" },
                new { Primaria = "Branco", Secundaria = "Azul" },
                new { Primaria = "Amarelo", Secundaria = "Verde" },
                new { Primaria = "Laranja", Secundaria = "Preto" },
                new { Primaria = "Roxo", Secundaria = "Branco" },
                new { Primaria = "Cinza", Secundaria = "Vermelho" },
                new { Primaria = "Azul Escuro", Secundaria = "Dourado" },
                new { Primaria = "Vermelho Escuro", Secundaria = "Preto" },
                new { Primaria = "Verde Escuro", Secundaria = "Amarelo" },
                new { Primaria = "Branco", Secundaria = "Vermelho" },
                new { Primaria = "Amarelo", Secundaria = "Azul" },
                new { Primaria = "Preto", Secundaria = "Branco" },
                new { Primaria = "Azul Claro", Secundaria = "Preto" },
                new { Primaria = "Verde Claro", Secundaria = "Dourado" },
                new { Primaria = "Roxo Escuro", Secundaria = "Amarelo" },
                new { Primaria = "Dourado", Secundaria = "Vermelho" },
                new { Primaria = "Prata", Secundaria = "Azul" }
            };

            var estados = new[]
            {
                "SP", "RJ", "MG", "RS", "PR", "BA", "PE", "SC", "CE", "GO",
                "PA", "ES", "RN", "AL", "MT", "DF", "PB", "SE", "MA", "AM"
            };

            var liga = new Liga { Nome = "Liga Tabajara 2025", Ativa = true };
            context.Liga.AddOrUpdate(l => l.Nome, liga);
            context.SaveChanges();

            for (int i = 1; i <= 20; i++)
            {
                var cores = coresUniformes[i - 1];
                var estado = estados[(i - 1) % estados.Length];
                var time = new Time
                {
                    Nome = $"Time {i}",
                    Cidade = $"Cidade {i}",
                    Estado = estado,
                    AnoFundacao = 1900 + i,
                    Estadio = $"Estádio {i}",
                    CapacidadeEstadio = 30000 + i * 1000,
                    CorPrimaria = cores.Primaria,
                    CorSecundaria = cores.Secundaria,
                    Ativo = true,
                    LigaId = liga.Id
                };
                context.Times.AddOrUpdate(t => t.Nome, time);
                context.SaveChanges();

                var posicoes = new[] { Posicao.GOLEIRO, Posicao.ZAGUEIRO, Posicao.LATERAL, Posicao.VOLANTE, Posicao.MEIA, Posicao.ATACANTE };
                int jogadorCount = 1;

                foreach (var posicao in posicoes)
                {
                    for (int j = 1; j <= 5; j++)
                    {
                        double altura;

                        if (posicao == Posicao.GOLEIRO)
                        {
                            altura = 1.80 + (random.NextDouble() * (2.00 - 1.80));
                        }
                        else
                        {
                            altura = 1.65 + (random.NextDouble() * (2.00 - 1.65));
                        }

                        altura = Math.Round(altura, 2);

                        double pesoMin = altura * altura * 18.5;
                        double pesoMax = altura * altura * 25.0;
                        double peso = pesoMin + (random.NextDouble() * (pesoMax - pesoMin));
                        peso = Math.Round(peso, 1);

                        PePreferido pePreferido;
                        double peChance = random.NextDouble();

                        if (peChance < 0.70)
                            pePreferido = PePreferido.DIREITO;
                        else if (peChance < 0.95)
                            pePreferido = PePreferido.ESQUERDO;
                        else
                            pePreferido = PePreferido.AMBIDESTRO;

                        int anoNascimento = random.Next(1990, 2008);
                        int mesNascimento = random.Next(1, 13);
                        int diaMax = DateTime.DaysInMonth(anoNascimento, mesNascimento);
                        int diaNascimento = random.Next(1, diaMax + 1);
                        var dataNascimento = new DateTime(anoNascimento, mesNascimento, diaNascimento);

                        context.Jogadores.AddOrUpdate(jogador => new { jogador.Nome, jogador.TimeId },
                            new Jogador
                            {
                                Nome = $"Jogador {jogadorCount} do Time {i}",
                                DataNascimento = dataNascimento,
                                Nacionalidade = "Brasil",
                                Posicao = posicao,
                                NumeroCamisa = jogadorCount,
                                Altura = altura,
                                Peso = peso,
                                PePreferido = pePreferido,
                                TimeId = time.Id
                            });
                        jogadorCount++;
                    }
                }
                context.SaveChanges();

                context.ComissaoTecnica.AddOrUpdate(ct => new { ct.Nome, ct.TimeId },
                    new ComissaoTecnica { Nome = $"Treinador do Time {i}", Cargo = Cargo.TREINADOR, DataNascimento = new DateTime(1970, 1, 1), TimeId = time.Id },
                    new ComissaoTecnica { Nome = $"Auxiliar do Time {i}", Cargo = Cargo.AUXILIAR, DataNascimento = new DateTime(1975, 1, 1), TimeId = time.Id },
                    new ComissaoTecnica { Nome = $"Preparador do Time {i}", Cargo = Cargo.PREPARADOR_FISICO, DataNascimento = new DateTime(1980, 1, 1), TimeId = time.Id },
                    new ComissaoTecnica { Nome = $"Fisiologista do Time {i}", Cargo = Cargo.FISIOLOGISTA, DataNascimento = new DateTime(1985, 1, 1), TimeId = time.Id },
                    new ComissaoTecnica { Nome = $"Fisioterapeuta do Time {i}", Cargo = Cargo.FISIOTERAPEUTA, DataNascimento = new DateTime(1990, 1, 1), TimeId = time.Id }
                );
                context.SaveChanges();
            }
        }
    }
}
