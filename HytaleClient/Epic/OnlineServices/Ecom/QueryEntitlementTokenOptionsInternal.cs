using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000558 RID: 1368
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryEntitlementTokenOptionsInternal : ISettable<QueryEntitlementTokenOptions>, IDisposable
	{
		// Token: 0x17000A55 RID: 2645
		// (set) Token: 0x060023A4 RID: 9124 RVA: 0x00034689 File Offset: 0x00032889
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000A56 RID: 2646
		// (set) Token: 0x060023A5 RID: 9125 RVA: 0x00034699 File Offset: 0x00032899
		public Utf8String[] EntitlementNames
		{
			set
			{
				Helper.Set<Utf8String>(value, ref this.m_EntitlementNames, out this.m_EntitlementNameCount);
			}
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x000346AF File Offset: 0x000328AF
		public void Set(ref QueryEntitlementTokenOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.EntitlementNames = other.EntitlementNames;
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x000346D4 File Offset: 0x000328D4
		public void Set(ref QueryEntitlementTokenOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.EntitlementNames = other.Value.EntitlementNames;
			}
		}

		// Token: 0x060023A8 RID: 9128 RVA: 0x0003471F File Offset: 0x0003291F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_EntitlementNames);
		}

		// Token: 0x04000F9F RID: 3999
		private int m_ApiVersion;

		// Token: 0x04000FA0 RID: 4000
		private IntPtr m_LocalUserId;

		// Token: 0x04000FA1 RID: 4001
		private IntPtr m_EntitlementNames;

		// Token: 0x04000FA2 RID: 4002
		private uint m_EntitlementNameCount;
	}
}
