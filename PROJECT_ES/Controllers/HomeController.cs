using System.Collections;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PROJECT_ES.Data;
using PROJECT_ES.Service;
using PROJECT_ES.ViewModels;

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
        
        var images = new List<string>
        {
            "/images/competitions/1.jpg",
            "/images/competitions/2.jpg",
            "/images/competitions/3.jpg",
            "/images/competitions/4.jpg",
            "/images/competitions/5.jpg",
            "/images/competitions/6.jpeg",
            "/images/competitions/7.jpg",
            "/images/competitions/8.jpg",
            "/images/competitions/9.png",
        };
        
        int index = 0;
        foreach (var competition in competitions)
        {
            var participantCount = GetParticipantCount(competition.Id);
            competition.n_participantes = participantCount;
            competition.Image = images[index % images.Count];
            index++;
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

    [HttpPost]
    public async Task<IActionResult> Subscribe(string email)
    {

        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
            INSERT INTO dbo.Vote (Username, Email, MovieID, CategoryID, CompetitionID)
            VALUES (null, @Email, null, null, null)
        ";

                var parameters = new
                {
                    Email = email
                };

                await connection.ExecuteAsync(query, parameters);
            }

            return RedirectToAction("FirstPage");
        }
    }
}