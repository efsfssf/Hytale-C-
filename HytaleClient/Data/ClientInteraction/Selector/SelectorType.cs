using System;
using System.Collections.Generic;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Math;

namespace HytaleClient.Data.ClientInteraction.Selector
{
	// Token: 0x02000B20 RID: 2848
	internal abstract class SelectorType
	{
		// Token: 0x060058DE RID: 22750
		public abstract Selector NewSelector(Random random);

		// Token: 0x060058DF RID: 22751 RVA: 0x001B2E74 File Offset: 0x001B1074
		internal static void SelectNearbyEntities(GameInstance gameInstance, Entity attacker, float range, Action<Entity> consumer, Predicate<Entity> filter)
		{
			Vector3 position = attacker.Position;
			position.Y += attacker.EyeOffset;
			SelectorType.SelectNearbyEntities(gameInstance, attacker, position, range, consumer, filter);
		}

		// Token: 0x060058E0 RID: 22752 RVA: 0x001B2EA8 File Offset: 0x001B10A8
		internal static void SelectNearbyEntities(GameInstance gameInstance, Entity attacker, Vector3 pos, float range, Action<Entity> consumer, Predicate<Entity> filter)
		{
			List<Entity> entitiesInSphere = gameInstance.EntityStoreModule.GetEntitiesInSphere(pos, range);
			foreach (Entity entity in entitiesInSphere)
			{
				bool flag = entity.NetworkId == attacker.NetworkId || entity.IsDead(true) || !entity.IsTangible() || entity.PredictionId != null;
				if (!flag)
				{
					bool flag2 = filter != null && !filter(entity);
					if (!flag2)
					{
						consumer(entity);
					}
				}
			}
		}

		// Token: 0x060058E1 RID: 22753 RVA: 0x001B2F5C File Offset: 0x001B115C
		internal static Vector3 GenerateDebugColor()
		{
			Random random = new Random();
			return new Vector3(random.NextFloat(0f, 1f), random.NextFloat(0f, 1f), random.NextFloat(0f, 1f));
		}
	}
}
