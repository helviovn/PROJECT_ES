using PROJECT_ES.Data;

namespace PROJECT_ES.Models;

// Builder class
public class VoteBuilder
{
    private Vote vote;

    public VoteBuilder()
    {
        vote = new Vote();
    }

    public VoteBuilder WithMovieId(int movieId)
    {
        vote.MovieId = movieId;
        return this;
    }

    public VoteBuilder WithCategoryId(int categoryId)
    {
        vote.CategoryId = categoryId;
        return this;
    }

    public VoteBuilder WithCompetitionId(int competitionId)
    {
        vote.CompetitionId = competitionId;
        return this;
    }

    public VoteBuilder WithUsername(string username)
    {
        vote.Username = username;
        return this;
    }

    public VoteBuilder WithEmail(string email)
    {
        vote.Email = email;
        return this;
    }

    public Vote Build()
    {
        return vote;
    }
}