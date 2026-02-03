using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ProgressionSnapshot
{
	// Token: 0x020002A8 RID: 680
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct BeginSnapshotOptionsInternal : ISettable<BeginSnapshotOptions>, IDisposable
	{
		// Token: 0x1700050E RID: 1294
		// (set) Token: 0x060012FC RID: 4860 RVA: 0x0001BAA8 File Offset: 0x00019CA8
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x0001BAB8 File Offset: 0x00019CB8
		public void Set(ref BeginSnapshotOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x0001BAD0 File Offset: 0x00019CD0
		public void Set(ref BeginSnapshotOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x0001BB06 File Offset: 0x00019D06
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x0400085A RID: 2138
		private int m_ApiVersion;

		// Token: 0x0400085B RID: 2139
		private IntPtr m_LocalUserId;
	}
}
