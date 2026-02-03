using System;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x02000026 RID: 38
	public struct CopyBestDisplayNameOptions
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000369 RID: 873 RVA: 0x00004DDB File Offset: 0x00002FDB
		// (set) Token: 0x0600036A RID: 874 RVA: 0x00004DE3 File Offset: 0x00002FE3
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600036B RID: 875 RVA: 0x00004DEC File Offset: 0x00002FEC
		// (set) Token: 0x0600036C RID: 876 RVA: 0x00004DF4 File Offset: 0x00002FF4
		public EpicAccountId TargetUserId { get; set; }
	}
}
