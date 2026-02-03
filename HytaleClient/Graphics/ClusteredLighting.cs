using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;
using HytaleClient.Utils;

namespace HytaleClient.Graphics
{
	// Token: 0x0200099E RID: 2462
	internal class ClusteredLighting
	{
		// Token: 0x1700129E RID: 4766
		// (get) Token: 0x06004EE6 RID: 20198 RVA: 0x001616B0 File Offset: 0x0015F8B0
		public uint GridWidth
		{
			get
			{
				return this._lightGridWidth;
			}
		}

		// Token: 0x1700129F RID: 4767
		// (get) Token: 0x06004EE7 RID: 20199 RVA: 0x001616B8 File Offset: 0x0015F8B8
		public uint GridHeight
		{
			get
			{
				return this._lightGridHeight;
			}
		}

		// Token: 0x170012A0 RID: 4768
		// (get) Token: 0x06004EE8 RID: 20200 RVA: 0x001616C0 File Offset: 0x0015F8C0
		public uint GridDepth
		{
			get
			{
				return this._lightGridDepth;
			}
		}

		// Token: 0x170012A1 RID: 4769
		// (get) Token: 0x06004EE9 RID: 20201 RVA: 0x001616C8 File Offset: 0x0015F8C8
		public float GridNearZ
		{
			get
			{
				return this._gridNearZ;
			}
		}

		// Token: 0x170012A2 RID: 4770
		// (get) Token: 0x06004EEA RID: 20202 RVA: 0x001616D0 File Offset: 0x0015F8D0
		public float GridFarZ
		{
			get
			{
				return this._gridFarZ;
			}
		}

		// Token: 0x170012A3 RID: 4771
		// (get) Token: 0x06004EEB RID: 20203 RVA: 0x001616D8 File Offset: 0x0015F8D8
		public float GridRangeCoef
		{
			get
			{
				return this._coef;
			}
		}

		// Token: 0x170012A4 RID: 4772
		// (get) Token: 0x06004EEC RID: 20204 RVA: 0x001616E0 File Offset: 0x0015F8E0
		public int LightCount
		{
			get
			{
				return this._globalLightDataCount;
			}
		}

		// Token: 0x170012A5 RID: 4773
		// (get) Token: 0x06004EED RID: 20205 RVA: 0x001616E8 File Offset: 0x0015F8E8
		public GLTexture LightGridTexture
		{
			get
			{
				return this._lightGridTexture.GLTexture;
			}
		}

		// Token: 0x170012A6 RID: 4774
		// (get) Token: 0x06004EEE RID: 20206 RVA: 0x001616F5 File Offset: 0x0015F8F5
		public GLTexture DirectPointLightGpuBufferTexture
		{
			get
			{
				return this._directPointLightBufferTexture.CurrentTexture;
			}
		}

		// Token: 0x170012A7 RID: 4775
		// (get) Token: 0x06004EEF RID: 20207 RVA: 0x00161702 File Offset: 0x0015F902
		public GLTexture LightIndicesGpuBufferTexture
		{
			get
			{
				return this._lightIndicesBufferTexture.CurrentTexture;
			}
		}

		// Token: 0x170012A8 RID: 4776
		// (get) Token: 0x06004EF0 RID: 20208 RVA: 0x0016170F File Offset: 0x0015F90F
		public GLBuffer PointLightsGpuBuffer
		{
			get
			{
				return this._pointLightsBuffer.Current;
			}
		}

		// Token: 0x06004EF1 RID: 20209 RVA: 0x0016171C File Offset: 0x0015F91C
		public void UseRefinedVoxelization(bool enable)
		{
			this._useRefinedVoxelization = enable;
		}

		// Token: 0x06004EF2 RID: 20210 RVA: 0x00161725 File Offset: 0x0015F925
		public void UseMappedGPUBuffers(bool enable)
		{
			this._useMappedGPUBuffers = enable;
		}

		// Token: 0x06004EF3 RID: 20211 RVA: 0x0016172E File Offset: 0x0015F92E
		public void UseLightDirectAccess(bool enable)
		{
			this._useLightDirectAccess = enable;
		}

		// Token: 0x06004EF4 RID: 20212 RVA: 0x00161737 File Offset: 0x0015F937
		public void UsePBO(bool enable)
		{
			this._usePBO = enable;
		}

		// Token: 0x06004EF5 RID: 20213 RVA: 0x00161740 File Offset: 0x0015F940
		public void UseDoubleBuffering(bool enable)
		{
			this._useDoubleBuffering = enable;
		}

		// Token: 0x06004EF6 RID: 20214 RVA: 0x00161749 File Offset: 0x0015F949
		public void UseParallelExecution(bool enable)
		{
			this._useParallelExecution = enable;
		}

		// Token: 0x06004EF7 RID: 20215 RVA: 0x00161754 File Offset: 0x0015F954
		public ClusteredLighting(GraphicsDevice graphics, RenderTargetStore renderTargetStore, Profiling profiling)
		{
			this._graphics = graphics;
			this._gl = this._graphics.GL;
			this._renderTargetStore = renderTargetStore;
			this._profiling = profiling;
		}

		// Token: 0x06004EF8 RID: 20216 RVA: 0x001617F4 File Offset: 0x0015F9F4
		public void Init()
		{
			this._lightGridTexture = new Texture(Texture.TextureTypes.Texture3D);
			this._lightGridTexture.CreateTexture3D(1, 1, 1, IntPtr.Zero, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.UNSIGNED_SHORT, GL.RG16UI, GL.RG_INTEGER, false);
			this._directPointLightCount = 0U;
			this._pointLightsBuffer.CreateStorage(GL.UNIFORM_BUFFER, GL.STREAM_DRAW, true, (uint)(this._pointLightData.Length * 4 * 4 * 2), 0U, GPUBuffer.GrowthPolicy.Never, 0U);
			this.SetGridResolution(16U, 8U, 24U);
		}

		// Token: 0x06004EF9 RID: 20217 RVA: 0x00161886 File Offset: 0x0015FA86
		public void Dispose()
		{
			this._pointLightsBuffer.DestroyStorage();
			this._lightIndicesBufferTexture.DestroyStorage();
			this._directPointLightBufferTexture.DestroyStorage();
			this._lightGridPBO.DestroyStorage();
			this._lightGridTexture.Dispose();
		}

		// Token: 0x06004EFA RID: 20218 RVA: 0x001618C5 File Offset: 0x0015FAC5
		public void SetupRenderingProfiles(int profileLightClusterClear, int profileLightClustering, int profileLightClusteringRefine, int profileLightFillGridData, int profileLightSendDataToGPU)
		{
			this._renderingProfileLightClusterClear = profileLightClusterClear;
			this._renderingProfileLightClustering = profileLightClustering;
			this._renderingProfileLightClusteringRefine = profileLightClusteringRefine;
			this._renderingProfileLightFillGridData = profileLightFillGridData;
			this._renderingProfileLightSendDataToGPU = profileLightSendDataToGPU;
		}

		// Token: 0x06004EFB RID: 20219 RVA: 0x001618ED File Offset: 0x0015FAED
		public void UseCustomZDistribution(bool custom)
		{
			this._useCustomZDistribution = custom;
			this._gridNearZ = (this._useCustomZDistribution ? 5f : 0.1f);
			this.SetupGrid();
		}

