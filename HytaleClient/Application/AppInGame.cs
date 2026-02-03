using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using HytaleClient.Core;
using HytaleClient.Data.Items;
using HytaleClient.Graphics;
using HytaleClient.InGame;
using HytaleClient.Interface.InGame.Pages.InventoryPanels;
using HytaleClient.Math;
using HytaleClient.Networking;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using Newtonsoft.Json.Linq;
using NLog;

namespace HytaleClient.Application
{
	// Token: 0x02000BE7 RID: 3047
	internal class AppInGame
	{
		// Token: 0x170013F8 RID: 5112
		// (get) Token: 0x060060A1 RID: 24737 RVA: 0x001F9EF0 File Offset: 0x001F80F0
		// (set) Token: 0x060060A2 RID: 24738 RVA: 0x001F9EF8 File Offset: 0x001F80F8
		public GameInstance Instance { get; private set; }

		// Token: 0x170013F9 RID: 5113
		// (get) Token: 0x060060A3 RID: 24739 RVA: 0x001F9F01 File Offset: 0x001F8101
		// (set) Token: 0x060060A4 RID: 24740 RVA: 0x001F9F09 File Offset: 0x001F8109
		public AppInGame.InGameOverlay CurrentOverlay { get; private set; }

		// Token: 0x170013FA RID: 5114
		// (get) Token: 0x060060A5 RID: 24741 RVA: 0x001F9F12 File Offset: 0x001F8112
		// (set) Token: 0x060060A6 RID: 24742 RVA: 0x001F9F1A File Offset: 0x001F811A
		public Page CurrentPage { get; private set; }

		// Token: 0x170013FB RID: 5115
		// (get) Token: 0x060060A7 RID: 24743 RVA: 0x001F9F23 File Offset: 0x001F8123
		// (set) Token: 0x060060A8 RID: 24744 RVA: 0x001F9F2B File Offset: 0x001F812B
		public bool IsToolsSettingsModalOpened { get; private set; } = false;

		// Token: 0x170013FC RID: 5116
		// (get) Token: 0x060060A9 RID: 24745 RVA: 0x001F9F34 File Offset: 0x001F8134
		// (set) Token: 0x060060AA RID: 24746 RVA: 0x001F9F3C File Offset: 0x001F813C
		public bool WasCurrentPageOpenedViaInteractionBinding { get; private set; }

		// Token: 0x170013FD RID: 5117
		// (get) Token: 0x060060AB RID: 24747 RVA: 0x001F9F45 File Offset: 0x001F8145
		// (set) Token: 0x060060AC RID: 24748 RVA: 0x001F9F4D File Offset: 0x001F814D
		public bool IsHudVisible { get; private set; } = true;

		// Token: 0x170013FE RID: 5118
		// (get) Token: 0x060060AD RID: 24749 RVA: 0x001F9F56 File Offset: 0x001F8156
		// (set) Token: 0x060060AE RID: 24750 RVA: 0x001F9F5E File Offset: 0x001F815E
		public bool IsFirstPersonViewVisible { get; private set; } = true;

		// Token: 0x170013FF RID: 5119
		// (get) Token: 0x060060AF RID: 24751 RVA: 0x001F9F67 File Offset: 0x001F8167
		// (set) Token: 0x060060B0 RID: 24752 RVA: 0x001F9F6F File Offset: 0x001F816F
		public bool IsPlayerListVisible { get; private set; }

		// Token: 0x17001400 RID: 5120
		// (get) Token: 0x060060B1 RID: 24753 RVA: 0x001F9F78 File Offset: 0x001F8178
		// (set) Token: 0x060060B2 RID: 24754 RVA: 0x001F9F80 File Offset: 0x001F8180
		public string ServerName { get; set; }

		// Token: 0x17001401 RID: 5121
		// (get) Token: 0x060060B3 RID: 24755 RVA: 0x001F9F89 File Offset: 0x001F8189
		// (set) Token: 0x060060B4 RID: 24756 RVA: 0x001F9F91 File Offset: 0x001F8191
		public AppInGame.ItemSelector ActiveItemSelector { get; private set; } = AppInGame.ItemSelector.None;

		// Token: 0x060060B5 RID: 24757 RVA: 0x001F9F9A File Offset: 0x001F819A
		public AppInGame(App app)
		{
			this._app = app;
		}

		// Token: 0x060060B6 RID: 24758 RVA: 0x001F9FC7 File Offset: 0x001F81C7
		internal void CreateInstance(ConnectionToServer connection)
		{
			this.Instance = new GameInstance(this._app, connection);
		}

		// Token: 0x060060B7 RID: 24759 RVA: 0x001F9FDD File Offset: 0x001F81DD
		internal void DisposeAndClearInstance()
		{
			GameInstance instance = this.Instance;
			if (instance != null)
			{
				instance.Dispose();
			}
			this.Instance = null;
		}

		// Token: 0x060060B8 RID: 24760 RVA: 0x001F9FFC File Offset: 0x001F81FC
		public void Reset(bool isStayingConnected)
		{
			this.CurrentOverlay = AppInGame.InGameOverlay.None;
			this.CurrentPage = 0;
			this.IsHudVisible = true;
			this.IsFirstPersonViewVisible = true;
			this.IsPlayerListVisible = false;
			this.ActiveItemSelector = AppInGame.ItemSelector.None;
			this.ResetInventoryState();
			this._app.Interface.InGameView.OnReset(isStayingConnected);
		}

