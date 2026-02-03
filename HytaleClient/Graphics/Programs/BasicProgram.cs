using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A54 RID: 2644
	internal class BasicProgram : GPUProgram
	{
		// Token: 0x06005436 RID: 21558 RVA: 0x00182610 File Offset: 0x00180810
		public BasicProgram(bool writeAlphaChannel = true, string variationName = null) : base("BasicVS.glsl", "BasicFS.glsl", variationName)
		{
			this._writeAlphaChannel = writeAlphaChannel;
		}

		// Token: 0x06005437 RID: 21559 RVA: 0x0018262C File Offset: 0x0018082C
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(new Dictionary<string, string>
			{
				{
					"USE_DISCARD",
					"1"
				},
				{
					"USE_COLOR_AND_OPACITY",
					"1"
				},
				{
					"WRITE_ALPHA",
					this._writeAlphaChannel ? "1" : "0"
				}
			});
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x04002F1E RID: 12062
		public Uniform MVPMatrix;

		// Token: 0x04002F1F RID: 12063
		public Uniform Color;

		// Token: 0x04002F20 RID: 12064
		public Uniform Opacity;

		// Token: 0x04002F21 RID: 12065
		public readonly Attrib AttribPosition;

		// Token: 0x04002F22 RID: 12066
		public readonly Attrib AttribTexCoords;

		// Token: 0x04002F23 RID: 12067
		private readonly bool _writeAlphaChannel;
	}
}
