using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PfcSsaNeon.Models
{
    public class Salario
    {
        public int SalarioId { get; set; }       
        public decimal Base { get; set; }     
        public decimal INSS { get; set; }
        public decimal? Familia { get; set; }
        public int FuncionarioId { get; set; }
        public virtual Funcionario Funcionarios { get; set; }
        public virtual ICollection<Desconto>Descontos { get; set; }
    }
}