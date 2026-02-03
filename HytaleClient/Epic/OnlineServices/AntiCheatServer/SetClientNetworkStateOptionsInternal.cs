using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatServer
{
	// Token: 0x0200068F RID: 1679
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SetClientNetworkStateOptionsInternal : ISettable<SetClientNetworkStateOptions>, IDisposable
	{
		// Token: 0x17000CC7 RID: 3271
		// (set) Token: 0x06002B98 RID: 11160 RVA: 0x00040318 File Offset: 0x0003E518
		public IntPtr ClientHandle
		{
			set
			{
				this.m_ClientHandle = value;
			}
		}

		// Token: 0x17000CC8 RID: 3272
		// (set) Token: 0x06002B99 RID: 11161 RVA: 0x00040322 File Offset: 0x0003E522
		public bool IsNetworkActive
		{
			set
			{
				Helper.Set(value, ref this.m_IsNetworkActive);
			}
		}

		// Token: 0x06002B9A RID: 11162 RVA: 0x00040332 File Offset: 0x0003E532
		public void Set(ref SetClientNetworkStateOptions other)
		{
			this.m_ApiVersion = 1;
			this.ClientHandle = other.ClientHandle;
			this.IsNetworkActive = other.IsNetworkActive;
		}

		// Token: 0x06002B9B RID: 11163 RVA: 0x00040358 File Offset: 0x0003E558
		public void Set(ref SetClientNetworkStateOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.ClientHandle = other.Value.ClientHandle;
				this.IsNetworkActive = other.Value.IsNetworkActive;
			}
		}

		// Token: 0x06002B9C RID: 11164 RVA: 0x000403A3 File Offset: 0x0003E5A3
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientHandle);
		}

		// Token: 0x040012B1 RID: 4785
		private int m_ApiVersion;

		// Token: 0x040012B2 RID: 4786
		private IntPtr m_ClientHandle;

		// Token: 0x040012B3 RID: 4787
		private int m_IsNetworkActive;
	}
}
