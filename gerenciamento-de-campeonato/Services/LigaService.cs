using gerenciamento_de_campeonato.Controllers;
using gerenciamento_de_campeonato.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gerenciamento_de_campeonato.Services
{
    public class LigaService
    {
        private readonly ApplicationDbContext _context;
        private readonly Random _random;

        public LigaService(ApplicationDbContext context)
        {
            _context = context;
            _random = new Random();
        }

        public void InicializarCampeonato(Liga liga)
        {
            var (isValida, mensagem) = IsLigaValidaDetalhada(liga);
            if (!isValida)
                throw new InvalidOperationException(mensagem);

            GerarRodadas(liga);
            SimularPartidas(liga.Id);
        }

        private void GerarRodadas(Liga liga)
        {
            var times = _context.Times.Where(t => t.Ativo && t.LigaId == liga.Id).ToList();
            var rounds = new List<Partida>();
            for (int i = 0; i < times.Count; i++)
            {
                for (int j = i + 1; j < times.Count; j++)
                {
                    rounds.Add(new Partida
                    {
                        TimeCasaId = times[i].Id,
                        TimeVisitanteId = times[j].Id,
                        Rodada = (i + j) % 38 + 1,
                        DataPartida = DateTime.Now.AddDays((i + j) * 7),
                        LigaId = liga.Id,
                        GolsTimeCasa = -1,
                        GolsTimeVisitante = -1
                    });
                    rounds.Add(new Partida
                    {
                        TimeCasaId = times[j].Id,
                        TimeVisitanteId = times[i].Id,
                        Rodada = (i + j + 19) % 38 + 1,
                        DataPartida = DateTime.Now.AddDays((i + j + 19) * 7),
                        LigaId = liga.Id,
                        GolsTimeCasa = -1,
                        GolsTimeVisitante = -1
                    });
                }
            }

            _context.Partidas.AddRange(rounds);

            foreach (var time in times)
            {
                if (!_context.Tabelas.Any(t => t.TimeId == time.Id && t.LigaId == liga.Id))
                {
                    _context.Tabelas.Add(new Tabela
                    {
                        TimeId = time.Id,
                        LigaId = liga.Id,
                        Pontos = 0,
                        Jogos = 0,
                        Vitorias = 0,
                        Empates = 0,
                        Derrotas = 0,
                        GolsPro = 0,
                        GolsContra = 0
                    });
                }
            }

            _context.SaveChanges();
        }

        private void SimularPartidas(int ligaId)
        {
            var partidas = _context.Partidas.Where(p => p.LigaId == ligaId && p.GolsTimeCasa == -1).ToList();
            foreach (var partida in partidas)
            {
                partida.GolsTimeCasa = _random.Next(0, 4); // Máximo 3 gols
                partida.GolsTimeVisitante = _random.Next(0, 4);

                var jogadoresCasa = _context.Jogadores.Where(j => j.TimeId == partida.TimeCasaId).ToList();
                var jogadoresVisitante = _context.Jogadores.Where(j => j.TimeId == partida.TimeVisitanteId).ToList();

                int autoGolsCasa = 0;
                int autoGolsVisitante = 0;

                for (int i = 0; i < partida.GolsTimeCasa; i++)
                {
                    if (jogadoresCasa.Any() && jogadoresVisitante.Any())
                    {
                        bool isAutoGol = _random.NextDouble() < 0.01;
                        if (isAutoGol)
                        {
                            var jogador = jogadoresCasa[_random.Next(jogadoresCasa.Count)];
                            autoGolsCasa++;
                            _context.Gol.Add(new Gol
                            {
                                PartidaId = partida.Id,
                                JogadorId = jogador.Id,
                                Minuto = _random.Next(1, 91)
                            });
                        }
                        else
                        {
                            var jogador = jogadoresVisitante[_random.Next(jogadoresVisitante.Count)];
                            _context.Gol.Add(new Gol
                            {
                                PartidaId = partida.Id,
                                JogadorId = jogador.Id,
                                Minuto = _random.Next(1, 91)
                            });
                        }
                    }
                }

                for (int i = 0; i < partida.GolsTimeVisitante; i++)
                {
                    if (jogadoresCasa.Any() && jogadoresVisitante.Any())
                    {
                        bool isAutoGol = _random.NextDouble() < 0.01;

                        if (isAutoGol)
                        {
                            var jogador = jogadoresVisitante[_random.Next(jogadoresVisitante.Count)];
                            autoGolsVisitante++;

                            _context.Gol.Add(new Gol
                            {
                                PartidaId = partida.Id,
                                JogadorId = jogador.Id,
                                Minuto = _random.Next(1, 91)
                            });
                        }
                        else
                        {
                            var jogador = jogadoresCasa[_random.Next(jogadoresCasa.Count)];

                            _context.Gol.Add(new Gol
                            {
                                PartidaId = partida.Id,
                                JogadorId = jogador.Id,
                                Minuto = _random.Next(1, 91)
                            });
                        }
                    }
                }

                UpdateTable(partida, autoGolsCasa, autoGolsVisitante);
            }

            _context.SaveChanges();
        }
        private void UpdateTable(Partida partida, int autoGolsCasa, int autoGolsVisitante)
        {
            var tabelaCasa = _context.Tabelas.FirstOrDefault(t => t.TimeId == partida.TimeCasaId && t.LigaId == partida.LigaId);
            var tabelaVisitante = _context.Tabelas.FirstOrDefault(t => t.TimeId == partida.TimeVisitanteId && t.LigaId == partida.LigaId);

            if (tabelaCasa == null || tabelaVisitante == null)
                throw new InvalidOperationException("Tabela não encontrada para um dos times.");

            tabelaCasa.Jogos++;
            tabelaVisitante.Jogos++;

            tabelaCasa.GolsPro += partida.GolsTimeCasa;
            tabelaVisitante.GolsPro += partida.GolsTimeVisitante;

            tabelaCasa.GolsContra += autoGolsCasa;
            tabelaVisitante.GolsContra += autoGolsVisitante;

            if (partida.GolsTimeCasa > partida.GolsTimeVisitante)
            {
                tabelaCasa.Pontos += 3;
                tabelaCasa.Vitorias++;
                tabelaVisitante.Derrotas++;
            }
            else if (partida.GolsTimeCasa < partida.GolsTimeVisitante)
            {
                tabelaVisitante.Pontos += 3;
                tabelaVisitante.Vitorias++;
                tabelaCasa.Derrotas++;
            }
            else
            {
                tabelaCasa.Pontos += 1;
                tabelaVisitante.Pontos += 1;
                tabelaCasa.Empates++;
                tabelaVisitante.Empates++;
            }

            _context.SaveChanges();
        }

        public List<Tabela> GetClassificacao(int ligaId)
        {
            var classificacao = _context.Tabelas
                .Where(t => t.LigaId == ligaId)
                .OrderByDescending(t => t.Pontos)
                .ThenByDescending(t => t.GolsPro - t.GolsContra)
                .ThenByDescending(t => t.GolsPro)
                .ToList();

            System.Diagnostics.Debug.WriteLine($"LeagueService.GetClassification: LigaId={ligaId}, Classificacao.Count={classificacao?.Count ?? 0}");

            return classificacao ?? new List<Tabela>();
        }
        public List<ArtilheiroViewModel> GetArtilheiros(int ligaId)
        {
            var artilheiros = _context.Gol
                .Where(g => g.Partida.LigaId == ligaId)
                .GroupBy(g => g.Jogador)
                .Select(g => new ArtilheiroViewModel
                {
                    JogadorId = g.Key.Id,
                    NomeJogador = g.Key.Nome,
                    Time = g.Key.Time.Nome,
                    Gols = g.Count()
                })
                .OrderByDescending(a => a.Gols)
                .ToList();

            System.Diagnostics.Debug.WriteLine($"LeagueService.GetArtilheiros: LigaId={ligaId}, Artilheiros.Count={artilheiros?.Count ?? 0}");

            return artilheiros ?? new List<ArtilheiroViewModel>();
        }

        public (bool IsValido, string Mensagem) IsTimeValidoDetalhado(Time time)
        {
            var jogadores = _context.Jogadores.Where(j => j.TimeId == time.Id).ToList();

            var jogadoresCount = jogadores.Count;

            var posicoesObrigatorias = new[] { Posicao.GOLEIRO, Posicao.ZAGUEIRO, Posicao.LATERAL, Posicao.VOLANTE, Posicao.MEIA, Posicao.ATACANTE };

            var posicoesFaltando = posicoesObrigatorias.Where(pos => !jogadores.Any(j => j.Posicao == pos)).ToList();

            var comissaoCount = _context.ComissaoTecnica
                .Where(ct => ct.TimeId == time.Id)
                .Select(ct => ct.Cargo)
                .Distinct()
                .Count();

            if (!time.Ativo)
                return (false, $"O time {time.Nome} não está ativo.");

            if (jogadoresCount < 30)
                return (false, $"O time {time.Nome} possui apenas {jogadoresCount} jogadores. Mínimo necessário: 30.");

            if (posicoesFaltando.Any())
                return (false, $"O time {time.Nome} não possui jogadores nas posições: {string.Join(", ", posicoesFaltando)}.");

            if (comissaoCount < 5)
                return (false, $"O time {time.Nome} possui apenas {comissaoCount} membros na comissão técnica. Mínimo necessário: 5 cargos distintos.");

            return (true, string.Empty);
        }

        public (bool IsValida, string Mensagem) IsLigaValidaDetalhada(Liga liga)
        {
            var times = _context.Times.Where(t => t.LigaId == liga.Id && t.Ativo).ToList();

            if (times.Count != 20)
                return (false, $"A liga possui {times.Count} times ativos. Exatamente 20 times são necessários.");

            foreach (var time in times)
            {
                var (isValido, mensagem) = IsTimeValidoDetalhado(time);

                if (!isValido)
                    return (false, mensagem);
            }

            return (true, string.Empty);
        }
    }
}