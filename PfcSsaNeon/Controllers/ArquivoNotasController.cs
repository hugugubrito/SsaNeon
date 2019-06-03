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
    public class ArquivoNotasController : Controller
    {
        private PfcSsaNeonContext db = new PfcSsaNeonContext();
        private ApplicationDbContext context = new ApplicationDbContext();

        // GET: ArquivoNotas
        public ActionResult Index(int? id)
        {
            try
            {
                var x = User.Identity.Name;
                var u = context.Users.Where(z => z.Email.Equals(x)).FirstOrDefault();
                if (Verificar.VerificarAcesso(2, 2, u))
                {
                    var arquivoNotas = db.ArquivoNotas.Include(a => a.DadosNota);
                    if (id != null)
                    {
                        arquivoNotas = db.ArquivoNotas.Include(a => a.DadosNota).Where(a => a.DadosNotaId == id);
                    }
                    return View(arquivoNotas.ToList());
                }

                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // GET: ArquivoNotas/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                var x = User.Identity.Name;
                var u = context.Users.Where(z => z.Email.Equals(x)).FirstOrDefault();
                if (Verificar.VerificarAcesso(2, 2, u))
                {

                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    ArquivoNota arquivoNota = db.ArquivoNotas.Find(id);
                    if (arquivoNota == null)
                    {
                        return HttpNotFound();
                    }
                    string mime = MimeMapping.GetMimeMapping(arquivoNota.NomeArquivo);
                    ViewBag.mime = mime;
                    return View(arquivoNota);
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // GET: ArquivoNotas/Create
        public ActionResult Create()
        {
            try
            {
                var x = User.Identity.Name;
                var u = context.Users.Where(z => z.Email.Equals(x)).FirstOrDefault();
                if (Verificar.VerificarAcesso(2, 2, u))
                {

                    ViewBag.DadosNotaId = new SelectList(db.DadosNotas, "DadosNotaId", "Cliente");
                    return View();
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // POST: ArquivoNotas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArquivoNotaId,NomeArquivo,CaminhoArquivo,DadosNotaId")] ArquivoNota arquivoNota)
        {
            if (ModelState.IsValid)
            {
                db.ArquivoNotas.Add(arquivoNota);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DadosNotaId = new SelectList(db.DadosNotas, "DadosNotaId", "Cliente", arquivoNota.DadosNotaId);
            return View(arquivoNota);
        }

        // GET: ArquivoNotas/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                var x = User.Identity.Name;
                var u = context.Users.Where(z => z.Email.Equals(x)).FirstOrDefault();
                if (Verificar.VerificarAcesso(2, 2, u))
                {

                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    ArquivoNota arquivoNota = db.ArquivoNotas.Find(id);
                    if (arquivoNota == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.DadosNotaId = new SelectList(db.DadosNotas, "DadosNotaId", "Cliente", arquivoNota.DadosNotaId);
                    return View(arquivoNota);
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // POST: ArquivoNotas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArquivoNotaId,NomeArquivo,CaminhoArquivo,DadosNotaId")] ArquivoNota arquivoNota)
        {
            if (ModelState.IsValid)
            {
                db.Entry(arquivoNota).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DadosNotaId = new SelectList(db.DadosNotas, "DadosNotaId", "Cliente", arquivoNota.DadosNotaId);
            return View(arquivoNota);
        }

        // GET: ArquivoNotas/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                var x = User.Identity.Name;
                var u = context.Users.Where(z => z.Email.Equals(x)).FirstOrDefault();
                if (Verificar.VerificarAcesso(2, 6, u))
                {

                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    ArquivoNota arquivoNota = db.ArquivoNotas.Find(id);
                    if (arquivoNota == null)
                    {
                        return HttpNotFound();
                    }
                    return View(arquivoNota);
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // POST: ArquivoNotas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArquivoNota arquivoNota = db.ArquivoNotas.Find(id);
            db.ArquivoNotas.Remove(arquivoNota);
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
