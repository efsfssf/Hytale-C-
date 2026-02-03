using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using HytaleClient.Core;
using HytaleClient.Data.Map;
using HytaleClient.Data.Map.Chunk;
using HytaleClient.Graphics.Map;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;

namespace HytaleClient.InGame.Modules.Map
{
	// Token: 0x0200090A RID: 2314
	internal class MapGeometryBuilder : Disposable
	{
		// Token: 0x06004580 RID: 17792 RVA: 0x000F42AC File Offset: 0x000F24AC
		public bool IsSolidBorderedChunk(int[] borderedChunkData)
		{
			ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[borderedChunkData[ChunkHelper.IndexOfBlockInBorderedChunk(1, 1, 1)]];
			BlockType.DrawType drawType = clientBlockType.DrawType;
			bool requiresAlphaBlending = clientBlockType.RequiresAlphaBlending;
			for (int i = 0; i < 39304; i++)
			{
				int num = borderedChunkData[i];
				bool flag = num != int.MaxValue && (this._gameInstance.MapModule.ClientBlockTypes[num].DrawType != drawType || this._gameInstance.MapModule.ClientBlockTypes[num].RequiresAlphaBlending != requiresAlphaBlending);
				if (flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004581 RID: 17793 RVA: 0x000F435C File Offset: 0x000F255C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ClearBlock(int index)
		{
			this._borderedChunkBlocks[index] = int.MaxValue;
		}

		// Token: 0x06004582 RID: 17794 RVA: 0x000F436C File Offset: 0x000F256C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ClearLine(int targetOffset)
		{
			Buffer.BlockCopy(this._missingChunkLine, 0, this._borderedChunkBlocks, targetOffset * 4, 128);
		}

		// Token: 0x06004583 RID: 17795 RVA: 0x000F438C File Offset: 0x000F258C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ClearScatteredLine(int index, int offset)
		{
			for (int i = 0; i < 32; i++)
			{
				this._borderedChunkBlocks[index] = int.MaxValue;
				index += offset;
			}
		}

		// Token: 0x06004584 RID: 17796 RVA: 0x000F43C0 File Offset: 0x000F25C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ClearMultipleLines(int targetOffset, int offset)
		{
			for (int i = 0; i < 32; i++)
			{
				Buffer.BlockCopy(this._missingChunkLine, 0, this._borderedChunkBlocks, targetOffset * 4, 128);
				targetOffset += offset;
			}
		}

		// Token: 0x06004585 RID: 17797 RVA: 0x000F4401 File Offset: 0x000F2601
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void EmptyBlock(int index)
		{
			this._borderedChunkBlocks[index] = 0;
		}

		// Token: 0x06004586 RID: 17798 RVA: 0x000F440D File Offset: 0x000F260D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void EmptyLine(int targetOffset)
		{
			Buffer.BlockCopy(this._emptyChunkLine, 0, this._borderedChunkBlocks, targetOffset * 4, 128);
		}

		// Token: 0x06004587 RID: 17799 RVA: 0x000F442C File Offset: 0x000F262C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void EmptyScatteredLine(int index, int offset)
		{
			for (int i = 0; i < 32; i++)
			{
				this._borderedChunkBlocks[index] = 0;
				index += offset;
			}
		}

		// Token: 0x06004588 RID: 17800 RVA: 0x000F445C File Offset: 0x000F265C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void EmptyMultipleLines(int targetOffset, int offset)
		{
			for (int i = 0; i < 32; i++)
			{
				Buffer.BlockCopy(this._emptyChunkLine, 0, this._borderedChunkBlocks, targetOffset * 4, 128);
				targetOffset += offset;
			}
		}

		// Token: 0x06004589 RID: 17801 RVA: 0x000F44A0 File Offset: 0x000F26A0
		private void SetupBorderedChunkData(int chunkX, int chunkY, int chunkZ, ChunkData chunkData, uint[] columnTints, ushort[][] environmentIds)
		{
			this._borderedChunkHitTimers.Clear();
			int num = ChunkHelper.IndexOfBlockInBorderedChunk(1, 1, 1);
			for (int i = 0; i < 32; i++)
			{
				for (int j = 0; j < 32; j++)
				{
					for (int k = 0; k < 32; k++)
					{
						this._borderedChunkBlocks[num + k] = chunkData.Blocks.Get(k, i, j);
					}
					num += 34;
				}
				num += 68;
			}
			foreach (ChunkData.BlockHitTimer blockHitTimer in chunkData.BlockHitTimers)
			{
				bool flag = blockHitTimer.BlockIndex == -1;
				if (!flag)
				{
					int num2 = ChunkHelper.IndexOfBlockInBorderedChunk(blockHitTimer.BlockIndex, 0, 0, 0);
					bool flag2 = num2 != -1;
					if (flag2)
					{
						this._borderedChunkHitTimers.Add(num2, blockHitTimer.Timer);
					}
				}
			}
			int num3 = 0;
			int num4 = ChunkHelper.IndexInBorderedChunkColumn(1, 1);
			for (int m = 0; m < 32; m++)
			{
				for (int n = 0; n < 32; n++)
				{
					int num5 = environmentIds[num3 + n].Length;
					this._borderedColumnEnvironmentIds[num4 + n] = new ushort[num5];
					int count = 2 * num5;
					Buffer.BlockCopy(environmentIds[num3 + n], 0, this._borderedColumnEnvironmentIds[num4 + n], 0, count);
				}
				Buffer.BlockCopy(columnTints, num3 * 4, this._blendedBlockCornerBiomeTints, num4 * 4, 128);
				num3 += 32;
				num4 += 34;
			}
			ChunkColumn chunkColumn = this._gameInstance.MapModule.GetChunkColumn(chunkX - 1, chunkZ - 1);
			bool flag3 = chunkColumn != null;
			if (flag3)
			{
				int num6 = ChunkHelper.IndexInChunkColumn(31, 31);
				int num7 = ChunkHelper.IndexInBorderedChunkColumn(0, 0);
				this._borderedColumnEnvironmentIds[num7] = chunkColumn.Environments[num6];
				this._blendedBlockCornerBiomeTints[num7] = chunkColumn.Tints[num6];
			}
			ChunkColumn chunkColumn2 = this._gameInstance.MapModule.GetChunkColumn(chunkX + 1, chunkZ - 1);
			bool flag4 = chunkColumn2 != null;
			if (flag4)
			{
				int num8 = ChunkHelper.IndexInChunkColumn(0, 31);
				int num9 = ChunkHelper.IndexInBorderedChunkColumn(33, 0);
				this._borderedColumnEnvironmentIds[num9] = chunkColumn2.Environments[num8];
				this._blendedBlockCornerBiomeTints[num9] = chunkColumn2.Tints[num8];
			}
			ChunkColumn chunkColumn3 = this._gameInstance.MapModule.GetChunkColumn(chunkX - 1, chunkZ + 1);
			bool flag5 = chunkColumn3 != null;
			if (flag5)
			{
				int num10 = ChunkHelper.IndexInChunkColumn(31, 0);
				int num11 = ChunkHelper.IndexInBorderedChunkColumn(0, 33);
				this._borderedColumnEnvironmentIds[num11] = chunkColumn3.Environments[num10];
				this._blendedBlockCornerBiomeTints[num11] = chunkColumn3.Tints[num10];
			}
			ChunkColumn chunkColumn4 = this._gameInstance.MapModule.GetChunkColumn(chunkX + 1, chunkZ + 1);
			bool flag6 = chunkColumn4 != null;
			if (flag6)
			{
				int num12 = ChunkHelper.IndexInChunkColumn(0, 0);
				int num13 = ChunkHelper.IndexInBorderedChunkColumn(33, 33);
				this._borderedColumnEnvironmentIds[num13] = chunkColumn4.Environments[num12];
				this._blendedBlockCornerBiomeTints[num13] = chunkColumn4.Tints[num12];
			}
			ChunkColumn chunkColumn5 = this._gameInstance.MapModule.GetChunkColumn(chunkX, chunkZ - 1);
			bool flag7 = chunkColumn5 != null;
			if (flag7)
			{
				int num14 = ChunkHelper.IndexInChunkColumn(0, 31);
				int num15 = ChunkHelper.IndexInBorderedChunkColumn(1, 0);
				for (int num16 = 0; num16 < 32; num16++)
				{
					int num17 = chunkColumn5.Environments[num14 + num16].Length;
					this._borderedColumnEnvironmentIds[num15 + num16] = new ushort[num17];
					int count2 = 2 * num17;
					Buffer.BlockCopy(chunkColumn5.Environments[num14 + num16], 0, this._borderedColumnEnvironmentIds[num15 + num16], 0, count2);
				}
				Buffer.BlockCopy(chunkColumn5.Tints, num14 * 4, this._blendedBlockCornerBiomeTints, num15 * 4, 128);
			}
			ChunkColumn chunkColumn6 = this._gameInstance.MapModule.GetChunkColumn(chunkX, chunkZ + 1);
			bool flag8 = chunkColumn6 != null;
			if (flag8)
			{
				int num18 = ChunkHelper.IndexInChunkColumn(0, 0);
				int num19 = ChunkHelper.IndexInBorderedChunkColumn(1, 33);
				for (int num20 = 0; num20 < 32; num20++)
				{
					int num21 = chunkColumn6.Environments[num18 + num20].Length;
					this._borderedColumnEnvironmentIds[num19 + num20] = new ushort[num21];
					int count3 = 2 * num21;
					Buffer.BlockCopy(chunkColumn6.Environments[num18 + num20], 0, this._borderedColumnEnvironmentIds[num19 + num20], 0, count3);
				}
				Buffer.BlockCopy(chunkColumn6.Tints, num18 * 4, this._blendedBlockCornerBiomeTints, num19 * 4, 128);
			}
			ChunkColumn chunkColumn7 = this._gameInstance.MapModule.GetChunkColumn(chunkX - 1, chunkZ);
			bool flag9 = chunkColumn7 != null;
			if (flag9)
			{
				int num22 = ChunkHelper.IndexInChunkColumn(31, 0);
				int num23 = ChunkHelper.IndexInBorderedChunkColumn(0, 1);
				for (int num24 = 0; num24 < 32; num24++)
				{
					this._blendedBlockCornerBiomeTints[num23] = chunkColumn7.Tints[num22];
					this._borderedColumnEnvironmentIds[num23] = chunkColumn7.Environments[num22];
					num22 += 32;
					num23 += 34;
				}
			}
			ChunkColumn chunkColumn8 = this._gameInstance.MapModule.GetChunkColumn(chunkX + 1, chunkZ);
			bool flag10 = chunkColumn8 != null;
			if (flag10)
			{
				int num25 = ChunkHelper.IndexInChunkColumn(0, 0);
				int num26 = ChunkHelper.IndexInBorderedChunkColumn(33, 1);
				for (int num27 = 0; num27 < 32; num27++)
				{
					this._blendedBlockCornerBiomeTints[num26] = chunkColumn8.Tints[num25];
					this._borderedColumnEnvironmentIds[num26] = chunkColumn8.Environments[num25];
					num25 += 32;
					num26 += 34;
				}
			}
			for (int num28 = 0; num28 < 33; num28++)
			{
				for (int num29 = 0; num29 < 33; num29++)
				{
					int num30 = ChunkHelper.IndexInBorderedChunkColumn(num28, num29);
					int num31 = ChunkHelper.IndexInBorderedChunkColumn(num28 + 1, num29);
					int num32 = ChunkHelper.IndexInBorderedChunkColumn(num28, num29 + 1);
					int num33 = ChunkHelper.IndexInBorderedChunkColumn(num28 + 1, num29 + 1);
					uint num34 = this._blendedBlockCornerBiomeTints[num30];
					uint num35 = (uint)((byte)(num34 >> 16));
					uint num36 = (uint)((byte)(num34 >> 8));
					uint num37 = (uint)((byte)num34);
					num34 = this._blendedBlockCornerBiomeTints[num31];
					num35 += (uint)((byte)(num34 >> 16));
					num36 += (uint)((byte)(num34 >> 8));
					num37 += (uint)((byte)num34);
					num34 = this._blendedBlockCornerBiomeTints[num32];
					num35 += (uint)((byte)(num34 >> 16));
					num36 += (uint)((byte)(num34 >> 8));
					num37 += (uint)((byte)num34);
					num34 = this._blendedBlockCornerBiomeTints[num33];
					num35 += (uint)((byte)(num34 >> 16));
					num36 += (uint)((byte)(num34 >> 8));
					num37 += (uint)((byte)num34);
					num35 = (uint)((byte)(num35 * 0.25f));
					num36 = (uint)((byte)(num36 * 0.25f));
					num37 = (uint)((byte)(num37 * 0.25f));
					this._blendedBlockCornerBiomeTints[num30] = (num35 << 16 | num36 << 8 | num37);
				}
			}
			Chunk chunk = this._gameInstance.MapModule.GetChunk(chunkX - 1, chunkY - 1, chunkZ - 1);
			bool flag11 = chunk != null;
			if (flag11)
			{
				object disposeLock = chunk.DisposeLock;
				lock (disposeLock)
				{
					bool flag13 = !chunk.Disposed;
					if (flag13)
					{
						int num38 = ChunkHelper.IndexOfBlockInChunk(31, 31, 31);
						this._borderedChunkBlocks[0] = chunk.Data.Blocks.Get(num38);
						int num39;
						float value;
						bool flag14 = chunk.Data.TryGetBlockHitTimer(num38, out num39, out value);
						if (flag14)
						{
							this._borderedChunkHitTimers.Add(0, value);
						}
					}
					else
					{
						this.ClearBlock(0);
					}
				}
			}
			else
			{
				this.ClearBlock(0);
			}
			int num40 = ChunkHelper.IndexOfBlockInBorderedChunk(1, 0, 0);
			Chunk chunk2 = this._gameInstance.MapModule.GetChunk(chunkX, chunkY - 1, chunkZ - 1);
			bool flag15 = chunk2 != null;
			if (flag15)
			{
				object disposeLock2 = chunk2.DisposeLock;
				lock (disposeLock2)
				{
					bool flag17 = !chunk2.Disposed;
					if (flag17)
					{
						int num41 = ChunkHelper.IndexOfBlockInChunk(0, 31, 31);
						for (int num42 = 0; num42 < 32; num42++)
						{
							this._borderedChunkBlocks[num40 + num42] = chunk2.Data.Blocks.Get(num41 + num42);
						}
						foreach (ChunkData.BlockHitTimer blockHitTimer2 in chunk2.Data.BlockHitTimers)
						{
							bool flag18 = blockHitTimer2.BlockIndex == -1;
							if (!flag18)
							{
								int num44 = ChunkHelper.IndexOfBlockInBorderedChunk(blockHitTimer2.BlockIndex, 0, -1, -1);
								bool flag19 = num44 != -1;
								if (flag19)
								{
									this._borderedChunkHitTimers.Add(num44, blockHitTimer2.Timer);
								}
							}
						}
					}
					else
					{
						this.ClearLine(num40);
					}
				}
			}
			else
			{
				this.ClearLine(num40);
			}
			Chunk chunk3 = this._gameInstance.MapModule.GetChunk(chunkX + 1, chunkY - 1, chunkZ - 1);
			int num45 = ChunkHelper.IndexOfBlockInBorderedChunk(33, 0, 0);
			bool flag20 = chunk3 != null;
			if (flag20)
			{
				object disposeLock3 = chunk3.DisposeLock;
				lock (disposeLock3)
				{
					bool flag22 = !chunk3.Disposed;
					if (flag22)
					{
						int num46 = ChunkHelper.IndexOfBlockInChunk(0, 31, 31);
						this._borderedChunkBlocks[num45] = chunk3.Data.Blocks.Get(num46);
						int num39;
						float value2;
						bool flag23 = chunk3.Data.TryGetBlockHitTimer(num46, out num39, out value2);
						if (flag23)
						{
							this._borderedChunkHitTimers.Add(num45, value2);
						}
					}
					else
					{
						this.ClearBlock(num45);
					}
				}
			}
			else
			{
				this.ClearBlock(num45);
			}
			Chunk chunk4 = this._gameInstance.MapModule.GetChunk(chunkX - 1, chunkY - 1, chunkZ);
			int num47 = ChunkHelper.IndexOfBlockInBorderedChunk(0, 0, 1);
			bool flag24 = chunk4 != null;
			if (flag24)
			{
				object disposeLock4 = chunk4.DisposeLock;
				lock (disposeLock4)
				{
					bool flag26 = !chunk4.Disposed;
					if (flag26)
					{
						int num48 = ChunkHelper.IndexOfBlockInChunk(31, 31, 0);
						for (int num49 = 0; num49 < 32; num49++)
						{
							this._borderedChunkBlocks[num47] = chunk4.Data.Blocks.Get(num48);
							num48 += 32;
							num47 += 34;
						}
						foreach (ChunkData.BlockHitTimer blockHitTimer3 in chunk4.Data.BlockHitTimers)
						{
							bool flag27 = blockHitTimer3.BlockIndex == -1;
							if (!flag27)
							{
								int num51 = ChunkHelper.IndexOfBlockInBorderedChunk(blockHitTimer3.BlockIndex, -1, -1, 0);
								bool flag28 = num51 != -1;
								if (flag28)
								{
									this._borderedChunkHitTimers.Add(num51, blockHitTimer3.Timer);
								}
							}
						}
					}
					else
					{
						this.ClearScatteredLine(num47, 34);
					}
				}
			}
			else
			{
				this.ClearScatteredLine(num47, 34);
			}
			Chunk chunk5 = this._gameInstance.MapModule.GetChunk(chunkX + 1, chunkY - 1, chunkZ);
			int num52 = ChunkHelper.IndexOfBlockInBorderedChunk(33, 0, 1);
			bool flag29 = chunk5 != null;
			if (flag29)
			{
				object disposeLock5 = chunk5.DisposeLock;
				lock (disposeLock5)
				{
					bool flag31 = !chunk5.Disposed;
					if (flag31)
					{
						int num53 = ChunkHelper.IndexOfBlockInChunk(0, 31, 0);
						for (int num54 = 0; num54 < 32; num54++)
						{
							this._borderedChunkBlocks[num52] = chunk5.Data.Blocks.Get(num53);
							num53 += 32;
							num52 += 34;
						}
						foreach (ChunkData.BlockHitTimer blockHitTimer4 in chunk5.Data.BlockHitTimers)
						{
							bool flag32 = blockHitTimer4.BlockIndex == -1;
							if (!flag32)
							{
								int num56 = ChunkHelper.IndexOfBlockInBorderedChunk(blockHitTimer4.BlockIndex, 1, -1, 0);
								bool flag33 = num56 != -1;
								if (flag33)
								{
									this._borderedChunkHitTimers.Add(num56, blockHitTimer4.Timer);
								}
							}
						}
					}
					else
					{
						this.ClearScatteredLine(num52, 34);
					}
				}
			}
			else
			{
				this.ClearScatteredLine(num52, 34);
			}
			Chunk chunk6 = this._gameInstance.MapModule.GetChunk(chunkX, chunkY - 1, chunkZ);
			int num57 = ChunkHelper.IndexOfBlockInBorderedChunk(1, 0, 1);
			bool flag34 = chunk6 != null;
			if (flag34)
			{
				object disposeLock6 = chunk6.DisposeLock;
				lock (disposeLock6)
				{
					bool flag36 = !chunk6.Disposed;
					if (flag36)
					{
						int num58 = ChunkHelper.IndexOfBlockInChunk(0, 31, 0);
						for (int num59 = 0; num59 < 32; num59++)
						{
							for (int num60 = 0; num60 < 32; num60++)
							{
								this._borderedChunkBlocks[num57 + num60] = chunk6.Data.Blocks.Get(num58 + num60);
							}
							num58 += 32;
							num57 += 34;
						}
						foreach (ChunkData.BlockHitTimer blockHitTimer5 in chunk6.Data.BlockHitTimers)
						{
							bool flag37 = blockHitTimer5.BlockIndex == -1;
							if (!flag37)
							{
								int num62 = ChunkHelper.IndexOfBlockInBorderedChunk(blockHitTimer5.BlockIndex, 0, -1, 0);
								bool flag38 = num62 != -1;
								if (flag38)
								{
									this._borderedChunkHitTimers.Add(num62, blockHitTimer5.Timer);
								}
							}
						}
					}
					else
					{
						this.ClearMultipleLines(num57, 34);
					}
				}
			}
			else
			{
				this.ClearMultipleLines(num57, 34);
			}
			Chunk chunk7 = this._gameInstance.MapModule.GetChunk(chunkX - 1, chunkY - 1, chunkZ + 1);
			int num63 = ChunkHelper.IndexOfBlockInBorderedChunk(0, 0, 33);
			bool flag39 = chunk7 != null;
			if (flag39)
			{
				object disposeLock7 = chunk7.DisposeLock;
				lock (disposeLock7)
				{
					bool flag41 = !chunk7.Disposed;
					if (flag41)
					{
						int blockIdx = ChunkHelper.IndexOfBlockInChunk(31, 31, 0);
						this._borderedChunkBlocks[num63] = chunk7.Data.Blocks.Get(blockIdx);
						foreach (ChunkData.BlockHitTimer blockHitTimer6 in chunk7.Data.BlockHitTimers)
						{
							bool flag42 = blockHitTimer6.BlockIndex == -1;
							if (!flag42)
							{
								int num65 = ChunkHelper.IndexOfBlockInBorderedChunk(blockHitTimer6.BlockIndex, -1, -1, 1);
								bool flag43 = num65 != -1;
								if (flag43)
								{
									this._borderedChunkHitTimers.Add(num65, blockHitTimer6.Timer);
								}
							}
						}
					}
					else
					{
						this.ClearBlock(num63);
					}
				}
			}
			else
			{
				this.ClearBlock(num63);
			}
			int num66 = ChunkHelper.IndexOfBlockInBorderedChunk(1, 0, 33);
			Chunk chunk8 = this._gameInstance.MapModule.GetChunk(chunkX, chunkY - 1, chunkZ + 1);
			bool flag44 = chunk8 != null;
			if (flag44)
			{
				object disposeLock8 = chunk8.DisposeLock;
				lock (disposeLock8)
				{
					bool flag46 = !chunk8.Disposed;
					if (flag46)
					{
						int num67 = ChunkHelper.IndexOfBlockInChunk(0, 31, 0);
						for (int num68 = 0; num68 < 32; num68++)
						{
							this._borderedChunkBlocks[num66 + num68] = chunk8.Data.Blocks.Get(num67 + num68);
						}
						foreach (ChunkData.BlockHitTimer blockHitTimer7 in chunk8.Data.BlockHitTimers)
						{
							bool flag47 = blockHitTimer7.BlockIndex == -1;
							if (!flag47)
							{
								int num70 = ChunkHelper.IndexOfBlockInBorderedChunk(blockHitTimer7.BlockIndex, 0, -1, 1);
								bool flag48 = num70 != -1;
								if (flag48)
								{
									this._borderedChunkHitTimers.Add(num70, blockHitTimer7.Timer);
								}
							}
						}
					}
					else
					{
						this.ClearLine(num66);
					}
				}
			}
			else
			{
				this.ClearLine(num66);
			}
			Chunk chunk9 = this._gameInstance.MapModule.GetChunk(chunkX + 1, chunkY - 1, chunkZ + 1);
			int num71 = ChunkHelper.IndexOfBlockInBorderedChunk(33, 0, 33);
			bool flag49 = chunk9 != null;
			if (flag49)
			{
				object disposeLock9 = chunk9.DisposeLock;
				lock (disposeLock9)
				{
					bool flag51 = !chunk9.Disposed;
					if (flag51)
					{
						int num72 = ChunkHelper.IndexOfBlockInChunk(0, 31, 0);
						this._borderedChunkBlocks[num71] = chunk9.Data.Blocks.Get(num72);
						int num39;
						float value3;
						bool flag52 = chunk9.Data.TryGetBlockHitTimer(num72, out num39, out value3);
						if (flag52)
						{
							this._borderedChunkHitTimers.Add(num71, value3);
						}
					}
					else
					{
						this.ClearBlock(num71);
					}
				}
			}
			else
			{
				this.ClearBlock(num71);
			}
			Chunk chunk10 = this._gameInstance.MapModule.GetChunk(chunkX - 1, chunkY, chunkZ - 1);
			int num73 = ChunkHelper.IndexOfBlockInBorderedChunk(0, 1, 0);
			bool flag53 = chunk10 != null;
			if (flag53)
			{
				object disposeLock10 = chunk10.DisposeLock;
				lock (disposeLock10)
				{
					bool flag55 = !chunk10.Disposed;
					if (flag55)
					{
						int num74 = ChunkHelper.IndexOfBlockInChunk(31, 0, 31);
						for (int num75 = 0; num75 < 32; num75++)
						{
							this._borderedChunkBlocks[num73] = chunk10.Data.Blocks.Get(num74);
							num74 += 1024;
							num73 += 1156;
						}
						foreach (ChunkData.BlockHitTimer blockHitTimer8 in chunk10.Data.BlockHitTimers)
						{
							bool flag56 = blockHitTimer8.BlockIndex == -1;
							if (!flag56)
							{
								int num77 = ChunkHelper.IndexOfBlockInBorderedChunk(blockHitTimer8.BlockIndex, -1, 0, -1);
								bool flag57 = num77 != -1;
								if (flag57)
								{
									this._borderedChunkHitTimers.Add(num77, blockHitTimer8.Timer);
								}
							}
						}
					}
					else
					{
						this.ClearScatteredLine(num73, 1156);
					}
				}
			}
			else
			{
				this.ClearScatteredLine(num73, 1156);
			}
			Chunk chunk11 = this._gameInstance.MapModule.GetChunk(chunkX, chunkY, chunkZ - 1);
			int num78 = ChunkHelper.IndexOfBlockInBorderedChunk(1, 1, 0);
			bool flag58 = chunk11 != null;
			if (flag58)
			{
				object disposeLock11 = chunk11.DisposeLock;
				lock (disposeLock11)
				{
					bool flag60 = !chunk11.Disposed;
					if (flag60)
					{
						int num79 = ChunkHelper.IndexOfBlockInChunk(0, 0, 31);
						for (int num80 = 0; num80 < 32; num80++)
						{
							for (int num81 = 0; num81 < 32; num81++)
							{
								this._borderedChunkBlocks[num78 + num81] = chunk11.Data.Blocks.Get(num79 + num81);
							}
							num79 += 1024;
							num78 += 1156;
						}
						foreach (ChunkData.BlockHitTimer blockHitTimer9 in chunk11.Data.BlockHitTimers)
						{
							bool flag61 = blockHitTimer9.BlockIndex == -1;
							if (!flag61)
							{
								int num83 = ChunkHelper.IndexOfBlockInBorderedChunk(blockHitTimer9.BlockIndex, 0, 0, -1);
								bool flag62 = num83 != -1;
								if (flag62)
								{
									this._borderedChunkHitTimers.Add(num83, blockHitTimer9.Timer);
								}
							}
						}
					}
					else
					{
						this.ClearMultipleLines(num78, 1156);
					}
				}
			}
			else
			{
				this.ClearMultipleLines(num78, 1156);
			}
			Chunk chunk12 = this._gameInstance.MapModule.GetChunk(chunkX + 1, chunkY, chunkZ - 1);
			int num84 = ChunkHelper.IndexOfBlockInBorderedChunk(33, 1, 0);
			bool flag63 = chunk12 != null;
			if (flag63)
			{
				object disposeLock12 = chunk12.DisposeLock;
				lock (disposeLock12)
				{
					bool flag65 = !chunk12.Disposed;
					if (flag65)
					{
						int num85 = ChunkHelper.IndexOfBlockInChunk(0, 0, 31);
						for (int num86 = 0; num86 < 32; num86++)
						{
							this._borderedChunkBlocks[num84] = chunk12.Data.Blocks.Get(num85);
							num85 += 1024;
							num84 += 1156;
						}
						foreach (ChunkData.BlockHitTimer blockHitTimer10 in chunk12.Data.BlockHitTimers)
						{
							bool flag66 = blockHitTimer10.BlockIndex == -1;
							if (!flag66)
							{
								int num88 = ChunkHelper.IndexOfBlockInBorderedChunk(blockHitTimer10.BlockIndex, 1, 0, -1);
								bool flag67 = num88 != -1;
								if (flag67)
								{
									this._borderedChunkHitTimers.Add(num88, blockHitTimer10.Timer);
								}
							}
						}
					}
					else
					{
						this.ClearScatteredLine(num84, 1156);
					}
				}
			}
			else
			{
				this.ClearScatteredLine(num84, 1156);
			}
			Chunk chunk13 = this._gameInstance.MapModule.GetChunk(chunkX - 1, chunkY, chunkZ);
			int num89 = ChunkHelper.IndexOfBlockInBorderedChunk(0, 1, 1);
			bool flag68 = chunk13 != null;
			if (flag68)
			{
				object disposeLock13 = chunk13.DisposeLock;
				lock (disposeLock13)
				{
					bool flag70 = !chunk13.Disposed;
					if (flag70)
					{
						int num90 = ChunkHelper.IndexOfBlockInChunk(31, 0, 0);
						for (int num91 = 0; num91 < 32; num91++)
						{
							for (int num92 = 0; num92 < 32; num92++)
							{
								this._borderedChunkBlocks[num89] = chunk13.Data.Blocks.Get(num90);
								num90 += 32;
								num89 += 34;
							}
							num89 += 68;
						}
						foreach (ChunkData.BlockHitTimer blockHitTimer11 in chunk13.Data.BlockHitTimers)
						{
							bool flag71 = blockHitTimer11.BlockIndex == -1;
							if (!flag71)
							{
								int num94 = ChunkHelper.IndexOfBlockInBorderedChunk(blockHitTimer11.BlockIndex, -1, 0, 0);
								bool flag72 = num94 != -1;
								if (flag72)
								{
									this._borderedChunkHitTimers.Add(num94, blockHitTimer11.Timer);
								}
							}
						}
					}
					else
					{
						for (int num95 = 0; num95 < 32; num95++)
						{
							this.ClearScatteredLine(num89, 34);
							num89 += 68;
						}
					}
				}
			}
			else
			{
				for (int num96 = 0; num96 < 32; num96++)
				{
					this.ClearScatteredLine(num89, 34);
					num89 += 68;
				}
			}
			Chunk chunk14 = this._gameInstance.MapModule.GetChunk(chunkX + 1, chunkY, chunkZ);
			int num97 = ChunkHelper.IndexOfBlockInBorderedChunk(33, 1, 1);
			bool flag73 = chunk14 != null;
			if (flag73)
			{
				object disposeLock14 = chunk14.DisposeLock;
				lock (disposeLock14)
				{
					bool flag75 = !chunk14.Disposed;
					if (flag75)
					{
						int num98 = ChunkHelper.IndexOfBlockInChunk(0, 0, 0);
						for (int num99 = 0; num99 < 32; num99++)
						{
							for (int num100 = 0; num100 < 32; num100++)
							{
								this._borderedChunkBlocks[num97] = chunk14.Data.Blocks.Get(num98);
								num98 += 32;
								num97 += 34;
							}
							num97 += 68;
						}
						foreach (ChunkData.BlockHitTimer blockHitTimer12 in chunk14.Data.BlockHitTimers)
						{
							bool flag76 = blockHitTimer12.BlockIndex == -1;
							if (!flag76)
							{
								int num102 = ChunkHelper.IndexOfBlockInBorderedChunk(blockHitTimer12.BlockIndex, 1, 0, 0);
								bool flag77 = num102 != -1;
								if (flag77)
								{
									this._borderedChunkHitTimers.Add(num102, blockHitTimer12.Timer);
								}
							}
						}
					}
					else
					{
						for (int num103 = 0; num103 < 32; num103++)
						{
							this.ClearScatteredLine(num97, 34);
							num97 += 68;
						}
					}
				}
			}
			else
			{
				for (int num104 = 0; num104 < 32; num104++)
				{
					this.ClearScatteredLine(num97, 34);
					num97 += 68;
				}
			}
			Chunk chunk15 = this._gameInstance.MapModule.GetChunk(chunkX - 1, chunkY, chunkZ + 1);
			int num105 = ChunkHelper.IndexOfBlockInBorderedChunk(0, 1, 33);
			bool flag78 = chunk15 != null;
			if (flag78)
			{
				object disposeLock15 = chunk15.DisposeLock;
				lock (disposeLock15)
				{
					bool flag80 = !chunk15.Disposed;
					if (flag80)
					{
						int num106 = ChunkHelper.IndexOfBlockInChunk(31, 0, 0);
						for (int num107 = 0; num107 < 32; num107++)
						{
							this._borderedChunkBlocks[num105] = chunk15.Data.Blocks.Get(num106);
							num106 += 1024;
							num105 += 1156;
						}
						foreach (ChunkData.BlockHitTimer blockHitTimer13 in chunk15.Data.BlockHitTimers)
						{
							bool flag81 = blockHitTimer13.BlockIndex == -1;
							if (!flag81)
							{
								int num109 = ChunkHelper.IndexOfBlockInBorderedChunk(blockHitTimer13.BlockIndex, -1, 0, 1);
								bool flag82 = num109 != -1;
								if (flag82)
								{
									this._borderedChunkHitTimers.Add(num109, blockHitTimer13.Timer);
								}
							}
						}
					}
					else
					{
						this.ClearScatteredLine(num105, 1156);
					}
				}
			}
			else
			{
				this.ClearScatteredLine(num105, 1156);
			}
			Chunk chunk16 = this._gameInstance.MapModule.GetChunk(chunkX, chunkY, chunkZ + 1);
			int num110 = ChunkHelper.IndexOfBlockInBorderedChunk(1, 1, 33);
			bool flag83 = chunk16 != null;
			if (flag83)
			{
				object disposeLock16 = chunk16.DisposeLock;
				lock (disposeLock16)
				{
					bool flag85 = !chunk16.Disposed;
					if (flag85)
					{
						int num111 = ChunkHelper.IndexOfBlockInChunk(0, 0, 0);
						for (int num112 = 0; num112 < 32; num112++)
						{
							for (int num113 = 0; num113 < 32; num113++)
							{
								this._borderedChunkBlocks[num110 + num113] = chunk16.Data.Blocks.Get(num111 + num113);
							}
							num111 += 1024;
							num110 += 1156;
						}
						foreach (ChunkData.BlockHitTimer blockHitTimer14 in chunk16.Data.BlockHitTimers)
						{
							bool flag86 = blockHitTimer14.BlockIndex == -1;
							if (!flag86)
							{
								int num115 = ChunkHelper.IndexOfBlockInBorderedChunk(blockHitTimer14.BlockIndex, 0, 0, 1);
								bool flag87 = num115 != -1;
								if (flag87)
								{
									this._borderedChunkHitTimers.Add(num115, blockHitTimer14.Timer);
								}
							}
						}
					}
					else
					{
						this.ClearMultipleLines(num110, 1156);
					}
				}
			}
			else
			{
				this.ClearMultipleLines(num110, 1156);
			}
			Chunk chunk17 = this._gameInstance.MapModule.GetChunk(chunkX + 1, chunkY, chunkZ + 1);
			int num116 = ChunkHelper.IndexOfBlockInBorderedChunk(33, 1, 33);
			bool flag88 = chunk17 != null;
			if (flag88)
			{
				object disposeLock17 = chunk17.DisposeLock;
				lock (disposeLock17)
				{
					bool flag90 = !chunk17.Disposed;
					if (flag90)
					{
						int num117 = ChunkHelper.IndexOfBlockInChunk(0, 0, 0);
						for (int num118 = 0; num118 < 32; num118++)
						{
							this._borderedChunkBlocks[num116] = chunk17.Data.Blocks.Get(num117);
							num117 += 1024;
							num116 += 1156;
						}
						foreach (ChunkData.BlockHitTimer blockHitTimer15 in chunk17.Data.BlockHitTimers)
						{
							bool flag91 = blockHitTimer15.BlockIndex == -1;
							if (!flag91)
							{
								int num120 = ChunkHelper.IndexOfBlockInBorderedChunk(blockHitTimer15.BlockIndex, 1, 0, 1);
								bool flag92 = num120 != -1;
								if (flag92)
								{
									this._borderedChunkHitTimers.Add(num120, blockHitTimer15.Timer);
								}
							}
						}
					}
					else
					{
						this.ClearScatteredLine(num116, 1156);
					}
				}
			}
			else
			{
				this.ClearScatteredLine(num116, 1156);
			}
			Chunk chunk18 = this._gameInstance.MapModule.GetChunk(chunkX - 1, chunkY + 1, chunkZ - 1);
			int num121 = ChunkHelper.IndexOfBlockInBorderedChunk(0, 33, 0);
			bool flag93 = chunk18 != null;
			if (flag93)
			{
				object disposeLock18 = chunk18.DisposeLock;
				lock (disposeLock18)
				{
					bool flag95 = !chunk18.Disposed;
					if (flag95)
					{
						int num122 = ChunkHelper.IndexOfBlockInChunk(31, 0, 31);
						this._borderedChunkBlocks[num121] = chunk18.Data.Blocks.Get(num122);
						int num39;
						float value4;
						bool flag96 = chunk18.Data.TryGetBlockHitTimer(num122, out num39, out value4);
						if (flag96)
						{
							this._borderedChunkHitTimers.Add(num121, value4);
						}
					}
					else
					{
						this.ClearBlock(num121);
					}
				}
			}
			else
			{
				bool flag97 = chunkY + 1 >= ChunkHelper.ChunksPerColumn;
				if (flag97)
				{
					this.EmptyBlock(num121);
				}
				else
				{
					this.ClearBlock(num121);
				}
			}
			Chunk chunk19 = this._gameInstance.MapModule.GetChunk(chunkX, chunkY + 1, chunkZ - 1);
			int num123 = ChunkHelper.IndexOfBlockInBorderedChunk(1, 33, 0);
			bool flag98 = chunk19 != null;
			if (flag98)
			{
				object disposeLock19 = chunk19.DisposeLock;
				lock (disposeLock19)
				{
					bool flag100 = !chunk19.Disposed;
					if (flag100)
					{
						int num124 = ChunkHelper.IndexOfBlockInChunk(0, 0, 31);
						for (int num125 = 0; num125 < 32; num125++)
						{
							this._borderedChunkBlocks[num123 + num125] = chunk19.Data.Blocks.Get(num124 + num125);
						}
						foreach (ChunkData.BlockHitTimer blockHitTimer16 in chunk19.Data.BlockHitTimers)
						{
							bool flag101 = blockHitTimer16.BlockIndex == -1;
							if (!flag101)
							{
								int num127 = ChunkHelper.IndexOfBlockInBorderedChunk(blockHitTimer16.BlockIndex, 0, 1, -1);
								bool flag102 = num127 != -1;
								if (flag102)
								{
									this._borderedChunkHitTimers.Add(num127, blockHitTimer16.Timer);
								}
							}
						}
					}
					else
					{
						this.ClearLine(num123);
					}
				}
			}
			else
			{
				bool flag103 = chunkY + 1 >= ChunkHelper.ChunksPerColumn;
				if (flag103)
				{
					this.EmptyLine(num123);
				}
				else
				{
					this.ClearLine(num123);
				}
			}
			Chunk chunk20 = this._gameInstance.MapModule.GetChunk(chunkX + 1, chunkY + 1, chunkZ - 1);
			int num128 = ChunkHelper.IndexOfBlockInBorderedChunk(33, 33, 0);
			bool flag104 = chunk20 != null;
			if (flag104)
			{
				object disposeLock20 = chunk20.DisposeLock;
				lock (disposeLock20)
				{
					bool flag106 = !chunk20.Disposed;
					if (flag106)
					{
						int num129 = ChunkHelper.IndexOfBlockInChunk(0, 0, 31);
						this._borderedChunkBlocks[num128] = chunk20.Data.Blocks.Get(num129);
						int num39;
						float value5;
						bool flag107 = chunk20.Data.TryGetBlockHitTimer(num129, out num39, out value5);
						if (flag107)
						{
							this._borderedChunkHitTimers.Add(num128, value5);
						}
					}
					else
					{
						this.ClearBlock(num128);
					}
				}
			}
			else
			{
				bool flag108 = chunkY + 1 >= ChunkHelper.ChunksPerColumn;
				if (flag108)
				{
					this.EmptyBlock(num128);
				}
				else
				{
					this.ClearBlock(num128);
				}
			}
			Chunk chunk21 = this._gameInstance.MapModule.GetChunk(chunkX - 1, chunkY + 1, chunkZ);
			int num130 = ChunkHelper.IndexOfBlockInBorderedChunk(0, 33, 1);
			bool flag109 = chunk21 != null;
			if (flag109)
			{
				object disposeLock21 = chunk21.DisposeLock;
				lock (disposeLock21)
				{
					bool flag111 = !chunk21.Disposed;
					if (flag111)
					{
						int num131 = ChunkHelper.IndexOfBlockInChunk(31, 0, 0);
						for (int num132 = 0; num132 < 32; num132++)
						{
							this._borderedChunkBlocks[num130] = chunk21.Data.Blocks.Get(num131);
							num131 += 32;
							num130 += 34;
						}
						foreach (ChunkData.BlockHitTimer blockHitTimer17 in chunk21.Data.BlockHitTimers)
						{
							bool flag112 = blockHitTimer17.BlockIndex == -1;
							if (!flag112)
							{
								int num134 = ChunkHelper.IndexOfBlockInBorderedChunk(blockHitTimer17.BlockIndex, -1, 1, 0);
								bool flag113 = num134 != -1;
								if (flag113)
								{
									this._borderedChunkHitTimers.Add(num134, blockHitTimer17.Timer);
								}
							}
						}
					}
					else
					{
						this.ClearScatteredLine(num130, 34);
					}
				}
			}
			else
			{
				bool flag114 = chunkY + 1 >= ChunkHelper.ChunksPerColumn;
				if (flag114)
				{
					this.EmptyScatteredLine(num130, 34);
				}
				else
				{
					this.ClearScatteredLine(num130, 34);
				}
			}
			Chunk chunk22 = this._gameInstance.MapModule.GetChunk(chunkX, chunkY + 1, chunkZ);
			int num135 = ChunkHelper.IndexOfBlockInBorderedChunk(1, 33, 1);
			bool flag115 = chunk22 != null;
			if (flag115)
			{
				object disposeLock22 = chunk22.DisposeLock;
				lock (disposeLock22)
				{
					bool flag117 = !chunk22.Disposed;
					if (flag117)
					{
						int num136 = ChunkHelper.IndexOfBlockInChunk(0, 0, 0);
						for (int num137 = 0; num137 < 32; num137++)
						{
							for (int num138 = 0; num138 < 32; num138++)
							{
								this._borderedChunkBlocks[num135 + num138] = chunk22.Data.Blocks.Get(num136 + num138);
							}
							num136 += 32;
							num135 += 34;
						}
						foreach (ChunkData.BlockHitTimer blockHitTimer18 in chunk22.Data.BlockHitTimers)
						{
							bool flag118 = blockHitTimer18.BlockIndex == -1;
							if (!flag118)
							{
								int num140 = ChunkHelper.IndexOfBlockInBorderedChunk(blockHitTimer18.BlockIndex, 0, 1, 0);
								bool flag119 = num140 != -1;
								if (flag119)
								{
									this._borderedChunkHitTimers.Add(num140, blockHitTimer18.Timer);
								}
							}
						}
					}
					else
					{
						this.ClearMultipleLines(num135, 34);
					}
				}
			}
			else
			{
				bool flag120 = chunkY + 1 >= ChunkHelper.ChunksPerColumn;
				if (flag120)
				{
					this.EmptyMultipleLines(num135, 34);
				}
				else
				{
					this.ClearMultipleLines(num135, 34);
				}
			}
			Chunk chunk23 = this._gameInstance.MapModule.GetChunk(chunkX + 1, chunkY + 1, chunkZ);
			int num141 = ChunkHelper.IndexOfBlockInBorderedChunk(33, 33, 1);
			bool flag121 = chunk23 != null;
			if (flag121)
			{
				object disposeLock23 = chunk23.DisposeLock;
				lock (disposeLock23)
				{
					bool flag123 = !chunk23.Disposed;
					if (flag123)
					{
						int num142 = ChunkHelper.IndexOfBlockInChunk(0, 0, 0);
						for (int num143 = 0; num143 < 32; num143++)
						{
							this._borderedChunkBlocks[num141] = chunk23.Data.Blocks.Get(num142);
							num142 += 32;
							num141 += 34;
						}
						foreach (ChunkData.BlockHitTimer blockHitTimer19 in chunk23.Data.BlockHitTimers)
						{
							bool flag124 = blockHitTimer19.BlockIndex == -1;
							if (!flag124)
							{
								int num145 = ChunkHelper.IndexOfBlockInBorderedChunk(blockHitTimer19.BlockIndex, 1, 1, 0);
								bool flag125 = num145 != -1;
								if (flag125)
								{
									this._borderedChunkHitTimers.Add(num145, blockHitTimer19.Timer);
								}
							}
						}
					}
					else
					{
						this.ClearScatteredLine(num141, 34);
					}
				}
			}
			else
			{
				bool flag126 = chunkY + 1 >= ChunkHelper.ChunksPerColumn;
				if (flag126)
				{
					this.EmptyScatteredLine(num141, 34);
				}
				else
				{
					this.ClearScatteredLine(num141, 34);
				}
			}
			Chunk chunk24 = this._gameInstance.MapModule.GetChunk(chunkX - 1, chunkY + 1, chunkZ + 1);
			int num146 = ChunkHelper.IndexOfBlockInBorderedChunk(0, 33, 33);
			bool flag127 = chunk24 != null;
			if (flag127)
			{
				object disposeLock24 = chunk24.DisposeLock;
				lock (disposeLock24)
				{
					bool flag129 = !chunk24.Disposed;
					if (flag129)
					{
						int num147 = ChunkHelper.IndexOfBlockInChunk(31, 0, 0);
						this._borderedChunkBlocks[num146] = chunk24.Data.Blocks.Get(num147);
						int num39;
						float value6;
						bool flag130 = chunk24.Data.TryGetBlockHitTimer(num147, out num39, out value6);
						if (flag130)
						{
							this._borderedChunkHitTimers.Add(num146, value6);
						}
					}
					else
					{
						this.ClearBlock(num146);
					}
				}
			}
			else
			{
				bool flag131 = chunkY + 1 >= ChunkHelper.ChunksPerColumn;
				if (flag131)
				{
					this.EmptyBlock(num146);
				}
				else
				{
					this.ClearBlock(num146);
				}
			}
			Chunk chunk25 = this._gameInstance.MapModule.GetChunk(chunkX, chunkY + 1, chunkZ + 1);
			int num148 = ChunkHelper.IndexOfBlockInBorderedChunk(1, 33, 33);
			bool flag132 = chunk25 != null;
			if (flag132)
			{
				object disposeLock25 = chunk25.DisposeLock;
				lock (disposeLock25)
				{
					bool flag134 = !chunk25.Disposed;
					if (flag134)
					{
						int num149 = ChunkHelper.IndexOfBlockInChunk(0, 0, 0);
						for (int num150 = 0; num150 < 32; num150++)
						{
							this._borderedChunkBlocks[num148 + num150] = chunk25.Data.Blocks.Get(num149 + num150);
						}
						foreach (ChunkData.BlockHitTimer blockHitTimer20 in chunk25.Data.BlockHitTimers)
						{
							bool flag135 = blockHitTimer20.BlockIndex == -1;
							if (!flag135)
							{
								int num152 = ChunkHelper.IndexOfBlockInBorderedChunk(blockHitTimer20.BlockIndex, 0, 1, 1);
								bool flag136 = num152 != -1;
								if (flag136)
								{
									this._borderedChunkHitTimers.Add(num152, blockHitTimer20.Timer);
								}
							}
						}
					}
					else
					{
						this.ClearLine(num148);
					}
				}
			}
			else
			{
				bool flag137 = chunkY + 1 >= ChunkHelper.ChunksPerColumn;
				if (flag137)
				{
					this.EmptyLine(num148);
				}
				else
				{
					this.ClearLine(num148);
				}
			}
			Chunk chunk26 = this._gameInstance.MapModule.GetChunk(chunkX + 1, chunkY + 1, chunkZ + 1);
			int num153 = ChunkHelper.IndexOfBlockInBorderedChunk(33, 33, 33);
			bool flag138 = chunk26 != null;
			if (flag138)
			{
				object disposeLock26 = chunk26.DisposeLock;
				lock (disposeLock26)
				{
					bool flag140 = !chunk26.Disposed;
					if (flag140)
					{
						int num154 = ChunkHelper.IndexOfBlockInChunk(0, 0, 0);
						this._borderedChunkBlocks[num153] = chunk26.Data.Blocks.Get(num154);
						int num39;
						float value7;
						bool flag141 = chunk26.Data.TryGetBlockHitTimer(num154, out num39, out value7);
						if (flag141)
						{
							this._borderedChunkHitTimers.Add(num153, value7);
						}
					}
					else
					{
						this.ClearBlock(num153);
					}
				}
			}
			else
			{
				bool flag142 = chunkY + 1 >= ChunkHelper.ChunksPerColumn;
				if (flag142)
				{
					this.EmptyBlock(num153);
				}
				else
				{
					this.ClearBlock(num153);
				}
			}
		}

		// Token: 0x0600458A RID: 17802 RVA: 0x000F7968 File Offset: 0x000F5B68
		public MapGeometryBuilder(GameInstance gameInstance)
		{
			this._gameInstance = gameInstance;
			this._chunkGeometryBuilder = new ChunkGeometryBuilder();
			for (int i = 0; i < this._missingChunkLine.Length; i++)
			{
				this._missingChunkLine[i] = int.MaxValue;
			}
			for (int j = 0; j < this._emptyChunkLine.Length; j++)
			{
				this._emptyChunkLine[j] = 0;
			}
			this.AllocateLightData();
			this.Resume();
		}

		// Token: 0x0600458B RID: 17803 RVA: 0x000F7AFC File Offset: 0x000F5CFC
		protected override void DoDispose()
		{
			this.Suspend();
			Disposable disposable;
			while (this._chunkGeometryBuilder.DisposeRequests.TryDequeue(out disposable))
			{
				disposable.Dispose();
			}
		}

		// Token: 0x0600458C RID: 17804 RVA: 0x000F7B30 File Offset: 0x000F5D30
		public void Suspend()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this._threadCancellationTokenSource.Cancel();
			this._restartSpiralEvent.Set();
			this._thread.Join();
			this._thread = null;
			this._threadCancellationTokenSource = null;
		}

		// Token: 0x0600458D RID: 17805 RVA: 0x000F7B70 File Offset: 0x000F5D70
		public void Resume()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this._chunkGeometryBuilder.SetBlockTypes(this._gameInstance.MapModule.ClientBlockTypes);
			this._chunkGeometryBuilder.SetBlockHitboxes(this._gameInstance.ServerSettings.BlockHitboxes);
			this._chunkGeometryBuilder.SetLightLevels(this._gameInstance.MapModule.LightLevels);
			this._chunkGeometryBuilder.SetEnvironments(this._gameInstance.ServerSettings.Environments);
			this._chunkGeometryBuilder.SetAtlasSizes(this._gameInstance.AtlasSizes);
			this._threadCancellationTokenSource = new CancellationTokenSource();
			this._threadCancellationToken = this._threadCancellationTokenSource.Token;
			this._thread = new Thread(new ThreadStart(this.ThreadStart))
			{
				Name = "BackgroundMapGeometryBuilder",
				IsBackground = true
			};
			this._thread.Start();
		}

		// Token: 0x0600458E RID: 17806 RVA: 0x000F7C64 File Offset: 0x000F5E64
		public void HandleDisposeRequests()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this._disposeRequestsStopwatch.Restart();
			Disposable disposable;
			while (this._chunkGeometryBuilder.DisposeRequests.TryDequeue(out disposable))
			{
				disposable.Dispose();
				bool flag = (float)this._disposeRequestsStopwatch.ElapsedMilliseconds > 0.1f;
				if (flag)
				{
					break;
				}
			}
		}

