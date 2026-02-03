using System;

namespace Epic.OnlineServices.IntegratedPlatform
{
	// Token: 0x020004C9 RID: 1225
	public struct SteamOptions
	{
		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x06001FBD RID: 8125 RVA: 0x0002E6C8 File Offset: 0x0002C8C8
		// (set) Token: 0x06001FBE RID: 8126 RVA: 0x0002E6D0 File Offset: 0x0002C8D0
		public Utf8String OverrideLibraryPath { get; set; }

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06001FBF RID: 8127 RVA: 0x0002E6D9 File Offset: 0x0002C8D9
		// (set) Token: 0x06001FC0 RID: 8128 RVA: 0x0002E6E1 File Offset: 0x0002C8E1
		public uint SteamMajorVersion { get; set; }

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06001FC1 RID: 8129 RVA: 0x0002E6EA File Offset: 0x0002C8EA
		// (set) Token: 0x06001FC2 RID: 8130 RVA: 0x0002E6F2 File Offset: 0x0002C8F2
		public uint SteamMinorVersion { get; set; }

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06001FC3 RID: 8131 RVA: 0x0002E6FB File Offset: 0x0002C8FB
		// (set) Token: 0x06001FC4 RID: 8132 RVA: 0x0002E703 File Offset: 0x0002C903
		public Utf8String SteamApiInterfaceVersionsArray { get; set; }

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06001FC5 RID: 8133 RVA: 0x0002E70C File Offset: 0x0002C90C
		// (set) Token: 0x06001FC6 RID: 8134 RVA: 0x0002E714 File Offset: 0x0002C914
		public uint SteamApiInterfaceVersionsArrayBytes { get; set; }

		// Token: 0x06001FC7 RID: 8135 RVA: 0x0002E720 File Offset: 0x0002C920
		internal void Set(ref SteamOptionsInternal other)
		{
			this.OverrideLibraryPath = other.OverrideLibraryPath;
			this.SteamMajorVersion = other.SteamMajorVersion;
			this.SteamMinorVersion = other.SteamMinorVersion;
			this.SteamApiInterfaceVersionsArray = other.SteamApiInterfaceVersionsArray;
			this.SteamApiInterfaceVersionsArrayBytes = other.SteamApiInterfaceVersionsArrayBytes;
		}
	}
}
