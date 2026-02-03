using System;
using System.Collections.Generic;
using Coherent.UI.Binding;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Machinima.Settings;
using HytaleClient.InGame.Modules.Machinima.Track;
using HytaleClient.Math;
using HytaleClient.Protocol;
using Newtonsoft.Json;

namespace HytaleClient.InGame.Modules.Machinima.Actors
{
	// Token: 0x02000927 RID: 2343
	[CoherentType]
	internal class EntityActor : SceneActor
	{
		// Token: 0x17001185 RID: 4485
		// (get) Token: 0x06004771 RID: 18289 RVA: 0x0010DFF6 File Offset: 0x0010C1F6
		// (set) Token: 0x06004772 RID: 18290 RVA: 0x0010DFFE File Offset: 0x0010C1FE
		[JsonIgnore]
		protected Entity _entity { get; set; }

		// Token: 0x17001186 RID: 4486
		// (get) Token: 0x06004773 RID: 18291 RVA: 0x0010E007 File Offset: 0x0010C207
		// (set) Token: 0x06004774 RID: 18292 RVA: 0x0010E00F File Offset: 0x0010C20F
		public float Scale { get; private set; } = 1f;

		// Token: 0x06004775 RID: 18293 RVA: 0x0010E018 File Offset: 0x0010C218
		protected override ActorType GetActorType()
		{
			return ActorType.Entity;
		}

		// Token: 0x06004776 RID: 18294 RVA: 0x0010E01B File Offset: 0x0010C21B
		public EntityActor(GameInstance gameInstance, string name, Entity entity) : base(gameInstance, name)
		{
			this._entity = entity;
		}

		// Token: 0x06004777 RID: 18295 RVA: 0x0010E045 File Offset: 0x0010C245
		public void SetEntity(Entity entity)
		{
			this._entity = entity;
		}

		// Token: 0x06004778 RID: 18296 RVA: 0x0010E050 File Offset: 0x0010C250
		public Entity GetEntity()
		{
			return this._entity;
		}

		// Token: 0x06004779 RID: 18297 RVA: 0x0010E068 File Offset: 0x0010C268
		public Model GetModel()
		{
			Entity entity = this._entity;
			return (entity != null) ? entity.ModelPacket : null;
		}

		// Token: 0x0600477A RID: 18298 RVA: 0x0010E08C File Offset: 0x0010C28C
		public void SetBaseModel(Model model)
		{
			this._baseModel = model;
			bool flag = this._entity != null;
			if (flag)
			{
				this._entity.SetCharacterModel(model, new string[0]);
			}
		}

		// Token: 0x0600477B RID: 18299 RVA: 0x0010E0C4 File Offset: 0x0010C2C4
		public void SetScale(float scale)
		{
			this.Scale = scale;
			bool flag = this._entity != null;
			if (flag)
			{
				this._entity.Scale = scale;
			}
		}

		// Token: 0x0600477C RID: 18300 RVA: 0x0010E0F8 File Offset: 0x0010C2F8
		public void ForceUpdate(GameInstance gameInstance)
		{
			bool flag = this._entity == null;
			if (!flag)
			{
				this._entity.SetPosition(this.Position);
				this._entity.SetBodyOrientation(this.Rotation);
				this._entity.LookOrientation = this.Look;
				this._entity.PositionProgress = 1f;
				this._entity.BodyOrientationProgress = 1f;
			}
		}

		// Token: 0x0600477D RID: 18301 RVA: 0x0010E16C File Offset: 0x0010C36C
		public Model GetBaseModel()
		{
			return this._baseModel;
		}

