using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200038D RID: 909
	public struct GetInviteIdByIndexOptions
	{
		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06001862 RID: 6242 RVA: 0x00023AE5 File Offset: 0x00021CE5
		// (set) Token: 0x06001863 RID: 6243 RVA: 0x00023AED File Offset: 0x00021CED
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06001864 RID: 6244 RVA: 0x00023AF6 File Offset: 0x00021CF6
		// (set) Token: 0x06001865 RID: 6245 RVA: 0x00023AFE File Offset: 0x00021CFE
		public uint Index { get; set; }
	}
}
