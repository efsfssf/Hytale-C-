using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000566 RID: 1382
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryOwnershipTokenCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryOwnershipTokenCallbackInfo>, ISettable<QueryOwnershipTokenCallbackInfo>, IDisposable
	{
		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x06002412 RID: 9234 RVA: 0x00035120 File Offset: 0x00033320
		// (set) Token: 0x06002413 RID: 9235 RVA: 0x00035138 File Offset: 0x00033338
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

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x06002414 RID: 9236 RVA: 0x00035144 File Offset: 0x00033344
		// (set) Token: 0x06002415 RID: 9237 RVA: 0x00035165 File Offset: 0x00033365
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

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x06002416 RID: 9238 RVA: 0x00035178 File Offset: 0x00033378
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x06002417 RID: 9239 RVA: 0x00035190 File Offset: 0x00033390
		// (set) Token: 0x06002418 RID: 9240 RVA: 0x000351B1 File Offset: 0x000333B1
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

		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x06002419 RID: 9241 RVA: 0x000351C4 File Offset: 0x000333C4
		// (set) Token: 0x0600241A RID: 9242 RVA: 0x000351E5 File Offset: 0x000333E5
		public Utf8String OwnershipToken
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_OwnershipToken, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_OwnershipToken);
			}
		}

		// Token: 0x0600241B RID: 9243 RVA: 0x000351F5 File Offset: 0x000333F5
		public void Set(ref QueryOwnershipTokenCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.OwnershipToken = other.OwnershipToken;
		}

		// Token: 0x0600241C RID: 9244 RVA: 0x0003522C File Offset: 0x0003342C
		public void Set(ref QueryOwnershipTokenCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.OwnershipToken = other.Value.OwnershipToken;
			}
		}

		// Token: 0x0600241D RID: 9245 RVA: 0x0003529A File Offset: 0x0003349A
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_OwnershipToken);
		}

		// Token: 0x0600241E RID: 9246 RVA: 0x000352C1 File Offset: 0x000334C1
		public void Get(out QueryOwnershipTokenCallbackInfo output)
		{
			output = default(QueryOwnershipTokenCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000FD2 RID: 4050
		private Result m_ResultCode;

		// Token: 0x04000FD3 RID: 4051
		private IntPtr m_ClientData;

		// Token: 0x04000FD4 RID: 4052
		private IntPtr m_LocalUserId;

		// Token: 0x04000FD5 RID: 4053
		private IntPtr m_OwnershipToken;
	}
}
