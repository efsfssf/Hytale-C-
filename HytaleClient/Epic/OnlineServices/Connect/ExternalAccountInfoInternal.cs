using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005E2 RID: 1506
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ExternalAccountInfoInternal : IGettable<ExternalAccountInfo>, ISettable<ExternalAccountInfo>, IDisposable
	{
		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x06002718 RID: 10008 RVA: 0x00039D40 File Offset: 0x00037F40
		// (set) Token: 0x06002719 RID: 10009 RVA: 0x00039D61 File Offset: 0x00037F61
		public ProductUserId ProductUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_ProductUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ProductUserId);
			}
		}

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x0600271A RID: 10010 RVA: 0x00039D74 File Offset: 0x00037F74
		// (set) Token: 0x0600271B RID: 10011 RVA: 0x00039D95 File Offset: 0x00037F95
		public Utf8String DisplayName
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_DisplayName, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_DisplayName);
			}
		}

		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x0600271C RID: 10012 RVA: 0x00039DA8 File Offset: 0x00037FA8
		// (set) Token: 0x0600271D RID: 10013 RVA: 0x00039DC9 File Offset: 0x00037FC9
		public Utf8String AccountId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_AccountId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_AccountId);
			}
		}

		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x0600271E RID: 10014 RVA: 0x00039DDC File Offset: 0x00037FDC
		// (set) Token: 0x0600271F RID: 10015 RVA: 0x00039DF4 File Offset: 0x00037FF4
		public ExternalAccountType AccountIdType
		{
			get
			{
				return this.m_AccountIdType;
			}
			set
			{
				this.m_AccountIdType = value;
			}
		}

		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x06002720 RID: 10016 RVA: 0x00039E00 File Offset: 0x00038000
		// (set) Token: 0x06002721 RID: 10017 RVA: 0x00039E21 File Offset: 0x00038021
		public DateTimeOffset? LastLoginTime
		{
			get
			{
				DateTimeOffset? result;
				Helper.Get(this.m_LastLoginTime, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LastLoginTime);
			}
		}

		// Token: 0x06002722 RID: 10018 RVA: 0x00039E34 File Offset: 0x00038034
		public void Set(ref ExternalAccountInfo other)
		{
			this.m_ApiVersion = 1;
			this.ProductUserId = other.ProductUserId;
			this.DisplayName = other.DisplayName;
			this.AccountId = other.AccountId;
			this.AccountIdType = other.AccountIdType;
			this.LastLoginTime = other.LastLoginTime;
		}

		// Token: 0x06002723 RID: 10019 RVA: 0x00039E8C File Offset: 0x0003808C
		public void Set(ref ExternalAccountInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.ProductUserId = other.Value.ProductUserId;
				this.DisplayName = other.Value.DisplayName;
				this.AccountId = other.Value.AccountId;
				this.AccountIdType = other.Value.AccountIdType;
				this.LastLoginTime = other.Value.LastLoginTime;
			}
		}

		// Token: 0x06002724 RID: 10020 RVA: 0x00039F16 File Offset: 0x00038116
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ProductUserId);
			Helper.Dispose(ref this.m_DisplayName);
			Helper.Dispose(ref this.m_AccountId);
		}

		// Token: 0x06002725 RID: 10021 RVA: 0x00039F3D File Offset: 0x0003813D
		public void Get(out ExternalAccountInfo output)
		{
			output = default(ExternalAccountInfo);
			output.Set(ref this);
		}

		// Token: 0x040010F3 RID: 4339
		private int m_ApiVersion;

		// Token: 0x040010F4 RID: 4340
		private IntPtr m_ProductUserId;

		// Token: 0x040010F5 RID: 4341
		private IntPtr m_DisplayName;

		// Token: 0x040010F6 RID: 4342
		private IntPtr m_AccountId;

		// Token: 0x040010F7 RID: 4343
		private ExternalAccountType m_AccountIdType;

		// Token: 0x040010F8 RID: 4344
		private long m_LastLoginTime;
	}
}
