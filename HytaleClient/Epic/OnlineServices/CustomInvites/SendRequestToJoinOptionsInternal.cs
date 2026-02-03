using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005BF RID: 1471
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SendRequestToJoinOptionsInternal : ISettable<SendRequestToJoinOptions>, IDisposable
	{
		// Token: 0x17000B26 RID: 2854
		// (set) Token: 0x0600264D RID: 9805 RVA: 0x000384FD File Offset: 0x000366FD
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000B27 RID: 2855
		// (set) Token: 0x0600264E RID: 9806 RVA: 0x0003850D File Offset: 0x0003670D
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x0003851D File Offset: 0x0003671D
		public void Set(ref SendRequestToJoinOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06002650 RID: 9808 RVA: 0x00038544 File Offset: 0x00036744
		public void Set(ref SendRequestToJoinOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x06002651 RID: 9809 RVA: 0x0003858F File Offset: 0x0003678F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04001091 RID: 4241
		private int m_ApiVersion;

		// Token: 0x04001092 RID: 4242
		private IntPtr m_LocalUserId;

		// Token: 0x04001093 RID: 4243
		private IntPtr m_TargetUserId;
	}
}
