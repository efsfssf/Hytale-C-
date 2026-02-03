using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Stats
{
	// Token: 0x020000C5 RID: 197
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyStatByNameOptionsInternal : ISettable<CopyStatByNameOptions>, IDisposable
	{
		// Token: 0x1700015E RID: 350
		// (set) Token: 0x06000745 RID: 1861 RVA: 0x0000A838 File Offset: 0x00008A38
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x1700015F RID: 351
		// (set) Token: 0x06000746 RID: 1862 RVA: 0x0000A848 File Offset: 0x00008A48
		public Utf8String Name
		{
			set
			{
				Helper.Set(value, ref this.m_Name);
			}
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0000A858 File Offset: 0x00008A58
		public void Set(ref CopyStatByNameOptions other)
		{
			this.m_ApiVersion = 1;
			this.TargetUserId = other.TargetUserId;
			this.Name = other.Name;
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0000A87C File Offset: 0x00008A7C
		public void Set(ref CopyStatByNameOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.TargetUserId = other.Value.TargetUserId;
				this.Name = other.Value.Name;
			}
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0000A8C7 File Offset: 0x00008AC7
		public void Dispose()
		{
			Helper.Dispose(ref this.m_TargetUserId);
			Helper.Dispose(ref this.m_Name);
		}

		// Token: 0x04000381 RID: 897
		private int m_ApiVersion;

		// Token: 0x04000382 RID: 898
		private IntPtr m_TargetUserId;

		// Token: 0x04000383 RID: 899
		private IntPtr m_Name;
	}
}
