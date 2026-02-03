using System;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A33 RID: 2611
	public struct GLBuffer
	{
		// Token: 0x060051DF RID: 20959 RVA: 0x00167580 File Offset: 0x00165780
		public GLBuffer(uint id)
		{
			this.InternalId = id;
		}

		// Token: 0x060051E0 RID: 20960 RVA: 0x0016758C File Offset: 0x0016578C
		public override bool Equals(object obj)
		{
			bool flag = obj is GLBuffer;
			return flag && ((GLBuffer)obj).InternalId == this.InternalId;
		}

		// Token: 0x060051E1 RID: 20961 RVA: 0x001675C4 File Offset: 0x001657C4
		public override int GetHashCode()
		{
			return this.InternalId.GetHashCode();
		}

		// Token: 0x060051E2 RID: 20962 RVA: 0x001675E4 File Offset: 0x001657E4
		public static bool operator ==(GLBuffer a, GLBuffer b)
		{
			return a.InternalId == b.InternalId;
		}

		// Token: 0x060051E3 RID: 20963 RVA: 0x00167604 File Offset: 0x00165804
		public static bool operator !=(GLBuffer a, GLBuffer b)
		{
			return a.InternalId != b.InternalId;
		}

		// Token: 0x04002CB9 RID: 11449
		public static readonly GLBuffer None = new GLBuffer(0U);

		// Token: 0x04002CBA RID: 11450
		public readonly uint InternalId;
	}
}
