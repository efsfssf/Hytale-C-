using System;
using HytaleClient.Math;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A3D RID: 2621
	public struct LightData
	{
		// Token: 0x06005247 RID: 21063 RVA: 0x00169DB0 File Offset: 0x00167FB0
		public static float ComputeRadiusFromColor(Vector3 color)
		{
			Vector3 vector = color * 15f * 0.635f;
			return Math.Max(vector.X, Math.Max(vector.Y, vector.Z));
		}

		// Token: 0x04002D5B RID: 11611
		public BoundingSphere Sphere;

		// Token: 0x04002D5C RID: 11612
		public Vector3 Color;
	}
}
