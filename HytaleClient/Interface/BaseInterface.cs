using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using HytaleClient.Audio;
using HytaleClient.Common.Collections;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Fonts;
using HytaleClient.Graphics.Programs;
using HytaleClient.Interface.CoherentUI;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using HytaleClient.Utils;
using NLog;
using SDL2;

namespace HytaleClient.Interface
{
	// Token: 0x020007FF RID: 2047
	internal abstract class BaseInterface : Disposable, IUIProvider
	{
		// Token: 0x17001004 RID: 4100
		// (get) Token: 0x0600387E RID: 14462 RVA: 0x00073C5F File Offset: 0x00071E5F
		// (set) Token: 0x0600387F RID: 14463 RVA: 0x00073C67 File Offset: 0x00071E67
		public BaseInterface.InterfaceFadeState FadeState { get; private set; }

		// Token: 0x06003880 RID: 14464 RVA: 0x00073C70 File Offset: 0x00071E70
		protected BaseInterface(Engine engine, FontManager fonts, CoUIManager coUiManager, string resourcesPath, bool isDevModeEnabled)
		{
			this.Engine = engine;
			this.CoUiManager = coUiManager;
			this._fonts = fonts;
			this._resourcesPath = resourcesPath;
			this._isDevModeEnabled = isDevModeEnabled;
			this.Desktop = new Desktop(this, this.Engine.Graphics, this.Engine.Graphics.Batcher2D);
			bool isDevModeEnabled2 = this._isDevModeEnabled;
			if (isDevModeEnabled2)
			{
				this._markupErrorDesktop = new Desktop(this, this.Engine.Graphics, this.Engine.Graphics.Batcher2D);
				this._markupErrorLayer = new MarkupErrorOverlay(this._markupErrorDesktop, null, "UI – Markup Error");
				FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(this._resourcesPath)
				{
					IncludeSubdirectories = true
				};
				fileSystemWatcher.Changed += new FileSystemEventHandler(this.OnFilesChanged);
				fileSystemWatcher.Deleted += new FileSystemEventHandler(this.OnFilesChanged);
				fileSystemWatcher.Created += new FileSystemEventHandler(this.OnFilesChanged);
				fileSystemWatcher.EnableRaisingEvents = true;
			}
			this.SetupViewports();
		}

		// Token: 0x06003881 RID: 14465 RVA: 0x00073DB1 File Offset: 0x00071FB1
		private void OnFilesChanged(object sender, FileSystemEventArgs e)
		{
			this._watchDelayTimer = 0.25f;
		}

		// Token: 0x06003882 RID: 14466 RVA: 0x00073DC0 File Offset: 0x00071FC0
		public void LoadAndBuild()
		{
			bool flag = this._isDevModeEnabled && this.HasMarkupError;
			if (flag)
			{
				this._markupErrorLayer.Clear();
				this._markupErrorDesktop.ClearAllLayers();
				this.HasMarkupError = false;
			}
			this.Desktop.ClearInput(true);
			this.LoadTextures(this.Desktop.Scale > 1f);
			try
			{
				this.LoadDocuments();
			}
			catch (TextParser.TextParserException ex)
			{
				bool flag2 = !this._isDevModeEnabled;
				if (flag2)
				{
					throw ex;
				}
				this.<LoadAndBuild>g__DisplayError|30_0(ex.RawMessage, ex.Span);
				return;
			}
			this.Build();
			this.Desktop.Layout();
			this.HasLoaded = true;
			BaseInterface.Logger.Info("Interface loaded.");
		}

		// Token: 0x06003883 RID: 14467
		protected abstract void Build();

		// Token: 0x06003884 RID: 14468 RVA: 0x00073E98 File Offset: 0x00072098
		protected override void DoDispose()
		{
			this.Desktop.ClearAllLayers();
			bool isDevModeEnabled = this._isDevModeEnabled;
			if (isDevModeEnabled)
			{
				this._markupErrorDesktop.ClearAllLayers();
			}
			this.DisposeTextures();
		}

