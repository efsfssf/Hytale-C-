using System;
using HytaleClient.InGame.Modules.Machinima.Actors;
using HytaleClient.InGame.Modules.Machinima.Track;
using Newtonsoft.Json;

namespace HytaleClient.InGame.Modules.Machinima.Events
{
	// Token: 0x02000920 RID: 2336
	internal class CameraEvent : KeyframeEvent
	{
		// Token: 0x06004730 RID: 18224 RVA: 0x0010D150 File Offset: 0x0010B350
		public CameraEvent(bool cameraState)
		{
			this.CameraState = cameraState;
			base.AllowDuplicates = false;
			base.Initialized = true;
		}

		// Token: 0x06004731 RID: 18225 RVA: 0x0010D174 File Offset: 0x0010B374
		public override void Execute(GameInstance gameInstance, SceneTrack track)
		{
			bool flag = track.Parent is CameraActor;
			if (flag)
			{
				(track.Parent as CameraActor).SetState(this.CameraState);
			}
		}

		// Token: 0x06004732 RID: 18226 RVA: 0x0010D1B0 File Offset: 0x0010B3B0
		public override string ToString()
		{
			return string.Format("#{0} - CameraEvent [CameraState: {1}]", this.Id, this.CameraState);
		}

		// Token: 0x040023CD RID: 9165
		[JsonProperty("CameraState")]
		public readonly bool CameraState;
	}
}
