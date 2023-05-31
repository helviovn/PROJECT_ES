
﻿using System.Data;

using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PROJECT_ES.Data;

namespace PROJECT_ES.Service;

public class CompetitionDetailsRepository
{
    private readonly string _connectionString;

    public CompetitionDetailsRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    
    public async Task<IEnumerable<Movie>> GetAllMoviesAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = @"SELECT Movie.* FROM Competition 
                      INNER JOIN Competition_has_Movie ON Competition.Id = Competition_has_Movie.CompetitionID 
                      INNER JOIN Movie ON Competition_has_Movie.MovieID = Movie.Id
                      WHERE Competition.Id = @id";

            var movies = await connection.QueryAsync<Movie>(query, new { id });

            return movies;
        }
    }
    
    public async Task<IEnumerable<Category>> GetAllCategoriesAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = @"SELECT Category.* FROM Competition 
                      INNER JOIN Competition_has_Category ON Competition.Id = Competition_has_Category.CompetitionID 
                      INNER JOIN Category on Competition_has_Category.CategoryID = Category.Id
                      WHERE Competition.Id = @id";

            var categories = await connection.QueryAsync<Category>(query, new { id });

            return categories;
        }
    }
    
    public async Task<IEnumerable<Competition>> GetAllDetailsAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"SELECT * FROM Competition WHERE Competition.Id = @id";

           using (var command = new SqlCommand(query, connection))
           {
               command.Parameters.AddWithValue("@id", id);
               using (var reader = await command.ExecuteReaderAsync())
               {
                   var competitions = new List<Competition>();
                   while (await reader.ReadAsync())
                   {
                       var competition = new Competition
                       {
                           Id = reader.GetInt32(0),
                           Description = reader.GetString(1),
                           Name = reader.GetString(2),
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


    public async Task<IEnumerable<Vote>> GetEstatistic(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"SELECT DISTINCT Category.Name AS CategoryName, Movie.Title AS MovieTitle, COUNT(*) AS VoteCount
            FROM Vote
            INNER JOIN Competition ON Competition.Id = Vote.CompetitionID
            INNER JOIN Category ON Category.Id = Vote.CategoryID
            INNER JOIN Movie ON Movie.Id = Vote.MovieID
            WHERE Competition.Id = @id
            GROUP BY Category.Name, Movie.Title
            ORDER BY Category.Name";
            var votes = await connection.QueryAsync<Vote>(query, new { id });
            return votes;
        }
    }


    public async Task<IEnumerable<Vote>> States()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"WITH VoteCounts AS (
            SELECT Competition.Name AS CompetitionName, Category.Name AS CategoryName, Movie.Title AS MovieTitle, COUNT(*) AS VoteCount,
            ROW_NUMBER() OVER (PARTITION BY Competition.Name, Category.Name ORDER BY COUNT(*) DESC) AS RowNum
            FROM Vote
            INNER JOIN Competition ON Competition.Id = Vote.CompetitionID
            INNER JOIN Category ON Category.Id = Vote.CategoryID
            INNER JOIN Movie ON Movie.Id = Vote.MovieID
            GROUP BY Competition.Name, Category.Name, Movie.Title)
            SELECT CompetitionName, CategoryName, MovieTitle, VoteCount
            FROM VoteCounts
            WHERE RowNum = 1
            ORDER BY CompetitionName;";
            var statistics = await connection.QueryAsync<Vote>(query);
            return statistics;
        }
    }
    
    public async Task<IEnumerable<Vote>> StatesByCompId(int competitionId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"WITH VoteCounts AS (
                        SELECT Competition.Name AS CompetitionName, Category.Name AS CategoryName, Movie.Title AS MovieTitle, COUNT(*) AS VoteCount,
                        ROW_NUMBER() OVER (PARTITION BY Competition.Name, Category.Name ORDER BY COUNT(*) DESC) AS RowNum
                          FROM Vote
                          INNER JOIN Competition ON Competition.Id = Vote.CompetitionID
                          INNER JOIN Category ON Category.Id = Vote.CategoryID
                          INNER JOIN Movie ON Movie.Id = Vote.MovieID
                          WHERE Competition.Id = @competitionId
                          GROUP BY Competition.Name, Category.Name, Movie.Title
                        )
                        SELECT CompetitionName, CategoryName, MovieTitle, VoteCount
                        FROM VoteCounts
                        WHERE RowNum = 1
                        ORDER BY CompetitionName;";
            var statistics = await connection.QueryAsync<Vote>(query,new { competitionId });
            return statistics;
        }
    }
    
}