using System;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.InGame.Hud
{
	// Token: 0x020008C7 RID: 2247
	internal class UtilitySlotSelector : ItemSlotSelector
	{
		// Token: 0x06004145 RID: 16709 RVA: 0x000C02D9 File Offset: 0x000BE4D9
		public UtilitySlotSelector(InGameView inGameView) : base(inGameView, true)
		{
		}

		// Token: 0x06004146 RID: 16710 RVA: 0x000C02E5 File Offset: 0x000BE4E5
		public override void Build()
		{
			base.Build();
			base.Find<Element>("Icon").Background = new PatchStyle("InGame/Hud/UtilitySlotSelectorIcon.png");
		}

		// Token: 0x06004147 RID: 16711 RVA: 0x000C030C File Offset: 0x000BE50C
		protected override void OnSlotSelected(int slot, bool clicked)
		{
			bool flag = slot == this.SelectedSlot - 1;
			if (!flag)
			{
				this.Interface.TriggerEventFromInterface("game.selectActiveUtilitySlot", slot, null, null);
			}
		}

		// Token: 0x04001FA0 RID: 8096
		public const string Id = "Hytale:UtilitySlotSelector";
	}
}
