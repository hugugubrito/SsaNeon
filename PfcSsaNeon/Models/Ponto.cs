using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PfcSsaNeon.Models
{
    public class Ponto
    {
        public int PontoId { get; set; }      
        [DataType(DataType.Date)]
        public DateTime DataPonto { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EntradaManha { get; set; }
        [DataType(DataType.Date)]
        public DateTime? SaidaManha { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EntradaTarde { get; set; }
        [DataType(DataType.Date)]
        public DateTime? SaidaTarde { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EntradaExtra { get; set; }
        [DataType(DataType.Date)]
        public DateTime? SaidaExtra { get; set; }
        public bool Abonar { get; set; }
        public int FuncionarioId { get; set; }
        public virtual Funcionario Funcionarios { get; set; }
    }
}