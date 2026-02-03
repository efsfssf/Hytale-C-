using System;
using HytaleClient.Math;

namespace HytaleClient.Data.ClientInteraction.Selector
{
	// Token: 0x02000B1C RID: 2844
	public struct Quad4
	{
		// Token: 0x060058D4 RID: 22740 RVA: 0x001B2A77 File Offset: 0x001B0C77
		public Quad4(Vector4[] points, int a, int b, int c, int d)
		{
			this.A = points[a];
			this.B = points[b];
			this.C = points[c];
			this.D = points[d];
		}

		// Token: 0x060058D5 RID: 22741 RVA: 0x001B2AB0 File Offset: 0x001B0CB0
		public Quad4 Multiply(Matrix matrix)
		{
			this.A = Vector4.Transform(this.A, matrix);
			this.B = Vector4.Transform(this.B, matrix);
			this.C = Vector4.Transform(this.C, matrix);
			this.D = Vector4.Transform(this.D, matrix);
			return this;
		}

		// Token: 0x060058D6 RID: 22742 RVA: 0x001B2B10 File Offset: 0x001B0D10
		public bool IsFullyInsideFrustum()
		{
			return this.A.IsInsideFrustum() && this.B.IsInsideFrustum() && this.C.IsInsideFrustum() && this.D.IsInsideFrustum();
		}

		// Token: 0x060058D7 RID: 22743 RVA: 0x001B2B58 File Offset: 0x001B0D58
		public Vector4 GetRandom(Random random)
		{
			float num = random.NextFloat(0f, 1f);
			float num2 = random.NextFloat(0f, 1f) * (1f - num);
			float num3 = 1f - num - num2;
			bool flag = random.NextDouble() < 0.5;
			Vector4 result;
			if (flag)
			{
				result = new Vector4(this.A.X * num3 + this.B.X * num + this.C.X * num2, this.A.Y * num3 + this.B.Y * num + this.C.Y * num2, this.A.Z * num3 + this.B.Z * num + this.C.Z * num2, this.A.W * num3 + this.B.W * num + this.C.W * num2);
			}
			else
			{
				result = new Vector4(this.A.X * num3 + this.C.X * num + this.D.X * num2, this.A.Y * num3 + this.C.Y * num + this.D.Y * num2, this.A.Z * num3 + this.C.Z * num + this.D.Z * num2, this.A.W * num3 + this.C.W * num + this.D.W * num2);
			}
			return result;
		}

		// Token: 0x04003744 RID: 14148
		public Vector4 A;

		// Token: 0x04003745 RID: 14149
		public Vector4 B;

		// Token: 0x04003746 RID: 14150
		public Vector4 C;

		// Token: 0x04003747 RID: 14151
		public Vector4 D;
	}
}