		// Token: 0x06004EFC RID: 20220 RVA: 0x00161918 File Offset: 0x0015FB18
		public void ChangeGridResolution(uint width, uint height, uint depth)
		{
			this._lightIndicesBufferTexture.DestroyStorage();
			this._directPointLightBufferTexture.DestroyStorage();
			this._lightGridPBO.DestroyStorage();
			this.SetGridResolution(width, height, depth);
		}

		// Token: 0x06004EFD RID: 20221 RVA: 0x0016194C File Offset: 0x0015FB4C
		public void SetGridResolution(uint width, uint height, uint depth)
		{
			this._lightGridWidth = width;
			this._lightGridHeight = height;
			this._lightGridDepth = depth;
			this._lightGridDataCount = this._lightGridWidth * this._lightGridHeight * this._lightGridDepth;
			this._lightGridData = new ushort[this._lightGridDataCount * 2U];
			uint num = 256U * this._lightGridDataCount;
			this._lightIndices = new ushort[num];
			this._directPointLightData = new Vector4[this._lightGridDataCount * 256U * 2U];
			this._lightGridPBO.CreateStorage(GL.PIXEL_UNPACK_BUFFER, GL.STREAM_DRAW, true, (uint)(this._lightGridData.Length * 2), 0U, GPUBuffer.GrowthPolicy.Never, 0U);
			this._lightGridTexture.UpdateTexture3D(width, height, depth, null);
			this._directPointLightBufferTexture.CreateStorage(GL.RGBA32F, GL.STREAM_DRAW, true, (uint)(this._directPointLightData.Length * 4 * 4), 0U, GPUBuffer.GrowthPolicy.Never, 0U);
			this._lightIndicesBufferTexture.CreateStorage(GL.R16UI, GL.STREAM_DRAW, true, (uint)(this._lightIndices.Length * 2), 0U, GPUBuffer.GrowthPolicy.Never, 0U);
			this._zSliceLightData = new ClusteredLighting.ZSliceLightData[this._lightGridDepth];
			for (int i = 0; i < this._zSliceLightData.Length; i++)
			{
				this._zSliceLightData[i] = new ClusteredLighting.ZSliceLightData(this._lightGridWidth, this._lightGridHeight);
			}
			this._zSlicesCount = (int)this._lightGridDepth;
			this._depthSlices = new float[this._zSlicesCount + 1];
			this.SetupGrid();
		}

		// Token: 0x06004EFE RID: 20222 RVA: 0x00161AB8 File Offset: 0x0015FCB8
		public void BuildFrustumGridPlanes()
		{
			uint lightGridWidth = this._lightGridWidth;
			uint lightGridHeight = this._lightGridHeight;
			uint lightGridDepth = this._lightGridDepth;
			bool flag = this._planesZ == null || (long)this._planesZ.Length != (long)((ulong)(lightGridDepth + 1U));
			if (flag)
			{
				this._planesZ = new Plane[lightGridDepth + 1U];
			}
			bool flag2 = this._planesY == null || (long)this._planesY.Length != (long)((ulong)(lightGridHeight + 1U));
			if (flag2)
			{
				this._planesY = new Plane[lightGridHeight + 1U];
			}
			bool flag3 = this._planesX == null || (long)this._planesX.Length != (long)((ulong)(lightGridWidth + 1U));
			if (flag3)
			{
				this._planesX = new Plane[lightGridWidth + 1U];
			}
			int num = 0;
			while ((long)num <= (long)((ulong)lightGridWidth))
			{
				float x = (float)num / lightGridWidth * 2f - 1f;
				Vector3 pointB = this.ConvertProjectionSpaceToViewSpace(new Vector3(x, 1f, -1f));
				Vector3 pointA = this.ConvertProjectionSpaceToViewSpace(new Vector3(x, -1f, -1f));
				this._planesX[num] = this.CreatePlaneAtOrigin(pointA, pointB);
				num++;
			}
			int num2 = 0;
			while ((long)num2 <= (long)((ulong)lightGridHeight))
			{
				float y = (float)num2 / lightGridHeight * 2f - 1f;
				Vector3 pointB2 = this.ConvertProjectionSpaceToViewSpace(new Vector3(-1f, y, -1f));
				Vector3 pointA2 = this.ConvertProjectionSpaceToViewSpace(new Vector3(1f, y, -1f));
				this._planesY[num2] = this.CreatePlaneAtOrigin(pointA2, pointB2);
				num2++;
			}
			int num3 = 0;
			while ((long)num3 <= (long)((ulong)lightGridDepth))
			{
				this._planesZ[num3] = new Plane(Vector3.Forward, this._depthSlices[num3]);
				num3++;
			}
		}

		// Token: 0x06004EFF RID: 20223 RVA: 0x00161CA0 File Offset: 0x0015FEA0
		private void SpheresVoxelizationPerSlice(uint z)
		{
			for (int i = 0; i < this._globalLightDataCount; i++)
			{
				bool flag = (uint)this._lightClusterBBox[i].minZ <= z && z <= (uint)this._lightClusterBBox[i].maxZ;
				if (flag)
				{
					ref Vector4 ptr = ref this._pointLightData[2 * i];
					for (byte b = this._lightClusterBBox[i].minY; b <= this._lightClusterBBox[i].maxY; b += 1)
					{
						for (byte b2 = this._lightClusterBBox[i].minX; b2 <= this._lightClusterBBox[i].maxX; b2 += 1)
						{
							this.RegisterLightInCluster((ushort)i, (uint)b2, (uint)b, z);
						}
					}
				}
			}
		}

