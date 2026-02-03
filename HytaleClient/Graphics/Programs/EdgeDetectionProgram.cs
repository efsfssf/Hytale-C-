using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A68 RID: 2664
	internal class EdgeDetectionProgram : GPUProgram
	{
		// Token: 0x0600546F RID: 21615 RVA: 0x00183C9A File Offset: 0x00181E9A
		public EdgeDetectionProgram() : base("ScreenVS.glsl", "EdgeDetectionFS.glsl", null)
		{
		}

		// Token: 0x06005470 RID: 21616 RVA: 0x00183CB0 File Offset: 0x00181EB0
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

		// Token: 0x06005471 RID: 21617 RVA: 0x00183D0A File Offset: 0x00181F0A
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.DepthTexture.SetValue(0);
		}

		// Token: 0x04002FFF RID: 12287
		public Uniform ProjectionMatrix;

		// Token: 0x04003000 RID: 12288
		public Uniform InvDepthTextureSize;

		// Token: 0x04003001 RID: 12289
		public Uniform FarClip;

		// Token: 0x04003002 RID: 12290
		private Uniform DepthTexture;

		// Token: 0x04003003 RID: 12291
		public bool UseLinearZ;
	}
}