		// Token: 0x06003885 RID: 14469 RVA: 0x00073ECF File Offset: 0x000720CF
		protected virtual float GetScale()
		{
			return this.Engine.Window.ViewportScale;
		}

		// Token: 0x06003886 RID: 14470 RVA: 0x00073EE4 File Offset: 0x000720E4
		public void OnWindowSizeChanged()
		{
			float scale = this.GetScale();
			bool flag = scale > 1f;
			bool flag2 = this.Desktop.Scale > 1f;
			bool flag3 = flag != flag2;
			if (flag3)
			{
				this.LoadTextures(flag);
			}
			this.SetupViewports();
		}

		// Token: 0x06003887 RID: 14471 RVA: 0x00073F34 File Offset: 0x00072134
		private void SetupViewports()
		{
			float scale = this.GetScale();
			this.Desktop.SetViewport(this.Engine.Window.Viewport, scale);
			bool isDevModeEnabled = this._isDevModeEnabled;
			if (isDevModeEnabled)
			{
				this._markupErrorDesktop.SetViewport(this.Engine.Window.Viewport, scale);
			}
		}

		// Token: 0x06003888 RID: 14472 RVA: 0x00073F8F File Offset: 0x0007218F
		public void CancelOnFadeComplete()
		{
			this._onFadeComplete = null;
		}

		// Token: 0x06003889 RID: 14473 RVA: 0x00073F9C File Offset: 0x0007219C
		public void FadeIn(Action onComplete = null, bool longFade = false)
		{
			bool flag = this._onFadeComplete != null;
			if (flag)
			{
				throw new InvalidOperationException("Cannot start a fade in while a fade completion callback is pending.");
			}
			this.Desktop.ClearInput(false);
			this.FadeState = BaseInterface.InterfaceFadeState.FadingIn;
			this._fadeTimer = 0f;
			this._fadeDuration = (longFade ? 1f : 0.15f);
			this._onFadeComplete = onComplete;
		}

		// Token: 0x0600388A RID: 14474 RVA: 0x00074000 File Offset: 0x00072200
		public void FadeOut(Action onComplete = null)
		{
			bool flag = this._onFadeComplete != null;
			if (flag)
			{
				throw new InvalidOperationException("Cannot start a fade out while a fade completion callback is pending.");
			}
			this.Desktop.ClearInput(false);
			this.FadeState = BaseInterface.InterfaceFadeState.FadingOut;
			this._fadeTimer = 0f;
			this._fadeDuration = 0.15f;
			this._onFadeComplete = onComplete;
		}

		// Token: 0x0600388B RID: 14475 RVA: 0x00074058 File Offset: 0x00072258
		public void ClearFlash()
		{
			this._flashTimer = -1f;
		}

		// Token: 0x0600388C RID: 14476 RVA: 0x00074065 File Offset: 0x00072265
		public void Flash()
		{
			this._flashTimer = 0f;
		}

		// Token: 0x0600388D RID: 14477 RVA: 0x00074072 File Offset: 0x00072272
		protected virtual void SetDrawOutlines(bool draw)
		{
			this.Desktop.DrawOutlines = !this.Desktop.DrawOutlines;
		}

		// Token: 0x0600388E RID: 14478 RVA: 0x0007408E File Offset: 0x0007228E
		public FontFamily GetFontFamily(string name)
		{
			return this._fonts.GetFontFamilyByName(name);
		}

