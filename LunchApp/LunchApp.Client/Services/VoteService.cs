using LunchApp.Client.Services.Interfaces;
using LunchApp.Grains.Abstractions;

namespace LunchApp.Client.Services
{
    public class VoteService : IVoteService
    {
        private readonly IClusterClient _clusterClient;

        public VoteService(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        public Task CastVote()
        {
            throw new NotImplementedException();
        }

        public Task<DateTime> GetTime()
        {
            throw new NotImplementedException();
        }

        public async Task SetTime(DateTime time)
        {
            var clockGrain = this._clusterClient.GetGrain<IClockGrain>(0);

            await clockGrain.SetTime(time);
        }
    }
}
