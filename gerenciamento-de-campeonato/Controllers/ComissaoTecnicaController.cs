using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using gerenciamento_de_campeonato.Models;

namespace gerenciamento_de_campeonato.Controllers
{
    public class ComissaoTecnicaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ComissaoTecnica
        public ActionResult Index(string searchString, string cargo)
        {
            var cargoList = new[] { "Treinador", "Fisiologista", "Preparador Físico", "Treinador de Goleiros", "Auxiliar", "Fisioterapeuta" }
                            .OrderBy(c => c).ToList();

            ViewBag.cargo = new SelectList(cargoList, cargo ?? "All");

            var comissao = from c in db.ComissaoTecnica
                           select c;

            if (!string.IsNullOrEmpty(searchString))
            {
                comissao = comissao.Where(c => c.Nome.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(cargo) && cargo != "All")
            {
                if(cargo.Equals("Preparador Físico"))
                {
                    cargo = "PREPARADOR_FISICO";
                }

                if (cargo.Equals("Treinador de Goleiros"))
                {
                    cargo = "TREINADOR_DE_GOLEIROS";
                }

                comissao = comissao.Where(c => c.Cargo.ToString().Equals(cargo));
            }

            return View(comissao.ToList());
        }

        // GET: ComissaoTecnica/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComissaoTecnica comissaoTecnica = db.ComissaoTecnica.Find(id);
            if (comissaoTecnica == null)
            {
                return HttpNotFound();
            }
            return View(comissaoTecnica);
        }

        // GET: ComissaoTecnica/Create
        public ActionResult Create()
        {
            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome");
            return View();
        }

        // POST: ComissaoTecnica/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,Cargo,DataNascimento,TimeId")] ComissaoTecnica comissaoTecnica)
        {
            if (ModelState.IsValid)
            {
                db.ComissaoTecnica.Add(comissaoTecnica);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome", comissaoTecnica.TimeId);
            return View(comissaoTecnica);
        }

        // GET: ComissaoTecnica/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComissaoTecnica comissaoTecnica = db.ComissaoTecnica.Find(id);
            if (comissaoTecnica == null)
            {
                return HttpNotFound();
            }
            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome", comissaoTecnica.TimeId);
            return View(comissaoTecnica);
        }

        // POST: ComissaoTecnica/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Cargo,DataNascimento,TimeId")] ComissaoTecnica comissaoTecnica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comissaoTecnica).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome", comissaoTecnica.TimeId);
            return View(comissaoTecnica);
        }

        // GET: ComissaoTecnica/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComissaoTecnica comissaoTecnica = db.ComissaoTecnica.Find(id);
            if (comissaoTecnica == null)
            {
                return HttpNotFound();
            }
            return View(comissaoTecnica);
        }

        // POST: ComissaoTecnica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ComissaoTecnica comissaoTecnica = db.ComissaoTecnica.Find(id);
            db.ComissaoTecnica.Remove(comissaoTecnica);
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
