using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A87 RID: 2695
	internal class VolumetricSunshaftProgram : GPUProgram
	{
		// Token: 0x0600550B RID: 21771 RVA: 0x00187BF8 File Offset: 0x00185DF8
		public VolumetricSunshaftProgram() : base("ScreenVS.glsl", "VolumetricSunshaftFS.glsl", null)
		{
		}

		// Token: 0x0600550C RID: 21772 RVA: 0x00187C1C File Offset: 0x00185E1C
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
					"USE_MANUAL_MODE",
					this.UseManualMode ? "1" : "0"
				},
				{
					"CASCADE_COUNT",
					this.CascadeCount.ToString()
				}
			});
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x0600550D RID: 21773 RVA: 0x00187CA8 File Offset: 0x00185EA8
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.GBuffer0Texture.SetValue(2);
			this.ShadowMap.SetValue(1);
			this.DepthTexture.SetValue(0);
			this.SceneDataBlock.SetupBindingPoint(this, 0U);
		}

		// Token: 0x04003166 RID: 12646
		public UniformBufferObject SceneDataBlock;

		// Token: 0x04003167 RID: 12647
		public Uniform FarCorners;

		// Token: 0x04003168 RID: 12648
		public Uniform SunDirection;

		// Token: 0x04003169 RID: 12649
		public Uniform SunColor;

		// Token: 0x0400316A RID: 12650
		private Uniform ShadowMap;

		// Token: 0x0400316B RID: 12651
		private Uniform DepthTexture;

		// Token: 0x0400316C RID: 12652
		private Uniform GBuffer0Texture;

		// Token: 0x0400316D RID: 12653
		public bool UseManualMode = false;

		// Token: 0x0400316E RID: 12654
		public uint CascadeCount = 1U;
	}
}
