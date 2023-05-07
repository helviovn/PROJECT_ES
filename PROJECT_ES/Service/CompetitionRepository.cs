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

    
    public async Task AddCompetitionWithMoviesAsync(Competition competition, List<int> movieIds)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var transaction = connection.BeginTransaction())
            {
                var competitionId = await AddCompetitionAsync(competition);
                var query =
                    "INSERT INTO dbo.Competition_has_Movie (CompetitionId, MovieId) VALUES (@CompetitionId, @MovieId)";

                var distinctMovieIds = movieIds.Distinct().ToList();
                foreach (var movieId in distinctMovieIds)
                {
                    var parameters = new { CompetitionId = competitionId, MovieId = movieId };
                    await connection.ExecuteAsync(query, parameters, transaction);
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
            
            var query = "INSERT INTO dbo.Competition (Description, Name, DataInicio, DataFim, Nparticipantes,Public) " +
                        "VALUES (@Description, @Name, @data_inicio, @data_fim, 0,0);" +
                        "SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new
            {
                Description = competition.Description,
                Name = competition.Name,
                data_inicio = competition.data_inicio,
                data_fim = competition.data_fim,
                n_participantes = competition.n_participantes,
                Public = competition.Public,
                //PUBLICCC PQ QUE NAO FUNCIONA?AFONSO
            };

            var competitionId = await connection.ExecuteScalarAsync<int>(query, parameters);
            return competitionId;
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
}