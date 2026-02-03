using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000560 RID: 1376
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryOwnershipBySandboxIdsOptionsInternal : ISettable<QueryOwnershipBySandboxIdsOptions>, IDisposable
	{
		// Token: 0x17000A6D RID: 2669
		// (set) Token: 0x060023E0 RID: 9184 RVA: 0x00034C55 File Offset: 0x00032E55
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000A6E RID: 2670
		// (set) Token: 0x060023E1 RID: 9185 RVA: 0x00034C65 File Offset: 0x00032E65
		public Utf8String[] SandboxIds
		{
			set
			{
				Helper.Set<Utf8String>(value, ref this.m_SandboxIds, out this.m_SandboxIdsCount);
			}
		}

		// Token: 0x060023E2 RID: 9186 RVA: 0x00034C7B File Offset: 0x00032E7B
		public void Set(ref QueryOwnershipBySandboxIdsOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.SandboxIds = other.SandboxIds;
		}

		// Token: 0x060023E3 RID: 9187 RVA: 0x00034CA0 File Offset: 0x00032EA0
		public void Set(ref QueryOwnershipBySandboxIdsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.SandboxIds = other.Value.SandboxIds;
			}
		}

		// Token: 0x060023E4 RID: 9188 RVA: 0x00034CEB File Offset: 0x00032EEB
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_SandboxIds);
		}

		// Token: 0x04000FB9 RID: 4025
		private int m_ApiVersion;

		// Token: 0x04000FBA RID: 4026
		private IntPtr m_LocalUserId;

		// Token: 0x04000FBB RID: 4027
		private IntPtr m_SandboxIds;

		// Token: 0x04000FBC RID: 4028
		private uint m_SandboxIdsCount;
	}
}
