using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using HytaleClient.Core;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A3E RID: 2622
	internal class OcclusionCulling : Disposable
	{
		// Token: 0x170012C0 RID: 4800
		// (get) Token: 0x06005248 RID: 21064 RVA: 0x00169DF4 File Offset: 0x00167FF4
		// (set) Token: 0x06005249 RID: 21065 RVA: 0x00169DFC File Offset: 0x00167FFC
		public bool IsActive { get; private set; }

		// Token: 0x170012C1 RID: 4801
		// (get) Token: 0x0600524A RID: 21066 RVA: 0x00169E05 File Offset: 0x00168005
		public int MaxInvalidScreenAreasForReprojection
		{
			get
			{
				return this._graphics.GPUProgramStore.HiZReprojectProgram.MaxInvalidScreenAreas;
			}
		}

		// Token: 0x0600524B RID: 21067 RVA: 0x00169E1C File Offset: 0x0016801C
		public OcclusionCulling(GraphicsDevice graphics, Profiling profiling)
		{
			this._graphics = graphics;
			this._profiling = profiling;
			this._visibleOccludees = new int[this.MaxOccludees];
			this._previousFrameInvalidScreenAreas = new Vector4[32];
			this.CreateGPUData();
		}

		// Token: 0x0600524C RID: 21068 RVA: 0x00169E76 File Offset: 0x00168076
		protected override void DoDispose()
		{
			this.DestroyGPUData();
			this._visibleOccludees = null;
		}

		// Token: 0x0600524D RID: 21069 RVA: 0x00169E88 File Offset: 0x00168088
		private void CreateGPUData()
		{
			GLFunctions gl = this._graphics.GL;
			int value = this.MaxOccludees * 4;
			this._visibleOccludeesTFBO = gl.GenBuffer();
			gl.BindBuffer(GL.ARRAY_BUFFER, this._visibleOccludeesTFBO);
			gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)value, IntPtr.Zero, GL.STATIC_READ);
			int value2 = this.MaxOccludees * OcclusionCulling.OccludeeData.Size;
			this._occludeesPositionsVBO = gl.GenBuffer();
			gl.BindBuffer(GL.ARRAY_BUFFER, this._occludeesPositionsVBO);
			gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)value2, IntPtr.Zero, GL.DYNAMIC_DRAW);
			this._occludeesVAO = gl.GenVertexArray();
			gl.BindVertexArray(this._occludeesVAO);
			HiZCullProgram hiZCullProgram = this._graphics.GPUProgramStore.HiZCullProgram;
			IntPtr pointer = IntPtr.Zero;
			gl.EnableVertexAttribArray(hiZCullProgram.AttribBoxMin.Index);
			gl.VertexAttribPointer(hiZCullProgram.AttribBoxMin.Index, 3, GL.FLOAT, false, OcclusionCulling.OccludeeData.Size, pointer);
			pointer += 12;
			gl.EnableVertexAttribArray(hiZCullProgram.AttribBoxMax.Index);
			gl.VertexAttribPointer(hiZCullProgram.AttribBoxMax.Index, 3, GL.FLOAT, false, OcclusionCulling.OccludeeData.Size, pointer);
			pointer += 12;
			this._reprojectedPointsVertexArray = gl.GenVertexArray();
			this._occlusionRenderTarget = new RenderTarget(this._graphics.OcclusionMapWidth, this._graphics.OcclusionMapHeight, "_occlusionRenderTarget");
			this._occlusionRenderTarget.AddTexture(RenderTarget.Target.Depth, GL.DEPTH_COMPONENT32, GL.DEPTH_COMPONENT, GL.UNSIGNED_INT, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, false, true, 1);
			this._occlusionRenderTarget.FinalizeSetup();
			this._occlusionRenderTargetB = new RenderTarget(this._graphics.OcclusionMapWidth, this._graphics.OcclusionMapHeight, "_occlusionRenderTargetB");
			this._occlusionRenderTargetB.AddTexture(RenderTarget.Target.Depth, GL.DEPTH_COMPONENT32, GL.DEPTH_COMPONENT, GL.UNSIGNED_INT, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, false, true, 1);
			this._occlusionRenderTargetB.FinalizeSetup();
			MeshProcessor.CreateSimpleBox(ref this._debugOccludeeMesh, 1f);
		}

		// Token: 0x0600524E RID: 21070 RVA: 0x0016A0C8 File Offset: 0x001682C8
		private void DestroyGPUData()
		{
			GLFunctions gl = this._graphics.GL;
			this._debugOccludeeMesh.Dispose();
			this._occlusionRenderTargetB.Dispose();
			this._occlusionRenderTarget.Dispose();
			gl.DeleteVertexArray(this._reprojectedPointsVertexArray);
			gl.DeleteVertexArray(this._occludeesVAO);
			gl.DeleteBuffer(this._occludeesPositionsVBO);
			gl.DeleteBuffer(this._visibleOccludeesTFBO);
		}

		// Token: 0x0600524F RID: 21071 RVA: 0x0016A13A File Offset: 0x0016833A
		public void SetupRenderingProfiles(int renderingProfileOcclusionBuildMap, int renderingProfileOcclusionRenderOccluders, int renderingProfileOcclusionReproject, int renderingProfileOcclusionCreateHiZ, int renderingProfileOcclusionPrepareOccludees, int renderingProfileOcclusionTestOccludees, int renderingProfileOcclusionFetchResults)
		{
			this._renderingProfileOcclusionBuildMap = renderingProfileOcclusionBuildMap;
			this._renderingProfileOcclusionRenderOccluders = renderingProfileOcclusionRenderOccluders;
			this._renderingProfileOcclusionReproject = renderingProfileOcclusionReproject;
			this._renderingProfileOcclusionCreateHiZ = renderingProfileOcclusionCreateHiZ;
			this._renderingProfileOcclusionPrepareOccludees = renderingProfileOcclusionPrepareOccludees;
			this._renderingProfileOcclusionTestOccludees = renderingProfileOcclusionTestOccludees;
			this._renderingProfileOcclusionFetchResults = renderingProfileOcclusionFetchResults;
		}

		// Token: 0x06005250 RID: 21072 RVA: 0x0016A174 File Offset: 0x00168374
		private void GrowOccludeesBuffersIfNecessary(int occludeesCount, int growth)
		{
			bool flag = occludeesCount >= this.MaxOccludees;
			if (flag)
			{
				GLFunctions gl = this._graphics.GL;
				this.MaxOccludees += growth;
				int value = this.MaxOccludees * 4;
				gl.BindBuffer(GL.ARRAY_BUFFER, this._visibleOccludeesTFBO);
				gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)value, IntPtr.Zero, GL.STATIC_READ);
				int value2 = this.MaxOccludees * OcclusionCulling.OccludeeData.Size;
				gl.BindBuffer(GL.ARRAY_BUFFER, this._occludeesPositionsVBO);
				gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)value2, IntPtr.Zero, GL.DYNAMIC_DRAW);
				Array.Resize<int>(ref this._visibleOccludees, this.MaxOccludees);
			}
		}

		// Token: 0x06005251 RID: 21073 RVA: 0x0016A240 File Offset: 0x00168440
		public void Update(ref Matrix viewRotationProjectionMatrix, float frameTime, bool isSpatialContinuityLost, Action drawOccluders, ref Matrix reprojectionMatrix, ref Matrix previousProjectionMatrix, RenderTarget previousZBuffer, RenderTarget.Target previousZBufferTarget, Vector4[] previousFrameInvalidScreenAreas, int previousFrameInvalidScreenAreaCount, bool fillReprojectionHoles, ref OcclusionCulling.OccludeeData[] candidateOccludees, int candidateOccludeesCount, ref int[] visibleOccludees)
		{
			Profiling profiling = this._profiling;
			if (isSpatialContinuityLost)
			{
				this._skipRemainingTime = 2000f;
			}
			else
			{
				bool flag = this._skipRemainingTime > 0f;
				if (flag)
				{
					this._skipRemainingTime -= frameTime;
				}
			}
			this.IsActive = (this.IsEnabled && this._skipRemainingTime <= 0f);
			bool isActive = this.IsActive;
			if (isActive)
			{
				this.BuildOcclusionMap(drawOccluders, ref reprojectionMatrix, ref previousProjectionMatrix, previousZBuffer, previousZBufferTarget, previousFrameInvalidScreenAreas, previousFrameInvalidScreenAreaCount, fillReprojectionHoles);
				this.PrepareOccludees(ref candidateOccludees, candidateOccludeesCount);
				this.TestOccludees(ref viewRotationProjectionMatrix);
			}
			else
			{
				for (int i = 0; i < this._visibleOccludees.Length; i++)
				{
					this._visibleOccludees[i] = 1;
				}
				visibleOccludees = this._visibleOccludees;
				profiling.SkipMeasure(this._renderingProfileOcclusionBuildMap);
				profiling.SkipMeasure(this._renderingProfileOcclusionRenderOccluders);
				profiling.SkipMeasure(this._renderingProfileOcclusionReproject);
				profiling.SkipMeasure(this._renderingProfileOcclusionCreateHiZ);
				profiling.SkipMeasure(this._renderingProfileOcclusionPrepareOccludees);
				profiling.SkipMeasure(this._renderingProfileOcclusionTestOccludees);
			}
		}

		// Token: 0x06005252 RID: 21074 RVA: 0x0016A36C File Offset: 0x0016856C
		private void BuildOcclusionMap(Action drawOccluders, ref Matrix reprojectionMatrix, ref Matrix previousProjectionMatrix, RenderTarget previousZBuffer, RenderTarget.Target previousZBufferTarget, Vector4[] previousFrameInvalidScreenAreas, int previousFrameInvalidScreenAreaCount, bool fillReprojectionHoles)
		{
			Profiling profiling = this._profiling;
			GraphicsDevice graphics = this._graphics;
			GLFunctions gl = graphics.GL;
			profiling.StartMeasure(this._renderingProfileOcclusionBuildMap);
			gl.ColorMask(false, false, false, false);
			gl.Enable(GL.DEPTH_TEST);
			gl.DepthMask(true);
			gl.Enable(GL.CULL_FACE);
			bool flag = previousZBuffer != null;
			bool flag2 = flag && fillReprojectionHoles;
			bool flag3 = flag2;
			if (flag3)
			{
				this._occlusionRenderTargetB.Bind(true, true);
			}
			else
			{
				this._occlusionRenderTarget.Bind(true, true);
			}
			profiling.StartMeasure(this._renderingProfileOcclusionRenderOccluders);
			drawOccluders();
			profiling.StopMeasure(this._renderingProfileOcclusionRenderOccluders);
			bool flag4 = flag;
			if (flag4)
			{
				profiling.StartMeasure(this._renderingProfileOcclusionReproject);
				this.ReprojectPreviousZBuffer(ref reprojectionMatrix, ref previousProjectionMatrix, previousZBuffer, previousZBufferTarget, previousFrameInvalidScreenAreas, previousFrameInvalidScreenAreaCount);
				bool flag5 = flag2;
				if (flag5)
				{
					this._occlusionRenderTargetB.Unbind();
					this._occlusionRenderTarget.Bind(true, false);
					this.FillHoles();
				}
				profiling.StopMeasure(this._renderingProfileOcclusionReproject);
			}
			else
			{
				profiling.SkipMeasure(this._renderingProfileOcclusionReproject);
			}
			profiling.StartMeasure(this._renderingProfileOcclusionCreateHiZ);
			this.CreateHiZOcclusionCullingMap();
			profiling.StopMeasure(this._renderingProfileOcclusionCreateHiZ);
			this._occlusionRenderTarget.Unbind();
			RenderTarget.BindHardwareFramebuffer();
			gl.ColorMask(true, true, true, true);
			profiling.StopMeasure(this._renderingProfileOcclusionBuildMap);
		}

		// Token: 0x06005253 RID: 21075 RVA: 0x0016A4E0 File Offset: 0x001686E0
		private void ReprojectPreviousZBuffer(ref Matrix reprojectionMatrix, ref Matrix previousProjectionMatrix, RenderTarget previousZBuffer, RenderTarget.Target previousZBufferTarget, Vector4[] previousFrameInvalidScreenAreas, int previousFrameInvalidScreenAreaCount)
		{
			GraphicsDevice graphics = this._graphics;
			GLFunctions gl = graphics.GL;
			gl.AssertActiveTexture(GL.TEXTURE0);
			Vector2 vector = new Vector2(512f, 256f);
			gl.PointSize(1f);
			HiZReprojectProgram hiZReprojectProgram = this._graphics.GPUProgramStore.HiZReprojectProgram;
			gl.BindTexture(GL.TEXTURE_2D, previousZBuffer.GetTexture(previousZBufferTarget));
			gl.UseProgram(hiZReprojectProgram);
			int num = Math.Min(hiZReprojectProgram.MaxInvalidScreenAreas, previousFrameInvalidScreenAreaCount);
			bool flag = previousFrameInvalidScreenAreas != null || previousFrameInvalidScreenAreaCount > num;
			if (flag)
			{
				Array.Copy(previousFrameInvalidScreenAreas, this._previousFrameInvalidScreenAreas, num);
			}
			Array.Clear(this._previousFrameInvalidScreenAreas, num, hiZReprojectProgram.MaxInvalidScreenAreas - num);
			hiZReprojectProgram.InvalidScreenAreas.SetValue(this._previousFrameInvalidScreenAreas, hiZReprojectProgram.MaxInvalidScreenAreas);
			hiZReprojectProgram.Resolutions.SetValue(vector.X, vector.Y, (float)previousZBuffer.Width, (float)previousZBuffer.Height);
			hiZReprojectProgram.ReprojectMatrix.SetValue(ref reprojectionMatrix);
			hiZReprojectProgram.ProjectionMatrix.SetValue(ref previousProjectionMatrix);
			gl.BindVertexArray(this._reprojectedPointsVertexArray);
			gl.DrawArrays(GL.NO_ERROR, 0, (int)(vector.X * vector.Y));
		}

		// Token: 0x06005254 RID: 21076 RVA: 0x0016A620 File Offset: 0x00168820
		private void FillHoles()
		{
			GraphicsDevice graphics = this._graphics;
			GLFunctions gl = graphics.GL;
			gl.AssertActiveTexture(GL.TEXTURE0);
			HiZFillHoleProgram hiZFillHoleProgram = this._graphics.GPUProgramStore.HiZFillHoleProgram;
			gl.BindTexture(GL.TEXTURE_2D, this._occlusionRenderTargetB.GetTexture(RenderTarget.Target.Depth));
			gl.UseProgram(hiZFillHoleProgram);
			graphics.ScreenTriangleRenderer.Draw();
		}

		// Token: 0x06005255 RID: 21077 RVA: 0x0016A688 File Offset: 0x00168888
		private void CreateHiZOcclusionCullingMap()
		{
			GraphicsDevice graphics = this._graphics;
			GLFunctions gl = graphics.GL;
			HiZBuildProgram hiZBuildProgram = graphics.GPUProgramStore.HiZBuildProgram;
			gl.UseProgram(hiZBuildProgram);
			gl.ActiveTexture(GL.TEXTURE0);
			gl.BindTexture(GL.TEXTURE_2D, this._occlusionRenderTarget.GetTexture(RenderTarget.Target.Depth));
			gl.DepthFunc(GL.ALWAYS);
			int textureMipLevelCount = this._occlusionRenderTarget.GetTextureMipLevelCount(RenderTarget.Target.Depth);
			int num = this._occlusionRenderTarget.Width;
			int num2 = this._occlusionRenderTarget.Height;
			graphics.ScreenTriangleRenderer.BindVertexArray();
			for (int i = 1; i < textureMipLevelCount; i++)
			{
				num /= 2;
				num2 /= 2;
				num = ((num > 0) ? num : 1);
				num2 = ((num2 > 0) ? num2 : 1);
				gl.Viewport(0, 0, num, num2);
				gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_BASE_LEVEL, i - 1);
				gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAX_LEVEL, i - 1);
				gl.FramebufferTexture2D(GL.FRAMEBUFFER, GL.DEPTH_ATTACHMENT, GL.TEXTURE_2D, this._occlusionRenderTarget.GetTexture(RenderTarget.Target.Depth), i);
				graphics.ScreenTriangleRenderer.DrawRaw();
			}
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_BASE_LEVEL, 0);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAX_LEVEL, textureMipLevelCount - 1);
			gl.FramebufferTexture2D(GL.FRAMEBUFFER, GL.DEPTH_ATTACHMENT, GL.TEXTURE_2D, this._occlusionRenderTarget.GetTexture(RenderTarget.Target.Depth), 0);
			gl.DepthFunc(GL.LEQUAL);
		}

		// Token: 0x06005256 RID: 21078 RVA: 0x0016A830 File Offset: 0x00168A30
		private unsafe void PrepareOccludees(ref OcclusionCulling.OccludeeData[] candidateOccludees, int candidateOccludeesCount)
		{
			Profiling profiling = this._profiling;
			GraphicsDevice graphics = this._graphics;
			GLFunctions gl = graphics.GL;
			profiling.StartMeasure(this._renderingProfileOcclusionPrepareOccludees);
			this._occludeesData = candidateOccludees;
			this._occludeesCount = candidateOccludeesCount;
			int growth = Math.Max(500, 2 * (this._occludeesCount - this.MaxOccludees));
			this.GrowOccludeesBuffersIfNecessary(this._occludeesCount, growth);
			gl.BindBuffer(GL.ARRAY_BUFFER, this._occludeesPositionsVBO);
			OcclusionCulling.OccludeeData[] array;
			OcclusionCulling.OccludeeData* value;
			if ((array = candidateOccludees) == null || array.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array[0];
			}
			gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(this._occludeesCount * OcclusionCulling.OccludeeData.Size), (IntPtr)((void*)value), GL.DYNAMIC_DRAW);
			array = null;
			profiling.StopMeasure(this._renderingProfileOcclusionPrepareOccludees);
		}

		// Token: 0x06005257 RID: 21079 RVA: 0x0016A908 File Offset: 0x00168B08
		private void TestOccludees(ref Matrix viewRotationProjectionMatrix)
		{
			Profiling profiling = this._profiling;
			GraphicsDevice graphics = this._graphics;
			GLFunctions gl = graphics.GL;
			profiling.StartMeasure(this._renderingProfileOcclusionTestOccludees);
			HiZCullProgram hiZCullProgram = graphics.GPUProgramStore.HiZCullProgram;
			gl.UseProgram(hiZCullProgram);
			hiZCullProgram.ViewportSize.SetValue((float)graphics.OcclusionMapWidth, (float)graphics.OcclusionMapHeight);
			hiZCullProgram.ViewProjectionMatrix.SetValue(ref viewRotationProjectionMatrix);
			gl.ActiveTexture(GL.TEXTURE0);
			gl.BindTexture(GL.TEXTURE_2D, this._occlusionRenderTarget.GetTexture(RenderTarget.Target.Depth));
			gl.Enable(GL.RASTERIZER_DISCARD);
			gl.BindBufferBase(GL.TRANSFORM_FEEDBACK_BUFFER, 0U, this._visibleOccludeesTFBO.InternalId);
			gl.BeginTransformFeedback(GL.NO_ERROR);
			gl.BindVertexArray(this._occludeesVAO);
			gl.DrawArrays(GL.NO_ERROR, 0, this._occludeesCount);
			gl.EndTransformFeedback();
			gl.Disable(GL.RASTERIZER_DISCARD);
			profiling.StopMeasure(this._renderingProfileOcclusionTestOccludees);
		}

		// Token: 0x06005258 RID: 21080 RVA: 0x0016AA10 File Offset: 0x00168C10
		public void FetchVisibleOccludeesFromGPU(ref int[] visibleOccludees)
		{
			Profiling profiling = this._profiling;
			GLFunctions gl = this._graphics.GL;
			bool isActive = this.IsActive;
			if (isActive)
			{
				profiling.StartMeasure(this._renderingProfileOcclusionFetchResults);
				gl.Flush();
				bool flag = this._occludeesCount > 0;
				if (flag)
				{
					gl.BindBufferBase(GL.TRANSFORM_FEEDBACK_BUFFER, 0U, this._visibleOccludeesTFBO.InternalId);
					IntPtr source = gl.MapBufferRange(GL.TRANSFORM_FEEDBACK_BUFFER, (IntPtr)0, (IntPtr)(4 * this._occludeesCount), GL.ONE);
					Marshal.Copy(source, this._visibleOccludees, 0, this._occludeesCount);
					gl.UnmapBuffer(GL.TRANSFORM_FEEDBACK_BUFFER);
				}
				visibleOccludees = this._visibleOccludees;
				profiling.StopMeasure(this._renderingProfileOcclusionFetchResults);
			}
			else
			{
				profiling.SkipMeasure(this._renderingProfileOcclusionFetchResults);
			}
		}

		// Token: 0x06005259 RID: 21081 RVA: 0x0016AAF8 File Offset: 0x00168CF8
		public void DebugDrawOcclusionMap(float opacity, int mipLevel)
		{
			this._graphics.RTStore.DebugDrawMap(true, false, false, 0, false, RenderTargetStore.DebugMapParam.ChromaSubsamplingMode.None, 0, this._occlusionRenderTarget.Width, this._occlusionRenderTarget.Height, this._occlusionRenderTarget.GetTexture(RenderTarget.Target.Depth), opacity, mipLevel, 0, 1f, 0f, Vector4.One);
		}

		// Token: 0x0600525A RID: 21082 RVA: 0x0016AB54 File Offset: 0x00168D54
		public void DebugDrawOccludees(int occludeeStartIndex, int occludeesCount, ref Matrix viewRotationProjectionMatrix, bool drawCulledOnly = false)
		{
			GLFunctions gl = this._graphics.GL;
			gl.AssertActiveTexture(GL.TEXTURE0);
			Debug.Assert(this._occludeesCount <= this._occludeesData.Length, "OccludeeData array was modified, and is not valid for debugging anymore.");
			bool flag = !this.IsActive || occludeesCount == 0 || this._occludeesCount == 0;
			if (!flag)
			{
				Debug.Assert(occludeeStartIndex < this._occludeesCount);
				Debug.Assert(occludeeStartIndex + occludeesCount <= this._occludeesCount);
				gl.BindTexture(GL.TEXTURE_2D, this._graphics.WhitePixelTexture.GLTexture);
				gl.Disable(GL.DEPTH_TEST);
				gl.BindVertexArray(this._debugOccludeeMesh.VertexArray);
				BasicProgram basicProgram = this._graphics.GPUProgramStore.BasicProgram;
				gl.UseProgram(basicProgram);
				basicProgram.Opacity.SetValue(1f);
				Vector3 vector = new Vector3(1f, 1f, 1f);
				Vector3 vector2 = new Vector3(1f, 0f, 0f);
				for (int i = 0; i < occludeesCount; i++)
				{
					int num = i + occludeeStartIndex;
					bool flag2 = this._visibleOccludees[num] == 1;
					bool flag3 = !flag2 || !drawCulledOnly;
					if (flag3)
					{
						ref OcclusionCulling.OccludeeData ptr = ref this._occludeesData[num];
						Vector3 scale = ptr.BoxMax - ptr.BoxMin;
						Vector3 translation = (ptr.BoxMax + ptr.BoxMin) * 0.5f;
						Matrix matrix;
						Matrix.Compose(scale, Quaternion.Identity, translation, out matrix);
						Matrix.Multiply(ref matrix, ref viewRotationProjectionMatrix, out matrix);
						Vector3 value = flag2 ? vector : vector2;
						basicProgram.Color.SetValue(value);
						basicProgram.MVPMatrix.SetValue(ref matrix);
						gl.DrawElements(GL.TRIANGLES, this._debugOccludeeMesh.Count, GL.UNSIGNED_SHORT, (IntPtr)0);
					}
				}
				gl.Enable(GL.DEPTH_TEST);
			}
		}

		// Token: 0x04002D5D RID: 11613
		public bool IsEnabled = true;

		// Token: 0x04002D5F RID: 11615
		private readonly GraphicsDevice _graphics;

		// Token: 0x04002D60 RID: 11616
		private readonly Profiling _profiling;

		// Token: 0x04002D61 RID: 11617
		private RenderTarget _occlusionRenderTarget;

		// Token: 0x04002D62 RID: 11618
		private RenderTarget _occlusionRenderTargetB;

		// Token: 0x04002D63 RID: 11619
		private GLVertexArray _reprojectedPointsVertexArray;

		// Token: 0x04002D64 RID: 11620
		private Vector4[] _previousFrameInvalidScreenAreas;

		// Token: 0x04002D65 RID: 11621
		private GLVertexArray _occludeesVAO;

		// Token: 0x04002D66 RID: 11622
		private GLBuffer _occludeesPositionsVBO;

		// Token: 0x04002D67 RID: 11623
		private GLBuffer _visibleOccludeesTFBO;

		// Token: 0x04002D68 RID: 11624
		private int MaxOccludees = 2000;

		// Token: 0x04002D69 RID: 11625
		private const int OccludeesGrowth = 500;

		// Token: 0x04002D6A RID: 11626
		private OcclusionCulling.OccludeeData[] _occludeesData;

		// Token: 0x04002D6B RID: 11627
		private int _occludeesCount;

		// Token: 0x04002D6C RID: 11628
		private int[] _visibleOccludees;

		// Token: 0x04002D6D RID: 11629
		private const float MinSkipDuration = 2000f;

		// Token: 0x04002D6E RID: 11630
		private float _skipRemainingTime;

		// Token: 0x04002D6F RID: 11631
		private Mesh _debugOccludeeMesh;

		// Token: 0x04002D70 RID: 11632
		private int _renderingProfileOcclusionBuildMap;

		// Token: 0x04002D71 RID: 11633
		private int _renderingProfileOcclusionRenderOccluders;

		// Token: 0x04002D72 RID: 11634
		private int _renderingProfileOcclusionReproject;

		// Token: 0x04002D73 RID: 11635
		private int _renderingProfileOcclusionCreateHiZ;

		// Token: 0x04002D74 RID: 11636
		private int _renderingProfileOcclusionPrepareOccludees;

		// Token: 0x04002D75 RID: 11637
		private int _renderingProfileOcclusionTestOccludees;

		// Token: 0x04002D76 RID: 11638
		private int _renderingProfileOcclusionFetchResults;

		// Token: 0x02000EAC RID: 3756
		[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 32)]
		public struct OccludeeData
		{
			// Token: 0x04004789 RID: 18313
			public static readonly int Size = Marshal.SizeOf(typeof(OcclusionCulling.OccludeeData));

			// Token: 0x0400478A RID: 18314
			public Vector3 BoxMin;

			// Token: 0x0400478B RID: 18315
			public Vector3 BoxMax;

			// Token: 0x0400478C RID: 18316
			public uint Padding1;

			// Token: 0x0400478D RID: 18317
			public uint Padding2;
		}
	}
}
