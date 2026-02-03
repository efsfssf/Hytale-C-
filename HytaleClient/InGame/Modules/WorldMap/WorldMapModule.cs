using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Fonts;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using NLog;
using SDL2;

namespace HytaleClient.InGame.Modules.WorldMap
{
	// Token: 0x020008FE RID: 2302
	internal class WorldMapModule : Module
	{
		// Token: 0x17001135 RID: 4405
		// (get) Token: 0x060044EA RID: 17642 RVA: 0x000ECACD File Offset: 0x000EACCD
		// (set) Token: 0x060044EB RID: 17643 RVA: 0x000ECAD5 File Offset: 0x000EACD5
		public bool MapNeedsDrawing { get; private set; }

		// Token: 0x17001136 RID: 4406
		// (get) Token: 0x060044EC RID: 17644 RVA: 0x000ECADE File Offset: 0x000EACDE
		// (set) Token: 0x060044ED RID: 17645 RVA: 0x000ECAE6 File Offset: 0x000EACE6
		public bool IsWorldMapEnabled { get; private set; } = true;

		// Token: 0x17001137 RID: 4407
		// (get) Token: 0x060044EE RID: 17646 RVA: 0x000ECAEF File Offset: 0x000EACEF
		// (set) Token: 0x060044EF RID: 17647 RVA: 0x000ECAF7 File Offset: 0x000EACF7
		public bool AllowTeleportToCoordinates { get; private set; } = true;

		// Token: 0x17001138 RID: 4408
		// (get) Token: 0x060044F0 RID: 17648 RVA: 0x000ECB00 File Offset: 0x000EAD00
		// (set) Token: 0x060044F1 RID: 17649 RVA: 0x000ECB08 File Offset: 0x000EAD08
		public bool AllowTeleportToMarkers { get; private set; } = true;

		// Token: 0x17001139 RID: 4409
		// (get) Token: 0x060044F2 RID: 17650 RVA: 0x000ECB11 File Offset: 0x000EAD11
		private float _windowScale
		{
			get
			{
				return (float)this._gameInstance.Engine.Window.Viewport.Height / 1080f;
			}
		}

