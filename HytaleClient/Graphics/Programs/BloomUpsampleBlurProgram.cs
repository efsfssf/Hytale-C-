using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A5A RID: 2650
	internal class BloomUpsampleBlurProgram : GPUProgram
	{
		// Token: 0x06005447 RID: 21575 RVA: 0x00182DEF File Offset: 0x00180FEF
		public BloomUpsampleBlurProgram() : base("ScreenVS.glsl", "BloomUpsampleBlurFS.glsl", null)
		{
		}

		// Token: 0x06005448 RID: 21576 RVA: 0x00182E04 File Offset: 0x00181004
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(new Dictionary<string, string>
			{
				{
					"METHOD",
					this.UpsampleMethod.ToString()
				}
			});
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x06005449 RID: 21577 RVA: 0x00182E55 File Offset: 0x00181055
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.ColorTexture.SetValue(0);
			this.ColorLowResTexture.SetValue(1);
		}

		// Token: 0x04002F74 RID: 12148
		public Uniform PixelSize;

		// Token: 0x04002F75 RID: 12149
		public Uniform Scale;

		// Token: 0x04002F76 RID: 12150
		public Uniform Intensity;

		// Token: 0x04002F77 RID: 12151
		private Uniform ColorTexture;

		// Token: 0x04002F78 RID: 12152
		private Uniform ColorLowResTexture;

		// Token: 0x04002F79 RID: 12153
		public int UpsampleMethod;
	}
}
