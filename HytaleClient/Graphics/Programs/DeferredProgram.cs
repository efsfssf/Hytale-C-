using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A5F RID: 2655
	internal class DeferredProgram : GPUProgram
	{
		// Token: 0x06005456 RID: 21590 RVA: 0x00183194 File Offset: 0x00181394
		public DeferredProgram(bool reverseZ, bool useDownsampledZ, bool useDeferredFog, bool useDeferredLight, bool useLowResLighting, bool useSSAO) : base("ScreenVS.glsl", "DeferredFS.glsl", null)
		{
			this.ReverseZ = reverseZ;
			this.UseDownsampledZ = useDownsampledZ;
			this.UseFog = useDeferredFog;
			this.UseLight = useDeferredLight;
			this.UseLowResLighting = useLowResLighting;
			this.UseSSAO = useSSAO;
		}

		// Token: 0x06005457 RID: 21591 RVA: 0x00183238 File Offset: 0x00181438
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(new Dictionary<string, string>
			{
				{
					"USE_FAR_CORNERS",
					"1"
				}
			});
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("REVERSE_Z", this.ReverseZ ? "1" : "0");
			dictionary.Add("USE_FOG", this.UseFog ? "1" : "0");
			dictionary.Add("USE_LIGHT", this.UseLight ? "1" : "0");
			dictionary.Add("USE_LINEAR_Z", this.UseLinearZ ? "1" : "0");
			dictionary.Add("USE_LOWRES_Z", this.UseDownsampledZ ? "1" : "0");
			dictionary.Add("USE_LOWRES_LIGHT", this.UseLowResLighting ? "1" : "0");
			dictionary.Add("USE_SSAO", this.UseSSAO ? "1" : "0");
			dictionary.Add("USE_EDGE_AWARE_UPSAMPLING", this.UseSmartUpsampling ? "1" : "0");
			dictionary.Add("USE_CLOUDS_SHADOWS", this.UseCloudsShadows ? "1" : "0");
			dictionary.Add("USE_UNDERWATER_CAUSTICS", this.UseUnderwaterCaustics ? "1" : "0");
			dictionary.Add("USE_SKY_AMBIENT", this.UseSkyAmbient ? "1" : "0");
			dictionary.Add("USE_LBUFFER_COMPRESSION", this.UseLightBufferCompression ? "1" : "0");
			dictionary.Add("USE_FOG_DITHERING", this.UseDithering ? "1" : "0");
			dictionary.Add("USE_SMOOTH_NEAR_MOOD_FOG_COLOR", this.UseSmoothNearMoodColor ? "1" : "0");
			dictionary.Add("USE_MOOD_FOG", this.UseMoodFog ? "1" : "0");
			dictionary.Add("USE_DEFERRED_SHADOW", this.UseDeferredShadow ? "1" : "0");
			dictionary.Add("USE_DEFERRED_SHADOW_BLURRED", this.UseDeferredShadowBlurred ? "1" : "0");
			dictionary.Add("USE_DEFERRED_SHADOW_INDOOR_FADING", this.UseDeferredShadowIndoorFading ? "1" : "0");
			dictionary.Add("INPUT_NORMALS_IN_WS", this.HasInputNormalsInWorldSpace ? "1" : "0");
			dictionary.Add("DEBUG_SHADOW_CASCADES", this.DebugShadowCascades ? "1" : "0");
			dictionary.Add("CASCADE_COUNT", this.CascadeCount.ToString());
			dictionary.Add("DEBUG_PIXELS", (this.DebugPixelInfoView != DeferredProgram.DebugPixelInfo.None) ? "1" : "0");
			switch (this.DebugPixelInfoView)
			{
			case DeferredProgram.DebugPixelInfo.UseCleanShadowBackfaces:
				dictionary.Add("DEBUG_PIXELS_USE_CLEAN_SHADOW_BACKFACES", "1");
				break;
			case DeferredProgram.DebugPixelInfo.HasBloom:
				dictionary.Add("DEBUG_PIXELS_HAS_BLOOM", "1");
				break;
			case DeferredProgram.DebugPixelInfo.HasSSAO:
				dictionary.Add("DEBUG_PIXELS_HAS_SSAO", "1");
				break;
			case DeferredProgram.DebugPixelInfo.FinalSSAO:
				dictionary.Add("DEBUG_PIXELS_FINAL_SSAO", "1");
				break;
			case DeferredProgram.DebugPixelInfo.FinalAmbient:
				dictionary.Add("DEBUG_PIXELS_FINAL_AMBIENT", "1");
				break;
			case DeferredProgram.DebugPixelInfo.FinalLight:
				dictionary.Add("DEBUG_PIXELS_FINAL_LIGHT", "1");
				break;
			}
			uint fragmentShader = base.CompileFragmentShader(dictionary);
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x06005458 RID: 21592 RVA: 0x001835E4 File Offset: 0x001817E4
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			bool useFog = this.UseFog;
			if (useFog)
			{
				this.FogNoiseTexture.SetValue(7);
			}
			bool useDeferredShadow = this.UseDeferredShadow;
			if (useDeferredShadow)
			{
				this.ShadowTexture.SetValue(6);
			}
			bool flag = this.UseUnderwaterCaustics || this.UseCloudsShadows;
			if (flag)
			{
				this.TopDownProjectionTexture.SetValue(4);
			}
			bool useSSAO = this.UseSSAO;
			if (useSSAO)
			{
				this.SSAOTexture.SetValue(3);
			}
			this.DepthTexture.SetValue(2);
			this.LightTexture.SetValue(1);
			this.ColorTexture.SetValue(0);
			this.SceneDataBlock.SetupBindingPoint(this, 0U);
		}

		// Token: 0x04002FAA RID: 12202
		public UniformBufferObject SceneDataBlock;

		// Token: 0x04002FAB RID: 12203
		public Uniform FarCorners;

		// Token: 0x04002FAC RID: 12204
		public Uniform DebugShadowMatrix;

		// Token: 0x04002FAD RID: 12205
		private Uniform ColorTexture;

		// Token: 0x04002FAE RID: 12206
		private Uniform LightTexture;

		// Token: 0x04002FAF RID: 12207
		private Uniform DepthTexture;

		// Token: 0x04002FB0 RID: 12208
		private Uniform SSAOTexture;

		// Token: 0x04002FB1 RID: 12209
		private Uniform TopDownProjectionTexture;

		// Token: 0x04002FB2 RID: 12210
		private Uniform FogNoiseTexture;

		// Token: 0x04002FB3 RID: 12211
		private Uniform ShadowTexture;

		// Token: 0x04002FB4 RID: 12212
		public bool ReverseZ;

		// Token: 0x04002FB5 RID: 12213
		public bool UseFog;

		// Token: 0x04002FB6 RID: 12214
		public bool UseLight;

		// Token: 0x04002FB7 RID: 12215
		public bool UseLinearZ;

		// Token: 0x04002FB8 RID: 12216
		public bool UseDownsampledZ;

		// Token: 0x04002FB9 RID: 12217
		public bool UseLowResLighting;

		// Token: 0x04002FBA RID: 12218
		public bool UseSSAO;

		// Token: 0x04002FBB RID: 12219
		public bool UseCloudsShadows = true;

		// Token: 0x04002FBC RID: 12220
		public bool UseUnderwaterCaustics = true;

		// Token: 0x04002FBD RID: 12221
		public bool UseSkyAmbient = true;

		// Token: 0x04002FBE RID: 12222
		public bool UseSmartUpsampling = false;

		// Token: 0x04002FBF RID: 12223
		public bool UseLightBufferCompression = false;

		// Token: 0x04002FC0 RID: 12224
		public bool UseDithering;

		// Token: 0x04002FC1 RID: 12225
		public bool UseSmoothNearMoodColor;

		// Token: 0x04002FC2 RID: 12226
		public bool UseMoodFog;

		// Token: 0x04002FC3 RID: 12227
		public bool UseDeferredShadow = true;

		// Token: 0x04002FC4 RID: 12228
		public bool UseDeferredShadowBlurred = true;

		// Token: 0x04002FC5 RID: 12229
		public bool UseDeferredShadowIndoorFading = false;

		// Token: 0x04002FC6 RID: 12230
		public bool HasInputNormalsInWorldSpace = true;

		// Token: 0x04002FC7 RID: 12231
		public bool DebugShadowCascades = false;

		// Token: 0x04002FC8 RID: 12232
		public uint CascadeCount = 1U;

		// Token: 0x04002FC9 RID: 12233
		public DeferredProgram.DebugPixelInfo DebugPixelInfoView = DeferredProgram.DebugPixelInfo.None;

		// Token: 0x02000ED7 RID: 3799
		public enum DebugPixelInfo
		{
			// Token: 0x040048BB RID: 18619
			None,
			// Token: 0x040048BC RID: 18620
			UseCleanShadowBackfaces,
			// Token: 0x040048BD RID: 18621
			HasBloom,
			// Token: 0x040048BE RID: 18622
			HasSSAO,
			// Token: 0x040048BF RID: 18623
			FinalSSAO,
			// Token: 0x040048C0 RID: 18624
			FinalAmbient,
			// Token: 0x040048C1 RID: 18625
			FinalLight
		}
	}
}
