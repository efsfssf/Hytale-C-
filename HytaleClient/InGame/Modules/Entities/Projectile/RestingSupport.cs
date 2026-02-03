using System;
using HytaleClient.InGame.Modules.Map;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x02000965 RID: 2405
	internal class RestingSupport
	{
		// Token: 0x06004B27 RID: 19239 RVA: 0x00134768 File Offset: 0x00132968
		public bool HasChanged(GameInstance gameInstance)
		{
			bool flag = this._supportBlocks == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				int num = 0;
				for (int i = this._supportMinZ; i <= this._supportMaxZ; i++)
				{
					for (int j = this._supportMinX; j <= this._supportMaxX; j++)
					{
						ChunkColumn chunkColumn = gameInstance.MapModule.GetChunkColumn(j, i);
						bool flag2 = chunkColumn != null;
						if (flag2)
						{
							for (int k = this._supportMinY; k <= this._supportMaxY; k++)
							{
								bool flag3 = this._supportBlocks[num++] != gameInstance.MapModule.GetBlock(j, k, i, int.MaxValue);
								if (flag3)
								{
									return true;
								}
							}
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06004B28 RID: 19240 RVA: 0x00134854 File Offset: 0x00132A54
		public void Rest(GameInstance gameInstance, BoundingBox boundingBox, Vector3 position)
		{
			bool flag = this._supportBlocks == null;
			if (flag)
			{
				Vector3 size = boundingBox.GetSize();
				int num = (int)(Math.Ceiling((double)(size.X + 1f)) * Math.Ceiling((double)(size.Z + 1f)) * Math.Ceiling((double)(size.Y + 1f)));
				this._supportBlocks = new int[num];
			}
			this._supportMinX = (int)Math.Floor((double)(position.X + boundingBox.Min.X));
			this._supportMaxX = (int)Math.Floor((double)(position.X + boundingBox.Max.X));
			this._supportMinZ = (int)Math.Floor((double)(position.Z + boundingBox.Min.Z));
			this._supportMaxZ = (int)Math.Floor((double)(position.Z + boundingBox.Max.Z));
			this._supportMinY = (int)Math.Floor((double)(position.Y + boundingBox.Min.Y));
			this._supportMaxY = (int)Math.Floor((double)(position.Y + boundingBox.Max.Y));
			int num2 = 0;
			for (int i = this._supportMinZ; i <= this._supportMaxZ; i++)
			{
				for (int j = this._supportMinX; j <= this._supportMaxX; j++)
				{
					ChunkColumn chunkColumn = gameInstance.MapModule.GetChunkColumn(j, i);
					bool flag2 = chunkColumn != null;
					if (flag2)
					{
						for (int k = this._supportMinY; k <= this._supportMaxY; k++)
						{
							this._supportBlocks[num2++] = gameInstance.MapModule.GetBlock(j, k, i, int.MaxValue);
						}
					}
					else
					{
						for (int l = this._supportMinY; l <= this._supportMaxY; l++)
						{
							this._supportBlocks[num2++] = 1;
						}
					}
				}
			}
		}

		// Token: 0x06004B29 RID: 19241 RVA: 0x00134A6C File Offset: 0x00132C6C
		public void Clear()
		{
			this._supportBlocks = null;
		}

		// Token: 0x040026D7 RID: 9943
		protected int _supportMinX;

		// Token: 0x040026D8 RID: 9944
		protected int _supportMaxX;

		// Token: 0x040026D9 RID: 9945
		protected int _supportMinY;

		// Token: 0x040026DA RID: 9946
		protected int _supportMaxY;

		// Token: 0x040026DB RID: 9947
		protected int _supportMinZ;

		// Token: 0x040026DC RID: 9948
		protected int _supportMaxZ;

		// Token: 0x040026DD RID: 9949
		protected int[] _supportBlocks;
	}
}
