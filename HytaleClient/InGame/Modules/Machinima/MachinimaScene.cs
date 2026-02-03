using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Coherent.UI.Binding;
using HytaleClient.Core;
using HytaleClient.InGame.Modules.Machinima.Actors;
using HytaleClient.InGame.Modules.Machinima.Events;
using HytaleClient.InGame.Modules.Machinima.Track;
using HytaleClient.Math;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Zlib;

namespace HytaleClient.InGame.Modules.Machinima
{
	// Token: 0x0200090E RID: 2318
	[CoherentType]
	internal class MachinimaScene : Disposable
	{
		// Token: 0x17001158 RID: 4440
		// (get) Token: 0x06004689 RID: 18057 RVA: 0x00108184 File Offset: 0x00106384
		[JsonProperty(PropertyName = "Actors")]
		[CoherentProperty("sceneObjects")]
		public List<SceneActor> Actors { get; } = new List<SceneActor>();

		// Token: 0x17001159 RID: 4441
		// (get) Token: 0x0600468A RID: 18058 RVA: 0x0010818C File Offset: 0x0010638C
		// (set) Token: 0x0600468B RID: 18059 RVA: 0x001081E1 File Offset: 0x001063E1
		[JsonProperty(PropertyName = "Origin")]
		[CoherentProperty("origin")]
		public Vector3 Origin
		{
			get
			{
				bool flag = this._origin.IsNaN();
				if (flag)
				{
					this._origin = this._gameInstance.LocalPlayer.Position;
					this.OriginLook = this._gameInstance.LocalPlayer.LookOrientation;
				}
				return this._origin;
			}
			set
			{
				this._origin = value;
			}
		}

