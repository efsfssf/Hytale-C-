using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005DB RID: 1499
	public struct Credentials
	{
		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x060026EE RID: 9966 RVA: 0x000399ED File Offset: 0x00037BED
		// (set) Token: 0x060026EF RID: 9967 RVA: 0x000399F5 File Offset: 0x00037BF5
		public Utf8String Token { get; set; }

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x060026F0 RID: 9968 RVA: 0x000399FE File Offset: 0x00037BFE
		// (set) Token: 0x060026F1 RID: 9969 RVA: 0x00039A06 File Offset: 0x00037C06
		public ExternalCredentialType Type { get; set; }

		// Token: 0x060026F2 RID: 9970 RVA: 0x00039A0F File Offset: 0x00037C0F
		internal void Set(ref CredentialsInternal other)
		{
			this.Token = other.Token;
			this.Type = other.Type;
		}
	}
}
