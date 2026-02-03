using System;
using HytaleClient.Graphics;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.CharacterController;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Selector
{
	// Token: 0x02000B1E RID: 2846
	internal class HorizontalSelector : SelectorType
	{
		// Token: 0x060058DA RID: 22746 RVA: 0x001B2E13 File Offset: 0x001B1013
		public HorizontalSelector(HorizontalSelector selector)
		{
			this._selector = selector;
		}

		// Token: 0x060058DB RID: 22747 RVA: 0x001B2E24 File Offset: 0x001B1024
		public override Selector NewSelector(Random random)
		{
			return new HorizontalSelector.Runtime(random, this._selector);
		}

		// Token: 0x0400374B RID: 14155
		private HorizontalSelector _selector;

		// Token: 0x02000F31 RID: 3889
		private class Runtime : Selector
		{
			// Token: 0x06006883 RID: 26755 RVA: 0x0021B450 File Offset: 0x00219650
			public Runtime(Random random, HorizontalSelector selector)
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

			// Token: 0x06006884 RID: 26756 RVA: 0x0021B4C8 File Offset: 0x002196C8
			public void Tick(GameInstance gameInstance, Entity attacker, float time, float runTime)
			{
				int num = 1;
				bool flag = this._selector.Direction == 1;
				if (flag)
				{
					num = -1;
				}
				CharacterControllerModule characterControllerModule = gameInstance.CharacterControllerModule;
				float num2 = attacker.EyeOffset + ((gameInstance.LocalPlayer == attacker) ? (characterControllerModule.MovementController.AutoJumpHeightShift + characterControllerModule.MovementController.CrouchHeightShift) : 0f);
				Vector3 position = attacker.Position;
				float x = position.X;
				float num3 = position.Y + num2;
				float z = position.Z;
				float num4 = time - this._lastTime;
				this._lastTime = time;
				float num5 = num4 / runTime;
				float num6 = this._selector.StartDistance / this._selector.EndDistance;
				float num7 = this._selector.YawLength * num5;
				float num8 = this._selector.YawLength * this._runTimeDeltaPercentageSum;
				float num9 = 2f * this._selector.EndDistance * num7 / 3.1415927f;
				float num10 = (num8 + num7 + this._selector.YawStartOffset) * (float)num;
				float num11 = num9 * num6;
				Matrix matrix = Matrix.CreateRotationZ(-this._selector.RollOffset) * Matrix.CreateRotationY(-num10) * Matrix.CreateRotationX(-this._selector.PitchOffset);
				float startDistance = this._selector.StartDistance;
				float endDistance = this._selector.EndDistance;
				this._projectionMatrix = matrix * Matrix.CreateProjectionFrustum(num11, num11, this._selector.ExtendBottom * num6, this._selector.ExtendTop * num6, startDistance, endDistance);
				Vector3 vector = Vector3.CreateFromYawPitch(attacker.LookOrientation.Yaw, attacker.LookOrientation.Pitch);
				this._viewMatrix = Matrix.CreateViewDirection(x, num3, z, vector.X, vector.Y, vector.Z, 0f, 1f, 0f);
				this._executor.SetOrigin(new Vector3(x, num3, z));
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
					Matrix matrix2 = Matrix.CreateRotationX(attacker.LookOrientation.Pitch) * Matrix.CreateRotationY(attacker.LookOrientation.Yaw) * Matrix.CreateTranslation(x, num3, z);
					gameInstance.InteractionModule.SelectorDebugMeshes.Add(new InteractionModule.DebugSelectorMesh(matrix2, mesh, 5f, vector2));
				}
				this._runTimeDeltaPercentageSum += num5;
			}

			// Token: 0x06006885 RID: 26757 RVA: 0x0021B7B4 File Offset: 0x002199B4
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

			// Token: 0x04004A58 RID: 19032
			private readonly HorizontalSelector _selector;

			// Token: 0x04004A59 RID: 19033
			private Matrix _projectionMatrix;

			// Token: 0x04004A5A RID: 19034
			private BoundingFrustum _projectionFrustum = new BoundingFrustum(Matrix.Identity);

			// Token: 0x04004A5B RID: 19035
			private Matrix _viewMatrix;

			// Token: 0x04004A5C RID: 19036
			private readonly HitDetectionExecutor _executor;

			// Token: 0x04004A5D RID: 19037
			private float _lastTime = 0f;

			// Token: 0x04004A5E RID: 19038
			private float _runTimeDeltaPercentageSum;

			// Token: 0x04004A5F RID: 19039
			private Vector3? _debugColor;
		}
	}
}
