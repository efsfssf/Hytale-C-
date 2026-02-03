using System;
using HytaleClient.Protocol;

namespace HytaleClient.Data.Entities.Initializers
{
	// Token: 0x02000B0E RID: 2830
	public class ClientHitboxCollisionConfigInitializer
	{
		// Token: 0x06005898 RID: 22680 RVA: 0x001B0AAC File Offset: 0x001AECAC
		public static void Initialize(HitboxCollisionConfig hitboxCollisionConfig, ref ClientHitboxCollisionConfig clientHitboxCollisionConfig)
		{
			HitboxCollisionConfig.CollisionType collisionType_ = hitboxCollisionConfig.CollisionType_;
			HitboxCollisionConfig.CollisionType collisionType = collisionType_;
			if (collisionType != null)
			{
				clientHitboxCollisionConfig.CollisionType = ClientHitboxCollisionConfig.ClientCollisionType.Soft;
			}
			else
			{
				clientHitboxCollisionConfig.CollisionType = ClientHitboxCollisionConfig.ClientCollisionType.Hard;
			}
			clientHitboxCollisionConfig.SoftCollisionOffsetRatio = hitboxCollisionConfig.SoftCollisionOffsetRatio;
		}
	}
}
