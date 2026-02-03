using System;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.Items
{
	// Token: 0x02000AF6 RID: 2806
	public class ClientItemPullbackConfig
	{
		// Token: 0x0600583F RID: 22591 RVA: 0x001AC578 File Offset: 0x001AA778
		public ClientItemPullbackConfig(ItemPullbackConfiguration properties)
		{
			bool flag = properties.LeftOffsetOverride != null;
			if (flag)
			{
				this.LeftOffsetOverride = new Vector3?(new Vector3(properties.LeftOffsetOverride.X, properties.LeftOffsetOverride.Y, properties.LeftOffsetOverride.Z));
			}
			bool flag2 = properties.LeftRotationOverride != null;
			if (flag2)
			{
				this.LeftRotationOverride = new Vector3?(new Vector3(properties.LeftRotationOverride.X, properties.LeftRotationOverride.Y, properties.LeftRotationOverride.Z));
			}
			bool flag3 = properties.RightOffsetOverride != null;
			if (flag3)
			{
				this.RightOffsetOverride = new Vector3?(new Vector3(properties.RightOffsetOverride.X, properties.RightOffsetOverride.Y, properties.RightOffsetOverride.Z));
			}
			bool flag4 = properties.RightRotationOverride != null;
			if (flag4)
			{
				this.RightRotationOverride = new Vector3?(new Vector3(properties.RightRotationOverride.X, properties.RightRotationOverride.Y, properties.RightRotationOverride.Z));
			}
		}

		// Token: 0x06005840 RID: 22592 RVA: 0x001AC685 File Offset: 0x001AA885
		public ClientItemPullbackConfig()
		{
		}

		// Token: 0x040036BD RID: 14013
		public Vector3? LeftOffsetOverride;

		// Token: 0x040036BE RID: 14014
		public Vector3? LeftRotationOverride;

		// Token: 0x040036BF RID: 14015
		public Vector3? RightOffsetOverride;

		// Token: 0x040036C0 RID: 14016
		public Vector3? RightRotationOverride;
	}
}
