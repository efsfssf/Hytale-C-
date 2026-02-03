using System;
using System.Linq;
using HytaleClient.Audio;
using HytaleClient.Data.FX;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Particles;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.None
{
	// Token: 0x02000B2C RID: 2860
	internal class DamageEntityInteraction : ClientInteraction
	{
		// Token: 0x06005900 RID: 22784 RVA: 0x001B361C File Offset: 0x001B181C
		public DamageEntityInteraction(int id, Interaction interaction) : base(id, interaction)
		{
			this._sortedTargetDamageKeys = Enumerable.ToArray<string>(interaction.TargetedDamage_.Keys);
			Array.Sort<string>(this._sortedTargetDamageKeys);
		}

		// Token: 0x06005901 RID: 22785 RVA: 0x001B364C File Offset: 0x001B184C
		protected override void Tick0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context)
		{
			Entity targetEntity = context.MetaStore.TargetEntity;
			bool flag = targetEntity == null || targetEntity.IsInvulnerable();
			if (flag)
			{
				context.Jump(context.Labels[0]);
				context.State.State = 3;
			}
			else
			{
				bool flag2 = false;
				int num = 1;
				Vector4? hitLocation = context.MetaStore.HitLocation;
				Vector3 position = targetEntity.Position;
				Vector3 position2 = context.Entity.Position;
				float num2 = (float)Math.Atan2((double)(position2.X - position.X), (double)(position2.Z - position.Z));
				DamageEffects damageEffects = null;
				for (int i = 0; i < targetEntity.RunningInteractions.Count; i++)
				{
					ClientInteraction clientInteraction = gameInstance.InteractionModule.Interactions[targetEntity.RunningInteractions[i]];
					bool flag3 = clientInteraction.Interaction.Type_ != 25;
					if (!flag3)
					{
						bool hasModifiers = clientInteraction.Interaction.HasModifiers;
						if (hasModifiers)
						{
							damageEffects = clientInteraction.Interaction.BlockedEffects;
							flag2 = true;
							break;
						}
						bool flag4 = clientInteraction.Interaction.AngledWielding_ != null;
						if (flag4)
						{
							float num3 = MathHelper.WrapAngle(num2 + 3.1415927f - targetEntity.BodyOrientation.Yaw);
							bool flag5 = Math.Abs(MathHelper.CompareAngle((double)num3, (double)clientInteraction.Interaction.AngledWielding_.AngleRad)) < (double)clientInteraction.Interaction.AngledWielding_.AngleDistanceRad;
							if (flag5)
							{
								damageEffects = clientInteraction.Interaction.BlockedEffects;
								flag2 = true;
								break;
							}
						}
					}
				}
				bool flag6 = false;
				bool flag7 = this.Interaction.AngledDamage_ != null;
				if (flag7)
				{
					float num4 = MathHelper.WrapAngle(num2 + 3.1415927f - targetEntity.BodyOrientation.Yaw);
					for (int j = 0; j < this.Interaction.AngledDamage_.Length; j++)
					{
						AngledDamage angledDamage = this.Interaction.AngledDamage_[j];
						bool flag8 = Math.Abs(MathHelper.CompareAngle((double)num4, angledDamage.Angle)) >= angledDamage.AngleDistance;
						if (!flag8)
						{
							num = 3 + j;
							DamageEntityInteraction.ApplyDamageEffects(context, damageEffects ?? angledDamage.DamageEffects_, gameInstance, targetEntity, hitLocation, num2);
							flag6 = true;
							break;
						}
					}
				}
				string hitDetail = context.MetaStore.HitDetail;
				bool flag9 = hitDetail != null;
				if (flag9)
				{
					TargetedDamage targetedDamage;
					bool flag10 = this.Interaction.TargetedDamage_.TryGetValue(hitDetail, out targetedDamage);
					if (flag10)
					{
						num = targetedDamage.Index;
						DamageEntityInteraction.ApplyDamageEffects(context, damageEffects ?? targetedDamage.DamageEffects_, gameInstance, targetEntity, hitLocation, num2);
						flag6 = true;
					}
				}
				bool flag11 = !flag6;
				if (flag11)
				{
					DamageEntityInteraction.ApplyDamageEffects(context, damageEffects ?? this.Interaction.DamageEffects_, gameInstance, targetEntity, hitLocation, num2);
				}
				bool flag12 = !flag2 && context.MetaStore.SelectMetaStore != null && this.Interaction.EntityStatsOnHit != null;
				if (flag12)
				{
					InteractionMetaStore selectMetaStore = context.MetaStore.SelectMetaStore;
					selectMetaStore.Sequence++;
					context.InstanceStore.PredictedStats = new EntityStatUpdate[this.Interaction.EntityStatsOnHit.Length];
					for (int k = 0; k < this.Interaction.EntityStatsOnHit.Length; k++)
					{
						EntityStatOnHit entityStatOnHit = this.Interaction.EntityStatsOnHit[k];
						bool flag13 = selectMetaStore.Sequence <= entityStatOnHit.MultipliersPerEntitiesHit.Length;
						float num5;
						if (flag13)
						{
							num5 = entityStatOnHit.MultipliersPerEntitiesHit[selectMetaStore.Sequence - 1];
						}
						else
						{
							num5 = entityStatOnHit.MultiplierPerExtraEntityHit;
						}
						context.InstanceStore.PredictedStats[k] = gameInstance.LocalPlayer.AddStatValue(entityStatOnHit.EntityStatIndex, entityStatOnHit.Amount * num5);
					}
				}
				Vector4 valueOrDefault;
				bool flag14;
				if (gameInstance.InteractionModule.ShowSelectorDebug)
				{
					if (hitLocation != null)
					{
						valueOrDefault = hitLocation.GetValueOrDefault();
						flag14 = true;
					}
					else
					{
						flag14 = false;
					}
				}
				else
				{
					flag14 = false;
				}
				bool flag15 = flag14;
				if (flag15)
				{
					Mesh mesh = default(Mesh);
					MeshProcessor.CreateSphere(ref mesh, 5, 8, 0.2f, 0, -1, -1);
					Matrix matrix = Matrix.CreateTranslation(valueOrDefault.X, valueOrDefault.Y, valueOrDefault.Z);
					gameInstance.InteractionModule.SelectorDebugMeshes.Add(new InteractionModule.DebugSelectorMesh(matrix, mesh, 5f, new Vector3(1f, 1f, 0f)));
				}
				gameInstance.App.Interface.InGameView.ReticleComponent.OnClientEvent(0, null);
				bool flag16 = flag2;
				if (flag16)
				{
					context.Jump(context.Labels[2]);
				}
				else
				{
					context.Jump(context.Labels[num]);
				}
				context.State.State = 0;
			}
		}

		// Token: 0x06005902 RID: 22786 RVA: 0x001B3B34 File Offset: 0x001B1D34
		private static void ApplyDamageEffects(InteractionContext context, DamageEffects damageEffects, GameInstance gameInstance, Entity target, Vector4? hitPos, float yawAngle)
		{
			bool flag = damageEffects == null;
			if (!flag)
			{
				WorldParticle[] worldParticles = damageEffects.WorldParticles;
				Vector4 valueOrDefault;
				bool flag2;
				if (worldParticles != null)
				{
					if (hitPos != null)
					{
						valueOrDefault = hitPos.GetValueOrDefault();
						flag2 = true;
					}
					else
					{
						flag2 = false;
					}
				}
				else
				{
					flag2 = false;
				}
				bool flag3 = flag2;
				if (flag3)
				{
					Quaternion quaternion;
					Quaternion.CreateFromYaw(yawAngle, out quaternion);
					context.InstanceStore.DamageParticles = new ParticleSystemProxy[worldParticles.Length];
					for (int i = 0; i < worldParticles.Length; i++)
					{
						WorldParticle worldParticle = worldParticles[i];
						ParticleSystemProxy particleSystemProxy;
						bool flag4 = !gameInstance.ParticleSystemStoreModule.TrySpawnSystem(worldParticle.SystemId, out particleSystemProxy, false, false);
						if (!flag4)
						{
							context.InstanceStore.DamageParticles[i] = particleSystemProxy;
							Vector3 vector = (worldParticle.PositionOffset != null) ? new Vector3(worldParticle.PositionOffset.X, worldParticle.PositionOffset.Y, worldParticle.PositionOffset.Z) : Vector3.Zero;
							vector = Vector3.Transform(vector, quaternion);
							particleSystemProxy.Position = new Vector3(valueOrDefault.X + vector.X, valueOrDefault.Y + vector.Y, valueOrDefault.Z + vector.Z);
							Direction direction = (worldParticle.RotationOffset != null) ? worldParticle.RotationOffset : new Direction(0f, 0f, 0f);
							Quaternion quaternion2;
							Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(direction.Yaw), MathHelper.ToRadians(direction.Pitch), MathHelper.ToRadians(direction.Roll), out quaternion2);
							particleSystemProxy.Rotation = quaternion * quaternion2;
							bool flag5 = worldParticle.Color_ != null;
							if (flag5)
							{
								particleSystemProxy.DefaultColor = UInt32Color.FromRGBA((byte)worldParticle.Color_.Red, (byte)worldParticle.Color_.Green, (byte)worldParticle.Color_.Blue, byte.MaxValue);
							}
							particleSystemProxy.Scale = worldParticle.Scale;
						}
					}
				}
				bool flag6 = target != null;
				if (flag6)
				{
					ModelParticle[] modelParticles = damageEffects.ModelParticles;
					bool flag7 = modelParticles != null;
					if (flag7)
					{
						ModelParticleSettings[] modelParticles2;
						ParticleProtocolInitializer.Initialize(modelParticles, out modelParticles2, gameInstance.EntityStoreModule.NodeNameManager);
						target.AddModelParticles(modelParticles2);
					}
					target.PredictStatusAnimation("Hurt");
				}
				uint networkWwiseId = ResourceManager.GetNetworkWwiseId(damageEffects.SoundEventIndex);
				Vector4 valueOrDefault2;
				bool flag8;
				if (hitPos != null)
				{
					valueOrDefault2 = hitPos.GetValueOrDefault();
					flag8 = true;
				}
				else
				{
					flag8 = false;
				}
				bool flag9 = flag8;
				if (flag9)
				{
					gameInstance.AudioModule.PlaySoundEvent(networkWwiseId, new Vector3(valueOrDefault2.X, valueOrDefault2.Y, valueOrDefault2.Z), Vector3.Zero, ref context.InstanceStore.DamageSoundEventReference);
				}
			}
		}

		// Token: 0x06005903 RID: 22787 RVA: 0x001B3DD4 File Offset: 0x001B1FD4
		public override void Compile(InteractionModule module, ClientRootInteraction.OperationsBuilder builder)
		{
			AngledDamage[] angledDamage_ = this.Interaction.AngledDamage_;
			ClientRootInteraction.Label[] array = new ClientRootInteraction.Label[3 + ((angledDamage_ != null) ? angledDamage_.Length : 0) + this.Interaction.TargetedDamage_.Count];
			builder.AddOperation(this.Id, array);
			ClientRootInteraction.Label label = builder.CreateUnresolvedLabel();
			array[0] = builder.CreateLabel();
			bool flag = this.Interaction.Failed != int.MinValue;
			if (flag)
			{
				ClientInteraction clientInteraction = module.Interactions[this.Interaction.Failed];
				clientInteraction.Compile(module, builder);
			}
			builder.Jump(label);
			array[1] = builder.CreateLabel();
			bool flag2 = this.Interaction.Next != int.MinValue;
			if (flag2)
			{
				ClientInteraction clientInteraction2 = module.Interactions[this.Interaction.Next];
				clientInteraction2.Compile(module, builder);
			}
			builder.Jump(label);
			array[2] = builder.CreateLabel();
			bool flag3 = this.Interaction.Blocked != int.MinValue;
			if (flag3)
			{
				ClientInteraction clientInteraction3 = module.Interactions[this.Interaction.Blocked];
				clientInteraction3.Compile(module, builder);
			}
			builder.Jump(label);
			int num = 3;
			bool flag4 = angledDamage_ != null;
			if (flag4)
			{
				foreach (AngledDamage angledDamage in angledDamage_)
				{
					array[num++] = builder.CreateLabel();
					bool flag5 = angledDamage.Next != int.MinValue;
					if (flag5)
					{
						ClientInteraction clientInteraction4 = module.Interactions[angledDamage.Next];
						clientInteraction4.Compile(module, builder);
					}
					builder.Jump(label);
				}
			}
			foreach (string key in this._sortedTargetDamageKeys)
			{
				TargetedDamage targetedDamage = this.Interaction.TargetedDamage_[key];
				array[num++] = builder.CreateLabel();
				bool flag6 = targetedDamage.Next != int.MinValue;
				if (flag6)
				{
					ClientInteraction clientInteraction5 = module.Interactions[targetedDamage.Next];
					clientInteraction5.Compile(module, builder);
				}
				builder.Jump(label);
			}
			builder.ResolveLabel(label);
		}

		// Token: 0x06005904 RID: 22788 RVA: 0x001B4008 File Offset: 0x001B2208
		protected override void Revert0(GameInstance gameInstance, InteractionType type, InteractionContext context)
		{
			Entity targetEntity = context.MetaStore.TargetEntity;
			bool flag = targetEntity != null;
			if (flag)
			{
				targetEntity.SetServerAnimation(null, 1, 0f, false);
				targetEntity.PredictedStatusCount--;
			}
			ParticleSystemProxy[] damageParticles = context.InstanceStore.DamageParticles;
			bool flag2 = damageParticles != null;
			if (flag2)
			{
				foreach (ParticleSystemProxy particleSystemProxy in damageParticles)
				{
					particleSystemProxy.Expire(true);
				}
			}
			bool flag3 = context.InstanceStore.DamageSoundEventReference.PlaybackId != -1;
			if (flag3)
			{
				gameInstance.AudioModule.ActionOnEvent(ref context.InstanceStore.DamageSoundEventReference, 0);
			}
			bool flag4 = context.InstanceStore.PredictedStats == null;
			if (!flag4)
			{
				for (int j = 0; j < context.InstanceStore.PredictedStats.Length; j++)
				{
					EntityStatUpdate update = context.InstanceStore.PredictedStats[j];
					gameInstance.LocalPlayer.CancelStatPrediction(this.Interaction.EntityStatsOnHit[j].EntityStatIndex, update);
				}
			}
		}

		// Token: 0x06005905 RID: 22789 RVA: 0x001B4129 File Offset: 0x001B2329
		protected override void MatchServer0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			context.State.State = context.ServerData.State;
			context.Jump(context.Labels[context.ServerData.NextLabel]);
		}

		// Token: 0x04003750 RID: 14160
		private const int FailedLabelIndex = 0;

		// Token: 0x04003751 RID: 14161
		private const int SuccessLabelIndex = 1;

		// Token: 0x04003752 RID: 14162
		private const int BlockedLabelIndex = 2;

		// Token: 0x04003753 RID: 14163
		private const int AngledLabelOffset = 3;

		// Token: 0x04003754 RID: 14164
		private string[] _sortedTargetDamageKeys;
	}
}
