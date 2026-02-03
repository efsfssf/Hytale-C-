using System;
using HytaleClient.Protocol;

namespace HytaleClient.Data.Items
{
	// Token: 0x02000AFA RID: 2810
	public class ClientItemReticle
	{
		// Token: 0x06005848 RID: 22600 RVA: 0x001AC98D File Offset: 0x001AAB8D
		public ClientItemReticle(ItemReticle packet)
		{
			this.HideBase = packet.HideBase;
			this.Parts = packet.Parts;
			this.Duration = packet.Duration;
		}

		// Token: 0x040036D1 RID: 14033
		public readonly bool HideBase;

		// Token: 0x040036D2 RID: 14034
		public readonly string[] Parts;

		// Token: 0x040036D3 RID: 14035
		public readonly float Duration;
	}
}
