using System;
using System.Collections.Generic;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x0200094A RID: 2378
	internal class BasicCollisionData
	{
		// Token: 0x06004A64 RID: 19044 RVA: 0x0012F12D File Offset: 0x0012D32D
		public void SetStart(Vector3 point, float start)
		{
			this.CollisionPoint = point;
			this.CollisionStart = start;
		}

		// Token: 0x0400261F RID: 9759
		public static readonly IComparer<BasicCollisionData> CollisionStartComparator = new BasicCollisionData.BasicCollisionDataComparer();

		// Token: 0x04002620 RID: 9760
		public Vector3 CollisionPoint;

		// Token: 0x04002621 RID: 9761
		public float CollisionStart;

		// Token: 0x02000E44 RID: 3652
		private class BasicCollisionDataComparer : Comparer<BasicCollisionData>
		{
			// Token: 0x06006738 RID: 26424 RVA: 0x0021734C File Offset: 0x0021554C
			public override int Compare(BasicCollisionData x, BasicCollisionData y)
			{
				return x.CollisionStart.CompareTo(y.CollisionStart);
			}
		}
	}
}
