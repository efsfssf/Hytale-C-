using System;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A36 RID: 2614
	public struct GLFramebuffer
	{
		// Token: 0x060051F1 RID: 20977 RVA: 0x0016779C File Offset: 0x0016599C
		public GLFramebuffer(uint id)
		{
			this.InternalId = id;
		}

		// Token: 0x060051F2 RID: 20978 RVA: 0x001677A8 File Offset: 0x001659A8
		public override bool Equals(object obj)
		{
			bool flag = obj is GLFramebuffer;
			return flag && ((GLFramebuffer)obj).InternalId == this.InternalId;
		}

		// Token: 0x060051F3 RID: 20979 RVA: 0x001677E0 File Offset: 0x001659E0
		public override int GetHashCode()
		{
			return this.InternalId.GetHashCode();
		}

		// Token: 0x060051F4 RID: 20980 RVA: 0x00167800 File Offset: 0x00165A00
		public static bool operator ==(GLFramebuffer a, GLFramebuffer b)
		{
			return a.InternalId == b.InternalId;
		}

		// Token: 0x060051F5 RID: 20981 RVA: 0x00167820 File Offset: 0x00165A20
		public static bool operator !=(GLFramebuffer a, GLFramebuffer b)
		{
			return a.InternalId != b.InternalId;
		}

		// Token: 0x04002CBF RID: 11455
		public static readonly GLFramebuffer None = new GLFramebuffer(0U);

		// Token: 0x04002CC0 RID: 11456
		public readonly uint InternalId;
	}
}
