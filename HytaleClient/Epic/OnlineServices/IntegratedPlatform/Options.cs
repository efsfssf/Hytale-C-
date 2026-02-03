using System;

namespace Epic.OnlineServices.IntegratedPlatform
{
	// Token: 0x020004C3 RID: 1219
	public struct Options
	{
		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06001F9D RID: 8093 RVA: 0x0002E3F5 File Offset: 0x0002C5F5
		// (set) Token: 0x06001F9E RID: 8094 RVA: 0x0002E3FD File Offset: 0x0002C5FD
		public Utf8String Type { get; set; }

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06001F9F RID: 8095 RVA: 0x0002E406 File Offset: 0x0002C606
		// (set) Token: 0x06001FA0 RID: 8096 RVA: 0x0002E40E File Offset: 0x0002C60E
		public IntegratedPlatformManagementFlags Flags { get; set; }

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06001FA1 RID: 8097 RVA: 0x0002E417 File Offset: 0x0002C617
		// (set) Token: 0x06001FA2 RID: 8098 RVA: 0x0002E41F File Offset: 0x0002C61F
		public IntPtr InitOptions { get; set; }

		// Token: 0x06001FA3 RID: 8099 RVA: 0x0002E428 File Offset: 0x0002C628
		internal void Set(ref OptionsInternal other)
		{
			this.Type = other.Type;
			this.Flags = other.Flags;
			this.InitOptions = other.InitOptions;
		}
	}
}
