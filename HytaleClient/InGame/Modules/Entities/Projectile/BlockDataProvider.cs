using System;
using HytaleClient.Data.Map;
using HytaleClient.InGame.Modules.Map;
using HytaleClient.Math;
using HytaleClient.Utils;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x0200094F RID: 2383
	internal class BlockDataProvider : BlockData
	{
		// Token: 0x06004A8D RID: 19085 RVA: 0x001303B5 File Offset: 0x0012E5B5
		public void Initialize(GameInstance gameInstance)
		{
			this._gameInstance = gameInstance;
			this._blockId = int.MaxValue;
			this.Cleanup0();
		}

		// Token: 0x06004A8E RID: 19086 RVA: 0x001303D1 File Offset: 0x0012E5D1
		public void Cleanup()
		{
			this._gameInstance = null;
			this.Cleanup0();
		}

		// Token: 0x06004A8F RID: 19087 RVA: 0x001303E4 File Offset: 0x0012E5E4
		public void Read(int x, int y, int z)
		{
			int num = this.ReadBlockId(x, y, z);
			bool flag = this._blockId == num;
			if (!flag)
			{
				bool flag2 = num == 0;
				if (flag2)
				{
					this.SetBlock(0, this._gameInstance.MapModule.ClientBlockTypes[0], 1);
				}
				else
				{
					bool flag3 = num == 1;
					if (flag3)
					{
						this.SetBlock(int.MaxValue, this._gameInstance.MapModule.ClientBlockTypes[1], 4);
					}
					else
					{
						this._blockId = num;
						this.BlockType = this._gameInstance.MapModule.ClientBlockTypes[this._blockId];
						bool unknown = this.BlockType.Unknown;
						if (unknown)
						{
							this.SetBlock(num, this.BlockType, 4);
						}
						else
						{
							this.IsFiller = (this.BlockType.FillerX != 0 || this.BlockType.FillerY != 0 || this.BlockType.FillerZ != 0);
							bool isFiller = this.IsFiller;
							if (isFiller)
							{
								this._originalBlockTypeId = this.BlockType.VariantOriginalId;
								bool flag4 = this._originalBlockTypeId == 0;
								if (flag4)
								{
									this._originalBlockTypeId = 1;
								}
								this.OriginalBlockType = this._gameInstance.MapModule.ClientBlockTypes[this._originalBlockTypeId];
								this.CollisionMaterials = ((this.OriginalBlockType.CollisionMaterial == 1) ? 4 : 0);
								this.CollisionMaterials += BlockDataProvider.MaterialFromFillLevel(this.BlockType);
							}
							else
							{
								this._originalBlockTypeId = this._blockId;
								this.OriginalBlockType = this.BlockType;
								bool flag5 = this.BlockType.CollisionMaterial == 1;
								if (flag5)
								{
									this.CollisionMaterials = 4;
									bool flag6 = this.BlockType.HitboxType != 0;
									if (flag6)
									{
										this.CollisionMaterials += BlockDataProvider.MaterialFromFillLevel(this.BlockType);
									}
								}
								else
								{
									this.CollisionMaterials = BlockDataProvider.MaterialFromFillLevel(this.BlockType);
								}
							}
							this.FillHeight = ((this.BlockType.CollisionMaterial == 2 || this.BlockType.FluidBlockId != 0) ? ((float)this.BlockType.VerticalFill / 8f) : 0f);
							this._blockBoundingBoxes = null;
						}
					}
				}
			}
		}

		// Token: 0x06004A90 RID: 19088 RVA: 0x00130620 File Offset: 0x0012E820
		protected int ReadBlockId(int x, int y, int z)
		{
			int num = ChunkHelper.ChunkCoordinate(x);
			int num2 = ChunkHelper.ChunkCoordinate(z);
			bool flag = this._chunk == null || this._chunk.X != num || this._chunk.Z != num2;
			if (flag)
			{
				this._chunk = this._gameInstance.MapModule.GetChunkColumn(num, num2);
				this._chunkSectionIndex = int.MinValue;
				this._chunkSection = null;
			}
			bool flag2 = this._chunk == null;
			int result;
			if (flag2)
			{
				result = 1;
			}
			else
			{
				int num3 = ChunkHelper.IndexSection(y);
				bool flag3 = this._chunkSection == null || this._chunkSection.Y != num3;
				if (flag3)
				{
					this._chunkSectionIndex = num3;
					this._chunkSection = ((num3 >= 0 && num3 < ChunkHelper.ChunksPerColumn) ? this._chunk.GetChunk(num3) : null);
				}
				bool flag4 = this._chunkSection == null;
				if (flag4)
				{
					result = 0;
				}
				else
				{
					result = this._chunkSection.Data.GetBlock(x, y, z);
				}
			}
			return result;
		}

		// Token: 0x06004A91 RID: 19089 RVA: 0x0013072C File Offset: 0x0012E92C
		protected void SetBlock(int id, ClientBlockType type, int material, BlockHitbox box)
		{
			this._blockId = id;
			this.BlockType = type;
			this.OriginalBlockType = this.BlockType;
			this._originalBlockTypeId = this._blockId;
			this.CollisionMaterials = material;
			this._blockBoundingBoxes = box;
			this.IsFiller = false;
			this.FillHeight = 0f;
		}

		// Token: 0x06004A92 RID: 19090 RVA: 0x00130781 File Offset: 0x0012E981
		protected void SetBlock(int id, ClientBlockType type, int material)
		{
			this.SetBlock(id, type, material, this._gameInstance.ServerSettings.BlockHitboxes[0]);
		}

		// Token: 0x06004A93 RID: 19091 RVA: 0x001307A0 File Offset: 0x0012E9A0
		protected void Cleanup0()
		{
			this._chunk = null;
			this._chunkSectionIndex = int.MinValue;
			this._chunkSection = null;
			this.BlockType = null;
			this._blockId = int.MaxValue;
			this.OriginalBlockType = null;
			this._originalBlockTypeId = int.MaxValue;
			this._blockBoundingBoxes = null;
		}

		// Token: 0x06004A94 RID: 19092 RVA: 0x001307F4 File Offset: 0x0012E9F4
		protected static int MaterialFromFillLevel(ClientBlockType blockType)
		{
			int num = (int)((blockType.CollisionMaterial == 2 || blockType.FluidBlockId != int.MaxValue) ? blockType.VerticalFill : 0);
			bool flag = num == 0;
			int result;
			if (flag)
			{
				result = 1;
			}
			else
			{
				bool flag2 = num == 8;
				if (flag2)
				{
					result = 2;
				}
				else
				{
					result = 3;
				}
			}
			return result;
		}

		// Token: 0x0400264C RID: 9804
		protected const int FullLevel = 8;

		// Token: 0x0400264D RID: 9805
		protected const int InvalidChunkSectionIndex = -2147483648;

		// Token: 0x0400264E RID: 9806
		protected GameInstance _gameInstance;

		// Token: 0x0400264F RID: 9807
		protected ChunkColumn _chunk;

		// Token: 0x04002650 RID: 9808
		protected int _chunkSectionIndex;

		// Token: 0x04002651 RID: 9809
		protected Chunk _chunkSection;
	}
}
