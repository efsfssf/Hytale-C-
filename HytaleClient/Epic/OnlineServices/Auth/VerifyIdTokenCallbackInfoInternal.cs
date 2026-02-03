using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000666 RID: 1638
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct VerifyIdTokenCallbackInfoInternal : ICallbackInfoInternal, IGettable<VerifyIdTokenCallbackInfo>, ISettable<VerifyIdTokenCallbackInfo>, IDisposable
	{
		// Token: 0x17000C76 RID: 3190
		// (get) Token: 0x06002A96 RID: 10902 RVA: 0x0003E82C File Offset: 0x0003CA2C
		// (set) Token: 0x06002A97 RID: 10903 RVA: 0x0003E844 File Offset: 0x0003CA44
		public Result ResultCode
		{
			get
			{
				return this.m_ResultCode;
			}
			set
			{
				this.m_ResultCode = value;
			}
		}

		// Token: 0x17000C77 RID: 3191
		// (get) Token: 0x06002A98 RID: 10904 RVA: 0x0003E850 File Offset: 0x0003CA50
		// (set) Token: 0x06002A99 RID: 10905 RVA: 0x0003E871 File Offset: 0x0003CA71
		public object ClientData
		{
			get
			{
				object result;
				Helper.Get(this.m_ClientData, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ClientData);
			}
		}

		// Token: 0x17000C78 RID: 3192
		// (get) Token: 0x06002A9A RID: 10906 RVA: 0x0003E884 File Offset: 0x0003CA84
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000C79 RID: 3193
		// (get) Token: 0x06002A9B RID: 10907 RVA: 0x0003E89C File Offset: 0x0003CA9C
		// (set) Token: 0x06002A9C RID: 10908 RVA: 0x0003E8BD File Offset: 0x0003CABD
		public Utf8String ApplicationId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_ApplicationId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ApplicationId);
			}
		}

		// Token: 0x17000C7A RID: 3194
		// (get) Token: 0x06002A9D RID: 10909 RVA: 0x0003E8D0 File Offset: 0x0003CAD0
		// (set) Token: 0x06002A9E RID: 10910 RVA: 0x0003E8F1 File Offset: 0x0003CAF1
		public Utf8String ClientId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_ClientId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ClientId);
			}
		}

		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x06002A9F RID: 10911 RVA: 0x0003E904 File Offset: 0x0003CB04
		// (set) Token: 0x06002AA0 RID: 10912 RVA: 0x0003E925 File Offset: 0x0003CB25
		public Utf8String ProductId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_ProductId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ProductId);
			}
		}

		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x06002AA1 RID: 10913 RVA: 0x0003E938 File Offset: 0x0003CB38
		// (set) Token: 0x06002AA2 RID: 10914 RVA: 0x0003E959 File Offset: 0x0003CB59
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

		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x06002AA3 RID: 10915 RVA: 0x0003E96C File Offset: 0x0003CB6C
		// (set) Token: 0x06002AA4 RID: 10916 RVA: 0x0003E98D File Offset: 0x0003CB8D
		public Utf8String DeploymentId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_DeploymentId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_DeploymentId);
			}
		}

		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x06002AA5 RID: 10917 RVA: 0x0003E9A0 File Offset: 0x0003CBA0
		// (set) Token: 0x06002AA6 RID: 10918 RVA: 0x0003E9C1 File Offset: 0x0003CBC1
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

		// Token: 0x17000C7F RID: 3199
		// (get) Token: 0x06002AA7 RID: 10919 RVA: 0x0003E9D4 File Offset: 0x0003CBD4
		// (set) Token: 0x06002AA8 RID: 10920 RVA: 0x0003E9F5 File Offset: 0x0003CBF5
		public bool IsExternalAccountInfoPresent
		{
			get
			{
				bool result;
				Helper.Get(this.m_IsExternalAccountInfoPresent, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_IsExternalAccountInfoPresent);
			}
		}

		// Token: 0x17000C80 RID: 3200
		// (get) Token: 0x06002AA9 RID: 10921 RVA: 0x0003EA08 File Offset: 0x0003CC08
		// (set) Token: 0x06002AAA RID: 10922 RVA: 0x0003EA20 File Offset: 0x0003CC20
		public ExternalAccountType ExternalAccountIdType
		{
			get
			{
				return this.m_ExternalAccountIdType;
			}
			set
			{
				this.m_ExternalAccountIdType = value;
			}
		}

		// Token: 0x17000C81 RID: 3201
		// (get) Token: 0x06002AAB RID: 10923 RVA: 0x0003EA2C File Offset: 0x0003CC2C
		// (set) Token: 0x06002AAC RID: 10924 RVA: 0x0003EA4D File Offset: 0x0003CC4D
		public Utf8String ExternalAccountId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_ExternalAccountId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ExternalAccountId);
			}
		}

		// Token: 0x17000C82 RID: 3202
		// (get) Token: 0x06002AAD RID: 10925 RVA: 0x0003EA60 File Offset: 0x0003CC60
		// (set) Token: 0x06002AAE RID: 10926 RVA: 0x0003EA81 File Offset: 0x0003CC81
		public Utf8String ExternalAccountDisplayName
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_ExternalAccountDisplayName, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ExternalAccountDisplayName);
			}
		}

		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x06002AAF RID: 10927 RVA: 0x0003EA94 File Offset: 0x0003CC94
		// (set) Token: 0x06002AB0 RID: 10928 RVA: 0x0003EAB5 File Offset: 0x0003CCB5
		public Utf8String Platform
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Platform, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Platform);
			}
		}

		// Token: 0x06002AB1 RID: 10929 RVA: 0x0003EAC8 File Offset: 0x0003CCC8
		public void Set(ref VerifyIdTokenCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.ApplicationId = other.ApplicationId;
			this.ClientId = other.ClientId;
			this.ProductId = other.ProductId;
			this.SandboxId = other.SandboxId;
			this.DeploymentId = other.DeploymentId;
			this.DisplayName = other.DisplayName;
			this.IsExternalAccountInfoPresent = other.IsExternalAccountInfoPresent;
			this.ExternalAccountIdType = other.ExternalAccountIdType;
			this.ExternalAccountId = other.ExternalAccountId;
			this.ExternalAccountDisplayName = other.ExternalAccountDisplayName;
			this.Platform = other.Platform;
		}

		// Token: 0x06002AB2 RID: 10930 RVA: 0x0003EB80 File Offset: 0x0003CD80
		public void Set(ref VerifyIdTokenCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.ApplicationId = other.Value.ApplicationId;
				this.ClientId = other.Value.ClientId;
				this.ProductId = other.Value.ProductId;
				this.SandboxId = other.Value.SandboxId;
				this.DeploymentId = other.Value.DeploymentId;
				this.DisplayName = other.Value.DisplayName;
				this.IsExternalAccountInfoPresent = other.Value.IsExternalAccountInfoPresent;
				this.ExternalAccountIdType = other.Value.ExternalAccountIdType;
				this.ExternalAccountId = other.Value.ExternalAccountId;
				this.ExternalAccountDisplayName = other.Value.ExternalAccountDisplayName;
				this.Platform = other.Value.Platform;
			}
		}

		// Token: 0x06002AB3 RID: 10931 RVA: 0x0003ECB0 File Offset: 0x0003CEB0
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_ApplicationId);
			Helper.Dispose(ref this.m_ClientId);
			Helper.Dispose(ref this.m_ProductId);
			Helper.Dispose(ref this.m_SandboxId);
			Helper.Dispose(ref this.m_DeploymentId);
			Helper.Dispose(ref this.m_DisplayName);
			Helper.Dispose(ref this.m_ExternalAccountId);
			Helper.Dispose(ref this.m_ExternalAccountDisplayName);
			Helper.Dispose(ref this.m_Platform);
		}

		// Token: 0x06002AB4 RID: 10932 RVA: 0x0003ED36 File Offset: 0x0003CF36
		public void Get(out VerifyIdTokenCallbackInfo output)
		{
			output = default(VerifyIdTokenCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04001242 RID: 4674
		private Result m_ResultCode;

		// Token: 0x04001243 RID: 4675
		private IntPtr m_ClientData;

		// Token: 0x04001244 RID: 4676
		private IntPtr m_ApplicationId;

		// Token: 0x04001245 RID: 4677
		private IntPtr m_ClientId;

		// Token: 0x04001246 RID: 4678
		private IntPtr m_ProductId;

		// Token: 0x04001247 RID: 4679
		private IntPtr m_SandboxId;

		// Token: 0x04001248 RID: 4680
		private IntPtr m_DeploymentId;

		// Token: 0x04001249 RID: 4681
		private IntPtr m_DisplayName;

		// Token: 0x0400124A RID: 4682
		private int m_IsExternalAccountInfoPresent;

		// Token: 0x0400124B RID: 4683
		private ExternalAccountType m_ExternalAccountIdType;

		// Token: 0x0400124C RID: 4684
		private IntPtr m_ExternalAccountId;

		// Token: 0x0400124D RID: 4685
		private IntPtr m_ExternalAccountDisplayName;

		// Token: 0x0400124E RID: 4686
		private IntPtr m_Platform;
	}
}
