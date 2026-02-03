using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Client
{
	// Token: 0x02000B3F RID: 2879
	internal class CooldownConditionInteraction : SimpleInstantInteraction
	{
		// Token: 0x0600594C RID: 22860 RVA: 0x001B7371 File Offset: 0x001B5571
		public CooldownConditionInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x0600594D RID: 22861 RVA: 0x001B7380 File Offset: 0x001B5580
		protected override void FirstRun(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			bool flag = this.CheckCooldown(gameInstance, this.Interaction.CooldownId);
			if (flag)
			{
				context.State.State = 3;
			}
			else
			{
				context.State.State = 0;
			}
		}

		// Token: 0x0600594E RID: 22862 RVA: 0x001B73C4 File Offset: 0x001B55C4
		protected bool CheckCooldown(GameInstance gameInstance, string id)
		{
			Cooldown cooldown = gameInstance.InteractionModule.GetCooldown(id);
			return cooldown != null && cooldown.HasCooldown(false);
		}
	}
}
