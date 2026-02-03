using System;
using System.Diagnostics;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A38 RID: 2616
	public struct GPUBuffer
	{
		// Token: 0x060051FF RID: 20991 RVA: 0x00167914 File Offset: 0x00165B14
		public static void InitializeGL(GLFunctions gl)
		{
			GPUBuffer._gl = gl;
		}

		// Token: 0x06005200 RID: 20992 RVA: 0x0016791C File Offset: 0x00165B1C
		public static void ReleaseGL()
		{
			GPUBuffer._gl = null;
		}

		// Token: 0x170012B0 RID: 4784
		// (get) Token: 0x06005201 RID: 20993 RVA: 0x00167924 File Offset: 0x00165B24
		public GLBuffer Current
		{
			get
			{
				return this._bufferCurrentRef;
			}
		}

		// Token: 0x170012B1 RID: 4785
		// (get) Token: 0x06005202 RID: 20994 RVA: 0x0016792C File Offset: 0x00165B2C
		public GLBuffer BufferPing
		{
			get
			{
				return this._bufferPing;
			}
		}

		// Token: 0x170012B2 RID: 4786
		// (get) Token: 0x06005203 RID: 20995 RVA: 0x00167934 File Offset: 0x00165B34
		public GLBuffer BufferPong
		{
			get
			{
				return this._bufferPong;
			}
		}

		// Token: 0x170012B3 RID: 4787
		// (get) Token: 0x06005204 RID: 20996 RVA: 0x0016793C File Offset: 0x00165B3C
		public bool UseDoubleBuffering
		{
			get
			{
				return this._useDoubleBuffering;
			}
		}

		// Token: 0x06005205 RID: 20997 RVA: 0x00167944 File Offset: 0x00165B44
		public void CreateStorage(GL target, GL usage, bool useDoubleBuffering, uint size, uint growth, GPUBuffer.GrowthPolicy policy, uint sizeLimit = 0U)
		{
			Debug.Assert(this._bufferCurrentRef == GLBuffer.None, "ERROR: attempt to CreateStorage for a GPUBuffer that already has storage allocated.");
			this._targetType = target;
			this._usageType = usage;
			this._size = size;
			this._growth = growth;
			this._growthPolicy = policy;
			this._sizeLimit = sizeLimit;
			this._useDoubleBuffering = useDoubleBuffering;
			this._bufferPing = GPUBuffer._gl.GenBuffer();
			GPUBuffer._gl.BindBuffer(this._targetType, this._bufferPing);
			GPUBuffer._gl.BufferData(this._targetType, (IntPtr)((long)((ulong)this._size)), IntPtr.Zero, this._usageType);
			bool useDoubleBuffering2 = this._useDoubleBuffering;
			if (useDoubleBuffering2)
			{
				this._bufferPong = GPUBuffer._gl.GenBuffer();
				GPUBuffer._gl.BindBuffer(this._targetType, this._bufferPong);
				GPUBuffer._gl.BufferData(this._targetType, (IntPtr)((long)((ulong)this._size)), IntPtr.Zero, this._usageType);
			}
			bool flag = this._targetType == GL.PIXEL_UNPACK_BUFFER;
			if (flag)
			{
				GPUBuffer._gl.BindBuffer(GL.PIXEL_UNPACK_BUFFER, GLBuffer.None);
			}
			this._bufferCurrentRef = this._bufferPing;
		}

		// Token: 0x06005206 RID: 20998 RVA: 0x00167A8C File Offset: 0x00165C8C
		public void DestroyStorage()
		{
			Debug.Assert(this._bufferCurrentRef != GLBuffer.None, "ERROR: attempt to DestroyStorage for a GPUBuffer that has no storage allocated.");
			GPUBuffer._gl.DeleteBuffer(this._bufferPong);
			bool useDoubleBuffering = this.UseDoubleBuffering;
			if (useDoubleBuffering)
			{
				GPUBuffer._gl.DeleteBuffer(this._bufferPing);
			}
			this._bufferCurrentRef = (this._bufferPing = (this._bufferPong = GLBuffer.None));
		}

		// Token: 0x06005207 RID: 20999 RVA: 0x00167B00 File Offset: 0x00165D00
		public void Swap()
		{
			Debug.Assert(this.UseDoubleBuffering, "ERROR: trying to swap a single buffered GPUBuffer");
			this._bufferCurrentRef = ((this._bufferCurrentRef == this._bufferPing) ? this._bufferPong : this._bufferPing);
		}

		// Token: 0x06005208 RID: 21000 RVA: 0x00167B3C File Offset: 0x00165D3C
		public bool GrowStorageIfNecessary(uint transferSize)
		{
			Debug.Assert(this._growthPolicy != GPUBuffer.GrowthPolicy.Never, "ERROR: GPUBuffer w/ GrowthPolicy 'Never' tried to increase its size.");
			bool result = false;
			bool flag = transferSize > this._size;
			if (flag)
			{
				this._size += Math.Max(this._growth, transferSize - this._size);
				this._size = ((this._growthPolicy == GPUBuffer.GrowthPolicy.GrowthAutoWithLimit) ? Math.Min(this._size, this._sizeLimit) : this._size);
				GPUBuffer._gl.BindBuffer(this._targetType, this._bufferPing);
				GPUBuffer._gl.BufferData(this._targetType, (IntPtr)((long)((ulong)this._size)), IntPtr.Zero, this._usageType);
				bool useDoubleBuffering = this.UseDoubleBuffering;
				if (useDoubleBuffering)
				{
					GPUBuffer._gl.BindBuffer(this._targetType, this._bufferPong);
					GPUBuffer._gl.BufferData(this._targetType, (IntPtr)((long)((ulong)this._size)), IntPtr.Zero, this._usageType);
				}
				result = true;
				bool flag2 = this._targetType == GL.PIXEL_UNPACK_BUFFER;
				if (flag2)
				{
					GPUBuffer._gl.BindBuffer(GL.PIXEL_UNPACK_BUFFER, GLBuffer.None);
				}
			}
			return result;
		}

		// Token: 0x06005209 RID: 21001 RVA: 0x00167C84 File Offset: 0x00165E84
		public IntPtr BeginTransfer(uint transferSize, uint transferStartOffset = 0U)
		{
			Debug.Assert(!this._isTransfering, "Trying to call BeginTransfer() but a transfer was in progres already. Are you missing a call to EndTransfer()?");
			this._isTransfering = true;
			Debug.Assert(transferSize > 0U, "Trying to transfer 0 data will cause a GL_INVALID_OPEARTION.");
			Debug.Assert(transferStartOffset == 0U || (this._growthPolicy != GPUBuffer.GrowthPolicy.GrowthAutoNoLimit && this._growthPolicy != GPUBuffer.GrowthPolicy.GrowthAutoWithLimit), "Trying to transfer data w/ an offset & while using auto growth is unsafe, and should be avoided. Make sure you grow your buffer manually if you want to Transfer its content in more than one batch.");
			bool flag = this._growthPolicy == GPUBuffer.GrowthPolicy.GrowthAutoNoLimit || this._growthPolicy == GPUBuffer.GrowthPolicy.GrowthAutoWithLimit;
			if (flag)
			{
				this.GrowStorageIfNecessary(transferSize);
			}
			GL access = this.UseDoubleBuffering ? ((GL)34U) : ((GL)10U);
			GPUBuffer._gl.BindBuffer(this._targetType, this._bufferCurrentRef);
			return GPUBuffer._gl.MapBufferRange(this._targetType, (IntPtr)((long)((ulong)transferStartOffset)), (IntPtr)((long)((ulong)transferSize)), access);
		}

		// Token: 0x0600520A RID: 21002 RVA: 0x00167D54 File Offset: 0x00165F54
		public void EndTransfer()
		{
			Debug.Assert(this._isTransfering, "Trying to call EndTransfer() but no transfer was in progres. Are you missing a call to BeginTransfer()?");
			this._isTransfering = false;
			GPUBuffer._gl.UnmapBuffer(this._targetType);
			bool flag = this._targetType == GL.PIXEL_UNPACK_BUFFER;
			if (flag)
			{
				GPUBuffer._gl.BindBuffer(GL.PIXEL_UNPACK_BUFFER, GLBuffer.None);
			}
		}

		// Token: 0x0600520B RID: 21003 RVA: 0x00167DB8 File Offset: 0x00165FB8
		public void TransferCopy(IntPtr cpuDataPtr, uint transferSize, uint destinationOffset = 0U)
		{
			bool flag = this._growthPolicy != GPUBuffer.GrowthPolicy.GrowthManual && this._growthPolicy != GPUBuffer.GrowthPolicy.Never;
			if (flag)
			{
				this.GrowStorageIfNecessary(transferSize);
			}
			GPUBuffer._gl.BindBuffer(this._targetType, this._bufferCurrentRef);
			GPUBuffer._gl.BufferSubData(this._targetType, (IntPtr)((long)((ulong)destinationOffset)), (IntPtr)((long)((ulong)transferSize)), cpuDataPtr);
		}

		// Token: 0x0600520C RID: 21004 RVA: 0x00167E28 File Offset: 0x00166028
		public void UnpackToTexture2D(GLTexture texture, int level, int xoffset, int yoffset, int width, int height, GL format, GL type)
		{
			Debug.Assert(this._targetType == GL.PIXEL_UNPACK_BUFFER, "ERROR: attempt to transfer data via PBO w/ a Buffer that is not ready for it.");
			GPUBuffer._gl.BindBuffer(this._targetType, this._bufferCurrentRef);
			GPUBuffer._gl.BindTexture(GL.TEXTURE_2D, texture);
			GPUBuffer._gl.TexSubImage2D(GL.TEXTURE_2D, level, xoffset, yoffset, width, height, format, type, IntPtr.Zero);
			GPUBuffer._gl.BindBuffer(this._targetType, GLBuffer.None);
		}

		// Token: 0x0600520D RID: 21005 RVA: 0x00167EB4 File Offset: 0x001660B4
		public void UnpackToTexture3D(GLTexture texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, GL format, GL type)
		{
			Debug.Assert(this._targetType == GL.PIXEL_UNPACK_BUFFER, "ERROR: attempt to transfer data via PBO w/ a Buffer that is not ready for it.");
			GPUBuffer._gl.BindBuffer(this._targetType, this._bufferCurrentRef);
			GPUBuffer._gl.BindTexture(GL.TEXTURE_3D, texture);
			GPUBuffer._gl.TexSubImage3D(GL.TEXTURE_3D, level, xoffset, yoffset, zoffset, width, height, depth, format, type, IntPtr.Zero);
			GPUBuffer._gl.BindBuffer(this._targetType, GLBuffer.None);
		}

		// Token: 0x04002CC3 RID: 11459
		private static GLFunctions _gl;

		// Token: 0x04002CC4 RID: 11460
		private GLBuffer _bufferCurrentRef;

		// Token: 0x04002CC5 RID: 11461
		private GLBuffer _bufferPing;

		// Token: 0x04002CC6 RID: 11462
		private GLBuffer _bufferPong;

		// Token: 0x04002CC7 RID: 11463
		private GL _targetType;

		// Token: 0x04002CC8 RID: 11464
		private GL _usageType;

		// Token: 0x04002CC9 RID: 11465
		private bool _useDoubleBuffering;

		// Token: 0x04002CCA RID: 11466
		private uint _size;

		// Token: 0x04002CCB RID: 11467
		private uint _growth;

		// Token: 0x04002CCC RID: 11468
		private uint _sizeLimit;

		// Token: 0x04002CCD RID: 11469
		private GPUBuffer.GrowthPolicy _growthPolicy;

		// Token: 0x04002CCE RID: 11470
		private bool _isTransfering;

		// Token: 0x02000EA6 RID: 3750
		public enum GrowthPolicy
		{
			// Token: 0x04004778 RID: 18296
			GrowthAutoNoLimit,
			// Token: 0x04004779 RID: 18297
			GrowthAutoWithLimit,
			// Token: 0x0400477A RID: 18298
			GrowthManual,
			// Token: 0x0400477B RID: 18299
			Never
		}
	}
}
