using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200018C RID: 396
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct StartSessionOptionsInternal : ISettable<StartSessionOptions>, IDisposable
	{
		// Token: 0x1700029F RID: 671
		// (set) Token: 0x06000B9A RID: 2970 RVA: 0x00010F56 File Offset: 0x0000F156
		public Utf8String SessionName
		{
			set
			{
				Helper.Set(value, ref this.m_SessionName);
			}
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x00010F66 File Offset: 0x0000F166
		public void Set(ref StartSessionOptions other)
		{
			this.m_ApiVersion = 1;
			this.SessionName = other.SessionName;
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x00010F80 File Offset: 0x0000F180
		public void Set(ref StartSessionOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.SessionName = other.Value.SessionName;
			}
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x00010FB6 File Offset: 0x0000F1B6
		public void Dispose()
		{
			Helper.Dispose(ref this.m_SessionName);
		}

		// Token: 0x04000554 RID: 1364
		private int m_ApiVersion;

		// Token: 0x04000555 RID: 1365
		private IntPtr m_SessionName;
	}
}
