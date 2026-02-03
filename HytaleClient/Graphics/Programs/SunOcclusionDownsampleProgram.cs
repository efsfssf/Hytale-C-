using System;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A82 RID: 2690
	internal class SunOcclusionDownsampleProgram : GPUProgram
	{
		// Token: 0x060054DB RID: 21723 RVA: 0x00187107 File Offset: 0x00185307
		public SunOcclusionDownsampleProgram() : base("ScreenVS.glsl", "SunOcclusionDownsampleFS.glsl", null)
		{
		}

		// Token: 0x060054DC RID: 21724 RVA: 0x0018711C File Offset: 0x0018531C
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(null);
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x060054DD RID: 21725 RVA: 0x00187150 File Offset: 0x00185350
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.DepthTexture.SetValue(1);
			this.ColorTexture.SetValue(0);
		}

		// Token: 0x04003145 RID: 12613
		public Uniform CameraPosition;

		// Token: 0x04003146 RID: 12614
		public Uniform CameraDirection;

		// Token: 0x04003147 RID: 12615
		private Uniform ColorTexture;

		// Token: 0x04003148 RID: 12616
		private Uniform DepthTexture;
	}
}
