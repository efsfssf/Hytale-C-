using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;

namespace HytaleClient.Graphics
{
	// Token: 0x0200099C RID: 2460
	internal class CascadedShadowMapping
	{
		// Token: 0x06004EA8 RID: 20136 RVA: 0x0015E434 File Offset: 0x0015C634
		public CascadedShadowMapping(GraphicsDevice graphics)
		{
			this._graphics = graphics;
			this._gpuProgramStore = graphics.GPUProgramStore;
			this._renderTargetStore = graphics.RTStore;
		}

		// Token: 0x06004EA9 RID: 20137 RVA: 0x0015E500 File Offset: 0x0015C700
		public void Dispose()
		{
			this._renderTargetStore = null;
			this._gpuProgramStore = null;
			this._graphics = null;
		}

		// Token: 0x17001295 RID: 4757
		// (get) Token: 0x06004EAA RID: 20138 RVA: 0x0015E518 File Offset: 0x0015C718
		public ref CascadedShadowMapping.CascadedShadowsBuildSettings CascadesSettings
		{
			get
			{
				return ref this._sunShadowCascades;
			}
		}

		// Token: 0x17001296 RID: 4758
		// (get) Token: 0x06004EAB RID: 20139 RVA: 0x0015E520 File Offset: 0x0015C720
		public ref CascadedShadowMapping.DeferredShadowsSettings DeferredShadowSettings
		{
			get
			{
				return ref this._sunDeferredShadows;
			}
		}

		// Token: 0x17001297 RID: 4759
		// (get) Token: 0x06004EAC RID: 20140 RVA: 0x0015E528 File Offset: 0x0015C728
		public bool[] CascadeNeedsUpdate
		{
			get
			{
				return this._cascadeNeedsUpdate;
			}
		}

		// Token: 0x17001298 RID: 4760
		// (get) Token: 0x06004EAD RID: 20141 RVA: 0x0015E530 File Offset: 0x0015C730
		public BoundingFrustum[] CascadeFrustums
		{
			get
			{
				return this._cascadeFrustums;
			}
		}

		// Token: 0x06004EAE RID: 20142 RVA: 0x0015E538 File Offset: 0x0015C738
		public void SetSunShadowsMaxWorldHeight(float maxWorldHeight)
		{
			this._maxWorldHeight = maxWorldHeight;
		}

		// Token: 0x06004EAF RID: 20143 RVA: 0x0015E544 File Offset: 0x0015C744
		public void SetSunShadowsSlopeScaleBias(float factor, float units)
		{
			bool flag = factor != float.NaN && units != float.NaN;
			if (flag)
			{
				this._sunShadowCascades.SlopeScaleBias.X = factor;
				this._sunShadowCascades.SlopeScaleBias.Y = units;
			}
		}

		// Token: 0x17001299 RID: 4761
		// (get) Token: 0x06004EB0 RID: 20144 RVA: 0x0015E590 File Offset: 0x0015C790
		public bool UseSunShadowsGlobalKDop
		{
			get
			{
				return this._sunShadowCascades.UseGlobalKDop;
			}
		}

		// Token: 0x06004EB1 RID: 20145 RVA: 0x0015E5A0 File Offset: 0x0015C7A0
		public void SetSunShadowsCascadeCount(int count)
		{
			int max = 4;
			int num = MathHelper.Clamp(count, 1, max);
			bool flag = this._sunShadowCascades.Count != num;
			if (flag)
			{
				int width = this._renderTargetStore.ShadowMap.Width / this._sunShadowCascades.Count;
				this._sunShadowCascades.Count = num;
				this.SetSunShadowMapResolution((uint)width, (uint)this._renderTargetStore.ShadowMap.Height);
				this._gpuProgramStore.DeferredShadowProgram.CascadeCount = (uint)num;
				this._gpuProgramStore.DeferredShadowProgram.Reset(true);
				this._gpuProgramStore.VolumetricSunshaftProgram.CascadeCount = (uint)num;
				this._gpuProgramStore.VolumetricSunshaftProgram.Reset(true);
			}
		}

		// Token: 0x06004EB2 RID: 20146 RVA: 0x0015E65C File Offset: 0x0015C85C
		public void SetSunShadowMapResolution(uint width, uint height = 0U)
		{
			RenderTargetStore rtstore = this._graphics.RTStore;
			height = ((height == 0U) ? width : height);
			width *= (uint)this._sunShadowCascades.Count;
			bool flag = width <= 8192U && height <= 8192U;
			if (flag)
			{
				bool flag2 = (long)rtstore.ShadowMap.Width != (long)((ulong)width) || (long)rtstore.ShadowMap.Height != (long)((ulong)height);
				if (flag2)
				{
					rtstore.ShadowMap.Resize((int)width, (int)height, false);
				}
			}
		}

		// Token: 0x06004EB3 RID: 20147 RVA: 0x0015E6E8 File Offset: 0x0015C8E8
		public void SetDeferredShadowResolutionScale(float scale)
		{
			RenderTargetStore rtstore = this._graphics.RTStore;
			bool flag = scale > 0f && scale <= 1f && this._sunDeferredShadows.ResolutionScale != scale;
			if (flag)
			{
				this._sunDeferredShadows.ResolutionScale = scale;
				rtstore.SetDeferredShadowResolutionScale(scale);
			}
		}

		// Token: 0x1700129A RID: 4762
		// (get) Token: 0x06004EB4 RID: 20148 RVA: 0x0015E740 File Offset: 0x0015C940
		public bool UseDeferredShadowBlur
		{
			get
			{
				return this._sunDeferredShadows.UseBlur;
			}
		}

		// Token: 0x06004EB5 RID: 20149 RVA: 0x0015E74D File Offset: 0x0015C94D
		public void SetSunShadowsGlobalKDopEnabled(bool enable)
		{
			this._sunShadowCascades.UseGlobalKDop = enable;
		}

		// Token: 0x06004EB6 RID: 20150 RVA: 0x0015E75C File Offset: 0x0015C95C
		public void SetSunShadowMapCachingEnabled(bool enable)
		{
			this._sunShadowCascades.UseCaching = enable;
		}

		// Token: 0x06004EB7 RID: 20151 RVA: 0x0015E76B File Offset: 0x0015C96B
		public void SetSunShadowMappingStableProjectionEnabled(bool enable)
		{
			this._sunShadowCascades.UseStableProjection = enable;
		}

		// Token: 0x06004EB8 RID: 20152 RVA: 0x0015E779 File Offset: 0x0015C979
		public void SetSunShadowMappingUseLinearZ(bool enable)
		{
			this._gpuProgramStore.DeferredShadowProgram.UseLinearZ = enable;
			this._gpuProgramStore.DeferredShadowProgram.Reset(true);
		}

