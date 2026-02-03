using System;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x02000717 RID: 1815
	[Flags]
	public enum PlatformFlags : ulong
	{
		// Token: 0x040014EC RID: 5356
		None = 0UL,
		// Token: 0x040014ED RID: 5357
		LoadingInEditor = 1UL,
		// Token: 0x040014EE RID: 5358
		DisableOverlay = 2UL,
		// Token: 0x040014EF RID: 5359
		DisableSocialOverlay = 4UL,
		// Token: 0x040014F0 RID: 5360
		Reserved1 = 8UL,
		// Token: 0x040014F1 RID: 5361
		WindowsEnableOverlayD3D9 = 16UL,
		// Token: 0x040014F2 RID: 5362
		WindowsEnableOverlayD3D10 = 32UL,
		// Token: 0x040014F3 RID: 5363
		WindowsEnableOverlayOpengl = 64UL,
		// Token: 0x040014F4 RID: 5364
		ConsoleEnableOverlayAutomaticUnloading = 128UL
	}
}
