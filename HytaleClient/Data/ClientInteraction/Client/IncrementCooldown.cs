using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Client
{
	// Token: 0x02000B42 RID: 2882
	internal class IncrementCooldown : SimpleInstantInteraction
	{
		// Token: 0x06005959 RID: 22873 RVA: 0x001B787C File Offset: 0x001B5A7C
		public IncrementCooldown(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x0600595A RID: 22874 RVA: 0x001B7888 File Offset: 0x001B5A88
		protected override void FirstRun(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			string cooldownId = this.Interaction.CooldownId;
			bool flag = string.IsNullOrEmpty(cooldownId);
			if (flag)
			{
				InteractionCooldown cooldown = context.Chain.RootInteraction.RootInteraction.Cooldown;
				bool flag2 = cooldown != null;
				if (flag2)
				{
					cooldownId = cooldown.CooldownId;
				}
			}
			this.ProcessCooldown(gameInstance, cooldownId);
			context.State.State = 0;
		}

		// Token: 0x0600595B RID: 22875 RVA: 0x001B78EC File Offset: 0x001B5AEC
		protected void ProcessCooldown(GameInstance gameInstance, string id)
		{
			Cooldown cooldown = gameInstance.InteractionModule.GetCooldown(id);
			bool flag = cooldown != null;
			if (flag)
			{
				bool flag2 = this.Interaction.CooldownIncrementTime != 0f;
				if (flag2)
				{
					cooldown.IncreaseTime(this.Interaction.CooldownIncrementTime);
				}
				bool flag3 = this.Interaction.CooldownIncrementCharge != 0;
				if (flag3)
				{
					cooldown.ReplenishCharge(this.Interaction.CooldownIncrementCharge, this.Interaction.CooldownIncrementInterrupt);
				}
				bool flag4 = this.Interaction.CooldownIncrementChargeTime != 0f;
				if (flag4)
				{
					cooldown.IncreaseChargeTime(this.Interaction.CooldownIncrementChargeTime);
				}
			}
		}
	}
}
