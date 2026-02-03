using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using HytaleClient.Audio;
using HytaleClient.Core;
using HytaleClient.Data.Map;
using HytaleClient.Data.Weather;
using HytaleClient.Graphics.Particles;
using HytaleClient.InGame.Modules.Map;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;

namespace HytaleClient.Graphics.Map
{
	// Token: 0x02000A8F RID: 2703
	internal class ChunkGeometryBuilder
	{
		// Token: 0x06005527 RID: 21799 RVA: 0x001888B8 File Offset: 0x00186AB8
		public ChunkGeometryBuilder()
		{
			this.EnsureEnoughChunkUpdateTasks();
		}

		// Token: 0x06005528 RID: 21800 RVA: 0x00188960 File Offset: 0x00186B60
		public void EnsureEnoughChunkUpdateTasks()
		{
			int num = 5 - this.ChunkUpdateTaskQueue.Count;
			for (int i = 0; i < num; i++)
			{
				this.ChunkUpdateTaskQueue.Add(new RenderedChunk.ChunkUpdateTask
				{
					OpaqueData = new ChunkGeometryData
					{
						Vertices = new ChunkVertex[60000],
						Indices = new uint[90000]
					},
					AlphaBlendedData = new ChunkGeometryData
					{
						Vertices = new ChunkVertex[60000],
						Indices = new uint[90000]
					},
					AlphaTestedData = new ChunkGeometryData
					{
						Vertices = new ChunkVertex[60000],
						Indices = new uint[90000]
					}
				});
			}
		}

		// Token: 0x06005529 RID: 21801 RVA: 0x00188A30 File Offset: 0x00186C30
		public void SetBlockTypes(ClientBlockType[] clientBlockTypes)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this._clientBlockTypes = clientBlockTypes;
		}

