using System;
using HytaleClient.Audio;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.InGame.Modules.Map;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x02000964 RID: 2404
	internal class PredictedProjectile : Entity, InteractionSource
	{
		// Token: 0x06004B0F RID: 19215 RVA: 0x00133098 File Offset: 0x00131298
		public PredictedProjectile(GameInstance gameInstance, int networkId, int creatorUuid, ProjectileConfig projectileConfig, Vector3 initialForce) : base(gameInstance, networkId)
		{
			this._config = projectileConfig;
			bool flag = this._config.PhysicsConfig_.Type == 0;
			if (flag)
			{
				this._creatorUuid = creatorUuid;
				this._blockCollisionProvider = new BlockCollisionProvider();
				this._blockCollisionProvider.RequestedCollisionMaterials = 6;
				this._blockCollisionProvider.ReportOverlaps = true;
				this._entityCollisionProvider = new EntityRefCollisionProvider();
				this._triggerTracker = new BlockTracker();
				this._restingSupport = new RestingSupport();
				this._velocity = default(Vector3);
				this._position = default(Vector3);
				this._movement = default(Vector3);
				this._reflected = default(Vector3);
				this._unreflected = default(Vector3);
				this._forceProviderEntity = new ForceProviderEntity(this);
				this._forceProviderEntity.Density = (float)this._config.PhysicsConfig_.Density;
				this._forceProviders = new ForceProvider[]
				{
					this._forceProviderEntity
				};
				this._forceProviderStandardState.NextTickVelocity = initialForce;
				this._impactConsumer = delegate(Entity this_, Vector3 hitPos, Entity target, string detailName)
				{
					InteractionType type = (target != null) ? 19 : 20;
					InteractionContext context = InteractionContext.ForProxy(this_, this._gameInstance.InventoryModule, type);
					Vector4 value = new Vector4(hitPos.X, hitPos.Y, hitPos.Z, 1f);
					this._gameInstance.InteractionModule.StartChain(context, type, InteractionModule.ClickType.None, null, (target != null) ? new int?(target.NetworkId) : null, new Vector4?(value), detailName);
				};
				this._bounceConsumer = delegate(GameInstance instance, Vector3 hitPos)
				{
					InteractionType interactionType = 21;
					InteractionContext interactionContext = InteractionContext.ForProxy(this, this._gameInstance.InventoryModule, interactionType);
					Vector4 value = new Vector4(hitPos.X, hitPos.Y, hitPos.Z, 1f);
					InteractionModule interactionModule = this._gameInstance.InteractionModule;
					InteractionContext context = interactionContext;
					InteractionType type = interactionType;
					InteractionModule.ClickType clickType = InteractionModule.ClickType.None;
					Action onCompletion = null;
					Vector4? hitPosition = new Vector4?(value);
					interactionModule.StartChain(context, type, clickType, onCompletion, null, hitPosition, null);
				};
			}
		}

		// Token: 0x06004B10 RID: 19216 RVA: 0x00133248 File Offset: 0x00131448
		public override void UpdateWithoutPosition(float deltaTime, float distanceToCamera, bool skipUpdateLogic = false)
		{
			base.UpdateWithoutPosition(deltaTime, distanceToCamera, skipUpdateLogic);
			bool flag = this.ServerEntity != null && this.ServerEntity.Disposed;
			if (flag)
			{
				this.ServerEntity = null;
				this.Removed = true;
				this._gameInstance.Engine.RunOnMainThread(this, delegate
				{
					this._gameInstance.EntityStoreModule.Despawn(this.NetworkId);
				}, true, false);
			}
			else
			{
				bool removed = this.Removed;
				if (!removed)
				{
					bool justSpawned = this._justSpawned;
					if (justSpawned)
					{
						InteractionType type = 18;
						InteractionContext context = InteractionContext.ForProxy(this, this._gameInstance.InventoryModule, type);
						this._gameInstance.InteractionModule.StartChain(context, type, InteractionModule.ClickType.None, null, null, null, null);
						this._justSpawned = false;
						bool removed2 = this.Removed;
						if (removed2)
						{
							return;
						}
					}
					float num = 1f / (float)this._gameInstance.ServerUpdatesPerSecond;
					num *= this._gameInstance.TimeDilationModifier;
					this._nextTickTime -= deltaTime;
					int num2 = 0;
					while (this._nextTickTime < 0f)
					{
						this._nextTickTime += num;
						num2++;
						PhysicsConfig.PhysicsType type2 = this._config.PhysicsConfig_.Type;
						PhysicsConfig.PhysicsType physicsType = type2;
						if (physicsType != null)
						{
							throw new ArgumentOutOfRangeException();
						}
						this.StandardPhysicsTick(num);
						bool flag2 = num2 > 5;
						if (flag2)
						{
							this._nextTickTime = num;
							break;
						}
					}
					this.LookOrientation = base.BodyOrientation;
				}
			}
		}

		// Token: 0x06004B11 RID: 19217 RVA: 0x001333D0 File Offset: 0x001315D0
		public static PredictedProjectile Spawn(Guid id, GameInstance gameInstance, ProjectileConfig config, PlayerEntity creator, Vector3 position, Vector3 direction)
		{
			EntityStoreModule entityStoreModule = gameInstance.EntityStoreModule;
			Vector3 vector = default(Vector3);
			Direction rotationOffset = config.RotationOffset;
			vector.Yaw = PhysicsMath.NormalizeTurnAngle(PhysicsMath.HeadingFromDirection(direction.X, direction.Z)) + rotationOffset.Yaw;
			vector.Pitch = PhysicsMath.PitchFromDirection(direction.X, direction.Y, direction.Z) + rotationOffset.Pitch;
			vector.Roll = rotationOffset.Roll;
			PhysicsMath.VectorFromAngles(vector.Yaw, vector.Pitch, ref direction);
			PredictedProjectile predictedProjectile = new PredictedProjectile(gameInstance, entityStoreModule.NextLocalEntityId(), creator.NetworkId, config, direction * (float)config.LaunchForce);
			predictedProjectile.PredictionId = new Guid?(id);
			predictedProjectile.Predictable = true;
			entityStoreModule.RegisterEntity(predictedProjectile);
			Vector3 calculatedOffset = PredictedProjectile.GetCalculatedOffset(config, vector.Pitch, vector.Yaw);
			position += calculatedOffset;
			predictedProjectile.SetSpawnTransform(position, vector, Vector3.Zero);
			predictedProjectile.SetCharacterModel(config.Model_, null);
			predictedProjectile.UpdateLight();
			bool debugPrediction = PredictedProjectile.DebugPrediction;
			if (debugPrediction)
			{
				predictedProjectile._topTint = new Vector3(1f, 0f, 1f);
				predictedProjectile._bottomTint = new Vector3(1f, 0f, 1f);
			}
			predictedProjectile.RecomputeDragFactors(predictedProjectile.Hitbox);
			gameInstance.AudioModule.TryRegisterSoundObject(position, vector, ref predictedProjectile.SoundObjectReference, false);
			uint networkWwiseId = ResourceManager.GetNetworkWwiseId(config.LaunchLocalSoundEventIndex);
			gameInstance.EntityStoreModule.QueueSoundEvent(networkWwiseId, predictedProjectile.NetworkId);
			return predictedProjectile;
		}

		// Token: 0x06004B12 RID: 19218 RVA: 0x00133578 File Offset: 0x00131778
		private static Vector3 GetCalculatedOffset(ProjectileConfig config, float pitch, float yaw)
		{
			Vector3 result = new Vector3(config.SpawnOffset.X, config.SpawnOffset.Y, config.SpawnOffset.Z);
			Quaternion quaternion = Quaternion.CreateFromAxisAngle(Vector3.Up, yaw);
			Quaternion quaternion2 = Quaternion.CreateFromAxisAngle(Vector3.Right, pitch);
			Vector3.Transform(ref result, ref quaternion2, out result);
			Vector3.Transform(ref result, ref quaternion, out result);
			return result;
		}

		// Token: 0x06004B13 RID: 19219 RVA: 0x001335E4 File Offset: 0x001317E4
		public bool TryGetInteractionId(InteractionType type, out int id)
		{
			bool flag = this._config.Interactions.TryGetValue(type, out id);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				id = int.MinValue;
				result = false;
			}
			return result;
		}

		// Token: 0x06004B14 RID: 19220 RVA: 0x0013361C File Offset: 0x0013181C
		private void StandardPhysicsTick(float dt)
		{
			PhysicsConfig physicsConfig_ = this._config.PhysicsConfig_;
			bool flag = this._state == PredictedProjectile.State.Inactive;
			if (flag)
			{
				this._velocity = Vector3.Zero;
			}
			else
			{
				bool flag2 = this._state == PredictedProjectile.State.Resting;
				if (flag2)
				{
					bool flag3 = this._forceProviderStandardState.ExternalForce.LengthSquared() == 0f && !this._restingSupport.HasChanged(this._gameInstance);
					if (flag3)
					{
						return;
					}
					this._state = PredictedProjectile.State.Active;
				}
				this._position = this._nextPosition;
				float mass = this._forceProviderEntity.GetMass(base.Hitbox.GetVolume());
				this._forceProviderStandardState.ConvertToForces(dt, mass);
				this._forceProviderStandardState.UpdateVelocity(ref this._velocity);
				bool flag4 = this._velocity.LengthSquared() * dt * dt >= 9.9999994E-11f || this._forceProviderStandardState.ExternalForce.LengthSquared() >= 0f;
				if (flag4)
				{
					this._state = PredictedProjectile.State.Active;
				}
				else
				{
					this._velocity = Vector3.Zero;
				}
				bool flag5 = this._state == PredictedProjectile.State.Resting && this._restingSupport.HasChanged(this._gameInstance);
				if (flag5)
				{
					this._state = PredictedProjectile.State.Active;
				}
				this._stateBefore.Position = this._position;
				this._stateBefore.Velocity = this._velocity;
				this._forceProviderEntity.ForceProviderStandardState = this._forceProviderStandardState;
				this._stateUpdater.Update(this._stateBefore, this._stateAfter, mass, dt, this._onGround, this._forceProviders);
				this._velocity = this._stateAfter.Velocity;
				this._movement = this._velocity * dt;
				this._forceProviderStandardState.Clear();
				bool flag6 = this._velocity.LengthSquared() * dt * dt >= 9.9999994E-11f;
				if (flag6)
				{
					this._state = PredictedProjectile.State.Active;
				}
				else
				{
					this._velocity = Vector3.Zero;
				}
				float num = 1f;
				bool provideCharacterCollisions = this._provideCharacterCollisions;
				if (provideCharacterCollisions)
				{
					Entity ignore = null;
					bool flag7 = this._creatorUuid != -1;
					if (flag7)
					{
						ignore = this._gameInstance.EntityStoreModule.GetEntity(this._creatorUuid);
					}
					num = this._entityCollisionProvider.ComputeNearest(this._gameInstance, base.Hitbox, this._position, this._movement, this, ignore);
					bool flag8 = num < 0f || num > 1f;
					if (flag8)
					{
						num = 1f;
					}
				}
				this._bounced = false;
				this._onGround = false;
				this._moveOutOfSolidVelocity = Vector3.Zero;
				this._movedInsideSolid = false;
				this._displacedMass = 0f;
				this._subSurfaceVolume = 0f;
				this._enterFluid = float.MaxValue;
				this._leaveFluid = float.MinValue;
				this._collisionStart = num;
				this._contactPosition = this._position + this._movement * this._collisionStart;
				this._contactNormal = Vector3.Zero;
				this._isSliding = true;
				Vector3 vector = this._position;
				this._nextMovement = Vector3.Zero;
				while (this._isSliding && this._movement != Vector3.Zero)
				{
					this._contactPosition = vector + this._movement * this._collisionStart;
					this._isSliding = false;
					this._blockCollisionProvider.Cast(this._gameInstance, base.Hitbox, vector, this._movement, this, this._triggerTracker, num);
					this._movement = this._nextMovement;
					vector = this._contactPosition;
				}
				this._movement = vector + this._nextMovement - this._position;
				this._fluidTracker.Reset();
				float density = (this._displacedMass > 0f) ? (this._displacedMass / this._subSurfaceVolume) : 1.2f;
				bool movedInsideSolid = this._movedInsideSolid;
				if (movedInsideSolid)
				{
					this._position += this._moveOutOfSolidVelocity * dt;
					this._velocity = this._moveOutOfSolidVelocity;
					this._forceProviderStandardState.DragCoefficient = this.GetDragCoefficient(density);
					this._forceProviderStandardState.DisplacedMass = this._displacedMass;
					this._forceProviderStandardState.Gravity = (float)physicsConfig_.Gravity;
					this.FinishTick();
				}
				else
				{
					float num2 = this._bounced ? this._collisionStart : 1f;
					bool flag9 = false;
					bool flag10 = !this._inFluid && this._enterFluid < this._collisionStart;
					if (flag10)
					{
						this._inFluid = true;
						num2 = this._enterFluid;
						this._velocityExtremaCount = 2;
						flag9 = true;
					}
					else
					{
						bool flag11 = this._inFluid && this._leaveFluid < this._collisionStart;
						if (flag11)
						{
							this._inFluid = false;
							num2 = this._leaveFluid;
							this._velocityExtremaCount = 2;
						}
					}
					bool flag12 = num2 > 0f && (double)num2 < 1.0;
					if (flag12)
					{
						this._stateUpdater.Update(this._stateBefore, this._stateAfter, mass, dt * num2, this._onGround, this._forceProviders);
						this._velocity = this._stateAfter.Velocity;
					}
					bool flag13 = this._inFluid && this._subSurfaceVolume < base.Hitbox.GetVolume();
					if (flag13)
					{
						bool flag14 = this._velocityExtremaCount > 0;
						if (flag14)
						{
							float y = this._stateBefore.Velocity.Y;
							float y2 = this._stateAfter.Velocity.Y;
							bool flag15 = y * y2 <= 0f;
							if (flag15)
							{
								this._velocityExtremaCount--;
							}
						}
					}
					bool isSwimming = this.IsSwimming;
					if (isSwimming)
					{
						ForceProviderStandardState forceProviderStandardState = this._forceProviderStandardState;
						forceProviderStandardState.ExternalForce.Y = forceProviderStandardState.ExternalForce.Y - this._stateAfter.Velocity.Y * ((float)physicsConfig_.SwimmingDampingFactor / mass);
					}
					bool flag16 = flag9;
					if (flag16)
					{
						this._forceProviderStandardState.ExternalImpulse += this._stateAfter.Velocity * (float)(-(float)physicsConfig_.HitWaterImpulseLoss) * mass;
					}
					this._forceProviderStandardState.DisplacedMass = this._displacedMass;
					this._forceProviderStandardState.DragCoefficient = this.GetDragCoefficient(density);
					this._forceProviderStandardState.Gravity = (float)physicsConfig_.Gravity;
					bool flag17 = this._entityCollisionProvider.Count > 0;
					if (flag17)
					{
						EntityContactData contact = this._entityCollisionProvider.GetContact(0);
						Entity entityReference = contact.EntityReference;
						bool flag18 = entityReference != null;
						if (flag18)
						{
							this._position = contact.CollisionPoint;
							this._state = PredictedProjectile.State.Inactive;
							bool flag19 = this._impactConsumer != null;
							if (flag19)
							{
								this._impactConsumer(this, this._position, entityReference, contact.CollisionDetailName);
							}
						}
						this.RotateBody(dt);
						this.FinishTick();
					}
					else
					{
						bool bounced = this._bounced;
						if (bounced)
						{
							this._position = this._contactPosition;
							this._bounces++;
							PredictedProjectile.ComputeReflectedVector(this._velocity, this._contactNormal, out this._velocity);
							bool flag20 = physicsConfig_.BounceCount == -1 || this._bounces <= physicsConfig_.BounceCount;
							if (flag20)
							{
								this._velocity *= (float)physicsConfig_.Bounciness;
							}
							bool flag21 = (physicsConfig_.BounceCount != -1 && this._bounces > physicsConfig_.BounceCount) || (double)(this._velocity.LengthSquared() * dt * dt) < physicsConfig_.BounceLimit * physicsConfig_.BounceLimit;
							if (flag21)
							{
								bool flag22 = this._contactNormal == Vector3.Up;
								bool flag23 = !physicsConfig_.AllowRolling && (physicsConfig_.SticksVertically || flag22);
								if (flag23)
								{
									this._state = PredictedProjectile.State.Resting;
									this._restingSupport.Rest(this._gameInstance, base.Hitbox, this._position);
									this._onGround = flag22;
									bool flag24 = this._impactConsumer != null;
									if (flag24)
									{
										this._impactConsumer(this, this._position, null, null);
									}
								}
								bool allowRolling = physicsConfig_.AllowRolling;
								if (allowRolling)
								{
									this._velocity.Y = 0f;
									this._velocity *= (float)physicsConfig_.RollingFrictionFactor;
									this._onGround = flag22;
								}
								else
								{
									this._velocity = Vector3.Zero;
								}
							}
							else
							{
								bool flag25 = this._bounceConsumer != null;
								if (flag25)
								{
									this._bounceConsumer(this._gameInstance, this._position);
								}
							}
							this.RotateBody(dt);
							this.FinishTick();
						}
						else
						{
							this._position += this._movement;
							this.RotateBody(dt);
							this.FinishTick();
						}
					}
				}
			}
		}

		// Token: 0x170011F9 RID: 4601
		// (get) Token: 0x06004B15 RID: 19221 RVA: 0x00133F2F File Offset: 0x0013212F
		public bool IsSwimming
		{
			get
			{
				return this._velocityExtremaCount <= 0;
			}
		}

		// Token: 0x06004B16 RID: 19222 RVA: 0x00133F40 File Offset: 0x00132140
		public PredictedProjectile.Result OnCollision(int blockX, int blockY, int blockZ, Vector3 direction, BlockContactData contactData, BlockData blockData, BoundingBox collider)
		{
			PhysicsConfig physicsConfig_ = this._config.PhysicsConfig_;
			BlockType.Material collisionMaterial = blockData.OriginalBlockType.CollisionMaterial;
			bool flag = physicsConfig_.MoveOutOfSolidSpeed > 0.0 && contactData.Overlapping && collisionMaterial == 1;
			PredictedProjectile.Result result;
			if (flag)
			{
				IntVector3? intVector = NearestBlockUtil.FindNearestBlock<MapModule>(this._position, delegate(IntVector3 block, MapModule w)
				{
					int block2 = w.GetBlock(block.X, block.Y, block.Z, int.MaxValue);
					return block2 != int.MaxValue && w.ClientBlockTypes[block2].CollisionMaterial != 1;
				}, this._gameInstance.MapModule);
				bool flag2 = intVector != null;
				Vector3 vector;
				if (flag2)
				{
					IntVector3 value = intVector.Value;
					vector = new Vector3((float)value.X, (float)value.Y, (float)value.Z);
					vector += new Vector3(0.5f, 0.5f, 0.5f);
					vector -= this._position;
					vector = vector.SetLength((float)physicsConfig_.MoveOutOfSolidSpeed);
				}
				else
				{
					vector = new Vector3(0f, (float)physicsConfig_.MoveOutOfSolidSpeed, 0f);
				}
				this._moveOutOfSolidVelocity += vector;
				this._movedInsideSolid = true;
				result = PredictedProjectile.Result.Continue;
			}
			else
			{
				bool flag3 = collisionMaterial == 2 && !this._fluidTracker.IsTracked(blockX, blockY, blockZ);
				if (flag3)
				{
					float collisionStart = contactData.CollisionStart;
					float collisionEnd = contactData.CollisionEnd;
					bool flag4 = collisionStart < this._enterFluid;
					if (flag4)
					{
						this._enterFluid = collisionStart;
					}
					bool flag5 = collisionEnd > this._leaveFluid;
					if (flag5)
					{
						this._leaveFluid = collisionEnd;
					}
					bool flag6 = collisionEnd <= collisionStart;
					if (flag6)
					{
						result = PredictedProjectile.Result.Continue;
					}
					else
					{
						float num = 1000f;
						float num2 = PhysicsMath.VolumeOfIntersection(base.Hitbox, this._contactPosition, collider, (float)blockX, (float)blockY, (float)blockZ);
						this._subSurfaceVolume += num2;
						this._displacedMass += num2 * num;
						this._fluidTracker.TrackNew(blockX, blockY, blockZ);
						result = PredictedProjectile.Result.Continue;
					}
				}
				else
				{
					bool overlapping = contactData.Overlapping;
					if (overlapping)
					{
						result = PredictedProjectile.Result.Continue;
					}
					else
					{
						float num3 = Vector3.Dot(direction, contactData.CollisionNormal);
						bool flag7 = collisionMaterial == 1 && (double)num3 == 0.0;
						if (flag7)
						{
						}
						bool flag8 = num3 >= 0f;
						if (flag8)
						{
							result = PredictedProjectile.Result.Continue;
						}
						else
						{
							this._contactPosition = contactData.CollisionPoint;
							this._contactNormal = contactData.CollisionNormal;
							bool allowRolling = physicsConfig_.AllowRolling;
							if (allowRolling)
							{
								Vector3 vector2 = this._stateBefore.Position + this._movement - this._contactPosition;
								bool flag9 = vector2 != Vector3.Zero;
								if (flag9)
								{
									float num4 = Vector3.Dot(vector2, this._contactNormal);
									this._nextMovement = vector2;
									this._nextMovement += this._contactNormal * -num4;
									this._isSliding = true;
								}
							}
							this._collisionStart = contactData.CollisionStart;
							this._bounced = true;
							result = PredictedProjectile.Result.Stop;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06004B17 RID: 19223 RVA: 0x00134260 File Offset: 0x00132460
		public PredictedProjectile.Result ProbeCollisionDamage(int blockX, int blockY, int blockZ, Vector3 direction, BlockContactData collisionData, BlockData blockData)
		{
			return PredictedProjectile.Result.Continue;
		}

		// Token: 0x06004B18 RID: 19224 RVA: 0x00134273 File Offset: 0x00132473
		public void OnCollisionDamage(int blockX, int blockY, int blockZ, Vector3 direction, BlockContactData collisionData, BlockData blockData)
		{
		}

		// Token: 0x06004B19 RID: 19225 RVA: 0x00134278 File Offset: 0x00132478
		public PredictedProjectile.Result ProbeCollisionTrigger(int blockX, int blockY, int blockZ, Vector3 direction, BlockContactData collisionData, BlockData blockData)
		{
			return PredictedProjectile.Result.Continue;
		}

		// Token: 0x06004B1A RID: 19226 RVA: 0x0013428C File Offset: 0x0013248C
		public void OnCollisionTriggerEnter(int blockX, int blockY, int blockZ, Vector3 direction, BlockContactData collisionData, BlockData blockData)
		{
			bool executeTriggers = this._executeTriggers;
			if (executeTriggers)
			{
			}
		}

		// Token: 0x06004B1B RID: 19227 RVA: 0x001342A8 File Offset: 0x001324A8
		public void OnCollisionTrigger(int blockX, int blockY, int blockZ, Vector3 direction, BlockContactData collisionData, BlockData blockData)
		{
			bool executeTriggers = this._executeTriggers;
			if (executeTriggers)
			{
			}
		}

		// Token: 0x06004B1C RID: 19228 RVA: 0x001342C4 File Offset: 0x001324C4
		public void OnCollisionTriggerExit(int blockX, int blockY, int blockZ)
		{
			bool executeTriggers = this._executeTriggers;
			if (executeTriggers)
			{
			}
		}

		// Token: 0x06004B1D RID: 19229 RVA: 0x001342E0 File Offset: 0x001324E0
		public PredictedProjectile.Result OnCollisionSliceFinished()
		{
			return PredictedProjectile.Result.Continue;
		}

		// Token: 0x06004B1E RID: 19230 RVA: 0x001342F3 File Offset: 0x001324F3
		public void OnCollisionFinished()
		{
		}

		// Token: 0x06004B1F RID: 19231 RVA: 0x001342F6 File Offset: 0x001324F6
		protected void FinishTick()
		{
			base.SetPosition(this._position);
			this._entityCollisionProvider.Clear();
		}

		// Token: 0x06004B20 RID: 19232 RVA: 0x00134314 File Offset: 0x00132514
		protected void RotateBody(float dt)
		{
			PhysicsConfig physicsConfig_ = this._config.PhysicsConfig_;
			bool flag = !physicsConfig_.ComputeYaw && !physicsConfig_.ComputePitch;
			if (!flag)
			{
				float x = this._stateAfter.Velocity.X;
				float z = this._stateAfter.Velocity.Z;
				bool flag2 = x * x + z * z <= 9.9999994E-11f;
				if (!flag2)
				{
					Vector3 nextBodyOrientation = this._nextBodyOrientation;
					switch (physicsConfig_.RotationMode_)
					{
					case 0:
						break;
					case 1:
					{
						bool computeYaw = physicsConfig_.ComputeYaw;
						if (computeYaw)
						{
							nextBodyOrientation.Yaw = PhysicsMath.NormalizeTurnAngle(PhysicsMath.HeadingFromDirection(x, z));
						}
						bool computePitch = physicsConfig_.ComputePitch;
						if (computePitch)
						{
							nextBodyOrientation.Pitch = PhysicsMath.PitchFromDirection(x, this._stateAfter.Velocity.Y, z);
						}
						break;
					}
					case 2:
					{
						bool computeYaw2 = physicsConfig_.ComputeYaw;
						if (computeYaw2)
						{
							nextBodyOrientation.Yaw = PhysicsMath.NormalizeTurnAngle(PhysicsMath.HeadingFromDirection(x, z));
						}
						bool computePitch2 = physicsConfig_.ComputePitch;
						if (computePitch2)
						{
							float pitch = nextBodyOrientation.Pitch;
							float num = PhysicsMath.PitchFromDirection(x, this._velocity.Y, z);
							float num2 = PhysicsMath.NormalizeTurnAngle(num - pitch);
							float num3 = this._velocity.LengthSquared() * dt * physicsConfig_.SpeedRotationFactor;
							bool flag3 = num2 > num3;
							if (flag3)
							{
								num = pitch + num3;
								num2 = num3;
							}
							else
							{
								bool flag4 = num2 < -num3;
								if (flag4)
								{
									num = pitch - num3;
									num2 = num3;
								}
							}
							nextBodyOrientation.Pitch = num;
							this._forceProviderStandardState.ExternalForce += this._stateAfter.Velocity * (num2 * -(float)physicsConfig_.RotationForce);
						}
						break;
					}
					case 3:
						nextBodyOrientation.Yaw = PhysicsMath.NormalizeTurnAngle(PhysicsMath.HeadingFromDirection(x, z));
						nextBodyOrientation.Pitch -= this._stateBefore.Velocity.Length() * physicsConfig_.RollingSpeed;
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
					base.SetBodyOrientation(nextBodyOrientation);
				}
			}
		}

		// Token: 0x06004B21 RID: 19233 RVA: 0x00134544 File Offset: 0x00132744
		public static void ComputeReflectedVector(Vector3 vec, Vector3 normal, out Vector3 result)
		{
			result = vec;
			float num = normal.LengthSquared();
			bool flag = num == 0f;
			if (!flag)
			{
				float num2 = Vector3.Dot(vec, normal) / num;
				result += normal * (-2f * num2);
			}
		}

		// Token: 0x06004B22 RID: 19234 RVA: 0x00134598 File Offset: 0x00132798
		protected float GetDragCoefficient(float density)
		{
			return this._dragMultiplier * density + this._dragOffset;
		}

		// Token: 0x06004B23 RID: 19235 RVA: 0x001345BC File Offset: 0x001327BC
		protected void RecomputeDragFactors(BoundingBox boundingBox)
		{
			PhysicsConfig physicsConfig_ = this._config.PhysicsConfig_;
			Vector3 size = boundingBox.GetSize();
			float area = size.X * size.Z;
			float mass = this._forceProviderEntity.GetMass(boundingBox.GetVolume());
			float num = PhysicsMath.ComputeDragCoefficient((float)physicsConfig_.TerminalVelocityAir, area, mass, (float)physicsConfig_.Gravity);
			float num2 = PhysicsMath.ComputeDragCoefficient((float)physicsConfig_.TerminalVelocityWater, area, mass, (float)physicsConfig_.Gravity);
			this._dragMultiplier = (num2 - num) / (float)(physicsConfig_.DensityWater - physicsConfig_.DensityAir);
			this._dragOffset = num - this._dragMultiplier * (float)physicsConfig_.DensityAir;
		}

		// Token: 0x040026AA RID: 9898
		public static bool DebugPrediction;

		// Token: 0x040026AB RID: 9899
		private float _nextTickTime;

		// Token: 0x040026AC RID: 9900
		private ProjectileConfig _config;

		// Token: 0x040026AD RID: 9901
		private bool _justSpawned = true;

		// Token: 0x040026AE RID: 9902
		protected const int WaterDetectionExtremaCount = 2;

		// Token: 0x040026AF RID: 9903
		protected readonly BlockCollisionProvider _blockCollisionProvider;

		// Token: 0x040026B0 RID: 9904
		protected readonly EntityRefCollisionProvider _entityCollisionProvider;

		// Token: 0x040026B1 RID: 9905
		protected readonly BlockTracker _triggerTracker;

		// Token: 0x040026B2 RID: 9906
		protected readonly RestingSupport _restingSupport;

		// Token: 0x040026B3 RID: 9907
		protected Vector3 _velocity;

		// Token: 0x040026B4 RID: 9908
		protected Vector3 _position;

		// Token: 0x040026B5 RID: 9909
		protected Vector3 _movement;

		// Token: 0x040026B6 RID: 9910
		protected Vector3 _nextMovement = default(Vector3);

		// Token: 0x040026B7 RID: 9911
		protected bool _bounced;

		// Token: 0x040026B8 RID: 9912
		protected int _bounces = 0;

		// Token: 0x040026B9 RID: 9913
		protected bool _onGround;

		// Token: 0x040026BA RID: 9914
		protected Vector3 _reflected;

		// Token: 0x040026BB RID: 9915
		protected Vector3 _unreflected;

		// Token: 0x040026BC RID: 9916
		protected bool _provideCharacterCollisions = true;

		// Token: 0x040026BD RID: 9917
		protected readonly int _creatorUuid = -1;

		// Token: 0x040026BE RID: 9918
		protected bool _executeTriggers;

		// Token: 0x040026BF RID: 9919
		protected Action<GameInstance, Vector3> _bounceConsumer;

		// Token: 0x040026C0 RID: 9920
		protected Action<Entity, Vector3, Entity, string> _impactConsumer;

		// Token: 0x040026C1 RID: 9921
		protected bool _movedInsideSolid;

		// Token: 0x040026C2 RID: 9922
		protected Vector3 _moveOutOfSolidVelocity = default(Vector3);

		// Token: 0x040026C3 RID: 9923
		protected Vector3 _contactPosition;

		// Token: 0x040026C4 RID: 9924
		protected Vector3 _contactNormal;

		// Token: 0x040026C5 RID: 9925
		protected float _collisionStart;

		// Token: 0x040026C6 RID: 9926
		protected PhysicsBodyStateUpdater _stateUpdater = new PhysicsBodyStateUpdaterSymplecticEuler();

		// Token: 0x040026C7 RID: 9927
		protected PhysicsBodyState _stateBefore = new PhysicsBodyState();

		// Token: 0x040026C8 RID: 9928
		protected PhysicsBodyState _stateAfter = new PhysicsBodyState();

		// Token: 0x040026C9 RID: 9929
		protected float _displacedMass;

		// Token: 0x040026CA RID: 9930
		protected float _subSurfaceVolume;

		// Token: 0x040026CB RID: 9931
		protected float _enterFluid;

		// Token: 0x040026CC RID: 9932
		protected float _leaveFluid;

		// Token: 0x040026CD RID: 9933
		protected bool _inFluid;

		// Token: 0x040026CE RID: 9934
		protected int _velocityExtremaCount = int.MaxValue;

		// Token: 0x040026CF RID: 9935
		protected PredictedProjectile.State _state = PredictedProjectile.State.Active;

		// Token: 0x040026D0 RID: 9936
		protected ForceProviderEntity _forceProviderEntity;

		// Token: 0x040026D1 RID: 9937
		protected ForceProvider[] _forceProviders;

		// Token: 0x040026D2 RID: 9938
		protected readonly ForceProviderStandardState _forceProviderStandardState = new ForceProviderStandardState();

		// Token: 0x040026D3 RID: 9939
		protected float _dragMultiplier;

		// Token: 0x040026D4 RID: 9940
		protected float _dragOffset;

		// Token: 0x040026D5 RID: 9941
		protected readonly BlockTracker _fluidTracker = new BlockTracker();

		// Token: 0x040026D6 RID: 9942
		protected bool _isSliding;

		// Token: 0x02000E4D RID: 3661
		public enum Result
		{
			// Token: 0x040045F2 RID: 17906
			Continue,
			// Token: 0x040045F3 RID: 17907
			Stop,
			// Token: 0x040045F4 RID: 17908
			StopNow
		}

		// Token: 0x02000E4E RID: 3662
		internal enum State
		{
			// Token: 0x040045F6 RID: 17910
			Active,
			// Token: 0x040045F7 RID: 17911
			Resting,
			// Token: 0x040045F8 RID: 17912
			Inactive
		}
	}
}
