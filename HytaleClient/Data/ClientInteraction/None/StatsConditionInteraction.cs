using System;
using System.Collections.Generic;
using HytaleClient.Data.EntityStats;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.None
{
	// Token: 0x02000B36 RID: 2870
	internal class StatsConditionInteraction : SimpleInstantInteraction
	{
		// Token: 0x06005926 RID: 22822 RVA: 0x001B4D58 File Offset: 0x001B2F58
		public StatsConditionInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x06005927 RID: 22823 RVA: 0x001B4D64 File Offset: 0x001B2F64
		protected override void FirstRun(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			bool flag = true;
			foreach (KeyValuePair<int, float> keyValuePair in this.Interaction.Costs)
			{
				ClientEntityStatValue entityStat = gameInstance.LocalPlayer.GetEntityStat(keyValuePair.Key);
				bool flag2 = entityStat != null;
				if (!flag2)
				{
					flag = false;
					break;
				}
				float value = this.GetValue(entityStat);
				bool lessThan = this.Interaction.LessThan;
				if (lessThan)
				{
					bool flag3 = value >= keyValuePair.Value;
					if (flag3)
					{
						flag = false;
						break;
					}
				}
				else
				{
					bool flag4 = value < keyValuePair.Value && !this.CanOverdraw(value, entityStat.Min);
					if (flag4)
					{
						flag = false;
						break;
					}
				}
			}
			bool flag5 = !flag;
			if (flag5)
			{
				context.State.State = 3;
			}
		}

		// Token: 0x06005928 RID: 22824 RVA: 0x001B4E64 File Offset: 0x001B3064
		protected float GetValue(ClientEntityStatValue stat)
		{
			return (this.Interaction.ValueType_ == 1) ? stat.Value : (stat.AsPercentage() * 100f);
		}

		// Token: 0x06005929 RID: 22825 RVA: 0x001B4E98 File Offset: 0x001B3098
		protected bool CanOverdraw(float value, float min)
		{
			return this.Interaction.Lenient && value > 0f && min < 0f;
		}
	}
}
