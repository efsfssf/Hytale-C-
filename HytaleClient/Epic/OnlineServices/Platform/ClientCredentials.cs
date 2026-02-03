using System;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x02000709 RID: 1801
	public struct ClientCredentials
	{
		// Token: 0x17000DD3 RID: 3539
		// (get) Token: 0x06002E81 RID: 11905 RVA: 0x00044E06 File Offset: 0x00043006
		// (set) Token: 0x06002E82 RID: 11906 RVA: 0x00044E0E File Offset: 0x0004300E
		public Utf8String ClientId { get; set; }

		// Token: 0x17000DD4 RID: 3540
		// (get) Token: 0x06002E83 RID: 11907 RVA: 0x00044E17 File Offset: 0x00043017
		// (set) Token: 0x06002E84 RID: 11908 RVA: 0x00044E1F File Offset: 0x0004301F
		public Utf8String ClientSecret { get; set; }

		// Token: 0x06002E85 RID: 11909 RVA: 0x00044E28 File Offset: 0x00043028
		internal void Set(ref ClientCredentialsInternal other)
		{
			this.ClientId = other.ClientId;
			this.ClientSecret = other.ClientSecret;
		}
	}
}
