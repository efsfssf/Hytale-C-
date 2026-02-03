using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Client
{
	// Token: 0x02000B4A RID: 2890
	internal class TriggerCooldownInteraction : SimpleInstantInteraction
	{
		// Token: 0x0600598B RID: 22923 RVA: 0x001BA566 File Offset: 0x001B8766
		public TriggerCooldownInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x0600598C RID: 22924 RVA: 0x001BA574 File Offset: 0x001B8774
		protected override void FirstRun(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			string cooldownId = null;
			float cooldownTime = 0f;
			float[] chargeTimes = null;
			bool interruptRecharge = false;
			bool flag = this.Interaction.Cooldown != null;
			if (flag)
			{
				cooldownId = this.Interaction.Cooldown.CooldownId;
				cooldownTime = this.Interaction.Cooldown.Cooldown;
				chargeTimes = this.Interaction.Cooldown.ChargeTimes;
				interruptRecharge = this.Interaction.Cooldown.InterruptRecharge;
			}
			this.ResetCooldown(context, gameInstance, cooldownId, cooldownTime, chargeTimes, interruptRecharge);
		}

		// Token: 0x0600598D RID: 22925 RVA: 0x001BA5F8 File Offset: 0x001B87F8
		protected void ResetCooldown(InteractionContext context, GameInstance gameInstance, string cooldownId, float cooldownTime, float[] chargeTimes, bool interruptRecharge0)
		{
			float num = 0.35f;
			float[] array = InteractionModule.DefaultChargeTimes;
			bool interruptRecharge = false;
			bool flag = cooldownId == null;
			if (flag)
			{
				RootInteraction rootInteraction = context.Chain.RootInteraction.RootInteraction;
				InteractionCooldown cooldown = rootInteraction.Cooldown;
				bool flag2 = cooldown != null;
				if (flag2)
				{
					cooldownId = cooldown.CooldownId;
					bool flag3 = cooldown.Cooldown > 0f;
					if (flag3)
					{
						num = cooldown.Cooldown;
					}
					bool interruptRecharge2 = cooldown.InterruptRecharge;
					if (interruptRecharge2)
					{
						interruptRecharge = true;
					}
					bool flag4 = cooldown.ChargeTimes != null && cooldown.ChargeTimes.Length != 0;
					if (flag4)
					{
						array = cooldown.ChargeTimes;
					}
				}
				bool flag5 = cooldownId == null;
				if (flag5)
				{
					cooldownId = rootInteraction.Id;
				}
			}
			Cooldown cooldown2 = gameInstance.InteractionModule.GetCooldown(cooldownId);
			bool flag6 = cooldown2 != null;
			if (flag6)
			{
				num = cooldown2.GetCooldown();
				array = cooldown2.GetCharges();
				interruptRecharge = cooldown2.InterruptRecharge();
			}
			bool flag7 = cooldownTime > 0f;
			if (flag7)
			{
				num = cooldownTime;
			}
			bool flag8 = chargeTimes != null && chargeTimes.Length != 0;
			if (flag8)
			{
				array = chargeTimes;
			}
			if (interruptRecharge0)
			{
				interruptRecharge = true;
			}
			Cooldown cooldown3 = gameInstance.InteractionModule.GetCooldown(cooldownId, num, array, true, interruptRecharge);
			cooldown3.SetCooldownMax(num);
			cooldown3.SetCharges(array);
			cooldown3.DeductCharge();
		}
	}
}
