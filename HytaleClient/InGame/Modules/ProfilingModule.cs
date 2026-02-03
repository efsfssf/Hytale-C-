using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using HytaleClient.Core;
using HytaleClient.Data.Map;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Batcher2D;
using HytaleClient.Graphics.Fonts;
using HytaleClient.Graphics.Gizmos;
using HytaleClient.Graphics.Programs;
using HytaleClient.InGame.Modules.Map;
using HytaleClient.InGame.Modules.WorldMap;
using HytaleClient.Math;
using HytaleClient.Utils;

namespace HytaleClient.InGame.Modules
{
	// Token: 0x020008FA RID: 2298
	internal class ProfilingModule : Module
	{
		// Token: 0x17001103 RID: 4355
		// (get) Token: 0x06004460 RID: 17504 RVA: 0x000E7497 File Offset: 0x000E5697
		// (set) Token: 0x06004461 RID: 17505 RVA: 0x000E749F File Offset: 0x000E569F
		public bool IsVisible { get; private set; } = false;

		// Token: 0x17001104 RID: 4356
		// (get) Token: 0x06004462 RID: 17506 RVA: 0x000E74A8 File Offset: 0x000E56A8
		// (set) Token: 0x06004463 RID: 17507 RVA: 0x000E74B0 File Offset: 0x000E56B0
		public float AccumulatedFrameTime { get; private set; }

		// Token: 0x17001105 RID: 4357
		// (get) Token: 0x06004464 RID: 17508 RVA: 0x000E74B9 File Offset: 0x000E56B9
		// (set) Token: 0x06004465 RID: 17509 RVA: 0x000E74C1 File Offset: 0x000E56C1
		public int AccumulatedFrames { get; private set; }

		// Token: 0x17001106 RID: 4358
		// (get) Token: 0x06004466 RID: 17510 RVA: 0x000E74CA File Offset: 0x000E56CA
		// (set) Token: 0x06004467 RID: 17511 RVA: 0x000E74D2 File Offset: 0x000E56D2
		public float MeanFrameDuration { get; private set; }

		// Token: 0x17001107 RID: 4359
		// (get) Token: 0x06004468 RID: 17512 RVA: 0x000E74DB File Offset: 0x000E56DB
		// (set) Token: 0x06004469 RID: 17513 RVA: 0x000E74E3 File Offset: 0x000E56E3
		public int DrawnTriangles { get; private set; }

		// Token: 0x17001108 RID: 4360
		// (get) Token: 0x0600446A RID: 17514 RVA: 0x000E74EC File Offset: 0x000E56EC
		// (set) Token: 0x0600446B RID: 17515 RVA: 0x000E74F4 File Offset: 0x000E56F4
		public int DrawCallsCount { get; private set; }

		// Token: 0x0600446C RID: 17516 RVA: 0x000E74FD File Offset: 0x000E56FD
		public void SetDrawCallStats(int drawCallCount, int drawnTriangles)
		{
			this.DrawCallsCount = drawCallCount;
			this.DrawnTriangles = drawnTriangles;
		}

		// Token: 0x17001109 RID: 4361
		// (get) Token: 0x0600446D RID: 17517 RVA: 0x000E7510 File Offset: 0x000E5710
		// (set) Token: 0x0600446E RID: 17518 RVA: 0x000E7518 File Offset: 0x000E5718
		public int TotalSentPacketLength { get; private set; } = 0;

		// Token: 0x1700110A RID: 4362
		// (get) Token: 0x0600446F RID: 17519 RVA: 0x000E7521 File Offset: 0x000E5721
		// (set) Token: 0x06004470 RID: 17520 RVA: 0x000E7529 File Offset: 0x000E5729
		public int TotalReceivedPacketLength { get; private set; } = 0;

		// Token: 0x1700110B RID: 4363
		// (get) Token: 0x06004471 RID: 17521 RVA: 0x000E7532 File Offset: 0x000E5732
		// (set) Token: 0x06004472 RID: 17522 RVA: 0x000E753A File Offset: 0x000E573A
		public float LastAccumulatedSentPacketLength { get; private set; }

		// Token: 0x1700110C RID: 4364
		// (get) Token: 0x06004473 RID: 17523 RVA: 0x000E7543 File Offset: 0x000E5743
		// (set) Token: 0x06004474 RID: 17524 RVA: 0x000E754B File Offset: 0x000E574B
		public float LastAccumulatedReceivedPacketLength { get; private set; }

		// Token: 0x06004475 RID: 17525 RVA: 0x000E7554 File Offset: 0x000E5754
		public ProfilingModule(GameInstance gameInstance) : base(gameInstance)
		{
			GraphicsDevice graphics = this._gameInstance.Engine.Graphics;
			this._font = this._gameInstance.App.Fonts.MonospaceFontFamily.RegularFont;
			this._batcher2d = new Batcher2D(graphics, false);
			this._textStats[0].Text = "Branch:   " + BuildInfo.BranchName;
			this._textStats[1].Text = "Revision: " + BuildInfo.RevisionId;
			string str = graphics.GPUInfo.Vendor.ToString();
			string renderer = graphics.GPUInfo.Renderer;
			string version = graphics.GPUInfo.Version;
			int availableNow = graphics.VideoMemory.AvailableNow;
			int capacity = graphics.VideoMemory.Capacity;
			int num = capacity - availableNow;
			this._textStats[2].Text = "Vendor:   " + str;
			this._textStats[3].Text = "Renderer: " + renderer;
			this._textStats[4].Text = "Version: " + version;
			this._textStats[5].Text = string.Format("VRAM: {0} / {1} MB", num / 1024, capacity / 1024);
			this._textStats[6].Text = "";
			this._textStats[17].Text = "";
			this._textStats[18].Text = "";
			this._textStats[8].Text = "";
			this._textStats[9].Text = "";
			this._textStats[12].Text = "";
			for (int i = 0; i < this._textStats.Length; i++)
			{
				this._textStats[i].Color = UInt32Color.White;
			}
			this._textStats[27].Text = "Sound Effect: None";
			this._textStats[26].Text = "Music: None";
			this._statsDrawCount = this._textStats.Length;
			this.InitializeGraphs(this._font);
			this._process = Process.GetCurrentProcess();
		}

