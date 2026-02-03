using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002C5 RID: 709
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct HasPresenceOptionsInternal : ISettable<HasPresenceOptions>, IDisposable
	{
		// Token: 0x17000533 RID: 1331
		// (set) Token: 0x06001380 RID: 4992 RVA: 0x0001C5AC File Offset: 0x0001A7AC
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000534 RID: 1332
		// (set) Token: 0x06001381 RID: 4993 RVA: 0x0001C5BC File Offset: 0x0001A7BC
		public EpicAccountId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x0001C5CC File Offset: 0x0001A7CC
		public void Set(ref HasPresenceOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x0001C5F0 File Offset: 0x0001A7F0
		public void Set(ref HasPresenceOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x0001C63B File Offset: 0x0001A83B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x0400088D RID: 2189
		private int m_ApiVersion;

		// Token: 0x0400088E RID: 2190
		private IntPtr m_LocalUserId;

		// Token: 0x0400088F RID: 2191
		private IntPtr m_TargetUserId;
	}
}
