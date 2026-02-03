using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A6E RID: 2670
	internal class HiZReprojectProgram : GPUProgram
	{
		// Token: 0x060054A3 RID: 21667 RVA: 0x00185174 File Offset: 0x00183374
		public HiZReprojectProgram() : base("HiZReprojectVS.glsl", null, null)
		{
			this.MaxInvalidScreenAreas = 10;
		}

		// Token: 0x060054A4 RID: 21668 RVA: 0x00185190 File Offset: 0x00183390
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(new Dictionary<string, string>
			{
				{
					"MAX_INVALID_AREAS",
					this.MaxInvalidScreenAreas.ToString()
				}
			});
			return base.MakeProgram(vertexShader, null, false, null);
		}

		// Token: 0x060054A5 RID: 21669 RVA: 0x001851DB File Offset: 0x001833DB
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.DepthTexture.SetValue(0);
		}

		// Token: 0x0400303C RID: 12348
		public Uniform Resolutions;

		// Token: 0x0400303D RID: 12349
		public Uniform ReprojectMatrix;

		// Token: 0x0400303E RID: 12350
		public Uniform ProjectionMatrix;

		// Token: 0x0400303F RID: 12351
		public Uniform InvalidScreenAreas;

		// Token: 0x04003040 RID: 12352
		private Uniform DepthTexture;

		// Token: 0x04003041 RID: 12353
		public readonly int MaxInvalidScreenAreas;
	}
}