		// Token: 0x0600388F RID: 14479 RVA: 0x0007409C File Offset: 0x0007229C
		public unsafe void OnUserInput(SDL.SDL_Event evt)
		{
			bool isDevModeEnabled = this._isDevModeEnabled;
			if (isDevModeEnabled)
			{
				bool flag = evt.type == SDL.SDL_EventType.SDL_KEYDOWN;
				if (flag)
				{
					bool flag2 = evt.key.keysym.sym == SDL.SDL_Keycode.SDLK_F8 && this.Desktop.IsShortcutKeyDown;
					if (flag2)
					{
						this.SetDrawOutlines(!this.Desktop.DrawOutlines);
					}
				}
				bool hasMarkupError = this.HasMarkupError;
				if (hasMarkupError)
				{
					return;
				}
			}
			bool flag3 = this.FadeState > BaseInterface.InterfaceFadeState.FadedIn;
			if (!flag3)
			{
				SDL.SDL_EventType type = evt.type;
				SDL.SDL_EventType sdl_EventType = type;
				switch (sdl_EventType)
				{
				case SDL.SDL_EventType.SDL_KEYDOWN:
					this.Desktop.OnKeyDown(evt.key.keysym.sym, (int)evt.key.repeat);
					break;
				case SDL.SDL_EventType.SDL_KEYUP:
					this.Desktop.OnKeyUp(evt.key.keysym.sym);
					break;
				case SDL.SDL_EventType.SDL_TEXTEDITING:
					break;
				case SDL.SDL_EventType.SDL_TEXTINPUT:
				{
					NativeArray<byte> nativeArray = new NativeArray<byte>(256, 1, 0);
					byte* ptr = &evt.text.text.FixedElementField;
					while (*ptr > 0)
					{
						ptr++;
					}
					int num = (int)((long)(ptr - &evt.text.text.FixedElementField));
					NativeArray<byte>.Copy((void*)(&evt.text.text.FixedElementField), 0, nativeArray, 0, num);
					string @string = Encoding.UTF8.GetString((byte*)nativeArray.GetBuffer(), num);
					this.Desktop.OnTextInput(@string);
					break;
				}
				default:
					switch (sdl_EventType)
					{
					case SDL.SDL_EventType.SDL_MOUSEMOTION:
						this.Desktop.OnMouseMove(this.Engine.Window.TransformSDLToViewportCoords(evt.motion.x, evt.motion.y));
						break;
					case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
						this.Desktop.OnMouseDown((int)evt.button.button, (int)evt.button.clicks);
						break;
					case SDL.SDL_EventType.SDL_MOUSEBUTTONUP:
						this.Desktop.OnMouseUp((int)evt.button.button, (int)evt.button.clicks);
						break;
					case SDL.SDL_EventType.SDL_MOUSEWHEEL:
					{
						bool isShiftKeyDown = this.Desktop.IsShiftKeyDown;
						if (isShiftKeyDown)
						{
							this.Desktop.OnMouseWheel(new Point(evt.wheel.y, evt.wheel.x));
						}
						else
						{
							this.Desktop.OnMouseWheel(new Point(evt.wheel.x, evt.wheel.y));
						}
						break;
					}
					}
					break;
				}
			}
		}

		// Token: 0x06003890 RID: 14480 RVA: 0x00074348 File Offset: 0x00072548
		public void Update(float deltaTime)
		{
			bool isDevModeEnabled = this._isDevModeEnabled;
			if (isDevModeEnabled)
			{
				bool flag = this._watchDelayTimer > 0f;
				if (flag)
				{
					this._watchDelayTimer = Math.Max(0f, this._watchDelayTimer - deltaTime);
					bool flag2 = this._watchDelayTimer == 0f;
					if (flag2)
					{
						this.LoadClientTexts();
						this.LoadAndBuild();
					}
				}
			}
			bool flag3 = this.FadeState != BaseInterface.InterfaceFadeState.FadedIn && this.FadeState != BaseInterface.InterfaceFadeState.FadedOut;
			if (flag3)
			{
				this._fadeTimer += deltaTime;
				bool flag4 = this._fadeTimer >= this._fadeDuration;
				if (flag4)
				{
					bool flag5 = this.FadeState == BaseInterface.InterfaceFadeState.FadingIn;
					if (flag5)
					{
						this.FadeState = BaseInterface.InterfaceFadeState.FadedIn;
					}
					else
					{
						this.FadeState = BaseInterface.InterfaceFadeState.FadedOut;
					}
					bool flag6 = this._onFadeComplete != null;
					if (flag6)
					{
						this.Engine.RunOnMainThread(this.Engine, this._onFadeComplete, true, false);
					}
					this._onFadeComplete = null;
				}
			}
			bool flag7 = this._flashTimer >= 0f;
			if (flag7)
			{
				this._flashTimer += deltaTime;
				bool flag8 = this._flashTimer >= 0.35f;
				if (flag8)
				{
					this._flashTimer = -1f;
				}
			}
			this.Desktop.Update(deltaTime);
		}

