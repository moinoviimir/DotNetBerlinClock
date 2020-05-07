namespace BerlinClock.Classes.TimeParser
{
    public interface ITimeParser
    {
        LampTimeModel Parse(string rawTimestamp = "HH:mm:ss");
    }
}