		// Token: 0x06004EB9 RID: 20153 RVA: 0x0015E79F File Offset: 0x0015C99F
		public void SetSunShadowsUseCleanBackfaces(bool enable)
		{
			this._gpuProgramStore.DeferredShadowProgram.UseCleanBackfaces = enable;
			this._gpuProgramStore.DeferredShadowProgram.Reset(true);
		}

		// Token: 0x06004EBA RID: 20154 RVA: 0x0015E7C8 File Offset: 0x0015C9C8
		public void SetDeferredShadowsBlurEnabled(bool enable)
		{
			bool flag = this._sunDeferredShadows.UseBlur != enable;
			if (flag)
			{
				this._sunDeferredShadows.UseBlur = enable;
			}
		}

		// Token: 0x06004EBB RID: 20155 RVA: 0x0015E7FC File Offset: 0x0015C9FC
		public void SetDeferredShadowsNoiseEnabled(bool enable)
		{
			bool flag = this._sunDeferredShadows.UseNoise != enable;
			if (flag)
			{
				this._sunDeferredShadows.UseNoise = enable;
				this._gpuProgramStore.DeferredShadowProgram.UseNoise = enable;
				this._gpuProgramStore.DeferredShadowProgram.Reset(true);
			}
		}

		// Token: 0x06004EBC RID: 20156 RVA: 0x0015E850 File Offset: 0x0015CA50
		public void SetDeferredShadowsManualModeEnabled(bool enable)
		{
			bool flag = this._sunDeferredShadows.UseManualMode != enable;
			if (flag)
			{
				this._sunDeferredShadows.UseManualMode = enable;
				this._gpuProgramStore.DeferredShadowProgram.UseManualMode = enable;
				this._gpuProgramStore.DeferredShadowProgram.Reset(true);
				this._gpuProgramStore.VolumetricSunshaftProgram.UseManualMode = enable;
				this._gpuProgramStore.VolumetricSunshaftProgram.Reset(true);
			}
		}

		// Token: 0x06004EBD RID: 20157 RVA: 0x0015E8C8 File Offset: 0x0015CAC8
		public void SetDeferredShadowsFadingEnabled(bool enable)
		{
			bool flag = this._sunDeferredShadows.UseFading != enable;
			if (flag)
			{
				this._sunDeferredShadows.UseFading = enable;
				this._gpuProgramStore.DeferredShadowProgram.UseFading = enable;
				this._gpuProgramStore.DeferredShadowProgram.Reset(true);
			}
		}

		// Token: 0x06004EBE RID: 20158 RVA: 0x0015E91C File Offset: 0x0015CB1C
		public void SetDeferredShadowsWithSingleSampleEnabled(bool enable)
		{
			bool flag = this._sunDeferredShadows.UseSingleSample != enable;
			if (flag)
			{
				this._sunDeferredShadows.UseSingleSample = enable;
				this._gpuProgramStore.DeferredShadowProgram.UseSingleSample = enable;
				this._gpuProgramStore.DeferredShadowProgram.Reset(true);
			}
		}

		// Token: 0x06004EBF RID: 20159 RVA: 0x0015E970 File Offset: 0x0015CB70
		public void SetDeferredShadowsCameraBiasEnabled(bool enable)
		{
			bool flag = this._sunDeferredShadows.UseCameraBias != enable;
			if (flag)
			{
				this._sunDeferredShadows.UseCameraBias = enable;
				this._gpuProgramStore.DeferredShadowProgram.UseCameraBias = enable;
				this._gpuProgramStore.DeferredShadowProgram.Reset(true);
			}
		}

		// Token: 0x06004EC0 RID: 20160 RVA: 0x0015E9C4 File Offset: 0x0015CBC4
		public void SetDeferredShadowsNormalBiasEnabled(bool enable)
		{
			bool flag = this._sunDeferredShadows.UseNormalBias != enable;
			if (flag)
			{
				this._sunDeferredShadows.UseNormalBias = enable;
				this._gpuProgramStore.DeferredShadowProgram.UseNormalBias = enable;
				this._gpuProgramStore.DeferredShadowProgram.Reset(true);
			}
		}

		// Token: 0x06004EC1 RID: 20161 RVA: 0x0015EA18 File Offset: 0x0015CC18
		public void Init(Action shadowCastersDrawFunc)
		{
			this.RegisterShadowCastersDrawFunc(shadowCastersDrawFunc);
			this.InitShadowCascades();
			this.InitDeferredShadows();
			this.InitShadowCascadesDebug();
		}

		// Token: 0x06004EC2 RID: 20162 RVA: 0x0015EA38 File Offset: 0x0015CC38
		public void Release()
		{
			this.DisposeShadowCascadesDebug();
			this.UnregisterShadowCastersDrawFunc();
		}

		// Token: 0x06004EC3 RID: 20163 RVA: 0x0015EA4C File Offset: 0x0015CC4C
		private void InitShadowCascades()
		{
			for (int i = 0; i < 4; i++)
			{
				this._cascadeFrustums[i] = new BoundingFrustum(Matrix.Identity);
			}
			this._sunShadowCascades.Count = 1;
			this._sunShadowCascades.UseGlobalKDop = true;
			this._sunShadowCascades.UseCaching = true;
			this._sunShadowCascades.UseStableProjection = true;
			this._sunShadowCascades.UseSlopeScaleBias = true;
			this._sunShadowCascades.SlopeScaleBias = new Vector2(1f, 1f);
		}

		// Token: 0x06004EC4 RID: 20164 RVA: 0x0015EAD4 File Offset: 0x0015CCD4
		private void InitDeferredShadows()
		{
			this._sunDeferredShadows.ResolutionScale = 1f;
			this._sunDeferredShadows.UseNoise = true;
			this._sunDeferredShadows.UseManualMode = false;
			this._sunDeferredShadows.UseFading = false;
			this._sunDeferredShadows.UseSingleSample = true;
			this._sunDeferredShadows.UseCameraBias = false;
			this._sunDeferredShadows.UseNormalBias = true;
			this._sunDeferredShadows.UseBlur = true;
		}

