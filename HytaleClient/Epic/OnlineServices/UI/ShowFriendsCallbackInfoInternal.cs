using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x0200008B RID: 139
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ShowFriendsCallbackInfoInternal : ICallbackInfoInternal, IGettable<ShowFriendsCallbackInfo>, ISettable<ShowFriendsCallbackInfo>, IDisposable
	{
		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x00007EE4 File Offset: 0x000060E4
		// (set) Token: 0x060005B1 RID: 1457 RVA: 0x00007EFC File Offset: 0x000060FC
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

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060005B2 RID: 1458 RVA: 0x00007F08 File Offset: 0x00006108
		// (set) Token: 0x060005B3 RID: 1459 RVA: 0x00007F29 File Offset: 0x00006129
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

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060005B4 RID: 1460 RVA: 0x00007F3C File Offset: 0x0000613C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060005B5 RID: 1461 RVA: 0x00007F54 File Offset: 0x00006154
		// (set) Token: 0x060005B6 RID: 1462 RVA: 0x00007F75 File Offset: 0x00006175
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

		// Token: 0x060005B7 RID: 1463 RVA: 0x00007F85 File Offset: 0x00006185
		public void Set(ref ShowFriendsCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00007FB0 File Offset: 0x000061B0
		public void Set(ref ShowFriendsCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00008009 File Offset: 0x00006209
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00008024 File Offset: 0x00006224
		public void Get(out ShowFriendsCallbackInfo output)
		{
			output = default(ShowFriendsCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040002CA RID: 714
		private Result m_ResultCode;

		// Token: 0x040002CB RID: 715
		private IntPtr m_ClientData;

		// Token: 0x040002CC RID: 716
		private IntPtr m_LocalUserId;
	}
}
