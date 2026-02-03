using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sanctions
{
	// Token: 0x020001A8 RID: 424
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryActivePlayerSanctionsOptionsInternal : ISettable<QueryActivePlayerSanctionsOptions>, IDisposable
	{
		// Token: 0x170002DE RID: 734
		// (set) Token: 0x06000C4B RID: 3147 RVA: 0x00011E49 File Offset: 0x00010049
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x170002DF RID: 735
		// (set) Token: 0x06000C4C RID: 3148 RVA: 0x00011E59 File Offset: 0x00010059
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x00011E69 File Offset: 0x00010069
		public void Set(ref QueryActivePlayerSanctionsOptions other)
		{
			this.m_ApiVersion = 2;
			this.TargetUserId = other.TargetUserId;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x00011E90 File Offset: 0x00010090
		public void Set(ref QueryActivePlayerSanctionsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.TargetUserId = other.Value.TargetUserId;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x00011EDB File Offset: 0x000100DB
		public void Dispose()
		{
			Helper.Dispose(ref this.m_TargetUserId);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000599 RID: 1433
		private int m_ApiVersion;

		// Token: 0x0400059A RID: 1434
		private IntPtr m_TargetUserId;

		// Token: 0x0400059B RID: 1435
		private IntPtr m_LocalUserId;
	}
}
