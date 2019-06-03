using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PfcSsaNeon.Models
{
    public class PfcSsaNeonContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public PfcSsaNeonContext() : base("name=PfcSsaNeonContext")
        {
        }

        public System.Data.Entity.DbSet<PfcSsaNeon.Models.DadosBancario> DadosBancarios { get; set; }

        public System.Data.Entity.DbSet<PfcSsaNeon.Models.Funcionario> Funcionarios { get; set; }

        public System.Data.Entity.DbSet<PfcSsaNeon.Models.Salario> Salarios { get; set; }

        public System.Data.Entity.DbSet<PfcSsaNeon.Models.DadosNota> DadosNotas { get; set; }

        public System.Data.Entity.DbSet<PfcSsaNeon.Models.Ponto> Pontoes { get; set; }

        public System.Data.Entity.DbSet<PfcSsaNeon.Models.Ferias> Ferias { get; set; }

        public System.Data.Entity.DbSet<PfcSsaNeon.Models.ArquivoNota> ArquivoNotas { get; set; }

        public System.Data.Entity.DbSet<PfcSsaNeon.Models.Desconto> Descontoes { get; set; }
    }
}
