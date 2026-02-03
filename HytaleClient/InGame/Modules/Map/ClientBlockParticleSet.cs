using System;
using System.Collections.Generic;
using HytaleClient.Graphics;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Map
{
	// Token: 0x02000909 RID: 2313
	internal class ClientBlockParticleSet
	{
		// Token: 0x0600457E RID: 17790 RVA: 0x000F4248 File Offset: 0x000F2448
		public ClientBlockParticleSet Clone()
		{
			return new ClientBlockParticleSet
			{
				Color = this.Color,
				Scale = this.Scale,
				PositionOffset = this.PositionOffset,
				RotationOffset = this.RotationOffset,
				ParticleSystemIds = new Dictionary<ClientBlockParticleEvent, string>(this.ParticleSystemIds)
			};
		}

		// Token: 0x040022EC RID: 8940
		public UInt32Color Color;

		// Token: 0x040022ED RID: 8941
		public float Scale;

		// Token: 0x040022EE RID: 8942
		public Vector3 PositionOffset;

		// Token: 0x040022EF RID: 8943
		public Quaternion RotationOffset;

		// Token: 0x040022F0 RID: 8944
		public Dictionary<ClientBlockParticleEvent, string> ParticleSystemIds;
	}
}
