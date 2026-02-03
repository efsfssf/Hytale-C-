using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x020004A6 RID: 1190
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryPermissionsCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryPermissionsCallbackInfo>, ISettable<QueryPermissionsCallbackInfo>, IDisposable
	{
		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06001F12 RID: 7954 RVA: 0x0002D690 File Offset: 0x0002B890
		// (set) Token: 0x06001F13 RID: 7955 RVA: 0x0002D6A8 File Offset: 0x0002B8A8
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

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06001F14 RID: 7956 RVA: 0x0002D6B4 File Offset: 0x0002B8B4
		// (set) Token: 0x06001F15 RID: 7957 RVA: 0x0002D6D5 File Offset: 0x0002B8D5
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

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06001F16 RID: 7958 RVA: 0x0002D6E8 File Offset: 0x0002B8E8
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06001F17 RID: 7959 RVA: 0x0002D700 File Offset: 0x0002B900
		// (set) Token: 0x06001F18 RID: 7960 RVA: 0x0002D721 File Offset: 0x0002B921
		public ProductUserId LocalUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_LocalUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06001F19 RID: 7961 RVA: 0x0002D734 File Offset: 0x0002B934
		// (set) Token: 0x06001F1A RID: 7962 RVA: 0x0002D755 File Offset: 0x0002B955
		public Utf8String KWSUserId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_KWSUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_KWSUserId);
			}
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x06001F1B RID: 7963 RVA: 0x0002D768 File Offset: 0x0002B968
		// (set) Token: 0x06001F1C RID: 7964 RVA: 0x0002D789 File Offset: 0x0002B989
		public Utf8String DateOfBirth
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_DateOfBirth, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_DateOfBirth);
			}
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x06001F1D RID: 7965 RVA: 0x0002D79C File Offset: 0x0002B99C
		// (set) Token: 0x06001F1E RID: 7966 RVA: 0x0002D7BD File Offset: 0x0002B9BD
		public bool IsMinor
		{
			get
			{
				bool result;
				Helper.Get(this.m_IsMinor, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_IsMinor);
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x06001F1F RID: 7967 RVA: 0x0002D7D0 File Offset: 0x0002B9D0
		// (set) Token: 0x06001F20 RID: 7968 RVA: 0x0002D7F1 File Offset: 0x0002B9F1
		public Utf8String ParentEmail
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_ParentEmail, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ParentEmail);
			}
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x0002D804 File Offset: 0x0002BA04
		public void Set(ref QueryPermissionsCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.KWSUserId = other.KWSUserId;
			this.DateOfBirth = other.DateOfBirth;
			this.IsMinor = other.IsMinor;
			this.ParentEmail = other.ParentEmail;
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x0002D870 File Offset: 0x0002BA70
		public void Set(ref QueryPermissionsCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.KWSUserId = other.Value.KWSUserId;
				this.DateOfBirth = other.Value.DateOfBirth;
				this.IsMinor = other.Value.IsMinor;
				this.ParentEmail = other.Value.ParentEmail;
			}
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x0002D920 File Offset: 0x0002BB20
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_KWSUserId);
			Helper.Dispose(ref this.m_DateOfBirth);
			Helper.Dispose(ref this.m_ParentEmail);
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x0002D95F File Offset: 0x0002BB5F
		public void Get(out QueryPermissionsCallbackInfo output)
		{
			output = default(QueryPermissionsCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000D7F RID: 3455
		private Result m_ResultCode;

		// Token: 0x04000D80 RID: 3456
		private IntPtr m_ClientData;

		// Token: 0x04000D81 RID: 3457
		private IntPtr m_LocalUserId;

		// Token: 0x04000D82 RID: 3458
		private IntPtr m_KWSUserId;

		// Token: 0x04000D83 RID: 3459
		private IntPtr m_DateOfBirth;

		// Token: 0x04000D84 RID: 3460
		private int m_IsMinor;

		// Token: 0x04000D85 RID: 3461
		private IntPtr m_ParentEmail;
	}
}
