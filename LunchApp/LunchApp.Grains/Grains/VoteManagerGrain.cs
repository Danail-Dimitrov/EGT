using LunchApp.Grains.Abstractions;
using LunchApp.Grains.States;

namespace LunchApp.Grains.Grains
{
    public class VoteManagerGrain : Grain, IVoteManagerGrain
    {
        private readonly IPersistentState<VoteManagerState> _state;
        public const short MAX_LOCATIONS = 5;

        public VoteManagerGrain([PersistentState("manager", "lunchapp")] IPersistentState<VoteManagerState> state)
        {
            _state = state;
        }

        public void Initialize(ICollection<string> locations)
        {
            // If there is no place for more locations, do nothing
            if (_state.State.Locations.Count + locations.Count > MAX_LOCATIONS)
                return;

            foreach (var location in locations)
                if (_state.State.Locations.Count < VoteManagerState.MAX_LOCATIONS)
                    _state.State.Locations.Add(location);
        }

        public void Vote(string user, string location)
        {
            if( _state.State.Locations.Contains(location) && 
                !_state.State.Votes.ContainsKey(user))
                _state.State.Votes.Add(user, location);
            else
                throw new InvalidOperationException("Invalid vote or user has already voted.");
        }

        public Task<bool> ExistsAsync()
        {
            return Task.FromResult(_state.State.Locations.Count > 0);
        }

        public Task<ICollection<string>> GetLocations()
        {
            return Task.FromResult(_state.State.Locations);
        }

        public Task<IDictionary<string, string>> GetVotes()
        {
            return Task.FromResult(_state.State.Votes);
        }
    }
}
