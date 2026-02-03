using System;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A72 RID: 2674
	internal class LinearZProgram : GPUProgram
	{
		// Token: 0x060054AE RID: 21678 RVA: 0x001854B1 File Offset: 0x001836B1
		public LinearZProgram() : base("ScreenVS.glsl", "LinearZFS.glsl", null)
		{
		}

		// Token: 0x060054AF RID: 21679 RVA: 0x001854D0 File Offset: 0x001836D0
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(null);
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x060054B0 RID: 21680 RVA: 0x00185504 File Offset: 0x00183704
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.DepthTexture.SetValue(0);
		}

		// Token: 0x04003064 RID: 12388
		public Uniform ProjectionMatrix;

		// Token: 0x04003065 RID: 12389
		public Uniform InvFarClip;

		// Token: 0x04003066 RID: 12390
		private Uniform DepthTexture;

		// Token: 0x04003067 RID: 12391
		public bool UseFullscreenTriangle = true;
	}
}
