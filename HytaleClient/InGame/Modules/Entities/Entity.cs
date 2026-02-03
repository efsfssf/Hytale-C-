using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HytaleClient.Audio;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Data.BlockyModels;
using HytaleClient.Data.Characters;
using HytaleClient.Data.Entities;
using HytaleClient.Data.EntityStats;
using HytaleClient.Data.EntityUI;
using HytaleClient.Data.FX;
using HytaleClient.Data.Items;
using HytaleClient.Data.Map;
using HytaleClient.Graphics;
using HytaleClient.Graphics.BlockyModels;
using HytaleClient.Graphics.Fonts;
using HytaleClient.Graphics.Particles;
using HytaleClient.Graphics.Trails;
using HytaleClient.InGame.Modules.Camera.Controllers;
using HytaleClient.InGame.Modules.Entities.Projectile;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.InGame.Modules.Map;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.InGame.Modules.Entities
{
	// Token: 0x02000945 RID: 2373
	internal class Entity : Disposable
	{
		// Token: 0x170011B9 RID: 4537
		// (get) Token: 0x06004908 RID: 18696 RVA: 0x0011BEE1 File Offset: 0x0011A0E1
		// (set) Token: 0x06004909 RID: 18697 RVA: 0x0011BEE9 File Offset: 0x0011A0E9
		public int CombatTextsCount { get; private set; }

		// Token: 0x0600490A RID: 18698 RVA: 0x0011BEF4 File Offset: 0x0011A0F4
		public void AddCombatText(CombatTextUpdate textUpdate)
		{
			ArrayUtils.GrowArrayIfNecessary<Entity.CombatText>(ref this.CombatTexts, this.CombatTextsCount, 10);
			int combatTextsCount = this.CombatTextsCount;
			this.CombatTextsCount = combatTextsCount + 1;
			int num = combatTextsCount;
			this.CombatTexts[num] = new Entity.CombatText
			{
				HitAngleDeg = (double)textUpdate.HitAngleDeg,
				Text = textUpdate.Text
			};
		}

		// Token: 0x0600490B RID: 18699 RVA: 0x0011BF59 File Offset: 0x0011A159
		public void ClearCombatTexts()
		{
			this.CombatTextsCount = 0;
		}

		// Token: 0x170011BA RID: 4538
		// (get) Token: 0x0600490C RID: 18700 RVA: 0x0011BF63 File Offset: 0x0011A163
		public bool ShouldRender
		{
			get
			{
				return !this.Removed && this.IsVisible && (!this.BeenPredicted || PredictedProjectile.DebugPrediction);
			}
		}

		// Token: 0x170011BB RID: 4539
		// (get) Token: 0x0600490D RID: 18701 RVA: 0x0011BF88 File Offset: 0x0011A188
		// (set) Token: 0x0600490E RID: 18702 RVA: 0x0011BF90 File Offset: 0x0011A190
		public bool BeenPredicted
		{
			get
			{
				return this._beenPredicted;
			}
			set
			{
				this._beenPredicted = true;
				bool debugPrediction = PredictedProjectile.DebugPrediction;
				if (debugPrediction)
				{
					this._topTint = new Vector3(0f, 1f, 0f);
					this._bottomTint = new Vector3(0f, 1f, 0f);
				}
			}
		}

		// Token: 0x170011BC RID: 4540
		// (get) Token: 0x0600490F RID: 18703 RVA: 0x0011BFE4 File Offset: 0x0011A1E4
		// (set) Token: 0x06004910 RID: 18704 RVA: 0x0011BFEC File Offset: 0x0011A1EC
		public Entity.EntityType Type { get; private set; } = Entity.EntityType.None;

		// Token: 0x170011BD RID: 4541
		// (get) Token: 0x06004911 RID: 18705 RVA: 0x0011BFF5 File Offset: 0x0011A1F5
		// (set) Token: 0x06004912 RID: 18706 RVA: 0x0011BFFD File Offset: 0x0011A1FD
		public BoundingBox Hitbox { get; private set; }

		// Token: 0x170011BE RID: 4542
		// (get) Token: 0x06004913 RID: 18707 RVA: 0x0011C006 File Offset: 0x0011A206
		// (set) Token: 0x06004914 RID: 18708 RVA: 0x0011C00E File Offset: 0x0011A20E
		public BoundingBox DefaultHitbox { get; private set; }

		// Token: 0x170011BF RID: 4543
		// (get) Token: 0x06004915 RID: 18709 RVA: 0x0011C017 File Offset: 0x0011A217
		// (set) Token: 0x06004916 RID: 18710 RVA: 0x0011C01F File Offset: 0x0011A21F
		public BoundingBox CrouchHitbox { get; private set; }

		// Token: 0x170011C0 RID: 4544
		// (get) Token: 0x06004917 RID: 18711 RVA: 0x0011C028 File Offset: 0x0011A228
		// (set) Token: 0x06004918 RID: 18712 RVA: 0x0011C030 File Offset: 0x0011A230
		public Dictionary<string, Entity.DetailBoundingBox[]> DetailBoundingBoxes { get; private set; } = new Dictionary<string, Entity.DetailBoundingBox[]>();

		// Token: 0x170011C1 RID: 4545
		// (get) Token: 0x06004919 RID: 18713 RVA: 0x0011C039 File Offset: 0x0011A239
		public Vector3 Position
		{
			get
			{
				return this._position;
			}
		}

		// Token: 0x170011C2 RID: 4546
		// (get) Token: 0x0600491A RID: 18714 RVA: 0x0011C041 File Offset: 0x0011A241
		public Vector3 NextPosition
		{
			get
			{
				return this._nextPosition;
			}
		}

		// Token: 0x170011C3 RID: 4547
		// (get) Token: 0x0600491B RID: 18715 RVA: 0x0011C049 File Offset: 0x0011A249
		public Vector3 PreviousPosition
		{
			get
			{
				return this._previousPosition;
			}
		}

		// Token: 0x0600491C RID: 18716 RVA: 0x0011C054 File Offset: 0x0011A254
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetPosition(Vector3 nextPosition)
		{
			this._previousPosition = this.Position;
			this._nextPosition = nextPosition;
			bool flag = this.NetworkId == this._gameInstance.LocalPlayerNetworkId || this.NetworkId == this._gameInstance.EntityStoreModule.MountEntityLocalId;
			this.PositionProgress = (flag ? 1f : 0f);
			this._position = (flag ? this._nextPosition : this._previousPosition);
		}

		// Token: 0x0600491D RID: 18717 RVA: 0x0011C0D0 File Offset: 0x0011A2D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetPositionTeleport(Vector3 nextPosition)
		{
			this._previousPosition = nextPosition;
			this._nextPosition = nextPosition;
			this.PositionProgress = 1f;
			this._position = nextPosition;
		}

		// Token: 0x170011C4 RID: 4548
		// (get) Token: 0x0600491E RID: 18718 RVA: 0x0011C0F3 File Offset: 0x0011A2F3
		public Vector3 BodyOrientation
		{
			get
			{
				return this._bodyOrientation;
			}
		}

		// Token: 0x0600491F RID: 18719 RVA: 0x0011C0FC File Offset: 0x0011A2FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetBodyOrientation(Vector3 nextOrientation)
		{
			this._previousBodyOrientation = this.BodyOrientation;
			this._nextBodyOrientation = nextOrientation;
			bool flag = this.NetworkId == this._gameInstance.LocalPlayerNetworkId || this.NetworkId == this._gameInstance.EntityStoreModule.MountEntityLocalId;
			this.BodyOrientationProgress = (flag ? 1f : 0f);
			this._bodyOrientation = (flag ? this._nextBodyOrientation : this._previousBodyOrientation);
		}

		// Token: 0x06004920 RID: 18720 RVA: 0x0011C178 File Offset: 0x0011A378
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetBodyOrientationTeleport(Vector3 nextOrientation)
		{
			this._previousBodyOrientation = nextOrientation;
			this._nextBodyOrientation = nextOrientation;
			this.BodyOrientationProgress = 1f;
			this._bodyOrientation = nextOrientation;
		}

		// Token: 0x170011C5 RID: 4549
		// (get) Token: 0x06004921 RID: 18721 RVA: 0x0011C19B File Offset: 0x0011A39B
		// (set) Token: 0x06004922 RID: 18722 RVA: 0x0011C1A3 File Offset: 0x0011A3A3
		public Vector4 BlockLightColor { get; private set; }

		// Token: 0x170011C6 RID: 4550
		// (get) Token: 0x06004923 RID: 18723 RVA: 0x0011C1AC File Offset: 0x0011A3AC
		// (set) Token: 0x06004924 RID: 18724 RVA: 0x0011C1B4 File Offset: 0x0011A3B4
		public ModelRenderer ModelRenderer { get; private set; }

		// Token: 0x170011C7 RID: 4551
		// (get) Token: 0x06004925 RID: 18725 RVA: 0x0011C1BD File Offset: 0x0011A3BD
		public Vector3 BottomTint
		{
			get
			{
				return this._bottomTint;
			}
		}

		// Token: 0x170011C8 RID: 4552
		// (get) Token: 0x06004926 RID: 18726 RVA: 0x0011C1C5 File Offset: 0x0011A3C5
		public Vector3 TopTint
		{
			get
			{
				return this._topTint;
			}
		}

		// Token: 0x170011C9 RID: 4553
		// (get) Token: 0x06004927 RID: 18727 RVA: 0x0011C1CD File Offset: 0x0011A3CD
		// (set) Token: 0x06004928 RID: 18728 RVA: 0x0011C1D5 File Offset: 0x0011A3D5
		public Model ModelPacket { get; private set; }

		// Token: 0x170011CA RID: 4554
		// (get) Token: 0x06004929 RID: 18729 RVA: 0x0011C1DE File Offset: 0x0011A3DE
		// (set) Token: 0x0600492A RID: 18730 RVA: 0x0011C1E6 File Offset: 0x0011A3E6
		public string[] ArmorIds { get; private set; } = new string[0];

		// Token: 0x170011CB RID: 4555
		// (get) Token: 0x0600492B RID: 18731 RVA: 0x0011C1EF File Offset: 0x0011A3EF
		// (set) Token: 0x0600492C RID: 18732 RVA: 0x0011C1F7 File Offset: 0x0011A3F7
		public ClientItemBase PrimaryItem { get; private set; }

		// Token: 0x170011CC RID: 4556
		// (get) Token: 0x0600492D RID: 18733 RVA: 0x0011C200 File Offset: 0x0011A400
		// (set) Token: 0x0600492E RID: 18734 RVA: 0x0011C208 File Offset: 0x0011A408
		public ClientItemBase SecondaryItem { get; private set; }

		// Token: 0x170011CD RID: 4557
		// (get) Token: 0x0600492F RID: 18735 RVA: 0x0011C211 File Offset: 0x0011A411
		// (set) Token: 0x06004930 RID: 18736 RVA: 0x0011C219 File Offset: 0x0011A419
		public float EyeOffset { get; private set; }

		// Token: 0x170011CE RID: 4558
		// (get) Token: 0x06004931 RID: 18737 RVA: 0x0011C222 File Offset: 0x0011A422
		// (set) Token: 0x06004932 RID: 18738 RVA: 0x0011C22A File Offset: 0x0011A42A
		public float CrouchOffset { get; private set; }

		// Token: 0x170011CF RID: 4559
		// (get) Token: 0x06004933 RID: 18739 RVA: 0x0011C233 File Offset: 0x0011A433
		public CameraSettings CameraSettings
		{
			get
			{
				CameraSettings result;
				if ((result = this.ActionCameraSettings) == null)
				{
					result = (this._itemCameraSettings ?? this._modelCameraSettings);
				}
				return result;
			}
		}

		// Token: 0x170011D0 RID: 4560
		// (get) Token: 0x06004934 RID: 18740 RVA: 0x0011C24F File Offset: 0x0011A44F
		// (set) Token: 0x06004935 RID: 18741 RVA: 0x0011C257 File Offset: 0x0011A457
		public string Name { get; private set; }

		// Token: 0x170011D1 RID: 4561
		// (get) Token: 0x06004936 RID: 18742 RVA: 0x0011C260 File Offset: 0x0011A460
		// (set) Token: 0x06004937 RID: 18743 RVA: 0x0011C268 File Offset: 0x0011A468
		public EntityAnimation CurrentMovementAnimation { get; private set; }

		// Token: 0x170011D2 RID: 4562
		// (get) Token: 0x06004938 RID: 18744 RVA: 0x0011C271 File Offset: 0x0011A471
		// (set) Token: 0x06004939 RID: 18745 RVA: 0x0011C279 File Offset: 0x0011A479
		public EntityAnimation CurrentActionAnimation { get; private set; } = null;

		// Token: 0x170011D3 RID: 4563
		// (get) Token: 0x0600493A RID: 18746 RVA: 0x0011C282 File Offset: 0x0011A482
		// (set) Token: 0x0600493B RID: 18747 RVA: 0x0011C28A File Offset: 0x0011A48A
		public TextRenderer NameplateTextRenderer { get; private set; }

		// Token: 0x170011D4 RID: 4564
		// (get) Token: 0x0600493C RID: 18748 RVA: 0x0011C293 File Offset: 0x0011A493
		// (set) Token: 0x0600493D RID: 18749 RVA: 0x0011C29B File Offset: 0x0011A49B
		public ClientItemBase ItemBase { get; private set; }

		// Token: 0x170011D5 RID: 4565
		// (get) Token: 0x0600493E RID: 18750 RVA: 0x0011C2A4 File Offset: 0x0011A4A4
		// (set) Token: 0x0600493F RID: 18751 RVA: 0x0011C2AC File Offset: 0x0011A4AC
		public float ItemAnimationTime { get; private set; }

		// Token: 0x170011D6 RID: 4566
		// (get) Token: 0x06004940 RID: 18752 RVA: 0x0011C2B5 File Offset: 0x0011A4B5
		// (set) Token: 0x06004941 RID: 18753 RVA: 0x0011C2BD File Offset: 0x0011A4BD
		public PlayerSkin PlayerSkin { get; set; }

		// Token: 0x06004942 RID: 18754 RVA: 0x0011C2C8 File Offset: 0x0011A4C8
		public Entity(GameInstance gameInstance, int networkId)
		{
			this._gameInstance = gameInstance;
			this.NetworkId = networkId;
			this.IsLocalEntity = (this.NetworkId < 0);
			ServerSettings serverSettings = this._gameInstance.ServerSettings;
			int num = (serverSettings != null) ? serverSettings.EntityStatTypes.Length : 0;
			this._entityStats = new ClientEntityStatValue[num];
			this._serverEntityStats = new ClientEntityStatValue[num];
			for (int i = 0; i < this._animationSoundEventReferences.Length; i++)
			{
				this._animationSoundEventReferences[i] = AudioDevice.SoundEventReference.None;
			}
			this.ModelVFX = new ClientModelVFX();
			this.ModelVFX.NoiseScale = new Vector2(50f);
		}

		// Token: 0x06004943 RID: 18755 RVA: 0x0011C584 File Offset: 0x0011A784
		protected override void DoDispose()
		{
			this.ClearCombatSequenceEffects();
			this.ClearFX();
			for (int i = 0; i < this.EntityItems.Count; i++)
			{
				Entity.EntityItem entityItem = this.EntityItems[i];
				bool flag = entityItem.SoundEventReference.PlaybackId != -1;
				if (flag)
				{
					this._gameInstance.AudioModule.ActionOnEvent(ref entityItem.SoundEventReference, 0);
				}
				entityItem.ClearFX();
				entityItem.ModelRenderer.Dispose();
				entityItem.ModelRenderer = null;
			}
			ParticleSystemProxy runParticleSystem = this._runParticleSystem;
			if (runParticleSystem != null)
			{
				runParticleSystem.Expire(false);
			}
			ParticleSystemProxy sprintParticleSystem = this._sprintParticleSystem;
			if (sprintParticleSystem != null)
			{
				sprintParticleSystem.Expire(false);
			}
			this.EntityItems.Clear();
			ModelRenderer modelRenderer = this.ModelRenderer;
			if (modelRenderer != null)
			{
				modelRenderer.Dispose();
			}
			ParticleSystemProxy itemParticleSystem = this._itemParticleSystem;
			if (itemParticleSystem != null)
			{
				itemParticleSystem.Expire(true);
			}
			TextRenderer nameplateTextRenderer = this.NameplateTextRenderer;
			if (nameplateTextRenderer != null)
			{
				nameplateTextRenderer.Dispose();
			}
			for (int j = 0; j < this._animationSoundEventReferences.Length; j++)
			{
				ref AudioDevice.SoundEventReference ptr = ref this._animationSoundEventReferences[j];
				bool flag2 = ptr.PlaybackId != -1;
				if (flag2)
				{
					this._gameInstance.AudioModule.ActionOnEvent(ref ptr, 0);
				}
			}
			for (int k = 0; k < this.EntityEffects.Length; k++)
			{
				ref Entity.UniqueEntityEffect ptr2 = ref this.EntityEffects[k];
				bool flag3 = ptr2.SoundEventReference.PlaybackId != -1;
				if (flag3)
				{
					this._gameInstance.AudioModule.ActionOnEvent(ref ptr2.SoundEventReference, 0);
				}
				bool flag4 = ptr2.ParticleSystems != null;
				if (flag4)
				{
					for (int l = 0; l < ptr2.ParticleSystems.Count; l++)
					{
						ptr2.ParticleSystems[l].ParticleSystemProxy.Expire(false);
					}
				}
			}
			foreach (Dictionary<int, InteractionMetaStore> dictionary in this.InteractionMetaStores.Values)
			{
				foreach (InteractionMetaStore interactionMetaStore in dictionary.Values)
				{
					bool flag5 = interactionMetaStore.SoundEventReference.PlaybackId != -1;
					if (flag5)
					{
						this._gameInstance.AudioModule.ActionOnEvent(ref interactionMetaStore.SoundEventReference, 0);
					}
				}
			}
			bool flag6 = this.SoundObjectReference.SoundObjectId != 1U;
			if (flag6)
			{
				this._gameInstance.AudioModule.UnregisterSoundObject(ref this.SoundObjectReference);
			}
		}

		// Token: 0x06004944 RID: 18756 RVA: 0x0011C878 File Offset: 0x0011AA78
		public virtual void SetTransform(Vector3 position, Vector3 bodyOrientation, Vector3 lookOrientation)
		{
			this.SetPosition(position);
			this.SetBodyOrientation(bodyOrientation);
			this.LookOrientation = lookOrientation;
		}

		// Token: 0x06004945 RID: 18757 RVA: 0x0011C892 File Offset: 0x0011AA92
		public void SkipTransformLerp()
		{
			this._previousPosition = this._nextPosition;
			this._previousBodyOrientation = this._nextBodyOrientation;
		}

		// Token: 0x06004946 RID: 18758 RVA: 0x0011C8B0 File Offset: 0x0011AAB0
		public void SetSpawnTransform(Vector3 position, Vector3 bodyOrientation, Vector3 lookOrientation)
		{
			this.SetTransform(position, bodyOrientation, lookOrientation);
			this._position = (this._previousPosition = this._nextPosition);
			this._bodyOrientation = (this._previousBodyOrientation = this._nextBodyOrientation);
		}

		// Token: 0x06004947 RID: 18759 RVA: 0x0011C8F4 File Offset: 0x0011AAF4
		public void SetName(string name, bool nameTagVisible)
		{
			this.Name = name;
			bool flag = string.IsNullOrWhiteSpace(name) || !nameTagVisible;
			if (flag)
			{
				TextRenderer nameplateTextRenderer = this.NameplateTextRenderer;
				if (nameplateTextRenderer != null)
				{
					nameplateTextRenderer.Dispose();
				}
				this.NameplateTextRenderer = null;
			}
			else
			{
				bool flag2 = this.NameplateTextRenderer == null;
				if (flag2)
				{
					this.NameplateTextRenderer = new TextRenderer(this._gameInstance.Engine.Graphics, this._gameInstance.App.Fonts.DefaultFontFamily.RegularFont, name, uint.MaxValue, 4278190080U);
				}
				else
				{
					this.NameplateTextRenderer.Text = name;
				}
			}
		}

		// Token: 0x06004948 RID: 18760 RVA: 0x0011C994 File Offset: 0x0011AB94
		private void CalculateEffectTint()
		{
			this._bottomTint = Vector3.Zero;
			this._topTint = Vector3.Zero;
			for (int i = 0; i < this.EntityEffects.Length; i++)
			{
				ref Entity.UniqueEntityEffect ptr = ref this.EntityEffects[i];
				bool isExpiring = ptr.IsExpiring;
				if (!isExpiring)
				{
					this._bottomTint.X = 1f - (1f - this._bottomTint.X) * (1f - ptr.BottomTint.X);
					this._bottomTint.Y = 1f - (1f - this._bottomTint.Y) * (1f - ptr.BottomTint.Y);
					this._bottomTint.Z = 1f - (1f - this._bottomTint.Z) * (1f - ptr.BottomTint.Z);
					this._topTint.X = 1f - (1f - this._topTint.X) * (1f - ptr.TopTint.X);
					this._topTint.Y = 1f - (1f - this._topTint.Y) * (1f - ptr.TopTint.Y);
					this._topTint.Z = 1f - (1f - this._topTint.Z) * (1f - ptr.TopTint.Z);
				}
			}
		}

		// Token: 0x06004949 RID: 18761 RVA: 0x0011CB2C File Offset: 0x0011AD2C
		private void ServerAddEffect(EntityEffectUpdate update)
		{
			bool flag = Entity.FindAndRemoveMatchEffectPrediction(this._predictedEffects, update.Id, 0, new bool?(update.Infinite));
			if (!flag)
			{
				this._serverEffects.Add(new Entity.ServerEffectEntry
				{
					CreationTime = DateTime.Now.Ticks / 10000L,
					Update = update
				});
				EntityEffect protocolEntityEffect = this._gameInstance.EntityStoreModule.EntityEffects[update.Id];
				this.AddEffect(update.Id, protocolEntityEffect, new float?(update.RemainingTime), new bool?(update.Infinite), new bool?(update.Debuff));
			}
		}

		// Token: 0x0600494A RID: 18762 RVA: 0x0011CBE0 File Offset: 0x0011ADE0
		private void ServerRemoveEffect(EntityEffectUpdate update)
		{
			bool flag = Entity.FindAndRemoveMatchEffectPrediction(this._predictedEffects, update.Id, 1, null);
			if (!flag)
			{
				this._serverEffects.Add(new Entity.ServerEffectEntry
				{
					CreationTime = DateTime.Now.Ticks / 10000L,
					Update = update
				});
				this.RemoveEffect(update.Id);
			}
		}

		// Token: 0x0600494B RID: 18763 RVA: 0x0011CC58 File Offset: 0x0011AE58
		public EntityEffectUpdate PredictedAddEffect(int networkEffectIndex, bool? infinite = null)
		{
			bool flag = Entity.FindAndRemoveMatchEffectPrediction(this._serverEffects, networkEffectIndex, 0, infinite);
			EntityEffectUpdate result;
			if (flag)
			{
				result = null;
			}
			else
			{
				EntityEffect entityEffect = this._gameInstance.EntityStoreModule.EntityEffects[networkEffectIndex];
				EntityEffectUpdate entityEffectUpdate = new EntityEffectUpdate(0, networkEffectIndex, entityEffect.Duration, infinite ?? entityEffect.Infinite, entityEffect.Debuff, entityEffect.StatusEffectIcon);
				this._predictedEffects.Add(entityEffectUpdate);
				this.AddEffect(networkEffectIndex, entityEffect, null, infinite, null);
				result = entityEffectUpdate;
			}
			return result;
		}

		// Token: 0x0600494C RID: 18764 RVA: 0x0011CCFC File Offset: 0x0011AEFC
		public EntityEffectUpdate PredictedRemoveEffect(int networkEffectIndex)
		{
			bool flag = Entity.FindAndRemoveMatchEffectPrediction(this._serverEffects, networkEffectIndex, 1, null);
			EntityEffectUpdate result;
			if (flag)
			{
				result = null;
			}
			else
			{
				EntityEffectUpdate entityEffectUpdate = new EntityEffectUpdate(1, networkEffectIndex, 0f, false, false, "");
				this._predictedEffects.Add(entityEffectUpdate);
				this.RemoveEffect(networkEffectIndex);
				result = entityEffectUpdate;
			}
			return result;
		}

		// Token: 0x0600494D RID: 18765 RVA: 0x0011CD58 File Offset: 0x0011AF58
		public void CancelPrediction(EntityEffectUpdate prediction)
		{
			bool flag = prediction == null;
			if (!flag)
			{
				bool flag2 = !this._predictedEffects.Remove(prediction);
				if (!flag2)
				{
					bool flag3 = prediction.Type == 0;
					if (flag3)
					{
						this.RemoveEffect(prediction.Id);
					}
					else
					{
						this.AddEffect(prediction.Id, null, null, null);
					}
				}
			}
		}

		// Token: 0x0600494E RID: 18766 RVA: 0x0011CDD0 File Offset: 0x0011AFD0
		private static bool FindAndRemoveMatchEffectPrediction(IList<EntityEffectUpdate> data, int networkEffectIndex, EntityEffectUpdate.EffectOp op, bool? infinite)
		{
			for (int i = 0; i < data.Count; i++)
			{
				EntityEffectUpdate entityEffectUpdate = data[i];
				bool flag = entityEffectUpdate.Type != op;
				if (!flag)
				{
					bool flag2 = entityEffectUpdate.Id != networkEffectIndex;
					if (!flag2)
					{
						bool flag3 = infinite != null && entityEffectUpdate.Infinite != infinite.Value;
						if (!flag3)
						{
							data.RemoveAt(i);
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600494F RID: 18767 RVA: 0x0011CE5C File Offset: 0x0011B05C
		private static bool FindAndRemoveMatchEffectPrediction(IList<Entity.ServerEffectEntry> data, int networkEffectIndex, EntityEffectUpdate.EffectOp op, bool? infinite)
		{
			for (int i = 0; i < data.Count; i++)
			{
				EntityEffectUpdate update = data[i].Update;
				bool flag = update.Type != op;
				if (!flag)
				{
					bool flag2 = update.Id != networkEffectIndex;
					if (!flag2)
					{
						bool flag3 = infinite != null && update.Infinite != infinite.Value;
						if (!flag3)
						{
							data.RemoveAt(i);
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06004950 RID: 18768 RVA: 0x0011CEF0 File Offset: 0x0011B0F0
		public void AddEffect(int networkEffectIndex, float? remainingDuration = null, bool? infinite = null, bool? debuff = null)
		{
			bool flag = networkEffectIndex >= this._gameInstance.EntityStoreModule.EntityEffects.Length;
			if (flag)
			{
				this._gameInstance.Chat.Error(string.Format("Entity Effect not found for index: {0}", networkEffectIndex));
			}
			else
			{
				EntityEffect entityEffect = this._gameInstance.EntityStoreModule.EntityEffects[networkEffectIndex];
				bool flag2 = entityEffect == null;
				if (flag2)
				{
					this._gameInstance.Chat.Error(string.Format("Entity Effect null for index: {0}", networkEffectIndex));
				}
				else
				{
					this.AddEffect(networkEffectIndex, entityEffect, remainingDuration, infinite, debuff);
				}
			}
		}

		// Token: 0x06004951 RID: 18769 RVA: 0x0011CF8C File Offset: 0x0011B18C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int GetEffectIndexFromNetworkEffectIndex(int networkEffectIndex)
		{
			for (int i = 0; i < this.EntityEffects.Length; i++)
			{
				bool flag = this.EntityEffects[i].NetworkEffectIndex == networkEffectIndex;
				if (flag)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06004952 RID: 18770 RVA: 0x0011CFD4 File Offset: 0x0011B1D4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool HasEffect(int networkEffectIndex)
		{
			int effectIndexFromNetworkEffectIndex = this.GetEffectIndexFromNetworkEffectIndex(networkEffectIndex);
			return effectIndexFromNetworkEffectIndex != -1 && !this.EntityEffects[effectIndexFromNetworkEffectIndex].IsExpiring;
		}

		// Token: 0x06004953 RID: 18771 RVA: 0x0011D00C File Offset: 0x0011B20C
		[Obsolete("Deprecated method. Use Entity#AddEffect(int entityEffectIndex) instead. This method is a temporary workaround to have BlockPlacementPreview#UpdatePreview() working.")]
		public void AddEffect(int networkEffectIndex, EntityEffect protocolEntityEffect, float? remainingDuration = null, bool? infinite = null, bool? debuff = null)
		{
			int num = this.GetEffectIndexFromNetworkEffectIndex(networkEffectIndex);
			bool flag = num != -1;
			bool flag2 = num == -1;
			if (flag2)
			{
				num = this.EntityEffects.Length;
				ArrayUtils.GrowArrayIfNecessary<Entity.UniqueEntityEffect>(ref this.EntityEffects, this.EntityEffects.Length, 1);
				this.EntityEffects[num] = new Entity.UniqueEntityEffect(protocolEntityEffect, networkEffectIndex);
				this._effectsOnEntityDirty = true;
			}
			bool flag3 = this.NetworkId < 0;
			if (flag3)
			{
			}
			ref Entity.UniqueEntityEffect ptr = ref this.EntityEffects[num];
			ptr.IsInfinite = (infinite ?? protocolEntityEffect.Infinite);
			ptr.IsDebuff = (debuff ?? protocolEntityEffect.Debuff);
			bool flag4 = flag;
			if (flag4)
			{
				bool flag5 = !ptr.IsExpiring;
				if (flag5)
				{
					switch (protocolEntityEffect.OverlapBehavior_)
					{
					case 0:
					{
						bool flag6 = !ptr.IsInfinite && remainingDuration == null;
						if (flag6)
						{
							remainingDuration = new float?(ptr.RemainingDuration + protocolEntityEffect.Duration);
						}
						break;
					}
					case 1:
						break;
					case 2:
						return;
					default:
						throw new ArgumentOutOfRangeException();
					}
				}
				else
				{
					bool flag7;
					if (this.NetworkId == this._gameInstance.LocalPlayerNetworkId)
					{
						EntityEffect.ApplicationEffects applicationEffects_ = protocolEntityEffect.ApplicationEffects_;
						flag7 = (((applicationEffects_ != null) ? applicationEffects_.ScreenEffect : null) != null);
					}
					else
					{
						flag7 = false;
					}
					bool flag8 = flag7;
					if (flag8)
					{
						this._gameInstance.ScreenEffectStoreModule.AddEntityScreenEffect(protocolEntityEffect.ApplicationEffects_.ScreenEffect);
					}
				}
				bool flag9 = ptr.ModelRenderer != null && ptr.SpawnAnimation != null;
				if (flag9)
				{
					ptr.ModelRenderer.SetSlotAnimation(0, ptr.SpawnAnimation.Data, ptr.SpawnAnimation.Looping, 1f, 0f, ptr.SpawnAnimation.BlendingDuration, null, false);
				}
				ptr.IsExpiring = false;
			}
			else
			{
				bool flag10;
				if (this.NetworkId == this._gameInstance.LocalPlayerNetworkId)
				{
					EntityEffect.ApplicationEffects applicationEffects_2 = protocolEntityEffect.ApplicationEffects_;
					flag10 = (((applicationEffects_2 != null) ? applicationEffects_2.ScreenEffect : null) != null);
				}
				else
				{
					flag10 = false;
				}
				bool flag11 = flag10;
				if (flag11)
				{
					this._gameInstance.ScreenEffectStoreModule.AddEntityScreenEffect(protocolEntityEffect.ApplicationEffects_.ScreenEffect);
				}
			}
			ptr.RemainingDuration = (remainingDuration ?? protocolEntityEffect.Duration);
			EntityEffect.ApplicationEffects applicationEffects_3 = protocolEntityEffect.ApplicationEffects_;
			bool flag12 = ((applicationEffects_3 != null) ? applicationEffects_3.EntityAnimationId : null) != null;
			if (flag12)
			{
				EntityAnimation animation = this.GetAnimation(protocolEntityEffect.ApplicationEffects_.EntityAnimationId);
				bool flag13 = animation != null;
				if (flag13)
				{
					this.ModelRenderer.SetSlotAnimation(6, animation.Data, animation.Looping, animation.Speed, 0f, animation.BlendingDuration, null, false);
					this.ServerAnimations[1] = protocolEntityEffect.ApplicationEffects_.EntityAnimationId;
				}
			}
			bool flag14 = protocolEntityEffect.ApplicationEffects_ != null;
			if (flag14)
			{
				bool flag15 = this != this._gameInstance.LocalPlayer;
				if (flag15)
				{
					uint networkWwiseId = ResourceManager.GetNetworkWwiseId(protocolEntityEffect.ApplicationEffects_.SoundEventIndexWorld);
					this._gameInstance.AudioModule.PlaySoundEvent(networkWwiseId, this.SoundObjectReference, ref ptr.SoundEventReference);
				}
				else
				{
					uint networkWwiseId2 = ResourceManager.GetNetworkWwiseId(protocolEntityEffect.ApplicationEffects_.SoundEventIndexLocal);
					this._gameInstance.AudioModule.PlayLocalSoundEvent(networkWwiseId2);
				}
				string modelVFXId = protocolEntityEffect.ApplicationEffects_.ModelVFXId;
				bool flag16 = modelVFXId != null;
				if (flag16)
				{
					int num2;
					bool flag17 = !this._gameInstance.EntityStoreModule.ModelVFXByIds.TryGetValue(modelVFXId, out num2);
					if (flag17)
					{
						this._gameInstance.App.DevTools.Error("Could not find model vfx: " + modelVFXId);
						return;
					}
					ModelVFX modelVFX = this._gameInstance.EntityStoreModule.ModelVFXs[num2];
					bool flag18 = modelVFX != null;
					if (flag18)
					{
						ModelVFX modelVFX2 = modelVFX;
						bool flag19 = modelVFX2.HighlightColor != null;
						bool flag20 = modelVFX2.AnimationDuration > 0f;
						bool flag21 = flag19;
						if (flag21)
						{
							this.ModelVFX.HighlightColor = new Vector3((float)((byte)modelVFX2.HighlightColor.Red) / 255f, (float)((byte)modelVFX2.HighlightColor.Green) / 255f, (float)((byte)modelVFX2.HighlightColor.Blue) / 255f);
						}
						bool flag22 = flag20;
						if (flag22)
						{
							this.ModelVFX.AnimationDuration = modelVFX2.AnimationDuration;
						}
						bool flag23 = modelVFX2.AnimationRange != null;
						if (flag23)
						{
							this.ModelVFX.AnimationRange = new Vector2(modelVFX2.AnimationRange.X, modelVFX2.AnimationRange.Y);
						}
						this.ModelVFX.LoopOption = modelVFX2.LoopOption_;
						switch (modelVFX2.CurveType_)
						{
						case 0:
							this.ModelVFX.CurveType = Easing.EasingType.Linear;
							break;
						case 1:
							this.ModelVFX.CurveType = Easing.EasingType.QuartIn;
							break;
						case 2:
							this.ModelVFX.CurveType = Easing.EasingType.QuartOut;
							break;
						case 3:
							this.ModelVFX.CurveType = Easing.EasingType.QuartInOut;
							break;
						}
						this.ModelVFX.HighlightThickness = modelVFX2.HighlightThickness;
						bool flag24 = modelVFX2.NoiseScale != null;
						if (flag24)
						{
							this.ModelVFX.NoiseScale = new Vector2(modelVFX2.NoiseScale.X, modelVFX2.NoiseScale.Y);
						}
						bool flag25 = modelVFX2.NoiseScrollSpeed != null;
						if (flag25)
						{
							this.ModelVFX.NoiseScrollSpeed = new Vector2(modelVFX2.NoiseScrollSpeed.X, modelVFX2.NoiseScrollSpeed.Y);
						}
						bool flag26 = modelVFX2.PostColor != null;
						bool flag27 = flag26;
						if (flag27)
						{
							this.ModelVFX.PostColor = new Vector4((float)((byte)modelVFX2.PostColor.Red) / 255f, (float)((byte)modelVFX2.PostColor.Green) / 255f, (float)((byte)modelVFX2.PostColor.Blue) / 255f, modelVFX2.PostColorOpacity);
						}
						ClientModelVFX.EffectDirections effectDirection_ = modelVFX2.EffectDirection_;
						SwitchTo switchTo_ = modelVFX2.SwitchTo_;
						int useBloom = modelVFX2.UseBloomOnHighlight ? 1 : 0;
						int useProgressiveHighlight = modelVFX2.UseProgessiveHighlight ? 1 : 0;
						this.ModelVFX.PackedModelVFXParams = Entity.UniqueEntityEffect.PackModelVFXData((int)effectDirection_, switchTo_, useBloom, useProgressiveHighlight);
						bool flag28 = this.NetworkId < 0;
						if (flag28)
						{
						}
						bool flag29 = flag19 && flag20;
						if (flag29)
						{
							this.ModelVFX.TriggerAnimation = true;
						}
					}
				}
			}
			bool flag30 = protocolEntityEffect.ModelOverride_ != null;
			if (flag30)
			{
				EntityEffect.ModelOverride modelOverride_ = protocolEntityEffect.ModelOverride_;
				string hash;
				BlockyModel blockyModel;
				bool flag31 = !this._gameInstance.HashesByServerAssetPath.TryGetValue(modelOverride_.Model, ref hash) || !this._gameInstance.EntityStoreModule.GetModel(hash, out blockyModel);
				if (flag31)
				{
					this._gameInstance.App.DevTools.Error("Failed to load entity effect model: " + modelOverride_.Model);
					return;
				}
				string key;
				bool flag32 = !this._gameInstance.HashesByServerAssetPath.TryGetValue(modelOverride_.Texture, ref key);
				if (flag32)
				{
					this._gameInstance.App.DevTools.Error("Failed to load entity effect texture: " + modelOverride_.Texture);
					return;
				}
				Point offset;
				bool flag33 = !this._gameInstance.EntityStoreModule.ImageLocations.TryGetValue(key, out offset);
				if (flag33)
				{
					this._gameInstance.App.DevTools.Error("Cannot use " + modelOverride_.Texture + " as an entity effect texture");
					return;
				}
				BlockyModel blockyModel2 = blockyModel.Clone();
				blockyModel2.SetAtlasIndex(1);
				blockyModel2.OffsetUVs(offset);
				ptr.ModelRenderer = new ModelRenderer(blockyModel2, this._gameInstance.AtlasSizes, this._gameInstance.Engine.Graphics, this._gameInstance.FrameCounter, false);
				bool flag34 = modelOverride_.AnimationSets != null;
				if (flag34)
				{
					bool keepPreviousFirstPersonAnimation = false;
					AnimationSet animationSet;
					bool flag35;
					if (modelOverride_.AnimationSets.TryGetValue("Spawn", out animationSet))
					{
						Animation[] animations = animationSet.Animations;
						flag35 = (animations != null && animations.Length != 0);
					}
					else
					{
						flag35 = false;
					}
					bool flag36 = flag35;
					if (flag36)
					{
						Animation weightedAnimation = this.GetWeightedAnimation(animationSet.Animations);
						string hash2;
						BlockyAnimation data;
						bool flag37 = this._gameInstance.HashesByServerAssetPath.TryGetValue(weightedAnimation.Animation_, ref hash2) && this._gameInstance.EntityStoreModule.GetAnimation(hash2, out data);
						if (flag37)
						{
							float speed = (weightedAnimation.Speed == 0f) ? 1f : weightedAnimation.Speed;
							ptr.SpawnAnimation = new EntityAnimation(data, speed, weightedAnimation.BlendingDuration * 60f, weightedAnimation.Looping, keepPreviousFirstPersonAnimation, ResourceManager.GetNetworkWwiseId(weightedAnimation.SoundEventIndex), weightedAnimation.Weight, weightedAnimation.FootstepIntervals, weightedAnimation.PassiveLoopCount, null, null, null, null, null, false);
							ptr.ModelRenderer.SetSlotAnimationNoBlending(0, ptr.SpawnAnimation.Data, ptr.SpawnAnimation.Looping, 1f, 0f);
						}
					}
					bool flag38;
					if (modelOverride_.AnimationSets.TryGetValue("Despawn", out animationSet))
					{
						Animation[] animations2 = animationSet.Animations;
						flag38 = (animations2 != null && animations2.Length != 0);
					}
					else
					{
						flag38 = false;
					}
					bool flag39 = flag38;
					if (flag39)
					{
						Animation weightedAnimation2 = this.GetWeightedAnimation(animationSet.Animations);
						string hash3;
						BlockyAnimation data2;
						bool flag40 = this._gameInstance.HashesByServerAssetPath.TryGetValue(weightedAnimation2.Animation_, ref hash3) && this._gameInstance.EntityStoreModule.GetAnimation(hash3, out data2);
						if (flag40)
						{
							float speed2 = (weightedAnimation2.Speed == 0f) ? 1f : weightedAnimation2.Speed;
							ptr.DespawnAnimation = new EntityAnimation(data2, speed2, weightedAnimation2.BlendingDuration * 60f, weightedAnimation2.Looping, keepPreviousFirstPersonAnimation, ResourceManager.GetNetworkWwiseId(weightedAnimation2.SoundEventIndex), weightedAnimation2.Weight, weightedAnimation2.FootstepIntervals, weightedAnimation2.PassiveLoopCount, null, null, null, null, null, false);
						}
					}
					bool flag41;
					if (modelOverride_.AnimationSets.TryGetValue("Idle", out animationSet))
					{
						Animation[] animations3 = animationSet.Animations;
						flag41 = (animations3 != null && animations3.Length != 0);
					}
					else
					{
						flag41 = false;
					}
					bool flag42 = flag41;
					if (flag42)
					{
						Animation weightedAnimation3 = this.GetWeightedAnimation(animationSet.Animations);
						string hash4;
						BlockyAnimation data3;
						bool flag43 = this._gameInstance.HashesByServerAssetPath.TryGetValue(weightedAnimation3.Animation_, ref hash4) && this._gameInstance.EntityStoreModule.GetAnimation(hash4, out data3);
						if (flag43)
						{
							float speed3 = (weightedAnimation3.Speed == 0f) ? 1f : weightedAnimation3.Speed;
							ptr.IdleAnimation = new EntityAnimation(data3, speed3, weightedAnimation3.BlendingDuration * 60f, weightedAnimation3.Looping, keepPreviousFirstPersonAnimation, ResourceManager.GetNetworkWwiseId(weightedAnimation3.SoundEventIndex), weightedAnimation3.Weight, weightedAnimation3.FootstepIntervals, weightedAnimation3.PassiveLoopCount, null, null, null, null, null, false);
						}
					}
				}
			}
			bool flag44;
			if (this.DoFirstPersonParticles())
			{
				EntityEffect.ApplicationEffects applicationEffects_4 = protocolEntityEffect.ApplicationEffects_;
				flag44 = (((applicationEffects_4 != null) ? applicationEffects_4.FirstPersonParticles : null) != null);
			}
			else
			{
				flag44 = false;
			}
			bool flag45 = flag44;
			ModelParticle[] array;
			if (!flag45)
			{
				EntityEffect.ApplicationEffects applicationEffects_5 = protocolEntityEffect.ApplicationEffects_;
				array = ((applicationEffects_5 != null) ? applicationEffects_5.Particles : null);
			}
			else
			{
				EntityEffect.ApplicationEffects applicationEffects_6 = protocolEntityEffect.ApplicationEffects_;
				array = ((applicationEffects_6 != null) ? applicationEffects_6.FirstPersonParticles : null);
			}
			ModelParticle[] array2 = array;
			bool flag46 = array2 != null;
			if (flag46)
			{
				ptr.ParticleSystems = new List<Entity.EntityParticle>();
				for (int i = 0; i < array2.Length; i++)
				{
					ModelParticleSettings particle = new ModelParticleSettings("");
					ParticleProtocolInitializer.Initialize(array2[i], ref particle, this._gameInstance.EntityStoreModule.NodeNameManager);
					Entity.EntityParticle entityParticle = this.AttachParticles(this._characterModel, this._entityParticles, particle, this.Scale);
					bool flag47 = entityParticle != null;
					if (flag47)
					{
						entityParticle.ParticleSystemProxy.SetFirstPerson(flag45);
						ptr.ParticleSystems.Add(entityParticle);
					}
				}
			}
			this.CalculateEffectTint();
			bool flag48 = this == this._gameInstance.LocalPlayer;
			if (flag48)
			{
				this._gameInstance.App.Interface.InGameView.OnEffectAdded(networkEffectIndex);
			}
		}

		// Token: 0x06004954 RID: 18772 RVA: 0x0011DC44 File Offset: 0x0011BE44
		public bool RemoveEffect(int networkEffectIndex)
		{
			int effectIndexFromNetworkEffectIndex = this.GetEffectIndexFromNetworkEffectIndex(networkEffectIndex);
			bool flag = effectIndexFromNetworkEffectIndex == -1 || this.EntityEffects[effectIndexFromNetworkEffectIndex].IsExpiring;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool displayDebugCommandsOnEntityEffect = this._gameInstance.EntityStoreModule.CurrentSetup.DisplayDebugCommandsOnEntityEffect;
				if (displayDebugCommandsOnEntityEffect)
				{
					this._gameInstance.Chat.Log("Removed " + this._gameInstance.EntityStoreModule.EntityEffects[networkEffectIndex].Id);
				}
				this._effectsOnEntityDirty = true;
				this.InternalRemoveEffect(effectIndexFromNetworkEffectIndex);
				result = true;
			}
			return result;
		}

		// Token: 0x06004955 RID: 18773 RVA: 0x0011DCDC File Offset: 0x0011BEDC
		private void InternalRemoveEffect(int localEntityEffectIndex)
		{
			ref Entity.UniqueEntityEffect ptr = ref this.EntityEffects[localEntityEffectIndex];
			bool flag = this.NetworkId == this._gameInstance.LocalPlayerNetworkId && ptr.ScreenEffect != null;
			if (flag)
			{
				this._gameInstance.ScreenEffectStoreModule.RemoveEntityScreenEffect(ptr.ScreenEffect);
			}
			bool flag2 = ptr.ParticleSystems != null;
			if (flag2)
			{
				for (int i = 0; i < ptr.ParticleSystems.Count; i++)
				{
					Entity.EntityParticle entityParticle = ptr.ParticleSystems[i];
					entityParticle.ParticleSystemProxy.Expire(false);
					bool flag3 = entityParticle.EntityPart == 2;
					if (flag3)
					{
						bool flag4 = this.PrimaryItem != null;
						if (flag4)
						{
							this.EntityItems[0].Particles.Remove(entityParticle);
						}
					}
					else
					{
						bool flag5 = entityParticle.EntityPart == 3;
						if (flag5)
						{
							bool flag6 = this.SecondaryItem != null;
							if (flag6)
							{
								this.EntityItems[(this.EntityItems.Count > 1) ? 1 : 0].Particles.Remove(entityParticle);
							}
						}
						else
						{
							this._entityParticles.Remove(entityParticle);
						}
					}
				}
			}
			this.ModelVFX.AnimationProgress = 0f;
			this.ModelVFX.AnimationDuration = 0f;
			bool flag7 = ptr.ModelRenderer != null;
			if (flag7)
			{
				bool flag8 = ptr.DespawnAnimation != null;
				if (flag8)
				{
					ptr.ModelRenderer.SetSlotAnimation(0, ptr.DespawnAnimation.Data, ptr.DespawnAnimation.Looping, 1f, 0f, ptr.DespawnAnimation.BlendingDuration, null, false);
				}
				else
				{
					ptr.ModelRenderer.SetSlotAnimation(0, null, true, 1f, 0f, 0f, null, false);
				}
			}
			bool flag9 = ptr.SoundEventReference.PlaybackId != -1;
			if (flag9)
			{
				this._gameInstance.AudioModule.ActionOnEvent(ref ptr.SoundEventReference, 0);
			}
			ptr.IsExpiring = true;
			this.CalculateEffectTint();
			bool flag10 = this == this._gameInstance.LocalPlayer;
			if (flag10)
			{
				this._gameInstance.App.Interface.InGameView.OnEffectRemoved(ptr.NetworkEffectIndex);
			}
		}

		// Token: 0x06004956 RID: 18774 RVA: 0x0011DF30 File Offset: 0x0011C130
		public void ClearEffects()
		{
			for (int i = 0; i < this.EntityEffects.Length; i++)
			{
				this.RemoveEffect(this.EntityEffects[i].NetworkEffectIndex);
			}
			this.EntityEffects = new Entity.UniqueEntityEffect[0];
		}

		// Token: 0x06004957 RID: 18775 RVA: 0x0011DF7C File Offset: 0x0011C17C
		public void SetCharacterModel(Model newModelPacket, string[] newArmorIds)
		{
			bool flag = newModelPacket != null;
			if (flag)
			{
				this.ModelPacket = newModelPacket;
				this.LoadCharacterAnimations();
			}
			bool flag2 = newArmorIds != null;
			if (flag2)
			{
				bool flag3 = newModelPacket == null;
				if (flag3)
				{
					bool flag4 = false;
					bool flag5 = this.ArmorIds.Length == newArmorIds.Length;
					if (flag5)
					{
						for (int i = 0; i < newArmorIds.Length; i++)
						{
							bool flag6 = newArmorIds[i] != this.ArmorIds[i];
							if (flag6)
							{
								flag4 = true;
								break;
							}
						}
					}
					else
					{
						flag4 = true;
					}
					bool flag7 = !flag4;
					if (flag7)
					{
						return;
					}
				}
				this.ArmorIds = newArmorIds;
			}
			this._armorLight.R = 0;
			this._armorLight.G = 0;
			this._armorLight.B = 0;
			this._itemLight.R = 0;
			this._itemLight.G = 0;
			this._itemLight.B = 0;
			this._itemCameraSettings = null;
			this.Type = Entity.EntityType.Character;
			bool flag8 = this.ModelPacket == null;
			if (flag8)
			{
				string[] array = new string[6];
				array[0] = "Attempted to set character model without sending a model first: NetworkId: ";
				array[1] = this.NetworkId.ToString();
				array[2] = ", Name: ";
				array[3] = this.Name;
				array[4] = ", PlayerSkin: ";
				int num = 5;
				PlayerSkin playerSkin = this.PlayerSkin;
				array[num] = ((playerSkin != null) ? playerSkin.ToString() : null);
				throw new Exception(string.Concat(array));
			}
			this.Scale = this.ModelPacket.Scale;
			this.EyeOffset = this.ModelPacket.EyeHeight;
			this.CrouchOffset = this.ModelPacket.CrouchOffset;
			this.DefaultHitbox = new BoundingBox(new Vector3(this.ModelPacket.Hitbox_.MinX, this.ModelPacket.Hitbox_.MinY, this.ModelPacket.Hitbox_.MinZ), new Vector3(this.ModelPacket.Hitbox_.MaxX, this.ModelPacket.Hitbox_.MaxY, this.ModelPacket.Hitbox_.MaxZ));
			this.CrouchHitbox = new BoundingBox(new Vector3(this.ModelPacket.Hitbox_.MinX, this.ModelPacket.Hitbox_.MinY, this.ModelPacket.Hitbox_.MinZ), new Vector3(this.ModelPacket.Hitbox_.MaxX, this.ModelPacket.Hitbox_.MaxY + this.CrouchOffset, this.ModelPacket.Hitbox_.MaxZ));
			this.Hitbox = this.DefaultHitbox;
			this.DetailBoundingBoxes.Clear();
			bool flag9 = this.ModelPacket.DetailBoxes != null;
			if (flag9)
			{
				foreach (KeyValuePair<string, DetailBox[]> keyValuePair in this.ModelPacket.DetailBoxes)
				{
					Entity.DetailBoundingBox[] array2 = new Entity.DetailBoundingBox[keyValuePair.Value.Length];
					for (int j = 0; j < array2.Length; j++)
					{
						DetailBox detailBox = keyValuePair.Value[j];
						array2[j] = new Entity.DetailBoundingBox
						{
							Offset = new Vector3(detailBox.Offset.X, detailBox.Offset.Y, detailBox.Offset.Z),
							Box = new BoundingBox(new Vector3(detailBox.Box.MinX, detailBox.Box.MinY, detailBox.Box.MinZ), new Vector3(detailBox.Box.MaxX, detailBox.Box.MaxY, detailBox.Box.MaxZ))
						};
					}
					this.DetailBoundingBoxes.Add(keyValuePair.Key, array2);
				}
			}
			this._modelCameraSettings = ((this.ModelPacket.Camera != null) ? new CameraSettings(this.ModelPacket.Camera) : new CameraSettings());
			bool flag10 = this._modelCameraSettings.PositionOffset == null;
			if (flag10)
			{
				this._modelCameraSettings.PositionOffset = new Vector3f(0.55f, 0.2f, 2.5f);
			}
			bool flag11 = this._modelCameraSettings.Yaw == null;
			if (flag11)
			{
				this._modelCameraSettings.Yaw = new CameraAxis(Entity.DefaultCameraAxis);
			}
			else
			{
				bool flag12 = this._modelCameraSettings.Yaw.AngleRange == null;
				if (flag12)
				{
					this._modelCameraSettings.Yaw.AngleRange = new Rangef(Entity.DefaultCameraAxis.AngleRange);
				}
				else
				{
					this._modelCameraSettings.Yaw.AngleRange.Min = MathHelper.ToRadians(this.ModelPacket.Camera.Yaw.AngleRange.Min);
					this._modelCameraSettings.Yaw.AngleRange.Max = MathHelper.ToRadians(this.ModelPacket.Camera.Yaw.AngleRange.Max);
				}
			}
			bool flag13 = this._modelCameraSettings.Pitch == null;
			if (flag13)
			{
				this._modelCameraSettings.Pitch = new CameraAxis(Entity.DefaultCameraAxis);
			}
			else
			{
				bool flag14 = this._modelCameraSettings.Pitch.AngleRange == null;
				if (flag14)
				{
					this._modelCameraSettings.Pitch.AngleRange = new Rangef(Entity.DefaultCameraAxis.AngleRange);
				}
				else
				{
					this._modelCameraSettings.Pitch.AngleRange.Min = MathHelper.ToRadians(this.ModelPacket.Camera.Pitch.AngleRange.Min);
					this._modelCameraSettings.Pitch.AngleRange.Max = MathHelper.ToRadians(this.ModelPacket.Camera.Pitch.AngleRange.Max);
				}
			}
			ModelRenderer modelRenderer = this.ModelRenderer;
			float animationTime = 0f;
			bool flag15 = this.ModelRenderer != null;
			if (flag15)
			{
				animationTime = this.ModelRenderer.GetSlotAnimationTime(0);
				this.ModelRenderer.Dispose();
				this.ClearCombatSequenceEffects();
				this.ClearFX();
				for (int k = 0; k < this.EntityItems.Count; k++)
				{
					Entity.EntityItem entityItem = this.EntityItems[k];
					bool flag16 = entityItem.SoundEventReference.PlaybackId != -1;
					if (flag16)
					{
						this._gameInstance.AudioModule.ActionOnEvent(ref entityItem.SoundEventReference, 0);
					}
					entityItem.ClearFX();
					entityItem.ModelRenderer.Dispose();
					entityItem.ModelRenderer = null;
				}
				this.EntityItems.Clear();
				this.ModelRenderer = null;
				bool flag17 = this.NetworkId == this._gameInstance.LocalPlayerNetworkId;
				if (flag17)
				{
					this._gameInstance.LocalPlayer.ClearFirstPersonView();
				}
			}
			for (int l = 0; l < this._animationSoundEventReferences.Length; l++)
			{
				ref AudioDevice.SoundEventReference ptr = ref this._animationSoundEventReferences[l];
				bool flag18 = ptr.PlaybackId != -1;
				if (flag18)
				{
					this._gameInstance.AudioModule.ActionOnEvent(ref ptr, 0);
					ptr = AudioDevice.SoundEventReference.None;
				}
			}
			bool flag19 = this.ModelPacket.Light != null;
			if (flag19)
			{
				ClientItemBaseProtocolInitializer.ParseLightColor(this.ModelPacket.Light, ref this._armorLight);
			}
			bool flag20 = this.PlayerSkin != null;
			if (flag20)
			{
				this.LoadPlayerModel();
			}
			else
			{
				this.LoadCharacterModel();
			}
			bool flag21 = this._characterModel == null;
			if (!flag21)
			{
				bool flag22 = this.ModelPacket.Particles != null;
				if (flag22)
				{
					for (int m = 0; m < this.ModelPacket.Particles.Length; m++)
					{
						ModelParticleSettings particle = new ModelParticleSettings("");
						ParticleProtocolInitializer.Initialize(this.ModelPacket.Particles[m], ref particle, this._gameInstance.EntityStoreModule.NodeNameManager);
						this.AttachParticles(this._characterModel, this._entityParticles, particle, this.Scale);
					}
				}
				bool flag23 = this.ModelPacket.Trails != null;
				if (flag23)
				{
					for (int n = 0; n < this.ModelPacket.Trails.Length; n++)
					{
						this.AttachTrails(this._characterModel, this._entityTrails, this.ModelPacket.Trails[n], this.Scale);
					}
				}
				bool flag24 = this.PrimaryItem != null;
				if (flag24)
				{
					this.AttachItem(this._characterModel, this.PrimaryItem, CharacterPartStore.RightAttachmentNodeNameId);
					this.HandleItemConditionAppearanceForAllEntityStats(this.PrimaryItem, 0);
				}
				bool flag25 = this.SecondaryItem != null;
				if (flag25)
				{
					this.AttachItem(this._characterModel, this.SecondaryItem, CharacterPartStore.LeftAttachmentNodeNameId);
					this.HandleItemConditionAppearanceForAllEntityStats(this.SecondaryItem, (this.PrimaryItem != null) ? 1 : 0);
				}
				this.SetupItemCamera();
				this.ModelRenderer = new ModelRenderer(this._characterModel, this._gameInstance.AtlasSizes, this._gameInstance.Engine.Graphics, this._gameInstance.FrameCounter, false);
				bool flag26 = modelRenderer != null;
				if (flag26)
				{
					this.ModelRenderer.CopyAllSlotAnimations(modelRenderer);
				}
				bool flag27 = this.ServerAnimations[0] != null;
				if (flag27)
				{
					this.SetServerAnimation(this.ServerAnimations[0], 0, -1f, false);
				}
				else
				{
					this.SetMovementAnimation(this._currentAnimationId ?? "Idle", animationTime, true, false);
				}
				bool flag28 = this.NetworkId == this._gameInstance.LocalPlayerNetworkId;
				if (flag28)
				{
					this._gameInstance.LocalPlayer.SetFirstPersonView(this._characterModel);
				}
				this.ModelRenderer.SetCameraNodes(this.CameraSettings);
				this.ModelRenderer.UpdatePose();
				ICameraController controller = this._gameInstance.CameraModule.Controller;
				bool flag29 = controller.AttachedTo == this;
				if (flag29)
				{
					controller.Reset(this._gameInstance, controller);
				}
			}
		}

		// Token: 0x06004958 RID: 18776 RVA: 0x0011E9D4 File Offset: 0x0011CBD4
		private void LoadCharacterModel()
		{
			this._characterModel = null;
			bool flag = this.ModelPacket.Model_ == null;
			if (flag)
			{
				this._gameInstance.App.DevTools.Error("Failed to load entity model, model isn't defined");
			}
			else
			{
				string hash;
				BlockyModel blockyModel;
				bool flag2 = !this._gameInstance.HashesByServerAssetPath.TryGetValue(this.ModelPacket.Model_, ref hash) || !this._gameInstance.EntityStoreModule.GetModel(hash, out blockyModel);
				if (flag2)
				{
					this._gameInstance.App.DevTools.Error("Failed to load entity model: " + this.ModelPacket.Model_);
				}
				else
				{
					bool flag3 = this.ModelPacket.Texture == null;
					if (flag3)
					{
						this._gameInstance.App.DevTools.Error("Failed to load entity model, texture isn't defined");
					}
					else
					{
						string key;
						bool flag4 = !this._gameInstance.HashesByServerAssetPath.TryGetValue(this.ModelPacket.Texture, ref key);
						if (flag4)
						{
							this._gameInstance.App.DevTools.Error("Failed to load entity texture: " + this.ModelPacket.Texture);
						}
						else
						{
							CharacterPartStore characterPartStore = this._gameInstance.App.CharacterPartStore;
							Point offset;
							bool flag5 = this._gameInstance.App.CharacterPartStore.ImageLocations.TryGetValue(this.ModelPacket.Texture, out offset);
							byte atlasIndex;
							if (flag5)
							{
								atlasIndex = 2;
							}
							else
							{
								bool flag6 = !this._gameInstance.EntityStoreModule.ImageLocations.TryGetValue(key, out offset);
								if (flag6)
								{
									this._gameInstance.App.DevTools.Error("Cannot use " + this.ModelPacket.Texture + " as an entity texture");
									return;
								}
								atlasIndex = 1;
							}
							this._characterModel = blockyModel.Clone();
							this._characterModel.SetAtlasIndex(atlasIndex);
							this._characterModel.OffsetUVs(offset);
							CharacterPartGradientSet characterPartGradientSet;
							CharacterPartTintColor characterPartTintColor;
							bool flag7 = this.ModelPacket.GradientSet != null && this.ModelPacket.GradientId != null && characterPartStore.GradientSets.TryGetValue(this.ModelPacket.GradientSet, out characterPartGradientSet) && characterPartGradientSet.Gradients.TryGetValue(this.ModelPacket.GradientId, out characterPartTintColor);
							if (flag7)
							{
								this._characterModel.SetGradientId(characterPartTintColor.GradientId);
							}
							bool flag8 = false;
							bool flag9 = false;
							foreach (string text in this.ArmorIds)
							{
								bool flag10 = text == null;
								if (!flag10)
								{
									ClientItemBase item = this._gameInstance.ItemLibraryModule.GetItem(text);
									bool flag11 = item == null || item.Armor == null;
									if (flag11)
									{
										this._gameInstance.App.DevTools.Error("Failed to load entity armor part, " + text + " isn't a valid armor item id.");
									}
									else
									{
										this._armorLight.R = (byte)MathHelper.Max((float)this._armorLight.R, (float)item.LightEmitted.R);
										this._armorLight.G = (byte)MathHelper.Max((float)this._armorLight.G, (float)item.LightEmitted.G);
										this._armorLight.B = (byte)MathHelper.Max((float)this._armorLight.B, (float)item.LightEmitted.B);
										this._characterModel.Attach(item.Model, this._gameInstance.EntityStoreModule.NodeNameManager, null, null, -1);
										bool flag12 = item.Armor.ArmorSlot == 0;
										if (flag12)
										{
											flag8 = true;
										}
										else
										{
											bool flag13 = item.Armor.ArmorSlot == 3;
											if (flag13)
											{
												flag9 = true;
											}
										}
									}
								}
							}
							bool flag14 = this.ModelPacket.Attachments != null;
							if (flag14)
							{
								foreach (ModelAttachment modelAttachment in this.ModelPacket.Attachments)
								{
									BlockyModel blockyModel2;
									byte value;
									Point value2;
									bool flag15 = !this.LoadAttachmentModel(modelAttachment.Model, modelAttachment.Texture, out blockyModel2, out value, out value2);
									if (!flag15)
									{
										bool flag16 = modelAttachment.GradientSet != null && modelAttachment.GradientId != null && characterPartStore.GradientSets.TryGetValue(modelAttachment.GradientSet, out characterPartGradientSet) && characterPartGradientSet.Gradients.TryGetValue(modelAttachment.GradientId, out characterPartTintColor);
										if (flag16)
										{
											blockyModel2.GradientId = characterPartTintColor.GradientId;
										}
										bool flag17 = flag9 && (modelAttachment.Model.Contains("Cosmetics/Legs") || modelAttachment.Model.Contains("Cosmetics/Feet"));
										if (!flag17)
										{
											bool flag18 = flag8 && modelAttachment.Model.Contains("Characters/Body_Attachments/Ears");
											if (!flag18)
											{
												bool flag19 = flag8 && modelAttachment.Model.Contains("Characters/Haircuts");
												if (flag19)
												{
													BlockyModelNode blockyModelNode = blockyModel2.AllNodes[0].Clone();
													BlockyModelNode blockyModelNode2 = blockyModel2.AllNodes[1].Clone();
													blockyModel2 = new BlockyModel(2);
													blockyModel2.AddNode(ref blockyModelNode, -1);
													blockyModel2.AddNode(ref blockyModelNode2, 0);
												}
												this._characterModel.Attach(blockyModel2, this._gameInstance.EntityStoreModule.NodeNameManager, new byte?(value), new Point?(value2), -1);
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06004959 RID: 18777 RVA: 0x0011EF84 File Offset: 0x0011D184
		private void LoadPlayerModel()
		{
			ClientPlayerSkin skin = new ClientPlayerSkin
			{
				BodyType = ((this.PlayerSkin.BodyType_ == null) ? CharacterBodyType.Masculine : CharacterBodyType.Feminine),
				SkinTone = this.PlayerSkin.SkinTone,
				Face = this.PlayerSkin.Face,
				Eyes = CharacterPartId.FromString(this.PlayerSkin.Eyes),
				Eyebrows = CharacterPartId.FromString(this.PlayerSkin.Eyebrows),
				SkinFeature = CharacterPartId.FromString(this.PlayerSkin.SkinFeature),
				Haircut = CharacterPartId.FromString(this.PlayerSkin.Haircut)
			};
			CharacterPartStore characterPartStore = this._gameInstance.App.CharacterPartStore;
			this.SetModel(characterPartStore, skin);
			HashSet<Cosmetic> cosmeticsToHide = this.LoadArmorParts();
			this.SetCosmeticsToDisplay(cosmeticsToHide, skin);
			this.LoadAttachments(characterPartStore, skin, cosmeticsToHide);
		}

		// Token: 0x0600495A RID: 18778 RVA: 0x0011F060 File Offset: 0x0011D260
		private HashSet<Cosmetic> LoadArmorParts()
		{
			HashSet<Cosmetic> hashSet = new HashSet<Cosmetic>();
			foreach (string text in this.ArmorIds)
			{
				bool flag = text == null;
				if (!flag)
				{
					ClientItemBase item = this._gameInstance.ItemLibraryModule.GetItem(text);
					bool flag2 = item == null || item.Armor == null;
					if (flag2)
					{
						this._gameInstance.App.DevTools.Error("Failed to load entity armor part, " + text + " isn't a valid armor item id.");
					}
					else
					{
						this._armorLight.R = (byte)MathHelper.Max((float)this._armorLight.R, (float)item.LightEmitted.R);
						this._armorLight.G = (byte)MathHelper.Max((float)this._armorLight.G, (float)item.LightEmitted.G);
						this._armorLight.B = (byte)MathHelper.Max((float)this._armorLight.B, (float)item.LightEmitted.B);
						this._characterModel.Attach(item.Model, this._gameInstance.EntityStoreModule.NodeNameManager, null, null, -1);
						Cosmetic[] cosmeticsToHide = this.GetCosmeticsToHide(item.Armor);
						bool flag3 = cosmeticsToHide != null;
						if (flag3)
						{
							hashSet.UnionWith(cosmeticsToHide);
						}
					}
				}
			}
			return hashSet;
		}

		// Token: 0x0600495B RID: 18779 RVA: 0x0011F1DC File Offset: 0x0011D3DC
		private void SetModel(CharacterPartStore characterPartStore, ClientPlayerSkin skin)
		{
			CharacterPartTintColor characterPartTintColor;
			characterPartStore.GradientSets["Skin"].Gradients.TryGetValue(skin.SkinTone, out characterPartTintColor);
			this._characterModel = characterPartStore.GetAndCloneModel(characterPartStore.GetBodyModelPath(skin.BodyType));
			this._characterModel.SetAtlasIndex(2);
			this._characterModel.OffsetUVs(characterPartStore.ImageLocations[(skin.BodyType == CharacterBodyType.Masculine) ? "Characters/Player_Textures/Masculine_Greyscale.png" : "Characters/Player_Textures/Feminine_Greyscale.png"]);
			this._characterModel.SetGradientId((characterPartTintColor != null) ? characterPartTintColor.GradientId : 0);
		}

		// Token: 0x0600495C RID: 18780 RVA: 0x0011F278 File Offset: 0x0011D478
		private void SetCosmeticsToDisplay(HashSet<Cosmetic> cosmeticsToHide, ClientPlayerSkin skin)
		{
			bool flag = !cosmeticsToHide.Contains(1);
			if (flag)
			{
				skin.FacialHair = CharacterPartId.FromString(this.PlayerSkin.FacialHair);
			}
			bool flag2 = !cosmeticsToHide.Contains(4);
			if (flag2)
			{
				skin.Pants = CharacterPartId.FromString(this.PlayerSkin.Pants);
			}
			bool flag3 = !cosmeticsToHide.Contains(5);
			if (flag3)
			{
				skin.Overpants = CharacterPartId.FromString(this.PlayerSkin.Overpants);
			}
			bool flag4 = !cosmeticsToHide.Contains(2);
			if (flag4)
			{
				skin.Undertop = CharacterPartId.FromString(this.PlayerSkin.Undertop);
			}
			bool flag5 = !cosmeticsToHide.Contains(3);
			if (flag5)
			{
				skin.Overtop = CharacterPartId.FromString(this.PlayerSkin.Overtop);
			}
			bool flag6 = !cosmeticsToHide.Contains(6);
			if (flag6)
			{
				skin.Shoes = CharacterPartId.FromString(this.PlayerSkin.Shoes);
			}
			bool flag7 = !cosmeticsToHide.Contains(8);
			if (flag7)
			{
				skin.HeadAccessory = CharacterPartId.FromString(this.PlayerSkin.HeadAccessory);
			}
			bool flag8 = !cosmeticsToHide.Contains(9);
			if (flag8)
			{
				skin.FaceAccessory = CharacterPartId.FromString(this.PlayerSkin.FaceAccessory);
			}
			bool flag9 = !cosmeticsToHide.Contains(10);
			if (flag9)
			{
				skin.EarAccessory = CharacterPartId.FromString(this.PlayerSkin.EarAccessory);
			}
			bool flag10 = !cosmeticsToHide.Contains(7);
			if (flag10)
			{
				skin.Gloves = CharacterPartId.FromString(this.PlayerSkin.Gloves);
			}
		}

		// Token: 0x0600495D RID: 18781 RVA: 0x0011F3FC File Offset: 0x0011D5FC
		private void LoadAttachments(CharacterPartStore characterPartStore, ClientPlayerSkin skin, HashSet<Cosmetic> cosmeticsToHide)
		{
			foreach (CharacterAttachment characterAttachment in characterPartStore.GetCharacterAttachments(skin))
			{
				BlockyModel blockyModel = characterPartStore.GetAndCloneModel(characterAttachment.Model);
				bool flag = blockyModel == null;
				if (flag)
				{
					Entity.Logger.Warn("Tried to load model which is not loaded or does not exist: {0}", characterAttachment.Texture);
				}
				else
				{
					Point value;
					bool flag2 = !characterPartStore.ImageLocations.TryGetValue(characterAttachment.Texture, out value);
					if (flag2)
					{
						Entity.Logger.Warn("Tried to load model texture which is not loaded or does not exist: {0}", characterAttachment.Texture);
					}
					else
					{
						bool flag3 = characterAttachment.IsUsingBaseNodeOnly || (characterAttachment.Model.StartsWith("Characters/Haircuts") && cosmeticsToHide.Contains(0));
						if (flag3)
						{
							BlockyModelNode blockyModelNode = blockyModel.AllNodes[0].Clone();
							BlockyModelNode blockyModelNode2 = blockyModel.AllNodes[1].Clone();
							blockyModel = new BlockyModel(2);
							blockyModel.AddNode(ref blockyModelNode, -1);
							blockyModel.AddNode(ref blockyModelNode2, 0);
						}
						blockyModel.GradientId = characterAttachment.GradientId;
						this._characterModel.Attach(blockyModel, this._gameInstance.EntityStoreModule.NodeNameManager, new byte?(2), new Point?(value), -1);
					}
				}
			}
		}

		// Token: 0x0600495E RID: 18782 RVA: 0x0011F574 File Offset: 0x0011D774
		private Cosmetic[] GetCosmeticsToHide(ClientItemArmor armor)
		{
			bool flag = armor.CosmeticsToHide != null;
			Cosmetic[] result;
			if (flag)
			{
				result = armor.CosmeticsToHide;
			}
			else
			{
				switch (armor.ArmorSlot)
				{
				case 0:
					result = Entity._defaultHeadCosmeticsToHide;
					break;
				case 1:
					result = Entity._defaultChestCosmeticsToHide;
					break;
				case 2:
					result = Entity._defaultHandsCosmeticsToHide;
					break;
				case 3:
					result = Entity._defaultLegsCosmeticsToHide;
					break;
				default:
					result = null;
					break;
				}
			}
			return result;
		}

		// Token: 0x0600495F RID: 18783 RVA: 0x0011F5E0 File Offset: 0x0011D7E0
		private void LoadCharacterAnimations()
		{
			this._animationSets.Clear();
			bool flag = this.ModelPacket.AnimationSets != null;
			if (flag)
			{
				foreach (KeyValuePair<string, AnimationSet> keyValuePair in this.ModelPacket.AnimationSets)
				{
					ClientAnimationSet clientAnimationSet = new ClientAnimationSet(keyValuePair.Key);
					clientAnimationSet.PassiveNextDelay = keyValuePair.Value.NextAnimationDelay;
					bool flag2 = keyValuePair.Value.Animations == null;
					if (!flag2)
					{
						for (int i = 0; i < keyValuePair.Value.Animations.Length; i++)
						{
							Animation animation = keyValuePair.Value.Animations[i];
							bool flag3 = animation == null;
							if (!flag3)
							{
								string hash;
								BlockyAnimation data;
								bool flag4 = animation.Animation_ != null && this._gameInstance.HashesByServerAssetPath.TryGetValue(animation.Animation_, ref hash) && this._gameInstance.EntityStoreModule.GetAnimation(hash, out data);
								if (flag4)
								{
									float speed = (animation.Speed == 0f) ? 1f : animation.Speed;
									bool keepPreviousFirstPersonAnimation = false;
									EntityAnimation entityAnimation = new EntityAnimation(data, speed, animation.BlendingDuration * 60f, animation.Looping, keepPreviousFirstPersonAnimation, ResourceManager.GetNetworkWwiseId(animation.SoundEventIndex), animation.Weight, animation.FootstepIntervals, animation.PassiveLoopCount, null, null, null, null, null, false);
									clientAnimationSet.Animations.Add(entityAnimation);
									clientAnimationSet.WeightSum += entityAnimation.Weight;
								}
								else
								{
									this._gameInstance.App.DevTools.Error("Failed to load entity animation in animationSet " + keyValuePair.Key + ": " + animation.Animation_);
								}
							}
						}
						bool flag5 = clientAnimationSet.Animations.Count == 0;
						if (flag5)
						{
							this._gameInstance.App.DevTools.Error("Failed to load animationSet " + keyValuePair.Key);
						}
						else
						{
							this._animationSets.Add(keyValuePair.Key, clientAnimationSet);
						}
					}
				}
			}
			this.SetAnimationSetFallback(this._animationSets, "Run", "Idle");
			this.SetAnimationSetFallback(this._animationSets, "RunBackward", "Run");
			this.SetAnimationSetFallback(this._animationSets, "Sprint", "Run");
			this.SetAnimationSetFallback(this._animationSets, "Jump", "Idle");
			this.SetAnimationSetFallback(this._animationSets, "JumpWalk", "Jump");
			this.SetAnimationSetFallback(this._animationSets, "JumpRun", "Jump");
			this.SetAnimationSetFallback(this._animationSets, "JumpSprint", "JumpRun");
			this.SetAnimationSetFallback(this._animationSets, "Fall", "Idle");
			this.SetAnimationSetFallback(this._animationSets, "Crouch", "Idle");
			this.SetAnimationSetFallback(this._animationSets, "CrouchWalk", "Run");
			this.SetAnimationSetFallback(this._animationSets, "CrouchWalkBackward", "CrouchWalk");
			this.SetAnimationSetFallback(this._animationSets, "CrouchSlide", "CrouchWalk");
			this.SetAnimationSetFallback(this._animationSets, "SafetyRoll", "CrouchWalk");
			this.SetAnimationSetFallback(this._animationSets, "FlyIdle", "Idle");
			this.SetAnimationSetFallback(this._animationSets, "Fly", "Run");
			this.SetAnimationSetFallback(this._animationSets, "FlyBackward", "Fly");
			this.SetAnimationSetFallback(this._animationSets, "FlyFast", "Fly");
			this.SetAnimationSetFallback(this._animationSets, "SwimBackward", "Swim");
			this.SetAnimationSetFallback(this._animationSets, "SwimFast", "Swim");
			this.SetAnimationSetFallback(this._animationSets, "SwimSink", "SwimDive");
			this.SetAnimationSetFallback(this._animationSets, "SwimFloat", "SwimSink");
			this.SetAnimationSetFallback(this._animationSets, "SwimIdle", "SwimSink");
			this.SetAnimationSetFallback(this._animationSets, "SwimDive", "Swim");
			this.SetAnimationSetFallback(this._animationSets, "SwimDiveFast", "SwimDive");
			this.SetAnimationSetFallback(this._animationSets, "SwimDiveBackward", "SwimDive");
			this.SetAnimationSetFallback(this._animationSets, "SwimJump", "JumpWalk");
			this.SetAnimationSetFallback(this._animationSets, "FluidIdle", "Idle");
			this.SetAnimationSetFallback(this._animationSets, "FluidWalk", "Run");
			this.SetAnimationSetFallback(this._animationSets, "FluidWalkBackward", "RunBackward");
			this.SetAnimationSetFallback(this._animationSets, "FluidRun", "Sprint");
		}

		// Token: 0x06004960 RID: 18784 RVA: 0x0011FAFC File Offset: 0x0011DCFC
		public void SetCharacterItemConsumable()
		{
			bool flag;
			if (this.ConsumableItem != null)
			{
				ClientItemBase primaryItem = this.PrimaryItem;
				string a = (primaryItem != null) ? primaryItem.Id : null;
				ClientItemBase consumableItem = this.ConsumableItem;
				flag = (a == ((consumableItem != null) ? consumableItem.Id : null));
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (!flag2)
			{
				this._originalPrimaryItem = this.PrimaryItem;
				ClientItemBase consumableItem2 = this.ConsumableItem;
				string newItemId = (consumableItem2 != null) ? consumableItem2.Id : null;
				ClientItemBase secondaryItem = this.SecondaryItem;
				this.SetCharacterItem(newItemId, (secondaryItem != null) ? secondaryItem.Id : null);
			}
		}

		// Token: 0x06004961 RID: 18785 RVA: 0x0011FB7C File Offset: 0x0011DD7C
		public void RestoreCharacterItem()
		{
			bool flag = this.ConsumableItem == null;
			if (!flag)
			{
				ClientItemBase originalPrimaryItem = this._originalPrimaryItem;
				string newItemId = (originalPrimaryItem != null) ? originalPrimaryItem.Id : null;
				ClientItemBase secondaryItem = this.SecondaryItem;
				this.SetCharacterItem(newItemId, (secondaryItem != null) ? secondaryItem.Id : null);
				this.ConsumableItem = null;
				this._originalPrimaryItem = null;
			}
		}

		// Token: 0x06004962 RID: 18786 RVA: 0x0011FBD4 File Offset: 0x0011DDD4
		public void ChangeCharacterItem(string newItemId, string newSecondaryItemId = null)
		{
			bool flag = this.ConsumableItem != null;
			if (flag)
			{
				ClientItemBase secondaryItem;
				this.GetEquipedItems(newItemId, newSecondaryItemId, out this._originalPrimaryItem, out secondaryItem);
				this.SecondaryItem = secondaryItem;
				ClientItemBase consumableItem = this.ConsumableItem;
				string newItemId2 = (consumableItem != null) ? consumableItem.Id : null;
				ClientItemBase secondaryItem2 = this.SecondaryItem;
				this.SetCharacterItem(newItemId2, (secondaryItem2 != null) ? secondaryItem2.Id : null);
			}
			else
			{
				this.SetCharacterItem(newItemId, newSecondaryItemId);
			}
		}

		// Token: 0x06004963 RID: 18787 RVA: 0x0011FC44 File Offset: 0x0011DE44
		private void GetEquipedItems(string newItemId, string newSecondaryItemId, out ClientItemBase newItem, out ClientItemBase newSecondaryItem)
		{
			newItem = this._gameInstance.ItemLibraryModule.GetItem(newItemId);
			bool flag3;
			if (newSecondaryItemId != null)
			{
				if (newItem != null)
				{
					ClientItemBase clientItemBase = newItem;
					bool? flag;
					if (clientItemBase == null)
					{
						flag = null;
					}
					else
					{
						ItemBase.ItemUtility utility = clientItemBase.Utility;
						flag = ((utility != null) ? new bool?(utility.Compatible) : null);
					}
					bool? flag2 = flag;
					flag3 = flag2.GetValueOrDefault();
				}
				else
				{
					flag3 = true;
				}
			}
			else
			{
				flag3 = false;
			}
			bool flag4 = flag3;
			if (flag4)
			{
				newSecondaryItem = this._gameInstance.ItemLibraryModule.GetItem(newSecondaryItemId);
			}
			else
			{
				newSecondaryItem = null;
			}
		}

		// Token: 0x06004964 RID: 18788 RVA: 0x0011FCCC File Offset: 0x0011DECC
		public void SetCharacterItem(string newItemId, string newSecondaryItemId = null)
		{
			ClientItemBase clientItemBase;
			ClientItemBase clientItemBase2;
			this.GetEquipedItems(newItemId, newSecondaryItemId, out clientItemBase, out clientItemBase2);
			PlayerEntity playerEntity;
			bool flag;
			if (this.NetworkId == this._gameInstance.LocalPlayerNetworkId)
			{
				playerEntity = (this as PlayerEntity);
				flag = (playerEntity != null);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				playerEntity.UpdateItemStatModifiers(clientItemBase, clientItemBase2);
			}
			bool flag3 = clientItemBase == this.PrimaryItem && clientItemBase2 == this.SecondaryItem;
			if (!flag3)
			{
				this._itemCameraSettings = null;
				this._itemLight.R = 0;
				this._itemLight.G = 0;
				this._itemLight.B = 0;
				this.PrimaryItem = clientItemBase;
				this.SecondaryItem = clientItemBase2;
				this.ClearCombatSequenceEffects();
				for (int i = 0; i < this.EntityItems.Count; i++)
				{
					Entity.EntityItem entityItem = this.EntityItems[i];
					bool flag4 = entityItem.SoundEventReference.PlaybackId != -1;
					if (flag4)
					{
						this._gameInstance.AudioModule.ActionOnEvent(ref entityItem.SoundEventReference, 0);
					}
					for (int j = 0; j < this.EntityItems[i].ParentParticles.Count; j++)
					{
						Entity.EntityParticle entityParticle = entityItem.ParentParticles[j];
						ParticleSystemProxy particleSystemProxy = entityParticle.ParticleSystemProxy;
						if (particleSystemProxy != null)
						{
							particleSystemProxy.Expire(false);
						}
						this._entityParticles.Remove(entityParticle);
					}
					for (int k = 0; k < this.EntityItems[i].ParentTrails.Count; k++)
					{
						Entity.EntityTrail entityTrail = entityItem.ParentTrails[k];
						bool flag5 = entityTrail.TrailProxy != null;
						if (flag5)
						{
							entityTrail.TrailProxy.IsExpired = true;
						}
						this._entityTrails.Remove(entityTrail);
					}
					entityItem.ClearFX();
					entityItem.ModelRenderer.Dispose();
					entityItem.ModelRenderer = null;
				}
				this.EntityItems.Clear();
				bool flag6 = this.NetworkId == this._gameInstance.LocalPlayerNetworkId;
				if (flag6)
				{
					this._gameInstance.LocalPlayer.ClearFirstPersonItems();
				}
				bool flag7 = this._characterModel == null;
				if (!flag7)
				{
					bool flag8 = this.PrimaryItem != null;
					if (flag8)
					{
						this.AttachItem(this._characterModel, this.PrimaryItem, CharacterPartStore.RightAttachmentNodeNameId);
						this.HandleItemConditionAppearanceForAllEntityStats(this.PrimaryItem, 0);
					}
					bool flag9 = this.SecondaryItem != null;
					if (flag9)
					{
						this.AttachItem(this._characterModel, this.SecondaryItem, CharacterPartStore.LeftAttachmentNodeNameId);
						this.HandleItemConditionAppearanceForAllEntityStats(this.SecondaryItem, (this.PrimaryItem != null) ? 1 : 0);
					}
					this.SetupItemCamera();
					float slotAnimationTime = this.ModelRenderer.GetSlotAnimationTime(0);
					bool flag10 = this.ServerAnimations[0] != null;
					if (flag10)
					{
						this.SetServerAnimation(this.ServerAnimations[0], 0, -1f, false);
					}
					else
					{
						this.SetMovementAnimation(this._currentAnimationId ?? "Idle", slotAnimationTime, true, false);
					}
					bool flag11 = this.NetworkId == this._gameInstance.LocalPlayerNetworkId;
					if (flag11)
					{
						this._gameInstance.LocalPlayer.SetFirstPersonItems();
					}
					ICameraController controller = this._gameInstance.CameraModule.Controller;
					bool flag12 = controller.AttachedTo == this;
					if (flag12)
					{
						controller.Reset(this._gameInstance, controller);
					}
					this.ModelRenderer.SetCameraNodes(this.CameraSettings);
				}
			}
		}

		// Token: 0x06004965 RID: 18789 RVA: 0x00120054 File Offset: 0x0011E254
		protected void RefreshCharacterItemParticles()
		{
			for (int i = 0; i < this.EntityItems.Count; i++)
			{
				Entity.EntityItem entityItem = this.EntityItems[i];
				for (int j = 0; j < this.EntityItems[i].Particles.Count; j++)
				{
					Entity.EntityParticle entityParticle = entityItem.Particles[j];
					ParticleSystemProxy particleSystemProxy = entityParticle.ParticleSystemProxy;
					if (particleSystemProxy != null)
					{
						particleSystemProxy.Expire(false);
					}
					this._entityParticles.Remove(entityParticle);
				}
				for (int k = 0; k < this.EntityItems[i].ParentParticles.Count; k++)
				{
					Entity.EntityParticle entityParticle2 = entityItem.ParentParticles[k];
					ParticleSystemProxy particleSystemProxy2 = entityParticle2.ParticleSystemProxy;
					if (particleSystemProxy2 != null)
					{
						particleSystemProxy2.Expire(false);
					}
					this._entityParticles.Remove(entityParticle2);
				}
				entityItem.ParentParticles.Clear();
				entityItem.Particles.Clear();
			}
			bool flag = this.PrimaryItem != null;
			if (flag)
			{
				this.RefreshItemParticles(this.PrimaryItem, this.EntityItems[0]);
				this.EntityItems[0].CurrentItemAppearanceCondition = null;
				this.HandleItemConditionAppearanceForAllEntityStats(this.PrimaryItem, 0);
			}
			bool flag2 = this.SecondaryItem != null;
			if (flag2)
			{
				int num = (this.PrimaryItem == null) ? 0 : 1;
				this.RefreshItemParticles(this.SecondaryItem, this.EntityItems[num]);
				this.EntityItems[num].CurrentItemAppearanceCondition = null;
				this.HandleItemConditionAppearanceForAllEntityStats(this.SecondaryItem, num);
			}
		}

		// Token: 0x06004966 RID: 18790 RVA: 0x00120204 File Offset: 0x0011E404
		private void RefreshItemParticles(ClientItemBase item, Entity.EntityItem entityItem)
		{
			bool flag = item == null;
			if (!flag)
			{
				BlockyModel model = item.Model;
				bool flag2 = item.BlockId != 0;
				if (flag2)
				{
					ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[item.BlockId];
					model = clientBlockType.FinalBlockyModel;
					bool flag3 = clientBlockType.Particles != null;
					if (flag3)
					{
						for (int i = 0; i < clientBlockType.Particles.Length; i++)
						{
							Entity.EntityParticle entityParticle = this.AttachParticles(model, entityItem.Particles, clientBlockType.Particles[i], this.Scale * entityItem.Scale * 0.5f);
							bool flag4 = entityParticle != null && entityParticle.EntityPart == 1;
							if (flag4)
							{
								entityItem.ParentParticles.Add(entityParticle);
							}
						}
					}
				}
				else
				{
					ModelParticleSettings[] array = (this.DoFirstPersonParticles() && item.FirstPersonParticles != null) ? item.FirstPersonParticles : item.Particles;
					bool flag5 = array != null;
					if (flag5)
					{
						for (int j = 0; j < array.Length; j++)
						{
							Entity.EntityParticle entityParticle2 = this.AttachParticles(model, entityItem.Particles, array[j], this.Scale * entityItem.Scale);
							bool flag6 = entityParticle2 != null && entityParticle2.EntityPart == 1;
							if (flag6)
							{
								entityItem.ParentParticles.Add(entityParticle2);
							}
						}
					}
				}
			}
		}

		// Token: 0x06004967 RID: 18791 RVA: 0x0012037C File Offset: 0x0011E57C
		protected void SetAnimationSetFallback(Dictionary<string, ClientAnimationSet> animations, string source, string fallback)
		{
			bool flag = !animations.ContainsKey(source);
			if (flag)
			{
				ClientAnimationSet clientAnimationSet;
				bool flag2 = animations.TryGetValue(fallback, out clientAnimationSet);
				if (flag2)
				{
					animations.Add(source, new ClientAnimationSet(source, clientAnimationSet));
				}
			}
		}

		// Token: 0x06004968 RID: 18792 RVA: 0x001203B8 File Offset: 0x0011E5B8
		public virtual bool AddCombatSequenceEffects(ModelParticle[] particles, ModelTrail[] trails)
		{
			List<Entity.EntityParticle> particleList = null;
			List<Entity.EntityTrail> trailList = null;
			BlockyModel model = null;
			float num = 1f;
			bool flag = this.PrimaryItem != null;
			if (flag)
			{
				particleList = this.EntityItems[0].Particles;
				trailList = this.EntityItems[0].Trails;
				model = this.PrimaryItem.Model;
				num = this.PrimaryItem.Scale;
			}
			bool flag2 = particles != null;
			if (flag2)
			{
				for (int i = 0; i < particles.Length; i++)
				{
					ModelParticleSettings particle = new ModelParticleSettings("");
					ParticleProtocolInitializer.Initialize(particles[i], ref particle, this._gameInstance.EntityStoreModule.NodeNameManager);
					Entity.EntityParticle entityParticle = this.AttachParticles(model, particleList, particle, this.Scale * num);
					bool flag3 = entityParticle != null;
					if (flag3)
					{
						this._combatSequenceParticles.Add(entityParticle);
					}
				}
			}
			bool flag4 = trails != null;
			if (flag4)
			{
				for (int j = 0; j < trails.Length; j++)
				{
					Entity.EntityTrail entityTrail = this.AttachTrails(model, trailList, trails[j], this.Scale * num);
					bool flag5 = entityTrail != null;
					if (flag5)
					{
						this._combatSequenceTrails.Add(entityTrail);
					}
				}
			}
			return true;
		}

		// Token: 0x06004969 RID: 18793 RVA: 0x001204F8 File Offset: 0x0011E6F8
		public void ClearCombatSequenceEffects()
		{
			for (int i = 0; i < this._combatSequenceParticles.Count; i++)
			{
				Entity.EntityParticle entityParticle = this._combatSequenceParticles[i];
				ParticleSystemProxy particleSystemProxy = entityParticle.ParticleSystemProxy;
				if (particleSystemProxy != null)
				{
					particleSystemProxy.Expire(false);
				}
				bool flag = entityParticle.EntityPart == 1;
				if (flag)
				{
					this._entityParticles.Remove(entityParticle);
				}
				else
				{
					bool flag2 = entityParticle.EntityPart == 3;
					if (flag2)
					{
						this.EntityItems[(this.EntityItems.Count > 1) ? 1 : 0].Particles.Remove(entityParticle);
					}
					else
					{
						this.EntityItems[0].Particles.Remove(entityParticle);
					}
				}
			}
			this._combatSequenceParticles.Clear();
			for (int j = 0; j < this._combatSequenceTrails.Count; j++)
			{
				Entity.EntityTrail entityTrail = this._combatSequenceTrails[j];
				bool flag3 = entityTrail.TrailProxy != null;
				if (flag3)
				{
					entityTrail.TrailProxy.IsExpired = true;
				}
				bool flag4 = entityTrail.EntityPart == 1;
				if (flag4)
				{
					this._entityTrails.Remove(entityTrail);
				}
				else
				{
					bool flag5 = entityTrail.EntityPart == 3;
					if (flag5)
					{
						this.EntityItems[(this.EntityItems.Count > 1) ? 1 : 0].Trails.Remove(entityTrail);
					}
					else
					{
						this.EntityItems[0].Trails.Remove(entityTrail);
					}
				}
			}
			this._combatSequenceTrails.Clear();
		}

		// Token: 0x0600496A RID: 18794 RVA: 0x00120698 File Offset: 0x0011E898
		private void SetupItemCamera()
		{
			ClientItemBase clientItemBase = this.PrimaryItem ?? this.SecondaryItem;
			CameraSettings cameraSettings;
			if (clientItemBase == null)
			{
				cameraSettings = null;
			}
			else
			{
				ClientItemPlayerAnimations playerAnimations = clientItemBase.PlayerAnimations;
				cameraSettings = ((playerAnimations != null) ? playerAnimations.Camera : null);
			}
			CameraSettings cameraSettings2 = cameraSettings;
			bool flag = cameraSettings2 == null;
			if (!flag)
			{
				this._itemCameraSettings = new CameraSettings(cameraSettings2);
				bool flag2 = this._itemCameraSettings.PositionOffset == null;
				if (flag2)
				{
					this._itemCameraSettings.PositionOffset = this._modelCameraSettings.PositionOffset;
				}
				bool flag3 = this._itemCameraSettings.Yaw == null;
				if (flag3)
				{
					this._itemCameraSettings.Yaw = new CameraAxis(this._modelCameraSettings.Yaw);
				}
				else
				{
					bool flag4 = this._itemCameraSettings.Yaw.AngleRange == null;
					if (flag4)
					{
						this._itemCameraSettings.Yaw.AngleRange = new Rangef(this._modelCameraSettings.Yaw.AngleRange);
					}
				}
				bool flag5 = this._itemCameraSettings.Pitch == null;
				if (flag5)
				{
					this._itemCameraSettings.Pitch = new CameraAxis(this._modelCameraSettings.Pitch);
				}
				else
				{
					bool flag6 = this._itemCameraSettings.Pitch.AngleRange == null;
					if (flag6)
					{
						this._itemCameraSettings.Pitch.AngleRange = new Rangef(this._modelCameraSettings.Pitch.AngleRange);
					}
				}
			}
		}

		// Token: 0x0600496B RID: 18795 RVA: 0x001207F0 File Offset: 0x0011E9F0
		private void AttachItem(BlockyModel model, ClientItemBase item, int defaultTargetAttachmentNameId)
		{
			Entity.EntityItem entityItem = new Entity.EntityItem(this._gameInstance);
			BlockyModel blockyModel = item.Model;
			entityItem.Scale = item.Scale;
			bool flag = item.BlockId != 0;
			if (flag)
			{
				ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[item.BlockId];
				blockyModel = clientBlockType.FinalBlockyModel;
				entityItem.Scale *= clientBlockType.BlockyModelScale;
			}
			bool flag2 = item.Armor == null && blockyModel.RootNodes.Count == 1 && blockyModel.AllNodes[blockyModel.RootNodes[0]].IsPiece;
			if (flag2)
			{
				entityItem.TargetNodeNameId = blockyModel.AllNodes[blockyModel.RootNodes[0]].NameId;
				entityItem.SetRootOffsets(Vector3.Negate(blockyModel.AllNodes[blockyModel.RootNodes[0]].Position), Quaternion.Inverse(blockyModel.AllNodes[blockyModel.RootNodes[0]].Orientation));
			}
			else
			{
				entityItem.TargetNodeNameId = defaultTargetAttachmentNameId;
			}
			bool flag3 = !model.NodeIndicesByNameId.TryGetValue(entityItem.TargetNodeNameId, out entityItem.TargetNodeIndex);
			if (flag3)
			{
				entityItem.TargetNodeIndex = 0;
			}
			entityItem.ModelRenderer = new ModelRenderer(blockyModel, this._gameInstance.AtlasSizes, this._gameInstance.Engine.Graphics, this._gameInstance.FrameCounter, false);
			BlockyAnimation animation = (item.BlockId != 0) ? this._gameInstance.MapModule.ClientBlockTypes[item.BlockId].BlockyAnimation : ((item != null) ? item.Animation : null);
			entityItem.ModelRenderer.SetSlotAnimation(0, animation, true, 1f, 0f, 0f, null, false);
			this.EntityItems.Add(entityItem);
			bool flag4 = item.BlockId != 0;
			if (flag4)
			{
				ClientBlockType clientBlockType2 = this._gameInstance.MapModule.ClientBlockTypes[item.BlockId];
				bool flag5 = clientBlockType2.Particles != null;
				if (flag5)
				{
					for (int i = 0; i < clientBlockType2.Particles.Length; i++)
					{
						Entity.EntityParticle entityParticle = this.AttachParticles(blockyModel, entityItem.Particles, clientBlockType2.Particles[i], this.Scale * entityItem.Scale * 0.5f);
						bool flag6 = entityParticle != null && entityParticle.EntityPart == 1;
						if (flag6)
						{
							entityItem.ParentParticles.Add(entityParticle);
						}
					}
				}
				this._gameInstance.AudioModule.PlaySoundEvent(clientBlockType2.SoundEventIndex, this.SoundObjectReference, ref entityItem.SoundEventReference);
				this._itemLight.R = (byte)MathHelper.Max((float)this._itemLight.R, (float)clientBlockType2.LightEmitted.R);
				this._itemLight.G = (byte)MathHelper.Max((float)this._itemLight.G, (float)clientBlockType2.LightEmitted.G);
				this._itemLight.B = (byte)MathHelper.Max((float)this._itemLight.B, (float)clientBlockType2.LightEmitted.B);
			}
			else
			{
				this._gameInstance.AudioModule.PlaySoundEvent(item.SoundEventIndex, this.SoundObjectReference, ref entityItem.SoundEventReference);
				ModelParticleSettings[] array = (this.DoFirstPersonParticles() && item.FirstPersonParticles != null) ? item.FirstPersonParticles : item.Particles;
				bool flag7 = array != null;
				if (flag7)
				{
					for (int j = 0; j < array.Length; j++)
					{
						Entity.EntityParticle entityParticle2 = this.AttachParticles(blockyModel, entityItem.Particles, array[j], this.Scale * entityItem.Scale);
						bool flag8 = entityParticle2 != null && entityParticle2.EntityPart == 1;
						if (flag8)
						{
							entityItem.ParentParticles.Add(entityParticle2);
						}
					}
				}
				bool flag9 = item.Trails != null;
				if (flag9)
				{
					for (int k = 0; k < item.Trails.Length; k++)
					{
						Entity.EntityTrail entityTrail = this.AttachTrails(blockyModel, entityItem.Trails, item.Trails[k], this.Scale * entityItem.Scale);
						bool flag10 = entityTrail != null && entityTrail.EntityPart == 1;
						if (flag10)
						{
							entityItem.ParentTrails.Add(entityTrail);
						}
					}
				}
				this._itemLight.R = (byte)MathHelper.Max((float)this._itemLight.R, (float)item.LightEmitted.R);
				this._itemLight.G = (byte)MathHelper.Max((float)this._itemLight.G, (float)item.LightEmitted.G);
				this._itemLight.B = (byte)MathHelper.Max((float)this._itemLight.B, (float)item.LightEmitted.B);
			}
		}

		// Token: 0x0600496C RID: 18796 RVA: 0x00120CE0 File Offset: 0x0011EEE0
		public void AddModelParticles(ModelParticleSettings[] modelParticles)
		{
			for (int i = 0; i < modelParticles.Length; i++)
			{
				this.AttachParticles(this._characterModel, this._entityParticles, modelParticles[i], this.Scale);
			}
		}

		// Token: 0x0600496D RID: 18797 RVA: 0x00120D20 File Offset: 0x0011EF20
		private Entity.EntityParticle AttachParticles(BlockyModel model, List<Entity.EntityParticle> particleList, ModelParticleSettings particle, float itemScale)
		{
			switch (particle.TargetEntityPart)
			{
			case 1:
				model = this._characterModel;
				particleList = this._entityParticles;
				itemScale = this.Scale;
				break;
			case 2:
			{
				bool flag = this.PrimaryItem == null;
				if (flag)
				{
					return null;
				}
				particleList = this.EntityItems[0].Particles;
				itemScale = this.Scale * this.PrimaryItem.Scale;
				bool flag2 = this.PrimaryItem.BlockId != 0;
				if (flag2)
				{
					model = this._gameInstance.MapModule.ClientBlockTypes[this.PrimaryItem.BlockId].FinalBlockyModel;
					itemScale *= 0.5f;
				}
				else
				{
					model = this.PrimaryItem.Model;
				}
				break;
			}
			case 3:
			{
				bool flag3 = this.SecondaryItem == null;
				if (flag3)
				{
					return null;
				}
				particleList = this.EntityItems[(this.EntityItems.Count > 1) ? 1 : 0].Particles;
				itemScale = this.Scale * this.SecondaryItem.Scale;
				bool flag4 = this.SecondaryItem.BlockId != 0;
				if (flag4)
				{
					model = this._gameInstance.MapModule.ClientBlockTypes[this.SecondaryItem.BlockId].FinalBlockyModel;
					itemScale *= 0.5f;
				}
				else
				{
					model = this.SecondaryItem.Model;
				}
				break;
			}
			default:
			{
				bool flag5 = model == null || particleList == null;
				if (flag5)
				{
					return null;
				}
				break;
			}
			}
			bool isTracked = !particle.DetachedFromModel;
			ParticleSystemProxy particleSystemProxy;
			bool flag6 = model == null || model.NodeCount == 0 || particle.SystemId == null || !this._gameInstance.ParticleSystemStoreModule.TrySpawnSystem(particle.SystemId, out particleSystemProxy, this.NetworkId == this._gameInstance.LocalPlayerNetworkId, isTracked);
			Entity.EntityParticle result;
			if (flag6)
			{
				result = null;
			}
			else
			{
				int num = 0;
				particleSystemProxy.Scale = itemScale * particle.Scale;
				bool flag7 = !particle.Color.IsTransparent;
				if (flag7)
				{
					particleSystemProxy.DefaultColor = particle.Color;
				}
				bool flag8 = particle.TargetNodeNameId != -1;
				if (flag8)
				{
					model.NodeIndicesByNameId.TryGetValue(particle.TargetNodeNameId, out num);
				}
				else
				{
					particle.TargetNodeNameId = model.AllNodes[0].NameId;
				}
				bool flag9 = num >= BlockyModel.MaxNodeCount;
				if (flag9)
				{
					result = null;
				}
				else
				{
					Entity.EntityParticle entityParticle = new Entity.EntityParticle(particleSystemProxy, particle.TargetEntityPart, num, particle.TargetNodeNameId, itemScale);
					entityParticle.PositionOffset = particle.PositionOffset * itemScale;
					entityParticle.RotationOffset = particle.RotationOffset;
					bool flag10 = !particle.DetachedFromModel;
					if (flag10)
					{
						particleList.Add(entityParticle);
						result = entityParticle;
					}
					else
					{
						AnimatedRenderer.NodeTransform nodeTransform = this.ModelRenderer.NodeTransforms[entityParticle.TargetNodeIndex];
						Vector3 position = this.RenderPosition + Vector3.Transform(nodeTransform.Position, this.RenderOrientation) * 0.015625f * this.Scale + Vector3.Transform(entityParticle.PositionOffset, this.RenderOrientation * nodeTransform.Orientation);
						entityParticle.ParticleSystemProxy.Position = position;
						entityParticle.ParticleSystemProxy.Rotation = this.RenderOrientation * nodeTransform.Orientation * entityParticle.RotationOffset;
						result = entityParticle;
					}
				}
			}
			return result;
		}

		// Token: 0x0600496E RID: 18798 RVA: 0x001210AC File Offset: 0x0011F2AC
		private Entity.EntityTrail AttachTrails(BlockyModel model, List<Entity.EntityTrail> trailList, ModelTrail modelTrail, float scale)
		{
			bool flag = modelTrail.TrailId == null;
			Entity.EntityTrail result;
			if (flag)
			{
				result = null;
			}
			else
			{
				int num = 0;
				switch (modelTrail.TargetEntityPart)
				{
				case 1:
					model = this._characterModel;
					trailList = this._entityTrails;
					scale = this.Scale;
					break;
				case 2:
				{
					bool flag2 = this.PrimaryItem == null;
					if (flag2)
					{
						return null;
					}
					model = ((this.PrimaryItem.BlockId != 0) ? this._gameInstance.MapModule.ClientBlockTypes[this.PrimaryItem.BlockId].FinalBlockyModel : this.PrimaryItem.Model);
					trailList = this.EntityItems[0].Trails;
					scale = this.Scale * this.PrimaryItem.Scale;
					break;
				}
				case 3:
				{
					bool flag3 = this.SecondaryItem == null;
					if (flag3)
					{
						return null;
					}
					model = ((this.SecondaryItem.BlockId != 0) ? this._gameInstance.MapModule.ClientBlockTypes[this.SecondaryItem.BlockId].FinalBlockyModel : this.SecondaryItem.Model);
					trailList = this.EntityItems[(this.EntityItems.Count > 1) ? 1 : 0].Trails;
					scale = this.Scale * this.SecondaryItem.Scale;
					break;
				}
				default:
				{
					bool flag4 = model == null || trailList == null;
					if (flag4)
					{
						return null;
					}
					break;
				}
				}
				TrailProxy trailProxy;
				bool flag5 = model.NodeCount == 0 || !this._gameInstance.TrailStoreModule.TrySpawnTrailProxy(modelTrail.TrailId, out trailProxy, this.NetworkId == this._gameInstance.LocalPlayerNetworkId);
				if (flag5)
				{
					result = null;
				}
				else
				{
					trailProxy.Scale = scale;
					bool flag6 = modelTrail.TargetNodeName != null;
					int num2;
					if (flag6)
					{
						num2 = this._gameInstance.EntityStoreModule.NodeNameManager.GetOrAddNameId(modelTrail.TargetNodeName);
						model.NodeIndicesByNameId.TryGetValue(num2, out num);
					}
					else
					{
						num2 = model.AllNodes[0].NameId;
					}
					bool flag7 = num >= BlockyModel.MaxNodeCount;
					if (flag7)
					{
						result = null;
					}
					else
					{
						Entity.EntityTrail entityTrail = new Entity.EntityTrail(trailProxy, modelTrail.TargetEntityPart, num, num2, modelTrail.FixedRotation);
						bool flag8 = modelTrail.PositionOffset != null;
						if (flag8)
						{
							entityTrail.PositionOffset.X = modelTrail.PositionOffset.X;
							entityTrail.PositionOffset.Y = modelTrail.PositionOffset.Y;
							entityTrail.PositionOffset.Z = modelTrail.PositionOffset.Z;
						}
						entityTrail.PositionOffset *= scale;
						bool flag9 = modelTrail.RotationOffset != null;
						if (flag9)
						{
							entityTrail.RotationOffset = Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(modelTrail.RotationOffset.Yaw), MathHelper.ToRadians(modelTrail.RotationOffset.Pitch), MathHelper.ToRadians(modelTrail.RotationOffset.Roll));
						}
						trailList.Add(entityTrail);
						result = entityTrail;
					}
				}
			}
			return result;
		}

		// Token: 0x0600496F RID: 18799 RVA: 0x001213CC File Offset: 0x0011F5CC
		private bool LoadAttachmentModel(string modelPath, string texturePath, out BlockyModel model, out byte atlasIndex, out Point uvOffset)
		{
			model = null;
			atlasIndex = 0;
			uvOffset = Point.Zero;
			bool flag = modelPath == null;
			bool result;
			if (flag)
			{
				this._gameInstance.App.DevTools.Error("Failed to load entity attachment, model isn't defined");
				result = false;
			}
			else
			{
				bool flag2 = this._gameInstance.App.CharacterPartStore.Models.TryGetValue("Common/" + modelPath, out model);
				if (!flag2)
				{
					string hash;
					bool flag3 = !this._gameInstance.HashesByServerAssetPath.TryGetValue(modelPath, ref hash) || !this._gameInstance.EntityStoreModule.GetModel(hash, out model);
					if (flag3)
					{
						this._gameInstance.App.DevTools.Error("Failed to load entity attachment model: " + modelPath + " with texture path: " + texturePath);
						return false;
					}
				}
				bool flag4 = texturePath == null;
				if (flag4)
				{
					this._gameInstance.App.DevTools.Error("Failed to load entity attachment, texture isn't defined");
					result = false;
				}
				else
				{
					bool flag5 = this._gameInstance.App.CharacterPartStore.ImageLocations.TryGetValue(texturePath, out uvOffset);
					if (flag5)
					{
						atlasIndex = 2;
					}
					else
					{
						string key;
						bool flag6 = !this._gameInstance.HashesByServerAssetPath.TryGetValue(texturePath, ref key);
						if (flag6)
						{
							this._gameInstance.App.DevTools.Error("Failed to load entity attachment texture: " + texturePath + " with model path: " + modelPath);
							return false;
						}
						bool flag7 = !this._gameInstance.EntityStoreModule.ImageLocations.TryGetValue(key, out uvOffset);
						if (flag7)
						{
							this._gameInstance.App.DevTools.Error("Cannot use " + texturePath + " as an entity attachment texture");
							return false;
						}
						atlasIndex = 1;
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06004970 RID: 18800 RVA: 0x001215A4 File Offset: 0x0011F7A4
		public void SetItem(Item itemPacket)
		{
			this._itemPacket = itemPacket;
			bool flag = this._itemParticleSystem != null;
			if (flag)
			{
				this._itemParticleSystem.Expire(true);
				this._itemParticleSystem = null;
			}
			bool flag2 = itemPacket == null;
			if (!flag2)
			{
				this.Type = Entity.EntityType.Item;
				this.DefaultHitbox = new BoundingBox(new Vector3(-0.25f, 0f, -0.25f), new Vector3(0.25f, 0.5f, 0.25f));
				this.Hitbox = this.DefaultHitbox;
				bool flag3 = this.ModelRenderer != null;
				if (flag3)
				{
					this.ModelRenderer.Dispose();
					this.ModelRenderer = null;
				}
				ClientItemBase item = this._gameInstance.ItemLibraryModule.GetItem(itemPacket.ItemId);
				bool flag4 = item == null;
				if (flag4)
				{
					item = this._gameInstance.ItemLibraryModule.GetItem("Unknown");
					this._gameInstance.App.DevTools.Error("Failed to load dropped item: " + itemPacket.ItemId);
				}
				this.ItemBase = item;
				BlockyModel model = item.Model;
				this.Scale = item.Scale;
				bool flag5 = item.BlockId != 0;
				if (flag5)
				{
					ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[item.BlockId];
					model = clientBlockType.FinalBlockyModel;
					this.Scale *= clientBlockType.BlockyModelScale;
				}
				this.ModelRenderer = new ModelRenderer(model, this._gameInstance.AtlasSizes, this._gameInstance.Engine.Graphics, this._gameInstance.FrameCounter, false);
				this.ModelRenderer.UpdatePose();
				bool flag6 = item.DroppedItemAnimation != null && !itemPacket.OverrideDroppedItemAnimation;
				if (flag6)
				{
					this.ModelRenderer.SetSlotAnimation(0, item.DroppedItemAnimation, true, 1f, 0f, 0f, null, false);
				}
				bool flag7 = item.ItemEntity.ParticleSystemId != null && !this.IsUsable() && !this.IsLocalEntity;
				if (flag7)
				{
					Color particleColor = item.ItemEntity.ParticleColor;
					UInt32Color defaultColor = (particleColor != null) ? UInt32Color.FromRGBA((byte)particleColor.Red, (byte)particleColor.Green, (byte)particleColor.Blue, byte.MaxValue) : UInt32Color.White;
					bool flag8 = this._gameInstance.ParticleSystemStoreModule.TrySpawnSystem(item.ItemEntity.ParticleSystemId, out this._itemParticleSystem, false, true);
					if (flag8)
					{
						this._itemParticleSystem.DefaultColor = defaultColor;
					}
				}
				bool showItemParticles = item.ItemEntity.ShowItemParticles;
				if (showItemParticles)
				{
					ModelParticleSettings[] array = (this.DoFirstPersonParticles() && item.FirstPersonParticles != null) ? item.FirstPersonParticles : item.Particles;
					bool flag9 = array != null;
					if (flag9)
					{
						for (int i = 0; i < array.Length; i++)
						{
							this.AttachParticles(model, this._entityParticles, array[i], this.Scale);
						}
					}
					bool flag10 = item.BlockId != 0;
					if (flag10)
					{
						ClientBlockType clientBlockType2 = this._gameInstance.MapModule.ClientBlockTypes[item.BlockId];
						bool flag11 = clientBlockType2.Particles != null;
						if (flag11)
						{
							for (int j = 0; j < clientBlockType2.Particles.Length; j++)
							{
								this.AttachParticles(model, this._entityParticles, clientBlockType2.Particles[j], this.Scale * 0.5f);
							}
						}
					}
				}
			}
		}

		// Token: 0x06004971 RID: 18801 RVA: 0x0012193C File Offset: 0x0011FB3C
		public void SetBlock(int blockId, float scale)
		{
			this.SetBlock(blockId);
			bool flag = scale > 1E-05f;
			if (flag)
			{
				this.Scale = scale;
			}
		}

		// Token: 0x06004972 RID: 18802 RVA: 0x00121968 File Offset: 0x0011FB68
		public void SetBlock(int blockId)
		{
			this._blockId = blockId;
			bool flag = blockId == 0;
			if (!flag)
			{
				this.Type = Entity.EntityType.Block;
				this.Scale = 2f;
				bool flag2 = this.ModelRenderer != null;
				if (flag2)
				{
					this.ModelRenderer.Dispose();
					this.ModelRenderer = null;
				}
				bool flag3 = blockId >= this._gameInstance.MapModule.ClientBlockTypes.Length;
				if (flag3)
				{
					this._gameInstance.App.DevTools.Error("Failed to find block for entity: " + blockId.ToString());
					blockId = 1;
				}
				ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[blockId];
				BoundingBox boundingBox = this._gameInstance.ServerSettings.BlockHitboxes[clientBlockType.HitboxType].BoundingBox;
				boundingBox.Min.X = boundingBox.Min.X - 0.5f;
				boundingBox.Min.Z = boundingBox.Min.Z - 0.5f;
				boundingBox.Max.X = boundingBox.Max.X - 0.5f;
				boundingBox.Max.Z = boundingBox.Max.Z - 0.5f;
				this.DefaultHitbox = boundingBox;
				this.Hitbox = boundingBox;
				BlockyModel finalBlockyModel = clientBlockType.FinalBlockyModel;
				this._characterModel = finalBlockyModel;
				this.ModelRenderer = new ModelRenderer(finalBlockyModel, this._gameInstance.AtlasSizes, this._gameInstance.Engine.Graphics, this._gameInstance.FrameCounter, false);
				this.ModelRenderer.UpdatePose();
			}
		}

		// Token: 0x06004973 RID: 18803 RVA: 0x00121AF0 File Offset: 0x0011FCF0
		public void SetDynamicLight(ColorLight dynamicLight)
		{
			bool flag = dynamicLight != null;
			if (flag)
			{
				ClientItemBaseProtocolInitializer.ParseLightColor(dynamicLight, ref this.DynamicLight);
			}
			else
			{
				this.DynamicLight = ColorRgb.Zero;
			}
			this.UpdateLight();
		}

		// Token: 0x06004974 RID: 18804 RVA: 0x00121B2C File Offset: 0x0011FD2C
		public void UpdateLight()
		{
			bool flag = !this.ShouldRender;
			if (flag)
			{
				this._gameInstance.EntityStoreModule.SetEntityLight(this.NetworkId, null);
			}
			byte b = this.DynamicLight.R;
			byte b2 = this.DynamicLight.G;
			byte b3 = this.DynamicLight.B;
			b = (byte)MathHelper.Max((float)b, (float)this._armorLight.R);
			b2 = (byte)MathHelper.Max((float)b2, (float)this._armorLight.G);
			b3 = (byte)MathHelper.Max((float)b3, (float)this._armorLight.B);
			b = (byte)MathHelper.Max((float)b, (float)this._itemLight.R);
			b2 = (byte)MathHelper.Max((float)b2, (float)this._itemLight.G);
			b3 = (byte)MathHelper.Max((float)b3, (float)this._itemLight.B);
			bool flag2 = b != 0 || b2 != 0 || b3 > 0;
			if (flag2)
			{
				this._gameInstance.EntityStoreModule.SetEntityLight(this.NetworkId, new Vector3?(new Vector3((float)b / 15f, (float)b2 / 15f, (float)b3 / 15f)));
			}
			else
			{
				this._gameInstance.EntityStoreModule.SetEntityLight(this.NetworkId, null);
			}
		}

		// Token: 0x06004975 RID: 18805 RVA: 0x00121C80 File Offset: 0x0011FE80
		public void RebuildRenderers(bool itemOnly)
		{
			bool flag = this._itemPacket != null;
			if (flag)
			{
				this.SetItem(this._itemPacket);
			}
			else
			{
				bool flag2 = this._blockId != 0;
				if (flag2)
				{
					this.SetBlock(this._blockId);
				}
				else
				{
					bool flag3 = this.ModelPacket != null;
					if (flag3)
					{
						bool flag4 = !itemOnly;
						if (flag4)
						{
							this.LoadCharacterAnimations();
							this.SetCharacterModel(null, null);
						}
						ClientItemBase primaryItem = this.PrimaryItem;
						string newItemId = (primaryItem != null) ? primaryItem.Id : null;
						this.PrimaryItem = null;
						ClientItemBase secondaryItem = this.SecondaryItem;
						string newSecondaryItemId = (secondaryItem != null) ? secondaryItem.Id : null;
						this.SecondaryItem = null;
						this.SetCharacterItem(newItemId, newSecondaryItemId);
						this.UpdateLight();
					}
				}
			}
		}

		// Token: 0x06004976 RID: 18806 RVA: 0x00121D3C File Offset: 0x0011FF3C
		protected EntityAnimation GetItemAnimation(ClientItemBase item, string animationId, bool useDefaultAnimations = true)
		{
			EntityAnimation entityAnimation = (item != null) ? item.GetAnimation(animationId) : null;
			bool flag = entityAnimation != null;
			EntityAnimation result;
			if (flag)
			{
				result = entityAnimation;
			}
			else
			{
				bool flag2 = useDefaultAnimations && this._gameInstance.ItemLibraryModule.DefaultItemPlayerAnimations.Animations.TryGetValue(animationId, out entityAnimation);
				if (flag2)
				{
					result = entityAnimation;
				}
				else
				{
					result = EntityAnimation.Empty;
				}
			}
			return result;
		}

		// Token: 0x06004977 RID: 18807 RVA: 0x00121D9C File Offset: 0x0011FF9C
		private void SetMovementAnimation(string animationId, float animationTime = -1f, bool force = false, bool noBlending = false)
		{
			bool flag = !this.CanPlayAnimations();
			if (!flag)
			{
				bool flag2 = this._currentAnimationId == animationId && !force;
				if (!flag2)
				{
					this.CycleJumpAnimations(animationId);
					bool flag3 = this._currentAnimationId != animationId;
					if (flag3)
					{
						this.ClearPassiveAnimationData();
					}
					this._currentAnimationId = animationId;
					this._currentAnimationRunTime = 0f;
					EntityAnimation entityAnimation = this.GetAnimation(animationId) ?? EntityAnimation.Empty;
					EntityAnimation itemAnimation = this.GetItemAnimation(this.PrimaryItem, this._currentAnimationId, true);
					EntityAnimation itemAnimation2 = this.GetItemAnimation(this.SecondaryItem, this._currentAnimationId, false);
					bool looping = entityAnimation.Looping;
					bool flag4 = !looping;
					if (flag4)
					{
						animationTime = 0f;
					}
					else
					{
						bool flag5 = animationTime == -1f;
						if (flag5)
						{
							animationTime = this.ModelRenderer.GetSlotAnimationTime(0);
						}
					}
					float num = entityAnimation.Speed;
					float num2 = itemAnimation.Speed;
					float num3 = itemAnimation2.Speed;
					bool flag6 = this == this._gameInstance.LocalPlayer;
					if (flag6)
					{
						float currentSpeedMultiplierDiff = this._gameInstance.CharacterControllerModule.MovementController.CurrentSpeedMultiplierDiff;
						num += num * currentSpeedMultiplierDiff;
						num2 += num2 * currentSpeedMultiplierDiff;
						num3 += num3 * currentSpeedMultiplierDiff;
					}
					if (noBlending)
					{
						this.ModelRenderer.SetSlotAnimationNoBlending(0, entityAnimation.Data, looping, num, animationTime);
						this.ModelRenderer.SetSlotAnimationNoBlending(1, itemAnimation.Data, looping, num2, animationTime);
						this.ModelRenderer.SetSlotAnimationNoBlending(2, itemAnimation2.Data, looping, num3, animationTime);
						bool flag7 = this.PrimaryItem != null && this.PrimaryItem.UsePlayerAnimations;
						if (flag7)
						{
							this.EntityItems[0].ModelRenderer.SetSlotAnimationNoBlending(1, itemAnimation.Data, looping, num2, animationTime);
						}
						bool flag8 = this.SecondaryItem != null && this.SecondaryItem.UsePlayerAnimations;
						if (flag8)
						{
							ModelRenderer modelRenderer = this.EntityItems[(this.PrimaryItem != null) ? 1 : 0].ModelRenderer;
							modelRenderer.SetSlotAnimationNoBlending(2, itemAnimation2.Data, looping, num3, animationTime);
						}
					}
					else
					{
						this.ModelRenderer.SetSlotAnimation(0, entityAnimation.Data, looping, num, animationTime, entityAnimation.BlendingDuration, null, false);
						this.ModelRenderer.SetSlotAnimation(1, itemAnimation.Data, looping, num2, animationTime, itemAnimation.BlendingDuration, null, false);
						this.ModelRenderer.SetSlotAnimation(2, itemAnimation2.Data, looping, num3, animationTime, itemAnimation2.BlendingDuration, null, false);
						bool flag9 = this.PrimaryItem != null && this.PrimaryItem.UsePlayerAnimations;
						if (flag9)
						{
							this.EntityItems[0].ModelRenderer.SetSlotAnimation(1, itemAnimation.Data, looping, num2, animationTime, itemAnimation.BlendingDuration, null, false);
						}
						bool flag10 = this.SecondaryItem != null && this.SecondaryItem.UsePlayerAnimations;
						if (flag10)
						{
							ModelRenderer modelRenderer2 = this.EntityItems[(this.PrimaryItem != null) ? 1 : 0].ModelRenderer;
							modelRenderer2.SetSlotAnimation(2, itemAnimation2.Data, looping, num3, animationTime, itemAnimation2.BlendingDuration, null, false);
						}
					}
					this.CurrentMovementAnimation = entityAnimation;
				}
			}
		}

		// Token: 0x06004978 RID: 18808 RVA: 0x001220D0 File Offset: 0x001202D0
		private void CycleJumpAnimations(string animationId)
		{
			bool flag = !Entity.JumpAnimations.Contains(animationId);
			if (!flag)
			{
				bool flag2 = this._lastJumpAnimation == null;
				if (flag2)
				{
					this._lastJumpAnimation = animationId;
				}
				else
				{
					ClientAnimationSet clientAnimationSet;
					bool flag3 = this._animationSets.TryGetValue(animationId, out clientAnimationSet);
					if (flag3)
					{
						ClientAnimationSet clientAnimationSet2;
						bool flag4 = this._lastJumpAnimation == animationId || (this._animationSets.TryGetValue(this._lastJumpAnimation, out clientAnimationSet2) && clientAnimationSet2.Animations.Count == clientAnimationSet.Animations.Count);
						if (flag4)
						{
							this._currentJumpAnimation++;
							this._currentJumpAnimation %= clientAnimationSet.Animations.Count;
						}
						else
						{
							this._currentJumpAnimation = 0;
						}
					}
					this._lastJumpAnimation = animationId;
				}
			}
		}

		// Token: 0x06004979 RID: 18809 RVA: 0x001221A5 File Offset: 0x001203A5
		public void PredictStatusAnimation(string animationId)
		{
			this.SetServerAnimation(animationId, 1, 0f, false);
			this.PredictedStatusCount++;
		}

		// Token: 0x0600497A RID: 18810 RVA: 0x001221C8 File Offset: 0x001203C8
		public virtual void SetServerAnimation(string animationId, AnimationSlot slot, float animationTime = -1f, bool storeCurrentAnimationId = false)
		{
			bool flag = !this.CanPlayAnimations();
			if (!flag)
			{
				int num;
				bool flag2 = !EntityAnimation.AnimationSlot.GetSlot(slot, out num);
				if (flag2)
				{
					this._gameInstance.App.DevTools.Error(string.Format("Unknown animation slot {0} on entity with model {1}", slot, this.ModelPacket.Model_));
				}
				else
				{
					bool flag3 = animationId != null;
					if (flag3)
					{
						bool flag4 = slot == 4;
						EntityAnimation entityAnimation;
						if (flag4)
						{
							Emote emote = this._gameInstance.App.CharacterPartStore.GetEmote(animationId);
							bool flag5 = emote == null;
							if (flag5)
							{
								this._gameInstance.App.DevTools.Error("No emote with id " + animationId + " on entity with model " + this.ModelPacket.Model_);
								return;
							}
							BlockyAnimation animation = this._gameInstance.App.CharacterPartStore.GetAnimation(emote.Animation);
							entityAnimation = new EntityAnimation(animation, 1f, 12f, false, false, 0U, 0f, Array.Empty<int>(), 1, null, null, null, null, null, false);
						}
						else
						{
							bool flag6 = slot == 2;
							if (flag6)
							{
								entityAnimation = this.GetItemAnimation(this.PrimaryItem, animationId, true);
								bool flag7 = entityAnimation == null || entityAnimation == EntityAnimation.Empty;
								if (flag7)
								{
									entityAnimation = this.GetAnimation(animationId);
								}
								bool flag8 = entityAnimation == null;
								if (flag8)
								{
									this._gameInstance.App.DevTools.Error("No animation with id " + animationId + " on entity with model " + this.ModelPacket.Model_);
									return;
								}
								this.CurrentActionAnimation = entityAnimation;
								this._currentAnimationId = animationId;
								num = 5;
							}
							else
							{
								entityAnimation = this.GetAnimation(animationId);
								bool flag9 = entityAnimation == null;
								if (flag9)
								{
									this._gameInstance.App.DevTools.Error("No animation with id " + animationId + " on entity with model " + this.ModelPacket.Model_);
									return;
								}
							}
						}
						bool flag10 = animationTime == -1f;
						if (flag10)
						{
							animationTime = this.ModelRenderer.GetSlotAnimationTime(num);
						}
						this.ModelRenderer.SetSlotAnimation(num, entityAnimation.Data, entityAnimation.Looping, entityAnimation.Speed, animationTime, entityAnimation.BlendingDuration, null, false);
						this.ServerAnimations[slot] = animationId;
						if (storeCurrentAnimationId)
						{
							this._currentAnimationId = animationId;
						}
						this.SetAnimationSound(entityAnimation, slot, false);
					}
					else
					{
						this.ModelRenderer.SetSlotAnimation(num, null, true, 1f, 0f, 12f, null, false);
						this.ServerAnimations[slot] = null;
						this.SetAnimationSound(null, slot, false);
					}
				}
			}
		}

		// Token: 0x0600497B RID: 18811 RVA: 0x00122474 File Offset: 0x00120674
		public virtual EntityAnimation GetTargetActionAnimation(InteractionType interactionType)
		{
			bool flag = interactionType == 1 && this.SecondaryItem != null;
			EntityAnimation result;
			if (flag)
			{
				EntityAnimation animation = this.SecondaryItem.GetAnimation("Attack");
				bool flag2 = animation != null;
				if (flag2)
				{
					result = animation;
				}
				else
				{
					result = EntityAnimation.Empty;
				}
			}
			else
			{
				string id = (interactionType == 1) ? "SecondaryAction" : "Attack";
				ClientItemBase primaryItem = this.PrimaryItem;
				EntityAnimation entityAnimation = (primaryItem != null) ? primaryItem.GetAnimation(id) : null;
				bool flag3 = entityAnimation != null;
				if (flag3)
				{
					result = entityAnimation;
				}
				else
				{
					result = EntityAnimation.Empty;
				}
			}
			return result;
		}

		// Token: 0x0600497C RID: 18812 RVA: 0x00122500 File Offset: 0x00120700
		public virtual void SetActionAnimation(EntityAnimation targetAnimation, float animationTime = 0f, bool holdLastFrame = false, bool force = false)
		{
			bool flag = !this.CanPlayAnimations() && targetAnimation != EntityAnimation.Empty;
			if (!flag)
			{
				bool flag2 = this.CurrentActionAnimation != null && this._isCurrentActionAnimationHoldingLastFrame && (force || this.CurrentActionAnimation != targetAnimation) && targetAnimation != EntityAnimation.Empty && !holdLastFrame;
				if (flag2)
				{
					float slotAnimationTime = this.ModelRenderer.GetSlotAnimationTime(5);
					this._previousActionAnimation = new Entity.ResumeActionAnimationData
					{
						EntityAnimation = this.CurrentActionAnimation,
						EntityModelRendererResumeTime = slotAnimationTime
					};
				}
				bool flag3 = targetAnimation == EntityAnimation.Empty;
				if (flag3)
				{
					bool flag4 = this._previousActionAnimation != null;
					this._previousActionAnimation = null;
					bool flag5 = this.CurrentActionAnimation != null && !this._isCurrentActionAnimationHoldingLastFrame && !this.IsDead(true) && flag4;
					if (flag5)
					{
						return;
					}
				}
				this.CurrentActionAnimation = targetAnimation;
				this._isCurrentActionAnimationHoldingLastFrame = holdLastFrame;
				BlockyAnimation blockyAnimation = (this._currentAnimationId == "Idle") ? targetAnimation.Data : targetAnimation.MovingData;
				BlockyAnimation slotAnimation = this.ModelRenderer.GetSlotAnimation(5);
				bool flag6 = blockyAnimation != slotAnimation || force;
				if (flag6)
				{
					this.ModelRenderer.SetSlotAnimation(5, blockyAnimation, targetAnimation.Looping, targetAnimation.Speed, animationTime, targetAnimation.BlendingDuration, null, force);
					bool flag7 = this.PrimaryItem != null;
					if (flag7)
					{
						this.EntityItems[0].ModelRenderer.SetSlotAnimation(5, blockyAnimation, targetAnimation.Looping, targetAnimation.Speed, animationTime, targetAnimation.BlendingDuration, null, force);
					}
				}
				BlockyAnimation faceData = targetAnimation.FaceData;
				BlockyAnimation slotAnimation2 = this.ModelRenderer.GetSlotAnimation(7);
				bool flag8 = faceData != slotAnimation2 || force;
				if (flag8)
				{
					this.ModelRenderer.SetSlotAnimation(7, faceData, true, 1f, 0f, 0f, null, force);
				}
				this.SetAnimationSound(targetAnimation, 2, false);
			}
		}

		// Token: 0x0600497D RID: 18813 RVA: 0x001226E4 File Offset: 0x001208E4
		public virtual void FinishAction()
		{
			bool flag = !this.CanPlayAnimations();
			if (!flag)
			{
				bool flag2 = this._previousActionAnimation != null;
				this.CurrentActionAnimation = null;
				this.ModelRenderer.SetSlotAnimation(5, null, true, 1f, 0f, 12f, null, false);
				bool flag3 = this.PrimaryItem != null;
				if (flag3)
				{
					this.EntityItems[0].ModelRenderer.SetSlotAnimation(5, null, true, 1f, 0f, 12f, null, false);
				}
				this.ModelRenderer.SetSlotAnimation(7, null, true, 1f, 0f, 0f, null, false);
				this.ClearCombatSequenceEffects();
				bool flag4 = flag2;
				if (flag4)
				{
					this.SetActionAnimation(this._previousActionAnimation.EntityAnimation, this._previousActionAnimation.EntityModelRendererResumeTime, true, false);
					this._previousActionAnimation = null;
				}
			}
		}

		// Token: 0x0600497E RID: 18814 RVA: 0x001227C4 File Offset: 0x001209C4
		public void SetEmotionAnimation(string animationId)
		{
			bool flag = !this.CanPlayAnimations();
			if (!flag)
			{
				Emote emote = this._gameInstance.App.CharacterPartStore.Emotes.Find((Emote x) => x.Id == animationId);
				BlockyAnimation animation = null;
				bool flag2 = emote != null;
				if (flag2)
				{
					animation = new BlockyAnimation();
					BlockyAnimationInitializer.Parse(AssetManager.GetBuiltInAsset("Common/" + emote.Animation), this._gameInstance.EntityStoreModule.NodeNameManager, ref animation);
				}
				this.ModelRenderer.SetSlotAnimation(7, animation, emote != null, 1f, 0f, 12f, null, false);
			}
		}

		// Token: 0x0600497F RID: 18815 RVA: 0x00122880 File Offset: 0x00120A80
		public void SetDebugAnimation(string animationId, string particleSystemId, string nodeName)
		{
			bool flag = !this.CanPlayAnimations();
			if (!flag)
			{
				EntityAnimation animation = this.GetAnimation(animationId);
				bool looping = animation.Looping;
				this.ModelRenderer.SetSlotAnimation(3, animation.Data, looping, 1f, 0f, 12f, null, false);
				bool flag2 = particleSystemId != null;
				if (flag2)
				{
					ModelParticleSettings modelParticleSettings = new ModelParticleSettings("");
					modelParticleSettings.SystemId = particleSystemId;
					bool flag3 = nodeName != null;
					if (flag3)
					{
						modelParticleSettings.TargetNodeNameId = this._gameInstance.EntityStoreModule.NodeNameManager.GetOrAddNameId(nodeName);
					}
					this.AttachParticles(this._characterModel, this._entityParticles, modelParticleSettings, this.Scale);
				}
			}
		}

		// Token: 0x06004980 RID: 18816 RVA: 0x00122934 File Offset: 0x00120B34
		private void SetAnimationSound(EntityAnimation animation, AnimationSlot slot, bool stopPreviousSoundEvent = false)
		{
			ref AudioDevice.SoundEventReference ptr = ref this._animationSoundEventReferences[slot];
			bool flag = ptr.PlaybackId != -1;
			if (flag)
			{
				this._gameInstance.AudioModule.ActionOnEvent(ref ptr, stopPreviousSoundEvent ? 0 : 3);
			}
			bool flag2 = animation == null;
			if (!flag2)
			{
				this._gameInstance.AudioModule.PlaySoundEvent(animation.SoundEventIndex, this.SoundObjectReference, ref ptr);
			}
		}

		// Token: 0x06004981 RID: 18817 RVA: 0x001229A2 File Offset: 0x00120BA2
		public void SetIsTangible(bool isTangible)
		{
			this._isTangible = isTangible;
		}

		// Token: 0x06004982 RID: 18818 RVA: 0x001229AC File Offset: 0x00120BAC
		public bool IsTangible()
		{
			return this._isTangible;
		}

		// Token: 0x06004983 RID: 18819 RVA: 0x001229C4 File Offset: 0x00120BC4
		public void SetInvulnerable(bool isInvulnerable)
		{
			this._isInvulnerable = isInvulnerable;
		}

		// Token: 0x06004984 RID: 18820 RVA: 0x001229D0 File Offset: 0x00120BD0
		public bool IsInvulnerable()
		{
			return this._isInvulnerable;
		}

		// Token: 0x06004985 RID: 18821 RVA: 0x001229E8 File Offset: 0x00120BE8
		public void SetUsable(bool usable)
		{
			this._usable = usable;
		}

		// Token: 0x06004986 RID: 18822 RVA: 0x001229F4 File Offset: 0x00120BF4
		public bool IsUsable()
		{
			return this._usable;
		}

		// Token: 0x06004987 RID: 18823 RVA: 0x00122A0C File Offset: 0x00120C0C
		public bool IsDead(bool nullHealthIsDead = true)
		{
			bool flag = this.ModelRenderer == null;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = DefaultEntityStats.Health == -1 || this.NetworkId < 0;
				if (flag2)
				{
					result = false;
				}
				else
				{
					ClientEntityStatValue entityStat = this.GetEntityStat(DefaultEntityStats.Health);
					bool flag3 = entityStat == null;
					if (flag3)
					{
						result = nullHealthIsDead;
					}
					else
					{
						result = (entityStat.Value <= entityStat.Min);
					}
				}
			}
			return result;
		}

		// Token: 0x06004988 RID: 18824 RVA: 0x00122A76 File Offset: 0x00120C76
		private bool CanPlayAnimations()
		{
			return !this.IsDead(false);
		}

		// Token: 0x06004989 RID: 18825 RVA: 0x00122A82 File Offset: 0x00120C82
		private int GetEntitySeed()
		{
			return Math.Abs(MathHelper.Hash(this.NetworkId) ^ MathHelper.Hash(Entity.ServerEntitySeed));
		}

		// Token: 0x0600498A RID: 18826 RVA: 0x00122A9F File Offset: 0x00120C9F
		public bool HasFX()
		{
			return this._entityParticles.Count + this._entityTrails.Count > 0;
		}

		// Token: 0x0600498B RID: 18827 RVA: 0x00122ABC File Offset: 0x00120CBC
		public void UpdateFX()
		{
			for (int i = 0; i < this._entityParticles.Count; i++)
			{
				Entity.EntityParticle entityParticle = this._entityParticles[i];
				entityParticle.ParticleSystemProxy.Visible = this.ShouldRender;
				entityParticle.ParticleSystemProxy.Position = this.RenderPosition + Vector3.Transform(this.ModelRenderer.NodeTransforms[entityParticle.TargetNodeIndex].Position, this.RenderOrientation) * 0.015625f * this.Scale + Vector3.Transform(entityParticle.PositionOffset, this.RenderOrientation * this.ModelRenderer.NodeTransforms[entityParticle.TargetNodeIndex].Orientation);
				entityParticle.ParticleSystemProxy.Rotation = this.RenderOrientation * this.ModelRenderer.NodeTransforms[entityParticle.TargetNodeIndex].Orientation * entityParticle.RotationOffset;
			}
			for (int j = 0; j < this._entityTrails.Count; j++)
			{
				Entity.EntityTrail entityTrail = this._entityTrails[j];
				entityTrail.TrailProxy.Visible = this.ShouldRender;
				entityTrail.TrailProxy.Position = this.RenderPosition + Vector3.Transform(this.ModelRenderer.NodeTransforms[entityTrail.TargetNodeIndex].Position, this.RenderOrientation) * 0.015625f * this.Scale + Vector3.Transform(entityTrail.PositionOffset, this.RenderOrientation * this.ModelRenderer.NodeTransforms[entityTrail.TargetNodeIndex].Orientation);
				entityTrail.TrailProxy.Rotation = (entityTrail.FixedRotation ? entityTrail.RotationOffset : (this.RenderOrientation * this.ModelRenderer.NodeTransforms[entityTrail.TargetNodeIndex].Orientation * entityTrail.RotationOffset));
			}
			for (int k = 0; k < this.EntityItems.Count; k++)
			{
				Entity.EntityItem entityItem = this.EntityItems[k];
				bool flag = entityItem.Particles.Count == 0 && entityItem.Trails.Count == 0;
				if (!flag)
				{
					ref AnimatedRenderer.NodeTransform ptr = ref this.ModelRenderer.NodeTransforms[entityItem.TargetNodeIndex];
					Quaternion quaternion = ptr.Orientation * entityItem.RootOrientationOffset;
					for (int l = 0; l < entityItem.Particles.Count; l++)
					{
						Entity.EntityParticle entityParticle2 = entityItem.Particles[l];
						ref AnimatedRenderer.NodeTransform ptr2 = ref entityItem.ModelRenderer.NodeTransforms[entityParticle2.TargetNodeIndex];
						entityParticle2.ParticleSystemProxy.Position = this.RenderPosition + Vector3.Transform(ptr.Position + Vector3.Transform(ptr2.Position * entityItem.Scale + entityItem.RootPositionOffset, quaternion), this.RenderOrientation) * 0.015625f * this.Scale + Vector3.Transform(entityParticle2.PositionOffset, this.RenderOrientation * quaternion * ptr2.Orientation);
						entityParticle2.ParticleSystemProxy.Rotation = this.RenderOrientation * quaternion * ptr2.Orientation * entityParticle2.RotationOffset;
					}
					for (int m = 0; m < entityItem.Trails.Count; m++)
					{
						Entity.EntityTrail entityTrail2 = entityItem.Trails[m];
						ref AnimatedRenderer.NodeTransform ptr3 = ref entityItem.ModelRenderer.NodeTransforms[entityTrail2.TargetNodeIndex];
						entityTrail2.TrailProxy.Position = this.RenderPosition + Vector3.Transform(ptr.Position + Vector3.Transform(ptr3.Position * entityItem.Scale + entityItem.RootPositionOffset, quaternion), this.RenderOrientation) * 0.015625f * this.Scale + Vector3.Transform(entityTrail2.PositionOffset, this.RenderOrientation * quaternion * ptr3.Orientation);
						entityTrail2.TrailProxy.Rotation = (entityTrail2.FixedRotation ? entityTrail2.RotationOffset : (this.RenderOrientation * quaternion * ptr3.Orientation * entityTrail2.RotationOffset));
					}
					entityItem.ModelVFX.UpdateAnimation(this._gameInstance.FrameTime);
				}
			}
		}

		// Token: 0x0600498C RID: 18828 RVA: 0x00122FD8 File Offset: 0x001211D8
		public void ClearFX()
		{
			for (int i = 0; i < this._entityParticles.Count; i++)
			{
				ParticleSystemProxy particleSystemProxy = this._entityParticles[i].ParticleSystemProxy;
				if (particleSystemProxy != null)
				{
					particleSystemProxy.Expire(false);
				}
			}
			this._entityParticles.Clear();
			for (int j = 0; j < this._entityTrails.Count; j++)
			{
				bool flag = this._entityTrails[j].TrailProxy != null;
				if (flag)
				{
					this._entityTrails[j].TrailProxy.IsExpired = true;
				}
			}
			this._entityTrails.Clear();
		}

		// Token: 0x0600498D RID: 18829 RVA: 0x00123084 File Offset: 0x00121284
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdatePosition(float deltaTime, int serverUpdatesPerSecond)
		{
			this.PositionProgress = Math.Min(1f, this.PositionProgress + deltaTime * (float)serverUpdatesPerSecond);
			this._position = Vector3.Lerp(this._previousPosition, this._nextPosition, this.PositionProgress);
			this.RenderPosition = this.Position;
		}

		// Token: 0x0600498E RID: 18830 RVA: 0x001230D8 File Offset: 0x001212D8
		public void UpdateEffectsFromServerPacket(EntityEffectUpdate[] changes)
		{
			foreach (EntityEffectUpdate entityEffectUpdate in changes)
			{
				EntityEffectUpdate.EffectOp type = entityEffectUpdate.Type;
				EntityEffectUpdate.EffectOp effectOp = type;
				if (effectOp != null)
				{
					if (effectOp != 1)
					{
						throw new ArgumentOutOfRangeException();
					}
					this.ServerRemoveEffect(entityEffectUpdate);
				}
				else
				{
					this.ServerAddEffect(entityEffectUpdate);
				}
			}
		}

		// Token: 0x0600498F RID: 18831 RVA: 0x0012312E File Offset: 0x0012132E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBodyOrientation(float deltaTime, int serverUpdatesPerSecond)
		{
			this.BodyOrientationProgress = Math.Min(1f, this.BodyOrientationProgress + deltaTime * (float)serverUpdatesPerSecond);
			this._bodyOrientation = Vector3.LerpAngle(this._previousBodyOrientation, this._nextBodyOrientation, this.BodyOrientationProgress);
		}

		// Token: 0x06004990 RID: 18832 RVA: 0x0012316C File Offset: 0x0012136C
		public void Update(float deltaTime, float distanceToCamera, bool skipUpdateLogic = false)
		{
			int serverUpdatesPerSecond = this._gameInstance.ServerUpdatesPerSecond;
			this.UpdatePosition(deltaTime, serverUpdatesPerSecond);
			this.UpdateWithoutPosition(deltaTime, distanceToCamera, skipUpdateLogic);
		}

		// Token: 0x06004991 RID: 18833 RVA: 0x0012319C File Offset: 0x0012139C
		public virtual void UpdateWithoutPosition(float deltaTime, float distanceToCamera, bool skipUpdateLogic = false)
		{
			bool flag = this.ShouldRender && !this._wasShouldRender;
			if (flag)
			{
				this.UpdateLight();
			}
			this._wasShouldRender = this.ShouldRender;
			int serverUpdatesPerSecond = this._gameInstance.ServerUpdatesPerSecond;
			this.UpdateBodyOrientation(deltaTime, serverUpdatesPerSecond);
			bool flag2 = !skipUpdateLogic;
			if (flag2)
			{
				Vector4 lightColorAtBlockPosition = this._gameInstance.MapModule.GetLightColorAtBlockPosition((int)Math.Floor((double)this.RenderPosition.X), (int)Math.Floor((double)(this.RenderPosition.Y + this.EyeOffset)), (int)Math.Floor((double)this.RenderPosition.Z));
				this.BlockLightColor = lightColorAtBlockPosition;
			}
			this.UpdateEntityEffects(deltaTime);
			this.UpdateStats();
			bool flag3 = this.Type == Entity.EntityType.Character;
			if (flag3)
			{
				this.UpdateCharacter(deltaTime, distanceToCamera, skipUpdateLogic);
			}
			else
			{
				bool flag4 = this.Type == Entity.EntityType.Item;
				if (flag4)
				{
					bool flag5 = this._itemParticleSystem != null;
					if (flag5)
					{
						this._itemParticleSystem.Position = this.RenderPosition;
					}
					bool flag6 = !this.IsUsable() && !this.IsLocalEntity;
					if (flag6)
					{
						this.ItemAnimationTime += deltaTime;
					}
					bool flag7 = this.ModelRenderer != null && this.ItemBase.DroppedItemAnimation != null;
					if (flag7)
					{
						this.UpdateModelRenderer(deltaTime, skipUpdateLogic);
					}
				}
				else
				{
					bool flag8 = this.Type == Entity.EntityType.Block;
					if (flag8)
					{
						this.UpdateCharacter(deltaTime, distanceToCamera, true);
					}
				}
			}
		}

		// Token: 0x06004992 RID: 18834 RVA: 0x0012331C File Offset: 0x0012151C
		private void UpdateEntityEffects(float deltaTime)
		{
			int num = this._serverEffects.FindIndex((Entity.ServerEffectEntry v) => DateTime.Now.Ticks / 10000L > v.CreationTime + 1000L);
			bool flag = num != -1;
			if (flag)
			{
				this._serverEffects.RemoveRange(0, num + 1);
			}
			int num2 = 0;
			float elapsedTime = deltaTime * 60f;
			float invServerUpdatesPerSecond = 1f / (float)this._gameInstance.ServerUpdatesPerSecond;
			for (int i = 0; i < this.EntityEffects.Length; i++)
			{
				ref Entity.UniqueEntityEffect ptr = ref this.EntityEffects[i];
				bool flag2 = ptr.NeedTick();
				if (flag2)
				{
					ptr.Tick(invServerUpdatesPerSecond, this, deltaTime);
				}
				bool flag3 = !ptr.IsInfinite;
				if (flag3)
				{
					ptr.RemainingDuration -= deltaTime;
					bool flag4 = ptr.RemainingDuration <= 0f;
					if (flag4)
					{
						this.RemoveEffect(ptr.NetworkEffectIndex);
					}
				}
				bool flag5 = ptr.ModelRenderer == null;
				if (flag5)
				{
					bool isExpiring = ptr.IsExpiring;
					if (isExpiring)
					{
						num2++;
					}
				}
				else
				{
					ptr.ModelRenderer.AdvancePlayback(elapsedTime);
					bool flag6 = !ptr.ModelRenderer.IsSlotPlayingAnimation(0);
					if (flag6)
					{
						bool isExpiring2 = ptr.IsExpiring;
						if (isExpiring2)
						{
							ptr.ModelRenderer.Dispose();
							ptr.ModelRenderer = null;
							num2++;
						}
						else
						{
							bool flag7 = ptr.IdleAnimation != null;
							if (flag7)
							{
								ptr.ModelRenderer.SetSlotAnimation(0, ptr.IdleAnimation.Data, ptr.IdleAnimation.Looping, ptr.IdleAnimation.Speed, 0f, ptr.IdleAnimation.BlendingDuration, null, false);
							}
						}
					}
				}
			}
			bool flag8 = num2 > 0;
			if (flag8)
			{
				int newSize = this.EntityEffects.Length - num2;
				ArrayUtils.CompactArray<Entity.UniqueEntityEffect>(ref this.EntityEffects, 0, this.EntityEffects.Length, delegate(ref Entity.UniqueEntityEffect e)
				{
					return e.IsExpiring;
				});
				this._effectsOnEntityDirty = true;
				Array.Resize<Entity.UniqueEntityEffect>(ref this.EntityEffects, newSize);
			}
		}

		// Token: 0x06004993 RID: 18835 RVA: 0x00123554 File Offset: 0x00121754
		private void UpdateCharacter(float deltaTime, float distanceToCamera, bool skipUpdateLogic = false)
		{
			bool flag = this.ServerAnimations[0] == null;
			if (flag)
			{
				this.UpdateMovementAnimation(deltaTime);
			}
			this.ModelVFX.UpdateAnimation(this._gameInstance.FrameTime);
			for (int i = 0; i < this.EntityItems.Count; i++)
			{
				this.EntityItems[i].ModelVFX.UpdateAnimation(this._gameInstance.FrameTime);
			}
			bool flag2 = this.ModelRenderer != null;
			if (flag2)
			{
				this.UpdateModelRenderer(deltaTime, skipUpdateLogic);
			}
			this.UpdateMovementFX(distanceToCamera, skipUpdateLogic);
			if (!skipUpdateLogic)
			{
				ClientEntityStatValue entityStat = this.GetEntityStat(DefaultEntityStats.Health);
				bool flag3 = entityStat != null;
				if (flag3)
				{
					bool flag4 = entityStat.Value == entityStat.Max && this.SmoothHealth >= 0.99f;
					if (flag4)
					{
						this.SmoothHealth = 1f;
					}
					else
					{
						this.SmoothHealth = MathHelper.Lerp(this.SmoothHealth, entityStat.Value / entityStat.Max, 0.35f);
					}
				}
				this.Hitbox = (this.GetRelativeMovementStates().IsCrouching ? this.CrouchHitbox : this.DefaultHitbox);
			}
		}

		// Token: 0x06004994 RID: 18836 RVA: 0x00123690 File Offset: 0x00121890
		private void UpdateModelRenderer(float deltaTime, bool skipUpdateLogic)
		{
			float elapsedTime = deltaTime * 60f;
			this.ModelRenderer.AdvancePlayback(elapsedTime);
			for (int i = 0; i < this.EntityItems.Count; i++)
			{
				this.EntityItems[i].ModelRenderer.AdvancePlayback(elapsedTime);
			}
			bool flag = !this.CanPlayAnimations();
			if (!flag)
			{
				bool flag2 = this.CurrentActionAnimation != null;
				if (flag2)
				{
					bool flag3 = !this.ModelRenderer.IsSlotPlayingAnimation(5) && !this._isCurrentActionAnimationHoldingLastFrame;
					if (flag3)
					{
						this.FinishAction();
					}
					else
					{
						float slotAnimationTime = this.ModelRenderer.GetSlotAnimationTime(5);
						this.SetActionAnimation(this.CurrentActionAnimation, slotAnimationTime, this._isCurrentActionAnimationHoldingLastFrame, false);
					}
				}
				float blendingDuration = skipUpdateLogic ? 0f : 12f;
				bool flag4 = this.ModelRenderer.GetSlotAnimation(3) != null && !this.ModelRenderer.IsSlotPlayingAnimation(3);
				if (flag4)
				{
					this.ModelRenderer.SetSlotAnimation(3, null, true, 1f, 0f, blendingDuration, null, false);
				}
				bool flag5 = this.ModelRenderer.GetSlotAnimation(4) != null && !this.ModelRenderer.IsSlotPlayingAnimation(4);
				if (flag5)
				{
					this.ModelRenderer.SetSlotAnimation(4, null, true, 1f, 0f, blendingDuration, null, false);
					this.ServerAnimations[1] = null;
				}
				bool flag6 = this.ModelRenderer.GetSlotAnimation(6) != null && !this.ModelRenderer.IsSlotPlayingAnimation(6);
				if (flag6)
				{
					this.ModelRenderer.SetSlotAnimation(6, null, true, 1f, 0f, blendingDuration, null, false);
					this.ServerAnimations[2] = null;
				}
				bool flag7 = this.ModelRenderer.GetSlotAnimation(7) != null && !this.ModelRenderer.IsSlotPlayingAnimation(7);
				if (flag7)
				{
					this.ModelRenderer.SetSlotAnimation(7, null, true, 1f, 0f, blendingDuration, null, false);
					this.ServerAnimations[3] = null;
				}
				bool flag8 = this.ModelRenderer.GetSlotAnimation(8) != null && !this.ModelRenderer.IsSlotPlayingAnimation(8);
				if (flag8)
				{
					this.ModelRenderer.SetSlotAnimation(8, null, true, 1f, 0f, 0f, null, false);
					this.ServerAnimations[4] = null;
				}
				bool flag9 = this.ServerAnimations[0] != null && !this.ModelRenderer.IsSlotPlayingAnimation(0);
				if (flag9)
				{
					this.ServerAnimations[0] = null;
				}
			}
		}

		// Token: 0x06004995 RID: 18837 RVA: 0x00123918 File Offset: 0x00121B18
		private void UpdateMovementAnimation(float deltaTime)
		{
			ref ClientMovementStates relativeMovementStates = ref this.GetRelativeMovementStates();
			bool flag = !relativeMovementStates.IsIdle;
			if (flag)
			{
				Vector3 vector = Vector3.Transform(this._nextPosition - this._previousPosition, Quaternion.Inverse(Quaternion.CreateFromYawPitchRoll(0f, this.BodyOrientation.Pitch, this.BodyOrientation.Roll)));
				float num = (float)Math.Atan2((double)(-(double)vector.X), (double)(-(double)vector.Z));
				float num2 = MathHelper.WrapAngle(num - this.LookOrientation.Yaw);
				bool flag2 = num2 > 2.0943952f || num2 < -2.0943952f;
				bool isFlying = relativeMovementStates.IsFlying;
				if (isFlying)
				{
					string animationId = flag2 ? (relativeMovementStates.IsForcedCrouching ? "FlyCrouchBackward" : "FlyBackward") : (relativeMovementStates.IsForcedCrouching ? "FlyCrouch" : (relativeMovementStates.IsSprinting ? "FlyFast" : "Fly"));
					this.SetMovementAnimation(animationId, -1f, false, false);
				}
				else
				{
					bool isClimbing = relativeMovementStates.IsClimbing;
					if (isClimbing)
					{
						float y = vector.Y;
						float num3 = MathHelper.WrapAngle(this.BodyOrientation.Yaw);
						bool flag3 = Math.Abs(vector.Z) > 0.01f;
						if (flag3)
						{
							bool flag4 = num3 > 0f;
							if (flag4)
							{
								bool flag5 = vector.Z < 0f;
								if (flag5)
								{
									this.SetMovementAnimation("ClimbRight", -1f, false, false);
								}
								else
								{
									this.SetMovementAnimation("ClimbLeft", -1f, false, false);
								}
							}
							else
							{
								bool flag6 = vector.Z < 0f;
								if (flag6)
								{
									this.SetMovementAnimation("ClimbLeft", -1f, false, false);
								}
								else
								{
									this.SetMovementAnimation("ClimbRight", -1f, false, false);
								}
							}
						}
						else
						{
							bool flag7 = Math.Abs(vector.X) > 0.01f;
							if (flag7)
							{
								bool flag8 = num3 < 1.5707964f && num3 > -1.5707964f;
								if (flag8)
								{
									bool flag9 = vector.X < 0f;
									if (flag9)
									{
										this.SetMovementAnimation("ClimbLeft", -1f, false, false);
									}
									else
									{
										this.SetMovementAnimation("ClimbRight", -1f, false, false);
									}
								}
								else
								{
									bool flag10 = vector.X < 0f;
									if (flag10)
									{
										this.SetMovementAnimation("ClimbRight", -1f, false, false);
									}
									else
									{
										this.SetMovementAnimation("ClimbLeft", -1f, false, false);
									}
								}
							}
							else
							{
								bool flag11 = y < 0f;
								if (flag11)
								{
									this.SetMovementAnimation("ClimbDown", -1f, false, false);
								}
								else
								{
									bool flag12 = y > 0f;
									if (flag12)
									{
										this.SetMovementAnimation("ClimbUp", -1f, false, false);
									}
								}
							}
						}
					}
					else
					{
						bool isSwimJumping = relativeMovementStates.IsSwimJumping;
						if (isSwimJumping)
						{
							bool flag13 = this._currentAnimationId != "SwimJump";
							if (flag13)
							{
								this.SetMovementAnimation("SwimJump", -1f, false, false);
							}
						}
						else
						{
							bool isSwimming = relativeMovementStates.IsSwimming;
							if (isSwimming)
							{
								float y2 = vector.Y;
								bool flag14 = !relativeMovementStates.IsHorizontalIdle;
								if (flag14)
								{
									bool flag15 = y2 >= 0f;
									if (flag15)
									{
										bool isOnGround = relativeMovementStates.IsOnGround;
										if (isOnGround)
										{
											bool isSprinting = relativeMovementStates.IsSprinting;
											if (isSprinting)
											{
												this.SetMovementAnimation("FluidRun", -1f, false, false);
											}
											else
											{
												bool isCrouching = relativeMovementStates.IsCrouching;
												if (isCrouching)
												{
													bool flag16 = flag2;
													if (flag16)
													{
														this.SetMovementAnimation("CrouchWalkBackward", -1f, false, false);
													}
													else
													{
														this.SetMovementAnimation("CrouchWalk", -1f, false, false);
													}
												}
												else
												{
													bool flag17 = flag2;
													if (flag17)
													{
														this.SetMovementAnimation("FluidWalkBackward", -1f, false, false);
													}
													else
													{
														this.SetMovementAnimation("FluidWalk", -1f, false, false);
													}
												}
											}
										}
										else
										{
											bool isSprinting2 = relativeMovementStates.IsSprinting;
											if (isSprinting2)
											{
												this.SetMovementAnimation("SwimFast", -1f, false, false);
											}
											else
											{
												bool flag18 = flag2;
												if (flag18)
												{
													this.SetMovementAnimation("SwimBackward", -1f, false, false);
												}
												else
												{
													this.SetMovementAnimation("Swim", -1f, false, false);
												}
											}
										}
									}
									else
									{
										bool isSprinting3 = relativeMovementStates.IsSprinting;
										if (isSprinting3)
										{
											this.SetMovementAnimation("SwimDiveFast", -1f, false, false);
										}
										else
										{
											bool flag19 = flag2;
											if (flag19)
											{
												this.SetMovementAnimation("SwimDiveBackward", -1f, false, false);
											}
											else
											{
												this.SetMovementAnimation("SwimDive", -1f, false, false);
											}
										}
									}
								}
								else
								{
									bool flag20 = y2 < 0f;
									if (flag20)
									{
										this.SetMovementAnimation("SwimSink", -1f, false, false);
									}
									else
									{
										bool flag21 = y2 > 0f;
										if (flag21)
										{
											this.SetMovementAnimation("SwimFloat", -1f, false, false);
										}
									}
								}
							}
							else
							{
								bool isRolling = relativeMovementStates.IsRolling;
								if (isRolling)
								{
									this.SetMovementAnimation("SafetyRoll", -1f, false, false);
								}
								else
								{
									bool isSliding = relativeMovementStates.IsSliding;
									if (isSliding)
									{
										this.SetMovementAnimation("CrouchSlide", -1f, false, false);
										PlayerEntity playerEntity = this as PlayerEntity;
										if (playerEntity != null)
										{
											playerEntity.SetFpAnimation("CrouchSlide", this.GetItemAnimation(null, "CrouchSlide", true));
										}
									}
									else
									{
										bool flag22 = relativeMovementStates.IsCrouching || relativeMovementStates.IsForcedCrouching;
										if (flag22)
										{
											bool flag23 = relativeMovementStates.IsJumping || relativeMovementStates.IsFalling;
											if (flag23)
											{
												this.SetMovementAnimation("JumpCrouch", -1f, false, false);
											}
											else
											{
												this.SetMovementAnimation(flag2 ? "CrouchWalkBackward" : "CrouchWalk", -1f, false, false);
											}
										}
										else
										{
											bool isJumping = relativeMovementStates.IsJumping;
											if (isJumping)
											{
												bool flag24 = this._currentAnimationId != "Jump" && this._currentAnimationId != "JumpWalk" && this._currentAnimationId != "JumpRun" && this._currentAnimationId != "JumpSprint" && this._currentAnimationId != "SwimJump";
												if (flag24)
												{
													bool isHorizontalIdle = relativeMovementStates.IsHorizontalIdle;
													if (isHorizontalIdle)
													{
														this.SetMovementAnimation("Jump", -1f, false, false);
													}
													else
													{
														bool isSprinting4 = relativeMovementStates.IsSprinting;
														if (isSprinting4)
														{
															this.SetMovementAnimation("JumpSprint", -1f, false, false);
														}
														else
														{
															bool isWalking = relativeMovementStates.IsWalking;
															if (isWalking)
															{
																this.SetMovementAnimation("JumpWalk", -1f, false, false);
															}
															else
															{
																this.SetMovementAnimation("JumpRun", -1f, false, false);
															}
														}
													}
												}
											}
											else
											{
												bool isFalling = relativeMovementStates.IsFalling;
												if (isFalling)
												{
													this.SetMovementAnimation("Fall", -1f, false, false);
												}
												else
												{
													bool isHorizontalIdle2 = relativeMovementStates.IsHorizontalIdle;
													if (isHorizontalIdle2)
													{
														this.SetMovementAnimation("Idle", -1f, false, false);
													}
													else
													{
														bool isMantling = relativeMovementStates.IsMantling;
														if (isMantling)
														{
															this.SetMovementAnimation("MantleUp", -1f, false, false);
															PlayerEntity playerEntity2 = this as PlayerEntity;
															if (playerEntity2 != null)
															{
																playerEntity2.SetFpAnimation("MantleUp", this.GetItemAnimation(null, "MantleUp", true));
															}
														}
														else
														{
															bool flag25 = relativeMovementStates.IsSprinting || (!relativeMovementStates.IsWalking && this._gameInstance.CharacterControllerModule.MovementController.SprintForceDurationLeft > 0f);
															string animationId2;
															if (flag25)
															{
																animationId2 = "Sprint";
															}
															else
															{
																bool isWalking2 = relativeMovementStates.IsWalking;
																if (isWalking2)
																{
																	animationId2 = (flag2 ? "WalkBackward" : "Walk");
																}
																else
																{
																	animationId2 = (flag2 ? "RunBackward" : "Run");
																}
															}
															this.SetMovementAnimation(animationId2, -1f, false, false);
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			else
			{
				bool isFlying2 = relativeMovementStates.IsFlying;
				if (isFlying2)
				{
					this.SetMovementAnimation(relativeMovementStates.IsForcedCrouching ? "FlyCrouchIdle" : "FlyIdle", -1f, false, false);
				}
				else
				{
					bool isCrouching2 = relativeMovementStates.IsCrouching;
					if (isCrouching2)
					{
						this.SetMovementAnimation("Crouch", -1f, false, false);
					}
					else
					{
						bool isClimbing2 = relativeMovementStates.IsClimbing;
						if (isClimbing2)
						{
							this.SetMovementAnimation("ClimbIdle", -1f, false, false);
						}
						else
						{
							bool isSwimming2 = relativeMovementStates.IsSwimming;
							if (isSwimming2)
							{
								this.SetMovementAnimation(relativeMovementStates.IsOnGround ? "FluidIdle" : "SwimIdle", -1f, false, false);
							}
							else
							{
								bool isMounting = relativeMovementStates.IsMounting;
								if (isMounting)
								{
									this.SetMovementAnimation("Mount", -1f, false, false);
								}
								else
								{
									this.SetMovementAnimation("Idle", -1f, false, false);
								}
							}
						}
					}
				}
			}
			this._currentAnimationRunTime += deltaTime;
			bool flag26 = this._currentAnimationRunTime >= StaticRandom.NextFloat((long)this.GetEntitySeed(), 2f, 10f);
			if (flag26)
			{
				this.SetMovementAnimation(this._currentAnimationId, -1f, true, false);
			}
			bool flag27 = this.ModelRenderer != null;
			if (flag27)
			{
				this.UpdatePassiveAnimation(deltaTime);
			}
		}

		// Token: 0x06004996 RID: 18838 RVA: 0x00124248 File Offset: 0x00122448
		private void UpdatePassiveAnimation(float deltaTime)
		{
			bool flag = this.ModelRenderer.IsSlotPlayingAnimation(3);
			if (!flag)
			{
				bool flag2 = this._passiveAnimation != null;
				if (flag2)
				{
					bool flag3 = this._countPassiveAnimation > 0;
					if (flag3)
					{
						this.PlayPassiveAnimation();
						return;
					}
					this._passiveAnimation = null;
				}
				this._nextPassiveAnimationTimer += deltaTime;
				bool flag4 = this._nextPassiveAnimationTimer < this._nextPassiveAnimationThreshold;
				if (!flag4)
				{
					ClientAnimationSet clientAnimationSet;
					bool flag5 = !this.TryGetTargetPassiveAnimation(out clientAnimationSet);
					if (flag5)
					{
						this.ClearPassiveAnimationData();
					}
					else
					{
						bool flag6 = this._nextPassiveAnimationThreshold > 0f;
						if (flag6)
						{
							this.SetPassiveAnimation(clientAnimationSet.Id);
							this.PlayPassiveAnimation();
						}
						this._nextPassiveAnimationThreshold = StaticRandom.NextFloat((long)this.GetEntitySeed(), clientAnimationSet.PassiveNextDelay.Min, clientAnimationSet.PassiveNextDelay.Max);
						this._nextPassiveAnimationTimer = 0f;
					}
				}
			}
		}

		// Token: 0x06004997 RID: 18839 RVA: 0x00124338 File Offset: 0x00122538
		public void ClearPassiveAnimationData()
		{
			this._nextPassiveAnimationTimer = 0f;
			this._nextPassiveAnimationThreshold = 0f;
			this._passiveAnimation = null;
			bool flag = this.ModelRenderer.GetSlotAnimation(3) != null;
			if (flag)
			{
				this.ModelRenderer.SetSlotAnimation(3, null, true, 1f, 0f, 12f, null, false);
			}
		}

		// Token: 0x06004998 RID: 18840 RVA: 0x00124398 File Offset: 0x00122598
		private void SetPassiveAnimation(string targetAnimationSetId)
		{
			EntityAnimation animation = this.GetAnimation(targetAnimationSetId);
			this._countPassiveAnimation = animation.PassiveLoopCount;
			this._passiveAnimation = animation;
		}

		// Token: 0x06004999 RID: 18841 RVA: 0x001243C4 File Offset: 0x001225C4
		private void PlayPassiveAnimation()
		{
			this.ModelRenderer.SetSlotAnimation(3, this._passiveAnimation.Data, false, this._passiveAnimation.Speed, 0f, this._passiveAnimation.BlendingDuration, null, false);
			this._countPassiveAnimation--;
		}

		// Token: 0x0600499A RID: 18842 RVA: 0x00124418 File Offset: 0x00122618
		private bool TryGetTargetPassiveAnimation(out ClientAnimationSet targetAnimationSet)
		{
			ClientAnimationSet clientAnimationSet;
			bool flag = !this._animationSets.TryGetValue(this._currentAnimationId + "Passive", out clientAnimationSet);
			bool result;
			if (flag)
			{
				targetAnimationSet = null;
				result = false;
			}
			else
			{
				bool flag2 = clientAnimationSet.PassiveNextDelay == null;
				if (flag2)
				{
					targetAnimationSet = null;
					result = false;
				}
				else
				{
					for (int i = 4; i < 9; i++)
					{
						bool flag3 = this.ModelRenderer.GetSlotAnimation(i) != null;
						if (flag3)
						{
							targetAnimationSet = null;
							return false;
						}
					}
					targetAnimationSet = clientAnimationSet;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600499B RID: 18843 RVA: 0x001244A8 File Offset: 0x001226A8
		private void UpdateMovementFX(float distanceToCamera, bool skipUpdateLogic)
		{
			ref ClientMovementStates relativeMovementStates = ref this.GetRelativeMovementStates();
			bool flag = relativeMovementStates.IsInFluid;
			bool flag2 = !skipUpdateLogic && distanceToCamera < 20f;
			if (flag2)
			{
				bool flag3 = !this._wasInFluid && flag;
				if (flag3)
				{
					Vector3 nextPosition = this._nextPosition;
					int num = this._gameInstance.MapModule.GetBlock(nextPosition, 0);
					ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[num];
					bool flag4 = num == 0;
					if (flag4)
					{
						flag = false;
					}
					else
					{
						bool flag5 = clientBlockType.FluidBlockId != num;
						if (flag5)
						{
							num = clientBlockType.FluidBlockId;
							clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[num];
						}
					}
					bool flag6 = (!relativeMovementStates.IsOnGround || clientBlockType.VerticalFill > 3) && clientBlockType.BlockParticleSetId != null;
					if (flag6)
					{
						ClientBlockParticleEvent particleEvent = (this._fallHeight - nextPosition.Y > 3f) ? ClientBlockParticleEvent.HardLand : ClientBlockParticleEvent.SoftLand;
						int num2 = 1;
						nextPosition.Y = (float)Math.Floor((double)nextPosition.Y);
						while ((float)num2 < this.Hitbox.Max.Y + 1f)
						{
							int block = this._gameInstance.MapModule.GetBlock((int)Math.Floor((double)nextPosition.X), (int)nextPosition.Y + num2, (int)Math.Floor((double)nextPosition.Z), 0);
							bool flag7 = block == 0;
							if (flag7)
							{
								break;
							}
							ClientBlockType clientBlockType2 = this._gameInstance.MapModule.ClientBlockTypes[block];
							bool flag8 = num != block;
							if (flag8)
							{
								bool flag9 = clientBlockType2.FluidBlockId == 0;
								if (flag9)
								{
									break;
								}
								bool flag10 = clientBlockType2.FluidBlockId != num;
								if (flag10)
								{
									num = clientBlockType2.FluidBlockId;
									clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[num];
								}
							}
							num2++;
						}
						ParticleSystemProxy particleSystemProxy;
						bool flag11 = (float)(num2 - 1) <= this.Hitbox.Max.Y && this._gameInstance.ParticleSystemStoreModule.TrySpawnBlockSystem(nextPosition, clientBlockType, particleEvent, out particleSystemProxy, false, false);
						if (flag11)
						{
							byte b = (clientBlockType.MaxFillLevel == 0) ? 8 : clientBlockType.MaxFillLevel;
							nextPosition.Y += (float)(num2 - (int)(1 - clientBlockType.VerticalFill / b));
							particleSystemProxy.Position = nextPosition;
							particleSystemProxy.Rotation = this.RenderOrientation;
						}
					}
				}
				else
				{
					bool flag12 = !this._wasOnGround && relativeMovementStates.IsOnGround && !flag;
					if (flag12)
					{
						Vector3 nextPosition2 = this._nextPosition;
						nextPosition2.Y -= 0.01f;
						float num3 = this._fallHeight - nextPosition2.Y;
						bool flag13 = num3 > 0.5f;
						if (flag13)
						{
							ClientBlockParticleEvent particleEvent2 = (num3 > 3f) ? ClientBlockParticleEvent.HardLand : ClientBlockParticleEvent.SoftLand;
							int block2 = this._gameInstance.MapModule.GetBlock(nextPosition2, 0);
							ClientBlockType clientBlockType3 = this._gameInstance.MapModule.ClientBlockTypes[block2];
							bool flag14 = clientBlockType3.BlockParticleSetId != null && clientBlockType3.FluidBlockId == 0;
							if (flag14)
							{
								ParticleSystemProxy particleSystemProxy2;
								bool flag15 = this._gameInstance.ParticleSystemStoreModule.TrySpawnBlockSystem(nextPosition2, clientBlockType3, particleEvent2, out particleSystemProxy2, false, false);
								if (flag15)
								{
									particleSystemProxy2.Position = nextPosition2;
									particleSystemProxy2.Rotation = this.RenderOrientation;
								}
							}
						}
					}
				}
				Vector3 renderPosition = this.RenderPosition;
				int num4 = this._gameInstance.MapModule.GetBlock(renderPosition, 1);
				ClientBlockType clientBlockType4 = this._gameInstance.MapModule.ClientBlockTypes[num4];
				bool flag16 = clientBlockType4.FluidBlockId != 0;
				if (flag16)
				{
					num4 = clientBlockType4.FluidBlockId;
					clientBlockType4 = this._gameInstance.MapModule.ClientBlockTypes[num4];
				}
				renderPosition.Y = (float)Math.Floor((double)renderPosition.Y);
				bool flag17 = num4 != this._previousBlockId;
				if (flag17)
				{
					bool flag18 = (float)this._previousBlockY == renderPosition.Y;
					if (flag18)
					{
						renderPosition.Y += (float)clientBlockType4.VerticalFill * (1f / (float)clientBlockType4.MaxFillLevel);
					}
					else
					{
						renderPosition.Y -= 1f - (float)clientBlockType4.VerticalFill * (1f / (float)clientBlockType4.MaxFillLevel);
					}
					ClientBlockType clientBlockType5 = this._gameInstance.MapModule.ClientBlockTypes[this._previousBlockId];
					bool flag19 = clientBlockType5.BlockParticleSetId != null;
					if (flag19)
					{
						ParticleSystemProxy particleSystemProxy3;
						bool flag20 = this._gameInstance.ParticleSystemStoreModule.TrySpawnBlockSystem(renderPosition, clientBlockType5, ClientBlockParticleEvent.MoveOut, out particleSystemProxy3, false, false);
						if (flag20)
						{
							particleSystemProxy3.Position = renderPosition;
							particleSystemProxy3.Rotation = this.RenderOrientation;
						}
					}
					this._previousBlockId = num4;
				}
				this._previousBlockY = (int)renderPosition.Y;
				bool isSprinting = relativeMovementStates.IsSprinting;
				if (isSprinting)
				{
					Vector3 renderPosition2 = this.RenderPosition;
					bool flag21 = relativeMovementStates.IsOnGround && !flag;
					if (flag21)
					{
						renderPosition2.Y -= 0.01f;
					}
					int num5 = this._gameInstance.MapModule.GetBlock(renderPosition2, 0);
					ClientBlockType clientBlockType6 = this._gameInstance.MapModule.ClientBlockTypes[num5];
					int num6 = 0;
					bool flag22 = flag;
					if (flag22)
					{
						bool flag23 = clientBlockType6.FluidBlockId != num5;
						if (flag23)
						{
							num5 = clientBlockType6.FluidBlockId;
							clientBlockType6 = this._gameInstance.MapModule.ClientBlockTypes[num5];
						}
						num6 = 1;
						renderPosition2.Y = (float)Math.Floor((double)renderPosition2.Y);
						while ((float)num6 < this.Hitbox.Max.Y + 1f)
						{
							int block3 = this._gameInstance.MapModule.GetBlock((int)Math.Floor((double)renderPosition2.X), (int)renderPosition2.Y + num6, (int)Math.Floor((double)renderPosition2.Z), 0);
							bool flag24 = block3 == 0;
							if (flag24)
							{
								break;
							}
							ClientBlockType clientBlockType7 = this._gameInstance.MapModule.ClientBlockTypes[block3];
							bool flag25 = num5 != block3;
							if (flag25)
							{
								bool flag26 = clientBlockType7.FluidBlockId == 0;
								if (flag26)
								{
									break;
								}
								bool flag27 = clientBlockType7.FluidBlockId != num5;
								if (flag27)
								{
									num5 = clientBlockType7.FluidBlockId;
									clientBlockType6 = this._gameInstance.MapModule.ClientBlockTypes[num5];
								}
							}
							num6++;
						}
						renderPosition2.Y += (float)num6 - (1f - (float)clientBlockType6.VerticalFill * (1f / (float)clientBlockType6.MaxFillLevel));
					}
					bool flag28 = (float)(num6 - 1) > this.Hitbox.Max.Y;
					bool flag29 = (this._previousSprintBlockId != num5 || this._sprintParticleSystem == null) && !flag28;
					if (flag29)
					{
						bool flag30 = this._sprintParticleSystem != null;
						if (flag30)
						{
							this._sprintParticleSystem.Expire(false);
							this._sprintParticleSystem = null;
						}
						bool flag31 = clientBlockType6.BlockParticleSetId != null;
						if (flag31)
						{
							this._gameInstance.ParticleSystemStoreModule.TrySpawnBlockSystem(renderPosition2, clientBlockType6, ClientBlockParticleEvent.Sprint, out this._sprintParticleSystem, false, true);
						}
						this._previousSprintBlockId = num5;
					}
					bool flag32 = this._sprintParticleSystem != null;
					if (flag32)
					{
						bool flag33 = flag28;
						if (flag33)
						{
							this._sprintParticleSystem.Expire(false);
							this._sprintParticleSystem = null;
						}
						else
						{
							this._sprintParticleSystem.Position = renderPosition2;
							this._sprintParticleSystem.Rotation = this.RenderOrientation;
						}
					}
				}
				else
				{
					bool flag34 = this._sprintParticleSystem != null;
					if (flag34)
					{
						this._sprintParticleSystem.Expire(false);
						this._sprintParticleSystem = null;
					}
				}
				bool isWalking = relativeMovementStates.IsWalking;
				if (isWalking)
				{
					Vector3 renderPosition3 = this.RenderPosition;
					bool flag35 = relativeMovementStates.IsOnGround && !flag;
					if (flag35)
					{
						renderPosition3.Y -= 0.01f;
					}
					int num7 = this._gameInstance.MapModule.GetBlock(renderPosition3, 0);
					ClientBlockType clientBlockType8 = this._gameInstance.MapModule.ClientBlockTypes[num7];
					int num8 = 0;
					bool flag36 = flag;
					if (flag36)
					{
						bool flag37 = clientBlockType8.FluidBlockId != num7;
						if (flag37)
						{
							num7 = clientBlockType8.FluidBlockId;
							clientBlockType8 = this._gameInstance.MapModule.ClientBlockTypes[num7];
						}
						num8 = 1;
						renderPosition3.Y = (float)Math.Floor((double)renderPosition3.Y);
						while ((float)num8 < this.Hitbox.Max.Y + 1f)
						{
							int block4 = this._gameInstance.MapModule.GetBlock((int)Math.Floor((double)renderPosition3.X), (int)renderPosition3.Y + num8, (int)Math.Floor((double)renderPosition3.Z), 0);
							bool flag38 = block4 == 0;
							if (flag38)
							{
								break;
							}
							ClientBlockType clientBlockType9 = this._gameInstance.MapModule.ClientBlockTypes[block4];
							bool flag39 = num7 != block4;
							if (flag39)
							{
								bool flag40 = clientBlockType9.FluidBlockId == 0;
								if (flag40)
								{
									break;
								}
								bool flag41 = clientBlockType9.FluidBlockId != num7;
								if (flag41)
								{
									num7 = clientBlockType9.FluidBlockId;
									clientBlockType8 = this._gameInstance.MapModule.ClientBlockTypes[num7];
								}
							}
							num8++;
						}
						renderPosition3.Y += (float)num8 - (1f - (float)clientBlockType8.VerticalFill * (1f / (float)clientBlockType8.MaxFillLevel));
					}
					bool flag42 = (float)(num8 - 1) > this.Hitbox.Max.Y;
					bool flag43 = (this._previousWalkBlockId != num7 || this._walkParticleSystem == null) && !flag42;
					if (flag43)
					{
						bool flag44 = this._walkParticleSystem != null;
						if (flag44)
						{
							this._walkParticleSystem.Expire(false);
							this._walkParticleSystem = null;
						}
						bool flag45 = clientBlockType8.BlockParticleSetId != null;
						if (flag45)
						{
							this._gameInstance.ParticleSystemStoreModule.TrySpawnBlockSystem(renderPosition3, clientBlockType8, ClientBlockParticleEvent.Walk, out this._walkParticleSystem, false, true);
						}
						this._previousWalkBlockId = num7;
					}
					bool flag46 = this._walkParticleSystem != null;
					if (flag46)
					{
						bool flag47 = flag42;
						if (flag47)
						{
							this._walkParticleSystem.Expire(false);
							this._walkParticleSystem = null;
						}
						else
						{
							this._walkParticleSystem.Position = renderPosition3;
							this._walkParticleSystem.Rotation = this.RenderOrientation;
						}
					}
				}
				else
				{
					bool flag48 = this._walkParticleSystem != null;
					if (flag48)
					{
						this._walkParticleSystem.Expire(false);
						this._walkParticleSystem = null;
					}
				}
				bool flag49 = !relativeMovementStates.IsSprinting && !relativeMovementStates.IsWalking && !relativeMovementStates.IsCrouching && !relativeMovementStates.IsIdle && !relativeMovementStates.IsFalling;
				if (flag49)
				{
					Vector3 renderPosition4 = this.RenderPosition;
					bool flag50 = relativeMovementStates.IsOnGround && !flag;
					if (flag50)
					{
						renderPosition4.Y -= 0.01f;
					}
					int num9 = this._gameInstance.MapModule.GetBlock(renderPosition4, 1);
					ClientBlockType clientBlockType10 = this._gameInstance.MapModule.ClientBlockTypes[num9];
					int num10 = 0;
					bool flag51 = flag;
					if (flag51)
					{
						bool flag52 = clientBlockType10.FluidBlockId != 0;
						if (flag52)
						{
							num9 = clientBlockType10.FluidBlockId;
							clientBlockType10 = this._gameInstance.MapModule.ClientBlockTypes[num9];
						}
						num10 = 1;
						renderPosition4.Y = (float)Math.Floor((double)renderPosition4.Y);
						while ((float)num10 < this.Hitbox.Max.Y + 1f)
						{
							int block5 = this._gameInstance.MapModule.GetBlock((int)Math.Floor((double)renderPosition4.X), (int)renderPosition4.Y + num10, (int)Math.Floor((double)renderPosition4.Z), 0);
							bool flag53 = block5 == 0;
							if (flag53)
							{
								break;
							}
							bool flag54 = num9 != block5;
							if (flag54)
							{
								ClientBlockType clientBlockType11 = this._gameInstance.MapModule.ClientBlockTypes[block5];
								bool flag55 = clientBlockType11.FluidBlockId == 0;
								if (flag55)
								{
									break;
								}
								bool flag56 = clientBlockType11.FluidBlockId != num9;
								if (flag56)
								{
									num9 = clientBlockType11.FluidBlockId;
									clientBlockType10 = this._gameInstance.MapModule.ClientBlockTypes[num9];
								}
							}
							num10++;
						}
						renderPosition4.Y += (float)num10 - (1f - (float)clientBlockType10.VerticalFill * (1f / (float)clientBlockType10.MaxFillLevel));
					}
					bool flag57 = (float)(num10 - 1) > this.Hitbox.Max.Y;
					bool flag58 = (this._previousRunBlockId != num9 || this._runParticleSystem == null) && !flag57;
					if (flag58)
					{
						bool flag59 = this._runParticleSystem != null;
						if (flag59)
						{
							this._runParticleSystem.Expire(false);
							this._runParticleSystem = null;
						}
						bool flag60 = clientBlockType10.BlockParticleSetId != null;
						if (flag60)
						{
							this._gameInstance.ParticleSystemStoreModule.TrySpawnBlockSystem(renderPosition4, clientBlockType10, ClientBlockParticleEvent.Run, out this._runParticleSystem, false, true);
						}
						this._previousRunBlockId = num9;
					}
					bool flag61 = this._runParticleSystem != null;
					if (flag61)
					{
						bool flag62 = flag57;
						if (flag62)
						{
							this._runParticleSystem.Expire(false);
							this._runParticleSystem = null;
						}
						else
						{
							this._runParticleSystem.Position = renderPosition4;
							this._runParticleSystem.Rotation = this.RenderOrientation;
						}
					}
				}
				else
				{
					bool flag63 = this._runParticleSystem != null;
					if (flag63)
					{
						this._runParticleSystem.Expire(false);
						this._runParticleSystem = null;
					}
				}
				bool flag64 = relativeMovementStates.IsFalling && !this._wasFalling;
				if (flag64)
				{
					this._fallHeight = this._nextPosition.Y;
				}
				else
				{
					bool isJumping = relativeMovementStates.IsJumping;
					if (isJumping)
					{
						bool flag65 = !this._wasJumping;
						if (flag65)
						{
							this._fallHeight = 0f;
						}
						this._fallHeight = Math.Max(this._fallHeight, this._nextPosition.Y);
					}
				}
			}
			else
			{
				this.ResetMovementParticleSystems();
			}
			this._wasInFluid = flag;
			this._wasOnGround = relativeMovementStates.IsOnGround;
			this._wasFalling = relativeMovementStates.IsFalling;
			this._wasJumping = relativeMovementStates.IsJumping;
		}

		// Token: 0x0600499C RID: 18844 RVA: 0x00125308 File Offset: 0x00123508
		public void ResetMovementParticleSystems()
		{
			bool flag = this._walkParticleSystem != null;
			if (flag)
			{
				this._walkParticleSystem.Expire(true);
				this._walkParticleSystem = null;
				this._previousWalkBlockId = -1;
			}
			bool flag2 = this._runParticleSystem != null;
			if (flag2)
			{
				this._runParticleSystem.Expire(true);
				this._runParticleSystem = null;
				this._previousRunBlockId = -1;
			}
			bool flag3 = this._sprintParticleSystem != null;
			if (flag3)
			{
				this._sprintParticleSystem.Expire(true);
				this._sprintParticleSystem = null;
				this._previousSprintBlockId = -1;
			}
		}

		// Token: 0x0600499D RID: 18845 RVA: 0x00125394 File Offset: 0x00123594
		public virtual ref ClientMovementStates GetRelativeMovementStates()
		{
			return ref this.ServerMovementStates;
		}

		// Token: 0x0600499E RID: 18846 RVA: 0x001253AC File Offset: 0x001235AC
		public EntityAnimation GetAnimation(string id)
		{
			ClientAnimationSet clientAnimationSet;
			bool flag = this._animationSets.TryGetValue(id, out clientAnimationSet);
			EntityAnimation result;
			if (flag)
			{
				bool flag2 = Entity.JumpAnimations.Contains(id);
				if (flag2)
				{
					result = clientAnimationSet.Animations[this._currentJumpAnimation];
				}
				else
				{
					result = clientAnimationSet.GetWeightedAnimation(this.GetEntitySeed());
				}
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600499F RID: 18847 RVA: 0x00125404 File Offset: 0x00123604
		public List<string> GetAnimationList(AnimationSlot slot)
		{
			List<string> animations = new List<string>();
			if (slot > 2)
			{
				if (slot - 3 <= 1)
				{
					this._gameInstance.App.CharacterPartStore.Emotes.ForEach(delegate(Emote k)
					{
						animations.Add(k.Id);
					});
				}
			}
			else
			{
				animations.AddRange(this._animationSets.Keys);
			}
			return animations;
		}

		// Token: 0x060049A0 RID: 18848 RVA: 0x00125484 File Offset: 0x00123684
		private Animation GetWeightedAnimation(Animation[] animations)
		{
			bool flag = animations.Length == 1;
			Animation result;
			if (flag)
			{
				result = animations[0];
			}
			else
			{
				float num = 0f;
				foreach (Animation animation in animations)
				{
					num += animation.Weight;
				}
				bool flag2 = num == 0f;
				if (flag2)
				{
					result = animations[this.GetEntitySeed() % animations.Length];
				}
				else
				{
					float num2 = StaticRandom.NextFloat((long)this.GetEntitySeed(), 0f, num);
					Animation animation2 = null;
					foreach (Animation animation3 in animations)
					{
						animation2 = animation3;
						num2 -= animation3.Weight;
						bool flag3 = num2 <= 0f;
						if (flag3)
						{
							break;
						}
					}
					result = animation2;
				}
			}
			return result;
		}

		// Token: 0x060049A1 RID: 18849 RVA: 0x00125550 File Offset: 0x00123750
		private bool DoFirstPersonParticles()
		{
			return this.NetworkId == this._gameInstance.LocalPlayerNetworkId && this._gameInstance.CameraModule.Controller.IsFirstPerson;
		}

		// Token: 0x060049A2 RID: 18850 RVA: 0x00125590 File Offset: 0x00123790
		public void UpdateInterpolation(float timeFraction)
		{
			this.RenderPosition = Vector3.Lerp(this._previousPosition, this._nextPosition, timeFraction) + new Vector3(0f, this._gameInstance.CharacterControllerModule.MovementController.AutoJumpHeightShift, 0f);
		}

		// Token: 0x170011D7 RID: 4567
		// (get) Token: 0x060049A3 RID: 18851 RVA: 0x001255DF File Offset: 0x001237DF
		// (set) Token: 0x060049A4 RID: 18852 RVA: 0x001255E7 File Offset: 0x001237E7
		public float SmoothHealth { get; set; } = -1f;

		// Token: 0x060049A5 RID: 18853 RVA: 0x001255F0 File Offset: 0x001237F0
		public void UpdateEntityStats(Dictionary<int, EntityStatUpdate[]> updates)
		{
			foreach (KeyValuePair<int, EntityStatUpdate[]> keyValuePair in updates)
			{
				int key = keyValuePair.Key;
				ClientEntityStatValue clientEntityStatValue = this._entityStats[key];
				float? previousValue = (clientEntityStatValue != null) ? new float?(clientEntityStatValue.Value) : null;
				for (int i = 0; i < keyValuePair.Value.Length; i++)
				{
					EntityStatUpdate update = keyValuePair.Value[i];
					this.TryAddServerStat(key, update);
				}
				clientEntityStatValue = this._entityStats[key];
				bool flag = clientEntityStatValue != null;
				if (flag)
				{
					this.EntityStatUpdated(key, previousValue, clientEntityStatValue);
				}
			}
		}

		// Token: 0x060049A6 RID: 18854 RVA: 0x001256C4 File Offset: 0x001238C4
		private void UpdateStats()
		{
			int num = this._serverStats.FindIndex((Entity.TimedStatUpdate v) => DateTime.Now.Ticks / 10000L > v.CreationTime + this._gameInstance.TimeModule.StatTimeoutThreshold);
			bool flag = num != -1;
			if (flag)
			{
				this._serverStats.RemoveRange(0, num + 1);
			}
			num = this._predictedStats.FindIndex((Entity.TimedStatUpdate v) => DateTime.Now.Ticks / 10000L > v.CreationTime + this._gameInstance.TimeModule.StatTimeoutThreshold);
			bool flag2 = num != -1;
			if (flag2)
			{
				HashSet<int> hashSet = new HashSet<int>();
				for (int i = 0; i <= num; i++)
				{
					Entity.Logger.Warn(string.Format("Removing mis-prediction: {0}", this._predictedStats[i].Update));
					hashSet.Add(this._predictedStats[i].Index);
				}
				this._predictedStats.RemoveRange(0, num + 1);
				foreach (int num2 in hashSet)
				{
					float value = this._entityStats[num2].Value;
					this.ReapplyPredictions(num2);
					this.EntityStatUpdated(num2, new float?(value), this._entityStats[num2]);
				}
			}
		}

		// Token: 0x060049A7 RID: 18855 RVA: 0x00125814 File Offset: 0x00123A14
		public bool GetStatModifier(int index, string key, out Modifier modifier)
		{
			modifier = null;
			ClientEntityStatValue entityStat = this.GetEntityStat(index);
			bool flag = entityStat == null;
			bool result;
			if (flag)
			{
				Entity.Logger.Warn(string.Format("No EntityStatValue found for index: {0}", index));
				result = false;
			}
			else
			{
				Dictionary<string, Modifier> modifiers = entityStat.Modifiers;
				result = (modifiers != null && modifiers.TryGetValue(key, out modifier));
			}
			return result;
		}

		// Token: 0x060049A8 RID: 18856 RVA: 0x00125870 File Offset: 0x00123A70
		public EntityStatUpdate PutStatModifier(int index, string key, Modifier modifier)
		{
			ClientEntityStatValue entityStat = this.GetEntityStat(index);
			bool flag = entityStat == null;
			EntityStatUpdate result;
			if (flag)
			{
				Entity.Logger.Warn(string.Format("No EntityStatValue found for index: {0}", index));
				result = null;
			}
			else
			{
				EntityStatUpdate entityStatUpdate = new EntityStatUpdate(2, true, 0f, null, key, modifier);
				this.TryAddPredictedStat(index, entityStatUpdate);
				result = entityStatUpdate;
			}
			return result;
		}

		// Token: 0x060049A9 RID: 18857 RVA: 0x001258CC File Offset: 0x00123ACC
		public bool RemoveModifier(int index, string key, out EntityStatUpdate update)
		{
			ClientEntityStatValue entityStat = this.GetEntityStat(index);
			bool flag = entityStat == null;
			bool result;
			if (flag)
			{
				Entity.Logger.Warn(string.Format("No EntityStatValue found for index: {0}", index));
				update = null;
				result = false;
			}
			else
			{
				Dictionary<string, Modifier> modifiers = entityStat.Modifiers;
				bool flag2 = modifiers != null && modifiers.ContainsKey(key);
				bool flag3 = !flag2;
				if (flag3)
				{
					update = null;
					result = false;
				}
				else
				{
					update = new EntityStatUpdate(3, true, 0f, null, key, null);
					this.TryAddPredictedStat(index, update);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060049AA RID: 18858 RVA: 0x00125954 File Offset: 0x00123B54
		public EntityStatUpdate SetStatValue(int index, float newValue)
		{
			ClientEntityStatValue entityStat = this.GetEntityStat(index);
			bool flag = entityStat == null;
			EntityStatUpdate result;
			if (flag)
			{
				Entity.Logger.Warn(string.Format("No EntityStatValue found for index: {0}", index));
				result = null;
			}
			else
			{
				EntityStatUpdate entityStatUpdate = new EntityStatUpdate(5, true, newValue, null, null, null);
				this.TryAddPredictedStat(index, entityStatUpdate);
				result = entityStatUpdate;
			}
			return result;
		}

		// Token: 0x060049AB RID: 18859 RVA: 0x001259AC File Offset: 0x00123BAC
		public EntityStatUpdate AddStatValue(int index, float amount)
		{
			ClientEntityStatValue entityStat = this.GetEntityStat(index);
			bool flag = entityStat == null;
			EntityStatUpdate result;
			if (flag)
			{
				Entity.Logger.Warn(string.Format("No EntityStatValue found for index: {0}", index));
				result = null;
			}
			else
			{
				EntityStatUpdate entityStatUpdate = new EntityStatUpdate(4, true, amount, null, null, null);
				this.TryAddPredictedStat(index, entityStatUpdate);
				result = entityStatUpdate;
			}
			return result;
		}

		// Token: 0x060049AC RID: 18860 RVA: 0x00125A04 File Offset: 0x00123C04
		public EntityStatUpdate SubtractStatValue(int index, float amount)
		{
			return this.AddStatValue(index, -amount);
		}

		// Token: 0x060049AD RID: 18861 RVA: 0x00125A20 File Offset: 0x00123C20
		public EntityStatUpdate MinimizeStatValue(int index)
		{
			ClientEntityStatValue entityStat = this.GetEntityStat(index);
			bool flag = entityStat == null;
			EntityStatUpdate result;
			if (flag)
			{
				Entity.Logger.Warn(string.Format("No EntityStatValue found for index: {0}", index));
				result = null;
			}
			else
			{
				EntityStatUpdate entityStatUpdate = new EntityStatUpdate(6, true, 0f, null, null, null);
				this.TryAddPredictedStat(index, entityStatUpdate);
				result = entityStatUpdate;
			}
			return result;
		}

		// Token: 0x060049AE RID: 18862 RVA: 0x00125A7C File Offset: 0x00123C7C
		public EntityStatUpdate MaximizeStatValue(int index)
		{
			ClientEntityStatValue entityStat = this.GetEntityStat(index);
			bool flag = entityStat == null;
			EntityStatUpdate result;
			if (flag)
			{
				Entity.Logger.Warn(string.Format("No EntityStatValue found for index: {0}", index));
				result = null;
			}
			else
			{
				EntityStatUpdate entityStatUpdate = new EntityStatUpdate(7, true, 0f, null, null, null);
				this.TryAddPredictedStat(index, entityStatUpdate);
				result = entityStatUpdate;
			}
			return result;
		}

		// Token: 0x060049AF RID: 18863 RVA: 0x00125AD8 File Offset: 0x00123CD8
		public EntityStatUpdate ResetStatValue(int index)
		{
			ClientEntityStatValue entityStat = this.GetEntityStat(index);
			bool flag = entityStat == null;
			EntityStatUpdate result;
			if (flag)
			{
				Entity.Logger.Warn(string.Format("No EntityStatValue found for index: {0}", index));
				result = null;
			}
			else
			{
				EntityStatUpdate entityStatUpdate = new EntityStatUpdate(8, true, 0f, null, null, null);
				this.TryAddPredictedStat(index, entityStatUpdate);
				result = entityStatUpdate;
			}
			return result;
		}

		// Token: 0x060049B0 RID: 18864 RVA: 0x00125B34 File Offset: 0x00123D34
		public void CancelStatPrediction(int index, EntityStatUpdate update)
		{
			ClientEntityStatValue clientEntityStatValue = this._entityStats[index];
			float? previousValue = (clientEntityStatValue != null) ? new float?(clientEntityStatValue.Value) : null;
			for (int i = 0; i < this._predictedStats.Count; i++)
			{
				Entity.TimedStatUpdate timedStatUpdate = this._predictedStats[i];
				bool flag = timedStatUpdate.Index != index || !timedStatUpdate.Update.Equals(update);
				if (!flag)
				{
					this._predictedStats.RemoveAt(i);
					this.ReapplyPredictions(index);
					break;
				}
			}
			clientEntityStatValue = this._entityStats[index];
			bool flag2 = clientEntityStatValue != null;
			if (flag2)
			{
				this.EntityStatUpdated(index, previousValue, clientEntityStatValue);
			}
		}

		// Token: 0x060049B1 RID: 18865 RVA: 0x00125BEC File Offset: 0x00123DEC
		private void ApplyStatUpdate(ClientEntityStatValue[] stats, int index, EntityStatUpdate update)
		{
			ClientEntityStatValue clientEntityStatValue = stats[index];
			ClientEntityStatType clientEntityStatType = this._gameInstance.ServerSettings.EntityStatTypes[index];
			bool flag = clientEntityStatValue == null && update.Op > 0;
			if (flag)
			{
				Entity.Logger.Error(string.Format("Attempted to access null entity stat {0}", index));
			}
			else
			{
				switch (update.Op)
				{
				case 0:
				{
					ClientEntityStatValue clientEntityStatValue2 = new ClientEntityStatValue();
					clientEntityStatValue2.Min = clientEntityStatType.Min;
					clientEntityStatValue2.Max = clientEntityStatType.Max;
					clientEntityStatValue2.Value = update.Value;
					clientEntityStatValue2.Modifiers = update.Modifiers;
					ClientEntityStatValue clientEntityStatValue3 = clientEntityStatValue2;
					stats[index] = clientEntityStatValue2;
					clientEntityStatValue = clientEntityStatValue3;
					clientEntityStatValue.CalculateModifiers(clientEntityStatType);
					clientEntityStatValue.Value = update.Value;
					break;
				}
				case 1:
					stats[index] = null;
					break;
				case 2:
					clientEntityStatValue.Modifiers = (clientEntityStatValue.Modifiers ?? new Dictionary<string, Modifier>());
					clientEntityStatValue.Modifiers[update.ModifierKey] = update.Modifier_;
					clientEntityStatValue.CalculateModifiers(clientEntityStatType);
					break;
				case 3:
				{
					Dictionary<string, Modifier> modifiers = clientEntityStatValue.Modifiers;
					if (modifiers != null)
					{
						modifiers.Remove(update.ModifierKey);
					}
					clientEntityStatValue.CalculateModifiers(clientEntityStatType);
					break;
				}
				case 4:
					clientEntityStatValue.Value += update.Value;
					break;
				case 5:
					clientEntityStatValue.Value = update.Value;
					break;
				case 6:
					clientEntityStatValue.Value = clientEntityStatValue.Min;
					break;
				case 7:
					clientEntityStatValue.Value = clientEntityStatValue.Max;
					break;
				case 8:
					clientEntityStatValue.Value = clientEntityStatType.Value;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		// Token: 0x060049B2 RID: 18866 RVA: 0x00125D9C File Offset: 0x00123F9C
		private void TryAddPredictedStat(int index, EntityStatUpdate update)
		{
			for (int i = 0; i < this._serverStats.Count; i++)
			{
				Entity.TimedStatUpdate timedStatUpdate = this._serverStats[i];
				bool flag = timedStatUpdate.Index != index || !timedStatUpdate.Update.Equals(update);
				if (!flag)
				{
					this._serverStats.RemoveAt(i);
					return;
				}
			}
			ClientEntityStatValue clientEntityStatValue = this._entityStats[index];
			float? previousValue = (clientEntityStatValue != null) ? new float?(clientEntityStatValue.Value) : null;
			this.ApplyStatUpdate(this._entityStats, index, update);
			this._predictedStats.Add(new Entity.TimedStatUpdate
			{
				CreationTime = DateTime.Now.Ticks / 10000L,
				Index = index,
				Update = update
			});
			clientEntityStatValue = this._entityStats[index];
			bool flag2 = clientEntityStatValue != null;
			if (flag2)
			{
				this.EntityStatUpdated(index, previousValue, clientEntityStatValue);
				return;
			}
		}

		// Token: 0x060049B3 RID: 18867 RVA: 0x00125E9C File Offset: 0x0012409C
		private void TryAddServerStat(int index, EntityStatUpdate update)
		{
			this.ApplyStatUpdate(this._serverEntityStats, index, update);
			bool predictable = update.Predictable;
			if (predictable)
			{
				for (int i = 0; i < this._predictedStats.Count; i++)
				{
					Entity.TimedStatUpdate timedStatUpdate = this._predictedStats[i];
					bool flag = timedStatUpdate.Index != index || !timedStatUpdate.Update.Equals(update);
					if (!flag)
					{
						this._predictedStats.RemoveAt(i);
						this.ReapplyPredictions(index);
						return;
					}
				}
			}
			this.ReapplyPredictions(index);
			bool predictable2 = update.Predictable;
			if (predictable2)
			{
				this._serverStats.Add(new Entity.TimedStatUpdate
				{
					CreationTime = DateTime.Now.Ticks / 10000L,
					Index = index,
					Update = update
				});
			}
		}

		// Token: 0x060049B4 RID: 18868 RVA: 0x00125F80 File Offset: 0x00124180
		private void ReapplyPredictions(int index)
		{
			ClientEntityStatValue clientEntityStatValue = this._entityStats[index];
			ClientEntityStatValue clientEntityStatValue2 = this._serverEntityStats[index];
			bool flag = clientEntityStatValue == null && clientEntityStatValue2 != null;
			if (flag)
			{
				clientEntityStatValue = (this._entityStats[index] = new ClientEntityStatValue());
			}
			else
			{
				bool flag2 = clientEntityStatValue != null && clientEntityStatValue2 == null;
				if (flag2)
				{
					this._entityStats[index] = null;
					return;
				}
			}
			Dictionary<string, Modifier> modifiers = clientEntityStatValue.Modifiers;
			if (modifiers != null)
			{
				modifiers.Clear();
			}
			bool flag3 = clientEntityStatValue.Modifiers == null && clientEntityStatValue2.Modifiers != null;
			if (flag3)
			{
				clientEntityStatValue.Modifiers = new Dictionary<string, Modifier>();
			}
			bool flag4 = clientEntityStatValue2.Modifiers != null;
			if (flag4)
			{
				foreach (KeyValuePair<string, Modifier> keyValuePair in clientEntityStatValue2.Modifiers)
				{
					clientEntityStatValue.Modifiers.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
			clientEntityStatValue.Max = clientEntityStatValue2.Max;
			clientEntityStatValue.Min = clientEntityStatValue2.Min;
			clientEntityStatValue.Value = clientEntityStatValue2.Value;
			for (int i = 0; i < this._predictedStats.Count; i++)
			{
				Entity.TimedStatUpdate timedStatUpdate = this._predictedStats[i];
				bool flag5 = timedStatUpdate.Index != index;
				if (!flag5)
				{
					this.ApplyStatUpdate(this._entityStats, index, timedStatUpdate.Update);
				}
			}
		}

		// Token: 0x060049B5 RID: 18869 RVA: 0x0012610C File Offset: 0x0012430C
		private void EntityStatUpdated(int entityStatIndex, float? previousValue, ClientEntityStatValue value)
		{
			bool flag = entityStatIndex == DefaultEntityStats.Health;
			if (flag)
			{
				float smoothHealth = value.Value / value.Max;
				bool flag2 = value.Value <= value.Min || this.SmoothHealth == -1f;
				if (flag2)
				{
					this.SmoothHealth = smoothHealth;
				}
			}
			this.HandleItemConditionAppearanceForEntityStat(this.PrimaryItem, 0, entityStatIndex);
			this.HandleItemConditionAppearanceForEntityStat(this.SecondaryItem, (this.PrimaryItem != null) ? 1 : 0, entityStatIndex);
			bool flag3 = this.NetworkId != this._gameInstance.LocalPlayerNetworkId;
			if (!flag3)
			{
				ClientEntityStatType clientEntityStatType = this._gameInstance.ServerSettings.EntityStatTypes[entityStatIndex];
				bool flag4 = Entity.TestMaxValueEffects(value, previousValue, clientEntityStatType.MaxValueEffects);
				if (flag4)
				{
					this.RunEntityStatEffects(clientEntityStatType.MaxValueEffects);
				}
				bool flag5 = Entity.TestMinValueEffects(value, previousValue, clientEntityStatType.MinValueEffects);
				if (flag5)
				{
					this.RunEntityStatEffects(clientEntityStatType.MinValueEffects);
				}
				bool flag6 = entityStatIndex == DefaultEntityStats.Health && previousValue != null;
				if (flag6)
				{
					float? num = previousValue;
					float min = value.Min;
					bool flag7 = num.GetValueOrDefault() <= min & num != null;
					if (flag7)
					{
						for (int i = 0; i < 9; i++)
						{
							this.ModelRenderer.SetSlotAnimationNoBlending(i, null, true, 1f, 0f);
						}
						this.SetMovementAnimation("Idle", 0f, true, true);
					}
					else
					{
						float value2 = value.Value;
						num = previousValue;
						bool flag8 = (value2 < num.GetValueOrDefault() & num != null) && value.Max > value.Value;
						if (flag8)
						{
							float alpha = MathHelper.Min(0.1f + (previousValue - value.Value).Value / value.Max * 2f, 1f);
							this._gameInstance.DamageEffectModule.IncreaseDamageEffect(alpha);
						}
					}
					bool flag9 = this.IsDead(true);
					if (flag9)
					{
						this._gameInstance.App.Interface.InGameView.Wielding = false;
					}
				}
				this._gameInstance.App.Interface.InGameView.OnStatChanged(entityStatIndex, value, previousValue);
			}
		}

		// Token: 0x060049B6 RID: 18870 RVA: 0x0012637C File Offset: 0x0012457C
		private void RunEntityStatEffects(EntityStatEffects effects)
		{
			uint networkWwiseId = ResourceManager.GetNetworkWwiseId(effects.SoundEventIndex);
			this._gameInstance.AudioModule.PlayLocalSoundEvent(networkWwiseId);
			bool flag = effects.Particles == null || effects.Particles.Length == 0;
			if (!flag)
			{
				foreach (ModelParticle networkParticle in effects.Particles)
				{
					ModelParticleSettings particle = new ModelParticleSettings("");
					ParticleProtocolInitializer.Initialize(networkParticle, ref particle, this._gameInstance.EntityStoreModule.NodeNameManager);
					this.AttachParticles(this._characterModel, this._entityParticles, particle, this.Scale);
				}
			}
		}

		// Token: 0x060049B7 RID: 18871 RVA: 0x00126424 File Offset: 0x00124624
		private void HandleItemConditionAppearanceForAllEntityStats(ClientItemBase item, int itemEntityIndex)
		{
			for (int i = 0; i < this._entityStats.Length; i++)
			{
				bool flag = this.HandleItemConditionAppearanceForEntityStat(item, itemEntityIndex, i);
				bool flag2 = flag;
				if (flag2)
				{
					break;
				}
			}
		}

		// Token: 0x060049B8 RID: 18872 RVA: 0x0012645C File Offset: 0x0012465C
		private bool HandleItemConditionAppearanceForEntityStat(ClientItemBase item, int itemEntityIndex, int entityStatIndex)
		{
			Dictionary<int, ClientItemAppearanceCondition[]> dictionary = (item != null) ? item.ItemAppearanceConditions : null;
			ClientItemAppearanceCondition[] array;
			bool flag = dictionary == null || !dictionary.TryGetValue(entityStatIndex, out array);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				ClientEntityStatValue clientEntityStatValue = this._entityStats[entityStatIndex];
				bool flag2 = clientEntityStatValue == null;
				if (flag2)
				{
					result = false;
				}
				else
				{
					Entity.EntityItem entityItem = this.EntityItems[itemEntityIndex];
					bool flag3 = entityItem.CurrentItemAppearanceCondition != null && entityItem.CurrentItemAppearanceCondition.EntityStatIndex == entityStatIndex;
					if (flag3)
					{
						this.CheckItemAppearanceConditionsToRemove(item, itemEntityIndex, clientEntityStatValue, array);
					}
					bool flag4 = entityItem.CurrentItemAppearanceCondition != null;
					result = (!flag4 && this.CheckItemAppearanceConditionsToApply(item, itemEntityIndex, entityStatIndex, clientEntityStatValue, array));
				}
			}
			return result;
		}

		// Token: 0x060049B9 RID: 18873 RVA: 0x0012650C File Offset: 0x0012470C
		private void CheckItemAppearanceConditionsToRemove(ClientItemBase item, int itemEntityIndex, ClientEntityStatValue entityStatValue, ClientItemAppearanceCondition[] itemAppearanceConditions)
		{
			ClientItemAppearanceCondition.Data currentItemAppearanceCondition = this.EntityItems[itemEntityIndex].CurrentItemAppearanceCondition;
			ClientItemAppearanceCondition clientItemAppearanceCondition = itemAppearanceConditions[currentItemAppearanceCondition.ConditionIndex];
			bool flag = clientItemAppearanceCondition.CanApplyCondition(entityStatValue);
			if (!flag)
			{
				bool flag2 = item.Model != null;
				if (flag2)
				{
					this.EntityItems[itemEntityIndex].ModelRenderer.Dispose();
					this.EntityItems[itemEntityIndex].ModelRenderer = new ModelRenderer(item.Model, this._gameInstance.AtlasSizes, this._gameInstance.Engine.Graphics, this._gameInstance.FrameCounter, false);
					this.EntityItems[itemEntityIndex].ModelRenderer.SetSlotAnimation(0, item.Animation, true, 1f, 0f, 0f, null, false);
				}
				bool flag3 = currentItemAppearanceCondition.EntityParticles != null;
				if (flag3)
				{
					for (int i = 0; i < currentItemAppearanceCondition.EntityParticles.Length; i++)
					{
						Entity.EntityParticle entityParticle = currentItemAppearanceCondition.EntityParticles[i];
						if (entityParticle != null)
						{
							ParticleSystemProxy particleSystemProxy = entityParticle.ParticleSystemProxy;
							if (particleSystemProxy != null)
							{
								particleSystemProxy.Expire(false);
							}
						}
						this.EntityItems[itemEntityIndex].Particles.Remove(entityParticle);
					}
				}
				this.EntityItems[itemEntityIndex].ModelVFX.Id = null;
				this.EntityItems[itemEntityIndex].CurrentItemAppearanceCondition = null;
			}
		}

		// Token: 0x060049BA RID: 18874 RVA: 0x0012667C File Offset: 0x0012487C
		private bool CheckItemAppearanceConditionsToApply(ClientItemBase item, int itemEntityIndex, int entityStatIndex, ClientEntityStatValue entityStatValue, ClientItemAppearanceCondition[] itemAppearanceConditionsForStat)
		{
			for (int i = 0; i < itemAppearanceConditionsForStat.Length; i++)
			{
				ClientItemAppearanceCondition clientItemAppearanceCondition = itemAppearanceConditionsForStat[i];
				bool flag = !clientItemAppearanceCondition.CanApplyCondition(entityStatValue);
				if (!flag)
				{
					ClientItemAppearanceCondition.Data data = new ClientItemAppearanceCondition.Data(entityStatIndex, i);
					bool flag2 = clientItemAppearanceCondition.ModelId != null || clientItemAppearanceCondition.Texture != null;
					if (flag2)
					{
						this.ApplyItemAppearanceConditionModel(item, itemEntityIndex, clientItemAppearanceCondition);
					}
					bool flag3 = this.NetworkId == this._gameInstance.LocalPlayerNetworkId && this._gameInstance.CameraModule.Controller.IsFirstPerson && clientItemAppearanceCondition.FirstPersonParticles != null;
					ModelParticleSettings[] array = flag3 ? clientItemAppearanceCondition.FirstPersonParticles : clientItemAppearanceCondition.Particles;
					bool flag4 = array != null;
					if (flag4)
					{
						this.ApplyItemAppearanceConditionParticles(item, itemEntityIndex, clientItemAppearanceCondition, data, flag3);
					}
					bool flag5 = clientItemAppearanceCondition.ModelVFXId != null;
					if (flag5)
					{
						int num;
						bool flag6 = !this._gameInstance.EntityStoreModule.ModelVFXByIds.TryGetValue(clientItemAppearanceCondition.ModelVFXId, out num);
						if (flag6)
						{
							this._gameInstance.App.DevTools.Error("Could not find model vfx: " + clientItemAppearanceCondition.ModelVFXId);
						}
						else
						{
							ModelVFX protocolModelVFX = this._gameInstance.EntityStoreModule.ModelVFXs[num];
							this.ApplyItemAppearanceConditionModelVFX(itemEntityIndex, clientItemAppearanceCondition, protocolModelVFX);
						}
					}
					this.EntityItems[itemEntityIndex].CurrentItemAppearanceCondition = data;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060049BB RID: 18875 RVA: 0x001267F4 File Offset: 0x001249F4
		private void ApplyItemAppearanceConditionModel(ClientItemBase item, int itemEntityIndex, ClientItemAppearanceCondition condition)
		{
			this.EntityItems[itemEntityIndex].ModelRenderer.Dispose();
			this.EntityItems[itemEntityIndex].ModelRenderer = new ModelRenderer(condition.Model, this._gameInstance.AtlasSizes, this._gameInstance.Engine.Graphics, this._gameInstance.FrameCounter, false);
			this.EntityItems[itemEntityIndex].ModelRenderer.SetSlotAnimation(0, item.Animation, true, 1f, 0f, 0f, null, false);
		}

		// Token: 0x060049BC RID: 18876 RVA: 0x0012688C File Offset: 0x00124A8C
		protected virtual void ApplyItemAppearanceConditionParticles(ClientItemBase item, int itemEntityIndex, ClientItemAppearanceCondition condition, ClientItemAppearanceCondition.Data data, bool firstPerson)
		{
			ModelParticleSettings[] array = firstPerson ? condition.FirstPersonParticles : condition.Particles;
			data.EntityParticles = new Entity.EntityParticle[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				Entity.EntityParticle entityParticle = this.AttachParticles(item.Model, this.EntityItems[itemEntityIndex].Particles, array[i], this.Scale * this.EntityItems[itemEntityIndex].Scale);
				data.EntityParticles[i] = entityParticle;
			}
		}

		// Token: 0x060049BD RID: 18877 RVA: 0x00126914 File Offset: 0x00124B14
		private void ApplyItemAppearanceConditionModelVFX(int itemEntityIndex, ClientItemAppearanceCondition condition, ModelVFX protocolModelVFX)
		{
			this.EntityItems[itemEntityIndex].ModelVFX.Id = condition.ModelVFXId;
			bool flag = protocolModelVFX.HighlightColor != null;
			bool flag2 = protocolModelVFX.AnimationDuration > 0f;
			bool flag3 = flag;
			if (flag3)
			{
				this.EntityItems[itemEntityIndex].ModelVFX.HighlightColor = new Vector3((float)((byte)protocolModelVFX.HighlightColor.Red) / 255f, (float)((byte)protocolModelVFX.HighlightColor.Green) / 255f, (float)((byte)protocolModelVFX.HighlightColor.Blue) / 255f);
			}
			bool flag4 = flag2;
			if (flag4)
			{
				this.EntityItems[itemEntityIndex].ModelVFX.AnimationDuration = protocolModelVFX.AnimationDuration;
			}
			bool flag5 = protocolModelVFX.AnimationRange != null;
			if (flag5)
			{
				this.EntityItems[itemEntityIndex].ModelVFX.AnimationRange = new Vector2(protocolModelVFX.AnimationRange.X, protocolModelVFX.AnimationRange.Y);
			}
			this.EntityItems[itemEntityIndex].ModelVFX.LoopOption = protocolModelVFX.LoopOption_;
			switch (protocolModelVFX.CurveType_)
			{
			case 0:
				this.EntityItems[itemEntityIndex].ModelVFX.CurveType = Easing.EasingType.Linear;
				break;
			case 1:
				this.EntityItems[itemEntityIndex].ModelVFX.CurveType = Easing.EasingType.QuartIn;
				break;
			case 2:
				this.EntityItems[itemEntityIndex].ModelVFX.CurveType = Easing.EasingType.QuartOut;
				break;
			case 3:
				this.EntityItems[itemEntityIndex].ModelVFX.CurveType = Easing.EasingType.QuartInOut;
				break;
			}
			this.EntityItems[itemEntityIndex].ModelVFX.HighlightThickness = protocolModelVFX.HighlightThickness;
			bool flag6 = protocolModelVFX.NoiseScale != null;
			if (flag6)
			{
				this.EntityItems[itemEntityIndex].ModelVFX.NoiseScale = new Vector2(protocolModelVFX.NoiseScale.X, protocolModelVFX.NoiseScale.Y);
			}
			bool flag7 = protocolModelVFX.NoiseScrollSpeed != null;
			if (flag7)
			{
				this.EntityItems[itemEntityIndex].ModelVFX.NoiseScrollSpeed = new Vector2(protocolModelVFX.NoiseScrollSpeed.X, protocolModelVFX.NoiseScrollSpeed.Y);
			}
			bool flag8 = protocolModelVFX.PostColor != null;
			bool flag9 = flag8;
			if (flag9)
			{
				this.EntityItems[itemEntityIndex].ModelVFX.PostColor = new Vector4((float)((byte)protocolModelVFX.PostColor.Red) / 255f, (float)((byte)protocolModelVFX.PostColor.Green) / 255f, (float)((byte)protocolModelVFX.PostColor.Blue) / 255f, protocolModelVFX.PostColorOpacity);
			}
			ClientModelVFX.EffectDirections effectDirection_ = protocolModelVFX.EffectDirection_;
			SwitchTo switchTo_ = protocolModelVFX.SwitchTo_;
			int useBloom = protocolModelVFX.UseBloomOnHighlight ? 1 : 0;
			int useProgressiveHighlight = protocolModelVFX.UseProgessiveHighlight ? 1 : 0;
			this.EntityItems[itemEntityIndex].ModelVFX.PackedModelVFXParams = Entity.UniqueEntityEffect.PackModelVFXData((int)effectDirection_, switchTo_, useBloom, useProgressiveHighlight);
			bool flag10 = flag && flag2;
			if (flag10)
			{
				this.EntityItems[itemEntityIndex].ModelVFX.TriggerAnimation = true;
			}
		}

		// Token: 0x060049BE RID: 18878 RVA: 0x00126C40 File Offset: 0x00124E40
		public ClientEntityStatValue GetEntityStat(int entityStatIndex)
		{
			bool flag = entityStatIndex < 0 || entityStatIndex >= this._entityStats.Length;
			ClientEntityStatValue result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = this._entityStats[entityStatIndex];
			}
			return result;
		}

		// Token: 0x060049BF RID: 18879 RVA: 0x00126C78 File Offset: 0x00124E78
		private static bool TestMaxValueEffects(ClientEntityStatValue value, float? previousValue, EntityStatEffects effects)
		{
			bool flag = effects == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = effects.TriggerAtZero && value.Min > 0f;
				if (flag2)
				{
					float? num = previousValue;
					float num2 = 0f;
					result = ((num.GetValueOrDefault() < num2 & num != null) && value.Value >= 0f);
				}
				else
				{
					float? num = previousValue;
					float num2 = value.Max;
					result = (!(num.GetValueOrDefault() == num2 & num != null) && value.Value == value.Max);
				}
			}
			return result;
		}

		// Token: 0x060049C0 RID: 18880 RVA: 0x00126D1C File Offset: 0x00124F1C
		private static bool TestMinValueEffects(ClientEntityStatValue value, float? previousValue, EntityStatEffects effects)
		{
			bool flag = effects == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = effects.TriggerAtZero && value.Min < 0f;
				if (flag2)
				{
					float? num = previousValue;
					float num2 = 0f;
					result = ((num.GetValueOrDefault() > num2 & num != null) && value.Value <= 0f);
				}
				else
				{
					float? num = previousValue;
					float num2 = value.Min;
					result = (!(num.GetValueOrDefault() == num2 & num != null) && value.Value == value.Min);
				}
			}
			return result;
		}

		// Token: 0x060049C1 RID: 18881 RVA: 0x00126DC0 File Offset: 0x00124FC0
		public bool TryGetUIComponent(int id, out ClientEntityUIComponent component)
		{
			component = null;
			ClientEntityUIComponent[] entityUIComponents = this._gameInstance.ServerSettings.EntityUIComponents;
			bool flag = entityUIComponents == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = id > entityUIComponents.Length - 1;
				if (flag2)
				{
					throw new ArgumentOutOfRangeException();
				}
				component = entityUIComponents[id];
				result = true;
			}
			return result;
		}

		// Token: 0x060049C2 RID: 18882 RVA: 0x00126E0A File Offset: 0x0012500A
		public void SetUIComponents(int[] components)
		{
			this.UIComponents = components;
		}

		// Token: 0x060049C3 RID: 18883 RVA: 0x00126E14 File Offset: 0x00125014
		public void ClearUIComponents()
		{
			this.UIComponents = Array.Empty<int>();
		}

		// Token: 0x04002518 RID: 9496
		private const int CombatTextsDefaultSize = 10;

		// Token: 0x04002519 RID: 9497
		private const int CombatTextsGrowth = 10;

		// Token: 0x0400251A RID: 9498
		public Entity.CombatText[] CombatTexts = new Entity.CombatText[10];

		// Token: 0x0400251C RID: 9500
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400251D RID: 9501
		public const byte MapAtlasIndex = 0;

		// Token: 0x0400251E RID: 9502
		public const byte EntitiesAtlasIndex = 1;

		// Token: 0x0400251F RID: 9503
		public const byte CosmeticsAtlasIndex = 2;

		// Token: 0x04002520 RID: 9504
		public static int ServerEntitySeed;

		// Token: 0x04002521 RID: 9505
		public const float DrawScaleFactor = 64f;

		// Token: 0x04002522 RID: 9506
		public const float DefaultScale = 0.015625f;

		// Token: 0x04002523 RID: 9507
		public const float MapToEntityScaleFactor = 0.5f;

		// Token: 0x04002524 RID: 9508
		private const float ParticleRenderingDistance = 20f;

		// Token: 0x04002525 RID: 9509
		private const long MaxServerEffectHistoryMs = 1000L;

		// Token: 0x04002526 RID: 9510
		private static readonly Cosmetic[] _defaultHeadCosmeticsToHide = new Cosmetic[]
		{
			default(Cosmetic),
			8,
			10
		};

		// Token: 0x04002527 RID: 9511
		private static readonly Cosmetic[] _defaultChestCosmeticsToHide = new Cosmetic[]
		{
			3
		};

		// Token: 0x04002528 RID: 9512
		private static readonly Cosmetic[] _defaultHandsCosmeticsToHide = new Cosmetic[]
		{
			7
		};

		// Token: 0x04002529 RID: 9513
		private static readonly Cosmetic[] _defaultLegsCosmeticsToHide = new Cosmetic[]
		{
			5,
			6
		};

		// Token: 0x0400252A RID: 9514
		protected readonly GameInstance _gameInstance;

		// Token: 0x0400252B RID: 9515
		public int NetworkId;

		// Token: 0x0400252C RID: 9516
		public readonly bool IsLocalEntity;

		// Token: 0x0400252D RID: 9517
		public bool IsVisible = true;

		// Token: 0x0400252E RID: 9518
		public bool Removed = false;

		// Token: 0x0400252F RID: 9519
		private bool _wasShouldRender;

		// Token: 0x04002530 RID: 9520
		public Guid? PredictionId;

		// Token: 0x04002531 RID: 9521
		public Entity ServerEntity;

		// Token: 0x04002532 RID: 9522
		public bool Predictable;

		// Token: 0x04002533 RID: 9523
		private bool _beenPredicted = false;

		// Token: 0x04002534 RID: 9524
		public bool VisibilityPrediction = true;

		// Token: 0x04002535 RID: 9525
		public uint LastAnimationUpdateFrameId;

		// Token: 0x04002536 RID: 9526
		public uint LastLogicUpdateFrameId;

		// Token: 0x04002538 RID: 9528
		private bool _isTangible = true;

		// Token: 0x04002539 RID: 9529
		private bool _isInvulnerable = false;

		// Token: 0x0400253A RID: 9530
		public Entity.UniqueEntityEffect[] EntityEffects = new Entity.UniqueEntityEffect[0];

		// Token: 0x0400253B RID: 9531
		private List<EntityEffectUpdate> _predictedEffects = new List<EntityEffectUpdate>();

		// Token: 0x0400253C RID: 9532
		private List<Entity.ServerEffectEntry> _serverEffects = new List<Entity.ServerEffectEntry>();

		// Token: 0x0400253D RID: 9533
		protected bool _effectsOnEntityDirty = false;

		// Token: 0x0400253E RID: 9534
		protected readonly List<Entity.EntityParticle> _entityParticles = new List<Entity.EntityParticle>();

		// Token: 0x0400253F RID: 9535
		protected readonly List<Entity.EntityTrail> _entityTrails = new List<Entity.EntityTrail>();

		// Token: 0x04002540 RID: 9536
		protected readonly List<Entity.EntityParticle> _combatSequenceParticles = new List<Entity.EntityParticle>();

		// Token: 0x04002541 RID: 9537
		protected readonly List<Entity.EntityTrail> _combatSequenceTrails = new List<Entity.EntityTrail>();

		// Token: 0x04002542 RID: 9538
		private ParticleSystemProxy _walkParticleSystem;

		// Token: 0x04002543 RID: 9539
		private ParticleSystemProxy _runParticleSystem;

		// Token: 0x04002544 RID: 9540
		private ParticleSystemProxy _sprintParticleSystem;

		// Token: 0x04002545 RID: 9541
		private int _previousWalkBlockId = -1;

		// Token: 0x04002546 RID: 9542
		private int _previousRunBlockId = -1;

		// Token: 0x04002547 RID: 9543
		private int _previousSprintBlockId = -1;

		// Token: 0x04002548 RID: 9544
		protected Vector3 _previousPosition;

		// Token: 0x04002549 RID: 9545
		protected Vector3 _nextPosition;

		// Token: 0x0400254A RID: 9546
		public float PositionProgress;

		// Token: 0x0400254B RID: 9547
		private Vector3 _position;

		// Token: 0x0400254C RID: 9548
		public Vector3 RenderPosition;

		// Token: 0x04002551 RID: 9553
		private Vector3 _previousBodyOrientation;

		// Token: 0x04002552 RID: 9554
		protected Vector3 _nextBodyOrientation;

		// Token: 0x04002553 RID: 9555
		public float BodyOrientationProgress;

		// Token: 0x04002554 RID: 9556
		private Vector3 _bodyOrientation;

		// Token: 0x04002555 RID: 9557
		public Quaternion RenderOrientation = Quaternion.Identity;

		// Token: 0x04002558 RID: 9560
		public List<Entity.EntityItem> EntityItems = new List<Entity.EntityItem>();

		// Token: 0x04002559 RID: 9561
		protected Vector3 _bottomTint = Vector3.Zero;

		// Token: 0x0400255A RID: 9562
		protected Vector3 _topTint = Vector3.Zero;

		// Token: 0x0400255B RID: 9563
		public bool UseDithering = false;

		// Token: 0x0400255C RID: 9564
		public ClientModelVFX ModelVFX;

		// Token: 0x0400255F RID: 9567
		private BlockyModel _characterModel;

		// Token: 0x04002562 RID: 9570
		public ClientItemBase ConsumableItem;

		// Token: 0x04002563 RID: 9571
		private ClientItemBase _originalPrimaryItem;

		// Token: 0x04002564 RID: 9572
		public ColorRgb DynamicLight;

		// Token: 0x04002565 RID: 9573
		private ColorRgb _armorLight;

		// Token: 0x04002566 RID: 9574
		private ColorRgb _itemLight;

		// Token: 0x04002567 RID: 9575
		public Vector3 LookOrientation;

		// Token: 0x04002568 RID: 9576
		public float Scale;

		// Token: 0x0400256B RID: 9579
		public CameraSettings ActionCameraSettings;

		// Token: 0x0400256C RID: 9580
		private CameraSettings _modelCameraSettings;

		// Token: 0x0400256D RID: 9581
		private CameraSettings _itemCameraSettings;

		// Token: 0x0400256E RID: 9582
		public ClientMovementStates ServerMovementStates = ClientMovementStates.Idle;

		// Token: 0x0400256F RID: 9583
		protected float _fallHeight = 0f;

		// Token: 0x04002570 RID: 9584
		private int _previousBlockId = 0;

		// Token: 0x04002571 RID: 9585
		private int _previousBlockY = 0;

		// Token: 0x04002572 RID: 9586
		private bool _wasOnGround = true;

		// Token: 0x04002573 RID: 9587
		private bool _wasInFluid = false;

		// Token: 0x04002574 RID: 9588
		protected bool _wasFalling = false;

		// Token: 0x04002575 RID: 9589
		private bool _wasJumping = false;

		// Token: 0x04002576 RID: 9590
		private bool _usable = false;

		// Token: 0x04002577 RID: 9591
		public AudioDevice.SoundEventReference SoundEventReference = AudioDevice.SoundEventReference.None;

		// Token: 0x04002578 RID: 9592
		public AudioDevice.SoundObjectReference SoundObjectReference = AudioDevice.SoundObjectReference.Empty;

		// Token: 0x04002579 RID: 9593
		public List<uint> ActiveSounds = new List<uint>();

		// Token: 0x0400257B RID: 9595
		public int PredictedStatusCount;

		// Token: 0x0400257C RID: 9596
		private readonly Dictionary<string, ClientAnimationSet> _animationSets = new Dictionary<string, ClientAnimationSet>();

		// Token: 0x0400257D RID: 9597
		protected string _currentAnimationId;

		// Token: 0x0400257E RID: 9598
		protected float _currentAnimationRunTime;

		// Token: 0x0400257F RID: 9599
		private float _nextPassiveAnimationTimer;

		// Token: 0x04002580 RID: 9600
		private float _nextPassiveAnimationThreshold;

		// Token: 0x04002581 RID: 9601
		private int _countPassiveAnimation;

		// Token: 0x04002582 RID: 9602
		private EntityAnimation _passiveAnimation;

		// Token: 0x04002585 RID: 9605
		private bool _isCurrentActionAnimationHoldingLastFrame;

		// Token: 0x04002586 RID: 9606
		public readonly string[] ServerAnimations = new string[typeof(AnimationSlot).GetEnumValues().Length];

		// Token: 0x04002587 RID: 9607
		private Entity.ResumeActionAnimationData _previousActionAnimation;

		// Token: 0x04002588 RID: 9608
		private readonly AudioDevice.SoundEventReference[] _animationSoundEventReferences = new AudioDevice.SoundEventReference[typeof(AnimationSlot).GetEnumValues().Length];

		// Token: 0x0400258A RID: 9610
		public int HitboxCollisionConfigIndex = -1;

		// Token: 0x0400258B RID: 9611
		public int RepulsionConfigIndex = -1;

		// Token: 0x0400258C RID: 9612
		public Vector2 LastPush = default(Vector2);

		// Token: 0x0400258D RID: 9613
		private Item _itemPacket;

		// Token: 0x0400258F RID: 9615
		private int _blockId;

		// Token: 0x04002590 RID: 9616
		private ParticleSystemProxy _itemParticleSystem;

		// Token: 0x04002592 RID: 9618
		private int _currentJumpAnimation = 0;

		// Token: 0x04002593 RID: 9619
		private string _lastJumpAnimation;

		// Token: 0x04002594 RID: 9620
		private static readonly List<string> JumpAnimations = new List<string>(3)
		{
			"JumpWalk",
			"JumpRun",
			"JumpSprint"
		};

		// Token: 0x04002596 RID: 9622
		public Dictionary<ValueTuple<int, ForkedChainId>, Dictionary<int, InteractionMetaStore>> InteractionMetaStores = new Dictionary<ValueTuple<int, ForkedChainId>, Dictionary<int, InteractionMetaStore>>();

		// Token: 0x04002597 RID: 9623
		public Dictionary<InteractionType, int> Interactions = new Dictionary<InteractionType, int>();

		// Token: 0x04002598 RID: 9624
		public List<int> RunningInteractions = new List<int>();

		// Token: 0x04002599 RID: 9625
		private static readonly CameraAxis DefaultCameraAxis = new CameraAxis(new Rangef(-0.7853982f, 0.7853982f), new CameraNode[]
		{
			1
		});

		// Token: 0x0400259A RID: 9626
		public const int UndefinedAttribute = -1;

		// Token: 0x0400259B RID: 9627
		public ClientEntityStatValue[] _entityStats;

		// Token: 0x0400259C RID: 9628
		public ClientEntityStatValue[] _serverEntityStats;

		// Token: 0x0400259D RID: 9629
		private List<Entity.TimedStatUpdate> _predictedStats = new List<Entity.TimedStatUpdate>();

		// Token: 0x0400259E RID: 9630
		private List<Entity.TimedStatUpdate> _serverStats = new List<Entity.TimedStatUpdate>();

		// Token: 0x040025A0 RID: 9632
		public int[] UIComponents;

		// Token: 0x02000E2E RID: 3630
		public struct CombatText
		{
			// Token: 0x04004563 RID: 17763
			public double HitAngleDeg;

			// Token: 0x04004564 RID: 17764
			public string Text;
		}

		// Token: 0x02000E2F RID: 3631
		public enum EntityType
		{
			// Token: 0x04004566 RID: 17766
			None,
			// Token: 0x04004567 RID: 17767
			Character,
			// Token: 0x04004568 RID: 17768
			Item,
			// Token: 0x04004569 RID: 17769
			Block
		}

		// Token: 0x02000E30 RID: 3632
		private struct ServerEffectEntry
		{
			// Token: 0x0400456A RID: 17770
			public long CreationTime;

			// Token: 0x0400456B RID: 17771
			public EntityEffectUpdate Update;
		}

		// Token: 0x02000E31 RID: 3633
		public struct UniqueEntityEffect
		{
			// Token: 0x06006708 RID: 26376 RVA: 0x00216B24 File Offset: 0x00214D24
			public UniqueEntityEffect(EntityEffect entityEffect, int networkEffectIndex)
			{
				this.NetworkEffectIndex = networkEffectIndex;
				EntityEffect.ApplicationEffects applicationEffects_ = entityEffect.ApplicationEffects_;
				this.ScreenEffect = ((applicationEffects_ != null) ? applicationEffects_.ScreenEffect : null);
				this.IsExpiring = false;
				this.IsInfinite = entityEffect.Infinite;
				this.IsDebuff = entityEffect.Debuff;
				this.StatusEffectIcon = entityEffect.StatusEffectIcon;
				this.RemainingDuration = entityEffect.Duration;
				this.ValueType = entityEffect.ValueType_;
				bool flag = entityEffect.StatModifiers != null && entityEffect.DamageCalculatorCooldown == 0.0;
				if (flag)
				{
					this.StatModifiers = new Entity.UniqueEntityEffect.StatModifier[entityEffect.StatModifiers.Count];
					int num = 0;
					foreach (KeyValuePair<int, float> keyValuePair in entityEffect.StatModifiers)
					{
						this.StatModifiers[num++] = new Entity.UniqueEntityEffect.StatModifier
						{
							StatIndex = keyValuePair.Key,
							Amount = keyValuePair.Value
						};
					}
				}
				else
				{
					this.StatModifiers = null;
				}
				EntityEffect.ApplicationEffects applicationEffects_2 = entityEffect.ApplicationEffects_;
				bool flag2 = ((applicationEffects_2 != null) ? applicationEffects_2.EntityBottomTint : null) != null;
				if (flag2)
				{
					this.BottomTint = new Vector3((float)((byte)entityEffect.ApplicationEffects_.EntityBottomTint.Red) / 255f, (float)((byte)entityEffect.ApplicationEffects_.EntityBottomTint.Green) / 255f, (float)((byte)entityEffect.ApplicationEffects_.EntityBottomTint.Blue) / 255f);
				}
				else
				{
					this.BottomTint = Vector3.Zero;
				}
				EntityEffect.ApplicationEffects applicationEffects_3 = entityEffect.ApplicationEffects_;
				bool flag3 = ((applicationEffects_3 != null) ? applicationEffects_3.EntityTopTint : null) != null;
				if (flag3)
				{
					this.TopTint = new Vector3((float)((byte)entityEffect.ApplicationEffects_.EntityTopTint.Red) / 255f, (float)((byte)entityEffect.ApplicationEffects_.EntityTopTint.Green) / 255f, (float)((byte)entityEffect.ApplicationEffects_.EntityTopTint.Blue) / 255f);
				}
				else
				{
					this.TopTint = Vector3.Zero;
				}
				this.ParticleSystems = null;
				this.ModelRenderer = null;
				this.SpawnAnimation = null;
				this.IdleAnimation = null;
				this.DespawnAnimation = null;
				this.SoundEventReference = AudioDevice.SoundEventReference.None;
				this._timeToNextTick = 0f;
			}

			// Token: 0x06006709 RID: 26377 RVA: 0x00216D8C File Offset: 0x00214F8C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool NeedTick()
			{
				return this.StatModifiers != null;
			}

			// Token: 0x0600670A RID: 26378 RVA: 0x00216D98 File Offset: 0x00214F98
			public void Tick(float invServerUpdatesPerSecond, Entity entity, float deltaTime)
			{
				this._timeToNextTick -= deltaTime;
				bool flag = this._timeToNextTick > 0f;
				if (!flag)
				{
					this._timeToNextTick = invServerUpdatesPerSecond;
					bool flag2 = this.StatModifiers != null;
					if (flag2)
					{
						this.AttemptEntityStats(entity);
					}
				}
			}

			// Token: 0x0600670B RID: 26379 RVA: 0x00216DE4 File Offset: 0x00214FE4
			private void AttemptEntityStats(Entity entity)
			{
				int i = 0;
				while (i < this.StatModifiers.Length)
				{
					Entity.UniqueEntityEffect.StatModifier statModifier = this.StatModifiers[i];
					float num = statModifier.Amount;
					bool flag = this.ValueType == 0;
					if (!flag)
					{
						goto IL_5C;
					}
					ClientEntityStatValue entityStat = entity.GetEntityStat(statModifier.StatIndex);
					bool flag2 = entityStat == null;
					if (!flag2)
					{
						num = num * (entityStat.Max - entityStat.Min) / 100f;
						goto IL_5C;
					}
					IL_7C:
					i++;
					continue;
					IL_5C:
					bool flag3 = num != 0f;
					if (flag3)
					{
						entity.AddStatValue(statModifier.StatIndex, num);
					}
					goto IL_7C;
				}
			}

			// Token: 0x0600670C RID: 26380 RVA: 0x00216E88 File Offset: 0x00215088
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static int PackModelVFXData(int direction, int switchTo, int useBloom, int useProgressiveHighlight)
			{
				return direction | switchTo << 3 | useBloom << 5 | useProgressiveHighlight << 6;
			}

			// Token: 0x0400456C RID: 17772
			public readonly int NetworkEffectIndex;

			// Token: 0x0400456D RID: 17773
			public Vector3 BottomTint;

			// Token: 0x0400456E RID: 17774
			public Vector3 TopTint;

			// Token: 0x0400456F RID: 17775
			public bool IsExpiring;

			// Token: 0x04004570 RID: 17776
			public string ScreenEffect;

			// Token: 0x04004571 RID: 17777
			public List<Entity.EntityParticle> ParticleSystems;

			// Token: 0x04004572 RID: 17778
			public ModelRenderer ModelRenderer;

			// Token: 0x04004573 RID: 17779
			public EntityAnimation SpawnAnimation;

			// Token: 0x04004574 RID: 17780
			public EntityAnimation IdleAnimation;

			// Token: 0x04004575 RID: 17781
			public EntityAnimation DespawnAnimation;

			// Token: 0x04004576 RID: 17782
			public Entity.UniqueEntityEffect.StatModifier[] StatModifiers;

			// Token: 0x04004577 RID: 17783
			public ValueType ValueType;

			// Token: 0x04004578 RID: 17784
			public AudioDevice.SoundEventReference SoundEventReference;

			// Token: 0x04004579 RID: 17785
			public bool IsInfinite;

			// Token: 0x0400457A RID: 17786
			public bool IsDebuff;

			// Token: 0x0400457B RID: 17787
			public string StatusEffectIcon;

			// Token: 0x0400457C RID: 17788
			public float RemainingDuration;

			// Token: 0x0400457D RID: 17789
			private float _timeToNextTick;

			// Token: 0x020010A1 RID: 4257
			public struct StatModifier
			{
				// Token: 0x04004EA8 RID: 20136
				public int StatIndex;

				// Token: 0x04004EA9 RID: 20137
				public float Amount;
			}
		}

		// Token: 0x02000E32 RID: 3634
		public class EntityItem
		{
			// Token: 0x1700145D RID: 5213
			// (get) Token: 0x0600670D RID: 26381 RVA: 0x00216EA9 File Offset: 0x002150A9
			public Vector3 RootPositionOffset
			{
				get
				{
					return this._rootPositionOffset;
				}
			}

			// Token: 0x1700145E RID: 5214
			// (get) Token: 0x0600670E RID: 26382 RVA: 0x00216EB1 File Offset: 0x002150B1
			public Quaternion RootOrientationOffset
			{
				get
				{
					return this._rootOrientationOffset;
				}
			}

			// Token: 0x1700145F RID: 5215
			// (get) Token: 0x0600670F RID: 26383 RVA: 0x00216EB9 File Offset: 0x002150B9
			public ref Matrix RootOffsetMatrix
			{
				get
				{
					return ref this._rootOffsetMatrix;
				}
			}

			// Token: 0x06006710 RID: 26384 RVA: 0x00216EC1 File Offset: 0x002150C1
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void SetRootOffsets(Vector3 positionOffset, Quaternion orientationOffset)
			{
				this._rootPositionOffset = positionOffset;
				this._rootOrientationOffset = orientationOffset;
				Matrix.Compose(orientationOffset, positionOffset, out this._rootOffsetMatrix);
			}

			// Token: 0x06006711 RID: 26385 RVA: 0x00216EE0 File Offset: 0x002150E0
			public EntityItem(GameInstance gameInstance)
			{
				this._gameInstance = gameInstance;
				this._rootOrientationOffset = Quaternion.Identity;
				this._rootOffsetMatrix = Matrix.Identity;
				this.ModelVFX = new ClientModelVFX();
			}

			// Token: 0x06006712 RID: 26386 RVA: 0x00216F49 File Offset: 0x00215149
			public bool HasFX()
			{
				return this.Particles.Count + this.Trails.Count > 0;
			}

			// Token: 0x06006713 RID: 26387 RVA: 0x00216F68 File Offset: 0x00215168
			public void ClearFX()
			{
				this.ParentParticles.Clear();
				for (int i = 0; i < this.Particles.Count; i++)
				{
					ParticleSystemProxy particleSystemProxy = this.Particles[i].ParticleSystemProxy;
					if (particleSystemProxy != null)
					{
						particleSystemProxy.Expire(false);
					}
				}
				this.Particles.Clear();
				this.ParentTrails.Clear();
				for (int j = 0; j < this.Trails.Count; j++)
				{
					bool flag = this.Trails[j].TrailProxy != null;
					if (flag)
					{
						this.Trails[j].TrailProxy.IsExpired = true;
					}
				}
				this.Trails.Clear();
			}

			// Token: 0x0400457E RID: 17790
			public ClientModelVFX ModelVFX;

			// Token: 0x0400457F RID: 17791
			public ModelRenderer ModelRenderer;

			// Token: 0x04004580 RID: 17792
			public float Scale;

			// Token: 0x04004581 RID: 17793
			public int TargetNodeNameId;

			// Token: 0x04004582 RID: 17794
			public int TargetNodeIndex;

			// Token: 0x04004583 RID: 17795
			public AudioDevice.SoundEventReference SoundEventReference;

			// Token: 0x04004584 RID: 17796
			public readonly List<Entity.EntityParticle> ParentParticles = new List<Entity.EntityParticle>();

			// Token: 0x04004585 RID: 17797
			public readonly List<Entity.EntityTrail> ParentTrails = new List<Entity.EntityTrail>();

			// Token: 0x04004586 RID: 17798
			public readonly List<Entity.EntityParticle> Particles = new List<Entity.EntityParticle>();

			// Token: 0x04004587 RID: 17799
			public readonly List<Entity.EntityTrail> Trails = new List<Entity.EntityTrail>();

			// Token: 0x04004588 RID: 17800
			public ClientItemAppearanceCondition.Data CurrentItemAppearanceCondition;

			// Token: 0x04004589 RID: 17801
			private readonly GameInstance _gameInstance;

			// Token: 0x0400458A RID: 17802
			private Vector3 _rootPositionOffset;

			// Token: 0x0400458B RID: 17803
			private Quaternion _rootOrientationOffset;

			// Token: 0x0400458C RID: 17804
			private Matrix _rootOffsetMatrix;
		}

		// Token: 0x02000E33 RID: 3635
		public class EntityTrail
		{
			// Token: 0x17001460 RID: 5216
			// (get) Token: 0x06006714 RID: 26388 RVA: 0x00217029 File Offset: 0x00215229
			// (set) Token: 0x06006715 RID: 26389 RVA: 0x00217031 File Offset: 0x00215231
			public TrailProxy TrailProxy { get; private set; }

			// Token: 0x17001461 RID: 5217
			// (get) Token: 0x06006716 RID: 26390 RVA: 0x0021703A File Offset: 0x0021523A
			// (set) Token: 0x06006717 RID: 26391 RVA: 0x00217042 File Offset: 0x00215242
			public EntityPart EntityPart { get; private set; }

			// Token: 0x17001462 RID: 5218
			// (get) Token: 0x06006718 RID: 26392 RVA: 0x0021704B File Offset: 0x0021524B
			// (set) Token: 0x06006719 RID: 26393 RVA: 0x00217053 File Offset: 0x00215253
			public int TargetNodeNameId { get; private set; }

			// Token: 0x17001463 RID: 5219
			// (get) Token: 0x0600671A RID: 26394 RVA: 0x0021705C File Offset: 0x0021525C
			// (set) Token: 0x0600671B RID: 26395 RVA: 0x00217064 File Offset: 0x00215264
			public int TargetNodeIndex { get; private set; }

			// Token: 0x17001464 RID: 5220
			// (get) Token: 0x0600671C RID: 26396 RVA: 0x0021706D File Offset: 0x0021526D
			// (set) Token: 0x0600671D RID: 26397 RVA: 0x00217075 File Offset: 0x00215275
			public bool FixedRotation { get; private set; }

			// Token: 0x0600671E RID: 26398 RVA: 0x00217080 File Offset: 0x00215280
			public EntityTrail(TrailProxy trailProxy, EntityPart entityPart, int targetNodeIndex, int targetNodeNameId, bool fixedRotation)
			{
				this.TrailProxy = trailProxy;
				this.EntityPart = entityPart;
				this.TargetNodeIndex = targetNodeIndex;
				this.TargetNodeNameId = targetNodeNameId;
				this.FixedRotation = fixedRotation;
			}

			// Token: 0x04004591 RID: 17809
			public int TargetFirstPersonNodeIndex = -1;

			// Token: 0x04004593 RID: 17811
			public Vector3 PositionOffset = Vector3.Zero;

			// Token: 0x04004594 RID: 17812
			public Quaternion RotationOffset = Quaternion.Identity;
		}

		// Token: 0x02000E34 RID: 3636
		public class EntityParticle
		{
			// Token: 0x17001465 RID: 5221
			// (get) Token: 0x0600671F RID: 26399 RVA: 0x002170DC File Offset: 0x002152DC
			// (set) Token: 0x06006720 RID: 26400 RVA: 0x002170E4 File Offset: 0x002152E4
			public ParticleSystemProxy ParticleSystemProxy { get; private set; }

			// Token: 0x17001466 RID: 5222
			// (get) Token: 0x06006721 RID: 26401 RVA: 0x002170ED File Offset: 0x002152ED
			// (set) Token: 0x06006722 RID: 26402 RVA: 0x002170F5 File Offset: 0x002152F5
			public EntityPart EntityPart { get; private set; }

			// Token: 0x17001467 RID: 5223
			// (get) Token: 0x06006723 RID: 26403 RVA: 0x002170FE File Offset: 0x002152FE
			// (set) Token: 0x06006724 RID: 26404 RVA: 0x00217106 File Offset: 0x00215306
			public int TargetNodeNameId { get; private set; }

			// Token: 0x17001468 RID: 5224
			// (get) Token: 0x06006725 RID: 26405 RVA: 0x0021710F File Offset: 0x0021530F
			// (set) Token: 0x06006726 RID: 26406 RVA: 0x00217117 File Offset: 0x00215317
			public int TargetNodeIndex { get; private set; }

			// Token: 0x06006727 RID: 26407 RVA: 0x00217120 File Offset: 0x00215320
			public EntityParticle(ParticleSystemProxy particleSystem, EntityPart entityPart, int targetNodeIndex, int targetNodeNameId, float itemScale)
			{
				this.ParticleSystemProxy = particleSystem;
				this.EntityPart = entityPart;
				this.TargetNodeIndex = targetNodeIndex;
				this.TargetNodeNameId = targetNodeNameId;
				this.ItemScale = itemScale;
			}

			// Token: 0x04004599 RID: 17817
			public int TargetFirstPersonNodeIndex = -1;

			// Token: 0x0400459A RID: 17818
			public Vector3 PositionOffset = Vector3.Zero;

			// Token: 0x0400459B RID: 17819
			public Quaternion RotationOffset = Quaternion.Identity;

			// Token: 0x0400459C RID: 17820
			public float ItemScale;
		}

		// Token: 0x02000E35 RID: 3637
		public struct DetailBoundingBox
		{
			// Token: 0x0400459D RID: 17821
			public Vector3 Offset;

			// Token: 0x0400459E RID: 17822
			public BoundingBox Box;
		}

		// Token: 0x02000E36 RID: 3638
		private class ResumeActionAnimationData
		{
			// Token: 0x0400459F RID: 17823
			public EntityAnimation EntityAnimation;

			// Token: 0x040045A0 RID: 17824
			public float EntityModelRendererResumeTime;
		}

		// Token: 0x02000E37 RID: 3639
		private struct TimedStatUpdate
		{
			// Token: 0x040045A1 RID: 17825
			public long CreationTime;

			// Token: 0x040045A2 RID: 17826
			public int Index;

			// Token: 0x040045A3 RID: 17827
			public EntityStatUpdate Update;
		}
	}
}
