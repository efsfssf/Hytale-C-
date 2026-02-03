using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Epic.OnlineServices;
using HytaleClient.Application.Auth;
using HytaleClient.Application.Services;
using HytaleClient.Application.Services.Api;
using HytaleClient.Common.Memory;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Data.Characters;
using HytaleClient.Data.UserSettings;
using HytaleClient.Graphics;
using HytaleClient.InGame;
using HytaleClient.Interface;
using HytaleClient.Interface.CoherentUI;
using HytaleClient.Interface.CoherentUI.Internals;
using HytaleClient.Interface.InGame.Hud.Abilities;
using HytaleClient.Interface.UI;
using HytaleClient.Math;
using HytaleClient.Networking;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using NLog;
using SDL2;

namespace HytaleClient.Application
{
	// Token: 0x02000BE4 RID: 3044
	internal class App : Disposable
	{
		// Token: 0x170013E9 RID: 5097
		// (get) Token: 0x0600605D RID: 24669 RVA: 0x001F7CF1 File Offset: 0x001F5EF1
		// (set) Token: 0x0600605E RID: 24670 RVA: 0x001F7CF9 File Offset: 0x001F5EF9
		public CharacterPartStore CharacterPartStore { get; private set; }

		// Token: 0x170013EA RID: 5098
		// (get) Token: 0x0600605F RID: 24671 RVA: 0x001F7D02 File Offset: 0x001F5F02
		// (set) Token: 0x06006060 RID: 24672 RVA: 0x001F7D0A File Offset: 0x001F5F0A
		public Interface Interface { get; private set; }

		// Token: 0x170013EB RID: 5099
		// (get) Token: 0x06006061 RID: 24673 RVA: 0x001F7D13 File Offset: 0x001F5F13
		// (set) Token: 0x06006062 RID: 24674 RVA: 0x001F7D1B File Offset: 0x001F5F1B
		public App.AppStage Stage { get; private set; }

		// Token: 0x170013EC RID: 5100
		// (get) Token: 0x06006063 RID: 24675 RVA: 0x001F7D24 File Offset: 0x001F5F24
		public string Username
		{
			get
			{
				return this.AuthManager.Settings.Username;
			}
		}

		// Token: 0x170013ED RID: 5101
		// (get) Token: 0x06006064 RID: 24676 RVA: 0x001F7D36 File Offset: 0x001F5F36
		// (set) Token: 0x06006065 RID: 24677 RVA: 0x001F7D3E File Offset: 0x001F5F3E
		public ClientPlayerSkin PlayerSkin { get; private set; }

		// Token: 0x170013EE RID: 5102
		// (get) Token: 0x06006066 RID: 24678 RVA: 0x001F7D47 File Offset: 0x001F5F47
		// (set) Token: 0x06006067 RID: 24679 RVA: 0x001F7D4F File Offset: 0x001F5F4F
		public string SingleplayerWorldName { get; private set; }

		// Token: 0x170013EF RID: 5103
		// (get) Token: 0x06006068 RID: 24680 RVA: 0x001F7D58 File Offset: 0x001F5F58
		// (set) Token: 0x06006069 RID: 24681 RVA: 0x001F7D60 File Offset: 0x001F5F60
		public SingleplayerServer SingleplayerServer { get; private set; }

		// Token: 0x170013F0 RID: 5104
		// (get) Token: 0x0600606A RID: 24682 RVA: 0x001F7D69 File Offset: 0x001F5F69
		// (set) Token: 0x0600606B RID: 24683 RVA: 0x001F7D71 File Offset: 0x001F5F71
		public SingleplayerServer ShuttingDownSingleplayerServer { get; private set; }

