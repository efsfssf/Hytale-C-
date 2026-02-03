using System;
using Epic.OnlineServices.IntegratedPlatform;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x0200071D RID: 1821
	public struct WindowsOptions
	{
		// Token: 0x17000E1F RID: 3615
		// (get) Token: 0x06002F26 RID: 12070 RVA: 0x00045CE0 File Offset: 0x00043EE0
		// (set) Token: 0x06002F27 RID: 12071 RVA: 0x00045CE8 File Offset: 0x00043EE8
		public IntPtr Reserved { get; set; }

		// Token: 0x17000E20 RID: 3616
		// (get) Token: 0x06002F28 RID: 12072 RVA: 0x00045CF1 File Offset: 0x00043EF1
		// (set) Token: 0x06002F29 RID: 12073 RVA: 0x00045CF9 File Offset: 0x00043EF9
		public Utf8String ProductId { get; set; }

		// Token: 0x17000E21 RID: 3617
		// (get) Token: 0x06002F2A RID: 12074 RVA: 0x00045D02 File Offset: 0x00043F02
		// (set) Token: 0x06002F2B RID: 12075 RVA: 0x00045D0A File Offset: 0x00043F0A
		public Utf8String SandboxId { get; set; }

		// Token: 0x17000E22 RID: 3618
		// (get) Token: 0x06002F2C RID: 12076 RVA: 0x00045D13 File Offset: 0x00043F13
		// (set) Token: 0x06002F2D RID: 12077 RVA: 0x00045D1B File Offset: 0x00043F1B
		public ClientCredentials ClientCredentials { get; set; }

		// Token: 0x17000E23 RID: 3619
		// (get) Token: 0x06002F2E RID: 12078 RVA: 0x00045D24 File Offset: 0x00043F24
		// (set) Token: 0x06002F2F RID: 12079 RVA: 0x00045D2C File Offset: 0x00043F2C
		public bool IsServer { get; set; }

		// Token: 0x17000E24 RID: 3620
		// (get) Token: 0x06002F30 RID: 12080 RVA: 0x00045D35 File Offset: 0x00043F35
		// (set) Token: 0x06002F31 RID: 12081 RVA: 0x00045D3D File Offset: 0x00043F3D
		public Utf8String EncryptionKey { get; set; }

		// Token: 0x17000E25 RID: 3621
		// (get) Token: 0x06002F32 RID: 12082 RVA: 0x00045D46 File Offset: 0x00043F46
		// (set) Token: 0x06002F33 RID: 12083 RVA: 0x00045D4E File Offset: 0x00043F4E
		public Utf8String OverrideCountryCode { get; set; }

		// Token: 0x17000E26 RID: 3622
		// (get) Token: 0x06002F34 RID: 12084 RVA: 0x00045D57 File Offset: 0x00043F57
		// (set) Token: 0x06002F35 RID: 12085 RVA: 0x00045D5F File Offset: 0x00043F5F
		public Utf8String OverrideLocaleCode { get; set; }

		// Token: 0x17000E27 RID: 3623
		// (get) Token: 0x06002F36 RID: 12086 RVA: 0x00045D68 File Offset: 0x00043F68
		// (set) Token: 0x06002F37 RID: 12087 RVA: 0x00045D70 File Offset: 0x00043F70
		public Utf8String DeploymentId { get; set; }

		// Token: 0x17000E28 RID: 3624
		// (get) Token: 0x06002F38 RID: 12088 RVA: 0x00045D79 File Offset: 0x00043F79
		// (set) Token: 0x06002F39 RID: 12089 RVA: 0x00045D81 File Offset: 0x00043F81
		public PlatformFlags Flags { get; set; }

		// Token: 0x17000E29 RID: 3625
		// (get) Token: 0x06002F3A RID: 12090 RVA: 0x00045D8A File Offset: 0x00043F8A
		// (set) Token: 0x06002F3B RID: 12091 RVA: 0x00045D92 File Offset: 0x00043F92
		public Utf8String CacheDirectory { get; set; }

		// Token: 0x17000E2A RID: 3626
		// (get) Token: 0x06002F3C RID: 12092 RVA: 0x00045D9B File Offset: 0x00043F9B
		// (set) Token: 0x06002F3D RID: 12093 RVA: 0x00045DA3 File Offset: 0x00043FA3
		public uint TickBudgetInMilliseconds { get; set; }

		// Token: 0x17000E2B RID: 3627
		// (get) Token: 0x06002F3E RID: 12094 RVA: 0x00045DAC File Offset: 0x00043FAC
		// (set) Token: 0x06002F3F RID: 12095 RVA: 0x00045DB4 File Offset: 0x00043FB4
		public WindowsRTCOptions? RTCOptions { get; set; }

		// Token: 0x17000E2C RID: 3628
		// (get) Token: 0x06002F40 RID: 12096 RVA: 0x00045DBD File Offset: 0x00043FBD
		// (set) Token: 0x06002F41 RID: 12097 RVA: 0x00045DC5 File Offset: 0x00043FC5
		public IntegratedPlatformOptionsContainer IntegratedPlatformOptionsContainerHandle { get; set; }

		// Token: 0x17000E2D RID: 3629
		// (get) Token: 0x06002F42 RID: 12098 RVA: 0x00045DCE File Offset: 0x00043FCE
		// (set) Token: 0x06002F43 RID: 12099 RVA: 0x00045DD6 File Offset: 0x00043FD6
		public IntPtr SystemSpecificOptions { get; set; }

		// Token: 0x17000E2E RID: 3630
		// (get) Token: 0x06002F44 RID: 12100 RVA: 0x00045DDF File Offset: 0x00043FDF
		// (set) Token: 0x06002F45 RID: 12101 RVA: 0x00045DE7 File Offset: 0x00043FE7
		public double? TaskNetworkTimeoutSeconds { get; set; }
	}
}