		// Token: 0x06004476 RID: 17526 RVA: 0x000E7896 File Offset: 0x000E5A96
		protected override void DoDispose()
		{
			this.DisposeGraphs();
			this._batcher2d.Dispose();
		}

		// Token: 0x06004477 RID: 17527 RVA: 0x000E78AC File Offset: 0x000E5AAC
		public override void Initialize()
		{
			bool autoProfiling = OptionsHelper.AutoProfiling;
			if (autoProfiling)
			{
				this.IsVisible = true;
				this._gameInstance.ExecuteCommand(".profiling on all");
			}
		}

		// Token: 0x06004478 RID: 17528 RVA: 0x000E78E0 File Offset: 0x000E5AE0
		public void SetupDetailedMeasures()
		{
			Engine engine = this._gameInstance.Engine;
			int num = 0;
			string text = "";
			Array.Resize<ProfilingModule.Stat>(ref this._textStats, this._textStats.Length + (engine.Profiling.MeasureCount + 1) * 2);
			this._textStats[ProfilingModule._defaultLength].Text = "Measure name";
			this._textStats[ProfilingModule._defaultLength].Color = UInt32Color.White;
			for (int i = 0; i < engine.Profiling.MeasureCount; i++)
			{
				int num2 = ProfilingModule._defaultLength + i + 1;
				string text2 = i.ToString().PadLeft(2) + ". " + engine.Profiling.GetMeasureName(i);
				this._textStats[num2].Text = text2;
				this._textStats[num2].Color = ((i % 2 == 0) ? UInt32Color.FromRGBA(192, 192, 192, byte.MaxValue) : UInt32Color.White);
				bool flag = text2.Length > num;
				if (flag)
				{
					num = text2.Length;
					text = text2;
				}
			}
			text += " ";
			this._largestMeasureNameSize = this._font.CalculateTextWidth(text) * 12f / (float)this._font.BaseSize;
			string text3 = string.Concat(new string[]
			{
				"CPU Avg".PadLeft(9),
				"(Max)".PadLeft(12),
				"GPU Avg".PadLeft(9),
				"Draws".PadLeft(6),
				"Triangles".PadLeft(10)
			});
			this._textStats[ProfilingModule._defaultLength + engine.Profiling.MeasureCount + 1].Text = text3;
			this._textStats[ProfilingModule._defaultLength + engine.Profiling.MeasureCount + 1].Color = UInt32Color.White;
			for (int j = 0; j < engine.Profiling.MeasureCount; j++)
			{
				int num3 = ProfilingModule._defaultLength + engine.Profiling.MeasureCount + 1 + j + 1;
				this._textStats[num3].Text = "";
				this._textStats[num3].Color = ((j % 2 == 0) ? UInt32Color.FromRGBA(192, 192, 192, byte.MaxValue) : UInt32Color.White);
			}
			this._measurableTableWidth = this._font.CalculateTextWidth(text3) * 12f / (float)this._font.BaseSize;
			this.Resize(this._gameInstance.Engine.Window.Viewport.Width, this._gameInstance.Engine.Window.Viewport.Height);
		}

		// Token: 0x06004479 RID: 17529 RVA: 0x000E7BE0 File Offset: 0x000E5DE0
		public void Resize(int width, int height)
		{
			this._width = width;
			this._height = height;
			float viewportScale = this._gameInstance.Engine.Window.ViewportScale;
			float num = 12f * viewportScale;
			float num2 = 10f * viewportScale;
			float num3 = 2f * num2;
			float num4 = (float)this._width - num3;
			float num5 = num2;
			for (int i = 0; i < ProfilingModule._defaultLength; i++)
			{
				this._textStats[i].Position.X = num3;
				this._textStats[i].Position.Y = num5;
				num5 += num;
			}
			float num6 = num2 + num4 - this._measurableTableWidth * viewportScale - num3 - this._largestMeasureNameSize * viewportScale;
			num5 = num2;
			for (int j = ProfilingModule._defaultLength; j < ProfilingModule._defaultLength + this._gameInstance.Engine.Profiling.MeasureCount + 1; j++)
			{
				this._textStats[j].Position.X = num6;
				this._textStats[j].Position.Y = num5;
				num5 += num;
			}
			num6 += this._largestMeasureNameSize * viewportScale;
			num5 = num2;
			for (int k = ProfilingModule._defaultLength + this._gameInstance.Engine.Profiling.MeasureCount + 1; k < ProfilingModule._defaultLength + this._gameInstance.Engine.Profiling.MeasureCount + 1 + this._gameInstance.Engine.Profiling.MeasureCount + 1; k++)
			{
				this._textStats[k].Position.X = num6;
				this._textStats[k].Position.Y = num5;
				num5 += num;
			}
			this.UpdateOrthographicProjectionMatrix();
			this.UpdateGraphWindowScale();
		}

		// Token: 0x0600447A RID: 17530 RVA: 0x000E7DD8 File Offset: 0x000E5FD8
		private void UpdateOrthographicProjectionMatrix()
		{
			Matrix.CreateOrthographicOffCenter(0f, (float)this._width, 0f, (float)this._height, 0.1f, 1000f, out this._graphOrthographicProjectionMatrix);
			Matrix.CreateOrthographicOffCenter(0f, (float)this._width, (float)this._height, 0f, 0.1f, 1000f, out this._batcherOrthographicProjectionMatrix);
		}

