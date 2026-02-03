using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005E8 RID: 1512
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct GetProductUserIdMappingOptionsInternal : ISettable<GetProductUserIdMappingOptions>, IDisposable
	{
		// Token: 0x17000B6F RID: 2927
		// (set) Token: 0x0600273E RID: 10046 RVA: 0x0003A10C File Offset: 0x0003830C
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000B70 RID: 2928
		// (set) Token: 0x0600273F RID: 10047 RVA: 0x0003A11C File Offset: 0x0003831C
		public ExternalAccountType AccountIdType
		{
			set
			{
				this.m_AccountIdType = value;
			}
		}

		// Token: 0x17000B71 RID: 2929
		// (set) Token: 0x06002740 RID: 10048 RVA: 0x0003A126 File Offset: 0x00038326
		public ProductUserId TargetProductUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetProductUserId);
			}
		}

		// Token: 0x06002741 RID: 10049 RVA: 0x0003A136 File Offset: 0x00038336
		public void Set(ref GetProductUserIdMappingOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.AccountIdType = other.AccountIdType;
			this.TargetProductUserId = other.TargetProductUserId;
		}

		// Token: 0x06002742 RID: 10050 RVA: 0x0003A168 File Offset: 0x00038368
		public void Set(ref GetProductUserIdMappingOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.AccountIdType = other.Value.AccountIdType;
				this.TargetProductUserId = other.Value.TargetProductUserId;
			}
		}

		// Token: 0x06002743 RID: 10051 RVA: 0x0003A1C8 File Offset: 0x000383C8
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetProductUserId);
		}

		// Token: 0x04001106 RID: 4358
		private int m_ApiVersion;

		// Token: 0x04001107 RID: 4359
		private IntPtr m_LocalUserId;

		// Token: 0x04001108 RID: 4360
		private ExternalAccountType m_AccountIdType;

		// Token: 0x04001109 RID: 4361
		private IntPtr m_TargetProductUserId;
	}
}
