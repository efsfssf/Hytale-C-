using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002E3 RID: 739
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryPresenceOptionsInternal : ISettable<QueryPresenceOptions>, IDisposable
	{
		// Token: 0x17000570 RID: 1392
		// (set) Token: 0x0600144F RID: 5199 RVA: 0x0001DACD File Offset: 0x0001BCCD
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000571 RID: 1393
		// (set) Token: 0x06001450 RID: 5200 RVA: 0x0001DADD File Offset: 0x0001BCDD
		public EpicAccountId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x0001DAED File Offset: 0x0001BCED
		public void Set(ref QueryPresenceOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x0001DB14 File Offset: 0x0001BD14
		public void Set(ref QueryPresenceOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x0001DB5F File Offset: 0x0001BD5F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x040008EC RID: 2284
		private int m_ApiVersion;

		// Token: 0x040008ED RID: 2285
		private IntPtr m_LocalUserId;

		// Token: 0x040008EE RID: 2286
		private IntPtr m_TargetUserId;
	}
}
