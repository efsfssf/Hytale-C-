using System;
using System.Collections.Generic;
using HytaleClient.Data;
using HytaleClient.Graphics;
using HytaleClient.Math;

namespace HytaleClient.Interface.AssetEditor.Utils
{
	// Token: 0x02000807 RID: 2055
	public class TextureAtlasUtils
	{
		// Token: 0x06003908 RID: 14600 RVA: 0x00076F18 File Offset: 0x00075118
		public static Texture CreateTextureAtlas(Dictionary<string, Image> textures, out Dictionary<string, Point> textureLocations)
		{
			int num = 0;
			int num2 = 0;
			foreach (Image image in textures.Values)
			{
				num += image.Width;
				bool flag = image.Height > num2;
				if (flag)
				{
					num2 = image.Height;
				}
			}
			textureLocations = new Dictionary<string, Point>();
			byte[] array = new byte[num * num2 * 4];
			int num3 = 0;
			int num4 = 0;
			foreach (KeyValuePair<string, Image> keyValuePair in textures)
			{
				Image value = keyValuePair.Value;
				for (int i = 0; i < value.Height; i++)
				{
					int dstOffset = (i * num + num3) * 4;
					Buffer.BlockCopy(value.Pixels, i * value.Width * 4, array, dstOffset, value.Width * 4);
				}
				textureLocations.Add(keyValuePair.Key, new Point(num3, 0));
				num3 += value.Width;
				num4++;
			}
			Texture texture = new Texture(Texture.TextureTypes.Texture2D);
			texture.CreateTexture2D(num, num2, null, 5, GL.NEAREST_MIPMAP_NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
			bool flag2 = textureLocations.Count > 0;
			if (flag2)
			{
				byte[][] pixelsPerMipmapLevel = Texture.BuildMipmapPixels(array, num, texture.MipmapLevelCount);
				texture.UpdateTexture2DMipMaps(pixelsPerMipmapLevel);
			}
			return texture;
		}
	}
}
