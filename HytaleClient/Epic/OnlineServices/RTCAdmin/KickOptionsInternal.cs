using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin
{
	// Token: 0x0200028B RID: 651
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct KickOptionsInternal : ISettable<KickOptions>, IDisposable
	{
		// Token: 0x170004D2 RID: 1234
		// (set) Token: 0x06001244 RID: 4676 RVA: 0x0001AA8B File Offset: 0x00018C8B
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (set) Token: 0x06001245 RID: 4677 RVA: 0x0001AA9B File Offset: 0x00018C9B
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x0001AAAB File Offset: 0x00018CAB
		public void Set(ref KickOptions other)
		{
			this.m_ApiVersion = 1;
			this.RoomName = other.RoomName;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x0001AAD0 File Offset: 0x00018CD0
		public void Set(ref KickOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.RoomName = other.Value.RoomName;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x0001AB1B File Offset: 0x00018D1B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_RoomName);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04000808 RID: 2056
		private int m_ApiVersion;

		// Token: 0x04000809 RID: 2057
		private IntPtr m_RoomName;

		// Token: 0x0400080A RID: 2058
		private IntPtr m_TargetUserId;
	}
}
