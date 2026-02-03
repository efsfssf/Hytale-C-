using System;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A35 RID: 2613
	public struct GLSampler
	{
		// Token: 0x060051EB RID: 20971 RVA: 0x001676E8 File Offset: 0x001658E8
		public GLSampler(uint id)
		{
			this.InternalId = id;
		}

		// Token: 0x060051EC RID: 20972 RVA: 0x001676F4 File Offset: 0x001658F4
		public override bool Equals(object obj)
		{
			bool flag = obj is GLSampler;
			return flag && ((GLSampler)obj).InternalId == this.InternalId;
		}

		// Token: 0x060051ED RID: 20973 RVA: 0x0016772C File Offset: 0x0016592C
		public override int GetHashCode()
		{
			return this.InternalId.GetHashCode();
		}

		// Token: 0x060051EE RID: 20974 RVA: 0x0016774C File Offset: 0x0016594C
		public static bool operator ==(GLSampler a, GLSampler b)
		{
			return a.InternalId == b.InternalId;
		}

		// Token: 0x060051EF RID: 20975 RVA: 0x0016776C File Offset: 0x0016596C
		public static bool operator !=(GLSampler a, GLSampler b)
		{
			return a.InternalId != b.InternalId;
		}

		// Token: 0x04002CBD RID: 11453
		public static readonly GLSampler None = new GLSampler(0U);

		// Token: 0x04002CBE RID: 11454
		public readonly uint InternalId;
	}
}
