namespace LunchApp.Grains.States
{
    [GenerateSerializer]
    public record ClockState
    {
        [Id(0)]
        public DateTime? OverriddenTime { get; set; }

        [Id(1)]
        public DateTime? OverriddenTimeSetAt { get; set; }
    }
}
