using System;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x0200002E RID: 46
	public struct CopyExternalUserInfoByIndexOptions
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000396 RID: 918 RVA: 0x000051DB File Offset: 0x000033DB
		// (set) Token: 0x06000397 RID: 919 RVA: 0x000051E3 File Offset: 0x000033E3
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000398 RID: 920 RVA: 0x000051EC File Offset: 0x000033EC
		// (set) Token: 0x06000399 RID: 921 RVA: 0x000051F4 File Offset: 0x000033F4
		public EpicAccountId TargetUserId { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600039A RID: 922 RVA: 0x000051FD File Offset: 0x000033FD
		// (set) Token: 0x0600039B RID: 923 RVA: 0x00005205 File Offset: 0x00003405
		public uint Index { get; set; }
	}
}
