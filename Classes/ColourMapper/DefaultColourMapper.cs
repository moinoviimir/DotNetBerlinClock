namespace BerlinClock.Classes.ColourMapper
{
    internal sealed class DefaultColourMapper : ColourMapperBase
    {
        protected override string OffColour { get; } = "O";
        protected override string MinorColour { get; } = "Y";
        protected override string MajorColour { get; } = "R";
    }
}
