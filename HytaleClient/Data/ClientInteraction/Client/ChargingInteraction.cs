using System;
using System.Collections.Generic;
using System.Linq;
using HytaleClient.Data.EntityStats;
using HytaleClient.Data.UserSettings;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Client
{
	// Token: 0x02000B3D RID: 2877
	internal class ChargingInteraction : ClientInteraction
	{
		// Token: 0x06005940 RID: 22848 RVA: 0x001B608C File Offset: 0x001B428C
		public ChargingInteraction(int id, Interaction interaction) : base(id, interaction)
		{
			bool flag = interaction.ChargedNext != null;
			if (flag)
			{
				this._highestChargeValue = Enumerable.Max(interaction.ChargedNext.Keys);
				this._shouldDisplayProgress = (interaction.DisplayProgress && (interaction.ChargedNext.Count != 1 || Enumerable.First<KeyValuePair<float, int>>(interaction.ChargedNext).Key != 0f));
				this._sortedKeys = Enumerable.ToArray<float>(interaction.ChargedNext.Keys);
				Array.Sort<float>(this._sortedKeys);
			}
			else
			{
				this._shouldDisplayProgress = false;
			}
			bool flag2 = this.Interaction.RunTime > 0f;
			if (flag2)
			{
				this._highestChargeValue = this.Interaction.RunTime;
			}
		}

		// Token: 0x06005941 RID: 22849 RVA: 0x001B6160 File Offset: 0x001B4360
		protected override void Tick0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context)
		{
			bool flag = ClientInteraction.Failed(context.State.State);
			if (!flag)
			{
				bool flag2 = this.Interaction.Forks != null;
				if (flag2)
				{
					bool flag3 = context.InstanceStore.ForkedChain == null || context.InstanceStore.ForkedChain.ClientState != 4;
					foreach (KeyValuePair<InteractionType, int> keyValuePair in this.Interaction.Forks)
					{
						InputBinding inputBindingForType = gameInstance.InteractionModule.GetInputBindingForType(keyValuePair.Key);
						gameInstance.InteractionModule.DisableInput(keyValuePair.Key);
						bool flag4 = !gameInstance.Input.CanConsumeBinding(inputBindingForType, false);
						if (!flag4)
						{
							gameInstance.Input.ConsumeBinding(inputBindingForType, false);
							bool flag5 = !flag3;
							if (!flag5)
							{
								InteractionContext context2 = context.Duplicate();
								context.InstanceStore.ForkedChain = context.Fork(keyValuePair.Key, context2, keyValuePair.Value);
								bool flag6 = context.State.ForkCounts == null;
								if (flag6)
								{
									context.State.ForkCounts = new Dictionary<InteractionType, int>();
								}
								int num;
								context.State.ForkCounts.TryGetValue(keyValuePair.Key, out num);
								context.State.ForkCounts[keyValuePair.Key] = num + 1;
							}
						}
					}
				}
				bool failOnDamage = this.Interaction.FailOnDamage;
				List<DamageInfo> damageInfos = gameInstance.InteractionModule.DamageInfos;
				if (firstRun)
				{
					gameInstance.CameraModule.SetTargetMouseModifier(this.Interaction.MouseSensitivityAdjustmentTarget, this.Interaction.MouseSensitivityAdjustmentDuration);
					context.State.State = 4;
				}
				else
				{
					bool flag7 = damageInfos.Count != 0 && failOnDamage;
					if (flag7)
					{
						gameInstance.CameraModule.SetTargetMouseModifier(1f, gameInstance.App.Settings.ResetMouseSensitivityDuration);
						context.State.State = 3;
						ClientRootInteraction.Label[] labels = context.Labels;
						float[] sortedKeys = this._sortedKeys;
						context.Jump(labels[(sortedKeys != null) ? sortedKeys.Length : 0]);
					}
					else
					{
						bool flag8 = damageInfos.Count > 0 && this.Interaction.ChargingDelay_ != null;
						if (flag8)
						{
							Interaction.ChargingDelay chargingDelay_ = this.Interaction.ChargingDelay_;
							for (int i = 0; i < damageInfos.Count; i++)
							{
								DamageInfo damageInfo = damageInfos[i];
								float damageAmount = damageInfo.DamageAmount;
								ClientEntityStatValue entityStat = context.Entity.GetEntityStat(DefaultEntityStats.Health);
								float num2 = damageAmount / ((entityStat != null) ? entityStat.Max : 1f);
								bool flag9 = num2 < chargingDelay_.MinHealth;
								if (!flag9)
								{
									num2 = MathHelper.Min(num2, chargingDelay_.MaxHealth);
									float amount = (num2 - chargingDelay_.MinHealth) / (chargingDelay_.MaxHealth - chargingDelay_.MinHealth);
									float num3 = MathHelper.Lerp(chargingDelay_.MinDelay, chargingDelay_.MaxDelay, amount);
									context.InstanceStore.TotalDelay += num3;
								}
							}
							bool flag10 = chargingDelay_.MaxTotalDelay > 0f;
							if (flag10)
							{
								context.InstanceStore.TotalDelay = MathHelper.Min(context.InstanceStore.TotalDelay, chargingDelay_.MaxTotalDelay);
							}
							context.InstanceStore.TotalDelay = MathHelper.Min(context.InstanceStore.TotalDelay, time);
						}
						time -= context.InstanceStore.TotalDelay;
						bool flag11 = (double)time >= ChargingInteraction.DisplayDelay && this._shouldDisplayProgress;
						bool flag12 = flag11 && !context.InstanceStore.ChargingVisible;
						if (flag12)
						{
							context.InstanceStore.ChargingVisible = true;
							gameInstance.App.Interface.TriggerEvent("combat.setShowChargeProgress", true, Enumerable.ToArray<float>(Enumerable.Select<float, float>(this.Interaction.ChargedNext.Keys, (float threshold) => threshold / this._highestChargeValue)), null, null, null, null);
						}
						bool flag13 = clickType == InteractionModule.ClickType.Held && ((this.Interaction.AllowIndefiniteHold && this.Interaction.RunTime <= 0f) || time < this._highestChargeValue);
						if (flag13)
						{
							bool flag14 = this.Interaction.CancelOnOtherClick && hasAnyButtonClick;
							if (flag14)
							{
								gameInstance.CameraModule.SetTargetMouseModifier(1f, gameInstance.App.Settings.ResetMouseSensitivityDuration);
								context.State.ChargeValue = -2f;
								context.State.State = 0;
							}
							else
							{
								context.State.State = 4;
								int num4 = (int)Math.Min((double)time / (double)this._highestChargeValue * 100.0, 100.0);
								bool flag15 = num4 != context.InstanceStore.PrimaryChargingLastProgress;
								if (flag15)
								{
									context.InstanceStore.PrimaryChargingLastProgress = num4;
									bool flag16 = flag11;
									if (flag16)
									{
										gameInstance.App.Interface.TriggerEvent("combat.setChargeProgress", num4, null, null, null, null, null);
									}
								}
							}
						}
						else
						{
							gameInstance.CameraModule.SetTargetMouseModifier(1f, gameInstance.App.Settings.ResetMouseSensitivityDuration);
							context.State.State = 0;
							context.State.ChargeValue = time;
							this.JumpToChargeValue(context);
						}
					}
				}
			}
		}

		// Token: 0x06005942 RID: 22850 RVA: 0x001B6714 File Offset: 0x001B4914
		private void JumpToChargeValue(InteractionContext context)
		{
			bool flag = this.Interaction.ChargedNext == null;
			if (!flag)
			{
				float num = 2.1474836E+09f;
				int num2 = -1;
				int num3 = 0;
				foreach (float num4 in this._sortedKeys)
				{
					bool flag2 = context.State.ChargeValue < num4;
					if (flag2)
					{
						num3++;
					}
					else
					{
						float num5 = context.State.ChargeValue - num4;
						bool flag3 = num2 == -1 || num5 < num;
						if (flag3)
						{
							num = num5;
							num2 = num3;
						}
						num3++;
					}
				}
				bool flag4 = num2 != -1;
				if (flag4)
				{
					context.Jump(context.Labels[num2]);
				}
			}
		}

		// Token: 0x06005943 RID: 22851 RVA: 0x001B67D0 File Offset: 0x001B49D0
		public override void Compile(InteractionModule module, ClientRootInteraction.OperationsBuilder builder)
		{
			ClientRootInteraction.Label label = builder.CreateUnresolvedLabel();
			Dictionary<float, int> chargedNext = this.Interaction.ChargedNext;
			ClientRootInteraction.Label[] array = new ClientRootInteraction.Label[((chargedNext != null) ? chargedNext.Count : 0) + 1];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = builder.CreateUnresolvedLabel();
			}
			builder.AddOperation(this.Id, array);
			builder.Jump(label);
			int num = 0;
			bool flag = this._sortedKeys != null;
			if (flag)
			{
				foreach (float key in this._sortedKeys)
				{
					builder.ResolveLabel(array[num]);
					ClientInteraction clientInteraction = module.Interactions[this.Interaction.ChargedNext[key]];
					clientInteraction.Compile(module, builder);
					builder.Jump(label);
					num++;
				}
			}
			builder.ResolveLabel(array[num]);
			bool flag2 = this.Interaction.Failed != int.MinValue;
			if (flag2)
			{
				ClientInteraction clientInteraction2 = module.Interactions[this.Interaction.Failed];
				clientInteraction2.Compile(module, builder);
			}
			builder.ResolveLabel(label);
		}

		// Token: 0x06005944 RID: 22852 RVA: 0x001B68FC File Offset: 0x001B4AFC
		public override void Handle(GameInstance gameInstance, bool firstRun, float time, InteractionType type, InteractionContext context)
		{
			base.Handle(gameInstance, firstRun, time, type, context);
			bool flag = context.State.State != 4;
			if (flag)
			{
				gameInstance.App.Interface.TriggerEvent("combat.setShowChargeProgress", false, null, null, null, null, null);
			}
		}

		// Token: 0x06005945 RID: 22853 RVA: 0x001B6951 File Offset: 0x001B4B51
		protected override void Revert0(GameInstance gameInstance, InteractionType type, InteractionContext context)
		{
			gameInstance.App.Interface.TriggerEvent("combat.setShowChargeProgress", false, null, null, null, null, null);
		}

		// Token: 0x06005946 RID: 22854 RVA: 0x001B6978 File Offset: 0x001B4B78
		protected override void MatchServer0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			bool flag = context.ServerData.State == 4 && context.ServerData.Progress < this._highestChargeValue;
			if (!flag)
			{
				bool flag2 = context.State.State == 3 && context.Labels != null;
				if (flag2)
				{
					ClientRootInteraction.Label[] labels = context.Labels;
					float[] sortedKeys = this._sortedKeys;
					context.Jump(labels[(sortedKeys != null) ? sortedKeys.Length : 0]);
				}
				else
				{
					context.State.State = 0;
					context.State.ChargeValue = context.ServerData.Progress;
					this.JumpToChargeValue(context);
				}
			}
		}

		// Token: 0x04003761 RID: 14177
		private const int MinimumPrimaryChargingDuration = 200;

		// Token: 0x04003762 RID: 14178
		private const float ChargingCanceled = -2f;

		// Token: 0x04003763 RID: 14179
		private static readonly double DisplayDelay = 0.2;

		// Token: 0x04003764 RID: 14180
		private readonly float _highestChargeValue;

		// Token: 0x04003765 RID: 14181
		private readonly bool _shouldDisplayProgress;

		// Token: 0x04003766 RID: 14182
		private readonly float[] _sortedKeys;
	}
}
