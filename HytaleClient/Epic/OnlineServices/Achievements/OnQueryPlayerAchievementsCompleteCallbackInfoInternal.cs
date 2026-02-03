using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000751 RID: 1873
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnQueryPlayerAchievementsCompleteCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnQueryPlayerAchievementsCompleteCallbackInfo>, ISettable<OnQueryPlayerAchievementsCompleteCallbackInfo>, IDisposable
	{
		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x0600309F RID: 12447 RVA: 0x00048320 File Offset: 0x00046520
		// (set) Token: 0x060030A0 RID: 12448 RVA: 0x00048338 File Offset: 0x00046538
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

		// Token: 0x17000EA9 RID: 3753
		// (get) Token: 0x060030A1 RID: 12449 RVA: 0x00048344 File Offset: 0x00046544
		// (set) Token: 0x060030A2 RID: 12450 RVA: 0x00048365 File Offset: 0x00046565
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

		// Token: 0x17000EAA RID: 3754
		// (get) Token: 0x060030A3 RID: 12451 RVA: 0x00048378 File Offset: 0x00046578
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000EAB RID: 3755
		// (get) Token: 0x060030A4 RID: 12452 RVA: 0x00048390 File Offset: 0x00046590
		// (set) Token: 0x060030A5 RID: 12453 RVA: 0x000483B1 File Offset: 0x000465B1
		public ProductUserId TargetUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_TargetUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x060030A6 RID: 12454 RVA: 0x000483C4 File Offset: 0x000465C4
		// (set) Token: 0x060030A7 RID: 12455 RVA: 0x000483E5 File Offset: 0x000465E5
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

		// Token: 0x060030A8 RID: 12456 RVA: 0x000483F5 File Offset: 0x000465F5
		public void Set(ref OnQueryPlayerAchievementsCompleteCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.TargetUserId = other.TargetUserId;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x060030A9 RID: 12457 RVA: 0x0004842C File Offset: 0x0004662C
		public void Set(ref OnQueryPlayerAchievementsCompleteCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.TargetUserId = other.Value.TargetUserId;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x060030AA RID: 12458 RVA: 0x0004849A File Offset: 0x0004669A
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_TargetUserId);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x060030AB RID: 12459 RVA: 0x000484C1 File Offset: 0x000466C1
		public void Get(out OnQueryPlayerAchievementsCompleteCallbackInfo output)
		{
			output = default(OnQueryPlayerAchievementsCompleteCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040015B2 RID: 5554
		private Result m_ResultCode;

		// Token: 0x040015B3 RID: 5555
		private IntPtr m_ClientData;

		// Token: 0x040015B4 RID: 5556
		private IntPtr m_TargetUserId;

		// Token: 0x040015B5 RID: 5557
		private IntPtr m_LocalUserId;
	}
}
