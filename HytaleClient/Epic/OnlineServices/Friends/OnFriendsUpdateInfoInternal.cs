using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004EC RID: 1260
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnFriendsUpdateInfoInternal : ICallbackInfoInternal, IGettable<OnFriendsUpdateInfo>, ISettable<OnFriendsUpdateInfo>, IDisposable
	{
		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x060020B1 RID: 8369 RVA: 0x0002FEB8 File Offset: 0x0002E0B8
		// (set) Token: 0x060020B2 RID: 8370 RVA: 0x0002FED9 File Offset: 0x0002E0D9
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

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x060020B3 RID: 8371 RVA: 0x0002FEEC File Offset: 0x0002E0EC
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x060020B4 RID: 8372 RVA: 0x0002FF04 File Offset: 0x0002E104
		// (set) Token: 0x060020B5 RID: 8373 RVA: 0x0002FF25 File Offset: 0x0002E125
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

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x060020B6 RID: 8374 RVA: 0x0002FF38 File Offset: 0x0002E138
		// (set) Token: 0x060020B7 RID: 8375 RVA: 0x0002FF59 File Offset: 0x0002E159
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

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x060020B8 RID: 8376 RVA: 0x0002FF6C File Offset: 0x0002E16C
		// (set) Token: 0x060020B9 RID: 8377 RVA: 0x0002FF84 File Offset: 0x0002E184
		public FriendsStatus PreviousStatus
		{
			get
			{
				return this.m_PreviousStatus;
			}
			set
			{
				this.m_PreviousStatus = value;
			}
		}

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x060020BA RID: 8378 RVA: 0x0002FF90 File Offset: 0x0002E190
		// (set) Token: 0x060020BB RID: 8379 RVA: 0x0002FFA8 File Offset: 0x0002E1A8
		public FriendsStatus CurrentStatus
		{
			get
			{
				return this.m_CurrentStatus;
			}
			set
			{
				this.m_CurrentStatus = value;
			}
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x0002FFB4 File Offset: 0x0002E1B4
		public void Set(ref OnFriendsUpdateInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
			this.PreviousStatus = other.PreviousStatus;
			this.CurrentStatus = other.CurrentStatus;
		}

		// Token: 0x060020BD RID: 8381 RVA: 0x00030004 File Offset: 0x0002E204
		public void Set(ref OnFriendsUpdateInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
				this.PreviousStatus = other.Value.PreviousStatus;
				this.CurrentStatus = other.Value.CurrentStatus;
			}
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x00030087 File Offset: 0x0002E287
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x000300AE File Offset: 0x0002E2AE
		public void Get(out OnFriendsUpdateInfo output)
		{
			output = default(OnFriendsUpdateInfo);
			output.Set(ref this);
		}

		// Token: 0x04000E36 RID: 3638
		private IntPtr m_ClientData;

		// Token: 0x04000E37 RID: 3639
		private IntPtr m_LocalUserId;

		// Token: 0x04000E38 RID: 3640
		private IntPtr m_TargetUserId;

		// Token: 0x04000E39 RID: 3641
		private FriendsStatus m_PreviousStatus;

		// Token: 0x04000E3A RID: 3642
		private FriendsStatus m_CurrentStatus;
	}
}
