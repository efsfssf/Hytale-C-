using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.None
{
	// Token: 0x02000B2A RID: 2858
	internal class ClearEntityEffectInteraction : SimpleInstantInteraction
	{
		// Token: 0x060058FB RID: 22779 RVA: 0x001B3446 File Offset: 0x001B1646
		public ClearEntityEffectInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x060058FC RID: 22780 RVA: 0x001B3452 File Offset: 0x001B1652
		protected override void FirstRun(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			context.InstanceStore.PredictedEffect = gameInstance.LocalPlayer.PredictedRemoveEffect(this.Interaction.EffectId);
		}

		// Token: 0x060058FD RID: 22781 RVA: 0x001B3477 File Offset: 0x001B1677
		protected override void Revert0(GameInstance gameInstance, InteractionType type, InteractionContext context)
		{
			gameInstance.LocalPlayer.CancelPrediction(context.InstanceStore.PredictedEffect);
		}
	}
}
