using System;
using System.Runtime.CompilerServices;
using HytaleClient.Core;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;
using HytaleClient.Utils;

namespace HytaleClient.Graphics
{
	// Token: 0x020009A1 RID: 2465
	internal class ForceFieldFXSystem : Disposable
	{
		// Token: 0x170012A9 RID: 4777
		// (get) Token: 0x06004F20 RID: 20256 RVA: 0x00163CC4 File Offset: 0x00161EC4
		public bool HasColorTasks
		{
			get
			{
				return this._colorDrawTaskCount > 0;
			}
		}

		// Token: 0x170012AA RID: 4778
		// (get) Token: 0x06004F21 RID: 20257 RVA: 0x00163CCF File Offset: 0x00161ECF
		public bool HasDistortionTasks
		{
			get
			{
				return this._distortionDrawTaskCount > 0;
			}
		}

		// Token: 0x06004F22 RID: 20258 RVA: 0x00163CDA File Offset: 0x00161EDA
		public ForceFieldFXSystem(GraphicsDevice graphics)
		{
			this._graphics = graphics;
			this.Initialize();
		}

		// Token: 0x06004F23 RID: 20259 RVA: 0x00163D14 File Offset: 0x00161F14
		private void Initialize()
		{
			ForceFieldProgram forceFieldProgram = this._graphics.GPUProgramStore.ForceFieldProgram;
			MeshProcessor.CreateSphere(ref this._meshSphere, 15, 16, 1f, (int)forceFieldProgram.AttribPosition.Index, (int)forceFieldProgram.AttribTexCoords.Index, (int)forceFieldProgram.AttribNormal.Index);
			MeshProcessor.CreateBox(ref this._meshBox, 2f, (int)forceFieldProgram.AttribPosition.Index, (int)forceFieldProgram.AttribTexCoords.Index, (int)forceFieldProgram.AttribNormal.Index);
			MeshProcessor.CreateQuad(ref this._meshQuad, 2f, (int)forceFieldProgram.AttribPosition.Index, (int)forceFieldProgram.AttribTexCoords.Index, (int)forceFieldProgram.AttribNormal.Index);
		}

		// Token: 0x06004F24 RID: 20260 RVA: 0x00163DCD File Offset: 0x00161FCD
		protected override void DoDispose()
		{
			this._meshBox.Dispose();
			this._meshSphere.Dispose();
			this._meshQuad.Dispose();
		}

		// Token: 0x06004F25 RID: 20261 RVA: 0x00163DF4 File Offset: 0x00161FF4
		public void BeginFrame()
		{
			this._wasSceneDataSent = false;
			this.ResetCounters();
		}

		// Token: 0x06004F26 RID: 20262 RVA: 0x00163E05 File Offset: 0x00162005
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PrepareForIncomingColorTasks(int size)
		{
			this._incomingColorDrawTaskCount += size;
			ArrayUtils.GrowArrayIfNecessary<ForceFieldFXSystem.ColorDrawTask>(ref this._colorDrawTasks, this._colorDrawTaskCount + size, 200);
		}

		// Token: 0x06004F27 RID: 20263 RVA: 0x00163E2F File Offset: 0x0016202F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PrepareForIncomingDistortionTasks(int size)
		{
			this._incomingDistortionDrawTaskCount += size;
			ArrayUtils.GrowArrayIfNecessary<ForceFieldFXSystem.DistortionDrawTask>(ref this._distortionDrawTasks, this._distortionDrawTaskCount + size, 200);
		}

		// Token: 0x06004F28 RID: 20264 RVA: 0x00163E5C File Offset: 0x0016205C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RegisterColorTask(ForceFieldFXSystem.FXShape shape, ref Matrix modelMatrix, ref Matrix normalMatrix, Vector2 uvAnimationSpeed, int outlineMode, Vector4 color, Vector4 intersectionHighlightColorOpacity, float intersectionHighlightThickness)
		{
			int colorDrawTaskCount = this._colorDrawTaskCount;
			this._colorDrawTasks[colorDrawTaskCount].Shape = shape;
			this._colorDrawTasks[colorDrawTaskCount].ModelMatrix = modelMatrix;
			this._colorDrawTasks[colorDrawTaskCount].NormalMatrix = normalMatrix;
			this._colorDrawTasks[colorDrawTaskCount].ColorOpacity = color;
			this._colorDrawTasks[colorDrawTaskCount].IntersectionHighlightColorOpacity = intersectionHighlightColorOpacity;
			this._colorDrawTasks[colorDrawTaskCount].IntersectionHighlightThickness = intersectionHighlightThickness;
			this._colorDrawTasks[colorDrawTaskCount].UVAnimationSpeed.X = uvAnimationSpeed.X;
			this._colorDrawTasks[colorDrawTaskCount].UVAnimationSpeed.Y = uvAnimationSpeed.Y;
			this._colorDrawTasks[colorDrawTaskCount].OutlineMode = outlineMode;
			this._colorDrawTaskCount++;
		}

