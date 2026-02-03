using System;
using System.Collections.Generic;
using HytaleClient.Protocol;

namespace HytaleClient.Data.Weather
{
	// Token: 0x02000ACD RID: 2765
	internal class ClientWorldEnvironment
	{
		// Token: 0x17001362 RID: 4962
		// (get) Token: 0x0600574F RID: 22351 RVA: 0x001A7232 File Offset: 0x001A5432
		// (set) Token: 0x06005750 RID: 22352 RVA: 0x001A723A File Offset: 0x001A543A
		public Dictionary<int, FluidParticle> FluidParticles { get; private set; } = new Dictionary<int, FluidParticle>();

		// Token: 0x06005751 RID: 22353 RVA: 0x001A7243 File Offset: 0x001A5443
		public ClientWorldEnvironment()
		{
		}

		// Token: 0x06005752 RID: 22354 RVA: 0x001A7260 File Offset: 0x001A5460
		public ClientWorldEnvironment(WorldEnvironment worldEnvironment)
		{
			this.Id = worldEnvironment.Id;
			bool flag = worldEnvironment.WaterTint != null;
			if (flag)
			{
				this.WaterTint = ((int)((byte)worldEnvironment.WaterTint.Red) << 16 | (int)((byte)worldEnvironment.WaterTint.Green) << 8 | (int)((byte)worldEnvironment.WaterTint.Blue));
			}
			bool flag2 = worldEnvironment.FluidParticles != null;
			if (flag2)
			{
				this.FluidParticles = new Dictionary<int, FluidParticle>(worldEnvironment.FluidParticles);
			}
		}

		// Token: 0x06005753 RID: 22355 RVA: 0x001A72F0 File Offset: 0x001A54F0
		public ClientWorldEnvironment Clone()
		{
			return new ClientWorldEnvironment
			{
				Id = this.Id,
				WaterTint = this.WaterTint,
				FluidParticles = new Dictionary<int, FluidParticle>(this.FluidParticles)
			};
		}

		// Token: 0x040034FB RID: 13563
		public string Id;

		// Token: 0x040034FC RID: 13564
		public int WaterTint = -1;
	}
}
