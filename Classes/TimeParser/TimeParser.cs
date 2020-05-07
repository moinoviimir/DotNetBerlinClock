using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly string TimeFormat;

        public TimeParser(string timeFormat = DefaultTimeFormat)
        {
            TimeFormat = timeFormat;
        }


        public LampTimeModel Parse(string rawTimestamp)
        {
            // Please see solution.md clarification on this bit.
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

    public interface IColourMapper
    {
        string MinorColour { get; }
        string MajorColour { get; }
        string MapToLampString(LampTimeModel model);
    }

    internal sealed class DefaultColourMapper : IColourMapper
    {
        private const string OffColour = "O";
        public string MinorColour { get; } = "Y";
        public string MajorColour { get; } = "R";

        private static string BoolToColour(bool value, string colour) => value ? colour : OffColour;
        private string BoolToMinorColour(bool value) => BoolToColour(value, MinorColour);
        private string BoolToMajorColour(bool value) => BoolToColour(value, MajorColour);
        private string BoolToQuarterColour(bool value, int i) => (i + 1) % 3 == 0 ? BoolToMajorColour(value) : BoolToMinorColour(value);

        public string MapToLampString(LampTimeModel model)
        {
            var builder = new StringBuilder();

            builder.AppendLine(BoolToMinorColour(model.SecondsAreEven));

            model.FiveHourLamps.Select(BoolToMajorColour).ToList().ForEach(el => builder.Append(el));
            builder.AppendLine();
            model.HourLamps.Select(BoolToMajorColour).ToList().ForEach(el => builder.Append(el));
            builder.AppendLine();
            model.FiveMinuteLamps.Select(BoolToQuarterColour).ToList().ForEach(el => builder.Append(el));
            builder.AppendLine();
            model.MinuteLamps.Select(BoolToMinorColour).ToList().ForEach(el => builder.Append(el));

            return builder.ToString();
        }

        private static StringBuilder AppendBooleanTransform(StringBuilder sb, IEnumerable<bool> array, Func<bool, string> selector)
        {
            var sequence = array.Select(selector);
            foreach (var element in sequence)
            {
                sb.Append(element);
            }
            sb.AppendLine();
            return sb;
        }
    }

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
