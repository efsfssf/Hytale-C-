using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x02000047 RID: 71
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryUserInfoCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryUserInfoCallbackInfo>, ISettable<QueryUserInfoCallbackInfo>, IDisposable
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x00005F84 File Offset: 0x00004184
		// (set) Token: 0x0600043E RID: 1086 RVA: 0x00005F9C File Offset: 0x0000419C
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

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x00005FA8 File Offset: 0x000041A8
		// (set) Token: 0x06000440 RID: 1088 RVA: 0x00005FC9 File Offset: 0x000041C9
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

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x00005FDC File Offset: 0x000041DC
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x00005FF4 File Offset: 0x000041F4
		// (set) Token: 0x06000443 RID: 1091 RVA: 0x00006015 File Offset: 0x00004215
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

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x00006028 File Offset: 0x00004228
		// (set) Token: 0x06000445 RID: 1093 RVA: 0x00006049 File Offset: 0x00004249
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

		// Token: 0x06000446 RID: 1094 RVA: 0x00006059 File Offset: 0x00004259
		public void Set(ref QueryUserInfoCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00006090 File Offset: 0x00004290
		public void Set(ref QueryUserInfoCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x000060FE File Offset: 0x000042FE
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00006125 File Offset: 0x00004325
		public void Get(out QueryUserInfoCallbackInfo output)
		{
			output = default(QueryUserInfoCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040001B2 RID: 434
		private Result m_ResultCode;

		// Token: 0x040001B3 RID: 435
		private IntPtr m_ClientData;

		// Token: 0x040001B4 RID: 436
		private IntPtr m_LocalUserId;

		// Token: 0x040001B5 RID: 437
		private IntPtr m_TargetUserId;
	}
}
