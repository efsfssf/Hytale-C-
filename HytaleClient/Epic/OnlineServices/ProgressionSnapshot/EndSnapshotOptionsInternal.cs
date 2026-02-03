using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ProgressionSnapshot
{
	// Token: 0x020002AE RID: 686
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct EndSnapshotOptionsInternal : ISettable<EndSnapshotOptions>, IDisposable
	{
		// Token: 0x17000519 RID: 1305
		// (set) Token: 0x0600131B RID: 4891 RVA: 0x0001BD76 File Offset: 0x00019F76
		public uint SnapshotId
		{
			set
			{
				this.m_SnapshotId = value;
			}
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x0001BD80 File Offset: 0x00019F80
		public void Set(ref EndSnapshotOptions other)
		{
			this.m_ApiVersion = 1;
			this.SnapshotId = other.SnapshotId;
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x0001BD98 File Offset: 0x00019F98
		public void Set(ref EndSnapshotOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.SnapshotId = other.Value.SnapshotId;
			}
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x0001BDCE File Offset: 0x00019FCE
		public void Dispose()
		{
		}

		// Token: 0x04000866 RID: 2150
		private int m_ApiVersion;

		// Token: 0x04000867 RID: 2151
		private uint m_SnapshotId;
	}
}
