using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Entities;

namespace HytaleClient.Data.ClientInteraction.Selector
{
	// Token: 0x02000B21 RID: 2849
	internal interface Selector
	{
		// Token: 0x060058E3 RID: 22755
		void Tick(GameInstance gameInstance, Entity attacker, float time, float runTime);

		// Token: 0x060058E4 RID: 22756
		void SelectTargetEntities(GameInstance gameInstance, Entity attacker, EntityHitConsumer consumer, Predicate<Entity> filter);
	}
}