		// Token: 0x0600606C RID: 24684 RVA: 0x001F7D7C File Offset: 0x001F5F7C
		public App()
		{
			bool generateUIDocs = OptionsHelper.GenerateUIDocs;
			if (generateUIDocs)
			{
				string fullPath = Path.GetFullPath("../../UIDocs.MediaWiki.txt");
				File.WriteAllText(fullPath, DocGen.GenerateMediaWikiPage());
				App.Logger.Info("Successfully generated UI documentation into " + fullPath);
				Environment.Exit(0);
			}
			this.Settings = Settings.Load();
			this.EOSPlatform = new EOSPlatformManager();
			Result result = this.EOSPlatform.Initialize();
			bool flag = result > Result.Success;
			if (flag)
			{
				App.Logger.Error(string.Format("Failed to initialize EOS Platform: {0}", result));
			}
			Window.WindowState initialState = this.Settings.Fullscreen ? Window.WindowState.Fullscreen : (this.Settings.Maximized ? Window.WindowState.Maximized : Window.WindowState.Normal);
			string text = Path.Combine(Paths.GameData, (BuildInfo.Platform == Platform.MacOS) ? "Icon-256.png" : "Icon-64.png");
			Window.WindowSettings windowSettings = new Window.WindowSettings
			{
				Title = "Hytale",
				Icon = new Image(File.ReadAllBytes(text)),
				InitialSize = new Point(this.Settings.ScreenResolution.Width, this.Settings.ScreenResolution.Height),
				MinimumSize = new Point(this.Settings.ScreenResolution.Width, this.Settings.ScreenResolution.Height),
				Borderless = (this.Settings.UseBorderlessForFullscreen && this.Settings.Fullscreen),
				InitialState = initialState,
				MinAspectRatio = 1f,
				Resizable = true
			};
			this.Engine = new Engine(windowSettings, false);
			this.Engine.InitializeAudio(this.Settings.AudioSettings.OutputDeviceId, "MASTERVOLUME", this.Settings.AudioSettings.MasterVolume, this.Settings.AudioSettings.GetCategoryRTPCsArray(), this.Settings.AudioSettings.GetCategoryVolumesArray());
			this.Engine.SetMouseRelativeModeRaw(this.Settings.MouseSettings.MouseRawInputMode);
			SDL.SDL_Rect sdl_Rect;
			SDL.SDL_GetDisplayBounds(SDL.SDL_GetWindowDisplayIndex(this.Engine.Window.Handle), out sdl_Rect);
			bool flag2 = !this.Settings.ScreenResolution.FitsIn(sdl_Rect.w, sdl_Rect.h);
			if (flag2)
			{
				this.Settings.ScreenResolution = ScreenResolutions.DefaultScreenResolution;
				this.Engine.Window.UpdateSize(this.Settings.ScreenResolution);
				this.Engine.Window.SetState(Window.WindowState.Normal, false, false);
			}
			this.Engine.Window.Show();
			this.Fonts = new FontManager();
			this.CharacterPartStore = new CharacterPartStore(this.Engine.Graphics.GL);
			this.AuthManager = new AuthManager();
			this.HytaleServices = new HytaleServices(this);
			this.HytaleServicesApi = new HytaleServicesApiClient();
			this.CoUIManager = new CoUIManager(this.Engine, new CoUIGameFileHandler(this));
			this.Startup = new AppStartup(this);
			this.MainMenu = new AppMainMenu(this);
			this.GameLoading = new AppGameLoading(this);
			this.InGame = new AppInGame(this);
			this.Disconnection = new AppDisconnection(this);
			this.DevTools = new DevTools(this);
			this.Ipc = IpcClient.CreateWriteOnlyClient();
		}

		// Token: 0x0600606D RID: 24685 RVA: 0x001F80F4 File Offset: 0x001F62F4
		protected override void DoDispose()
		{
			Debug.Assert(this.Stage == App.AppStage.Exited);
			this.Settings.Save();
			this.InGame.DisposeAndClearInstance();
			this.Interface.Dispose();
			this.Fonts.Dispose();
			this.CoUIManager.Dispose();
			this.CharacterPartStore.Dispose();
			this.HytaleServices.Dispose();
			EOSPlatformManager eosplatform = this.EOSPlatform;
			if (eosplatform != null)
			{
				eosplatform.Dispose();
			}
			this.Engine.Dispose();
			AssetManager.Shutdown();
			this.Ipc.Dispose();
		}

		// Token: 0x0600606E RID: 24686 RVA: 0x001F8198 File Offset: 0x001F6398
		internal void SetStage(App.AppStage newStage)
		{
			Debug.Assert(newStage != this.Stage);
			Debug.Assert(ThreadHelper.IsMainThread());
			App.Logger.Info<App.AppStage, App.AppStage>("Changing from Stage {from} to {to}", this.Stage, newStage);
			bool isOpen = this.DevTools.IsOpen;
			if (isOpen)
			{
				this.DevTools.Close();
			}
			switch (this.Stage)
			{
			case App.AppStage.Startup:
				this.Startup.CleanUp();
				break;
			case App.AppStage.MainMenu:
				this.MainMenu.CleanUp();
				break;
			case App.AppStage.GameLoading:
				this.GameLoading.CleanUp();
				break;
			case App.AppStage.InGame:
				this.InGame.CleanUp();
				break;
			case App.AppStage.Disconnection:
				this.Disconnection.CleanUp();
				break;
			}
			GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
			this.Stage = newStage;
			Debug.Assert(this.InGame.Instance == null || newStage == App.AppStage.InGame);
			bool flag = newStage != App.AppStage.Exited;
			if (flag)
			{
				this.Engine.Graphics.SetVSyncEnabled(newStage != App.AppStage.InGame || this.Settings.VSync);
				this.Engine.Graphics.GPUProgramStore.ResetProgramUniforms();
				this.ResetElapsedTime();
				this.Interface.OnAppStageChanged();
			}
			else
			{
				SDL.SDL_HideWindow(this.Engine.Window.Handle);
			}
		}

