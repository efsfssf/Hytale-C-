using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A7D RID: 2685
	internal class RadialGlowMaskProgram : GPUProgram
	{
		// Token: 0x060054CD RID: 21709 RVA: 0x00186D2C File Offset: 0x00184F2C
		public RadialGlowMaskProgram() : base("RadialGlowMaskVS.glsl", "RadialGlowMaskFS.glsl", null)
		{
		}

		// Token: 0x060054CE RID: 21710 RVA: 0x00186D44 File Offset: 0x00184F44
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			Dictionary<string, string> defines = new Dictionary<string, string>();
			uint fragmentShader = base.CompileFragmentShader(defines);
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x060054CF RID: 21711 RVA: 0x00186D7E File Offset: 0x00184F7E
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.DepthTexture.SetValue(2);
			this.GlowMaskTexture.SetValue(1);
			this.SceneColorTexture.SetValue(0);
		}

		// Token: 0x04003117 RID: 12567
		public Uniform MVPMatrix;

		// Token: 0x04003118 RID: 12568
		public Uniform SunMVPMatrix;

		// Token: 0x04003119 RID: 12569
		public Uniform ProjectionMatrix;

		// Token: 0x0400311A RID: 12570
		private Uniform SceneColorTexture;

		// Token: 0x0400311B RID: 12571
		private Uniform GlowMaskTexture;

		// Token: 0x0400311C RID: 12572
		private Uniform DepthTexture;
	}
}
