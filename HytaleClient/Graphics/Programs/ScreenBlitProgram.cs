using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A7F RID: 2687
	internal class ScreenBlitProgram : GPUProgram
	{
		// Token: 0x060054D3 RID: 21715 RVA: 0x00186E1E File Offset: 0x0018501E
		public ScreenBlitProgram(bool writeAlphaChannel = true, string variationName = null) : base("ScreenVS.glsl", "BasicFS.glsl", variationName)
		{
			this._writeAlphaChannel = writeAlphaChannel;
		}

		// Token: 0x060054D4 RID: 21716 RVA: 0x00186E3C File Offset: 0x0018503C
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(new Dictionary<string, string>
			{
				{
					"USE_DISCARD",
					"0"
				},
				{
					"USE_COLOR_AND_OPACITY",
					"0"
				},
				{
					"WRITE_ALPHA",
					this._writeAlphaChannel ? "1" : "0"
				}
			});
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x0400311E RID: 12574
		public Uniform MipLevel;

		// Token: 0x0400311F RID: 12575
		private readonly bool _writeAlphaChannel;
	}
}
