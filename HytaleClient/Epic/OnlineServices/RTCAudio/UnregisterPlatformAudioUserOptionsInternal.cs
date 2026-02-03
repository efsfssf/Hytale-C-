using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200026D RID: 621
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UnregisterPlatformAudioUserOptionsInternal : ISettable<UnregisterPlatformAudioUserOptions>, IDisposable
	{
		// Token: 0x17000463 RID: 1123
		// (set) Token: 0x06001144 RID: 4420 RVA: 0x000191CF File Offset: 0x000173CF
		public Utf8String UserId
		{
			set
			{
				Helper.Set(value, ref this.m_UserId);
			}
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x000191DF File Offset: 0x000173DF
		public void Set(ref UnregisterPlatformAudioUserOptions other)
		{
			this.m_ApiVersion = 1;
			this.UserId = other.UserId;
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x000191F8 File Offset: 0x000173F8
		public void Set(ref UnregisterPlatformAudioUserOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.UserId = other.Value.UserId;
			}
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x0001922E File Offset: 0x0001742E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_UserId);
		}

		// Token: 0x04000796 RID: 1942
		private int m_ApiVersion;

		// Token: 0x04000797 RID: 1943
		private IntPtr m_UserId;
	}
}