		// Token: 0x0600458F RID: 17807 RVA: 0x000F7CC3 File Offset: 0x000F5EC3
		public void EnsureEnoughChunkUpdateTasks()
		{
			this._chunkGeometryBuilder.EnsureEnoughChunkUpdateTasks();
		}

		// Token: 0x06004590 RID: 17808 RVA: 0x000F7CD1 File Offset: 0x000F5ED1
		public int GetChunkUpdateTaskQueueCount()
		{
			return this._chunkGeometryBuilder.ChunkUpdateTaskQueue.Count;
		}

		// Token: 0x06004591 RID: 17809 RVA: 0x000F7CE4 File Offset: 0x000F5EE4
		public void EnqueueChunkUpdateTask(RenderedChunk.ChunkUpdateTask chunkUpdateTask)
		{
			bool flag = chunkUpdateTask.AnimatedBlocks != null;
			if (flag)
			{
				for (int i = 0; i < chunkUpdateTask.AnimatedBlocks.Length; i++)
				{
					this._chunkGeometryBuilder.DisposeRequests.Enqueue(chunkUpdateTask.AnimatedBlocks[i].Renderer);
				}
				chunkUpdateTask.AnimatedBlocks = null;
			}
			this._chunkGeometryBuilder.ChunkUpdateTaskQueue.Add(chunkUpdateTask);
		}

