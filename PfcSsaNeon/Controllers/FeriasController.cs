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
    public class FeriasController : Controller
    {
        private PfcSsaNeonContext db = new PfcSsaNeonContext();
        private ApplicationDbContext context = new ApplicationDbContext();

        // GET: Ferias
        public ActionResult Index(int? id)
        {
            try
            {
                var x = User.Identity.Name;
                var u = context.Users.Where(z => z.Email.Equals(x)).FirstOrDefault();
                if (Verificar.VerificarAcesso(1, 3, u))
                {

                    var ferias = db.Ferias.Include(f => f.Funcionarios);
                    if (id != null)
                    {
                        ferias = db.Ferias.Include(f => f.Funcionarios).Where(f => f.FuncionarioId == id);
                    }
                    ViewBag.Fid = id;
                    return View(ferias.ToList());
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // GET: Ferias/Details/5
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
                    ViewBag.funcionario = db.Funcionarios.Find(id);
                    var ferias = db.Ferias.Where(f => f.FuncionarioId == id);
                    if (ferias == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.ferias = ferias;
                    return View(ferias);
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // GET: Ferias/Create
        public ActionResult Create(int? id)
        {
            try
            {
                var x = User.Identity.Name;
                var u = context.Users.Where(z => z.Email.Equals(x)).FirstOrDefault();
                if (Verificar.VerificarAcesso(1, 4, u))
                {
                    ViewBag.FuncionarioId = new SelectList(db.Funcionarios, "FuncionarioId", "Nome");
                    Funcionario funcionario = db.Funcionarios.Find(id);
                    ViewBag.NomeF = funcionario;
                    TempData["Funcionario"] = id;
                    return View();
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // POST: Ferias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FeriasId,InicioFerias,Dias,FuncionarioId")] Ferias ferias)
        {
            ferias.FuncionarioId = (int)TempData["Funcionario"];
            if (ModelState.IsValid)
            {
                db.Ferias.Add(ferias);
                db.SaveChanges();
                return RedirectToAction("Index" + "/" + ferias.FuncionarioId, "Ferias");
            }

            ViewBag.FuncionarioId = new SelectList(db.Funcionarios, "FuncionarioId", "Nome", ferias.FuncionarioId);
            return View(ferias);
        }

        // GET: Ferias/Edit/5
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
                    Ferias ferias = db.Ferias.Find(id);
                    if (ferias == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.FuncionarioId = new SelectList(db.Funcionarios, "FuncionarioId", "Nome", ferias.FuncionarioId);
                    return View(ferias);
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // POST: Ferias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FeriasId,InicioFerias,Dias,FuncionarioId")] Ferias ferias)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ferias).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index" + "/" + ferias.FuncionarioId, "Ferias");
            }
            ViewBag.FuncionarioId = new SelectList(db.Funcionarios, "FuncionarioId", "Nome", ferias.FuncionarioId);
            return View(ferias);
        }

        // GET: Ferias/Delete/5
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
                    Ferias ferias = db.Ferias.Find(id);
                    if (ferias == null)
                    {
                        return HttpNotFound();
                    }
                    return View(ferias);
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // POST: Ferias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ferias ferias = db.Ferias.Find(id);
            db.Ferias.Remove(ferias);
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
