using System;
using System.Collections.Generic;
using System.Diagnostics;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A45 RID: 2629
	internal class RenderTargetStore
	{
		// Token: 0x060052E6 RID: 21222 RVA: 0x00170E70 File Offset: 0x0016F070
		public RenderTargetStore(GraphicsDevice graphics, int width, int height, Vector2 shadowMapResolution, Vector2 deferredShadowResolutionScale, Vector2 ssaoResolutionScale)
		{
			this._graphics = graphics;
			this._momentsTransparencyResolutionScale = new Vector2(1f);
			this._deferredShadowResolutionScale = deferredShadowResolutionScale;
			this._ssaoResolutionScale = ssaoResolutionScale;
			int width2 = (int)((float)width * 0.5f);
			int height2 = (int)((float)height * 0.5f);
			int width3 = (int)((float)width * 0.25f);
			int height3 = (int)((float)height * 0.25f);
			int width4 = (int)((float)width * 0.125f);
			int height4 = (int)((float)height * 0.125f);
			int num = (int)((float)width * 0.0625f);
			int num2 = (int)((float)height * 0.0625f);
			int width5 = (int)((float)num * 0.5f);
			int height5 = (int)((float)num2 * 0.5f);
			this.HardwareZ = new RenderTarget(width, height, "HardwareZ");
			this.HardwareZ.AddTexture(RenderTarget.Target.Depth, GL.DEPTH24_STENCIL8, GL.DEPTH_STENCIL, GL.UNSIGNED_INT_24_8, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, false, false, 1);
			this.HardwareZ.FinalizeSetup();
			this.HardwareZHalfRes = new RenderTarget(width2, height2, "HardwareZHalfRes");
			this.HardwareZHalfRes.AddTexture(RenderTarget.Target.Depth, GL.DEPTH24_STENCIL8, GL.DEPTH_STENCIL, GL.UNSIGNED_INT_24_8, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, false, false, 1);
			this.HardwareZHalfRes.FinalizeSetup();
			this.HardwareZQuarterRes = new RenderTarget(width3, height3, "HardwareZQuarterRes");
			this.HardwareZQuarterRes.AddTexture(RenderTarget.Target.Depth, GL.DEPTH24_STENCIL8, GL.DEPTH_STENCIL, GL.UNSIGNED_INT_24_8, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, false, false, 1);
			this.HardwareZQuarterRes.FinalizeSetup();
			this.HardwareZEighthRes = new RenderTarget(width4, height4, "HardwareZEighthRes");
			this.HardwareZEighthRes.AddTexture(RenderTarget.Target.Depth, GL.DEPTH24_STENCIL8, GL.DEPTH_STENCIL, GL.UNSIGNED_INT_24_8, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, false, false, 1);
			this.HardwareZEighthRes.FinalizeSetup();
			this.LinearZ = new RenderTarget(width, height, "LinearZ");
			this.LinearZ.AddTexture(RenderTarget.Target.Color0, GL.R16F, GL.RED, GL.FLOAT, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, false, false, 1);
			this.LinearZ.FinalizeSetup();
			this.LinearZHalfRes = new RenderTarget(width2, height2, "LinearZHalfRes");
			this.LinearZHalfRes.AddTexture(RenderTarget.Target.Color0, GL.R16F, GL.RED, GL.FLOAT, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, false, false, 1);
			this.LinearZHalfRes.FinalizeSetup();
			this.Edges = new RenderTarget(width, height, "Edges");
			this.Edges.UseAsRenderTexture(this.HardwareZ.GetTexture(RenderTarget.Target.Depth), true, RenderTarget.Target.Depth, GL.DEPTH24_STENCIL8, GL.DEPTH_STENCIL, GL.UNSIGNED_INT_24_8, 1, 1);
			this.Edges.AddTexture(RenderTarget.Target.Color0, GL.R8, GL.RED, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.Edges.FinalizeSetup();
			this.Edges.SetClearBits(true, false, false);
			this.GBuffer = new RenderTarget(width, height, "GBuffer");
			this.GBuffer.UseAsRenderTexture(this.HardwareZ.GetTexture(RenderTarget.Target.Depth), true, RenderTarget.Target.Depth, GL.DEPTH24_STENCIL8, GL.DEPTH_STENCIL, GL.UNSIGNED_INT_24_8, 1, 1);
			this.GBuffer.AddTexture(RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, false, false, 1);
			this.GBuffer.AddTexture(RenderTarget.Target.Color2, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, false, false, 1);
			this.GBuffer.FinalizeSetup();
			this.GBuffer.SetClearBits(false, true, true);
			this.PingSceneColor = new RenderTarget(width, height, "PingSceneColor");
			this.PingSceneColor.UseAsRenderTexture(this.HardwareZ.GetTexture(RenderTarget.Target.Depth), true, RenderTarget.Target.Depth, GL.DEPTH24_STENCIL8, GL.DEPTH_STENCIL, GL.UNSIGNED_INT_24_8, 1, 1);
			this.PingSceneColor.AddTexture(RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.NEAREST, GL.LINEAR, GL.CLAMP_TO_EDGE, false, true, 1);
			this.PingSceneColor.FinalizeSetup();
			this.PingSceneColor.SetClearBits(true, false, false);
			this.PongSceneColor = new RenderTarget(width, height, "PongSceneColor");
			this.PongSceneColor.UseAsRenderTexture(this.HardwareZ.GetTexture(RenderTarget.Target.Depth), true, RenderTarget.Target.Depth, GL.DEPTH24_STENCIL8, GL.DEPTH_STENCIL, GL.UNSIGNED_INT_24_8, 1, 1);
			this.PongSceneColor.AddTexture(RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.NEAREST, GL.LINEAR, GL.CLAMP_TO_EDGE, false, true, 1);
			this.PongSceneColor.FinalizeSetup();
			this.PongSceneColor.SetClearBits(true, false, false);
			this.PingFinalSceneColor = new RenderTarget(width, height, "PingFinalSceneColor");
			this.PingFinalSceneColor.UseAsRenderTexture(this.HardwareZ.GetTexture(RenderTarget.Target.Depth), true, RenderTarget.Target.Depth, GL.DEPTH24_STENCIL8, GL.DEPTH_STENCIL, GL.UNSIGNED_INT_24_8, 1, 1);
			this.PingFinalSceneColor.AddTexture(RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.PingFinalSceneColor.FinalizeSetup();
			this.PingFinalSceneColor.SetClearBits(false, false, false);
			this.PongFinalSceneColor = new RenderTarget(width, height, "PongFinalSceneColor");
			this.PongFinalSceneColor.UseAsRenderTexture(this.HardwareZ.GetTexture(RenderTarget.Target.Depth), true, RenderTarget.Target.Depth, GL.DEPTH24_STENCIL8, GL.DEPTH_STENCIL, GL.UNSIGNED_INT_24_8, 1, 1);
			this.PongFinalSceneColor.AddTexture(RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.PongFinalSceneColor.AddTexture(RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.PongFinalSceneColor.FinalizeSetup();
			this.PongFinalSceneColor.SetClearBits(false, false, false);
			this.SceneColorHalfRes = new RenderTarget(width2, height2, "SceneColorHalfRes");
			this.SceneColorHalfRes.AddTexture(RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, true, 1);
			this.SceneColorHalfRes.FinalizeSetup();
			this.SceneColorHalfRes.SetClearBits(true, false, false);
			this.LightBufferFullRes = new RenderTarget(width, height, "LightBufferFullRes");
			this.LightBufferFullRes.UseAsRenderTexture(this.GBuffer.GetTexture(RenderTarget.Target.Depth), true, RenderTarget.Target.Depth, GL.DEPTH24_STENCIL8, GL.DEPTH_STENCIL, GL.UNSIGNED_INT_24_8, 1, 1);
			this.LightBufferFullRes.UseAsRenderTexture(this.GBuffer.GetTexture(RenderTarget.Target.Color2), true, RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, 1, 1);
			this.LightBufferFullRes.FinalizeSetup();
			this.LightBufferFullRes.SetClearBits(true, false, false);
			this.LightBufferHalfRes = new RenderTarget(width2, height2, "LightBufferHalfRes");
			this.LightBufferHalfRes.UseAsRenderTexture(this.HardwareZHalfRes.GetTexture(RenderTarget.Target.Depth), true, RenderTarget.Target.Depth, GL.DEPTH24_STENCIL8, GL.DEPTH_STENCIL, GL.UNSIGNED_INT_24_8, 1, 1);
			this.LightBufferHalfRes.AddTexture(RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.LightBufferHalfRes.FinalizeSetup();
			this.LightBufferHalfRes.SetClearBits(true, false, true);
			this.Transparency = new RenderTarget(width, height, "Transparency");
			this.Transparency.UseAsRenderTexture(this.HardwareZ.GetTexture(RenderTarget.Target.Depth), true, RenderTarget.Target.Depth, GL.DEPTH24_STENCIL8, GL.DEPTH_STENCIL, GL.UNSIGNED_INT_24_8, 1, 1);
			this.Transparency.AddTexture(RenderTarget.Target.Color0, GL.RGBA16F, GL.RGBA, GL.FLOAT, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, false, false, 1);
			this.Transparency.AddTexture(RenderTarget.Target.Color1, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, false, false, 1);
			this.Transparency.FinalizeSetup();
			this.Transparency.SetClearBits(false, false, false);
			this.TransparencyHalfRes = new RenderTarget(width2, height2, "TransparencyHalfRes");
			this.TransparencyHalfRes.UseAsRenderTexture(this.HardwareZHalfRes.GetTexture(RenderTarget.Target.Depth), true, RenderTarget.Target.Depth, GL.DEPTH24_STENCIL8, GL.DEPTH_STENCIL, GL.UNSIGNED_INT_24_8, 1, 1);
			this.TransparencyHalfRes.AddTexture(RenderTarget.Target.Color0, GL.RGBA16F, GL.RGBA, GL.FLOAT, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.TransparencyHalfRes.AddTexture(RenderTarget.Target.Color1, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.TransparencyHalfRes.FinalizeSetup();
			this.TransparencyHalfRes.SetClearBits(false, false, false);
			this.TransparencyQuarterRes = new RenderTarget(width3, height3, "TransparencyQuarterRes");
			this.TransparencyQuarterRes.UseAsRenderTexture(this.HardwareZQuarterRes.GetTexture(RenderTarget.Target.Depth), true, RenderTarget.Target.Depth, GL.DEPTH24_STENCIL8, GL.DEPTH_STENCIL, GL.UNSIGNED_INT_24_8, 1, 1);
			this.TransparencyQuarterRes.AddTexture(RenderTarget.Target.Color0, GL.RGBA16F, GL.RGBA, GL.FLOAT, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.TransparencyQuarterRes.AddTexture(RenderTarget.Target.Color1, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.TransparencyQuarterRes.FinalizeSetup();
			this.TransparencyQuarterRes.SetClearBits(false, false, false);
			this.MomentsTransparencyCapture = new RenderTarget(width, height, "MomentsTransparencyCapture");
			this.MomentsTransparencyCapture.UseAsRenderTexture(this.HardwareZ.GetTexture(RenderTarget.Target.Depth), true, RenderTarget.Target.Depth, GL.DEPTH24_STENCIL8, GL.DEPTH_STENCIL, GL.UNSIGNED_INT_24_8, 1, 1);
			this.MomentsTransparencyCapture.AddTexture(RenderTarget.Target.Color0, GL.RGBA32F, GL.RGBA, GL.FLOAT, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.MomentsTransparencyCapture.AddTexture(RenderTarget.Target.Color1, GL.R32F, GL.RED, GL.FLOAT, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.MomentsTransparencyCapture.FinalizeSetup();
			this.MomentsTransparencyCapture.SetClearBits(false, false, false);
			this.Distortion = new RenderTarget(width2, height2, "Distortion");
			this.Distortion.UseAsRenderTexture(this.HardwareZHalfRes.GetTexture(RenderTarget.Target.Depth), true, RenderTarget.Target.Depth, GL.DEPTH24_STENCIL8, GL.DEPTH_STENCIL, GL.UNSIGNED_INT_24_8, 1, 1);
			this.Distortion.AddTexture(RenderTarget.Target.Color0, GL.RG16F, GL.RG, GL.FLOAT, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.Distortion.FinalizeSetup();
			this.Distortion.SetClearBits(true, false, false);
			this.DebugFXOverdraw = new RenderTarget(width, height, "DebugFXOverdraw");
			this.DebugFXOverdraw.UseAsRenderTexture(this.HardwareZ.GetTexture(RenderTarget.Target.Depth), true, RenderTarget.Target.Depth, GL.DEPTH24_STENCIL8, GL.DEPTH_STENCIL, GL.UNSIGNED_INT_24_8, 1, 1);
			this.DebugFXOverdraw.AddTexture(RenderTarget.Target.Color0, GL.R16F, GL.RED, GL.FLOAT, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, false, false, 1);
			this.DebugFXOverdraw.FinalizeSetup();
			this.DebugFXOverdraw.SetClearBits(true, false, false);
			this.ShadowMap = new RenderTarget((int)shadowMapResolution.X, (int)shadowMapResolution.Y, "ShadowMap");
			this.ShadowMap.AddTexture(RenderTarget.Target.Depth, GL.DEPTH_COMPONENT32F, GL.DEPTH_COMPONENT, GL.FLOAT, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_BORDER, true, false, 1);
			this.ShadowMap.FinalizeSetup();
			int width6 = (int)((float)width * deferredShadowResolutionScale.X);
			int height6 = (int)((float)height * deferredShadowResolutionScale.Y);
			this.DeferredShadow = new RenderTarget(width6, height6, "DeferredShadow");
			this.DeferredShadow.AddTexture(RenderTarget.Target.Color0, GL.R8, GL.RED, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.DeferredShadow.FinalizeSetup();
			int width7 = (int)((float)width * ssaoResolutionScale.X);
			int height7 = (int)((float)height * ssaoResolutionScale.Y);
			this.PingSSAO = new RenderTarget(width7, height7, "PingSSAO");
			this.PingSSAO.AddTexture(RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.PingSSAO.FinalizeSetup();
			this.PongSSAO = new RenderTarget(width7, height7, "PongSSAO");
			this.PongSSAO.AddTexture(RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.PongSSAO.FinalizeSetup();
			this.BlurSSAOAndShadowTmp = new RenderTarget(width7, height7, "BlurSSAOAndShadowTmp");
			this.BlurSSAOAndShadowTmp.AddTexture(RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.BlurSSAOAndShadowTmp.FinalizeSetup();
			this.BlurSSAOAndShadow = new RenderTarget(width7, height7, "BlurSSAOAndShadow");
			this.BlurSSAOAndShadow.AddTexture(RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.BlurSSAOAndShadow.FinalizeSetup();
			this.DOFBlurXBis = new RenderTarget(width2, height2, "DOFBlurXBis");
			this.DOFBlurXBis.AddTexture(RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.DOFBlurXBis.AddTexture(RenderTarget.Target.Color1, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.DOFBlurXBis.FinalizeSetup();
			this.DOFBlurXBis.SetClearBits(true, false, false);
			this.DOFBlurYBis = new RenderTarget(width2, height2, "DOFBlurYBis");
			this.DOFBlurYBis.AddTexture(RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.DOFBlurYBis.AddTexture(RenderTarget.Target.Color1, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.DOFBlurYBis.FinalizeSetup();
			this.DOFBlurYBis.SetClearBits(true, false, false);
			this.DOFBlurX = new RenderTarget(width2, height2, "DOFBlurX");
			this.DOFBlurX.AddTexture(RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.DOFBlurX.FinalizeSetup();
			this.DOFBlurX.SetClearBits(true, false, false);
			this.DOFBlurY = new RenderTarget(width2, height2, "DOFBlurY");
			this.DOFBlurY.AddTexture(RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.DOFBlurY.FinalizeSetup();
			this.DOFBlurY.SetClearBits(true, false, false);
			this.CoC = new RenderTarget(width, height, "CoC");
			this.CoC.AddTexture(RenderTarget.Target.Color0, GL.RG8, GL.RG, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.CoC.FinalizeSetup();
			this.CoC.SetClearBits(true, false, false);
			this.Downsample = new RenderTarget(width2, height2, "Downsample");
			this.Downsample.AddTexture(RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.Downsample.AddTexture(RenderTarget.Target.Color1, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.Downsample.AddTexture(RenderTarget.Target.Color2, GL.RG8, GL.RG, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.Downsample.FinalizeSetup();
			this.Downsample.SetClearBits(true, false, false);
			this.NearCoCMaxX = new RenderTarget(width2, height2, "NearCoCMaxX");
			this.NearCoCMaxX.AddTexture(RenderTarget.Target.Color0, GL.R8, GL.RED, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.NearCoCMaxX.FinalizeSetup();
			this.NearCoCMaxX.SetClearBits(true, false, false);
			this.NearCoCMaxY = new RenderTarget(width2, height2, "NearCoCMaxY");
			this.NearCoCMaxY.AddTexture(RenderTarget.Target.Color0, GL.R8, GL.RED, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.NearCoCMaxY.FinalizeSetup();
			this.NearCoCMaxY.SetClearBits(true, false, false);
			this.NearCoCBlurX = new RenderTarget(width2, height2, "NearCoCBlurX");
			this.NearCoCBlurX.AddTexture(RenderTarget.Target.Color0, GL.R8, GL.RED, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.NearCoCBlurX.FinalizeSetup();
			this.NearCoCBlurX.SetClearBits(true, false, false);
			this.NearCoCBlurY = new RenderTarget(width2, height2, "NearCoCBlurY");
			this.NearCoCBlurY.AddTexture(RenderTarget.Target.Color0, GL.R8, GL.RED, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.NearCoCBlurY.FinalizeSetup();
			this.NearCoCBlurY.SetClearBits(true, false, false);
			this.NearFarField = new RenderTarget(width2, height2, "NearFarField");
			this.NearFarField.AddTexture(RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.NearFarField.AddTexture(RenderTarget.Target.Color1, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.NearFarField.FinalizeSetup();
			this.NearFarField.SetClearBits(true, false, false);
			this.Fill = new RenderTarget(width2, height2, "Fill");
			this.Fill.AddTexture(RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.Fill.AddTexture(RenderTarget.Target.Color1, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.Fill.FinalizeSetup();
			this.Fill.SetClearBits(true, false, false);
			this.BlurXResBy2 = new RenderTarget(width2, height2, "BlurXResBy2");
			this.BlurXResBy2.AddTexture(RenderTarget.Target.Color0, GL.RGB8, GL.RGB, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.BlurXResBy2.FinalizeSetup();
			this.BlurXResBy2.SetClearBits(true, false, false);
			this.BlurYResBy2 = new RenderTarget(width2, height2, "BlurYResBy2");
			this.BlurYResBy2.AddTexture(RenderTarget.Target.Color0, GL.RGB8, GL.RGB, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.BlurYResBy2.FinalizeSetup();
			this.BlurYResBy2.SetClearBits(true, false, false);
			this.BlurXResBy4 = new RenderTarget(width3, height3, "BlurXResBy4");
			this.BlurXResBy4.AddTexture(RenderTarget.Target.Color0, GL.RGB8, GL.RGB, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.BlurXResBy4.FinalizeSetup();
			this.BlurXResBy4.SetClearBits(true, false, false);
			this.BlurYResBy4 = new RenderTarget(width3, height3, "BlurYResBy4");
			this.BlurYResBy4.AddTexture(RenderTarget.Target.Color0, GL.RGB8, GL.RGB, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.BlurYResBy4.FinalizeSetup();
			this.BlurYResBy4.SetClearBits(true, false, false);
			this.BlurXResBy8 = new RenderTarget(width4, height4, "BlurXResBy8");
			this.BlurXResBy8.AddTexture(RenderTarget.Target.Color0, GL.RGB8, GL.RGB, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.BlurXResBy8.FinalizeSetup();
			this.BlurXResBy8.SetClearBits(true, false, false);
			this.BlurYResBy8 = new RenderTarget(width4, height4, "BlurYResBy8");
			this.BlurYResBy8.AddTexture(RenderTarget.Target.Color0, GL.RGB8, GL.RGB, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.BlurYResBy8.FinalizeSetup();
			this.BlurYResBy8.SetClearBits(true, false, false);
			this.BlurXResBy16 = new RenderTarget(num, num2, "BlurXResBy16");
			this.BlurXResBy16.AddTexture(RenderTarget.Target.Color0, GL.RGB8, GL.RGB, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.BlurXResBy16.FinalizeSetup();
			this.BlurXResBy16.SetClearBits(true, false, false);
			this.BlurYResBy16 = new RenderTarget(num, num2, "BlurYResBy16");
			this.BlurYResBy16.AddTexture(RenderTarget.Target.Color0, GL.RGB8, GL.RGB, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.BlurYResBy16.FinalizeSetup();
			this.BlurYResBy16.SetClearBits(true, false, false);
			this.BlurXResBy32 = new RenderTarget(width5, height5, "BlurXResBy32");
			this.BlurXResBy32.AddTexture(RenderTarget.Target.Color0, GL.RGB8, GL.RGB, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.BlurXResBy32.FinalizeSetup();
			this.BlurXResBy32.SetClearBits(true, false, false);
			this.SunRT = new RenderTarget(width, height, "SunRT");
			this.SunRT.UseAsRenderTexture(this.HardwareZ.GetTexture(RenderTarget.Target.Depth), true, RenderTarget.Target.Depth, GL.DEPTH24_STENCIL8, GL.DEPTH_STENCIL, GL.UNSIGNED_INT_24_8, 1, 1);
			this.SunRT.AddTexture(RenderTarget.Target.Color0, GL.RGB8, GL.RGB, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.SunRT.FinalizeSetup();
			this.SunRT.SetClearBits(true, false, false);
			this.SunshaftX = new RenderTarget(width3, height3, "SunshaftX");
			this.SunshaftX.AddTexture(RenderTarget.Target.Color0, GL.RGB8, GL.RGB, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.SunshaftX.FinalizeSetup();
			this.SunshaftX.SetClearBits(true, false, false);
			this.SunshaftY = new RenderTarget(width3, height3, "SunshaftY");
			this.SunshaftY.AddTexture(RenderTarget.Target.Color0, GL.RGB8, GL.RGB, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this.SunshaftY.FinalizeSetup();
			this.SunshaftY.SetClearBits(true, false, false);
			this.VolumetricSunshaft = new RenderTarget(width2, height2, "VolumetricSunshaft");
			this.VolumetricSunshaft.UseAsRenderTexture(this.HardwareZHalfRes.GetTexture(RenderTarget.Target.Depth), true, RenderTarget.Target.Depth, GL.DEPTH24_STENCIL8, GL.DEPTH_STENCIL, GL.UNSIGNED_INT_24_8, 1, 1);
			this.VolumetricSunshaft.AddTexture(RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.NEAREST, GL.LINEAR, GL.CLAMP_TO_EDGE, false, true, 1);
			this.VolumetricSunshaft.FinalizeSetup();
			this.VolumetricSunshaft.SetClearBits(true, false, false);
			this.SunOcclusionBufferLowRes = new RenderTarget(512, 256, "SunOcclusionBufferLowRes");
			this.SunOcclusionBufferLowRes.AddTexture(RenderTarget.Target.Color0, GL.R8, GL.RED, GL.UNSIGNED_BYTE, GL.LINEAR_MIPMAP_LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, true, 1);
			this.SunOcclusionBufferLowRes.FinalizeSetup();
			this.SunOcclusionHistory = new RenderTarget(512, 1, "SunOcclusionHistory");
			this.SunOcclusionHistory.AddTexture(RenderTarget.Target.Color0, GL.R8, GL.RED, GL.UNSIGNED_BYTE, GL.LINEAR_MIPMAP_NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, false, false, 1);
			this.SunOcclusionHistory.FinalizeSetup();
			this.SSAORaw = this.PingSSAO;
			this.PreviousSSAORaw = this.PongSSAO;
			this.SceneColor = this.PingSceneColor;
			this.PreviousSceneColor = this.PongSceneColor;
			this.InitDebugMapInfos();
		}

		// Token: 0x060052E7 RID: 21223 RVA: 0x001726F4 File Offset: 0x001708F4
		public void Dispose()
		{
			this.SunOcclusionHistory.Dispose();
			this.SunOcclusionBufferLowRes.Dispose();
			this.VolumetricSunshaft.Dispose();
			this.SunshaftY.Dispose();
			this.SunshaftX.Dispose();
			this.SunRT.Dispose();
			this.BlurXResBy32.Dispose();
			this.BlurYResBy16.Dispose();
			this.BlurXResBy16.Dispose();
			this.BlurYResBy8.Dispose();
			this.BlurXResBy8.Dispose();
			this.BlurYResBy4.Dispose();
			this.BlurXResBy4.Dispose();
			this.BlurYResBy2.Dispose();
			this.BlurXResBy2.Dispose();
			this.Fill.Dispose();
			this.NearFarField.Dispose();
			this.NearCoCBlurY.Dispose();
			this.NearCoCBlurX.Dispose();
			this.NearCoCMaxY.Dispose();
			this.NearCoCMaxX.Dispose();
			this.Downsample.Dispose();
			this.CoC.Dispose();
			this.DOFBlurY.Dispose();
			this.DOFBlurX.Dispose();
			this.DOFBlurYBis.Dispose();
			this.DOFBlurXBis.Dispose();
			this.BlurSSAOAndShadowTmp.Dispose();
			this.BlurSSAOAndShadow.Dispose();
			this.PongSSAO.Dispose();
			this.PingSSAO.Dispose();
			this.DeferredShadow.Dispose();
			this.ShadowMap.Dispose();
			this.DebugFXOverdraw.Dispose();
			this.Distortion.Dispose();
			this.MomentsTransparencyCapture.Dispose();
			this.TransparencyQuarterRes.Dispose();
			this.TransparencyHalfRes.Dispose();
			this.Transparency.Dispose();
			this.LightBufferHalfRes.Dispose();
			this.LightBufferFullRes.Dispose();
			this.SceneColorHalfRes.Dispose();
			this.PongFinalSceneColor.Dispose();
			this.PingFinalSceneColor.Dispose();
			this.PongSceneColor.Dispose();
			this.PingSceneColor.Dispose();
			this.GBuffer.Dispose();
			this.Edges.Dispose();
			this.LinearZHalfRes.Dispose();
			this.LinearZ.Dispose();
			this.HardwareZEighthRes.Dispose();
			this.HardwareZQuarterRes.Dispose();
			this.HardwareZHalfRes.Dispose();
			this.HardwareZ.Dispose();
		}

		// Token: 0x060052E8 RID: 21224 RVA: 0x0017298C File Offset: 0x00170B8C
		public void Resize(int windowWidth, int windowHeight, float renderScale = 1f)
		{
			this._viewportSize.X = (float)windowWidth;
			this._viewportSize.Y = (float)windowHeight;
			int num = (int)(this._viewportSize.X * renderScale);
			int num2 = (int)(this._viewportSize.Y * renderScale);
			int width = (int)((float)num * 0.5f);
			int height = (int)((float)num2 * 0.5f);
			int width2 = (int)((float)num * 0.25f);
			int height2 = (int)((float)num2 * 0.25f);
			int width3 = (int)((float)num * 0.125f);
			int height3 = (int)((float)num2 * 0.125f);
			int num3 = (int)((float)num * 0.0625f);
			int height4 = (int)((float)num2 * 0.0625f);
			int width4 = (int)((float)num3 * 0.5f);
			int height5 = (int)((float)num3 * 0.5f);
			this.HardwareZ.Resize(num, num2, false);
			this.HardwareZHalfRes.Resize(width, height, false);
			this.HardwareZQuarterRes.Resize(width2, height2, false);
			this.HardwareZEighthRes.Resize(width3, height3, false);
			this.LinearZ.Resize(num, num2, false);
			this.LinearZHalfRes.Resize(width, height, false);
			this.Edges.Resize(num, num2, false);
			this.GBuffer.Resize(num, num2, false);
			this.PingSceneColor.Resize(num, num2, false);
			this.PongSceneColor.Resize(num, num2, false);
			this.PingFinalSceneColor.Resize(num, num2, false);
			this.PongFinalSceneColor.Resize(num, num2, false);
			this.SceneColorHalfRes.Resize(width, height, false);
			this.LightBufferFullRes.Resize(num, num2, false);
			this.LightBufferHalfRes.Resize(width, height, false);
			this.Transparency.Resize(num, num2, false);
			this.TransparencyHalfRes.Resize(width, height, false);
			this.TransparencyQuarterRes.Resize(width2, height2, false);
			int width5 = (int)((float)num * this._momentsTransparencyResolutionScale.X);
			int height6 = (int)((float)num2 * this._momentsTransparencyResolutionScale.Y);
			this.MomentsTransparencyCapture.Resize(width5, height6, false);
			this.Distortion.Resize(width, height, false);
			this.DebugFXOverdraw.Resize(num, num2, false);
			int width6 = (int)((float)num * this._deferredShadowResolutionScale.X);
			int height7 = (int)((float)num2 * this._deferredShadowResolutionScale.Y);
			this.DeferredShadow.Resize(width6, height7, false);
			this.ResizeSSAOBuffers(num, num2, this._ssaoResolutionScale);
			this.SunshaftX.Resize(width2, height2, false);
			this.SunshaftY.Resize(width2, height2, false);
			this.SunRT.Resize(num, num2, false);
			this.VolumetricSunshaft.Resize(width, height, false);
			this.BlurXResBy32.Resize(width4, height5, false);
			this.BlurYResBy16.Resize(num3, height4, false);
			this.BlurXResBy16.Resize(num3, height4, false);
			this.BlurYResBy8.Resize(width3, height3, false);
			this.BlurXResBy8.Resize(width3, height3, false);
			this.BlurYResBy4.Resize(width2, height2, false);
			this.BlurXResBy4.Resize(width2, height2, false);
			this.BlurXResBy2.Resize(width, height, false);
			this.BlurYResBy2.Resize(width, height, false);
			this.Fill.Resize(width, height, false);
			this.NearFarField.Resize(width, height, false);
			this.NearCoCBlurY.Resize(width, height, false);
			this.NearCoCBlurX.Resize(width, height, false);
			this.NearCoCMaxY.Resize(width, height, false);
			this.NearCoCMaxX.Resize(width, height, false);
			this.Downsample.Resize(width, height, false);
			this.CoC.Resize(num, num2, false);
			this.DOFBlurY.Resize(width, height, false);
			this.DOFBlurX.Resize(width, height, false);
			this.DOFBlurYBis.Resize(width, height, false);
			this.DOFBlurXBis.Resize(width, height, false);
		}

		// Token: 0x060052E9 RID: 21225 RVA: 0x00172D70 File Offset: 0x00170F70
		public void SetMomentsTransparencyResolutionScale(float scale)
		{
			Debug.Assert(scale == 1f || scale == 0.5f || scale == 0.25f || scale == 0.125f, "Unsupported resolution scale for Moments RenderTarget.");
			this._momentsTransparencyResolutionScale = new Vector2(scale);
			int width = (int)((float)this.HardwareZ.Width * this._momentsTransparencyResolutionScale.X);
			int height = (int)((float)this.HardwareZ.Height * this._momentsTransparencyResolutionScale.Y);
			GLTexture texture = this.HardwareZ.GetTexture(RenderTarget.Target.Depth);
			bool flag = scale == 0.5f;
			if (flag)
			{
				texture = this.HardwareZHalfRes.GetTexture(RenderTarget.Target.Depth);
			}
			else
			{
				bool flag2 = scale == 0.25f;
				if (flag2)
				{
					texture = this.HardwareZQuarterRes.GetTexture(RenderTarget.Target.Depth);
				}
				else
				{
					bool flag3 = scale == 0.125f;
					if (flag3)
					{
						texture = this.HardwareZEighthRes.GetTexture(RenderTarget.Target.Depth);
					}
				}
			}
			this.MomentsTransparencyCapture.UseAsRenderTexture(texture, true, RenderTarget.Target.Depth, GL.DEPTH24_STENCIL8, GL.DEPTH_STENCIL, GL.UNSIGNED_INT_24_8, 1, 1);
			this.MomentsTransparencyCapture.FinalizeSetup();
			this.MomentsTransparencyCapture.Resize(width, height, false);
		}

		// Token: 0x060052EA RID: 21226 RVA: 0x00172E88 File Offset: 0x00171088
		public void SetDeferredShadowResolutionScale(float scale)
		{
			this._deferredShadowResolutionScale = new Vector2(scale);
			int width = (int)((float)this.GBuffer.Width * this._deferredShadowResolutionScale.X);
			int height = (int)((float)this.GBuffer.Height * this._deferredShadowResolutionScale.Y);
			this.DeferredShadow.Resize(width, height, false);
		}

		// Token: 0x060052EB RID: 21227 RVA: 0x00172EE5 File Offset: 0x001710E5
		public void ResizeShadowMap(int width, int height)
		{
			this.ShadowMap.Resize(width, height, false);
		}

		// Token: 0x060052EC RID: 21228 RVA: 0x00172EF8 File Offset: 0x001710F8
		public void ResizeSSAOBuffers(int gbufferWidth, int gbufferHeight, Vector2 ssaoResolutionScale)
		{
			this._ssaoResolutionScale = ssaoResolutionScale;
			int width = (int)((float)gbufferWidth * ssaoResolutionScale.X);
			int height = (int)((float)gbufferHeight * ssaoResolutionScale.Y);
			this.PingSSAO.Resize(width, height, false);
			this.PongSSAO.Resize(width, height, false);
			this.BlurSSAOAndShadowTmp.Resize(width, height, false);
			this.BlurSSAOAndShadow.Resize(width, height, false);
		}

		// Token: 0x060052ED RID: 21229 RVA: 0x00172F5F File Offset: 0x0017115F
		public void BeginFrame()
		{
			this.PingPongFinalRenderTarget();
			this.PingPongSSAOTarget();
		}

		// Token: 0x060052EE RID: 21230 RVA: 0x00172F70 File Offset: 0x00171170
		private void PingPongFinalRenderTarget()
		{
			bool flag = this.SceneColor == this.PingSceneColor;
			if (flag)
			{
				this.PreviousSceneColor = this.PingSceneColor;
				this.SceneColor = this.PongSceneColor;
				this.FinalSceneColor = this.PingFinalSceneColor;
				this.PreviousFinalSceneColor = this.PongFinalSceneColor;
			}
			else
			{
				this.PreviousSceneColor = this.PongSceneColor;
				this.SceneColor = this.PingSceneColor;
				this.FinalSceneColor = this.PongFinalSceneColor;
				this.PreviousFinalSceneColor = this.PingFinalSceneColor;
			}
		}

		// Token: 0x060052EF RID: 21231 RVA: 0x00172FF8 File Offset: 0x001711F8
		private void PingPongSSAOTarget()
		{
			bool flag = this.SSAORaw == this.PingSSAO;
			if (flag)
			{
				this.PreviousSSAORaw = this.PingSSAO;
				this.SSAORaw = this.PongSSAO;
			}
			else
			{
				this.PreviousSSAORaw = this.PongSSAO;
				this.SSAORaw = this.PingSSAO;
			}
		}

		// Token: 0x060052F0 RID: 21232 RVA: 0x00173050 File Offset: 0x00171250
		private void InitDebugMapInfos()
		{
			this._debugMapInfo.Add("blur", new RenderTargetStore.DebugMapParam(this.BlurYResBy4, RenderTarget.Target.Color0, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("scene_color_final", new RenderTargetStore.DebugMapParam(this.PingFinalSceneColor, RenderTarget.Target.Color0, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("scene_color_half", new RenderTargetStore.DebugMapParam(this.SceneColorHalfRes, RenderTarget.Target.Color0, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("gbuffer0", new RenderTargetStore.DebugMapParam(this.GBuffer, RenderTarget.Target.Color0, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("gbuffer_albedo", new RenderTargetStore.DebugMapParam(this.GBuffer, RenderTarget.Target.Color0, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.Color, RenderTargetStore.DebugMapParam.ColorChannelBits.RG, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("gbuffer_normal", new RenderTargetStore.DebugMapParam(this.GBuffer, RenderTarget.Target.Color0, false, false, true, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.BA, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("gbuffer1", new RenderTargetStore.DebugMapParam(this.GBuffer, RenderTarget.Target.Color2, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("gbuffer_light", new RenderTargetStore.DebugMapParam(this.GBuffer, RenderTarget.Target.Color2, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.Light, RenderTargetStore.DebugMapParam.ColorChannelBits.RG, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("gbuffer_sun", new RenderTargetStore.DebugMapParam(this.GBuffer, RenderTarget.Target.Color2, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.B, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("hw_z", new RenderTargetStore.DebugMapParam(this.HardwareZ, RenderTarget.Target.Depth, true, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("hw_z_half", new RenderTargetStore.DebugMapParam(this.HardwareZHalfRes, RenderTarget.Target.Depth, true, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("hw_z_quarter", new RenderTargetStore.DebugMapParam(this.HardwareZQuarterRes, RenderTarget.Target.Depth, true, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("hw_z_eighth", new RenderTargetStore.DebugMapParam(this.HardwareZEighthRes, RenderTarget.Target.Depth, true, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("linear_z", new RenderTargetStore.DebugMapParam(this.LinearZ, RenderTarget.Target.Color0, true, true, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("linear_z_half", new RenderTargetStore.DebugMapParam(this.LinearZHalfRes, RenderTarget.Target.Color0, true, true, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("edges", new RenderTargetStore.DebugMapParam(this.Edges, RenderTarget.Target.Color0, true, true, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("lbuffer", new RenderTargetStore.DebugMapParam(this.LightBufferFullRes, RenderTarget.Target.Color0, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.Light, RenderTargetStore.DebugMapParam.ColorChannelBits.RG, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("lbuffer_low", new RenderTargetStore.DebugMapParam(this.LightBufferHalfRes, RenderTarget.Target.Color0, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("blend_moment", new RenderTargetStore.DebugMapParam(this.MomentsTransparencyCapture, RenderTarget.Target.Color0, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("blend_tod", new RenderTargetStore.DebugMapParam(this.MomentsTransparencyCapture, RenderTarget.Target.Color1, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.R, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("blend_accu", new RenderTargetStore.DebugMapParam(this.Transparency, RenderTarget.Target.Color0, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("blend_weight", new RenderTargetStore.DebugMapParam(this.Transparency, RenderTarget.Target.Color0, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.A, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("blend_reveal", new RenderTargetStore.DebugMapParam(this.Transparency, RenderTarget.Target.Color1, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.R, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("blend_add", new RenderTargetStore.DebugMapParam(this.Transparency, RenderTarget.Target.Color1, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.A, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("blend_beta", new RenderTargetStore.DebugMapParam(this.Transparency, RenderTarget.Target.Color1, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.A, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("blend_accu_lowres", new RenderTargetStore.DebugMapParam(this.TransparencyHalfRes, RenderTarget.Target.Color0, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("blend_weight_lowres", new RenderTargetStore.DebugMapParam(this.TransparencyHalfRes, RenderTarget.Target.Color0, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.A, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("blend_reveal_lowres", new RenderTargetStore.DebugMapParam(this.TransparencyHalfRes, RenderTarget.Target.Color1, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.R, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("blend_add_lowres", new RenderTargetStore.DebugMapParam(this.TransparencyHalfRes, RenderTarget.Target.Color1, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.A, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("blend_beta_lowres", new RenderTargetStore.DebugMapParam(this.TransparencyHalfRes, RenderTarget.Target.Color1, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.A, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("shadowmap", new RenderTargetStore.DebugMapParam(this.ShadowMap, RenderTarget.Target.Depth, true, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("deferredshadow", new RenderTargetStore.DebugMapParam(this.DeferredShadow, RenderTarget.Target.Color0, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.R, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("shadow", new RenderTargetStore.DebugMapParam(this.BlurSSAOAndShadow, RenderTarget.Target.Color0, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.A, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("ssao", new RenderTargetStore.DebugMapParam(this.BlurSSAOAndShadow, RenderTarget.Target.Color0, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.R, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("ssao_raw", new RenderTargetStore.DebugMapParam(this.SSAORaw, RenderTarget.Target.Color0, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.R, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("bloom", new RenderTargetStore.DebugMapParam(this.BlurXResBy2, RenderTarget.Target.Color0, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("vol_sunshaft", new RenderTargetStore.DebugMapParam(this.VolumetricSunshaft, RenderTarget.Target.Color0, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("sun_occlusion", new RenderTargetStore.DebugMapParam(this.SunOcclusionBufferLowRes, RenderTarget.Target.Color0, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, 1f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("sun_occlusion_history", new RenderTargetStore.DebugMapParam(this.SunOcclusionHistory, RenderTarget.Target.Color0, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, 1f, 0.1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("distortion", new RenderTargetStore.DebugMapParam(this.Distortion, RenderTarget.Target.Color0, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, 50f, 1f, 1f, 1f, 0f));
			this._debugMapInfo.Add("overdraw", new RenderTargetStore.DebugMapParam(this.DebugFXOverdraw, RenderTarget.Target.Color0, false, false, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits.R, 1f, 1f, 1f, 1f, 10f));
		}

		// Token: 0x060052F1 RID: 21233 RVA: 0x001739EC File Offset: 0x00171BEC
		public void SetDebugMapChromaSubsamplingMode(string mapName, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode chromaSubsamplingMode)
		{
			RenderTargetStore.DebugMapParam value;
			bool flag = this._debugMapInfo.TryGetValue(mapName, out value);
			if (flag)
			{
				value.ChromaSubSamplingMode = chromaSubsamplingMode;
				value.ColorChannels = ((chromaSubsamplingMode != RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None) ? RenderTargetStore.DebugMapParam.ColorChannelBits.RG : RenderTargetStore.DebugMapParam.ColorChannelBits.RGB);
				this._debugMapInfo[mapName] = value;
			}
		}

		// Token: 0x060052F2 RID: 21234 RVA: 0x00173A34 File Offset: 0x00171C34
		public string GetDebugMapList()
		{
			return string.Format("{0}", string.Join(", ", this._debugMapInfo.Keys));
		}

		// Token: 0x060052F3 RID: 21235 RVA: 0x00173A68 File Offset: 0x00171C68
		public bool ContainsDebugMap(string mapName)
		{
			return this._debugMapInfo.ContainsKey(mapName);
		}

		// Token: 0x060052F4 RID: 21236 RVA: 0x00173A86 File Offset: 0x00171C86
		public void RegisterDebugMap(string mapName, Texture texture)
		{
			this._debugMapInfo.Add(mapName, new RenderTargetStore.DebugMapParam(texture, RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, false));
		}

		// Token: 0x060052F5 RID: 21237 RVA: 0x00173A9F File Offset: 0x00171C9F
		public void RegisterDebugMap2DArray(string mapName, GLTexture texture, int width, int height, int layerCount)
		{
			this._debugMapInfo.Add(mapName, new RenderTargetStore.DebugMapParam(texture, width, height, layerCount));
		}

		// Token: 0x060052F6 RID: 21238 RVA: 0x00173ABA File Offset: 0x00171CBA
		public void RegisterDebugMapCubemap(string mapName, Texture texture)
		{
			this._debugMapInfo.Add(mapName, new RenderTargetStore.DebugMapParam(texture, RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, true));
		}

		// Token: 0x060052F7 RID: 21239 RVA: 0x00173AD3 File Offset: 0x00171CD3
		public void UnregisterDebugMap(string mapName)
		{
			this._debugMapInfo.Remove(mapName);
		}

		// Token: 0x060052F8 RID: 21240 RVA: 0x00173AE4 File Offset: 0x00171CE4
		public void SetDebugMapViewport(string mapName, float width, float height)
		{
			RenderTargetStore.DebugMapParam value;
			bool flag = this._debugMapInfo.TryGetValue(mapName, out value);
			if (flag)
			{
				value.ViewportScale = new Vector2(width, height);
				this._debugMapInfo[mapName] = value;
			}
		}

		// Token: 0x060052F9 RID: 21241 RVA: 0x00173B24 File Offset: 0x00171D24
		public void SetDebugMapScale(string mapName, float scale)
		{
			RenderTargetStore.DebugMapParam value;
			bool flag = this._debugMapInfo.TryGetValue(mapName, out value);
			if (flag)
			{
				value.Scale = scale;
				this._debugMapInfo[mapName] = value;
			}
		}

		// Token: 0x060052FA RID: 21242 RVA: 0x00173B5C File Offset: 0x00171D5C
		public void DebugDrawMaps(string[] mapNames, bool verticalDisplay, float opacity = 0f, int mipLevel = 0, int layer = 0)
		{
			GLFunctions gl = this._graphics.GL;
			bool flag = false;
			Vector4 viewport = new Vector4(0f, 0f, this._viewportSize.X, this._viewportSize.Y);
			GLTexture map = GLTexture.None;
			bool debugAsTexture2DArray = false;
			bool flag2 = false;
			int textureWidth = 0;
			int textureHeight = 0;
			for (int i = 0; i < mapNames.Length; i++)
			{
				RenderTargetStore.DebugMapParam debugMapParam = this._debugMapInfo[mapNames[i]];
				switch (debugMapParam.InputType)
				{
				case RenderTargetStore.DebugMapParam.DebugMapInputType.Texture2D:
					debugAsTexture2DArray = false;
					flag2 = false;
					map = debugMapParam.Texture2D.GLTexture;
					textureWidth = debugMapParam.Texture2D.Width;
					textureHeight = debugMapParam.Texture2D.Height;
					break;
				case RenderTargetStore.DebugMapParam.DebugMapInputType.Texture2DArray:
					debugAsTexture2DArray = true;
					flag2 = false;
					map = debugMapParam.Texture2DArray.Texture;
					textureWidth = debugMapParam.Texture2DArray.Width;
					textureHeight = debugMapParam.Texture2DArray.Height;
					break;
				case RenderTargetStore.DebugMapParam.DebugMapInputType.RenderTarget:
					debugAsTexture2DArray = false;
					flag2 = false;
					map = debugMapParam.RenderTarget.GetTexture(debugMapParam.Target);
					textureWidth = debugMapParam.RenderTarget.Width;
					textureHeight = debugMapParam.RenderTarget.Height;
					break;
				case RenderTargetStore.DebugMapParam.DebugMapInputType.Cubemap:
					debugAsTexture2DArray = false;
					flag2 = true;
					map = debugMapParam.Texture2D.GLTexture;
					textureWidth = debugMapParam.Texture2D.Width;
					textureHeight = debugMapParam.Texture2D.Height;
					break;
				default:
					Debug.Assert(false, string.Format("Invalid DebugMapInputType {0}", (int)debugMapParam.InputType));
					break;
				}
				float num = (mapNames.Length == 1) ? debugMapParam.Scale : 1f;
				Vector2 vector = debugMapParam.ViewportScale * this._viewportSize;
				bool flag3 = mapNames.Length > 1 || vector != Vector2.Zero || num != 1f;
				if (flag3)
				{
					float num2 = 1f / (float)mapNames.Length;
					flag = true;
					vector *= num * num2;
					int num3 = (!verticalDisplay) ? ((int)((float)i * num2 * this._viewportSize.X)) : 0;
					int num4 = verticalDisplay ? ((int)((float)i * num2 * this._viewportSize.Y)) : 0;
					viewport = new Vector4((float)num3, (float)num4, (float)((int)vector.X), (float)((int)vector.Y));
					gl.Viewport(num3, num4, (int)vector.X, (int)vector.Y);
				}
				bool flag4 = flag2;
				if (flag4)
				{
					flag = true;
					float num5 = this._viewportSize.Y / 3f;
					float num6 = this._viewportSize.X / 4f;
					gl.Viewport((int)(2f * num6), (int)num5, (int)num6, (int)num5);
					this.DebugDrawMap(debugMapParam.HasZValues, debugMapParam.HasLinearZValues, debugAsTexture2DArray, 1, debugMapParam.UseNormalQuantization, debugMapParam.ChromaSubSamplingMode, (int)debugMapParam.ColorChannels, textureWidth, textureHeight, map, opacity, mipLevel, layer, debugMapParam.Multiplier, debugMapParam.DebugMaxOverdraw, viewport);
					gl.Viewport(0, (int)num5, (int)num6, (int)num5);
					this.DebugDrawMap(debugMapParam.HasZValues, debugMapParam.HasLinearZValues, debugAsTexture2DArray, 2, debugMapParam.UseNormalQuantization, debugMapParam.ChromaSubSamplingMode, (int)debugMapParam.ColorChannels, textureWidth, textureHeight, map, opacity, mipLevel, layer, debugMapParam.Multiplier, debugMapParam.DebugMaxOverdraw, viewport);
					gl.Viewport((int)num6, (int)(2f * num5), (int)num6, (int)num5);
					this.DebugDrawMap(debugMapParam.HasZValues, debugMapParam.HasLinearZValues, debugAsTexture2DArray, 3, debugMapParam.UseNormalQuantization, debugMapParam.ChromaSubSamplingMode, (int)debugMapParam.ColorChannels, textureWidth, textureHeight, map, opacity, mipLevel, layer, debugMapParam.Multiplier, debugMapParam.DebugMaxOverdraw, viewport);
					gl.Viewport((int)num6, 0, (int)num6, (int)num5);
					this.DebugDrawMap(debugMapParam.HasZValues, debugMapParam.HasLinearZValues, debugAsTexture2DArray, 4, debugMapParam.UseNormalQuantization, debugMapParam.ChromaSubSamplingMode, (int)debugMapParam.ColorChannels, textureWidth, textureHeight, map, opacity, mipLevel, layer, debugMapParam.Multiplier, debugMapParam.DebugMaxOverdraw, viewport);
					gl.Viewport((int)num6, (int)num5, (int)num6, (int)num5);
					this.DebugDrawMap(debugMapParam.HasZValues, debugMapParam.HasLinearZValues, debugAsTexture2DArray, 5, debugMapParam.UseNormalQuantization, debugMapParam.ChromaSubSamplingMode, (int)debugMapParam.ColorChannels, textureWidth, textureHeight, map, opacity, mipLevel, layer, debugMapParam.Multiplier, debugMapParam.DebugMaxOverdraw, viewport);
					gl.Viewport((int)(3f * num6), (int)num5, (int)num6, (int)num5);
					this.DebugDrawMap(debugMapParam.HasZValues, debugMapParam.HasLinearZValues, debugAsTexture2DArray, 6, debugMapParam.UseNormalQuantization, debugMapParam.ChromaSubSamplingMode, (int)debugMapParam.ColorChannels, textureWidth, textureHeight, map, opacity, mipLevel, layer, debugMapParam.Multiplier, debugMapParam.DebugMaxOverdraw, viewport);
				}
				else
				{
					this.DebugDrawMap(debugMapParam.HasZValues, debugMapParam.HasLinearZValues, debugAsTexture2DArray, 0, debugMapParam.UseNormalQuantization, debugMapParam.ChromaSubSamplingMode, (int)debugMapParam.ColorChannels, textureWidth, textureHeight, map, opacity, mipLevel, layer, debugMapParam.Multiplier, debugMapParam.DebugMaxOverdraw, viewport);
				}
			}
			bool flag5 = flag;
			if (flag5)
			{
				gl.Viewport(0, 0, (int)this._viewportSize.X, (int)this._viewportSize.Y);
			}
		}

		// Token: 0x060052FB RID: 21243 RVA: 0x001740B4 File Offset: 0x001722B4
		public void DebugDrawMap(bool debugAsZMap, bool debugAsLinearZ, bool debugAsTexture2DArray, int cubemapFaceID, bool useNormalQuantization, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode chromaSubsamplingMode, int colorChannels, int textureWidth, int textureHeight, GLTexture map, float opacity, int mipLevel, int layer, float multiplier, float debugMaxOverdraw, Vector4 viewport)
		{
			GLFunctions gl = this._graphics.GL;
			DebugDrawMapProgram debugDrawMapProgram = this._graphics.GPUProgramStore.DebugDrawMapProgram;
			gl.UseProgram(debugDrawMapProgram);
			if (debugAsTexture2DArray)
			{
				gl.ActiveTexture(GL.TEXTURE1);
				gl.BindTexture(GL.TEXTURE_2D_ARRAY, map);
				gl.ActiveTexture(GL.TEXTURE0);
			}
			else
			{
				bool flag = cubemapFaceID != 0;
				if (flag)
				{
					gl.ActiveTexture(GL.TEXTURE2);
					gl.BindTexture(GL.TEXTURE_CUBE_MAP, map);
					gl.ActiveTexture(GL.TEXTURE0);
				}
				else
				{
					gl.ActiveTexture(GL.TEXTURE0);
					gl.BindTexture(GL.TEXTURE_2D, map);
				}
			}
			int value = debugAsLinearZ ? 1 : 0;
			int value2 = (debugAsLinearZ || debugAsZMap) ? 1 : 0;
			int value3 = debugAsTexture2DArray ? 1 : 0;
			debugDrawMapProgram.DebugZ.SetValue(value2);
			debugDrawMapProgram.LinearZ.SetValue(value);
			debugDrawMapProgram.DebugTexture2DArray.SetValue(value3);
			debugDrawMapProgram.CubemapFace.SetValue(cubemapFaceID);
			debugDrawMapProgram.NormalQuantization.SetValue(useNormalQuantization ? 1 : 0);
			debugDrawMapProgram.ChromaSubsampling.SetValue((int)chromaSubsamplingMode);
			debugDrawMapProgram.ColorChannels.SetValue(colorChannels);
			debugDrawMapProgram.Opacity.SetValue(opacity);
			debugDrawMapProgram.MipLevel.SetValue(mipLevel);
			debugDrawMapProgram.Layer.SetValue(layer);
			debugDrawMapProgram.Multiplier.SetValue(multiplier);
			debugDrawMapProgram.DebugMaxOverdraw.SetValue(debugMaxOverdraw);
			debugDrawMapProgram.TextureSize.SetValue((float)textureWidth, (float)textureHeight);
			viewport = ((viewport == Vector4.One) ? new Vector4(0f, 0f, this._viewportSize.X, this._viewportSize.Y) : viewport);
			debugDrawMapProgram.Viewport.SetValue(viewport);
			this._graphics.ScreenTriangleRenderer.Draw();
		}

		// Token: 0x04002DBC RID: 11708
		public RenderTarget HardwareZ;

		// Token: 0x04002DBD RID: 11709
		public RenderTarget HardwareZHalfRes;

		// Token: 0x04002DBE RID: 11710
		public RenderTarget HardwareZQuarterRes;

		// Token: 0x04002DBF RID: 11711
		public RenderTarget HardwareZEighthRes;

		// Token: 0x04002DC0 RID: 11712
		public RenderTarget LinearZ;

		// Token: 0x04002DC1 RID: 11713
		public RenderTarget LinearZHalfRes;

		// Token: 0x04002DC2 RID: 11714
		public RenderTarget Edges;

		// Token: 0x04002DC3 RID: 11715
		public RenderTarget GBuffer;

		// Token: 0x04002DC4 RID: 11716
		public RenderTarget PingSceneColor;

		// Token: 0x04002DC5 RID: 11717
		public RenderTarget PongSceneColor;

		// Token: 0x04002DC6 RID: 11718
		public RenderTarget PingFinalSceneColor;

		// Token: 0x04002DC7 RID: 11719
		public RenderTarget PongFinalSceneColor;

		// Token: 0x04002DC8 RID: 11720
		public RenderTarget SceneColorHalfRes;

		// Token: 0x04002DC9 RID: 11721
		public RenderTarget LightBufferFullRes;

		// Token: 0x04002DCA RID: 11722
		public RenderTarget LightBufferHalfRes;

		// Token: 0x04002DCB RID: 11723
		public RenderTarget Transparency;

		// Token: 0x04002DCC RID: 11724
		public RenderTarget TransparencyHalfRes;

		// Token: 0x04002DCD RID: 11725
		public RenderTarget TransparencyQuarterRes;

		// Token: 0x04002DCE RID: 11726
		public RenderTarget MomentsTransparencyCapture;

		// Token: 0x04002DCF RID: 11727
		public RenderTarget Distortion;

		// Token: 0x04002DD0 RID: 11728
		public RenderTarget DebugFXOverdraw;

		// Token: 0x04002DD1 RID: 11729
		public RenderTarget VolumetricSunshaft;

		// Token: 0x04002DD2 RID: 11730
		public RenderTarget ShadowMap;

		// Token: 0x04002DD3 RID: 11731
		public RenderTarget DeferredShadow;

		// Token: 0x04002DD4 RID: 11732
		public RenderTarget PingSSAO;

		// Token: 0x04002DD5 RID: 11733
		public RenderTarget PongSSAO;

		// Token: 0x04002DD6 RID: 11734
		public RenderTarget BlurSSAOAndShadowTmp;

		// Token: 0x04002DD7 RID: 11735
		public RenderTarget BlurSSAOAndShadow;

		// Token: 0x04002DD8 RID: 11736
		public RenderTarget DOFBlurXBis;

		// Token: 0x04002DD9 RID: 11737
		public RenderTarget DOFBlurYBis;

		// Token: 0x04002DDA RID: 11738
		public RenderTarget DOFBlurX;

		// Token: 0x04002DDB RID: 11739
		public RenderTarget DOFBlurY;

		// Token: 0x04002DDC RID: 11740
		public RenderTarget CoC;

		// Token: 0x04002DDD RID: 11741
		public RenderTarget Downsample;

		// Token: 0x04002DDE RID: 11742
		public RenderTarget NearCoCMaxX;

		// Token: 0x04002DDF RID: 11743
		public RenderTarget NearCoCMaxY;

		// Token: 0x04002DE0 RID: 11744
		public RenderTarget NearCoCBlurX;

		// Token: 0x04002DE1 RID: 11745
		public RenderTarget NearCoCBlurY;

		// Token: 0x04002DE2 RID: 11746
		public RenderTarget NearFarField;

		// Token: 0x04002DE3 RID: 11747
		public RenderTarget Fill;

		// Token: 0x04002DE4 RID: 11748
		public RenderTarget BlurXResBy2;

		// Token: 0x04002DE5 RID: 11749
		public RenderTarget BlurYResBy2;

		// Token: 0x04002DE6 RID: 11750
		public RenderTarget BlurXResBy4;

		// Token: 0x04002DE7 RID: 11751
		public RenderTarget BlurYResBy4;

		// Token: 0x04002DE8 RID: 11752
		public RenderTarget BlurXResBy8;

		// Token: 0x04002DE9 RID: 11753
		public RenderTarget BlurYResBy8;

		// Token: 0x04002DEA RID: 11754
		public RenderTarget BlurXResBy16;

		// Token: 0x04002DEB RID: 11755
		public RenderTarget BlurYResBy16;

		// Token: 0x04002DEC RID: 11756
		public RenderTarget BlurXResBy32;

		// Token: 0x04002DED RID: 11757
		public RenderTarget SunRT;

		// Token: 0x04002DEE RID: 11758
		public RenderTarget SunshaftX;

		// Token: 0x04002DEF RID: 11759
		public RenderTarget SunshaftY;

		// Token: 0x04002DF0 RID: 11760
		public RenderTarget SunOcclusionBufferLowRes;

		// Token: 0x04002DF1 RID: 11761
		public RenderTarget SunOcclusionHistory;

		// Token: 0x04002DF2 RID: 11762
		public RenderTarget SSAORaw;

		// Token: 0x04002DF3 RID: 11763
		public RenderTarget PreviousSSAORaw;

		// Token: 0x04002DF4 RID: 11764
		public RenderTarget SceneColor;

		// Token: 0x04002DF5 RID: 11765
		public RenderTarget PreviousSceneColor;

		// Token: 0x04002DF6 RID: 11766
		public RenderTarget FinalSceneColor;

		// Token: 0x04002DF7 RID: 11767
		public RenderTarget PreviousFinalSceneColor;

		// Token: 0x04002DF8 RID: 11768
		private GraphicsDevice _graphics;

		// Token: 0x04002DF9 RID: 11769
		private Vector2 _momentsTransparencyResolutionScale;

		// Token: 0x04002DFA RID: 11770
		private Vector2 _deferredShadowResolutionScale;

		// Token: 0x04002DFB RID: 11771
		private Vector2 _ssaoResolutionScale;

		// Token: 0x04002DFC RID: 11772
		private Vector2 _viewportSize;

		// Token: 0x04002DFD RID: 11773
		private Dictionary<string, RenderTargetStore.DebugMapParam> _debugMapInfo = new Dictionary<string, RenderTargetStore.DebugMapParam>();

		// Token: 0x02000EBC RID: 3772
		public struct DebugMapParam
		{
			// Token: 0x060067EB RID: 26603 RVA: 0x00219428 File Offset: 0x00217628
			public DebugMapParam(Texture texture, RenderTargetStore.DebugMapParam.ColorChannelBits colorChannels = RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, bool isACubemap = false)
			{
				this.InputType = (isACubemap ? RenderTargetStore.DebugMapParam.DebugMapInputType.Cubemap : RenderTargetStore.DebugMapParam.DebugMapInputType.Texture2D);
				this.Texture2D = texture;
				this.Texture2DArray.Texture = GLTexture.None;
				this.Texture2DArray.Width = 0;
				this.Texture2DArray.Height = 0;
				this.Texture2DArray.LayerCount = 0;
				this.RenderTarget = null;
				this.Target = RenderTarget.Target.MAX;
				this.HasZValues = false;
				this.HasLinearZValues = false;
				this.UseNormalQuantization = false;
				this.ChromaSubSamplingMode = RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None;
				this.Multiplier = 1f;
				this.DebugMaxOverdraw = 0f;
				this.ColorChannels = colorChannels;
				this.ViewportScale = Vector2.One;
				this.Scale = 1f;
			}

			// Token: 0x060067EC RID: 26604 RVA: 0x002194DC File Offset: 0x002176DC
			public DebugMapParam(RenderTarget renderTarget, RenderTarget.Target target, bool hasZValues, bool hasLinearZValues, bool useNormalQuantization = false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode chromaSubsamplingMode = RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, RenderTargetStore.DebugMapParam.ColorChannelBits colorChannels = RenderTargetStore.DebugMapParam.ColorChannelBits.RGB, float multiplier = 1f, float widthScale = 1f, float heightScale = 1f, float scale = 1f, float debugMaxOverdraw = 0f)
			{
				this.InputType = RenderTargetStore.DebugMapParam.DebugMapInputType.RenderTarget;
				this.Texture2D = null;
				this.Texture2DArray.Texture = GLTexture.None;
				this.Texture2DArray.Width = 0;
				this.Texture2DArray.Height = 0;
				this.Texture2DArray.LayerCount = 0;
				this.RenderTarget = renderTarget;
				this.Target = target;
				this.HasZValues = hasZValues;
				this.HasLinearZValues = hasLinearZValues;
				this.UseNormalQuantization = useNormalQuantization;
				this.ChromaSubSamplingMode = chromaSubsamplingMode;
				this.Multiplier = multiplier;
				this.DebugMaxOverdraw = debugMaxOverdraw;
				this.ColorChannels = colorChannels;
				this.ViewportScale = new Vector2(widthScale, heightScale);
				this.Scale = scale;
			}

			// Token: 0x060067ED RID: 26605 RVA: 0x00219588 File Offset: 0x00217788
			public DebugMapParam(GLTexture texture, int width, int height, int layerCount)
			{
				this.InputType = RenderTargetStore.DebugMapParam.DebugMapInputType.Texture2DArray;
				this.Texture2D = null;
				this.Texture2DArray.Texture = texture;
				this.Texture2DArray.Width = width;
				this.Texture2DArray.Height = height;
				this.Texture2DArray.LayerCount = layerCount;
				this.RenderTarget = null;
				this.Target = RenderTarget.Target.MAX;
				this.HasZValues = false;
				this.HasLinearZValues = false;
				this.UseNormalQuantization = false;
				this.ChromaSubSamplingMode = RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None;
				this.Multiplier = 1f;
				this.DebugMaxOverdraw = 0f;
				this.ColorChannels = RenderTargetStore.DebugMapParam.ColorChannelBits.RGB;
				this.ViewportScale = Vector2.One;
				this.Scale = 1f;
			}

			// Token: 0x040047F3 RID: 18419
			public RenderTargetStore.DebugMapParam.DebugMapInputType InputType;

			// Token: 0x040047F4 RID: 18420
			public Texture Texture2D;

			// Token: 0x040047F5 RID: 18421
			public RenderTargetStore.DebugMapParam.Texture2DArrayInfo Texture2DArray;

			// Token: 0x040047F6 RID: 18422
			public RenderTarget RenderTarget;

			// Token: 0x040047F7 RID: 18423
			public RenderTarget.Target Target;

			// Token: 0x040047F8 RID: 18424
			public bool HasZValues;

			// Token: 0x040047F9 RID: 18425
			public bool HasLinearZValues;

			// Token: 0x040047FA RID: 18426
			public bool UseNormalQuantization;

			// Token: 0x040047FB RID: 18427
			public RenderTargetStore.DebugMapParam.ChromaSubsamplingMode ChromaSubSamplingMode;

			// Token: 0x040047FC RID: 18428
			public float Multiplier;

			// Token: 0x040047FD RID: 18429
			public float DebugMaxOverdraw;

			// Token: 0x040047FE RID: 18430
			public RenderTargetStore.DebugMapParam.ColorChannelBits ColorChannels;

			// Token: 0x040047FF RID: 18431
			public Vector2 ViewportScale;

			// Token: 0x04004800 RID: 18432
			public float Scale;

			// Token: 0x020010A3 RID: 4259
			public enum ColorChannelBits
			{
				// Token: 0x04004EAE RID: 20142
				A = 1,
				// Token: 0x04004EAF RID: 20143
				B,
				// Token: 0x04004EB0 RID: 20144
				G = 4,
				// Token: 0x04004EB1 RID: 20145
				R = 8,
				// Token: 0x04004EB2 RID: 20146
				BA = 3,
				// Token: 0x04004EB3 RID: 20147
				GB = 6,
				// Token: 0x04004EB4 RID: 20148
				RB = 10,
				// Token: 0x04004EB5 RID: 20149
				RG = 12,
				// Token: 0x04004EB6 RID: 20150
				RGB = 14
			}

			// Token: 0x020010A4 RID: 4260
			public enum ChromaSubsamplingMode
			{
				// Token: 0x04004EB8 RID: 20152
				None,
				// Token: 0x04004EB9 RID: 20153
				Color,
				// Token: 0x04004EBA RID: 20154
				Light
			}

			// Token: 0x020010A5 RID: 4261
			public enum DebugMapInputType
			{
				// Token: 0x04004EBC RID: 20156
				Texture2D,
				// Token: 0x04004EBD RID: 20157
				Texture2DArray,
				// Token: 0x04004EBE RID: 20158
				RenderTarget,
				// Token: 0x04004EBF RID: 20159
				Cubemap
			}

			// Token: 0x020010A6 RID: 4262
			public struct Texture2DArrayInfo
			{
				// Token: 0x04004EC0 RID: 20160
				public GLTexture Texture;

				// Token: 0x04004EC1 RID: 20161
				public int Width;

				// Token: 0x04004EC2 RID: 20162
				public int Height;

				// Token: 0x04004EC3 RID: 20163
				public int LayerCount;
			}
		}
	}
}
