using System;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x02000704 RID: 1796
	public struct AndroidInitializeOptionsSystemInitializeOptions
	{
		// Token: 0x17000DCD RID: 3533
		// (get) Token: 0x06002E39 RID: 11833 RVA: 0x000443EA File Offset: 0x000425EA
		// (set) Token: 0x06002E3A RID: 11834 RVA: 0x000443F2 File Offset: 0x000425F2
		public IntPtr Reserved { get; set; }

		// Token: 0x17000DCE RID: 3534
		// (get) Token: 0x06002E3B RID: 11835 RVA: 0x000443FB File Offset: 0x000425FB
		// (set) Token: 0x06002E3C RID: 11836 RVA: 0x00044403 File Offset: 0x00042603
		public Utf8String OptionalInternalDirectory { get; set; }

		// Token: 0x17000DCF RID: 3535
		// (get) Token: 0x06002E3D RID: 11837 RVA: 0x0004440C File Offset: 0x0004260C
		// (set) Token: 0x06002E3E RID: 11838 RVA: 0x00044414 File Offset: 0x00042614
		public Utf8String OptionalExternalDirectory { get; set; }

		// Token: 0x06002E3F RID: 11839 RVA: 0x0004441D File Offset: 0x0004261D
		internal void Set(ref AndroidInitializeOptionsSystemInitializeOptionsInternal other)
		{
			this.Reserved = other.Reserved;
			this.OptionalInternalDirectory = other.OptionalInternalDirectory;
			this.OptionalExternalDirectory = other.OptionalExternalDirectory;
		}
	}
}
