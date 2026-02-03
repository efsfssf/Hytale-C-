using System;
using HytaleClient.Core;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Gizmos
{
	// Token: 0x02000A9B RID: 2715
	internal class LineRenderer : Disposable
	{
		// Token: 0x0600557C RID: 21884 RVA: 0x00192A70 File Offset: 0x00190C70
		public LineRenderer(GraphicsDevice graphics, BasicProgram program)
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

		// Token: 0x0600557D RID: 21885 RVA: 0x00192B74 File Offset: 0x00190D74
		public unsafe void UpdateLineData(Vector3[] linePoints)
		{
			GLFunctions gl = this._graphics.GL;
			this._indiceCount = (linePoints.Length - 1) * 2;
			float[] array = new float[linePoints.Length * 8];
			ushort[] array2 = new ushort[this._indiceCount];
			for (int i = 0; i < linePoints.Length; i++)
			{
				array[i * 8] = linePoints[i].X;
				array[i * 8 + 1] = linePoints[i].Y;
				array[i * 8 + 2] = linePoints[i].Z;
				array[i * 8 + 3] = 0f;
				array[i * 8 + 4] = 0f;
				array[i * 8 + 5] = 0f;
				array[i * 8 + 6] = 0f;
				array[i * 8 + 7] = 0f;
			}
			for (int j = 0; j < this._indiceCount / 2; j++)
			{
				array2[j * 2] = (ushort)j;
				array2[j * 2 + 1] = (ushort)(j + 1);
			}
			gl.BindVertexArray(this._vertexArray);
			gl.BindBuffer(this._vertexArray, GL.ARRAY_BUFFER, this._verticesBuffer);
			float[] array3;
			float* value;
			if ((array3 = array) == null || array3.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array3[0];
			}
			gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(array.Length * 4), (IntPtr)((void*)value), GL.STATIC_DRAW);
			array3 = null;
			gl.BindBuffer(this._vertexArray, GL.ELEMENT_ARRAY_BUFFER, this._indicesBuffer);
			ushort[] array4;
			ushort* value2;
			if ((array4 = array2) == null || array4.Length == 0)
			{
				value2 = null;
			}
			else
			{
				value2 = &array4[0];
			}
			gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, (IntPtr)(array2.Length * 2), (IntPtr)((void*)value2), GL.STATIC_DRAW);
			array4 = null;
		}

		// Token: 0x0600557E RID: 21886 RVA: 0x00192D44 File Offset: 0x00190F44
		protected override void DoDispose()
		{
			this._graphics.GL.DeleteBuffer(this._verticesBuffer);
			this._graphics.GL.DeleteBuffer(this._indicesBuffer);
			this._graphics.GL.DeleteVertexArray(this._vertexArray);
		}

		// Token: 0x0600557F RID: 21887 RVA: 0x00192D98 File Offset: 0x00190F98
		public void Draw(ref Matrix MVPMatrix, Vector3 color, float opacity)
		{
			this._program.AssertInUse();
			this._graphics.GL.AssertTextureBound(GL.TEXTURE0, this._graphics.WhitePixelTexture.GLTexture);
			this._program.MVPMatrix.SetValue(ref MVPMatrix);
			this._program.Color.SetValue(color);
			this._program.Opacity.SetValue(opacity);
			GLFunctions gl = this._graphics.GL;
			gl.BindVertexArray(this._vertexArray);
			gl.DrawElements(GL.ONE, this._indiceCount, GL.UNSIGNED_SHORT, (IntPtr)0);
		}

		// Token: 0x04003270 RID: 12912
		private readonly GraphicsDevice _graphics;

		// Token: 0x04003271 RID: 12913
		private readonly BasicProgram _program;

		// Token: 0x04003272 RID: 12914
		private GLVertexArray _vertexArray;

		// Token: 0x04003273 RID: 12915
		private GLBuffer _verticesBuffer;

		// Token: 0x04003274 RID: 12916
		private GLBuffer _indicesBuffer;

		// Token: 0x04003275 RID: 12917
		private int _indiceCount = 0;
	}
}
