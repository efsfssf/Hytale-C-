using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using HytaleClient.Application.Auth;
using HytaleClient.AssetEditor.Backends;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Interface;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Data.Characters;
using HytaleClient.Data.UserSettings;
using HytaleClient.Graphics;
using HytaleClient.Interface.CoherentUI;
using HytaleClient.Interface.CoherentUI.Internals;
using HytaleClient.Math;
using HytaleClient.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using SDL2;

namespace HytaleClient.AssetEditor
{
	// Token: 0x02000B89 RID: 2953
	internal class AssetEditorApp : Disposable
	{
		// Token: 0x17001390 RID: 5008
		// (get) Token: 0x06005AE4 RID: 23268 RVA: 0x001C5C62 File Offset: 0x001C3E62
		// (set) Token: 0x06005AE5 RID: 23269 RVA: 0x001C5C6A File Offset: 0x001C3E6A
		public AssetEditorApp.AppStage Stage { get; private set; }

		// Token: 0x06005AE6 RID: 23270 RVA: 0x001C5C74 File Offset: 0x001C3E74
		public AssetEditorApp()
		{
			this.LoadSettings();
			Window.WindowState initialState = this.Settings.Fullscreen ? Window.WindowState.Fullscreen : (this.Settings.Maximized ? Window.WindowState.Maximized : Window.WindowState.Normal);
			string text = Path.Combine(Paths.EditorData, (BuildInfo.Platform == Platform.MacOS) ? "Icon-256.png" : "Icon-64.png");
			Window.WindowSettings windowSettings = new Window.WindowSettings
			{
				Title = "Hytale Asset Editor",
				Icon = new Image(File.ReadAllBytes(text)),
				InitialSize = new Point(1280, 720),
				MinimumSize = new Point(50, 50),
				Borderless = (this.Settings.UseBorderlessForFullscreen && this.Settings.Fullscreen),
				InitialState = initialState,
				Resizable = true
			};
			this.Engine = new Engine(windowSettings, true);
			this.Engine.Profiling.Initialize(1, 200);
			this.Engine.Graphics.SetVSyncEnabled(true);
			this.Engine.Window.Show();
			this.AuthManager = new AuthManager();
			this.CoUIManager = new CoUIManager(this.Engine, new CoUIFileHandler());
			this.Fonts = new FontManager();
			this.CharacterPartStore = new CharacterPartStore(this.Engine.Graphics.GL);
			this.Interface = new AssetEditorInterface(this, OptionsHelper.IsUiDevEnabled);
			this.Startup = new AssetEditorAppStartup(this);
			this.MainMenu = new AssetEditorAppMainMenu(this);
			this.Editor = new AssetEditorAppEditor(this);
			this._ipcClient = IpcClient.CreateReadWriteClient(new Action<string, JObject>(this.OnReceiveIpcMessage));
		}

		// Token: 0x06005AE7 RID: 23271 RVA: 0x001C5E54 File Offset: 0x001C4054
		protected override void DoDispose()
		{
			Debug.Assert(this.Stage == AssetEditorApp.AppStage.Exited);
			this.SaveSettingsBlocking();
			this._ipcClient.Dispose();
			this.CharacterPartStore.Dispose();
			this.Interface.Dispose();
			this.Fonts.Dispose();
			this.CoUIManager.Dispose();
			this.Engine.Dispose();
		}

		// Token: 0x06005AE8 RID: 23272 RVA: 0x001C5EC0 File Offset: 0x001C40C0
		internal void SetStage(AssetEditorApp.AppStage newStage)
		{
			Debug.Assert(newStage != this.Stage);
			Debug.Assert(ThreadHelper.IsMainThread());
			AssetEditorApp.Logger.Info<AssetEditorApp.AppStage, AssetEditorApp.AppStage>("Changing from Stage {from} to {to}", this.Stage, newStage);
			switch (this.Stage)
			{
			case AssetEditorApp.AppStage.Startup:
				this.Startup.CleanUp();
				break;
			case AssetEditorApp.AppStage.MainMenu:
				this.MainMenu.CleanUp();
				break;
			case AssetEditorApp.AppStage.Editor:
				this.Editor.CleanUp();
				break;
			}
			this.Stage = newStage;
			bool flag = newStage != AssetEditorApp.AppStage.Exited;
			if (flag)
			{
				GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
				bool flag2 = this.Stage == AssetEditorApp.AppStage.MainMenu && this._withheldOpenEditorIpcMessage != null;
				if (flag2)
				{
					this.HandleWithheldOpenEditorIpcMessage(true);
				}
				this.Engine.Graphics.GPUProgramStore.ResetProgramUniforms();
				this.ResetElapsedTime();
				this.Interface.OnAppStageChanged();
			}
			else
			{
				SDL.SDL_HideWindow(this.Engine.Window.Handle);
			}
		}

