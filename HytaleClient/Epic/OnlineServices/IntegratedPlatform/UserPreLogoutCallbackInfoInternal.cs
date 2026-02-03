using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	// Token: 0x020004CE RID: 1230
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UserPreLogoutCallbackInfoInternal : ICallbackInfoInternal, IGettable<UserPreLogoutCallbackInfo>, ISettable<UserPreLogoutCallbackInfo>, IDisposable
	{
		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06002005 RID: 8197 RVA: 0x0002EDE8 File Offset: 0x0002CFE8
		// (set) Token: 0x06002006 RID: 8198 RVA: 0x0002EE09 File Offset: 0x0002D009
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

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x06002007 RID: 8199 RVA: 0x0002EE1C File Offset: 0x0002D01C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x06002008 RID: 8200 RVA: 0x0002EE34 File Offset: 0x0002D034
		// (set) Token: 0x06002009 RID: 8201 RVA: 0x0002EE55 File Offset: 0x0002D055
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

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x0600200A RID: 8202 RVA: 0x0002EE68 File Offset: 0x0002D068
		// (set) Token: 0x0600200B RID: 8203 RVA: 0x0002EE89 File Offset: 0x0002D089
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

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x0600200C RID: 8204 RVA: 0x0002EE9C File Offset: 0x0002D09C
		// (set) Token: 0x0600200D RID: 8205 RVA: 0x0002EEBD File Offset: 0x0002D0BD
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

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x0600200E RID: 8206 RVA: 0x0002EED0 File Offset: 0x0002D0D0
		// (set) Token: 0x0600200F RID: 8207 RVA: 0x0002EEF1 File Offset: 0x0002D0F1
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

		// Token: 0x06002010 RID: 8208 RVA: 0x0002EF04 File Offset: 0x0002D104
		public void Set(ref UserPreLogoutCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.PlatformType = other.PlatformType;
			this.LocalPlatformUserId = other.LocalPlatformUserId;
			this.AccountId = other.AccountId;
			this.ProductUserId = other.ProductUserId;
		}

		// Token: 0x06002011 RID: 8209 RVA: 0x0002EF54 File Offset: 0x0002D154
		public void Set(ref UserPreLogoutCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.PlatformType = other.Value.PlatformType;
				this.LocalPlatformUserId = other.Value.LocalPlatformUserId;
				this.AccountId = other.Value.AccountId;
				this.ProductUserId = other.Value.ProductUserId;
			}
		}

		// Token: 0x06002012 RID: 8210 RVA: 0x0002EFD7 File Offset: 0x0002D1D7
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_PlatformType);
			Helper.Dispose(ref this.m_LocalPlatformUserId);
			Helper.Dispose(ref this.m_AccountId);
			Helper.Dispose(ref this.m_ProductUserId);
		}

		// Token: 0x06002013 RID: 8211 RVA: 0x0002F016 File Offset: 0x0002D216
		public void Get(out UserPreLogoutCallbackInfo output)
		{
			output = default(UserPreLogoutCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000DF0 RID: 3568
		private IntPtr m_ClientData;

		// Token: 0x04000DF1 RID: 3569
		private IntPtr m_PlatformType;

		// Token: 0x04000DF2 RID: 3570
		private IntPtr m_LocalPlatformUserId;

		// Token: 0x04000DF3 RID: 3571
		private IntPtr m_AccountId;

		// Token: 0x04000DF4 RID: 3572
		private IntPtr m_ProductUserId;
	}
}
