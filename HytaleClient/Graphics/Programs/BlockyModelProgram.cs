using System;
using System.Collections.Generic;
using HytaleClient.Data.BlockyModels;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A56 RID: 2646
	internal class BlockyModelProgram : GPUProgram
	{
		// Token: 0x0600543B RID: 21563 RVA: 0x00182740 File Offset: 0x00180940
		public BlockyModelProgram(bool useDeferred, bool useSceneDataOverride, bool useCompleteForwardVersion, bool firstPersonView = false, bool useEntityDataBuffer = false, bool useDistortionRT = false, string variationName = null) : base("BlockyModelVS.glsl", "BlockyModelFS.glsl", variationName)
		{
			this.Deferred = (!useDistortionRT && useDeferred);
			this._useSceneDataOverride = useSceneDataOverride;
			this._firstPersonView = firstPersonView;
			this._useCompleteForwardVersion = (!useDistortionRT && !useDeferred && useCompleteForwardVersion);
			this._useForwardClusteredLighting = this._useCompleteForwardVersion;
			this._useEntityDataBuffer = useEntityDataBuffer;
			this._useDistortionRT = useDistortionRT;
		}

		// Token: 0x0600543C RID: 21564 RVA: 0x001827C4 File Offset: 0x001809C4
		public override bool Initialize()
		{
			base.Initialize();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("DEFERRED", this.Deferred ? "1" : "0");
			dictionary.Add("MAX_NODES_COUNT", BlockyModel.MaxNodeCount.ToString());
			dictionary.Add("COMPLETE_VERSION", this._useCompleteForwardVersion ? "1" : "0");
			dictionary.Add("FIRST_PERSON_VIEW", this._firstPersonView ? "1" : "0");
			dictionary.Add("USE_CLUSTERED_LIGHTING", (!this.Deferred && this._useForwardClusteredLighting) ? "1" : "0");
			dictionary.Add("USE_SCENE_DATA_OVERRIDE", this._useSceneDataOverride ? "1" : "0");
			dictionary.Add("USE_ENTITY_DATA_BUFFER", this._useEntityDataBuffer ? "1" : "0");
			dictionary.Add("USE_DISTORTION_RT", this._useDistortionRT ? "1" : "0");
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			dictionary2.Add("DEFERRED", this.Deferred ? "1" : "0");
			dictionary2.Add("USE_LBUFFER_COMPRESSION", this.UseLightBufferCompression ? "1" : "0");
			dictionary2.Add("COMPLETE_VERSION", this._useCompleteForwardVersion ? "1" : "0");
			dictionary2.Add("FIRST_PERSON_VIEW", this._firstPersonView ? "1" : "0");
			dictionary2.Add("USE_CLUSTERED_LIGHTING", (!this.Deferred && this._useForwardClusteredLighting) ? "1" : "0");
			dictionary2.Add("USE_DIRECT_ACCESS", this.UseLightDirectAccess ? "1" : "0");
			dictionary2.Add("USE_CUSTOM_Z_DISTRIBUTION", this.UseCustomZDistribution ? "1" : "0");
			dictionary2.Add("USE_SCENE_DATA_OVERRIDE", this._useSceneDataOverride ? "1" : "0");
			dictionary2.Add("USE_ENTITY_DATA_BUFFER", this._useEntityDataBuffer ? "1" : "0");
			dictionary2.Add("USE_DISTORTION_RT", this._useDistortionRT ? "1" : "0");
			foreach (ShadingMode shadingMode in (ShadingMode[])Enum.GetValues(typeof(ShadingMode)))
			{
				string key = "SHADING_" + shadingMode.ToString().ToUpper();
				int num = (int)shadingMode;
				string value = num.ToString();
				dictionary.Add(key, value);
				dictionary2.Add(key, value);
			}
			uint vertexShader = base.CompileVertexShader(dictionary);
			uint fragmentShader = base.CompileFragmentShader(dictionary2);
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x0600543D RID: 21565 RVA: 0x00182AC0 File Offset: 0x00180CC0
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.Texture0.SetValue(0);
			this.Texture1.SetValue(1);
			this.Texture2.SetValue(2);
			this.GradientAtlasTexture.SetValue(3);
			this.NoiseTexture.SetValue(4);
			this.EntityDataBuffer.SetValue(5);
			this.ModelVFXDataBuffer.SetValue(6);
			this.SceneDataBlock.SetupBindingPoint(this, 0U);
			this.PointLightBlock.SetupBindingPoint(this, 2U);
			this.NodeBlock.SetupBindingPoint(this, 5U);
		}

		// Token: 0x04002F31 RID: 12081
		public UniformBufferObject SceneDataBlock;

		// Token: 0x04002F32 RID: 12082
		public UniformBufferObject NodeBlock;

		// Token: 0x04002F33 RID: 12083
		public UniformBufferObject PointLightBlock;

		// Token: 0x04002F34 RID: 12084
		public Uniform DrawId;

		// Token: 0x04002F35 RID: 12085
		public Uniform CurrentInvViewportSize;

		// Token: 0x04002F36 RID: 12086
		public Uniform InvModelHeight;

		// Token: 0x04002F37 RID: 12087
		public Uniform ModelMatrix;

		// Token: 0x04002F38 RID: 12088
		public Uniform StaticLightColor;

		// Token: 0x04002F39 RID: 12089
		public Uniform BottomTint;

		// Token: 0x04002F3A RID: 12090
		public Uniform TopTint;

		// Token: 0x04002F3B RID: 12091
		public Uniform UseDithering;

		// Token: 0x04002F3C RID: 12092
		public Uniform ModelVFXAnimationProgress;

		// Token: 0x04002F3D RID: 12093
		public Uniform ModelVFXHighlightColorAndThickness;

		// Token: 0x04002F3E RID: 12094
		public Uniform ModelVFXNoiseParams;

		// Token: 0x04002F3F RID: 12095
		public Uniform ModelVFXPackedParams;

		// Token: 0x04002F40 RID: 12096
		public Uniform ModelVFXPostColor;

		// Token: 0x04002F41 RID: 12097
		public Uniform AtlasSizeFactor0;

		// Token: 0x04002F42 RID: 12098
		public Uniform AtlasSizeFactor1;

		// Token: 0x04002F43 RID: 12099
		public Uniform AtlasSizeFactor2;

		// Token: 0x04002F44 RID: 12100
		public Uniform ViewMatrix;

		// Token: 0x04002F45 RID: 12101
		public Uniform ViewProjectionMatrix;

		// Token: 0x04002F46 RID: 12102
		public Uniform NearScreendoorThreshold;

		// Token: 0x04002F47 RID: 12103
		private Uniform Texture0;

		// Token: 0x04002F48 RID: 12104
		private Uniform Texture1;

		// Token: 0x04002F49 RID: 12105
		private Uniform Texture2;

		// Token: 0x04002F4A RID: 12106
		private Uniform GradientAtlasTexture;

		// Token: 0x04002F4B RID: 12107
		private Uniform NoiseTexture;

		// Token: 0x04002F4C RID: 12108
		private Uniform EntityDataBuffer;

		// Token: 0x04002F4D RID: 12109
		private Uniform ModelVFXDataBuffer;

		// Token: 0x04002F4E RID: 12110
		private Uniform LightGridTexture;

		// Token: 0x04002F4F RID: 12111
		private Uniform LightIndicesBufferTexture;

		// Token: 0x04002F50 RID: 12112
		private Uniform LightBufferTexture;

		// Token: 0x04002F51 RID: 12113
		public readonly Attrib AttribNodeIndex;

		// Token: 0x04002F52 RID: 12114
		public readonly Attrib AttribAtlasIndexAndShadingModeAndGradientId;

		// Token: 0x04002F53 RID: 12115
		public readonly Attrib AttribPosition;

		// Token: 0x04002F54 RID: 12116
		public readonly Attrib AttribTexCoords;

		// Token: 0x04002F55 RID: 12117
		public bool Deferred;

		// Token: 0x04002F56 RID: 12118
		public bool UseLightBufferCompression;

		// Token: 0x04002F57 RID: 12119
		public bool UseLightDirectAccess = true;

		// Token: 0x04002F58 RID: 12120
		public bool UseCustomZDistribution = true;

		// Token: 0x04002F59 RID: 12121
		private readonly bool _useForwardClusteredLighting = true;

		// Token: 0x04002F5A RID: 12122
		private readonly bool _useEntityDataBuffer;

		// Token: 0x04002F5B RID: 12123
		private readonly bool _useDistortionRT;

		// Token: 0x04002F5C RID: 12124
		private readonly bool _useCompleteForwardVersion;

		// Token: 0x04002F5D RID: 12125
		private readonly bool _firstPersonView;

		// Token: 0x04002F5E RID: 12126
		private readonly bool _useSceneDataOverride;
	}
}
