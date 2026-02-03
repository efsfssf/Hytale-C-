using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A70 RID: 2672
	internal class LightMixProgram : GPUProgram
	{
		// Token: 0x060054A9 RID: 21673 RVA: 0x0018533F File Offset: 0x0018353F
		public LightMixProgram(string variationName = null) : base("ScreenVS.glsl", "LightMixFS.glsl", variationName)
		{
		}

		// Token: 0x060054AA RID: 21674 RVA: 0x0018535C File Offset: 0x0018355C
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(new Dictionary<string, string>
			{
				{
					"USE_LBUFFER_COMPRESSION",
					this.UseLightBufferCompression ? "1" : "0"
				}
			});
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x04003050 RID: 12368
		public bool UseLightBufferCompression = false;
	}
}
