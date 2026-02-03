using System;
using HytaleClient.Protocol;

namespace HytaleClient.Data.Entities
{
	// Token: 0x02000B0B RID: 2827
	public class ClientHitboxCollisionConfig
	{
		// Token: 0x0600588B RID: 22667 RVA: 0x001B0487 File Offset: 0x001AE687
		public ClientHitboxCollisionConfig()
		{
		}

		// Token: 0x0600588C RID: 22668 RVA: 0x001B0494 File Offset: 0x001AE694
		public ClientHitboxCollisionConfig(HitboxCollisionConfig hitboxCollisionConfig)
		{
			HitboxCollisionConfig.CollisionType collisionType_ = hitboxCollisionConfig.CollisionType_;
			HitboxCollisionConfig.CollisionType collisionType = collisionType_;
			if (collisionType != null)
			{
				this.CollisionType = ClientHitboxCollisionConfig.ClientCollisionType.Soft;
			}
			else
			{
				this.CollisionType = ClientHitboxCollisionConfig.ClientCollisionType.Hard;
			}
			this.SoftCollisionOffsetRatio = hitboxCollisionConfig.SoftCollisionOffsetRatio;
		}

		// Token: 0x0600588D RID: 22669 RVA: 0x001B04D8 File Offset: 0x001AE6D8
		public ClientHitboxCollisionConfig Clone()
		{
			return new ClientHitboxCollisionConfig
			{
				CollisionType = this.CollisionType,
				SoftCollisionOffsetRatio = this.SoftCollisionOffsetRatio
			};
		}

		// Token: 0x04003706 RID: 14086
		public const int NoHitboxCollisionConfigIndex = -1;

		// Token: 0x04003707 RID: 14087
		public ClientHitboxCollisionConfig.ClientCollisionType CollisionType;

		// Token: 0x04003708 RID: 14088
		public float SoftCollisionOffsetRatio;

		// Token: 0x02000F25 RID: 3877
		public enum ClientCollisionType
		{
			// Token: 0x04004A40 RID: 19008
			Hard,
			// Token: 0x04004A41 RID: 19009
			Soft
		}
	}
}
