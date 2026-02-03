using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HytaleClient.Application.Services;
using HytaleClient.Application.Services.Api;
using HytaleClient.Audio;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Data.BlockyModels;
using HytaleClient.Data.Characters;
using HytaleClient.Graphics;
using HytaleClient.Graphics.BlockyModels;
using HytaleClient.Graphics.Programs;
using HytaleClient.Interface.MainMenu.Pages;
using HytaleClient.Math;
using HytaleClient.Networking;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using SDL2;

namespace HytaleClient.Application
{
	// Token: 0x02000BE8 RID: 3048
	internal class AppMainMenu
	{
		// Token: 0x17001404 RID: 5124
		// (get) Token: 0x060060EA RID: 24810 RVA: 0x001FB896 File Offset: 0x001F9A96
		// (set) Token: 0x060060EB RID: 24811 RVA: 0x001FB89E File Offset: 0x001F9A9E
		public AppMainMenu.MainMenuPage CurrentPage { get; private set; }

		// Token: 0x060060EC RID: 24812 RVA: 0x001FB8A8 File Offset: 0x001F9AA8
		public AppMainMenu(App app)
		{
			this._app = app;
			this.GatherWorldList();
		}

		// Token: 0x060060ED RID: 24813 RVA: 0x001FB941 File Offset: 0x001F9B41
		internal void SetPageToReturnTo(AppMainMenu.MainMenuPage page)
		{
			Debug.Assert(this._app.Stage != App.AppStage.MainMenu);
			this.CurrentPage = page;
		}

		// Token: 0x060060EE RID: 24814 RVA: 0x001FB964 File Offset: 0x001F9B64
		private void InitializeMainMenuMusic()
		{
			uint eventId;
			bool flag = this._app.Engine.Audio.ResourceManager.WwiseEventIds.TryGetValue("MENU_INIT", out eventId);
			if (flag)
			{
				this._musicPlaybackId = this._app.Engine.Audio.PostEvent(eventId, AudioDevice.PlayerSoundObjectReference);
			}
			else
			{
				AppMainMenu.Logger.Warn("Could not load UI music: {0}", "MENU_INIT");
			}
		}

		// Token: 0x060060EF RID: 24815 RVA: 0x001FB9D8 File Offset: 0x001F9BD8
		public void StopMusic()
		{
			this._app.Engine.Audio.ActionOnEvent(this._musicPlaybackId, 0, 5000, 0);
		}

		// Token: 0x17001405 RID: 5125
		// (get) Token: 0x060060F0 RID: 24816 RVA: 0x001FB9FE File Offset: 0x001F9BFE
		// (set) Token: 0x060060F1 RID: 24817 RVA: 0x001FBA06 File Offset: 0x001F9C06
		public Server[] LanServers { get; private set; } = new Server[0];

		// Token: 0x17001406 RID: 5126
		// (get) Token: 0x060060F2 RID: 24818 RVA: 0x001FBA0F File Offset: 0x001F9C0F
		// (set) Token: 0x060060F3 RID: 24819 RVA: 0x001FBA17 File Offset: 0x001F9C17
		public ClientPlayerSkin EditedSkin { get; private set; }

