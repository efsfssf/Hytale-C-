using System;

namespace HytaleClient.Math
{
	// Token: 0x020007DE RID: 2014
	public class BlockHitbox : IEquatable<BlockHitbox>
	{
		// Token: 0x0600352E RID: 13614 RVA: 0x0005F1CA File Offset: 0x0005D3CA
		public BlockHitbox()
		{
			this.BoundingBox = new BoundingBox(Vector3.Zero, Vector3.One);
			this.Boxes = new BoundingBox[]
			{
				this.BoundingBox
			};
		}

		// Token: 0x0600352F RID: 13615 RVA: 0x0005F204 File Offset: 0x0005D404
		public BlockHitbox(BoundingBox[] boxes)
		{
			this.Boxes = boxes;
			this.BoundingBox = new BoundingBox(new Vector3(float.MaxValue), new Vector3(float.MinValue));
			foreach (BoundingBox boundingBox in this.Boxes)
			{
				this.BoundingBox.Min = Vector3.Min(this.BoundingBox.Min, boundingBox.Min);
				this.BoundingBox.Max = Vector3.Max(this.BoundingBox.Max, boundingBox.Max);
			}
		}

		// Token: 0x06003530 RID: 13616 RVA: 0x0005F2A4 File Offset: 0x0005D4A4
		public BlockHitbox(float[][] boxes)
		{
			this.Boxes = new BoundingBox[boxes.Length];
			for (int i = 0; i < boxes.Length; i++)
			{
				this.Boxes[i].Min = new Vector3(boxes[i][0], boxes[i][1], boxes[i][2]);
				this.Boxes[i].Max = new Vector3(boxes[i][3], boxes[i][4], boxes[i][5]);
			}
			this.BoundingBox = new BoundingBox(new Vector3(float.MaxValue), new Vector3(float.MinValue));
			foreach (BoundingBox boundingBox in this.Boxes)
			{
				this.BoundingBox.Min = Vector3.Min(this.BoundingBox.Min, boundingBox.Min);
				this.BoundingBox.Max = Vector3.Max(this.BoundingBox.Max, boundingBox.Max);
			}
		}

		// Token: 0x06003531 RID: 13617 RVA: 0x0005F3A9 File Offset: 0x0005D5A9
		public void Rotate(int pitch, int yaw)
		{
			BlockHitbox.RotateBoxes(this.Boxes, pitch, yaw);
		}

		// Token: 0x06003532 RID: 13618 RVA: 0x0005F3BC File Offset: 0x0005D5BC
		public void Translate(Vector3 offset)
		{
			for (int i = 0; i < this.Boxes.Length; i++)
			{
				this.Boxes[i].Translate(offset);
			}
		}

		// Token: 0x06003533 RID: 13619 RVA: 0x0005F3F4 File Offset: 0x0005D5F4
		public BoundingBox GetVoxelBounds()
		{
			return new BoundingBox(new Vector3((float)Math.Floor((double)this.BoundingBox.Min.X), (float)Math.Floor((double)this.BoundingBox.Min.Y), (float)Math.Floor((double)this.BoundingBox.Min.Z)), new Vector3((float)Math.Ceiling((double)this.BoundingBox.Max.X), (float)Math.Ceiling((double)this.BoundingBox.Max.Y), (float)Math.Ceiling((double)this.BoundingBox.Max.Z)));
		}

		// Token: 0x06003534 RID: 13620 RVA: 0x0005F4A0 File Offset: 0x0005D6A0
		public bool IsOversized()
		{
			return this.BoundingBox.Min.X < 0f || this.BoundingBox.Min.Y < 0f || this.BoundingBox.Min.Z < 0f || this.BoundingBox.Max.X > 1f || this.BoundingBox.Max.Y > 1f || this.BoundingBox.Max.Z > 1f;
		}

