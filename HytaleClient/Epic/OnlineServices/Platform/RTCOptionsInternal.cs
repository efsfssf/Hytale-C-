using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x0200071C RID: 1820
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RTCOptionsInternal : IGettable<RTCOptions>, ISettable<RTCOptions>, IDisposable
	{
		// Token: 0x17000E1D RID: 3613
		// (get) Token: 0x06002F1E RID: 12062 RVA: 0x00045C08 File Offset: 0x00043E08
		// (set) Token: 0x06002F1F RID: 12063 RVA: 0x00045C20 File Offset: 0x00043E20
		public IntPtr PlatformSpecificOptions
		{
			get
			{
				return this.m_PlatformSpecificOptions;
			}
			set
			{
				this.m_PlatformSpecificOptions = value;
			}
		}

		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x06002F20 RID: 12064 RVA: 0x00045C2C File Offset: 0x00043E2C
		// (set) Token: 0x06002F21 RID: 12065 RVA: 0x00045C44 File Offset: 0x00043E44
		public RTCBackgroundMode BackgroundMode
		{
			get
			{
				return this.m_BackgroundMode;
			}
			set
			{
				this.m_BackgroundMode = value;
			}
		}

		// Token: 0x06002F22 RID: 12066 RVA: 0x00045C4E File Offset: 0x00043E4E
		public void Set(ref RTCOptions other)
		{
			this.m_ApiVersion = 2;
			this.PlatformSpecificOptions = other.PlatformSpecificOptions;
			this.BackgroundMode = other.BackgroundMode;
		}

		// Token: 0x06002F23 RID: 12067 RVA: 0x00045C74 File Offset: 0x00043E74
		public void Set(ref RTCOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.PlatformSpecificOptions = other.Value.PlatformSpecificOptions;
				this.BackgroundMode = other.Value.BackgroundMode;
			}
		}

		// Token: 0x06002F24 RID: 12068 RVA: 0x00045CBF File Offset: 0x00043EBF
		public void Dispose()
		{
			Helper.Dispose(ref this.m_PlatformSpecificOptions);
		}

		// Token: 0x06002F25 RID: 12069 RVA: 0x00045CCE File Offset: 0x00043ECE
		public void Get(out RTCOptions output)
		{
			output = default(RTCOptions);
			output.Set(ref this);
		}

		// Token: 0x040014FA RID: 5370
		private int m_ApiVersion;

		// Token: 0x040014FB RID: 5371
		private IntPtr m_PlatformSpecificOptions;

		// Token: 0x040014FC RID: 5372
		private RTCBackgroundMode m_BackgroundMode;
	}
}
