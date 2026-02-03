using System;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x02000321 RID: 801
	public struct ReadFileDataCallbackInfo : ICallbackInfo
	{
		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x060015D4 RID: 5588 RVA: 0x0001FB63 File Offset: 0x0001DD63
		// (set) Token: 0x060015D5 RID: 5589 RVA: 0x0001FB6B File Offset: 0x0001DD6B
		public object ClientData { get; set; }

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x060015D6 RID: 5590 RVA: 0x0001FB74 File Offset: 0x0001DD74
		// (set) Token: 0x060015D7 RID: 5591 RVA: 0x0001FB7C File Offset: 0x0001DD7C
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x060015D8 RID: 5592 RVA: 0x0001FB85 File Offset: 0x0001DD85
		// (set) Token: 0x060015D9 RID: 5593 RVA: 0x0001FB8D File Offset: 0x0001DD8D
		public Utf8String Filename { get; set; }

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x060015DA RID: 5594 RVA: 0x0001FB96 File Offset: 0x0001DD96
		// (set) Token: 0x060015DB RID: 5595 RVA: 0x0001FB9E File Offset: 0x0001DD9E
		public uint TotalFileSizeBytes { get; set; }

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x060015DC RID: 5596 RVA: 0x0001FBA7 File Offset: 0x0001DDA7
		// (set) Token: 0x060015DD RID: 5597 RVA: 0x0001FBAF File Offset: 0x0001DDAF
		public bool IsLastChunk { get; set; }

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x060015DE RID: 5598 RVA: 0x0001FBB8 File Offset: 0x0001DDB8
		// (set) Token: 0x060015DF RID: 5599 RVA: 0x0001FBC0 File Offset: 0x0001DDC0
		public ArraySegment<byte> DataChunk { get; set; }

		// Token: 0x060015E0 RID: 5600 RVA: 0x0001FBCC File Offset: 0x0001DDCC
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x060015E1 RID: 5601 RVA: 0x0001FBE8 File Offset: 0x0001DDE8
		internal void Set(ref ReadFileDataCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.Filename = other.Filename;
			this.TotalFileSizeBytes = other.TotalFileSizeBytes;
			this.IsLastChunk = other.IsLastChunk;
			this.DataChunk = other.DataChunk;
		}
	}
}
