using System;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x020000BB RID: 187
	public struct ReadFileDataCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x00009D3F File Offset: 0x00007F3F
		// (set) Token: 0x060006F3 RID: 1779 RVA: 0x00009D47 File Offset: 0x00007F47
		public object ClientData { get; set; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x00009D50 File Offset: 0x00007F50
		// (set) Token: 0x060006F5 RID: 1781 RVA: 0x00009D58 File Offset: 0x00007F58
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x00009D61 File Offset: 0x00007F61
		// (set) Token: 0x060006F7 RID: 1783 RVA: 0x00009D69 File Offset: 0x00007F69
		public Utf8String Filename { get; set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060006F8 RID: 1784 RVA: 0x00009D72 File Offset: 0x00007F72
		// (set) Token: 0x060006F9 RID: 1785 RVA: 0x00009D7A File Offset: 0x00007F7A
		public uint TotalFileSizeBytes { get; set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x00009D83 File Offset: 0x00007F83
		// (set) Token: 0x060006FB RID: 1787 RVA: 0x00009D8B File Offset: 0x00007F8B
		public bool IsLastChunk { get; set; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x00009D94 File Offset: 0x00007F94
		// (set) Token: 0x060006FD RID: 1789 RVA: 0x00009D9C File Offset: 0x00007F9C
		public ArraySegment<byte> DataChunk { get; set; }

		// Token: 0x060006FE RID: 1790 RVA: 0x00009DA8 File Offset: 0x00007FA8
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x00009DC4 File Offset: 0x00007FC4
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
