using System;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x02000963 RID: 2403
	internal class PhysicsBodyStateUpdaterSymplecticEuler : PhysicsBodyStateUpdater
	{
		// Token: 0x06004B0D RID: 19213 RVA: 0x00133067 File Offset: 0x00131267
		public override void Update(PhysicsBodyState before, PhysicsBodyState after, float mass, float dt, bool onGround, ForceProvider[] forceProvider)
		{
			base.ComputeAcceleration(before, onGround, forceProvider, mass, dt);
			base.UpdateAndClampVelocity(before, after, dt);
			PhysicsBodyStateUpdater.UpdatePositionAfterVelocity(before, after, dt);
		}
	}
}
