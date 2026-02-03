using System;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A47 RID: 2631
	internal struct Mesh
	{
		// Token: 0x170012F0 RID: 4848
		// (get) Token: 0x060053BE RID: 21438 RVA: 0x0017DADF File Offset: 0x0017BCDF
		public bool UseIndices
		{
			get
			{
				return this.IndicesBuffer != GLBuffer.None;
			}
		}

		// Token: 0x060053BF RID: 21439 RVA: 0x0017DAF1 File Offset: 0x0017BCF1
		public static void InitializeGL(GLFunctions gl)
		{
			Mesh._gl = gl;
		}

		// Token: 0x060053C0 RID: 21440 RVA: 0x0017DAF9 File Offset: 0x0017BCF9
		public static void ReleaseGL()
		{
			Mesh._gl = null;
		}

		// Token: 0x060053C1 RID: 21441 RVA: 0x0017DB04 File Offset: 0x0017BD04
		public void CreateGPUData(bool useIndices = true)
		{
			this.Count = 0;
			this.VertexArray = Mesh._gl.GenVertexArray();
			this.VerticesBuffer = Mesh._gl.GenBuffer();
			this.IndicesBuffer = (useIndices ? Mesh._gl.GenBuffer() : GLBuffer.None);
		}

		// Token: 0x060053C2 RID: 21442 RVA: 0x0017DB54 File Offset: 0x0017BD54
		public void Dispose()
		{
			bool flag = this.IndicesBuffer != GLBuffer.None;
			if (flag)
			{
				Mesh._gl.DeleteBuffer(this.IndicesBuffer);
			}
			Mesh._gl.DeleteBuffer(this.VerticesBuffer);
			Mesh._gl.DeleteVertexArray(this.VertexArray);
			this.IndicesBuffer = GLBuffer.None;
			this.VerticesBuffer = GLBuffer.None;
			this.VertexArray = GLVertexArray.None;
		}

		// Token: 0x04002EAF RID: 11951
		public int Count;

		// Token: 0x04002EB0 RID: 11952
		public GLVertexArray VertexArray;

		// Token: 0x04002EB1 RID: 11953
		public GLBuffer VerticesBuffer;

		// Token: 0x04002EB2 RID: 11954
		public GLBuffer IndicesBuffer;

		// Token: 0x04002EB3 RID: 11955
		private static GLFunctions _gl;
	}
}
