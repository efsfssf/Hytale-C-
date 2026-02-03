using System;
using HytaleClient.Core;
using HytaleClient.Graphics.Programs;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A43 RID: 2627
	public class QuadRenderer : Disposable
	{
		// Token: 0x060052C3 RID: 21187 RVA: 0x0016FB5C File Offset: 0x0016DD5C
		public QuadRenderer(GraphicsDevice graphics, Attrib attribPosition, Attrib attribTexCoords)
		{
			this._graphics = graphics;
			GLFunctions gl = this._graphics.GL;
			this._vertexArray = gl.GenVertexArray();
			gl.BindVertexArray(this._vertexArray);
			this._vertexBuffer = gl.GenBuffer();
			gl.BindBuffer(this._vertexArray, GL.ARRAY_BUFFER, this._vertexBuffer);
			gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(QuadRenderer.Vertices.Length * 4), IntPtr.Zero, GL.DYNAMIC_DRAW);
			gl.EnableVertexAttribArray(attribPosition.Index);
			gl.VertexAttribPointer(attribPosition.Index, 3, GL.FLOAT, false, 32, IntPtr.Zero);
			gl.EnableVertexAttribArray(attribTexCoords.Index);
			gl.VertexAttribPointer(attribTexCoords.Index, 2, GL.FLOAT, false, 32, (IntPtr)12);
			this.UpdateUVs(1f, 1f, 0f, 0f);
		}

		// Token: 0x060052C4 RID: 21188 RVA: 0x0016FC61 File Offset: 0x0016DE61
		protected override void DoDispose()
		{
			this._graphics.GL.DeleteBuffer(this._vertexBuffer);
			this._graphics.GL.DeleteVertexArray(this._vertexArray);
		}

		// Token: 0x060052C5 RID: 21189 RVA: 0x0016FC94 File Offset: 0x0016DE94
		public unsafe void UpdateUVs(float right, float bottom, float left = 0f, float top = 0f)
		{
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(QuadRenderer).FullName);
			}
			GLFunctions gl = this._graphics.GL;
			gl.BindVertexArray(this._vertexArray);
			gl.BindBuffer(this._vertexArray, GL.ARRAY_BUFFER, this._vertexBuffer);
			float[] vertices = QuadRenderer.Vertices;
			int num = 3;
			float[] vertices2 = QuadRenderer.Vertices;
			int num2 = 27;
			QuadRenderer.Vertices[43] = right;
			vertices[num] = (vertices2[num2] = right);
			float[] vertices3 = QuadRenderer.Vertices;
			int num3 = 20;
			float[] vertices4 = QuadRenderer.Vertices;
			int num4 = 36;
			QuadRenderer.Vertices[44] = bottom;
			vertices3[num3] = (vertices4[num4] = bottom);
			float[] vertices5 = QuadRenderer.Vertices;
			int num5 = 11;
			float[] vertices6 = QuadRenderer.Vertices;
			int num6 = 19;
			QuadRenderer.Vertices[35] = left;
			vertices5[num5] = (vertices6[num6] = left);
			float[] vertices7 = QuadRenderer.Vertices;
			int num7 = 4;
			float[] vertices8 = QuadRenderer.Vertices;
			int num8 = 12;
			QuadRenderer.Vertices[28] = top;
			vertices7[num7] = (vertices8[num8] = top);
			float[] array;
			float* value;
			if ((array = QuadRenderer.Vertices) == null || array.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array[0];
			}
			gl.BufferSubData(GL.ARRAY_BUFFER, IntPtr.Zero, (IntPtr)(QuadRenderer.Vertices.Length * 4), (IntPtr)((void*)value));
			array = null;
		}

		// Token: 0x060052C6 RID: 21190 RVA: 0x0016FDBC File Offset: 0x0016DFBC
		public void Draw()
		{
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(QuadRenderer).FullName);
			}
			GLFunctions gl = this._graphics.GL;
			gl.BindVertexArray(this._vertexArray);
			gl.DrawArrays(GL.TRIANGLES, 0, 6);
		}

		// Token: 0x060052C7 RID: 21191 RVA: 0x0016FE0C File Offset: 0x0016E00C
		public void BindVertexArray()
		{
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(QuadRenderer).FullName);
			}
			GLFunctions gl = this._graphics.GL;
			gl.BindVertexArray(this._vertexArray);
		}

		// Token: 0x060052C8 RID: 21192 RVA: 0x0016FE54 File Offset: 0x0016E054
		public void DrawRaw()
		{
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(QuadRenderer).FullName);
			}
			GLFunctions gl = this._graphics.GL;
			gl.DrawArrays(GL.TRIANGLES, 0, 6);
		}

		// Token: 0x04002DAD RID: 11693
		private readonly GraphicsDevice _graphics;

		// Token: 0x04002DAE RID: 11694
		private readonly GLVertexArray _vertexArray;

		// Token: 0x04002DAF RID: 11695
		private readonly GLBuffer _vertexBuffer;

		// Token: 0x04002DB0 RID: 11696
		private static readonly float[] Vertices = new float[]
		{
			1f,
			1f,
			0f,
			1f,
			0f,
			0f,
			0f,
			0f,
			0f,
			1f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			1f,
			0f,
			0f,
			0f,
			1f,
			1f,
			0f,
			1f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			1f,
			0f,
			0f,
			0f,
			1f,
			0f,
			0f,
			1f,
			1f,
			0f,
			0f,
			0f
		};
	}
}