		// Token: 0x06004592 RID: 17810 RVA: 0x000F7D54 File Offset: 0x000F5F54
		public void RestartSpiral(Vector3 positionInChunk, int startChunkX, int startChunkY, int startChunkZ, int spiralRadius)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			object spiralIteratorLock = this._spiralIteratorLock;
			lock (spiralIteratorLock)
			{
				this._startChunkX = startChunkX;
				this._startChunkY = startChunkY;
				this._startChunkZ = startChunkZ;
				this._spiralRadius = spiralRadius;
				this.OrderNearestChunks(positionInChunk, startChunkX, startChunkY, startChunkZ);
			}
			this._restartSpiralEvent.Set();
		}

		// Token: 0x06004593 RID: 17811 RVA: 0x000F7DD4 File Offset: 0x000F5FD4
		private void OrderNearestChunks(Vector3 positionInChunk, int startChunkX, int startChunkY, int startChunkZ)
		{
			for (int i = 0; i < MapGeometryBuilder.RebuildOffsets.Length; i++)
			{
				int num = MapGeometryBuilder.RebuildOffsets[i];
				for (int j = 0; j < MapGeometryBuilder.RebuildOffsets.Length; j++)
				{
					int num2 = MapGeometryBuilder.RebuildOffsets[j];
					for (int k = 0; k < MapGeometryBuilder.RebuildOffsets.Length; k++)
					{
						int num3 = MapGeometryBuilder.RebuildOffsets[k];
						int num4 = (i * 3 + j) * 3 + k;
						this._nearestChunks[num4] = new IntVector3(startChunkX + num2, startChunkY + num3, startChunkZ + num);
						Vector3 value = new Vector3(15.5f + (float)(32 * num2), 15.5f + (float)(32 * num3), 15.5f + (float)(32 * num));
						float num5 = Vector3.DistanceSquared(positionInChunk, value);
						this._nearestChunkDistances[num4] = num5;
					}
				}
			}
			this._nearestChunkDistances[0] = 0f;
			Array.Sort<float, IntVector3>(this._nearestChunkDistances, this._nearestChunks);
		}

