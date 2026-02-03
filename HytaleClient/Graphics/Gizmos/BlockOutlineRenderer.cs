using System;
using HytaleClient.Core;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Gizmos
{
	// Token: 0x02000A95 RID: 2709
	internal class BlockOutlineRenderer : Disposable
	{
		// Token: 0x06005546 RID: 21830 RVA: 0x0018F008 File Offset: 0x0018D208
		public unsafe BlockOutlineRenderer(GraphicsDevice graphics)
		{
			this._graphics = graphics;
			GLFunctions gl = graphics.GL;
			this._vertexArray = gl.GenVertexArray();
			gl.BindVertexArray(this._vertexArray);
			this._verticesBuffer = gl.GenBuffer();
			gl.BindBuffer(this._vertexArray, GL.ARRAY_BUFFER, this._verticesBuffer);
			float[] array;
			float* value;
			if ((array = BlockOutlineRenderer.Vertices) == null || array.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array[0];
			}
			gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(BlockOutlineRenderer.Vertices.Length * 4), (IntPtr)((void*)value), GL.STATIC_DRAW);
			array = null;
			this._indicesBuffer = gl.GenBuffer();
			gl.BindBuffer(this._vertexArray, GL.ELEMENT_ARRAY_BUFFER, this._indicesBuffer);
			ushort[] array2;
			ushort* value2;
			if ((array2 = BlockOutlineRenderer.Indices) == null || array2.Length == 0)
			{
				value2 = null;
			}
			else
			{
				value2 = &array2[0];
			}
			gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, (IntPtr)(BlockOutlineRenderer.Indices.Length * 2), (IntPtr)((void*)value2), GL.STATIC_DRAW);
			array2 = null;
			BasicProgram basicProgram = this._graphics.GPUProgramStore.BasicProgram;
			gl.EnableVertexAttribArray(basicProgram.AttribPosition.Index);
			gl.VertexAttribPointer(basicProgram.AttribPosition.Index, 3, GL.FLOAT, false, 32, IntPtr.Zero);
			gl.EnableVertexAttribArray(basicProgram.AttribTexCoords.Index);
			gl.VertexAttribPointer(basicProgram.AttribTexCoords.Index, 2, GL.FLOAT, false, 32, (IntPtr)12);
		}

		// Token: 0x06005547 RID: 21831 RVA: 0x0018F1A8 File Offset: 0x0018D3A8
		protected override void DoDispose()
		{
			GLFunctions gl = this._graphics.GL;
			gl.DeleteBuffer(this._verticesBuffer);
			gl.DeleteBuffer(this._indicesBuffer);
			gl.DeleteVertexArray(this._vertexArray);
		}

		// Token: 0x06005548 RID: 21832 RVA: 0x0018F1EC File Offset: 0x0018D3EC
		public void Draw(Vector3 position, BlockHitbox hitbox, Matrix viewProjectionMatrix, bool debugOutline)
		{
			BasicProgram basicProgram = this._graphics.GPUProgramStore.BasicProgram;
			basicProgram.AssertInUse();
			basicProgram.Opacity.AssertValue(0.12f);
			this._graphics.GL.AssertTextureBound(GL.TEXTURE0, this._graphics.WhitePixelTexture.GLTexture);
			basicProgram.Color.SetValue(debugOutline ? this._graphics.BlueColor : this._graphics.BlackColor);
			Vector3 vector = hitbox.BoundingBox.Max - hitbox.BoundingBox.Min;
			position += hitbox.BoundingBox.Min;
			Matrix.CreateScale(ref vector, out this._matrix);
			Matrix.CreateTranslation(ref position, out this._tempMatrix);
			Matrix.Multiply(ref this._matrix, ref this._tempMatrix, out this._matrix);
			Matrix.Multiply(ref this._matrix, ref viewProjectionMatrix, out this._matrix);
			basicProgram.MVPMatrix.SetValue(ref this._matrix);
			GLFunctions gl = this._graphics.GL;
			gl.BindVertexArray(this._vertexArray);
			gl.DrawElements(GL.TRIANGLES, BlockOutlineRenderer.Indices.Length, GL.UNSIGNED_SHORT, (IntPtr)0);
		}

		// Token: 0x0400321D RID: 12829
		private readonly GraphicsDevice _graphics;

		// Token: 0x0400321E RID: 12830
		private GLVertexArray _vertexArray;

		// Token: 0x0400321F RID: 12831
		private GLBuffer _verticesBuffer;

		// Token: 0x04003220 RID: 12832
		private GLBuffer _indicesBuffer;

		// Token: 0x04003221 RID: 12833
		private Matrix _matrix;

		// Token: 0x04003222 RID: 12834
		private Matrix _tempMatrix;

		// Token: 0x04003223 RID: 12835
		private const float LineWidth = 0.01f;

		// Token: 0x04003224 RID: 12836
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
			0f,
			1f,
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
			0f,
			0.01f,
			0f,
			1f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0.01f,
			1f,
			1f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0.99f,
			1f,
			0f,
			0f,
			0f,
			0f,
			0f,
			1f,
			0.99f,
			1f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0.99f,
			1f,
			1f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0.99f,
			0f,
			1f,
			0f,
			0f,
			0f,
			0f,
			0f,
			1f,
			0.01f,
			1f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0.01f,
			1f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0.01f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0.01f,
			1f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0.99f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			1f,
			0.99f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0.99f,
			1f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0.99f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			1f,
			0.01f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0.01f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0.01f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			1f,
			0.01f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			1f,
			0.99f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0.99f,
			0f,
			0f,
			0f,
			0f,
			0f,
			1f,
			0f,
			0.01f,
			0f,
			0f,
			0f,
			0f,
			0f,
			1f,
			1f,
			0.01f,
			0f,
			0f,
			0f,
			0f,
			0f,
			1f,
			1f,
			0.99f,
			0f,
			0f,
			0f,
			0f,
			0f,
			1f,
			0f,
			0.99f,
			0f,
			0f,
			0f,
			0f,
			0f
		};

		// Token: 0x04003225 RID: 12837
		private static readonly ushort[] Indices = new ushort[]
		{
			0,
			1,
			8,
			8,
			1,
			9,
			1,
			2,
			10,
			10,
			2,
			11,
			2,
			3,
			12,
			12,
			3,
			13,
			3,
			0,
			14,
			14,
			0,
			15,
			4,
			5,
			16,
			16,
			5,
			17,
			5,
			6,
			18,
			18,
			6,
			19,
			6,
			7,
			20,
			20,
			7,
			21,
			7,
			4,
			22,
			22,
			4,
			23,
			5,
			4,
			24,
			24,
			5,
			25,
			1,
			5,
			18,
			18,
			1,
			10,
			0,
			1,
			26,
			26,
			0,
			27,
			4,
			0,
			15,
			15,
			4,
			23,
			6,
			7,
			28,
			28,
			6,
			29,
			2,
			6,
			19,
			19,
			2,
			11,
			3,
			2,
			30,
			30,
			3,
			31,
			7,
			3,
			14,
			14,
			7,
			22,
			1,
			5,
			9,
			9,
			5,
			17,
			2,
			1,
			30,
			30,
			1,
			26,
			6,
			2,
			20,
			20,
			2,
			12,
			6,
			5,
			29,
			29,
			5,
			25,
			0,
			4,
			8,
			8,
			4,
			16,
			3,
			0,
			31,
			31,
			0,
			27,
			7,
			3,
			21,
			21,
			3,
			13,
			7,
			4,
			28,
			28,
			4,
			24
		};
	}
}
