using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eserciziofinale_S1M5.Models
{
    public class Violazioni
    {
        public int IdViolazione { get; set; }
        public string Descrizione { get; set; }
        public bool Contestabile { get; set; }
    }
}