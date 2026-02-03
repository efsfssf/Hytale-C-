using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002E9 RID: 745
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SetPresenceOptionsInternal : ISettable<SetPresenceOptions>, IDisposable
	{
		// Token: 0x1700057B RID: 1403
		// (set) Token: 0x06001473 RID: 5235 RVA: 0x0001DD6C File Offset: 0x0001BF6C
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x1700057C RID: 1404
		// (set) Token: 0x06001474 RID: 5236 RVA: 0x0001DD7C File Offset: 0x0001BF7C
		public PresenceModification PresenceModificationHandle
		{
			set
			{
				Helper.Set(value, ref this.m_PresenceModificationHandle);
			}
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x0001DD8C File Offset: 0x0001BF8C
		public void Set(ref SetPresenceOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.PresenceModificationHandle = other.PresenceModificationHandle;
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x0001DDB0 File Offset: 0x0001BFB0
		public void Set(ref SetPresenceOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.PresenceModificationHandle = other.Value.PresenceModificationHandle;
			}
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x0001DDFB File Offset: 0x0001BFFB
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_PresenceModificationHandle);
		}

		// Token: 0x040008F7 RID: 2295
		private int m_ApiVersion;

		// Token: 0x040008F8 RID: 2296
		private IntPtr m_LocalUserId;

		// Token: 0x040008F9 RID: 2297
		private IntPtr m_PresenceModificationHandle;
	}
}
