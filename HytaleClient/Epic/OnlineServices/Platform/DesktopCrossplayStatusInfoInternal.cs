using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x0200070D RID: 1805
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DesktopCrossplayStatusInfoInternal : IGettable<DesktopCrossplayStatusInfo>, ISettable<DesktopCrossplayStatusInfo>, IDisposable
	{
		// Token: 0x17000DD9 RID: 3545
		// (get) Token: 0x06002E93 RID: 11923 RVA: 0x00044F7C File Offset: 0x0004317C
		// (set) Token: 0x06002E94 RID: 11924 RVA: 0x00044F94 File Offset: 0x00043194
		public DesktopCrossplayStatus Status
		{
			get
			{
				return this.m_Status;
			}
			set
			{
				this.m_Status = value;
			}
		}

		// Token: 0x17000DDA RID: 3546
		// (get) Token: 0x06002E95 RID: 11925 RVA: 0x00044FA0 File Offset: 0x000431A0
		// (set) Token: 0x06002E96 RID: 11926 RVA: 0x00044FB8 File Offset: 0x000431B8
		public int ServiceInitResult
		{
			get
			{
				return this.m_ServiceInitResult;
			}
			set
			{
				this.m_ServiceInitResult = value;
			}
		}

		// Token: 0x06002E97 RID: 11927 RVA: 0x00044FC2 File Offset: 0x000431C2
		public void Set(ref DesktopCrossplayStatusInfo other)
		{
			this.Status = other.Status;
			this.ServiceInitResult = other.ServiceInitResult;
		}

		// Token: 0x06002E98 RID: 11928 RVA: 0x00044FE0 File Offset: 0x000431E0
		public void Set(ref DesktopCrossplayStatusInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.Status = other.Value.Status;
				this.ServiceInitResult = other.Value.ServiceInitResult;
			}
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x00045024 File Offset: 0x00043224
		public void Dispose()
		{
		}

		// Token: 0x06002E9A RID: 11930 RVA: 0x00045027 File Offset: 0x00043227
		public void Get(out DesktopCrossplayStatusInfo output)
		{
			output = default(DesktopCrossplayStatusInfo);
			output.Set(ref this);
		}

		// Token: 0x040014A1 RID: 5281
		private DesktopCrossplayStatus m_Status;

		// Token: 0x040014A2 RID: 5282
		private int m_ServiceInitResult;
	}
}
