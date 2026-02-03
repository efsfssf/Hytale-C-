using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Hypixel.ProtoPlus;
using HytaleClient.Application;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Data.Items;
using HytaleClient.Data.Map;
using HytaleClient.Data.UserSettings;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Programs;
using HytaleClient.InGame.Commands;
using HytaleClient.InGame.Modules;
using HytaleClient.InGame.Modules.AmbienceFX;
using HytaleClient.InGame.Modules.Audio;
using HytaleClient.InGame.Modules.BuilderTools;
using HytaleClient.InGame.Modules.Camera;
using HytaleClient.InGame.Modules.Camera.Controllers;
using HytaleClient.InGame.Modules.CharacterController;
using HytaleClient.InGame.Modules.Collision;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.ImmersiveScreen;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.InGame.Modules.InterfaceRenderPreview;
using HytaleClient.InGame.Modules.Machinima;
using HytaleClient.InGame.Modules.Map;
using HytaleClient.InGame.Modules.Particles;
using HytaleClient.InGame.Modules.Shortcuts;
using HytaleClient.InGame.Modules.Trails;
using HytaleClient.InGame.Modules.WorldMap;
using HytaleClient.Interface;
using HytaleClient.Interface.InGame.Hud.Abilities;
using HytaleClient.Math;
using HytaleClient.Networking;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using NLog;
using SDL2;

namespace HytaleClient.InGame
{
	// Token: 0x020008E4 RID: 2276
	internal class GameInstance : Disposable
	{
		// Token: 0x170010CF RID: 4303
		// (get) Token: 0x06004277 RID: 17015 RVA: 0x000C929A File Offset: 0x000C749A
		// (set) Token: 0x06004278 RID: 17016 RVA: 0x000C92A2 File Offset: 0x000C74A2
		public int LocalPlayerNetworkId { get; private set; }

		// Token: 0x170010D0 RID: 4304
		// (get) Token: 0x06004279 RID: 17017 RVA: 0x000C92AB File Offset: 0x000C74AB
		// (set) Token: 0x0600427A RID: 17018 RVA: 0x000C92B3 File Offset: 0x000C74B3
		public PlayerEntity LocalPlayer { get; private set; }

		// Token: 0x170010D1 RID: 4305
		// (get) Token: 0x0600427B RID: 17019 RVA: 0x000C92BC File Offset: 0x000C74BC
		// (set) Token: 0x0600427C RID: 17020 RVA: 0x000C92C4 File Offset: 0x000C74C4
		public GameMode GameMode { get; private set; } = 0;

		// Token: 0x170010D2 RID: 4306
		// (get) Token: 0x0600427D RID: 17021 RVA: 0x000C92CD File Offset: 0x000C74CD
		// (set) Token: 0x0600427E RID: 17022 RVA: 0x000C92D5 File Offset: 0x000C74D5
		public float ActiveFieldOfView { get; private set; }

		// Token: 0x170010D3 RID: 4307
		// (get) Token: 0x0600427F RID: 17023 RVA: 0x000C92DE File Offset: 0x000C74DE
		public bool IsPlaying
		{
			get
			{
				return !base.Disposed && this._stage == GameInstance.GameInstanceStage.WorldJoined && this.LocalPlayer != null;
			}
		}

		// Token: 0x170010D4 RID: 4308
		// (get) Token: 0x06004280 RID: 17024 RVA: 0x000C92FD File Offset: 0x000C74FD
		// (set) Token: 0x06004281 RID: 17025 RVA: 0x000C9305 File Offset: 0x000C7505
		public ServerSettings ServerSettings { get; private set; }

		// Token: 0x170010D5 RID: 4309
		// (get) Token: 0x06004282 RID: 17026 RVA: 0x000C930E File Offset: 0x000C750E
		// (set) Token: 0x06004283 RID: 17027 RVA: 0x000C9316 File Offset: 0x000C7516
		public bool IsReadyToDraw { get; private set; }

		// Token: 0x06004284 RID: 17028 RVA: 0x000C9320 File Offset: 0x000C7520
		public GameInstance(App app, ConnectionToServer establishedConnection)
		{
			this.App = app;
			this.Engine = app.Engine;
			this.Input = new Input(this.Engine, app.Settings.InputBindings);
			this.Chat = new Chat(this);
			this.Notifications = new Notifications(this);
			this.HitDetection = new HitDetection(this);
			this.InitModules();
			this.InitDraw();
			this.Connection = establishedConnection;
			this._packetHandler = new PacketHandler(this);
			this.Connection.OnPacketReceived = new Action<byte[], int>(this._packetHandler.Receive);
			ConnectionMode connectionMode = (this.Connection.Referral != null) ? 4 : 2;
			bool isInsecure = this.App.AuthManager.Settings.IsInsecure;
			if (isInsecure)
			{
				connectionMode = 3;
			}
			this.Connection.SendPacketImmediate(new Connect("f4c63561b2d2f5120b4c81ad1b8544e396088277d88f650aea892b6f0cb113f", 1643968234458L, connectionMode, this.App.Settings.Language ?? Language.SystemLanguage));
			bool flag = connectionMode == 3;
			if (flag)
			{
				this.Connection.SendPacket(new SetUsername(this.App.Username));
			}
			else
			{
				bool flag2 = this.App.AuthManager.CertPathBytes == null;
				if (flag2)
				{
					throw new Exception("Attempted to execute an online-mode handshake while not authenticated!");
				}
				Auth1 packet = new Auth1((sbyte[])this.App.AuthManager.CertPathBytes);
				this.Connection.SendPacket(packet);
			}
		}

		// Token: 0x06004285 RID: 17029 RVA: 0x000C96E8 File Offset: 0x000C78E8
		protected override void DoDispose()
		{
			this.DisposeDraw();
			this._packetHandler.Dispose();
			this.Connection.OnPacketReceived = null;
			AssetManager.UnloadServerRequiredAssets();
			foreach (Module module in this._modules)
			{
				module.Dispose();
			}
			this.AudioModule.Dispose();
			this.Engine.Audio.ResourceManager.FilePathsByFileName.Clear();
			this.Engine.Audio.RefreshBanks();
		}

		// Token: 0x170010D6 RID: 4310
		// (get) Token: 0x06004286 RID: 17030 RVA: 0x000C979C File Offset: 0x000C799C
		public bool IsOnPacketHandlerThread
		{
			get
			{
				return this._packetHandler.IsOnThread;
			}
		}

		// Token: 0x06004287 RID: 17031 RVA: 0x000C97A9 File Offset: 0x000C79A9
		public void InjectPacket(ProtoPacket packet)
		{
			this._packetHandler.Receive(packet);
		}

		// Token: 0x06004288 RID: 17032 RVA: 0x000C97BC File Offset: 0x000C79BC
		public int AddPendingCallback<T>(Disposable disposable, Action<FailureReply, T> callback) where T : ProtoPacket
		{
			return this._packetHandler.AddPendingCallback<T>(disposable, callback);
		}

		// Token: 0x06004289 RID: 17033 RVA: 0x000C97DB File Offset: 0x000C79DB
		public void SetServerSettings(ServerSettings newSettings)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this.ServerSettings = newSettings;
		}

