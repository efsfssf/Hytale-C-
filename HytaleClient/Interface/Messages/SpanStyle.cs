using System;
using HytaleClient.Graphics;

namespace HytaleClient.Interface.Messages
{
	// Token: 0x02000817 RID: 2071
	public struct SpanStyle
	{
		// Token: 0x04001951 RID: 6481
		public bool IsUppercase;

		// Token: 0x04001952 RID: 6482
		public bool IsBold;

		// Token: 0x04001953 RID: 6483
		public bool IsItalics;

		// Token: 0x04001954 RID: 6484
		public bool IsUnderlined;

		// Token: 0x04001955 RID: 6485
		public UInt32Color? Color;

		// Token: 0x04001956 RID: 6486
		public string Link;

		// Token: 0x04001957 RID: 6487
		public string LastTag;
	}
}
