using Microsoft.Data.SqlClient;
using PROJECT_ES.Data;

namespace PROJECT_ES.Services;

public class MovieRepository
{
    public static List<Movie> getMovie()
    {
        List<Movie> movies = new List<Movie>();
        using (SqlConnection connection = new SqlConnection("Server=tcp:engesoft.database.windows.net,1433;Initial Catalog=X;Persist Security Info=False;User ID=engsoft;Password=dnaXHaP6W5KV2v4f;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
        {
            string sql = "SELECT * FROM Movie";
            SqlCommand command = new SqlCommand(sql, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Movie movie = new Movie();
                    movie.Id = Convert.ToInt32(reader["Id"]);
                    movie.Description = reader["Description"].ToString();
                    movie.Title = reader["Title"].ToString();
                    movie.Diretor = reader["Diretor"].ToString();
                    movie.Actor = reader["Actor"].ToString();
                    movie.Genre = reader["Genre"].ToString();
                    movie.Writers = reader["Writers"].ToString();
                    movie.Image = reader["Image"].ToString();
                    movie.Date = reader["Date"].ToString();
                    movies.Add(movie);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        return movies;
    }
}