using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	// Token: 0x020004CC RID: 1228
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UserLoginStatusChangedCallbackInfoInternal : ICallbackInfoInternal, IGettable<UserLoginStatusChangedCallbackInfo>, ISettable<UserLoginStatusChangedCallbackInfo>, IDisposable
	{
		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x06001FE6 RID: 8166 RVA: 0x0002EA54 File Offset: 0x0002CC54
		// (set) Token: 0x06001FE7 RID: 8167 RVA: 0x0002EA75 File Offset: 0x0002CC75
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

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x06001FE8 RID: 8168 RVA: 0x0002EA88 File Offset: 0x0002CC88
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06001FE9 RID: 8169 RVA: 0x0002EAA0 File Offset: 0x0002CCA0
		// (set) Token: 0x06001FEA RID: 8170 RVA: 0x0002EAC1 File Offset: 0x0002CCC1
		public Utf8String PlatformType
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_PlatformType, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_PlatformType);
			}
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06001FEB RID: 8171 RVA: 0x0002EAD4 File Offset: 0x0002CCD4
		// (set) Token: 0x06001FEC RID: 8172 RVA: 0x0002EAF5 File Offset: 0x0002CCF5
		public Utf8String LocalPlatformUserId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_LocalPlatformUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LocalPlatformUserId);
			}
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x06001FED RID: 8173 RVA: 0x0002EB08 File Offset: 0x0002CD08
		// (set) Token: 0x06001FEE RID: 8174 RVA: 0x0002EB29 File Offset: 0x0002CD29
		public EpicAccountId AccountId
		{
			get
			{
				EpicAccountId result;
				Helper.Get<EpicAccountId>(this.m_AccountId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_AccountId);
			}
		}

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x06001FEF RID: 8175 RVA: 0x0002EB3C File Offset: 0x0002CD3C
		// (set) Token: 0x06001FF0 RID: 8176 RVA: 0x0002EB5D File Offset: 0x0002CD5D
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

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x06001FF1 RID: 8177 RVA: 0x0002EB70 File Offset: 0x0002CD70
		// (set) Token: 0x06001FF2 RID: 8178 RVA: 0x0002EB88 File Offset: 0x0002CD88
		public LoginStatus PreviousLoginStatus
		{
			get
			{
				return this.m_PreviousLoginStatus;
			}
			set
			{
				this.m_PreviousLoginStatus = value;
			}
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x06001FF3 RID: 8179 RVA: 0x0002EB94 File Offset: 0x0002CD94
		// (set) Token: 0x06001FF4 RID: 8180 RVA: 0x0002EBAC File Offset: 0x0002CDAC
		public LoginStatus CurrentLoginStatus
		{
			get
			{
				return this.m_CurrentLoginStatus;
			}
			set
			{
				this.m_CurrentLoginStatus = value;
			}
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x0002EBB8 File Offset: 0x0002CDB8
		public void Set(ref UserLoginStatusChangedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.PlatformType = other.PlatformType;
			this.LocalPlatformUserId = other.LocalPlatformUserId;
			this.AccountId = other.AccountId;
			this.ProductUserId = other.ProductUserId;
			this.PreviousLoginStatus = other.PreviousLoginStatus;
			this.CurrentLoginStatus = other.CurrentLoginStatus;
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x0002EC24 File Offset: 0x0002CE24
		public void Set(ref UserLoginStatusChangedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.PlatformType = other.Value.PlatformType;
				this.LocalPlatformUserId = other.Value.LocalPlatformUserId;
				this.AccountId = other.Value.AccountId;
				this.ProductUserId = other.Value.ProductUserId;
				this.PreviousLoginStatus = other.Value.PreviousLoginStatus;
				this.CurrentLoginStatus = other.Value.CurrentLoginStatus;
			}
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x0002ECD4 File Offset: 0x0002CED4
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_PlatformType);
			Helper.Dispose(ref this.m_LocalPlatformUserId);
			Helper.Dispose(ref this.m_AccountId);
			Helper.Dispose(ref this.m_ProductUserId);
		}

		// Token: 0x06001FF8 RID: 8184 RVA: 0x0002ED13 File Offset: 0x0002CF13
		public void Get(out UserLoginStatusChangedCallbackInfo output)
		{
			output = default(UserLoginStatusChangedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000DE4 RID: 3556
		private IntPtr m_ClientData;

		// Token: 0x04000DE5 RID: 3557
		private IntPtr m_PlatformType;

		// Token: 0x04000DE6 RID: 3558
		private IntPtr m_LocalPlatformUserId;

		// Token: 0x04000DE7 RID: 3559
		private IntPtr m_AccountId;

		// Token: 0x04000DE8 RID: 3560
		private IntPtr m_ProductUserId;

		// Token: 0x04000DE9 RID: 3561
		private LoginStatus m_PreviousLoginStatus;

		// Token: 0x04000DEA RID: 3562
		private LoginStatus m_CurrentLoginStatus;
	}
}
