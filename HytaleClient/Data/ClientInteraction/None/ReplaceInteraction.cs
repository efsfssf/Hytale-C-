using System;
using System.Collections.Generic;
using HytaleClient.Data.Items;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.None
{
	// Token: 0x02000B31 RID: 2865
	internal class ReplaceInteraction : ClientInteraction
	{
		// Token: 0x06005912 RID: 22802 RVA: 0x001B44ED File Offset: 0x001B26ED
		public ReplaceInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x06005913 RID: 22803 RVA: 0x001B44FC File Offset: 0x001B26FC
		protected override void Tick0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context)
		{
			bool flag = ClientInteraction.Failed(context.State.State);
			if (!flag)
			{
				bool flag2 = !firstRun;
				if (!flag2)
				{
					int interactions = this.GetInteractions(gameInstance, context);
					bool flag3 = interactions == int.MinValue;
					if (flag3)
					{
						context.State.State = 3;
					}
					else
					{
						context.State.State = 0;
						context.Execute(gameInstance.InteractionModule.RootInteractions[interactions]);
					}
				}
			}
		}

		// Token: 0x06005914 RID: 22804 RVA: 0x001B4574 File Offset: 0x001B2774
		protected override void Revert0(GameInstance gameInstance, InteractionType type, InteractionContext context)
		{
		}

		// Token: 0x06005915 RID: 22805 RVA: 0x001B4578 File Offset: 0x001B2778
		protected override void MatchServer0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			context.State.State = context.ServerData.State;
			bool flag = context.State.State == 0;
			if (flag)
			{
				context.Execute(gameInstance.InteractionModule.RootInteractions[context.ServerData.EnteredRootInteraction]);
			}
		}

		// Token: 0x06005916 RID: 22806 RVA: 0x001B45D4 File Offset: 0x001B27D4
		public int GetInteractions(GameInstance gameInstance, InteractionContext context)
		{
			int result = this.Interaction.DefaultValue;
			bool flag = context.OriginalItemType != null;
			if (flag)
			{
				ClientItemBase item = gameInstance.ItemLibraryModule.GetItem(context.OriginalItemType);
				int minValue = int.MinValue;
				bool? flag2;
				if (item == null)
				{
					flag2 = null;
				}
				else
				{
					Dictionary<string, int> interactionVars = item.InteractionVars;
					flag2 = ((interactionVars != null) ? new bool?(interactionVars.TryGetValue(this.Interaction.Variable, out minValue)) : null);
				}
				bool? flag3 = flag2;
				bool valueOrDefault = flag3.GetValueOrDefault();
				if (valueOrDefault)
				{
					result = minValue;
				}
			}
			return result;
		}
	}
}
