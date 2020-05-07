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

        public TimeConverter()
        {
            
        }

        public string convertTime(string aTime)
        {
            throw new NotImplementedException();
        }
    }
}
