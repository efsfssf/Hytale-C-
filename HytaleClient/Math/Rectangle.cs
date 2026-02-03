using System;
using System.Diagnostics;

namespace HytaleClient.Math
{
	// Token: 0x020007F6 RID: 2038
	[DebuggerDisplay("{DebugDisplayString,nq}")]
	[Serializable]
	public struct Rectangle : IEquatable<Rectangle>
	{
		// Token: 0x17000FDD RID: 4061
		// (get) Token: 0x0600373A RID: 14138 RVA: 0x0006DD98 File Offset: 0x0006BF98
		public int Left
		{
			get
			{
				return this.X;
			}
		}

		// Token: 0x17000FDE RID: 4062
		// (get) Token: 0x0600373B RID: 14139 RVA: 0x0006DDB0 File Offset: 0x0006BFB0
		public int Right
		{
			get
			{
				return this.X + this.Width;
			}
		}

		// Token: 0x17000FDF RID: 4063
		// (get) Token: 0x0600373C RID: 14140 RVA: 0x0006DDD0 File Offset: 0x0006BFD0
		public int Top
		{
			get
			{
				return this.Y;
			}
		}

		// Token: 0x17000FE0 RID: 4064
		// (get) Token: 0x0600373D RID: 14141 RVA: 0x0006DDE8 File Offset: 0x0006BFE8
		public int Bottom
		{
			get
			{
				return this.Y + this.Height;
			}
		}

		// Token: 0x17000FE1 RID: 4065
		// (get) Token: 0x0600373E RID: 14142 RVA: 0x0006DE08 File Offset: 0x0006C008
		// (set) Token: 0x0600373F RID: 14143 RVA: 0x0006DE2B File Offset: 0x0006C02B
		public Point Location
		{
			get
			{
				return new Point(this.X, this.Y);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
			}
		}

		// Token: 0x17000FE2 RID: 4066
		// (get) Token: 0x06003740 RID: 14144 RVA: 0x0006DE48 File Offset: 0x0006C048
		public Point Center
		{
			get
			{
				return new Point(this.X + this.Width / 2, this.Y + this.Height / 2);
			}
		}

		// Token: 0x17000FE3 RID: 4067
		// (get) Token: 0x06003741 RID: 14145 RVA: 0x0006DE80 File Offset: 0x0006C080
		public bool IsEmpty
		{
			get
			{
				return this.Width == 0 && this.Height == 0 && this.X == 0 && this.Y == 0;
			}
		}

		// Token: 0x17000FE4 RID: 4068
		// (get) Token: 0x06003742 RID: 14146 RVA: 0x0006DEB8 File Offset: 0x0006C0B8
		public static Rectangle Empty
		{
			get
			{
				return Rectangle.emptyRectangle;
			}
		}

		// Token: 0x17000FE5 RID: 4069
		// (get) Token: 0x06003743 RID: 14147 RVA: 0x0006DED0 File Offset: 0x0006C0D0
		internal string DebugDisplayString
		{
			get
			{
				return string.Concat(new string[]
				{
					this.X.ToString(),
					" ",
					this.Y.ToString(),
					" ",
					this.Width.ToString(),
					" ",
					this.Height.ToString()
				});
			}
		}

		// Token: 0x06003744 RID: 14148 RVA: 0x0006DF3D File Offset: 0x0006C13D
		public Rectangle(int x, int y, int width, int height)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
		}

		// Token: 0x06003745 RID: 14149 RVA: 0x0006DF60 File Offset: 0x0006C160
		public bool Contains(int x, int y)
		{
			return this.X <= x && x < this.X + this.Width && this.Y <= y && y < this.Y + this.Height;
		}

		// Token: 0x06003746 RID: 14150 RVA: 0x0006DFA8 File Offset: 0x0006C1A8
		public bool Contains(Point value)
		{
			return this.X <= value.X && value.X < this.X + this.Width && this.Y <= value.Y && value.Y < this.Y + this.Height;
		}

		// Token: 0x06003747 RID: 14151 RVA: 0x0006E004 File Offset: 0x0006C204
		public bool Contains(Rectangle value)
		{
			return this.X <= value.X && value.X + value.Width <= this.X + this.Width && this.Y <= value.Y && value.Y + value.Height <= this.Y + this.Height;
		}

		// Token: 0x06003748 RID: 14152 RVA: 0x0006E070 File Offset: 0x0006C270
		public void Contains(ref Point value, out bool result)
		{
			result = (this.X <= value.X && value.X < this.X + this.Width && this.Y <= value.Y && value.Y < this.Y + this.Height);
		}

		// Token: 0x06003749 RID: 14153 RVA: 0x0006E0CC File Offset: 0x0006C2CC
		public void Contains(ref Rectangle value, out bool result)
		{
			result = (this.X <= value.X && value.X + value.Width <= this.X + this.Width && this.Y <= value.Y && value.Y + value.Height <= this.Y + this.Height);
		}

		// Token: 0x0600374A RID: 14154 RVA: 0x0006E136 File Offset: 0x0006C336
		public void Offset(Point offset)
		{
			this.X += offset.X;
			this.Y += offset.Y;
		}

