using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Math;
using HytaleClient.Utils;
using Microsoft.CSharp.RuntimeBinder;
using NLog;
using SDL2;
using Utf8Json;

namespace HytaleClient.Graphics.Fonts
{
	// Token: 0x02000ABE RID: 2750
	public class Font : Disposable
	{
		// Token: 0x17001340 RID: 4928
		// (get) Token: 0x060056B7 RID: 22199 RVA: 0x001A0749 File Offset: 0x0019E949
		// (set) Token: 0x060056B8 RID: 22200 RVA: 0x001A0751 File Offset: 0x0019E951
		public float FallbackGlyphAdvance { get; private set; }

		// Token: 0x17001341 RID: 4929
		// (get) Token: 0x060056B9 RID: 22201 RVA: 0x001A075A File Offset: 0x0019E95A
		// (set) Token: 0x060056BA RID: 22202 RVA: 0x001A0762 File Offset: 0x0019E962
		public Rectangle FallbackGlyphAtlasRectangle { get; private set; }

		// Token: 0x17001342 RID: 4930
		// (get) Token: 0x060056BB RID: 22203 RVA: 0x001A076B File Offset: 0x0019E96B
		// (set) Token: 0x060056BC RID: 22204 RVA: 0x001A0773 File Offset: 0x0019E973
		public Texture TextureAtlas { get; private set; }

