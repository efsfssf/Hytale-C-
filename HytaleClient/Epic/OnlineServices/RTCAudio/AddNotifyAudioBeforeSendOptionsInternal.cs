using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x020001F9 RID: 505
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyAudioBeforeSendOptionsInternal : ISettable<AddNotifyAudioBeforeSendOptions>, IDisposable
	{
		// Token: 0x170003B0 RID: 944
		// (set) Token: 0x06000EA5 RID: 3749 RVA: 0x00015785 File Offset: 0x00013985
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170003B1 RID: 945
		// (set) Token: 0x06000EA6 RID: 3750 RVA: 0x00015795 File Offset: 0x00013995
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x000157A5 File Offset: 0x000139A5
		public void Set(ref AddNotifyAudioBeforeSendOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x000157CC File Offset: 0x000139CC
		public void Set(ref AddNotifyAudioBeforeSendOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
			}
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x00015817 File Offset: 0x00013A17
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
		}

		// Token: 0x040006A0 RID: 1696
		private int m_ApiVersion;

		// Token: 0x040006A1 RID: 1697
		private IntPtr m_LocalUserId;

		// Token: 0x040006A2 RID: 1698
		private IntPtr m_RoomName;
	}
}