		// Token: 0x06004F00 RID: 20224 RVA: 0x00161D94 File Offset: 0x0015FF94
		public void Prepare(LightData[] lightData, int lightCount, float WorldFieldOfView, Vector3 cameraPosition, ref Matrix viewRotationMatrix, ref Matrix projectionMatrix)
		{
			bool useDoubleBuffering = this._useDoubleBuffering;
			if (useDoubleBuffering)
			{
				this.PingPongBuffers();
			}
			this._globalLightDataCount = Math.Min(1024, lightCount);
			bool flag = projectionMatrix != this._projectionMatrix;
			if (flag)
			{
				this._projectionMatrix = projectionMatrix;
				this._invProjectionMatrix = Matrix.Invert(this._projectionMatrix);
				this.BuildFrustumGridPlanes();
			}
			this._profiling.StartMeasure(this._renderingProfileLightClusterClear);
			this.ClearLightGridData();
			this._profiling.StopMeasure(this._renderingProfileLightClusterClear);
			this._profiling.StartMeasure(this._renderingProfileLightClustering);
			float num = 1f / (float)Math.Tan((double)(WorldFieldOfView / 2f));
			Vector3 column = projectionMatrix.Column0;
			Vector3 column2 = projectionMatrix.Column1;
			Vector3 column3 = projectionMatrix.Column3;
			for (int i = 0; i < this._globalLightDataCount; i++)
			{
				ref Vector3 ptr = ref lightData[i].Color;
				ref BoundingSphere ptr2 = ref lightData[i].Sphere;
				float radius = lightData[i].Sphere.Radius;
				Vector4 vector = new Vector4(ptr2.Center.X, ptr2.Center.Y, ptr2.Center.Z, 1f);
				vector.X -= cameraPosition.X;
				vector.Y -= cameraPosition.Y;
				vector.Z -= cameraPosition.Z;
				vector = Vector4.Transform(vector, viewRotationMatrix);
				this._pointLightData[2 * i] = new Vector4(vector.X, vector.Y, vector.Z, radius);
				this._pointLightData[2 * i + 1] = new Vector4(ptr.X, ptr.Y, ptr.Z, 1f);
				float num2 = vector.Z + radius;
				float num3 = vector.Z - radius;
				this._lightClusterBBox[i].minZ = (byte)this.GetLightGridDepthSlice(-num2);
				this._lightClusterBBox[i].maxZ = (byte)this.GetLightGridDepthSlice(-num3);
				bool flag2 = lightData[i].Sphere.Contains(cameraPosition) == ContainmentType.Contains;
				Vector2 zero;
				Vector2 one;
				if (flag2)
				{
					zero = Vector2.Zero;
					one = Vector2.One;
				}
				else
				{
					Vector3 vector2 = new Vector3(vector.X, vector.Y, vector.Z);
					this.GetBoundingBox(ref vector2, radius, -0.1f, ref column, ref column2, ref column3, out zero, out one);
				}
				this._lightClusterBBox[i].minX = (byte)MathHelper.Clamp(zero.X * this._lightGridWidth, 0f, this._lightGridWidth - 1U);
				this._lightClusterBBox[i].maxX = (byte)MathHelper.Clamp(one.X * this._lightGridWidth, 0f, this._lightGridWidth - 1U);
				this._lightClusterBBox[i].minY = (byte)MathHelper.Clamp(zero.Y * this._lightGridHeight, 0f, this._lightGridHeight - 1U);
				this._lightClusterBBox[i].maxY = (byte)MathHelper.Clamp(one.Y * this._lightGridHeight, 0f, this._lightGridHeight - 1U);
				bool useRefinedVoxelization = this._useRefinedVoxelization;
				if (useRefinedVoxelization)
				{
					Vector4 vector3;
					Vector4.Transform(ref vector, ref projectionMatrix, out vector3);
					vector3 /= vector3.W;
					vector3.X = vector3.X * 0.5f + 0.5f;
					vector3.Y = vector3.Y * 0.5f + 0.5f;
					vector3.Z = vector3.Z * 0.5f + 0.5f;
					this._lightRefinementInfo[i].CenterZ = this.GetLightGridDepthSlice(-vector.Z);
					this._lightRefinementInfo[i].CenterY = (uint)Math.Floor((double)(vector3.Y * this._lightGridHeight));
				}
				Debug.Assert((uint)this._lightClusterBBox[i].minZ < this._lightGridDepth && (uint)this._lightClusterBBox[i].maxZ < this._lightGridDepth, "Error in the light cluster bounding box computation on axis Z");
				Debug.Assert((uint)this._lightClusterBBox[i].minY < this._lightGridHeight && (uint)this._lightClusterBBox[i].maxY < this._lightGridHeight, "Error in the light cluster bounding box computation on axis Y");
				Debug.Assert((uint)this._lightClusterBBox[i].minX < this._lightGridWidth && (uint)this._lightClusterBBox[i].maxX < this._lightGridWidth, "Error in the light cluster bounding box computation on axis X");
			}
			this._profiling.StopMeasure(this._renderingProfileLightClustering);
			this._profiling.StartMeasure(this._renderingProfileLightClusteringRefine);
			bool flag3 = !this._useRefinedVoxelization;
			if (flag3)
			{
				bool useParallelExecution = this._useParallelExecution;
				if (useParallelExecution)
				{
					Parallel.For(0L, (long)((ulong)this._lightGridDepth), delegate(long z)
					{
						this.SpheresVoxelizationPerSlice((uint)z);
					});
				}
				else
				{
					int num4 = 0;
					while ((long)num4 < (long)((ulong)this._lightGridDepth))
					{
						this.SpheresVoxelizationPerSlice((uint)num4);
						num4++;
					}
				}
			}
			else
			{
				bool useParallelExecution2 = this._useParallelExecution;
				if (useParallelExecution2)
				{
					Parallel.For(0L, (long)((ulong)this._lightGridDepth), delegate(long z)
					{
						this.RefineSpheresVoxelizationPerSlice((uint)z);
					});
				}
				else
				{
					int num5 = 0;
					while ((long)num5 < (long)((ulong)this._lightGridDepth))
					{
						this.RefineSpheresVoxelizationPerSlice((uint)num5);
						num5++;
					}
				}
			}
			this._profiling.StopMeasure(this._renderingProfileLightClusteringRefine);
			this._profiling.StartMeasure(this._renderingProfileLightFillGridData);
			this.FillClusteredLightingBuffers();
			this._profiling.StopMeasure(this._renderingProfileLightFillGridData);
		}

		// Token: 0x06004F01 RID: 20225 RVA: 0x001623D0 File Offset: 0x001605D0
		public unsafe void SendDataToGPU()
		{
			this._profiling.StartMeasure(this._renderingProfileLightSendDataToGPU);
			int xoffset = 0;
			int yoffset = 0;
			int zoffset = 0;
			bool usePBO = this._usePBO;
			if (usePBO)
			{
				this._lightGridPBO.UnpackToTexture3D(this._lightGridTexture.GLTexture, 0, xoffset, yoffset, zoffset, (int)this._lightGridWidth, (int)this._lightGridHeight, (int)this._lightGridDepth, GL.RG_INTEGER, GL.UNSIGNED_SHORT);
			}
			else
			{
				this._lightGridTexture.UpdateTexture3D(this._lightGridWidth, this._lightGridHeight, this._lightGridDepth, this._lightGridData);
			}
			bool flag = !this._useLightDirectAccess;
			if (flag)
			{
				Vector4[] array;
				Vector4* value;
				if ((array = this._pointLightData) == null || array.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array[0];
				}
				this._pointLightsBuffer.TransferCopy((IntPtr)((void*)value), (uint)(this._globalLightDataCount * 4 * 8), 0U);
				array = null;
				bool flag2 = !this._useMappedGPUBuffers;
				if (flag2)
				{
					ushort[] array2;
					ushort* value2;
					if ((array2 = this._lightIndices) == null || array2.Length == 0)
					{
						value2 = null;
					}
					else
					{
						value2 = &array2[0];
					}
					this._lightIndicesBufferTexture.TransferCopy((IntPtr)((void*)value2), this._lightIndicesCount * 2U, 0U);
					array2 = null;
				}
			}
			else
			{
				bool flag3 = !this._useMappedGPUBuffers;
				if (flag3)
				{
					Vector4[] array;
					Vector4* value3;
					if ((array = this._directPointLightData) == null || array.Length == 0)
					{
						value3 = null;
					}
					else
					{
						value3 = &array[0];
					}
					this._directPointLightBufferTexture.TransferCopy((IntPtr)((void*)value3), this._directPointLightCount * 2U * 4U * 4U, 0U);
					array = null;
				}
			}
			this._profiling.StopMeasure(this._renderingProfileLightSendDataToGPU);
		}

		// Token: 0x06004F02 RID: 20226 RVA: 0x0016257C File Offset: 0x0016077C
		public void SkipMeasures()
		{
			this._profiling.SkipMeasure(this._renderingProfileLightClusterClear);
			this._profiling.SkipMeasure(this._renderingProfileLightClustering);
			this._profiling.SkipMeasure(this._renderingProfileLightClusteringRefine);
			this._profiling.SkipMeasure(this._renderingProfileLightFillGridData);
			this._profiling.SkipMeasure(this._renderingProfileLightSendDataToGPU);
		}

