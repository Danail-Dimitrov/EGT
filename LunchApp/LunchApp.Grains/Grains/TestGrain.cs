using LunchApp.Grains.Abstractions;

namespace LunchApp.Grains.Grains
{
    public class TestGrain : Grain, ITestGrain
    {
        private int num;

        public async Task<int> GetValue()
        {
            return num;
        }

        public async Task Init(int num)
        {
            this.num = num;
        }
    }
}