		// Token: 0x06004594 RID: 17812 RVA: 0x000F7EE4 File Offset: 0x000F60E4
		private void ThreadStart()
		{
			for (;;)
			{
				this._restartSpiralEvent.WaitOne();
				for (;;)
				{
					IL_13:
					bool isCancellationRequested = this._threadCancellationToken.IsCancellationRequested;
					if (isCancellationRequested)
					{
						return;
					}
					int chunkYMin = this._gameInstance.MapModule.ChunkYMin;
					object spiralIteratorLock = this._spiralIteratorLock;
					int startChunkY;
					lock (spiralIteratorLock)
					{
						int startChunkX = this._startChunkX;
						startChunkY = this._startChunkY;
						int startChunkZ = this._startChunkZ;
						this._spiralIterator.Initialize(this._startChunkX, this._startChunkZ, this._spiralRadius);
					}
					for (int i = 0; i < this._nearestChunks.Length; i++)
					{
						IntVector3 intVector = this._nearestChunks[i];
						ChunkColumn chunkColumn = this._gameInstance.MapModule.GetChunkColumn(ChunkHelper.IndexOfChunkColumn(intVector.X, intVector.Z));
						bool flag2 = chunkColumn == null;
						if (!flag2)
						{
							bool flag3 = intVector.Y < chunkYMin;
							if (!flag3)
							{
								Chunk chunk = chunkColumn.GetChunk(intVector.Y);
								bool flag4 = chunk == null || chunk.Rendered == null;
								if (!flag4)
								{
									bool flag5 = chunk.Rendered.RebuildState == RenderedChunk.ChunkRebuildState.ReadyForRebuild;
									if (flag5)
									{
										this.RebuildChunk(chunkColumn, chunk);
										bool flag6 = this._restartSpiralEvent.WaitOne(0);
										if (flag6)
										{
											goto IL_13;
										}
									}
								}
							}
						}
					}
					foreach (long indexChunk in this._spiralIterator)
					{
						ChunkColumn chunkColumn2 = this._gameInstance.MapModule.GetChunkColumn(indexChunk);
						bool flag7 = chunkColumn2 == null;
						if (!flag7)
						{
							for (int j = 0; j < ChunkHelper.ChunksPerColumn; j++)
							{
								bool isCancellationRequested2 = this._threadCancellationToken.IsCancellationRequested;
								if (isCancellationRequested2)
								{
									return;
								}
								int num = startChunkY - j;
								bool flag8 = num < 0;
								if (flag8)
								{
									num = startChunkY - num;
								}
								bool flag9 = num < chunkYMin;
								if (!flag9)
								{
									Chunk chunk2 = chunkColumn2.GetChunk(num);
									bool flag10 = chunk2 == null || chunk2.Rendered == null;
									if (!flag10)
									{
										bool flag11 = chunk2.Rendered.RebuildState == RenderedChunk.ChunkRebuildState.ReadyForRebuild;
										if (flag11)
										{
											this.RebuildChunk(chunkColumn2, chunk2);
											bool flag12 = this._restartSpiralEvent.WaitOne(0);
											if (flag12)
											{
												goto IL_13;
											}
										}
									}
								}
							}
						}
					}
					break;
				}
			}
		}

