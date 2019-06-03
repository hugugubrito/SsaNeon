using Microsoft.AspNet.Identity.EntityFramework;
using PfcSsaNeon.Helper;
using PfcSsaNeon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PfcSsaNeon.Controllers
{
    public class HomeController : Controller
    {
        private PfcSsaNeonContext db = new PfcSsaNeonContext();
        private ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult Index()
        {
            try
            {
                var x = User.Identity.Name;
                var u = context.Users.Where(z => z.Email.Equals(x)).FirstOrDefault();
                if (Verificar.VerificarAcesso(1, 1, u))
                {
                    ViewBag.Editar = Session["UsuarioId"];
                    ViewBag.Nome = Session["UsuarioNome"];
                    List<FeriasVw> Fv = new List<FeriasVw>();
                    var f1 = db.Funcionarios.Where(f => f.Esta_Ativo == true);
                    List<Ferias> listaferias = new List<Ferias>();
                    IQueryable<Ferias> fe;
                    foreach (var dados in f1)
                    {
                        fe = db.Ferias.Where(a => a.FuncionarioId == dados.FuncionarioId);
                        int i = 0;
                        foreach (var a in fe)
                        {
                            if (i < a.FeriasId)
                            {
                                i = a.FeriasId;
                            }
                        }
                        FeriasVw ferias;
                        var Ferias = db.Ferias.Find(i);
                        if (Ferias == null)
                        {
                            ferias = new FeriasVw(dados.FuncionarioId, dados.Nome + " " + dados.Sobrenome, dados.CPF, dados.Cargo, 0, dados.Admissao);
                        }
                        else
                        {
                            ferias = new FeriasVw(dados.FuncionarioId, dados.Nome + " " + dados.Sobrenome, dados.CPF, dados.Cargo, Ferias.Dias, Ferias.InicioFerias);
                        }
                        Fv.Add(ferias);
                    }
                    ViewBag.f1 = Fv;

                    return View();
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // GET: Create
        public ActionResult GerarFolha()
        {
            try
            {
                var x = User.Identity.Name;
                var u = context.Users.Where(z => z.Email.Equals(x)).FirstOrDefault();
                if (Verificar.VerificarAcesso(1, 4, u))
                {
                    ViewBag.FuncionariosId = db.Funcionarios.Where(f => f.Esta_Ativo == true);
                    return View();
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GerarFolha(Desconto desconto)
        {
            if (ModelState.IsValid)
            {
                db.Descontoes.Add(desconto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SalarioId = new SelectList(db.Salarios, "SalarioId", "SalarioId", desconto.SalarioId);
            return View(desconto);
        }

        public ActionResult Redirect(ApplicationUser usuario)
        {

            var per = new Dictionary<string, int>();

            per.Add("usuario", usuario.PUsuario);
            per.Add("RH", usuario.PRh);
            per.Add("Financeiro", usuario.PFinanceiro);

            if (usuario.PUsuario==0 && usuario.PRh==0 && usuario.PFinanceiro==0)
            {
                return RedirectToAction("Index");
            }
            var y = per.Aggregate((l, r) => l.Value >= r.Value ? l : r).Key;
            if (y.Equals("RH"))
            {
                return RedirectToAction("Index");
            }
            else if (y.Equals("Financeiro"))
            {
                return RedirectToAction("Index", "DadosNotas");
            }
            else if (y.Equals("usuario"))
            {
                return RedirectToAction("GerenciarUsuarios", "Account");
            }
            return HttpNotFound();
        }

    }
}