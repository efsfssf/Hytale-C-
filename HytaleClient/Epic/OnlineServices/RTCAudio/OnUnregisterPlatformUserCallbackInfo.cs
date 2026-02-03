using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000246 RID: 582
	public struct OnUnregisterPlatformUserCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06001052 RID: 4178 RVA: 0x00017706 File Offset: 0x00015906
		// (set) Token: 0x06001053 RID: 4179 RVA: 0x0001770E File Offset: 0x0001590E
		public Result ResultCode { get; set; }

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06001054 RID: 4180 RVA: 0x00017717 File Offset: 0x00015917
		// (set) Token: 0x06001055 RID: 4181 RVA: 0x0001771F File Offset: 0x0001591F
		public object ClientData { get; set; }

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06001056 RID: 4182 RVA: 0x00017728 File Offset: 0x00015928
		// (set) Token: 0x06001057 RID: 4183 RVA: 0x00017730 File Offset: 0x00015930
		public Utf8String PlatformUserId { get; set; }

		// Token: 0x06001058 RID: 4184 RVA: 0x0001773C File Offset: 0x0001593C
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x00017759 File Offset: 0x00015959
		internal void Set(ref OnUnregisterPlatformUserCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.PlatformUserId = other.PlatformUserId;
		}
	}
}