		// Token: 0x06004595 RID: 17813 RVA: 0x000F81A0 File Offset: 0x000F63A0
		private void RebuildChunk(ChunkColumn chunkColumn, Chunk chunk)
		{
			object disposeLock = chunk.DisposeLock;
			lock (disposeLock)
			{
				bool disposed = chunk.Disposed;
				if (disposed)
				{
					return;
				}
				chunk.Rendered.RebuildState = RenderedChunk.ChunkRebuildState.Rebuilding;
				bool flag2 = chunk.Data.Blocks.IsSolidAir();
				if (flag2)
				{
					chunk.Rendered.UpdateTask = null;
					chunk.Rendered.RebuildState = RenderedChunk.ChunkRebuildState.UpdateReady;
					return;
				}
				this.SetupBorderedChunkData(chunk.X, chunk.Y, chunk.Z, chunk.Data, chunkColumn.Tints, chunkColumn.Environments);
				bool flag3 = this.IsSolidBorderedChunk(this._borderedChunkBlocks);
				if (flag3)
				{
					chunk.Rendered.UpdateTask = null;
					chunk.Rendered.RebuildState = RenderedChunk.ChunkRebuildState.UpdateReady;
					return;
				}
			}
			this.CalculateLighting(chunk);
			object disposeLock2 = chunk.DisposeLock;
			lock (disposeLock2)
			{
				bool disposed2 = chunk.Disposed;
				if (disposed2)
				{
					return;
				}
				Buffer.BlockCopy(chunk.Data.BorderedLightAmounts, 0, this._borderedChunkLightAmounts, 0, chunk.Data.BorderedLightAmounts.Length * 2);
			}
			RenderedChunk.ChunkUpdateTask chunkUpdateTask = this._chunkGeometryBuilder.BuildGeometry(chunk.X, chunk.Y, chunk.Z, chunkColumn, this._borderedChunkBlocks, this._borderedChunkHitTimers, this._borderedChunkLightAmounts, this._blendedBlockCornerBiomeTints, this._borderedColumnEnvironmentIds, this._gameInstance.MapModule.TextureAtlas.Width, this._gameInstance.MapModule.TextureAtlas.Height, this._threadCancellationToken);
			object disposeLock3 = chunk.DisposeLock;
			lock (disposeLock3)
			{
				bool disposed3 = chunk.Disposed;
				if (disposed3)
				{
					bool flag6 = chunkUpdateTask != null;
					if (flag6)
					{
						this.EnqueueChunkUpdateTask(chunkUpdateTask);
					}
				}
				else
				{
					bool isCancellationRequested = this._threadCancellationToken.IsCancellationRequested;
					if (isCancellationRequested)
					{
						chunk.Rendered.RebuildState = RenderedChunk.ChunkRebuildState.ReadyForRebuild;
					}
					else
					{
						chunk.Rendered.UpdateTask = chunkUpdateTask;
						chunk.Rendered.RebuildState = RenderedChunk.ChunkRebuildState.UpdateReady;
					}
				}
			}
		}