		// Token: 0x06005AE9 RID: 23273 RVA: 0x001C5FD3 File Offset: 0x001C41D3
		public void Exit()
		{
			this.SetStage(AssetEditorApp.AppStage.Exited);
		}

		// Token: 0x06005AEA RID: 23274 RVA: 0x001C5FE0 File Offset: 0x001C41E0
		private void OnReceiveIpcMessage(string messageType, JObject data)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			bool flag = messageType != "OpenEditor";
			if (!flag)
			{
				this.Engine.RunOnMainThread(this, delegate
				{
					this.HandleOpenEditorIpcMessage(data, true);
				}, false, false);
			}
		}

		// Token: 0x06005AEB RID: 23275 RVA: 0x001C603C File Offset: 0x001C423C
		private void HandleOpenEditorIpcMessage(JObject data, bool raiseWindow)
		{
			bool flag = this.Stage == AssetEditorApp.AppStage.Exited;
			if (!flag)
			{
				bool flag2 = this.Stage == AssetEditorApp.AppStage.Startup;
				if (flag2)
				{
					this._withheldOpenEditorIpcMessage = data;
				}
				else
				{
					bool flag3 = false;
					JToken jtoken;
					bool flag4 = data.TryGetValue("Cosmetics", ref jtoken);
					if (flag4)
					{
						flag3 = (bool)jtoken;
					}
					AssetIdReference none = AssetIdReference.None;
					string text = null;
					JToken jtoken2;
					bool flag5 = data.TryGetValue("AssetPath", ref jtoken2);
					if (flag5)
					{
						text = (string)jtoken2;
					}
					else
					{
						JToken jtoken3;
						bool flag6 = data.TryGetValue("AssetId", ref jtoken3);
						if (flag6)
						{
							none = new AssetIdReference((string)data["AssetType"], (string)jtoken3);
						}
					}
					bool flag7 = flag3;
					if (flag7)
					{
						bool flag8 = this.Stage == AssetEditorApp.AppStage.MainMenu;
						if (flag8)
						{
							bool isConnectingToServer = this.MainMenu.IsConnectingToServer;
							if (isConnectingToServer)
							{
								this.MainMenu.CancelConnection();
							}
							this.Editor.OpenCosmeticsEditor();
							bool flag9 = text != null;
							if (flag9)
							{
								this.Editor.OpenAsset(text);
							}
							else
							{
								bool flag10 = none.Id != null;
								if (flag10)
								{
									this.Editor.OpenAsset(none);
								}
							}
						}
						else
						{
							bool flag11 = this.Stage == AssetEditorApp.AppStage.Editor;
							if (flag11)
							{
								bool flag12 = this.Editor.Backend is LocalAssetEditorBackend;
								if (flag12)
								{
									bool flag13 = text != null;
									if (flag13)
									{
										this.Editor.OpenAsset(text);
									}
									else
									{
										bool flag14 = none.Id != null;
										if (flag14)
										{
											this.Editor.OpenAsset(none);
										}
									}
								}
								else
								{
									this._withheldOpenEditorIpcMessage = data;
									this.Editor.ShowIpcServerConnectionPrompt(this.Interface.GetText("ui.assetEditor.cosmeticEditor", null, true));
								}
							}
						}
					}
					else
					{
						string text2 = (string)data["Hostname"];
						int num = (int)data["Port"];
						bool flag15 = this.Stage == AssetEditorApp.AppStage.MainMenu;
						if (flag15)
						{
							bool isConnectingToServer2 = this.MainMenu.IsConnectingToServer;
							if (isConnectingToServer2)
							{
								bool flag16 = this.MainMenu.Connection.Hostname != text2 || this.MainMenu.Connection.Port != num;
								if (flag16)
								{
									this.MainMenu.CancelConnection();
									this.MainMenu.ConnectToServer(text2, num);
								}
							}
							else
							{
								this.MainMenu.ConnectToServer(text2, num);
							}
							bool flag17 = text != null;
							if (flag17)
							{
								this.MainMenu.AssetPathToOpen = text;
								this.MainMenu.AssetIdToOpen = AssetIdReference.None;
							}
							else
							{
								bool flag18 = none.Id != null;
								if (flag18)
								{
									this.MainMenu.AssetPathToOpen = null;
									this.MainMenu.AssetIdToOpen = none;
								}
							}
						}
						else
						{
							bool flag19 = this.Stage == AssetEditorApp.AppStage.Editor;
							if (flag19)
							{
								ServerAssetEditorBackend serverAssetEditorBackend = this.Editor.Backend as ServerAssetEditorBackend;
								bool flag20 = serverAssetEditorBackend != null && serverAssetEditorBackend.Hostname == text2 && serverAssetEditorBackend.Port == num;
								if (flag20)
								{
									bool flag21 = text != null;
									if (flag21)
									{
										this.Editor.OpenAsset(text);
									}
									else
									{
										bool flag22 = none.Id != null;
										if (flag22)
										{
											this.Editor.OpenAsset(none);
										}
									}
								}
								else
								{
									this._withheldOpenEditorIpcMessage = data;
									this.Editor.ShowIpcServerConnectionPrompt((string)data["Name"]);
								}
							}
						}
					}
					if (raiseWindow)
					{
						this.Engine.Window.Raise();
					}
				}
			}
		}

