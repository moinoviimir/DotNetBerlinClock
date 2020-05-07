namespace BerlinClock.Classes.ColourMapper
{
    public interface IColourMapper
    {
        string MinorColour { get; }
        string MajorColour { get; }
        string MapToLampString(LampTimeModel model);
    }
}
