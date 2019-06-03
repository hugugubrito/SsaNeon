using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PfcSsaNeon.Models
{
    public class Funcionario
    {
        public int FuncionarioId { get; set; }

        [Required]
        [MaxLength(25, ErrorMessage = "VOCÊ ULTRAPASSOU O LIMITE MAXIMO DE 25 CARACTERES")]
        public string Nome { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "VOCÊ ULTRAPASSOU O LIMITE MAXIMO DE 30 CARACTERES")]
        public string Sobrenome { get; set; }

        [MaxLength(50, ErrorMessage = "VOCÊ ULTRAPASSOU O LIMITE MAXIMO DE 50 CARACTERES")]
        public string NomePai { get; set; }

        [MaxLength(50, ErrorMessage = "VOCÊ ULTRAPASSOU O LIMITE MAXIMO DE 50 CARACTERES")]
        public string NomeMae { get; set; }

        public int QuantidadeFilhos { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [MaxLength(11, ErrorMessage = "VOCÊ ULTRAPASSOU O LIMITE MAXIMO DE 11 CARACTERES")]
        [MinLength(11, ErrorMessage = "DIGITE O CPF COMPLETO")]
        public string CPF { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [MaxLength(10, ErrorMessage = "VOCÊ ULTRAPASSOU O LIMITE MAXIMO DE 10 CARACTERES")]
        [MinLength(10, ErrorMessage = "DIGITE O RG COMPLETO")]
        public string RG { get; set; }

        [DataType(DataType.Date)]
        public DateTime Nascimento { get; set; }
       
        [MaxLength(11, ErrorMessage = "VOCÊ ULTRAPASSOU O LIMITE MAXIMO DE 11 CARACTERES")]
        [MinLength(11, ErrorMessage = "DIGITE O NUMERO DA CNH COMPLETO")]
        public string CNH { get; set; }

        [Required]        
        [MaxLength(12, ErrorMessage = "VOCÊ ULTRAPASSOU O LIMITE MAXIMO DE 12 CARACTERES")]
        [MinLength(12, ErrorMessage = "DIGITE O NUMERO DO TITULO DE ELEITOR COMPLETO")]
        public string TituloEleitor { get; set; }

        [MaxLength(12, ErrorMessage = "VOCÊ ULTRAPASSOU O LIMITE MAXIMO DE 12 CARACTERES")]
        [MinLength(12, ErrorMessage = "DIGITE O NUMERO DA CERTIDÃO DE ALISTAMENTO MILITAR COMPLETO")]
        public string CertMilitar { get; set; }

        [MaxLength(11, ErrorMessage = "VOCÊ ULTRAPASSOU O LIMITE MAXIMO DE 11 CARACTERES")]
        [MinLength(11, ErrorMessage = "DIGITE O NUMERO DA CNH COMPLETO")]
        public string PIS { get; set; }

        [DataType(DataType.Date)]
        public System.DateTime Admissao { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "VOCÊ ULTRAPASSOU O LIMITE MAXIMO DE 50 CARACTERES")]
        public string Cargo { get; set; }

        [Range(1, 24, ErrorMessage = "DIGITE HORARIO VALIDO")]
        public decimal HorasDia { get; set; }

        [Range(1, 7, ErrorMessage = "INFORME UMNUMERO DE 1 A 7")]
        public int DiasJornada { get; set; }

        [Required]
        [MaxLength(15, ErrorMessage = "VOCÊ ULTRAPASSOU O LIMITE MAXIMO DE 15 CARACTERES")]
        public string EstadoCivil { get; set; }

        [MaxLength(25, ErrorMessage = "VOCÊ ULTRAPASSOU O LIMITE MAXIMO DE 25 CARACTERES")]
        public string NomeConjugue { get; set; }

        [Required]
        [MaxLength(70, ErrorMessage = "VOCÊ ULTRAPASSOU O LIMITE MAXIMO DE 70 CARACTERES")]
        public string Endereco { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "VOCÊ ULTRAPASSOU O LIMITE MAXIMO DE 20 CARACTERES")]
        public string Cidade { get; set; }

        [Required]
        [MaxLength(8, ErrorMessage = "VOCÊ ULTRAPASSOU O LIMITE MAXIMO DE 8 CARACTERES")]
        public string CEP { get; set; }

        public bool Esta_Ativo { get; set; }

        public virtual ICollection <Salario> Salarios { get; set; }

        public virtual ICollection<Ponto> Pontos { get; set; }

        public virtual ICollection<Ferias> Ferias { get; set; }

        public virtual ICollection <DadosBancario> DadosBancarios { get; set; }
    }
}