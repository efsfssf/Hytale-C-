using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004E8 RID: 1256
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnBlockedUsersUpdateInfoInternal : ICallbackInfoInternal, IGettable<OnBlockedUsersUpdateInfo>, ISettable<OnBlockedUsersUpdateInfo>, IDisposable
	{
		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x06002090 RID: 8336 RVA: 0x0002FC34 File Offset: 0x0002DE34
		// (set) Token: 0x06002091 RID: 8337 RVA: 0x0002FC55 File Offset: 0x0002DE55
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

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06002092 RID: 8338 RVA: 0x0002FC68 File Offset: 0x0002DE68
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x06002093 RID: 8339 RVA: 0x0002FC80 File Offset: 0x0002DE80
		// (set) Token: 0x06002094 RID: 8340 RVA: 0x0002FCA1 File Offset: 0x0002DEA1
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

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x06002095 RID: 8341 RVA: 0x0002FCB4 File Offset: 0x0002DEB4
		// (set) Token: 0x06002096 RID: 8342 RVA: 0x0002FCD5 File Offset: 0x0002DED5
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

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x06002097 RID: 8343 RVA: 0x0002FCE8 File Offset: 0x0002DEE8
		// (set) Token: 0x06002098 RID: 8344 RVA: 0x0002FD09 File Offset: 0x0002DF09
		public bool Blocked
		{
			get
			{
				bool result;
				Helper.Get(this.m_Blocked, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Blocked);
			}
		}

		// Token: 0x06002099 RID: 8345 RVA: 0x0002FD19 File Offset: 0x0002DF19
		public void Set(ref OnBlockedUsersUpdateInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
			this.Blocked = other.Blocked;
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x0002FD50 File Offset: 0x0002DF50
		public void Set(ref OnBlockedUsersUpdateInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
				this.Blocked = other.Value.Blocked;
			}
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x0002FDBE File Offset: 0x0002DFBE
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x0002FDE5 File Offset: 0x0002DFE5
		public void Get(out OnBlockedUsersUpdateInfo output)
		{
			output = default(OnBlockedUsersUpdateInfo);
			output.Set(ref this);
		}

		// Token: 0x04000E2D RID: 3629
		private IntPtr m_ClientData;

		// Token: 0x04000E2E RID: 3630
		private IntPtr m_LocalUserId;

		// Token: 0x04000E2F RID: 3631
		private IntPtr m_TargetUserId;

		// Token: 0x04000E30 RID: 3632
		private int m_Blocked;
	}
}
