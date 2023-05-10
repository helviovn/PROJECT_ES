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

            var query = @"SELECT * FROM Competition WHERE Competition.Id = @id";

            var details = await connection.QueryAsync<Competition>(query, new { id });

            return details;
        }
    }
    
}