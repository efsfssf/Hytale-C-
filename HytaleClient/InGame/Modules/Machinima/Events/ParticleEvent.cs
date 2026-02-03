using System;
using HytaleClient.Graphics.Particles;
using HytaleClient.InGame.Modules.Machinima.Track;
using HytaleClient.Math;
using Newtonsoft.Json;

namespace HytaleClient.InGame.Modules.Machinima.Events
{
	// Token: 0x02000923 RID: 2339
	internal class ParticleEvent : KeyframeEvent
	{
		// Token: 0x06004742 RID: 18242 RVA: 0x0010D4F9 File Offset: 0x0010B6F9
		public ParticleEvent(string particleSystemId)
		{
			this.ParticleSystemId = particleSystemId;
			base.AllowDuplicates = true;
			base.Initialized = true;
		}

		// Token: 0x06004743 RID: 18243 RVA: 0x0010D51C File Offset: 0x0010B71C
		public override void Execute(GameInstance gameInstance, SceneTrack track)
		{
			bool flag = this._particleSystemProxy != null;
			if (flag)
			{
				this._particleSystemProxy.Expire(false);
				this._particleSystemProxy = null;
			}
			bool flag2 = this._particleSystemProxy == null;
			if (flag2)
			{
				bool flag3 = !gameInstance.ParticleSystemStoreModule.TrySpawnSystem(this.ParticleSystemId, out this._particleSystemProxy, false, true);
				if (flag3)
				{
					return;
				}
			}
			this._particleSystemProxy.Position = track.Parent.Position;
			this._particleSystemProxy.Rotation = Quaternion.CreateFromYawPitchRoll(track.Parent.Rotation.Yaw, track.Parent.Rotation.Pitch, track.Parent.Rotation.Roll);
			track.AddParticleSystem(this._particleSystemProxy);
		}

		// Token: 0x06004744 RID: 18244 RVA: 0x0010D5E4 File Offset: 0x0010B7E4
		public override string ToString()
		{
			return string.Format("#{0} - ParticleEvent [ParticleId: '{1}']", this.Id, this.ParticleSystemId);
		}

		// Token: 0x040023D3 RID: 9171
		[JsonProperty("ParticleId")]
		public readonly string ParticleSystemId;

		// Token: 0x040023D4 RID: 9172
		private ParticleSystemProxy _particleSystemProxy;
	}
}
