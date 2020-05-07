using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BerlinClock.Classes.TimeParser;

namespace BerlinClock
{
    public class TimeConverter : ITimeConverter
    {
        private readonly ITimeParser _timeParser;
        private readonly IColourMapper _colourMapper;

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
