using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PfcSsaNeon.Models
{
    public class ArquivoNota
    {
        public int ArquivoNotaId { get; set; }
        public string NomeArquivo { get; set; }
        public string CaminhoArquivo { get; set; }
        public int DadosNotaId { get; set; }
        public DadosNota DadosNota { get; set; }
    }
}