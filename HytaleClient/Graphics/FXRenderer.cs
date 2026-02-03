using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using HytaleClient.Core;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;

namespace HytaleClient.Graphics
{
	// Token: 0x020009A4 RID: 2468
	internal class FXRenderer : Disposable
	{
		// Token: 0x06004F3C RID: 20284 RVA: 0x001653AD File Offset: 0x001635AD
		public FXRenderer(GraphicsDevice graphics)
		{
			this._graphics = graphics;
		}

		// Token: 0x06004F3D RID: 20285 RVA: 0x001653CF File Offset: 0x001635CF
		public void Initialize(int maxParticleCount, int particleMaxDrawCount)
		{
			this._maxFXDrawCount = particleMaxDrawCount;
			this.InitMemory(maxParticleCount);
			this.CreateGPUData(particleMaxDrawCount);
		}

		// Token: 0x06004F3E RID: 20286 RVA: 0x001653E9 File Offset: 0x001635E9
		protected override void DoDispose()
		{
			this.DestroyGPUData();
		}

		// Token: 0x06004F3F RID: 20287 RVA: 0x001653F3 File Offset: 0x001635F3
		private void InitMemory(int itemMaxCount)
		{
			this.FXVertexBuffer = default(FXVertexBuffer);
			this.FXVertexBuffer.Initialize(itemMaxCount);
		}

		// Token: 0x06004F40 RID: 20288 RVA: 0x0016540F File Offset: 0x0016360F
		private void CreateGPUData(int particleMaxCount)
		{
			this.InitFXGPUData(particleMaxCount);
			this.InitDrawDataBufferTexture();
		}

		// Token: 0x06004F41 RID: 20289 RVA: 0x00165421 File Offset: 0x00163621
		private void DestroyGPUData()
		{
			this.DisposeDrawDataBufferTexture();
			this.DisposeFXGPUData();
		}

		// Token: 0x06004F42 RID: 20290 RVA: 0x00165434 File Offset: 0x00163634
		private void InitFXGPUData(int maxParticleDrawCount)
		{
			this._gpuData = new FXRenderer.FXGPUData[2];
			this._gpuData[0].CreateGPUData(this._graphics, maxParticleDrawCount);
			this._gpuData[1].CreateGPUData(this._graphics, maxParticleDrawCount);
		}

		// Token: 0x06004F43 RID: 20291 RVA: 0x00165480 File Offset: 0x00163680
		private void DisposeFXGPUData()
		{
			this._gpuData[0].Dispose(this._graphics.GL);
			this._gpuData[1].Dispose(this._graphics.GL);
		}

		// Token: 0x06004F44 RID: 20292 RVA: 0x001654BD File Offset: 0x001636BD
		private void InitDrawDataBufferTexture()
		{
			this._drawDataBufferTexture.CreateStorage(GL.RGBA32F, GL.STREAM_DRAW, true, this._bufferSize, 1024U, GPUBuffer.GrowthPolicy.GrowthAutoNoLimit, 0U);
		}

		// Token: 0x06004F45 RID: 20293 RVA: 0x001654E4 File Offset: 0x001636E4
		public bool TryBeginDrawDataTransfer(out IntPtr dataPtr)
		{
			dataPtr = IntPtr.Zero;
			uint num = (uint)((int)this._drawTaskCount * FXRenderer.DrawDataSize);
			bool flag = num == 0U;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this._drawDataBufferTexture.GrowStorageIfNecessary(num);
				dataPtr = this._drawDataBufferTexture.BeginTransfer(num);
				result = true;
			}
			return result;
		}

		// Token: 0x06004F46 RID: 20294 RVA: 0x00165532 File Offset: 0x00163732
		public void EndDrawDataTransfer()
		{
			this._drawDataBufferTexture.EndTransfer();
		}

		// Token: 0x06004F47 RID: 20295 RVA: 0x00165541 File Offset: 0x00163741
		private void DisposeDrawDataBufferTexture()
		{
			this._drawDataBufferTexture.DestroyStorage();
		}