		// Token: 0x0600447B RID: 17531 RVA: 0x000E7E44 File Offset: 0x000E6044
		[Obsolete]
		public override void Tick()
		{
			bool flag = this._gameInstance.Input.ConsumeBinding(this._gameInstance.App.Settings.InputBindings.ToggleProfiling, false);
			if (flag)
			{
				this.IsVisible = !this.IsVisible;
			}
			bool flag2 = !this.IsVisible;
			if (!flag2)
			{
				Vector3 position = this._gameInstance.LocalPlayer.Position;
				this._statsDrawCount = (this.IsDetailedProfilingEnabled ? this._textStats.Length : ProfilingModule._defaultLength);
				Vector2 viewportSize = this._gameInstance.SceneRenderer.Data.ViewportSize;
				this._textStats[7].Text = string.Format("Resolution: {0}x{1}", viewportSize.X, viewportSize.Y);
				this._textStats[10].Text = string.Format("Map Atlas: {0}x{1}, ", this._gameInstance.MapModule.TextureAtlas.Width, this._gameInstance.MapModule.TextureAtlas.Height) + string.Format("Entity Atlas: {0}x{1}", this._gameInstance.EntityStoreModule.TextureAtlas.Width, this._gameInstance.EntityStoreModule.TextureAtlas.Height);
				this._textStats[11].Text = string.Format("View Distance: {0}, Effective: {1:##.0}", this._gameInstance.App.Settings.ViewDistance, this._gameInstance.MapModule.EffectiveViewDistance);
				this._textStats[13].Text = string.Format("Entities: {0}", this._gameInstance.EntityStoreModule.GetEntitiesCount());
				this._textStats[14].Text = string.Format("Particles - Systems: {0} ({1}), ", this._gameInstance.Engine.FXSystem.Particles.ParticleSystemCount, this._gameInstance.Engine.FXSystem.Particles.ParticleSystemProxyCount) + string.Format("Draws: - Blend: {0} - Erosion: {1} ", this._gameInstance.Engine.FXSystem.Particles.PreviousFrameBlendDrawCount, this._gameInstance.Engine.FXSystem.Particles.PreviousFrameErosionDrawCount) + string.Format("- Distortion: {0}", this._gameInstance.Engine.FXSystem.Particles.PreviousFrameDistortionDrawCount);
				this._textStats[15].Text = string.Format("Trails: {0} ({1})", this._gameInstance.Engine.FXSystem.Trails.TrailCount, this._gameInstance.Engine.FXSystem.Trails.TrailProxyCount);
				this._textStats[16].Text = string.Format("Immersive Views: {0}", this._gameInstance.ImmersiveScreenModule.GetScreenCount());
				this._textStats[19].Text = AssetManager.GetAssetCountInfo();
				this._textStats[20].Text = string.Format("Audio Events: {0} ", this._gameInstance.Engine.Audio.PlaybackCount);
				int num = (int)Math.Floor((double)position.X);
				int num2 = (int)Math.Floor((double)position.Y);
				int num3 = (int)Math.Floor((double)position.Z);
				int num4 = num >> 5;
				int num5 = num2 >> 5;
				int num6 = num3 >> 5;
				int num7 = num - num4 * 32;
				int num8 = num2 - num5 * 32;
				int num9 = num3 - num6 * 32;
				ChunkColumn chunkColumn = this._gameInstance.MapModule.GetChunkColumn(num4, num6);
				bool flag3 = chunkColumn != null;
				if (flag3)
				{
					int num10 = (num9 << 5) + num7;
					uint num11 = chunkColumn.Tints[num10];
					this._textStats[21].Text = string.Format("Heightmap: {0}, ", chunkColumn.Heights[num10]) + string.Format("Tint: #{0:X2}{1:X2}{2:X2}", (byte)(num11 >> 16), (byte)(num11 >> 8), (byte)num11);
					Chunk chunk = chunkColumn.GetChunk(num5);
					bool flag4 = chunk != null;
					if (flag4)
					{
						string str = "-";
						string str2 = "-";
						int num12 = ChunkHelper.IndexOfWorldBlockInChunk(num, num2, num3);
						bool flag5 = chunk.Data.SelfLightAmounts != null;
						if (flag5)
						{
							ushort num13 = chunk.Data.SelfLightAmounts[num12];
							int num14 = (int)(num13 & 15);
							int num15 = num13 >> 4 & 15;
							int num16 = num13 >> 8 & 15;
							int num17 = num13 >> 12 & 15;
							str = string.Format("R: {0}, G: {1}, B: {2}, S: {3}", new object[]
							{
								num14,
								num15,
								num16,
								num17
							});
						}
						bool flag6 = chunk.Data.BorderedLightAmounts != null;
						if (flag6)
						{
							int num18 = ChunkHelper.IndexOfBlockInBorderedChunk(num12, 0, 0, 0);
							ushort num19 = chunk.Data.BorderedLightAmounts[num18];
							int num20 = (int)(num19 & 15);
							int num21 = num19 >> 4 & 15;
							int num22 = num19 >> 8 & 15;
							int num23 = num19 >> 12 & 15;
							str2 = string.Format("R: {0}, G: {1}, B: {2}, S: {3}", new object[]
							{
								num20,
								num21,
								num22,
								num23
							});
						}
						this._textStats[22].Text = "Light - Local: " + str + ", Global: " + str2;
					}
					else
					{
						this._textStats[22].Text = "Light: -";
					}
				}
				else
				{
					this._textStats[21].Text = "Heightmap: -, Tint: -";
					this._textStats[22].Text = "Light: -";
				}
				WorldMapModule.ClientBiomeData clientBiomeData;
				bool flag7 = this._gameInstance.WorldMapModule.TryGetBiomeAtPosition(position, out clientBiomeData);
				if (flag7)
				{
					this._textStats[23].Text = "Biome: " + clientBiomeData.ZoneName + " - " + clientBiomeData.BiomeName;
				}
				else
				{
					this._textStats[23].Text = "Biome: -";
				}
				this._textStats[24].Text = "Environment: " + this._gameInstance.WeatherModule.CurrentEnvironment.Id;
				this._textStats[25].Text = "Weather: " + this._gameInstance.WeatherModule.CurrentWeather.Id;
				bool flag8 = this._previousMusicSoundIndex != this._gameInstance.AmbienceFXModule.CurrentMusicSoundEventIndex;
				if (flag8)
				{
					string str3;
					bool flag9 = this._gameInstance.AmbienceFXModule.CurrentMusicSoundEventIndex == 0U || !this._gameInstance.Engine.Audio.ResourceManager.DebugWwiseIds.TryGetValue(this._gameInstance.AmbienceFXModule.CurrentMusicSoundEventIndex, out str3);
					if (flag9)
					{
						str3 = "None";
					}
					this._textStats[26].Text = "Music: " + this._gameInstance.AmbienceFXModule.AmbienceFXs[this._gameInstance.AmbienceFXModule.MusicAmbienceFXIndex].Id + " - " + str3;
					this._previousMusicSoundIndex = this._gameInstance.AmbienceFXModule.CurrentMusicSoundEventIndex;
				}
				bool flag10 = this._previousSoundEffectIndex != this._gameInstance.AudioModule.CurrentEffectSoundEventIndex;
				if (flag10)
				{
					string str4 = "None";
					string text;
					bool flag11 = this._gameInstance.AudioModule.CurrentEffectSoundEventIndex != 0U && this._gameInstance.Engine.Audio.ResourceManager.DebugWwiseIds.TryGetValue(this._gameInstance.AudioModule.CurrentEffectSoundEventIndex, out text);
					if (flag11)
					{
						str4 = text;
					}
					this._textStats[27].Text = "Sound Effect: " + str4;
					this._previousSoundEffectIndex = this._gameInstance.AudioModule.CurrentEffectSoundEventIndex;
				}
				this._textStats[28].Text = string.Format("Chunk: ({0}, {1}, {2}) in ({3}, {4}, {5})", new object[]
				{
					num7,
					num8,
					num9,
					num4,
					num5,
					num6
				});
				double num24 = Math.Round((double)position.X, 3);
				double num25 = Math.Round((double)position.Y, 3);
				double num26 = Math.Round((double)position.Z, 3);
				this._textStats[29].Text = string.Format("Feet Position: ({0:##.000}, {1:##.000}, {2:##.000})", num24, num25, num26);
				double num27 = Math.Round((double)(this._gameInstance.LocalPlayer.LookOrientation.X * 180f) / 3.141592653589793, 4);
				double num28 = Math.Round((double)(this._gameInstance.LocalPlayer.LookOrientation.Y * 180f) / 3.141592653589793, 4);
				double num29 = Math.Round((double)(this._gameInstance.LocalPlayer.LookOrientation.Z * 180f) / 3.141592653589793, 4);
				this._textStats[31].Text = string.Format("Orientation: ({0:##.0000}, {1:##.0000}, {2:##.0000})", num27, num28, num29);
				int hitboxCollisionConfigIndex = this._gameInstance.LocalPlayer.HitboxCollisionConfigIndex;
				this._textStats[30].Text = string.Format("Collision Setting: ({0}) - Collided Entities: ({1})", hitboxCollisionConfigIndex, this._gameInstance.CharacterControllerModule.MovementController.CollidedEntities.Count);
				bool hasFoundTargetBlock = this._gameInstance.InteractionModule.HasFoundTargetBlock;
				if (hasFoundTargetBlock)
				{
					HitDetection.RaycastHit targetBlockHit = this._gameInstance.InteractionModule.TargetBlockHit;
					ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[targetBlockHit.BlockId];
					this._textStats[32].Text = string.Format("Target Block: {0} ({1}, {2}, {3}), Hitbox: {4}", new object[]
					{
						clientBlockType.Name,
						targetBlockHit.BlockPosition.X,
						targetBlockHit.BlockPosition.Y,
						targetBlockHit.BlockPosition.Z,
						clientBlockType.HitboxType
					});
				}
				else
				{
					this._textStats[32].Text = "Target Block: -, Hitbox: -";
				}
				double num30 = Math.Round((double)this._gameInstance.CharacterControllerModule.MovementController.LastMoveForce.X, 3);
				double num31 = Math.Round((double)this._gameInstance.CharacterControllerModule.MovementController.LastMoveForce.Y, 3);
				double num32 = Math.Round((double)this._gameInstance.CharacterControllerModule.MovementController.LastMoveForce.Z, 3);
				this._textStats[33].Text = string.Format("Last Move Force: ({0:##.0000}, {1:##.0000}, {2:##.0000})", num30, num31, num32);
				double num33 = Math.Round((double)this._gameInstance.CharacterControllerModule.MovementController.WishDirection.X, 3);
				double num34 = Math.Round((double)this._gameInstance.CharacterControllerModule.MovementController.WishDirection.Y, 3);
				this._textStats[34].Text = string.Format("Wish Direction: ({0:##.0000}, {1:##.0000})", num33, num34);
				this._textStats[35].Text = string.Format("Disposable - Undisposed: {0}, ", Disposable.UndisposedDisposables.Count) + string.Format("Unfinalized: {0}", Disposable.UnfinalizedDisposables.Count);
			}
		}

