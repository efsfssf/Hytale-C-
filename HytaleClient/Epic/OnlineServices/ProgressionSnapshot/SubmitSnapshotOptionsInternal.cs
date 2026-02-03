using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ProgressionSnapshot
{
	// Token: 0x020002B7 RID: 695
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SubmitSnapshotOptionsInternal : ISettable<SubmitSnapshotOptions>, IDisposable
	{
		// Token: 0x17000522 RID: 1314
		// (set) Token: 0x0600134D RID: 4941 RVA: 0x0001C173 File Offset: 0x0001A373
		public uint SnapshotId
		{
			set
			{
				this.m_SnapshotId = value;
			}
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x0001C17D File Offset: 0x0001A37D
		public void Set(ref SubmitSnapshotOptions other)
		{
			this.m_ApiVersion = 1;
			this.SnapshotId = other.SnapshotId;
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x0001C194 File Offset: 0x0001A394
		public void Set(ref SubmitSnapshotOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.SnapshotId = other.Value.SnapshotId;
			}
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x0001C1CA File Offset: 0x0001A3CA
		public void Dispose()
		{
		}

		// Token: 0x04000875 RID: 2165
		private int m_ApiVersion;

		// Token: 0x04000876 RID: 2166
		private uint m_SnapshotId;
	}
}
