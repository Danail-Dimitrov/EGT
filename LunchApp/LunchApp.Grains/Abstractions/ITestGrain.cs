namespace LunchApp.Grains.Abstractions
{
    public interface ITestGrain : IGrainWithIntegerKey
    {
        Task Init(int num);

        Task<int> GetValue();
    }
}
