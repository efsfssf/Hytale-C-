using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x020000E0 RID: 224
	public struct ActiveSessionInfo
	{
		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060007E9 RID: 2025 RVA: 0x0000B7EC File Offset: 0x000099EC
		// (set) Token: 0x060007EA RID: 2026 RVA: 0x0000B7F4 File Offset: 0x000099F4
		public Utf8String SessionName { get; set; }

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x0000B7FD File Offset: 0x000099FD
		// (set) Token: 0x060007EC RID: 2028 RVA: 0x0000B805 File Offset: 0x00009A05
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x0000B80E File Offset: 0x00009A0E
		// (set) Token: 0x060007EE RID: 2030 RVA: 0x0000B816 File Offset: 0x00009A16
		public OnlineSessionState State { get; set; }

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x0000B81F File Offset: 0x00009A1F
		// (set) Token: 0x060007F0 RID: 2032 RVA: 0x0000B827 File Offset: 0x00009A27
		public SessionDetailsInfo? SessionDetails { get; set; }

		// Token: 0x060007F1 RID: 2033 RVA: 0x0000B830 File Offset: 0x00009A30
		internal void Set(ref ActiveSessionInfoInternal other)
		{
			this.SessionName = other.SessionName;
			this.LocalUserId = other.LocalUserId;
			this.State = other.State;
			this.SessionDetails = other.SessionDetails;
		}
	}
}
