using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A8C RID: 2700
	internal class ZOnlyChunkProgram : GPUProgram
	{
		// Token: 0x0600551A RID: 21786 RVA: 0x00188180 File Offset: 0x00186380
		public ZOnlyChunkProgram(bool buildShadowMaps, bool animated, int maxNodeCount, bool alphaTest, bool useCompressedPosition, bool useFoliageCulling, float mipLodBias = 0f, string variationName = null) : base("ZOnlyChunkVS.glsl", "ZOnlyChunkFS.glsl", variationName)
		{
			this.BuildsShadowMaps = buildShadowMaps;
			this.Animated = animated;
			this.MaxNodeCount = maxNodeCount;
			this.AlphaTest = alphaTest;
			this._useCompressedPosition = useCompressedPosition;
			this._useFoliageCulling = useFoliageCulling;
			this._mipLodBias = mipLodBias;
		}

		// Token: 0x0600551B RID: 21787 RVA: 0x00188200 File Offset: 0x00186400
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(new Dictionary<string, string>
			{
				{
					"ALPHA_TEST",
					this.AlphaTest ? "1" : "0"
				},
				{
					"USE_COMPRESSED_POSITION",
					this._useCompressedPosition ? "1" : "0"
				},
				{
					"USE_FOLIAGE_CULLING",
					this._useFoliageCulling ? "1" : "0"
				},
				{
					"USE_DRAW_INSTANCED",
					this.UseDrawInstanced ? "1" : "0"
				},
				{
					"USE_BACKFACE_CULLING",
					this.UseBackfaceCulling ? "1" : "0"
				},
				{
					"USE_DISTANT_BACKFACE_CULLING",
					this.UseDistantBackfaceCulling ? "1" : "0"
				},
				{
					"DISTANT_BACKFACE_CULLING_DISTANCE",
					this.DistantBackfaceCullingDistance.ToString()
				},
				{
					"SHADOW_VERSION",
					this.BuildsShadowMaps ? "1" : "0"
				},
				{
					"ANIMATED",
					this.Animated ? "1" : "0"
				},
				{
					"MAX_NODES_COUNT",
					this.MaxNodeCount.ToString()
				}
			});
			List<GPUProgram.AttribBindingInfo> list = new List<GPUProgram.AttribBindingInfo>(5);
			list.Add(new GPUProgram.AttribBindingInfo(0U, "vertPositionAndDoubleSidedAndBlockId"));
			list.Add(new GPUProgram.AttribBindingInfo(2U, "vertDataPacked"));
			bool flag = this.AlphaTest || this.UseDrawInstanced;
			bool result;
			if (flag)
			{
				uint fragmentShader = base.CompileFragmentShader(new Dictionary<string, string>
				{
					{
						"USE_DRAW_INSTANCED",
						this.UseDrawInstanced ? "1" : "0"
					},
					{
						"ALPHA_TEST",
						this.AlphaTest ? "1" : "0"
					},
					{
						"MIP_LOD_BIAS",
						this._mipLodBias.ToString()
					}
				});
				bool alphaTest = this.AlphaTest;
				if (alphaTest)
				{
					list.Add(new GPUProgram.AttribBindingInfo(1U, "vertTexCoords"));
				}
				result = base.MakeProgram(vertexShader, fragmentShader, list, true, null);
			}
			else
			{
				result = base.MakeProgram(vertexShader, list, true, null);
			}
			return result;
		}

		// Token: 0x0600551C RID: 21788 RVA: 0x00188448 File Offset: 0x00186648
		protected override void InitUniforms()
		{
			bool alphaTest = this.AlphaTest;
			if (alphaTest)
			{
				GPUProgram._gl.UseProgram(this);
				this.Texture.SetValue(0);
			}
			bool animated = this.Animated;
			if (animated)
			{
				this.NodeBlock.SetupBindingPoint(this, 5U);
			}
		}

		// Token: 0x04003197 RID: 12695
		public UniformBufferObject NodeBlock;

		// Token: 0x04003198 RID: 12696
		public Uniform ModelMatrix;

		// Token: 0x04003199 RID: 12697
		public Uniform ViewProjectionMatrix;

		// Token: 0x0400319A RID: 12698
		public Uniform Time;

		// Token: 0x0400319B RID: 12699
		public Uniform LightPositions;

		// Token: 0x0400319C RID: 12700
		public Uniform TargetCascades;

		// Token: 0x0400319D RID: 12701
		public Uniform ViewportInfos;

		// Token: 0x0400319E RID: 12702
		private Uniform Texture;

		// Token: 0x0400319F RID: 12703
		private readonly bool BuildsShadowMaps;

		// Token: 0x040031A0 RID: 12704
		private readonly bool Animated;

		// Token: 0x040031A1 RID: 12705
		public bool AlphaTest = true;

		// Token: 0x040031A2 RID: 12706
		public bool UseDrawInstanced = false;

		// Token: 0x040031A3 RID: 12707
		public bool UseBackfaceCulling = true;

		// Token: 0x040031A4 RID: 12708
		public bool UseDistantBackfaceCulling = false;

		// Token: 0x040031A5 RID: 12709
		public float DistantBackfaceCullingDistance = 92f;

		// Token: 0x040031A6 RID: 12710
		private readonly int MaxNodeCount;

		// Token: 0x040031A7 RID: 12711
		private float _mipLodBias;

		// Token: 0x040031A8 RID: 12712
		private bool _useCompressedPosition;

		// Token: 0x040031A9 RID: 12713
		private bool _useFoliageCulling;
	}
}
