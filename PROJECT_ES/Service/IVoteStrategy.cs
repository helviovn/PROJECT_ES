using PROJECT_ES.Data;
using PROJECT_ES.ViewModels;

namespace PROJECT_ES.Service;

public interface IVoteStrategy
{
    Task<bool> CanVote(VoteViewModel viewModel);
}

public class CompetitionDateStrategy : IVoteStrategy
{
    private readonly CompetitionRepository _competitionRepository;

    public CompetitionDateStrategy(CompetitionRepository competitionRepository)
    {
        _competitionRepository = competitionRepository;
    }

    public async Task<bool> CanVote(VoteViewModel viewModel)
    {
        Competition competition = await _competitionRepository.GetCompetitionByIdAsync(viewModel.CompetitionId);
        DateTime currentDate = DateTime.Now;
        return currentDate >= competition.data_inicio && currentDate <= competition.data_fim;
    }
}

