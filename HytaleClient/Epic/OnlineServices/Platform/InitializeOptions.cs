using System;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x02000710 RID: 1808
	public struct InitializeOptions
	{
		// Token: 0x17000DDB RID: 3547
		// (get) Token: 0x06002E9E RID: 11934 RVA: 0x00045068 File Offset: 0x00043268
		// (set) Token: 0x06002E9F RID: 11935 RVA: 0x00045070 File Offset: 0x00043270
		public IntPtr AllocateMemoryFunction { get; set; }

		// Token: 0x17000DDC RID: 3548
		// (get) Token: 0x06002EA0 RID: 11936 RVA: 0x00045079 File Offset: 0x00043279
		// (set) Token: 0x06002EA1 RID: 11937 RVA: 0x00045081 File Offset: 0x00043281
		public IntPtr ReallocateMemoryFunction { get; set; }

		// Token: 0x17000DDD RID: 3549
		// (get) Token: 0x06002EA2 RID: 11938 RVA: 0x0004508A File Offset: 0x0004328A
		// (set) Token: 0x06002EA3 RID: 11939 RVA: 0x00045092 File Offset: 0x00043292
		public IntPtr ReleaseMemoryFunction { get; set; }

		// Token: 0x17000DDE RID: 3550
		// (get) Token: 0x06002EA4 RID: 11940 RVA: 0x0004509B File Offset: 0x0004329B
		// (set) Token: 0x06002EA5 RID: 11941 RVA: 0x000450A3 File Offset: 0x000432A3
		public Utf8String ProductName { get; set; }

		// Token: 0x17000DDF RID: 3551
		// (get) Token: 0x06002EA6 RID: 11942 RVA: 0x000450AC File Offset: 0x000432AC
		// (set) Token: 0x06002EA7 RID: 11943 RVA: 0x000450B4 File Offset: 0x000432B4
		public Utf8String ProductVersion { get; set; }

		// Token: 0x17000DE0 RID: 3552
		// (get) Token: 0x06002EA8 RID: 11944 RVA: 0x000450BD File Offset: 0x000432BD
		// (set) Token: 0x06002EA9 RID: 11945 RVA: 0x000450C5 File Offset: 0x000432C5
		public IntPtr Reserved { get; set; }

		// Token: 0x17000DE1 RID: 3553
		// (get) Token: 0x06002EAA RID: 11946 RVA: 0x000450CE File Offset: 0x000432CE
		// (set) Token: 0x06002EAB RID: 11947 RVA: 0x000450D6 File Offset: 0x000432D6
		public IntPtr SystemInitializeOptions { get; set; }

		// Token: 0x17000DE2 RID: 3554
		// (get) Token: 0x06002EAC RID: 11948 RVA: 0x000450DF File Offset: 0x000432DF
		// (set) Token: 0x06002EAD RID: 11949 RVA: 0x000450E7 File Offset: 0x000432E7
		public InitializeThreadAffinity? OverrideThreadAffinity { get; set; }
	}
}
