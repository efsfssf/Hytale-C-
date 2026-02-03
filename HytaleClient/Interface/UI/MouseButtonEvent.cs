using System;

namespace HytaleClient.Interface.UI
{
	// Token: 0x0200082A RID: 2090
	public struct MouseButtonEvent
	{
		// Token: 0x06003A89 RID: 14985 RVA: 0x000858E1 File Offset: 0x00083AE1
		public MouseButtonEvent(int button, int clicks)
		{
			this.Button = button;
			this.Clicks = clicks;
		}

		// Token: 0x04001A5A RID: 6746
		public readonly int Button;

		// Token: 0x04001A5B RID: 6747
		public readonly int Clicks;
	}
}