		// Token: 0x060060B9 RID: 24761 RVA: 0x001FA058 File Offset: 0x001F8258
		public void Open()
		{
			Debug.Assert(this._app.Stage == App.AppStage.MainMenu || this._app.Stage == App.AppStage.GameLoading);
			this.Instance.Connection.OnDisconnected = new Action<Exception>(this.OnDisconnectedWithError);
			this._app.SetStage(App.AppStage.InGame);
			bool flag = this.CurrentPage > 0;
			if (flag)
			{
				this._app.Interface.InGameView.OnPageChanged();
			}
			this.UpdateInputStates(false);
		}

		// Token: 0x060060BA RID: 24762 RVA: 0x001FA0E0 File Offset: 0x001F82E0
		internal void CleanUp()
		{
			this.PrepareToExit();
			this._app.CoUIManager.SetFocusedWebView(null);
			this._app.Interface.Desktop.IsFocused = true;
			this.Reset(false);
			this.DisposeAndClearInstance();
			this._isCapturingWorldPreviewBeforeExit = false;
			this._hasPreparedToExit = false;
		}

		// Token: 0x060060BB RID: 24763 RVA: 0x001FA13C File Offset: 0x001F833C
		internal void OnNewFrame(float deltaTime)
		{
			Engine engine = this._app.Engine;
			GLFunctions gl = engine.Graphics.GL;
			bool disposed = this.Instance.Disposed;
			if (disposed)
			{
				this._app.Interface.PrepareForDraw();
				gl.Disable(GL.DEPTH_TEST);
				gl.BlendFunc(GL.ONE, GL.ONE_MINUS_SRC_ALPHA);
				gl.Viewport(engine.Window.Viewport);
				this._app.Interface.Draw();
				bool flag = this._worldPreviewScreenshotFenceSync != IntPtr.Zero;
				if (flag)
				{
					this.FetchWorldPreviewScreenshot(this._isExitingApplication);
				}
				bool isExitingApplication = this._isExitingApplication;
				if (isExitingApplication)
				{
					this._app.Exit();
				}
				else
				{
					this._app.MainMenu.Open(AppMainMenu.MainMenuPage.Home);
				}
			}
			else
			{
				this.Instance.Input.UpdateBindings();
				deltaTime *= this.Instance.TimeDilationModifier;
				this.Instance.ProfilingModule.SetDrawCallStats(gl.DrawCallsCount, gl.DrawnVertices / 3);
				gl.ResetDrawCallStats();
				engine.Profiling.SwapMeasureBuffers();
				engine.Profiling.StartMeasure(0);
				engine.Profiling.StartMeasure(2);
				this.Instance.OnNewFrame(deltaTime, engine.Window.GetState() != Window.WindowState.Minimized);
				engine.Profiling.StopMeasure(2);
				engine.Profiling.StartMeasure(42);
				gl.ClearColor(0f, 0f, 0f, 1f);
				gl.Clear((GL)16640U);
				bool isReadyToDraw = this.Instance.IsReadyToDraw;
				if (isReadyToDraw)
				{
					gl.Enable(GL.DEPTH_TEST);
					gl.DepthMask(true);
					this.Instance.DrawScene();
					gl.Disable(GL.BLEND);
					gl.Viewport(engine.Window.Viewport);
					this.Instance.DrawPostEffect();
					bool isCapturingWorldPreviewBeforeExit = this._isCapturingWorldPreviewBeforeExit;
					if (isCapturingWorldPreviewBeforeExit)
					{
						this.CaptureWorldPreviewScreenshot();
					}
				}
				gl.Disable(GL.DEPTH_TEST);
				gl.Enable(GL.BLEND);
				this.Instance.DrawAfterPostEffect();
				engine.Profiling.StopMeasure(42);
				engine.Profiling.StopMeasure(0);
				gl.UseProgram(engine.Graphics.GPUProgramStore.BasicProgram);
				gl.BlendFunc(GL.ONE, GL.ONE_MINUS_SRC_ALPHA);
				this._app.Interface.Draw();
				gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
				gl.Enable(GL.DEPTH_TEST);
				gl.Clear(GL.DEPTH_BUFFER_BIT);
				this.Instance.DrawAfterInterface();
				this.Instance.Input.EndUserInput();
				bool isCapturingWorldPreviewBeforeExit2 = this._isCapturingWorldPreviewBeforeExit;
				if (isCapturingWorldPreviewBeforeExit2)
				{
					this.PrepareToExit();
				}
			}
		}

		// Token: 0x060060BC RID: 24764 RVA: 0x001FA42C File Offset: 0x001F862C
		private void PrepareToExit()
		{
			bool hasPreparedToExit = this._hasPreparedToExit;
			if (!hasPreparedToExit)
			{
				this._hasPreparedToExit = true;
				this.Instance.Connection.SendPacketImmediate(new Disconnect("Player leave", 0));
				this.Instance.Connection.Close();
				bool flag = this._app.SingleplayerServer != null;
				if (flag)
				{
					bool flag2 = !this._app.SingleplayerServer.Process.HasExited;
					if (flag2)
					{
						this._app.SingleplayerServer.Close();
					}
					this._app.OnSinglePlayerServerShuttingDown();
				}
				this.Instance.Dispose();
				this._app.Engine.Window.SetMouseLock(false);
			}
		}