		// Token: 0x06004EC5 RID: 20165 RVA: 0x0015EB48 File Offset: 0x0015CD48
		public void Update(ref CascadedShadowMapping.InputParams csmInputParams, ref CascadedShadowMapping.RenderData csmRenderParams)
		{
			this._csmInputParams = csmInputParams;
			bool isFrozenForDebug = this.IsFrozenForDebug;
			if (isFrozenForDebug)
			{
				for (int i = 0; i < this._sunShadowCascades.Count; i++)
				{
					this._cascadeCachedTranslations[i] += csmInputParams.CameraPositionDelta;
					csmRenderParams.CascadeCachedTranslations[i] = this._cascadeCachedTranslations[i];
				}
			}
			else
			{
				int size = 48;
				this.UpdateShadowCascadeDistances(size);
				bool flag = !this._sunShadowCascades.UseCaching || this._csmInputParams.IsSpatialContinuityLost;
				for (int j = 0; j < this._sunShadowCascades.Count; j++)
				{
					this._cascadeNeedsUpdate[j] = (flag || this.ShouldUpdateCascade(j, this._csmInputParams.FrameId, this._csmInputParams.QuantifiedCameraMotion));
				}
				Vector3 virtualSunPosition;
				Matrix matrix;
				this.UpdateGlobalShadowFrustum(this._csmInputParams.LightDirection, this._csmInputParams.WorldFieldOfView, this._csmInputParams.AspectRatio, this._csmInputParams.CameraPosition, ref this._csmInputParams.ViewRotationMatrix, out virtualSunPosition, out matrix);
				csmRenderParams.VirtualSunPosition = virtualSunPosition;
				csmRenderParams.VirtualSunViewFrustum.Matrix = matrix;
				this.UpdateShadowCascadeFrustums(this._csmInputParams.LightDirection, this._csmInputParams.WorldFieldOfView, this._csmInputParams.AspectRatio, this._csmInputParams.CameraPosition, ref this._csmInputParams.ViewRotationMatrix, this._csmInputParams.CameraPositionDelta, ref csmRenderParams);
				bool useSunShadowsGlobalKDop = this.UseSunShadowsGlobalKDop;
				if (useSunShadowsGlobalKDop)
				{
					float farPlaneDistance = this._cascadeDistances[this._sunShadowCascades.Count];
					Matrix matrix2 = Matrix.CreatePerspectiveFieldOfView(this._csmInputParams.WorldFieldOfView, this._csmInputParams.AspectRatio, this._csmInputParams.NearClipDistance, farPlaneDistance);
					Matrix value = Matrix.Multiply(this._csmInputParams.ViewRotationMatrix, matrix2);
					BoundingFrustum frustum = new BoundingFrustum(value);
					csmRenderParams.VirtualSunKDopFrustum.BuildFrom(frustum, this._csmInputParams.LightDirection);
				}
				csmRenderParams.VirtualSunDirection = this._csmInputParams.LightDirection;
				for (int k = 0; k < 4; k++)
				{
					csmRenderParams.CascadeDistanceAndTexelScales[k] = this._cascadeDistanceAndTexelScales[k];
					csmRenderParams.CascadeCachedTranslations[k] = this._cascadeCachedTranslations[k];
				}
			}
		}

		// Token: 0x06004EC6 RID: 20166 RVA: 0x0015EDC4 File Offset: 0x0015CFC4
		private void StabilizeProjection(int shadowMapWidth, int shadowMapHeight, ref Matrix shadowViewMatrix, ref Matrix shadowProjectionMatrix, ref Matrix shadowViewProjectionMatrix)
		{
			Vector3 vector = new Vector3(0f, 0f, 0f);
			vector = Vector3.Transform(vector, shadowViewProjectionMatrix);
			vector.X = vector.X * (float)shadowMapWidth / 2f;
			vector.Y = vector.Y * (float)shadowMapWidth / 2f;
			vector.Z = vector.Z * (float)shadowMapWidth / 2f;
			Vector3 value;
			value.X = (float)MathHelper.Round(vector.X);
			value.Y = (float)MathHelper.Round(vector.Y);
			value.Z = (float)MathHelper.Round(vector.Z);
			Vector3 vector2 = value - vector;
			vector2.X = vector2.X * 2f / (float)shadowMapWidth;
			vector2.Y = vector2.Y * 2f / (float)shadowMapWidth;
			vector2.Z = vector2.Z * 2f / (float)shadowMapWidth;
			shadowProjectionMatrix.M41 += vector2.X;
			shadowProjectionMatrix.M42 += vector2.Y;
			shadowProjectionMatrix.M43 += 0f;
			Matrix.Multiply(ref shadowViewMatrix, ref shadowProjectionMatrix, out shadowViewProjectionMatrix);
		}

		// Token: 0x06004EC7 RID: 20167 RVA: 0x0015EEF8 File Offset: 0x0015D0F8
		private void ComputeShadowCascadeData(Vector3 lightDirection, float nearDistance, float farDistance, float worldFoV, float aspectRatio, Vector3 cameraPosition, ref Matrix viewRotationMatrix, int shadowMapWidth, int shadowMapHeight, bool useStableProjection, out Vector3 virtualLightPosition, out Matrix shadowViewMatrix, out Matrix shadowProjectionMatrix, out Matrix shadowViewProjectionMatrix)
		{
			Matrix matrix = Matrix.CreatePerspectiveFieldOfView(worldFoV, aspectRatio, nearDistance, farDistance);
			Matrix value = Matrix.Multiply(viewRotationMatrix, matrix);
			BoundingFrustum boundingFrustum = new BoundingFrustum(value);
			boundingFrustum.GetCorners(this._tmpFrustumCorners);
			BoundingSphere boundingSphere;
			boundingSphere.Center = (this._tmpFrustumCorners[7] + this._tmpFrustumCorners[6] + this._tmpFrustumCorners[5] + this._tmpFrustumCorners[4] + this._tmpFrustumCorners[3] + this._tmpFrustumCorners[2] + this._tmpFrustumCorners[1] + this._tmpFrustumCorners[0]) / 8f;
			boundingSphere.Radius = Vector3.Distance(this._tmpFrustumCorners[0], boundingSphere.Center);
			Vector3 value2 = new Vector3(0.1f, 0f, 0f);
			float num = Math.Min(this._maxWorldHeight, 400f);
			num -= cameraPosition.Y;
			num = Math.Max(boundingSphere.Center.Y + 10f, num);
			Plane plane = new Plane(Vector3.Down, num);
			Vector3 vector;
			CascadedShadowMapping.ComputeIntersection(plane, boundingSphere.Center, lightDirection, out vector);
			vector += value2;
			Matrix matrix2 = Matrix.CreateLookAt(vector, boundingSphere.Center, Vector3.Up);
			float num2 = Vector3.Distance(boundingSphere.Center, vector) + boundingSphere.Radius;
			num2 = MathHelper.Clamp(num2, 0f, 720f);
			num2 = (float)MathHelper.Round(num2);
			num2 = 720f;
			float num3 = (float)MathHelper.Round(boundingSphere.Radius) + 1f;
			float num4 = num3 * 2f;
			Matrix matrix3 = Matrix.CreateOrthographic(num4, num4, 1f, num2);
			Matrix matrix4 = Matrix.Multiply(matrix2, matrix3);
			if (useStableProjection)
			{
				this.StabilizeProjection(shadowMapWidth, shadowMapHeight, ref matrix2, ref matrix3, ref matrix4);
			}
			shadowViewMatrix = matrix2;
			shadowProjectionMatrix = matrix3;
			shadowViewProjectionMatrix = matrix4;
			virtualLightPosition = vector;
		}

