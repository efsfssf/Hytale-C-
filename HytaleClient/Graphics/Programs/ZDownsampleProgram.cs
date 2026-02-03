using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A89 RID: 2697
	internal class ZDownsampleProgram : GPUProgram
	{
		// Token: 0x06005511 RID: 21777 RVA: 0x00187D69 File Offset: 0x00185F69
		public ZDownsampleProgram(bool writeToColor, bool writeToDepth, bool useLinearZ, string variationName = null) : base("ScreenVS.glsl", "ZDownsampleFS.glsl", variationName)
		{
			this.WriteToColor = writeToColor;
			this.WriteToDepth = writeToDepth;
			this.UseLinearZ = useLinearZ;
		}

		// Token: 0x06005512 RID: 21778 RVA: 0x00187D94 File Offset: 0x00185F94
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(new Dictionary<string, string>
			{
				{
					"OUTPUT_COLOR",
					this.WriteToColor ? "1" : "0"
				},
				{
					"OUTPUT_DEPTH",
					this.WriteToDepth ? "1" : "0"
				},
				{
					"INPUT_IS_LINEAR",
					this.UseLinearZ ? "1" : "0"
				}
			});
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x06005513 RID: 21779 RVA: 0x00187E2E File Offset: 0x0018602E
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.ZBuffer.SetValue(0);
		}

		// Token: 0x04003174 RID: 12660
		public Uniform Mode;

		// Token: 0x04003175 RID: 12661
		public Uniform ProjectionMatrix;

		// Token: 0x04003176 RID: 12662
		public Uniform FarClipAndInverse;

		// Token: 0x04003177 RID: 12663
		public Uniform PixelSize;

		// Token: 0x04003178 RID: 12664
		private Uniform ZBuffer;

		// Token: 0x04003179 RID: 12665
		private readonly bool WriteToColor;

		// Token: 0x0400317A RID: 12666
		private readonly bool WriteToDepth;

		// Token: 0x0400317B RID: 12667
		private readonly bool UseLinearZ;

		// Token: 0x02000EDF RID: 3807
		public enum DownsamplingMode
		{
			// Token: 0x040048EB RID: 18667
			Z_MAX,
			// Token: 0x040048EC RID: 18668
			Z_MIN,
			// Token: 0x040048ED RID: 18669
			Z_MIN_MAX,
			// Token: 0x040048EE RID: 18670
			Z_ROTATED_GRID
		}
	}
}
