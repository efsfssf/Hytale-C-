using System;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x0200074C RID: 1868
	public struct OnQueryDefinitionsCompleteCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000E9F RID: 3743
		// (get) Token: 0x0600307E RID: 12414 RVA: 0x0004812F File Offset: 0x0004632F
		// (set) Token: 0x0600307F RID: 12415 RVA: 0x00048137 File Offset: 0x00046337
		public Result ResultCode { get; set; }

		// Token: 0x17000EA0 RID: 3744
		// (get) Token: 0x06003080 RID: 12416 RVA: 0x00048140 File Offset: 0x00046340
		// (set) Token: 0x06003081 RID: 12417 RVA: 0x00048148 File Offset: 0x00046348
		public object ClientData { get; set; }

		// Token: 0x06003082 RID: 12418 RVA: 0x00048154 File Offset: 0x00046354
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06003083 RID: 12419 RVA: 0x00048171 File Offset: 0x00046371
		internal void Set(ref OnQueryDefinitionsCompleteCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}
	}
}
