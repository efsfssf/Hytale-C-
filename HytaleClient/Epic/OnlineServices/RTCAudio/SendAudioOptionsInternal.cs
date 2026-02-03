using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000263 RID: 611
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SendAudioOptionsInternal : ISettable<SendAudioOptions>, IDisposable
	{
		// Token: 0x17000447 RID: 1095
		// (set) Token: 0x0600110C RID: 4364 RVA: 0x00018C9E File Offset: 0x00016E9E
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000448 RID: 1096
		// (set) Token: 0x0600110D RID: 4365 RVA: 0x00018CAE File Offset: 0x00016EAE
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x17000449 RID: 1097
		// (set) Token: 0x0600110E RID: 4366 RVA: 0x00018CBE File Offset: 0x00016EBE
		public AudioBuffer? Buffer
		{
			set
			{
				Helper.Set<AudioBuffer, AudioBufferInternal>(ref value, ref this.m_Buffer);
			}
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x00018CCF File Offset: 0x00016ECF
		public void Set(ref SendAudioOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.Buffer = other.Buffer;
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x00018D00 File Offset: 0x00016F00
		public void Set(ref SendAudioOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.Buffer = other.Value.Buffer;
			}
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x00018D60 File Offset: 0x00016F60
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
			Helper.Dispose(ref this.m_Buffer);
		}

		// Token: 0x04000775 RID: 1909
		private int m_ApiVersion;

		// Token: 0x04000776 RID: 1910
		private IntPtr m_LocalUserId;

		// Token: 0x04000777 RID: 1911
		private IntPtr m_RoomName;

		// Token: 0x04000778 RID: 1912
		private IntPtr m_Buffer;
	}
}
