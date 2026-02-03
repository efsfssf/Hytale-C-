using System;
using System.Collections.Generic;
using System.Linq;
using Coherent.UI.Binding;
using HytaleClient.InGame.Modules.Machinima.Events;
using HytaleClient.InGame.Modules.Machinima.Settings;
using HytaleClient.Math;
using Newtonsoft.Json;

namespace HytaleClient.InGame.Modules.Machinima.Track
{
	// Token: 0x02000912 RID: 2322
	[CoherentType]
	internal class TrackKeyframe
	{
		// Token: 0x17001161 RID: 4449
		// (get) Token: 0x060046DD RID: 18141 RVA: 0x0010B788 File Offset: 0x00109988
		// (set) Token: 0x060046DE RID: 18142 RVA: 0x0010B790 File Offset: 0x00109990
		[CoherentProperty("settings")]
		public Dictionary<string, IKeyframeSetting> Settings { get; private set; }

		// Token: 0x17001162 RID: 4450
		// (get) Token: 0x060046DF RID: 18143 RVA: 0x0010B799 File Offset: 0x00109999
		// (set) Token: 0x060046E0 RID: 18144 RVA: 0x0010B7A1 File Offset: 0x001099A1
		public List<KeyframeEvent> Events { get; private set; }

		// Token: 0x17001163 RID: 4451
		// (get) Token: 0x060046E1 RID: 18145 RVA: 0x0010B7AC File Offset: 0x001099AC
		[JsonIgnore]
		[CoherentProperty("events")]
		public string[] CoherentEvents
		{
			get
			{
				return Enumerable.ToArray<string>(Enumerable.Select<KeyframeEvent, string>(this.Events, (KeyframeEvent evt) => evt.ToCoherentJson()));
			}
		}

		// Token: 0x060046E2 RID: 18146 RVA: 0x0010B7F0 File Offset: 0x001099F0
		public TrackKeyframe(float frame)
		{
			this.Settings = new Dictionary<string, IKeyframeSetting>();
			this.Events = new List<KeyframeEvent>();
			this.Frame = frame;
			this.Id = TrackKeyframe.NextId++;
		}

		// Token: 0x060046E3 RID: 18147 RVA: 0x0010B842 File Offset: 0x00109A42
		public void AddSetting(IKeyframeSetting setting)
		{
			this.Settings[setting.Name] = setting;
		}

		// Token: 0x060046E4 RID: 18148 RVA: 0x0010B858 File Offset: 0x00109A58
		public void RemoveSetting(string name)
		{
			this.Settings.Remove(name);
		}

		// Token: 0x060046E5 RID: 18149 RVA: 0x0010B868 File Offset: 0x00109A68
		public KeyframeSetting<T> GetSetting<T>(string name)
		{
			bool flag = this.Settings.ContainsKey(name);
			KeyframeSetting<T> result;
			if (flag)
			{
				result = (KeyframeSetting<T>)this.Settings[name];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060046E6 RID: 18150 RVA: 0x0010B8A0 File Offset: 0x00109AA0
		public KeyframeSetting<T> GetSetting<T>(KeyframeSettingType settingType)
		{
			bool flag = this.Settings.ContainsKey(settingType.ToString());
			KeyframeSetting<T> result;
			if (flag)
			{
				result = (KeyframeSetting<T>)this.Settings[settingType.ToString()];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060046E7 RID: 18151 RVA: 0x0010B8F0 File Offset: 0x00109AF0
		public void AddEvent(KeyframeEvent keyframeEvent)
		{
			bool flag = !keyframeEvent.AllowDuplicates;
			if (flag)
			{
				for (int i = 0; i < this.Events.Count; i++)
				{
					bool flag2 = this.Events[i].Name == keyframeEvent.Name;
					if (flag2)
					{
						throw new TrackKeyframe.DuplicateKeyframeEvent();
					}
				}
			}
			this.Events.Add(keyframeEvent);
		}

		// Token: 0x060046E8 RID: 18152 RVA: 0x0010B95C File Offset: 0x00109B5C
		public void RemoveEvent(int eventId)
		{
			int num = -1;
			for (int i = 0; i < this.Events.Count; i++)
			{
				bool flag = this.Events[i].Id == eventId;
				if (flag)
				{
					num = i;
					break;
				}
			}
			bool flag2 = num == -1;
			if (!flag2)
			{
				this.Events.RemoveAt(num);
			}
		}

		// Token: 0x060046E9 RID: 18153 RVA: 0x0010B9C0 File Offset: 0x00109BC0
		public KeyframeEvent GetEvent(int eventId)
		{
			for (int i = 0; i < this.Events.Count; i++)
			{
				bool flag = this.Events[i].Id == eventId;
				if (flag)
				{
					return this.Events[i];
				}
			}
			return null;
		}

		// Token: 0x060046EA RID: 18154 RVA: 0x0010BA18 File Offset: 0x00109C18
		public bool HasEvent(int eventId)
		{
			return this.GetEvent(eventId) != null;
		}

		// Token: 0x060046EB RID: 18155 RVA: 0x0010BA34 File Offset: 0x00109C34
		public void TriggerEvents(GameInstance gameInstance, SceneTrack track)
		{
			bool flag = this.Events.Count == 0;
			if (!flag)
			{
				for (int i = 0; i < this.Events.Count; i++)
				{
					this.Events[i].Execute(gameInstance, track);
				}
			}
		}

		// Token: 0x060046EC RID: 18156 RVA: 0x0010BA88 File Offset: 0x00109C88
		public TrackKeyframe Clone()
		{
			TrackKeyframe trackKeyframe = new TrackKeyframe(this.Frame);
			foreach (KeyValuePair<string, IKeyframeSetting> keyValuePair in this.Settings)
			{
				trackKeyframe.AddSetting(keyValuePair.Value.Clone());
			}
			foreach (KeyframeEvent keyframeEvent in this.Events)
			{
				trackKeyframe.AddEvent(keyframeEvent.Clone());
			}
			return trackKeyframe;
		}

		// Token: 0x040023A8 RID: 9128
		private static int NextId;

		// Token: 0x040023A9 RID: 9129
		public static readonly BoundingBox KeyframeBox = new BoundingBox(new Vector3(-0.125f, -0.125f, -0.125f), new Vector3(0.125f, 0.125f, 0.125f));

		// Token: 0x040023AA RID: 9130
		public static readonly BoundingBox PathBox = new BoundingBox(new Vector3(-0.0625f, -0.0625f, -0.0625f), new Vector3(0.0625f, 0.0625f, 0.0625f));

		// Token: 0x040023AB RID: 9131
		[JsonIgnore]
		[CoherentProperty("id")]
		public readonly int Id;

		// Token: 0x040023AE RID: 9134
		[CoherentProperty("frame")]
		public float Frame = 0f;

		// Token: 0x02000E02 RID: 3586
		public class DuplicateKeyframeEvent : Exception
		{
		}
	}
}
