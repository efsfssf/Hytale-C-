using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A6F RID: 2671
	internal class LightClusteredProgram : GPUProgram
	{
		// Token: 0x060054A6 RID: 21670 RVA: 0x001851F7 File Offset: 0x001833F7
		public LightClusteredProgram() : base("ScreenVS.glsl", "LightClusteredFS.glsl", null)
		{
		}

		// Token: 0x060054A7 RID: 21671 RVA: 0x0018521C File Offset: 0x0018341C
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(new Dictionary<string, string>
			{
				{
					"USE_FAR_CORNERS",
					"1"
				}
			});
			uint fragmentShader = base.CompileFragmentShader(new Dictionary<string, string>
			{
				{
					"DEBUG",
					this.Debug ? "1" : "0"
				},
				{
					"USE_LINEAR_Z",
					this.UseLinearZ ? "1" : "0"
				},
				{
					"USE_DIRECT_ACCESS",
					this.UseLightDirectAccess ? "1" : "0"
				},
				{
					"USE_CUSTOM_Z_DISTRIBUTION",
					this.UseCustomZDistribution ? "1" : "0"
				}
			});
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x060054A8 RID: 21672 RVA: 0x001852F0 File Offset: 0x001834F0
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.LightIndicesOrDataBufferTexture.SetValue(2);
			this.LightGridTexture.SetValue(1);
			this.DepthTexture.SetValue(0);
			this.PointLightBlock.SetupBindingPoint(this, 2U);
		}

		// Token: 0x04003042 RID: 12354
		public Uniform ProjectionMatrix;

		// Token: 0x04003043 RID: 12355
		public Uniform FarCorners;

		// Token: 0x04003044 RID: 12356
		public Uniform FarClip;

		// Token: 0x04003045 RID: 12357
		public Uniform LightGridResolution;

		// Token: 0x04003046 RID: 12358
		public Uniform ZSlicesParams;

		// Token: 0x04003047 RID: 12359
		public Uniform UseLBufferCompression;

		// Token: 0x04003048 RID: 12360
		public UniformBufferObject PointLightBlock;

		// Token: 0x04003049 RID: 12361
		private Uniform DepthTexture;

		// Token: 0x0400304A RID: 12362
		private Uniform LightGridTexture;

		// Token: 0x0400304B RID: 12363
		private Uniform LightIndicesOrDataBufferTexture;

		// Token: 0x0400304C RID: 12364
		public bool UseLinearZ;

		// Token: 0x0400304D RID: 12365
		public bool UseLightDirectAccess = true;

		// Token: 0x0400304E RID: 12366
		public bool UseCustomZDistribution = true;

		// Token: 0x0400304F RID: 12367
		public bool Debug;
	}
}
