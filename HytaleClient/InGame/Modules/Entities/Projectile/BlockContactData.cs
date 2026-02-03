using System;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x0200094D RID: 2381
	internal class BlockContactData
	{
		// Token: 0x06004A7E RID: 19070 RVA: 0x00130102 File Offset: 0x0012E302
		public void Clear()
		{
		}

		// Token: 0x06004A7F RID: 19071 RVA: 0x00130105 File Offset: 0x0012E305
		public void Assign(BlockContactData other)
		{
			this.Assign(other, other.Damage, other.IsSubmergeFluid);
		}

		// Token: 0x06004A80 RID: 19072 RVA: 0x0013011C File Offset: 0x0012E31C
		public void Assign(BlockContactData other, int damage, bool isSubmergedFluid)
		{
			this.CollisionNormal = other.CollisionNormal;
			this.CollisionPoint = other.CollisionPoint;
			this.CollisionStart = other.CollisionStart;
			this.CollisionEnd = other.CollisionEnd;
			this.OnGround = other.OnGround;
			this.Overlapping = other.Overlapping;
			this.SetDamageAndSubmerged(damage, isSubmergedFluid);
		}

		// Token: 0x06004A81 RID: 19073 RVA: 0x0013017B File Offset: 0x0012E37B
		public void SetDamageAndSubmerged(int damage, bool isSubmerge)
		{
			this.Damage = damage;
			this.IsSubmergeFluid = isSubmerge;
		}

		// Token: 0x0400263A RID: 9786
		public Vector3 CollisionNormal;

		// Token: 0x0400263B RID: 9787
		public Vector3 CollisionPoint;

		// Token: 0x0400263C RID: 9788
		public float CollisionStart;

		// Token: 0x0400263D RID: 9789
		public float CollisionEnd;

		// Token: 0x0400263E RID: 9790
		public bool OnGround;

		// Token: 0x0400263F RID: 9791
		public int Damage;

		// Token: 0x04002640 RID: 9792
		public bool IsSubmergeFluid;

		// Token: 0x04002641 RID: 9793
		public bool Overlapping;
	}
}