		// Token: 0x0600606F RID: 24687 RVA: 0x001F8304 File Offset: 0x001F6504
		internal void ReplaceCharacterPartStore(CharacterPartStore newPartStore)
		{
			this.CharacterPartStore.Dispose();
			this.CharacterPartStore = newPartStore;
		}

		// Token: 0x06006070 RID: 24688 RVA: 0x001F831B File Offset: 0x001F651B
		internal void SetupInterface()
		{
			Debug.Assert(this.Interface == null);
			this.Interface = new Interface(this, Path.Combine(Paths.GameData, "Interface"), OptionsHelper.IsUiDevEnabled);
		}

		// Token: 0x06006071 RID: 24689 RVA: 0x001F834E File Offset: 0x001F654E
		internal void SetPlayerSkin(ClientPlayerSkin playerSkin)
		{
			this.PlayerSkin = playerSkin;
		}

		// Token: 0x06006072 RID: 24690 RVA: 0x001F8359 File Offset: 0x001F6559
		internal void SetSingleplayerWorldName(string name)
		{
			this.SingleplayerWorldName = name;
		}

		// Token: 0x06006073 RID: 24691 RVA: 0x001F8364 File Offset: 0x001F6564
		internal void SetSingleplayerServer(SingleplayerServer server)
		{
			Debug.Assert(this.ShuttingDownSingleplayerServer == null);
			this.SingleplayerServer = server;
		}

		// Token: 0x06006074 RID: 24692 RVA: 0x001F837E File Offset: 0x001F657E
		public void OnSinglePlayerServerShuttingDown()
		{
			this.ShuttingDownSingleplayerServer = this.SingleplayerServer;
			this.SingleplayerServer = null;
		}

		// Token: 0x06006075 RID: 24693 RVA: 0x001F8398 File Offset: 0x001F6598
		public void OnSingleplayerServerShutdown(SingleplayerServer server)
		{
			bool flag = server == this.ShuttingDownSingleplayerServer;
			if (flag)
			{
				this.ShuttingDownSingleplayerServer = null;
			}
			else
			{
				bool flag2 = server == this.SingleplayerServer;
				if (flag2)
				{
					this.SingleplayerServer = null;
				}
			}
		}

		// Token: 0x06006076 RID: 24694 RVA: 0x001F83D4 File Offset: 0x001F65D4
		public void Exit()
		{
			bool flag = this.Stage == App.AppStage.GameLoading;
			if (flag)
			{
				this.GameLoading.Abort();
			}
			this.SetStage(App.AppStage.Exited);
		}

		// Token: 0x06006077 RID: 24695 RVA: 0x001F8403 File Offset: 0x001F6603
		public void Update()
		{
			EOSPlatformManager eosplatform = this.EOSPlatform;
			if (eosplatform != null)
			{
				eosplatform.Tick();
			}
		}

		// Token: 0x170013F1 RID: 5105
		// (get) Token: 0x06006078 RID: 24696 RVA: 0x001F8418 File Offset: 0x001F6618
		// (set) Token: 0x06006079 RID: 24697 RVA: 0x001F8420 File Offset: 0x001F6620
		public float CpuTime { get; private set; }

		// Token: 0x0600607A RID: 24698 RVA: 0x001F8429 File Offset: 0x001F6629
		public void ResetElapsedTime()
		{
			this._resetElapsedTime = true;
		}

