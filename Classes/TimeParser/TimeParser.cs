using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerlinClock.Classes.TimeParser
{
    public interface ITimeParser
    {
        LampTimeModel Parse(string rawTimestamp = "HH:mm:ss");
    }

    internal sealed class TimeParser : ITimeParser
    {
        const string DefaultTimeFormat = "HH:mm:ss";
        private static readonly string TimeFormat;

        public TimeParser(string timeFormat = TimeFormat)
        {
            this.TimeFormat = timeFormat;
        }


        public LampTimeModel Parse(string rawTimestamp)
        {
            DateTime timestamp;
            try
            {
                timestamp = DateTime.ParseExact(rawTimestamp, TimeFormat, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Couldn't parse the timestamp. Expected format is {TimeFormat}", ex);
            }

            var secondsAreEven = timestamp.Seconds % 2;
            var fiveHourLampCount = timestamp.Hours / 5;
            var hourLampsCount = timestamp.Hours % fiveHourLampCount;
            var fiveMinuteLampCount = timestamp.Minutes / 5;
            var minuteLampCount = timestamp.Minutes % fiveMinuteLampCount;

            var fiveHourArray = ConvertToLampArray(fiveHourLampCount, 4);
            var hourArray = ConvertToLampArray(hourLampsCount, 4);
            var fiveMinuteArray = ConvertToLampArray(fiveMinuteLampCount, 11);
            var minuteLampArray = ConvertToLampArray(minuteLampCount, 4);

            return new LampTimeModel(secondsAreEven, fiveHourArray, hourArray, fiveMinuteArray, minuteLampArray);
        }

        private static ICollection<bool> ConvertToLampArray(int value, int totalCount)
        {
            var result = new bool[totalCount];
            for (int i; i < value; i++)
            {
                result[i] = true;
            }
            return result;
        }
    }

    public interface IColourMapper
    {
        string MapToLampString(LampTimeModel model);
    }

    internal sealed class DefaultColourMapper : IColourMapper
    {
        private static readonly string MainColour = "Y";
        private static readonly string QuarterColour = "R";

        private static string BoolToColour(bool value, string colour = MainColour) => value && colour;

        private static string BoolToQuarterColour(bool value, int position) => i % 3 == 0 ? BoolToColour(value, QuarterColour) : BoolToColour(value);

        public string MapToLampString(LampTimeModel model)
        {
            var builder = new StringBuilder();

            builder.AppendLine(BoolToColour(model.SecondsAreEven));
            builder.AppendLine(model.FiveHourLamps.Select(BoolToColour));
            builder.AppendLine(model.HourLamps.Select(BoolToColour));
            builder.AppendLine(FiveMinuteLamps.Select(BoolToQuarterColour));
            builder.AppendLine(MinuteLamps.Select(BoolToColour));

            return builder.ToString();
        }
    }

    internal readonly struct LampTimeModel
    {
        public bool SecondsAreEven { get; }
        public IReadOnlyCollection<bool> FiveHourLamps { get; }
        public IReadOnlyCollection<bool> HourLamps { get; }
        public IReadOnlyCollection<bool> FiveMinuteLamps { get; }
        public IReadOnlyCollection<bool> MinuteLamps { get; }

        private readonly int _fiveHourLampCount;

        public LampTimeModel(bool secondsAreEven, ICollection<bool> fiveHourLamps, ICollection<bool> hourLamps,
            ICollection<bool> fiveMinuteLamps, ICollection<bool> minuteLamps)
        {
            SecondsAreEven = secondsAreEven;
            FiveHourLamps = fiveHourLamps;
            HourLamps = hourLamps;
            FiveMinuteLamps = fiveMinuteLamps;
            MinuteLamps = minuteLamps;
        }
    }
}
