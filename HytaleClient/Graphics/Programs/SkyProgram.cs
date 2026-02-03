using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A80 RID: 2688
	internal class SkyProgram : GPUProgram
	{
		// Token: 0x060054D5 RID: 21717 RVA: 0x00186EB8 File Offset: 0x001850B8
		public SkyProgram() : base("SkyAndCloudsVS.glsl", "SkyFS.glsl", null)
		{
		}

		// Token: 0x060054D6 RID: 21718 RVA: 0x00186EE4 File Offset: 0x001850E4
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(new Dictionary<string, string>
			{
				{
					"SKY_VERSION",
					"1"
				},
				{
					"USE_MOOD_FOG",
					this.UseMoodFog ? "1" : "0"
				}
			});
			uint fragmentShader = base.CompileFragmentShader(new Dictionary<string, string>
			{
				{
					"USE_MOOD_FOG",
					this.UseMoodFog ? "1" : "0"
				},
				{
					"FOG_DITHERING",
					this.UseDitheringOnFog ? "1" : "0"
				},
				{
					"SKY_DITHERING",
					this.UseDitheringOnSky ? "1" : "0"
				}
			});
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x060054D7 RID: 21719 RVA: 0x00186FB7 File Offset: 0x001851B7
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.SunOcclusionHistory.SetValue(4);
		}

		// Token: 0x04003120 RID: 12576
		public Uniform MVPMatrix;

		// Token: 0x04003121 RID: 12577
		public Uniform StarsOpacity;

		// Token: 0x04003122 RID: 12578
		public Uniform TopGradientColor;

		// Token: 0x04003123 RID: 12579
		public Uniform SunsetColor;

		// Token: 0x04003124 RID: 12580
		private Uniform SunOcclusionHistory;

		// Token: 0x04003125 RID: 12581
		public Uniform CameraPosition;

		// Token: 0x04003126 RID: 12582
		public Uniform FogFrontColor;

		// Token: 0x04003127 RID: 12583
		public Uniform FogBackColor;

		// Token: 0x04003128 RID: 12584
		public Uniform FogMoodParams;

		// Token: 0x04003129 RID: 12585
		public Uniform SunPosition;

		// Token: 0x0400312A RID: 12586
		public Uniform SunScale;

		// Token: 0x0400312B RID: 12587
		public Uniform SunGlowColor;

		// Token: 0x0400312C RID: 12588
		public Uniform MoonOpacity;

		// Token: 0x0400312D RID: 12589
		public Uniform MoonScale;

		// Token: 0x0400312E RID: 12590
		public Uniform MoonGlowColor;

		// Token: 0x0400312F RID: 12591
		public Uniform DrawSkySunMoonStars;

		// Token: 0x04003130 RID: 12592
		public readonly Attrib AttribPosition;

		// Token: 0x04003131 RID: 12593
		public readonly Attrib AttribTexCoords;

		// Token: 0x04003132 RID: 12594
		public bool UseMoodFog = true;

		// Token: 0x04003133 RID: 12595
		public bool UseDitheringOnFog = true;

		// Token: 0x04003134 RID: 12596
		public bool UseDitheringOnSky = true;
	}
}
