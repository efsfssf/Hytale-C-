using System;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A83 RID: 2691
	internal class TemporalAAProgram : GPUProgram
	{
		// Token: 0x060054DE RID: 21726 RVA: 0x00187179 File Offset: 0x00185379
		public TemporalAAProgram() : base("ScreenVS.glsl", "TemporalAAFS.glsl", null)
		{
		}

		// Token: 0x060054DF RID: 21727 RVA: 0x00187190 File Offset: 0x00185390
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(null);
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x060054E0 RID: 21728 RVA: 0x001871C4 File Offset: 0x001853C4
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.ColorTexture.SetValue(0);
			this.PreviousColorTexture.SetValue(1);
		}

		// Token: 0x04003149 RID: 12617
		public Uniform PixelSize;

		// Token: 0x0400314A RID: 12618
		public Uniform NeighborHoodCheck;

		// Token: 0x0400314B RID: 12619
		private Uniform ColorTexture;

		// Token: 0x0400314C RID: 12620
		private Uniform PreviousColorTexture;
	}
}
