using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A58 RID: 2648
	internal class BloomDownsampleBlurProgram : GPUProgram
	{
		// Token: 0x06005441 RID: 21569 RVA: 0x00182C47 File Offset: 0x00180E47
		public BloomDownsampleBlurProgram() : base("ScreenVS.glsl", "BloomDownsampleBlurFS.glsl", null)
		{
		}

		// Token: 0x06005442 RID: 21570 RVA: 0x00182C5C File Offset: 0x00180E5C
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(new Dictionary<string, string>
			{
				{
					"METHOD",
					this.DownsampleMethod.ToString()
				}
			});
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x06005443 RID: 21571 RVA: 0x00182CAD File Offset: 0x00180EAD
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.ColorTexture.SetValue(0);
		}

		// Token: 0x04002F66 RID: 12134
		public Uniform PixelSize;

		// Token: 0x04002F67 RID: 12135
		private Uniform ColorTexture;

		// Token: 0x04002F68 RID: 12136
		public int DownsampleMethod;
	}
}
