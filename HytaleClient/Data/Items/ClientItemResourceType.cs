using System;
using Coherent.UI.Binding;
using HytaleClient.Protocol;

namespace HytaleClient.Data.Items
{
	// Token: 0x02000AF9 RID: 2809
	[CoherentType]
	internal class ClientItemResourceType
	{
		// Token: 0x06005847 RID: 22599 RVA: 0x001AC96B File Offset: 0x001AAB6B
		public ClientItemResourceType(ItemBase.ItemResourceType resource)
		{
			this.Id = resource.Id;
			this.Quantity = resource.Quantity;
		}

		// Token: 0x040036CF RID: 14031
		[CoherentProperty("id")]
		public readonly string Id;

		// Token: 0x040036D0 RID: 14032
		[CoherentProperty("quantity")]
		public readonly int Quantity;
	}
}
