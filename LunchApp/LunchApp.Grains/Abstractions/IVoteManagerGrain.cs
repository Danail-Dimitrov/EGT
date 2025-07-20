namespace LunchApp.Grains.Abstractions
{
    public interface IVoteManagerGrain : IGrainWithStringKey
    {
        void Initialize(ICollection<string> locations);

        void Vote(string user,string location);

        Task<bool> ExistsAsync();

        Task<ICollection<string>> GetLocations();

        Task<IDictionary<string, string>> GetVotes();
    }
}
