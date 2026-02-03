using System;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules.Interaction
{
	// Token: 0x0200093B RID: 2363
	public interface InteractionSource
	{
		// Token: 0x060048BC RID: 18620
		bool TryGetInteractionId(InteractionType type, out int id);
	}
}
