using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000273 RID: 627
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UpdateParticipantVolumeOptionsInternal : ISettable<UpdateParticipantVolumeOptions>, IDisposable
	{
		// Token: 0x17000477 RID: 1143
		// (set) Token: 0x06001175 RID: 4469 RVA: 0x00019650 File Offset: 0x00017850
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000478 RID: 1144
		// (set) Token: 0x06001176 RID: 4470 RVA: 0x00019660 File Offset: 0x00017860
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x17000479 RID: 1145
		// (set) Token: 0x06001177 RID: 4471 RVA: 0x00019670 File Offset: 0x00017870
		public ProductUserId ParticipantId
		{
			set
			{
				Helper.Set(value, ref this.m_ParticipantId);
			}
		}

		// Token: 0x1700047A RID: 1146
		// (set) Token: 0x06001178 RID: 4472 RVA: 0x00019680 File Offset: 0x00017880
		public float Volume
		{
			set
			{
				this.m_Volume = value;
			}
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x0001968A File Offset: 0x0001788A
		public void Set(ref UpdateParticipantVolumeOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.ParticipantId = other.ParticipantId;
			this.Volume = other.Volume;
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x000196C8 File Offset: 0x000178C8
		public void Set(ref UpdateParticipantVolumeOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.ParticipantId = other.Value.ParticipantId;
				this.Volume = other.Value.Volume;
			}
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x0001973D File Offset: 0x0001793D
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
			Helper.Dispose(ref this.m_ParticipantId);
		}

		// Token: 0x040007AB RID: 1963
		private int m_ApiVersion;

		// Token: 0x040007AC RID: 1964
		private IntPtr m_LocalUserId;

		// Token: 0x040007AD RID: 1965
		private IntPtr m_RoomName;

		// Token: 0x040007AE RID: 1966
		private IntPtr m_ParticipantId;

		// Token: 0x040007AF RID: 1967
		private float m_Volume;
	}
}
