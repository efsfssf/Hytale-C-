using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000208 RID: 520
	public struct AudioDevicesChangedCallbackInfo : ICallbackInfo
	{
		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06000F0B RID: 3851 RVA: 0x000161C9 File Offset: 0x000143C9
		// (set) Token: 0x06000F0C RID: 3852 RVA: 0x000161D1 File Offset: 0x000143D1
		public object ClientData { get; set; }

		// Token: 0x06000F0D RID: 3853 RVA: 0x000161DC File Offset: 0x000143DC
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x000161F7 File Offset: 0x000143F7
		internal void Set(ref AudioDevicesChangedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
		}
	}
}
