using System;
using System.Diagnostics;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A39 RID: 2617
	public struct GPUBufferTexture
	{
		// Token: 0x0600520E RID: 21006 RVA: 0x00167F42 File Offset: 0x00166142
		public static void InitializeGL(GLFunctions gl)
		{
			GPUBufferTexture._gl = gl;
		}

		// Token: 0x0600520F RID: 21007 RVA: 0x00167F4A File Offset: 0x0016614A
		public static void ReleaseGL()
		{
			GPUBufferTexture._gl = null;
		}

		// Token: 0x170012B4 RID: 4788
		// (get) Token: 0x06005210 RID: 21008 RVA: 0x00167F52 File Offset: 0x00166152
		public GLBuffer CurrentBuffer
		{
			get
			{
				return this._gpuBuffer.Current;
			}
		}

		// Token: 0x170012B5 RID: 4789
		// (get) Token: 0x06005211 RID: 21009 RVA: 0x00167F5F File Offset: 0x0016615F
		public GLTexture CurrentTexture
		{
			get
			{
				return this._bufferTextureCurrentRef;
			}
		}

		// Token: 0x170012B6 RID: 4790
		// (get) Token: 0x06005212 RID: 21010 RVA: 0x00167F67 File Offset: 0x00166167
		public bool UseDoubleBuffering
		{
			get
			{
				return this._gpuBuffer.UseDoubleBuffering;
			}
		}

		// Token: 0x06005213 RID: 21011 RVA: 0x00167F74 File Offset: 0x00166174
		public void CreateStorage(GL storageFormat, GL usage, bool useDoubleBuffering, uint size, uint growth, GPUBuffer.GrowthPolicy policy, uint sizeLimit = 0U)
		{
			Debug.Assert(this._bufferTextureCurrentRef == GLTexture.None, "ERROR: attempt to CreateStorage for a GPUBufferTexture that already has storage allocated.");
			this._gpuBuffer.CreateStorage(GL.TEXTURE_BUFFER, usage, useDoubleBuffering, size, growth, policy, sizeLimit);
			this._storageFormat = storageFormat;
			this._bufferTexturePing = GPUBufferTexture._gl.GenTexture();
			GPUBufferTexture._gl.BindTexture(GL.TEXTURE_BUFFER, this._bufferTexturePing);
			GPUBufferTexture._gl.TexBuffer(GL.TEXTURE_BUFFER, this._storageFormat, this._gpuBuffer.BufferPing.InternalId);
			bool useDoubleBuffering2 = this.UseDoubleBuffering;
			if (useDoubleBuffering2)
			{
				this._bufferTexturePong = GPUBufferTexture._gl.GenTexture();
				GPUBufferTexture._gl.BindTexture(GL.TEXTURE_BUFFER, this._bufferTexturePong);
				GPUBufferTexture._gl.TexBuffer(GL.TEXTURE_BUFFER, this._storageFormat, this._gpuBuffer.BufferPong.InternalId);
			}
			this._bufferTextureCurrentRef = this._bufferTexturePing;
		}

		// Token: 0x06005214 RID: 21012 RVA: 0x0016807C File Offset: 0x0016627C
		public void DestroyStorage()
		{
			Debug.Assert(this._bufferTextureCurrentRef != GLTexture.None, "ERROR: attempt to DestroyStorage for a GPUBufferTexture that has no storage allocated.");
			GPUBufferTexture._gl.DeleteTexture(this._bufferTexturePong);
			bool useDoubleBuffering = this.UseDoubleBuffering;
			if (useDoubleBuffering)
			{
				GPUBufferTexture._gl.DeleteTexture(this._bufferTexturePing);
			}
			this._bufferTextureCurrentRef = (this._bufferTexturePing = (this._bufferTexturePong = GLTexture.None));
			this._gpuBuffer.DestroyStorage();
		}

		// Token: 0x06005215 RID: 21013 RVA: 0x001680FC File Offset: 0x001662FC
		public void Swap()
		{
			Debug.Assert(this.UseDoubleBuffering, "ERROR: trying to swap a single buffered GPUBuffer");
			this._gpuBuffer.Swap();
			this._bufferTextureCurrentRef = ((this._bufferTextureCurrentRef == this._bufferTexturePing) ? this._bufferTexturePong : this._bufferTexturePing);
		}

		// Token: 0x06005216 RID: 21014 RVA: 0x0016814E File Offset: 0x0016634E
		public void GrowStorageIfNecessary(uint transferSize)
		{
			this._gpuBuffer.GrowStorageIfNecessary(transferSize);
		}

		// Token: 0x06005217 RID: 21015 RVA: 0x00168160 File Offset: 0x00166360
		public IntPtr BeginTransfer(uint transferSize)
		{
			return this._gpuBuffer.BeginTransfer(transferSize, 0U);
		}

		// Token: 0x06005218 RID: 21016 RVA: 0x0016817F File Offset: 0x0016637F
		public void EndTransfer()
		{
			this._gpuBuffer.EndTransfer();
		}

		// Token: 0x06005219 RID: 21017 RVA: 0x0016818E File Offset: 0x0016638E
		public void TransferCopy(IntPtr cpuDataPtr, uint transferSize, uint destinationOffset = 0U)
		{
			this._gpuBuffer.TransferCopy(cpuDataPtr, transferSize, destinationOffset);
		}

		// Token: 0x04002CCF RID: 11471
		private static GLFunctions _gl;

		// Token: 0x04002CD0 RID: 11472
		private GPUBuffer _gpuBuffer;

		// Token: 0x04002CD1 RID: 11473
		private GL _storageFormat;

		// Token: 0x04002CD2 RID: 11474
		private GLTexture _bufferTextureCurrentRef;

		// Token: 0x04002CD3 RID: 11475
		private GLTexture _bufferTexturePing;

		// Token: 0x04002CD4 RID: 11476
		private GLTexture _bufferTexturePong;
	}
}
