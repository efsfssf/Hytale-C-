using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A7C RID: 2684
	internal class RadialGlowLuminanceProgram : GPUProgram
	{
		// Token: 0x060054CA RID: 21706 RVA: 0x00186C84 File Offset: 0x00184E84
		public RadialGlowLuminanceProgram(int nbSamples) : base("RadialGlowLuminanceVS.glsl", "RadialGlowLuminanceFS.glsl", null)
		{
			this._nbSamples = nbSamples;
		}

		// Token: 0x060054CB RID: 21707 RVA: 0x00186CA0 File Offset: 0x00184EA0
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(new Dictionary<string, string>
			{
				{
					"NB_SAMPLES",
					this._nbSamples.ToString()
				}
			});
			uint fragmentShader = base.CompileFragmentShader(new Dictionary<string, string>
			{
				{
					"NB_SAMPLES",
					this._nbSamples.ToString()
				}
			});
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x060054CC RID: 21708 RVA: 0x00186D10 File Offset: 0x00184F10
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.GlowMaskTexture.SetValue(0);
		}

		// Token: 0x04003112 RID: 12562
		public Uniform MVPMatrix;

		// Token: 0x04003113 RID: 12563
		public Uniform SunMVPMatrix;

		// Token: 0x04003114 RID: 12564
		public Uniform ScaleFactor;

		// Token: 0x04003115 RID: 12565
		private Uniform GlowMaskTexture;

		// Token: 0x04003116 RID: 12566
		private int _nbSamples;
	}
}
