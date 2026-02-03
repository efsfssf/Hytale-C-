using System;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A34 RID: 2612
	public struct GLTexture
	{
		// Token: 0x060051E5 RID: 20965 RVA: 0x00167634 File Offset: 0x00165834
		public GLTexture(uint id)
		{
			this.InternalId = id;
		}

		// Token: 0x060051E6 RID: 20966 RVA: 0x00167640 File Offset: 0x00165840
		public override bool Equals(object obj)
		{
			bool flag = obj is GLTexture;
			return flag && ((GLTexture)obj).InternalId == this.InternalId;
		}

		// Token: 0x060051E7 RID: 20967 RVA: 0x00167678 File Offset: 0x00165878
		public override int GetHashCode()
		{
			return this.InternalId.GetHashCode();
		}

		// Token: 0x060051E8 RID: 20968 RVA: 0x00167698 File Offset: 0x00165898
		public static bool operator ==(GLTexture a, GLTexture b)
		{
			return a.InternalId == b.InternalId;
		}

		// Token: 0x060051E9 RID: 20969 RVA: 0x001676B8 File Offset: 0x001658B8
		public static bool operator !=(GLTexture a, GLTexture b)
		{
			return a.InternalId != b.InternalId;
		}

		// Token: 0x04002CBB RID: 11451
		public static readonly GLTexture None = new GLTexture(0U);

		// Token: 0x04002CBC RID: 11452
		public readonly uint InternalId;
	}
}