		// Token: 0x06004F03 RID: 20227 RVA: 0x001625E4 File Offset: 0x001607E4
		public void SetupLightDataTextures(uint gridTextureUnit, uint indicesOrDataTextureUnit)
		{
			GLFunctions gl = this._graphics.GL;
			GLTexture texture = this._useLightDirectAccess ? this.DirectPointLightGpuBufferTexture : this.LightIndicesGpuBufferTexture;
			gl.ActiveTexture(GL.TEXTURE0 + indicesOrDataTextureUnit);
			gl.BindTexture(GL.TEXTURE_BUFFER, texture);
			gl.ActiveTexture(GL.TEXTURE0 + gridTextureUnit);
			gl.BindTexture(GL.TEXTURE_3D, this.LightGridTexture);
		}

		// Token: 0x06004F04 RID: 20228 RVA: 0x00162650 File Offset: 0x00160850
		public void DrawDeferredLights(Vector3[] frustumFarCornersVS, ref Matrix projectionMatrix, bool fullResolution, bool secondPass)
		{
			this._gl.AssertDepthMask(false);
			this._gl.AssertBlendFunc(GL.SRC_ALPHA, GL.ONE);
			this._gl.AssertEnabled(GL.BLEND);
			GLFunctions gl = this._graphics.GL;
			LightClusteredProgram lightClusteredProgram = this._graphics.GPUProgramStore.LightClusteredProgram;
			gl.UseProgram(lightClusteredProgram);
			bool flag = !secondPass;
			if (flag)
			{
				this.SetupLightDataTextures(1U, 2U);
				this._gl.ActiveTexture(GL.TEXTURE0);
				bool useLinearZForLight = this._graphics.UseLinearZForLight;
				if (useLinearZForLight)
				{
					lightClusteredProgram.FarCorners.SetValue(frustumFarCornersVS);
				}
				else
				{
					lightClusteredProgram.ProjectionMatrix.SetValue(ref projectionMatrix);
				}
				bool flag2 = !this._useLightDirectAccess;
				if (flag2)
				{
					lightClusteredProgram.PointLightBlock.SetBuffer(this.PointLightsGpuBuffer);
				}
				lightClusteredProgram.FarClip.SetValue(1024f);
				lightClusteredProgram.LightGridResolution.SetValue(this.GridWidth, this.GridHeight, this.GridDepth);
				lightClusteredProgram.ZSlicesParams.SetValue(this.GridNearZ, this.GridFarZ, this.GridRangeCoef);
			}
			GLTexture texture = this._graphics.UseLinearZForLight ? this._renderTargetStore.LinearZ.GetTexture(RenderTarget.Target.Color0) : this._renderTargetStore.GBuffer.GetTexture(RenderTarget.Target.Depth);
			this._gl.BindTexture(GL.TEXTURE_2D, texture);
			float value = fullResolution ? 1f : 0f;
			lightClusteredProgram.UseLBufferCompression.SetValue(value);
			this._graphics.ScreenTriangleRenderer.Draw();
			this._gl.AssertDepthMask(false);
			this._gl.AssertBlendFunc(GL.SRC_ALPHA, GL.ONE);
			this._gl.AssertEnabled(GL.BLEND);
		}

		// Token: 0x06004F05 RID: 20229 RVA: 0x00162824 File Offset: 0x00160A24
		private void PingPongBuffers()
		{
			this._lightGridPBO.Swap();
			this._lightIndicesBufferTexture.Swap();
			this._directPointLightBufferTexture.Swap();
			this._pointLightsBuffer.Swap();
		}

		// Token: 0x06004F06 RID: 20230 RVA: 0x00162857 File Offset: 0x00160A57
		private void SetupGrid()
		{
			ClusteredLighting.ComputeGridDepthSlices(this._useCustomZDistribution, this._zSlicesCount, this._gridNearZ, this._gridFarZ, ref this._coef, ref this._depthSlices);
			this.BuildFrustumGridPlanes();
		}

		// Token: 0x06004F07 RID: 20231 RVA: 0x0016288C File Offset: 0x00160A8C
		private static void ComputeGridDepthSlices(bool customNearZ, int zSliceCount, float nearZ, float farZ, ref float distributionCoef, ref float[] depthSlices)
		{
			int num = customNearZ ? 1 : 0;
			int num2 = customNearZ ? (zSliceCount - 1) : zSliceCount;
			float num3 = farZ / nearZ;
			distributionCoef = 1f / (float)Math.Log((double)num3);
			depthSlices[0] = 0.1f;
			for (int i = 0; i <= num2; i++)
			{
				depthSlices[i + num] = (float)((double)nearZ * Math.Pow((double)num3, (double)((float)i / (float)num2)));
			}
		}

		// Token: 0x06004F08 RID: 20232 RVA: 0x001628FC File Offset: 0x00160AFC
		private static uint GetLightGridDepthSlice(bool customNearZ, int zSliceCount, float nearZ, float distributionCoef, float z)
		{
			int num = customNearZ ? (zSliceCount - 1) : zSliceCount;
			double num2 = Math.Log((double)(z / nearZ)) * (double)distributionCoef;
			uint num3 = (uint)Math.Max(0.0, (double)num * num2);
			return customNearZ ? ((z < nearZ) ? 0U : (num3 + 1U)) : num3;
		}

		// Token: 0x06004F09 RID: 20233 RVA: 0x00162950 File Offset: 0x00160B50
		private uint GetLightGridDepthSlice(float z)
		{
			return ClusteredLighting.GetLightGridDepthSlice(this._useCustomZDistribution, this._zSlicesCount, this._gridNearZ, this._coef, z);
		}

		// Token: 0x06004F0A RID: 20234 RVA: 0x00162980 File Offset: 0x00160B80
		private void ProjectSphereToPlane(ref Plane plane, ref BoundingSphere sphere, out BoundingSphere projectedSphere)
		{
			float num;
			Vector3.Dot(ref plane.Normal, ref sphere.Center, out num);
			num -= plane.D;
			projectedSphere.Center = sphere.Center - plane.Normal * num;
			projectedSphere.Radius = (float)Math.Sqrt(Math.Max(0.0, (double)(sphere.Radius * sphere.Radius - num * num)));
		}

		// Token: 0x06004F0B RID: 20235 RVA: 0x001629F4 File Offset: 0x00160BF4
		private Vector3 ConvertProjectionSpaceToViewSpace(Vector3 posPS)
		{
			Vector4 vector = Vector4.Transform(posPS, this._invProjectionMatrix);
			return new Vector3(vector.X / vector.W, vector.Y / vector.W, vector.Z / vector.W);
		}

