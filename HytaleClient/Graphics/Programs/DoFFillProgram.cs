using System;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A65 RID: 2661
	internal class DoFFillProgram : GPUProgram
	{
		// Token: 0x06005468 RID: 21608 RVA: 0x00183B1A File Offset: 0x00181D1A
		public DoFFillProgram() : base("ScreenVS.glsl", "DoFFillFS.glsl", null)
		{
		}

		// Token: 0x06005469 RID: 21609 RVA: 0x00183B30 File Offset: 0x00181D30
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(null);
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x0600546A RID: 21610 RVA: 0x00183B64 File Offset: 0x00181D64
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.CoCLowResTexture.SetValue(0);
			this.NearCoCBlurredLowResTexture.SetValue(1);
			this.NearFieldLowResTexture.SetValue(2);
			this.FarFieldLowResTexture.SetValue(3);
		}

		// Token: 0x04002FF5 RID: 12277
		public Uniform PixelSize;

		// Token: 0x04002FF6 RID: 12278
		private Uniform CoCLowResTexture;

		// Token: 0x04002FF7 RID: 12279
		private Uniform NearCoCBlurredLowResTexture;

		// Token: 0x04002FF8 RID: 12280
		private Uniform NearFieldLowResTexture;

		// Token: 0x04002FF9 RID: 12281
		private Uniform FarFieldLowResTexture;
	}
}
