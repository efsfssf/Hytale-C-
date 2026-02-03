using System;
using Coherent.UI.Binding;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using Newtonsoft.Json.Linq;

namespace HytaleClient.Data.Items
{
	// Token: 0x02000AFC RID: 2812
	[CoherentType]
	public class ClientItemStack
	{
		// Token: 0x0600584C RID: 22604 RVA: 0x001ACAFD File Offset: 0x001AACFD
		public ClientItemStack()
		{
		}

		// Token: 0x0600584D RID: 22605 RVA: 0x001ACB07 File Offset: 0x001AAD07
		public ClientItemStack(string id, int quantity = 1)
		{
			this.Id = id;
			this.Quantity = quantity;
		}

		// Token: 0x0600584E RID: 22606 RVA: 0x001ACB20 File Offset: 0x001AAD20
		public ClientItemStack(Item packet)
		{
			this.Id = packet.ItemId;
			this.Quantity = packet.Quantity;
			this.Durability = packet.Durability;
			this.MaxDurability = packet.MaxDurability;
			bool flag = packet.Metadata != null;
			if (flag)
			{
				this.Metadata = ProtoHelper.DeserializeBson((byte[])packet.Metadata);
				JObject metadata = this.Metadata;
				this.StringifiedMetadata = ((metadata != null) ? metadata.ToString() : null);
			}
		}

		// Token: 0x0600584F RID: 22607 RVA: 0x001ACBA4 File Offset: 0x001AADA4
		public ClientItemStack(ClientItemStack other)
		{
			this.Id = other.Id;
			this.Quantity = other.Quantity;
			this.Durability = other.Durability;
			this.MaxDurability = other.MaxDurability;
			this.Metadata = other.Metadata;
			this.StringifiedMetadata = other.StringifiedMetadata;
		}

		// Token: 0x06005850 RID: 22608 RVA: 0x001ACC04 File Offset: 0x001AAE04
		public override string ToString()
		{
			return string.Format("ItemId: {0}, Quantity: {1}, Durability: {2}, MaxDurability: {3}, Metadata: {4}", new object[]
			{
				this.Id,
				this.Quantity,
				this.Durability,
				this.MaxDurability,
				this.Metadata
			});
		}

		// Token: 0x06005851 RID: 22609 RVA: 0x001ACC64 File Offset: 0x001AAE64
		public Item ToItemPacket(bool includeMetadata)
		{
			return new Item(this.Id, this.Quantity, this.Durability, this.MaxDurability, false, includeMetadata ? ((sbyte[])ProtoHelper.SerializeBson(this.Metadata)) : null);
		}

		// Token: 0x06005852 RID: 22610 RVA: 0x001ACCAC File Offset: 0x001AAEAC
		public ClientItemStack Clone()
		{
			return new ClientItemStack(this);
		}

		// Token: 0x06005853 RID: 22611 RVA: 0x001ACCC4 File Offset: 0x001AAEC4
		public bool IsEquivalentType(ClientItemStack itemStack)
		{
			bool flag = itemStack == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !this.Id.Equals(itemStack.Id);
				result = (!flag2 && ((this.Metadata != null) ? this.Metadata.Equals(itemStack.Metadata) : (itemStack.Metadata == null)));
			}
			return result;
		}

		// Token: 0x06005854 RID: 22612 RVA: 0x001ACD24 File Offset: 0x001AAF24
		public static bool IsEquivalent(ClientItemStack itemOne, ClientItemStack itemTwo)
		{
			return itemOne == itemTwo || (itemOne != null && itemOne.IsEquivalentType(itemTwo));
		}

		// Token: 0x040036D8 RID: 14040
		[CoherentProperty("itemId")]
		public string Id;

		// Token: 0x040036D9 RID: 14041
		[CoherentProperty("quantity")]
		public int Quantity;

		// Token: 0x040036DA RID: 14042
		[CoherentProperty("durability")]
		public double Durability;

		// Token: 0x040036DB RID: 14043
		[CoherentProperty("maxDurability")]
		public double MaxDurability;

		// Token: 0x040036DC RID: 14044
		public JObject Metadata;

		// Token: 0x040036DD RID: 14045
		[CoherentProperty("metadata")]
		public readonly string StringifiedMetadata;
	}
}
