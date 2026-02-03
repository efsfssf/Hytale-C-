using System;
using System.Diagnostics;

namespace HytaleClient.Math
{
	// Token: 0x020007F1 RID: 2033
	[DebuggerDisplay("{DebugDisplayString,nq}")]
	[Serializable]
	public struct Point : IEquatable<Point>
	{
		// Token: 0x17000FD8 RID: 4056
		// (get) Token: 0x060036DF RID: 14047 RVA: 0x0006BCF0 File Offset: 0x00069EF0
		public static Point Zero
		{
			get
			{
				return Point.zeroPoint;
			}
		}

		// Token: 0x17000FD9 RID: 4057
		// (get) Token: 0x060036E0 RID: 14048 RVA: 0x0006BD08 File Offset: 0x00069F08
		internal string DebugDisplayString
		{
			get
			{
				return this.X.ToString() + " " + this.Y.ToString();
			}
		}

		// Token: 0x060036E1 RID: 14049 RVA: 0x0006BD3A File Offset: 0x00069F3A
		public Point(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x060036E2 RID: 14050 RVA: 0x0006BD4C File Offset: 0x00069F4C
		public bool Equals(Point other)
		{
			return this.X == other.X && this.Y == other.Y;
		}

		// Token: 0x060036E3 RID: 14051 RVA: 0x0006BD80 File Offset: 0x00069F80
		public override bool Equals(object obj)
		{
			return obj is Point && this.Equals((Point)obj);
		}

		// Token: 0x060036E4 RID: 14052 RVA: 0x0006BDAC File Offset: 0x00069FAC
		public override int GetHashCode()
		{
			return this.X ^ this.Y;
		}

		// Token: 0x060036E5 RID: 14053 RVA: 0x0006BDCC File Offset: 0x00069FCC
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{X:",
				this.X.ToString(),
				" Y:",
				this.Y.ToString(),
				"}"
			});
		}

		// Token: 0x060036E6 RID: 14054 RVA: 0x0006BE20 File Offset: 0x0006A020
		public static Point operator +(Point value1, Point value2)
		{
			return new Point(value1.X + value2.X, value1.Y + value2.Y);
		}

		// Token: 0x060036E7 RID: 14055 RVA: 0x0006BE54 File Offset: 0x0006A054
		public static Point operator -(Point value1, Point value2)
		{
			return new Point(value1.X - value2.X, value1.Y - value2.Y);
		}

		// Token: 0x060036E8 RID: 14056 RVA: 0x0006BE88 File Offset: 0x0006A088
		public static Point operator *(Point value1, Point value2)
		{
			return new Point(value1.X * value2.X, value1.Y * value2.Y);
		}

		// Token: 0x060036E9 RID: 14057 RVA: 0x0006BEBC File Offset: 0x0006A0BC
		public static Point operator /(Point value1, Point value2)
		{
			return new Point(value1.X / value2.X, value1.Y / value2.Y);
		}

		// Token: 0x060036EA RID: 14058 RVA: 0x0006BEF0 File Offset: 0x0006A0F0
		public static bool operator ==(Point a, Point b)
		{
			return a.Equals(b);
		}

		// Token: 0x060036EB RID: 14059 RVA: 0x0006BF0C File Offset: 0x0006A10C
		public static bool operator !=(Point a, Point b)
		{
			return !a.Equals(b);
		}

		// Token: 0x0400183A RID: 6202
		public int X;

		// Token: 0x0400183B RID: 6203
		public int Y;

		// Token: 0x0400183C RID: 6204
		private static readonly Point zeroPoint = default(Point);
	}
}
