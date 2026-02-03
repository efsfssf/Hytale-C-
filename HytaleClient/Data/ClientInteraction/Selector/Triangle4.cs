using System;
using HytaleClient.Math;

namespace HytaleClient.Data.ClientInteraction.Selector
{
	// Token: 0x02000B1D RID: 2845
	internal struct Triangle4
	{
		// Token: 0x060058D8 RID: 22744 RVA: 0x001B2D12 File Offset: 0x001B0F12
		public Triangle4(Vector4 a, Vector4 b, Vector4 c)
		{
			this.A = a;
			this.B = b;
			this.C = c;
		}

		// Token: 0x060058D9 RID: 22745 RVA: 0x001B2D2C File Offset: 0x001B0F2C
		public Vector4 GetRandom(Random random)
		{
			float num = random.NextFloat(0f, 1f);
			float num2 = random.NextFloat(0f, 1f);
			float num3 = 1f - num - num2;
			return new Vector4(this.A.X * num3 + this.B.X * num + this.C.X * num2, this.A.Y * num3 + this.B.Y * num + this.C.Y * num2, this.A.Z * num3 + this.B.Z * num + this.C.Z * num2, this.A.W * num3 + this.B.W * num + this.C.W * num2);
		}

		// Token: 0x04003748 RID: 14152
		public Vector4 A;

		// Token: 0x04003749 RID: 14153
		public Vector4 B;

		// Token: 0x0400374A RID: 14154
		public Vector4 C;
	}
}
