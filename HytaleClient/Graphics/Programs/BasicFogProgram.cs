using System;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A53 RID: 2643
	internal class BasicFogProgram : GPUProgram
	{
		// Token: 0x06005434 RID: 21556 RVA: 0x001825C7 File Offset: 0x001807C7
		public BasicFogProgram() : base("BasicFogVS.glsl", "BasicFogFS.glsl", null)
		{
		}

		// Token: 0x06005435 RID: 21557 RVA: 0x001825DC File Offset: 0x001807DC
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(null);
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x04002F15 RID: 12053
		public Uniform MVPMatrix;

		// Token: 0x04002F16 RID: 12054
		public Uniform ModelMatrix;

		// Token: 0x04002F17 RID: 12055
		public Uniform Color;

		// Token: 0x04002F18 RID: 12056
		public Uniform Opacity;

		// Token: 0x04002F19 RID: 12057
		public Uniform CameraPosition;

		// Token: 0x04002F1A RID: 12058
		public Uniform FogColor;

		// Token: 0x04002F1B RID: 12059
		public Uniform FogParams;

		// Token: 0x04002F1C RID: 12060
		public readonly Attrib AttribPosition;

		// Token: 0x04002F1D RID: 12061
		public readonly Attrib AttribTexCoords;
	}
}
