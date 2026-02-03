using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002BD RID: 701
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyPresenceOptionsInternal : ISettable<CopyPresenceOptions>, IDisposable
	{
		// Token: 0x17000525 RID: 1317
		// (set) Token: 0x0600135B RID: 4955 RVA: 0x0001C24E File Offset: 0x0001A44E
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000526 RID: 1318
		// (set) Token: 0x0600135C RID: 4956 RVA: 0x0001C25E File Offset: 0x0001A45E
		public EpicAccountId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x0001C26E File Offset: 0x0001A46E
		public void Set(ref CopyPresenceOptions other)
		{
			this.m_ApiVersion = 3;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x0001C294 File Offset: 0x0001A494
		public void Set(ref CopyPresenceOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 3;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x0001C2DF File Offset: 0x0001A4DF
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x0400087B RID: 2171
		private int m_ApiVersion;

		// Token: 0x0400087C RID: 2172
		private IntPtr m_LocalUserId;

		// Token: 0x0400087D RID: 2173
		private IntPtr m_TargetUserId;
	}
}
