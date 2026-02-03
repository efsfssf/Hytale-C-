using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006F3 RID: 1779
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct PollStatusOptionsInternal : ISettable<PollStatusOptions>, IDisposable
	{
		// Token: 0x17000D9E RID: 3486
		// (set) Token: 0x06002DD8 RID: 11736 RVA: 0x00043AE6 File Offset: 0x00041CE6
		public uint OutMessageLength
		{
			set
			{
				this.m_OutMessageLength = value;
			}
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x00043AF0 File Offset: 0x00041CF0
		public void Set(ref PollStatusOptions other)
		{
			this.m_ApiVersion = 1;
			this.OutMessageLength = other.OutMessageLength;
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x00043B08 File Offset: 0x00041D08
		public void Set(ref PollStatusOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.OutMessageLength = other.Value.OutMessageLength;
			}
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x00043B3E File Offset: 0x00041D3E
		public void Dispose()
		{
		}

		// Token: 0x04001435 RID: 5173
		private int m_ApiVersion;

		// Token: 0x04001436 RID: 5174
		private uint m_OutMessageLength;
	}
}
