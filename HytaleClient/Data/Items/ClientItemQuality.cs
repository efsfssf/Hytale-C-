using System;
using HytaleClient.Graphics;
using HytaleClient.Protocol;

namespace HytaleClient.Data.Items
{
	// Token: 0x02000AF7 RID: 2807
	internal class ClientItemQuality
	{
		// Token: 0x17001367 RID: 4967
		// (get) Token: 0x06005841 RID: 22593 RVA: 0x001AC68F File Offset: 0x001AA88F
		// (set) Token: 0x06005842 RID: 22594 RVA: 0x001AC697 File Offset: 0x001AA897
		public string Id { get; private set; }

		// Token: 0x06005843 RID: 22595 RVA: 0x001AC6A0 File Offset: 0x001AA8A0
		public ClientItemQuality()
		{
		}

		// Token: 0x06005844 RID: 22596 RVA: 0x001AC6AC File Offset: 0x001AA8AC
		public ClientItemQuality(ItemQuality itemQuality)
		{
			this.Id = itemQuality.Id;
			this.ItemTooltipTexture = (itemQuality.ItemTooltipTexture ?? "");
			this.ItemTooltipArrowTexture = (itemQuality.ItemTooltipArrowTexture ?? "");
			this.SlotTexture = (itemQuality.SlotTexture ?? "");
			this.SpecialSlotTexture = (itemQuality.SpecialSlotTexture ?? "");
			this.TextColor = ((itemQuality.TextColor != null) ? UInt32Color.FromRGBA((byte)itemQuality.TextColor.Red, (byte)itemQuality.TextColor.Green, (byte)itemQuality.TextColor.Blue, byte.MaxValue) : UInt32Color.FromHexString("#c9d2dd"));
			this.LocalizationKey = (itemQuality.LocalizationKey ?? "Missing");
			this.VisibleQualityLabel = itemQuality.VisibleQualityLabel;
			this.RenderSpecialSlot = itemQuality.RenderSpecialSlot;
		}

		// Token: 0x06005845 RID: 22597 RVA: 0x001AC798 File Offset: 0x001AA998
		public ClientItemQuality Clone()
		{
			return new ClientItemQuality
			{
				Id = this.Id,
				ItemTooltipTexture = this.ItemTooltipTexture,
				ItemTooltipArrowTexture = this.ItemTooltipArrowTexture,
				SlotTexture = this.SlotTexture,
				SpecialSlotTexture = this.SpecialSlotTexture,
				TextColor = this.TextColor,
				LocalizationKey = this.LocalizationKey,
				VisibleQualityLabel = this.VisibleQualityLabel,
				RenderSpecialSlot = this.RenderSpecialSlot
			};
		}

		// Token: 0x040036C2 RID: 14018
		public string ItemTooltipTexture;

		// Token: 0x040036C3 RID: 14019
		public string ItemTooltipArrowTexture;

		// Token: 0x040036C4 RID: 14020
		public string SlotTexture;

		// Token: 0x040036C5 RID: 14021
		public string SpecialSlotTexture;

		// Token: 0x040036C6 RID: 14022
		public UInt32Color TextColor;

		// Token: 0x040036C7 RID: 14023
		public string LocalizationKey;

		// Token: 0x040036C8 RID: 14024
		public bool VisibleQualityLabel;

		// Token: 0x040036C9 RID: 14025
		public bool RenderSpecialSlot;
	}
}