		// Token: 0x0600477E RID: 18302 RVA: 0x0010E184 File Offset: 0x0010C384
		public virtual void Spawn(GameInstance gameInstance)
		{
			bool flag = this._entity != null || this is PlayerActor;
			if (!flag)
			{
				Model model = this.GetBaseModel();
				bool flag2 = model == null;
				if (flag2)
				{
					model = gameInstance.LocalPlayer.ModelPacket;
					this.SetBaseModel(model);
				}
				Vector3 value = new Vector3(0f, 100f, 0f);
				Vector3 bodyOrientation = Vector3.Zero;
				Vector3 lookOrientation = Vector3.Zero;
				SceneTrack track = base.Track;
				bool flag3;
				if (track == null)
				{
					flag3 = false;
				}
				else
				{
					List<TrackKeyframe> keyframes = track.Keyframes;
					int? num = (keyframes != null) ? new int?(keyframes.Count) : null;
					int num2 = 0;
					flag3 = (num.GetValueOrDefault() > num2 & num != null);
				}
				bool flag4 = flag3;
				if (flag4)
				{
					TrackKeyframe trackKeyframe = base.Track.Keyframes[0];
					value = trackKeyframe.GetSetting<Vector3>("Position").Value;
					bodyOrientation = trackKeyframe.GetSetting<Vector3>("Rotation").Value;
					lookOrientation = trackKeyframe.GetSetting<Vector3>("Look").Value;
				}
				Entity entity;
				gameInstance.EntityStoreModule.Spawn(-1, out entity);
				entity.SetIsTangible(true);
				entity.SetCharacterModel(model, new string[0]);
				entity.SetSpawnTransform(value, bodyOrientation, lookOrientation);
				entity.Scale = this.Scale;
				this._entity = entity;
			}
		}

		// Token: 0x0600477F RID: 18303 RVA: 0x0010E2D8 File Offset: 0x0010C4D8
		public void UpdateModel(GameInstance gameInstance, string modelId = null)
		{
			bool flag = modelId != null;
			if (flag)
			{
				this.ModelId = modelId;
			}
			bool flag2 = this._entity == null || string.IsNullOrWhiteSpace(this.ModelId);
			if (!flag2)
			{
				gameInstance.Connection.SendPacket(new RequestMachinimaActorModel(this.ModelId, gameInstance.MachinimaModule.ActiveScene.Name, base.Name));
			}
		}

		// Token: 0x06004780 RID: 18304 RVA: 0x0010E340 File Offset: 0x0010C540
		public virtual void Despawn(GameInstance gameInstance)
		{
			bool flag = this._entity == null || this is PlayerActor;
			if (!flag)
			{
				gameInstance.EntityStoreModule.Despawn(this._entity.NetworkId);
				this._entity = null;
			}
		}

		// Token: 0x06004781 RID: 18305 RVA: 0x0010E388 File Offset: 0x0010C588
		public override TrackKeyframe CreateKeyframe(float frame)
		{
			TrackKeyframe trackKeyframe = new TrackKeyframe(frame);
			trackKeyframe.AddSetting(new PositionSetting(this._entity.Position));
			trackKeyframe.AddSetting(new RotationSetting(this._entity.BodyOrientation));
			trackKeyframe.AddSetting(new LookSetting(this._entity.LookOrientation));
			return trackKeyframe;
		}

		// Token: 0x06004782 RID: 18306 RVA: 0x0010E3E8 File Offset: 0x0010C5E8
		public override void LoadKeyframe(TrackKeyframe keyframe)
		{
			base.LoadKeyframe(keyframe);
			bool flag = this._entity == null;
			if (!flag)
			{
				this._entity.SetPosition(this.Position);
				this._entity.SetBodyOrientation(this.Rotation);
				this._entity.LookOrientation = this.Look;
				this._entity.BodyOrientationProgress = 1f;
				this._entity.PositionProgress = 1f;
			}
		}

		// Token: 0x06004783 RID: 18307 RVA: 0x0010E464 File Offset: 0x0010C664
		public override SceneActor Clone(GameInstance gameInstance)
		{
			SceneActor sceneActor = new EntityActor(gameInstance, "clone", null);
			base.Track.CopyToActor(ref sceneActor);
			EntityActor entityActor = sceneActor as EntityActor;
			entityActor.SetBaseModel(this._baseModel);
			entityActor.SetScale(this.Scale);
			return entityActor;
		}

		// Token: 0x040023E8 RID: 9192
		protected Model _baseModel;

		// Token: 0x040023E9 RID: 9193
		public string ModelId = "";
	}
}
