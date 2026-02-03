using System;
using System.Collections.Generic;
using System.Linq;
using HytaleClient.Protocol;

namespace HytaleClient.Data.Items
{
	// Token: 0x02000AFB RID: 2811
	internal class ClientItemReticleConfig
	{
		// Token: 0x06005849 RID: 22601 RVA: 0x001AC9BB File Offset: 0x001AABBB
		public ClientItemReticleConfig()
		{
		}

		// Token: 0x0600584A RID: 22602 RVA: 0x001AC9C8 File Offset: 0x001AABC8
		public ClientItemReticleConfig(ItemReticleConfig packet)
		{
			this.Id = packet.Id;
			this.Base = packet.Base;
			Dictionary<int, ClientItemReticle> serverEvents;
			if (packet.ServerEvents != null)
			{
				serverEvents = Enumerable.ToDictionary<KeyValuePair<int, ItemReticle>, int, ClientItemReticle>(packet.ServerEvents, (KeyValuePair<int, ItemReticle> kvp) => kvp.Key, (KeyValuePair<int, ItemReticle> kvp) => new ClientItemReticle(kvp.Value));
			}
			else
			{
				serverEvents = new Dictionary<int, ClientItemReticle>();
			}
			this.ServerEvents = serverEvents;
			Dictionary<ItemReticleClientEvent, ClientItemReticle> clientEvents;
			if (packet.ClientEvents != null)
			{
				clientEvents = Enumerable.ToDictionary<KeyValuePair<ItemReticleClientEvent, ItemReticle>, ItemReticleClientEvent, ClientItemReticle>(packet.ClientEvents, (KeyValuePair<ItemReticleClientEvent, ItemReticle> kvp) => kvp.Key, (KeyValuePair<ItemReticleClientEvent, ItemReticle> kvp) => new ClientItemReticle(kvp.Value));
			}
			else
			{
				clientEvents = new Dictionary<ItemReticleClientEvent, ClientItemReticle>();
			}
			this.ClientEvents = clientEvents;
		}

		// Token: 0x0600584B RID: 22603 RVA: 0x001ACAB4 File Offset: 0x001AACB4
		public ClientItemReticleConfig Clone()
		{
			return new ClientItemReticleConfig
			{
				Id = this.Id,
				Base = this.Base,
				ServerEvents = this.ServerEvents,
				ClientEvents = this.ClientEvents
			};
		}

		// Token: 0x040036D4 RID: 14036
		public string Id;

		// Token: 0x040036D5 RID: 14037
		public string[] Base;

		// Token: 0x040036D6 RID: 14038
		public Dictionary<int, ClientItemReticle> ServerEvents;

		// Token: 0x040036D7 RID: 14039
		public Dictionary<ItemReticleClientEvent, ClientItemReticle> ClientEvents;
	}
}
