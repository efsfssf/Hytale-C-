using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x020007AB RID: 1963
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SetPortRangeOptionsInternal : ISettable<SetPortRangeOptions>, IDisposable
	{
		// Token: 0x17000F6D RID: 3949
		// (set) Token: 0x060032E3 RID: 13027 RVA: 0x0004BDC4 File Offset: 0x00049FC4
		public ushort Port
		{
			set
			{
				this.m_Port = value;
			}
		}

		// Token: 0x17000F6E RID: 3950
		// (set) Token: 0x060032E4 RID: 13028 RVA: 0x0004BDCE File Offset: 0x00049FCE
		public ushort MaxAdditionalPortsToTry
		{
			set
			{
				this.m_MaxAdditionalPortsToTry = value;
			}
		}

		// Token: 0x060032E5 RID: 13029 RVA: 0x0004BDD8 File Offset: 0x00049FD8
		public void Set(ref SetPortRangeOptions other)
		{
			this.m_ApiVersion = 1;
			this.Port = other.Port;
			this.MaxAdditionalPortsToTry = other.MaxAdditionalPortsToTry;
		}

		// Token: 0x060032E6 RID: 13030 RVA: 0x0004BDFC File Offset: 0x00049FFC
		public void Set(ref SetPortRangeOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Port = other.Value.Port;
				this.MaxAdditionalPortsToTry = other.Value.MaxAdditionalPortsToTry;
			}
		}

		// Token: 0x060032E7 RID: 13031 RVA: 0x0004BE47 File Offset: 0x0004A047
		public void Dispose()
		{
		}

		// Token: 0x040016D4 RID: 5844
		private int m_ApiVersion;

		// Token: 0x040016D5 RID: 5845
		private ushort m_Port;

		// Token: 0x040016D6 RID: 5846
		private ushort m_MaxAdditionalPortsToTry;
	}
}
