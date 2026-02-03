using System;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x0200086A RID: 2154
	[UIMarkupData]
	public class NumberFieldFormat
	{
		// Token: 0x04001C2D RID: 7213
		public int MaxDecimalPlaces = 0;

		// Token: 0x04001C2E RID: 7214
		public decimal Step = 1m;

		// Token: 0x04001C2F RID: 7215
		public decimal MinValue = decimal.MinValue;

		// Token: 0x04001C30 RID: 7216
		public decimal MaxValue = decimal.MaxValue;

		// Token: 0x04001C31 RID: 7217
		public decimal DefaultValue;

		// Token: 0x04001C32 RID: 7218
		public string Suffix;
	}
}
