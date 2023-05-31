using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PROJECT_ES.Data;
using System.Net;
using System.Net.Mail;

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
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "SELECT DISTINCT Email FROM Vote";

            var emails = new List<string>();

            using (var command = new SqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var email = reader.GetString(0);
                        emails.Add(email);
                    }
                }
            }

            // Envio de e-mail 
            string senderEmail = "webvotecine@gmail.com";
            string senderPassword = "nzmvhgrsnsxetlco";
            string smtpServer = "smtp.gmail.com";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(senderEmail);

// Adicione os destinatários à lista de e-mails BCC
            foreach (var email in emails)
            {
                message.Bcc.Add(email);
            }

            message.Subject = "Nova COMPETIÇÃO";
            message.Body = "Informamos que a competição " + @competition.Name + " foi criada";

            SmtpClient smtpClient = new SmtpClient(smtpServer, 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

            try
            {
                smtpClient.Send(message);
            }
            catch (SmtpException ex)
            {
                // Trate a exceção aqui
            }

        }
    }



    private async Task<int> AddCompetitionAsync(Competition competition)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            
            var query = "INSERT INTO dbo.Competition (Description, Name, DataInicio, DataFim, Nparticipantes,Ispublic) " +
                        "VALUES (@Description, @Name, @data_inicio, @data_fim, 0,1);" +
                        "SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new
            {
                Description = competition.Description,
                Name = competition.Name,
                data_inicio = competition.data_inicio,
                data_fim = competition.data_fim,
                n_participantes = competition.n_participantes,
                Public = competition.Ispublic,
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

            var query = "SELECT * FROM dbo.Competition ";

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
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
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
    
    public async Task<Competition> GetCompetitionByIdAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "SELECT * FROM dbo.Competition WHERE Id = @Id ";

            var parameters = new { Id = id };

            var competition = await connection.QueryFirstOrDefaultAsync<Competition>(query, parameters);

            return competition;
        }
    }
    

    public async Task UpdateCompetitionAsync(Competition competition)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "UPDATE dbo.Competition SET " +
                        "Name = @name, " +
                        "Description = @description, " +
                        "DataInicio = @dataInicio, " +
                        "DataFim = @dataFim, " +
                        "NParticipantes = @nParticipantes, " +
                        "Ispublic = @isPublic " +
                        "WHERE Id = @id";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", competition.Id);
                command.Parameters.AddWithValue("@name", competition.Name);
                command.Parameters.AddWithValue("@description", competition.Description);
                command.Parameters.AddWithValue("@dataInicio", competition.data_inicio);
                command.Parameters.AddWithValue("@dataFim", competition.data_fim);
                command.Parameters.AddWithValue("@nParticipantes", competition.n_participantes);
                command.Parameters.AddWithValue("@isPublic", competition.Ispublic);

                await command.ExecuteNonQueryAsync();
            }
        }
        
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "SELECT DISTINCT Email FROM Vote WHERE CompetitionID = @id";

            var emails = new List<string>();

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", competition.Id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var email = reader.GetString(0);
                        emails.Add(email);
                    }
                }
            }

            // Envio de e-mail apenas se houver destinatários
            if (emails.Count > 0)
            {
                string senderEmail = "webvotecine@gmail.com";
                string senderPassword = "nzmvhgrsnsxetlco";
                string smtpServer = "smtp.gmail.com";

                MailMessage message = new MailMessage();
                message.From = new MailAddress(senderEmail);

                foreach (var email in emails)
                {
                    message.Bcc.Add(email);
                }

                message.Subject = "Encerramento da COMPETIÇÃO";
                message.Body = "Informamos que a competição " + competition.Name + " que subscreveu encontra-se encerrada";

                SmtpClient smtpClient = new SmtpClient(smtpServer, 587);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

                try
                {
                    smtpClient.Send(message);
                }
                catch (SmtpException ex)
                {
                    // Lida com exceção do SmtpClient
                }
            }
        }

    }



    public async Task DeleteCompetitionAsync(int id)
{
    using (var connection = new SqlConnection(_connectionString))
    {
        await connection.OpenAsync();

        var query = "DELETE FROM dbo.Competition WHERE Id = @Id";

        var parameters = new { Id = id };

        await connection.ExecuteAsync(query, parameters);
    }
}
    
    public async Task<IEnumerable<Competition>> EditCompetition(int id, Competition competition)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "UPDATE dbo.Competition SET Description = @Description, Name = @Name, DataInicio = @DataInicio, DataFim = @DataFim, Ispublic = @Ispublic WHERE Id = @Id";

            var parameters = new
            {
                Description = competition.Description,
                Name = competition.Name,
                DataInicio = competition.data_inicio,
                DataFim = competition.data_fim,
                n_participantes = competition.n_participantes,
                Ispublic = competition.Ispublic,
                Id = id
            };


            await connection.ExecuteAsync(query, parameters);

            var competitions = await connection.QueryAsync<Competition>("SELECT * FROM dbo.Competition");

            return competitions;
        }
    }


}