using System;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002C6 RID: 710
	public struct Info
	{
		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06001385 RID: 4997 RVA: 0x0001C656 File Offset: 0x0001A856
		// (set) Token: 0x06001386 RID: 4998 RVA: 0x0001C65E File Offset: 0x0001A85E
		public Status Status { get; set; }

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001387 RID: 4999 RVA: 0x0001C667 File Offset: 0x0001A867
		// (set) Token: 0x06001388 RID: 5000 RVA: 0x0001C66F File Offset: 0x0001A86F
		public EpicAccountId UserId { get; set; }

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06001389 RID: 5001 RVA: 0x0001C678 File Offset: 0x0001A878
		// (set) Token: 0x0600138A RID: 5002 RVA: 0x0001C680 File Offset: 0x0001A880
		public Utf8String ProductId { get; set; }

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x0600138B RID: 5003 RVA: 0x0001C689 File Offset: 0x0001A889
		// (set) Token: 0x0600138C RID: 5004 RVA: 0x0001C691 File Offset: 0x0001A891
		public Utf8String ProductVersion { get; set; }

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x0600138D RID: 5005 RVA: 0x0001C69A File Offset: 0x0001A89A
		// (set) Token: 0x0600138E RID: 5006 RVA: 0x0001C6A2 File Offset: 0x0001A8A2
		public Utf8String Platform { get; set; }

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x0600138F RID: 5007 RVA: 0x0001C6AB File Offset: 0x0001A8AB
		// (set) Token: 0x06001390 RID: 5008 RVA: 0x0001C6B3 File Offset: 0x0001A8B3
		public Utf8String RichText { get; set; }

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001391 RID: 5009 RVA: 0x0001C6BC File Offset: 0x0001A8BC
		// (set) Token: 0x06001392 RID: 5010 RVA: 0x0001C6C4 File Offset: 0x0001A8C4
		public DataRecord[] Records { get; set; }

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001393 RID: 5011 RVA: 0x0001C6CD File Offset: 0x0001A8CD
		// (set) Token: 0x06001394 RID: 5012 RVA: 0x0001C6D5 File Offset: 0x0001A8D5
		public Utf8String ProductName { get; set; }

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001395 RID: 5013 RVA: 0x0001C6DE File Offset: 0x0001A8DE
		// (set) Token: 0x06001396 RID: 5014 RVA: 0x0001C6E6 File Offset: 0x0001A8E6
		public Utf8String IntegratedPlatform { get; set; }

		// Token: 0x06001397 RID: 5015 RVA: 0x0001C6F0 File Offset: 0x0001A8F0
		internal void Set(ref InfoInternal other)
		{
			this.Status = other.Status;
			this.UserId = other.UserId;
			this.ProductId = other.ProductId;
			this.ProductVersion = other.ProductVersion;
			this.Platform = other.Platform;
			this.RichText = other.RichText;
			this.Records = other.Records;
			this.ProductName = other.ProductName;
			this.IntegratedPlatform = other.IntegratedPlatform;
		}
	}
}
