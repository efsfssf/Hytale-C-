using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A78 RID: 2680
	internal class MaxProgram : GPUProgram
	{
		// Token: 0x060054BD RID: 21693 RVA: 0x00186297 File Offset: 0x00184497
		public MaxProgram(bool useVec3, int kernelSize, string variationName = null) : base("ScreenVS.glsl", "MaxFilterFS.glsl", variationName)
		{
			this._useVec3 = useVec3;
			this._kernelSize = kernelSize;
		}

		// Token: 0x060054BE RID: 21694 RVA: 0x001862BC File Offset: 0x001844BC
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(new Dictionary<string, string>
			{
				{
					"USE_VEC3",
					this._useVec3 ? "1" : "0"
				},
				{
					"KERNEL_SIZE",
					this._kernelSize.ToString()
				}
			});
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x060054BF RID: 21695 RVA: 0x0018632D File Offset: 0x0018452D
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.ColorTexture.SetValue(0);
		}

		// Token: 0x040030B0 RID: 12464
		public Uniform PixelSize;

		// Token: 0x040030B1 RID: 12465
		public Uniform HorizontalPass;

		// Token: 0x040030B2 RID: 12466
		protected Uniform ColorTexture;

		// Token: 0x040030B3 RID: 12467
		private bool _useVec3;

		// Token: 0x040030B4 RID: 12468
		private int _kernelSize;
	}
}
