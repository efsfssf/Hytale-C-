using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x020001FD RID: 509
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyAudioInputStateOptionsInternal : ISettable<AddNotifyAudioInputStateOptions>, IDisposable
	{
		// Token: 0x170003B4 RID: 948
		// (set) Token: 0x06000EB1 RID: 3761 RVA: 0x00015882 File Offset: 0x00013A82
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170003B5 RID: 949
		// (set) Token: 0x06000EB2 RID: 3762 RVA: 0x00015892 File Offset: 0x00013A92
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x000158A2 File Offset: 0x00013AA2
		public void Set(ref AddNotifyAudioInputStateOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x000158C8 File Offset: 0x00013AC8
		public void Set(ref AddNotifyAudioInputStateOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
			}
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x00015913 File Offset: 0x00013B13
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
		}

		// Token: 0x040006A6 RID: 1702
		private int m_ApiVersion;

		// Token: 0x040006A7 RID: 1703
		private IntPtr m_LocalUserId;

		// Token: 0x040006A8 RID: 1704
		private IntPtr m_RoomName;
	}
}
