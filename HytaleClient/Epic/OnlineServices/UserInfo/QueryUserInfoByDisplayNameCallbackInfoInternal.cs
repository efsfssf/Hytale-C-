using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x0200003F RID: 63
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryUserInfoByDisplayNameCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryUserInfoByDisplayNameCallbackInfo>, ISettable<QueryUserInfoByDisplayNameCallbackInfo>, IDisposable
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x000057A0 File Offset: 0x000039A0
		// (set) Token: 0x060003F1 RID: 1009 RVA: 0x000057B8 File Offset: 0x000039B8
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

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x000057C4 File Offset: 0x000039C4
		// (set) Token: 0x060003F3 RID: 1011 RVA: 0x000057E5 File Offset: 0x000039E5
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

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x000057F8 File Offset: 0x000039F8
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x00005810 File Offset: 0x00003A10
		// (set) Token: 0x060003F6 RID: 1014 RVA: 0x00005831 File Offset: 0x00003A31
		public EpicAccountId LocalUserId
		{
			get
			{
				EpicAccountId result;
				Helper.Get<EpicAccountId>(this.m_LocalUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x00005844 File Offset: 0x00003A44
		// (set) Token: 0x060003F8 RID: 1016 RVA: 0x00005865 File Offset: 0x00003A65
		public EpicAccountId TargetUserId
		{
			get
			{
				EpicAccountId result;
				Helper.Get<EpicAccountId>(this.m_TargetUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x00005878 File Offset: 0x00003A78
		// (set) Token: 0x060003FA RID: 1018 RVA: 0x00005899 File Offset: 0x00003A99
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

		// Token: 0x060003FB RID: 1019 RVA: 0x000058AC File Offset: 0x00003AAC
		public void Set(ref QueryUserInfoByDisplayNameCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
			this.DisplayName = other.DisplayName;
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x000058FC File Offset: 0x00003AFC
		public void Set(ref QueryUserInfoByDisplayNameCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
				this.DisplayName = other.Value.DisplayName;
			}
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000597F File Offset: 0x00003B7F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
			Helper.Dispose(ref this.m_DisplayName);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x000059B2 File Offset: 0x00003BB2
		public void Get(out QueryUserInfoByDisplayNameCallbackInfo output)
		{
			output = default(QueryUserInfoByDisplayNameCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000191 RID: 401
		private Result m_ResultCode;

		// Token: 0x04000192 RID: 402
		private IntPtr m_ClientData;

		// Token: 0x04000193 RID: 403
		private IntPtr m_LocalUserId;

		// Token: 0x04000194 RID: 404
		private IntPtr m_TargetUserId;

		// Token: 0x04000195 RID: 405
		private IntPtr m_DisplayName;
	}
}
