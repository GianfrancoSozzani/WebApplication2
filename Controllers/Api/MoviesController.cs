// Importa le classi necessarie per creare controller API
using Microsoft.AspNetCore.Mvc;

// Importa il namespace del progetto per accedere al modello Movie
using WebApplication2.Models;

// Importa le classi necessarie per connettersi a SQL Server
using Microsoft.Data.SqlClient;

// Definisce il namespace del controller, coerente con la struttura del progetto
namespace WebApplication2.Controllers.Api
{
    // Indica che questa classe è un controller API (abilita funzionalità automatiche come la validazione dei dati)
    [ApiController]

    // Definisce la route dell'API: sarà raggiungibile tramite /api/movies
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        // Stringa di connessione a SQL Server. Specifica il nome del server, il database e che si usa l'autenticazione Windows.
        private readonly string _connectionString = "Server=DESKTOP-KM2T7UL\\SQLEXPRESS;Database=prova_api;Trusted_Connection=True;TrustServerCertificate=True;";

        // Metodo che risponde a richieste HTTP GET
        [HttpGet]
        public IActionResult GetMovies()
        {
            // Lista che conterrà i film letti dal database
            var movies = new List<Movie>();

            // Apertura della connessione al database SQL Server
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open(); // Apre la connessione

                // Query SQL per ottenere tutti i film dalla tabella Movies
                string query = "SELECT Id, Title, Year FROM Movies";

                // Crea un comando SQL da eseguire sulla connessione
                using (SqlCommand cmd = new SqlCommand(query, conn))

                // Esegue la query e ottiene un lettore per scorrere i risultati
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Scorre ogni riga restituita dalla query
                    while (reader.Read())
                    {
                        // Crea un oggetto Movie con i dati della riga corrente e lo aggiunge alla lista
                        movies.Add(new Movie
                        {
                            Id = reader.GetInt32(0),        // Colonna 0: Id (int)
                            Title = reader.GetString(1),    // Colonna 1: Title (string)
                            Year = reader.GetInt32(2)       // Colonna 2: Year (int)
                        });
                    }
                }
            }

            // Restituisce la lista di film come risposta HTTP 200 (OK) in formato JSON
            return Ok(movies);
        }

        [HttpGet("{id?}")]
        public IActionResult GetMovieById(int? id)
        {
            var movies = new List<Movie>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query;
                SqlCommand cmd;

                
                    query = "SELECT Id, Title, Year FROM Movies WHERE Id = @Id";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id.Value);
                

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        movies.Add(new Movie
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Year = reader.GetInt32(2)
                        });
                    }
                }
            }

            if (id.HasValue && movies.Count == 0)
                return NotFound(new { message = $"Nessun film trovato con ID {id.Value}" });

            return Ok(movies);
        }
        


    }
}