		// Token: 0x0600447C RID: 17532 RVA: 0x000E8AC8 File Offset: 0x000E6CC8
		[Obsolete]
		public override void OnNewFrame(float time)
		{
			int accumulatedFrames = this.AccumulatedFrames;
			this.AccumulatedFrames = accumulatedFrames + 1;
			int num = Interlocked.Exchange(ref this._gameInstance.Connection.SentPacketLength, 0);
			int num2 = Interlocked.Exchange(ref this._gameInstance.Connection.ReceivedPacketLength, 0);
			this._accumulatedSentPacketLength += num;
			this._accumulatedReceivedPacketLength += num2;
			this.AccumulatedFrameTime += time;
			while (this.AccumulatedFrameTime >= 1f)
			{
				this.MeanFrameDuration = (float)Math.Round((double)(this.AccumulatedFrameTime / (float)this.AccumulatedFrames * 1000f), 2);
				this.TotalSentPacketLength += this._accumulatedSentPacketLength;
				this.TotalReceivedPacketLength += this._accumulatedReceivedPacketLength;
				this.LastAccumulatedSentPacketLength = (float)this._accumulatedSentPacketLength / this.AccumulatedFrameTime;
				this.LastAccumulatedReceivedPacketLength = (float)this._accumulatedReceivedPacketLength / this.AccumulatedFrameTime;
				bool isVisible = this.IsVisible;
				if (isVisible)
				{
					this._textStats[6].Text = string.Format("Mean Frame: {0}ms ({1} frames over {2}s)", this.MeanFrameDuration, this.AccumulatedFrames, this.AccumulatedFrameTime);
					int num3 = this._gameInstance.Engine.Graphics.UpdateAvailableGPUMemory();
					int capacity = this._gameInstance.Engine.Graphics.VideoMemory.Capacity;
					int num4 = capacity - num3;
					this._textStats[5].Text = ((num3 == 0) ? "VRAM: N/A" : string.Format("VRAM: {0} / {1} MB", num4 / 1024, capacity / 1024));
					this._textStats[17].Text = string.Format("Sent:     {0:0.000} KB/s (Total: {1:0.000}  KB)", this.LastAccumulatedSentPacketLength / 1000f, (float)this.TotalSentPacketLength / 1000f);
					this._textStats[18].Text = string.Format("Received: {0:0.000} KB/s (Total: {1:0.000} KB)", this.LastAccumulatedReceivedPacketLength / 1000f, (float)this.TotalReceivedPacketLength / 1000f);
					this._textStats[8].Text = string.Format("Draws: {0}, Triangles: {1}", this.DrawCallsCount, this.DrawnTriangles);
					int shadowCascadeDrawCallCount = this._gameInstance.SceneRenderer.GetShadowCascadeDrawCallCount(0);
					int shadowCascadeDrawCallCount2 = this._gameInstance.SceneRenderer.GetShadowCascadeDrawCallCount(1);
					int shadowCascadeDrawCallCount3 = this._gameInstance.SceneRenderer.GetShadowCascadeDrawCallCount(2);
					int shadowCascadeDrawCallCount4 = this._gameInstance.SceneRenderer.GetShadowCascadeDrawCallCount(3);
					int shadowCascadeKiloTriangleCount = this._gameInstance.SceneRenderer.GetShadowCascadeKiloTriangleCount(0);
					int shadowCascadeKiloTriangleCount2 = this._gameInstance.SceneRenderer.GetShadowCascadeKiloTriangleCount(1);
					int shadowCascadeKiloTriangleCount3 = this._gameInstance.SceneRenderer.GetShadowCascadeKiloTriangleCount(2);
					int shadowCascadeKiloTriangleCount4 = this._gameInstance.SceneRenderer.GetShadowCascadeKiloTriangleCount(3);
					this._textStats[9].Text = string.Format("Shadow Cascades - Draws: {0}/{1}/{2}/{3}, ", new object[]
					{
						shadowCascadeDrawCallCount,
						shadowCascadeDrawCallCount2,
						shadowCascadeDrawCallCount3,
						shadowCascadeDrawCallCount4
					}) + string.Format("Triangles: {0}K/{1}K/{2}K/{3}K", new object[]
					{
						shadowCascadeKiloTriangleCount,
						shadowCascadeKiloTriangleCount2,
						shadowCascadeKiloTriangleCount3,
						shadowCascadeKiloTriangleCount4
					});
					this._textStats[12].Text = string.Format("Chunks - Loaded: {0}, Drawable: {1} (Max: {2}), ", this._gameInstance.MapModule.LoadedChunksCount, this._gameInstance.MapModule.DrawableChunksCount, this._gameInstance.MapModule.GetMaxChunksLoaded()) + string.Format("Chunk Columns: {0}", this._gameInstance.MapModule.ChunkColumnCount());
					bool isDetailedProfilingEnabled = this.IsDetailedProfilingEnabled;
					if (isDetailedProfilingEnabled)
					{
						this.UpdateRenderingProfiles();
					}
				}
				this._accumulatedSentPacketLength -= (int)this.LastAccumulatedSentPacketLength;
				this._accumulatedReceivedPacketLength -= (int)this.LastAccumulatedReceivedPacketLength;
				this.AccumulatedFrames -= (int)((float)this.AccumulatedFrames / this.AccumulatedFrameTime);
				this.AccumulatedFrameTime -= 1f;
			}
			this._fpsData.RecordValue(time * 1000f);
			this._cpuData.RecordValue(this._gameInstance.App.CpuTime * 1000f);
			this._networkDataSent.RecordValue((float)num);
			this._networkDataReceived.RecordValue((float)num2);
			this._garbageCollectionData.RecordValue((float)GC.GetTotalMemory(false) / 1024f / 1024f);
			this._processMemoryData.RecordValue((float)this._process.PrivateMemorySize64 / 1024f / 1024f);
			bool isVisible2 = this.IsVisible;
			if (isVisible2)
			{
				this.UpdateGraphs();
			}
		}

