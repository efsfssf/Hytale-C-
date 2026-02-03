using System;

namespace HytaleClient.Math
{
	// Token: 0x020007FE RID: 2046
	public struct UShortVector4
	{
		// Token: 0x17001003 RID: 4099
		// (get) Token: 0x0600387C RID: 14460 RVA: 0x00073C28 File Offset: 0x00071E28
		public static UShortVector4 Zero
		{
			get
			{
				return UShortVector4._zero;
			}
		}

		// Token: 0x0600387D RID: 14461 RVA: 0x00073C3F File Offset: 0x00071E3F
		public UShortVector4(ushort x, ushort y, ushort z, ushort w)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.W = w;
		}

		// Token: 0x04001872 RID: 6258
		public ushort X;

		// Token: 0x04001873 RID: 6259
		public ushort Y;

		// Token: 0x04001874 RID: 6260
		public ushort Z;

		// Token: 0x04001875 RID: 6261
		public ushort W;

		// Token: 0x04001876 RID: 6262
		private static UShortVector4 _zero;
	}
}
