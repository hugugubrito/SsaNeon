using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PfcSsaNeon.Controllers
{
    public class ErrosController : Controller
    {
        // GET: Erros
        public ActionResult erroPonto()
        {
            return View();
        }
        public ActionResult erroPermissao()
        {
            return View();
        }
        public ActionResult erroFolha()
        {
            return View();
        }
    }
}