using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A69 RID: 2665
	internal class ForceFieldProgram : GPUProgram
	{
		// Token: 0x06005472 RID: 21618 RVA: 0x00183D28 File Offset: 0x00181F28
		public ForceFieldProgram(bool discardFaceHighlight = false, bool useUndergroundColor = false, string variationName = null) : base("BasicVS.glsl", "ForceFieldFS.glsl", variationName)
		{
			this._discardFaceHighlight = discardFaceHighlight;
			this._useUndergroundColor = useUndergroundColor;
		}

		// Token: 0x06005473 RID: 21619 RVA: 0x00183D90 File Offset: 0x00181F90
		public void SetupTextureUnits(ref ForceFieldProgram.TextureUnitLayout textureUnitLayout, bool initUniforms = false)
		{
			Debug.Assert(GPUProgram.IsResourceBindingLayoutValid<ForceFieldProgram.TextureUnitLayout>(textureUnitLayout), "Invalid TextureUnitLayout.");
			this._textureUnitLayout = textureUnitLayout;
			if (initUniforms)
			{
				this.InitUniforms();
			}
		}

		// Token: 0x06005474 RID: 21620 RVA: 0x00183DD0 File Offset: 0x00181FD0
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(new Dictionary<string, string>
			{
				{
					"NEED_POS_VS",
					"1"
				},
				{
					"NEED_POS_WS",
					this._discardFaceHighlight ? "1" : "0"
				},
				{
					"USE_VERT_NORMALS",
					"1"
				}
			});
			uint fragmentShader = base.CompileFragmentShader(new Dictionary<string, string>
			{
				{
					"USE_OIT",
					this.UseOIT ? "1" : "0"
				},
				{
					"DISCARD_FACE_HIGHLIGHT",
					this._discardFaceHighlight ? "1" : "0"
				},
				{
					"USE_UNDERGROUND_COLOR",
					this._useUndergroundColor ? "1" : "0"
				}
			});
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x06005475 RID: 21621 RVA: 0x00183EB4 File Offset: 0x001820B4
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.MomentsTexture.SetValue((int)this._textureUnitLayout.OITMoments);
			this.TotalOpticalDepthTexture.SetValue((int)this._textureUnitLayout.OITTotalOpticalDepth);
			this.DepthTexture.SetValue((int)this._textureUnitLayout.SceneDepth);
			this.Texture.SetValue((int)this._textureUnitLayout.Texture);
			this.SceneDataBlock.SetupBindingPoint(this, 0U);
		}

		// Token: 0x04003004 RID: 12292
		public readonly int DrawModeColor = 0;

		// Token: 0x04003005 RID: 12293
		public readonly int DrawModeDistortion = 1;

		// Token: 0x04003006 RID: 12294
		public readonly int BlendModePremultLinear = 0;

		// Token: 0x04003007 RID: 12295
		public readonly int BlendModeAdd = 1;

		// Token: 0x04003008 RID: 12296
		public readonly int BlendModeLinear = 2;

		// Token: 0x04003009 RID: 12297
		public readonly int OutlineModeNone = 0;

		// Token: 0x0400300A RID: 12298
		public readonly int OutlineModeUV = 1;

		// Token: 0x0400300B RID: 12299
		public readonly int OutlineModeNormal = 2;

		// Token: 0x0400300C RID: 12300
		public UniformBufferObject SceneDataBlock;

		// Token: 0x0400300D RID: 12301
		public Uniform ViewMatrix;

		// Token: 0x0400300E RID: 12302
		public Uniform ViewProjectionMatrix;

		// Token: 0x0400300F RID: 12303
		public Uniform ModelMatrix;

		// Token: 0x04003010 RID: 12304
		public Uniform NormalMatrix;

		// Token: 0x04003011 RID: 12305
		public Uniform ColorOpacity;

		// Token: 0x04003012 RID: 12306
		public Uniform IntersectionHighlightColorOpacity;

		// Token: 0x04003013 RID: 12307
		public Uniform IntersectionHighlightThickness;

		// Token: 0x04003014 RID: 12308
		public Uniform UVAnimationSpeed;

		// Token: 0x04003015 RID: 12309
		public Uniform OutlineMode;

		// Token: 0x04003016 RID: 12310
		public Uniform DrawAndBlendMode;

		// Token: 0x04003017 RID: 12311
		public Uniform CurrentInvViewportSize;

		// Token: 0x04003018 RID: 12312
		public Uniform OITParams;

		// Token: 0x04003019 RID: 12313
		private Uniform MomentsTexture;

		// Token: 0x0400301A RID: 12314
		private Uniform TotalOpticalDepthTexture;

		// Token: 0x0400301B RID: 12315
		private Uniform Texture;

		// Token: 0x0400301C RID: 12316
		private Uniform DepthTexture;

		// Token: 0x0400301D RID: 12317
		public readonly Attrib AttribPosition;

		// Token: 0x0400301E RID: 12318
		public readonly Attrib AttribTexCoords;

		// Token: 0x0400301F RID: 12319
		public readonly Attrib AttribNormal;

		// Token: 0x04003020 RID: 12320
		public bool UseOIT;

		// Token: 0x04003021 RID: 12321
		private bool _discardFaceHighlight;

		// Token: 0x04003022 RID: 12322
		private bool _useUndergroundColor;

		// Token: 0x04003023 RID: 12323
		private ForceFieldProgram.TextureUnitLayout _textureUnitLayout;

		// Token: 0x02000ED8 RID: 3800
		public struct TextureUnitLayout
		{
			// Token: 0x040048C2 RID: 18626
			public byte Texture;

			// Token: 0x040048C3 RID: 18627
			public byte SceneDepth;

			// Token: 0x040048C4 RID: 18628
			public byte OITMoments;

			// Token: 0x040048C5 RID: 18629
			public byte OITTotalOpticalDepth;
		}
	}
}
