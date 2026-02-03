using System;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x02000953 RID: 2387
	internal class BoxCollisionData : BasicCollisionData
	{
		// Token: 0x06004AB6 RID: 19126 RVA: 0x001313AC File Offset: 0x0012F5AC
		public void SetEnd(float collisionEnd, Vector3 collisionNormal)
		{
			this.CollisionEnd = collisionEnd;
			this.CollisionNormal = collisionNormal;
		}

		// Token: 0x0400265B RID: 9819
		public float CollisionEnd;

		// Token: 0x0400265C RID: 9820
		public Vector3 CollisionNormal;
	}
}
