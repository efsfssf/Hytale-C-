using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x02000720 RID: 1824
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct WindowsRTCOptionsInternal : IGettable<WindowsRTCOptions>, ISettable<WindowsRTCOptions>, IDisposable
	{
		// Token: 0x17000E41 RID: 3649
		// (get) Token: 0x06002F5E RID: 12126 RVA: 0x00046228 File Offset: 0x00044428
		// (set) Token: 0x06002F5F RID: 12127 RVA: 0x00046249 File Offset: 0x00044449
		public WindowsRTCOptionsPlatformSpecificOptions? PlatformSpecificOptions
		{
			get
			{
				WindowsRTCOptionsPlatformSpecificOptions? result;
				Helper.Get<WindowsRTCOptionsPlatformSpecificOptionsInternal, WindowsRTCOptionsPlatformSpecificOptions>(this.m_PlatformSpecificOptions, out result);
				return result;
			}
			set
			{
				Helper.Set<WindowsRTCOptionsPlatformSpecificOptions, WindowsRTCOptionsPlatformSpecificOptionsInternal>(ref value, ref this.m_PlatformSpecificOptions);
			}
		}

		// Token: 0x17000E42 RID: 3650
		// (get) Token: 0x06002F60 RID: 12128 RVA: 0x0004625C File Offset: 0x0004445C
		// (set) Token: 0x06002F61 RID: 12129 RVA: 0x00046274 File Offset: 0x00044474
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

		// Token: 0x06002F62 RID: 12130 RVA: 0x0004627E File Offset: 0x0004447E
		public void Set(ref WindowsRTCOptions other)
		{
			this.m_ApiVersion = 2;
			this.PlatformSpecificOptions = other.PlatformSpecificOptions;
			this.BackgroundMode = other.BackgroundMode;
		}

		// Token: 0x06002F63 RID: 12131 RVA: 0x000462A4 File Offset: 0x000444A4
		public void Set(ref WindowsRTCOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.PlatformSpecificOptions = other.Value.PlatformSpecificOptions;
				this.BackgroundMode = other.Value.BackgroundMode;
			}
		}

		// Token: 0x06002F64 RID: 12132 RVA: 0x000462EF File Offset: 0x000444EF
		public void Dispose()
		{
			Helper.Dispose(ref this.m_PlatformSpecificOptions);
		}

		// Token: 0x06002F65 RID: 12133 RVA: 0x000462FE File Offset: 0x000444FE
		public void Get(out WindowsRTCOptions output)
		{
			output = default(WindowsRTCOptions);
			output.Set(ref this);
		}

		// Token: 0x04001520 RID: 5408
		private int m_ApiVersion;

		// Token: 0x04001521 RID: 5409
		private IntPtr m_PlatformSpecificOptions;

		// Token: 0x04001522 RID: 5410
		private RTCBackgroundMode m_BackgroundMode;
	}
}
