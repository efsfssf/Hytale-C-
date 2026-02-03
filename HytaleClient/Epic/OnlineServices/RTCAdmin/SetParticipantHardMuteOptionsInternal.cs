using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin
{
	// Token: 0x0200029A RID: 666
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SetParticipantHardMuteOptionsInternal : ISettable<SetParticipantHardMuteOptions>, IDisposable
	{
		// Token: 0x170004F1 RID: 1265
		// (set) Token: 0x060012AE RID: 4782 RVA: 0x0001B3C4 File Offset: 0x000195C4
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (set) Token: 0x060012AF RID: 4783 RVA: 0x0001B3D4 File Offset: 0x000195D4
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (set) Token: 0x060012B0 RID: 4784 RVA: 0x0001B3E4 File Offset: 0x000195E4
		public bool Mute
		{
			set
			{
				Helper.Set(value, ref this.m_Mute);
			}
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x0001B3F4 File Offset: 0x000195F4
		public void Set(ref SetParticipantHardMuteOptions other)
		{
			this.m_ApiVersion = 1;
			this.RoomName = other.RoomName;
			this.TargetUserId = other.TargetUserId;
			this.Mute = other.Mute;
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x0001B428 File Offset: 0x00019628
		public void Set(ref SetParticipantHardMuteOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.RoomName = other.Value.RoomName;
				this.TargetUserId = other.Value.TargetUserId;
				this.Mute = other.Value.Mute;
			}
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x0001B488 File Offset: 0x00019688
		public void Dispose()
		{
			Helper.Dispose(ref this.m_RoomName);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x0400082E RID: 2094
		private int m_ApiVersion;

		// Token: 0x0400082F RID: 2095
		private IntPtr m_RoomName;

		// Token: 0x04000830 RID: 2096
		private IntPtr m_TargetUserId;

		// Token: 0x04000831 RID: 2097
		private int m_Mute;
	}
}
