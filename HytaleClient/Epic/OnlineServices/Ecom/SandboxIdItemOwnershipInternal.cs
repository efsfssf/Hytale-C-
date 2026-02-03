using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200056E RID: 1390
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SandboxIdItemOwnershipInternal : IGettable<SandboxIdItemOwnership>, ISettable<SandboxIdItemOwnership>, IDisposable
	{
		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x06002450 RID: 9296 RVA: 0x00035740 File Offset: 0x00033940
		// (set) Token: 0x06002451 RID: 9297 RVA: 0x00035761 File Offset: 0x00033961
		public Utf8String SandboxId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_SandboxId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_SandboxId);
			}
		}

		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x06002452 RID: 9298 RVA: 0x00035774 File Offset: 0x00033974
		// (set) Token: 0x06002453 RID: 9299 RVA: 0x0003579B File Offset: 0x0003399B
		public Utf8String[] OwnedCatalogItemIds
		{
			get
			{
				Utf8String[] result;
				Helper.Get<Utf8String>(this.m_OwnedCatalogItemIds, out result, this.m_OwnedCatalogItemIdsCount);
				return result;
			}
			set
			{
				Helper.Set<Utf8String>(value, ref this.m_OwnedCatalogItemIds, out this.m_OwnedCatalogItemIdsCount);
			}
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x000357B1 File Offset: 0x000339B1
		public void Set(ref SandboxIdItemOwnership other)
		{
			this.SandboxId = other.SandboxId;
			this.OwnedCatalogItemIds = other.OwnedCatalogItemIds;
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x000357D0 File Offset: 0x000339D0
		public void Set(ref SandboxIdItemOwnership? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.SandboxId = other.Value.SandboxId;
				this.OwnedCatalogItemIds = other.Value.OwnedCatalogItemIds;
			}
		}

		// Token: 0x06002456 RID: 9302 RVA: 0x00035814 File Offset: 0x00033A14
		public void Dispose()
		{
			Helper.Dispose(ref this.m_SandboxId);
			Helper.Dispose(ref this.m_OwnedCatalogItemIds);
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x0003582F File Offset: 0x00033A2F
		public void Get(out SandboxIdItemOwnership output)
		{
			output = default(SandboxIdItemOwnership);
			output.Set(ref this);
		}

		// Token: 0x04000FEE RID: 4078
		private IntPtr m_SandboxId;

		// Token: 0x04000FEF RID: 4079
		private IntPtr m_OwnedCatalogItemIds;

		// Token: 0x04000FF0 RID: 4080
		private uint m_OwnedCatalogItemIdsCount;
	}
}
