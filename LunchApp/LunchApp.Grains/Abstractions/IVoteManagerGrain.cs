namespace LunchApp.Grains.Abstractions
{
    public interface IVoteManagerGrain : IGrainWithStringKey
    {
        Task Initialize(ICollection<string> locations);

        Task Vote(string user,string location);

        Task<bool> ExistsAsync();

        Task<ICollection<string>> GetLocations();

        Task<IDictionary<string, string>> GetVotes();
    }
}
