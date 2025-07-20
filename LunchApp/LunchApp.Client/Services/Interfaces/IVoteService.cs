namespace LunchApp.Client.Services.Interfaces
{
    public interface IVoteService
    {
        public Task<DateTime> GetTime();

        public Task SetTime(DateTime time);

        public Task CastVote();
    }
}
