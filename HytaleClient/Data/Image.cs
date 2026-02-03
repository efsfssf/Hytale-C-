using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.Data
{
	// Token: 0x02000AC8 RID: 2760
	public class Image
	{
		// Token: 0x060056FF RID: 22271 RVA: 0x001A5844 File Offset: 0x001A3A44
		public Image(int width, int height, byte[] pixels)
		{
			this.Width = width;
			this.Height = height;
			this.Pixels = pixels;
		}

		// Token: 0x06005700 RID: 22272 RVA: 0x001A5863 File Offset: 0x001A3A63
		public Image(byte[] data) : this("", data)
		{
		}

		// Token: 0x06005701 RID: 22273 RVA: 0x001A5874 File Offset: 0x001A3A74
		public unsafe Image(string name, byte[] data)
		{
			this.Name = name;
			object @lock = Image.Lock;
			lock (@lock)
			{
				IntPtr intPtr;
				fixed (byte[] array = data)
				{
					byte* value;
					if (data == null || array.Length == 0)
					{
						value = null;
					}
					else
					{
						value = &array[0];
					}
					IntPtr src = SDL.SDL_RWFromMem((IntPtr)((void*)value), data.Length);
					intPtr = SDL_image.IMG_Load_RW(src, 1);
				}
				bool flag2 = intPtr == IntPtr.Zero;
				if (flag2)
				{
					throw new Exception("Could not load image: " + name + " - " + SDL.SDL_GetError());
				}
				SDL.SDL_Surface* ptr = (SDL.SDL_Surface*)((void*)intPtr);
				SDL.SDL_PixelFormat* ptr2 = (SDL.SDL_PixelFormat*)((void*)ptr->format);
				bool flag3 = ptr2->format != SDL.SDL_PIXELFORMAT_ABGR8888;
				if (flag3)
				{
					IntPtr intPtr2 = SDL.SDL_ConvertSurfaceFormat(intPtr, SDL.SDL_PIXELFORMAT_ABGR8888, 0U);
					SDL.SDL_FreeSurface(intPtr);
					intPtr = intPtr2;
				}
				SDL.SDL_Surface* ptr3 = (SDL.SDL_Surface*)((void*)intPtr);
				this.Width = ptr3->w;
				this.Height = ptr3->h;
				this.Pixels = new byte[this.Width * this.Height * 4];
				Marshal.Copy(ptr3->pixels, this.Pixels, 0, this.Pixels.Length);
				SDL.SDL_FreeSurface(intPtr);
			}
		}

		// Token: 0x06005702 RID: 22274 RVA: 0x001A59E0 File Offset: 0x001A3BE0
		public Image(string name, int width, int height, byte[] pixels)
		{
			bool flag = pixels.Length != width * height * 4;
			if (flag)
			{
				throw new ArgumentException("Could not load image: " + name + " - Pixels array length must be width * height * 4");
			}
			this.Name = name;
			this.Width = width;
			this.Height = height;
			this.Pixels = pixels;
		}

		// Token: 0x06005703 RID: 22275 RVA: 0x001A5A3C File Offset: 0x001A3C3C
		public static bool TryGetPngDimensions(string path, out int width, out int height)
		{
			bool result;
			try
			{
				using (BinaryReader binaryReader = new BinaryReader(File.OpenRead(path)))
				{
					binaryReader.BaseStream.Position += 16L;
					byte[] array = binaryReader.ReadBytes(4);
					width = ((int)array[0] << 24 | (int)array[1] << 16 | (int)array[2] << 8 | (int)array[3]);
					byte[] array2 = binaryReader.ReadBytes(4);
					height = ((int)array2[0] << 24 | (int)array2[1] << 16 | (int)array2[2] << 8 | (int)array2[3]);
				}
				result = true;
			}
			catch
			{
				width = (height = -1);
				result = false;
			}
			return result;
		}

		// Token: 0x06005704 RID: 22276 RVA: 0x001A5AF0 File Offset: 0x001A3CF0
		public void SavePNG(string path, int destWidth, int destHeight, uint redMask = 255U, uint greenMask = 65280U, uint blueMask = 16711680U, uint alphaMask = 4278190080U)
		{
			this.DoWithSurface(destWidth, destHeight, delegate(IntPtr surface)
			{
				bool flag = SDL_image.IMG_SavePNG(surface, path) != 0;
				if (flag)
				{
					throw new Exception("Could not save PNG: " + path + " - " + SDL.SDL_GetError());
				}
			}, redMask, greenMask, blueMask, alphaMask);
		}

		// Token: 0x06005705 RID: 22277 RVA: 0x001A5B28 File Offset: 0x001A3D28
		public void SavePNG(string path, uint redMask = 255U, uint greenMask = 65280U, uint blueMask = 16711680U, uint alphaMask = 4278190080U)
		{
			this.SavePNG(path, this.Width, this.Height, redMask, greenMask, blueMask, alphaMask);
		}

		// Token: 0x06005706 RID: 22278 RVA: 0x001A5B48 File Offset: 0x001A3D48
		public unsafe void DoWithSurface(int destWidth, int destHeight, Action<IntPtr> action, uint redMask = 255U, uint greenMask = 65280U, uint blueMask = 16711680U, uint alphaMask = 4278190080U)
		{
			object @lock = Image.Lock;
			lock (@lock)
			{
				byte[] array;
				byte* value;
				if ((array = this.Pixels) == null || array.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array[0];
				}
				IntPtr intPtr = SDL.SDL_CreateRGBSurfaceFrom((IntPtr)((void*)value), this.Width, this.Height, 32, this.Width * 4, redMask, greenMask, blueMask, alphaMask);
				bool flag2 = destWidth != this.Width || destHeight != this.Height;
				if (flag2)
				{
					IntPtr intPtr2 = SDL.SDL_CreateRGBSurface(((SDL.SDL_Surface*)((void*)intPtr))->flags, destWidth, destHeight, 32, redMask, greenMask, blueMask, alphaMask);
					bool flag3 = SDL.SDL_BlitScaled(intPtr, IntPtr.Zero, intPtr2, IntPtr.Zero) < 0;
					if (flag3)
					{
						throw new Exception("SDL_BlitScaled failed: " + SDL.SDL_GetError());
					}
					SDL.SDL_FreeSurface(intPtr);
					intPtr = intPtr2;
				}
				try
				{
					action(intPtr);
				}
				finally
				{
					SDL.SDL_FreeSurface(intPtr);
				}
				array = null;
			}
		}

		// Token: 0x06005707 RID: 22279 RVA: 0x001A5C78 File Offset: 0x001A3E78
		public static Image Pack(string name, int width, List<Image> images, out Dictionary<Image, Point> imageLocations, bool doPadding, CancellationToken cancellationToken = default(CancellationToken))
		{
			byte[] array = Image.Pack(width, images, out imageLocations, doPadding, cancellationToken);
			return new Image(name, width, array.Length / width / 4, array);
		}

		// Token: 0x06005708 RID: 22280 RVA: 0x001A5CA8 File Offset: 0x001A3EA8
		public static byte[] Pack(int width, List<Image> images, out Dictionary<Image, Point> imageLocations, bool doPadding, CancellationToken cancellationToken = default(CancellationToken))
		{
			Image.<>c__DisplayClass15_0 CS$<>8__locals1;
			CS$<>8__locals1.width = width;
			int num = 32;
			imageLocations = new Dictionary<Image, Point>();
			Point point = Point.Zero;
			int num2 = 0;
			int num3 = doPadding ? 2 : 0;
			int num4 = doPadding ? 1 : 0;
			foreach (Image image in images)
			{
				bool isCancellationRequested = cancellationToken.IsCancellationRequested;
				if (isCancellationRequested)
				{
					imageLocations = null;
					return null;
				}
				bool flag = point.X + image.Width + num3 > CS$<>8__locals1.width;
				if (flag)
				{
					point.X = 0;
					point.Y = num2;
				}
				while (point.Y + image.Height >= num)
				{
					num <<= 1;
				}
				num2 = Math.Max(num2, point.Y + image.Height + num3);
				imageLocations[image] = new Point(point.X + num4, point.Y + num4);
				point.X += image.Width + num3;
			}
			CS$<>8__locals1.pixels = new byte[CS$<>8__locals1.width * num * 4];
			foreach (Image image2 in images)
			{
				Image.<>c__DisplayClass15_1 CS$<>8__locals2;
				CS$<>8__locals2.image = image2;
				bool isCancellationRequested2 = cancellationToken.IsCancellationRequested;
				if (isCancellationRequested2)
				{
					imageLocations = null;
					return null;
				}
				point = imageLocations[CS$<>8__locals2.image];
				for (int i = 0; i < CS$<>8__locals2.image.Height; i++)
				{
					int dstOffset = ((point.Y + i) * CS$<>8__locals1.width + point.X) * 4;
					Buffer.BlockCopy(CS$<>8__locals2.image.Pixels, i * CS$<>8__locals2.image.Width * 4, CS$<>8__locals1.pixels, dstOffset, CS$<>8__locals2.image.Width * 4);
				}
				if (doPadding)
				{
					Image.<Pack>g__CopyPixel|15_0(0, 0, point.X - 1, point.Y - 1, ref CS$<>8__locals1, ref CS$<>8__locals2);
					Buffer.BlockCopy(CS$<>8__locals2.image.Pixels, 0, CS$<>8__locals1.pixels, ((point.Y - 1) * CS$<>8__locals1.width + point.X) * 4, CS$<>8__locals2.image.Width * 4);
					Image.<Pack>g__CopyPixel|15_0(CS$<>8__locals2.image.Width - 1, 0, point.X + CS$<>8__locals2.image.Width, point.Y - 1, ref CS$<>8__locals1, ref CS$<>8__locals2);
					for (int j = 0; j < CS$<>8__locals2.image.Height; j++)
					{
						Image.<Pack>g__CopyPixel|15_0(0, j, point.X - 1, point.Y + j, ref CS$<>8__locals1, ref CS$<>8__locals2);
					}
					for (int k = 0; k < CS$<>8__locals2.image.Height; k++)
					{
						Image.<Pack>g__CopyPixel|15_0(CS$<>8__locals2.image.Width - 1, k, point.X + CS$<>8__locals2.image.Width, point.Y + k, ref CS$<>8__locals1, ref CS$<>8__locals2);
					}
					Image.<Pack>g__CopyPixel|15_0(0, CS$<>8__locals2.image.Height - 1, point.X - 1, point.Y + CS$<>8__locals2.image.Height, ref CS$<>8__locals1, ref CS$<>8__locals2);
					Buffer.BlockCopy(CS$<>8__locals2.image.Pixels, (CS$<>8__locals2.image.Height - 1) * CS$<>8__locals2.image.Width * 4, CS$<>8__locals1.pixels, ((point.Y + CS$<>8__locals2.image.Height) * CS$<>8__locals1.width + point.X) * 4, CS$<>8__locals2.image.Width * 4);
					Image.<Pack>g__CopyPixel|15_0(CS$<>8__locals2.image.Width - 1, CS$<>8__locals2.image.Height - 1, point.X + CS$<>8__locals2.image.Width, point.Y + CS$<>8__locals2.image.Height, ref CS$<>8__locals1, ref CS$<>8__locals2);
				}
			}
			return CS$<>8__locals1.pixels;
		}

		// Token: 0x0600570A RID: 22282 RVA: 0x001A6124 File Offset: 0x001A4324
		[CompilerGenerated]
		internal static void <Pack>g__CopyPixel|15_0(int x, int y, int dstX, int dstY, ref Image.<>c__DisplayClass15_0 A_4, ref Image.<>c__DisplayClass15_1 A_5)
		{
			A_4.pixels[(dstY * A_4.width + dstX) * 4] = A_5.image.Pixels[(y * A_5.image.Width + x) * 4];
			A_4.pixels[(dstY * A_4.width + dstX) * 4 + 2] = A_5.image.Pixels[(y * A_5.image.Width + x) * 4 + 2];
			A_4.pixels[(dstY * A_4.width + dstX) * 4 + 3] = A_5.image.Pixels[(y * A_5.image.Width + x) * 4 + 3];
			A_4.pixels[(dstY * A_4.width + dstX) * 4 + 1] = A_5.image.Pixels[(y * A_5.image.Width + x) * 4 + 1];
		}

		// Token: 0x040034BB RID: 13499
		private static readonly object Lock = new object();

		// Token: 0x040034BC RID: 13500
		public readonly byte[] Pixels;

		// Token: 0x040034BD RID: 13501
		public readonly int Width;

		// Token: 0x040034BE RID: 13502
		public readonly int Height;

		// Token: 0x040034BF RID: 13503
		public readonly string Name;

		// Token: 0x040034C0 RID: 13504
		private const int MinimumPackingHeight = 32;
	}
}
