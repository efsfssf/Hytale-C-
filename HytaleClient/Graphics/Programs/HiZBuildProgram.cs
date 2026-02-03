using System;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A6B RID: 2667
	internal class HiZBuildProgram : GPUProgram
	{
		// Token: 0x0600549A RID: 21658 RVA: 0x0018503D File Offset: 0x0018323D
		public HiZBuildProgram() : base("ScreenVS.glsl", "HiZBuildFS.glsl", null)
		{
		}

		// Token: 0x0600549B RID: 21659 RVA: 0x00185054 File Offset: 0x00183254
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(null);
			return base.MakeProgram(vertexShader, fragmentShader, null, false, null);
		}

		// Token: 0x0600549C RID: 21660 RVA: 0x00185088 File Offset: 0x00183288
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.HiZBuffer.SetValue(0);
		}

		// Token: 0x04003035 RID: 12341
		private Uniform HiZBuffer;
	}
}
