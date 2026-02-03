using System;

namespace Epic.OnlineServices
{
	// Token: 0x0200001C RID: 28
	public struct PageQuery
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000325 RID: 805 RVA: 0x000046F3 File Offset: 0x000028F3
		// (set) Token: 0x06000326 RID: 806 RVA: 0x000046FB File Offset: 0x000028FB
		public int StartIndex { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000327 RID: 807 RVA: 0x00004704 File Offset: 0x00002904
		// (set) Token: 0x06000328 RID: 808 RVA: 0x0000470C File Offset: 0x0000290C
		public int MaxCount { get; set; }

		// Token: 0x06000329 RID: 809 RVA: 0x00004715 File Offset: 0x00002915
		internal void Set(ref PageQueryInternal other)
		{
			this.StartIndex = other.StartIndex;
			this.MaxCount = other.MaxCount;
		}
	}
}
