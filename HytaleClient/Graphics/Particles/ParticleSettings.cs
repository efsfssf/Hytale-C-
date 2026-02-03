using System;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Particles
{
	// Token: 0x02000AB4 RID: 2740
	internal class ParticleSettings
	{
		// Token: 0x0400337D RID: 13181
		public const int DefaultFrameIndex = 0;

		// Token: 0x0400337E RID: 13182
		public static readonly Vector2 DefaultScale = Vector2.One;

		// Token: 0x0400337F RID: 13183
		public static readonly Quaternion DefaultRotation = Quaternion.Identity;

		// Token: 0x04003380 RID: 13184
		public static readonly UInt32Color DefaultColor = UInt32Color.White;

		// Token: 0x04003381 RID: 13185
		public const float DefaultOpacity = 1f;

		// Token: 0x04003382 RID: 13186
		public const bool DefaultRandomTextureInverse = false;

		// Token: 0x04003383 RID: 13187
		public string Id;

		// Token: 0x04003384 RID: 13188
		public string TexturePath;

		// Token: 0x04003385 RID: 13189
		public UShortVector2 FrameSize;

		// Token: 0x04003386 RID: 13190
		public Rectangle ImageLocation;

		// Token: 0x04003387 RID: 13191
		public ParticleSettings.SoftParticles SoftParticlesOption = ParticleSettings.SoftParticles.Enable;

		// Token: 0x04003388 RID: 13192
		public float SoftParticlesFadeFactor = 1f;

		// Token: 0x04003389 RID: 13193
		public bool UseSpriteBlending;

		// Token: 0x0400338A RID: 13194
		public ParticleSettings.UVOptions UVOption;

		// Token: 0x0400338B RID: 13195
		public ParticleSettings.ScaleRatioConstraint ScaleRatio = ParticleSettings.ScaleRatioConstraint.OneToOne;

		// Token: 0x0400338C RID: 13196
		public ParticleSettings.ScaleKeyframe[] ScaleKeyframes;

		// Token: 0x0400338D RID: 13197
		public ParticleSettings.RotationKeyframe[] RotationKeyframes;

		// Token: 0x0400338E RID: 13198
		public ParticleSettings.RangeKeyframe[] TextureIndexKeyFrames;

		// Token: 0x0400338F RID: 13199
		public ParticleSettings.ColorKeyframe[] ColorKeyframes;

		// Token: 0x04003390 RID: 13200
		public ParticleSettings.OpacityKeyframe[] OpacityKeyframes;

		// Token: 0x04003391 RID: 13201
		public byte ScaleKeyFrameCount;

		// Token: 0x04003392 RID: 13202
		public byte RotationKeyFrameCount;

		// Token: 0x04003393 RID: 13203
		public byte TextureKeyFrameCount;

		// Token: 0x04003394 RID: 13204
		public byte ColorKeyFrameCount;

		// Token: 0x04003395 RID: 13205
		public byte OpacityKeyFrameCount;

		// Token: 0x02000EF9 RID: 3833
		public enum ScaleRatioConstraint
		{
			// Token: 0x0400496D RID: 18797
			OneToOne,
			// Token: 0x0400496E RID: 18798
			Preserved,
			// Token: 0x0400496F RID: 18799
			None
		}

		// Token: 0x02000EFA RID: 3834
		public enum UVOptions
		{
			// Token: 0x04004971 RID: 18801
			None,
			// Token: 0x04004972 RID: 18802
			RandomFlipU,
			// Token: 0x04004973 RID: 18803
			RandomFlipV,
			// Token: 0x04004974 RID: 18804
			RandomFlipUV,
			// Token: 0x04004975 RID: 18805
			FlipU,
			// Token: 0x04004976 RID: 18806
			FlipV,
			// Token: 0x04004977 RID: 18807
			FlipUV
		}

		// Token: 0x02000EFB RID: 3835
		public enum SoftParticles
		{
			// Token: 0x04004979 RID: 18809
			Enable,
			// Token: 0x0400497A RID: 18810
			Disable,
			// Token: 0x0400497B RID: 18811
			Require
		}

		// Token: 0x02000EFC RID: 3836
		public struct RangeKeyframe
		{
			// Token: 0x0400497C RID: 18812
			public byte Time;

			// Token: 0x0400497D RID: 18813
			public byte Min;

			// Token: 0x0400497E RID: 18814
			public byte Max;
		}

		// Token: 0x02000EFD RID: 3837
		public struct ScaleKeyframe
		{
			// Token: 0x0400497F RID: 18815
			public byte Time;

			// Token: 0x04004980 RID: 18816
			public Vector2 Min;

			// Token: 0x04004981 RID: 18817
			public Vector2 Max;
		}

		// Token: 0x02000EFE RID: 3838
		public struct RotationKeyframe
		{
			// Token: 0x04004982 RID: 18818
			public byte Time;

			// Token: 0x04004983 RID: 18819
			public Vector3 Min;

			// Token: 0x04004984 RID: 18820
			public Vector3 Max;
		}

		// Token: 0x02000EFF RID: 3839
		public struct ColorKeyframe
		{
			// Token: 0x04004985 RID: 18821
			public byte Time;

			// Token: 0x04004986 RID: 18822
			public UInt32Color Color;
		}

		// Token: 0x02000F00 RID: 3840
		public struct OpacityKeyframe
		{
			// Token: 0x04004987 RID: 18823
			public byte Time;

			// Token: 0x04004988 RID: 18824
			public float Opacity;
		}
	}
}
