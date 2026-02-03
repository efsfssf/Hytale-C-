using System;
using System.Collections.Generic;
using HytaleClient.Audio;
using HytaleClient.Data.ClientInteraction.Client;
using HytaleClient.Data.ClientInteraction.None;
using HytaleClient.Data.ClientInteraction.Server;
using HytaleClient.Data.Items;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction
{
	// Token: 0x02000B11 RID: 2833
	internal abstract class ClientInteraction
	{
		// Token: 0x0600589E RID: 22686 RVA: 0x001B0D2C File Offset: 0x001AEF2C
		public ClientInteraction(int id, Interaction interaction)
		{
			this.Id = id;
			this.Interaction = interaction;
			this.Tags = ((interaction.Tags != null) ? new HashSet<int>(interaction.Tags) : new HashSet<int>());
			this.Rules = new ClientInteraction.ClientInteractionRules(interaction.Rules);
		}

		// Token: 0x0600589F RID: 22687 RVA: 0x001B0D80 File Offset: 0x001AEF80
		public void Tick(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context)
		{
			int operationCounter = context.OperationCounter;
			int callDepth = context.Chain.GetCallDepth();
			bool flag = !this.TickInternal(gameInstance, clickType, hasAnyButtonClick, firstRun, time, type, context);
			if (flag)
			{
				this.Tick0(gameInstance, clickType, hasAnyButtonClick, firstRun, time, type, context);
			}
			InteractionState state = context.State.State;
			InteractionState interactionState = state;
			if (interactionState <= 1 || interactionState == 3)
			{
				bool flag2 = context.InstanceStore.TimeShift != null;
				if (flag2)
				{
					context.SetTimeShift(context.InstanceStore.TimeShift.Value);
				}
				bool flag3 = context.OperationCounter == operationCounter && callDepth == context.Chain.GetCallDepth();
				if (flag3)
				{
					context.OperationCounter++;
				}
			}
		}

		// Token: 0x060058A0 RID: 22688 RVA: 0x001B0E54 File Offset: 0x001AF054
		private bool TickInternal(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context)
		{
			InteractionSettings interactionSettings;
			this.Interaction.Settings.TryGetValue(gameInstance.GameMode, out interactionSettings);
			bool flag = !firstRun;
			if (flag)
			{
				bool flag2 = (context.AllowSkipChainOnClick || (interactionSettings != null && interactionSettings.AllowSkipOnClick)) && hasAnyButtonClick;
				if (flag2)
				{
					context.State.State = 1;
					context.State.Progress = time;
					return true;
				}
			}
			bool flag3 = !ClientInteraction.Failed(context.State.State);
			if (flag3)
			{
				float val = 0f;
				bool flag4 = this.Interaction.Effects != null && this.Interaction.Effects.WaitForAnimationToFinish;
				if (flag4)
				{
					ItemLibraryModule itemLibraryModule = gameInstance.ItemLibraryModule;
					ClientItemStack heldItem = context.HeldItem;
					ClientItemBase item = itemLibraryModule.GetItem((heldItem != null) ? heldItem.Id : null);
					Interaction.InteractionEffects effects = this.Interaction.Effects;
					EntityAnimation entityAnimation = (((effects != null) ? effects.ItemAnimationId : null) != null) ? ((item != null) ? item.GetAnimation(this.Interaction.Effects.ItemAnimationId) : null) : null;
					val = ((entityAnimation != null) ? ((float)entityAnimation.FirstPersonData.Duration * entityAnimation.Speed / 60f) : 0f);
				}
				float num = Math.Max(this.Interaction.RunTime, val);
				bool flag5 = time < num;
				if (flag5)
				{
					context.State.State = 4;
				}
				else
				{
					bool flag6 = num > 0f;
					if (flag6)
					{
						context.InstanceStore.TimeShift = new float?(time - num);
					}
					context.State.State = 0;
				}
				context.State.Progress = time;
			}
			return false;
		}

		// Token: 0x060058A1 RID: 22689
		protected abstract void Tick0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context);

		// Token: 0x060058A2 RID: 22690 RVA: 0x001B1014 File Offset: 0x001AF214
		public virtual void Handle(GameInstance gameInstance, bool firstRun, float time, InteractionType type, InteractionContext context)
		{
			InteractionSyncData state = context.State;
			bool flag = this.Interaction.Camera != null;
			if (flag)
			{
				bool flag2 = this.Interaction.Camera.FirstPerson != null;
				if (flag2)
				{
					bool flag3 = state.State == 4;
					if (flag3)
					{
						bool flag4 = time >= context.InstanceStore.LastFirstPersonCameraTime;
						if (flag4)
						{
							InteractionCamera[] firstPerson = this.Interaction.Camera.FirstPerson;
							int i = 0;
							while (i < firstPerson.Length)
							{
								InteractionCamera interactionCamera = firstPerson[i];
								bool flag5 = interactionCamera.Time <= context.InstanceStore.LastFirstPersonCameraTime;
								if (flag5)
								{
									i++;
								}
								else
								{
									bool flag6 = time > interactionCamera.Time;
									if (flag6)
									{
										break;
									}
									context.InstanceStore.LastFirstPersonCameraTime = interactionCamera.Time;
									gameInstance.CharacterControllerModule.MovementController.SetFirstPersonCameraOffset(interactionCamera.Time - time, new Vector3(interactionCamera.Position.X, interactionCamera.Position.Y, interactionCamera.Position.Z), new Vector3(interactionCamera.Rotation.Pitch, interactionCamera.Rotation.Yaw, interactionCamera.Rotation.Roll));
									break;
								}
							}
						}
					}
					else
					{
						gameInstance.CharacterControllerModule.MovementController.SetFirstPersonCameraOffset(0f, Vector3.Zero, Vector3.Zero);
					}
				}
				bool flag7 = this.Interaction.Camera.ThirdPerson != null;
				if (flag7)
				{
					bool flag8 = state.State == 4;
					if (flag8)
					{
						bool flag9 = time >= context.InstanceStore.LastThirdPersonCameraTime;
						if (flag9)
						{
							InteractionCamera[] thirdPerson = this.Interaction.Camera.ThirdPerson;
							int j = 0;
							while (j < thirdPerson.Length)
							{
								InteractionCamera interactionCamera2 = thirdPerson[j];
								bool flag10 = interactionCamera2.Time <= context.InstanceStore.LastThirdPersonCameraTime;
								if (flag10)
								{
									j++;
								}
								else
								{
									bool flag11 = time > interactionCamera2.Time;
									if (flag11)
									{
										break;
									}
									context.InstanceStore.LastThirdPersonCameraTime = interactionCamera2.Time;
									gameInstance.CharacterControllerModule.MovementController.SetThirdPersonCameraOffset(interactionCamera2.Time - time, new Vector3(interactionCamera2.Position.X, interactionCamera2.Position.Y, interactionCamera2.Position.Z), new Vector3(interactionCamera2.Rotation.Pitch, interactionCamera2.Rotation.Yaw, interactionCamera2.Rotation.Roll));
									break;
								}
							}
						}
					}
					else
					{
						gameInstance.CharacterControllerModule.MovementController.SetThirdPersonCameraOffset(0f, Vector3.Zero, Vector3.Zero);
					}
				}
			}
			bool isFirstInteraction = context.Chain.OperationIndex == 0;
			bool flag12 = state.State != 4;
			if (flag12)
			{
				bool flag13 = firstRun && state.State == 0;
				if (flag13)
				{
					this.HandlePlayFor(gameInstance, context.Entity, type, context, context.InstanceStore, false, isFirstInteraction, false);
				}
				this.HandlePlayFor(gameInstance, context.Entity, type, context, context.InstanceStore, true, isFirstInteraction, false);
			}
			else if (firstRun)
			{
				this.HandlePlayFor(gameInstance, context.Entity, type, context, context.InstanceStore, false, isFirstInteraction, false);
			}
		}

		// Token: 0x060058A3 RID: 22691 RVA: 0x001B139C File Offset: 0x001AF59C
		public void Revert(GameInstance gameInstance, InteractionType type, InteractionContext context)
		{
			bool isFirstInteraction = context.Chain.OperationIndex == 0;
			this.HandlePlayFor(gameInstance, context.Entity, type, context, context.InstanceStore, true, isFirstInteraction, true);
			this.Revert0(gameInstance, type, context);
		}

		// Token: 0x060058A4 RID: 22692
		protected abstract void Revert0(GameInstance gameInstance, InteractionType type, InteractionContext context);

		// Token: 0x060058A5 RID: 22693 RVA: 0x001B13DC File Offset: 0x001AF5DC
		public void MatchServer(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			int operationCounter = context.OperationCounter;
			int callDepth = context.Chain.GetCallDepth();
			this.MatchServer0(gameInstance, clickType, hasAnyButtonClick, type, context);
			InteractionState state = context.State.State;
			InteractionState interactionState = state;
			if (interactionState <= 1 || interactionState == 3)
			{
				bool flag = context.OperationCounter == operationCounter && callDepth == context.Chain.GetCallDepth();
				if (flag)
				{
					context.OperationCounter++;
				}
			}
		}

		// Token: 0x060058A6 RID: 22694
		protected abstract void MatchServer0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context);

		// Token: 0x060058A7 RID: 22695 RVA: 0x001B145E File Offset: 0x001AF65E
		public virtual void Compile(InteractionModule module, ClientRootInteraction.OperationsBuilder builder)
		{
			builder.AddOperation(this.Id);
		}

		// Token: 0x060058A8 RID: 22696 RVA: 0x001B1470 File Offset: 0x001AF670
		public virtual InteractionChain MapForkChain(InteractionContext context, InteractionChainData data)
		{
			return null;
		}

		// Token: 0x060058A9 RID: 22697 RVA: 0x001B1484 File Offset: 0x001AF684
		public void HandlePlayFor(GameInstance gameInstance, Entity entity, InteractionType type, InteractionContext context, InteractionMetaStore metaStore, bool cancel, bool isFirstInteraction, bool force = false)
		{
			bool flag = !cancel;
			if (flag)
			{
				entity.RunningInteractions.Add(this.Id);
			}
			else
			{
				entity.RunningInteractions.Remove(this.Id);
			}
			Interaction.InteractionEffects effects = this.Interaction.Effects;
			string text = (effects != null) ? effects.ItemAnimationId : null;
			ItemLibraryModule itemLibraryModule = gameInstance.ItemLibraryModule;
			ClientItemStack heldItem = context.HeldItem;
			ClientItemBase item = itemLibraryModule.GetItem((heldItem != null) ? heldItem.Id : null);
			bool flag2 = entity.NetworkId == gameInstance.LocalPlayer.NetworkId;
			EntityAnimation entityAnimation = null;
			bool flag3 = text != null;
			if (flag3)
			{
				ClientItemPlayerAnimations clientItemPlayerAnimations;
				bool flag4 = effects != null && effects.ItemPlayerAnimationsId != null && gameInstance.ItemLibraryModule.GetItemPlayerAnimation(effects.ItemPlayerAnimationsId, out clientItemPlayerAnimations);
				if (flag4)
				{
					clientItemPlayerAnimations.Animations.TryGetValue(text, out entityAnimation);
				}
				else
				{
					bool flag5 = item != null;
					if (flag5)
					{
						entityAnimation = item.GetAnimation(text);
					}
					else
					{
						gameInstance.ItemLibraryModule.DefaultItemPlayerAnimations.Animations.TryGetValue(text, out entityAnimation);
					}
				}
			}
			bool flag6 = isFirstInteraction && entityAnimation == null;
			if (flag6)
			{
				text = ((type == 1) ? "SecondaryAction" : "Attack");
				entityAnimation = ((item != null) ? item.GetAnimation(text) : null);
			}
			bool flag7 = flag2 && ((effects != null) ? effects.MovementEffects_ : null) != null;
			if (flag7)
			{
				gameInstance.LocalPlayer.UpdateActiveInteraction(this.Id, cancel);
			}
			if (cancel)
			{
				bool flag8 = type == 6 && !flag2;
				if (flag8)
				{
					entity.RestoreCharacterItem();
				}
				bool flag9 = entityAnimation != null && effects != null && (effects.ClearAnimationOnFinish || force);
				if (flag9)
				{
					entity.SetActionAnimation(EntityAnimation.Empty, 0f, false, false);
				}
				bool flag10 = effects != null && effects.ClearSoundEventOnFinish && metaStore.SoundEventReference.PlaybackId != -1;
				if (flag10)
				{
					gameInstance.AudioModule.ActionOnEvent(ref metaStore.SoundEventReference, 0);
				}
			}
			else
			{
				bool flag11 = type == 6 && !flag2;
				if (flag11)
				{
					entity.SetCharacterItemConsumable();
				}
				bool flag12 = entityAnimation != null;
				if (flag12)
				{
					PlayerEntity playerEntity = entity as PlayerEntity;
					bool flag13 = playerEntity != null;
					if (flag13)
					{
						playerEntity.CurrentFirstPersonAnimationId = text;
					}
					entity.SetActionAnimation(entityAnimation, 0f, this.Interaction.AllowIndefiniteHold, true);
				}
				bool flag14 = effects != null;
				if (flag14)
				{
					ModelParticle[] array = (gameInstance.CameraModule.Controller.IsFirstPerson && flag2 && effects.FirstPersonParticles != null) ? effects.FirstPersonParticles : effects.Particles;
					bool flag15 = array != null || effects.Trails != null;
					if (flag15)
					{
						entity.ClearCombatSequenceEffects();
						entity.AddCombatSequenceEffects(array, effects.Trails);
					}
					uint networkWwiseId = ResourceManager.GetNetworkWwiseId(effects.LocalSoundEventIndex);
					bool flag16 = flag2;
					if (flag16)
					{
						bool ignoreSoundObject = effects.IgnoreSoundObject;
						if (ignoreSoundObject)
						{
							gameInstance.AudioModule.PlaySoundEvent(networkWwiseId, entity.Position, entity.BodyOrientation, ref metaStore.SoundEventReference);
						}
						else
						{
							gameInstance.AudioModule.PlaySoundEvent(networkWwiseId, entity.SoundObjectReference, ref metaStore.SoundEventReference);
						}
						bool flag17 = effects.CameraShake != null;
						if (flag17)
						{
							gameInstance.CameraModule.CameraShakeController.PlayCameraShake(effects.CameraShake.CameraShakeId, effects.CameraShake.Intensity, effects.CameraShake.Mode);
						}
					}
					uint networkWwiseId2 = ResourceManager.GetNetworkWwiseId(effects.WorldSoundEventIndex);
					bool flag18 = networkWwiseId2 > 0U;
					if (flag18)
					{
						bool flag19 = flag2;
						if (!flag19)
						{
							bool flag20 = networkWwiseId == 0U || !flag2;
							if (flag20)
							{
								bool ignoreSoundObject2 = effects.IgnoreSoundObject;
								if (ignoreSoundObject2)
								{
									gameInstance.AudioModule.PlaySoundEvent(networkWwiseId2, entity.Position, entity.BodyOrientation, ref metaStore.SoundEventReference);
								}
								else
								{
									gameInstance.AudioModule.PlaySoundEvent(networkWwiseId2, entity.SoundObjectReference, ref metaStore.SoundEventReference);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060058AA RID: 22698 RVA: 0x001B1884 File Offset: 0x001AFA84
		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}: {3}", new object[]
			{
				"Id",
				this.Id,
				"Interaction",
				this.Interaction
			});
		}

		// Token: 0x060058AB RID: 22699 RVA: 0x001B18D0 File Offset: 0x001AFAD0
		public static ClientInteraction Parse(int id, Interaction interaction)
		{
			ClientInteraction result;
			switch (interaction.Type_)
			{
			case 0:
				result = new SimpleInteraction(id, interaction);
				break;
			case 1:
				result = new SimpleBlockInteraction(id, interaction);
				break;
			case 2:
				result = new PlaceBlockInteraction(id, interaction);
				break;
			case 3:
				result = new BreakBlockInteraction(id, interaction);
				break;
			case 4:
				result = new PickBlockInteraction(id, interaction);
				break;
			case 5:
				result = new UseBlockInteraction(id, interaction);
				break;
			case 6:
				result = new UseEntityInteraction(id, interaction);
				break;
			case 7:
				result = new BuilderToolInteraction(id, interaction);
				break;
			case 8:
				result = new ModifyInventoryInteraction(id, interaction);
				break;
			case 9:
				result = new ChargingInteraction(id, interaction);
				break;
			case 10:
				result = new ChainingInteraction(id, interaction);
				break;
			case 11:
				result = new ConditionInteraction(id, interaction);
				break;
			case 12:
				result = new StatsConditionInteraction(id, interaction);
				break;
			case 13:
				result = new BlockConditionInteraction(id, interaction);
				break;
			case 14:
				result = new ReplaceInteraction(id, interaction);
				break;
			case 15:
				result = new ChangeBlockInteraction(id, interaction);
				break;
			case 16:
				result = new ChangeStateInteraction(id, interaction);
				break;
			case 17:
				result = new ConditionalPlaceCropInteraction(id, interaction);
				break;
			case 18:
				result = new FirstClickInteraction(id, interaction);
				break;
			case 19:
				result = new RefillContainerInteraction(id, interaction);
				break;
			case 20:
				result = new SelectInteraction(id, interaction);
				break;
			case 21:
				result = new DamageEntityInteraction(id, interaction);
				break;
			case 22:
				result = new RepeatInteraction(id, interaction);
				break;
			case 23:
				result = new ParallelInteraction(id, interaction);
				break;
			case 24:
				result = new ChangeActiveSlotInteraction(id, interaction);
				break;
			case 25:
				result = new WieldingInteraction(id, interaction);
				break;
			case 26:
				result = new EffectConditionInteraction(id, interaction);
				break;
			case 27:
				result = new ApplyForceInteraction(id, interaction);
				break;
			case 28:
				result = new ApplyEffectInteraction(id, interaction);
				break;
			case 29:
				result = new ClearEntityEffectInteraction(id, interaction);
				break;
			case 30:
				result = new SerialInteraction(id, interaction);
				break;
			case 31:
				result = new ChangeStatInteraction(id, interaction);
				break;
			case 32:
				result = new MovementConditionInteraction(id, interaction);
				break;
			case 33:
				result = new ProjectileInteraction(id, interaction);
				break;
			case 34:
				result = new RemoveEntityInteraction(id, interaction);
				break;
			case 35:
				result = new ResetCooldownInteraction(id, interaction);
				break;
			case 36:
				result = new TriggerCooldownInteraction(id, interaction);
				break;
			case 37:
				result = new CooldownConditionInteraction(id, interaction);
				break;
			case 38:
				result = new ChainFlagInteraction(id, interaction);
				break;
			case 39:
				result = new IncrementCooldown(id, interaction);
				break;
			case 40:
				result = new CancelChainInteraction(id, interaction);
				break;
			case 41:
				result = new SetChainVariableInteraction(id, interaction);
				break;
			case 42:
				result = new EvaluateChainVariableInteraction(id, interaction);
				break;
			case 43:
				result = new ClearChainVariableInteraction(id, interaction);
				break;
			case 44:
				result = new RunRootInteraction(id, interaction);
				break;
			default:
				throw new Exception(string.Format("Unknown Interaction type: {0}", interaction.Type_));
			}
			return result;
		}

		// Token: 0x060058AC RID: 22700 RVA: 0x001B1BEC File Offset: 0x001AFDEC
		public static bool Failed(InteractionState state)
		{
			bool result;
			switch (state)
			{
			case 0:
			case 4:
				result = false;
				break;
			case 1:
			case 2:
			case 3:
				result = true;
				break;
			default:
				throw new Exception("Unknown state: " + state.ToString());
			}
			return result;
		}

		// Token: 0x060058AD RID: 22701 RVA: 0x001B1C40 File Offset: 0x001AFE40
		public static Entity GetEntity(GameInstance gameInstance, InteractionContext context, InteractionTarget target)
		{
			Entity result;
			switch (target)
			{
			case 0:
				result = context.Entity;
				break;
			case 1:
				result = gameInstance.LocalPlayer;
				break;
			case 2:
				result = context.MetaStore.TargetEntity;
				break;
			default:
				throw new ArgumentOutOfRangeException("target", target, null);
			}
			return result;
		}

		// Token: 0x060058AE RID: 22702 RVA: 0x001B1C9C File Offset: 0x001AFE9C
		public static bool TryGetEntity(GameInstance gameInstance, InteractionContext context, InteractionTarget target, out Entity entity)
		{
			entity = ClientInteraction.GetEntity(gameInstance, context, target);
			return entity != null;
		}

		// Token: 0x04003721 RID: 14113
		public const float DefaultCooldownTimeSeconds = 0.35f;

		// Token: 0x04003722 RID: 14114
		public const int UndefinedAsset = -2147483648;

		// Token: 0x04003723 RID: 14115
		public readonly int Id;

		// Token: 0x04003724 RID: 14116
		public readonly Interaction Interaction;

		// Token: 0x04003725 RID: 14117
		public readonly HashSet<int> Tags;

		// Token: 0x04003726 RID: 14118
		public readonly ClientInteraction.ClientInteractionRules Rules;

		// Token: 0x02000F26 RID: 3878
		public class ClientInteractionRules
		{
			// Token: 0x06006848 RID: 26696 RVA: 0x0021A77C File Offset: 0x0021897C
			public ClientInteractionRules(InteractionRules rules)
			{
				bool flag = rules.BlockedBy != null;
				if (flag)
				{
					this.BlockedBy = new HashSet<InteractionType>(rules.BlockedBy);
				}
				this.BlockedByBypassIndex = rules.BlockedByBypassIndex;
				bool flag2 = rules.Blocking != null;
				if (flag2)
				{
					this.Blocking = new HashSet<InteractionType>(rules.Blocking);
				}
				this.BlockingBypassIndex = rules.BlockingBypassIndex;
				bool flag3 = rules.InterruptedBy != null;
				if (flag3)
				{
					this.InterruptedBy = new HashSet<InteractionType>(rules.InterruptedBy);
				}
				this.InterruptedByBypassIndex = rules.InterruptedByBypassIndex;
				bool flag4 = rules.Interrupting != null;
				if (flag4)
				{
					this.Interrupting = new HashSet<InteractionType>(rules.Interrupting);
				}
				this.InterruptingBypassIndex = rules.InterruptingBypassIndex;
			}

			// Token: 0x06006849 RID: 26697 RVA: 0x0021A83C File Offset: 0x00218A3C
			public bool ValidateInterrupts(InteractionType type, HashSet<int> selfTags, InteractionType otherType, HashSet<int> otherTags, ClientInteraction.ClientInteractionRules otherRules)
			{
				bool flag = otherRules.InterruptedBy != null && otherRules.InterruptedBy.Contains(type);
				if (flag)
				{
					bool flag2 = otherRules.InterruptedByBypassIndex == int.MinValue || !selfTags.Contains(otherRules.InterruptedByBypassIndex);
					if (flag2)
					{
						return true;
					}
				}
				bool flag3 = this.Interrupting != null && this.Interrupting.Contains(otherType);
				if (flag3)
				{
					bool flag4 = this.InterruptingBypassIndex == int.MinValue || !otherTags.Contains(this.InterruptingBypassIndex);
					if (flag4)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x0600684A RID: 26698 RVA: 0x0021A8E0 File Offset: 0x00218AE0
			public bool ValidateBlocked(InteractionType type, HashSet<int> selfTags, InteractionType otherType, HashSet<int> otherTags, ClientInteraction.ClientInteractionRules otherRules)
			{
				HashSet<InteractionType> hashSet = this.BlockedBy ?? ClientInteraction.ClientInteractionRules.DefaultInteractionBlockedBy[type];
				bool flag = hashSet.Contains(otherType);
				if (flag)
				{
					bool flag2 = this.BlockedByBypassIndex == int.MinValue || !otherTags.Contains(this.BlockedByBypassIndex);
					if (flag2)
					{
						return true;
					}
				}
				bool flag3 = otherRules.Blocking != null && otherRules.Blocking.Contains(type);
				if (flag3)
				{
					bool flag4 = otherRules.BlockingBypassIndex == int.MinValue || !selfTags.Contains(otherRules.BlockingBypassIndex);
					if (flag4)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x0600684B RID: 26699 RVA: 0x0021A98C File Offset: 0x00218B8C
			// Note: this type is marked as 'beforefieldinit'.
			static ClientInteractionRules()
			{
				HashSet<InteractionType> hashSet = new HashSet<InteractionType>();
				hashSet.Add(0);
				hashSet.Add(1);
				hashSet.Add(2);
				hashSet.Add(6);
				hashSet.Add(3);
				hashSet.Add(4);
				hashSet.Add(5);
				hashSet.Add(7);
				hashSet.Add(8);
				hashSet.Add(15);
				hashSet.Add(14);
				ClientInteraction.ClientInteractionRules.StandardInput = hashSet;
				Dictionary<InteractionType, HashSet<InteractionType>> dictionary = new Dictionary<InteractionType, HashSet<InteractionType>>();
				dictionary.Add(0, ClientInteraction.ClientInteractionRules.StandardInput);
				dictionary.Add(1, ClientInteraction.ClientInteractionRules.StandardInput);
				dictionary.Add(2, ClientInteraction.ClientInteractionRules.StandardInput);
				dictionary.Add(3, ClientInteraction.ClientInteractionRules.StandardInput);
				dictionary.Add(4, ClientInteraction.ClientInteractionRules.StandardInput);
				dictionary.Add(5, ClientInteraction.ClientInteractionRules.StandardInput);
				dictionary.Add(7, ClientInteraction.ClientInteractionRules.StandardInput);
				dictionary.Add(8, ClientInteraction.ClientInteractionRules.StandardInput);
				dictionary.Add(6, ClientInteraction.ClientInteractionRules.StandardInput);
				dictionary.Add(9, new HashSet<InteractionType>());
				dictionary.Add(10, new HashSet<InteractionType>());
				dictionary.Add(11, new HashSet<InteractionType>());
				dictionary.Add(12, new HashSet<InteractionType>());
				dictionary.Add(13, new HashSet<InteractionType>());
				dictionary.Add(16, new HashSet<InteractionType>());
				dictionary.Add(17, new HashSet<InteractionType>());
				InteractionType key = 14;
				HashSet<InteractionType> hashSet2 = new HashSet<InteractionType>();
				hashSet2.Add(14);
				hashSet2.Add(15);
				dictionary.Add(key, hashSet2);
				InteractionType key2 = 15;
				HashSet<InteractionType> hashSet3 = new HashSet<InteractionType>();
				hashSet3.Add(14);
				hashSet3.Add(15);
				dictionary.Add(key2, hashSet3);
				dictionary.Add(18, new HashSet<InteractionType>());
				dictionary.Add(21, new HashSet<InteractionType>());
				dictionary.Add(20, new HashSet<InteractionType>());
				dictionary.Add(19, new HashSet<InteractionType>());
				InteractionType key3 = 22;
				HashSet<InteractionType> hashSet4 = new HashSet<InteractionType>();
				hashSet4.Add(22);
				dictionary.Add(key3, hashSet4);
				InteractionType key4 = 23;
				HashSet<InteractionType> hashSet5 = new HashSet<InteractionType>();
				hashSet5.Add(23);
				dictionary.Add(key4, hashSet5);
				InteractionType key5 = 24;
				HashSet<InteractionType> hashSet6 = new HashSet<InteractionType>();
				hashSet6.Add(24);
				dictionary.Add(key5, hashSet6);
				InteractionType key6 = 25;
				HashSet<InteractionType> hashSet7 = new HashSet<InteractionType>();
				hashSet7.Add(25);
				dictionary.Add(key6, hashSet7);
				ClientInteraction.ClientInteractionRules.DefaultInteractionBlockedBy = dictionary;
			}

			// Token: 0x04004A42 RID: 19010
			private const int TAG_NOT_FOUND = -2147483648;

			// Token: 0x04004A43 RID: 19011
			private static readonly HashSet<InteractionType> StandardInput;

			// Token: 0x04004A44 RID: 19012
			private static readonly Dictionary<InteractionType, HashSet<InteractionType>> DefaultInteractionBlockedBy;

			// Token: 0x04004A45 RID: 19013
			public readonly HashSet<InteractionType> BlockedBy;

			// Token: 0x04004A46 RID: 19014
			public readonly int BlockedByBypassIndex;

			// Token: 0x04004A47 RID: 19015
			public readonly HashSet<InteractionType> Blocking;

			// Token: 0x04004A48 RID: 19016
			public readonly int BlockingBypassIndex;

			// Token: 0x04004A49 RID: 19017
			public readonly HashSet<InteractionType> InterruptedBy;

			// Token: 0x04004A4A RID: 19018
			public readonly int InterruptedByBypassIndex;

			// Token: 0x04004A4B RID: 19019
			public readonly HashSet<InteractionType> Interrupting;

			// Token: 0x04004A4C RID: 19020
			public readonly int InterruptingBypassIndex;
		}
	}
}
