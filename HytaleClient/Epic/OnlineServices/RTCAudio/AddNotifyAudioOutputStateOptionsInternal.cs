using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x020001FF RID: 511
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyAudioOutputStateOptionsInternal : ISettable<AddNotifyAudioOutputStateOptions>, IDisposable
	{
		// Token: 0x170003B8 RID: 952
		// (set) Token: 0x06000EBA RID: 3770 RVA: 0x00015950 File Offset: 0x00013B50
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170003B9 RID: 953
		// (set) Token: 0x06000EBB RID: 3771 RVA: 0x00015960 File Offset: 0x00013B60
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x00015970 File Offset: 0x00013B70
		public void Set(ref AddNotifyAudioOutputStateOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x00015994 File Offset: 0x00013B94
		public void Set(ref AddNotifyAudioOutputStateOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
			}
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x000159DF File Offset: 0x00013BDF
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
		}

		// Token: 0x040006AB RID: 1707
		private int m_ApiVersion;

		// Token: 0x040006AC RID: 1708
		private IntPtr m_LocalUserId;

		// Token: 0x040006AD RID: 1709
		private IntPtr m_RoomName;
	}
}
