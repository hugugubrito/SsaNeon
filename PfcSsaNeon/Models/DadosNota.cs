using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PfcSsaNeon.Models
{
    public enum Status { PENDENTE, PAGO, VENCIDO, CANCELADO }
    public class DadosNota
    {
        public int DadosNotaId { get; set; }
        [Required]
        public string Cliente { get; set; }
        [MaxLength(14, ErrorMessage = "VOCÊ ULTRAPASSOU O LIMITE MAXIMO DE 14 CARACTERES")]
        [MinLength(14, ErrorMessage = "DIGITE O CNPJ COMPLETO")]
        public string CNPJ { get; set; }
        [Required]
        public string NumeroNf { get; set; }
        [DataType(DataType.Date)]
        public DateTime Emissao { get; set; }
        public double Valor { get; set; }
        [DataType(DataType.Date)]
        public DateTime Vencimento { get; set; }

        public string Observacao { get; set; }

        public Status Status { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Pagamento { get; set; }
        public string Cobranca { get; set; }
        

        public ICollection<ArquivoNota> ArquivoNotas { get; set; }
    }
}