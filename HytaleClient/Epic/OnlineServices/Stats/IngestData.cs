using System;

namespace Epic.OnlineServices.Stats
{
	// Token: 0x020000C8 RID: 200
	public struct IngestData
	{
		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000750 RID: 1872 RVA: 0x0000A961 File Offset: 0x00008B61
		// (set) Token: 0x06000751 RID: 1873 RVA: 0x0000A969 File Offset: 0x00008B69
		public Utf8String StatName { get; set; }

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000752 RID: 1874 RVA: 0x0000A972 File Offset: 0x00008B72
		// (set) Token: 0x06000753 RID: 1875 RVA: 0x0000A97A File Offset: 0x00008B7A
		public int IngestAmount { get; set; }

		// Token: 0x06000754 RID: 1876 RVA: 0x0000A983 File Offset: 0x00008B83
		internal void Set(ref IngestDataInternal other)
		{
			this.StatName = other.StatName;
			this.IngestAmount = other.IngestAmount;
		}
	}
}