		// Token: 0x06003891 RID: 14481 RVA: 0x00074498 File Offset: 0x00072698
		public void PrepareForDraw()
		{
			this.Desktop.PrepareForDraw();
			bool flag = this.FadeState > BaseInterface.InterfaceFadeState.FadedIn;
			if (flag)
			{
				float num = 1f;
				float num2 = Math.Min(1f, this._fadeTimer / this._fadeDuration);
				bool flag2 = this.FadeState == BaseInterface.InterfaceFadeState.FadingIn;
				if (flag2)
				{
					num = 1f - num2 * num2;
				}
				else
				{
					bool flag3 = this.FadeState == BaseInterface.InterfaceFadeState.FadingOut;
					if (flag3)
					{
						num = num2 * num2;
					}
				}
				this.Desktop.Batcher2D.RequestDrawTexture(this.Desktop.Graphics.WhitePixelTexture, new Rectangle(0, 0, 1, 1), this.Engine.Window.Viewport, UInt32Color.FromRGBA(0, 0, 0, (byte)(255f * num)));
			}
			bool flag4 = this._flashTimer > 0f;
			if (flag4)
			{
				float num3 = 1f - this._flashTimer / 0.35f;
				this.Desktop.Batcher2D.RequestDrawTexture(this.Desktop.Graphics.WhitePixelTexture, new Rectangle(0, 0, 1, 1), this.Engine.Window.Viewport, UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(255f * num3)));
			}
			bool flag5 = this._isDevModeEnabled && this.HasMarkupError;
			if (flag5)
			{
				this._markupErrorDesktop.PrepareForDraw();
			}
		}

		// Token: 0x06003892 RID: 14482 RVA: 0x000745FC File Offset: 0x000727FC
		public void Draw()
		{
			GLFunctions gl = this.Engine.Graphics.GL;
			Matrix matrix = Matrix.CreateTranslation(0f, 0f, 5f) * Matrix.CreateOrthographicOffCenter(0f, (float)this.Engine.Window.Viewport.Width, (float)this.Engine.Window.Viewport.Height, 0f, 0.1f, 100f);
			Batcher2DProgram batcher2DProgram = this.Engine.Graphics.GPUProgramStore.Batcher2DProgram;
			gl.UseProgram(batcher2DProgram);
			batcher2DProgram.MVPMatrix.SetValue(ref matrix);
			gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
			bool flag = this._fonts.TextureArray2D != null;
			if (flag)
			{
				gl.ActiveTexture(GL.TEXTURE2);
				gl.BindTexture(GL.TEXTURE_2D_ARRAY, this._fonts.TextureArray2D.GLTexture);
				gl.ActiveTexture(GL.TEXTURE0);
			}
			this.Desktop.Batcher2D.Draw();
			gl.BlendFunc(GL.ONE, GL.ONE_MINUS_SRC_ALPHA);
			gl.UseProgram(this.Engine.Graphics.GPUProgramStore.BasicProgram);
		}

		// Token: 0x06003893 RID: 14483 RVA: 0x00074740 File Offset: 0x00072940
		private void LoadDocuments()
		{
			this._documentsLibrary.Clear();
			foreach (string text in Directory.EnumerateFiles(this._resourcesPath, "*.ui", 1))
			{
				string text2 = text.Substring(this._resourcesPath.Length + 1).Replace("\\", "/");
				Document value = DocumentParser.Parse(File.ReadAllText(text), text2);
				this._documentsLibrary.Add(text2, value);
			}
			foreach (KeyValuePair<string, Document> keyValuePair in this._documentsLibrary)
			{
				keyValuePair.Value.ResolveProperties(this.Desktop.Provider);
			}
		}

