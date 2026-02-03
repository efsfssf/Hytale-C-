using System;
using HytaleClient.Protocol;

namespace HytaleClient.Data.Entities.Initializers
{
	// Token: 0x02000B10 RID: 2832
	public class ClientRepulsionConfigInitializer
	{
		// Token: 0x0600589C RID: 22684 RVA: 0x001B0CF7 File Offset: 0x001AEEF7
		public static void Initialize(RepulsionConfig repulsionConfig, ref ClientRepulsionConfig clientRepulsionConfig)
		{
			clientRepulsionConfig.Radius = repulsionConfig.Radius;
			clientRepulsionConfig.MaxForce = repulsionConfig.MaxForce;
			clientRepulsionConfig.MinForce = repulsionConfig.MinForce;
		}
	}
}
