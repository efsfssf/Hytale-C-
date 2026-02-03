using System;

namespace HytaleClient.Math
{
	// Token: 0x020007F5 RID: 2037
	public static class Ray_Hytale
	{
		// Token: 0x06003738 RID: 14136 RVA: 0x0006DD4C File Offset: 0x0006BF4C
		public static Vector3 GetAt(this Ray ray, float t)
		{
			return ray.Position + t * ray.Direction;
		}

		// Token: 0x06003739 RID: 14137 RVA: 0x0006DD78 File Offset: 0x0006BF78
		public static float Distance(this Ray ray, float t)
		{
			return ray.Direction.Length() * t;
		}
	}
}
