using System;
using System.Diagnostics;

namespace HytaleClient.Math
{
	// Token: 0x020007F4 RID: 2036
	[DebuggerDisplay("{DebugDisplayString,nq}")]
	[Serializable]
	public struct Ray : IEquatable<Ray>
	{
		// Token: 0x17000FDC RID: 4060
		// (get) Token: 0x06003729 RID: 14121 RVA: 0x0006D4DC File Offset: 0x0006B6DC
		internal string DebugDisplayString
		{
			get
			{
				return string.Concat(new string[]
				{
					"Pos( ",
					this.Position.DebugDisplayString,
					" ) \r\n",
					"Dir( ",
					this.Direction.DebugDisplayString,
					" )"
				});
			}
		}

		// Token: 0x0600372A RID: 14122 RVA: 0x0006D535 File Offset: 0x0006B735
		public Ray(Vector3 position, Vector3 direction)
		{
			this.Position = position;
			this.Direction = direction;
		}

		// Token: 0x0600372B RID: 14123 RVA: 0x0006D548 File Offset: 0x0006B748
		public override bool Equals(object obj)
		{
			return obj is Ray && this.Equals((Ray)obj);
		}

		// Token: 0x0600372C RID: 14124 RVA: 0x0006D574 File Offset: 0x0006B774
		public bool Equals(Ray other)
		{
			return this.Position.Equals(other.Position) && this.Direction.Equals(other.Direction);
		}

		// Token: 0x0600372D RID: 14125 RVA: 0x0006D5B0 File Offset: 0x0006B7B0
		public override int GetHashCode()
		{
			return this.Position.GetHashCode() ^ this.Direction.GetHashCode();
		}

		// Token: 0x0600372E RID: 14126 RVA: 0x0006D5E8 File Offset: 0x0006B7E8
		public float? Intersects(BoundingBox box)
		{
			float? num = null;
			float? num2 = null;
			bool flag = MathHelper.WithinEpsilon(this.Direction.X, 0f);
			if (flag)
			{
				bool flag2 = this.Position.X < box.Min.X || this.Position.X > box.Max.X;
				if (flag2)
				{
					return null;
				}
			}
			else
			{
				num = new float?((box.Min.X - this.Position.X) / this.Direction.X);
				num2 = new float?((box.Max.X - this.Position.X) / this.Direction.X);
				float? num3 = num;
				float? num4 = num2;
				bool flag3 = num3.GetValueOrDefault() > num4.GetValueOrDefault() & (num3 != null & num4 != null);
				if (flag3)
				{
					float? num5 = num;
					num = num2;
					num2 = num5;
				}
			}
			bool flag4 = MathHelper.WithinEpsilon(this.Direction.Y, 0f);
			if (flag4)
			{
				bool flag5 = this.Position.Y < box.Min.Y || this.Position.Y > box.Max.Y;
				if (flag5)
				{
					return null;
				}
			}
			else
			{
				float num6 = (box.Min.Y - this.Position.Y) / this.Direction.Y;
				float num7 = (box.Max.Y - this.Position.Y) / this.Direction.Y;
				bool flag6 = num6 > num7;
				if (flag6)
				{
					float num8 = num6;
					num6 = num7;
					num7 = num8;
				}
				bool flag7;
				if (num != null)
				{
					float? num4 = num;
					float num9 = num7;
					if (num4.GetValueOrDefault() > num9 & num4 != null)
					{
						flag7 = true;
						goto IL_222;
					}
				}
				if (num2 != null)
				{
					float num10 = num6;
					float? num4 = num2;
					flag7 = (num10 > num4.GetValueOrDefault() & num4 != null);
				}
				else
				{
					flag7 = false;
				}
				IL_222:
				bool flag8 = flag7;
				if (flag8)
				{
					return null;
				}
				bool flag9;
				if (num != null)
				{
					float num11 = num6;
					float? num4 = num;
					flag9 = (num11 > num4.GetValueOrDefault() & num4 != null);
				}
				else
				{
					flag9 = true;
				}
				bool flag10 = flag9;
				if (flag10)
				{
					num = new float?(num6);
				}
				bool flag11;
				if (num2 != null)
				{
					float num12 = num7;
					float? num4 = num2;
					flag11 = (num12 < num4.GetValueOrDefault() & num4 != null);
				}
				else
				{
					flag11 = true;
				}
				bool flag12 = flag11;
				if (flag12)
				{
					num2 = new float?(num7);
				}
			}
			bool flag13 = MathHelper.WithinEpsilon(this.Direction.Z, 0f);
			if (flag13)
			{
				bool flag14 = this.Position.Z < box.Min.Z || this.Position.Z > box.Max.Z;
				if (flag14)
				{
					return null;
				}
			}
			else
			{
				float num13 = (box.Min.Z - this.Position.Z) / this.Direction.Z;
				float num14 = (box.Max.Z - this.Position.Z) / this.Direction.Z;
				bool flag15 = num13 > num14;
				if (flag15)
				{
					float num15 = num13;
					num13 = num14;
					num14 = num15;
				}
				bool flag16;
				if (num != null)
				{
					float? num4 = num;
					float num9 = num14;
					if (num4.GetValueOrDefault() > num9 & num4 != null)
					{
						flag16 = true;
						goto IL_3B9;
					}
				}
				if (num2 != null)
				{
					float num16 = num13;
					float? num4 = num2;
					flag16 = (num16 > num4.GetValueOrDefault() & num4 != null);
				}
				else
				{
					flag16 = false;
				}
				IL_3B9:
				bool flag17 = flag16;
				if (flag17)
				{
					return null;
				}
				bool flag18;
				if (num != null)
				{
					float num17 = num13;
					float? num4 = num;
					flag18 = (num17 > num4.GetValueOrDefault() & num4 != null);
				}
				else
				{
					flag18 = true;
				}
				bool flag19 = flag18;
				if (flag19)
				{
					num = new float?(num13);
				}
				bool flag20;
				if (num2 != null)
				{
					float num18 = num14;
					float? num4 = num2;
					flag20 = (num18 < num4.GetValueOrDefault() & num4 != null);
				}
				else
				{
					flag20 = true;
				}
				bool flag21 = flag20;
				if (flag21)
				{
					num2 = new float?(num14);
				}
			}
			bool flag22;
			if (num != null)
			{
				float? num4 = num;
				float num9 = 0f;
				if (num4.GetValueOrDefault() < num9 & num4 != null)
				{
					num4 = num2;
					num9 = 0f;
					flag22 = (num4.GetValueOrDefault() > num9 & num4 != null);
					goto IL_47C;
				}
			}
			flag22 = false;
			IL_47C:
			bool flag23 = flag22;
			float? result;
			if (flag23)
			{
				result = new float?(0f);
			}
			else
			{
				float? num4 = num;
				float num9 = 0f;
				bool flag24 = num4.GetValueOrDefault() < num9 & num4 != null;
				if (flag24)
				{
					result = null;
				}
				else
				{
					result = num;
				}
			}
			return result;
		}

