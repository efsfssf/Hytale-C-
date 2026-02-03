using System;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x0200032C RID: 812
	public enum WriteResult
	{
		// Token: 0x040009B4 RID: 2484
		ContinueWriting = 1,
		// Token: 0x040009B5 RID: 2485
		CompleteRequest,
		// Token: 0x040009B6 RID: 2486
		FailRequest,
		// Token: 0x040009B7 RID: 2487
		CancelRequest
	}
}
