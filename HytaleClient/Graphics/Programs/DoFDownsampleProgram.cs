using System;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A64 RID: 2660
	internal class DoFDownsampleProgram : GPUProgram
	{
		// Token: 0x06005465 RID: 21605 RVA: 0x00183A9A File Offset: 0x00181C9A
		public DoFDownsampleProgram() : base("ScreenVS.glsl", "DoFDownsampleFS.glsl", null)
		{
		}

		// Token: 0x06005466 RID: 21606 RVA: 0x00183AB0 File Offset: 0x00181CB0
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(null);
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x06005467 RID: 21607 RVA: 0x00183AE4 File Offset: 0x00181CE4
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.ColorTextureLinear.SetValue(0);
			this.ColorTexturePoint.SetValue(1);
			this.CoCTexture.SetValue(2);
		}

		// Token: 0x04002FF1 RID: 12273
		public Uniform PixelSize;

		// Token: 0x04002FF2 RID: 12274
		private Uniform CoCTexture;

		// Token: 0x04002FF3 RID: 12275
		private Uniform ColorTextureLinear;

		// Token: 0x04002FF4 RID: 12276
		private Uniform ColorTexturePoint;
	}
}
