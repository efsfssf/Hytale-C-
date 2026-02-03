using System;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x0200095E RID: 2398
	internal class ForceProviderStandardState
	{
		// Token: 0x06004AF1 RID: 19185 RVA: 0x00132374 File Offset: 0x00130574
		public ForceProviderStandardState()
		{
			this.NextTickVelocity = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		}

		// Token: 0x06004AF2 RID: 19186 RVA: 0x001323E0 File Offset: 0x001305E0
		public void ConvertToForces(float dt, float mass)
		{
			this.ExternalForce += this.ExternalAcceleration * (1f / mass);
			this.ExternalForce += this.ExternalImpulse * (1f / dt);
			this.ExternalAcceleration = Vector3.Zero;
			this.ExternalImpulse = Vector3.Zero;
		}

		// Token: 0x06004AF3 RID: 19187 RVA: 0x0013244C File Offset: 0x0013064C
		public void UpdateVelocity(ref Vector3 velocity)
		{
			bool flag = this.NextTickVelocity.X < float.MaxValue;
			if (flag)
			{
				velocity = this.NextTickVelocity;
				this.NextTickVelocity = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			}
			velocity += this.ExternalVelocity;
			this.ExternalVelocity = Vector3.Zero;
		}

		// Token: 0x06004AF4 RID: 19188 RVA: 0x001324BA File Offset: 0x001306BA
		public void Clear()
		{
			this.ExternalForce = Vector3.Zero;
		}

		// Token: 0x04002693 RID: 9875
		public float DisplacedMass;

		// Token: 0x04002694 RID: 9876
		public float DragCoefficient;

		// Token: 0x04002695 RID: 9877
		public float Gravity;

		// Token: 0x04002696 RID: 9878
		public Vector3 NextTickVelocity = default(Vector3);

		// Token: 0x04002697 RID: 9879
		public Vector3 ExternalVelocity = default(Vector3);

		// Token: 0x04002698 RID: 9880
		public Vector3 ExternalAcceleration = default(Vector3);

		// Token: 0x04002699 RID: 9881
		public Vector3 ExternalForce = default(Vector3);

		// Token: 0x0400269A RID: 9882
		public Vector3 ExternalImpulse = default(Vector3);
	}
}
