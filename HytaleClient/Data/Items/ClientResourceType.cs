using System;
using Coherent.UI.Binding;
using HytaleClient.Protocol;

namespace HytaleClient.Data.Items
{
	// Token: 0x02000AFD RID: 2813
	[CoherentType]
	public class ClientResourceType
	{
		// Token: 0x06005855 RID: 22613 RVA: 0x001ACD4A File Offset: 0x001AAF4A
		public ClientResourceType(ResourceType packet)
		{
			this.Id = packet.Id;
			this.Icon = packet.Icon;
		}

		// Token: 0x040036DE RID: 14046
		[CoherentProperty("id")]
		public readonly string Id;

		// Token: 0x040036DF RID: 14047
		[CoherentProperty("icon")]
		public readonly string Icon;
	}
}
