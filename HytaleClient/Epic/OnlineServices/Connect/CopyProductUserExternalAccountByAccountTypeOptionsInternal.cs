using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005CE RID: 1486
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CopyProductUserExternalAccountByAccountTypeOptionsInternal : ISettable<CopyProductUserExternalAccountByAccountTypeOptions>, IDisposable
	{
		// Token: 0x17000B39 RID: 2873
		// (set) Token: 0x060026AC RID: 9900 RVA: 0x000393F8 File Offset: 0x000375F8
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x17000B3A RID: 2874
		// (set) Token: 0x060026AD RID: 9901 RVA: 0x00039408 File Offset: 0x00037608
		public ExternalAccountType AccountIdType
		{
			set
			{
				this.m_AccountIdType = value;
			}
		}

		// Token: 0x060026AE RID: 9902 RVA: 0x00039412 File Offset: 0x00037612
		public void Set(ref CopyProductUserExternalAccountByAccountTypeOptions other)
		{
			this.m_ApiVersion = 1;
			this.TargetUserId = other.TargetUserId;
			this.AccountIdType = other.AccountIdType;
		}

		// Token: 0x060026AF RID: 9903 RVA: 0x00039438 File Offset: 0x00037638
		public void Set(ref CopyProductUserExternalAccountByAccountTypeOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.TargetUserId = other.Value.TargetUserId;
				this.AccountIdType = other.Value.AccountIdType;
			}
		}

		// Token: 0x060026B0 RID: 9904 RVA: 0x00039483 File Offset: 0x00037683
		public void Dispose()
		{
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x040010C9 RID: 4297
		private int m_ApiVersion;

		// Token: 0x040010CA RID: 4298
		private IntPtr m_TargetUserId;

		// Token: 0x040010CB RID: 4299
		private ExternalAccountType m_AccountIdType;
	}
}