		// Token: 0x0600552A RID: 21802 RVA: 0x00188A45 File Offset: 0x00186C45
		public void SetBlockHitboxes(BlockHitbox[] blockHitboxes)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this._blockHitboxes = blockHitboxes;
		}

		// Token: 0x0600552B RID: 21803 RVA: 0x00188A5A File Offset: 0x00186C5A
		public void SetLightLevels(float[] lightLevels)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this._lightLevels = lightLevels;
		}

		// Token: 0x0600552C RID: 21804 RVA: 0x00188A6F File Offset: 0x00186C6F
		public void SetEnvironments(ClientWorldEnvironment[] environments)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this._environments = environments;
		}

		// Token: 0x0600552D RID: 21805 RVA: 0x00188A84 File Offset: 0x00186C84
		public void SetAtlasSizes(Point[] atlasSizes)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this._atlasSizes = atlasSizes;
		}

		// Token: 0x0600552E RID: 21806 RVA: 0x00188A99 File Offset: 0x00186C99
		public void SetLODEnabled(bool LODEnabled)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this._LODEnabled = LODEnabled;
		}

		// Token: 0x0600552F RID: 21807 RVA: 0x00188AB0 File Offset: 0x00186CB0
		public RenderedChunk.ChunkUpdateTask BuildGeometry(int chunkX, int chunkY, int chunkZ, ChunkColumn chunkColumn, int[] borderedChunkBlocks, Dictionary<int, float> borderedChunkBlockHitTimers, ushort[] borderedChunkLightAmounts, uint[] borderedColumnTints, ushort[][] borderedColumnEnvironmentIds, int atlasTextureWidth, int atlasTextureHeight, CancellationToken cancellationToken)
		{
			RenderedChunk.ChunkUpdateTask chunkUpdateTask;
			try
			{
				chunkUpdateTask = this.ChunkUpdateTaskQueue.Take(cancellationToken);
			}
			catch (OperationCanceledException)
			{
				return null;
			}
			bool flag = borderedChunkBlockHitTimers.Count > 0;
			int num = 1191;
			int num2 = 0;
			chunkUpdateTask.OpaqueData.VerticesCount = 0;
			chunkUpdateTask.OpaqueData.IndicesCount = 0;
			chunkUpdateTask.AlphaBlendedData.VerticesCount = 0;
			chunkUpdateTask.AlphaBlendedData.IndicesCount = 0;
			chunkUpdateTask.AlphaTestedData.VerticesCount = 0;
			chunkUpdateTask.AlphaTestedData.IndicesCount = 0;
			chunkUpdateTask.AlphaTestedAnimatedLowLODIndicesCount = 0;
			chunkUpdateTask.AlphaTestedLowLODIndicesCount = 0;
			chunkUpdateTask.AlphaTestedHighLODIndicesCount = 0;
			chunkUpdateTask.AlphaTestedAnimatedHighLODIndicesCount = 0;
			chunkUpdateTask.SolidPlaneMinY = 0;
			chunkUpdateTask.IsUnderground = false;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			Array.Clear(this._solidBlockHeight, 0, this._solidBlockHeight.Length);
			for (int i = 0; i < 32; i++)
			{
				for (int j = 0; j < 32; j++)
				{
					for (int k = 0; k < 32; k++)
					{
						ClientBlockType clientBlockType = this._clientBlockTypes[borderedChunkBlocks[num]];
						byte b = (clientBlockType.MaxFillLevel == 0) ? 8 : clientBlockType.MaxFillLevel;
						bool flag2 = clientBlockType.DrawType == 0;
						if (flag2)
						{
							num++;
							num2++;
						}
						else
						{
							bool flag3 = clientBlockType.DrawType == 2 && clientBlockType.Opacity == 0;
							if (flag3)
							{
								int num6 = k + 32 * j;
								this._solidBlockHeight[num6] = (byte)(i + 1);
							}
							bool flag4 = flag && borderedChunkBlockHitTimers.ContainsKey(num);
							bool flag5 = clientBlockType.IsAnimated() || flag4;
							if (flag5)
							{
								num3++;
							}
							int num7 = 0;
							bool shouldRenderCube = clientBlockType.ShouldRenderCube;
							if (shouldRenderCube)
							{
								ChunkGeometryData chunkGeometryData = flag4 ? chunkUpdateTask.AlphaTestedData : ((!clientBlockType.RequiresAlphaBlending) ? chunkUpdateTask.OpaqueData : chunkUpdateTask.AlphaBlendedData);
								for (int l = 0; l < 6; l++)
								{
									int num8 = num + ChunkGeometryBuilder.AdjacentBlockOffsetsBySide[l].Main;
									int num9 = borderedChunkBlocks[num8];
									bool flag6 = num9 == int.MaxValue;
									if (!flag6)
									{
										ClientBlockType clientBlockType2 = this._clientBlockTypes[num9];
										byte b2 = (clientBlockType2.MaxFillLevel == 0) ? 8 : clientBlockType2.MaxFillLevel;
										ClientBlockType adjacentTopClientBlockType = null;
										int num10 = num8 + ChunkGeometryBuilder.AdjacentBlockOffsetsBySide[0].Main;
										bool flag7 = num10 < borderedChunkBlocks.Length && borderedChunkBlocks[num10] != int.MaxValue;
										if (flag7)
										{
											adjacentTopClientBlockType = this._clientBlockTypes[borderedChunkBlocks[num10]];
										}
										bool flag8 = false;
										bool flag9 = num9 == 0 || (!clientBlockType2.ShouldRenderCube && clientBlockType2.VerticalFill == b2) || (clientBlockType2.RequiresAlphaBlending && !clientBlockType.RequiresAlphaBlending) || (flag && borderedChunkBlockHitTimers.ContainsKey(num8));
										if (flag9)
										{
											flag8 = true;
										}
										else
										{
											bool flag10 = l == 0;
											if (flag10)
											{
												bool flag11 = clientBlockType.VerticalFill != b;
												if (flag11)
												{
													flag8 = true;
												}
												else
												{
													bool requiresAlphaBlending = clientBlockType.RequiresAlphaBlending;
													if (requiresAlphaBlending)
													{
														int num11 = num + ChunkGeometryBuilder.AdjacentBlockOffsetsBySide[0].Main;
														int num12 = borderedChunkBlocks[num11];
														bool flag12 = num12 == int.MaxValue || !this._clientBlockTypes[num12].ShouldRenderCube;
														if (flag12)
														{
															flag8 = true;
														}
													}
												}
											}
										}
										bool flag13 = l != 0 && clientBlockType2.VerticalFill < b2;
										if (flag13)
										{
											bool flag14 = clientBlockType.VerticalFill == b;
											if (flag14)
											{
												flag8 = true;
											}
											else
											{
												bool flag15 = l == 1;
												if (flag15)
												{
													flag8 = true;
												}
												else
												{
													bool flag16 = (num7 & 1) == 0;
													if (flag16)
													{
														num7 |= 1;
														chunkGeometryData.VerticesCount += 4;
														chunkGeometryData.IndicesCount += 6;
														bool flag17 = flag4;
														if (flag17)
														{
															chunkUpdateTask.AlphaTestedAnimatedLowLODIndicesCount += 6;
														}
													}
													flag8 = (clientBlockType2.VerticalFill < clientBlockType.VerticalFill || clientBlockType2.RequiresAlphaBlending);
												}
											}
										}
										bool flag18 = flag8;
										if (flag18)
										{
											int num13 = 1 << l;
											bool flag19 = (num7 & num13) == 0;
											if (flag19)
											{
												num7 |= num13;
												chunkGeometryData.VerticesCount += 4;
												chunkGeometryData.IndicesCount += 6;
												bool flag20 = flag4;
												if (flag20)
												{
													chunkUpdateTask.AlphaTestedAnimatedLowLODIndicesCount += 6;
												}
											}
										}
										bool flag21 = l >= 2 && (num7 & 1) != 0;
										if (flag21)
										{
											bool flag22 = ChunkGeometryBuilder.ShouldAddTransition(clientBlockType, clientBlockType2, adjacentTopClientBlockType);
											if (flag22)
											{
												chunkUpdateTask.AlphaTestedData.VerticesCount += 4;
												chunkUpdateTask.AlphaTestedData.IndicesCount += 6;
												chunkUpdateTask.AlphaTestedLowLODIndicesCount += 6;
											}
										}
									}
								}
							}
							this._chunkVisibleSideFlags[num2] = (byte)num7;
							bool flag23 = clientBlockType.Particles != null && (!clientBlockType.ShouldRenderCube || clientBlockType.RequiresAlphaBlending || num7 > 0);
							if (flag23)
							{
								for (int m = 0; m < clientBlockType.Particles.Length; m++)
								{
									bool flag24 = clientBlockType.Particles[m].SystemId != null;
									if (flag24)
									{
										num4++;
									}
								}
							}
							bool flag25 = ChunkGeometryBuilder.ShouldRegisterSound(clientBlockType);
							if (flag25)
							{
								num5++;
							}
							bool flag26 = clientBlockType.RenderedBlockyModel != null && (!clientBlockType.ShouldRenderCube || clientBlockType.RequiresAlphaBlending || num7 > 0);
							if (flag26)
							{
								chunkUpdateTask.AlphaTestedData.VerticesCount += clientBlockType.RenderedBlockyModel.StaticVertices.Length;
								chunkUpdateTask.AlphaTestedData.IndicesCount += clientBlockType.RenderedBlockyModel.StaticIndices.Length;
								bool flag27 = clientBlockType.IsAnimated() || flag4;
								if (flag27)
								{
									int num14 = this._LODEnabled ? ((int)clientBlockType.RenderedBlockyModel.LowLODIndicesCount) : clientBlockType.RenderedBlockyModel.StaticIndices.Length;
									chunkUpdateTask.AlphaTestedAnimatedLowLODIndicesCount += num14;
									chunkUpdateTask.AlphaTestedAnimatedHighLODIndicesCount += clientBlockType.RenderedBlockyModel.StaticIndices.Length - num14;
								}
								else
								{
									int num15 = this._LODEnabled ? ((int)clientBlockType.RenderedBlockyModel.LowLODIndicesCount) : clientBlockType.RenderedBlockyModel.StaticIndices.Length;
									chunkUpdateTask.AlphaTestedLowLODIndicesCount += num15;
									chunkUpdateTask.AlphaTestedHighLODIndicesCount += clientBlockType.RenderedBlockyModel.StaticIndices.Length - num15;
								}
							}
							num++;
							num2++;
						}
					}
					num += 2;
				}
				num += 68;
			}
			int num16 = 1000;
			for (int n = 0; n < this._solidBlockHeight.Length; n++)
			{
				num16 = Math.Min(num16, (int)this._solidBlockHeight[n]);
				bool flag28 = this._solidBlockHeight[n] == 0;
				if (flag28)
				{
					num16 = 0;
					break;
				}
			}
			bool flag29 = num16 > 0;
			if (flag29)
			{
				chunkUpdateTask.SolidPlaneMinY = num16;
			}
			bool flag30 = chunkUpdateTask.OpaqueData.IndicesCount == 0 && chunkUpdateTask.AlphaTestedData.IndicesCount == 0 && chunkUpdateTask.AlphaBlendedData.IndicesCount == 0;
			RenderedChunk.ChunkUpdateTask result;
			if (flag30)
			{
				this.ChunkUpdateTaskQueue.Add(chunkUpdateTask);
				result = null;
			}
			else
			{
				bool flag31 = chunkUpdateTask.OpaqueData.VerticesCount > chunkUpdateTask.OpaqueData.Vertices.Length;
				if (flag31)
				{
					Array.Resize<ChunkVertex>(ref chunkUpdateTask.OpaqueData.Vertices, chunkUpdateTask.OpaqueData.VerticesCount);
				}
				bool flag32 = chunkUpdateTask.OpaqueData.IndicesCount > chunkUpdateTask.OpaqueData.Indices.Length;
				if (flag32)
				{
					Array.Resize<uint>(ref chunkUpdateTask.OpaqueData.Indices, chunkUpdateTask.OpaqueData.IndicesCount);
				}
				bool flag33 = chunkUpdateTask.AlphaBlendedData.VerticesCount > chunkUpdateTask.AlphaBlendedData.Vertices.Length;
				if (flag33)
				{
					Array.Resize<ChunkVertex>(ref chunkUpdateTask.AlphaBlendedData.Vertices, chunkUpdateTask.AlphaBlendedData.VerticesCount);
				}
				bool flag34 = chunkUpdateTask.AlphaBlendedData.IndicesCount > chunkUpdateTask.AlphaBlendedData.Indices.Length;
				if (flag34)
				{
					Array.Resize<uint>(ref chunkUpdateTask.AlphaBlendedData.Indices, chunkUpdateTask.AlphaBlendedData.IndicesCount);
				}
				bool flag35 = chunkUpdateTask.AlphaTestedData.VerticesCount > chunkUpdateTask.AlphaTestedData.Vertices.Length;
				if (flag35)
				{
					Array.Resize<ChunkVertex>(ref chunkUpdateTask.AlphaTestedData.Vertices, chunkUpdateTask.AlphaTestedData.VerticesCount);
				}
				bool flag36 = chunkUpdateTask.AlphaTestedData.IndicesCount > chunkUpdateTask.AlphaTestedData.Indices.Length;
				if (flag36)
				{
					Array.Resize<uint>(ref chunkUpdateTask.AlphaTestedData.Indices, chunkUpdateTask.AlphaTestedData.IndicesCount);
				}
				bool flag37 = num3 > 0;
				if (flag37)
				{
					chunkUpdateTask.AnimatedBlocks = new RenderedChunk.AnimatedBlock[num3];
				}
				bool flag38 = num4 > 0;
				if (flag38)
				{
					chunkUpdateTask.MapParticles = new RenderedChunk.MapParticle[num4];
				}
				bool flag39 = num5 > 0;
				if (flag39)
				{
					chunkUpdateTask.SoundObjects = new RenderedChunk.MapSoundObject[num5];
				}
				num = 1191;
				num2 = 0;
				chunkUpdateTask.OpaqueData.VerticesOffset = 0U;
				chunkUpdateTask.OpaqueData.IndicesOffset = 0;
				chunkUpdateTask.AlphaBlendedData.VerticesOffset = 0U;
				chunkUpdateTask.AlphaBlendedData.IndicesOffset = 0;
				chunkUpdateTask.AlphaTestedData.VerticesOffset = 0U;
				chunkUpdateTask.AlphaTestedData.IndicesOffset = 0;
				int num17 = 0;
				int num18 = 0;
				int num19 = 0;
				int num20 = 0;
				int num21 = 0;
				int num22 = 0;
				int num23 = 0;
				Array.Clear(this._environmentTracker, 0, this._environmentTracker.Length);
				for (int num24 = 0; num24 < 32; num24++)
				{
					for (int num25 = 0; num25 < 32; num25++)
					{
						for (int num26 = 0; num26 < 32; num26++)
						{
							ClientBlockType clientBlockType3 = this._clientBlockTypes[borderedChunkBlocks[num]];
							bool flag40 = clientBlockType3.DrawType == 0;
							if (flag40)
							{
								num++;
								num2++;
							}
							else
							{
								bool flag41 = flag && borderedChunkBlockHitTimers.ContainsKey(num);
								byte b3 = this._chunkVisibleSideFlags[num2];
								BoundingBox boundingBox = this._blockHitboxes[clientBlockType3.HitboxType].BoundingBox;
								float hitboxHeight = boundingBox.Max.Y * 32f;
								bool flag42 = clientBlockType3.CubeShaderEffect == ClientBlockType.ClientShaderEffect.Water;
								if (flag42)
								{
									int num27 = chunkY * 32 + num24;
									int num28 = ChunkHelper.IndexInBorderedChunkColumn(num26 + 1, num25 + 1);
									int num29 = num28 * 3;
									bool flag43 = num27 >= (int)this._environmentTracker[num29];
									if (flag43)
									{
										ChunkHelper.GetEnvironmentId(borderedColumnEnvironmentIds[num28], num27, this._environmentTracker, num29);
									}
									ushort num30 = this._environmentTracker[num29 + 2];
									int waterTint = this._environments[(int)num30].WaterTint;
									byte b4 = (byte)(waterTint >> 16);
									byte b5 = (byte)(waterTint >> 8);
									byte b6 = (byte)waterTint;
									int num31 = num26 + 1;
									int num32 = num24 + 1;
									int num33 = num25 + 1;
									for (int num34 = 0; num34 < this._cornerEnvironmentWaterTints.Length; num34++)
									{
										int num35 = 1;
										int num36 = num34 % 4;
										ClientBlockType.ClientShaderEffect clientShaderEffect = (waterTint != -1) ? ClientBlockType.ClientShaderEffect.WaterEnvironmentColor : ClientBlockType.ClientShaderEffect.Water;
										bool flag44 = clientShaderEffect == ClientBlockType.ClientShaderEffect.WaterEnvironmentColor;
										bool flag45 = !flag44;
										uint num37 = (uint)b4;
										uint num38 = (uint)b5;
										uint num39 = (uint)b6;
										int num40 = 0;
										int num41 = (num34 < 4) ? 1 : -1;
										int num42 = 0;
										switch (num36)
										{
										case 0:
											num40 = 1;
											num42 = -1;
											break;
										case 1:
											num40 = -1;
											num42 = -1;
											break;
										case 2:
											num40 = -1;
											num42 = 1;
											break;
										case 3:
											num40 = 1;
											num42 = 1;
											break;
										}
										int x = num31 + num40;
										int y = num32;
										int z = num33;
										int num43 = ChunkHelper.IndexOfBlockInBorderedChunk(x, y, z);
										int num44 = borderedChunkBlocks[num43];
										bool flag46 = num44 != int.MaxValue && this._clientBlockTypes[num44].CubeShaderEffect == ClientBlockType.ClientShaderEffect.Water;
										bool flag47 = flag46;
										if (flag47)
										{
											int num45 = ChunkHelper.IndexInBorderedChunkColumn(x, z);
											num29 = num45 * 3;
											bool flag48 = num27 >= (int)this._environmentTracker[num29];
											if (flag48)
											{
												ChunkHelper.GetEnvironmentId(borderedColumnEnvironmentIds[num45], num27, this._environmentTracker, num29);
											}
											ushort num46 = this._environmentTracker[num29 + 2];
											int waterTint2 = this._environments[(int)num46].WaterTint;
											bool flag49 = waterTint2 != -1;
											if (flag49)
											{
												num37 += (uint)((byte)(waterTint2 >> 16));
												num38 += (uint)((byte)(waterTint2 >> 8));
												num39 += (uint)((byte)waterTint2);
												flag44 = true;
											}
											else
											{
												num37 += 255U;
												num38 += 255U;
												num39 += 255U;
												flag45 = true;
											}
											num35++;
										}
										x = num31;
										y = num32;
										z = num33 + num42;
										int num47 = ChunkHelper.IndexOfBlockInBorderedChunk(x, y, z);
										int num48 = borderedChunkBlocks[num47];
										bool flag50 = num48 != int.MaxValue && this._clientBlockTypes[num48].CubeShaderEffect == ClientBlockType.ClientShaderEffect.Water;
										bool flag51 = flag50;
										if (flag51)
										{
											int num49 = ChunkHelper.IndexInBorderedChunkColumn(x, z);
											num29 = num49 * 3;
											bool flag52 = num27 >= (int)this._environmentTracker[num29];
											if (flag52)
											{
												ChunkHelper.GetEnvironmentId(borderedColumnEnvironmentIds[num49], num27, this._environmentTracker, num29);
											}
											ushort num50 = this._environmentTracker[num29 + 2];
											int waterTint3 = this._environments[(int)num50].WaterTint;
											bool flag53 = waterTint3 != -1;
											if (flag53)
											{
												num37 += (uint)((byte)(waterTint3 >> 16));
												num38 += (uint)((byte)(waterTint3 >> 8));
												num39 += (uint)((byte)waterTint3);
												flag44 = true;
											}
											else
											{
												num37 += 255U;
												num38 += 255U;
												num39 += 255U;
												flag45 = true;
											}
											num35++;
										}
										x = num31 + num40;
										y = num32;
										z = num33 + num42;
										int num51 = ChunkHelper.IndexOfBlockInBorderedChunk(x, y, z);
										int num52 = borderedChunkBlocks[num51];
										bool flag54 = num52 != int.MaxValue && this._clientBlockTypes[num52].CubeShaderEffect == ClientBlockType.ClientShaderEffect.Water;
										bool flag55 = flag54;
										if (flag55)
										{
											int num53 = ChunkHelper.IndexInBorderedChunkColumn(x, z);
											num29 = num53 * 3;
											bool flag56 = num27 >= (int)this._environmentTracker[num29];
											if (flag56)
											{
												ChunkHelper.GetEnvironmentId(borderedColumnEnvironmentIds[num53], num27, this._environmentTracker, num29);
											}
											ushort num54 = this._environmentTracker[num29 + 2];
											int waterTint4 = this._environments[(int)num54].WaterTint;
											bool flag57 = waterTint4 != -1;
											if (flag57)
											{
												num37 += (uint)((byte)(waterTint4 >> 16));
												num38 += (uint)((byte)(waterTint4 >> 8));
												num39 += (uint)((byte)waterTint4);
												flag44 = true;
											}
											else
											{
												num37 += 255U;
												num38 += 255U;
												num39 += 255U;
												flag45 = true;
											}
											num35++;
										}
										x = num31 + num40;
										y = num32 + num41;
										z = num33;
										int num55 = ChunkHelper.IndexOfBlockInBorderedChunk(x, y, z);
										int num56 = borderedChunkBlocks[num55];
										bool flag58 = num56 != int.MaxValue && this._clientBlockTypes[num56].CubeShaderEffect == ClientBlockType.ClientShaderEffect.Water;
										bool flag59 = flag58;
										if (flag59)
										{
											int num57 = ChunkHelper.IndexInBorderedChunkColumn(x, z);
											num29 = num57 * 3;
											bool flag60 = num27 + num41 >= (int)this._environmentTracker[num29];
											if (flag60)
											{
												ChunkHelper.GetEnvironmentId(borderedColumnEnvironmentIds[num57], num27 + num41, this._environmentTracker, num29);
											}
											ushort num58 = this._environmentTracker[num29 + 2];
											int waterTint5 = this._environments[(int)num58].WaterTint;
											bool flag61 = waterTint5 != -1;
											if (flag61)
											{
												num37 += (uint)((byte)(waterTint5 >> 16));
												num38 += (uint)((byte)(waterTint5 >> 8));
												num39 += (uint)((byte)waterTint5);
												flag44 = true;
											}
											else
											{
												num37 += 255U;
												num38 += 255U;
												num39 += 255U;
												flag45 = true;
											}
											num35++;
										}
										x = num31;
										y = num32 + num41;
										z = num33 + num42;
										int num59 = ChunkHelper.IndexOfBlockInBorderedChunk(x, y, z);
										int num60 = borderedChunkBlocks[num59];
										bool flag62 = num60 != int.MaxValue && this._clientBlockTypes[num60].CubeShaderEffect == ClientBlockType.ClientShaderEffect.Water;
										bool flag63 = flag62;
										if (flag63)
										{
											int num61 = ChunkHelper.IndexInBorderedChunkColumn(x, z);
											num29 = num61 * 3;
											bool flag64 = num27 + num41 >= (int)this._environmentTracker[num29];
											if (flag64)
											{
												ChunkHelper.GetEnvironmentId(borderedColumnEnvironmentIds[num61], num27 + num41, this._environmentTracker, num29);
											}
											ushort num62 = this._environmentTracker[num29 + 2];
											int waterTint6 = this._environments[(int)num62].WaterTint;
											bool flag65 = waterTint6 != -1;
											if (flag65)
											{
												num37 += (uint)((byte)(waterTint6 >> 16));
												num38 += (uint)((byte)(waterTint6 >> 8));
												num39 += (uint)((byte)waterTint6);
												flag44 = true;
											}
											else
											{
												num37 += 255U;
												num38 += 255U;
												num39 += 255U;
												flag45 = true;
											}
											num35++;
										}
										int num63 = ChunkHelper.IndexOfBlockInBorderedChunk(num31 + num40, num32 + num41, num33 + num42);
										int num64 = borderedChunkBlocks[num63];
										bool flag66 = num64 != int.MaxValue && this._clientBlockTypes[num64].CubeShaderEffect == ClientBlockType.ClientShaderEffect.Water;
										bool flag67 = flag66;
										if (flag67)
										{
											int num65 = ChunkHelper.IndexInBorderedChunkColumn(num31 + num40, num33 + num42);
											num29 = num65 * 3;
											bool flag68 = num27 + num41 >= (int)this._environmentTracker[num29];
											if (flag68)
											{
												ChunkHelper.GetEnvironmentId(borderedColumnEnvironmentIds[num65], num27 + num41, this._environmentTracker, num29);
											}
											ushort num66 = this._environmentTracker[num29 + 2];
											int waterTint7 = this._environments[(int)num66].WaterTint;
											bool flag69 = waterTint7 != -1;
											if (flag69)
											{
												num37 += (uint)((byte)(waterTint7 >> 16));
												num38 += (uint)((byte)(waterTint7 >> 8));
												num39 += (uint)((byte)waterTint7);
												flag44 = true;
											}
											else
											{
												num37 += 255U;
												num38 += 255U;
												num39 += 255U;
												flag45 = true;
											}
											num35++;
										}
										x = num31;
										y = num32 + num41;
										z = num33;
										int num67 = ChunkHelper.IndexOfBlockInBorderedChunk(x, y, z);
										int num68 = borderedChunkBlocks[num67];
										bool flag70 = num68 != int.MaxValue && this._clientBlockTypes[num68].CubeShaderEffect == ClientBlockType.ClientShaderEffect.Water;
										bool flag71 = flag70;
										if (flag71)
										{
											int num69 = ChunkHelper.IndexInBorderedChunkColumn(x, z);
											num29 = num69 * 3;
											bool flag72 = num27 + num41 >= (int)this._environmentTracker[num29];
											if (flag72)
											{
												ChunkHelper.GetEnvironmentId(borderedColumnEnvironmentIds[num69], num27 + num41, this._environmentTracker, num29);
											}
											ushort num70 = this._environmentTracker[num29 + 2];
											int waterTint8 = this._environments[(int)num70].WaterTint;
											bool flag73 = waterTint8 != -1;
											if (flag73)
											{
												num37 += (uint)((byte)(waterTint8 >> 16));
												num38 += (uint)((byte)(waterTint8 >> 8));
												num39 += (uint)((byte)waterTint8);
												flag44 = true;
											}
											else
											{
												num37 += 255U;
												num38 += 255U;
												num39 += 255U;
												flag45 = true;
											}
											num35++;
										}
										bool flag74 = flag45 && flag44;
										if (flag74)
										{
											clientShaderEffect = ClientBlockType.ClientShaderEffect.WaterEnvironmentTransition;
										}
										num37 = (uint)((ulong)num37 / (ulong)((long)num35));
										num38 = (uint)((ulong)num38 / (ulong)((long)num35));
										num39 = (uint)((ulong)num39 / (ulong)((long)num35));
										this._cornerEnvironmentWaterTints[num34] = (num37 << 16 | num38 << 8 | num39 | (uint)((uint)clientShaderEffect << 24));
									}
								}
								int? num71 = null;
								int num72 = (num25 << 5) + num26;
								uint biomeTintColor = chunkColumn.Tints[num72];
								float num73 = 0f;
								float num74 = 0f;
								float num75 = 0f;
								bool flag75 = clientBlockType3.RandomRotation == 4;
								if (flag75)
								{
									num71 = new int?(MathHelper.Hash(num26, num24, num25));
									int num76 = Math.Abs(num71.Value + MathHelper.HashOne);
									num73 = (float)(num76 % 4) * 1.5707964f;
								}
								else
								{
									bool flag76 = clientBlockType3.RandomRotation == 3;
									if (flag76)
									{
										num71 = new int?(MathHelper.Hash(num26, num25));
										int num76 = Math.Abs(num71.Value + MathHelper.HashFive);
										num73 = MathHelper.ToRadians((float)(num76 % 360));
									}
									else
									{
										bool flag77 = clientBlockType3.RandomRotation == 2 || clientBlockType3.RandomRotation == 1;
										if (flag77)
										{
											num71 = new int?(MathHelper.Hash(num26, num24, num25));
											int num76 = Math.Abs(num71.Value + MathHelper.HashTwo);
											num73 = MathHelper.ToRadians((float)(num76 % 360));
										}
										bool flag78 = clientBlockType3.RandomRotation == 1;
										if (flag78)
										{
											bool flag79 = num71 == null;
											if (flag79)
											{
												num71 = new int?(MathHelper.Hash(num26, num24, num25));
											}
											int num76 = Math.Abs(num71.Value + MathHelper.HashThree);
											num74 = MathHelper.ToRadians((float)(num76 % 360));
											num76 = Math.Abs(num71.Value + MathHelper.HashFour);
											num75 = MathHelper.ToRadians((float)(num76 % 360));
										}
									}
								}
								bool flag80 = num73 != 0f || num74 != 0f || num75 != 0f;
								if (flag80)
								{
									Matrix.CreateFromYawPitchRoll(num73, num74, num75, out this._tempBlockRotationMatrix);
									Matrix.Multiply(ref this._tempBlockRotationMatrix, ref clientBlockType3.RotationMatrix, out this._tempBlockRotationMatrix);
									Matrix.Multiply(ref clientBlockType3.BlockyModelTranslatedScaleMatrix, ref ChunkGeometryBuilder.NegativeHalfBlockOffsetMatrix, out this._tempBlockWorldMatrix);
									Matrix.Multiply(ref this._tempBlockWorldMatrix, ref this._tempBlockRotationMatrix, out this._tempBlockWorldMatrix);
									Matrix.Multiply(ref this._tempBlockWorldMatrix, ref ChunkGeometryBuilder.PositiveHalfBlockOffsetMatrix, out this._tempBlockWorldMatrix);
									Matrix.Invert(ref this._tempBlockWorldMatrix, out this._tempCubeBlockWorldInvertMatrix);
									Matrix.AddTranslation(ref this._tempCubeBlockWorldInvertMatrix, 0f, -16f, 0f);
								}
								else
								{
									this._tempBlockWorldMatrix = clientBlockType3.WorldMatrix;
									this._tempBlockRotationMatrix = clientBlockType3.RotationMatrix;
									this._tempCubeBlockWorldInvertMatrix = clientBlockType3.CubeBlockInvertMatrix;
								}
								Matrix.AddTranslation(ref this._tempBlockWorldMatrix, (float)num26, (float)num24, (float)num25);
								bool flag81 = clientBlockType3.IsAnimated() || flag41;
								if (flag81)
								{
									float val = Math.Max(Math.Abs(boundingBox.Min.X), boundingBox.Max.X);
									float val2 = Math.Max(Math.Abs(boundingBox.Min.Z), boundingBox.Max.Z);
									float num77 = Math.Max(val, val2);
									Vector3 vector = new Vector3((float)(chunkX * 32 + num26 - clientBlockType3.FillerX), (float)(chunkY * 32 + num24 - clientBlockType3.FillerY), (float)(chunkZ * 32 + num25 - clientBlockType3.FillerZ));
									BoundingBox boundingBox2 = new BoundingBox(new Vector3(vector.X - num77, vector.Y - boundingBox.Min.Y, vector.Z - num77), new Vector3(vector.X + num77, vector.Y + boundingBox.Max.Y, vector.Z + num77));
									ChunkGeometryData chunkGeometryData2 = new ChunkGeometryData();
									bool flag82 = clientBlockType3.RenderedBlockyModel != null;
									if (flag82)
									{
										chunkGeometryData2.VerticesCount += clientBlockType3.RenderedBlockyModel.AnimatedVertices.Length;
										chunkGeometryData2.IndicesCount += clientBlockType3.RenderedBlockyModel.AnimatedIndices.Length;
									}
									bool shouldRenderCube2 = clientBlockType3.ShouldRenderCube;
									if (shouldRenderCube2)
									{
										chunkGeometryData2.VerticesCount += 24;
										chunkGeometryData2.IndicesCount += 36;
									}
									chunkGeometryData2.Vertices = new ChunkVertex[chunkGeometryData2.VerticesCount];
									chunkGeometryData2.Indices = new uint[chunkGeometryData2.IndicesCount];
									bool flag83 = clientBlockType3.RenderedBlockyModel != null;
									if (flag83)
									{
										for (int num78 = 0; num78 < clientBlockType3.RenderedBlockyModel.AnimatedIndices.Length; num78++)
										{
											chunkGeometryData2.Indices[chunkGeometryData2.IndicesOffset + num78] = chunkGeometryData2.VerticesOffset + (uint)clientBlockType3.RenderedBlockyModel.AnimatedIndices[num78];
										}
										chunkGeometryData2.IndicesOffset += clientBlockType3.RenderedBlockyModel.AnimatedIndices.Length;
									}
									ChunkGeometryBuilder.CreateBlockGeometry(this._clientBlockTypes, this._lightLevels, clientBlockType3, num, hitboxHeight, Vector3.Zero, num26, num24, num25, ref num71, byte.MaxValue, Matrix.Identity, this._tempBlockRotationMatrix, this._tempCubeBlockWorldInvertMatrix, this._texCoordsByCorner, this._sideMaskTexCoordsByCorner, this._cornerOcclusions, this._cornerShaderEffects, biomeTintColor, borderedChunkBlocks, borderedChunkLightAmounts, borderedColumnTints, this._cornerEnvironmentWaterTints, atlasTextureWidth, atlasTextureHeight, chunkGeometryData2, chunkGeometryData2, chunkUpdateTask.AlphaTestedAnimatedLowLODIndicesCount, ref num17, true);
									float animationTimeOffset = 0f;
									bool flag84 = clientBlockType3.BlockyAnimation != null;
									if (flag84)
									{
										bool flag85 = num71 == null;
										if (flag85)
										{
											num71 = new int?(MathHelper.Hash(num26, num24, num25));
										}
										animationTimeOffset = (float)(Math.Abs(num71.Value) % clientBlockType3.BlockyAnimation.Duration);
									}
									chunkUpdateTask.AnimatedBlocks[num20] = new RenderedChunk.AnimatedBlock
									{
										Index = num2,
										IsBeingHit = flag41,
										Position = vector,
										BoundingBox = boundingBox2,
										Matrix = this._tempBlockWorldMatrix,
										Renderer = new AnimatedBlockRenderer(clientBlockType3.FinalBlockyModel, this._atlasSizes, chunkGeometryData2, null, false),
										Animation = clientBlockType3.BlockyAnimation,
										AnimationTimeOffset = animationTimeOffset
									};
									bool flag86 = clientBlockType3.Particles != null;
									if (flag86)
									{
										chunkUpdateTask.AnimatedBlocks[num20].MapParticleIndices = new int[clientBlockType3.Particles.Length];
										Vector3 vector2 = new Vector3((float)(chunkX * 32 + num26), (float)(chunkY * 32 + num24), (float)(chunkZ * 32 + num25));
										Quaternion quaternion = Quaternion.CreateFromYawPitchRoll(MathHelper.RotationToRadians(clientBlockType3.RotationYaw), MathHelper.RotationToRadians(clientBlockType3.RotationPitch), 0f);
										Quaternion quaternion2 = Quaternion.CreateFromYawPitchRoll(num73, num74, num75);
										quaternion *= quaternion2;
										Vector3 value = Vector3.Transform(new Vector3(0f, -0.5f, 0f), quaternion);
										vector2 += new Vector3(0.5f, 0.5f, 0.5f) + value;
										for (int num79 = 0; num79 < clientBlockType3.Particles.Length; num79++)
										{
											ref ModelParticleSettings ptr = ref clientBlockType3.Particles[num79];
											chunkUpdateTask.MapParticles[num21] = new RenderedChunk.MapParticle
											{
												BlockIndex = num2,
												Position = vector2,
												Rotation = quaternion,
												PositionOffset = ptr.PositionOffset,
												RotationOffset = ptr.RotationOffset,
												TargetNodeIndex = ptr.TargetNodeIndex,
												ParticleSystemId = ptr.SystemId,
												Color = ptr.Color,
												BlockScale = clientBlockType3.BlockyModelScale,
												Scale = clientBlockType3.BlockyModelScale * ptr.Scale
											};
											chunkUpdateTask.AnimatedBlocks[num20].MapParticleIndices[num79] = num21;
											num21++;
										}
									}
									num20++;
								}
								bool flag87 = clientBlockType3.RenderedBlockyModel != null && (!clientBlockType3.ShouldRenderCube || clientBlockType3.RequiresAlphaBlending || b3 > 0);
								if (flag87)
								{
									bool flag88 = clientBlockType3.IsAnimated() || flag41;
									if (flag88)
									{
										int num80 = this._LODEnabled ? ((int)clientBlockType3.RenderedBlockyModel.LowLODIndicesCount) : clientBlockType3.RenderedBlockyModel.StaticIndices.Length;
										int indicesOffset = chunkUpdateTask.AlphaTestedData.IndicesOffset;
										for (int num81 = 0; num81 < num80; num81++)
										{
											chunkUpdateTask.AlphaTestedData.Indices[indicesOffset + num81] = chunkUpdateTask.AlphaTestedData.VerticesOffset + (uint)clientBlockType3.RenderedBlockyModel.StaticIndices[num81];
										}
										chunkUpdateTask.AlphaTestedData.IndicesOffset += num80;
										int num82 = chunkUpdateTask.AlphaTestedAnimatedLowLODIndicesCount + chunkUpdateTask.AlphaTestedLowLODIndicesCount + chunkUpdateTask.AlphaTestedHighLODIndicesCount + num19;
										for (int num83 = 0; num83 < clientBlockType3.RenderedBlockyModel.StaticIndices.Length - num80; num83++)
										{
											chunkUpdateTask.AlphaTestedData.Indices[num82 + num83] = chunkUpdateTask.AlphaTestedData.VerticesOffset + (uint)clientBlockType3.RenderedBlockyModel.StaticIndices[num80 + num83];
										}
										num19 += clientBlockType3.RenderedBlockyModel.StaticIndices.Length - num80;
									}
									else
									{
										int num84 = this._LODEnabled ? ((int)clientBlockType3.RenderedBlockyModel.LowLODIndicesCount) : clientBlockType3.RenderedBlockyModel.StaticIndices.Length;
										int num85 = chunkUpdateTask.AlphaTestedAnimatedLowLODIndicesCount + num17;
										for (int num86 = 0; num86 < num84; num86++)
										{
											chunkUpdateTask.AlphaTestedData.Indices[num85 + num86] = chunkUpdateTask.AlphaTestedData.VerticesOffset + (uint)clientBlockType3.RenderedBlockyModel.StaticIndices[num86];
										}
										num17 += num84;
										int num87 = chunkUpdateTask.AlphaTestedAnimatedLowLODIndicesCount + chunkUpdateTask.AlphaTestedLowLODIndicesCount + num18;
										for (int num88 = 0; num88 < clientBlockType3.RenderedBlockyModel.StaticIndices.Length - num84; num88++)
										{
											chunkUpdateTask.AlphaTestedData.Indices[num87 + num88] = chunkUpdateTask.AlphaTestedData.VerticesOffset + (uint)clientBlockType3.RenderedBlockyModel.StaticIndices[num84 + num88];
										}
										num18 += clientBlockType3.RenderedBlockyModel.StaticIndices.Length - num84;
									}
								}
								bool flag89 = ChunkGeometryBuilder.ShouldRegisterSound(clientBlockType3);
								if (flag89)
								{
									Vector3 position = new Vector3((float)(chunkX * 32 + num26), (float)(chunkY * 32 + num24), (float)(chunkZ * 32 + num25));
									chunkUpdateTask.SoundObjects[num22] = new RenderedChunk.MapSoundObject
									{
										BlockIndex = num2,
										SoundEventReference = AudioDevice.SoundEventReference.None,
										SoundEventIndex = clientBlockType3.SoundEventIndex,
										Position = position
									};
									num22++;
								}
								bool flag90 = clientBlockType3.Particles != null && !flag41 && ((clientBlockType3.RenderedBlockyModel != null && !clientBlockType3.IsAnimated()) || (clientBlockType3.ShouldRenderCube && (clientBlockType3.RequiresAlphaBlending || b3 > 0)));
								if (flag90)
								{
									Vector3 value2 = new Vector3((float)(chunkX * 32 + num26), (float)(chunkY * 32 + num24), (float)(chunkZ * 32 + num25));
									Quaternion quaternion3 = Quaternion.CreateFromYawPitchRoll(MathHelper.RotationToRadians(clientBlockType3.RotationYaw), MathHelper.RotationToRadians(clientBlockType3.RotationPitch), 0f);
									Quaternion quaternion4 = Quaternion.CreateFromYawPitchRoll(num73, num74, num75);
									quaternion3 *= quaternion4;
									Vector3 value3 = Vector3.Transform(new Vector3(0f, -0.5f, 0f), quaternion3);
									value2 += new Vector3(0.5f, 0.5f, 0.5f) + value3;
									for (int num89 = 0; num89 < clientBlockType3.Particles.Length; num89++)
									{
										ref ModelParticleSettings ptr2 = ref clientBlockType3.Particles[num89];
										Vector3 value4 = Vector3.Zero;
										Quaternion quaternion5 = Quaternion.Identity;
										bool flag91 = clientBlockType3.RenderedBlockyModel != null;
										if (flag91)
										{
											value4 = Vector3.Transform(clientBlockType3.RenderedBlockyModel.NodeParentTransforms[ptr2.TargetNodeIndex].Position * 0.03125f * clientBlockType3.BlockyModelScale, quaternion3) + Vector3.Transform(ptr2.PositionOffset * clientBlockType3.BlockyModelScale, quaternion3 * clientBlockType3.RenderedBlockyModel.NodeParentTransforms[ptr2.TargetNodeIndex].Orientation);
											quaternion5 = clientBlockType3.RenderedBlockyModel.NodeParentTransforms[ptr2.TargetNodeIndex].Orientation * ptr2.RotationOffset;
										}
										else
										{
											value4 = Vector3.Transform(Vector3.Up * 0.5f * clientBlockType3.BlockyModelScale, quaternion3) + Vector3.Transform(ptr2.PositionOffset * clientBlockType3.BlockyModelScale, quaternion3);
											quaternion5 = ptr2.RotationOffset;
										}
										chunkUpdateTask.MapParticles[num21] = new RenderedChunk.MapParticle
										{
											BlockIndex = num2,
											Position = value2 + value4,
											RotationOffset = quaternion3 * quaternion5,
											TargetNodeIndex = ptr2.TargetNodeIndex,
											ParticleSystemId = ptr2.SystemId,
											Color = ptr2.Color,
											Scale = clientBlockType3.BlockyModelScale * ptr2.Scale
										};
										num21++;
									}
								}
								ChunkGeometryData cubeVertexData = flag41 ? chunkUpdateTask.AlphaTestedData : ((!clientBlockType3.RequiresAlphaBlending) ? chunkUpdateTask.OpaqueData : chunkUpdateTask.AlphaBlendedData);
								ushort num90 = borderedChunkLightAmounts[num];
								int val3 = num90 >> 12 & 15;
								num23 = Math.Max(val3, num23);
								ChunkGeometryBuilder.CreateBlockGeometry(this._clientBlockTypes, this._lightLevels, clientBlockType3, num, hitboxHeight, new Vector3((float)num26, (float)num24, (float)num25), num26, num24, num25, ref num71, b3, this._tempBlockWorldMatrix, this._tempBlockRotationMatrix, Matrix.Identity, this._texCoordsByCorner, this._sideMaskTexCoordsByCorner, this._cornerOcclusions, this._cornerShaderEffects, biomeTintColor, borderedChunkBlocks, borderedChunkLightAmounts, borderedColumnTints, this._cornerEnvironmentWaterTints, atlasTextureWidth, atlasTextureHeight, cubeVertexData, chunkUpdateTask.AlphaTestedData, chunkUpdateTask.AlphaTestedAnimatedLowLODIndicesCount, ref num17, false);
								num++;
								num2++;
							}
						}
						num += 2;
					}
					num += 68;
				}
				chunkUpdateTask.IsUnderground = (num23 <= 2);
				bool flag92 = (ulong)chunkUpdateTask.OpaqueData.VerticesOffset != (ulong)((long)chunkUpdateTask.OpaqueData.VerticesCount);
				if (flag92)
				{
					throw new Exception("Invalid opaque vertex count");
				}
				bool flag93 = chunkUpdateTask.OpaqueData.IndicesOffset != chunkUpdateTask.OpaqueData.IndicesCount;
				if (flag93)
				{
					throw new Exception("Invalid opaque index count");
				}
				bool flag94 = (ulong)chunkUpdateTask.AlphaBlendedData.VerticesOffset != (ulong)((long)chunkUpdateTask.AlphaBlendedData.VerticesCount);
				if (flag94)
				{
					throw new Exception("Invalid alpha-blended vertex count");
				}
				bool flag95 = chunkUpdateTask.AlphaBlendedData.IndicesOffset != chunkUpdateTask.AlphaBlendedData.IndicesCount;
				if (flag95)
				{
					throw new Exception("Invalid alpha-blended index count");
				}
				bool flag96 = (ulong)chunkUpdateTask.AlphaTestedData.VerticesOffset != (ulong)((long)chunkUpdateTask.AlphaTestedData.VerticesCount);
				if (flag96)
				{
					throw new Exception("Invalid alpha-tested vertex count");
				}
				bool flag97 = chunkUpdateTask.AlphaTestedData.IndicesOffset != chunkUpdateTask.AlphaTestedAnimatedLowLODIndicesCount;
				if (flag97)
				{
					throw new Exception("Invalid alpha-tested low LOD animated indices count");
				}
				bool flag98 = num17 != chunkUpdateTask.AlphaTestedLowLODIndicesCount;
				if (flag98)
				{
					throw new Exception("Invalid alpha-tested low LOD indices count");
				}
				bool flag99 = num18 != chunkUpdateTask.AlphaTestedHighLODIndicesCount;
				if (flag99)
				{
					throw new Exception("Invalid alpha-tested high LOD indices count");
				}
				bool flag100 = num19 != chunkUpdateTask.AlphaTestedAnimatedHighLODIndicesCount;
				if (flag100)
				{
					throw new Exception("Invalid alpha-tested high LOD animated indices count");
				}
				result = chunkUpdateTask;
			}
			return result;
		}

		// Token: 0x06005530 RID: 21808 RVA: 0x0018AE1C File Offset: 0x0018901C
		public static void CreateBlockGeometry(ClientBlockType[] clientBlockTypes, float[] lightLevels, ClientBlockType blockType, int borderedBlockIndex, float hitboxHeight, Vector3 cubeOffset, int chunkX, int chunkY, int chunkZ, ref int? seed, byte visibleSideFlags, Matrix blockyModelMatrix, Matrix blockModelRotationMatrix, Matrix cubeMatrix, UShortVector2[] texCoordsByCorner, UShortVector2[] sideMaskTexCoordsByCorner, int[] cornerOcclusions, ClientBlockType.ClientShaderEffect[] cornerShaderEffects, uint biomeTintColor, int[] borderedChunkBlocks, ushort[] borderedLightAmounts, uint[] borderedColumnTints, uint[] environmentTints, int atlasTextureWidth, int atlasTextureHeight, ChunkGeometryData cubeVertexData, ChunkGeometryData alphaTestedVertexData, int alphaTestedAnimatedLowLODIndicesStart, ref int alphaTestedLowLODIndicesOffset, bool isAnimated)
		{
			byte b = (byte)(biomeTintColor >> 16);
			byte b2 = (byte)(biomeTintColor >> 8);
			byte b3 = (byte)biomeTintColor;
			bool flag = !ChunkGeometryBuilder.NoTint.Equals(ChunkGeometryBuilder.ForceTint);
			if (flag)
			{
				b = (byte)ChunkGeometryBuilder.ForceTint.X;
				b2 = (byte)ChunkGeometryBuilder.ForceTint.Y;
				b3 = (byte)ChunkGeometryBuilder.ForceTint.Z;
			}
			ShortVector3 positionPacked = default(ShortVector3);
			bool flag2 = blockType.RenderedBlockyModel != null && (!blockType.ShouldRenderCube || blockType.RequiresAlphaBlending || visibleSideFlags > 0);
			if (flag2)
			{
				int num = blockType.SelfTintColorsBySide[0];
				byte b4 = (byte)(num >> 16);
				byte b5 = (byte)(num >> 8);
				byte b6 = (byte)num;
				float num2 = blockType.BiomeTintMultipliersBySide[0];
				uint num3 = (uint)((float)b4 + (float)(b - b4) * num2);
				uint num4 = (uint)((float)b5 + (float)(b2 - b5) * num2);
				uint num5 = (uint)((float)b6 + (float)(b3 - b6) * num2);
				uint num6 = num3 | num4 << 8 | num5 << 16;
				uint useBillboard = blockType.RenderedBlockyModel.UsesBillboardLOD ? 1U : 0U;
				uint num7 = (blockType.RenderedBlockyModel.HasOnlyQuads || blockType.CollisionMaterial == 1) ? 32U : 0U;
				ushort num8 = borderedLightAmounts[borderedBlockIndex];
				uint num9 = 0U;
				for (int i = 0; i < 4; i++)
				{
					int num10 = num8 >> i * 4 & 15;
					float num11 = lightLevels[num10];
					num9 |= (uint)(num11 * 255f) << i * 8;
				}
				int num12 = 0;
				bool flag3 = blockType.BlockyTextureWeights.Length > 1;
				if (flag3)
				{
					bool flag4 = seed == null;
					if (flag4)
					{
						seed = new int?(MathHelper.Hash(chunkX, chunkY, chunkZ));
					}
					num12 = ChunkGeometryBuilder.CalculateBlockTextureIndex(blockType.BlockyTextureWeights, seed.Value);
				}
				Vector2 vector = blockType.RenderedBlockyModelTextureOrigins[num12];
				ushort num13 = VertexCompression.CompressBlockLocalPosition(chunkX, chunkY, chunkZ);
				StaticBlockyModelVertex[] array = isAnimated ? blockType.RenderedBlockyModel.AnimatedVertices : blockType.RenderedBlockyModel.StaticVertices;
				for (int j = 0; j < array.Length / 4; j++)
				{
					int num14 = j * 4;
					long num15 = (long)((ulong)alphaTestedVertexData.VerticesOffset + (ulong)((long)num14));
					Vector3 position;
					Vector3.Transform(ref array[num14].Position, ref blockyModelMatrix, out position);
					Vector3 position2;
					Vector3.Transform(ref array[num14 + 1].Position, ref blockyModelMatrix, out position2);
					Vector3 position3;
					Vector3.Transform(ref array[num14 + 2].Position, ref blockyModelMatrix, out position3);
					Vector3 position4;
					Vector3.Transform(ref array[num14 + 3].Position, ref blockyModelMatrix, out position4);
					uint normalAndNodeIndex;
					checked
					{
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].PositionPacked = VertexCompression.Vector3PositionToShortVector3(position, 64f);
						alphaTestedVertexData.Vertices[(int)((IntPtr)(unchecked(num15 + 1L)))].PositionPacked = VertexCompression.Vector3PositionToShortVector3(position2, 64f);
						alphaTestedVertexData.Vertices[(int)((IntPtr)(unchecked(num15 + 2L)))].PositionPacked = VertexCompression.Vector3PositionToShortVector3(position3, 64f);
						alphaTestedVertexData.Vertices[(int)((IntPtr)(unchecked(num15 + 3L)))].PositionPacked = VertexCompression.Vector3PositionToShortVector3(position4, 64f);
						ushort doubleSidedAndBlockId = unchecked((ushort)((1U & array[num14].DoubleSided) << 15 | (uint)num13));
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].DoubleSidedAndBlockId = doubleSidedAndBlockId;
						alphaTestedVertexData.Vertices[(int)((IntPtr)(unchecked(num15 + 1L)))].DoubleSidedAndBlockId = doubleSidedAndBlockId;
						alphaTestedVertexData.Vertices[(int)((IntPtr)(unchecked(num15 + 2L)))].DoubleSidedAndBlockId = doubleSidedAndBlockId;
						alphaTestedVertexData.Vertices[(int)((IntPtr)(unchecked(num15 + 3L)))].DoubleSidedAndBlockId = doubleSidedAndBlockId;
						Vector3 vector2;
						if (isAnimated)
						{
							vector2 = array[num14].Normal;
						}
						else
						{
							vector2 = Vector3.TransformNormal(array[num14].Normal, blockModelRotationMatrix);
						}
						normalAndNodeIndex = (VertexCompression.NormalizedXYZToUint(vector2.X, vector2.Y, vector2.Z) | (uint)((uint)array[num14].NodeIndex << 24));
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].NormalAndNodeIndex = normalAndNodeIndex;
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].TextureCoordinates.X = VertexCompression.NormalizedTexCoordToUshort(unchecked(array[num14].TextureCoordinates.X + vector.X));
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].TextureCoordinates.Y = VertexCompression.NormalizedTexCoordToUshort(unchecked(array[num14].TextureCoordinates.Y + vector.Y));
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].MaskTextureCoordinates = ChunkGeometryBuilder.NoSideMaskUV;
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].GlowColorAndSunlight = num9;
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].UseBillboard = useBillboard;
						uint num16 = (blockType.BlockyModelShaderEffect == ClientBlockType.ClientShaderEffect.WindAttached) ? unchecked((uint)(MathHelper.Clamp(array[num14].Position.Y / hitboxHeight, 0f, 1f) * 14f)) : ((uint)blockType.BlockyModelShaderEffect);
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].TintColorAndEffectAndShadingMode = (num6 | (num7 | num16) << 24 | (uint)((uint)array[num14].ShadingMode << 30));
					}
					num14++;
					num15 += 1L;
					checked
					{
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].NormalAndNodeIndex = normalAndNodeIndex;
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].TextureCoordinates.X = VertexCompression.NormalizedTexCoordToUshort(unchecked(array[num14].TextureCoordinates.X + vector.X));
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].TextureCoordinates.Y = VertexCompression.NormalizedTexCoordToUshort(unchecked(array[num14].TextureCoordinates.Y + vector.Y));
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].MaskTextureCoordinates = ChunkGeometryBuilder.NoSideMaskUV;
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].GlowColorAndSunlight = num9;
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].UseBillboard = useBillboard;
						uint num16 = (blockType.BlockyModelShaderEffect == ClientBlockType.ClientShaderEffect.WindAttached) ? unchecked((uint)(MathHelper.Clamp(array[num14].Position.Y / hitboxHeight, 0f, 1f) * 14f)) : ((uint)blockType.BlockyModelShaderEffect);
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].TintColorAndEffectAndShadingMode = (num6 | (num7 | num16) << 24 | (uint)((uint)array[num14].ShadingMode << 30));
					}
					num14++;
					num15 += 1L;
					checked
					{
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].NormalAndNodeIndex = normalAndNodeIndex;
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].TextureCoordinates.X = VertexCompression.NormalizedTexCoordToUshort(unchecked(array[num14].TextureCoordinates.X + vector.X));
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].TextureCoordinates.Y = VertexCompression.NormalizedTexCoordToUshort(unchecked(array[num14].TextureCoordinates.Y + vector.Y));
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].MaskTextureCoordinates = ChunkGeometryBuilder.NoSideMaskUV;
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].GlowColorAndSunlight = num9;
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].UseBillboard = useBillboard;
						uint num16 = (blockType.BlockyModelShaderEffect == ClientBlockType.ClientShaderEffect.WindAttached) ? unchecked((uint)(MathHelper.Clamp(array[num14].Position.Y / hitboxHeight, 0f, 1f) * 14f)) : ((uint)blockType.BlockyModelShaderEffect);
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].TintColorAndEffectAndShadingMode = (num6 | (num7 | num16) << 24 | (uint)((uint)array[num14].ShadingMode << 30));
					}
					num14++;
					num15 += 1L;
					checked
					{
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].NormalAndNodeIndex = normalAndNodeIndex;
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].TextureCoordinates.X = VertexCompression.NormalizedTexCoordToUshort(unchecked(array[num14].TextureCoordinates.X + vector.X));
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].TextureCoordinates.Y = VertexCompression.NormalizedTexCoordToUshort(unchecked(array[num14].TextureCoordinates.Y + vector.Y));
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].MaskTextureCoordinates = ChunkGeometryBuilder.NoSideMaskUV;
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].GlowColorAndSunlight = num9;
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].UseBillboard = useBillboard;
						uint num16 = (blockType.BlockyModelShaderEffect == ClientBlockType.ClientShaderEffect.WindAttached) ? unchecked((uint)(MathHelper.Clamp(array[num14].Position.Y / hitboxHeight, 0f, 1f) * 14f)) : ((uint)blockType.BlockyModelShaderEffect);
						alphaTestedVertexData.Vertices[(int)((IntPtr)num15)].TintColorAndEffectAndShadingMode = (num6 | (num7 | num16) << 24 | (uint)((uint)array[num14].ShadingMode << 30));
					}
				}
				alphaTestedVertexData.VerticesOffset += (uint)array.Length;
			}
			bool flag5 = blockType.ShouldRenderCube && visibleSideFlags > 0;
			if (flag5)
			{
				ushort num17 = 0;
				ushort num18 = 0;
				ushort num19 = 0;
				ushort num20 = 0;
				uint num21 = 0U;
				bool flag6 = false;
				uint num22 = blockType.RequiresAlphaBlending ? 1U : 0U;
				bool flag7 = (visibleSideFlags & 1) > 0;
				byte b7 = (blockType.MaxFillLevel == 0) ? 8 : blockType.MaxFillLevel;
				float num23 = (float)blockType.VerticalFill / (float)b7;
				int num24 = 0;
				bool flag8 = blockType.CubeTextureWeights.Length > 1;
				if (flag8)
				{
					bool flag9 = seed == null;
					if (flag9)
					{
						seed = new int?(MathHelper.Hash(chunkX, chunkY, chunkZ));
					}
					num24 = ChunkGeometryBuilder.CalculateBlockTextureIndex(blockType.CubeTextureWeights, seed.Value);
				}
				for (int k = 0; k < 6; k++)
				{
					ChunkGeometryBuilder.AdjacentBlockOffsets adjacentBlockOffsets = ChunkGeometryBuilder.AdjacentBlockOffsetsBySide[k];
					int num25 = borderedBlockIndex + adjacentBlockOffsets.Main;
					bool flag10 = k == 0 && blockType.VerticalFill < b7;
					if (flag10)
					{
						num25 = borderedBlockIndex;
					}
					byte b8 = (byte)(1 << k);
					bool flag11 = (visibleSideFlags & b8) > 0;
					if (flag11)
					{
						ClientBlockType.CubeTexture cubeTexture = blockType.CubeTextures[k];
						int num26 = atlasTextureWidth / 32;
						int num27 = cubeTexture.TileLinearPositionsInAtlas[num24];
						float num28 = (float)(num27 % num26 * 32);
						float num29 = (float)(num27 / num26 * 32);
						float u = (num28 + 0.04f) / (float)atlasTextureWidth;
						float u2 = (num28 + 32f - 0.04f) / (float)atlasTextureWidth;
						float u3 = (num29 + ((k >= 2) ? (32f * (1f - num23)) : 0f) + 0.04f) / (float)atlasTextureHeight;
						float u4 = (num29 + 32f - 0.04f) / (float)atlasTextureHeight;
						ushort x = VertexCompression.NormalizedTexCoordToUshort(u);
						ushort x2 = VertexCompression.NormalizedTexCoordToUshort(u2);
						ushort y = VertexCompression.NormalizedTexCoordToUshort(u3);
						ushort y2 = VertexCompression.NormalizedTexCoordToUshort(u4);
						int rotation = cubeTexture.Rotation;
						int num30 = rotation;
						if (num30 <= 90)
						{
							if (num30 != 0)
							{
								if (num30 != 90)
								{
									goto IL_D1B;
								}
								texCoordsByCorner[0].X = x;
								texCoordsByCorner[1].X = x;
								texCoordsByCorner[2].X = x2;
								texCoordsByCorner[3].X = x2;
								texCoordsByCorner[0].Y = y;
								texCoordsByCorner[1].Y = y2;
								texCoordsByCorner[2].Y = y2;
								texCoordsByCorner[3].Y = y;
							}
							else
							{
								texCoordsByCorner[0].X = x2;
								texCoordsByCorner[1].X = x;
								texCoordsByCorner[2].X = x;
								texCoordsByCorner[3].X = x2;
								texCoordsByCorner[0].Y = y;
								texCoordsByCorner[1].Y = y;
								texCoordsByCorner[2].Y = y2;
								texCoordsByCorner[3].Y = y2;
							}
						}
						else if (num30 != 180)
						{
							if (num30 != 270)
							{
								goto IL_D1B;
							}
							texCoordsByCorner[0].X = x2;
							texCoordsByCorner[1].X = x2;
							texCoordsByCorner[2].X = x;
							texCoordsByCorner[3].X = x;
							texCoordsByCorner[0].Y = y2;
							texCoordsByCorner[1].Y = y;
							texCoordsByCorner[2].Y = y;
							texCoordsByCorner[3].Y = y2;
						}
						else
						{
							texCoordsByCorner[0].X = x;
							texCoordsByCorner[1].X = x2;
							texCoordsByCorner[2].X = x2;
							texCoordsByCorner[3].X = x;
							texCoordsByCorner[0].Y = y2;
							texCoordsByCorner[1].Y = y2;
							texCoordsByCorner[2].Y = y;
							texCoordsByCorner[3].Y = y;
						}
						bool flag12 = k >= 2 && blockType.CubeSideMaskTextureAtlasIndex != -1;
						bool flag13 = flag12;
						if (flag13)
						{
							int num31 = blockType.CubeSideMaskTextureAtlasIndex * 32;
							float num32 = (float)(num31 % atlasTextureWidth);
							float num33 = (float)(num31 / atlasTextureWidth * 32);
							float u5 = (num32 + 0.04f) / (float)atlasTextureWidth;
							float u6 = (num32 + 32f - 0.04f) / (float)atlasTextureWidth;
							float u7 = (num33 + 0.04f) / (float)atlasTextureHeight;
							float u8 = (num33 + 32f - 0.04f) / (float)atlasTextureHeight;
							ushort x3 = VertexCompression.NormalizedTexCoordToUshort(u5);
							ushort x4 = VertexCompression.NormalizedTexCoordToUshort(u6);
							ushort y3 = VertexCompression.NormalizedTexCoordToUshort(u7);
							ushort y4 = VertexCompression.NormalizedTexCoordToUshort(u8);
							sideMaskTexCoordsByCorner[0].X = x4;
							sideMaskTexCoordsByCorner[0].Y = y3;
							sideMaskTexCoordsByCorner[1].X = x3;
							sideMaskTexCoordsByCorner[1].Y = y3;
							sideMaskTexCoordsByCorner[2].X = x3;
							sideMaskTexCoordsByCorner[2].Y = y4;
							sideMaskTexCoordsByCorner[3].X = x4;
							sideMaskTexCoordsByCorner[3].Y = y4;
						}
						else
						{
							sideMaskTexCoordsByCorner[0] = ChunkGeometryBuilder.NoSideMaskUV;
							sideMaskTexCoordsByCorner[1] = ChunkGeometryBuilder.NoSideMaskUV;
							sideMaskTexCoordsByCorner[2] = ChunkGeometryBuilder.NoSideMaskUV;
							sideMaskTexCoordsByCorner[3] = ChunkGeometryBuilder.NoSideMaskUV;
						}
						int num34 = flag12 ? 0 : k;
						float num35 = blockType.BiomeTintMultipliersBySide[num34];
						for (int l = 0; l < 4; l++)
						{
							uint num36 = 0U;
							uint num37 = 255U;
							uint num38 = 255U;
							uint num39 = 255U;
							int num40 = borderedChunkBlocks[num25];
							bool flag14 = num40 != int.MaxValue && clientBlockTypes[num40].IsOccluder;
							bool flag15 = !flag14;
							if (flag15)
							{
								num20 = borderedLightAmounts[num25];
							}
							int num41 = num25 + adjacentBlockOffsets.Horizontal * ChunkGeometryBuilder.AdjacentBlockSignsByCorner[l][0];
							int num42 = borderedChunkBlocks[num41];
							bool flag16 = num42 != int.MaxValue && clientBlockTypes[num42].IsOccluder;
							bool flag17 = !flag16;
							if (flag17)
							{
								num17 = borderedLightAmounts[num41];
							}
							int num43 = num25 + adjacentBlockOffsets.Vertical * ChunkGeometryBuilder.AdjacentBlockSignsByCorner[l][1];
							int num44 = borderedChunkBlocks[num43];
							bool flag18 = num44 != int.MaxValue && clientBlockTypes[num44].IsOccluder;
							bool flag19 = !flag18;
							if (flag19)
							{
								num18 = borderedLightAmounts[num43];
							}
							int num45 = num41 + adjacentBlockOffsets.Vertical * ChunkGeometryBuilder.AdjacentBlockSignsByCorner[l][1];
							int num46 = borderedChunkBlocks[num45];
							bool flag20 = num46 != int.MaxValue && clientBlockTypes[num46].IsOccluder;
							bool flag21 = !flag20;
							if (flag21)
							{
								num19 = borderedLightAmounts[num45];
							}
							for (int m = 0; m < 4; m++)
							{
								float num47 = 0f;
								int num48 = 0;
								bool flag22 = !flag14;
								if (flag22)
								{
									int num49 = num20 >> m * 4 & 15;
									num47 += lightLevels[num49];
									num48++;
								}
								bool flag23 = !flag16;
								if (flag23)
								{
									int num50 = num17 >> m * 4 & 15;
									num47 += lightLevels[num50];
									num48++;
								}
								bool flag24 = !flag18;
								if (flag24)
								{
									int num51 = num18 >> m * 4 & 15;
									num47 += lightLevels[num51];
									num48++;
								}
								bool flag25 = !flag20;
								if (flag25)
								{
									int num52 = num19 >> m * 4 & 15;
									num47 += lightLevels[num52];
									num48++;
								}
								bool flag26 = num48 != 0;
								if (flag26)
								{
									num47 /= (float)num48;
								}
								bool flag27 = (flag16 && flag18) || (flag16 && flag14) || (flag18 && flag14);
								if (flag27)
								{
									num47 *= 0.7f;
									cornerOcclusions[l] = 2;
								}
								else
								{
									bool flag28 = flag16 || flag18 || flag20 || flag14;
									if (flag28)
									{
										num47 *= 0.85f;
										cornerOcclusions[l] = 1;
									}
									else
									{
										cornerOcclusions[l] = 0;
									}
								}
								num36 |= (uint)(num47 * 255f) << m * 8;
							}
							uint num53 = 0U;
							Vector3 vector3 = ChunkGeometryBuilder.CornersPerSide[k][l];
							ClientBlockType.ClientShaderEffect clientShaderEffect = blockType.CubeShaderEffect;
							bool flag29 = (clientShaderEffect == ClientBlockType.ClientShaderEffect.Water || clientShaderEffect == ClientBlockType.ClientShaderEffect.Lava) && vector3.Y > 0f && (float)blockType.VerticalFill > (float)blockType.MaxFillLevel * 0.125f;
							if (flag29)
							{
								num53 = 32U;
								bool flag30 = blockType.VerticalFill == blockType.MaxFillLevel;
								if (flag30)
								{
									int z = (int)Math.Floor((double)(vector3.Z + 0.5f));
									int z2 = (int)Math.Floor((double)(vector3.Z - 0.5f));
									int x5 = (int)Math.Floor((double)(vector3.X + 0.5f));
									int x6 = (int)Math.Floor((double)(vector3.X - 0.5f));
									int num54 = borderedBlockIndex + ChunkHelper.IndexOfBlockInBorderedChunk(x5, 1, z);
									int num55 = borderedChunkBlocks[num54];
									int num56 = borderedBlockIndex + ChunkHelper.IndexOfBlockInBorderedChunk(x6, 1, z);
									int num57 = borderedChunkBlocks[num56];
									int num58 = borderedBlockIndex + ChunkHelper.IndexOfBlockInBorderedChunk(x5, 1, z2);
									int num59 = borderedChunkBlocks[num58];
									int num60 = borderedBlockIndex + ChunkHelper.IndexOfBlockInBorderedChunk(x6, 1, z2);
									int num61 = borderedChunkBlocks[num60];
									bool flag31 = (num55 != int.MaxValue && clientBlockTypes[num55].ShouldRenderCube) || (num57 != int.MaxValue && clientBlockTypes[num57].ShouldRenderCube) || (num59 != int.MaxValue && clientBlockTypes[num59].ShouldRenderCube) || (num61 != int.MaxValue && clientBlockTypes[num61].ShouldRenderCube);
									if (flag31)
									{
										num53 = 0U;
									}
								}
							}
							Vector3 position5 = new Vector3(cubeOffset.X + vector3.X, cubeOffset.Y + vector3.Y * num23, cubeOffset.Z + vector3.Z);
							Vector3.Transform(ref position5, ref cubeMatrix, out position5);
							positionPacked = VertexCompression.Vector3PositionToShortVector3(position5, 64f);
							Vector3 vector4 = Vector3.Normalize(Vector3.TransformNormal(adjacentBlockOffsets.Normal, cubeMatrix));
							uint normalAndNodeIndex2 = VertexCompression.NormalizedXYZToUint(vector4.X, vector4.Y, vector4.Z) | (uint)((uint)(blockType.FinalBlockyModel.NodeCount - 1) << 24);
							bool flag32 = blockType.RenderedBlockyModel == null;
							if (flag32)
							{
								int num62 = blockType.SelfTintColorsBySide[num34];
								byte b9 = (byte)(num62 >> 16);
								byte b10 = (byte)(num62 >> 8);
								byte b11 = (byte)num62;
								int num63 = 0;
								int num64 = 0;
								switch (k)
								{
								case 0:
									switch (l)
									{
									case 0:
										num63 = 1;
										num64 = 0;
										break;
									case 1:
										num63 = 0;
										num64 = 0;
										break;
									case 2:
										num63 = 0;
										num64 = 1;
										break;
									case 3:
										num63 = 1;
										num64 = 1;
										break;
									}
									break;
								case 1:
									switch (l)
									{
									case 0:
										num63 = 0;
										num64 = 0;
										break;
									case 1:
										num63 = 1;
										num64 = 0;
										break;
									case 2:
										num63 = 1;
										num64 = 1;
										break;
									case 3:
										num63 = 0;
										num64 = 1;
										break;
									}
									break;
								case 2:
									switch (l)
									{
									case 0:
									case 3:
										num63 = 0;
										num64 = 1;
										break;
									case 1:
									case 2:
										num63 = 0;
										num64 = 0;
										break;
									}
									break;
								case 3:
									switch (l)
									{
									case 0:
									case 3:
										num63 = 1;
										num64 = 0;
										break;
									case 1:
									case 2:
										num63 = 1;
										num64 = 1;
										break;
									}
									break;
								case 4:
									switch (l)
									{
									case 0:
									case 3:
										num63 = 1;
										num64 = 1;
										break;
									case 1:
									case 2:
										num63 = 0;
										num64 = 1;
										break;
									}
									break;
								case 5:
									switch (l)
									{
									case 0:
									case 3:
										num63 = 0;
										num64 = 0;
										break;
									case 1:
									case 2:
										num63 = 1;
										num64 = 0;
										break;
									}
									break;
								}
								uint num65 = borderedColumnTints[ChunkHelper.IndexInBorderedChunkColumn(chunkX + num63, chunkZ + num64)];
								byte b12 = (byte)(num65 >> 16);
								byte b13 = (byte)(num65 >> 8);
								byte b14 = (byte)num65;
								bool flag33 = !ChunkGeometryBuilder.NoTint.Equals(ChunkGeometryBuilder.ForceTint);
								if (flag33)
								{
									b12 = (byte)ChunkGeometryBuilder.ForceTint.X;
									b13 = (byte)ChunkGeometryBuilder.ForceTint.Y;
									b14 = (byte)ChunkGeometryBuilder.ForceTint.Z;
								}
								num37 = (uint)((float)b9 + (float)(b12 - b9) * num35);
								num38 = (uint)((float)b10 + (float)(b13 - b10) * num35);
								num39 = (uint)((float)b11 + (float)(b14 - b11) * num35);
							}
							bool flag34 = clientShaderEffect == ClientBlockType.ClientShaderEffect.Water;
							if (flag34)
							{
								int num66 = l;
								switch (k)
								{
								case 1:
									switch (l)
									{
									case 0:
										num66 = 5;
										break;
									case 1:
										num66 = 4;
										break;
									case 2:
										num66 = 7;
										break;
									case 3:
										num66 = 6;
										break;
									}
									break;
								case 2:
									switch (l)
									{
									case 0:
										num66 = 2;
										break;
									case 1:
										num66 = 1;
										break;
									case 2:
										num66 = 5;
										break;
									case 3:
										num66 = 6;
										break;
									}
									break;
								case 3:
									switch (l)
									{
									case 0:
										num66 = 0;
										break;
									case 1:
										num66 = 3;
										break;
									case 2:
										num66 = 7;
										break;
									case 3:
										num66 = 4;
										break;
									}
									break;
								case 4:
									switch (l)
									{
									case 0:
										num66 = 3;
										break;
									case 1:
										num66 = 2;
										break;
									case 2:
										num66 = 6;
										break;
									case 3:
										num66 = 7;
										break;
									}
									break;
								case 5:
									switch (l)
									{
									case 0:
										num66 = 1;
										break;
									case 1:
										num66 = 0;
										break;
									case 2:
										num66 = 4;
										break;
									case 3:
										num66 = 5;
										break;
									}
									break;
								}
								uint num67 = environmentTints[num66];
								float num68 = (float)((byte)(num67 >> 16)) / 255f;
								float num69 = (float)((byte)(num67 >> 8)) / 255f;
								float num70 = (float)((byte)num67) / 255f;
								bool flag35 = !ChunkGeometryBuilder.NoTint.Equals(ChunkGeometryBuilder.ForceTint);
								if (flag35)
								{
									num68 = (float)((byte)ChunkGeometryBuilder.ForceTint.X) / 255f;
									num69 = (float)((byte)ChunkGeometryBuilder.ForceTint.Y) / 255f;
									num70 = (float)((byte)ChunkGeometryBuilder.ForceTint.Z) / 255f;
								}
								clientShaderEffect = (ClientBlockType.ClientShaderEffect)((byte)(num67 >> 24));
								cornerShaderEffects[l] = clientShaderEffect;
								num37 = (uint)(num37 * num68);
								num38 = (uint)(num38 * num69);
								num39 = (uint)(num39 * num70);
							}
							uint num71 = num37 | num38 << 8 | num39 << 16 | (uint)((uint)clientShaderEffect << 24);
							cubeVertexData.Vertices[(int)(checked((IntPtr)(unchecked((ulong)cubeVertexData.VerticesOffset + (ulong)((long)l)))))] = new ChunkVertex
							{
								PositionPacked = positionPacked,
								DoubleSidedAndBlockId = (ushort)((1U & num22) << 15 | (uint)VertexCompression.CompressBlockLocalPosition(chunkX, chunkY, chunkZ)),
								NormalAndNodeIndex = normalAndNodeIndex2,
								TextureCoordinates = texCoordsByCorner[l],
								MaskTextureCoordinates = sideMaskTexCoordsByCorner[l],
								TintColorAndEffectAndShadingMode = (num71 | num53 << 24 | (uint)((uint)blockType.CubeShadingMode << 30)),
								GlowColorAndSunlight = num36,
								UseBillboard = 0U
							};
						}
						bool flag36 = cornerOcclusions[0] + cornerOcclusions[2] > cornerOcclusions[1] + cornerOcclusions[3] || cornerShaderEffects[0] != cornerShaderEffects[2];
						cubeVertexData.Indices[cubeVertexData.IndicesOffset] = cubeVertexData.VerticesOffset;
						cubeVertexData.Indices[cubeVertexData.IndicesOffset + 1] = cubeVertexData.VerticesOffset + 1U;
						cubeVertexData.Indices[cubeVertexData.IndicesOffset + 2] = cubeVertexData.VerticesOffset + (flag36 ? 3U : 2U);
						cubeVertexData.Indices[cubeVertexData.IndicesOffset + 3] = cubeVertexData.VerticesOffset + (flag36 ? 1U : 0U);
						cubeVertexData.Indices[cubeVertexData.IndicesOffset + 4] = cubeVertexData.VerticesOffset + 2U;
						cubeVertexData.Indices[cubeVertexData.IndicesOffset + 5] = cubeVertexData.VerticesOffset + 3U;
						bool flag37 = k == 0;
						if (flag37)
						{
							num21 = cubeVertexData.VerticesOffset;
							flag6 = flag36;
						}
						cubeVertexData.VerticesOffset += 4U;
						cubeVertexData.IndicesOffset += 6;
						goto IL_197D;
						IL_D1B:
						throw new Exception(string.Format("Unsupported Z rotation value for texture: {0}°", cubeTexture.Rotation));
					}
					IL_197D:
					int num72 = borderedChunkBlocks[num25];
					bool flag38 = !isAnimated && k >= 2 && flag7 && num72 != int.MaxValue;
					if (flag38)
					{
						ClientBlockType clientBlockType = clientBlockTypes[num72];
						ClientBlockType adjacentTopClientBlockType = null;
						int num73 = num25 + ChunkGeometryBuilder.AdjacentBlockOffsetsBySide[0].Main;
						bool flag39 = num73 < borderedChunkBlocks.Length && borderedChunkBlocks[num73] != int.MaxValue;
						if (flag39)
						{
							adjacentTopClientBlockType = clientBlockTypes[borderedChunkBlocks[num73]];
						}
						bool flag40 = ChunkGeometryBuilder.ShouldAddTransition(blockType, clientBlockType, adjacentTopClientBlockType);
						if (flag40)
						{
							int num74 = clientBlockType.TransitionTextureAtlasIndex * 32;
							float num75 = (float)(num74 % atlasTextureWidth);
							float num76 = (float)(num74 / atlasTextureWidth * 32);
							float u9 = (num75 + 0.04f) / (float)atlasTextureWidth;
							float u10 = (num75 + 32f - 0.04f) / (float)atlasTextureWidth;
							float u11 = (num76 + 0.04f) / (float)atlasTextureHeight;
							float u12 = (num76 + 32f - 0.04f) / (float)atlasTextureHeight;
							ushort x7 = VertexCompression.NormalizedTexCoordToUshort(u9);
							ushort x8 = VertexCompression.NormalizedTexCoordToUshort(u10);
							ushort y5 = VertexCompression.NormalizedTexCoordToUshort(u11);
							ushort y6 = VertexCompression.NormalizedTexCoordToUshort(u12);
							switch (k)
							{
							case 2:
								texCoordsByCorner[0].X = x8;
								texCoordsByCorner[0].Y = y6;
								texCoordsByCorner[1].X = x8;
								texCoordsByCorner[1].Y = y5;
								texCoordsByCorner[2].X = x7;
								texCoordsByCorner[2].Y = y5;
								texCoordsByCorner[3].X = x7;
								texCoordsByCorner[3].Y = y6;
								break;
							case 3:
								texCoordsByCorner[0].X = x7;
								texCoordsByCorner[0].Y = y5;
								texCoordsByCorner[1].X = x7;
								texCoordsByCorner[1].Y = y6;
								texCoordsByCorner[2].X = x8;
								texCoordsByCorner[2].Y = y6;
								texCoordsByCorner[3].X = x8;
								texCoordsByCorner[3].Y = y5;
								break;
							case 4:
								texCoordsByCorner[0].X = x7;
								texCoordsByCorner[0].Y = y6;
								texCoordsByCorner[1].X = x8;
								texCoordsByCorner[1].Y = y6;
								texCoordsByCorner[2].X = x8;
								texCoordsByCorner[2].Y = y5;
								texCoordsByCorner[3].X = x7;
								texCoordsByCorner[3].Y = y5;
								break;
							case 5:
								texCoordsByCorner[0].X = x8;
								texCoordsByCorner[0].Y = y5;
								texCoordsByCorner[1].X = x7;
								texCoordsByCorner[1].Y = y5;
								texCoordsByCorner[2].X = x7;
								texCoordsByCorner[2].Y = y6;
								texCoordsByCorner[3].X = x8;
								texCoordsByCorner[3].Y = y6;
								break;
							}
							int num77 = clientBlockType.SelfTintColorsBySide[0];
							byte b15 = (byte)(num77 >> 16);
							byte b16 = (byte)(num77 >> 8);
							byte b17 = (byte)num77;
							float num78 = clientBlockType.BiomeTintMultipliersBySide[0];
							int num79 = 0;
							int num80 = 0;
							for (int n = 0; n < 4; n++)
							{
								bool flag41 = k == 4;
								if (flag41)
								{
									switch (n)
									{
									case 0:
									case 3:
										num79 = 1;
										num80 = 1;
										break;
									case 1:
									case 2:
										num79 = 0;
										num80 = 1;
										break;
									}
								}
								else
								{
									bool flag42 = k == 5;
									if (flag42)
									{
										switch (n)
										{
										case 0:
										case 3:
											num79 = 1;
											num80 = 0;
											break;
										case 1:
										case 2:
											num79 = 0;
											num80 = 0;
											break;
										}
									}
									else
									{
										bool flag43 = k == 3;
										if (flag43)
										{
											int num81 = n;
											int num82 = num81;
											if (num82 > 1)
											{
												if (num82 - 2 <= 1)
												{
													num79 = 1;
													num80 = 1;
												}
											}
											else
											{
												num79 = 1;
												num80 = 0;
											}
										}
										else
										{
											bool flag44 = k == 2;
											if (flag44)
											{
												int num83 = n;
												int num84 = num83;
												if (num84 > 1)
												{
													if (num84 - 2 <= 1)
													{
														num79 = 0;
														num80 = 1;
													}
												}
												else
												{
													num79 = 0;
													num80 = 0;
												}
											}
										}
									}
								}
								uint num85 = borderedColumnTints[ChunkHelper.IndexInBorderedChunkColumn(chunkX + num79, chunkZ + num80)];
								byte b18 = (byte)(num85 >> 16);
								byte b19 = (byte)(num85 >> 8);
								byte b20 = (byte)num85;
								uint num86 = (uint)((float)b15 + (float)(b18 - b15) * num78);
								uint num87 = (uint)((float)b16 + (float)(b19 - b16) * num78);
								uint num88 = (uint)((float)b17 + (float)(b20 - b17) * num78);
								uint num89 = num86 | num87 << 8 | num88 << 16;
								num89 |= (uint)((uint)clientBlockType.CubeShaderEffect << 24 | (ClientBlockType.ClientShaderEffect)((uint)clientBlockType.CubeShadingMode << 30));
								checked
								{
									ChunkVertex chunkVertex = cubeVertexData.Vertices[(int)((IntPtr)(unchecked((ulong)num21 + (ulong)((long)n))))];
									alphaTestedVertexData.Vertices[(int)((IntPtr)(unchecked((ulong)alphaTestedVertexData.VerticesOffset + (ulong)((long)n))))] = new ChunkVertex
									{
										PositionPacked = chunkVertex.PositionPacked,
										DoubleSidedAndBlockId = chunkVertex.DoubleSidedAndBlockId,
										NormalAndNodeIndex = chunkVertex.NormalAndNodeIndex,
										TextureCoordinates = texCoordsByCorner[n],
										MaskTextureCoordinates = ChunkGeometryBuilder.NoSideMaskUV,
										TintColorAndEffectAndShadingMode = num89,
										GlowColorAndSunlight = chunkVertex.GlowColorAndSunlight,
										UseBillboard = chunkVertex.UseBillboard
									};
								}
							}
							int num90 = alphaTestedAnimatedLowLODIndicesStart + alphaTestedLowLODIndicesOffset;
							alphaTestedVertexData.Indices[num90] = alphaTestedVertexData.VerticesOffset;
							alphaTestedVertexData.Indices[num90 + 1] = alphaTestedVertexData.VerticesOffset + 1U;
							alphaTestedVertexData.Indices[num90 + 2] = alphaTestedVertexData.VerticesOffset + (flag6 ? 3U : 2U);
							alphaTestedVertexData.Indices[num90 + 3] = alphaTestedVertexData.VerticesOffset + (flag6 ? 1U : 0U);
							alphaTestedVertexData.Indices[num90 + 4] = alphaTestedVertexData.VerticesOffset + 2U;
							alphaTestedVertexData.Indices[num90 + 5] = alphaTestedVertexData.VerticesOffset + 3U;
							alphaTestedVertexData.VerticesOffset += 4U;
							alphaTestedLowLODIndicesOffset += 6;
						}
					}
				}
			}
		}

		// Token: 0x06005531 RID: 21809 RVA: 0x0018CDF0 File Offset: 0x0018AFF0
		private static bool ShouldAddTransition(ClientBlockType clientBlockType, ClientBlockType adjacentClientBlockType, ClientBlockType adjacentTopClientBlockType)
		{
			bool flag = clientBlockType.TransitionGroupId == -1 || adjacentClientBlockType.TransitionTextureAtlasIndex == -1 || adjacentClientBlockType.TransitionToGroupIds == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = clientBlockType == adjacentClientBlockType || (adjacentTopClientBlockType != null && adjacentTopClientBlockType.ShouldRenderCube && !adjacentTopClientBlockType.RequiresAlphaBlending);
				if (flag2)
				{
					result = false;
				}
				else
				{
					for (int i = 0; i < adjacentClientBlockType.TransitionToGroupIds.Length; i++)
					{
						int num = adjacentClientBlockType.TransitionToGroupIds[i];
						bool flag3 = num == -1 || num != clientBlockType.TransitionGroupId;
						if (!flag3)
						{
							bool flag4 = adjacentClientBlockType.TransitionGroupId == clientBlockType.TransitionGroupId && clientBlockType.TransitionToGroupIds != null;
							if (flag4)
							{
								for (int j = 0; j < clientBlockType.TransitionToGroupIds.Length; j++)
								{
									bool flag5 = clientBlockType.TransitionToGroupIds[j] == clientBlockType.TransitionGroupId;
									if (flag5)
									{
										return adjacentClientBlockType.Id > clientBlockType.Id;
									}
								}
							}
							return true;
						}
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06005532 RID: 21810 RVA: 0x0018CF0C File Offset: 0x0018B10C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool ShouldRegisterSound(ClientBlockType blockType)
		{
			bool result;
			if (blockType.SoundEventIndex != 0U)
			{
				Dictionary<InteractionType, int> interactions = blockType.Interactions;
				result = ((interactions != null && interactions.Count == 0) || !blockType.IsAnimated());
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06005533 RID: 21811 RVA: 0x0018CF4C File Offset: 0x0018B14C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int CalculateBlockTextureIndex(float[] weights, int seed)
		{
			bool flag = weights.Length <= 1;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				double val = (double)(seed & int.MaxValue) / 2147483647.0;
				double num = Math.Min(val, 0.9999);
				for (int i = 0; i < weights.Length; i++)
				{
					bool flag2 = (num -= (double)weights[i]) <= 0.0001;
					if (flag2)
					{
						return i;
					}
				}
				result = 0;
			}
			return result;
		}

		// Token: 0x040031B7 RID: 12727
		private const float MaskOffsetEpsilon = 0.0001f;

		// Token: 0x040031B8 RID: 12728
		public static readonly int[][] AdjacentBlockSignsByCorner = new int[][]
		{
			new int[]
			{
				1,
				1
			},
			new int[]
			{
				-1,
				1
			},
			new int[]
			{
				-1,
				-1
			},
			new int[]
			{
				1,
				-1
			}
		};

		// Token: 0x040031B9 RID: 12729
		public static readonly ChunkGeometryBuilder.AdjacentBlockOffsets[] AdjacentBlockOffsetsBySide = new ChunkGeometryBuilder.AdjacentBlockOffsets[]
		{
			new ChunkGeometryBuilder.AdjacentBlockOffsets
			{
				Main = ChunkHelper.IndexOfBlockInBorderedChunk(0, 1, 0),
				Horizontal = ChunkHelper.IndexOfBlockInBorderedChunk(1, 0, 0),
				Vertical = ChunkHelper.IndexOfBlockInBorderedChunk(0, 0, -1),
				SideMaskOffset = new Vector3(0f, 0.0001f, 0f),
				Normal = new Vector3(0f, 1f, 0f),
				PackedNormal = VertexCompression.NormalizedXYZToUint(0f, 1f, 0f)
			},
			new ChunkGeometryBuilder.AdjacentBlockOffsets
			{
				Main = ChunkHelper.IndexOfBlockInBorderedChunk(0, -1, 0),
				Horizontal = ChunkHelper.IndexOfBlockInBorderedChunk(-1, 0, 0),
				Vertical = ChunkHelper.IndexOfBlockInBorderedChunk(0, 0, -1),
				SideMaskOffset = new Vector3(0f, -0.0001f, 0f),
				Normal = new Vector3(0f, -1f, 0f),
				PackedNormal = VertexCompression.NormalizedXYZToUint(0f, -1f, 0f)
			},
			new ChunkGeometryBuilder.AdjacentBlockOffsets
			{
				Main = ChunkHelper.IndexOfBlockInBorderedChunk(-1, 0, 0),
				Horizontal = ChunkHelper.IndexOfBlockInBorderedChunk(0, 0, 1),
				Vertical = ChunkHelper.IndexOfBlockInBorderedChunk(0, 1, 0),
				SideMaskOffset = new Vector3(-0.0001f, 0f, 0f),
				Normal = new Vector3(-1f, 0f, 0f),
				PackedNormal = VertexCompression.NormalizedXYZToUint(-1f, 0f, 0f)
			},
			new ChunkGeometryBuilder.AdjacentBlockOffsets
			{
				Main = ChunkHelper.IndexOfBlockInBorderedChunk(1, 0, 0),
				Horizontal = ChunkHelper.IndexOfBlockInBorderedChunk(0, 0, -1),
				Vertical = ChunkHelper.IndexOfBlockInBorderedChunk(0, 1, 0),
				SideMaskOffset = new Vector3(0.0001f, 0f, 0f),
				Normal = new Vector3(1f, 0f, 0f),
				PackedNormal = VertexCompression.NormalizedXYZToUint(1f, 0f, 0f)
			},
			new ChunkGeometryBuilder.AdjacentBlockOffsets
			{
				Main = ChunkHelper.IndexOfBlockInBorderedChunk(0, 0, 1),
				Horizontal = ChunkHelper.IndexOfBlockInBorderedChunk(1, 0, 0),
				Vertical = ChunkHelper.IndexOfBlockInBorderedChunk(0, 1, 0),
				SideMaskOffset = new Vector3(0f, 0f, 0.0001f),
				Normal = new Vector3(0f, 0f, 1f),
				PackedNormal = VertexCompression.NormalizedXYZToUint(0f, 0f, 1f)
			},
			new ChunkGeometryBuilder.AdjacentBlockOffsets
			{
				Main = ChunkHelper.IndexOfBlockInBorderedChunk(0, 0, -1),
				Horizontal = ChunkHelper.IndexOfBlockInBorderedChunk(-1, 0, 0),
				Vertical = ChunkHelper.IndexOfBlockInBorderedChunk(0, 1, 0),
				SideMaskOffset = new Vector3(0f, 0f, -0.0001f),
				Normal = new Vector3(0f, 0f, -1f),
				PackedNormal = VertexCompression.NormalizedXYZToUint(0f, 0f, -1f)
			}
		};

		// Token: 0x040031BA RID: 12730
		public const int BlockSize = 32;

		// Token: 0x040031BB RID: 12731
		public const float BlockScale = 0.03125f;

		// Token: 0x040031BC RID: 12732
		public const float TextureBleedOffset = 0.04f;

		// Token: 0x040031BD RID: 12733
		public static readonly UShortVector2 NoSideMaskUV = new UShortVector2
		{
			X = 0,
			Y = 0
		};

		// Token: 0x040031BE RID: 12734
		public static Matrix PositiveHalfBlockOffsetMatrix = Matrix.CreateTranslation(0.5f, 0.5f, 0.5f);

		// Token: 0x040031BF RID: 12735
		public static Matrix NegativeHalfBlockOffsetMatrix = Matrix.CreateTranslation(-0.5f, -0.5f, -0.5f);

		// Token: 0x040031C0 RID: 12736
		private static readonly Vector3 BackBottomLeft = new Vector3(0f, 0f, 0f);

		// Token: 0x040031C1 RID: 12737
		private static readonly Vector3 BackBottomRight = new Vector3(1f, 0f, 0f);

		// Token: 0x040031C2 RID: 12738
		private static readonly Vector3 BackTopLeft = new Vector3(0f, 1f, 0f);

		// Token: 0x040031C3 RID: 12739
		private static readonly Vector3 BackTopRight = new Vector3(1f, 1f, 0f);

		// Token: 0x040031C4 RID: 12740
		private static readonly Vector3 FrontBottomLeft = new Vector3(0f, 0f, 1f);

		// Token: 0x040031C5 RID: 12741
		private static readonly Vector3 FrontBottomRight = new Vector3(1f, 0f, 1f);

		// Token: 0x040031C6 RID: 12742
		private static readonly Vector3 FrontTopLeft = new Vector3(0f, 1f, 1f);

		// Token: 0x040031C7 RID: 12743
		private static readonly Vector3 FrontTopRight = new Vector3(1f, 1f, 1f);

		// Token: 0x040031C8 RID: 12744
		public static readonly Vector3[][] CornersPerSide = new Vector3[][]
		{
			new Vector3[]
			{
				ChunkGeometryBuilder.BackTopRight,
				ChunkGeometryBuilder.BackTopLeft,
				ChunkGeometryBuilder.FrontTopLeft,
				ChunkGeometryBuilder.FrontTopRight
			},
			new Vector3[]
			{
				ChunkGeometryBuilder.BackBottomLeft,
				ChunkGeometryBuilder.BackBottomRight,
				ChunkGeometryBuilder.FrontBottomRight,
				ChunkGeometryBuilder.FrontBottomLeft
			},
			new Vector3[]
			{
				ChunkGeometryBuilder.FrontTopLeft,
				ChunkGeometryBuilder.BackTopLeft,
				ChunkGeometryBuilder.BackBottomLeft,
				ChunkGeometryBuilder.FrontBottomLeft
			},
			new Vector3[]
			{
				ChunkGeometryBuilder.BackTopRight,
				ChunkGeometryBuilder.FrontTopRight,
				ChunkGeometryBuilder.FrontBottomRight,
				ChunkGeometryBuilder.BackBottomRight
			},
			new Vector3[]
			{
				ChunkGeometryBuilder.FrontTopRight,
				ChunkGeometryBuilder.FrontTopLeft,
				ChunkGeometryBuilder.FrontBottomLeft,
				ChunkGeometryBuilder.FrontBottomRight
			},
			new Vector3[]
			{
				ChunkGeometryBuilder.BackTopLeft,
				ChunkGeometryBuilder.BackTopRight,
				ChunkGeometryBuilder.BackBottomRight,
				ChunkGeometryBuilder.BackBottomLeft
			}
		};

		// Token: 0x040031C9 RID: 12745
		public BlockingCollection<RenderedChunk.ChunkUpdateTask> ChunkUpdateTaskQueue = new BlockingCollection<RenderedChunk.ChunkUpdateTask>();

		// Token: 0x040031CA RID: 12746
		public ConcurrentQueue<Disposable> DisposeRequests = new ConcurrentQueue<Disposable>();

		// Token: 0x040031CB RID: 12747
		private ClientBlockType[] _clientBlockTypes;

		// Token: 0x040031CC RID: 12748
		private BlockHitbox[] _blockHitboxes;

		// Token: 0x040031CD RID: 12749
		private ClientWorldEnvironment[] _environments;

		// Token: 0x040031CE RID: 12750
		private float[] _lightLevels;

		// Token: 0x040031CF RID: 12751
		private Point[] _atlasSizes;

		// Token: 0x040031D0 RID: 12752
		private bool _LODEnabled = true;

		// Token: 0x040031D1 RID: 12753
		private const float FluidShaderHeightThreshold = 0.125f;

		// Token: 0x040031D2 RID: 12754
		private byte[] _solidBlockHeight = new byte[1024];

		// Token: 0x040031D3 RID: 12755
		private readonly byte[] _chunkVisibleSideFlags = new byte[32768];

		// Token: 0x040031D4 RID: 12756
		private readonly UShortVector2[] _texCoordsByCorner = new UShortVector2[4];

		// Token: 0x040031D5 RID: 12757
		private readonly UShortVector2[] _sideMaskTexCoordsByCorner = new UShortVector2[4];

		// Token: 0x040031D6 RID: 12758
		private readonly int[] _cornerOcclusions = new int[4];

		// Token: 0x040031D7 RID: 12759
		private readonly ushort[] _environmentTracker = new ushort[3468];

		// Token: 0x040031D8 RID: 12760
		private readonly uint[] _cornerEnvironmentWaterTints = new uint[8];

		// Token: 0x040031D9 RID: 12761
		private readonly ClientBlockType.ClientShaderEffect[] _cornerShaderEffects = new ClientBlockType.ClientShaderEffect[4];

		// Token: 0x040031DA RID: 12762
		private Matrix _tempBlockRotationMatrix;

		// Token: 0x040031DB RID: 12763
		private Matrix _tempBlockWorldMatrix;

		// Token: 0x040031DC RID: 12764
		private Matrix _tempCubeBlockWorldInvertMatrix;

		// Token: 0x040031DD RID: 12765
		public static ShortVector3 NoTint = default(ShortVector3);

		// Token: 0x040031DE RID: 12766
		public static ShortVector3 ForceTint = ChunkGeometryBuilder.NoTint;

		// Token: 0x02000EE0 RID: 3808
		public class AdjacentBlockOffsets
		{
			// Token: 0x040048EF RID: 18671
			public int Main;

			// Token: 0x040048F0 RID: 18672
			public int Vertical;

			// Token: 0x040048F1 RID: 18673
			public int Horizontal;

			// Token: 0x040048F2 RID: 18674
			public Vector3 SideMaskOffset;

			// Token: 0x040048F3 RID: 18675
			public Vector3 Normal;

			// Token: 0x040048F4 RID: 18676
			public uint PackedNormal;
		}
	}
}
