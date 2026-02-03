using System;
using Epic.OnlineServices.IntegratedPlatform;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x02000715 RID: 1813
	public struct Options
	{
		// Token: 0x17000DFB RID: 3579
		// (get) Token: 0x06002EDE RID: 11998 RVA: 0x000456C1 File Offset: 0x000438C1
		// (set) Token: 0x06002EDF RID: 11999 RVA: 0x000456C9 File Offset: 0x000438C9
		public IntPtr Reserved { get; set; }

		// Token: 0x17000DFC RID: 3580
		// (get) Token: 0x06002EE0 RID: 12000 RVA: 0x000456D2 File Offset: 0x000438D2
		// (set) Token: 0x06002EE1 RID: 12001 RVA: 0x000456DA File Offset: 0x000438DA
		public Utf8String ProductId { get; set; }

		// Token: 0x17000DFD RID: 3581
		// (get) Token: 0x06002EE2 RID: 12002 RVA: 0x000456E3 File Offset: 0x000438E3
		// (set) Token: 0x06002EE3 RID: 12003 RVA: 0x000456EB File Offset: 0x000438EB
		public Utf8String SandboxId { get; set; }

		// Token: 0x17000DFE RID: 3582
		// (get) Token: 0x06002EE4 RID: 12004 RVA: 0x000456F4 File Offset: 0x000438F4
		// (set) Token: 0x06002EE5 RID: 12005 RVA: 0x000456FC File Offset: 0x000438FC
		public ClientCredentials ClientCredentials { get; set; }

		// Token: 0x17000DFF RID: 3583
		// (get) Token: 0x06002EE6 RID: 12006 RVA: 0x00045705 File Offset: 0x00043905
		// (set) Token: 0x06002EE7 RID: 12007 RVA: 0x0004570D File Offset: 0x0004390D
		public bool IsServer { get; set; }

		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x06002EE8 RID: 12008 RVA: 0x00045716 File Offset: 0x00043916
		// (set) Token: 0x06002EE9 RID: 12009 RVA: 0x0004571E File Offset: 0x0004391E
		public Utf8String EncryptionKey { get; set; }

		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x06002EEA RID: 12010 RVA: 0x00045727 File Offset: 0x00043927
		// (set) Token: 0x06002EEB RID: 12011 RVA: 0x0004572F File Offset: 0x0004392F
		public Utf8String OverrideCountryCode { get; set; }

		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x06002EEC RID: 12012 RVA: 0x00045738 File Offset: 0x00043938
		// (set) Token: 0x06002EED RID: 12013 RVA: 0x00045740 File Offset: 0x00043940
		public Utf8String OverrideLocaleCode { get; set; }

		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x06002EEE RID: 12014 RVA: 0x00045749 File Offset: 0x00043949
		// (set) Token: 0x06002EEF RID: 12015 RVA: 0x00045751 File Offset: 0x00043951
		public Utf8String DeploymentId { get; set; }

		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x06002EF0 RID: 12016 RVA: 0x0004575A File Offset: 0x0004395A
		// (set) Token: 0x06002EF1 RID: 12017 RVA: 0x00045762 File Offset: 0x00043962
		public PlatformFlags Flags { get; set; }

		// Token: 0x17000E05 RID: 3589
		// (get) Token: 0x06002EF2 RID: 12018 RVA: 0x0004576B File Offset: 0x0004396B
		// (set) Token: 0x06002EF3 RID: 12019 RVA: 0x00045773 File Offset: 0x00043973
		public Utf8String CacheDirectory { get; set; }

		// Token: 0x17000E06 RID: 3590
		// (get) Token: 0x06002EF4 RID: 12020 RVA: 0x0004577C File Offset: 0x0004397C
		// (set) Token: 0x06002EF5 RID: 12021 RVA: 0x00045784 File Offset: 0x00043984
		public uint TickBudgetInMilliseconds { get; set; }

		// Token: 0x17000E07 RID: 3591
		// (get) Token: 0x06002EF6 RID: 12022 RVA: 0x0004578D File Offset: 0x0004398D
		// (set) Token: 0x06002EF7 RID: 12023 RVA: 0x00045795 File Offset: 0x00043995
		public RTCOptions? RTCOptions { get; set; }

		// Token: 0x17000E08 RID: 3592
		// (get) Token: 0x06002EF8 RID: 12024 RVA: 0x0004579E File Offset: 0x0004399E
		// (set) Token: 0x06002EF9 RID: 12025 RVA: 0x000457A6 File Offset: 0x000439A6
		public IntegratedPlatformOptionsContainer IntegratedPlatformOptionsContainerHandle { get; set; }

		// Token: 0x17000E09 RID: 3593
		// (get) Token: 0x06002EFA RID: 12026 RVA: 0x000457AF File Offset: 0x000439AF
		// (set) Token: 0x06002EFB RID: 12027 RVA: 0x000457B7 File Offset: 0x000439B7
		public IntPtr SystemSpecificOptions { get; set; }

		// Token: 0x17000E0A RID: 3594
		// (get) Token: 0x06002EFC RID: 12028 RVA: 0x000457C0 File Offset: 0x000439C0
		// (set) Token: 0x06002EFD RID: 12029 RVA: 0x000457C8 File Offset: 0x000439C8
		public double? TaskNetworkTimeoutSeconds { get; set; }
	}
}
