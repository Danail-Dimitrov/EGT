namespace LunchApp.Grains.States
{
    [GenerateSerializer]
    public class VoteManagerState
    {
        public const short MAX_LOCATIONS = 5;

        public VoteManagerState()
        {
            Locations = new List<string>(5);
            Votes = new Dictionary<string, string>();
            IsVotingEnabled = true;
        }

        [Id(0)]
        public ICollection<string> Locations { get; set; }

        [Id(2)]
        public IDictionary<string, string> Votes { get; set; }

        [Id(3)]
        public bool IsVotingEnabled { get; set; }
    }
}
