using System;
using HytaleClient.Core;
using HytaleClient.Graphics.Gizmos.Models;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Gizmos
{
	// Token: 0x02000A9C RID: 2716
	internal class PrimitiveModelRenderer : Disposable
	{
		// Token: 0x06005580 RID: 21888 RVA: 0x00192E40 File Offset: 0x00191040
		public PrimitiveModelRenderer(GraphicsDevice graphics, BasicProgram program)
		{
			this._graphics = graphics;
			this._program = program;
			GLFunctions gl = graphics.GL;
			this._vertexArray = gl.GenVertexArray();
			gl.BindVertexArray(this._vertexArray);
			this._verticesBuffer = gl.GenBuffer();
			gl.BindBuffer(this._vertexArray, GL.ARRAY_BUFFER, this._verticesBuffer);
			this._indicesBuffer = gl.GenBuffer();
			gl.BindBuffer(this._vertexArray, GL.ELEMENT_ARRAY_BUFFER, this._indicesBuffer);
			gl.EnableVertexAttribArray(program.AttribPosition.Index);
			gl.VertexAttribPointer(program.AttribPosition.Index, 3, GL.FLOAT, false, 32, IntPtr.Zero);
			gl.EnableVertexAttribArray(program.AttribTexCoords.Index);
			gl.VertexAttribPointer(program.AttribTexCoords.Index, 2, GL.FLOAT, false, 32, (IntPtr)12);
		}

		// Token: 0x06005581 RID: 21889 RVA: 0x00192F44 File Offset: 0x00191144
		public unsafe void UpdateModelData(PrimitiveModelData modelData)
		{
			GLFunctions gl = this._graphics.GL;
			this._indiceCount = modelData.Indices.Length;
			gl.BindVertexArray(this._vertexArray);
			gl.BindBuffer(this._vertexArray, GL.ARRAY_BUFFER, this._verticesBuffer);
			float[] array;
			float* value;
			if ((array = modelData.Vertices) == null || array.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array[0];
			}
			gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(modelData.Vertices.Length * 4), (IntPtr)((void*)value), GL.STATIC_DRAW);
			array = null;
			gl.BindBuffer(this._vertexArray, GL.ELEMENT_ARRAY_BUFFER, this._indicesBuffer);
			ushort[] array2;
			ushort* value2;
			if ((array2 = modelData.Indices) == null || array2.Length == 0)
			{
				value2 = null;
			}
			else
			{
				value2 = &array2[0];
			}
			gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, (IntPtr)(modelData.Indices.Length * 2), (IntPtr)((void*)value2), GL.STATIC_DRAW);
			array2 = null;
		}

		// Token: 0x06005582 RID: 21890 RVA: 0x00193048 File Offset: 0x00191248
		protected override void DoDispose()
		{
			GLFunctions gl = this._graphics.GL;
			gl.DeleteBuffer(this._verticesBuffer);
			gl.DeleteBuffer(this._indicesBuffer);
			gl.DeleteVertexArray(this._vertexArray);
		}

		// Token: 0x06005583 RID: 21891 RVA: 0x0019308C File Offset: 0x0019128C
		public void Draw(Matrix viewProjectionMatrix, Matrix transformMatrix, Vector3 color, float opacity, GL drawMode = GL.ONE)
		{
			GLFunctions gl = this._graphics.GL;
			this._program.AssertInUse();
			gl.AssertTextureBound(GL.TEXTURE0, this._graphics.WhitePixelTexture.GLTexture);
			this._matrix = Matrix.Identity;
			Matrix.Multiply(ref this._matrix, ref transformMatrix, out this._matrix);
			Matrix.Multiply(ref this._matrix, ref viewProjectionMatrix, out this._matrix);
			this._program.MVPMatrix.SetValue(ref this._matrix);
			this._program.Color.SetValue(color);
			this._program.Opacity.SetValue(opacity);
			gl.BindVertexArray(this._vertexArray);
			gl.DrawElements(drawMode, this._indiceCount, GL.UNSIGNED_SHORT, (IntPtr)0);
		}

		// Token: 0x04003276 RID: 12918
		private readonly GraphicsDevice _graphics;

		// Token: 0x04003277 RID: 12919
		private readonly BasicProgram _program;

		// Token: 0x04003278 RID: 12920
		private GLVertexArray _vertexArray;

		// Token: 0x04003279 RID: 12921
		private GLBuffer _verticesBuffer;

		// Token: 0x0400327A RID: 12922
		private GLBuffer _indicesBuffer;

		// Token: 0x0400327B RID: 12923
		private Matrix _matrix;

		// Token: 0x0400327C RID: 12924
		private int _indiceCount = 0;
	}
}
