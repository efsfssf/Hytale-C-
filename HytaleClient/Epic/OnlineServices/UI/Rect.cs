using System;

namespace Epic.OnlineServices.UI
{
	// Token: 0x0200007E RID: 126
	public struct Rect
	{
		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x00007759 File Offset: 0x00005959
		// (set) Token: 0x06000552 RID: 1362 RVA: 0x00007761 File Offset: 0x00005961
		public int X { get; set; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x0000776A File Offset: 0x0000596A
		// (set) Token: 0x06000554 RID: 1364 RVA: 0x00007772 File Offset: 0x00005972
		public int Y { get; set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000555 RID: 1365 RVA: 0x0000777B File Offset: 0x0000597B
		// (set) Token: 0x06000556 RID: 1366 RVA: 0x00007783 File Offset: 0x00005983
		public uint Width { get; set; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x0000778C File Offset: 0x0000598C
		// (set) Token: 0x06000558 RID: 1368 RVA: 0x00007794 File Offset: 0x00005994
		public uint Height { get; set; }

		// Token: 0x06000559 RID: 1369 RVA: 0x0000779D File Offset: 0x0000599D
		internal void Set(ref RectInternal other)
		{
			this.X = other.X;
			this.Y = other.Y;
			this.Width = other.Width;
			this.Height = other.Height;
		}
	}
}
