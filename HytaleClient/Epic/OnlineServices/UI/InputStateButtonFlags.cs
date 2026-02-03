using System;

namespace Epic.OnlineServices.UI
{
	// Token: 0x0200005F RID: 95
	[Flags]
	public enum InputStateButtonFlags
	{
		// Token: 0x040001F0 RID: 496
		None = 0,
		// Token: 0x040001F1 RID: 497
		DPadLeft = 1,
		// Token: 0x040001F2 RID: 498
		DPadRight = 2,
		// Token: 0x040001F3 RID: 499
		DPadDown = 4,
		// Token: 0x040001F4 RID: 500
		DPadUp = 8,
		// Token: 0x040001F5 RID: 501
		FaceButtonLeft = 16,
		// Token: 0x040001F6 RID: 502
		FaceButtonRight = 32,
		// Token: 0x040001F7 RID: 503
		FaceButtonBottom = 64,
		// Token: 0x040001F8 RID: 504
		FaceButtonTop = 128,
		// Token: 0x040001F9 RID: 505
		LeftShoulder = 256,
		// Token: 0x040001FA RID: 506
		RightShoulder = 512,
		// Token: 0x040001FB RID: 507
		LeftTrigger = 1024,
		// Token: 0x040001FC RID: 508
		RightTrigger = 2048,
		// Token: 0x040001FD RID: 509
		SpecialLeft = 4096,
		// Token: 0x040001FE RID: 510
		SpecialRight = 8192,
		// Token: 0x040001FF RID: 511
		LeftThumbstick = 16384,
		// Token: 0x04000200 RID: 512
		RightThumbstick = 32768
	}
}
