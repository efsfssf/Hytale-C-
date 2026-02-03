using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A5C RID: 2652
	internal class CloudsProgram : GPUProgram
	{
		// Token: 0x0600544D RID: 21581 RVA: 0x00182F66 File Offset: 0x00181166
		public CloudsProgram() : base("SkyAndCloudsVS.glsl", "CloudsFS.glsl", null)
		{
		}

		// Token: 0x0600544E RID: 21582 RVA: 0x00182F8C File Offset: 0x0018118C
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(new Dictionary<string, string>
			{
				{
					"SKY_VERSION",
					"0"
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
					"USE_FOG_DITHERING",
					this.UseDithering ? "1" : "0"
				}
			});
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x0600544F RID: 21583 RVA: 0x00183040 File Offset: 0x00181240
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.Texture0.SetValue(0);
			this.Texture1.SetValue(1);
			this.Texture2.SetValue(2);
			this.Texture3.SetValue(3);
			this.SunOcclusionHistory.SetValue(4);
			this.FlowTexture.SetValue(5);
		}

		// Token: 0x04002F82 RID: 12162
		public Uniform MVPMatrix;

		// Token: 0x04002F83 RID: 12163
		public Uniform Colors;

		// Token: 0x04002F84 RID: 12164
		public Uniform UVOffsets;

		// Token: 0x04002F85 RID: 12165
		public Uniform UVMotionParams;

		// Token: 0x04002F86 RID: 12166
		public Uniform CloudsTextureCount;

		// Token: 0x04002F87 RID: 12167
		private Uniform SunOcclusionHistory;

		// Token: 0x04002F88 RID: 12168
		public Uniform CameraPosition;

		// Token: 0x04002F89 RID: 12169
		public Uniform FogFrontColor;

		// Token: 0x04002F8A RID: 12170
		public Uniform FogBackColor;

		// Token: 0x04002F8B RID: 12171
		public Uniform FogMoodParams;

		// Token: 0x04002F8C RID: 12172
		public Uniform SunPosition;

		// Token: 0x04002F8D RID: 12173
		private Uniform Texture0;

		// Token: 0x04002F8E RID: 12174
		private Uniform Texture1;

		// Token: 0x04002F8F RID: 12175
		private Uniform Texture2;

		// Token: 0x04002F90 RID: 12176
		private Uniform Texture3;

		// Token: 0x04002F91 RID: 12177
		private Uniform FlowTexture;

		// Token: 0x04002F92 RID: 12178
		public readonly Attrib AttribPosition;

		// Token: 0x04002F93 RID: 12179
		public readonly Attrib AttribTexCoords;

		// Token: 0x04002F94 RID: 12180
		public bool UseMoodFog = true;

		// Token: 0x04002F95 RID: 12181
		public bool UseDithering = true;
	}
}
