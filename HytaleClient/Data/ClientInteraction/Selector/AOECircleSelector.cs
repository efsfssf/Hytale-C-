using System;
using HytaleClient.Graphics;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Selector
{
	// Token: 0x02000B17 RID: 2839
	internal class AOECircleSelector : SelectorType
	{
		// Token: 0x060058C1 RID: 22721 RVA: 0x001B2385 File Offset: 0x001B0585
		public AOECircleSelector(AOECircleSelector selector)
		{
			this._selector = selector;
		}

		// Token: 0x060058C2 RID: 22722 RVA: 0x001B2398 File Offset: 0x001B0598
		public override Selector NewSelector(Random random)
		{
			return new AOECircleSelector.Runtime(this._selector);
		}

		// Token: 0x04003730 RID: 14128
		private AOECircleSelector _selector;

		// Token: 0x02000F2E RID: 3886
		private class Runtime : Selector
		{
			// Token: 0x06006877 RID: 26743 RVA: 0x0021B010 File Offset: 0x00219210
			public Runtime(AOECircleSelector selector)
			{
				this._selector = selector;
			}

			// Token: 0x06006878 RID: 26744 RVA: 0x0021B024 File Offset: 0x00219224
			public void Tick(GameInstance gameInstance, Entity attacker, float time, float runTime)
			{
				Vector3 vector = this.SelectTargetPosition(attacker);
				float x = vector.X;
				float y = vector.Y;
				float z = vector.Z;
				bool showSelectorDebug = gameInstance.InteractionModule.ShowSelectorDebug;
				if (showSelectorDebug)
				{
					Mesh mesh = default(Mesh);
					MeshProcessor.CreateSphere(ref mesh, 5, 8, this._selector.Range, 0, -1, -1);
					Vector3 vector2 = this._debugColor ?? SelectorType.GenerateDebugColor();
					this._debugColor = new Vector3?(vector2);
					Matrix matrix = Matrix.CreateTranslation(x, y, z);
					gameInstance.InteractionModule.SelectorDebugMeshes.Add(new InteractionModule.DebugSelectorMesh(matrix, mesh, 5f, vector2));
				}
			}

			// Token: 0x06006879 RID: 26745 RVA: 0x0021B0DC File Offset: 0x002192DC
			public void SelectTargetEntities(GameInstance gameInstance, Entity attacker, EntityHitConsumer consumer, Predicate<Entity> filter)
			{
				Vector3 pos = this.SelectTargetPosition(attacker);
				SelectorType.SelectNearbyEntities(gameInstance, attacker, pos, this._selector.Range, delegate(Entity entity)
				{
					consumer(entity, new Vector4(entity.Position, 1f));
				}, filter);
			}

			// Token: 0x0600687A RID: 26746 RVA: 0x0021B124 File Offset: 0x00219324
			private Vector3 SelectTargetPosition(Entity attacker)
			{
				Vector3 vector = attacker.Position;
				Vector3f offset = this._selector.Offset;
				bool flag = offset.X != 0f || offset.Y != 0f || offset.Z != 0f;
				if (flag)
				{
					vector = new Vector3(offset.X, offset.Y, offset.Z);
					Quaternion quaternion = Quaternion.CreateFromAxisAngle(Vector3.Up, attacker.LookOrientation.Yaw);
					Vector3.Transform(ref vector, ref quaternion, out vector);
					vector += attacker.Position;
				}
				return vector;
			}

			// Token: 0x04004A53 RID: 19027
			private readonly AOECircleSelector _selector;

			// Token: 0x04004A54 RID: 19028
			private Vector3? _debugColor;
		}
	}
}
