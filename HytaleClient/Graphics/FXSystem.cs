using System;
using HytaleClient.Core;
using HytaleClient.Graphics.Particles;
using HytaleClient.Graphics.Trails;
using HytaleClient.Math;
using NLog;

namespace HytaleClient.Graphics
{
	// Token: 0x020009A5 RID: 2469
	internal class FXSystem : Disposable
	{
		// Token: 0x06004F54 RID: 20308 RVA: 0x001659B1 File Offset: 0x00163BB1
		public void SetupDrawDataTexture(uint textureUnitId)
		{
			this._fxRenderer.SetupDrawDataTexture(textureUnitId);
		}

		// Token: 0x06004F55 RID: 20309 RVA: 0x001659C0 File Offset: 0x00163BC0
		public void DrawErosion()
		{
			this._fxRenderer.DrawErosion();
		}

		// Token: 0x06004F56 RID: 20310 RVA: 0x001659CE File Offset: 0x00163BCE
		public void DrawTransparencyLowRes()
		{
			this._fxRenderer.DrawTransparencyLowRes();
		}

		// Token: 0x06004F57 RID: 20311 RVA: 0x001659DC File Offset: 0x00163BDC
		public void DrawTransparency()
		{
			this._fxRenderer.DrawTransparency();
		}

		// Token: 0x06004F58 RID: 20312 RVA: 0x001659EA File Offset: 0x00163BEA
		public void DrawDistortion()
		{
			this._fxRenderer.DrawDistortion();
		}

		// Token: 0x170012AE RID: 4782
		// (get) Token: 0x06004F59 RID: 20313 RVA: 0x001659F8 File Offset: 0x00163BF8
		public GLSampler SmoothSampler
		{
			get
			{
				return this._smoothSampler;
			}
		}

		// Token: 0x06004F5A RID: 20314 RVA: 0x00165A00 File Offset: 0x00163C00
		public FXSystem(GraphicsDevice graphics, Profiling profiling, float engineTimeStep)
		{
			this._graphics = graphics;
			this._profiling = profiling;
			this._engineTimeStep = engineTimeStep;
			bool flag = this._graphics != null;
			if (flag)
			{
				this.Particles = new ParticleFXSystem(this._graphics, this._engineTimeStep);
				this.Trails = new TrailFXSystem(this._graphics, this._engineTimeStep);
				this._fxRenderer = new FXRenderer(this._graphics);
				this._fxRenderer.Initialize(this.Particles.MaxParticleCount + this.Trails.SegmentBufferStorageMaxCount, this.Particles.MaxParticleDrawCount + this.Trails.MaxParticleDrawCount);
				this.ForceFields = new ForceFieldFXSystem(this._graphics);
				GLFunctions gl = this._graphics.GL;
				this._smoothSampler = gl.GenSampler();
				gl.SamplerParameteri(this._smoothSampler, GL.TEXTURE_WRAP_S, GL.CLAMP_TO_EDGE);
				gl.SamplerParameteri(this._smoothSampler, GL.TEXTURE_WRAP_T, GL.CLAMP_TO_EDGE);
				gl.SamplerParameteri(this._smoothSampler, GL.TEXTURE_MIN_FILTER, GL.LINEAR);
				gl.SamplerParameteri(this._smoothSampler, GL.TEXTURE_MAG_FILTER, GL.LINEAR);
			}
		}

		// Token: 0x06004F5B RID: 20315 RVA: 0x00165B48 File Offset: 0x00163D48
		protected override void DoDispose()
		{
			bool flag = this._graphics != null;
			if (flag)
			{
				GLFunctions gl = this._graphics.GL;
				gl.DeleteSampler(this._smoothSampler);
				this.ForceFields.Dispose();
				this.Trails.Dispose();
				this.Particles.Dispose();
			}
		}

		// Token: 0x06004F5C RID: 20316 RVA: 0x00165BA2 File Offset: 0x00163DA2
		public void SetupRenderingProfile(int renderingProfileSendVertexData)
		{
			this._renderingProfileSendVertexData = renderingProfileSendVertexData;
		}

