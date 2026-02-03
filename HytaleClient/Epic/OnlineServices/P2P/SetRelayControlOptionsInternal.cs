using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x020007AD RID: 1965
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SetRelayControlOptionsInternal : ISettable<SetRelayControlOptions>, IDisposable
	{
		// Token: 0x17000F70 RID: 3952
		// (set) Token: 0x060032EA RID: 13034 RVA: 0x0004BE5B File Offset: 0x0004A05B
		public RelayControl RelayControl
		{
			set
			{
				this.m_RelayControl = value;
			}
		}

		// Token: 0x060032EB RID: 13035 RVA: 0x0004BE65 File Offset: 0x0004A065
		public void Set(ref SetRelayControlOptions other)
		{
			this.m_ApiVersion = 1;
			this.RelayControl = other.RelayControl;
		}

		// Token: 0x060032EC RID: 13036 RVA: 0x0004BE7C File Offset: 0x0004A07C
		public void Set(ref SetRelayControlOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.RelayControl = other.Value.RelayControl;
			}
		}

		// Token: 0x060032ED RID: 13037 RVA: 0x0004BEB2 File Offset: 0x0004A0B2
		public void Dispose()
		{
		}

		// Token: 0x040016D8 RID: 5848
		private int m_ApiVersion;

		// Token: 0x040016D9 RID: 5849
		private RelayControl m_RelayControl;
	}
}
