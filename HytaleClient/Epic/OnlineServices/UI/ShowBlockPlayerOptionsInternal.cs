using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000089 RID: 137
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ShowBlockPlayerOptionsInternal : ISettable<ShowBlockPlayerOptions>, IDisposable
	{
		// Token: 0x170000DE RID: 222
		// (set) Token: 0x060005A3 RID: 1443 RVA: 0x00007DBB File Offset: 0x00005FBB
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170000DF RID: 223
		// (set) Token: 0x060005A4 RID: 1444 RVA: 0x00007DCB File Offset: 0x00005FCB
		public EpicAccountId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00007DDB File Offset: 0x00005FDB
		public void Set(ref ShowBlockPlayerOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x00007E00 File Offset: 0x00006000
		public void Set(ref ShowBlockPlayerOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00007E4B File Offset: 0x0000604B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x040002C4 RID: 708
		private int m_ApiVersion;

		// Token: 0x040002C5 RID: 709
		private IntPtr m_LocalUserId;

		// Token: 0x040002C6 RID: 710
		private IntPtr m_TargetUserId;
	}
}