		// Token: 0x06004F29 RID: 20265 RVA: 0x00163F48 File Offset: 0x00162148
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RegisterDistortionTask(ForceFieldFXSystem.FXShape shape, ref Matrix modelMatrix, Vector2 uvAnimationSpeed)
		{
			int distortionDrawTaskCount = this._distortionDrawTaskCount;
			this._distortionDrawTasks[distortionDrawTaskCount].Shape = shape;
			this._distortionDrawTasks[distortionDrawTaskCount].ModelMatrix = modelMatrix;
			this._distortionDrawTasks[distortionDrawTaskCount].UVAnimationSpeed.X = uvAnimationSpeed.X;
			this._distortionDrawTasks[distortionDrawTaskCount].UVAnimationSpeed.Y = uvAnimationSpeed.Y;
			this._distortionDrawTaskCount++;
		}

		// Token: 0x06004F2A RID: 20266 RVA: 0x00163FCC File Offset: 0x001621CC
		public void SetupSceneData(ref Matrix viewMatrix, ref Matrix viewProjectionMatrix)
		{
			this._viewMatrix = viewMatrix;
			this._viewProjectionMatrix = viewProjectionMatrix;
		}

		// Token: 0x06004F2B RID: 20267 RVA: 0x00163FE8 File Offset: 0x001621E8
		public void DrawColor(bool sendDataToGPU = true)
		{
			ForceFieldProgram forceFieldProgram = this._graphics.GPUProgramStore.ForceFieldProgram;
			GLFunctions gl = this._graphics.GL;
			bool flag = sendDataToGPU && !this._wasSceneDataSent;
			if (flag)
			{
				this.SendSceneDataToGPU();
			}
			for (int i = 0; i < this._colorDrawTaskCount; i++)
			{
				ref ForceFieldFXSystem.ColorDrawTask ptr = ref this._colorDrawTasks[i];
				forceFieldProgram.ModelMatrix.SetValue(ref ptr.ModelMatrix);
				forceFieldProgram.NormalMatrix.SetValue(ref ptr.NormalMatrix);
				forceFieldProgram.UVAnimationSpeed.SetValue(ptr.UVAnimationSpeed);
				forceFieldProgram.OutlineMode.SetValue(ptr.OutlineMode);
				forceFieldProgram.ColorOpacity.SetValue(ptr.ColorOpacity);
				forceFieldProgram.IntersectionHighlightColorOpacity.SetValue(ptr.IntersectionHighlightColorOpacity);
				forceFieldProgram.IntersectionHighlightThickness.SetValue(ptr.IntersectionHighlightThickness);
				switch (ptr.Shape)
				{
				case ForceFieldFXSystem.FXShape.Sphere:
					gl.BindVertexArray(this._meshSphere.VertexArray);
					gl.DrawElements(GL.TRIANGLES, this._meshSphere.Count, GL.UNSIGNED_SHORT, IntPtr.Zero);
					break;
				case ForceFieldFXSystem.FXShape.Box:
					gl.BindVertexArray(this._meshBox.VertexArray);
					gl.DrawArrays(GL.TRIANGLES, 0, this._meshBox.Count);
					break;
				case ForceFieldFXSystem.FXShape.Quad:
					gl.BindVertexArray(this._meshQuad.VertexArray);
					gl.DrawArrays(GL.TRIANGLES, 0, this._meshQuad.Count);
					break;
				}
			}
		}

		// Token: 0x06004F2C RID: 20268 RVA: 0x0016418C File Offset: 0x0016238C
		public void DrawDistortion()
		{
			ForceFieldProgram forceFieldProgram = this._graphics.GPUProgramStore.ForceFieldProgram;
			GLFunctions gl = this._graphics.GL;
			gl.AssertActiveTexture(GL.TEXTURE0);
			gl.BindTexture(GL.TEXTURE_2D, this.NormalMap);
			gl.UseProgram(forceFieldProgram);
			bool flag = !this._wasSceneDataSent;
			if (flag)
			{
				this.SendSceneDataToGPU();
			}
			forceFieldProgram.DrawAndBlendMode.SetValue(forceFieldProgram.DrawModeDistortion, forceFieldProgram.BlendModePremultLinear);
			for (int i = 0; i < this._distortionDrawTaskCount; i++)
			{
				ref ForceFieldFXSystem.DistortionDrawTask ptr = ref this._distortionDrawTasks[i];
				forceFieldProgram.ModelMatrix.SetValue(ref ptr.ModelMatrix);
				forceFieldProgram.UVAnimationSpeed.SetValue(ptr.UVAnimationSpeed);
				switch (ptr.Shape)
				{
				case ForceFieldFXSystem.FXShape.Sphere:
					gl.BindVertexArray(this._meshSphere.VertexArray);
					gl.DrawElements(GL.TRIANGLES, this._meshSphere.Count, GL.UNSIGNED_SHORT, IntPtr.Zero);
					break;
				case ForceFieldFXSystem.FXShape.Box:
					gl.BindVertexArray(this._meshBox.VertexArray);
					gl.DrawArrays(GL.TRIANGLES, 0, this._meshBox.Count);
					break;
				case ForceFieldFXSystem.FXShape.Quad:
					gl.BindVertexArray(this._meshQuad.VertexArray);
					gl.DrawArrays(GL.TRIANGLES, 0, this._meshQuad.Count);
					break;
				}
			}
		}

