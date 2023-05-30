using System.Collections;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PROJECT_ES.Controllers;
using PROJECT_ES.Data;
using PROJECT_ES.Service;
using PROJECT_ES.ViewModels;

public class HomeController : BaseController
{
    private readonly string _connectionString;

    public HomeController(IConfiguration configuration, CompetitionRepository competitionRepository,
        CompetitionDetailsRepository competitionDetailsRepository, CategoryRepository categoryRepository)
        : base(competitionRepository, competitionDetailsRepository, categoryRepository)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

/*
    public async Task<IActionResult> FirstPage()
    {
        var competitions = await _competitionRepository.GetCompetitionsAsync();
        return View(competitions);
    }
    
    */
    
    protected override async Task<IActionResult> GetViewModel(int competitionId, int categoryId)
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
        return await BaseAction(0, 0);
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