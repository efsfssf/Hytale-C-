using System;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A37 RID: 2615
	public struct GLQuery
	{
		// Token: 0x060051F7 RID: 20983 RVA: 0x00167850 File Offset: 0x00165A50
		public GLQuery(uint id)
		{
			this.InternalId = id;
		}

		// Token: 0x060051F8 RID: 20984 RVA: 0x0016785A File Offset: 0x00165A5A
		public static implicit operator uint(GLQuery q)
		{
			return q.InternalId;
		}

		// Token: 0x060051F9 RID: 20985 RVA: 0x00167862 File Offset: 0x00165A62
		public static implicit operator GLQuery(uint q)
		{
			return new GLQuery(q);
		}

		// Token: 0x060051FA RID: 20986 RVA: 0x0016786C File Offset: 0x00165A6C
		public override bool Equals(object obj)
		{
			bool flag = obj is GLQuery;
			return flag && ((GLQuery)obj).InternalId == this.InternalId;
		}

		// Token: 0x060051FB RID: 20987 RVA: 0x001678A4 File Offset: 0x00165AA4
		public override int GetHashCode()
		{
			return this.InternalId.GetHashCode();
		}

		// Token: 0x060051FC RID: 20988 RVA: 0x001678C4 File Offset: 0x00165AC4
		public static bool operator ==(GLQuery a, GLQuery b)
		{
			return a.InternalId == b.InternalId;
		}

		// Token: 0x060051FD RID: 20989 RVA: 0x001678E4 File Offset: 0x00165AE4
		public static bool operator !=(GLQuery a, GLQuery b)
		{
			return a.InternalId != b.InternalId;
		}

		// Token: 0x04002CC1 RID: 11457
		public static readonly GLQuery None = new GLQuery(0U);

		// Token: 0x04002CC2 RID: 11458
		public readonly uint InternalId;
	}
}
