using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x020004A2 RID: 1186
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryAgeGateCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryAgeGateCallbackInfo>, ISettable<QueryAgeGateCallbackInfo>, IDisposable
	{
		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06001EF2 RID: 7922 RVA: 0x0002D3C0 File Offset: 0x0002B5C0
		// (set) Token: 0x06001EF3 RID: 7923 RVA: 0x0002D3D8 File Offset: 0x0002B5D8
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

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06001EF4 RID: 7924 RVA: 0x0002D3E4 File Offset: 0x0002B5E4
		// (set) Token: 0x06001EF5 RID: 7925 RVA: 0x0002D405 File Offset: 0x0002B605
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

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06001EF6 RID: 7926 RVA: 0x0002D418 File Offset: 0x0002B618
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06001EF7 RID: 7927 RVA: 0x0002D430 File Offset: 0x0002B630
		// (set) Token: 0x06001EF8 RID: 7928 RVA: 0x0002D451 File Offset: 0x0002B651
		public Utf8String CountryCode
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_CountryCode, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_CountryCode);
			}
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06001EF9 RID: 7929 RVA: 0x0002D464 File Offset: 0x0002B664
		// (set) Token: 0x06001EFA RID: 7930 RVA: 0x0002D47C File Offset: 0x0002B67C
		public uint AgeOfConsent
		{
			get
			{
				return this.m_AgeOfConsent;
			}
			set
			{
				this.m_AgeOfConsent = value;
			}
		}

		// Token: 0x06001EFB RID: 7931 RVA: 0x0002D486 File Offset: 0x0002B686
		public void Set(ref QueryAgeGateCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.CountryCode = other.CountryCode;
			this.AgeOfConsent = other.AgeOfConsent;
		}

		// Token: 0x06001EFC RID: 7932 RVA: 0x0002D4C0 File Offset: 0x0002B6C0
		public void Set(ref QueryAgeGateCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.CountryCode = other.Value.CountryCode;
				this.AgeOfConsent = other.Value.AgeOfConsent;
			}
		}

		// Token: 0x06001EFD RID: 7933 RVA: 0x0002D52E File Offset: 0x0002B72E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_CountryCode);
		}

		// Token: 0x06001EFE RID: 7934 RVA: 0x0002D549 File Offset: 0x0002B749
		public void Get(out QueryAgeGateCallbackInfo output)
		{
			output = default(QueryAgeGateCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000D73 RID: 3443
		private Result m_ResultCode;

		// Token: 0x04000D74 RID: 3444
		private IntPtr m_ClientData;

		// Token: 0x04000D75 RID: 3445
		private IntPtr m_CountryCode;

		// Token: 0x04000D76 RID: 3446
		private uint m_AgeOfConsent;
	}
}