		// Token: 0x06004F48 RID: 20296 RVA: 0x00165550 File Offset: 0x00163750
		public void SetupDrawDataTexture(uint unitId)
		{
			GLFunctions gl = this._graphics.GL;
			gl.ActiveTexture(GL.TEXTURE0 + unitId);
			gl.BindTexture(GL.TEXTURE_BUFFER, this._drawDataBufferTexture.CurrentTexture);
		}

		// Token: 0x06004F49 RID: 20297 RVA: 0x0016558F File Offset: 0x0016378F
		public void BeginFrame()
		{
			this.ResetDrawCounters();
			this.PingPongBuffers();
		}

		// Token: 0x06004F4A RID: 20298 RVA: 0x001655A0 File Offset: 0x001637A0
		private void ResetDrawCounters()
		{
			this._drawTaskCount = 0;
			this.ErosionDrawParams.Count = 0;
			this.BlendLowResDrawParams.Count = 0;
			this.BlendDrawParams.Count = 0;
			this.BlendDrawParams.IsStartOffsetSet = false;
			this.BlendFPVDrawParams.Count = 0;
			this.BlendFPVDrawParams.IsStartOffsetSet = false;
			this.DistortionDrawParams.Count = 0;
			this.DistortionDrawParams.IsStartOffsetSet = false;
		}

		// Token: 0x06004F4B RID: 20299 RVA: 0x00165618 File Offset: 0x00163818
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ushort ReserveDrawTask()
		{
			ushort drawTaskCount = this._drawTaskCount;
			this._drawTaskCount += 1;
			return drawTaskCount;
		}

		// Token: 0x06004F4C RID: 20300 RVA: 0x00165641 File Offset: 0x00163841
		public void ClearVertexData()
		{
			this.FXVertexBuffer.ClearVertexDataStorage();
		}

		// Token: 0x06004F4D RID: 20301 RVA: 0x00165650 File Offset: 0x00163850
		public unsafe void SendVertexDataToGPU()
		{
			GLFunctions gl = this._graphics.GL;
			int num = Math.Min(this._maxFXDrawCount, this.FXVertexBuffer.GetVertexDataStored());
			bool flag = num > 0;
			if (flag)
			{
				this._currentGPUDataId++;
				this._currentGPUDataId %= 2;
				gl.BindBuffer(GL.ARRAY_BUFFER, this._gpuData[this._currentGPUDataId].VerticesBuffer);
				FXVertex[] array;
				FXVertex* value;
				if ((array = this.FXVertexBuffer.ParticleVertices) == null || array.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array[0];
				}
				gl.BufferSubData(GL.ARRAY_BUFFER, IntPtr.Zero, (IntPtr)(num * 4 * FXVertex.Size), (IntPtr)((void*)value));
				array = null;
			}
		}

		// Token: 0x06004F4E RID: 20302 RVA: 0x00165720 File Offset: 0x00163920
		public void DrawTransparency()
		{
			bool flag = this.BlendDrawParams.Count > 0 || this.BlendFPVDrawParams.Count > 0;
			if (flag)
			{
				GLFunctions gl = this._graphics.GL;
				gl.BindVertexArray(this._gpuData[this._currentGPUDataId].VertexArray);
				bool flag2 = this.BlendDrawParams.Count > 0;
				if (flag2)
				{
					gl.DrawElements(GL.TRIANGLES, this.BlendDrawParams.Count * 6, GL.UNSIGNED_INT, (IntPtr)((long)((ulong)(this.BlendDrawParams.StartOffset * 6U * 4U))));
				}
				bool flag3 = this.BlendFPVDrawParams.Count > 0;
				if (flag3)
				{
					gl.DrawElements(GL.TRIANGLES, this.BlendFPVDrawParams.Count * 6, GL.UNSIGNED_INT, (IntPtr)((long)((ulong)(this.BlendFPVDrawParams.StartOffset * 6U * 4U))));
				}
			}
		}