		// Token: 0x06004F0C RID: 20236 RVA: 0x00162A40 File Offset: 0x00160C40
		private void RefineSpheresVoxelizationPerSlice(uint z)
		{
			for (int i = 0; i < this._globalLightDataCount; i++)
			{
				bool flag = (uint)this._lightClusterBBox[i].minZ <= z && z <= (uint)this._lightClusterBBox[i].maxZ;
				if (flag)
				{
					uint minZ = (uint)this._lightClusterBBox[i].minZ;
					uint maxZ = (uint)this._lightClusterBBox[i].maxZ;
					uint minY = (uint)this._lightClusterBBox[i].minY;
					uint maxY = (uint)this._lightClusterBBox[i].maxY;
					uint minX = (uint)this._lightClusterBBox[i].minX;
					uint maxX = (uint)this._lightClusterBBox[i].maxX;
					ref Vector4 ptr = ref this._pointLightData[2 * i];
					uint centerZ = this._lightRefinementInfo[i].CenterZ;
					uint centerY = this._lightRefinementInfo[i].CenterY;
					BoundingSphere boundingSphere = new BoundingSphere(new Vector3(ptr.X, ptr.Y, ptr.Z), ptr.W);
					bool flag2 = z != centerZ;
					if (flag2)
					{
						Plane plane = (z < centerZ) ? this._planesZ[(int)(z + 1U)] : new Plane(-this._planesZ[(int)z].Normal, -this._planesZ[(int)z].D);
						this.ProjectSphereToPlane(ref plane, ref boundingSphere, out boundingSphere);
					}
					for (uint num = minY; num <= maxY; num += 1U)
					{
						BoundingSphere boundingSphere2 = boundingSphere;
						bool flag3 = num != centerY;
						if (flag3)
						{
							Plane plane2 = (num < centerY) ? this._planesY[(int)(num + 1U)] : new Plane(-this._planesY[(int)num].Normal, -this._planesY[(int)num].D);
							this.ProjectSphereToPlane(ref plane2, ref boundingSphere2, out boundingSphere2);
						}
						int j = (int)minX;
						do
						{
							j++;
						}
						while ((long)j <= (long)((ulong)maxX) && this.ComputeSignedDistanceFromPlane(ref boundingSphere2.Center, ref this._planesX[j]) >= boundingSphere2.Radius);
						int num2 = (int)(maxX + 1U);
						do
						{
							num2--;
						}
						while (num2 >= j && -this.ComputeSignedDistanceFromPlane(ref boundingSphere2.Center, ref this._planesX[num2]) >= boundingSphere2.Radius);
						j--;
						num2++;
						while (j < num2)
						{
							this.RegisterLightInCluster((ushort)i, (uint)j, num, z);
							j++;
						}
					}
				}
			}
		}

		// Token: 0x06004F0D RID: 20237 RVA: 0x00162D18 File Offset: 0x00160F18
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private float ComputeSignedDistanceFromPlane(ref Vector3 point, ref Plane plane)
		{
			return Vector3.Dot(plane.Normal, point);
		}

		// Token: 0x06004F0E RID: 20238 RVA: 0x00162D3C File Offset: 0x00160F3C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private Plane CreatePlaneAtOrigin(Vector3 pointA, Vector3 pointB)
		{
			return new Plane(Vector3.Normalize(Vector3.Cross(pointA, pointB)), 0f);
		}

		// Token: 0x06004F0F RID: 20239 RVA: 0x00162D64 File Offset: 0x00160F64
		private void GetBoundsForAxis(bool xAxis, ref Vector3 center, float radius, float nearZ, out Vector3 U, out Vector3 L)
		{
			bool flag = center.Z + radius < nearZ;
			Vector3 vector = xAxis ? new Vector3(1f, 0f, 0f) : new Vector3(0f, 1f, 0f);
			Vector2 vector2 = new Vector2(Vector3.Dot(vector, center), center.Z);
			Vector2 vector3 = new Vector2(0f);
			Vector2 vector4 = new Vector2(0f);
			float num = Vector2.Dot(vector2, vector2) - MathHelper.Square(radius);
			float num2 = 0f;
			float num3 = 0f;
			bool flag2 = num > 0f;
			if (flag2)
			{
				float num4 = (float)Math.Sqrt((double)num);
				float num5 = vector2.Length();
				num2 = num4 / num5;
				num3 = radius / num5;
			}
			float num6 = 0f;
			bool flag3 = !flag;
			if (flag3)
			{
				num6 = (float)Math.Sqrt((double)(MathHelper.Square(radius) - MathHelper.Square(nearZ - vector2.Y)));
			}
			bool flag4 = num > 0f;
			if (flag4)
			{
				Vector2 value = new Vector2(num2, -num3);
				Vector2 value2 = new Vector2(num3, num2);
				vector3 = new Vector2(Vector2.Dot(value, vector2), Vector2.Dot(value2, vector2)) * num2;
			}
			bool flag5 = !flag && (num <= 0f || vector3.Y > nearZ);
			if (flag5)
			{
				vector3.X = vector2.X + num6;
				vector3.Y = nearZ;
			}
			num3 *= -1f;
			num6 *= -1f;
			bool flag6 = num > 0f;
			if (flag6)
			{
				Vector2 value3 = new Vector2(num2, -num3);
				Vector2 value4 = new Vector2(num3, num2);
				vector4 = new Vector2(Vector2.Dot(value3, vector2), Vector2.Dot(value4, vector2)) * num2;
			}
			bool flag7 = !flag && (num <= 0f || vector4.Y > nearZ);
			if (flag7)
			{
				vector4.X = vector2.X + num6;
				vector4.Y = nearZ;
			}
			num3 *= -1f;
			num6 *= -1f;
			U = vector * vector3.X;
			U.Z = vector3.Y;
			L = vector * vector4.X;
			L.Z = vector4.Y;
		}

		// Token: 0x06004F10 RID: 20240 RVA: 0x00162FD0 File Offset: 0x001611D0
		private void GetBoundingBox(ref Vector3 center, float radius, float nearZ, ref Vector3 projectionMatrixColumn0, ref Vector3 projectionMatrixColumn1, ref Vector3 projectionMatrixColumn3, out Vector2 min, out Vector2 max)
		{
			Vector3 vector;
			Vector3 vector2;
			this.GetBoundsForAxis(true, ref center, radius, nearZ, out vector, out vector2);
			Vector3 vector3;
			Vector3 vector4;
			this.GetBoundsForAxis(false, ref center, radius, nearZ, out vector3, out vector4);
			max.X = Vector3.Dot(vector, projectionMatrixColumn0) / Vector3.Dot(vector, projectionMatrixColumn3);
			min.X = Vector3.Dot(vector2, projectionMatrixColumn0) / Vector3.Dot(vector2, projectionMatrixColumn3);
			max.Y = Vector3.Dot(vector3, projectionMatrixColumn1) / Vector3.Dot(vector3, projectionMatrixColumn3);
			min.Y = Vector3.Dot(vector4, projectionMatrixColumn1) / Vector3.Dot(vector4, projectionMatrixColumn3);
			max.X = 0.5f + 0.5f * max.X;
			min.X = 0.5f + 0.5f * min.X;
			max.Y = 0.5f + 0.5f * max.Y;
			min.Y = 0.5f + 0.5f * min.Y;
		}

		// Token: 0x06004F11 RID: 20241 RVA: 0x001630EC File Offset: 0x001612EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void RegisterLightInCluster(ushort lightId, uint clusterX, uint clusterY, uint clusterZ)
		{
			bool flag = lightId >= 1024;
			if (flag)
			{
				throw new Exception(string.Format("Light Id too high - we support light rendering only up to {0}.", 1024));
			}
			bool flag2 = clusterX >= this._lightGridWidth || clusterY >= this._lightGridHeight || clusterZ >= this._lightGridDepth;
			if (flag2)
			{
				throw new Exception(string.Format("Cluster XYZ ({0}, {1}, {2}) out of bounds ({3}, {4}, {5}).", new object[]
				{
					clusterX,
					clusterY,
					clusterZ,
					this._lightGridWidth,
					this._lightGridHeight,
					this._lightGridDepth
				}));
			}
			this._zSliceLightData[(int)clusterZ].RegisterLight(clusterX, clusterY, lightId);
		}

