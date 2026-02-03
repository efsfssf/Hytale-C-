using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000660 RID: 1632
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryIdTokenCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryIdTokenCallbackInfo>, ISettable<QueryIdTokenCallbackInfo>, IDisposable
	{
		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x06002A37 RID: 10807 RVA: 0x0003DEDC File Offset: 0x0003C0DC
		// (set) Token: 0x06002A38 RID: 10808 RVA: 0x0003DEF4 File Offset: 0x0003C0F4
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

		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x06002A39 RID: 10809 RVA: 0x0003DF00 File Offset: 0x0003C100
		// (set) Token: 0x06002A3A RID: 10810 RVA: 0x0003DF21 File Offset: 0x0003C121
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

		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x06002A3B RID: 10811 RVA: 0x0003DF34 File Offset: 0x0003C134
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x06002A3C RID: 10812 RVA: 0x0003DF4C File Offset: 0x0003C14C
		// (set) Token: 0x06002A3D RID: 10813 RVA: 0x0003DF6D File Offset: 0x0003C16D
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

		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x06002A3E RID: 10814 RVA: 0x0003DF80 File Offset: 0x0003C180
		// (set) Token: 0x06002A3F RID: 10815 RVA: 0x0003DFA1 File Offset: 0x0003C1A1
		public EpicAccountId TargetAccountId
		{
			get
			{
				EpicAccountId result;
				Helper.Get<EpicAccountId>(this.m_TargetAccountId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_TargetAccountId);
			}
		}

		// Token: 0x06002A40 RID: 10816 RVA: 0x0003DFB1 File Offset: 0x0003C1B1
		public void Set(ref QueryIdTokenCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetAccountId = other.TargetAccountId;
		}

		// Token: 0x06002A41 RID: 10817 RVA: 0x0003DFE8 File Offset: 0x0003C1E8
		public void Set(ref QueryIdTokenCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetAccountId = other.Value.TargetAccountId;
			}
		}

		// Token: 0x06002A42 RID: 10818 RVA: 0x0003E056 File Offset: 0x0003C256
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetAccountId);
		}

		// Token: 0x06002A43 RID: 10819 RVA: 0x0003E07D File Offset: 0x0003C27D
		public void Get(out QueryIdTokenCallbackInfo output)
		{
			output = default(QueryIdTokenCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04001217 RID: 4631
		private Result m_ResultCode;

		// Token: 0x04001218 RID: 4632
		private IntPtr m_ClientData;

		// Token: 0x04001219 RID: 4633
		private IntPtr m_LocalUserId;

		// Token: 0x0400121A RID: 4634
		private IntPtr m_TargetAccountId;
	}
}
