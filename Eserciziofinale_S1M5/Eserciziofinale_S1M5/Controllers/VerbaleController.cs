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
    public class VerbaleController : Controller
    {
        // Variabili per memorizzare le liste di violazioni e trasgressori
        private List<Violazioni> listaViolazioni;
        private List<Trasgressori> listaTrasgressori;

        // Metodo per ottenere la stringa di connessione dal file di configurazione
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Verbali"].ConnectionString;
        }

        // Metodo per ottenere un verbale dal database in base all'ID
        private Verbale GetVerbaleById(int IdVerbale)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM Verbale WHERE IdVerbale = @IdVerbale";

                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@IdVerbale", IdVerbale);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Verbale verbale = new Verbale
                            {
                                IdVerbale = (int)reader["IdVerbale"],
                                DataViolazione = (DateTime)reader["DataViolazione"],
                                IndirizzoViolazione = reader["IndirizzoViolazione"].ToString(),
                                NominativoAgente = reader["NominativoAgente"].ToString(),
                                DataTrascrizioneVerbale = (DateTime)reader["DataTrascrizioneVerbale"],
                                Importo = (decimal)reader["Importo"],
                                DecurtamentoPunti = reader.IsDBNull(reader.GetOrdinal("DecurtamentoPunti")) ? 0 : (int)reader["DecurtamentoPunti"],
                                IdAnagrafica = (int)reader["IdAnagrafica"],
                                IdViolazione = (int)reader["IdViolazione"]
                            };
                            return verbale;
                        }
                        return null;
                    }
                }
            }
        }

        // Metodo per ottenere la lista delle violazioni dal database
        [HttpGet]
        public List<Violazioni> ListaViolazioni()
        {
            if (listaViolazioni == null)
            {
                listaViolazioni = new List<Violazioni>();

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
                                };

                                listaViolazioni.Add(violazione); // Aggiunge la violazione alla lista
                            }
                        }
                    }
                }
            }
            return listaViolazioni; // Restituisce la lista delle violazioni
        }

        // Metodo per ottenere la lista dei trasgressori dal database
        [HttpGet]
        public List<Trasgressori> ListaTrasgressori()
        {
            if (listaTrasgressori == null)
            {
                listaTrasgressori = new List<Trasgressori>();

                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "SELECT * FROM Anagrafica";

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

                                listaTrasgressori.Add(trasgressore); // Aggiunge il trasgressore alla lista
                            }
                        }
                    }
                }
            }
            return listaTrasgressori; // Restituisce la lista dei trasgressori
        }

        // Metodo per visualizzare la lista dei verbali
        [HttpGet]
        public ActionResult ListaVerbali()
        {
            List<Verbale> verbali = new List<Verbale>();

            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                string query = "SELECT * FROM Verbale";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Verbale verbale = new Verbale
                            {
                                IdVerbale = (int)reader["IdVerbale"],
                                DataViolazione = (DateTime)reader["DataViolazione"],
                                IndirizzoViolazione = reader["IndirizzoViolazione"].ToString(),
                                NominativoAgente = reader["NominativoAgente"].ToString(),
                                DataTrascrizioneVerbale = (DateTime)reader["DataTrascrizioneVerbale"],
                                Importo = (decimal)reader["Importo"],
                                DecurtamentoPunti = reader.IsDBNull(reader.GetOrdinal("DecurtamentoPunti")) ? 0 : (int)reader["DecurtamentoPunti"],
                                IdAnagrafica = (int)reader["IdAnagrafica"],
                                IdViolazione = (int)reader["IdViolazione"],
                            };

                            verbali.Add(verbale); // Aggiunge il verbale alla lista
                        }
                    }
                }
            }
            return View(verbali); // Restituisce la vista con la lista dei verbali
        }

        // Metodo per visualizzare il form per aggiungere un nuovo verbale
        [HttpGet]
        public ActionResult AggiungiVerbale()
        {
            return View();
        }

        // Metodo per aggiungere un nuovo verbale al database
        [HttpPost]
        public ActionResult AggiungiVerbale(Verbale model, string Descrizione)
        {
            if (ModelState.IsValid)
            {
                int idViolazione = GetIdViolazioneFromDescrizione(Descrizione);

                if (idViolazione != -1)
                {
                    string query = "INSERT INTO Verbale (DataViolazione, IndirizzoViolazione, NominativoAgente, DataTrascrizioneVerbale, Importo, DecurtamentoPunti, IdAnagrafica, IdViolazione)" + "VALUES (@DataViolazione, @IndirizzoViolazione, @NominativoAgente, @DataTrascrizioneVerbale, @Importo, @DecurtamentoPunti, @IdAnagrafica, @IdViolazione)";

                    using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                    {
                        sqlConnection.Open();
                        using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                        {
                            cmd.Parameters.AddWithValue("@DataViolazione", model.DataViolazione);
                            cmd.Parameters.AddWithValue("@IndirizzoViolazione", model.IndirizzoViolazione);
                            cmd.Parameters.AddWithValue("@NominativoAgente", model.NominativoAgente);
                            cmd.Parameters.AddWithValue("@DataTrascrizioneVerbale", model.DataTrascrizioneVerbale);
                            cmd.Parameters.AddWithValue("@Importo", model.Importo);
                            cmd.Parameters.AddWithValue("@DecurtamentoPunti", model.DecurtamentoPunti);
                            cmd.Parameters.AddWithValue("@IdAnagrafica", model.IdAnagrafica);
                            cmd.Parameters.AddWithValue("@IdViolazione", idViolazione);
                            cmd.ExecuteNonQuery(); 
                        }
                    }
                    TempData["Messaggio"] = "Verbale aggiunto con successo!";
                    return RedirectToAction("ListaVerbali"); 
                }
                else
                {
                    TempData["Errore"] = "La descrizione della violazione non è valida.";
                }
            }
            TempData["Errore"] = "Il modello non è valido. Correggi gli errori e riprova.";
            return View(model); // Se il modello non è valido, restituisce il modello alla vista
        }

        // Metodo per ottenere l'ID della violazione dato il suo nome
        private int GetIdViolazioneFromDescrizione(string descrizione)
        {
            int idViolazione = -1; // Inizializza l'ID della violazione a -1
            string query = "SELECT IdViolazione FROM TipoViolazione WHERE Descrizione = @Descrizione";

            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@Descrizione", descrizione);
                    object result = cmd.ExecuteScalar(); 

                    if (result != null)
                    {
                        idViolazione = Convert.ToInt32(result); 
                    }
                }
            }

            return idViolazione; // Restituisce l'ID della violazione
        }

        // Metodo per eliminare un verbale dal database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminaVerbale(int IdVerbale)
        {
            Verbale verbaleDaEliminare = GetVerbaleById(IdVerbale);

            if (verbaleDaEliminare == null)
            {
                TempData["Errore"] = "Violazione non trovata!";
            }
            else
            {
                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();
                    string query = "DELETE FROM Verbale WHERE IdVerbale = @IdVerbale";

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@IdVerbale", IdVerbale);
                        cmd.ExecuteNonQuery(); 
                    }
                }
                TempData["Messaggio"] = "Verbale eliminato con successo!";
            }
            return RedirectToAction("ListaVerbali"); 
        }

        // Metodo per visualizzare il form per modificare un verbale
        [HttpGet]
        public ActionResult ModificaVerbale(int IdVerbale)
        {
            Verbale verbaleDaModificare = GetVerbaleById(IdVerbale);

            if (verbaleDaModificare == null)
            {
                TempData["Errore"] = "Verbale non trovato!";
            }

            return View(verbaleDaModificare); // Restituisce la vista con il verbale da modificare
        }

        // Metodo per modificare un verbale nel database
        [HttpPost]
        public ActionResult ModificaVerbale(Verbale verbaleModificato)
        {
            if (ModelState.IsValid)
            {
                string query =
                    "UPDATE Verbale SET " +
                    "DataViolazione = @DataViolazione, " +
                    "IndirizzoViolazione = @IndirizzoViolazione, " +
                    "NominativoAgente = @NominativoAgente, " +
                    "DataTrascrizioneVerbale = @DataTrascrizioneVerbale, " +
                    "Importo = @Importo, " +
                    "DecurtamentoPunti = @DecurtamentoPunti " +
                    "WHERE IdVerbale = @IdVerbale";

                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@IdVerbale", verbaleModificato.IdVerbale);
                        cmd.Parameters.AddWithValue("@DataViolazione", verbaleModificato.DataViolazione);
                        cmd.Parameters.AddWithValue("@IndirizzoViolazione", verbaleModificato.IndirizzoViolazione);
                        cmd.Parameters.AddWithValue("@NominativoAgente", verbaleModificato.NominativoAgente);
                        cmd.Parameters.AddWithValue("@DataTrascrizioneVerbale", verbaleModificato.DataTrascrizioneVerbale);
                        cmd.Parameters.AddWithValue("@Importo", verbaleModificato.Importo);
                        cmd.Parameters.AddWithValue("@DecurtamentoPunti", verbaleModificato.DecurtamentoPunti);
                        cmd.ExecuteNonQuery(); 
                    }
                }
                TempData["Messaggio"] = "Verbale modificato con successo!";
            }
            return RedirectToAction("ListaVerbali"); 
        }
    }
}
