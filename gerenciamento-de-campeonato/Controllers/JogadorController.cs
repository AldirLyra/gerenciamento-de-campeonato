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

namespace gerenciamento_de_campeonato.Controllers
{
    public class JogadorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Jogador
        public ActionResult Index(string searchString, string posicao, string pePreferido)
        {
            var posicaoList = Enum.GetNames(typeof(Posicao)).OrderBy(p => p).ToList();
            ViewBag.posicao = new SelectList(posicaoList, posicao ?? "All");

            var pePreferidoList = new[] { "Direito", "Esquerdo", "Ambidestro" }.OrderBy(p => p).ToList();
            ViewBag.pePreferido = new SelectList(pePreferidoList, pePreferido ?? "All");

            var jogadores = from j in db.Jogadores
                            select j;

            if (!string.IsNullOrEmpty(searchString))
            {
                jogadores = jogadores.Where(j => j.Nome.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(posicao) && posicao != "All")
            {
                if (Enum.TryParse(posicao, out Posicao posicaoEnum))
                {
                    jogadores = jogadores.Where(j => j.Posicao == posicaoEnum);
                }
            }
            if (!string.IsNullOrEmpty(pePreferido) && pePreferido != "All")
            {
                jogadores = jogadores.Where(j => j.PePreferido.ToString().Equals(pePreferido));
            }
            return View(jogadores.ToList());
        }

        // GET: Jogador/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jogador jogador = db.Jogadores.Find(id);
            if (jogador == null)
            {
                return HttpNotFound();
            }
            return View(jogador);
        }

        // GET: Jogador/Create
        public ActionResult Create()
        {
            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome");
            return View();
        }

        // POST: Jogador/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,DataNascimento,Nacionalidade,Posicao,NumeroCamisa,Altura,Peso,PePreferido,TimeId")] Jogador jogador)
        {
            if (ModelState.IsValid)
            {
                db.Jogadores.Add(jogador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome", jogador.TimeId);
            return View(jogador);
        }

        // GET: Jogador/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jogador jogador = db.Jogadores.Find(id);
            if (jogador == null)
            {
                return HttpNotFound();
            }
            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome", jogador.TimeId);
            return View(jogador);
        }

        // POST: Jogador/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,DataNascimento,Nacionalidade,Posicao,NumeroCamisa,Altura,Peso,PePreferido,TimeId")] Jogador jogador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jogador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome", jogador.TimeId);
            return View(jogador);
        }

        // GET: Jogador/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jogador jogador = db.Jogadores.Find(id);
            if (jogador == null)
            {
                return HttpNotFound();
            }
            return View(jogador);
        }

        // POST: Jogador/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Jogador jogador = db.Jogadores.Find(id);
            db.Jogadores.Remove(jogador);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
