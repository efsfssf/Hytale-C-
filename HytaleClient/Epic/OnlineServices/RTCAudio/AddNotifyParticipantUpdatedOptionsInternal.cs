using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000201 RID: 513
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyParticipantUpdatedOptionsInternal : ISettable<AddNotifyParticipantUpdatedOptions>, IDisposable
	{
		// Token: 0x170003BC RID: 956
		// (set) Token: 0x06000EC3 RID: 3779 RVA: 0x00015A1C File Offset: 0x00013C1C
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170003BD RID: 957
		// (set) Token: 0x06000EC4 RID: 3780 RVA: 0x00015A2C File Offset: 0x00013C2C
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x00015A3C File Offset: 0x00013C3C
		public void Set(ref AddNotifyParticipantUpdatedOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x00015A60 File Offset: 0x00013C60
		public void Set(ref AddNotifyParticipantUpdatedOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
			}
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x00015AAB File Offset: 0x00013CAB
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
		}

		// Token: 0x040006B0 RID: 1712
		private int m_ApiVersion;

		// Token: 0x040006B1 RID: 1713
		private IntPtr m_LocalUserId;

		// Token: 0x040006B2 RID: 1714
		private IntPtr m_RoomName;
	}
}
