using System;
using System.Collections.Generic;
using HytaleClient.Graphics;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules
{
	// Token: 0x020008F0 RID: 2288
	internal class DebugDisplayModule : Module
	{
		// Token: 0x170010FC RID: 4348
		// (get) Token: 0x060043F3 RID: 17395 RVA: 0x000E3208 File Offset: 0x000E1408
		public bool ShouldDraw
		{
			get
			{
				return this._debugMeshes.Count > 0;
			}
		}

		// Token: 0x060043F4 RID: 17396 RVA: 0x000E3218 File Offset: 0x000E1418
		public DebugDisplayModule(GameInstance gameInstance) : base(gameInstance)
		{
			MeshProcessor.CreateSphere(ref this._sphereMesh, 5, 8, 0.5f, 0, -1, -1);
			MeshProcessor.CreateCylinder(ref this._cylinderMesh, 8, 0.5f, 0, -1, -1);
			MeshProcessor.CreateCone(ref this._coneMesh, 8, 0.5f, 0, -1, -1);
			MeshProcessor.CreateSimpleBox(ref this._cubeMesh, 1f);
		}

		// Token: 0x060043F5 RID: 17397 RVA: 0x000E328A File Offset: 0x000E148A
		protected override void DoDispose()
		{
			base.DoDispose();
			this._sphereMesh.Dispose();
			this._cylinderMesh.Dispose();
			this._coneMesh.Dispose();
			this._cubeMesh.Dispose();
		}

		// Token: 0x060043F6 RID: 17398 RVA: 0x000E32C4 File Offset: 0x000E14C4
		public void AddForce(Vector3 position, Vector3 force, Vector3 color, float time, bool fade)
		{
			float num = (float)Math.Atan2((double)force.Z, (double)force.X);
			float radians = (float)Math.Atan2(Math.Sqrt((double)(force.X * force.X + force.Z * force.Z)), (double)force.Y);
			Matrix baseMatrix = Matrix.CreateRotationX(radians) * Matrix.CreateRotationY(-num + 1.5707964f) * Matrix.CreateTranslation(position);
			this.AddArrow(baseMatrix, color, force.Length(), time, fade);
		}

		// Token: 0x060043F7 RID: 17399 RVA: 0x000E3350 File Offset: 0x000E1550
		public void AddArrow(Matrix baseMatrix, Vector3 debugColor, float length, float time, bool fade)
		{
			length -= 0.3f;
			bool flag = length > 0f;
			if (flag)
			{
				Matrix matrix = Matrix.CreateScale(0.1f, length, 0.1f) * Matrix.CreateTranslation(new Vector3(0f, length * 0.5f, 0f)) * baseMatrix;
				this.Add(1, matrix, time, debugColor, fade);
			}
			Matrix matrix2 = Matrix.CreateScale(0.3f, 0.3f, 0.3f) * Matrix.CreateTranslation(new Vector3(0f, length + 0.15f, 0f)) * baseMatrix;
			this.Add(2, matrix2, time, debugColor, fade);
		}

		// Token: 0x060043F8 RID: 17400 RVA: 0x000E3403 File Offset: 0x000E1603
		public void Add(DisplayDebug.DebugShape shape, Matrix matrix, float time, Vector3 debugColor, bool fade = true)
		{
			this._debugMeshes.Add(new DebugDisplayModule.DebugMesh(shape, matrix, time, debugColor, fade));
		}

		// Token: 0x060043F9 RID: 17401 RVA: 0x000E3420 File Offset: 0x000E1620
		public void Draw(GraphicsDevice graphics, GLFunctions gl, float delta, ref Vector3 cameraPosition, ref Matrix viewProjectionMatrix)
		{
			foreach (DebugDisplayModule.DebugMesh debugMesh in this._debugMeshes)
			{
				Matrix matrix = debugMesh.Matrix * Matrix.CreateTranslation(-cameraPosition) * viewProjectionMatrix;
				graphics.GPUProgramStore.BasicProgram.MVPMatrix.SetValue(ref matrix);
				graphics.GPUProgramStore.BasicProgram.Opacity.SetValue(debugMesh.Fade ? (0.8f * (debugMesh.Time / debugMesh.InitialTime)) : 0.8f);
				graphics.GPUProgramStore.BasicProgram.Color.SetValue(debugMesh.DebugColor);
				ref Mesh ptr = ref this._sphereMesh;
				switch (debugMesh.Shape)
				{
				case 0:
					ptr = ref this._sphereMesh;
					break;
				case 1:
					ptr = ref this._cylinderMesh;
					break;
				case 2:
					ptr = ref this._coneMesh;
					break;
				case 3:
					ptr = ref this._cubeMesh;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				gl.BindVertexArray(ptr.VertexArray);
				gl.DrawElements(GL.TRIANGLES, ptr.Count, GL.UNSIGNED_SHORT, IntPtr.Zero);
				gl.PolygonMode(GL.FRONT_AND_BACK, GL.LINE);
				graphics.GPUProgramStore.BasicProgram.Opacity.SetValue(debugMesh.Fade ? (debugMesh.Time / debugMesh.InitialTime) : 1f);
				graphics.GPUProgramStore.BasicProgram.Color.SetValue(graphics.BlackColor);
				gl.DrawElements(GL.TRIANGLES, ptr.Count, GL.UNSIGNED_SHORT, IntPtr.Zero);
				gl.PolygonMode(GL.FRONT_AND_BACK, GL.FILL);
				debugMesh.Time -= delta;
			}
			this._debugMeshes.RemoveAll((DebugDisplayModule.DebugMesh s) => s.Time <= 0f);
		}

		// Token: 0x0400217D RID: 8573
		private readonly List<DebugDisplayModule.DebugMesh> _debugMeshes = new List<DebugDisplayModule.DebugMesh>();

		// Token: 0x0400217E RID: 8574
		private Mesh _sphereMesh;

		// Token: 0x0400217F RID: 8575
		private Mesh _cylinderMesh;

		// Token: 0x04002180 RID: 8576
		private Mesh _coneMesh;

		// Token: 0x04002181 RID: 8577
		private Mesh _cubeMesh;

		// Token: 0x02000DBD RID: 3517
		public class DebugMesh
		{
			// Token: 0x06006633 RID: 26163 RVA: 0x002134BC File Offset: 0x002116BC
			public DebugMesh(DisplayDebug.DebugShape shape, Matrix matrix, float time, Vector3 debugColor, bool fade)
			{
				this.Shape = shape;
				this.Matrix = matrix;
				this.Time = time;
				this.InitialTime = time;
				this.DebugColor = debugColor;
				this.Fade = fade;
			}

			// Token: 0x040043AA RID: 17322
			public readonly DisplayDebug.DebugShape Shape;

			// Token: 0x040043AB RID: 17323
			public readonly Matrix Matrix;

			// Token: 0x040043AC RID: 17324
			public float Time;

			// Token: 0x040043AD RID: 17325
			public readonly float InitialTime;

			// Token: 0x040043AE RID: 17326
			public readonly Vector3 DebugColor;

			// Token: 0x040043AF RID: 17327
			public readonly bool Fade;
		}
	}
}
