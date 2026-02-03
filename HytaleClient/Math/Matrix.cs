using System;
using System.Diagnostics;

namespace HytaleClient.Math
{
	// Token: 0x020007E8 RID: 2024
	[DebuggerDisplay("{DebugDisplayString,nq}")]
	[Serializable]
	public struct Matrix : IEquatable<Matrix>
	{
		// Token: 0x17000FC4 RID: 4036
		// (get) Token: 0x0600362C RID: 13868 RVA: 0x00065B4C File Offset: 0x00063D4C
		// (set) Token: 0x0600362D RID: 13869 RVA: 0x00065B75 File Offset: 0x00063D75
		public Vector3 Backward
		{
			get
			{
				return new Vector3(this.M31, this.M32, this.M33);
			}
			set
			{
				this.M31 = value.X;
				this.M32 = value.Y;
				this.M33 = value.Z;
			}
		}

		// Token: 0x17000FC5 RID: 4037
		// (get) Token: 0x0600362E RID: 13870 RVA: 0x00065B9C File Offset: 0x00063D9C
		// (set) Token: 0x0600362F RID: 13871 RVA: 0x00065BC8 File Offset: 0x00063DC8
		public Vector3 Down
		{
			get
			{
				return new Vector3(-this.M21, -this.M22, -this.M23);
			}
			set
			{
				this.M21 = -value.X;
				this.M22 = -value.Y;
				this.M23 = -value.Z;
			}
		}

		// Token: 0x17000FC6 RID: 4038
		// (get) Token: 0x06003630 RID: 13872 RVA: 0x00065BF4 File Offset: 0x00063DF4
		// (set) Token: 0x06003631 RID: 13873 RVA: 0x00065C20 File Offset: 0x00063E20
		public Vector3 Forward
		{
			get
			{
				return new Vector3(-this.M31, -this.M32, -this.M33);
			}
			set
			{
				this.M31 = -value.X;
				this.M32 = -value.Y;
				this.M33 = -value.Z;
			}
		}

		// Token: 0x17000FC7 RID: 4039
		// (get) Token: 0x06003632 RID: 13874 RVA: 0x00065C4C File Offset: 0x00063E4C
		public static Matrix Identity
		{
			get
			{
				return Matrix.identity;
			}
		}

		// Token: 0x17000FC8 RID: 4040
		// (get) Token: 0x06003633 RID: 13875 RVA: 0x00065C64 File Offset: 0x00063E64
		// (set) Token: 0x06003634 RID: 13876 RVA: 0x00065C90 File Offset: 0x00063E90
		public Vector3 Left
		{
			get
			{
				return new Vector3(-this.M11, -this.M12, -this.M13);
			}
			set
			{
				this.M11 = -value.X;
				this.M12 = -value.Y;
				this.M13 = -value.Z;
			}
		}

		// Token: 0x17000FC9 RID: 4041
		// (get) Token: 0x06003635 RID: 13877 RVA: 0x00065CBC File Offset: 0x00063EBC
		// (set) Token: 0x06003636 RID: 13878 RVA: 0x00065CE5 File Offset: 0x00063EE5
		public Vector3 Right
		{
			get
			{
				return new Vector3(this.M11, this.M12, this.M13);
			}
			set
			{
				this.M11 = value.X;
				this.M12 = value.Y;
				this.M13 = value.Z;
			}
		}

		// Token: 0x17000FCA RID: 4042
		// (get) Token: 0x06003637 RID: 13879 RVA: 0x00065D0C File Offset: 0x00063F0C
		// (set) Token: 0x06003638 RID: 13880 RVA: 0x00065D35 File Offset: 0x00063F35
		public Vector3 Translation
		{
			get
			{
				return new Vector3(this.M41, this.M42, this.M43);
			}
			set
			{
				this.M41 = value.X;
				this.M42 = value.Y;
				this.M43 = value.Z;
			}
		}

		// Token: 0x17000FCB RID: 4043
		// (get) Token: 0x06003639 RID: 13881 RVA: 0x00065D5C File Offset: 0x00063F5C
		// (set) Token: 0x0600363A RID: 13882 RVA: 0x00065D85 File Offset: 0x00063F85
		public Vector3 Up
		{
			get
			{
				return new Vector3(this.M21, this.M22, this.M23);
			}
			set
			{
				this.M21 = value.X;
				this.M22 = value.Y;
				this.M23 = value.Z;
			}
		}

		// Token: 0x17000FCC RID: 4044
		// (get) Token: 0x0600363B RID: 13883 RVA: 0x00065DAC File Offset: 0x00063FAC
		internal string DebugDisplayString
		{
			get
			{
				return string.Concat(new string[]
				{
					"( ",
					this.M11.ToString(),
					" ",
					this.M12.ToString(),
					" ",
					this.M13.ToString(),
					" ",
					this.M14.ToString(),
					" ) \r\n",
					"( ",
					this.M21.ToString(),
					" ",
					this.M22.ToString(),
					" ",
					this.M23.ToString(),
					" ",
					this.M24.ToString(),
					" ) \r\n",
					"( ",
					this.M31.ToString(),
					" ",
					this.M32.ToString(),
					" ",
					this.M33.ToString(),
					" ",
					this.M34.ToString(),
					" ) \r\n",
					"( ",
					this.M41.ToString(),
					" ",
					this.M42.ToString(),
					" ",
					this.M43.ToString(),
					" ",
					this.M44.ToString(),
					" )"
				});
			}
		}

		// Token: 0x0600363C RID: 13884 RVA: 0x00065F68 File Offset: 0x00064168
		public Matrix(float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31, float m32, float m33, float m34, float m41, float m42, float m43, float m44)
		{
			this.M11 = m11;
			this.M12 = m12;
			this.M13 = m13;
			this.M14 = m14;
			this.M21 = m21;
			this.M22 = m22;
			this.M23 = m23;
			this.M24 = m24;
			this.M31 = m31;
			this.M32 = m32;
			this.M33 = m33;
			this.M34 = m34;
			this.M41 = m41;
			this.M42 = m42;
			this.M43 = m43;
			this.M44 = m44;
		}

		// Token: 0x0600363D RID: 13885 RVA: 0x00065FF4 File Offset: 0x000641F4
		public bool Decompose(out Vector3 scale, out Quaternion rotation, out Vector3 translation)
		{
			translation.X = this.M41;
			translation.Y = this.M42;
			translation.Z = this.M43;
			float num = (float)((Math.Sign(this.M11 * this.M12 * this.M13 * this.M14) < 0) ? -1 : 1);
			float num2 = (float)((Math.Sign(this.M21 * this.M22 * this.M23 * this.M24) < 0) ? -1 : 1);
			float num3 = (float)((Math.Sign(this.M31 * this.M32 * this.M33 * this.M34) < 0) ? -1 : 1);
			scale.X = num * (float)Math.Sqrt((double)(this.M11 * this.M11 + this.M12 * this.M12 + this.M13 * this.M13));
			scale.Y = num2 * (float)Math.Sqrt((double)(this.M21 * this.M21 + this.M22 * this.M22 + this.M23 * this.M23));
			scale.Z = num3 * (float)Math.Sqrt((double)(this.M31 * this.M31 + this.M32 * this.M32 + this.M33 * this.M33));
			bool flag = MathHelper.WithinEpsilon(scale.X, 0f) || MathHelper.WithinEpsilon(scale.Y, 0f) || MathHelper.WithinEpsilon(scale.Z, 0f);
			bool result;
			if (flag)
			{
				rotation = Quaternion.Identity;
				result = false;
			}
			else
			{
				Matrix matrix = new Matrix(this.M11 / scale.X, this.M12 / scale.X, this.M13 / scale.X, 0f, this.M21 / scale.Y, this.M22 / scale.Y, this.M23 / scale.Y, 0f, this.M31 / scale.Z, this.M32 / scale.Z, this.M33 / scale.Z, 0f, 0f, 0f, 0f, 1f);
				rotation = Quaternion.CreateFromRotationMatrix(matrix);
				result = true;
			}
			return result;
		}

		// Token: 0x0600363E RID: 13886 RVA: 0x0006624C File Offset: 0x0006444C
		public float Determinant()
		{
			float num = this.M33 * this.M44 - this.M34 * this.M43;
			float num2 = this.M32 * this.M44 - this.M34 * this.M42;
			float num3 = this.M32 * this.M43 - this.M33 * this.M42;
			float num4 = this.M31 * this.M44 - this.M34 * this.M41;
			float num5 = this.M31 * this.M43 - this.M33 * this.M41;
			float num6 = this.M31 * this.M42 - this.M32 * this.M41;
			return this.M11 * (this.M22 * num - this.M23 * num2 + this.M24 * num3) - this.M12 * (this.M21 * num - this.M23 * num4 + this.M24 * num5) + this.M13 * (this.M21 * num2 - this.M22 * num4 + this.M24 * num6) - this.M14 * (this.M21 * num3 - this.M22 * num5 + this.M23 * num6);
		}

		// Token: 0x0600363F RID: 13887 RVA: 0x00066398 File Offset: 0x00064598
		public bool Equals(Matrix other)
		{
			return this.M11 == other.M11 && this.M12 == other.M12 && this.M13 == other.M13 && this.M14 == other.M14 && this.M21 == other.M21 && this.M22 == other.M22 && this.M23 == other.M23 && this.M24 == other.M24 && this.M31 == other.M31 && this.M32 == other.M32 && this.M33 == other.M33 && this.M34 == other.M34 && this.M41 == other.M41 && this.M42 == other.M42 && this.M43 == other.M43 && this.M44 == other.M44;
		}

		// Token: 0x06003640 RID: 13888 RVA: 0x000664A4 File Offset: 0x000646A4
		public override bool Equals(object obj)
		{
			return obj is Matrix && this.Equals((Matrix)obj);
		}

		// Token: 0x06003641 RID: 13889 RVA: 0x000664D0 File Offset: 0x000646D0
		public override int GetHashCode()
		{
			return this.M11.GetHashCode() + this.M12.GetHashCode() + this.M13.GetHashCode() + this.M14.GetHashCode() + this.M21.GetHashCode() + this.M22.GetHashCode() + this.M23.GetHashCode() + this.M24.GetHashCode() + this.M31.GetHashCode() + this.M32.GetHashCode() + this.M33.GetHashCode() + this.M34.GetHashCode() + this.M41.GetHashCode() + this.M42.GetHashCode() + this.M43.GetHashCode() + this.M44.GetHashCode();
		}

