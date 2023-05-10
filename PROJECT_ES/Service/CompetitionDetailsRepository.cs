using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PROJECT_ES.Data;

namespace PROJECT_ES.Service;

public class CompetitionDetailsRepository
{
    private readonly string _connectionString;

    public CompetitionDetailsRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    
    public async Task<IEnumerable<Movie>> GetAllMoviesAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = @"SELECT Movie.* FROM Competition 
                      INNER JOIN Competition_has_Movie ON Competition.Id = Competition_has_Movie.CompetitionID 
                      INNER JOIN Movie ON Competition_has_Movie.MovieID = Movie.Id
                      WHERE Competition.Id = @id";

            var movies = await connection.QueryAsync<Movie>(query, new { id });

            return movies;
        }
    }
    
    public async Task<IEnumerable<Category>> GetAllCategoriesAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = @"SELECT Category.* FROM Competition 
                      INNER JOIN Competition_has_Category ON Competition.Id = Competition_has_Category.CompetitionID 
                      INNER JOIN Category on Competition_has_Category.CategoryID = Category.Id
                      WHERE Competition.Id = @id";

            var categories = await connection.QueryAsync<Category>(query, new { id });

            return categories;
        }
    }
    
    public async Task<IEnumerable<Competition>> GetAllDetailsAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "SELECT * FROM dbo.Competition";

            using (var command = new SqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var competitions = new List<Competition>();
                    while (await reader.ReadAsync())
                    {
                        var competition = new Competition
                        {
                            Id = reader.GetInt32(0),
                            Description = reader.GetString(1),
                            Name = reader.GetString(2),
                            data_inicio = reader.GetDateTime(3),
                            data_fim = reader.GetDateTime(4),
                            n_participantes = reader.GetInt32(5),
                            Ispublic = reader.GetBoolean(6),
                        };

                        competitions.Add(competition);
                    }

                    return competitions;
                }
            }
        }
    }
    
}