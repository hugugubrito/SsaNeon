using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using PfcSsaNeon.Helper;
using PfcSsaNeon.Models;

namespace PfcSsaNeon.Controllers
{
    public class DadosNotasController : Controller
    {
        private PfcSsaNeonContext db = new PfcSsaNeonContext();
        private ApplicationDbContext context = new ApplicationDbContext();

        private DateTime DataVerificada;
        // GET: DadosNotas
        public ActionResult Index()
        {
            try
            {
                var x = User.Identity.Name;
                var u = context.Users.Where(z => z.Email.Equals(x)).FirstOrDefault();
                if (Verificar.VerificarAcesso(2, 2, u))
                {

                    if (DataVerificada != DateTime.Now)
                    {
                        DataVerificada = DateTime.Now;
                        ViewBag.Editar = Session["UsuarioId"];
                        ViewBag.Nome = Session["UsuarioNome"];
                        var notas = db.DadosNotas.Where(n => n.Status == Status.PENDENTE && n.Vencimento < DateTime.Now);
                        foreach (var vencidas in notas)
                        {
                            vencidas.Status = Status.VENCIDO;
                            db.Entry(vencidas).State = EntityState.Modified;
                        }
                        db.SaveChanges();
                    }
                    string y = (string)TempData["numero"];
                    if (y != null)
                    {
                        if (y=="")
                        {
                            return View(db.DadosNotas.ToList());
                        }
                        var z = db.DadosNotas.Where(n => n.NumeroNf.Equals(y)).ToList();
                        return View(z);
                    }

                    return View(db.DadosNotas.ToList());
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        [HttpPost]
        public ActionResult Index(string numero)
        {
            if (numero == null)
            {
                TempData["numero"] = "null";
            }
            TempData["numero"] = numero;
            return RedirectToAction("Index", "DadosNotas");
        }

        // GET: DadosNotas/Details/5
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
                    DadosNota dadosNota = db.DadosNotas.Find(id);
                    if (dadosNota == null)
                    {
                        return HttpNotFound();
                    }
                    var arquivo = db.ArquivoNotas.Where(a => a.DadosNotaId == id).FirstOrDefault();

                    string mime = MimeMapping.GetMimeMapping(arquivo.NomeArquivo);
                    ViewBag.mime = mime;
                    ViewBag.arquivos = db.ArquivoNotas.Where(a => a.DadosNotaId == id).ToList();

                    return View(dadosNota);
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        public FileResult Download(int id)
        {
            var arquivo = db.ArquivoNotas.Find(id);

            string mime = MimeMapping.GetMimeMapping(arquivo.NomeArquivo);
            string mimetype = "";
            switch (mime)
            {
                case "application/pdf":
                    mimetype = "pdf";
                    break;
                case "image/jpeg":
                    mimetype = "jpeg";
                    break;
                case "image/jpg":
                    mimetype = "jpg";
                    break;
                case "image/png":
                    mimetype = "png";
                    break;
            }
            //Os parametros para o arquivo são
            //1. o caminho do aruivo on servidor
            //2. o tipo de conteudo do tipo MIME
            //3. o parametro para o arquivos salvo pelo navegador
            return File(arquivo.CaminhoArquivo, mime, "DOC_NOTAS." + mimetype);
        }

        // GET: DadosNotas/Create
        public ActionResult Create()
        {
            try
            {
                var x = User.Identity.Name;
                var u = context.Users.Where(z => z.Email.Equals(x)).FirstOrDefault();
                if (Verificar.VerificarAcesso(2, 2, u))
                {
                    return View();
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // POST: DadosNotas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DadosNotaId,Cliente,CNPJ,NumeroNf,Emissao,Valor,Vencimento,Observacao,Pagamento,Cobranca")] DadosNota dadosNota, IEnumerable<HttpPostedFileBase> Arquivos)
        {
            if (ModelState.IsValid)
            {
                if (dadosNota.Vencimento < DateTime.Now)
                {
                    dadosNota.Status = Status.VENCIDO;
                }
                else
                {
                    dadosNota.Status = Status.PENDENTE;
                }

                db.DadosNotas.Add(dadosNota);
                try
                {
                    foreach (var arquivo in Arquivos)
                    {
                        ArquivoNota a1 = new ArquivoNota
                        {
                            DadosNotaId = dadosNota.DadosNotaId
                        };
                        if (arquivo.ContentLength > 0)
                        {
                            a1.NomeArquivo = Path.GetFileName(arquivo.FileName);
                            a1.CaminhoArquivo = Path.Combine(Server.MapPath("~/Notas"), a1.NomeArquivo);
                            arquivo.SaveAs(a1.CaminhoArquivo);
                            db.ArquivoNotas.Add(a1);
                        }
                    }
                }
                catch
                {
                    ModelState.AddModelError("Arquivos", "ARQUIVO INVALIDO");
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dadosNota);
        }

        // GET: DadosNotas/Edit/5
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
                    DadosNota dadosNota = db.DadosNotas.Find(id);
                    if (dadosNota == null)
                    {
                        return HttpNotFound();
                    }
                    return View(dadosNota);
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // POST: DadosNotas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DadosNotaId,Cliente,CNPJ,NumeroNf,Emissao,Valor,Vencimento,Observacao,Pagamento,Cobranca")] DadosNota dadosNota)
        {
            if (ModelState.IsValid)
            {
                if (dadosNota.Pagamento != null)
                {
                    dadosNota.Status = Status.PAGO;
                }
                db.Entry(dadosNota).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dadosNota);
        }

        // GET: DadosNotas/Delete/5
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
                    DadosNota dadosNota = db.DadosNotas.Find(id);
                    if (dadosNota == null)
                    {
                        return HttpNotFound();
                    }
                    return View(dadosNota);
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // POST: DadosNotas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DadosNota dadosNota = db.DadosNotas.Find(id);
            db.DadosNotas.Remove(dadosNota);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CancelarNota(int id)
        {
            var x = User.Identity.Name;
            var u = context.Users.Where(z => z.Email.Equals(x)).FirstOrDefault();
            if (Verificar.VerificarAcesso(2, 4, u))
            {

                var nota = db.DadosNotas.Find(id);
                nota.Status = Status.CANCELADO;
                db.Entry(nota).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound();
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
