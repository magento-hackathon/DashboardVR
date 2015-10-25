using System;
using System.Text.RegularExpressions;

public class TimestampHelper
{
	public DateTime from;
	public DateTime to;

	public TimestampHelper (string fromToTimestamp)
	{
		string[] parts = Regex.Split(fromToTimestamp, "-");

		if (parts.Length != 2) {
			throw new ArgumentException("Invalid input: " + fromToTimestamp);
		}

		from = TimestampHelper.parseTimestamp (parts [0]);
		to = TimestampHelper.parseTimestamp (parts [1]);
	}

	public static TimestampHelper Instance (string fromToTimestamp)
	{
		TimestampHelper helper = new TimestampHelper (fromToTimestamp);
		return helper;
	}

	public static DateTime parseTimestamp (string timestamp)
	{
		DateTime date = (new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                .AddSeconds (double.Parse (timestamp));
		// , CultureInfo.InvariantCulture

		return date;
	}

	public string FormatFromToStr ()
	{
		return this.from.ToShortDateString () + " - " + this.to.ToShortDateString ();
	}

}