		// Token: 0x06003894 RID: 14484 RVA: 0x00074838 File Offset: 0x00072A38
		public bool TryGetDocument(string path, out Document document)
		{
			return this._documentsLibrary.TryGetValue(path, out document);
		}

		// Token: 0x06003895 RID: 14485 RVA: 0x00074847 File Offset: 0x00072A47
		private void LoadClientTexts()
		{
			this._clientTexts = Language.LoadLanguage(this._language);
		}

		// Token: 0x06003896 RID: 14486 RVA: 0x0007485B File Offset: 0x00072A5B
		public void SetLanguageAndLoad(string language)
		{
			this._language = language;
			this.LoadClientTexts();
		}

		// Token: 0x06003897 RID: 14487 RVA: 0x0007486C File Offset: 0x00072A6C
		public string GetText(string key, Dictionary<string, string> parameters = null, bool returnFallback = true)
		{
			string text;
			bool flag = !this._clientTexts.TryGetValue(key, out text) && !this._serverTexts.TryGetValue(key, out text);
			string result;
			if (flag)
			{
				result = (returnFallback ? key : null);
			}
			else
			{
				bool flag2 = parameters != null;
				if (flag2)
				{
					foreach (KeyValuePair<string, string> keyValuePair in parameters)
					{
						text = text.Replace("{" + keyValuePair.Key + "}", keyValuePair.Value);
					}
				}
				result = text;
			}
			return result;
		}

		// Token: 0x06003898 RID: 14488 RVA: 0x00074920 File Offset: 0x00072B20
		public string GetServerText(string key, Dictionary<string, string> parameters = null, bool returnFallback = true)
		{
			string text;
			bool flag = !this._serverTexts.TryGetValue(key, out text);
			string result;
			if (flag)
			{
				result = (returnFallback ? key : null);
			}
			else
			{
				bool flag2 = parameters != null;
				if (flag2)
				{
					foreach (KeyValuePair<string, string> keyValuePair in parameters)
					{
						text = text.Replace("{" + keyValuePair.Key + "}", keyValuePair.Value);
					}
				}
				result = text;
			}
			return result;
		}

		// Token: 0x06003899 RID: 14489 RVA: 0x000749C0 File Offset: 0x00072BC0
		public void SetServerMessages(Dictionary<string, string> dict)
		{
			this._serverTexts = dict;
		}

		// Token: 0x0600389A RID: 14490 RVA: 0x000749CC File Offset: 0x00072BCC
		public void AddServerMessages(Dictionary<string, string> dict)
		{
			foreach (KeyValuePair<string, string> keyValuePair in dict)
			{
				this._serverTexts[keyValuePair.Key] = keyValuePair.Value;
			}
		}

		// Token: 0x0600389B RID: 14491 RVA: 0x00074A30 File Offset: 0x00072C30
		public void RemoveServerMessages(ICollection<string> keys)
		{
			foreach (string key in keys)
			{
				this._serverTexts.Remove(key);
			}
		}

		// Token: 0x0600389C RID: 14492 RVA: 0x00074A84 File Offset: 0x00072C84
		public string FormatNumber(int value)
		{
			return value.ToString("N0");
		}

		// Token: 0x0600389D RID: 14493 RVA: 0x00074A92 File Offset: 0x00072C92
		public string FormatNumber(float value)
		{
			return value.ToString("N");
		}

		// Token: 0x0600389E RID: 14494 RVA: 0x00074AA0 File Offset: 0x00072CA0
		public string FormatNumber(double value)
		{
			return value.ToString("N");
		}

