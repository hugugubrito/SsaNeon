using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PfcSsaNeon.Models
{
    public class DadosBancario
    {
        public int DadosBancarioId { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "VOCÊ ULTRAPASSOU O LIMITE MAXIMO DE 30 CARACTERES")]
        public string Banco { get; set; }
        public int AG { get; set; }
        [Required]
        [MaxLength(15, ErrorMessage = "VOCÊ ULTRAPASSOU O LIMITE MAXIMO DE 15 CARACTERES")]
        public string Conta { get; set; }
        public int FuncionarioId { get; set; }
        public virtual Funcionario Funcionarios { get; set; }
    }
}