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
    public class LigaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Liga
        public ActionResult Index()
        {
            return View(db.Liga.ToList());
        }

        // GET: Liga/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Liga liga = db.Liga.Find(id);
            if (liga == null)
            {
                return HttpNotFound();
            }
            return View(liga);
        }

        // GET: Liga/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Liga/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,Ativa")] Liga liga)
        {
            if (ModelState.IsValid)
            {
                db.Liga.Add(liga);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(liga);
        }

        // GET: Liga/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Liga liga = db.Liga.Find(id);
            if (liga == null)
            {
                return HttpNotFound();
            }
            return View(liga);
        }

        // POST: Liga/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Ativa")] Liga liga)
        {
            if (ModelState.IsValid)
            {
                db.Entry(liga).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(liga);
        }

        // GET: Liga/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Liga liga = db.Liga.Find(id);
            if (liga == null)
            {
                return HttpNotFound();
            }
            return View(liga);
        }

        // POST: Liga/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Liga liga = db.Liga.Find(id);
            db.Liga.Remove(liga);
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
