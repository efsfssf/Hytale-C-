using System;
using HytaleClient.InGame.Modules.Machinima.Actors;
using HytaleClient.InGame.Modules.Machinima.Track;
using Newtonsoft.Json;

namespace HytaleClient.InGame.Modules.Machinima.Events
{
	// Token: 0x02000925 RID: 2341
	internal class TargetEvent : KeyframeEvent
	{
		// Token: 0x06004748 RID: 18248 RVA: 0x0010D6D8 File Offset: 0x0010B8D8
		public TargetEvent(SceneActor targetActor)
		{
			this._targetActor = targetActor;
			this.TargetName = ((targetActor == null) ? "" : targetActor.Name);
			base.AllowDuplicates = false;
			base.Initialized = true;
		}

		// Token: 0x06004749 RID: 18249 RVA: 0x0010D70F File Offset: 0x0010B90F
		public TargetEvent(string actorName)
		{
			this.TargetName = actorName;
			base.AllowDuplicates = false;
		}

		// Token: 0x0600474A RID: 18250 RVA: 0x0010D728 File Offset: 0x0010B928
		public override void Initialize(MachinimaScene scene)
		{
			bool initialized = base.Initialized;
			if (!initialized)
			{
				bool flag = this.TargetName == "";
				if (flag)
				{
					this._targetActor = null;
				}
				else
				{
					this._targetActor = scene.GetActor(this.TargetName);
				}
				base.Initialized = true;
			}
		}

		// Token: 0x0600474B RID: 18251 RVA: 0x0010D77C File Offset: 0x0010B97C
		public SceneActor GetTarget()
		{
			return this._targetActor;
		}

		// Token: 0x0600474C RID: 18252 RVA: 0x0010D794 File Offset: 0x0010B994
		public override void Execute(GameInstance gameInstance, SceneTrack track)
		{
			bool flag = track.Parent != null && track.Parent != null;
			if (flag)
			{
				track.Parent.SetLookTarget(this._targetActor);
			}
		}

		// Token: 0x0600474D RID: 18253 RVA: 0x0010D7D0 File Offset: 0x0010B9D0
		public override string ToString()
		{
			return string.Format("#{0} - TargetEvent [Target: '{1}']", this.Id, this.TargetName);
		}

		// Token: 0x040023D9 RID: 9177
		private SceneActor _targetActor;

		// Token: 0x040023DA RID: 9178
		[JsonProperty("TargetName")]
		public string TargetName;
	}
}
