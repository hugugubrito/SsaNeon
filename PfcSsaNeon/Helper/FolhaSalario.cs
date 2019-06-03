using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PfcSsaNeon.Models;

namespace PfcSsaNeon.Helper
{
    public class FolhaSalario
    {
        public int FuncionarioId { get; set; }
        public string Nome { get; set; }
        public Funcionario Funcionario { get; set; }
        public Decimal Valor { get; set; }
        public string FormaPagamento { get; set; }
        public DadosBancario B { get; set; }
        public Salario S { get; set; }
        public IEnumerable<Desconto> D { get; set; }
        public Ferias F { get; set; }
        public IEnumerable<Ponto> P { get; set; }
        public bool isFerias{ get; set; }
    }
}