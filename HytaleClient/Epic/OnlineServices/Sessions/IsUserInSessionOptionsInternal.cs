using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200010D RID: 269
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct IsUserInSessionOptionsInternal : ISettable<IsUserInSessionOptions>, IDisposable
	{
		// Token: 0x170001D9 RID: 473
		// (set) Token: 0x060008B5 RID: 2229 RVA: 0x0000CB14 File Offset: 0x0000AD14
		public Utf8String SessionName
		{
			set
			{
				Helper.Set(value, ref this.m_SessionName);
			}
		}

		// Token: 0x170001DA RID: 474
		// (set) Token: 0x060008B6 RID: 2230 RVA: 0x0000CB24 File Offset: 0x0000AD24
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0000CB34 File Offset: 0x0000AD34
		public void Set(ref IsUserInSessionOptions other)
		{
			this.m_ApiVersion = 1;
			this.SessionName = other.SessionName;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0000CB58 File Offset: 0x0000AD58
		public void Set(ref IsUserInSessionOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.SessionName = other.Value.SessionName;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0000CBA3 File Offset: 0x0000ADA3
		public void Dispose()
		{
			Helper.Dispose(ref this.m_SessionName);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04000427 RID: 1063
		private int m_ApiVersion;

		// Token: 0x04000428 RID: 1064
		private IntPtr m_SessionName;

		// Token: 0x04000429 RID: 1065
		private IntPtr m_TargetUserId;
	}
}
