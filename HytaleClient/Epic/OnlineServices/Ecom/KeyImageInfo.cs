using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200053E RID: 1342
	public struct KeyImageInfo
	{
		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x06002312 RID: 8978 RVA: 0x00033EDC File Offset: 0x000320DC
		// (set) Token: 0x06002313 RID: 8979 RVA: 0x00033EE4 File Offset: 0x000320E4
		public Utf8String Type { get; set; }

		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x06002314 RID: 8980 RVA: 0x00033EED File Offset: 0x000320ED
		// (set) Token: 0x06002315 RID: 8981 RVA: 0x00033EF5 File Offset: 0x000320F5
		public Utf8String Url { get; set; }

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x06002316 RID: 8982 RVA: 0x00033EFE File Offset: 0x000320FE
		// (set) Token: 0x06002317 RID: 8983 RVA: 0x00033F06 File Offset: 0x00032106
		public uint Width { get; set; }

		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x06002318 RID: 8984 RVA: 0x00033F0F File Offset: 0x0003210F
		// (set) Token: 0x06002319 RID: 8985 RVA: 0x00033F17 File Offset: 0x00032117
		public uint Height { get; set; }

		// Token: 0x0600231A RID: 8986 RVA: 0x00033F20 File Offset: 0x00032120
		internal void Set(ref KeyImageInfoInternal other)
		{
			this.Type = other.Type;
			this.Url = other.Url;
			this.Width = other.Width;
			this.Height = other.Height;
		}
	}
}