		// Token: 0x0600428A RID: 17034 RVA: 0x000C97F4 File Offset: 0x000C79F4
		public void OnSetupComplete()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			GameInstance.Logger.Info("GameInstance.OnSetupComplete()");
			this.App.MainMenu.StopMusic();
			this.AudioModule.Initialize();
			foreach (Module module in this._modules)
			{
				module.Initialize();
			}
			this.App.ResetElapsedTime();
			this._stage = GameInstance.GameInstanceStage.WaitingForJoinWorldPacket;
			this.App.Interface.FadeOut(null);
		}

		// Token: 0x0600428B RID: 17035 RVA: 0x000C98A8 File Offset: 0x000C7AA8
		public void PrepareJoiningWorld(JoinWorld packet)
		{
			GameInstance.Logger.Info("GameInstance.PrepareJoiningWorld()");
			bool isAwaitingJoiningWorldFade = this._isAwaitingJoiningWorldFade;
			if (isAwaitingJoiningWorldFade)
			{
				GameInstance.Logger.Info("JoingingWorld OnFadeComplete canceled.");
				this.App.Interface.CancelOnFadeComplete();
				this._isAwaitingJoiningWorldFade = false;
			}
			bool fadeInOut = packet.FadeInOut;
			if (fadeInOut)
			{
				this._isAwaitingJoiningWorldFade = true;
				this.App.Interface.FadeOut(delegate
				{
					this.StartJoiningWorld(packet);
					this._isAwaitingJoiningWorldFade = false;
				});
			}
			else
			{
				this.StartJoiningWorld(packet);
			}
		}

		// Token: 0x0600428C RID: 17036 RVA: 0x000C9954 File Offset: 0x000C7B54
		private void StartJoiningWorld(JoinWorld packet)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			GameInstance.Logger.Info("GameInstance.StartJoiningWorld()");
			bool clearWorld = packet.ClearWorld;
			if (clearWorld)
			{
				this.MapModule.ClearAllColumns();
				this.EntityStoreModule.DespawnAll();
				this.SetLocalPlayer(null);
			}
			this.Connection.SendPacket(new ClientReady(true, false));
			this._stage = GameInstance.GameInstanceStage.WaitingForNearbyChunks;
			this._stopwatchSinceJoiningServer.Restart();
			this.Input.ResetKeys();
			this.Input.ResetMouseButtons();
		}

		// Token: 0x0600428D RID: 17037 RVA: 0x000C99E8 File Offset: 0x000C7BE8
		public void SetGameMode(GameMode gameMode, bool executeCommand = false)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this.GameMode = gameMode;
			this.App.Interface.InGameView.OnGameModeChanged();
			bool flag = this.GameMode == 0;
			if (flag)
			{
				this.CharacterControllerModule.MovementController.MovementStates.IsFlying = false;
				this.BuilderToolsModule.PlaySelection.CancelAllActions(null);
			}
			if (executeCommand)
			{
				bool flag2 = gameMode == 1;
				if (flag2)
				{
					this.Chat.SendCommand("gm c", Array.Empty<object>());
				}
				else
				{
					this.Chat.SendCommand("gm a", Array.Empty<object>());
				}
			}
		}

		// Token: 0x0600428E RID: 17038 RVA: 0x000C9A93 File Offset: 0x000C7C93
		public void SetLocalPlayerId(int localPlayerId)
		{
			this.LocalPlayerNetworkId = localPlayerId;
		}

		// Token: 0x0600428F RID: 17039 RVA: 0x000C9A9E File Offset: 0x000C7C9E
		public void SetLocalPlayer(PlayerEntity entity)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this.LocalPlayer = entity;
		}

		// Token: 0x06004290 RID: 17040 RVA: 0x000C9AB4 File Offset: 0x000C7CB4
		private void OnWorldJoined()
		{
			Debug.Assert(this._stage == GameInstance.GameInstanceStage.WaitingForNearbyChunks, string.Format("Called OnWorldJoined but stage was {0}", this._stage));
			GameInstance.Logger.Info("GameInstance.OnWorldJoined()");
			this._stage = GameInstance.GameInstanceStage.WorldJoined;
			bool flag = this.App.Stage == App.AppStage.GameLoading;
			if (flag)
			{
				this.App.GameLoading.AssertStage(AppGameLoading.GameLoadingStage.Loading);
				this.App.GameLoading.SetStage(AppGameLoading.GameLoadingStage.Complete);
				this.App.InGame.Open();
			}
			this.Connection.SendPacket(new ClientReady(true, true));
			this.App.Interface.FadeIn(delegate
			{
				this.CharacterControllerModule.MovementController.MovementEnabled = true;
			}, true);
			this.Chat.HandleBeforePlayingMessages();
			ServerCameraController serverCameraController = this.CameraModule.Controller as ServerCameraController;
			bool flag2 = serverCameraController != null;
			if (flag2)
			{
				serverCameraController.OnWorldJoined();
			}
			bool flag3 = this.App.SingleplayerServer != null;
			if (flag3)
			{
				AffinityHelper.SetupSingleplayerAffinity(this.App.SingleplayerServer.Process);
			}
			this.Connection.SendPacket(this.App.Settings.DebugSettings.CreatePacket());
		}

		// Token: 0x06004291 RID: 17041 RVA: 0x000C9BEC File Offset: 0x000C7DEC
		public void DisconnectWithReason(string reason, Exception exception)
		{
			GameInstance.Logger.Info<App.AppStage, string>("Disconnecting with error during stage {0}: {1}", this.App.Stage, reason);
			GameInstance.Logger.Info<Exception>(exception);
			bool diagnosticMode = this.App.Settings.DiagnosticMode;
			if (diagnosticMode)
			{
				reason = exception.Message;
			}
			this.Connection.SendPacketImmediate(new Disconnect(reason, 1));
			this.Connection.Close();
			this.App.Disconnection.SetReason(reason);
			this.App.Disconnection.Open(exception.Message, this.Connection.Hostname, this.Connection.Port);
		}

		// Token: 0x06004292 RID: 17042 RVA: 0x000C9C9C File Offset: 0x000C7E9C
		private void ManageReticleEvents()
		{
			InputBindings inputBindings = this.App.Settings.InputBindings;
			bool flag = this.Input.IsBindingHeld(inputBindings.StrafeLeft, false) && !this.Input.IsBindingHeld(inputBindings.StrafeRight, false);
			if (flag)
			{
				this.App.Interface.InGameView.ReticleComponent.OnClientEvent(2, null);
			}
			else
			{
				bool flag2 = this.Input.IsBindingHeld(inputBindings.StrafeRight, false) && !this.Input.IsBindingHeld(inputBindings.StrafeLeft, false);
				if (flag2)
				{
					this.App.Interface.InGameView.ReticleComponent.OnClientEvent(3, null);
				}
				else
				{
					this.App.Interface.InGameView.ReticleComponent.RemoveClientReticle(2);
					this.App.Interface.InGameView.ReticleComponent.RemoveClientReticle(3);
				}
			}
		}

		// Token: 0x06004293 RID: 17043 RVA: 0x000C9D98 File Offset: 0x000C7F98
		public void OnUserInput(SDL.SDL_Event evt)
		{
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(GameInstance).FullName);
			}
			InputBindings inputBindings = this.App.Settings.InputBindings;
			bool mapNeedsDrawing = this.WorldMapModule.MapNeedsDrawing;
			if (mapNeedsDrawing)
			{
				this.WorldMapModule.OnUserInput(evt);
			}
			bool flag = !this.App.Engine.Window.IsMouseLocked;
			if (flag)
			{
				this.InterfaceRenderPreviewModule.OnUserInput(evt);
			}
			bool flag2 = evt.type == SDL.SDL_EventType.SDL_MOUSEMOTION;
			if (flag2)
			{
				bool displayCursor = this.CameraModule.Controller.DisplayCursor;
				if (displayCursor)
				{
					this.CameraModule.Controller.OnMouseInput(evt);
				}
				else
				{
					bool flag3 = this.Engine.Window.IsMouseLocked && !this.Input.MouseInputDisabled;
					if (flag3)
					{
						this.CameraModule.OffsetLook((float)(-(float)evt.motion.yrel), (float)(-(float)evt.motion.xrel));
					}
				}
			}
			else
			{
				SDL.SDL_EventType type = evt.type;
				SDL.SDL_EventType sdl_EventType = type;
				switch (sdl_EventType)
				{
				case SDL.SDL_EventType.SDL_KEYDOWN:
				{
					this.Input.OnUserInput(evt);
					this.ManageReticleEvents();
					bool flag4 = evt.key.repeat == 0 && !this.Engine.Window.IsMouseLocked && Input.EventMatchesBinding(evt, inputBindings.DropItem);
					if (flag4)
					{
						this.App.Interface.TriggerEvent("inventory.dropItemBindingDown", null, null, null, null, null, null);
					}
					goto IL_337;
				}
				case SDL.SDL_EventType.SDL_KEYUP:
				{
					this.Input.OnUserInput(evt);
					this.ManageReticleEvents();
					bool flag5 = !this.Engine.Window.IsMouseLocked && Input.EventMatchesBinding(evt, inputBindings.DropItem);
					if (flag5)
					{
						this.App.Interface.TriggerEvent("inventory.dropItemBindingUp", null, null, null, null, null, null);
					}
					goto IL_337;
				}
				case SDL.SDL_EventType.SDL_TEXTEDITING:
					break;
				case SDL.SDL_EventType.SDL_TEXTINPUT:
					goto IL_337;
				default:
					switch (sdl_EventType)
					{
					case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
					{
						bool displayCursor2 = this.CameraModule.Controller.DisplayCursor;
						if (displayCursor2)
						{
							this.CameraModule.Controller.OnMouseInput(evt);
						}
						this.Input.OnUserInput(evt);
						goto IL_337;
					}
					case SDL.SDL_EventType.SDL_MOUSEBUTTONUP:
					{
						bool displayCursor3 = this.CameraModule.Controller.DisplayCursor;
						if (displayCursor3)
						{
							this.CameraModule.Controller.OnMouseInput(evt);
						}
						this.Input.OnUserInput(evt);
						goto IL_337;
					}
					case SDL.SDL_EventType.SDL_MOUSEWHEEL:
					{
						bool shouldForwardMouseWheelEvents = this.InteractionModule.ShouldForwardMouseWheelEvents;
						if (shouldForwardMouseWheelEvents)
						{
							this.InteractionModule.OnMouseWheelEvent(evt);
							goto IL_337;
						}
						bool displayCursor4 = this.CameraModule.Controller.DisplayCursor;
						if (displayCursor4)
						{
							this.CameraModule.Controller.OnMouseInput(evt);
						}
						bool flag6 = this.Engine.Window.IsMouseLocked && evt.wheel.y != 0;
						if (flag6)
						{
							this.InventoryModule.ScrollHotbarSlot(evt.wheel.y < 0);
						}
						goto IL_337;
					}
					}
					break;
				}
				throw new ArgumentOutOfRangeException("evt", evt.type.ToString());
				IL_337:
				bool flag7 = this.DrawOcclusionMap || this.DebugDrawLight || this.DebugMap;
				if (flag7)
				{
					bool flag8 = this.Input.ConsumeKey(SDL.SDL_Scancode.SDL_SCANCODE_UP, false);
					if (flag8)
					{
						bool flag9 = this._debugDrawMapOpacityStep < 5;
						if (flag9)
						{
							this._debugDrawMapOpacityStep++;
							this.Chat.Log("Debug Map Transparency : " + this._debugDrawMapOpacityStep.ToString() + " / " + 5.ToString());
						}
					}
					bool flag10 = this.Input.ConsumeKey(SDL.SDL_Scancode.SDL_SCANCODE_DOWN, false);
					if (flag10)
					{
						bool flag11 = this._debugDrawMapOpacityStep > 0;
						if (flag11)
						{
							this._debugDrawMapOpacityStep--;
							this.Chat.Log("Debug Map Transparency : " + this._debugDrawMapOpacityStep.ToString() + " / " + 5.ToString());
						}
					}
					bool flag12 = this.Input.ConsumeKey(SDL.SDL_Scancode.SDL_SCANCODE_RIGHT, false);
					if (flag12)
					{
						bool flag13 = this._debugTextureArrayActiveLayer < this._debugTextureArrayLayerCount - 1;
						if (flag13)
						{
							this._debugTextureArrayActiveLayer++;
							int num = this._debugTextureArrayActiveLayer + 1;
							this.Chat.Log("Debug Texture Array Layer : " + num.ToString() + " / " + this._debugTextureArrayLayerCount.ToString());
						}
					}
					bool flag14 = this.Input.ConsumeKey(SDL.SDL_Scancode.SDL_SCANCODE_LEFT, false);
					if (flag14)
					{
						bool flag15 = this._debugTextureArrayActiveLayer > 0;
						if (flag15)
						{
							this._debugTextureArrayActiveLayer--;
							int num2 = this._debugTextureArrayActiveLayer + 1;
							this.Chat.Log("Debug Texture Array Layer : " + num2.ToString() + " / " + this._debugTextureArrayLayerCount.ToString());
						}
					}
					bool flag16 = this.Input.ConsumeKey(SDL.SDL_Scancode.SDL_SCANCODE_KP_PLUS, false);
					if (flag16)
					{
						bool flag17 = this._debugDrawMapLevel < 8;
						if (flag17)
						{
							this._debugDrawMapLevel++;
							this.Chat.Log("Draw Map Level : " + this._debugDrawMapLevel.ToString() + " / " + 8.ToString());
						}
					}
					bool flag18 = this.Input.ConsumeKey(SDL.SDL_Scancode.SDL_SCANCODE_KP_MINUS, false);
					if (flag18)
					{
						bool flag19 = this._debugDrawMapLevel > 0;
						if (flag19)
						{
							this._debugDrawMapLevel--;
							this.Chat.Log("Draw Map Level : " + this._debugDrawMapLevel.ToString() + " / " + 8.ToString());
						}
					}
				}
				bool flag20 = this.Input.ConsumeKey(SDL.SDL_Scancode.SDL_SCANCODE_F10, false);
				if (flag20)
				{
					this.Wireframe = ((this.Wireframe != GameInstance.WireframePass.OnAll) ? GameInstance.WireframePass.OnAll : GameInstance.WireframePass.Off);
					this.Chat.Log("Wireframe: " + this.Wireframe.ToString());
				}
				bool flag21 = this.Input.ConsumeKey(SDL.SDL_Scancode.SDL_SCANCODE_F6, false);
				if (flag21)
				{
					this.ReloadShaderTextures();
					this.Chat.Log("Shader Textures have been reloaded.");
				}
				bool flag22 = evt.type == SDL.SDL_EventType.SDL_KEYDOWN && evt.key.repeat == 0;
				if (flag22)
				{
					SDL.SDL_Keycode sym = evt.key.keysym.sym;
					SDL.SDL_Keycode sdl_Keycode = sym;
					SDL.SDL_Keycode? keycode = inputBindings.Chat.Keycode;
					bool flag23 = (sdl_Keycode == keycode.GetValueOrDefault() & keycode != null) && !this.Chat.IsOpen;
					if (flag23)
					{
						this.Chat.TryOpen(inputBindings.Chat.Keycode, false);
					}
					else
					{
						SDL.SDL_Keycode sdl_Keycode2 = sym;
						keycode = inputBindings.Command.Keycode;
						bool flag24 = (sdl_Keycode2 == keycode.GetValueOrDefault() & keycode != null) && !this.Chat.IsOpen;
						if (flag24)
						{
							this.Chat.TryOpen(inputBindings.Command.Keycode, true);
						}
						else
						{
							SDL.SDL_Keycode sdl_Keycode3 = sym;
							keycode = inputBindings.SwitchHudVisibility.Keycode;
							bool flag25 = (sdl_Keycode3 == keycode.GetValueOrDefault() & keycode != null) && !this.App.Interface.Desktop.IsShortcutKeyDown;
							if (flag25)
							{
								this.App.InGame.SwitchHudVisibility();
							}
						}
					}
					bool flag26 = this.App.InGame.CurrentOverlay == AppInGame.InGameOverlay.None;
					if (flag26)
					{
						bool flag27 = !this.App.InGame.HasUnclosablePage && !this.Chat.IsOpen && this.App.Interface.Desktop.FocusedElement == null;
						if (flag27)
						{
							SDL.SDL_Keycode sdl_Keycode4 = sym;
							keycode = inputBindings.OpenToolsSettings.Keycode;
							bool flag28 = (sdl_Keycode4 == keycode.GetValueOrDefault() & keycode != null) && this.App.InGame.Instance.GameMode == 1;
							if (flag28)
							{
								bool isToolsSettingsModalOpened = this.App.InGame.IsToolsSettingsModalOpened;
								if (isToolsSettingsModalOpened)
								{
									this.App.InGame.CloseToolsSettingsModal();
								}
								else
								{
									this.App.InGame.OpenToolsSettingsPage();
								}
							}
							else
							{
								SDL.SDL_Keycode sdl_Keycode5 = sym;
								keycode = inputBindings.ToolPaintBrush.Keycode;
								bool flag29 = (sdl_Keycode5 == keycode.GetValueOrDefault() & keycode != null) && this.App.InGame.Instance.GameMode == 1;
								if (flag29)
								{
									this.App.InGame.ToogleToolById("EditorTool_PlayPaint");
								}
								else
								{
									SDL.SDL_Keycode sdl_Keycode6 = sym;
									keycode = inputBindings.ToolSculptBrush.Keycode;
									bool flag30 = (sdl_Keycode6 == keycode.GetValueOrDefault() & keycode != null) && this.App.InGame.Instance.GameMode == 1;
									if (flag30)
									{
										this.App.InGame.ToogleToolById("EditorTool_PlaySculpt");
									}
									else
									{
										SDL.SDL_Keycode sdl_Keycode7 = sym;
										keycode = inputBindings.ToolSelectionTool.Keycode;
										bool flag31 = (sdl_Keycode7 == keycode.GetValueOrDefault() & keycode != null) && this.App.InGame.Instance.GameMode == 1;
										if (flag31)
										{
											this.App.InGame.ToogleToolById("EditorTool_PlaySelection");
										}
										else
										{
											SDL.SDL_Keycode sdl_Keycode8 = sym;
											keycode = inputBindings.ToolLine.Keycode;
											bool flag32 = (sdl_Keycode8 == keycode.GetValueOrDefault() & keycode != null) && this.App.InGame.Instance.GameMode == 1;
											if (flag32)
											{
												this.App.InGame.ToogleToolById("EditorTool_Line");
											}
											else
											{
												SDL.SDL_Keycode sdl_Keycode9 = sym;
												keycode = inputBindings.ToolPaste.Keycode;
												bool flag33 = (sdl_Keycode9 == keycode.GetValueOrDefault() & keycode != null) && this.App.InGame.Instance.GameMode == 1;
												if (flag33)
												{
													this.App.InGame.ToogleToolById("EditorTool_Paste");
												}
												else
												{
													SDL.SDL_Keycode sdl_Keycode10 = sym;
													keycode = inputBindings.OpenInventory.Keycode;
													bool flag34 = sdl_Keycode10 == keycode.GetValueOrDefault() & keycode != null;
													if (flag34)
													{
														bool flag35 = this.App.InGame.CurrentPage == 2;
														if (flag35)
														{
															this.App.Interface.InGameView.TryClosePageOrOverlayWithInputBinding();
														}
														else
														{
															this.App.InGame.SetCurrentPage(2, false, true);
															bool flag36 = this.App.InGame.Instance.InventoryModule.UsingToolsItem() && !this.App.InGame.IsToolsSettingsModalOpened;
															if (flag36)
															{
																this.App.InGame.Instance.BuilderToolsModule.ClearConfiguringTool();
															}
														}
													}
													else
													{
														SDL.SDL_Keycode sdl_Keycode11 = sym;
														keycode = inputBindings.OpenMap.Keycode;
														bool flag37 = sdl_Keycode11 == keycode.GetValueOrDefault() & keycode != null;
														if (flag37)
														{
															bool flag38 = this.App.InGame.CurrentPage == 6;
															if (flag38)
															{
																this.App.Interface.InGameView.TryClosePageOrOverlayWithInputBinding();
															}
															else
															{
																bool isWorldMapEnabled = this.WorldMapModule.IsWorldMapEnabled;
																if (isWorldMapEnabled)
																{
																	this.App.InGame.SetCurrentPage(6, false, true);
																}
																else
																{
																	this.Chat.Log("ui.map.disabled");
																}
															}
														}
														else
														{
															bool flag39 = this.App.InGame.CurrentPage == 0;
															if (flag39)
															{
																SDL.SDL_Keycode sdl_Keycode12 = sym;
																keycode = inputBindings.OpenAssetEditor.Keycode;
																bool flag40 = sdl_Keycode12 == keycode.GetValueOrDefault() & keycode != null;
																if (flag40)
																{
																	bool flag41 = this.Input.IsShiftHeld() && this.InteractionModule.HasFoundTargetBlock;
																	bool flag42 = flag41;
																	if (flag42)
																	{
																		this.App.InGame.OpenAssetIdInAssetEditor("Item", ClientBlockType.GetOriginalBlockName(this.MapModule.ClientBlockTypes[this.InteractionModule.TargetBlockHit.BlockId].Name));
																	}
																	else
																	{
																		this.App.InGame.OpenAssetEditor();
																	}
																}
																else
																{
																	SDL.SDL_Keycode sdl_Keycode13 = sym;
																	keycode = inputBindings.OpenMachinimaEditor.Keycode;
																	bool flag43 = sdl_Keycode13 == keycode.GetValueOrDefault() & keycode != null;
																	if (flag43)
																	{
																		this.MachinimaModule.ShowInterface();
																	}
																	else
																	{
																		SDL.SDL_Keycode sdl_Keycode14 = sym;
																		keycode = inputBindings.ShowPlayerList.Keycode;
																		bool flag44 = sdl_Keycode14 == keycode.GetValueOrDefault() & keycode != null;
																		if (flag44)
																		{
																			this.App.InGame.SetPlayerListVisible(true);
																		}
																		else
																		{
																			SDL.SDL_Keycode sdl_Keycode15 = sym;
																			keycode = inputBindings.ShowUtilitySlotSelector.Keycode;
																			bool flag45 = (sdl_Keycode15 == keycode.GetValueOrDefault() & keycode != null) && !this.BuilderToolsModule.HasActiveBrush;
																			if (flag45)
																			{
																				this.App.InGame.SetActiveItemSelector(AppInGame.ItemSelector.Utility);
																			}
																			else
																			{
																				SDL.SDL_Keycode sdl_Keycode16 = sym;
																				keycode = inputBindings.ShowConsumableSlotSelector.Keycode;
																				bool flag46 = sdl_Keycode16 == keycode.GetValueOrDefault() & keycode != null;
																				if (flag46)
																				{
																					this._consumableDownTime.Restart();
																				}
																				else
																				{
																					bool flag47 = this.GameMode == 1 && this.BuilderToolsModule.HasActiveBrush;
																					if (flag47)
																					{
																						SDL.SDL_Keycode sdl_Keycode17 = sym;
																						keycode = inputBindings.TertiaryItemAction.Keycode;
																						bool flag48 = sdl_Keycode17 == keycode.GetValueOrDefault() & keycode != null;
																						if (flag48)
																						{
																							this.App.InGame.SetActiveItemSelector(AppInGame.ItemSelector.BuilderToolsMaterial);
																						}
																						else
																						{
																							SDL.SDL_Keycode sdl_Keycode18 = sym;
																							keycode = inputBindings.ToggleBuilderToolsLegend.Keycode;
																							bool flag49 = sdl_Keycode18 == keycode.GetValueOrDefault() & keycode != null;
																							if (flag49)
																							{
																								this.App.Settings.BuilderToolsSettings.DisplayLegend = !this.App.Settings.BuilderToolsSettings.DisplayLegend;
																								this.App.Settings.Save();
																								this.App.Interface.InGameView.UpdateBuilderToolsLegendVisibility(true);
																							}
																						}
																					}
																					else
																					{
																						SDL.SDL_Keycode sdl_Keycode19 = sym;
																						keycode = inputBindings.TertiaryItemAction.Keycode;
																						bool flag50 = sdl_Keycode19 == keycode.GetValueOrDefault() & keycode != null;
																						if (flag50)
																						{
																							AbilitiesHudComponent abilitiesHudComponent = this.App.Interface.InGameView.AbilitiesHudComponent;
																							if (abilitiesHudComponent != null)
																							{
																								abilitiesHudComponent.OnTertiaryAction();
																							}
																						}
																					}
																				}
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
					else
					{
						bool flag51;
						if (this.App.InGame.CurrentOverlay == AppInGame.InGameOverlay.MachinimaEditor)
						{
							SDL.SDL_Keycode sdl_Keycode20 = sym;
							keycode = inputBindings.OpenMachinimaEditor.Keycode;
							flag51 = (sdl_Keycode20 == keycode.GetValueOrDefault() & keycode != null);
						}
						else
						{
							flag51 = false;
						}
						bool flag52 = flag51;
						if (flag52)
						{
							this.EditorWebViewModule.WebView.TriggerEvent("requestCloseInGameOverlayWithInputBinding", EditorWebViewModule.WebViewType.MachinimaEditor, null, null, null, null);
						}
					}
				}
				else
				{
					bool flag53 = evt.type == SDL.SDL_EventType.SDL_KEYUP;
					if (flag53)
					{
						bool flag54 = this.App.InGame.CurrentOverlay == AppInGame.InGameOverlay.None && this.App.InGame.CurrentPage == 0;
						if (flag54)
						{
							SDL.SDL_Keycode sym2 = evt.key.keysym.sym;
							SDL.SDL_Keycode sdl_Keycode21 = sym2;
							SDL.SDL_Keycode? keycode = inputBindings.ShowPlayerList.Keycode;
							bool flag55 = sdl_Keycode21 == keycode.GetValueOrDefault() & keycode != null;
							if (flag55)
							{
								this.App.InGame.SetPlayerListVisible(false);
							}
							else
							{
								SDL.SDL_Keycode sdl_Keycode22 = sym2;
								keycode = inputBindings.ShowUtilitySlotSelector.Keycode;
								bool flag56 = sdl_Keycode22 == keycode.GetValueOrDefault() & keycode != null;
								if (flag56)
								{
									this.App.InGame.SetActiveItemSelector(AppInGame.ItemSelector.None);
								}
								else
								{
									SDL.SDL_Keycode sdl_Keycode23 = sym2;
									keycode = inputBindings.ShowConsumableSlotSelector.Keycode;
									bool flag57 = sdl_Keycode23 == keycode.GetValueOrDefault() & keycode != null;
									if (flag57)
									{
										bool flag58 = this.App.InGame.ActiveItemSelector == AppInGame.ItemSelector.Consumable;
										if (flag58)
										{
											this.App.InGame.SetActiveItemSelector(AppInGame.ItemSelector.None);
										}
										else
										{
											bool flag59 = this._consumableDownTime.IsRunning && this._consumableDownTime.ElapsedMilliseconds < 200L;
											if (flag59)
											{
												InventoryModule inventoryModule = this.App.InGame.Instance.InventoryModule;
												inventoryModule.SetActiveConsumableSlot(inventoryModule.ConsumableActiveSlot, true, true);
											}
										}
										this._consumableDownTime.Reset();
									}
									else
									{
										SDL.SDL_Keycode sdl_Keycode24 = sym2;
										keycode = inputBindings.TertiaryItemAction.Keycode;
										bool flag60 = sdl_Keycode24 == keycode.GetValueOrDefault() & keycode != null;
										if (flag60)
										{
											this.App.InGame.SetActiveItemSelector(AppInGame.ItemSelector.None);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06004294 RID: 17044 RVA: 0x000CAE7A File Offset: 0x000C907A
		private void TickAllModules()
		{
			this.TimeModule.Tick();
			this.CharacterControllerModule.Tick();
			this._networkModule.Tick();
			this.MachinimaModule.Tick();
			this.ProfilingModule.Tick();
		}

		// Token: 0x06004295 RID: 17045 RVA: 0x000CAEBC File Offset: 0x000C90BC
		private void PreUpdateModules(float timeFraction)
		{
			this.CharacterControllerModule.PreUpdate(timeFraction);
			this.TimeModule.OnNewFrame(this.DeltaTime);
			this.CameraModule.Update(this.DeltaTime);
			this.InventoryModule.Update(this.DeltaTime);
			this.EntityStoreModule.PreUpdate(this.DeltaTime);
			this.Engine.Profiling.StartMeasure(8);
			this.EntityStoreModule.PrepareLights(this.CameraModule.Controller.Position);
			this.Engine.Profiling.StopMeasure(8);
			this.Engine.Profiling.StartMeasure(7);
			this.InteractionModule.Update(this.DeltaTime);
			this.Engine.Profiling.StopMeasure(7);
			this.ParticleSystemStoreModule.PreUpdate(this.CameraModule.Controller.Position);
		}

		// Token: 0x06004296 RID: 17046 RVA: 0x000CAFB4 File Offset: 0x000C91B4
		private void UpdateModules()
		{
			this.Engine.Profiling.StartMeasure(20);
			this.MapModule.Update(this.DeltaTime);
			this.Engine.Profiling.StopMeasure(20);
			this.Engine.Profiling.StartMeasure(23);
			this.EntityStoreModule.Update(this.DeltaTime);
			this.Engine.Profiling.StopMeasure(23);
			this.AudioModule.Update(this.DeltaTime);
			this.MachinimaModule.OnNewFrame(this.DeltaTime);
			this.Engine.Profiling.StartMeasure(21);
			this.AmbienceFXModule.Update(this.DeltaTime);
			this.Engine.Profiling.StopMeasure(21);
			this.BuilderToolsModule.Update(this.DeltaTime);
			this.Engine.Profiling.StartMeasure(22);
			this.WeatherModule.Update(this.DeltaTime);
			this.Engine.Profiling.StopMeasure(22);
			this.ScreenEffectStoreModule.Update(this.DeltaTime);
			this.DamageEffectModule.Update(this.DeltaTime);
			this.ProfilingModule.OnNewFrame(this.DeltaTime);
			this.ImmersiveScreenModule.Update(this.DeltaTime);
			this.ShortcutsModule.Update();
			this.InterfaceRenderPreviewModule.Update(this.DeltaTime);
			this.WorldMapModule.Update(this.DeltaTime);
			this.CharacterControllerModule.Update(this.DeltaTime);
			this._movementSoundModule.Update(this.DeltaTime);
		}

		// Token: 0x06004297 RID: 17047 RVA: 0x000CB174 File Offset: 0x000C9374
		private void BeginFrame()
		{
			this.Engine.Profiling.RegisterExternalMeasure(1, (float)this.Engine.TimeSpentInQueuedActions, 0f);
			this.Engine.AnimationSystem.BeginFrame();
			this.Engine.FXSystem.BeginFrame();
			this.Engine.Graphics.RTStore.BeginFrame();
			this.SceneRenderer.BeginFrame();
			this.MapModule.BeginFrame();
			this.EntityStoreModule.BeginFrame();
			this._cameraSceneView.ResetCounters();
			this._sunSceneView.ResetCounters();
		}

		// Token: 0x06004298 RID: 17048 RVA: 0x000CB21C File Offset: 0x000C941C
		public void OnNewFrame(float deltaTime, bool needsDrawing)
		{
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(GameInstance).FullName);
			}
			this.IsReadyToDraw = false;
			uint frameCounter = this.FrameCounter;
			this.FrameCounter = frameCounter + 1U;
			this.FrameTime = (float)this._stopwatchSinceJoiningServer.ElapsedMilliseconds * 0.001f;
			this.DeltaTime = deltaTime;
			bool flag = this._consumableDownTime.ElapsedMilliseconds >= 200L;
			if (flag)
			{
				this._consumableDownTime.Reset();
				bool flag2 = this.App.InGame.CurrentPage == null && this.App.InGame.CurrentOverlay == AppInGame.InGameOverlay.None && !this.Chat.IsOpen;
				if (flag2)
				{
					this.App.InGame.SetActiveItemSelector(AppInGame.ItemSelector.Consumable);
				}
			}
			this.BeginFrame();
			bool flag3 = !this.IsPlaying;
			if (flag3)
			{
				bool flag4 = this._stage == GameInstance.GameInstanceStage.WaitingForNearbyChunks && this.LocalPlayer != null;
				if (flag4)
				{
					this.MapModule.PrepareChunks(this.FrameTime);
					bool flag5 = this.App.Interface.FadeState == BaseInterface.InterfaceFadeState.FadedOut && (this.MapModule.AreNearbyChunksRendered || this._stopwatchSinceJoiningServer.ElapsedMilliseconds > 3000L);
					if (flag5)
					{
						this.OnWorldJoined();
					}
				}
				bool flag6 = !this.IsPlaying;
				if (flag6)
				{
					return;
				}
			}
			this.Engine.Profiling.StartMeasure(3);
			this.MapModule.PrepareChunks(this.FrameTime);
			this.Engine.Profiling.StopMeasure(3);
			this.Engine.Profiling.StartMeasure(4);
			this.EntityStoreModule.PrepareEntities();
			this.Engine.Profiling.StopMeasure(4);
			this.Engine.Profiling.StartMeasure(5);
			this._tickAccumulator += deltaTime;
			bool flag7 = this._tickAccumulator > 0.083333336f;
			if (flag7)
			{
				this._tickAccumulator = 0.083333336f;
			}
			while (this._tickAccumulator >= 0.016666668f)
			{
				this.TickAllModules();
				this._tickAccumulator -= 0.016666668f;
			}
			float timeFraction = Math.Min(this._tickAccumulator / 0.016666668f, 1f);
			this.Engine.Profiling.StopMeasure(5);
			this.Engine.Profiling.StartMeasure(6);
			this.PreUpdateModules(timeFraction);
			this.Engine.Profiling.StopMeasure(6);
			if (needsDrawing)
			{
				this.UpdateRenderData();
				this.Engine.Profiling.StartMeasure(9);
				this.MapModule.ProcessFrustumCulling(this._cameraSceneView);
				this.MapModule.GatherRenderableChunks(this._cameraSceneView);
				this.Engine.Profiling.StopMeasure(9);
				this.Engine.Profiling.StartMeasure(10);
				this._cameraSceneView.SortChunksByDistance();
				this.MapModule.PrepareChunksForDraw(this._cameraSceneView);
				this.Engine.Profiling.StopMeasure(10);
				this.Engine.Profiling.StartMeasure(11);
				this.LocalPlayer.RegisterAnimationTasks();
				this.Engine.AnimationSystem.ProcessAnimationTasks();
				this.Engine.Profiling.StopMeasure(11);
				this.Engine.Profiling.StartMeasure(12);
				this.UpdateOcclusionCulling();
				this.Engine.Profiling.StopMeasure(12);
			}
			this.Engine.Profiling.StartMeasure(19);
			this.UpdateModules();
			this.Engine.Profiling.StopMeasure(19);
			this.Connection.TriggerSend();
			if (needsDrawing)
			{
				this.Engine.Profiling.StartMeasure(24);
				this.UpdateSceneData();
				this.UpdateInterfaceData();
				this.Engine.Profiling.StopMeasure(24);
				this.IsReadyToDraw = true;
				this.Chat.NotifyPlayerOfSkippedDiagnosticMessages();
			}
		}

		// Token: 0x06004299 RID: 17049 RVA: 0x000CB668 File Offset: 0x000C9868
		public bool IsBuilderModeEnabled()
		{
			return this.GameMode == 1 && this.App.Settings.BuilderMode;
		}

		// Token: 0x0600429A RID: 17050 RVA: 0x000CB698 File Offset: 0x000C9898
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void BeginWireframeMode(GameInstance.WireframePass pass)
		{
			bool flag = this.Wireframe == pass;
			if (flag)
			{
				this.Engine.Graphics.GL.PolygonMode(GL.FRONT_AND_BACK, GL.LINE);
			}
		}

		// Token: 0x0600429B RID: 17051 RVA: 0x000CB6D8 File Offset: 0x000C98D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void EndWireframeMode(GameInstance.WireframePass pass)
		{
			bool flag = this.Wireframe == pass;
			if (flag)
			{
				this.Engine.Graphics.GL.PolygonMode(GL.FRONT_AND_BACK, GL.FILL);
			}
		}

		// Token: 0x170010D7 RID: 4311
		// (get) Token: 0x0600429C RID: 17052 RVA: 0x000CB718 File Offset: 0x000C9918
		// (set) Token: 0x0600429D RID: 17053 RVA: 0x000CB720 File Offset: 0x000C9920
		public PostEffectRenderer PostEffectRenderer { get; private set; }

		// Token: 0x0600429E RID: 17054 RVA: 0x000CB729 File Offset: 0x000C9929
		private void InitLighting()
		{
			this.UseClusteredLightingCustomZDistribution(true);
			this.UseClusteredLightingDirectAccess(true);
			this.UseClusteredLightingRefinedVoxelization(true);
			this.UseClusteredLightingMappedGPUBuffers(true);
			this.UseClusteredLightingPBO(true);
			this.SetLightBufferCompression(true);
		}

		// Token: 0x0600429F RID: 17055 RVA: 0x000CB75C File Offset: 0x000C995C
		public void SetLightBufferCompression(bool enable)
		{
			this.SceneRenderer.SetLightBufferCompression(enable);
		}

		// Token: 0x060042A0 RID: 17056 RVA: 0x000CB76B File Offset: 0x000C996B
		public void UseClusteredLighting(bool enable)
		{
			this.SceneRenderer.UseClusteredLighting = enable;
		}

		// Token: 0x060042A1 RID: 17057 RVA: 0x000CB779 File Offset: 0x000C9979
		public void UseClusteredLightingRefinedVoxelization(bool enable)
		{
			this.SceneRenderer.ClusteredLighting.UseRefinedVoxelization(enable);
		}

		// Token: 0x060042A2 RID: 17058 RVA: 0x000CB78D File Offset: 0x000C998D
		public void UseClusteredLightingMappedGPUBuffers(bool enable)
		{
			this.SceneRenderer.ClusteredLighting.UseMappedGPUBuffers(enable);
		}

		// Token: 0x060042A3 RID: 17059 RVA: 0x000CB7A1 File Offset: 0x000C99A1
		public void UseClusteredLightingPBO(bool enable)
		{
			this.SceneRenderer.ClusteredLighting.UsePBO(enable);
		}

		// Token: 0x060042A4 RID: 17060 RVA: 0x000CB7B5 File Offset: 0x000C99B5
		public void UseClusteredLightingDoubleBuffering(bool enable)
		{
			this.SceneRenderer.ClusteredLighting.UseDoubleBuffering(enable);
		}

		// Token: 0x060042A5 RID: 17061 RVA: 0x000CB7CC File Offset: 0x000C99CC
		public void SetClusteredLightingGridResolution(uint width, uint height, uint depth)
		{
			this.SceneRenderer.ClusteredLighting.ChangeGridResolution(width, height, depth);
			bool debugTiles = this.Engine.Graphics.GPUProgramStore.PostEffectProgram.DebugTiles;
			ClusteredLighting clusteredLighting = this.SceneRenderer.ClusteredLighting;
			Vector2 resolution = (!debugTiles) ? Vector2.Zero : new Vector2(clusteredLighting.GridWidth, clusteredLighting.GridHeight);
			this.PostEffectRenderer.UpdateDebugTileResolution(resolution);
		}

		// Token: 0x060042A6 RID: 17062 RVA: 0x000CB844 File Offset: 0x000C9A44
		public void UseClusteredLightingCustomZDistribution(bool enable)
		{
			this.SceneRenderer.ClusteredLighting.UseCustomZDistribution(enable);
			GPUProgramStore gpuprogramStore = this.Engine.Graphics.GPUProgramStore;
			bool flag = gpuprogramStore.LightClusteredProgram.UseCustomZDistribution != enable;
			if (flag)
			{
				gpuprogramStore.LightClusteredProgram.UseCustomZDistribution = enable;
				gpuprogramStore.MapChunkAlphaBlendedProgram.UseCustomZDistribution = enable;
				gpuprogramStore.MapBlockAnimatedProgram.UseCustomZDistribution = enable;
				gpuprogramStore.ParticleProgram.UseCustomZDistribution = enable;
				gpuprogramStore.ParticleErosionProgram.UseCustomZDistribution = enable;
				gpuprogramStore.LightClusteredProgram.Reset(true);
				gpuprogramStore.MapChunkAlphaBlendedProgram.Reset(true);
				gpuprogramStore.MapBlockAnimatedProgram.Reset(true);
				gpuprogramStore.ParticleProgram.Reset(true);
				gpuprogramStore.ParticleErosionProgram.Reset(true);
			}
		}

		// Token: 0x060042A7 RID: 17063 RVA: 0x000CB90C File Offset: 0x000C9B0C
		public void UseClusteredLightingDirectAccess(bool enable)
		{
			this.SceneRenderer.ClusteredLighting.UseLightDirectAccess(enable);
			GPUProgramStore gpuprogramStore = this.Engine.Graphics.GPUProgramStore;
			bool flag = gpuprogramStore.LightClusteredProgram.UseLightDirectAccess != enable;
			if (flag)
			{
				gpuprogramStore.LightClusteredProgram.UseLightDirectAccess = enable;
				gpuprogramStore.MapChunkAlphaBlendedProgram.UseLightDirectAccess = enable;
				gpuprogramStore.MapBlockAnimatedProgram.UseLightDirectAccess = enable;
				gpuprogramStore.ParticleProgram.UseLightDirectAccess = enable;
				gpuprogramStore.ParticleErosionProgram.UseLightDirectAccess = enable;
				gpuprogramStore.LightClusteredProgram.Reset(true);
				gpuprogramStore.MapChunkAlphaBlendedProgram.Reset(true);
				gpuprogramStore.MapBlockAnimatedProgram.Reset(true);
				gpuprogramStore.ParticleProgram.Reset(true);
				gpuprogramStore.ParticleErosionProgram.Reset(true);
			}
		}

		// Token: 0x060042A8 RID: 17064 RVA: 0x000CB9D4 File Offset: 0x000C9BD4
		public void SetUseShadowBackfaceLODDistance(bool enable, float distance = -1f)
		{
			ZOnlyChunkProgram mapChunkShadowMapProgram = this.Engine.Graphics.GPUProgramStore.MapChunkShadowMapProgram;
			mapChunkShadowMapProgram.UseDistantBackfaceCulling = enable;
			mapChunkShadowMapProgram.DistantBackfaceCullingDistance = ((distance > 0f) ? distance : mapChunkShadowMapProgram.DistantBackfaceCullingDistance);
			mapChunkShadowMapProgram.Reset(true);
		}

		// Token: 0x060042A9 RID: 17065 RVA: 0x000CBA20 File Offset: 0x000C9C20
		public void UseAlphaBlendedChunksSunShadows(bool enable)
		{
			MapChunkAlphaBlendedProgram mapChunkAlphaBlendedProgram = this.Engine.Graphics.GPUProgramStore.MapChunkAlphaBlendedProgram;
			bool flag = mapChunkAlphaBlendedProgram.UseForwardSunShadows != enable;
			if (flag)
			{
				mapChunkAlphaBlendedProgram.UseForwardSunShadows = enable;
				mapChunkAlphaBlendedProgram.Reset(true);
			}
		}

		// Token: 0x060042AA RID: 17066 RVA: 0x000CBA68 File Offset: 0x000C9C68
		public void UseParticleSunShadows(bool enable)
		{
			GPUProgramStore gpuprogramStore = this.Engine.Graphics.GPUProgramStore;
			bool flag = gpuprogramStore.ParticleProgram.UseSunShadows != enable;
			if (flag)
			{
				gpuprogramStore.ParticleProgram.UseSunShadows = enable;
				gpuprogramStore.ParticleProgram.Reset(true);
			}
			bool flag2 = gpuprogramStore.ParticleErosionProgram.UseSunShadows != enable;
			if (flag2)
			{
				gpuprogramStore.ParticleErosionProgram.UseSunShadows = enable;
				gpuprogramStore.ParticleErosionProgram.Reset(true);
			}
		}

		// Token: 0x060042AB RID: 17067 RVA: 0x000CBAE8 File Offset: 0x000C9CE8
		private void InitShadowMapping()
		{
			this.SceneRenderer.SetSunShadowsEnabled(true);
			this.SceneRenderer.SetSunShadowsWithChunks(true);
			int sunShadowsCascadeCount = this.Engine.Graphics.IsGPULowEnd ? 3 : 4;
			this.SceneRenderer.SetSunShadowsCascadeCount(sunShadowsCascadeCount);
			this.SceneRenderer.SetSunShadowMapResolution(1024U, 1024U);
			this.SceneRenderer.SetSunShadowsIntensity(0.7f);
			this.SceneRenderer.SetDeferredShadowsBlurEnabled(true);
			this.SceneRenderer.SetDeferredShadowsNoiseEnabled(true);
			this.SceneRenderer.SetDeferredShadowsFadingEnabled(true);
			this.SceneRenderer.SetSunShadowMappingStableProjectionEnabled(false);
			this.SceneRenderer.SetSunShadowsGlobalKDopEnabled(true);
			this.SceneRenderer.SetSunShadowCastersSmartCascadeDispatchEnabled(true);
			this.SceneRenderer.SetSunShadowCastersDrawInstancedEnabled(false);
			this.SceneRenderer.SetSunShadowsSafeAngleEnabled(true);
			this.SceneRenderer.SetSunShadowsDirectionSun(true);
			this.SceneRenderer.SetDeferredShadowsCameraBiasEnabled(true);
			this.SceneRenderer.SetDeferredShadowsNormalBiasEnabled(true);
			this.SceneRenderer.SetDeferredShadowsManualModeEnabled(false);
			this.SceneRenderer.SetSunShadowMapCachingEnabled(true);
			float deferredShadowResolutionScale = this.Engine.Graphics.IsGPULowEnd ? 0.5f : 1f;
			this.SceneRenderer.SetDeferredShadowResolutionScale(deferredShadowResolutionScale);
		}

		// Token: 0x170010D8 RID: 4312
		// (get) Token: 0x060042AC RID: 17068 RVA: 0x000CBC30 File Offset: 0x000C9E30
		// (set) Token: 0x060042AD RID: 17069 RVA: 0x000CBC38 File Offset: 0x000C9E38
		public uint FrameCounter { get; private set; }

		// Token: 0x170010D9 RID: 4313
		// (get) Token: 0x060042AE RID: 17070 RVA: 0x000CBC41 File Offset: 0x000C9E41
		// (set) Token: 0x060042AF RID: 17071 RVA: 0x000CBC49 File Offset: 0x000C9E49
		public float FrameTime { get; private set; }

		// Token: 0x170010DA RID: 4314
		// (get) Token: 0x060042B0 RID: 17072 RVA: 0x000CBC52 File Offset: 0x000C9E52
		// (set) Token: 0x060042B1 RID: 17073 RVA: 0x000CBC5A File Offset: 0x000C9E5A
		public float DeltaTime { get; private set; }

		// Token: 0x060042B2 RID: 17074 RVA: 0x000CBC64 File Offset: 0x000C9E64
		public void SetUseLOD(bool enable)
		{
			this.MapModule.LODSetup.Enabled = enable;
			this.ParticleSystemStoreModule.DistanceCheck = enable;
			GPUProgramStore gpuprogramStore = this.Engine.Graphics.GPUProgramStore;
			gpuprogramStore.MapChunkNearAlphaTestedProgram.UseLOD = enable;
			gpuprogramStore.MapChunkFarAlphaTestedProgram.UseLOD = enable;
			gpuprogramStore.MapChunkNearAlphaTestedProgram.Reset(true);
			gpuprogramStore.MapChunkFarAlphaTestedProgram.Reset(true);
		}

		// Token: 0x060042B3 RID: 17075 RVA: 0x000CBCD4 File Offset: 0x000C9ED4
		public void SetLODDistance(uint distance, uint range = 0U)
		{
			float num = MathHelper.Clamp(distance, 0f, 512f);
			GPUProgramStore gpuprogramStore = this.Engine.Graphics.GPUProgramStore;
			gpuprogramStore.MapChunkNearAlphaTestedProgram.LODDistance = num;
			gpuprogramStore.MapChunkFarAlphaTestedProgram.LODDistance = num;
			gpuprogramStore.MapChunkNearAlphaTestedProgram.Reset(true);
			gpuprogramStore.MapChunkFarAlphaTestedProgram.Reset(true);
			this.MapModule.LODSetup.StartDistance = num;
			this.MapModule.LODSetup.InvRange = ((range == 0U) ? this.MapModule.LODSetup.InvRange : (1f / range));
		}

		// Token: 0x060042B4 RID: 17076 RVA: 0x000CBD78 File Offset: 0x000C9F78
		public void PrintLODState()
		{
			this.Chat.Log(string.Format("LOD state:\n- enabled ? {0}!\n- distance start {1}\n- range {2}\n- distance treshold for entity rotation {3}", new object[]
			{
				this.MapModule.LODSetup.Enabled,
				this.MapModule.LODSetup.StartDistance,
				1.0 / (double)this.MapModule.LODSetup.InvRange,
				this.EntityStoreModule.CurrentSetup.DistanceToCameraBeforeRotation
			}));
		}

		// Token: 0x170010DB RID: 4315
		// (get) Token: 0x060042B5 RID: 17077 RVA: 0x000CBE0D File Offset: 0x000CA00D
		// (set) Token: 0x060042B6 RID: 17078 RVA: 0x000CBE15 File Offset: 0x000CA015
		public float ResolutionScale { get; private set; } = 1f;

		// Token: 0x060042B7 RID: 17079 RVA: 0x000CBE20 File Offset: 0x000CA020
		public bool SetResolutionScale(float scale)
		{
			bool flag = scale < this.ResolutionScaleMin || scale > this.ResolutionScaleMax;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this.ResolutionScale = scale;
				this.Resize(this.Engine.Window.Viewport.Width, this.Engine.Window.Viewport.Height);
				result = true;
			}
			return result;
		}

		// Token: 0x170010DC RID: 4316
		// (get) Token: 0x060042B8 RID: 17080 RVA: 0x000CBE8C File Offset: 0x000CA08C
		// (set) Token: 0x060042B9 RID: 17081 RVA: 0x000CBEA4 File Offset: 0x000CA0A4
		public bool TestBranch
		{
			get
			{
				return this._testBranch;
			}
			set
			{
				this._testBranch = value;
			}
		}

		// Token: 0x060042BA RID: 17082 RVA: 0x000CBEB0 File Offset: 0x000CA0B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void BeginDebugEntitiesZTest()
		{
			bool debugEntitiesZTest = this.DebugEntitiesZTest;
			if (debugEntitiesZTest)
			{
				this.Engine.Graphics.GL.Disable(GL.DEPTH_TEST);
			}
		}

		// Token: 0x060042BB RID: 17083 RVA: 0x000CBEE4 File Offset: 0x000CA0E4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void EndDebugEntitiesZTest()
		{
			bool debugEntitiesZTest = this.DebugEntitiesZTest;
			if (debugEntitiesZTest)
			{
				this.Engine.Graphics.GL.Enable(GL.DEPTH_TEST);
			}
		}

		// Token: 0x060042BC RID: 17084 RVA: 0x000CBF18 File Offset: 0x000CA118
		public void ToggleDebugParticleOverdraw()
		{
			this._debugParticleOverdraw = !this._debugParticleOverdraw;
			this.Engine.Graphics.GPUProgramStore.ParticleProgram.UseDebugOverdraw = this._debugParticleOverdraw;
			this.Engine.Graphics.GPUProgramStore.ParticleProgram.Reset(true);
			bool debugParticleOverdraw = this._debugParticleOverdraw;
			if (debugParticleOverdraw)
			{
				string[] names = new string[]
				{
					"overdraw"
				};
				this.SelectDebugMaps(names, false);
			}
			else
			{
				this.DebugMap = false;
			}
		}

		// Token: 0x060042BD RID: 17085 RVA: 0x000CBFA0 File Offset: 0x000CA1A0
		public void ToggleDebugParticleTexture()
		{
			this._debugParticleTexture = !this._debugParticleTexture;
			this.Engine.Graphics.GPUProgramStore.ParticleProgram.UseDebugTexture = this._debugParticleTexture;
			this.Engine.Graphics.GPUProgramStore.ParticleProgram.Reset(true);
		}

		// Token: 0x060042BE RID: 17086 RVA: 0x000CBFF9 File Offset: 0x000CA1F9
		public void ToggleDebugParticleBoundingVolume()
		{
			this._debugParticleBoundingVolume = !this._debugParticleBoundingVolume;
		}

		// Token: 0x060042BF RID: 17087 RVA: 0x000CC00A File Offset: 0x000CA20A
		public void ToggleDebugParticleZTest()
		{
			this._debugParticleZTestEnabled = !this._debugParticleZTestEnabled;
		}

		// Token: 0x060042C0 RID: 17088 RVA: 0x000CC01B File Offset: 0x000CA21B
		public void ToggleParticleSimulationPaused()
		{
			this.Engine.FXSystem.Particles.SetPaused(!this.Engine.FXSystem.Particles.IsPaused);
		}

		// Token: 0x060042C1 RID: 17089 RVA: 0x000CC04C File Offset: 0x000CA24C
		public void ToggleDebugParticleUVMotion()
		{
			this._debugParticleUVMotion = !this._debugParticleUVMotion;
			this.Engine.Graphics.GPUProgramStore.ParticleProgram.UseDebugUVMotion = this._debugParticleUVMotion;
			this.Engine.Graphics.GPUProgramStore.ParticleErosionProgram.UseDebugUVMotion = this._debugParticleUVMotion;
			this.Engine.Graphics.GPUProgramStore.ParticleProgram.Reset(true);
			this.Engine.Graphics.GPUProgramStore.ParticleErosionProgram.Reset(true);
		}

		// Token: 0x060042C2 RID: 17090 RVA: 0x000CC0E1 File Offset: 0x000CA2E1
		public void SetParticleLowResRenderingEnabled(bool enable)
		{
			this.Engine.FXSystem.Particles.IsLowResRenderingEnabled = enable;
		}

		// Token: 0x060042C3 RID: 17091 RVA: 0x000CC0FC File Offset: 0x000CA2FC
		public void SetDebugLightClusters(bool enable)
		{
			bool flag = this._debugLightClusters != enable;
			if (flag)
			{
				this._debugLightClusters = enable;
				this.Engine.Graphics.GPUProgramStore.LightClusteredProgram.Debug = enable;
				this.Engine.Graphics.GPUProgramStore.PostEffectProgram.DebugTiles = enable;
				ClusteredLighting clusteredLighting = this.SceneRenderer.ClusteredLighting;
				Vector2 resolution = (!enable) ? Vector2.Zero : new Vector2(clusteredLighting.GridWidth, clusteredLighting.GridHeight);
				this.PostEffectRenderer.UpdateDebugTileResolution(resolution);
				this.Engine.Graphics.GPUProgramStore.LightClusteredProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.PostEffectProgram.Reset(true);
			}
		}

		// Token: 0x060042C4 RID: 17092 RVA: 0x000CC1D0 File Offset: 0x000CA3D0
		public void SetChunkUseFoliageFading(bool enable)
		{
			bool flag = this._chunkUseFoliageFading != enable;
			if (flag)
			{
				this._chunkUseFoliageFading = enable;
				this.Engine.Graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.UseFoliageFading = enable;
				this.Engine.Graphics.GPUProgramStore.MapChunkFarAlphaTestedProgram.UseFoliageFading = enable;
				this.Engine.Graphics.GPUProgramStore.MapChunkFarOpaqueProgram.UseFoliageFading = enable;
				this.Engine.Graphics.GPUProgramStore.MapChunkNearAlphaTestedProgram.UseFoliageFading = enable;
				this.Engine.Graphics.GPUProgramStore.MapChunkNearOpaqueProgram.UseFoliageFading = enable;
				this.Engine.Graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.MapChunkFarAlphaTestedProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.MapChunkFarOpaqueProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.MapChunkNearAlphaTestedProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.MapChunkNearOpaqueProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.MapBlockAnimatedProgram.Reset(true);
			}
		}

		// Token: 0x060042C5 RID: 17093 RVA: 0x000CC32C File Offset: 0x000CA52C
		public void SetDebugChunkBoundaries(bool enable)
		{
			bool flag = this._debugChunkBoundaries != enable;
			if (flag)
			{
				this._debugChunkBoundaries = enable;
				this.Engine.Graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.UseDebugBoundaries = enable;
				this.Engine.Graphics.GPUProgramStore.MapChunkFarAlphaTestedProgram.UseDebugBoundaries = enable;
				this.Engine.Graphics.GPUProgramStore.MapChunkFarOpaqueProgram.UseDebugBoundaries = enable;
				this.Engine.Graphics.GPUProgramStore.MapChunkNearAlphaTestedProgram.UseDebugBoundaries = enable;
				this.Engine.Graphics.GPUProgramStore.MapChunkNearOpaqueProgram.UseDebugBoundaries = enable;
				this.Engine.Graphics.GPUProgramStore.MapBlockAnimatedProgram.UseDebugBoundaries = enable;
				this.Engine.Graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.MapChunkFarAlphaTestedProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.MapChunkFarOpaqueProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.MapChunkNearAlphaTestedProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.MapChunkNearOpaqueProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.MapBlockAnimatedProgram.Reset(true);
			}
		}

		// Token: 0x060042C6 RID: 17094 RVA: 0x000CC4A0 File Offset: 0x000CA6A0
		public string GetDebugPixelInfoList()
		{
			return string.Format("{0}", string.Join(", ", Enum.GetNames(typeof(DeferredProgram.DebugPixelInfo))));
		}

		// Token: 0x060042C7 RID: 17095 RVA: 0x000CC4D8 File Offset: 0x000CA6D8
		public bool SetDebugPixelInfo(bool enable, string name = null)
		{
			bool result = false;
			DeferredProgram.DebugPixelInfo debugPixelInfo = DeferredProgram.DebugPixelInfo.None;
			bool flag = name != null;
			if (flag)
			{
				debugPixelInfo = (DeferredProgram.DebugPixelInfo)Enum.Parse(typeof(DeferredProgram.DebugPixelInfo), name, true);
			}
			DeferredProgram deferredProgram = this.Engine.Graphics.GPUProgramStore.DeferredProgram;
			bool flag2 = debugPixelInfo != deferredProgram.DebugPixelInfoView;
			if (flag2)
			{
				deferredProgram.DebugPixelInfoView = debugPixelInfo;
				deferredProgram.Reset(true);
				result = true;
			}
			return result;
		}

		// Token: 0x060042C8 RID: 17096 RVA: 0x000CC54C File Offset: 0x000CA74C
		public int SelectDebugMaps(string[] names, bool verticalDisplay)
		{
			this._debugMapVerticalDisplay = verticalDisplay;
			RenderTargetStore rtstore = this.Engine.Graphics.RTStore;
			int num = 0;
			for (int i = 0; i < names.Length; i++)
			{
				bool flag = rtstore.ContainsDebugMap(names[i]);
				num += (flag ? 1 : 0);
			}
			this.DebugMap = (num > 0);
			this._activeDebugMapsNames = new string[num];
			int num2 = 0;
			for (int j = 0; j < names.Length; j++)
			{
				bool flag2 = rtstore.ContainsDebugMap(names[j]);
				bool flag3 = flag2;
				if (flag3)
				{
					this._activeDebugMapsNames[num2] = names[j];
					num2++;
				}
			}
			return num;
		}

		// Token: 0x060042C9 RID: 17097 RVA: 0x000CC5FF File Offset: 0x000CA7FF
		public void SetSkyAmbientIntensity(float value)
		{
			this.SkyAmbientIntensity = Math.Min(1f, Math.Max(value, 0f));
		}

		// Token: 0x060042CA RID: 17098 RVA: 0x000CC620 File Offset: 0x000CA820
		public void InitFog()
		{
			GLFunctions gl = this.Engine.Graphics.GL;
			RenderTargetStore rtstore = this.Engine.Graphics.RTStore;
			this.UseFog(this.WeatherModule.ActiveFogMode);
			this.UseMoodFog(true);
			this.UseMoodFogSmoothColor(false);
			this.SetMoodFogDensityVariationScale(1f);
			this.SetMoodFogSpeedFactor(1f);
			int width = this.Engine.Graphics.IsGPULowEnd ? 256 : 512;
			rtstore.SunOcclusionHistory.Resize(width, 1, false);
			rtstore.SunOcclusionHistory.Bind(false, true);
			float[] data = new float[]
			{
				0.5f
			};
			gl.ClearBufferfv(GL.COLOR, 0, data);
			rtstore.SunOcclusionHistory.Unbind();
			RenderTarget.BindHardwareFramebuffer();
		}

		// Token: 0x060042CB RID: 17099 RVA: 0x000CC6FC File Offset: 0x000CA8FC
		public void UseFog(WeatherModule.FogMode fogMode)
		{
			bool flag;
			switch (fogMode)
			{
			case WeatherModule.FogMode.Dynamic:
				flag = true;
				break;
			case WeatherModule.FogMode.Static:
				flag = true;
				break;
			case WeatherModule.FogMode.Off:
				flag = false;
				break;
			default:
				flag = false;
				break;
			}
			bool flag2 = this.Engine.Graphics.GPUProgramStore.ParticleProgram.UseFog != flag;
			if (flag2)
			{
				this.Engine.Graphics.GPUProgramStore.ParticleProgram.UseFog = flag;
				this.Engine.Graphics.GPUProgramStore.ParticleErosionProgram.UseFog = flag;
				this.Engine.Graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.UseFog = flag;
				this.Engine.Graphics.GPUProgramStore.DeferredProgram.UseFog = flag;
				this.Engine.Graphics.GPUProgramStore.MapBlockAnimatedProgram.UseFog = flag;
				this.Engine.Graphics.GPUProgramStore.SkyProgram.UseMoodFog = flag;
				this.Engine.Graphics.GPUProgramStore.CloudsProgram.UseMoodFog = flag;
				this.Engine.Graphics.GPUProgramStore.ParticleProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.ParticleErosionProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.DeferredProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.MapBlockAnimatedProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.SkyProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.CloudsProgram.Reset(true);
			}
		}

		// Token: 0x060042CC RID: 17100 RVA: 0x000CC8DC File Offset: 0x000CAADC
		public void UseMoodFog(bool enable)
		{
			bool flag = this._useMoodFog != enable;
			if (flag)
			{
				this._useMoodFog = enable;
				this.Engine.Graphics.GPUProgramStore.ParticleProgram.UseMoodFog = enable;
				this.Engine.Graphics.GPUProgramStore.ParticleErosionProgram.UseMoodFog = enable;
				this.Engine.Graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.UseMoodFog = enable;
				this.Engine.Graphics.GPUProgramStore.DeferredProgram.UseMoodFog = enable;
				this.Engine.Graphics.GPUProgramStore.MapBlockAnimatedProgram.UseMoodFog = enable;
				this.Engine.Graphics.GPUProgramStore.SkyProgram.UseMoodFog = enable;
				this.Engine.Graphics.GPUProgramStore.CloudsProgram.UseMoodFog = enable;
				this.Engine.Graphics.GPUProgramStore.ParticleProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.ParticleErosionProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.DeferredProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.MapBlockAnimatedProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.SkyProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.CloudsProgram.Reset(true);
			}
		}

		// Token: 0x060042CD RID: 17101 RVA: 0x000CCA88 File Offset: 0x000CAC88
		public void UseMoodFogOnSky(bool enable)
		{
			this.Engine.Graphics.GPUProgramStore.SkyProgram.UseMoodFog = enable;
			this.Engine.Graphics.GPUProgramStore.CloudsProgram.UseMoodFog = enable;
			this.Engine.Graphics.GPUProgramStore.SkyProgram.Reset(true);
			this.Engine.Graphics.GPUProgramStore.CloudsProgram.Reset(true);
		}

		// Token: 0x060042CE RID: 17102 RVA: 0x000CCB04 File Offset: 0x000CAD04
		public void UseCustomMoodFog(bool enable)
		{
			this._useCustomMoodFog = enable;
		}

		// Token: 0x060042CF RID: 17103 RVA: 0x000CCB10 File Offset: 0x000CAD10
		public void SetMoodFogCustomDensity(float density)
		{
			bool useCustomMoodFog = this._useCustomMoodFog;
			if (useCustomMoodFog)
			{
				this._customDensity = density;
			}
		}

		// Token: 0x060042D0 RID: 17104 RVA: 0x000CCB34 File Offset: 0x000CAD34
		public void SetMoodFogHeightCustomHeightFalloff(float falloff)
		{
			bool useCustomMoodFog = this._useCustomMoodFog;
			if (useCustomMoodFog)
			{
				this._customHeightFalloff = falloff;
			}
		}

		// Token: 0x060042D1 RID: 17105 RVA: 0x000CCB55 File Offset: 0x000CAD55
		public void SetMoodFogDensityVariationScale(float variation)
		{
			this._densityVariationScale = variation;
			this.SceneRenderer.Data.FogMoodParams.W = variation;
		}

		// Token: 0x060042D2 RID: 17106 RVA: 0x000CCB75 File Offset: 0x000CAD75
		public void SetMoodFogSpeedFactor(float speed)
		{
			this._fogSpeedFactor = speed;
			this.SceneRenderer.Data.FogMoodParams.Z = speed;
		}

		// Token: 0x060042D3 RID: 17107 RVA: 0x000CCB98 File Offset: 0x000CAD98
		public void SetMoodFogDensityUnderwater(float density)
		{
			bool useMoodFog = this._useMoodFog;
			if (useMoodFog)
			{
				this.SceneRenderer.Data.FogDensityUnderwater = density;
			}
		}

		// Token: 0x060042D4 RID: 17108 RVA: 0x000CCBC3 File Offset: 0x000CADC3
		public void SetMoodFogHeightFalloffUnderwater(float falloff)
		{
			this.SceneRenderer.Data.FogHeightFalloffUnderwater = falloff;
		}

		// Token: 0x060042D5 RID: 17109 RVA: 0x000CCBD8 File Offset: 0x000CADD8
		public void UseMoodFogDithering(bool enable)
		{
			bool flag = this.Engine.Graphics.GPUProgramStore.DeferredProgram.UseDithering != enable;
			if (flag)
			{
				this.Engine.Graphics.GPUProgramStore.DeferredProgram.UseDithering = enable;
				this.Engine.Graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.UseDithering = enable;
				this.Engine.Graphics.GPUProgramStore.MapBlockAnimatedProgram.UseDithering = enable;
				this.Engine.Graphics.GPUProgramStore.SkyProgram.UseDitheringOnFog = enable;
				this.Engine.Graphics.GPUProgramStore.CloudsProgram.UseDithering = enable;
				this.Engine.Graphics.GPUProgramStore.DeferredProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.MapBlockAnimatedProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.SkyProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.CloudsProgram.Reset(true);
			}
		}

		// Token: 0x060042D6 RID: 17110 RVA: 0x000CCD24 File Offset: 0x000CAF24
		public void UseMoodFogDitheringOnSkyAndClouds(bool enable)
		{
			this.Engine.Graphics.GPUProgramStore.SkyProgram.UseDitheringOnFog = enable;
			this.Engine.Graphics.GPUProgramStore.CloudsProgram.UseDithering = enable;
			this.Engine.Graphics.GPUProgramStore.SkyProgram.Reset(true);
			this.Engine.Graphics.GPUProgramStore.CloudsProgram.Reset(true);
		}

		// Token: 0x060042D7 RID: 17111 RVA: 0x000CCDA0 File Offset: 0x000CAFA0
		public void UseMoodFogSmoothColor(bool enable)
		{
			bool flag = this.Engine.Graphics.GPUProgramStore.DeferredProgram.UseSmoothNearMoodColor != enable;
			if (flag)
			{
				this.Engine.Graphics.GPUProgramStore.DeferredProgram.UseSmoothNearMoodColor = enable;
				this.Engine.Graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.UseSmoothNearMoodColor = enable;
				this.Engine.Graphics.GPUProgramStore.ParticleProgram.UseSmoothNearMoodColor = enable;
				this.Engine.Graphics.GPUProgramStore.ParticleErosionProgram.UseSmoothNearMoodColor = enable;
				this.Engine.Graphics.GPUProgramStore.MapBlockAnimatedProgram.UseSmoothNearMoodColor = enable;
				this.Engine.Graphics.GPUProgramStore.DeferredProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.ParticleProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.ParticleErosionProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.MapBlockAnimatedProgram.Reset(true);
			}
		}

		// Token: 0x060042D8 RID: 17112 RVA: 0x000CCEEC File Offset: 0x000CB0EC
		public void PrintFogState()
		{
			string text = "Fog state :";
			switch (this.WeatherModule.ActiveFogMode)
			{
			case WeatherModule.FogMode.Dynamic:
				text += " dynamic";
				break;
			case WeatherModule.FogMode.Static:
				text += " static";
				break;
			case WeatherModule.FogMode.Off:
				text += " off";
				break;
			}
			bool useMoodFog = this._useMoodFog;
			if (useMoodFog)
			{
				float num = this.SceneRenderer.Data.FogHeightFalloffUnderwater * 100f;
				float num2 = (float)Math.Round(Math.Log((double)(this.SceneRenderer.Data.FogDensityUnderwater + 1f)), 2);
				float num3 = this._useCustomMoodFog ? this._customDensity : this.WeatherModule.FogDensity;
				float num4 = this._useCustomMoodFog ? this._customHeightFalloff : this.WeatherModule.FogHeightFalloff;
				text += " mood_on";
				bool useCustomMoodFog = this._useCustomMoodFog;
				if (useCustomMoodFog)
				{
					text += " custom";
				}
				text = text + "\n underwater density : " + num2.ToString();
				text = text + "\n underwater falloff : " + num.ToString();
				text = text + "\n global density : " + num3.ToString();
				text = text + "\n global falloff : " + num4.ToString();
				text = text + "\n speed factor : " + this._fogSpeedFactor.ToString();
				text = text + "\n variation scale : " + this._densityVariationScale.ToString();
			}
			else
			{
				text += " mood_off";
			}
			this.Chat.Log(text);
		}

		// Token: 0x060042D9 RID: 17113 RVA: 0x000CD094 File Offset: 0x000CB294
		public void UseDitheringOnSky(bool enable)
		{
			this.Engine.Graphics.GPUProgramStore.SkyProgram.UseDitheringOnSky = enable;
			this.Engine.Graphics.GPUProgramStore.SkyProgram.Reset(true);
		}

		// Token: 0x060042DA RID: 17114 RVA: 0x000CD0D0 File Offset: 0x000CB2D0
		public void SetWaterQuality(int quality)
		{
			bool flag = this._waterQuality != quality;
			if (flag)
			{
				bool flag2 = this._waterQuality == 0 || quality == 0;
				if (flag2)
				{
					this.Engine.Graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.WriteRenderConfigBitsInAlpha = (quality != 0);
					this.Engine.Graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.Reset(true);
				}
				this._waterQuality = quality;
			}
		}

		// Token: 0x060042DB RID: 17115 RVA: 0x000CD148 File Offset: 0x000CB348
		public void SetUseUnderwaterCaustics(bool enable)
		{
			GraphicsDevice graphics = this.Engine.Graphics;
			bool flag = graphics.GPUProgramStore.DeferredProgram.UseUnderwaterCaustics != enable;
			if (flag)
			{
				graphics.GPUProgramStore.DeferredProgram.UseUnderwaterCaustics = enable;
				graphics.GPUProgramStore.DeferredProgram.Reset(true);
			}
			bool flag2 = graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.UseUnderwaterCaustics != enable;
			if (flag2)
			{
				graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.UseUnderwaterCaustics = enable;
				graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.Reset(true);
			}
		}

		// Token: 0x060042DC RID: 17116 RVA: 0x000CD1E0 File Offset: 0x000CB3E0
		public void SetUnderwaterCausticsIntensity(float value)
		{
			this._underwaterCausticsIntensity = Math.Min(1f, Math.Max(value, 0f));
		}

		// Token: 0x060042DD RID: 17117 RVA: 0x000CD1FE File Offset: 0x000CB3FE
		public void SetUnderwaterCausticsScale(float value)
		{
			this._underwaterCausticsScale = value;
		}

		// Token: 0x060042DE RID: 17118 RVA: 0x000CD208 File Offset: 0x000CB408
		public void SetUnderwaterCausticsDistortion(float value)
		{
			this._underwaterCausticsDistortion = value;
		}

		// Token: 0x060042DF RID: 17119 RVA: 0x000CD214 File Offset: 0x000CB414
		public void PrintUnderwaterCausticsParams()
		{
			this.Chat.Log(string.Format("Underwater caustics current params: enabled = {0}, itensity = {1}, scale = {2}, distortion = {3}.", new object[]
			{
				this.Engine.Graphics.GPUProgramStore.DeferredProgram.UseUnderwaterCaustics,
				this._underwaterCausticsIntensity,
				this._underwaterCausticsScale,
				this._underwaterCausticsDistortion
			}));
		}

		// Token: 0x060042E0 RID: 17120 RVA: 0x000CD28A File Offset: 0x000CB48A
		public void SetCloudsUVMotionScale(float value)
		{
			this._cloudsUVMotionScale = value;
		}

		// Token: 0x060042E1 RID: 17121 RVA: 0x000CD294 File Offset: 0x000CB494
		public void SetCloudsUVMotionStrength(float value)
		{
			this._cloudsUVMotionStrength = value * 0.01f;
		}

		// Token: 0x060042E2 RID: 17122 RVA: 0x000CD2A4 File Offset: 0x000CB4A4
		public void PrintCloudsUVMotionParams()
		{
			this.Chat.Log(string.Format("Clouds UV Motion params: scale = {0}, strength = {1}.", this._cloudsUVMotionScale, this._cloudsUVMotionStrength * 100f));
		}

		// Token: 0x060042E3 RID: 17123 RVA: 0x000CD2DC File Offset: 0x000CB4DC
		public void SetUseCloudsShadows(bool enable)
		{
			GraphicsDevice graphics = this.Engine.Graphics;
			bool flag = graphics.GPUProgramStore.DeferredProgram.UseCloudsShadows != enable;
			if (flag)
			{
				graphics.GPUProgramStore.DeferredProgram.UseCloudsShadows = enable;
				graphics.GPUProgramStore.DeferredProgram.Reset(true);
			}
			bool flag2 = graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.UseCloudsShadows != enable;
			if (flag2)
			{
				graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.UseCloudsShadows = enable;
				graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.Reset(true);
			}
		}

		// Token: 0x060042E4 RID: 17124 RVA: 0x000CD374 File Offset: 0x000CB574
		public void SetCloudsShadowsIntensity(float value)
		{
			this._cloudsShadowsIntensity = Math.Min(1f, Math.Max(value, 0f));
		}

		// Token: 0x060042E5 RID: 17125 RVA: 0x000CD392 File Offset: 0x000CB592
		public void SetCloudsShadowsScale(float value)
		{
			this._cloudsShadowsScale = value;
		}

		// Token: 0x060042E6 RID: 17126 RVA: 0x000CD39C File Offset: 0x000CB59C
		public void SetCloudsShadowsBlurriness(float value)
		{
			this._cloudsShadowsBlurriness = value;
		}

		// Token: 0x060042E7 RID: 17127 RVA: 0x000CD3A6 File Offset: 0x000CB5A6
		public void SetCloudsShadowsSpeed(float value)
		{
			this._cloudsShadowsSpeed = value;
		}

		// Token: 0x060042E8 RID: 17128 RVA: 0x000CD3B0 File Offset: 0x000CB5B0
		public void PrintCloudsShadowsParams()
		{
			this.Chat.Log(string.Format("Clouds shadows current params: enabled = {0}, itensity = {1}, scale = {2}, blur = {3}, speed = {4}.", new object[]
			{
				this.Engine.Graphics.GPUProgramStore.DeferredProgram.UseCloudsShadows,
				this._cloudsShadowsIntensity,
				this._cloudsShadowsScale,
				this._cloudsShadowsBlurriness,
				this._cloudsShadowsSpeed
			}));
		}

		// Token: 0x060042E9 RID: 17129 RVA: 0x000CD434 File Offset: 0x000CB634
		public void InitPostEffects()
		{
			RenderTargetStore rtstore = this.Engine.Graphics.RTStore;
			PostEffectProgram postEffectProgram = this.Engine.Graphics.GPUProgramStore.PostEffectProgram;
			this.PostEffectRenderer = new PostEffectRenderer(this.Engine.Graphics, this.Engine.Profiling, postEffectProgram);
			this.PostEffectRenderer.InitDepthOfField(rtstore.SceneColor.GetTexture(RenderTarget.Target.Depth), false, 2, 1f, 2f, 30f, 70f, 0.5f, 0.3f);
			this.PostEffectRenderer.InitBloom(this.WeatherModule.SkyRenderer.SunTexture, this.WeatherModule.SkyRenderer.MoonTexture, this._glowMask.GLTexture, new Action(this.WeatherModule.SkyRenderer.DrawSun), new Action(this.WeatherModule.SkyRenderer.DrawMoon), true, true, false, false, true, true, 0, 0.3f, 8f, 0.25f, 0.3f, 4f);
		}

		// Token: 0x060042EA RID: 17130 RVA: 0x000CD548 File Offset: 0x000CB748
		public void UseVolumetricSunshaft(bool enable)
		{
			this._useVolumetricSunshaft = enable;
			PostEffectProgram postEffectProgram = this.Engine.Graphics.GPUProgramStore.PostEffectProgram;
			postEffectProgram.UseVolumetricSunshaft = enable;
			postEffectProgram.Reset(true);
		}

		// Token: 0x060042EB RID: 17131 RVA: 0x000CD584 File Offset: 0x000CB784
		private void InitForceField()
		{
			Image image = new Image(File.ReadAllBytes(Path.Combine(Paths.GameData, "ShaderTextures/ShieldNormalMap.png")));
			this._forceFieldNormalMap = new Texture(Texture.TextureTypes.Texture2D);
			this._forceFieldNormalMap.CreateTexture2D(image.Width, image.Height, image.Pixels, 5, GL.LINEAR_MIPMAP_NEAREST, GL.LINEAR, GL.REPEAT, GL.REPEAT, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, true);
			this.Engine.FXSystem.ForceFields.NormalMap = this._forceFieldNormalMap.GLTexture;
		}

		// Token: 0x060042EC RID: 17132 RVA: 0x000CD61B File Offset: 0x000CB81B
		private void DisposeForceField()
		{
			this._forceFieldNormalMap.Dispose();
		}

		// Token: 0x060042ED RID: 17133 RVA: 0x000CD62C File Offset: 0x000CB82C
		private void UpdateForceField()
		{
			ForceFieldProgram forceFieldProgram = this.Engine.Graphics.GPUProgramStore.ForceFieldProgram;
			Vector2 uvAnimationSpeed = new Vector2(0f, -0.05f);
			Vector4 color = new Vector4(0f, 0.4f, 0.8f, 0.15f);
			Vector4 intersectionHighlightColorOpacity = new Vector4(0f, 0.4f, 0.8f, 0.75f);
			float intersectionHighlightThickness = 1f;
			bool flag = this.ForceFieldTest == 1;
			ForceFieldFXSystem.FXShape shape;
			float num;
			int outlineMode;
			if (flag)
			{
				shape = ForceFieldFXSystem.FXShape.Quad;
				num = 5f;
				outlineMode = (this.ForceFieldOptionOutline ? forceFieldProgram.OutlineModeUV : forceFieldProgram.OutlineModeNone);
			}
			else
			{
				bool flag2 = this.ForceFieldTest == 2;
				if (flag2)
				{
					shape = ForceFieldFXSystem.FXShape.Sphere;
					num = 1.5f;
					outlineMode = (this.ForceFieldOptionOutline ? forceFieldProgram.OutlineModeNormal : forceFieldProgram.OutlineModeNone);
				}
				else
				{
					shape = ForceFieldFXSystem.FXShape.Sphere;
					num = 1.5f;
					outlineMode = (this.ForceFieldOptionOutline ? forceFieldProgram.OutlineModeNormal : forceFieldProgram.OutlineModeNone);
				}
			}
			int num2 = Math.Min(this.ForceFieldCount, 20);
			Vector3 value = new Vector3(0f, 0f, num);
			for (int i = 0; i < num2; i++)
			{
				bool flag3 = this.ForceFieldTest == 1;
				if (flag3)
				{
					this._forceFieldModelMatrices[i] = Matrix.CreateTranslation(new Vector3(0f, 0f, 15f) + (float)i * value + this.SceneRenderer.Data.PlayerRenderPosition);
				}
				else
				{
					bool flag4 = this.ForceFieldTest == 2;
					if (flag4)
					{
						this._forceFieldModelMatrices[i] = Matrix.CreateTranslation(new Vector3(0f, num * 0.5f, 0f) + (float)i * value + this.SceneRenderer.Data.PlayerRenderPosition);
					}
					else
					{
						this._forceFieldModelMatrices[i] = Matrix.CreateTranslation(new Vector3(0f, num * 0.5f, 0f) + (float)i * value + this.SceneRenderer.Data.PlayerRenderPosition);
					}
				}
				Matrix matrix = Matrix.CreateScale(num);
				Matrix.Multiply(ref matrix, ref this._forceFieldModelMatrices[i], out this._forceFieldModelMatrices[i]);
				this._forceFieldNormalMatrices[i] = Matrix.Transpose(Matrix.Invert(this._forceFieldModelMatrices[i] * this.SceneRenderer.Data.ViewRotationMatrix));
			}
			bool flag5 = this.ForceFieldTest > 0;
			if (flag5)
			{
				float num3 = this.ForceFieldOptionAnimation ? this.FrameTime : 0f;
				this.Engine.FXSystem.ForceFields.SetupSceneData(ref this.SceneRenderer.Data.ViewRotationMatrix, ref this.SceneRenderer.Data.ViewRotationProjectionMatrix);
				int num4 = num2;
				this.Engine.FXSystem.ForceFields.PrepareForIncomingColorTasks(num4);
				this.Engine.FXSystem.ForceFields.PrepareForIncomingDistortionTasks(num4);
				for (int j = 0; j < num4; j++)
				{
					bool forceFieldOptionColor = this.ForceFieldOptionColor;
					if (forceFieldOptionColor)
					{
						this.Engine.FXSystem.ForceFields.RegisterColorTask(shape, ref this._forceFieldModelMatrices[j], ref this._forceFieldNormalMatrices[j], uvAnimationSpeed, outlineMode, color, intersectionHighlightColorOpacity, intersectionHighlightThickness);
					}
					bool forceFieldOptionDistortion = this.ForceFieldOptionDistortion;
					if (forceFieldOptionDistortion)
					{
						this.Engine.FXSystem.ForceFields.RegisterDistortionTask(shape, ref this._forceFieldModelMatrices[j], uvAnimationSpeed);
					}
				}
			}
		}

		// Token: 0x060042EE RID: 17134 RVA: 0x000CDA08 File Offset: 0x000CBC08
		private void InitOIT()
		{
			Debug.Assert(this.SceneRenderer.OIT != null);
			this.SceneRenderer.OIT.SetupRenderingProfiles(70, 71, 72, 73, 74);
			this.SetupOIT(OrderIndependentTransparency.Method.MOIT);
			this.SetupOITPrepassScale(8U);
			this.SceneRenderer.OIT.SetupTextureUnits(18, 17);
			this.SceneRenderer.OIT.RegisterDrawTransparentsFunc(new DrawTransparencyFunc(this.DrawTransparentsFullRes), new DrawTransparencyFunc(this.DrawTransparentsHalfRes), null);
			this._oitRes = 0;
		}

		// Token: 0x060042EF RID: 17135 RVA: 0x000CDA9C File Offset: 0x000CBC9C
		public void SetUseChunksOIT(bool enable)
		{
			bool flag = this._useChunksOIT != enable;
			if (flag)
			{
				this._useChunksOIT = enable;
				this.Engine.Graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.UseOIT = enable;
				this.Engine.Graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.WriteRenderConfigBitsInAlpha = !enable;
				this.Engine.Graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.Reset(true);
			}
		}

		// Token: 0x060042F0 RID: 17136 RVA: 0x000CDB18 File Offset: 0x000CBD18
		public void ChangeOITResolution()
		{
			this._oitRes = (this._oitRes + 1) % 3;
			switch (this._oitRes)
			{
			case 0:
				this.SceneRenderer.OIT.RegisterDrawTransparentsFunc(new DrawTransparencyFunc(this.DrawTransparentsFullRes), new DrawTransparencyFunc(this.DrawTransparentsHalfRes), null);
				break;
			case 1:
				this.SceneRenderer.OIT.RegisterDrawTransparentsFunc(null, new DrawTransparencyFunc(this.DrawTransparentsFullRes), new DrawTransparencyFunc(this.DrawTransparentsHalfRes));
				break;
			case 2:
				this.SceneRenderer.OIT.RegisterDrawTransparentsFunc(null, null, new DrawTransparencyFunc(this.DrawTransparents));
				break;
			}
		}

		// Token: 0x060042F1 RID: 17137 RVA: 0x000CDBCC File Offset: 0x000CBDCC
		public void UseOITEdgeFixup(bool fixupHalfRes, bool fixupQuarterRes)
		{
			this.SceneRenderer.OIT.UseEdgeFixupPass(fixupHalfRes, fixupQuarterRes, 7);
		}

		// Token: 0x060042F2 RID: 17138 RVA: 0x000CDBE4 File Offset: 0x000CBDE4
		public void SetupOIT(OrderIndependentTransparency.Method method)
		{
			bool flag = this.SceneRenderer.OIT.CurrentMethod != method;
			if (flag)
			{
				this.SceneRenderer.OIT.SetMethod(method);
				this.Engine.Graphics.GPUProgramStore.ParticleProgram.UseOIT = (method > OrderIndependentTransparency.Method.None);
				this.Engine.Graphics.GPUProgramStore.ForceFieldProgram.UseOIT = (method > OrderIndependentTransparency.Method.None);
				this.Engine.Graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.UseOIT = (this._useChunksOIT && method > OrderIndependentTransparency.Method.None);
				this.Engine.Graphics.GPUProgramStore.ParticleProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.ForceFieldProgram.Reset(true);
				this.Engine.Graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.Reset(true);
			}
		}

		// Token: 0x060042F3 RID: 17139 RVA: 0x000CDCDC File Offset: 0x000CBEDC
		public bool SetupOITPrepassScale(uint prepassInvScale)
		{
			bool result = true;
			OrderIndependentTransparency.ResolutionScale prepassResolutionScale = OrderIndependentTransparency.ResolutionScale.Full;
			switch (prepassInvScale)
			{
			case 1U:
				prepassResolutionScale = OrderIndependentTransparency.ResolutionScale.Full;
				goto IL_3D;
			case 2U:
				prepassResolutionScale = OrderIndependentTransparency.ResolutionScale.Half;
				goto IL_3D;
			case 3U:
				break;
			case 4U:
				prepassResolutionScale = OrderIndependentTransparency.ResolutionScale.Quarter;
				goto IL_3D;
			default:
				if (prepassInvScale == 8U)
				{
					prepassResolutionScale = OrderIndependentTransparency.ResolutionScale.Eighth;
					goto IL_3D;
				}
				break;
			}
			result = false;
			IL_3D:
			this.SceneRenderer.OIT.SetPrepassResolutionScale(prepassResolutionScale);
			return result;
		}

		// Token: 0x170010DD RID: 4317
		// (get) Token: 0x060042F4 RID: 17140 RVA: 0x000CDD3F File Offset: 0x000CBF3F
		// (set) Token: 0x060042F5 RID: 17141 RVA: 0x000CDD47 File Offset: 0x000CBF47
		public string[] RenderPassNames { get; private set; } = new string[13];

		// Token: 0x060042F6 RID: 17142 RVA: 0x000CDD50 File Offset: 0x000CBF50
		public void SetRenderPassEnabled(uint passId, bool enable)
		{
			bool flag = passId >= 13U;
			if (flag)
			{
				throw new Exception(string.Format("Invalid pass id {0} - it should be < {1}", passId, 13U));
			}
			if (passId != 12U)
			{
				this._renderPassStates[(int)passId] = enable;
			}
			else
			{
				this.PostEffectRenderer.UseFXAA(enable);
			}
		}

		// Token: 0x060042F7 RID: 17143 RVA: 0x000CDDAC File Offset: 0x000CBFAC
		private void InitRenderSetup()
		{
			int num = 0;
			while ((long)num < 13L)
			{
				this._renderPassStates[num] = true;
				num++;
			}
			this.RenderPassNames[0] = "shadowmap";
			this.RenderPassNames[1] = "firstperson";
			this.RenderPassNames[2] = "map_near_opaque";
			this.RenderPassNames[3] = "map_near_alphatested";
			this.RenderPassNames[4] = "map_anim";
			this.RenderPassNames[5] = "map_far_opaque";
			this.RenderPassNames[6] = "map_far_alphatested";
			this.RenderPassNames[7] = "entities";
			this.RenderPassNames[8] = "sky";
			this.RenderPassNames[9] = "map_alphablended";
			this.RenderPassNames[10] = "vfx";
			this.RenderPassNames[11] = "names";
			this.RenderPassNames[12] = "postfx";
		}

		// Token: 0x060042F8 RID: 17144 RVA: 0x000CDE83 File Offset: 0x000CC083
		private void CreateCubeMapMesh()
		{
			MeshProcessor.CreateSimpleBox(ref this._cube, 2f);
		}

		// Token: 0x060042F9 RID: 17145 RVA: 0x000CDE98 File Offset: 0x000CC098
		private void DrawCubeMapTest()
		{
			GLFunctions gl = this.Engine.Graphics.GL;
			gl.Disable(GL.CULL_FACE);
			gl.ActiveTexture(GL.TEXTURE0);
			gl.BindTexture(GL.TEXTURE_CUBE_MAP, this._cubemap.GLTexture);
			gl.BindVertexArray(this._cube.VertexArray);
			CubemapProgram cubemapProgram = this.Engine.Graphics.GPUProgramStore.CubemapProgram;
			gl.UseProgram(cubemapProgram);
			Matrix identity = Matrix.Identity;
			Matrix.ApplyScale(ref identity, 30f);
			Matrix.Multiply(ref identity, ref this.SceneRenderer.Data.ViewRotationProjectionMatrix, out identity);
			cubemapProgram.MVPMatrix.SetValue(ref identity);
			gl.DrawElements(GL.TRIANGLES, this._cube.Count, GL.UNSIGNED_SHORT, (IntPtr)0);
			gl.Enable(GL.CULL_FACE);
		}

		// Token: 0x060042FA RID: 17146 RVA: 0x000CDF7B File Offset: 0x000CC17B
		private void DisposeCubemapMesh()
		{
			this._cube.Dispose();
		}

		// Token: 0x060042FB RID: 17147 RVA: 0x000CDF8C File Offset: 0x000CC18C
		public static byte[][] ReadCubemapImagesFromDisk(string pathToCubemapFolder)
		{
			Image image = new Image(File.ReadAllBytes(Path.Combine(pathToCubemapFolder, "right.png")));
			Image image2 = new Image(File.ReadAllBytes(Path.Combine(pathToCubemapFolder, "left.png")));
			Image image3 = new Image(File.ReadAllBytes(Path.Combine(pathToCubemapFolder, "top.png")));
			Image image4 = new Image(File.ReadAllBytes(Path.Combine(pathToCubemapFolder, "bottom.png")));
			Image image5 = new Image(File.ReadAllBytes(Path.Combine(pathToCubemapFolder, "front.png")));
			Image image6 = new Image(File.ReadAllBytes(Path.Combine(pathToCubemapFolder, "back.png")));
			Debug.Assert(image.Width == image2.Width && image2.Width == image3.Width && image3.Width == image4.Width && image4.Width == image5.Width && image5.Width == image6.Width);
			Debug.Assert(image.Height == image2.Height && image2.Height == image3.Height && image3.Height == image4.Height && image4.Height == image5.Height && image5.Height == image6.Height);
			return new byte[][]
			{
				image.Pixels,
				image2.Pixels,
				image3.Pixels,
				image4.Pixels,
				image5.Pixels,
				image6.Pixels
			};
		}

		// Token: 0x170010DE RID: 4318
		// (get) Token: 0x060042FC RID: 17148 RVA: 0x000CE112 File Offset: 0x000CC312
		// (set) Token: 0x060042FD RID: 17149 RVA: 0x000CE11A File Offset: 0x000CC31A
		public Point[] AtlasSizes { get; private set; }

		// Token: 0x060042FE RID: 17150 RVA: 0x000CE124 File Offset: 0x000CC324
		public void UpdateAtlasSizes()
		{
			this.AtlasSizes = new Point[]
			{
				new Point(this.MapModule.TextureAtlas.Width, this.MapModule.TextureAtlas.Height),
				new Point(this.EntityStoreModule.TextureAtlas.Width, this.EntityStoreModule.TextureAtlas.Height),
				new Point(this.App.CharacterPartStore.TextureAtlas.Width, this.App.CharacterPartStore.TextureAtlas.Height),
				new Point(this.FXModule.TextureAtlas.Width, this.FXModule.TextureAtlas.Height)
			};
		}

		// Token: 0x060042FF RID: 17151 RVA: 0x000CE1FC File Offset: 0x000CC3FC
		private void InitShaderTextures()
		{
			Image image = new Image(File.ReadAllBytes(Path.Combine(Paths.GameData, "ShaderTextures/WaterNormals.png")));
			this._waterNormals = new Texture(Texture.TextureTypes.Texture2D);
			this._waterNormals.CreateTexture2D(image.Width, image.Height, image.Pixels, 5, GL.LINEAR_MIPMAP_NEAREST, GL.LINEAR, GL.REPEAT, GL.REPEAT, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, true);
			Image image2 = new Image(File.ReadAllBytes(Path.Combine(Paths.GameData, "ShaderTextures/WaterCaustics.png")));
			this._waterCaustics = new Texture(Texture.TextureTypes.Texture2D);
			this._waterCaustics.CreateTexture2D(image2.Width, image2.Height, image2.Pixels, 5, GL.LINEAR, GL.LINEAR, GL.REPEAT, GL.REPEAT, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, true);
			Image image3 = new Image(File.ReadAllBytes(Path.Combine(Paths.GameData, "ShaderTextures/FlowMap.png")));
			this._flowMap = new Texture(Texture.TextureTypes.Texture2D);
			this._flowMap.CreateTexture2D(image3.Width, image3.Height, image3.Pixels, 5, GL.LINEAR, GL.LINEAR, GL.REPEAT, GL.REPEAT, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, true);
			Image image4 = new Image(File.ReadAllBytes(Path.Combine(Paths.GameData, "ShaderTextures/FogNoiseMap.png")));
			this._fogNoise = new Texture(Texture.TextureTypes.Texture2D);
			this._fogNoise.CreateTexture2D(image4.Width, image4.Height, image4.Pixels, 5, GL.LINEAR, GL.LINEAR, GL.REPEAT, GL.REPEAT, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, true);
			Image image5 = new Image(File.ReadAllBytes(Path.Combine(Paths.GameData, "ShaderTextures/GlowMask.png")));
			this._glowMask = new Texture(Texture.TextureTypes.Texture2D);
			this._glowMask.CreateTexture2D(image5.Width, image5.Height, image5.Pixels, 5, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
			byte[][] pixels = GameInstance.ReadCubemapImagesFromDisk(Path.Combine(Paths.GameData, "ShaderTextures/skybox"));
			this._cubemap = new Texture(Texture.TextureTypes.TextureCubemap);
			this._cubemap.CreateTextureCubemap(2048, 2048, pixels, 0, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
		}

		// Token: 0x06004300 RID: 17152 RVA: 0x000CE480 File Offset: 0x000CC680
		private void DisposeShaderTextures()
		{
			this._cubemap.Dispose();
			this._glowMask.Dispose();
			this._fogNoise.Dispose();
			this._flowMap.Dispose();
			this._waterCaustics.Dispose();
			this._waterNormals.Dispose();
		}

		// Token: 0x06004301 RID: 17153 RVA: 0x000CE4D8 File Offset: 0x000CC6D8
		private void InitDraw()
		{
			GLFunctions gl = this.Engine.Graphics.GL;
			int width = this.Engine.Window.Viewport.Width;
			int height = this.Engine.Window.Viewport.Height;
			this.Engine.AnimationSystem.SetTransferMethod(AnimationSystem.TransferMethod.ParallelInterleaved);
			this.SceneRenderer = new SceneRenderer(this.Engine.Graphics, this.Engine.Profiling, width, height);
			this._cameraSceneView = new SceneView();
			this._sunSceneView = new SceneView();
			this.SetFieldOfView((float)this.App.Settings.FieldOfView);
			this.InitTexureUnitsUsage();
			this.InitShaderTextures();
			this.InitPostEffects();
			this.InitGraphicsProfiling();
			this.InitDebugMapInfos();
			this.InitForceField();
			this.InitRenderSetup();
			this.InitFog();
			this.InitRenderingOptions();
			this.SetRenderingOptions(ref this.IngameMode);
			this.UseVolumetricSunshaft(false);
			this.PostEffectRenderer.UseBloomSunShaft(false);
			this.InitShadowMapping();
			this.InitLighting();
			this.InitOIT();
			this.CreateCubeMapMesh();
		}

		// Token: 0x06004302 RID: 17154 RVA: 0x000CE603 File Offset: 0x000CC803
		private void DisposeDraw()
		{
			this.ReleaseDebugMapInfos();
			this.DisposeForceField();
			this.DisposeShaderTextures();
			this.SceneRenderer.Dispose();
			this.PostEffectRenderer.Dispose();
			this.DisposeCubemapMesh();
		}

		// Token: 0x06004303 RID: 17155 RVA: 0x000CE63C File Offset: 0x000CC83C
		private void InitTexureUnitsUsage()
		{
			GPUProgramStore gpuprogramStore = this.Engine.Graphics.GPUProgramStore;
			ForceFieldProgram.TextureUnitLayout textureUnitLayout;
			textureUnitLayout.Texture = 9;
			textureUnitLayout.SceneDepth = 12;
			textureUnitLayout.OITTotalOpticalDepth = 17;
			textureUnitLayout.OITMoments = 18;
			gpuprogramStore.ForceFieldProgram.SetupTextureUnits(ref textureUnitLayout, true);
			gpuprogramStore.BuilderToolProgram.SetupTextureUnits(ref textureUnitLayout, true);
			ParticleProgram.TextureUnitLayout textureUnitLayout2;
			textureUnitLayout2.Atlas = 7;
			textureUnitLayout2.LinearFilteredAtlas = 8;
			textureUnitLayout2.UVMotion = 10;
			textureUnitLayout2.FXDataBuffer = 11;
			textureUnitLayout2.LightIndicesOrDataBuffer = 16;
			textureUnitLayout2.LightGrid = 15;
			textureUnitLayout2.ShadowMap = 14;
			textureUnitLayout2.FogNoise = 13;
			textureUnitLayout2.SceneDepth = 12;
			textureUnitLayout2.OITMoments = 18;
			textureUnitLayout2.OITTotalOpticalDepth = 17;
			gpuprogramStore.ParticleProgram.SetupTextureUnits(ref textureUnitLayout2, true);
			gpuprogramStore.ParticleErosionProgram.SetupTextureUnits(ref textureUnitLayout2, true);
			gpuprogramStore.ParticleDistortionProgram.SetupTextureUnits(ref textureUnitLayout2, true);
			MapChunkAlphaBlendedProgram.TextureUnitLayout textureUnitLayout3;
			textureUnitLayout3.Texture = 0;
			textureUnitLayout3.SceneDepth = 12;
			textureUnitLayout3.SceneDepthLowRes = 1;
			textureUnitLayout3.Normals = 2;
			textureUnitLayout3.Refraction = 3;
			textureUnitLayout3.SceneColor = 4;
			textureUnitLayout3.Caustics = 5;
			textureUnitLayout3.CloudShadow = 6;
			textureUnitLayout3.FogNoise = 13;
			textureUnitLayout3.ShadowMap = 14;
			textureUnitLayout3.LightIndicesOrDataBuffer = 16;
			textureUnitLayout3.LightGrid = 15;
			textureUnitLayout3.OITMoments = 18;
			textureUnitLayout3.OITTotalOpticalDepth = 17;
			gpuprogramStore.MapChunkAlphaBlendedProgram.SetupTextureUnits(ref textureUnitLayout3, true);
		}

		// Token: 0x06004304 RID: 17156 RVA: 0x000CE7B4 File Offset: 0x000CC9B4
		private void SetupChunkAlphaBlendedTextures(bool skipLightTextures, bool skipShadowMap, bool skipSceneDepth, bool skipFogNoise)
		{
			GLFunctions gl = this.Engine.Graphics.GL;
			RenderTargetStore rtstore = this.Engine.Graphics.RTStore;
			RenderTarget renderTarget = this.Engine.Graphics.IsGPULowEnd ? rtstore.LinearZHalfRes : rtstore.LinearZ;
			bool flag = this.WeatherModule.SkyRenderer.CloudsTextures.Length != 0;
			if (flag)
			{
				gl.ActiveTexture(GL.TEXTURE6);
				gl.BindTexture(GL.TEXTURE_2D, this.WeatherModule.SkyRenderer.CloudsTextures[0]);
			}
			gl.ActiveTexture(GL.TEXTURE5);
			gl.BindTexture(GL.TEXTURE_2D, this._waterCaustics.GLTexture);
			gl.ActiveTexture(GL.TEXTURE1);
			gl.BindTexture(GL.TEXTURE_2D, rtstore.LinearZHalfRes.GetTexture(RenderTarget.Target.Color0));
			gl.ActiveTexture(GL.TEXTURE2);
			gl.BindTexture(GL.TEXTURE_2D, this._waterNormals.GLTexture);
			gl.ActiveTexture(GL.TEXTURE3);
			gl.BindSampler(3U, this.Engine.Graphics.SamplerLinearMipmapLinearA);
			gl.BindTexture(GL.TEXTURE_2D, rtstore.SceneColorHalfRes.GetTexture(RenderTarget.Target.Color0));
			gl.ActiveTexture(GL.TEXTURE4);
			gl.BindSampler(4U, this.Engine.Graphics.SamplerLinearMipmapLinearB);
			gl.BindTexture(GL.TEXTURE_2D, rtstore.PreviousSceneColor.GetTexture(RenderTarget.Target.Color0));
			gl.ActiveTexture(GL.TEXTURE0);
			gl.BindTexture(GL.TEXTURE_2D, this.MapModule.TextureAtlas.GLTexture);
			bool flag2 = !skipLightTextures;
			if (flag2)
			{
				this.SceneRenderer.ClusteredLighting.SetupLightDataTextures(15U, 16U);
			}
			bool flag3 = !skipSceneDepth;
			if (flag3)
			{
				gl.ActiveTexture(GL.TEXTURE12);
				gl.BindTexture(GL.TEXTURE_2D, renderTarget.GetTexture(RenderTarget.Target.Color0));
			}
			bool flag4 = !skipFogNoise;
			if (flag4)
			{
				gl.ActiveTexture(GL.TEXTURE13);
				gl.BindTexture(GL.TEXTURE_2D, this._fogNoise.GLTexture);
			}
			bool flag5 = !skipShadowMap;
			if (flag5)
			{
				gl.ActiveTexture(GL.TEXTURE14);
				gl.BindTexture(GL.TEXTURE_2D, rtstore.ShadowMap.GetTexture(RenderTarget.Target.Depth));
			}
			gl.ActiveTexture(GL.TEXTURE0);
		}

		// Token: 0x06004305 RID: 17157 RVA: 0x000CEA10 File Offset: 0x000CCC10
		private void SetupVFXTextures(bool skipLightTextures, bool skipShadowMap, bool skipSceneDepth, bool skipFogNoise, bool skipForceFieldTextures)
		{
			GLFunctions gl = this.Engine.Graphics.GL;
			RenderTargetStore rtstore = this.Engine.Graphics.RTStore;
			RenderTarget renderTarget = this.Engine.Graphics.IsGPULowEnd ? rtstore.LinearZHalfRes : rtstore.LinearZ;
			bool flag = !skipLightTextures;
			if (flag)
			{
				this.SceneRenderer.ClusteredLighting.SetupLightDataTextures(15U, 16U);
			}
			this.Engine.FXSystem.SetupDrawDataTexture(11U);
			gl.ActiveTexture(GL.TEXTURE8);
			gl.BindSampler(8U, this.Engine.FXSystem.SmoothSampler);
			gl.BindTexture(GL.TEXTURE_2D, this.FXModule.TextureAtlas.GLTexture);
			gl.ActiveTexture(GL.TEXTURE7);
			gl.BindTexture(GL.TEXTURE_2D, this.FXModule.TextureAtlas.GLTexture);
			gl.ActiveTexture(GL.TEXTURE10);
			gl.BindTexture(GL.TEXTURE_2D_ARRAY, this.FXModule.UVMotionTextureArray2D);
			bool flag2 = !skipForceFieldTextures;
			if (flag2)
			{
				gl.ActiveTexture(GL.TEXTURE9);
				gl.BindTexture(GL.TEXTURE_2D, this.Engine.FXSystem.ForceFields.NormalMap);
			}
			bool flag3 = !skipSceneDepth;
			if (flag3)
			{
				gl.ActiveTexture(GL.TEXTURE12);
				gl.BindTexture(GL.TEXTURE_2D, renderTarget.GetTexture(RenderTarget.Target.Color0));
			}
			bool flag4 = !skipFogNoise;
			if (flag4)
			{
				gl.ActiveTexture(GL.TEXTURE13);
				gl.BindTexture(GL.TEXTURE_2D, this._fogNoise.GLTexture);
			}
			bool flag5 = !skipShadowMap;
			if (flag5)
			{
				gl.ActiveTexture(GL.TEXTURE14);
				gl.BindTexture(GL.TEXTURE_2D, rtstore.ShadowMap.GetTexture(RenderTarget.Target.Depth));
			}
			gl.ActiveTexture(GL.TEXTURE0);
		}

		// Token: 0x06004306 RID: 17158 RVA: 0x000CEBF0 File Offset: 0x000CCDF0
		private void InitDebugMapInfos()
		{
			RenderTargetStore rtstore = this.Engine.Graphics.RTStore;
			rtstore.RegisterDebugMap("atlas_map", this.MapModule.TextureAtlas);
			rtstore.RegisterDebugMap("atlas_entity", this.EntityStoreModule.TextureAtlas);
			rtstore.RegisterDebugMap("atlas_fx", this.FXModule.TextureAtlas);
			rtstore.RegisterDebugMap("water_normals", this._waterNormals);
			rtstore.RegisterDebugMap("water_caustics", this._waterCaustics);
			rtstore.RegisterDebugMap("flow", this._flowMap);
			this._debugTextureArrayLayerCount = this.FXModule.UVMotionTextureCount;
			rtstore.RegisterDebugMap2DArray("uvmotion", this.FXModule.UVMotionTextureArray2D, 64, 64, this._debugTextureArrayLayerCount);
			rtstore.RegisterDebugMapCubemap("cubemap", this._cubemap);
			this._activeDebugMapsNames = new string[]
			{
				"atlas_map"
			};
		}

		// Token: 0x06004307 RID: 17159 RVA: 0x000CECE4 File Offset: 0x000CCEE4
		private void ReleaseDebugMapInfos()
		{
			RenderTargetStore rtstore = this.Engine.Graphics.RTStore;
			rtstore.UnregisterDebugMap("atlas_map");
			rtstore.UnregisterDebugMap("atlas_entity");
			rtstore.UnregisterDebugMap("atlas_fx");
			rtstore.UnregisterDebugMap("water_normals");
			rtstore.UnregisterDebugMap("water_caustics");
			rtstore.UnregisterDebugMap("flow");
			rtstore.UnregisterDebugMap("uvmotion");
			rtstore.UnregisterDebugMap("cubemap");
		}

		// Token: 0x06004308 RID: 17160 RVA: 0x000CED64 File Offset: 0x000CCF64
		private void UpdateDebugDrawMap()
		{
			RenderTargetStore rtstore = this.Engine.Graphics.RTStore;
			bool flag = this._debugTextureArrayLayerCount != this.FXModule.UVMotionTextureCount;
			if (flag)
			{
				rtstore.UnregisterDebugMap("uvmotion");
				rtstore.RegisterDebugMap2DArray("uvmotion", this.FXModule.UVMotionTextureArray2D, 64, 64, this.FXModule.UVMotionTextureCount);
				this._debugTextureArrayLayerCount = this.FXModule.UVMotionTextureCount;
			}
		}

		// Token: 0x06004309 RID: 17161 RVA: 0x000CEDE4 File Offset: 0x000CCFE4
		public void ReloadShaderTextures()
		{
			this.ReleaseDebugMapInfos();
			this.DisposeShaderTextures();
			this.InitShaderTextures();
			this.InitDebugMapInfos();
			this.PostEffectRenderer.InitBloom(this.WeatherModule.SkyRenderer.SunTexture, this.WeatherModule.SkyRenderer.MoonTexture, this._glowMask.GLTexture, new Action(this.WeatherModule.SkyRenderer.DrawSun), new Action(this.WeatherModule.SkyRenderer.DrawMoon), true, true, false, false, true, true, 0, 0.3f, 8f, 0.25f, 0.3f, 4f);
		}

		// Token: 0x0600430A RID: 17162 RVA: 0x000CEE94 File Offset: 0x000CD094
		private void InitGraphicsProfiling()
		{
			this.Engine.Profiling.Initialize(84, 200);
			this.Engine.Profiling.CreateMeasure("Full-Frame", 0, false, false, false);
			this.Engine.Profiling.CreateMeasure("QueuedActions", 1, true, false, true);
			this.Engine.Profiling.CreateMeasure("OnNewFrame", 2, true, false, false);
			this.Engine.Profiling.CreateMeasure("-> Prepare-Chunks", 3, true, false, false);
			this.Engine.Profiling.CreateMeasure("-> Prepare-Entities", 4, true, false, false);
			this.Engine.Profiling.CreateMeasure("-> Modules.Tick", 5, true, false, false);
			this.Engine.Profiling.CreateMeasure("-> Modules.PreUpdate", 6, true, false, false);
			this.Engine.Profiling.CreateMeasure(" --> InteractionModule", 7, true, false, false);
			this.Engine.Profiling.CreateMeasure(" --> PrepareLights", 8, true, false, false);
			this.Engine.Profiling.CreateMeasure("-> GatherChunks", 9, true, false, false);
			this.Engine.Profiling.CreateMeasure("-> PrepareChunksForDraw", 10, true, false, false);
			this.Engine.Profiling.CreateMeasure("-> UpdateAnimation-Early", 11, true, false, false);
			this.Engine.Profiling.CreateMeasure("-> Occlusion-Setup", 12, false, false, false);
			this.Engine.Profiling.CreateMeasure(" --> BuildMap", 13, false, false, false);
			this.Engine.Profiling.CreateMeasure("   -> RenderOccluders", 14, false, false, false);
			this.Engine.Profiling.CreateMeasure("   -> Reproject", 15, false, false, false);
			this.Engine.Profiling.CreateMeasure("   -> CreateHiZ", 16, false, false, false);
			this.Engine.Profiling.CreateMeasure(" --> PrepareOccludees", 17, false, false, false);
			this.Engine.Profiling.CreateMeasure(" --> TestOccludees", 18, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> Modules.Update", 19, true, false, false);
			this.Engine.Profiling.CreateMeasure(" --> MapModule.Update", 20, true, false, false);
			this.Engine.Profiling.CreateMeasure(" --> EntityStoreModule.Update", 23, true, false, false);
			this.Engine.Profiling.CreateMeasure(" --> AmbienceUpdate", 21, true, false, false);
			this.Engine.Profiling.CreateMeasure(" --> WeatherUpdate", 22, true, false, false);
			this.Engine.Profiling.CreateMeasure("-> UpdateSceneData", 24, true, false, false);
			this.Engine.Profiling.CreateMeasure(" --> GatherChunksForShadowMap", 25, true, false, false);
			this.Engine.Profiling.CreateMeasure(" --> GatherAnimatedChunks", 26, true, false, false);
			this.Engine.Profiling.CreateMeasure(" --> GatherEntities", 27, true, false, false);
			this.Engine.Profiling.CreateMeasure(" --> UpdateFX", 28, true, false, false);
			this.Engine.Profiling.CreateMeasure(" --> GatherFX", 29, true, false, false);
			this.Engine.Profiling.CreateMeasure(" --> UpdateFXSimulation", 30, true, false, false);
			this.Engine.Profiling.CreateMeasure(" --> UpdateAnimation", 31, true, false, false);
			this.Engine.Profiling.CreateMeasure(" --> PrepareEntitiesForDraw", 32, true, false, false);
			this.Engine.Profiling.CreateMeasure(" --> PrepareShadowCascades", 33, true, false, false);
			this.Engine.Profiling.CreateMeasure(" --> UpdateLights", 34, true, false, false);
			this.Engine.Profiling.CreateMeasure("   -> Clear", 35, true, false, false);
			this.Engine.Profiling.CreateMeasure("   -> Clustering", 36, true, false, false);
			this.Engine.Profiling.CreateMeasure("   -> Refine", 37, true, false, false);
			this.Engine.Profiling.CreateMeasure("   -> FillGridData", 38, true, false, false);
			this.Engine.Profiling.CreateMeasure("   -> SendDataToGPU", 39, true, false, false);
			this.Engine.Profiling.CreateMeasure(" --> PrepareFXForDraw", 40, true, false, false);
			this.Engine.Profiling.CreateMeasure("   -> SendDataToGPU", 41, true, false, false);
			this.Engine.Profiling.CreateMeasure("Render", 42, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> Occlusion-FetchResults", 43, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> AnalyzeSunOcclusion", 44, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> Reflection-BuildMips", 45, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> ShadowMap-Build", 46, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> FirstPerson", 47, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> World-Near", 48, false, false, false);
			this.Engine.Profiling.CreateMeasure(" --> World-Animated", 49, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> World-Far", 50, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> Entities", 51, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> LinearZ", 52, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> LinearZDownsample", 53, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> ZDownsample", 54, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> Edge-Detection", 55, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> DeferredShadow", 56, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> SSAO", 57, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> Blur(AO,DefShadow)", 58, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> Volumetric sunshafts", 59, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> Lights", 60, false, true, false);
			this.Engine.Profiling.CreateMeasure(" --> Stencil", 61, false, false, false);
			this.Engine.Profiling.CreateMeasure(" --> Full-Res", 62, false, false, false);
			this.Engine.Profiling.CreateMeasure(" --> Low-Res", 63, false, false, false);
			this.Engine.Profiling.CreateMeasure(" --> Mix", 64, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> ApplyDeferred", 65, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> Particles-Opaque", 66, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> Weather", 67, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> World-AlphaBlended", 68, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> Transparency", 69, false, false, false);
			this.Engine.Profiling.CreateMeasure(" --> OIT-Prepass", 70, false, false, false);
			this.Engine.Profiling.CreateMeasure(" --> OIT-Accumulate-Quarter-Res", 71, false, false, false);
			this.Engine.Profiling.CreateMeasure(" --> OIT-Accumulate-Half-Res", 72, false, false, false);
			this.Engine.Profiling.CreateMeasure(" --> OIT-Accumulate-Full-Res", 73, false, false, false);
			this.Engine.Profiling.CreateMeasure(" --> OIT-Composite", 74, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> Texts", 75, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> Distortion", 76, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> PostFX", 77, false, false, false);
			this.Engine.Profiling.CreateMeasure(" ---> DepthOfField", 78, false, false, false);
			this.Engine.Profiling.CreateMeasure(" ---> Bloom", 79, false, false, false);
			this.Engine.Profiling.CreateMeasure(" ---> Combine + FXAA", 80, false, false, false);
			this.Engine.Profiling.CreateMeasure(" ---> TAA", 81, false, false, false);
			this.Engine.Profiling.CreateMeasure(" ---> Blur", 82, false, false, false);
			this.Engine.Profiling.CreateMeasure("-> ScreenFX", 83, false, false, false);
			this.ProfilingModule.SetupDetailedMeasures();
			this.SceneRenderer.SetupRenderingProfiles(60, 62, 63, 61, 64, 52, 53, 54, 55);
			this.SceneRenderer.SetupClusteredLightingRenderingProfiles(35, 36, 37, 38, 39);
			this.Engine.OcclusionCulling.SetupRenderingProfiles(13, 14, 15, 16, 17, 18, 43);
			this.Engine.FXSystem.SetupRenderingProfile(41);
			this.PostEffectRenderer.SetupRenderingProfiles(78, 79, 80, 81, 82);
		}

		// Token: 0x0600430B RID: 17163 RVA: 0x000CF815 File Offset: 0x000CDA15
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetFieldOfView(float fieldOfView)
		{
			this.ActiveFieldOfView = fieldOfView;
			this.SceneRenderer.ComputeNearChunkDistance(this.ActiveFieldOfView);
		}

		// Token: 0x0600430C RID: 17164 RVA: 0x000CF832 File Offset: 0x000CDA32
		public void SetOcclusionCulling(bool enable)
		{
			this.Engine.OcclusionCulling.IsEnabled = enable;
			this._requestedOpaqueChunkOccludersCount = 15;
		}

		// Token: 0x0600430D RID: 17165 RVA: 0x000CF84E File Offset: 0x000CDA4E
		public void SetOpaqueOccludersCount(int count)
		{
			this._requestedOpaqueChunkOccludersCount = count;
		}

		// Token: 0x0600430E RID: 17166 RVA: 0x000CF857 File Offset: 0x000CDA57
		public void UseOcclusionCullingReprojection(bool enable)
		{
			this._useOcclusionCullingReprojection = enable;
		}

		// Token: 0x0600430F RID: 17167 RVA: 0x000CF860 File Offset: 0x000CDA60
		public void UseOcclusionCullingReprojectionHoleFilling(bool enable)
		{
			this._useOcclusionCullingReprojectionHoleFilling = enable;
		}

		// Token: 0x06004310 RID: 17168 RVA: 0x000CF869 File Offset: 0x000CDA69
		public void UseChunkOccluderPlanes(bool enable)
		{
			this._useChunkOccluderPlanes = enable;
		}

		// Token: 0x06004311 RID: 17169 RVA: 0x000CF872 File Offset: 0x000CDA72
		public void UseOpaqueChunkOccluders(bool enable)
		{
			this._useOpaqueChunkOccluders = enable;
		}

		// Token: 0x06004312 RID: 17170 RVA: 0x000CF87C File Offset: 0x000CDA7C
		public void UseAlphaTestedChunkOccluders(bool enable)
		{
			bool flag = this._useAlphaTestedChunkOccluders != enable;
			if (flag)
			{
				this._useAlphaTestedChunkOccluders = enable;
				this.Engine.Graphics.GPUProgramStore.ZOnlyMapChunkProgram.AlphaTest = this._useAlphaTestedChunkOccluders;
				this.Engine.Graphics.GPUProgramStore.ZOnlyMapChunkProgram.Reset(true);
			}
		}

		// Token: 0x06004313 RID: 17171 RVA: 0x000CF8E0 File Offset: 0x000CDAE0
		public void DrawOccluders()
		{
			GLFunctions gl = this.Engine.Graphics.GL;
			bool useLocalPlayerOccluder = this.UseLocalPlayerOccluder;
			if (useLocalPlayerOccluder)
			{
				this.SceneRenderer.SetupEntityShadowMapDataTexture(7U);
				this.SceneRenderer.SetupModelVFXDataTexture(6U);
				gl.ActiveTexture(GL.TEXTURE4);
				gl.BindTexture(GL.TEXTURE_2D, this._fogNoise.GLTexture);
				gl.ActiveTexture(GL.TEXTURE2);
				gl.BindTexture(GL.TEXTURE_2D, this.App.CharacterPartStore.TextureAtlas.GLTexture);
				gl.ActiveTexture(GL.TEXTURE1);
				gl.BindTexture(GL.TEXTURE_2D, this.EntityStoreModule.TextureAtlas.GLTexture);
				gl.ActiveTexture(GL.TEXTURE0);
				gl.BindTexture(GL.TEXTURE_2D, this.MapModule.TextureAtlas.GLTexture);
				this.LocalPlayer.DrawOccluders(this._cameraSceneView);
			}
			this.SceneRenderer.DrawOccluders();
		}

		// Token: 0x06004314 RID: 17172 RVA: 0x000CF9E8 File Offset: 0x000CDBE8
		private void UpdateOcclusionCulling()
		{
			this.SceneRenderer.PrepareOcclusionCulling(this._requestedOpaqueChunkOccludersCount, this._useChunkOccluderPlanes, this._useOpaqueChunkOccluders, this._useAlphaTestedChunkOccluders, 0, this.MapModule.TextureAtlas.GLTexture);
			int candidateOccludeesCount;
			ref OcclusionCulling.OccludeeData[] occludeesData = ref this.SceneRenderer.GetOccludeesData(out candidateOccludeesCount);
			ref int[] visibleOccludees = ref this.SceneRenderer.VisibleOccludees;
			Action drawOccluders = new Action(this.DrawOccluders);
			RenderTarget previousZBuffer = this._useOcclusionCullingReprojection ? this.Engine.Graphics.RTStore.LinearZHalfRes : null;
			RenderTarget.Target previousZBufferTarget = RenderTarget.Target.Color0;
			int val;
			Vector3[] array;
			this.MapModule.GetBlocksRemovedThisFrame(this.SceneRenderer.PreviousData.CameraPosition, this.SceneRenderer.Data.CameraPosition, this.SceneRenderer.Data.RelativeViewFrustum, 16f, out val, out array);
			int maxInvalidScreenAreasForReprojection = this.Engine.OcclusionCulling.MaxInvalidScreenAreasForReprojection;
			int num = Math.Min(maxInvalidScreenAreasForReprojection, val);
			Vector3 one = Vector3.One;
			for (int i = 0; i < num; i++)
			{
				MathHelper.ComputeScreenArea(array[i], one, ref this.SceneRenderer.PreviousData.ViewRotationProjectionMatrix, out this._previousFrameInvalidScreenAreas[i]);
			}
			Vector4[] previousFrameInvalidScreenAreas = (num == 0) ? null : this._previousFrameInvalidScreenAreas;
			this.Engine.OcclusionCulling.Update(ref this.SceneRenderer.Data.ViewRotationProjectionMatrix, this.FrameTime, this.SceneRenderer.IsSpatialContinuityLost(), drawOccluders, ref this.SceneRenderer.Data.ReprojectFromPreviousViewToCurrentProjection, ref this.SceneRenderer.PreviousData.ProjectionMatrix, previousZBuffer, previousZBufferTarget, previousFrameInvalidScreenAreas, num, this._useOcclusionCullingReprojectionHoleFilling, ref occludeesData, candidateOccludeesCount, ref visibleOccludees);
		}

		// Token: 0x06004315 RID: 17173 RVA: 0x000CFB9C File Offset: 0x000CDD9C
		public void Resize(int width, int height)
		{
			this.Engine.Graphics.RTStore.Resize(width, height, this.ResolutionScale);
			this.ProfilingModule.Resize(width, height);
			int width2 = (int)((float)width * this.ResolutionScale);
			int height2 = (int)((float)height * this.ResolutionScale);
			this.SceneRenderer.Resize(width2, height2);
			this.PostEffectRenderer.Resize(width, height, this.ResolutionScale);
			this.InterfaceRenderPreviewModule.Resize(width, height);
			this.WorldMapModule.Resize(width, height);
			this.DamageEffectModule.Resize(width, height);
			this.EditorWebViewModule.OnWindowSizeChanged();
		}

		// Token: 0x06004316 RID: 17174 RVA: 0x000CFC44 File Offset: 0x000CDE44
		private void UpdateDynamicLights()
		{
			this.GlobalLightDataCount = 0;
			int entityLightCount = this.EntityStoreModule.EntityLightCount;
			bool useOcclusionCullingForLights = this.Engine.OcclusionCulling.IsActive && this.UseOcclusionCullingForLights;
			this.EntityStoreModule.GatherLights(ref this.SceneRenderer.Data.ViewFrustum, useOcclusionCullingForLights, 1024, ref this.GlobalLightData, out this.GlobalLightDataCount);
			this.SceneRenderer.PrepareLights(this.GlobalLightData, this.GlobalLightDataCount);
		}

		// Token: 0x06004317 RID: 17175 RVA: 0x000CFCC8 File Offset: 0x000CDEC8
		private void UpdateAtmosphericData()
		{
			this._isCameraUnderwater = this.WeatherModule.IsUnderWater;
			float num = this.SceneRenderer.Data.Time * 0.035f;
			float num2 = -this.WeatherModule.SkyRenderer.CloudOffsets[0] * this._cloudsShadowsSpeed;
			float cloudsShadowIntensity = MathHelper.Lerp(0f, this._cloudsShadowsIntensity, this.WeatherModule.CloudsTransitionOpacity);
			this._projectionTexture = (this._isCameraUnderwater ? this._waterCaustics.GLTexture : this.WeatherModule.SkyRenderer.CloudsTextures[0]);
			Vector4 sunLightColor = new Vector4(this.WeatherModule.SunlightColor.X, this.WeatherModule.SunlightColor.Y, this.WeatherModule.SunlightColor.Z, this.WeatherModule.SunLight);
			float num3 = 1f - Math.Abs(this.WeatherModule.NormalizedSunPosition.Y);
			num3 = (float)Math.Pow((double)num3, 2.5);
			Vector3 vector = this._isCameraUnderwater ? this.WeatherModule.FogColor : new Vector3(this.WeatherModule.SkyTopGradientColor.X, this.WeatherModule.SkyTopGradientColor.Y, this.WeatherModule.SkyTopGradientColor.Z);
			Vector3 vector2 = Vector3.Lerp(this.WeatherModule.FogColor, new Vector3(this.WeatherModule.SunsetColor.X, this.WeatherModule.SunsetColor.Y, this.WeatherModule.SunsetColor.Z), num3);
			Vector3 fogColor = this.WeatherModule.FogColor;
			float y = (this.WeatherModule.ActiveFogMode == WeatherModule.FogMode.Off) ? 0f : this.WeatherModule.LerpFogEnd;
			float w = (this.WeatherModule.ActiveFogMode == WeatherModule.FogMode.Off || !this._isCameraUnderwater) ? 0f : this.WeatherModule.FogDepthFalloff;
			float z = (this.WeatherModule.ActiveFogMode == WeatherModule.FogMode.Off || !this._isCameraUnderwater) ? 0f : this.WeatherModule.FogDepthStart;
			Vector4 fogParams = new Vector4(this.WeatherModule.LerpFogStart, y, z, w);
			bool useCustomMoodFog = this._useCustomMoodFog;
			float y2;
			float num8;
			if (useCustomMoodFog)
			{
				float num4 = this._customHeightFalloff - 1.5f;
				float num5 = this._customDensity * num4;
				float num6 = (float)Math.Exp((double)num5) - 1f;
				float num7 = (float)Math.Exp((double)this.SceneRenderer.Data.FogDensityUnderwater) - 1f;
				y2 = (this._isCameraUnderwater ? num7 : num6);
				num8 = (this._isCameraUnderwater ? (this.SceneRenderer.Data.FogHeightFalloffUnderwater * 0.01f) : (this._customHeightFalloff * 0.01f));
			}
			else
			{
				float num9 = this.WeatherModule.FogHeightFalloff - 1.5f;
				float num10 = this.WeatherModule.FogDensity * num9;
				num8 = (this._isCameraUnderwater ? (this.SceneRenderer.Data.FogHeightFalloffUnderwater * 0.01f) : (this.WeatherModule.FogHeightFalloff * 0.01f));
				float num11 = (float)Math.Exp((double)num10) - 1f;
				y2 = ((!this._isCameraUnderwater && this._useMoodFog) ? num11 : 0f);
			}
			float num12 = this._isCameraUnderwater ? num : num2;
			float num13 = (num12 == 0f) ? (this.SceneRenderer.Data.Time * 0.01f) : num12;
			num13 *= this._fogSpeedFactor;
			Vector4 fogMoodParams = new Vector4(num8, y2, num13, this._densityVariationScale);
			float fogHeightDensityAtViewer = (float)Math.Exp((double)(-(double)num8 * this.SceneRenderer.Data.CameraPosition.Y));
			Vector3 ambientFrontColor = Vector3.Lerp(vector, vector2, num3);
			Vector3 ambientBackColor = Vector3.Lerp(vector, fogColor, num3);
			float amount = 1f;
			bool useLessSkyAmbientAtNoon = this.UseLessSkyAmbientAtNoon;
			if (useLessSkyAmbientAtNoon)
			{
				amount = ((this.WeatherModule.NormalizedSunPosition.Y < 0f) ? 1f : num3);
			}
			float ambientIntensity = MathHelper.Lerp(this.SkyAmbientIntensityAtNoon, this.SkyAmbientIntensity, amount);
			bool isUnderWater = this.WeatherModule.IsUnderWater;
			if (isUnderWater)
			{
				vector2 = fogColor;
			}
			this.SceneRenderer.UpdateAtmosphericData(this._isCameraUnderwater, this.WeatherModule.SunColor, sunLightColor, this.WeatherModule.NormalizedSunPosition, vector, vector2, fogColor, fogParams, fogMoodParams, fogHeightDensityAtViewer, ambientFrontColor, ambientBackColor, ambientIntensity, num, this._underwaterCausticsDistortion, this._underwaterCausticsScale, this._underwaterCausticsIntensity, num2, this._cloudsShadowsBlurriness, this._cloudsShadowsScale, cloudsShadowIntensity);
		}

		// Token: 0x06004318 RID: 17176 RVA: 0x000D0180 File Offset: 0x000CE380
		private void UpdateRenderData()
		{
			this.SceneRenderer.UpdateProjectionMatrix(this.ActiveFieldOfView, this.Engine.Window.AspectRatio, this.PostEffectRenderer.NeedsJittering);
			this.SceneRenderer.UpdateRenderData(this.CameraModule.Controller.Rotation, this.CameraModule.Controller.Position, this.LocalPlayer.RenderPosition, this.FrameCounter, this.RenderTimePaused ? 0f : this.FrameTime, this.DeltaTime);
			this._cameraSceneView.Frustum = this.SceneRenderer.Data.ViewFrustum;
			this._cameraSceneView.Position = this.SceneRenderer.Data.CameraPosition;
			this._cameraSceneView.Direction = this.SceneRenderer.Data.CameraDirection;
			bool isSunShadowMappingEnabled = this.SceneRenderer.IsSunShadowMappingEnabled;
			if (isSunShadowMappingEnabled)
			{
				this.SceneRenderer.UpdateSunShadowRenderData();
				this._sunSceneView.Position = this.SceneRenderer.Data.SunShadowRenderData.VirtualSunPosition;
				this._sunSceneView.Direction = this.SceneRenderer.Data.SunShadowRenderData.VirtualSunDirection;
				this._sunSceneView.Frustum = this.SceneRenderer.Data.SunShadowRenderData.VirtualSunViewFrustum;
				this._sunSceneView.KDopFrustum = this.SceneRenderer.Data.SunShadowRenderData.VirtualSunKDopFrustum;
				this._sunSceneView.UseKDopForCulling = this.SceneRenderer.UseSunShadowsGlobalKDop;
			}
		}

		// Token: 0x06004319 RID: 17177 RVA: 0x000D031C File Offset: 0x000CE51C
		private void UpdateSceneData()
		{
			bool isSunShadowMappingEnabled = this.SceneRenderer.IsSunShadowMappingEnabled;
			bool isWorldShadowEnabled = this.SceneRenderer.IsWorldShadowEnabled;
			this.UpdateAtmosphericData();
			bool flag = isSunShadowMappingEnabled;
			if (flag)
			{
				int height = ChunkHelper.Height;
				this.SceneRenderer.SetSunShadowsMaxWorldHeight((float)height);
				bool flag2 = isWorldShadowEnabled;
				if (flag2)
				{
					this.Engine.Profiling.StartMeasure(25);
					this.MapModule.ProcessFrustumCulling(this._sunSceneView);
					this.MapModule.GatherRenderableChunksForShadowMap(this._sunSceneView, this.CullUndergroundChunkShadowCasters, 2000);
					this._sunSceneView.SortChunksByDistance();
					this.MapModule.PrepareForSunShadowMapDraw(this._sunSceneView, this.SceneRenderer.Data.CameraPosition);
					this.Engine.Profiling.StopMeasure(25);
				}
				else
				{
					this.Engine.Profiling.SkipMeasure(25);
				}
			}
			this.Engine.Profiling.StartMeasure(27);
			SceneView sunSceneView = isSunShadowMappingEnabled ? this._sunSceneView : null;
			this.EntityStoreModule.ProcessFrustumCulling(this._cameraSceneView, sunSceneView);
			this.EntityStoreModule.GatherRenderableEntities(this._cameraSceneView, sunSceneView, this.SceneRenderer.Data.SunShadowRenderData.VirtualSunDirection, this.UseAnimationLOD, this.CullUndergroundEntityShadowCasters, this.CullSmallEntityShadowCasters);
			this._cameraSceneView.SortEntitiesByDistance();
			bool flag3 = isSunShadowMappingEnabled;
			if (flag3)
			{
				this._sunSceneView.SortEntitiesByDistance();
			}
			bool isFirstPerson = this.CameraModule.Controller.IsFirstPerson;
			Vector3 renderPosition = this.LocalPlayer.RenderPosition;
			this.EntityStoreModule.ExtractClosestEntityPositions(this._cameraSceneView, isFirstPerson, renderPosition);
			this.Engine.Profiling.StopMeasure(27);
			this.Engine.Profiling.StartMeasure(26);
			this.MapModule.GatherRenderableAnimatedBlocks(this._cameraSceneView, sunSceneView, true);
			this.Engine.Profiling.StopMeasure(26);
			this._foliageInteractionParams = new Vector3(1.5f, 4f, 0.33f);
			this.Engine.Profiling.StartMeasure(34);
			this.UpdateDynamicLights();
			this.Engine.Profiling.StopMeasure(34);
			this.Engine.Profiling.StartMeasure(31);
			this.Engine.AnimationSystem.ProcessAnimationTasks();
			this.Engine.Profiling.StopMeasure(31);
			this.Engine.Profiling.StartMeasure(32);
			this.EntityStoreModule.PrepareForDraw(this._cameraSceneView, ref this.SceneRenderer.Data.ViewMatrix, ref this.SceneRenderer.Data.ProjectionMatrix, ref this.SceneRenderer.Data.ViewProjectionMatrix);
			this.SceneRenderer.SendEntityDataToGPU();
			this.SceneRenderer.SendModelVFXDataToGPU();
			bool flag4 = isSunShadowMappingEnabled;
			if (flag4)
			{
				this.EntityStoreModule.PrepareForShadowMapDraw(this._sunSceneView);
			}
			this._atlasSizeFactor0 = new Vector2((float)this.MapModule.TextureAtlas.Width / 2048f, (float)this.MapModule.TextureAtlas.Height / 2048f);
			this._atlasSizeFactor1 = new Vector2((float)this.EntityStoreModule.TextureAtlas.Width / 2048f, (float)this.EntityStoreModule.TextureAtlas.Height / 2048f);
			this._atlasSizeFactor2 = new Vector2((float)this.App.CharacterPartStore.TextureAtlas.Width / 2048f, (float)this.App.CharacterPartStore.TextureAtlas.Height / 2048f);
			this.Engine.Profiling.StopMeasure(32);
			bool flag5 = isSunShadowMappingEnabled;
			if (flag5)
			{
				this.Engine.Profiling.StartMeasure(33);
				this.SceneRenderer.PrepareShadowCastersForDraw();
				this.SceneRenderer.SendEntityShadowMapDataToGPU();
				this.Engine.Profiling.StopMeasure(33);
			}
			else
			{
				this.Engine.Profiling.SkipMeasure(33);
			}
			bool flag6 = this.LocalPlayer.FirstPersonViewNeedsDrawing();
			if (flag6)
			{
				bool flag7 = !this.Engine.OcclusionCulling.IsEnabled || !this.UseLocalPlayerOccluder;
				if (flag7)
				{
					this.LocalPlayer.PrepareForDrawInFirstPersonView();
				}
				this.LocalPlayer.UpdateFirstPersonFX();
			}
			this.Engine.Profiling.StartMeasure(28);
			this.EntityStoreModule.ProcessFXUpdateTasks();
			this.LocalPlayer.PrepareFXForViewSwitch();
			this.ParticleSystemStoreModule.Update(this.SceneRenderer.Data.CameraPosition);
			this.TrailStoreModule.Update(this.SceneRenderer.Data.CameraPosition);
			this.Engine.Profiling.StopMeasure(28);
			this.Engine.Profiling.StartMeasure(29);
			this.ParticleSystemStoreModule.GatherRenderableSpawners(this.SceneRenderer.Data.CameraPosition, this.SceneRenderer.Data.ViewFrustum);
			this.Engine.Profiling.StopMeasure(29);
			this.Engine.Profiling.StartMeasure(30);
			this.Engine.FXSystem.Particles.UpdateSimulation(this.DeltaTime);
			this.Engine.FXSystem.Trails.UpdateSimulation(this.DeltaTime);
			this.Engine.Profiling.StopMeasure(30);
			bool debugInfoNeedsDrawing = this.EntityStoreModule.DebugInfoNeedsDrawing;
			if (debugInfoNeedsDrawing)
			{
				this.EntityStoreModule.PrepareDebugInfoForDraw(this._cameraSceneView, ref this.SceneRenderer.Data.ViewProjectionMatrix);
			}
			Vector3 horizonPosition = this.WeatherModule.FluidHorizonPosition - this.SceneRenderer.Data.CameraPosition;
			this.WeatherModule.SkyRenderer.PrepareSkyForDraw(ref this.SceneRenderer.Data.ViewRotationProjectionMatrix);
			this.WeatherModule.SkyRenderer.PrepareCloudsForDraw(ref this.SceneRenderer.Data.ViewRotationProjectionMatrix, ref this.WeatherModule.SkyRotation);
			this.WeatherModule.SkyRenderer.PrepareHorizonForDraw(ref this.SceneRenderer.Data.ViewRotationProjectionMatrix, horizonPosition, this.WeatherModule.FluidHorizonScale);
			bool flag8 = this.WeatherModule.SkyRenderer.SunNeedsDrawing(this.SceneRenderer.Data.SunPositionWS, this.SceneRenderer.Data.CameraDirection, this.WeatherModule.SunScale);
			if (flag8)
			{
				this.WeatherModule.SkyRenderer.PrepareSunForDraw(ref this.SceneRenderer.Data.ViewRotationMatrix, ref this.SceneRenderer.Data.ProjectionMatrix, this.SceneRenderer.Data.SunPositionWS, this.WeatherModule.SunScale);
			}
			bool flag9 = this.WeatherModule.SkyRenderer.MoonNeedsDrawing(this.SceneRenderer.Data.SunPositionWS, this.SceneRenderer.Data.CameraDirection, this.WeatherModule.MoonScale);
			if (flag9)
			{
				this.WeatherModule.SkyRenderer.PrepareMoonForDraw(ref this.SceneRenderer.Data.ViewRotationMatrix, ref this.SceneRenderer.Data.ProjectionMatrix, this.SceneRenderer.Data.SunPositionWS, this.WeatherModule.MoonScale);
			}
			this.BuilderToolsModule.SelectionArea.Update();
			bool flag10 = this.ImmersiveScreenModule.NeedsDrawing();
			if (flag10)
			{
				this.ImmersiveScreenModule.PrepareForDraw(ref this.SceneRenderer.Data.ViewProjectionMatrix);
			}
			this.Engine.Profiling.StartMeasure(40);
			this.Engine.FXSystem.PrepareForDraw(this.SceneRenderer.Data.CameraPosition);
			this.UpdateForceField();
			this.Engine.Profiling.StopMeasure(40);
			bool mapNeedsDrawing = this.WorldMapModule.MapNeedsDrawing;
			if (mapNeedsDrawing)
			{
				this.WorldMapModule.PrepareMapForDraw();
			}
			this.DamageEffectModule.PrepareForDraw();
			bool isVisible = this.ProfilingModule.IsVisible;
			if (isVisible)
			{
				this.ProfilingModule.PrepareForDraw();
			}
		}

		// Token: 0x0600431A RID: 17178 RVA: 0x000D0B80 File Offset: 0x000CED80
		public void DrawScene()
		{
			RenderTargetStore rtstore = this.Engine.Graphics.RTStore;
			bool isSunShadowMappingEnabled = this.SceneRenderer.IsSunShadowMappingEnabled;
			bool isWorldShadowEnabled = this.SceneRenderer.IsWorldShadowEnabled;
			this.SceneRenderer.BeginDraw();
			GLFunctions gl = this.Engine.Graphics.GL;
			gl.Enable(GL.CULL_FACE);
			gl.Disable(GL.BLEND);
			bool useMoodFog = this._useMoodFog;
			if (useMoodFog)
			{
				this.Engine.Profiling.StartMeasure(44);
				this.SceneRenderer.AnalyzeSunOcclusion();
				this.Engine.Profiling.StopMeasure(44);
			}
			else
			{
				this.Engine.Profiling.SkipMeasure(44);
			}
			this.SceneRenderer.SendSceneDataToGPU();
			this.Engine.Profiling.StartMeasure(45);
			this.SceneRenderer.BuildReflectionMips();
			this.Engine.Profiling.StopMeasure(45);
			gl.Enable(GL.CULL_FACE);
			gl.Enable(GL.DEPTH_TEST);
			gl.DepthFunc(GL.LEQUAL);
			gl.DepthMask(true);
			this.SceneRenderer.SetupEntityShadowMapDataTexture(7U);
			this.SceneRenderer.SetupModelVFXDataTexture(6U);
			this.SceneRenderer.SetupEntityDataTexture(5U);
			gl.ActiveTexture(GL.TEXTURE4);
			gl.BindTexture(GL.TEXTURE_2D, this._fogNoise.GLTexture);
			gl.ActiveTexture(GL.TEXTURE3);
			gl.BindTexture(GL.TEXTURE_2D, this.App.CharacterPartStore.CharacterGradientAtlas.GLTexture);
			gl.ActiveTexture(GL.TEXTURE2);
			gl.BindTexture(GL.TEXTURE_2D, this.App.CharacterPartStore.TextureAtlas.GLTexture);
			gl.ActiveTexture(GL.TEXTURE1);
			gl.BindTexture(GL.TEXTURE_2D, this.EntityStoreModule.TextureAtlas.GLTexture);
			gl.ActiveTexture(GL.TEXTURE0);
			gl.BindTexture(GL.TEXTURE_2D, this.MapModule.TextureAtlas.GLTexture);
			bool flag = this._renderPassStates[0] && isSunShadowMappingEnabled;
			if (flag)
			{
				this.Engine.Profiling.StartMeasure(46);
				this.SceneRenderer.BuildShadowMap();
				this.Engine.Profiling.StopMeasure(46);
			}
			else
			{
				this.Engine.Profiling.SkipMeasure(46);
			}
			gl.Disable(GL.CULL_FACE);
			rtstore.GBuffer.Bind(true, true);
			float w = this.SceneRenderer.Data.SunLightColor.W;
			float num = 0f;
			float[] data = new float[]
			{
				0f,
				0f,
				0f,
				1f
			};
			float[] data2 = new float[]
			{
				w,
				w,
				1f,
				num
			};
			gl.ClearBufferfv(GL.COLOR, 0, data);
			gl.ClearBufferfv(GL.COLOR, 1, data2);
			this.BeginWireframeMode(GameInstance.WireframePass.OnAll);
			this.Engine.Graphics.GPUProgramStore.MapChunkNearOpaqueProgram.SceneDataBlock.SetBuffer(this.SceneRenderer.SceneDataBuffer);
			this.Engine.Graphics.GPUProgramStore.MapChunkNearAlphaTestedProgram.SceneDataBlock.SetBuffer(this.SceneRenderer.SceneDataBuffer);
			this.Engine.Graphics.GPUProgramStore.MapChunkFarOpaqueProgram.SceneDataBlock.SetBuffer(this.SceneRenderer.SceneDataBuffer);
			this.Engine.Graphics.GPUProgramStore.MapChunkFarAlphaTestedProgram.SceneDataBlock.SetBuffer(this.SceneRenderer.SceneDataBuffer);
			this.Engine.Graphics.GPUProgramStore.MapChunkAlphaBlendedProgram.SceneDataBlock.SetBuffer(this.SceneRenderer.SceneDataBuffer);
			this.Engine.Graphics.GPUProgramStore.MapBlockAnimatedProgram.SceneDataBlock.SetBuffer(this.SceneRenderer.SceneDataBuffer);
			this.Engine.Graphics.GPUProgramStore.BlockyModelProgram.SceneDataBlock.SetBuffer(this.SceneRenderer.SceneDataBuffer);
			this.Engine.Graphics.GPUProgramStore.FirstPersonBlockyModelProgram.SceneDataBlock.SetBuffer(this.SceneRenderer.SceneDataBuffer);
			this.Engine.Graphics.GPUProgramStore.FirstPersonClippingBlockyModelProgram.SceneDataBlock.SetBuffer(this.SceneRenderer.SceneDataBuffer);
			this.Engine.Graphics.GPUProgramStore.BlockyModelDistortionProgram.SceneDataBlock.SetBuffer(this.SceneRenderer.SceneDataBuffer);
			this.Engine.Graphics.GPUProgramStore.BlockyModelDitheringProgram.SceneDataBlock.SetBuffer(this.SceneRenderer.SceneDataBuffer);
			this.Engine.Graphics.GPUProgramStore.VolumetricSunshaftProgram.SceneDataBlock.SetBuffer(this.SceneRenderer.SceneDataBuffer);
			BlockyModelProgram blockyModelProgram = this.Engine.Graphics.GPUProgramStore.BlockyModelProgram;
			this.Engine.Profiling.StartMeasure(47);
			bool flag2 = !this.LocalPlayer.IsFirstPersonClipping();
			if (flag2)
			{
				gl.Enable(GL.STENCIL_TEST);
			}
			bool flag3 = this._renderPassStates[1] && this.LocalPlayer.FirstPersonViewNeedsDrawing();
			if (flag3)
			{
				this.BeginWireframeMode(GameInstance.WireframePass.OnEntities);
				gl.StencilFunc(GL.ALWAYS, 0, 255U);
				gl.StencilOp(GL.KEEP, GL.KEEP, GL.REPLACE);
				gl.StencilMask(255U);
				gl.ColorMask(false, false, false, false);
				gl.DepthMask(false);
				BlockyModelProgram blockyModelProgram2 = this.LocalPlayer.IsFirstPersonClipping() ? this.Engine.Graphics.GPUProgramStore.FirstPersonClippingBlockyModelProgram : this.Engine.Graphics.GPUProgramStore.FirstPersonBlockyModelProgram;
				gl.UseProgram(blockyModelProgram2);
				blockyModelProgram2.ViewProjectionMatrix.SetValue(ref this.SceneRenderer.Data.FirstPersonProjectionMatrix);
				this.LocalPlayer.SendFirstPersonViewUniforms(this._atlasSizeFactor0, this._atlasSizeFactor1, this._atlasSizeFactor2);
				this.LocalPlayer.DrawInFirstPersonView();
				gl.ColorMask(true, true, true, true);
				gl.DepthMask(true);
				gl.StencilMask(0U);
				gl.StencilFunc(GL.EQUAL, 0, 255U);
				this.LocalPlayer.DrawInFirstPersonView();
				this.EndWireframeMode(GameInstance.WireframePass.OnEntities);
			}
			else
			{
				gl.DepthMask(true);
				gl.StencilMask(0U);
			}
			this.Engine.Profiling.StopMeasure(47);
			gl.StencilFunc(GL.NOTEQUAL, 0, 255U);
			MapChunkNearProgram mapChunkNearOpaqueProgram = this.Engine.Graphics.GPUProgramStore.MapChunkNearOpaqueProgram;
			gl.UseProgram(mapChunkNearOpaqueProgram);
			this.Engine.Profiling.StartMeasure(48);
			bool flag4 = this._renderPassStates[2];
			if (flag4)
			{
				this.BeginWireframeMode(GameInstance.WireframePass.OnMapOpaque);
				this.SceneRenderer.DrawMapChunksOpaque(true, false);
				this.EndWireframeMode(GameInstance.WireframePass.OnMapOpaque);
			}
			bool flag5 = this.SceneRenderer.MapBlocksAnimatedNeedDrawing();
			bool flag6 = this._renderPassStates[4] && flag5;
			if (flag6)
			{
				this.Engine.Profiling.StartMeasure(49);
				MapBlockAnimatedProgram mapBlockAnimatedProgram = this.Engine.Graphics.GPUProgramStore.MapBlockAnimatedProgram;
				gl.UseProgram(mapBlockAnimatedProgram);
				this.BeginWireframeMode(GameInstance.WireframePass.OnMapAnim);
				this.SceneRenderer.DrawMapBlocksAnimated();
				this.EndWireframeMode(GameInstance.WireframePass.OnMapAnim);
				this.Engine.Profiling.StopMeasure(49);
			}
			else
			{
				this.Engine.Profiling.SkipMeasure(49);
			}
			MapChunkNearProgram mapChunkNearAlphaTestedProgram = this.Engine.Graphics.GPUProgramStore.MapChunkNearAlphaTestedProgram;
			gl.UseProgram(mapChunkNearAlphaTestedProgram);
			Vector3 playerRenderPosition = this.SceneRenderer.Data.PlayerRenderPosition;
			Vector3[] closestEntityPositions = this.EntityStoreModule.ClosestEntityPositions;
			mapChunkNearAlphaTestedProgram.FoliageInteractionPositions.SetValue(closestEntityPositions);
			mapChunkNearAlphaTestedProgram.FoliageInteractionParams.SetValue(this._foliageInteractionParams);
			bool flag7 = this._renderPassStates[3];
			if (flag7)
			{
				this.BeginWireframeMode(GameInstance.WireframePass.OnMapAlphaTested);
				this.SceneRenderer.DrawMapChunksAlphaTested(true, false);
				this.EndWireframeMode(GameInstance.WireframePass.OnMapAlphaTested);
			}
			this.Engine.Profiling.StopMeasure(48);
			OcclusionCulling occlusionCulling = this.Engine.OcclusionCulling;
			bool isActive = occlusionCulling.IsActive;
			bool flag8 = isActive;
			bool flag9 = isActive && this.UseOcclusionCullingForEntities;
			bool flag10 = flag9 && this.UseOcclusionCullingForEntitiesAnimations;
			bool flag11 = isActive && this.UseOcclusionCullingForLights;
			bool flag12 = isActive && this.UseOcclusionCullingForParticles;
			occlusionCulling.FetchVisibleOccludeesFromGPU(ref this.SceneRenderer.VisibleOccludees);
			this.Engine.Profiling.StartMeasure(50);
			MapChunkFarProgram mapChunkFarOpaqueProgram = this.Engine.Graphics.GPUProgramStore.MapChunkFarOpaqueProgram;
			gl.UseProgram(mapChunkFarOpaqueProgram);
			bool flag13 = this._renderPassStates[5];
			if (flag13)
			{
				this.BeginWireframeMode(GameInstance.WireframePass.OnMapOpaque);
				this.SceneRenderer.DrawMapChunksOpaque(false, isActive);
				this.EndWireframeMode(GameInstance.WireframePass.OnMapOpaque);
			}
			MapChunkFarProgram mapChunkFarAlphaTestedProgram = this.Engine.Graphics.GPUProgramStore.MapChunkFarAlphaTestedProgram;
			gl.UseProgram(mapChunkFarAlphaTestedProgram);
			bool flag14 = this._renderPassStates[6];
			if (flag14)
			{
				this.BeginWireframeMode(GameInstance.WireframePass.OnMapAlphaTested);
				this.SceneRenderer.DrawMapChunksAlphaTested(false, isActive);
				this.EndWireframeMode(GameInstance.WireframePass.OnMapAlphaTested);
			}
			this.Engine.Profiling.StopMeasure(50);
			gl.Enable(GL.CULL_FACE);
			bool flag15 = this._renderPassStates[7];
			if (flag15)
			{
				this.Engine.Profiling.StartMeasure(51);
				gl.UseProgram(blockyModelProgram);
				blockyModelProgram.NearScreendoorThreshold.SetValue(0.55f);
				this.BeginDebugEntitiesZTest();
				this.BeginWireframeMode(GameInstance.WireframePass.OnEntities);
				blockyModelProgram.AtlasSizeFactor0.SetValue(this._atlasSizeFactor0);
				blockyModelProgram.AtlasSizeFactor1.SetValue(this._atlasSizeFactor1);
				blockyModelProgram.AtlasSizeFactor2.SetValue(this._atlasSizeFactor2);
				this.SceneRenderer.DrawEntityCharactersAndItems(flag9);
				this.EndWireframeMode(GameInstance.WireframePass.OnEntities);
				this.EndDebugEntitiesZTest();
				this.Engine.Profiling.StopMeasure(51);
			}
			else
			{
				this.Engine.Profiling.SkipMeasure(51);
			}
			rtstore.GBuffer.Unbind();
			this.EndWireframeMode(GameInstance.WireframePass.OnAll);
			gl.DepthMask(false);
			this.SceneRenderer.RenderIntermediateBuffers();
			gl.StencilMask(0U);
			gl.StencilFunc(GL.ALWAYS, 0, 255U);
			bool flag16 = false;
			bool flag17 = isSunShadowMappingEnabled;
			if (flag17)
			{
				flag16 = this.SceneRenderer.UseDeferredShadowBlur;
				this.Engine.Profiling.StartMeasure(56);
				this.SceneRenderer.DrawDeferredShadow();
				this.Engine.Profiling.StopMeasure(56);
			}
			else
			{
				this.Engine.Profiling.SkipMeasure(56);
			}
			bool useSSAO = this.SceneRenderer.UseSSAO;
			if (useSSAO)
			{
				flag16 = (flag16 || this.SceneRenderer.UseSSAOBlur);
				this.Engine.Profiling.StartMeasure(57);
				this.SceneRenderer.DrawSSAO();
				this.Engine.Profiling.StopMeasure(57);
			}
			else
			{
				this.Engine.Profiling.SkipMeasure(57);
			}
			bool flag18 = flag16;
			if (flag18)
			{
				this.Engine.Profiling.StartMeasure(58);
				this.SceneRenderer.BlurSSAOAndShadow();
				this.Engine.Profiling.StopMeasure(58);
			}
			else
			{
				this.Engine.Profiling.SkipMeasure(58);
			}
			bool useVolumetricSunshaft = this._useVolumetricSunshaft;
			if (useVolumetricSunshaft)
			{
				this.Engine.Profiling.StartMeasure(59);
				this.DrawVolumetricSunshafts();
				this.Engine.Profiling.StopMeasure(59);
			}
			BasicProgram basicProgram = this.Engine.Graphics.GPUProgramStore.BasicProgram;
			this.Engine.Profiling.StartMeasure(60);
			this.SceneRenderer.DrawLightPass();
			this.Engine.Profiling.StopMeasure(60);
			this.Engine.Profiling.StartMeasure(65);
			OrderIndependentTransparency.Method currentMethod = this.SceneRenderer.OIT.CurrentMethod;
			bool flag19 = currentMethod == OrderIndependentTransparency.Method.POIT;
			if (flag19)
			{
				rtstore.FinalSceneColor.Bind(true, true);
			}
			else
			{
				rtstore.SceneColor.Bind(false, true);
			}
			float[] data3 = new float[4];
			gl.ClearBufferfv(GL.COLOR, 0, data3);
			this.SceneRenderer.ApplyDeferred(this._projectionTexture, this._fogNoise.GLTexture);
			this.Engine.Profiling.StopMeasure(65);
			RenderTarget renderTarget = this.Engine.Graphics.IsGPULowEnd ? rtstore.LinearZHalfRes : rtstore.LinearZ;
			this.BeginWireframeMode(GameInstance.WireframePass.OnAll);
			this.SceneRenderer.ClusteredLighting.SetupLightDataTextures(15U, 16U);
			bool flag20 = this.Engine.FXSystem.Particles.HasErosionTasks || this.Engine.FXSystem.Trails.HasErosionTasks;
			flag20 = (flag20 && this._renderPassStates[10]);
			gl.Disable(GL.CULL_FACE);
			bool flag21 = flag20;
			if (flag21)
			{
				this.Engine.Profiling.StartMeasure(66);
				gl.DepthMask(true);
				this.SetupVFXTextures(true, false, false, false, true);
				ParticleProgram particleErosionProgram = this.Engine.Graphics.GPUProgramStore.ParticleErosionProgram;
				gl.UseProgram(particleErosionProgram);
				particleErosionProgram.InvTextureAtlasSize.SetValue(1f / (float)this.FXModule.TextureAtlas.Width, 1f / (float)this.FXModule.TextureAtlas.Height);
				particleErosionProgram.CurrentInvViewportSize.SetValue(this.SceneRenderer.Data.InvViewportSize);
				this.Engine.FXSystem.DrawErosion();
				gl.BindSampler(1U, GLSampler.None);
				gl.DepthMask(false);
				this.Engine.Profiling.StopMeasure(66);
			}
			else
			{
				this.Engine.Profiling.SkipMeasure(66);
			}
			gl.Enable(GL.BLEND);
			gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
			gl.ColorMask(true, true, true, false);
			bool flag22 = this._renderPassStates[8];
			if (flag22)
			{
				this.Engine.Profiling.StartMeasure(67);
				bool flag23 = true;
				bool flag24 = this.WeatherModule.SkyRenderer.SunNeedsDrawing(this.SceneRenderer.Data.SunPositionWS, this.SceneRenderer.Data.CameraDirection, this.WeatherModule.SunScale);
				bool flag25 = this.WeatherModule.SkyRenderer.MoonNeedsDrawing(this.SceneRenderer.Data.SunPositionWS, this.SceneRenderer.Data.CameraDirection, this.WeatherModule.MoonScale);
				bool flag26 = this.WeatherModule.SkyRenderer.StarsNeedDrawing(this.SceneRenderer.Data.SunPositionWS);
				flag23 = (flag23 && !this._isCameraUnderwater);
				SkyProgram skyProgram = this.Engine.Graphics.GPUProgramStore.SkyProgram;
				gl.UseProgram(skyProgram);
				gl.ActiveTexture(GL.TEXTURE4);
				gl.BindTexture(GL.TEXTURE_2D, rtstore.SunOcclusionHistory.GetTexture(RenderTarget.Target.Color0));
				gl.ActiveTexture(GL.TEXTURE0);
				gl.BindTexture(GL.TEXTURE_2D, this.WeatherModule.SkyRenderer.StarsTexture);
				skyProgram.StarsOpacity.SetValue(this.WeatherModule.StarsOpacity);
				skyProgram.TopGradientColor.SetValue(this.WeatherModule.SkyTopGradientColor);
				skyProgram.SunsetColor.SetValue(this.WeatherModule.SunsetColor);
				skyProgram.FogFrontColor.SetValue(this.SceneRenderer.Data.FogFrontColor);
				skyProgram.FogBackColor.SetValue(this.SceneRenderer.Data.FogBackColor);
				bool useMoodFog2 = this._useMoodFog;
				if (useMoodFog2)
				{
					skyProgram.FogMoodParams.SetValue(this.SceneRenderer.Data.FogMoodParams.X, this.SceneRenderer.Data.FogMoodParams.Y, this.SceneRenderer.Data.FogHeightDensityAtViewer);
					skyProgram.CameraPosition.SetValue(this.SceneRenderer.Data.CameraPosition);
				}
				skyProgram.SunPosition.SetValue(this.SceneRenderer.Data.SunPositionWS);
				float value = MathHelper.Min(this.WeatherModule.SunScale, 3f);
				float value2 = MathHelper.Min(this.WeatherModule.MoonScale, 3f);
				skyProgram.SunScale.SetValue(value);
				skyProgram.SunGlowColor.SetValue(this.WeatherModule.SunGlowColor);
				skyProgram.MoonOpacity.SetValue(this.WeatherModule.MoonColor.W);
				skyProgram.MoonScale.SetValue(value2);
				skyProgram.MoonGlowColor.SetValue(this.WeatherModule.MoonGlowColor);
				skyProgram.DrawSkySunMoonStars.SetValue((float)(flag23 ? 1 : 0), (float)(flag24 ? 1 : 0), (float)(flag25 ? 1 : 0), (float)(flag26 ? 1 : 0));
				this.BeginWireframeMode(GameInstance.WireframePass.OnSky);
				this.WeatherModule.SkyRenderer.DrawSky();
				this.EndWireframeMode(GameInstance.WireframePass.OnSky);
				bool flag27 = !this._isCameraUnderwater && (flag24 || flag25);
				if (flag27)
				{
					gl.UseProgram(basicProgram);
					bool flag28 = flag24;
					if (flag28)
					{
						basicProgram.Opacity.SetValue(1f);
						basicProgram.Color.SetValue(this.WeatherModule.SunColor.X, this.WeatherModule.SunColor.Y, this.WeatherModule.SunColor.Z);
						gl.BindTexture(GL.TEXTURE_2D, this.WeatherModule.SkyRenderer.SunTexture);
						this.WeatherModule.SkyRenderer.DrawSun();
					}
					bool flag29 = flag25;
					if (flag29)
					{
						basicProgram.Opacity.SetValue(this.WeatherModule.MoonColor.W);
						basicProgram.Color.SetValue(this.WeatherModule.MoonColor.X, this.WeatherModule.MoonColor.Y, this.WeatherModule.MoonColor.Z);
						gl.BindTexture(GL.TEXTURE_2D, this.WeatherModule.SkyRenderer.MoonTexture);
						this.WeatherModule.SkyRenderer.DrawMoon();
					}
				}
				bool flag30 = !this._isCameraUnderwater;
				if (flag30)
				{
					CloudsProgram cloudsProgram = this.Engine.Graphics.GPUProgramStore.CloudsProgram;
					gl.UseProgram(cloudsProgram);
					cloudsProgram.Colors.SetValue(this.WeatherModule.SkyRenderer.CloudColors);
					cloudsProgram.UVOffsets.SetValue(this.WeatherModule.SkyRenderer.CloudOffsets);
					cloudsProgram.UVMotionParams.SetValue(this._cloudsUVMotionScale, this._cloudsUVMotionStrength);
					cloudsProgram.FogFrontColor.SetValue(this.SceneRenderer.Data.FogFrontColor);
					cloudsProgram.FogBackColor.SetValue(this.SceneRenderer.Data.FogBackColor);
					cloudsProgram.SunPosition.SetValue(this.SceneRenderer.Data.SunPositionWS);
					bool useMoodFog3 = this._useMoodFog;
					if (useMoodFog3)
					{
						cloudsProgram.FogMoodParams.SetValue(this.SceneRenderer.Data.FogMoodParams.X, this.SceneRenderer.Data.FogMoodParams.Y, this.SceneRenderer.Data.FogHeightDensityAtViewer);
						cloudsProgram.CameraPosition.SetValue(this.SceneRenderer.Data.CameraPosition);
					}
					gl.ActiveTexture(GL.TEXTURE5);
					gl.BindTexture(GL.TEXTURE_2D, this._flowMap.GLTexture);
					int cloudsTexturesCount = this.WeatherModule.SkyRenderer.CloudsTexturesCount;
					bool flag31 = cloudsTexturesCount != 0;
					if (flag31)
					{
						for (int i = cloudsTexturesCount - 1; i >= 0; i--)
						{
							gl.ActiveTexture(GL.TEXTURE0 + (uint)i);
							gl.BindTexture(GL.TEXTURE_2D, this.WeatherModule.SkyRenderer.CloudsTextures[i]);
						}
						cloudsProgram.CloudsTextureCount.SetValue(cloudsTexturesCount);
						this.WeatherModule.SkyRenderer.DrawClouds();
					}
				}
				this.Engine.Profiling.StopMeasure(67);
			}
			else
			{
				this.Engine.Profiling.SkipMeasure(67);
			}
			bool useSkyboxTest = this.UseSkyboxTest;
			if (useSkyboxTest)
			{
				this.DrawCubeMapTest();
			}
			RenderTarget renderTarget2 = (currentMethod == OrderIndependentTransparency.Method.POIT) ? rtstore.FinalSceneColor : rtstore.SceneColor;
			this.SceneRenderer.BlitSceneColorToHalfRes(renderTarget2, GL.LINEAR, true, false, true);
			bool flag32 = this._renderPassStates[9] && !this._useChunksOIT;
			if (flag32)
			{
				this.Engine.Profiling.StartMeasure(68);
				gl.Disable(GL.STENCIL_TEST);
				gl.ColorMask(true, true, true, true);
				MapChunkAlphaBlendedProgram mapChunkAlphaBlendedProgram = this.Engine.Graphics.GPUProgramStore.MapChunkAlphaBlendedProgram;
				gl.UseProgram(mapChunkAlphaBlendedProgram);
				mapChunkAlphaBlendedProgram.InvTextureAtlasSize.SetValue(1f / (float)this.MapModule.TextureAtlas.Width, 1f / (float)this.MapModule.TextureAtlas.Height);
				mapChunkAlphaBlendedProgram.WaterTintColor.SetValue(this.WeatherModule.WaterTintColor);
				mapChunkAlphaBlendedProgram.WaterQuality.SetValue(this._waterQuality);
				mapChunkAlphaBlendedProgram.CurrentInvViewportSize.SetValue(this.SceneRenderer.Data.InvViewportSize);
				this.SetupChunkAlphaBlendedTextures(true, mapChunkAlphaBlendedProgram.UseForwardSunShadows, false, this._useMoodFog);
				bool flag33 = this._waterQuality != 0;
				if (flag33)
				{
					gl.Disable(GL.BLEND);
					gl.DepthMask(true);
				}
				this.BeginWireframeMode(GameInstance.WireframePass.OnMapAlphaBlend);
				this.SceneRenderer.DrawMapChunksAlphaBlended(isActive);
				this.EndWireframeMode(GameInstance.WireframePass.OnMapAlphaBlend);
				bool flag34 = this._waterQuality != 0;
				if (flag34)
				{
					gl.Enable(GL.BLEND);
					gl.DepthMask(false);
				}
				gl.ColorMask(true, true, true, false);
				gl.BindSampler((uint)mapChunkAlphaBlendedProgram.TextureUnits.SceneColor, GLSampler.None);
				gl.BindSampler((uint)mapChunkAlphaBlendedProgram.TextureUnits.Refraction, GLSampler.None);
				this.Engine.Profiling.StopMeasure(68);
			}
			else
			{
				this.Engine.Profiling.SkipMeasure(68);
			}
			gl.Disable(GL.STENCIL_TEST);
			gl.StencilMask(255U);
			gl.ActiveTexture(GL.TEXTURE0);
			gl.BindTexture(GL.TEXTURE_2D, this.Engine.Graphics.WhitePixelTexture.GLTexture);
			bool flag35 = this.WeatherModule.IsUnderWater && this.WeatherModule.ActiveFogMode != WeatherModule.FogMode.Off;
			bool flag36 = this.InteractionModule.TargetBlockOutineNeedsDrawing() || this.EntityStoreModule.DebugInfoNeedsDrawing || flag35 || this.BuilderToolsModule.NeedsDrawing() || this.ImmersiveScreenModule.NeedsDrawing() || this.MachinimaModule.NeedsDrawing() || this.InteractionModule.ShowSelectorDebug || this.DebugDisplayModule.ShouldDraw;
			bool flag37 = flag36;
			if (flag37)
			{
				gl.UseProgram(basicProgram);
				bool flag38 = this.InteractionModule.TargetBlockOutineNeedsDrawing();
				if (flag38)
				{
					basicProgram.Color.SetValue(this.Engine.Graphics.BlackColor);
					basicProgram.Opacity.SetValue(0.12f);
					this.InteractionModule.DrawTargetBlockOutline(ref this.SceneRenderer.Data.CameraPosition, ref this.SceneRenderer.Data.ViewRotationProjectionMatrix);
					ClientItemBase primaryItem = this.LocalPlayer.PrimaryItem;
					bool flag39 = this.App.Settings.DisplayBlockSubfaces && primaryItem != null;
					if (flag39)
					{
						int blockId = primaryItem.BlockId;
						ClientBlockType blockType = this.MapModule.ClientBlockTypes[blockId];
						this.InteractionModule.DrawTargetBlockSubface(ref this.SceneRenderer.Data.CameraPosition, ref this.SceneRenderer.Data.ViewRotationProjectionMatrix, blockType);
					}
				}
				bool flag40 = this.InteractionModule.BlockBreakHealth.NeedsDrawing();
				if (flag40)
				{
					this.InteractionModule.BlockBreakHealth.Draw();
				}
				bool showSelectorDebug = this.InteractionModule.ShowSelectorDebug;
				if (showSelectorDebug)
				{
					this.InteractionModule.DrawDebugSelector(this.Engine.Graphics, gl, ref this.SceneRenderer.Data.CameraPosition, ref this.SceneRenderer.Data.ViewRotationProjectionMatrix);
				}
				bool shouldDraw = this.DebugDisplayModule.ShouldDraw;
				if (shouldDraw)
				{
					this.DebugDisplayModule.Draw(this.Engine.Graphics, gl, this.DeltaTime, ref this.SceneRenderer.Data.CameraPosition, ref this.SceneRenderer.Data.ViewRotationProjectionMatrix);
				}
				bool debugInfoNeedsDrawing = this.EntityStoreModule.DebugInfoNeedsDrawing;
				if (debugInfoNeedsDrawing)
				{
					this.SceneRenderer.DrawEntityDebugInfo();
				}
				bool flag41 = flag35;
				if (flag41)
				{
					basicProgram.Color.SetValue(this.WeatherModule.FogColor.X, this.WeatherModule.FogColor.Y, this.WeatherModule.FogColor.Z);
					basicProgram.Opacity.SetValue(1f);
					this.WeatherModule.SkyRenderer.DrawHorizon();
				}
				bool flag42 = this.BuilderToolsModule.NeedsDrawing();
				if (flag42)
				{
					this.SetupVFXTextures(true, true, false, true, false);
					this.BuilderToolsModule.Draw(ref this.SceneRenderer.Data.ViewRotationProjectionMatrix);
				}
				bool flag43 = this.MachinimaModule.NeedsDrawing();
				if (flag43)
				{
					gl.DepthMask(true);
					this.MachinimaModule.Draw(ref this.SceneRenderer.Data.ViewProjectionMatrix);
					gl.DepthMask(false);
				}
				bool flag44 = this.ImmersiveScreenModule.NeedsDrawing();
				if (flag44)
				{
					basicProgram.Color.SetValue(this.Engine.Graphics.WhiteColor);
					basicProgram.Opacity.SetValue(1f);
					this.ImmersiveScreenModule.Draw();
				}
			}
			renderTarget2.Unbind();
			bool flag45 = this.Engine.FXSystem.Particles.ParticleSpawnerDrawCount != 0;
			bool flag46 = this.Engine.FXSystem.Trails.BlendDrawCount != 0;
			bool hasColorTasks = this.Engine.FXSystem.ForceFields.HasColorTasks;
			flag45 = (flag45 && this._renderPassStates[10]);
			bool flag47 = flag45 || flag46 || hasColorTasks || currentMethod == OrderIndependentTransparency.Method.POIT || this._useChunksOIT;
			if (flag47)
			{
				this.Engine.Profiling.StartMeasure(69);
				FXSystem fxsystem = this.Engine.FXSystem;
				this.SetupVFXTextures(true, false, false, false, false);
				bool useChunksOIT = this._useChunksOIT;
				if (useChunksOIT)
				{
					this.SetupChunkAlphaBlendedTextures(true, true, true, true);
				}
				this.Engine.Graphics.GPUProgramStore.ParticleProgram.SceneDataBlock.SetBuffer(this.SceneRenderer.SceneDataBuffer);
				this.Engine.Graphics.GPUProgramStore.ForceFieldProgram.SceneDataBlock.SetBuffer(this.SceneRenderer.SceneDataBuffer);
				this.Engine.Graphics.GPUProgramStore.BuilderToolProgram.SceneDataBlock.SetBuffer(this.SceneRenderer.SceneDataBuffer);
				bool flag48 = currentMethod > OrderIndependentTransparency.Method.None;
				if (flag48)
				{
					bool flag49 = fxsystem.ForceFields.HasColorTasks || fxsystem.Particles.HighResDrawCount + fxsystem.Trails.BlendDrawCount > 0;
					bool flag50 = fxsystem.Particles.LowResDrawCount > 0;
					bool hasQuarterResItems = false;
					flag49 = (flag49 || this._useChunksOIT);
					switch (this._oitRes)
					{
					case 1:
						hasQuarterResItems = flag50;
						flag50 = flag49;
						flag49 = false;
						break;
					case 2:
						hasQuarterResItems = (flag49 || flag50);
						flag50 = false;
						flag49 = false;
						break;
					}
					this.SceneRenderer.OIT.Draw(flag49, flag50, hasQuarterResItems);
				}
				else
				{
					this.SceneRenderer.OIT.SkipInternalMeasures();
					gl.BlendFunc(GL.ONE, GL.ONE_MINUS_SRC_ALPHA);
					bool flag51 = flag45;
					if (flag51)
					{
						this.DrawParticles((int)currentMethod, 0, this.SceneRenderer.Data.InvViewportSize, false, true);
						this.Engine.FXSystem.DrawTransparencyLowRes();
					}
					bool flag52 = hasColorTasks;
					if (flag52)
					{
						this.DrawForceFields((int)currentMethod, 0, this.SceneRenderer.Data.ViewportSize, true);
					}
					gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
				}
				gl.BindSampler(8U, GLSampler.None);
				gl.BindSampler(4U, GLSampler.None);
				gl.BindSampler(3U, GLSampler.None);
				this.Engine.Profiling.StopMeasure(69);
			}
			else
			{
				this.Engine.Profiling.SkipMeasure(69);
				this.SceneRenderer.OIT.SkipInternalMeasures();
			}
			gl.DepthMask(true);
			bool flag53 = this.SceneRenderer.HasVisibleNameplates || this.BuilderToolsModule.NeedsTextDrawing() || this.MachinimaModule.TextNeedsDrawing();
			bool flag54 = this._renderPassStates[11] && flag53;
			if (flag54)
			{
				this.Engine.Profiling.StartMeasure(75);
				TextProgram textProgram = this.Engine.Graphics.GPUProgramStore.TextProgram;
				gl.UseProgram(textProgram);
				gl.BindTexture(GL.TEXTURE_2D, this.App.Fonts.DefaultFontFamily.RegularFont.TextureAtlas.GLTexture);
				textProgram.FogColor.SetValue(this.WeatherModule.FogColor);
				textProgram.FogParams.SetValue(this.SceneRenderer.Data.FogParams);
				textProgram.FillThreshold.SetValue(0f);
				textProgram.OutlineThreshold.SetValue(0f);
				textProgram.OutlineBlurThreshold.SetValue(0f);
				textProgram.OutlineOffset.SetValue(Vector2.Zero);
				textProgram.Opacity.SetValue(1f);
				this.SceneRenderer.DrawEntityNameplates(flag9);
				bool flag55 = this.BuilderToolsModule.NeedsTextDrawing();
				if (flag55)
				{
					this.BuilderToolsModule.DrawText(ref this.SceneRenderer.Data.ViewProjectionMatrix);
				}
				bool flag56 = this.MachinimaModule.TextNeedsDrawing();
				if (flag56)
				{
					this.MachinimaModule.DrawText(ref this.SceneRenderer.Data.ViewProjectionMatrix);
				}
				this.Engine.Profiling.StopMeasure(75);
			}
			else
			{
				this.Engine.Profiling.SkipMeasure(75);
			}
			this.SceneRenderer.SetupModelVFXDataTexture(6U);
			this.SceneRenderer.SetupEntityDataTexture(5U);
			gl.ActiveTexture(GL.TEXTURE4);
			gl.BindTexture(GL.TEXTURE_2D, this._flowMap.GLTexture);
			gl.ActiveTexture(GL.TEXTURE2);
			gl.BindTexture(GL.TEXTURE_2D, this.App.CharacterPartStore.TextureAtlas.GLTexture);
			gl.ActiveTexture(GL.TEXTURE1);
			gl.BindTexture(GL.TEXTURE_2D, this.EntityStoreModule.TextureAtlas.GLTexture);
			gl.ActiveTexture(GL.TEXTURE0);
			gl.BindTexture(GL.TEXTURE_2D, this.MapModule.TextureAtlas.GLTexture);
			BlockyModelProgram blockyModelDitheringProgram = this.Engine.Graphics.GPUProgramStore.BlockyModelDitheringProgram;
			gl.UseProgram(blockyModelDitheringProgram);
			this.SceneRenderer.DrawForwardEntity(this._atlasSizeFactor0, this._atlasSizeFactor1, this._atlasSizeFactor2);
			this.EndWireframeMode(GameInstance.WireframePass.OnAll);
			gl.StencilMask(255U);
			bool flag57 = flag8 && this.DebugDrawOccludeeChunks;
			bool flag58 = flag9 && this.DebugDrawOccludeeEntities;
			bool flag59 = flag11 && this.DebugDrawOccludeeLights;
			bool flag60 = flag12 && this.DebugDrawOccludeeParticles;
			bool flag61 = flag57 || flag58 || flag59 || flag60 || this.DebugDrawLight || this._debugParticleBoundingVolume || this.Engine.FXSystem.Particles.DebugInfoNeedsDrawing();
			bool flag62 = flag61;
			if (flag62)
			{
				gl.Enable(GL.DEPTH_TEST);
				gl.Disable(GL.BLEND);
				gl.DepthMask(false);
				gl.PolygonMode(GL.FRONT_AND_BACK, GL.LINE);
				bool flag63 = flag57;
				if (flag63)
				{
					this.Engine.OcclusionCulling.DebugDrawOccludees(this.SceneRenderer.ChunkOccludeesOffset, this.SceneRenderer.ChunkOccludeesCount, ref this.SceneRenderer.Data.ViewRotationProjectionMatrix, true);
				}
				bool flag64 = flag58;
				if (flag64)
				{
					this.Engine.OcclusionCulling.DebugDrawOccludees(this.SceneRenderer.EntityOccludeesOffset, this.SceneRenderer.EntityOccludeesCount, ref this.SceneRenderer.Data.ViewRotationProjectionMatrix, false);
				}
				bool flag65 = flag59;
				if (flag65)
				{
					this.Engine.OcclusionCulling.DebugDrawOccludees(this.SceneRenderer.LightOccludeesOffset, this.SceneRenderer.LightOccludeesCount, ref this.SceneRenderer.Data.ViewRotationProjectionMatrix, false);
				}
				bool flag66 = flag60;
				if (flag66)
				{
					this.Engine.OcclusionCulling.DebugDrawOccludees(this.SceneRenderer.ParticleOccludeesOffset, this.SceneRenderer.ParticleOccludeesCount, ref this.SceneRenderer.Data.ViewRotationProjectionMatrix, false);
				}
				bool debugDrawLight = this.DebugDrawLight;
				if (debugDrawLight)
				{
					this.SceneRenderer.DebugDrawLights(this.GlobalLightData, this.GlobalLightDataCount);
				}
				bool flag67 = this._debugParticleBoundingVolume || this.Engine.FXSystem.Particles.DebugInfoNeedsDrawing();
				if (flag67)
				{
					bool flag68 = !this._debugParticleZTestEnabled;
					if (flag68)
					{
						gl.Disable(GL.DEPTH_TEST);
					}
					gl.UseProgram(basicProgram);
					gl.BindTexture(GL.TEXTURE_2D, this.Engine.Graphics.WhitePixelTexture.GLTexture);
					this.Engine.FXSystem.Particles.DrawDebugInfo(ref this.SceneRenderer.Data.ViewProjectionMatrix);
					this.Engine.FXSystem.Particles.DrawDebugBoundingVolumes(ref this.SceneRenderer.Data.CameraPosition, ref this.SceneRenderer.Data.ViewRotationProjectionMatrix);
				}
				gl.DepthMask(true);
				gl.Enable(GL.BLEND);
				gl.PolygonMode(GL.FRONT_AND_BACK, GL.FILL);
			}
			bool needsDebugDrawShadowRelated = this.SceneRenderer.NeedsDebugDrawShadowRelated;
			if (needsDebugDrawShadowRelated)
			{
				this.SceneRenderer.DebugDrawShadowRelated();
			}
			gl.StencilMask(255U);
			rtstore.SceneColor.Unbind();
			gl.ColorMask(true, true, true, true);
			bool isDistortionEnabled = this.PostEffectRenderer.IsDistortionEnabled;
			if (isDistortionEnabled)
			{
				this.Engine.Profiling.StartMeasure(76);
				rtstore.Distortion.Bind(false, true);
				gl.DepthMask(false);
				gl.BlendEquationSeparate(GL.FUNC_ADD, GL.FUNC_ADD);
				gl.BlendFunc(GL.ONE, GL.ONE);
				gl.ClearColor(0f, 0f, 0f, 0f);
				gl.Clear(GL.COLOR_BUFFER_BIT);
				bool hasDistortionTasks = this.Engine.FXSystem.ForceFields.HasDistortionTasks;
				if (hasDistortionTasks)
				{
					this.Engine.FXSystem.ForceFields.DrawDistortion();
				}
				bool flag69 = this.SceneRenderer.HasEntityDistortionTask || this.LocalPlayer.NeedsDistortionDraw;
				if (flag69)
				{
					gl.Enable(GL.CULL_FACE);
					this.SceneRenderer.SetupModelVFXDataTexture(6U);
					this.SceneRenderer.SetupEntityDataTexture(5U);
					gl.ActiveTexture(GL.TEXTURE4);
					gl.BindTexture(GL.TEXTURE_2D, this._flowMap.GLTexture);
					gl.ActiveTexture(GL.TEXTURE2);
					gl.BindTexture(GL.TEXTURE_2D, this.App.CharacterPartStore.TextureAtlas.GLTexture);
					gl.ActiveTexture(GL.TEXTURE1);
					gl.BindTexture(GL.TEXTURE_2D, this.EntityStoreModule.TextureAtlas.GLTexture);
					gl.ActiveTexture(GL.TEXTURE0);
					gl.BindTexture(GL.TEXTURE_2D, this.MapModule.TextureAtlas.GLTexture);
					bool hasEntityDistortionTask = this.SceneRenderer.HasEntityDistortionTask;
					if (hasEntityDistortionTask)
					{
						BlockyModelProgram blockyModelDistortionProgram = this.Engine.Graphics.GPUProgramStore.BlockyModelDistortionProgram;
						gl.UseProgram(blockyModelDistortionProgram);
						blockyModelDistortionProgram.CurrentInvViewportSize.SetValue(rtstore.Distortion.InvResolution);
						this.SceneRenderer.DrawEntityDistortion(false);
					}
					bool needsDistortionDraw = this.LocalPlayer.NeedsDistortionDraw;
					if (needsDistortionDraw)
					{
						BlockyModelProgram firstPersonDistortionBlockyModelProgram = this.Engine.Graphics.GPUProgramStore.FirstPersonDistortionBlockyModelProgram;
						gl.UseProgram(firstPersonDistortionBlockyModelProgram);
						firstPersonDistortionBlockyModelProgram.ViewMatrix.SetValue(ref this.SceneRenderer.Data.FirstPersonViewMatrix);
						firstPersonDistortionBlockyModelProgram.ViewProjectionMatrix.SetValue(ref this.SceneRenderer.Data.FirstPersonProjectionMatrix);
						this.LocalPlayer.DrawDistortionInFirstPersonView();
					}
					gl.Disable(GL.CULL_FACE);
				}
				bool flag70 = this.Engine.FXSystem.Particles.HasDistortionTasks || this.Engine.FXSystem.Trails.HasDistortionTasks;
				if (flag70)
				{
					this.Engine.FXSystem.SetupDrawDataTexture(11U);
					gl.ActiveTexture(GL.TEXTURE7);
					gl.BindTexture(GL.TEXTURE_2D, this.FXModule.TextureAtlas.GLTexture);
					gl.ActiveTexture(GL.TEXTURE0);
					ParticleProgram particleDistortionProgram = this.Engine.Graphics.GPUProgramStore.ParticleDistortionProgram;
					gl.UseProgram(particleDistortionProgram);
					particleDistortionProgram.InvTextureAtlasSize.SetValue(1f / (float)this.FXModule.TextureAtlas.Width, 1f / (float)this.FXModule.TextureAtlas.Height);
					particleDistortionProgram.CurrentInvViewportSize.SetValue(rtstore.Distortion.InvResolution);
					this.Engine.FXSystem.DrawDistortion();
				}
				gl.ClearColor(0f, 0f, 0f, 1f);
				gl.DepthMask(true);
				gl.BlendEquationSeparate(GL.FUNC_ADD, GL.FUNC_ADD);
				gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
				rtstore.Distortion.Unbind();
				this.Engine.Profiling.StopMeasure(76);
			}
			else
			{
				this.Engine.Profiling.SkipMeasure(76);
			}
			bool debugParticleOverdraw = this._debugParticleOverdraw;
			if (debugParticleOverdraw)
			{
				gl.AssertEnabled(GL.BLEND);
				rtstore.DebugFXOverdraw.Bind(true, true);
				bool flag71 = this.Engine.FXSystem.Particles.ParticleSpawnerDrawCount != 0;
				if (flag71)
				{
					gl.DepthMask(false);
					gl.BlendFunc(GL.ONE, GL.ONE);
					ParticleProgram particleProgram = this.Engine.Graphics.GPUProgramStore.ParticleProgram;
					gl.UseProgram(particleProgram);
					particleProgram.DebugOverdraw.SetValue(1);
					this.DrawParticles(0, 0, rtstore.DebugFXOverdraw.InvResolution, false, false);
					this.Engine.FXSystem.DrawTransparencyLowRes();
					particleProgram.DebugOverdraw.SetValue(0);
					gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
					gl.DepthMask(true);
				}
				rtstore.DebugFXOverdraw.Unbind();
			}
			bool updateEntities = flag10;
			bool updateLights = flag11;
			this.EntityStoreModule.UpdateVisibilityPrediction(this.SceneRenderer.VisibleOccludees, this.SceneRenderer.EntityOccludeesOffset, this.SceneRenderer.EntityOccludeesCount, this.SceneRenderer.LightOccludeesOffset, this.SceneRenderer.LightOccludeesCount, updateEntities, updateLights);
			bool updateParticles = flag12;
			this.ParticleSystemStoreModule.UpdateVisibilityPrediction(this.SceneRenderer.VisibleOccludees, this.SceneRenderer.ParticleOccludeesOffset, this.SceneRenderer.ParticleOccludeesCount, updateParticles);
		}

		// Token: 0x0600431B RID: 17179 RVA: 0x000D3550 File Offset: 0x000D1750
		private void DrawAlphaBlendedMapChunks(int methodId, int extra, Vector2 invViewportSize, bool lowRes, bool sendDataToGPU)
		{
			GLFunctions gl = this.Engine.Graphics.GL;
			RenderTargetStore rtstore = this.Engine.Graphics.RTStore;
			bool flag = !this._debugParticleZTestEnabled;
			if (flag)
			{
				gl.Disable(GL.DEPTH_TEST);
			}
			MapChunkAlphaBlendedProgram mapChunkAlphaBlendedProgram = this.Engine.Graphics.GPUProgramStore.MapChunkAlphaBlendedProgram;
			gl.UseProgram(mapChunkAlphaBlendedProgram);
			mapChunkAlphaBlendedProgram.CurrentInvViewportSize.SetValue(invViewportSize);
			mapChunkAlphaBlendedProgram.OITParams.SetValue(methodId, extra);
			mapChunkAlphaBlendedProgram.InvTextureAtlasSize.SetValue(1f / (float)this.MapModule.TextureAtlas.Width, 1f / (float)this.MapModule.TextureAtlas.Height);
			mapChunkAlphaBlendedProgram.WaterTintColor.SetValue(this.WeatherModule.WaterTintColor);
			mapChunkAlphaBlendedProgram.WaterQuality.SetValue(this._waterQuality);
			this.SceneRenderer.DrawMapChunksAlphaBlended(true);
			bool flag2 = !this._debugParticleZTestEnabled;
			if (flag2)
			{
				gl.Enable(GL.DEPTH_TEST);
			}
		}

		// Token: 0x0600431C RID: 17180 RVA: 0x000D3660 File Offset: 0x000D1860
		private void DrawParticles(int methodId, int extra, Vector2 invViewportSize, bool lowRes, bool sendDataToGPU)
		{
			GLFunctions gl = this.Engine.Graphics.GL;
			bool flag = !this._debugParticleZTestEnabled;
			if (flag)
			{
				gl.Disable(GL.DEPTH_TEST);
			}
			ParticleProgram particleProgram = this.Engine.Graphics.GPUProgramStore.ParticleProgram;
			gl.UseProgram(particleProgram);
			particleProgram.InvTextureAtlasSize.SetValue(1f / (float)this.FXModule.TextureAtlas.Width, 1f / (float)this.FXModule.TextureAtlas.Height);
			particleProgram.CurrentInvViewportSize.SetValue(invViewportSize);
			particleProgram.OITParams.SetValue(methodId, extra);
			if (lowRes)
			{
				this.Engine.FXSystem.DrawTransparencyLowRes();
			}
			else
			{
				this.Engine.FXSystem.DrawTransparency();
			}
			bool flag2 = !this._debugParticleZTestEnabled;
			if (flag2)
			{
				gl.Enable(GL.DEPTH_TEST);
			}
		}

		// Token: 0x0600431D RID: 17181 RVA: 0x000D3750 File Offset: 0x000D1950
		private void DrawForceFields(int methodId, int extra, Vector2 invViewportSize, bool sendDataToGPU)
		{
			GLFunctions gl = this.Engine.Graphics.GL;
			ForceFieldProgram forceFieldProgram = this.Engine.Graphics.GPUProgramStore.ForceFieldProgram;
			gl.UseProgram(forceFieldProgram);
			forceFieldProgram.DrawAndBlendMode.SetValue(forceFieldProgram.DrawModeColor, forceFieldProgram.BlendModePremultLinear);
			forceFieldProgram.CurrentInvViewportSize.SetValue(invViewportSize);
			forceFieldProgram.OITParams.SetValue(methodId, extra);
			this.Engine.FXSystem.ForceFields.DrawColor(sendDataToGPU);
		}

		// Token: 0x0600431E RID: 17182 RVA: 0x000D37D8 File Offset: 0x000D19D8
		private void DrawTransparents(int methodId, int extra, Vector2 invViewportSize, bool sendDataToGPU)
		{
			this.DrawTransparentsFullRes(methodId, extra, invViewportSize, sendDataToGPU);
			this.DrawTransparentsHalfRes(methodId, extra, invViewportSize, sendDataToGPU);
		}

		// Token: 0x0600431F RID: 17183 RVA: 0x000D37F4 File Offset: 0x000D19F4
		private void DrawTransparentsFullRes(int methodId, int extra, Vector2 invViewportSize, bool sendDataToGPU)
		{
			bool useChunksOIT = this._useChunksOIT;
			if (useChunksOIT)
			{
				this.DrawAlphaBlendedMapChunks(methodId, extra, invViewportSize, false, sendDataToGPU);
			}
			bool flag = this.Engine.FXSystem.Particles.HighResDrawCount > 0 || this.Engine.FXSystem.Trails.BlendDrawCount > 0;
			if (flag)
			{
				this.DrawParticles(methodId, extra, invViewportSize, false, sendDataToGPU);
			}
			bool hasColorTasks = this.Engine.FXSystem.ForceFields.HasColorTasks;
			if (hasColorTasks)
			{
				this.DrawForceFields(methodId, extra, invViewportSize, sendDataToGPU);
			}
		}

		// Token: 0x06004320 RID: 17184 RVA: 0x000D3888 File Offset: 0x000D1A88
		private void DrawTransparentsHalfRes(int methodId, int extra, Vector2 invViewportSize, bool sendDataToGPU)
		{
			bool flag = this.Engine.FXSystem.Particles.LowResDrawCount > 0;
			if (flag)
			{
				this.DrawParticles(methodId, extra, invViewportSize, true, sendDataToGPU);
			}
		}

		// Token: 0x06004321 RID: 17185 RVA: 0x000D38C4 File Offset: 0x000D1AC4
		public void DrawVolumetricSunshafts()
		{
			GLFunctions gl = this.Engine.Graphics.GL;
			RenderTargetStore rtstore = this.Engine.Graphics.RTStore;
			rtstore.VolumetricSunshaft.Bind(true, true);
			VolumetricSunshaftProgram volumetricSunshaftProgram = this.Engine.Graphics.GPUProgramStore.VolumetricSunshaftProgram;
			volumetricSunshaftProgram.SceneDataBlock.SetBuffer(this.SceneRenderer.SceneDataBuffer);
			gl.UseProgram(volumetricSunshaftProgram);
			gl.ActiveTexture(GL.TEXTURE2);
			gl.BindTexture(GL.TEXTURE_2D, rtstore.GBuffer.GetTexture(RenderTarget.Target.Color0));
			gl.ActiveTexture(GL.TEXTURE1);
			gl.BindTexture(GL.TEXTURE_2D, rtstore.ShadowMap.GetTexture(RenderTarget.Target.Depth));
			gl.ActiveTexture(GL.TEXTURE0);
			gl.BindTexture(GL.TEXTURE_2D, rtstore.LinearZHalfRes.GetTexture(RenderTarget.Target.Color0));
			volumetricSunshaftProgram.FarCorners.SetValue(this.SceneRenderer.Data.FrustumFarCornersWS);
			float num = this.SceneRenderer.Data.SunPositionWS.Y + 0.2f;
			bool flag = num > 0f;
			if (flag)
			{
				Vector4 value = new Vector4(this.WeatherModule.SunGlowColor.X, this.WeatherModule.SunGlowColor.Y, this.WeatherModule.SunGlowColor.Z, 1f);
				volumetricSunshaftProgram.SunColor.SetValue(value);
			}
			else
			{
				Vector4 value2 = new Vector4(this.WeatherModule.MoonGlowColor.X, this.WeatherModule.MoonGlowColor.Y, this.WeatherModule.MoonGlowColor.Z, 0.25f);
				volumetricSunshaftProgram.SunColor.SetValue(value2);
			}
			volumetricSunshaftProgram.SunDirection.SetValue(this.SceneRenderer.Data.SunShadowRenderData.VirtualSunDirection);
			this.Engine.Graphics.ScreenTriangleRenderer.Draw();
			rtstore.VolumetricSunshaft.Unbind();
			BlurProgram blurProgram = this.Engine.Graphics.GPUProgramStore.BlurProgram;
			gl.UseProgram(blurProgram);
			rtstore.BlurXResBy2.Bind(false, true);
			gl.BindTexture(GL.TEXTURE_2D, rtstore.VolumetricSunshaft.GetTexture(RenderTarget.Target.Color0));
			blurProgram.PixelSize.SetValue(1f / (float)rtstore.BlurXResBy2.Width, 1f / (float)rtstore.BlurXResBy2.Height);
			blurProgram.BlurScale.SetValue(1f);
			blurProgram.HorizontalPass.SetValue(1f);
			this.Engine.Graphics.ScreenTriangleRenderer.DrawRaw();
			rtstore.BlurXResBy2.Unbind();
			rtstore.VolumetricSunshaft.Bind(false, false);
			gl.BindTexture(GL.TEXTURE_2D, rtstore.BlurXResBy2.GetTexture(RenderTarget.Target.Color0));
			blurProgram.HorizontalPass.SetValue(0f);
			this.Engine.Graphics.ScreenTriangleRenderer.DrawRaw();
			rtstore.VolumetricSunshaft.Unbind();
		}

		// Token: 0x06004322 RID: 17186 RVA: 0x000D3BE0 File Offset: 0x000D1DE0
		public void DrawPostEffect()
		{
			bool flag = !this.IsPlaying;
			if (!flag)
			{
				RenderTargetStore rtstore = this.Engine.Graphics.RTStore;
				this.Engine.Profiling.StartMeasure(77);
				float time = (float)this._stopwatchSinceJoiningServer.ElapsedMilliseconds / 1000f;
				bool isUnderWater = this.WeatherModule.IsUnderWater;
				float distortionAmplitude = 0f;
				float distortionFrequency = 0f;
				float colorSaturation = 1f;
				Vector3 colorFilter = new Vector3(this.WeatherModule.ColorFilter.X, this.WeatherModule.ColorFilter.Y, this.WeatherModule.ColorFilter.Z);
				bool flag2 = isUnderWater;
				if (flag2)
				{
					FluidFX fluidFX = this.WeatherModule.FluidFX;
					Vector3 vector = (fluidFX.FogMode == null) ? Vector3.One : this.WeatherModule.FluidBlockLightColor;
					distortionAmplitude = fluidFX.DistortionAmplitude;
					distortionFrequency = fluidFX.DistortionFrequency;
					colorSaturation = fluidFX.ColorSaturation;
					colorFilter = new Vector3((float)((byte)fluidFX.ColorFilter.Red) / 255f * vector.X, (float)((byte)fluidFX.ColorFilter.Green) / 255f * vector.Y, (float)((byte)fluidFX.ColorFilter.Blue) / 255f * vector.Z);
				}
				this.PostEffectRenderer.UpdateDistortion(time, distortionAmplitude, distortionFrequency);
				this.PostEffectRenderer.UpdateColorFilters(colorFilter, colorSaturation);
				bool isBloomEnabled = this.PostEffectRenderer.IsBloomEnabled;
				if (isBloomEnabled)
				{
					bool flag3 = isUnderWater && this.UseBloomUnderwater;
					if (flag3)
					{
						this.PostEffectRenderer.SetBloomOnPowIntensity(this.UnderwaterBloomIntensity);
						this.PostEffectRenderer.SetBloomOnPowPower(this.UnderwaterBloomPower);
					}
					else
					{
						this.PostEffectRenderer.SetBloomOnPowIntensity(this.DefaultBloomIntensity);
						this.PostEffectRenderer.SetBloomOnPowPower(this.DefaultBloomPower);
					}
					bool allowBloom = !isUnderWater || this.UseBloomUnderwater;
					bool isSunVisible = this.WeatherModule.SkyRenderer.SunNeedsDrawing(this.SceneRenderer.Data.SunPositionWS, this.SceneRenderer.Data.CameraDirection, this.WeatherModule.SunScale);
					bool isMoonVisible = this.WeatherModule.SkyRenderer.MoonNeedsDrawing(this.SceneRenderer.Data.SunPositionWS, this.SceneRenderer.Data.CameraDirection, this.WeatherModule.MoonScale);
					this.PostEffectRenderer.UpdateBloom(this.WeatherModule.SkyRenderer.SunMVPMatrix, isSunVisible, allowBloom, this.SceneRenderer.Data.SunColor, isMoonVisible, this.WeatherModule.MoonColor, this.SceneRenderer.Data.Time);
				}
				bool isTemporalAAEnabled = this.PostEffectRenderer.IsTemporalAAEnabled;
				if (isTemporalAAEnabled)
				{
					this.PostEffectRenderer.UpdateTemporalAA(this.SceneRenderer.Data.HasCameraMoved);
				}
				bool isDepthOfFieldEnabled = this.PostEffectRenderer.IsDepthOfFieldEnabled;
				if (isDepthOfFieldEnabled)
				{
					this.PostEffectRenderer.UpdateDepthOfField(this.SceneRenderer.Data.ProjectionMatrix);
				}
				this.PostEffectRenderer.Draw(rtstore.SceneColor.GetTexture(RenderTarget.Target.Color0), rtstore.Distortion.GetTexture(RenderTarget.Target.Color0), this.Engine.Window.Viewport.Width, this.Engine.Window.Viewport.Height, this.ResolutionScale, null);
				this.Engine.Profiling.StopMeasure(77);
			}
		}

		// Token: 0x06004323 RID: 17187 RVA: 0x000D3F6C File Offset: 0x000D216C
		public void DrawAfterPostEffect()
		{
			bool flag = !this.IsPlaying;
			if (!flag)
			{
				GLFunctions gl = this.Engine.Graphics.GL;
				BasicProgram basicProgram = this.Engine.Graphics.GPUProgramStore.BasicProgram;
				gl.UseProgram(basicProgram);
				this.Engine.Profiling.StartMeasure(83);
				bool flag2 = this.DamageEffectModule.NeedsDrawing();
				if (flag2)
				{
					basicProgram.Color.SetValue(this.Engine.Graphics.WhiteColor);
					this.DamageEffectModule.Draw();
				}
				bool flag3 = this.ScreenEffectStoreModule.NeedsDrawing();
				if (flag3)
				{
					this.ScreenEffectStoreModule.Draw();
				}
				this.Engine.Profiling.StopMeasure(83);
				bool mapNeedsDrawing = this.WorldMapModule.MapNeedsDrawing;
				if (mapNeedsDrawing)
				{
					this.WorldMapModule.DrawMap();
				}
				TextProgram textProgram = this.Engine.Graphics.GPUProgramStore.TextProgram;
				bool isVisible = this.ProfilingModule.IsVisible;
				if (isVisible)
				{
					gl.UseProgram(textProgram);
					textProgram.FillThreshold.SetValue(0f);
					textProgram.OutlineThreshold.SetValue(0f);
					textProgram.OutlineBlurThreshold.SetValue(0f);
					textProgram.OutlineOffset.SetValue(Vector2.Zero);
					textProgram.FogParams.SetValue(Vector4.Zero);
				}
				RenderTargetStore rtstore = this.Engine.Graphics.RTStore;
				bool drawOcclusionMap = this.DrawOcclusionMap;
				if (drawOcclusionMap)
				{
					float opacity = (float)this._debugDrawMapOpacityStep / 5f;
					this.Engine.OcclusionCulling.DebugDrawOcclusionMap(opacity, this._debugDrawMapLevel);
				}
				else
				{
					bool debugMap = this.DebugMap;
					if (debugMap)
					{
						this.UpdateDebugDrawMap();
						float opacity2 = (float)this._debugDrawMapOpacityStep / 5f;
						rtstore.DebugDrawMaps(this._activeDebugMapsNames, this._debugMapVerticalDisplay, opacity2, this._debugDrawMapLevel, this._debugTextureArrayActiveLayer);
					}
				}
				bool isVisible2 = this.ProfilingModule.IsVisible;
				if (isVisible2)
				{
					gl.UseProgram(basicProgram);
					gl.BindTexture(GL.TEXTURE_2D, this.Engine.Graphics.WhitePixelTexture.GLTexture);
					basicProgram.Opacity.SetValue(0.8f);
					this.ProfilingModule.DrawGraphsData();
					gl.UseProgram(this.Engine.Graphics.GPUProgramStore.Batcher2DProgram);
					this.ProfilingModule.Draw();
				}
			}
		}

		// Token: 0x06004324 RID: 17188 RVA: 0x000D41ED File Offset: 0x000D23ED
		private void UpdateInterfaceData()
		{
			this.App.Interface.PrepareForDraw();
			this.InterfaceRenderPreviewModule.PrepareForDraw();
		}

		// Token: 0x06004325 RID: 17189 RVA: 0x000D4210 File Offset: 0x000D2410
		public void DrawAfterInterface()
		{
			bool flag = this.EditorWebViewModule.NeedsDrawing();
			if (flag)
			{
				this.EditorWebViewModule.Draw();
			}
			bool flag2 = this.InterfaceRenderPreviewModule.NeedsDrawing();
			if (flag2)
			{
				this.InterfaceRenderPreviewModule.Draw();
			}
		}

		// Token: 0x06004326 RID: 17190 RVA: 0x000D4254 File Offset: 0x000D2454
		public void RegisterCommand(string command, GameInstance.Command method)
		{
			this._commands.Add(command, method);
		}

		// Token: 0x06004327 RID: 17191 RVA: 0x000D4265 File Offset: 0x000D2465
		public void UnregisterCommand(string command)
		{
			this._commands.Remove(command);
		}

		// Token: 0x06004328 RID: 17192 RVA: 0x000D4278 File Offset: 0x000D2478
		public bool IsRegisteredCommand(string command)
		{
			return this._commands.ContainsKey(command);
		}

		// Token: 0x06004329 RID: 17193 RVA: 0x000D4298 File Offset: 0x000D2498
		public void ExecuteCommand(string str)
		{
			string[] array = str.Split(new char[]
			{
				' '
			}, StringSplitOptions.RemoveEmptyEntries);
			bool flag = array[0].StartsWith("..");
			if (flag)
			{
				array[0] = array[0].Substring("..".Length);
				this.ShortcutsModule.ExecuteMacro(array);
			}
			else
			{
				array[0] = array[0].Substring(".".Length);
				bool flag2 = array[0].Length == 0;
				if (flag2)
				{
					this.Chat.Log("Please enter a command after the . symbol");
				}
				else
				{
					GameInstance.Command command;
					bool flag3 = !this._commands.TryGetValue(array[0], out command);
					if (flag3)
					{
						this.Chat.Log("Unknown local command! '" + str + "'");
					}
					else
					{
						string[] array2 = new string[array.Length - 1];
						Array.Copy(array, 1, array2, 0, array.Length - 1);
						try
						{
							command(array2);
						}
						catch (InvalidCommandUsage invalidCommandUsage)
						{
							this.Chat.Log("Invalid usage!");
							object[] customAttributes = invalidCommandUsage.TargetSite.GetCustomAttributes(typeof(UsageAttribute), false);
							UsageAttribute usageAttribute = (customAttributes.Length != 0) ? ((UsageAttribute)customAttributes[0]) : ((UsageAttribute)Attribute.GetCustomAttribute(command.Method, typeof(UsageAttribute)));
							bool flag4 = usageAttribute != null;
							if (flag4)
							{
								this.Chat.Log(usageAttribute.ToString());
							}
						}
						catch (Exception ex)
						{
							GameInstance.Logger.Error(ex, "Exception running command '{0}':", new object[]
							{
								str
							});
							this.Chat.Log("Exception running command! '" + str + "': " + ex.Message);
						}
					}
				}
			}
		}

		// Token: 0x0600432A RID: 17194 RVA: 0x000D4470 File Offset: 0x000D2670
		public string GetCommandDescription(string commandName)
		{
			string result = "N/A";
			GameInstance.Command command;
			bool flag = this._commands.TryGetValue(commandName, out command);
			if (flag)
			{
				DescriptionAttribute descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(command.Method, typeof(DescriptionAttribute));
				bool flag2 = descriptionAttribute != null;
				if (flag2)
				{
					result = descriptionAttribute.Description;
				}
			}
			return result;
		}

		// Token: 0x0600432B RID: 17195 RVA: 0x000D44D0 File Offset: 0x000D26D0
		[Usage("help", new string[]
		{
			"[command]"
		})]
		[Description("Provides help for commands.")]
		public void HelpCommand(string[] args)
		{
			bool flag = args.Length == 0;
			if (flag)
			{
				List<string> list = new List<string>();
				foreach (string text in this._commands.Keys)
				{
					list.Add("- " + text + ": " + this.GetCommandDescription(text));
				}
				list.Sort();
				string message = "Available commands:\n" + string.Join("\n", list);
				this.Chat.Log(message);
			}
			else
			{
				GameInstance.Command command;
				bool flag2 = !this._commands.TryGetValue(args[0], out command);
				if (flag2)
				{
					this.Chat.Log("Unknown local command! '" + args[0] + "'");
				}
				else
				{
					UsageAttribute usageAttribute = (UsageAttribute)Attribute.GetCustomAttribute(command.Method, typeof(UsageAttribute));
					this.Chat.Log(((usageAttribute != null) ? usageAttribute.ToString() : null) ?? ("No usage info found for command '" + args[0] + "'"));
				}
			}
		}

		// Token: 0x170010DF RID: 4319
		// (get) Token: 0x0600432C RID: 17196 RVA: 0x000D4614 File Offset: 0x000D2814
		// (set) Token: 0x0600432D RID: 17197 RVA: 0x000D461C File Offset: 0x000D281C
		public TimeModule TimeModule { get; private set; }

		// Token: 0x170010E0 RID: 4320
		// (get) Token: 0x0600432E RID: 17198 RVA: 0x000D4625 File Offset: 0x000D2825
		// (set) Token: 0x0600432F RID: 17199 RVA: 0x000D462D File Offset: 0x000D282D
		public AudioModule AudioModule { get; private set; }

		// Token: 0x170010E1 RID: 4321
		// (get) Token: 0x06004330 RID: 17200 RVA: 0x000D4636 File Offset: 0x000D2836
		// (set) Token: 0x06004331 RID: 17201 RVA: 0x000D463E File Offset: 0x000D283E
		public MapModule MapModule { get; private set; }

		// Token: 0x170010E2 RID: 4322
		// (get) Token: 0x06004332 RID: 17202 RVA: 0x000D4647 File Offset: 0x000D2847
		// (set) Token: 0x06004333 RID: 17203 RVA: 0x000D464F File Offset: 0x000D284F
		public ItemLibraryModule ItemLibraryModule { get; private set; }

		// Token: 0x170010E3 RID: 4323
		// (get) Token: 0x06004334 RID: 17204 RVA: 0x000D4658 File Offset: 0x000D2858
		// (set) Token: 0x06004335 RID: 17205 RVA: 0x000D4660 File Offset: 0x000D2860
		public CharacterControllerModule CharacterControllerModule { get; private set; }

		// Token: 0x170010E4 RID: 4324
		// (get) Token: 0x06004336 RID: 17206 RVA: 0x000D4669 File Offset: 0x000D2869
		// (set) Token: 0x06004337 RID: 17207 RVA: 0x000D4671 File Offset: 0x000D2871
		public CameraModule CameraModule { get; private set; }

		// Token: 0x170010E5 RID: 4325
		// (get) Token: 0x06004338 RID: 17208 RVA: 0x000D467A File Offset: 0x000D287A
		// (set) Token: 0x06004339 RID: 17209 RVA: 0x000D4682 File Offset: 0x000D2882
		public CollisionModule CollisionModule { get; private set; }

		// Token: 0x170010E6 RID: 4326
		// (get) Token: 0x0600433A RID: 17210 RVA: 0x000D468B File Offset: 0x000D288B
		// (set) Token: 0x0600433B RID: 17211 RVA: 0x000D4693 File Offset: 0x000D2893
		public EntityStoreModule EntityStoreModule { get; private set; }

		// Token: 0x170010E7 RID: 4327
		// (get) Token: 0x0600433C RID: 17212 RVA: 0x000D469C File Offset: 0x000D289C
		// (set) Token: 0x0600433D RID: 17213 RVA: 0x000D46A4 File Offset: 0x000D28A4
		public InventoryModule InventoryModule { get; private set; }

		// Token: 0x170010E8 RID: 4328
		// (get) Token: 0x0600433E RID: 17214 RVA: 0x000D46AD File Offset: 0x000D28AD
		// (set) Token: 0x0600433F RID: 17215 RVA: 0x000D46B5 File Offset: 0x000D28B5
		public InteractionModule InteractionModule { get; private set; }

		// Token: 0x170010E9 RID: 4329
		// (get) Token: 0x06004340 RID: 17216 RVA: 0x000D46BE File Offset: 0x000D28BE
		// (set) Token: 0x06004341 RID: 17217 RVA: 0x000D46C6 File Offset: 0x000D28C6
		public BuilderToolsModule BuilderToolsModule { get; private set; }

		// Token: 0x170010EA RID: 4330
		// (get) Token: 0x06004342 RID: 17218 RVA: 0x000D46CF File Offset: 0x000D28CF
		// (set) Token: 0x06004343 RID: 17219 RVA: 0x000D46D7 File Offset: 0x000D28D7
		public MachinimaModule MachinimaModule { get; private set; }

		// Token: 0x170010EB RID: 4331
		// (get) Token: 0x06004344 RID: 17220 RVA: 0x000D46E0 File Offset: 0x000D28E0
		// (set) Token: 0x06004345 RID: 17221 RVA: 0x000D46E8 File Offset: 0x000D28E8
		public FXModule FXModule { get; private set; }

		// Token: 0x170010EC RID: 4332
		// (get) Token: 0x06004346 RID: 17222 RVA: 0x000D46F1 File Offset: 0x000D28F1
		// (set) Token: 0x06004347 RID: 17223 RVA: 0x000D46F9 File Offset: 0x000D28F9
		public TrailStoreModule TrailStoreModule { get; private set; }

		// Token: 0x170010ED RID: 4333
		// (get) Token: 0x06004348 RID: 17224 RVA: 0x000D4702 File Offset: 0x000D2902
		// (set) Token: 0x06004349 RID: 17225 RVA: 0x000D470A File Offset: 0x000D290A
		public ParticleSystemStoreModule ParticleSystemStoreModule { get; private set; }

		// Token: 0x170010EE RID: 4334
		// (get) Token: 0x0600434A RID: 17226 RVA: 0x000D4713 File Offset: 0x000D2913
		// (set) Token: 0x0600434B RID: 17227 RVA: 0x000D471B File Offset: 0x000D291B
		public ScreenEffectStoreModule ScreenEffectStoreModule { get; private set; }

		// Token: 0x170010EF RID: 4335
		// (get) Token: 0x0600434C RID: 17228 RVA: 0x000D4724 File Offset: 0x000D2924
		// (set) Token: 0x0600434D RID: 17229 RVA: 0x000D472C File Offset: 0x000D292C
		public WeatherModule WeatherModule { get; private set; }

		// Token: 0x170010F0 RID: 4336
		// (get) Token: 0x0600434E RID: 17230 RVA: 0x000D4735 File Offset: 0x000D2935
		// (set) Token: 0x0600434F RID: 17231 RVA: 0x000D473D File Offset: 0x000D293D
		public AmbienceFXModule AmbienceFXModule { get; private set; }

		// Token: 0x170010F1 RID: 4337
		// (get) Token: 0x06004350 RID: 17232 RVA: 0x000D4746 File Offset: 0x000D2946
		// (set) Token: 0x06004351 RID: 17233 RVA: 0x000D474E File Offset: 0x000D294E
		public DamageEffectModule DamageEffectModule { get; private set; }

		// Token: 0x170010F2 RID: 4338
		// (get) Token: 0x06004352 RID: 17234 RVA: 0x000D4757 File Offset: 0x000D2957
		// (set) Token: 0x06004353 RID: 17235 RVA: 0x000D475F File Offset: 0x000D295F
		public ClientFeatureModule ClientFeatureModule { get; private set; }

		// Token: 0x170010F3 RID: 4339
		// (get) Token: 0x06004354 RID: 17236 RVA: 0x000D4768 File Offset: 0x000D2968
		// (set) Token: 0x06004355 RID: 17237 RVA: 0x000D4770 File Offset: 0x000D2970
		public ProfilingModule ProfilingModule { get; private set; }

		// Token: 0x170010F4 RID: 4340
		// (get) Token: 0x06004356 RID: 17238 RVA: 0x000D4779 File Offset: 0x000D2979
		// (set) Token: 0x06004357 RID: 17239 RVA: 0x000D4781 File Offset: 0x000D2981
		public ShortcutsModule ShortcutsModule { get; private set; }

		// Token: 0x170010F5 RID: 4341
		// (get) Token: 0x06004358 RID: 17240 RVA: 0x000D478A File Offset: 0x000D298A
		// (set) Token: 0x06004359 RID: 17241 RVA: 0x000D4792 File Offset: 0x000D2992
		public ImmersiveScreenModule ImmersiveScreenModule { get; private set; }

		// Token: 0x170010F6 RID: 4342
		// (get) Token: 0x0600435A RID: 17242 RVA: 0x000D479B File Offset: 0x000D299B
		// (set) Token: 0x0600435B RID: 17243 RVA: 0x000D47A3 File Offset: 0x000D29A3
		public InterfaceRenderPreviewModule InterfaceRenderPreviewModule { get; private set; }

		// Token: 0x170010F7 RID: 4343
		// (get) Token: 0x0600435C RID: 17244 RVA: 0x000D47AC File Offset: 0x000D29AC
		// (set) Token: 0x0600435D RID: 17245 RVA: 0x000D47B4 File Offset: 0x000D29B4
		public EditorWebViewModule EditorWebViewModule { get; private set; }

		// Token: 0x170010F8 RID: 4344
		// (get) Token: 0x0600435E RID: 17246 RVA: 0x000D47BD File Offset: 0x000D29BD
		// (set) Token: 0x0600435F RID: 17247 RVA: 0x000D47C5 File Offset: 0x000D29C5
		public WorldMapModule WorldMapModule { get; private set; }

		// Token: 0x170010F9 RID: 4345
		// (get) Token: 0x06004360 RID: 17248 RVA: 0x000D47CE File Offset: 0x000D29CE
		// (set) Token: 0x06004361 RID: 17249 RVA: 0x000D47D6 File Offset: 0x000D29D6
		public DebugDisplayModule DebugDisplayModule { get; private set; }

		// Token: 0x06004362 RID: 17250 RVA: 0x000D47E0 File Offset: 0x000D29E0
		private void InitModules()
		{
			this.TimeModule = this.AddModule<TimeModule>(new TimeModule(this));
			this.AudioModule = new AudioModule(this);
			this.MapModule = this.AddModule<MapModule>(new MapModule(this));
			this.ItemLibraryModule = this.AddModule<ItemLibraryModule>(new ItemLibraryModule(this));
			this.EditorWebViewModule = this.AddModule<EditorWebViewModule>(new EditorWebViewModule(this));
			this.CharacterControllerModule = this.AddModule<CharacterControllerModule>(new CharacterControllerModule(this));
			this.CameraModule = this.AddModule<CameraModule>(new CameraModule(this));
			this.EntityStoreModule = this.AddModule<EntityStoreModule>(new EntityStoreModule(this));
			this.CollisionModule = this.AddModule<CollisionModule>(new CollisionModule(this));
			this._networkModule = this.AddModule<NetworkModule>(new NetworkModule(this));
			this._movementSoundModule = this.AddModule<MovementSoundModule>(new MovementSoundModule(this));
			this.InventoryModule = this.AddModule<InventoryModule>(new InventoryModule(this));
			this.InteractionModule = this.AddModule<InteractionModule>(new InteractionModule(this));
			this.BuilderToolsModule = this.AddModule<BuilderToolsModule>(new BuilderToolsModule(this));
			this.MachinimaModule = this.AddModule<MachinimaModule>(new MachinimaModule(this));
			this.FXModule = this.AddModule<FXModule>(new FXModule(this));
			this.TrailStoreModule = this.AddModule<TrailStoreModule>(new TrailStoreModule(this));
			this.WeatherModule = this.AddModule<WeatherModule>(new WeatherModule(this));
			this.ScreenEffectStoreModule = this.AddModule<ScreenEffectStoreModule>(new ScreenEffectStoreModule(this));
			this.ParticleSystemStoreModule = this.AddModule<ParticleSystemStoreModule>(new ParticleSystemStoreModule(this));
			this.AmbienceFXModule = this.AddModule<AmbienceFXModule>(new AmbienceFXModule(this));
			this.DamageEffectModule = this.AddModule<DamageEffectModule>(new DamageEffectModule(this));
			this.ClientFeatureModule = this.AddModule<ClientFeatureModule>(new ClientFeatureModule(this));
			this._autoCameraModule = this.AddModule<AutoCameraModule>(new AutoCameraModule(this));
			this._debugCommandsModule = this.AddModule<DebugCommandsModule>(new DebugCommandsModule(this));
			this.ProfilingModule = this.AddModule<ProfilingModule>(new ProfilingModule(this));
			this.ShortcutsModule = this.AddModule<ShortcutsModule>(new ShortcutsModule(this));
			this.ImmersiveScreenModule = this.AddModule<ImmersiveScreenModule>(new ImmersiveScreenModule(this));
			this.InterfaceRenderPreviewModule = this.AddModule<InterfaceRenderPreviewModule>(new InterfaceRenderPreviewModule(this, false));
			this.WorldMapModule = this.AddModule<WorldMapModule>(new WorldMapModule(this));
			this.DebugDisplayModule = this.AddModule<DebugDisplayModule>(new DebugDisplayModule(this));
		}

		// Token: 0x06004363 RID: 17251 RVA: 0x000D4A34 File Offset: 0x000D2C34
		public T AddModule<T>(T module) where T : Module
		{
			this._modules.Add(module);
			return module;
		}

		// Token: 0x06004364 RID: 17252 RVA: 0x000D4A5C File Offset: 0x000D2C5C
		private void InitRenderingOptions()
		{
			bool isGPULowEnd = this.Engine.Graphics.IsGPULowEnd;
			this.TrailerMode.UseDof = false;
			this.TrailerMode.DofQuality = 3;
			this.TrailerMode.UseBloom = true;
			this.TrailerMode.BloomQuality = 1;
			this.TrailerMode.UseSunshaft = true;
			this.TrailerMode.WaterQuality = 3;
			this.TrailerMode.SsaoQuality = 2;
			this.TrailerMode.RenderScale = 200;
			this.TrailerMode.UseFxaaSharpened = true;
			this.TrailerMode.UseFxaa = true;
			this.TrailerMode.UseTaa = false;
			this.TrailerMode.UseFoliageFading = false;
			this.TrailerMode.UseLod = false;
			this.TrailerMode.LodDistanceStart = 160U;
			this.CutscenesMode.UseDof = false;
			this.CutscenesMode.DofQuality = (isGPULowEnd ? 1 : 3);
			this.CutscenesMode.UseBloom = true;
			this.CutscenesMode.BloomQuality = 1;
			this.CutscenesMode.UseSunshaft = true;
			this.CutscenesMode.WaterQuality = 3;
			this.CutscenesMode.SsaoQuality = (isGPULowEnd ? 0 : 1);
			this.CutscenesMode.RenderScale = 100;
			this.CutscenesMode.UseFxaaSharpened = true;
			this.CutscenesMode.UseFxaa = true;
			this.CutscenesMode.UseTaa = false;
			this.CutscenesMode.UseFoliageFading = true;
			this.CutscenesMode.UseLod = true;
			this.CutscenesMode.LodDistanceStart = 190U;
			this.IngameMode.UseDof = false;
			this.IngameMode.DofQuality = (isGPULowEnd ? 1 : 3);
			this.IngameMode.UseBloom = true;
			this.IngameMode.BloomQuality = 0;
			this.IngameMode.UseSunshaft = !isGPULowEnd;
			this.IngameMode.WaterQuality = (isGPULowEnd ? 2 : 3);
			this.IngameMode.SsaoQuality = (isGPULowEnd ? 0 : 1);
			this.IngameMode.RenderScale = (isGPULowEnd ? 70 : 100);
			this.IngameMode.UseFxaaSharpened = !isGPULowEnd;
			this.IngameMode.UseFxaa = true;
			this.IngameMode.UseTaa = isGPULowEnd;
			this.IngameMode.UseFoliageFading = true;
			this.IngameMode.UseLod = true;
			this.IngameMode.LodDistanceStart = 160U;
			this.LowEndGPUMode.UseDof = false;
			this.LowEndGPUMode.DofQuality = 1;
			this.LowEndGPUMode.UseBloom = true;
			this.LowEndGPUMode.BloomQuality = 0;
			this.LowEndGPUMode.UseSunshaft = false;
			this.LowEndGPUMode.WaterQuality = 2;
			this.LowEndGPUMode.SsaoQuality = 0;
			this.LowEndGPUMode.RenderScale = 70;
			this.LowEndGPUMode.UseFxaaSharpened = false;
			this.LowEndGPUMode.UseFxaa = true;
			this.LowEndGPUMode.UseTaa = true;
			this.LowEndGPUMode.UseFoliageFading = true;
			this.LowEndGPUMode.UseLod = true;
			this.LowEndGPUMode.LodDistanceStart = 160U;
		}

		// Token: 0x06004365 RID: 17253 RVA: 0x000D4D60 File Offset: 0x000D2F60
		public void SetRenderingOptions(ref RenderingOptions renderingOptions)
		{
			this.PostEffectRenderer.UseDepthOfField(renderingOptions.UseDof);
			this.PostEffectRenderer.SetDepthOfFieldVersion(renderingOptions.DofQuality);
			this.PostEffectRenderer.UseBloom(renderingOptions.UseBloom);
			this.PostEffectRenderer.SetBloomVersion(renderingOptions.BloomQuality);
			this.PostEffectRenderer.UseBloomSunShaft(renderingOptions.UseSunshaft);
			this.SetWaterQuality(renderingOptions.WaterQuality);
			this.SceneRenderer.SetUseSSAO(true, true, renderingOptions.SsaoQuality);
			float resolutionScale = (float)(this.App.Settings.AutomaticRenderScale ? renderingOptions.RenderScale : this.App.Settings.RenderScale) * 0.01f;
			this.SetResolutionScale(resolutionScale);
			this.PostEffectRenderer.UseFXAASharpened(renderingOptions.UseFxaaSharpened, -1f);
			this.PostEffectRenderer.UseFXAA(renderingOptions.UseFxaa);
			this.PostEffectRenderer.UseTemporalAA(renderingOptions.UseTaa);
			this.SetChunkUseFoliageFading(renderingOptions.UseFoliageFading);
			this.SetUseLOD(renderingOptions.UseLod);
			this.SetLODDistance(renderingOptions.LodDistanceStart, 0U);
		}

		// Token: 0x06004366 RID: 17254 RVA: 0x000D4E86 File Offset: 0x000D3086
		public void RegisterHashForServerAsset(string serverAssetPath, string hash)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this.HashesByServerAssetPath[serverAssetPath] = hash;
		}

		// Token: 0x06004367 RID: 17255 RVA: 0x000D4EA4 File Offset: 0x000D30A4
		public void RemoveHashForServerAsset(string serverAssetPath)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			string text;
			this.HashesByServerAssetPath.TryRemove(serverAssetPath, ref text);
		}

		// Token: 0x0400207B RID: 8315
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400207C RID: 8316
		public float TimeDilationModifier = 1f;

		// Token: 0x0400207D RID: 8317
		public int ServerUpdatesPerSecond = 30;

		// Token: 0x0400207E RID: 8318
		private GameInstance.GameInstanceStage _stage = GameInstance.GameInstanceStage.WaitingForJoinWorldPacket;

		// Token: 0x0400207F RID: 8319
		private bool _isAwaitingJoiningWorldFade = false;

		// Token: 0x04002084 RID: 8324
		public readonly App App;

		// Token: 0x04002085 RID: 8325
		public readonly Engine Engine;

		// Token: 0x04002086 RID: 8326
		public readonly Input Input;

		// Token: 0x04002087 RID: 8327
		private const int ConsumableDownDuration = 200;

		// Token: 0x04002088 RID: 8328
		private readonly Stopwatch _consumableDownTime = new Stopwatch();

		// Token: 0x04002089 RID: 8329
		public readonly Chat Chat;

		// Token: 0x0400208A RID: 8330
		public readonly Notifications Notifications;

		// Token: 0x0400208B RID: 8331
		[Obsolete]
		public readonly HitDetection HitDetection;

		// Token: 0x0400208C RID: 8332
		public bool RenderPlayers = true;

		// Token: 0x0400208E RID: 8334
		public readonly ConnectionToServer Connection;

		// Token: 0x0400208F RID: 8335
		private readonly PacketHandler _packetHandler;

		// Token: 0x04002090 RID: 8336
		private readonly Stopwatch _stopwatchSinceJoiningServer = new Stopwatch();

		// Token: 0x04002092 RID: 8338
		private float _tickAccumulator;

		// Token: 0x04002093 RID: 8339
		public SceneRenderer SceneRenderer;

		// Token: 0x04002094 RID: 8340
		private SceneView _cameraSceneView;

		// Token: 0x04002095 RID: 8341
		private SceneView _sunSceneView;

		// Token: 0x04002096 RID: 8342
		public GameInstance.WireframePass Wireframe;

		// Token: 0x04002098 RID: 8344
		private Vector3 _foliageInteractionParams;

		// Token: 0x04002099 RID: 8345
		private LightData[] GlobalLightData = new LightData[1024];

		// Token: 0x0400209A RID: 8346
		private int GlobalLightDataCount;

		// Token: 0x0400209B RID: 8347
		public bool CullUndergroundChunkShadowCasters = true;

		// Token: 0x0400209C RID: 8348
		public bool CullUndergroundEntityShadowCasters = true;

		// Token: 0x0400209D RID: 8349
		public bool CullSmallEntityShadowCasters = true;

		// Token: 0x040020A1 RID: 8353
		private bool _isCameraUnderwater;

		// Token: 0x040020A2 RID: 8354
		public bool UseAnimationLOD = true;

		// Token: 0x040020A3 RID: 8355
		public readonly float ResolutionScaleMin = 0.25f;

		// Token: 0x040020A4 RID: 8356
		public readonly float ResolutionScaleMax = 4f;

		// Token: 0x040020A6 RID: 8358
		public bool DrawOcclusionMap;

		// Token: 0x040020A7 RID: 8359
		public bool DebugDrawOccludeeChunks;

		// Token: 0x040020A8 RID: 8360
		public bool DebugDrawOccludeeEntities;

		// Token: 0x040020A9 RID: 8361
		public bool DebugDrawOccludeeLights;

		// Token: 0x040020AA RID: 8362
		public bool DebugDrawOccludeeParticles;

		// Token: 0x040020AB RID: 8363
		private int _requestedOpaqueChunkOccludersCount = 15;

		// Token: 0x040020AC RID: 8364
		private bool _useChunkOccluderPlanes = true;

		// Token: 0x040020AD RID: 8365
		private bool _useOpaqueChunkOccluders = true;

		// Token: 0x040020AE RID: 8366
		private bool _useAlphaTestedChunkOccluders = true;

		// Token: 0x040020AF RID: 8367
		private bool _useOcclusionCullingReprojection = true;

		// Token: 0x040020B0 RID: 8368
		private bool _useOcclusionCullingReprojectionHoleFilling = true;

		// Token: 0x040020B1 RID: 8369
		private Vector4[] _previousFrameInvalidScreenAreas = new Vector4[10];

		// Token: 0x040020B2 RID: 8370
		public bool RenderTimePaused;

		// Token: 0x040020B3 RID: 8371
		private bool _testBranch;

		// Token: 0x040020B4 RID: 8372
		public bool DebugEntitiesZTest;

		// Token: 0x040020B5 RID: 8373
		public bool DebugCollisionOnlyCollided;

		// Token: 0x040020B6 RID: 8374
		private bool _debugParticleOverdraw;

		// Token: 0x040020B7 RID: 8375
		private bool _debugParticleTexture;

		// Token: 0x040020B8 RID: 8376
		private bool _debugParticleBoundingVolume;

		// Token: 0x040020B9 RID: 8377
		private bool _debugParticleZTestEnabled = true;

		// Token: 0x040020BA RID: 8378
		private bool _debugParticleUVMotion;

		// Token: 0x040020BB RID: 8379
		public bool DebugDrawLight;

		// Token: 0x040020BC RID: 8380
		private bool _debugLightClusters;

		// Token: 0x040020BD RID: 8381
		private bool _chunkUseFoliageFading = true;

		// Token: 0x040020BE RID: 8382
		private bool _debugChunkBoundaries;

		// Token: 0x040020BF RID: 8383
		public bool DebugMap;

		// Token: 0x040020C0 RID: 8384
		private const int DebugDrawMapLevelMax = 8;

		// Token: 0x040020C1 RID: 8385
		private const int DebugDrawMapOpacityStepMax = 5;

		// Token: 0x040020C2 RID: 8386
		private int _debugDrawMapLevel;

		// Token: 0x040020C3 RID: 8387
		private int _debugDrawMapOpacityStep = 5;

		// Token: 0x040020C4 RID: 8388
		private int _debugTextureArrayActiveLayer;

		// Token: 0x040020C5 RID: 8389
		private int _debugTextureArrayLayerCount;

		// Token: 0x040020C6 RID: 8390
		private bool _debugMapVerticalDisplay;

		// Token: 0x040020C7 RID: 8391
		private string[] _activeDebugMapsNames;

		// Token: 0x040020C8 RID: 8392
		public bool UseLessSkyAmbientAtNoon = true;

		// Token: 0x040020C9 RID: 8393
		public float SkyAmbientIntensityAtNoon = 0f;

		// Token: 0x040020CA RID: 8394
		public float SkyAmbientIntensity = 0.12f;

		// Token: 0x040020CB RID: 8395
		private Texture _fogNoise;

		// Token: 0x040020CC RID: 8396
		private bool _useMoodFog;

		// Token: 0x040020CD RID: 8397
		private bool _useCustomMoodFog;

		// Token: 0x040020CE RID: 8398
		private float _customDensity = 1f;

		// Token: 0x040020CF RID: 8399
		private float _customHeightFalloff = 8f;

		// Token: 0x040020D0 RID: 8400
		private float _densityVariationScale;

		// Token: 0x040020D1 RID: 8401
		private float _fogSpeedFactor;

		// Token: 0x040020D2 RID: 8402
		public bool UseSkyboxTest;

		// Token: 0x040020D3 RID: 8403
		private int _waterQuality;

		// Token: 0x040020D4 RID: 8404
		private Texture _waterNormals;

		// Token: 0x040020D5 RID: 8405
		private Texture _waterCaustics;

		// Token: 0x040020D6 RID: 8406
		private float _underwaterCausticsIntensity = 1f;

		// Token: 0x040020D7 RID: 8407
		private float _underwaterCausticsScale = 0.095f;

		// Token: 0x040020D8 RID: 8408
		private float _underwaterCausticsDistortion = 0.05f;

		// Token: 0x040020D9 RID: 8409
		private float _cloudsUVMotionScale = 30f;

		// Token: 0x040020DA RID: 8410
		private float _cloudsUVMotionStrength = 0.0005f;

		// Token: 0x040020DB RID: 8411
		private float _cloudsShadowsIntensity = 0.25f;

		// Token: 0x040020DC RID: 8412
		private float _cloudsShadowsScale = 0.005f;

		// Token: 0x040020DD RID: 8413
		private float _cloudsShadowsBlurriness = 3.5f;

		// Token: 0x040020DE RID: 8414
		private float _cloudsShadowsSpeed = 1f;

		// Token: 0x040020DF RID: 8415
		private GLTexture _projectionTexture;

		// Token: 0x040020E0 RID: 8416
		private Texture _flowMap;

		// Token: 0x040020E1 RID: 8417
		private Texture _glowMask;

		// Token: 0x040020E2 RID: 8418
		public bool UseBloomUnderwater = true;

		// Token: 0x040020E3 RID: 8419
		public float UnderwaterBloomIntensity = 0.25f;

		// Token: 0x040020E4 RID: 8420
		public float UnderwaterBloomPower = 8f;

		// Token: 0x040020E5 RID: 8421
		public float DefaultBloomIntensity = 0.04f;

		// Token: 0x040020E6 RID: 8422
		public float DefaultBloomPower = 5f;

		// Token: 0x040020E7 RID: 8423
		private bool _useVolumetricSunshaft;

		// Token: 0x040020E8 RID: 8424
		public int ForceFieldTest;

		// Token: 0x040020E9 RID: 8425
		public bool ForceFieldOptionAnimation = true;

		// Token: 0x040020EA RID: 8426
		public bool ForceFieldOptionOutline = true;

		// Token: 0x040020EB RID: 8427
		public bool ForceFieldOptionDistortion = true;

		// Token: 0x040020EC RID: 8428
		public bool ForceFieldOptionColor = true;

		// Token: 0x040020ED RID: 8429
		public int ForceFieldCount = 1;

		// Token: 0x040020EE RID: 8430
		private const int MaxFieldMatricesCount = 20;

		// Token: 0x040020EF RID: 8431
		private Matrix[] _forceFieldModelMatrices = new Matrix[20];

		// Token: 0x040020F0 RID: 8432
		private Matrix[] _forceFieldNormalMatrices = new Matrix[20];

		// Token: 0x040020F1 RID: 8433
		private Texture _forceFieldNormalMap;

		// Token: 0x040020F2 RID: 8434
		private bool _useChunksOIT;

		// Token: 0x040020F3 RID: 8435
		private int _oitRes;

		// Token: 0x040020F4 RID: 8436
		private const uint RenderPassesCount = 13U;

		// Token: 0x040020F6 RID: 8438
		private readonly bool[] _renderPassStates = new bool[13];

		// Token: 0x040020F7 RID: 8439
		private Texture _cubemap;

		// Token: 0x040020F8 RID: 8440
		private Mesh _cube;

		// Token: 0x040020F9 RID: 8441
		private Vector2 _atlasSizeFactor0;

		// Token: 0x040020FA RID: 8442
		private Vector2 _atlasSizeFactor1;

		// Token: 0x040020FB RID: 8443
		private Vector2 _atlasSizeFactor2;

		// Token: 0x040020FD RID: 8445
		public bool UseLocalPlayerOccluder = true;

		// Token: 0x040020FE RID: 8446
		public bool UseOcclusionCullingForEntities = true;

		// Token: 0x040020FF RID: 8447
		public bool UseOcclusionCullingForEntitiesAnimations = true;

		// Token: 0x04002100 RID: 8448
		public bool UseOcclusionCullingForLights = true;

		// Token: 0x04002101 RID: 8449
		public bool UseOcclusionCullingForParticles = true;

		// Token: 0x04002102 RID: 8450
		public const string LocalCommandPrefix = ".";

		// Token: 0x04002103 RID: 8451
		public const string ServerCommandPrefix = "/";

		// Token: 0x04002104 RID: 8452
		public const string MacroCommandPrefix = "..";

		// Token: 0x04002105 RID: 8453
		private readonly Dictionary<string, GameInstance.Command> _commands = new Dictionary<string, GameInstance.Command>();

		// Token: 0x04002106 RID: 8454
		private readonly List<Module> _modules = new List<Module>();

		// Token: 0x04002122 RID: 8482
		private NetworkModule _networkModule;

		// Token: 0x04002123 RID: 8483
		private MovementSoundModule _movementSoundModule;

		// Token: 0x04002124 RID: 8484
		private AutoCameraModule _autoCameraModule;

		// Token: 0x04002125 RID: 8485
		private DebugCommandsModule _debugCommandsModule;

		// Token: 0x04002126 RID: 8486
		public RenderingOptions TrailerMode;

		// Token: 0x04002127 RID: 8487
		public RenderingOptions CutscenesMode;

		// Token: 0x04002128 RID: 8488
		public RenderingOptions IngameMode;

		// Token: 0x04002129 RID: 8489
		public RenderingOptions LowEndGPUMode;

		// Token: 0x0400212A RID: 8490
		public readonly ConcurrentDictionary<string, string> HashesByServerAssetPath = new ConcurrentDictionary<string, string>();

		// Token: 0x02000DA2 RID: 3490
		private enum GameInstanceStage
		{
			// Token: 0x040042D0 RID: 17104
			WaitingForJoinWorldPacket,
			// Token: 0x040042D1 RID: 17105
			WaitingForNearbyChunks,
			// Token: 0x040042D2 RID: 17106
			WorldJoined
		}

		// Token: 0x02000DA3 RID: 3491
		public enum WireframePass
		{
			// Token: 0x040042D4 RID: 17108
			Off,
			// Token: 0x040042D5 RID: 17109
			OnAll,
			// Token: 0x040042D6 RID: 17110
			OnEntities,
			// Token: 0x040042D7 RID: 17111
			OnMapOpaque,
			// Token: 0x040042D8 RID: 17112
			OnMapAlphaTested,
			// Token: 0x040042D9 RID: 17113
			OnMapAnim,
			// Token: 0x040042DA RID: 17114
			OnMapAlphaBlend,
			// Token: 0x040042DB RID: 17115
			OnSky
		}

		// Token: 0x02000DA4 RID: 3492
		private enum RenderPassId
		{
			// Token: 0x040042DD RID: 17117
			ShadowMap,
			// Token: 0x040042DE RID: 17118
			FirstPerson,
			// Token: 0x040042DF RID: 17119
			MapNear_Opaque,
			// Token: 0x040042E0 RID: 17120
			MapNear_AlphaTested,
			// Token: 0x040042E1 RID: 17121
			MapAnimated,
			// Token: 0x040042E2 RID: 17122
			MapFar_Opaque,
			// Token: 0x040042E3 RID: 17123
			MapFar_AlphaTested,
			// Token: 0x040042E4 RID: 17124
			Entities,
			// Token: 0x040042E5 RID: 17125
			Sky,
			// Token: 0x040042E6 RID: 17126
			MapAlphaBlended,
			// Token: 0x040042E7 RID: 17127
			VFX,
			// Token: 0x040042E8 RID: 17128
			Nameplates,
			// Token: 0x040042E9 RID: 17129
			PostFX,
			// Token: 0x040042EA RID: 17130
			Max
		}

		// Token: 0x02000DA5 RID: 3493
		public enum RenderingProfile
		{
			// Token: 0x040042EC RID: 17132
			FullFrame,
			// Token: 0x040042ED RID: 17133
			QueuedActions,
			// Token: 0x040042EE RID: 17134
			OnNewFrame,
			// Token: 0x040042EF RID: 17135
			ChunksPrepare,
			// Token: 0x040042F0 RID: 17136
			EntitiesPrepare,
			// Token: 0x040042F1 RID: 17137
			ModulesTick,
			// Token: 0x040042F2 RID: 17138
			ModulesPreUpdate,
			// Token: 0x040042F3 RID: 17139
			InteractionModule,
			// Token: 0x040042F4 RID: 17140
			LightsPrepare,
			// Token: 0x040042F5 RID: 17141
			ChunksGather,
			// Token: 0x040042F6 RID: 17142
			ChunksPrepareForDraw,
			// Token: 0x040042F7 RID: 17143
			AnimationUpdateEarly,
			// Token: 0x040042F8 RID: 17144
			OcclusionSetup,
			// Token: 0x040042F9 RID: 17145
			OcclusionBuildMap,
			// Token: 0x040042FA RID: 17146
			OcclusionRenderOccluders,
			// Token: 0x040042FB RID: 17147
			OcclusionReproject,
			// Token: 0x040042FC RID: 17148
			OcclusionCreateHiZ,
			// Token: 0x040042FD RID: 17149
			OcclusionPrepareOccludees,
			// Token: 0x040042FE RID: 17150
			OcclusionTestOccludees,
			// Token: 0x040042FF RID: 17151
			ModulesUpdate,
			// Token: 0x04004300 RID: 17152
			MapModuleUpdate,
			// Token: 0x04004301 RID: 17153
			AmbienceUpdate,
			// Token: 0x04004302 RID: 17154
			WeatherUpdate,
			// Token: 0x04004303 RID: 17155
			EntityStoreModuleUpdate,
			// Token: 0x04004304 RID: 17156
			UpdateSceneData,
			// Token: 0x04004305 RID: 17157
			ChunksShadowGather,
			// Token: 0x04004306 RID: 17158
			AnimatedChunksGather,
			// Token: 0x04004307 RID: 17159
			EntitiesGather,
			// Token: 0x04004308 RID: 17160
			FXUpdate,
			// Token: 0x04004309 RID: 17161
			FXGather,
			// Token: 0x0400430A RID: 17162
			FXUpdateSimulation,
			// Token: 0x0400430B RID: 17163
			AnimationUpdate,
			// Token: 0x0400430C RID: 17164
			EntitiesPrepareForDraw,
			// Token: 0x0400430D RID: 17165
			ShadowCascadesPrepare,
			// Token: 0x0400430E RID: 17166
			LightsUpdate,
			// Token: 0x0400430F RID: 17167
			LightsUpdateClear,
			// Token: 0x04004310 RID: 17168
			LightsUpdateClustering,
			// Token: 0x04004311 RID: 17169
			LightsUpdateRefine,
			// Token: 0x04004312 RID: 17170
			LightsUpdateFillGridData,
			// Token: 0x04004313 RID: 17171
			LightsUpdateSendDataToGPU,
			// Token: 0x04004314 RID: 17172
			FXPrepareForDraw,
			// Token: 0x04004315 RID: 17173
			FXSendDataToGPU,
			// Token: 0x04004316 RID: 17174
			Render,
			// Token: 0x04004317 RID: 17175
			OcclusionFetchResults,
			// Token: 0x04004318 RID: 17176
			AnalyzeSunOcclusion,
			// Token: 0x04004319 RID: 17177
			ReflectionBuildMips,
			// Token: 0x0400431A RID: 17178
			ShadowMap,
			// Token: 0x0400431B RID: 17179
			FirstPersonView,
			// Token: 0x0400431C RID: 17180
			MapNear,
			// Token: 0x0400431D RID: 17181
			MapAnimated,
			// Token: 0x0400431E RID: 17182
			MapFar,
			// Token: 0x0400431F RID: 17183
			Entities,
			// Token: 0x04004320 RID: 17184
			LinearZ,
			// Token: 0x04004321 RID: 17185
			LinearZDownsample,
			// Token: 0x04004322 RID: 17186
			ZDownsample,
			// Token: 0x04004323 RID: 17187
			EdgeDetection,
			// Token: 0x04004324 RID: 17188
			DeferredShadow,
			// Token: 0x04004325 RID: 17189
			SSAO,
			// Token: 0x04004326 RID: 17190
			BlurSSAOAndShadow,
			// Token: 0x04004327 RID: 17191
			VolumetricSunshafts,
			// Token: 0x04004328 RID: 17192
			Lights,
			// Token: 0x04004329 RID: 17193
			LightsStencil,
			// Token: 0x0400432A RID: 17194
			LightsFullRes,
			// Token: 0x0400432B RID: 17195
			LightsLowRes,
			// Token: 0x0400432C RID: 17196
			LightsMix,
			// Token: 0x0400432D RID: 17197
			ApplyDeferred,
			// Token: 0x0400432E RID: 17198
			ParticlesOpaque,
			// Token: 0x0400432F RID: 17199
			Weather,
			// Token: 0x04004330 RID: 17200
			MapAlphaBlended,
			// Token: 0x04004331 RID: 17201
			Transparency,
			// Token: 0x04004332 RID: 17202
			OITPrepass,
			// Token: 0x04004333 RID: 17203
			OITAccumulateQuarterRes,
			// Token: 0x04004334 RID: 17204
			OITAccumulateHalfRes,
			// Token: 0x04004335 RID: 17205
			OITAccumulateFullRes,
			// Token: 0x04004336 RID: 17206
			OITComposite,
			// Token: 0x04004337 RID: 17207
			Texts,
			// Token: 0x04004338 RID: 17208
			Distortion,
			// Token: 0x04004339 RID: 17209
			PostFX,
			// Token: 0x0400433A RID: 17210
			DepthOfField,
			// Token: 0x0400433B RID: 17211
			Bloom,
			// Token: 0x0400433C RID: 17212
			CombineAndFXAA,
			// Token: 0x0400433D RID: 17213
			TAA,
			// Token: 0x0400433E RID: 17214
			Blur,
			// Token: 0x0400433F RID: 17215
			ScreenFX,
			// Token: 0x04004340 RID: 17216
			MAX
		}

		// Token: 0x02000DA6 RID: 3494
		private enum TransparencyPassTextureUnit
		{
			// Token: 0x04004342 RID: 17218
			MapAtlas,
			// Token: 0x04004343 RID: 17219
			SceneDepthLowRes,
			// Token: 0x04004344 RID: 17220
			Normals,
			// Token: 0x04004345 RID: 17221
			Refraction,
			// Token: 0x04004346 RID: 17222
			SceneColor,
			// Token: 0x04004347 RID: 17223
			Caustics,
			// Token: 0x04004348 RID: 17224
			CloudShadow,
			// Token: 0x04004349 RID: 17225
			FXAtlasPointSampling,
			// Token: 0x0400434A RID: 17226
			FXAtlasLinearSampling,
			// Token: 0x0400434B RID: 17227
			ForceField,
			// Token: 0x0400434C RID: 17228
			UVMotion,
			// Token: 0x0400434D RID: 17229
			FXData,
			// Token: 0x0400434E RID: 17230
			SceneDepth,
			// Token: 0x0400434F RID: 17231
			FogNoise,
			// Token: 0x04004350 RID: 17232
			ShadowMap,
			// Token: 0x04004351 RID: 17233
			LightGrid,
			// Token: 0x04004352 RID: 17234
			LightIndicesOrDataBuffer,
			// Token: 0x04004353 RID: 17235
			OITTotalOpticalDepth,
			// Token: 0x04004354 RID: 17236
			OITMoments
		}

		// Token: 0x02000DA7 RID: 3495
		// (Invoke) Token: 0x060065B5 RID: 26037
		public delegate void Command(string[] args);

		// Token: 0x02000DA8 RID: 3496
		private class DebugModule : Module
		{
			// Token: 0x060065B8 RID: 26040 RVA: 0x00212321 File Offset: 0x00210521
			public DebugModule(GameInstance gameInstance) : base(gameInstance)
			{
			}

			// Token: 0x060065B9 RID: 26041 RVA: 0x0021232C File Offset: 0x0021052C
			[Obsolete]
			public override void OnNewFrame(float deltaTime)
			{
				GameInstance.Logger.Info("DebugModule: OnNewFrame({0})", deltaTime);
			}

			// Token: 0x060065BA RID: 26042 RVA: 0x00212340 File Offset: 0x00210540
			[Obsolete]
			public override void Tick()
			{
				GameInstance.Logger.Info("DebugModule: Tick()");
			}

			// Token: 0x060065BB RID: 26043 RVA: 0x00212353 File Offset: 0x00210553
			protected override void DoDispose()
			{
				GameInstance.Logger.Info("DebugModule: Dispose()");
			}
		}
	}
}
