using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001AE RID: 430
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyParticipantStatusChangedOptionsInternal : ISettable<AddNotifyParticipantStatusChangedOptions>, IDisposable
	{
		// Token: 0x170002E6 RID: 742
		// (set) Token: 0x06000C65 RID: 3173 RVA: 0x000121A0 File Offset: 0x000103A0
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170002E7 RID: 743
		// (set) Token: 0x06000C66 RID: 3174 RVA: 0x000121B0 File Offset: 0x000103B0
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x000121C0 File Offset: 0x000103C0
		public void Set(ref AddNotifyParticipantStatusChangedOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x000121E4 File Offset: 0x000103E4
		public void Set(ref AddNotifyParticipantStatusChangedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
			}
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x0001222F File Offset: 0x0001042F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
		}

		// Token: 0x040005AE RID: 1454
		private int m_ApiVersion;

		// Token: 0x040005AF RID: 1455
		private IntPtr m_LocalUserId;

		// Token: 0x040005B0 RID: 1456
		private IntPtr m_RoomName;
	}
}
