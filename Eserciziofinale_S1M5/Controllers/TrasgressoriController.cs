using Eserciziofinale_S1M5.Models; 
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eserciziofinale_S1M5.Controllers
{
    public class TrasgressoriController : Controller
    {
        // Metodo per ottenere la stringa di connessione dal file di configurazione
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Verbali"].ConnectionString;
        }

        // Metodo per ottenere un trasgressore dal database in base all'ID
        private Trasgressori GetTrasgressoreById(int IdAnagrafica)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM Anagrafica WHERE IdAnagrafica = @IdAnagrafica";

                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@IdAnagrafica", IdAnagrafica);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Trasgressori trasgressore = new Trasgressori
                            {
                                IdAnagrafica = (int)reader["IdAnagrafica"],
                                Cognome = reader["Cognome"].ToString(),
                                Nome = reader["Nome"].ToString(),
                                Indirizzo = reader["Indirizzo"].ToString(),
                                Citta = reader["Citta"].ToString(),
                                Cap = reader["Cap"].ToString(),
                                CodiceFiscale = reader["CodiceFiscale"].ToString()
                            };
                            return trasgressore;
                        }
                        return null; // Restituisce null se non trova un trasgressore con quell'ID
                    }
                }
            }
        }

        // Metodo per visualizzare la lista dei trasgressori
        [HttpGet]
        public ActionResult ListaTrasgressori()
        {
            List<Trasgressori> trasgressori = new List<Trasgressori>();

            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                string query = "SELECT * FROM Anagrafica";

                // Esegue la query per ottenere tutti i trasgressori
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Trasgressori trasgressore = new Trasgressori
                            {
                                IdAnagrafica = (int)reader["IdAnagrafica"],
                                Cognome = reader["Cognome"].ToString(),
                                Nome = reader["Nome"].ToString(),
                                Indirizzo = reader["Indirizzo"].ToString(),
                                Citta = reader["Citta"].ToString(),
                                Cap = reader["Cap"].ToString(),
                                CodiceFiscale = reader["CodiceFiscale"].ToString()
                            };

                            trasgressori.Add(trasgressore);
                        }
                    }
                }

            }
            return View(trasgressori); // Restituisce la lista dei trasgressori alla vista
        }

        // Metodo per visualizzare il form per l'aggiunta di un trasgressore
        [HttpGet]
        public ActionResult AggiungiTrasgressore()
        {
            return View();
        }

        // Metodo per aggiungere un trasgressore al database
        [HttpPost]
        public ActionResult AggiungiTrasgressore(Trasgressori model)
        {
            if (ModelState.IsValid) 
            {
                string query = "INSERT INTO Anagrafica (Cognome, Nome, Indirizzo, Citta, Cap, CodiceFiscale)" + "VALUES (@Cognome, @Nome, @Indirizzo, @Citta, @Cap, @CodiceFiscale)";

                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@Cognome", model.Cognome);
                        cmd.Parameters.AddWithValue("@Nome", model.Nome);
                        cmd.Parameters.AddWithValue("@Indirizzo", model.Indirizzo);
                        cmd.Parameters.AddWithValue("@Citta", model.Citta);
                        cmd.Parameters.AddWithValue("@Cap", model.Cap);
                        cmd.Parameters.AddWithValue("@CodiceFiscale", model.CodiceFiscale);
                        cmd.ExecuteNonQuery(); 
                    }
                }
                TempData["Messaggio"] = "Trasgressore aggiunto con successo!";
                return RedirectToAction("ListaTrasgressori"); 
            }
            TempData["Errore"] = "Il modello non è valido. Correggi gli errori e riprova.";
            return View(model); // Se il modello non è valido, restituisce il modello alla vista
        }

        // Metodo per eliminare un trasgressore dal database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminaTrasgressore(int IdAnagrafica)
        {
            Trasgressori trasgressoreDaEliminare = GetTrasgressoreById(IdAnagrafica);

            if (trasgressoreDaEliminare == null)
            {
                TempData["Errore"] = "Trasgressore non trovato!";
            }
            else
            {
                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();
                    string query = "DELETE FROM Anagrafica WHERE IdAnagrafica = @IdAnagrafica";

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@IdAnagrafica", IdAnagrafica);
                        cmd.ExecuteNonQuery(); 
                    }
                }
                TempData["Messaggio"] = "Trasgressore eliminato con successo!";
            }
            return RedirectToAction("ListaTrasgressori"); 
        }

        // Metodo per visualizzare il form per la modifica di un trasgressore
        [HttpGet]
        public ActionResult ModificaTrasgressore(int IdAnagrafica)
        {
            Trasgressori trasgressoreDaModificare = GetTrasgressoreById(IdAnagrafica);

            if (trasgressoreDaModificare == null)
            {
                TempData["Errore"] = "Trasgressore non trovato!";
            }
            return View(trasgressoreDaModificare); // Restituisce il trasgressore da modificare alla vista
        }

        // Metodo per modificare un trasgressore nel database
        [HttpPost]
        public ActionResult ModificaTrasgressore(Trasgressori trasgressoreModificato)
        {
            if (ModelState.IsValid) 
            {
                string query = "UPDATE Anagrafica SET " +
                    "Cognome = @Cognome, " +
                    "Nome = @Nome, " +
                    "Indirizzo = @Indirizzo, " +
                    "Citta = @Citta, " +
                    "Cap = @Cap, " +
                    "Codice = @Codice " +
                    "WHERE IdAnagrafica = @IdAnagrafica";

                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@IdAnagrafica", trasgressoreModificato.IdAnagrafica);
                        cmd.Parameters.AddWithValue("@Cognome", trasgressoreModificato.Cognome);
                        cmd.Parameters.AddWithValue("@Nome", trasgressoreModificato.Nome);
                        cmd.Parameters.AddWithValue("@Indirizzo", trasgressoreModificato.Indirizzo);
                        cmd.Parameters.AddWithValue("@Citta", trasgressoreModificato.Citta);
                        cmd.Parameters.AddWithValue("@Cap", trasgressoreModificato.Cap);
                        cmd.Parameters.AddWithValue("@CodiceFiscale", trasgressoreModificato.CodiceFiscale);
                        cmd.ExecuteNonQuery(); 
                    }
                }
                TempData["Messaggio"] = "Trasgressore modificato con successo!";
            }
            return RedirectToAction("ListaTrasgressori"); 
        }
    }
}
