using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;
using NLog;

namespace HytaleClient.Data.ClientInteraction.None
{
	// Token: 0x02000B24 RID: 2852
	internal class ApplyEffectInteraction : SimpleInstantInteraction
	{
		// Token: 0x060058EB RID: 22763 RVA: 0x001B2FE2 File Offset: 0x001B11E2
		public ApplyEffectInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x060058EC RID: 22764 RVA: 0x001B2FF0 File Offset: 0x001B11F0
		protected override void FirstRun(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			Entity entity;
			bool flag = ClientInteraction.TryGetEntity(gameInstance, context, this.Interaction.EntityTarget, out entity);
			if (flag)
			{
				context.InstanceStore.PredictedEffect = entity.PredictedAddEffect(this.Interaction.EffectId, null);
			}
			else
			{
				ApplyEffectInteraction.Logger.Error(string.Format("Entity does not exist for ApplyEffectInteraction in {0}, ID: {1}", type, this.Id));
			}
		}

		// Token: 0x060058ED RID: 22765 RVA: 0x001B306C File Offset: 0x001B126C
		protected override void Revert0(GameInstance gameInstance, InteractionType type, InteractionContext context)
		{
			Entity entity;
			bool flag = ClientInteraction.TryGetEntity(gameInstance, context, this.Interaction.EntityTarget, out entity);
			if (flag)
			{
				entity.CancelPrediction(context.InstanceStore.PredictedEffect);
			}
			else
			{
				ApplyEffectInteraction.Logger.Error(string.Format("Entity does not exist for ApplyEffectInteraction in {0}, ID: {1}", type, this.Id));
			}
		}

		// Token: 0x0400374F RID: 14159
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
	}
}
