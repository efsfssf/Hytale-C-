using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005C1 RID: 1473
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SetCustomInviteOptionsInternal : ISettable<SetCustomInviteOptions>, IDisposable
	{
		// Token: 0x17000B2A RID: 2858
		// (set) Token: 0x06002656 RID: 9814 RVA: 0x000385CC File Offset: 0x000367CC
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000B2B RID: 2859
		// (set) Token: 0x06002657 RID: 9815 RVA: 0x000385DC File Offset: 0x000367DC
		public Utf8String Payload
		{
			set
			{
				Helper.Set(value, ref this.m_Payload);
			}
		}

		// Token: 0x06002658 RID: 9816 RVA: 0x000385EC File Offset: 0x000367EC
		public void Set(ref SetCustomInviteOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.Payload = other.Payload;
		}

		// Token: 0x06002659 RID: 9817 RVA: 0x00038610 File Offset: 0x00036810
		public void Set(ref SetCustomInviteOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.Payload = other.Value.Payload;
			}
		}

		// Token: 0x0600265A RID: 9818 RVA: 0x0003865B File Offset: 0x0003685B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_Payload);
		}

		// Token: 0x04001096 RID: 4246
		private int m_ApiVersion;

		// Token: 0x04001097 RID: 4247
		private IntPtr m_LocalUserId;

		// Token: 0x04001098 RID: 4248
		private IntPtr m_Payload;
	}
}