		// Token: 0x0600447D RID: 17533 RVA: 0x000E8FFC File Offset: 0x000E71FC
		private void UpdateRenderingProfiles()
		{
			Engine engine = this._gameInstance.Engine;
			for (int i = 0; i < engine.Profiling.MeasureCount; i++)
			{
				ref Profiling.MeasureInfo measureInfo = ref engine.Profiling.GetMeasureInfo(i);
				ref Profiling.CPUMeasure cpumeasure = ref engine.Profiling.GetCPUMeasure(i);
				ref Profiling.GPUMeasure gpumeasure = ref engine.Profiling.GetGPUMeasure(i);
				string text = "";
				bool flag = measureInfo.AccumulatedFrameCount > 1;
				if (flag)
				{
					bool isEnabled = measureInfo.IsEnabled;
					if (isEnabled)
					{
						float num = (float)Math.Round((double)(cpumeasure.AccumulatedElapsedTime / (float)measureInfo.AccumulatedFrameCount), 4);
						float num2 = (float)Math.Round((double)cpumeasure.MaxElapsedTime, 4);
						text = num.ToString("N4").PadLeft(9) + (" (" + num2.ToString("N4") + ")").PadLeft(12);
						bool hasGpuStats = measureInfo.HasGpuStats;
						if (hasGpuStats)
						{
							float num3 = (float)Math.Round((double)(gpumeasure.AccumulatedElapsedTime / (float)measureInfo.AccumulatedFrameCount), 4);
							int num4 = gpumeasure.DrawnVertices / 3;
							text = text + num3.ToString("N4").PadLeft(9) + gpumeasure.DrawCalls.ToString().PadLeft(6) + num4.ToString().PadLeft(10);
						}
					}
				}
				this._textStats[ProfilingModule._defaultLength + engine.Profiling.MeasureCount + 1 + i + 1].Text = text;
			}
		}

