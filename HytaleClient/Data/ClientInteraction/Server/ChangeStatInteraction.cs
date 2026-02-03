using System;
using System.Collections.Generic;
using HytaleClient.Data.EntityStats;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;
using NLog;

namespace HytaleClient.Data.ClientInteraction.Server
{
	// Token: 0x02000B15 RID: 2837
	internal class ChangeStatInteraction : SimpleInstantInteraction
	{
		// Token: 0x060058BB RID: 22715 RVA: 0x001B1FDA File Offset: 0x001B01DA
		public ChangeStatInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x060058BC RID: 22716 RVA: 0x001B1FE8 File Offset: 0x001B01E8
		protected override void FirstRun(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			Entity entity;
			bool flag = ClientInteraction.TryGetEntity(gameInstance, context, this.Interaction.EntityTarget, out entity);
			if (flag)
			{
				EntityStatUpdate[] array = new EntityStatUpdate[this.Interaction.StatModifiers.Count];
				int num = 0;
				foreach (KeyValuePair<int, float> keyValuePair in this.Interaction.StatModifiers)
				{
					int key = keyValuePair.Key;
					float num2 = keyValuePair.Value;
					bool flag2 = this.Interaction.ValueType_ == 0;
					if (flag2)
					{
						ClientEntityStatValue entityStat = entity.GetEntityStat(key);
						bool flag3 = entityStat == null;
						if (flag3)
						{
							num++;
							continue;
						}
						num2 = num2 * (entityStat.Max - entityStat.Min) / 100f;
					}
					bool flag4 = num2 != 0f;
					if (flag4)
					{
						Interaction.ChangeStatBehaviour changeStatBehaviour_ = this.Interaction.ChangeStatBehaviour_;
						Interaction.ChangeStatBehaviour changeStatBehaviour = changeStatBehaviour_;
						if (changeStatBehaviour != null)
						{
							if (changeStatBehaviour != 1)
							{
								throw new ArgumentOutOfRangeException();
							}
							array[num++] = entity.SetStatValue(key, num2);
						}
						else
						{
							array[num++] = entity.AddStatValue(key, num2);
						}
					}
				}
				context.InstanceStore.PredictedStats = array;
			}
			else
			{
				ChangeStatInteraction.Logger.Error(string.Format("Entity does not exist for ChangeStatInteraction in {0}, ID: {1}", type, this.Id));
			}
		}

		// Token: 0x060058BD RID: 22717 RVA: 0x001B2170 File Offset: 0x001B0370
		protected override void Revert0(GameInstance gameInstance, InteractionType type, InteractionContext context)
		{
			bool flag = context.InstanceStore.PredictedStats == null;
			if (!flag)
			{
				int num = 0;
				foreach (KeyValuePair<int, float> keyValuePair in this.Interaction.StatModifiers)
				{
					EntityStatUpdate entityStatUpdate = context.InstanceStore.PredictedStats[num++];
					bool flag2 = entityStatUpdate == null;
					if (!flag2)
					{
						gameInstance.LocalPlayer.CancelStatPrediction(keyValuePair.Key, entityStatUpdate);
					}
				}
			}
		}

		// Token: 0x0400372F RID: 14127
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
	}
}
