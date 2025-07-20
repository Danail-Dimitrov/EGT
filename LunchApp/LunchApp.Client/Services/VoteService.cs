using LunchApp.Client.Services.Interfaces;
using LunchApp.Grains.Abstractions;
using System;

namespace LunchApp.Client.Services
{
    public class VoteService : IVoteService
    {
        private readonly IClusterClient _clusterClient;

        public VoteService(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        public async Task<bool> CanDisplayVote()
        {
            var clockGrain = this._clusterClient.GetGrain<IClockGrain>(0);

            var time = await clockGrain.GetCurrentTime();

            TimeSpan targetTime = new TimeSpan(13, 30, 0);

            return time.TimeOfDay < targetTime;
        }

        public async Task CastVote(string userName, string location)
        {
            var time = await GetTime();
            var date = time.ToString("dd-MM-yyyy");

            var voteGrain = _clusterClient.GetGrain<IVoteManagerGrain>(date);
            voteGrain.Vote(userName, location);
        }

        public async Task CreateVote(List<string> strings)
        {
            var time = await GetTime();
            var date = time.ToString("dd-MM-yyyy");

            var voteGrain = _clusterClient.GetGrain<IVoteManagerGrain>(date);
            voteGrain.Initialize(strings);
        }

        public async Task<bool> ExistsVoteForDay(DateTime date)
        {
            string currentDateString = date.ToString("dd-MM-yyyy");

            var voteGrain = _clusterClient.GetGrain<IVoteManagerGrain>(currentDateString);

            return await voteGrain.ExistsAsync();
        }

        public async Task<ICollection<string>> GetLocations()
        {
            var time = await GetTime();
            var date = time.ToString("dd-MM-yyyy");

            var voteGrain = _clusterClient.GetGrain<IVoteManagerGrain>(date);
            return await voteGrain.GetLocations();
        }

        public async Task<DateTime> GetTime()
        {
            var clockGrain = this._clusterClient.GetGrain<IClockGrain>(0);

            return await clockGrain.GetCurrentTime();
        }

        public async Task<IDictionary<string, string>> GetVotes()
        {
            var time = await GetTime();
            var date = time.ToString("dd-MM-yyyy");

            var voteGrain = _clusterClient.GetGrain<IVoteManagerGrain>(date);
            return await voteGrain.GetVotes();
        }

        public async Task<bool> HasVotingEnded()
        {
            var clockGrain = this._clusterClient.GetGrain<IClockGrain>(0);

            var time = await clockGrain.GetCurrentTime();

            TimeSpan targetTime = new TimeSpan(11, 30, 0);

            return time.TimeOfDay > targetTime;
        }

        public async Task SetTime(DateTime time)
        {
            var clockGrain = this._clusterClient.GetGrain<IClockGrain>(0);

            await clockGrain.SetTime(time);
        }
    }
}
