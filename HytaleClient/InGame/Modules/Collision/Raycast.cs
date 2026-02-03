using System;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Collision
{
	// Token: 0x0200096B RID: 2411
	public static class Raycast
	{
		// Token: 0x06004B4E RID: 19278 RVA: 0x00136100 File Offset: 0x00134300
		public static bool RaycastBox(Ray ray, BoundingBox box, ref Raycast.Result result, ref Raycast.Options options)
		{
			return Raycast.RaycastBox(ray, box.Min, box.Max, ref result, ref options);
		}

		// Token: 0x06004B4F RID: 19279 RVA: 0x00136128 File Offset: 0x00134328
		public static bool RaycastBox(Ray ray, BoundingBox box, Vector3 offset, ref Raycast.Result result, ref Raycast.Options options)
		{
			Vector3 min = box.Min + offset;
			Vector3 max = box.Max + offset;
			return Raycast.RaycastBox(ray, min, max, ref result, ref options);
		}

		// Token: 0x06004B50 RID: 19280 RVA: 0x00136160 File Offset: 0x00134360
		public static bool RaycastBox(Ray ray, Vector3 min, Vector3 max, ref Raycast.Result result, ref Raycast.Options options)
		{
			Vector3 position = ray.Position;
			Vector3 direction = ray.Direction;
			bool flag = false;
			float num = (min.X - position.X) / direction.X;
			bool flag2 = num < result.NearT && num > options.Epsilon;
			if (flag2)
			{
				float num2 = position.Z + direction.Z * num;
				float num3 = position.Y + direction.Y * num;
				bool flag3 = num2 >= min.Z && num2 <= max.Z && num3 >= min.Y && num3 <= max.Y;
				if (flag3)
				{
					result.NearT = num;
					result.TextureCoord.X = num2;
					result.TextureCoord.Y = num3;
					result.Normal = IntVector3.Left;
					flag = true;
				}
			}
			num = (max.X - position.X) / direction.X;
			bool flag4 = num < result.NearT && num > options.Epsilon;
			if (flag4)
			{
				float num2 = position.Z + direction.Z * num;
				float num3 = position.Y + direction.Y * num;
				bool flag5 = num2 >= min.Z && num2 <= max.Z && num3 >= min.Y && num3 <= max.Y;
				if (flag5)
				{
					result.NearT = num;
					result.TextureCoord.X = num2;
					result.TextureCoord.Y = num3;
					result.Normal = IntVector3.Right;
					flag = true;
				}
			}
			num = (min.Y - position.Y) / direction.Y;
			bool flag6 = num < result.NearT && num > options.Epsilon;
			if (flag6)
			{
				float num2 = position.X + direction.X * num;
				float num3 = position.Z + direction.Z * num;
				bool flag7 = num2 >= min.X && num2 <= max.X && num3 >= min.Z && num3 <= max.Z;
				if (flag7)
				{
					result.NearT = num;
					result.TextureCoord.X = num2;
					result.TextureCoord.Y = num3;
					result.Normal = IntVector3.Down;
					flag = true;
				}
			}
			num = (max.Y - position.Y) / direction.Y;
			bool flag8 = num < result.NearT && num > options.Epsilon;
			if (flag8)
			{
				float num2 = position.X + direction.X * num;
				float num3 = position.Z + direction.Z * num;
				bool flag9 = num2 >= min.X && num2 <= max.X && num3 >= min.Z && num3 <= max.Z;
				if (flag9)
				{
					result.NearT = num;
					result.TextureCoord.X = num2;
					result.TextureCoord.Y = num3;
					result.Normal = IntVector3.Up;
					flag = true;
				}
			}
			num = (min.Z - position.Z) / direction.Z;
			bool flag10 = num < result.NearT && num > options.Epsilon;
			if (flag10)
			{
				float num2 = position.X + direction.X * num;
				float num3 = position.Y + direction.Y * num;
				bool flag11 = num2 >= min.X && num2 <= max.X && num3 >= min.Y && num3 <= max.Y;
				if (flag11)
				{
					result.NearT = num;
					result.TextureCoord.X = num2;
					result.TextureCoord.Y = num3;
					result.Normal = IntVector3.Forward;
					flag = true;
				}
			}
			num = (max.Z - position.Z) / direction.Z;
			bool flag12 = num < result.NearT && num > options.Epsilon;
			if (flag12)
			{
				float num2 = position.X + direction.X * num;
				float num3 = position.Y + direction.Y * num;
				bool flag13 = num2 >= min.X && num2 <= max.X && num3 >= min.Y && num3 <= max.Y;
				if (flag13)
				{
					result.NearT = num;
					result.TextureCoord.X = num2;
					result.TextureCoord.Y = num3;
					result.Normal = IntVector3.Backward;
					flag = true;
				}
			}
			bool flag14 = flag;
			if (flag14)
			{
				result.Ray = ray;
			}
			return flag;
		}

		// Token: 0x06004B51 RID: 19281 RVA: 0x001365EC File Offset: 0x001347EC
		public static bool RaycastBox(Ray ray, BoundingBox box, ref Raycast.Result result)
		{
			return Raycast.RaycastBox(ray, box, ref result, ref Raycast.Options.Default);
		}

		// Token: 0x06004B52 RID: 19282 RVA: 0x001365FB File Offset: 0x001347FB
		public static bool RaycastBox(Ray ray, BoundingBox box, Vector3 offset, ref Raycast.Result result)
		{
			return Raycast.RaycastBox(ray, box, offset, ref result, ref Raycast.Options.Default);
		}

		// Token: 0x06004B53 RID: 19283 RVA: 0x0013660C File Offset: 0x0013480C
		public static bool RaycastBoxSingle(Ray ray, BoundingBox box, out Raycast.Result result)
		{
			result = Raycast.Result.Default;
			return Raycast.RaycastBox(ray, box, ref result, ref Raycast.Options.Default);
		}

		// Token: 0x06004B54 RID: 19284 RVA: 0x00136638 File Offset: 0x00134838
		public static bool RaycastBoxSingle(Ray ray, BoundingBox box, Vector3 offset, out Raycast.Result result)
		{
			result = Raycast.Result.Default;
			return Raycast.RaycastBox(ray, box, offset, ref result, ref Raycast.Options.Default);
		}

		// Token: 0x06004B55 RID: 19285 RVA: 0x00136664 File Offset: 0x00134864
		public static bool RaycastBoxSingle(Ray ray, BoundingBox box, out Raycast.Result result, ref Raycast.Options options)
		{
			result = Raycast.Result.Default;
			return Raycast.RaycastBox(ray, box, ref result, ref options);
		}

		// Token: 0x06004B56 RID: 19286 RVA: 0x0013668C File Offset: 0x0013488C
		public static bool RaycastBoxSingle(Ray ray, BoundingBox box, Vector3 offset, out Raycast.Result result, ref Raycast.Options options)
		{
			result = Raycast.Result.Default;
			return Raycast.RaycastBox(ray, box, offset, ref result, ref options);
		}

		// Token: 0x02000E5C RID: 3676
		public struct Result
		{
			// Token: 0x1700146B RID: 5227
			// (get) Token: 0x06006786 RID: 26502 RVA: 0x002180EB File Offset: 0x002162EB
			public static Raycast.Result Default { get; } = new Raycast.Result
			{
				Ray = default(Ray),
				NearT = float.PositiveInfinity,
				Normal = IntVector3.Zero,
				TextureCoord = Vector2.Zero
			};

			// Token: 0x06006787 RID: 26503 RVA: 0x002180F4 File Offset: 0x002162F4
			public bool IsSuccess()
			{
				return !float.IsPositiveInfinity(this.NearT);
			}

			// Token: 0x06006788 RID: 26504 RVA: 0x00218114 File Offset: 0x00216314
			public Vector3 GetTarget()
			{
				return this.Ray.GetAt(this.NearT);
			}

			// Token: 0x06006789 RID: 26505 RVA: 0x00218138 File Offset: 0x00216338
			public float Distance()
			{
				return this.Ray.Direction.Length() * this.NearT;
			}

			// Token: 0x04004629 RID: 17961
			public Ray Ray;

			// Token: 0x0400462A RID: 17962
			public float NearT;

			// Token: 0x0400462B RID: 17963
			public IntVector3 Normal;

			// Token: 0x0400462C RID: 17964
			public Vector2 TextureCoord;
		}

		// Token: 0x02000E5D RID: 3677
		public struct Options
		{
			// Token: 0x0600678B RID: 26507 RVA: 0x002181B0 File Offset: 0x002163B0
			public Options(float distance)
			{
				this = Raycast.Options.Default;
				this.Distance = distance;
			}

			// Token: 0x0600678C RID: 26508 RVA: 0x002181C5 File Offset: 0x002163C5
			public void SetBidirectional(bool state)
			{
				this.Epsilon = (state ? float.NegativeInfinity : 0f);
			}

			// Token: 0x0400462D RID: 17965
			public static Raycast.Options Default = new Raycast.Options
			{
				Distance = 128f,
				Epsilon = 0f
			};

			// Token: 0x0400462E RID: 17966
			public const float DefaultDistance = 128f;

			// Token: 0x0400462F RID: 17967
			public const float DefaultEpsilon = 0f;

			// Token: 0x04004630 RID: 17968
			public float Distance;

			// Token: 0x04004631 RID: 17969
			public float Epsilon;
		}
	}
}
