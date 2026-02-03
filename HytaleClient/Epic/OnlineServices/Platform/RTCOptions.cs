using System;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x0200071B RID: 1819
	public struct RTCOptions
	{
		// Token: 0x17000E1B RID: 3611
		// (get) Token: 0x06002F19 RID: 12057 RVA: 0x00045BC6 File Offset: 0x00043DC6
		// (set) Token: 0x06002F1A RID: 12058 RVA: 0x00045BCE File Offset: 0x00043DCE
		public IntPtr PlatformSpecificOptions { get; set; }

		// Token: 0x17000E1C RID: 3612
		// (get) Token: 0x06002F1B RID: 12059 RVA: 0x00045BD7 File Offset: 0x00043DD7
		// (set) Token: 0x06002F1C RID: 12060 RVA: 0x00045BDF File Offset: 0x00043DDF
		public RTCBackgroundMode BackgroundMode { get; set; }

		// Token: 0x06002F1D RID: 12061 RVA: 0x00045BE8 File Offset: 0x00043DE8
		internal void Set(ref RTCOptionsInternal other)
		{
			this.PlatformSpecificOptions = other.PlatformSpecificOptions;
			this.BackgroundMode = other.BackgroundMode;
		}
	}
}
