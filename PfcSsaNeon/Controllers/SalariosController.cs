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
    public class SalariosController : Controller
    {
        private PfcSsaNeonContext db = new PfcSsaNeonContext();
        private ApplicationDbContext context = new ApplicationDbContext();
        // GET: Salarios
        public ActionResult Index()
        {
            try
            {
                var x = User.Identity.Name;
                var u = context.Users.Where(z => z.Email.Equals(x)).FirstOrDefault();
                if (Verificar.VerificarAcesso(1, 3, u))
                {
                    var salarios = db.Salarios.Include(s => s.Funcionarios);
                    return View(salarios.ToList());
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // GET: Salarios/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                var u = (ApplicationUser)context.Users.Find((int)Session["UsuarioId"]);
                if (Verificar.VerificarAcesso(1, 3, u))
                {

                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Salario salario = db.Salarios.Find(id);
                    if (salario == null)
                    {
                        return HttpNotFound();
                    }
                    return View(salario);
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // GET: Salarios/Create
        public ActionResult Create(int? FId)
        {
            try
            {
                var x = User.Identity.Name;
                var u = context.Users.Where(z => z.Email.Equals(x)).FirstOrDefault();
                if (Verificar.VerificarAcesso(1, 4, u))
                {

                    Funcionario funcionario = db.Funcionarios.Find(FId);
                    ViewBag.NomeF = funcionario;

                    ViewBag.FuncionarioId = funcionario.FuncionarioId;
                    if (db.DadosBancarios.FirstOrDefault(d => d.FuncionarioId == FId) != null)
                    {
                        DadosBancario dadosBancarios = db.DadosBancarios.First(d => d.FuncionarioId == FId);
                        ViewBag.dadosbancariosid = dadosBancarios.DadosBancarioId;
                    }
                    else
                    {
                        ViewBag.dadosbancariosid = null;
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

        // POST: Salarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SalarioId,Base,INSS,Familia")] Salario salario, int FuncionarioId)
        {
            salario.FuncionarioId = FuncionarioId;
            if (ModelState.IsValid)
            {
                db.Salarios.Add(salario);
                db.SaveChanges();
                return RedirectToAction("Details" + "/" + FuncionarioId, "Funcionarios");
            }
            return View(salario);
        }

        // GET: Salarios/Edit/5
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
                    Salario salario = db.Salarios.Find(id);
                    if (salario == null)
                    {
                        return HttpNotFound();
                    }
                    Funcionario funcionario = db.Funcionarios.Find(salario.FuncionarioId);
                    ViewBag.FuncionarioId = funcionario.FuncionarioId;
                    ViewBag.NomeF = db.Funcionarios.Find(salario.FuncionarioId);

                    return View(salario);
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // POST: Salarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SalarioId,Base,INSS,Familia,FuncionarioId")] Salario salario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details" + "/" + salario.FuncionarioId, "Funcionarios");
            }
            ViewBag.FuncionarioId = new SelectList(db.Funcionarios, "FuncionarioId", "Nome", salario.FuncionarioId);
            return View(salario);
        }

        // GET: Salarios/Delete/5
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
                    Salario salario = db.Salarios.Find(id);
                    if (salario == null)
                    {
                        return HttpNotFound();
                    }
                    return View(salario);
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // POST: Salarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Salario salario = db.Salarios.Find(id);
            db.Salarios.Remove(salario);
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