		// Token: 0x0600447E RID: 17534 RVA: 0x000E918C File Offset: 0x000E738C
		public void PrepareForDraw()
		{
			this._batcher2d.RequestDrawTexture(this._gameInstance.Engine.Graphics.WhitePixelTexture, new Rectangle(0, 0, 1, 1), new Vector3(0f, 0f, 0f), (float)this._width, (float)this._height, this._backgroundColor);
			int num = this.IsPartialRenderingProfilesEnabled ? (ProfilingModule._defaultLength + 1) : this._statsDrawCount;
			for (int i = 0; i < num; i++)
			{
				ref ProfilingModule.Stat ptr = ref this._textStats[i];
				this._batcher2d.RequestDrawText(this._font, 12f * this._currentScale, ptr.Text, new Vector3(ptr.Position.X, ptr.Position.Y, 0f), ptr.Color, false, false, 0f);
			}
			bool isPartialRenderingProfilesEnabled = this.IsPartialRenderingProfilesEnabled;
			if (isPartialRenderingProfilesEnabled)
			{
				float viewportScale = this._gameInstance.Engine.Window.ViewportScale;
				float num2 = 12f * viewportScale;
				int num3 = ProfilingModule._defaultLength + 1;
				float num4 = this._textStats[num3].Position.Y;
				for (int j = ProfilingModule._defaultLength + 1; j < ProfilingModule._defaultLength + 1 + this._gameInstance.Engine.Profiling.MeasureCount; j++)
				{
					bool flag = this._gameInstance.Engine.Profiling.GetMeasureInfo(j - ProfilingModule._defaultLength - 1).HasGpuStats == !this.IsCPUOnlyRenderingProfilesEnabled;
					if (flag)
					{
						ref ProfilingModule.Stat ptr2 = ref this._textStats[j];
						this._batcher2d.RequestDrawText(this._font, 12f * this._currentScale, ptr2.Text, new Vector3(ptr2.Position.X, num4, 0f), ptr2.Color, false, false, 0f);
						ref ProfilingModule.Stat ptr3 = ref this._textStats[this._gameInstance.Engine.Profiling.MeasureCount + 1 + j];
						this._batcher2d.RequestDrawText(this._font, 12f * this._currentScale, ptr3.Text, new Vector3(ptr3.Position.X, num4, 0f), ptr3.Color, false, false, 0f);
						num4 += num2;
					}
				}
				ref ProfilingModule.Stat ptr4 = ref this._textStats[ProfilingModule._defaultLength + 1 + this._gameInstance.Engine.Profiling.MeasureCount];
				this._batcher2d.RequestDrawText(this._font, 12f * this._currentScale, ptr4.Text, new Vector3(ptr4.Position.X, ptr4.Position.Y, 0f), ptr4.Color, false, false, 0f);
			}
			this.PrepareForDrawGraphsAxisAndLabels();
		}

		// Token: 0x0600447F RID: 17535 RVA: 0x000E949C File Offset: 0x000E769C
		public void Draw()
		{
			Batcher2DProgram batcher2DProgram = this._gameInstance.Engine.Graphics.GPUProgramStore.Batcher2DProgram;
			batcher2DProgram.MVPMatrix.SetValue(ref this._batcherOrthographicProjectionMatrix);
			this._batcher2d.Draw();
		}

		// Token: 0x06004480 RID: 17536 RVA: 0x000E94E8 File Offset: 0x000E76E8
		private void InitializeGraphs(Font font)
		{
			this._fpsGraph = new Graph(this._gameInstance.Engine.Graphics, this._batcher2d, font, 350, "FPS/CPU");
			this._fpsGraph.Color = new Vector3(1f, 0f, 0f);
			this._fpsGraph.AddAxis("", 0f);
			this._fpsGraph.AddAxis("15", 66.666664f);
			this._fpsGraph.AddAxis("30", 33.333332f);
			this._fpsGraph.AddAxis("60", 16.666666f);
			this._fpsGraph.AddAxis("120", 8.333333f);
			this._fpsGraph.AddAxis("400", 2.5f);
			this._cpuGraph = new Graph(this._gameInstance.Engine.Graphics, this._batcher2d, font, 350, "");
			this._cpuGraph.Color = new Vector3(1f, 1f, 0f);
			this._networkSentGraph = new Graph(this._gameInstance.Engine.Graphics, this._batcher2d, font, 350, "Network Send/Receive");
			this._networkSentGraph.Color = new Vector3(0f, 0.75f, 1f);
			this._networkSentGraph.AddAxis("", 0f);
			this._networkSentGraph.AddAxis("100 B/s", 100f);
			this._networkSentGraph.AddAxis("200 B/s", 200f);
			this._networkSentGraph.AddAxis("400 B/s", 400f);
			this._networkReceivedGraph = new Graph(this._gameInstance.Engine.Graphics, this._batcher2d, font, 350, "");
			this._networkReceivedGraph.Color = new Vector3(1f, 0.75f, 0f);
			this._garbageCollectionGraph = new Graph(this._gameInstance.Engine.Graphics, this._batcher2d, font, 350, "Heap/Process Memory");
			this._garbageCollectionGraph.Color = new Vector3(1f, 1f, 0f);
			this._garbageCollectionGraph.AxisUnit = " MiB";
			this._garbageCollectionGraph.AddAxis("", 0f);
			this._garbageCollectionGraph.AddAxis("512 MiB", 512f);
			this._garbageCollectionGraph.AddAxis("1 GiB", 1024f);
			this._garbageCollectionGraph.AddAxis("2 GiB", 2048f);
			this._garbageCollectionGraph.AddAxis("3 GiB", 3072f);
			this._garbageCollectionGraph.AddAxis("4 GiB", 4096f);
			this._processMemoryGraph = new Graph(this._gameInstance.Engine.Graphics, this._batcher2d, font, 350, "");
			this._processMemoryGraph.Color = new Vector3(1f, 0f, 0f);
			this.UpdateGraphWindowScale();
		}

		// Token: 0x06004481 RID: 17537 RVA: 0x000E9830 File Offset: 0x000E7A30
		private void DisposeGraphs()
		{
			this._fpsGraph.Dispose();
			this._cpuGraph.Dispose();
			this._networkSentGraph.Dispose();
			this._networkReceivedGraph.Dispose();
			this._garbageCollectionGraph.Dispose();
			this._processMemoryGraph.Dispose();
		}

