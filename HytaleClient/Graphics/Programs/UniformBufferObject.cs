using System;
using System.Runtime.CompilerServices;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A86 RID: 2694
	internal struct UniformBufferObject
	{
		// Token: 0x06005505 RID: 21765 RVA: 0x00187B36 File Offset: 0x00185D36
		public static void InitializeGL(GLFunctions gl)
		{
			UniformBufferObject._gl = gl;
		}

		// Token: 0x06005506 RID: 21766 RVA: 0x00187B3E File Offset: 0x00185D3E
		public static void ReleaseGL()
		{
			UniformBufferObject._gl = null;
		}

		// Token: 0x06005507 RID: 21767 RVA: 0x00187B46 File Offset: 0x00185D46
		public UniformBufferObject(GPUProgram program, uint blockIndex, string name)
		{
			this._blockIndex = blockIndex;
			this._bindingPointIndex = uint.MaxValue;
			this.Name = name;
		}

		// Token: 0x06005508 RID: 21768 RVA: 0x00187B60 File Offset: 0x00185D60
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetupBindingPoint(GPUProgram program, uint bindingPointIndex)
		{
			this._bindingPointIndex = bindingPointIndex;
			bool flag = this._blockIndex != uint.MaxValue;
			if (flag)
			{
				UniformBufferObject._gl.UniformBlockBinding(program.ProgramId, this._blockIndex, bindingPointIndex);
			}
		}

		// Token: 0x06005509 RID: 21769 RVA: 0x00187BA2 File Offset: 0x00185DA2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetBuffer(GLBuffer buffer)
		{
			UniformBufferObject._gl.BindBufferBase(GL.UNIFORM_BUFFER, this._bindingPointIndex, buffer.InternalId);
		}

		// Token: 0x0600550A RID: 21770 RVA: 0x00187BC6 File Offset: 0x00185DC6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetBufferRange(GLBuffer buffer, uint offset, uint count)
		{
			UniformBufferObject._gl.BindBufferRange(GL.UNIFORM_BUFFER, this._bindingPointIndex, buffer.InternalId, (IntPtr)((long)((ulong)offset)), (IntPtr)((long)((ulong)count)));
		}

		// Token: 0x04003162 RID: 12642
		private uint _blockIndex;

		// Token: 0x04003163 RID: 12643
		private uint _bindingPointIndex;

		// Token: 0x04003164 RID: 12644
		public string Name;

		// Token: 0x04003165 RID: 12645
		private static GLFunctions _gl;
	}
}
