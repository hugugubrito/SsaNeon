using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using PfcSsaNeon.Helper;
using PfcSsaNeon.Models;

namespace PfcSsaNeon.Controllers
{
    public class DescontosController : Controller
    {
        private PfcSsaNeonContext db = new PfcSsaNeonContext();
        private ApplicationDbContext context = new ApplicationDbContext();

        // GET: Descontos
        public ActionResult Index()
        {
            try
            {
                var x = User.Identity.Name;
                var u = context.Users.Where(z => z.Email.Equals(x)).FirstOrDefault();
                if (Verificar.VerificarAcesso(1, 3, u))
                {
                    var descontoes = db.Descontoes.Include(d => d.Salarios);
                    return View(descontoes.ToList());
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // GET: Descontos/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                var x = User.Identity.Name;
                var u = context.Users.Where(z => z.Email.Equals(x)).FirstOrDefault();
                if (Verificar.VerificarAcesso(1, 3, u))
                {

                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Desconto desconto = db.Descontoes.Find(id);
                    if (desconto == null)
                    {
                        return HttpNotFound();
                    }
                    return View(desconto);
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // GET: Descontos/Create
        public ActionResult Create(int? id, bool negativo)
        {
            try
            {
                var x = User.Identity.Name;
                var u = context.Users.Where(z => z.Email.Equals(x)).FirstOrDefault();
                if (Verificar.VerificarAcesso(1, 4, u))
                {

                    Salario sal = db.Salarios.Where(s => s.FuncionarioId == id).FirstOrDefault();
                    TempData["negativo"] = negativo;
                    ViewBag.SalarioId = sal.SalarioId;
                    return View();
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // POST: Descontos/Create
        [HttpPost]
        public ActionResult Create(Desconto desconto, int SalarioId)
        {
            desconto.SalarioId = SalarioId;
            if (ModelState.IsValid)
            {
                if ((bool)TempData["negativo"])
                {
                    desconto.Valor -= desconto.Valor * 2;
                }
                db.Descontoes.Add(desconto);
                db.SaveChanges();
                return RedirectToAction("GerarFolha", "Home");
            }
            ViewBag.SalarioId = new SelectList(db.Salarios, "SalarioId", "SalarioId", desconto.SalarioId);
            return View(desconto);

        }

        // GET: Descontos/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                var x = User.Identity.Name;
                var u = context.Users.Where(z => z.Email.Equals(x)).FirstOrDefault();
                if (Verificar.VerificarAcesso(1, 4, u))
                {

                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Desconto desconto = db.Descontoes.Find(id);
                    if (desconto == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.SalarioId = new SelectList(db.Salarios, "SalarioId", "SalarioId", desconto.SalarioId);
                    return View(desconto);
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // POST: Descontos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DescontoId,Percento,Valor,Descricao,Data,SalarioId")] Desconto desconto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(desconto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SalarioId = new SelectList(db.Salarios, "SalarioId", "SalarioId", desconto.SalarioId);
            return View(desconto);

        }

        // GET: Descontos/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                var x = User.Identity.Name;
                var u = context.Users.Where(z => z.Email.Equals(x)).FirstOrDefault();
                if (Verificar.VerificarAcesso(1, 4, u))
                {

                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Desconto desconto = db.Descontoes.Find(id);
                    if (desconto == null)
                    {
                        return HttpNotFound();
                    }
                    return View(desconto);
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // POST: Descontos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Desconto desconto = db.Descontoes.Find(id);
            db.Descontoes.Remove(desconto);
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
