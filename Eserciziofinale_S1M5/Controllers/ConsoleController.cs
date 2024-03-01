using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;

namespace Eserciziofinale_S1M5.Controllers
{
    public class ConsoleController : Controller
    {
        // Metodo per ottenere la stringa di connessione dal file di configurazione
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Verbali"].ConnectionString;
        }

        // Metodo per visualizzare la vista della console
        public ActionResult Console()
        {
            return View();
        }

        // Metodo per ottenere il totale dei verbali per tipo di violazione
        public ActionResult TotaleVerbaliPerTipo()
        {
            SqlConnection connection = new SqlConnection(GetConnectionString());
            connection.Open();

            // Query per ottenere il totale dei verbali per tipo di violazione
            string query = @"SELECT TipoViolazione.descrizione AS TipoViolazione, COUNT(Verbale.idverbale) AS NrVerbaliPerTipo
                    FROM Verbale
                    INNER JOIN TipoViolazione ON Verbale.idviolazione = TipoViolazione.idviolazione
                    GROUP BY TipoViolazione.descrizione;";

            List<dynamic> risultati = new List<dynamic>();
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                dynamic risultato = new ExpandoObject();
                risultato.TipoViolazione = reader["TipoViolazione"];
                risultato.NrVerbaliPerTipo = reader["NrVerbaliPerTipo"];
                risultati.Add(risultato);
            }
            reader.Close();
            connection.Close();

            ViewBag.Risultati = risultati;

            return View();
        }

        // Metodo per ottenere il totale dei punti decurtati per trasgressore
        public ActionResult TotalePuntiDecurtatiPerTrasgressore()
        {
            SqlConnection connection = new SqlConnection(GetConnectionString());
            connection.Open();

            // Query per ottenere il totale dei punti decurtati per trasgressore
            string query = @"SELECT Anagrafica.Cognome, Anagrafica.Nome, SUM(Verbale.DecurtamentoPunti) AS SommaPuntiDecurtati
                    FROM Anagrafica
                    LEFT JOIN Verbale ON Anagrafica.idanagrafica = Verbale.idanagrafica
                    GROUP BY Anagrafica.Cognome, Anagrafica.Nome";
            List<dynamic> risultati = new List<dynamic>();
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                dynamic risultato = new ExpandoObject();
                risultato.Cognome = reader["Cognome"];
                risultato.Nome = reader["Nome"];
                // Controllo se il valore SommaPuntiDecurtati è null e lo imposto a 0 se necessario
                risultato.SommaPuntiDecurtati = reader["SommaPuntiDecurtati"] == DBNull.Value ? 0 : reader["SommaPuntiDecurtati"];
                risultati.Add(risultato);
            }
            reader.Close();
            connection.Close();
            ViewBag.Risultati = risultati;

            return View();
        }

        // Metodo per ottenere i verbali con decurtamento di punti superiore a 10
        public ActionResult VerbaliConDecurtamentoPiuDi10Punti()
        {
            SqlConnection connection = new SqlConnection(GetConnectionString());
            connection.Open();

            // Query per ottenere i verbali con decurtamento di punti superiore a 10
            string query = @"SELECT Anagrafica.Cognome, Anagrafica.Nome, Verbale.IndirizzoViolazione, Verbale.DataViolazione, Verbale.Importo, Verbale.DecurtamentoPunti
                    FROM Anagrafica
                    INNER JOIN Verbale ON Anagrafica.idanagrafica = Verbale.idanagrafica
                    WHERE Verbale.DecurtamentoPunti > 10";
            List<dynamic> risultati = new List<dynamic>();
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                dynamic risultato = new ExpandoObject();
                risultato.Cognome = reader["Cognome"];
                risultato.Nome = reader["Nome"];
                risultato.IndirizzoViolazione = reader["IndirizzoViolazione"];
                risultato.DataViolazione = reader["DataViolazione"];
                risultato.Importo = reader["Importo"];
                risultato.DecurtamentoPunti = reader["DecurtamentoPunti"];
                risultati.Add(risultato);
            }
            reader.Close();
            connection.Close();
            ViewBag.Risultati = risultati;

            return View();
        }

        // Metodo per ottenere i verbali con importo superiore a 400
        public ActionResult VerbaliConImportoSuperioreA400()
        {
            SqlConnection connection = new SqlConnection(GetConnectionString());
            connection.Open();

            // Query per ottenere i verbali con importo superiore a 400
            string query = @"SELECT Anagrafica.Cognome, Anagrafica.Nome, Verbale.IndirizzoViolazione, Verbale.DataViolazione, Verbale.Importo, Verbale.DecurtamentoPunti
                    FROM Anagrafica
                    INNER JOIN Verbale ON Anagrafica.idanagrafica = Verbale.idanagrafica
                    WHERE Verbale.Importo > 400.00";

            List<dynamic> risultati = new List<dynamic>();
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                dynamic risultato = new ExpandoObject();
                risultato.Cognome = reader["Cognome"];
                risultato.Nome = reader["Nome"];
                risultato.IndirizzoViolazione = reader["IndirizzoViolazione"];
                risultato.DataViolazione = reader["DataViolazione"];
                risultato.Importo = reader["Importo"];
                risultato.DecurtamentoPunti = reader["DecurtamentoPunti"];
                risultati.Add(risultato);
            }
            reader.Close();
            connection.Close();
            ViewBag.Risultati = risultati;

            return View();
        }
    }
}
