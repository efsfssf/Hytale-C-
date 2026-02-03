using System;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x02000721 RID: 1825
	public struct WindowsRTCOptionsPlatformSpecificOptions
	{
		// Token: 0x17000E43 RID: 3651
		// (get) Token: 0x06002F66 RID: 12134 RVA: 0x00046310 File Offset: 0x00044510
		// (set) Token: 0x06002F67 RID: 12135 RVA: 0x00046318 File Offset: 0x00044518
		public Utf8String XAudio29DllPath { get; set; }

		// Token: 0x06002F68 RID: 12136 RVA: 0x00046321 File Offset: 0x00044521
		internal void Set(ref WindowsRTCOptionsPlatformSpecificOptionsInternal other)
		{
			this.XAudio29DllPath = other.XAudio29DllPath;
		}
	}
}
