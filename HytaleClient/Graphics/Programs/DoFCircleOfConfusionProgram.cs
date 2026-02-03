using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A63 RID: 2659
	internal class DoFCircleOfConfusionProgram : GPUProgram
	{
		// Token: 0x06005462 RID: 21602 RVA: 0x00183A0D File Offset: 0x00181C0D
		public DoFCircleOfConfusionProgram() : base("ScreenVS.glsl", "DoFCircleOfConfusionFS.glsl", null)
		{
		}

		// Token: 0x06005463 RID: 21603 RVA: 0x00183A24 File Offset: 0x00181C24
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(new Dictionary<string, string>
			{
				{
					"USE_LINEAR_Z",
					this.UseLinearZ ? "1" : "0"
				}
			});
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x06005464 RID: 21604 RVA: 0x00183A7E File Offset: 0x00181C7E
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.DepthTexture.SetValue(0);
		}

		// Token: 0x04002FE9 RID: 12265
		public Uniform ProjectionMatrix;

		// Token: 0x04002FEA RID: 12266
		public Uniform FarClip;

		// Token: 0x04002FEB RID: 12267
		public Uniform NearBlurry;

		// Token: 0x04002FEC RID: 12268
		public Uniform NearSharp;

		// Token: 0x04002FED RID: 12269
		public Uniform FarSharp;

		// Token: 0x04002FEE RID: 12270
		public Uniform FarBlurry;

		// Token: 0x04002FEF RID: 12271
		private Uniform DepthTexture;

		// Token: 0x04002FF0 RID: 12272
		public bool UseLinearZ;
	}
}
