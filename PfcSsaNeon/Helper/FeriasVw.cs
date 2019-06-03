using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PfcSsaNeon.Helper
{
    public class FeriasVw
    {
        public int FuncionarioId { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Cargo { get; set; }
        public System.DateTime InicioFerias { get; set; }
        public System.DateTime FimFerias { get; set; }

        public FeriasVw(int funcionarioId,string nome, string cPF, string cargo, int Dias=0 ,DateTime inicioFerias= new DateTime())
        {
            FuncionarioId = funcionarioId;
            Nome = nome;        
            CPF = cPF;
            Cargo = cargo;
            InicioFerias = inicioFerias;
            FimFerias = inicioFerias.AddDays(Dias);
            if (FimFerias == InicioFerias)
            {
                InicioFerias= new DateTime();
            }
        }
    }
}