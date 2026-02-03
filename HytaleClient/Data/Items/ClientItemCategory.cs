using System;
using System.Linq;
using Coherent.UI.Binding;
using HytaleClient.Protocol;

namespace HytaleClient.Data.Items
{
	// Token: 0x02000AF3 RID: 2803
	[CoherentType]
	internal class ClientItemCategory
	{
		// Token: 0x0600583B RID: 22587 RVA: 0x001AC2C0 File Offset: 0x001AA4C0
		public ClientItemCategory(ItemCategory packet)
		{
			this.Id = packet.Id;
			this.Icon = packet.Icon;
			this.Order = packet.Order;
			this.InfoDisplayMode = packet.InfoDisplayMode;
			bool flag = packet.Children != null;
			if (flag)
			{
				this.Children = Enumerable.ToArray<ClientItemCategory>(Enumerable.Select<ItemCategory, ClientItemCategory>(Enumerable.Where<ItemCategory>(packet.Children, (ItemCategory category) => category != null), (ItemCategory category) => new ClientItemCategory(category)));
			}
		}

		// Token: 0x040036B0 RID: 14000
		[CoherentProperty("id")]
		public readonly string Id;

		// Token: 0x040036B1 RID: 14001
		[CoherentProperty("icon")]
		public readonly string Icon;

		// Token: 0x040036B2 RID: 14002
		[CoherentProperty("order")]
		public readonly int Order;

		// Token: 0x040036B3 RID: 14003
		[CoherentProperty("infoDisplayMode")]
		public readonly ItemGridInfoDisplayMode InfoDisplayMode;

		// Token: 0x040036B4 RID: 14004
		[CoherentProperty("children")]
		public readonly ClientItemCategory[] Children;
	}
}
