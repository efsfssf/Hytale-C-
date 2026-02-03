using System;
using System.Diagnostics;
using HytaleClient.Protocol;

namespace HytaleClient.Math
{
	// Token: 0x020007E5 RID: 2021
	[DebuggerDisplay("{DebugDisplayString,nq}")]
	public struct IntVector3 : IEquatable<IntVector3>
	{
		// Token: 0x17000FB6 RID: 4022
		// (get) Token: 0x060035D5 RID: 13781 RVA: 0x000643B0 File Offset: 0x000625B0
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
					this.Z.ToString()
				});
			}
		}

		// Token: 0x17000FB7 RID: 4023
		// (get) Token: 0x060035D6 RID: 13782 RVA: 0x00064408 File Offset: 0x00062608
		public static IntVector3 Zero
		{
			get
			{
				return IntVector3.zero;
			}
		}

		// Token: 0x17000FB8 RID: 4024
		// (get) Token: 0x060035D7 RID: 13783 RVA: 0x00064420 File Offset: 0x00062620
		public static IntVector3 One
		{
			get
			{
				return IntVector3.one;
			}
		}

		// Token: 0x17000FB9 RID: 4025
		// (get) Token: 0x060035D8 RID: 13784 RVA: 0x00064438 File Offset: 0x00062638
		public static IntVector3 UnitX
		{
			get
			{
				return IntVector3.unitX;
			}
		}

		// Token: 0x17000FBA RID: 4026
		// (get) Token: 0x060035D9 RID: 13785 RVA: 0x00064450 File Offset: 0x00062650
		public static IntVector3 UnitY
		{
			get
			{
				return IntVector3.unitY;
			}
		}

		// Token: 0x17000FBB RID: 4027
		// (get) Token: 0x060035DA RID: 13786 RVA: 0x00064468 File Offset: 0x00062668
		public static IntVector3 UnitZ
		{
			get
			{
				return IntVector3.unitZ;
			}
		}

		// Token: 0x17000FBC RID: 4028
		// (get) Token: 0x060035DB RID: 13787 RVA: 0x00064480 File Offset: 0x00062680
		public static IntVector3 Up
		{
			get
			{
				return IntVector3.up;
			}
		}

		// Token: 0x17000FBD RID: 4029
		// (get) Token: 0x060035DC RID: 13788 RVA: 0x00064498 File Offset: 0x00062698
		public static IntVector3 Down
		{
			get
			{
				return IntVector3.down;
			}
		}

		// Token: 0x17000FBE RID: 4030
		// (get) Token: 0x060035DD RID: 13789 RVA: 0x000644B0 File Offset: 0x000626B0
		public static IntVector3 Right
		{
			get
			{
				return IntVector3.right;
			}
		}

		// Token: 0x17000FBF RID: 4031
		// (get) Token: 0x060035DE RID: 13790 RVA: 0x000644C8 File Offset: 0x000626C8
		public static IntVector3 Left
		{
			get
			{
				return IntVector3.left;
			}
		}

		// Token: 0x17000FC0 RID: 4032
		// (get) Token: 0x060035DF RID: 13791 RVA: 0x000644E0 File Offset: 0x000626E0
		public static IntVector3 Forward
		{
			get
			{
				return IntVector3.forward;
			}
		}

		// Token: 0x17000FC1 RID: 4033
		// (get) Token: 0x060035E0 RID: 13792 RVA: 0x000644F8 File Offset: 0x000626F8
		public static IntVector3 Backward
		{
			get
			{
				return IntVector3.backward;
			}
		}

		// Token: 0x17000FC2 RID: 4034
		// (get) Token: 0x060035E1 RID: 13793 RVA: 0x00064510 File Offset: 0x00062710
		public static IntVector3 Min
		{
			get
			{
				return IntVector3.min;
			}
		}

		// Token: 0x17000FC3 RID: 4035
		// (get) Token: 0x060035E2 RID: 13794 RVA: 0x00064528 File Offset: 0x00062728
		public static IntVector3 Max
		{
			get
			{
				return IntVector3.max;
			}
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x0006453F File Offset: 0x0006273F
		public IntVector3(int x, int y, int z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x060035E4 RID: 13796 RVA: 0x00064557 File Offset: 0x00062757
		public IntVector3(Vector3 v)
		{
			this.X = (int)Math.Floor((double)v.X);
			this.Y = (int)Math.Floor((double)v.Y);
			this.Z = (int)Math.Floor((double)v.Z);
		}

		// Token: 0x060035E5 RID: 13797 RVA: 0x00064593 File Offset: 0x00062793
		public IntVector3(int value)
		{
			this.X = value;
			this.Y = value;
			this.Z = value;
		}

		// Token: 0x060035E6 RID: 13798 RVA: 0x000645AC File Offset: 0x000627AC
		public Vector3 ToVector3()
		{
			return new Vector3((float)this.X, (float)this.Y, (float)this.Z);
		}

		// Token: 0x060035E7 RID: 13799 RVA: 0x000645D8 File Offset: 0x000627D8
		public BlockPosition ToBlockPosition()
		{
			return new BlockPosition(this.X, this.Y, this.Z);
		}

		// Token: 0x060035E8 RID: 13800 RVA: 0x00064604 File Offset: 0x00062804
		public override bool Equals(object obj)
		{
			bool result;
			if (obj is IntVector3)
			{
				IntVector3 other = (IntVector3)obj;
				result = this.Equals(other);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060035E9 RID: 13801 RVA: 0x0006462A File Offset: 0x0006282A
		public bool Equals(IntVector3 other)
		{
			return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
		}

		// Token: 0x060035EA RID: 13802 RVA: 0x0006465C File Offset: 0x0006285C
		public override int GetHashCode()
		{
			int num = -307843816;
			num = num * -1521134295 + this.X.GetHashCode();
			num = num * -1521134295 + this.Y.GetHashCode();
			return num * -1521134295 + this.Z.GetHashCode();
		}

		// Token: 0x060035EB RID: 13803 RVA: 0x000646B4 File Offset: 0x000628B4
		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}: {3}, {4}: {5}", new object[]
			{
				"X",
				this.X,
				"Y",
				this.Y,
				"Z",
				this.Z
			});
		}

		// Token: 0x060035EC RID: 13804 RVA: 0x00064718 File Offset: 0x00062918
		public IntVector3 Subtract(int x, int y, int z)
		{
			return new IntVector3(this.X - x, this.Y - y, this.Z - z);
		}

		// Token: 0x060035ED RID: 13805 RVA: 0x00064748 File Offset: 0x00062948
		public static implicit operator Vector3(IntVector3 v)
		{
			return new Vector3((float)v.X, (float)v.Y, (float)v.Z);
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x00064774 File Offset: 0x00062974
		public static bool operator ==(IntVector3 value1, IntVector3 value2)
		{
			return value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z;
		}

		// Token: 0x060035EF RID: 13807 RVA: 0x000647B4 File Offset: 0x000629B4
		public static bool operator !=(IntVector3 value1, IntVector3 value2)
		{
			return !(value1 == value2);
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x000647D0 File Offset: 0x000629D0
		public static IntVector3 operator +(IntVector3 value1, IntVector3 value2)
		{
			value1.X += value2.X;
			value1.Y += value2.Y;
			value1.Z += value2.Z;
			return value1;
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x00064818 File Offset: 0x00062A18
		public static IntVector3 operator -(IntVector3 value1, IntVector3 value2)
		{
			value1.X -= value2.X;
			value1.Y -= value2.Y;
			value1.Z -= value2.Z;
			return value1;
		}

		// Token: 0x040017EF RID: 6127
		private static IntVector3 zero = new IntVector3(0, 0, 0);

		// Token: 0x040017F0 RID: 6128
		private static readonly IntVector3 one = new IntVector3(1, 1, 1);

		// Token: 0x040017F1 RID: 6129
		private static readonly IntVector3 unitX = new IntVector3(1, 0, 0);

		// Token: 0x040017F2 RID: 6130
		private static readonly IntVector3 unitY = new IntVector3(0, 1, 0);

		// Token: 0x040017F3 RID: 6131
		private static readonly IntVector3 unitZ = new IntVector3(0, 0, 1);

		// Token: 0x040017F4 RID: 6132
		private static readonly IntVector3 up = new IntVector3(0, 1, 0);

		// Token: 0x040017F5 RID: 6133
		private static readonly IntVector3 down = new IntVector3(0, -1, 0);

		// Token: 0x040017F6 RID: 6134
		private static readonly IntVector3 right = new IntVector3(1, 0, 0);

		// Token: 0x040017F7 RID: 6135
		private static readonly IntVector3 left = new IntVector3(-1, 0, 0);

		// Token: 0x040017F8 RID: 6136
		private static readonly IntVector3 forward = new IntVector3(0, 0, -1);

		// Token: 0x040017F9 RID: 6137
		private static readonly IntVector3 backward = new IntVector3(0, 0, 1);

		// Token: 0x040017FA RID: 6138
		private static readonly IntVector3 min = new IntVector3(int.MinValue, int.MinValue, int.MinValue);

		// Token: 0x040017FB RID: 6139
		private static readonly IntVector3 max = new IntVector3(int.MaxValue, int.MaxValue, int.MaxValue);

		// Token: 0x040017FC RID: 6140
		public int X;

		// Token: 0x040017FD RID: 6141
		public int Y;

		// Token: 0x040017FE RID: 6142
		public int Z;
	}
}
