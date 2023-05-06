

using Dapper;
using Microsoft.Data.SqlClient;
using PROJECT_ES.Data;

namespace PROJECT_ES.Service;

public class MovieRepository
{
    private readonly string _connectionString;

    public MovieRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<int> AddMovieAsync(Movie movie)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "INSERT INTO dbo.Movie (Title, Description, Diretor, Actor, Genre, Writers, Image, Date) " +
                        "VALUES (@Title, @Description, @Diretor, @Actor, @Genre, @Writers, @Image, @Date);" +
                        "SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new
            {
                Title = movie.Title,
                Description = movie.Description,
                Diretor = movie.Diretor,
                Actor = movie.Actor,
                Genre = movie.Genre,
                Writers = movie.Writers,
                Image = movie.Image,
                Date = movie.Date
            };

            var id = await connection.ExecuteScalarAsync<int>(query, parameters);

            return id;
        }
    }

    public async Task<Movie> GetMovieAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "SELECT * FROM dbo.Movie WHERE Id = @Id";

            var parameters = new { Id = id };
            var movie = await connection.QuerySingleOrDefaultAsync<Movie>(query, parameters);

            return movie;
        }
    }
    public async Task<Movie> GetMovieAsync(string title)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "SELECT * FROM dbo.Movie WHERE Title = @Title";

            var parameters = new { Title = title };
            var movie = await connection.QuerySingleOrDefaultAsync<Movie>(query, parameters);

            return movie;
        }
    }
    public async Task<IEnumerable<Movie>> GetMoviesAsync()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "SELECT * FROM dbo.Movie";

            var movies = await connection.QueryAsync<Movie>(query);

            return movies;
        }
    }

    public async Task UpdateMovieAsync(Movie movie)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "UPDATE dbo.Movie SET Title = @Title, Description = @Description, " +
                        "Director = @Director, Actor = @Actor, Genre = @Genre, Writers = @Writers, " +
                        "Image = @Image, Date = @Date WHERE Id = @Id";

            var parameters = new
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Director = movie.Diretor,
                Actor = movie.Actor,
                Genre = movie.Genre,
                Writers = movie.Writers,
                Image = movie.Image,
                Date = movie.Date
            };

            await connection.ExecuteAsync(query, parameters);
        }
    }

    public async Task DeleteMovieAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "DELETE FROM dbo.Movie WHERE Id = @Id";

            var parameters = new { Id = id };

            await connection.ExecuteAsync(query, parameters);
        }
    }
}