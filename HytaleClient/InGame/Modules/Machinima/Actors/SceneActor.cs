using System;
using Coherent.UI.Binding;
using HytaleClient.Core;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Machinima.Settings;
using HytaleClient.InGame.Modules.Machinima.Track;
using HytaleClient.Math;
using HytaleClient.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HytaleClient.InGame.Modules.Machinima.Actors
{
	// Token: 0x0200092B RID: 2347
	[CoherentType]
	internal abstract class SceneActor : Disposable
	{
		// Token: 0x17001188 RID: 4488
		// (get) Token: 0x06004792 RID: 18322 RVA: 0x0010E958 File Offset: 0x0010CB58
		// (set) Token: 0x06004793 RID: 18323 RVA: 0x0010E960 File Offset: 0x0010CB60
		[JsonProperty(PropertyName = "Name")]
		[CoherentProperty("name")]
		public string Name { get; set; }

		// Token: 0x17001189 RID: 4489
		// (get) Token: 0x06004794 RID: 18324 RVA: 0x0010E969 File Offset: 0x0010CB69
		// (set) Token: 0x06004795 RID: 18325 RVA: 0x0010E971 File Offset: 0x0010CB71
		public SceneActor LookTargetActor { get; private set; }

		// Token: 0x1700118A RID: 4490
		// (get) Token: 0x06004796 RID: 18326 RVA: 0x0010E97A File Offset: 0x0010CB7A
		// (set) Token: 0x06004797 RID: 18327 RVA: 0x0010E982 File Offset: 0x0010CB82
		public SceneActor PositionTargetActor { get; private set; }

		// Token: 0x1700118B RID: 4491
		// (get) Token: 0x06004798 RID: 18328 RVA: 0x0010E98C File Offset: 0x0010CB8C
		// (set) Token: 0x06004799 RID: 18329 RVA: 0x0010E9A4 File Offset: 0x0010CBA4
		[JsonProperty(PropertyName = "Track")]
		[CoherentProperty("track")]
		public SceneTrack Track
		{
			get
			{
				return this._track;
			}
			set
			{
				bool flag = this._track != null;
				if (flag)
				{
					this._track.Dispose();
				}
				this._track = value;
			}
		}

		// Token: 0x1700118C RID: 4492
		// (get) Token: 0x0600479A RID: 18330 RVA: 0x0010E9D4 File Offset: 0x0010CBD4
		[JsonProperty(PropertyName = "Type")]
		[CoherentProperty("type")]
		public ActorType Type
		{
			get
			{
				return this.GetActorType();
			}
		}

		// Token: 0x0600479B RID: 18331 RVA: 0x0010E9EC File Offset: 0x0010CBEC
		protected virtual ActorType GetActorType()
		{
			return ActorType.Reference;
		}

		// Token: 0x0600479C RID: 18332 RVA: 0x0010E9F0 File Offset: 0x0010CBF0
		public SceneActor(GameInstance gameInstance, string name)
		{
			this.Name = name;
			this._track = new SceneTrack(gameInstance, this);
			this.Id = ++SceneActor._nextId;
		}

		// Token: 0x0600479D RID: 18333 RVA: 0x0010EA55 File Offset: 0x0010CC55
		public virtual void Draw(ref Matrix viewProjectionMatrix)
		{
			this.Track.Draw(ref viewProjectionMatrix, true, true, true);
		}

		// Token: 0x0600479E RID: 18334 RVA: 0x0010EA68 File Offset: 0x0010CC68
		public virtual void Update(float currentFrame, float lastFrame)
		{
			this.Track.Update(currentFrame, lastFrame);
		}

		// Token: 0x0600479F RID: 18335 RVA: 0x0010EA79 File Offset: 0x0010CC79
		protected override void DoDispose()
		{
			this.Track.Dispose();
		}

		// Token: 0x060047A0 RID: 18336 RVA: 0x0010EA88 File Offset: 0x0010CC88
		public void SetLookTarget(SceneActor actor)
		{
			this.LookTargetActor = actor;
		}

		// Token: 0x060047A1 RID: 18337 RVA: 0x0010EA94 File Offset: 0x0010CC94
		public virtual TrackKeyframe CreateKeyframe(float frame)
		{
			TrackKeyframe trackKeyframe = new TrackKeyframe(frame);
			trackKeyframe.AddSetting(new PositionSetting(this.Position));
			trackKeyframe.AddSetting(new RotationSetting(this.Rotation));
			trackKeyframe.AddSetting(new LookSetting(this.Look));
			return trackKeyframe;
		}

		// Token: 0x060047A2 RID: 18338 RVA: 0x0010EAE4 File Offset: 0x0010CCE4
		public TrackKeyframe CreateKeyframe(float frame, Vector3 position, Vector3 rotation, Vector3 look)
		{
			TrackKeyframe trackKeyframe = new TrackKeyframe(frame);
			trackKeyframe.AddSetting(new PositionSetting(position));
			trackKeyframe.AddSetting(new RotationSetting(rotation));
			trackKeyframe.AddSetting(new LookSetting(look));
			return trackKeyframe;
		}

		// Token: 0x060047A3 RID: 18339 RVA: 0x0010EB28 File Offset: 0x0010CD28
		public virtual void LoadKeyframe(TrackKeyframe keyframe)
		{
			Vector3 position = this.Position;
			Vector3 value = keyframe.GetSetting<Vector3>("Position").Value;
			bool flag = this.PositionTargetActor != null;
			if (flag)
			{
				Vector3 position2 = this.PositionTargetActor.Position;
				this.Position = position2 + value;
			}
			else
			{
				this.Position = value;
			}
			this.Rotation = keyframe.GetSetting<Vector3>("Rotation").Value;
			bool flag2 = this.LookTargetActor != null;
			if (flag2)
			{
				Vector3 position3 = this.LookTargetActor.Position;
				bool flag3 = this.LookTargetActor is EntityActor;
				if (flag3)
				{
					Entity entity = (this.LookTargetActor as EntityActor).GetEntity();
					bool flag4 = entity != null && entity.Type == Entity.EntityType.Character;
					if (flag4)
					{
						position3 = entity.Position;
						position3.Y += entity.EyeOffset;
					}
				}
				Vector3 position4 = this.Position;
				bool flag5 = this is EntityActor;
				if (flag5)
				{
					Entity entity2 = (this as EntityActor).GetEntity();
					bool flag6 = entity2 != null && entity2.Type == Entity.EntityType.Character;
					if (flag6)
					{
						position4 = entity2.Position;
						position4.Y += entity2.EyeOffset;
					}
				}
				Vector3 targetDirection = Vector3.GetTargetDirection(position4, position3);
				Vector3 value2 = keyframe.GetSetting<Vector3>("Look").Value;
				this.Look.X = targetDirection.X;
				this.Look.Y = targetDirection.Y;
				this.Look.Z = value2.Z;
			}
			else
			{
				this.Look = keyframe.GetSetting<Vector3>("Look").Value;
				this.Look.Y = this.Look.Y + this.Rotation.Y;
			}
			float num = MathHelper.WrapAngle(this.Look.Yaw - this.Rotation.Yaw);
			bool flag7 = num > 0.7853982f;
			if (flag7)
			{
				this.Rotation.Yaw = this.Rotation.Yaw + (num - 0.7853982f);
			}
			else
			{
				bool flag8 = num < -0.7853982f;
				if (flag8)
				{
					this.Rotation.Yaw = this.Rotation.Yaw + (num + 0.7853982f);
				}
			}
		}

		// Token: 0x060047A4 RID: 18340 RVA: 0x0010ED68 File Offset: 0x0010CF68
		public void AlignToPath(bool alignAll = false)
		{
			for (int i = 0; i < this.Track.Keyframes.Count; i++)
			{
				TrackKeyframe trackKeyframe = this.Track.Keyframes[i];
				float frame = trackKeyframe.Frame;
				KeyframeSetting<Vector3> setting = trackKeyframe.GetSetting<Vector3>("Position");
				bool flag;
				if (setting != null)
				{
					Vector3 value = setting.Value;
					flag = false;
				}
				else
				{
					flag = true;
				}
				bool flag2 = flag;
				if (!flag2)
				{
					Vector3 value2 = setting.Value;
					Vector3 source = (i == 0) ? value2 : this.Track.Path.GetPathPosition(i - 1, 0.99f, false, Easing.EasingType.Linear);
					Vector3 target = (i >= this.Track.Keyframes.Count - 1) ? value2 : this.Track.Path.GetPathPosition(i, 0.1f, false, Easing.EasingType.Linear);
					Vector3 targetDirection = Vector3.GetTargetDirection(source, target);
					Vector3 value3 = alignAll ? targetDirection : new Vector3(0f, targetDirection.Y, 0f);
					this.Track.Keyframes[i].GetSetting<Vector3>("Rotation").Value = value3;
					this.Track.Keyframes[i].GetSetting<Vector3>("Look").Value = new Vector3(targetDirection.X, 0f, targetDirection.Z);
				}
			}
			this.Track.UpdateKeyframeData();
		}

		// Token: 0x060047A5 RID: 18341 RVA: 0x0010EED0 File Offset: 0x0010D0D0
		public static SceneActor ConvertJsonObject(GameInstance gameInstance, SerializedSceneObject actorData)
		{
			SceneActor sceneActor;
			switch (actorData.Type)
			{
			case 0:
				sceneActor = new ReferenceActor(gameInstance, actorData.Name);
				break;
			case 1:
				sceneActor = new CameraActor(gameInstance, actorData.Name);
				break;
			case 2:
				sceneActor = new PlayerActor(gameInstance, actorData.Name);
				((PlayerActor)sceneActor).SetBaseModel(actorData.Model);
				break;
			case 3:
			{
				sceneActor = new EntityActor(gameInstance, actorData.Name, null);
				EntityActor entityActor = sceneActor as EntityActor;
				entityActor.SetBaseModel(actorData.Model);
				entityActor.ModelId = ((actorData.ModelId == null) ? "" : actorData.ModelId);
				entityActor.SetScale((actorData.Scale <= 0f) ? 1f : actorData.Scale);
				break;
			}
			case 4:
				sceneActor = new ItemActor(gameInstance, actorData.Name, null, actorData.ItemId);
				(sceneActor as EntityActor).SetScale((actorData.Scale <= 0f) ? 1f : actorData.Scale);
				break;
			default:
				return null;
			}
			sceneActor.Track = actorData.Track;
			return sceneActor;
		}

		// Token: 0x060047A6 RID: 18342 RVA: 0x0010F000 File Offset: 0x0010D200
		public virtual void WriteToJsonObject(JsonSerializer serializer, JsonWriter writer)
		{
			JObject jobject = new JObject();
			jobject.Add("Name", JToken.FromObject(this.Name));
			jobject.Add("Type", JToken.FromObject(this.Type));
			JTokenWriter jtokenWriter = new JTokenWriter();
			serializer.Serialize(jtokenWriter, this.Track, typeof(SceneTrack));
			jtokenWriter.Close();
			jobject.Add("Track", jtokenWriter.Token);
			ItemActor itemActor = this as ItemActor;
			bool flag = itemActor != null;
			if (flag)
			{
				jobject.Add("ItemId", JToken.FromObject(itemActor.ItemId));
				jobject.Add("Scale", JToken.FromObject(itemActor.Scale));
			}
			else
			{
				EntityActor entityActor = this as EntityActor;
				bool flag2 = entityActor != null;
				if (flag2)
				{
					Model model = entityActor.GetBaseModel();
					bool flag3 = model == null;
					if (flag3)
					{
						model = ((entityActor == null) ? null : entityActor.GetEntity().ModelPacket);
					}
					bool flag4 = model != null;
					if (flag4)
					{
						jobject.Add("Model", JToken.FromObject(entityActor.GetBaseModel()));
					}
					jobject.Add("ModelId", JToken.FromObject(entityActor.ModelId));
					jobject.Add("Scale", JToken.FromObject(entityActor.Scale));
				}
			}
			jobject.WriteTo(writer, Array.Empty<JsonConverter>());
		}

		// Token: 0x060047A7 RID: 18343
		public abstract SceneActor Clone(GameInstance gameInstance);

		// Token: 0x040023F2 RID: 9202
		private static int _nextId;

		// Token: 0x040023F3 RID: 9203
		[CoherentProperty("id")]
		public readonly int Id;

		// Token: 0x040023F5 RID: 9205
		[CoherentProperty("visible")]
		public bool Visible = true;

		// Token: 0x040023F6 RID: 9206
		public Vector3 Position = Vector3.Zero;

		// Token: 0x040023F7 RID: 9207
		public Vector3 Rotation = Vector3.Zero;

		// Token: 0x040023F8 RID: 9208
		public Vector3 Look = Vector3.Zero;

		// Token: 0x040023FB RID: 9211
		private SceneTrack _track;

		// Token: 0x040023FC RID: 9212
		protected Matrix _modelMatrix;

		// Token: 0x040023FD RID: 9213
		protected Matrix _tempMatrix;
	}
}
