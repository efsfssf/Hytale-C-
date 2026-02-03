using System;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Particles
{
	// Token: 0x02000AB1 RID: 2737
	internal struct ParticleAttractor
	{
		// Token: 0x06005607 RID: 22023 RVA: 0x0019A64C File Offset: 0x0019884C
		public void Apply(Vector3 position, Vector3 offsetPosition, ref Vector3 acceleration, ref Vector3 impulse)
		{
			offsetPosition = this.Position + offsetPosition * this.TrailPositionMultiplier;
			Vector3 vector = Vector3.Zero;
			bool flag = this.RadialAxis != Vector3.Zero;
			float num;
			if (flag)
			{
				Vector3 value = this.RadialAxis * Vector3.Dot(this.RadialAxis, position - offsetPosition);
				vector = offsetPosition + value;
				num = Vector3.Distance(vector, position);
			}
			else
			{
				num = Vector3.Distance(offsetPosition, position);
			}
			bool flag2 = this.Radius != 0f && num > this.Radius;
			if (!flag2)
			{
				acceleration *= this.DampingMultiplier;
				bool flag3 = num != 0f && (this.RadialAcceleration != 0f || this.RadialTangentAcceleration != 0f || this.RadialImpulse != 0f || this.RadialTangentImpulse != 0f);
				if (flag3)
				{
					Vector3 vector2 = Vector3.Zero;
					Vector3 vector3 = Vector3.Zero;
					bool flag4 = this.RadialAxis != Vector3.Zero;
					if (flag4)
					{
						vector2 = Vector3.Normalize(vector - position);
						vector3 = Vector3.Cross(this.RadialAxis, vector2);
					}
					else
					{
						vector2 = Vector3.Normalize(offsetPosition - position);
						Quaternion rotation = Quaternion.CreateFromVectors(Vector3.Forward, offsetPosition - position);
						Vector3 vector4 = Vector3.Transform(Vector3.UnitY, rotation);
						vector4.Normalize();
						vector3 = Vector3.Cross(vector4, vector2);
					}
					bool flag5 = vector3 != Vector3.Zero;
					if (flag5)
					{
						vector3.Normalize();
					}
					acceleration += vector2 * -this.RadialAcceleration + vector3 * this.RadialTangentAcceleration;
					impulse += vector2 * -this.RadialImpulse + vector3 * this.RadialTangentImpulse;
				}
				acceleration += this.LinearAcceleration;
				impulse += this.LinearImpulse;
			}
		}

		// Token: 0x04003330 RID: 13104
		public Vector3 Position;

		// Token: 0x04003331 RID: 13105
		public float TrailPositionMultiplier;

		// Token: 0x04003332 RID: 13106
		public Vector3 RadialAxis;

		// Token: 0x04003333 RID: 13107
		public Vector3 DampingMultiplier;

		// Token: 0x04003334 RID: 13108
		public float Radius;

		// Token: 0x04003335 RID: 13109
		public float RadialAcceleration;

		// Token: 0x04003336 RID: 13110
		public float RadialTangentAcceleration;

		// Token: 0x04003337 RID: 13111
		public float RadialImpulse;

		// Token: 0x04003338 RID: 13112
		public float RadialTangentImpulse;

		// Token: 0x04003339 RID: 13113
		public Vector3 LinearAcceleration;

		// Token: 0x0400333A RID: 13114
		public Vector3 LinearImpulse;
	}
}
