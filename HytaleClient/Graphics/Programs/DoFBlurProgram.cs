using System;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A62 RID: 2658
	internal class DoFBlurProgram : GPUProgram
	{
		// Token: 0x0600545F RID: 21599 RVA: 0x00183998 File Offset: 0x00181B98
		public DoFBlurProgram() : base("ScreenVS.glsl", "DoFBlurFS.glsl", null)
		{
		}

		// Token: 0x06005460 RID: 21600 RVA: 0x001839B0 File Offset: 0x00181BB0
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(null);
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x06005461 RID: 21601 RVA: 0x001839E4 File Offset: 0x00181BE4
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.SceneColorTexture.SetValue(0);
			this.SceneColorTexture2.SetValue(1);
		}

		// Token: 0x04002FE3 RID: 12259
		public Uniform PixelSize;

		// Token: 0x04002FE4 RID: 12260
		public Uniform NearBlurScale;

		// Token: 0x04002FE5 RID: 12261
		public Uniform FarBlurScale;

		// Token: 0x04002FE6 RID: 12262
		public Uniform HorizontalPass;

		// Token: 0x04002FE7 RID: 12263
		private Uniform SceneColorTexture;

		// Token: 0x04002FE8 RID: 12264
		private Uniform SceneColorTexture2;
	}
}
