using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Eserciziofinale_S1M5.Models
{
    public class Verbale
    {
        public int IdVerbale { get; set; }

        [Required(ErrorMessage = "La data della violazione è obbligatoria.")]
        [DataType(DataType.Date)]
        public DateTime DataViolazione { get; set; }

        [Required(ErrorMessage = "L'indirizzo della violazione è obbligatorio.")]
        public string IndirizzoViolazione { get; set; }

        [Required(ErrorMessage = "Il nominativo dell'agente è obbligatorio.")]
        public string NominativoAgente { get; set; }

        [Required(ErrorMessage = "La data di trascrizione del verbale è obbligatoria.")]
        [DataType(DataType.Date)]
        public DateTime DataTrascrizioneVerbale { get; set; }

        [Required(ErrorMessage = "L'importo è obbligatorio.")]
        public decimal Importo { get; set; }

        [Required(ErrorMessage = "Il numero di punti da decurtare è obbligatorio.")]
        public int DecurtamentoPunti { get; set; }

        public int IdAnagrafica { get; set; }
        public int IdViolazione { get; set; }
    }
}