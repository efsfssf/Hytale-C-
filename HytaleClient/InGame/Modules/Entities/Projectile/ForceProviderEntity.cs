using System;
using HytaleClient.Utils;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x0200095C RID: 2396
	internal class ForceProviderEntity : ForceProviderStandard
	{
		// Token: 0x06004AE1 RID: 19169 RVA: 0x00132008 File Offset: 0x00130208
		public ForceProviderEntity(Entity entity)
		{
			this._entity = entity;
		}

		// Token: 0x06004AE2 RID: 19170 RVA: 0x00132024 File Offset: 0x00130224
		public override ForceProviderStandardState GetForceProviderStandardState()
		{
			return this.ForceProviderStandardState;
		}

		// Token: 0x06004AE3 RID: 19171 RVA: 0x0013203C File Offset: 0x0013023C
		public override float GetMass(float volume)
		{
			return volume * this.GetDensity();
		}

		// Token: 0x06004AE4 RID: 19172 RVA: 0x00132058 File Offset: 0x00130258
		public override float GetVolume()
		{
			return this._entity.Hitbox.GetVolume();
		}

		// Token: 0x06004AE5 RID: 19173 RVA: 0x00132080 File Offset: 0x00130280
		public override float GetProjectedArea(PhysicsBodyState bodyState, float speed)
		{
			float num = PhysicsMath.ComputeProjectedArea(bodyState.Velocity, this._entity.Hitbox);
			return (num == 0f) ? 0f : (num / speed);
		}

		// Token: 0x06004AE6 RID: 19174 RVA: 0x001320BC File Offset: 0x001302BC
		public override float GetDensity()
		{
			return this.Density;
		}

		// Token: 0x06004AE7 RID: 19175 RVA: 0x001320D4 File Offset: 0x001302D4
		public override float GetFrictionCoefficient()
		{
			return 0f;
		}

		// Token: 0x0400268F RID: 9871
		protected Entity _entity;

		// Token: 0x04002690 RID: 9872
		public ForceProviderStandardState ForceProviderStandardState;

		// Token: 0x04002691 RID: 9873
		public float Density = 700f;
	}
}