		// Token: 0x06004596 RID: 17814 RVA: 0x000F8404 File Offset: 0x000F6604
		private void AllocateLightData()
		{
			for (int i = 0; i < 27; i++)
			{
				this.adjacentChunkLightAmounts[i] = new ushort[32768];
			}
			int maxChunksLoaded = this._gameInstance.MapModule.GetMaxChunksLoaded();
			for (int j = 0; j < maxChunksLoaded; j++)
			{
				this._selfLightAmountQueue.Enqueue(new ushort[32768]);
				this._borderedLightAmountQueue.Enqueue(new ushort[39304]);
			}
			Interlocked.Add(ref this._selfLightAmountAllocatedTotal, maxChunksLoaded);
			Interlocked.Add(ref this._borderedLightAmountAllocatedTotal, maxChunksLoaded);
		}

		// Token: 0x06004597 RID: 17815 RVA: 0x000F84A0 File Offset: 0x000F66A0
		public void EnqueueSelfLightAmountArray(ushort[] selfLightAmountData)
		{
			bool flag = this._selfLightAmountAllocatedTotal < this._gameInstance.MapModule.GetMaxChunksLoaded();
			if (flag)
			{
				Array.Clear(selfLightAmountData, 0, selfLightAmountData.Length);
				this._selfLightAmountQueue.Enqueue(selfLightAmountData);
			}
			else
			{
				Interlocked.Decrement(ref this._selfLightAmountAllocatedTotal);
			}
		}

		// Token: 0x06004598 RID: 17816 RVA: 0x000F84F4 File Offset: 0x000F66F4
		public ushort[] DequeueSelfLightAmountArray()
		{
			ushort[] result;
			bool flag = !this._selfLightAmountQueue.TryDequeue(out result);
			if (flag)
			{
				result = new ushort[32768];
				Interlocked.Increment(ref this._selfLightAmountAllocatedTotal);
			}
			return result;
		}

		// Token: 0x06004599 RID: 17817 RVA: 0x000F8534 File Offset: 0x000F6734
		public void EnqueueBorderedLightAmountArray(ushort[] borderedLightAmountData)
		{
			bool flag = this._borderedLightAmountAllocatedTotal < this._gameInstance.MapModule.GetMaxChunksLoaded();
			if (flag)
			{
				Array.Clear(borderedLightAmountData, 0, borderedLightAmountData.Length);
				this._borderedLightAmountQueue.Enqueue(borderedLightAmountData);
			}
			else
			{
				Interlocked.Decrement(ref this._borderedLightAmountAllocatedTotal);
			}
		}

		// Token: 0x0600459A RID: 17818 RVA: 0x000F8588 File Offset: 0x000F6788
		public ushort[] DequeueBorderedLightAmountArray()
		{
			ushort[] result;
			bool flag = !this._borderedLightAmountQueue.TryDequeue(out result);
			if (flag)
			{
				result = new ushort[39304];
				Interlocked.Increment(ref this._borderedLightAmountAllocatedTotal);
			}
			return result;
		}

