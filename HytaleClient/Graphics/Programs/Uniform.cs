using System;
using System.Runtime.CompilerServices;
using HytaleClient.Math;
using NLog;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A85 RID: 2693
	internal struct Uniform
	{
		// Token: 0x060054E3 RID: 21731 RVA: 0x00187238 File Offset: 0x00185438
		public static void InitializeGL(GLFunctions gl)
		{
			Uniform._gl = gl;
		}

		// Token: 0x060054E4 RID: 21732 RVA: 0x00187240 File Offset: 0x00185440
		public static void ReleaseGL()
		{
			Uniform._gl = null;
		}

		// Token: 0x17001301 RID: 4865
		// (get) Token: 0x060054E5 RID: 21733 RVA: 0x00187248 File Offset: 0x00185448
		public bool IsValid
		{
			get
			{
				return this._location != -1;
			}
		}

		// Token: 0x060054E6 RID: 21734 RVA: 0x00187258 File Offset: 0x00185458
		public Uniform(int location, string name, GPUProgram program)
		{
			this._location = location;
			this.Name = name;
			this._program = program;
			this._value = null;
			this._failedAssertValueMessage = string.Concat(new string[]
			{
				"Unexpected value for uniform ",
				name,
				" in program ",
				this._program.GetType().Name,
				"!"
			});
		}

		// Token: 0x060054E7 RID: 21735 RVA: 0x001872C2 File Offset: 0x001854C2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetValue(int value)
		{
			this._program.AssertInUse();
			this._value = value;
			Uniform._gl.Uniform1i(this._location, value);
		}

		// Token: 0x060054E8 RID: 21736 RVA: 0x001872F4 File Offset: 0x001854F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetValue(int x, int y)
		{
			this._program.AssertInUse();
			Uniform._gl.Uniform2i(this._location, x, y);
		}

		// Token: 0x060054E9 RID: 21737 RVA: 0x0018731B File Offset: 0x0018551B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetValue(int x, int y, int z)
		{
			this._program.AssertInUse();
			Uniform._gl.Uniform3i(this._location, x, y, z);
		}

		// Token: 0x060054EA RID: 21738 RVA: 0x00187344 File Offset: 0x00185544
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void SetValue(int[] values, int count)
		{
			this._program.AssertInUse();
			this._value = values;
			fixed (int[] array = values)
			{
				int* value;
				if (values == null || array.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array[0];
				}
				Uniform._gl.Uniform1iv(this._location, count, (IntPtr)((void*)value));
			}
		}

		// Token: 0x060054EB RID: 21739 RVA: 0x001873A0 File Offset: 0x001855A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void SetValue(int[] values, int start, int count)
		{
			this._program.AssertInUse();
			this._value = values;
			fixed (int[] array = values)
			{
				int* value;
				if (values == null || array.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array[0];
				}
				Uniform._gl.Uniform1iv(this._location, count, IntPtr.Add((IntPtr)((void*)value), start * 4));
			}
		}

		// Token: 0x060054EC RID: 21740 RVA: 0x00187402 File Offset: 0x00185602
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetValue(float value)
		{
			this._program.AssertInUse();
			this._value = value;
			Uniform._gl.Uniform1f(this._location, value);
		}

		// Token: 0x060054ED RID: 21741 RVA: 0x00187434 File Offset: 0x00185634
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void SetValue(float[] values)
		{
			this._program.AssertInUse();
			this._value = values;
			fixed (float[] array = values)
			{
				float* value;
				if (values == null || array.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array[0];
				}
				Uniform._gl.Uniform1fv(this._location, values.Length, (IntPtr)((void*)value));
			}
		}

		// Token: 0x060054EE RID: 21742 RVA: 0x00187490 File Offset: 0x00185690
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetValue(Vector2 vector)
		{
			this._program.AssertInUse();
			this._value = vector;
			Uniform._gl.Uniform2f(this._location, vector.X, vector.Y);
		}

		// Token: 0x060054EF RID: 21743 RVA: 0x001874D0 File Offset: 0x001856D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void SetValue(Vector2[] vectors)
		{
			this._program.AssertInUse();
			this._value = vectors;
			fixed (Vector2[] array = vectors)
			{
				Vector2* value;
				if (vectors == null || array.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array[0];
				}
				Uniform._gl.Uniform2fv(this._location, vectors.Length, (IntPtr)((void*)value));
			}
		}

		// Token: 0x060054F0 RID: 21744 RVA: 0x0018752C File Offset: 0x0018572C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetValue(float x, float y)
		{
			this._program.AssertInUse();
			this._value = new Vector2(x, y);
			Uniform._gl.Uniform2f(this._location, x, y);
		}

		// Token: 0x060054F1 RID: 21745 RVA: 0x00187568 File Offset: 0x00185768
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetValue(Vector3 vector)
		{
			this._program.AssertInUse();
			this._value = vector;
			Uniform._gl.Uniform3f(this._location, vector.X, vector.Y, vector.Z);
		}

		// Token: 0x060054F2 RID: 21746 RVA: 0x001875B8 File Offset: 0x001857B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void SetValue(Vector3[] vectors)
		{
			this._program.AssertInUse();
			this._value = vectors;
			fixed (Vector3[] array = vectors)
			{
				Vector3* value;
				if (vectors == null || array.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array[0];
				}
				Uniform._gl.Uniform3fv(this._location, vectors.Length, (IntPtr)((void*)value));
			}
		}

		// Token: 0x060054F3 RID: 21747 RVA: 0x00187614 File Offset: 0x00185814
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void SetValue(Vector3[] vectors, int count)
		{
			this._program.AssertInUse();
			this._value = vectors;
			fixed (Vector3[] array = vectors)
			{
				Vector3* value;
				if (vectors == null || array.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array[0];
				}
				Uniform._gl.Uniform3fv(this._location, count, (IntPtr)((void*)value));
			}
		}

		// Token: 0x060054F4 RID: 21748 RVA: 0x00187670 File Offset: 0x00185870
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void SetValue(Vector3[] vectors, int start, int count)
		{
			this._program.AssertInUse();
			this._value = vectors;
			fixed (Vector3[] array = vectors)
			{
				Vector3* value;
				if (vectors == null || array.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array[0];
				}
				Uniform._gl.Uniform3fv(this._location, count, IntPtr.Add((IntPtr)((void*)value), start * sizeof(Vector3)));
			}
		}

		// Token: 0x060054F5 RID: 21749 RVA: 0x001876D7 File Offset: 0x001858D7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetValue(float x, float y, float z)
		{
			this._program.AssertInUse();
			this._value = new Vector3(x, y, z);
			Uniform._gl.Uniform3f(this._location, x, y, z);
		}

		// Token: 0x060054F6 RID: 21750 RVA: 0x00187714 File Offset: 0x00185914
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetValue(Vector4 vector)
		{
			this._program.AssertInUse();
			this._value = vector;
			Uniform._gl.Uniform4f(this._location, vector.X, vector.Y, vector.Z, vector.W);
		}

		// Token: 0x060054F7 RID: 21751 RVA: 0x00187768 File Offset: 0x00185968
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void SetValue(Vector4[] vectors)
		{
			this._program.AssertInUse();
			this._value = vectors;
			fixed (Vector4[] array = vectors)
			{
				Vector4* value;
				if (vectors == null || array.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array[0];
				}
				Uniform._gl.Uniform4fv(this._location, vectors.Length, (IntPtr)((void*)value));
			}
		}

		// Token: 0x060054F8 RID: 21752 RVA: 0x001877C4 File Offset: 0x001859C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void SetValue(Vector4[] vectors, int count)
		{
			this._program.AssertInUse();
			this._value = vectors;
			fixed (Vector4[] array = vectors)
			{
				Vector4* value;
				if (vectors == null || array.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array[0];
				}
				Uniform._gl.Uniform4fv(this._location, count, (IntPtr)((void*)value));
			}
		}

		// Token: 0x060054F9 RID: 21753 RVA: 0x00187820 File Offset: 0x00185A20
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void SetValue(Vector4[] vectors, int start, int count)
		{
			this._program.AssertInUse();
			this._value = vectors;
			fixed (Vector4[] array = vectors)
			{
				Vector4* value;
				if (vectors == null || array.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array[0];
				}
				Uniform._gl.Uniform4fv(this._location, count, IntPtr.Add((IntPtr)((void*)value), start * sizeof(Vector4)));
			}
		}

		// Token: 0x060054FA RID: 21754 RVA: 0x00187887 File Offset: 0x00185A87
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetValue(float x, float y, float z, float w)
		{
			this._program.AssertInUse();
			this._value = new Vector4(x, y, z, w);
			Uniform._gl.Uniform4f(this._location, x, y, z, w);
		}

		// Token: 0x060054FB RID: 21755 RVA: 0x001878C8 File Offset: 0x00185AC8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void SetValue(ref Matrix matrix)
		{
			this._program.AssertInUse();
			this._value = matrix;
			fixed (Matrix* ptr = &matrix)
			{
				Matrix* value = ptr;
				Uniform._gl.UniformMatrix4fv(this._location, 1, false, (IntPtr)((void*)value));
			}
		}

		// Token: 0x060054FC RID: 21756 RVA: 0x0018791C File Offset: 0x00185B1C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void SetValue(Matrix[] matrices)
		{
			this._program.AssertInUse();
			this._value = matrices;
			fixed (Matrix[] array = matrices)
			{
				Matrix* value;
				if (matrices == null || array.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array[0];
				}
				Uniform._gl.UniformMatrix4fv(this._location, matrices.Length, false, (IntPtr)((void*)value));
			}
		}

		// Token: 0x060054FD RID: 21757 RVA: 0x00187979 File Offset: 0x00185B79
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetValue(IntPtr matrix, int matrixCount)
		{
			this._program.AssertInUse();
			this._value = new Tuple<IntPtr, int>(matrix, matrixCount);
			Uniform._gl.UniformMatrix4fv(this._location, matrixCount, false, matrix);
		}

		// Token: 0x060054FE RID: 21758 RVA: 0x001879AE File Offset: 0x00185BAE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Reset()
		{
			this._value = null;
		}

		// Token: 0x060054FF RID: 21759 RVA: 0x001879B8 File Offset: 0x00185BB8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssertValue(int value)
		{
			int? num = this._value as int?;
			bool flag = !(num.GetValueOrDefault() == value & num != null);
			if (flag)
			{
				throw new Exception(this._failedAssertValueMessage);
			}
		}

		// Token: 0x06005500 RID: 21760 RVA: 0x00187A00 File Offset: 0x00185C00
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssertValue(float value)
		{
			float? num = this._value as float?;
			bool flag = !(num.GetValueOrDefault() == value & num != null);
			if (flag)
			{
				throw new Exception(this._failedAssertValueMessage);
			}
		}

		// Token: 0x06005501 RID: 21761 RVA: 0x00187A48 File Offset: 0x00185C48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssertValue(Vector2 vector)
		{
			bool flag = this._value as Vector2? != vector;
			if (flag)
			{
				throw new Exception(this._failedAssertValueMessage);
			}
		}

		// Token: 0x06005502 RID: 21762 RVA: 0x00187A94 File Offset: 0x00185C94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssertValue(Vector3 vector)
		{
			bool flag = this._value as Vector3? != vector;
			if (flag)
			{
				throw new Exception(this._failedAssertValueMessage);
			}
		}

		// Token: 0x06005503 RID: 21763 RVA: 0x00187AE0 File Offset: 0x00185CE0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssertValue(Vector4 vector)
		{
			bool flag = this._value as Vector4? != vector;
			if (flag)
			{
				throw new Exception(this._failedAssertValueMessage);
			}
		}

		// Token: 0x0400315B RID: 12635
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400315C RID: 12636
		private int _location;

		// Token: 0x0400315D RID: 12637
		private static GLFunctions _gl;

		// Token: 0x0400315E RID: 12638
		public string Name;

		// Token: 0x0400315F RID: 12639
		private GPUProgram _program;

		// Token: 0x04003160 RID: 12640
		private object _value;

		// Token: 0x04003161 RID: 12641
		private string _failedAssertValueMessage;
	}
}
