using System;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x0200070C RID: 1804
	public struct DesktopCrossplayStatusInfo
	{
		// Token: 0x17000DD7 RID: 3543
		// (get) Token: 0x06002E8E RID: 11918 RVA: 0x00044F3D File Offset: 0x0004313D
		// (set) Token: 0x06002E8F RID: 11919 RVA: 0x00044F45 File Offset: 0x00043145
		public DesktopCrossplayStatus Status { get; set; }

		// Token: 0x17000DD8 RID: 3544
		// (get) Token: 0x06002E90 RID: 11920 RVA: 0x00044F4E File Offset: 0x0004314E
		// (set) Token: 0x06002E91 RID: 11921 RVA: 0x00044F56 File Offset: 0x00043156
		public int ServiceInitResult { get; set; }

		// Token: 0x06002E92 RID: 11922 RVA: 0x00044F5F File Offset: 0x0004315F
		internal void Set(ref DesktopCrossplayStatusInfoInternal other)
		{
			this.Status = other.Status;
			this.ServiceInitResult = other.ServiceInitResult;
		}
	}
}
