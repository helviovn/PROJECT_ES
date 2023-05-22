using System.Collections;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PROJECT_ES.Data;
using PROJECT_ES.Service;

public class HomeController  : Controller
{

    private readonly CompetitionRepository _competitionRepository;
    private readonly string _connectionString;

    public HomeController(IConfiguration configuration, CompetitionRepository competitionRepository)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _competitionRepository = competitionRepository;
    }

/*
    public async Task<IActionResult> FirstPage()
    {
        var competitions = await _competitionRepository.GetCompetitionsAsync();
        return View(competitions);
    }
    
    */
    public async Task<IActionResult> FirstPage()
    {
        var competitions = await _competitionRepository.GetCompetitionsAsync();

        foreach (var competition in competitions)
        {
            var participantCount = GetParticipantCount(competition.Id);
            competition.n_participantes = participantCount;
        }

        return View(competitions);
    }
    
    public async Task<IActionResult> Home()
    {
        return RedirectToAction("FirstPage");
    }
    
    public  int GetParticipantCount(int competitionId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var query = @"
            SELECT COUNT(DISTINCT Email) 
            FROM dbo.Vote 
            WHERE CompetitionID = @CompetitionID";

            var parameters = new
            {
                CompetitionID = competitionId
            };

            return connection.ExecuteScalar<int>(query, parameters);
        }
    }
}