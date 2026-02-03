using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005E4 RID: 1508
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetExternalAccountMappingsOptionsInternal : ISettable<GetExternalAccountMappingsOptions>, IDisposable
	{
		// Token: 0x17000B67 RID: 2919
		// (set) Token: 0x0600272C RID: 10028 RVA: 0x00039F82 File Offset: 0x00038182
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000B68 RID: 2920
		// (set) Token: 0x0600272D RID: 10029 RVA: 0x00039F92 File Offset: 0x00038192
		public ExternalAccountType AccountIdType
		{
			set
			{
				this.m_AccountIdType = value;
			}
		}

		// Token: 0x17000B69 RID: 2921
		// (set) Token: 0x0600272E RID: 10030 RVA: 0x00039F9C File Offset: 0x0003819C
		public Utf8String TargetExternalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetExternalUserId);
			}
		}

		// Token: 0x0600272F RID: 10031 RVA: 0x00039FAC File Offset: 0x000381AC
		public void Set(ref GetExternalAccountMappingsOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.AccountIdType = other.AccountIdType;
			this.TargetExternalUserId = other.TargetExternalUserId;
		}

		// Token: 0x06002730 RID: 10032 RVA: 0x00039FE0 File Offset: 0x000381E0
		public void Set(ref GetExternalAccountMappingsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.AccountIdType = other.Value.AccountIdType;
				this.TargetExternalUserId = other.Value.TargetExternalUserId;
			}
		}

		// Token: 0x06002731 RID: 10033 RVA: 0x0003A040 File Offset: 0x00038240
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetExternalUserId);
		}

		// Token: 0x040010FC RID: 4348
		private int m_ApiVersion;

		// Token: 0x040010FD RID: 4349
		private IntPtr m_LocalUserId;

		// Token: 0x040010FE RID: 4350
		private ExternalAccountType m_AccountIdType;

		// Token: 0x040010FF RID: 4351
		private IntPtr m_TargetExternalUserId;
	}
}
