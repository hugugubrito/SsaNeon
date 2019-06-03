using PfcSsaNeon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PfcSsaNeon.Helper
{
    public static class Verificar
    {

        public static bool VerificarAcesso(int key, int nivel, ApplicationUser u)
        {
            if (u.Status == Ativo.ATIVO)
            {
                switch (key)
                {
                    case 1:
                        if (u.PRh >= nivel)
                        {
                            return true;
                        }
                        break;
                    case 2:
                        if (u.PFinanceiro >= nivel)
                        {
                            return true;
                        }
                        break;
                    case 3:
                        if (u.PUsuario >= nivel)
                        {
                            return true;
                        }
                        break;
                }
            }
            return false;
        }
    }
}