		// Token: 0x0600389F RID: 14495 RVA: 0x00074AB0 File Offset: 0x00072CB0
		public string FormatRelativeTime(DateTime dateTime)
		{
			TimeSpan t = DateTime.Now.Subtract(dateTime);
			bool flag = t <= TimeSpan.FromSeconds(1.0);
			string text;
			if (flag)
			{
				text = this.GetText("ui.general.relativeTime.now", null, true);
			}
			else
			{
				bool flag2 = t <= TimeSpan.FromSeconds(60.0);
				if (flag2)
				{
					text = this.GetText("ui.general.relativeTime.lessThanAMinute", null, true);
				}
				else
				{
					bool flag3 = t <= TimeSpan.FromMinutes(60.0);
					if (flag3)
					{
						text = this.GetText("ui.general.relativeTime.minute" + ((t.Minutes > 1) ? "s" : ""), new Dictionary<string, string>
						{
							{
								"minutes, number",
								t.Minutes.ToString()
							}
						}, true);
					}
					else
					{
						bool flag4 = t <= TimeSpan.FromHours(24.0);
						if (flag4)
						{
							text = this.GetText("ui.general.relativeTime.hour" + ((t.Hours > 1) ? "s" : ""), new Dictionary<string, string>
							{
								{
									"hours, number",
									t.Hours.ToString()
								}
							}, true);
						}
						else
						{
							bool flag5 = t <= TimeSpan.FromDays(30.0);
							if (flag5)
							{
								text = this.GetText("ui.general.relativeTime.day" + ((t.Days > 1) ? "s" : ""), new Dictionary<string, string>
								{
									{
										"days, number",
										t.Days.ToString()
									}
								}, true);
							}
							else
							{
								bool flag6 = t <= TimeSpan.FromDays(365.0);
								if (flag6)
								{
									text = this.GetText("ui.general.relativeTime.month" + ((t.Days > 30) ? "s" : ""), new Dictionary<string, string>
									{
										{
											"months, number",
											(t.Days / 30).ToString()
										}
									}, true);
								}
								else
								{
									text = this.GetText("ui.general.relativeTime.year" + ((t.Days > 365) ? "s" : ""), new Dictionary<string, string>
									{
										{
											"years, number",
											(t.Days / 365).ToString()
										}
									}, true);
								}
							}
						}
					}
				}
			}
			return text;
		}

		// Token: 0x060038A0 RID: 14496 RVA: 0x00074D20 File Offset: 0x00072F20
		public void PlaySound(SoundStyle sound)
		{
			bool flag = this.Engine.Audio == null;
			if (!flag)
			{
				uint eventId;
				bool flag2 = !this.Engine.Audio.ResourceManager.WwiseEventIds.TryGetValue(sound.SoundPath.Value, out eventId);
				if (flag2)
				{
					BaseInterface.Logger.Warn("Unknown UI sound: {0}", sound.SoundPath.Value);
				}
				else
				{
					this.Engine.Audio.PostEvent(eventId, AudioDevice.PlayerSoundObjectReference);
				}
			}
		}

		// Token: 0x17001005 RID: 4101
		// (get) Token: 0x060038A1 RID: 14497 RVA: 0x00074DA5 File Offset: 0x00072FA5
		// (set) Token: 0x060038A2 RID: 14498 RVA: 0x00074DAD File Offset: 0x00072FAD
		public TextureArea WhitePixel { get; private set; }

		// Token: 0x17001006 RID: 4102
		// (get) Token: 0x060038A3 RID: 14499 RVA: 0x00074DB6 File Offset: 0x00072FB6
		// (set) Token: 0x060038A4 RID: 14500 RVA: 0x00074DBE File Offset: 0x00072FBE
		public TextureArea MissingTexture { get; private set; }

		// Token: 0x17001007 RID: 4103
		// (get) Token: 0x060038A5 RID: 14501 RVA: 0x00074DC7 File Offset: 0x00072FC7
		public Point TextureAtlasSize
		{
			get
			{
				return new Point(this._atlas.Width, this._atlas.Height);
			}
		}

