using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using gerenciamento_de_campeonato.Models;
using gerenciamento_de_campeonato.Services;

namespace gerenciamento_de_campeonato.Controllers
{
    public class EstatisticaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly LigaService _ligaService;

        public EstatisticaController()
        {
            try
            {
                _context = new ApplicationDbContext();
                _ligaService = new LigaService(_context);
                Debug.WriteLine("EstatisticaController: Inicializado com sucesso.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"EstatisticaController: Erro ao inicializar, Mensagem={ex.Message}, StackTrace={ex.StackTrace}");
                throw;
            }
        }

        public ActionResult Index(string jogos, string estadio, int ligaId = 1)
        {
            try
            {
                Debug.WriteLine($"EstatisticaController.Index: Iniciando para ligaId={ligaId}");
                if (ligaId <= 0)
                {
                    ViewBag.ErrorMessage = "ID da liga inválido.";
                    Debug.WriteLine($"EstatisticaController.Index: ligaId={ligaId}, Erro: ID inválido");
                    return View(new EstatisticaViewModel
                    {
                        LigaId = ligaId,
                        Classificacao = new System.Collections.Generic.List<Tabela>(),
                        Artilheiros = new System.Collections.Generic.List<ArtilheiroViewModel>(),
                        Partidas = new System.Collections.Generic.List<Partida>()
                    });
                }

                var liga = _context.Liga.Find(ligaId);
                if (liga == null)
                {
                    ViewBag.ErrorMessage = "Liga não encontrada.";
                    Debug.WriteLine($"EstatisticaController.Index: ligaId={ligaId}, Erro: Liga não encontrada");
                    return View(new EstatisticaViewModel
                    {
                        LigaId = ligaId,
                        Classificacao = new List<Tabela>(),
                        Artilheiros = new List<ArtilheiroViewModel>(),
                        Partidas = new List<Partida>()
                    });
                }

                //filtros
                var estadioList = _context.Tabelas
                    .Where(t => t.LigaId == ligaId && t.Time != null)
                    .Select(t => t.Time.Estadio)
                    .Distinct()
                    .OrderBy(e => e)
                    .ToList();

                ViewBag.estadio = new SelectList(estadioList, estadio ?? "All");

                var classificacao = _ligaService.GetClassificacao(ligaId) ?? new List<Tabela>();

                if (!string.IsNullOrEmpty(jogos) && int.TryParse(jogos, out int jogosFiltro))
                {
                    classificacao = classificacao.Where(t => t.Jogos == jogosFiltro).ToList();
                }
                if (!string.IsNullOrEmpty(estadio) && estadio != "All")
                {
                    classificacao = classificacao.Where(t => t.Time != null && t.Time.Estadio == estadio).ToList();
                }

                var model = new EstatisticaViewModel
                {
                    LigaId = ligaId,
                    Classificacao = _ligaService.GetClassificacao(ligaId) ?? new List<Tabela>(),
                    Artilheiros = _ligaService.GetArtilheiros(ligaId) ?? new List<ArtilheiroViewModel>(),
                    Partidas = _context.Partidas.Where(p => p.LigaId == ligaId).OrderBy(p => p.Rodada).ToList() ?? new List<Partida>()
                };

                Debug.WriteLine($"EstatisticaController.Index: LigaId={ligaId}, Classificacao.Count={model.Classificacao?.Count ?? 0}, Artilheiros.Count={model.Artilheiros?.Count ?? 0}, Partidas.Count={model.Partidas?.Count ?? 0}");

                if (model.Classificacao == null)
                {
                    Debug.WriteLine($"EstatisticaController.Index: Classificacao é null para ligaId={ligaId}");
                    model.Classificacao = new System.Collections.Generic.List<Tabela>();
                }

                if (!model.Classificacao.Any())
                {
                    ViewBag.InfoMessage = "A simulação ainda não foi iniciada para esta liga. Clique em 'Iniciar Simulação' na página inicial.";
                }

                return View(model);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"EstatisticaController.Index: Erro inesperado, ligaId={ligaId}, Mensagem={ex.Message}, StackTrace={ex.StackTrace}");
                ViewBag.ErrorMessage = "Ocorreu um erro ao carregar as estatísticas. Tente novamente.";
                return View(new EstatisticaViewModel
                {
                    LigaId = ligaId,
                    Classificacao = new List<Tabela>(),
                    Artilheiros = new List<ArtilheiroViewModel>(),
                    Partidas = new List<Partida>()
                });
            }
        }

        [HttpPost]
        public ActionResult IniciarSimulacao(int ligaId)
        {
            try
            {
                Debug.WriteLine($"EstatisticaController.IniciarSimulacao: Iniciando para ligaId={ligaId}");
                var liga = _context.Liga.Find(ligaId);

                if (liga == null)
                {
                    Debug.WriteLine($"EstatisticaController.IniciarSimulacao: Liga não encontrada para ligaId={ligaId}");
                    return Json(new { success = false, message = "Liga não encontrada." });
                }

                var (isValida, mensagemValidacao) = _ligaService.IsLigaValidaDetalhada(liga);
                if (!isValida)
                {
                    Debug.WriteLine($"EstatisticaController.IniciarSimulacao: Validação falhou, Mensagem={mensagemValidacao}");
                    return Json(new { success = false, message = mensagemValidacao });
                }

                _ligaService.InicializarCampeonato(liga);
                Debug.WriteLine($"EstatisticaController.IniciarSimulacao: Simulação concluída para ligaId={ligaId}");
                return Json(new { success = true, redirectUrl = Url.Action("Index", new { ligaId }) });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"EstatisticaController.IniciarSimulacao: Erro, ligaId={ligaId}, Mensagem={ex.Message}, StackTrace={ex.StackTrace}");
                return Json(new { success = false, message = "Erro ao iniciar simulação: " + ex.Message });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class EstatisticaViewModel
    {
        public int LigaId { get; set; }
        public List<Tabela> Classificacao { get; set; }
        public List<ArtilheiroViewModel> Artilheiros { get; set; }
        public List<Partida> Partidas { get; set; }
    }

    public class ArtilheiroViewModel
    {
        public int JogadorId { get; set; }
        public string NomeJogador { get; set; }
        public string Time { get; set; }
        public int Gols { get; set; }
    }
}