		// Token: 0x0600374B RID: 14155 RVA: 0x0006E15F File Offset: 0x0006C35F
		public void Offset(int offsetX, int offsetY)
		{
			this.X += offsetX;
			this.Y += offsetY;
		}

		// Token: 0x0600374C RID: 14156 RVA: 0x0006E17E File Offset: 0x0006C37E
		public void Inflate(int horizontalValue, int verticalValue)
		{
			this.X -= horizontalValue;
			this.Y -= verticalValue;
			this.Width += horizontalValue * 2;
			this.Height += verticalValue * 2;
		}

		// Token: 0x0600374D RID: 14157 RVA: 0x0006E1C0 File Offset: 0x0006C3C0
		public bool Equals(Rectangle other)
		{
			return this == other;
		}

		// Token: 0x0600374E RID: 14158 RVA: 0x0006E1E0 File Offset: 0x0006C3E0
		public override bool Equals(object obj)
		{
			return obj is Rectangle && this == (Rectangle)obj;
		}

		// Token: 0x0600374F RID: 14159 RVA: 0x0006E210 File Offset: 0x0006C410
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{X:",
				this.X.ToString(),
				" Y:",
				this.Y.ToString(),
				" Width:",
				this.Width.ToString(),
				" Height:",
				this.Height.ToString(),
				"}"
			});
		}

		// Token: 0x06003750 RID: 14160 RVA: 0x0006E290 File Offset: 0x0006C490
		public override int GetHashCode()
		{
			return this.X ^ this.Y ^ this.Width ^ this.Height;
		}

		// Token: 0x06003751 RID: 14161 RVA: 0x0006E2C0 File Offset: 0x0006C4C0
		public bool Intersects(Rectangle value)
		{
			return value.Left < this.Right && this.Left < value.Right && value.Top < this.Bottom && this.Top < value.Bottom;
		}

		// Token: 0x06003752 RID: 14162 RVA: 0x0006E311 File Offset: 0x0006C511
		public void Intersects(ref Rectangle value, out bool result)
		{
			result = (value.Left < this.Right && this.Left < value.Right && value.Top < this.Bottom && this.Top < value.Bottom);
		}

		// Token: 0x06003753 RID: 14163 RVA: 0x0006E354 File Offset: 0x0006C554
		public static bool operator ==(Rectangle a, Rectangle b)
		{
			return a.X == b.X && a.Y == b.Y && a.Width == b.Width && a.Height == b.Height;
		}

		// Token: 0x06003754 RID: 14164 RVA: 0x0006E3A4 File Offset: 0x0006C5A4
		public static bool operator !=(Rectangle a, Rectangle b)
		{
			return !(a == b);
		}

		// Token: 0x06003755 RID: 14165 RVA: 0x0006E3C0 File Offset: 0x0006C5C0
		public static Rectangle Intersect(Rectangle value1, Rectangle value2)
		{
			Rectangle result;
			Rectangle.Intersect(ref value1, ref value2, out result);
			return result;
		}

		// Token: 0x06003756 RID: 14166 RVA: 0x0006E3E0 File Offset: 0x0006C5E0
		public static void Intersect(ref Rectangle value1, ref Rectangle value2, out Rectangle result)
		{
			bool flag = value1.Intersects(value2);
			if (flag)
			{
				int num = Math.Min(value1.X + value1.Width, value2.X + value2.Width);
				int num2 = Math.Max(value1.X, value2.X);
				int num3 = Math.Max(value1.Y, value2.Y);
				int num4 = Math.Min(value1.Y + value1.Height, value2.Y + value2.Height);
				result = new Rectangle(num2, num3, num - num2, num4 - num3);
			}
			else
			{
				result = new Rectangle(0, 0, 0, 0);
			}
		}

		// Token: 0x06003757 RID: 14167 RVA: 0x0006E48C File Offset: 0x0006C68C
		public static Rectangle Union(Rectangle value1, Rectangle value2)
		{
			int num = Math.Min(value1.X, value2.X);
			int num2 = Math.Min(value1.Y, value2.Y);
			return new Rectangle(num, num2, Math.Max(value1.Right, value2.Right) - num, Math.Max(value1.Bottom, value2.Bottom) - num2);
		}

		// Token: 0x06003758 RID: 14168 RVA: 0x0006E4F4 File Offset: 0x0006C6F4
		public static void Union(ref Rectangle value1, ref Rectangle value2, out Rectangle result)
		{
			result.X = Math.Min(value1.X, value2.X);
			result.Y = Math.Min(value1.Y, value2.Y);
			result.Width = Math.Max(value1.Right, value2.Right) - result.X;
			result.Height = Math.Max(value1.Bottom, value2.Bottom) - result.Y;
		}

		// Token: 0x04001844 RID: 6212
		public int X;

		// Token: 0x04001845 RID: 6213
		public int Y;

		// Token: 0x04001846 RID: 6214
		public int Width;

		// Token: 0x04001847 RID: 6215
		public int Height;

		// Token: 0x04001848 RID: 6216
		private static Rectangle emptyRectangle = default(Rectangle);
	}
}
