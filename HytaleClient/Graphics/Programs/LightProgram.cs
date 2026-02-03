using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A71 RID: 2673
	internal class LightProgram : GPUProgram
	{
		// Token: 0x060054AB RID: 21675 RVA: 0x001853B6 File Offset: 0x001835B6
		public LightProgram(int maxDeferredLights) : base("LightVS.glsl", "LightFS.glsl", null)
		{
			this.MaxDeferredLights = maxDeferredLights;
		}

		// Token: 0x060054AC RID: 21676 RVA: 0x001853DC File Offset: 0x001835DC
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(new Dictionary<string, string>
			{
				{
					"USE_LINEAR_Z",
					this.UseLinearZ ? "1" : "0"
				}
			});
			uint fragmentShader = base.CompileFragmentShader(new Dictionary<string, string>
			{
				{
					"USE_LINEAR_Z",
					this.UseLinearZ ? "1" : "0"
				},
				{
					"MAX_DEFERRED_LIGHTS",
					this.MaxDeferredLights.ToString()
				},
				{
					"USE_LBUFFER_COMPRESSION",
					this.UseLightBufferCompression ? "1" : "0"
				}
			});
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x060054AD RID: 21677 RVA: 0x00185495 File Offset: 0x00183695
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.DepthTexture.SetValue(0);
		}

		// Token: 0x04003051 RID: 12369
		public Uniform ModelMatrix;

		// Token: 0x04003052 RID: 12370
		public Uniform ViewMatrix;

		// Token: 0x04003053 RID: 12371
		public Uniform ProjectionMatrix;

		// Token: 0x04003054 RID: 12372
		private Uniform DepthTexture;

		// Token: 0x04003055 RID: 12373
		public Uniform FarClip;

		// Token: 0x04003056 RID: 12374
		public Uniform FarCorners;

		// Token: 0x04003057 RID: 12375
		public Uniform InvScreenSize;

		// Token: 0x04003058 RID: 12376
		public Uniform Color;

		// Token: 0x04003059 RID: 12377
		public Uniform PositionSize;

		// Token: 0x0400305A RID: 12378
		public Uniform GlobalLightPositionSizes;

		// Token: 0x0400305B RID: 12379
		public Uniform GlobalLightColors;

		// Token: 0x0400305C RID: 12380
		public Uniform LightGroup;

		// Token: 0x0400305D RID: 12381
		public Uniform UseLightGroup;

		// Token: 0x0400305E RID: 12382
		public Uniform TransferMethod;

		// Token: 0x0400305F RID: 12383
		public Uniform Debug;

		// Token: 0x04003060 RID: 12384
		public readonly Attrib AttribPosition;

		// Token: 0x04003061 RID: 12385
		public bool UseLinearZ;

		// Token: 0x04003062 RID: 12386
		public bool UseLightBufferCompression = false;

		// Token: 0x04003063 RID: 12387
		public int MaxDeferredLights;
	}
}
