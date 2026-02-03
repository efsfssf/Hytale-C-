using System;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.InGame.Hud
{
	// Token: 0x020008B7 RID: 2231
	internal class ConsumableSlotSelector : ItemSlotSelector
	{
		// Token: 0x060040B8 RID: 16568 RVA: 0x000BC096 File Offset: 0x000BA296
		public ConsumableSlotSelector(InGameView inGameView) : base(inGameView, true)
		{
		}

		// Token: 0x060040B9 RID: 16569 RVA: 0x000BC0A2 File Offset: 0x000BA2A2
		public override void Build()
		{
			base.Build();
			base.Find<Element>("Icon").Background = new PatchStyle("InGame/Hud/ConsumableSlotSelectorIcon.png");
		}

		// Token: 0x060040BA RID: 16570 RVA: 0x000BC0C8 File Offset: 0x000BA2C8
		protected override void OnSlotSelected(int slot, bool clicked)
		{
			if (clicked)
			{
				this.Interface.TriggerEventFromInterface("game.useConsumableSlot", slot, null, null);
			}
			else
			{
				this.Interface.TriggerEventFromInterface("game.selectActiveConsumableSlot", slot, null, null);
			}
		}

		// Token: 0x04001F08 RID: 7944
		public const string Id = "Hytale:ConsumableSlotSelector";
	}
}
