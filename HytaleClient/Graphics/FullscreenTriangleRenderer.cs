using System;
using System.Runtime.CompilerServices;
using HytaleClient.Core;

namespace HytaleClient.Graphics
{
	// Token: 0x020009A0 RID: 2464
	public class FullscreenTriangleRenderer : Disposable
	{
		// Token: 0x06004F1B RID: 20251 RVA: 0x00163BF4 File Offset: 0x00161DF4
		public FullscreenTriangleRenderer(GraphicsDevice graphics)
		{
			this._graphics = graphics;
			GLFunctions gl = this._graphics.GL;
			this._vertexArray = gl.GenVertexArray();
		}

		// Token: 0x06004F1C RID: 20252 RVA: 0x00163C28 File Offset: 0x00161E28
		protected override void DoDispose()
		{
			this._graphics.GL.DeleteVertexArray(this._vertexArray);
		}

		// Token: 0x06004F1D RID: 20253 RVA: 0x00163C44 File Offset: 0x00161E44
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Draw()
		{
			GLFunctions gl = this._graphics.GL;
			gl.BindVertexArray(this._vertexArray);
			gl.DrawArrays(GL.TRIANGLES, 0, 3);
		}

		// Token: 0x06004F1E RID: 20254 RVA: 0x00163C78 File Offset: 0x00161E78
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindVertexArray()
		{
			GLFunctions gl = this._graphics.GL;
			gl.BindVertexArray(this._vertexArray);
		}

		// Token: 0x06004F1F RID: 20255 RVA: 0x00163CA0 File Offset: 0x00161EA0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawRaw()
		{
			GLFunctions gl = this._graphics.GL;
			gl.DrawArrays(GL.TRIANGLES, 0, 3);
		}

		// Token: 0x04002A5E RID: 10846
		private readonly GraphicsDevice _graphics;

		// Token: 0x04002A5F RID: 10847
		private readonly GLVertexArray _vertexArray;
	}
}
