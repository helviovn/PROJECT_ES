using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PROJECT_ES.Data;

namespace PROJECT_ES.Service;

public class CompetitionRepository
{
    private readonly string _connectionString;

    public CompetitionRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    
    public async Task AddCompetitionWithMoviesAsync(Competition competition, List<int> movieIds,List<int> categoriesIds)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var transaction = connection.BeginTransaction())
            {
                var competitionId = await AddCompetitionAsync(competition);
                var distinctMovieIds = movieIds.Distinct().ToList();
                var query =
                    "INSERT INTO dbo.Competition_has_Movie (CompetitionID, MovieID) VALUES (@CompetitionId, @MovieId)";
                
                foreach (var movieId in distinctMovieIds)
                {
                    var parameters = new { CompetitionId = competitionId, MovieId = movieId };
                    await connection.ExecuteAsync(query, parameters, transaction);
                }
                var query2 =
                    "INSERT INTO dbo.Competition_has_Category (CompetitionID, CategoryID) VALUES (@CompetitionId, @CategoryID)";

                
                var distinctCategoriesIds = categoriesIds.Distinct().ToList();
                foreach (var categoriesId in distinctCategoriesIds)
                {
                    var parameters2 = new { CompetitionId = competitionId, CategoryID = categoriesId };
                    await connection.ExecuteAsync(query2, parameters2, transaction);
                }
                transaction.Commit();
            }
        }
    }



    private async Task<int> AddCompetitionAsync(Competition competition)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            
            var query = "INSERT INTO dbo.Competition (Description, Name, DataInicio, DataFim, Nparticipantes,Ispublic) " +
                        "VALUES (@Description, @Name, @data_inicio, @data_fim, 0,0);" +
                        "SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new
            {
                Description = competition.Description,
                Name = competition.Name,
                data_inicio = competition.data_inicio,
                data_fim = competition.data_fim,
                n_participantes = competition.n_participantes,
                Public = competition.Ispublic,
                //PUBLICCC PQ QUE NAO FUNCIONA?AFONSO
            };

            var competitionId = await connection.ExecuteScalarAsync<int>(query, parameters);

            return competitionId;
        }
    }

    public async Task<int> ViewDetails(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "SELECT * FROM dbo.Movie WHERE Id = @Id";

            var parameters = new { Id = id };

            await connection.ExecuteAsync(query, parameters);

            return id;
        }
    }

    public async Task<IEnumerable<Competition>> GetCompetitionsAsync()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "SELECT * FROM dbo.Competition";
            var competitions = await connection.QueryAsync<Competition>(query);

            return competitions;
        }
    }

    public async Task AllDetails(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "";

        }
    }
    

    
    
    
    

}