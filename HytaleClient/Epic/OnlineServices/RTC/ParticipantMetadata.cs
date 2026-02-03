using System;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001CE RID: 462
	public struct ParticipantMetadata
	{
		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000D4B RID: 3403 RVA: 0x00013404 File Offset: 0x00011604
		// (set) Token: 0x06000D4C RID: 3404 RVA: 0x0001340C File Offset: 0x0001160C
		public Utf8String Key { get; set; }

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000D4D RID: 3405 RVA: 0x00013415 File Offset: 0x00011615
		// (set) Token: 0x06000D4E RID: 3406 RVA: 0x0001341D File Offset: 0x0001161D
		public Utf8String Value { get; set; }

		// Token: 0x06000D4F RID: 3407 RVA: 0x00013426 File Offset: 0x00011626
		internal void Set(ref ParticipantMetadataInternal other)
		{
			this.Key = other.Key;
			this.Value = other.Value;
		}
	}
}
