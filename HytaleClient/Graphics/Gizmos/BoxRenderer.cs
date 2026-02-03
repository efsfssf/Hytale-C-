using System;
using HytaleClient.Core;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Gizmos
{
	// Token: 0x02000A98 RID: 2712
	internal class BoxRenderer : Disposable
	{
		// Token: 0x0600556B RID: 21867 RVA: 0x001913F4 File Offset: 0x0018F5F4
		public unsafe BoxRenderer(GraphicsDevice graphics, BasicProgram program)
		{
			this._graphics = graphics;
			this._program = program;
			GLFunctions gl = graphics.GL;
			this._vertexArray = gl.GenVertexArray();
			gl.BindVertexArray(this._vertexArray);
			this._verticesBuffer = gl.GenBuffer();
			gl.BindBuffer(this._vertexArray, GL.ARRAY_BUFFER, this._verticesBuffer);
			float[] array;
			float* value;
			if ((array = BoxRenderer.Vertices) == null || array.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array[0];
			}
			gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(BoxRenderer.Vertices.Length * 4), (IntPtr)((void*)value), GL.STATIC_DRAW);
			array = null;
			this._indicesBuffer = gl.GenBuffer();
			gl.BindBuffer(this._vertexArray, GL.ELEMENT_ARRAY_BUFFER, this._indicesBuffer);
			ushort[] array2;
			ushort* value2;
			if ((array2 = BoxRenderer.Indices) == null || array2.Length == 0)
			{
				value2 = null;
			}
			else
			{
				value2 = &array2[0];
			}
			gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, (IntPtr)(BoxRenderer.Indices.Length * 2), (IntPtr)((void*)value2), GL.STATIC_DRAW);
			array2 = null;
			gl.EnableVertexAttribArray(program.AttribPosition.Index);
			gl.VertexAttribPointer(program.AttribPosition.Index, 3, GL.FLOAT, false, 32, IntPtr.Zero);
			gl.EnableVertexAttribArray(program.AttribTexCoords.Index);
			gl.VertexAttribPointer(program.AttribTexCoords.Index, 2, GL.FLOAT, false, 32, (IntPtr)12);
		}

		// Token: 0x0600556C RID: 21868 RVA: 0x00191588 File Offset: 0x0018F788
		protected override void DoDispose()
		{
			this._graphics.GL.DeleteBuffer(this._verticesBuffer);
			this._graphics.GL.DeleteBuffer(this._indicesBuffer);
			this._graphics.GL.DeleteVertexArray(this._vertexArray);
		}

		// Token: 0x0600556D RID: 21869 RVA: 0x001915DC File Offset: 0x0018F7DC
		public void Draw(ref Matrix MVPMatrix, Vector3 outlineColor, float outlineOpacity, Vector3 quadColor, float quadOpacity)
		{
			this._program.AssertInUse();
			this._graphics.GL.AssertTextureBound(GL.TEXTURE0, this._graphics.WhitePixelTexture.GLTexture);
			this._program.MVPMatrix.SetValue(ref MVPMatrix);
			this._program.Color.SetValue(quadColor);
			this._program.Opacity.SetValue(quadOpacity);
			GLFunctions gl = this._graphics.GL;
			gl.BindVertexArray(this._vertexArray);
			gl.DrawElements(GL.TRIANGLES, 36, GL.UNSIGNED_SHORT, (IntPtr)0);
			bool flag = quadOpacity != outlineOpacity;
			if (flag)
			{
				this._program.Opacity.SetValue(outlineOpacity);
			}
			bool flag2 = quadColor != outlineColor;
			if (flag2)
			{
				this._program.Color.SetValue(outlineColor);
			}
			gl.DrawElements(GL.ONE, 24, GL.UNSIGNED_SHORT, (IntPtr)72);
		}

		// Token: 0x0600556E RID: 21870 RVA: 0x001916D4 File Offset: 0x0018F8D4
		public void Draw(Vector3 position, BoundingBox box, Matrix viewProjectionMatrix, Vector3 outlineColor, float outlineOpacity, Vector3 quadColor, float quadOpacity)
		{
			this._program.AssertInUse();
			this._graphics.GL.AssertTextureBound(GL.TEXTURE0, this._graphics.WhitePixelTexture.GLTexture);
			Vector3 vector = box.Min + position;
			Vector3 vector2 = box.GetSize() / Vector3.One;
			Matrix.CreateScale(ref vector2, out this._matrix);
			Matrix.CreateTranslation(ref vector, out this._tempMatrix);
			Matrix.Multiply(ref this._matrix, ref this._tempMatrix, out this._matrix);
			Matrix.Multiply(ref this._matrix, ref viewProjectionMatrix, out this._matrix);
			this._program.MVPMatrix.SetValue(ref this._matrix);
			this._program.Color.SetValue(quadColor);
			this._program.Opacity.SetValue(quadOpacity);
			GLFunctions gl = this._graphics.GL;
			gl.BindVertexArray(this._vertexArray);
			gl.DrawElements(GL.TRIANGLES, 36, GL.UNSIGNED_SHORT, (IntPtr)0);
			bool flag = quadOpacity != outlineOpacity;
			if (flag)
			{
				this._program.Opacity.SetValue(outlineOpacity);
			}
			bool flag2 = quadColor != outlineColor;
			if (flag2)
			{
				this._program.Color.SetValue(outlineColor);
			}
			gl.DrawElements(GL.ONE, 24, GL.UNSIGNED_SHORT, (IntPtr)72);
		}

		// Token: 0x04003247 RID: 12871
		private const int QuadIndiceCount = 36;

		// Token: 0x04003248 RID: 12872
		private const int OutlineIndiceCount = 24;

		// Token: 0x04003249 RID: 12873
		private readonly GraphicsDevice _graphics;

		// Token: 0x0400324A RID: 12874
		private readonly BasicProgram _program;

		// Token: 0x0400324B RID: 12875
		private GLVertexArray _vertexArray;

		// Token: 0x0400324C RID: 12876
		private GLBuffer _verticesBuffer;

		// Token: 0x0400324D RID: 12877
		private GLBuffer _indicesBuffer;

		// Token: 0x0400324E RID: 12878
		private Matrix _matrix;

		// Token: 0x0400324F RID: 12879
		private Matrix _tempMatrix;

		// Token: 0x04003250 RID: 12880
		private static readonly float[] Vertices = new float[]
		{
			0f,
			0f,
			1f,
			0f,
			0f,
			0f,
			0f,
			0f,
			1f,
			0f,
			1f,
			0f,
			0f,
			0f,
			0f,
			0f,
			1f,
			1f,
			1f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			1f,
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
			1f,
			1f,
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
			0f,
			0f,
			0f,
			0f
		};

		// Token: 0x04003251 RID: 12881
		private static readonly ushort[] Indices = new ushort[]
		{
			0,
			1,
			2,
			0,
			2,
			3,
			4,
			5,
			6,
			4,
			6,
			7,
			5,
			3,
			2,
			5,
			2,
			6,
			4,
			7,
			1,
			4,
			1,
			0,
			7,
			6,
			2,
			7,
			2,
			1,
			4,
			0,
			3,
			4,
			3,
			5,
			0,
			1,
			1,
			2,
			2,
			3,
			3,
			0,
			4,
			5,
			5,
			6,
			6,
			7,
			7,
			4,
			0,
			4,
			1,
			7,
			2,
			6,
			3,
			5
		};
	}
}
