using System;
using HytaleClient.Graphics;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Selector
{
	// Token: 0x02000B18 RID: 2840
	internal class AOECylinderSelector : SelectorType
	{
		// Token: 0x060058C3 RID: 22723 RVA: 0x001B23B5 File Offset: 0x001B05B5
		public AOECylinderSelector(AOECylinderSelector selector)
		{
			this._selector = selector;
		}

		// Token: 0x060058C4 RID: 22724 RVA: 0x001B23C8 File Offset: 0x001B05C8
		public override Selector NewSelector(Random random)
		{
			return new AOECylinderSelector.Runtime(this._selector);
		}

		// Token: 0x04003731 RID: 14129
		private AOECylinderSelector _selector;

		// Token: 0x02000F2F RID: 3887
		private class Runtime : Selector
		{
			// Token: 0x0600687B RID: 26747 RVA: 0x0021B1C7 File Offset: 0x002193C7
			public Runtime(AOECylinderSelector selector)
			{
				this._selector = selector;
			}

			// Token: 0x0600687C RID: 26748 RVA: 0x0021B1D8 File Offset: 0x002193D8
			public void Tick(GameInstance gameInstance, Entity attacker, float time, float runTime)
			{
				bool flag = !gameInstance.InteractionModule.ShowSelectorDebug;
				if (!flag)
				{
					Vector3 vector = this.SelectTargetPosition(attacker);
					float x = vector.X;
					float yPosition = vector.Y + this._selector.Height / 2f;
					float z = vector.Z;
					Mesh mesh = default(Mesh);
					MeshProcessor.CreateSphere(ref mesh, 5, 8, this._selector.Range, 0, -1, -1);
					Vector3 vector2 = this._debugColor ?? SelectorType.GenerateDebugColor();
					this._debugColor = new Vector3?(vector2);
					Matrix matrix = Matrix.CreateTranslation(x, yPosition, z);
					gameInstance.InteractionModule.SelectorDebugMeshes.Add(new InteractionModule.DebugSelectorMesh(matrix, mesh, 5f, vector2));
				}
			}

			// Token: 0x0600687D RID: 26749 RVA: 0x0021B2A8 File Offset: 0x002194A8
			public void SelectTargetEntities(GameInstance gameInstance, Entity attacker, EntityHitConsumer consumer, Predicate<Entity> filter)
			{
				Vector3 position = this.SelectTargetPosition(attacker);
				SelectorType.SelectNearbyEntities(gameInstance, attacker, position, this._selector.Range, delegate(Entity entity)
				{
					float num = entity.Position.Y - position.Y;
					bool flag = num < 0f || num > this._selector.Height;
					if (!flag)
					{
						consumer(entity, new Vector4(entity.Position, 1f));
					}
				}, filter);
			}

			// Token: 0x0600687E RID: 26750 RVA: 0x0021B300 File Offset: 0x00219500
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

			// Token: 0x04004A55 RID: 19029
			private readonly AOECylinderSelector _selector;

			// Token: 0x04004A56 RID: 19030
			private Vector3? _debugColor;
		}
	}
}
