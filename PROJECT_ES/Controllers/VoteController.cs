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
using PROJECT_ES.Models;


//Interface do observer
public interface IObserver
{
    void NotifyVote();
}

public class VoteController : Controller
{
    private readonly string _connectionString;
    private readonly CompetitionRepository _competitionRepository;
    private readonly CompetitionDetailsRepository _competitionDetailsRepository;
    private readonly CategoryRepository _categoryRepository;
    private readonly List<IObserver> _observers; //lista de observadores

    public VoteController(IConfiguration configuration, CompetitionRepository competitionRepository,
        CompetitionDetailsRepository competitionDetailsRepository, CategoryRepository categoryRepository)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");

        _competitionRepository = competitionRepository;
        _competitionDetailsRepository = competitionDetailsRepository;
        _categoryRepository = categoryRepository;
        
        _observers = new List<IObserver>();
    }

    public void RegisterObserver(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void UnregisterObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }

    private void NotifyObservers()
    {
        foreach (var observer in _observers)
        {
            observer.NotifyVote();
        }
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
                Email = vote.Email
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

            MailMessage message = new MailMessage();
            message.From = new MailAddress(senderEmail);
            message.To.Add(viewModel.Email);
            message.Subject = "Confirmação de Voto";
            message.Body = "Obrigado por votar na competição!\nO seu voto foi registado com sucesso no filme " + viewModel.Name;



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

            NotifyObservers();

            return Json(new { done = true });
        }

        return View("VotingPage", viewModel);
    }
}

public class EmailSender : IObserver
{
    private readonly string _senderEmail;
    private readonly string _senderPassword;
    private readonly string _smtpServer;

    public EmailSender(string senderEmail, string senderPassword, string smtpServer)
    {
        _senderEmail = senderEmail;
        _senderPassword = senderPassword;
        _smtpServer = smtpServer;
    }

    public void NotifyVote()
    {
       /* string recipientEmail = ""; // Insira o endereço de e-mail do destinatário aqui
        string subject = "Confirmação de Voto";
        string body = "Obrigado por votar na competição!<br>\nO seu voto foi registrado com sucesso.";

        MailMessage message = new MailMessage();
        message.From = new MailAddress(_senderEmail);
        message.To.Add(recipientEmail);
        message.Subject = subject;
        message.Body = body;
        message.IsBodyHtml = true;

        SmtpClient smtpClient = new SmtpClient(_smtpServer, 587);
        smtpClient.EnableSsl = true;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential(_senderEmail, _senderPassword);

        try
        {
            smtpClient.Send(message);
        }
        catch (SmtpException ex)
        {
            // Trate a exceção de envio de e-mail aqui
        }*/
    }
}

