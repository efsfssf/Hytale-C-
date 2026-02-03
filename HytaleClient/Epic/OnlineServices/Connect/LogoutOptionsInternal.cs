using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005F8 RID: 1528
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LogoutOptionsInternal : ISettable<LogoutOptions>, IDisposable
	{
		// Token: 0x17000B9F RID: 2975
		// (set) Token: 0x060027B9 RID: 10169 RVA: 0x0003ACF3 File Offset: 0x00038EF3
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x060027BA RID: 10170 RVA: 0x0003AD03 File Offset: 0x00038F03
		public void Set(ref LogoutOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x060027BB RID: 10171 RVA: 0x0003AD1C File Offset: 0x00038F1C
		public void Set(ref LogoutOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x060027BC RID: 10172 RVA: 0x0003AD52 File Offset: 0x00038F52
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04001136 RID: 4406
		private int m_ApiVersion;

		// Token: 0x04001137 RID: 4407
		private IntPtr m_LocalUserId;
	}
}
