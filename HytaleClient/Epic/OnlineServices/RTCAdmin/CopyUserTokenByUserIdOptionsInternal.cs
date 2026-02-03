using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin
{
	// Token: 0x02000287 RID: 647
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyUserTokenByUserIdOptionsInternal : ISettable<CopyUserTokenByUserIdOptions>, IDisposable
	{
		// Token: 0x170004C9 RID: 1225
		// (set) Token: 0x0600122C RID: 4652 RVA: 0x0001A87C File Offset: 0x00018A7C
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x170004CA RID: 1226
		// (set) Token: 0x0600122D RID: 4653 RVA: 0x0001A88C File Offset: 0x00018A8C
		public uint QueryId
		{
			set
			{
				this.m_QueryId = value;
			}
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x0001A896 File Offset: 0x00018A96
		public void Set(ref CopyUserTokenByUserIdOptions other)
		{
			this.m_ApiVersion = 2;
			this.TargetUserId = other.TargetUserId;
			this.QueryId = other.QueryId;
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x0001A8BC File Offset: 0x00018ABC
		public void Set(ref CopyUserTokenByUserIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.TargetUserId = other.Value.TargetUserId;
				this.QueryId = other.Value.QueryId;
			}
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x0001A907 File Offset: 0x00018B07
		public void Dispose()
		{
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x040007FF RID: 2047
		private int m_ApiVersion;

		// Token: 0x04000800 RID: 2048
		private IntPtr m_TargetUserId;

		// Token: 0x04000801 RID: 2049
		private uint m_QueryId;
	}
}
