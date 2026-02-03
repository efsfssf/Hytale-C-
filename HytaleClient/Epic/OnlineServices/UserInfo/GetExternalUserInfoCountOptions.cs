using System;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x02000034 RID: 52
	public struct GetExternalUserInfoCountOptions
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x000055DE File Offset: 0x000037DE
		// (set) Token: 0x060003C1 RID: 961 RVA: 0x000055E6 File Offset: 0x000037E6
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x000055EF File Offset: 0x000037EF
		// (set) Token: 0x060003C3 RID: 963 RVA: 0x000055F7 File Offset: 0x000037F7
		public EpicAccountId TargetUserId { get; set; }
	}
}
