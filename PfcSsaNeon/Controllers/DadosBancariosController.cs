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
    public class DadosBancariosController : Controller
    {
        private PfcSsaNeonContext db = new PfcSsaNeonContext();
        private ApplicationDbContext context = new ApplicationDbContext();

        // GET: DadosBancarios
        public ActionResult Index()
        {
            try
            {
                var x = User.Identity.Name;
                var u = context.Users.Where(z => z.Email.Equals(x)).FirstOrDefault();
                if (Verificar.VerificarAcesso(1, 3, u))
                {
                    var dadosBancarios = db.DadosBancarios.Include(d => d.Funcionarios);
                    return View(dadosBancarios.ToList());
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // GET: DadosBancarios/Details/5
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
                    DadosBancario dadosBancario = db.DadosBancarios.Find(id);
                    if (dadosBancario == null)
                    {
                        return HttpNotFound();
                    }
                    return View(dadosBancario);
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // GET: DadosBancarios/Create
        public ActionResult Create(int? id, bool? verificar)
        {
            try
            {
                var x = User.Identity.Name;
                var u = context.Users.Where(z => z.Email.Equals(x)).FirstOrDefault();
                if (Verificar.VerificarAcesso(1, 4, u))
                {
                    Funcionario funcionario = db.Funcionarios.Find(id);
                    ViewBag.F1 = funcionario.FuncionarioId;
                    ViewBag.NomeF = funcionario;
                    if (verificar != null) {
                    TempData["verificar"] = true;
                    }
                    return View();
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros"); 
            }
        }

        // POST: DadosBancarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DadosBancarioId,Banco,AG,Conta")] DadosBancario dadosBancario, int FuncionarioId)
        {
            dadosBancario.FuncionarioId = FuncionarioId;
            if (ModelState.IsValid)
            {
                db.DadosBancarios.Add(dadosBancario);
                db.SaveChanges();
                if (TempData["verificar"] != null)
                {
                    return RedirectToAction("Create", "Salarios", new { FId = FuncionarioId, dBId = dadosBancario.DadosBancarioId });
                }
                else
                {
                    return RedirectToAction("details" + "/" + FuncionarioId, "Funcionarios");            
                }
            }
            return View(dadosBancario);
        }

        // GET: DadosBancarios/Edit/5
        public ActionResult Edit(int? id, int? funcionarioId, bool? verificar)
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
                    DadosBancario dadosBancario = db.DadosBancarios.Find(id);
                    if (dadosBancario == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["verificar"] = verificar;

                    Funcionario funcionario = db.Funcionarios.Find(funcionarioId);
                    ViewBag.FuncionarioId = funcionario.FuncionarioId;
                    ViewBag.NomeF = funcionario;
                    return View(dadosBancario);
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // POST: DadosBancarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DadosBancarioId,Banco,AG,Conta,FuncionarioId")] DadosBancario dadosBancario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dadosBancario).State = EntityState.Modified;
                db.SaveChanges();
                if (TempData["verificar"] != null)
                {
                    return RedirectToAction("Create", "Salarios", new { FId = dadosBancario.FuncionarioId, dBId = dadosBancario.DadosBancarioId });
                }
                return RedirectToAction("details" + "/" + dadosBancario.FuncionarioId, "Funcionarios");
            }
            ViewBag.FuncionarioId = new SelectList(db.Funcionarios, "FuncionarioId", "Nome", dadosBancario.FuncionarioId);
            return View(dadosBancario);
        }

        // GET: DadosBancarios/Delete/5
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
                    DadosBancario dadosBancario = db.DadosBancarios.Find(id);
                    if (dadosBancario == null)
                    {
                        return HttpNotFound();
                    }
                    return View(dadosBancario);
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // POST: DadosBancarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DadosBancario dadosBancario = db.DadosBancarios.Find(id);
            db.DadosBancarios.Remove(dadosBancario);
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
