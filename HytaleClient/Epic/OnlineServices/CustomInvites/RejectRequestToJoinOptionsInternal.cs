using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005B0 RID: 1456
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RejectRequestToJoinOptionsInternal : ISettable<RejectRequestToJoinOptions>, IDisposable
	{
		// Token: 0x17000AEF RID: 2799
		// (set) Token: 0x060025C4 RID: 9668 RVA: 0x00037715 File Offset: 0x00035915
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000AF0 RID: 2800
		// (set) Token: 0x060025C5 RID: 9669 RVA: 0x00037725 File Offset: 0x00035925
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x00037735 File Offset: 0x00035935
		public void Set(ref RejectRequestToJoinOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x060025C7 RID: 9671 RVA: 0x0003775C File Offset: 0x0003595C
		public void Set(ref RejectRequestToJoinOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x000377A7 File Offset: 0x000359A7
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04001058 RID: 4184
		private int m_ApiVersion;

		// Token: 0x04001059 RID: 4185
		private IntPtr m_LocalUserId;

		// Token: 0x0400105A RID: 4186
		private IntPtr m_TargetUserId;
	}
}
