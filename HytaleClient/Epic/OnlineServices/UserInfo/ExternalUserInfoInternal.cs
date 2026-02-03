using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x02000033 RID: 51
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ExternalUserInfoInternal : IGettable<ExternalUserInfo>, ISettable<ExternalUserInfo>, IDisposable
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x00005434 File Offset: 0x00003634
		// (set) Token: 0x060003B5 RID: 949 RVA: 0x0000544C File Offset: 0x0000364C
		public ExternalAccountType AccountType
		{
			get
			{
				return this.m_AccountType;
			}
			set
			{
				this.m_AccountType = value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x00005458 File Offset: 0x00003658
		// (set) Token: 0x060003B7 RID: 951 RVA: 0x00005479 File Offset: 0x00003679
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

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x0000548C File Offset: 0x0000368C
		// (set) Token: 0x060003B9 RID: 953 RVA: 0x000054AD File Offset: 0x000036AD
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

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060003BA RID: 954 RVA: 0x000054C0 File Offset: 0x000036C0
		// (set) Token: 0x060003BB RID: 955 RVA: 0x000054E1 File Offset: 0x000036E1
		public Utf8String DisplayNameSanitized
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_DisplayNameSanitized, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_DisplayNameSanitized);
			}
		}

		// Token: 0x060003BC RID: 956 RVA: 0x000054F1 File Offset: 0x000036F1
		public void Set(ref ExternalUserInfo other)
		{
			this.m_ApiVersion = 2;
			this.AccountType = other.AccountType;
			this.AccountId = other.AccountId;
			this.DisplayName = other.DisplayName;
			this.DisplayNameSanitized = other.DisplayNameSanitized;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00005530 File Offset: 0x00003730
		public void Set(ref ExternalUserInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.AccountType = other.Value.AccountType;
				this.AccountId = other.Value.AccountId;
				this.DisplayName = other.Value.DisplayName;
				this.DisplayNameSanitized = other.Value.DisplayNameSanitized;
			}
		}

		// Token: 0x060003BE RID: 958 RVA: 0x000055A5 File Offset: 0x000037A5
		public void Dispose()
		{
			Helper.Dispose(ref this.m_AccountId);
			Helper.Dispose(ref this.m_DisplayName);
			Helper.Dispose(ref this.m_DisplayNameSanitized);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x000055CC File Offset: 0x000037CC
		public void Get(out ExternalUserInfo output)
		{
			output = default(ExternalUserInfo);
			output.Set(ref this);
		}

		// Token: 0x04000181 RID: 385
		private int m_ApiVersion;

		// Token: 0x04000182 RID: 386
		private ExternalAccountType m_AccountType;

		// Token: 0x04000183 RID: 387
		private IntPtr m_AccountId;

		// Token: 0x04000184 RID: 388
		private IntPtr m_DisplayName;

		// Token: 0x04000185 RID: 389
		private IntPtr m_DisplayNameSanitized;
	}
}
