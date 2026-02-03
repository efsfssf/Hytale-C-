using System;
using System.Collections.Generic;
using HytaleClient.Data.ClientInteraction.Selector;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.None
{
	// Token: 0x02000B33 RID: 2867
	internal class SelectInteraction : SimpleInteraction
	{
		// Token: 0x0600591A RID: 22810 RVA: 0x001B4710 File Offset: 0x001B2910
		public SelectInteraction(int id, Interaction interaction) : base(id, interaction)
		{
			bool flag = interaction.AoeCircleSelector_ != null;
			if (flag)
			{
				this._selector = new AOECircleSelector(interaction.AoeCircleSelector_);
			}
			else
			{
				bool flag2 = interaction.HorizontalSelector_ != null;
				if (flag2)
				{
					this._selector = new HorizontalSelector(interaction.HorizontalSelector_);
				}
				else
				{
					bool flag3 = interaction.StabSelector_ != null;
					if (flag3)
					{
						this._selector = new StabSelector(interaction.StabSelector_);
					}
					else
					{
						bool flag4 = interaction.AoeCylinderSelector_ != null;
						if (flag4)
						{
							this._selector = new AOECylinderSelector(interaction.AoeCylinderSelector_);
						}
						else
						{
							bool flag5 = interaction.RaycastSelector_ != null;
							if (!flag5)
							{
								throw new ArgumentException("Missing selector for interaction");
							}
							this._selector = new RaycastSelector(interaction.RaycastSelector_);
						}
					}
				}
			}
		}

		// Token: 0x0600591B RID: 22811 RVA: 0x001B47E4 File Offset: 0x001B29E4
		protected override void Tick0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context)
		{
			SelectInteraction.<>c__DisplayClass2_0 CS$<>8__locals1 = new SelectInteraction.<>c__DisplayClass2_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.gameInstance = gameInstance;
			CS$<>8__locals1.context = context;
			bool flag = this._selector == null;
			if (!flag)
			{
				bool flag2 = firstRun || CS$<>8__locals1.context.InstanceStore.EntitySelector == null;
				if (flag2)
				{
					CS$<>8__locals1.context.InstanceStore.EntitySelector = this._selector.NewSelector(CS$<>8__locals1.gameInstance.InteractionModule.Random);
					bool flag3 = firstRun && time <= 0f && this.Interaction.RunTime > 0f;
					if (flag3)
					{
						return;
					}
				}
				Selector entitySelector = CS$<>8__locals1.context.InstanceStore.EntitySelector;
				entitySelector.Tick(CS$<>8__locals1.gameInstance, CS$<>8__locals1.context.Entity, Math.Min(time, this.Interaction.RunTime), this.Interaction.RunTime);
				bool flag4 = this.Interaction.HitEntity != int.MinValue || this.Interaction.HitEntityRules != null;
				bool flag5 = flag4;
				if (flag5)
				{
					HashSet<int> hitEntities = CS$<>8__locals1.context.InstanceStore.HitEntities = (CS$<>8__locals1.context.InstanceStore.HitEntities ?? new HashSet<int>());
					entitySelector.SelectTargetEntities(CS$<>8__locals1.gameInstance, CS$<>8__locals1.context.Entity, delegate(Entity entity, Vector4 hit)
					{
						bool flag8 = entity.PredictionId != null;
						if (!flag8)
						{
							bool flag9 = !hitEntities.Add(entity.NetworkId);
							if (!flag9)
							{
								int num3 = CS$<>8__locals1.<>4__this.Interaction.HitEntity;
								bool flag10 = num3 != int.MinValue;
								if (flag10)
								{
									bool flag11 = entity.NetworkId == CS$<>8__locals1.context.Entity.NetworkId || entity.IsDead(true) || !entity.IsTangible();
									if (flag11)
									{
										num3 = int.MinValue;
									}
								}
								bool flag12 = CS$<>8__locals1.<>4__this.Interaction.HitEntityRules != null;
								if (flag12)
								{
									foreach (HitEntity hitEntity in CS$<>8__locals1.<>4__this.Interaction.HitEntityRules)
									{
										bool flag13 = true;
										foreach (EntityMatcher matcher in hitEntity.Matchers)
										{
											bool flag14 = !SelectInteraction.MatchRule(matcher, CS$<>8__locals1.gameInstance, CS$<>8__locals1.context.Entity, entity);
											if (flag14)
											{
												flag13 = false;
												break;
											}
										}
										bool flag15 = flag13;
										if (flag15)
										{
											num3 = hitEntity.Next;
										}
									}
								}
								bool flag16 = num3 == int.MinValue;
								if (!flag16)
								{
									List<SelectedHitEntity> list = CS$<>8__locals1.context.InstanceStore.RecordedHits = (CS$<>8__locals1.context.InstanceStore.RecordedHits ?? new List<SelectedHitEntity>());
									list.Add(new SelectedHitEntity(entity.NetworkId, new Vector3f(hit.X, hit.Y, hit.Z), entity.Position.ToPositionPacket(), entity.BodyOrientation.ToDirectionPacket()));
									InteractionContext interactionContext = CS$<>8__locals1.context.Duplicate();
									interactionContext.MetaStore.TargetEntity = entity;
									interactionContext.MetaStore.HitLocation = new Vector4?(hit);
									interactionContext.MetaStore.TargetBlock = null;
									interactionContext.MetaStore.TargetBlockRaw = null;
									interactionContext.MetaStore.SelectMetaStore = CS$<>8__locals1.context.InstanceStore;
									CS$<>8__locals1.context.ForkPredicted(new InteractionChainData(), CS$<>8__locals1.context.Chain.Type, interactionContext, num3);
								}
							}
						}
					}, new Predicate<Entity>(CS$<>8__locals1.<Tick0>g__Filter|0));
					CS$<>8__locals1.context.State.AttackerPos = CS$<>8__locals1.gameInstance.LocalPlayer.Position.ToPositionPacket();
					CS$<>8__locals1.context.State.AttackerRot = CS$<>8__locals1.gameInstance.LocalPlayer.BodyOrientation.ToDirectionPacket();
					SelectedHitEntity[] hitEntities3 = CS$<>8__locals1.context.State.HitEntities;
					int? num = (hitEntities3 != null) ? new int?(hitEntities3.Length) : null;
					HashSet<int> hitEntities2 = CS$<>8__locals1.context.InstanceStore.HitEntities;
					int? num2 = (hitEntities2 != null) ? new int?(hitEntities2.Count) : null;
					bool flag6 = !(num.GetValueOrDefault() == num2.GetValueOrDefault() & num != null == (num2 != null));
					if (flag6)
					{
						InteractionSyncData state = CS$<>8__locals1.context.State;
						List<SelectedHitEntity> recordedHits = CS$<>8__locals1.context.InstanceStore.RecordedHits;
						state.HitEntities = ((recordedHits != null) ? recordedHits.ToArray() : null);
					}
					bool flag7 = CS$<>8__locals1.context.Labels != null && hitEntities.Count == 0 && CS$<>8__locals1.context.State.State == null && (this.Interaction.FailOn == 1 || this.Interaction.FailOn == 3);
					if (flag7)
					{
						CS$<>8__locals1.context.State.State = 3;
					}
				}
				base.Tick0(CS$<>8__locals1.gameInstance, clickType, hasAnyButtonClick, firstRun, time, type, CS$<>8__locals1.context);
			}
		}

		// Token: 0x0600591C RID: 22812 RVA: 0x001B4B68 File Offset: 0x001B2D68
		public override InteractionChain MapForkChain(InteractionContext context, InteractionChainData data)
		{
			bool flag = data.BlockPosition_ != null;
			InteractionChain result;
			if (flag)
			{
				result = null;
			}
			else
			{
				Dictionary<ulong, InteractionChain> forkedChains = context.Chain.ForkedChains;
				foreach (InteractionChain interactionChain in forkedChains.Values)
				{
					bool flag2 = interactionChain.BaseForkedChainId.EntryIndex != context.Entry.Index;
					if (!flag2)
					{
						InteractionChainData chainData = interactionChain.ChainData;
						bool flag3 = chainData.EntityId == data.EntityId;
						if (flag3)
						{
							return interactionChain;
						}
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x0600591D RID: 22813 RVA: 0x001B4C28 File Offset: 0x001B2E28
		private static bool MatchRule(EntityMatcher matcher, GameInstance gameInstance, Entity attacker, Entity target)
		{
			return SelectInteraction.MatchRule0(matcher, gameInstance, attacker, target) ^ matcher.Invert;
		}

		// Token: 0x0600591E RID: 22814 RVA: 0x001B4C4C File Offset: 0x001B2E4C
		private static bool MatchRule0(EntityMatcher matcher, GameInstance gameInstance, Entity attacker, Entity target)
		{
			bool result;
			switch (matcher.Type)
			{
			case 0:
				result = true;
				break;
			case 1:
				result = !target.IsInvulnerable();
				break;
			case 2:
				result = (target.PlayerSkin != null);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return result;
		}

		// Token: 0x04003755 RID: 14165
		private readonly SelectorType _selector;
	}
}