		// Token: 0x06005AEC RID: 23276 RVA: 0x001C63D8 File Offset: 0x001C45D8
		public void HandleWithheldOpenEditorIpcMessage(bool raiseWindow)
		{
			JObject withheldOpenEditorIpcMessage = this._withheldOpenEditorIpcMessage;
			this._withheldOpenEditorIpcMessage = null;
			this.HandleOpenEditorIpcMessage(withheldOpenEditorIpcMessage, raiseWindow);
		}

		// Token: 0x17001391 RID: 5009
		// (get) Token: 0x06005AED RID: 23277 RVA: 0x001C63FD File Offset: 0x001C45FD
		// (set) Token: 0x06005AEE RID: 23278 RVA: 0x001C6405 File Offset: 0x001C4605
		public float CpuTime { get; private set; }

		// Token: 0x06005AEF RID: 23279 RVA: 0x001C640E File Offset: 0x001C460E
		public void ResetElapsedTime()
		{
			this._resetElapsedTime = true;
		}

		// Token: 0x06005AF0 RID: 23280 RVA: 0x001C6418 File Offset: 0x001C4618
		public void RunLoop()
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			while (this.Stage != AssetEditorApp.AppStage.Exited)
			{
				bool flag = false;
				for (;;)
				{
					SDL.SDL_Event sdl_Event;
					bool flag2 = this.Stage != AssetEditorApp.AppStage.Exited && SDL.SDL_PollEvent(out sdl_Event) == 1;
					if (!flag2)
					{
						break;
					}
					SDL.SDL_EventType type = sdl_Event.type;
					SDL.SDL_EventType sdl_EventType = type;
					if (sdl_EventType <= SDL.SDL_EventType.SDL_WINDOWEVENT)
					{
						if (sdl_EventType != SDL.SDL_EventType.SDL_QUIT)
						{
							if (sdl_EventType == SDL.SDL_EventType.SDL_WINDOWEVENT)
							{
								switch (sdl_Event.window.windowEvent)
								{
								case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_MOVED:
								case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_SIZE_CHANGED:
									flag = true;
									break;
								case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_MINIMIZED:
								case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_MAXIMIZED:
								case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_RESTORED:
								{
									Window.WindowState state = this.Engine.Window.GetState();
									bool flag3 = state != Window.WindowState.Minimized;
									if (flag3)
									{
										bool flag4 = state == Window.WindowState.Maximized;
										bool flag5 = this.Settings.Maximized != flag4;
										if (flag5)
										{
											this.Settings.Maximized = flag4;
											this.SaveSettings();
										}
									}
									break;
								}
								case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_GAINED:
								case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST:
								{
									this.Engine.Window.OnFocusChanged(sdl_Event.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_GAINED);
									bool flag6 = sdl_Event.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST;
									if (flag6)
									{
										this.Interface.Desktop.ClearInput(false);
									}
									this.Interface.OnWindowFocusChanged();
									break;
								}
								}
							}
						}
						else
						{
							AssetEditorApp.Logger.Info("Received SDL_QUIT event");
							bool flag7 = this.Stage == AssetEditorApp.AppStage.Editor && this.Interface.AssetEditor.CheckHasUnexportedChanges(true, new Action(this.Exit));
							if (!flag7)
							{
								this.Exit();
							}
						}
					}
					else if (sdl_EventType - SDL.SDL_EventType.SDL_KEYDOWN <= 1U || sdl_EventType == SDL.SDL_EventType.SDL_TEXTINPUT || sdl_EventType - SDL.SDL_EventType.SDL_MOUSEMOTION <= 3U)
					{
						bool flag8 = !this.Engine.Window.IsFocused;
						if (!flag8)
						{
							this.OnUserInput(sdl_Event);
						}
					}
				}
				bool flag9 = this.Stage == AssetEditorApp.AppStage.Exited;
				if (flag9)
				{
					break;
				}
				bool flag10 = flag;
				if (flag10)
				{
					this.Engine.Window.SetupViewport(false);
					this.OnWindowSizeChanged();
				}
				this.CoUIManager.Update();
				this.Engine.Temp_ProcessQueuedActions();
				bool flag11 = this.Stage == AssetEditorApp.AppStage.Exited;
				if (flag11)
				{
					break;
				}
				float deltaTime = (float)stopwatch.Elapsed.TotalSeconds;
				stopwatch.Restart();
				bool resetElapsedTime = this._resetElapsedTime;
				if (resetElapsedTime)
				{
					deltaTime = 0f;
					this._resetElapsedTime = false;
				}
				this.Fonts.BuildMissingGlyphs();
				bool flag12 = this.Stage == AssetEditorApp.AppStage.Editor;
				if (flag12)
				{
					this.Editor.GameTime.OnNewFrame(deltaTime);
				}
				this.Interface.Update(deltaTime);
				this.Interface.PrepareForDraw();
				GLFunctions gl = this.Engine.Graphics.GL;
				gl.Viewport(this.Engine.Window.Viewport);
				RenderTarget.BindHardwareFramebuffer();
				gl.ClearColor(0f, 0f, 0f, 1f);
				gl.Clear((GL)17664U);
				gl.Disable(GL.DEPTH_TEST);
				gl.Enable(GL.BLEND);
				gl.ActiveTexture(GL.TEXTURE0);
				gl.BlendFunc(GL.ONE, GL.ONE_MINUS_SRC_ALPHA);
				this.Interface.Draw();
				gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
				SDL.SDL_GL_SwapWindow(this.Engine.Window.Handle);
				int num = this.Engine.Window.IsFocused ? 60 : 30;
				float num2 = 1f / (float)num;
				this.CpuTime = (float)stopwatch.Elapsed.TotalSeconds;
				bool flag13 = this.CpuTime < num2;
				if (flag13)
				{
					Thread.Sleep((int)(MathHelper.Max(0f, num2 - this.CpuTime - 0.002f) * 1000f));
					for (float num3 = (float)stopwatch.Elapsed.TotalSeconds; num3 < num2; num3 = (float)stopwatch.Elapsed.TotalSeconds)
					{
						Thread.Sleep(0);
					}
				}
				bool flag14 = this.Engine.Window.GetState() == Window.WindowState.Minimized;
				if (flag14)
				{
					Thread.Sleep(10);
				}
			}
		}

		// Token: 0x06005AF1 RID: 23281 RVA: 0x001C68B0 File Offset: 0x001C4AB0
		private void OnUserInput(SDL.SDL_Event @event)
		{
			bool flag = @event.type == SDL.SDL_EventType.SDL_KEYDOWN && @event.key.keysym.sym == SDL.SDL_Keycode.SDLK_F9;
			if (flag)
			{
				bool forceReset = (@event.key.keysym.mod & SDL.SDL_Keymod.KMOD_SHIFT) > SDL.SDL_Keymod.KMOD_NONE;
				this.Engine.Graphics.GPUProgramStore.ResetPrograms(forceReset);
				AssetEditorApp.Logger.Info("Shaders have been reloaded.");
			}
			bool flag2 = this.CoUIManager.FocusedWebView == null;
			if (flag2)
			{
				this.Interface.OnUserInput(@event);
			}
			else
			{
				WebView webView = this.CoUIManager.FocusedWebView;
				SDL.SDL_Event evtCopy = @event;
				this.CoUIManager.RunInThread(delegate
				{
					CoUIViewInputForwarder.OnUserInput(webView, evtCopy, this.Engine.Window);
				});
			}
		}

		// Token: 0x06005AF2 RID: 23282 RVA: 0x001C698C File Offset: 0x001C4B8C
		private void OnWindowSizeChanged()
		{
			this.Interface.OnWindowSizeChanged();
			int width = this.Engine.Window.Viewport.Width;
			int height = this.Engine.Window.Viewport.Height;
			this.Engine.Graphics.RTStore.Resize(width, height, 1f);
		}

		// Token: 0x17001392 RID: 5010
		// (get) Token: 0x06005AF3 RID: 23283 RVA: 0x001C69EF File Offset: 0x001C4BEF
		// (set) Token: 0x06005AF4 RID: 23284 RVA: 0x001C69F7 File Offset: 0x001C4BF7
		public AssetEditorSettings Settings { get; private set; }

		// Token: 0x06005AF5 RID: 23285 RVA: 0x001C6A00 File Offset: 0x001C4C00
		public void ApplySettings(AssetEditorSettings newSettings)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			AssetEditorSettings settings = this.Settings;
			this.Settings = newSettings;
			this.SaveSettings();
			bool flag = settings.Language != newSettings.Language;
			if (flag)
			{
				this.Interface.SetLanguageAndLoad(newSettings.Language);
				this.Interface.LoadAndBuild();
				bool flag2 = this.Stage == AssetEditorApp.AppStage.Editor;
				if (flag2)
				{
					this.Editor.Backend.OnLanguageChanged();
				}
			}
			bool flag3 = settings.Fullscreen != newSettings.Fullscreen || settings.UseBorderlessForFullscreen != newSettings.UseBorderlessForFullscreen;
			if (flag3)
			{
				Window.WindowState windowState = this.Engine.Window.GetState();
				bool flag4 = windowState == Window.WindowState.Fullscreen;
				if (flag4)
				{
					windowState = Window.WindowState.Normal;
				}
				this.Engine.Window.SetState(newSettings.Fullscreen ? Window.WindowState.Fullscreen : windowState, newSettings.Fullscreen && newSettings.UseBorderlessForFullscreen, false);
			}
			bool flag5 = settings.AssetsPath != newSettings.AssetsPath;
			if (flag5)
			{
				bool flag6 = this.Stage == AssetEditorApp.AppStage.Editor;
				if (flag6)
				{
					this.Editor.OnAssetEditorPathChanged();
				}
			}
			bool flag7 = settings.DisplayDefaultAssetPathWarning != newSettings.DisplayDefaultAssetPathWarning;
			if (flag7)
			{
				bool flag8 = this.Stage == AssetEditorApp.AppStage.Editor;
				if (flag8)
				{
					this.Interface.AssetEditor.UpdateAssetPathWarning(newSettings.DisplayDefaultAssetPathWarning);
				}
			}
		}

		// Token: 0x06005AF6 RID: 23286 RVA: 0x001C6B6D File Offset: 0x001C4D6D
		private string GetSettingsJsonString()
		{
			return JsonConvert.SerializeObject(this.Settings, 1, this._settingsSerializerSettings);
		}

		// Token: 0x06005AF7 RID: 23287 RVA: 0x001C6B84 File Offset: 0x001C4D84
		public void SaveSettings()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this._settingsSaveCounter++;
			int version = this._settingsSaveCounter;
			string jsonString = this.GetSettingsJsonString();
			Task.Run(delegate()
			{
				this.SaveSettingsToFile(jsonString, version);
			}).ContinueWith(delegate(Task t)
			{
				bool flag = !t.IsFaulted;
				if (!flag)
				{
					AssetEditorApp.Logger.Error(t.Exception, "Failed to save asset editor settings");
				}
			});
		}

		// Token: 0x06005AF8 RID: 23288 RVA: 0x001C6C08 File Offset: 0x001C4E08
		private void SaveSettingsBlocking()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this._settingsSaveCounter++;
			int settingsSaveCounter = this._settingsSaveCounter;
			string settingsJsonString = this.GetSettingsJsonString();
			this.SaveSettingsToFile(settingsJsonString, settingsSaveCounter);
		}

		// Token: 0x06005AF9 RID: 23289 RVA: 0x001C6C48 File Offset: 0x001C4E48
		private void SaveSettingsToFile(string data, int count)
		{
			object settingsSaveLock = this._settingsSaveLock;
			lock (settingsSaveLock)
			{
				bool flag2 = this._lastSavedSettingsCounter > count;
				if (!flag2)
				{
					File.WriteAllText(this._settingsPath + ".new", data);
					bool flag3 = File.Exists(this._settingsPath);
					if (flag3)
					{
						File.Replace(this._settingsPath + ".new", this._settingsPath, this._settingsPath + ".bak");
					}
					else
					{
						File.Move(this._settingsPath + ".new", this._settingsPath);
					}
					this._lastSavedSettingsCounter = count;
				}
			}
		}

		// Token: 0x06005AFA RID: 23290 RVA: 0x001C6D10 File Offset: 0x001C4F10
		private void LoadSettings()
		{
			AssetEditorApp.Logger.Info("Loading settings...");
			this.Settings = new AssetEditorSettings();
			bool flag = !File.Exists(this._settingsPath);
			if (flag)
			{
				AssetEditorApp.Logger.Info("Settings file does not exist. Initializing...");
				this.Settings.Initialize();
				this.SaveSettings();
			}
			else
			{
				JObject jobject;
				try
				{
					jobject = JObject.Parse(File.ReadAllText(this._settingsPath));
				}
				catch (Exception exception)
				{
					AssetEditorApp.Logger.Error(exception, "Failed to load asset editor settings json");
					this.Settings.Initialize();
					this.SaveSettings();
					return;
				}
				int num = 1;
				JToken jtoken;
				bool flag2 = jobject.TryGetValue("FormatVersion", ref jtoken);
				if (flag2)
				{
					num = (int)jtoken;
				}
				int num2 = 1;
				bool flag3 = num2 > num;
				bool flag4 = flag3;
				if (flag4)
				{
					AssetEditorSettings.Migrate(jobject, num);
					AssetEditorApp.Logger.Info<int, int>("Migrated settings from format version {0} to {1}", num, num2);
				}
				try
				{
					JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(this._settingsSerializerSettings);
					using (JTokenReader jtokenReader = new JTokenReader(jobject))
					{
						jsonSerializer.Populate(jtokenReader, this.Settings);
					}
				}
				catch (Exception exception2)
				{
					AssetEditorApp.Logger.Error(exception2, "Failed to convert JSON object to settings object");
					this.Settings.Initialize();
					this.SaveSettings();
					return;
				}
				this.Settings.FormatVersion = num2;
				this.Settings.Initialize();
				bool flag5 = flag3;
				if (flag5)
				{
					this.SaveSettings();
				}
			}
		}

		// Token: 0x040038F2 RID: 14578
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x040038F4 RID: 14580
		public readonly Engine Engine;

		// Token: 0x040038F5 RID: 14581
		public readonly AuthManager AuthManager;

		// Token: 0x040038F6 RID: 14582
		public readonly CoUIManager CoUIManager;

		// Token: 0x040038F7 RID: 14583
		public readonly AssetEditorInterface Interface;

		// Token: 0x040038F8 RID: 14584
		public readonly FontManager Fonts;

		// Token: 0x040038F9 RID: 14585
		public readonly CharacterPartStore CharacterPartStore;

		// Token: 0x040038FA RID: 14586
		public readonly AssetEditorAppStartup Startup;

		// Token: 0x040038FB RID: 14587
		public readonly AssetEditorAppEditor Editor;

		// Token: 0x040038FC RID: 14588
		public readonly AssetEditorAppMainMenu MainMenu;

		// Token: 0x040038FD RID: 14589
		private readonly IpcClient _ipcClient;

		// Token: 0x040038FE RID: 14590
		private JObject _withheldOpenEditorIpcMessage;

		// Token: 0x04003900 RID: 14592
		private bool _resetElapsedTime = false;

		// Token: 0x04003902 RID: 14594
		private readonly JsonSerializerSettings _settingsSerializerSettings = new JsonSerializerSettings();

		// Token: 0x04003903 RID: 14595
		private readonly string _settingsPath = Path.Combine(Paths.UserData, "Settings.json");

		// Token: 0x04003904 RID: 14596
		private readonly object _settingsSaveLock = new object();

		// Token: 0x04003905 RID: 14597
		private int _lastSavedSettingsCounter;

		// Token: 0x04003906 RID: 14598
		private int _settingsSaveCounter;

		// Token: 0x02000F75 RID: 3957
		public enum AppStage
		{
			// Token: 0x04004B17 RID: 19223
			Initial,
			// Token: 0x04004B18 RID: 19224
			Startup,
			// Token: 0x04004B19 RID: 19225
			MainMenu,
			// Token: 0x04004B1A RID: 19226
			Editor,
			// Token: 0x04004B1B RID: 19227
			Exited
		}
	}
}
