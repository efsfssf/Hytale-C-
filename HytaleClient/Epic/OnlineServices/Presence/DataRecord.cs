using System;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002C0 RID: 704
	public struct DataRecord
	{
		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001366 RID: 4966 RVA: 0x0001C379 File Offset: 0x0001A579
		// (set) Token: 0x06001367 RID: 4967 RVA: 0x0001C381 File Offset: 0x0001A581
		public Utf8String Key { get; set; }

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001368 RID: 4968 RVA: 0x0001C38A File Offset: 0x0001A58A
		// (set) Token: 0x06001369 RID: 4969 RVA: 0x0001C392 File Offset: 0x0001A592
		public Utf8String Value { get; set; }

		// Token: 0x0600136A RID: 4970 RVA: 0x0001C39B File Offset: 0x0001A59B
		internal void Set(ref DataRecordInternal other)
		{
			this.Key = other.Key;
			this.Value = other.Value;
		}
	}
}
