using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A79 RID: 2681
	internal class OITCompositeProgram : GPUProgram
	{
		// Token: 0x060054C0 RID: 21696 RVA: 0x00186349 File Offset: 0x00184549
		public OITCompositeProgram() : base("ScreenVS.glsl", "OITCompositeFS.glsl", null)
		{
		}

		// Token: 0x060054C1 RID: 21697 RVA: 0x00186360 File Offset: 0x00184560
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(new Dictionary<string, string>
			{
				{
					"USE_FAR_CORNERS",
					"1"
				}
			});
			Dictionary<string, string> defines = new Dictionary<string, string>();
			uint fragmentShader = base.CompileFragmentShader(defines);
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x060054C2 RID: 21698 RVA: 0x001863B4 File Offset: 0x001845B4
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.AccumulationQuarterResTexture.SetValue(6);
			this.RevealAddQuarterResTexture.SetValue(5);
			this.AccumulationHalfResTexture.SetValue(4);
			this.RevealAddHalfResTexture.SetValue(3);
			this.BackgroundTexture.SetValue(2);
			this.RevealAddTexture.SetValue(1);
			this.AccumulationTexture.SetValue(0);
		}

		// Token: 0x040030B5 RID: 12469
		public Uniform OITMethod;

		// Token: 0x040030B6 RID: 12470
		public Uniform InputResolutionUsed;

		// Token: 0x040030B7 RID: 12471
		private Uniform AccumulationQuarterResTexture;

		// Token: 0x040030B8 RID: 12472
		private Uniform RevealAddQuarterResTexture;

		// Token: 0x040030B9 RID: 12473
		private Uniform AccumulationHalfResTexture;

		// Token: 0x040030BA RID: 12474
		private Uniform RevealAddHalfResTexture;

		// Token: 0x040030BB RID: 12475
		private Uniform AccumulationTexture;

		// Token: 0x040030BC RID: 12476
		private Uniform RevealAddTexture;

		// Token: 0x040030BD RID: 12477
		private Uniform BackgroundTexture;
	}
}
