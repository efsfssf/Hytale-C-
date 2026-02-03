using System;
using System.Collections.Generic;
using HytaleClient.Data.BlockyModels;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A8A RID: 2698
	internal class ZOnlyBlockyModelProgram : GPUProgram
	{
		// Token: 0x06005514 RID: 21780 RVA: 0x00187E4A File Offset: 0x0018604A
		public ZOnlyBlockyModelProgram(BlockyModelProgram blockyModelProgram, bool useModelVFX = false, string variationName = null) : base("ZOnlyBlockyModelVS.glsl", "ZOnlyBlockyModelFS.glsl", variationName)
		{
			this._blockyModelProgram = blockyModelProgram;
			this.UseModelVFX = useModelVFX;
		}

		// Token: 0x06005515 RID: 21781 RVA: 0x00187E8C File Offset: 0x0018608C
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(new Dictionary<string, string>
			{
				{
					"MAX_NODES_COUNT",
					BlockyModel.MaxNodeCount.ToString()
				},
				{
					"USE_BIAS_METHOD_1",
					this.UseBiasMethod1 ? "1" : "0"
				},
				{
					"USE_BIAS_METHOD_2",
					this.UseBiasMethod2 ? "1" : "0"
				},
				{
					"USE_DRAW_INSTANCED",
					this.UseDrawInstanced ? "1" : "0"
				},
				{
					"USE_MODEL_VFX",
					this.UseModelVFX ? "1" : "0"
				}
			});
			uint fragmentShader = base.CompileFragmentShader(new Dictionary<string, string>
			{
				{
					"USE_BIAS_METHOD_1",
					this.UseBiasMethod1 ? "1" : "0"
				},
				{
					"USE_BIAS_METHOD_2",
					this.UseBiasMethod2 ? "1" : "0"
				},
				{
					"USE_DRAW_INSTANCED",
					this.UseDrawInstanced ? "1" : "0"
				},
				{
					"USE_MODEL_VFX",
					this.UseModelVFX ? "1" : "0"
				}
			});
			return base.MakeProgram(vertexShader, fragmentShader, new List<GPUProgram.AttribBindingInfo>(5)
			{
				new GPUProgram.AttribBindingInfo(this._blockyModelProgram.AttribNodeIndex.Index, "vertNodeIndex"),
				new GPUProgram.AttribBindingInfo(this._blockyModelProgram.AttribAtlasIndexAndShadingModeAndGradientId.Index, "vertAtlasIndexAndShadingModeAndGradientId"),
				new GPUProgram.AttribBindingInfo(this._blockyModelProgram.AttribPosition.Index, "vertPosition"),
				new GPUProgram.AttribBindingInfo(this._blockyModelProgram.AttribTexCoords.Index, "vertTexCoords")
			}, true, null);
		}

		// Token: 0x06005516 RID: 21782 RVA: 0x00188078 File Offset: 0x00186278
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.Texture0.SetValue(0);
			this.Texture1.SetValue(1);
			this.Texture2.SetValue(2);
			this.NoiseTexture.SetValue(4);
			this.ModelVFXDataBuffer.SetValue(6);
			this.EntityShadowMapDataBuffer.SetValue(7);
			this.NodeBlock.SetupBindingPoint(this, 5U);
		}

		// Token: 0x0400317C RID: 12668
		public UniformBufferObject NodeBlock;

		// Token: 0x0400317D RID: 12669
		public Uniform ModelMatrix;

		// Token: 0x0400317E RID: 12670
		public Uniform ViewMatrix;

		// Token: 0x0400317F RID: 12671
		public Uniform ViewProjectionMatrix;

		// Token: 0x04003180 RID: 12672
		public Uniform ViewportInfos;

		// Token: 0x04003181 RID: 12673
		public Uniform InvModelHeight;

		// Token: 0x04003182 RID: 12674
		public Uniform Time;

		// Token: 0x04003183 RID: 12675
		public Uniform DrawId;

		// Token: 0x04003184 RID: 12676
		public Uniform ModelVFXAnimationProgress;

		// Token: 0x04003185 RID: 12677
		public Uniform ModelVFXId;

		// Token: 0x04003186 RID: 12678
		private Uniform Texture0;

		// Token: 0x04003187 RID: 12679
		private Uniform Texture1;

		// Token: 0x04003188 RID: 12680
		private Uniform Texture2;

		// Token: 0x04003189 RID: 12681
		private Uniform NoiseTexture;

		// Token: 0x0400318A RID: 12682
		private Uniform EntityShadowMapDataBuffer;

		// Token: 0x0400318B RID: 12683
		private Uniform ModelVFXDataBuffer;

		// Token: 0x0400318C RID: 12684
		public readonly Attrib AttribNodeIndex;

		// Token: 0x0400318D RID: 12685
		public readonly Attrib AttribAtlasIndexAndShadingModeAndGradientId;

		// Token: 0x0400318E RID: 12686
		public readonly Attrib AttribPosition;

		// Token: 0x0400318F RID: 12687
		public readonly Attrib AttribTexCoords;

		// Token: 0x04003190 RID: 12688
		public bool UseBiasMethod1 = false;

		// Token: 0x04003191 RID: 12689
		public bool UseBiasMethod2 = false;

		// Token: 0x04003192 RID: 12690
		public bool UseDrawInstanced = false;

		// Token: 0x04003193 RID: 12691
		public bool UseModelVFX = false;

		// Token: 0x04003194 RID: 12692
		private BlockyModelProgram _blockyModelProgram;
	}
}
