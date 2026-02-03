using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001D3 RID: 467
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RoomStatisticsUpdatedInfoInternal : ICallbackInfoInternal, IGettable<RoomStatisticsUpdatedInfo>, ISettable<RoomStatisticsUpdatedInfo>, IDisposable
	{
		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000D85 RID: 3461 RVA: 0x000139D0 File Offset: 0x00011BD0
		// (set) Token: 0x06000D86 RID: 3462 RVA: 0x000139F1 File Offset: 0x00011BF1
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

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000D87 RID: 3463 RVA: 0x00013A04 File Offset: 0x00011C04
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000D88 RID: 3464 RVA: 0x00013A1C File Offset: 0x00011C1C
		// (set) Token: 0x06000D89 RID: 3465 RVA: 0x00013A3D File Offset: 0x00011C3D
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

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000D8A RID: 3466 RVA: 0x00013A50 File Offset: 0x00011C50
		// (set) Token: 0x06000D8B RID: 3467 RVA: 0x00013A71 File Offset: 0x00011C71
		public Utf8String RoomName
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_RoomName, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000D8C RID: 3468 RVA: 0x00013A84 File Offset: 0x00011C84
		// (set) Token: 0x06000D8D RID: 3469 RVA: 0x00013AA5 File Offset: 0x00011CA5
		public Utf8String Statistic
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Statistic, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Statistic);
			}
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x00013AB5 File Offset: 0x00011CB5
		public void Set(ref RoomStatisticsUpdatedInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.Statistic = other.Statistic;
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x00013AEC File Offset: 0x00011CEC
		public void Set(ref RoomStatisticsUpdatedInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.Statistic = other.Value.Statistic;
			}
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x00013B5A File Offset: 0x00011D5A
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
			Helper.Dispose(ref this.m_Statistic);
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x00013B8D File Offset: 0x00011D8D
		public void Get(out RoomStatisticsUpdatedInfo output)
		{
			output = default(RoomStatisticsUpdatedInfo);
			output.Set(ref this);
		}

		// Token: 0x0400061D RID: 1565
		private IntPtr m_ClientData;

		// Token: 0x0400061E RID: 1566
		private IntPtr m_LocalUserId;

		// Token: 0x0400061F RID: 1567
		private IntPtr m_RoomName;

		// Token: 0x04000620 RID: 1568
		private IntPtr m_Statistic;
	}
}
