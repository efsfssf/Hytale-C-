using System;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x02000702 RID: 1794
	public struct AndroidInitializeOptions
	{
		// Token: 0x17000DBD RID: 3517
		// (get) Token: 0x06002E1E RID: 11806 RVA: 0x000440E1 File Offset: 0x000422E1
		// (set) Token: 0x06002E1F RID: 11807 RVA: 0x000440E9 File Offset: 0x000422E9
		public IntPtr AllocateMemoryFunction { get; set; }

		// Token: 0x17000DBE RID: 3518
		// (get) Token: 0x06002E20 RID: 11808 RVA: 0x000440F2 File Offset: 0x000422F2
		// (set) Token: 0x06002E21 RID: 11809 RVA: 0x000440FA File Offset: 0x000422FA
		public IntPtr ReallocateMemoryFunction { get; set; }

		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x06002E22 RID: 11810 RVA: 0x00044103 File Offset: 0x00042303
		// (set) Token: 0x06002E23 RID: 11811 RVA: 0x0004410B File Offset: 0x0004230B
		public IntPtr ReleaseMemoryFunction { get; set; }

		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x06002E24 RID: 11812 RVA: 0x00044114 File Offset: 0x00042314
		// (set) Token: 0x06002E25 RID: 11813 RVA: 0x0004411C File Offset: 0x0004231C
		public Utf8String ProductName { get; set; }

		// Token: 0x17000DC1 RID: 3521
		// (get) Token: 0x06002E26 RID: 11814 RVA: 0x00044125 File Offset: 0x00042325
		// (set) Token: 0x06002E27 RID: 11815 RVA: 0x0004412D File Offset: 0x0004232D
		public Utf8String ProductVersion { get; set; }

		// Token: 0x17000DC2 RID: 3522
		// (get) Token: 0x06002E28 RID: 11816 RVA: 0x00044136 File Offset: 0x00042336
		// (set) Token: 0x06002E29 RID: 11817 RVA: 0x0004413E File Offset: 0x0004233E
		public IntPtr Reserved { get; set; }

		// Token: 0x17000DC3 RID: 3523
		// (get) Token: 0x06002E2A RID: 11818 RVA: 0x00044147 File Offset: 0x00042347
		// (set) Token: 0x06002E2B RID: 11819 RVA: 0x0004414F File Offset: 0x0004234F
		public AndroidInitializeOptionsSystemInitializeOptions? SystemInitializeOptions { get; set; }

		// Token: 0x17000DC4 RID: 3524
		// (get) Token: 0x06002E2C RID: 11820 RVA: 0x00044158 File Offset: 0x00042358
		// (set) Token: 0x06002E2D RID: 11821 RVA: 0x00044160 File Offset: 0x00042360
		public InitializeThreadAffinity? OverrideThreadAffinity { get; set; }
	}
}
