using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A59 RID: 2649
	internal class BloomSelectProgram : GPUProgram
	{
		// Token: 0x06005444 RID: 21572 RVA: 0x00182CC9 File Offset: 0x00180EC9
		public BloomSelectProgram() : base("ScreenVS.glsl", "BloomSelectFS.glsl", null)
		{
		}

		// Token: 0x06005445 RID: 21573 RVA: 0x00182CE0 File Offset: 0x00180EE0
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(new Dictionary<string, string>
			{
				{
					"SUN_OR_MOON",
					this.SunOrMoon ? "1" : "0"
				},
				{
					"FULLBRIGHT",
					this.Fullbright ? "1" : "0"
				},
				{
					"POW",
					this.Pow ? "1" : "0"
				},
				{
					"USE_DITHERING",
					this.UseDithering ? "1" : "0"
				}
			});
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x06005446 RID: 21574 RVA: 0x00182D9C File Offset: 0x00180F9C
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			bool flag = this.Pow || this.Fullbright;
			if (flag)
			{
				this.SceneColorTexture.SetValue(0);
			}
			bool sunOrMoon = this.SunOrMoon;
			if (sunOrMoon)
			{
				this.SunMoonTexture.SetValue(1);
			}
		}

		// Token: 0x04002F69 RID: 12137
		public Uniform Power;

		// Token: 0x04002F6A RID: 12138
		public Uniform SunMoonIntensity;

		// Token: 0x04002F6B RID: 12139
		public Uniform PowerOptions;

		// Token: 0x04002F6C RID: 12140
		public Uniform UseSunOrMoon;

		// Token: 0x04002F6D RID: 12141
		public Uniform Time;

		// Token: 0x04002F6E RID: 12142
		private Uniform SceneColorTexture;

		// Token: 0x04002F6F RID: 12143
		private Uniform SunMoonTexture;

		// Token: 0x04002F70 RID: 12144
		public bool SunOrMoon;

		// Token: 0x04002F71 RID: 12145
		public bool Fullbright;

		// Token: 0x04002F72 RID: 12146
		public bool Pow;

		// Token: 0x04002F73 RID: 12147
		public bool UseDithering;
	}
}
