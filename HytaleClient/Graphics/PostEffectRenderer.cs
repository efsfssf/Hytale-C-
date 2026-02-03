using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A41 RID: 2625
	internal class PostEffectRenderer
	{
		// Token: 0x170012C9 RID: 4809
		// (get) Token: 0x06005275 RID: 21109 RVA: 0x0016B9EF File Offset: 0x00169BEF
		public bool NeedsJittering
		{
			get
			{
				return this._postEffectSettings.UseTemporalAA && !this.NeedsScreenBlur;
			}
		}

		// Token: 0x170012CA RID: 4810
		// (get) Token: 0x06005276 RID: 21110 RVA: 0x0016BA0A File Offset: 0x00169C0A
		public bool IsBloomEnabled
		{
			get
			{
				return this._postEffectSettings.UseBloom;
			}
		}

		// Token: 0x170012CB RID: 4811
		// (get) Token: 0x06005277 RID: 21111 RVA: 0x0016BA17 File Offset: 0x00169C17
		public bool IsTemporalAAEnabled
		{
			get
			{
				return this._postEffectSettings.UseTemporalAA;
			}
		}

		// Token: 0x170012CC RID: 4812
		// (get) Token: 0x06005278 RID: 21112 RVA: 0x0016BA24 File Offset: 0x00169C24
		public bool IsDepthOfFieldEnabled
		{
			get
			{
				return this._postEffectSettings.UseDepthOfField;
			}
		}

		// Token: 0x170012CD RID: 4813
		// (get) Token: 0x06005279 RID: 21113 RVA: 0x0016BA31 File Offset: 0x00169C31
		public bool IsDistortionEnabled
		{
			get
			{
				return this._postEffectProgram.UseDistortion;
			}
		}

		// Token: 0x0600527A RID: 21114 RVA: 0x0016BA40 File Offset: 0x00169C40
		public PostEffectRenderer(GraphicsDevice graphics, Profiling profiling, PostEffectProgram program)
		{
			this._graphics = graphics;
			this._gpuProgramStore = graphics.GPUProgramStore;
			this._gl = this._graphics.GL;
			this._profiling = profiling;
			this._postEffectProgram = program;
			this.InitSampler();
			this._postEffectSettings = default(PostEffectRenderer.Settings);
			this._postEffectSettings.InitDefault();
			this._postEffectDrawParameters = default(PostEffectRenderer.DrawParams);
			this._postEffectDrawParameters.InitDefault();
			this.SetupDepthOfField(1f, 2f, 30f, 70f, 0.5f, 0.3f);
		}

		// Token: 0x0600527B RID: 21115 RVA: 0x0016BAE3 File Offset: 0x00169CE3
		public void Dispose()
		{
			this.DisposeSampler();
		}

		// Token: 0x0600527C RID: 21116 RVA: 0x0016BAF0 File Offset: 0x00169CF0
		private void InitSampler()
		{
			this._linearSampler = this._gl.GenSampler();
			this._gl.SamplerParameteri(this._linearSampler, GL.TEXTURE_WRAP_S, GL.CLAMP_TO_EDGE);
			this._gl.SamplerParameteri(this._linearSampler, GL.TEXTURE_WRAP_T, GL.CLAMP_TO_EDGE);
			this._gl.SamplerParameteri(this._linearSampler, GL.TEXTURE_MIN_FILTER, GL.LINEAR);
			this._gl.SamplerParameteri(this._linearSampler, GL.TEXTURE_MAG_FILTER, GL.LINEAR);
		}

		// Token: 0x0600527D RID: 21117 RVA: 0x0016BB7F File Offset: 0x00169D7F
		private void DisposeSampler()
		{
			this._gl.DeleteSampler(this._linearSampler);
		}

		// Token: 0x0600527E RID: 21118 RVA: 0x0016BB94 File Offset: 0x00169D94
		public void Resize(int width, int height, float renderScale)
		{
			this._width = width;
			this._height = height;
			this._renderScale = renderScale;
		}

		// Token: 0x0600527F RID: 21119 RVA: 0x0016BBAC File Offset: 0x00169DAC
		public void SetupRenderingProfiles(int renderingProfileDepthOfField, int renderingProfileBloom, int renderingProfileCombineAndFxaa, int renderingProfileTaa, int renderingProfileBlur)
		{
			this._renderingProfileDepthOfField = renderingProfileDepthOfField;
			this._renderingProfileBloom = renderingProfileBloom;
			this._renderingProfileCombineAndFxaa = renderingProfileCombineAndFxaa;
			this._renderingProfileTaa = renderingProfileTaa;
			this._renderingProfileBlur = renderingProfileBlur;
		}

		// Token: 0x06005280 RID: 21120 RVA: 0x0016BBD4 File Offset: 0x00169DD4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SetInputs(GLTexture input, GLTexture inputFX, int width, int height, float renderScale)
		{
			this._input = input;
			this._inputFX = inputFX;
			this.Resize(width, height, renderScale);
		}

		// Token: 0x06005281 RID: 21121 RVA: 0x0016BBF1 File Offset: 0x00169DF1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SetOutput(RenderTarget output)
		{
			this._outputRenderTarget = output;
		}

		// Token: 0x06005282 RID: 21122 RVA: 0x0016BBFC File Offset: 0x00169DFC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void BindOutputFramebuffer()
		{
			bool flag = this._outputRenderTarget != null;
			if (flag)
			{
				this._outputRenderTarget.Bind(true, true);
			}
			else
			{
				RenderTarget.BindHardwareFramebuffer();
			}
		}

		// Token: 0x06005283 RID: 21123 RVA: 0x0016BC30 File Offset: 0x00169E30
		public void Draw(GLTexture input, GLTexture inputFX, int width, int height, float renderScale, RenderTarget output = null)
		{
			this.SetInputs(input, inputFX, width, height, renderScale);
			this.SetOutput(output);
			bool flag = this._input == GLTexture.None;
			if (flag)
			{
				throw new Exception("RenderTarget was never specified! you are missing a call to SetInputs");
			}
			this._gl.AssertDisabled(GL.BLEND);
			this._gl.AssertActiveTexture(GL.TEXTURE0);
			bool useBloom = this._postEffectSettings.UseBloom;
			if (useBloom)
			{
				bool flag2 = this._postEffectSettings.BloomSettings.UseSun && this._postEffectSettings.BloomSettings.DrawSun == null;
				if (flag2)
				{
					throw new Exception("Bloom : Action DrawSun was never specified! you are missing a call to InitBloom");
				}
				bool flag3 = this._postEffectSettings.BloomSettings.UseMoon && this._postEffectSettings.BloomSettings.DrawMoon == null;
				if (flag3)
				{
					throw new Exception("Bloom : Action DrawSun was never specified! you are missing a call to InitBloom");
				}
				bool flag4 = this._postEffectSettings.BloomSettings.UseSun && this._postEffectDrawParameters.BloomParams.SunColor == new Vector3(-1f);
				if (flag4)
				{
					throw new Exception("Bloom : SunMVP was never specified! you are missing a call to UpdateBloomParameters");
				}
				this._profiling.StartMeasure(this._renderingProfileBloom);
				this.DrawBloom();
				this._profiling.StopMeasure(this._renderingProfileBloom);
			}
			else
			{
				this._profiling.SkipMeasure(this._renderingProfileBloom);
			}
			bool useDepthOfField = this._postEffectSettings.UseDepthOfField;
			if (useDepthOfField)
			{
				bool flag5 = this._postEffectDrawParameters.DepthOfFieldParams.ProjectionMatrix == Matrix.Identity;
				if (flag5)
				{
					throw new Exception("Depth of field : Projection Matrix was not updated ! you are missing a call to UpdateDepthOfFieldParameters");
				}
				bool flag6 = this._depthInput == GLTexture.None;
				if (flag6)
				{
					throw new Exception("Depth of field : DepthTexture was never set, you are missing a call to UpdateDepthOfFieldParameters");
				}
				this._profiling.StartMeasure(this._renderingProfileDepthOfField);
				this.DrawDepthOfField();
				this._profiling.StopMeasure(this._renderingProfileDepthOfField);
			}
			else
			{
				this._profiling.SkipMeasure(this._renderingProfileDepthOfField);
			}
			RenderTargetStore rtstore = this._graphics.RTStore;
			this._profiling.StartMeasure(this._renderingProfileCombineAndFxaa);
			bool flag7 = this._postEffectSettings.UseBloom || this._postEffectSettings.UseDepthOfField;
			bool flag8 = flag7;
			if (flag8)
			{
				this._gl.Viewport(0, 0, this._width, this._height);
			}
			bool flag9 = this.NeedsScreenBlur || this._postEffectSettings.UseTemporalAA;
			if (flag9)
			{
				rtstore.FinalSceneColor.Bind(false, true);
			}
			else
			{
				this.BindOutputFramebuffer();
			}
			this._graphics.GL.UseProgram(this._postEffectProgram);
			bool useVolumetricSunshaft = this._postEffectProgram.UseVolumetricSunshaft;
			if (useVolumetricSunshaft)
			{
				this._gl.ActiveTexture(GL.TEXTURE9);
				this._gl.BindTexture(GL.TEXTURE_2D, rtstore.VolumetricSunshaft.GetTexture(RenderTarget.Target.Color0));
			}
			bool useBloom2 = this._postEffectSettings.UseBloom;
			if (useBloom2)
			{
				this._gl.ActiveTexture(GL.TEXTURE7);
				this._gl.BindTexture(GL.TEXTURE_2D, rtstore.BlurXResBy2.GetTexture(RenderTarget.Target.Color0));
				this._postEffectProgram.ApplyBloom.SetValue(this._postEffectDrawParameters.BloomParams.ApplyBloom);
			}
			bool useDepthOfField2 = this._postEffectProgram.UseDepthOfField;
			if (useDepthOfField2)
			{
				bool flag10 = this._postEffectProgram.DepthOfFieldVersion == 0;
				if (flag10)
				{
					this._postEffectProgram.NearBlurMax.SetValue(this._postEffectDrawParameters.DepthOfFieldParams.NearBlurMax);
					this._postEffectProgram.FarBlurMax.SetValue(this._postEffectDrawParameters.DepthOfFieldParams.FarBlurMax);
					this._postEffectProgram.NearBlurry.SetValue(this._postEffectDrawParameters.DepthOfFieldParams.NearBlurry);
					this._postEffectProgram.NearSharp.SetValue(this._postEffectDrawParameters.DepthOfFieldParams.NearSharp);
					this._postEffectProgram.FarSharp.SetValue(this._postEffectDrawParameters.DepthOfFieldParams.FarSharp);
					this._postEffectProgram.FarBlurry.SetValue(this._postEffectDrawParameters.DepthOfFieldParams.FarBlurry);
					this._postEffectProgram.ProjectionMatrix.SetValue(ref this._postEffectDrawParameters.DepthOfFieldParams.ProjectionMatrix);
					this._gl.ActiveTexture(GL.TEXTURE1);
					this._gl.BindTexture(GL.TEXTURE_2D, this._depthInput);
				}
				else
				{
					bool flag11 = this._postEffectProgram.DepthOfFieldVersion == 1;
					if (flag11)
					{
						this._postEffectProgram.NearBlurry.SetValue(this._postEffectDrawParameters.DepthOfFieldParams.NearBlurry);
						this._postEffectProgram.NearSharp.SetValue(this._postEffectDrawParameters.DepthOfFieldParams.NearSharp);
						this._postEffectProgram.FarSharp.SetValue(this._postEffectDrawParameters.DepthOfFieldParams.FarSharp);
						this._postEffectProgram.FarBlurry.SetValue(this._postEffectDrawParameters.DepthOfFieldParams.FarBlurry);
						this._postEffectProgram.ProjectionMatrix.SetValue(ref this._postEffectDrawParameters.DepthOfFieldParams.ProjectionMatrix);
						this._gl.ActiveTexture(GL.TEXTURE2);
						this._gl.BindTexture(GL.TEXTURE_2D, rtstore.DOFBlurY.GetTexture(RenderTarget.Target.Color0));
						this._gl.ActiveTexture(GL.TEXTURE1);
						this._gl.BindTexture(GL.TEXTURE_2D, this._depthInput);
					}
					else
					{
						bool flag12 = this._postEffectProgram.DepthOfFieldVersion == 2;
						if (flag12)
						{
							this._postEffectProgram.NearBlurry.SetValue(this._postEffectDrawParameters.DepthOfFieldParams.NearBlurry);
							this._postEffectProgram.NearSharp.SetValue(this._postEffectDrawParameters.DepthOfFieldParams.NearSharp);
							this._postEffectProgram.FarSharp.SetValue(this._postEffectDrawParameters.DepthOfFieldParams.FarSharp);
							this._postEffectProgram.FarBlurry.SetValue(this._postEffectDrawParameters.DepthOfFieldParams.FarBlurry);
							this._postEffectProgram.ProjectionMatrix.SetValue(ref this._postEffectDrawParameters.DepthOfFieldParams.ProjectionMatrix);
							this._gl.ActiveTexture(GL.TEXTURE3);
							this._gl.BindTexture(GL.TEXTURE_2D, rtstore.DOFBlurYBis.GetTexture(RenderTarget.Target.Color1));
							this._gl.ActiveTexture(GL.TEXTURE2);
							this._gl.BindTexture(GL.TEXTURE_2D, rtstore.DOFBlurYBis.GetTexture(RenderTarget.Target.Color0));
							this._gl.ActiveTexture(GL.TEXTURE1);
							this._gl.BindTexture(GL.TEXTURE_2D, this._depthInput);
						}
						else
						{
							bool flag13 = this._postEffectProgram.DepthOfFieldVersion == 3;
							if (flag13)
							{
								this._gl.ActiveTexture(GL.TEXTURE6);
								this._gl.BindTexture(GL.TEXTURE_2D, this._input);
								this._gl.ActiveTexture(GL.TEXTURE5);
								this._gl.BindTexture(GL.TEXTURE_2D, rtstore.NearFarField.GetTexture(RenderTarget.Target.Color1));
								this._gl.ActiveTexture(GL.TEXTURE4);
								this._gl.BindTexture(GL.TEXTURE_2D, rtstore.NearFarField.GetTexture(RenderTarget.Target.Color0));
								this._gl.ActiveTexture(GL.TEXTURE3);
								this._gl.BindTexture(GL.TEXTURE_2D, rtstore.NearCoCBlurY.GetTexture(RenderTarget.Target.Color0));
								this._gl.ActiveTexture(GL.TEXTURE2);
								this._gl.BindTexture(GL.TEXTURE_2D, rtstore.Downsample.GetTexture(RenderTarget.Target.Color2));
								this._gl.ActiveTexture(GL.TEXTURE1);
								this._gl.BindTexture(GL.TEXTURE_2D, rtstore.CoC.GetTexture(RenderTarget.Target.Color0));
							}
						}
					}
				}
			}
			this._gl.ActiveTexture(GL.TEXTURE8);
			this._gl.BindTexture(GL.TEXTURE_2D, this._inputFX);
			this._gl.ActiveTexture(GL.TEXTURE0);
			this._gl.BindSampler(0U, this._linearSampler);
			this._gl.BindTexture(GL.TEXTURE_2D, this._input);
			float x = 1f / (float)this._width;
			float y = 1f / (float)this._height;
			this._postEffectProgram.PixelSize.SetValue(x, y);
			this._postEffectProgram.Time.SetValue(this._postEffectDrawParameters.Time);
			this._postEffectProgram.DistortionAmplitude.SetValue(this._postEffectDrawParameters.DistortionAmplitude);
			this._postEffectProgram.DistortionFrequency.SetValue(this._postEffectDrawParameters.DistortionFrequency);
			this._postEffectProgram.ColorBrightnessContrast.SetValue(this._postEffectDrawParameters.ColorBrightness, this._postEffectDrawParameters.ColorContrast);
			this._postEffectProgram.ColorSaturation.SetValue(this._postEffectDrawParameters.ColorSaturation);
			this._postEffectProgram.ColorFilter.SetValue(this._postEffectDrawParameters.ColorFilter);
			this._postEffectProgram.VolumetricSunshaftStrength.SetValue(this._postEffectDrawParameters.VolumetricSunshaftStrength);
			bool debugTiles = this._postEffectProgram.DebugTiles;
			if (debugTiles)
			{
				this._postEffectProgram.DebugTileResolution.SetValue(this._postEffectDrawParameters.DebugTileResolution);
			}
			this._graphics.ScreenTriangleRenderer.Draw();
			this._gl.BindSampler(0U, GLSampler.None);
			this._profiling.StopMeasure(this._renderingProfileCombineAndFxaa);
			bool needsJittering = this.NeedsJittering;
			if (needsJittering)
			{
				this._profiling.StartMeasure(this._renderingProfileTaa);
				this.DrawTemporalAA();
				this._profiling.StopMeasure(this._renderingProfileTaa);
			}
			else
			{
				this._profiling.SkipMeasure(this._renderingProfileTaa);
			}
			bool needsScreenBlur = this.NeedsScreenBlur;
			if (needsScreenBlur)
			{
				this._profiling.StartMeasure(this._renderingProfileBlur);
				this.DrawBlurredScreen();
				this._profiling.StopMeasure(this._renderingProfileBlur);
			}
			else
			{
				this._profiling.SkipMeasure(this._renderingProfileBlur);
			}
		}

		// Token: 0x06005284 RID: 21124 RVA: 0x0016C6B5 File Offset: 0x0016A8B5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateDistortion(float time, float distortionAmplitude, float distortionFrequency)
		{
			this._postEffectDrawParameters.Time = time;
			this._postEffectDrawParameters.DistortionAmplitude = distortionAmplitude;
			this._postEffectDrawParameters.DistortionFrequency = distortionFrequency;
		}

		// Token: 0x06005285 RID: 21125 RVA: 0x0016C6DC File Offset: 0x0016A8DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateColorFilters(Vector3 colorFilter, float colorSaturation)
		{
			this._postEffectDrawParameters.ColorFilter = colorFilter;
			this._postEffectDrawParameters.ColorSaturation = colorSaturation;
		}

		// Token: 0x06005286 RID: 21126 RVA: 0x0016C6F7 File Offset: 0x0016A8F7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateDebugTileResolution(Vector2 resolution)
		{
			this._postEffectDrawParameters.DebugTileResolution = resolution;
		}

		// Token: 0x06005287 RID: 21127 RVA: 0x0016C706 File Offset: 0x0016A906
		public void UseBlur(bool enable)
		{
			this._postEffectSettings.RequestScreenBlur = enable;
		}

		// Token: 0x06005288 RID: 21128 RVA: 0x0016C715 File Offset: 0x0016A915
		public void SetBlurStrength(int strength)
		{
			Debug.Assert(strength >= 0 && strength <= 3, string.Format("Invalid blur strength {0}. Valid values are [0-3].", strength));
			this._postEffectSettings.BlurredScreenSettings.ScreenBlurStrength = strength;
		}

		// Token: 0x170012CE RID: 4814
		// (get) Token: 0x06005289 RID: 21129 RVA: 0x0016C74D File Offset: 0x0016A94D
		private bool NeedsScreenBlur
		{
			get
			{
				return this._postEffectSettings.RequestScreenBlur && this._postEffectSettings.BlurredScreenSettings.ScreenBlurStrength > 0;
			}
		}

		// Token: 0x0600528A RID: 21130 RVA: 0x0016C774 File Offset: 0x0016A974
		private void DrawBlurredScreen()
		{
			RenderTargetStore rtstore = this._graphics.RTStore;
			RenderTarget sceneColorHalfRes = rtstore.SceneColorHalfRes;
			RenderTarget renderTarget = (this._postEffectSettings.BlurredScreenSettings.ScreenBlurStrength == 3) ? rtstore.BlurXResBy8 : rtstore.BlurXResBy4;
			RenderTarget renderTarget2 = (this._postEffectSettings.BlurredScreenSettings.ScreenBlurStrength == 3) ? rtstore.BlurYResBy8 : rtstore.BlurYResBy4;
			rtstore.FinalSceneColor.CopyColorTo(rtstore.SceneColorHalfRes, GL.COLOR_ATTACHMENT0, GL.COLOR_ATTACHMENT0, GL.LINEAR, false, false);
			GraphicsDevice graphics = this._graphics;
			BlurProgram blurProgram = this._gpuProgramStore.BlurProgram;
			renderTarget.Bind(false, true);
			this._gl.UseProgram(blurProgram);
			this._gl.BindTexture(GL.TEXTURE_2D, sceneColorHalfRes.GetTexture(RenderTarget.Target.Color0));
			blurProgram.PixelSize.SetValue(1f / (float)sceneColorHalfRes.Width, 1f / (float)sceneColorHalfRes.Height);
			float screenBlurScale = this._postEffectSettings.BlurredScreenSettings.ScreenBlurScale;
			blurProgram.BlurScale.SetValue(screenBlurScale);
			blurProgram.HorizontalPass.SetValue(1f);
			graphics.ScreenTriangleRenderer.Draw();
			renderTarget.Unbind();
			renderTarget2.Bind(false, false);
			this._gl.BindTexture(GL.TEXTURE_2D, renderTarget.GetTexture(RenderTarget.Target.Color0));
			blurProgram.PixelSize.SetValue(1f / (float)renderTarget.Width, 1f / (float)renderTarget.Height);
			blurProgram.HorizontalPass.SetValue(0f);
			graphics.ScreenTriangleRenderer.Draw();
			renderTarget2.Unbind();
			bool flag = this._postEffectSettings.BlurredScreenSettings.ScreenBlurStrength > 1;
			if (flag)
			{
				renderTarget.Bind(false, false);
				this._gl.UseProgram(blurProgram);
				this._gl.BindTexture(GL.TEXTURE_2D, renderTarget2.GetTexture(RenderTarget.Target.Color0));
				blurProgram.HorizontalPass.SetValue(1f);
				graphics.ScreenTriangleRenderer.Draw();
				renderTarget.Unbind();
				renderTarget2.Bind(false, false);
				this._gl.BindTexture(GL.TEXTURE_2D, renderTarget.GetTexture(RenderTarget.Target.Color0));
				blurProgram.HorizontalPass.SetValue(0f);
				graphics.ScreenTriangleRenderer.Draw();
				renderTarget2.Unbind();
			}
			ScreenBlitProgram screenBlitProgram = this._gpuProgramStore.ScreenBlitProgram;
			this.BindOutputFramebuffer();
			this._gl.Viewport(0, 0, this._width, this._height);
			this._gl.BindTexture(GL.TEXTURE_2D, renderTarget2.GetTexture(RenderTarget.Target.Color0));
			this._gl.UseProgram(screenBlitProgram);
			screenBlitProgram.MipLevel.SetValue(0);
			graphics.ScreenTriangleRenderer.Draw();
		}

		// Token: 0x0600528B RID: 21131 RVA: 0x0016CA45 File Offset: 0x0016AC45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateTemporalAA(bool hasCameraMoved)
		{
			this._hasCameraMoved = hasCameraMoved;
		}

		// Token: 0x0600528C RID: 21132 RVA: 0x0016CA50 File Offset: 0x0016AC50
		public void UseTemporalAA(bool enable)
		{
			bool flag = enable != this._postEffectSettings.UseTemporalAA;
			if (flag)
			{
				this._postEffectSettings.UseTemporalAA = enable;
				PostEffectProgram postEffectProgram = this._postEffectProgram;
				postEffectProgram.SharpenStrength = (enable ? 0.2f : 0.1f);
				postEffectProgram.Reset(true);
			}
		}

		// Token: 0x0600528D RID: 21133 RVA: 0x0016CAA8 File Offset: 0x0016ACA8
		public void UseFXAA(bool enable)
		{
			bool flag = enable != this._postEffectSettings.UseFXAAA;
			if (flag)
			{
				this._postEffectSettings.UseFXAAA = enable;
				PostEffectProgram postEffectProgram = this._postEffectProgram;
				postEffectProgram.UseFXAA = enable;
				postEffectProgram.Reset(true);
			}
		}

		// Token: 0x0600528E RID: 21134 RVA: 0x0016CAF0 File Offset: 0x0016ACF0
		public void UseFXAASharpened(bool enable, float strength = -1f)
		{
			bool flag = enable != this._postEffectSettings.UseSharpenPostEffect || strength != -1f;
			if (flag)
			{
				this._postEffectSettings.UseSharpenPostEffect = enable;
				PostEffectProgram postEffectProgram = this._postEffectProgram;
				postEffectProgram.UseSharpenEffect = enable;
				bool flag2 = strength != -1f;
				if (flag2)
				{
					postEffectProgram.SharpenStrength = strength;
				}
				postEffectProgram.Reset(true);
			}
		}

		// Token: 0x0600528F RID: 21135 RVA: 0x0016CB58 File Offset: 0x0016AD58
		private void DrawTemporalAA()
		{
			RenderTargetStore rtstore = this._graphics.RTStore;
			rtstore.FinalSceneColor.Unbind();
			this.BindOutputFramebuffer();
			this._gl.Viewport(0, 0, this._width, this._height);
			this._gl.ActiveTexture(GL.TEXTURE1);
			this._gl.BindTexture(GL.TEXTURE_2D, rtstore.PreviousFinalSceneColor.GetTexture(RenderTarget.Target.Color0));
			this._gl.ActiveTexture(GL.TEXTURE0);
			this._gl.BindTexture(GL.TEXTURE_2D, rtstore.FinalSceneColor.GetTexture(RenderTarget.Target.Color0));
			TemporalAAProgram temporalAAProgram = this._gpuProgramStore.TemporalAAProgram;
			this._gl.UseProgram(temporalAAProgram);
			temporalAAProgram.PixelSize.SetValue(rtstore.FinalSceneColor.InvWidth, rtstore.FinalSceneColor.InvHeight);
			temporalAAProgram.NeighborHoodCheck.SetValue(this._hasCameraMoved ? 1 : 0);
			this._graphics.ScreenTriangleRenderer.Draw();
		}

		// Token: 0x06005290 RID: 21136 RVA: 0x0016CC5F File Offset: 0x0016AE5F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetPostFXBrightness(float value)
		{
			this._postEffectDrawParameters.ColorBrightness = value;
		}

		// Token: 0x06005291 RID: 21137 RVA: 0x0016CC6E File Offset: 0x0016AE6E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetPostFXContrast(float value)
		{
			this._postEffectDrawParameters.ColorContrast = value;
		}

		// Token: 0x06005292 RID: 21138 RVA: 0x0016CC7D File Offset: 0x0016AE7D
		public void SetVolumetricSunshaftStrength(float value)
		{
			this._postEffectDrawParameters.VolumetricSunshaftStrength = value;
		}

		// Token: 0x06005293 RID: 21139 RVA: 0x0016CC8C File Offset: 0x0016AE8C
		public void InitDepthOfField(GLTexture depthTexture, bool useDepthOfField = false, int version = 2, float nearBlurry = 1f, float nearSharp = 2f, float farSharp = 30f, float farBlurry = 70f, float nearBlurMax = 0.5f, float farBlurMax = 0.3f)
		{
			this._depthInput = depthTexture;
			this._postEffectSettings.UseDepthOfField = useDepthOfField;
			this._postEffectProgram.UseDepthOfField = useDepthOfField;
			this._postEffectSettings.DoFSettings.Version = version;
			this._postEffectProgram.DepthOfFieldVersion = version;
			this._postEffectProgram.Reset(true);
			this.SetupDepthOfField(nearBlurry, nearSharp, farSharp, farBlurry, nearBlurMax, farBlurMax);
			this._postEffectDrawParameters.DepthOfFieldParams.ProjectionMatrix = Matrix.Identity;
		}

		// Token: 0x06005294 RID: 21140 RVA: 0x0016CD0C File Offset: 0x0016AF0C
		public void SetupDepthOfField(float nearBlurry = 1f, float nearSharp = 2f, float farSharp = 30f, float farBlurry = 70f, float nearBlurMax = 0.5f, float farBlurMax = 0.3f)
		{
			this._postEffectDrawParameters.DepthOfFieldParams.NearBlurry = nearBlurry;
			this._postEffectDrawParameters.DepthOfFieldParams.NearSharp = nearSharp;
			this._postEffectDrawParameters.DepthOfFieldParams.FarSharp = farSharp;
			this._postEffectDrawParameters.DepthOfFieldParams.FarBlurry = farBlurry;
			this._postEffectDrawParameters.DepthOfFieldParams.NearBlurMax = nearBlurMax;
			this._postEffectDrawParameters.DepthOfFieldParams.FarBlurMax = farBlurMax;
		}

		// Token: 0x06005295 RID: 21141 RVA: 0x0016CD84 File Offset: 0x0016AF84
		public void SetDepthOfFieldVersion(int version)
		{
			bool flag = this._postEffectSettings.DoFSettings.Version != version;
			if (flag)
			{
				this._postEffectSettings.DoFSettings.Version = version;
				PostEffectProgram postEffectProgram = this._postEffectProgram;
				postEffectProgram.DepthOfFieldVersion = version;
				postEffectProgram.Reset(true);
			}
		}

		// Token: 0x06005296 RID: 21142 RVA: 0x0016CDD8 File Offset: 0x0016AFD8
		public void UseDepthOfField(bool enable)
		{
			bool flag = this._postEffectSettings.UseDepthOfField != enable;
			if (flag)
			{
				this._postEffectSettings.UseDepthOfField = enable;
				PostEffectProgram postEffectProgram = this._postEffectProgram;
				postEffectProgram.UseDepthOfField = enable;
				postEffectProgram.Reset(true);
			}
		}

		// Token: 0x06005297 RID: 21143 RVA: 0x0016CE1F File Offset: 0x0016B01F
		public void UpdateDepthOfField(Matrix projectionMatrix)
		{
			this._postEffectDrawParameters.DepthOfFieldParams.ProjectionMatrix = projectionMatrix;
		}

		// Token: 0x06005298 RID: 21144 RVA: 0x0016CE34 File Offset: 0x0016B034
		private void DrawDepthOfField()
		{
			int version = this._postEffectSettings.DoFSettings.Version;
			PostEffectRenderer.DepthOfFieldDrawParams depthOfFieldParams = this._postEffectDrawParameters.DepthOfFieldParams;
			RenderTargetStore rtstore = this._graphics.RTStore;
			bool flag = version == 0;
			if (!flag)
			{
				bool flag2 = version == 1;
				if (flag2)
				{
					rtstore.SceneColor.CopyColorTo(rtstore.SceneColorHalfRes, GL.COLOR_ATTACHMENT0, GL.COLOR_ATTACHMENT0, GL.LINEAR, true, false);
					BlurProgram blurProgram = this._gpuProgramStore.BlurProgram;
					rtstore.DOFBlurX.Bind(true, true);
					this._gl.UseProgram(blurProgram);
					this._gl.ActiveTexture(GL.TEXTURE0);
					this._gl.BindTexture(GL.TEXTURE_2D, rtstore.SceneColorHalfRes.GetTexture(RenderTarget.Target.Color0));
					blurProgram.PixelSize.SetValue(1f / (float)rtstore.DOFBlurX.Width, 1f / (float)rtstore.DOFBlurX.Height);
					float value = depthOfFieldParams.NearBlurMax + depthOfFieldParams.FarBlurMax;
					blurProgram.BlurScale.SetValue(value);
					blurProgram.HorizontalPass.SetValue(1f);
					this._graphics.ScreenTriangleRenderer.Draw();
					rtstore.DOFBlurX.Unbind();
					rtstore.DOFBlurY.Bind(true, false);
					this._gl.ActiveTexture(GL.TEXTURE0);
					this._gl.BindTexture(GL.TEXTURE_2D, rtstore.DOFBlurX.GetTexture(RenderTarget.Target.Color0));
					blurProgram.HorizontalPass.SetValue(0f);
					this._graphics.ScreenTriangleRenderer.DrawRaw();
					rtstore.DOFBlurY.Unbind();
				}
				else
				{
					bool flag3 = version == 2;
					if (flag3)
					{
						rtstore.SceneColor.CopyColorTo(rtstore.SceneColorHalfRes, GL.COLOR_ATTACHMENT0, GL.COLOR_ATTACHMENT0, GL.LINEAR, true, false);
						DoFBlurProgram doFBlurProgram = this._gpuProgramStore.DoFBlurProgram;
						this._gl.UseProgram(doFBlurProgram);
						rtstore.DOFBlurXBis.Bind(true, true);
						this._gl.ActiveTexture(GL.TEXTURE1);
						this._gl.BindTexture(GL.TEXTURE_2D, rtstore.SceneColorHalfRes.GetTexture(RenderTarget.Target.Color0));
						this._gl.ActiveTexture(GL.TEXTURE0);
						this._gl.BindTexture(GL.TEXTURE_2D, rtstore.SceneColorHalfRes.GetTexture(RenderTarget.Target.Color0));
						doFBlurProgram.PixelSize.SetValue(1f / (float)rtstore.DOFBlurXBis.Width, 1f / (float)rtstore.DOFBlurXBis.Height);
						float value2 = depthOfFieldParams.NearBlurMax * 2f;
						float value3 = depthOfFieldParams.FarBlurMax * 2f;
						doFBlurProgram.NearBlurScale.SetValue(value2);
						doFBlurProgram.FarBlurScale.SetValue(value3);
						doFBlurProgram.HorizontalPass.SetValue(1f);
						this._graphics.ScreenTriangleRenderer.Draw();
						rtstore.DOFBlurXBis.Unbind();
						rtstore.DOFBlurYBis.Bind(true, false);
						this._gl.ActiveTexture(GL.TEXTURE1);
						this._gl.BindTexture(GL.TEXTURE_2D, rtstore.DOFBlurXBis.GetTexture(RenderTarget.Target.Color1));
						this._gl.ActiveTexture(GL.TEXTURE0);
						this._gl.BindTexture(GL.TEXTURE_2D, rtstore.DOFBlurXBis.GetTexture(RenderTarget.Target.Color0));
						doFBlurProgram.HorizontalPass.SetValue(0f);
						this._graphics.ScreenTriangleRenderer.DrawRaw();
						rtstore.DOFBlurYBis.Unbind();
					}
					else
					{
						bool flag4 = version == 3;
						if (flag4)
						{
							Vector2 value4 = new Vector2(1f / (float)rtstore.NearCoCBlurX.Width, 1f / (float)rtstore.NearCoCBlurX.Height);
							DoFCircleOfConfusionProgram doFCircleOfConfusionProgram = this._gpuProgramStore.DoFCircleOfConfusionProgram;
							DoFDownsampleProgram doFDownsampleProgram = this._gpuProgramStore.DoFDownsampleProgram;
							DoFNearCoCBlurProgram doFNearCoCBlurProgram = this._gpuProgramStore.DoFNearCoCBlurProgram;
							MaxProgram doFNearCoCMaxProgram = this._gpuProgramStore.DoFNearCoCMaxProgram;
							DepthOfFieldAdvancedProgram depthOfFieldAdvancedProgram = this._gpuProgramStore.DepthOfFieldAdvancedProgram;
							DoFFillProgram doFFillProgram = this._gpuProgramStore.DoFFillProgram;
							rtstore.CoC.Bind(true, true);
							this._gl.UseProgram(doFCircleOfConfusionProgram);
							this._gl.ActiveTexture(GL.TEXTURE0);
							this._gl.BindTexture(GL.TEXTURE_2D, this._depthInput);
							doFCircleOfConfusionProgram.NearBlurry.SetValue(depthOfFieldParams.NearBlurry);
							doFCircleOfConfusionProgram.NearSharp.SetValue(depthOfFieldParams.NearSharp);
							doFCircleOfConfusionProgram.FarSharp.SetValue(depthOfFieldParams.FarSharp);
							doFCircleOfConfusionProgram.FarBlurry.SetValue(depthOfFieldParams.FarBlurry);
							doFCircleOfConfusionProgram.ProjectionMatrix.SetValue(ref depthOfFieldParams.ProjectionMatrix);
							bool useLinearZ = doFCircleOfConfusionProgram.UseLinearZ;
							if (useLinearZ)
							{
								doFCircleOfConfusionProgram.FarClip.SetValue(1024f);
							}
							this._graphics.ScreenTriangleRenderer.Draw();
							rtstore.CoC.Unbind();
							rtstore.Downsample.Bind(true, true);
							this._gl.UseProgram(doFDownsampleProgram);
							this._gl.ActiveTexture(GL.TEXTURE2);
							this._gl.BindTexture(GL.TEXTURE_2D, rtstore.CoC.GetTexture(RenderTarget.Target.Color0));
							this._gl.ActiveTexture(GL.TEXTURE1);
							this._gl.BindTexture(GL.TEXTURE_2D, this._input);
							this._gl.ActiveTexture(GL.TEXTURE0);
							this._gl.BindTexture(GL.TEXTURE_2D, this._input);
							doFDownsampleProgram.PixelSize.SetValue(1f / (float)rtstore.SceneColor.Width, 1f / (float)rtstore.SceneColor.Height);
							this._graphics.ScreenTriangleRenderer.DrawRaw();
							rtstore.Downsample.Unbind();
							rtstore.NearCoCMaxX.Bind(true, false);
							this._gl.UseProgram(doFNearCoCMaxProgram);
							this._gl.ActiveTexture(GL.TEXTURE0);
							this._gl.BindTexture(GL.TEXTURE_2D, rtstore.Downsample.GetTexture(RenderTarget.Target.Color2));
							doFNearCoCMaxProgram.PixelSize.SetValue(value4);
							doFNearCoCMaxProgram.HorizontalPass.SetValue(1f);
							this._graphics.ScreenTriangleRenderer.DrawRaw();
							rtstore.NearCoCMaxX.Unbind();
							rtstore.NearCoCMaxY.Bind(true, false);
							this._gl.ActiveTexture(GL.TEXTURE0);
							this._gl.BindTexture(GL.TEXTURE_2D, rtstore.NearCoCMaxX.GetTexture(RenderTarget.Target.Color0));
							doFNearCoCMaxProgram.HorizontalPass.SetValue(0f);
							this._graphics.ScreenTriangleRenderer.DrawRaw();
							rtstore.NearCoCMaxY.Unbind();
							rtstore.NearCoCBlurX.Bind(true, false);
							this._gl.UseProgram(doFNearCoCBlurProgram);
							this._gl.ActiveTexture(GL.TEXTURE0);
							this._gl.BindTexture(GL.TEXTURE_2D, rtstore.NearCoCMaxY.GetTexture(RenderTarget.Target.Color0));
							doFNearCoCBlurProgram.PixelSize.SetValue(value4);
							doFNearCoCBlurProgram.HorizontalPass.SetValue(1f);
							this._graphics.ScreenTriangleRenderer.DrawRaw();
							rtstore.NearCoCBlurX.Unbind();
							rtstore.NearCoCBlurY.Bind(true, false);
							this._gl.ActiveTexture(GL.TEXTURE0);
							this._gl.BindTexture(GL.TEXTURE_2D, rtstore.NearCoCBlurX.GetTexture(RenderTarget.Target.Color0));
							doFNearCoCBlurProgram.HorizontalPass.SetValue(0f);
							this._graphics.ScreenTriangleRenderer.DrawRaw();
							rtstore.NearCoCBlurY.Unbind();
							rtstore.NearFarField.Bind(true, true);
							this._gl.UseProgram(depthOfFieldAdvancedProgram);
							this._gl.ActiveTexture(GL.TEXTURE5);
							this._gl.BindTexture(GL.TEXTURE_2D, rtstore.NearCoCBlurY.GetTexture(RenderTarget.Target.Color0));
							this._gl.ActiveTexture(GL.TEXTURE4);
							this._gl.BindTexture(GL.TEXTURE_2D, rtstore.Downsample.GetTexture(RenderTarget.Target.Color2));
							this._gl.ActiveTexture(GL.TEXTURE3);
							this._gl.BindTexture(GL.TEXTURE_2D, rtstore.Downsample.GetTexture(RenderTarget.Target.Color1));
							this._gl.ActiveTexture(GL.TEXTURE2);
							this._gl.BindTexture(GL.TEXTURE_2D, rtstore.Downsample.GetTexture(RenderTarget.Target.Color2));
							this._gl.ActiveTexture(GL.TEXTURE1);
							this._gl.BindTexture(GL.TEXTURE_2D, rtstore.Downsample.GetTexture(RenderTarget.Target.Color1));
							this._gl.ActiveTexture(GL.TEXTURE0);
							this._gl.BindTexture(GL.TEXTURE_2D, rtstore.Downsample.GetTexture(RenderTarget.Target.Color0));
							depthOfFieldAdvancedProgram.NearBlurMax.SetValue(depthOfFieldParams.NearBlurMax);
							depthOfFieldAdvancedProgram.FarBlurMax.SetValue(depthOfFieldParams.FarBlurMax);
							depthOfFieldAdvancedProgram.PixelSize.SetValue(value4);
							this._graphics.ScreenTriangleRenderer.DrawRaw();
							rtstore.NearFarField.Unbind();
							rtstore.Fill.Bind(true, true);
							this._gl.UseProgram(doFFillProgram);
							this._gl.ActiveTexture(GL.TEXTURE3);
							this._gl.BindTexture(GL.TEXTURE_2D, rtstore.NearFarField.GetTexture(RenderTarget.Target.Color1));
							this._gl.ActiveTexture(GL.TEXTURE2);
							this._gl.BindTexture(GL.TEXTURE_2D, rtstore.NearFarField.GetTexture(RenderTarget.Target.Color0));
							this._gl.ActiveTexture(GL.TEXTURE1);
							this._gl.BindTexture(GL.TEXTURE_2D, rtstore.NearCoCBlurY.GetTexture(RenderTarget.Target.Color0));
							this._gl.ActiveTexture(GL.TEXTURE0);
							this._gl.BindTexture(GL.TEXTURE_2D, rtstore.Downsample.GetTexture(RenderTarget.Target.Color2));
							doFFillProgram.PixelSize.SetValue(value4);
							this._graphics.ScreenTriangleRenderer.DrawRaw();
							rtstore.Fill.Unbind();
						}
					}
				}
				this._gl.ActiveTexture(GL.TEXTURE0);
			}
		}

		// Token: 0x06005299 RID: 21145 RVA: 0x0016D8A8 File Offset: 0x0016BAA8
		public void InitBloom(GLTexture sunTexture, GLTexture moonTexture, GLTexture glowMask, Action drawSun = null, Action drawMoon = null, bool useBloom = false, bool useSun = false, bool useMoon = false, bool useSunshaft = false, bool usePow = false, bool useFullbright = false, int version = 0, float globalIntensity = 0.3f, float power = 8f, float sunIntensity = 0.25f, float sunshaftIntensity = 0.3f, float sunshaftScaleFactor = 4f)
		{
			this._postEffectSettings.UseBloom = useBloom;
			this._postEffectProgram.UseBloom = useBloom;
			this._postEffectSettings.BloomSettings.SunTexture = sunTexture;
			this._postEffectSettings.BloomSettings.GlowMask = glowMask;
			this._postEffectSettings.BloomSettings.DrawSun = drawSun;
			this._postEffectSettings.BloomSettings.MoonTexture = moonTexture;
			this._postEffectSettings.BloomSettings.DrawMoon = drawMoon;
			this.SetBloomVersion(version);
			this._postEffectSettings.BloomSettings.UseSun = useSun;
			this._postEffectSettings.BloomSettings.UseMoon = useMoon;
			this._gpuProgramStore.BloomSelectProgram.SunOrMoon = (useSun || useMoon);
			this._postEffectProgram.UseSunshaft = useSunshaft;
			this._postEffectSettings.BloomSettings.UseSunshaft = useSunshaft;
			this._gpuProgramStore.BloomCompositeProgram.UseSunshaft = useSunshaft;
			this._postEffectSettings.BloomSettings.UseFullbright = useFullbright;
			this._gpuProgramStore.BloomSelectProgram.Fullbright = useFullbright;
			this._postEffectSettings.BloomSettings.UsePow = usePow;
			this._gpuProgramStore.BloomSelectProgram.Pow = usePow;
			bool sunFbPow = this._gpuProgramStore.BloomSelectProgram.SunOrMoon || this._postEffectSettings.BloomSettings.UseFullbright || this._postEffectSettings.BloomSettings.UsePow;
			this._gpuProgramStore.BloomCompositeProgram.SunFbPow = sunFbPow;
			this._postEffectProgram.SunFbPow = sunFbPow;
			this._gpuProgramStore.BloomSelectProgram.Reset(true);
			this._gpuProgramStore.BloomCompositeProgram.Reset(true);
			this._postEffectProgram.Reset(true);
			this.SetBloomGlobalIntensity(globalIntensity);
			this._postEffectDrawParameters.BloomParams.Intensities = new float[]
			{
				1f,
				2f,
				3f,
				4f,
				5f
			};
			this.SetBloomPower(power);
			this.SetSunIntensity(sunIntensity);
			this.SetSunshaftIntensity(sunshaftIntensity);
			this.SetSunshaftScaleFactor(sunshaftScaleFactor);
			this.SetBloomOnPowIntensity(0.04f);
			this.SetBloomOnPowPower(5f);
		}

		// Token: 0x0600529A RID: 21146 RVA: 0x0016DAD0 File Offset: 0x0016BCD0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBloom(Matrix sunMVPMatrix, bool isSunVisible, bool allowBloom, Vector3 sunColor, bool isMoonVisible, Vector4 moonColor, float time)
		{
			this._postEffectDrawParameters.BloomParams.SunMVP = sunMVPMatrix;
			this._postEffectDrawParameters.BloomParams.IsSunVisible = isSunVisible;
			this._postEffectDrawParameters.BloomParams.IsMoonVisible = isMoonVisible;
			this._postEffectDrawParameters.BloomParams.isBloomAllowed = allowBloom;
			this._postEffectDrawParameters.BloomParams.SunColor = sunColor;
			this._postEffectDrawParameters.BloomParams.MoonColor = moonColor;
			this._postEffectDrawParameters.BloomParams.Time = time;
			PostEffectRenderer.BloomSettings bloomSettings = this._postEffectSettings.BloomSettings;
			this._postEffectDrawParameters.BloomParams.ApplyBloom = ((this._postEffectSettings.UseBloom && allowBloom) ? 1 : 0);
		}

		// Token: 0x0600529B RID: 21147 RVA: 0x0016DB8A File Offset: 0x0016BD8A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetSunshaftScaleFactor(float factor)
		{
			this._postEffectDrawParameters.BloomParams.SunshaftScale = factor;
		}

		// Token: 0x0600529C RID: 21148 RVA: 0x0016DB9E File Offset: 0x0016BD9E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetSunshaftIntensity(float intensity)
		{
			this._postEffectDrawParameters.BloomParams.SunshaftIntensity = intensity;
		}

		// Token: 0x0600529D RID: 21149 RVA: 0x0016DBB4 File Offset: 0x0016BDB4
		public void SetBloomVersion(int version)
		{
			bool flag = version == 0;
			if (flag)
			{
				this._postEffectSettings.BloomSettings.Version = 0;
				this.SetDownsampleMethod(2);
				this.SetUpsampleMethod(1);
			}
			else
			{
				this._postEffectSettings.BloomSettings.Version = 1;
				this.SetDownsampleMethod(2);
				this.SetUpsampleMethod(2);
			}
		}

		// Token: 0x0600529E RID: 21150 RVA: 0x0016DC14 File Offset: 0x0016BE14
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetDownsampleMethod(int method)
		{
			this._postEffectSettings.BloomSettings.DownsampleMethod = MathHelper.Clamp(method, 0, 3);
			this._gpuProgramStore.BloomDownsampleBlurProgram.DownsampleMethod = this._postEffectSettings.BloomSettings.DownsampleMethod;
			this._gpuProgramStore.BloomDownsampleBlurProgram.Reset(true);
		}

		// Token: 0x0600529F RID: 21151 RVA: 0x0016DC6C File Offset: 0x0016BE6C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetUpsampleMethod(int method)
		{
			this._postEffectSettings.BloomSettings.UpsampleMethod = MathHelper.Clamp(method, 0, 2);
			this._gpuProgramStore.BloomUpsampleBlurProgram.UpsampleMethod = this._postEffectSettings.BloomSettings.UpsampleMethod;
			this._gpuProgramStore.BloomUpsampleBlurProgram.Reset(true);
		}

		// Token: 0x060052A0 RID: 21152 RVA: 0x0016DCC4 File Offset: 0x0016BEC4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetBloomGlobalIntensity(float intensity)
		{
			this._postEffectDrawParameters.BloomParams.GlobalIntensity = intensity;
		}

		// Token: 0x060052A1 RID: 21153 RVA: 0x0016DCD8 File Offset: 0x0016BED8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetBloomIntensities(float i0, float i1, float i2, float i3, float i4)
		{
			this._postEffectDrawParameters.BloomParams.Intensities[0] = i0;
			this._postEffectDrawParameters.BloomParams.Intensities[1] = i1;
			this._postEffectDrawParameters.BloomParams.Intensities[2] = i2;
			this._postEffectDrawParameters.BloomParams.Intensities[3] = i3;
			this._postEffectDrawParameters.BloomParams.Intensities[4] = i4;
		}

		// Token: 0x060052A2 RID: 21154 RVA: 0x0016DD47 File Offset: 0x0016BF47
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetBloomPower(float power)
		{
			this._postEffectDrawParameters.BloomParams.Power = power;
		}

		// Token: 0x060052A3 RID: 21155 RVA: 0x0016DD5B File Offset: 0x0016BF5B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetSunIntensity(float intensity)
		{
			this._postEffectDrawParameters.BloomParams.SunMoonIntensity = intensity;
		}

		// Token: 0x060052A4 RID: 21156 RVA: 0x0016DD6F File Offset: 0x0016BF6F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetBloomOnPowIntensity(float intensity)
		{
			this._postEffectDrawParameters.BloomParams.PowIntensity = intensity;
		}

		// Token: 0x060052A5 RID: 21157 RVA: 0x0016DD83 File Offset: 0x0016BF83
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetBloomOnPowPower(float power)
		{
			this._postEffectDrawParameters.BloomParams.PowPower = power;
		}

		// Token: 0x060052A6 RID: 21158 RVA: 0x0016DD98 File Offset: 0x0016BF98
		public void UseBloom(bool enable)
		{
			bool flag = enable != this._postEffectSettings.UseBloom;
			if (flag)
			{
				this._postEffectSettings.UseBloom = enable;
				PostEffectProgram postEffectProgram = this._postEffectProgram;
				postEffectProgram.UseBloom = enable;
				postEffectProgram.Reset(true);
			}
		}

		// Token: 0x060052A7 RID: 21159 RVA: 0x0016DDE0 File Offset: 0x0016BFE0
		public void UseBloomOnSun(bool enable)
		{
			bool flag = enable != this._postEffectSettings.BloomSettings.UseSun;
			if (flag)
			{
				bool useMoon = this._postEffectSettings.BloomSettings.UseMoon;
				this._postEffectSettings.BloomSettings.UseSun = enable;
				this._gpuProgramStore.BloomSelectProgram.SunOrMoon = (enable || useMoon);
				bool sunFbPow = this._gpuProgramStore.BloomSelectProgram.SunOrMoon || this._postEffectSettings.BloomSettings.UseFullbright || this._postEffectSettings.BloomSettings.UsePow;
				this._gpuProgramStore.BloomCompositeProgram.SunFbPow = sunFbPow;
				this._postEffectProgram.SunFbPow = sunFbPow;
				this._gpuProgramStore.BloomSelectProgram.Reset(true);
				this._gpuProgramStore.BloomCompositeProgram.Reset(true);
				this._postEffectProgram.Reset(true);
			}
		}

		// Token: 0x060052A8 RID: 21160 RVA: 0x0016DEC8 File Offset: 0x0016C0C8
		public void UseBloomOnMoon(bool enable)
		{
			bool flag = enable != this._postEffectSettings.BloomSettings.UseMoon;
			if (flag)
			{
				bool useSun = this._postEffectSettings.BloomSettings.UseSun;
				this._postEffectSettings.BloomSettings.UseMoon = enable;
				this._gpuProgramStore.BloomSelectProgram.SunOrMoon = (enable || useSun);
				bool sunFbPow = this._gpuProgramStore.BloomSelectProgram.SunOrMoon || this._postEffectSettings.BloomSettings.UseFullbright || this._postEffectSettings.BloomSettings.UsePow;
				this._gpuProgramStore.BloomCompositeProgram.SunFbPow = sunFbPow;
				this._postEffectProgram.SunFbPow = sunFbPow;
				this._gpuProgramStore.BloomSelectProgram.Reset(true);
				this._gpuProgramStore.BloomCompositeProgram.Reset(true);
				this._postEffectProgram.Reset(true);
			}
		}

		// Token: 0x060052A9 RID: 21161 RVA: 0x0016DFB0 File Offset: 0x0016C1B0
		public void UseBloomOnFullbright(bool enable)
		{
			bool flag = enable != this._postEffectSettings.BloomSettings.UseFullbright;
			if (flag)
			{
				this._postEffectSettings.BloomSettings.UseFullbright = enable;
				this._gpuProgramStore.BloomSelectProgram.Fullbright = enable;
				bool sunFbPow = this._gpuProgramStore.BloomSelectProgram.SunOrMoon || this._postEffectSettings.BloomSettings.UseFullbright || this._postEffectSettings.BloomSettings.UsePow;
				this._gpuProgramStore.BloomCompositeProgram.SunFbPow = sunFbPow;
				this._postEffectProgram.SunFbPow = sunFbPow;
				this._gpuProgramStore.BloomSelectProgram.Reset(true);
				this._gpuProgramStore.BloomCompositeProgram.Reset(true);
				this._postEffectProgram.Reset(true);
			}
		}

		// Token: 0x060052AA RID: 21162 RVA: 0x0016E088 File Offset: 0x0016C288
		public void UseBloomOnFullscreen(bool enable)
		{
			bool flag = enable != this._postEffectSettings.BloomSettings.UsePow;
			if (flag)
			{
				this._postEffectSettings.BloomSettings.UsePow = enable;
				this._gpuProgramStore.BloomSelectProgram.Pow = enable;
				bool sunFbPow = this._gpuProgramStore.BloomSelectProgram.SunOrMoon || this._postEffectSettings.BloomSettings.UseFullbright || this._postEffectSettings.BloomSettings.UsePow;
				this._gpuProgramStore.BloomCompositeProgram.SunFbPow = sunFbPow;
				this._postEffectProgram.SunFbPow = sunFbPow;
				this._gpuProgramStore.BloomSelectProgram.Reset(true);
				this._gpuProgramStore.BloomCompositeProgram.Reset(true);
				this._postEffectProgram.Reset(true);
			}
		}

		// Token: 0x060052AB RID: 21163 RVA: 0x0016E160 File Offset: 0x0016C360
		public void UseBloomSunShaft(bool enable)
		{
			bool flag = enable != this._postEffectSettings.BloomSettings.UseSunshaft;
			if (flag)
			{
				this._postEffectSettings.BloomSettings.UseSunshaft = enable;
				this._gpuProgramStore.BloomCompositeProgram.UseSunshaft = enable;
				this._postEffectProgram.UseSunshaft = enable;
				this._gpuProgramStore.BloomCompositeProgram.Reset(true);
				this._postEffectProgram.Reset(true);
			}
		}

		// Token: 0x060052AC RID: 21164 RVA: 0x0016E1D7 File Offset: 0x0016C3D7
		public void UseDitheringOnBloom(bool enable)
		{
			this._gpuProgramStore.BloomSelectProgram.UseDithering = enable;
			this._gpuProgramStore.BloomSelectProgram.Reset(true);
		}

		// Token: 0x060052AD RID: 21165 RVA: 0x0016E200 File Offset: 0x0016C400
		public string PrintBloomState()
		{
			PostEffectRenderer.BloomSettings bloomSettings = this._postEffectSettings.BloomSettings;
			PostEffectRenderer.BloomDrawParams bloomParams = this._postEffectDrawParameters.BloomParams;
			string text = "Bloom state :";
			bool useBloom = this._postEffectSettings.UseBloom;
			if (useBloom)
			{
				float[] intensities = bloomParams.Intensities;
				text += " on";
				text = text + " v" + bloomSettings.Version.ToString();
				bool useSun = bloomSettings.UseSun;
				if (useSun)
				{
					text += " sun";
				}
				bool useMoon = bloomSettings.UseMoon;
				if (useMoon)
				{
					text += " moon";
				}
				bool usePow = bloomSettings.UsePow;
				if (usePow)
				{
					text += " pow";
				}
				bool useFullbright = bloomSettings.UseFullbright;
				if (useFullbright)
				{
					text += " fb";
				}
				bool useSunshaft = bloomSettings.UseSunshaft;
				if (useSunshaft)
				{
					text += " sunshaft";
				}
				text = text + "\n down " + bloomSettings.DownsampleMethod.ToString();
				text = text + " up " + bloomSettings.UpsampleMethod.ToString();
				text += "\n global intensity=";
				text += bloomParams.GlobalIntensity.ToString();
				text += "\n intensities=";
				text = string.Concat(new string[]
				{
					text,
					intensities[0].ToString(),
					" ",
					intensities[1].ToString(),
					" ",
					intensities[2].ToString(),
					" ",
					intensities[3].ToString(),
					" ",
					intensities[4].ToString()
				});
				text += "\n power=";
				text += bloomParams.Power.ToString();
				text += "\n sunshaft_scale=";
				text += bloomParams.SunshaftScale.ToString();
				text += "\n sunshaft_intensity=";
				text += bloomParams.SunshaftIntensity.ToString();
				text += "\n sun_moon_intensity=";
				text += bloomParams.SunMoonIntensity.ToString();
				bool usePow2 = bloomSettings.UsePow;
				if (usePow2)
				{
					text += "\n pow_intensity=";
					text += bloomParams.PowIntensity.ToString();
					text += "\n pow_power=";
					text += bloomParams.PowPower.ToString();
				}
			}
			else
			{
				text += " off";
			}
			return text;
		}

		// Token: 0x060052AE RID: 21166 RVA: 0x0016E4A0 File Offset: 0x0016C6A0
		private void DrawBloom()
		{
			this._gl.AssertActiveTexture(GL.TEXTURE0);
			PostEffectRenderer.BloomSettings bloomSettings = this._postEffectSettings.BloomSettings;
			PostEffectRenderer.BloomDrawParams bloomParams = this._postEffectDrawParameters.BloomParams;
			RenderTargetStore rtstore = this._graphics.RTStore;
			bool useSun = bloomSettings.UseSun;
			bool useMoon = bloomSettings.UseMoon;
			bool flag = useSun || useMoon;
			bool useSunshaft = bloomSettings.UseSunshaft;
			bool usePow = bloomSettings.UsePow;
			bool useFullbright = bloomSettings.UseFullbright;
			bool flag2 = this._postEffectSettings.UseBloom && bloomParams.isBloomAllowed;
			this._postEffectDrawParameters.BloomParams.ApplyBloom = (flag2 ? 1 : 0);
			bool flag3 = flag2;
			if (flag3)
			{
				bool flag4 = useSun || useMoon;
				if (flag4)
				{
					BasicProgram basicProgram = this._gpuProgramStore.BasicProgram;
					this._gl.UseProgram(basicProgram);
					this._gl.Enable(GL.DEPTH_TEST);
					rtstore.SunRT.Bind(true, true);
					bool flag5 = bloomParams.IsSunVisible && useSun;
					if (flag5)
					{
						basicProgram.Opacity.SetValue(1f);
						basicProgram.Color.SetValue(bloomParams.SunColor.X, bloomParams.SunColor.Y, bloomParams.SunColor.Z);
						this._gl.BindTexture(GL.TEXTURE_2D, bloomSettings.SunTexture);
						bloomSettings.DrawSun();
					}
					bool flag6 = bloomParams.IsMoonVisible && useMoon;
					if (flag6)
					{
						basicProgram.Opacity.SetValue(bloomParams.MoonColor.W);
						basicProgram.Color.SetValue(bloomParams.MoonColor.X, bloomParams.MoonColor.Y, bloomParams.MoonColor.Z);
						this._gl.BindTexture(GL.TEXTURE_2D, bloomSettings.MoonTexture);
						bloomSettings.DrawMoon();
					}
					rtstore.SunRT.Unbind();
					this._gl.Disable(GL.DEPTH_TEST);
				}
				BlurProgram blurProgram = this._gpuProgramStore.BlurProgram;
				this._graphics.ScreenTriangleRenderer.BindVertexArray();
				bool flag7 = useSunshaft && bloomParams.IsSunVisible;
				if (flag7)
				{
					this._gl.UseProgram(blurProgram);
					rtstore.SunshaftX.Bind(false, true);
					this._gl.BindTexture(GL.TEXTURE_2D, this._input);
					blurProgram.PixelSize.SetValue(1f / (float)rtstore.SunshaftX.Width, 1f / (float)rtstore.SunshaftX.Height);
					blurProgram.BlurScale.SetValue(1f);
					blurProgram.HorizontalPass.SetValue(1f);
					this._graphics.ScreenTriangleRenderer.DrawRaw();
					rtstore.SunshaftX.Unbind();
					rtstore.SunshaftY.Bind(false, false);
					this._gl.BindTexture(GL.TEXTURE_2D, rtstore.SunshaftX.GetTexture(RenderTarget.Target.Color0));
					blurProgram.HorizontalPass.SetValue(0f);
					this._graphics.ScreenTriangleRenderer.DrawRaw();
					rtstore.SunshaftY.Unbind();
					RadialGlowMaskProgram radialGlowMaskProgram = this._gpuProgramStore.RadialGlowMaskProgram;
					this._gl.UseProgram(radialGlowMaskProgram);
					rtstore.SunshaftX.Bind(false, false);
					this._gl.ActiveTexture(GL.TEXTURE2);
					this._gl.BindTexture(GL.TEXTURE_2D, rtstore.LinearZ.GetTexture(RenderTarget.Target.Color0));
					this._gl.ActiveTexture(GL.TEXTURE1);
					this._gl.BindTexture(GL.TEXTURE_2D, bloomSettings.GlowMask);
					this._gl.ActiveTexture(GL.TEXTURE0);
					this._gl.BindTexture(GL.TEXTURE_2D, rtstore.SunshaftY.GetTexture(RenderTarget.Target.Color0));
					radialGlowMaskProgram.MVPMatrix.SetValue(ref this._graphics.ScreenMatrix);
					radialGlowMaskProgram.SunMVPMatrix.SetValue(ref bloomParams.SunMVP);
					this._graphics.ScreenQuadRenderer.Draw();
					rtstore.SunshaftX.Unbind();
					RadialGlowLuminanceProgram radialGlowLuminanceProgram = this._gpuProgramStore.RadialGlowLuminanceProgram;
					this._gl.UseProgram(radialGlowLuminanceProgram);
					rtstore.SunshaftY.Bind(false, false);
					this._gl.BindTexture(GL.TEXTURE_2D, rtstore.SunshaftX.GetTexture(RenderTarget.Target.Color0));
					radialGlowLuminanceProgram.MVPMatrix.SetValue(ref this._graphics.ScreenMatrix);
					radialGlowLuminanceProgram.SunMVPMatrix.SetValue(ref bloomParams.SunMVP);
					radialGlowLuminanceProgram.ScaleFactor.SetValue(-bloomParams.SunshaftScale);
					this._graphics.ScreenQuadRenderer.Draw();
					rtstore.SunshaftY.Unbind();
				}
				bool flag8 = flag || usePow || useFullbright;
				if (flag8)
				{
					BloomSelectProgram bloomSelectProgram = this._gpuProgramStore.BloomSelectProgram;
					this._gl.UseProgram(bloomSelectProgram);
					rtstore.BlurXResBy2.Bind(false, true);
					bool flag9 = flag;
					if (flag9)
					{
						int value = (bloomParams.IsSunVisible || bloomParams.IsMoonVisible) ? 1 : 0;
						bloomSelectProgram.UseSunOrMoon.SetValue(value);
						bool useDithering = bloomSelectProgram.UseDithering;
						if (useDithering)
						{
							bloomSelectProgram.Time.SetValue(bloomParams.Time);
						}
						bool flag10 = bloomParams.IsSunVisible || bloomParams.IsMoonVisible;
						if (flag10)
						{
							this._gl.ActiveTexture(GL.TEXTURE1);
							this._gl.BindTexture(GL.TEXTURE_2D, rtstore.SunRT.GetTexture(RenderTarget.Target.Color0));
							float num = bloomParams.SunMoonIntensity;
							num *= (bloomParams.IsMoonVisible ? bloomParams.MoonColor.W : 1f);
							bloomSelectProgram.SunMoonIntensity.SetValue(num);
						}
					}
					bool flag11 = usePow || useFullbright;
					if (flag11)
					{
						this._gl.ActiveTexture(GL.TEXTURE0);
						this._gl.BindTexture(GL.TEXTURE_2D, this._input);
						bloomSelectProgram.Power.SetValue(bloomParams.Power);
					}
					bool flag12 = usePow;
					if (flag12)
					{
						bloomSelectProgram.PowerOptions.SetValue(bloomParams.PowIntensity, bloomParams.PowPower);
					}
					this._graphics.ScreenTriangleRenderer.DrawRaw();
					rtstore.BlurXResBy2.Unbind();
					BloomDownsampleBlurProgram bloomDownsampleBlurProgram = this._gpuProgramStore.BloomDownsampleBlurProgram;
					this._gl.UseProgram(bloomDownsampleBlurProgram);
					rtstore.BlurXResBy4.Bind(false, true);
					this._gl.ActiveTexture(GL.TEXTURE0);
					this._gl.BindTexture(GL.TEXTURE_2D, rtstore.BlurXResBy2.GetTexture(RenderTarget.Target.Color0));
					bloomDownsampleBlurProgram.PixelSize.SetValue(this._renderScale / (float)rtstore.BlurXResBy2.Width, this._renderScale / (float)rtstore.BlurXResBy2.Height);
					this._graphics.ScreenTriangleRenderer.DrawRaw();
					rtstore.BlurXResBy4.Unbind();
					rtstore.BlurXResBy8.Bind(false, true);
					this._gl.BindTexture(GL.TEXTURE_2D, rtstore.BlurXResBy4.GetTexture(RenderTarget.Target.Color0));
					bloomDownsampleBlurProgram.PixelSize.SetValue(this._renderScale / (float)rtstore.BlurXResBy4.Width, this._renderScale / (float)rtstore.BlurXResBy4.Height);
					this._graphics.ScreenTriangleRenderer.DrawRaw();
					rtstore.BlurXResBy8.Unbind();
					rtstore.BlurXResBy16.Bind(false, true);
					this._gl.BindTexture(GL.TEXTURE_2D, rtstore.BlurXResBy8.GetTexture(RenderTarget.Target.Color0));
					bloomDownsampleBlurProgram.PixelSize.SetValue(this._renderScale / (float)rtstore.BlurXResBy8.Width, this._renderScale / (float)rtstore.BlurXResBy8.Height);
					this._graphics.ScreenTriangleRenderer.DrawRaw();
					rtstore.BlurXResBy16.Unbind();
					rtstore.BlurXResBy32.Bind(false, true);
					this._gl.BindTexture(GL.TEXTURE_2D, rtstore.BlurXResBy16.GetTexture(RenderTarget.Target.Color0));
					bloomDownsampleBlurProgram.PixelSize.SetValue(this._renderScale / (float)rtstore.BlurXResBy16.Width, this._renderScale / (float)rtstore.BlurXResBy16.Height);
					this._graphics.ScreenTriangleRenderer.DrawRaw();
					rtstore.BlurXResBy32.Unbind();
					BloomUpsampleBlurProgram bloomUpsampleBlurProgram = this._gpuProgramStore.BloomUpsampleBlurProgram;
					this._gl.UseProgram(bloomUpsampleBlurProgram);
					rtstore.BlurYResBy16.Bind(false, true);
					this._gl.ActiveTexture(GL.TEXTURE1);
					this._gl.BindTexture(GL.TEXTURE_2D, rtstore.BlurXResBy32.GetTexture(RenderTarget.Target.Color0));
					this._gl.ActiveTexture(GL.TEXTURE0);
					this._gl.BindTexture(GL.TEXTURE_2D, rtstore.BlurXResBy16.GetTexture(RenderTarget.Target.Color0));
					bloomUpsampleBlurProgram.PixelSize.SetValue(this._renderScale / (float)rtstore.BlurXResBy32.Width, this._renderScale / (float)rtstore.BlurXResBy32.Height);
					bloomUpsampleBlurProgram.Scale.SetValue(1f);
					bloomUpsampleBlurProgram.Intensity.SetValue(bloomParams.Intensities[4], bloomParams.Intensities[3]);
					this._graphics.ScreenTriangleRenderer.DrawRaw();
					rtstore.BlurYResBy16.Unbind();
					rtstore.BlurYResBy8.Bind(false, true);
					this._gl.ActiveTexture(GL.TEXTURE1);
					this._gl.BindTexture(GL.TEXTURE_2D, rtstore.BlurYResBy16.GetTexture(RenderTarget.Target.Color0));
					this._gl.ActiveTexture(GL.TEXTURE0);
					this._gl.BindTexture(GL.TEXTURE_2D, rtstore.BlurXResBy8.GetTexture(RenderTarget.Target.Color0));
					bloomUpsampleBlurProgram.PixelSize.SetValue(this._renderScale / (float)rtstore.BlurYResBy16.Width, this._renderScale / (float)rtstore.BlurYResBy16.Height);
					bloomUpsampleBlurProgram.Scale.SetValue(1f);
					bloomUpsampleBlurProgram.Intensity.SetValue(1f, bloomParams.Intensities[2]);
					this._graphics.ScreenTriangleRenderer.DrawRaw();
					rtstore.BlurYResBy8.Unbind();
					rtstore.BlurYResBy4.Bind(false, true);
					this._gl.ActiveTexture(GL.TEXTURE1);
					this._gl.BindTexture(GL.TEXTURE_2D, rtstore.BlurYResBy8.GetTexture(RenderTarget.Target.Color0));
					this._gl.ActiveTexture(GL.TEXTURE0);
					this._gl.BindTexture(GL.TEXTURE_2D, rtstore.BlurXResBy4.GetTexture(RenderTarget.Target.Color0));
					bloomUpsampleBlurProgram.PixelSize.SetValue(this._renderScale / (float)rtstore.BlurYResBy8.Width, this._renderScale / (float)rtstore.BlurYResBy8.Height);
					bloomUpsampleBlurProgram.Scale.SetValue(1f);
					bloomUpsampleBlurProgram.Intensity.SetValue(1f, bloomParams.Intensities[1]);
					this._graphics.ScreenTriangleRenderer.DrawRaw();
					rtstore.BlurYResBy4.Unbind();
					rtstore.BlurYResBy2.Bind(false, true);
					this._gl.ActiveTexture(GL.TEXTURE1);
					this._gl.BindTexture(GL.TEXTURE_2D, rtstore.BlurYResBy4.GetTexture(RenderTarget.Target.Color0));
					this._gl.ActiveTexture(GL.TEXTURE0);
					this._gl.BindTexture(GL.TEXTURE_2D, rtstore.BlurXResBy2.GetTexture(RenderTarget.Target.Color0));
					bloomUpsampleBlurProgram.PixelSize.SetValue(this._renderScale / (float)rtstore.BlurYResBy4.Width, this._renderScale / (float)rtstore.BlurYResBy4.Height);
					bloomUpsampleBlurProgram.Scale.SetValue(1f);
					bloomUpsampleBlurProgram.Intensity.SetValue(1f, bloomParams.Intensities[0]);
					this._graphics.ScreenTriangleRenderer.DrawRaw();
					rtstore.BlurYResBy2.Unbind();
				}
				bool flag13 = flag || usePow || useFullbright || useSunshaft;
				if (flag13)
				{
					rtstore.BlurXResBy2.Bind(false, true);
					BloomCompositeProgram bloomCompositeProgram = this._gpuProgramStore.BloomCompositeProgram;
					this._gl.UseProgram(bloomCompositeProgram);
					bool flag14 = useSunshaft;
					if (flag14)
					{
						this._gl.ActiveTexture(GL.TEXTURE3);
						this._gl.BindTexture(GL.TEXTURE_2D, rtstore.SunshaftY.GetTexture(RenderTarget.Target.Color0));
						bloomCompositeProgram.SunshaftIntensity.SetValue(bloomParams.IsSunVisible ? bloomParams.SunshaftIntensity : 0f);
					}
					this._gl.ActiveTexture(GL.TEXTURE0);
					this._gl.BindTexture(GL.TEXTURE_2D, rtstore.BlurYResBy2.GetTexture(RenderTarget.Target.Color0));
					bloomCompositeProgram.BloomIntensity.SetValue(bloomParams.GlobalIntensity);
					this._graphics.ScreenTriangleRenderer.DrawRaw();
					rtstore.BlurXResBy2.Unbind();
				}
			}
		}

		// Token: 0x060052AF RID: 21167 RVA: 0x0016F1C8 File Offset: 0x0016D3C8
		public void UseDistortion(bool enable)
		{
			this._postEffectProgram.UseDistortion = enable;
			this._postEffectProgram.Reset(true);
		}

		// Token: 0x04002D8B RID: 11659
		private bool _hasCameraMoved;

		// Token: 0x04002D8C RID: 11660
		private PostEffectRenderer.DrawParams _postEffectDrawParameters;

		// Token: 0x04002D8D RID: 11661
		private PostEffectRenderer.Settings _postEffectSettings;

		// Token: 0x04002D8E RID: 11662
		private RenderTarget _outputRenderTarget;

		// Token: 0x04002D8F RID: 11663
		private GLTexture _input;

		// Token: 0x04002D90 RID: 11664
		private GLTexture _inputFX;

		// Token: 0x04002D91 RID: 11665
		private GLTexture _depthInput;

		// Token: 0x04002D92 RID: 11666
		private int _width;

		// Token: 0x04002D93 RID: 11667
		private int _height;

		// Token: 0x04002D94 RID: 11668
		private float _renderScale;

		// Token: 0x04002D95 RID: 11669
		private readonly GraphicsDevice _graphics;

		// Token: 0x04002D96 RID: 11670
		private readonly GPUProgramStore _gpuProgramStore;

		// Token: 0x04002D97 RID: 11671
		private GLFunctions _gl;

		// Token: 0x04002D98 RID: 11672
		private GLSampler _linearSampler;

		// Token: 0x04002D99 RID: 11673
		private PostEffectProgram _postEffectProgram;

		// Token: 0x04002D9A RID: 11674
		private Profiling _profiling;

		// Token: 0x04002D9B RID: 11675
		private int _renderingProfileDepthOfField;

		// Token: 0x04002D9C RID: 11676
		private int _renderingProfileBloom;

		// Token: 0x04002D9D RID: 11677
		private int _renderingProfileCombineAndFxaa;

		// Token: 0x04002D9E RID: 11678
		private int _renderingProfileBlur;

		// Token: 0x04002D9F RID: 11679
		private int _renderingProfileTaa;

		// Token: 0x02000EAF RID: 3759
		private struct DepthOfFieldDrawParams
		{
			// Token: 0x0400479A RID: 18330
			public float NearBlurMax;

			// Token: 0x0400479B RID: 18331
			public float NearBlurry;

			// Token: 0x0400479C RID: 18332
			public float NearSharp;

			// Token: 0x0400479D RID: 18333
			public float FarSharp;

			// Token: 0x0400479E RID: 18334
			public float FarBlurry;

			// Token: 0x0400479F RID: 18335
			public float FarBlurMax;

			// Token: 0x040047A0 RID: 18336
			public Matrix ProjectionMatrix;
		}

		// Token: 0x02000EB0 RID: 3760
		private struct BloomDrawParams
		{
			// Token: 0x040047A1 RID: 18337
			public float GlobalIntensity;

			// Token: 0x040047A2 RID: 18338
			public float Power;

			// Token: 0x040047A3 RID: 18339
			public float SunshaftIntensity;

			// Token: 0x040047A4 RID: 18340
			public float SunshaftScale;

			// Token: 0x040047A5 RID: 18341
			public float SunMoonIntensity;

			// Token: 0x040047A6 RID: 18342
			public int ApplyBloom;

			// Token: 0x040047A7 RID: 18343
			public float[] Intensities;

			// Token: 0x040047A8 RID: 18344
			public float PowIntensity;

			// Token: 0x040047A9 RID: 18345
			public float PowPower;

			// Token: 0x040047AA RID: 18346
			public Matrix SunMVP;

			// Token: 0x040047AB RID: 18347
			public bool IsSunVisible;

			// Token: 0x040047AC RID: 18348
			public bool IsMoonVisible;

			// Token: 0x040047AD RID: 18349
			public bool isBloomAllowed;

			// Token: 0x040047AE RID: 18350
			public Vector3 SunColor;

			// Token: 0x040047AF RID: 18351
			public Vector4 MoonColor;

			// Token: 0x040047B0 RID: 18352
			public float Time;
		}

		// Token: 0x02000EB1 RID: 3761
		private struct DrawParams
		{
			// Token: 0x060067E9 RID: 26601 RVA: 0x0021935C File Offset: 0x0021755C
			public void InitDefault()
			{
				this.Time = 0f;
				this.DistortionFrequency = 0f;
				this.DistortionAmplitude = 0f;
				this.ColorBrightness = 0f;
				this.ColorContrast = 1f;
				this.ColorSaturation = 1f;
				this.ColorFilter = new Vector3(1f);
				this.DebugTileResolution = new Vector2(0f);
				this.BloomParams.SunColor = new Vector3(-1f);
				this.VolumetricSunshaftStrength = 2f;
			}

			// Token: 0x040047B1 RID: 18353
			public float Time;

			// Token: 0x040047B2 RID: 18354
			public float DistortionAmplitude;

			// Token: 0x040047B3 RID: 18355
			public float DistortionFrequency;

			// Token: 0x040047B4 RID: 18356
			public float ColorBrightness;

			// Token: 0x040047B5 RID: 18357
			public float ColorContrast;

			// Token: 0x040047B6 RID: 18358
			public float ColorSaturation;

			// Token: 0x040047B7 RID: 18359
			public float VolumetricSunshaftStrength;

			// Token: 0x040047B8 RID: 18360
			public Vector3 ColorFilter;

			// Token: 0x040047B9 RID: 18361
			public Vector2 DebugTileResolution;

			// Token: 0x040047BA RID: 18362
			public PostEffectRenderer.DepthOfFieldDrawParams DepthOfFieldParams;

			// Token: 0x040047BB RID: 18363
			public PostEffectRenderer.BloomDrawParams BloomParams;
		}

		// Token: 0x02000EB2 RID: 3762
		private struct Settings
		{
			// Token: 0x060067EA RID: 26602 RVA: 0x002193EC File Offset: 0x002175EC
			public void InitDefault()
			{
				this.UseFXAAA = true;
				this.UseSharpenPostEffect = true;
				this.BloomSettings.DrawSun = null;
				this.BlurredScreenSettings.ScreenBlurStrength = 2;
				this.BlurredScreenSettings.ScreenBlurScale = 1f;
			}

			// Token: 0x040047BC RID: 18364
			public bool UseDepthOfField;

			// Token: 0x040047BD RID: 18365
			public bool UseBloom;

			// Token: 0x040047BE RID: 18366
			public bool UseTemporalAA;

			// Token: 0x040047BF RID: 18367
			public bool UseFXAAA;

			// Token: 0x040047C0 RID: 18368
			public bool UseSharpenPostEffect;

			// Token: 0x040047C1 RID: 18369
			public bool RequestScreenBlur;

			// Token: 0x040047C2 RID: 18370
			public PostEffectRenderer.BloomSettings BloomSettings;

			// Token: 0x040047C3 RID: 18371
			public PostEffectRenderer.DepthOfFieldSettings DoFSettings;

			// Token: 0x040047C4 RID: 18372
			public PostEffectRenderer.BlurredScreenSettings BlurredScreenSettings;
		}

		// Token: 0x02000EB3 RID: 3763
		private struct BloomSettings
		{
			// Token: 0x040047C5 RID: 18373
			public int Version;

			// Token: 0x040047C6 RID: 18374
			public bool UseSun;

			// Token: 0x040047C7 RID: 18375
			public bool UseMoon;

			// Token: 0x040047C8 RID: 18376
			public bool UseSunshaft;

			// Token: 0x040047C9 RID: 18377
			public bool UseFullbright;

			// Token: 0x040047CA RID: 18378
			public bool UsePow;

			// Token: 0x040047CB RID: 18379
			public int DownsampleMethod;

			// Token: 0x040047CC RID: 18380
			public int UpsampleMethod;

			// Token: 0x040047CD RID: 18381
			public GLTexture GlowMask;

			// Token: 0x040047CE RID: 18382
			public Action DrawSun;

			// Token: 0x040047CF RID: 18383
			public GLTexture SunTexture;

			// Token: 0x040047D0 RID: 18384
			public Action DrawMoon;

			// Token: 0x040047D1 RID: 18385
			public GLTexture MoonTexture;
		}

		// Token: 0x02000EB4 RID: 3764
		private struct DepthOfFieldSettings
		{
			// Token: 0x040047D2 RID: 18386
			public int Version;
		}

		// Token: 0x02000EB5 RID: 3765
		private struct BlurredScreenSettings
		{
			// Token: 0x040047D3 RID: 18387
			public int ScreenBlurStrength;

			// Token: 0x040047D4 RID: 18388
			public float ScreenBlurScale;
		}
	}
}
