using System;

namespace Epic.OnlineServices.Stats
{
	// Token: 0x020000D2 RID: 210
	public struct OnQueryStatsCompleteCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000790 RID: 1936 RVA: 0x0000ADF7 File Offset: 0x00008FF7
		// (set) Token: 0x06000791 RID: 1937 RVA: 0x0000ADFF File Offset: 0x00008FFF
		public Result ResultCode { get; set; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000792 RID: 1938 RVA: 0x0000AE08 File Offset: 0x00009008
		// (set) Token: 0x06000793 RID: 1939 RVA: 0x0000AE10 File Offset: 0x00009010
		public object ClientData { get; set; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x0000AE19 File Offset: 0x00009019
		// (set) Token: 0x06000795 RID: 1941 RVA: 0x0000AE21 File Offset: 0x00009021
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x0000AE2A File Offset: 0x0000902A
		// (set) Token: 0x06000797 RID: 1943 RVA: 0x0000AE32 File Offset: 0x00009032
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x06000798 RID: 1944 RVA: 0x0000AE3C File Offset: 0x0000903C
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0000AE59 File Offset: 0x00009059
		internal void Set(ref OnQueryStatsCompleteCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}
	}
}
