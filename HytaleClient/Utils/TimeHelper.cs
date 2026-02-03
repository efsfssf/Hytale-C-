using System;
using System.Diagnostics;
using HytaleClient.Protocol;

namespace HytaleClient.Utils
{
	// Token: 0x020007D4 RID: 2004
	public static class TimeHelper
	{
		// Token: 0x0600343D RID: 13373 RVA: 0x00053D28 File Offset: 0x00051F28
		public static long GetEpochMilliseconds(DateTime? time = null)
		{
			return (long)((time ?? DateTime.UtcNow) - TimeHelper.EpochDateTime).TotalMilliseconds;
		}

		// Token: 0x0600343E RID: 13374 RVA: 0x00053D68 File Offset: 0x00051F68
		public static long GetEpochSeconds(DateTime? time = null)
		{
			return (long)((time ?? DateTime.UtcNow) - TimeHelper.EpochDateTime).TotalSeconds;
		}

		// Token: 0x0600343F RID: 13375 RVA: 0x00053DA8 File Offset: 0x00051FA8
		public static DateTime InstantDataToDateTime(InstantData instantData)
		{
			TimeSpan t = TimeSpan.FromTicks(instantData.Seconds * 10000000L + (long)instantData.Nanos / 100L);
			return TimeHelper.EpochDateTime + t;
		}

		// Token: 0x06003440 RID: 13376 RVA: 0x00053DE4 File Offset: 0x00051FE4
		public static InstantData DateTimeToInstantData(DateTime dateTime)
		{
			TimeSpan timeSpan = dateTime - TimeHelper.EpochDateTime;
			long num = (long)timeSpan.TotalSeconds;
			long num2 = timeSpan.Ticks - num * 10000000L;
			return new InstantData(num, (int)(num2 * 100L));
		}

		// Token: 0x06003441 RID: 13377 RVA: 0x00053E28 File Offset: 0x00052028
		public static string FormatMillis(long millis)
		{
			long num = millis / 1000L;
			millis %= 1000L;
			bool flag = num > 0L;
			string result;
			if (flag)
			{
				result = num.ToString() + "s " + millis.ToString() + "ms";
			}
			else
			{
				result = millis.ToString() + "ms";
			}
			return result;
		}

		// Token: 0x06003442 RID: 13378 RVA: 0x00053E88 File Offset: 0x00052088
		public static string FormatNanos(long nanos)
		{
			long num = nanos / 1000000000L;
			nanos %= 1000000000L;
			long num2 = nanos / 1000000L;
			nanos %= 1000000L;
			bool flag = num > 0L;
			string result;
			if (flag)
			{
				result = string.Concat(new string[]
				{
					num.ToString(),
					"s ",
					num2.ToString(),
					"ms ",
					nanos.ToString(),
					"ns"
				});
			}
			else
			{
				bool flag2 = num2 > 0L;
				if (flag2)
				{
					result = num2.ToString() + "ms " + nanos.ToString() + "ns";
				}
				else
				{
					result = nanos.ToString() + "ns";
				}
			}
			return result;
		}

		// Token: 0x06003443 RID: 13379 RVA: 0x00053F4C File Offset: 0x0005214C
		public static string FormatMicros(long micros)
		{
			long num = micros / 1000000L;
			micros %= 1000000L;
			long num2 = micros / 1000L;
			micros %= 1000L;
			bool flag = num > 0L;
			string result;
			if (flag)
			{
				result = string.Concat(new string[]
				{
					num.ToString(),
					"s ",
					num2.ToString(),
					"ms ",
					micros.ToString(),
					"µs"
				});
			}
			else
			{
				bool flag2 = num2 > 0L;
				if (flag2)
				{
					result = num2.ToString() + "ms " + micros.ToString() + "µs";
				}
				else
				{
					result = micros.ToString() + "µs";
				}
			}
			return result;
		}

		// Token: 0x06003444 RID: 13380 RVA: 0x00054010 File Offset: 0x00052210
		public static string FormatTicks(long ticks)
		{
			return TimeHelper.FormatMicros(ticks * (Stopwatch.Frequency / 1000000L));
		}

		// Token: 0x06003445 RID: 13381 RVA: 0x00054038 File Offset: 0x00052238
		public static float GetDayProgressInHours(DateTime dateTime)
		{
			return (float)dateTime.Hour + (float)dateTime.Minute / 60f + (float)dateTime.Second / 3600f + (float)dateTime.Millisecond / 3600000f;
		}

		// Token: 0x06003446 RID: 13382 RVA: 0x00054080 File Offset: 0x00052280
		public static DateTime IncrementDateTimeBySeconds(DateTime dateTime, float seconds, int secondsPerDay)
		{
			long num = (long)((double)seconds * (86400.0 / (double)secondsPerDay) * 10000000.0);
			long num2 = dateTime.Ticks + num;
			bool flag = num2 < TimeHelper.ZeroYear.Ticks;
			DateTime result;
			if (flag)
			{
				result = TimeHelper.MaxTime - TimeSpan.FromTicks(TimeHelper.ZeroYear.Ticks - num2);
			}
			else
			{
				bool flag2 = num2 > TimeHelper.MaxTime.Ticks;
				if (flag2)
				{
					result = TimeHelper.ZeroYear + TimeSpan.FromTicks(TimeHelper.MaxTime.Ticks - num2);
				}
				else
				{
					result = dateTime + TimeSpan.FromTicks(num);
				}
			}
			return result;
		}

		// Token: 0x0400176C RID: 5996
		public const long NanosPerTick = 100L;

		// Token: 0x0400176D RID: 5997
		public const long NanosPerMillisecond = 1000000L;

		// Token: 0x0400176E RID: 5998
		public const long NanosPerSecond = 1000000000L;

		// Token: 0x0400176F RID: 5999
		public const long MicrosPerMillisecond = 1000L;

		// Token: 0x04001770 RID: 6000
		public const long MicrosPerSecond = 1000000L;

		// Token: 0x04001771 RID: 6001
		public const long MillisPerSecond = 1000L;

		// Token: 0x04001772 RID: 6002
		public const int SecondsPerDay = 86400;

		// Token: 0x04001773 RID: 6003
		public const float HoursPerDay = 24f;

		// Token: 0x04001774 RID: 6004
		public static readonly DateTime EpochDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x04001775 RID: 6005
		public static readonly DateTime ZeroYear = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x04001776 RID: 6006
		public static readonly DateTime MaxTime = DateTime.MaxValue;
	}
}
