using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x02000613 RID: 1555
	public struct QueryExternalAccountMappingsCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x06002825 RID: 10277 RVA: 0x0003AD61 File Offset: 0x00038F61
		// (set) Token: 0x06002826 RID: 10278 RVA: 0x0003AD69 File Offset: 0x00038F69
		public Result ResultCode { get; set; }

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x06002827 RID: 10279 RVA: 0x0003AD72 File Offset: 0x00038F72
		// (set) Token: 0x06002828 RID: 10280 RVA: 0x0003AD7A File Offset: 0x00038F7A
		public object ClientData { get; set; }

		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x06002829 RID: 10281 RVA: 0x0003AD83 File Offset: 0x00038F83
		// (set) Token: 0x0600282A RID: 10282 RVA: 0x0003AD8B File Offset: 0x00038F8B
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x0600282B RID: 10283 RVA: 0x0003AD94 File Offset: 0x00038F94
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600282C RID: 10284 RVA: 0x0003ADB1 File Offset: 0x00038FB1
		internal void Set(ref QueryExternalAccountMappingsCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
