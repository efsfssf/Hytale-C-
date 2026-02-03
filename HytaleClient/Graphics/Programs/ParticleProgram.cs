using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A7A RID: 2682
	internal class ParticleProgram : GPUProgram
	{
		// Token: 0x060054C3 RID: 21699 RVA: 0x0018642C File Offset: 0x0018462C
		public ParticleProgram(bool useForwardClusteredLighting = true, bool useLightDirectAccess = true, bool useCustomZDistribution = true, bool useSunShadows = true, bool useDistortionRT = false, bool useErosion = false, string variationName = null) : base("ParticleVS.glsl", "ParticleFS.glsl", variationName)
		{
			this.UseForwardClusteredLighting = useForwardClusteredLighting;
			this.UseLightDirectAccess = useLightDirectAccess;
			this.UseCustomZDistribution = useCustomZDistribution;
			this.UseSunShadows = useSunShadows;
			this.UseDistortionRT = useDistortionRT;
			this.UseErosion = useErosion;
		}

		// Token: 0x060054C4 RID: 21700 RVA: 0x00186484 File Offset: 0x00184684
		public void SetupTextureUnits(ref ParticleProgram.TextureUnitLayout textureUnitLayout, bool initUniforms = false)
		{
			Debug.Assert(GPUProgram.IsResourceBindingLayoutValid<ParticleProgram.TextureUnitLayout>(textureUnitLayout), "Invalid TextureUnitLayout.");
			this._textureUnitLayout = textureUnitLayout;
			if (initUniforms)
			{
				this.InitUniforms();
			}
		}

		// Token: 0x060054C5 RID: 21701 RVA: 0x001864C4 File Offset: 0x001846C4
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(new Dictionary<string, string>
			{
				{
					"USE_SMOOTH_NEAR_MOOD_FOG_COLOR",
					this.UseSmoothNearMoodColor ? "1" : "0"
				},
				{
					"USE_MOOD_FOG",
					this.UseMoodFog ? "1" : "0"
				},
				{
					"USE_FOG",
					this.UseFog ? "1" : "0"
				},
				{
					"USE_CLUSTERED_LIGHTING",
					this.UseForwardClusteredLighting ? "1" : "0"
				},
				{
					"USE_DIRECT_ACCESS",
					this.UseLightDirectAccess ? "1" : "0"
				},
				{
					"USE_CUSTOM_Z_DISTRIBUTION",
					this.UseCustomZDistribution ? "1" : "0"
				},
				{
					"USE_SUN_SHADOWS",
					this.UseSunShadows ? "1" : "0"
				},
				{
					"USE_LINEAR_Z",
					this.UseLinearZ ? "1" : "0"
				},
				{
					"CASCADE_COUNT",
					this.SunShadowCascadeCount.ToString()
				},
				{
					"USE_NOISE",
					"0"
				},
				{
					"USE_SINGLE_SAMPLE",
					"1"
				},
				{
					"USE_CAMERA_BIAS",
					"0"
				},
				{
					"USE_NORMAL_BIAS",
					"0"
				},
				{
					"USE_DISTORTION_RT",
					this.UseDistortionRT ? "1" : "0"
				}
			});
			uint fragmentShader = base.CompileFragmentShader(new Dictionary<string, string>
			{
				{
					"DEBUG_TEXTURE",
					this.UseDebugTexture ? "1" : "0"
				},
				{
					"DEBUG_OVERDRAW",
					this.UseDebugOverdraw ? "1" : "0"
				},
				{
					"DEBUG_UVMOTION",
					this.UseDebugUVMotion ? "1" : "0"
				},
				{
					"USE_MOOD_FOG",
					this.UseMoodFog ? "1" : "0"
				},
				{
					"USE_FOG",
					this.UseFog ? "1" : "0"
				},
				{
					"USE_DISTORTION_RT",
					this.UseDistortionRT ? "1" : "0"
				},
				{
					"USE_OIT",
					this.UseOIT ? "1" : "0"
				},
				{
					"USE_EROSION",
					this.UseErosion ? "1" : "0"
				}
			});
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x060054C6 RID: 21702 RVA: 0x00186784 File Offset: 0x00184984
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.MomentsTexture.SetValue((int)this._textureUnitLayout.OITMoments);
			this.TotalOpticalDepthTexture.SetValue((int)this._textureUnitLayout.OITTotalOpticalDepth);
			this.LightIndicesOrDataBufferTexture.SetValue((int)this._textureUnitLayout.LightIndicesOrDataBuffer);
			this.LightGridTexture.SetValue((int)this._textureUnitLayout.LightGrid);
			bool useSunShadows = this.UseSunShadows;
			if (useSunShadows)
			{
				this.ShadowMap.SetValue((int)this._textureUnitLayout.ShadowMap);
			}
			bool useMoodFog = this.UseMoodFog;
			if (useMoodFog)
			{
				this.FogNoiseTexture.SetValue((int)this._textureUnitLayout.FogNoise);
			}
			this.SpawnerDataBuffer.SetValue((int)this._textureUnitLayout.FXDataBuffer);
			this.UVMotionTexture.SetValue((int)this._textureUnitLayout.UVMotion);
			this.DepthTexture.SetValue((int)this._textureUnitLayout.SceneDepth);
			this.SmoothTexture.SetValue((int)this._textureUnitLayout.LinearFilteredAtlas);
			this.Texture.SetValue((int)this._textureUnitLayout.Atlas);
			this.SceneDataBlock.SetupBindingPoint(this, 0U);
			this.PointLightBlock.SetupBindingPoint(this, 2U);
		}

		// Token: 0x040030BE RID: 12478
		public UniformBufferObject SceneDataBlock;

		// Token: 0x040030BF RID: 12479
		public UniformBufferObject PointLightBlock;

		// Token: 0x040030C0 RID: 12480
		public Uniform DebugOverdraw;

		// Token: 0x040030C1 RID: 12481
		public Uniform InvTextureAtlasSize;

		// Token: 0x040030C2 RID: 12482
		public Uniform CurrentInvViewportSize;

		// Token: 0x040030C3 RID: 12483
		public Uniform OITParams;

		// Token: 0x040030C4 RID: 12484
		private Uniform MomentsTexture;

		// Token: 0x040030C5 RID: 12485
		private Uniform TotalOpticalDepthTexture;

		// Token: 0x040030C6 RID: 12486
		private Uniform LightGridTexture;

		// Token: 0x040030C7 RID: 12487
		private Uniform LightIndicesOrDataBufferTexture;

		// Token: 0x040030C8 RID: 12488
		private Uniform ShadowMap;

		// Token: 0x040030C9 RID: 12489
		private Uniform FogNoiseTexture;

		// Token: 0x040030CA RID: 12490
		private Uniform SmoothTexture;

		// Token: 0x040030CB RID: 12491
		private Uniform Texture;

		// Token: 0x040030CC RID: 12492
		private Uniform DepthTexture;

		// Token: 0x040030CD RID: 12493
		private Uniform UVMotionTexture;

		// Token: 0x040030CE RID: 12494
		private Uniform SpawnerDataBuffer;

		// Token: 0x040030CF RID: 12495
		public readonly Attrib AttribData1;

		// Token: 0x040030D0 RID: 12496
		public readonly Attrib AttribData2;

		// Token: 0x040030D1 RID: 12497
		public readonly Attrib AttribData3;

		// Token: 0x040030D2 RID: 12498
		public readonly Attrib AttribData4;

		// Token: 0x040030D3 RID: 12499
		public bool UseDebugOverdraw;

		// Token: 0x040030D4 RID: 12500
		public bool UseDebugTexture;

		// Token: 0x040030D5 RID: 12501
		public bool UseDebugUVMotion;

		// Token: 0x040030D6 RID: 12502
		public bool UseForwardClusteredLighting;

		// Token: 0x040030D7 RID: 12503
		public bool UseLightDirectAccess;

		// Token: 0x040030D8 RID: 12504
		public bool UseCustomZDistribution;

		// Token: 0x040030D9 RID: 12505
		public bool UseSunShadows;

		// Token: 0x040030DA RID: 12506
		public bool UseLinearZ;

		// Token: 0x040030DB RID: 12507
		public uint SunShadowCascadeCount = 1U;

		// Token: 0x040030DC RID: 12508
		public bool UseSmoothNearMoodColor;

		// Token: 0x040030DD RID: 12509
		public bool UseMoodFog;

		// Token: 0x040030DE RID: 12510
		public bool UseFog;

		// Token: 0x040030DF RID: 12511
		public bool UseOIT;

		// Token: 0x040030E0 RID: 12512
		public bool UseDistortionRT;

		// Token: 0x040030E1 RID: 12513
		public bool UseErosion;

		// Token: 0x040030E2 RID: 12514
		private ParticleProgram.TextureUnitLayout _textureUnitLayout;

		// Token: 0x02000EDE RID: 3806
		public struct TextureUnitLayout
		{
			// Token: 0x040048DF RID: 18655
			public byte Atlas;

			// Token: 0x040048E0 RID: 18656
			public byte LinearFilteredAtlas;

			// Token: 0x040048E1 RID: 18657
			public byte UVMotion;

			// Token: 0x040048E2 RID: 18658
			public byte FXDataBuffer;

			// Token: 0x040048E3 RID: 18659
			public byte LightIndicesOrDataBuffer;

			// Token: 0x040048E4 RID: 18660
			public byte LightGrid;

			// Token: 0x040048E5 RID: 18661
			public byte ShadowMap;

			// Token: 0x040048E6 RID: 18662
			public byte FogNoise;

			// Token: 0x040048E7 RID: 18663
			public byte SceneDepth;

			// Token: 0x040048E8 RID: 18664
			public byte OITMoments;

			// Token: 0x040048E9 RID: 18665
			public byte OITTotalOpticalDepth;
		}
	}
}
