
namespace LunchApp.Client.Services.Interfaces
{
    public interface IVoteService
    {
        Task<DateTime> GetTime();

        Task SetTime(DateTime time);

        Task<ICollection<string>> GetLocations();

        Task CastVote(string userName, string location);

        Task<bool> ExistsVoteForDay(DateTime date);

        Task CreateVote(List<string> strings);

        Task<bool> HasVotingEnded();

        Task<IDictionary<string, string>> GetVotes();
        Task<bool> CanDisplayVote();
    }
}
