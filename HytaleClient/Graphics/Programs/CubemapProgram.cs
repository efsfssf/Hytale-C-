using System;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A5D RID: 2653
	internal class CubemapProgram : GPUProgram
	{
		// Token: 0x06005450 RID: 21584 RVA: 0x001830A8 File Offset: 0x001812A8
		public CubemapProgram() : base("CubemapVS.glsl", "CubemapFS.glsl", null)
		{
		}

		// Token: 0x06005451 RID: 21585 RVA: 0x001830C0 File Offset: 0x001812C0
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(null);
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x06005452 RID: 21586 RVA: 0x001830F4 File Offset: 0x001812F4
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.Texture.SetValue(0);
		}

		// Token: 0x04002F96 RID: 12182
		public Uniform MVPMatrix;

		// Token: 0x04002F97 RID: 12183
		private Uniform Texture;

		// Token: 0x04002F98 RID: 12184
		public readonly Attrib AttribPosition;
	}
}
