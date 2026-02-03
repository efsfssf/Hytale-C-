using System;
using HytaleClient.Data.Map;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Map
{
	// Token: 0x02000905 RID: 2309
	internal struct BlockAccessor
	{
		// Token: 0x0600456F RID: 17775 RVA: 0x000F3DF5 File Offset: 0x000F1FF5
		public BlockAccessor(MapModule mapModule)
		{
			this._mapModule = mapModule;
			this._chunk = null;
		}

		// Token: 0x06004570 RID: 17776 RVA: 0x000F3E08 File Offset: 0x000F2008
		public int GetBlockId(IntVector3 block)
		{
			int num = block.X >> 5;
			int num2 = block.Y >> 5;
			int num3 = block.Z >> 5;
			bool flag = this._chunk == null || this._chunk.X != num || this._chunk.Y != num2 || this._chunk.Z != num3;
			if (flag)
			{
				this._chunk = this._mapModule.GetChunk(num, num2, num3);
			}
			bool flag2 = this._chunk == null;
			int result;
			if (flag2)
			{
				result = 0;
			}
			else
			{
				result = this._chunk.Data.GetBlock(block.X, block.Y, block.Z);
			}
			return result;
		}

		// Token: 0x06004571 RID: 17777 RVA: 0x000F3EC0 File Offset: 0x000F20C0
		public ClientBlockType GetBlockType(IntVector3 block)
		{
			return this._mapModule.ClientBlockTypes[this.GetBlockId(block)];
		}

		// Token: 0x06004572 RID: 17778 RVA: 0x000F3EE8 File Offset: 0x000F20E8
		public int GetBlockIdFiller(IntVector3 block)
		{
			ClientBlockType blockType = this.GetBlockType(block);
			int id = blockType.Id;
			bool flag = blockType.FillerX == 0 && blockType.FillerY == 0 && blockType.FillerZ == 0;
			int result;
			if (flag)
			{
				result = id;
			}
			else
			{
				block.X -= blockType.FillerX;
				block.Y -= blockType.FillerY;
				block.Z -= blockType.FillerZ;
				result = this._mapModule.GetBlock(block.X, block.Y, block.Z, 1);
			}
			return result;
		}

		// Token: 0x06004573 RID: 17779 RVA: 0x000F3F80 File Offset: 0x000F2180
		public ClientBlockType GetBlockTypeFiller(IntVector3 block)
		{
			return this._mapModule.ClientBlockTypes[this.GetBlockIdFiller(block)];
		}

		// Token: 0x040022D0 RID: 8912
		private readonly MapModule _mapModule;

		// Token: 0x040022D1 RID: 8913
		private Chunk _chunk;
	}
}
