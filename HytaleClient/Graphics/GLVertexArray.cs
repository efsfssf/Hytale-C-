using System;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A32 RID: 2610
	public struct GLVertexArray
	{
		// Token: 0x060051D9 RID: 20953 RVA: 0x001674CC File Offset: 0x001656CC
		public GLVertexArray(uint id)
		{
			this.InternalId = id;
		}

		// Token: 0x060051DA RID: 20954 RVA: 0x001674D8 File Offset: 0x001656D8
		public override bool Equals(object obj)
		{
			bool flag = obj is GLVertexArray;
			return flag && ((GLVertexArray)obj).InternalId == this.InternalId;
		}

		// Token: 0x060051DB RID: 20955 RVA: 0x00167510 File Offset: 0x00165710
		public override int GetHashCode()
		{
			return this.InternalId.GetHashCode();
		}

		// Token: 0x060051DC RID: 20956 RVA: 0x00167530 File Offset: 0x00165730
		public static bool operator ==(GLVertexArray a, GLVertexArray b)
		{
			return a.InternalId == b.InternalId;
		}

		// Token: 0x060051DD RID: 20957 RVA: 0x00167550 File Offset: 0x00165750
		public static bool operator !=(GLVertexArray a, GLVertexArray b)
		{
			return a.InternalId != b.InternalId;
		}

		// Token: 0x04002CB7 RID: 11447
		public static readonly GLVertexArray None = new GLVertexArray(0U);

		// Token: 0x04002CB8 RID: 11448
		public readonly uint InternalId;
	}
}
