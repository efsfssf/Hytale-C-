using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000277 RID: 631
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UpdateReceivingOptionsInternal : ISettable<UpdateReceivingOptions>, IDisposable
	{
		// Token: 0x1700048C RID: 1164
		// (set) Token: 0x060011A3 RID: 4515 RVA: 0x00019B08 File Offset: 0x00017D08
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x1700048D RID: 1165
		// (set) Token: 0x060011A4 RID: 4516 RVA: 0x00019B18 File Offset: 0x00017D18
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x1700048E RID: 1166
		// (set) Token: 0x060011A5 RID: 4517 RVA: 0x00019B28 File Offset: 0x00017D28
		public ProductUserId ParticipantId
		{
			set
			{
				Helper.Set(value, ref this.m_ParticipantId);
			}
		}

		// Token: 0x1700048F RID: 1167
		// (set) Token: 0x060011A6 RID: 4518 RVA: 0x00019B38 File Offset: 0x00017D38
		public bool AudioEnabled
		{
			set
			{
				Helper.Set(value, ref this.m_AudioEnabled);
			}
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x00019B48 File Offset: 0x00017D48
		public void Set(ref UpdateReceivingOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.ParticipantId = other.ParticipantId;
			this.AudioEnabled = other.AudioEnabled;
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x00019B88 File Offset: 0x00017D88
		public void Set(ref UpdateReceivingOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.ParticipantId = other.Value.ParticipantId;
				this.AudioEnabled = other.Value.AudioEnabled;
			}
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x00019BFD File Offset: 0x00017DFD
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
			Helper.Dispose(ref this.m_ParticipantId);
		}

		// Token: 0x040007C0 RID: 1984
		private int m_ApiVersion;

		// Token: 0x040007C1 RID: 1985
		private IntPtr m_LocalUserId;

		// Token: 0x040007C2 RID: 1986
		private IntPtr m_RoomName;

		// Token: 0x040007C3 RID: 1987
		private IntPtr m_ParticipantId;

		// Token: 0x040007C4 RID: 1988
		private int m_AudioEnabled;
	}
}
