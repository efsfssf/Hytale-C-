using System;
using HytaleClient.Data.Items;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Server
{
	// Token: 0x02000B16 RID: 2838
	internal class ModifyInventoryInteraction : SimpleInstantInteraction
	{
		// Token: 0x060058BF RID: 22719 RVA: 0x001B2220 File Offset: 0x001B0420
		public ModifyInventoryInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x060058C0 RID: 22720 RVA: 0x001B222C File Offset: 0x001B042C
		protected override void FirstRun(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			bool flag = this.Interaction.RequiredGameMode != gameInstance.GameMode;
			if (!flag)
			{
				bool flag2 = this.Interaction.ItemToRemove != null;
				if (flag2)
				{
				}
				bool flag3 = context.HeldItem != null && this.Interaction.AdjustHeldItemQuantity < 0 && context.HeldItem.Quantity == -this.Interaction.AdjustHeldItemQuantity;
				if (flag3)
				{
					context.HeldItemContainer[context.HeldItemSlot] = null;
					gameInstance.InventoryModule.UpdateAll();
					context.HeldItem = null;
				}
				ClientItemStack heldItem = context.HeldItem;
				ClientItemStack clientItemStack = (heldItem != null) ? heldItem.Clone() : null;
				bool flag4 = clientItemStack == null;
				if (!flag4)
				{
					clientItemStack.Durability += this.Interaction.AdjustHeldItemDurability;
					bool flag5 = clientItemStack.Durability <= 0.0 && clientItemStack.MaxDurability > 0.0;
					if (flag5)
					{
						bool flag6 = this.Interaction.BrokenItem == null;
						if (flag6)
						{
							clientItemStack = null;
						}
						else
						{
							clientItemStack = new ClientItemStack(this.Interaction.BrokenItem, 1);
						}
					}
					context.HeldItemContainer[context.HeldItemSlot] = clientItemStack;
					gameInstance.InventoryModule.UpdateAll();
					context.HeldItem = clientItemStack;
				}
			}
		}
	}
}
