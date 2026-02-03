using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction
{
	// Token: 0x02000B14 RID: 2836
	internal class SimpleInteraction : ClientInteraction
	{
		// Token: 0x060058B5 RID: 22709 RVA: 0x001B1E09 File Offset: 0x001B0009
		public SimpleInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x060058B6 RID: 22710 RVA: 0x001B1E15 File Offset: 0x001B0015
		protected override void Tick0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context)
		{
			this.BaseTick(context);
		}

		// Token: 0x060058B7 RID: 22711 RVA: 0x001B1E24 File Offset: 0x001B0024
		public override void Compile(InteractionModule module, ClientRootInteraction.OperationsBuilder builder)
		{
			bool flag = this.Interaction.Next == int.MinValue && this.Interaction.Failed == int.MinValue;
			if (flag)
			{
				builder.AddOperation(this.Id);
			}
			else
			{
				ClientRootInteraction.Label label = builder.CreateUnresolvedLabel();
				ClientRootInteraction.Label label2 = builder.CreateUnresolvedLabel();
				builder.AddOperation(this.Id, new ClientRootInteraction.Label[]
				{
					label
				});
				bool flag2 = this.Interaction.Next != int.MinValue;
				if (flag2)
				{
					ClientInteraction clientInteraction = module.Interactions[this.Interaction.Next];
					clientInteraction.Compile(module, builder);
				}
				bool flag3 = this.Interaction.Failed != int.MinValue;
				if (flag3)
				{
					builder.Jump(label2);
				}
				builder.ResolveLabel(label);
				bool flag4 = this.Interaction.Failed != int.MinValue;
				if (flag4)
				{
					ClientInteraction clientInteraction2 = module.Interactions[this.Interaction.Failed];
					clientInteraction2.Compile(module, builder);
				}
				builder.ResolveLabel(label2);
			}
		}

		// Token: 0x060058B8 RID: 22712 RVA: 0x001B1F3C File Offset: 0x001B013C
		protected override void Revert0(GameInstance gameInstance, InteractionType type, InteractionContext context)
		{
		}

		// Token: 0x060058B9 RID: 22713 RVA: 0x001B1F3F File Offset: 0x001B013F
		protected override void MatchServer0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			context.State.State = context.ServerData.State;
			this.BaseTick(context);
		}

		// Token: 0x060058BA RID: 22714 RVA: 0x001B1F64 File Offset: 0x001B0164
		private void BaseTick(InteractionContext context)
		{
			bool flag = this.Interaction.WaitForDataFrom_ == 1 && context.ServerData != null && context.ServerData.State == 3;
			if (flag)
			{
				context.State.State = 3;
			}
			bool flag2 = context.State.State == 3 && context.Labels != null;
			if (flag2)
			{
				context.Jump(context.Labels[0]);
			}
		}

		// Token: 0x0400372E RID: 14126
		private const int FailedLabelIndex = 0;
	}
}