		// Token: 0x0600459B RID: 17819 RVA: 0x000F85C8 File Offset: 0x000F67C8
		private void CalculateLighting(Chunk centerChunk)
		{
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					ChunkColumn chunkColumn = this._gameInstance.MapModule.GetChunkColumn(centerChunk.X + (i - 1), centerChunk.Z + (j - 1));
					bool flag = chunkColumn == null;
					if (flag)
					{
						for (int k = 0; k < 3; k++)
						{
							int num = (k * 3 + j) * 3 + i;
							this.adjacentChunkBlocks[num] = null;
							ushort[] array = this.adjacentChunkLightAmounts[num];
							for (int l = 0; l < array.Length; l++)
							{
								array[l] = 61440;
							}
						}
					}
					else
					{
						object disposeLock = chunkColumn.DisposeLock;
						lock (disposeLock)
						{
							for (int m = 0; m < 3; m++)
							{
								Chunk chunk = null;
								bool flag3 = !chunkColumn.Disposed;
								if (flag3)
								{
									chunk = chunkColumn.GetChunk(centerChunk.Y + (m - 1));
								}
								int num2 = (m * 3 + j) * 3 + i;
								bool flag4 = false;
								bool flag5 = chunk != null;
								if (flag5)
								{
									object disposeLock2 = chunk.DisposeLock;
									lock (disposeLock2)
									{
										bool flag7 = chunk.Rendered != null;
										if (flag7)
										{
											flag4 = true;
											bool selfLightNeedsUpdate = chunk.Data.SelfLightNeedsUpdate;
											if (selfLightNeedsUpdate)
											{
												chunk.Data.SelfLightNeedsUpdate = false;
												bool flag8 = chunk.Data.SelfLightAmounts == null;
												if (flag8)
												{
													chunk.Data.SelfLightAmounts = this.DequeueSelfLightAmountArray();
												}
												int num3 = (centerChunk.Y + (m - 1)) * 32;
												for (int n = 0; n < 32; n++)
												{
													for (int num4 = 0; num4 < 32; num4++)
													{
														ushort num5 = chunkColumn.Heights[(n << 5) + num4];
														for (int num6 = 0; num6 < 32; num6++)
														{
															int num7 = (num3 + num6 >= (int)num5) ? 15 : 0;
															int blockIdx = (num6 * 32 + n) * 32 + num4;
															ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[chunk.Data.Blocks.Get(blockIdx)];
															ushort num8 = (ushort)((int)clientBlockType.LightEmitted.R | (int)clientBlockType.LightEmitted.G << 4 | (int)clientBlockType.LightEmitted.B << 8 | num7 << 12);
															chunk.Data.SelfLightAmounts[(num6 * 32 + n) * 32 + num4] = num8;
														}
													}
												}
												for (int num9 = 0; num9 < 32; num9++)
												{
													for (int num10 = 0; num10 < 32; num10++)
													{
														for (int num11 = 0; num11 < 32; num11++)
														{
															int num12 = (num9 * 32 + num10) * 32 + num11;
															ClientBlockType clientBlockType2 = this._gameInstance.MapModule.ClientBlockTypes[chunk.Data.Blocks.Get(num12)];
															bool flag9 = clientBlockType2.Opacity == 0;
															if (!flag9)
															{
																for (int num13 = 0; num13 < 4; num13++)
																{
																	this._floodFillQueue.Push(num12);
																	while (this._floodFillQueue.Count > 0)
																	{
																		int num14 = this._floodFillQueue.Pop();
																		ushort num15 = chunk.Data.SelfLightAmounts[num14];
																		int num16 = num15 >> 4 * num13 & 15;
																		bool flag10 = num16 <= 1;
																		if (!flag10)
																		{
																			ClientBlockType clientBlockType3 = this._gameInstance.MapModule.ClientBlockTypes[chunk.Data.Blocks.Get(num14)];
																			bool flag11 = clientBlockType3.Opacity == 0;
																			if (!flag11)
																			{
																				bool flag12 = clientBlockType3.Opacity == 2 || clientBlockType3.Opacity == 1;
																				if (flag12)
																				{
																					num16--;
																					bool flag13 = num16 <= 1;
																					if (flag13)
																					{
																						continue;
																					}
																				}
																				int num17 = num14 % 32;
																				int num18 = num14 / 32 % 32;
																				int num19 = num14 / 1024;
																				bool flag14 = num17 < 31;
																				if (flag14)
																				{
																					this.FloodIntoBlock(num14 + 1, chunk.Data.Blocks.Get(num14 + 1), chunk.Data.SelfLightAmounts, num13, num16);
																				}
																				bool flag15 = num17 > 0;
																				if (flag15)
																				{
																					this.FloodIntoBlock(num14 + -1, chunk.Data.Blocks.Get(num14 + -1), chunk.Data.SelfLightAmounts, num13, num16);
																				}
																				bool flag16 = num19 < 31;
																				if (flag16)
																				{
																					this.FloodIntoBlock(num14 + 1024, chunk.Data.Blocks.Get(num14 + 1024), chunk.Data.SelfLightAmounts, num13, num16);
																				}
																				bool flag17 = num19 > 0;
																				if (flag17)
																				{
																					this.FloodIntoBlock(num14 + -1024, chunk.Data.Blocks.Get(num14 + -1024), chunk.Data.SelfLightAmounts, num13, num16);
																				}
																				bool flag18 = num18 < 31;
																				if (flag18)
																				{
																					this.FloodIntoBlock(num14 + 32, chunk.Data.Blocks.Get(num14 + 32), chunk.Data.SelfLightAmounts, num13, num16);
																				}
																				bool flag19 = num18 > 0;
																				if (flag19)
																				{
																					this.FloodIntoBlock(num14 + -32, chunk.Data.Blocks.Get(num14 + -32), chunk.Data.SelfLightAmounts, num13, num16);
																				}
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
											this.adjacentChunkBlocks[num2] = chunk.Data.Blocks;
											Buffer.BlockCopy(chunk.Data.SelfLightAmounts, 0, this.adjacentChunkLightAmounts[num2], 0, chunk.Data.SelfLightAmounts.Length * 2);
										}
									}
								}
								bool flag20 = !flag4;
								if (flag20)
								{
									this.adjacentChunkBlocks[num2] = null;
									ushort[] array2 = this.adjacentChunkLightAmounts[num2];
									for (int num20 = 0; num20 < array2.Length; num20++)
									{
										array2[num20] = 61440;
									}
								}
							}
						}
					}
				}
			}
			for (int num21 = 0; num21 < 2; num21++)
			{
				for (int num22 = 0; num22 < 3; num22++)
				{
					for (int num23 = 0; num23 < 3; num23++)
					{
						for (int num24 = 0; num24 < 3; num24++)
						{
							int num25 = (num24 * 3 + num23) * 3 + num22;
							bool flag21 = num23 < 2;
							if (flag21)
							{
								int targetChunkOffset = (num24 * 3 + (num23 + 1)) * 3 + num22;
								for (int num26 = 0; num26 < 32; num26++)
								{
									for (int num27 = 0; num27 < 32; num27++)
									{
										int num28 = (num27 * 32 + 32 - 1) * 32 + num26;
										ushort sourceLightAmount = this.adjacentChunkLightAmounts[num25][num28];
										int targetBlockIndex = num27 * 32 * 32 + num26;
										this.FloodIntoChunk((int)sourceLightAmount, targetChunkOffset, targetBlockIndex);
									}
								}
							}
							bool flag22 = num23 > 0;
							if (flag22)
							{
								int targetChunkOffset2 = (num24 * 3 + (num23 - 1)) * 3 + num22;
								for (int num29 = 0; num29 < 32; num29++)
								{
									for (int num30 = 0; num30 < 32; num30++)
									{
										int num31 = num30 * 32 * 32 + num29;
										ushort sourceLightAmount2 = this.adjacentChunkLightAmounts[num25][num31];
										int targetBlockIndex2 = (num30 * 32 + 32 - 1) * 32 + num29;
										this.FloodIntoChunk((int)sourceLightAmount2, targetChunkOffset2, targetBlockIndex2);
									}
								}
							}
							bool flag23 = num22 < 2;
							if (flag23)
							{
								int targetChunkOffset3 = (num24 * 3 + num23) * 3 + num22 + 1;
								for (int num32 = 0; num32 < 32; num32++)
								{
									for (int num33 = 0; num33 < 32; num33++)
									{
										int num34 = (num32 * 32 + num33) * 32 + 32 - 1;
										ushort sourceLightAmount3 = this.adjacentChunkLightAmounts[num25][num34];
										int targetBlockIndex3 = (num32 * 32 + num33) * 32;
										this.FloodIntoChunk((int)sourceLightAmount3, targetChunkOffset3, targetBlockIndex3);
									}
								}
							}
							bool flag24 = num22 > 0;
							if (flag24)
							{
								int targetChunkOffset4 = (num24 * 3 + num23) * 3 + num22 - 1;
								for (int num35 = 0; num35 < 32; num35++)
								{
									for (int num36 = 0; num36 < 32; num36++)
									{
										int num37 = (num35 * 32 + num36) * 32;
										ushort sourceLightAmount4 = this.adjacentChunkLightAmounts[num25][num37];
										int targetBlockIndex4 = (num35 * 32 + num36) * 32 + 32 - 1;
										this.FloodIntoChunk((int)sourceLightAmount4, targetChunkOffset4, targetBlockIndex4);
									}
								}
							}
							bool flag25 = num24 < 2;
							if (flag25)
							{
								int targetChunkOffset5 = ((num24 + 1) * 3 + num23) * 3 + num22;
								for (int num38 = 0; num38 < 32; num38++)
								{
									for (int num39 = 0; num39 < 32; num39++)
									{
										int num40 = (992 + num39) * 32 + num38;
										ushort sourceLightAmount5 = this.adjacentChunkLightAmounts[num25][num40];
										int targetBlockIndex5 = num39 * 32 + num38;
										this.FloodIntoChunk((int)sourceLightAmount5, targetChunkOffset5, targetBlockIndex5);
									}
								}
							}
							bool flag26 = num24 > 0;
							if (flag26)
							{
								int targetChunkOffset6 = ((num24 - 1) * 3 + num23) * 3 + num22;
								for (int num41 = 0; num41 < 32; num41++)
								{
									for (int num42 = 0; num42 < 32; num42++)
									{
										int num43 = num42 * 32 + num41;
										ushort sourceLightAmount6 = this.adjacentChunkLightAmounts[num25][num43];
										int targetBlockIndex6 = (992 + num42) * 32 + num41;
										this.FloodIntoChunk((int)sourceLightAmount6, targetChunkOffset6, targetBlockIndex6);
									}
								}
							}
						}
					}
				}
			}
			bool flag27 = centerChunk.Data.BorderedLightAmounts == null;
			if (flag27)
			{
				centerChunk.Data.BorderedLightAmounts = this.DequeueBorderedLightAmountArray();
			}
			ushort[] borderedLightAmounts = centerChunk.Data.BorderedLightAmounts;
			ushort[] src = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(0, 0, 0)];
			for (int num44 = 0; num44 < 32; num44++)
			{
				for (int num45 = 0; num45 < 32; num45++)
				{
					Buffer.BlockCopy(src, (num44 * 32 + num45) * 32 * 2, borderedLightAmounts, (((1 + num44) * 34 + (1 + num45)) * 34 + 1) * 2, 64);
				}
			}
			ushort[] src2 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(0, -1, 0)];
			ushort[] src3 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(0, 1, 0)];
			ushort[] src4 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(0, 0, -1)];
			ushort[] src5 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(0, 0, 1)];
			ushort[] array3 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(1, 0, 0)];
			ushort[] array4 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(-1, 0, 0)];
			for (int num46 = 0; num46 < 32; num46++)
			{
				Buffer.BlockCopy(src2, (992 + num46) * 32 * 2, borderedLightAmounts, ((num46 + 1) * 34 + 1) * 2, 64);
				Buffer.BlockCopy(src3, num46 * 32 * 2, borderedLightAmounts, ((1123 + num46) * 34 + 1) * 2, 64);
				Buffer.BlockCopy(src4, (num46 * 32 + 32 - 1) * 32 * 2, borderedLightAmounts, ((num46 + 1) * 34 * 34 + 1) * 2, 64);
				Buffer.BlockCopy(src5, num46 * 32 * 32 * 2, borderedLightAmounts, (((num46 + 1) * 34 + 34 - 1) * 34 + 1) * 2, 64);
			}
			for (int num47 = 0; num47 < 32; num47++)
			{
				for (int num48 = 0; num48 < 32; num48++)
				{
					borderedLightAmounts[((num47 + 1) * 34 + num48 + 1) * 34] = array4[(num47 * 32 + num48) * 32 + 32 - 1];
					borderedLightAmounts[((num47 + 1) * 34 + num48 + 1) * 34 + 34 - 1] = array3[(num47 * 32 + num48) * 32];
				}
			}
			ushort[] array5 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(-1, -1, -1)];
			int num49 = 0;
			borderedLightAmounts[num49] = array5[ChunkHelper.IndexOfBlockInChunk(31, 31, 31)];
			ushort[] src6 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(0, -1, -1)];
			int srcOffset = ChunkHelper.IndexOfBlockInChunk(0, 31, 31) * 2;
			int dstOffset = ChunkHelper.IndexOfBlockInBorderedChunk(1, 0, 0) * 2;
			Buffer.BlockCopy(src6, srcOffset, borderedLightAmounts, dstOffset, 64);
			ushort[] array6 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(1, -1, -1)];
			int num50 = ChunkHelper.IndexOfBlockInBorderedChunk(33, 0, 0);
			borderedLightAmounts[num50] = array6[ChunkHelper.IndexOfBlockInChunk(0, 31, 31)];
			ushort[] array7 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(-1, -1, 0)];
			int num51 = ChunkHelper.IndexOfBlockInChunk(31, 31, 0);
			int num52 = ChunkHelper.IndexOfBlockInBorderedChunk(0, 0, 1);
			for (int num53 = 0; num53 < 32; num53++)
			{
				borderedLightAmounts[num52] = array7[num51];
				num51 += 32;
				num52 += 34;
			}
			ushort[] array8 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(1, -1, 0)];
			int num54 = ChunkHelper.IndexOfBlockInChunk(0, 31, 0);
			int num55 = ChunkHelper.IndexOfBlockInBorderedChunk(33, 0, 1);
			for (int num56 = 0; num56 < 32; num56++)
			{
				borderedLightAmounts[num55] = array8[num54];
				num54 += 32;
				num55 += 34;
			}
			ushort[] array9 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(-1, -1, 1)];
			int num57 = ChunkHelper.IndexOfBlockInBorderedChunk(0, 0, 33);
			borderedLightAmounts[num57] = array9[ChunkHelper.IndexOfBlockInChunk(31, 31, 0)];
			ushort[] src7 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(0, -1, 1)];
			int srcOffset2 = ChunkHelper.IndexOfBlockInChunk(0, 31, 0) * 2;
			int dstOffset2 = ChunkHelper.IndexOfBlockInBorderedChunk(1, 0, 33) * 2;
			Buffer.BlockCopy(src7, srcOffset2, borderedLightAmounts, dstOffset2, 64);
			ushort[] array10 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(1, -1, 1)];
			int num58 = ChunkHelper.IndexOfBlockInBorderedChunk(33, 0, 33);
			borderedLightAmounts[num58] = array10[ChunkHelper.IndexOfBlockInChunk(0, 31, 0)];
			ushort[] array11 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(-1, 0, -1)];
			int num59 = ChunkHelper.IndexOfBlockInChunk(31, 0, 31);
			int num60 = ChunkHelper.IndexOfBlockInBorderedChunk(0, 1, 0);
			for (int num61 = 0; num61 < 32; num61++)
			{
				borderedLightAmounts[num60] = array11[num59];
				num59 += 1024;
				num60 += 1156;
			}
			ushort[] array12 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(1, 0, -1)];
			int num62 = ChunkHelper.IndexOfBlockInChunk(0, 0, 31);
			int num63 = ChunkHelper.IndexOfBlockInBorderedChunk(33, 1, 0);
			for (int num64 = 0; num64 < 32; num64++)
			{
				borderedLightAmounts[num63] = array12[num62];
				num62 += 1024;
				num63 += 1156;
			}
			ushort[] array13 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(-1, 0, 1)];
			int num65 = ChunkHelper.IndexOfBlockInChunk(31, 0, 0);
			int num66 = ChunkHelper.IndexOfBlockInBorderedChunk(0, 1, 33);
			for (int num67 = 0; num67 < 32; num67++)
			{
				borderedLightAmounts[num66] = array13[num65];
				num65 += 1024;
				num66 += 1156;
			}
			ushort[] array14 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(1, 0, 1)];
			int num68 = ChunkHelper.IndexOfBlockInChunk(0, 0, 0);
			int num69 = ChunkHelper.IndexOfBlockInBorderedChunk(33, 1, 33);
			for (int num70 = 0; num70 < 32; num70++)
			{
				borderedLightAmounts[num69] = array14[num68];
				num68 += 1024;
				num69 += 1156;
			}
			ushort[] array15 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(-1, 1, -1)];
			int num71 = ChunkHelper.IndexOfBlockInBorderedChunk(0, 33, 0);
			borderedLightAmounts[num71] = array15[ChunkHelper.IndexOfBlockInChunk(31, 0, 31)];
			ushort[] src8 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(0, 1, -1)];
			int srcOffset3 = ChunkHelper.IndexOfBlockInChunk(0, 0, 31) * 2;
			int dstOffset3 = ChunkHelper.IndexOfBlockInBorderedChunk(1, 33, 0) * 2;
			Buffer.BlockCopy(src8, srcOffset3, borderedLightAmounts, dstOffset3, 64);
			ushort[] array16 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(1, 1, -1)];
			int num72 = ChunkHelper.IndexOfBlockInBorderedChunk(33, 33, 0);
			borderedLightAmounts[num72] = array16[ChunkHelper.IndexOfBlockInChunk(0, 0, 31)];
			ushort[] array17 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(-1, 1, 0)];
			int num73 = ChunkHelper.IndexOfBlockInChunk(31, 0, 0);
			int num74 = ChunkHelper.IndexOfBlockInBorderedChunk(0, 33, 1);
			for (int num75 = 0; num75 < 32; num75++)
			{
				borderedLightAmounts[num74] = array17[num73];
				num73 += 32;
				num74 += 34;
			}
			ushort[] array18 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(1, 1, 0)];
			int num76 = ChunkHelper.IndexOfBlockInChunk(0, 0, 0);
			int num77 = ChunkHelper.IndexOfBlockInBorderedChunk(33, 33, 1);
			for (int num78 = 0; num78 < 32; num78++)
			{
				borderedLightAmounts[num77] = array18[num76];
				num76 += 32;
				num77 += 34;
			}
			ushort[] array19 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(-1, 1, 1)];
			int num79 = ChunkHelper.IndexOfBlockInBorderedChunk(0, 33, 33);
			borderedLightAmounts[num79] = array19[ChunkHelper.IndexOfBlockInChunk(31, 0, 0)];
			ushort[] src9 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(0, 1, 1)];
			int srcOffset4 = ChunkHelper.IndexOfBlockInChunk(0, 0, 0) * 2;
			int dstOffset4 = ChunkHelper.IndexOfBlockInBorderedChunk(1, 33, 33) * 2;
			Buffer.BlockCopy(src9, srcOffset4, borderedLightAmounts, dstOffset4, 64);
			ushort[] array20 = this.adjacentChunkLightAmounts[MapGeometryBuilder.GetAdjacentChunkIndex(1, 1, 1)];
			int num80 = ChunkHelper.IndexOfBlockInBorderedChunk(33, 33, 33);
			borderedLightAmounts[num80] = array20[ChunkHelper.IndexOfBlockInChunk(0, 0, 0)];
		}

		// Token: 0x0600459C RID: 17820 RVA: 0x000F97E8 File Offset: 0x000F79E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetAdjacentChunkIndex(int x, int y, int z)
		{
			return ((y + 1) * 3 + z + 1) * 3 + x + 1;
		}

		// Token: 0x0600459D RID: 17821 RVA: 0x000F980C File Offset: 0x000F7A0C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void FloodIntoBlock(int blockIndex, int blockId, ushort[] lightAmounts, int channel, int channelLightAmount)
		{
			bool flag = this._gameInstance.MapModule.ClientBlockTypes[blockId].Opacity == 2;
			if (flag)
			{
				channelLightAmount--;
			}
			ushort num = lightAmounts[blockIndex];
			int num2 = num >> 4 * channel & 15;
			bool flag2 = num2 < channelLightAmount - 1;
			if (flag2)
			{
				num2 = channelLightAmount - 1;
				lightAmounts[blockIndex] = (ushort)(((int)num & ~(15 << 4 * channel)) | num2 << 4 * channel);
				this._floodFillQueue.Push(blockIndex);
			}
		}

		// Token: 0x0600459E RID: 17822 RVA: 0x000F988C File Offset: 0x000F7A8C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void FloodIntoChunk(int sourceLightAmount, int targetChunkOffset, int targetBlockIndex)
		{
			PaletteChunkData paletteChunkData = this.adjacentChunkBlocks[targetChunkOffset];
			bool flag = paletteChunkData == null;
			if (!flag)
			{
				int num = paletteChunkData.Get(targetBlockIndex);
				ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[num];
				bool flag2 = clientBlockType.Opacity == 0;
				if (!flag2)
				{
					ushort[] array = this.adjacentChunkLightAmounts[targetChunkOffset];
					for (int i = 0; i < 4; i++)
					{
						int channelLightAmount = sourceLightAmount >> 4 * i & 15;
						this.FloodIntoBlock(targetBlockIndex, num, array, i, channelLightAmount);
						while (this._floodFillQueue.Count > 0)
						{
							int num2 = this._floodFillQueue.Pop();
							ushort num3 = array[num2];
							int num4 = num3 >> 4 * i & 15;
							bool flag3 = num4 <= 1;
							if (!flag3)
							{
								ClientBlockType clientBlockType2 = this._gameInstance.MapModule.ClientBlockTypes[paletteChunkData.Get(num2)];
								bool flag4 = clientBlockType2.Opacity == 0;
								if (!flag4)
								{
									bool flag5 = clientBlockType2.Opacity == 2 || clientBlockType2.Opacity == 1;
									if (flag5)
									{
										num4--;
										bool flag6 = num4 <= 1;
										if (flag6)
										{
											continue;
										}
									}
									int num5 = num2 % 32;
									int num6 = num2 / 32 % 32;
									int num7 = num2 / 1024;
									bool flag7 = num5 < 31;
									if (flag7)
									{
										this.FloodIntoBlock(num2 + 1, paletteChunkData.Get(num2 + 1), array, i, num4);
									}
									bool flag8 = num5 > 0;
									if (flag8)
									{
										this.FloodIntoBlock(num2 + -1, paletteChunkData.Get(num2 + -1), array, i, num4);
									}
									bool flag9 = num7 < 31;
									if (flag9)
									{
										this.FloodIntoBlock(num2 + 1024, paletteChunkData.Get(num2 + 1024), array, i, num4);
									}
									bool flag10 = num7 > 0;
									if (flag10)
									{
										this.FloodIntoBlock(num2 + -1024, paletteChunkData.Get(num2 + -1024), array, i, num4);
									}
									bool flag11 = num6 < 31;
									if (flag11)
									{
										this.FloodIntoBlock(num2 + 32, paletteChunkData.Get(num2 + 32), array, i, num4);
									}
									bool flag12 = num6 > 0;
									if (flag12)
									{
										this.FloodIntoBlock(num2 + -32, paletteChunkData.Get(num2 + -32), array, i, num4);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x040022F1 RID: 8945
		private const int BytesPerBlock = 4;

		// Token: 0x040022F2 RID: 8946
		private const int BytesPerBlockLine = 128;

		// Token: 0x040022F3 RID: 8947
		private const int SliceBlockCount = 1024;

		// Token: 0x040022F4 RID: 8948
		private const int BorderedSliceBlockCount = 1156;

		// Token: 0x040022F5 RID: 8949
		private readonly int[] _borderedChunkBlocks = new int[39304];

		// Token: 0x040022F6 RID: 8950
		private readonly Dictionary<int, float> _borderedChunkHitTimers = new Dictionary<int, float>();

		// Token: 0x040022F7 RID: 8951
		private readonly int[] _missingChunkLine = new int[32];

		// Token: 0x040022F8 RID: 8952
		private readonly int[] _emptyChunkLine = new int[32];

		// Token: 0x040022F9 RID: 8953
		private readonly ushort[] _borderedChunkLightAmounts = new ushort[39304];

		// Token: 0x040022FA RID: 8954
		private const int BytesPerTint = 4;

		// Token: 0x040022FB RID: 8955
		private const int BytesPerTintLine = 128;

		// Token: 0x040022FC RID: 8956
		private readonly uint[] _blendedBlockCornerBiomeTints = new uint[1156];

		// Token: 0x040022FD RID: 8957
		private readonly ushort[][] _borderedColumnEnvironmentIds = new ushort[1156][];

		// Token: 0x040022FE RID: 8958
		private readonly GameInstance _gameInstance;

		// Token: 0x040022FF RID: 8959
		private readonly ChunkGeometryBuilder _chunkGeometryBuilder;

		// Token: 0x04002300 RID: 8960
		private CancellationTokenSource _threadCancellationTokenSource;

		// Token: 0x04002301 RID: 8961
		private CancellationToken _threadCancellationToken;

		// Token: 0x04002302 RID: 8962
		private Thread _thread;

		// Token: 0x04002303 RID: 8963
		private readonly SpiralIterator _spiralIterator = new SpiralIterator();

		// Token: 0x04002304 RID: 8964
		private readonly object _spiralIteratorLock = new object();

		// Token: 0x04002305 RID: 8965
		private readonly AutoResetEvent _restartSpiralEvent = new AutoResetEvent(false);

		// Token: 0x04002306 RID: 8966
		private static readonly int[] RebuildOffsets = new int[]
		{
			0,
			-1,
			1
		};

		// Token: 0x04002307 RID: 8967
		private float[] _nearestChunkDistances = new float[27];

		// Token: 0x04002308 RID: 8968
		private IntVector3[] _nearestChunks = new IntVector3[27];

		// Token: 0x04002309 RID: 8969
		private int _startChunkX = int.MaxValue;

		// Token: 0x0400230A RID: 8970
		private int _startChunkY = int.MaxValue;

		// Token: 0x0400230B RID: 8971
		private int _startChunkZ = int.MaxValue;

		// Token: 0x0400230C RID: 8972
		private int _spiralRadius;

		// Token: 0x0400230D RID: 8973
		private Stopwatch _disposeRequestsStopwatch = new Stopwatch();

		// Token: 0x0400230E RID: 8974
		private FastIntQueue _floodFillQueue = new FastIntQueue(1024);

		// Token: 0x0400230F RID: 8975
		private const int BytesPerLightBlock = 2;

		// Token: 0x04002310 RID: 8976
		private const int BytesPerLightBlockLine = 64;

		// Token: 0x04002311 RID: 8977
		private const int XPlusOne = 1;

		// Token: 0x04002312 RID: 8978
		private const int XMinusOne = -1;

		// Token: 0x04002313 RID: 8979
		private const int ZPlusOne = 32;

		// Token: 0x04002314 RID: 8980
		private const int ZMinusOne = -32;

		// Token: 0x04002315 RID: 8981
		private const int YPlusOne = 1024;

		// Token: 0x04002316 RID: 8982
		private const int YMinusOne = -1024;

		// Token: 0x04002317 RID: 8983
		private readonly PaletteChunkData[] adjacentChunkBlocks = new PaletteChunkData[27];

		// Token: 0x04002318 RID: 8984
		private readonly ushort[][] adjacentChunkLightAmounts = new ushort[27][];

		// Token: 0x04002319 RID: 8985
		private readonly ConcurrentQueue<ushort[]> _selfLightAmountQueue = new ConcurrentQueue<ushort[]>();

		// Token: 0x0400231A RID: 8986
		private int _selfLightAmountAllocatedTotal = 0;

		// Token: 0x0400231B RID: 8987
		private readonly ConcurrentQueue<ushort[]> _borderedLightAmountQueue = new ConcurrentQueue<ushort[]>();

		// Token: 0x0400231C RID: 8988
		private int _borderedLightAmountAllocatedTotal = 0;
	}
}
