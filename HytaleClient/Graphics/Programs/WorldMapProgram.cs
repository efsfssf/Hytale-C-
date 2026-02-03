using System;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A88 RID: 2696
	internal class WorldMapProgram : GPUProgram
	{
		// Token: 0x0600550E RID: 21774 RVA: 0x00187CF7 File Offset: 0x00185EF7
		public WorldMapProgram() : base("BasicVS.glsl", "WorldMapFS.glsl", null)
		{
		}

		// Token: 0x0600550F RID: 21775 RVA: 0x00187D0C File Offset: 0x00185F0C
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(null);
			return base.MakeProgram(vertexShader, fragmentShader, null, false, null);
		}

		// Token: 0x06005510 RID: 21776 RVA: 0x00187D40 File Offset: 0x00185F40
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.Texture.SetValue(0);
			this.MaskTexture.SetValue(1);
		}

		// Token: 0x0400316F RID: 12655
		public Uniform MVPMatrix;

		// Token: 0x04003170 RID: 12656
		private Uniform Texture;

		// Token: 0x04003171 RID: 12657
		private Uniform MaskTexture;

		// Token: 0x04003172 RID: 12658
		public readonly Attrib AttribPosition;

		// Token: 0x04003173 RID: 12659
		public readonly Attrib AttribTexCoords;
	}
}
