using System;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006C3 RID: 1731
	public struct Quat
	{
		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x06002CF4 RID: 11508 RVA: 0x0004271A File Offset: 0x0004091A
		// (set) Token: 0x06002CF5 RID: 11509 RVA: 0x00042722 File Offset: 0x00040922
		public float w { get; set; }

		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x06002CF6 RID: 11510 RVA: 0x0004272B File Offset: 0x0004092B
		// (set) Token: 0x06002CF7 RID: 11511 RVA: 0x00042733 File Offset: 0x00040933
		public float x { get; set; }

		// Token: 0x17000D69 RID: 3433
		// (get) Token: 0x06002CF8 RID: 11512 RVA: 0x0004273C File Offset: 0x0004093C
		// (set) Token: 0x06002CF9 RID: 11513 RVA: 0x00042744 File Offset: 0x00040944
		public float y { get; set; }

		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x06002CFA RID: 11514 RVA: 0x0004274D File Offset: 0x0004094D
		// (set) Token: 0x06002CFB RID: 11515 RVA: 0x00042755 File Offset: 0x00040955
		public float z { get; set; }

		// Token: 0x06002CFC RID: 11516 RVA: 0x0004275E File Offset: 0x0004095E
		internal void Set(ref QuatInternal other)
		{
			this.w = other.w;
			this.x = other.x;
			this.y = other.y;
			this.z = other.z;
		}
	}
}
