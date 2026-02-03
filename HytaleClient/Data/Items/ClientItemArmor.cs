using System;
using Coherent.UI.Binding;
using HytaleClient.Protocol;

namespace HytaleClient.Data.Items
{
	// Token: 0x02000AF1 RID: 2801
	[CoherentType]
	internal class ClientItemArmor
	{
		// Token: 0x06005837 RID: 22583 RVA: 0x001AC214 File Offset: 0x001AA414
		public ClientItemArmor(ItemBase.ItemArmor armor)
		{
			bool flag = armor != null;
			if (flag)
			{
				this.ArmorSlot = armor.ArmorSlot;
				this.CosmeticsToHide = armor.CosmeticsToHide;
			}
		}

		// Token: 0x04003683 RID: 13955
		[CoherentProperty("armorSlot")]
		public readonly ItemBase.ItemArmor.ItemArmorSlot ArmorSlot;

		// Token: 0x04003684 RID: 13956
		[CoherentProperty("cosmeticsToHide")]
		public readonly Cosmetic[] CosmeticsToHide;
	}
}
