using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A3B RID: 2619
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 34)]
	internal struct GPUTimer
	{
		// Token: 0x06005221 RID: 21025 RVA: 0x00168ADD File Offset: 0x00166CDD
		public static void InitializeGL(GLFunctions gl)
		{
			GPUTimer._gl = gl;
		}

		// Token: 0x06005222 RID: 21026 RVA: 0x00168AE5 File Offset: 0x00166CE5
		public static void ReleaseGL()
		{
			GPUTimer._gl = null;
		}

		// Token: 0x170012B7 RID: 4791
		// (get) Token: 0x06005223 RID: 21027 RVA: 0x00168AED File Offset: 0x00166CED
		public ulong ElapsedTime
		{
			get
			{
				return this._time;
			}
		}

		// Token: 0x170012B8 RID: 4792
		// (get) Token: 0x06005224 RID: 21028 RVA: 0x00168AF5 File Offset: 0x00166CF5
		public double ElapsedTimeInMilliseconds
		{
			get
			{
				return this._time / 1000000.0;
			}
		}

		// Token: 0x06005225 RID: 21029 RVA: 0x00168B0C File Offset: 0x00166D0C
		public unsafe void CreateStorage(bool useDoubleBuffering)
		{
			Debug.Assert(this._maxBuffering == 0, "GPUTimer storage is already created - or never destroyed.");
			this._maxBuffering = (useDoubleBuffering ? 2 : 3);
			this._current = 0;
			for (int i = 0; i < (int)this._maxBuffering; i++)
			{
				*(ref this._queries.FixedElementField + (IntPtr)(i * 2) * 4) = GPUTimer._gl.GenQuery();
				*(ref this._queries.FixedElementField + (IntPtr)(i * 2 + 1) * 4) = GPUTimer._gl.GenQuery();
			}
		}

		// Token: 0x06005226 RID: 21030 RVA: 0x00168BA0 File Offset: 0x00166DA0
		public unsafe void DestroyStorage()
		{
			Debug.Assert(this._maxBuffering > 0, "GPUTimer storage was never created - or already destroyed.");
			for (int i = 0; i < (int)this._maxBuffering; i++)
			{
				GPUTimer._gl.DeleteQuery(*(ref this._queries.FixedElementField + (IntPtr)(i * 2) * 4));
				GPUTimer._gl.DeleteQuery(*(ref this._queries.FixedElementField + (IntPtr)(i * 2 + 1) * 4));
			}
			this._maxBuffering = 0;
		}

		// Token: 0x06005227 RID: 21031 RVA: 0x00168C28 File Offset: 0x00166E28
		public void Swap()
		{
			Debug.Assert(this._maxBuffering > 0, "GPUTimer storage was never created.");
			this._current = (this._current + 1) % this._maxBuffering;
		}

		// Token: 0x06005228 RID: 21032 RVA: 0x00168C58 File Offset: 0x00166E58
		public unsafe void RequestStart()
		{
			Debug.Assert(this._maxBuffering > 0, "GPUTimer storage was never created.");
			GPUTimer._gl.QueryCounter(*(ref this._queries.FixedElementField + (IntPtr)(this._current * 2) * 4), GL.TIMESTAMP);
		}

		// Token: 0x06005229 RID: 21033 RVA: 0x00168CA8 File Offset: 0x00166EA8
		public unsafe void RequestStop()
		{
			Debug.Assert(this._maxBuffering > 0, "GPUTimer storage was never created.");
			GPUTimer._gl.QueryCounter(*(ref this._queries.FixedElementField + (IntPtr)(this._current * 2 + 1) * 4), GL.TIMESTAMP);
		}

		// Token: 0x0600522A RID: 21034 RVA: 0x00168CFC File Offset: 0x00166EFC
		public unsafe void FetchPreviousResultFromGPU()
		{
			Debug.Assert(this._maxBuffering > 0, "GPUTimer storage was never created.");
			byte b = (this._current + 1) % this._maxBuffering;
			ulong num;
			GPUTimer._gl.GetQueryObjectui64v(*(ref this._queries.FixedElementField + (IntPtr)(b * 2) * 4), GL.QUERY_RESULT, out num);
			ulong num2;
			GPUTimer._gl.GetQueryObjectui64v(*(ref this._queries.FixedElementField + (IntPtr)(b * 2 + 1) * 4), GL.QUERY_RESULT, out num2);
			this._time = num2 - num;
		}

		// Token: 0x04002D22 RID: 11554
		private static GLFunctions _gl;

		// Token: 0x04002D23 RID: 11555
		private byte _maxBuffering;

		// Token: 0x04002D24 RID: 11556
		private byte _current;

		// Token: 0x04002D25 RID: 11557
		private ulong _time;

		// Token: 0x04002D26 RID: 11558
		[FixedBuffer(typeof(uint), 6)]
		private GPUTimer.<_queries>e__FixedBuffer _queries;

		// Token: 0x02000EA7 RID: 3751
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 24)]
		public struct <_queries>e__FixedBuffer
		{
			// Token: 0x0400477C RID: 18300
			public uint FixedElementField;
		}
	}
}