		// Token: 0x06004F4F RID: 20303 RVA: 0x00165808 File Offset: 0x00163A08
		public void DrawTransparencyLowRes()
		{
			bool flag = this.BlendLowResDrawParams.Count > 0;
			if (flag)
			{
				GLFunctions gl = this._graphics.GL;
				gl.BindVertexArray(this._gpuData[this._currentGPUDataId].VertexArray);
				gl.DrawElements(GL.TRIANGLES, this.BlendLowResDrawParams.Count * 6, GL.UNSIGNED_INT, (IntPtr)((long)((ulong)(this.BlendLowResDrawParams.StartOffset * 6U * 4U))));
			}
		}

		// Token: 0x06004F50 RID: 20304 RVA: 0x00165884 File Offset: 0x00163A84
		public void DrawDistortion()
		{
			bool flag = this.DistortionDrawParams.Count > 0;
			if (flag)
			{
				GLFunctions gl = this._graphics.GL;
				gl.BindVertexArray(this._gpuData[this._currentGPUDataId].VertexArray);
				gl.DrawElements(GL.TRIANGLES, this.DistortionDrawParams.Count * 6, GL.UNSIGNED_INT, (IntPtr)((long)((ulong)(this.DistortionDrawParams.StartOffset * 6U * 4U))));
			}
		}

		// Token: 0x06004F51 RID: 20305 RVA: 0x00165900 File Offset: 0x00163B00
		public void DrawErosion()
		{
			bool flag = this.ErosionDrawParams.Count > 0;
			if (flag)
			{
				GLFunctions gl = this._graphics.GL;
				gl.BindVertexArray(this._gpuData[this._currentGPUDataId].VertexArray);
				gl.DrawElements(GL.TRIANGLES, this.ErosionDrawParams.Count * 6, GL.UNSIGNED_INT, (IntPtr)((long)((ulong)(this.ErosionDrawParams.StartOffset * 6U * 4U))));
			}
		}

		// Token: 0x06004F52 RID: 20306 RVA: 0x0016597A File Offset: 0x00163B7A
		private void PingPongBuffers()
		{
			this._drawDataBufferTexture.Swap();
		}

		// Token: 0x04002A79 RID: 10873
		public FXRenderer.DrawParams ErosionDrawParams;

		// Token: 0x04002A7A RID: 10874
		public FXRenderer.DrawParams BlendLowResDrawParams;

		// Token: 0x04002A7B RID: 10875
		public FXRenderer.DrawParams BlendDrawParams;

		// Token: 0x04002A7C RID: 10876
		public FXRenderer.DrawParams BlendFPVDrawParams;

		// Token: 0x04002A7D RID: 10877
		public FXRenderer.DrawParams DistortionDrawParams;

		// Token: 0x04002A7E RID: 10878
		public FXVertexBuffer FXVertexBuffer;

		// Token: 0x04002A7F RID: 10879
		public static readonly int DrawDataSize = Marshal.SizeOf(typeof(Matrix)) + Marshal.SizeOf(typeof(Vector4)) * 8;

		// Token: 0x04002A80 RID: 10880
		private readonly GraphicsDevice _graphics;

		// Token: 0x04002A81 RID: 10881
		private FXRenderer.FXGPUData[] _gpuData;

		// Token: 0x04002A82 RID: 10882
		private GPUBufferTexture _drawDataBufferTexture;

		// Token: 0x04002A83 RID: 10883
		private const uint BufferGrowth = 1024U;

		// Token: 0x04002A84 RID: 10884
		private uint _bufferSize = (uint)(FXRenderer.DrawDataSize * 2048);

		// Token: 0x04002A85 RID: 10885
		private int _currentGPUDataId;

		// Token: 0x04002A86 RID: 10886
		private ushort _drawTaskCount;

		// Token: 0x04002A87 RID: 10887
		private int _maxFXDrawCount;

		// Token: 0x02000EA1 RID: 3745
		public enum DrawType
		{
			// Token: 0x0400476A RID: 18282
			Particle,
			// Token: 0x0400476B RID: 18283
			Trail
		}

