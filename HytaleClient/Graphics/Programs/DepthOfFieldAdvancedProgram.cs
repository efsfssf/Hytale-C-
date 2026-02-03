using System;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A61 RID: 2657
	internal class DepthOfFieldAdvancedProgram : GPUProgram
	{
		// Token: 0x0600545C RID: 21596 RVA: 0x001838E4 File Offset: 0x00181AE4
		public DepthOfFieldAdvancedProgram() : base("ScreenVS.glsl", "DepthOfFieldAdvancedFS.glsl", null)
		{
		}

		// Token: 0x0600545D RID: 21597 RVA: 0x001838FC File Offset: 0x00181AFC
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(null);
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x0600545E RID: 21598 RVA: 0x00183930 File Offset: 0x00181B30
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.SceneColorLowResTexture.SetValue(0);
			this.ColorMulFarCoCLowResTextureLinear.SetValue(1);
			this.CoCLowResTextureLinear.SetValue(2);
			this.ColorMulFarCoCLowResTexturePoint.SetValue(3);
			this.CoCLowResTexturePoint.SetValue(4);
			this.NearCoCBlurredLowResTexture.SetValue(5);
		}

		// Token: 0x04002FDA RID: 12250
		public Uniform PixelSize;

		// Token: 0x04002FDB RID: 12251
		public Uniform FarBlurMax;

		// Token: 0x04002FDC RID: 12252
		public Uniform NearBlurMax;

		// Token: 0x04002FDD RID: 12253
		private Uniform SceneColorLowResTexture;

		// Token: 0x04002FDE RID: 12254
		private Uniform ColorMulFarCoCLowResTextureLinear;

		// Token: 0x04002FDF RID: 12255
		private Uniform CoCLowResTextureLinear;

		// Token: 0x04002FE0 RID: 12256
		private Uniform ColorMulFarCoCLowResTexturePoint;

		// Token: 0x04002FE1 RID: 12257
		private Uniform CoCLowResTexturePoint;

		// Token: 0x04002FE2 RID: 12258
		private Uniform NearCoCBlurredLowResTexture;
	}
}
