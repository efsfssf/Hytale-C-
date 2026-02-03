using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200043A RID: 1082
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ParseConnectStringOptionsInternal : ISettable<ParseConnectStringOptions>, IDisposable
	{
		// Token: 0x170007E5 RID: 2021
		// (set) Token: 0x06001C66 RID: 7270 RVA: 0x000297BF File Offset: 0x000279BF
		public Utf8String ConnectString
		{
			set
			{
				Helper.Set(value, ref this.m_ConnectString);
			}
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x000297CF File Offset: 0x000279CF
		public void Set(ref ParseConnectStringOptions other)
		{
			this.m_ApiVersion = 1;
			this.ConnectString = other.ConnectString;
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x000297E8 File Offset: 0x000279E8
		public void Set(ref ParseConnectStringOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.ConnectString = other.Value.ConnectString;
			}
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x0002981E File Offset: 0x00027A1E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ConnectString);
		}

		// Token: 0x04000C65 RID: 3173
		private int m_ApiVersion;

		// Token: 0x04000C66 RID: 3174
		private IntPtr m_ConnectString;
	}
}
