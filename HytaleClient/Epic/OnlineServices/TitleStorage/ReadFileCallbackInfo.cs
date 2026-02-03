using System;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x020000B9 RID: 185
	public struct ReadFileCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x00009AF2 File Offset: 0x00007CF2
		// (set) Token: 0x060006DC RID: 1756 RVA: 0x00009AFA File Offset: 0x00007CFA
		public Result ResultCode { get; set; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x00009B03 File Offset: 0x00007D03
		// (set) Token: 0x060006DE RID: 1758 RVA: 0x00009B0B File Offset: 0x00007D0B
		public object ClientData { get; set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x00009B14 File Offset: 0x00007D14
		// (set) Token: 0x060006E0 RID: 1760 RVA: 0x00009B1C File Offset: 0x00007D1C
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x00009B25 File Offset: 0x00007D25
		// (set) Token: 0x060006E2 RID: 1762 RVA: 0x00009B2D File Offset: 0x00007D2D
		public Utf8String Filename { get; set; }

		// Token: 0x060006E3 RID: 1763 RVA: 0x00009B38 File Offset: 0x00007D38
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x00009B55 File Offset: 0x00007D55
		internal void Set(ref ReadFileCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.Filename = other.Filename;
		}
	}
}
