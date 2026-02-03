using System;
using System.Collections.Generic;
using HytaleClient.Data.Map;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A75 RID: 2677
	internal abstract class MapChunkBaseProgram : GPUProgram
	{
		// Token: 0x060054B8 RID: 21688 RVA: 0x00185B10 File Offset: 0x00183D10
		public MapChunkBaseProgram(bool alphaTest, bool alphaBlend, bool near, bool useDeferred, bool useLOD, string variationName = null) : base("MapChunkVS.glsl", "MapChunkFS.glsl", variationName)
		{
			this._alphaTest = alphaTest;
			this._alphaBlend = alphaBlend;
			this._near = near;
			this.Deferred = useDeferred;
			this.UseLOD = useLOD;
		}

		// Token: 0x060054B9 RID: 21689 RVA: 0x00185B9C File Offset: 0x00183D9C
		public override bool Initialize()
		{
			base.Initialize();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("DEFERRED", this.Deferred ? "1" : "0");
			dictionary.Add("ALPHA_TEST", this._alphaTest ? "1" : "0");
			dictionary.Add("ALPHA_BLEND", this._alphaBlend ? "1" : "0");
			dictionary.Add("NEAR", this._near ? "1" : "0");
			dictionary.Add("ANIMATED", "0");
			dictionary.Add("USE_FOLIAGE_FADING", this.UseFoliageFading ? "1" : "0");
			dictionary.Add("USE_LOD", this.UseLOD ? "1" : "0");
			dictionary.Add("LOD_DISTANCE", this.LODDistance.ToString());
			dictionary.Add("USE_FOG_DITHERING", this.UseDithering ? "1" : "0");
			dictionary.Add("USE_SMOOTH_NEAR_MOOD_FOG_COLOR", this.UseSmoothNearMoodColor ? "1" : "0");
			dictionary.Add("USE_MOOD_FOG", this.UseMoodFog ? "1" : "0");
			dictionary.Add("USE_FOG", this.UseFog ? "1" : "0");
			dictionary.Add("USE_OIT", (!this.Deferred && this.UseOIT) ? "1" : "0");
			dictionary.Add("USE_CLOUDS_SHADOWS", this.UseCloudsShadows ? "1" : "0");
			dictionary.Add("USE_UNDERWATER_CAUSTICS", this.UseUnderwaterCaustics ? "1" : "0");
			dictionary.Add("USE_SKY_AMBIENT", this.UseSkyAmbient ? "1" : "0");
			dictionary.Add("DEBUG_BOUNDARIES", this.UseDebugBoundaries ? "1" : "0");
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			dictionary2.Add("DEFERRED", this.Deferred ? "1" : "0");
			dictionary2.Add("USE_LBUFFER_COMPRESSION", this.UseLightBufferCompression ? "1" : "0");
			dictionary2.Add("ALPHA_TEST", this._alphaTest ? "1" : "0");
			dictionary2.Add("ALPHA_BLEND", this._alphaBlend ? "1" : "0");
			dictionary2.Add("NEAR", this._near ? "1" : "0");
			dictionary2.Add("ANIMATED", "0");
			dictionary2.Add("USE_CLOUDS_SHADOWS", this.UseCloudsShadows ? "1" : "0");
			dictionary2.Add("USE_UNDERWATER_CAUSTICS", this.UseUnderwaterCaustics ? "1" : "0");
			dictionary2.Add("USE_SKY_AMBIENT", this.UseSkyAmbient ? "1" : "0");
			dictionary2.Add("USE_CLUSTERED_LIGHTING", (!this.Deferred && this.UseForwardClusteredLighting) ? "1" : "0");
			dictionary2.Add("USE_DIRECT_ACCESS", (!this.Deferred && this.UseLightDirectAccess) ? "1" : "0");
			dictionary2.Add("USE_CUSTOM_Z_DISTRIBUTION", (!this.Deferred && this.UseCustomZDistribution) ? "1" : "0");
			dictionary2.Add("WRITE_RENDERCONFIG_IN_ALPHA", (!this.Deferred && this.WriteRenderConfigBitsInAlpha) ? "1" : "0");
			dictionary2.Add("USE_FORWARD_SUN_SHADOWS", (!this.Deferred && this.UseForwardSunShadows) ? "1" : "0");
			dictionary2.Add("USE_LINEAR_Z", this.UseLinearZ ? "1" : "0");
			dictionary2.Add("CASCADE_COUNT", this.SunShadowCascadeCount.ToString());
			dictionary2.Add("USE_NOISE", "1");
			dictionary2.Add("USE_SINGLE_SAMPLE", "1");
			dictionary2.Add("USE_CAMERA_BIAS", "0");
			dictionary2.Add("USE_NORMAL_BIAS", "1");
			dictionary2.Add("USE_FOG_DITHERING", (!this.Deferred && this.UseDithering) ? "1" : "0");
			dictionary2.Add("USE_SMOOTH_NEAR_MOOD_FOG_COLOR", (!this.Deferred && this.UseSmoothNearMoodColor) ? "1" : "0");
			dictionary2.Add("USE_MOOD_FOG", (!this.Deferred && this.UseMoodFog) ? "1" : "0");
			dictionary2.Add("USE_FOG", (!this.Deferred && this.UseFog) ? "1" : "0");
			dictionary2.Add("USE_OIT", (!this.Deferred && this.UseOIT) ? "1" : "0");
			foreach (ClientBlockType.ClientShaderEffect clientShaderEffect in (ClientBlockType.ClientShaderEffect[])Enum.GetValues(typeof(ClientBlockType.ClientShaderEffect)))
			{
				string key = "EFFECT_" + clientShaderEffect.ToString().ToUpper();
				int num = (int)clientShaderEffect;
				string value = num.ToString();
				dictionary.Add(key, value);
				dictionary2.Add(key, value);
			}
			foreach (ShadingMode shadingMode in (ShadingMode[])Enum.GetValues(typeof(ShadingMode)))
			{
				string key2 = "SHADING_" + shadingMode.ToString().ToUpper();
				int num = (int)shadingMode;
				string value2 = num.ToString();
				dictionary.Add(key2, value2);
				dictionary2.Add(key2, value2);
			}
			uint vertexShader = base.CompileVertexShader(dictionary);
			uint fragmentShader = base.CompileFragmentShader(dictionary2);
			return base.MakeProgram(vertexShader, fragmentShader, new List<GPUProgram.AttribBindingInfo>(5)
			{
				new GPUProgram.AttribBindingInfo(0U, "vertPositionAndDoubleSidedAndBlockId"),
				new GPUProgram.AttribBindingInfo(1U, "vertTexCoords"),
				new GPUProgram.AttribBindingInfo(2U, "vertDataPacked")
			}, true, null);
		}

		// Token: 0x060054BA RID: 21690 RVA: 0x00186234 File Offset: 0x00184434
		protected override void InitUniforms()
		{
			this.SceneDataBlock.SetupBindingPoint(this, 0U);
			bool flag = !this.Deferred && this.UseForwardClusteredLighting;
			if (flag)
			{
				this.PointLightBlock.SetupBindingPoint(this, 2U);
			}
		}

		// Token: 0x0400308E RID: 12430
		public UniformBufferObject SceneDataBlock;

		// Token: 0x0400308F RID: 12431
		public UniformBufferObject PointLightBlock;

		// Token: 0x04003090 RID: 12432
		public Uniform ModelMatrix;

		// Token: 0x04003091 RID: 12433
		protected Uniform LightGridTexture;

		// Token: 0x04003092 RID: 12434
		protected Uniform LightIndicesOrDataBufferTexture;

		// Token: 0x04003093 RID: 12435
		public readonly Attrib AttribPositionAndDoubleSidedAndBlockId;

		// Token: 0x04003094 RID: 12436
		public readonly Attrib AttribTexCoords;

		// Token: 0x04003095 RID: 12437
		public readonly Attrib AttribDataPacked;

		// Token: 0x04003096 RID: 12438
		public float LODDistance = 160f;

		// Token: 0x04003097 RID: 12439
		public bool UseLOD;

		// Token: 0x04003098 RID: 12440
		public bool UseFoliageFading = true;

		// Token: 0x04003099 RID: 12441
		public bool UseDebugBoundaries;

		// Token: 0x0400309A RID: 12442
		public bool Deferred;

		// Token: 0x0400309B RID: 12443
		public bool UseLightBufferCompression;

		// Token: 0x0400309C RID: 12444
		public bool UseForwardClusteredLighting = true;

		// Token: 0x0400309D RID: 12445
		public bool UseLightDirectAccess = true;

		// Token: 0x0400309E RID: 12446
		public bool UseCustomZDistribution = true;

		// Token: 0x0400309F RID: 12447
		public bool WriteRenderConfigBitsInAlpha = true;

		// Token: 0x040030A0 RID: 12448
		public bool UseForwardSunShadows;

		// Token: 0x040030A1 RID: 12449
		public bool UseLinearZ;

		// Token: 0x040030A2 RID: 12450
		public uint SunShadowCascadeCount = 1U;

		// Token: 0x040030A3 RID: 12451
		public bool UseCloudsShadows;

		// Token: 0x040030A4 RID: 12452
		public bool UseUnderwaterCaustics = true;

		// Token: 0x040030A5 RID: 12453
		public bool UseSkyAmbient = true;

		// Token: 0x040030A6 RID: 12454
		public bool UseDithering;

		// Token: 0x040030A7 RID: 12455
		public bool UseSmoothNearMoodColor;

		// Token: 0x040030A8 RID: 12456
		public bool UseMoodFog;

		// Token: 0x040030A9 RID: 12457
		public bool UseFog;

		// Token: 0x040030AA RID: 12458
		public bool UseOIT;

		// Token: 0x040030AB RID: 12459
		private readonly bool _alphaTest;

		// Token: 0x040030AC RID: 12460
		private readonly bool _alphaBlend;

		// Token: 0x040030AD RID: 12461
		private readonly bool _near;
	}
}
