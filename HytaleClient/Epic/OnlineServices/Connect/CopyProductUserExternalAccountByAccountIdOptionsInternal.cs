using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005CC RID: 1484
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyProductUserExternalAccountByAccountIdOptionsInternal : ISettable<CopyProductUserExternalAccountByAccountIdOptions>, IDisposable
	{
		// Token: 0x17000B35 RID: 2869
		// (set) Token: 0x060026A3 RID: 9891 RVA: 0x0003932B File Offset: 0x0003752B
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x17000B36 RID: 2870
		// (set) Token: 0x060026A4 RID: 9892 RVA: 0x0003933B File Offset: 0x0003753B
		public Utf8String AccountId
		{
			set
			{
				Helper.Set(value, ref this.m_AccountId);
			}
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x0003934B File Offset: 0x0003754B
		public void Set(ref CopyProductUserExternalAccountByAccountIdOptions other)
		{
			this.m_ApiVersion = 1;
			this.TargetUserId = other.TargetUserId;
			this.AccountId = other.AccountId;
		}

		// Token: 0x060026A6 RID: 9894 RVA: 0x00039370 File Offset: 0x00037570
		public void Set(ref CopyProductUserExternalAccountByAccountIdOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.TargetUserId = other.Value.TargetUserId;
				this.AccountId = other.Value.AccountId;
			}
		}

		// Token: 0x060026A7 RID: 9895 RVA: 0x000393BB File Offset: 0x000375BB
		public void Dispose()
		{
			Helper.Dispose(ref this.m_TargetUserId);
			Helper.Dispose(ref this.m_AccountId);
		}

		// Token: 0x040010C4 RID: 4292
		private int m_ApiVersion;

		// Token: 0x040010C5 RID: 4293
		private IntPtr m_TargetUserId;

		// Token: 0x040010C6 RID: 4294
		private IntPtr m_AccountId;
	}
}
