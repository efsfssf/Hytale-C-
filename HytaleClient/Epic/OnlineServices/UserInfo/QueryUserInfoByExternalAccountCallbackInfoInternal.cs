using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x02000043 RID: 67
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryUserInfoByExternalAccountCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryUserInfoByExternalAccountCallbackInfo>, ISettable<QueryUserInfoByExternalAccountCallbackInfo>, IDisposable
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x00005B74 File Offset: 0x00003D74
		// (set) Token: 0x06000417 RID: 1047 RVA: 0x00005B8C File Offset: 0x00003D8C
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

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000418 RID: 1048 RVA: 0x00005B98 File Offset: 0x00003D98
		// (set) Token: 0x06000419 RID: 1049 RVA: 0x00005BB9 File Offset: 0x00003DB9
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

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x00005BCC File Offset: 0x00003DCC
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x00005BE4 File Offset: 0x00003DE4
		// (set) Token: 0x0600041C RID: 1052 RVA: 0x00005C05 File Offset: 0x00003E05
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

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x00005C18 File Offset: 0x00003E18
		// (set) Token: 0x0600041E RID: 1054 RVA: 0x00005C39 File Offset: 0x00003E39
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

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x00005C4C File Offset: 0x00003E4C
		// (set) Token: 0x06000420 RID: 1056 RVA: 0x00005C64 File Offset: 0x00003E64
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

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x00005C70 File Offset: 0x00003E70
		// (set) Token: 0x06000422 RID: 1058 RVA: 0x00005C91 File Offset: 0x00003E91
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

		// Token: 0x06000423 RID: 1059 RVA: 0x00005CA4 File Offset: 0x00003EA4
		public void Set(ref QueryUserInfoByExternalAccountCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.ExternalAccountId = other.ExternalAccountId;
			this.AccountType = other.AccountType;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00005D00 File Offset: 0x00003F00
		public void Set(ref QueryUserInfoByExternalAccountCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.ExternalAccountId = other.Value.ExternalAccountId;
				this.AccountType = other.Value.AccountType;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00005D9B File Offset: 0x00003F9B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_ExternalAccountId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00005DCE File Offset: 0x00003FCE
		public void Get(out QueryUserInfoByExternalAccountCallbackInfo output)
		{
			output = default(QueryUserInfoByExternalAccountCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040001A1 RID: 417
		private Result m_ResultCode;

		// Token: 0x040001A2 RID: 418
		private IntPtr m_ClientData;

		// Token: 0x040001A3 RID: 419
		private IntPtr m_LocalUserId;

		// Token: 0x040001A4 RID: 420
		private IntPtr m_ExternalAccountId;

		// Token: 0x040001A5 RID: 421
		private ExternalAccountType m_AccountType;

		// Token: 0x040001A6 RID: 422
		private IntPtr m_TargetUserId;
	}
}
