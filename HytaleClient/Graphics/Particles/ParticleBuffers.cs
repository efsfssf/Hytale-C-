using System;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Particles
{
	// Token: 0x02000AB2 RID: 2738
	internal struct ParticleBuffers : IFXDataStorage
	{
		// Token: 0x06005608 RID: 22024 RVA: 0x0019A894 File Offset: 0x00198A94
		public void Initialize(int maxParticles)
		{
			this.Count = maxParticles;
			this.Life = new ParticleBuffers.ParticleLifeData[maxParticles];
			this.Scale = new Vector2[maxParticles];
			this.ScaleRatio = new float[maxParticles];
			this.Data0 = new ParticleBuffers.ParticleSimulationData[maxParticles];
			this.Data1 = new ParticleBuffers.ParticleRenderData[maxParticles];
		}

		// Token: 0x06005609 RID: 22025 RVA: 0x0019A8E5 File Offset: 0x00198AE5
		public void Release()
		{
		}

		// Token: 0x0400333B RID: 13115
		public const int InverseUTextureBitId = 0;

		// Token: 0x0400333C RID: 13116
		public const int InverseVTextureBitId = 1;

		// Token: 0x0400333D RID: 13117
		public const int CollisionBitId = 2;

		// Token: 0x0400333E RID: 13118
		public int Count;

		// Token: 0x0400333F RID: 13119
		public ParticleBuffers.ParticleLifeData[] Life;

		// Token: 0x04003340 RID: 13120
		public Vector2[] Scale;

		// Token: 0x04003341 RID: 13121
		public float[] ScaleRatio;

		// Token: 0x04003342 RID: 13122
		public ParticleBuffers.ParticleSimulationData[] Data0;

		// Token: 0x04003343 RID: 13123
		public ParticleBuffers.ParticleRenderData[] Data1;

		// Token: 0x02000EEF RID: 3823
		public struct ParticleRenderData
		{
			// Token: 0x0400493C RID: 18748
			public Vector3 Position;

			// Token: 0x0400493D RID: 18749
			public Vector3 Velocity;

			// Token: 0x0400493E RID: 18750
			public Vector3 AttractorVelocity;

			// Token: 0x0400493F RID: 18751
			public Quaternion Rotation;

			// Token: 0x04004940 RID: 18752
			public uint Color;

			// Token: 0x04004941 RID: 18753
			public byte TargetTextureIndex;

			// Token: 0x04004942 RID: 18754
			public byte PrevTargetTextureIndex;

			// Token: 0x04004943 RID: 18755
			public ushort TargetTextureBlendProgress;

			// Token: 0x04004944 RID: 18756
			public byte BoolData;

			// Token: 0x04004945 RID: 18757
			public ushort Seed;
		}

		// Token: 0x02000EF0 RID: 3824
		public struct ParticleSimulationData
		{
			// Token: 0x04004946 RID: 18758
			public Vector3 SpawnerPositionAtSpawn;

			// Token: 0x04004947 RID: 18759
			public Quaternion RotationOffset;

			// Token: 0x04004948 RID: 18760
			public Vector2 ScaleStep;

			// Token: 0x04004949 RID: 18761
			public float ScaleNextKeyframeTime;

			// Token: 0x0400494A RID: 18762
			public byte ScaleAnimationIndex;

			// Token: 0x0400494B RID: 18763
			public Vector3 RotationStep;

			// Token: 0x0400494C RID: 18764
			public Vector3 CurrentRotation;

			// Token: 0x0400494D RID: 18765
			public float RotationNextKeyframeTime;

			// Token: 0x0400494E RID: 18766
			public byte RotationAnimationIndex;

			// Token: 0x0400494F RID: 18767
			public float TextureNextKeyframeTime;

			// Token: 0x04004950 RID: 18768
			public byte TextureAnimationIndex;
		}

		// Token: 0x02000EF1 RID: 3825
		public struct ParticleLifeData
		{
			// Token: 0x04004951 RID: 18769
			public float LifeSpanTimer;

			// Token: 0x04004952 RID: 18770
			public float LifeSpan;
		}
	}
}
