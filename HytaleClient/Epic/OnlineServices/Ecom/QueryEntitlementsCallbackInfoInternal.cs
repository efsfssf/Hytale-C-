using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000552 RID: 1362
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryEntitlementsCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryEntitlementsCallbackInfo>, ISettable<QueryEntitlementsCallbackInfo>, IDisposable
	{
		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x0600236F RID: 9071 RVA: 0x00034164 File Offset: 0x00032364
		// (set) Token: 0x06002370 RID: 9072 RVA: 0x0003417C File Offset: 0x0003237C
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

		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x06002371 RID: 9073 RVA: 0x00034188 File Offset: 0x00032388
		// (set) Token: 0x06002372 RID: 9074 RVA: 0x000341A9 File Offset: 0x000323A9
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

		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x06002373 RID: 9075 RVA: 0x000341BC File Offset: 0x000323BC
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x06002374 RID: 9076 RVA: 0x000341D4 File Offset: 0x000323D4
		// (set) Token: 0x06002375 RID: 9077 RVA: 0x000341F5 File Offset: 0x000323F5
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

		// Token: 0x06002376 RID: 9078 RVA: 0x00034205 File Offset: 0x00032405
		public void Set(ref QueryEntitlementsCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06002377 RID: 9079 RVA: 0x00034230 File Offset: 0x00032430
		public void Set(ref QueryEntitlementsCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06002378 RID: 9080 RVA: 0x00034289 File Offset: 0x00032489
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x06002379 RID: 9081 RVA: 0x000342A4 File Offset: 0x000324A4
		public void Get(out QueryEntitlementsCallbackInfo output)
		{
			output = default(QueryEntitlementsCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000F88 RID: 3976
		private Result m_ResultCode;

		// Token: 0x04000F89 RID: 3977
		private IntPtr m_ClientData;

		// Token: 0x04000F8A RID: 3978
		private IntPtr m_LocalUserId;
	}
}
