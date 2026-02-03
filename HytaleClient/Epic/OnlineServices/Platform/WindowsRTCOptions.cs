using System;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x0200071F RID: 1823
	public struct WindowsRTCOptions
	{
		// Token: 0x17000E3F RID: 3647
		// (get) Token: 0x06002F59 RID: 12121 RVA: 0x000461E6 File Offset: 0x000443E6
		// (set) Token: 0x06002F5A RID: 12122 RVA: 0x000461EE File Offset: 0x000443EE
		public WindowsRTCOptionsPlatformSpecificOptions? PlatformSpecificOptions { get; set; }

		// Token: 0x17000E40 RID: 3648
		// (get) Token: 0x06002F5B RID: 12123 RVA: 0x000461F7 File Offset: 0x000443F7
		// (set) Token: 0x06002F5C RID: 12124 RVA: 0x000461FF File Offset: 0x000443FF
		public RTCBackgroundMode BackgroundMode { get; set; }

		// Token: 0x06002F5D RID: 12125 RVA: 0x00046208 File Offset: 0x00044408
		internal void Set(ref WindowsRTCOptionsInternal other)
		{
			this.PlatformSpecificOptions = other.PlatformSpecificOptions;
			this.BackgroundMode = other.BackgroundMode;
		}
	}
}
