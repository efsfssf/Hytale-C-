using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatServer
{
	// Token: 0x0200067D RID: 1661
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct BeginSessionOptionsInternal : ISettable<BeginSessionOptions>, IDisposable
	{
		// Token: 0x17000CA7 RID: 3239
		// (set) Token: 0x06002B3F RID: 11071 RVA: 0x0003FD4B File Offset: 0x0003DF4B
		public uint RegisterTimeoutSeconds
		{
			set
			{
				this.m_RegisterTimeoutSeconds = value;
			}
		}

		// Token: 0x17000CA8 RID: 3240
		// (set) Token: 0x06002B40 RID: 11072 RVA: 0x0003FD55 File Offset: 0x0003DF55
		public Utf8String ServerName
		{
			set
			{
				Helper.Set(value, ref this.m_ServerName);
			}
		}

		// Token: 0x17000CA9 RID: 3241
		// (set) Token: 0x06002B41 RID: 11073 RVA: 0x0003FD65 File Offset: 0x0003DF65
		public bool EnableGameplayData
		{
			set
			{
				Helper.Set(value, ref this.m_EnableGameplayData);
			}
		}

		// Token: 0x17000CAA RID: 3242
		// (set) Token: 0x06002B42 RID: 11074 RVA: 0x0003FD75 File Offset: 0x0003DF75
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06002B43 RID: 11075 RVA: 0x0003FD85 File Offset: 0x0003DF85
		public void Set(ref BeginSessionOptions other)
		{
			this.m_ApiVersion = 3;
			this.RegisterTimeoutSeconds = other.RegisterTimeoutSeconds;
			this.ServerName = other.ServerName;
			this.EnableGameplayData = other.EnableGameplayData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06002B44 RID: 11076 RVA: 0x0003FDC4 File Offset: 0x0003DFC4
		public void Set(ref BeginSessionOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 3;
				this.RegisterTimeoutSeconds = other.Value.RegisterTimeoutSeconds;
				this.ServerName = other.Value.ServerName;
				this.EnableGameplayData = other.Value.EnableGameplayData;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06002B45 RID: 11077 RVA: 0x0003FE39 File Offset: 0x0003E039
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ServerName);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04001289 RID: 4745
		private int m_ApiVersion;

		// Token: 0x0400128A RID: 4746
		private uint m_RegisterTimeoutSeconds;

		// Token: 0x0400128B RID: 4747
		private IntPtr m_ServerName;

		// Token: 0x0400128C RID: 4748
		private int m_EnableGameplayData;

		// Token: 0x0400128D RID: 4749
		private IntPtr m_LocalUserId;
	}
}
