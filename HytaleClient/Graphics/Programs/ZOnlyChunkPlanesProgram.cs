using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A8B RID: 2699
	internal class ZOnlyChunkPlanesProgram : GPUProgram
	{
		// Token: 0x06005517 RID: 21783 RVA: 0x001880EE File Offset: 0x001862EE
		public ZOnlyChunkPlanesProgram(string variationName = null) : base("ZOnlyChunkPlanesVS.glsl", "ZOnlyChunkFS.glsl", variationName)
		{
		}

		// Token: 0x06005518 RID: 21784 RVA: 0x00188104 File Offset: 0x00186304
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(new Dictionary<string, string>
			{
				{
					"ALPHA_TEST",
					"0"
				},
				{
					"USE_DRAW_INSTANCED",
					"0"
				}
			});
			return base.MakeProgram(vertexShader, fragmentShader, new List<GPUProgram.AttribBindingInfo>(5)
			{
				new GPUProgram.AttribBindingInfo(0U, "vertPosition")
			}, true, null);
		}

		// Token: 0x06005519 RID: 21785 RVA: 0x0018817B File Offset: 0x0018637B
		protected override void InitUniforms()
		{
		}

		// Token: 0x04003195 RID: 12693
		public Uniform ViewProjectionMatrix;

		// Token: 0x04003196 RID: 12694
		public readonly Attrib AttribPosition;
	}
}
