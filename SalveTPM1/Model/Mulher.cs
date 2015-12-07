using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SalveTPM1.Model
{
    
    [SQLite.Table("Mulher")]
    class Mulher
    {
        [SQLite.Column("ID")]
        [SQLite.AutoIncrement]
        [SQLite.PrimaryKey]
        public int? ID { get; set; }

        [SQLite.Column("nome")]
        public String nome { get; set; }

        [SQLite.Column("dataUltimaMestruacao")]
        public DateTime dataUltimaMestruacao { get; set; }

        [SQLite.Column("ligacao")]
        public String ligacao { get; set; }

        [SQLite.Column("isPrincipal")]
        public Boolean isPrincipal { get; set; }

        public Mulher() { }


        public String dataUltimaMestruacaoFormatado
        {
            get
            {
                if (dataUltimaMestruacao != null)
                {
                    return dataUltimaMestruacao.ToString("dd/MM/yyyy");
                }

                return null;
            }            
        }

        public String dataInicioTpm
        {
            get
            {
                if (dataUltimaMestruacao != null)
                {
                    DateTime dataInicioTpm = dataUltimaMestruacao.AddDays(21);
                    return dataInicioTpm.ToString("dd/MM/yyyy");
                }
                return null;
            }  
        }

        public String dataFimTpm
        {
            get
            {
                if (dataUltimaMestruacao != null)
                {
                    DateTime dataFimTpm = dataUltimaMestruacao.AddDays(29);
                    return dataFimTpm.ToString("dd/MM/yyyy");
                }
                return null;
            }
        }

        public String dataAproximadaMestruacao
        {
            get
            {
                if (dataUltimaMestruacao != null)
                {
                    DateTime dataMestruacao = dataUltimaMestruacao.AddDays(28);
                    return dataMestruacao.ToString("dd/MM/yyyy");
                }
                return null;
            }
        }

        public DateTime? dataTimeAproximadaMestruacao
        {
            get
            {
                if (dataUltimaMestruacao != null)
                {
                    DateTime dataMestruacao = dataUltimaMestruacao.AddDays(28);
                    return dataMestruacao;
                }
                return null;
            }
        }


        public String dataMaisFertil
        {
            get
            {
                if (dataUltimaMestruacao != null)
                {
                    DateTime dataMaisFertil = dataUltimaMestruacao.AddDays(14);
                    return dataMaisFertil.ToString("dd/MM/yyyy");
                }
                return null;
            }
        }


        public String dataInicioPeriodoFertil
        {
            get
            {
                if (dataUltimaMestruacao != null)
                {
                    DateTime dataMaisFertil = dataUltimaMestruacao.AddDays(14);
                    DateTime dataInicioFertil = dataMaisFertil.AddDays(-3);
                    return dataInicioFertil.ToString("dd/MM/yyyy");
                }
                return null;
            }
        }

         public String dataFimPeriodoFertil
         {
             get
                {
                    if (dataUltimaMestruacao != null)
                    {
                        DateTime dataMaisFertil = dataUltimaMestruacao.AddDays(14);
                        DateTime dataFimFertil = dataMaisFertil.AddDays(3);
                        return dataFimFertil.ToString("dd/MM/yyyy");
                    }
                    return null;
                }
        }


         public Double porcentagemTpm
         {
             get
             {
                 if (dataUltimaMestruacao != null)
                 {
                     DateTime dataAtual = DateTime.Now;
                     DateTime dataInicio =  dataUltimaMestruacao.AddDays(21);
                     DateTime dataFim = dataUltimaMestruacao.AddDays(29);
                     DateTime proximaMestruacao = dataUltimaMestruacao.AddDays(28);

                     if (dataAtual.Ticks >= dataInicio.Ticks && dataAtual.Ticks <= dataFim.Ticks)
                     {

                         TimeSpan diferencaTotal = dataAtual - dataInicio;

                         int diferencaDias = diferencaTotal.Days;

                         return (diferencaDias * 100) / 7;
                     }
                     else
                     {
                         return 0;
                     }

                 }
                 return 0;
             }
         }


         public String nomeMulherFormatado
         {
             get
             {
                 return ligacao + ": "+ nome;
             }
         }


         public String caminhoImagemTpm
         {
             get
             {
                 if(porcentagemTpm > 0 && porcentagemTpm <= 25){
                     return "ms-appx:///Assets/velocimetroTpm1.png";
                 }else if(porcentagemTpm > 25 && porcentagemTpm <= 50){
                     return "ms-appx:///Assets/velocimetroTpm2.png";
                 }else if (porcentagemTpm > 50 && porcentagemTpm <= 75)
                 {
                     return "ms-appx:///Assets/velocimetroTpm3.png";
                 }
                 else if (porcentagemTpm > 75 )
                 {
                     return "ms-appx:///Assets/velocimetroTpm4.png";
                 }
                 else
                 {
                     return "ms-appx:///Assets/velocimetroSemTpm.png";
                 }
             }
         }

         public String caminhoImagemTpmPequeno
         {
             get
             {
                 if (porcentagemTpm > 0 && porcentagemTpm <= 25)
                 {
                     return "ms-appx:///Assets/velocimetroTpm1Pequeno.png";
                 }
                 else if (porcentagemTpm > 25 && porcentagemTpm <= 50)
                 {
                     return "ms-appx:///Assets/velocimetroTpm2Pequeno.png";
                 }
                 else if (porcentagemTpm > 50 && porcentagemTpm <= 75)
                 {
                     return "ms-appx:///Assets/velocimetroTpm3Pequeno.png";
                 }
                 else if (porcentagemTpm > 75)
                 {
                     return "ms-appx:///Assets/velocimetroTpm4Pequeno.png";
                 }
                 else
                 {
                     return "ms-appx:///Assets/velocimetroSemTpmPequeno.png";
                 }
             }
         }

         public String caminhoImagemFraseTpm
         {
             get
             {
                 if (porcentagemTpm > 0 && porcentagemTpm <= 25)
                 {
                     return "ms-appx:///Assets/fraseInicioTpm.png";
                 }
                 else if (porcentagemTpm > 25 && porcentagemTpm <= 50)
                 {
                     return "ms-appx:///Assets/fraseMeioTpm.png";
                 }
                 else if (porcentagemTpm > 50 && porcentagemTpm <= 75)
                 {
                     return "ms-appx:///Assets/fraseQuaseFimTpm.png";
                 }
                 else if (porcentagemTpm > 75)
                 {
                     return "ms-appx:///Assets/fraseFimTpm.png";
                 }
                 else
                 {
                     return "ms-appx:///Assets/fraseSemTpm.png";
                 }
             }
         }
      
    }
}