		// Token: 0x06004F12 RID: 20242 RVA: 0x001631BC File Offset: 0x001613BC
		private void ClearLightGridData()
		{
			bool flag = !this._usePBO;
			if (flag)
			{
				Array.Clear(this._lightGridData, 0, this._lightGridData.Length);
			}
			int num = 0;
			while ((long)num < (long)((ulong)this._lightGridDepth))
			{
				this._zSliceLightData[num].ClearData();
				num++;
			}
		}

		// Token: 0x06004F13 RID: 20243 RVA: 0x00163218 File Offset: 0x00161418
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private uint GetLightGridAccess(uint x, uint y, uint z)
		{
			return z * (this._lightGridWidth * this._lightGridHeight) + y * this._lightGridWidth + x;
		}

		// Token: 0x06004F14 RID: 20244 RVA: 0x00163244 File Offset: 0x00161444
		private unsafe void FillClusteredLightingBuffersSlice(uint z, IntPtr lightGridPtr, IntPtr directPointLightPtr, IntPtr lightIndicesPtr)
		{
			bool flag = this._zSliceLightData[(int)z].ActiveLightRefCount > 0U || this._usePBO;
			if (flag)
			{
				uint num = 0U;
				uint num2 = this._zSliceLightData[(int)z].LightRefCountInPreviousSlices * 2U;
				uint num3 = this._zSliceLightData[(int)z].LightRefCountInPreviousSlices;
				for (uint num4 = 0U; num4 < this._lightGridHeight; num4 += 1U)
				{
					for (uint num5 = 0U; num5 < this._lightGridWidth; num5 += 1U)
					{
						uint num6 = (uint)this._zSliceLightData[(int)z].LightCounts[(int)num];
						bool flag2 = num6 > 0U;
						if (flag2)
						{
							uint lightGridAccess = this.GetLightGridAccess(num5, num4, z);
							uint num7 = this._useLightDirectAccess ? num2 : num3;
							uint num8 = num6;
							uint num9 = num7 >> 3;
							uint num10 = num8 << 3 | (num7 & 7U);
							bool usePBO = this._usePBO;
							if (usePBO)
							{
								ushort* ptr = (ushort*)lightGridPtr.ToPointer();
								ptr[(ulong)(2U * lightGridAccess) * 2UL / 2UL] = (ushort)num9;
								ptr[(ulong)(2U * lightGridAccess + 1U) * 2UL / 2UL] = (ushort)num10;
							}
							else
							{
								this._lightGridData[(int)(2U * lightGridAccess)] = (ushort)num9;
								this._lightGridData[(int)(2U * lightGridAccess + 1U)] = (ushort)num10;
							}
							int num11 = 0;
							int num12 = 0;
							while ((long)num12 < 32L)
							{
								uint num13 = this._zSliceLightData[(int)z].Bitfields[(int)(checked((IntPtr)(unchecked((ulong)(num * 32U) + (ulong)((long)num12)))))];
								bool flag3 = num13 > 0U;
								if (flag3)
								{
									for (int i = 0; i < 32; i++)
									{
										bool flag4 = BitUtils.IsBitOn(i, num13);
										if (flag4)
										{
											ushort num14 = (ushort)(num12 * 32 + i);
											bool flag5 = !this._useMappedGPUBuffers;
											if (flag5)
											{
												bool flag6 = !this._useLightDirectAccess;
												if (flag6)
												{
													ushort num15 = num14 * 2;
													this._lightIndices[(int)num3] = num15;
													num3 += 1U;
												}
												else
												{
													this._directPointLightData[(int)num2] = this._pointLightData[(int)(2 * num14)];
													this._directPointLightData[(int)(num2 + 1U)] = this._pointLightData[(int)(2 * num14 + 1)];
													num2 += 2U;
												}
											}
											else
											{
												bool flag7 = !this._useLightDirectAccess;
												if (flag7)
												{
													ushort num16 = num14 * 2;
													ushort* ptr2 = (ushort*)lightIndicesPtr.ToPointer();
													ptr2[(ulong)num3 * 2UL / 2UL] = num16;
													num3 += 1U;
												}
												else
												{
													Vector4* ptr3 = (Vector4*)directPointLightPtr.ToPointer();
													ptr3[(ulong)num2 * (ulong)((long)sizeof(Vector4)) / (ulong)sizeof(Vector4)] = this._pointLightData[(int)(2 * num14)];
													ptr3[(ulong)(num2 + 1U) * (ulong)((long)sizeof(Vector4)) / (ulong)sizeof(Vector4)] = this._pointLightData[(int)(2 * num14 + 1)];
													num2 += 2U;
												}
											}
											num11++;
											bool flag8 = (long)num11 == (long)((ulong)num6);
											if (flag8)
											{
												break;
											}
										}
									}
								}
								bool flag9 = (long)num11 == (long)((ulong)num6);
								if (flag9)
								{
									break;
								}
								num12++;
							}
						}
						else
						{
							bool usePBO2 = this._usePBO;
							if (usePBO2)
							{
								uint lightGridAccess2 = this.GetLightGridAccess(num5, num4, z);
								ushort* ptr4 = (ushort*)lightGridPtr.ToPointer();
								ptr4[(ulong)(2U * lightGridAccess2) * 2UL / 2UL] = 0;
								ptr4[(ulong)(2U * lightGridAccess2 + 1U) * 2UL / 2UL] = 0;
							}
						}
						num += 1U;
					}
				}
			}
		}

