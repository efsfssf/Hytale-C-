using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A57 RID: 2647
	internal class BloomCompositeProgram : GPUProgram
	{
		// Token: 0x0600543E RID: 21566 RVA: 0x00182B5F File Offset: 0x00180D5F
		public BloomCompositeProgram() : base("ScreenVS.glsl", "BloomCompositeFS.glsl", null)
		{
		}

		// Token: 0x0600543F RID: 21567 RVA: 0x00182B74 File Offset: 0x00180D74
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(new Dictionary<string, string>
			{
				{
					"SUN_FB_POW",
					this.SunFbPow ? "1" : "0"
				},
				{
					"USE_SUNSHAFT",
					this.UseSunshaft ? "1" : "0"
				}
			});
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x06005440 RID: 21568 RVA: 0x00182BF0 File Offset: 0x00180DF0
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			bool flag = this.SunFbPow || this.UseSunshaft;
			if (flag)
			{
				this.BloomTexture.SetValue(0);
			}
			bool useSunshaft = this.UseSunshaft;
			if (useSunshaft)
			{
				this.SunshaftTexture.SetValue(3);
			}
		}

		// Token: 0x04002F5F RID: 12127
		private Uniform BloomTexture;

		// Token: 0x04002F60 RID: 12128
		private Uniform SunshaftTexture;

		// Token: 0x04002F61 RID: 12129
		public Uniform BloomIntensity;

		// Token: 0x04002F62 RID: 12130
		public Uniform SunshaftIntensity;

		// Token: 0x04002F63 RID: 12131
		public bool SunFbPow;

		// Token: 0x04002F64 RID: 12132
		public bool UseSunshaft;

		// Token: 0x04002F65 RID: 12133
		public int BloomVersion;
	}
}