		// Token: 0x060060BD RID: 24765 RVA: 0x001FA4F0 File Offset: 0x001F86F0
		public void UpdateInputStates(bool skipResetKeys = false)
		{
			bool flag = this.IsToolsSettingsModalOpened || this.CurrentPage != null || this.CurrentOverlay != AppInGame.InGameOverlay.None || this.Instance.Chat.IsOpen || this._app.DevTools.IsOpen || this._app.Interface.InGameView.UtilitySlotSelector.IsMounted || this._app.Interface.InGameView.ConsumableSlotSelector.IsMounted || this._app.Interface.InGameView.BuilderToolsMaterialSlotSelector.IsMounted;
			if (flag)
			{
				bool flag2 = this.CurrentPage == null && (this._app.Interface.InGameView.UtilitySlotSelector.IsMounted || this._app.Interface.InGameView.ConsumableSlotSelector.IsMounted || this._app.Interface.InGameView.BuilderToolsMaterialSlotSelector.IsMounted);
				this._app.Engine.Window.SetMouseLock(flag2);
				this.Instance.Input.MouseInputDisabled = flag2;
				bool flag3 = (this.CurrentPage == 6 || flag2) && this.CurrentOverlay == AppInGame.InGameOverlay.None && !this.Instance.Chat.IsOpen;
				if (flag3)
				{
					this.Instance.Input.KeyInputDisabled = false;
				}
				else
				{
					this.Instance.Input.KeyInputDisabled = true;
					bool flag4 = !skipResetKeys;
					if (flag4)
					{
						this.Instance.Input.ResetKeys();
					}
				}
				this.Instance.Input.ResetMouseButtons();
				bool flag5 = this.CurrentOverlay == AppInGame.InGameOverlay.MachinimaEditor;
				this._app.CoUIManager.SetFocusedWebView(flag5 ? this.Instance.EditorWebViewModule.WebView : null);
				this._app.Interface.Desktop.IsFocused = !flag5;
			}
			else
			{
				this.Instance.Input.MouseInputDisabled = false;
				this._app.Engine.Window.SetMouseLock(true);
				this.Instance.Input.KeyInputDisabled = false;
				this.Instance.Input.ResetMouseButtons();
				bool flag6 = !skipResetKeys;
				if (flag6)
				{
					this.Instance.Input.ResetKeys();
				}
				this._app.CoUIManager.SetFocusedWebView(null);
				this._app.Interface.Desktop.IsFocused = false;
			}
		}

		// Token: 0x060060BE RID: 24766 RVA: 0x001FA78C File Offset: 0x001F898C
		public void RequestExit(bool exitApplication = false)
		{
			this._isExitingApplication = exitApplication;
			bool flag = this._app.SingleplayerServer != null && this.Instance.IsPlaying;
			if (flag)
			{
				this._isCapturingWorldPreviewBeforeExit = true;
			}
			else if (exitApplication)
			{
				this._app.Exit();
			}
			else
			{
				this._app.MainMenu.Open(AppMainMenu.MainMenuPage.Home);
			}
		}

		// Token: 0x060060BF RID: 24767 RVA: 0x001FA7F4 File Offset: 0x001F89F4
		private void OnDisconnectedWithError(Exception exception)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			bool hasPreparedToExit = this._hasPreparedToExit;
			if (!hasPreparedToExit)
			{
				bool flag = this._app.Stage != App.AppStage.InGame;
				if (!flag)
				{
					AppInGame.Logger.Info("Disconnected with error:");
					AppInGame.Logger.Error<Exception>(exception);
					this._app.MainMenu.SetPageToReturnTo(AppMainMenu.MainMenuPage.Home);
					this._app.Disconnection.Open(exception.Message, this.Instance.Connection.Hostname, this.Instance.Connection.Port);
				}
			}
		}