		// Token: 0x06004F2D RID: 20269 RVA: 0x00164308 File Offset: 0x00162508
		private void ResetCounters()
		{
			this._colorDrawTaskCount = 0;
			this._distortionDrawTaskCount = 0;
			this._incomingColorDrawTaskCount = 0;
			this._incomingDistortionDrawTaskCount = 0;
		}

		// Token: 0x06004F2E RID: 20270 RVA: 0x00164328 File Offset: 0x00162528
		private void SendSceneDataToGPU()
		{
			ForceFieldProgram forceFieldProgram = this._graphics.GPUProgramStore.ForceFieldProgram;
			forceFieldProgram.AssertInUse();
			forceFieldProgram.ViewMatrix.SetValue(ref this._viewMatrix);
			forceFieldProgram.ViewProjectionMatrix.SetValue(ref this._viewProjectionMatrix);
			this._wasSceneDataSent = true;
		}

		// Token: 0x04002A60 RID: 10848
		public GLTexture NormalMap;

		// Token: 0x04002A61 RID: 10849
		private GraphicsDevice _graphics;

		// Token: 0x04002A62 RID: 10850
		private const int DrawTaskDefaultSize = 200;

		// Token: 0x04002A63 RID: 10851
		private const int DrawTaskGrowth = 50;

		// Token: 0x04002A64 RID: 10852
		private ForceFieldFXSystem.ColorDrawTask[] _colorDrawTasks = new ForceFieldFXSystem.ColorDrawTask[200];

		// Token: 0x04002A65 RID: 10853
		private ForceFieldFXSystem.DistortionDrawTask[] _distortionDrawTasks = new ForceFieldFXSystem.DistortionDrawTask[200];

		// Token: 0x04002A66 RID: 10854
		private int _colorDrawTaskCount;

		// Token: 0x04002A67 RID: 10855
		private int _distortionDrawTaskCount;

		// Token: 0x04002A68 RID: 10856
		private int _incomingColorDrawTaskCount;

		// Token: 0x04002A69 RID: 10857
		private int _incomingDistortionDrawTaskCount;

		// Token: 0x04002A6A RID: 10858
		private bool _wasSceneDataSent;

		// Token: 0x04002A6B RID: 10859
		private Matrix _viewMatrix;

		// Token: 0x04002A6C RID: 10860
		private Matrix _viewProjectionMatrix;

		// Token: 0x04002A6D RID: 10861
		private float _farClippingPlane;

		// Token: 0x04002A6E RID: 10862
		private Vector3 _timeSinCos;

		// Token: 0x04002A6F RID: 10863
		private Mesh _meshQuad;

		// Token: 0x04002A70 RID: 10864
		private Mesh _meshSphere;

		// Token: 0x04002A71 RID: 10865
		private Mesh _meshBox;

		// Token: 0x02000E9C RID: 3740
		public enum FXShape
		{
			// Token: 0x04004756 RID: 18262
			Sphere,
			// Token: 0x04004757 RID: 18263
			Box,
			// Token: 0x04004758 RID: 18264
			Quad,
			// Token: 0x04004759 RID: 18265
			Custom,
			// Token: 0x0400475A RID: 18266
			MAX
		}

		// Token: 0x02000E9D RID: 3741
		private struct ColorDrawTask
		{
			// Token: 0x0400475B RID: 18267
			public Matrix ModelMatrix;

			// Token: 0x0400475C RID: 18268
			public Matrix NormalMatrix;

			// Token: 0x0400475D RID: 18269
			public Vector4 ColorOpacity;

			// Token: 0x0400475E RID: 18270
			public Vector4 IntersectionHighlightColorOpacity;

			// Token: 0x0400475F RID: 18271
			public float IntersectionHighlightThickness;

			// Token: 0x04004760 RID: 18272
			public Vector2 UVAnimationSpeed;

			// Token: 0x04004761 RID: 18273
			public int OutlineMode;

			// Token: 0x04004762 RID: 18274
			public ForceFieldFXSystem.FXShape Shape;
		}

		// Token: 0x02000E9E RID: 3742
		private struct DistortionDrawTask
		{
			// Token: 0x04004763 RID: 18275
			public Matrix ModelMatrix;

			// Token: 0x04004764 RID: 18276
			public Vector2 UVAnimationSpeed;

			// Token: 0x04004765 RID: 18277
			public ForceFieldFXSystem.FXShape Shape;
		}
	}
}
