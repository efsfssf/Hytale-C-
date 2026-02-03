using System;
using System.Collections.Generic;
using HytaleClient.Audio;
using HytaleClient.Data.Map.Chunk;
using HytaleClient.Utils;

namespace HytaleClient.Data.Map
{
	// Token: 0x02000ADF RID: 2783
	internal class ChunkData
	{
		// Token: 0x0600579B RID: 22427 RVA: 0x001A99C0 File Offset: 0x001A7BC0
		public ChunkData()
		{
			for (int i = 0; i < this.BlockHitTimers.Length; i++)
			{
				this.BlockHitTimers[i].BlockIndex = -1;
			}
		}

		// Token: 0x0600579C RID: 22428 RVA: 0x001A9A28 File Offset: 0x001A7C28
		public int GetBlock(int worldX, int worldY, int worldZ)
		{
			return this.Blocks.Get(ChunkHelper.IndexOfWorldBlockInChunk(worldX, worldY, worldZ));
		}

		// Token: 0x0600579D RID: 22429 RVA: 0x001A9A50 File Offset: 0x001A7C50
		public void SetBlock(int worldX, int worldY, int worldZ, int blockId)
		{
			int num = ChunkHelper.IndexOfWorldBlockInChunk(worldX, worldY, worldZ);
			this.Blocks.Set(num, blockId);
			for (int i = 0; i < this.BlockHitTimers.Length; i++)
			{
				bool flag = this.BlockHitTimers[i].BlockIndex == num;
				if (flag)
				{
					this.BlockHitTimers[i].BlockIndex = -1;
					this.BlockHitTimers[i].Timer = 0f;
					break;
				}
			}
		}

		// Token: 0x0600579E RID: 22430 RVA: 0x001A9AD4 File Offset: 0x001A7CD4
		public bool TryGetBlockHitTimer(int blockIndex, out int slotIndex, out float hitTimer)
		{
			for (int i = 0; i < this.BlockHitTimers.Length; i++)
			{
				bool flag = this.BlockHitTimers[i].BlockIndex == blockIndex;
				if (flag)
				{
					slotIndex = i;
					hitTimer = this.BlockHitTimers[i].Timer;
					return true;
				}
			}
			slotIndex = -1;
			hitTimer = 0f;
			return false;
		}

		// Token: 0x0600579F RID: 22431 RVA: 0x001A9B40 File Offset: 0x001A7D40
		public void SetBlockHitTimer(int blockIndex, float hitTimer)
		{
			bool flag = hitTimer == 0f;
			if (flag)
			{
				for (int i = 0; i < this.BlockHitTimers.Length; i++)
				{
					bool flag2 = this.BlockHitTimers[i].BlockIndex == blockIndex;
					if (flag2)
					{
						this.BlockHitTimers[i].BlockIndex = -1;
						this.BlockHitTimers[i].Timer = 0f;
						break;
					}
				}
			}
			else
			{
				bool flag3 = this.Blocks.Get(blockIndex) == 0;
				if (flag3)
				{
					throw new Exception("SetBlockHitTimer must never set the hitTimer > 0 for an empty block!");
				}
				int num = -1;
				for (int j = 0; j < this.BlockHitTimers.Length; j++)
				{
					bool flag4 = this.BlockHitTimers[j].BlockIndex == blockIndex;
					if (flag4)
					{
						this.BlockHitTimers[j].BlockIndex = blockIndex;
						this.BlockHitTimers[j].Timer = hitTimer;
						return;
					}
					bool flag5 = num == -1 && this.BlockHitTimers[j].BlockIndex == -1;
					if (flag5)
					{
						num = j;
					}
				}
				bool flag6 = num != -1;
				if (flag6)
				{
					this.BlockHitTimers[num].BlockIndex = blockIndex;
					this.BlockHitTimers[num].Timer = hitTimer;
				}
			}
		}

		// Token: 0x040035EF RID: 13807
		public PaletteChunkData Blocks = new PaletteChunkData();

		// Token: 0x040035F0 RID: 13808
		public Dictionary<int, ChunkData.InteractionStateInfo> CurrentInteractionStates = new Dictionary<int, ChunkData.InteractionStateInfo>();

		// Token: 0x040035F1 RID: 13809
		public const int MaxBlockHitTimers = 16;

		// Token: 0x040035F2 RID: 13810
		public readonly ChunkData.BlockHitTimer[] BlockHitTimers = new ChunkData.BlockHitTimer[16];

		// Token: 0x040035F3 RID: 13811
		public ushort[] SelfLightAmounts;

		// Token: 0x040035F4 RID: 13812
		public bool SelfLightNeedsUpdate = true;

		// Token: 0x040035F5 RID: 13813
		public ushort[] BorderedLightAmounts;

		// Token: 0x02000F1A RID: 3866
		public struct InteractionStateInfo
		{
			// Token: 0x04004A14 RID: 18964
			public int BlockId;

			// Token: 0x04004A15 RID: 18965
			public ClientBlockType BlockType;

			// Token: 0x04004A16 RID: 18966
			public float StateFrameTime;

			// Token: 0x04004A17 RID: 18967
			public AudioDevice.SoundEventReference SoundEventReference;
		}

		// Token: 0x02000F1B RID: 3867
		public struct BlockHitTimer
		{
			// Token: 0x04004A18 RID: 18968
			public int BlockIndex;

			// Token: 0x04004A19 RID: 18969
			public float Timer;
		}
	}
}
