using System;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Particles
{
	// Token: 0x02000AB8 RID: 2744
	internal class ParticleSpawnerSettings
	{
		// Token: 0x040033BC RID: 13244
		public const int DefaultMaxConcurrentParticles = 512;

		// Token: 0x040033BD RID: 13245
		public static readonly Vector2 DefaultParticleLifeSpan = new Vector2(2f, 2f);

		// Token: 0x040033BE RID: 13246
		public static readonly Vector2 DefaultSpawnRate = Vector2.One;

		// Token: 0x040033BF RID: 13247
		public ParticleSettings ParticleSettings;

		// Token: 0x040033C0 RID: 13248
		public FXSystem.RenderMode RenderMode = FXSystem.RenderMode.BlendLinear;

		// Token: 0x040033C1 RID: 13249
		public ParticleFXSystem.ParticleRotationInfluence RotationInfluence = ParticleFXSystem.ParticleRotationInfluence.None;

		// Token: 0x040033C2 RID: 13250
		public ParticleFXSystem.ParticleCollisionBlockType ParticleCollisionBlockType = ParticleFXSystem.ParticleCollisionBlockType.None;

		// Token: 0x040033C3 RID: 13251
		public ParticleFXSystem.ParticleCollisionAction ParticleCollisionAction = ParticleFXSystem.ParticleCollisionAction.Expire;

		// Token: 0x040033C4 RID: 13252
		public ParticleFXSystem.ParticleRotationInfluence ParticleCollisionRotationInfluence = ParticleFXSystem.ParticleRotationInfluence.None;

		// Token: 0x040033C5 RID: 13253
		public float LightInfluence = 0f;

		// Token: 0x040033C6 RID: 13254
		public float TrailSpawnerPositionMultiplier;

		// Token: 0x040033C7 RID: 13255
		public float TrailSpawnerRotationMultiplier;

		// Token: 0x040033C8 RID: 13256
		public bool ParticleRotateWithSpawner = false;

		// Token: 0x040033C9 RID: 13257
		public bool LinearFiltering = false;

		// Token: 0x040033CA RID: 13258
		public bool IsLowRes = false;

		// Token: 0x040033CB RID: 13259
		public ParticleSpawnerSettings.UVMotionParams UVMotion;

		// Token: 0x040033CC RID: 13260
		public ParticleSpawnerSettings.IntersectionHighlightParams IntersectionHighlight;

		// Token: 0x040033CD RID: 13261
		public float CameraOffset;

		// Token: 0x040033CE RID: 13262
		public float VelocityStretchMultiplier;

		// Token: 0x040033CF RID: 13263
		public float LifeSpan = 0f;

		// Token: 0x040033D0 RID: 13264
		public Point TotalParticles = new Point(-1, -1);

		// Token: 0x040033D1 RID: 13265
		public int MaxConcurrentParticles = 512;

		// Token: 0x040033D2 RID: 13266
		public Vector2 ParticleLifeSpan = ParticleSpawnerSettings.DefaultParticleLifeSpan;

		// Token: 0x040033D3 RID: 13267
		public Vector2 SpawnRate = ParticleSpawnerSettings.DefaultSpawnRate;

		// Token: 0x040033D4 RID: 13268
		public bool SpawnBurst = false;

		// Token: 0x040033D5 RID: 13269
		public Vector2 WaveDelay = Vector2.Zero;

		// Token: 0x040033D6 RID: 13270
		public ParticleSpawnerSettings.InitialVelocity InitialVelocityMin;

		// Token: 0x040033D7 RID: 13271
		public ParticleSpawnerSettings.InitialVelocity InitialVelocityMax;

		// Token: 0x040033D8 RID: 13272
		public ParticleSpawnerSettings.Shape EmitShape = ParticleSpawnerSettings.Shape.Sphere;

		// Token: 0x040033D9 RID: 13273
		public Vector3 EmitOffsetMin;

		// Token: 0x040033DA RID: 13274
		public Vector3 EmitOffsetMax;

		// Token: 0x040033DB RID: 13275
		public bool UseEmitDirection;

		// Token: 0x040033DC RID: 13276
		public ParticleAttractor[] Attractors = new ParticleAttractor[0];

		// Token: 0x02000F02 RID: 3842
		public enum Shape
		{
			// Token: 0x04004996 RID: 18838
			Sphere,
			// Token: 0x04004997 RID: 18839
			Cube,
			// Token: 0x04004998 RID: 18840
			Circle,
			// Token: 0x04004999 RID: 18841
			FullCube
		}

		// Token: 0x02000F03 RID: 3843
		public struct InitialVelocity
		{
			// Token: 0x06006826 RID: 26662 RVA: 0x0021A49A File Offset: 0x0021869A
			public InitialVelocity(float yaw, float pitch, float speed)
			{
				this.Yaw = yaw;
				this.Pitch = pitch;
				this.Speed = speed;
			}

			// Token: 0x0400499A RID: 18842
			public float Yaw;

			// Token: 0x0400499B RID: 18843
			public float Pitch;

			// Token: 0x0400499C RID: 18844
			public float Speed;
		}

		// Token: 0x02000F04 RID: 3844
		public struct UVMotionParams
		{
			// Token: 0x0400499D RID: 18845
			public string TexturePath;

			// Token: 0x0400499E RID: 18846
			public int TextureId;

			// Token: 0x0400499F RID: 18847
			public bool AddRandomUVOffset;

			// Token: 0x040049A0 RID: 18848
			public Vector2 Speed;

			// Token: 0x040049A1 RID: 18849
			public float Strength;

			// Token: 0x040049A2 RID: 18850
			public ParticleSpawnerSettings.UVMotionCurveType StrengthCurveType;

			// Token: 0x040049A3 RID: 18851
			public float Scale;
		}

		// Token: 0x02000F05 RID: 3845
		public enum UVMotionCurveType
		{
			// Token: 0x040049A5 RID: 18853
			Constant,
			// Token: 0x040049A6 RID: 18854
			IncreaseLinear,
			// Token: 0x040049A7 RID: 18855
			IncreaseQuartIn,
			// Token: 0x040049A8 RID: 18856
			IncreaseQuartInOut,
			// Token: 0x040049A9 RID: 18857
			IncreaseQuartOut,
			// Token: 0x040049AA RID: 18858
			DecreaseLinear,
			// Token: 0x040049AB RID: 18859
			DecreaseQuartIn,
			// Token: 0x040049AC RID: 18860
			DecreaseQuartInOut,
			// Token: 0x040049AD RID: 18861
			DecreaseQuartOut
		}

		// Token: 0x02000F06 RID: 3846
		public struct IntersectionHighlightParams
		{
			// Token: 0x040049AE RID: 18862
			public float Threshold;

			// Token: 0x040049AF RID: 18863
			public Vector3 Color;
		}
	}
}
