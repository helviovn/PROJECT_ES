using System.Collections;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PROJECT_ES.Controllers;
using PROJECT_ES.Data;
using PROJECT_ES.Service;
using PROJECT_ES.ViewModels;
using Microsoft.AspNetCore.Identity;


public class HomeController : BaseController
{
    private readonly string _connectionString;
    
    private readonly UserManager<IdentityUser> _userManager;

    public HomeController(IConfiguration configuration, CompetitionRepository competitionRepository,
        CompetitionDetailsRepository competitionDetailsRepository, CategoryRepository categoryRepository
        ,UserManager<IdentityUser> userManager)
        : base(competitionRepository, competitionDetailsRepository, categoryRepository)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _userManager = userManager;
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
        
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            ViewBag.IsAdmin = isAdmin;
        }
        else
        {
            // Handle the case where the user is not found or not authenticated
            ViewBag.IsAdmin = false; // Set a default value or handle accordingly
        }

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

            return RedirectToAction("Home");
        }
    }


    /*[HttpPost]
    public async Task<IActionResult> Vote(VoteViewModel viewModel)
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
                Email = viewModel.Email
            };

            await connection.ExecuteAsync(query, parameters);
        }

        return View("FirstPage");
    }*/
}