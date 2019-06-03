using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Rotativa;
using PfcSsaNeon.Models;
using PfcSsaNeon.Helper;
using PagedList;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PfcSsaNeon.Controllers
{
    public class FolhaController : Controller
    {
        private PfcSsaNeonContext db = new PfcSsaNeonContext();
        private ApplicationDbContext context = new ApplicationDbContext();

        //Get Dados Folha
        public ActionResult dadosFolha(int? id)
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
                    ViewBag.Funcionario = id;
                    return View();
                }
                return RedirectToAction("erroPermissao", "Erros");
            }
            catch
            {
                return RedirectToAction("erroPermissao", "Erros");
            }
        }

        // POST Dados Folha
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult dadosFolha(DateTime d1, DateTime d2, int FuncionarioId, bool isferias,string FormaPagamento)
        {
            ViewBag.FormaPagamento = FormaPagamento;
            return FolhaPDF(FuncionarioId, d1, d2, (bool)isferias);
        }

        private FolhaSalario Make(DateTime d1, DateTime d2, int FuncionarioId, bool? isferias)
        {
            FolhaSalario f1 = SalarioPdf(d1, d2, FuncionarioId, (bool)isferias);

            if (isferias == false)
            {
                ViewBag.Total = SalarioTotal(f1, d1, d2);
            }
            else
            {
                ViewBag.Total = FeriasTotal(f1, d1);
            }

            return f1;
        }


        public ActionResult FolhaPDF(int? id, DateTime d1, DateTime d2, bool isFerias = false)
        {
            try
            {
                if (id != null)
                {

                    var FolhaPDF = Make(d1, d2, (int)id, isFerias);

                    ViewBag.Folha = FolhaPDF;
                    var pdf = new ViewAsPdf
                    {
                        ViewName = "FolhaPDF",
                        IsGrayScale = false,
                        PageSize = Rotativa.Options.Size.A4,
                        Model = FolhaPDF
                    };
                    return pdf;
                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch
            {
                return RedirectToAction("erroFolha", "Erros");
            }
        }
        public FolhaSalario SalarioPdf(DateTime D1, DateTime D2, int id, bool isferias = false)
        {
            var Funcionario = new FolhaSalario();

            if (id != 0)
            {

                Funcionario.FuncionarioId = id;
                Funcionario.Funcionario = db.Funcionarios.Find(id);
                Funcionario.Nome = db.Funcionarios.Find(id).Nome;
                Funcionario.S = db.Salarios.Single(s => s.FuncionarioId == id);
                Funcionario.D = db.Descontoes.Where(d => d.SalarioId == Funcionario.S.SalarioId);
                var dadosbancarios = db.DadosBancarios.Single(b => b.FuncionarioId == id);
                if (dadosbancarios != null)
                {
                    Funcionario.B = dadosbancarios;
                }
                else
                {
                    Funcionario.B = new DadosBancario();
                }

                if (isferias == true)
                {
                    Funcionario.F = db.Ferias.Single(fe => fe.FuncionarioId == id && fe.InicioFerias.Month == D1.Month);
                    Funcionario.P = null;
                }
                else
                {
                    Funcionario.P = db.Pontoes.Where(p => p.FuncionarioId == id && ((p.DataPonto >= D1 || p.DataPonto <= D2)));
                    Funcionario.F = null;
                }

            }

            return Funcionario;
        }

        private Decimal AplicarDesconto(FolhaSalario F, Decimal Valor)
        {

            decimal valor = 0;
            // Aplicando todos os descontos;
            foreach (var d in F.D)
            {
                if (d.Valor > 0)
                {
                    if (d.Percento)
                    {
                        valor += (Valor * d.Valor) / 100;
                    }
                    else
                    {
                        valor += d.Valor;
                    }
                }
            }

            return valor;
        }

        private decimal SalarioTotal(FolhaSalario F, DateTime D1, DateTime D2)
        {
            // Pegando quando deve ser pago
            var sumHoras = 0;
            var sumMinutes = 0;
            var totalDias = 0;
            foreach (var reg in F.P)
            {
                totalDias++;

                if (reg.SaidaManha != null && reg.EntradaManha != null)
                {
                    DateTime SaidaManha = (DateTime)reg.SaidaManha;
                    DateTime EntradaManha = (DateTime)reg.EntradaManha;
                    sumHoras += (SaidaManha.Hour - EntradaManha.Hour);
                    sumMinutes += (SaidaManha.Minute - EntradaManha.Minute);
                }

                if (reg.SaidaTarde != null && reg.EntradaTarde != null)
                {
                    DateTime SaidaTarde = (DateTime)reg.SaidaTarde;
                    DateTime EntradaTarde = (DateTime)reg.EntradaTarde;
                    sumHoras += (SaidaTarde.Hour - EntradaTarde.Hour);
                    sumMinutes += (SaidaTarde.Minute - EntradaTarde.Minute);
                }
                if (reg.Abonar == true)
                {
                    sumHoras += (int)F.Funcionario.HorasDia;
                }
            }

            var help = (int)sumMinutes / 60;
            sumHoras += help;
            sumMinutes -= help * 60;

            var DiasHora = sumHoras / F.Funcionario.HorasDia;

            int diasM = DateTime.DaysInMonth(D1.Year, D1.Month);

            var trabalho = F.Funcionario.HorasDia * F.Funcionario.DiasJornada;

            var trienio = DateTime.Now.Subtract(F.Funcionario.Admissao);
            decimal sBruto;
            decimal sbaseDias;
            var a = (int)(trienio.TotalDays / 365);
            if (a >= 3)
            {
                decimal d = a / 3;

                sBruto = (decimal)(F.S.Base + (F.S.Base * (Math.Floor(d) * 3) / 100) + F.S.Familia);
            }
            else
            {
                sBruto = (decimal)(F.S.Base + F.S.Familia);
            }
            sbaseDias = sBruto / diasM * totalDias;
            decimal inss = sbaseDias * F.S.INSS / 100;
            decimal sBrutuDias = 0;
            foreach (var bonus in F.D)
            {
                if (bonus.Valor < 0)
                {
                    sBrutuDias = sBrutuDias - (bonus.Valor);
                }
            }
            sBrutuDias = Math.Round((sBrutuDias + sBruto) / diasM * totalDias, 2);

            decimal liquido = sBrutuDias - (AplicarDesconto(F, sbaseDias) + inss);

            return (Math.Round(liquido, 2));
        }

        private decimal FeriasTotal(FolhaSalario F, DateTime D)
        {
            if (F.F == null)
            {
                return 0;
            }
            var trienio = DateTime.Now.Subtract(F.Funcionario.Admissao);
            decimal sBruto;
            decimal tFerias;
            decimal inss;
            var a = (int)(trienio.TotalDays / 365);

            if (a >= 3)
            {
                decimal d = a / 3;

                sBruto = (decimal)(F.S.Base + (F.S.Base * (Math.Floor(d) * 3) / 100));
            }
            else
            {
                sBruto = F.S.Base;
            }

            sBruto = sBruto / 30 * F.F.Dias;
            tFerias = sBruto * (Decimal)(33.33) / 100;
            sBruto = sBruto + tFerias;
            inss = sBruto * F.S.INSS / 100;
            foreach (var bonus in F.D)
            {
                if (bonus.Valor < 0)
                {
                    sBruto = sBruto - (bonus.Valor);
                }
            }
            sBruto = sBruto - inss - AplicarDesconto(F, sBruto);

            return Math.Round(sBruto, 2);
        }
    }
}