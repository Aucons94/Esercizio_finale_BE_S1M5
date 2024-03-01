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
    public class ViolazioniController : Controller
    {
        // Metodo per ottenere la stringa di connessione dal file di configurazione
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Verbali"].ConnectionString;
        }

        // Metodo per ottenere una violazione dal database in base all'ID
        private Violazioni GetViolazioneById(int IdViolazione)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM TipoViolazione WHERE IdViolazione = @IdViolazione";

                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@IdViolazione", IdViolazione);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Violazioni violazione = new Violazioni
                            {
                                IdViolazione = (int)reader["IdViolazione"],
                                Descrizione = reader["Descrizione"].ToString(),
                                Contestabile = (bool)reader["Contestabile"],
                            };
                            return violazione;
                        }
                        return null;
                    }
                }
            }
        }

        // Metodo per visualizzare la lista delle violazioni
        [HttpGet]
        public ActionResult ListaViolazioni()
        {
            List<Violazioni> violazioni = new List<Violazioni>();

            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                string query = "SELECT * FROM TipoViolazione";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Violazioni violazione = new Violazioni
                            {
                                IdViolazione = (int)reader["IdViolazione"],
                                Descrizione = reader["Descrizione"].ToString(),
                                Contestabile = (bool)reader["Contestabile"],
                            };

                            violazioni.Add(violazione);
                        }
                    }
                }
            }
            return View(violazioni);
        }

        // Metodo per visualizzare il form per aggiungere una nuova violazione
        [HttpGet]
        public ActionResult AggiungiViolazione()
        {
            return View();
        }

        // Metodo per aggiungere una nuova violazione al database
        [HttpPost]
        public ActionResult AggiungiViolazione(Violazioni model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(model.Descrizione))
                    {
                        string query = "INSERT INTO TipoViolazione (Descrizione, Contestabile) VALUES (@Descrizione, @Contestabile)";
                        using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                        {
                            sqlConnection.Open();
                            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                            {
                                cmd.Parameters.AddWithValue("@Descrizione", model.Descrizione);
                                cmd.Parameters.AddWithValue("@Contestabile", model.Contestabile); // Utilizza direttamente il valore del modello

                                cmd.ExecuteNonQuery();
                            }
                        }
                        TempData["Messaggio"] = "Violazione aggiunta con successo!";
                        return RedirectToAction("ListaViolazioni");
                    }
                    else
                    {
                        TempData["Errore"] = "La descrizione è richiesta.";
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    TempData["Errore"] = "Si è verificato un errore durante l'aggiunta della violazione: " + ex.Message;
                    return View(model);
                }
            }
            TempData["Errore"] = "Il modello non è valido. Correggi gli errori e riprova.";
            return View(model);
        }

        // Metodo per eliminare una violazione dal database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminaViolazione(int IdViolazione)
        {
            Violazioni violazioneDaEliminare = GetViolazioneById(IdViolazione);

            if (violazioneDaEliminare == null)
            {
                TempData["Errore"] = "Violazione non trovata!";
            }
            else
            {
                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();
                    string query = "DELETE FROM TipoViolazione WHERE IdViolazione = @IdViolazione";

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@IdViolazione", IdViolazione);
                        cmd.ExecuteNonQuery();
                    }
                }
                TempData["Messaggio"] = "Violazione eliminata con successo!";
            }
            return RedirectToAction("ListaViolazioni");
        }

        // Metodo per visualizzare il form per modificare una violazione
        [HttpGet]
        public ActionResult ModificaViolazione(int IdViolazione)
        {
            Violazioni violazioneDaModificare = GetViolazioneById(IdViolazione);

            if (violazioneDaModificare == null)
            {
                TempData["Errore"] = "Violazione non trovata!";
            }
            return View(violazioneDaModificare);
        }

        // Metodo per modificare una violazione nel database
        [HttpPost]
        public ActionResult ModificaViolazione(Violazioni violazioneModificata)
        {
            if (ModelState.IsValid)
            {
                string query =
                    "UPDATE TipoViolazione SET " +
                    "Descrizione = @Descrizione, " + 
                    "Contestabile = @Contestabile " +
                    "WHERE IdViolazione = @IdViolazione";

                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@IdViolazione", violazioneModificata.IdViolazione);
                        cmd.Parameters.AddWithValue("@Descrizione", violazioneModificata.Descrizione);
                        cmd.Parameters.AddWithValue("@Contestabile", violazioneModificata.Contestabile); 

                        cmd.ExecuteNonQuery();
                    }
                }
                TempData["Messaggio"] = "Violazione modificata con successo!";
            }
            return RedirectToAction("ListaViolazioni");
        }

    }
}
