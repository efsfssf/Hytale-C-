using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Selector
{
	// Token: 0x02000B1F RID: 2847
	internal class RaycastSelector : SelectorType
	{
		// Token: 0x060058DC RID: 22748 RVA: 0x001B2E42 File Offset: 0x001B1042
		public RaycastSelector(RaycastSelector selector)
		{
			this._selector = selector;
		}

		// Token: 0x060058DD RID: 22749 RVA: 0x001B2E54 File Offset: 0x001B1054
		public override Selector NewSelector(Random random)
		{
			return new RaycastSelector.Runtime(this._selector);
		}

		// Token: 0x0400374C RID: 14156
		private const int MaxDistance = 30;

		// Token: 0x0400374D RID: 14157
		private RaycastSelector _selector;

		// Token: 0x02000F32 RID: 3890
		private class Runtime : Selector
		{
			// Token: 0x06006886 RID: 26758 RVA: 0x0021B809 File Offset: 0x00219A09
			public Runtime(RaycastSelector selector)
			{
				this._selector = selector;
			}

			// Token: 0x06006887 RID: 26759 RVA: 0x0021B81C File Offset: 0x00219A1C
			public void Tick(GameInstance gameInstance, Entity attacker, float time, float runTime)
			{
				Vector3 origin = this.SelectTargetPosition(attacker);
				Vector3 lookOrientation = attacker.LookOrientation;
				Quaternion rotation = Quaternion.CreateFromYawPitchRoll(lookOrientation.Yaw, lookOrientation.Pitch, 0f);
				Vector3 direction = Vector3.Transform(Vector3.Forward, rotation);
				bool flag;
				HitDetection.RaycastHit raycastHit;
				gameInstance.HitDetection.Raycast(origin, direction, new HitDetection.RaycastOptions
				{
					Distance = (float)this._selector.Distance,
					RequiredBlockTag = this._selector.BlockTagIndex,
					IgnoreFluids = this._selector.IgnoreFluids,
					IgnoreEmptyCollisionMaterial = this._selector.IgnoreEmptyCollisionMaterial
				}, out flag, out raycastHit, out this._hasEntity, out this._entityHitData);
			}

			// Token: 0x06006888 RID: 26760 RVA: 0x0021B8C8 File Offset: 0x00219AC8
			public void SelectTargetEntities(GameInstance gameInstance, Entity attacker, EntityHitConsumer consumer, Predicate<Entity> filter)
			{
				bool hasEntity = this._hasEntity;
				if (hasEntity)
				{
					consumer(this._entityHitData.Entity, new Vector4(this._entityHitData.RayBoxCollision.Position));
				}
			}

			// Token: 0x06006889 RID: 26761 RVA: 0x0021B90C File Offset: 0x00219B0C
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

			// Token: 0x04004A60 RID: 19040
			private readonly RaycastSelector _selector;

			// Token: 0x04004A61 RID: 19041
			private Vector3? _debugColor;

			// Token: 0x04004A62 RID: 19042
			private bool _hasEntity;

			// Token: 0x04004A63 RID: 19043
			private HitDetection.EntityHitData _entityHitData;
		}
	}
}
