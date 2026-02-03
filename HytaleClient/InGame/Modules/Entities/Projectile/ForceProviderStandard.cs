using System;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x0200095D RID: 2397
	internal abstract class ForceProviderStandard : ForceProvider
	{
		// Token: 0x06004AE8 RID: 19176
		public abstract float GetMass(float volume);

		// Token: 0x06004AE9 RID: 19177
		public abstract float GetVolume();

		// Token: 0x06004AEA RID: 19178
		public abstract float GetDensity();

		// Token: 0x06004AEB RID: 19179
		public abstract float GetProjectedArea(PhysicsBodyState bodyState, float speed);

		// Token: 0x06004AEC RID: 19180
		public abstract float GetFrictionCoefficient();

		// Token: 0x06004AED RID: 19181
		public abstract ForceProviderStandardState GetForceProviderStandardState();

		// Token: 0x06004AEE RID: 19182 RVA: 0x001320EC File Offset: 0x001302EC
		public void Update(PhysicsBodyState bodyState, ForceAccumulator accumulator, bool onGround)
		{
			ForceProviderStandardState forceProviderStandardState = this.GetForceProviderStandardState();
			Vector3 externalForce = forceProviderStandardState.ExternalForce;
			float y = externalForce.Y;
			accumulator.Force += externalForce;
			float speed = accumulator.Speed;
			float num = forceProviderStandardState.DragCoefficient * this.GetProjectedArea(bodyState, speed) * speed;
			this._dragForce = bodyState.Velocity * -num;
			this.ClipForce(ref this._dragForce, accumulator.ResistanceForceLimit);
			accumulator.Force += this._dragForce;
			float num2 = -forceProviderStandardState.Gravity * this.GetMass(this.GetVolume());
			if (onGround)
			{
				float num3 = (num2 + y) * this.GetFrictionCoefficient();
				bool flag = speed > 0f && num3 > 0f;
				if (flag)
				{
					num3 /= speed;
					accumulator.Force.X = accumulator.Force.X - bodyState.Velocity.X * num3;
					accumulator.Force.Z = accumulator.Force.Z - bodyState.Velocity.Z * num3;
				}
			}
			else
			{
				accumulator.Force.Y = accumulator.Force.Y + num2;
			}
			bool flag2 = forceProviderStandardState.DisplacedMass != 0f;
			if (flag2)
			{
				accumulator.Force.Y = accumulator.Force.Y + forceProviderStandardState.DisplacedMass * forceProviderStandardState.Gravity;
			}
		}

		// Token: 0x06004AEF RID: 19183 RVA: 0x0013224C File Offset: 0x0013044C
		public void ClipForce(ref Vector3 value, Vector3 threshold)
		{
			bool flag = threshold.X < 0f;
			if (flag)
			{
				bool flag2 = value.X < threshold.X;
				if (flag2)
				{
					value.X = threshold.X;
				}
			}
			else
			{
				bool flag3 = value.X > threshold.X;
				if (flag3)
				{
					value.X = threshold.X;
				}
			}
			bool flag4 = threshold.Y < 0f;
			if (flag4)
			{
				bool flag5 = value.Y < threshold.Y;
				if (flag5)
				{
					value.Y = threshold.Y;
				}
			}
			else
			{
				bool flag6 = value.Y > threshold.Y;
				if (flag6)
				{
					value.Y = threshold.Y;
				}
			}
			bool flag7 = threshold.Z < 0f;
			if (flag7)
			{
				bool flag8 = value.Z < threshold.Z;
				if (flag8)
				{
					value.Z = threshold.Z;
				}
			}
			else
			{
				bool flag9 = value.Z > threshold.Z;
				if (flag9)
				{
					value.Z = threshold.Z;
				}
			}
		}

		// Token: 0x04002692 RID: 9874
		protected Vector3 _dragForce = default(Vector3);
	}
}
