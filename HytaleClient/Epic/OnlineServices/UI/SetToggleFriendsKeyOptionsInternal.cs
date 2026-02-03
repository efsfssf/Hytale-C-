using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000087 RID: 135
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SetToggleFriendsKeyOptionsInternal : ISettable<SetToggleFriendsKeyOptions>, IDisposable
	{
		// Token: 0x170000DB RID: 219
		// (set) Token: 0x0600059B RID: 1435 RVA: 0x00007D3E File Offset: 0x00005F3E
		public KeyCombination KeyCombination
		{
			set
			{
				this.m_KeyCombination = value;
			}
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x00007D48 File Offset: 0x00005F48
		public void Set(ref SetToggleFriendsKeyOptions other)
		{
			this.m_ApiVersion = 1;
			this.KeyCombination = other.KeyCombination;
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x00007D60 File Offset: 0x00005F60
		public void Set(ref SetToggleFriendsKeyOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.KeyCombination = other.Value.KeyCombination;
			}
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00007D96 File Offset: 0x00005F96
		public void Dispose()
		{
		}

		// Token: 0x040002C0 RID: 704
		private int m_ApiVersion;

		// Token: 0x040002C1 RID: 705
		private KeyCombination m_KeyCombination;
	}
}
