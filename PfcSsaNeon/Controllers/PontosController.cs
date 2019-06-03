using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using PfcSsaNeon.Models;

namespace PfcSsaNeon.Controllers
{
    public class PontosController : Controller
    {
        private PfcSsaNeonContext db = new PfcSsaNeonContext();
        private ApplicationDbContext context = new ApplicationDbContext();

        // GET: Pontos
        public ActionResult Index()
        {
            var pontoes = db.Pontoes.Include(p => p.Funcionarios);
            return View(pontoes.ToList());
        }

        // GET: Pontos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ponto ponto = db.Pontoes.Find(id);
            if (ponto == null)
            {
                return HttpNotFound();
            }
            return View(ponto);
        }

        // GET: Pontos/Create
        public ActionResult Create()
        {
            ViewBag.FuncionarioId = new SelectList(db.Funcionarios, "FuncionarioId", "Nome");
            return View();
        }

        // POST: Pontos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PontoId,DataPonto,EntradaManha,SaidaManha,EntradaTarde,SaidaTarde,EntradaExtra,SaidaExtra,FuncionarioId,Abonar")] Ponto ponto)
        {
            if (ModelState.IsValid)
            {
                db.Pontoes.Add(ponto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FuncionarioId = new SelectList(db.Funcionarios, "FuncionarioId", "Nome", ponto.FuncionarioId);
            return View(ponto);
        }

        // GET: Pontos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ponto ponto = db.Pontoes.Find(id);
            if (ponto == null)
            {
                return HttpNotFound();
            }
            ViewBag.FuncionarioId = new SelectList(db.Funcionarios, "FuncionarioId", "Nome", ponto.FuncionarioId);
            return View(ponto);
        }

        // POST: Pontos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PontoId,DataPonto,EntradaManha,SaidaManha,EntradaTarde,SaidaTarde,EntradaExtra,SaidaExtra,FuncionarioId,Abonar")] Ponto ponto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ponto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FuncionarioId = new SelectList(db.Funcionarios, "FuncionarioId", "Nome", ponto.FuncionarioId);
            return View(ponto);
        }

        // GET: Pontos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ponto ponto = db.Pontoes.Find(id);
            if (ponto == null)
            {
                return HttpNotFound();
            }
            return View(ponto);
        }

        // POST: Pontos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ponto ponto = db.Pontoes.Find(id);
            db.Pontoes.Remove(ponto);
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
