using System.Collections.Generic;

namespace BerlinClock.Classes
{
    public sealed class LampTimeModel
    {
        public bool SecondsAreEven { get; }
        public IEnumerable<bool> FiveHourLamps { get; }
        public IEnumerable<bool> HourLamps { get; }
        public IEnumerable<bool> FiveMinuteLamps { get; }
        public IEnumerable<bool> MinuteLamps { get; }

        public LampTimeModel(bool secondsAreEven, ICollection<bool> fiveHourLamps, ICollection<bool> hourLamps,
            ICollection<bool> fiveMinuteLamps, ICollection<bool> minuteLamps)
        {
            SecondsAreEven = secondsAreEven;
            FiveHourLamps = fiveHourLamps;
            HourLamps = hourLamps;
            FiveMinuteLamps = fiveMinuteLamps;
            MinuteLamps = minuteLamps;
        }

        // the 24:00:00 model represents an unclear business case, and, as such, is handled separately
        public static LampTimeModel TwentyFourHourModel { get; } = new LampTimeModel(
            true,
            new[] { true, true, true, true },
            new[] { true, true, true, true },
            new[] { false, false, false, false, false, false, false, false, false, false, false },
            new[] { false, false, false, false });
    }
}