		// Token: 0x06004EC8 RID: 20168 RVA: 0x0015F128 File Offset: 0x0015D328
		private void UpdateShadowCascadeDistances(int size)
		{
			this._cascadeDistances[0] = this._csmInputParams.NearClipDistance;
			switch (this._sunShadowCascades.Count)
			{
			case 1:
				this._cascadeSizes[0] = (float)size;
				this._cascadeDistances[1] = (float)size;
				this._cascadeDistanceAndTexelScales[0].X = (float)size;
				this._cascadeDistanceAndTexelScales[0].Y = 0.25f;
				break;
			case 2:
				this._cascadeSizes[0] = (float)(size / 4);
				this._cascadeSizes[1] = (float)size;
				this._cascadeDistances[1] = (float)size / 4f;
				this._cascadeDistances[2] = (float)size;
				this._cascadeDistanceAndTexelScales[0].X = (float)size / 4f;
				this._cascadeDistanceAndTexelScales[1].X = (float)size;
				this._cascadeDistanceAndTexelScales[0].Y = 1f;
				this._cascadeDistanceAndTexelScales[1].Y = 0.25f;
				break;
			case 3:
				this._cascadeSizes[0] = (float)(size / 4);
				this._cascadeSizes[1] = (float)size * 1f;
				this._cascadeSizes[2] = (float)size * 2f;
				this._cascadeDistances[1] = (float)size / 4f;
				this._cascadeDistances[2] = (float)size * 1f;
				this._cascadeDistances[3] = (float)size * 2f;
				this._cascadeDistanceAndTexelScales[0].X = (float)size / 4f;
				this._cascadeDistanceAndTexelScales[1].X = (float)size * 1f;
				this._cascadeDistanceAndTexelScales[2].X = (float)size * 2f;
				this._cascadeDistanceAndTexelScales[0].Y = 1f;
				this._cascadeDistanceAndTexelScales[1].Y = 0.25f;
				this._cascadeDistanceAndTexelScales[2].Y = 0.125f;
				break;
			case 4:
				this._cascadeSizes[0] = (float)(size / 4);
				this._cascadeSizes[1] = (float)size;
				this._cascadeSizes[2] = (float)((int)((float)size * 2f));
				this._cascadeSizes[3] = (float)(size * 5);
				this._cascadeDistances[1] = (float)size / 4f;
				this._cascadeDistances[2] = (float)size;
				this._cascadeDistances[3] = (float)size * 2f;
				this._cascadeDistances[4] = (float)size * 5f;
				this._cascadeDistanceAndTexelScales[0].X = (float)size / 4f;
				this._cascadeDistanceAndTexelScales[1].X = (float)size;
				this._cascadeDistanceAndTexelScales[2].X = (float)size * 2f;
				this._cascadeDistanceAndTexelScales[3].X = (float)size * 5f;
				this._cascadeDistanceAndTexelScales[0].Y = 1f;
				this._cascadeDistanceAndTexelScales[1].Y = 0.25f;
				this._cascadeDistanceAndTexelScales[2].Y = 0.125f;
				this._cascadeDistanceAndTexelScales[3].Y = 0.05f;
				break;
			}
		}

		// Token: 0x06004EC9 RID: 20169 RVA: 0x0015F458 File Offset: 0x0015D658
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool ShouldUpdateCascade(int cascadeId, uint frameId, Vector2 quantifiedCameraMotion)
		{
			bool flag = quantifiedCameraMotion.X >= 25f || quantifiedCameraMotion.Y >= 0.2617994f;
			int count = this._sunShadowCascades.Count;
			int num = count;
			bool result;
			if (num != 3)
			{
				if (num != 4)
				{
					result = true;
				}
				else
				{
					bool flag2 = flag;
					if (flag2)
					{
						result = (cascadeId == 0 || (cascadeId == 1 && frameId % 2U == 1U) || (cascadeId == 2 && frameId % 2U == 1U) || (cascadeId == 3 && frameId % 2U == 0U));
					}
					else
					{
						result = (cascadeId == 0 || (cascadeId == 1 && frameId % 4U != 0U) || (cascadeId == 2 && frameId % 2U == 1U) || (cascadeId == 3 && frameId % 4U == 0U));
					}
				}
			}
			else
			{
				result = (cascadeId == 0 || (cascadeId == 1 && frameId % 2U == 0U) || (cascadeId == 2 && frameId % 2U == 1U));
			}
			return result;
		}

		// Token: 0x06004ECA RID: 20170 RVA: 0x0015F52C File Offset: 0x0015D72C
		private void UpdateShadowCascadeFrustums(Vector3 lightDirection, float worldFoV, float aspectRatio, Vector3 cameraPosition, ref Matrix viewMatrix, Vector3 cameraPositionDelta, ref CascadedShadowMapping.RenderData outRenderData)
		{
			for (int i = 0; i < this._sunShadowCascades.Count; i++)
			{
				bool flag = this._cascadeNeedsUpdate[i];
				if (flag)
				{
					this.ComputeShadowCascadeData(lightDirection, this._cascadeDistances[i], this._cascadeDistances[i + 1], worldFoV, aspectRatio, cameraPosition, ref viewMatrix, this._renderTargetStore.ShadowMap.Width / this._sunShadowCascades.Count, this._renderTargetStore.ShadowMap.Height, this._sunShadowCascades.UseStableProjection, out outRenderData.VirtualSunPositions[i], out outRenderData.VirtualSunViewRotationMatrix[i], out outRenderData.VirtualSunProjectionMatrix[i], out outRenderData.VirtualSunViewRotationProjectionMatrix[i]);
					this._cascadeCachedTranslations[i] = new Vector3(0f);
					this._cascadeFrustums[i].Matrix = outRenderData.VirtualSunViewRotationProjectionMatrix[i];
				}
				else
				{
					this._cascadeCachedTranslations[i] += cameraPositionDelta;
				}
			}
		}