		// Token: 0x06004482 RID: 17538 RVA: 0x000E9888 File Offset: 0x000E7A88
		private void UpdateGraphs()
		{
			bool flag = this._currentScale != this._gameInstance.Engine.Window.ViewportScale;
			if (flag)
			{
				this.UpdateGraphWindowScale();
			}
			this._fpsGraph.UpdateHistory(this._fpsData, 1f);
			this._cpuGraph.UpdateHistory(this._cpuData, 1f);
			this._networkSentGraph.UpdateHistory(this._networkDataSent, 1f);
			this._networkReceivedGraph.UpdateHistory(this._networkDataReceived, 1f);
			float scale = ProfilingModule.CalculateAxisScale(MathHelper.Max(this._garbageCollectionData.AverageValue, this._processMemoryData.AverageValue), 500f);
			this._garbageCollectionGraph.UpdateHistory(this._garbageCollectionData, scale);
			this._processMemoryGraph.UpdateHistory(this._processMemoryData, scale);
		}

		// Token: 0x06004483 RID: 17539 RVA: 0x000E996C File Offset: 0x000E7B6C
		private void UpdateGraphWindowScale()
		{
			float viewportScale = this._gameInstance.Engine.Window.ViewportScale;
			this._fpsGraph.Position = (this._cpuGraph.Position = new Vector3(50f * viewportScale, 115f * viewportScale, -1f));
			this._fpsGraph.LabelPosition = (this._cpuGraph.LabelPosition = new Vector3(50f * viewportScale, (float)this._gameInstance.Engine.Window.Viewport.Height - 115f * viewportScale, -1f));
			this._fpsGraph.Scale = (this._cpuGraph.Scale = new Vector3(1f * viewportScale, 2.25f * viewportScale, 1f));
			this._fpsGraph.UpdateTextHeight(12f * viewportScale);
			this._fpsGraph.UpdateAxisData(ref this._graphOrthographicProjectionMatrix);
			this._cpuGraph.UpdateTextHeight(12f * viewportScale);
			this._cpuGraph.UpdateAxisData(ref this._graphOrthographicProjectionMatrix);
			this._networkSentGraph.Position = (this._networkReceivedGraph.Position = new Vector3(475f * viewportScale, 115f * viewportScale, -1f));
			this._networkSentGraph.LabelPosition = (this._networkReceivedGraph.LabelPosition = new Vector3(475f * viewportScale, (float)this._gameInstance.Engine.Window.Viewport.Height - 115f * viewportScale, -1f));
			this._networkSentGraph.Scale = (this._networkReceivedGraph.Scale = new Vector3(1f * viewportScale, 0.375f * viewportScale, 1f));
			this._networkSentGraph.UpdateTextHeight(12f * viewportScale);
			this._networkSentGraph.UpdateAxisData(ref this._graphOrthographicProjectionMatrix);
			this._networkReceivedGraph.UpdateTextHeight(12f * viewportScale);
			this._networkReceivedGraph.UpdateAxisData(ref this._graphOrthographicProjectionMatrix);
			this._garbageCollectionGraph.Position = new Vector3(915f * viewportScale, 115f * viewportScale, -1f);
			this._garbageCollectionGraph.LabelPosition = new Vector3(915f * viewportScale, (float)this._gameInstance.Engine.Window.Viewport.Height - 115f * viewportScale, -1f);
			this._garbageCollectionGraph.Scale = new Vector3(1f * viewportScale, 0.3f * viewportScale, 1f);
			this._garbageCollectionGraph.UpdateTextHeight(12f * viewportScale);
			this._garbageCollectionGraph.UpdateAxisData(ref this._graphOrthographicProjectionMatrix);
			this._processMemoryGraph.Position = this._garbageCollectionGraph.Position;
			this._processMemoryGraph.LabelPosition = this._garbageCollectionGraph.LabelPosition;
			this._processMemoryGraph.Scale = this._garbageCollectionGraph.Scale;
			this._processMemoryGraph.UpdateTextHeight(12f * viewportScale);
			this._processMemoryGraph.UpdateAxisData(ref this._graphOrthographicProjectionMatrix);
			this._currentScale = viewportScale;
		}

		// Token: 0x06004484 RID: 17540 RVA: 0x000E9C9C File Offset: 0x000E7E9C
		public void DrawGraphsData()
		{
			this._fpsGraph.DrawData();
			this._cpuGraph.DrawData();
			this._networkSentGraph.DrawData();
			this._networkReceivedGraph.DrawData();
			this._processMemoryGraph.DrawData();
			this._garbageCollectionGraph.DrawData();
		}

		// Token: 0x06004485 RID: 17541 RVA: 0x000E9CF2 File Offset: 0x000E7EF2
		public void PrepareForDrawGraphsAxisAndLabels()
		{
			this._fpsGraph.PrepareForDrawAxisAndLabels();
			this._networkSentGraph.PrepareForDrawAxisAndLabels();
			this._garbageCollectionGraph.PrepareForDrawAxisAndLabels();
		}

		// Token: 0x06004486 RID: 17542 RVA: 0x000E9D1C File Offset: 0x000E7F1C
		private static float CalculateAxisScale(float average, float max)
		{
			return (float)Math.Ceiling((double)(MathHelper.Max(1f, average / max) * 2f)) / 2f;
		}

		// Token: 0x040021C9 RID: 8649
		private const float MarginSize = 10f;

		// Token: 0x040021CA RID: 8650
		private const float TextHeight = 12f;

		// Token: 0x040021CB RID: 8651
		private const int MeasurableColumnWidthCpu = 9;

		// Token: 0x040021CC RID: 8652
		private const int MeasurableColumnWidthCpuMax = 12;

		// Token: 0x040021CD RID: 8653
		private const int MeasurableColumnWidthGpu = 9;

		// Token: 0x040021CE RID: 8654
		private const int MeasurableColumnWidthDraws = 6;

		// Token: 0x040021CF RID: 8655
		private const int MeasurableColumnWidthTriangles = 10;

		// Token: 0x040021D1 RID: 8657
		public bool IsDetailedProfilingEnabled = false;

		// Token: 0x040021D2 RID: 8658
		public bool IsCPUOnlyRenderingProfilesEnabled = false;

		// Token: 0x040021D3 RID: 8659
		public bool IsPartialRenderingProfilesEnabled = false;

		// Token: 0x040021D4 RID: 8660
		public Matrix _graphOrthographicProjectionMatrix;

		// Token: 0x040021D5 RID: 8661
		public Matrix _batcherOrthographicProjectionMatrix;

		// Token: 0x040021DB RID: 8667
		private int _width;