		// Token: 0x06003642 RID: 13890 RVA: 0x000665A4 File Offset: 0x000647A4
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{M11:",
				this.M11.ToString(),
				" M12:",
				this.M12.ToString(),
				" M13:",
				this.M13.ToString(),
				" M14:",
				this.M14.ToString(),
				"} {M21:",
				this.M21.ToString(),
				" M22:",
				this.M22.ToString(),
				" M23:",
				this.M23.ToString(),
				" M24:",
				this.M24.ToString(),
				"} {M31:",
				this.M31.ToString(),
				" M32:",
				this.M32.ToString(),
				" M33:",
				this.M33.ToString(),
				" M34:",
				this.M34.ToString(),
				"} {M41:",
				this.M41.ToString(),
				" M42:",
				this.M42.ToString(),
				" M43:",
				this.M43.ToString(),
				" M44:",
				this.M44.ToString(),
				"}"
			});
		}

		// Token: 0x06003643 RID: 13891 RVA: 0x00066744 File Offset: 0x00064944
		public static Matrix Add(Matrix matrix1, Matrix matrix2)
		{
			matrix1.M11 += matrix2.M11;
			matrix1.M12 += matrix2.M12;
			matrix1.M13 += matrix2.M13;
			matrix1.M14 += matrix2.M14;
			matrix1.M21 += matrix2.M21;
			matrix1.M22 += matrix2.M22;
			matrix1.M23 += matrix2.M23;
			matrix1.M24 += matrix2.M24;
			matrix1.M31 += matrix2.M31;
			matrix1.M32 += matrix2.M32;
			matrix1.M33 += matrix2.M33;
			matrix1.M34 += matrix2.M34;
			matrix1.M41 += matrix2.M41;
			matrix1.M42 += matrix2.M42;
			matrix1.M43 += matrix2.M43;
			matrix1.M44 += matrix2.M44;
			return matrix1;
		}

		// Token: 0x06003644 RID: 13892 RVA: 0x00066868 File Offset: 0x00064A68
		public static void Add(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
		{
			result.M11 = matrix1.M11 + matrix2.M11;
			result.M12 = matrix1.M12 + matrix2.M12;
			result.M13 = matrix1.M13 + matrix2.M13;
			result.M14 = matrix1.M14 + matrix2.M14;
			result.M21 = matrix1.M21 + matrix2.M21;
			result.M22 = matrix1.M22 + matrix2.M22;
			result.M23 = matrix1.M23 + matrix2.M23;
			result.M24 = matrix1.M24 + matrix2.M24;
			result.M31 = matrix1.M31 + matrix2.M31;
			result.M32 = matrix1.M32 + matrix2.M32;
			result.M33 = matrix1.M33 + matrix2.M33;
			result.M34 = matrix1.M34 + matrix2.M34;
			result.M41 = matrix1.M41 + matrix2.M41;
			result.M42 = matrix1.M42 + matrix2.M42;
			result.M43 = matrix1.M43 + matrix2.M43;
			result.M44 = matrix1.M44 + matrix2.M44;
		}

		// Token: 0x06003645 RID: 13893 RVA: 0x000669A8 File Offset: 0x00064BA8
		public static Matrix CreateBillboard(Vector3 objectPosition, Vector3 cameraPosition, Vector3 cameraUpVector, Vector3? cameraForwardVector)
		{
			Matrix result;
			Matrix.CreateBillboard(ref objectPosition, ref cameraPosition, ref cameraUpVector, cameraForwardVector, out result);
			return result;
		}

		// Token: 0x06003646 RID: 13894 RVA: 0x000669CC File Offset: 0x00064BCC
		public static void CreateBillboard(ref Vector3 objectPosition, ref Vector3 cameraPosition, ref Vector3 cameraUpVector, Vector3? cameraForwardVector, out Matrix result)
		{
			Vector3 vector;
			vector.X = objectPosition.X - cameraPosition.X;
			vector.Y = objectPosition.Y - cameraPosition.Y;
			vector.Z = objectPosition.Z - cameraPosition.Z;
			float num = vector.LengthSquared();
			bool flag = num < 0.0001f;
			if (flag)
			{
				vector = ((cameraForwardVector != null) ? (-cameraForwardVector.Value) : Vector3.Forward);
			}
			else
			{
				Vector3.Multiply(ref vector, 1f / (float)Math.Sqrt((double)num), out vector);
			}
			Vector3 vector2;
			Vector3.Cross(ref cameraUpVector, ref vector, out vector2);
			vector2.Normalize();
			Vector3 vector3;
			Vector3.Cross(ref vector, ref vector2, out vector3);
			result.M11 = vector2.X;
			result.M12 = vector2.Y;
			result.M13 = vector2.Z;
			result.M14 = 0f;
			result.M21 = vector3.X;
			result.M22 = vector3.Y;
			result.M23 = vector3.Z;
			result.M24 = 0f;
			result.M31 = vector.X;
			result.M32 = vector.Y;
			result.M33 = vector.Z;
			result.M34 = 0f;
			result.M41 = objectPosition.X;
			result.M42 = objectPosition.Y;
			result.M43 = objectPosition.Z;
			result.M44 = 1f;
		}

		// Token: 0x06003647 RID: 13895 RVA: 0x00066B54 File Offset: 0x00064D54
		public static Matrix CreateConstrainedBillboard(Vector3 objectPosition, Vector3 cameraPosition, Vector3 rotateAxis, Vector3? cameraForwardVector, Vector3? objectForwardVector)
		{
			Matrix result;
			Matrix.CreateConstrainedBillboard(ref objectPosition, ref cameraPosition, ref rotateAxis, cameraForwardVector, objectForwardVector, out result);
			return result;
		}

		// Token: 0x06003648 RID: 13896 RVA: 0x00066B78 File Offset: 0x00064D78
		public static void CreateConstrainedBillboard(ref Vector3 objectPosition, ref Vector3 cameraPosition, ref Vector3 rotateAxis, Vector3? cameraForwardVector, Vector3? objectForwardVector, out Matrix result)
		{
			Vector3 vector;
			vector.X = objectPosition.X - cameraPosition.X;
			vector.Y = objectPosition.Y - cameraPosition.Y;
			vector.Z = objectPosition.Z - cameraPosition.Z;
			float num = vector.LengthSquared();
			bool flag = num < 0.0001f;
			if (flag)
			{
				vector = ((cameraForwardVector != null) ? (-cameraForwardVector.Value) : Vector3.Forward);
			}
			else
			{
				Vector3.Multiply(ref vector, 1f / (float)Math.Sqrt((double)num), out vector);
			}
			Vector3 vector2 = rotateAxis;
			float value;
			Vector3.Dot(ref rotateAxis, ref vector, out value);
			bool flag2 = Math.Abs(value) > 0.9982547f;
			Vector3 vector3;
			Vector3 vector4;
			if (flag2)
			{
				bool flag3 = objectForwardVector != null;
				if (flag3)
				{
					vector3 = objectForwardVector.Value;
					Vector3.Dot(ref rotateAxis, ref vector3, out value);
					bool flag4 = Math.Abs(value) > 0.9982547f;
					if (flag4)
					{
						value = rotateAxis.X * Vector3.Forward.X + rotateAxis.Y * Vector3.Forward.Y + rotateAxis.Z * Vector3.Forward.Z;
						vector3 = ((Math.Abs(value) > 0.9982547f) ? Vector3.Right : Vector3.Forward);
					}
				}
				else
				{
					value = rotateAxis.X * Vector3.Forward.X + rotateAxis.Y * Vector3.Forward.Y + rotateAxis.Z * Vector3.Forward.Z;
					vector3 = ((Math.Abs(value) > 0.9982547f) ? Vector3.Right : Vector3.Forward);
				}
				Vector3.Cross(ref rotateAxis, ref vector3, out vector4);
				vector4.Normalize();
				Vector3.Cross(ref vector4, ref rotateAxis, out vector3);
				vector3.Normalize();
			}
			else
			{
				Vector3.Cross(ref rotateAxis, ref vector, out vector4);
				vector4.Normalize();
				Vector3.Cross(ref vector4, ref vector2, out vector3);
				vector3.Normalize();
			}
			result.M11 = vector4.X;
			result.M12 = vector4.Y;
			result.M13 = vector4.Z;
			result.M14 = 0f;
			result.M21 = vector2.X;
			result.M22 = vector2.Y;
			result.M23 = vector2.Z;
			result.M24 = 0f;
			result.M31 = vector3.X;
			result.M32 = vector3.Y;
			result.M33 = vector3.Z;
			result.M34 = 0f;
			result.M41 = objectPosition.X;
			result.M42 = objectPosition.Y;
			result.M43 = objectPosition.Z;
			result.M44 = 1f;
		}

		// Token: 0x06003649 RID: 13897 RVA: 0x00066E40 File Offset: 0x00065040
		public static Matrix CreateFromAxisAngle(Vector3 axis, float angle)
		{
			Matrix result;
			Matrix.CreateFromAxisAngle(ref axis, angle, out result);
			return result;
		}

		// Token: 0x0600364A RID: 13898 RVA: 0x00066E60 File Offset: 0x00065060
		public static void CreateFromAxisAngle(ref Vector3 axis, float angle, out Matrix result)
		{
			float x = axis.X;
			float y = axis.Y;
			float z = axis.Z;
			float num = (float)Math.Sin((double)angle);
			float num2 = (float)Math.Cos((double)angle);
			float num3 = x * x;
			float num4 = y * y;
			float num5 = z * z;
			float num6 = x * y;
			float num7 = x * z;
			float num8 = y * z;
			result.M11 = num3 + num2 * (1f - num3);
			result.M12 = num6 - num2 * num6 + num * z;
			result.M13 = num7 - num2 * num7 - num * y;
			result.M14 = 0f;
			result.M21 = num6 - num2 * num6 - num * z;
			result.M22 = num4 + num2 * (1f - num4);
			result.M23 = num8 - num2 * num8 + num * x;
			result.M24 = 0f;
			result.M31 = num7 - num2 * num7 + num * y;
			result.M32 = num8 - num2 * num8 - num * x;
			result.M33 = num5 + num2 * (1f - num5);
			result.M34 = 0f;
			result.M41 = 0f;
			result.M42 = 0f;
			result.M43 = 0f;
			result.M44 = 1f;
		}

		// Token: 0x0600364B RID: 13899 RVA: 0x00066FAC File Offset: 0x000651AC
		public static Matrix CreateFromQuaternion(Quaternion quaternion)
		{
			Matrix result;
			Matrix.CreateFromQuaternion(ref quaternion, out result);
			return result;
		}

		// Token: 0x0600364C RID: 13900 RVA: 0x00066FCC File Offset: 0x000651CC
		public static void CreateFromQuaternion(ref Quaternion quaternion, out Matrix result)
		{
			float num = quaternion.X * quaternion.X;
			float num2 = quaternion.Y * quaternion.Y;
			float num3 = quaternion.Z * quaternion.Z;
			float num4 = quaternion.X * quaternion.Y;
			float num5 = quaternion.Z * quaternion.W;
			float num6 = quaternion.Z * quaternion.X;
			float num7 = quaternion.Y * quaternion.W;
			float num8 = quaternion.Y * quaternion.Z;
			float num9 = quaternion.X * quaternion.W;
			result.M11 = 1f - 2f * (num2 + num3);
			result.M12 = 2f * (num4 + num5);
			result.M13 = 2f * (num6 - num7);
			result.M14 = 0f;
			result.M21 = 2f * (num4 - num5);
			result.M22 = 1f - 2f * (num3 + num);
			result.M23 = 2f * (num8 + num9);
			result.M24 = 0f;
			result.M31 = 2f * (num6 + num7);
			result.M32 = 2f * (num8 - num9);
			result.M33 = 1f - 2f * (num2 + num);
			result.M34 = 0f;
			result.M41 = 0f;
			result.M42 = 0f;
			result.M43 = 0f;
			result.M44 = 1f;
		}

		// Token: 0x0600364D RID: 13901 RVA: 0x00067150 File Offset: 0x00065350
		public static Matrix CreateFromYawPitchRoll(float yaw, float pitch, float roll)
		{
			Matrix result;
			Matrix.CreateFromYawPitchRoll(yaw, pitch, roll, out result);
			return result;
		}

		// Token: 0x0600364E RID: 13902 RVA: 0x00067170 File Offset: 0x00065370
		public static void CreateFromYawPitchRoll(float yaw, float pitch, float roll, out Matrix result)
		{
			Quaternion quaternion;
			Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll, out quaternion);
			Matrix.CreateFromQuaternion(ref quaternion, out result);
		}

		// Token: 0x0600364F RID: 13903 RVA: 0x00067194 File Offset: 0x00065394
		public static Matrix CreateLookAt(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector)
		{
			Matrix result;
			Matrix.CreateLookAt(ref cameraPosition, ref cameraTarget, ref cameraUpVector, out result);
			return result;
		}

		// Token: 0x06003650 RID: 13904 RVA: 0x000671B8 File Offset: 0x000653B8
		public static void CreateLookAt(ref Vector3 cameraPosition, ref Vector3 cameraTarget, ref Vector3 cameraUpVector, out Matrix result)
		{
			Vector3 vector = Vector3.Normalize(cameraPosition - cameraTarget);
			Vector3 vector2 = Vector3.Normalize(Vector3.Cross(cameraUpVector, vector));
			Vector3 vector3 = Vector3.Cross(vector, vector2);
			result.M11 = vector2.X;
			result.M12 = vector3.X;
			result.M13 = vector.X;
			result.M14 = 0f;
			result.M21 = vector2.Y;
			result.M22 = vector3.Y;
			result.M23 = vector.Y;
			result.M24 = 0f;
			result.M31 = vector2.Z;
			result.M32 = vector3.Z;
			result.M33 = vector.Z;
			result.M34 = 0f;
			result.M41 = -Vector3.Dot(vector2, cameraPosition);
			result.M42 = -Vector3.Dot(vector3, cameraPosition);
			result.M43 = -Vector3.Dot(vector, cameraPosition);
			result.M44 = 1f;
		}

		// Token: 0x06003651 RID: 13905 RVA: 0x000672C8 File Offset: 0x000654C8
		public static Matrix CreateOrthographic(float width, float height, float zNearPlane, float zFarPlane)
		{
			Matrix result;
			Matrix.CreateOrthographic(width, height, zNearPlane, zFarPlane, out result);
			return result;
		}

		// Token: 0x06003652 RID: 13906 RVA: 0x000672E8 File Offset: 0x000654E8
		public static void CreateOrthographic(float width, float height, float zNearPlane, float zFarPlane, out Matrix result)
		{
			result.M11 = 2f / width;
			result.M12 = (result.M13 = (result.M14 = 0f));
			result.M22 = 2f / height;
			result.M21 = (result.M23 = (result.M24 = 0f));
			result.M33 = 1f / (zNearPlane - zFarPlane);
			result.M31 = (result.M32 = (result.M34 = 0f));
			result.M41 = (result.M42 = 0f);
			result.M43 = zNearPlane / (zNearPlane - zFarPlane);
			result.M44 = 1f;
		}

		// Token: 0x06003653 RID: 13907 RVA: 0x000673B0 File Offset: 0x000655B0
		public static Matrix CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNearPlane, float zFarPlane)
		{
			Matrix result;
			Matrix.CreateOrthographicOffCenter(left, right, bottom, top, zNearPlane, zFarPlane, out result);
			return result;
		}

		// Token: 0x06003654 RID: 13908 RVA: 0x000673D4 File Offset: 0x000655D4
		public static void CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNearPlane, float zFarPlane, out Matrix result)
		{
			result.M11 = (float)(2.0 / ((double)right - (double)left));
			result.M12 = 0f;
			result.M13 = 0f;
			result.M14 = 0f;
			result.M21 = 0f;
			result.M22 = (float)(2.0 / ((double)top - (double)bottom));
			result.M23 = 0f;
			result.M24 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = (float)(1.0 / ((double)zNearPlane - (double)zFarPlane));
			result.M34 = 0f;
			result.M41 = (float)(((double)left + (double)right) / ((double)left - (double)right));
			result.M42 = (float)(((double)top + (double)bottom) / ((double)bottom - (double)top));
			result.M43 = (float)((double)zNearPlane / ((double)zNearPlane - (double)zFarPlane));
			result.M44 = 1f;
		}

		// Token: 0x06003655 RID: 13909 RVA: 0x000674DC File Offset: 0x000656DC
		public static Matrix CreatePerspective(float width, float height, float nearPlaneDistance, float farPlaneDistance)
		{
			Matrix result;
			Matrix.CreatePerspective(width, height, nearPlaneDistance, farPlaneDistance, out result);
			return result;
		}

		// Token: 0x06003656 RID: 13910 RVA: 0x000674FC File Offset: 0x000656FC
		public static void CreatePerspective(float width, float height, float nearPlaneDistance, float farPlaneDistance, out Matrix result)
		{
			bool flag = nearPlaneDistance <= 0f;
			if (flag)
			{
				throw new ArgumentException("nearPlaneDistance <= 0");
			}
			bool flag2 = farPlaneDistance <= 0f;
			if (flag2)
			{
				throw new ArgumentException("farPlaneDistance <= 0");
			}
			bool flag3 = nearPlaneDistance >= farPlaneDistance;
			if (flag3)
			{
				throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
			}
			result.M11 = 2f * nearPlaneDistance / width;
			result.M12 = (result.M13 = (result.M14 = 0f));
			result.M22 = 2f * nearPlaneDistance / height;
			result.M21 = (result.M23 = (result.M24 = 0f));
			result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
			result.M31 = (result.M32 = 0f);
			result.M34 = -1f;
			result.M41 = (result.M42 = (result.M44 = 0f));
			result.M43 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
		}

		// Token: 0x06003657 RID: 13911 RVA: 0x00067614 File Offset: 0x00065814
		public static Matrix CreatePerspectiveFieldOfView(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance)
		{
			Matrix result;
			Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearPlaneDistance, farPlaneDistance, out result);
			return result;
		}

		// Token: 0x06003658 RID: 13912 RVA: 0x00067634 File Offset: 0x00065834
		public static void CreatePerspectiveFieldOfView(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance, out Matrix result)
		{
			bool flag = fieldOfView <= 0f || fieldOfView >= 3.141593f;
			if (flag)
			{
				throw new ArgumentException("fieldOfView <= 0 or >= PI");
			}
			bool flag2 = nearPlaneDistance <= 0f;
			if (flag2)
			{
				throw new ArgumentException("nearPlaneDistance <= 0");
			}
			bool flag3 = farPlaneDistance <= 0f;
			if (flag3)
			{
				throw new ArgumentException("farPlaneDistance <= 0");
			}
			bool flag4 = nearPlaneDistance >= farPlaneDistance;
			if (flag4)
			{
				throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
			}
			float num = 1f / (float)Math.Tan((double)(fieldOfView * 0.5f));
			float m = num / aspectRatio;
			result.M11 = m;
			result.M12 = (result.M13 = (result.M14 = 0f));
			result.M22 = num;
			result.M21 = (result.M23 = (result.M24 = 0f));
			result.M31 = (result.M32 = 0f);
			result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
			result.M34 = -1f;
			result.M41 = (result.M42 = (result.M44 = 0f));
			result.M43 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
		}

		// Token: 0x06003659 RID: 13913 RVA: 0x0006778C File Offset: 0x0006598C
		public static Matrix CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float nearPlaneDistance, float farPlaneDistance)
		{
			Matrix result;
			Matrix.CreatePerspectiveOffCenter(left, right, bottom, top, nearPlaneDistance, farPlaneDistance, out result);
			return result;
		}

		// Token: 0x0600365A RID: 13914 RVA: 0x000677B0 File Offset: 0x000659B0
		public static void CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float nearPlaneDistance, float farPlaneDistance, out Matrix result)
		{
			bool flag = nearPlaneDistance <= 0f;
			if (flag)
			{
				throw new ArgumentException("nearPlaneDistance <= 0");
			}
			bool flag2 = farPlaneDistance <= 0f;
			if (flag2)
			{
				throw new ArgumentException("farPlaneDistance <= 0");
			}
			bool flag3 = nearPlaneDistance >= farPlaneDistance;
			if (flag3)
			{
				throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
			}
			result.M11 = 2f * nearPlaneDistance / (right - left);
			result.M12 = (result.M13 = (result.M14 = 0f));
			result.M22 = 2f * nearPlaneDistance / (top - bottom);
			result.M21 = (result.M23 = (result.M24 = 0f));
			result.M31 = (left + right) / (right - left);
			result.M32 = (top + bottom) / (top - bottom);
			result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
			result.M34 = -1f;
			result.M43 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
			result.M41 = (result.M42 = (result.M44 = 0f));
		}

		// Token: 0x0600365B RID: 13915 RVA: 0x000678E0 File Offset: 0x00065AE0
		public static Matrix CreateRotationX(float radians)
		{
			Matrix result;
			Matrix.CreateRotationX(radians, out result);
			return result;
		}

		// Token: 0x0600365C RID: 13916 RVA: 0x000678FC File Offset: 0x00065AFC
		public static void CreateRotationX(float radians, out Matrix result)
		{
			result = Matrix.Identity;
			float num = (float)Math.Cos((double)radians);
			float num2 = (float)Math.Sin((double)radians);
			result.M22 = num;
			result.M23 = num2;
			result.M32 = -num2;
			result.M33 = num;
		}

		// Token: 0x0600365D RID: 13917 RVA: 0x00067944 File Offset: 0x00065B44
		public static Matrix CreateRotationY(float radians)
		{
			Matrix result;
			Matrix.CreateRotationY(radians, out result);
			return result;
		}

		// Token: 0x0600365E RID: 13918 RVA: 0x00067960 File Offset: 0x00065B60
		public static void CreateRotationY(float radians, out Matrix result)
		{
			result = Matrix.Identity;
			float num = (float)Math.Cos((double)radians);
			float num2 = (float)Math.Sin((double)radians);
			result.M11 = num;
			result.M13 = -num2;
			result.M31 = num2;
			result.M33 = num;
		}

		// Token: 0x0600365F RID: 13919 RVA: 0x000679A8 File Offset: 0x00065BA8
		public static Matrix CreateRotationZ(float radians)
		{
			Matrix result;
			Matrix.CreateRotationZ(radians, out result);
			return result;
		}

		// Token: 0x06003660 RID: 13920 RVA: 0x000679C4 File Offset: 0x00065BC4
		public static void CreateRotationZ(float radians, out Matrix result)
		{
			result = Matrix.Identity;
			float num = (float)Math.Cos((double)radians);
			float num2 = (float)Math.Sin((double)radians);
			result.M11 = num;
			result.M12 = num2;
			result.M21 = -num2;
			result.M22 = num;
		}

		// Token: 0x06003661 RID: 13921 RVA: 0x00067A0C File Offset: 0x00065C0C
		public static Matrix CreateScale(float scale)
		{
			Matrix result;
			Matrix.CreateScale(scale, scale, scale, out result);
			return result;
		}

		// Token: 0x06003662 RID: 13922 RVA: 0x00067A2A File Offset: 0x00065C2A
		public static void CreateScale(float scale, out Matrix result)
		{
			Matrix.CreateScale(scale, scale, scale, out result);
		}

		// Token: 0x06003663 RID: 13923 RVA: 0x00067A38 File Offset: 0x00065C38
		public static Matrix CreateScale(float xScale, float yScale, float zScale)
		{
			Matrix result;
			Matrix.CreateScale(xScale, yScale, zScale, out result);
			return result;
		}

		// Token: 0x06003664 RID: 13924 RVA: 0x00067A58 File Offset: 0x00065C58
		public static void CreateScale(float xScale, float yScale, float zScale, out Matrix result)
		{
			result.M11 = xScale;
			result.M12 = 0f;
			result.M13 = 0f;
			result.M14 = 0f;
			result.M21 = 0f;
			result.M22 = yScale;
			result.M23 = 0f;
			result.M24 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = zScale;
			result.M34 = 0f;
			result.M41 = 0f;
			result.M42 = 0f;
			result.M43 = 0f;
			result.M44 = 1f;
		}

		// Token: 0x06003665 RID: 13925 RVA: 0x00067B0C File Offset: 0x00065D0C
		public static Matrix CreateScale(Vector3 scales)
		{
			Matrix result;
			Matrix.CreateScale(ref scales, out result);
			return result;
		}

		// Token: 0x06003666 RID: 13926 RVA: 0x00067B2C File Offset: 0x00065D2C
		public static void CreateScale(ref Vector3 scales, out Matrix result)
		{
			result.M11 = scales.X;
			result.M12 = 0f;
			result.M13 = 0f;
			result.M14 = 0f;
			result.M21 = 0f;
			result.M22 = scales.Y;
			result.M23 = 0f;
			result.M24 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = scales.Z;
			result.M34 = 0f;
			result.M41 = 0f;
			result.M42 = 0f;
			result.M43 = 0f;
			result.M44 = 1f;
		}

		// Token: 0x06003667 RID: 13927 RVA: 0x00067BF0 File Offset: 0x00065DF0
		public static Matrix CreateShadow(Vector3 lightDirection, Plane plane)
		{
			Matrix result;
			Matrix.CreateShadow(ref lightDirection, ref plane, out result);
			return result;
		}

		// Token: 0x06003668 RID: 13928 RVA: 0x00067C10 File Offset: 0x00065E10
		public static void CreateShadow(ref Vector3 lightDirection, ref Plane plane, out Matrix result)
		{
			float num = plane.Normal.X * lightDirection.X + plane.Normal.Y * lightDirection.Y + plane.Normal.Z * lightDirection.Z;
			float num2 = -plane.Normal.X;
			float num3 = -plane.Normal.Y;
			float num4 = -plane.Normal.Z;
			float num5 = -plane.D;
			result.M11 = num2 * lightDirection.X + num;
			result.M12 = num2 * lightDirection.Y;
			result.M13 = num2 * lightDirection.Z;
			result.M14 = 0f;
			result.M21 = num3 * lightDirection.X;
			result.M22 = num3 * lightDirection.Y + num;
			result.M23 = num3 * lightDirection.Z;
			result.M24 = 0f;
			result.M31 = num4 * lightDirection.X;
			result.M32 = num4 * lightDirection.Y;
			result.M33 = num4 * lightDirection.Z + num;
			result.M34 = 0f;
			result.M41 = num5 * lightDirection.X;
			result.M42 = num5 * lightDirection.Y;
			result.M43 = num5 * lightDirection.Z;
			result.M44 = num;
		}

		// Token: 0x06003669 RID: 13929 RVA: 0x00067D60 File Offset: 0x00065F60
		public static Matrix CreateTranslation(float xPosition, float yPosition, float zPosition)
		{
			Matrix result;
			Matrix.CreateTranslation(xPosition, yPosition, zPosition, out result);
			return result;
		}

		// Token: 0x0600366A RID: 13930 RVA: 0x00067D80 File Offset: 0x00065F80
		public static void CreateTranslation(ref Vector3 position, out Matrix result)
		{
			result.M11 = 1f;
			result.M12 = 0f;
			result.M13 = 0f;
			result.M14 = 0f;
			result.M21 = 0f;
			result.M22 = 1f;
			result.M23 = 0f;
			result.M24 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = 1f;
			result.M34 = 0f;
			result.M41 = position.X;
			result.M42 = position.Y;
			result.M43 = position.Z;
			result.M44 = 1f;
		}

		// Token: 0x0600366B RID: 13931 RVA: 0x00067E44 File Offset: 0x00066044
		public static Matrix CreateTranslation(Vector3 position)
		{
			Matrix result;
			Matrix.CreateTranslation(ref position, out result);
			return result;
		}

		// Token: 0x0600366C RID: 13932 RVA: 0x00067E64 File Offset: 0x00066064
		public static void CreateTranslation(float xPosition, float yPosition, float zPosition, out Matrix result)
		{
			result.M11 = 1f;
			result.M12 = 0f;
			result.M13 = 0f;
			result.M14 = 0f;
			result.M21 = 0f;
			result.M22 = 1f;
			result.M23 = 0f;
			result.M24 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = 1f;
			result.M34 = 0f;
			result.M41 = xPosition;
			result.M42 = yPosition;
			result.M43 = zPosition;
			result.M44 = 1f;
		}

		// Token: 0x0600366D RID: 13933 RVA: 0x00067F18 File Offset: 0x00066118
		public static Matrix CreateReflection(Plane value)
		{
			Matrix result;
			Matrix.CreateReflection(ref value, out result);
			return result;
		}

		// Token: 0x0600366E RID: 13934 RVA: 0x00067F38 File Offset: 0x00066138
		public static void CreateReflection(ref Plane value, out Matrix result)
		{
			Plane plane;
			Plane.Normalize(ref value, out plane);
			value.Normalize();
			float x = plane.Normal.X;
			float y = plane.Normal.Y;
			float z = plane.Normal.Z;
			float num = -2f * x;
			float num2 = -2f * y;
			float num3 = -2f * z;
			result.M11 = num * x + 1f;
			result.M12 = num2 * x;
			result.M13 = num3 * x;
			result.M14 = 0f;
			result.M21 = num * y;
			result.M22 = num2 * y + 1f;
			result.M23 = num3 * y;
			result.M24 = 0f;
			result.M31 = num * z;
			result.M32 = num2 * z;
			result.M33 = num3 * z + 1f;
			result.M34 = 0f;
			result.M41 = num * plane.D;
			result.M42 = num2 * plane.D;
			result.M43 = num3 * plane.D;
			result.M44 = 1f;
		}

		// Token: 0x0600366F RID: 13935 RVA: 0x0006805C File Offset: 0x0006625C
		public static Matrix CreateWorld(Vector3 position, Vector3 forward, Vector3 up)
		{
			Matrix result;
			Matrix.CreateWorld(ref position, ref forward, ref up, out result);
			return result;
		}

		// Token: 0x06003670 RID: 13936 RVA: 0x00068080 File Offset: 0x00066280
		public static void CreateWorld(ref Vector3 position, ref Vector3 forward, ref Vector3 up, out Matrix result)
		{
			Vector3 forward2;
			Vector3.Normalize(ref forward, out forward2);
			Vector3 right;
			Vector3.Cross(ref forward, ref up, out right);
			Vector3 up2;
			Vector3.Cross(ref right, ref forward, out up2);
			right.Normalize();
			up2.Normalize();
			result = default(Matrix);
			result.Right = right;
			result.Up = up2;
			result.Forward = forward2;
			result.Translation = position;
			result.M44 = 1f;
		}

		// Token: 0x06003671 RID: 13937 RVA: 0x000680F4 File Offset: 0x000662F4
		public static Matrix Divide(Matrix matrix1, Matrix matrix2)
		{
			matrix1.M11 /= matrix2.M11;
			matrix1.M12 /= matrix2.M12;
			matrix1.M13 /= matrix2.M13;
			matrix1.M14 /= matrix2.M14;
			matrix1.M21 /= matrix2.M21;
			matrix1.M22 /= matrix2.M22;
			matrix1.M23 /= matrix2.M23;
			matrix1.M24 /= matrix2.M24;
			matrix1.M31 /= matrix2.M31;
			matrix1.M32 /= matrix2.M32;
			matrix1.M33 /= matrix2.M33;
			matrix1.M34 /= matrix2.M34;
			matrix1.M41 /= matrix2.M41;
			matrix1.M42 /= matrix2.M42;
			matrix1.M43 /= matrix2.M43;
			matrix1.M44 /= matrix2.M44;
			return matrix1;
		}

		// Token: 0x06003672 RID: 13938 RVA: 0x00068248 File Offset: 0x00066448
		public static void Divide(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
		{
			result.M11 = matrix1.M11 / matrix2.M11;
			result.M12 = matrix1.M12 / matrix2.M12;
			result.M13 = matrix1.M13 / matrix2.M13;
			result.M14 = matrix1.M14 / matrix2.M14;
			result.M21 = matrix1.M21 / matrix2.M21;
			result.M22 = matrix1.M22 / matrix2.M22;
			result.M23 = matrix1.M23 / matrix2.M23;
			result.M24 = matrix1.M24 / matrix2.M24;
			result.M31 = matrix1.M31 / matrix2.M31;
			result.M32 = matrix1.M32 / matrix2.M32;
			result.M33 = matrix1.M33 / matrix2.M33;
			result.M34 = matrix1.M34 / matrix2.M34;
			result.M41 = matrix1.M41 / matrix2.M41;
			result.M42 = matrix1.M42 / matrix2.M42;
			result.M43 = matrix1.M43 / matrix2.M43;
			result.M44 = matrix1.M44 / matrix2.M44;
		}

		// Token: 0x06003673 RID: 13939 RVA: 0x00068388 File Offset: 0x00066588
		public static Matrix Divide(Matrix matrix1, float divider)
		{
			float num = 1f / divider;
			matrix1.M11 *= num;
			matrix1.M12 *= num;
			matrix1.M13 *= num;
			matrix1.M14 *= num;
			matrix1.M21 *= num;
			matrix1.M22 *= num;
			matrix1.M23 *= num;
			matrix1.M24 *= num;
			matrix1.M31 *= num;
			matrix1.M32 *= num;
			matrix1.M33 *= num;
			matrix1.M34 *= num;
			matrix1.M41 *= num;
			matrix1.M42 *= num;
			matrix1.M43 *= num;
			matrix1.M44 *= num;
			return matrix1;
		}

		// Token: 0x06003674 RID: 13940 RVA: 0x00068494 File Offset: 0x00066694
		public static void Divide(ref Matrix matrix1, float divider, out Matrix result)
		{
			float num = 1f / divider;
			result.M11 = matrix1.M11 * num;
			result.M12 = matrix1.M12 * num;
			result.M13 = matrix1.M13 * num;
			result.M14 = matrix1.M14 * num;
			result.M21 = matrix1.M21 * num;
			result.M22 = matrix1.M22 * num;
			result.M23 = matrix1.M23 * num;
			result.M24 = matrix1.M24 * num;
			result.M31 = matrix1.M31 * num;
			result.M32 = matrix1.M32 * num;
			result.M33 = matrix1.M33 * num;
			result.M34 = matrix1.M34 * num;
			result.M41 = matrix1.M41 * num;
			result.M42 = matrix1.M42 * num;
			result.M43 = matrix1.M43 * num;
			result.M44 = matrix1.M44 * num;
		}

		// Token: 0x06003675 RID: 13941 RVA: 0x0006858C File Offset: 0x0006678C
		public static Matrix Invert(Matrix matrix)
		{
			Matrix.Invert(ref matrix, out matrix);
			return matrix;
		}

		// Token: 0x06003676 RID: 13942 RVA: 0x000685AC File Offset: 0x000667AC
		public static void Invert(ref Matrix matrix, out Matrix result)
		{
			float m = matrix.M11;
			float m2 = matrix.M12;
			float m3 = matrix.M13;
			float m4 = matrix.M14;
			float m5 = matrix.M21;
			float m6 = matrix.M22;
			float m7 = matrix.M23;
			float m8 = matrix.M24;
			float m9 = matrix.M31;
			float m10 = matrix.M32;
			float m11 = matrix.M33;
			float m12 = matrix.M34;
			float m13 = matrix.M41;
			float m14 = matrix.M42;
			float m15 = matrix.M43;
			float m16 = matrix.M44;
			float num = (float)((double)m11 * (double)m16 - (double)m12 * (double)m15);
			float num2 = (float)((double)m10 * (double)m16 - (double)m12 * (double)m14);
			float num3 = (float)((double)m10 * (double)m15 - (double)m11 * (double)m14);
			float num4 = (float)((double)m9 * (double)m16 - (double)m12 * (double)m13);
			float num5 = (float)((double)m9 * (double)m15 - (double)m11 * (double)m13);
			float num6 = (float)((double)m9 * (double)m14 - (double)m10 * (double)m13);
			float num7 = (float)((double)m6 * (double)num - (double)m7 * (double)num2 + (double)m8 * (double)num3);
			float num8 = (float)(-(float)((double)m5 * (double)num - (double)m7 * (double)num4 + (double)m8 * (double)num5));
			float num9 = (float)((double)m5 * (double)num2 - (double)m6 * (double)num4 + (double)m8 * (double)num6);
			float num10 = (float)(-(float)((double)m5 * (double)num3 - (double)m6 * (double)num5 + (double)m7 * (double)num6));
			float num11 = (float)(1.0 / ((double)m * (double)num7 + (double)m2 * (double)num8 + (double)m3 * (double)num9 + (double)m4 * (double)num10));
			result.M11 = num7 * num11;
			result.M21 = num8 * num11;
			result.M31 = num9 * num11;
			result.M41 = num10 * num11;
			result.M12 = (float)(-(float)((double)m2 * (double)num - (double)m3 * (double)num2 + (double)m4 * (double)num3) * (double)num11);
			result.M22 = (float)(((double)m * (double)num - (double)m3 * (double)num4 + (double)m4 * (double)num5) * (double)num11);
			result.M32 = (float)(-(float)((double)m * (double)num2 - (double)m2 * (double)num4 + (double)m4 * (double)num6) * (double)num11);
			result.M42 = (float)(((double)m * (double)num3 - (double)m2 * (double)num5 + (double)m3 * (double)num6) * (double)num11);
			float num12 = (float)((double)m7 * (double)m16 - (double)m8 * (double)m15);
			float num13 = (float)((double)m6 * (double)m16 - (double)m8 * (double)m14);
			float num14 = (float)((double)m6 * (double)m15 - (double)m7 * (double)m14);
			float num15 = (float)((double)m5 * (double)m16 - (double)m8 * (double)m13);
			float num16 = (float)((double)m5 * (double)m15 - (double)m7 * (double)m13);
			float num17 = (float)((double)m5 * (double)m14 - (double)m6 * (double)m13);
			result.M13 = (float)(((double)m2 * (double)num12 - (double)m3 * (double)num13 + (double)m4 * (double)num14) * (double)num11);
			result.M23 = (float)(-(float)((double)m * (double)num12 - (double)m3 * (double)num15 + (double)m4 * (double)num16) * (double)num11);
			result.M33 = (float)(((double)m * (double)num13 - (double)m2 * (double)num15 + (double)m4 * (double)num17) * (double)num11);
			result.M43 = (float)(-(float)((double)m * (double)num14 - (double)m2 * (double)num16 + (double)m3 * (double)num17) * (double)num11);
			float num18 = (float)((double)m7 * (double)m12 - (double)m8 * (double)m11);
			float num19 = (float)((double)m6 * (double)m12 - (double)m8 * (double)m10);
			float num20 = (float)((double)m6 * (double)m11 - (double)m7 * (double)m10);
			float num21 = (float)((double)m5 * (double)m12 - (double)m8 * (double)m9);
			float num22 = (float)((double)m5 * (double)m11 - (double)m7 * (double)m9);
			float num23 = (float)((double)m5 * (double)m10 - (double)m6 * (double)m9);
			result.M14 = (float)(-(float)((double)m2 * (double)num18 - (double)m3 * (double)num19 + (double)m4 * (double)num20) * (double)num11);
			result.M24 = (float)(((double)m * (double)num18 - (double)m3 * (double)num21 + (double)m4 * (double)num22) * (double)num11);
			result.M34 = (float)(-(float)((double)m * (double)num19 - (double)m2 * (double)num21 + (double)m4 * (double)num23) * (double)num11);
			result.M44 = (float)(((double)m * (double)num20 - (double)m2 * (double)num22 + (double)m3 * (double)num23) * (double)num11);
		}

		// Token: 0x06003677 RID: 13943 RVA: 0x000689B4 File Offset: 0x00066BB4
		public static Matrix Lerp(Matrix matrix1, Matrix matrix2, float amount)
		{
			matrix1.M11 += (matrix2.M11 - matrix1.M11) * amount;
			matrix1.M12 += (matrix2.M12 - matrix1.M12) * amount;
			matrix1.M13 += (matrix2.M13 - matrix1.M13) * amount;
			matrix1.M14 += (matrix2.M14 - matrix1.M14) * amount;
			matrix1.M21 += (matrix2.M21 - matrix1.M21) * amount;
			matrix1.M22 += (matrix2.M22 - matrix1.M22) * amount;
			matrix1.M23 += (matrix2.M23 - matrix1.M23) * amount;
			matrix1.M24 += (matrix2.M24 - matrix1.M24) * amount;
			matrix1.M31 += (matrix2.M31 - matrix1.M31) * amount;
			matrix1.M32 += (matrix2.M32 - matrix1.M32) * amount;
			matrix1.M33 += (matrix2.M33 - matrix1.M33) * amount;
			matrix1.M34 += (matrix2.M34 - matrix1.M34) * amount;
			matrix1.M41 += (matrix2.M41 - matrix1.M41) * amount;
			matrix1.M42 += (matrix2.M42 - matrix1.M42) * amount;
			matrix1.M43 += (matrix2.M43 - matrix1.M43) * amount;
			matrix1.M44 += (matrix2.M44 - matrix1.M44) * amount;
			return matrix1;
		}

		// Token: 0x06003678 RID: 13944 RVA: 0x00068B98 File Offset: 0x00066D98
		public static void Lerp(ref Matrix matrix1, ref Matrix matrix2, float amount, out Matrix result)
		{
			result.M11 = matrix1.M11 + (matrix2.M11 - matrix1.M11) * amount;
			result.M12 = matrix1.M12 + (matrix2.M12 - matrix1.M12) * amount;
			result.M13 = matrix1.M13 + (matrix2.M13 - matrix1.M13) * amount;
			result.M14 = matrix1.M14 + (matrix2.M14 - matrix1.M14) * amount;
			result.M21 = matrix1.M21 + (matrix2.M21 - matrix1.M21) * amount;
			result.M22 = matrix1.M22 + (matrix2.M22 - matrix1.M22) * amount;
			result.M23 = matrix1.M23 + (matrix2.M23 - matrix1.M23) * amount;
			result.M24 = matrix1.M24 + (matrix2.M24 - matrix1.M24) * amount;
			result.M31 = matrix1.M31 + (matrix2.M31 - matrix1.M31) * amount;
			result.M32 = matrix1.M32 + (matrix2.M32 - matrix1.M32) * amount;
			result.M33 = matrix1.M33 + (matrix2.M33 - matrix1.M33) * amount;
			result.M34 = matrix1.M34 + (matrix2.M34 - matrix1.M34) * amount;
			result.M41 = matrix1.M41 + (matrix2.M41 - matrix1.M41) * amount;
			result.M42 = matrix1.M42 + (matrix2.M42 - matrix1.M42) * amount;
			result.M43 = matrix1.M43 + (matrix2.M43 - matrix1.M43) * amount;
			result.M44 = matrix1.M44 + (matrix2.M44 - matrix1.M44) * amount;
		}

		// Token: 0x06003679 RID: 13945 RVA: 0x00068D68 File Offset: 0x00066F68
		public static Matrix Multiply(Matrix matrix1, Matrix matrix2)
		{
			float m = matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21 + matrix1.M13 * matrix2.M31 + matrix1.M14 * matrix2.M41;
			float m2 = matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22 + matrix1.M13 * matrix2.M32 + matrix1.M14 * matrix2.M42;
			float m3 = matrix1.M11 * matrix2.M13 + matrix1.M12 * matrix2.M23 + matrix1.M13 * matrix2.M33 + matrix1.M14 * matrix2.M43;
			float m4 = matrix1.M11 * matrix2.M14 + matrix1.M12 * matrix2.M24 + matrix1.M13 * matrix2.M34 + matrix1.M14 * matrix2.M44;
			float m5 = matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21 + matrix1.M23 * matrix2.M31 + matrix1.M24 * matrix2.M41;
			float m6 = matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22 + matrix1.M23 * matrix2.M32 + matrix1.M24 * matrix2.M42;
			float m7 = matrix1.M21 * matrix2.M13 + matrix1.M22 * matrix2.M23 + matrix1.M23 * matrix2.M33 + matrix1.M24 * matrix2.M43;
			float m8 = matrix1.M21 * matrix2.M14 + matrix1.M22 * matrix2.M24 + matrix1.M23 * matrix2.M34 + matrix1.M24 * matrix2.M44;
			float m9 = matrix1.M31 * matrix2.M11 + matrix1.M32 * matrix2.M21 + matrix1.M33 * matrix2.M31 + matrix1.M34 * matrix2.M41;
			float m10 = matrix1.M31 * matrix2.M12 + matrix1.M32 * matrix2.M22 + matrix1.M33 * matrix2.M32 + matrix1.M34 * matrix2.M42;
			float m11 = matrix1.M31 * matrix2.M13 + matrix1.M32 * matrix2.M23 + matrix1.M33 * matrix2.M33 + matrix1.M34 * matrix2.M43;
			float m12 = matrix1.M31 * matrix2.M14 + matrix1.M32 * matrix2.M24 + matrix1.M33 * matrix2.M34 + matrix1.M34 * matrix2.M44;
			float m13 = matrix1.M41 * matrix2.M11 + matrix1.M42 * matrix2.M21 + matrix1.M43 * matrix2.M31 + matrix1.M44 * matrix2.M41;
			float m14 = matrix1.M41 * matrix2.M12 + matrix1.M42 * matrix2.M22 + matrix1.M43 * matrix2.M32 + matrix1.M44 * matrix2.M42;
			float m15 = matrix1.M41 * matrix2.M13 + matrix1.M42 * matrix2.M23 + matrix1.M43 * matrix2.M33 + matrix1.M44 * matrix2.M43;
			float m16 = matrix1.M41 * matrix2.M14 + matrix1.M42 * matrix2.M24 + matrix1.M43 * matrix2.M34 + matrix1.M44 * matrix2.M44;
			matrix1.M11 = m;
			matrix1.M12 = m2;
			matrix1.M13 = m3;
			matrix1.M14 = m4;
			matrix1.M21 = m5;
			matrix1.M22 = m6;
			matrix1.M23 = m7;
			matrix1.M24 = m8;
			matrix1.M31 = m9;
			matrix1.M32 = m10;
			matrix1.M33 = m11;
			matrix1.M34 = m12;
			matrix1.M41 = m13;
			matrix1.M42 = m14;
			matrix1.M43 = m15;
			matrix1.M44 = m16;
			return matrix1;
		}

		// Token: 0x0600367A RID: 13946 RVA: 0x00069198 File Offset: 0x00067398
		public static void Multiply(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
		{
			float m = matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21 + matrix1.M13 * matrix2.M31 + matrix1.M14 * matrix2.M41;
			float m2 = matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22 + matrix1.M13 * matrix2.M32 + matrix1.M14 * matrix2.M42;
			float m3 = matrix1.M11 * matrix2.M13 + matrix1.M12 * matrix2.M23 + matrix1.M13 * matrix2.M33 + matrix1.M14 * matrix2.M43;
			float m4 = matrix1.M11 * matrix2.M14 + matrix1.M12 * matrix2.M24 + matrix1.M13 * matrix2.M34 + matrix1.M14 * matrix2.M44;
			float m5 = matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21 + matrix1.M23 * matrix2.M31 + matrix1.M24 * matrix2.M41;
			float m6 = matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22 + matrix1.M23 * matrix2.M32 + matrix1.M24 * matrix2.M42;
			float m7 = matrix1.M21 * matrix2.M13 + matrix1.M22 * matrix2.M23 + matrix1.M23 * matrix2.M33 + matrix1.M24 * matrix2.M43;
			float m8 = matrix1.M21 * matrix2.M14 + matrix1.M22 * matrix2.M24 + matrix1.M23 * matrix2.M34 + matrix1.M24 * matrix2.M44;
			float m9 = matrix1.M31 * matrix2.M11 + matrix1.M32 * matrix2.M21 + matrix1.M33 * matrix2.M31 + matrix1.M34 * matrix2.M41;
			float m10 = matrix1.M31 * matrix2.M12 + matrix1.M32 * matrix2.M22 + matrix1.M33 * matrix2.M32 + matrix1.M34 * matrix2.M42;
			float m11 = matrix1.M31 * matrix2.M13 + matrix1.M32 * matrix2.M23 + matrix1.M33 * matrix2.M33 + matrix1.M34 * matrix2.M43;
			float m12 = matrix1.M31 * matrix2.M14 + matrix1.M32 * matrix2.M24 + matrix1.M33 * matrix2.M34 + matrix1.M34 * matrix2.M44;
			float m13 = matrix1.M41 * matrix2.M11 + matrix1.M42 * matrix2.M21 + matrix1.M43 * matrix2.M31 + matrix1.M44 * matrix2.M41;
			float m14 = matrix1.M41 * matrix2.M12 + matrix1.M42 * matrix2.M22 + matrix1.M43 * matrix2.M32 + matrix1.M44 * matrix2.M42;
			float m15 = matrix1.M41 * matrix2.M13 + matrix1.M42 * matrix2.M23 + matrix1.M43 * matrix2.M33 + matrix1.M44 * matrix2.M43;
			float m16 = matrix1.M41 * matrix2.M14 + matrix1.M42 * matrix2.M24 + matrix1.M43 * matrix2.M34 + matrix1.M44 * matrix2.M44;
			result.M11 = m;
			result.M12 = m2;
			result.M13 = m3;
			result.M14 = m4;
			result.M21 = m5;
			result.M22 = m6;
			result.M23 = m7;
			result.M24 = m8;
			result.M31 = m9;
			result.M32 = m10;
			result.M33 = m11;
			result.M34 = m12;
			result.M41 = m13;
			result.M42 = m14;
			result.M43 = m15;
			result.M44 = m16;
		}

		// Token: 0x0600367B RID: 13947 RVA: 0x000695B0 File Offset: 0x000677B0
		public static Matrix Multiply(Matrix matrix1, float scaleFactor)
		{
			matrix1.M11 *= scaleFactor;
			matrix1.M12 *= scaleFactor;
			matrix1.M13 *= scaleFactor;
			matrix1.M14 *= scaleFactor;
			matrix1.M21 *= scaleFactor;
			matrix1.M22 *= scaleFactor;
			matrix1.M23 *= scaleFactor;
			matrix1.M24 *= scaleFactor;
			matrix1.M31 *= scaleFactor;
			matrix1.M32 *= scaleFactor;
			matrix1.M33 *= scaleFactor;
			matrix1.M34 *= scaleFactor;
			matrix1.M41 *= scaleFactor;
			matrix1.M42 *= scaleFactor;
			matrix1.M43 *= scaleFactor;
			matrix1.M44 *= scaleFactor;
			return matrix1;
		}

		// Token: 0x0600367C RID: 13948 RVA: 0x00069684 File Offset: 0x00067884
		public static void Multiply(ref Matrix matrix1, float scaleFactor, out Matrix result)
		{
			result.M11 = matrix1.M11 * scaleFactor;
			result.M12 = matrix1.M12 * scaleFactor;
			result.M13 = matrix1.M13 * scaleFactor;
			result.M14 = matrix1.M14 * scaleFactor;
			result.M21 = matrix1.M21 * scaleFactor;
			result.M22 = matrix1.M22 * scaleFactor;
			result.M23 = matrix1.M23 * scaleFactor;
			result.M24 = matrix1.M24 * scaleFactor;
			result.M31 = matrix1.M31 * scaleFactor;
			result.M32 = matrix1.M32 * scaleFactor;
			result.M33 = matrix1.M33 * scaleFactor;
			result.M34 = matrix1.M34 * scaleFactor;
			result.M41 = matrix1.M41 * scaleFactor;
			result.M42 = matrix1.M42 * scaleFactor;
			result.M43 = matrix1.M43 * scaleFactor;
			result.M44 = matrix1.M44 * scaleFactor;
		}

		// Token: 0x0600367D RID: 13949 RVA: 0x00069774 File Offset: 0x00067974
		public static Matrix Negate(Matrix matrix)
		{
			matrix.M11 = -matrix.M11;
			matrix.M12 = -matrix.M12;
			matrix.M13 = -matrix.M13;
			matrix.M14 = -matrix.M14;
			matrix.M21 = -matrix.M21;
			matrix.M22 = -matrix.M22;
			matrix.M23 = -matrix.M23;
			matrix.M24 = -matrix.M24;
			matrix.M31 = -matrix.M31;
			matrix.M32 = -matrix.M32;
			matrix.M33 = -matrix.M33;
			matrix.M34 = -matrix.M34;
			matrix.M41 = -matrix.M41;
			matrix.M42 = -matrix.M42;
			matrix.M43 = -matrix.M43;
			matrix.M44 = -matrix.M44;
			return matrix;
		}

		// Token: 0x0600367E RID: 13950 RVA: 0x00069868 File Offset: 0x00067A68
		public static void Negate(ref Matrix matrix, out Matrix result)
		{
			result.M11 = -matrix.M11;
			result.M12 = -matrix.M12;
			result.M13 = -matrix.M13;
			result.M14 = -matrix.M14;
			result.M21 = -matrix.M21;
			result.M22 = -matrix.M22;
			result.M23 = -matrix.M23;
			result.M24 = -matrix.M24;
			result.M31 = -matrix.M31;
			result.M32 = -matrix.M32;
			result.M33 = -matrix.M33;
			result.M34 = -matrix.M34;
			result.M41 = -matrix.M41;
			result.M42 = -matrix.M42;
			result.M43 = -matrix.M43;
			result.M44 = -matrix.M44;
		}

		// Token: 0x0600367F RID: 13951 RVA: 0x00069948 File Offset: 0x00067B48
		public static Matrix Subtract(Matrix matrix1, Matrix matrix2)
		{
			matrix1.M11 -= matrix2.M11;
			matrix1.M12 -= matrix2.M12;
			matrix1.M13 -= matrix2.M13;
			matrix1.M14 -= matrix2.M14;
			matrix1.M21 -= matrix2.M21;
			matrix1.M22 -= matrix2.M22;
			matrix1.M23 -= matrix2.M23;
			matrix1.M24 -= matrix2.M24;
			matrix1.M31 -= matrix2.M31;
			matrix1.M32 -= matrix2.M32;
			matrix1.M33 -= matrix2.M33;
			matrix1.M34 -= matrix2.M34;
			matrix1.M41 -= matrix2.M41;
			matrix1.M42 -= matrix2.M42;
			matrix1.M43 -= matrix2.M43;
			matrix1.M44 -= matrix2.M44;
			return matrix1;
		}

		// Token: 0x06003680 RID: 13952 RVA: 0x00069A6C File Offset: 0x00067C6C
		public static void Subtract(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
		{
			result.M11 = matrix1.M11 - matrix2.M11;
			result.M12 = matrix1.M12 - matrix2.M12;
			result.M13 = matrix1.M13 - matrix2.M13;
			result.M14 = matrix1.M14 - matrix2.M14;
			result.M21 = matrix1.M21 - matrix2.M21;
			result.M22 = matrix1.M22 - matrix2.M22;
			result.M23 = matrix1.M23 - matrix2.M23;
			result.M24 = matrix1.M24 - matrix2.M24;
			result.M31 = matrix1.M31 - matrix2.M31;
			result.M32 = matrix1.M32 - matrix2.M32;
			result.M33 = matrix1.M33 - matrix2.M33;
			result.M34 = matrix1.M34 - matrix2.M34;
			result.M41 = matrix1.M41 - matrix2.M41;
			result.M42 = matrix1.M42 - matrix2.M42;
			result.M43 = matrix1.M43 - matrix2.M43;
			result.M44 = matrix1.M44 - matrix2.M44;
		}

		// Token: 0x06003681 RID: 13953 RVA: 0x00069BAC File Offset: 0x00067DAC
		public static Matrix Transpose(Matrix matrix)
		{
			Matrix result;
			Matrix.Transpose(ref matrix, out result);
			return result;
		}

		// Token: 0x06003682 RID: 13954 RVA: 0x00069BCC File Offset: 0x00067DCC
		public static void Transpose(ref Matrix matrix, out Matrix result)
		{
			Matrix matrix2;
			matrix2.M11 = matrix.M11;
			matrix2.M12 = matrix.M21;
			matrix2.M13 = matrix.M31;
			matrix2.M14 = matrix.M41;
			matrix2.M21 = matrix.M12;
			matrix2.M22 = matrix.M22;
			matrix2.M23 = matrix.M32;
			matrix2.M24 = matrix.M42;
			matrix2.M31 = matrix.M13;
			matrix2.M32 = matrix.M23;
			matrix2.M33 = matrix.M33;
			matrix2.M34 = matrix.M43;
			matrix2.M41 = matrix.M14;
			matrix2.M42 = matrix.M24;
			matrix2.M43 = matrix.M34;
			matrix2.M44 = matrix.M44;
			result = matrix2;
		}

		// Token: 0x06003683 RID: 13955 RVA: 0x00069CB4 File Offset: 0x00067EB4
		public static Matrix Transform(Matrix value, Quaternion rotation)
		{
			Matrix result;
			Matrix.Transform(ref value, ref rotation, out result);
			return result;
		}

		// Token: 0x06003684 RID: 13956 RVA: 0x00069CD4 File Offset: 0x00067ED4
		public static void Transform(ref Matrix value, ref Quaternion rotation, out Matrix result)
		{
			Matrix matrix = Matrix.CreateFromQuaternion(rotation);
			Matrix.Multiply(ref value, ref matrix, out result);
		}

		// Token: 0x06003685 RID: 13957 RVA: 0x00069CF8 File Offset: 0x00067EF8
		public static Matrix operator +(Matrix matrix1, Matrix matrix2)
		{
			return Matrix.Add(matrix1, matrix2);
		}

		// Token: 0x06003686 RID: 13958 RVA: 0x00069D14 File Offset: 0x00067F14
		public static Matrix operator /(Matrix matrix1, Matrix matrix2)
		{
			return Matrix.Divide(matrix1, matrix2);
		}

		// Token: 0x06003687 RID: 13959 RVA: 0x00069D30 File Offset: 0x00067F30
		public static Matrix operator /(Matrix matrix, float divider)
		{
			return Matrix.Divide(matrix, divider);
		}

		// Token: 0x06003688 RID: 13960 RVA: 0x00069D4C File Offset: 0x00067F4C
		public static bool operator ==(Matrix matrix1, Matrix matrix2)
		{
			return matrix1.Equals(matrix2);
		}

		// Token: 0x06003689 RID: 13961 RVA: 0x00069D68 File Offset: 0x00067F68
		public static bool operator !=(Matrix matrix1, Matrix matrix2)
		{
			return !matrix1.Equals(matrix2);
		}

		// Token: 0x0600368A RID: 13962 RVA: 0x00069D88 File Offset: 0x00067F88
		public static Matrix operator *(Matrix matrix1, Matrix matrix2)
		{
			return Matrix.Multiply(matrix1, matrix2);
		}

		// Token: 0x0600368B RID: 13963 RVA: 0x00069DA4 File Offset: 0x00067FA4
		public static Matrix operator *(Matrix matrix, float scaleFactor)
		{
			return Matrix.Multiply(matrix, scaleFactor);
		}

		// Token: 0x0600368C RID: 13964 RVA: 0x00069DC0 File Offset: 0x00067FC0
		public static Matrix operator -(Matrix matrix1, Matrix matrix2)
		{
			return Matrix.Subtract(matrix1, matrix2);
		}

		// Token: 0x0600368D RID: 13965 RVA: 0x00069DDC File Offset: 0x00067FDC
		public static Matrix operator -(Matrix matrix)
		{
			return Matrix.Negate(matrix);
		}

		// Token: 0x0600368E RID: 13966 RVA: 0x00069DF4 File Offset: 0x00067FF4
		public Matrix(float[] data)
		{
			bool flag = data.Length != 16;
			if (flag)
			{
				throw new ArgumentException();
			}
			this.M11 = data[0];
			this.M12 = data[1];
			this.M13 = data[2];
			this.M14 = data[3];
			this.M21 = data[4];
			this.M22 = data[5];
			this.M23 = data[6];
			this.M24 = data[7];
			this.M31 = data[8];
			this.M32 = data[9];
			this.M33 = data[10];
			this.M34 = data[11];
			this.M41 = data[12];
			this.M42 = data[13];
			this.M43 = data[14];
			this.M44 = data[15];
		}

		// Token: 0x17000FCD RID: 4045
		// (get) Token: 0x0600368F RID: 13967 RVA: 0x00069EB0 File Offset: 0x000680B0
		public Vector3 Row0
		{
			get
			{
				return new Vector3(this.M11, this.M12, this.M13);
			}
		}

		// Token: 0x17000FCE RID: 4046
		// (get) Token: 0x06003690 RID: 13968 RVA: 0x00069EDC File Offset: 0x000680DC
		public Vector3 Row1
		{
			get
			{
				return new Vector3(this.M21, this.M22, this.M23);
			}
		}

		// Token: 0x17000FCF RID: 4047
		// (get) Token: 0x06003691 RID: 13969 RVA: 0x00069F08 File Offset: 0x00068108
		public Vector3 Row2
		{
			get
			{
				return new Vector3(this.M31, this.M32, this.M33);
			}
		}

		// Token: 0x17000FD0 RID: 4048
		// (get) Token: 0x06003692 RID: 13970 RVA: 0x00069F34 File Offset: 0x00068134
		public Vector3 Row3
		{
			get
			{
				return new Vector3(this.M41, this.M42, this.M43);
			}
		}

		// Token: 0x17000FD1 RID: 4049
		// (get) Token: 0x06003693 RID: 13971 RVA: 0x00069F60 File Offset: 0x00068160
		public Vector3 Column0
		{
			get
			{
				return new Vector3(this.M11, this.M21, this.M31);
			}
		}

		// Token: 0x17000FD2 RID: 4050
		// (get) Token: 0x06003694 RID: 13972 RVA: 0x00069F8C File Offset: 0x0006818C
		public Vector3 Column1
		{
			get
			{
				return new Vector3(this.M12, this.M22, this.M32);
			}
		}

		// Token: 0x17000FD3 RID: 4051
		// (get) Token: 0x06003695 RID: 13973 RVA: 0x00069FB8 File Offset: 0x000681B8
		public Vector3 Column2
		{
			get
			{
				return new Vector3(this.M13, this.M23, this.M33);
			}
		}

		// Token: 0x17000FD4 RID: 4052
		// (get) Token: 0x06003696 RID: 13974 RVA: 0x00069FE4 File Offset: 0x000681E4
		public Vector3 Column3
		{
			get
			{
				return new Vector3(this.M14, this.M24, this.M34);
			}
		}

		// Token: 0x06003697 RID: 13975 RVA: 0x0006A010 File Offset: 0x00068210
		public static void Copy(ref Matrix source, out Matrix result)
		{
			result.M11 = source.M11;
			result.M12 = source.M12;
			result.M13 = source.M13;
			result.M14 = source.M14;
			result.M21 = source.M21;
			result.M22 = source.M22;
			result.M23 = source.M23;
			result.M24 = source.M24;
			result.M31 = source.M31;
			result.M32 = source.M32;
			result.M33 = source.M33;
			result.M34 = source.M34;
			result.M41 = source.M41;
			result.M42 = source.M42;
			result.M43 = source.M43;
			result.M44 = source.M44;
		}

		// Token: 0x06003698 RID: 13976 RVA: 0x0006A0E0 File Offset: 0x000682E0
		public static void CreatePerspectiveFieldOfViewReverseZ(float fieldOfView, float aspectRatio, float nearPlaneDistance, out Matrix result)
		{
			bool flag = fieldOfView <= 0f || fieldOfView >= 3.141593f;
			if (flag)
			{
				throw new ArgumentException("fieldOfView <= 0 or >= PI");
			}
			bool flag2 = nearPlaneDistance <= 0f;
			if (flag2)
			{
				throw new ArgumentException("nearPlaneDistance <= 0");
			}
			float num = 1f / (float)Math.Tan((double)(fieldOfView * 0.5f));
			float m = num / aspectRatio;
			result.M11 = m;
			result.M12 = (result.M13 = (result.M14 = 0f));
			result.M22 = num;
			result.M21 = (result.M23 = (result.M24 = 0f));
			result.M31 = (result.M32 = (result.M33 = (result.M41 = (result.M42 = (result.M44 = 0f)))));
			result.M34 = -1f;
			result.M43 = 2f * nearPlaneDistance;
		}

		// Token: 0x06003699 RID: 13977 RVA: 0x0006A1F0 File Offset: 0x000683F0
		public static Matrix CreateProjectionFrustum(float left, float right, float bottom, float top, float near, float far)
		{
			float num = 1f / (right + left);
			float num2 = 1f / (top + bottom);
			float num3 = 1f / (near - far);
			return new Matrix
			{
				M34 = -1f,
				M11 = 2f * (near * num),
				M22 = 2f * (near * num2),
				M43 = 2f * (far * near * num3),
				M31 = 2f * (right - left) * num,
				M32 = (top - bottom) * num2,
				M33 = (far + near) * num3
			};
		}

		// Token: 0x0600369A RID: 13978 RVA: 0x0006A29C File Offset: 0x0006849C
		public static Matrix CreateProjectionOrtho(float left, float right, float bottom, float top, float near, float far)
		{
			float num = 1f / (right + left);
			float num2 = 1f / (top + bottom);
			float num3 = -1f / (far - near);
			float m = 2f * num;
			float m2 = 2f * num2;
			float m3 = 2f * num3;
			return new Matrix
			{
				M44 = 1f,
				M11 = m,
				M22 = m2,
				M33 = m3,
				M41 = -(right - left) * num,
				M42 = -(top - bottom) * num2,
				M43 = (far + near) * num3
			};
		}

		// Token: 0x0600369B RID: 13979 RVA: 0x0006A344 File Offset: 0x00068544
		public static Matrix CreateViewDirection(float eyeX, float eyeY, float eyeZ, float dirX, float dirY, float dirZ, float upX, float upY, float upZ)
		{
			float num = 1f / (float)Math.Sqrt((double)(dirX * dirX + dirY * dirY + dirZ * dirZ));
			dirX *= num;
			dirY *= num;
			dirZ *= num;
			float num2 = dirY * upZ - dirZ * upY;
			float num3 = dirZ * upX - dirX * upZ;
			float num4 = dirX * upY - dirY * upX;
			float num5 = 1f / (float)Math.Sqrt((double)(num2 * num2 + num3 * num3 + num4 * num4));
			num2 *= num5;
			num3 *= num5;
			num4 *= num5;
			float m = num3 * dirZ - num4 * dirY;
			float m2 = num4 * dirX - num2 * dirZ;
			float m3 = num2 * dirY - num3 * dirX;
			Matrix matrix = new Matrix
			{
				M11 = num2,
				M12 = m,
				M13 = -dirX,
				M14 = 0f,
				M21 = num3,
				M22 = m2,
				M23 = -dirY,
				M24 = 0f,
				M31 = num4,
				M32 = m3,
				M33 = -dirZ,
				M34 = 0f,
				M41 = 0f,
				M42 = 0f,
				M43 = 0f,
				M44 = 1f
			};
			return Matrix.CreateTranslation(-eyeX, -eyeY, -eyeZ) * matrix;
		}

		// Token: 0x0600369C RID: 13980 RVA: 0x0006A4B2 File Offset: 0x000686B2
		public static void AddTranslation(ref Matrix matrix, float x, float y, float z)
		{
			matrix.M41 += x;
			matrix.M42 += y;
			matrix.M43 += z;
		}

		// Token: 0x0600369D RID: 13981 RVA: 0x0006A4D8 File Offset: 0x000686D8
		public static void ApplyScale(ref Matrix matrix, Vector3 scale)
		{
			matrix.M11 *= scale.X;
			matrix.M12 *= scale.X;
			matrix.M13 *= scale.X;
			matrix.M14 *= scale.X;
			matrix.M21 *= scale.Y;
			matrix.M22 *= scale.Y;
			matrix.M23 *= scale.Y;
			matrix.M24 *= scale.Y;
			matrix.M31 *= scale.Z;
			matrix.M32 *= scale.Z;
			matrix.M33 *= scale.Z;
			matrix.M34 *= scale.Z;
		}

		// Token: 0x0600369E RID: 13982 RVA: 0x0006A5A8 File Offset: 0x000687A8
		public static void ApplyScale(ref Matrix matrix, float scale)
		{
			matrix.M11 *= scale;
			matrix.M12 *= scale;
			matrix.M13 *= scale;
			matrix.M14 *= scale;
			matrix.M21 *= scale;
			matrix.M22 *= scale;
			matrix.M23 *= scale;
			matrix.M24 *= scale;
			matrix.M31 *= scale;
			matrix.M32 *= scale;
			matrix.M33 *= scale;
			matrix.M34 *= scale;
		}

		// Token: 0x0600369F RID: 13983 RVA: 0x0006A63C File Offset: 0x0006883C
		public static Matrix Compose(float scale, Quaternion quaternion, Vector3 translation)
		{
			Matrix result;
			Matrix.Compose(scale, quaternion, translation, out result);
			return result;
		}

		// Token: 0x060036A0 RID: 13984 RVA: 0x0006A65C File Offset: 0x0006885C
		public static void Compose(float scale, Quaternion quaternion, Vector3 translation, out Matrix result)
		{
			float num = quaternion.X * quaternion.X;
			float num2 = quaternion.Y * quaternion.Y;
			float num3 = quaternion.Z * quaternion.Z;
			float num4 = quaternion.X * quaternion.Y;
			float num5 = quaternion.Z * quaternion.W;
			float num6 = quaternion.Z * quaternion.X;
			float num7 = quaternion.Y * quaternion.W;
			float num8 = quaternion.Y * quaternion.Z;
			float num9 = quaternion.X * quaternion.W;
			result.M11 = scale * (1f - 2f * (num2 + num3));
			result.M12 = scale * 2f * (num4 + num5);
			result.M13 = scale * 2f * (num6 - num7);
			result.M14 = 0f;
			result.M21 = scale * 2f * (num4 - num5);
			result.M22 = scale * (1f - 2f * (num3 + num));
			result.M23 = scale * 2f * (num8 + num9);
			result.M24 = 0f;
			result.M31 = scale * 2f * (num6 + num7);
			result.M32 = scale * 2f * (num8 - num9);
			result.M33 = scale * (1f - 2f * (num2 + num));
			result.M34 = 0f;
			result.M41 = translation.X;
			result.M42 = translation.Y;
			result.M43 = translation.Z;
			result.M44 = 1f;
		}

		// Token: 0x060036A1 RID: 13985 RVA: 0x0006A7F4 File Offset: 0x000689F4
		public static void Compose(Quaternion quaternion, Vector3 translation, out Matrix result)
		{
			float num = quaternion.X * quaternion.X;
			float num2 = quaternion.Y * quaternion.Y;
			float num3 = quaternion.Z * quaternion.Z;
			float num4 = quaternion.X * quaternion.Y;
			float num5 = quaternion.Z * quaternion.W;
			float num6 = quaternion.Z * quaternion.X;
			float num7 = quaternion.Y * quaternion.W;
			float num8 = quaternion.Y * quaternion.Z;
			float num9 = quaternion.X * quaternion.W;
			result.M11 = 1f - 2f * (num2 + num3);
			result.M12 = 2f * (num4 + num5);
			result.M13 = 2f * (num6 - num7);
			result.M14 = 0f;
			result.M21 = 2f * (num4 - num5);
			result.M22 = 1f - 2f * (num3 + num);
			result.M23 = 2f * (num8 + num9);
			result.M24 = 0f;
			result.M31 = 2f * (num6 + num7);
			result.M32 = 2f * (num8 - num9);
			result.M33 = 1f - 2f * (num2 + num);
			result.M34 = 0f;
			result.M41 = translation.X;
			result.M42 = translation.Y;
			result.M43 = translation.Z;
			result.M44 = 1f;
		}

		// Token: 0x060036A2 RID: 13986 RVA: 0x0006A978 File Offset: 0x00068B78
		public static void Compose(float scaleX, float scaleY, float scaleZ, Quaternion quaternion, Vector3 translation, out Matrix result)
		{
			float num = quaternion.X * quaternion.X;
			float num2 = quaternion.Y * quaternion.Y;
			float num3 = quaternion.Z * quaternion.Z;
			float num4 = quaternion.X * quaternion.Y;
			float num5 = quaternion.Z * quaternion.W;
			float num6 = quaternion.Z * quaternion.X;
			float num7 = quaternion.Y * quaternion.W;
			float num8 = quaternion.Y * quaternion.Z;
			float num9 = quaternion.X * quaternion.W;
			result.M11 = scaleX * (1f - 2f * (num2 + num3));
			result.M12 = scaleX * 2f * (num4 + num5);
			result.M13 = scaleX * 2f * (num6 - num7);
			result.M14 = 0f;
			result.M21 = scaleY * 2f * (num4 - num5);
			result.M22 = scaleY * (1f - 2f * (num3 + num));
			result.M23 = scaleY * 2f * (num8 + num9);
			result.M24 = 0f;
			result.M31 = scaleZ * 2f * (num6 + num7);
			result.M32 = scaleZ * 2f * (num8 - num9);
			result.M33 = scaleZ * (1f - 2f * (num2 + num));
			result.M34 = 0f;
			result.M41 = translation.X;
			result.M42 = translation.Y;
			result.M43 = translation.Z;
			result.M44 = 1f;
		}

		// Token: 0x060036A3 RID: 13987 RVA: 0x0006AB24 File Offset: 0x00068D24
		public static void Compose(Vector3 scale, Quaternion quaternion, Vector3 translation, out Matrix result)
		{
			float num = quaternion.X * quaternion.X;
			float num2 = quaternion.Y * quaternion.Y;
			float num3 = quaternion.Z * quaternion.Z;
			float num4 = quaternion.X * quaternion.Y;
			float num5 = quaternion.Z * quaternion.W;
			float num6 = quaternion.Z * quaternion.X;
			float num7 = quaternion.Y * quaternion.W;
			float num8 = quaternion.Y * quaternion.Z;
			float num9 = quaternion.X * quaternion.W;
			result.M11 = scale.X * (1f - 2f * (num2 + num3));
			result.M12 = scale.X * 2f * (num4 + num5);
			result.M13 = scale.X * 2f * (num6 - num7);
			result.M14 = 0f;
			result.M21 = scale.Y * 2f * (num4 - num5);
			result.M22 = scale.Y * (1f - 2f * (num3 + num));
			result.M23 = scale.Y * 2f * (num8 + num9);
			result.M24 = 0f;
			result.M31 = scale.Z * 2f * (num6 + num7);
			result.M32 = scale.Z * 2f * (num8 - num9);
			result.M33 = scale.Z * (1f - 2f * (num2 + num));
			result.M34 = 0f;
			result.M41 = translation.X;
			result.M42 = translation.Y;
			result.M43 = translation.Z;
			result.M44 = 1f;
		}

		// Token: 0x060036A4 RID: 13988 RVA: 0x0006ACE8 File Offset: 0x00068EE8
		public static void Compose(float scale, float yaw, Vector3 translation, out Matrix result)
		{
			float num = (float)Math.Cos((double)yaw);
			float num2 = (float)Math.Sin((double)yaw);
			result.M11 = scale * num;
			result.M12 = 0f;
			result.M13 = scale * -num2;
			result.M14 = 0f;
			result.M21 = 0f;
			result.M22 = scale;
			result.M23 = 0f;
			result.M24 = 0f;
			result.M31 = scale * num2;
			result.M32 = 0f;
			result.M33 = scale * num;
			result.M34 = 0f;
			result.M41 = translation.X;
			result.M42 = translation.Y;
			result.M43 = translation.Z;
			result.M44 = 1f;
		}

		// Token: 0x060036A5 RID: 13989 RVA: 0x0006ADB0 File Offset: 0x00068FB0
		public static void Compose(Vector3 scale, float yaw, Vector3 translation, out Matrix result)
		{
			float num = (float)Math.Cos((double)yaw);
			float num2 = (float)Math.Sin((double)yaw);
			result.M11 = scale.X * num;
			result.M12 = 0f;
			result.M13 = scale.X * -num2;
			result.M14 = 0f;
			result.M21 = 0f;
			result.M22 = scale.Y;
			result.M23 = 0f;
			result.M24 = 0f;
			result.M31 = scale.Z * num2;
			result.M32 = 0f;
			result.M33 = scale.Z * num;
			result.M34 = 0f;
			result.M41 = translation.X;
			result.M42 = translation.Y;
			result.M43 = translation.Z;
			result.M44 = 1f;
		}

		// Token: 0x060036A6 RID: 13990 RVA: 0x0006AE94 File Offset: 0x00069094
		public static float[] ToFlatFloatArray(Matrix matrix)
		{
			return new float[]
			{
				matrix.M11,
				matrix.M12,
				matrix.M13,
				matrix.M14,
				matrix.M21,
				matrix.M22,
				matrix.M23,
				matrix.M24,
				matrix.M31,
				matrix.M32,
				matrix.M33,
				matrix.M34,
				matrix.M41,
				matrix.M42,
				matrix.M43,
				matrix.M44
			};
		}

		// Token: 0x060036A7 RID: 13991 RVA: 0x0006AF44 File Offset: 0x00069144
		public static void CreateLookDirection(Vector3 direction, out Matrix result)
		{
			Vector3 vector = new Vector3(0f, 1f, 0f);
			Vector3 vector2 = Vector3.Normalize(direction);
			Vector3 vector3 = Vector3.Normalize(Vector3.Cross(vector, vector2));
			Vector3 vector4 = Vector3.Cross(vector2, vector3);
			result.M11 = vector3.X;
			result.M12 = vector4.X;
			result.M13 = vector2.X;
			result.M14 = 0f;
			result.M21 = vector3.Y;
			result.M22 = vector4.Y;
			result.M23 = vector2.Y;
			result.M24 = 0f;
			result.M31 = vector3.Z;
			result.M32 = vector4.Z;
			result.M33 = vector2.Z;
			result.M34 = 0f;
			result.M41 = 0f;
			result.M42 = 0f;
			result.M43 = 0f;
			result.M44 = 1f;
		}

		// Token: 0x0400180F RID: 6159
		public float M11;

		// Token: 0x04001810 RID: 6160
		public float M12;

		// Token: 0x04001811 RID: 6161
		public float M13;

		// Token: 0x04001812 RID: 6162
		public float M14;

		// Token: 0x04001813 RID: 6163
		public float M21;

		// Token: 0x04001814 RID: 6164
		public float M22;

		// Token: 0x04001815 RID: 6165
		public float M23;

		// Token: 0x04001816 RID: 6166
		public float M24;

		// Token: 0x04001817 RID: 6167
		public float M31;

		// Token: 0x04001818 RID: 6168
		public float M32;

		// Token: 0x04001819 RID: 6169
		public float M33;

		// Token: 0x0400181A RID: 6170
		public float M34;

		// Token: 0x0400181B RID: 6171
		public float M41;

		// Token: 0x0400181C RID: 6172
		public float M42;

		// Token: 0x0400181D RID: 6173
		public float M43;

		// Token: 0x0400181E RID: 6174
		public float M44;

		// Token: 0x0400181F RID: 6175
		private static Matrix identity = new Matrix(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f);
	}
}
