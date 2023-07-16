using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using PROJECT_ES.Data;
using PROJECT_ES.Service;
using PROJECT_ES.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using PROJECT_ES.Adapters;
using PROJECT_ES.Interfaces;
using PROJECT_ES.Models;
using Microsoft.AspNetCore.Identity;



public class VoteController : Controller
{
    private readonly string _connectionString;
    private readonly CompetitionRepository _competitionRepository;
    private readonly CompetitionDetailsRepository _competitionDetailsRepository;
    private readonly CategoryRepository _categoryRepository;
    private readonly VoteAdapter _voteAdapter;
    private readonly VoteObservable _voteObservable;
    private readonly UserManager<IdentityUser> _userManager;

    public VoteController(
        IConfiguration configuration,
        CompetitionRepository competitionRepository,
        CompetitionDetailsRepository competitionDetailsRepository,
        CategoryRepository categoryRepository,
        VoteAdapter voteAdapter,
        VoteObservable voteObservable,UserManager<IdentityUser> userManager)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _competitionRepository = competitionRepository;
        _competitionDetailsRepository = competitionDetailsRepository;
        _categoryRepository = categoryRepository;
        _voteAdapter = voteAdapter;
        _voteObservable = voteObservable;
        _userManager = userManager;
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
        return View("VotingPage", viewModel);
    }

    //mesma pessoa não pode votar na mm categoria
    [HttpPost]
    public async Task<IActionResult> Vote(VoteViewModel viewModel)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query1 = @"
                        SELECT COUNT(*) FROM dbo.Vote
                        WHERE CompetitionID=@CompetitionID AND CategoryID=@CategoryID AND Email=@Email";
            
            // Crie um novo objeto Vote com base nos dados do viewModel
            var vote = new Vote
            {
                CompetitionId = viewModel.CompetitionId,
                CategoryId = viewModel.CategoryId,
                MovieId = viewModel.MovieId,
                Username = viewModel.Name,
                Email = viewModel.Email
            };

            // Adapte o objeto Vote para VoteViewModel usando o VoteAdapter
            var adaptedVote = _voteAdapter.Adapt(vote);
            
            var parameters = new
            {
                CompetitionID = adaptedVote.CompetitionId,
                CategoryID = adaptedVote.CategoryId,
                Email = adaptedVote.Email
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
            // Usando o VoteBuilder para construir um objeto Vote
            var vote = new VoteBuilder()
                .WithCategoryId(viewModel.CategoryId)
                .WithCompetitionId(viewModel.CompetitionId)
                .WithMovieId(viewModel.MovieId)
                .WithUsername(viewModel.Name)
                .WithEmail(viewModel.Email)
                .Build();
            
            var parameters = new
            {
                CompetitionID = vote.CompetitionId,
                CategoryID = vote.CategoryId,
                MovieID = vote.MovieId,
                Username = vote.Username,
                Email = vote.Email
            };
            

            await connection.ExecuteAsync(query, parameters);

            // Envio de e-mail 

            string senderEmail = "webvotecine@gmail.com";
            string senderPassword = "nzmvhgrsnsxetlco";
            string smtpServer = "smtp.gmail.com";
            
            var MovieName = await _competitionDetailsRepository.GetMovieNameByMovieIDAsync(viewModel.MovieId);
            var CategoryName = await _competitionDetailsRepository.GetCategoryNameByCategoryIDAsync(viewModel.CategoryId);
            var CompetitionName = await _competitionDetailsRepository.GetCompetitionNameByCompetitionIDAsync(viewModel.CompetitionId);

            MailMessage message = new MailMessage();
            message.From = new MailAddress(senderEmail);
            message.To.Add(viewModel.Email);
            message.Subject = "Confirmação de Voto";
            message.Body = "Obrigado "+ viewModel.Name+" por votar na categoria "+CategoryName+" da competição "+CompetitionName+"!\nO seu voto foi registado com sucesso no filme "+MovieName+".";


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
                //preciso de meter um erro aqui 
            }

            _voteObservable.NotifyObservers(vote);

            return Json(new { done = true });
        }

        return View("VotingPage", viewModel);
    }
}



