using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PfcSsaNeon.Helper;
using PfcSsaNeon.Models;

namespace PfcSsaNeon.Controllers
{
    public class FuncionariosController : Controller
    {
        private PfcSsaNeonContext db = new PfcSsaNeonContext();
        private ApplicationDbContext context = new ApplicationDbContext();

        // GET: Funcionarios
        public ActionResult Index()
        {
            try
            {
                var x = User.Identity.Name;
                var u = context.Users.Where(z => z.Email.Equals(x)).FirstOrDefault();

                if (Verificar.VerificarAcesso(1, 1, u))
                {
                    return View(db.Funcionarios.ToList());
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // GET: Funcionarios/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                var x = User.Identity.Name;
                var u = context.Users.Where(z => z.Email.Equals(x)).FirstOrDefault();
                if (Verificar.VerificarAcesso(1, 2, u))
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Funcionario funcionario = db.Funcionarios.Find(id);
                    if (funcionario == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["FuncionarioId"] = funcionario.FuncionarioId;
                    if (TempData["d1"] != null && TempData["d2"] != null)
                    {
                        DateTime d1 = (DateTime)TempData["d1"];
                        DateTime d2 = (DateTime)TempData["d2"];
                        ViewBag.pontos = db.Pontoes.Where(p => p.FuncionarioId == id && p.DataPonto >= d1 && p.DataPonto <= d2).ToList();
                    }
                    else
                    {
                        ViewBag.pontos = db.Pontoes.Where(p => p.FuncionarioId == id && p.DataPonto.Month == DateTime.Now.Month).ToList();
                    }

                    if (db.DadosBancarios.FirstOrDefault(f => f.FuncionarioId == id) == null)
                    {
                        ViewBag.dadosBancario = null;
                    }
                    else
                    {
                        DadosBancario dadosBancario = db.DadosBancarios.First(f => f.FuncionarioId == id);
                        ViewBag.dadosBancario = dadosBancario;
                    }

                    if (db.Salarios.FirstOrDefault(i => i.FuncionarioId == id) != null)
                    {
                        Salario salario = db.Salarios.First(i => i.FuncionarioId == id);
                        ViewBag.salario = salario;
                    }
                    else
                    {
                        ViewBag.salario = null;
                    }
                    if (!db.Ferias.Where(f => f.FuncionarioId == id).Any())
                    {
                        ViewBag.verificar = true;
                    }
                    else
                    {
                        ViewBag.verificar = false;
                    }
                    ViewBag.Nv = u.PRh;

                    return View(funcionario);
                }

                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // POST: Pontos/Create      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(DateTime DataPonto, DateTime? EntradaManha, DateTime? SaidaManha, DateTime? EntradaTarde, DateTime? SaidaTarde, DateTime? EntradaExtra, DateTime? SaidaExtra, bool Abonar)
        {
            if (DataPonto == null)
            {
                return RedirectToAction("erroPonto", "Erros");
            }
            if (DataPonto > DateTime.Now)
            {
                return RedirectToAction("erroPonto", "Erros");
            }
            if (EntradaManha >= SaidaManha || EntradaTarde >= SaidaTarde)
            {
                return RedirectToAction("erroPonto", "Erros");
            }
            if (EntradaManha == null && SaidaManha != null || EntradaManha != null && SaidaManha == null)
            {
                return RedirectToAction("erroPonto", "Erros");
            }
            if (EntradaTarde == null && SaidaTarde != null || EntradaTarde != null && SaidaTarde == null)
            {
                return RedirectToAction("erroPonto", "Erros");
            }
            if (EntradaManha == null && SaidaManha == null && EntradaTarde == null && SaidaTarde == null && EntradaExtra == null && SaidaExtra == null && Abonar == false)
            {
                return RedirectToAction("erroPonto", "Erros");
            }

            var funcionarioId = Convert.ToInt32(TempData["FuncionarioId"]);

            Ponto ponto = new Ponto();
            ponto.DataPonto = DataPonto;
            ponto.EntradaManha = EntradaManha;
            ponto.SaidaManha = SaidaManha;
            ponto.EntradaTarde = EntradaTarde;
            ponto.SaidaTarde = SaidaTarde;
            ponto.EntradaExtra = EntradaExtra;
            ponto.SaidaExtra = SaidaExtra;
            ponto.FuncionarioId = funcionarioId;
            ponto.Abonar = Abonar;

            if (ModelState.IsValid)
            {
                db.Pontoes.Add(ponto);
                db.SaveChanges();
                return RedirectToAction("Details" + "/" + funcionarioId, "Funcionarios");
            }
            return View(ponto);
        }

        [HttpPost]
        public ActionResult Filtro(DateTime d1, DateTime d2)
        {
            if (d2 < d1)
            {
                ModelState.AddModelError("d2", "INFORME UM PERIODO VALIDO");
            }
            TempData["d1"] = d1;
            TempData["d2"] = d2;
            return RedirectToAction("Details" + "/" + (int)TempData["FuncionarioId"], "Funcionarios");
        }

        // GET: Funcionarios/Create
        public ActionResult Create()
        {
            try
            {
                var x = User.Identity.Name;
                var u = context.Users.Where(z => z.Email.Equals(x)).FirstOrDefault();
                if (Verificar.VerificarAcesso(1, 4, u))
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

        // POST: Funcionarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FuncionarioId,Nome,Sobrenome,NomePai,NomeMae,QuantidadeFilhos,CPF,RG,Nascimento,CNH,TituloEleitor,CertMilitar,PIS,Admissao,Cargo,HorasDia,DiasJornada,EstadoCivil,NomeConjugue,Endereco,Cidade,CEP")] Funcionario funcionario)
        {
            var verificar = db.Funcionarios;
            foreach (var verifica in verificar)
            {
                if (funcionario.CPF == verifica.CPF)
                {
                    ModelState.AddModelError("CPF", "ESTE CPF JÁ ESTÁ CADASTRADO!");
                }
                if (funcionario.RG == verifica.RG)
                {
                    ModelState.AddModelError("RG", "ESTE RG JÁ ESTÁ CADASTRADO!");
                }
                if (funcionario.CNH == verifica.CNH && funcionario.CNH != null)
                {
                    ModelState.AddModelError("CNH", "ESTE CNH JÁ ESTÁ CADASTRADO!");
                }
                if (funcionario.TituloEleitor == verifica.TituloEleitor && funcionario.TituloEleitor != null)
                {
                    ModelState.AddModelError("TituloEleitor", "ESTE TITULO DE ELEITOR JÁ ESTÁ CADASTRADO!");
                }
                if (funcionario.CertMilitar == verifica.CertMilitar && funcionario.CertMilitar != null)
                {
                    ModelState.AddModelError("CertMilitar", "ESTE CERTIFICADO DE ALISTAMENTO MILITAR JÁ ESTÁ CADASTRADO!");
                }
                if (funcionario.Admissao > DateTime.Now)
                {
                    ModelState.AddModelError("Admissao", "INFORME UMA DATA VALIDA!");
                }
                if (funcionario.Nascimento >= DateTime.Now)
                {
                    ModelState.AddModelError("Admissao", "INFORME UMA DATA VALIDA!");
                }
            }

            if (ModelState.IsValid)
            {
                funcionario.Esta_Ativo = true;

                db.Funcionarios.Add(funcionario);
                db.SaveChanges();
                //TempData["FuncionarioId"] = funcionario.FuncionarioId;
                return RedirectToAction("Create" , "DadosBancarios",new{id = funcionario.FuncionarioId ,verificar = true });
            }

            return View(funcionario);
        }

        // GET: Funcionarios/Edit/5
        public ActionResult Edit(int? id, bool? verificar)
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
                    Funcionario funcionario = db.Funcionarios.Find(id);                   
                    Decimal.ToInt32(funcionario.HorasDia);
                    if (funcionario == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["verificar"] = verificar;
                    ViewBag.verificar = verificar;
                    return View(funcionario);
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // POST: Funcionarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FuncionarioId,Nome,Sobrenome,NomePai,NomeMae,QuantidadeFilhos,CPF,RG,Nascimento,CNH,TituloEleitor,CertMilitar,PIS,Admissao,Cargo,HorasDia,DiasJornada,EstadoCivil,Esta_Ativo,NomeConjugue,Endereco,Cidade,CEP")] Funcionario funcionario)
        {
            var verificar = db.Funcionarios.Where(f => f.FuncionarioId != funcionario.FuncionarioId);
            foreach (var verifica in verificar)
            {
                if (funcionario.CPF == verifica.CPF)
                {
                    ModelState.AddModelError("CPF", "ESTE CPF JÁ ESTÁ CADASTRADO!");
                }
                if (funcionario.RG == verifica.RG)
                {
                    ModelState.AddModelError("RG", "ESTE RG JÁ ESTÁ CADASTRADO!");
                }
                if (funcionario.CNH == verifica.CNH && funcionario.CNH != null)
                {
                    ModelState.AddModelError("CNH", "ESTE CNH JÁ ESTÁ CADASTRADO!");
                }
                if (funcionario.TituloEleitor == verifica.TituloEleitor && funcionario.TituloEleitor != null)
                {
                    ModelState.AddModelError("TituloEleitor", "ESTE TITULO DE ELEITOR JÁ ESTÁ CADASTRADO!");
                }
                if (funcionario.CertMilitar == verifica.CertMilitar && funcionario.CertMilitar != null)
                {
                    ModelState.AddModelError("CertMilitar", "ESTE CERTIFICADO DE ALISTAMENTO MILITAR JÁ ESTÁ CADASTRADO!");
                }
            }
            if (TempData["verificar"] != null)
            {
                funcionario.Esta_Ativo = true;
            }
            if (ModelState.IsValid)
            {
                Convert.ToInt32(funcionario.HorasDia);
                db.Entry(funcionario).State = EntityState.Modified;
                db.SaveChanges();
                if (TempData["verificar"] != null)
                {                   
                    return RedirectToAction("Create", "DadosBancarios", new { id = funcionario.FuncionarioId, verificar = true });
                }
                return RedirectToAction("details" + "/" + funcionario.FuncionarioId, "Funcionarios");
            }
            return View(funcionario);
        }

        // GET: Funcionarios/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Funcionario funcionario = db.Funcionarios.Find(id);
                if (funcionario == null)
                {
                    return HttpNotFound();
                }
                return View(funcionario);
            }
            catch
            {
                return HttpNotFound();
            }
        }

        // POST: Funcionarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Funcionario funcionario = db.Funcionarios.Find(id);
            db.Funcionarios.Remove(funcionario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Pontos/Create
        public ActionResult CreatePonto()
        {
            try
            {
                var u = (ApplicationUser)context.Users.Find((int)Session["UsuarioId"]);
                if (Verificar.VerificarAcesso(1, 2, u))
                {
                    ViewBag.FuncionarioId = new SelectList(db.Funcionarios, "FuncionarioId", "Nome");
                    return View();
                }

                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // POST: Pontos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePonto([Bind(Include = "PontoId,DataPonto,EntradaManha,SaidaManha,EntradaTarde,SaidaTarde,EntradaExtra,SaidaExtra,FuncionarioId")] Ponto ponto)
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
