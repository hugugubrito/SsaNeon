using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PfcSsaNeon.Models
{
    public class Desconto
    {
        public int DescontoId { get; set; }
        public bool Percento { get; set; }
        public decimal Valor { get; set; }
        [Required]
        public string Descricao { get; set; }
        [DataType(DataType.Date)]
        public System.DateTime Data { get; set; }
        public int SalarioId { get; set; }
        public virtual Salario Salarios { get; set; }
    }
}