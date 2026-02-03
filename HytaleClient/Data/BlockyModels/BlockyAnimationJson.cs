using System;
using System.Collections.Generic;

namespace HytaleClient.Data.BlockyModels
{
	// Token: 0x02000B60 RID: 2912
	public struct BlockyAnimationJson
	{
		// Token: 0x040037F4 RID: 14324
		public int Duration;

		// Token: 0x040037F5 RID: 14325
		public bool HoldLastKeyframe;

		// Token: 0x040037F6 RID: 14326
		public IDictionary<string, AnimationNode> NodeAnimations;
	}
}
