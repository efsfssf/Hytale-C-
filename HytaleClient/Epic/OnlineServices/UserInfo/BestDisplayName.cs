using System;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x02000024 RID: 36
	public struct BestDisplayName
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000350 RID: 848 RVA: 0x00004B19 File Offset: 0x00002D19
		// (set) Token: 0x06000351 RID: 849 RVA: 0x00004B21 File Offset: 0x00002D21
		public EpicAccountId UserId { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000352 RID: 850 RVA: 0x00004B2A File Offset: 0x00002D2A
		// (set) Token: 0x06000353 RID: 851 RVA: 0x00004B32 File Offset: 0x00002D32
		public Utf8String DisplayName { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000354 RID: 852 RVA: 0x00004B3B File Offset: 0x00002D3B
		// (set) Token: 0x06000355 RID: 853 RVA: 0x00004B43 File Offset: 0x00002D43
		public Utf8String DisplayNameSanitized { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000356 RID: 854 RVA: 0x00004B4C File Offset: 0x00002D4C
		// (set) Token: 0x06000357 RID: 855 RVA: 0x00004B54 File Offset: 0x00002D54
		public Utf8String Nickname { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000358 RID: 856 RVA: 0x00004B5D File Offset: 0x00002D5D
		// (set) Token: 0x06000359 RID: 857 RVA: 0x00004B65 File Offset: 0x00002D65
		public uint PlatformType { get; set; }

		// Token: 0x0600035A RID: 858 RVA: 0x00004B70 File Offset: 0x00002D70
		internal void Set(ref BestDisplayNameInternal other)
		{
			this.UserId = other.UserId;
			this.DisplayName = other.DisplayName;
			this.DisplayNameSanitized = other.DisplayNameSanitized;
			this.Nickname = other.Nickname;
			this.PlatformType = other.PlatformType;
		}
	}
}
