using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A60 RID: 2656
	internal class DeferredShadowProgram : GPUProgram
	{
		// Token: 0x06005459 RID: 21593 RVA: 0x0018369C File Offset: 0x0018189C
		public DeferredShadowProgram() : base("ScreenVS.glsl", "DeferredShadowFS.glsl", null)
		{
		}

		// Token: 0x0600545A RID: 21594 RVA: 0x001836FC File Offset: 0x001818FC
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
			uint fragmentShader = base.CompileFragmentShader(new Dictionary<string, string>
			{
				{
					"USE_LINEAR_Z",
					this.UseLinearZ ? "1" : "0"
				},
				{
					"USE_NOISE",
					this.UseNoise ? "1" : "0"
				},
				{
					"USE_CAMERA_BIAS",
					this.UseCameraBias ? "1" : "0"
				},
				{
					"USE_NORMAL_BIAS",
					this.UseNormalBias ? "1" : "0"
				},
				{
					"USE_FADING",
					this.UseFading ? "1" : "0"
				},
				{
					"USE_MANUAL_MODE",
					this.UseManualMode ? "1" : "0"
				},
				{
					"USE_SINGLE_SAMPLE",
					this.UseSingleSample ? "1" : "0"
				},
				{
					"USE_CLEAN_BACKFACES",
					this.UseCleanBackfaces ? "1" : "0"
				},
				{
					"CASCADE_COUNT",
					this.CascadeCount.ToString()
				},
				{
					"INPUT_NORMALS_IN_WS",
					this.HasInputNormalsInWorldSpace ? "1" : "0"
				}
			});
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x0600545B RID: 21595 RVA: 0x00183888 File Offset: 0x00181A88
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.GBuffer1Texture.SetValue(3);
			this.GBuffer0Texture.SetValue(2);
			this.ShadowMap.SetValue(1);
			this.DepthTexture.SetValue(0);
			this.SceneDataBlock.SetupBindingPoint(this, 0U);
		}

		// Token: 0x04002FCA RID: 12234
		public UniformBufferObject SceneDataBlock;

		// Token: 0x04002FCB RID: 12235
		public Uniform FarCorners;

		// Token: 0x04002FCC RID: 12236
		private Uniform ShadowMap;

		// Token: 0x04002FCD RID: 12237
		private Uniform DepthTexture;

		// Token: 0x04002FCE RID: 12238
		private Uniform GBuffer0Texture;

		// Token: 0x04002FCF RID: 12239
		private Uniform GBuffer1Texture;

		// Token: 0x04002FD0 RID: 12240
		public bool UseLinearZ;

		// Token: 0x04002FD1 RID: 12241
		public bool UseNoise = true;

		// Token: 0x04002FD2 RID: 12242
		public bool UseManualMode = false;

		// Token: 0x04002FD3 RID: 12243
		public bool UseFading = false;

		// Token: 0x04002FD4 RID: 12244
		public bool UseSingleSample = true;

		// Token: 0x04002FD5 RID: 12245
		public bool UseCameraBias = false;

		// Token: 0x04002FD6 RID: 12246
		public bool UseNormalBias = true;

		// Token: 0x04002FD7 RID: 12247
		public bool UseCleanBackfaces = false;

		// Token: 0x04002FD8 RID: 12248
		public bool HasInputNormalsInWorldSpace = true;

		// Token: 0x04002FD9 RID: 12249
		public uint CascadeCount = 1U;
	}
}