		// Token: 0x0600372F RID: 14127 RVA: 0x0006DABD File Offset: 0x0006BCBD
		public void Intersects(ref BoundingBox box, out float? result)
		{
			result = this.Intersects(box);
		}

		// Token: 0x06003730 RID: 14128 RVA: 0x0006DAD4 File Offset: 0x0006BCD4
		public float? Intersects(BoundingSphere sphere)
		{
			float? result;
			this.Intersects(ref sphere, out result);
			return result;
		}

		// Token: 0x06003731 RID: 14129 RVA: 0x0006DAF4 File Offset: 0x0006BCF4
		public float? Intersects(Plane plane)
		{
			float? result;
			this.Intersects(ref plane, out result);
			return result;
		}

		// Token: 0x06003732 RID: 14130 RVA: 0x0006DB14 File Offset: 0x0006BD14
		public float? Intersects(BoundingFrustum frustum)
		{
			float? result;
			frustum.Intersects(ref this, out result);
			return result;
		}

		// Token: 0x06003733 RID: 14131 RVA: 0x0006DB34 File Offset: 0x0006BD34
		public void Intersects(ref Plane plane, out float? result)
		{
			float num = Vector3.Dot(this.Direction, plane.Normal);
			bool flag = Math.Abs(num) < 1E-05f;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = new float?((-plane.D - Vector3.Dot(plane.Normal, this.Position)) / num);
				float? num2 = result;
				float num3 = 0f;
				bool flag2 = num2.GetValueOrDefault() < num3 & num2 != null;
				if (flag2)
				{
					num2 = result;
					num3 = -1E-05f;
					bool flag3 = num2.GetValueOrDefault() < num3 & num2 != null;
					if (flag3)
					{
						result = null;
					}
					else
					{
						result = new float?(0f);
					}
				}
			}
		}

		// Token: 0x06003734 RID: 14132 RVA: 0x0006DC00 File Offset: 0x0006BE00
		public void Intersects(ref BoundingSphere sphere, out float? result)
		{
			Vector3 vector = sphere.Center - this.Position;
			float num = vector.LengthSquared();
			float num2 = sphere.Radius * sphere.Radius;
			bool flag = num < num2;
			if (flag)
			{
				result = new float?(0f);
			}
			else
			{
				float num3;
				Vector3.Dot(ref this.Direction, ref vector, out num3);
				bool flag2 = num3 < 0f;
				if (flag2)
				{
					result = null;
				}
				else
				{
					float num4 = num2 + num3 * num3 - num;
					result = ((num4 < 0f) ? null : new float?(num3 - (float)Math.Sqrt((double)num4)));
				}
			}
		}

		// Token: 0x06003735 RID: 14133 RVA: 0x0006DCB0 File Offset: 0x0006BEB0
		public static bool operator !=(Ray a, Ray b)
		{
			return !a.Equals(b);
		}

		// Token: 0x06003736 RID: 14134 RVA: 0x0006DCD0 File Offset: 0x0006BED0
		public static bool operator ==(Ray a, Ray b)
		{
			return a.Equals(b);
		}

		// Token: 0x06003737 RID: 14135 RVA: 0x0006DCEC File Offset: 0x0006BEEC
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{{Position:",
				this.Position.ToString(),
				" Direction:",
				this.Direction.ToString(),
				"}}"
			});
		}

		// Token: 0x04001842 RID: 6210
		public Vector3 Position;

		// Token: 0x04001843 RID: 6211
		public Vector3 Direction;
	}
}
