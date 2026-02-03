using System;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.ImmersiveScreen.Screens
{
	// Token: 0x0200093E RID: 2366
	public struct ViewPlaneIntersection
	{
		// Token: 0x060048E1 RID: 18657 RVA: 0x0011AC36 File Offset: 0x00118E36
		public ViewPlaneIntersection(Vector3 worldPos, Vector2 pixelPos)
		{
			this.WorldPosition = worldPos;
			this.PixelPosition = pixelPos;
		}

		// Token: 0x040024E2 RID: 9442
		public Vector3 WorldPosition;

		// Token: 0x040024E3 RID: 9443
		public Vector2 PixelPosition;
	}
}
