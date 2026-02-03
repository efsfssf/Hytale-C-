using System;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Machinima.Actors;
using HytaleClient.InGame.Modules.Machinima.Track;
using HytaleClient.Protocol;
using Newtonsoft.Json;

namespace HytaleClient.InGame.Modules.Machinima.Events
{
	// Token: 0x0200091F RID: 2335
	internal class AnimationEvent : KeyframeEvent
	{
		// Token: 0x0600472D RID: 18221 RVA: 0x0010D061 File Offset: 0x0010B261
		public AnimationEvent(string animationId, AnimationSlot slot = 0)
		{
			this.AnimationId = animationId;
			this.Slot = slot;
			base.AllowDuplicates = true;
			base.Initialized = true;
		}

		// Token: 0x0600472E RID: 18222 RVA: 0x0010D090 File Offset: 0x0010B290
		public override void Execute(GameInstance gameInstance, SceneTrack track)
		{
			bool flag = track.Parent is EntityActor;
			if (flag)
			{
				Entity entity = ((EntityActor)track.Parent).GetEntity();
				bool flag2 = entity != null;
				if (flag2)
				{
					string text = (string.IsNullOrWhiteSpace(this.AnimationId) || this.AnimationId.ToLower() == "off") ? null : this.AnimationId;
					gameInstance.InjectPacket(new PlayAnimation(entity.NetworkId, null, text, this.Slot));
				}
			}
		}

		// Token: 0x0600472F RID: 18223 RVA: 0x0010D118 File Offset: 0x0010B318
		public override string ToString()
		{
			return string.Format("#{0} AnimationEvent [Id: '{1}', Slot: '{2}']", this.Id, this.AnimationId, this.Slot);
		}

		// Token: 0x040023CB RID: 9163
		[JsonProperty("AnimationId")]
		public readonly string AnimationId;

		// Token: 0x040023CC RID: 9164
		[JsonProperty("AnimationSlot")]
		public readonly AnimationSlot Slot = 0;
	}
}
