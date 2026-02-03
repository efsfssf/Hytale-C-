using System;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A7E RID: 2686
	internal class SceneBrightnessPackProgram : GPUProgram
	{
		// Token: 0x060054D0 RID: 21712 RVA: 0x00186DB4 File Offset: 0x00184FB4
		public SceneBrightnessPackProgram() : base("SceneBrightnessPackVS.glsl", null, null)
		{
		}

		// Token: 0x060054D1 RID: 21713 RVA: 0x00186DC8 File Offset: 0x00184FC8
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			string[] transformFeedbackVaryings = new string[]
			{
				"outSceneBrightness"
			};
			return base.MakeProgram(vertexShader, null, false, transformFeedbackVaryings);
		}

		// Token: 0x060054D2 RID: 21714 RVA: 0x00186E02 File Offset: 0x00185002
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.SunOcclusionHistory.SetValue(0);
		}

		// Token: 0x0400311D RID: 12573
		public Uniform SunOcclusionHistory;
	}
}
