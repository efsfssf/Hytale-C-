using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.None
{
	// Token: 0x02000B28 RID: 2856
	internal class ChangeActiveSlotInteraction : ClientInteraction
	{
		// Token: 0x060058F5 RID: 22773 RVA: 0x001B31A4 File Offset: 0x001B13A4
		public ChangeActiveSlotInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x060058F6 RID: 22774 RVA: 0x001B31B0 File Offset: 0x001B13B0
		protected override void Tick0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context)
		{
			bool flag = !firstRun;
			if (flag)
			{
				context.State.State = 0;
			}
			else
			{
				context.InstanceStore.OriginalSlot = new int?(gameInstance.InventoryModule.HotbarActiveSlot);
				bool flag2 = this.Interaction.TargetSlot == int.MinValue;
				int num;
				if (flag2)
				{
					num = context.MetaStore.TargetSlot.Value;
				}
				else
				{
					bool flag3 = gameInstance.InventoryModule.HotbarActiveSlot == this.Interaction.TargetSlot;
					if (flag3)
					{
						context.State.State = 0;
						return;
					}
					num = this.Interaction.TargetSlot;
					context.MetaStore.TargetSlot = new int?(num);
				}
				gameInstance.InventoryModule.SetActiveHotbarSlot(num, false);
				bool disableSlotFork = context.MetaStore.DisableSlotFork;
				if (disableSlotFork)
				{
					bool flag4 = context.ServerData == null;
					if (flag4)
					{
						context.State.State = 4;
					}
					else
					{
						context.State.State = context.ServerData.State;
					}
				}
				else
				{
					InteractionContext interactionContext = InteractionContext.ForInteraction(gameInstance, gameInstance.InventoryModule, 14, null);
					int rootInteractionId2;
					bool rootInteractionId = interactionContext.GetRootInteractionId(gameInstance, 14, out rootInteractionId2);
					if (rootInteractionId)
					{
						bool flag5 = this.Interaction.TargetSlot != int.MinValue;
						if (flag5)
						{
							interactionContext.MetaStore.TargetSlot = new int?(num);
						}
						context.Fork(14, interactionContext, rootInteractionId2);
					}
					context.State.State = 0;
				}
			}
		}

		// Token: 0x060058F7 RID: 22775 RVA: 0x001B3344 File Offset: 0x001B1544
		protected override void Revert0(GameInstance gameInstance, InteractionType type, InteractionContext context)
		{
			bool flag = this.Interaction.TargetSlot == int.MinValue;
			int num;
			if (flag)
			{
				num = context.MetaStore.TargetSlot.Value;
			}
			else
			{
				num = this.Interaction.TargetSlot;
				context.MetaStore.TargetSlot = new int?(num);
			}
			bool flag2 = gameInstance.InventoryModule.HotbarActiveSlot == num;
			if (flag2)
			{
				gameInstance.InventoryModule.SetActiveHotbarSlot(context.InstanceStore.OriginalSlot.Value, false);
			}
		}

		// Token: 0x060058F8 RID: 22776 RVA: 0x001B33CD File Offset: 0x001B15CD
		protected override void MatchServer0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			this.Tick0(gameInstance, clickType, hasAnyButtonClick, true, 0f, type, context);
		}
	}
}
