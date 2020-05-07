using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BerlinClock.Classes.ColourMapper
{
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
}
