using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A7B RID: 2683
	internal class PostEffectProgram : GPUProgram
	{
		// Token: 0x060054C7 RID: 21703 RVA: 0x001868D0 File Offset: 0x00184AD0
		public PostEffectProgram(bool reverseZ, bool useAntiAliasingHighQuality, bool discardDark, string variationName = null) : base("ScreenVS.glsl", "PostEffectFS.glsl", variationName)
		{
			this.ReverseZ = reverseZ;
			this.UseAntiAliasingHighQuality = useAntiAliasingHighQuality;
			this.DiscardDark = discardDark;
		}

		// Token: 0x060054C8 RID: 21704 RVA: 0x00186930 File Offset: 0x00184B30
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(new Dictionary<string, string>
			{
				{
					"REVERSE_Z",
					this.ReverseZ ? "1" : "0"
				},
				{
					"DEBUG_TILES",
					this.DebugTiles ? "1" : "0"
				},
				{
					"USE_FXAA_HIGH_QUALITY",
					this.UseAntiAliasingHighQuality ? "1" : "0"
				},
				{
					"USE_FXAA",
					this.UseFXAA ? "1" : "0"
				},
				{
					"DISCARD_DARK",
					this.DiscardDark ? "1" : "0"
				},
				{
					"USE_SHARPEN",
					this.UseSharpenEffect ? "1" : "0"
				},
				{
					"SHARPEN_STRENGTH",
					this.SharpenStrength.ToString(GPUProgram.DecimalPointFormatting)
				},
				{
					"DOF_VERSION",
					this.DepthOfFieldVersion.ToString()
				},
				{
					"USE_DOF",
					this.UseDepthOfField ? "1" : "0"
				},
				{
					"USE_BLOOM",
					this.UseBloom ? "1" : "0"
				},
				{
					"SUN_FB_POW",
					this.SunFbPow ? "1" : "0"
				},
				{
					"USE_SUNSHAFT",
					this.UseSunshaft ? "1" : "0"
				},
				{
					"USE_LINEAR_Z",
					this.UseLinearZ ? "1" : "0"
				},
				{
					"USE_DISTORTION",
					this.UseDistortion ? "1" : "0"
				},
				{
					"USE_VOL_SUNSHAFT",
					this.UseVolumetricSunshaft ? "1" : "0"
				}
			});
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x060054C9 RID: 21705 RVA: 0x00186B40 File Offset: 0x00184D40
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.DistortionTexture.SetValue(8);
			this.ColorTexture.SetValue(0);
			bool useDepthOfField = this.UseDepthOfField;
			if (useDepthOfField)
			{
				bool flag = this.DepthOfFieldVersion != 3;
				if (flag)
				{
					this.DepthTexture.SetValue(1);
				}
				bool flag2 = this.DepthOfFieldVersion == 1;
				if (flag2)
				{
					this.BlurTexture.SetValue(2);
				}
				else
				{
					bool flag3 = this.DepthOfFieldVersion == 2;
					if (flag3)
					{
						this.NearBlurTexture.SetValue(2);
						this.FarBlurTexture.SetValue(3);
					}
					else
					{
						bool flag4 = this.DepthOfFieldVersion == 3;
						if (flag4)
						{
							this.CoCTexture.SetValue(1);
							this.CoCLowResTexture.SetValue(2);
							this.NearCoCBlurredLowResTexture.SetValue(3);
							this.NearFieldLowResTexture.SetValue(4);
							this.FarFieldLowResTexture.SetValue(5);
							this.SceneColorLowResTexturePoint.SetValue(6);
						}
					}
				}
			}
			bool useBloom = this.UseBloom;
			if (useBloom)
			{
				this.BloomTexture.SetValue(7);
			}
			bool useVolumetricSunshaft = this.UseVolumetricSunshaft;
			if (useVolumetricSunshaft)
			{
				this.VolumetricSunshaftTexture.SetValue(9);
			}
		}

		// Token: 0x040030E3 RID: 12515
		public Uniform PixelSize;

		// Token: 0x040030E4 RID: 12516
		public Uniform DebugTileResolution;

		// Token: 0x040030E5 RID: 12517
		public Uniform Time;

		// Token: 0x040030E6 RID: 12518
		public Uniform DistortionAmplitude;

		// Token: 0x040030E7 RID: 12519
		public Uniform DistortionFrequency;

		// Token: 0x040030E8 RID: 12520
		public Uniform ColorBrightnessContrast;

		// Token: 0x040030E9 RID: 12521
		public Uniform ColorSaturation;

		// Token: 0x040030EA RID: 12522
		public Uniform ColorFilter;

		// Token: 0x040030EB RID: 12523
		public Uniform ProjectionMatrix;

		// Token: 0x040030EC RID: 12524
		public Uniform FarClip;

		// Token: 0x040030ED RID: 12525
		public Uniform ApplyBloom;

		// Token: 0x040030EE RID: 12526
		public Uniform VolumetricSunshaftStrength;

		// Token: 0x040030EF RID: 12527
		public Uniform NearBlurMax;

		// Token: 0x040030F0 RID: 12528
		public Uniform NearBlurry;

		// Token: 0x040030F1 RID: 12529
		public Uniform NearSharp;

		// Token: 0x040030F2 RID: 12530
		public Uniform FarSharp;

		// Token: 0x040030F3 RID: 12531
		public Uniform FarBlurry;

		// Token: 0x040030F4 RID: 12532
		public Uniform FarBlurMax;

		// Token: 0x040030F5 RID: 12533
		private Uniform ColorTexture;

		// Token: 0x040030F6 RID: 12534
		private Uniform DistortionTexture;

		// Token: 0x040030F7 RID: 12535
		private Uniform DepthTexture;

		// Token: 0x040030F8 RID: 12536
		private Uniform BlurTexture;

		// Token: 0x040030F9 RID: 12537
		private Uniform NearBlurTexture;

		// Token: 0x040030FA RID: 12538
		private Uniform FarBlurTexture;

		// Token: 0x040030FB RID: 12539
		private Uniform CoCTexture;

		// Token: 0x040030FC RID: 12540
		private Uniform CoCLowResTexture;

		// Token: 0x040030FD RID: 12541
		private Uniform NearCoCBlurredLowResTexture;

		// Token: 0x040030FE RID: 12542
		private Uniform NearFieldLowResTexture;

		// Token: 0x040030FF RID: 12543
		private Uniform FarFieldLowResTexture;

		// Token: 0x04003100 RID: 12544
		private Uniform SceneColorLowResTexturePoint;

		// Token: 0x04003101 RID: 12545
		private Uniform BloomTexture;

		// Token: 0x04003102 RID: 12546
		private Uniform VolumetricSunshaftTexture;

		// Token: 0x04003103 RID: 12547
		public bool UseFXAA = true;

		// Token: 0x04003104 RID: 12548
		public bool UseSharpenEffect = true;

		// Token: 0x04003105 RID: 12549
		public float SharpenStrength = 0.1f;

		// Token: 0x04003106 RID: 12550
		private readonly bool UseAntiAliasingHighQuality;

		// Token: 0x04003107 RID: 12551
		private readonly bool DiscardDark;

		// Token: 0x04003108 RID: 12552
		public bool ReverseZ;

		// Token: 0x04003109 RID: 12553
		public bool DebugTiles;

		// Token: 0x0400310A RID: 12554
		public int DepthOfFieldVersion = 3;

		// Token: 0x0400310B RID: 12555
		public bool UseDepthOfField;

		// Token: 0x0400310C RID: 12556
		public bool UseBloom;

		// Token: 0x0400310D RID: 12557
		public bool SunFbPow;

		// Token: 0x0400310E RID: 12558
		public bool UseSunshaft;

		// Token: 0x0400310F RID: 12559
		public bool UseDistortion = true;

		// Token: 0x04003110 RID: 12560
		public bool UseVolumetricSunshaft;

		// Token: 0x04003111 RID: 12561
		private bool UseLinearZ;
	}
}
