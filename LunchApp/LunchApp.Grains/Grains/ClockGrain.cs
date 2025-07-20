using LunchApp.Grains.Abstractions;
using LunchApp.Grains.States;

namespace LunchApp.Grains.Grains
{
    public class ClockGrain : Grain, IClockGrain
    {
        private readonly IPersistentState<ClockState> _state;

        // clock -> state name
        // votes -> storage provider name
        public ClockGrain([PersistentState("clock", "lunchapp")] IPersistentState<ClockState> state)
        {
            _state = state;
        }

        public async Task SetTime(DateTime simulatedTime)
        {
            _state.State.OverriddenTime = simulatedTime;
            _state.State.OverriddenTimeSetAt = DateTime.UtcNow;
            await _state.WriteStateAsync();
        }

        public Task<DateTime> GetCurrentTime()
        {
            if (_state.State.OverriddenTime is null || _state.State.OverriddenTimeSetAt is null)
                return Task.FromResult(DateTime.UtcNow);

            var elapsed = DateTime.UtcNow - _state.State.OverriddenTimeSetAt.Value;
            return Task.FromResult(_state.State.OverriddenTime.Value.Add(elapsed));
        }

        public async Task ResetTime()
        {
            _state.State.OverriddenTime = null;
            await _state.WriteStateAsync();
        }
    }
}
