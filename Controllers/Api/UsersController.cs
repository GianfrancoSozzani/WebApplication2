// Importiamo i namespace necessari
using Microsoft.AspNetCore.Mvc; // Per creare il controller API
using WebApplication2.Models; // Per accedere al modello User
using Microsoft.Data.SqlClient; // Per interagire con SQL Server

// Definizione del namespace del controller
namespace WebApplication2.Controllers.Api
{
    // Attributo che indica che questa classe è un controller API
    [ApiController]

    // Definisce la route base: api/users
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        // Stringa di connessione a SQL Server (personalizzata per il tuo ambiente)
        private readonly string _connectionString = "Server=FRANCESCO\\SQLEXPRESS;Database=prova_api;Trusted_Connection=True;TrustServerCertificate=True;";

        // Metodo che gestisce le richieste POST per aggiungere un utente
        [HttpPost]
        public IActionResult AddUser([FromBody] User user)
        {
            // Apertura della connessione al database usando la stringa di connessione
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open(); // Apriamo la connessione

                // Query SQL parametrizzata per inserire un nuovo utente
                string query = "INSERT INTO Users (Name, Email) VALUES (@Name, @Email)";

                // Creazione del comando SQL da eseguire
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Aggiungiamo i parametri alla query in modo sicuro (contro SQL Injection)
                    cmd.Parameters.AddWithValue("@Name", user.Name);
                    cmd.Parameters.AddWithValue("@Email", user.Email);

                    // Eseguiamo la query senza aspettarci un risultato di ritorno
                    cmd.ExecuteNonQuery();
                }
            }

            // Se tutto è andato bene, restituiamo una risposta 200 OK con un messaggio JSON
            return Ok(new { message = "Utente aggiunto con successo!" });
        }
    }
}
