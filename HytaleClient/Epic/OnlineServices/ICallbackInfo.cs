using System;

namespace Epic.OnlineServices
{
	// Token: 0x0200000B RID: 11
	internal interface ICallbackInfo
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000080 RID: 128
		object ClientData { get; }

		// Token: 0x06000081 RID: 129
		Result? GetResultCode();
	}
}
