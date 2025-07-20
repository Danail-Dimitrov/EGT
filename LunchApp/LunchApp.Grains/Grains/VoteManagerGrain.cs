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

        public async Task Initialize(ICollection<string> locations)
        {
            _state.State.Locations ??= new List<string>(5);
            _state.State.Votes ??= new Dictionary<string, string>();

            if (_state.State.Locations.Count + locations.Count > MAX_LOCATIONS)
                return;

            foreach (var location in locations)
                if (_state.State.Locations.Count < VoteManagerState.MAX_LOCATIONS)
                    _state.State.Locations.Add(location);

            await _state.WriteStateAsync();
        }

        public async Task Vote(string user, string location)
        {
            if (_state.State.Locations.Contains(location) &&
                !_state.State.Votes.ContainsKey(user))
            {
                _state.State.Votes.Add(user, location);
                await _state.WriteStateAsync();
            }
            else
            {
                throw new InvalidOperationException("Invalid vote or user has already voted.");
            }
        }

        public Task<bool> ExistsAsync()
        {
            return Task.FromResult(_state.State.Locations != null && _state.State.Locations.Count > 0);
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
