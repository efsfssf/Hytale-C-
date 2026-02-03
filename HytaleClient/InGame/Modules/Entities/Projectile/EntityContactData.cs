using System;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x02000958 RID: 2392
	internal class EntityContactData
	{
		// Token: 0x06004ACF RID: 19151 RVA: 0x00131A9B File Offset: 0x0012FC9B
		public void Assign(Vector3 collisionPoint, float collisionStart, float collisionEnd, Entity entityReference, string collisionDetailName)
		{
			this.CollisionPoint = collisionPoint;
			this.CollisionStart = collisionStart;
			this.CollisionEnd = collisionEnd;
			this.EntityReference = entityReference;
			this.CollisionDetailName = collisionDetailName;
		}

		// Token: 0x06004AD0 RID: 19152 RVA: 0x00131AC3 File Offset: 0x0012FCC3
		public void Clear()
		{
			this.EntityReference = null;
		}

		// Token: 0x04002679 RID: 9849
		public Vector3 CollisionPoint;

		// Token: 0x0400267A RID: 9850
		public float CollisionStart;

		// Token: 0x0400267B RID: 9851
		public float CollisionEnd;

		// Token: 0x0400267C RID: 9852
		public Entity EntityReference;

		// Token: 0x0400267D RID: 9853
		public string CollisionDetailName;
	}
}
