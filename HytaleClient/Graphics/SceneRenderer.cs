using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using HytaleClient.Core;
using HytaleClient.Graphics.Gizmos;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;
using HytaleClient.Utils;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A46 RID: 2630
	internal class SceneRenderer : Disposable
	{
		// Token: 0x170012D6 RID: 4822
		// (get) Token: 0x060052FC RID: 21244 RVA: 0x0017429A File Offset: 0x0017249A
		// (set) Token: 0x060052FD RID: 21245 RVA: 0x001742A2 File Offset: 0x001724A2
		public bool UseSSAO { get; private set; } = true;

		// Token: 0x060052FE RID: 21246 RVA: 0x001742AB File Offset: 0x001724AB
		public void ResetSSAOParameters()
		{
			this.SSAOParamOcclusionMax = 0.3f;
			this.SSAOParamOcclusionStrength = 3f;
			this.SSAOParamRadius = 0.75f;
			this._ssaoParamRadiusProjectedScale = 500f;
		}

		// Token: 0x060052FF RID: 21247 RVA: 0x001742DC File Offset: 0x001724DC
		private void PrepareSSAOParameters()
		{
			this._ssaoPackedParameters.X = this.SSAOParamOcclusionMax;
			this._ssaoPackedParameters.Y = this.SSAOParamOcclusionStrength;
			this._ssaoPackedParameters.Z = this.SSAOParamRadius;
			this._ssaoPackedParameters.W = this.SSAOParamRadius * (this._ssaoParamRadiusProjectedScale * this.Data.ViewportSize.X / 1920f);
			this._ssaoTemporalSampleOffset = (float)((ulong)this.Data.FrameCounter % (ulong)((long)this._ssaoSamplesCount)) * 6.2831855f / (float)this._ssaoSamplesCount;
		}

		// Token: 0x170012D7 RID: 4823
		// (get) Token: 0x06005300 RID: 21248 RVA: 0x00174375 File Offset: 0x00172575
		// (set) Token: 0x06005301 RID: 21249 RVA: 0x0017437D File Offset: 0x0017257D
		public int SSAOQuality { get; private set; } = 0;

		// Token: 0x06005302 RID: 21250 RVA: 0x00174388 File Offset: 0x00172588
		private void ComputeSSAOSamplesData()
		{
			for (int i = 0; i < this._ssaoSamplesCount; i++)
			{
				this._ssaoSamplesData[i].X = (float)(Math.Sqrt((double)i + 0.5) / Math.Sqrt((double)this._ssaoSamplesCount));
				this._ssaoSamplesData[i].Y = (float)i * 2.4f;
			}
		}

		// Token: 0x06005303 RID: 21251 RVA: 0x001743F8 File Offset: 0x001725F8
		public void SetUseSSAO(bool useSSAO, bool useTemporalFiltering = true, int quality = -1)
		{
			bool flag = false;
			bool flag2 = quality >= 0 && quality != this.SSAOQuality;
			if (flag2)
			{
				this.SSAOQuality = quality;
				switch (quality)
				{
				case 0:
					this._ssaoResolutionScale = new Vector2(0.5f, 0.5f);
					this._ssaoSamplesCount = 4;
					this._gpuProgramStore.BlurSSAOAndShadowProgram.UseEdgeAwareness = false;
					this._ssaoTapsSource = this._renderTargetStore.LinearZHalfRes;
					break;
				case 1:
					this._ssaoResolutionScale = new Vector2(0.7f, 0.7f);
					this._ssaoSamplesCount = 6;
					this._gpuProgramStore.BlurSSAOAndShadowProgram.UseEdgeAwareness = true;
					this._ssaoTapsSource = this._renderTargetStore.LinearZ;
					break;
				case 2:
					this._ssaoResolutionScale = new Vector2(1f, 1f);
					this._ssaoSamplesCount = 12;
					this._gpuProgramStore.BlurSSAOAndShadowProgram.UseEdgeAwareness = true;
					this._ssaoTapsSource = this._renderTargetStore.LinearZ;
					break;
				}
				this._gpuProgramStore.SSAOProgram.SamplesCount = this._ssaoSamplesCount;
				this.ComputeSSAOSamplesData();
				this._renderTargetStore.ResizeSSAOBuffers(this._renderTargetStore.GBuffer.Width, this._renderTargetStore.GBuffer.Height, this._ssaoResolutionScale);
				flag = true;
			}
			bool flag3 = useTemporalFiltering != this._gpuProgramStore.SSAOProgram.UseTemporalFiltering;
			if (flag3)
			{
				this._gpuProgramStore.SSAOProgram.UseTemporalFiltering = useTemporalFiltering;
				flag = true;
			}
			bool flag4 = this.UseSSAO != useSSAO;
			if (flag4)
			{
				this.UseSSAO = useSSAO;
				this._gpuProgramStore.DeferredProgram.UseSSAO = useSSAO;
				flag = true;
			}
			bool flag5 = flag;
			if (flag5)
			{
				this._gpuProgramStore.DeferredProgram.Reset(true);
				this._gpuProgramStore.SSAOProgram.Reset(true);
				this._gpuProgramStore.BlurSSAOAndShadowProgram.Reset(true);
			}
		}

		// Token: 0x06005304 RID: 21252 RVA: 0x00174600 File Offset: 0x00172800
		public void SetUseSkyAmbient(bool enable)
		{
			bool flag = this._useSkyAmbient != enable;
			if (flag)
			{
				this._useSkyAmbient = enable;
				this._gpuProgramStore.DeferredProgram.UseSkyAmbient = enable;
				this._gpuProgramStore.DeferredProgram.Reset(true);
				this._gpuProgramStore.MapChunkAlphaBlendedProgram.UseSkyAmbient = enable;
				this._gpuProgramStore.MapChunkAlphaBlendedProgram.Reset(true);
			}
		}

		// Token: 0x170012D8 RID: 4824
		// (get) Token: 0x06005305 RID: 21253 RVA: 0x0017466D File Offset: 0x0017286D
		public bool HasDownsampledZ
		{
			get
			{
				return this._hasDownsampledZ;
			}
		}

		// Token: 0x170012D9 RID: 4825
		// (get) Token: 0x06005306 RID: 21254 RVA: 0x00174675 File Offset: 0x00172875
		public GLBuffer SceneDataBuffer
		{
			get
			{
				return this._sceneDataBuffer.Current;
			}
		}

		// Token: 0x06005307 RID: 21255 RVA: 0x00174684 File Offset: 0x00172884
		public SceneRenderer(GraphicsDevice graphics, Profiling profiling, int width, int height)
		{
			this._graphics = graphics;
			this._gpuProgramStore = graphics.GPUProgramStore;
			this._gl = this._graphics.GL;
			this.Profiling = profiling;
			this._renderTargetStore = graphics.RTStore;
			this._ssaoTapsSource = this._renderTargetStore.LinearZHalfRes;
			this.ComputeSSAOSamplesData();
			this.ResetSSAOParameters();
			this.Data.Init(width, height);
			this.PreviousData.Init(width, height);
			this.CreateGPUData();
			this.InitLighting();
			this.InitOcclusionCulling();
			this.InitEntityRendering();
			this.InitSunShadows();
			this.InitOIT();
		}

		// Token: 0x06005308 RID: 21256 RVA: 0x001749E5 File Offset: 0x00172BE5
		protected override void DoDispose()
		{
			this.DestroyGPUData();
			this.DisposeLighting();
			this.DisposeOcclusionCulling();
			this.DisposeEntityRendering();
			this.DisposeSunShadows();
			this.DisposeOIT();
		}

		// Token: 0x06005309 RID: 21257 RVA: 0x00174A14 File Offset: 0x00172C14
		private void CreateGPUData()
		{
			this._pointSampler = this._gl.GenSampler();
			this._gl.SamplerParameteri(this._pointSampler, GL.TEXTURE_WRAP_S, GL.CLAMP_TO_EDGE);
			this._gl.SamplerParameteri(this._pointSampler, GL.TEXTURE_WRAP_T, GL.CLAMP_TO_EDGE);
			this._gl.SamplerParameteri(this._pointSampler, GL.TEXTURE_MAG_FILTER, GL.NEAREST);
			this._gl.SamplerParameteri(this._pointSampler, GL.TEXTURE_MIN_FILTER, GL.NEAREST);
			this._linearSampler = this._gl.GenSampler();
			this._gl.SamplerParameteri(this._linearSampler, GL.TEXTURE_WRAP_S, GL.CLAMP_TO_EDGE);
			this._gl.SamplerParameteri(this._linearSampler, GL.TEXTURE_WRAP_T, GL.CLAMP_TO_EDGE);
			this._gl.SamplerParameteri(this._linearSampler, GL.TEXTURE_MAG_FILTER, GL.LINEAR);
			this._gl.SamplerParameteri(this._linearSampler, GL.TEXTURE_MIN_FILTER, GL.LINEAR);
			this._sceneDataBuffer.CreateStorage(GL.UNIFORM_BUFFER, GL.STREAM_DRAW, true, 1248U, 0U, GPUBuffer.GrowthPolicy.Never, 0U);
			this._emptyVAO = this._gl.GenVertexArray();
		}

		// Token: 0x0600530A RID: 21258 RVA: 0x00174B54 File Offset: 0x00172D54
		private void DestroyGPUData()
		{
			this._gl.DeleteVertexArray(this._emptyVAO);
			this._sceneDataBuffer.DestroyStorage();
			this._gl.DeleteSampler(this._linearSampler);
			this._gl.DeleteSampler(this._pointSampler);
		}

		// Token: 0x0600530B RID: 21259 RVA: 0x00174BA4 File Offset: 0x00172DA4
		private void InitOIT()
		{
			this.OIT = new OrderIndependentTransparency(this._graphics, this._renderTargetStore, this.Profiling);
		}

		// Token: 0x0600530C RID: 21260 RVA: 0x00174BC4 File Offset: 0x00172DC4
		private void DisposeOIT()
		{
			this.OIT = null;
		}

		// Token: 0x0600530D RID: 21261 RVA: 0x00174BCE File Offset: 0x00172DCE
		public void SetupRenderingProfiles(int profileLights, int profileLightsFullRes, int profileLightsLowRes, int profileLightsStencil, int profileLightsMix, int profileLinearZ, int profileLinearZDownsample, int profileZDownsample, int profileEdgeDetection)
		{
			this.SetupLightRenderingProfiles(profileLights, profileLightsFullRes, profileLightsLowRes, profileLightsStencil, profileLightsMix);
			this._renderingProfileLinearZ = profileLinearZ;
			this._renderingProfileLinearZDownsample = profileLinearZDownsample;
			this._renderingProfileZDownsample = profileZDownsample;
			this._renderingProfileEdgeDetection = profileEdgeDetection;
		}

		// Token: 0x0600530E RID: 21262 RVA: 0x00174BFF File Offset: 0x00172DFF
		public void BeginFrame()
		{
			this.ResetOcclusionCullingCounters();
			this.ResetMapCounters();
			this.ResetEntityCounters();
			this.ResetSunShadowsCounters();
			this.PingPongBuffers();
			this.PingPongEntityDataBuffers();
			this.PingPongEntityShadowMapDataBuffers();
		}

		// Token: 0x0600530F RID: 21263 RVA: 0x00174C33 File Offset: 0x00172E33
		public void BeginDraw()
		{
			this.PrepareSSAOParameters();
			this.UpdateLightingHeuristics();
		}

		// Token: 0x06005310 RID: 21264 RVA: 0x00174C44 File Offset: 0x00172E44
		private void PingPongBuffers()
		{
			this._sceneDataBuffer.Swap();
		}

		// Token: 0x06005311 RID: 21265 RVA: 0x00174C54 File Offset: 0x00172E54
		public void Resize(int width, int height)
		{
			int num = (int)((float)width * 0.25f);
			int num2 = (int)((float)height * 0.25f);
			int num3 = (int)((float)width * 0.125f);
			int num4 = (int)((float)height * 0.125f);
			int num5 = (int)((float)width * 0.0625f);
			int num6 = (int)((float)height * 0.0625f);
			this.Data.SetViewportSize(width, height);
		}

		// Token: 0x06005312 RID: 21266 RVA: 0x00174CB0 File Offset: 0x00172EB0
		public void UpdateProjectionMatrix(float fov, double ratio, bool useJittering = false)
		{
			float num = MathHelper.ToRadians(fov);
			this._graphics.CreatePerspectiveMatrix(num, (float)ratio, 0.1f, 1024f, out this.Data.ProjectionMatrix);
			float fieldOfView = MathHelper.ToRadians(MathHelper.Clamp(fov, 40f, 70f));
			this._graphics.CreatePerspectiveMatrix(fieldOfView, (float)ratio, 0.1f, 1024f, out this.Data.FirstPersonProjectionMatrix);
			if (useJittering)
			{
				this.Data.ProjectionJittering = ((this.Data.FrameCounter % 2U == 0U) ? new Vector2(0.5f, 0f) : new Vector2(0f, 0.5f));
				this.Data.ProjectionJittering.X = this.Data.ProjectionJittering.X / (float)this._graphics.RTStore.FinalSceneColor.Width;
				this.Data.ProjectionJittering.Y = this.Data.ProjectionJittering.Y / (float)this._graphics.RTStore.FinalSceneColor.Height;
				this.Data.ProjectionMatrix.M31 = this.Data.ProjectionJittering.X;
				this.Data.ProjectionMatrix.M32 = this.Data.ProjectionJittering.Y;
				this.Data.FirstPersonProjectionMatrix.M31 = this.Data.ProjectionJittering.X;
				this.Data.FirstPersonProjectionMatrix.M32 = this.Data.ProjectionJittering.Y;
			}
			this.Data.WorldFieldOfView = num;
			this.Data.AspectRatio = (float)ratio;
		}

		// Token: 0x06005313 RID: 21267 RVA: 0x00174E54 File Offset: 0x00173054
		public void UpdateRenderData(Vector3 cameraLook, Vector3 cameraPosition, Vector3 localPlayerPosition, uint frameCounter, float time, float deltaTime)
		{
			this.PreviousData = this.Data;
			this.Data.FrameCounter = frameCounter;
			this.Data.Time = time;
			this.Data.DeltaTime = deltaTime;
			cameraLook.X = ((cameraLook.X > 1.5657964f) ? 1.5657964f : cameraLook.X);
			cameraLook.X = ((cameraLook.X < -1.5657964f) ? -1.5657964f : cameraLook.X);
			Matrix.CreateRotationX(-cameraLook.X, out this.Data.ViewMatrix);
			Matrix matrix;
			Matrix.CreateRotationY(-cameraLook.Y, out matrix);
			Matrix.Multiply(ref matrix, ref this.Data.ViewMatrix, out this.Data.ViewRotationMatrix);
			Matrix.CreateRotationZ(-cameraLook.Z, out matrix);
			Matrix.Multiply(ref this.Data.ViewRotationMatrix, ref matrix, out this.Data.ViewRotationMatrix);
			Matrix.Multiply(ref this.Data.ViewRotationMatrix, ref this.Data.ProjectionMatrix, out this.Data.ViewRotationProjectionMatrix);
			this.Data.InvViewRotationProjectionMatrix = Matrix.Invert(this.Data.ViewRotationProjectionMatrix);
			this.Data.InvViewRotationMatrix = Matrix.Invert(this.Data.ViewRotationMatrix);
			this.Data.CameraPosition = cameraPosition;
			this.Data.PlayerRenderPosition = localPlayerPosition - this.Data.CameraPosition;
			Vector3 vector = -this.Data.CameraPosition;
			Matrix.CreateTranslation(ref vector, out matrix);
			Matrix.Multiply(ref matrix, ref this.Data.ViewRotationMatrix, out this.Data.ViewMatrix);
			Matrix.Multiply(ref this.Data.ViewMatrix, ref this.Data.ProjectionMatrix, out this.Data.ViewProjectionMatrix);
			this.Data.ViewFrustum.Matrix = this.Data.ViewProjectionMatrix;
			this.Data.RelativeViewFrustum.Matrix = this.Data.ViewRotationProjectionMatrix;
			this.Data.InvViewProjectionMatrix = Matrix.Invert(this.Data.ViewProjectionMatrix);
			this.Data.InvViewMatrix = Matrix.Invert(this.Data.ViewMatrix);
			this.Data.ReprojectFromCurrentViewToPreviousProjectionMatrix = Matrix.Multiply(this.Data.InvViewMatrix, this.PreviousData.ViewProjectionMatrix);
			this.Data.ReprojectFromPreviousViewToCurrentProjection = Matrix.Multiply(this.PreviousData.InvViewMatrix, this.Data.ViewProjectionMatrix);
			Matrix matrix2;
			Matrix.CreateRotationX(cameraLook.X, out matrix2);
			Matrix.CreateRotationY(cameraLook.Y, out matrix);
			Matrix.Multiply(ref matrix2, ref matrix, out matrix2);
			this.Data.CameraDirection = Vector3.Transform(Vector3.Forward, matrix2);
			this.Data.HasCameraMoved = (this.Data.ViewMatrix != this.PreviousData.ViewMatrix);
			BoundingFrustum boundingFrustum = new BoundingFrustum(this.Data.ViewRotationProjectionMatrix);
			boundingFrustum.GetFarCorners(this.Data.FrustumFarCornersWS);
			Vector3.Transform(this.Data.FrustumFarCornersWS, 0, ref this.Data.ViewRotationMatrix, this.Data.FrustumFarCornersVS, 0, 4);
			this.Data.FrustumFarCornersWS[0] = this.Data.FrustumFarCornersWS[3] + (this.Data.FrustumFarCornersWS[0] - this.Data.FrustumFarCornersWS[3]) * 2f;
			this.Data.FrustumFarCornersWS[2] = this.Data.FrustumFarCornersWS[3] + (this.Data.FrustumFarCornersWS[2] - this.Data.FrustumFarCornersWS[3]) * 2f;
			this.Data.FrustumFarCornersVS[0] = this.Data.FrustumFarCornersVS[3] + (this.Data.FrustumFarCornersVS[0] - this.Data.FrustumFarCornersVS[3]) * 2f;
			this.Data.FrustumFarCornersVS[2] = this.Data.FrustumFarCornersVS[3] + (this.Data.FrustumFarCornersVS[2] - this.Data.FrustumFarCornersVS[3]) * 2f;
		}

		// Token: 0x06005314 RID: 21268 RVA: 0x001752FC File Offset: 0x001734FC
		public void UpdateAtmosphericData(bool isCameraUnderwater, Vector3 sunColor, Vector4 sunLightColor, Vector3 sunPosition, Vector3 fogTopColor, Vector3 fogFrontColor, Vector3 fogBackColor, Vector4 fogParams, Vector4 fogMoodParams, float fogHeightDensityAtViewer, Vector3 ambientFrontColor, Vector3 ambientBackColor, float ambientIntensity, float waterCausticsAnimTime, float waterCausticsDistortion, float waterCausticsScale, float waterCausticsIntensity, float cloudsShadowAnimTime, float cloudsShadowsBlurriness, float cloudsShadowsScale, float cloudsShadowIntensity)
		{
			this.Data.IsCameraUnderwater = isCameraUnderwater;
			this.Data.SunColor = sunColor;
			this.Data.SunLightColor = sunLightColor;
			this.Data.SunPositionWS = sunPosition;
			this.Data.SunPositionVS = Vector3.TransformNormal(sunPosition, this.Data.ViewRotationMatrix);
			this.Data.FogTopColor = fogTopColor;
			this.Data.FogFrontColor = fogFrontColor;
			this.Data.FogBackColor = fogBackColor;
			this.Data.FogParams = fogParams;
			this.Data.FogMoodParams = fogMoodParams;
			this.Data.FogHeightDensityAtViewer = fogHeightDensityAtViewer;
			this.Data.AmbientFrontColor = ambientFrontColor;
			this.Data.AmbientBackColor = ambientBackColor;
			this.Data.AmbientIntensity = ambientIntensity;
			this.Data.WaterCausticsAnimTime = waterCausticsAnimTime;
			this.Data.WaterCausticsDistortion = waterCausticsDistortion;
			this.Data.WaterCausticsScale = waterCausticsScale;
			this.Data.WaterCausticsIntensity = waterCausticsIntensity;
			this.Data.CloudsShadowAnimTime = cloudsShadowAnimTime;
			this.Data.CloudsShadowBlurriness = cloudsShadowsBlurriness;
			this.Data.CloudsShadowScale = cloudsShadowsScale;
			this.Data.CloudsShadowIntensity = cloudsShadowIntensity;
		}

		// Token: 0x06005315 RID: 21269 RVA: 0x00175438 File Offset: 0x00173638
		public void AnalyzeSunOcclusion()
		{
			SunOcclusionDownsampleProgram sunOcclusionDownsampleProgram = this._gpuProgramStore.SunOcclusionDownsampleProgram;
			this._gl.UseProgram(sunOcclusionDownsampleProgram);
			this._renderTargetStore.SunOcclusionBufferLowRes.Bind(false, true);
			this._gl.ActiveTexture(GL.TEXTURE1);
			this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.LinearZHalfRes.GetTexture(RenderTarget.Target.Color0));
			this._gl.ActiveTexture(GL.TEXTURE0);
			this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.LightBufferFullRes.GetTexture(RenderTarget.Target.Color0));
			sunOcclusionDownsampleProgram.CameraPosition.SetValue(this.Data.CameraPosition);
			sunOcclusionDownsampleProgram.CameraDirection.SetValue(this.Data.CameraDirection);
			this._graphics.ScreenTriangleRenderer.Draw();
			this._renderTargetStore.SunOcclusionBufferLowRes.Unbind();
			this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.SunOcclusionBufferLowRes.GetTexture(RenderTarget.Target.Color0));
			this._gl.GenerateMipmap(GL.TEXTURE_2D);
			this._renderTargetStore.SunOcclusionHistory.Bind(false, false);
			int x = (int)(this.Data.FrameCounter % (uint)this._renderTargetStore.SunOcclusionHistory.Width);
			this._gl.Viewport(x, 0, 1, 1);
			this._gl.ActiveTexture(GL.TEXTURE0);
			this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.SunOcclusionBufferLowRes.GetTexture(RenderTarget.Target.Color0));
			ScreenBlitProgram screenBlitProgram = this._gpuProgramStore.ScreenBlitProgram;
			this._gl.UseProgram(screenBlitProgram);
			int value = this._renderTargetStore.SunOcclusionBufferLowRes.GetTextureMipLevelCount(RenderTarget.Target.Color0) - 1;
			screenBlitProgram.MipLevel.SetValue(value);
			this._graphics.ScreenTriangleRenderer.Draw();
			this._renderTargetStore.SunOcclusionHistory.Unbind();
			this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.SunOcclusionHistory.GetTexture(RenderTarget.Target.Color0));
			this._gl.GenerateMipmap(GL.TEXTURE_2D);
		}

		// Token: 0x06005316 RID: 21270 RVA: 0x00175667 File Offset: 0x00173867
		public void BuildReflectionMips()
		{
			this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.PreviousSceneColor.GetTexture(RenderTarget.Target.Color0));
			this._gl.GenerateMipmap(GL.TEXTURE_2D);
		}

		// Token: 0x06005317 RID: 21271 RVA: 0x001756A4 File Offset: 0x001738A4
		public unsafe void SendSceneDataToGPU()
		{
			IntPtr pointer = this._sceneDataBuffer.BeginTransfer(1232U, 0U);
			int num = 0;
			Matrix* ptr = (Matrix*)pointer.ToPointer();
			ptr[(IntPtr)(num++) * (IntPtr)sizeof(Matrix)] = this.Data.FirstPersonViewMatrix;
			ptr[(IntPtr)(num++) * (IntPtr)sizeof(Matrix)] = this.Data.FirstPersonProjectionMatrix;
			ptr[(IntPtr)(num++) * (IntPtr)sizeof(Matrix)] = this.Data.ViewRotationMatrix;
			ptr[(IntPtr)(num++) * (IntPtr)sizeof(Matrix)] = this.Data.ProjectionMatrix;
			ptr[(IntPtr)(num++) * (IntPtr)sizeof(Matrix)] = this.Data.ViewRotationProjectionMatrix;
			ptr[(IntPtr)(num++) * (IntPtr)sizeof(Matrix)] = this.Data.InvViewRotationMatrix;
			ptr[(IntPtr)(num++) * (IntPtr)sizeof(Matrix)] = this.Data.InvViewRotationProjectionMatrix;
			ptr[(IntPtr)(num++) * (IntPtr)sizeof(Matrix)] = this.Data.ReprojectFromCurrentViewToPreviousProjectionMatrix;
			pointer = IntPtr.Add(pointer, sizeof(Matrix) * num);
			num = 0;
			Vector4* ptr2 = (Vector4*)pointer.ToPointer();
			Vector4 vector = new Vector4(-2f / this.Data.ProjectionMatrix.M11, -2f / this.Data.ProjectionMatrix.M22, (1f - this.Data.ProjectionMatrix.M13) / this.Data.ProjectionMatrix.M11, (1f + this.Data.ProjectionMatrix.M23) / this.Data.ProjectionMatrix.M22);
			ptr2[(IntPtr)(num++) * (IntPtr)sizeof(Vector4)] = vector;
			ptr2[(IntPtr)(num++) * (IntPtr)sizeof(Vector4)] = new Vector4(this.Data.CameraPosition, this.Data.IsCameraUnderwater ? 1f : 0f);
			ptr2[(IntPtr)(num++) * (IntPtr)sizeof(Vector4)] = new Vector4(this.Data.CameraDirection);
			ptr2[(IntPtr)(num++) * (IntPtr)sizeof(Vector4)] = new Vector4(this.Data.ViewportSize, this.Data.InvViewportSize);
			ptr2[(IntPtr)(num++) * (IntPtr)sizeof(Vector4)] = new Vector4(0.1f, 1024f, 0f, 0f);
			ptr2[(IntPtr)(num++) * (IntPtr)sizeof(Vector4)] = new Vector4(this.Data.Time, (float)Math.Sin((double)this.Data.Time), (float)Math.Cos((double)this.Data.Time), this.Data.DeltaTime);
			ptr2[(IntPtr)(num++) * (IntPtr)sizeof(Vector4)] = new Vector4(this.Data.SunPositionWS);
			ptr2[(IntPtr)(num++) * (IntPtr)sizeof(Vector4)] = new Vector4(this.Data.SunPositionVS);
			ptr2[(IntPtr)(num++) * (IntPtr)sizeof(Vector4)] = this.Data.SunLightColor;
			ptr2[(IntPtr)(num++) * (IntPtr)sizeof(Vector4)] = new Vector4(this.Data.SunColor);
			ptr2[(IntPtr)(num++) * (IntPtr)sizeof(Vector4)] = new Vector4(this.Data.AmbientFrontColor, this.Data.AmbientIntensity);
			ptr2[(IntPtr)(num++) * (IntPtr)sizeof(Vector4)] = new Vector4(this.Data.AmbientBackColor, this.Data.WaterCausticsAnimTime);
			ptr2[(IntPtr)(num++) * (IntPtr)sizeof(Vector4)] = new Vector4(this.Data.FogTopColor, this.Data.WaterCausticsDistortion);
			ptr2[(IntPtr)(num++) * (IntPtr)sizeof(Vector4)] = new Vector4(this.Data.FogFrontColor, this.Data.WaterCausticsScale);
			ptr2[(IntPtr)(num++) * (IntPtr)sizeof(Vector4)] = new Vector4(this.Data.FogBackColor, this.Data.WaterCausticsIntensity);
			ptr2[(IntPtr)(num++) * (IntPtr)sizeof(Vector4)] = this.Data.FogParams;
			ptr2[(IntPtr)(num++) * (IntPtr)sizeof(Vector4)] = this.Data.FogMoodParams;
			ptr2[(IntPtr)(num++) * (IntPtr)sizeof(Vector4)] = new Vector4(this.Data.FogHeightDensityAtViewer, 0f, 0f, this.Data.CloudsShadowAnimTime);
			ptr2[(IntPtr)(num++) * (IntPtr)sizeof(Vector4)] = new Vector4(this.ClusteredLighting.GridWidth, this.ClusteredLighting.GridHeight, this.ClusteredLighting.GridDepth, 0f);
			ptr2[(IntPtr)(num++) * (IntPtr)sizeof(Vector4)] = new Vector4(this.ClusteredLighting.GridNearZ, this.ClusteredLighting.GridFarZ, this.ClusteredLighting.GridRangeCoef, 0f);
			ptr2[(IntPtr)(num++) * (IntPtr)sizeof(Vector4)] = new Vector4(this.Data.SunShadowRenderData.DynamicShadowIntensity, this.Data.CloudsShadowBlurriness, this.Data.CloudsShadowScale, this.Data.CloudsShadowIntensity);
			for (int i = 0; i < 4; i++)
			{
				ptr2[(IntPtr)(num++) * (IntPtr)sizeof(Vector4)] = new Vector4(this.Data.SunShadowRenderData.CascadeDistanceAndTexelScales[i], 0f, 0f);
			}
			for (int j = 0; j < 4; j++)
			{
				ptr2[(IntPtr)(num++) * (IntPtr)sizeof(Vector4)] = new Vector4(this.Data.SunShadowRenderData.CascadeCachedTranslations[j], 0f);
			}
			pointer = IntPtr.Add(pointer, sizeof(Vector4) * num);
			num = 0;
			ptr = (Matrix*)pointer.ToPointer();
			for (int k = 0; k < 4; k++)
			{
				ptr[(IntPtr)(num++) * (IntPtr)sizeof(Matrix)] = this.Data.SunShadowRenderData.VirtualSunViewRotationProjectionMatrix[k];
			}
			this._sceneDataBuffer.EndTransfer();
			this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.SunOcclusionHistory.GetTexture(RenderTarget.Target.Color0));
			this._gl.UseProgram(this._gpuProgramStore.SceneBrightnessPackProgram);
			this._gl.Enable(GL.RASTERIZER_DISCARD);
			this._gl.BindBufferRange(GL.TRANSFORM_FEEDBACK_BUFFER, 0U, this._sceneDataBuffer.Current.InternalId, (IntPtr)((long)((ulong)1232)), (IntPtr)((long)((ulong)16)));
			this._gl.BeginTransformFeedback(GL.NO_ERROR);
			this._gl.BindVertexArray(this._emptyVAO);
			this._gl.DrawArrays(GL.NO_ERROR, 0, 1);
			this._gl.EndTransformFeedback();
			this._gl.Disable(GL.RASTERIZER_DISCARD);
		}

		// Token: 0x06005318 RID: 21272 RVA: 0x00175E24 File Offset: 0x00174024
		public bool IsSpatialContinuityLost()
		{
			float x = this.QuantifyCameraMotion().X;
			return x > 225f;
		}

		// Token: 0x06005319 RID: 21273 RVA: 0x00175E4C File Offset: 0x0017404C
		private Vector2 QuantifyCameraMotion()
		{
			float x = (this.Data.CameraPosition - this.PreviousData.CameraPosition).LengthSquared();
			float y = (float)Math.Acos((double)Vector3.Dot(this.Data.CameraDirection, this.PreviousData.CameraDirection));
			return new Vector2(x, y);
		}

		// Token: 0x0600531A RID: 21274 RVA: 0x00175EAC File Offset: 0x001740AC
		public void RenderIntermediateBuffers()
		{
			bool needsZBufferEighthRes = this.OIT.NeedsZBufferEighthRes;
			bool flag = this.OIT.NeedsZBufferQuarterRes || needsZBufferEighthRes;
			bool flag2 = this.OIT.NeedsZBufferHalfRes || flag || this._lightResolution > SceneRenderer.LightingResolution.FULL;
			bool flag3 = this._requestLinearZ || this._requestDownsampledLinearZ || this.UseLinearZForLighting;
			bool flag4 = this._requestDownsampledLinearZ || (flag3 && flag2);
			this._hasDownsampledZ = flag2;
			bool flag5 = flag3 || flag2 || flag4 || flag || needsZBufferEighthRes;
			if (flag5)
			{
				this._gl.Disable(GL.STENCIL_TEST);
				this._gl.Disable(GL.DEPTH_TEST);
				bool flag6 = flag3;
				if (flag6)
				{
					this.Profiling.StartMeasure(this._renderingProfileLinearZ);
					RenderTarget hardwareZ = this._renderTargetStore.HardwareZ;
					RenderTarget linearZ = this._renderTargetStore.LinearZ;
					this.GenerateLinearZ(hardwareZ, linearZ);
					this.Profiling.StopMeasure(this._renderingProfileLinearZ);
				}
				else
				{
					this.Profiling.SkipMeasure(this._renderingProfileLinearZ);
				}
				bool flag7 = flag4;
				if (flag7)
				{
					this.Profiling.StartMeasure(this._renderingProfileLinearZDownsample);
					bool setupViewport = true;
					RenderTarget linearZ2 = this._renderTargetStore.LinearZ;
					RenderTarget linearZHalfRes = this._renderTargetStore.LinearZHalfRes;
					ZDownsampleProgram.DownsamplingMode mode = ZDownsampleProgram.DownsamplingMode.Z_MIN_MAX;
					this.DownsampleLinearZ(setupViewport, linearZ2, linearZHalfRes, mode);
					this.Profiling.StopMeasure(this._renderingProfileLinearZDownsample);
				}
				else
				{
					this.Profiling.SkipMeasure(this._renderingProfileLinearZDownsample);
				}
				bool flag8 = flag2 || flag || needsZBufferEighthRes;
				if (flag8)
				{
					this.Profiling.StartMeasure(this._renderingProfileZDownsample);
					this._gl.Enable(GL.DEPTH_TEST);
					this._gl.DepthMask(true);
					this._gl.DepthFunc(GL.ALWAYS);
					ZDownsampleProgram.DownsamplingMode mode2 = ZDownsampleProgram.DownsamplingMode.Z_MIN;
					ZDownsampleProgram.DownsamplingMode mode3 = ZDownsampleProgram.DownsamplingMode.Z_MIN;
					ZDownsampleProgram.DownsamplingMode mode4 = ZDownsampleProgram.DownsamplingMode.Z_MIN;
					bool flag9 = flag2;
					if (flag9)
					{
						this.DownsampleZBuffer(true, this._renderTargetStore.HardwareZ, this._renderTargetStore.HardwareZHalfRes, mode2);
					}
					bool flag10 = flag;
					if (flag10)
					{
						this.DownsampleZBuffer(true, this._renderTargetStore.HardwareZHalfRes, this._renderTargetStore.HardwareZQuarterRes, mode3);
					}
					bool flag11 = needsZBufferEighthRes;
					if (flag11)
					{
						this.DownsampleZBuffer(true, this._renderTargetStore.HardwareZQuarterRes, this._renderTargetStore.HardwareZEighthRes, mode4);
					}
					this._gl.DepthFunc(GL.LEQUAL);
					this._gl.DepthMask(false);
					this.Profiling.StopMeasure(this._renderingProfileZDownsample);
				}
				else
				{
					this.Profiling.SkipMeasure(this._renderingProfileZDownsample);
				}
				bool flag12 = !flag2;
				if (flag12)
				{
					this._gl.Enable(GL.DEPTH_TEST);
				}
				this._gl.Enable(GL.STENCIL_TEST);
			}
			else
			{
				this.Profiling.SkipMeasure(this._renderingProfileLinearZ);
				this.Profiling.SkipMeasure(this._renderingProfileLinearZDownsample);
				this.Profiling.SkipMeasure(this._renderingProfileZDownsample);
			}
			int num = this.UseClusteredLighting ? this.ClusteredLighting.LightCount : this.ClassicDeferredLighting.LightCount;
			bool flag13 = (num > 0 && this._lightResolution == SceneRenderer.LightingResolution.MIXED) || this.OIT.HasHalfResPass || this.OIT.HasQuarterResPass;
			bool flag14 = flag13;
			if (flag14)
			{
				this.Profiling.StartMeasure(this._renderingProfileEdgeDetection);
				this._gl.Disable(GL.DEPTH_TEST);
				this._renderTargetStore.Edges.Bind(true, true);
				int inputDownscaleFactor = this.OIT.HasQuarterResPass ? 4 : 2;
				this.TagEdges(7, inputDownscaleFactor);
				this._renderTargetStore.Edges.Unbind();
				this._gl.Enable(GL.DEPTH_TEST);
				this.Profiling.StopMeasure(this._renderingProfileEdgeDetection);
			}
			else
			{
				this.Profiling.SkipMeasure(this._renderingProfileEdgeDetection);
			}
		}

		// Token: 0x0600531B RID: 21275 RVA: 0x001762A8 File Offset: 0x001744A8
		public void TagEdges(byte writeStencilBitId = 7, int inputDownscaleFactor = 1)
		{
			Debug.Assert(inputDownscaleFactor == 1 || inputDownscaleFactor == 2 || inputDownscaleFactor == 4);
			Debug.Assert(writeStencilBitId < 8, string.Format("Invalid stencil bit id requested for Edges: {0}. Valide entries are[0-7].", writeStencilBitId));
			this._gl.AssertEnabled(GL.STENCIL_TEST);
			this._gl.AssertDisabled(GL.DEPTH_TEST);
			this._gl.AssertDepthMask(false);
			bool useLinearZ = this._gpuProgramStore.EdgeDetectionProgram.UseLinearZ;
			GLTexture texture;
			Vector2 value;
			switch (inputDownscaleFactor)
			{
			case 2:
				texture = (useLinearZ ? this._renderTargetStore.LinearZHalfRes.GetTexture(RenderTarget.Target.Color0) : this._renderTargetStore.HardwareZHalfRes.GetTexture(RenderTarget.Target.Depth));
				value = (useLinearZ ? this._renderTargetStore.LinearZHalfRes.InvResolution : this._renderTargetStore.HardwareZHalfRes.InvResolution);
				goto IL_15A;
			case 4:
				texture = this._renderTargetStore.HardwareZQuarterRes.GetTexture(RenderTarget.Target.Depth);
				value = this._renderTargetStore.HardwareZQuarterRes.InvResolution;
				goto IL_15A;
			}
			texture = (useLinearZ ? this._renderTargetStore.LinearZ.GetTexture(RenderTarget.Target.Color0) : this._renderTargetStore.HardwareZ.GetTexture(RenderTarget.Target.Depth));
			value = (useLinearZ ? this._renderTargetStore.LinearZ.InvResolution : this._renderTargetStore.HardwareZ.InvResolution);
			IL_15A:
			uint mask = 1U << (int)writeStencilBitId;
			this._gl.StencilFunc(GL.ALWAYS, 1 << (int)writeStencilBitId, mask);
			this._gl.StencilMask(mask);
			this._gl.StencilOp(GL.KEEP, GL.KEEP, GL.REPLACE);
			this._gl.BindTexture(GL.TEXTURE_2D, texture);
			EdgeDetectionProgram edgeDetectionProgram = this._gpuProgramStore.EdgeDetectionProgram;
			this._gl.UseProgram(edgeDetectionProgram);
			edgeDetectionProgram.InvDepthTextureSize.SetValue(value);
			bool flag = useLinearZ;
			if (flag)
			{
				edgeDetectionProgram.FarClip.SetValue(1024f);
			}
			else
			{
				edgeDetectionProgram.ProjectionMatrix.SetValue(ref this.Data.ProjectionMatrix);
			}
			this._graphics.ScreenTriangleRenderer.Draw();
			this._gl.ColorMask(true, true, true, true);
			this._gl.StencilMask(255U);
		}

		// Token: 0x0600531C RID: 21276 RVA: 0x0017650C File Offset: 0x0017470C
		public void DrawSSAO()
		{
			this._renderTargetStore.SSAORaw.Bind(true, true);
			this._gl.ActiveTexture(GL.TEXTURE4);
			this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.DeferredShadow.GetTexture(RenderTarget.Target.Color0));
			this._gl.ActiveTexture(GL.TEXTURE3);
			this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.BlurSSAOAndShadow.GetTexture(RenderTarget.Target.Color0));
			this._gl.ActiveTexture(GL.TEXTURE2);
			this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.GBuffer.GetTexture(RenderTarget.Target.Color0));
			this._gl.ActiveTexture(GL.TEXTURE1);
			this._gl.BindTexture(GL.TEXTURE_2D, this._ssaoTapsSource.GetTexture(RenderTarget.Target.Color0));
			this._gl.ActiveTexture(GL.TEXTURE0);
			this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.LinearZ.GetTexture(RenderTarget.Target.Color0));
			SSAOProgram ssaoprogram = this._gpuProgramStore.SSAOProgram;
			this._gl.UseProgram(ssaoprogram);
			ssaoprogram.PackedParameters.SetValue(this._ssaoPackedParameters);
			ssaoprogram.ViewportSize.SetValue(this.Data.ViewportSize.X, this.Data.ViewportSize.Y);
			ssaoprogram.ViewMatrix.SetValue(ref this.Data.ViewRotationMatrix);
			ssaoprogram.ProjectionMatrix.SetValue(ref this.Data.ProjectionMatrix);
			ssaoprogram.ReprojectMatrix.SetValue(ref this.Data.ReprojectFromCurrentViewToPreviousProjectionMatrix);
			ssaoprogram.FarCorners.SetValue(this.Data.FrustumFarCornersVS);
			ssaoprogram.SamplesData.SetValue(this._ssaoSamplesData);
			ssaoprogram.TemporalSampleOffset.SetValue(this._ssaoTemporalSampleOffset);
			this._graphics.ScreenTriangleRenderer.Draw();
			this._renderTargetStore.SSAORaw.Unbind();
		}

		// Token: 0x0600531D RID: 21277 RVA: 0x00176724 File Offset: 0x00174924
		public void BlurSSAOAndShadow()
		{
			BlurProgram blurSSAOAndShadowProgram = this._gpuProgramStore.BlurSSAOAndShadowProgram;
			this._renderTargetStore.BlurSSAOAndShadowTmp.Bind(true, false);
			this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.SSAORaw.GetTexture(RenderTarget.Target.Color0));
			this._gl.UseProgram(blurSSAOAndShadowProgram);
			blurSSAOAndShadowProgram.PixelSize.SetValue(1f / (float)this._renderTargetStore.BlurSSAOAndShadow.Width, 1f / (float)this._renderTargetStore.BlurSSAOAndShadow.Height);
			blurSSAOAndShadowProgram.BlurScale.SetValue(1f);
			blurSSAOAndShadowProgram.HorizontalPass.SetValue(1f);
			this._graphics.ScreenTriangleRenderer.DrawRaw();
			this._renderTargetStore.BlurSSAOAndShadowTmp.Unbind();
			this._renderTargetStore.BlurSSAOAndShadow.Bind(false, false);
			this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.BlurSSAOAndShadowTmp.GetTexture(RenderTarget.Target.Color0));
			blurSSAOAndShadowProgram.HorizontalPass.SetValue(0f);
			this._graphics.ScreenTriangleRenderer.DrawRaw();
			this._renderTargetStore.BlurSSAOAndShadow.Unbind();
		}

		// Token: 0x0600531E RID: 21278 RVA: 0x00176868 File Offset: 0x00174A68
		public void ApplyDeferred(GLTexture topProjectionTexture, GLTexture fogNoiseTexture)
		{
			GLFunctions gl = this._graphics.GL;
			this._gl.StencilFunc(GL.ALWAYS, 0, 255U);
			this._gl.ActiveTexture(GL.TEXTURE6);
			this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.DeferredShadow.GetTexture(RenderTarget.Target.Color0));
			DeferredProgram deferredProgram = this._gpuProgramStore.DeferredProgram;
			bool useMoodFog = deferredProgram.UseMoodFog;
			if (useMoodFog)
			{
				this._gl.ActiveTexture(GL.TEXTURE7);
				this._gl.BindTexture(GL.TEXTURE_2D, fogNoiseTexture);
			}
			this._gl.ActiveTexture(GL.TEXTURE4);
			this._gl.BindTexture(GL.TEXTURE_2D, topProjectionTexture);
			RenderTarget renderTarget = this.UseSSAOBlur ? this._renderTargetStore.BlurSSAOAndShadow : this._renderTargetStore.SSAORaw;
			this._gl.ActiveTexture(GL.TEXTURE3);
			bool useSmartUpsampling = deferredProgram.UseSmartUpsampling;
			if (useSmartUpsampling)
			{
				this._gl.BindSampler(3U, this._pointSampler);
			}
			this._gl.BindTexture(GL.TEXTURE_2D, renderTarget.GetTexture(RenderTarget.Target.Color0));
			GLTexture texture = this._graphics.UseLinearZ ? this._renderTargetStore.LinearZ.GetTexture(RenderTarget.Target.Color0) : this._renderTargetStore.GBuffer.GetTexture(RenderTarget.Target.Depth);
			this._gl.ActiveTexture(GL.TEXTURE2);
			this._gl.BindTexture(GL.TEXTURE_2D, texture);
			this._gl.ActiveTexture(GL.TEXTURE1);
			this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.LightBufferFullRes.GetTexture(RenderTarget.Target.Color0));
			this._gl.ActiveTexture(GL.TEXTURE0);
			this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.GBuffer.GetTexture(RenderTarget.Target.Color0));
			deferredProgram.SceneDataBlock.SetBuffer(this.SceneDataBuffer);
			this._gl.UseProgram(deferredProgram);
			bool useLinearZ = this._graphics.UseLinearZ;
			if (useLinearZ)
			{
				deferredProgram.FarCorners.SetValue(this.Data.FrustumFarCornersWS);
			}
			bool debugShadowCascades = deferredProgram.DebugShadowCascades;
			if (debugShadowCascades)
			{
				deferredProgram.DebugShadowMatrix.SetValue(this.Data.SunShadowRenderData.VirtualSunViewRotationProjectionMatrix);
			}
			this._graphics.ScreenTriangleRenderer.Draw();
			bool useSmartUpsampling2 = deferredProgram.UseSmartUpsampling;
			if (useSmartUpsampling2)
			{
				this._gl.BindSampler(3U, GLSampler.None);
			}
		}

		// Token: 0x0600531F RID: 21279 RVA: 0x00176AF4 File Offset: 0x00174CF4
		public void BlitSceneColorToHalfRes(RenderTarget sceneColor, GL filteringMode = GL.LINEAR, bool generateMipMap = false, bool bindSource = false, bool rebindSourceAfter = true)
		{
			sceneColor.CopyColorTo(this._renderTargetStore.SceneColorHalfRes, GL.COLOR_ATTACHMENT0, GL.COLOR_ATTACHMENT0, filteringMode, bindSource, rebindSourceAfter);
			if (generateMipMap)
			{
				this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.SceneColorHalfRes.GetTexture(RenderTarget.Target.Color0));
				this._gl.GenerateMipmap(GL.TEXTURE_2D);
			}
		}

		// Token: 0x06005320 RID: 21280 RVA: 0x00176B64 File Offset: 0x00174D64
		public void GenerateLinearZ(RenderTarget source, RenderTarget destination)
		{
			this._gl.AssertDisabled(GL.STENCIL_TEST);
			this._gl.AssertDisabled(GL.DEPTH_TEST);
			this._gl.AssertDepthMask(false);
			destination.Bind(false, false);
			LinearZProgram linearZProgram = this._gpuProgramStore.LinearZProgram;
			this._gl.UseProgram(linearZProgram);
			linearZProgram.ProjectionMatrix.SetValue(ref this.Data.ProjectionMatrix);
			linearZProgram.InvFarClip.SetValue(0.0009765625f);
			this._gl.BindTexture(GL.TEXTURE_2D, source.GetTexture(RenderTarget.Target.Depth));
			this._graphics.ScreenTriangleRenderer.Draw();
			destination.Unbind();
		}

		// Token: 0x06005321 RID: 21281 RVA: 0x00176C1C File Offset: 0x00174E1C
		public void DownsampleZBuffer(bool setupViewport, RenderTarget source, RenderTarget destination, ZDownsampleProgram.DownsamplingMode mode)
		{
			this._gl.AssertDisabled(GL.STENCIL_TEST);
			this._gl.AssertEnabled(GL.DEPTH_TEST);
			this._gl.AssertDepthFunc(GL.ALWAYS);
			this._gl.AssertDepthMask(true);
			destination.Bind(false, setupViewport);
			this._gl.BindTexture(GL.TEXTURE_2D, source.GetTexture(RenderTarget.Target.Depth));
			ZDownsampleProgram zdownsampleProgram = this._gpuProgramStore.ZDownsampleProgram;
			this._gl.UseProgram(zdownsampleProgram);
			zdownsampleProgram.Mode.SetValue((int)mode);
			zdownsampleProgram.PixelSize.SetValue(source.InvWidth, source.InvHeight);
			this._graphics.ScreenTriangleRenderer.Draw();
			destination.Unbind();
		}

		// Token: 0x06005322 RID: 21282 RVA: 0x00176CE4 File Offset: 0x00174EE4
		public void DownsampleLinearZ(bool setupViewport, RenderTarget source, RenderTarget destination, ZDownsampleProgram.DownsamplingMode mode)
		{
			this._gl.AssertDisabled(GL.STENCIL_TEST);
			this._gl.AssertDisabled(GL.DEPTH_TEST);
			destination.Bind(false, setupViewport);
			this._gl.BindTexture(GL.TEXTURE_2D, source.GetTexture(RenderTarget.Target.Color0));
			ZDownsampleProgram linearZDownsampleProgram = this._gpuProgramStore.LinearZDownsampleProgram;
			this._gl.UseProgram(linearZDownsampleProgram);
			linearZDownsampleProgram.Mode.SetValue((int)mode);
			linearZDownsampleProgram.PixelSize.SetValue(source.InvWidth, source.InvHeight);
			this._graphics.ScreenTriangleRenderer.Draw();
			destination.Unbind();
		}

		// Token: 0x170012DA RID: 4826
		// (get) Token: 0x06005323 RID: 21283 RVA: 0x00176D8C File Offset: 0x00174F8C
		public bool HasVisibleNameplates
		{
			get
			{
				return this._nameplateDrawTaskCount != 0;
			}
		}

		// Token: 0x06005324 RID: 21284 RVA: 0x00176DA7 File Offset: 0x00174FA7
		public void SetupModelVFXDataTexture(uint unitId)
		{
			this._gl.ActiveTexture(GL.TEXTURE0 + unitId);
			this._gl.BindTexture(GL.TEXTURE_BUFFER, this._modelVFXDataBufferTexture.CurrentTexture);
		}

		// Token: 0x06005325 RID: 21285 RVA: 0x00176DD9 File Offset: 0x00174FD9
		public void InitModelVFXGPUData()
		{
			this._modelVFXDataBufferTexture.CreateStorage(GL.RGBA32F, GL.STREAM_DRAW, true, this._modelVFXBufferSize, 200U, GPUBuffer.GrowthPolicy.GrowthAutoNoLimit, 0U);
		}

		// Token: 0x06005326 RID: 21286 RVA: 0x00176E00 File Offset: 0x00175000
		public void DisposeModelVFXGPUData()
		{
			this._modelVFXDataBufferTexture.DestroyStorage();
		}

		// Token: 0x06005327 RID: 21287 RVA: 0x00176E10 File Offset: 0x00175010
		public unsafe void SendModelVFXDataToGPU()
		{
			uint num = (uint)(this._modelVFXDrawTaskCount * (int)SceneRenderer.ModelVFXDataSize);
			bool flag = num > 0U;
			if (flag)
			{
				this._modelVFXDataBufferTexture.GrowStorageIfNecessary(num);
				IntPtr pointer = this._modelVFXDataBufferTexture.BeginTransfer(num);
				for (int i = 0; i < this._modelVFXDrawTaskCount; i++)
				{
					Vector4* ptr = (Vector4*)IntPtr.Add(pointer, i * (int)SceneRenderer.ModelVFXDataSize).ToPointer();
					*ptr = new Vector4(this._modelVFXDrawTasks[i].ModelVFXHighlightColor.X, this._modelVFXDrawTasks[i].ModelVFXHighlightColor.Y, this._modelVFXDrawTasks[i].ModelVFXHighlightColor.Z, this._modelVFXDrawTasks[i].ModelVFXHighlightThickness);
					ptr[1] = new Vector4(this._modelVFXDrawTasks[i].ModelVFXNoiseScale.X, this._modelVFXDrawTasks[i].ModelVFXNoiseScale.Y, this._modelVFXDrawTasks[i].ModelVFXNoiseScrollSpeed.X, this._modelVFXDrawTasks[i].ModelVFXNoiseScrollSpeed.Y);
					ptr[2] = new Vector4(this._modelVFXDrawTasks[i].ModelVFXPostColor.X, this._modelVFXDrawTasks[i].ModelVFXPostColor.Y, this._modelVFXDrawTasks[i].ModelVFXPostColor.Z, this._modelVFXDrawTasks[i].ModelVFXPostColor.W);
					ptr[3] = new Vector4((float)this._modelVFXDrawTasks[i].ModelVFXPackedParams, 0f, 0f, 0f);
				}
				this._modelVFXDataBufferTexture.EndTransfer();
			}
		}

		// Token: 0x06005328 RID: 21288 RVA: 0x00177006 File Offset: 0x00175206
		public void SetupEntityDataTexture(uint unitId)
		{
			this._gl.ActiveTexture(GL.TEXTURE0 + unitId);
			this._gl.BindTexture(GL.TEXTURE_BUFFER, this._entityDataBufferTexture.CurrentTexture);
		}

		// Token: 0x06005329 RID: 21289 RVA: 0x00177038 File Offset: 0x00175238
		private void InitEntitiesGPUData()
		{
			this._entityDataBufferTexture.CreateStorage(GL.RGBA32F, GL.STREAM_DRAW, true, this._entityBufferSize, 1024U, GPUBuffer.GrowthPolicy.GrowthAutoNoLimit, 0U);
		}

		// Token: 0x0600532A RID: 21290 RVA: 0x0017705F File Offset: 0x0017525F
		private void DisposeEntitiesGPUData()
		{
			this._entityDataBufferTexture.DestroyStorage();
		}

		// Token: 0x0600532B RID: 21291 RVA: 0x0017706E File Offset: 0x0017526E
		private void PingPongEntityDataBuffers()
		{
			this._entityDataBufferTexture.Swap();
		}

		// Token: 0x0600532C RID: 21292 RVA: 0x00177080 File Offset: 0x00175280
		public unsafe void SendEntityDataToGPU()
		{
			uint num = (uint)((this._entityDrawTaskCount + this._entityForwardDrawTaskCount) * SceneRenderer.GPUEntityDataSize);
			uint num2 = (uint)(this._entityDistortionDrawTaskCount * SceneRenderer.GPUEntityDistortionDataSize);
			uint num3 = num + num2;
			bool flag = num3 > 0U;
			if (flag)
			{
				this._entityDataBufferTexture.GrowStorageIfNecessary(num3);
				IntPtr pointer = this._entityDataBufferTexture.BeginTransfer(num3);
				for (int i = 0; i < this._entityDrawTaskCount; i++)
				{
					IntPtr pointer2 = IntPtr.Add(pointer, i * SceneRenderer.GPUEntityDataSize);
					Matrix* ptr = (Matrix*)pointer2.ToPointer();
					*ptr = this._entityDrawTasks[i].ModelMatrix;
					Vector4* ptr2 = (Vector4*)IntPtr.Add(pointer2, sizeof(Matrix)).ToPointer();
					*ptr2 = this._entityDrawTasks[i].BlockLightColor;
					ptr2[1] = new Vector4(this._entityDrawTasks[i].BottomTint.X, this._entityDrawTasks[i].BottomTint.Y, this._entityDrawTasks[i].BottomTint.Z, this._entityDrawTasks[i].ModelVFXAnimationProgress);
					ptr2[2] = new Vector4(this._entityDrawTasks[i].TopTint.X, this._entityDrawTasks[i].TopTint.Y, this._entityDrawTasks[i].TopTint.Z, (float)this._entityDrawTasks[i].ModelVFXId);
					ptr2[3] = new Vector4(this._entityDrawTasks[i].InvModelHeight, this._entityDrawTasks[i].UseDithering, 0f, 0f);
				}
				for (int j = 0; j < this._entityForwardDrawTaskCount; j++)
				{
					IntPtr pointer3 = IntPtr.Add(pointer, (j + this._entityDrawTaskCount) * SceneRenderer.GPUEntityDataSize);
					Matrix* ptr3 = (Matrix*)pointer3.ToPointer();
					*ptr3 = this._entityForwardDrawTasks[j].ModelMatrix;
					Vector4* ptr4 = (Vector4*)IntPtr.Add(pointer3, sizeof(Matrix)).ToPointer();
					*ptr4 = this._entityForwardDrawTasks[j].BlockLightColor;
					ptr4[1] = new Vector4(this._entityForwardDrawTasks[j].BottomTint.X, this._entityForwardDrawTasks[j].BottomTint.Y, this._entityForwardDrawTasks[j].BottomTint.Z, this._entityForwardDrawTasks[j].ModelVFXAnimationProgress);
					ptr4[2] = new Vector4(this._entityForwardDrawTasks[j].TopTint.X, this._entityForwardDrawTasks[j].TopTint.Y, this._entityForwardDrawTasks[j].TopTint.Z, (float)this._entityForwardDrawTasks[j].ModelVFXId);
					ptr4[3] = new Vector4(this._entityForwardDrawTasks[j].InvModelHeight, this._entityForwardDrawTasks[j].UseDithering, 0f, 0f);
				}
				pointer = IntPtr.Add(pointer, (int)num);
				for (int k = 0; k < this._entityDistortionDrawTaskCount; k++)
				{
					IntPtr pointer4 = IntPtr.Add(pointer, k * SceneRenderer.GPUEntityDistortionDataSize);
					Matrix* ptr5 = (Matrix*)pointer4.ToPointer();
					*ptr5 = this._entityDistortionDrawTasks[k].ModelMatrix;
					Vector4* ptr6 = (Vector4*)IntPtr.Add(pointer4, sizeof(Matrix)).ToPointer();
					*ptr6 = new Vector4(this._entityDistortionDrawTasks[k].ModelVFXAnimationProgress, (float)this._entityDistortionDrawTasks[k].ModelVFXId, this._entityDistortionDrawTasks[k].InvModelHeight, 0f);
				}
				this._entityDataBufferTexture.EndTransfer();
			}
		}

		// Token: 0x170012DB RID: 4827
		// (get) Token: 0x0600532D RID: 21293 RVA: 0x001774F6 File Offset: 0x001756F6
		public bool HasEntityDistortionTask
		{
			get
			{
				return this._entityDistortionDrawTaskCount > 0;
			}
		}

		// Token: 0x0600532E RID: 21294 RVA: 0x00177504 File Offset: 0x00175704
		protected void InitEntityRendering()
		{
			BasicFogProgram basicFogProgram = this._gpuProgramStore.BasicFogProgram;
			BasicProgram basicProgram = this._gpuProgramStore.BasicProgram;
			this._quadRenderer = new QuadRenderer(this._graphics, basicFogProgram.AttribPosition, basicFogProgram.AttribTexCoords);
			this._boxRenderer = new BoxRenderer(this._graphics, basicProgram);
			this._lineRenderer = new LineRenderer(this._graphics, basicProgram);
			this._lineRenderer.UpdateLineData(new Vector3[]
			{
				new Vector3(0f, 0f, 0f),
				new Vector3(1f, 0f, 0f)
			});
			this.InitEntitiesGPUData();
			this.InitModelVFXGPUData();
		}

		// Token: 0x0600532F RID: 21295 RVA: 0x001775C1 File Offset: 0x001757C1
		protected void DisposeEntityRendering()
		{
			this._lineRenderer.Dispose();
			this._boxRenderer.Dispose();
			this._quadRenderer.Dispose();
			this.DisposeEntitiesGPUData();
			this.DisposeModelVFXGPUData();
		}

		// Token: 0x06005330 RID: 21296 RVA: 0x001775F6 File Offset: 0x001757F6
		private void ResetEntityCounters()
		{
			this._entityDrawTaskCount = 0;
			this._entityForwardDrawTaskCount = 0;
			this._entityDistortionDrawTaskCount = 0;
			this._modelVFXDrawTaskCount = 0;
			this._incomingEntityDrawTaskCount = 0;
			this._nameplateDrawTaskCount = 0;
			this._debugInfoDrawTaskCount = 0;
		}

		// Token: 0x06005331 RID: 21297 RVA: 0x0017762C File Offset: 0x0017582C
		public void PrepareForIncomingEntityDrawTasks(int size)
		{
			this._incomingEntityDrawTaskCount += size;
			ArrayUtils.GrowArrayIfNecessary<SceneRenderer.EntityDrawTask>(ref this._entityDrawTasks, this._incomingEntityDrawTaskCount, 200);
			ArrayUtils.GrowArrayIfNecessary<SceneRenderer.EntityDrawTask>(ref this._entityForwardDrawTasks, this._incomingEntityDrawTaskCount, 200);
			ArrayUtils.GrowArrayIfNecessary<SceneRenderer.EntityDistortionDrawTask>(ref this._entityDistortionDrawTasks, this._incomingEntityDrawTaskCount, 200);
			ArrayUtils.GrowArrayIfNecessary<SceneRenderer.ModelVFXDrawTask>(ref this._modelVFXDrawTasks, this._incomingEntityDrawTaskCount, 200);
		}

		// Token: 0x06005332 RID: 21298 RVA: 0x001776A4 File Offset: 0x001758A4
		public void RegisterEntityDrawTasks(int entityLocalId, ref Matrix modelMatrix, GLVertexArray vertexArray, int dataCount, GLBuffer animationData, uint animationDataOffset, ushort animationDataCount, Vector4 blockLightColor, Vector3 bottomTint, Vector3 topTint, float modelHeight, bool useDithering, float modelVFXAnimationProgress, int packedModelVFXParams, int modelVFXId)
		{
			if (useDithering)
			{
				int entityForwardDrawTaskCount = this._entityForwardDrawTaskCount;
				this._entityForwardDrawTasks[entityForwardDrawTaskCount].BlockLightColor = blockLightColor;
				this._entityForwardDrawTasks[entityForwardDrawTaskCount].BottomTint = bottomTint;
				this._entityForwardDrawTasks[entityForwardDrawTaskCount].TopTint = topTint;
				this._entityForwardDrawTasks[entityForwardDrawTaskCount].InvModelHeight = 1f / modelHeight;
				this._entityForwardDrawTasks[entityForwardDrawTaskCount].ModelMatrix = modelMatrix;
				this._entityForwardDrawTasks[entityForwardDrawTaskCount].VertexArray = vertexArray;
				this._entityForwardDrawTasks[entityForwardDrawTaskCount].DataCount = dataCount;
				this._entityForwardDrawTasks[entityForwardDrawTaskCount].AnimationData = animationData;
				this._entityForwardDrawTasks[entityForwardDrawTaskCount].AnimationDataOffset = animationDataOffset;
				this._entityForwardDrawTasks[entityForwardDrawTaskCount].AnimationDataSize = animationDataCount * 64;
				this._entityForwardDrawTasks[entityForwardDrawTaskCount].ModelVFXAnimationProgress = modelVFXAnimationProgress;
				this._entityForwardDrawTasks[entityForwardDrawTaskCount].ModelVFXId = modelVFXId;
				this._entityForwardDrawTasks[entityForwardDrawTaskCount].UseDithering = (useDithering ? 1f : 0f);
				this._entityForwardDrawTasks[entityForwardDrawTaskCount].EntityLocalId = (ushort)entityLocalId;
				this._entityForwardDrawTaskCount++;
			}
			else
			{
				int entityDrawTaskCount = this._entityDrawTaskCount;
				this._entityDrawTasks[entityDrawTaskCount].BlockLightColor = blockLightColor;
				this._entityDrawTasks[entityDrawTaskCount].BottomTint = bottomTint;
				this._entityDrawTasks[entityDrawTaskCount].TopTint = topTint;
				this._entityDrawTasks[entityDrawTaskCount].InvModelHeight = 1f / modelHeight;
				this._entityDrawTasks[entityDrawTaskCount].ModelMatrix = modelMatrix;
				this._entityDrawTasks[entityDrawTaskCount].VertexArray = vertexArray;
				this._entityDrawTasks[entityDrawTaskCount].DataCount = dataCount;
				this._entityDrawTasks[entityDrawTaskCount].AnimationData = animationData;
				this._entityDrawTasks[entityDrawTaskCount].AnimationDataOffset = animationDataOffset;
				this._entityDrawTasks[entityDrawTaskCount].AnimationDataSize = animationDataCount * 64;
				this._entityDrawTasks[entityDrawTaskCount].ModelVFXAnimationProgress = modelVFXAnimationProgress;
				this._entityDrawTasks[entityDrawTaskCount].ModelVFXId = modelVFXId;
				this._entityDrawTasks[entityDrawTaskCount].UseDithering = (useDithering ? 1f : 0f);
				this._entityDrawTasks[entityDrawTaskCount].EntityLocalId = (ushort)entityLocalId;
				this._entityDrawTaskCount++;
			}
			int num = packedModelVFXParams >> 3 & 3;
			int entityDistortionDrawTaskCount = this._entityDistortionDrawTaskCount;
			bool flag = modelVFXAnimationProgress != 0f && num == 2;
			if (flag)
			{
				this._entityDistortionDrawTasks[entityDistortionDrawTaskCount].InvModelHeight = 1f / modelHeight;
				this._entityDistortionDrawTasks[entityDistortionDrawTaskCount].ModelMatrix = modelMatrix;
				this._entityDistortionDrawTasks[entityDistortionDrawTaskCount].VertexArray = vertexArray;
				this._entityDistortionDrawTasks[entityDistortionDrawTaskCount].DataCount = dataCount;
				this._entityDistortionDrawTasks[entityDistortionDrawTaskCount].AnimationData = animationData;
				this._entityDistortionDrawTasks[entityDistortionDrawTaskCount].AnimationDataOffset = animationDataOffset;
				this._entityDistortionDrawTasks[entityDistortionDrawTaskCount].AnimationDataSize = animationDataCount * 64;
				this._entityDistortionDrawTasks[entityDistortionDrawTaskCount].ModelVFXAnimationProgress = modelVFXAnimationProgress;
				this._entityDistortionDrawTasks[entityDistortionDrawTaskCount].ModelVFXId = modelVFXId;
				this._entityDistortionDrawTasks[entityDistortionDrawTaskCount].EntityLocalId = (ushort)entityLocalId;
				this._entityDistortionDrawTaskCount++;
			}
		}

		// Token: 0x06005333 RID: 21299 RVA: 0x00177A4C File Offset: 0x00175C4C
		public int RegisterModelVFXTask(float modelVFXAnimationProgress, Vector3 modelVFXHighlightColor, float modelVFXHighlightThickness, Vector2 modelVFXNoiseScale, Vector2 modelVFXNoiseScrollSpeed, int packedModelVFXParams, Vector4 modelVFXPostColor, int entityTaskId = 0, int distortionTaskId = 0, int shadowTaskId = 0)
		{
			int num = -1;
			bool flag = modelVFXAnimationProgress != 0f;
			if (flag)
			{
				num = this._modelVFXDrawTaskCount;
				this._modelVFXDrawTasks[num].ModelVFXHighlightColor = modelVFXHighlightColor;
				this._modelVFXDrawTasks[num].ModelVFXHighlightThickness = modelVFXHighlightThickness;
				this._modelVFXDrawTasks[num].ModelVFXNoiseScale = modelVFXNoiseScale;
				this._modelVFXDrawTasks[num].ModelVFXNoiseScrollSpeed = modelVFXNoiseScrollSpeed;
				this._modelVFXDrawTasks[num].ModelVFXPackedParams = packedModelVFXParams;
				this._modelVFXDrawTasks[num].ModelVFXPostColor = modelVFXPostColor;
				this._modelVFXDrawTaskCount++;
			}
			return num;
		}

		// Token: 0x06005334 RID: 21300 RVA: 0x00177AFC File Offset: 0x00175CFC
		public void RegisterEntityNameplateDrawTask(int entityLocalId, ref Matrix mvpMatrix, Vector3 position, float fillBlurThreshold, GLVertexArray vertexArray, ushort dataCount)
		{
			ArrayUtils.GrowArrayIfNecessary<SceneRenderer.NameplateDrawTask>(ref this._nameplateDrawTasks, this._nameplateDrawTaskCount, 50);
			int nameplateDrawTaskCount = this._nameplateDrawTaskCount;
			this._nameplateDrawTasks[nameplateDrawTaskCount].FillBlurThreshold = fillBlurThreshold;
			this._nameplateDrawTasks[nameplateDrawTaskCount].MVPMatrix = mvpMatrix;
			this._nameplateDrawTasks[nameplateDrawTaskCount].Position = position;
			this._nameplateDrawTasks[nameplateDrawTaskCount].VertexArray = vertexArray;
			this._nameplateDrawTasks[nameplateDrawTaskCount].DataCount = dataCount;
			this._nameplateDrawTasks[nameplateDrawTaskCount].EntityLocalId = (ushort)entityLocalId;
			this._nameplateDrawTaskCount++;
		}

		// Token: 0x06005335 RID: 21301 RVA: 0x00177BA8 File Offset: 0x00175DA8
		public void RegisterEntityDebugDrawTask(bool hit, bool renderCollision, bool collided, int levelOfDetail, ref Matrix lineSightMVPMatrix, ref Matrix headMVPMatrix, ref Matrix boxMVPMatrix, ref Matrix sphereMVPMatrix, ref Matrix boxCollisionMatrix, ref Matrix cylinderCollisionMatrix, ref Matrix lineRepulsionMVPMatrix, SceneRenderer.DebugInfoDetailTask[] detailTasks)
		{
			ArrayUtils.GrowArrayIfNecessary<SceneRenderer.DebugInfoDrawTask>(ref this._debugInfoDrawTasks, this._debugInfoDrawTaskCount, 50);
			int debugInfoDrawTaskCount = this._debugInfoDrawTaskCount;
			this._debugInfoDrawTasks[debugInfoDrawTaskCount].LineSightMVPMatrix = lineSightMVPMatrix;
			this._debugInfoDrawTasks[debugInfoDrawTaskCount].BoxHeadMVPMatrix = headMVPMatrix;
			this._debugInfoDrawTasks[debugInfoDrawTaskCount].BoxMVPMatrix = boxMVPMatrix;
			this._debugInfoDrawTasks[debugInfoDrawTaskCount].SphereMVPMatrix = sphereMVPMatrix;
			this._debugInfoDrawTasks[debugInfoDrawTaskCount].BoxCollisionMatrix = boxCollisionMatrix;
			this._debugInfoDrawTasks[debugInfoDrawTaskCount].CylinderCollisionMatrix = cylinderCollisionMatrix;
			this._debugInfoDrawTasks[debugInfoDrawTaskCount].LineRepulsionMVPMatrix = lineRepulsionMVPMatrix;
			switch (levelOfDetail)
			{
			case 0:
				this._debugInfoDrawTasks[debugInfoDrawTaskCount].SphereColor = new Vector3(1f, 0f, 0f);
				break;
			case 1:
				this._debugInfoDrawTasks[debugInfoDrawTaskCount].SphereColor = new Vector3(0f, 1f, 0f);
				break;
			case 2:
				this._debugInfoDrawTasks[debugInfoDrawTaskCount].SphereColor = new Vector3(0f, 0f, 1f);
				break;
			case 3:
				this._debugInfoDrawTasks[debugInfoDrawTaskCount].SphereColor = new Vector3(1f, 1f, 1f);
				break;
			}
			this._debugInfoDrawTasks[debugInfoDrawTaskCount].Hit = hit;
			this._debugInfoDrawTasks[debugInfoDrawTaskCount].RenderCollision = renderCollision;
			this._debugInfoDrawTasks[debugInfoDrawTaskCount].Collided = collided;
			this._debugInfoDrawTasks[debugInfoDrawTaskCount].DetailTasks = detailTasks;
			this._debugInfoDrawTaskCount++;
		}

		// Token: 0x06005336 RID: 21302 RVA: 0x00177D90 File Offset: 0x00175F90
		public void DrawForwardEntity(Vector2 atlasSizeFactor0, Vector2 atlasSizeFactor1, Vector2 atlasSizeFactor2)
		{
			BlockyModelProgram blockyModelDitheringProgram = this._gpuProgramStore.BlockyModelDitheringProgram;
			blockyModelDitheringProgram.AssertInUse();
			GLFunctions gl = this._graphics.GL;
			blockyModelDitheringProgram.AtlasSizeFactor0.SetValue(atlasSizeFactor0);
			blockyModelDitheringProgram.AtlasSizeFactor1.SetValue(atlasSizeFactor1);
			blockyModelDitheringProgram.AtlasSizeFactor2.SetValue(atlasSizeFactor2);
			for (int i = 0; i < this._entityForwardDrawTaskCount; i++)
			{
				blockyModelDitheringProgram.DrawId.SetValue(0, i + this._entityDrawTaskCount);
				blockyModelDitheringProgram.NodeBlock.SetBufferRange(this._entityForwardDrawTasks[i].AnimationData, this._entityForwardDrawTasks[i].AnimationDataOffset, (uint)this._entityForwardDrawTasks[i].AnimationDataSize);
				gl.BindVertexArray(this._entityForwardDrawTasks[i].VertexArray);
				gl.DrawElements(GL.TRIANGLES, this._entityForwardDrawTasks[i].DataCount, GL.UNSIGNED_SHORT, (IntPtr)0);
			}
		}

		// Token: 0x06005337 RID: 21303 RVA: 0x00177E94 File Offset: 0x00176094
		public void DrawEntityCharactersAndItems(bool useOcclusionCulling = false)
		{
			BlockyModelProgram blockyModelProgram = this._gpuProgramStore.BlockyModelProgram;
			blockyModelProgram.AssertInUse();
			GLFunctions gl = this._graphics.GL;
			int entityOccludeesOffset = this.EntityOccludeesOffset;
			if (useOcclusionCulling)
			{
				for (int i = 0; i < this._entityDrawTaskCount; i++)
				{
					bool flag = this.VisibleOccludees[entityOccludeesOffset + (int)this._entityDrawTasks[i].EntityLocalId] == 1;
					if (flag)
					{
						blockyModelProgram.DrawId.SetValue(0, i);
						blockyModelProgram.NodeBlock.SetBufferRange(this._entityDrawTasks[i].AnimationData, this._entityDrawTasks[i].AnimationDataOffset, (uint)this._entityDrawTasks[i].AnimationDataSize);
						gl.BindVertexArray(this._entityDrawTasks[i].VertexArray);
						gl.DrawElements(GL.TRIANGLES, this._entityDrawTasks[i].DataCount, GL.UNSIGNED_SHORT, (IntPtr)0);
					}
				}
			}
			else
			{
				for (int j = 0; j < this._entityDrawTaskCount; j++)
				{
					blockyModelProgram.DrawId.SetValue(0, j);
					blockyModelProgram.NodeBlock.SetBufferRange(this._entityDrawTasks[j].AnimationData, this._entityDrawTasks[j].AnimationDataOffset, (uint)this._entityDrawTasks[j].AnimationDataSize);
					gl.BindVertexArray(this._entityDrawTasks[j].VertexArray);
					gl.DrawElements(GL.TRIANGLES, this._entityDrawTasks[j].DataCount, GL.UNSIGNED_SHORT, (IntPtr)0);
				}
			}
		}

		// Token: 0x06005338 RID: 21304 RVA: 0x00178068 File Offset: 0x00176268
		public void DrawEntityDistortion(bool useOcclusionCulling = false)
		{
			BlockyModelProgram blockyModelDistortionProgram = this._gpuProgramStore.BlockyModelDistortionProgram;
			blockyModelDistortionProgram.AssertInUse();
			GLFunctions gl = this._graphics.GL;
			int entityOccludeesOffset = this.EntityOccludeesOffset;
			if (useOcclusionCulling)
			{
				for (int i = 0; i < this._entityDistortionDrawTaskCount; i++)
				{
					bool flag = this.VisibleOccludees[entityOccludeesOffset + (int)this._entityDistortionDrawTasks[i].EntityLocalId] == 1;
					if (flag)
					{
						blockyModelDistortionProgram.DrawId.SetValue(this._entityDrawTaskCount + this._entityForwardDrawTaskCount, i);
						blockyModelDistortionProgram.NodeBlock.SetBufferRange(this._entityDistortionDrawTasks[i].AnimationData, this._entityDistortionDrawTasks[i].AnimationDataOffset, (uint)this._entityDistortionDrawTasks[i].AnimationDataSize);
						gl.BindVertexArray(this._entityDistortionDrawTasks[i].VertexArray);
						gl.DrawElements(GL.TRIANGLES, this._entityDistortionDrawTasks[i].DataCount, GL.UNSIGNED_SHORT, (IntPtr)0);
					}
				}
			}
			else
			{
				for (int j = 0; j < this._entityDistortionDrawTaskCount; j++)
				{
					blockyModelDistortionProgram.DrawId.SetValue(this._entityDrawTaskCount + this._entityForwardDrawTaskCount, j);
					blockyModelDistortionProgram.NodeBlock.SetBufferRange(this._entityDistortionDrawTasks[j].AnimationData, this._entityDistortionDrawTasks[j].AnimationDataOffset, (uint)this._entityDistortionDrawTasks[j].AnimationDataSize);
					gl.BindVertexArray(this._entityDistortionDrawTasks[j].VertexArray);
					gl.DrawElements(GL.TRIANGLES, this._entityDistortionDrawTasks[j].DataCount, GL.UNSIGNED_SHORT, (IntPtr)0);
				}
			}
		}

		// Token: 0x06005339 RID: 21305 RVA: 0x00178254 File Offset: 0x00176454
		public void DrawEntityNameplates(bool useOcclusionCulling = false)
		{
			TextProgram textProgram = this._gpuProgramStore.TextProgram;
			textProgram.AssertInUse();
			textProgram.FillThreshold.AssertValue(0f);
			textProgram.OutlineThreshold.AssertValue(0f);
			textProgram.OutlineBlurThreshold.AssertValue(0f);
			textProgram.OutlineOffset.AssertValue(Vector2.Zero);
			textProgram.Opacity.AssertValue(1f);
			GLFunctions gl = this._graphics.GL;
			int entityOccludeesOffset = this.EntityOccludeesOffset;
			if (useOcclusionCulling)
			{
				for (int i = 0; i < this._nameplateDrawTaskCount; i++)
				{
					bool flag = this.VisibleOccludees[entityOccludeesOffset + (int)this._nameplateDrawTasks[i].EntityLocalId] == 1;
					if (flag)
					{
						textProgram.Position.SetValue(this._nameplateDrawTasks[i].Position);
						textProgram.FillBlurThreshold.SetValue(this._nameplateDrawTasks[i].FillBlurThreshold);
						textProgram.MVPMatrix.SetValue(ref this._nameplateDrawTasks[i].MVPMatrix);
						gl.BindVertexArray(this._nameplateDrawTasks[i].VertexArray);
						gl.DrawElements(GL.TRIANGLES, (int)this._nameplateDrawTasks[i].DataCount, GL.UNSIGNED_SHORT, (IntPtr)0);
					}
				}
			}
			else
			{
				for (int j = 0; j < this._nameplateDrawTaskCount; j++)
				{
					textProgram.Position.SetValue(this._nameplateDrawTasks[j].Position);
					textProgram.FillBlurThreshold.SetValue(this._nameplateDrawTasks[j].FillBlurThreshold);
					textProgram.MVPMatrix.SetValue(ref this._nameplateDrawTasks[j].MVPMatrix);
					gl.BindVertexArray(this._nameplateDrawTasks[j].VertexArray);
					gl.DrawElements(GL.TRIANGLES, (int)this._nameplateDrawTasks[j].DataCount, GL.UNSIGNED_SHORT, (IntPtr)0);
				}
			}
		}

		// Token: 0x0600533A RID: 21306 RVA: 0x00178490 File Offset: 0x00176690
		public void DrawEntityDebugInfo()
		{
			for (int i = 0; i < this._debugInfoDrawTaskCount; i++)
			{
				this._lineRenderer.Draw(ref this._debugInfoDrawTasks[i].LineSightMVPMatrix, this._graphics.RedColor, 1f);
				this._lineRenderer.Draw(ref this._debugInfoDrawTasks[i].LineRepulsionMVPMatrix, this._graphics.BlueColor, 1f);
				this._boxRenderer.Draw(ref this._debugInfoDrawTasks[i].BoxHeadMVPMatrix, this._graphics.RedColor, 1f, this._graphics.RedColor, 0.2f);
				Vector3 vector = this._debugInfoDrawTasks[i].Hit ? this._graphics.BlueColor : this._graphics.WhiteColor;
				this._boxRenderer.Draw(ref this._debugInfoDrawTasks[i].BoxMVPMatrix, vector, 1f, vector, 0.2f);
				bool flag = this._debugInfoDrawTasks[i].DetailTasks != null;
				if (flag)
				{
					for (int j = 0; j < this._debugInfoDrawTasks[i].DetailTasks.Length; j++)
					{
						ref SceneRenderer.DebugInfoDetailTask ptr = ref this._debugInfoDrawTasks[i].DetailTasks[j];
						this._boxRenderer.Draw(ref ptr.Matrix, ptr.Color, 1f, ptr.Color, 0.2f);
					}
				}
				this._gpuProgramStore.BasicProgram.MVPMatrix.SetValue(ref this._debugInfoDrawTasks[i].SphereMVPMatrix);
				this._gpuProgramStore.BasicProgram.Opacity.SetValue(0.075f);
				this._gpuProgramStore.BasicProgram.Color.SetValue(this._debugInfoDrawTasks[i].SphereColor);
				this._gl.BindVertexArray(this._sphereLightMesh.VertexArray);
				this._gl.DrawElements(GL.TRIANGLES, this._sphereLightMesh.Count, GL.UNSIGNED_SHORT, (IntPtr)0);
				bool renderCollision = this._debugInfoDrawTasks[i].RenderCollision;
				if (renderCollision)
				{
					Vector3 vector2 = this._debugInfoDrawTasks[i].Collided ? this._graphics.GreenColor : this._graphics.CyanColor;
					this._boxRenderer.Draw(ref this._debugInfoDrawTasks[i].BoxCollisionMatrix, vector2, 2f, vector2, 0.2f);
					this._gpuProgramStore.BasicProgram.MVPMatrix.SetValue(ref this._debugInfoDrawTasks[i].CylinderCollisionMatrix);
					this._gpuProgramStore.BasicProgram.Opacity.SetValue(0.075f);
					this._gpuProgramStore.BasicProgram.Color.SetValue(this._graphics.BlueColor);
					this._gl.BindVertexArray(this._cylinderMesh.VertexArray);
					this._gl.DrawElements(GL.TRIANGLES, this._cylinderMesh.Count, GL.UNSIGNED_SHORT, (IntPtr)0);
				}
			}
		}

		// Token: 0x0600533B RID: 21307 RVA: 0x001787E4 File Offset: 0x001769E4
		private void InitLighting()
		{
			this.SetLinearZForLight(this.UseLinearZForLighting, true);
			MeshProcessor.CreateSphere(ref this._sphereLightMesh, 5, 8, 1f, 0, -1, -1);
			MeshProcessor.CreateCylinder(ref this._cylinderMesh, 8, 1f, 0, -1, -1);
			this.ClassicDeferredLighting = new ClassicDeferredLighting(this._graphics, this._renderTargetStore);
			this.ClassicDeferredLighting.Init();
			this.ClusteredLighting = new ClusteredLighting(this._graphics, this._renderTargetStore, this.Profiling);
			this.ClusteredLighting.Init();
		}

		// Token: 0x0600533C RID: 21308 RVA: 0x00178877 File Offset: 0x00176A77
		private void DisposeLighting()
		{
			this.ClusteredLighting.Dispose();
			this.ClusteredLighting = null;
			this.ClassicDeferredLighting.Dispose();
			this.ClassicDeferredLighting = null;
			this._sphereLightMesh.Dispose();
		}

		// Token: 0x0600533D RID: 21309 RVA: 0x001788AC File Offset: 0x00176AAC
		public void SetupLightRenderingProfiles(int profileLights, int profileLightsFullRes, int profileLightsLowRes, int profileLightsStencil, int profileLightsMix)
		{
			this._renderingProfileLights = profileLights;
			this._renderingProfileLightsFullRes = profileLightsFullRes;
			this._renderingProfileLightsLowRes = profileLightsLowRes;
			this._renderingProfileLightsStencil = profileLightsStencil;
			this._renderingProfileLightsMix = profileLightsMix;
		}

		// Token: 0x0600533E RID: 21310 RVA: 0x001788D4 File Offset: 0x00176AD4
		public void SetupClusteredLightingRenderingProfiles(int profileLightClusterClear, int profileLightClustering, int profileLightClusteringRefine, int profileLightFillGridData, int profileLightSendDataToGPU)
		{
			this.ClusteredLighting.SetupRenderingProfiles(profileLightClusterClear, profileLightClustering, profileLightClusteringRefine, profileLightFillGridData, profileLightSendDataToGPU);
		}

		// Token: 0x0600533F RID: 21311 RVA: 0x001788EC File Offset: 0x00176AEC
		public void SetLinearZForLight(bool enable, bool force = false)
		{
			bool flag = enable != this.UseLinearZForLighting || force;
			if (flag)
			{
				this.UseLinearZForLighting = enable;
				this._graphics.UseLinearZForLight = enable;
				this._graphics.UseLinearZ = enable;
				this._gpuProgramStore.ResetPrograms(true);
			}
		}

		// Token: 0x06005340 RID: 21312 RVA: 0x0017893C File Offset: 0x00176B3C
		public void SetLightBufferCompression(bool enable)
		{
			this._useLBufferCompression = enable;
			RenderTargetStore rtstore = this._graphics.RTStore;
			RenderTargetStore.DebugMapParam.ChromaSubsamplingMode chromaSubsamplingMode = enable ? RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.Light : RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None;
			rtstore.SetDebugMapChromaSubsamplingMode("lbuffer", chromaSubsamplingMode);
			rtstore.SetDebugMapChromaSubsamplingMode("gbuffer1_light", chromaSubsamplingMode);
			this._gpuProgramStore.BlockyModelProgram.UseLightBufferCompression = enable;
			this._gpuProgramStore.FirstPersonBlockyModelProgram.UseLightBufferCompression = enable;
			this._gpuProgramStore.MapChunkAlphaBlendedProgram.UseLightBufferCompression = enable;
			this._gpuProgramStore.MapChunkFarAlphaTestedProgram.UseLightBufferCompression = enable;
			this._gpuProgramStore.MapChunkFarOpaqueProgram.UseLightBufferCompression = enable;
			this._gpuProgramStore.MapChunkNearAlphaTestedProgram.UseLightBufferCompression = enable;
			this._gpuProgramStore.MapChunkNearOpaqueProgram.UseLightBufferCompression = enable;
			this._gpuProgramStore.MapBlockAnimatedProgram.UseLightBufferCompression = enable;
			this._gpuProgramStore.DeferredProgram.UseLightBufferCompression = enable;
			this._gpuProgramStore.LightProgram.UseLightBufferCompression = enable;
			this._gpuProgramStore.LightMixProgram.UseLightBufferCompression = enable;
			this._gpuProgramStore.BlockyModelProgram.Reset(true);
			this._gpuProgramStore.FirstPersonBlockyModelProgram.Reset(true);
			this._gpuProgramStore.MapChunkAlphaBlendedProgram.Reset(true);
			this._gpuProgramStore.MapChunkFarAlphaTestedProgram.Reset(true);
			this._gpuProgramStore.MapChunkFarOpaqueProgram.Reset(true);
			this._gpuProgramStore.MapChunkNearAlphaTestedProgram.Reset(true);
			this._gpuProgramStore.MapChunkNearOpaqueProgram.Reset(true);
			this._gpuProgramStore.MapBlockAnimatedProgram.Reset(true);
			this._gpuProgramStore.DeferredProgram.Reset(true);
			this._gpuProgramStore.LightProgram.Reset(true);
			this._gpuProgramStore.LightMixProgram.Reset(true);
		}

		// Token: 0x06005341 RID: 21313 RVA: 0x00178B00 File Offset: 0x00176D00
		public void SetLightingResolution(SceneRenderer.LightingResolution res)
		{
			this._lightResolution = res;
		}

		// Token: 0x06005342 RID: 21314 RVA: 0x00178B0C File Offset: 0x00176D0C
		private void UpdateLightingHeuristics()
		{
			bool useDynamicLightResolutionSelection = this.UseDynamicLightResolutionSelection;
			if (useDynamicLightResolutionSelection)
			{
				int renderingProfileLights = this._renderingProfileLights;
				float num = this.Profiling.GetGPUMeasure(renderingProfileLights).AccumulatedElapsedTime / (float)this.Profiling.GetMeasureInfo(renderingProfileLights).AccumulatedFrameCount;
				bool flag = num > 10f;
				if (flag)
				{
					this._lightResolution = SceneRenderer.LightingResolution.LOW;
				}
				else
				{
					bool flag2 = num > 5f;
					if (flag2)
					{
						bool flag3 = this._lightResolution == SceneRenderer.LightingResolution.LOW;
						if (flag3)
						{
							bool flag4 = num < 7.5f;
							if (flag4)
							{
								this._lightResolution = SceneRenderer.LightingResolution.MIXED;
							}
						}
						else
						{
							this._lightResolution = SceneRenderer.LightingResolution.MIXED;
						}
					}
					else
					{
						bool flag5 = this._lightResolution == SceneRenderer.LightingResolution.LOW || this._lightResolution == SceneRenderer.LightingResolution.MIXED;
						if (flag5)
						{
							bool flag6 = num < 3.5f;
							if (flag6)
							{
								this._lightResolution = SceneRenderer.LightingResolution.FULL;
							}
						}
						else
						{
							this._lightResolution = SceneRenderer.LightingResolution.FULL;
						}
					}
				}
			}
			bool flag7 = this._lightResolution > SceneRenderer.LightingResolution.FULL;
			if (flag7)
			{
				this._lightBuffer = this._renderTargetStore.LightBufferHalfRes;
			}
			else
			{
				this._lightBuffer = this._renderTargetStore.LightBufferFullRes;
			}
		}

		// Token: 0x06005343 RID: 21315 RVA: 0x00178C24 File Offset: 0x00176E24
		public void PrepareLights(LightData[] lightData, int lightCount)
		{
			this.ClusteredLighting.Prepare(lightData, lightCount, this.Data.WorldFieldOfView, this.Data.CameraPosition, ref this.Data.ViewRotationMatrix, ref this.Data.ProjectionMatrix);
			this.ClusteredLighting.SendDataToGPU();
			bool flag = !this.UseClusteredLighting;
			if (flag)
			{
				this.ClassicDeferredLighting.PrepareLightsForDraw(lightData, lightCount, this.Data.CameraPosition, ref this.Data.ViewRotationMatrix, ref this.Data.InvViewRotationMatrix, true);
				this.ClusteredLighting.SkipMeasures();
			}
		}

		// Token: 0x06005344 RID: 21316 RVA: 0x00178CC4 File Offset: 0x00176EC4
		public void DrawLightPass()
		{
			GLFunctions gl = this._graphics.GL;
			BasicProgram basicProgram = this._gpuProgramStore.BasicProgram;
			int num = this.UseClusteredLighting ? this.ClusteredLighting.LightCount : this.ClassicDeferredLighting.LightCount;
			bool flag = num > 0;
			bool flag2 = this._graphics.UseDeferredLight && flag;
			if (flag2)
			{
				this._gl.StencilMask(255U);
				this._lightBuffer.Bind(this._lightResolution > SceneRenderer.LightingResolution.FULL, true);
				bool flag3 = !this.UseClusteredLighting && this.ClassicDeferredLighting.UseStencilForOuterLights;
				if (flag3)
				{
					this.Profiling.StartMeasure(this._renderingProfileLightsStencil);
					this.ClassicDeferredLighting.TagStencil(32U, ref this.Data.ViewRotationProjectionMatrix);
					this.Profiling.StopMeasure(this._renderingProfileLightsStencil);
				}
				else
				{
					this.Profiling.SkipMeasure(this._renderingProfileLightsStencil);
				}
				int num2 = (this._lightResolution != SceneRenderer.LightingResolution.FULL) ? this._renderingProfileLightsLowRes : this._renderingProfileLightsFullRes;
				this._gl.Enable(GL.BLEND);
				this._gl.BlendFunc(GL.SRC_ALPHA, GL.ONE);
				bool useLightBlendMax = this.UseLightBlendMax;
				if (useLightBlendMax)
				{
					this._gl.BlendEquationSeparate(GL.MAX, GL.MAX);
				}
				else
				{
					this._gl.BlendEquationSeparate(GL.FUNC_ADD, GL.FUNC_ADD);
				}
				bool blue = this._lightResolution != SceneRenderer.LightingResolution.FULL || !this._useLBufferCompression;
				this._gl.ColorMask(true, true, blue, false);
				this.Profiling.StartMeasure(num2);
				bool fullResolution = this._lightResolution == SceneRenderer.LightingResolution.FULL;
				bool useClusteredLighting = this.UseClusteredLighting;
				if (useClusteredLighting)
				{
					this.ClusteredLighting.DrawDeferredLights(this.Data.FrustumFarCornersVS, ref this.Data.ProjectionMatrix, fullResolution, false);
				}
				else
				{
					this.ClassicDeferredLighting.DrawDeferredLights(fullResolution, this.ClassicDeferredLighting.UseStencilForOuterLights, ref this.Data.ViewRotationMatrix, ref this.Data.ProjectionMatrix, 1024f);
				}
				this.Profiling.StopMeasure(num2);
				bool flag4 = num2 == this._renderingProfileLightsFullRes;
				if (flag4)
				{
					this.Profiling.SkipMeasure(this._renderingProfileLightsLowRes);
					this.Profiling.SkipMeasure(this._renderingProfileLightsMix);
				}
				this._lightBuffer.Unbind();
				bool flag5 = this._lightResolution > SceneRenderer.LightingResolution.FULL;
				if (flag5)
				{
					this._renderTargetStore.LightBufferFullRes.Bind(false, true);
					this.Profiling.StartMeasure(this._renderingProfileLightsMix);
					this._gl.ColorMask(true, true, !this._useLBufferCompression, false);
					this._gl.StencilFunc(GL.NOTEQUAL, 128, 128U);
					this._gl.StencilMask(0U);
					this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.LightBufferHalfRes.GetTexture(RenderTarget.Target.Color0));
					LightMixProgram lightMixProgram = this._gpuProgramStore.LightMixProgram;
					this._gl.UseProgram(lightMixProgram);
					this._graphics.ScreenTriangleRenderer.Draw();
					this.Profiling.StopMeasure(this._renderingProfileLightsMix);
					bool flag6 = this._lightResolution == SceneRenderer.LightingResolution.MIXED;
					if (flag6)
					{
						this.Profiling.StartMeasure(this._renderingProfileLightsFullRes);
						this._gl.StencilFunc(GL.EQUAL, 128, 128U);
						bool useClusteredLighting2 = this.UseClusteredLighting;
						if (useClusteredLighting2)
						{
							this.ClusteredLighting.DrawDeferredLights(this.Data.FrustumFarCornersVS, ref this.Data.ProjectionMatrix, true, true);
						}
						else
						{
							this.ClassicDeferredLighting.DrawDeferredLights(true, false, ref this.Data.ViewRotationMatrix, ref this.Data.ProjectionMatrix, 1024f);
						}
						this.Profiling.StopMeasure(this._renderingProfileLightsFullRes);
					}
					else
					{
						this.Profiling.SkipMeasure(this._renderingProfileLightsFullRes);
					}
					this._renderTargetStore.LightBufferFullRes.Unbind();
				}
				bool useLightBlendMax2 = this.UseLightBlendMax;
				if (useLightBlendMax2)
				{
					this._gl.BlendEquationSeparate(GL.FUNC_ADD, GL.FUNC_ADD);
				}
				this._gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
				this._gl.Disable(GL.BLEND);
				this._gl.Disable(GL.STENCIL_TEST);
				this._gl.Enable(GL.DEPTH_TEST);
			}
			else
			{
				this.Profiling.SkipMeasure(this._renderingProfileLightsStencil);
				this.Profiling.SkipMeasure(this._renderingProfileLightsFullRes);
				this.Profiling.SkipMeasure(this._renderingProfileLightsLowRes);
				this.Profiling.SkipMeasure(this._renderingProfileLightsMix);
			}
			this._gl.ColorMask(true, true, true, true);
		}

		// Token: 0x06005345 RID: 21317 RVA: 0x001791C0 File Offset: 0x001773C0
		public void DebugDrawLights(LightData[] lightData, int lightCount)
		{
			this._gl.AssertActiveTexture(GL.TEXTURE0);
			this._gl.BindTexture(GL.TEXTURE_2D, this._graphics.WhitePixelTexture.GLTexture);
			this._gl.BindVertexArray(this._sphereLightMesh.VertexArray);
			BasicProgram basicProgram = this._gpuProgramStore.BasicProgram;
			this._gl.UseProgram(basicProgram);
			basicProgram.Opacity.SetValue(1f);
			for (int i = 0; i < lightCount; i++)
			{
				float radius = lightData[i].Sphere.Radius;
				Vector3 vector = lightData[i].Sphere.Center;
				BoundingSphere boundingSphere;
				boundingSphere.Center = vector;
				boundingSphere.Radius = radius;
				vector -= this.Data.CameraPosition;
				Matrix matrix;
				Matrix.CreateScale(radius, out matrix);
				Matrix.AddTranslation(ref matrix, vector.X, vector.Y, vector.Z);
				Matrix.Multiply(ref matrix, ref this.Data.ViewRotationProjectionMatrix, out matrix);
				Vector3 value = (boundingSphere.Contains(this.Data.CameraPosition) > ContainmentType.Disjoint) ? new Vector3(0f, 1f, 0f) : new Vector3(1f, 0f, 0f);
				basicProgram.Color.SetValue(value);
				basicProgram.MVPMatrix.SetValue(ref matrix);
				this._gl.DrawElements(GL.TRIANGLES, this._sphereLightMesh.Count, GL.UNSIGNED_SHORT, (IntPtr)0);
			}
		}

		// Token: 0x06005346 RID: 21318 RVA: 0x00179370 File Offset: 0x00177570
		public void ComputeNearChunkDistance(float fieldOfView)
		{
			float num = MathHelper.Clamp(70f / fieldOfView, 1f, 2f);
			this._nearChunkDistance = 64f * num;
		}

		// Token: 0x06005347 RID: 21319 RVA: 0x001793A4 File Offset: 0x001775A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool IsNear(float distance)
		{
			return distance < this._nearChunkDistance;
		}

		// Token: 0x06005348 RID: 21320 RVA: 0x001793C0 File Offset: 0x001775C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool MapBlocksAnimatedNeedDrawing()
		{
			return this._animatedBlockDrawTaskCount > 0;
		}

		// Token: 0x06005349 RID: 21321 RVA: 0x001793DB File Offset: 0x001775DB
		private void ResetMapCounters()
		{
			this._animatedBlockDrawTaskCount = 0;
			this._opaqueDrawTaskCount = 0;
			this._alphaTestedDrawTaskCount = 0;
			this._alphaBlendedDrawTaskCount = 0;
			this._farProgramOpaqueChunkStartIndex = 0;
			this._nearProgramAlphaBlendedChunkStartIndex = 0;
			this._farProgramAlphaTestedChunkStartIndex = 0;
		}

		// Token: 0x0600534A RID: 21322 RVA: 0x0017940F File Offset: 0x0017760F
		public void PrepareForIncomingMapChunkDrawTasks(int opaqueCount, int alphaTestedCount, int alphaBlendedCount)
		{
			ArrayUtils.GrowArrayIfNecessary<SceneRenderer.ChunkDrawTask>(ref this._opaqueDrawTasks, opaqueCount, 100);
			ArrayUtils.GrowArrayIfNecessary<SceneRenderer.ChunkDrawTask>(ref this._alphaTestedDrawTasks, alphaTestedCount, 100);
			ArrayUtils.GrowArrayIfNecessary<SceneRenderer.ChunkDrawTask>(ref this._alphaBlendedDrawTasks, alphaBlendedCount, 50);
			this.PrepareForIncomingChunkOccludees(opaqueCount, alphaTestedCount, alphaBlendedCount);
		}

		// Token: 0x0600534B RID: 21323 RVA: 0x0017944C File Offset: 0x0017764C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RegisterMapChunkOpaqueDrawTask(ref Matrix modelMatrix, GLVertexArray vertexArray, int indicesCount, bool isNear)
		{
			this._opaqueDrawTasks[this._opaqueDrawTaskCount].VertexArray = vertexArray;
			this._opaqueDrawTasks[this._opaqueDrawTaskCount].DataOffset = IntPtr.Zero;
			this._opaqueDrawTasks[this._opaqueDrawTaskCount].DataCount = indicesCount;
			this._opaqueDrawTasks[this._opaqueDrawTaskCount].ModelMatrix = modelMatrix;
			this._opaqueDrawTaskCount++;
			this.RegisterOccludeeChunkOpaque(modelMatrix.Translation);
			if (isNear)
			{
				this._farProgramOpaqueChunkStartIndex = this._opaqueDrawTaskCount;
			}
		}

		// Token: 0x0600534C RID: 21324 RVA: 0x001794EC File Offset: 0x001776EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RegisterMapChunkAlphaTestedDrawTask(ref Matrix modelMatrix, GLVertexArray vertexArray, IntPtr offset, int indicesCount, bool isNear)
		{
			this._alphaTestedDrawTasks[this._alphaTestedDrawTaskCount].VertexArray = vertexArray;
			this._alphaTestedDrawTasks[this._alphaTestedDrawTaskCount].DataOffset = offset;
			this._alphaTestedDrawTasks[this._alphaTestedDrawTaskCount].DataCount = indicesCount;
			this._alphaTestedDrawTasks[this._alphaTestedDrawTaskCount].ModelMatrix = modelMatrix;
			this._alphaTestedDrawTaskCount++;
			this.RegisterOccludeeChunkAlphaTested(modelMatrix.Translation);
			if (isNear)
			{
				this._farProgramAlphaTestedChunkStartIndex = this._alphaTestedDrawTaskCount;
			}
		}

		// Token: 0x0600534D RID: 21325 RVA: 0x0017958C File Offset: 0x0017778C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RegisterMapChunkAlphaBlendedDrawTask(ref Matrix modelMatrix, GLVertexArray vertexArray, int indicesCount, bool isNear)
		{
			this._alphaBlendedDrawTasks[this._alphaBlendedDrawTaskCount].VertexArray = vertexArray;
			this._alphaBlendedDrawTasks[this._alphaBlendedDrawTaskCount].DataOffset = IntPtr.Zero;
			this._alphaBlendedDrawTasks[this._alphaBlendedDrawTaskCount].DataCount = indicesCount;
			this._alphaBlendedDrawTasks[this._alphaBlendedDrawTaskCount].ModelMatrix = modelMatrix;
			this._alphaBlendedDrawTaskCount++;
			this.RegisterOccludeeChunkAlphaBlended(modelMatrix.Translation);
			if (isNear)
			{
				this._nearProgramAlphaBlendedChunkStartIndex = this._alphaBlendedDrawTaskCount;
			}
		}

		// Token: 0x0600534E RID: 21326 RVA: 0x0017962C File Offset: 0x0017782C
		public void PrepareForIncomingMapBlockAnimatedDrawTasks(int count)
		{
			ArrayUtils.GrowArrayIfNecessary<SceneRenderer.AnimatedBlockDrawTask>(ref this._animatedBlockDrawTasks, this._animatedBlockDrawTaskCount + count, 25);
		}

		// Token: 0x0600534F RID: 21327 RVA: 0x00179648 File Offset: 0x00177848
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RegisterMapBlockAnimatedDrawTask(ref Matrix modelMatrix, GLVertexArray vertexArray, int indicesCount, GLBuffer animationData, uint animationDataOffset, uint animationDataCount)
		{
			this._animatedBlockDrawTasks[this._animatedBlockDrawTaskCount].VertexArray = vertexArray;
			this._animatedBlockDrawTasks[this._animatedBlockDrawTaskCount].DataCount = indicesCount;
			this._animatedBlockDrawTasks[this._animatedBlockDrawTaskCount].AnimationData = animationData;
			this._animatedBlockDrawTasks[this._animatedBlockDrawTaskCount].AnimationDataOffset = animationDataOffset;
			this._animatedBlockDrawTasks[this._animatedBlockDrawTaskCount].AnimationDataSize = (ushort)(animationDataCount * 64U);
			this._animatedBlockDrawTasks[this._animatedBlockDrawTaskCount].ModelMatrix = modelMatrix;
			this._animatedBlockDrawTaskCount++;
		}

		// Token: 0x06005350 RID: 21328 RVA: 0x001796FC File Offset: 0x001778FC
		public void DrawMapChunksOpaque(bool nearChunks, bool useOcclusionCulling)
		{
			GLFunctions gl = this._graphics.GL;
			if (useOcclusionCulling)
			{
				if (nearChunks)
				{
					MapChunkBaseProgram mapChunkNearOpaqueProgram = this._gpuProgramStore.MapChunkNearOpaqueProgram;
					mapChunkNearOpaqueProgram.AssertInUse();
					for (int i = 0; i < this._farProgramOpaqueChunkStartIndex; i++)
					{
						bool flag = this.VisibleOccludees[i] == 1;
						if (flag)
						{
							mapChunkNearOpaqueProgram.ModelMatrix.SetValue(ref this._opaqueDrawTasks[i].ModelMatrix);
							gl.BindVertexArray(this._opaqueDrawTasks[i].VertexArray);
							gl.DrawElements(GL.TRIANGLES, this._opaqueDrawTasks[i].DataCount, GL.UNSIGNED_INT, this._opaqueDrawTasks[i].DataOffset);
						}
					}
				}
				else
				{
					MapChunkBaseProgram mapChunkFarOpaqueProgram = this._gpuProgramStore.MapChunkFarOpaqueProgram;
					mapChunkFarOpaqueProgram.AssertInUse();
					for (int j = this._farProgramOpaqueChunkStartIndex; j < this._opaqueDrawTaskCount; j++)
					{
						bool flag2 = this.VisibleOccludees[j] == 1;
						if (flag2)
						{
							mapChunkFarOpaqueProgram.ModelMatrix.SetValue(ref this._opaqueDrawTasks[j].ModelMatrix);
							gl.BindVertexArray(this._opaqueDrawTasks[j].VertexArray);
							gl.DrawElements(GL.TRIANGLES, this._opaqueDrawTasks[j].DataCount, GL.UNSIGNED_INT, this._opaqueDrawTasks[j].DataOffset);
						}
					}
				}
			}
			else if (nearChunks)
			{
				MapChunkBaseProgram mapChunkNearOpaqueProgram2 = this._gpuProgramStore.MapChunkNearOpaqueProgram;
				mapChunkNearOpaqueProgram2.AssertInUse();
				for (int k = 0; k < this._farProgramOpaqueChunkStartIndex; k++)
				{
					mapChunkNearOpaqueProgram2.ModelMatrix.SetValue(ref this._opaqueDrawTasks[k].ModelMatrix);
					gl.BindVertexArray(this._opaqueDrawTasks[k].VertexArray);
					gl.DrawElements(GL.TRIANGLES, this._opaqueDrawTasks[k].DataCount, GL.UNSIGNED_INT, this._opaqueDrawTasks[k].DataOffset);
				}
			}
			else
			{
				MapChunkBaseProgram mapChunkFarOpaqueProgram2 = this._gpuProgramStore.MapChunkFarOpaqueProgram;
				mapChunkFarOpaqueProgram2.AssertInUse();
				for (int l = this._farProgramOpaqueChunkStartIndex; l < this._opaqueDrawTaskCount; l++)
				{
					mapChunkFarOpaqueProgram2.ModelMatrix.SetValue(ref this._opaqueDrawTasks[l].ModelMatrix);
					gl.BindVertexArray(this._opaqueDrawTasks[l].VertexArray);
					gl.DrawElements(GL.TRIANGLES, this._opaqueDrawTasks[l].DataCount, GL.UNSIGNED_INT, this._opaqueDrawTasks[l].DataOffset);
				}
			}
		}

		// Token: 0x06005351 RID: 21329 RVA: 0x001799F8 File Offset: 0x00177BF8
		public void DrawMapChunksAlphaTested(bool nearChunks, bool useOcclusionCulling)
		{
			GLFunctions gl = this._graphics.GL;
			int alphaTestedChunkOccludeesOffset = this.AlphaTestedChunkOccludeesOffset;
			if (useOcclusionCulling)
			{
				if (nearChunks)
				{
					MapChunkBaseProgram mapChunkNearAlphaTestedProgram = this._gpuProgramStore.MapChunkNearAlphaTestedProgram;
					mapChunkNearAlphaTestedProgram.AssertInUse();
					for (int i = 0; i < this._farProgramAlphaTestedChunkStartIndex; i++)
					{
						bool flag = this.VisibleOccludees[i + alphaTestedChunkOccludeesOffset] == 1;
						if (flag)
						{
							mapChunkNearAlphaTestedProgram.ModelMatrix.SetValue(ref this._alphaTestedDrawTasks[i].ModelMatrix);
							gl.BindVertexArray(this._alphaTestedDrawTasks[i].VertexArray);
							gl.DrawElements(GL.TRIANGLES, this._alphaTestedDrawTasks[i].DataCount, GL.UNSIGNED_INT, this._alphaTestedDrawTasks[i].DataOffset);
						}
					}
				}
				else
				{
					MapChunkBaseProgram mapChunkFarAlphaTestedProgram = this._gpuProgramStore.MapChunkFarAlphaTestedProgram;
					mapChunkFarAlphaTestedProgram.AssertInUse();
					for (int j = this._farProgramAlphaTestedChunkStartIndex; j < this._alphaTestedDrawTaskCount; j++)
					{
						bool flag2 = this.VisibleOccludees[j + alphaTestedChunkOccludeesOffset] == 1;
						if (flag2)
						{
							mapChunkFarAlphaTestedProgram.ModelMatrix.SetValue(ref this._alphaTestedDrawTasks[j].ModelMatrix);
							gl.BindVertexArray(this._alphaTestedDrawTasks[j].VertexArray);
							gl.DrawElements(GL.TRIANGLES, this._alphaTestedDrawTasks[j].DataCount, GL.UNSIGNED_INT, this._alphaTestedDrawTasks[j].DataOffset);
						}
					}
				}
			}
			else if (nearChunks)
			{
				MapChunkBaseProgram mapChunkNearAlphaTestedProgram2 = this._gpuProgramStore.MapChunkNearAlphaTestedProgram;
				mapChunkNearAlphaTestedProgram2.AssertInUse();
				for (int k = 0; k < this._farProgramAlphaTestedChunkStartIndex; k++)
				{
					mapChunkNearAlphaTestedProgram2.ModelMatrix.SetValue(ref this._alphaTestedDrawTasks[k].ModelMatrix);
					gl.BindVertexArray(this._alphaTestedDrawTasks[k].VertexArray);
					gl.DrawElements(GL.TRIANGLES, this._alphaTestedDrawTasks[k].DataCount, GL.UNSIGNED_INT, this._alphaTestedDrawTasks[k].DataOffset);
				}
			}
			else
			{
				MapChunkBaseProgram mapChunkFarAlphaTestedProgram2 = this._gpuProgramStore.MapChunkFarAlphaTestedProgram;
				mapChunkFarAlphaTestedProgram2.AssertInUse();
				for (int l = this._farProgramAlphaTestedChunkStartIndex; l < this._alphaTestedDrawTaskCount; l++)
				{
					mapChunkFarAlphaTestedProgram2.ModelMatrix.SetValue(ref this._alphaTestedDrawTasks[l].ModelMatrix);
					gl.BindVertexArray(this._alphaTestedDrawTasks[l].VertexArray);
					gl.DrawElements(GL.TRIANGLES, this._alphaTestedDrawTasks[l].DataCount, GL.UNSIGNED_INT, this._alphaTestedDrawTasks[l].DataOffset);
				}
			}
		}

		// Token: 0x06005352 RID: 21330 RVA: 0x00179D04 File Offset: 0x00177F04
		public void DrawMapChunksAlphaBlended(bool useOcclusionCulling)
		{
			MapChunkBaseProgram mapChunkAlphaBlendedProgram = this._gpuProgramStore.MapChunkAlphaBlendedProgram;
			mapChunkAlphaBlendedProgram.AssertInUse();
			GLFunctions gl = this._graphics.GL;
			int alphaBlendedOccludeesOffset = this.AlphaBlendedOccludeesOffset;
			int num = this._nearProgramAlphaBlendedChunkStartIndex - 1;
			Debug.Assert(num < this._alphaBlendedDrawTaskCount);
			if (useOcclusionCulling)
			{
				for (int i = this._alphaBlendedDrawTaskCount - 1; i > num; i--)
				{
					bool flag = this.VisibleOccludees[i + alphaBlendedOccludeesOffset] == 1;
					if (flag)
					{
						mapChunkAlphaBlendedProgram.ModelMatrix.SetValue(ref this._alphaBlendedDrawTasks[i].ModelMatrix);
						gl.BindVertexArray(this._alphaBlendedDrawTasks[i].VertexArray);
						gl.DrawElements(GL.TRIANGLES, this._alphaBlendedDrawTasks[i].DataCount, GL.UNSIGNED_INT, this._alphaBlendedDrawTasks[i].DataOffset);
					}
				}
				for (int j = num; j >= 0; j--)
				{
					bool flag2 = this.VisibleOccludees[j + alphaBlendedOccludeesOffset] == 1;
					if (flag2)
					{
						mapChunkAlphaBlendedProgram.ModelMatrix.SetValue(ref this._alphaBlendedDrawTasks[j].ModelMatrix);
						gl.BindVertexArray(this._alphaBlendedDrawTasks[j].VertexArray);
						gl.DrawElements(GL.TRIANGLES, this._alphaBlendedDrawTasks[j].DataCount, GL.UNSIGNED_INT, this._alphaBlendedDrawTasks[j].DataOffset);
					}
				}
			}
			else
			{
				for (int k = this._alphaBlendedDrawTaskCount - 1; k > num; k--)
				{
					mapChunkAlphaBlendedProgram.ModelMatrix.SetValue(ref this._alphaBlendedDrawTasks[k].ModelMatrix);
					gl.BindVertexArray(this._alphaBlendedDrawTasks[k].VertexArray);
					gl.DrawElements(GL.TRIANGLES, this._alphaBlendedDrawTasks[k].DataCount, GL.UNSIGNED_INT, this._alphaBlendedDrawTasks[k].DataOffset);
				}
				for (int l = num; l >= 0; l--)
				{
					mapChunkAlphaBlendedProgram.ModelMatrix.SetValue(ref this._alphaBlendedDrawTasks[l].ModelMatrix);
					gl.BindVertexArray(this._alphaBlendedDrawTasks[l].VertexArray);
					gl.DrawElements(GL.TRIANGLES, this._alphaBlendedDrawTasks[l].DataCount, GL.UNSIGNED_INT, this._alphaBlendedDrawTasks[l].DataOffset);
				}
			}
		}

		// Token: 0x06005353 RID: 21331 RVA: 0x00179FB0 File Offset: 0x001781B0
		public void DrawMapBlocksAnimated()
		{
			MapBlockAnimatedProgram mapBlockAnimatedProgram = this._gpuProgramStore.MapBlockAnimatedProgram;
			mapBlockAnimatedProgram.AssertInUse();
			GLFunctions gl = this._graphics.GL;
			for (int i = 0; i < this._animatedBlockDrawTaskCount; i++)
			{
				mapBlockAnimatedProgram.ModelMatrix.SetValue(ref this._animatedBlockDrawTasks[i].ModelMatrix);
				mapBlockAnimatedProgram.NodeBlock.SetBufferRange(this._animatedBlockDrawTasks[i].AnimationData, this._animatedBlockDrawTasks[i].AnimationDataOffset, (uint)this._animatedBlockDrawTasks[i].AnimationDataSize);
				gl.BindVertexArray(this._animatedBlockDrawTasks[i].VertexArray);
				gl.DrawElements(GL.TRIANGLES, this._animatedBlockDrawTasks[i].DataCount, GL.UNSIGNED_INT, (IntPtr)0);
			}
		}

		// Token: 0x170012DC RID: 4828
		// (get) Token: 0x06005354 RID: 21332 RVA: 0x0017A092 File Offset: 0x00178292
		public int ChunkOccludeesOffset
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170012DD RID: 4829
		// (get) Token: 0x06005355 RID: 21333 RVA: 0x0017A095 File Offset: 0x00178295
		public int OpaqueChunkOccludeesOffset
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170012DE RID: 4830
		// (get) Token: 0x06005356 RID: 21334 RVA: 0x0017A098 File Offset: 0x00178298
		public int AlphaTestedChunkOccludeesOffset
		{
			get
			{
				return this._opaqueOccludeesCount;
			}
		}

		// Token: 0x170012DF RID: 4831
		// (get) Token: 0x06005357 RID: 21335 RVA: 0x0017A0A0 File Offset: 0x001782A0
		public int AlphaBlendedOccludeesOffset
		{
			get
			{
				return this._opaqueOccludeesCount + this._alphaTestedOccludeesCount;
			}
		}

		// Token: 0x170012E0 RID: 4832
		// (get) Token: 0x06005358 RID: 21336 RVA: 0x0017A0AF File Offset: 0x001782AF
		public int EntityOccludeesOffset
		{
			get
			{
				return this._opaqueOccludeesCount + this._alphaTestedOccludeesCount + this._alphaBlendedOccludeesCount;
			}
		}

		// Token: 0x170012E1 RID: 4833
		// (get) Token: 0x06005359 RID: 21337 RVA: 0x0017A0C5 File Offset: 0x001782C5
		public int LightOccludeesOffset
		{
			get
			{
				return this._opaqueOccludeesCount + this._alphaTestedOccludeesCount + this._alphaBlendedOccludeesCount + this._entitiesOccludeesCount;
			}
		}

		// Token: 0x170012E2 RID: 4834
		// (get) Token: 0x0600535A RID: 21338 RVA: 0x0017A0E2 File Offset: 0x001782E2
		public int ParticleOccludeesOffset
		{
			get
			{
				return this._opaqueOccludeesCount + this._alphaTestedOccludeesCount + this._alphaBlendedOccludeesCount + this._entitiesOccludeesCount + this._lightOccludeesCount;
			}
		}

		// Token: 0x170012E3 RID: 4835
		// (get) Token: 0x0600535B RID: 21339 RVA: 0x0017A106 File Offset: 0x00178306
		public int ChunkOccludeesCount
		{
			get
			{
				return this._opaqueOccludeesCount + this._alphaTestedOccludeesCount + this._alphaBlendedOccludeesCount;
			}
		}

		// Token: 0x170012E4 RID: 4836
		// (get) Token: 0x0600535C RID: 21340 RVA: 0x0017A11C File Offset: 0x0017831C
		public int OpaqueChunkOccludeesCount
		{
			get
			{
				return this._opaqueOccludeesCount;
			}
		}

		// Token: 0x170012E5 RID: 4837
		// (get) Token: 0x0600535D RID: 21341 RVA: 0x0017A124 File Offset: 0x00178324
		public int AlphaTestedChunkOccludeesCount
		{
			get
			{
				return this._alphaTestedOccludeesCount;
			}
		}

		// Token: 0x170012E6 RID: 4838
		// (get) Token: 0x0600535E RID: 21342 RVA: 0x0017A12C File Offset: 0x0017832C
		public int AlphaBlendedOccludeesCount
		{
			get
			{
				return this._alphaBlendedOccludeesCount;
			}
		}

		// Token: 0x170012E7 RID: 4839
		// (get) Token: 0x0600535F RID: 21343 RVA: 0x0017A134 File Offset: 0x00178334
		public int EntityOccludeesCount
		{
			get
			{
				return this._entitiesOccludeesCount;
			}
		}

		// Token: 0x170012E8 RID: 4840
		// (get) Token: 0x06005360 RID: 21344 RVA: 0x0017A13C File Offset: 0x0017833C
		public int LightOccludeesCount
		{
			get
			{
				return this._lightOccludeesCount;
			}
		}

		// Token: 0x170012E9 RID: 4841
		// (get) Token: 0x06005361 RID: 21345 RVA: 0x0017A144 File Offset: 0x00178344
		public int ParticleOccludeesCount
		{
			get
			{
				return this._particleOccludeesCount;
			}
		}

		// Token: 0x06005362 RID: 21346 RVA: 0x0017A14C File Offset: 0x0017834C
		private void InitOcclusionCulling()
		{
			this.CreateOcclusionCullingGPUData();
		}

		// Token: 0x06005363 RID: 21347 RVA: 0x0017A156 File Offset: 0x00178356
		private void DisposeOcclusionCulling()
		{
			this.DestroyOcclusionCullingGPUData();
		}

		// Token: 0x06005364 RID: 21348 RVA: 0x0017A160 File Offset: 0x00178360
		private unsafe void CreateOcclusionCullingGPUData()
		{
			GLFunctions gl = this._graphics.GL;
			int num = 4000;
			int num2 = 4 * num;
			int num3 = 16;
			int num4 = 6 * num;
			ushort[] array = new ushort[num4];
			for (int i = 0; i < num; i++)
			{
				array[i * 6] = (ushort)(i * 4);
				array[i * 6 + 1] = (ushort)(i * 4 + 1);
				array[i * 6 + 2] = (ushort)(i * 4 + 2);
				array[i * 6 + 3] = (ushort)(i * 4);
				array[i * 6 + 4] = (ushort)(i * 4 + 2);
				array[i * 6 + 5] = (ushort)(i * 4 + 3);
			}
			this._occluderPlanesVertexArray = gl.GenVertexArray();
			gl.BindVertexArray(this._occluderPlanesVertexArray);
			this._occluderPlanesVerticesBuffer = gl.GenBuffer();
			gl.BindBuffer(this._occluderPlanesVertexArray, GL.ARRAY_BUFFER, this._occluderPlanesVerticesBuffer);
			gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(num2 * num3), IntPtr.Zero, GL.DYNAMIC_DRAW);
			this._occluderPlanesIndicesBuffer = gl.GenBuffer();
			gl.BindBuffer(this._occluderPlanesVertexArray, GL.ELEMENT_ARRAY_BUFFER, this._occluderPlanesIndicesBuffer);
			ushort[] array2;
			ushort* value;
			if ((array2 = array) == null || array2.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array2[0];
			}
			gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, (IntPtr)(num4 * 2), (IntPtr)((void*)value), GL.STATIC_DRAW);
			array2 = null;
			gl.EnableVertexAttribArray(0U);
			gl.VertexAttribPointer(0U, 4, GL.FLOAT, false, num3, IntPtr.Zero);
		}

		// Token: 0x06005365 RID: 21349 RVA: 0x0017A2F8 File Offset: 0x001784F8
		private void DestroyOcclusionCullingGPUData()
		{
			GLFunctions gl = this._graphics.GL;
			gl.DeleteBuffer(this._occluderPlanesVerticesBuffer);
			gl.DeleteBuffer(this._occluderPlanesIndicesBuffer);
			gl.DeleteVertexArray(this._occluderPlanesVertexArray);
		}

		// Token: 0x06005366 RID: 21350 RVA: 0x0017A339 File Offset: 0x00178539
		private void ResetOcclusionCullingCounters()
		{
			this._opaqueOccludersCount = 0;
			this._occluderPlanesCount = 0;
			this._opaqueOccludeesCount = 0;
			this._alphaTestedOccludeesCount = 0;
			this._alphaBlendedOccludeesCount = 0;
			this._entitiesOccludeesCount = 0;
			this._lightOccludeesCount = 0;
			this._particleOccludeesCount = 0;
		}

		// Token: 0x06005367 RID: 21351 RVA: 0x0017A374 File Offset: 0x00178574
		public void GatherOcclusionCullingStats(out int occludedCount, out int occludedTrianglesCount)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < this._opaqueDrawTaskCount; i++)
			{
				bool flag = this.VisibleOccludees[i] == 0;
				if (flag)
				{
					num++;
					num2 += this._opaqueDrawTasks[i].DataCount;
				}
			}
			int num3 = this._opaqueDrawTaskCount;
			for (int j = 0; j < this._alphaTestedDrawTaskCount; j++)
			{
				bool flag2 = this.VisibleOccludees[j + num3] == 0;
				if (flag2)
				{
					num++;
					num2 += this._alphaTestedDrawTasks[j].DataCount;
				}
			}
			num3 += this._alphaTestedDrawTaskCount;
			for (int k = 0; k < this._alphaBlendedDrawTaskCount; k++)
			{
				bool flag3 = this.VisibleOccludees[k + num3] == 0;
				if (flag3)
				{
					num++;
					num2 += this._alphaBlendedDrawTasks[k].DataCount;
				}
			}
			occludedCount = num;
			occludedTrianglesCount = num2 / 3;
		}

		// Token: 0x06005368 RID: 21352 RVA: 0x0017A47A File Offset: 0x0017867A
		public void PrepareForIncomingChunkOccludees(int opaqueCount, int alphaTestedCount, int alphaBlendedCount)
		{
			ArrayUtils.GrowArrayIfNecessary<Vector3>(ref this._opaqueOccludeesData, opaqueCount, 100);
			ArrayUtils.GrowArrayIfNecessary<Vector3>(ref this._alphaTestedOccludeesData, alphaTestedCount, 100);
			ArrayUtils.GrowArrayIfNecessary<Vector3>(ref this._alphaBlendedOccludeesData, alphaBlendedCount, 50);
		}

		// Token: 0x06005369 RID: 21353 RVA: 0x0017A4AA File Offset: 0x001786AA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RegisterOccludeeChunkOpaque(Vector3 position)
		{
			this._opaqueOccludeesData[this._opaqueOccludeesCount] = position;
			this._opaqueOccludeesCount++;
		}

		// Token: 0x0600536A RID: 21354 RVA: 0x0017A4CD File Offset: 0x001786CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RegisterOccludeeChunkAlphaTested(Vector3 position)
		{
			this._alphaTestedOccludeesData[this._alphaTestedOccludeesCount] = position;
			this._alphaTestedOccludeesCount++;
		}

		// Token: 0x0600536B RID: 21355 RVA: 0x0017A4F0 File Offset: 0x001786F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RegisterOccludeeChunkAlphaBlended(Vector3 position)
		{
			this._alphaBlendedOccludeesData[this._alphaBlendedOccludeesCount] = position;
			this._alphaBlendedOccludeesCount++;
		}

		// Token: 0x0600536C RID: 21356 RVA: 0x0017A513 File Offset: 0x00178713
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PrepareForIncomingChunkOccluderPlane(int count)
		{
			ArrayUtils.GrowArrayIfNecessary<Vector4>(ref this._occluderPlanes, this._occluderPlanesCount + 4 * count, 1000);
		}

		// Token: 0x0600536D RID: 21357 RVA: 0x0017A534 File Offset: 0x00178734
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RegisterChunkOccluderPlane(Vector3 position, float minSolidPlaneY)
		{
			this._occluderPlanes[this._occluderPlanesCount] = new Vector4(position.X, position.Y, position.Z, minSolidPlaneY);
			this._occluderPlanes[this._occluderPlanesCount + 1] = new Vector4(position.X, position.Y, position.Z, minSolidPlaneY);
			this._occluderPlanes[this._occluderPlanesCount + 2] = new Vector4(position.X, position.Y, position.Z, minSolidPlaneY);
			this._occluderPlanes[this._occluderPlanesCount + 3] = new Vector4(position.X, position.Y, position.Z, minSolidPlaneY);
			this._occluderPlanesCount += 4;
		}

		// Token: 0x0600536E RID: 21358 RVA: 0x0017A5FA File Offset: 0x001787FA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PrepareForIncomingOccludeeEntity(int count)
		{
			ArrayUtils.GrowArrayIfNecessary<BoundingBox>(ref this._entitiesOccludeesData, count, 250);
		}

		// Token: 0x0600536F RID: 21359 RVA: 0x0017A60F File Offset: 0x0017880F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RegisterOccludeeEntity(ref BoundingBox boundingBox)
		{
			this._entitiesOccludeesData[this._entitiesOccludeesCount] = boundingBox;
			this._entitiesOccludeesCount++;
		}

		// Token: 0x06005370 RID: 21360 RVA: 0x0017A637 File Offset: 0x00178837
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PrepareForIncomingOccludeeLight(int count)
		{
			ArrayUtils.GrowArrayIfNecessary<BoundingBox>(ref this._lightOccludeesData, count, 250);
		}

		// Token: 0x06005371 RID: 21361 RVA: 0x0017A64C File Offset: 0x0017884C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RegisterOccludeeLight(ref BoundingBox boundingBox)
		{
			this._lightOccludeesData[this._lightOccludeesCount] = boundingBox;
			this._lightOccludeesCount++;
		}

		// Token: 0x06005372 RID: 21362 RVA: 0x0017A674 File Offset: 0x00178874
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PrepareForIncomingOccludeeParticle(int count)
		{
			ArrayUtils.GrowArrayIfNecessary<BoundingBox>(ref this._particleOccludeesData, count, 250);
		}

		// Token: 0x06005373 RID: 21363 RVA: 0x0017A689 File Offset: 0x00178889
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RegisterOccludeeParticle(ref BoundingBox boundingBox)
		{
			this._particleOccludeesData[this._particleOccludeesCount] = boundingBox;
			this._particleOccludeesCount++;
		}

		// Token: 0x06005374 RID: 21364 RVA: 0x0017A6B4 File Offset: 0x001788B4
		public ref OcclusionCulling.OccludeeData[] GetOccludeesData(out int occludeesCount)
		{
			occludeesCount = this._opaqueOccludeesCount + this._alphaTestedOccludeesCount + this._alphaBlendedOccludeesCount + this._entitiesOccludeesCount + this._lightOccludeesCount + this._particleOccludeesCount;
			int growth = Math.Max(500, 2 * (occludeesCount - 2000));
			ArrayUtils.GrowArrayIfNecessary<OcclusionCulling.OccludeeData>(ref this._occludeesData, occludeesCount, growth);
			Vector3 value = new Vector3(32f);
			int num = 0;
			for (int i = 0; i < this._opaqueOccludeesCount; i++)
			{
				this._occludeesData[i].BoxMin = this._opaqueOccludeesData[i];
				this._occludeesData[i].BoxMax = this._opaqueOccludeesData[i] + value;
			}
			num += this._opaqueOccludeesCount;
			for (int j = 0; j < this._alphaTestedOccludeesCount; j++)
			{
				this._occludeesData[j + num].BoxMin = this._alphaTestedOccludeesData[j];
				this._occludeesData[j + num].BoxMax = this._alphaTestedOccludeesData[j] + value;
			}
			num += this._alphaTestedOccludeesCount;
			for (int k = 0; k < this._alphaBlendedOccludeesCount; k++)
			{
				this._occludeesData[k + num].BoxMin = this._alphaBlendedOccludeesData[k];
				this._occludeesData[k + num].BoxMax = this._alphaBlendedOccludeesData[k] + value;
			}
			num += this._alphaBlendedOccludeesCount;
			for (int l = 0; l < this._entitiesOccludeesCount; l++)
			{
				this._occludeesData[l + num].BoxMin = this._entitiesOccludeesData[l].Min;
				this._occludeesData[l + num].BoxMax = this._entitiesOccludeesData[l].Max;
			}
			num += this._entitiesOccludeesCount;
			for (int m = 0; m < this._lightOccludeesCount; m++)
			{
				this._occludeesData[m + num].BoxMin = this._lightOccludeesData[m].Min;
				this._occludeesData[m + num].BoxMax = this._lightOccludeesData[m].Max;
			}
			num += this._lightOccludeesCount;
			for (int n = 0; n < this._particleOccludeesCount; n++)
			{
				this._occludeesData[n + num].BoxMin = this._particleOccludeesData[n].Min;
				this._occludeesData[n + num].BoxMax = this._particleOccludeesData[n].Max;
			}
			return ref this._occludeesData;
		}

		// Token: 0x06005375 RID: 21365 RVA: 0x0017A9B8 File Offset: 0x00178BB8
		public void PrepareOcclusionCulling(int requestedOpaqueOccludersCount, bool useChunkOccluderPlanes, bool useOpaqueChunkOccluders, bool useAlphaTestedChunkOccluders, int mapAtlasTextureUnit, GLTexture mapAtlasTexture)
		{
			this._occlusionCullingSetup.RequestedOpaqueChunkOccludersCount = (byte)requestedOpaqueOccludersCount;
			this._occlusionCullingSetup.UseChunkOccluderPlanes = useChunkOccluderPlanes;
			this._occlusionCullingSetup.UseOpaqueChunkOccluders = useOpaqueChunkOccluders;
			this._occlusionCullingSetup.UseAlphaTestedChunkOccluders = useAlphaTestedChunkOccluders;
			this._occlusionCullingSetup.MapAtlasTextureUnit = (byte)mapAtlasTextureUnit;
			this._occlusionCullingSetup.MapAtlasTexture = mapAtlasTexture;
			int num = Math.Min(100, this._opaqueDrawTaskCount);
			int num2 = 0;
			bool flag = num > 0;
			if (flag)
			{
				for (int i = 0; i < this._opaqueDrawTaskCount; i++)
				{
					this._opaqueOccludersIDs[num2] = (byte)i;
					num2++;
					bool flag2 = num2 == num;
					if (flag2)
					{
						break;
					}
				}
			}
			this._opaqueOccludersCount = (byte)num2;
		}

		// Token: 0x06005376 RID: 21366 RVA: 0x0017AA6C File Offset: 0x00178C6C
		private unsafe void DrawChunkOccluderPlanes()
		{
			GLFunctions gl = this._graphics.GL;
			int num = 16;
			gl.BindBuffer(GL.ARRAY_BUFFER, this._occluderPlanesVerticesBuffer);
			Vector4[] array;
			Vector4* value;
			if ((array = this._occluderPlanes) == null || array.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array[0];
			}
			gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(this._occluderPlanesCount * num), (IntPtr)((void*)value), GL.DYNAMIC_DRAW);
			array = null;
			ZOnlyChunkPlanesProgram zonlyMapChunkPlanesProgram = this._gpuProgramStore.ZOnlyMapChunkPlanesProgram;
			gl.UseProgram(zonlyMapChunkPlanesProgram);
			zonlyMapChunkPlanesProgram.ViewProjectionMatrix.SetValue(ref this.Data.ViewRotationProjectionMatrix);
			int count = this._occluderPlanesCount / 4 * 6;
			gl.BindVertexArray(this._occluderPlanesVertexArray);
			gl.DrawElements(GL.TRIANGLES, count, GL.UNSIGNED_SHORT, IntPtr.Zero);
		}

		// Token: 0x06005377 RID: 21367 RVA: 0x0017AB44 File Offset: 0x00178D44
		private void DrawChunkOccluders()
		{
			GLFunctions gl = this._graphics.GL;
			ZOnlyChunkProgram zonlyMapChunkProgram = this._gpuProgramStore.ZOnlyMapChunkProgram;
			gl.ActiveTexture(GL.TEXTURE0 + (uint)this._occlusionCullingSetup.MapAtlasTextureUnit);
			gl.BindTexture(GL.TEXTURE_2D, this._occlusionCullingSetup.MapAtlasTexture);
			gl.UseProgram(zonlyMapChunkProgram);
			zonlyMapChunkProgram.ViewProjectionMatrix.SetValue(ref this.Data.ViewRotationProjectionMatrix);
			bool useOpaqueChunkOccluders = this._occlusionCullingSetup.UseOpaqueChunkOccluders;
			if (useOpaqueChunkOccluders)
			{
				byte b = Math.Min(this._occlusionCullingSetup.RequestedOpaqueChunkOccludersCount, this._opaqueOccludersCount);
				for (int i = 0; i < (int)b; i++)
				{
					byte b2 = this._opaqueOccludersIDs[i];
					zonlyMapChunkProgram.ModelMatrix.SetValue(ref this._opaqueDrawTasks[(int)b2].ModelMatrix);
					gl.BindVertexArray(this._opaqueDrawTasks[(int)b2].VertexArray);
					gl.DrawElements(GL.TRIANGLES, this._opaqueDrawTasks[(int)b2].DataCount, GL.UNSIGNED_INT, this._opaqueDrawTasks[(int)b2].DataOffset);
				}
			}
			bool useAlphaTestedChunkOccluders = this._occlusionCullingSetup.UseAlphaTestedChunkOccluders;
			if (useAlphaTestedChunkOccluders)
			{
				zonlyMapChunkProgram.Time.SetValue(this.Data.Time);
				gl.Disable(GL.CULL_FACE);
				int num = Math.Min(Math.Min(8, this._alphaTestedDrawTaskCount), this._farProgramAlphaTestedChunkStartIndex);
				for (int j = 0; j < num; j++)
				{
					zonlyMapChunkProgram.ModelMatrix.SetValue(ref this._alphaTestedDrawTasks[j].ModelMatrix);
					gl.BindVertexArray(this._alphaTestedDrawTasks[j].VertexArray);
					gl.DrawElements(GL.TRIANGLES, this._alphaTestedDrawTasks[j].DataCount, GL.UNSIGNED_INT, this._alphaTestedDrawTasks[j].DataOffset);
				}
				gl.Enable(GL.CULL_FACE);
			}
		}

		// Token: 0x06005378 RID: 21368 RVA: 0x0017AD50 File Offset: 0x00178F50
		public void DrawOccluders()
		{
			bool useChunkOccluderPlanes = this._occlusionCullingSetup.UseChunkOccluderPlanes;
			if (useChunkOccluderPlanes)
			{
				this.DrawChunkOccluderPlanes();
			}
			bool flag = this._occlusionCullingSetup.UseOpaqueChunkOccluders || this._occlusionCullingSetup.UseAlphaTestedChunkOccluders;
			if (flag)
			{
				this.DrawChunkOccluders();
			}
		}

		// Token: 0x06005379 RID: 21369 RVA: 0x0017AD9A File Offset: 0x00178F9A
		private void InitSunShadows()
		{
			this.UseSunShadows = true;
			this.InitShadowCasting();
			this._cascadedShadowMapping = new CascadedShadowMapping(this._graphics);
			this._cascadedShadowMapping.Init(new Action(this.DrawShadowCasters));
		}

		// Token: 0x0600537A RID: 21370 RVA: 0x0017ADD4 File Offset: 0x00178FD4
		private void DisposeSunShadows()
		{
			this._cascadedShadowMapping.Release();
			this._cascadedShadowMapping.Dispose();
			this._cascadedShadowMapping = null;
			this.DisposeEntitiesShadowMapGPUData();
		}

		// Token: 0x0600537B RID: 21371 RVA: 0x0017AE00 File Offset: 0x00179000
		private void InitShadowCasting()
		{
			for (int i = 0; i < 4; i++)
			{
				this._cascadeDrawTaskId[i] = new ushort[1000];
			}
			this._sunShadowCasting.DirectionType = SceneRenderer.SunShadowCastingSettings.ShadowDirectionType.TopDown;
			this._sunShadowCasting.Direction = Vector3.Down;
			this._sunShadowCasting.ShadowIntensity = 0.68f;
			this._sunShadowCasting.UseSafeAngle = true;
			this._sunShadowCasting.UseChunkShadowCasters = false;
			this._sunShadowCasting.UseEntitiesModelVFX = true;
			this._sunShadowCasting.UseDrawInstanced = false;
			this._sunShadowCasting.UseSmartCascadeDispatch = true;
			this.InitEntitiesShadowMapGPUData();
		}

		// Token: 0x0600537C RID: 21372 RVA: 0x0017AEA4 File Offset: 0x001790A4
		private void ResetSunShadowsCounters()
		{
			for (int i = 0; i < 4; i++)
			{
				this._cascadeEntityDrawTaskCount[i] = 0;
				this._cascadeChunkDrawTaskCount[i] = 0;
				this._cascadeAnimatedBlockDrawTaskCount[i] = 0;
			}
			this._entityShadowMapDrawTaskCount = 0;
			this._incomingEntityShadowMapDrawTaskCount = 0;
			this._chunkShadowMapDrawTaskCount = 0;
			this._incomingChunkShadowMapDrawTaskCount = 0;
			this._animatedBlockShadowMapDrawTaskCount = 0;
			this._incomingAnimatedBlockShadowMapDrawTaskCount = 0;
		}

		// Token: 0x170012EA RID: 4842
		// (get) Token: 0x0600537D RID: 21373 RVA: 0x0017AF09 File Offset: 0x00179109
		public bool IsSunShadowMappingEnabled
		{
			get
			{
				return this.UseSunShadows;
			}
		}

		// Token: 0x170012EB RID: 4843
		// (get) Token: 0x0600537E RID: 21374 RVA: 0x0017AF11 File Offset: 0x00179111
		public bool IsWorldShadowEnabled
		{
			get
			{
				return this._sunShadowCasting.UseChunkShadowCasters;
			}
		}

		// Token: 0x170012EC RID: 4844
		// (get) Token: 0x0600537F RID: 21375 RVA: 0x0017AF1E File Offset: 0x0017911E
		public bool UseSunShadowsSmartCascadeDispatch
		{
			get
			{
				return this._sunShadowCasting.UseSmartCascadeDispatch;
			}
		}

		// Token: 0x06005380 RID: 21376 RVA: 0x0017AF2B File Offset: 0x0017912B
		public void SetSunShadowsMaxWorldHeight(float maxWorldHeight)
		{
			this._cascadedShadowMapping.SetSunShadowsMaxWorldHeight(maxWorldHeight);
		}

		// Token: 0x06005381 RID: 21377 RVA: 0x0017AF3A File Offset: 0x0017913A
		public void SetSunShadowCastersDrawInstancedEnabled(bool enable)
		{
			this._sunShadowCasting.UseDrawInstanced = enable;
		}

		// Token: 0x06005382 RID: 21378 RVA: 0x0017AF48 File Offset: 0x00179148
		public void SetSunShadowCastersSmartCascadeDispatchEnabled(bool enable)
		{
			this._sunShadowCasting.UseSmartCascadeDispatch = enable;
		}

		// Token: 0x06005383 RID: 21379 RVA: 0x0017AF56 File Offset: 0x00179156
		public void SetSunShadowsSafeAngleEnabled(bool enable)
		{
			this._sunShadowCasting.UseSafeAngle = enable;
		}

		// Token: 0x06005384 RID: 21380 RVA: 0x0017AF64 File Offset: 0x00179164
		public void SetSunShadowsEnabled(bool enable)
		{
			bool flag = this.UseSunShadows != enable;
			if (flag)
			{
				this.UseSunShadows = enable;
				this._gpuProgramStore.DeferredProgram.UseDeferredShadow = enable;
				this._gpuProgramStore.DeferredProgram.Reset(true);
			}
		}

		// Token: 0x06005385 RID: 21381 RVA: 0x0017AFB0 File Offset: 0x001791B0
		public void SetSunShadowsWithChunks(bool enable)
		{
			bool flag = this._sunShadowCasting.UseChunkShadowCasters != enable;
			if (flag)
			{
				this._sunShadowCasting.UseChunkShadowCasters = enable;
				this._sunShadowCasting.UseChunkShadowCasters = enable;
				this._gpuProgramStore.DeferredProgram.UseDeferredShadowIndoorFading = enable;
				this._gpuProgramStore.DeferredProgram.Reset(true);
			}
		}

		// Token: 0x06005386 RID: 21382 RVA: 0x0017B010 File Offset: 0x00179210
		public void ToggleSunShadowsWithModelVFXs()
		{
			this._sunShadowCasting.UseEntitiesModelVFX = !this._sunShadowCasting.UseEntitiesModelVFX;
			this._gpuProgramStore.BlockyModelShadowMapProgram.UseModelVFX = this._sunShadowCasting.UseEntitiesModelVFX;
			this._gpuProgramStore.BlockyModelShadowMapProgram.Reset(true);
		}

		// Token: 0x06005387 RID: 21383 RVA: 0x0017B064 File Offset: 0x00179264
		public void SetSunShadowsIntensity(float value)
		{
			float shadowIntensity = MathHelper.Clamp(value, 0f, 1f);
			this._sunShadowCasting.ShadowIntensity = shadowIntensity;
		}

		// Token: 0x06005388 RID: 21384 RVA: 0x0017B090 File Offset: 0x00179290
		public void SetSunShadowsDirectionTopDown()
		{
			bool flag = this._sunShadowCasting.DirectionType > SceneRenderer.SunShadowCastingSettings.ShadowDirectionType.TopDown;
			if (flag)
			{
				this._sunShadowCasting.DirectionType = SceneRenderer.SunShadowCastingSettings.ShadowDirectionType.TopDown;
				this._sunShadowCasting.Direction = Vector3.Down;
				this.SetSunShadowsUseCleanBackfaces(false);
			}
		}

		// Token: 0x06005389 RID: 21385 RVA: 0x0017B0D8 File Offset: 0x001792D8
		public void SetSunShadowsDirectionCustom(Vector3 direction)
		{
			Vector3 vector = Vector3.Normalize(direction);
			bool flag = this._sunShadowCasting.DirectionType != SceneRenderer.SunShadowCastingSettings.ShadowDirectionType.StaticCustom || this._sunShadowCasting.Direction != vector;
			if (flag)
			{
				this._sunShadowCasting.DirectionType = SceneRenderer.SunShadowCastingSettings.ShadowDirectionType.StaticCustom;
				this._sunShadowCasting.Direction = vector;
				this.SetSunShadowsUseCleanBackfaces(true);
			}
		}

		// Token: 0x0600538A RID: 21386 RVA: 0x0017B138 File Offset: 0x00179338
		public void SetSunShadowsDirectionSun(bool useCleanBackFaces)
		{
			bool flag = this._sunShadowCasting.DirectionType != SceneRenderer.SunShadowCastingSettings.ShadowDirectionType.DynamicSun;
			if (flag)
			{
				this._sunShadowCasting.DirectionType = SceneRenderer.SunShadowCastingSettings.ShadowDirectionType.DynamicSun;
				this.SetSunShadowsUseCleanBackfaces(useCleanBackFaces);
			}
		}

		// Token: 0x0600538B RID: 21387 RVA: 0x0017B174 File Offset: 0x00179374
		public void ToggleSunShadowCastersDrawInstanced()
		{
			this._sunShadowCasting.UseDrawInstanced = !this._sunShadowCasting.UseDrawInstanced;
			this._gpuProgramStore.BlockyModelShadowMapProgram.UseDrawInstanced = this._sunShadowCasting.UseDrawInstanced;
			this._gpuProgramStore.BlockyModelShadowMapProgram.Reset(true);
			this._gpuProgramStore.MapChunkShadowMapProgram.UseDrawInstanced = this._sunShadowCasting.UseDrawInstanced;
			this._gpuProgramStore.MapChunkShadowMapProgram.Reset(true);
		}

		// Token: 0x0600538C RID: 21388 RVA: 0x0017B1F8 File Offset: 0x001793F8
		public void ToggleSunShadowsBiasMethod1()
		{
			this._gpuProgramStore.BlockyModelShadowMapProgram.UseBiasMethod1 = !this._gpuProgramStore.BlockyModelShadowMapProgram.UseBiasMethod1;
			this._gpuProgramStore.BlockyModelShadowMapProgram.UseBiasMethod2 = false;
			this._gpuProgramStore.BlockyModelShadowMapProgram.Reset(true);
		}

		// Token: 0x0600538D RID: 21389 RVA: 0x0017B24C File Offset: 0x0017944C
		public void ToggleSunShadowsBiasMethod2()
		{
			this._gpuProgramStore.BlockyModelShadowMapProgram.UseBiasMethod2 = !this._gpuProgramStore.BlockyModelShadowMapProgram.UseBiasMethod2;
			this._gpuProgramStore.BlockyModelShadowMapProgram.UseBiasMethod1 = false;
			this._gpuProgramStore.BlockyModelShadowMapProgram.Reset(true);
		}

		// Token: 0x0600538E RID: 21390 RVA: 0x0017B2A0 File Offset: 0x001794A0
		public void SetSunShadowsCascadeCount(int count)
		{
			this._cascadedShadowMapping.SetSunShadowsCascadeCount(count);
			int count2 = this._cascadedShadowMapping.CascadesSettings.Count;
			this._gpuProgramStore.ParticleErosionProgram.SunShadowCascadeCount = (uint)count2;
			this._gpuProgramStore.ParticleErosionProgram.Reset(true);
			this._gpuProgramStore.ParticleProgram.SunShadowCascadeCount = (uint)count2;
			this._gpuProgramStore.ParticleProgram.Reset(true);
			this._gpuProgramStore.MapChunkAlphaBlendedProgram.SunShadowCascadeCount = (uint)count2;
			this._gpuProgramStore.MapChunkAlphaBlendedProgram.Reset(true);
			bool debugShadowCascades = this._gpuProgramStore.DeferredProgram.DebugShadowCascades;
			if (debugShadowCascades)
			{
				this._gpuProgramStore.DeferredProgram.CascadeCount = (uint)count2;
				this._gpuProgramStore.DeferredProgram.Reset(true);
			}
		}

		// Token: 0x0600538F RID: 21391 RVA: 0x0017B370 File Offset: 0x00179570
		public void SetSunShadowMappingUseLinearZ(bool enable)
		{
			this._cascadedShadowMapping.SetSunShadowMappingUseLinearZ(enable);
			this._gpuProgramStore.ParticleErosionProgram.UseLinearZ = enable;
			this._gpuProgramStore.ParticleErosionProgram.Reset(true);
			this._gpuProgramStore.ParticleProgram.UseLinearZ = enable;
			this._gpuProgramStore.ParticleProgram.Reset(true);
			this._gpuProgramStore.MapChunkAlphaBlendedProgram.UseLinearZ = enable;
			this._gpuProgramStore.MapChunkAlphaBlendedProgram.Reset(true);
		}

		// Token: 0x06005390 RID: 21392 RVA: 0x0017B3F4 File Offset: 0x001795F4
		public void SetSunShadowsUseCleanBackfaces(bool enable)
		{
			this._gpuProgramStore.DeferredShadowProgram.UseCleanBackfaces = enable;
			this._gpuProgramStore.DeferredShadowProgram.Reset(true);
		}

		// Token: 0x06005391 RID: 21393 RVA: 0x0017B41C File Offset: 0x0017961C
		public void SetDeferredShadowsBlurEnabled(bool enable)
		{
			bool flag = this._cascadedShadowMapping.DeferredShadowSettings.UseBlur != enable;
			if (flag)
			{
				this._cascadedShadowMapping.SetDeferredShadowsBlurEnabled(enable);
				this._gpuProgramStore.DeferredProgram.UseDeferredShadowBlurred = enable;
				this._gpuProgramStore.DeferredProgram.Reset(true);
			}
		}

		// Token: 0x170012ED RID: 4845
		// (get) Token: 0x06005392 RID: 21394 RVA: 0x0017B476 File Offset: 0x00179676
		public bool UseDeferredShadowBlur
		{
			get
			{
				return this._cascadedShadowMapping.UseDeferredShadowBlur;
			}
		}

		// Token: 0x170012EE RID: 4846
		// (get) Token: 0x06005393 RID: 21395 RVA: 0x0017B483 File Offset: 0x00179683
		public bool UseSunShadowsGlobalKDop
		{
			get
			{
				return this._cascadedShadowMapping.UseSunShadowsGlobalKDop;
			}
		}

		// Token: 0x06005394 RID: 21396 RVA: 0x0017B490 File Offset: 0x00179690
		public void SetSunShadowsGlobalKDopEnabled(bool enable)
		{
			this._cascadedShadowMapping.SetSunShadowsGlobalKDopEnabled(enable);
		}

		// Token: 0x06005395 RID: 21397 RVA: 0x0017B49F File Offset: 0x0017969F
		public void SetSunShadowsSlopeScaleBias(float factor, float units)
		{
			this._cascadedShadowMapping.SetSunShadowsSlopeScaleBias(factor, units);
		}

		// Token: 0x06005396 RID: 21398 RVA: 0x0017B4AF File Offset: 0x001796AF
		public void SetSunShadowMapResolution(uint width, uint height = 0U)
		{
			this._cascadedShadowMapping.SetSunShadowMapResolution(width, height);
		}

		// Token: 0x06005397 RID: 21399 RVA: 0x0017B4BF File Offset: 0x001796BF
		public void SetDeferredShadowResolutionScale(float scale)
		{
			this._cascadedShadowMapping.SetDeferredShadowResolutionScale(scale);
		}

		// Token: 0x06005398 RID: 21400 RVA: 0x0017B4CE File Offset: 0x001796CE
		public void SetSunShadowMapCachingEnabled(bool enable)
		{
			this._cascadedShadowMapping.SetSunShadowMapCachingEnabled(enable);
		}

		// Token: 0x06005399 RID: 21401 RVA: 0x0017B4DD File Offset: 0x001796DD
		public void SetSunShadowMappingStableProjectionEnabled(bool enable)
		{
			this._cascadedShadowMapping.SetSunShadowMappingStableProjectionEnabled(enable);
		}

		// Token: 0x0600539A RID: 21402 RVA: 0x0017B4EC File Offset: 0x001796EC
		public void SetDeferredShadowsNoiseEnabled(bool enable)
		{
			this._cascadedShadowMapping.SetDeferredShadowsNoiseEnabled(enable);
		}

		// Token: 0x0600539B RID: 21403 RVA: 0x0017B4FB File Offset: 0x001796FB
		public void SetDeferredShadowsManualModeEnabled(bool enable)
		{
			this._cascadedShadowMapping.SetDeferredShadowsManualModeEnabled(enable);
		}

		// Token: 0x0600539C RID: 21404 RVA: 0x0017B50A File Offset: 0x0017970A
		public void SetDeferredShadowsFadingEnabled(bool enable)
		{
			this._cascadedShadowMapping.SetDeferredShadowsFadingEnabled(enable);
		}

		// Token: 0x0600539D RID: 21405 RVA: 0x0017B519 File Offset: 0x00179719
		public void SetDeferredShadowsWithSingleSampleEnabled(bool enable)
		{
			this._cascadedShadowMapping.SetDeferredShadowsWithSingleSampleEnabled(enable);
		}

		// Token: 0x0600539E RID: 21406 RVA: 0x0017B528 File Offset: 0x00179728
		public void SetDeferredShadowsCameraBiasEnabled(bool enable)
		{
			this._cascadedShadowMapping.SetDeferredShadowsCameraBiasEnabled(enable);
		}

		// Token: 0x0600539F RID: 21407 RVA: 0x0017B537 File Offset: 0x00179737
		public void SetDeferredShadowsNormalBiasEnabled(bool enable)
		{
			this._cascadedShadowMapping.SetDeferredShadowsNormalBiasEnabled(enable);
		}

		// Token: 0x060053A0 RID: 21408 RVA: 0x0017B548 File Offset: 0x00179748
		public int GetShadowCascadeDrawCallCount(int cascadeId)
		{
			Debug.Assert(cascadeId < 4);
			return (int)this._cascadeStats[cascadeId].DrawCallCount;
		}

		// Token: 0x060053A1 RID: 21409 RVA: 0x0017B578 File Offset: 0x00179778
		public int GetShadowCascadeKiloTriangleCount(int cascadeId)
		{
			Debug.Assert(cascadeId < 4);
			return (int)this._cascadeStats[cascadeId].KiloTriangleCount;
		}

		// Token: 0x060053A2 RID: 21410 RVA: 0x0017B5A5 File Offset: 0x001797A5
		public void SetupEntityShadowMapDataTexture(uint unitId)
		{
			this._gl.ActiveTexture(GL.TEXTURE0 + unitId);
			this._gl.BindTexture(GL.TEXTURE_BUFFER, this._entityShadowMapDataBufferTexture.CurrentTexture);
		}

		// Token: 0x060053A3 RID: 21411 RVA: 0x0017B5D7 File Offset: 0x001797D7
		private void InitEntitiesShadowMapGPUData()
		{
			this._entityShadowMapDataBufferTexture.CreateStorage(GL.RGBA32F, GL.STREAM_DRAW, true, this._entityShadowMapBufferSize, 1024U, GPUBuffer.GrowthPolicy.GrowthAutoNoLimit, 0U);
		}

		// Token: 0x060053A4 RID: 21412 RVA: 0x0017B5FE File Offset: 0x001797FE
		private void DisposeEntitiesShadowMapGPUData()
		{
			this._entityShadowMapDataBufferTexture.DestroyStorage();
		}

		// Token: 0x060053A5 RID: 21413 RVA: 0x0017B60D File Offset: 0x0017980D
		private void PingPongEntityShadowMapDataBuffers()
		{
			this._entityShadowMapDataBufferTexture.Swap();
		}

		// Token: 0x060053A6 RID: 21414 RVA: 0x0017B61C File Offset: 0x0017981C
		public unsafe void SendEntityShadowMapDataToGPU()
		{
			uint num = (uint)(this._entityShadowMapDrawTaskCount * SceneRenderer.GPUEntityShadowMapDataSize);
			bool flag = num > 0U;
			if (flag)
			{
				this._entityShadowMapDataBufferTexture.GrowStorageIfNecessary(num);
				IntPtr pointer = this._entityShadowMapDataBufferTexture.BeginTransfer(num);
				for (int i = 0; i < this._entityShadowMapDrawTaskCount; i++)
				{
					IntPtr pointer2 = IntPtr.Add(pointer, i * SceneRenderer.GPUEntityShadowMapDataSize);
					Matrix* ptr = (Matrix*)pointer2.ToPointer();
					*ptr = this._entityShadowMapDrawTasks[i].ModelMatrix;
					Vector4* ptr2 = (Vector4*)IntPtr.Add(pointer2, sizeof(Matrix)).ToPointer();
					*ptr2 = new Vector4((float)this._entityShadowMapDrawTasks[i].CascadeFirstLast.X, (float)this._entityShadowMapDrawTasks[i].CascadeFirstLast.Y, 0f, 0f);
					ptr2[1] = new Vector4((float)this._entityShadowMapDrawTasks[i].ModelVFXId, this._entityShadowMapDrawTasks[i].InvModelHeight, this._entityShadowMapDrawTasks[i].ModelVFXAnimationProgress, 0f);
				}
				this._entityShadowMapDataBufferTexture.EndTransfer();
			}
		}

		// Token: 0x060053A7 RID: 21415 RVA: 0x0017B763 File Offset: 0x00179963
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PrepareForIncomingEntitySunShadowCasterDrawTasks(int size)
		{
			this._incomingEntityShadowMapDrawTaskCount += size;
			ArrayUtils.GrowArrayIfNecessary<SceneRenderer.EntityShadowMapDrawTask>(ref this._entityShadowMapDrawTasks, this._incomingEntityShadowMapDrawTaskCount, 200);
			ArrayUtils.GrowArrayIfNecessary<BoundingSphere>(ref this._entitiesBoundingVolumes, this._incomingEntityShadowMapDrawTaskCount, 200);
		}

		// Token: 0x060053A8 RID: 21416 RVA: 0x0017B7A4 File Offset: 0x001799A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RegisterEntitySunShadowCasterDrawTask(ref BoundingSphere boundingSphere, ref Matrix modelMatrix, GLVertexArray vertexArray, int dataCount, GLBuffer animationData, uint animationDataOffset, uint animationDataCount, float modelHeight, float modelVFXAnimationProgress, int modelVFXId)
		{
			int entityShadowMapDrawTaskCount = this._entityShadowMapDrawTaskCount;
			this._entitiesBoundingVolumes[entityShadowMapDrawTaskCount] = boundingSphere;
			this._entityShadowMapDrawTasks[entityShadowMapDrawTaskCount].ModelMatrix = modelMatrix;
			this._entityShadowMapDrawTasks[entityShadowMapDrawTaskCount].VertexArray = vertexArray;
			this._entityShadowMapDrawTasks[entityShadowMapDrawTaskCount].DataCount = dataCount;
			this._entityShadowMapDrawTasks[entityShadowMapDrawTaskCount].AnimationData = animationData;
			this._entityShadowMapDrawTasks[entityShadowMapDrawTaskCount].AnimationDataOffset = animationDataOffset;
			this._entityShadowMapDrawTasks[entityShadowMapDrawTaskCount].AnimationDataSize = (ushort)(animationDataCount * 64U);
			this._entityShadowMapDrawTasks[entityShadowMapDrawTaskCount].InvModelHeight = 1f / modelHeight;
			this._entityShadowMapDrawTasks[entityShadowMapDrawTaskCount].ModelVFXAnimationProgress = modelVFXAnimationProgress;
			this._entityShadowMapDrawTasks[entityShadowMapDrawTaskCount].ModelVFXId = modelVFXId;
			this._entityShadowMapDrawTaskCount++;
		}

		// Token: 0x060053A9 RID: 21417 RVA: 0x0017B891 File Offset: 0x00179A91
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PrepareForIncomingMapChunkSunShadowCasterDrawTasks(int size)
		{
			this._incomingChunkShadowMapDrawTaskCount += size;
			ArrayUtils.GrowArrayIfNecessary<SceneRenderer.ChunkShadowMapDrawTask>(ref this._chunkShadowMapDrawTasks, this._incomingChunkShadowMapDrawTaskCount, 200);
		}

		// Token: 0x060053AA RID: 21418 RVA: 0x0017B8BC File Offset: 0x00179ABC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RegisterMapChunkSunShadowCasterDrawTask(ref Matrix modelMatrix, GLVertexArray vertexArray, int dataCount, IntPtr dataOffset)
		{
			int chunkShadowMapDrawTaskCount = this._chunkShadowMapDrawTaskCount;
			this._chunkShadowMapDrawTasks[chunkShadowMapDrawTaskCount].ModelMatrix = modelMatrix;
			this._chunkShadowMapDrawTasks[chunkShadowMapDrawTaskCount].VertexArray = vertexArray;
			this._chunkShadowMapDrawTasks[chunkShadowMapDrawTaskCount].DataCount = dataCount;
			this._chunkShadowMapDrawTasks[chunkShadowMapDrawTaskCount].DataOffset = dataOffset;
			this._chunkShadowMapDrawTaskCount++;
		}

		// Token: 0x060053AB RID: 21419 RVA: 0x0017B92D File Offset: 0x00179B2D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PrepareForIncomingMapBlockAnimatedSunShadowCasterDrawTasks(int size)
		{
			this._incomingAnimatedBlockShadowMapDrawTaskCount += size;
			ArrayUtils.GrowArrayIfNecessary<SceneRenderer.AnimatedBlockShadowMapDrawTask>(ref this._animatedBlockShadowMapDrawTasks, this._incomingAnimatedBlockShadowMapDrawTaskCount, 200);
			ArrayUtils.GrowArrayIfNecessary<BoundingBox>(ref this._animatedBlockBoundingVolumes, this._incomingAnimatedBlockShadowMapDrawTaskCount, 200);
		}

		// Token: 0x060053AC RID: 21420 RVA: 0x0017B96C File Offset: 0x00179B6C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RegisterMapBlockAnimatedSunShadowCasterDrawTask(ref BoundingBox boundingBox, ref Matrix modelMatrix, GLVertexArray vertexArray, int indicesCount, GLBuffer animationData, uint animationDataOffset, uint animationDataCount)
		{
			int animatedBlockShadowMapDrawTaskCount = this._animatedBlockShadowMapDrawTaskCount;
			this._animatedBlockBoundingVolumes[animatedBlockShadowMapDrawTaskCount] = boundingBox;
			this._animatedBlockShadowMapDrawTasks[animatedBlockShadowMapDrawTaskCount].ModelMatrix = modelMatrix;
			this._animatedBlockShadowMapDrawTasks[animatedBlockShadowMapDrawTaskCount].VertexArray = vertexArray;
			this._animatedBlockShadowMapDrawTasks[animatedBlockShadowMapDrawTaskCount].DataCount = indicesCount;
			this._animatedBlockShadowMapDrawTasks[animatedBlockShadowMapDrawTaskCount].DataOffset = IntPtr.Zero;
			this._animatedBlockShadowMapDrawTasks[animatedBlockShadowMapDrawTaskCount].AnimationData = animationData;
			this._animatedBlockShadowMapDrawTasks[animatedBlockShadowMapDrawTaskCount].AnimationDataOffset = animationDataOffset;
			this._animatedBlockShadowMapDrawTasks[animatedBlockShadowMapDrawTaskCount].AnimationDataSize = (ushort)(animationDataCount * 64U);
			this._animatedBlockShadowMapDrawTaskCount++;
		}

		// Token: 0x060053AD RID: 21421 RVA: 0x0017BA30 File Offset: 0x00179C30
		public void PrepareShadowCastersForDraw()
		{
			bool flag = (!this._sunShadowCasting.UseSmartCascadeDispatch && !this._sunShadowCasting.UseDrawInstanced) || this._cascadedShadowMapping.CascadesSettings.Count == 1;
			if (!flag)
			{
				int itemCount = this._entityShadowMapDrawTaskCount + this._chunkShadowMapDrawTaskCount + this._animatedBlockShadowMapDrawTaskCount;
				ArrayUtils.GrowArrayIfNecessary<ushort>(ref this._cascadeDrawTaskId[0], itemCount, 500);
				ArrayUtils.GrowArrayIfNecessary<ushort>(ref this._cascadeDrawTaskId[1], itemCount, 500);
				ArrayUtils.GrowArrayIfNecessary<ushort>(ref this._cascadeDrawTaskId[2], itemCount, 500);
				ArrayUtils.GrowArrayIfNecessary<ushort>(ref this._cascadeDrawTaskId[3], itemCount, 500);
				for (int i = 0; i < this._entityShadowMapDrawTaskCount; i++)
				{
					ContainmentType containmentType = this._cascadedShadowMapping.CascadeFrustums[0].Contains(this._entitiesBoundingVolumes[i]);
					ContainmentType containmentType2 = this._cascadedShadowMapping.CascadeFrustums[1].Contains(this._entitiesBoundingVolumes[i]);
					ContainmentType containmentType3 = this._cascadedShadowMapping.CascadeFrustums[2].Contains(this._entitiesBoundingVolumes[i]);
					ContainmentType containmentType4 = this._cascadedShadowMapping.CascadeFrustums[3].Contains(this._entitiesBoundingVolumes[i]);
					bool useDrawInstanced = this._sunShadowCasting.UseDrawInstanced;
					if (useDrawInstanced)
					{
						int num = 3;
						int num2 = 0;
						bool flag2 = this._cascadedShadowMapping.CascadeNeedsUpdate[0] && containmentType > ContainmentType.Disjoint;
						if (flag2)
						{
							num = Math.Min(num, 0);
							num2 = Math.Max(num2, 0);
						}
						bool flag3 = this._cascadedShadowMapping.CascadeNeedsUpdate[1] && containmentType2 != ContainmentType.Disjoint && containmentType != ContainmentType.Contains;
						if (flag3)
						{
							num = Math.Min(num, 1);
							num2 = Math.Max(num2, 1);
						}
						bool flag4 = this._cascadedShadowMapping.CascadeNeedsUpdate[2] && containmentType3 != ContainmentType.Disjoint && containmentType2 != ContainmentType.Contains;
						if (flag4)
						{
							num = Math.Min(num, 2);
							num2 = Math.Max(num2, 2);
						}
						bool flag5 = this._cascadedShadowMapping.CascadeNeedsUpdate[3] && containmentType4 != ContainmentType.Disjoint && containmentType3 != ContainmentType.Contains;
						if (flag5)
						{
							num = Math.Min(num, 3);
							num2 = Math.Max(num2, 3);
						}
						bool flag6 = num <= num2;
						if (flag6)
						{
							ushort num3 = this._cascadeEntityDrawTaskCount[0];
							this._cascadeDrawTaskId[0][(int)num3] = (ushort)i;
							ushort[] cascadeEntityDrawTaskCount = this._cascadeEntityDrawTaskCount;
							int num4 = 0;
							cascadeEntityDrawTaskCount[num4] += 1;
							this._entityShadowMapDrawTasks[i].CascadeFirstLast.X = (ushort)num;
							this._entityShadowMapDrawTasks[i].CascadeFirstLast.Y = (ushort)num2;
						}
					}
					else
					{
						bool flag7 = this._cascadedShadowMapping.CascadeNeedsUpdate[0] && containmentType > ContainmentType.Disjoint;
						if (flag7)
						{
							ushort num5 = this._cascadeEntityDrawTaskCount[0];
							this._cascadeDrawTaskId[0][(int)num5] = (ushort)i;
							ushort[] cascadeEntityDrawTaskCount2 = this._cascadeEntityDrawTaskCount;
							int num6 = 0;
							cascadeEntityDrawTaskCount2[num6] += 1;
						}
						bool flag8 = this._cascadedShadowMapping.CascadeNeedsUpdate[1] && containmentType2 != ContainmentType.Disjoint && containmentType != ContainmentType.Contains;
						if (flag8)
						{
							ushort num7 = this._cascadeEntityDrawTaskCount[1];
							this._cascadeDrawTaskId[1][(int)num7] = (ushort)i;
							ushort[] cascadeEntityDrawTaskCount3 = this._cascadeEntityDrawTaskCount;
							int num8 = 1;
							cascadeEntityDrawTaskCount3[num8] += 1;
						}
						bool flag9 = this._cascadedShadowMapping.CascadeNeedsUpdate[2] && containmentType3 != ContainmentType.Disjoint && containmentType2 != ContainmentType.Contains;
						if (flag9)
						{
							ushort num9 = this._cascadeEntityDrawTaskCount[2];
							this._cascadeDrawTaskId[2][(int)num9] = (ushort)i;
							ushort[] cascadeEntityDrawTaskCount4 = this._cascadeEntityDrawTaskCount;
							int num10 = 2;
							cascadeEntityDrawTaskCount4[num10] += 1;
						}
						bool flag10 = this._cascadedShadowMapping.CascadeNeedsUpdate[3] && containmentType4 != ContainmentType.Disjoint && containmentType3 != ContainmentType.Contains;
						if (flag10)
						{
							ushort num11 = this._cascadeEntityDrawTaskCount[3];
							this._cascadeDrawTaskId[3][(int)num11] = (ushort)i;
							ushort[] cascadeEntityDrawTaskCount5 = this._cascadeEntityDrawTaskCount;
							int num12 = 3;
							cascadeEntityDrawTaskCount5[num12] += 1;
						}
					}
				}
				for (int j = 0; j < this._chunkShadowMapDrawTaskCount; j++)
				{
					BoundingBox box;
					box.Min = this._chunkShadowMapDrawTasks[j].ModelMatrix.Translation;
					box.Max = this._chunkShadowMapDrawTasks[j].ModelMatrix.Translation + new Vector3(32f);
					ContainmentType containmentType5 = this._cascadedShadowMapping.CascadeFrustums[0].Contains(box);
					ContainmentType containmentType6 = this._cascadedShadowMapping.CascadeFrustums[1].Contains(box);
					ContainmentType containmentType7 = this._cascadedShadowMapping.CascadeFrustums[2].Contains(box);
					ContainmentType containmentType8 = this._cascadedShadowMapping.CascadeFrustums[3].Contains(box);
					bool useDrawInstanced2 = this._sunShadowCasting.UseDrawInstanced;
					if (useDrawInstanced2)
					{
						int num13 = 3;
						int num14 = 0;
						bool flag11 = this._cascadedShadowMapping.CascadeNeedsUpdate[0] && containmentType5 > ContainmentType.Disjoint;
						if (flag11)
						{
							num13 = Math.Min(num13, 0);
							num14 = Math.Max(num14, 0);
						}
						bool flag12 = this._cascadedShadowMapping.CascadeNeedsUpdate[1] && containmentType6 != ContainmentType.Disjoint && containmentType5 != ContainmentType.Contains;
						if (flag12)
						{
							num13 = Math.Min(num13, 1);
							num14 = Math.Max(num14, 1);
						}
						bool flag13 = this._cascadedShadowMapping.CascadeNeedsUpdate[2] && containmentType7 != ContainmentType.Disjoint && containmentType6 != ContainmentType.Contains;
						if (flag13)
						{
							num13 = Math.Min(num13, 2);
							num14 = Math.Max(num14, 2);
						}
						bool flag14 = this._cascadedShadowMapping.CascadeNeedsUpdate[3] && containmentType8 != ContainmentType.Disjoint && containmentType7 != ContainmentType.Contains;
						if (flag14)
						{
							num13 = Math.Min(num13, 3);
							num14 = Math.Max(num14, 3);
						}
						bool flag15 = num13 <= num14;
						if (flag15)
						{
							int num15 = (int)(this._cascadeChunkDrawTaskCount[0] + this._cascadeEntityDrawTaskCount[0]);
							this._cascadeDrawTaskId[0][num15] = (ushort)j;
							ushort[] cascadeChunkDrawTaskCount = this._cascadeChunkDrawTaskCount;
							int num16 = 0;
							cascadeChunkDrawTaskCount[num16] += 1;
							this._chunkShadowMapDrawTasks[j].CascadeFirstLast.X = (ushort)num13;
							this._chunkShadowMapDrawTasks[j].CascadeFirstLast.Y = (ushort)num14;
						}
					}
					else
					{
						bool flag16 = this._cascadedShadowMapping.CascadeNeedsUpdate[0] && containmentType5 > ContainmentType.Disjoint;
						if (flag16)
						{
							int num17 = (int)(this._cascadeChunkDrawTaskCount[0] + this._cascadeEntityDrawTaskCount[0]);
							this._cascadeDrawTaskId[0][num17] = (ushort)j;
							ushort[] cascadeChunkDrawTaskCount2 = this._cascadeChunkDrawTaskCount;
							int num18 = 0;
							cascadeChunkDrawTaskCount2[num18] += 1;
						}
						bool flag17 = this._cascadedShadowMapping.CascadeNeedsUpdate[1] && containmentType6 != ContainmentType.Disjoint && containmentType5 != ContainmentType.Contains;
						if (flag17)
						{
							int num19 = (int)(this._cascadeChunkDrawTaskCount[1] + this._cascadeEntityDrawTaskCount[1]);
							this._cascadeDrawTaskId[1][num19] = (ushort)j;
							ushort[] cascadeChunkDrawTaskCount3 = this._cascadeChunkDrawTaskCount;
							int num20 = 1;
							cascadeChunkDrawTaskCount3[num20] += 1;
						}
						bool flag18 = this._cascadedShadowMapping.CascadeNeedsUpdate[2] && containmentType7 != ContainmentType.Disjoint && containmentType6 != ContainmentType.Contains;
						if (flag18)
						{
							int num21 = (int)(this._cascadeChunkDrawTaskCount[2] + this._cascadeEntityDrawTaskCount[2]);
							this._cascadeDrawTaskId[2][num21] = (ushort)j;
							ushort[] cascadeChunkDrawTaskCount4 = this._cascadeChunkDrawTaskCount;
							int num22 = 2;
							cascadeChunkDrawTaskCount4[num22] += 1;
						}
						bool flag19 = this._cascadedShadowMapping.CascadeNeedsUpdate[3] && containmentType8 != ContainmentType.Disjoint && containmentType7 != ContainmentType.Contains;
						if (flag19)
						{
							int num23 = (int)(this._cascadeChunkDrawTaskCount[3] + this._cascadeEntityDrawTaskCount[3]);
							this._cascadeDrawTaskId[3][num23] = (ushort)j;
							ushort[] cascadeChunkDrawTaskCount5 = this._cascadeChunkDrawTaskCount;
							int num24 = 3;
							cascadeChunkDrawTaskCount5[num24] += 1;
						}
					}
				}
				for (int k = 0; k < this._animatedBlockShadowMapDrawTaskCount; k++)
				{
					ContainmentType containmentType9 = this._cascadedShadowMapping.CascadeFrustums[0].Contains(this._animatedBlockBoundingVolumes[k]);
					ContainmentType containmentType10 = this._cascadedShadowMapping.CascadeFrustums[1].Contains(this._animatedBlockBoundingVolumes[k]);
					ContainmentType containmentType11 = this._cascadedShadowMapping.CascadeFrustums[2].Contains(this._animatedBlockBoundingVolumes[k]);
					ContainmentType containmentType12 = this._cascadedShadowMapping.CascadeFrustums[3].Contains(this._animatedBlockBoundingVolumes[k]);
					bool useDrawInstanced3 = this._sunShadowCasting.UseDrawInstanced;
					if (useDrawInstanced3)
					{
						int num25 = 3;
						int num26 = 0;
						bool flag20 = this._cascadedShadowMapping.CascadeNeedsUpdate[0] && containmentType9 > ContainmentType.Disjoint;
						if (flag20)
						{
							num25 = Math.Min(num25, 0);
							num26 = Math.Max(num26, 0);
						}
						bool flag21 = this._cascadedShadowMapping.CascadeNeedsUpdate[1] && containmentType10 != ContainmentType.Disjoint && containmentType9 != ContainmentType.Contains;
						if (flag21)
						{
							num25 = Math.Min(num25, 1);
							num26 = Math.Max(num26, 1);
						}
						bool flag22 = this._cascadedShadowMapping.CascadeNeedsUpdate[2] && containmentType11 != ContainmentType.Disjoint && containmentType10 != ContainmentType.Contains;
						if (flag22)
						{
							num25 = Math.Min(num25, 2);
							num26 = Math.Max(num26, 2);
						}
						bool flag23 = this._cascadedShadowMapping.CascadeNeedsUpdate[3] && containmentType12 != ContainmentType.Disjoint && containmentType11 != ContainmentType.Contains;
						if (flag23)
						{
							num25 = Math.Min(num25, 3);
							num26 = Math.Max(num26, 3);
						}
						bool flag24 = num25 <= num26;
						if (flag24)
						{
							ushort num27 = this._cascadeEntityDrawTaskCount[0];
							this._cascadeDrawTaskId[0][(int)num27] = (ushort)k;
							ushort[] cascadeAnimatedBlockDrawTaskCount = this._cascadeAnimatedBlockDrawTaskCount;
							int num28 = 0;
							cascadeAnimatedBlockDrawTaskCount[num28] += 1;
							this._animatedBlockShadowMapDrawTasks[k].CascadeFirstLast.X = (ushort)num25;
							this._animatedBlockShadowMapDrawTasks[k].CascadeFirstLast.Y = (ushort)num26;
						}
					}
					else
					{
						bool flag25 = this._cascadedShadowMapping.CascadeNeedsUpdate[0] && containmentType9 > ContainmentType.Disjoint;
						if (flag25)
						{
							int num29 = (int)(this._cascadeAnimatedBlockDrawTaskCount[0] + this._cascadeChunkDrawTaskCount[0] + this._cascadeEntityDrawTaskCount[0]);
							this._cascadeDrawTaskId[0][num29] = (ushort)k;
							ushort[] cascadeAnimatedBlockDrawTaskCount2 = this._cascadeAnimatedBlockDrawTaskCount;
							int num30 = 0;
							cascadeAnimatedBlockDrawTaskCount2[num30] += 1;
						}
						bool flag26 = this._cascadedShadowMapping.CascadeNeedsUpdate[1] && containmentType10 != ContainmentType.Disjoint && containmentType9 != ContainmentType.Contains;
						if (flag26)
						{
							int num31 = (int)(this._cascadeAnimatedBlockDrawTaskCount[1] + this._cascadeChunkDrawTaskCount[1] + this._cascadeEntityDrawTaskCount[1]);
							this._cascadeDrawTaskId[1][num31] = (ushort)k;
							ushort[] cascadeAnimatedBlockDrawTaskCount3 = this._cascadeAnimatedBlockDrawTaskCount;
							int num32 = 1;
							cascadeAnimatedBlockDrawTaskCount3[num32] += 1;
						}
						bool flag27 = this._cascadedShadowMapping.CascadeNeedsUpdate[2] && containmentType11 != ContainmentType.Disjoint && containmentType10 != ContainmentType.Contains;
						if (flag27)
						{
							int num33 = (int)(this._cascadeAnimatedBlockDrawTaskCount[2] + this._cascadeChunkDrawTaskCount[2] + this._cascadeEntityDrawTaskCount[2]);
							this._cascadeDrawTaskId[2][num33] = (ushort)k;
							ushort[] cascadeAnimatedBlockDrawTaskCount4 = this._cascadeAnimatedBlockDrawTaskCount;
							int num34 = 2;
							cascadeAnimatedBlockDrawTaskCount4[num34] += 1;
						}
						bool flag28 = this._cascadedShadowMapping.CascadeNeedsUpdate[3] && containmentType12 != ContainmentType.Disjoint && containmentType11 != ContainmentType.Contains;
						if (flag28)
						{
							int num35 = (int)(this._cascadeAnimatedBlockDrawTaskCount[3] + this._cascadeChunkDrawTaskCount[3] + this._cascadeEntityDrawTaskCount[3]);
							this._cascadeDrawTaskId[3][num35] = (ushort)k;
							ushort[] cascadeAnimatedBlockDrawTaskCount5 = this._cascadeAnimatedBlockDrawTaskCount;
							int num36 = 3;
							cascadeAnimatedBlockDrawTaskCount5[num36] += 1;
						}
					}
				}
			}
		}

		// Token: 0x060053AE RID: 21422 RVA: 0x0017C568 File Offset: 0x0017A768
		private void DrawShadowCasters()
		{
			GLFunctions gl = this._graphics.GL;
			int num = this._renderTargetStore.ShadowMap.Width / this._cascadedShadowMapping.CascadesSettings.Count;
			bool useDrawInstanced = this._sunShadowCasting.UseDrawInstanced;
			if (useDrawInstanced)
			{
				int drawnVertices = gl.DrawnVertices;
				int drawCallsCount = gl.DrawCallsCount;
				float num2 = 1f / (float)this._cascadedShadowMapping.CascadesSettings.Count;
				this._gl.Viewport(0, 0, this._renderTargetStore.ShadowMap.Width, this._renderTargetStore.ShadowMap.Height);
				this.DrawEntityShadowCasters(-1);
				bool useChunkShadowCasters = this._sunShadowCasting.UseChunkShadowCasters;
				if (useChunkShadowCasters)
				{
					this.DrawMapChunkShadowCasters(-1);
					this.DrawMapBlockAnimatedShadowCasters(-1);
				}
				this._cascadeStats[0].KiloTriangleCount = (ushort)((gl.DrawnVertices - drawnVertices) / 3000);
				this._cascadeStats[0].DrawCallCount = (ushort)(gl.DrawCallsCount - drawCallsCount);
				for (int i = 1; i < this._cascadeStats.Length; i++)
				{
					this._cascadeStats[i].KiloTriangleCount = 0;
					this._cascadeStats[i].DrawCallCount = 0;
				}
			}
			else
			{
				for (int j = 0; j < this._cascadedShadowMapping.CascadesSettings.Count; j++)
				{
					bool flag = this._cascadedShadowMapping.CascadeNeedsUpdate[j];
					if (flag)
					{
						int x = j * num;
						int drawnVertices2 = gl.DrawnVertices;
						int drawCallsCount2 = gl.DrawCallsCount;
						this._gl.Viewport(x, 0, num, this._renderTargetStore.ShadowMap.Height);
						this.DrawEntityShadowCasters(j);
						bool useChunkShadowCasters2 = this._sunShadowCasting.UseChunkShadowCasters;
						if (useChunkShadowCasters2)
						{
							this.DrawMapChunkShadowCasters(j);
							this.DrawMapBlockAnimatedShadowCasters(j);
						}
						this._cascadeStats[j].KiloTriangleCount = (ushort)((gl.DrawnVertices - drawnVertices2) / 3000);
						this._cascadeStats[j].DrawCallCount = (ushort)(gl.DrawCallsCount - drawCallsCount2);
					}
					else
					{
						this._cascadeStats[j].KiloTriangleCount = 0;
						this._cascadeStats[j].DrawCallCount = 0;
					}
				}
			}
		}

		// Token: 0x060053AF RID: 21423 RVA: 0x0017C7D8 File Offset: 0x0017A9D8
		private void DrawEntityShadowCasters(int targetCascade = -1)
		{
			Debug.Assert(targetCascade != -1 || this._sunShadowCasting.UseDrawInstanced, "Invalid usage - either UseDrawInstanced, or specify a target shadow cascade.");
			Debug.Assert(targetCascade < this._cascadedShadowMapping.CascadesSettings.Count, string.Format("Invalid usage - impossible to draw cascade {0} when there are only {1}.", targetCascade, this._cascadedShadowMapping.CascadesSettings.Count));
			GLFunctions gl = this._graphics.GL;
			ZOnlyBlockyModelProgram blockyModelShadowMapProgram = this._gpuProgramStore.BlockyModelShadowMapProgram;
			gl.UseProgram(blockyModelShadowMapProgram);
			bool useDrawInstanced = this._sunShadowCasting.UseDrawInstanced;
			if (useDrawInstanced)
			{
				float x = 1f / (float)this._cascadedShadowMapping.CascadesSettings.Count;
				blockyModelShadowMapProgram.ViewportInfos.SetValue(x, (float)this._renderTargetStore.ShadowMap.Width);
				bool useBiasMethod = blockyModelShadowMapProgram.UseBiasMethod2;
				if (useBiasMethod)
				{
					blockyModelShadowMapProgram.ViewMatrix.SetValue(this.Data.SunShadowRenderData.VirtualSunViewRotationMatrix);
				}
				blockyModelShadowMapProgram.ViewProjectionMatrix.SetValue(this.Data.SunShadowRenderData.VirtualSunViewRotationProjectionMatrix);
			}
			else
			{
				bool useBiasMethod2 = blockyModelShadowMapProgram.UseBiasMethod2;
				if (useBiasMethod2)
				{
					blockyModelShadowMapProgram.ViewMatrix.SetValue(ref this.Data.SunShadowRenderData.VirtualSunViewRotationMatrix[targetCascade]);
				}
				blockyModelShadowMapProgram.ViewProjectionMatrix.SetValue(ref this.Data.SunShadowRenderData.VirtualSunViewRotationProjectionMatrix[targetCascade]);
			}
			bool flag = this._sunShadowCasting.UseDrawInstanced && this._cascadedShadowMapping.CascadesSettings.Count > 1;
			if (flag)
			{
				bool useEntitiesModelVFX = this._sunShadowCasting.UseEntitiesModelVFX;
				if (useEntitiesModelVFX)
				{
					blockyModelShadowMapProgram.Time.SetValue(this.Data.Time);
					for (int i = 0; i < (int)this._cascadeEntityDrawTaskCount[0]; i++)
					{
						ushort num = this._cascadeDrawTaskId[0][i];
						ref SceneRenderer.EntityShadowMapDrawTask ptr = ref this._entityShadowMapDrawTasks[(int)num];
						blockyModelShadowMapProgram.DrawId.SetValue((int)num);
						blockyModelShadowMapProgram.ModelVFXId.SetValue(ptr.ModelVFXId);
						blockyModelShadowMapProgram.NodeBlock.SetBufferRange(ptr.AnimationData, ptr.AnimationDataOffset, (uint)ptr.AnimationDataSize);
						gl.BindVertexArray(ptr.VertexArray);
						gl.DrawElementsInstanced(GL.TRIANGLES, ptr.DataCount, GL.UNSIGNED_SHORT, (IntPtr)0, (int)(ptr.CascadeFirstLast.Y - ptr.CascadeFirstLast.X + 1));
					}
				}
				else
				{
					for (int j = 0; j < (int)this._cascadeEntityDrawTaskCount[0]; j++)
					{
						ushort num2 = this._cascadeDrawTaskId[0][j];
						ref SceneRenderer.EntityShadowMapDrawTask ptr2 = ref this._entityShadowMapDrawTasks[(int)num2];
						blockyModelShadowMapProgram.DrawId.SetValue((int)num2);
						blockyModelShadowMapProgram.NodeBlock.SetBufferRange(ptr2.AnimationData, ptr2.AnimationDataOffset, (uint)ptr2.AnimationDataSize);
						gl.BindVertexArray(ptr2.VertexArray);
						gl.DrawElementsInstanced(GL.TRIANGLES, ptr2.DataCount, GL.UNSIGNED_SHORT, (IntPtr)0, (int)(ptr2.CascadeFirstLast.Y - ptr2.CascadeFirstLast.X + 1));
					}
				}
			}
			else
			{
				bool flag2 = this._sunShadowCasting.UseSmartCascadeDispatch && this._cascadedShadowMapping.CascadesSettings.Count > 1;
				if (flag2)
				{
					bool useEntitiesModelVFX2 = this._sunShadowCasting.UseEntitiesModelVFX;
					if (useEntitiesModelVFX2)
					{
						blockyModelShadowMapProgram.Time.SetValue(this.Data.Time);
						for (int k = 0; k < (int)this._cascadeEntityDrawTaskCount[targetCascade]; k++)
						{
							ushort num3 = this._cascadeDrawTaskId[targetCascade][k];
							ref SceneRenderer.EntityShadowMapDrawTask ptr3 = ref this._entityShadowMapDrawTasks[(int)num3];
							blockyModelShadowMapProgram.DrawId.SetValue((int)num3);
							blockyModelShadowMapProgram.ModelVFXId.SetValue(ptr3.ModelVFXId);
							blockyModelShadowMapProgram.NodeBlock.SetBufferRange(ptr3.AnimationData, ptr3.AnimationDataOffset, (uint)ptr3.AnimationDataSize);
							gl.BindVertexArray(ptr3.VertexArray);
							gl.DrawElements(GL.TRIANGLES, ptr3.DataCount, GL.UNSIGNED_SHORT, (IntPtr)0);
						}
					}
					else
					{
						for (int l = 0; l < (int)this._cascadeEntityDrawTaskCount[targetCascade]; l++)
						{
							ushort num4 = this._cascadeDrawTaskId[targetCascade][l];
							ref SceneRenderer.EntityShadowMapDrawTask ptr4 = ref this._entityShadowMapDrawTasks[(int)num4];
							blockyModelShadowMapProgram.DrawId.SetValue((int)num4);
							blockyModelShadowMapProgram.NodeBlock.SetBufferRange(ptr4.AnimationData, ptr4.AnimationDataOffset, (uint)ptr4.AnimationDataSize);
							gl.BindVertexArray(ptr4.VertexArray);
							gl.DrawElements(GL.TRIANGLES, ptr4.DataCount, GL.UNSIGNED_SHORT, (IntPtr)0);
						}
					}
				}
				else
				{
					bool useEntitiesModelVFX3 = this._sunShadowCasting.UseEntitiesModelVFX;
					if (useEntitiesModelVFX3)
					{
						blockyModelShadowMapProgram.Time.SetValue(this.Data.Time);
						for (int m = 0; m < this._entityShadowMapDrawTaskCount; m++)
						{
							ref SceneRenderer.EntityShadowMapDrawTask ptr5 = ref this._entityShadowMapDrawTasks[m];
							blockyModelShadowMapProgram.DrawId.SetValue(m);
							blockyModelShadowMapProgram.ModelVFXId.SetValue(ptr5.ModelVFXId);
							blockyModelShadowMapProgram.NodeBlock.SetBufferRange(ptr5.AnimationData, ptr5.AnimationDataOffset, (uint)ptr5.AnimationDataSize);
							gl.BindVertexArray(ptr5.VertexArray);
							gl.DrawElements(GL.TRIANGLES, ptr5.DataCount, GL.UNSIGNED_SHORT, (IntPtr)0);
						}
					}
					else
					{
						for (int n = 0; n < this._entityShadowMapDrawTaskCount; n++)
						{
							ref SceneRenderer.EntityShadowMapDrawTask ptr6 = ref this._entityShadowMapDrawTasks[n];
							blockyModelShadowMapProgram.DrawId.SetValue(n);
							blockyModelShadowMapProgram.NodeBlock.SetBufferRange(ptr6.AnimationData, ptr6.AnimationDataOffset, (uint)ptr6.AnimationDataSize);
							gl.BindVertexArray(ptr6.VertexArray);
							gl.DrawElements(GL.TRIANGLES, ptr6.DataCount, GL.UNSIGNED_SHORT, (IntPtr)0);
						}
					}
				}
			}
		}

		// Token: 0x060053B0 RID: 21424 RVA: 0x0017CE1C File Offset: 0x0017B01C
		private void DrawMapChunkShadowCasters(int targetCascade = -1)
		{
			Debug.Assert(targetCascade != -1 || this._sunShadowCasting.UseDrawInstanced, "Invalid usage - either UseDrawInstanced, or specify a target shadow cascade.");
			Debug.Assert(targetCascade < this._cascadedShadowMapping.CascadesSettings.Count, string.Format("Invalid usage - impossible to draw cascade {0} when there are only {1}.", targetCascade, this._cascadedShadowMapping.CascadesSettings.Count));
			GLFunctions gl = this._graphics.GL;
			gl.Disable(GL.CULL_FACE);
			ZOnlyChunkProgram mapChunkShadowMapProgram = this._gpuProgramStore.MapChunkShadowMapProgram;
			gl.UseProgram(mapChunkShadowMapProgram);
			bool useDrawInstanced = this._sunShadowCasting.UseDrawInstanced;
			if (useDrawInstanced)
			{
				float x = 1f / (float)this._cascadedShadowMapping.CascadesSettings.Count;
				mapChunkShadowMapProgram.ViewportInfos.SetValue(x, (float)this._renderTargetStore.ShadowMap.Width);
				mapChunkShadowMapProgram.ViewProjectionMatrix.SetValue(this.Data.SunShadowRenderData.VirtualSunViewRotationProjectionMatrix);
				mapChunkShadowMapProgram.LightPositions.SetValue(this.Data.SunShadowRenderData.VirtualSunPositions);
			}
			else
			{
				mapChunkShadowMapProgram.ViewProjectionMatrix.SetValue(ref this.Data.SunShadowRenderData.VirtualSunViewRotationProjectionMatrix[targetCascade]);
				mapChunkShadowMapProgram.LightPositions.SetValue(this.Data.SunShadowRenderData.VirtualSunPositions[targetCascade]);
			}
			mapChunkShadowMapProgram.Time.SetValue(this.Data.Time);
			bool flag = this._sunShadowCasting.UseDrawInstanced && this._cascadedShadowMapping.CascadesSettings.Count > 1;
			if (flag)
			{
				ushort num = this._cascadeEntityDrawTaskCount[0];
				for (int i = 0; i < (int)this._cascadeChunkDrawTaskCount[0]; i++)
				{
					ushort num2 = this._cascadeDrawTaskId[0][(int)num + i];
					ref SceneRenderer.ChunkShadowMapDrawTask ptr = ref this._chunkShadowMapDrawTasks[(int)num2];
					mapChunkShadowMapProgram.TargetCascades.SetValue((int)ptr.CascadeFirstLast.X, (int)ptr.CascadeFirstLast.Y);
					mapChunkShadowMapProgram.ModelMatrix.SetValue(ref ptr.ModelMatrix);
					gl.BindVertexArray(ptr.VertexArray);
					gl.DrawElementsInstanced(GL.TRIANGLES, ptr.DataCount, GL.UNSIGNED_INT, ptr.DataOffset, (int)(ptr.CascadeFirstLast.Y - ptr.CascadeFirstLast.X + 1));
				}
			}
			else
			{
				bool flag2 = this._sunShadowCasting.UseSmartCascadeDispatch && this._cascadedShadowMapping.CascadesSettings.Count > 1;
				if (flag2)
				{
					ushort num3 = this._cascadeEntityDrawTaskCount[targetCascade];
					mapChunkShadowMapProgram.TargetCascades.SetValue(targetCascade, 0);
					for (int j = 0; j < (int)this._cascadeChunkDrawTaskCount[targetCascade]; j++)
					{
						ushort num4 = this._cascadeDrawTaskId[targetCascade][(int)num3 + j];
						ref SceneRenderer.ChunkShadowMapDrawTask ptr2 = ref this._chunkShadowMapDrawTasks[(int)num4];
						mapChunkShadowMapProgram.ModelMatrix.SetValue(ref ptr2.ModelMatrix);
						gl.BindVertexArray(ptr2.VertexArray);
						gl.DrawElements(GL.TRIANGLES, ptr2.DataCount, GL.UNSIGNED_INT, ptr2.DataOffset);
					}
				}
				else
				{
					mapChunkShadowMapProgram.TargetCascades.SetValue(targetCascade, 0);
					for (int k = 0; k < this._chunkShadowMapDrawTaskCount; k++)
					{
						ref SceneRenderer.ChunkShadowMapDrawTask ptr3 = ref this._chunkShadowMapDrawTasks[k];
						mapChunkShadowMapProgram.ModelMatrix.SetValue(ref ptr3.ModelMatrix);
						gl.BindVertexArray(ptr3.VertexArray);
						gl.DrawElements(GL.TRIANGLES, ptr3.DataCount, GL.UNSIGNED_INT, ptr3.DataOffset);
					}
				}
			}
			gl.Enable(GL.CULL_FACE);
		}

		// Token: 0x060053B1 RID: 21425 RVA: 0x0017D1D4 File Offset: 0x0017B3D4
		private void DrawMapBlockAnimatedShadowCasters(int targetCascade = -1)
		{
			Debug.Assert(targetCascade != -1 || this._sunShadowCasting.UseDrawInstanced, "Invalid usage - either UseDrawInstanced, or specify a target shadow cascade.");
			Debug.Assert(targetCascade < this._cascadedShadowMapping.CascadesSettings.Count, string.Format("Invalid usage - impossible to draw cascade {0} when there are only {1}.", targetCascade, this._cascadedShadowMapping.CascadesSettings.Count));
			GLFunctions gl = this._graphics.GL;
			gl.Disable(GL.CULL_FACE);
			ZOnlyChunkProgram mapBlockAnimatedShadowMapProgram = this._gpuProgramStore.MapBlockAnimatedShadowMapProgram;
			gl.UseProgram(mapBlockAnimatedShadowMapProgram);
			bool useDrawInstanced = this._sunShadowCasting.UseDrawInstanced;
			if (useDrawInstanced)
			{
				float x = 1f / (float)this._cascadedShadowMapping.CascadesSettings.Count;
				mapBlockAnimatedShadowMapProgram.ViewportInfos.SetValue(x, (float)this._renderTargetStore.ShadowMap.Width);
				mapBlockAnimatedShadowMapProgram.ViewProjectionMatrix.SetValue(this.Data.SunShadowRenderData.VirtualSunViewRotationProjectionMatrix);
				mapBlockAnimatedShadowMapProgram.LightPositions.SetValue(this.Data.SunShadowRenderData.VirtualSunPositions);
			}
			else
			{
				mapBlockAnimatedShadowMapProgram.ViewProjectionMatrix.SetValue(ref this.Data.SunShadowRenderData.VirtualSunViewRotationProjectionMatrix[targetCascade]);
				mapBlockAnimatedShadowMapProgram.LightPositions.SetValue(this.Data.SunShadowRenderData.VirtualSunPositions[targetCascade]);
			}
			mapBlockAnimatedShadowMapProgram.Time.SetValue(this.Data.Time);
			bool flag = this._sunShadowCasting.UseDrawInstanced && this._cascadedShadowMapping.CascadesSettings.Count > 1;
			if (flag)
			{
				int num = (int)(this._cascadeChunkDrawTaskCount[0] + this._cascadeEntityDrawTaskCount[0]);
				for (int i = 0; i < (int)this._cascadeAnimatedBlockDrawTaskCount[0]; i++)
				{
					ushort num2 = this._cascadeDrawTaskId[0][num + i];
					ref SceneRenderer.AnimatedBlockShadowMapDrawTask ptr = ref this._animatedBlockShadowMapDrawTasks[(int)num2];
					mapBlockAnimatedShadowMapProgram.TargetCascades.SetValue((int)ptr.CascadeFirstLast.X, (int)ptr.CascadeFirstLast.Y);
					mapBlockAnimatedShadowMapProgram.ModelMatrix.SetValue(ref ptr.ModelMatrix);
					mapBlockAnimatedShadowMapProgram.NodeBlock.SetBufferRange(ptr.AnimationData, ptr.AnimationDataOffset, (uint)ptr.AnimationDataSize);
					gl.BindVertexArray(ptr.VertexArray);
					gl.DrawElementsInstanced(GL.TRIANGLES, ptr.DataCount, GL.UNSIGNED_INT, ptr.DataOffset, (int)(ptr.CascadeFirstLast.Y - ptr.CascadeFirstLast.X + 1));
				}
			}
			else
			{
				bool flag2 = this._sunShadowCasting.UseSmartCascadeDispatch && this._cascadedShadowMapping.CascadesSettings.Count > 1;
				if (flag2)
				{
					int num3 = (int)(this._cascadeChunkDrawTaskCount[targetCascade] + this._cascadeEntityDrawTaskCount[targetCascade]);
					mapBlockAnimatedShadowMapProgram.TargetCascades.SetValue(targetCascade, 0);
					for (int j = 0; j < (int)this._cascadeAnimatedBlockDrawTaskCount[targetCascade]; j++)
					{
						ushort num4 = this._cascadeDrawTaskId[targetCascade][num3 + j];
						ref SceneRenderer.AnimatedBlockShadowMapDrawTask ptr2 = ref this._animatedBlockShadowMapDrawTasks[(int)num4];
						mapBlockAnimatedShadowMapProgram.ModelMatrix.SetValue(ref ptr2.ModelMatrix);
						mapBlockAnimatedShadowMapProgram.NodeBlock.SetBufferRange(ptr2.AnimationData, ptr2.AnimationDataOffset, (uint)ptr2.AnimationDataSize);
						gl.BindVertexArray(ptr2.VertexArray);
						gl.DrawElements(GL.TRIANGLES, ptr2.DataCount, GL.UNSIGNED_INT, ptr2.DataOffset);
					}
				}
				else
				{
					mapBlockAnimatedShadowMapProgram.TargetCascades.SetValue(targetCascade, 0);
					for (int k = 0; k < this._animatedBlockShadowMapDrawTaskCount; k++)
					{
						ref SceneRenderer.AnimatedBlockShadowMapDrawTask ptr3 = ref this._animatedBlockShadowMapDrawTasks[k];
						mapBlockAnimatedShadowMapProgram.ModelMatrix.SetValue(ref ptr3.ModelMatrix);
						mapBlockAnimatedShadowMapProgram.NodeBlock.SetBufferRange(ptr3.AnimationData, ptr3.AnimationDataOffset, (uint)ptr3.AnimationDataSize);
						gl.BindVertexArray(ptr3.VertexArray);
						gl.DrawElements(GL.TRIANGLES, ptr3.DataCount, GL.UNSIGNED_INT, ptr3.DataOffset);
					}
				}
			}
			gl.Enable(GL.CULL_FACE);
		}

		// Token: 0x060053B2 RID: 21426 RVA: 0x0017D60C File Offset: 0x0017B80C
		public void UpdateSunShadowRenderData()
		{
			Vector3 vector = this._sunShadowCasting.Direction;
			this.Data.SunShadowRenderData.DynamicShadowIntensity = this._sunShadowCasting.ShadowIntensity;
			bool flag = this._sunShadowCasting.DirectionType == SceneRenderer.SunShadowCastingSettings.ShadowDirectionType.DynamicSun;
			if (flag)
			{
				float num = this.Data.SunPositionWS.Y + 0.2f;
				bool flag2 = num > 0f;
				if (flag2)
				{
					vector = -this.Data.SunPositionWS;
				}
				else
				{
					vector = this.Data.SunPositionWS;
				}
				this.Data.SunShadowRenderData.DynamicShadowIntensity = MathHelper.Lerp(this._sunShadowCasting.ShadowIntensity, 0.99f, 1f - MathHelper.Clamp(Math.Abs(num) * 10f, 0f, 1f));
			}
			bool useSafeAngle = this._sunShadowCasting.UseSafeAngle;
			if (useSafeAngle)
			{
				vector = Vector3.Lerp(vector, Vector3.Down, 0.35f);
			}
			CascadedShadowMapping.InputParams inputParams;
			inputParams.LightDirection = vector;
			inputParams.WorldFieldOfView = this.Data.WorldFieldOfView;
			inputParams.AspectRatio = this.Data.AspectRatio;
			inputParams.NearClipDistance = 0.1f;
			inputParams.CameraPosition = this.Data.CameraPosition;
			inputParams.ViewRotationMatrix = this.Data.ViewRotationMatrix;
			inputParams.ViewRotationProjectionMatrix = this.Data.ViewRotationProjectionMatrix;
			inputParams.IsSpatialContinuityLost = this.IsSpatialContinuityLost();
			inputParams.QuantifiedCameraMotion = this.QuantifyCameraMotion();
			inputParams.CameraPositionDelta = this.Data.CameraPosition - this.PreviousData.CameraPosition;
			inputParams.FrameId = this.Data.FrameCounter;
			this._cascadedShadowMapping.Update(ref inputParams, ref this.Data.SunShadowRenderData);
		}

		// Token: 0x060053B3 RID: 21427 RVA: 0x0017D7DF File Offset: 0x0017B9DF
		public void BuildShadowMap()
		{
			this._cascadedShadowMapping.BuildShadowMap();
		}

		// Token: 0x060053B4 RID: 21428 RVA: 0x0017D7ED File Offset: 0x0017B9ED
		public void DrawDeferredShadow()
		{
			this._cascadedShadowMapping.DrawDeferredShadow(this.SceneDataBuffer, this.Data.FrustumFarCornersWS);
		}

		// Token: 0x170012EF RID: 4847
		// (get) Token: 0x060053B5 RID: 21429 RVA: 0x0017D80C File Offset: 0x0017BA0C
		public bool NeedsDebugDrawShadowRelated
		{
			get
			{
				return this._cascadedShadowMapping.NeedsDebugDrawShadowRelated;
			}
		}

		// Token: 0x060053B6 RID: 21430 RVA: 0x0017D819 File Offset: 0x0017BA19
		public void ToggleFreeze()
		{
			this._cascadedShadowMapping.ToggleFreeze();
		}

		// Token: 0x060053B7 RID: 21431 RVA: 0x0017D827 File Offset: 0x0017BA27
		public void ToggleCameraFrustumDebug()
		{
			this._cascadedShadowMapping.ToggleCameraFrustumDebug(this.Data.CameraPosition);
		}

		// Token: 0x060053B8 RID: 21432 RVA: 0x0017D840 File Offset: 0x0017BA40
		public void ToggleCameraFrustumSplitsDebug()
		{
			this._cascadedShadowMapping.ToggleCameraFrustumSplitsDebug(this.Data.CameraPosition);
		}

		// Token: 0x060053B9 RID: 21433 RVA: 0x0017D859 File Offset: 0x0017BA59
		public void ToggleShadowCascadeFrustumDebug()
		{
			this._cascadedShadowMapping.ToggleShadowCascadeFrustumDebug(this.Data.CameraPosition);
		}

		// Token: 0x060053BA RID: 21434 RVA: 0x0017D872 File Offset: 0x0017BA72
		public void DebugDrawShadowRelated()
		{
			this._cascadedShadowMapping.DebugDrawShadowRelated(ref this.Data.ViewProjectionMatrix);
		}

		// Token: 0x060053BB RID: 21435 RVA: 0x0017D88C File Offset: 0x0017BA8C
		public void ToggleSunShadowMapCascadeDebug()
		{
			DeferredProgram deferredProgram = this._gpuProgramStore.DeferredProgram;
			deferredProgram.CascadeCount = (uint)this._cascadedShadowMapping.CascadesSettings.Count;
			deferredProgram.DebugShadowCascades = !deferredProgram.DebugShadowCascades;
			deferredProgram.Reset(true);
		}

		// Token: 0x060053BC RID: 21436 RVA: 0x0017D8D4 File Offset: 0x0017BAD4
		public string WriteShadowMappingStateToString()
		{
			string str = "ShadowMapping state :";
			str = str + "\n.Enabled: " + this.UseSunShadows.ToString();
			str = str + "\n.Intensity: " + this._sunShadowCasting.ShadowIntensity.ToString();
			str = str + "\n.Light dir.: " + this._sunShadowCasting.DirectionType.ToString();
			str = str + "\n.Safe angle: " + this._sunShadowCasting.UseSafeAngle.ToString();
			str = str + "\n.Chunks shadow: " + this._sunShadowCasting.UseChunkShadowCasters.ToString();
			str = str + "\n.ModelVFX shadow: " + this._sunShadowCasting.UseEntitiesModelVFX.ToString();
			string text = this._gpuProgramStore.BlockyModelShadowMapProgram.UseBiasMethod1 ? "model hack#1, " : "";
			text += (this._gpuProgramStore.BlockyModelShadowMapProgram.UseBiasMethod2 ? "model hack#2, " : "");
			str = str + "\n.Bias methods: " + text;
			str = str + "\n.Draw Instanced: " + this._sunShadowCasting.UseDrawInstanced.ToString();
			str = str + "\n.Cascade smart dispatch: " + this._sunShadowCasting.UseSmartCascadeDispatch.ToString();
			return str + "\n" + this._cascadedShadowMapping.WriteShadowMappingStateToString();
		}

		// Token: 0x04002DFE RID: 11774
		private RenderTargetStore _renderTargetStore;

		// Token: 0x04002DFF RID: 11775
		public const float NearClippingPlane = 0.1f;

		// Token: 0x04002E00 RID: 11776
		public const float FarClippingPlane = 1024f;

		// Token: 0x04002E01 RID: 11777
		public SceneRenderer.SceneData Data;

		// Token: 0x04002E02 RID: 11778
		public SceneRenderer.SceneData PreviousData;

		// Token: 0x04002E03 RID: 11779
		public bool UseSSAOBlur = true;

		// Token: 0x04002E05 RID: 11781
		public float SSAOParamOcclusionMax;

		// Token: 0x04002E06 RID: 11782
		public float SSAOParamOcclusionStrength;

		// Token: 0x04002E07 RID: 11783
		public float SSAOParamRadius;

		// Token: 0x04002E08 RID: 11784
		private float _ssaoParamRadiusProjectedScale;

		// Token: 0x04002E09 RID: 11785
		private Vector4 _ssaoPackedParameters;

		// Token: 0x04002E0A RID: 11786
		public static Vector2 DefaultSSAOResolutionScale = new Vector2(0.5f, 0.5f);

		// Token: 0x04002E0C RID: 11788
		private Vector2 _ssaoResolutionScale = SceneRenderer.DefaultSSAOResolutionScale;

		// Token: 0x04002E0D RID: 11789
		private RenderTarget _ssaoTapsSource;

		// Token: 0x04002E0E RID: 11790
		private int _ssaoSamplesCount = 4;

		// Token: 0x04002E0F RID: 11791
		private Vector2[] _ssaoSamplesData = new Vector2[16];

		// Token: 0x04002E10 RID: 11792
		private float _ssaoTemporalSampleOffset;

		// Token: 0x04002E11 RID: 11793
		private bool _useSkyAmbient = true;

		// Token: 0x04002E12 RID: 11794
		private readonly Profiling Profiling;

		// Token: 0x04002E13 RID: 11795
		private int _renderingProfileLinearZ;

		// Token: 0x04002E14 RID: 11796
		private int _renderingProfileLinearZDownsample;

		// Token: 0x04002E15 RID: 11797
		private int _renderingProfileZDownsample;

		// Token: 0x04002E16 RID: 11798
		private int _renderingProfileEdgeDetection;

		// Token: 0x04002E17 RID: 11799
		private const int FirstPersonMinFov = 40;

		// Token: 0x04002E18 RID: 11800
		private const int FirstPersonMaxFov = 70;

		// Token: 0x04002E19 RID: 11801
		private GLSampler _pointSampler;

		// Token: 0x04002E1A RID: 11802
		private GLSampler _linearSampler;

		// Token: 0x04002E1B RID: 11803
		private readonly GraphicsDevice _graphics;

		// Token: 0x04002E1C RID: 11804
		private readonly GPUProgramStore _gpuProgramStore;

		// Token: 0x04002E1D RID: 11805
		private readonly GLFunctions _gl;

		// Token: 0x04002E1E RID: 11806
		private bool _hasDownsampledZ;

		// Token: 0x04002E1F RID: 11807
		public OrderIndependentTransparency OIT;

		// Token: 0x04002E20 RID: 11808
		public const byte EdgesStencilBit = 7;

		// Token: 0x04002E21 RID: 11809
		private GPUBuffer _sceneDataBuffer;

		// Token: 0x04002E22 RID: 11810
		private GLVertexArray _emptyVAO;

		// Token: 0x04002E23 RID: 11811
		private const uint SceneDataBufferSize = 1248U;

		// Token: 0x04002E24 RID: 11812
		private QuadRenderer _quadRenderer;

		// Token: 0x04002E25 RID: 11813
		private BoxRenderer _boxRenderer;

		// Token: 0x04002E26 RID: 11814
		private LineRenderer _lineRenderer;

		// Token: 0x04002E27 RID: 11815
		private GPUBufferTexture _modelVFXDataBufferTexture;

		// Token: 0x04002E28 RID: 11816
		public static uint ModelVFXDataSize = (uint)(Marshal.SizeOf(typeof(Vector4)) * 4);

		// Token: 0x04002E29 RID: 11817
		private const int ModelVFXBufferDefaultSize = 500;

		// Token: 0x04002E2A RID: 11818
		private const int ModelVFXBufferGrowth = 200;

		// Token: 0x04002E2B RID: 11819
		private SceneRenderer.ModelVFXDrawTask[] _modelVFXDrawTasks = new SceneRenderer.ModelVFXDrawTask[500];

		// Token: 0x04002E2C RID: 11820
		private int _modelVFXDrawTaskCount;

		// Token: 0x04002E2D RID: 11821
		private uint _modelVFXBufferSize = SceneRenderer.ModelVFXDataSize * 256U;

		// Token: 0x04002E2E RID: 11822
		private GPUBufferTexture _entityDataBufferTexture;

		// Token: 0x04002E2F RID: 11823
		public static readonly int GPUEntityDataSize = Marshal.SizeOf(typeof(Matrix)) + Marshal.SizeOf(typeof(Vector4)) * 4;

		// Token: 0x04002E30 RID: 11824
		private const uint EntityBufferGrowth = 1024U;

		// Token: 0x04002E31 RID: 11825
		private uint _entityBufferSize = (uint)(SceneRenderer.GPUEntityDataSize * 2048);

		// Token: 0x04002E32 RID: 11826
		private const int EntityDrawTasksDefaultSize = 500;

		// Token: 0x04002E33 RID: 11827
		private const int EntityDrawTasksGrowth = 200;

		// Token: 0x04002E34 RID: 11828
		private SceneRenderer.EntityDrawTask[] _entityDrawTasks = new SceneRenderer.EntityDrawTask[500];

		// Token: 0x04002E35 RID: 11829
		private int _incomingEntityDrawTaskCount;

		// Token: 0x04002E36 RID: 11830
		private int _entityDrawTaskCount;

		// Token: 0x04002E37 RID: 11831
		private SceneRenderer.EntityDrawTask[] _entityForwardDrawTasks = new SceneRenderer.EntityDrawTask[500];

		// Token: 0x04002E38 RID: 11832
		private int _entityForwardDrawTaskCount;

		// Token: 0x04002E39 RID: 11833
		public static readonly int GPUEntityDistortionDataSize = Marshal.SizeOf(typeof(Matrix)) + Marshal.SizeOf(typeof(Vector4));

		// Token: 0x04002E3A RID: 11834
		private const uint EntityDistortionBufferGrowth = 1024U;

		// Token: 0x04002E3B RID: 11835
		private uint _entityDistortionBufferSize = (uint)(SceneRenderer.GPUEntityDataSize * 2048);

		// Token: 0x04002E3C RID: 11836
		private const int EntityDistortionDrawTasksDefaultSize = 500;

		// Token: 0x04002E3D RID: 11837
		private const int EntityDistortionDrawTasksGrowth = 200;

		// Token: 0x04002E3E RID: 11838
		private SceneRenderer.EntityDistortionDrawTask[] _entityDistortionDrawTasks = new SceneRenderer.EntityDistortionDrawTask[500];

		// Token: 0x04002E3F RID: 11839
		private int _entityDistortionDrawTaskCount;

		// Token: 0x04002E40 RID: 11840
		private const int NameplateDrawTasksDefaultSize = 100;

		// Token: 0x04002E41 RID: 11841
		private const int NameplateDrawTasksGrowth = 50;

		// Token: 0x04002E42 RID: 11842
		private SceneRenderer.NameplateDrawTask[] _nameplateDrawTasks = new SceneRenderer.NameplateDrawTask[100];

		// Token: 0x04002E43 RID: 11843
		private int _nameplateDrawTaskCount;

		// Token: 0x04002E44 RID: 11844
		private const int DebugInfoDrawTasksDefaultSize = 100;

		// Token: 0x04002E45 RID: 11845
		private const int DebugInfoDrawTasksGrowth = 50;

		// Token: 0x04002E46 RID: 11846
		private int _debugInfoDrawTaskCount;

		// Token: 0x04002E47 RID: 11847
		private SceneRenderer.DebugInfoDrawTask[] _debugInfoDrawTasks = new SceneRenderer.DebugInfoDrawTask[100];

		// Token: 0x04002E48 RID: 11848
		public bool UseDynamicLightResolutionSelection = true;

		// Token: 0x04002E49 RID: 11849
		public bool UseLinearZForLighting = true;

		// Token: 0x04002E4A RID: 11850
		public bool UseLightBlendMax = true;

		// Token: 0x04002E4B RID: 11851
		public bool UseClusteredLighting = true;

		// Token: 0x04002E4C RID: 11852
		public ClusteredLighting ClusteredLighting;

		// Token: 0x04002E4D RID: 11853
		public ClassicDeferredLighting ClassicDeferredLighting;

		// Token: 0x04002E4E RID: 11854
		private RenderTarget _lightBuffer;

		// Token: 0x04002E4F RID: 11855
		private SceneRenderer.LightingResolution _lightResolution = SceneRenderer.LightingResolution.FULL;

		// Token: 0x04002E50 RID: 11856
		private bool _requestLinearZ = true;

		// Token: 0x04002E51 RID: 11857
		private bool _requestDownsampledLinearZ = true;

		// Token: 0x04002E52 RID: 11858
		private bool _useLBufferCompression;

		// Token: 0x04002E53 RID: 11859
		private Mesh _sphereLightMesh;

		// Token: 0x04002E54 RID: 11860
		private Mesh _cylinderMesh;

		// Token: 0x04002E55 RID: 11861
		private int _renderingProfileLights;

		// Token: 0x04002E56 RID: 11862
		private int _renderingProfileLightsFullRes;

		// Token: 0x04002E57 RID: 11863
		private int _renderingProfileLightsLowRes;

		// Token: 0x04002E58 RID: 11864
		private int _renderingProfileLightsStencil;

		// Token: 0x04002E59 RID: 11865
		private int _renderingProfileLightsMix;

		// Token: 0x04002E5A RID: 11866
		private const int OpaqueDrawTasksDefaultSize = 400;

		// Token: 0x04002E5B RID: 11867
		private const int OpaqueDrawTasksGrowth = 100;

		// Token: 0x04002E5C RID: 11868
		private SceneRenderer.ChunkDrawTask[] _opaqueDrawTasks = new SceneRenderer.ChunkDrawTask[400];

		// Token: 0x04002E5D RID: 11869
		private int _opaqueDrawTaskCount;

		// Token: 0x04002E5E RID: 11870
		private const int AlphaBlendedDrawTasksDefaultSize = 200;

		// Token: 0x04002E5F RID: 11871
		private const int AlphaBlendedDrawTasksGrowth = 50;

		// Token: 0x04002E60 RID: 11872
		private SceneRenderer.ChunkDrawTask[] _alphaBlendedDrawTasks = new SceneRenderer.ChunkDrawTask[200];

		// Token: 0x04002E61 RID: 11873
		private int _alphaBlendedDrawTaskCount;

		// Token: 0x04002E62 RID: 11874
		private const int AlphaTestedDrawTasksDefaultSize = 400;

		// Token: 0x04002E63 RID: 11875
		private const int AlphaTestedDrawTasksGrowth = 100;

		// Token: 0x04002E64 RID: 11876
		private SceneRenderer.ChunkDrawTask[] _alphaTestedDrawTasks = new SceneRenderer.ChunkDrawTask[400];

		// Token: 0x04002E65 RID: 11877
		private int _alphaTestedDrawTaskCount;

		// Token: 0x04002E66 RID: 11878
		private readonly byte ChunkDrawTagOpaque = 1;

		// Token: 0x04002E67 RID: 11879
		private readonly byte ChunkDrawTagAlphaTested = 2;

		// Token: 0x04002E68 RID: 11880
		private readonly byte ChunkDrawTagAlphaBlended = 4;

		// Token: 0x04002E69 RID: 11881
		private readonly byte ChunkDrawTagAnimated = 8;

		// Token: 0x04002E6A RID: 11882
		private int _farProgramOpaqueChunkStartIndex;

		// Token: 0x04002E6B RID: 11883
		private int _nearProgramAlphaBlendedChunkStartIndex = -1;

		// Token: 0x04002E6C RID: 11884
		private int _farProgramAlphaTestedChunkStartIndex;

		// Token: 0x04002E6D RID: 11885
		private const int AnimatedBlockDrawTasksDefaultSize = 100;

		// Token: 0x04002E6E RID: 11886
		private const int AnimatedBlockDrawTasksGrowth = 25;

		// Token: 0x04002E6F RID: 11887
		private SceneRenderer.AnimatedBlockDrawTask[] _animatedBlockDrawTasks = new SceneRenderer.AnimatedBlockDrawTask[100];

		// Token: 0x04002E70 RID: 11888
		private int _animatedBlockDrawTaskCount;

		// Token: 0x04002E71 RID: 11889
		private readonly Vector3 _chunkSize = new Vector3(32f);

		// Token: 0x04002E72 RID: 11890
		private readonly Vector3 _chunkHalfSize = new Vector3(16f);

		// Token: 0x04002E73 RID: 11891
		private float _nearChunkDistance = 64f;

		// Token: 0x04002E74 RID: 11892
		public int[] VisibleOccludees;

		// Token: 0x04002E75 RID: 11893
		private const byte MaxAlphaTestedOccluders = 10;

		// Token: 0x04002E76 RID: 11894
		private const byte MaxOpaqueOccluders = 100;

		// Token: 0x04002E77 RID: 11895
		private byte[] _opaqueOccludersIDs = new byte[100];

		// Token: 0x04002E78 RID: 11896
		private byte _opaqueOccludersCount;

		// Token: 0x04002E79 RID: 11897
		private const int MaxOccludees = 2000;

		// Token: 0x04002E7A RID: 11898
		private const int OccludeesGrowth = 500;

		// Token: 0x04002E7B RID: 11899
		private OcclusionCulling.OccludeeData[] _occludeesData = new OcclusionCulling.OccludeeData[2000];

		// Token: 0x04002E7C RID: 11900
		private int _opaqueOccludeesCount;

		// Token: 0x04002E7D RID: 11901
		private Vector3[] _opaqueOccludeesData = new Vector3[400];

		// Token: 0x04002E7E RID: 11902
		private int _alphaTestedOccludeesCount;

		// Token: 0x04002E7F RID: 11903
		private Vector3[] _alphaTestedOccludeesData = new Vector3[400];

		// Token: 0x04002E80 RID: 11904
		private int _alphaBlendedOccludeesCount;

		// Token: 0x04002E81 RID: 11905
		private Vector3[] _alphaBlendedOccludeesData = new Vector3[200];

		// Token: 0x04002E82 RID: 11906
		private int _entitiesOccludeesCount;

		// Token: 0x04002E83 RID: 11907
		private BoundingBox[] _entitiesOccludeesData = new BoundingBox[1000];

		// Token: 0x04002E84 RID: 11908
		private int _lightOccludeesCount;

		// Token: 0x04002E85 RID: 11909
		private BoundingBox[] _lightOccludeesData = new BoundingBox[1000];

		// Token: 0x04002E86 RID: 11910
		private int _particleOccludeesCount;

		// Token: 0x04002E87 RID: 11911
		private BoundingBox[] _particleOccludeesData = new BoundingBox[1000];

		// Token: 0x04002E88 RID: 11912
		private const int MaxOccluderPlanes = 2000;

		// Token: 0x04002E89 RID: 11913
		private const int OccluderPlanesGrowth = 1000;

		// Token: 0x04002E8A RID: 11914
		private Vector4[] _occluderPlanes = new Vector4[2000];

		// Token: 0x04002E8B RID: 11915
		private int _occluderPlanesCount;

		// Token: 0x04002E8C RID: 11916
		private GLVertexArray _occluderPlanesVertexArray;

		// Token: 0x04002E8D RID: 11917
		private GLBuffer _occluderPlanesVerticesBuffer;

		// Token: 0x04002E8E RID: 11918
		private GLBuffer _occluderPlanesIndicesBuffer;

		// Token: 0x04002E8F RID: 11919
		private SceneRenderer.ChunkOcclusionCullingSetup _occlusionCullingSetup;

		// Token: 0x04002E90 RID: 11920
		private CascadedShadowMapping _cascadedShadowMapping;

		// Token: 0x04002E91 RID: 11921
		private bool UseSunShadows;

		// Token: 0x04002E92 RID: 11922
		private SceneRenderer.SunShadowCastingSettings _sunShadowCasting;

		// Token: 0x04002E93 RID: 11923
		private GPUBufferTexture _entityShadowMapDataBufferTexture;

		// Token: 0x04002E94 RID: 11924
		public static readonly int GPUEntityShadowMapDataSize = Marshal.SizeOf(typeof(Matrix)) + Marshal.SizeOf(typeof(Vector4)) * 2;

		// Token: 0x04002E95 RID: 11925
		private const uint EntityShadowMapBufferGrowth = 1024U;

		// Token: 0x04002E96 RID: 11926
		private uint _entityShadowMapBufferSize = (uint)(SceneRenderer.GPUEntityShadowMapDataSize * 2048);

		// Token: 0x04002E97 RID: 11927
		private const int EntityShadowMapDrawTasksDefaultSize = 500;

		// Token: 0x04002E98 RID: 11928
		private const int EntityShadowMapDrawTasksGrowth = 200;

		// Token: 0x04002E99 RID: 11929
		private SceneRenderer.EntityShadowMapDrawTask[] _entityShadowMapDrawTasks = new SceneRenderer.EntityShadowMapDrawTask[500];

		// Token: 0x04002E9A RID: 11930
		private BoundingSphere[] _entitiesBoundingVolumes = new BoundingSphere[500];

		// Token: 0x04002E9B RID: 11931
		private int _incomingEntityShadowMapDrawTaskCount;

		// Token: 0x04002E9C RID: 11932
		private int _entityShadowMapDrawTaskCount;

		// Token: 0x04002E9D RID: 11933
		private const int ChunkShadowMapDrawTasksDefaultSize = 500;

		// Token: 0x04002E9E RID: 11934
		private const int ChunkShadowMapDrawTasksGrowth = 200;

		// Token: 0x04002E9F RID: 11935
		private SceneRenderer.ChunkShadowMapDrawTask[] _chunkShadowMapDrawTasks = new SceneRenderer.ChunkShadowMapDrawTask[500];

		// Token: 0x04002EA0 RID: 11936
		private int _incomingChunkShadowMapDrawTaskCount;

		// Token: 0x04002EA1 RID: 11937
		private int _chunkShadowMapDrawTaskCount;

		// Token: 0x04002EA2 RID: 11938
		private const int AnimatedBlockShadowMapDrawTasksDefaultSize = 500;

		// Token: 0x04002EA3 RID: 11939
		private const int AnimatedBlockShadowMapDrawTasksGrowth = 200;

		// Token: 0x04002EA4 RID: 11940
		private SceneRenderer.AnimatedBlockShadowMapDrawTask[] _animatedBlockShadowMapDrawTasks = new SceneRenderer.AnimatedBlockShadowMapDrawTask[500];

		// Token: 0x04002EA5 RID: 11941
		private BoundingBox[] _animatedBlockBoundingVolumes = new BoundingBox[500];

		// Token: 0x04002EA6 RID: 11942
		private int _incomingAnimatedBlockShadowMapDrawTaskCount;

		// Token: 0x04002EA7 RID: 11943
		private int _animatedBlockShadowMapDrawTaskCount;

		// Token: 0x04002EA8 RID: 11944
		private const int CascadeDrawTasksDefaultSize = 1000;

		// Token: 0x04002EA9 RID: 11945
		private const int CascadeDrawTasksGrowth = 500;

		// Token: 0x04002EAA RID: 11946
		private ushort[][] _cascadeDrawTaskId = new ushort[4][];

		// Token: 0x04002EAB RID: 11947
		private ushort[] _cascadeEntityDrawTaskCount = new ushort[4];

		// Token: 0x04002EAC RID: 11948
		private ushort[] _cascadeChunkDrawTaskCount = new ushort[4];

		// Token: 0x04002EAD RID: 11949
		private ushort[] _cascadeAnimatedBlockDrawTaskCount = new ushort[4];

		// Token: 0x04002EAE RID: 11950
		private SceneRenderer.ShadowCascadeStats[] _cascadeStats = new SceneRenderer.ShadowCascadeStats[4];

		// Token: 0x02000EBD RID: 3773
		public struct SceneData
		{
			// Token: 0x060067EE RID: 26606 RVA: 0x00219634 File Offset: 0x00217834
			public void Init(int width = 1, int height = 1)
			{
				this.SetViewportSize(width, height);
				this.FrameCounter = 0U;
				this.WorldFieldOfView = 1.5707964f;
				this.AspectRatio = 1.3333334f;
				this.ViewRotationMatrix = Matrix.Identity;
				this.ViewRotationProjectionMatrix = Matrix.Identity;
				this.InvViewRotationProjectionMatrix = Matrix.Identity;
				this.InvViewRotationMatrix = Matrix.Identity;
				this.InvViewMatrix = Matrix.Identity;
				this.InvViewProjectionMatrix = Matrix.Identity;
				this.ViewMatrix = Matrix.Identity;
				this.ViewProjectionMatrix = Matrix.Identity;
				this.ProjectionMatrix = Matrix.Identity;
				this.FirstPersonViewMatrix = Matrix.Identity;
				this.FirstPersonProjectionMatrix = Matrix.Identity;
				this.ReprojectFromCurrentViewToPreviousProjectionMatrix = Matrix.Identity;
				this.ReprojectFromPreviousViewToCurrentProjection = Matrix.Identity;
				this.ProjectionJittering = Vector2.Zero;
				this.HasCameraMoved = true;
				this.IsCameraUnderwater = false;
				this.CameraPosition = Vector3.Zero;
				this.CameraDirection = Vector3.Zero;
				this.ViewFrustum = new BoundingFrustum(Matrix.Identity);
				this.RelativeViewFrustum = new BoundingFrustum(Matrix.Identity);
				this.FrustumFarCornersWS = new Vector3[4];
				this.FrustumFarCornersVS = new Vector3[4];
				this.SunShadowRenderData.VirtualSunViewFrustum = new BoundingFrustum(Matrix.Identity);
				this.SunShadowRenderData.VirtualSunKDopFrustum = new KDop(13);
				this.SunShadowRenderData.VirtualSunPositions = new Vector3[4];
				this.SunShadowRenderData.VirtualSunViewRotationMatrix = new Matrix[4];
				this.SunShadowRenderData.VirtualSunProjectionMatrix = new Matrix[4];
				this.SunShadowRenderData.VirtualSunViewRotationProjectionMatrix = new Matrix[4];
				this.SunShadowRenderData.CascadeDistanceAndTexelScales = new Vector2[4];
				this.SunShadowRenderData.CascadeCachedTranslations = new Vector3[4];
				this.SunColor = Vector3.One;
				this.SunLightColor = Vector4.One;
				this.SunPositionWS = Vector3.One;
				this.SunPositionVS = Vector3.One;
				this.AmbientFrontColor = Vector3.One;
				this.AmbientBackColor = Vector3.One;
				this.AmbientIntensity = 0f;
				this.FogTopColor = Vector3.One;
				this.FogFrontColor = Vector3.One;
				this.FogBackColor = Vector3.One;
				this.FogParams = Vector4.One;
				this.FogHeightFalloffUnderwater = 4f;
				this.FogDensityUnderwater = 0.3f;
				this.WaterCausticsAnimTime = 0f;
				this.WaterCausticsDistortion = 0f;
				this.WaterCausticsScale = 0.05f;
				this.WaterCausticsIntensity = 1f;
				this.CloudsShadowAnimTime = 0f;
				this.CloudsShadowBlurriness = 3.5f;
				this.CloudsShadowScale = 0.005f;
				this.CloudsShadowIntensity = 0.5f;
			}

			// Token: 0x060067EF RID: 26607 RVA: 0x002198D2 File Offset: 0x00217AD2
			public void SetViewportSize(int width, int height)
			{
				this.ViewportSize = new Vector2((float)width, (float)height);
				this.InvViewportSize = new Vector2((float)(1.0 / (double)width), (float)(1.0 / (double)height));
			}

			// Token: 0x04004801 RID: 18433
			public float Time;

			// Token: 0x04004802 RID: 18434
			public float DeltaTime;

			// Token: 0x04004803 RID: 18435
			public uint FrameCounter;

			// Token: 0x04004804 RID: 18436
			public float WorldFieldOfView;

			// Token: 0x04004805 RID: 18437
			public float AspectRatio;

			// Token: 0x04004806 RID: 18438
			public Vector2 ViewportSize;

			// Token: 0x04004807 RID: 18439
			public Vector2 InvViewportSize;

			// Token: 0x04004808 RID: 18440
			public Matrix ViewRotationMatrix;

			// Token: 0x04004809 RID: 18441
			public Matrix ViewRotationProjectionMatrix;

			// Token: 0x0400480A RID: 18442
			public Matrix InvViewRotationProjectionMatrix;

			// Token: 0x0400480B RID: 18443
			public Matrix InvViewRotationMatrix;

			// Token: 0x0400480C RID: 18444
			public Matrix InvViewMatrix;

			// Token: 0x0400480D RID: 18445
			public Matrix InvViewProjectionMatrix;

			// Token: 0x0400480E RID: 18446
			public Matrix ViewMatrix;

			// Token: 0x0400480F RID: 18447
			public Matrix ViewProjectionMatrix;

			// Token: 0x04004810 RID: 18448
			public Matrix ProjectionMatrix;

			// Token: 0x04004811 RID: 18449
			public Matrix FirstPersonViewMatrix;

			// Token: 0x04004812 RID: 18450
			public Matrix FirstPersonProjectionMatrix;

			// Token: 0x04004813 RID: 18451
			public Matrix ReprojectFromCurrentViewToPreviousProjectionMatrix;

			// Token: 0x04004814 RID: 18452
			public Matrix ReprojectFromPreviousViewToCurrentProjection;

			// Token: 0x04004815 RID: 18453
			public Vector2 ProjectionJittering;

			// Token: 0x04004816 RID: 18454
			public bool HasCameraMoved;

			// Token: 0x04004817 RID: 18455
			public bool IsCameraUnderwater;

			// Token: 0x04004818 RID: 18456
			public Vector3 CameraPosition;

			// Token: 0x04004819 RID: 18457
			public Vector3 CameraDirection;

			// Token: 0x0400481A RID: 18458
			public Vector3 PlayerRenderPosition;

			// Token: 0x0400481B RID: 18459
			public BoundingFrustum ViewFrustum;

			// Token: 0x0400481C RID: 18460
			public BoundingFrustum RelativeViewFrustum;

			// Token: 0x0400481D RID: 18461
			public Vector3[] FrustumFarCornersWS;

			// Token: 0x0400481E RID: 18462
			public Vector3[] FrustumFarCornersVS;

			// Token: 0x0400481F RID: 18463
			public CascadedShadowMapping.RenderData SunShadowRenderData;

			// Token: 0x04004820 RID: 18464
			public Vector3 SunColor;

			// Token: 0x04004821 RID: 18465
			public Vector4 SunLightColor;

			// Token: 0x04004822 RID: 18466
			public Vector3 SunPositionWS;

			// Token: 0x04004823 RID: 18467
			public Vector3 SunPositionVS;

			// Token: 0x04004824 RID: 18468
			public Vector3 AmbientFrontColor;

			// Token: 0x04004825 RID: 18469
			public Vector3 AmbientBackColor;

			// Token: 0x04004826 RID: 18470
			public float AmbientIntensity;

			// Token: 0x04004827 RID: 18471
			public Vector3 FogTopColor;

			// Token: 0x04004828 RID: 18472
			public Vector3 FogFrontColor;

			// Token: 0x04004829 RID: 18473
			public Vector3 FogBackColor;

			// Token: 0x0400482A RID: 18474
			public Vector4 FogParams;

			// Token: 0x0400482B RID: 18475
			public float FogHeightFalloffUnderwater;

			// Token: 0x0400482C RID: 18476
			public float FogDensityUnderwater;

			// Token: 0x0400482D RID: 18477
			public Vector4 FogMoodParams;

			// Token: 0x0400482E RID: 18478
			public float FogHeightDensityAtViewer;

			// Token: 0x0400482F RID: 18479
			public float WaterCausticsAnimTime;

			// Token: 0x04004830 RID: 18480
			public float WaterCausticsDistortion;

			// Token: 0x04004831 RID: 18481
			public float WaterCausticsScale;

			// Token: 0x04004832 RID: 18482
			public float WaterCausticsIntensity;

			// Token: 0x04004833 RID: 18483
			public float CloudsShadowAnimTime;

			// Token: 0x04004834 RID: 18484
			public float CloudsShadowBlurriness;

			// Token: 0x04004835 RID: 18485
			public float CloudsShadowScale;

			// Token: 0x04004836 RID: 18486
			public float CloudsShadowIntensity;
		}

		// Token: 0x02000EBE RID: 3774
		public struct ModelVFXDrawTask
		{
			// Token: 0x04004837 RID: 18487
			public Vector3 ModelVFXHighlightColor;

			// Token: 0x04004838 RID: 18488
			public float ModelVFXHighlightThickness;

			// Token: 0x04004839 RID: 18489
			public Vector2 ModelVFXNoiseScale;

			// Token: 0x0400483A RID: 18490
			public Vector2 ModelVFXNoiseScrollSpeed;

			// Token: 0x0400483B RID: 18491
			public int ModelVFXPackedParams;

			// Token: 0x0400483C RID: 18492
			public Vector4 ModelVFXPostColor;
		}

		// Token: 0x02000EBF RID: 3775
		private struct EntityDrawTask
		{
			// Token: 0x0400483D RID: 18493
			public Vector4 BlockLightColor;

			// Token: 0x0400483E RID: 18494
			public Vector3 BottomTint;

			// Token: 0x0400483F RID: 18495
			public Vector3 TopTint;

			// Token: 0x04004840 RID: 18496
			public float InvModelHeight;

			// Token: 0x04004841 RID: 18497
			public Matrix ModelMatrix;

			// Token: 0x04004842 RID: 18498
			public GLVertexArray VertexArray;

			// Token: 0x04004843 RID: 18499
			public int DataCount;

			// Token: 0x04004844 RID: 18500
			public GLBuffer AnimationData;

			// Token: 0x04004845 RID: 18501
			public uint AnimationDataOffset;

			// Token: 0x04004846 RID: 18502
			public ushort AnimationDataSize;

			// Token: 0x04004847 RID: 18503
			public float ModelVFXAnimationProgress;

			// Token: 0x04004848 RID: 18504
			public int ModelVFXId;

			// Token: 0x04004849 RID: 18505
			public float UseDithering;

			// Token: 0x0400484A RID: 18506
			public ushort EntityLocalId;
		}

		// Token: 0x02000EC0 RID: 3776
		private struct EntityDistortionDrawTask
		{
			// Token: 0x0400484B RID: 18507
			public float InvModelHeight;

			// Token: 0x0400484C RID: 18508
			public Matrix ModelMatrix;

			// Token: 0x0400484D RID: 18509
			public GLVertexArray VertexArray;

			// Token: 0x0400484E RID: 18510
			public int DataCount;

			// Token: 0x0400484F RID: 18511
			public GLBuffer AnimationData;

			// Token: 0x04004850 RID: 18512
			public uint AnimationDataOffset;

			// Token: 0x04004851 RID: 18513
			public ushort AnimationDataSize;

			// Token: 0x04004852 RID: 18514
			public float ModelVFXAnimationProgress;

			// Token: 0x04004853 RID: 18515
			public int ModelVFXId;

			// Token: 0x04004854 RID: 18516
			public ushort EntityLocalId;
		}

		// Token: 0x02000EC1 RID: 3777
		private struct NameplateDrawTask
		{
			// Token: 0x04004855 RID: 18517
			public Matrix MVPMatrix;

			// Token: 0x04004856 RID: 18518
			public Vector3 Position;

			// Token: 0x04004857 RID: 18519
			public float FillBlurThreshold;

			// Token: 0x04004858 RID: 18520
			public GLVertexArray VertexArray;

			// Token: 0x04004859 RID: 18521
			public ushort DataCount;

			// Token: 0x0400485A RID: 18522
			public ushort EntityLocalId;
		}

		// Token: 0x02000EC2 RID: 3778
		private struct DebugInfoDrawTask
		{
			// Token: 0x0400485B RID: 18523
			public Matrix LineSightMVPMatrix;

			// Token: 0x0400485C RID: 18524
			public Matrix LineRepulsionMVPMatrix;

			// Token: 0x0400485D RID: 18525
			public Matrix BoxHeadMVPMatrix;

			// Token: 0x0400485E RID: 18526
			public Matrix BoxMVPMatrix;

			// Token: 0x0400485F RID: 18527
			public Matrix BoxCollisionMatrix;

			// Token: 0x04004860 RID: 18528
			public Matrix CylinderCollisionMatrix;

			// Token: 0x04004861 RID: 18529
			public Matrix SphereMVPMatrix;

			// Token: 0x04004862 RID: 18530
			public Vector3 SphereColor;

			// Token: 0x04004863 RID: 18531
			public SceneRenderer.DebugInfoDetailTask[] DetailTasks;

			// Token: 0x04004864 RID: 18532
			public bool Hit;

			// Token: 0x04004865 RID: 18533
			public bool RenderCollision;

			// Token: 0x04004866 RID: 18534
			public bool Collided;
		}

		// Token: 0x02000EC3 RID: 3779
		public struct DebugInfoDetailTask
		{
			// Token: 0x04004867 RID: 18535
			public Vector3 Color;

			// Token: 0x04004868 RID: 18536
			public Matrix Matrix;
		}

		// Token: 0x02000EC4 RID: 3780
		public enum LightingResolution
		{
			// Token: 0x0400486A RID: 18538
			FULL,
			// Token: 0x0400486B RID: 18539
			MIXED,
			// Token: 0x0400486C RID: 18540
			LOW
		}

		// Token: 0x02000EC5 RID: 3781
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct ChunkDrawTask
		{
			// Token: 0x0400486D RID: 18541
			public GLVertexArray VertexArray;

			// Token: 0x0400486E RID: 18542
			public int DataCount;

			// Token: 0x0400486F RID: 18543
			public IntPtr DataOffset;

			// Token: 0x04004870 RID: 18544
			public Matrix ModelMatrix;
		}

		// Token: 0x02000EC6 RID: 3782
		private struct AnimatedBlockDrawTask
		{
			// Token: 0x04004871 RID: 18545
			public GLVertexArray VertexArray;

			// Token: 0x04004872 RID: 18546
			public int DataCount;

			// Token: 0x04004873 RID: 18547
			public GLBuffer AnimationData;

			// Token: 0x04004874 RID: 18548
			public uint AnimationDataOffset;

			// Token: 0x04004875 RID: 18549
			public ushort AnimationDataSize;

			// Token: 0x04004876 RID: 18550
			public Matrix ModelMatrix;
		}

		// Token: 0x02000EC7 RID: 3783
		private struct ChunkOcclusionCullingSetup
		{
			// Token: 0x04004877 RID: 18551
			public byte RequestedOpaqueChunkOccludersCount;

			// Token: 0x04004878 RID: 18552
			public bool UseChunkOccluderPlanes;

			// Token: 0x04004879 RID: 18553
			public bool UseOpaqueChunkOccluders;

			// Token: 0x0400487A RID: 18554
			public bool UseAlphaTestedChunkOccluders;

			// Token: 0x0400487B RID: 18555
			public byte MapAtlasTextureUnit;

			// Token: 0x0400487C RID: 18556
			public GLTexture MapAtlasTexture;
		}

		// Token: 0x02000EC8 RID: 3784
		private struct SunShadowCastingSettings
		{
			// Token: 0x0400487D RID: 18557
			public SceneRenderer.SunShadowCastingSettings.ShadowDirectionType DirectionType;

			// Token: 0x0400487E RID: 18558
			public Vector3 Direction;

			// Token: 0x0400487F RID: 18559
			public float ShadowIntensity;

			// Token: 0x04004880 RID: 18560
			public bool UseSafeAngle;

			// Token: 0x04004881 RID: 18561
			public bool UseChunkShadowCasters;

			// Token: 0x04004882 RID: 18562
			public bool UseEntitiesModelVFX;

			// Token: 0x04004883 RID: 18563
			public bool UseDrawInstanced;

			// Token: 0x04004884 RID: 18564
			public bool UseSmartCascadeDispatch;

			// Token: 0x020010A7 RID: 4263
			public enum ShadowDirectionType
			{
				// Token: 0x04004EC5 RID: 20165
				TopDown,
				// Token: 0x04004EC6 RID: 20166
				StaticCustom,
				// Token: 0x04004EC7 RID: 20167
				DynamicSun
			}
		}

		// Token: 0x02000EC9 RID: 3785
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct EntityShadowMapDrawTask
		{
			// Token: 0x04004885 RID: 18565
			public Matrix ModelMatrix;

			// Token: 0x04004886 RID: 18566
			public GLVertexArray VertexArray;

			// Token: 0x04004887 RID: 18567
			public int DataCount;

			// Token: 0x04004888 RID: 18568
			public GLBuffer AnimationData;

			// Token: 0x04004889 RID: 18569
			public uint AnimationDataOffset;

			// Token: 0x0400488A RID: 18570
			public ushort AnimationDataSize;

			// Token: 0x0400488B RID: 18571
			public UShortVector2 CascadeFirstLast;

			// Token: 0x0400488C RID: 18572
			public float InvModelHeight;

			// Token: 0x0400488D RID: 18573
			public float ModelVFXAnimationProgress;

			// Token: 0x0400488E RID: 18574
			public int ModelVFXId;
		}

		// Token: 0x02000ECA RID: 3786
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct ChunkShadowMapDrawTask
		{
			// Token: 0x0400488F RID: 18575
			public Matrix ModelMatrix;

			// Token: 0x04004890 RID: 18576
			public GLVertexArray VertexArray;

			// Token: 0x04004891 RID: 18577
			public int DataCount;

			// Token: 0x04004892 RID: 18578
			public IntPtr DataOffset;

			// Token: 0x04004893 RID: 18579
			public UShortVector2 CascadeFirstLast;
		}

		// Token: 0x02000ECB RID: 3787
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct AnimatedBlockShadowMapDrawTask
		{
			// Token: 0x04004894 RID: 18580
			public Matrix ModelMatrix;

			// Token: 0x04004895 RID: 18581
			public GLVertexArray VertexArray;

			// Token: 0x04004896 RID: 18582
			public int DataCount;

			// Token: 0x04004897 RID: 18583
			public IntPtr DataOffset;

			// Token: 0x04004898 RID: 18584
			public GLBuffer AnimationData;

			// Token: 0x04004899 RID: 18585
			public uint AnimationDataOffset;

			// Token: 0x0400489A RID: 18586
			public ushort AnimationDataSize;

			// Token: 0x0400489B RID: 18587
			public UShortVector2 CascadeFirstLast;
		}

		// Token: 0x02000ECC RID: 3788
		private struct ShadowCascadeStats
		{
			// Token: 0x0400489C RID: 18588
			public ushort DrawCallCount;

			// Token: 0x0400489D RID: 18589
			public ushort KiloTriangleCount;
		}
	}
}
