using System;
using System.Runtime.CompilerServices;
using HytaleClient.Data.Map;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Utils
{
	// Token: 0x020007CC RID: 1996
	internal static class RotationHelper
	{
		// Token: 0x06003419 RID: 13337 RVA: 0x00053124 File Offset: 0x00051324
		public static Rotation Add(Rotation a, Rotation b)
		{
			int num = a + b;
			return num % typeof(Rotation).GetEnumValues().Length;
		}

		// Token: 0x0600341A RID: 13338 RVA: 0x00053154 File Offset: 0x00051354
		public static Rotation Subtract(Rotation a, Rotation b)
		{
			int num = a - b;
			bool flag = num < 0;
			if (flag)
			{
				num += typeof(Rotation).GetEnumValues().Length;
			}
			return num;
		}

		// Token: 0x0600341B RID: 13339 RVA: 0x0005318C File Offset: 0x0005138C
		public static void Rotate(int x, int z, Rotation rotation, out int outX, out int outZ)
		{
			switch (rotation)
			{
			case 0:
				outX = x;
				outZ = z;
				break;
			case 1:
				outX = z;
				outZ = -x;
				break;
			case 2:
				outX = -x;
				outZ = -z;
				break;
			case 3:
				outX = -z;
				outZ = x;
				break;
			default:
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600341C RID: 13340 RVA: 0x000531E4 File Offset: 0x000513E4
		public static void GetHorizontalNormal(Vector3 rotation, out float x, out float z)
		{
			bool flag = rotation.Y >= -0.7853982f && rotation.Y <= 0.7853982f;
			if (flag)
			{
				x = 0f;
				z = 1f;
			}
			else
			{
				bool flag2 = rotation.Y >= 0.7853982f && rotation.Y <= 2.3561945f;
				if (flag2)
				{
					x = 1f;
					z = 0f;
				}
				else
				{
					bool flag3 = rotation.Y >= 2.3561945f || rotation.Y <= -2.3561945f;
					if (flag3)
					{
						x = 0f;
						z = -1f;
					}
					else
					{
						x = -1f;
						z = 0f;
					}
				}
			}
		}

		// Token: 0x0600341D RID: 13341 RVA: 0x000532A4 File Offset: 0x000514A4
		public static void GetVariantRotationOptions(Vector3 targetNormal, BlockType.VariantRotation rotation, out bool rotateX, out bool rotateY, out Rotation defaultPitch, out Rotation[] yawValues)
		{
			rotateX = false;
			rotateY = false;
			defaultPitch = 0;
			switch (rotation)
			{
			case 0:
				yawValues = RotationHelper._rotationNoneYawValues;
				break;
			case 1:
				rotateX = true;
				yawValues = RotationHelper._rotationWallYawValues;
				break;
			case 2:
				rotateY = true;
				yawValues = RotationHelper._rotationNoneYawValues;
				break;
			case 3:
			{
				bool flag = targetNormal.Y == 0f;
				if (flag)
				{
					rotateX = true;
					defaultPitch = 1;
					yawValues = RotationHelper._rotationWallYawValues;
				}
				else
				{
					yawValues = RotationHelper._rotationNESWYawValues;
				}
				break;
			}
			case 4:
			{
				bool flag2 = targetNormal.Y == 0f;
				if (flag2)
				{
					rotateX = true;
					defaultPitch = 1;
					yawValues = RotationHelper._rotationNESWYawValues;
				}
				else
				{
					rotateY = true;
					yawValues = RotationHelper._rotationNESWYawValues;
				}
				break;
			}
			case 5:
				rotateX = true;
				yawValues = RotationHelper._rotationNESWYawValues;
				break;
			case 6:
				rotateX = true;
				rotateY = true;
				yawValues = RotationHelper._rotationNESWYawValues;
				break;
			case 7:
				rotateX = true;
				rotateY = true;
				yawValues = RotationHelper._rotationNESWYawValues;
				break;
			default:
				throw new NotImplementedException(string.Format("Unknown BlockType.VariantRotation type: {0}", rotation));
			}
		}

		// Token: 0x0600341E RID: 13342 RVA: 0x000533BC File Offset: 0x000515BC
		public static BlockFace FromNormal(Vector3 normal)
		{
			bool flag = (double)Math.Abs(normal.X) > 0.5;
			BlockFace result;
			if (flag)
			{
				result = ((normal.X < 0f) ? 6 : 5);
			}
			else
			{
				bool flag2 = (double)Math.Abs(normal.Z) > 0.5;
				if (flag2)
				{
					result = ((normal.Z < 0f) ? 3 : 4);
				}
				else
				{
					result = ((normal.Y < 0f) ? 2 : 1);
				}
			}
			return result;
		}

		// Token: 0x0600341F RID: 13343 RVA: 0x00053440 File Offset: 0x00051640
		public static Vector3 ToNormal(BlockFace face)
		{
			Vector3 result;
			switch (face)
			{
			case 1:
				result = new Vector3(0f, 1f, 0f);
				break;
			case 2:
				result = new Vector3(0f, -1f, 0f);
				break;
			case 3:
				result = new Vector3(0f, 0f, -1f);
				break;
			case 4:
				result = new Vector3(0f, 0f, 1f);
				break;
			case 5:
				result = new Vector3(1f, 0f, 0f);
				break;
			case 6:
				result = new Vector3(-1f, 0f, 0f);
				break;
			default:
				throw new Exception("Invalid block face");
			}
			return result;
		}

		// Token: 0x06003420 RID: 13344 RVA: 0x00053510 File Offset: 0x00051710
		public static BlockFace RotateBlockFace(BlockFace face, ClientBlockType block)
		{
			Vector3 position = RotationHelper.ToNormal(face);
			Vector3 normal = Vector3.Transform(position, block.RotationMatrix);
			return RotationHelper.FromNormal(normal);
		}

		// Token: 0x06003421 RID: 13345 RVA: 0x0005353C File Offset: 0x0005173C
		// Note: this type is marked as 'beforefieldinit'.
		static RotationHelper()
		{
			Rotation[] array = new Rotation[4];
			RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.BAED642339816AFFB3FE8719792D0E4CE82F12DB72B7373D244EAA65445800FE).FieldHandle);
			RotationHelper._rotationNESWYawValues = array;
		}

		// Token: 0x04001758 RID: 5976
		private static readonly Rotation[] _rotationWallYawValues = new Rotation[]
		{
			default(Rotation),
			1,
			default(Rotation),
			1
		};

		// Token: 0x04001759 RID: 5977
		private static readonly Rotation[] _rotationNoneYawValues = new Rotation[4];

		// Token: 0x0400175A RID: 5978
		private static readonly Rotation[] _rotationNESWYawValues;
	}
}
