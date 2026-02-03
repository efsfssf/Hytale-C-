using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Client
{
	// Token: 0x02000B4C RID: 2892
	internal class UseEntityInteraction : SimpleInstantInteraction
	{
		// Token: 0x06005991 RID: 22929 RVA: 0x001BA82D File Offset: 0x001B8A2D
		public UseEntityInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x06005992 RID: 22930 RVA: 0x001BA83C File Offset: 0x001B8A3C
		protected override void FirstRun(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			bool flag = context.MetaStore.TargetEntity == null;
			if (flag)
			{
				context.State.State = 3;
			}
			else
			{
				Entity targetEntity = context.MetaStore.TargetEntity;
				context.State.EntityId = targetEntity.NetworkId;
				int num;
				bool flag2 = targetEntity.Interactions.TryGetValue(type, out num);
				if (flag2)
				{
					context.State.State = 0;
					context.Execute(gameInstance.InteractionModule.RootInteractions[num]);
				}
				else
				{
					context.State.State = 3;
				}
			}
		}

		// Token: 0x06005993 RID: 22931 RVA: 0x001BA8D4 File Offset: 0x001B8AD4
		protected override void MatchServer0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			context.State.State = context.ServerData.State;
			bool flag = context.State.State == 0;
			if (flag)
			{
				context.Execute(gameInstance.InteractionModule.RootInteractions[context.ServerData.EnteredRootInteraction]);
			}
			base.MatchServer0(gameInstance, clickType, hasAnyButtonClick, type, context);
		}
	}
}