		// Token: 0x06004F15 RID: 20245 RVA: 0x001635AC File Offset: 0x001617AC
		private void FillClusteredLightingBuffers()
		{
			IntPtr lightGridPtr = IntPtr.Zero;
			IntPtr directPointLightPtr = IntPtr.Zero;
			IntPtr lightIndicesPtr = IntPtr.Zero;
			bool usePBO = this._usePBO;
			if (usePBO)
			{
				lightGridPtr = this._lightGridPBO.BeginTransfer((uint)(this._lightGridData.Length * 2), 0U);
			}
			bool useMappedGPUBuffers = this._useMappedGPUBuffers;
			if (useMappedGPUBuffers)
			{
				bool flag = !this._useLightDirectAccess;
				if (flag)
				{
					lightIndicesPtr = this._lightIndicesBufferTexture.BeginTransfer((uint)(this._lightIndices.Length * 2));
				}
				else
				{
					directPointLightPtr = this._directPointLightBufferTexture.BeginTransfer((uint)(this._directPointLightData.Length * 4 * 4));
				}
			}
			for (uint num = 1U; num < this._lightGridDepth; num += 1U)
			{
				this._zSliceLightData[(int)num].LightRefCountInPreviousSlices = this._zSliceLightData[(int)(num - 1U)].LightRefCountInPreviousSlices + this._zSliceLightData[(int)(num - 1U)].ActiveLightRefCount;
			}
			this._lightIndicesCount = this._zSliceLightData[(int)(this._lightGridDepth - 1U)].LightRefCountInPreviousSlices + this._zSliceLightData[(int)(this._lightGridDepth - 1U)].ActiveLightRefCount;
			this._directPointLightCount = this._zSliceLightData[(int)(this._lightGridDepth - 1U)].LightRefCountInPreviousSlices + this._zSliceLightData[(int)(this._lightGridDepth - 1U)].ActiveLightRefCount;
			bool useParallelExecution = this._useParallelExecution;
			if (useParallelExecution)
			{
				Parallel.For(0L, (long)((ulong)this._lightGridDepth), delegate(long z)
				{
					this.FillClusteredLightingBuffersSlice((uint)z, lightGridPtr, directPointLightPtr, lightIndicesPtr);
				});
			}
			else
			{
				int num2 = 0;
				while ((long)num2 < (long)((ulong)this._lightGridDepth))
				{
					this.FillClusteredLightingBuffersSlice((uint)num2, lightGridPtr, directPointLightPtr, lightIndicesPtr);
					num2++;
				}
			}
			bool useMappedGPUBuffers2 = this._useMappedGPUBuffers;
			if (useMappedGPUBuffers2)
			{
				bool flag2 = !this._useLightDirectAccess;
				if (flag2)
				{
					this._lightIndicesBufferTexture.EndTransfer();
				}
				else
				{
					this._directPointLightBufferTexture.EndTransfer();
				}
			}
			bool usePBO2 = this._usePBO;
			if (usePBO2)
			{
				this._lightGridPBO.EndTransfer();
			}
		}

		// Token: 0x06004F16 RID: 20246 RVA: 0x001637EC File Offset: 0x001619EC
		public static void UnitTest()
		{
			float nearZ = 0.1f;
			float farZ = 500f;
			float distributionCoef = 1f;
			float[] array = new float[17];
			int num = array.Length;
			ClusteredLighting.TestData[] array2 = new ClusteredLighting.TestData[num];
			ClusteredLighting.ComputeGridDepthSlices(false, 16, nearZ, farZ, ref distributionCoef, ref array);
			float[] array3 = new float[]
			{
				0.1f,
				0.17f,
				0.29f,
				0.49f,
				0.84f,
				1.43f,
				2.44f,
				4.15f,
				7.07f,
				12.04f,
				20.5f,
				34.91f,
				59.46f,
				101.25f,
				172.42f,
				293.62f,
				500f
			};
			for (int i = 0; i < num; i++)
			{
				array2[i].ExpectedResult = array3[i];
				array2[i].Result = array[i];
			}
			for (int j = 0; j < num; j++)
			{
				Debug.Assert(array2[j].CheckResult(), string.Format("Error in test data {0}.", j));
			}
			num = 10;
			ClusteredLighting.ComputeGridDepthSlices(false, 16, nearZ, farZ, ref distributionCoef, ref array);
			float[] array4 = new float[]
			{
				0.1f,
				0.3f,
				1f,
				2f,
				5f,
				10f,
				30f,
				50f,
				100f,
				200f
			};
			int[] array5 = new int[]
			{
				0,
				2,
				4,
				5,
				7,
				8,
				10,
				11,
				12,
				14
			};
			for (int k = 0; k < num; k++)
			{
				array2[k].Input = array4[k];
				array2[k].ExpectedResultInt = array5[k];
				array2[k].ResultInt = (int)ClusteredLighting.GetLightGridDepthSlice(false, 16, nearZ, distributionCoef, array4[k]);
			}
			for (int l = 0; l < num; l++)
			{
				Debug.Assert(array2[l].CheckResultInt(), string.Format("Error in test data {0}.", l));
			}
			num = array.Length;
			bool customNearZ = true;
			nearZ = 5f;
			ClusteredLighting.ComputeGridDepthSlices(customNearZ, 16, nearZ, farZ, ref distributionCoef, ref array);
			float[] array6 = new float[]
			{
				0.1f,
				5f,
				6.79f,
				9.24f,
				12.56f,
				17.07f,
				23.21f,
				31.55f,
				42.88f,
				58.29f,
				79.24f,
				107.72f,
				146.43f,
				199.05f,
				270.58f,
				367.82f,
				500f
			};
			for (int m = 0; m < num; m++)
			{
				array2[m].ExpectedResult = array6[m];
				array2[m].Result = array[m];
			}
			for (int n = 0; n < num; n++)
			{
				Debug.Assert(array2[n].CheckResult(), string.Format("Error in test data {0}.", n));
			}
			num = 10;
			bool customNearZ2 = true;
			nearZ = 5f;
			ClusteredLighting.ComputeGridDepthSlices(customNearZ2, 16, nearZ, farZ, ref distributionCoef, ref array);
			float[] array7 = new float[]
			{
				0.1f,
				0.3f,
				1f,
				2f,
				5.1f,
				10f,
				30f,
				50f,
				100f,
				200f
			};
			int[] array8 = new int[]
			{
				0,
				0,
				0,
				0,
				1,
				3,
				6,
				8,
				10,
				13
			};
			for (int num2 = 0; num2 < num; num2++)
			{
				array2[num2].Input = array7[num2];
				array2[num2].ExpectedResultInt = array8[num2];
				array2[num2].ResultInt = (int)ClusteredLighting.GetLightGridDepthSlice(customNearZ2, 16, nearZ, distributionCoef, array7[num2]);
			}
			for (int num3 = 0; num3 < num; num3++)
			{
				Debug.Assert(array2[num3].CheckResultInt(), string.Format("Error in test data {0}.", num3));
			}
		}

		// Token: 0x04002A29 RID: 10793
		private bool _useRefinedVoxelization;

		// Token: 0x04002A2A RID: 10794
		private bool _useMappedGPUBuffers;

		// Token: 0x04002A2B RID: 10795
		private bool _useLightDirectAccess = true;

		// Token: 0x04002A2C RID: 10796
		private bool _usePBO;

		// Token: 0x04002A2D RID: 10797
		private bool _useDoubleBuffering = true;

		// Token: 0x04002A2E RID: 10798
		private bool _useParallelExecution = true;

		// Token: 0x04002A2F RID: 10799
		private uint _lightGridWidth;

		// Token: 0x04002A30 RID: 10800
		private uint _lightGridHeight;

		// Token: 0x04002A31 RID: 10801
		private uint _lightGridDepth;

		// Token: 0x04002A32 RID: 10802
		private uint _lightGridDataCount;

		// Token: 0x04002A33 RID: 10803
		private ushort[] _lightGridData;

		// Token: 0x04002A34 RID: 10804
		private GPUBuffer _lightGridPBO;

		// Token: 0x04002A35 RID: 10805
		private Texture _lightGridTexture;

		// Token: 0x04002A36 RID: 10806
		private ushort[] _lightIndices;

		// Token: 0x04002A37 RID: 10807
		private GPUBufferTexture _lightIndicesBufferTexture;

		// Token: 0x04002A38 RID: 10808
		private uint _lightIndicesCount;

		// Token: 0x04002A39 RID: 10809
		private uint _directPointLightCount;

		// Token: 0x04002A3A RID: 10810
		private GPUBufferTexture _directPointLightBufferTexture;

		// Token: 0x04002A3B RID: 10811
		private Vector4[] _directPointLightData;