		// Token: 0x040021DC RID: 8668
		private int _height;

		// Token: 0x040021DD RID: 8669
		private int _accumulatedReceivedPacketLength;

		// Token: 0x040021DE RID: 8670
		private int _accumulatedSentPacketLength;

		// Token: 0x040021E3 RID: 8675
		private UInt32Color _backgroundColor = UInt32Color.FromRGBA(0, 0, 0, 125);

		// Token: 0x040021E4 RID: 8676
		private Font _font;

		// Token: 0x040021E5 RID: 8677
		private float _largestMeasureNameSize;

		// Token: 0x040021E6 RID: 8678
		private float _measurableTableWidth;

		// Token: 0x040021E7 RID: 8679
		private static readonly int _defaultLength = Enum.GetNames(typeof(ProfilingModule.Keys)).Length;

		// Token: 0x040021E8 RID: 8680
		public int _statsDrawCount = 0;

		// Token: 0x040021E9 RID: 8681
		private ProfilingModule.Stat[] _textStats = new ProfilingModule.Stat[ProfilingModule._defaultLength];

		// Token: 0x040021EA RID: 8682
		private Batcher2D _batcher2d;

		// Token: 0x040021EB RID: 8683
		private uint _previousMusicSoundIndex = 0U;

		// Token: 0x040021EC RID: 8684
		private uint _previousSoundEffectIndex = 0U;

		// Token: 0x040021ED RID: 8685
		private readonly Process _process;

		// Token: 0x040021EE RID: 8686
		private const int GraphHeight = 150;

		// Token: 0x040021EF RID: 8687
		private const int HistoryDuration = 350;

		// Token: 0x040021F0 RID: 8688
		private const float FpsGraphMaxValue = 66.666664f;

		// Token: 0x040021F1 RID: 8689
		private const float NetworkGraphMaxValue = 400f;

		// Token: 0x040021F2 RID: 8690
		private const float GarbageCollectionMaxValue = 500f;

		// Token: 0x040021F3 RID: 8691
		private Graph.DataSet _fpsData = new Graph.DataSet(350);

		// Token: 0x040021F4 RID: 8692
		private Graph.DataSet _cpuData = new Graph.DataSet(350);

		// Token: 0x040021F5 RID: 8693
		private Graph.DataSet _networkDataSent = new Graph.DataSet(350);

		// Token: 0x040021F6 RID: 8694
		private Graph.DataSet _networkDataReceived = new Graph.DataSet(350);

		// Token: 0x040021F7 RID: 8695
		private Graph.DataSet _garbageCollectionData = new Graph.DataSet(350);

		// Token: 0x040021F8 RID: 8696
		private Graph.DataSet _processMemoryData = new Graph.DataSet(350);

		// Token: 0x040021F9 RID: 8697
		private Graph _fpsGraph;

		// Token: 0x040021FA RID: 8698
		private Graph _cpuGraph;

		// Token: 0x040021FB RID: 8699
		private Graph _networkSentGraph;

		// Token: 0x040021FC RID: 8700
		private Graph _networkReceivedGraph;

		// Token: 0x040021FD RID: 8701
		private Graph _garbageCollectionGraph;

		// Token: 0x040021FE RID: 8702
		private Graph _processMemoryGraph;

		// Token: 0x040021FF RID: 8703
		private float _currentScale;

		// Token: 0x02000DC7 RID: 3527
		private enum Keys
		{
			// Token: 0x040043CA RID: 17354
			Branch,
			// Token: 0x040043CB RID: 17355
			Revision,
			// Token: 0x040043CC RID: 17356
			Vendor,
			// Token: 0x040043CD RID: 17357
			Renderer,
			// Token: 0x040043CE RID: 17358
			Version,
			// Token: 0x040043CF RID: 17359
			Vram,
			// Token: 0x040043D0 RID: 17360
			FPS,
			// Token: 0x040043D1 RID: 17361
			Resolution,
			// Token: 0x040043D2 RID: 17362
			DrawCallsCountAndTriangles,
			// Token: 0x040043D3 RID: 17363
			ShadowDrawCallsCountAndTriangles,
			// Token: 0x040043D4 RID: 17364
			AtlasSizes,
			// Token: 0x040043D5 RID: 17365
			ViewDistance,
			// Token: 0x040043D6 RID: 17366
			ChunkColumns,
			// Token: 0x040043D7 RID: 17367
			Entities,
			// Token: 0x040043D8 RID: 17368
			LoadedParticles,
			// Token: 0x040043D9 RID: 17369
			LoadedTrails,
			// Token: 0x040043DA RID: 17370
			LoadedImmersiveScreens,
			// Token: 0x040043DB RID: 17371
			SentPacketLength,
			// Token: 0x040043DC RID: 17372
			ReceivedPacketLength,
			// Token: 0x040043DD RID: 17373
			AssetCount,
			// Token: 0x040043DE RID: 17374
			AudioPlaybacksActive,
			// Token: 0x040043DF RID: 17375
			HeightmapAndTint,
			// Token: 0x040043E0 RID: 17376
			Light,
			// Token: 0x040043E1 RID: 17377
			Biome,
			// Token: 0x040043E2 RID: 17378
			Environment,
			// Token: 0x040043E3 RID: 17379
			Weather,
			// Token: 0x040043E4 RID: 17380
			Music,
			// Token: 0x040043E5 RID: 17381
			SoundEffect,
			// Token: 0x040043E6 RID: 17382
			ChunkPosition,
			// Token: 0x040043E7 RID: 17383
			FeetPosition,
			// Token: 0x040043E8 RID: 17384
			Collision,
			// Token: 0x040043E9 RID: 17385
			Orientation,
			// Token: 0x040043EA RID: 17386
			TargetBlock,
			// Token: 0x040043EB RID: 17387
			LastMoveForce,
			// Token: 0x040043EC RID: 17388
			WishDirection,
			// Token: 0x040043ED RID: 17389
			Disposables
		}

		// Token: 0x02000DC8 RID: 3528
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct Stat
		{
			// Token: 0x040043EE RID: 17390
			public string Text;

			// Token: 0x040043EF RID: 17391
			public Vector2 Position;

			// Token: 0x040043F0 RID: 17392
			public UInt32Color Color;
		}
	}
}