		// Token: 0x060056BD RID: 22205 RVA: 0x001A077C File Offset: 0x0019E97C
		public Font(GraphicsDevice graphics, string basePath, int fontId, int baseSize = 32, int sdfScaledownFactor = 8, int sdfSpread = 8, Font.CharacterRange[] initialCharacterRanges = null)
		{
			this._graphics = graphics;
			this.FontId = fontId;
			this.BaseSize = baseSize;
			this.Spread = sdfSpread;
			this.ScaledownFactor = sdfScaledownFactor;
			string text = basePath + ".ttf";
			this._ttfFont = SDL_ttf.TTF_OpenFont(text, this.BaseSize * sdfScaledownFactor);
			bool flag = this._ttfFont == IntPtr.Zero;
			if (flag)
			{
				Font.Logger.Error(SDL.SDL_GetError());
				throw new Exception("Could not open font from: " + text);
			}
			this.Name = SDL_ttf.TTF_FontFaceFamilyName(this._ttfFont);
			this.Height = SDL_ttf.TTF_FontHeight(this._ttfFont) / sdfScaledownFactor;
			this.LineSkip = SDL_ttf.TTF_FontLineSkip(this._ttfFont) / sdfScaledownFactor;
			this.Ascent = SDL_ttf.TTF_FontAscent(this._ttfFont) / sdfScaledownFactor;
			this.Descent = SDL_ttf.TTF_FontDescent(this._ttfFont) / sdfScaledownFactor;
			string text2 = basePath + "Atlas.png";
			string text3 = basePath + "Glyphs.json";
			this._width = 2048;
			this._height = 2048;
			bool flag2 = File.Exists(text2) && File.Exists(text3);
			if (flag2)
			{
				Image image = null;
				try
				{
					image = new Image(File.ReadAllBytes(text2));
					this._initialPixels = image.Pixels;
					object arg = JsonSerializer.Deserialize<object>(File.ReadAllBytes(text3));
					if (Font.<>o__36.<>p__10 == null)
					{
						Font.<>o__36.<>p__10 = CallSite<Func<CallSite, object, IDictionary<string, object>>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(IDictionary<string, object>), typeof(Font)));
					}
					foreach (KeyValuePair<string, object> keyValuePair in Font.<>o__36.<>p__10.Target(Font.<>o__36.<>p__10, arg))
					{
						ushort num = ushort.Parse(keyValuePair.Key, CultureInfo.InvariantCulture);
						Dictionary<ushort, float> glyphAdvances = this.GlyphAdvances;
						ushort key = num;
						if (Font.<>o__36.<>p__1 == null)
						{
							Font.<>o__36.<>p__1 = CallSite<Func<CallSite, object, float>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(float), typeof(Font)));
						}
						Func<CallSite, object, float> target = Font.<>o__36.<>p__1.Target;
						CallSite <>p__ = Font.<>o__36.<>p__1;
						if (Font.<>o__36.<>p__0 == null)
						{
							Font.<>o__36.<>p__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Font), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
							}));
						}
						glyphAdvances[key] = target(<>p__, Font.<>o__36.<>p__0.Target(Font.<>o__36.<>p__0, keyValuePair.Value, "Advance"));
						if (Font.<>o__36.<>p__3 == null)
						{
							Font.<>o__36.<>p__3 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(int), typeof(Font)));
						}
						Func<CallSite, object, int> target2 = Font.<>o__36.<>p__3.Target;
						CallSite <>p__2 = Font.<>o__36.<>p__3;
						if (Font.<>o__36.<>p__2 == null)
						{
							Font.<>o__36.<>p__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Font), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
							}));
						}
						int num2 = target2(<>p__2, Font.<>o__36.<>p__2.Target(Font.<>o__36.<>p__2, keyValuePair.Value, "Y"));
						if (Font.<>o__36.<>p__5 == null)
						{
							Font.<>o__36.<>p__5 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(int), typeof(Font)));
						}
						Func<CallSite, object, int> target3 = Font.<>o__36.<>p__5.Target;
						CallSite <>p__3 = Font.<>o__36.<>p__5;
						if (Font.<>o__36.<>p__4 == null)
						{
							Font.<>o__36.<>p__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Font), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
							}));
						}
						int num3 = target3(<>p__3, Font.<>o__36.<>p__4.Target(Font.<>o__36.<>p__4, keyValuePair.Value, "Height"));
						Dictionary<ushort, Rectangle> glyphAtlasRectangles = this.GlyphAtlasRectangles;
						ushort key2 = num;
						if (Font.<>o__36.<>p__7 == null)
						{
							Font.<>o__36.<>p__7 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(int), typeof(Font)));
						}
						Func<CallSite, object, int> target4 = Font.<>o__36.<>p__7.Target;
						CallSite <>p__4 = Font.<>o__36.<>p__7;
						if (Font.<>o__36.<>p__6 == null)
						{
							Font.<>o__36.<>p__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Font), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
							}));
						}
						int x = target4(<>p__4, Font.<>o__36.<>p__6.Target(Font.<>o__36.<>p__6, keyValuePair.Value, "X"));
						int y = num2;
						if (Font.<>o__36.<>p__9 == null)
						{
							Font.<>o__36.<>p__9 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(int), typeof(Font)));
						}
						Func<CallSite, object, int> target5 = Font.<>o__36.<>p__9.Target;
						CallSite <>p__5 = Font.<>o__36.<>p__9;
						if (Font.<>o__36.<>p__8 == null)
						{
							Font.<>o__36.<>p__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Font), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
							}));
						}
						glyphAtlasRectangles[key2] = new Rectangle(x, y, target5(<>p__5, Font.<>o__36.<>p__8.Target(Font.<>o__36.<>p__8, keyValuePair.Value, "Width")), num3);
						this._glyphLowestY = Math.Max(this._glyphLowestY, num2 + num3);
					}
					this._nextGlyphLocation = new Point(0, this._glyphLowestY);
				}
				catch (Exception exception)
				{
					bool flag3 = image == null;
					if (flag3)
					{
						Font.Logger.Error(exception, "Failed to load font atlas: " + text2);
					}
					else
					{
						Font.Logger.Error(exception, "Failed to load glyph file: " + text3);
					}
				}
			}
			this._initialCharacterRanges = (initialCharacterRanges ?? Font.DefaultCharacterRanges);
		}

		// Token: 0x060056BE RID: 22206 RVA: 0x001A0D8C File Offset: 0x0019EF8C
		public void BuildTexture()
		{
			bool flag = this._initialPixels == null;
			if (flag)
			{
				this._initialPixels = new byte[this._width * this._height * 4];
				for (int i = 0; i < this._initialPixels.Length; i++)
				{
					this._initialPixels[i] = byte.MaxValue;
				}
			}
			this.TextureAtlas = new Texture(Texture.TextureTypes.Texture2D);
			this.TextureAtlas.CreateTexture2D(this._width, this._height, this._initialPixels, 5, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
			this._initialPixels = null;
			HashSet<ushort> hashSet = new HashSet<ushort>();
			foreach (Font.CharacterRange characterRange in this._initialCharacterRanges)
			{
				for (ushort num = characterRange.Start; num <= characterRange.End; num += 1)
				{
					bool flag2 = !this.GlyphAtlasRectangles.ContainsKey(num);
					if (flag2)
					{
						hashSet.Add(num);
					}
				}
			}
			bool flag3 = hashSet.Count > 0;
			if (flag3)
			{
				this.AddGlyphs(hashSet);
			}
			this.FallbackGlyphAdvance = this.GlyphAdvances[63];
			this.FallbackGlyphAtlasRectangle = this.GlyphAtlasRectangles[63];
		}

		// Token: 0x060056BF RID: 22207 RVA: 0x001A0EF4 File Offset: 0x0019F0F4
		public unsafe void WriteCacheToDisk(string basePath)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			string path = basePath + "Atlas.png";
			string text = basePath + "Glyphs.json";
			byte[] array = new byte[this.TextureAtlas.Width * this.TextureAtlas.Height * 4];
			this._graphics.GL.BindTexture(GL.TEXTURE_2D, this.TextureAtlas.GLTexture);
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
			this._graphics.GL.GetTexImage(GL.TEXTURE_2D, 0, GL.RGBA, GL.UNSIGNED_BYTE, (IntPtr)((void*)value));
			array2 = null;
			new Image(this.TextureAtlas.Width, this.TextureAtlas.Height, array).SavePNG(path, 255U, 65280U, 16711680U, 4278190080U);
			Dictionary<ushort, Dictionary<string, object>> dictionary = new Dictionary<ushort, Dictionary<string, object>>();
			foreach (ushort key in this.GlyphAtlasRectangles.Keys)
			{
				Rectangle rectangle = this.GlyphAtlasRectangles[key];
				Dictionary<string, object> value2 = new Dictionary<string, object>
				{
					{
						"Advance",
						this.GlyphAdvances[key]
					},
					{
						"X",
						rectangle.X
					},
					{
						"Y",
						rectangle.Y
					},
					{
						"Width",
						rectangle.Width
					},
					{
						"Height",
						rectangle.Height
					}
				};
				dictionary.Add(key, value2);
			}
			File.WriteAllBytes(text, JsonSerializer.Serialize<Dictionary<ushort, Dictionary<string, object>>>(dictionary));
		}

		// Token: 0x060056C0 RID: 22208 RVA: 0x001A10F0 File Offset: 0x0019F2F0
		protected override void DoDispose()
		{
			Texture textureAtlas = this.TextureAtlas;
			if (textureAtlas != null)
			{
				textureAtlas.Dispose();
			}
			bool flag = this._ttfFont != IntPtr.Zero;
			if (flag)
			{
				SDL_ttf.TTF_CloseFont(this._ttfFont);
			}
		}

		// Token: 0x060056C1 RID: 22209 RVA: 0x001A1130 File Offset: 0x0019F330
		public void BuildMissingGlyphs()
		{
			bool flag = this._missingGlyphs.Count > 0;
			if (flag)
			{
				this.AddGlyphs(this._missingGlyphs);
			}
			this._missingGlyphs.Clear();
		}

		// Token: 0x060056C2 RID: 22210 RVA: 0x001A116C File Offset: 0x0019F36C
		private unsafe void AddGlyphs(IEnumerable<ushort> characters)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this._graphics.GL.BindTexture(GL.TEXTURE_2D, this.TextureAtlas.GLTexture);
			SDL.SDL_Color fg = new SDL.SDL_Color
			{
				r = 0,
				g = 0,
				b = 0,
				a = byte.MaxValue
			};
			foreach (ushort num in characters)
			{
				bool flag = SDL_ttf.TTF_GlyphIsProvided(this._ttfFont, num) == 0;
				if (flag)
				{
					this.GlyphAdvances[num] = this.FallbackGlyphAdvance;
					this.GlyphAtlasRectangles[num] = this.FallbackGlyphAtlasRectangle;
				}
				else
				{
					int num2;
					int num3;
					int num4;
					int num5;
					int num6;
					SDL_ttf.TTF_GlyphMetrics(this._ttfFont, num, out num2, out num3, out num4, out num5, out num6);
					this.GlyphAdvances[num] = (float)num6 / (float)this.ScaledownFactor;
					IntPtr intPtr = SDL_ttf.TTF_RenderGlyph_Solid(this._ttfFont, num, fg);
					SDL.SDL_LockSurface(intPtr);
					SDL.SDL_Surface sdl_Surface = (SDL.SDL_Surface)Marshal.PtrToStructure(intPtr, typeof(SDL.SDL_Surface));
					byte[] array = new byte[sdl_Surface.w * sdl_Surface.h];
					for (int i = 0; i < sdl_Surface.h; i++)
					{
						Marshal.Copy(sdl_Surface.pixels + i * sdl_Surface.pitch, array, i * sdl_Surface.w, sdl_Surface.w);
					}
					int num7;
					int num8;
					byte[] pixels = SignedDistanceField.Generate(array, sdl_Surface.w, sdl_Surface.h, this.ScaledownFactor, this.Spread, 1, out num7, out num8);
					SDL.SDL_UnlockSurface(intPtr);
					SDL.SDL_FreeSurface(intPtr);
					int num9 = this._nextGlyphLocation.X + num7;
					bool flag2 = num9 >= this.TextureAtlas.Width;
					if (flag2)
					{
						this._nextGlyphLocation.Y = this._glyphLowestY;
						this._nextGlyphLocation.X = 0;
					}
					int num10 = this._nextGlyphLocation.Y + num8;
					bool flag3 = num10 >= this.TextureAtlas.Height;
					if (flag3)
					{
						bool flag4 = !this._hasWarnedAboutFullAtlas;
						if (flag4)
						{
							Font.Logger.Warn("{0} font atlas is full, glyph can't be added. Must implement resizing.", this.Name);
						}
						this._hasWarnedAboutFullAtlas = true;
						this.GlyphAdvances[num] = this.FallbackGlyphAdvance;
						this.GlyphAtlasRectangles[num] = this.FallbackGlyphAtlasRectangle;
					}
					else
					{
						Image image = new Image(string.Format("Character {0}", (char)num), num7, num8, pixels);
						Rectangle rectangle = this.GlyphAtlasRectangles[num] = new Rectangle(this._nextGlyphLocation.X, this._nextGlyphLocation.Y, num7, num8);
						this._nextGlyphLocation.X = this._nextGlyphLocation.X + num7;
						this._glyphLowestY = this._nextGlyphLocation.Y + num8;
						try
						{
							byte[] array2;
							byte* value;
							if ((array2 = image.Pixels) == null || array2.Length == 0)
							{
								value = null;
							}
							else
							{
								value = &array2[0];
							}
							this._graphics.GL.TexSubImage2D(GL.TEXTURE_2D, 0, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, GL.RGBA, GL.UNSIGNED_BYTE, (IntPtr)((void*)value));
						}
						finally
						{
							byte[] array2 = null;
						}
					}
				}
			}
		}

		// Token: 0x060056C3 RID: 22211 RVA: 0x001A1520 File Offset: 0x0019F720
		public float GetCharacterAdvance(ushort character)
		{
			float result;
			bool flag = !this.GlyphAdvances.TryGetValue(character, out result);
			if (flag)
			{
				bool flag2 = SDL_ttf.TTF_GlyphIsProvided(this._ttfFont, character) == 0;
				if (flag2)
				{
					result = (this.GlyphAdvances[character] = this.FallbackGlyphAdvance);
					this.GlyphAtlasRectangles[character] = this.FallbackGlyphAtlasRectangle;
				}
				else
				{
					int num;
					int num2;
					int num3;
					int num4;
					int num5;
					SDL_ttf.TTF_GlyphMetrics(this._ttfFont, character, out num, out num2, out num3, out num4, out num5);
					result = (this.GlyphAdvances[character] = (float)num5 / (float)this.ScaledownFactor);
					this._missingGlyphs.Add(character);
				}
			}
			return result;
		}

		// Token: 0x060056C4 RID: 22212 RVA: 0x001A15D0 File Offset: 0x0019F7D0
		public float CalculateTextWidth(string text)
		{
			float num = 0f;
			foreach (ushort character in text)
			{
				num += this.GetCharacterAdvance(character);
			}
			return num;
		}

		// Token: 0x04003426 RID: 13350
		public static Font.CharacterRange[] DefaultCharacterRanges = new Font.CharacterRange[]
		{
			new Font.CharacterRange(32, 127),
			new Font.CharacterRange(160, 255),
			new Font.CharacterRange(1024, 1279)
		};

		// Token: 0x04003427 RID: 13351
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003428 RID: 13352
		public readonly string Name;

		// Token: 0x04003429 RID: 13353
		public readonly int Height;

		// Token: 0x0400342A RID: 13354
		public readonly int LineSkip;

		// Token: 0x0400342B RID: 13355
		public readonly int Ascent;

		// Token: 0x0400342C RID: 13356
		public readonly int Descent;

		// Token: 0x0400342D RID: 13357
		public readonly int FontId;

		// Token: 0x0400342E RID: 13358
		public readonly int BaseSize;

		// Token: 0x0400342F RID: 13359
		public readonly int Spread;

		// Token: 0x04003430 RID: 13360
		public readonly int ScaledownFactor;

		// Token: 0x04003431 RID: 13361
		public readonly Dictionary<ushort, float> GlyphAdvances = new Dictionary<ushort, float>();

		// Token: 0x04003432 RID: 13362
		public readonly Dictionary<ushort, Rectangle> GlyphAtlasRectangles = new Dictionary<ushort, Rectangle>();

		// Token: 0x04003436 RID: 13366
		private readonly GraphicsDevice _graphics;

		// Token: 0x04003437 RID: 13367
		private readonly IntPtr _ttfFont;

		// Token: 0x04003438 RID: 13368
		private Point _nextGlyphLocation;

		// Token: 0x04003439 RID: 13369
		private int _glyphLowestY;

		// Token: 0x0400343A RID: 13370
		private readonly HashSet<ushort> _missingGlyphs = new HashSet<ushort>();

		// Token: 0x0400343B RID: 13371
		private bool _hasWarnedAboutFullAtlas;

		// Token: 0x0400343C RID: 13372
		private int _width;

		// Token: 0x0400343D RID: 13373
		private int _height;

		// Token: 0x0400343E RID: 13374
		private byte[] _initialPixels;

		// Token: 0x0400343F RID: 13375
		private Font.CharacterRange[] _initialCharacterRanges;

		// Token: 0x02000F0A RID: 3850
		public struct CharacterRange
		{
			// Token: 0x06006828 RID: 26664 RVA: 0x0021A520 File Offset: 0x00218720
			public CharacterRange(ushort start, ushort end)
			{
				this.Start = start;
				this.End = end;
			}

			// Token: 0x040049D5 RID: 18901
			public readonly ushort Start;

			// Token: 0x040049D6 RID: 18902
			public readonly ushort End;
		}
	}
}
