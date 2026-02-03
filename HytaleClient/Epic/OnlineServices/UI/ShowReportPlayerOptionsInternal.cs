using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000093 RID: 147
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ShowReportPlayerOptionsInternal : ISettable<ShowReportPlayerOptions>, IDisposable
	{
		// Token: 0x170000F8 RID: 248
		// (set) Token: 0x060005E5 RID: 1509 RVA: 0x000083F4 File Offset: 0x000065F4
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170000F9 RID: 249
		// (set) Token: 0x060005E6 RID: 1510 RVA: 0x00008404 File Offset: 0x00006604
		public EpicAccountId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00008414 File Offset: 0x00006614
		public void Set(ref ShowReportPlayerOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00008438 File Offset: 0x00006638
		public void Set(ref ShowReportPlayerOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00008483 File Offset: 0x00006683
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x040002DF RID: 735
		private int m_ApiVersion;

		// Token: 0x040002E0 RID: 736
		private IntPtr m_LocalUserId;

		// Token: 0x040002E1 RID: 737
		private IntPtr m_TargetUserId;
	}
}
