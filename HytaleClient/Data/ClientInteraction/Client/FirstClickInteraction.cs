using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Client
{
	// Token: 0x02000B41 RID: 2881
	internal class FirstClickInteraction : ClientInteraction
	{
		// Token: 0x06005954 RID: 22868 RVA: 0x001B76E8 File Offset: 0x001B58E8
		public FirstClickInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x06005955 RID: 22869 RVA: 0x001B76F4 File Offset: 0x001B58F4
		protected override void Tick0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context)
		{
			bool flag = clickType != InteractionModule.ClickType.Single && context.Labels != null;
			if (flag)
			{
				context.State.State = 3;
				context.Jump(context.Labels[0]);
			}
			else
			{
				context.State.State = 0;
			}
		}

		// Token: 0x06005956 RID: 22870 RVA: 0x001B7747 File Offset: 0x001B5947
		protected override void Revert0(GameInstance gameInstance, InteractionType type, InteractionContext context)
		{
		}

		// Token: 0x06005957 RID: 22871 RVA: 0x001B774A File Offset: 0x001B594A
		protected override void MatchServer0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			this.Tick0(gameInstance, clickType, hasAnyButtonClick, true, 0f, type, context);
		}

		// Token: 0x06005958 RID: 22872 RVA: 0x001B7764 File Offset: 0x001B5964
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

		// Token: 0x0400376B RID: 14187
		private const int FailedLabelIndex = 0;
	}
}
