using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eserciziofinale_S1M5.Models
{
    public class Trasgressori
    {
        public int IdAnagrafica { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string Indirizzo { get; set; }
        public string Citta { get; set; }
        public string Cap { get; set; }
        public string CodiceFiscale { get; set; }
        public string AnagraficaCompleta => $"{Cognome} {Nome}";
    }
}