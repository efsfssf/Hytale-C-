using System;
using System.Diagnostics;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A40 RID: 2624
	internal class OrderIndependentTransparency
	{
		// Token: 0x170012C2 RID: 4802
		// (get) Token: 0x0600525F RID: 21087 RVA: 0x0016AD65 File Offset: 0x00168F65
		public OrderIndependentTransparency.Method CurrentMethod
		{
			get
			{
				return this._method;
			}
		}

		// Token: 0x170012C3 RID: 4803
		// (get) Token: 0x06005260 RID: 21088 RVA: 0x0016AD6D File Offset: 0x00168F6D
		public bool HasFullResPass
		{
			get
			{
				return this._drawTransparentsFullRes != null;
			}
		}

		// Token: 0x170012C4 RID: 4804
		// (get) Token: 0x06005261 RID: 21089 RVA: 0x0016AD78 File Offset: 0x00168F78
		public bool HasHalfResPass
		{
			get
			{
				return this._drawTransparentsHalfRes != null;
			}
		}

		// Token: 0x170012C5 RID: 4805
		// (get) Token: 0x06005262 RID: 21090 RVA: 0x0016AD83 File Offset: 0x00168F83
		public bool HasQuarterResPass
		{
			get
			{
				return this._drawTransparentsQuarterRes != null;
			}
		}

		// Token: 0x170012C6 RID: 4806
		// (get) Token: 0x06005263 RID: 21091 RVA: 0x0016AD8E File Offset: 0x00168F8E
		public bool NeedsZBufferHalfRes
		{
			get
			{
				return (this._method == OrderIndependentTransparency.Method.MOIT && this._prepassResolutionScale >= OrderIndependentTransparency.ResolutionScale.Half) || this.HasHalfResPass;
			}
		}

		// Token: 0x170012C7 RID: 4807
		// (get) Token: 0x06005264 RID: 21092 RVA: 0x0016ADAB File Offset: 0x00168FAB
		public bool NeedsZBufferQuarterRes
		{
			get
			{
				return (this._method == OrderIndependentTransparency.Method.MOIT && this._prepassResolutionScale >= OrderIndependentTransparency.ResolutionScale.Quarter) || this.HasQuarterResPass;
			}
		}

		// Token: 0x170012C8 RID: 4808
		// (get) Token: 0x06005265 RID: 21093 RVA: 0x0016ADC8 File Offset: 0x00168FC8
		public bool NeedsZBufferEighthRes
		{
			get
			{
				return this._method == OrderIndependentTransparency.Method.MOIT && this._prepassResolutionScale >= OrderIndependentTransparency.ResolutionScale.Eighth;
			}
		}

		// Token: 0x06005266 RID: 21094 RVA: 0x0016ADE2 File Offset: 0x00168FE2
		public void UseEdgeFixupPass(bool fixupHalfRes, bool fixupQuarterRes, byte edgesStencilBit)
		{
			Debug.Assert(edgesStencilBit < 8, string.Format("Invalid stencil bit id requested for Edges: {0}. Valide entries are[0-7].", edgesStencilBit));
			this._drawHalfResEdgeFixup = fixupHalfRes;
			this._drawQuarterResEdgeFixup = fixupQuarterRes;
			this._edgesStencilBit = edgesStencilBit;
		}

		// Token: 0x06005267 RID: 21095 RVA: 0x0016AE14 File Offset: 0x00169014
		public OrderIndependentTransparency(GraphicsDevice graphics, RenderTargetStore renderTargetStore, Profiling profiling)
		{
			this._graphics = graphics;
			this._gl = this._graphics.GL;
			this._renderTargetStore = renderTargetStore;
			this._profiling = profiling;
			this._useFallback = !this._graphics.SupportsDrawBuffersBlend;
		}

		// Token: 0x06005268 RID: 21096 RVA: 0x0016AE71 File Offset: 0x00169071
		public void Init()
		{
		}

		// Token: 0x06005269 RID: 21097 RVA: 0x0016AE74 File Offset: 0x00169074
		public void Dispose()
		{
		}

		// Token: 0x0600526A RID: 21098 RVA: 0x0016AE77 File Offset: 0x00169077
		public void SetupRenderingProfiles(int profileOITPrepass, int profileOITAccumulateQuarterRes, int profileOITAccumulateHalfRes, int profileOITAccumulateFullRes, int profileOITComposite)
		{
			this._profileOITPrepass = profileOITPrepass;
			this._profileOITAccumulateQuarterRes = profileOITAccumulateQuarterRes;
			this._profileOITAccumulateHalfRes = profileOITAccumulateHalfRes;
			this._profileOITAccumulateFullRes = profileOITAccumulateFullRes;
			this._profileOITComposite = profileOITComposite;
		}

		// Token: 0x0600526B RID: 21099 RVA: 0x0016AE9F File Offset: 0x0016909F
		public void SetMethod(OrderIndependentTransparency.Method method)
		{
			this._method = method;
		}

		// Token: 0x0600526C RID: 21100 RVA: 0x0016AEA8 File Offset: 0x001690A8
		public void SetPrepassResolutionScale(OrderIndependentTransparency.ResolutionScale scale)
		{
			Debug.Assert(scale < OrderIndependentTransparency.ResolutionScale.End, "Unsupported OIT prepass ResolutionScale.");
			bool flag = scale != this._prepassResolutionScale;
			if (flag)
			{
				this._prepassResolutionScale = scale;
				float num = 1f;
				switch (scale)
				{
				case OrderIndependentTransparency.ResolutionScale.Full:
					num /= 1f;
					break;
				case OrderIndependentTransparency.ResolutionScale.Half:
					num /= 2f;
					break;
				case OrderIndependentTransparency.ResolutionScale.Quarter:
					num /= 4f;
					break;
				case OrderIndependentTransparency.ResolutionScale.Eighth:
					num /= 8f;
					break;
				default:
					Debug.Assert(false, "OITPrepass setup error.");
					break;
				}
				this._renderTargetStore.SetMomentsTransparencyResolutionScale(num);
			}
		}

		// Token: 0x0600526D RID: 21101 RVA: 0x0016AF43 File Offset: 0x00169143
		public void RegisterDrawTransparentsFunc(DrawTransparencyFunc drawFullResFunc, DrawTransparencyFunc drawHalfResFunc, DrawTransparencyFunc drawQuarterResFunc)
		{
			this._drawTransparentsFullRes = drawFullResFunc;
			this._drawTransparentsHalfRes = drawHalfResFunc;
			this._drawTransparentsQuarterRes = drawQuarterResFunc;
		}

		// Token: 0x0600526E RID: 21102 RVA: 0x0016AF5B File Offset: 0x0016915B
		public void SetupTextureUnits(byte moitMomentUnit, byte moitTotalOpticalDepthUnit)
		{
			Debug.Assert(moitMomentUnit != moitTotalOpticalDepthUnit);
			this._moitMomentUnit = moitMomentUnit;
			this._moitTotalOpticalDepthUnit = moitTotalOpticalDepthUnit;
		}

		// Token: 0x0600526F RID: 21103 RVA: 0x0016AF7C File Offset: 0x0016917C
		public void SkipInternalMeasures()
		{
			this._profiling.SkipMeasure(this._profileOITPrepass);
			this._profiling.SkipMeasure(this._profileOITAccumulateQuarterRes);
			this._profiling.SkipMeasure(this._profileOITAccumulateHalfRes);
			this._profiling.SkipMeasure(this._profileOITAccumulateFullRes);
			this._profiling.SkipMeasure(this._profileOITComposite);
		}

		// Token: 0x06005270 RID: 21104 RVA: 0x0016AFE4 File Offset: 0x001691E4
		public void Draw(bool hasFullResItems = true, bool hasHalfResItems = true, bool hasQuarterResItems = true)
		{
			Debug.Assert(this._drawTransparentsFullRes != null || this._drawTransparentsHalfRes != null || this._drawTransparentsQuarterRes != null, "No TransparencyFunc was defined. Make sure you call RegisterDrawTransparentsFunc.");
			bool flag = this._method == OrderIndependentTransparency.Method.None || (hasFullResItems && hasHalfResItems && hasQuarterResItems);
			if (flag)
			{
				this.SkipInternalMeasures();
			}
			else
			{
				this._graphics.SaveColorMask();
				this._gl.ColorMask(true, true, true, true);
				this._gl.BlendEquation(GL.FUNC_ADD);
				bool flag2 = this._drawQuarterResEdgeFixup && this.HasQuarterResPass;
				bool flag3 = this._drawHalfResEdgeFixup && this.HasHalfResPass;
				bool flag4 = hasQuarterResItems && this.HasQuarterResPass;
				bool flag5 = hasHalfResItems && this.HasHalfResPass;
				bool flag6 = (hasFullResItems && this.HasFullResPass) || flag3 || flag2;
				bool flag7 = this._method == OrderIndependentTransparency.Method.MOIT;
				if (flag7)
				{
					this._profiling.StartMeasure(this._profileOITPrepass);
					this.DrawPrepass(this._drawTransparentsFullRes, this._drawTransparentsHalfRes, this._drawTransparentsQuarterRes);
					this._profiling.StopMeasure(this._profileOITPrepass);
				}
				else
				{
					this._profiling.SkipMeasure(this._profileOITPrepass);
				}
				bool flag8 = flag4;
				if (flag8)
				{
					this._profiling.StartMeasure(this._profileOITAccumulateQuarterRes);
					this.DrawAccumulation(this._renderTargetStore.TransparencyQuarterRes, this._drawTransparentsQuarterRes, null, null);
					this._profiling.StopMeasure(this._profileOITAccumulateQuarterRes);
				}
				else
				{
					this._profiling.SkipMeasure(this._profileOITAccumulateQuarterRes);
				}
				bool flag9 = flag5;
				if (flag9)
				{
					this._profiling.StartMeasure(this._profileOITAccumulateHalfRes);
					this.DrawAccumulation(this._renderTargetStore.TransparencyHalfRes, this._drawTransparentsHalfRes, null, null);
					this._profiling.StopMeasure(this._profileOITAccumulateHalfRes);
				}
				else
				{
					this._profiling.SkipMeasure(this._profileOITAccumulateHalfRes);
				}
				bool flag10 = flag6;
				if (flag10)
				{
					DrawTransparencyFunc drawFixupFunc = flag3 ? this._drawTransparentsHalfRes : null;
					DrawTransparencyFunc drawFixupFunc2 = flag2 ? this._drawTransparentsQuarterRes : null;
					this._profiling.StartMeasure(this._profileOITAccumulateFullRes);
					this.DrawAccumulation(this._renderTargetStore.Transparency, this._drawTransparentsFullRes, drawFixupFunc, drawFixupFunc2);
					this._profiling.StopMeasure(this._profileOITAccumulateFullRes);
				}
				else
				{
					this._profiling.SkipMeasure(this._profileOITAccumulateFullRes);
				}
				this._graphics.RestoreColorMask();
				this._profiling.StartMeasure(this._profileOITComposite);
				this.DrawComposite(flag6, flag5, flag4);
				this._profiling.StopMeasure(this._profileOITComposite);
			}
		}

		// Token: 0x06005271 RID: 21105 RVA: 0x0016B28C File Offset: 0x0016948C
		private void DrawPrepass(DrawTransparencyFunc drawFullResFunc, DrawTransparencyFunc drawHalfResFunc, DrawTransparencyFunc drawQuarterResFunc)
		{
			Debug.Assert(drawFullResFunc != null || drawHalfResFunc != null || drawQuarterResFunc != null);
			Debug.Assert(this._method == OrderIndependentTransparency.Method.MOIT, "OIT Prepass is only required for MOIT.");
			bool setupViewport = this._prepassResolutionScale > OrderIndependentTransparency.ResolutionScale.Full;
			this._renderTargetStore.MomentsTransparencyCapture.Bind(false, setupViewport);
			float[] data = new float[4];
			float[] array = new float[]
			{
				1f,
				1f,
				1f,
				1f
			};
			this._gl.ClearBufferfv(GL.COLOR, 0, data);
			this._gl.ClearBufferfv(GL.COLOR, 1, data);
			this._gl.BlendFunci(0U, GL.ONE, GL.ONE);
			this._gl.BlendFunci(1U, GL.ONE, GL.ONE);
			bool flag = drawFullResFunc != null;
			if (flag)
			{
				drawFullResFunc((int)this._method, 0, this._renderTargetStore.MomentsTransparencyCapture.InvResolution, true);
			}
			bool flag2 = drawHalfResFunc != null;
			if (flag2)
			{
				drawHalfResFunc((int)this._method, 0, this._renderTargetStore.MomentsTransparencyCapture.InvResolution, true);
			}
			bool flag3 = drawQuarterResFunc != null;
			if (flag3)
			{
				drawQuarterResFunc((int)this._method, 0, this._renderTargetStore.MomentsTransparencyCapture.InvResolution, true);
			}
			this._renderTargetStore.MomentsTransparencyCapture.Unbind();
		}

		// Token: 0x06005272 RID: 21106 RVA: 0x0016B3DC File Offset: 0x001695DC
		private void DrawTransparentGeometry(bool sendDataToGPU, int oitMethod, int extra, Vector2 invViewportSize, DrawTransparencyFunc drawFunc, DrawTransparencyFunc drawFixupFunc1 = null, DrawTransparencyFunc drawFixupFunc2 = null)
		{
			bool flag = drawFunc != null;
			if (flag)
			{
				drawFunc(oitMethod, extra, invViewportSize, sendDataToGPU);
			}
			bool flag2 = drawFixupFunc1 != null || drawFixupFunc2 != null;
			if (flag2)
			{
				this._gl.Enable(GL.STENCIL_TEST);
				this._gl.StencilMask(0U);
				this._gl.StencilOp(GL.KEEP, GL.KEEP, GL.KEEP);
				this._gl.StencilFunc(GL.EQUAL, 1 << (int)this._edgesStencilBit, 1U << (int)this._edgesStencilBit);
				bool flag3 = drawFixupFunc1 != null;
				if (flag3)
				{
					drawFixupFunc1(oitMethod, extra, invViewportSize, false);
				}
				bool flag4 = drawFixupFunc2 != null;
				if (flag4)
				{
					drawFixupFunc2(oitMethod, extra, invViewportSize, false);
				}
				this._gl.Disable(GL.STENCIL_TEST);
			}
		}

		// Token: 0x06005273 RID: 21107 RVA: 0x0016B4B8 File Offset: 0x001696B8
		private void DrawAccumulation(RenderTarget transparencyRenderTarget, DrawTransparencyFunc drawFunc, DrawTransparencyFunc drawFixupFunc1 = null, DrawTransparencyFunc drawFixupFunc2 = null)
		{
			Debug.Assert(drawFunc != null || drawFixupFunc1 != null || drawFixupFunc2 != null);
			Debug.Assert(transparencyRenderTarget != null);
			Debug.Assert(this._moitMomentUnit != this._moitTotalOpticalDepthUnit || this._method != OrderIndependentTransparency.Method.MOIT, "Invalid TextureUnit used for MOIT.");
			bool flag = this._method == OrderIndependentTransparency.Method.MOIT;
			if (flag)
			{
				this._gl.ActiveTexture(GL.TEXTURE0 + (uint)this._moitMomentUnit);
				this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.MomentsTransparencyCapture.GetTexture(RenderTarget.Target.Color0));
				this._gl.ActiveTexture(GL.TEXTURE0 + (uint)this._moitTotalOpticalDepthUnit);
				this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.MomentsTransparencyCapture.GetTexture(RenderTarget.Target.Color1));
			}
			transparencyRenderTarget.Bind(false, true);
			float[] data = new float[4];
			float[] data2 = new float[]
			{
				1f,
				1f,
				1f,
				0f
			};
			this._gl.ClearBufferfv(GL.COLOR, 0, data);
			this._gl.ClearBufferfv(GL.COLOR, 1, data2);
			OrderIndependentTransparency.Method oitMethod = (this._method == OrderIndependentTransparency.Method.MOIT) ? (this._method + 1) : this._method;
			bool sendDataToGPU = this._method != OrderIndependentTransparency.Method.MOIT;
			GL sfactor = GL.ONE;
			GL dfactor = GL.ONE;
			GL srcRGB = GL.NO_ERROR;
			GL dstRGB = GL.ONE_MINUS_SRC_COLOR;
			GL srcAlpha = GL.ONE;
			GL dstAlpha = GL.ONE;
			bool flag2 = !this._useFallback;
			if (flag2)
			{
				this._gl.BlendFunci(0U, sfactor, dfactor);
				this._gl.BlendFuncSeparatei(1U, srcRGB, dstRGB, srcAlpha, dstAlpha);
				this.DrawTransparentGeometry(sendDataToGPU, (int)oitMethod, 0, transparencyRenderTarget.InvResolution, drawFunc, drawFixupFunc1, drawFixupFunc2);
			}
			else
			{
				this._gl.DrawBuffer(GL.COLOR_ATTACHMENT0);
				this._gl.BlendFunc(sfactor, dfactor);
				this.DrawTransparentGeometry(sendDataToGPU, (int)oitMethod, 0, transparencyRenderTarget.InvResolution, drawFunc, drawFixupFunc1, drawFixupFunc2);
				this._gl.DrawBuffer(GL.COLOR_ATTACHMENT1);
				this._gl.BlendFuncSeparate(srcRGB, dstRGB, srcAlpha, dstAlpha);
				this.DrawTransparentGeometry(sendDataToGPU, (int)oitMethod, 1, transparencyRenderTarget.InvResolution, drawFunc, drawFixupFunc1, drawFixupFunc2);
				transparencyRenderTarget.SetupDrawBuffers();
			}
			transparencyRenderTarget.Unbind();
		}

		// Token: 0x06005274 RID: 21108 RVA: 0x0016B704 File Offset: 0x00169904
		private void DrawComposite(bool needsFullResPass, bool needsHalfResPass, bool needsQuarterResPass)
		{
			this._gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
			bool setupViewport = !needsFullResPass && (needsHalfResPass || needsQuarterResPass);
			this._renderTargetStore.SceneColor.Bind(false, setupViewport);
			bool flag = this._method == OrderIndependentTransparency.Method.POIT;
			if (flag)
			{
				this._gl.Disable(GL.BLEND);
				this._gl.ActiveTexture(GL.TEXTURE2);
				this._gl.BindSampler(2U, GLSampler.None);
				this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.FinalSceneColor.GetTexture(RenderTarget.Target.Color0));
			}
			this._gl.ActiveTexture(GL.TEXTURE6);
			this._gl.BindSampler(6U, GLSampler.None);
			this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.TransparencyQuarterRes.GetTexture(RenderTarget.Target.Color0));
			this._gl.ActiveTexture(GL.TEXTURE5);
			this._gl.BindSampler(5U, GLSampler.None);
			this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.TransparencyQuarterRes.GetTexture(RenderTarget.Target.Color1));
			this._gl.ActiveTexture(GL.TEXTURE4);
			this._gl.BindSampler(4U, GLSampler.None);
			this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.TransparencyHalfRes.GetTexture(RenderTarget.Target.Color0));
			this._gl.ActiveTexture(GL.TEXTURE3);
			this._gl.BindSampler(3U, GLSampler.None);
			this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.TransparencyHalfRes.GetTexture(RenderTarget.Target.Color1));
			this._gl.ActiveTexture(GL.TEXTURE1);
			this._gl.BindSampler(1U, GLSampler.None);
			this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.Transparency.GetTexture(RenderTarget.Target.Color1));
			this._gl.ActiveTexture(GL.TEXTURE0);
			this._gl.BindSampler(0U, GLSampler.None);
			this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.Transparency.GetTexture(RenderTarget.Target.Color0));
			OITCompositeProgram oitcompositeProgram = this._graphics.GPUProgramStore.OITCompositeProgram;
			this._gl.UseProgram(oitcompositeProgram);
			oitcompositeProgram.OITMethod.SetValue((int)this._method);
			Vector4 value;
			value.X = (float)(needsFullResPass ? 1 : 0);
			value.Y = (float)(needsHalfResPass ? 1 : 0);
			value.Z = (float)(needsQuarterResPass ? 1 : 0);
			value.W = 0f;
			oitcompositeProgram.InputResolutionUsed.SetValue(value);
			this._graphics.ScreenTriangleRenderer.Draw();
			bool flag2 = this._method == OrderIndependentTransparency.Method.POIT;
			if (flag2)
			{
				this._gl.Enable(GL.BLEND);
			}
		}

		// Token: 0x04002D77 RID: 11639
		private int _profileOITPrepass;

		// Token: 0x04002D78 RID: 11640
		private int _profileOITAccumulateQuarterRes;

		// Token: 0x04002D79 RID: 11641
		private int _profileOITAccumulateHalfRes;

		// Token: 0x04002D7A RID: 11642
		private int _profileOITAccumulateFullRes;

		// Token: 0x04002D7B RID: 11643
		private int _profileOITComposite;

		// Token: 0x04002D7C RID: 11644
		private readonly Profiling _profiling;

		// Token: 0x04002D7D RID: 11645
		private readonly RenderTargetStore _renderTargetStore;

		// Token: 0x04002D7E RID: 11646
		private readonly GraphicsDevice _graphics;

		// Token: 0x04002D7F RID: 11647
		private readonly GLFunctions _gl;

		// Token: 0x04002D80 RID: 11648
		private OrderIndependentTransparency.Method _method;

		// Token: 0x04002D81 RID: 11649
		private OrderIndependentTransparency.ResolutionScale _prepassResolutionScale;

		// Token: 0x04002D82 RID: 11650
		private byte _moitMomentUnit;

		// Token: 0x04002D83 RID: 11651
		private byte _moitTotalOpticalDepthUnit;

		// Token: 0x04002D84 RID: 11652
		private DrawTransparencyFunc _drawTransparentsFullRes;

		// Token: 0x04002D85 RID: 11653
		private DrawTransparencyFunc _drawTransparentsHalfRes;

		// Token: 0x04002D86 RID: 11654
		private DrawTransparencyFunc _drawTransparentsQuarterRes;

		// Token: 0x04002D87 RID: 11655
		private byte _edgesStencilBit;

		// Token: 0x04002D88 RID: 11656
		private bool _drawHalfResEdgeFixup = false;

		// Token: 0x04002D89 RID: 11657
		private bool _drawQuarterResEdgeFixup = false;

		// Token: 0x04002D8A RID: 11658
		private bool _useFallback;

		// Token: 0x02000EAD RID: 3757
		public enum Method
		{
			// Token: 0x0400478F RID: 18319
			None,
			// Token: 0x04004790 RID: 18320
			WBOIT,
			// Token: 0x04004791 RID: 18321
			WBOITExt,
			// Token: 0x04004792 RID: 18322
			POIT,
			// Token: 0x04004793 RID: 18323
			MOIT
		}

		// Token: 0x02000EAE RID: 3758
		public enum ResolutionScale
		{
			// Token: 0x04004795 RID: 18325
			Full,
			// Token: 0x04004796 RID: 18326
			Half,
			// Token: 0x04004797 RID: 18327
			Quarter,
			// Token: 0x04004798 RID: 18328
			Eighth,
			// Token: 0x04004799 RID: 18329
			End
		}
	}
}
