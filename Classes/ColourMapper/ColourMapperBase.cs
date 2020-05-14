using System.Linq;
using System.Text;

namespace BerlinClock.Classes.ColourMapper
{
    public abstract class ColourMapperBase
    {
        protected abstract string OffColour { get; }
        protected abstract string MinorColour { get; }
        protected abstract string MajorColour { get; }

        protected string BoolToColour(bool value, string colour) => value ? colour : OffColour;
        protected string BoolToMinorColour(bool value) => BoolToColour(value, MinorColour);
        protected string BoolToMajorColour(bool value) => BoolToColour(value, MajorColour);
        protected string BoolToQuarterColour(bool value, int i) => (i + 1) % 3 == 0 ? BoolToMajorColour(value) : BoolToMinorColour(value);

        public virtual string MapToLampString(LampTimeModel model)
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
    }
}
