using System;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006CD RID: 1741
	public struct Vec3f
	{
		// Token: 0x17000D83 RID: 3459
		// (get) Token: 0x06002D37 RID: 11575 RVA: 0x00042CB9 File Offset: 0x00040EB9
		// (set) Token: 0x06002D38 RID: 11576 RVA: 0x00042CC1 File Offset: 0x00040EC1
		public float x { get; set; }

		// Token: 0x17000D84 RID: 3460
		// (get) Token: 0x06002D39 RID: 11577 RVA: 0x00042CCA File Offset: 0x00040ECA
		// (set) Token: 0x06002D3A RID: 11578 RVA: 0x00042CD2 File Offset: 0x00040ED2
		public float y { get; set; }

		// Token: 0x17000D85 RID: 3461
		// (get) Token: 0x06002D3B RID: 11579 RVA: 0x00042CDB File Offset: 0x00040EDB
		// (set) Token: 0x06002D3C RID: 11580 RVA: 0x00042CE3 File Offset: 0x00040EE3
		public float z { get; set; }

		// Token: 0x06002D3D RID: 11581 RVA: 0x00042CEC File Offset: 0x00040EEC
		internal void Set(ref Vec3fInternal other)
		{
			this.x = other.x;
			this.y = other.y;
			this.z = other.z;
		}
	}
}
