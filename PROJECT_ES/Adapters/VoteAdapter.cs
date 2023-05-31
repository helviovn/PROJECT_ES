using PROJECT_ES.Data;
using PROJECT_ES.ViewModels;

namespace PROJECT_ES.Adapters;

public class VoteAdapter
{
    public VoteViewModel Adapt(Vote vote)
    {
        var voteViewModel = new VoteViewModel
        {
            CompetitionId = vote.CompetitionId,
            CategoryId = vote.CategoryId,
            MovieId = vote.MovieId,
            Name = vote.Username,
            Email = vote.Email
        };

        return voteViewModel;
    }
}
