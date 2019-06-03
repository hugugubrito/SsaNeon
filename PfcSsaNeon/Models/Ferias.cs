using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PfcSsaNeon.Models
{
    public class Ferias
    {
        public int FeriasId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public System.DateTime InicioFerias { get; set; }
        public int Dias { get; set; }
        public int? FuncionarioId { get; set; }
        public virtual Funcionario Funcionarios { get; set; }     
    }
}