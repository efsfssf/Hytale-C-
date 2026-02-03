using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin
{
	// Token: 0x02000295 RID: 661
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryJoinRoomTokenOptionsInternal : ISettable<QueryJoinRoomTokenOptions>, IDisposable
	{
		// Token: 0x170004E5 RID: 1253
		// (set) Token: 0x06001288 RID: 4744 RVA: 0x0001AEAC File Offset: 0x000190AC
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (set) Token: 0x06001289 RID: 4745 RVA: 0x0001AEBC File Offset: 0x000190BC
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (set) Token: 0x0600128A RID: 4746 RVA: 0x0001AECC File Offset: 0x000190CC
		public ProductUserId[] TargetUserIds
		{
			set
			{
				Helper.Set<ProductUserId>(value, ref this.m_TargetUserIds, out this.m_TargetUserIdsCount);
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (set) Token: 0x0600128B RID: 4747 RVA: 0x0001AEE2 File Offset: 0x000190E2
		public Utf8String TargetUserIpAddresses
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserIpAddresses);
			}
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x0001AEF2 File Offset: 0x000190F2
		public void Set(ref QueryJoinRoomTokenOptions other)
		{
			this.m_ApiVersion = 2;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.TargetUserIds = other.TargetUserIds;
			this.TargetUserIpAddresses = other.TargetUserIpAddresses;
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x0001AF30 File Offset: 0x00019130
		public void Set(ref QueryJoinRoomTokenOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.TargetUserIds = other.Value.TargetUserIds;
				this.TargetUserIpAddresses = other.Value.TargetUserIpAddresses;
			}
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x0001AFA5 File Offset: 0x000191A5
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
			Helper.Dispose(ref this.m_TargetUserIds);
			Helper.Dispose(ref this.m_TargetUserIpAddresses);
		}

		// Token: 0x0400081B RID: 2075
		private int m_ApiVersion;

		// Token: 0x0400081C RID: 2076
		private IntPtr m_LocalUserId;

		// Token: 0x0400081D RID: 2077
		private IntPtr m_RoomName;

		// Token: 0x0400081E RID: 2078
		private IntPtr m_TargetUserIds;

		// Token: 0x0400081F RID: 2079
		private uint m_TargetUserIdsCount;

		// Token: 0x04000820 RID: 2080
		private IntPtr m_TargetUserIpAddresses;
	}
}
