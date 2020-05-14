using System;
using System.Collections.Generic;
using System.Globalization;

namespace BerlinClock.Classes.TimeParser
{
    internal sealed class TimeParser : ITimeParser
    {
        const string DefaultTimeFormat = "HH:mm:ss";
        private readonly string TimeFormat;

        public TimeParser(string timeFormat = DefaultTimeFormat)
        {
            TimeFormat = timeFormat;
        }

        public LampTimeModel Parse(string rawTimestamp)
        {
            // 24:00:00 is valid from the perspective of ISO-8601, but I would argue it is not a valid state for a clock dial:
            // while for e.g. contractual time boundaries terms like "from 00:00:00 to 24:0:00" are used, they signify an interval,
            // whereas clock dials only ever reflect instants.
            // Ordinarily, I'd talk to business about this asap, as a spec like this indicates that I probably misunderstand the intent of the feature,
            // and the faster I correct it, the better. In the context of a training exercise however, I'll settle for this comment.
            // I'm implementing the logic as an explicit bypass, because once we've talked to business, either this bit of the spec goes away, 
            // or the intent is somewhat reimagined, and a partial rewrite is then needed either way.
            if (string.Equals(rawTimestamp, "24:00:00", StringComparison.InvariantCulture))
            {
                return LampTimeModel.TwentyFourHourModel;
            }

            DateTime timestamp;
            try
            {
                timestamp = DateTime.ParseExact(rawTimestamp, TimeFormat, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Couldn't parse the timestamp. Expected format is {TimeFormat}", ex);
            }

            var secondsAreEven = timestamp.Second % 2 == 0;
            var fiveHourLampCount = timestamp.Hour / 5;
            var hourLampsCount = timestamp.Hour - (fiveHourLampCount * 5);
            var fiveMinuteLampCount = timestamp.Minute / 5;
            var minuteLampCount = timestamp.Minute - (fiveMinuteLampCount * 5);

            var fiveHourArray = ConvertToLampArray(fiveHourLampCount, 4);
            var hourArray = ConvertToLampArray(hourLampsCount, 4);
            var fiveMinuteArray = ConvertToLampArray(fiveMinuteLampCount, 11);
            var minuteLampArray = ConvertToLampArray(minuteLampCount, 4);

            return new LampTimeModel(secondsAreEven, fiveHourArray, hourArray, fiveMinuteArray, minuteLampArray);
        }

        private static ICollection<bool> ConvertToLampArray(int value, int totalCount)
        {
            var result = new bool[totalCount];
            for (int i = 0; i < value; i++)
            {
                result[i] = true;
            }
            return result;
        }
    }
}
