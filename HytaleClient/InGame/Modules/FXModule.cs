using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Graphics;
using HytaleClient.Math;
using HytaleClient.Networking;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.InGame.Modules
{
	// Token: 0x020008F2 RID: 2290
	internal class FXModule : Module
	{
		// Token: 0x170010FD RID: 4349
		// (get) Token: 0x06004405 RID: 17413 RVA: 0x000E3B6E File Offset: 0x000E1D6E
		public GLTexture UVMotionTextureArray2D
		{
			get
			{
				return this._uvMotionTextureArray2D.GLTexture;
			}
		}

		// Token: 0x170010FE RID: 4350
		// (get) Token: 0x06004406 RID: 17414 RVA: 0x000E3B7B File Offset: 0x000E1D7B
		public int UVMotionTextureCount
		{
			get
			{
				return this._uvMotionTextureArrayLayerCount;
			}
		}

		// Token: 0x06004407 RID: 17415 RVA: 0x000E3B83 File Offset: 0x000E1D83
		public FXModule(GameInstance gameInstance) : base(gameInstance)
		{
			this._gl = gameInstance.Engine.Graphics.GL;
			this.CreateGPUData();
		}

		// Token: 0x06004408 RID: 17416 RVA: 0x000E3BAC File Offset: 0x000E1DAC
		public void PrepareAtlas(Dictionary<string, PacketHandler.TextureInfo> particleTextureInfos, Dictionary<string, PacketHandler.TextureInfo> trailTextureInfos, out Dictionary<string, Rectangle> upcomingImageLocations, out byte[][] upcomingAtlasPixelsPerLevel, CancellationToken cancellationToken)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			upcomingImageLocations = new Dictionary<string, Rectangle>();
			Dictionary<string, PacketHandler.TextureInfo> dictionary = new Dictionary<string, PacketHandler.TextureInfo>();
			foreach (KeyValuePair<string, PacketHandler.TextureInfo> keyValuePair in trailTextureInfos)
			{
				dictionary[keyValuePair.Key] = keyValuePair.Value;
			}
			foreach (KeyValuePair<string, PacketHandler.TextureInfo> keyValuePair2 in particleTextureInfos)
			{
				dictionary[keyValuePair2.Key] = keyValuePair2.Value;
			}
			List<PacketHandler.TextureInfo> list = new List<PacketHandler.TextureInfo>(dictionary.Values);
			list.Sort((PacketHandler.TextureInfo a, PacketHandler.TextureInfo b) => b.Height.CompareTo(a.Height));
			Point zero = Point.Zero;
			int num = 0;
			int num2 = 512;
			for (int i = 0; i < list.Count; i++)
			{
				PacketHandler.TextureInfo textureInfo = list[i];
				bool isCancellationRequested = cancellationToken.IsCancellationRequested;
				if (isCancellationRequested)
				{
					upcomingAtlasPixelsPerLevel = null;
					return;
				}
				bool flag = zero.X + textureInfo.Width > this.TextureAtlas.Width;
				if (flag)
				{
					zero.X = 0;
					zero.Y = num;
				}
				while (zero.Y + textureInfo.Height > num2)
				{
					num2 <<= 1;
				}
				upcomingImageLocations.Add(textureInfo.Checksum, new Rectangle(zero.X, zero.Y, textureInfo.Width, textureInfo.Height));
				num = Math.Max(num, zero.Y + textureInfo.Height + 2);
				zero.X += textureInfo.Width + 2;
			}
			byte[] array = new byte[this.TextureAtlas.Width * num2 * 4];
			for (int j = 0; j < list.Count; j++)
			{
				PacketHandler.TextureInfo textureInfo2 = list[j];
				bool isCancellationRequested2 = cancellationToken.IsCancellationRequested;
				if (isCancellationRequested2)
				{
					upcomingAtlasPixelsPerLevel = null;
					return;
				}
				try
				{
					Image image = new Image(AssetManager.GetAssetUsingHash(textureInfo2.Checksum, false));
					for (int k = 0; k < image.Height; k++)
					{
						Rectangle rectangle = upcomingImageLocations[textureInfo2.Checksum];
						int dstOffset = ((rectangle.Y + k) * this.TextureAtlas.Width + rectangle.X) * 4;
						Buffer.BlockCopy(image.Pixels, k * image.Width * 4, array, dstOffset, image.Width * 4);
					}
				}
				catch (Exception exception)
				{
					FXModule.Logger.Error(exception, "Failed to load FX texture: " + AssetManager.GetAssetLocalPathUsingHash(textureInfo2.Checksum));
				}
			}
			upcomingAtlasPixelsPerLevel = Texture.BuildMipmapPixels(array, this.TextureAtlas.Width, this.TextureAtlas.MipmapLevelCount);
		}

		// Token: 0x06004409 RID: 17417 RVA: 0x000E3EE8 File Offset: 0x000E20E8
		public void CreateAtlasTextures(Dictionary<string, Rectangle> imageLocations, byte[][] atlasPixelsPerLevel)
		{
			this.ImageLocations = imageLocations;
			this.TextureAtlas.UpdateTexture2DMipMaps(atlasPixelsPerLevel);
			this._gameInstance.ParticleSystemStoreModule.UpdateTextures();
			this._gameInstance.TrailStoreModule.UpdateTextures();
		}

		// Token: 0x0600440A RID: 17418 RVA: 0x000E3F24 File Offset: 0x000E2124
		public void PrepareUVMotionTextureArray(List<string> uvMotionTexturePaths, out byte[][] upcomingAtlasPixelsPerLevel)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			this._uvMotionTextureArrayLayerCount = uvMotionTexturePaths.Count;
			byte[] array = new byte[4096 * this._uvMotionTextureArrayLayerCount * 4];
			int num = 0;
			for (int i = 0; i < uvMotionTexturePaths.Count; i++)
			{
				string text = uvMotionTexturePaths[i];
				try
				{
					Image image = new Image(AssetManager.GetBuiltInAsset(Path.Combine("Common", text)));
					int dstOffset = num * 64 * 4;
					Buffer.BlockCopy(image.Pixels, 0, array, dstOffset, image.Width * image.Height * 4);
					num += image.Height;
				}
				catch (Exception exception)
				{
					FXModule.Logger.Error(exception, "Failed to load UV motion texture: " + text);
				}
			}
			upcomingAtlasPixelsPerLevel = Texture.BuildMipmapPixels(array, 64, this.TextureAtlas.MipmapLevelCount);
		}

		// Token: 0x0600440B RID: 17419 RVA: 0x000E4018 File Offset: 0x000E2218
		public void CreateUVMotionTextureArray(byte[][] atlasPixelsPerLevel)
		{
			this._uvMotionTextureArray2D.UpdateTexture2DArray(64, 64, this._uvMotionTextureArrayLayerCount, atlasPixelsPerLevel[0]);
		}

		// Token: 0x0600440C RID: 17420 RVA: 0x000E4034 File Offset: 0x000E2234
		private void CreateGPUData()
		{
			this.TextureAtlas = new Texture(Texture.TextureTypes.Texture2D);
			this.TextureAtlas.CreateTexture2D(2048, 32, null, 0, GL.NEAREST_MIPMAP_NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
			this._uvMotionTextureArray2D = new Texture(Texture.TextureTypes.Texture2DArray);
			this._uvMotionTextureArray2D.CreateTexture2DArray(64, 64, this._uvMotionTextureArrayLayerCount, null, GL.LINEAR, GL.LINEAR, GL.REPEAT, GL.REPEAT, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
		}

		// Token: 0x0600440D RID: 17421 RVA: 0x000E40CE File Offset: 0x000E22CE
		private void DestroyGPUData()
		{
			this._uvMotionTextureArray2D.Dispose();
			this.TextureAtlas.Dispose();
		}

		// Token: 0x0600440E RID: 17422 RVA: 0x000E40E9 File Offset: 0x000E22E9
		protected override void DoDispose()
		{
			this.DestroyGPUData();
		}

		// Token: 0x04002189 RID: 8585
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400218A RID: 8586
		public const byte FXAtlasIndex = 3;

		// Token: 0x0400218B RID: 8587
		private const int TextureSpacing = 2;

		// Token: 0x0400218C RID: 8588
		public Dictionary<string, Rectangle> ImageLocations;

		// Token: 0x0400218D RID: 8589
		public Texture TextureAtlas;

		// Token: 0x0400218E RID: 8590
		private GLFunctions _gl;

		// Token: 0x0400218F RID: 8591
		private Texture _uvMotionTextureArray2D;

		// Token: 0x04002190 RID: 8592
		private const int _uvMotionTextureArrayHeight = 64;

		// Token: 0x04002191 RID: 8593
		private const int _uvMotionTextureArrayWidth = 64;

		// Token: 0x04002192 RID: 8594
		private int _uvMotionTextureArrayLayerCount;
	}
}
