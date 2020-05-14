using BerlinClock.Classes.TimeParser;
using BerlinClock.Classes.ColourMapper;

namespace BerlinClock
{
    public class TimeConverter : ITimeConverter
    {
        private readonly ITimeParser _timeParser;
        private readonly ColourMapperBase _colourMapper;

        public TimeConverter()
        {
            _timeParser = new TimeParser();
            _colourMapper = new DefaultColourMapper();
        }

        public string convertTime(string aTime)
        {
            return _colourMapper.MapToLampString(
                _timeParser.Parse(aTime));
        }
    }
}
