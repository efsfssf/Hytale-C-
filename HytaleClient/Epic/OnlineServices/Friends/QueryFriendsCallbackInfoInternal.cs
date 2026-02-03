using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004F4 RID: 1268
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryFriendsCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryFriendsCallbackInfo>, ISettable<QueryFriendsCallbackInfo>, IDisposable
	{
		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x060020E0 RID: 8416 RVA: 0x0003013C File Offset: 0x0002E33C
		// (set) Token: 0x060020E1 RID: 8417 RVA: 0x00030154 File Offset: 0x0002E354
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

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x060020E2 RID: 8418 RVA: 0x00030160 File Offset: 0x0002E360
		// (set) Token: 0x060020E3 RID: 8419 RVA: 0x00030181 File Offset: 0x0002E381
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

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x060020E4 RID: 8420 RVA: 0x00030194 File Offset: 0x0002E394
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x060020E5 RID: 8421 RVA: 0x000301AC File Offset: 0x0002E3AC
		// (set) Token: 0x060020E6 RID: 8422 RVA: 0x000301CD File Offset: 0x0002E3CD
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

		// Token: 0x060020E7 RID: 8423 RVA: 0x000301DD File Offset: 0x0002E3DD
		public void Set(ref QueryFriendsCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x00030208 File Offset: 0x0002E408
		public void Set(ref QueryFriendsCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x00030261 File Offset: 0x0002E461
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x0003027C File Offset: 0x0002E47C
		public void Get(out QueryFriendsCallbackInfo output)
		{
			output = default(QueryFriendsCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000E3E RID: 3646
		private Result m_ResultCode;

		// Token: 0x04000E3F RID: 3647
		private IntPtr m_ClientData;

		// Token: 0x04000E40 RID: 3648
		private IntPtr m_LocalUserId;
	}
}
