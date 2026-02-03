using System;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006E6 RID: 1766
	public struct OnClientIntegrityViolatedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000D91 RID: 3473
		// (get) Token: 0x06002D94 RID: 11668 RVA: 0x00043795 File Offset: 0x00041995
		// (set) Token: 0x06002D95 RID: 11669 RVA: 0x0004379D File Offset: 0x0004199D
		public object ClientData { get; set; }

		// Token: 0x17000D92 RID: 3474
		// (get) Token: 0x06002D96 RID: 11670 RVA: 0x000437A6 File Offset: 0x000419A6
		// (set) Token: 0x06002D97 RID: 11671 RVA: 0x000437AE File Offset: 0x000419AE
		public AntiCheatClientViolationType ViolationType { get; set; }

		// Token: 0x17000D93 RID: 3475
		// (get) Token: 0x06002D98 RID: 11672 RVA: 0x000437B7 File Offset: 0x000419B7
		// (set) Token: 0x06002D99 RID: 11673 RVA: 0x000437BF File Offset: 0x000419BF
		public Utf8String ViolationMessage { get; set; }

		// Token: 0x06002D9A RID: 11674 RVA: 0x000437C8 File Offset: 0x000419C8
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06002D9B RID: 11675 RVA: 0x000437E3 File Offset: 0x000419E3
		internal void Set(ref OnClientIntegrityViolatedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.ViolationType = other.ViolationType;
			this.ViolationMessage = other.ViolationMessage;
		}
	}
}