		// Token: 0x06004F5D RID: 20317 RVA: 0x00165BAC File Offset: 0x00163DAC
		public void BeginFrame()
		{
			this.ForceFields.BeginFrame();
			this.Particles.BeginFrame();
			this._fxRenderer.BeginFrame();
		}

		// Token: 0x06004F5E RID: 20318 RVA: 0x00165BD4 File Offset: 0x00163DD4
		public void PrepareForDraw(Vector3 cameraPosition)
		{
			this.Particles.DispatchSpawnersDrawTasks(true);
			this.PrepareVertexDataStorage();
			IntPtr intPtr;
			bool flag = this._fxRenderer.TryBeginDrawDataTransfer(out intPtr);
			if (flag)
			{
				this.Particles.PrepareForDraw(this._fxRenderer, cameraPosition, intPtr);
				this.Trails.PrepareForDraw(this._fxRenderer, cameraPosition, intPtr);
				this._fxRenderer.EndDrawDataTransfer();
			}
			this._profiling.StartMeasure(this._renderingProfileSendVertexData);
			this._fxRenderer.SendVertexDataToGPU();
			this._profiling.StopMeasure(this._renderingProfileSendVertexData);
		}

		// Token: 0x06004F5F RID: 20319 RVA: 0x00165C70 File Offset: 0x00163E70
		private void PrepareVertexDataStorage()
		{
			this._fxRenderer.ClearVertexData();
			this.Particles.PrepareErosionVertexDataStorage(this._fxRenderer);
			this.Trails.PrepareErosionVertexDataStorage(this._fxRenderer);
			this.Particles.PrepareLowResVertexDataStorage(this._fxRenderer);
			this.Particles.PrepareBlendVertexDataStorage(this._fxRenderer);
			this.Trails.PrepareBlendVertexDataStorage(this._fxRenderer);
			this.Particles.PrepareFPVVertexDataStorage(this._fxRenderer);
			this.Trails.PrepareFPVVertexDataStorage(this._fxRenderer);
			this.Particles.PrepareDistortionVertexDataStorage(this._fxRenderer);
			this.Trails.PrepareDistortionVertexDataStorage(this._fxRenderer);
		}

		// Token: 0x06004F60 RID: 20320 RVA: 0x00165D2C File Offset: 0x00163F2C
		public void ProcessFXTasks()
		{
		}

		// Token: 0x04002A88 RID: 10888
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04002A89 RID: 10889
		public readonly byte FXDrawTagForceFieldColor = 1;

		// Token: 0x04002A8A RID: 10890
		public readonly byte FXDrawTagForceFieldDistorion = 2;

		// Token: 0x04002A8B RID: 10891
		public readonly ParticleFXSystem Particles;

		// Token: 0x04002A8C RID: 10892
		public readonly TrailFXSystem Trails;

		// Token: 0x04002A8D RID: 10893
		public readonly ForceFieldFXSystem ForceFields;

		// Token: 0x04002A8E RID: 10894
		private readonly GraphicsDevice _graphics;

		// Token: 0x04002A8F RID: 10895
		private readonly Profiling _profiling;

		// Token: 0x04002A90 RID: 10896
		private readonly FXRenderer _fxRenderer;

		// Token: 0x04002A91 RID: 10897
		private GLSampler _smoothSampler;

		// Token: 0x04002A92 RID: 10898
		private readonly float _engineTimeStep;

		// Token: 0x04002A93 RID: 10899
		private int _renderingProfileSendVertexData;

		// Token: 0x02000EA4 RID: 3748
		public enum RenderMode
		{
			// Token: 0x04004773 RID: 18291
			BlendLinear,
			// Token: 0x04004774 RID: 18292
			BlendAdd,
			// Token: 0x04004775 RID: 18293
			Erosion,
			// Token: 0x04004776 RID: 18294
			Distortion
		}
	}
}
