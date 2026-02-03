using System;
using HytaleClient.Graphics;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Selector
{
	// Token: 0x02000B23 RID: 2851
	internal class StabSelector : SelectorType
	{
		// Token: 0x060058E9 RID: 22761 RVA: 0x001B2FB2 File Offset: 0x001B11B2
		public StabSelector(StabSelector selector)
		{
			this._selector = selector;
		}

		// Token: 0x060058EA RID: 22762 RVA: 0x001B2FC4 File Offset: 0x001B11C4
		public override Selector NewSelector(Random random)
		{
			return new StabSelector.Runtime(random, this._selector);
		}

		// Token: 0x0400374E RID: 14158
		private StabSelector _selector;

		// Token: 0x02000F33 RID: 3891
		private class Runtime : Selector
		{
			// Token: 0x0600688A RID: 26762 RVA: 0x0021B9B0 File Offset: 0x00219BB0
			public Runtime(Random random, StabSelector selector)
			{
				this._executor = new HitDetectionExecutor(random);
				this._selector = selector;
				bool testLineOfSight = this._selector.TestLineOfSight;
				if (testLineOfSight)
				{
					this._executor.LosProvider = HitDetectionExecutor.DefaultLineOfSightSolid;
				}
				else
				{
					this._executor.LosProvider = HitDetectionExecutor.DefaultLineOfSightTrue;
				}
			}

			// Token: 0x0600688B RID: 26763 RVA: 0x0021BA28 File Offset: 0x00219C28
			public void Tick(GameInstance gameInstance, Entity attacker, float time, float runTime)
			{
				float eyeOffset = attacker.EyeOffset;
				Vector3 position = attacker.Position;
				float x = position.X;
				float num = position.Y + eyeOffset;
				float z = position.Z;
				float num2 = time - this._lastTime;
				this._lastTime = time;
				float num3 = num2 / runTime;
				float num4 = this._selector.EndDistance - this._selector.StartDistance;
				float num5 = this._runTimeDeltaPercentageSum * num4 + this._selector.StartDistance;
				float num6 = (this._runTimeDeltaPercentageSum + num3) * num4 + this._selector.StartDistance;
				Matrix matrix = Matrix.CreateRotationZ(-this._selector.RollOffset) * Matrix.CreateRotationY(-this._selector.YawOffset) * Matrix.CreateRotationX(-this._selector.PitchOffset);
				float near = num5;
				float far = num6;
				this._projectionMatrix = matrix * Matrix.CreateProjectionOrtho(this._selector.ExtendLeft, this._selector.ExtendRight, this._selector.ExtendBottom, this._selector.ExtendTop, near, far);
				Vector3 vector = Vector3.CreateFromYawPitch(attacker.LookOrientation.Yaw, attacker.LookOrientation.Pitch);
				this._viewMatrix = Matrix.CreateViewDirection(x, num, z, vector.X, vector.Y, vector.Z, 0f, 1f, 0f);
				this._executor.SetOrigin(new Vector3(x, num, z));
				this._executor.ProjectionMatrix = this._projectionMatrix;
				this._executor.ViewMatrix = this._viewMatrix;
				bool showSelectorDebug = gameInstance.InteractionModule.ShowSelectorDebug;
				if (showSelectorDebug)
				{
					this._projectionFrustum.Matrix = this._projectionMatrix;
					Mesh mesh = default(Mesh);
					MeshProcessor.CreateFrustum(ref mesh, ref this._projectionFrustum);
					Vector3 vector2 = this._debugColor ?? SelectorType.GenerateDebugColor();
					this._debugColor = new Vector3?(vector2);
					Matrix matrix2 = Matrix.CreateRotationX(attacker.LookOrientation.Pitch) * Matrix.CreateRotationY(attacker.LookOrientation.Yaw) * Matrix.CreateTranslation(x, num, z);
					gameInstance.InteractionModule.SelectorDebugMeshes.Add(new InteractionModule.DebugSelectorMesh(matrix2, mesh, 5f, vector2));
				}
				this._runTimeDeltaPercentageSum += num3;
			}

			// Token: 0x0600688C RID: 26764 RVA: 0x0021BC9C File Offset: 0x00219E9C
			public void SelectTargetEntities(GameInstance gameInstance, Entity attacker, EntityHitConsumer consumer, Predicate<Entity> filter)
			{
				SelectorType.SelectNearbyEntities(gameInstance, attacker, this._selector.EndDistance + 1f, delegate(Entity entity)
				{
					BoundingBox hitbox = entity.Hitbox;
					Matrix modelMatrix = Matrix.CreateScale(hitbox.GetSize()) * Matrix.CreateTranslation(hitbox.Min) * Matrix.CreateTranslation(entity.Position);
					bool flag = this._executor.Test(gameInstance, HitDetectionExecutor.CUBE_QUADS, modelMatrix);
					if (flag)
					{
						consumer(entity, this._executor.GetHitLocation());
					}
				}, filter);
			}

			// Token: 0x04004A64 RID: 19044
			private readonly StabSelector _selector;

			// Token: 0x04004A65 RID: 19045
			private Matrix _projectionMatrix;

			// Token: 0x04004A66 RID: 19046
			private BoundingFrustum _projectionFrustum = new BoundingFrustum(Matrix.Identity);

			// Token: 0x04004A67 RID: 19047
			private Matrix _viewMatrix;

			// Token: 0x04004A68 RID: 19048
			private readonly HitDetectionExecutor _executor;

			// Token: 0x04004A69 RID: 19049
			private float _lastTime = 0f;

			// Token: 0x04004A6A RID: 19050
			private float _runTimeDeltaPercentageSum;

			// Token: 0x04004A6B RID: 19051
			private Vector3? _debugColor;
		}
	}
}