		// Token: 0x04002A3C RID: 10812
		private Vector4[] _pointLightData = new Vector4[2048];

		// Token: 0x04002A3D RID: 10813
		private GPUBuffer _pointLightsBuffer;

		// Token: 0x04002A3E RID: 10814
		private int _globalLightDataCount;

		// Token: 0x04002A3F RID: 10815
		private ClusteredLighting.ClusterBBox[] _lightClusterBBox = new ClusteredLighting.ClusterBBox[1024];

		// Token: 0x04002A40 RID: 10816
		private ClusteredLighting.LightRefinementInfo[] _lightRefinementInfo = new ClusteredLighting.LightRefinementInfo[1024];

		// Token: 0x04002A41 RID: 10817
		private ClusteredLighting.ZSliceLightData[] _zSliceLightData;

		// Token: 0x04002A42 RID: 10818
		private Plane[] _planesZ;

		// Token: 0x04002A43 RID: 10819
		private Plane[] _planesY;

		// Token: 0x04002A44 RID: 10820
		private Plane[] _planesX;

		// Token: 0x04002A45 RID: 10821
		private bool _useCustomZDistribution = true;

		// Token: 0x04002A46 RID: 10822
		private const float GridNearZCustom = 5f;

		// Token: 0x04002A47 RID: 10823
		private const float GridNearZDefault = 0.1f;

		// Token: 0x04002A48 RID: 10824
		private float _gridNearZ = 5f;

		// Token: 0x04002A49 RID: 10825
		private float _gridFarZ = 500f;

		// Token: 0x04002A4A RID: 10826
		private float _coef;

		// Token: 0x04002A4B RID: 10827
		private int _zSlicesCount;

		// Token: 0x04002A4C RID: 10828
		private float[] _depthSlices;

		// Token: 0x04002A4D RID: 10829
		private Matrix _projectionMatrix;

		// Token: 0x04002A4E RID: 10830
		private Matrix _invProjectionMatrix;

		// Token: 0x04002A4F RID: 10831
		private int _renderingProfileLightClusterClear;

		// Token: 0x04002A50 RID: 10832
		private int _renderingProfileLightClustering;

		// Token: 0x04002A51 RID: 10833
		private int _renderingProfileLightClusteringRefine;

		// Token: 0x04002A52 RID: 10834
		private int _renderingProfileLightFillGridData;

		// Token: 0x04002A53 RID: 10835
		private int _renderingProfileLightSendDataToGPU;

		// Token: 0x04002A54 RID: 10836
		private readonly Profiling _profiling;

		// Token: 0x04002A55 RID: 10837
		private readonly RenderTargetStore _renderTargetStore;

		// Token: 0x04002A56 RID: 10838
		private readonly GraphicsDevice _graphics;

		// Token: 0x04002A57 RID: 10839
		private readonly GLFunctions _gl;

		// Token: 0x02000E97 RID: 3735
		private struct ZSliceLightData
		{
			// Token: 0x17001474 RID: 5236
			// (get) Token: 0x060067D9 RID: 26585 RVA: 0x00218EAD File Offset: 0x002170AD
			public uint ActiveLightRefCount
			{
				get
				{
					return this._lightRefCount;
				}
			}

			// Token: 0x060067DA RID: 26586 RVA: 0x00218EB5 File Offset: 0x002170B5
			public ZSliceLightData(uint width, uint height)
			{
				this._width = width;
				this._height = height;
				this._lightRefCount = 0U;
				this.LightRefCountInPreviousSlices = 0U;
				this.Bitfields = new uint[width * height * 32U];
				this.LightCounts = new ushort[width * height];
			}

			// Token: 0x060067DB RID: 26587 RVA: 0x00218EF4 File Offset: 0x002170F4
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private uint GetClusterIndex(uint x, uint y)
			{
				return y * this._width + x;
			}

			// Token: 0x060067DC RID: 26588 RVA: 0x00218F10 File Offset: 0x00217110
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void RegisterLight(uint x, uint y, ushort lightId)
			{
				uint clusterIndex = this.GetClusterIndex(x, y);
				int num = (int)(lightId / 32);
				int bitId = (int)(lightId % 32);
				BitUtils.SwitchOnBit(bitId, ref this.Bitfields[(int)(checked((IntPtr)(unchecked((ulong)(clusterIndex * 32U) + (ulong)((long)num)))))]);
				ushort[] lightCounts = this.LightCounts;
				uint num2 = clusterIndex;
				lightCounts[(int)num2] = lightCounts[(int)num2] + 1;
				this._lightRefCount += 1U;
			}

			// Token: 0x060067DD RID: 26589 RVA: 0x00218F6C File Offset: 0x0021716C
			public void ClearData()
			{
				bool flag = this._lightRefCount > 0U;
				if (flag)
				{
					this._lightRefCount = 0U;
					Array.Clear(this.Bitfields, 0, this.Bitfields.Length);
					Array.Clear(this.LightCounts, 0, this.LightCounts.Length);
				}
				this.LightRefCountInPreviousSlices = 0U;
			}

			// Token: 0x0400473C RID: 18236
			public const uint MaxLights = 1024U;

			// Token: 0x0400473D RID: 18237
			public const uint UintPerCluster = 32U;

			// Token: 0x0400473E RID: 18238
			public uint[] Bitfields;

			// Token: 0x0400473F RID: 18239
			public ushort[] LightCounts;

			// Token: 0x04004740 RID: 18240
			public uint LightRefCountInPreviousSlices;

			// Token: 0x04004741 RID: 18241
			private uint _lightRefCount;

			// Token: 0x04004742 RID: 18242
			private uint _width;

			// Token: 0x04004743 RID: 18243
			private uint _height;
		}

		// Token: 0x02000E98 RID: 3736
		private struct ClusterBBox
		{
			// Token: 0x04004744 RID: 18244
			public byte minZ;

			// Token: 0x04004745 RID: 18245
			public byte maxZ;

			// Token: 0x04004746 RID: 18246
			public byte minY;

			// Token: 0x04004747 RID: 18247
			public byte maxY;

			// Token: 0x04004748 RID: 18248
			public byte minX;

			// Token: 0x04004749 RID: 18249
			public byte maxX;
		}

		// Token: 0x02000E99 RID: 3737
		private struct LightRefinementInfo
		{
			// Token: 0x0400474A RID: 18250
			public uint CenterY;

			// Token: 0x0400474B RID: 18251
			public uint CenterZ;
		}

		// Token: 0x02000E9A RID: 3738
		private struct TestData
		{
			// Token: 0x060067DE RID: 26590 RVA: 0x00218FC4 File Offset: 0x002171C4
			public bool CheckResult()
			{
				return MathHelper.Distance(this.Result, this.ExpectedResult) < 0.01f;
			}

			// Token: 0x060067DF RID: 26591 RVA: 0x00218FF0 File Offset: 0x002171F0
			public bool CheckResultInt()
			{
				return this.ResultInt == this.ExpectedResultInt;
			}

			// Token: 0x0400474C RID: 18252
			public float Input;

			// Token: 0x0400474D RID: 18253
			public float Result;

			// Token: 0x0400474E RID: 18254
			public float ExpectedResult;

			// Token: 0x0400474F RID: 18255
			public int ResultInt;

			// Token: 0x04004750 RID: 18256
			public int ExpectedResultInt;
		}
	}
}
