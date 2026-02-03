using System;
using HytaleClient.Data.BlockyModels;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Map
{
	// Token: 0x02000A8E RID: 2702
	internal class AnimatedBlockRenderer : AnimatedRenderer
	{
		// Token: 0x17001303 RID: 4867
		// (get) Token: 0x06005522 RID: 21794 RVA: 0x00188603 File Offset: 0x00186803
		// (set) Token: 0x06005523 RID: 21795 RVA: 0x0018860B File Offset: 0x0018680B
		public GLVertexArray VertexArray { get; private set; }

		// Token: 0x06005524 RID: 21796 RVA: 0x00188614 File Offset: 0x00186814
		public AnimatedBlockRenderer(BlockyModel model, Point[] atlasSizes, ChunkGeometryData vertexData, GraphicsDevice graphics = null, bool selfManageNodeBuffer = false) : base(model, atlasSizes, selfManageNodeBuffer)
		{
			this._vertexData = vertexData;
			this.IndicesCount = this._vertexData.IndicesCount;
			bool flag = graphics != null;
			if (flag)
			{
				this.CreateGPUData(graphics);
			}
		}

		// Token: 0x06005525 RID: 21797 RVA: 0x00188658 File Offset: 0x00186858
		public unsafe override void CreateGPUData(GraphicsDevice graphics)
		{
			base.CreateGPUData(graphics);
			GLFunctions gl = graphics.GL;
			this.VertexArray = gl.GenVertexArray();
			gl.BindVertexArray(this.VertexArray);
			this._verticesBuffer = gl.GenBuffer();
			gl.BindBuffer(this.VertexArray, GL.ARRAY_BUFFER, this._verticesBuffer);
			ChunkVertex[] array;
			ChunkVertex* value;
			if ((array = this._vertexData.Vertices) == null || array.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array[0];
			}
			gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(this._vertexData.VerticesCount * ChunkVertex.Size), (IntPtr)((void*)value), GL.STATIC_DRAW);
			array = null;
			this._indicesBuffer = gl.GenBuffer();
			gl.BindBuffer(this.VertexArray, GL.ELEMENT_ARRAY_BUFFER, this._indicesBuffer);
			uint[] array2;
			uint* value2;
			if ((array2 = this._vertexData.Indices) == null || array2.Length == 0)
			{
				value2 = null;
			}
			else
			{
				value2 = &array2[0];
			}
			gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, (IntPtr)(this._vertexData.IndicesCount * 4), (IntPtr)((void*)value2), GL.STATIC_DRAW);
			array2 = null;
			this._vertexData = null;
			MapBlockAnimatedProgram mapBlockAnimatedProgram = graphics.GPUProgramStore.MapBlockAnimatedProgram;
			IntPtr pointer = IntPtr.Zero;
			gl.EnableVertexAttribArray(mapBlockAnimatedProgram.AttribPositionAndDoubleSidedAndBlockId.Index);
			gl.VertexAttribIPointer(mapBlockAnimatedProgram.AttribPositionAndDoubleSidedAndBlockId.Index, 4, GL.SHORT, ChunkVertex.Size, pointer);
			pointer += 8;
			gl.EnableVertexAttribArray(mapBlockAnimatedProgram.AttribTexCoords.Index);
			gl.VertexAttribPointer(mapBlockAnimatedProgram.AttribTexCoords.Index, 4, GL.UNSIGNED_SHORT, true, ChunkVertex.Size, pointer);
			pointer += 8;
			gl.EnableVertexAttribArray(mapBlockAnimatedProgram.AttribDataPacked.Index);
			gl.VertexAttribIPointer(mapBlockAnimatedProgram.AttribDataPacked.Index, 4, GL.UNSIGNED_INT, ChunkVertex.Size, pointer);
			pointer += 16;
		}

		// Token: 0x06005526 RID: 21798 RVA: 0x00188860 File Offset: 0x00186A60
		protected override void DoDispose()
		{
			base.DoDispose();
			bool flag = this._graphics != null;
			if (flag)
			{
				GLFunctions gl = this._graphics.GL;
				gl.DeleteBuffer(this._verticesBuffer);
				gl.DeleteBuffer(this._indicesBuffer);
				gl.DeleteVertexArray(this.VertexArray);
			}
		}

		// Token: 0x040031B2 RID: 12722
		private ChunkGeometryData _vertexData;

		// Token: 0x040031B3 RID: 12723
		public readonly int IndicesCount;

		// Token: 0x040031B5 RID: 12725
		private GLBuffer _verticesBuffer;

		// Token: 0x040031B6 RID: 12726
		private GLBuffer _indicesBuffer;
	}
}