		// Token: 0x060060F4 RID: 24820 RVA: 0x001FBA20 File Offset: 0x001F9C20
		public unsafe void Open(AppMainMenu.MainMenuPage page)
		{
			bool flag = this._app.Stage != App.AppStage.MainMenu;
			if (flag)
			{
				App.AppStage stage = this._app.Stage;
				App.AppStage appStage = stage;
				if (appStage != App.AppStage.Startup && appStage - App.AppStage.GameLoading > 2)
				{
					Debug.Assert(false);
				}
				this._app.SetSingleplayerWorldName(null);
				this._app.SetStage(App.AppStage.MainMenu);
				Engine engine = this._app.Engine;
				int width = engine.Window.Viewport.Width;
				int height = engine.Window.Viewport.Height;
				engine.Graphics.RTStore.Resize(width, height, 1f);
				engine.Profiling.Initialize(1, 200);
				this.SceneRenderer = new SceneRenderer(engine.Graphics, engine.Profiling, width, height);
				PostEffectProgram mainMenuPostEffectProgram = engine.Graphics.GPUProgramStore.MainMenuPostEffectProgram;
				this.PostEffectRenderer = new PostEffectRenderer(engine.Graphics, engine.Profiling, mainMenuPostEffectProgram);
				this._backgroundRenderer = new QuadRenderer(engine.Graphics, engine.Graphics.GPUProgramStore.BasicProgram.AttribPosition, engine.Graphics.GPUProgramStore.BasicProgram.AttribTexCoords);
				GLFunctions gl = engine.Graphics.GL;
				gl.Enable(GL.BLEND);
				gl.Enable(GL.CULL_FACE);
				Image image = new Image(File.ReadAllBytes(Path.Combine(Paths.GameData, "Backgrounds", this._app.Interface.MainMenuBackgroundImagePath)));
				this._defaultBackgroundTexture = gl.GenTexture();
				gl.BindTexture(GL.TEXTURE_2D, this._defaultBackgroundTexture);
				gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, 9729);
				gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, 9729);
				gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_S, 33071);
				gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_T, 33071);
				byte[] array;
				byte* value;
				if ((array = image.Pixels) == null || array.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array[0];
				}
				gl.TexImage2D(GL.TEXTURE_2D, 0, 6408, image.Width, image.Height, 0, GL.RGBA, GL.UNSIGNED_BYTE, (IntPtr)((void*)value));
				array = null;
				Image image2 = new Image(File.ReadAllBytes(Path.Combine(Paths.GameData, "Backgrounds", this._app.Interface.MainMenuBackgroundImagePath.Replace(".png", "Blurred.png"))));
				this._defaultBlurredBackgroundTexture = gl.GenTexture();
				gl.BindTexture(GL.TEXTURE_2D, this._defaultBlurredBackgroundTexture);
				gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, 9729);
				gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, 9729);
				gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_S, 33071);
				gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_T, 33071);
				byte* value2;
				if ((array = image2.Pixels) == null || array.Length == 0)
				{
					value2 = null;
				}
				else
				{
					value2 = &array[0];
				}
				gl.TexImage2D(GL.TEXTURE_2D, 0, 6408, image2.Width, image2.Height, 0, GL.RGBA, GL.UNSIGNED_BYTE, (IntPtr)((void*)value2));
				array = null;
				Image image3 = new Image(File.ReadAllBytes(Path.Combine(Paths.GameData, "Backgrounds", "CharacterCreator.png")));
				this._characterCreatorBackgroundTexture = gl.GenTexture();
				gl.BindTexture(GL.TEXTURE_2D, this._characterCreatorBackgroundTexture);
				gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, 9729);
				gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, 9729);
				gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_S, 33071);
				gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_T, 33071);
				byte* value3;
				if ((array = image3.Pixels) == null || array.Length == 0)
				{
					value3 = null;
				}
				else
				{
					value3 = &array[0];
				}
				gl.TexImage2D(GL.TEXTURE_2D, 0, 6408, image3.Width, image3.Height, 0, GL.RGBA, GL.UNSIGNED_BYTE, (IntPtr)((void*)value3));
				array = null;
				this.SetupCharacter();
				this.SetupCharacterAssetPreviews();
				this.SetupLanDiscovery();
				this.InitializeMainMenuMusic();
				this._app.Interface.FadeIn(null, false);
			}
			this.CurrentPage = page;
			this._app.Interface.DevToolsLayer.DevTools.ResetGameInfoState();
			this._app.Interface.MainMenuView.OnPageChanged();
		}

		// Token: 0x060060F5 RID: 24821 RVA: 0x001FBF4C File Offset: 0x001FA14C
		internal void CleanUp()
		{
			this.DisposeCharacter();
			this.DisposeCharacterAssetPreviews();
			this.DisposeLanDiscovery();
			this._backgroundRenderer.Dispose();
			this._backgroundRenderer = null;
			GLFunctions gl = this._app.Engine.Graphics.GL;
			gl.DeleteTexture(this._characterCreatorBackgroundTexture);
			gl.DeleteTexture(this._defaultBlurredBackgroundTexture);
			gl.DeleteTexture(this._defaultBackgroundTexture);
			this._characterCreatorBackgroundTexture = GLTexture.None;
			this._defaultBlurredBackgroundTexture = GLTexture.None;
			this._defaultBackgroundTexture = GLTexture.None;
			this.SceneRenderer.Dispose();
			this.SceneRenderer = null;
			this.PostEffectRenderer.Dispose();
			this.PostEffectRenderer = null;
		}

		// Token: 0x060060F6 RID: 24822 RVA: 0x001FC008 File Offset: 0x001FA208
		internal void OnUserInput(SDL.SDL_Event @event)
		{
			bool flag = @event.type == SDL.SDL_EventType.SDL_KEYDOWN && Input.EventMatchesBinding(@event, this._app.Settings.InputBindings.OpenAssetEditor) && this._app.Interface.Desktop.FocusedElement == null;
			if (flag)
			{
				this.OpenCosmeticEditor();
			}
			this.OnCharacterRotate(@event);
		}

		// Token: 0x060060F7 RID: 24823 RVA: 0x001FC070 File Offset: 0x001FA270
		internal void OnNewFrame(float deltaTime)
		{
			this.UpdateLanDiscovery(deltaTime);
			this._app.Interface.PrepareForDraw();
			Engine engine = this._app.Engine;
			RenderTargetStore rtstore = engine.Graphics.RTStore;
			GLFunctions gl = engine.Graphics.GL;
			gl.Viewport(engine.Window.Viewport);
			int width = engine.Window.Viewport.Width;
			int height = engine.Window.Viewport.Height;
			rtstore.BeginFrame();
			this.SceneRenderer.BeginDraw();
			this.PostEffectRenderer.UseTemporalAA(false);
			rtstore.SceneColor.Bind(false, true);
			gl.ClearColor(0f, 0f, 0f, 1f);
			gl.Clear((GL)17664U);
			gl.Disable(GL.DEPTH_TEST);
			gl.Disable(GL.BLEND);
			BasicProgram basicProgram = engine.Graphics.GPUProgramStore.BasicProgram;
			gl.UseProgram(basicProgram);
			AppMainMenu.MainMenuPage currentPage = this.CurrentPage;
			AppMainMenu.MainMenuPage mainMenuPage = currentPage;
			GLTexture texture;
			if (mainMenuPage != AppMainMenu.MainMenuPage.Home)
			{
				if (mainMenuPage != AppMainMenu.MainMenuPage.MyAvatar)
				{
					texture = this._defaultBlurredBackgroundTexture;
				}
				else
				{
					texture = this._characterCreatorBackgroundTexture;
				}
			}
			else
			{
				texture = this._defaultBackgroundTexture;
			}
			gl.BindTexture(GL.TEXTURE_2D, texture);
			basicProgram.Opacity.SetValue(1f);
			basicProgram.Color.SetValue(engine.Graphics.WhiteColor);
			basicProgram.MVPMatrix.SetValue(ref engine.Graphics.ScreenMatrix);
			this._backgroundRenderer.Draw();
			bool flag = this.CurrentPage == AppMainMenu.MainMenuPage.Home || this.CurrentPage == AppMainMenu.MainMenuPage.MyAvatar;
			if (flag)
			{
				gl.Enable(GL.DEPTH_TEST);
				gl.UseProgram(engine.Graphics.GPUProgramStore.BlockyModelForwardProgram);
				gl.ActiveTexture(GL.TEXTURE3);
				gl.BindTexture(GL.TEXTURE_2D, this._app.CharacterPartStore.CharacterGradientAtlas.GLTexture);
				gl.ActiveTexture(GL.TEXTURE0);
				gl.BindTexture(GL.TEXTURE_2D, this._app.CharacterPartStore.TextureAtlas.GLTexture);
				this.DrawCharacters(deltaTime);
				gl.Disable(GL.DEPTH_TEST);
			}
			rtstore.SceneColor.Unbind();
			gl.Viewport(engine.Window.Viewport);
			this.PostEffectRenderer.Draw(rtstore.SceneColor.GetTexture(RenderTarget.Target.Color0), GLTexture.None, width, height, 1f, null);
			gl.Enable(GL.BLEND);
			gl.BlendFunc(GL.ONE, GL.ONE_MINUS_SRC_ALPHA);
			this._app.Interface.Draw();
			gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
		}

		// Token: 0x060060F8 RID: 24824 RVA: 0x001FC344 File Offset: 0x001FA544
		public bool TryUpdateSingleplayerWorldOptions(string path, AppMainMenu.WorldOptions options, out string error)
		{
			string text = Path.Combine(new string[]
			{
				Paths.Saves,
				path,
				"worlds",
				"default",
				"config.bson"
			});
			try
			{
				JObject jobject = JObject.Parse(File.ReadAllText(text));
				jobject["DisplayName"] = options.Name;
				jobject["GameMode"] = options.GameMode.ToString();
				jobject["IsSpawningNPC"] = options.NpcSpawning;
				string text2 = JsonConvert.SerializeObject(jobject, 1, new JsonSerializerSettings
				{
					FloatFormatHandling = 1
				});
				File.WriteAllText(text, text2);
			}
			catch (Exception ex)
			{
				error = ex.Message;
				return false;
			}
			AppMainMenu.Logger.Info("Updated options for world \"{0}\"", path);
			error = null;
			return true;
		}

		// Token: 0x060060F9 RID: 24825 RVA: 0x001FC43C File Offset: 0x001FA63C
		public bool TryDeleteSingleplayerWorld(string worldDirectoryName, out string error)
		{
			bool result;
			try
			{
				AppMainMenu.Logger.Info("Deleting singleplayer world \"{0}\"", worldDirectoryName);
				string text = Path.Combine(Paths.Saves, worldDirectoryName);
				bool flag = !Path.GetFullPath(text).StartsWith(Path.GetFullPath(Path.Combine(new string[]
				{
					Paths.Saves
				})));
				if (flag)
				{
					AppMainMenu.Logger.Warn("Failed to delete world {0} path is outside of Saves directory!", worldDirectoryName);
					error = "Path is outsides of Saves directory";
					result = false;
				}
				else
				{
					Directory.Delete(text, true);
					this.GatherWorldList();
					error = null;
					result = true;
				}
			}
			catch (Exception ex)
			{
				AppMainMenu.Logger.Error<Exception>(ex);
				error = ex.Message;
				result = false;
			}
			return result;
		}

		// Token: 0x060060FA RID: 24826 RVA: 0x001FC4F0 File Offset: 0x001FA6F0
		public void OpenSingleplayerWorldFolder(string worldDirectoryName)
		{
			Process.Start(Path.Combine(Paths.Saves, worldDirectoryName));
		}

		// Token: 0x060060FB RID: 24827 RVA: 0x001FC504 File Offset: 0x001FA704
		public void GatherWorldList()
		{
			this.Worlds.Clear();
			bool flag = Directory.Exists(Paths.Saves);
			if (flag)
			{
				foreach (string text in Directory.GetDirectories(Paths.Saves))
				{
					string text2 = text.Substring(Paths.Saves.Length + 1);
					string name = text2;
					GameMode gameMode = 0;
					bool npcSpawning = true;
					string text3 = Path.Combine(text, "worlds", "default", "config.bson");
					bool flag2 = File.Exists(text3);
					if (flag2)
					{
						JObject jobject = JObject.Parse(File.ReadAllText(text3));
						bool flag3 = jobject["DisplayName"] != null;
						if (flag3)
						{
							name = Extensions.Value<string>(jobject["DisplayName"]);
						}
						bool flag4 = jobject["GameMode"] != null;
						if (flag4)
						{
							Enum.TryParse<GameMode>(Extensions.Value<string>(jobject["GameMode"]), out gameMode);
						}
						bool flag5 = jobject["IsSpawningNPC"] != null;
						if (flag5)
						{
							npcSpawning = Extensions.Value<bool>(jobject["IsSpawningNPC"]);
						}
					}
					this.Worlds.Add(new AppMainMenu.World
					{
						Path = text2 + Path.DirectorySeparatorChar.ToString(),
						Options = new AppMainMenu.WorldOptions
						{
							Name = name,
							GameMode = gameMode,
							NpcSpawning = npcSpawning
						},
						HasPreviewImage = File.Exists(Path.Combine(Paths.Saves, text, "preview.png")),
						LastPreviewImageWriteTime = File.GetLastWriteTime(Path.Combine(Paths.Saves, text, "preview.png")).ToString("o"),
						LastWriteTime = File.GetLastWriteTime(Path.Combine(Paths.Saves, text)).ToString("o")
					});
				}
			}
			this.Worlds.Sort((AppMainMenu.World a, AppMainMenu.World b) => string.Compare(b.LastWriteTime, a.LastWriteTime, StringComparison.Ordinal));
		}

		// Token: 0x060060FC RID: 24828 RVA: 0x001FC70C File Offset: 0x001FA90C
		public void JoinSingleplayerWorld(string worldDirectoryName)
		{
			string text;
			bool flag = !this.CanConnectToServer("join world " + worldDirectoryName, out text);
			if (!flag)
			{
				AppMainMenu.Logger.Info("Connecting to singleplayer world \"{0}\"...", worldDirectoryName);
				this._app.Interface.FadeOut(delegate
				{
					this._app.GameLoading.Open(worldDirectoryName);
					this._app.Interface.FadeIn(null, false);
				});
			}
		}

		// Token: 0x060060FD RID: 24829 RVA: 0x001FC784 File Offset: 0x001FA984
		public bool TryCreateSingleplayerWorld(AppMainMenu.WorldOptions options, out string error)
		{
			bool flag = !this.CanConnectToServer("create world " + options.Name, out error);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				string path = new Regex("[+:\\/\\\\*?\"<>|]+").Replace(options.Name, string.Empty).Trim();
				string text = Paths.EnsureUniqueDirname(Path.Combine(Paths.Saves, path));
				AppMainMenu.Logger.Info("Creating new singleplayer world in \"{0}\"...", text);
				JObject jobject = new JObject();
				jobject["DisplayName"] = options.Name;
				jobject["GameMode"] = options.GameMode.ToString();
				jobject["IsSpawningNPC"] = options.NpcSpawning;
				bool flatWorld = options.FlatWorld;
				if (flatWorld)
				{
					jobject["WorldGen"] = JObject.Parse(File.ReadAllText(Path.Combine(Paths.GameData, "DefaultFlatWorldConfig.json")))["WorldGen"];
				}
				try
				{
					Directory.CreateDirectory(Path.Combine(text, "worlds", "default"));
					File.WriteAllText(Path.Combine(text, "worlds", "default", "config.bson"), jobject.ToString());
				}
				catch (Exception ex)
				{
					AppMainMenu.Logger.Error(ex, "Failed to create world");
					error = ex.Message;
					return false;
				}
				this.JoinSingleplayerWorld(Path.GetFileName(text));
				result = true;
			}
			return result;
		}

		// Token: 0x060060FE RID: 24830 RVA: 0x001FC910 File Offset: 0x001FAB10
		public void SetupLanDiscovery()
		{
			object lanDiscoverySocketLock = this._lanDiscoverySocketLock;
			lock (lanDiscoverySocketLock)
			{
				this._lanDiscoverySocket = new UdpClient
				{
					ExclusiveAddressUse = false,
					EnableBroadcast = true
				};
				this._lanDiscoverySocket.Client.SetSocketOption(65535, 4, true);
				this._lanDiscoverySocket.Client.Bind(new IPEndPoint(IPAddress.Any, 5511));
				this._lanDiscoverySocket.BeginReceive(new AsyncCallback(this.LanDiscoveryReceive), null);
			}
			this.ProbeLanServers();
		}

		// Token: 0x060060FF RID: 24831 RVA: 0x001FC9C4 File Offset: 0x001FABC4
		public void DisposeLanDiscovery()
		{
			object lanDiscoverySocketLock = this._lanDiscoverySocketLock;
			lock (lanDiscoverySocketLock)
			{
				this._lanDiscoverySocket.Client.Close();
				this._lanDiscoverySocket.Close();
				this._lanDiscoverySocket = null;
			}
		}

		// Token: 0x06006100 RID: 24832 RVA: 0x001FCA28 File Offset: 0x001FAC28
		public void UpdateLanDiscovery(float elapsedTime)
		{
			this._lanDiscoverTimer += elapsedTime;
			bool flag = this._lanDiscoverTimer < 5f;
			if (!flag)
			{
				this._lanDiscoverTimer = 0f;
				this.ProbeLanServers();
				object newlyDiscoveredLanServersLock = this._newlyDiscoveredLanServersLock;
				lock (newlyDiscoveredLanServersLock)
				{
					bool flag3 = this._newlyDiscoveredLanServers.RemoveWhere((Server details) => details.Updated.ElapsedMilliseconds >= 10000L) > 0;
					if (flag3)
					{
						this.LanServers = Enumerable.ToArray<Server>(this._newlyDiscoveredLanServers);
						bool flag4 = this.ActiveServerListTab == AppMainMenu.ServerListTab.Lan;
						if (flag4)
						{
							this._app.Interface.MainMenuView.ServersPage.BuildServerList();
						}
					}
				}
			}
		}

		// Token: 0x06006101 RID: 24833 RVA: 0x001FCB0C File Offset: 0x001FAD0C
		private void ProbeLanServers()
		{
			object lanDiscoverySocketLock = this._lanDiscoverySocketLock;
			lock (lanDiscoverySocketLock)
			{
				bool flag2 = this._lanDiscoverySocket == null;
				if (!flag2)
				{
					try
					{
						IPEndPoint ipendPoint = new IPEndPoint(IPAddress.Broadcast, 5510);
						this._lanDiscoverySocket.Send(AppMainMenu.LanDiscoveryRequestHeader, AppMainMenu.LanDiscoveryRequestHeader.Length, ipendPoint);
					}
					catch (SocketException)
					{
					}
				}
			}
		}

		// Token: 0x06006102 RID: 24834 RVA: 0x001FCB9C File Offset: 0x001FAD9C
		private void LanDiscoveryReceive(IAsyncResult result)
		{
			object lanDiscoverySocketLock = this._lanDiscoverySocketLock;
			lock (lanDiscoverySocketLock)
			{
				bool flag2 = this._lanDiscoverySocket == null;
				if (!flag2)
				{
					try
					{
						IPEndPoint endPoint = null;
						byte[] bytes = this._lanDiscoverySocket.EndReceive(result, ref endPoint);
						this.HandleLanDiscoveryData(bytes, endPoint);
					}
					catch (ObjectDisposedException)
					{
					}
					catch (Exception value)
					{
						AppMainMenu.Logger.Error<Exception>(value);
					}
					try
					{
						this._lanDiscoverySocket.BeginReceive(new AsyncCallback(this.LanDiscoveryReceive), null);
					}
					catch (ObjectDisposedException)
					{
					}
				}
			}
		}

		// Token: 0x06006103 RID: 24835 RVA: 0x001FCC6C File Offset: 0x001FAE6C
		private void HandleLanDiscoveryData(byte[] bytes, IPEndPoint endPoint)
		{
			bool flag = AppMainMenu.LanDiscoveryReplyHeader.Length > bytes.Length;
			if (!flag)
			{
				bool flag2 = Enumerable.Any<byte>(Enumerable.Where<byte>(AppMainMenu.LanDiscoveryReplyHeader, (byte t, int i) => bytes[i] != t));
				if (!flag2)
				{
					byte b = bytes[AppMainMenu.LanDiscoveryReplyHeader.Length];
					bool flag3 = b > 0;
					if (flag3)
					{
						AppMainMenu.Logger.Warn<byte, IPEndPoint>("Received LAN Discovery packet with incompatible version: {0} from {1}", b, endPoint);
					}
					else
					{
						using (MemoryStream memoryStream = new MemoryStream(bytes, AppMainMenu.LanDiscoveryReplyHeader.Length + 1, bytes.Length - AppMainMenu.LanDiscoveryReplyHeader.Length - 1))
						{
							using (BinaryReader binaryReader = new BinaryReader(memoryStream))
							{
								IPAddress ipaddress = new IPAddress(binaryReader.ReadBytes((int)binaryReader.ReadByte()));
								ushort num = binaryReader.ReadUInt16();
								string @string = Encoding.UTF8.GetString(binaryReader.ReadBytes((int)binaryReader.ReadUInt16()));
								uint onlinePlayers = binaryReader.ReadUInt32();
								uint maxPlayers = binaryReader.ReadUInt32();
								bool flag4 = object.Equals(ipaddress, IPAddress.Any);
								if (flag4)
								{
									ipaddress = endPoint.Address;
								}
								Server server = new Server
								{
									IsLan = true,
									Host = string.Format("{0}:{1}", ipaddress, num),
									Name = @string,
									MaxPlayers = (int)maxPlayers,
									OnlinePlayers = (int)onlinePlayers
								};
								object newlyDiscoveredLanServersLock = this._newlyDiscoveredLanServersLock;
								bool flag5 = false;
								try
								{
									Monitor.Enter(newlyDiscoveredLanServersLock, ref flag5);
									bool flag6 = this._newlyDiscoveredLanServers.Add(server);
									bool flag7 = false;
									bool flag8 = !flag6;
									if (flag8)
									{
										foreach (Server server2 in this._newlyDiscoveredLanServers)
										{
											bool flag9 = !server2.Equals(server);
											if (!flag9)
											{
												bool flag10 = server2.MaxPlayers != server.MaxPlayers;
												if (flag10)
												{
													flag7 = true;
												}
												bool flag11 = server2.OnlinePlayers != server.OnlinePlayers;
												if (flag11)
												{
													flag7 = true;
												}
												server2.MaxPlayers = server.MaxPlayers;
												server2.OnlinePlayers = server.OnlinePlayers;
												server2.Updated.Restart();
												break;
											}
										}
									}
									bool flag12 = !flag7 && !flag6;
									if (!flag12)
									{
										AppMainMenu.Logger.Info<IPAddress, ushort>("Discovered LAN server: {0}:{1}", ipaddress, num);
										Server[] lanServers = Enumerable.ToArray<Server>(this._newlyDiscoveredLanServers);
										this._app.Engine.RunOnMainThread(this._app.Engine, delegate
										{
											this.LanServers = lanServers;
											bool flag13 = this.ActiveServerListTab == AppMainMenu.ServerListTab.Lan;
											if (flag13)
											{
												this._app.Interface.MainMenuView.ServersPage.BuildServerList();
											}
										}, false, false);
									}
								}
								finally
								{
									if (flag5)
									{
										Monitor.Exit(newlyDiscoveredLanServersLock);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06006104 RID: 24836 RVA: 0x001FCFC8 File Offset: 0x001FB1C8
		public void QueueForSharedSinglePlayerWorld(Guid worldId)
		{
			string text;
			bool flag = !this.CanConnectToServer(string.Format("queue for {0}", worldId), out text);
			if (!flag)
			{
				bool flag2 = !this.ServicesConnected(string.Format("queue for {0}", worldId));
				if (!flag2)
				{
					this._app.HytaleServices.JoinSharedSinglePlayerWorld(worldId, null, null);
				}
			}
		}

		// Token: 0x06006105 RID: 24837 RVA: 0x001FD02C File Offset: 0x001FB22C
		public void CreateSharedSinglePlayerWorld(string name)
		{
			bool flag = !this.ServicesConnected("create world " + name);
			if (!flag)
			{
				this._app.HytaleServices.CreateSharedSinglePlayerWorld(name, null, null);
			}
		}

		// Token: 0x06006106 RID: 24838 RVA: 0x001FD068 File Offset: 0x001FB268
		private bool ServicesConnected(string attemptedAction)
		{
			bool flag = this._app.Interface.ServiceState != HytaleServices.ServiceState.Connected;
			bool result;
			if (flag)
			{
				AppMainMenu.Logger.Warn("Tried to {0} but not authenticated with services!", attemptedAction);
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06006107 RID: 24839 RVA: 0x001FD0AC File Offset: 0x001FB2AC
		private void SetupCharacterAssetPreviews()
		{
			BasicProgram basicProgram = this._app.Engine.Graphics.GPUProgramStore.BasicProgram;
			Image image = new Image(File.ReadAllBytes(Path.Combine(Paths.GameData, "Interface/MainMenu/MyAvatar/PartBackgroundSelected@2x.png")));
			this._characterAssetPreviewSelectionBackgroundTexture = new Texture(Texture.TextureTypes.Texture2D);
			this._characterAssetPreviewSelectionBackgroundTexture.CreateTexture2D(image.Width, image.Height, image.Pixels, 5, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
			this._characterAssetPreviewBackgroundQuadRenderer = new QuadRenderer(this._app.Engine.Graphics, basicProgram.AttribPosition, basicProgram.AttribTexCoords);
			int sampleCount = this._useMSAAForAssetPreview ? 4 : 1;
			this._characterAssetPreviewRenderTarget = new RenderTarget(92, 149, "_characterAssetPreviewRenderTarget");
			this._characterAssetPreviewRenderTarget.AddTexture(RenderTarget.Target.Depth, GL.DEPTH24_STENCIL8, GL.DEPTH_STENCIL, GL.UNSIGNED_INT_24_8, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, false, false, sampleCount);
			this._characterAssetPreviewRenderTarget.AddTexture(RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, sampleCount);
			this._characterAssetPreviewRenderTarget.FinalizeSetup();
			this._characterAssetFinalPreviewRenderTarget = new RenderTarget(92, 149, "_characterAssetFinalPreviewRenderTarget");
			this._characterAssetFinalPreviewRenderTarget.AddTexture(RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this._characterAssetFinalPreviewRenderTarget.FinalizeSetup();
			this._characterOnScreen = new AppMainMenu.CharacterOnScreen(this._app, new Rectangle(0, 0, 92, 149), -0.3926991f, 1f);
		}

		// Token: 0x06006108 RID: 24840 RVA: 0x001FD26B File Offset: 0x001FB46B
		private void DisposeCharacterAssetPreviews()
		{
			this._characterOnScreen.Dispose();
			this._characterAssetFinalPreviewRenderTarget.Dispose();
			this._characterAssetPreviewRenderTarget.Dispose();
			this._characterAssetPreviewSelectionBackgroundTexture.Dispose();
			this._characterAssetPreviewBackgroundQuadRenderer.Dispose();
		}

		// Token: 0x06006109 RID: 24841 RVA: 0x001FD2AC File Offset: 0x001FB4AC
		public void RenderAssetPreviews(AppMainMenu.RenderCharacterPartPreviewCommand[] events)
		{
			GLFunctions gl = this._app.Engine.Graphics.GL;
			gl.Enable(GL.DEPTH_TEST);
			gl.Disable(GL.BLEND);
			foreach (AppMainMenu.RenderCharacterPartPreviewCommand renderCharacterPartPreviewCommand in events)
			{
				bool flag = renderCharacterPartPreviewCommand.BackgroundColor != null;
				if (flag)
				{
					ColorRgba value = renderCharacterPartPreviewCommand.BackgroundColor.Value;
					gl.ClearColor((float)value.R / 255f, (float)value.G / 255f, (float)value.B / 255f, (float)value.A / 255f);
				}
				else
				{
					gl.ClearColor(0.18431373f, 0.22745098f, 0.30980393f, 1f);
				}
				this._characterAssetPreviewRenderTarget.Bind(true, true);
				bool selected = renderCharacterPartPreviewCommand.Selected;
				if (selected)
				{
					gl.Disable(GL.DEPTH_TEST);
					BasicProgram basicProgram = this._app.Engine.Graphics.GPUProgramStore.BasicProgram;
					gl.UseProgram(basicProgram);
					basicProgram.Opacity.SetValue(1f);
					gl.BindTexture(GL.TEXTURE_2D, this._characterAssetPreviewSelectionBackgroundTexture.GLTexture);
					basicProgram.Color.SetValue(this._app.Engine.Graphics.WhiteColor);
					this._characterAssetPreviewBackgroundQuadRenderer.Draw();
					gl.Enable(GL.DEPTH_TEST);
				}
				ClientPlayerSkin clientPlayerSkin = new ClientPlayerSkin
				{
					BodyType = this.EditedSkin.BodyType,
					SkinTone = this.EditedSkin.SkinTone,
					Haircut = this.EditedSkin.Haircut,
					FacialHair = this.EditedSkin.FacialHair,
					Eyes = this.EditedSkin.Eyes,
					Face = this.EditedSkin.Face,
					Eyebrows = this.EditedSkin.Eyebrows
				};
				CharacterPartStore characterPartStore = this._app.CharacterPartStore;
				switch (renderCharacterPartPreviewCommand.Property)
				{
				case PlayerSkinProperty.Haircut:
					clientPlayerSkin.Haircut = renderCharacterPartPreviewCommand.Id;
					this._characterOnScreen.Translation = new Vector3(0f, -8.3f, -8f);
					break;
				case PlayerSkinProperty.FacialHair:
					clientPlayerSkin.FacialHair = renderCharacterPartPreviewCommand.Id;
					this._characterOnScreen.Translation = new Vector3(0f, -6.7f, -8f);
					break;
				case PlayerSkinProperty.Eyebrows:
					clientPlayerSkin.Eyebrows = renderCharacterPartPreviewCommand.Id;
					this._characterOnScreen.Translation = new Vector3(-0.2f, -8.3f, -6.5f);
					break;
				case PlayerSkinProperty.Eyes:
					clientPlayerSkin.Eyes = renderCharacterPartPreviewCommand.Id;
					this._characterOnScreen.Translation = new Vector3(-0.2f, -8.3f, -6f);
					break;
				case PlayerSkinProperty.Face:
					clientPlayerSkin.Face = renderCharacterPartPreviewCommand.Id.PartId;
					this._characterOnScreen.Translation = new Vector3(-0.2f, -8.3f, -6f);
					break;
				case PlayerSkinProperty.Pants:
					clientPlayerSkin.Pants = renderCharacterPartPreviewCommand.Id;
					this._characterOnScreen.Translation = new Vector3(0f, -2.5f, -8f);
					break;
				case PlayerSkinProperty.Overpants:
					clientPlayerSkin.Overpants = renderCharacterPartPreviewCommand.Id;
					this._characterOnScreen.Translation = new Vector3(0f, -2.5f, -8f);
					break;
				case PlayerSkinProperty.Undertop:
					clientPlayerSkin.Undertop = renderCharacterPartPreviewCommand.Id;
					this._characterOnScreen.Translation = new Vector3(0f, -4.9f, -8f);
					break;
				case PlayerSkinProperty.Overtop:
					clientPlayerSkin.Overtop = renderCharacterPartPreviewCommand.Id;
					this._characterOnScreen.Translation = new Vector3(0f, -4.9f, -8f);
					break;
				case PlayerSkinProperty.Shoes:
					clientPlayerSkin.Shoes = renderCharacterPartPreviewCommand.Id;
					this._characterOnScreen.Translation = new Vector3(0f, -2.2f, -8f);
					break;
				case PlayerSkinProperty.HeadAccessory:
					clientPlayerSkin.HeadAccessory = renderCharacterPartPreviewCommand.Id;
					this._characterOnScreen.Translation = new Vector3(0f, -8.3f, -8f);
					break;
				case PlayerSkinProperty.FaceAccessory:
					clientPlayerSkin.FaceAccessory = renderCharacterPartPreviewCommand.Id;
					this._characterOnScreen.Translation = new Vector3(0f, -8.3f, -8f);
					break;
				case PlayerSkinProperty.EarAccessory:
					clientPlayerSkin.EarAccessory = renderCharacterPartPreviewCommand.Id;
					this._characterOnScreen.Translation = new Vector3(-1.2f, -7.9f, -3.6f);
					break;
				case PlayerSkinProperty.Gloves:
					clientPlayerSkin.Gloves = renderCharacterPartPreviewCommand.Id;
					this._characterOnScreen.Translation = new Vector3(0f, -4.9f, -8f);
					break;
				}
				gl.ActiveTexture(GL.TEXTURE3);
				gl.BindTexture(GL.TEXTURE_2D, characterPartStore.CharacterGradientAtlas.GLTexture);
				gl.ActiveTexture(GL.TEXTURE0);
				gl.BindTexture(GL.TEXTURE_2D, characterPartStore.TextureAtlas.GLTexture);
				gl.UseProgram(this._app.Engine.Graphics.GPUProgramStore.BlockyModelForwardProgram);
				this._characterOnScreen.SetRotation((float)((renderCharacterPartPreviewCommand.Property == PlayerSkinProperty.EarAccessory) ? -1 : 1) * 3.1415927f / 8f);
				try
				{
					this._characterOnScreen.InitializeRendering(clientPlayerSkin, characterPartStore);
				}
				catch
				{
					break;
				}
				this._characterOnScreen.Draw(0f);
				this._characterAssetPreviewRenderTarget.Unbind();
				bool flag2 = !this._useMSAAForAssetPreview;
				if (flag2)
				{
					this.PostEffectRenderer.Draw(this._characterAssetPreviewRenderTarget.GetTexture(RenderTarget.Target.Color0), GLTexture.None, this._characterAssetPreviewRenderTarget.Width, this._characterAssetPreviewRenderTarget.Height, 1f, this._characterAssetFinalPreviewRenderTarget);
				}
				else
				{
					this._characterAssetPreviewRenderTarget.ResolveTo(this._characterAssetFinalPreviewRenderTarget, GL.COLOR_ATTACHMENT0, GL.COLOR_ATTACHMENT0, GL.NEAREST, false, false);
				}
				Texture texture = new Texture(Texture.TextureTypes.Texture2D);
				byte[] pixels = this._characterAssetFinalPreviewRenderTarget.ReadPixels(1, GL.RGBA, this._useMSAAForAssetPreview);
				texture.CreateTexture2D(this._characterAssetFinalPreviewRenderTarget.Width, this._characterAssetFinalPreviewRenderTarget.Height, pixels, 5, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
				this._app.Interface.MainMenuView.MyAvatarPage.OnPreviewRendered(renderCharacterPartPreviewCommand.Id, texture);
			}
		}

		// Token: 0x0600610A RID: 24842 RVA: 0x001FD9A4 File Offset: 0x001FBBA4
		private string CharacterAssetPreview_BuildAssetId(string assetId, string colorId, string variantId)
		{
			bool flag = variantId != null;
			string result;
			if (flag)
			{
				result = string.Concat(new string[]
				{
					assetId,
					".",
					colorId,
					".",
					variantId
				});
			}
			else
			{
				result = assetId + "." + colorId;
			}
			return result;
		}

		// Token: 0x0600610B RID: 24843 RVA: 0x001FD9F8 File Offset: 0x001FBBF8
		private void SetupCharacter()
		{
			bool flag = !this._app.AuthManager.Settings.IsInsecure;
			if (flag)
			{
				AppMainMenu.<>c__DisplayClass81_0 CS$<>8__locals1;
				CS$<>8__locals1.meta = (this._app.AuthManager.Metadata["playerOptions"] as JObject);
				bool flag2 = CS$<>8__locals1.meta != null;
				if (flag2)
				{
					AppMainMenu.Logger.Info<JObject>("Starting with meta {0}", CS$<>8__locals1.meta);
					AppMainMenu.<>c__DisplayClass81_1 CS$<>8__locals2;
					CS$<>8__locals2.partStore = this._app.CharacterPartStore;
					this.EditedSkin = new ClientPlayerSkin();
					JToken jtoken = CS$<>8__locals1.meta["bodyType"];
					bool flag3 = jtoken != null;
					if (flag3)
					{
						this.EditedSkin.BodyType = (CharacterBodyType)Enum.Parse(typeof(CharacterBodyType), (string)CS$<>8__locals1.meta["bodyType"]);
					}
					else
					{
						this.EditedSkin.BodyType = ((this._characterRandom.Next(2) == 0) ? CharacterBodyType.Masculine : CharacterBodyType.Feminine);
					}
					Dictionary<string, CharacterPartTintColor> gradients = CS$<>8__locals2.partStore.GradientSets["Skin"].Gradients;
					bool flag4 = gradients.ContainsKey((string)CS$<>8__locals1.meta["skinTone"]);
					if (flag4)
					{
						this.EditedSkin.SkinTone = (string)CS$<>8__locals1.meta["skinTone"];
					}
					else
					{
						this.EditedSkin.SkinTone = Enumerable.ElementAt<KeyValuePair<string, CharacterPartTintColor>>(gradients, this._characterRandom.Next(gradients.Count)).Key;
					}
					CharacterPart characterPart;
					bool flag5 = CS$<>8__locals1.meta.ContainsKey("face") && CS$<>8__locals2.partStore.TryGetCharacterPart(PlayerSkinProperty.Face, (string)CS$<>8__locals1.meta["face"], out characterPart);
					if (flag5)
					{
						this.EditedSkin.Face = characterPart.Id;
					}
					else
					{
						this.EditedSkin.Face = CS$<>8__locals2.partStore.GetDefaultPartFor(this.EditedSkin.BodyType, CS$<>8__locals2.partStore.Faces).Id;
					}
					this.EditedSkin.Eyes = AppMainMenu.<SetupCharacter>g__GetCharacterPartId|81_0("eyes", PlayerSkinProperty.Eyes, CS$<>8__locals2.partStore.GetDefaultPartIdFor(this.EditedSkin.BodyType, CS$<>8__locals2.partStore.Eyes), ref CS$<>8__locals1, ref CS$<>8__locals2);
					this.EditedSkin.Haircut = AppMainMenu.<SetupCharacter>g__GetCharacterPartId|81_0("haircut", PlayerSkinProperty.Haircut, null, ref CS$<>8__locals1, ref CS$<>8__locals2);
					this.EditedSkin.FacialHair = AppMainMenu.<SetupCharacter>g__GetCharacterPartId|81_0("facialHair", PlayerSkinProperty.FacialHair, null, ref CS$<>8__locals1, ref CS$<>8__locals2);
					this.EditedSkin.Eyebrows = AppMainMenu.<SetupCharacter>g__GetCharacterPartId|81_0("eyebrows", PlayerSkinProperty.Eyebrows, null, ref CS$<>8__locals1, ref CS$<>8__locals2);
					this.EditedSkin.Pants = AppMainMenu.<SetupCharacter>g__GetCharacterPartId|81_0("pants", PlayerSkinProperty.Pants, null, ref CS$<>8__locals1, ref CS$<>8__locals2);
					this.EditedSkin.Overpants = AppMainMenu.<SetupCharacter>g__GetCharacterPartId|81_0("overpants", PlayerSkinProperty.Overpants, null, ref CS$<>8__locals1, ref CS$<>8__locals2);
					this.EditedSkin.Undertop = AppMainMenu.<SetupCharacter>g__GetCharacterPartId|81_0("undertop", PlayerSkinProperty.Undertop, null, ref CS$<>8__locals1, ref CS$<>8__locals2);
					this.EditedSkin.Overtop = AppMainMenu.<SetupCharacter>g__GetCharacterPartId|81_0("overtop", PlayerSkinProperty.Overtop, null, ref CS$<>8__locals1, ref CS$<>8__locals2);
					this.EditedSkin.Shoes = AppMainMenu.<SetupCharacter>g__GetCharacterPartId|81_0("shoes", PlayerSkinProperty.Shoes, null, ref CS$<>8__locals1, ref CS$<>8__locals2);
					this.EditedSkin.HeadAccessory = AppMainMenu.<SetupCharacter>g__GetCharacterPartId|81_0("headAccessory", PlayerSkinProperty.HeadAccessory, null, ref CS$<>8__locals1, ref CS$<>8__locals2);
					this.EditedSkin.FaceAccessory = AppMainMenu.<SetupCharacter>g__GetCharacterPartId|81_0("faceAccessory", PlayerSkinProperty.FaceAccessory, null, ref CS$<>8__locals1, ref CS$<>8__locals2);
					this.EditedSkin.EarAccessory = AppMainMenu.<SetupCharacter>g__GetCharacterPartId|81_0("earAccessory", PlayerSkinProperty.EarAccessory, null, ref CS$<>8__locals1, ref CS$<>8__locals2);
					this.EditedSkin.SkinFeature = AppMainMenu.<SetupCharacter>g__GetCharacterPartId|81_0("skinFeature", PlayerSkinProperty.SkinFeature, null, ref CS$<>8__locals1, ref CS$<>8__locals2);
					this.EditedSkin.Gloves = AppMainMenu.<SetupCharacter>g__GetCharacterPartId|81_0("gloves", PlayerSkinProperty.Gloves, null, ref CS$<>8__locals1, ref CS$<>8__locals2);
					this._app.SetPlayerSkin(new ClientPlayerSkin(this.EditedSkin));
					AppMainMenu.Logger.Info<ClientPlayerSkin>("Got live skin {0}", this.EditedSkin);
				}
			}
			bool flag6 = this.EditedSkin == null;
			if (flag6)
			{
				this.EditedSkin = this.GetNakedCharacter(null);
				this._app.SetPlayerSkin(new ClientPlayerSkin(this.EditedSkin));
			}
		}

		// Token: 0x0600610C RID: 24844 RVA: 0x001FDE28 File Offset: 0x001FC028
		private void DisposeCharacter()
		{
			foreach (AppMainMenu.CharacterOnScreen characterOnScreen in this.CharactersOnScreen.Values)
			{
				characterOnScreen.Dispose();
			}
			this.CharactersOnScreen.Clear();
		}

		// Token: 0x0600610D RID: 24845 RVA: 0x001FDE90 File Offset: 0x001FC090
		public bool HasUnsavedSkinChanges()
		{
			return !this.EditedSkin.Equals(this._app.PlayerSkin);
		}

		// Token: 0x0600610E RID: 24846 RVA: 0x001FDEAC File Offset: 0x001FC0AC
		public void SaveCharacter()
		{
			AppMainMenu.Logger.Info("Saving character...");
			JObject jobject = new JObject();
			jobject.Add("bodyType", this.EditedSkin.BodyType.ToString());
			jobject.Add("skinTone", this.EditedSkin.SkinTone);
			jobject.Add("face", this.EditedSkin.Face);
			string text = "haircut";
			CharacterPartId haircut = this.EditedSkin.Haircut;
			jobject.Add(text, (haircut != null) ? haircut.ToString() : null);
			string text2 = "facialHair";
			CharacterPartId facialHair = this.EditedSkin.FacialHair;
			jobject.Add(text2, (facialHair != null) ? facialHair.ToString() : null);
			string text3 = "eyebrows";
			CharacterPartId eyebrows = this.EditedSkin.Eyebrows;
			jobject.Add(text3, (eyebrows != null) ? eyebrows.ToString() : null);
			string text4 = "eyes";
			CharacterPartId eyes = this.EditedSkin.Eyes;
			jobject.Add(text4, (eyes != null) ? eyes.ToString() : null);
			string text5 = "pants";
			CharacterPartId pants = this.EditedSkin.Pants;
			jobject.Add(text5, (pants != null) ? pants.ToString() : null);
			string text6 = "overpants";
			CharacterPartId overpants = this.EditedSkin.Overpants;
			jobject.Add(text6, (overpants != null) ? overpants.ToString() : null);
			string text7 = "undertop";
			CharacterPartId undertop = this.EditedSkin.Undertop;
			jobject.Add(text7, (undertop != null) ? undertop.ToString() : null);
			string text8 = "overtop";
			CharacterPartId overtop = this.EditedSkin.Overtop;
			jobject.Add(text8, (overtop != null) ? overtop.ToString() : null);
			string text9 = "shoes";
			CharacterPartId shoes = this.EditedSkin.Shoes;
			jobject.Add(text9, (shoes != null) ? shoes.ToString() : null);
			string text10 = "headAccessory";
			CharacterPartId headAccessory = this.EditedSkin.HeadAccessory;
			jobject.Add(text10, (headAccessory != null) ? headAccessory.ToString() : null);
			string text11 = "faceAccessory";
			CharacterPartId faceAccessory = this.EditedSkin.FaceAccessory;
			jobject.Add(text11, (faceAccessory != null) ? faceAccessory.ToString() : null);
			string text12 = "earAccessory";
			CharacterPartId earAccessory = this.EditedSkin.EarAccessory;
			jobject.Add(text12, (earAccessory != null) ? earAccessory.ToString() : null);
			string text13 = "skinFeature";
			CharacterPartId skinFeature = this.EditedSkin.SkinFeature;
			jobject.Add(text13, (skinFeature != null) ? skinFeature.ToString() : null);
			string text14 = "gloves";
			CharacterPartId gloves = this.EditedSkin.Gloves;
			jobject.Add(text14, (gloves != null) ? gloves.ToString() : null);
			JObject metadata = jobject;
			this._app.SetPlayerSkin(new ClientPlayerSkin(this.EditedSkin));
			bool flag = !this._app.AuthManager.Settings.IsInsecure;
			if (flag)
			{
				this._app.HytaleServices.SetPlayerOptions(metadata, delegate(Exception exception)
				{
					bool flag2 = exception == null;
					if (!flag2)
					{
						this._app.Engine.RunOnMainThread(this._app.Engine, delegate
						{
							bool flag3 = this._app.Stage != App.AppStage.MainMenu || this.CurrentPage != AppMainMenu.MainMenuPage.MyAvatar;
							if (!flag3)
							{
								this._app.Interface.MainMenuView.MyAvatarPage.OnFailedToSync(exception);
							}
						}, true, false);
					}
				});
			}
		}

		// Token: 0x0600610F RID: 24847 RVA: 0x001FE1B2 File Offset: 0x001FC3B2
		public void MakeEditedSkinNaked()
		{
			this.UpdateEditedSkin(delegate
			{
				this.EditedSkin = this.GetNakedCharacter(this.EditedSkin);
			}, true);
		}

		// Token: 0x06006110 RID: 24848 RVA: 0x001FE1CC File Offset: 0x001FC3CC
		public void AddCharacterOnScreen(AppMainMenu.AddCharacterOnScreenEvent evt)
		{
			AppMainMenu.CharacterOnScreen characterOnScreen;
			bool flag = !this.CharactersOnScreen.TryGetValue(evt.Id, out characterOnScreen);
			if (flag)
			{
				characterOnScreen = (this.CharactersOnScreen[evt.Id] = new AppMainMenu.CharacterOnScreen(this._app, evt.Viewport, evt.InitialModelAngle, evt.Scale));
			}
			else
			{
				characterOnScreen.Viewport = evt.Viewport;
				characterOnScreen.Scale = evt.Scale;
			}
			characterOnScreen.InitializeRendering(new ClientPlayerSkin(this.EditedSkin), this._app.CharacterPartStore);
		}

		// Token: 0x06006111 RID: 24849 RVA: 0x001FE264 File Offset: 0x001FC464
		public void RemoveCharacterFromScreen(string id)
		{
			AppMainMenu.CharacterOnScreen characterOnScreen;
			bool flag = this.CharactersOnScreen.TryGetValue(id, out characterOnScreen);
			if (flag)
			{
				characterOnScreen.Dispose();
				this.CharactersOnScreen.Remove(id);
			}
		}

		// Token: 0x06006112 RID: 24850 RVA: 0x001FE29A File Offset: 0x001FC49A
		public void ClearSkinEditHistory()
		{
			this._skinUndoStack.Clear();
			this._skinRedoStack.Clear();
		}

		// Token: 0x06006113 RID: 24851 RVA: 0x001FE2B5 File Offset: 0x001FC4B5
		public void CancelCharacter()
		{
			this.EditedSkin = new ClientPlayerSkin(this._app.PlayerSkin);
			this.UpdateCharacterSkinsOnScreen();
		}

		// Token: 0x06006114 RID: 24852 RVA: 0x001FE2D8 File Offset: 0x001FC4D8
		public void PlayCharacterEmote(string emoteId)
		{
			CharacterPartStore characterPartStore = this._app.CharacterPartStore;
			Emote emote = characterPartStore.Emotes.Find((Emote x) => x.Id == emoteId);
			bool flag = emote == null;
			if (!flag)
			{
				BlockyAnimation animation = new BlockyAnimation();
				BlockyAnimationInitializer.Parse(AssetManager.GetBuiltInAsset("Common/" + emote.Animation), this._app.CharacterPartStore.CharacterNodeNameManager, ref animation);
				Enumerable.First<KeyValuePair<string, AppMainMenu.CharacterOnScreen>>(this.CharactersOnScreen).Value.CharacterRenderer.SetSlotAnimation(1, animation, false, 1f, 0f, 12f, null, false);
			}
		}

		// Token: 0x06006115 RID: 24853 RVA: 0x001FE38C File Offset: 0x001FC58C
		public void ReloadCharacterAssets()
		{
			CancellationTokenSource reloadCharacterAssetsCancelTokenSource = this._reloadCharacterAssetsCancelTokenSource;
			if (reloadCharacterAssetsCancelTokenSource != null)
			{
				reloadCharacterAssetsCancelTokenSource.Cancel();
			}
			this._reloadCharacterAssetsCancelTokenSource = new CancellationTokenSource();
			CancellationToken cancelToken = this._reloadCharacterAssetsCancelTokenSource.Token;
			CharacterPartStore upcomingCharacterPartStore = new CharacterPartStore(this._app.Engine.Graphics.GL);
			ThreadPool.QueueUserWorkItem(delegate(object _)
			{
				bool textureAtlasNeedsUpdate = true;
				upcomingCharacterPartStore.LoadAssets(new HashSet<string>(), ref textureAtlasNeedsUpdate, cancelToken);
				byte[][] upcomingHairGradientAtlasPixels;
				upcomingCharacterPartStore.PrepareGradientAtlas(out upcomingHairGradientAtlasPixels);
				upcomingCharacterPartStore.LoadModelData(this._app.Engine, new HashSet<string>(), textureAtlasNeedsUpdate);
				this._app.Engine.RunOnMainThread(this._app.Engine, delegate
				{
					bool isCancellationRequested = cancelToken.IsCancellationRequested;
					if (!isCancellationRequested)
					{
						this._app.ReplaceCharacterPartStore(upcomingCharacterPartStore);
						this._app.CharacterPartStore.BuildGradientTexture(upcomingHairGradientAtlasPixels);
						bool flag = this._app.Stage == App.AppStage.MainMenu && this.CurrentPage == AppMainMenu.MainMenuPage.MyAvatar;
						if (flag)
						{
							this.UpdateCharacterSkinsOnScreen();
							this._app.Interface.MainMenuView.MyAvatarPage.OnCharacterChanged();
							this._app.Interface.MainMenuView.MyAvatarPage.OnAssetsReloaded();
						}
					}
				}, false, false);
			});
		}

		// Token: 0x06006116 RID: 24854 RVA: 0x001FE408 File Offset: 0x001FC608
		public void UndoCharacterSkinChange()
		{
			ClientPlayerSkin clientPlayerSkin = this._skinUndoStack.Pop();
			bool flag = clientPlayerSkin == null;
			if (!flag)
			{
				this._skinRedoStack.Push(new ClientPlayerSkin(this.EditedSkin));
				this.EditedSkin = clientPlayerSkin;
				this.UpdateCharacterSkinsOnScreen();
				this._app.Interface.MainMenuView.MyAvatarPage.OnCharacterChanged();
				this._app.Interface.MainMenuView.MyAvatarPage.OnSetCanUndoRedoSelection(this._skinUndoStack.Count > 0, this._skinRedoStack.Count > 0);
			}
		}

		// Token: 0x06006117 RID: 24855 RVA: 0x001FE4A4 File Offset: 0x001FC6A4
		public void RedoCharacterSkinChange()
		{
			ClientPlayerSkin clientPlayerSkin = this._skinRedoStack.Pop();
			bool flag = clientPlayerSkin == null;
			if (!flag)
			{
				this._skinUndoStack.Push(new ClientPlayerSkin(this.EditedSkin));
				this.EditedSkin = clientPlayerSkin;
				this.UpdateCharacterSkinsOnScreen();
				this._app.Interface.MainMenuView.MyAvatarPage.OnCharacterChanged();
				this._app.Interface.MainMenuView.MyAvatarPage.OnSetCanUndoRedoSelection(this._skinUndoStack.Count > 0, this._skinRedoStack.Count > 0);
			}
		}

		// Token: 0x06006118 RID: 24856 RVA: 0x001FE540 File Offset: 0x001FC740
		public void RandomizeCharacter(HashSet<PlayerSkinProperty> lockedProperties)
		{
			CharacterPartStore partStore = this._app.CharacterPartStore;
			this.UpdateEditedSkin(delegate
			{
				bool flag = !lockedProperties.Contains(PlayerSkinProperty.BodyType);
				if (flag)
				{
					this.EditedSkin.BodyType = ((this._characterRandom.NextDouble() > 0.5) ? CharacterBodyType.Masculine : CharacterBodyType.Feminine);
				}
				bool flag2 = !lockedProperties.Contains(PlayerSkinProperty.SkinTone);
				if (flag2)
				{
					CharacterPartGradientSet characterPartGradientSet = partStore.GradientSets["Skin"];
					this.EditedSkin.SkinTone = Enumerable.ElementAt<KeyValuePair<string, CharacterPartTintColor>>(characterPartGradientSet.Gradients, this._characterRandom.Next(characterPartGradientSet.Gradients.Count)).Key;
				}
				bool flag3 = !lockedProperties.Contains(PlayerSkinProperty.Face);
				if (flag3)
				{
					this.EditedSkin.Face = this.GetRandomCharacterAsset<CharacterPart>(partStore.Faces, false, null, null).PartId;
				}
				bool flag4 = !lockedProperties.Contains(PlayerSkinProperty.Haircut);
				if (flag4)
				{
					this.EditedSkin.Haircut = this.GetRandomCharacterAsset<CharacterHaircutPart>(partStore.Haircuts, true, null, null);
				}
				bool flag5 = !lockedProperties.Contains(PlayerSkinProperty.Pants);
				if (flag5)
				{
					this.EditedSkin.Pants = this.GetRandomCharacterAsset<CharacterPart>(partStore.Pants, true, null, null);
				}
				bool flag6 = !lockedProperties.Contains(PlayerSkinProperty.Overpants);
				if (flag6)
				{
					this.EditedSkin.Overpants = this.GetRandomCharacterAsset<CharacterPart>(partStore.Overpants, true, null, null);
				}
				bool flag7 = !lockedProperties.Contains(PlayerSkinProperty.Undertop);
				if (flag7)
				{
					this.EditedSkin.Undertop = this.GetRandomCharacterAsset<CharacterPart>(partStore.Undertops, true, null, null);
				}
				bool flag8 = !lockedProperties.Contains(PlayerSkinProperty.Overtop);
				if (flag8)
				{
					this.EditedSkin.Overtop = this.GetRandomCharacterAsset<CharacterPart>(partStore.Overtops, true, null, null);
				}
				bool flag9 = !lockedProperties.Contains(PlayerSkinProperty.Shoes);
				if (flag9)
				{
					this.EditedSkin.Shoes = this.GetRandomCharacterAsset<CharacterPart>(partStore.Shoes, true, null, null);
				}
				bool flag10 = !lockedProperties.Contains(PlayerSkinProperty.Eyebrows);
				if (flag10)
				{
					ClientPlayerSkin editedSkin = this.EditedSkin;
					AppMainMenu <>4__this = this;
					List<CharacterPart> eyebrows = partStore.Eyebrows;
					bool allowNull = true;
					CharacterPartId haircut = this.EditedSkin.Haircut;
					string matchColor = (haircut != null) ? haircut.ColorId : null;
					CharacterPartId facialHair = this.EditedSkin.FacialHair;
					editedSkin.Eyebrows = <>4__this.GetRandomCharacterAsset<CharacterPart>(eyebrows, allowNull, matchColor, (facialHair != null) ? facialHair.ColorId : null);
				}
				bool flag11 = !lockedProperties.Contains(PlayerSkinProperty.Eyes);
				if (flag11)
				{
					this.EditedSkin.Eyes = this.GetRandomCharacterAsset<CharacterPart>(partStore.Eyes, false, null, null);
				}
				bool flag12 = !lockedProperties.Contains(PlayerSkinProperty.FacialHair);
				if (flag12)
				{
					bool flag13 = this._characterRandom.Next(10) > 4 && this.EditedSkin.BodyType == CharacterBodyType.Masculine;
					if (flag13)
					{
						ClientPlayerSkin editedSkin2 = this.EditedSkin;
						AppMainMenu <>4__this2 = this;
						List<CharacterPart> facialHair2 = partStore.FacialHair;
						bool allowNull2 = true;
						CharacterPartId haircut2 = this.EditedSkin.Haircut;
						string matchColor2 = (haircut2 != null) ? haircut2.ColorId : null;
						CharacterPartId eyebrows2 = this.EditedSkin.Eyebrows;
						editedSkin2.FacialHair = <>4__this2.GetRandomCharacterAsset<CharacterPart>(facialHair2, allowNull2, matchColor2, (eyebrows2 != null) ? eyebrows2.ColorId : null);
					}
					else
					{
						this.EditedSkin.FacialHair = null;
					}
				}
				bool flag14 = !lockedProperties.Contains(PlayerSkinProperty.HeadAccessory);
				if (flag14)
				{
					bool flag15 = this._characterRandom.Next(10) > 8;
					if (flag15)
					{
						this.EditedSkin.HeadAccessory = this.GetRandomCharacterAsset<CharacterHeadAccessoryPart>(partStore.HeadAccessory, true, null, null);
					}
					else
					{
						this.EditedSkin.HeadAccessory = null;
					}
				}
				bool flag16 = !lockedProperties.Contains(PlayerSkinProperty.FaceAccessory);
				if (flag16)
				{
					bool flag17 = this._characterRandom.Next(10) > 8;
					if (flag17)
					{
						this.EditedSkin.FaceAccessory = this.GetRandomCharacterAsset<CharacterPart>(partStore.FaceAccessory, true, null, null);
					}
					else
					{
						this.EditedSkin.FaceAccessory = null;
					}
				}
				bool flag18 = !lockedProperties.Contains(PlayerSkinProperty.EarAccessory);
				if (flag18)
				{
					bool flag19 = this._characterRandom.Next(10) > 8;
					if (flag19)
					{
						this.EditedSkin.EarAccessory = this.GetRandomCharacterAsset<CharacterPart>(partStore.EarAccessory, true, null, null);
					}
					else
					{
						this.EditedSkin.EarAccessory = null;
					}
				}
				bool flag20 = !lockedProperties.Contains(PlayerSkinProperty.SkinFeature);
				if (flag20)
				{
					bool flag21 = this._characterRandom.Next(10) > 8;
					if (flag21)
					{
						this.EditedSkin.SkinFeature = this.GetRandomCharacterAsset<CharacterPart>(partStore.SkinFeatures, true, null, null);
					}
					else
					{
						this.EditedSkin.SkinFeature = null;
					}
				}
			}, true);
		}

		// Token: 0x06006119 RID: 24857 RVA: 0x001FE588 File Offset: 0x001FC788
		public void SetCharacterAsset(PlayerSkinProperty property, CharacterPartId id, bool updateInterface = true)
		{
			CharacterPartStore partStore = this._app.CharacterPartStore;
			this.UpdateEditedSkin(delegate
			{
				switch (property)
				{
				case PlayerSkinProperty.BodyType:
				{
					this.EditedSkin.BodyType = (CharacterBodyType)Enum.Parse(typeof(CharacterBodyType), id.PartId);
					this.EditedSkin.Eyes = partStore.GetDefaultPartIdFor(this.EditedSkin.BodyType, partStore.Eyes);
					bool flag = this.EditedSkin.Eyebrows != null;
					if (flag)
					{
						CharacterPart defaultPartFor = partStore.GetDefaultPartFor(this.EditedSkin.BodyType, partStore.Eyebrows);
						CharacterPartStore characterPartStore = this._app.CharacterPartStore;
						CharacterPart part = defaultPartFor;
						CharacterPartId eyebrows = this.EditedSkin.Eyebrows;
						List<string> colorOptions = characterPartStore.GetColorOptions(part, (eyebrows != null) ? eyebrows.VariantId : null);
						string colorId = colorOptions.Contains(this.EditedSkin.Eyebrows.ColorId) ? this.EditedSkin.Eyebrows.ColorId : Enumerable.First<string>(colorOptions);
						ClientPlayerSkin editedSkin = this.EditedSkin;
						string id2 = defaultPartFor.Id;
						CharacterPartId eyebrows2 = this.EditedSkin.Eyebrows;
						editedSkin.Eyebrows = new CharacterPartId(id2, (eyebrows2 != null) ? eyebrows2.VariantId : null, colorId);
					}
					this._app.Interface.MainMenuView.MyAvatarPage.OnCharacterChanged();
					break;
				}
				case PlayerSkinProperty.SkinTone:
					this.EditedSkin.SkinTone = id.PartId;
					break;
				case PlayerSkinProperty.Haircut:
					this.EditedSkin.Haircut = id;
					break;
				case PlayerSkinProperty.FacialHair:
					this.EditedSkin.FacialHair = id;
					break;
				case PlayerSkinProperty.Eyebrows:
					this.EditedSkin.Eyebrows = id;
					break;
				case PlayerSkinProperty.Eyes:
					this.EditedSkin.Eyes = id;
					break;
				case PlayerSkinProperty.Face:
					this.EditedSkin.Face = id.PartId;
					break;
				case PlayerSkinProperty.Pants:
					this.EditedSkin.Pants = id;
					break;
				case PlayerSkinProperty.Overpants:
					this.EditedSkin.Overpants = id;
					break;
				case PlayerSkinProperty.Undertop:
					this.EditedSkin.Undertop = id;
					break;
				case PlayerSkinProperty.Overtop:
					this.EditedSkin.Overtop = id;
					break;
				case PlayerSkinProperty.Shoes:
					this.EditedSkin.Shoes = id;
					break;
				case PlayerSkinProperty.HeadAccessory:
					this.EditedSkin.HeadAccessory = id;
					break;
				case PlayerSkinProperty.FaceAccessory:
					this.EditedSkin.FaceAccessory = id;
					break;
				case PlayerSkinProperty.EarAccessory:
					this.EditedSkin.EarAccessory = id;
					break;
				case PlayerSkinProperty.SkinFeature:
					this.EditedSkin.SkinFeature = id;
					break;
				case PlayerSkinProperty.Gloves:
					this.EditedSkin.Gloves = id;
					break;
				}
			}, updateInterface);
		}

		// Token: 0x0600611A RID: 24858 RVA: 0x001FE5D8 File Offset: 0x001FC7D8
		public JObject GetSkinJson()
		{
			CharacterPartStore partStore = this._app.CharacterPartStore;
			JArray attachments = new JArray();
			JObject jobject = new JObject();
			jobject["Name"] = "MyModel";
			jobject["Parent"] = "Player";
			jobject["Model"] = partStore.GetBodyModelPath(this.EditedSkin.BodyType).Replace("Common/", string.Empty);
			jobject["Texture"] = ((this.EditedSkin.BodyType == CharacterBodyType.Feminine) ? "Characters/Player_Textures/Feminine_Greyscale.png" : "Characters/Player_Textures/Masculine_Greyscale.png");
			jobject["GradientSet"] = this.EditedSkin.SkinTone;
			JObject jobject2 = jobject;
			partStore.GetCharacterAttachments(this.EditedSkin).ForEach(delegate(CharacterAttachment attachment)
			{
				JObject jobject3 = new JObject();
				jobject3.Add("Model", attachment.Model.Replace("Common/", ""));
				jobject3.Add("Texture", attachment.Texture.Replace("Common/", ""));
				JObject jobject4 = jobject3;
				string text;
				string text2;
				bool flag = attachment.GradientId != 0 && partStore.TryGetGradientByIndex(attachment.GradientId, out text, out text2);
				if (flag)
				{
					jobject4["GradientSet"] = text;
					jobject4["GradientId"] = text2;
				}
				attachments.Add(jobject4);
			});
			jobject2["Attachments"] = attachments;
			return jobject2;
		}

		// Token: 0x0600611B RID: 24859 RVA: 0x001FE6F4 File Offset: 0x001FC8F4
		private ClientPlayerSkin GetNakedCharacter(ClientPlayerSkin character)
		{
			bool flag = character != null;
			ClientPlayerSkin result;
			if (flag)
			{
				result = new ClientPlayerSkin
				{
					BodyType = character.BodyType,
					SkinTone = character.SkinTone,
					Eyes = character.Eyes,
					Eyebrows = character.Eyebrows,
					Face = character.Face
				};
			}
			else
			{
				CharacterPartStore characterPartStore = this._app.CharacterPartStore;
				CharacterBodyType bodyType = (this._characterRandom.NextDouble() > 0.5) ? CharacterBodyType.Masculine : CharacterBodyType.Feminine;
				Dictionary<string, CharacterPartTintColor> gradients = characterPartStore.GradientSets["Skin"].Gradients;
				result = new ClientPlayerSkin
				{
					BodyType = bodyType,
					SkinTone = Enumerable.ElementAt<KeyValuePair<string, CharacterPartTintColor>>(gradients, this._characterRandom.Next(gradients.Count)).Key,
					Face = Enumerable.ElementAt<CharacterPart>(characterPartStore.Faces, this._characterRandom.Next(characterPartStore.Faces.Count)).Id,
					Eyes = characterPartStore.GetDefaultPartIdFor(bodyType, characterPartStore.Eyes),
					Eyebrows = characterPartStore.GetDefaultPartIdFor(bodyType, characterPartStore.Eyebrows)
				};
			}
			return result;
		}

		// Token: 0x0600611C RID: 24860 RVA: 0x001FE81C File Offset: 0x001FCA1C
		private CharacterPartId GetRandomCharacterAsset<T>(List<T> assets, bool allowNull = true, string matchColor = null, string matchColor2 = null) where T : CharacterPart
		{
			int num = this._characterRandom.Next(assets.Count + (allowNull ? 1 : 0));
			bool flag = num == assets.Count;
			CharacterPartId result;
			if (flag)
			{
				result = null;
			}
			else
			{
				T t = assets[num];
				string variantId = null;
				bool flag2 = t.Variants != null;
				if (flag2)
				{
					variantId = Enumerable.ElementAt<string>(t.Variants.Keys, this._characterRandom.Next(t.Variants.Count));
				}
				List<string> colorOptions = this._app.CharacterPartStore.GetColorOptions(t, variantId);
				string text = null;
				bool flag3 = matchColor != null;
				if (flag3)
				{
					bool flag4 = colorOptions.Contains(matchColor);
					if (flag4)
					{
						text = matchColor;
					}
					else
					{
						bool flag5 = matchColor2 != null;
						if (flag5)
						{
							bool flag6 = colorOptions.Contains(matchColor2);
							if (flag6)
							{
								text = matchColor2;
							}
						}
					}
				}
				bool flag7 = text == null;
				if (flag7)
				{
					text = Enumerable.ElementAt<string>(colorOptions, this._characterRandom.Next(colorOptions.Count));
				}
				result = new CharacterPartId(t.Id, variantId, text);
			}
			return result;
		}

		// Token: 0x0600611D RID: 24861 RVA: 0x001FE948 File Offset: 0x001FCB48
		private void UpdateEditedSkin(Action setter, bool updateInterface = true)
		{
			ClientPlayerSkin clientPlayerSkin = new ClientPlayerSkin(this.EditedSkin);
			setter();
			bool flag = this.EditedSkin.Equals(clientPlayerSkin);
			if (!flag)
			{
				if (updateInterface)
				{
					this._app.Interface.MainMenuView.MyAvatarPage.OnCharacterChanged();
				}
				this.UpdateCharacterSkinsOnScreen();
				bool flag2 = this._skinRedoStack.Count > 0;
				if (flag2)
				{
					this._skinRedoStack.Clear();
				}
				ClientPlayerSkin clientPlayerSkin2 = this._skinUndoStack.Peek();
				bool flag3 = clientPlayerSkin2 != null && clientPlayerSkin2.Equals(clientPlayerSkin);
				if (!flag3)
				{
					this._skinUndoStack.Push(clientPlayerSkin);
					this._app.Interface.MainMenuView.MyAvatarPage.OnSetCanUndoRedoSelection(this._skinUndoStack.Count > 0, this._skinRedoStack.Count > 0);
				}
			}
		}

		// Token: 0x0600611E RID: 24862 RVA: 0x001FEA2C File Offset: 0x001FCC2C
		public void UpdateCharacterSkinsOnScreen()
		{
			foreach (AppMainMenu.CharacterOnScreen characterOnScreen in this.CharactersOnScreen.Values)
			{
				characterOnScreen.InitializeRendering(this.EditedSkin, this._app.CharacterPartStore);
			}
		}

		// Token: 0x0600611F RID: 24863 RVA: 0x001FEA98 File Offset: 0x001FCC98
		public void OpenAssetIdInCosmeticEditor(string assetType, string assetId)
		{
			IpcClient ipc = this._app.Ipc;
			string command = "OpenEditor";
			JObject jobject = new JObject();
			jobject.Add("Cosmetics", true);
			jobject.Add("AssetType", assetType);
			jobject.Add("AssetId", assetId);
			ipc.SendCommand(command, jobject);
		}

		// Token: 0x06006120 RID: 24864 RVA: 0x001FEAF7 File Offset: 0x001FCCF7
		public void OpenCosmeticEditor()
		{
			IpcClient ipc = this._app.Ipc;
			string command = "OpenEditor";
			JObject jobject = new JObject();
			jobject.Add("Cosmetics", true);
			ipc.SendCommand(command, jobject);
		}

		// Token: 0x06006121 RID: 24865 RVA: 0x001FEB28 File Offset: 0x001FCD28
		public void OnCharacterRotate(SDL.SDL_Event evt)
		{
			bool flag = this.CurrentPage != AppMainMenu.MainMenuPage.Home && this.CurrentPage != AppMainMenu.MainMenuPage.MyAvatar;
			if (!flag)
			{
				switch (evt.type)
				{
				case SDL.SDL_EventType.SDL_MOUSEMOTION:
					foreach (AppMainMenu.CharacterOnScreen characterOnScreen in this.CharactersOnScreen.Values)
					{
						bool flag2 = characterOnScreen.DragIfApplicable(evt);
						if (flag2)
						{
							break;
						}
					}
					break;
				case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
					foreach (AppMainMenu.CharacterOnScreen characterOnScreen2 in this.CharactersOnScreen.Values)
					{
						bool flag3 = characterOnScreen2.StartDraggingIfApplicable(evt);
						if (flag3)
						{
							break;
						}
					}
					break;
				case SDL.SDL_EventType.SDL_MOUSEBUTTONUP:
					foreach (AppMainMenu.CharacterOnScreen characterOnScreen3 in this.CharactersOnScreen.Values)
					{
						bool flag4 = characterOnScreen3.StopDraggingIfApplicable(evt);
						if (flag4)
						{
							break;
						}
					}
					break;
				}
			}
		}

		// Token: 0x06006122 RID: 24866 RVA: 0x001FEC8C File Offset: 0x001FCE8C
		public void ResetCharacters()
		{
			foreach (AppMainMenu.CharacterOnScreen characterOnScreen in this.CharactersOnScreen.Values)
			{
				characterOnScreen.SetRotation(-0.3926991f);
			}
		}

		// Token: 0x06006123 RID: 24867 RVA: 0x001FECEC File Offset: 0x001FCEEC
		private void DrawCharacters(float deltaTime)
		{
			GLFunctions gl = this._app.Engine.Graphics.GL;
			Rectangle viewport = this._app.Engine.Window.Viewport;
			foreach (AppMainMenu.CharacterOnScreen characterOnScreen in this.CharactersOnScreen.Values)
			{
				gl.Viewport(viewport.X + characterOnScreen.Viewport.X, viewport.Y + viewport.Height - (characterOnScreen.Viewport.Y + characterOnScreen.Viewport.Height), characterOnScreen.Viewport.Width, characterOnScreen.Viewport.Height);
				characterOnScreen.Draw(deltaTime);
			}
		}

		// Token: 0x17001407 RID: 5127
		// (get) Token: 0x06006124 RID: 24868 RVA: 0x001FEDCC File Offset: 0x001FCFCC
		// (set) Token: 0x06006125 RID: 24869 RVA: 0x001FEDD4 File Offset: 0x001FCFD4
		public AppMainMenu.ServerListTab ActiveServerListTab { get; private set; }

		// Token: 0x17001408 RID: 5128
		// (get) Token: 0x06006126 RID: 24870 RVA: 0x001FEDDD File Offset: 0x001FCFDD
		// (set) Token: 0x06006127 RID: 24871 RVA: 0x001FEDE5 File Offset: 0x001FCFE5
		public bool IsFetchingList { get; private set; }

		// Token: 0x06006128 RID: 24872 RVA: 0x001FEDF0 File Offset: 0x001FCFF0
		public void QueueForMinigame(string joinKey)
		{
			string text;
			bool flag = !this.CanConnectToServer("queue for " + joinKey, out text);
			if (!flag)
			{
				this._app.HytaleServices.JoinGameQueue(joinKey, null, true, null, null);
			}
		}

		// Token: 0x06006129 RID: 24873 RVA: 0x001FEE30 File Offset: 0x001FD030
		public bool CanConnectToServer(string attemptedAction, out string reason)
		{
			bool flag = this._app.HytaleServices.QueueTicket.Ticket != null;
			bool result;
			if (flag)
			{
				AppMainMenu.Logger.Warn<string, string>("Tried to {0} but already queued for {1}", attemptedAction, this._app.HytaleServices.QueueTicket.Ticket);
				reason = "Already queued for " + this._app.HytaleServices.QueueTicket.Ticket + ".";
				result = false;
			}
			else
			{
				reason = null;
				result = true;
			}
			return result;
		}

		// Token: 0x0600612A RID: 24874 RVA: 0x001FEEB4 File Offset: 0x001FD0B4
		public void TryConnectToServer(Server server)
		{
			bool flag = server == null;
			if (flag)
			{
				AppMainMenu.Logger.Warn("Error whilst connecting to server because server object is null");
			}
			else
			{
				this.AddServerToRecentServers(server.UUID);
				string text;
				bool flag2 = !this.CanConnectToServer("connect to " + server.Host, out text);
				if (!flag2)
				{
					AppMainMenu.Logger.Info<string, string>("Connecting to multiplayer server \"{0}\" at {1}", server.Name, server.Host);
					string hostname;
					int port;
					string argument;
					bool flag3 = !HostnameHelper.TryParseHostname(server.Host, 5520, out hostname, out port, out argument);
					if (flag3)
					{
						AppMainMenu.Logger.Warn("Invalid address: {0}", argument);
					}
					else
					{
						this._app.Interface.FadeOut(delegate
						{
							this._app.GameLoading.Open(hostname, port, null);
							this._app.Interface.FadeIn(null, false);
						});
					}
				}
			}
		}

		// Token: 0x0600612B RID: 24875 RVA: 0x001FEF94 File Offset: 0x001FD194
		private CancellationToken GetNewFetchCancelToken()
		{
			CancellationTokenSource serverFetchCancellationToken = this._serverFetchCancellationToken;
			if (serverFetchCancellationToken != null)
			{
				serverFetchCancellationToken.Cancel();
			}
			this._serverFetchCancellationToken = new CancellationTokenSource();
			return this._serverFetchCancellationToken.Token;
		}

		// Token: 0x0600612C RID: 24876 RVA: 0x001FEFD0 File Offset: 0x001FD1D0
		public void FetchAndShowPublicServers(string name = null, string[] tags = null)
		{
			this.CancelFetchServerDetails();
			ServersPage serversPage = this._app.Interface.MainMenuView.ServersPage;
			bool flag = this.ActiveServerListTab > AppMainMenu.ServerListTab.Internet;
			if (flag)
			{
				this.ActiveServerListTab = AppMainMenu.ServerListTab.Internet;
				serversPage.OnActiveTabChanged(false);
			}
			else
			{
				bool flag2 = tags == null;
				if (flag2)
				{
					serversPage.ClearTags();
				}
			}
			this.IsFetchingList = true;
			serversPage.BuildServerList();
			CancellationToken cancelToken = this.GetNewFetchCancelToken();
			HytaleServicesApiClient.FetchPublicServerQuery query = new HytaleServicesApiClient.FetchPublicServerQuery
			{
				Name = name,
				Tags = tags
			};
			this._app.HytaleServicesApi.FetchPublicServers(query, 50, HytaleServicesApiClient.SortOrder.Ascending, HytaleServicesApiClient.ServerSortField.Name).ContinueWith(delegate(Task<Server[]> t)
			{
				bool isFaulted = t.IsFaulted;
				if (isFaulted)
				{
					AppMainMenu.Logger.Error(t.Exception, "Failed to fetch public servers from API");
				}
				this._app.Engine.RunOnMainThread(this._app.Engine, delegate
				{
					bool isCancellationRequested = cancelToken.IsCancellationRequested;
					if (!isCancellationRequested)
					{
						this.IsFetchingList = false;
						this._app.Interface.MainMenuView.ServersPage.OnServersReceived(t.IsFaulted ? null : t.Result);
					}
				}, false, false);
			});
		}

		// Token: 0x0600612D RID: 24877 RVA: 0x001FF09C File Offset: 0x001FD29C
		public void FetchAndShowFavoriteServers()
		{
			this.CancelFetchServerDetails();
			ServersPage serversPage = this._app.Interface.MainMenuView.ServersPage;
			bool flag = this.ActiveServerListTab != AppMainMenu.ServerListTab.Favorites;
			if (flag)
			{
				this.ActiveServerListTab = AppMainMenu.ServerListTab.Favorites;
				serversPage.OnActiveTabChanged(true);
			}
			this.IsFetchingList = true;
			serversPage.BuildServerList();
			CancellationToken cancelToken = this.GetNewFetchCancelToken();
			this._app.HytaleServicesApi.FetchFavoriteServers(this._app.AuthManager.Settings.Uuid).ContinueWith(delegate(Task<Server[]> t)
			{
				bool isFaulted = t.IsFaulted;
				if (isFaulted)
				{
					AppMainMenu.Logger.Error(t.Exception, "Failed to fetch public servers from API");
				}
				this._app.Engine.RunOnMainThread(this._app.Engine, delegate
				{
					bool isCancellationRequested = cancelToken.IsCancellationRequested;
					if (!isCancellationRequested)
					{
						this.IsFetchingList = false;
						this._app.Interface.MainMenuView.ServersPage.OnServersReceived(t.IsFaulted ? null : t.Result);
					}
				}, false, false);
			});
		}

		// Token: 0x0600612E RID: 24878 RVA: 0x001FF148 File Offset: 0x001FD348
		public void FetchAndShowRecentServers()
		{
			this.CancelFetchServerDetails();
			ServersPage serversPage = this._app.Interface.MainMenuView.ServersPage;
			bool flag = this.ActiveServerListTab != AppMainMenu.ServerListTab.Recent;
			if (flag)
			{
				this.ActiveServerListTab = AppMainMenu.ServerListTab.Recent;
				serversPage.OnActiveTabChanged(true);
			}
			this.IsFetchingList = true;
			serversPage.BuildServerList();
			CancellationToken cancelToken = this.GetNewFetchCancelToken();
			this._app.HytaleServicesApi.FetchRecentServers(this._app.AuthManager.Settings.Uuid).ContinueWith(delegate(Task<Server[]> t)
			{
				bool isFaulted = t.IsFaulted;
				if (isFaulted)
				{
					AppMainMenu.Logger.Error(t.Exception, "Failed to fetch public servers from API");
				}
				this._app.Engine.RunOnMainThread(this._app.Engine, delegate
				{
					bool isCancellationRequested = cancelToken.IsCancellationRequested;
					if (!isCancellationRequested)
					{
						this.IsFetchingList = false;
						this._app.Interface.MainMenuView.ServersPage.OnServersReceived(t.IsFaulted ? null : t.Result);
					}
				}, false, false);
			});
		}

		// Token: 0x0600612F RID: 24879 RVA: 0x001FF1F4 File Offset: 0x001FD3F4
		public void AddServerToRecentServers(Guid serverUuid)
		{
			this._app.HytaleServicesApi.AddServerToRecents(serverUuid, this._app.AuthManager.Settings.Uuid).ContinueWith(delegate(Task t)
			{
				bool flag = !t.IsFaulted;
				if (!flag)
				{
					AppMainMenu.Logger.Error(t.Exception, "Failed to add server to recents");
				}
			});
		}

		// Token: 0x06006130 RID: 24880 RVA: 0x001FF24D File Offset: 0x001FD44D
		public void CancelFetchServerDetails()
		{
			CancellationTokenSource serverFetchDetailsCancellationToken = this._serverFetchDetailsCancellationToken;
			if (serverFetchDetailsCancellationToken != null)
			{
				serverFetchDetailsCancellationToken.Cancel();
			}
			this._serverFetchDetailsCancellationToken = null;
		}

		// Token: 0x06006131 RID: 24881 RVA: 0x001FF26C File Offset: 0x001FD46C
		public void FetchServerDetails(Guid serverUuid)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			CancellationTokenSource serverFetchDetailsCancellationToken = this._serverFetchDetailsCancellationToken;
			if (serverFetchDetailsCancellationToken != null)
			{
				serverFetchDetailsCancellationToken.Cancel();
			}
			this._serverFetchDetailsCancellationToken = new CancellationTokenSource();
			CancellationToken token = this._serverFetchCancellationToken.Token;
			this._app.Interface.MainMenuView.ServersPage.SetSelectedServerDetails(null, true);
			Task.Run(delegate()
			{
				Server server = null;
				try
				{
					server = this._app.HytaleServicesApi.FetchServerDetails(serverUuid).GetAwaiter().GetResult();
					bool isCancellationRequested = token.IsCancellationRequested;
					if (isCancellationRequested)
					{
						return;
					}
					Server[] result = this._app.HytaleServicesApi.FetchFavoriteServers(this._app.AuthManager.Settings.Uuid).GetAwaiter().GetResult();
					foreach (Server server2 in result)
					{
						bool flag = !server2.UUID.Equals(server.UUID);
						if (!flag)
						{
							server.IsFavorite = true;
							break;
						}
					}
				}
				catch (Exception ex)
				{
					Logger logger = AppMainMenu.Logger;
					Exception exception = ex;
					string str = "Failed to fetch details for server ";
					Guid serverUuid2 = serverUuid;
					logger.Error(exception, str + serverUuid2.ToString());
				}
				this._app.Engine.RunOnMainThread(this._app.Engine, delegate
				{
					bool isCancellationRequested2 = token.IsCancellationRequested;
					if (!isCancellationRequested2)
					{
						this._app.Interface.MainMenuView.ServersPage.SetSelectedServerDetails(server, true);
					}
				}, false, false);
			});
		}

		// Token: 0x06006132 RID: 24882 RVA: 0x001FF2F6 File Offset: 0x001FD4F6
		public void AddServerToFavorites(Guid serverUuid)
		{
			this._app.HytaleServicesApi.AddServerToFavorites(serverUuid, this._app.AuthManager.Settings.Uuid).ContinueWith(delegate(Task t)
			{
				bool isFaulted = t.IsFaulted;
				if (isFaulted)
				{
					AppMainMenu.Logger.Error(t.Exception, "Failed to add server to favorites");
				}
				this._app.Engine.RunOnMainThread(this._app.Engine, delegate
				{
					bool flag = !t.IsFaulted;
					if (!flag)
					{
						this._app.Interface.MainMenuView.ServersPage.OnFailedToToggleFavoriteServer(t.Exception.Message);
					}
				}, false, false);
			});
		}

		// Token: 0x06006133 RID: 24883 RVA: 0x001FF331 File Offset: 0x001FD531
		public void RemoveServerFromFavorites(Guid serverUuid)
		{
			this._app.HytaleServicesApi.RemoveServerFromFavorites(serverUuid, this._app.AuthManager.Settings.Uuid).ContinueWith(delegate(Task t)
			{
				bool isFaulted = t.IsFaulted;
				if (isFaulted)
				{
					AppMainMenu.Logger.Error(t.Exception, "Failed to remove server from favorites");
				}
				this._app.Engine.RunOnMainThread(this._app.Engine, delegate
				{
					bool flag = !t.IsFaulted;
					if (!flag)
					{
						this._app.Interface.MainMenuView.ServersPage.OnFailedToToggleFavoriteServer(t.Exception.Message);
					}
				}, false, false);
			});
		}

		// Token: 0x06006134 RID: 24884 RVA: 0x001FF36C File Offset: 0x001FD56C
		public void RebootServer(string connectionAddress)
		{
			string hostname;
			int port;
			string argument;
			bool flag = !HostnameHelper.TryParseHostname(connectionAddress, 5520, out hostname, out port, out argument);
			if (flag)
			{
				AppMainMenu.Logger.Warn("Failed to parse hostname: {0}", argument);
			}
			else
			{
				ConnectionToServer connection = null;
				connection = new ConnectionToServer(this._app.Engine, hostname, port, delegate(Exception exception)
				{
					bool flag2 = exception == null;
					if (flag2)
					{
						AppMainMenu.Logger.Info("Connected to server: {0}", connectionAddress);
						connection.SendPacketImmediate(new Connect("f4c63561b2d2f5120b4c81ad1b8544e396088277d88f650aea892b6f0cb113f", 1643968234458L, 5, Language.SystemLanguage));
						connection.Close();
					}
				}, delegate(Exception exception)
				{
					AppMainMenu.Logger.Info("Disconnected from server: {0}", connectionAddress);
					AppMainMenu.Logger.Error<Exception>(exception);
				});
			}
		}

		// Token: 0x06006135 RID: 24885 RVA: 0x001FF3F4 File Offset: 0x001FD5F4
		public void ShowFriendsServers()
		{
			ServersPage serversPage = this._app.Interface.MainMenuView.ServersPage;
			bool flag = this.ActiveServerListTab != AppMainMenu.ServerListTab.Friends;
			if (flag)
			{
				this.ActiveServerListTab = AppMainMenu.ServerListTab.Friends;
				serversPage.OnActiveTabChanged(true);
			}
			serversPage.OnServersReceived(null);
		}

		// Token: 0x06006137 RID: 24887 RVA: 0x001FF478 File Offset: 0x001FD678
		[CompilerGenerated]
		internal static CharacterPartId <SetupCharacter>g__GetCharacterPartId|81_0(string jsonKey, PlayerSkinProperty property, CharacterPartId defaultValue = null, ref AppMainMenu.<>c__DisplayClass81_0 A_3, ref AppMainMenu.<>c__DisplayClass81_1 A_4)
		{
			bool flag = A_3.meta.ContainsKey(jsonKey);
			if (flag)
			{
				string text = (string)A_3.meta[jsonKey];
				bool flag2 = text == null;
				if (flag2)
				{
					return defaultValue;
				}
				CharacterPartId characterPartId = CharacterPartId.FromString(text);
				CharacterPart characterPart;
				bool flag3 = !A_4.partStore.TryGetCharacterPart(property, characterPartId.PartId, out characterPart);
				if (flag3)
				{
					return defaultValue;
				}
				bool flag4 = characterPartId.VariantId != null;
				if (flag4)
				{
					CharacterPartVariant characterPartVariant;
					bool flag5 = characterPart.Variants == null || !characterPart.Variants.TryGetValue(characterPartId.VariantId, out characterPartVariant);
					if (flag5)
					{
						return defaultValue;
					}
					bool flag6 = characterPartVariant.Textures != null && characterPartVariant.Textures.ContainsKey(characterPartId.ColorId);
					if (flag6)
					{
						return characterPartId;
					}
				}
				else
				{
					bool flag7 = characterPart.Textures != null && characterPart.Textures.ContainsKey(characterPartId.ColorId);
					if (flag7)
					{
						return characterPartId;
					}
				}
				CharacterPartGradientSet characterPartGradientSet;
				bool flag8 = characterPart.GradientSet != null && A_4.partStore.GradientSets.TryGetValue(characterPart.GradientSet, out characterPartGradientSet) && characterPartGradientSet.Gradients.ContainsKey(characterPartId.ColorId);
				if (flag8)
				{
					return characterPartId;
				}
			}
			return defaultValue;
		}

		// Token: 0x04003C39 RID: 15417
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003C3A RID: 15418
		private const string MainMenuWWiseId = "MENU_INIT";

		// Token: 0x04003C3B RID: 15419
		private const int MusicFadeDuration = 5000;

		// Token: 0x04003C3C RID: 15420
		private readonly App _app;

		// Token: 0x04003C3E RID: 15422
		public SceneRenderer SceneRenderer;

		// Token: 0x04003C3F RID: 15423
		public PostEffectRenderer PostEffectRenderer;

		// Token: 0x04003C40 RID: 15424
		public GLTexture _defaultBackgroundTexture;

		// Token: 0x04003C41 RID: 15425
		public GLTexture _defaultBlurredBackgroundTexture;

		// Token: 0x04003C42 RID: 15426
		public GLTexture _characterCreatorBackgroundTexture;

		// Token: 0x04003C43 RID: 15427
		public QuadRenderer _backgroundRenderer;

		// Token: 0x04003C44 RID: 15428
		private int[] _mainMenuMusicFileIndices;

		// Token: 0x04003C45 RID: 15429
		private int _musicTrackIndex;

		// Token: 0x04003C46 RID: 15430
		private bool _musicIsFadingOut;

		// Token: 0x04003C47 RID: 15431
		private int _musicPlaybackId = -1;

		// Token: 0x04003C48 RID: 15432
		public readonly List<AppMainMenu.World> Worlds = new List<AppMainMenu.World>();

		// Token: 0x04003C49 RID: 15433
		private const int LanDiscoveryBroadcastPort = 5510;

		// Token: 0x04003C4A RID: 15434
		private const int LanDiscoveryPort = 5511;

		// Token: 0x04003C4B RID: 15435
		private static readonly byte[] LanDiscoveryReplyHeader = Encoding.ASCII.GetBytes("HYTALE_DISCOVER_REPLY");

		// Token: 0x04003C4C RID: 15436
		private static readonly byte[] LanDiscoveryRequestHeader = Encoding.ASCII.GetBytes("HYTALE_DISCOVER_REQUEST");

		// Token: 0x04003C4D RID: 15437
		private readonly object _newlyDiscoveredLanServersLock = new object();

		// Token: 0x04003C4E RID: 15438
		private readonly HashSet<Server> _newlyDiscoveredLanServers = new HashSet<Server>();

		// Token: 0x04003C50 RID: 15440
		private readonly object _lanDiscoverySocketLock = new object();

		// Token: 0x04003C51 RID: 15441
		private UdpClient _lanDiscoverySocket;

		// Token: 0x04003C52 RID: 15442
		private float _lanDiscoverTimer;

		// Token: 0x04003C54 RID: 15444
		private const int CharacterAssetPreviewWidth = 92;

		// Token: 0x04003C55 RID: 15445
		private const int CharacterAssetPreviewHeight = 149;

		// Token: 0x04003C56 RID: 15446
		private bool _useMSAAForAssetPreview = false;

		// Token: 0x04003C57 RID: 15447
		private RenderTarget _characterAssetPreviewRenderTarget;

		// Token: 0x04003C58 RID: 15448
		private RenderTarget _characterAssetFinalPreviewRenderTarget;

		// Token: 0x04003C59 RID: 15449
		private AppMainMenu.CharacterOnScreen _characterOnScreen;

		// Token: 0x04003C5A RID: 15450
		private Texture _characterAssetPreviewSelectionBackgroundTexture;

		// Token: 0x04003C5B RID: 15451
		private QuadRenderer _characterAssetPreviewBackgroundQuadRenderer;

		// Token: 0x04003C5C RID: 15452
		private readonly Random _characterRandom = new Random();

		// Token: 0x04003C5D RID: 15453
		private readonly DropOutStack<ClientPlayerSkin> _skinUndoStack = new DropOutStack<ClientPlayerSkin>(100);

		// Token: 0x04003C5E RID: 15454
		private readonly DropOutStack<ClientPlayerSkin> _skinRedoStack = new DropOutStack<ClientPlayerSkin>(100);

		// Token: 0x04003C5F RID: 15455
		private readonly Dictionary<string, AppMainMenu.CharacterOnScreen> CharactersOnScreen = new Dictionary<string, AppMainMenu.CharacterOnScreen>();

		// Token: 0x04003C60 RID: 15456
		private CancellationTokenSource _reloadCharacterAssetsCancelTokenSource;

		// Token: 0x04003C61 RID: 15457
		private CancellationTokenSource _serverFetchCancellationToken;

		// Token: 0x04003C62 RID: 15458
		private CancellationTokenSource _serverFetchDetailsCancellationToken;

		// Token: 0x0200102F RID: 4143
		private class CharacterOnScreen : Disposable
		{
			// Token: 0x06006A86 RID: 27270 RVA: 0x002230B0 File Offset: 0x002212B0
			public CharacterOnScreen(App app, Rectangle viewport, float initialModelAngle = -0.3926991f, float scale = 1f)
			{
				this._app = app;
				this.Viewport = viewport;
				this.Scale = scale;
				this.SetRotation(initialModelAngle);
			}

			// Token: 0x06006A87 RID: 27271 RVA: 0x002230FD File Offset: 0x002212FD
			protected override void DoDispose()
			{
				ModelRenderer characterRenderer = this.CharacterRenderer;
				if (characterRenderer != null)
				{
					characterRenderer.Dispose();
				}
			}

			// Token: 0x06006A88 RID: 27272 RVA: 0x00223114 File Offset: 0x00221314
			public void InitializeRendering(ClientPlayerSkin skin, CharacterPartStore characterPartStore)
			{
				float aspectRatio = (float)this.Viewport.Width / (float)this.Viewport.Height;
				this._app.Engine.Graphics.CreatePerspectiveMatrix(0.7853982f, aspectRatio, 0.1f, 1000f, out this._projectionMatrix);
				Dictionary<string, CharacterPartTintColor> gradients = characterPartStore.GradientSets["Skin"].Gradients;
				CharacterPartTintColor value;
				bool flag = !gradients.TryGetValue(skin.SkinTone, out value);
				if (flag)
				{
					value = Enumerable.First<KeyValuePair<string, CharacterPartTintColor>>(gradients).Value;
				}
				BlockyModel andCloneModel = characterPartStore.GetAndCloneModel(characterPartStore.GetBodyModelPath(skin.BodyType));
				andCloneModel.OffsetUVs(characterPartStore.ImageLocations[(skin.BodyType == CharacterBodyType.Masculine) ? "Characters/Player_Textures/Masculine_Greyscale.png" : "Characters/Player_Textures/Feminine_Greyscale.png"]);
				andCloneModel.SetGradientId(value.GradientId);
				foreach (CharacterAttachment characterAttachment in characterPartStore.GetCharacterAttachments(skin))
				{
					bool flag2 = characterAttachment.Model == null;
					if (flag2)
					{
						AppMainMenu.CharacterOnScreen.Logger.Warn("Model is not assigned");
					}
					else
					{
						BlockyModel blockyModel = characterPartStore.GetAndCloneModel(characterAttachment.Model);
						bool flag3 = blockyModel == null;
						if (flag3)
						{
							AppMainMenu.CharacterOnScreen.Logger.Warn("Tried to clone model which is not loaded or does not exist: {0}", characterAttachment.Model);
						}
						else
						{
							Point value2;
							bool flag4 = !characterPartStore.ImageLocations.TryGetValue(characterAttachment.Texture, out value2);
							if (flag4)
							{
								AppMainMenu.CharacterOnScreen.Logger.Warn("Tried to get model texture which is not loaded or does not exist: {0}", characterAttachment.Texture);
							}
							else
							{
								bool isUsingBaseNodeOnly = characterAttachment.IsUsingBaseNodeOnly;
								if (isUsingBaseNodeOnly)
								{
									BlockyModelNode blockyModelNode = blockyModel.AllNodes[0].Clone();
									BlockyModelNode blockyModelNode2 = blockyModel.AllNodes[1].Clone();
									blockyModel = new BlockyModel(2);
									blockyModel.AddNode(ref blockyModelNode, -1);
									blockyModel.AddNode(ref blockyModelNode2, 0);
								}
								blockyModel.GradientId = characterAttachment.GradientId;
								BlockyModel blockyModel2 = andCloneModel;
								BlockyModel attachment = blockyModel;
								NodeNameManager characterNodeNameManager = characterPartStore.CharacterNodeNameManager;
								Point? uvOffset = new Point?(value2);
								blockyModel2.Attach(attachment, characterNodeNameManager, null, uvOffset, -1);
							}
						}
					}
				}
				ModelRenderer characterRenderer = this.CharacterRenderer;
				float startTime = (characterRenderer != null) ? characterRenderer.GetSlotAnimationTime(0) : 0f;
				BlockyAnimation blockyAnimation = null;
				float startTime2 = 0f;
				ModelRenderer characterRenderer2 = this.CharacterRenderer;
				bool flag5 = ((characterRenderer2 != null) ? characterRenderer2.GetSlotAnimation(1) : null) != null;
				if (flag5)
				{
					startTime2 = this.CharacterRenderer.GetSlotAnimationTime(1);
					blockyAnimation = this.CharacterRenderer.GetSlotAnimation(1);
				}
				ModelRenderer characterRenderer3 = this.CharacterRenderer;
				if (characterRenderer3 != null)
				{
					characterRenderer3.Dispose();
				}
				this.CharacterRenderer = new ModelRenderer(andCloneModel, characterPartStore.AtlasSizes, this._app.Engine.Graphics, 0U, true);
				this.CharacterRenderer.SetSlotAnimation(0, characterPartStore.IdleAnimation, true, 1f, startTime, 0f, null, false);
				bool flag6 = blockyAnimation != null;
				if (flag6)
				{
					this.CharacterRenderer.SetSlotAnimation(1, blockyAnimation, false, 1f, startTime2, 0f, null, false);
				}
			}

			// Token: 0x06006A89 RID: 27273 RVA: 0x00223448 File Offset: 0x00221648
			public bool StartDraggingIfApplicable(SDL.SDL_Event evt)
			{
				Point value = this._app.Engine.Window.TransformSDLToViewportCoords(evt.button.x, evt.button.y);
				bool flag = evt.button.button != 1 || !this.Viewport.Contains(value);
				bool result;
				if (flag)
				{
					result = false;
				}
				else
				{
					this._isMouseDragging = true;
					this._dragStartMouseX = evt.button.x;
					this._dragStartCharacterModelAngle = this._characterModelAngle;
					result = true;
				}
				return result;
			}

			// Token: 0x06006A8A RID: 27274 RVA: 0x002234D4 File Offset: 0x002216D4
			public bool StopDraggingIfApplicable(SDL.SDL_Event evt)
			{
				bool flag = evt.button.button != 1 || !this._isMouseDragging;
				bool result;
				if (flag)
				{
					result = false;
				}
				else
				{
					this._isMouseDragging = false;
					result = true;
				}
				return result;
			}

			// Token: 0x06006A8B RID: 27275 RVA: 0x00223510 File Offset: 0x00221710
			public void StopDragging()
			{
				this._isMouseDragging = false;
			}

			// Token: 0x06006A8C RID: 27276 RVA: 0x0022351C File Offset: 0x0022171C
			public bool DragIfApplicable(SDL.SDL_Event evt)
			{
				bool flag = !this._isMouseDragging;
				bool result;
				if (flag)
				{
					result = false;
				}
				else
				{
					this._characterModelAngle = this._dragStartCharacterModelAngle + (float)(evt.motion.x - this._dragStartMouseX) / 64f;
					result = true;
				}
				return result;
			}

			// Token: 0x06006A8D RID: 27277 RVA: 0x00223568 File Offset: 0x00221768
			public void SetRotation(float rotation = -0.3926991f)
			{
				this._lerpCharacterModelAngle = rotation;
				this._characterModelAngle = rotation;
			}

			// Token: 0x06006A8E RID: 27278 RVA: 0x00223588 File Offset: 0x00221788
			public void Draw(float deltaTime)
			{
				GLFunctions gl = this._app.Engine.Graphics.GL;
				BlockyModelProgram blockyModelForwardProgram = this._app.Engine.Graphics.GPUProgramStore.BlockyModelForwardProgram;
				blockyModelForwardProgram.AssertInUse();
				gl.AssertEnabled(GL.DEPTH_TEST);
				bool flag = this.CharacterRenderer.GetSlotAnimation(1) != null && !this.CharacterRenderer.IsSlotPlayingAnimation(1);
				if (flag)
				{
					this.CharacterRenderer.SetSlotAnimation(1, null, true, 1f, 0f, 12f, null, false);
				}
				this.CharacterRenderer.AdvancePlayback(deltaTime * 60f);
				this.CharacterRenderer.UpdatePose();
				this.CharacterRenderer.SendDataToGPU();
				this._lerpCharacterModelAngle = MathHelper.Lerp(this._lerpCharacterModelAngle, this._characterModelAngle, MathHelper.Min(1f, 10f * deltaTime));
				Matrix matrix = Matrix.CreateRotationY(this._lerpCharacterModelAngle);
				Matrix matrix2 = Matrix.CreateScale(0.083333336f * this.Scale);
				Matrix matrix3 = matrix * matrix2 * Matrix.CreateTranslation(this.Translation);
				blockyModelForwardProgram.ViewProjectionMatrix.SetValue(ref this._projectionMatrix);
				blockyModelForwardProgram.ModelMatrix.SetValue(ref matrix3);
				blockyModelForwardProgram.NodeBlock.SetBuffer(this.CharacterRenderer.NodeBuffer);
				this.CharacterRenderer.Draw();
			}

			// Token: 0x04004D5B RID: 19803
			private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

			// Token: 0x04004D5C RID: 19804
			public const int IdleAnimationSlotIndex = 0;

			// Token: 0x04004D5D RID: 19805
			public const int EmoteAnimationSlotIndex = 1;

			// Token: 0x04004D5E RID: 19806
			public const float EmoteBlendingDuration = 12f;

			// Token: 0x04004D5F RID: 19807
			private const float DefaultCharacterModelAngle = -0.3926991f;

			// Token: 0x04004D60 RID: 19808
			private float _characterModelAngle;

			// Token: 0x04004D61 RID: 19809
			private float _lerpCharacterModelAngle;

			// Token: 0x04004D62 RID: 19810
			private bool _isMouseDragging;

			// Token: 0x04004D63 RID: 19811
			private int _dragStartMouseX;

			// Token: 0x04004D64 RID: 19812
			private float _dragStartCharacterModelAngle;

			// Token: 0x04004D65 RID: 19813
			public Rectangle Viewport;

			// Token: 0x04004D66 RID: 19814
			public ModelRenderer CharacterRenderer;

			// Token: 0x04004D67 RID: 19815
			public Vector3 Translation = new Vector3(0f, -5.3f, -16f);

			// Token: 0x04004D68 RID: 19816
			private Matrix _projectionMatrix;

			// Token: 0x04004D69 RID: 19817
			public float Scale;

			// Token: 0x04004D6A RID: 19818
			private readonly App _app;
		}

		// Token: 0x02001030 RID: 4144
		public enum MainMenuPage
		{
			// Token: 0x04004D6C RID: 19820
			Home,
			// Token: 0x04004D6D RID: 19821
			Servers,
			// Token: 0x04004D6E RID: 19822
			Minigames,
			// Token: 0x04004D6F RID: 19823
			Adventure,
			// Token: 0x04004D70 RID: 19824
			WorldOptions,
			// Token: 0x04004D71 RID: 19825
			MyAvatar,
			// Token: 0x04004D72 RID: 19826
			Settings,
			// Token: 0x04004D73 RID: 19827
			SharedSinglePlayer
		}

		// Token: 0x02001031 RID: 4145
		public class World
		{
			// Token: 0x04004D74 RID: 19828
			public string Path;

			// Token: 0x04004D75 RID: 19829
			public string LastWriteTime;

			// Token: 0x04004D76 RID: 19830
			public AppMainMenu.WorldOptions Options;

			// Token: 0x04004D77 RID: 19831
			public bool HasPreviewImage;

			// Token: 0x04004D78 RID: 19832
			public string LastPreviewImageWriteTime;
		}

		// Token: 0x02001032 RID: 4146
		public class WorldOptions
		{
			// Token: 0x04004D79 RID: 19833
			public string Name;

			// Token: 0x04004D7A RID: 19834
			public bool FlatWorld;

			// Token: 0x04004D7B RID: 19835
			public bool NpcSpawning;

			// Token: 0x04004D7C RID: 19836
			public GameMode GameMode;
		}

		// Token: 0x02001033 RID: 4147
		public class RenderCharacterPartPreviewCommand
		{
			// Token: 0x04004D7D RID: 19837
			public CharacterPartId Id;

			// Token: 0x04004D7E RID: 19838
			public PlayerSkinProperty Property;

			// Token: 0x04004D7F RID: 19839
			public bool Selected;

			// Token: 0x04004D80 RID: 19840
			public ColorRgba? BackgroundColor;
		}

		// Token: 0x02001034 RID: 4148
		public class AddCharacterOnScreenEvent
		{
			// Token: 0x04004D81 RID: 19841
			public Rectangle Viewport;

			// Token: 0x04004D82 RID: 19842
			public string Id;

			// Token: 0x04004D83 RID: 19843
			public float InitialModelAngle;

			// Token: 0x04004D84 RID: 19844
			public float Scale;
		}

		// Token: 0x02001035 RID: 4149
		public enum ServerListTab
		{
			// Token: 0x04004D86 RID: 19846
			Internet,
			// Token: 0x04004D87 RID: 19847
			Recent,
			// Token: 0x04004D88 RID: 19848
			Favorites,
			// Token: 0x04004D89 RID: 19849
			Friends,
			// Token: 0x04004D8A RID: 19850
			Lan
		}
	}
}
