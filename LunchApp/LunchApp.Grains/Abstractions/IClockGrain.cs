namespace LunchApp.Grains.Abstractions
{
    public interface IClockGrain : Orleans.IGrainWithIntegerKey
    {
        // Gets the current time
        Task<DateTime> GetCurrentTime();

        // Sets the current time
        Task SetTime(DateTime time);
        
        // Goes back to system time
        Task ResetTime(); 
    }
}
