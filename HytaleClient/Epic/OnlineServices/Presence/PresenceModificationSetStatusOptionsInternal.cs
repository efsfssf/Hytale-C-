using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002DF RID: 735
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct PresenceModificationSetStatusOptionsInternal : ISettable<PresenceModificationSetStatusOptions>, IDisposable
	{
		// Token: 0x17000564 RID: 1380
		// (set) Token: 0x06001430 RID: 5168 RVA: 0x0001D802 File Offset: 0x0001BA02
		public Status Status
		{
			set
			{
				this.m_Status = value;
			}
		}

		// Token: 0x06001431 RID: 5169 RVA: 0x0001D80C File Offset: 0x0001BA0C
		public void Set(ref PresenceModificationSetStatusOptions other)
		{
			this.m_ApiVersion = 1;
			this.Status = other.Status;
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x0001D824 File Offset: 0x0001BA24
		public void Set(ref PresenceModificationSetStatusOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Status = other.Value.Status;
			}
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x0001D85A File Offset: 0x0001BA5A
		public void Dispose()
		{
		}

		// Token: 0x040008E0 RID: 2272
		private int m_ApiVersion;

		// Token: 0x040008E1 RID: 2273
		private Status m_Status;
	}
}
