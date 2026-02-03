using System;
using HytaleClient.Core;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.BuilderTools.Tools.Brush
{
	// Token: 0x0200098F RID: 2447
	public class BrushAxisLockPlaneRenderer : Disposable
	{
		// Token: 0x06004E0F RID: 19983 RVA: 0x00157034 File Offset: 0x00155234
		public BrushAxisLockPlaneRenderer(GraphicsDevice graphics)
		{
			this._graphics = graphics;
			GLFunctions gl = this._graphics.GL;
			this._vertexArray = gl.GenVertexArray();
			gl.BindVertexArray(this._vertexArray);
			this._verticesBuffer = gl.GenBuffer();
			gl.BindBuffer(this._vertexArray, GL.ARRAY_BUFFER, this._verticesBuffer);
			this._indicesBuffer = gl.GenBuffer();
			gl.BindBuffer(this._vertexArray, GL.ELEMENT_ARRAY_BUFFER, this._indicesBuffer);
			BasicProgram basicProgram = this._graphics.GPUProgramStore.BasicProgram;
			gl.EnableVertexAttribArray(basicProgram.AttribPosition.Index);
			gl.VertexAttribPointer(basicProgram.AttribPosition.Index, 3, GL.FLOAT, false, 32, IntPtr.Zero);
			gl.EnableVertexAttribArray(basicProgram.AttribTexCoords.Index);
			gl.VertexAttribPointer(basicProgram.AttribTexCoords.Index, 2, GL.FLOAT, false, 32, (IntPtr)12);
		}

		// Token: 0x06004E10 RID: 19984 RVA: 0x00157154 File Offset: 0x00155354
		protected override void DoDispose()
		{
			GLFunctions gl = this._graphics.GL;
			gl.DeleteVertexArray(this._vertexArray);
			gl.DeleteBuffer(this._verticesBuffer);
			gl.DeleteBuffer(this._indicesBuffer);
		}

		// Token: 0x06004E11 RID: 19985 RVA: 0x00157198 File Offset: 0x00155398
		public unsafe void UpdatePlane(Vector3 pos, Matrix rotationMatrix)
		{
			this._centerPos = pos;
			this._rotationMatrix = rotationMatrix;
			int num = 752;
			int num2 = 752;
			int num3 = num + num2;
			this._vertices = new float[num3 * 8];
			this._indices = new ushort[num3 * 2];
			int num4 = 0;
			ushort num5 = 0;
			for (int i = 0; i <= 375; i += 5)
			{
				this.BuildLine(ref num4, ref num5, new Vector3((float)i - 187.5f, -187.5f, 0f), new Vector3((float)i - 187.5f, 187.5f, 0f));
			}
			for (int j = 0; j <= 375; j += 5)
			{
				this.BuildLine(ref num4, ref num5, new Vector3(-187.5f, (float)j - 187.5f, 0f), new Vector3(187.5f, (float)j - 187.5f, 0f));
			}
			GLFunctions gl = this._graphics.GL;
			gl.BindVertexArray(this._vertexArray);
			gl.BindBuffer(this._vertexArray, GL.ARRAY_BUFFER, this._verticesBuffer);
			float[] array;
			float* value;
			if ((array = this._vertices) == null || array.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array[0];
			}
			gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(this._vertices.Length * 4), (IntPtr)((void*)value), GL.STATIC_DRAW);
			array = null;
			gl.BindBuffer(this._vertexArray, GL.ELEMENT_ARRAY_BUFFER, this._indicesBuffer);
			ushort[] array2;
			ushort* value2;
			if ((array2 = this._indices) == null || array2.Length == 0)
			{
				value2 = null;
			}
			else
			{
				value2 = &array2[0];
			}
			gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, (IntPtr)(this._indices.Length * 2), (IntPtr)((void*)value2), GL.STATIC_DRAW);
			array2 = null;
		}

		// Token: 0x06004E12 RID: 19986 RVA: 0x00157390 File Offset: 0x00155590
		private void BuildLine(ref int vertexInc, ref ushort indexInc, Vector3 a, Vector3 b)
		{
			int num = vertexInc / 8;
			this.AddLineVertex(ref vertexInc, a);
			this.AddLineVertex(ref vertexInc, b);
			ushort[] indices = this._indices;
			ushort num2 = indexInc;
			indexInc = num2 + 1;
			indices[(int)num2] = (ushort)num;
			ushort[] indices2 = this._indices;
			num2 = indexInc;
			indexInc = num2 + 1;
			indices2[(int)num2] = (ushort)(num + 1);
		}

		// Token: 0x06004E13 RID: 19987 RVA: 0x001573E0 File Offset: 0x001555E0
		private void AddLineVertex(ref int vertexInc, Vector3 pos)
		{
			float[] vertices = this._vertices;
			int num = vertexInc;
			vertexInc = num + 1;
			vertices[num] = pos.X;
			float[] vertices2 = this._vertices;
			num = vertexInc;
			vertexInc = num + 1;
			vertices2[num] = pos.Y;
			float[] vertices3 = this._vertices;
			num = vertexInc;
			vertexInc = num + 1;
			vertices3[num] = pos.Z;
			float[] vertices4 = this._vertices;
			num = vertexInc;
			vertexInc = num + 1;
			vertices4[num] = 0f;
			float[] vertices5 = this._vertices;
			num = vertexInc;
			vertexInc = num + 1;
			vertices5[num] = 0f;
			float[] vertices6 = this._vertices;
			num = vertexInc;
			vertexInc = num + 1;
			vertices6[num] = 0f;
			float[] vertices7 = this._vertices;
			num = vertexInc;
			vertexInc = num + 1;
			vertices7[num] = 0f;
			float[] vertices8 = this._vertices;
			num = vertexInc;
			vertexInc = num + 1;
			vertices8[num] = 0f;
		}

		// Token: 0x06004E14 RID: 19988 RVA: 0x0015749C File Offset: 0x0015569C
		public void Draw(ref Matrix viewProjectionMatrix, Vector3 positionOffset, Vector3 color, float opacity)
		{
			BasicProgram basicProgram = this._graphics.GPUProgramStore.BasicProgram;
			basicProgram.AssertInUse();
			this._graphics.GL.AssertTextureBound(GL.TEXTURE0, this._graphics.WhitePixelTexture.GLTexture);
			Vector3 vector = this._centerPos + positionOffset;
			Matrix matrix;
			Matrix.CreateTranslation(ref vector, out matrix);
			Matrix.Multiply(ref this._rotationMatrix, ref matrix, out this._matrix);
			Matrix.Multiply(ref this._matrix, ref viewProjectionMatrix, out this._matrix);
			basicProgram.MVPMatrix.SetValue(ref this._matrix);
			GLFunctions gl = this._graphics.GL;
			gl.BindVertexArray(this._vertexArray);
			basicProgram.Color.SetValue(color);
			basicProgram.Opacity.SetValue(opacity);
			gl.DepthFunc((!this._graphics.UseReverseZ) ? GL.LEQUAL : GL.GEQUAL);
			gl.DrawElements(GL.ONE, this._indices.Length, GL.UNSIGNED_SHORT, (IntPtr)0);
		}

		// Token: 0x04002922 RID: 10530
		private const int GridWidth = 75;

		// Token: 0x04002923 RID: 10531
		private const int GridHeight = 75;

		// Token: 0x04002924 RID: 10532
		private const int GridSquareSize = 5;

		// Token: 0x04002925 RID: 10533
		private const int TotalGridWidth = 375;

		// Token: 0x04002926 RID: 10534
		private const int TotalGridHeight = 375;

		// Token: 0x04002927 RID: 10535
		private readonly GraphicsDevice _graphics;

		// Token: 0x04002928 RID: 10536
		private float[] _vertices;

		// Token: 0x04002929 RID: 10537
		private ushort[] _indices;

		// Token: 0x0400292A RID: 10538
		private readonly GLVertexArray _vertexArray;

		// Token: 0x0400292B RID: 10539
		private readonly GLBuffer _verticesBuffer;

		// Token: 0x0400292C RID: 10540
		private readonly GLBuffer _indicesBuffer;

		// Token: 0x0400292D RID: 10541
		private Vector3 _centerPos;

		// Token: 0x0400292E RID: 10542
		private Matrix _matrix = Matrix.Identity;

		// Token: 0x0400292F RID: 10543
		private Matrix _rotationMatrix = Matrix.Identity;
	}
}
