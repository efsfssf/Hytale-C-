using System;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A6D RID: 2669
	internal class HiZFillHoleProgram : GPUProgram
	{
		// Token: 0x060054A0 RID: 21664 RVA: 0x0018510E File Offset: 0x0018330E
		public HiZFillHoleProgram() : base("ScreenVS.glsl", "HiZFillHoleFS.glsl", null)
		{
		}

		// Token: 0x060054A1 RID: 21665 RVA: 0x00185124 File Offset: 0x00183324
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(null);
			return base.MakeProgram(vertexShader, fragmentShader, null, false, null);
		}

		// Token: 0x060054A2 RID: 21666 RVA: 0x00185158 File Offset: 0x00183358
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.DepthTexture.SetValue(0);
		}

		// Token: 0x0400303B RID: 12347
		private Uniform DepthTexture;
	}
}
