using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000156 RID: 342
	public struct SessionDetailsInfo
	{
		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000A2F RID: 2607 RVA: 0x0000E3A0 File Offset: 0x0000C5A0
		// (set) Token: 0x06000A30 RID: 2608 RVA: 0x0000E3A8 File Offset: 0x0000C5A8
		public Utf8String SessionId { get; set; }

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000A31 RID: 2609 RVA: 0x0000E3B1 File Offset: 0x0000C5B1
		// (set) Token: 0x06000A32 RID: 2610 RVA: 0x0000E3B9 File Offset: 0x0000C5B9
		public Utf8String HostAddress { get; set; }

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000A33 RID: 2611 RVA: 0x0000E3C2 File Offset: 0x0000C5C2
		// (set) Token: 0x06000A34 RID: 2612 RVA: 0x0000E3CA File Offset: 0x0000C5CA
		public uint NumOpenPublicConnections { get; set; }

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000A35 RID: 2613 RVA: 0x0000E3D3 File Offset: 0x0000C5D3
		// (set) Token: 0x06000A36 RID: 2614 RVA: 0x0000E3DB File Offset: 0x0000C5DB
		public SessionDetailsSettings? Settings { get; set; }

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x0000E3E4 File Offset: 0x0000C5E4
		// (set) Token: 0x06000A38 RID: 2616 RVA: 0x0000E3EC File Offset: 0x0000C5EC
		public ProductUserId OwnerUserId { get; set; }

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x0000E3F5 File Offset: 0x0000C5F5
		// (set) Token: 0x06000A3A RID: 2618 RVA: 0x0000E3FD File Offset: 0x0000C5FD
		public Utf8String OwnerServerClientId { get; set; }

		// Token: 0x06000A3B RID: 2619 RVA: 0x0000E408 File Offset: 0x0000C608
		internal void Set(ref SessionDetailsInfoInternal other)
		{
			this.SessionId = other.SessionId;
			this.HostAddress = other.HostAddress;
			this.NumOpenPublicConnections = other.NumOpenPublicConnections;
			this.Settings = other.Settings;
			this.OwnerUserId = other.OwnerUserId;
			this.OwnerServerClientId = other.OwnerServerClientId;
		}
	}
}
