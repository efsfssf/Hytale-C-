using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A66 RID: 2662
	internal abstract class DoFNearCoCBaseProgram : GPUProgram
	{
		// Token: 0x0600546B RID: 21611 RVA: 0x00183BB2 File Offset: 0x00181DB2
		public DoFNearCoCBaseProgram(string vertexShaderFileName, string fragmentShaderFileName) : base(vertexShaderFileName, fragmentShaderFileName, null)
		{
		}

		// Token: 0x0600546C RID: 21612 RVA: 0x00183BC8 File Offset: 0x00181DC8
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(new Dictionary<string, string>
			{
				{
					"USE_MVP_MATRIX",
					(!this.UseFullscreenTriangle) ? "1" : "0"
				},
				{
					"USE_VBO_ATTRIBUTES",
					(!this.UseFullscreenTriangle) ? "1" : "0"
				},
				{
					"USE_FULLSCREEN_TRIANGLE",
					this.UseFullscreenTriangle ? "1" : "0"
				}
			});
			Dictionary<string, string> defines = new Dictionary<string, string>();
			uint fragmentShader = base.CompileFragmentShader(defines);
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x0600546D RID: 21613 RVA: 0x00183C6A File Offset: 0x00181E6A
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.CoCTexture.SetValue(0);
		}

		// Token: 0x04002FFA RID: 12282
		public Uniform MVPMatrix;

		// Token: 0x04002FFB RID: 12283
		public Uniform PixelSize;

		// Token: 0x04002FFC RID: 12284
		public Uniform HorizontalPass;

		// Token: 0x04002FFD RID: 12285
		protected Uniform CoCTexture;

		// Token: 0x04002FFE RID: 12286
		public bool UseFullscreenTriangle = true;
	}
}
