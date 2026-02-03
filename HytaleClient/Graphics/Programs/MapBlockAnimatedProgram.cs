using System;
using System.Collections.Generic;
using HytaleClient.Data.Map;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A73 RID: 2675
	internal class MapBlockAnimatedProgram : GPUProgram
	{
		// Token: 0x060054B1 RID: 21681 RVA: 0x00185520 File Offset: 0x00183720
		public MapBlockAnimatedProgram(int maxNodeCount, bool useDeferred, bool useSceneDataOverride, bool writeRenderConfigBitsInAlpha, string variationName = null) : base("MapChunkVS.glsl", "MapChunkFS.glsl", variationName)
		{
			this.MaxNodeCount = maxNodeCount;
			this.Deferred = useDeferred;
			this.WriteRenderConfigBitsInAlpha = writeRenderConfigBitsInAlpha;
			this._useSceneDataOverride = useSceneDataOverride;
			this.UseFog = (this.UseMoodFog = this.Deferred);
		}

		// Token: 0x060054B2 RID: 21682 RVA: 0x00185584 File Offset: 0x00183784
		public override bool Initialize()
		{
			base.Initialize();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("DEFERRED", this.Deferred ? "1" : "0");
			dictionary.Add("ALPHA_TEST", "1");
			dictionary.Add("ALPHA_BLEND", "0");
			dictionary.Add("NEAR", "1");
			dictionary.Add("ANIMATED", "1");
			dictionary.Add("MAX_NODES_COUNT", this.MaxNodeCount.ToString());
			dictionary.Add("USE_LOD", "0");
			dictionary.Add("DEBUG_BOUNDARIES", this.UseDebugBoundaries ? "1" : "0");
			dictionary.Add("USE_SCENE_DATA_OVERRIDE", this._useSceneDataOverride ? "1" : "0");
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			dictionary2.Add("DEFERRED", this.Deferred ? "1" : "0");
			dictionary2.Add("USE_LBUFFER_COMPRESSION", this.UseLightBufferCompression ? "1" : "0");
			dictionary2.Add("ALPHA_TEST", "1");
			dictionary2.Add("ALPHA_BLEND", "0");
			dictionary2.Add("NEAR", "1");
			dictionary2.Add("ANIMATED", "1");
			dictionary2.Add("USE_CLUSTERED_LIGHTING", "0");
			dictionary2.Add("WRITE_RENDERCONFIG_IN_ALPHA", (!this.Deferred && this.WriteRenderConfigBitsInAlpha) ? "1" : "0");
			dictionary2.Add("USE_FOG_DITHERING", this.UseDithering ? "1" : "0");
			dictionary2.Add("USE_SMOOTH_NEAR_MOOD_FOG_COLOR", this.UseSmoothNearMoodColor ? "1" : "0");
			dictionary2.Add("USE_MOOD_FOG", this.UseMoodFog ? "1" : "0");
			dictionary2.Add("USE_FOG", this.UseFog ? "1" : "0");
			dictionary2.Add("DEBUG_OCCLUSION_CULLING", "0");
			dictionary2.Add("USE_SCENE_DATA_OVERRIDE", this._useSceneDataOverride ? "1" : "0");
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

		// Token: 0x060054B3 RID: 21683 RVA: 0x0018592E File Offset: 0x00183B2E
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.SceneDataBlock.SetupBindingPoint(this, 0U);
			this.NodeBlock.SetupBindingPoint(this, 5U);
		}

		// Token: 0x04003068 RID: 12392
		public UniformBufferObject SceneDataBlock;

		// Token: 0x04003069 RID: 12393
		public UniformBufferObject NodeBlock;

		// Token: 0x0400306A RID: 12394
		public Uniform ModelMatrix;

		// Token: 0x0400306B RID: 12395
		public Uniform ViewProjectionMatrix;

		// Token: 0x0400306C RID: 12396
		public readonly Attrib AttribPositionAndDoubleSidedAndBlockId;

		// Token: 0x0400306D RID: 12397
		public readonly Attrib AttribTexCoords;

		// Token: 0x0400306E RID: 12398
		public readonly Attrib AttribDataPacked;

		// Token: 0x0400306F RID: 12399
		public bool UseDebugBoundaries;

		// Token: 0x04003070 RID: 12400
		public bool Deferred;

		// Token: 0x04003071 RID: 12401
		public bool UseLightBufferCompression;

		// Token: 0x04003072 RID: 12402
		public bool UseForwardClusteredLighting;

		// Token: 0x04003073 RID: 12403
		public bool UseLightDirectAccess = true;

		// Token: 0x04003074 RID: 12404
		public bool UseCustomZDistribution = true;

		// Token: 0x04003075 RID: 12405
		public bool WriteRenderConfigBitsInAlpha;

		// Token: 0x04003076 RID: 12406
		public bool UseDithering;

		// Token: 0x04003077 RID: 12407
		public bool UseSmoothNearMoodColor;

		// Token: 0x04003078 RID: 12408
		public bool UseMoodFog;

		// Token: 0x04003079 RID: 12409
		public bool UseFog;

		// Token: 0x0400307A RID: 12410
		private readonly int MaxNodeCount;

		// Token: 0x0400307B RID: 12411
		private bool _useSceneDataOverride;
	}
}
