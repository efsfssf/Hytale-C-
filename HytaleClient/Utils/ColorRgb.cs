using System;

namespace HytaleClient.Utils
{
	// Token: 0x020007BA RID: 1978
	public struct ColorRgb
	{
		// Token: 0x06003353 RID: 13139 RVA: 0x0004EF4F File Offset: 0x0004D14F
		public ColorRgb(byte r, byte g, byte b)
		{
			this.R = r;
			this.G = g;
			this.B = b;
		}

		// Token: 0x0400170A RID: 5898
		public static readonly ColorRgb Zero = new ColorRgb(0, 0, 0);

		// Token: 0x0400170B RID: 5899
		public byte R;

		// Token: 0x0400170C RID: 5900
		public byte G;

		// Token: 0x0400170D RID: 5901
		public byte B;
	}
}
