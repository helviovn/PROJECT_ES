using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using PROJECT_ES.Data;
using PROJECT_ES.Service;
using PROJECT_ES.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;


public class VoteController : Controller
{
    private readonly string _connectionString;
    private readonly CompetitionRepository _competitionRepository;
    private readonly CompetitionDetailsRepository _competitionDetailsRepository;
    private readonly CategoryRepository _categoryRepository;

    public VoteController(IConfiguration configuration, CompetitionRepository competitionRepository,
        CompetitionDetailsRepository competitionDetailsRepository, CategoryRepository categoryRepository)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");

        _competitionRepository = competitionRepository;
        _competitionDetailsRepository = competitionDetailsRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IActionResult> VotingPage(int competitionId, int categoryId)
    {
        var competition = await _competitionRepository.GetCompetitionByIdAsync(competitionId);
        var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
        var movies = await _competitionDetailsRepository.GetAllMoviesAsync(competitionId);

        var viewModel = new VoteViewModel()
        {
            Competition = competition,
            Category = category,
            Movies = movies
        };

        return View("VotingPage", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Vote(VoteViewModel viewModel)
    {

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();


            var query1 = @"
                        SELECT COUNT(*) FROM dbo.Vote
                        WHERE CompetitionID=@CompetitionID AND CategoryID=@CategoryID AND Email=@Email";

            var parameters = new
            {
                CompetitionID = viewModel.CompetitionId,
                CategoryID = viewModel.CategoryId,
                Email = viewModel.Email
            };
            var AlreadyVote = await connection.ExecuteScalarAsync<int>(query1, parameters);

            if (AlreadyVote > 0)
            {
                return Json(new { error = true });
            }
        }

        using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
            INSERT INTO dbo.Vote (Username, Email, MovieID, CategoryID, CompetitionID)
            VALUES (@Username, @Email, @MovieID, @CategoryID, @CompetitionID)
        ";

                var parameters = new
                {
                    CompetitionID = viewModel.CompetitionId,
                    CategoryID = viewModel.CategoryId,
                    MovieID = viewModel.MovieId,
                    Username = viewModel.Name,
                    Email = viewModel.Email
                };

                await connection.ExecuteAsync(query, parameters);
                return Json(new { done = true });
                
            }
        
        return View("VotingPage", viewModel);
    }
}
    


 

   
    
