using System;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x0200095A RID: 2394
	public class ForceAccumulator
	{
		// Token: 0x06004ADD RID: 19165 RVA: 0x00131F77 File Offset: 0x00130177
		public void Initialize(PhysicsBodyState state, float mass, float timeStep)
		{
			this.Force = Vector3.Zero;
			this.Speed = state.Velocity.Length();
			this.ResistanceForceLimit = state.Velocity * (-mass / timeStep);
		}

		// Token: 0x06004ADE RID: 19166 RVA: 0x00131FAC File Offset: 0x001301AC
		public void ComputeResultingForce(PhysicsBodyState state, bool onGround, ForceProvider[] forceProviders, float mass, float timeStep)
		{
			this.Initialize(state, mass, timeStep);
			foreach (ForceProvider forceProvider in forceProviders)
			{
				forceProvider.Update(state, this, onGround);
			}
		}

		// Token: 0x0400268C RID: 9868
		public float Speed;

		// Token: 0x0400268D RID: 9869
		public Vector3 Force = default(Vector3);

		// Token: 0x0400268E RID: 9870
		public Vector3 ResistanceForceLimit = default(Vector3);
	}
}