		// Token: 0x0600607B RID: 24699 RVA: 0x001F8434 File Offset: 0x001F6634
		public void RunLoop()
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			while (this.Stage != App.AppStage.Exited)
			{
				bool flag = false;
				for (;;)
				{
					SDL.SDL_Event sdl_Event;
					bool flag2 = this.Stage != App.AppStage.Exited && SDL.SDL_PollEvent(out sdl_Event) == 1;
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
											this.Settings.Save();
										}
									}
									this.OnWindowSizeChanged();
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
									break;
								}
								}
							}
						}
						else
						{
							App.Logger.Info("Received SDL_QUIT event");
							bool flag7 = this.Stage == App.AppStage.InGame;
							if (!flag7)
							{
								goto IL_281;
							}
							bool flag8 = this.InGame.CurrentOverlay != AppInGame.InGameOverlay.ConfirmQuit;
							if (flag8)
							{
								bool isOpen = this.DevTools.IsOpen;
								if (isOpen)
								{
									this.DevTools.Close();
								}
								this.InGame.SetCurrentOverlay(AppInGame.InGameOverlay.ConfirmQuit);
							}
						}
					}
					else if (sdl_EventType - SDL.SDL_EventType.SDL_KEYDOWN <= 1U || sdl_EventType == SDL.SDL_EventType.SDL_TEXTINPUT || sdl_EventType - SDL.SDL_EventType.SDL_MOUSEMOTION <= 3U)
					{
						bool flag9 = !this.Engine.Window.IsFocused;
						if (!flag9)
						{
							this.OnUserInput(sdl_Event);
							WebView webView = this.CoUIManager.FocusedWebView;
							bool flag10 = webView != null;
							if (flag10)
							{
								SDL.SDL_Event evtCopy = sdl_Event;
								this.CoUIManager.RunInThread(delegate
								{
									CoUIViewInputForwarder.OnUserInput(webView, evtCopy, this.Engine.Window);
								});
							}
						}
					}
				}
				bool flag11 = this.Stage == App.AppStage.Exited;
				if (flag11)
				{
					break;
				}
				bool flag12 = flag;
				if (flag12)
				{
					this.Engine.Window.SetupViewport(false);
					this.OnWindowSizeChanged();
				}
				EOSPlatformManager eosplatform = this.EOSPlatform;
				if (eosplatform != null)
				{
					eosplatform.Tick();
				}
				this.CoUIManager.Update();
				this.Engine.Temp_ProcessQueuedActions();
				bool flag13 = this.Stage == App.AppStage.Exited;
				if (flag13)
				{
					break;
				}
				bool flag14 = this.DevTools.IsDiagnosticsModeEnabled && this.Interface.HasLoaded;
				if (flag14)
				{
					this.DevTools.HandleMessageQueue();
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
				this.Interface.Update(deltaTime);
				App.AppStage stage = this.Stage;
				App.AppStage appStage = stage;
				if (appStage != App.AppStage.MainMenu)
				{
					if (appStage != App.AppStage.InGame)
					{
						this.Interface.PrepareForDraw();
						GLFunctions gl = this.Engine.Graphics.GL;
						gl.Viewport(this.Engine.Window.Viewport);
						gl.ClearColor(0f, 0f, 0f, 1f);
						gl.Clear((GL)17664U);
						gl.Enable(GL.BLEND);
						gl.BlendFunc(GL.ONE, GL.ONE_MINUS_SRC_ALPHA);
						this.Interface.Draw();
						gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
					}
					else
					{
						this.InGame.OnNewFrame(deltaTime);
					}
				}
				else
				{
					this.MainMenu.OnNewFrame(deltaTime);
				}
				NativeMemory.EndBump();
				this.HandleScreenshotting();
				SDL.SDL_GL_SwapWindow(this.Engine.Window.Handle);
				int num = this.Engine.Window.IsFocused ? this.Settings.FpsLimit : 30;
				bool flag15 = this.Settings.UnlimitedFps && this.Engine.Window.IsFocused;
				float num2 = 1f / (float)num;
				this.CpuTime = (float)stopwatch.Elapsed.TotalSeconds;
				bool flag16 = !flag15 && this.CpuTime < num2;
				if (flag16)
				{
					Thread.Sleep((int)(MathHelper.Max(0f, num2 - this.CpuTime - 0.002f) * 1000f));
					for (float num3 = (float)stopwatch.Elapsed.TotalSeconds; num3 < num2; num3 = (float)stopwatch.Elapsed.TotalSeconds)
					{
						Thread.Sleep(0);
					}
				}
				bool flag17 = this.Engine.Window.GetState() == Window.WindowState.Minimized;
				if (flag17)
				{
					Thread.Sleep(10);
				}
				continue;
				IL_281:
				this.Exit();
				break;
			}
		}

		// Token: 0x0600607C RID: 24700 RVA: 0x001F89E4 File Offset: 0x001F6BE4
		private void OnUserInput(SDL.SDL_Event @event)
		{
			bool flag = @event.type == SDL.SDL_EventType.SDL_KEYDOWN && Input.EventMatchesBinding(@event, this.Settings.InputBindings.OpenDevTools) && this.Settings.DiagnosticMode;
			if (flag)
			{
				bool flag2 = !this.DevTools.IsOpen && this.Interface.Desktop.FocusedElement == null;
				if (flag2)
				{
					this.DevTools.Open();
				}
				else
				{
					bool isOpen = this.DevTools.IsOpen;
					if (isOpen)
					{
						this.DevTools.Close();
					}
				}
			}
			else
			{
				switch (this.Stage)
				{
				case App.AppStage.Startup:
					return;
				case App.AppStage.MainMenu:
					this.MainMenu.OnUserInput(@event);
					break;
				case App.AppStage.InGame:
				{
					bool flag3 = !this.InGame.Instance.Disposed;
					if (flag3)
					{
						this.InGame.Instance.OnUserInput(@event);
					}
					break;
				}
				}
				SDL.SDL_EventType type = @event.type;
				SDL.SDL_EventType sdl_EventType = type;
				if (sdl_EventType == SDL.SDL_EventType.SDL_KEYDOWN || sdl_EventType == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN)
				{
					bool flag4 = Input.EventMatchesBinding(@event, this.Settings.InputBindings.TakeScreenshot);
					if (flag4)
					{
						this.ScheduleScreenshot();
					}
					else
					{
						bool flag5 = Input.EventMatchesBinding(@event, this.Settings.InputBindings.ToggleFullscreen);
						if (flag5)
						{
							this.ToggleFullscreen();
						}
					}
				}
				bool flag6 = @event.type == SDL.SDL_EventType.SDL_KEYDOWN && @event.key.keysym.sym == SDL.SDL_Keycode.SDLK_F9;
				if (flag6)
				{
					bool forceReset = (@event.key.keysym.mod & SDL.SDL_Keymod.KMOD_SHIFT) > SDL.SDL_Keymod.KMOD_NONE;
					this.Engine.Graphics.GPUProgramStore.ResetPrograms(forceReset);
					GameInstance instance = this.InGame.Instance;
					if (instance != null)
					{
						instance.Chat.Log("Shaders have been reloaded.");
					}
				}
				bool flag7 = this.CoUIManager.FocusedWebView == null;
				if (flag7)
				{
					this.Interface.OnUserInput(@event);
				}
			}
		}

		// Token: 0x0600607D RID: 24701 RVA: 0x001F8BF4 File Offset: 0x001F6DF4
		private void OnWindowSizeChanged()
		{
			this.Interface.OnWindowSizeChanged();
			int width = this.Engine.Window.Viewport.Width;
			int height = this.Engine.Window.Viewport.Height;
			App.AppStage stage = this.Stage;
			App.AppStage appStage = stage;
			if (appStage != App.AppStage.MainMenu)
			{
				if (appStage - App.AppStage.GameLoading <= 1)
				{
					GameInstance instance = this.InGame.Instance;
					if (instance != null)
					{
						instance.Resize(width, height);
					}
				}
			}
			else
			{
				this.Engine.Graphics.RTStore.Resize(width, height, 1f);
				this.MainMenu.SceneRenderer.Resize(width, height);
				this.MainMenu.PostEffectRenderer.Resize(width, height, 1f);
			}
		}

		// Token: 0x0600607E RID: 24702 RVA: 0x001F8CB8 File Offset: 0x001F6EB8
		private void ScheduleScreenshot()
		{
			bool flag = this._screenshotFenceSync == IntPtr.Zero;
			if (flag)
			{
				this.Interface.ClearFlash();
				this._wasScreenshotRequested = true;
			}
		}

		// Token: 0x0600607F RID: 24703 RVA: 0x001F8CF0 File Offset: 0x001F6EF0
		private void HandleScreenshotting()
		{
			GLFunctions gl = this.Engine.Graphics.GL;
			bool wasScreenshotRequested = this._wasScreenshotRequested;
			if (wasScreenshotRequested)
			{
				Debug.Assert(this._screenshotFenceSync == IntPtr.Zero);
				this._wasScreenshotRequested = false;
				this._upcomingScreenshotSize = new Point(this.Engine.Window.Viewport.Width, this.Engine.Window.Viewport.Height);
				gl.BindVertexArray(GLVertexArray.None);
				this._screenshotPixelBuffer = gl.GenBuffer();
				gl.BindBuffer(GLVertexArray.None, GL.PIXEL_PACK_BUFFER, this._screenshotPixelBuffer);
				gl.BufferData(GL.PIXEL_PACK_BUFFER, (IntPtr)(this._upcomingScreenshotSize.X * this._upcomingScreenshotSize.Y * 4), IntPtr.Zero, GL.DYNAMIC_READ);
				gl.ReadPixels(0, 0, this._upcomingScreenshotSize.X, this._upcomingScreenshotSize.Y, GL.BGRA, GL.UNSIGNED_BYTE, IntPtr.Zero);
				gl.BindBuffer(GLVertexArray.None, GL.PIXEL_PACK_BUFFER, GLBuffer.None);
				this._screenshotFenceSync = gl.FenceSync(GL.SYNC_GPU_COMMANDS_COMPLETE, GL.NO_ERROR);
				bool flag = this._screenshotFenceSync == IntPtr.Zero;
				if (flag)
				{
					gl.DeleteBuffer(this._screenshotPixelBuffer);
					throw new Exception("Failed to get fence sync!");
				}
				this.Interface.Flash();
			}
			else
			{
				bool flag2 = this._screenshotFenceSync != IntPtr.Zero;
				if (flag2)
				{
					IntPtr value;
					gl.GetSynciv(this._screenshotFenceSync, GL.SYNC_STATUS, (IntPtr)8, IntPtr.Zero, out value);
					bool flag3 = (int)value != 37145;
					if (!flag3)
					{
						Point size = this._upcomingScreenshotSize;
						this._upcomingScreenshotSize = Point.Zero;
						gl.BindVertexArray(GLVertexArray.None);
						gl.BindBuffer(GLVertexArray.None, GL.PIXEL_PACK_BUFFER, this._screenshotPixelBuffer);
						IntPtr pointer = gl.MapBufferRange(GL.PIXEL_PACK_BUFFER, IntPtr.Zero, (IntPtr)(size.X * size.Y * 4), GL.ONE);
						byte[] rawPixels = new byte[size.X * size.Y * 4];
						for (int i = 0; i < size.Y; i++)
						{
							Marshal.Copy(pointer + i * size.X * 4, rawPixels, (size.Y - i - 1) * size.X * 4, size.X * 4);
						}
						gl.UnmapBuffer(GL.PIXEL_PACK_BUFFER);
						gl.BindBuffer(GLVertexArray.None, GL.PIXEL_PACK_BUFFER, GLBuffer.None);
						gl.DeleteBuffer(this._screenshotPixelBuffer);
						gl.DeleteSync(this._screenshotFenceSync);
						this._screenshotPixelBuffer = GLBuffer.None;
						this._screenshotFenceSync = IntPtr.Zero;
						ThreadPool.QueueUserWorkItem(delegate(object _)
						{
							Directory.CreateDirectory(this.ScreenshotsPath);
							try
							{
								string tempFileName = Path.GetTempFileName();
								new Image(size.X, size.Y, rawPixels).SavePNG(tempFileName, 16711680U, 65280U, 255U, 0U);
								int num = 0;
								int j = 0;
								while (j < 100)
								{
									string arg = (num > 0) ? string.Format("_{0}", num) : "";
									string text = Path.Combine(this.ScreenshotsPath, string.Format("Hytale{0:yyyy-MM-dd_HH-mm-ss}{1}.png", DateTime.Now, arg));
									num++;
									try
									{
										File.Move(tempFileName, text);
									}
									catch
									{
										goto IL_B5;
									}
									goto IL_B0;
									IL_B5:
									j++;
									continue;
									IL_B0:
									return;
								}
								throw new Exception("Could not move temp file to screenshots path");
							}
							catch (Exception ex)
							{
								Logger logger = App.Logger;
								string str = "Failed to save screenshot: ";
								Exception ex2 = ex;
								logger.Error(str + ((ex2 != null) ? ex2.ToString() : null));
								bool flag4 = this.InGame.Instance != null;
								if (flag4)
								{
									this.Engine.RunOnMainThread(this.InGame.Instance, delegate
									{
										this.InGame.Instance.Chat.Log(this.Interface.GetText("ui.general.failedToSaveScreenshot", null, true));
									}, false, false);
								}
							}
						});
					}
				}
			}
		}

		// Token: 0x170013F2 RID: 5106
		// (get) Token: 0x06006080 RID: 24704 RVA: 0x001F905D File Offset: 0x001F725D
		// (set) Token: 0x06006081 RID: 24705 RVA: 0x001F9065 File Offset: 0x001F7265
		public Settings Settings { get; private set; }

		// Token: 0x06006082 RID: 24706 RVA: 0x001F9070 File Offset: 0x001F7270
		public void ApplyNewSettings(Settings newSettings)
		{
			Settings settings = this.Settings;
			this.Settings = newSettings;
			bool flag = settings.Language != newSettings.Language;
			if (flag)
			{
				this.Interface.SetLanguageAndLoad(newSettings.Language);
				this.Interface.LoadAndBuild();
				bool flag2 = this.Stage == App.AppStage.InGame;
				if (flag2)
				{
					this.InGame.Instance.Connection.SendPacket(new UpdateLanguage(this.Settings.Language ?? Language.SystemLanguage));
				}
			}
			bool flag3 = settings.DynamicUIScaling != newSettings.DynamicUIScaling || settings.StaticUIScale != newSettings.StaticUIScale;
			if (flag3)
			{
				this.Interface.OnWindowSizeChanged();
			}
			bool flag4 = settings.Fullscreen != newSettings.Fullscreen || settings.UseBorderlessForFullscreen != newSettings.UseBorderlessForFullscreen || !settings.ScreenResolution.Equals(newSettings.ScreenResolution);
			if (flag4)
			{
				ScreenResolution screenResolution = newSettings.ScreenResolution;
				this.Engine.Window.UpdateSize(screenResolution);
				Window.WindowState windowState = this.Engine.Window.GetState();
				bool flag5 = windowState == Window.WindowState.Fullscreen;
				if (flag5)
				{
					windowState = Window.WindowState.Normal;
				}
				this.Engine.Window.SetState(newSettings.Fullscreen ? Window.WindowState.Fullscreen : windowState, newSettings.Fullscreen && newSettings.UseBorderlessForFullscreen, !settings.ScreenResolution.Equals(newSettings.ScreenResolution));
			}
			bool flag6 = settings.MouseSettings.MouseRawInputMode != newSettings.MouseSettings.MouseRawInputMode;
			if (flag6)
			{
				this.Engine.SetMouseRelativeModeRaw(this.Settings.MouseSettings.MouseRawInputMode);
			}
			bool flag7 = this.Stage == App.AppStage.InGame;
			if (flag7)
			{
				bool flag8 = settings.VSync != newSettings.VSync;
				if (flag8)
				{
					this.Engine.Graphics.SetVSyncEnabled(newSettings.VSync);
				}
				bool flag9 = settings.ViewDistance != newSettings.ViewDistance;
				if (flag9)
				{
					this.InGame.Instance.Connection.SendPacket(new ViewRadius(this.Settings.ViewDistance));
				}
				bool flag10 = settings.RenderScale != newSettings.RenderScale;
				if (flag10)
				{
					this.InGame.Instance.SetResolutionScale((float)newSettings.RenderScale * 0.01f);
				}
				bool flag11 = settings.ViewBobbingEffect != newSettings.ViewBobbingEffect;
				if (flag11)
				{
					this.InGame.Instance.CameraModule.CameraShakeController.Reset();
				}
				bool flag12 = settings.ViewBobbingIntensity != newSettings.ViewBobbingIntensity;
				if (flag12)
				{
					this.InGame.Instance.CameraModule.CameraShakeController.Reset();
				}
				bool flag13 = settings.CameraShakeEffect != newSettings.CameraShakeEffect;
				if (flag13)
				{
					this.InGame.Instance.CameraModule.CameraShakeController.Reset();
				}
				bool flag14 = settings.FirstPersonCameraShakeIntensity != newSettings.FirstPersonCameraShakeIntensity;
				if (flag14)
				{
					this.InGame.Instance.CameraModule.CameraShakeController.Reset();
				}
				bool flag15 = settings.ThirdPersonCameraShakeIntensity != newSettings.ThirdPersonCameraShakeIntensity;
				if (flag15)
				{
					this.InGame.Instance.CameraModule.CameraShakeController.Reset();
				}
				bool flag16 = settings.InputBindings != newSettings.InputBindings;
				if (flag16)
				{
					this.InGame.Instance.Input.SetInputBindings(newSettings.InputBindings);
					AbilitiesHudComponent abilitiesHudComponent = this.InGame.Instance.App.Interface.InGameView.AbilitiesHudComponent;
					if (abilitiesHudComponent != null)
					{
						abilitiesHudComponent.OnUpdateInputBindings();
					}
				}
				bool flag17 = settings.BuilderToolsSettings.DisplayLegend != newSettings.BuilderToolsSettings.DisplayLegend;
				if (flag17)
				{
					this.InGame.Instance.App.Interface.InGameView.UpdateBuilderToolsLegendVisibility(false);
				}
			}
			bool flag18 = settings.AudioSettings.OutputDeviceId != newSettings.AudioSettings.OutputDeviceId;
			if (flag18)
			{
				this.Engine.Audio.ReplaceOutputDevice(newSettings.AudioSettings.OutputDeviceId);
			}
			bool flag19 = settings.AudioSettings.MasterVolume != newSettings.AudioSettings.MasterVolume;
			if (flag19)
			{
				this.Engine.Audio.MasterVolume = newSettings.AudioSettings.MasterVolume;
			}
			foreach (KeyValuePair<string, float> keyValuePair in newSettings.AudioSettings.CategoryVolumes)
			{
				bool flag20 = keyValuePair.Value != settings.AudioSettings.CategoryVolumes[keyValuePair.Key];
				if (flag20)
				{
					this.Engine.Audio.SetCategoryVolume((int)Enum.Parse(typeof(AudioSettings.SoundCategory), keyValuePair.Key), keyValuePair.Value);
				}
			}
			bool flag21 = settings.DiagnosticMode != newSettings.DiagnosticMode;
			if (flag21)
			{
				bool flag22 = !newSettings.DiagnosticMode;
				if (flag22)
				{
					this.DevTools.ClearNotifications();
				}
				this.DevTools.IsDiagnosticsModeEnabled = newSettings.DiagnosticMode;
			}
		}

		// Token: 0x06006083 RID: 24707 RVA: 0x001F95E4 File Offset: 0x001F77E4
		public void ToggleFullscreen()
		{
			Settings settings = this.Settings.Clone();
			settings.Fullscreen = !settings.Fullscreen;
			this.ApplyNewSettings(settings);
			this.Settings.Save();
		}

		// Token: 0x04003BF9 RID: 15353
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003BFA RID: 15354
		public readonly Engine Engine;

		// Token: 0x04003BFB RID: 15355
		public readonly AuthManager AuthManager;

		// Token: 0x04003BFC RID: 15356
		public readonly HytaleServices HytaleServices;

		// Token: 0x04003BFD RID: 15357
		public readonly HytaleServicesApiClient HytaleServicesApi;

		// Token: 0x04003BFE RID: 15358
		public readonly FontManager Fonts;

		// Token: 0x04003C00 RID: 15360
		public readonly CoUIManager CoUIManager;

		// Token: 0x04003C02 RID: 15362
		public readonly DevTools DevTools;

		// Token: 0x04003C03 RID: 15363
		public readonly EOSPlatformManager EOSPlatform;

		// Token: 0x04003C05 RID: 15365
		public readonly AppStartup Startup;

		// Token: 0x04003C06 RID: 15366
		public readonly AppMainMenu MainMenu;

		// Token: 0x04003C07 RID: 15367
		public readonly AppGameLoading GameLoading;

		// Token: 0x04003C08 RID: 15368
		public readonly AppInGame InGame;

		// Token: 0x04003C09 RID: 15369
		public readonly AppDisconnection Disconnection;

		// Token: 0x04003C0E RID: 15374
		public readonly IpcClient Ipc;

		// Token: 0x04003C10 RID: 15376
		private bool _resetElapsedTime = false;

		// Token: 0x04003C11 RID: 15377
		private readonly string ScreenshotsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Hytale Screenshots");

		// Token: 0x04003C12 RID: 15378
		private bool _wasScreenshotRequested;

		// Token: 0x04003C13 RID: 15379
		private Point _upcomingScreenshotSize;

		// Token: 0x04003C14 RID: 15380
		private GLBuffer _screenshotPixelBuffer;

		// Token: 0x04003C15 RID: 15381
		private IntPtr _screenshotFenceSync;

		// Token: 0x02001021 RID: 4129
		public enum AppStage
		{
			// Token: 0x04004D25 RID: 19749
			Initial,
			// Token: 0x04004D26 RID: 19750
			Startup,
			// Token: 0x04004D27 RID: 19751
			MainMenu,
			// Token: 0x04004D28 RID: 19752
			GameLoading,
			// Token: 0x04004D29 RID: 19753
			InGame,
			// Token: 0x04004D2A RID: 19754
			Disconnection,
			// Token: 0x04004D2B RID: 19755
			Exited
		}

		// Token: 0x02001022 RID: 4130
		public enum SoundGroupType : byte
		{
			// Token: 0x04004D2D RID: 19757
			UI,
			// Token: 0x04004D2E RID: 19758
			MainMenu,
			// Token: 0x04004D2F RID: 19759
			InGameAssets,
			// Token: 0x04004D30 RID: 19760
			InGameCustomUI
		}
	}
}
