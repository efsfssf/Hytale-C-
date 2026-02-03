using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000232 RID: 562
	public struct OnQueryInputDevicesInformationCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06000FD3 RID: 4051 RVA: 0x00016EF1 File Offset: 0x000150F1
		// (set) Token: 0x06000FD4 RID: 4052 RVA: 0x00016EF9 File Offset: 0x000150F9
		public Result ResultCode { get; set; }

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06000FD5 RID: 4053 RVA: 0x00016F02 File Offset: 0x00015102
		// (set) Token: 0x06000FD6 RID: 4054 RVA: 0x00016F0A File Offset: 0x0001510A
		public object ClientData { get; set; }

		// Token: 0x06000FD7 RID: 4055 RVA: 0x00016F14 File Offset: 0x00015114
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x00016F31 File Offset: 0x00015131
		internal void Set(ref OnQueryInputDevicesInformationCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
		}
	}
}
