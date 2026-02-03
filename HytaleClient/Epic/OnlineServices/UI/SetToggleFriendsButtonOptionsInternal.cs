using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000085 RID: 133
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SetToggleFriendsButtonOptionsInternal : ISettable<SetToggleFriendsButtonOptions>, IDisposable
	{
		// Token: 0x170000D9 RID: 217
		// (set) Token: 0x06000595 RID: 1429 RVA: 0x00007CD2 File Offset: 0x00005ED2
		public InputStateButtonFlags ButtonCombination
		{
			set
			{
				this.m_ButtonCombination = value;
			}
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x00007CDC File Offset: 0x00005EDC
		public void Set(ref SetToggleFriendsButtonOptions other)
		{
			this.m_ApiVersion = 1;
			this.ButtonCombination = other.ButtonCombination;
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x00007CF4 File Offset: 0x00005EF4
		public void Set(ref SetToggleFriendsButtonOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.ButtonCombination = other.Value.ButtonCombination;
			}
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x00007D2A File Offset: 0x00005F2A
		public void Dispose()
		{
		}

		// Token: 0x040002BD RID: 701
		private int m_ApiVersion;

		// Token: 0x040002BE RID: 702
		private InputStateButtonFlags m_ButtonCombination;
	}
}
