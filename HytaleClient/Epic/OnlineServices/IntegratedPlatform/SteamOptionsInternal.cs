using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	// Token: 0x020004CA RID: 1226
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SteamOptionsInternal : IGettable<SteamOptions>, ISettable<SteamOptions>, IDisposable
	{
		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x06001FC8 RID: 8136 RVA: 0x0002E770 File Offset: 0x0002C970
		// (set) Token: 0x06001FC9 RID: 8137 RVA: 0x0002E791 File Offset: 0x0002C991
		public Utf8String OverrideLibraryPath
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_OverrideLibraryPath, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_OverrideLibraryPath);
			}
		}

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x06001FCA RID: 8138 RVA: 0x0002E7A4 File Offset: 0x0002C9A4
		// (set) Token: 0x06001FCB RID: 8139 RVA: 0x0002E7BC File Offset: 0x0002C9BC
		public uint SteamMajorVersion
		{
			get
			{
				return this.m_SteamMajorVersion;
			}
			set
			{
				this.m_SteamMajorVersion = value;
			}
		}

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x06001FCC RID: 8140 RVA: 0x0002E7C8 File Offset: 0x0002C9C8
		// (set) Token: 0x06001FCD RID: 8141 RVA: 0x0002E7E0 File Offset: 0x0002C9E0
		public uint SteamMinorVersion
		{
			get
			{
				return this.m_SteamMinorVersion;
			}
			set
			{
				this.m_SteamMinorVersion = value;
			}
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06001FCE RID: 8142 RVA: 0x0002E7EC File Offset: 0x0002C9EC
		// (set) Token: 0x06001FCF RID: 8143 RVA: 0x0002E80D File Offset: 0x0002CA0D
		public Utf8String SteamApiInterfaceVersionsArray
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_SteamApiInterfaceVersionsArray, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_SteamApiInterfaceVersionsArray);
			}
		}

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06001FD0 RID: 8144 RVA: 0x0002E820 File Offset: 0x0002CA20
		// (set) Token: 0x06001FD1 RID: 8145 RVA: 0x0002E838 File Offset: 0x0002CA38
		public uint SteamApiInterfaceVersionsArrayBytes
		{
			get
			{
				return this.m_SteamApiInterfaceVersionsArrayBytes;
			}
			set
			{
				this.m_SteamApiInterfaceVersionsArrayBytes = value;
			}
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x0002E844 File Offset: 0x0002CA44
		public void Set(ref SteamOptions other)
		{
			this.m_ApiVersion = 3;
			this.OverrideLibraryPath = other.OverrideLibraryPath;
			this.SteamMajorVersion = other.SteamMajorVersion;
			this.SteamMinorVersion = other.SteamMinorVersion;
			this.SteamApiInterfaceVersionsArray = other.SteamApiInterfaceVersionsArray;
			this.SteamApiInterfaceVersionsArrayBytes = other.SteamApiInterfaceVersionsArrayBytes;
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x0002E89C File Offset: 0x0002CA9C
		public void Set(ref SteamOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 3;
				this.OverrideLibraryPath = other.Value.OverrideLibraryPath;
				this.SteamMajorVersion = other.Value.SteamMajorVersion;
				this.SteamMinorVersion = other.Value.SteamMinorVersion;
				this.SteamApiInterfaceVersionsArray = other.Value.SteamApiInterfaceVersionsArray;
				this.SteamApiInterfaceVersionsArrayBytes = other.Value.SteamApiInterfaceVersionsArrayBytes;
			}
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x0002E926 File Offset: 0x0002CB26
		public void Dispose()
		{
			Helper.Dispose(ref this.m_OverrideLibraryPath);
			Helper.Dispose(ref this.m_SteamApiInterfaceVersionsArray);
		}

		// Token: 0x06001FD5 RID: 8149 RVA: 0x0002E941 File Offset: 0x0002CB41
		public void Get(out SteamOptions output)
		{
			output = default(SteamOptions);
			output.Set(ref this);
		}

		// Token: 0x04000DD7 RID: 3543
		private int m_ApiVersion;

		// Token: 0x04000DD8 RID: 3544
		private IntPtr m_OverrideLibraryPath;

		// Token: 0x04000DD9 RID: 3545
		private uint m_SteamMajorVersion;

		// Token: 0x04000DDA RID: 3546
		private uint m_SteamMinorVersion;

		// Token: 0x04000DDB RID: 3547
		private IntPtr m_SteamApiInterfaceVersionsArray;

		// Token: 0x04000DDC RID: 3548
		private uint m_SteamApiInterfaceVersionsArrayBytes;
	}
}
