using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x02000722 RID: 1826
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct WindowsRTCOptionsPlatformSpecificOptionsInternal : IGettable<WindowsRTCOptionsPlatformSpecificOptions>, ISettable<WindowsRTCOptionsPlatformSpecificOptions>, IDisposable
	{
		// Token: 0x17000E44 RID: 3652
		// (get) Token: 0x06002F69 RID: 12137 RVA: 0x00046334 File Offset: 0x00044534
		// (set) Token: 0x06002F6A RID: 12138 RVA: 0x00046355 File Offset: 0x00044555
		public Utf8String XAudio29DllPath
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_XAudio29DllPath, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_XAudio29DllPath);
			}
		}

		// Token: 0x06002F6B RID: 12139 RVA: 0x00046365 File Offset: 0x00044565
		public void Set(ref WindowsRTCOptionsPlatformSpecificOptions other)
		{
			this.m_ApiVersion = 1;
			this.XAudio29DllPath = other.XAudio29DllPath;
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x0004637C File Offset: 0x0004457C
		public void Set(ref WindowsRTCOptionsPlatformSpecificOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.XAudio29DllPath = other.Value.XAudio29DllPath;
			}
		}

		// Token: 0x06002F6D RID: 12141 RVA: 0x000463B2 File Offset: 0x000445B2
		public void Dispose()
		{
			Helper.Dispose(ref this.m_XAudio29DllPath);
		}

		// Token: 0x06002F6E RID: 12142 RVA: 0x000463C1 File Offset: 0x000445C1
		public void Get(out WindowsRTCOptionsPlatformSpecificOptions output)
		{
			output = default(WindowsRTCOptionsPlatformSpecificOptions);
			output.Set(ref this);
		}

		// Token: 0x04001524 RID: 5412
		private int m_ApiVersion;

		// Token: 0x04001525 RID: 5413
		private IntPtr m_XAudio29DllPath;
	}
}
