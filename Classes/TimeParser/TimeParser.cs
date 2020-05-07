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
            // Please see solution.md for clarification on this bit.
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
