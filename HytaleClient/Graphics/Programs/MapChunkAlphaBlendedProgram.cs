using System;
using System.Diagnostics;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A74 RID: 2676
	internal class MapChunkAlphaBlendedProgram : MapChunkBaseProgram
	{
		// Token: 0x17001300 RID: 4864
		// (get) Token: 0x060054B4 RID: 21684 RVA: 0x00185959 File Offset: 0x00183B59
		public ref MapChunkAlphaBlendedProgram.TextureUnitLayout TextureUnits
		{
			get
			{
				return ref this._textureUnitLayout;
			}
		}

		// Token: 0x060054B5 RID: 21685 RVA: 0x00185961 File Offset: 0x00183B61
		public MapChunkAlphaBlendedProgram(bool useForwardSunShadows, string variationName = null) : base(false, true, true, false, false, variationName)
		{
			this.UseForwardSunShadows = useForwardSunShadows;
		}

		// Token: 0x060054B6 RID: 21686 RVA: 0x00185978 File Offset: 0x00183B78
		public void SetupTextureUnits(ref MapChunkAlphaBlendedProgram.TextureUnitLayout textureUnitLayout, bool initUniforms = false)
		{
			Debug.Assert(GPUProgram.IsResourceBindingLayoutValid<MapChunkAlphaBlendedProgram.TextureUnitLayout>(textureUnitLayout), "Invalid TextureUnitLayout.");
			this._textureUnitLayout = textureUnitLayout;
			if (initUniforms)
			{
				this.InitUniforms();
			}
		}

		// Token: 0x060054B7 RID: 21687 RVA: 0x001859B8 File Offset: 0x00183BB8
		protected override void InitUniforms()
		{
			base.InitUniforms();
			GPUProgram._gl.UseProgram(this);
			this.MomentsTexture.SetValue((int)this._textureUnitLayout.OITMoments);
			this.TotalOpticalDepthTexture.SetValue((int)this._textureUnitLayout.OITTotalOpticalDepth);
			this.CloudShadowTexture.SetValue((int)this._textureUnitLayout.CloudShadow);
			this.CausticsTexture.SetValue((int)this._textureUnitLayout.Caustics);
			this.LightIndicesOrDataBufferTexture.SetValue((int)this._textureUnitLayout.LightIndicesOrDataBuffer);
			this.LightGridTexture.SetValue((int)this._textureUnitLayout.LightGrid);
			this.ShadowMap.SetValue((int)this._textureUnitLayout.ShadowMap);
			bool useMoodFog = this.UseMoodFog;
			if (useMoodFog)
			{
				this.FogNoiseTexture.SetValue((int)this._textureUnitLayout.FogNoise);
			}
			this.LowResDepthTexture.SetValue((int)this._textureUnitLayout.SceneDepthLowRes);
			this.NormalsTexture.SetValue((int)this._textureUnitLayout.Normals);
			this.RefractionTexture.SetValue((int)this._textureUnitLayout.Refraction);
			this.SceneTexture.SetValue((int)this._textureUnitLayout.SceneColor);
			this.DepthTexture.SetValue((int)this._textureUnitLayout.SceneDepth);
		}

		// Token: 0x0400307C RID: 12412
		public Uniform DebugOverdraw;

		// Token: 0x0400307D RID: 12413
		public Uniform CurrentInvViewportSize;

		// Token: 0x0400307E RID: 12414
		public Uniform OITParams;

		// Token: 0x0400307F RID: 12415
		private Uniform MomentsTexture;

		// Token: 0x04003080 RID: 12416
		private Uniform TotalOpticalDepthTexture;

		// Token: 0x04003081 RID: 12417
		public Uniform InvTextureAtlasSize;

		// Token: 0x04003082 RID: 12418
		public Uniform WaterTintColor;

		// Token: 0x04003083 RID: 12419
		public Uniform WaterQuality;

		// Token: 0x04003084 RID: 12420
		private Uniform NormalsTexture;

		// Token: 0x04003085 RID: 12421
		private Uniform DepthTexture;

		// Token: 0x04003086 RID: 12422
		private Uniform LowResDepthTexture;

		// Token: 0x04003087 RID: 12423
		private Uniform SceneTexture;

		// Token: 0x04003088 RID: 12424
		private Uniform RefractionTexture;

		// Token: 0x04003089 RID: 12425
		private Uniform FogNoiseTexture;

		// Token: 0x0400308A RID: 12426
		private Uniform ShadowMap;

		// Token: 0x0400308B RID: 12427
		private Uniform CausticsTexture;

		// Token: 0x0400308C RID: 12428
		private Uniform CloudShadowTexture;

		// Token: 0x0400308D RID: 12429
		private MapChunkAlphaBlendedProgram.TextureUnitLayout _textureUnitLayout;

		// Token: 0x02000EDD RID: 3805
		public struct TextureUnitLayout
		{
			// Token: 0x040048D1 RID: 18641
			public byte Texture;

			// Token: 0x040048D2 RID: 18642
			public byte SceneDepth;

			// Token: 0x040048D3 RID: 18643
			public byte SceneDepthLowRes;

			// Token: 0x040048D4 RID: 18644
			public byte Normals;

			// Token: 0x040048D5 RID: 18645
			public byte Refraction;

			// Token: 0x040048D6 RID: 18646
			public byte SceneColor;

			// Token: 0x040048D7 RID: 18647
			public byte Caustics;

			// Token: 0x040048D8 RID: 18648
			public byte CloudShadow;

			// Token: 0x040048D9 RID: 18649
			public byte FogNoise;

			// Token: 0x040048DA RID: 18650
			public byte ShadowMap;

			// Token: 0x040048DB RID: 18651
			public byte LightIndicesOrDataBuffer;

			// Token: 0x040048DC RID: 18652
			public byte LightGrid;

			// Token: 0x040048DD RID: 18653
			public byte OITMoments;

			// Token: 0x040048DE RID: 18654
			public byte OITTotalOpticalDepth;
		}
	}
}
