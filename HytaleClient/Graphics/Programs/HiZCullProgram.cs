using System;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A6C RID: 2668
	internal class HiZCullProgram : GPUProgram
	{
		// Token: 0x0600549D RID: 21661 RVA: 0x001850A4 File Offset: 0x001832A4
		public HiZCullProgram() : base("HiZCullVS.glsl", null, null)
		{
		}

		// Token: 0x0600549E RID: 21662 RVA: 0x001850B8 File Offset: 0x001832B8
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			string[] transformFeedbackVaryings = new string[]
			{
				"outVisible"
			};
			return base.MakeProgram(vertexShader, null, false, transformFeedbackVaryings);
		}

		// Token: 0x0600549F RID: 21663 RVA: 0x001850F2 File Offset: 0x001832F2
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.HiZBuffer.SetValue(0);
		}

		// Token: 0x04003036 RID: 12342
		public Uniform ViewProjectionMatrix;

		// Token: 0x04003037 RID: 12343
		public Uniform ViewportSize;

		// Token: 0x04003038 RID: 12344
		public Uniform HiZBuffer;

		// Token: 0x04003039 RID: 12345
		public readonly Attrib AttribBoxMin;

		// Token: 0x0400303A RID: 12346
		public readonly Attrib AttribBoxMax;
	}
}
