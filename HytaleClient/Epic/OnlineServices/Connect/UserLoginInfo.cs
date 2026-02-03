using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x02000623 RID: 1571
	public struct UserLoginInfo
	{
		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x0600289B RID: 10395 RVA: 0x0003B859 File Offset: 0x00039A59
		// (set) Token: 0x0600289C RID: 10396 RVA: 0x0003B861 File Offset: 0x00039A61
		public Utf8String DisplayName { get; set; }

		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x0600289D RID: 10397 RVA: 0x0003B86A File Offset: 0x00039A6A
		// (set) Token: 0x0600289E RID: 10398 RVA: 0x0003B872 File Offset: 0x00039A72
		public Utf8String NsaIdToken { get; set; }

		// Token: 0x0600289F RID: 10399 RVA: 0x0003B87B File Offset: 0x00039A7B
		internal void Set(ref UserLoginInfoInternal other)
		{
			this.DisplayName = other.DisplayName;
			this.NsaIdToken = other.NsaIdToken;
		}
	}
}
