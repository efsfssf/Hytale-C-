using System;
using System.Diagnostics;
using HytaleClient.Core;
using HytaleClient.Math;
using HytaleClient.Utils;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A4B RID: 2635
	public class Texture : Disposable
	{
		// Token: 0x060053D8 RID: 21464 RVA: 0x0017FADA File Offset: 0x0017DCDA
		public static void InitializeGL(GLFunctions gl)
		{
			Texture._gl = gl;
		}

		// Token: 0x060053D9 RID: 21465 RVA: 0x0017FAE2 File Offset: 0x0017DCE2
		public static void ReleaseGL()
		{
			Texture._gl = null;
		}

		// Token: 0x170012F3 RID: 4851
		// (get) Token: 0x060053DA RID: 21466 RVA: 0x0017FAEA File Offset: 0x0017DCEA
		public int Width
		{
			get
			{
				return this._width;
			}
		}

		// Token: 0x170012F4 RID: 4852
		// (get) Token: 0x060053DB RID: 21467 RVA: 0x0017FAF2 File Offset: 0x0017DCF2
		public int Height
		{
			get
			{
				return this._height;
			}
		}

		// Token: 0x170012F5 RID: 4853
		// (get) Token: 0x060053DC RID: 21468 RVA: 0x0017FAFA File Offset: 0x0017DCFA
		public int MipmapLevelCount
		{
			get
			{
				return this._mipmapLevelCount;
			}
		}

		// Token: 0x170012F6 RID: 4854
		// (get) Token: 0x060053DD RID: 21469 RVA: 0x0017FB02 File Offset: 0x0017DD02
		// (set) Token: 0x060053DE RID: 21470 RVA: 0x0017FB0A File Offset: 0x0017DD0A
		public GLTexture GLTexture { get; protected set; }

		// Token: 0x060053DF RID: 21471 RVA: 0x0017FB13 File Offset: 0x0017DD13
		public Texture(Texture.TextureTypes type)
		{
			this.TextureType = type;
		}

		// Token: 0x060053E0 RID: 21472 RVA: 0x0017FB24 File Offset: 0x0017DD24
		public unsafe void CreateTexture2D(int width, int height, byte[] pixels = null, int mipmapLevelCount = 5, GL minFilter = GL.NEAREST, GL magFilter = GL.NEAREST, GL wrapS = GL.CLAMP_TO_EDGE, GL wrapT = GL.CLAMP_TO_EDGE, GL internalFormat = GL.RGBA, GL format = GL.RGBA, GL type = GL.UNSIGNED_BYTE, bool requestMipMapChain = false)
		{
			Debug.Assert(this.TextureType == Texture.TextureTypes.Texture2D);
			this._width = width;
			this._height = height;
			this._mipmapLevelCount = mipmapLevelCount;
			this._internalFormat = internalFormat;
			this._format = format;
			this._type = type;
			this._requestMipMapChain = requestMipMapChain;
			this.GLTexture = Texture._gl.GenTexture();
			Texture._gl.BindTexture(GL.TEXTURE_2D, this.GLTexture);
			Texture._gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAX_LEVEL, this._mipmapLevelCount);
			Texture._gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, (int)minFilter);
			Texture._gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, (int)magFilter);
			Texture._gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_S, (int)wrapS);
			Texture._gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_T, (int)wrapT);
			fixed (byte[] array = pixels)
			{
				byte* value;
				if (pixels == null || array.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array[0];
				}
				Texture._gl.TexImage2D(GL.TEXTURE_2D, 0, (int)internalFormat, width, height, 0, format, type, (IntPtr)((void*)value));
			}
			bool requestMipMapChain2 = this._requestMipMapChain;
			if (requestMipMapChain2)
			{
				Texture._gl.GenerateMipmap(GL.TEXTURE_2D);
			}
		}

		// Token: 0x060053E1 RID: 21473 RVA: 0x0017FC90 File Offset: 0x0017DE90
		public unsafe void UpdateTexture2DMipMaps(byte[][] pixelsPerMipmapLevel)
		{
			Debug.Assert(this.TextureType == Texture.TextureTypes.Texture2D);
			Debug.Assert(ThreadHelper.IsMainThread());
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			this._height = pixelsPerMipmapLevel[0].Length / this._width / 4;
			int num = this._width;
			int num2 = this._height;
			Texture._gl.BindTexture(GL.TEXTURE_2D, this.GLTexture);
			for (int i = 0; i < pixelsPerMipmapLevel.Length; i++)
			{
				byte[] array;
				byte* value;
				if ((array = pixelsPerMipmapLevel[i]) == null || array.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array[0];
				}
				Texture._gl.TexImage2D(GL.TEXTURE_2D, i, (int)this._internalFormat, num, num2, 0, this._format, this._type, (IntPtr)((void*)value));
				array = null;
				num = Math.Max(num / 2, 1);
				num2 = Math.Max(num2 / 2, 1);
			}
		}

		// Token: 0x060053E2 RID: 21474 RVA: 0x0017FD90 File Offset: 0x0017DF90
		public unsafe void UpdateTexture2D(byte[] pixels)
		{
			Debug.Assert(this.TextureType == Texture.TextureTypes.Texture2D);
			Debug.Assert(ThreadHelper.IsMainThread());
			Texture._gl.BindTexture(GL.TEXTURE_2D, this.GLTexture);
			fixed (byte[] array = pixels)
			{
				byte* value;
				if (pixels == null || array.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array[0];
				}
				Texture._gl.TexSubImage2D(GL.TEXTURE_2D, 0, 0, 0, this._width, this._height, this._format, this._type, (IntPtr)((void*)value));
			}
			bool requestMipMapChain = this._requestMipMapChain;
			if (requestMipMapChain)
			{
				Texture._gl.GenerateMipmap(GL.TEXTURE_2D);
			}
		}

		// Token: 0x060053E3 RID: 21475 RVA: 0x0017FE40 File Offset: 0x0017E040
		public unsafe void CreateTextureCubemap(int width, int height, byte[][] pixels, int mipmapLevelCount, GL minFilter, GL magFilter, GL wrapS, GL wrapT, GL wrapR, GL internalFormat = GL.RGBA, GL format = GL.RGBA, GL type = GL.UNSIGNED_BYTE, bool requestMipMapChain = false)
		{
			Debug.Assert(this.TextureType == Texture.TextureTypes.TextureCubemap);
			this._width = width;
			this._height = height;
			this._mipmapLevelCount = mipmapLevelCount;
			this._internalFormat = internalFormat;
			this._format = format;
			this._type = type;
			this._requestMipMapChain = requestMipMapChain;
			this.GLTexture = Texture._gl.GenTexture();
			Texture._gl.BindTexture(GL.TEXTURE_CUBE_MAP, this.GLTexture);
			Texture._gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAX_LEVEL, this._mipmapLevelCount);
			Texture._gl.TexParameteri(GL.TEXTURE_CUBE_MAP, GL.TEXTURE_MIN_FILTER, (int)minFilter);
			Texture._gl.TexParameteri(GL.TEXTURE_CUBE_MAP, GL.TEXTURE_MAG_FILTER, (int)magFilter);
			Texture._gl.TexParameteri(GL.TEXTURE_CUBE_MAP, GL.TEXTURE_WRAP_S, (int)wrapS);
			Texture._gl.TexParameteri(GL.TEXTURE_CUBE_MAP, GL.TEXTURE_WRAP_T, (int)wrapT);
			Texture._gl.TexParameteri(GL.TEXTURE_CUBE_MAP, GL.TEXTURE_WRAP_R, (int)wrapR);
			byte[] array;
			byte* value;
			if ((array = pixels[0]) == null || array.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array[0];
			}
			byte[] array2;
			byte* value2;
			if ((array2 = pixels[1]) == null || array2.Length == 0)
			{
				value2 = null;
			}
			else
			{
				value2 = &array2[0];
			}
			byte[] array3;
			byte* value3;
			if ((array3 = pixels[2]) == null || array3.Length == 0)
			{
				value3 = null;
			}
			else
			{
				value3 = &array3[0];
			}
			byte[] array4;
			byte* value4;
			if ((array4 = pixels[3]) == null || array4.Length == 0)
			{
				value4 = null;
			}
			else
			{
				value4 = &array4[0];
			}
			byte[] array5;
			byte* value5;
			if ((array5 = pixels[4]) == null || array5.Length == 0)
			{
				value5 = null;
			}
			else
			{
				value5 = &array5[0];
			}
			byte[] array6;
			byte* value6;
			if ((array6 = pixels[5]) == null || array6.Length == 0)
			{
				value6 = null;
			}
			else
			{
				value6 = &array6[0];
			}
			Texture._gl.TexImage2D(GL.TEXTURE_CUBE_MAP_POSITIVE_X, 0, (int)internalFormat, width, height, 0, format, type, (IntPtr)((void*)value));
			Texture._gl.TexImage2D((GL)34070U, 0, (int)internalFormat, width, height, 0, format, type, (IntPtr)((void*)value2));
			Texture._gl.TexImage2D((GL)34071U, 0, (int)internalFormat, width, height, 0, format, type, (IntPtr)((void*)value3));
			Texture._gl.TexImage2D((GL)34072U, 0, (int)internalFormat, width, height, 0, format, type, (IntPtr)((void*)value4));
			Texture._gl.TexImage2D((GL)34074U, 0, (int)internalFormat, width, height, 0, format, type, (IntPtr)((void*)value6));
			Texture._gl.TexImage2D((GL)34073U, 0, (int)internalFormat, width, height, 0, format, type, (IntPtr)((void*)value5));
			array = null;
			array2 = null;
			array3 = null;
			array4 = null;
			array5 = null;
			array6 = null;
			if (requestMipMapChain)
			{
				Texture._gl.GenerateMipmap(GL.TEXTURE_CUBE_MAP);
			}
		}

		// Token: 0x060053E4 RID: 21476 RVA: 0x0018012C File Offset: 0x0017E32C
		public void CreateTexture3D(int width, int height, int depth, IntPtr data, GL minFilter, GL magFilter, GL wrapS, GL wrapT, GL wrapR, GL type, GL internalFormat, GL format, bool requestMipMapChain = false)
		{
			Debug.Assert(this.TextureType == Texture.TextureTypes.Texture3D);
			this._width = width;
			this._height = height;
			this._depth = depth;
			this._internalFormat = internalFormat;
			this._format = format;
			this._type = type;
			this._requestMipMapChain = requestMipMapChain;
			this.GLTexture = Texture._gl.GenTexture();
			Texture._gl.BindTexture(GL.TEXTURE_3D, this.GLTexture);
			Texture._gl.TexParameteri(GL.TEXTURE_3D, GL.TEXTURE_MIN_FILTER, (int)minFilter);
			Texture._gl.TexParameteri(GL.TEXTURE_3D, GL.TEXTURE_MAG_FILTER, (int)magFilter);
			Texture._gl.TexParameteri(GL.TEXTURE_3D, GL.TEXTURE_WRAP_S, (int)wrapS);
			Texture._gl.TexParameteri(GL.TEXTURE_3D, GL.TEXTURE_WRAP_T, (int)wrapT);
			Texture._gl.TexParameteri(GL.TEXTURE_3D, GL.TEXTURE_WRAP_R, (int)wrapR);
			Texture._gl.TexImage3D(GL.TEXTURE_3D, 0, (int)internalFormat, width, height, depth, 0, format, type, data);
			bool requestMipMapChain2 = this._requestMipMapChain;
			if (requestMipMapChain2)
			{
				Texture._gl.GenerateMipmap(GL.TEXTURE_3D);
			}
		}

		// Token: 0x060053E5 RID: 21477 RVA: 0x00180274 File Offset: 0x0017E474
		public unsafe void UpdateTexture3D(uint width, uint height, uint depth, ushort[] data)
		{
			Debug.Assert(this.TextureType == Texture.TextureTypes.Texture3D);
			bool flag = (ulong)width != (ulong)((long)this._width) || (ulong)height != (ulong)((long)this._height) || (ulong)depth != (ulong)((long)this._depth);
			bool flag2 = flag;
			if (flag2)
			{
				this._width = (int)width;
				this._height = (int)height;
				this._depth = (int)depth;
			}
			Texture._gl.BindTexture(GL.TEXTURE_3D, this.GLTexture);
			fixed (ushort[] array = data)
			{
				ushort* value;
				if (data == null || array.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array[0];
				}
				bool flag3 = flag;
				if (flag3)
				{
					Texture._gl.TexImage3D(GL.TEXTURE_3D, 0, (int)this._internalFormat, (int)width, (int)height, (int)depth, 0, this._format, this._type, (IntPtr)((void*)value));
				}
				else
				{
					Texture._gl.TexSubImage3D(GL.TEXTURE_3D, 0, 0, 0, 0, (int)width, (int)height, (int)depth, this._format, this._type, (IntPtr)((void*)value));
				}
			}
			bool requestMipMapChain = this._requestMipMapChain;
			if (requestMipMapChain)
			{
				Texture._gl.GenerateMipmap(GL.TEXTURE_3D);
			}
		}

		// Token: 0x060053E6 RID: 21478 RVA: 0x00180398 File Offset: 0x0017E598
		public unsafe void CreateTexture2DArray(int width, int height, int layerCount, byte[] pixels = null, GL minFilter = GL.NEAREST, GL magFilter = GL.NEAREST, GL wrapS = GL.CLAMP_TO_EDGE, GL wrapT = GL.CLAMP_TO_EDGE, GL internalFormat = GL.RGBA, GL format = GL.RGBA, GL type = GL.UNSIGNED_BYTE, bool requestMipMapChain = false)
		{
			Debug.Assert(this.TextureType == Texture.TextureTypes.Texture2DArray);
			this._width = width;
			this._height = height;
			this._layerCount = layerCount;
			this._internalFormat = internalFormat;
			this._format = format;
			this._type = type;
			this._requestMipMapChain = requestMipMapChain;
			this.GLTexture = Texture._gl.GenTexture();
			Texture._gl.BindTexture(GL.TEXTURE_2D_ARRAY, this.GLTexture);
			Texture._gl.TexParameteri(GL.TEXTURE_2D_ARRAY, GL.TEXTURE_MIN_FILTER, (int)minFilter);
			Texture._gl.TexParameteri(GL.TEXTURE_2D_ARRAY, GL.TEXTURE_MAG_FILTER, (int)magFilter);
			Texture._gl.TexParameteri(GL.TEXTURE_2D_ARRAY, GL.TEXTURE_WRAP_S, (int)wrapS);
			Texture._gl.TexParameteri(GL.TEXTURE_2D_ARRAY, GL.TEXTURE_WRAP_T, (int)wrapT);
			fixed (byte[] array = pixels)
			{
				byte* value;
				if (pixels == null || array.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array[0];
				}
				Texture._gl.TexImage3D(GL.TEXTURE_2D_ARRAY, 0, (int)internalFormat, width, height, layerCount, 0, format, type, (IntPtr)((void*)value));
			}
			bool requestMipMapChain2 = this._requestMipMapChain;
			if (requestMipMapChain2)
			{
				Texture._gl.GenerateMipmap(GL.TEXTURE_2D_ARRAY);
			}
		}

		// Token: 0x060053E7 RID: 21479 RVA: 0x001804E4 File Offset: 0x0017E6E4
		public unsafe void UpdateTexture2DArray(int width, int height, int layerCount, byte[] pixels)
		{
			Debug.Assert(this.TextureType == Texture.TextureTypes.Texture2DArray);
			bool flag = width != this._width || height != this._height || layerCount != this._layerCount;
			bool flag2 = flag;
			if (flag2)
			{
				this._width = width;
				this._height = height;
				this._layerCount = layerCount;
			}
			Texture._gl.BindTexture(GL.TEXTURE_2D_ARRAY, this.GLTexture);
			fixed (byte[] array = pixels)
			{
				byte* value;
				if (pixels == null || array.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array[0];
				}
				bool flag3 = flag;
				if (flag3)
				{
					Texture._gl.TexImage3D(GL.TEXTURE_2D_ARRAY, 0, (int)this._internalFormat, width, height, layerCount, 0, this._format, this._type, (IntPtr)((void*)value));
				}
				else
				{
					Texture._gl.TexSubImage3D(GL.TEXTURE_2D_ARRAY, 0, 0, 0, 0, width, height, layerCount, this._format, this._type, (IntPtr)((void*)value));
				}
			}
			bool requestMipMapChain = this._requestMipMapChain;
			if (requestMipMapChain)
			{
				Texture._gl.GenerateMipmap(GL.TEXTURE_2D_ARRAY);
			}
		}

		// Token: 0x060053E8 RID: 21480 RVA: 0x00180600 File Offset: 0x0017E800
		public unsafe void UpdateTexture2DArrayLayer(Texture texture, int layer)
		{
			Debug.Assert(this.TextureType == Texture.TextureTypes.Texture2DArray);
			Debug.Assert(texture.Width == this._width && texture.Height == this._height && texture._format == this._format && layer <= this._layerCount);
			byte[] array = new byte[this._width * this._height * 4];
			Texture._gl.BindTexture(GL.TEXTURE_2D, texture.GLTexture);
			byte[] array2;
			byte* value;
			if ((array2 = array) == null || array2.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array2[0];
			}
			Texture._gl.GetTexImage(GL.TEXTURE_2D, 0, GL.RGBA, GL.UNSIGNED_BYTE, (IntPtr)((void*)value));
			array2 = null;
			Texture._gl.BindTexture(GL.TEXTURE_2D_ARRAY, this.GLTexture);
			byte* value2;
			if ((array2 = array) == null || array2.Length == 0)
			{
				value2 = null;
			}
			else
			{
				value2 = &array2[0];
			}
			Texture._gl.TexSubImage3D(GL.TEXTURE_2D_ARRAY, 0, 0, 0, layer, this._width, this._height, 1, this._format, this._type, (IntPtr)((void*)value2));
			array2 = null;
			bool requestMipMapChain = this._requestMipMapChain;
			if (requestMipMapChain)
			{
				Texture._gl.GenerateMipmap(GL.TEXTURE_2D_ARRAY);
			}
		}

		// Token: 0x060053E9 RID: 21481 RVA: 0x00180754 File Offset: 0x0017E954
		protected override void DoDispose()
		{
			this.DestroyGPUData();
		}

		// Token: 0x060053EA RID: 21482 RVA: 0x0018075E File Offset: 0x0017E95E
		public void DestroyGPUData()
		{
			Texture._gl.DeleteTexture(this.GLTexture);
		}

		// Token: 0x060053EB RID: 21483 RVA: 0x00180774 File Offset: 0x0017E974
		public static byte[][] BuildMipmapPixels(byte[] atlasPixels, int width, int mipmapLevelCount)
		{
			int num = atlasPixels.Length / width / 4;
			byte[][] array = new byte[mipmapLevelCount + 1][];
			array[0] = atlasPixels;
			int num2 = width;
			int num3 = num;
			byte[] array2 = array[0];
			for (int i = 1; i <= mipmapLevelCount; i++)
			{
				int num4 = num2;
				num2 = Math.Max(num2 / 2, 1);
				num3 = Math.Max(num3 / 2, 1);
				byte[] array3 = array[i] = new byte[num2 * num3 * 4];
				for (int j = 0; j < num3; j++)
				{
					for (int k = 0; k < num2; k++)
					{
						int num5 = j * 2 * num4 + k * 2;
						Vector4 vector = new Vector4((float)array2[num5 * 4], (float)array2[num5 * 4 + 1], (float)array2[num5 * 4 + 2], (float)array2[num5 * 4 + 3]);
						int num6 = j * 2 * num4 + k * 2 + 1;
						Vector4 vector2 = new Vector4((float)array2[num6 * 4], (float)array2[num6 * 4 + 1], (float)array2[num6 * 4 + 2], (float)array2[num6 * 4 + 3]);
						int num7 = (j * 2 + 1) * num4 + k * 2;
						Vector4 vector3 = new Vector4((float)array2[num7 * 4], (float)array2[num7 * 4 + 1], (float)array2[num7 * 4 + 2], (float)array2[num7 * 4 + 3]);
						int num8 = (j * 2 + 1) * num4 + k * 2 + 1;
						Vector4 vector4 = new Vector4((float)array2[num8 * 4], (float)array2[num8 * 4 + 1], (float)array2[num8 * 4 + 2], (float)array2[num8 * 4 + 3]);
						bool flag = vector.W == 0f;
						if (flag)
						{
							vector = vector2;
						}
						else
						{
							bool flag2 = vector2.W == 0f;
							if (flag2)
							{
								vector2 = vector;
							}
						}
						Vector4 vector5 = Vector4.Lerp(vector, vector2, 0.5f);
						bool flag3 = vector3.W == 0f;
						if (flag3)
						{
							vector3 = vector4;
						}
						else
						{
							bool flag4 = vector4.W == 0f;
							if (flag4)
							{
								vector4 = vector3;
							}
						}
						Vector4 vector6 = Vector4.Lerp(vector3, vector4, 0.5f);
						bool flag5 = vector5.W == 0f;
						if (flag5)
						{
							vector5 = vector6;
						}
						else
						{
							bool flag6 = vector6.W == 0f;
							if (flag6)
							{
								vector6 = vector5;
							}
						}
						Vector4 vector7 = Vector4.Lerp(vector5, vector6, 0.5f);
						int num9 = j * num2 + k;
						array3[num9 * 4] = (byte)Math.Round((double)vector7.X);
						array3[num9 * 4 + 1] = (byte)Math.Round((double)vector7.Y);
						array3[num9 * 4 + 2] = (byte)Math.Round((double)vector7.Z);
						array3[num9 * 4 + 3] = (byte)Math.Round((double)vector7.W);
					}
				}
				array2 = array3;
			}
			return array;
		}

		// Token: 0x04002ED1 RID: 11985
		public readonly Texture.TextureTypes TextureType;

		// Token: 0x04002ED2 RID: 11986
		public const int DefaultAtlasWidth = 2048;

		// Token: 0x04002ED3 RID: 11987
		public const int MinimumAtlasHeight = 32;

		// Token: 0x04002ED4 RID: 11988
		public const int DefaultMipmapLevelCount = 5;

		// Token: 0x04002ED6 RID: 11990
		protected static GLFunctions _gl;

		// Token: 0x04002ED7 RID: 11991
		private int _width;

		// Token: 0x04002ED8 RID: 11992
		private int _height;

		// Token: 0x04002ED9 RID: 11993
		private int _depth;

		// Token: 0x04002EDA RID: 11994
		private int _layerCount;

		// Token: 0x04002EDB RID: 11995
		private int _mipmapLevelCount;

		// Token: 0x04002EDC RID: 11996
		private GL _internalFormat;

		// Token: 0x04002EDD RID: 11997
		private GL _format;

		// Token: 0x04002EDE RID: 11998
		private GL _type;

		// Token: 0x04002EDF RID: 11999
		private bool _requestMipMapChain;

		// Token: 0x02000ECD RID: 3789
		public enum TextureTypes
		{
			// Token: 0x0400489F RID: 18591
			Texture2D,
			// Token: 0x040048A0 RID: 18592
			Texture2DArray,
			// Token: 0x040048A1 RID: 18593
			Texture3D,
			// Token: 0x040048A2 RID: 18594
			TextureCubemap
		}
	}
}
