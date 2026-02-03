using System;
using HytaleClient.Protocol;

namespace HytaleClient.Data.Entities
{
	// Token: 0x02000B0D RID: 2829
	public class ClientRepulsionConfig
	{
		// Token: 0x06005895 RID: 22677 RVA: 0x001B0A33 File Offset: 0x001AEC33
		public ClientRepulsionConfig()
		{
		}

		// Token: 0x06005896 RID: 22678 RVA: 0x001B0A3D File Offset: 0x001AEC3D
		public ClientRepulsionConfig(RepulsionConfig repulsionConfig)
		{
			this.Radius = repulsionConfig.Radius;
			this.MinForce = repulsionConfig.MinForce;
			this.MaxForce = repulsionConfig.MaxForce;
		}

		// Token: 0x06005897 RID: 22679 RVA: 0x001B0A6C File Offset: 0x001AEC6C
		public ClientRepulsionConfig Clone()
		{
			return new ClientRepulsionConfig
			{
				Radius = this.Radius,
				MinForce = this.MinForce,
				MaxForce = this.MaxForce
			};
		}

		// Token: 0x0400371D RID: 14109
		public const int NoRepulsionConfigIndex = -1;

		// Token: 0x0400371E RID: 14110
		public float Radius;

		// Token: 0x0400371F RID: 14111
		public float MinForce;

		// Token: 0x04003720 RID: 14112
		public float MaxForce;
	}
}
