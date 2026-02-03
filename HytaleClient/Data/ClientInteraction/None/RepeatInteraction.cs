using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.None
{
	// Token: 0x02000B30 RID: 2864
	internal class RepeatInteraction : SimpleInteraction
	{
		// Token: 0x0600590F RID: 22799 RVA: 0x001B4352 File Offset: 0x001B2552
		public RepeatInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x06005910 RID: 22800 RVA: 0x001B4360 File Offset: 0x001B2560
		protected override void Tick0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context)
		{
			bool flag = firstRun && this.Interaction.Repeat != -1;
			if (flag)
			{
				context.InstanceStore.RemainingRepeats = this.Interaction.Repeat;
			}
			bool flag2 = context.InstanceStore.ForkedChain != null;
			if (flag2)
			{
				switch (context.InstanceStore.ForkedChain.ClientState)
				{
				case 0:
				{
					bool flag3 = this.Interaction.Repeat != -1 && context.InstanceStore.RemainingRepeats <= 0;
					if (flag3)
					{
						context.State.State = 0;
						base.Tick0(gameInstance, clickType, hasAnyButtonClick, firstRun, time, type, context);
						return;
					}
					context.State.State = 4;
					break;
				}
				case 3:
					context.State.State = 3;
					base.Tick0(gameInstance, clickType, hasAnyButtonClick, firstRun, time, type, context);
					return;
				case 4:
					context.State.State = 4;
					return;
				}
			}
			context.InstanceStore.ForkedChain = context.Fork(context.Duplicate(), this.Interaction.ForkInteractions);
			context.State.State = 4;
			bool flag4 = this.Interaction.Repeat != -1;
			if (flag4)
			{
				context.InstanceStore.RemainingRepeats--;
			}
		}

		// Token: 0x06005911 RID: 22801 RVA: 0x001B44DD File Offset: 0x001B26DD
		protected override void MatchServer0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			context.State.State = 4;
		}
	}
}
