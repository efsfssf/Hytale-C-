using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000755 RID: 1877
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnUnlockAchievementsCompleteCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnUnlockAchievementsCompleteCallbackInfo>, ISettable<OnUnlockAchievementsCompleteCallbackInfo>, IDisposable
	{
		// Token: 0x17000EB1 RID: 3761
		// (get) Token: 0x060030BE RID: 12478 RVA: 0x0004856C File Offset: 0x0004676C
		// (set) Token: 0x060030BF RID: 12479 RVA: 0x00048584 File Offset: 0x00046784
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

		// Token: 0x17000EB2 RID: 3762
		// (get) Token: 0x060030C0 RID: 12480 RVA: 0x00048590 File Offset: 0x00046790
		// (set) Token: 0x060030C1 RID: 12481 RVA: 0x000485B1 File Offset: 0x000467B1
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

		// Token: 0x17000EB3 RID: 3763
		// (get) Token: 0x060030C2 RID: 12482 RVA: 0x000485C4 File Offset: 0x000467C4
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000EB4 RID: 3764
		// (get) Token: 0x060030C3 RID: 12483 RVA: 0x000485DC File Offset: 0x000467DC
		// (set) Token: 0x060030C4 RID: 12484 RVA: 0x000485FD File Offset: 0x000467FD
		public ProductUserId UserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_UserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_UserId);
			}
		}

		// Token: 0x17000EB5 RID: 3765
		// (get) Token: 0x060030C5 RID: 12485 RVA: 0x00048610 File Offset: 0x00046810
		// (set) Token: 0x060030C6 RID: 12486 RVA: 0x00048628 File Offset: 0x00046828
		public uint AchievementsCount
		{
			get
			{
				return this.m_AchievementsCount;
			}
			set
			{
				this.m_AchievementsCount = value;
			}
		}

		// Token: 0x060030C7 RID: 12487 RVA: 0x00048632 File Offset: 0x00046832
		public void Set(ref OnUnlockAchievementsCompleteCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.UserId = other.UserId;
			this.AchievementsCount = other.AchievementsCount;
		}

		// Token: 0x060030C8 RID: 12488 RVA: 0x0004866C File Offset: 0x0004686C
		public void Set(ref OnUnlockAchievementsCompleteCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.UserId = other.Value.UserId;
				this.AchievementsCount = other.Value.AchievementsCount;
			}
		}

		// Token: 0x060030C9 RID: 12489 RVA: 0x000486DA File Offset: 0x000468DA
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_UserId);
		}

		// Token: 0x060030CA RID: 12490 RVA: 0x000486F5 File Offset: 0x000468F5
		public void Get(out OnUnlockAchievementsCompleteCallbackInfo output)
		{
			output = default(OnUnlockAchievementsCompleteCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040015BA RID: 5562
		private Result m_ResultCode;

		// Token: 0x040015BB RID: 5563
		private IntPtr m_ClientData;

		// Token: 0x040015BC RID: 5564
		private IntPtr m_UserId;

		// Token: 0x040015BD RID: 5565
		private uint m_AchievementsCount;
	}
}