		// Token: 0x060038A6 RID: 14502 RVA: 0x00074DE4 File Offset: 0x00072FE4
		protected virtual void LoadTextures(bool use2x)
		{
			Texture atlas = this._atlas;
			if (atlas != null)
			{
				atlas.Dispose();
			}
			this._atlasTextureAreas.Clear();
			List<string> list = new List<string>();
			foreach (string text in Directory.EnumerateFiles(this._resourcesPath, "*.png", 1))
			{
				string item = text.Substring(this._resourcesPath.Length + 1);
				bool flag = text.EndsWith("@2x.png");
				bool flag2 = !use2x && flag;
				if (flag2)
				{
					string text2 = text.Replace("@2x.png", ".png");
					bool flag3 = File.Exists(text2);
					if (flag3)
					{
						continue;
					}
				}
				else
				{
					bool flag4 = use2x && !flag;
					if (flag4)
					{
						string text3 = text.Replace(".png", "@2x.png");
						bool flag5 = File.Exists(text3);
						if (flag5)
						{
							continue;
						}
					}
				}
				list.Add(item);
			}
			int num = use2x ? 8192 : 4096;
			this._atlas = new Texture(Texture.TextureTypes.Texture2D);
			this._atlas.CreateTexture2D(num, num, null, 0, GL.LINEAR_MIPMAP_LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
			List<Image> list2 = new List<Image>();
			list2.Add(BaseInterface.MakeWhitePixelImage("Special:WhitePixel"));
			list2.Add(BaseInterface.MakeMissingImage("Special:Missing"));
			foreach (string text4 in list)
			{
				list2.Add(new Image(text4, File.ReadAllBytes(Path.Combine(this._resourcesPath, text4))));
			}
			list2.Sort((Image a, Image b) => b.Height.CompareTo(a.Height));
			Dictionary<Image, Point> dictionary;
			byte[] atlasPixels = Image.Pack(num, list2, out dictionary, true, default(CancellationToken));
			this._atlas.UpdateTexture2DMipMaps(Texture.BuildMipmapPixels(atlasPixels, num, this._atlas.MipmapLevelCount));
			foreach (KeyValuePair<Image, Point> keyValuePair in dictionary)
			{
				Image key = keyValuePair.Key;
				Point value = keyValuePair.Value;
				string text5 = key.Name.Replace("\\", "/");
				int num2 = text5.EndsWith("@2x.png") ? 2 : 1;
				bool flag6 = num2 == 2;
				if (flag6)
				{
					text5 = text5.Replace("@2x.png", ".png");
				}
				this._atlasTextureAreas.Add(text5, new TextureArea(this._atlas, value.X, value.Y, key.Width, key.Height, num2));
			}
			this.WhitePixel = this.MakeTextureArea("Special:WhitePixel");
			this.MissingTexture = this.MakeTextureArea("Special:Missing");
		}

		// Token: 0x060038A7 RID: 14503 RVA: 0x00075124 File Offset: 0x00073324
		private void DisposeTextures()
		{
			Texture atlas = this._atlas;
			if (atlas != null)
			{
				atlas.Dispose();
			}
			this._atlasTextureAreas.Clear();
		}

		// Token: 0x060038A8 RID: 14504 RVA: 0x00075148 File Offset: 0x00073348
		public TextureArea MakeTextureArea(string path)
		{
			TextureArea textureArea;
			bool flag = this._atlasTextureAreas.TryGetValue(path, out textureArea);
			TextureArea result;
			if (flag)
			{
				result = textureArea.Clone();
			}
			else
			{
				result = this.MissingTexture.Clone();
			}
			return result;
		}

		// Token: 0x060038A9 RID: 14505 RVA: 0x00075180 File Offset: 0x00073380
		public static Image MakeWhitePixelImage(string name)
		{
			return new Image(name, 1, 1, new byte[]
			{
				byte.MaxValue,
				byte.MaxValue,
				byte.MaxValue,
				byte.MaxValue
			});
		}

		// Token: 0x060038AA RID: 14506 RVA: 0x0007519C File Offset: 0x0007339C
		public static Image MakeMissingImage(string name)
		{
			byte[] array = new byte[4096];
			for (int i = 0; i < 32; i++)
			{
				for (int j = 0; j < 32; j++)
				{
					array[(i * 32 + j) * 4] = byte.MaxValue;
					bool flag = Math.Abs(j - i) > 2 && Math.Abs(31 - j - i) > 2;
					if (flag)
					{
						array[(i * 32 + j) * 4 + 1] = byte.MaxValue;
						array[(i * 32 + j) * 4 + 2] = byte.MaxValue;
					}
					array[(i * 32 + j) * 4 + 3] = byte.MaxValue;
				}
			}
			return new Image(name, 32, 32, array);
		}

		// Token: 0x060038AC RID: 14508 RVA: 0x00075263 File Offset: 0x00073463
		[CompilerGenerated]
		private void <LoadAndBuild>g__DisplayError|30_0(string message, TextParserSpan span)
		{
			this._markupErrorLayer.Setup(message, span);
			this._markupErrorDesktop.SetLayer(0, this._markupErrorLayer);
			this.HasMarkupError = true;
		}

		// Token: 0x04001877 RID: 6263
		public static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04001878 RID: 6264
		public readonly Engine Engine;

		// Token: 0x04001879 RID: 6265
		public readonly CoUIManager CoUiManager;

		// Token: 0x0400187A RID: 6266
		public readonly Desktop Desktop;

		// Token: 0x0400187B RID: 6267
		private readonly FontManager _fonts;

		// Token: 0x0400187C RID: 6268
		private readonly string _resourcesPath;

		// Token: 0x0400187D RID: 6269
		private readonly bool _isDevModeEnabled;

		// Token: 0x0400187E RID: 6270
		public const int CustomUIErrorLayerKey = 1;

		// Token: 0x0400187F RID: 6271
		public const int PopupLayerKey = 2;

		// Token: 0x04001880 RID: 6272
		public const int OverlayLayerKey = 3;

		// Token: 0x04001881 RID: 6273
		public const int ModalLayerKey = 4;

		// Token: 0x04001882 RID: 6274
		public const int ConsoleLayerKey = 5;

		// Token: 0x04001883 RID: 6275
		public bool HasMarkupError;

		// Token: 0x04001884 RID: 6276
		public bool HasLoaded;

		// Token: 0x04001886 RID: 6278
		private Action _onFadeComplete;

		// Token: 0x04001887 RID: 6279
		private float _fadeTimer;

		// Token: 0x04001888 RID: 6280
		private float _fadeDuration;

		// Token: 0x04001889 RID: 6281
		private float _flashTimer = -1f;

		// Token: 0x0400188A RID: 6282
		private const float FlashDuration = 0.35f;

		// Token: 0x0400188B RID: 6283
		private readonly Desktop _markupErrorDesktop;

		// Token: 0x0400188C RID: 6284
		private readonly MarkupErrorOverlay _markupErrorLayer;

		// Token: 0x0400188D RID: 6285
		private float _watchDelayTimer;

		// Token: 0x0400188E RID: 6286
		private const float WatchDelayDuration = 0.25f;

		// Token: 0x0400188F RID: 6287
		private readonly Dictionary<string, Document> _documentsLibrary = new Dictionary<string, Document>();

		// Token: 0x04001890 RID: 6288
		private string _language;

		// Token: 0x04001891 RID: 6289
		private Dictionary<string, string> _clientTexts = new Dictionary<string, string>();

		// Token: 0x04001892 RID: 6290
		private Dictionary<string, string> _serverTexts = new Dictionary<string, string>();

		// Token: 0x04001893 RID: 6291
		public const string NameWhitePixel = "Special:WhitePixel";

		// Token: 0x04001894 RID: 6292
		public const string NameMissing = "Special:Missing";

		// Token: 0x04001897 RID: 6295
		private Texture _atlas;

		// Token: 0x04001898 RID: 6296
		private Dictionary<string, TextureArea> _atlasTextureAreas = new Dictionary<string, TextureArea>();

		// Token: 0x02000CD0 RID: 3280
		public enum InterfaceFadeState
		{
			// Token: 0x04003FFA RID: 16378
			FadedIn,
			// Token: 0x04003FFB RID: 16379
			FadingIn,
			// Token: 0x04003FFC RID: 16380
			FadingOut,
			// Token: 0x04003FFD RID: 16381
			FadedOut
		}
	}
}