		// Token: 0x060060C0 RID: 24768 RVA: 0x001FA898 File Offset: 0x001F8A98
		public byte[] GetAsset(string name)
		{
			string hash;
			bool flag = !this.Instance.HashesByServerAssetPath.TryGetValue(name, ref hash);
			byte[] result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = AssetManager.GetAssetUsingHash(hash, false);
			}
			return result;
		}

		// Token: 0x060060C1 RID: 24769 RVA: 0x001FA8CF File Offset: 0x001F8ACF
		public void OnChatOpenChanged()
		{
			this.UpdateInputStates(false);
		}

		// Token: 0x17001402 RID: 5122
		// (get) Token: 0x060060C2 RID: 24770 RVA: 0x001FA8DA File Offset: 0x001F8ADA
		public bool HasUnclosablePage
		{
			get
			{
				return this.CurrentPage == 7 && this._customPageLifetime == 0;
			}
		}

		// Token: 0x060060C3 RID: 24771 RVA: 0x001FA8F4 File Offset: 0x001F8AF4
		public void TryClosePageOrOverlay()
		{
			bool flag = this.CurrentOverlay > AppInGame.InGameOverlay.None;
			if (flag)
			{
				this.SetCurrentOverlay(AppInGame.InGameOverlay.None);
			}
			else
			{
				this.TryClosePage();
			}
		}

		// Token: 0x060060C4 RID: 24772 RVA: 0x001FA920 File Offset: 0x001F8B20
		public void TryClosePage()
		{
			Page currentPage = this.CurrentPage;
			Page page = currentPage;
			if (page != null)
			{
				if (page != 2)
				{
					if (page != 7)
					{
						this.SetCurrentPage(0, false, true);
					}
					else
					{
						bool flag = this._customPageLifetime == 0;
						if (flag)
						{
							this.SetCurrentOverlay(AppInGame.InGameOverlay.InGameMenu);
						}
						else
						{
							this._app.Interface.InGameView.CustomPage.StartLoading();
							this.Instance.Connection.SendPacket(new CustomPageEvent(2, null));
						}
					}
				}
				else
				{
					this.SetCurrentPage(0, false, true);
				}
			}
		}

		// Token: 0x060060C5 RID: 24773 RVA: 0x001FA9B4 File Offset: 0x001F8BB4
		public void SetCurrentPage(Page page, bool wasOpenedWithInteractionBinding = false, bool playSound = false)
		{
			bool flag = page == 7;
			if (flag)
			{
				throw new Exception("OpenCustomPage must be used for Page of type Custom");
			}
			bool flag2 = page == 3 && this.Instance.ImmersiveScreenModule.ActiveWebScreen == null;
			if (!flag2)
			{
				bool flag3 = page == 6 || page == 7;
				if (flag3)
				{
					this.CloseToolsSettingsModal();
				}
				bool flag4 = this.CurrentPage == 7;
				if (flag4)
				{
					this.Instance.Connection.SendPacket(new CustomPageEvent(0, null));
				}
				Page currentPage = this.CurrentPage;
				this.CurrentPage = page;
				this.WasCurrentPageOpenedViaInteractionBinding = wasOpenedWithInteractionBinding;
				bool flag5 = this._app.Stage == App.AppStage.InGame;
				if (flag5)
				{
					if (playSound)
					{
						bool flag6 = this.CurrentPage == 0;
						if (flag6)
						{
							this._app.Interface.InGameView.PlayPageCloseSound(currentPage);
						}
						else
						{
							this._app.Interface.InGameView.PlayPageOpenSound(this.CurrentPage);
						}
					}
					this._app.Interface.InGameView.OnPageChanged();
					this.UpdateInputStates(this.CurrentPage == null && currentPage == 6);
				}
				bool flag7 = this.ActiveItemSelector > AppInGame.ItemSelector.None;
				if (flag7)
				{
					this.SetActiveItemSelector(AppInGame.ItemSelector.None);
				}
			}
		}

		// Token: 0x060060C6 RID: 24774 RVA: 0x001FAAF4 File Offset: 0x001F8CF4
		public void CloseToolsSettingsModal()
		{
			this.IsToolsSettingsModalOpened = false;
			bool flag = this._app.Stage == App.AppStage.InGame;
			if (flag)
			{
				this._app.Interface.InGameView.CloseToolsSettingsModal();
				this.UpdateInputStates(false);
			}
		}

		// Token: 0x060060C7 RID: 24775 RVA: 0x001FAB3C File Offset: 0x001F8D3C
		public void OpenToolsSettingsPage()
		{
			this.IsToolsSettingsModalOpened = true;
			bool flag = this._app.Stage == App.AppStage.InGame;
			if (flag)
			{
				this._app.Interface.InGameView.OpenToolsSettingsPage();
				this.UpdateInputStates(false);
				bool flag2 = this.CurrentPage == 6;
				if (flag2)
				{
					this.SetCurrentPage(0, false, false);
				}
			}
		}

		// Token: 0x060060C8 RID: 24776 RVA: 0x001FAB9C File Offset: 0x001F8D9C
		public void ToogleToolById(string toolId)
		{
			ClientItemStack activeToolsItem = this._app.InGame.Instance.InventoryModule.GetActiveToolsItem();
			bool flag = ((activeToolsItem != null) ? activeToolsItem.Id : null) == toolId && this.IsToolsSettingsModalOpened;
			if (flag)
			{
				this.CloseToolsSettingsModal();
			}
			else
			{
				this._app.Interface.InGameView.ToolsSettingsPage.SelectToolById(toolId);
				this.OpenToolsSettingsPage();
			}
		}

		// Token: 0x060060C9 RID: 24777 RVA: 0x001FAC14 File Offset: 0x001F8E14
		public void OpenOrUpdateCustomPage(CustomPage packet)
		{
			this.Instance.Connection.SendPacket(new CustomPageEvent(0, null));
			this._customPageLifetime = packet.Lifetime;
			this.WasCurrentPageOpenedViaInteractionBinding = (packet.Lifetime == 2);
			this._app.Interface.InGameView.CustomPage.Apply(packet);
			bool flag = this.CurrentPage != 7;
			if (flag)
			{
				this.CurrentPage = 7;
				bool flag2 = this._app.Stage == App.AppStage.InGame;
				if (flag2)
				{
					this._app.Interface.InGameView.OnPageChanged();
					this.UpdateInputStates(false);
				}
			}
		}

		// Token: 0x060060CA RID: 24778 RVA: 0x001FACBE File Offset: 0x001F8EBE
		public void UpdateCustomHud(CustomHud packet)
		{
			this._app.Interface.InGameView.CustomHud.Apply(packet);
		}

		// Token: 0x060060CB RID: 24779 RVA: 0x001FACDD File Offset: 0x001F8EDD
		public void SetActiveItemSelector(AppInGame.ItemSelector itemSelector)
		{
			this.ActiveItemSelector = itemSelector;
			this._app.Interface.InGameView.OnActiveItemSelectorChanged();
			this.UpdateInputStates(true);
		}

		// Token: 0x060060CC RID: 24780 RVA: 0x001FAD08 File Offset: 0x001F8F08
		public void SetCurrentOverlay(AppInGame.InGameOverlay overlay)
		{
			this.CurrentOverlay = overlay;
			this._app.Interface.InGameView.OnOverlayChanged();
			this.UpdateInputStates(false);
			bool flag = this.ActiveItemSelector > AppInGame.ItemSelector.None;
			if (flag)
			{
				this.SetActiveItemSelector(AppInGame.ItemSelector.None);
			}
		}

		// Token: 0x060060CD RID: 24781 RVA: 0x001FAD54 File Offset: 0x001F8F54
		public void SwitchHudVisibility()
		{
			bool isHudVisible = this.IsHudVisible;
			if (isHudVisible)
			{
				this.IsHudVisible = false;
				this._app.DevTools.ClearNotifications();
			}
			else
			{
				bool isFirstPersonViewVisible = this.IsFirstPersonViewVisible;
				if (isFirstPersonViewVisible)
				{
					this.IsFirstPersonViewVisible = false;
				}
				else
				{
					this.IsHudVisible = (this.IsFirstPersonViewVisible = true);
				}
			}
			this._app.Interface.InGameView.OnHudVisibilityChanged();
		}

		// Token: 0x060060CE RID: 24782 RVA: 0x001FADC5 File Offset: 0x001F8FC5
		public void SetPlayerListVisible(bool visible)
		{
			this.IsPlayerListVisible = visible;
			this._app.Interface.InGameView.OnPlayerListVisibilityChanged();
		}

		// Token: 0x060060CF RID: 24783 RVA: 0x001FADE6 File Offset: 0x001F8FE6
		public void SetSceneBlurEnabled(bool enabled)
		{
			this.Instance.PostEffectRenderer.UseBlur(enabled);
		}

		// Token: 0x060060D0 RID: 24784 RVA: 0x001FADFB File Offset: 0x001F8FFB
		public void SendCustomPageData(JObject data)
		{
			this.Instance.Connection.SendPacket(new CustomPageEvent(1, BsonHelper.ToBson(data)));
		}

		// Token: 0x060060D1 RID: 24785 RVA: 0x001FAE1C File Offset: 0x001F901C
		public void SendChatMessageOrExecuteCommand(string message)
		{
			bool flag = message.StartsWith(".");
			if (flag)
			{
				this.Instance.ExecuteCommand(message);
			}
			else
			{
				this.Instance.Chat.SendMessage(message);
			}
		}

		// Token: 0x060060D2 RID: 24786 RVA: 0x001FAE5A File Offset: 0x001F905A
		public void OpenAssetEditor()
		{
			this.OpenEditor(new JObject());
		}

		// Token: 0x060060D3 RID: 24787 RVA: 0x001FAE69 File Offset: 0x001F9069
		public void OpenAssetPathInAssetEditor(string assetPath)
		{
			JObject jobject = new JObject();
			jobject.Add("AssetPath", assetPath);
			this.OpenEditor(jobject);
		}

		// Token: 0x060060D4 RID: 24788 RVA: 0x001FAE8A File Offset: 0x001F908A
		public void OpenAssetIdInAssetEditor(string assetType, string assetId)
		{
			JObject jobject = new JObject();
			jobject.Add("AssetType", assetType);
			jobject.Add("AssetId", assetId);
			this.OpenEditor(jobject);
		}

		// Token: 0x060060D5 RID: 24789 RVA: 0x001FAEC0 File Offset: 0x001F90C0
		private void OpenEditor(JObject data)
		{
			data["Name"] = (this.ServerName ?? string.Format("{0}:{1}", this.Instance.Connection.Hostname, this.Instance.Connection.Port));
			data["Hostname"] = this.Instance.Connection.Hostname;
			data["Port"] = this.Instance.Connection.Port;
			this.Instance.Connection.SendPacket(new AssetEditorInitialize());
			this._app.Ipc.SendCommand("OpenEditor", data);
		}

		// Token: 0x17001403 RID: 5123
		// (get) Token: 0x060060D6 RID: 24790 RVA: 0x001FAF87 File Offset: 0x001F9187
		// (set) Token: 0x060060D7 RID: 24791 RVA: 0x001FAF8F File Offset: 0x001F918F
		public ClientItemCategory[] ItemCategories { get; private set; }

		// Token: 0x060060D8 RID: 24792 RVA: 0x001FAF98 File Offset: 0x001F9198
		private void ResetInventoryState()
		{
			this.ItemCategories = null;
		}

		// Token: 0x060060D9 RID: 24793 RVA: 0x001FAFA4 File Offset: 0x001F91A4
		public void SendSetCreativeItemPacket(int section, int slot, ClientItemStack itemStack, bool overwrite = false)
		{
			Item item = itemStack.ToItemPacket(false);
			this.Instance.Connection.SendPacket(new SetCreativeItem(new InventoryPosition(section, slot, item), overwrite));
		}

		// Token: 0x060060DA RID: 24794 RVA: 0x001FAFDC File Offset: 0x001F91DC
		public void SendSmartGiveCreativeItemPacket(ClientItemStack itemStack, SmartMoveType moveType)
		{
			Item item = itemStack.ToItemPacket(false);
			this.Instance.Connection.SendPacket(new SmartGiveCreativeItem(item, moveType));
		}

		// Token: 0x060060DB RID: 24795 RVA: 0x001FB00C File Offset: 0x001F920C
		public void SendMoveItemStackPacket(ClientItemStack itemStack, int sourceSectionId, int sourceSlotId, int targetSectionId, int targetSlotId)
		{
			Item item = itemStack.ToItemPacket(false);
			this.Instance.Connection.SendPacket(new MoveItemStack(new InventoryPosition(sourceSectionId, sourceSlotId, item), new InventoryPosition(targetSectionId, targetSlotId, null)));
		}

		// Token: 0x060060DC RID: 24796 RVA: 0x001FB04C File Offset: 0x001F924C
		public void SendSmartMoveItemStackPacket(ClientItemStack itemStack, int sourceSectionId, int sourceSlotId, SmartMoveType moveType)
		{
			Item item = itemStack.ToItemPacket(false);
			this.Instance.Connection.SendPacket(new SmartMoveItemStack(new InventoryPosition(sourceSectionId, sourceSlotId, item), moveType));
		}

		// Token: 0x060060DD RID: 24797 RVA: 0x001FB082 File Offset: 0x001F9282
		public void SendTakeAllItemStacksPacket(int inventorySectionId)
		{
			this.Instance.Connection.SendPacket(new TakeAllItemStacks(inventorySectionId));
		}

		// Token: 0x060060DE RID: 24798 RVA: 0x001FB09C File Offset: 0x001F929C
		public void SendDropItemStackPacket(ClientItemStack itemStack, int sectionId, int slotId)
		{
			Item item = itemStack.ToItemPacket(true);
			this.Instance.Connection.SendPacket(new DropItemStack(new InventoryPosition(sectionId, slotId, item)));
		}

		// Token: 0x060060DF RID: 24799 RVA: 0x001FB0D0 File Offset: 0x001F92D0
		public void SendOpenInventoryPacket()
		{
			this.Instance.Connection.SendPacket(new OpenInventory());
		}

		// Token: 0x060060E0 RID: 24800 RVA: 0x001FB0E8 File Offset: 0x001F92E8
		public void SendCloseWindowPacket(int id)
		{
			this.Instance.Connection.SendPacket(new CloseWindow(id));
		}

		// Token: 0x060060E1 RID: 24801 RVA: 0x001FB101 File Offset: 0x001F9301
		public void SendSendWindowActionPacket(int windowId, string key, string data)
		{
			this.Instance.Connection.SendPacket(new SendWindowAction(windowId, key, data));
		}

		// Token: 0x060060E2 RID: 24802 RVA: 0x001FB11C File Offset: 0x001F931C
		public void SendSortInventoryPacket(SortType sortType)
		{
			this.Instance.Connection.SendPacket(new SortInventory(sortType));
		}

		// Token: 0x060060E3 RID: 24803 RVA: 0x001FB138 File Offset: 0x001F9338
		public void OnItemCategoriesInitialized(ItemCategory[] categories)
		{
			this.ItemCategories = new ClientItemCategory[categories.Length];
			for (int i = 0; i < categories.Length; i++)
			{
				this.ItemCategories[i] = new ClientItemCategory(categories[i]);
			}
			Array.Sort<ClientItemCategory>(this.ItemCategories, (ClientItemCategory a, ClientItemCategory b) => a.Order - b.Order);
			ItemLibraryPanel itemLibraryPanel = this._app.Interface.InGameView.InventoryPage.ItemLibraryPanel;
			itemLibraryPanel.EnsureValidCategorySelected();
			bool flag = this._app.Stage == App.AppStage.InGame;
			if (flag)
			{
				itemLibraryPanel.SetupCategories();
			}
		}

		// Token: 0x060060E4 RID: 24804 RVA: 0x001FB1E0 File Offset: 0x001F93E0
		public bool HandleHotbarLoad(sbyte hotbarIndex)
		{
			GameInstance instance = this._app.InGame.Instance;
			bool flag = instance.GameMode == 1;
			bool result;
			if (flag)
			{
				instance.Connection.SendPacket(new LoadHotbar(hotbarIndex));
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060060E5 RID: 24805 RVA: 0x001FB228 File Offset: 0x001F9428
		public void OnItemCategoriesAdded(ItemCategory[] categories)
		{
			Dictionary<string, ClientItemCategory> dictionary = new Dictionary<string, ClientItemCategory>();
			foreach (ClientItemCategory clientItemCategory in this.ItemCategories)
			{
				dictionary[clientItemCategory.Id] = clientItemCategory;
			}
			foreach (ItemCategory itemCategory in categories)
			{
				dictionary[itemCategory.Id] = new ClientItemCategory(itemCategory);
			}
			this.ItemCategories = Enumerable.ToArray<ClientItemCategory>(dictionary.Values);
			Array.Sort<ClientItemCategory>(this.ItemCategories, (ClientItemCategory a, ClientItemCategory b) => a.Order - b.Order);
			this._app.Interface.InGameView.InventoryPage.ItemLibraryPanel.SetupCategories();
		}

		// Token: 0x060060E6 RID: 24806 RVA: 0x001FB2F4 File Offset: 0x001F94F4
		public void OnItemCategoriesRemoved(ItemCategory[] categories)
		{
			string[] array = new string[categories.Length];
			for (int i = 0; i < categories.Length; i++)
			{
				array[i] = categories[i].Id;
			}
			List<ClientItemCategory> list = new List<ClientItemCategory>();
			foreach (ClientItemCategory clientItemCategory in this.ItemCategories)
			{
				bool flag = Enumerable.Contains<string>(array, clientItemCategory.Id);
				if (!flag)
				{
					list.Add(clientItemCategory);
				}
			}
			this.ItemCategories = list.ToArray();
			this._app.Interface.InGameView.InventoryPage.ItemLibraryPanel.SetupCategories();
		}

		// Token: 0x060060E7 RID: 24807 RVA: 0x001FB3A0 File Offset: 0x001F95A0
		private void FetchWorldPreviewScreenshot(bool saveFromMainThread)
		{
			AppInGame.<>c__DisplayClass108_0 CS$<>8__locals1 = new AppInGame.<>c__DisplayClass108_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.saveFromMainThread = saveFromMainThread;
			Debug.Assert(this._worldPreviewScreenshotFenceSync != IntPtr.Zero);
			AppInGame.Logger.Info("Capturing world preview screenshot...");
			GLFunctions gl = this._app.Engine.Graphics.GL;
			IntPtr value;
			gl.GetSynciv(this._worldPreviewScreenshotFenceSync, GL.SYNC_STATUS, (IntPtr)8, IntPtr.Zero, out value);
			bool flag = (int)value != 37145;
			if (!flag)
			{
				gl.BindVertexArray(GLVertexArray.None);
				gl.BindBuffer(GLVertexArray.None, GL.PIXEL_PACK_BUFFER, this._worldPreviewScreenshotPixelBuffer);
				IntPtr pointer = gl.MapBufferRange(GL.PIXEL_PACK_BUFFER, IntPtr.Zero, (IntPtr)(this._worldPreviewOutputWidth * 360 * 4), GL.ONE);
				CS$<>8__locals1.rawPixels = new byte[this._worldPreviewOutputWidth * 360 * 4];
				for (int i = 0; i < 360; i++)
				{
					Marshal.Copy(pointer + i * this._worldPreviewOutputWidth * 4, CS$<>8__locals1.rawPixels, (360 - i - 1) * this._worldPreviewOutputWidth * 4, this._worldPreviewOutputWidth * 4);
				}
				gl.UnmapBuffer(GL.PIXEL_PACK_BUFFER);
				gl.BindBuffer(GLVertexArray.None, GL.PIXEL_PACK_BUFFER, GLBuffer.None);
				gl.DeleteBuffer(this._worldPreviewScreenshotPixelBuffer);
				gl.DeleteSync(this._worldPreviewScreenshotFenceSync);
				gl.DeleteFramebuffer(this._worldPreviewFramebuffer);
				gl.DeleteTexture(this._worldPreviewTexture);
				this._worldPreviewScreenshotPixelBuffer = GLBuffer.None;
				this._worldPreviewScreenshotFenceSync = IntPtr.Zero;
				CS$<>8__locals1.width = this._worldPreviewOutputWidth;
				CS$<>8__locals1.worldDirectoryName = this._app.SingleplayerWorldName;
				CS$<>8__locals1.filePath = Path.Combine(Paths.Saves, CS$<>8__locals1.worldDirectoryName, "preview.png");
				CS$<>8__locals1.tmpFilePath = Path.Combine(Paths.Saves, CS$<>8__locals1.worldDirectoryName, "preview.tmp.png");
				bool saveFromMainThread2 = CS$<>8__locals1.saveFromMainThread;
				if (saveFromMainThread2)
				{
					CS$<>8__locals1.<FetchWorldPreviewScreenshot>g__SaveFile|1();
				}
				else
				{
					ThreadPool.QueueUserWorkItem(delegate(object _)
					{
						base.<FetchWorldPreviewScreenshot>g__SaveFile|1();
					});
				}
			}
		}

		// Token: 0x060060E8 RID: 24808 RVA: 0x001FB5EC File Offset: 0x001F97EC
		private void CaptureWorldPreviewScreenshot()
		{
			Debug.Assert(this._isCapturingWorldPreviewBeforeExit && this._worldPreviewScreenshotFenceSync == IntPtr.Zero);
			Rectangle viewport = this._app.Engine.Window.Viewport;
			GLFunctions gl = this._app.Engine.Graphics.GL;
			this._worldPreviewOutputWidth = (int)((float)viewport.Width / (float)viewport.Height * 360f);
			gl.AssertActiveTexture(GL.TEXTURE0);
			this._worldPreviewTexture = gl.GenTexture();
			gl.BindTexture(GL.TEXTURE_2D, this._worldPreviewTexture);
			gl.TexImage2D(GL.TEXTURE_2D, 0, 32856, this._worldPreviewOutputWidth, 360, 0, GL.RGBA, GL.UNSIGNED_BYTE, IntPtr.Zero);
			this._worldPreviewFramebuffer = gl.GenFramebuffer();
			gl.BindFramebuffer(GL.DRAW_FRAMEBUFFER, this._worldPreviewFramebuffer);
			gl.FramebufferTexture2D(GL.FRAMEBUFFER, GL.COLOR_ATTACHMENT0, GL.TEXTURE_2D, this._worldPreviewTexture, 0);
			glDrawBuffers drawBuffers = gl.DrawBuffers;
			int n = 1;
			GL[] array = new GL[4];
			array[0] = GL.COLOR_ATTACHMENT0;
			drawBuffers(n, array);
			GL gl2 = gl.CheckFramebufferStatus(GL.FRAMEBUFFER);
			bool flag = gl2 != GL.FRAMEBUFFER_COMPLETE;
			if (flag)
			{
				throw new Exception("Incomplete Framebuffer object, status: " + gl2.ToString());
			}
			gl.BlitFramebuffer(0, 0, viewport.Width, viewport.Height, 0, 0, this._worldPreviewOutputWidth, 360, GL.COLOR_BUFFER_BIT, GL.LINEAR);
			gl.BindFramebuffer(GL.READ_FRAMEBUFFER, this._worldPreviewFramebuffer);
			gl.ReadBuffer(GL.COLOR_ATTACHMENT0);
			gl.BindVertexArray(GLVertexArray.None);
			this._worldPreviewScreenshotPixelBuffer = gl.GenBuffer();
			gl.BindBuffer(GLVertexArray.None, GL.PIXEL_PACK_BUFFER, this._worldPreviewScreenshotPixelBuffer);
			gl.BufferData(GL.PIXEL_PACK_BUFFER, (IntPtr)(this._worldPreviewOutputWidth * 360 * 4), IntPtr.Zero, GL.DYNAMIC_READ);
			gl.ReadPixels(0, 0, this._worldPreviewOutputWidth, 360, GL.BGRA, GL.UNSIGNED_BYTE, IntPtr.Zero);
			gl.BindBuffer(GLVertexArray.None, GL.PIXEL_PACK_BUFFER, GLBuffer.None);
			gl.BindFramebuffer(GL.FRAMEBUFFER, GLFramebuffer.None);
			this._worldPreviewScreenshotFenceSync = gl.FenceSync(GL.SYNC_GPU_COMMANDS_COMPLETE, GL.NO_ERROR);
			bool flag2 = this._worldPreviewScreenshotFenceSync == IntPtr.Zero;
			if (flag2)
			{
				throw new Exception("Failed to get fence sync!");
			}
		}

		// Token: 0x04003C21 RID: 15393
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003C22 RID: 15394
		private readonly App _app;

		// Token: 0x04003C28 RID: 15400
		private CustomPageLifetime _customPageLifetime;

		// Token: 0x04003C2E RID: 15406
		private bool _isCapturingWorldPreviewBeforeExit;

		// Token: 0x04003C2F RID: 15407
		private bool _hasPreparedToExit;

		// Token: 0x04003C30 RID: 15408
		private bool _isExitingApplication;

		// Token: 0x04003C32 RID: 15410
		private GLBuffer _worldPreviewScreenshotPixelBuffer;

		// Token: 0x04003C33 RID: 15411
		private IntPtr _worldPreviewScreenshotFenceSync;

		// Token: 0x04003C34 RID: 15412
		private GLTexture _worldPreviewTexture;

		// Token: 0x04003C35 RID: 15413
		private GLFramebuffer _worldPreviewFramebuffer;

		// Token: 0x04003C36 RID: 15414
		private const int WorldPreviewColorChannels = 4;

		// Token: 0x04003C37 RID: 15415
		private const int WorldPreviewOutputHeight = 360;

		// Token: 0x04003C38 RID: 15416
		private int _worldPreviewOutputWidth;

		// Token: 0x0200102B RID: 4139
		public enum InGameOverlay
		{
			// Token: 0x04004D48 RID: 19784
			None,
			// Token: 0x04004D49 RID: 19785
			InGameMenu,
			// Token: 0x04004D4A RID: 19786
			MachinimaEditor,
			// Token: 0x04004D4B RID: 19787
			ConfirmQuit
		}

		// Token: 0x0200102C RID: 4140
		public enum ItemSelector
		{
			// Token: 0x04004D4D RID: 19789
			None,
			// Token: 0x04004D4E RID: 19790
			Utility,
			// Token: 0x04004D4F RID: 19791
			Consumable,
			// Token: 0x04004D50 RID: 19792
			BuilderToolsMaterial
		}
	}
}
