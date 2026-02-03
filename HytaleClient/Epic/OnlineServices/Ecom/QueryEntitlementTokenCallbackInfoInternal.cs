using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000556 RID: 1366
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryEntitlementTokenCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryEntitlementTokenCallbackInfo>, ISettable<QueryEntitlementTokenCallbackInfo>, IDisposable
	{
		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x06002393 RID: 9107 RVA: 0x000344B4 File Offset: 0x000326B4
		// (set) Token: 0x06002394 RID: 9108 RVA: 0x000344CC File Offset: 0x000326CC
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

		// Token: 0x17000A4F RID: 2639
		// (get) Token: 0x06002395 RID: 9109 RVA: 0x000344D8 File Offset: 0x000326D8
		// (set) Token: 0x06002396 RID: 9110 RVA: 0x000344F9 File Offset: 0x000326F9
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

		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x06002397 RID: 9111 RVA: 0x0003450C File Offset: 0x0003270C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x06002398 RID: 9112 RVA: 0x00034524 File Offset: 0x00032724
		// (set) Token: 0x06002399 RID: 9113 RVA: 0x00034545 File Offset: 0x00032745
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

		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x0600239A RID: 9114 RVA: 0x00034558 File Offset: 0x00032758
		// (set) Token: 0x0600239B RID: 9115 RVA: 0x00034579 File Offset: 0x00032779
		public Utf8String EntitlementToken
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_EntitlementToken, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_EntitlementToken);
			}
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x00034589 File Offset: 0x00032789
		public void Set(ref QueryEntitlementTokenCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.EntitlementToken = other.EntitlementToken;
		}

		// Token: 0x0600239D RID: 9117 RVA: 0x000345C0 File Offset: 0x000327C0
		public void Set(ref QueryEntitlementTokenCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.EntitlementToken = other.Value.EntitlementToken;
			}
		}

		// Token: 0x0600239E RID: 9118 RVA: 0x0003462E File Offset: 0x0003282E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_EntitlementToken);
		}

		// Token: 0x0600239F RID: 9119 RVA: 0x00034655 File Offset: 0x00032855
		public void Get(out QueryEntitlementTokenCallbackInfo output)
		{
			output = default(QueryEntitlementTokenCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000F99 RID: 3993
		private Result m_ResultCode;

		// Token: 0x04000F9A RID: 3994
		private IntPtr m_ClientData;

		// Token: 0x04000F9B RID: 3995
		private IntPtr m_LocalUserId;

		// Token: 0x04000F9C RID: 3996
		private IntPtr m_EntitlementToken;
	}
}
