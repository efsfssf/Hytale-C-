using System;
using HytaleClient.Data.Items;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.InGame.Hud
{
	// Token: 0x020008B4 RID: 2228
	internal class BuilderToolsMaterialSlotSelector : ItemSlotSelector
	{
		// Token: 0x0600408B RID: 16523 RVA: 0x000BAA5F File Offset: 0x000B8C5F
		public BuilderToolsMaterialSlotSelector(InGameView inGameView) : base(inGameView, false)
		{
		}

		// Token: 0x0600408C RID: 16524 RVA: 0x000BAA6B File Offset: 0x000B8C6B
		public override void Build()
		{
			base.Build();
			base.Find<Element>("Icon").Background = new PatchStyle("InGame/Hud/UtilitySlotSelectorIcon.png");
		}

		// Token: 0x0600408D RID: 16525 RVA: 0x000BAA90 File Offset: 0x000B8C90
		protected override void OnSlotSelected(int slot, bool clicked)
		{
			ClientItemStack itemStack = base.GetItemStack(slot);
			bool flag = itemStack != null;
			if (flag)
			{
				this.SelectedSlot = slot;
				this.Interface.TriggerEventFromInterface("builderTools.selectActiveToolMaterial", itemStack, null, null);
			}
		}
	}
}
