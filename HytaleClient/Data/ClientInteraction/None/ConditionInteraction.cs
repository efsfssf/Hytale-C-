using System;
using HytaleClient.Data.Entities;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.None
{
	// Token: 0x02000B2B RID: 2859
	internal class ConditionInteraction : SimpleInteraction
	{
		// Token: 0x060058FE RID: 22782 RVA: 0x001B3491 File Offset: 0x001B1691
		public ConditionInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x060058FF RID: 22783 RVA: 0x001B34A0 File Offset: 0x001B16A0
		protected override void Tick0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context)
		{
			bool flag = context.State.State > 0;
			if (!flag)
			{
				bool flag2 = true;
				bool flag3 = this.Interaction.HasRequiredGameMode && this.Interaction.RequiredGameMode != gameInstance.GameMode;
				if (flag3)
				{
					flag2 = false;
				}
				ref ClientMovementStates ptr = ref gameInstance.CharacterControllerModule.MovementController.MovementStates;
				bool flag4 = this.Interaction.HasJumping && this.Interaction.Jumping != ptr.IsJumping;
				if (flag4)
				{
					flag2 = false;
				}
				bool flag5 = this.Interaction.HasSwimming && this.Interaction.Swimming != ptr.IsSwimming;
				if (flag5)
				{
					flag2 = false;
				}
				bool flag6 = this.Interaction.HasCrouching && this.Interaction.Crouching != ptr.IsCrouching;
				if (flag6)
				{
					flag2 = false;
				}
				bool flag7 = this.Interaction.HasRunning && this.Interaction.Running != ptr.IsSprinting;
				if (flag7)
				{
					flag2 = false;
				}
				bool flag8 = this.Interaction.HasFlying && this.Interaction.Flying != ptr.IsFlying;
				if (flag8)
				{
					flag2 = false;
				}
				context.State.State = (flag2 ? 0 : 3);
				base.Tick0(gameInstance, clickType, hasAnyButtonClick, firstRun, time, type, context);
			}
		}
	}
}
