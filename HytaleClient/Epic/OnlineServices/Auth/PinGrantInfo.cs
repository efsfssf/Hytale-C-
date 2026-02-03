using System;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x0200065D RID: 1629
	public struct PinGrantInfo
	{
		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x06002A18 RID: 10776 RVA: 0x0003DC1D File Offset: 0x0003BE1D
		// (set) Token: 0x06002A19 RID: 10777 RVA: 0x0003DC25 File Offset: 0x0003BE25
		public Utf8String UserCode { get; set; }

		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x06002A1A RID: 10778 RVA: 0x0003DC2E File Offset: 0x0003BE2E
		// (set) Token: 0x06002A1B RID: 10779 RVA: 0x0003DC36 File Offset: 0x0003BE36
		public Utf8String VerificationURI { get; set; }

		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x06002A1C RID: 10780 RVA: 0x0003DC3F File Offset: 0x0003BE3F
		// (set) Token: 0x06002A1D RID: 10781 RVA: 0x0003DC47 File Offset: 0x0003BE47
		public int ExpiresIn { get; set; }

		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x06002A1E RID: 10782 RVA: 0x0003DC50 File Offset: 0x0003BE50
		// (set) Token: 0x06002A1F RID: 10783 RVA: 0x0003DC58 File Offset: 0x0003BE58
		public Utf8String VerificationURIComplete { get; set; }

		// Token: 0x06002A20 RID: 10784 RVA: 0x0003DC61 File Offset: 0x0003BE61
		internal void Set(ref PinGrantInfoInternal other)
		{
			this.UserCode = other.UserCode;
			this.VerificationURI = other.VerificationURI;
			this.ExpiresIn = other.ExpiresIn;
			this.VerificationURIComplete = other.VerificationURIComplete;
		}
	}
}
