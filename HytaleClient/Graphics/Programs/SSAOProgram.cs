using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A81 RID: 2689
	internal class SSAOProgram : GPUProgram
	{
		// Token: 0x060054D8 RID: 21720 RVA: 0x00186FD3 File Offset: 0x001851D3
		public SSAOProgram() : base("ScreenVS.glsl", "SSAOFS.glsl", null)
		{
		}

		// Token: 0x060054D9 RID: 21721 RVA: 0x00187000 File Offset: 0x00185200
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
			uint fragmentShader = base.CompileFragmentShader(new Dictionary<string, string>
			{
				{
					"SAMPLES_COUNT",
					this.SamplesCount.ToString()
				},
				{
					"USE_TEMPORAL_FILTERING",
					this.UseTemporalFiltering ? "1" : "0"
				},
				{
					"INPUT_NORMALS_IN_WS",
					this.HasInputNormalsInWorldSpace ? "1" : "0"
				}
			});
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x060054DA RID: 21722 RVA: 0x001870AC File Offset: 0x001852AC
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.ShadowTexture.SetValue(4);
			this.SSAOCacheTexture.SetValue(3);
			this.GBufferTexture.SetValue(2);
			this.TapsSourceTexture.SetValue(1);
			this.DepthTexture.SetValue(0);
		}

		// Token: 0x04003135 RID: 12597
		public Uniform PackedParameters;

		// Token: 0x04003136 RID: 12598
		public Uniform ViewportSize;

		// Token: 0x04003137 RID: 12599
		public Uniform ProjectionMatrix;

		// Token: 0x04003138 RID: 12600
		public Uniform ViewMatrix;

		// Token: 0x04003139 RID: 12601
		public Uniform ReprojectMatrix;

		// Token: 0x0400313A RID: 12602
		public Uniform FarCorners;

		// Token: 0x0400313B RID: 12603
		public Uniform SamplesData;

		// Token: 0x0400313C RID: 12604
		public Uniform TemporalSampleOffset;

		// Token: 0x0400313D RID: 12605
		private Uniform DepthTexture;

		// Token: 0x0400313E RID: 12606
		private Uniform TapsSourceTexture;

		// Token: 0x0400313F RID: 12607
		private Uniform GBufferTexture;

		// Token: 0x04003140 RID: 12608
		private Uniform SSAOCacheTexture;

		// Token: 0x04003141 RID: 12609
		private Uniform ShadowTexture;

		// Token: 0x04003142 RID: 12610
		public int SamplesCount = 8;

		// Token: 0x04003143 RID: 12611
		public bool UseTemporalFiltering = true;

		// Token: 0x04003144 RID: 12612
		public bool HasInputNormalsInWorldSpace = true;
	}
}
