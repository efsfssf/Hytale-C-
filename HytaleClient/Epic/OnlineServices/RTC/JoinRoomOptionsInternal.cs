using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001BB RID: 443
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct JoinRoomOptionsInternal : ISettable<JoinRoomOptions>, IDisposable
	{
		// Token: 0x1700031D RID: 797
		// (set) Token: 0x06000CE3 RID: 3299 RVA: 0x00012D9C File Offset: 0x00010F9C
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x1700031E RID: 798
		// (set) Token: 0x06000CE4 RID: 3300 RVA: 0x00012DAC File Offset: 0x00010FAC
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x1700031F RID: 799
		// (set) Token: 0x06000CE5 RID: 3301 RVA: 0x00012DBC File Offset: 0x00010FBC
		public Utf8String ClientBaseUrl
		{
			set
			{
				Helper.Set(value, ref this.m_ClientBaseUrl);
			}
		}

		// Token: 0x17000320 RID: 800
		// (set) Token: 0x06000CE6 RID: 3302 RVA: 0x00012DCC File Offset: 0x00010FCC
		public Utf8String ParticipantToken
		{
			set
			{
				Helper.Set(value, ref this.m_ParticipantToken);
			}
		}

		// Token: 0x17000321 RID: 801
		// (set) Token: 0x06000CE7 RID: 3303 RVA: 0x00012DDC File Offset: 0x00010FDC
		public ProductUserId ParticipantId
		{
			set
			{
				Helper.Set(value, ref this.m_ParticipantId);
			}
		}

		// Token: 0x17000322 RID: 802
		// (set) Token: 0x06000CE8 RID: 3304 RVA: 0x00012DEC File Offset: 0x00010FEC
		public JoinRoomFlags Flags
		{
			set
			{
				this.m_Flags = value;
			}
		}

		// Token: 0x17000323 RID: 803
		// (set) Token: 0x06000CE9 RID: 3305 RVA: 0x00012DF6 File Offset: 0x00010FF6
		public bool ManualAudioInputEnabled
		{
			set
			{
				Helper.Set(value, ref this.m_ManualAudioInputEnabled);
			}
		}

		// Token: 0x17000324 RID: 804
		// (set) Token: 0x06000CEA RID: 3306 RVA: 0x00012E06 File Offset: 0x00011006
		public bool ManualAudioOutputEnabled
		{
			set
			{
				Helper.Set(value, ref this.m_ManualAudioOutputEnabled);
			}
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x00012E18 File Offset: 0x00011018
		public void Set(ref JoinRoomOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.ClientBaseUrl = other.ClientBaseUrl;
			this.ParticipantToken = other.ParticipantToken;
			this.ParticipantId = other.ParticipantId;
			this.Flags = other.Flags;
			this.ManualAudioInputEnabled = other.ManualAudioInputEnabled;
			this.ManualAudioOutputEnabled = other.ManualAudioOutputEnabled;
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x00012E98 File Offset: 0x00011098
		public void Set(ref JoinRoomOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.ClientBaseUrl = other.Value.ClientBaseUrl;
				this.ParticipantToken = other.Value.ParticipantToken;
				this.ParticipantId = other.Value.ParticipantId;
				this.Flags = other.Value.Flags;
				this.ManualAudioInputEnabled = other.Value.ManualAudioInputEnabled;
				this.ManualAudioOutputEnabled = other.Value.ManualAudioOutputEnabled;
			}
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x00012F64 File Offset: 0x00011164
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
			Helper.Dispose(ref this.m_ClientBaseUrl);
			Helper.Dispose(ref this.m_ParticipantToken);
			Helper.Dispose(ref this.m_ParticipantId);
		}

		// Token: 0x040005EA RID: 1514
		private int m_ApiVersion;

		// Token: 0x040005EB RID: 1515
		private IntPtr m_LocalUserId;

		// Token: 0x040005EC RID: 1516
		private IntPtr m_RoomName;

		// Token: 0x040005ED RID: 1517
		private IntPtr m_ClientBaseUrl;

		// Token: 0x040005EE RID: 1518
		private IntPtr m_ParticipantToken;

		// Token: 0x040005EF RID: 1519
		private IntPtr m_ParticipantId;

		// Token: 0x040005F0 RID: 1520
		private JoinRoomFlags m_Flags;

		// Token: 0x040005F1 RID: 1521
		private int m_ManualAudioInputEnabled;

		// Token: 0x040005F2 RID: 1522
		private int m_ManualAudioOutputEnabled;
	}
}
