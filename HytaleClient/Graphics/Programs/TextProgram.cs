using System;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A84 RID: 2692
	internal class TextProgram : GPUProgram
	{
		// Token: 0x060054E1 RID: 21729 RVA: 0x001871ED File Offset: 0x001853ED
		public TextProgram() : base("TextVS.glsl", "TextFS.glsl", null)
		{
		}

		// Token: 0x060054E2 RID: 21730 RVA: 0x00187204 File Offset: 0x00185404
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(null);
			return base.MakeProgram(vertexShader, fragmentShader, null, false, null);
		}

		// Token: 0x0400314D RID: 12621
		public Uniform MVPMatrix;

		// Token: 0x0400314E RID: 12622
		public Uniform Position;

		// Token: 0x0400314F RID: 12623
		public Uniform FogColor;

		// Token: 0x04003150 RID: 12624
		public Uniform FogParams;

		// Token: 0x04003151 RID: 12625
		public Uniform FillThreshold;

		// Token: 0x04003152 RID: 12626
		public Uniform FillBlurThreshold;

		// Token: 0x04003153 RID: 12627
		public Uniform OutlineThreshold;

		// Token: 0x04003154 RID: 12628
		public Uniform OutlineBlurThreshold;

		// Token: 0x04003155 RID: 12629
		public Uniform OutlineOffset;

		// Token: 0x04003156 RID: 12630
		public Uniform Opacity;

		// Token: 0x04003157 RID: 12631
		public readonly Attrib AttribPosition;

		// Token: 0x04003158 RID: 12632
		public readonly Attrib AttribTexCoords;

		// Token: 0x04003159 RID: 12633
		public readonly Attrib AttribFillColor;

		// Token: 0x0400315A RID: 12634
		public readonly Attrib AttribOutlineColor;
	}
}
