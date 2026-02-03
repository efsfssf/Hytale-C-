using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005F0 RID: 1520
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LoginCallbackInfoInternal : ICallbackInfoInternal, IGettable<LoginCallbackInfo>, ISettable<LoginCallbackInfo>, IDisposable
	{
		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x06002777 RID: 10103 RVA: 0x0003A65C File Offset: 0x0003885C
		// (set) Token: 0x06002778 RID: 10104 RVA: 0x0003A674 File Offset: 0x00038874
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

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x06002779 RID: 10105 RVA: 0x0003A680 File Offset: 0x00038880
		// (set) Token: 0x0600277A RID: 10106 RVA: 0x0003A6A1 File Offset: 0x000388A1
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

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x0600277B RID: 10107 RVA: 0x0003A6B4 File Offset: 0x000388B4
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x0600277C RID: 10108 RVA: 0x0003A6CC File Offset: 0x000388CC
		// (set) Token: 0x0600277D RID: 10109 RVA: 0x0003A6ED File Offset: 0x000388ED
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

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x0600277E RID: 10110 RVA: 0x0003A700 File Offset: 0x00038900
		// (set) Token: 0x0600277F RID: 10111 RVA: 0x0003A721 File Offset: 0x00038921
		public ContinuanceToken ContinuanceToken
		{
			get
			{
				ContinuanceToken result;
				Helper.Get<ContinuanceToken>(this.m_ContinuanceToken, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ContinuanceToken);
			}
		}

		// Token: 0x06002780 RID: 10112 RVA: 0x0003A731 File Offset: 0x00038931
		public void Set(ref LoginCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.ContinuanceToken = other.ContinuanceToken;
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x0003A768 File Offset: 0x00038968
		public void Set(ref LoginCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.ContinuanceToken = other.Value.ContinuanceToken;
			}
		}

		// Token: 0x06002782 RID: 10114 RVA: 0x0003A7D6 File Offset: 0x000389D6
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_ContinuanceToken);
		}

		// Token: 0x06002783 RID: 10115 RVA: 0x0003A7FD File Offset: 0x000389FD
		public void Get(out LoginCallbackInfo output)
		{
			output = default(LoginCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400111E RID: 4382
		private Result m_ResultCode;

		// Token: 0x0400111F RID: 4383
		private IntPtr m_ClientData;

		// Token: 0x04001120 RID: 4384
		private IntPtr m_LocalUserId;

		// Token: 0x04001121 RID: 4385
		private IntPtr m_ContinuanceToken;
	}
}
