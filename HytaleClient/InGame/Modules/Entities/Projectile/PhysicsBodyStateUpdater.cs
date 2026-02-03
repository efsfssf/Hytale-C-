using System;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x02000962 RID: 2402
	internal class PhysicsBodyStateUpdater
	{
		// Token: 0x06004B01 RID: 19201 RVA: 0x00132EDF File Offset: 0x001310DF
		public virtual void Update(PhysicsBodyState before, PhysicsBodyState after, float mass, float dt, bool onGround, ForceProvider[] forceProvider)
		{
			this.ComputeAcceleration(before, onGround, forceProvider, mass, dt);
			PhysicsBodyStateUpdater.UpdatePositionBeforeVelocity(before, after, dt);
			this.UpdateAndClampVelocity(before, after, dt);
		}

		// Token: 0x06004B02 RID: 19202 RVA: 0x00132F06 File Offset: 0x00131106
		protected static void UpdatePositionBeforeVelocity(PhysicsBodyState before, PhysicsBodyState after, float dt)
		{
			after.Position = before.Position + before.Velocity * dt;
		}

		// Token: 0x06004B03 RID: 19203 RVA: 0x00132F26 File Offset: 0x00131126
		protected static void UpdatePositionAfterVelocity(PhysicsBodyState before, PhysicsBodyState after, float dt)
		{
			after.Position = before.Position + after.Velocity * dt;
		}

		// Token: 0x06004B04 RID: 19204 RVA: 0x00132F46 File Offset: 0x00131146
		protected void UpdateAndClampVelocity(PhysicsBodyState before, PhysicsBodyState after, float dt)
		{
			this.UpdateVelocity(before, after, dt);
			after.Velocity = after.Velocity.ClipToZero(1E-06f);
		}

		// Token: 0x06004B05 RID: 19205 RVA: 0x00132F69 File Offset: 0x00131169
		protected void UpdateVelocity(PhysicsBodyState before, PhysicsBodyState after, float dt)
		{
			after.Velocity = before.Velocity + this._acceleration * dt;
		}

		// Token: 0x06004B06 RID: 19206 RVA: 0x00132F89 File Offset: 0x00131189
		protected void ComputeAcceleration(float mass)
		{
			this._acceleration = this._accumulator.Force * (1f / mass);
		}

		// Token: 0x06004B07 RID: 19207 RVA: 0x00132FA9 File Offset: 0x001311A9
		protected void ComputeAcceleration(PhysicsBodyState state, bool onGround, ForceProvider[] forceProviders, float mass, float timeStep)
		{
			this._accumulator.ComputeResultingForce(state, onGround, forceProviders, mass, timeStep);
			this.ComputeAcceleration(mass);
		}

		// Token: 0x06004B08 RID: 19208 RVA: 0x00132FC8 File Offset: 0x001311C8
		protected void AssignAcceleration(PhysicsBodyState state)
		{
			state.Velocity = this._acceleration;
		}

		// Token: 0x06004B09 RID: 19209 RVA: 0x00132FD7 File Offset: 0x001311D7
		protected void AddAcceleration(PhysicsBodyState state, float scale)
		{
			state.Velocity += this._acceleration * scale;
		}

		// Token: 0x06004B0A RID: 19210 RVA: 0x00132FF7 File Offset: 0x001311F7
		protected void AddAcceleration(PhysicsBodyState state)
		{
			state.Velocity += this._acceleration;
		}

		// Token: 0x06004B0B RID: 19211 RVA: 0x00133011 File Offset: 0x00131211
		protected void ConvertAccelerationToVelocity(PhysicsBodyState before, PhysicsBodyState after, float scale)
		{
			after.Velocity = after.Velocity * scale + before.Velocity;
			after.Velocity = after.Velocity.ClipToZero(1E-06f);
		}

		// Token: 0x040026A7 RID: 9895
		protected const float MinVelocity = 1E-06f;

		// Token: 0x040026A8 RID: 9896
		protected Vector3 _acceleration = default(Vector3);

		// Token: 0x040026A9 RID: 9897
		protected readonly ForceAccumulator _accumulator = new ForceAccumulator();
	}
}
