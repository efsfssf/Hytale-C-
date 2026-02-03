using System;
using HytaleClient.InGame.Modules.Machinima.Track;
using HytaleClient.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HytaleClient.InGame.Modules.Machinima.Events
{
	// Token: 0x02000922 RID: 2338
	internal abstract class KeyframeEvent
	{
		// Token: 0x1700116E RID: 4462
		// (get) Token: 0x06004736 RID: 18230 RVA: 0x0010D279 File Offset: 0x0010B479
		[JsonIgnore]
		public string Name
		{
			get
			{
				return base.GetType().Name;
			}
		}

		// Token: 0x1700116F RID: 4463
		// (get) Token: 0x06004737 RID: 18231 RVA: 0x0010D286 File Offset: 0x0010B486
		// (set) Token: 0x06004738 RID: 18232 RVA: 0x0010D28E File Offset: 0x0010B48E
		[JsonIgnore]
		public bool AllowDuplicates { get; protected set; }

		// Token: 0x17001170 RID: 4464
		// (get) Token: 0x06004739 RID: 18233 RVA: 0x0010D297 File Offset: 0x0010B497
		// (set) Token: 0x0600473A RID: 18234 RVA: 0x0010D29F File Offset: 0x0010B49F
		[JsonIgnore]
		public bool Initialized { get; protected set; }

		// Token: 0x0600473B RID: 18235
		public abstract void Execute(GameInstance gameInstance, SceneTrack track);

		// Token: 0x0600473C RID: 18236 RVA: 0x0010D2A8 File Offset: 0x0010B4A8
		public virtual void Initialize(MachinimaScene scene)
		{
			this.Initialized = true;
		}

		// Token: 0x0600473D RID: 18237 RVA: 0x0010D2B3 File Offset: 0x0010B4B3
		public KeyframeEvent()
		{
			this.Id = KeyframeEvent.NextId++;
		}

		// Token: 0x0600473E RID: 18238 RVA: 0x0010D2D0 File Offset: 0x0010B4D0
		public JObject ToJsonObject()
		{
			JObject jobject = new JObject();
			jobject.Add(this.Name, JToken.FromObject(this));
			return jobject;
		}

		// Token: 0x0600473F RID: 18239 RVA: 0x0010D2FC File Offset: 0x0010B4FC
		public string ToCoherentJson()
		{
			JObject jobject = this.ToJsonObject();
			jobject = (JObject)jobject[((JProperty)jobject.First).Name];
			jobject["Id"] = this.Id;
			jobject["Name"] = this.Name;
			jobject["AllowDuplicates"] = this.AllowDuplicates;
			return jobject.ToString();
		}

		// Token: 0x06004740 RID: 18240 RVA: 0x0010D37C File Offset: 0x0010B57C
		public static KeyframeEvent ConvertJsonObject(JObject jsonData)
		{
			string name = ((JProperty)jsonData.First).Name;
			string text = name;
			string a = text;
			KeyframeEvent result;
			if (!(a == "AnimationEvent"))
			{
				if (!(a == "CommandEvent"))
				{
					if (!(a == "ParticleEvent"))
					{
						if (!(a == "TargetEvent"))
						{
							if (!(a == "CameraEvent"))
							{
								result = null;
							}
							else
							{
								bool cameraState = Extensions.Value<bool>(jsonData["CameraEvent"]["CameraState"]);
								result = new CameraEvent(cameraState);
							}
						}
						else
						{
							string actorName = Extensions.Value<string>(jsonData["TargetEvent"]["TargetName"]);
							result = new TargetEvent(actorName);
						}
					}
					else
					{
						string particleSystemId = Extensions.Value<string>(jsonData["ParticleEvent"]["ParticleId"]);
						result = new ParticleEvent(particleSystemId);
					}
				}
				else
				{
					string command = Extensions.Value<string>(jsonData["CommandEvent"]["Command"]);
					result = new CommandEvent(command);
				}
			}
			else
			{
				string animationId = Extensions.Value<string>(jsonData["AnimationEvent"]["AnimationId"]);
				AnimationSlot slot = Extensions.Value<int>(jsonData["AnimationEvent"]["AnimationSlot"]);
				result = new AnimationEvent(animationId, slot);
			}
			return result;
		}

		// Token: 0x06004741 RID: 18241 RVA: 0x0010D4DC File Offset: 0x0010B6DC
		public virtual KeyframeEvent Clone()
		{
			return KeyframeEvent.ConvertJsonObject(this.ToJsonObject());
		}

		// Token: 0x040023CF RID: 9167
		private static int NextId;

		// Token: 0x040023D0 RID: 9168
		[JsonIgnore]
		public readonly int Id;
	}
}
