using System;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A55 RID: 2645
	internal class Batcher2DProgram : GPUProgram
	{
		// Token: 0x06005438 RID: 21560 RVA: 0x001826A8 File Offset: 0x001808A8
		public Batcher2DProgram() : base("Batcher2DVS.glsl", "Batcher2DFS.glsl", null)
		{
		}

		// Token: 0x06005439 RID: 21561 RVA: 0x001826C0 File Offset: 0x001808C0
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(null);
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x0600543A RID: 21562 RVA: 0x001826F4 File Offset: 0x001808F4
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.Texture.SetValue(0);
			this.MaskTexture.SetValue(1);
			this.FontTexture.SetValue(2);
		}

		// Token: 0x04002F24 RID: 12068
		public readonly Uniform MVPMatrix;

		// Token: 0x04002F25 RID: 12069
		private readonly Uniform Texture;

		// Token: 0x04002F26 RID: 12070
		private readonly Uniform MaskTexture;

		// Token: 0x04002F27 RID: 12071
		private readonly Uniform FontTexture;

		// Token: 0x04002F28 RID: 12072
		public readonly Attrib AttribPosition;

		// Token: 0x04002F29 RID: 12073
		public readonly Attrib AttribTexCoords;

		// Token: 0x04002F2A RID: 12074
		public readonly Attrib AttribScissor;

		// Token: 0x04002F2B RID: 12075
		public readonly Attrib AttribMaskTextureArea;

		// Token: 0x04002F2C RID: 12076
		public readonly Attrib AttribMaskBounds;

		// Token: 0x04002F2D RID: 12077
		public readonly Attrib AttribFillColor;

		// Token: 0x04002F2E RID: 12078
		public readonly Attrib AttribOutlineColor;

		// Token: 0x04002F2F RID: 12079
		public readonly Attrib AttribSDFSettings;

		// Token: 0x04002F30 RID: 12080
		public readonly Attrib AttribFontId;

		// Token: 0x02000ED6 RID: 3798
		public enum TextureUnit
		{
			// Token: 0x040048B7 RID: 18615
			Texture,
			// Token: 0x040048B8 RID: 18616
			MaskTexture,
			// Token: 0x040048B9 RID: 18617
			FontTexture
		}
	}
}