		// Token: 0x02000EA2 RID: 3746
		public struct FXGPUData
		{
			// Token: 0x060067E4 RID: 26596 RVA: 0x00219044 File Offset: 0x00217244
			public unsafe void CreateGPUData(GraphicsDevice graphics, int maxParticles)
			{
				bool flag = this.VertexArray != GLVertexArray.None;
				if (flag)
				{
					throw new Exception("Error : GPUData.CreateGPUData() must be called once only.");
				}
				GLFunctions gl = graphics.GL;
				int num = 6 * maxParticles;
				uint[] array = new uint[num];
				for (int i = 0; i < maxParticles; i++)
				{
					array[i * 6] = (uint)(i * 4);
					array[i * 6 + 1] = (uint)(i * 4 + 1);
					array[i * 6 + 2] = (uint)(i * 4 + 2);
					array[i * 6 + 3] = (uint)(i * 4);
					array[i * 6 + 4] = (uint)(i * 4 + 2);
					array[i * 6 + 5] = (uint)(i * 4 + 3);
				}
				this.VertexArray = gl.GenVertexArray();
				gl.BindVertexArray(this.VertexArray);
				this.VerticesBuffer = gl.GenBuffer();
				gl.BindBuffer(this.VertexArray, GL.ARRAY_BUFFER, this.VerticesBuffer);
				gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(4 * maxParticles * FXVertex.Size), IntPtr.Zero, GL.DYNAMIC_DRAW);
				this.IndicesBuffer = gl.GenBuffer();
				gl.BindBuffer(this.VertexArray, GL.ELEMENT_ARRAY_BUFFER, this.IndicesBuffer);
				uint[] array2;
				uint* value;
				if ((array2 = array) == null || array2.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array2[0];
				}
				gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, (IntPtr)(num * 4), (IntPtr)((void*)value), GL.STATIC_DRAW);
				array2 = null;
				this.SetupVertexAttributes(graphics);
			}

			// Token: 0x060067E5 RID: 26597 RVA: 0x002191C4 File Offset: 0x002173C4
			public void Dispose(GLFunctions gl)
			{
				bool flag = this.VertexArray == GLVertexArray.None;
				if (flag)
				{
					throw new Exception("Error : GPUData.Dispose() was already called (or GPUData was never created ?).");
				}
				gl.DeleteVertexArray(this.VertexArray);
				gl.DeleteBuffer(this.VerticesBuffer);
				gl.DeleteBuffer(this.IndicesBuffer);
			}

			// Token: 0x060067E6 RID: 26598 RVA: 0x00219218 File Offset: 0x00217418
			private void SetupVertexAttributes(GraphicsDevice graphics)
			{
				GLFunctions gl = graphics.GL;
				ParticleProgram particleProgram = graphics.GPUProgramStore.ParticleProgram;
				IntPtr pointer = IntPtr.Zero;
				gl.EnableVertexAttribArray(particleProgram.AttribData1.Index);
				gl.VertexAttribPointer(particleProgram.AttribData1.Index, 4, GL.FLOAT, false, FXVertex.Size, pointer);
				pointer += 16;
				gl.EnableVertexAttribArray(particleProgram.AttribData2.Index);
				gl.VertexAttribPointer(particleProgram.AttribData2.Index, 4, GL.FLOAT, false, FXVertex.Size, pointer);
				pointer += 16;
				gl.EnableVertexAttribArray(particleProgram.AttribData3.Index);
				gl.VertexAttribPointer(particleProgram.AttribData3.Index, 4, GL.FLOAT, false, FXVertex.Size, pointer);
				pointer += 16;
				gl.EnableVertexAttribArray(particleProgram.AttribData4.Index);
				gl.VertexAttribPointer(particleProgram.AttribData4.Index, 4, GL.FLOAT, false, FXVertex.Size, pointer);
				pointer += 16;
			}

			// Token: 0x0400476C RID: 18284
			public GLVertexArray VertexArray;

			// Token: 0x0400476D RID: 18285
			public GLBuffer VerticesBuffer;

			// Token: 0x0400476E RID: 18286
			public GLBuffer IndicesBuffer;
		}

		// Token: 0x02000EA3 RID: 3747
		public struct DrawParams
		{
			// Token: 0x0400476F RID: 18287
			public bool IsStartOffsetSet;

			// Token: 0x04004770 RID: 18288
			public uint StartOffset;

			// Token: 0x04004771 RID: 18289
			public int Count;
		}
	}
}
