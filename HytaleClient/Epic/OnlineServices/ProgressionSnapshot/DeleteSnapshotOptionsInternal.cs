using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ProgressionSnapshot
{
	// Token: 0x020002AC RID: 684
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DeleteSnapshotOptionsInternal : ISettable<DeleteSnapshotOptions>, IDisposable
	{
		// Token: 0x17000517 RID: 1303
		// (set) Token: 0x06001315 RID: 4885 RVA: 0x0001BCF7 File Offset: 0x00019EF7
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x0001BD07 File Offset: 0x00019F07
		public void Set(ref DeleteSnapshotOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x0001BD20 File Offset: 0x00019F20
		public void Set(ref DeleteSnapshotOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x0001BD56 File Offset: 0x00019F56
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000863 RID: 2147
		private int m_ApiVersion;

		// Token: 0x04000864 RID: 2148
		private IntPtr m_LocalUserId;
	}
}
