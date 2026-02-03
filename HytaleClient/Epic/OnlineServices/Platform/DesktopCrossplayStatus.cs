using System;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x0200070B RID: 1803
	public enum DesktopCrossplayStatus
	{
		// Token: 0x04001496 RID: 5270
		Ok,
		// Token: 0x04001497 RID: 5271
		ApplicationNotBootstrapped,
		// Token: 0x04001498 RID: 5272
		ServiceNotInstalled,
		// Token: 0x04001499 RID: 5273
		ServiceStartFailed,
		// Token: 0x0400149A RID: 5274
		ServiceNotRunning,
		// Token: 0x0400149B RID: 5275
		OverlayDisabled,
		// Token: 0x0400149C RID: 5276
		OverlayNotInstalled,
		// Token: 0x0400149D RID: 5277
		OverlayTrustCheckFailed,
		// Token: 0x0400149E RID: 5278
		OverlayLoadFailed
	}
}
