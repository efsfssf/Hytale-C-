using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000375 RID: 885
	public struct AttributeData
	{
		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x060017B0 RID: 6064 RVA: 0x0002293C File Offset: 0x00020B3C
		// (set) Token: 0x060017B1 RID: 6065 RVA: 0x00022944 File Offset: 0x00020B44
		public Utf8String Key { get; set; }

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x060017B2 RID: 6066 RVA: 0x0002294D File Offset: 0x00020B4D
		// (set) Token: 0x060017B3 RID: 6067 RVA: 0x00022955 File Offset: 0x00020B55
		public AttributeDataValue Value { get; set; }

		// Token: 0x060017B4 RID: 6068 RVA: 0x0002295E File Offset: 0x00020B5E
		internal void Set(ref AttributeDataInternal other)
		{
			this.Key = other.Key;
			this.Value = other.Value;
		}
	}
}