		// Token: 0x1700115A RID: 4442
		// (get) Token: 0x0600468C RID: 18060 RVA: 0x001081EC File Offset: 0x001063EC
		// (set) Token: 0x0600468D RID: 18061 RVA: 0x00108204 File Offset: 0x00106404
		[JsonIgnore]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				bool flag = this._isActive == value;
				if (!flag)
				{
					this._isActive = value;
					foreach (SceneActor sceneActor in this.Actors)
					{
						EntityActor entityActor = sceneActor as EntityActor;
						bool flag2 = entityActor != null;
						if (flag2)
						{
							bool isActive = this._isActive;
							if (isActive)
							{
								entityActor.Spawn(this._gameInstance);
							}
							else
							{
								entityActor.Despawn(this._gameInstance);
							}
						}
					}
					bool isActive2 = this._isActive;
					if (isActive2)
					{
						this.RunStartupCommands();
					}
				}
			}
		}

		// Token: 0x0600468E RID: 18062 RVA: 0x001082BC File Offset: 0x001064BC
		public MachinimaScene(GameInstance gameInstance, string name)
		{
			this._gameInstance = gameInstance;
			this.Name = name;
		}

		// Token: 0x0600468F RID: 18063 RVA: 0x0010830C File Offset: 0x0010650C
		public void Initialize(GameInstance gameInstance)
		{
			this._gameInstance = gameInstance;
			for (int i = 0; i < this.Actors.Count; i++)
			{
				this.Actors[i].Track.Initialize(this._gameInstance, this.Actors[i]);
				foreach (TrackKeyframe trackKeyframe in this.Actors[i].Track.Keyframes)
				{
					bool flag = trackKeyframe.Events.Count > 0;
					if (flag)
					{
						foreach (KeyframeEvent keyframeEvent in trackKeyframe.Events)
						{
							keyframeEvent.Initialize(this);
						}
					}
				}
			}
		}

		// Token: 0x06004690 RID: 18064 RVA: 0x00108420 File Offset: 0x00106620
		public void Update(float frame)
		{
			foreach (SceneActor sceneActor in this.Actors)
			{
				sceneActor.Update(frame, this._lastFrame);
			}
			this._lastFrame = frame;
		}

		// Token: 0x06004691 RID: 18065 RVA: 0x00108488 File Offset: 0x00106688
		protected override void DoDispose()
		{
			foreach (SceneActor sceneActor in this.Actors)
			{
				sceneActor.Dispose();
			}
		}

		// Token: 0x06004692 RID: 18066 RVA: 0x001084E0 File Offset: 0x001066E0
		public void Draw(ref Matrix viewProjectionMatrix)
		{
			foreach (SceneActor sceneActor in this.Actors)
			{
				bool flag = !sceneActor.Visible;
				if (!flag)
				{
					sceneActor.Draw(ref viewProjectionMatrix);
				}
			}
		}

		// Token: 0x06004693 RID: 18067 RVA: 0x00108548 File Offset: 0x00106748
		public float GetSceneLength()
		{
			float num = 0f;
			foreach (SceneActor sceneActor in this.Actors)
			{
				float trackLength = sceneActor.Track.GetTrackLength();
				bool flag = trackLength > num;
				if (flag)
				{
					num = trackLength;
				}
			}
			return num;
		}

		// Token: 0x06004694 RID: 18068 RVA: 0x001085C4 File Offset: 0x001067C4
		public void ListActors()
		{
			bool flag = this.Actors.Count == 0;
			if (flag)
			{
				this._gameInstance.Chat.Log("No actors currently exist in scene '" + this.Name + "'");
			}
			else
			{
				this._gameInstance.Chat.Log(string.Format("{0} Actors in scene '{1}':", this.Actors.Count, this.Name));
				foreach (SceneActor sceneActor in this.Actors)
				{
					int count = sceneActor.Track.Keyframes.Count;
					float num = (count > 0) ? sceneActor.Track.Keyframes[count - 1].Frame : 0f;
					double num2 = Math.Round((double)(num / this._gameInstance.MachinimaModule.PlaybackFPS * 10f)) / 10.0;
					string text = sceneActor.GetType().Name.Replace("Actor", "");
					string text2 = (sceneActor == this._gameInstance.MachinimaModule.ActiveActor) ? " - Active" : "";
					string message = string.Format("'{0}' ({1}) - [{2} keyframes - {3} sec]{4}", new object[]
					{
						sceneActor.Name,
						text,
						count,
						num2,
						text2
					});
					this._gameInstance.Chat.Log(message);
				}
			}
		}

		// Token: 0x06004695 RID: 18069 RVA: 0x00108780 File Offset: 0x00106980
		public bool AddActor(SceneActor actor, bool addStartKeyframe = true)
		{
			bool flag = string.IsNullOrEmpty(actor.Name) || this.HasActor(actor.Name);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = actor is PlayerActor;
				if (flag2)
				{
					foreach (SceneActor sceneActor in this.Actors)
					{
						bool flag3 = sceneActor is PlayerActor;
						if (flag3)
						{
							this._gameInstance.Chat.Log("Only one player actor may be added per scene.");
							return false;
						}
					}
				}
				else
				{
					EntityActor entityActor = actor as EntityActor;
					bool flag4 = entityActor != null && this.IsActive;
					if (flag4)
					{
						entityActor.Spawn(this._gameInstance);
					}
				}
				if (addStartKeyframe)
				{
					Vector3 position = (actor is CameraActor) ? this._gameInstance.CameraModule.GetLookRay().Position : this._gameInstance.LocalPlayer.Position;
					Vector3 lookOrientation = this._gameInstance.LocalPlayer.LookOrientation;
					Vector3 bodyOrientation = this._gameInstance.LocalPlayer.BodyOrientation;
					bodyOrientation.Y = MathHelper.WrapAngle(bodyOrientation.Y);
					lookOrientation.Y -= bodyOrientation.Y;
					TrackKeyframe keyframe = actor.CreateKeyframe(0f, position, bodyOrientation, lookOrientation);
					actor.Track.AddKeyframe(keyframe, true);
				}
				this.Actors.Add(actor);
				result = true;
			}
			return result;
		}

		// Token: 0x06004696 RID: 18070 RVA: 0x0010891C File Offset: 0x00106B1C
		public bool RemoveActor(string actorName)
		{
			int num = -1;
			for (int i = 0; i < this.Actors.Count; i++)
			{
				bool flag = this.Actors[i].Name == actorName;
				if (flag)
				{
					num = i;
					break;
				}
			}
			bool flag2 = num > -1;
			bool result;
			if (flag2)
			{
				EntityActor entityActor = this.Actors[num] as EntityActor;
				bool flag3 = entityActor != null;
				if (flag3)
				{
					entityActor.Despawn(this._gameInstance);
				}
				this.Actors[num].Dispose();
				this.Actors.RemoveAt(num);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06004697 RID: 18071 RVA: 0x001089CC File Offset: 0x00106BCC
		public SceneActor GetActor(string actorName)
		{
			foreach (SceneActor sceneActor in this.Actors)
			{
				bool flag = sceneActor.Name == actorName;
				if (flag)
				{
					return sceneActor;
				}
			}
			return null;
		}

		// Token: 0x06004698 RID: 18072 RVA: 0x00108A38 File Offset: 0x00106C38
		public SceneActor GetActor(int actorId)
		{
			foreach (SceneActor sceneActor in this.Actors)
			{
				bool flag = sceneActor.Id == actorId;
				if (flag)
				{
					return sceneActor;
				}
			}
			return null;
		}

		// Token: 0x06004699 RID: 18073 RVA: 0x00108AA0 File Offset: 0x00106CA0
		public TrackKeyframe GetEventKeyframe(int eventId)
		{
			foreach (SceneActor sceneActor in this.Actors)
			{
				foreach (TrackKeyframe trackKeyframe in sceneActor.Track.Keyframes)
				{
					bool flag = trackKeyframe.HasEvent(eventId);
					if (flag)
					{
						return trackKeyframe;
					}
				}
			}
			return null;
		}

		// Token: 0x0600469A RID: 18074 RVA: 0x00108B50 File Offset: 0x00106D50
		public bool HasActor(string actorName)
		{
			return this.GetActor(actorName) != null;
		}

		// Token: 0x0600469B RID: 18075 RVA: 0x00108B70 File Offset: 0x00106D70
		public List<SceneActor> GetActors()
		{
			return this.Actors;
		}

		// Token: 0x0600469C RID: 18076 RVA: 0x00108B88 File Offset: 0x00106D88
		public void ClearActors()
		{
			foreach (SceneActor sceneActor in this.Actors)
			{
				sceneActor.Dispose();
			}
			this.Actors.Clear();
		}

		// Token: 0x0600469D RID: 18077 RVA: 0x00108BEC File Offset: 0x00106DEC
		public void OffsetOrigin(Vector3 offset)
		{
			Vector3 offset2 = offset - this.Origin;
			foreach (SceneActor sceneActor in this.Actors)
			{
				sceneActor.Track.OffsetPositions(offset2);
			}
			this.Origin = offset;
		}

		// Token: 0x0600469E RID: 18078 RVA: 0x00108C60 File Offset: 0x00106E60
		public void Rotate(Vector3 rotation, Vector3 origin)
		{
			bool flag = origin.IsNaN();
			if (flag)
			{
				origin = this.Origin;
			}
			Quaternion quaternion = Quaternion.CreateFromYawPitchRoll(rotation.Yaw, rotation.Pitch, rotation.Roll);
			Matrix matrix;
			Matrix.CreateFromQuaternion(ref quaternion, out matrix);
			for (int i = 0; i < this.Actors.Count; i++)
			{
				this.Actors[i].Track.RotatePath(rotation, origin);
			}
			this.Origin = Vector3.Transform(this.Origin - origin, matrix) + origin;
			this.OriginLook.Y = MathHelper.WrapAngle(this.OriginLook.Y + rotation.Y);
		}

		// Token: 0x0600469F RID: 18079 RVA: 0x00108D20 File Offset: 0x00106F20
		public string GetNextActorName(string actorName)
		{
			string text = actorName;
			bool flag = string.IsNullOrEmpty(text);
			if (flag)
			{
				text = "actor";
			}
			bool flag2 = this.HasActor(text);
			if (flag2)
			{
				for (int i = 1; i < 999999; i++)
				{
					string text2 = string.Format("{0}{1}", text, i);
					bool flag3 = !this.HasActor(text2);
					if (flag3)
					{
						return text2;
					}
				}
			}
			return text;
		}

		// Token: 0x060046A0 RID: 18080 RVA: 0x00108D9C File Offset: 0x00106F9C
		private void RunStartupCommands()
		{
			foreach (string text in this._startupCommands)
			{
				bool flag = text.StartsWith(".");
				if (flag)
				{
					this._gameInstance.ExecuteCommand(text);
				}
				else
				{
					bool flag2 = text.StartsWith("/");
					if (flag2)
					{
						this._gameInstance.Chat.SendCommand(text.Substring(1), Array.Empty<object>());
					}
				}
			}
		}

		// Token: 0x060046A1 RID: 18081 RVA: 0x00108E3C File Offset: 0x0010703C
		public string Serialize(JsonSerializerSettings serializerSettings)
		{
			return JsonConvert.SerializeObject(this, 1, serializerSettings);
		}

		// Token: 0x060046A2 RID: 18082 RVA: 0x00108E58 File Offset: 0x00107058
		public static MachinimaScene Deserialize(string jsonString, GameInstance gameInstance, JsonSerializerSettings serializerSettings)
		{
			MachinimaScene result;
			try
			{
				JObject jobject = JObject.Parse(jsonString);
				MachinimaScene.UpdateSceneData(ref jobject);
				MachinimaScene machinimaScene = jobject.ToObject<MachinimaScene>(JsonSerializer.Create(serializerSettings));
				machinimaScene.Initialize(gameInstance);
				result = machinimaScene;
			}
			catch (Exception ex)
			{
				gameInstance.Chat.Error("Error deserializing scene data! " + ex.Message);
				Trace.WriteLine(ex);
				result = null;
			}
			return result;
		}

		// Token: 0x060046A3 RID: 18083 RVA: 0x00108ECC File Offset: 0x001070CC
		public byte[] ToCompressedByteArray(JsonSerializerSettings serializerSettings)
		{
			string s = this.Serialize(serializerSettings);
			byte[] bytes = Encoding.Default.GetBytes(s);
			int num = bytes.Length;
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (ZLibStream zlibStream = new ZLibStream(memoryStream, 1))
				{
					zlibStream.Write(bytes, 0, bytes.Length);
				}
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x060046A4 RID: 18084 RVA: 0x00108F60 File Offset: 0x00107160
		public static MachinimaScene FromCompressedByteArray(byte[] compressedByteArray, GameInstance gameInstance, JsonSerializerSettings serializerSettings)
		{
			MemoryStream memoryStream = new MemoryStream();
			byte[] bytes;
			using (MemoryStream memoryStream2 = new MemoryStream(compressedByteArray))
			{
				using (ZLibStream zlibStream = new ZLibStream(memoryStream2, 0))
				{
					zlibStream.CopyTo(memoryStream);
					zlibStream.Close();
					memoryStream.Position = 0L;
					bytes = memoryStream.ToArray();
				}
			}
			string @string = Encoding.Default.GetString(bytes);
			return MachinimaScene.Deserialize(@string, gameInstance, serializerSettings);
		}

		// Token: 0x060046A5 RID: 18085 RVA: 0x00108FFC File Offset: 0x001071FC
		public static void UpdateSceneData(ref JObject sceneObject)
		{
			JToken jtoken = sceneObject["Version"];
			int num = (jtoken != null) ? jtoken.ToObject<int>() : 0;
			int num2 = num;
			int num3 = num2;
			if (num3 == 0)
			{
				foreach (JToken jtoken2 in sceneObject["Actors"])
				{
					JToken jtoken3 = jtoken2["Track"];
					foreach (JToken jtoken4 in ((jtoken3 != null) ? jtoken3["Keyframes"] : null))
					{
						Vector3 vector = jtoken4["Settings"]["Look"].ToObject<Vector3>();
						vector.Y -= jtoken4["Settings"]["Rotation"].ToObject<Vector3>().Y;
						jtoken4["Settings"]["Look"] = JObject.FromObject(vector);
					}
				}
			}
			sceneObject["Version"] = 1;
		}

		// Token: 0x04002383 RID: 9091
		[JsonProperty(PropertyName = "Version")]
		public const int FILE_VERSION = 1;

		// Token: 0x04002384 RID: 9092
		private GameInstance _gameInstance;

		// Token: 0x04002385 RID: 9093
		[JsonProperty(PropertyName = "Name")]
		[CoherentProperty("name")]
		public string Name;

		// Token: 0x04002387 RID: 9095
		private Vector3 _origin = Vector3.NaN;

		// Token: 0x04002388 RID: 9096
		[JsonProperty(PropertyName = "OriginLook")]
		[CoherentProperty("originLook")]
		public Vector3 OriginLook = Vector3.Zero;

		// Token: 0x04002389 RID: 9097
		[JsonProperty(PropertyName = "StartupCommands")]
		private List<string> _startupCommands = new List<string>();

		// Token: 0x0400238A RID: 9098
		private bool _isActive;

		// Token: 0x0400238B RID: 9099
		private float _lastFrame;
	}
}
