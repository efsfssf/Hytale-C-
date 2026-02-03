using System;
using HytaleClient.Data.Items;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.InGame
{
	// Token: 0x02000887 RID: 2183
	[UIMarkupData]
	internal class ItemGridSlot
	{
		// Token: 0x170010A3 RID: 4259
		// (get) Token: 0x06003E63 RID: 15971 RVA: 0x000A72DA File Offset: 0x000A54DA
		// (set) Token: 0x06003E64 RID: 15972 RVA: 0x000A72E2 File Offset: 0x000A54E2
		public TextureArea ItemIcon { get; private set; }

		// Token: 0x170010A4 RID: 4260
		// (get) Token: 0x06003E65 RID: 15973 RVA: 0x000A72EB File Offset: 0x000A54EB
		// (set) Token: 0x06003E66 RID: 15974 RVA: 0x000A72F3 File Offset: 0x000A54F3
		public TexturePatch BackgroundPatch { get; private set; }

		// Token: 0x170010A5 RID: 4261
		// (get) Token: 0x06003E67 RID: 15975 RVA: 0x000A72FC File Offset: 0x000A54FC
		// (set) Token: 0x06003E68 RID: 15976 RVA: 0x000A7304 File Offset: 0x000A5504
		public TexturePatch OverlayPatch { get; private set; }

		// Token: 0x170010A6 RID: 4262
		// (get) Token: 0x06003E69 RID: 15977 RVA: 0x000A730D File Offset: 0x000A550D
		// (set) Token: 0x06003E6A RID: 15978 RVA: 0x000A7315 File Offset: 0x000A5515
		public TextureArea IconTextureArea { get; private set; }

		// Token: 0x06003E6B RID: 15979 RVA: 0x000A731E File Offset: 0x000A551E
		public ItemGridSlot()
		{
		}

		// Token: 0x06003E6C RID: 15980 RVA: 0x000A732F File Offset: 0x000A552F
		public ItemGridSlot(ClientItemStack itemStack)
		{
			this.ItemStack = itemStack;
		}

		// Token: 0x06003E6D RID: 15981 RVA: 0x000A7348 File Offset: 0x000A5548
		public void ApplyStyles(InGameView inGameView, Desktop desktop)
		{
			ClientItemBase clientItemBase;
			bool flag = this.ItemStack != null && inGameView.Items.TryGetValue(this.ItemStack.Id, out clientItemBase);
			if (flag)
			{
				this.ItemIcon = inGameView.GetTextureAreaForItemIcon(clientItemBase.Icon);
			}
			else
			{
				this.ItemIcon = null;
			}
			this.BackgroundPatch = ((this.Background != null) ? desktop.MakeTexturePatch(this.Background) : null);
			this.OverlayPatch = ((this.Overlay != null) ? desktop.MakeTexturePatch(this.Overlay) : null);
			this.IconTextureArea = ((this.Icon != null) ? (this.Icon.TextureArea ?? ((this.Icon.TexturePath != null) ? desktop.Provider.MakeTextureArea(this.Icon.TexturePath.Value) : desktop.Provider.WhitePixel)) : null);
		}

		// Token: 0x04001D4B RID: 7499
		public ClientItemStack ItemStack;

		// Token: 0x04001D4C RID: 7500
		public PatchStyle Background;

		// Token: 0x04001D4D RID: 7501
		public PatchStyle Overlay;

		// Token: 0x04001D4E RID: 7502
		public PatchStyle Icon;

		// Token: 0x04001D4F RID: 7503
		public bool IsItemIncompatible;

		// Token: 0x04001D50 RID: 7504
		public string Name;

		// Token: 0x04001D51 RID: 7505
		public string Description;

		// Token: 0x04001D52 RID: 7506
		public int? InventorySlotIndex;

		// Token: 0x04001D53 RID: 7507
		public bool SkipItemQualityBackground;

		// Token: 0x04001D54 RID: 7508
		public bool IsActivatable = true;
	}
}
