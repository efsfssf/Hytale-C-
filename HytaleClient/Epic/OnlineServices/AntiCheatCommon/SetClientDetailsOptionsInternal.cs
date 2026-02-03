using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006CA RID: 1738
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SetClientDetailsOptionsInternal : ISettable<SetClientDetailsOptions>, IDisposable
	{
		// Token: 0x17000D7E RID: 3454
		// (set) Token: 0x06002D2B RID: 11563 RVA: 0x00042B7C File Offset: 0x00040D7C
		public IntPtr ClientHandle
		{
			set
			{
				this.m_ClientHandle = value;
			}
		}

		// Token: 0x17000D7F RID: 3455
		// (set) Token: 0x06002D2C RID: 11564 RVA: 0x00042B86 File Offset: 0x00040D86
		public AntiCheatCommonClientFlags ClientFlags
		{
			set
			{
				this.m_ClientFlags = value;
			}
		}

		// Token: 0x17000D80 RID: 3456
		// (set) Token: 0x06002D2D RID: 11565 RVA: 0x00042B90 File Offset: 0x00040D90
		public AntiCheatCommonClientInput ClientInputMethod
		{
			set
			{
				this.m_ClientInputMethod = value;
			}
		}

		// Token: 0x06002D2E RID: 11566 RVA: 0x00042B9A File Offset: 0x00040D9A
		public void Set(ref SetClientDetailsOptions other)
		{
			this.m_ApiVersion = 1;
			this.ClientHandle = other.ClientHandle;
			this.ClientFlags = other.ClientFlags;
			this.ClientInputMethod = other.ClientInputMethod;
		}

		// Token: 0x06002D2F RID: 11567 RVA: 0x00042BCC File Offset: 0x00040DCC
		public void Set(ref SetClientDetailsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.ClientHandle = other.Value.ClientHandle;
				this.ClientFlags = other.Value.ClientFlags;
				this.ClientInputMethod = other.Value.ClientInputMethod;
			}
		}

		// Token: 0x06002D30 RID: 11568 RVA: 0x00042C2C File Offset: 0x00040E2C
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientHandle);
		}

		// Token: 0x040013E0 RID: 5088
		private int m_ApiVersion;

		// Token: 0x040013E1 RID: 5089
		private IntPtr m_ClientHandle;

		// Token: 0x040013E2 RID: 5090
		private AntiCheatCommonClientFlags m_ClientFlags;

		// Token: 0x040013E3 RID: 5091
		private AntiCheatCommonClientInput m_ClientInputMethod;
	}
}
