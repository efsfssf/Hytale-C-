using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ProgressionSnapshot
{
	// Token: 0x020002A6 RID: 678
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddProgressionOptionsInternal : ISettable<AddProgressionOptions>, IDisposable
	{
		// Token: 0x1700050A RID: 1290
		// (set) Token: 0x060012F4 RID: 4852 RVA: 0x0001B9C0 File Offset: 0x00019BC0
		public uint SnapshotId
		{
			set
			{
				this.m_SnapshotId = value;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (set) Token: 0x060012F5 RID: 4853 RVA: 0x0001B9CA File Offset: 0x00019BCA
		public Utf8String Key
		{
			set
			{
				Helper.Set(value, ref this.m_Key);
			}
		}

		// Token: 0x1700050C RID: 1292
		// (set) Token: 0x060012F6 RID: 4854 RVA: 0x0001B9DA File Offset: 0x00019BDA
		public Utf8String Value
		{
			set
			{
				Helper.Set(value, ref this.m_Value);
			}
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x0001B9EA File Offset: 0x00019BEA
		public void Set(ref AddProgressionOptions other)
		{
			this.m_ApiVersion = 1;
			this.SnapshotId = other.SnapshotId;
			this.Key = other.Key;
			this.Value = other.Value;
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x0001BA1C File Offset: 0x00019C1C
		public void Set(ref AddProgressionOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.SnapshotId = other.Value.SnapshotId;
				this.Key = other.Value.Key;
				this.Value = other.Value.Value;
			}
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x0001BA7C File Offset: 0x00019C7C
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Key);
			Helper.Dispose(ref this.m_Value);
		}

		// Token: 0x04000855 RID: 2133
		private int m_ApiVersion;

		// Token: 0x04000856 RID: 2134
		private uint m_SnapshotId;

		// Token: 0x04000857 RID: 2135
		private IntPtr m_Key;

		// Token: 0x04000858 RID: 2136
		private IntPtr m_Value;
	}
}
