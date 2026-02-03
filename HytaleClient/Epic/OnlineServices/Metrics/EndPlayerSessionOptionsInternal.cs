using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Metrics
{
	// Token: 0x02000352 RID: 850
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct EndPlayerSessionOptionsInternal : ISettable<EndPlayerSessionOptions>, IDisposable
	{
		// Token: 0x17000667 RID: 1639
		// (set) Token: 0x06001748 RID: 5960 RVA: 0x00022015 File Offset: 0x00020215
		public EndPlayerSessionOptionsAccountId AccountId
		{
			set
			{
				Helper.Set<EndPlayerSessionOptionsAccountId, EndPlayerSessionOptionsAccountIdInternal>(ref value, ref this.m_AccountId);
			}
		}

		// Token: 0x06001749 RID: 5961 RVA: 0x00022026 File Offset: 0x00020226
		public void Set(ref EndPlayerSessionOptions other)
		{
			this.m_ApiVersion = 1;
			this.AccountId = other.AccountId;
		}

		// Token: 0x0600174A RID: 5962 RVA: 0x00022040 File Offset: 0x00020240
		public void Set(ref EndPlayerSessionOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.AccountId = other.Value.AccountId;
			}
		}

		// Token: 0x0600174B RID: 5963 RVA: 0x00022076 File Offset: 0x00020276
		public void Dispose()
		{
			Helper.Dispose<EndPlayerSessionOptionsAccountIdInternal>(ref this.m_AccountId);
		}

		// Token: 0x04000A20 RID: 2592
		private int m_ApiVersion;

		// Token: 0x04000A21 RID: 2593
		private EndPlayerSessionOptionsAccountIdInternal m_AccountId;
	}
}
