using System;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x02000028 RID: 40
	public struct CopyBestDisplayNameWithPlatformOptions
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000372 RID: 882 RVA: 0x00004EAA File Offset: 0x000030AA
		// (set) Token: 0x06000373 RID: 883 RVA: 0x00004EB2 File Offset: 0x000030B2
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000374 RID: 884 RVA: 0x00004EBB File Offset: 0x000030BB
		// (set) Token: 0x06000375 RID: 885 RVA: 0x00004EC3 File Offset: 0x000030C3
		public EpicAccountId TargetUserId { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000376 RID: 886 RVA: 0x00004ECC File Offset: 0x000030CC
		// (set) Token: 0x06000377 RID: 887 RVA: 0x00004ED4 File Offset: 0x000030D4
		public uint TargetPlatformType { get; set; }
	}
}