		// Token: 0x06004ECB RID: 20171 RVA: 0x0015F64C File Offset: 0x0015D84C
		private void UpdateGlobalShadowFrustum(Vector3 lightDirection, float worldFoV, float aspectRatio, Vector3 cameraPosition, ref Matrix viewRotationMatrix, out Vector3 virtualLightPosition, out Matrix virtualSunViewFrustumMatrix)
		{
			float farDistance = this._cascadeDistances[this._sunShadowCascades.Count];
			Vector3 vector;
			Matrix matrix;
			Matrix matrix2;
			Matrix matrix3;
			this.ComputeShadowCascadeData(lightDirection, this._csmInputParams.NearClipDistance, farDistance, worldFoV, aspectRatio, cameraPosition, ref viewRotationMatrix, this._renderTargetStore.ShadowMap.Width, this._renderTargetStore.ShadowMap.Height, false, out vector, out matrix, out matrix2, out matrix3);
			virtualLightPosition = vector;
			virtualSunViewFrustumMatrix = matrix3;
		}

		// Token: 0x06004ECC RID: 20172 RVA: 0x0015F6C4 File Offset: 0x0015D8C4
		private static float ComputeDistance(Plane plane, Vector3 point)
		{
			return Vector3.Dot(plane.Normal, point) + plane.D;
		}

		// Token: 0x06004ECD RID: 20173 RVA: 0x0015F6E9 File Offset: 0x0015D8E9
		private static void ComputePointAlongRay(Vector3 rayOrigin, Vector3 rayDirection, float distance, out Vector3 point)
		{
			point = rayOrigin + rayDirection * distance;
		}

		// Token: 0x06004ECE RID: 20174 RVA: 0x0015F700 File Offset: 0x0015D900
		public static bool ComputeIntersection(Plane plane, Vector3 rayOrigin, Vector3 rayDirection, out Vector3 intersection)
		{
			bool result = false;
			intersection = Vector3.Zero;
			float num = Vector3.Dot(plane.Normal, rayDirection);
			bool flag = num != 0f;
			if (flag)
			{
				float distance = -CascadedShadowMapping.ComputeDistance(plane, rayOrigin) / num;
				CascadedShadowMapping.ComputePointAlongRay(rayOrigin, rayDirection, distance, out intersection);
				result = true;
			}
			return result;
		}

		// Token: 0x06004ECF RID: 20175 RVA: 0x0015F757 File Offset: 0x0015D957
		private void RegisterShadowCastersDrawFunc(Action shadowCastersDrawFunc)
		{
			this._shadowCastersDrawFunc = shadowCastersDrawFunc;
		}

		// Token: 0x06004ED0 RID: 20176 RVA: 0x0015F761 File Offset: 0x0015D961
		private void UnregisterShadowCastersDrawFunc()
		{
			this._shadowCastersDrawFunc = null;
		}

		// Token: 0x06004ED1 RID: 20177 RVA: 0x0015F76C File Offset: 0x0015D96C
		public void BuildShadowMap()
		{
			GLFunctions gl = this._graphics.GL;
			Debug.Assert(this._shadowCastersDrawFunc != null, "Did you forget to call RegisterShadowCastersDrawFunc()?");
			gl.AssertEnabled(GL.DEPTH_TEST);
			gl.AssertEnabled(GL.CULL_FACE);
			gl.AssertDepthFunc(GL.LEQUAL);
			gl.AssertDepthMask(true);
			gl.DepthFunc(GL.LESS);
			gl.ColorMask(false, false, false, false);
			bool flag = !this._sunShadowCascades.UseCaching || this._csmInputParams.IsSpatialContinuityLost;
			this._renderTargetStore.ShadowMap.Bind(flag, false);
			int num = this._renderTargetStore.ShadowMap.Width / this._sunShadowCascades.Count;
			bool flag2 = !flag;
			if (flag2)
			{
				gl.Enable(GL.SCISSOR_TEST);
				int num2 = 1;
				for (int i = 1; i < this._sunShadowCascades.Count; i++)
				{
					bool flag3 = !this._cascadeNeedsUpdate[i];
					if (flag3)
					{
						break;
					}
					num2++;
				}
				gl.Scissor(0, 0, num * num2, this._renderTargetStore.ShadowMap.Height);
				gl.Clear(GL.DEPTH_BUFFER_BIT);
				for (int j = num2 + 1; j < this._sunShadowCascades.Count; j++)
				{
					bool flag4 = this._cascadeNeedsUpdate[j];
					if (flag4)
					{
						gl.Scissor(j * num, 0, num, this._renderTargetStore.ShadowMap.Height);
						gl.Clear(GL.DEPTH_BUFFER_BIT);
					}
				}
				gl.Disable(GL.SCISSOR_TEST);
			}
			bool useSlopeScaleBias = this._sunShadowCascades.UseSlopeScaleBias;
			if (useSlopeScaleBias)
			{
				gl.Enable(GL.POLYGON_OFFSET_FILL);
				gl.PolygonOffset(this._sunShadowCascades.SlopeScaleBias.X, this._sunShadowCascades.SlopeScaleBias.Y);
			}
			this._shadowCastersDrawFunc();
			this._renderTargetStore.ShadowMap.Unbind();
			gl.DepthFunc(GL.LEQUAL);
			gl.Disable(GL.POLYGON_OFFSET_FILL);
			gl.CullFace(GL.BACK);
			gl.ColorMask(true, true, true, true);
			gl.AssertCullFace(GL.BACK);
			gl.AssertDepthFunc(GL.LEQUAL);
			gl.AssertEnabled(GL.DEPTH_TEST);
			gl.AssertDepthMask(true);
		}

