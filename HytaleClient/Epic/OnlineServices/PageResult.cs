using System;

namespace Epic.OnlineServices
{
	// Token: 0x0200001E RID: 30
	public struct PageResult
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000332 RID: 818 RVA: 0x00004800 File Offset: 0x00002A00
		// (set) Token: 0x06000333 RID: 819 RVA: 0x00004808 File Offset: 0x00002A08
		public int StartIndex { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000334 RID: 820 RVA: 0x00004811 File Offset: 0x00002A11
		// (set) Token: 0x06000335 RID: 821 RVA: 0x00004819 File Offset: 0x00002A19
		public int Count { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000336 RID: 822 RVA: 0x00004822 File Offset: 0x00002A22
		// (set) Token: 0x06000337 RID: 823 RVA: 0x0000482A File Offset: 0x00002A2A
		public int TotalCount { get; set; }

		// Token: 0x06000338 RID: 824 RVA: 0x00004833 File Offset: 0x00002A33
		internal void Set(ref PageResultInternal other)
		{
			this.StartIndex = other.StartIndex;
			this.Count = other.Count;
			this.TotalCount = other.TotalCount;
		}
	}
}
