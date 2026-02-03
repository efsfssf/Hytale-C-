using System;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A5E RID: 2654
	internal class DebugDrawMapProgram : GPUProgram
	{
		// Token: 0x06005453 RID: 21587 RVA: 0x00183110 File Offset: 0x00181310
		public DebugDrawMapProgram() : base("ScreenVS.glsl", "DebugDrawMapFS.glsl", null)
		{
		}

		// Token: 0x06005454 RID: 21588 RVA: 0x00183128 File Offset: 0x00181328
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(null);
			return base.MakeProgram(vertexShader, fragmentShader, null, false, null);
		}

		// Token: 0x06005455 RID: 21589 RVA: 0x0018315C File Offset: 0x0018135C
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.Texture2D.SetValue(0);
			this.Texture2DArray.SetValue(1);
			this.TextureCubemap.SetValue(2);
		}

		// Token: 0x04002F99 RID: 12185
		public Uniform Viewport;

		// Token: 0x04002F9A RID: 12186
		public Uniform TextureSize;

		// Token: 0x04002F9B RID: 12187
		public Uniform MipLevel;

		// Token: 0x04002F9C RID: 12188
		public Uniform Opacity;

		// Token: 0x04002F9D RID: 12189
		public Uniform Layer;

		// Token: 0x04002F9E RID: 12190
		public Uniform Multiplier;

		// Token: 0x04002F9F RID: 12191
		public Uniform DebugMaxOverdraw;

		// Token: 0x04002FA0 RID: 12192
		public Uniform DebugZ;

		// Token: 0x04002FA1 RID: 12193
		public Uniform LinearZ;

		// Token: 0x04002FA2 RID: 12194
		public Uniform DebugTexture2DArray;

		// Token: 0x04002FA3 RID: 12195
		public Uniform CubemapFace;

		// Token: 0x04002FA4 RID: 12196
		public Uniform NormalQuantization;

		// Token: 0x04002FA5 RID: 12197
		public Uniform ChromaSubsampling;

		// Token: 0x04002FA6 RID: 12198
		public Uniform ColorChannels;

		// Token: 0x04002FA7 RID: 12199
		private Uniform Texture2D;

		// Token: 0x04002FA8 RID: 12200
		private Uniform Texture2DArray;

		// Token: 0x04002FA9 RID: 12201
		private Uniform TextureCubemap;
	}
}