		// Token: 0x060044F3 RID: 17651 RVA: 0x000ECB34 File Offset: 0x000EAD34
		public WorldMapModule(GameInstance gameInstance) : base(gameInstance)
		{
			this._font = this._gameInstance.App.Fonts.DefaultFontFamily.RegularFont;
			BasicProgram basicProgram = this._gameInstance.Engine.Graphics.GPUProgramStore.BasicProgram;
			this._mapRenderer = new QuadRenderer(this._gameInstance.Engine.Graphics, basicProgram.AttribPosition, basicProgram.AttribTexCoords);
			this._textureAtlas = new HytaleClient.Graphics.Texture(HytaleClient.Graphics.Texture.TextureTypes.Texture2D);
			this._textureAtlas.CreateTexture2D(2048, 32, null, 0, GL.LINEAR_MIPMAP_LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
			GLFunctions gl = this._gameInstance.Engine.Graphics.GL;
			this._mapTexture = gl.GenTexture();
			gl.BindTexture(GL.TEXTURE_2D, this._mapTexture);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, 9728);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, 9728);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_S, 33071);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_T, 33071);
			this._maskTexture = gl.GenTexture();
			gl.BindTexture(GL.TEXTURE_2D, this._maskTexture);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, 9728);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, 9728);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_S, 33071);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_T, 33071);
			int width = this._gameInstance.Engine.Window.Viewport.Width;
			int height = this._gameInstance.Engine.Window.Viewport.Height;
			this._renderTarget = new RenderTarget(width, height, "WorldMap");
			this._renderTarget.AddTexture(RenderTarget.Target.Depth, GL.DEPTH24_STENCIL8, GL.DEPTH_STENCIL, GL.UNSIGNED_INT_24_8, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, false, false, 1);
			this._renderTarget.AddTexture(RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this._renderTarget.FinalizeSetup();
			this.Resize(width, height);
			this._updaterThreadCancellationTokenSource = new CancellationTokenSource();
			this._updaterThreadCancellationToken = this._updaterThreadCancellationTokenSource.Token;
			this._worldMapThread = new Thread(new ThreadStart(this.RunWorldMapThread))
			{
				Name = "WorldMap",
				IsBackground = true
			};
			this._worldMapThread.Start();
		}

		// Token: 0x060044F4 RID: 17652 RVA: 0x000ECFAC File Offset: 0x000EB1AC
		protected override void DoDispose()
		{
			this._mapRenderer.Dispose();
			this._textureAtlas.Dispose();
			this._renderTarget.Dispose();
			TextRenderer hoveredMarkerNameRenderer = this._hoveredMarkerNameRenderer;
			if (hoveredMarkerNameRenderer != null)
			{
				hoveredMarkerNameRenderer.Dispose();
			}
			this._gameInstance.Engine.Graphics.GL.DeleteTexture(this._mapTexture);
			this._gameInstance.Engine.Graphics.GL.DeleteTexture(this._maskTexture);
			foreach (WorldMapModule.Texture texture in this._textures.Values)
			{
				texture.QuadRenderer.Dispose();
			}
			this._updaterThreadCancellationTokenSource.Cancel();
			this._worldMapThread.Join();
			this._updaterThreadCancellationTokenSource.Dispose();
		}

		// Token: 0x060044F5 RID: 17653 RVA: 0x000ED0AC File Offset: 0x000EB2AC
		public void PrepareTextureAtlas(out Dictionary<string, WorldMapModule.Texture> upcomingTextures, out byte[][] upcomingAtlasPixelsPerLevel, CancellationToken cancellationToken)
		{
			upcomingTextures = new Dictionary<string, WorldMapModule.Texture>();
			List<Image> list = new List<Image>();
			Dictionary<Image, string> dictionary = new Dictionary<Image, string>();
			foreach (KeyValuePair<string, string> keyValuePair in this._gameInstance.HashesByServerAssetPath)
			{
				bool isCancellationRequested = cancellationToken.IsCancellationRequested;
				if (isCancellationRequested)
				{
					upcomingAtlasPixelsPerLevel = null;
					return;
				}
				string key = keyValuePair.Key;
				bool flag = !key.EndsWith(".png") || !key.StartsWith("UI/WorldMap/");
				if (!flag)
				{
					try
					{
						Image image = new Image(AssetManager.GetAssetUsingHash(keyValuePair.Value, false));
						list.Add(image);
						dictionary[image] = key;
					}
					catch (Exception exception)
					{
						WorldMapModule.Logger.Error(exception, "Failed to load worldmap texture: " + AssetManager.GetAssetLocalPathUsingHash(keyValuePair.Value));
					}
				}
			}
			list.Sort((Image a, Image b) => b.Height.CompareTo(a.Height));
			Dictionary<Image, Point> dictionary2;
			byte[] atlasPixels = Image.Pack(this._textureAtlas.Width, list, out dictionary2, false, cancellationToken);
			bool flag2 = dictionary2 == null;
			if (flag2)
			{
				upcomingAtlasPixelsPerLevel = null;
			}
			else
			{
				foreach (KeyValuePair<Image, Point> keyValuePair2 in dictionary2)
				{
					Point value = keyValuePair2.Value;
					Image key2 = keyValuePair2.Key;
					string text = dictionary[key2];
					WorldMapModule.Texture texture = new WorldMapModule.Texture(new Rectangle(value.X, value.Y, key2.Width, key2.Height));
					upcomingTextures[text] = texture;
					bool flag3 = text.StartsWith("UI/WorldMap/MapMarkers/");
					if (flag3)
					{
						texture.GenerateHitDetectionMap(key2.Pixels);
					}
				}
				upcomingAtlasPixelsPerLevel = HytaleClient.Graphics.Texture.BuildMipmapPixels(atlasPixels, this._textureAtlas.Width, this._textureAtlas.MipmapLevelCount);
			}
		}

		// Token: 0x060044F6 RID: 17654 RVA: 0x000ED2E4 File Offset: 0x000EB4E4
		public void BuildTextureAtlas(Dictionary<string, WorldMapModule.Texture> textures, byte[][] atlasPixelsPerLevel)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			foreach (string key in this._markers.Keys)
			{
				WorldMapModule.MapMarker mapMarker = this._markers[key];
				bool flag = textures.ContainsKey("UI/WorldMap/MapMarkers/" + mapMarker.MarkerImage);
				if (!flag)
				{
					this._gameInstance.App.DevTools.Error("World map marker type image '" + mapMarker.MarkerImage + "' was removed!");
				}
			}
			this._textureAtlas.UpdateTexture2DMipMaps(atlasPixelsPerLevel);
			bool flag2 = this._textures != null;
			if (flag2)
			{
				foreach (WorldMapModule.Texture texture in this._textures.Values)
				{
					texture.QuadRenderer.Dispose();
				}
			}
			this._textures = textures;
			foreach (WorldMapModule.Texture texture2 in textures.Values)
			{
				texture2.Init(this._gameInstance.Engine.Graphics, this._textureAtlas);
			}
			bool flag3 = !this._textures.TryGetValue("UI/WorldMap/MapMarkers/Player.png", out this._playerMarkerTexture);
			if (flag3)
			{
				this._playerMarkerTexture = new WorldMapModule.Texture(default(Rectangle));
				this._playerMarkerTexture.Init(this._gameInstance.Engine.Graphics, this._textureAtlas);
				WorldMapModule.Logger.Error("No player map marker texture has been provided (\"UI/WorldMap/MapMarkers/Player.png\").");
			}
			bool flag4 = !this._textures.TryGetValue("UI/WorldMap/MapMarkers/Coordinate.png", out this._coordinateMarkerTexture);
			if (flag4)
			{
				this._coordinateMarkerTexture = new WorldMapModule.Texture(default(Rectangle));
				this._coordinateMarkerTexture.Init(this._gameInstance.Engine.Graphics, this._textureAtlas);
				WorldMapModule.Logger.Error("No coordinate map marker texture has been provided (\"UI/WorldMap/MapMarkers/Coordinate.png\").");
			}
			bool flag5 = !this._textures.TryGetValue("UI/WorldMap/LocationHighlightAnimation.png", out this._highlightAnimationTexture);
			if (flag5)
			{
				this._highlightAnimationTexture = new WorldMapModule.Texture(default(Rectangle));
				this._highlightAnimationTexture.Init(this._gameInstance.Engine.Graphics, this._textureAtlas);
				WorldMapModule.Logger.Error("No location highlight animation texture has been provided (\"UI/WorldMap/LocationHighlightAnimation.png\").");
			}
			this.UpdateMaskTexture();
		}

		// Token: 0x060044F7 RID: 17655 RVA: 0x000ED5B0 File Offset: 0x000EB7B0
		public void Resize(int width, int height)
		{
			this._projectionMatrix = Matrix.CreateTranslation(0f, 0f, -1f) * Matrix.CreateOrthographicOffCenter((float)(-(float)width) / 2f, (float)width / 2f, (float)(-(float)height) / 2f, (float)height / 2f, 0.1f, 1000f);
			this._renderTarget.Resize(width, height, false);
			this.ClampOffset(false);
		}

		// Token: 0x060044F8 RID: 17656 RVA: 0x000ED628 File Offset: 0x000EB828
		public void SetVisible(bool visible)
		{
			this.MapNeedsDrawing = visible;
			this._contextMenuMarker = WorldMapModule.MarkerSelection.None;
			this._hoveredMarker = WorldMapModule.MarkerSelection.None;
			TextRenderer hoveredMarkerNameRenderer = this._hoveredMarkerNameRenderer;
			if (hoveredMarkerNameRenderer != null)
			{
				hoveredMarkerNameRenderer.Dispose();
			}
			this._hoveredMarkerNameRenderer = null;
			this._highlightAnimationFrame = 0f;
			if (visible)
			{
				this.CenterMapOnPlayer();
			}
			this._gameInstance.Connection.SendPacket(new UpdateWorldMapVisible(this.MapNeedsDrawing));
		}

		// Token: 0x060044F9 RID: 17657 RVA: 0x000ED6A0 File Offset: 0x000EB8A0
		private void CenterMapOnPlayer()
		{
			this._offsetChunksX = this._gameInstance.LocalPlayer.Position.X / 32f;
			this._offsetChunksZ = this._gameInstance.LocalPlayer.Position.Z / 32f;
			this.ClampOffset(false);
		}

		// Token: 0x060044FA RID: 17658 RVA: 0x000ED6F8 File Offset: 0x000EB8F8
		public bool TryGetBiomeAtPosition(Vector3 position, out WorldMapModule.ClientBiomeData biomeData)
		{
			biomeData = null;
			int num = (int)position.X;
			int num2 = (int)position.Z;
			UpdateWorldMap.Chunk.Image image;
			bool flag = this._images.TryGetValue(ChunkHelper.IndexOfChunkColumn(num >> 5, num2 >> 5), out image);
			bool result;
			if (flag)
			{
				int num3 = num & 31;
				int num4 = num2 & 31;
				int x = (int)((float)image.Width * ((float)num3 / 32f));
				int z = (int)((float)image.Height * ((float)num4 / 32f));
				int num5 = this.IndexPixel(x, z, image.Width, image.Height);
				int chunkPixel = image.PixelToBiomeId[num5];
				ushort key = this.PixelToBiomeId(chunkPixel);
				result = this._biomes.TryGetValue(key, out biomeData);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060044FB RID: 17659 RVA: 0x000ED7B0 File Offset: 0x000EB9B0
		public void UpdateMapSettings(Dictionary<short, BiomeData> biomes, bool isEnabled, bool allowTeleportToCoordinates, bool allowTeleportToMarkers)
		{
			Debug.Assert(ThreadHelper.IsOnThread(this._worldMapThread));
			this._gameInstance.Engine.RunOnMainThread(this, delegate
			{
				this.IsWorldMapEnabled = isEnabled;
				this.AllowTeleportToCoordinates = allowTeleportToCoordinates;
				this.AllowTeleportToMarkers = allowTeleportToMarkers;
				this._gameInstance.App.Interface.InGameView.InputBindingsComponent.OnWorldMapSettingsUpdated();
			}, false, false);
			this._biomes.Clear();
			foreach (KeyValuePair<short, BiomeData> keyValuePair in biomes)
			{
				int biomeColor = keyValuePair.Value.BiomeColor;
				this._biomes[(ushort)keyValuePair.Key] = new WorldMapModule.ClientBiomeData
				{
					ZoneId = keyValuePair.Value.ZoneId,
					ZoneName = keyValuePair.Value.ZoneName,
					BiomeName = keyValuePair.Value.BiomeName,
					Color = new byte[]
					{
						(byte)(biomeColor >> 16 & 255),
						(byte)(biomeColor >> 8 & 255),
						(byte)(biomeColor & 255)
					}
				};
			}
			this._imagesToUpdate.UnionWith(this._images.Keys);
			this._mapTextureNeedsUpdate = true;
		}

		// Token: 0x060044FC RID: 17660 RVA: 0x000ED918 File Offset: 0x000EBB18
		public void SetMapChunk(int chunkX, int chunkZ, UpdateWorldMap.Chunk.Image image)
		{
			Debug.Assert(ThreadHelper.IsOnThread(this._worldMapThread));
			long num = ChunkHelper.IndexOfChunkColumn(chunkX, chunkZ);
			bool flag = image != null;
			if (flag)
			{
				this._images[num] = image;
			}
			else
			{
				this._images.Remove(num);
			}
			this._imagesToUpdate.Add(num);
			this._imagesToUpdate.Add(ChunkHelper.IndexOfChunkColumn(chunkX - 1, chunkZ));
			this._imagesToUpdate.Add(ChunkHelper.IndexOfChunkColumn(chunkX, chunkZ - 1));
			this._mapTextureNeedsUpdate = true;
		}

		// Token: 0x060044FD RID: 17661 RVA: 0x000ED9A4 File Offset: 0x000EBBA4
		public void AddMapMarker(WorldMapModule.MapMarker mapMarker)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			bool flag = !this._textures.ContainsKey("UI/WorldMap/MapMarkers/" + mapMarker.MarkerImage);
			if (flag)
			{
				this._gameInstance.App.DevTools.Error("World map marker type image '" + mapMarker.MarkerImage + "' doesn't exist!");
			}
			WorldMapModule.MapMarker mapMarker2;
			bool flag2 = this._markers.TryGetValue(mapMarker.Id, out mapMarker2);
			if (flag2)
			{
				mapMarker.LerpX = mapMarker2.LerpX;
				mapMarker.LerpZ = mapMarker2.LerpZ;
				mapMarker.LerpYaw = mapMarker2.LerpYaw;
				this._markers[mapMarker.Id] = mapMarker;
				this._gameInstance.App.Interface.InGameView.CompassComponent.OnWorldMapMarkerUpdated(mapMarker);
			}
			else
			{
				mapMarker.LerpX = mapMarker.X;
				mapMarker.LerpZ = mapMarker.Z;
				mapMarker.LerpYaw = mapMarker.Yaw;
				this._markers[mapMarker.Id] = mapMarker;
				this._gameInstance.App.Interface.InGameView.CompassComponent.OnWorldMapMarkerAdded(mapMarker);
			}
		}

		// Token: 0x060044FE RID: 17662 RVA: 0x000EDADC File Offset: 0x000EBCDC
		public void RemoveMapMarker(string[] ids)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			foreach (string key in ids)
			{
				WorldMapModule.MapMarker mapMarker;
				this._markers.TryGetValue(key, out mapMarker);
				bool flag = mapMarker == this._selectedMarker.MapMarker;
				if (flag)
				{
					this._selectedMarker = WorldMapModule.MarkerSelection.None;
				}
				this._markers.Remove(key);
				this._gameInstance.App.Interface.InGameView.CompassComponent.OnWorldMapMarkerRemoved(mapMarker);
			}
		}

		// Token: 0x060044FF RID: 17663 RVA: 0x000EDB68 File Offset: 0x000EBD68
		public unsafe void UpdateMaskTexture()
		{
			string hash;
			bool flag = this._gameInstance.HashesByServerAssetPath.TryGetValue("UI/WorldMap/MapMask.png", ref hash);
			if (flag)
			{
				try
				{
					Image image = new Image(AssetManager.GetAssetUsingHash(hash, false));
					GLFunctions gl = this._gameInstance.Engine.Graphics.GL;
					gl.BindTexture(GL.TEXTURE_2D, this._maskTexture);
					try
					{
						byte[] array;
						byte* value;
						if ((array = image.Pixels) == null || array.Length == 0)
						{
							value = null;
						}
						else
						{
							value = &array[0];
						}
						gl.TexImage2D(GL.TEXTURE_2D, 0, 6408, image.Width, image.Height, 0, GL.RGBA, GL.UNSIGNED_BYTE, (IntPtr)((void*)value));
					}
					finally
					{
						byte[] array = null;
					}
				}
				catch (Exception exception)
				{
					WorldMapModule.Logger.Error(exception, "Failed to load worldmap mask texture: UI/WorldMap/MapMask.png");
				}
			}
			else
			{
				WorldMapModule.Logger.Error("Asset doesn't exist: UI/WorldMap/MapMask.png");
			}
		}

		// Token: 0x06004500 RID: 17664 RVA: 0x000EDC78 File Offset: 0x000EBE78
		private unsafe void SendDataToGPU()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			Debug.Assert(this._image.Width > 0 && this._image.Height > 0);
			Debug.Assert(this._mapTexture != GLTexture.None);
			GLFunctions gl = this._gameInstance.Engine.Graphics.GL;
			gl.BindTexture(GL.TEXTURE_2D, this._mapTexture);
			byte[] array;
			byte* value;
			if ((array = this._image.Pixels) == null || array.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array[0];
			}
			gl.TexImage2D(GL.TEXTURE_2D, 0, 6408, this._image.Width, this._image.Height, 0, GL.BGRA, GL.UNSIGNED_BYTE, (IntPtr)((void*)value));
			array = null;
		}

		// Token: 0x06004501 RID: 17665 RVA: 0x000EDD58 File Offset: 0x000EBF58
		public void Update(float deltaTime)
		{
			bool mapNeedsDrawing = this.MapNeedsDrawing;
			bool flag = this._mapTextureNeedsTransfer && mapNeedsDrawing;
			if (flag)
			{
				this.SendDataToGPU();
				this._mapTextureNeedsTransfer = false;
			}
			bool flag2 = this._mapTextureNeedsUpdate && !this._mapTextureIsUpdating && this.IsWorldMapEnabled;
			if (flag2)
			{
				this._mapTextureNeedsUpdate = false;
				this._mapTextureIsUpdating = true;
				this.RunOnWorldMapThread(delegate
				{
					HashSet<long> imagesBeingUpdated = this._imagesBeingUpdated;
					this._imagesBeingUpdated = this._imagesToUpdate;
					this._imagesToUpdate = imagesBeingUpdated;
					this._imagesToUpdate.Clear();
					this._tempImagesKeys.Clear();
					this._tempImagesKeys.AddRange(this._images.Keys);
					this.RunUpdateTextureTask(this._imagesBeingUpdated, this._tempImagesKeys);
				});
			}
			bool flag3 = !this.MapNeedsDrawing;
			if (!flag3)
			{
				bool flag4 = !this._isMovingOffset;
				if (flag4)
				{
					this.ClampOffset(true);
				}
				this.HandleKeyNavigation(deltaTime);
				this._highlightAnimationFrame += deltaTime * 15f;
				foreach (WorldMapModule.MapMarker mapMarker in this._markers.Values)
				{
					mapMarker.LerpX = MathHelper.Lerp(mapMarker.LerpX, mapMarker.X, deltaTime);
					mapMarker.LerpZ = MathHelper.Lerp(mapMarker.LerpZ, mapMarker.Z, deltaTime);
					bool flag5 = !float.IsNaN(mapMarker.Yaw);
					if (flag5)
					{
						mapMarker.LerpYaw = MathHelper.WrapAngle(MathHelper.LerpAngle(mapMarker.LerpYaw, mapMarker.Yaw, 2f * deltaTime));
					}
					else
					{
						mapMarker.LerpYaw = 0f;
					}
				}
			}
		}

		// Token: 0x06004502 RID: 17666 RVA: 0x000EDEE0 File Offset: 0x000EC0E0
		public void PrepareMapForDraw()
		{
			bool flag = !this.MapNeedsDrawing;
			if (flag)
			{
				throw new Exception("PrepareMapForDraw called when it was not required. Please check with MapNeedsDrawing first before calling this.");
			}
			int num = this._mapTextureMinChunkZ + this._mapTextureChunkHeight;
			float windowScale = this._windowScale;
			Matrix.CreateTranslation(((float)this._mapTextureMinChunkX - this._offsetChunksX) * this._scale * windowScale, ((float)(-(float)num) + this._offsetChunksZ) * this._scale * windowScale, 0f, out this._tempMatrix);
			Matrix.Multiply(ref this._tempMatrix, ref this._projectionMatrix, out this._mapMatrix);
			float xScale = (float)this._mapTextureChunkWidth * this._scale * windowScale;
			float yScale = (float)this._mapTextureChunkHeight * this._scale * windowScale;
			Matrix.CreateScale(xScale, yScale, 1f, out this._tempMatrix);
			Matrix.Multiply(ref this._tempMatrix, ref this._mapMatrix, out this._mapMatrix);
			int num2 = 2;
			num2 += this._markers.Count;
			bool flag2 = this._selectedMarker.Type == WorldMapModule.MarkerSelectionType.Coordinates;
			if (flag2)
			{
				num2 += 2;
			}
			else
			{
				bool flag3 = this._selectedMarker.Type == WorldMapModule.MarkerSelectionType.ServerMarker;
				if (flag3)
				{
					num2++;
				}
			}
			bool flag4 = this._contextMenuMarker.Type == WorldMapModule.MarkerSelectionType.Coordinates && !this._contextMenuMarker.Equals(this._selectedMarker);
			if (flag4)
			{
				num2++;
			}
			bool flag5 = num2 >= this._mapMarkerDrawTasks.Length;
			if (flag5)
			{
				Array.Resize<WorldMapModule.MarkerDrawTask>(ref this._mapMarkerDrawTasks, num2 + 5);
			}
			this._mapMarkerDrawTaskCount = 0;
			foreach (WorldMapModule.MapMarker mapMarker in this._markers.Values)
			{
				WorldMapModule.Texture marker;
				bool flag6 = !this._textures.TryGetValue("UI/WorldMap/MapMarkers/" + mapMarker.MarkerImage, out marker);
				if (!flag6)
				{
					bool flag7 = mapMarker == this._selectedMarker.MapMarker;
					bool flag8 = flag7;
					if (flag8)
					{
						this.PrepareHighlightAnimationForDraw(this._mapMarkerDrawTaskCount, mapMarker.LerpX, mapMarker.LerpZ);
						this._mapMarkerDrawTaskCount++;
					}
					this.PrepareMarkerForDraw(this._mapMarkerDrawTaskCount, marker, mapMarker.LerpX, mapMarker.LerpZ, float.IsNaN(mapMarker.LerpYaw) ? 0f : mapMarker.LerpYaw, mapMarker == this._hoveredMarker.MapMarker, flag7, false);
					this._mapMarkerDrawTaskCount++;
				}
			}
			bool flag9 = this._selectedMarker.Type == WorldMapModule.MarkerSelectionType.Coordinates;
			if (flag9)
			{
				this.PrepareHighlightAnimationForDraw(this._mapMarkerDrawTaskCount, (float)this._selectedMarker.Coordinates.X, (float)this._selectedMarker.Coordinates.Y);
				this._mapMarkerDrawTaskCount++;
				this.PrepareMarkerForDraw(this._mapMarkerDrawTaskCount, this._coordinateMarkerTexture, (float)this._selectedMarker.Coordinates.X, (float)this._selectedMarker.Coordinates.Y, 0f, this._hoveredMarker.Type == WorldMapModule.MarkerSelectionType.Coordinates, true, false);
				this._mapMarkerDrawTaskCount++;
			}
			bool flag10 = this._contextMenuMarker.Type == WorldMapModule.MarkerSelectionType.Coordinates && !this._contextMenuMarker.Equals(this._selectedMarker);
			if (flag10)
			{
				this.PrepareMarkerForDraw(this._mapMarkerDrawTaskCount, this._coordinateMarkerTexture, (float)this._contextMenuMarker.Coordinates.X, (float)this._contextMenuMarker.Coordinates.Y, 0f, this._hoveredMarker.Type == WorldMapModule.MarkerSelectionType.Coordinates, false, false);
				this._mapMarkerDrawTaskCount++;
			}
			this.PrepareHighlightAnimationForDraw(this._mapMarkerDrawTaskCount, this._gameInstance.LocalPlayer.Position.X, this._gameInstance.LocalPlayer.Position.Z);
			this._mapMarkerDrawTaskCount++;
			this.PrepareMarkerForDraw(this._mapMarkerDrawTaskCount, this._playerMarkerTexture, this._gameInstance.LocalPlayer.Position.X, this._gameInstance.LocalPlayer.Position.Z, this._gameInstance.LocalPlayer.LookOrientation.Yaw, this._hoveredMarker.Type == WorldMapModule.MarkerSelectionType.LocalPlayer, this._selectedMarker.Type == WorldMapModule.MarkerSelectionType.LocalPlayer, true);
			this._mapMarkerDrawTaskCount++;
			bool flag11 = this._hoveredMarkerNameRenderer != null;
			if (flag11)
			{
				float num3 = 20f * windowScale;
				float scale = 14f / (float)this._gameInstance.App.Fonts.DefaultFontFamily.RegularFont.BaseSize * windowScale;
				Point worldPosition = this.GetWorldPosition(this._hoveredMarker);
				Matrix.CreateTranslation(-this._hoveredMarkerNameRenderer.GetHorizontalOffset(TextRenderer.TextAlignment.Center), -this._hoveredMarkerNameRenderer.GetVerticalOffset(TextRenderer.TextVerticalAlignment.Middle), 0f, out this._hoveredMarkerNameMatrix);
				Matrix.CreateScale(scale, out this._tempMatrix);
				Matrix.Multiply(ref this._hoveredMarkerNameMatrix, ref this._tempMatrix, out this._hoveredMarkerNameMatrix);
				Matrix.AddTranslation(ref this._hoveredMarkerNameMatrix, ((float)worldPosition.X / 32f - this._offsetChunksX) * this._scale * windowScale, ((float)(-(float)worldPosition.Y) / 32f + this._offsetChunksZ) * this._scale * windowScale - num3, 0f);
				Matrix.Multiply(ref this._hoveredMarkerNameMatrix, ref this._projectionMatrix, out this._hoveredMarkerNameMatrix);
			}
		}

		// Token: 0x06004503 RID: 17667 RVA: 0x000EE484 File Offset: 0x000EC684
		private void PrepareHighlightAnimationForDraw(int drawTaskIndex, float blockX, float blockZ)
		{
			float windowScale = this._windowScale;
			float scaledWidth = 128f * windowScale * 0.5f;
			float scaledHeight = 128f * windowScale * 0.5f;
			this.BuildMatrix(out this._mapMarkerDrawTasks[drawTaskIndex].MVPMatrix, blockX, blockZ, scaledWidth, scaledHeight, 0f);
			this._mapMarkerDrawTasks[drawTaskIndex].Color = this._gameInstance.Engine.Graphics.WhiteColor;
			int num = (int)this._highlightAnimationFrame % 10;
			float num2 = (float)this._highlightAnimationTexture.Rectangle.Width / 128f;
			float num3 = (float)this._highlightAnimationTexture.Rectangle.X + (float)num % num2 * 128f;
			float num4 = (float)this._highlightAnimationTexture.Rectangle.Y + (float)((int)Math.Floor((double)((float)num / num2))) * 128f;
			this._highlightAnimationTexture.QuadRenderer.UpdateUVs((num3 + 128f) / (float)this._textureAtlas.Width, (num4 + 128f) / (float)this._textureAtlas.Height, num3 / (float)this._textureAtlas.Width, num4 / (float)this._textureAtlas.Height);
			this._mapMarkerDrawTasks[drawTaskIndex].Renderer = this._highlightAnimationTexture.QuadRenderer;
		}

		// Token: 0x06004504 RID: 17668 RVA: 0x000EE5DC File Offset: 0x000EC7DC
		private void PrepareMarkerForDraw(int drawTaskIndex, WorldMapModule.Texture marker, float blockX, float blockZ, float yaw = 0f, bool hover = false, bool selected = false, bool localPlayer = false)
		{
			float windowScale = this._windowScale;
			float scaledWidth = (float)marker.Rectangle.Width * windowScale;
			float scaledHeight = (float)marker.Rectangle.Height * windowScale;
			this.BuildMatrix(out this._mapMarkerDrawTasks[drawTaskIndex].MVPMatrix, blockX, blockZ, scaledWidth, scaledHeight, yaw);
			if (localPlayer)
			{
				this._mapMarkerDrawTasks[drawTaskIndex].Color = this._localPlayerColor;
			}
			else if (selected)
			{
				this._mapMarkerDrawTasks[drawTaskIndex].Color = (hover ? this._selectionHoverColor : this._selectionColor);
			}
			else if (hover)
			{
				this._mapMarkerDrawTasks[drawTaskIndex].Color = this._hoverColor;
			}
			else
			{
				this._mapMarkerDrawTasks[drawTaskIndex].Color = this._gameInstance.Engine.Graphics.WhiteColor;
			}
			this._mapMarkerDrawTasks[drawTaskIndex].Renderer = marker.QuadRenderer;
		}

		// Token: 0x06004505 RID: 17669 RVA: 0x000EE6D8 File Offset: 0x000EC8D8
		private void BuildMatrix(out Matrix matrix, float blockX, float blockZ, float scaledWidth, float scaledHeight, float yaw = 0f)
		{
			float windowScale = this._windowScale;
			Matrix.CreateTranslation((blockX / 32f - this._offsetChunksX) * this._scale * windowScale, (-blockZ / 32f + this._offsetChunksZ) * this._scale * windowScale, 0f, out this._tempMatrix);
			Matrix.Multiply(ref this._tempMatrix, ref this._projectionMatrix, out matrix);
			bool flag = yaw != 0f;
			if (flag)
			{
				Matrix.CreateRotationZ(yaw, out this._tempMatrix);
				Matrix.Multiply(ref this._tempMatrix, ref matrix, out matrix);
			}
			Matrix.CreateTranslation(-(scaledWidth / 2f), -(scaledHeight / 2f), 0f, out this._tempMatrix);
			Matrix.Multiply(ref this._tempMatrix, ref matrix, out matrix);
			Matrix.CreateScale(scaledWidth, scaledHeight, 1f, out this._tempMatrix);
			Matrix.Multiply(ref this._tempMatrix, ref matrix, out matrix);
		}

		// Token: 0x06004506 RID: 17670 RVA: 0x000EE7C4 File Offset: 0x000EC9C4
		public void DrawMap()
		{
			bool flag = !this.MapNeedsDrawing;
			if (flag)
			{
				throw new Exception("DrawMap called when it was not required. Please check with MapNeedsDrawing first before calling this.");
			}
			GraphicsDevice graphics = this._gameInstance.Engine.Graphics;
			GLFunctions gl = graphics.GL;
			BasicProgram basicProgram = graphics.GPUProgramStore.BasicProgram;
			basicProgram.AssertInUse();
			gl.BindTexture(GL.TEXTURE_2D, graphics.WhitePixelTexture.GLTexture);
			basicProgram.MVPMatrix.SetValue(ref graphics.ScreenMatrix);
			basicProgram.Color.SetValue(graphics.BlackColor);
			basicProgram.Opacity.SetValue(0.5f);
			graphics.ScreenQuadRenderer.Draw();
			basicProgram.Opacity.SetValue(1f);
			this._renderTarget.Bind(true, true);
			gl.BindTexture(GL.TEXTURE_2D, this._mapTexture);
			basicProgram.MVPMatrix.SetValue(ref this._mapMatrix);
			basicProgram.Color.SetValue(graphics.WhiteColor);
			this._mapRenderer.Draw();
			gl.BindTexture(GL.TEXTURE_2D, this._textureAtlas.GLTexture);
			for (int i = 0; i < this._mapMarkerDrawTaskCount; i++)
			{
				basicProgram.MVPMatrix.SetValue(ref this._mapMarkerDrawTasks[i].MVPMatrix);
				basicProgram.Color.SetValue(this._mapMarkerDrawTasks[i].Color);
				this._mapMarkerDrawTasks[i].Renderer.Draw();
			}
			bool flag2 = this._hoveredMarkerNameRenderer != null;
			if (flag2)
			{
				TextProgram textProgram = graphics.GPUProgramStore.TextProgram;
				gl.UseProgram(textProgram);
				gl.BindTexture(GL.TEXTURE_2D, this._font.TextureAtlas.GLTexture);
				textProgram.FillThreshold.SetValue(0f);
				textProgram.FillBlurThreshold.SetValue(0.2f);
				textProgram.OutlineThreshold.SetValue(0f);
				textProgram.OutlineBlurThreshold.SetValue(0f);
				textProgram.OutlineOffset.SetValue(Vector2.Zero);
				textProgram.FogParams.SetValue(Vector4.Zero);
				textProgram.Opacity.SetValue(1f);
				textProgram.MVPMatrix.SetValue(ref this._hoveredMarkerNameMatrix);
				this._hoveredMarkerNameRenderer.Draw();
			}
			this._renderTarget.Unbind();
			RenderTarget.BindHardwareFramebuffer();
			WorldMapProgram worldMapProgram = graphics.GPUProgramStore.WorldMapProgram;
			gl.UseProgram(worldMapProgram);
			gl.ActiveTexture(GL.TEXTURE1);
			gl.BindTexture(GL.TEXTURE_2D, this._maskTexture);
			gl.ActiveTexture(GL.TEXTURE0);
			gl.BindTexture(GL.TEXTURE_2D, this._renderTarget.GetTexture(RenderTarget.Target.Color0));
			worldMapProgram.MVPMatrix.SetValue(ref graphics.ScreenMatrix);
			graphics.ScreenQuadRenderer.Draw();
		}

		// Token: 0x06004507 RID: 17671 RVA: 0x000EEAC4 File Offset: 0x000ECCC4
		private void MoveOffset(float x, float y)
		{
			bool flag = this._contextMenuMarker.Type > WorldMapModule.MarkerSelectionType.None;
			if (flag)
			{
				this.HideContextMenu();
			}
			float windowScale = this._windowScale;
			this._offsetChunksX += -x / (this._scale * windowScale);
			this._offsetChunksZ += -y / (this._scale * windowScale);
			this.ClampOffset(false);
		}

		// Token: 0x06004508 RID: 17672 RVA: 0x000EEB2C File Offset: 0x000ECD2C
		private void Zoom(float zoom)
		{
			bool flag = this._contextMenuMarker.Type > WorldMapModule.MarkerSelectionType.None;
			if (flag)
			{
				this.HideContextMenu();
			}
			this._scale += zoom;
			this._scale = MathHelper.Clamp(this._scale, 2f, 64f);
			this.ClampOffset(false);
		}

		// Token: 0x06004509 RID: 17673 RVA: 0x000EEB84 File Offset: 0x000ECD84
		private void ClampOffset(bool smooth = false)
		{
			bool flag = this._minChunkX >= this._maxChunkX || this._minChunkZ >= this._maxChunkZ;
			if (!flag)
			{
				float num = this._isMovingOffset ? -32f : 0f;
				float windowScale = this._windowScale;
				float num2 = this._gameInstance.LocalPlayer.Position.X / 32f;
				bool flag2 = num2 < (float)this._minChunkX || num2 > (float)this._maxChunkX;
				if (flag2)
				{
					this._offsetChunksX = num2;
				}
				else
				{
					int num3 = this._maxChunkX - this._minChunkX;
					float num4 = ((float)num3 + num) / (this._scale * windowScale);
					if (smooth)
					{
						float value = MathHelper.Clamp(this._offsetChunksX, (float)this._minChunkX + num4, (float)this._maxChunkX - num4);
						this._offsetChunksX = MathHelper.Lerp(this._offsetChunksX, value, 0.3f);
					}
					else
					{
						this._offsetChunksX = MathHelper.Clamp(this._offsetChunksX, (float)this._minChunkX + num4, (float)this._maxChunkX - num4);
					}
				}
				float num5 = this._gameInstance.LocalPlayer.Position.Z / 32f;
				bool flag3 = num5 < (float)this._minChunkZ || num5 > (float)this._maxChunkZ;
				if (flag3)
				{
					this._offsetChunksZ = num5;
				}
				else
				{
					int num6 = this._maxChunkZ - this._minChunkZ;
					float num7 = ((float)num6 + num) / (this._scale * this._windowScale);
					if (smooth)
					{
						float value2 = MathHelper.Clamp(this._offsetChunksZ, (float)this._minChunkZ + num7, (float)this._maxChunkZ - num7);
						this._offsetChunksZ = MathHelper.Lerp(this._offsetChunksZ, value2, 0.3f);
					}
					else
					{
						this._offsetChunksZ = MathHelper.Clamp(this._offsetChunksZ, (float)this._minChunkZ + num7, (float)this._maxChunkZ - num7);
					}
				}
			}
		}

		// Token: 0x0600450A RID: 17674 RVA: 0x000EED86 File Offset: 0x000ECF86
		private void HideContextMenu()
		{
			this._contextMenuMarker = WorldMapModule.MarkerSelection.None;
			this._gameInstance.App.Interface.TriggerEvent("worldMap.hideContextMenu", null, null, null, null, null, null);
		}

		// Token: 0x0600450B RID: 17675 RVA: 0x000EEDB8 File Offset: 0x000ECFB8
		private Point GetWorldPosition(WorldMapModule.MarkerSelection marker)
		{
			Point result;
			switch (marker.Type)
			{
			case WorldMapModule.MarkerSelectionType.LocalPlayer:
				result = new Point((int)Math.Floor((double)this._gameInstance.LocalPlayer.Position.X), (int)Math.Floor((double)this._gameInstance.LocalPlayer.Position.Z));
				break;
			case WorldMapModule.MarkerSelectionType.ServerMarker:
				result = new Point((int)Math.Floor((double)marker.MapMarker.X), (int)Math.Floor((double)marker.MapMarker.Z));
				break;
			case WorldMapModule.MarkerSelectionType.Coordinates:
				result = marker.Coordinates;
				break;
			default:
				result = Point.Zero;
				break;
			}
			return result;
		}

		// Token: 0x0600450C RID: 17676 RVA: 0x000EEE64 File Offset: 0x000ED064
		private Point ScreenToBlockPosition(int mouseX, int mouseY)
		{
			float windowScale = this._windowScale;
			return new Point((int)((float)mouseX / (this._scale * windowScale) * 32f + this._offsetChunksX * 32f), (int)((float)mouseY / (this._scale * windowScale) * 32f + this._offsetChunksZ * 32f));
		}

		// Token: 0x0600450D RID: 17677 RVA: 0x000EEEC0 File Offset: 0x000ED0C0
		private bool IsMouseAtWorldPosition(int screenX, int screenY, int blockX, int blockZ, WorldMapModule.Texture texture, float yaw = 0f)
		{
			float windowScale = this._windowScale;
			float num = ((float)blockX / 32f - this._offsetChunksX) * this._scale * windowScale;
			float num2 = ((float)blockZ / 32f - this._offsetChunksZ) * this._scale * windowScale;
			bool flag = yaw != 0f;
			if (flag)
			{
				MathHelper.RotateAroundPoint(ref screenX, ref screenY, yaw, (int)num, (int)num2);
			}
			float num3 = (float)texture.Rectangle.Width * windowScale;
			float num4 = (float)texture.Rectangle.Height * windowScale;
			Rectangle rectangle = new Rectangle((int)(num - num3 / 2f), (int)(num2 - num4 / 2f), (int)num3, (int)num4);
			bool flag2 = !rectangle.Contains(screenX, screenY);
			return !flag2 && texture.IsPointOpaque((int)((float)(screenX - rectangle.X) / windowScale), (int)((float)(screenY - rectangle.Y) / windowScale));
		}

		// Token: 0x0600450E RID: 17678 RVA: 0x000EEFAC File Offset: 0x000ED1AC
		private WorldMapModule.MarkerSelection GetMarkerAtMousePosition(int screenX, int screenY)
		{
			bool flag = this.IsMouseAtWorldPosition(screenX, screenY, (int)this._gameInstance.LocalPlayer.Position.X, (int)this._gameInstance.LocalPlayer.Position.Z, this._playerMarkerTexture, this._gameInstance.LocalPlayer.LookOrientation.Yaw);
			WorldMapModule.MarkerSelection result;
			if (flag)
			{
				result = new WorldMapModule.MarkerSelection
				{
					Type = WorldMapModule.MarkerSelectionType.LocalPlayer
				};
			}
			else
			{
				bool flag2 = this._selectedMarker.Type == WorldMapModule.MarkerSelectionType.Coordinates;
				if (flag2)
				{
					bool flag3 = this.IsMouseAtWorldPosition(screenX, screenY, this._selectedMarker.Coordinates.X, this._selectedMarker.Coordinates.Y, this._coordinateMarkerTexture, 0f);
					if (flag3)
					{
						return this._selectedMarker;
					}
				}
				WorldMapModule.MapMarker mapMarker = null;
				foreach (WorldMapModule.MapMarker mapMarker2 in this._markers.Values)
				{
					WorldMapModule.Texture texture;
					bool flag4 = !this._textures.TryGetValue("UI/WorldMap/MapMarkers/" + mapMarker2.MarkerImage, out texture);
					if (!flag4)
					{
						bool flag5 = this.IsMouseAtWorldPosition(screenX, screenY, (int)Math.Floor((double)mapMarker2.LerpX), (int)Math.Floor((double)mapMarker2.LerpZ), texture, mapMarker2.LerpYaw);
						if (flag5)
						{
							mapMarker = mapMarker2;
						}
					}
				}
				bool flag6 = mapMarker != null;
				if (flag6)
				{
					result = new WorldMapModule.MarkerSelection
					{
						Type = WorldMapModule.MarkerSelectionType.ServerMarker,
						MapMarker = mapMarker
					};
				}
				else
				{
					result = WorldMapModule.MarkerSelection.None;
				}
			}
			return result;
		}

		// Token: 0x0600450F RID: 17679 RVA: 0x000EF160 File Offset: 0x000ED360
		private void SetHighlightedBiome(ushort biomeId)
		{
			Debug.Assert(ThreadHelper.IsOnThread(this._worldMapThread));
			bool flag = this._hoveredBiomeId == biomeId;
			if (!flag)
			{
				this._hoveredBiomeId = biomeId;
				WorldMapModule.ClientBiomeData biome;
				bool flag2 = biomeId != ushort.MaxValue && this._biomes.TryGetValue(biomeId, out biome);
				if (flag2)
				{
					this._gameInstance.Engine.RunOnMainThread(this, delegate
					{
						this._gameInstance.App.Interface.TriggerEvent("worldMap.setHighlightedBiome", biome.ZoneName, biome.BiomeName, null, null, null, null);
					}, false, false);
				}
				else
				{
					bool flag3 = biomeId != ushort.MaxValue;
					if (flag3)
					{
						this._gameInstance.Engine.RunOnMainThread(this, delegate
						{
							this._gameInstance.App.Interface.TriggerEvent("worldMap.setHighlightedBiome", null, null, null, null, null, null);
						}, false, false);
					}
					else
					{
						this._gameInstance.Engine.RunOnMainThread(this, delegate
						{
							this._gameInstance.App.Interface.TriggerEvent("worldMap.setHighlightedBiome", "Error: Unknown Zone!!", "Error: Unknown Biome!!", null, null, null, null);
						}, false, false);
					}
				}
			}
		}

		// Token: 0x06004510 RID: 17680 RVA: 0x000EF244 File Offset: 0x000ED444
		public void OnSelectContextMarker()
		{
			bool flag = this._contextMenuMarker.Type == WorldMapModule.MarkerSelectionType.None;
			if (!flag)
			{
				this._selectedMarker = this._contextMenuMarker;
				this.HideContextMenu();
			}
		}

		// Token: 0x06004511 RID: 17681 RVA: 0x000EF27C File Offset: 0x000ED47C
		public void OnDeselectContextMarker()
		{
			bool flag = !this._contextMenuMarker.Equals(this._selectedMarker);
			if (!flag)
			{
				this._selectedMarker = WorldMapModule.MarkerSelection.None;
				this.HideContextMenu();
			}
		}

		// Token: 0x06004512 RID: 17682 RVA: 0x000EF2B8 File Offset: 0x000ED4B8
		public void OnTeleportToContextMarker()
		{
			WorldMapModule.MarkerSelectionType type = this._contextMenuMarker.Type;
			WorldMapModule.MarkerSelectionType markerSelectionType = type;
			if (markerSelectionType != WorldMapModule.MarkerSelectionType.ServerMarker)
			{
				if (markerSelectionType != WorldMapModule.MarkerSelectionType.Coordinates)
				{
					this._gameInstance.Chat.Error(string.Format("Teleport is not supported for selected marker type {0}", this._contextMenuMarker.Type));
					return;
				}
				this._gameInstance.Chat.SendCommand("tp", new object[]
				{
					this._contextMenuMarker.Coordinates.X,
					"_2",
					this._contextMenuMarker.Coordinates.Y
				});
			}
			else
			{
				this._gameInstance.Connection.SendPacket(new TeleportToWorldMapMarker(this._contextMenuMarker.MapMarker.Id));
			}
			this.HideContextMenu();
		}

		// Token: 0x06004513 RID: 17683 RVA: 0x000EF394 File Offset: 0x000ED594
		private void HandleKeyNavigation(float dt)
		{
			bool flag = this._gameInstance.Input.IsKeyHeld(SDL.SDL_Scancode.SDL_SCANCODE_DOWN, false);
			if (flag)
			{
				this.MoveOffset(0f, dt * -100f);
			}
			else
			{
				bool flag2 = this._gameInstance.Input.IsKeyHeld(SDL.SDL_Scancode.SDL_SCANCODE_UP, false);
				if (flag2)
				{
					this.MoveOffset(0f, dt * 100f);
				}
			}
			bool flag3 = this._gameInstance.Input.IsKeyHeld(SDL.SDL_Scancode.SDL_SCANCODE_LEFT, false);
			if (flag3)
			{
				this.MoveOffset(dt * 100f, 0f);
			}
			else
			{
				bool flag4 = this._gameInstance.Input.IsKeyHeld(SDL.SDL_Scancode.SDL_SCANCODE_RIGHT, false);
				if (flag4)
				{
					this.MoveOffset(dt * -100f, 0f);
				}
			}
		}

		// Token: 0x06004514 RID: 17684 RVA: 0x000EF458 File Offset: 0x000ED658
		public void OnUserInput(SDL.SDL_Event evt)
		{
			SDL.SDL_EventType type = evt.type;
			SDL.SDL_EventType sdl_EventType = type;
			if (sdl_EventType != SDL.SDL_EventType.SDL_KEYDOWN)
			{
				switch (sdl_EventType)
				{
				case SDL.SDL_EventType.SDL_MOUSEMOTION:
				{
					Window window = this._gameInstance.Engine.Window;
					Point point = window.TransformSDLToViewportCoords(evt.motion.x, evt.motion.y);
					float num = (float)window.Viewport.Width / 2f;
					float num2 = (float)window.Viewport.Height / 2f;
					int num3 = (int)((float)point.X - num);
					int num4 = (int)((float)point.Y - num2);
					int num5 = point.X - this._previousMouseX;
					int num6 = point.Y - this._previousMouseY;
					bool flag = this._isMovingOffset && this._contextMenuMarker.Type > WorldMapModule.MarkerSelectionType.None;
					if (flag)
					{
						bool flag2 = Math.Abs(num5) > 2 || Math.Abs(num5) > 2;
						if (flag2)
						{
							this.MoveOffset((float)num5, (float)num6);
						}
					}
					else
					{
						bool isMovingOffset = this._isMovingOffset;
						if (isMovingOffset)
						{
							this.MoveOffset((float)num5, (float)num6);
						}
						else
						{
							Point blockPosition = this.ScreenToBlockPosition(num3, num4);
							this.RunOnWorldMapThread(delegate
							{
								UpdateWorldMap.Chunk.Image image;
								bool flag9 = this._images.TryGetValue(ChunkHelper.IndexOfChunkColumn(blockPosition.X >> 5, blockPosition.Y >> 5), out image);
								if (flag9)
								{
									int num11 = blockPosition.X & 31;
									int num12 = blockPosition.Y & 31;
									int x = (int)((float)image.Width * ((float)num11 / 32f));
									int z = (int)((float)image.Height * ((float)num12 / 32f));
									int num13 = this.IndexPixel(x, z, image.Width, image.Height);
									int chunkPixel = image.PixelToBiomeId[num13];
									ushort highlightedBiome = this.PixelToBiomeId(chunkPixel);
									this.SetHighlightedBiome(highlightedBiome);
								}
								else
								{
									this.SetHighlightedBiome(ushort.MaxValue);
								}
							});
						}
					}
					WorldMapModule.MarkerSelection markerAtMousePosition = this.GetMarkerAtMousePosition(num3, num4);
					bool flag3 = !this._hoveredMarker.Equals(markerAtMousePosition);
					if (flag3)
					{
						WorldMapModule.MarkerSelectionType type2 = markerAtMousePosition.Type;
						WorldMapModule.MarkerSelectionType markerSelectionType = type2;
						string text;
						if (markerSelectionType != WorldMapModule.MarkerSelectionType.LocalPlayer)
						{
							if (markerSelectionType != WorldMapModule.MarkerSelectionType.ServerMarker)
							{
								text = null;
							}
							else
							{
								text = markerAtMousePosition.MapMarker.Name;
							}
						}
						else
						{
							text = "You";
						}
						bool flag4 = text != null;
						if (flag4)
						{
							bool flag5 = this._hoveredMarkerNameRenderer != null;
							if (flag5)
							{
								this._hoveredMarkerNameRenderer.Text = text;
							}
							else
							{
								this._hoveredMarkerNameRenderer = new TextRenderer(this._gameInstance.Engine.Graphics, this._gameInstance.App.Fonts.DefaultFontFamily.RegularFont, text, uint.MaxValue, 4278190080U);
							}
						}
						else
						{
							TextRenderer hoveredMarkerNameRenderer = this._hoveredMarkerNameRenderer;
							if (hoveredMarkerNameRenderer != null)
							{
								hoveredMarkerNameRenderer.Dispose();
							}
							this._hoveredMarkerNameRenderer = null;
						}
					}
					this._hoveredMarker = markerAtMousePosition;
					this._previousMouseX = point.X;
					this._previousMouseY = point.Y;
					break;
				}
				case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
				{
					Input.MouseButton button = (Input.MouseButton)evt.button.button;
					Input.MouseButton mouseButton = button;
					if (mouseButton != Input.MouseButton.SDL_BUTTON_LEFT)
					{
						if (mouseButton == Input.MouseButton.SDL_BUTTON_RIGHT)
						{
							Window window2 = this._gameInstance.Engine.Window;
							Point point2 = window2.TransformSDLToViewportCoords(evt.button.x, evt.button.y);
							float num7 = (float)window2.Viewport.Width / 2f;
							float num8 = (float)window2.Viewport.Height / 2f;
							int num9 = (int)((float)point2.X - num7);
							int num10 = (int)((float)point2.Y - num8);
							WorldMapModule.MarkerSelection markerSelection = this.GetMarkerAtMousePosition(num9, num10);
							bool flag6 = markerSelection.Type == WorldMapModule.MarkerSelectionType.None;
							if (flag6)
							{
								markerSelection = new WorldMapModule.MarkerSelection
								{
									Type = WorldMapModule.MarkerSelectionType.Coordinates,
									Coordinates = this.ScreenToBlockPosition(num9, num10)
								};
								this._gameInstance.App.Interface.InGameView.MapPage.OnMarkerPlaced();
							}
							bool flag7 = markerSelection.Type != WorldMapModule.MarkerSelectionType.LocalPlayer;
							if (flag7)
							{
								this._contextMenuMarker = markerSelection;
								this._gameInstance.App.Interface.InGameView.MapPage.OnShowContextMenu(this._contextMenuMarker, this._selectedMarker.Equals(this._contextMenuMarker));
							}
						}
					}
					else
					{
						this._isMovingOffset = true;
						Point point3 = this._gameInstance.Engine.Window.TransformSDLToViewportCoords(evt.button.x, evt.button.y);
						this._previousMouseX = point3.X;
						this._previousMouseY = point3.Y;
					}
					break;
				}
				case SDL.SDL_EventType.SDL_MOUSEBUTTONUP:
					this._isMovingOffset = false;
					break;
				case SDL.SDL_EventType.SDL_MOUSEWHEEL:
				{
					bool flag8 = evt.wheel.y != 0;
					if (flag8)
					{
						this.Zoom((evt.wheel.y < 0) ? -1f : 1f);
					}
					break;
				}
				}
			}
			else
			{
				SDL.SDL_Keycode sym = evt.key.keysym.sym;
				SDL.SDL_Keycode sdl_Keycode = sym;
				if (sdl_Keycode != SDL.SDL_Keycode.SDLK_SPACE)
				{
					switch (sdl_Keycode)
					{
					case SDL.SDL_Keycode.SDLK_PLUS:
						this.Zoom(0.4f);
						break;
					case SDL.SDL_Keycode.SDLK_MINUS:
						this.Zoom(-0.4f);
						break;
					case SDL.SDL_Keycode.SDLK_1:
						this._maxHeightShading -= 0.25f;
						this.RunOnWorldMapThread(delegate
						{
							WorldMapModule.Logger.Info("MaxHeightShading: {0}", this._maxHeightShading);
							this._imagesToUpdate.UnionWith(this._images.Keys);
							this._mapTextureNeedsUpdate = true;
						});
						break;
					case SDL.SDL_Keycode.SDLK_2:
						this._maxHeightShading += 0.25f;
						this.RunOnWorldMapThread(delegate
						{
							WorldMapModule.Logger.Info("MaxHeightShading: {0}", this._maxHeightShading);
							this._imagesToUpdate.UnionWith(this._images.Keys);
							this._mapTextureNeedsUpdate = true;
						});
						break;
					case SDL.SDL_Keycode.SDLK_3:
						this._maxBorderShading -= 0.25f;
						this.RunOnWorldMapThread(delegate
						{
							WorldMapModule.Logger.Info("MaxBorderShading: {0}", this._maxBorderShading);
							this._imagesToUpdate.UnionWith(this._images.Keys);
							this._mapTextureNeedsUpdate = true;
						});
						break;
					case SDL.SDL_Keycode.SDLK_4:
						this._maxBorderShading += 0.25f;
						this.RunOnWorldMapThread(delegate
						{
							WorldMapModule.Logger.Info("MaxBorderShading: {0}", this._maxBorderShading);
							this._imagesToUpdate.UnionWith(this._images.Keys);
							this._mapTextureNeedsUpdate = true;
						});
						break;
					case SDL.SDL_Keycode.SDLK_5:
						this._maxBorderSize -= 0.25f;
						this.RunOnWorldMapThread(delegate
						{
							WorldMapModule.Logger.Info("MaxBorderize: {0}", this._maxBorderSize);
							this._imagesToUpdate.UnionWith(this._images.Keys);
							this._mapTextureNeedsUpdate = true;
						});
						break;
					case SDL.SDL_Keycode.SDLK_6:
						this._maxBorderSize += 0.25f;
						this.RunOnWorldMapThread(delegate
						{
							WorldMapModule.Logger.Info("MaxBorderSize: {0}", this._maxBorderSize);
							this._imagesToUpdate.UnionWith(this._images.Keys);
							this._mapTextureNeedsUpdate = true;
						});
						break;
					case SDL.SDL_Keycode.SDLK_7:
						this._maxBorderSaturation -= 0.05f;
						this.RunOnWorldMapThread(delegate
						{
							WorldMapModule.Logger.Info("MaxBorderSaturation: {0}", this._maxBorderSaturation);
							this._imagesToUpdate.UnionWith(this._images.Keys);
							this._mapTextureNeedsUpdate = true;
						});
						break;
					case SDL.SDL_Keycode.SDLK_8:
						this._maxBorderSaturation += 0.05f;
						this.RunOnWorldMapThread(delegate
						{
							WorldMapModule.Logger.Info("MaxBorderSaturation: {0}", this._maxBorderSaturation);
							this._imagesToUpdate.UnionWith(this._images.Keys);
							this._mapTextureNeedsUpdate = true;
						});
						break;
					}
				}
				else
				{
					this.CenterMapOnPlayer();
				}
			}
		}

		// Token: 0x06004515 RID: 17685 RVA: 0x000EFAB6 File Offset: 0x000EDCB6
		public void RunOnWorldMapThread(Action action)
		{
			Debug.Assert(!ThreadHelper.IsOnThread(this._worldMapThread));
			this._worldMapThreadActionQueue.Add(action, this._updaterThreadCancellationToken);
		}

		// Token: 0x06004516 RID: 17686 RVA: 0x000EFAE0 File Offset: 0x000EDCE0
		private void RunWorldMapThread()
		{
			while (!this._updaterThreadCancellationToken.IsCancellationRequested)
			{
				Action action;
				try
				{
					action = this._worldMapThreadActionQueue.Take(this._updaterThreadCancellationToken);
				}
				catch (OperationCanceledException)
				{
					break;
				}
				action();
			}
		}

		// Token: 0x06004517 RID: 17687 RVA: 0x000EFB38 File Offset: 0x000EDD38
		private void RunUpdateTextureTask(HashSet<long> imagesToUpdate, List<long> imagesKeys)
		{
			bool flag = this._images.Count == 0;
			if (flag)
			{
				this._mapTextureIsUpdating = false;
			}
			else
			{
				int minChunkX = int.MaxValue;
				int minChunkZ = int.MaxValue;
				int maxChunkX = int.MinValue;
				int maxChunkZ = int.MinValue;
				int chunkImageWidth = 0;
				int chunkImageHeight = 0;
				foreach (KeyValuePair<long, UpdateWorldMap.Chunk.Image> keyValuePair in this._images)
				{
					int num = ChunkHelper.XOfChunkColumnIndex(keyValuePair.Key);
					int num2 = ChunkHelper.ZOfChunkColumnIndex(keyValuePair.Key);
					bool flag2 = num < minChunkX;
					if (flag2)
					{
						minChunkX = num;
					}
					bool flag3 = num2 < minChunkZ;
					if (flag3)
					{
						minChunkZ = num2;
					}
					bool flag4 = num > maxChunkX;
					if (flag4)
					{
						maxChunkX = num;
					}
					bool flag5 = num2 > maxChunkZ;
					if (flag5)
					{
						maxChunkZ = num2;
					}
					int width = keyValuePair.Value.Width;
					int height = keyValuePair.Value.Height;
					bool flag6 = width > chunkImageWidth;
					if (flag6)
					{
						chunkImageWidth = width;
					}
					bool flag7 = height > chunkImageHeight;
					if (flag7)
					{
						chunkImageHeight = height;
					}
				}
				int num3 = maxChunkX - minChunkX + 1;
				int num4 = maxChunkZ - minChunkZ + 1;
				bool flag8 = num3 <= 0 && num4 <= 0;
				if (flag8)
				{
					WorldMapModule.Logger.Warn("No size!");
					this._mapTextureIsUpdating = false;
				}
				else
				{
					int num5 = this._mapTextureChunkWidth * this._mapChunkImageWidth;
					int num6 = this._mapTextureChunkHeight * this._mapChunkImageHeight;
					int num7 = this._mapTextureChunkWidth / 2 * this._mapChunkImageWidth;
					int num8 = this._mapTextureChunkHeight / 2 * this._mapChunkImageHeight;
					int textureChunkWidth = this._mapTextureChunkWidth;
					int textureChunkHeight = this._mapTextureChunkHeight;
					int textureMinChunkX = this._mapTextureMinChunkX;
					int textureMinChunkZ = this._mapTextureMinChunkZ;
					int textureCenterChunkX = this._mapTextureCenterChunkX;
					int textureCenterChunkZ = this._mapTextureCenterChunkZ;
					bool flag9 = minChunkX < this._mapTextureMinChunkX || minChunkZ < this._mapTextureMinChunkZ || maxChunkX + 1 >= this._mapTextureMinChunkX + this._mapTextureChunkWidth || maxChunkZ + 1 >= this._mapTextureMinChunkZ + this._mapTextureChunkHeight || this._mapChunkImageWidth != chunkImageWidth || this._mapChunkImageHeight != chunkImageHeight;
					if (flag9)
					{
						int num9 = (int)((float)num3 * 1.5f);
						int num10 = (int)((float)num4 * 1.5f);
						textureChunkWidth = num9 + num9 % 2;
						textureChunkHeight = num10 + num10 % 2;
						num5 = textureChunkWidth * chunkImageWidth;
						num6 = textureChunkHeight * chunkImageHeight;
						int maxTextureSize = this._gameInstance.Engine.Graphics.MaxTextureSize;
						bool flag10 = num5 > maxTextureSize || num6 > maxTextureSize;
						if (flag10)
						{
							WorldMapModule.Logger.Warn("Requested texture size is too big! WorldMapModule need to be designed w/ around that limitation!");
							this._mapTextureIsUpdating = false;
							return;
						}
						num7 = textureChunkWidth / 2 * chunkImageWidth;
						num8 = textureChunkHeight / 2 * chunkImageHeight;
						textureCenterChunkX = minChunkX + num3 / 2;
						textureCenterChunkZ = minChunkZ + num4 / 2;
						textureMinChunkX = textureCenterChunkX - textureChunkWidth / 2;
						textureMinChunkZ = textureCenterChunkZ - textureChunkHeight / 2;
						this._image = new Image(num5, num6, new byte[num5 * num6 * 4]);
						imagesToUpdate.UnionWith(imagesKeys);
					}
					foreach (long num11 in imagesToUpdate)
					{
						bool isCancellationRequested = this._updaterThreadCancellationToken.IsCancellationRequested;
						if (isCancellationRequested)
						{
							return;
						}
						int num12 = ChunkHelper.XOfChunkColumnIndex(num11);
						int num13 = ChunkHelper.ZOfChunkColumnIndex(num11);
						UpdateWorldMap.Chunk.Image image;
						bool flag11 = !this._images.TryGetValue(num11, out image);
						if (!flag11)
						{
							float num14 = (float)chunkImageWidth / (float)image.Width;
							float num15 = (float)chunkImageHeight / (float)image.Height;
							int num16 = num7 - (textureCenterChunkX - num12) * chunkImageWidth;
							int num17 = num8 - (textureCenterChunkZ - num13) * chunkImageHeight;
							for (int i = 0; i < image.Width; i++)
							{
								for (int j = 0; j < image.Height; j++)
								{
									int num18 = this.IndexPixel(i, j, image.Width, image.Height);
									int chunkPixel = image.PixelToBiomeId[num18];
									ushort num19 = this.PixelToBiomeId(chunkPixel);
									WorldMapModule.ClientBiomeData clientBiomeData;
									bool flag12 = num19 != ushort.MaxValue && this._biomes.TryGetValue(num19, out clientBiomeData);
									if (flag12)
									{
										float num20 = this.PixelHeight(chunkPixel);
										float num21 = this.PixelBorder(chunkPixel);
										float num22 = 0f;
										bool flag13 = i + 1 < image.Width;
										if (flag13)
										{
											int num23 = this.IndexPixel(i + 1, j, image.Width, image.Height);
											float num24 = this.PixelHeight(image.PixelToBiomeId[num23]);
											num22 += num24 - num20;
										}
										else
										{
											UpdateWorldMap.Chunk.Image image2;
											bool flag14 = this._images.TryGetValue(ChunkHelper.IndexOfChunkColumn(num12 + 1, num13), out image2);
											if (flag14)
											{
												int num25 = this.IndexPixel(0, j, image2.Width, image2.Height);
												float num26 = this.PixelHeight(image2.PixelToBiomeId[num25]);
												num22 += num26 - num20;
											}
										}
										bool flag15 = j + 1 < image.Height;
										if (flag15)
										{
											int num27 = this.IndexPixel(i, j + 1, image.Width, image.Height);
											float num28 = this.PixelHeight(image.PixelToBiomeId[num27]);
											num22 += num28 - num20;
										}
										else
										{
											UpdateWorldMap.Chunk.Image image3;
											bool flag16 = this._images.TryGetValue(ChunkHelper.IndexOfChunkColumn(num12, num13 + 1), out image3);
											if (flag16)
											{
												int num29 = this.IndexPixel(i, 0, image3.Width, image3.Height);
												float num30 = this.PixelHeight(image3.PixelToBiomeId[num29]);
												num22 += num30 - num20;
											}
										}
										float num31 = num22 / 2f;
										float percent = num31 * this._maxHeightShading;
										ColorRgba colorRgba = new ColorRgba(clientBiomeData.Color[0], clientBiomeData.Color[1], clientBiomeData.Color[2], byte.MaxValue);
										colorRgba.Darken(percent);
										bool flag17 = num21 < 100f;
										if (flag17)
										{
											ColorHsla colorHsla = ColorHsla.FromRgba(colorRgba);
											colorHsla.Saturate((100f - num21) * this._maxBorderSaturation / 100f);
											colorHsla.ToRgb(out colorRgba.R, out colorRgba.G, out colorRgba.B);
											colorRgba.Darken((100f - num21) / -this._maxBorderShading);
										}
										int num32 = 0;
										while ((float)num32 < num14)
										{
											int num33 = 0;
											while ((float)num33 < num15)
											{
												int num34 = i + num16;
												int num35 = j + num17;
												int num36 = num35 * this._image.Width + num34;
												this._image.Pixels[num36 * 4] = colorRgba.B;
												this._image.Pixels[num36 * 4 + 1] = colorRgba.G;
												this._image.Pixels[num36 * 4 + 2] = colorRgba.R;
												this._image.Pixels[num36 * 4 + 3] = byte.MaxValue;
												num33++;
											}
											num32++;
										}
									}
									else
									{
										int num37 = 0;
										while ((float)num37 < num14)
										{
											int num38 = 0;
											while ((float)num38 < num15)
											{
												int num39 = i + num16;
												int num40 = j + num17;
												int num41 = num40 * this._image.Width + num39;
												this._image.Pixels[num41 * 4] = 0;
												this._image.Pixels[num41 * 4 + 1] = 0;
												this._image.Pixels[num41 * 4 + 2] = 0;
												this._image.Pixels[num41 * 4 + 3] = byte.MaxValue;
												num38++;
											}
											num37++;
										}
									}
								}
							}
						}
					}
					this._gameInstance.Engine.RunOnMainThread(this, delegate
					{
						this._mapTextureNeedsTransfer = true;
						this._minChunkX = minChunkX;
						this._minChunkZ = minChunkZ;
						this._maxChunkX = maxChunkX;
						this._maxChunkZ = maxChunkZ;
						this._mapChunkImageWidth = chunkImageWidth;
						this._mapChunkImageHeight = chunkImageHeight;
						this._mapTextureChunkWidth = textureChunkWidth;
						this._mapTextureChunkHeight = textureChunkHeight;
						this._mapTextureMinChunkX = textureMinChunkX;
						this._mapTextureMinChunkZ = textureMinChunkZ;
						this._mapTextureCenterChunkX = textureCenterChunkX;
						this._mapTextureCenterChunkZ = textureCenterChunkZ;
						this._mapTextureIsUpdating = false;
					}, false, true);
				}
			}
		}

		// Token: 0x06004518 RID: 17688 RVA: 0x000F0480 File Offset: 0x000EE680
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ushort PixelToBiomeId(int chunkPixel)
		{
			return (ushort)(chunkPixel >> 16);
		}

		// Token: 0x06004519 RID: 17689 RVA: 0x000F0498 File Offset: 0x000EE698
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private float PixelBorder(int chunkPixel)
		{
			int num = chunkPixel >> 10 & 63;
			float value = (float)num * (this._maxBorderSize / 63f);
			return MathHelper.Min(value, 1f) * 100f;
		}

		// Token: 0x0600451A RID: 17690 RVA: 0x000F04D4 File Offset: 0x000EE6D4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private float PixelHeight(int chunkPixel)
		{
			return (float)(chunkPixel & 1023) / 255f * 100f;
		}

		// Token: 0x0600451B RID: 17691 RVA: 0x000F04FC File Offset: 0x000EE6FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int IndexPixel(int x, int z, int width, int height)
		{
			return z * width + x;
		}

		// Token: 0x04002262 RID: 8802
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04002263 RID: 8803
		private const string MapMaskTexture = "UI/WorldMap/MapMask.png";

		// Token: 0x04002264 RID: 8804
		private const float HighlightAnimationWidth = 128f;

		// Token: 0x04002265 RID: 8805
		private const float HighlightAnimationHeight = 128f;

		// Token: 0x04002266 RID: 8806
		private const int HighlightAnimationFPS = 15;

		// Token: 0x04002267 RID: 8807
		private const int HighlightAnimationLastFrame = 10;

		// Token: 0x04002268 RID: 8808
		private const float DefaultScale = 16f;

		// Token: 0x04002269 RID: 8809
		private const float MinScale = 2f;

		// Token: 0x0400226A RID: 8810
		private const float MaxScale = 64f;

		// Token: 0x0400226B RID: 8811
		private readonly Font _font;

		// Token: 0x0400226C RID: 8812
		private readonly QuadRenderer _mapRenderer;

		// Token: 0x0400226D RID: 8813
		private readonly GLTexture _maskTexture;

		// Token: 0x0400226E RID: 8814
		private readonly HytaleClient.Graphics.Texture _textureAtlas;

		// Token: 0x0400226F RID: 8815
		private readonly Vector3 _localPlayerColor = new Vector3(0f, 209f, 255f) / 255f;

		// Token: 0x04002270 RID: 8816
		private readonly Vector3 _selectionColor = new Vector3(242f, 206f, 5f) / 255f;

		// Token: 0x04002271 RID: 8817
		private readonly Vector3 _selectionHoverColor = new Vector3(255f, 225f, 59f) / 255f;

		// Token: 0x04002272 RID: 8818
		private readonly Vector3 _hoverColor = new Vector3(182f, 215f, 255f) / 255f;

		// Token: 0x04002273 RID: 8819
		private GLTexture _mapTexture;

		// Token: 0x04002274 RID: 8820
		private GLTexture _mapTextureUpcoming;

		// Token: 0x04002275 RID: 8821
		private WorldMapModule.Texture _playerMarkerTexture;

		// Token: 0x04002276 RID: 8822
		private WorldMapModule.Texture _coordinateMarkerTexture;

		// Token: 0x04002277 RID: 8823
		private WorldMapModule.Texture _highlightAnimationTexture;

		// Token: 0x04002278 RID: 8824
		private Dictionary<string, WorldMapModule.Texture> _textures = new Dictionary<string, WorldMapModule.Texture>();

		// Token: 0x04002279 RID: 8825
		private TextRenderer _hoveredMarkerNameRenderer;

		// Token: 0x0400227A RID: 8826
		private readonly RenderTarget _renderTarget;

		// Token: 0x0400227B RID: 8827
		private WorldMapModule.MarkerSelection _selectedMarker;

		// Token: 0x0400227C RID: 8828
		private WorldMapModule.MarkerSelection _hoveredMarker;

		// Token: 0x0400227D RID: 8829
		private WorldMapModule.MarkerSelection _contextMenuMarker;

		// Token: 0x0400227E RID: 8830
		private Matrix _tempMatrix;

		// Token: 0x0400227F RID: 8831
		private Matrix _projectionMatrix;

		// Token: 0x04002280 RID: 8832
		private Matrix _mapMatrix;

		// Token: 0x04002281 RID: 8833
		private Matrix _hoveredMarkerNameMatrix;

		// Token: 0x04002282 RID: 8834
		private const int MarkerDrawTasksDefaultSize = 20;

		// Token: 0x04002283 RID: 8835
		private const int MarkerDrawTasksGrowth = 5;

		// Token: 0x04002284 RID: 8836
		private WorldMapModule.MarkerDrawTask[] _mapMarkerDrawTasks = new WorldMapModule.MarkerDrawTask[20];

		// Token: 0x04002285 RID: 8837
		private int _mapMarkerDrawTaskCount;

		// Token: 0x04002286 RID: 8838
		private readonly Dictionary<ushort, WorldMapModule.ClientBiomeData> _biomes = new Dictionary<ushort, WorldMapModule.ClientBiomeData>();

		// Token: 0x04002287 RID: 8839
		private readonly Dictionary<string, WorldMapModule.MapMarker> _markers = new Dictionary<string, WorldMapModule.MapMarker>();

		// Token: 0x04002288 RID: 8840
		private readonly Dictionary<long, UpdateWorldMap.Chunk.Image> _images = new Dictionary<long, UpdateWorldMap.Chunk.Image>();

		// Token: 0x04002289 RID: 8841
		private float _scale = 16f;

		// Token: 0x0400228A RID: 8842
		private bool _isMovingOffset;

		// Token: 0x0400228B RID: 8843
		private float _offsetChunksX;

		// Token: 0x0400228C RID: 8844
		private float _offsetChunksZ;

		// Token: 0x0400228D RID: 8845
		private int _minChunkX = int.MaxValue;

		// Token: 0x0400228E RID: 8846
		private int _minChunkZ = int.MaxValue;

		// Token: 0x0400228F RID: 8847
		private int _maxChunkX = int.MinValue;

		// Token: 0x04002290 RID: 8848
		private int _maxChunkZ = int.MinValue;

		// Token: 0x04002291 RID: 8849
		private HashSet<long> _imagesToUpdate = new HashSet<long>();

		// Token: 0x04002292 RID: 8850
		private HashSet<long> _imagesBeingUpdated = new HashSet<long>();

		// Token: 0x04002293 RID: 8851
		private readonly List<long> _tempImagesKeys = new List<long>();

		// Token: 0x04002294 RID: 8852
		private bool _mapTextureNeedsUpdate = false;

		// Token: 0x04002295 RID: 8853
		private bool _mapTextureIsUpdating = false;

		// Token: 0x04002296 RID: 8854
		private bool _mapTextureNeedsTransfer = false;

		// Token: 0x04002297 RID: 8855
		private int _mapChunkImageWidth;

		// Token: 0x04002298 RID: 8856
		private int _mapChunkImageHeight;

		// Token: 0x04002299 RID: 8857
		private int _mapTextureChunkWidth;

		// Token: 0x0400229A RID: 8858
		private int _mapTextureChunkHeight;

		// Token: 0x0400229B RID: 8859
		private int _mapTextureMinChunkX;

		// Token: 0x0400229C RID: 8860
		private int _mapTextureMinChunkZ;

		// Token: 0x0400229D RID: 8861
		private int _mapTextureCenterChunkX;

		// Token: 0x0400229E RID: 8862
		private int _mapTextureCenterChunkZ;

		// Token: 0x0400229F RID: 8863
		private int _previousMouseX;

		// Token: 0x040022A0 RID: 8864
		private int _previousMouseY;

		// Token: 0x040022A1 RID: 8865
		private float _highlightAnimationFrame;

		// Token: 0x040022A2 RID: 8866
		private volatile ushort _hoveredBiomeId;

		// Token: 0x040022A3 RID: 8867
		private float _maxHeightShading = 2f;

		// Token: 0x040022A4 RID: 8868
		private float _maxBorderSize = 1.25f;

		// Token: 0x040022A5 RID: 8869
		private float _maxBorderShading = 3f;

		// Token: 0x040022A6 RID: 8870
		private float _maxBorderSaturation = 0.3f;

		// Token: 0x040022AB RID: 8875
		private const float ImageIncreaseScaleSize = 1.5f;

		// Token: 0x040022AC RID: 8876
		private const int ImageGridSpacing = 0;

		// Token: 0x040022AD RID: 8877
		private const int ChannelCount = 4;

		// Token: 0x040022AE RID: 8878
		private Image _image;

		// Token: 0x040022AF RID: 8879
		private readonly Thread _worldMapThread;

		// Token: 0x040022B0 RID: 8880
		private readonly CancellationTokenSource _updaterThreadCancellationTokenSource;

		// Token: 0x040022B1 RID: 8881
		private readonly CancellationToken _updaterThreadCancellationToken;

		// Token: 0x040022B2 RID: 8882
		private readonly BlockingCollection<Action> _worldMapThreadActionQueue = new BlockingCollection<Action>();

		// Token: 0x02000DCC RID: 3532
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct MarkerDrawTask
		{
			// Token: 0x040043FD RID: 17405
			public QuadRenderer Renderer;

			// Token: 0x040043FE RID: 17406
			public Matrix MVPMatrix;

			// Token: 0x040043FF RID: 17407
			public Vector3 Color;
		}

		// Token: 0x02000DCD RID: 3533
		public class ClientBiomeData
		{
			// Token: 0x04004400 RID: 17408
			public int ZoneId;

			// Token: 0x04004401 RID: 17409
			public string ZoneName;

			// Token: 0x04004402 RID: 17410
			public string BiomeName;

			// Token: 0x04004403 RID: 17411
			public byte[] Color;
		}

		// Token: 0x02000DCE RID: 3534
		public class MapMarker
		{
			// Token: 0x04004404 RID: 17412
			public string Id;

			// Token: 0x04004405 RID: 17413
			public string Name;

			// Token: 0x04004406 RID: 17414
			public string MarkerImage;

			// Token: 0x04004407 RID: 17415
			public float X;

			// Token: 0x04004408 RID: 17416
			public float Y;

			// Token: 0x04004409 RID: 17417
			public float Z;

			// Token: 0x0400440A RID: 17418
			public float Yaw;

			// Token: 0x0400440B RID: 17419
			public float Pitch;

			// Token: 0x0400440C RID: 17420
			public float Roll;

			// Token: 0x0400440D RID: 17421
			public float LerpX;

			// Token: 0x0400440E RID: 17422
			public float LerpZ;

			// Token: 0x0400440F RID: 17423
			public float LerpYaw;
		}

		// Token: 0x02000DCF RID: 3535
		public class Texture
		{
			// Token: 0x06006650 RID: 26192 RVA: 0x00213B16 File Offset: 0x00211D16
			public Texture(Rectangle rectangle)
			{
				this.Rectangle = rectangle;
			}

			// Token: 0x06006651 RID: 26193 RVA: 0x00213B28 File Offset: 0x00211D28
			public void Init(GraphicsDevice graphics, HytaleClient.Graphics.Texture atlas)
			{
				this.QuadRenderer = new QuadRenderer(graphics, graphics.GPUProgramStore.BasicProgram.AttribPosition, graphics.GPUProgramStore.BasicProgram.AttribTexCoords);
				this.QuadRenderer.UpdateUVs((float)(this.Rectangle.Width + this.Rectangle.X) / (float)atlas.Width, (float)(this.Rectangle.Height + this.Rectangle.Y) / (float)atlas.Height, (float)this.Rectangle.X / (float)atlas.Width, (float)this.Rectangle.Y / (float)atlas.Height);
			}

			// Token: 0x06006652 RID: 26194 RVA: 0x00213BD8 File Offset: 0x00211DD8
			public void GenerateHitDetectionMap(byte[] pixels)
			{
				this._opaquePixels = new bool[this.Rectangle.Width * this.Rectangle.Height];
				for (int i = 0; i < this._opaquePixels.Length; i++)
				{
					this._opaquePixels[i] = (pixels[i * 4 + 3] == byte.MaxValue);
				}
			}

			// Token: 0x06006653 RID: 26195 RVA: 0x00213C38 File Offset: 0x00211E38
			public bool IsPointOpaque(int x, int y)
			{
				return this._opaquePixels[y * this.Rectangle.Width + x];
			}

			// Token: 0x04004410 RID: 17424
			public Rectangle Rectangle;

			// Token: 0x04004411 RID: 17425
			public QuadRenderer QuadRenderer;

			// Token: 0x04004412 RID: 17426
			private bool[] _opaquePixels;
		}

		// Token: 0x02000DD0 RID: 3536
		public enum MarkerSelectionType
		{
			// Token: 0x04004414 RID: 17428
			None,
			// Token: 0x04004415 RID: 17429
			LocalPlayer,
			// Token: 0x04004416 RID: 17430
			ServerMarker,
			// Token: 0x04004417 RID: 17431
			Coordinates
		}

		// Token: 0x02000DD1 RID: 3537
		public struct MarkerSelection
		{
			// Token: 0x17001458 RID: 5208
			// (get) Token: 0x06006654 RID: 26196 RVA: 0x00213C60 File Offset: 0x00211E60
			public static WorldMapModule.MarkerSelection None
			{
				get
				{
					return WorldMapModule.MarkerSelection._noSelection;
				}
			}

			// Token: 0x06006655 RID: 26197 RVA: 0x00213C68 File Offset: 0x00211E68
			public bool Equals(WorldMapModule.MarkerSelection other)
			{
				return other.Type == this.Type && other.MapMarker == this.MapMarker && other.Coordinates == this.Coordinates;
			}

			// Token: 0x04004418 RID: 17432
			private static readonly WorldMapModule.MarkerSelection _noSelection = new WorldMapModule.MarkerSelection
			{
				Type = WorldMapModule.MarkerSelectionType.None
			};

			// Token: 0x04004419 RID: 17433
			public WorldMapModule.MarkerSelectionType Type;

			// Token: 0x0400441A RID: 17434
			public WorldMapModule.MapMarker MapMarker;

			// Token: 0x0400441B RID: 17435
			public Point Coordinates;
		}
	}
}
