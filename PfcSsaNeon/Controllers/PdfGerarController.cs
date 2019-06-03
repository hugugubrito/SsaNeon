using Rotativa;
using PfcSsaNeon.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SsaNeon.Helper;

namespace SsaNeon.Controllers
{
    public class PdfGerarController : Controller
    {
        // GET: PdfGerar
        public ActionResult FichaDeControl()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FichaDeControl(int? NumeroOp, string Nome, string END, string Local, string Telefone, string Atendimento)
        {
            FichaDeControle Ficha = new FichaDeControle();

            Ficha.NumeroOp = NumeroOp;
            Ficha.Nome = Nome;
            Ficha.END = END;
            Ficha.Local = Local;
            Ficha.Telefone = Telefone;
            Ficha.Atendimento = Atendimento;

            ViewBag.Ficha = Ficha;
            var pdf = new ViewAsPdf
            {
                ViewName = "FichaDeControlePdf",
                IsGrayScale = false,
                PageSize = Rotativa.Options.Size.A4,
                Model = Ficha
            };
            return pdf;
        }
        public ActionResult OrdemDeProducao()
        {
            return View();
        }
        [HttpPost]
        public ActionResult OrdemDeProducao(int? Nservico, string Data, string Entrega, string Hora, string Nfc, string Razaosocial, string End, string Cidade, string Email, string Cnpj, string Bairro, string Tel, string Cel, string Uf, string Cep, string Fax, string Instalacao, string LocalDaInstalacao, string EndInst, string Voltagem, string Item, int? Quantidade, string Engenho, string DescricaoEngenho, string ValorServico, string ValorVisita, string Bv, string Comissao, string FormaPagamento, string Observacoes, string Atendimento)
        {
            OP op = new OP()
            {
                Nservico = Nservico,
                Data = Data,
                Entrega = Entrega,
                Hora = Hora,
                Nfc = Nfc,
                Razaosocial = Razaosocial,
                End = End,
                Cidade = Cidade,
                Email = Email,
                Cnpj = Cnpj,
                Bairro = Bairro,
                Tel = Tel,
                Cel = Cel,
                Uf = Uf,
                Cep = Cep,
                Fax = Fax,
                Instalacao = Instalacao,
                LocalDaInstalacao = LocalDaInstalacao,
                EndInst = EndInst,
                Voltagem = Voltagem,
                Item = Item,
                Quantidade = Quantidade,
                Engenho = Engenho,
                DescricaoEngenho = DescricaoEngenho,
                ValorServico = ValorServico,
                ValorVisita = ValorVisita,
                Bv = Bv,
                Comissao = Comissao,
                FormaPagamento = FormaPagamento,
                Observacoes = Observacoes,
                Atendimento = Atendimento
            };

            ViewBag.Op = op;
            var pdf = new ViewAsPdf
            {
                ViewName = "OrdemDeProducaoPdf",
                IsGrayScale = false,
                PageSize = Rotativa.Options.Size.A4,
                Model = op
            };
            return pdf;
        }
    }
}