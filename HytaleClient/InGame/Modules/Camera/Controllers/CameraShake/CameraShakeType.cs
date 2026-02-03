using System;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules.Camera.Controllers.CameraShake
{
	// Token: 0x0200097A RID: 2426
	public class CameraShakeType
	{
		// Token: 0x06004CCC RID: 19660 RVA: 0x001475F0 File Offset: 0x001457F0
		public CameraShakeType(CameraShakeConfig config) : this(config.Duration, config.StartTime, config.Continuous, config.EaseIn, config.EaseOut, config.Offset, config.Rotation)
		{
		}

		// Token: 0x06004CCD RID: 19661 RVA: 0x00147624 File Offset: 0x00145824
		public CameraShakeType(float duration, float startTime, bool continuous, EasingConfig easeIn, EasingConfig easeOut, CameraShakeConfig.OffsetNoise offset, CameraShakeConfig.RotationNoise rotation)
		{
			this.Duration = duration;
			this.StartTime = startTime;
			this.Continuous = continuous;
			this.EaseIn = ((easeIn != null) ? easeIn.Time : 0f);
			this.EaseOut = ((easeOut != null) ? easeOut.Time : 0f);
			this.EaseInType = ((easeIn != null) ? easeIn.Type : 0);
			this.EaseOutType = ((easeOut != null) ? easeOut.Type : 0);
			this.Offset = CameraShakeType.CreateVecNoise(offset.X, offset.Y, offset.Z);
			this.Rotation = CameraShakeType.CreateVecNoise(rotation.Pitch, rotation.Yaw, rotation.Roll);
		}

		// Token: 0x06004CCE RID: 19662 RVA: 0x001476E8 File Offset: 0x001458E8
		private static CameraShakeType.Vec3Noise CreateVecNoise(NoiseConfig[] x, NoiseConfig[] y, NoiseConfig[] z)
		{
			Noise1 noiseX = Noise1Combiner.Summed(Noise1Helper.CreateNoises(x));
			Noise1 noiseY = Noise1Combiner.Summed(Noise1Helper.CreateNoises(y));
			Noise1 noiseZ = Noise1Combiner.Summed(Noise1Helper.CreateNoises(z));
			return new CameraShakeType.Vec3Noise(noiseX, noiseY, noiseZ);
		}

		// Token: 0x04002840 RID: 10304
		public static readonly CameraShakeType None = new CameraShakeType(0f, 0f, true, null, null, new CameraShakeConfig.OffsetNoise(), new CameraShakeConfig.RotationNoise());

		// Token: 0x04002841 RID: 10305
		public readonly float Duration;

		// Token: 0x04002842 RID: 10306
		public readonly float StartTime;

		// Token: 0x04002843 RID: 10307
		public readonly float EaseIn;

		// Token: 0x04002844 RID: 10308
		public readonly float EaseOut;

		// Token: 0x04002845 RID: 10309
		public readonly bool Continuous;

		// Token: 0x04002846 RID: 10310
		public readonly Easing.EasingType EaseInType;

		// Token: 0x04002847 RID: 10311
		public readonly Easing.EasingType EaseOutType;

		// Token: 0x04002848 RID: 10312
		public readonly CameraShakeType.Vec3Noise Offset;

		// Token: 0x04002849 RID: 10313
		public readonly CameraShakeType.Vec3Noise Rotation;

		// Token: 0x02000E66 RID: 3686
		public class Vec3Noise
		{
			// Token: 0x060067A0 RID: 26528 RVA: 0x0021838B File Offset: 0x0021658B
			public Vec3Noise(Noise1 noiseX, Noise1 noiseY, Noise1 noiseZ)
			{
				this._noiseX = noiseX;
				this._noiseY = noiseY;
				this._noiseZ = noiseZ;
			}

			// Token: 0x060067A1 RID: 26529 RVA: 0x002183AC File Offset: 0x002165AC
			public Vector3 Eval(int seed, float t)
			{
				float x = this._noiseX.Eval(seed, t);
				float y = this._noiseY.Eval(seed, t);
				float z = this._noiseZ.Eval(seed, t);
				return new Vector3(x, y, z);
			}

			// Token: 0x04004647 RID: 17991
			private readonly Noise1 _noiseX;

			// Token: 0x04004648 RID: 17992
			private readonly Noise1 _noiseY;

			// Token: 0x04004649 RID: 17993
			private readonly Noise1 _noiseZ;
		}
	}
}
