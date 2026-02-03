using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000631 RID: 1585
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyIdTokenOptionsInternal : ISettable<CopyIdTokenOptions>, IDisposable
	{
		// Token: 0x17000BF2 RID: 3058
		// (set) Token: 0x06002910 RID: 10512 RVA: 0x0003C861 File Offset: 0x0003AA61
		public EpicAccountId AccountId
		{
			set
			{
				Helper.Set(value, ref this.m_AccountId);
			}
		}

		// Token: 0x06002911 RID: 10513 RVA: 0x0003C871 File Offset: 0x0003AA71
		public void Set(ref CopyIdTokenOptions other)
		{
			this.m_ApiVersion = 1;
			this.AccountId = other.AccountId;
		}

		// Token: 0x06002912 RID: 10514 RVA: 0x0003C888 File Offset: 0x0003AA88
		public void Set(ref CopyIdTokenOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.AccountId = other.Value.AccountId;
			}
		}

		// Token: 0x06002913 RID: 10515 RVA: 0x0003C8BE File Offset: 0x0003AABE
		public void Dispose()
		{
			Helper.Dispose(ref this.m_AccountId);
		}

		// Token: 0x040011AA RID: 4522
		private int m_ApiVersion;

		// Token: 0x040011AB RID: 4523
		private IntPtr m_AccountId;
	}
}
