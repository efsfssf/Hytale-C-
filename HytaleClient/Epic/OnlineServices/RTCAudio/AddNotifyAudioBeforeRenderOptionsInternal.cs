using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x020001F7 RID: 503
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AddNotifyAudioBeforeRenderOptionsInternal : ISettable<AddNotifyAudioBeforeRenderOptions>, IDisposable
	{
		// Token: 0x170003AB RID: 939
		// (set) Token: 0x06000E9B RID: 3739 RVA: 0x00015686 File Offset: 0x00013886
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170003AC RID: 940
		// (set) Token: 0x06000E9C RID: 3740 RVA: 0x00015696 File Offset: 0x00013896
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x170003AD RID: 941
		// (set) Token: 0x06000E9D RID: 3741 RVA: 0x000156A6 File Offset: 0x000138A6
		public bool UnmixedAudio
		{
			set
			{
				Helper.Set(value, ref this.m_UnmixedAudio);
			}
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x000156B6 File Offset: 0x000138B6
		public void Set(ref AddNotifyAudioBeforeRenderOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.UnmixedAudio = other.UnmixedAudio;
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x000156E8 File Offset: 0x000138E8
		public void Set(ref AddNotifyAudioBeforeRenderOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.UnmixedAudio = other.Value.UnmixedAudio;
			}
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x00015748 File Offset: 0x00013948
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
		}

		// Token: 0x0400069A RID: 1690
		private int m_ApiVersion;

		// Token: 0x0400069B RID: 1691
		private IntPtr m_LocalUserId;

		// Token: 0x0400069C RID: 1692
		private IntPtr m_RoomName;

		// Token: 0x0400069D RID: 1693
		private int m_UnmixedAudio;
	}
}