		// Token: 0x06004ED2 RID: 20178 RVA: 0x0015F9F8 File Offset: 0x0015DBF8
		public void DrawDeferredShadow(GLBuffer sceneDataBuffer, Vector3[] frustumFarCornersWS)
		{
			GLFunctions gl = this._graphics.GL;
			this._renderTargetStore.DeferredShadow.Bind(false, true);
			float[] data = new float[]
			{
				1f,
				1f,
				1f,
				1f
			};
			gl.ClearBufferfv(GL.COLOR, 0, data);
			DeferredShadowProgram deferredShadowProgram = this._gpuProgramStore.DeferredShadowProgram;
			deferredShadowProgram.SceneDataBlock.SetBuffer(sceneDataBuffer);
			gl.UseProgram(deferredShadowProgram);
			gl.ActiveTexture(GL.TEXTURE3);
			gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.GBuffer.GetTexture(RenderTarget.Target.Color2));
			gl.ActiveTexture(GL.TEXTURE2);
			gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.GBuffer.GetTexture(RenderTarget.Target.Color0));
			gl.ActiveTexture(GL.TEXTURE1);
			gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.ShadowMap.GetTexture(RenderTarget.Target.Depth));
			gl.ActiveTexture(GL.TEXTURE0);
			bool useLinearZ = deferredShadowProgram.UseLinearZ;
			if (useLinearZ)
			{
				gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.LinearZ.GetTexture(RenderTarget.Target.Color0));
				deferredShadowProgram.FarCorners.SetValue(frustumFarCornersWS);
			}
			else
			{
				gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.GBuffer.GetTexture(RenderTarget.Target.Depth));
			}
			this._graphics.ScreenTriangleRenderer.Draw();
			this._renderTargetStore.DeferredShadow.Unbind();
		}

		// Token: 0x06004ED3 RID: 20179 RVA: 0x0015FB6C File Offset: 0x0015DD6C
		public string WriteShadowMappingStateToString()
		{
			string text = "CSM state :";
			text = text + "\n.Deferred res. scale: " + this._sunDeferredShadows.ResolutionScale.ToString();
			text = text + "\n.Noise: " + this._sunDeferredShadows.UseNoise.ToString();
			text = text + "\n.Blur: " + this._sunDeferredShadows.UseBlur.ToString();
			text = text + "\n.Fade: " + this._sunDeferredShadows.UseFading.ToString();
			text = text + "\n.Stable: " + this._sunShadowCascades.UseStableProjection.ToString();
			text = string.Concat(new string[]
			{
				text,
				"\n.Map resolution: ",
				this._graphics.RTStore.ShadowMap.Width.ToString(),
				"x",
				this._graphics.RTStore.ShadowMap.Height.ToString()
			});
			string text2 = this._sunDeferredShadows.UseCameraBias ? "camera, " : "";
			text2 += (this._sunDeferredShadows.UseNormalBias ? "normal, " : "");
			text2 += (this._sunShadowCascades.UseSlopeScaleBias ? ("slope scale " + this._sunShadowCascades.SlopeScaleBias.ToString() + ", ") : "");
			text2 += (this._gpuProgramStore.BlockyModelShadowMapProgram.UseBiasMethod1 ? "model hack#1, " : "");
			text2 += (this._gpuProgramStore.BlockyModelShadowMapProgram.UseBiasMethod2 ? "model hack#2, " : "");
			text = text + "\n.Bias methods: " + text2;
			string text3 = this._sunShadowCascades.UseGlobalKDop ? "global, " : "";
			text3 += ((text3 == "") ? "none" : "");
			text = text + "\n.K-Dop: " + text3;
			text = text + "\n.Caching: " + this._sunShadowCascades.UseCaching.ToString();
			return text + "\n.Cascade count: " + this._sunShadowCascades.Count.ToString();
		}

		// Token: 0x1700129B RID: 4763
		// (get) Token: 0x06004ED4 RID: 20180 RVA: 0x0015FDC3 File Offset: 0x0015DFC3
		public bool NeedsDebugDrawShadowRelated
		{
			get
			{
				return this._isDebuggingCameraFrustum || this._isDebuggingCameraFrustumSplits || this._isDebuggingShadowCascadeFrustums;
			}
		}

		// Token: 0x1700129C RID: 4764
		// (get) Token: 0x06004ED5 RID: 20181 RVA: 0x0015FDDE File Offset: 0x0015DFDE
		private bool IsFrozenForDebug
		{
			get
			{
				return this._isDebuggingFreeze || this._isDebuggingCameraFrustumSplits || this._isDebuggingShadowCascadeFrustums;
			}
		}

		// Token: 0x06004ED6 RID: 20182 RVA: 0x0015FDF9 File Offset: 0x0015DFF9
		public void ToggleFreeze()
		{
			this._isDebuggingFreeze = !this._isDebuggingFreeze;
		}

		// Token: 0x06004ED7 RID: 20183 RVA: 0x0015FE0C File Offset: 0x0015E00C
		public void ToggleCameraFrustumDebug(Vector3 cameraPosition)
		{
			this._isDebuggingCameraFrustum = !this._isDebuggingCameraFrustum;
			bool isDebuggingCameraFrustum = this._isDebuggingCameraFrustum;
			if (isDebuggingCameraFrustum)
			{
				this._debugCameraPosition = cameraPosition;
				BoundingFrustum boundingFrustum = new BoundingFrustum(Matrix.Identity);
				float farPlaneDistance = this._cascadeDistances[this._sunShadowCascades.Count];
				Matrix matrix = Matrix.CreatePerspectiveFieldOfView(this._csmInputParams.WorldFieldOfView, this._csmInputParams.AspectRatio, this._csmInputParams.NearClipDistance, farPlaneDistance);
				Matrix matrix2 = Matrix.Multiply(this._csmInputParams.ViewRotationMatrix, matrix);
				boundingFrustum.Matrix = matrix2;
				MeshProcessor.CreateFrustum(ref this._debugCameraFrustumMesh, ref boundingFrustum);
			}
		}

		// Token: 0x06004ED8 RID: 20184 RVA: 0x0015FEAC File Offset: 0x0015E0AC
		public void ToggleCameraFrustumSplitsDebug(Vector3 cameraPosition)
		{
			this._isDebuggingCameraFrustumSplits = !this._isDebuggingCameraFrustumSplits;
			bool isDebuggingCameraFrustumSplits = this._isDebuggingCameraFrustumSplits;
			if (isDebuggingCameraFrustumSplits)
			{
				this._debugCameraPosition = cameraPosition;
				BoundingFrustum boundingFrustum = new BoundingFrustum(Matrix.Identity);
				for (int i = 0; i < 4; i++)
				{
					Matrix matrix = Matrix.CreatePerspectiveFieldOfView(this._csmInputParams.WorldFieldOfView, this._csmInputParams.AspectRatio, this._cascadeDistances[i], this._cascadeDistances[i + 1]);
					Matrix value = Matrix.Multiply(this._csmInputParams.ViewRotationMatrix, matrix);
					boundingFrustum = new BoundingFrustum(value);
					MeshProcessor.CreateFrustum(ref this._debugCameraFrustumSplitMeshes[i], ref boundingFrustum);
				}
			}
		}

		// Token: 0x06004ED9 RID: 20185 RVA: 0x0015FF5C File Offset: 0x0015E15C
		public void ToggleShadowCascadeFrustumDebug(Vector3 cameraPosition)
		{
			this._isDebuggingShadowCascadeFrustums = !this._isDebuggingShadowCascadeFrustums;
			bool isDebuggingShadowCascadeFrustums = this._isDebuggingShadowCascadeFrustums;
			if (isDebuggingShadowCascadeFrustums)
			{
				this._debugCameraPosition = cameraPosition;
				for (int i = 0; i < 4; i++)
				{
					MeshProcessor.CreateFrustum(ref this._debugShadowCascadeFrustumMeshes[i], ref this._cascadeFrustums[i]);
				}
			}
		}

		// Token: 0x06004EDA RID: 20186 RVA: 0x0015FFBC File Offset: 0x0015E1BC
		public void DebugDrawShadowRelated(ref Matrix viewProjectionMatrix)
		{
			GLFunctions gl = this._graphics.GL;
			gl.DepthMask(false);
			gl.Enable(GL.DEPTH_TEST);
			gl.Disable(GL.CULL_FACE);
			gl.BlendFunc(GL.ONE, GL.ONE_MINUS_SRC_ALPHA);
			gl.BlendEquationSeparate(GL.FUNC_ADD, GL.FUNC_ADD);
			float num = 0.25f;
			BasicProgram basicProgram = this._gpuProgramStore.BasicProgram;
			gl.UseProgram(basicProgram);
			Matrix matrix = Matrix.CreateTranslation(this._debugCameraPosition);
			matrix = Matrix.Multiply(matrix, viewProjectionMatrix);
			bool isDebuggingCameraFrustum = this._isDebuggingCameraFrustum;
			if (isDebuggingCameraFrustum)
			{
				basicProgram.MVPMatrix.SetValue(ref matrix);
				basicProgram.Opacity.SetValue(num);
				basicProgram.Color.SetValue(new Vector3(1f, 1f, 1f) * num);
				gl.BindVertexArray(this._debugCameraFrustumMesh.VertexArray);
				gl.DrawElements(GL.TRIANGLES, this._debugCameraFrustumMesh.Count, GL.UNSIGNED_SHORT, (IntPtr)0);
				gl.PolygonMode(GL.FRONT_AND_BACK, GL.LINE);
				basicProgram.Opacity.SetValue(1f);
				gl.DrawElements(GL.TRIANGLES, this._debugCameraFrustumMesh.Count, GL.UNSIGNED_SHORT, (IntPtr)0);
				gl.PolygonMode(GL.FRONT_AND_BACK, GL.FILL);
			}
			bool isDebuggingCameraFrustumSplits = this._isDebuggingCameraFrustumSplits;
			if (isDebuggingCameraFrustumSplits)
			{
				for (int i = 0; i < this._sunShadowCascades.Count; i++)
				{
					basicProgram.MVPMatrix.SetValue(ref matrix);
					basicProgram.Opacity.SetValue(num);
					basicProgram.Color.SetValue(this._debugCascadeColors[i] * num);
					gl.BindVertexArray(this._debugCameraFrustumSplitMeshes[i].VertexArray);
					gl.DrawElements(GL.TRIANGLES, this._debugCameraFrustumSplitMeshes[i].Count, GL.UNSIGNED_SHORT, (IntPtr)0);
					gl.PolygonMode(GL.FRONT_AND_BACK, GL.LINE);
					basicProgram.Opacity.SetValue(1f);
					basicProgram.Color.SetValue(this._debugCascadeColors[i]);
					gl.DrawElements(GL.TRIANGLES, this._debugCameraFrustumSplitMeshes[i].Count, GL.UNSIGNED_SHORT, (IntPtr)0);
					gl.PolygonMode(GL.FRONT_AND_BACK, GL.FILL);
				}
			}
			bool isDebuggingShadowCascadeFrustums = this._isDebuggingShadowCascadeFrustums;
			if (isDebuggingShadowCascadeFrustums)
			{
				for (int j = 0; j < this._sunShadowCascades.Count; j++)
				{
					basicProgram.MVPMatrix.SetValue(ref matrix);
					basicProgram.Opacity.SetValue(num);
					basicProgram.Color.SetValue(this._debugCascadeColors[j] * num);
					gl.BindVertexArray(this._debugShadowCascadeFrustumMeshes[j].VertexArray);
					gl.DrawElements(GL.TRIANGLES, this._debugShadowCascadeFrustumMeshes[j].Count, GL.UNSIGNED_SHORT, (IntPtr)0);
					gl.PolygonMode(GL.FRONT_AND_BACK, GL.LINE);
					basicProgram.Opacity.SetValue(1f);
					basicProgram.Color.SetValue(this._debugCascadeColors[j]);
					gl.DrawElements(GL.TRIANGLES, this._debugShadowCascadeFrustumMeshes[j].Count, GL.UNSIGNED_SHORT, (IntPtr)0);
					gl.PolygonMode(GL.FRONT_AND_BACK, GL.FILL);
				}
			}
			gl.BlendEquationSeparate(GL.FUNC_ADD, GL.FUNC_ADD);
			gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
			gl.Disable(GL.DEPTH_TEST);
			gl.DepthMask(true);
		}

		// Token: 0x06004EDB RID: 20187 RVA: 0x001603B4 File Offset: 0x0015E5B4
		private void InitShadowCascadesDebug()
		{
			this._debugCascadeColors[0] = new Vector3(1f, 0f, 0f);
			this._debugCascadeColors[1] = new Vector3(0f, 1f, 0f);
			this._debugCascadeColors[2] = new Vector3(0f, 0f, 1f);
			this._debugCascadeColors[3] = new Vector3(1f, 1f, 0f);
		}

		// Token: 0x06004EDC RID: 20188 RVA: 0x00160444 File Offset: 0x0015E644
		private void DisposeShadowCascadesDebug()
		{
			bool isDebuggingCameraFrustum = this._isDebuggingCameraFrustum;
			if (isDebuggingCameraFrustum)
			{
				this._debugCameraFrustumMesh.Dispose();
			}
			bool isDebuggingCameraFrustumSplits = this._isDebuggingCameraFrustumSplits;
			if (isDebuggingCameraFrustumSplits)
			{
				for (int i = 0; i < 4; i++)
				{
					this._debugCameraFrustumSplitMeshes[i].Dispose();
				}
			}
			bool isDebuggingShadowCascadeFrustums = this._isDebuggingShadowCascadeFrustums;
			if (isDebuggingShadowCascadeFrustums)
			{
				for (int j = 0; j < 4; j++)
				{
					this._debugShadowCascadeFrustumMeshes[j].Dispose();
				}
			}
		}

		// Token: 0x040029FA RID: 10746
		public static Vector2 DefaultDeferredShadowResolutionScale = new Vector2(1f);

		// Token: 0x040029FB RID: 10747
		private GraphicsDevice _graphics;

		// Token: 0x040029FC RID: 10748
		private GPUProgramStore _gpuProgramStore;

		// Token: 0x040029FD RID: 10749
		private RenderTargetStore _renderTargetStore;

		// Token: 0x040029FE RID: 10750
		private CascadedShadowMapping.CascadedShadowsBuildSettings _sunShadowCascades;

		// Token: 0x040029FF RID: 10751
		private CascadedShadowMapping.DeferredShadowsSettings _sunDeferredShadows;

		// Token: 0x04002A00 RID: 10752
		private CascadedShadowMapping.InputParams _csmInputParams;

		// Token: 0x04002A01 RID: 10753
		private float _maxWorldHeight = 320f;

		// Token: 0x04002A02 RID: 10754
		private bool[] _cascadeNeedsUpdate = new bool[4];

		// Token: 0x04002A03 RID: 10755
		private BoundingFrustum[] _cascadeFrustums = new BoundingFrustum[4];

		// Token: 0x04002A04 RID: 10756
		private Vector2[] _cascadeDistanceAndTexelScales = new Vector2[4];

		// Token: 0x04002A05 RID: 10757
		private Vector3[] _cascadeCachedTranslations = new Vector3[4];

		// Token: 0x04002A06 RID: 10758
		private float[] _cascadeSizes = new float[4];

		// Token: 0x04002A07 RID: 10759
		private float[] _cascadeDistances = new float[5];

		// Token: 0x04002A08 RID: 10760
		private Vector3[] _tmpFrustumCorners = new Vector3[8];

		// Token: 0x04002A09 RID: 10761
		private Action _shadowCastersDrawFunc;

		// Token: 0x04002A0A RID: 10762
		private bool _isDebuggingFreeze;

		// Token: 0x04002A0B RID: 10763
		private bool _isDebuggingCameraFrustum = false;

		// Token: 0x04002A0C RID: 10764
		private bool _isDebuggingCameraFrustumSplits = false;

		// Token: 0x04002A0D RID: 10765
		private bool _isDebuggingShadowCascadeFrustums = false;

		// Token: 0x04002A0E RID: 10766
		private Vector3 _debugCameraPosition;

		// Token: 0x04002A0F RID: 10767
		private Mesh _debugCameraFrustumMesh;

		// Token: 0x04002A10 RID: 10768
		private Mesh[] _debugCameraFrustumSplitMeshes = new Mesh[4];

		// Token: 0x04002A11 RID: 10769
		private Mesh[] _debugShadowCascadeFrustumMeshes = new Mesh[4];

		// Token: 0x04002A12 RID: 10770
		private Vector3[] _debugCascadeColors = new Vector3[4];

		// Token: 0x02000E91 RID: 3729
		public struct RenderData
		{
			// Token: 0x04004710 RID: 18192
			public const int MaxShadowMapCascades = 4;

			// Token: 0x04004711 RID: 18193
			public float DynamicShadowIntensity;

			// Token: 0x04004712 RID: 18194
			public BoundingFrustum VirtualSunViewFrustum;

			// Token: 0x04004713 RID: 18195
			public KDop VirtualSunKDopFrustum;

			// Token: 0x04004714 RID: 18196
			public Vector3 VirtualSunPosition;

			// Token: 0x04004715 RID: 18197
			public Vector3 VirtualSunDirection;

			// Token: 0x04004716 RID: 18198
			public Vector3[] VirtualSunPositions;

			// Token: 0x04004717 RID: 18199
			public Matrix[] VirtualSunViewRotationMatrix;

			// Token: 0x04004718 RID: 18200
			public Matrix[] VirtualSunProjectionMatrix;

			// Token: 0x04004719 RID: 18201
			public Matrix[] VirtualSunViewRotationProjectionMatrix;

			// Token: 0x0400471A RID: 18202
			public Vector2[] CascadeDistanceAndTexelScales;

			// Token: 0x0400471B RID: 18203
			public Vector3[] CascadeCachedTranslations;
		}

		// Token: 0x02000E92 RID: 3730
		public struct InputParams
		{
			// Token: 0x0400471C RID: 18204
			public Vector3 LightDirection;

			// Token: 0x0400471D RID: 18205
			public float WorldFieldOfView;

			// Token: 0x0400471E RID: 18206
			public float AspectRatio;

			// Token: 0x0400471F RID: 18207
			public float NearClipDistance;

			// Token: 0x04004720 RID: 18208
			public Vector3 CameraPosition;

			// Token: 0x04004721 RID: 18209
			public Matrix ViewRotationMatrix;

			// Token: 0x04004722 RID: 18210
			public Matrix ViewRotationProjectionMatrix;

			// Token: 0x04004723 RID: 18211
			public bool IsSpatialContinuityLost;

			// Token: 0x04004724 RID: 18212
			public Vector2 QuantifiedCameraMotion;

			// Token: 0x04004725 RID: 18213
			public Vector3 CameraPositionDelta;

			// Token: 0x04004726 RID: 18214
			public uint FrameId;
		}

		// Token: 0x02000E93 RID: 3731
		public struct CascadedShadowsBuildSettings
		{
			// Token: 0x04004727 RID: 18215
			public int Count;

			// Token: 0x04004728 RID: 18216
			public bool UseGlobalKDop;

			// Token: 0x04004729 RID: 18217
			public bool UseCaching;

			// Token: 0x0400472A RID: 18218
			public bool UseStableProjection;

			// Token: 0x0400472B RID: 18219
			public bool UseSlopeScaleBias;

			// Token: 0x0400472C RID: 18220
			public Vector2 SlopeScaleBias;
		}

		// Token: 0x02000E94 RID: 3732
		public struct DeferredShadowsSettings
		{
			// Token: 0x0400472D RID: 18221
			public float ResolutionScale;

			// Token: 0x0400472E RID: 18222
			public bool UseBlur;

			// Token: 0x0400472F RID: 18223
			public bool UseNoise;

			// Token: 0x04004730 RID: 18224
			public bool UseManualMode;

			// Token: 0x04004731 RID: 18225
			public bool UseFading;

			// Token: 0x04004732 RID: 18226
			public bool UseSingleSample;

			// Token: 0x04004733 RID: 18227
			public bool UseCameraBias;

			// Token: 0x04004734 RID: 18228
			public bool UseNormalBias;
		}
	}
}
