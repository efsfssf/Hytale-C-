using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCData
{
	// Token: 0x020001F1 RID: 497
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UpdateReceivingOptionsInternal : ISettable<UpdateReceivingOptions>, IDisposable
	{
		// Token: 0x17000393 RID: 915
		// (set) Token: 0x06000E67 RID: 3687 RVA: 0x00015148 File Offset: 0x00013348
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000394 RID: 916
		// (set) Token: 0x06000E68 RID: 3688 RVA: 0x00015158 File Offset: 0x00013358
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x17000395 RID: 917
		// (set) Token: 0x06000E69 RID: 3689 RVA: 0x00015168 File Offset: 0x00013368
		public ProductUserId ParticipantId
		{
			set
			{
				Helper.Set(value, ref this.m_ParticipantId);
			}
		}

		// Token: 0x17000396 RID: 918
		// (set) Token: 0x06000E6A RID: 3690 RVA: 0x00015178 File Offset: 0x00013378
		public bool DataEnabled
		{
			set
			{
				Helper.Set(value, ref this.m_DataEnabled);
			}
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x00015188 File Offset: 0x00013388
		public void Set(ref UpdateReceivingOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.ParticipantId = other.ParticipantId;
			this.DataEnabled = other.DataEnabled;
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x000151C8 File Offset: 0x000133C8
		public void Set(ref UpdateReceivingOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.ParticipantId = other.Value.ParticipantId;
				this.DataEnabled = other.Value.DataEnabled;
			}
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x0001523D File Offset: 0x0001343D
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
			Helper.Dispose(ref this.m_ParticipantId);
		}

		// Token: 0x04000681 RID: 1665
		private int m_ApiVersion;

		// Token: 0x04000682 RID: 1666
		private IntPtr m_LocalUserId;

		// Token: 0x04000683 RID: 1667
		private IntPtr m_RoomName;

		// Token: 0x04000684 RID: 1668
		private IntPtr m_ParticipantId;

		// Token: 0x04000685 RID: 1669
		private int m_DataEnabled;
	}
}
