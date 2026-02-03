using System;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x0200079B RID: 1947
	public struct OnQueryNATTypeCompleteInfo : ICallbackInfo
	{
		// Token: 0x17000F41 RID: 3905
		// (get) Token: 0x06003270 RID: 12912 RVA: 0x0004B4FF File Offset: 0x000496FF
		// (set) Token: 0x06003271 RID: 12913 RVA: 0x0004B507 File Offset: 0x00049707
		public Result ResultCode { get; set; }

		// Token: 0x17000F42 RID: 3906
		// (get) Token: 0x06003272 RID: 12914 RVA: 0x0004B510 File Offset: 0x00049710
		// (set) Token: 0x06003273 RID: 12915 RVA: 0x0004B518 File Offset: 0x00049718
		public object ClientData { get; set; }

		// Token: 0x17000F43 RID: 3907
		// (get) Token: 0x06003274 RID: 12916 RVA: 0x0004B521 File Offset: 0x00049721
		// (set) Token: 0x06003275 RID: 12917 RVA: 0x0004B529 File Offset: 0x00049729
		public NATType NATType { get; set; }

		// Token: 0x06003276 RID: 12918 RVA: 0x0004B534 File Offset: 0x00049734
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06003277 RID: 12919 RVA: 0x0004B551 File Offset: 0x00049751
		internal void Set(ref OnQueryNATTypeCompleteInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.NATType = other.NATType;
		}
	}
}