		// Token: 0x06003535 RID: 13621 RVA: 0x0005F540 File Offset: 0x0005D740
		public BlockHitbox Clone()
		{
			BoundingBox[] array = new BoundingBox[this.Boxes.Length];
			for (int i = 0; i < this.Boxes.Length; i++)
			{
				array[i] = this.Boxes[i];
			}
			return new BlockHitbox(array);
		}

		// Token: 0x06003536 RID: 13622 RVA: 0x0005F590 File Offset: 0x0005D790
		public static void RotateBoxes(BoundingBox[] boxes, int pitch, int yaw)
		{
			if (pitch != 0)
			{
				if (pitch != 90)
				{
					if (pitch != 180)
					{
						throw new Exception("Unsupported pitch for BlockHitbox.RotateBoxes");
					}
					for (int i = 0; i < boxes.Length; i++)
					{
						float y = boxes[i].Min.Y;
						float y2 = boxes[i].Max.Y;
						float z = boxes[i].Min.Z;
						float z2 = boxes[i].Max.Z;
						boxes[i].Min.Y = 1f - y2;
						boxes[i].Max.Y = 1f - y;
						boxes[i].Min.Z = 1f - z2;
						boxes[i].Max.Z = 1f - z;
					}
				}
				else
				{
					for (int j = 0; j < boxes.Length; j++)
					{
						float y3 = boxes[j].Min.Y;
						float y4 = boxes[j].Max.Y;
						float z3 = boxes[j].Min.Z;
						float z4 = boxes[j].Max.Z;
						boxes[j].Min.Y = z3;
						boxes[j].Min.Z = y3;
						boxes[j].Max.Y = z4;
						boxes[j].Max.Z = y4;
					}
				}
			}
			if (yaw <= 90)
			{
				if (yaw == 0)
				{
					return;
				}
				if (yaw == 90)
				{
					for (int k = 0; k < boxes.Length; k++)
					{
						float x = boxes[k].Min.X;
						float x2 = boxes[k].Max.X;
						float z5 = boxes[k].Min.Z;
						float z6 = boxes[k].Max.Z;
						boxes[k].Min.X = z5;
						boxes[k].Min.Z = 1f - x2;
						boxes[k].Max.X = z6;
						boxes[k].Max.Z = 1f - x;
					}
					return;
				}
			}
			else
			{
				if (yaw == 180)
				{
					for (int l = 0; l < boxes.Length; l++)
					{
						float x3 = boxes[l].Min.X;
						float x4 = boxes[l].Max.X;
						float z7 = boxes[l].Min.Z;
						float z8 = boxes[l].Max.Z;
						boxes[l].Min.X = 1f - x4;
						boxes[l].Max.X = 1f - x3;
						boxes[l].Min.Z = 1f - z8;
						boxes[l].Max.Z = 1f - z7;
					}
					return;
				}
				if (yaw == 270)
				{
					for (int m = 0; m < boxes.Length; m++)
					{
						float x5 = boxes[m].Min.X;
						float x6 = boxes[m].Max.X;
						float z9 = boxes[m].Min.Z;
						float z10 = boxes[m].Max.Z;
						boxes[m].Min.X = 1f - z10;
						boxes[m].Min.Z = x5;
						boxes[m].Max.X = 1f - z9;
						boxes[m].Max.Z = x6;
					}
					return;
				}
			}
			throw new Exception("Unsupported yaw for BlockHitbox.RotateBoxes");
		}

		// Token: 0x06003537 RID: 13623 RVA: 0x0005FA28 File Offset: 0x0005DC28
		public bool Equals(BlockHitbox other)
		{
			bool flag = !this.BoundingBox.Equals(other.BoundingBox) || this.Boxes.Length != other.Boxes.Length;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < this.Boxes.Length; i++)
				{
					bool flag2 = !this.Boxes[i].Equals(other.Boxes[i]);
					if (flag2)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x040017DA RID: 6106
		public BoundingBox BoundingBox;

		// Token: 0x040017DB RID: 6107
		public BoundingBox[] Boxes;
	}
}
