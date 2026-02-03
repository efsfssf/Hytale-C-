using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x020000FE RID: 254
	public struct DestroySessionCallbackInfo : ICallbackInfo
	{
		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000872 RID: 2162 RVA: 0x0000C58D File Offset: 0x0000A78D
		// (set) Token: 0x06000873 RID: 2163 RVA: 0x0000C595 File Offset: 0x0000A795
		public Result ResultCode { get; set; }

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000874 RID: 2164 RVA: 0x0000C59E File Offset: 0x0000A79E
		// (set) Token: 0x06000875 RID: 2165 RVA: 0x0000C5A6 File Offset: 0x0000A7A6
		public object ClientData { get; set; }

		// Token: 0x06000876 RID: 2166 RVA: 0x0000C5B0 File Offset: 0x0000A7B0
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x0000C5CD File Offset: 0x0000A7CD
		internal void Set(ref DestroySessionCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}
	}
}
