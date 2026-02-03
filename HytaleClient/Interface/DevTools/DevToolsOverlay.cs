using System;
using System.Collections.Generic;
using System.Linq;
using HytaleClient.Application;
using HytaleClient.Core;
using HytaleClient.Data.Map;
using HytaleClient.Graphics;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules;
using HytaleClient.InGame.Modules.Map;
using HytaleClient.InGame.Modules.WorldMap;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using HytaleClient.Utils;
using SDL2;

namespace HytaleClient.Interface.DevTools
{
	// Token: 0x020008D8 RID: 2264
	internal class DevToolsOverlay : Element
	{
		// Token: 0x060041DD RID: 16861 RVA: 0x000C48E4 File Offset: 0x000C2AE4
		public DevToolsOverlay(Interface @interface, Desktop desktop) : base(desktop, null)
		{
			this._interface = @interface;
			this._popup = new PopupMenuLayer(this.Desktop, null);
			this._infoEntries = new DevToolsOverlay.InfoEntry[Enum.GetValues(typeof(DevToolsOverlay.GameInfoKey)).Length];
		}

		// Token: 0x060041DE RID: 16862 RVA: 0x000C4948 File Offset: 0x000C2B48
		public void Build()
		{
			base.Clear();
			Document document;
			this.Desktop.Provider.TryGetDocument("DevTools/DevToolsOverlay.ui", out document);
			this._popup.Style = document.ResolveNamedValue<PopupMenuLayerStyle>(this.Desktop.Provider, "PopupMenuLayerStyle");
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._consoleLog = uifragment.Get<Group>("ConsoleLog");
			this._consoleLog.Visible = true;
			this._gameInfo = uifragment.Get<Group>("GameInfo");
			this._tabNavigation = uifragment.Get<TabNavigation>("Tabs");
			this._tabNavigation.SelectedTab = this._activeTab.ToString();
			uifragment.Get<TextButton>("MenuButton").Activating = delegate()
			{
				this._popup.SetItems(new PopupMenuItem[]
				{
					new PopupMenuItem(this.Desktop.Provider.GetText("ui.devTools.popupMenu.copyGameInfo", null, true), new Action(this.CopyAllValues), null, null),
					new PopupMenuItem(this.Desktop.Provider.GetText("ui.devTools.popupMenu.createDefect", null, true), new Action(this.CreateDefect), null, null)
				});
				this._popup.Open();
			};
			string[] names = Enum.GetNames(typeof(DevToolsOverlay.Tabs));
			TabNavigation.Tab[] array = new TabNavigation.Tab[names.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new TabNavigation.Tab
				{
					Id = names[i],
					Text = this.Desktop.Provider.GetText("ui.devTools.tabs." + names[i], null, true)
				};
			}
			this._tabNavigation.Tabs = array;
			this._tabNavigation.SelectedTabChanged = delegate()
			{
				this._activeTab = (DevToolsOverlay.Tabs)Enum.Parse(typeof(DevToolsOverlay.Tabs), this._tabNavigation.SelectedTab);
				DevToolsOverlay.Tabs activeTab = this._activeTab;
				DevToolsOverlay.Tabs tabs = activeTab;
				if (tabs != DevToolsOverlay.Tabs.Console)
				{
					if (tabs == DevToolsOverlay.Tabs.GameInfo)
					{
						this._gameInfo.Visible = true;
						this._consoleLog.Visible = false;
					}
				}
				else
				{
					this._gameInfo.Visible = false;
					this._consoleLog.Visible = true;
				}
				base.Layout(null, true);
				this.ScrollDownLog();
			};
			for (int j = 0; j < this._consoleMessages.Count; j++)
			{
				DevToolsOverlay.Message message = this._consoleMessages[j];
				message.Element = this.BuildConsoleMessage(message);
				this._consoleMessages[j] = message;
			}
			this.BuildInfoEntries();
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.InitializeStaticValues();
				this.UpdateValues();
			}
		}

		// Token: 0x060041DF RID: 16863 RVA: 0x000C4B1F File Offset: 0x000C2D1F
		protected override void OnMounted()
		{
			this.InitializeStaticValues();
			this.UpdateValues();
			this.ScrollDownLog();
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x060041E0 RID: 16864 RVA: 0x000C4B4F File Offset: 0x000C2D4F
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x060041E1 RID: 16865 RVA: 0x000C4B6C File Offset: 0x000C2D6C
		private void Animate(float deltaTime)
		{
			bool flag = !this._gameInfo.IsMounted;
			if (!flag)
			{
				this._deltaTime += deltaTime;
				bool flag2 = this._deltaTime < 1f;
				if (!flag2)
				{
					this._deltaTime = 0f;
					this.UpdateValues();
					base.Layout(null, true);
				}
			}
		}

		// Token: 0x060041E2 RID: 16866 RVA: 0x000C4BD4 File Offset: 0x000C2DD4
		private void BuildSectionHeader(string key)
		{
			UIFragment uifragment = this._infoHeaderDoc.Instantiate(this.Desktop, this._gameInfo);
			uifragment.Get<Label>("Label").Text = this.Desktop.Provider.GetText("ui.devTools.gameInfo.sections." + key, null, true);
		}

		// Token: 0x060041E3 RID: 16867 RVA: 0x000C4C28 File Offset: 0x000C2E28
		private void InitializeStaticValues()
		{
			this.SetValue(DevToolsOverlay.GameInfoKey.Branch, BuildInfo.BranchName);
			this.SetValue(DevToolsOverlay.GameInfoKey.Revision, BuildInfo.RevisionId);
			this.SetValue(DevToolsOverlay.GameInfoKey.FrameworkVersion, Environment.Version.ToString());
			GraphicsDevice graphics = this.Desktop.Graphics;
			string value = graphics.GPUInfo.Vendor.ToString();
			string renderer = graphics.GPUInfo.Renderer;
			string version = graphics.GPUInfo.Version;
			this.SetValue(DevToolsOverlay.GameInfoKey.GPUVendor, value);
			this.SetValue(DevToolsOverlay.GameInfoKey.GPURenderer, renderer);
			this.SetValue(DevToolsOverlay.GameInfoKey.GPUVersion, version);
		}

		// Token: 0x060041E4 RID: 16868 RVA: 0x000C4CBC File Offset: 0x000C2EBC
		private void BuildEntry(DevToolsOverlay.GameInfoKey key, string assetEditorType = null)
		{
			UIFragment uifragment = this._infoEntryDoc.Instantiate(this.Desktop, this._gameInfo);
			Label label = uifragment.Get<Label>("Name");
			Label label2 = uifragment.Get<Label>("Value");
			Button button = uifragment.Get<Button>("Button");
			Action <>9__2;
			Action <>9__1;
			button.RightClicking = delegate()
			{
				List<PopupMenuItem> list = new List<PopupMenuItem>();
				string text = this.Desktop.Provider.GetText("ui.devTools.gameStats.popupMenu.copyValue", null, true);
				Action onActivate;
				if ((onActivate = <>9__2) == null)
				{
					onActivate = (<>9__2 = delegate()
					{
						SDL.SDL_SetClipboardText(this._infoEntries[(int)key].Value);
					});
				}
				list.Add(new PopupMenuItem(text, onActivate, null, null));
				List<PopupMenuItem> list2 = list;
				bool flag = assetEditorType != null;
				if (flag)
				{
					List<PopupMenuItem> list3 = list2;
					string text2 = this.Desktop.Provider.GetText("ui.devTools.gameStats.popupMenu.editInAssetEditor", null, true);
					Action onActivate2;
					if ((onActivate2 = <>9__1) == null)
					{
						onActivate2 = (<>9__1 = delegate()
						{
							string value = this._infoEntries[(int)key].Value;
							bool flag2 = value == "";
							if (!flag2)
							{
								this._interface.App.DevTools.Close();
								this._interface.App.InGame.OpenAssetIdInAssetEditor(assetEditorType, value);
							}
						});
					}
					list3.Add(new PopupMenuItem(text2, onActivate2, null, null));
				}
				this._popup.SetItems(list2);
				this._popup.Open();
			};
			label.Text = this.Desktop.Provider.GetText("ui.devTools.gameInfo.keys." + key.ToString(), null, true);
			this._infoEntries[(int)key].Label = label2;
		}

		// Token: 0x060041E5 RID: 16869 RVA: 0x000C4D80 File Offset: 0x000C2F80
		private void BuildInfoEntries()
		{
			this._gameInfo.Clear();
			for (int i = this._infoEntries.Length; i < this._infoEntries.Length; i++)
			{
				this._infoEntries[i] = default(DevToolsOverlay.InfoEntry);
			}
			this.Desktop.Provider.TryGetDocument("DevTools/InfoHeader.ui", out this._infoHeaderDoc);
			this.Desktop.Provider.TryGetDocument("DevTools/InfoEntry.ui", out this._infoEntryDoc);
			this.BuildSectionHeader("Build");
			this.BuildEntry(DevToolsOverlay.GameInfoKey.Branch, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.Revision, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.FrameworkVersion, null);
			this.BuildSectionHeader("Hardware");
			this.BuildEntry(DevToolsOverlay.GameInfoKey.GPUVendor, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.GPURenderer, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.GPUVersion, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.WindowSize, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.ViewportSize, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.SceneSize, null);
			this.BuildSectionHeader("World");
			this.BuildEntry(DevToolsOverlay.GameInfoKey.Biome, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.Zone, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.Environment, "Environment");
			this.BuildEntry(DevToolsOverlay.GameInfoKey.Weather, "Weather");
			this.BuildEntry(DevToolsOverlay.GameInfoKey.Entities, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.ViewDistance, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.Heightmap, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.Tint, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.Light, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.AudioEvents, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.ImmersiveViews, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.TargetBlock, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.HitBox, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.Orientation, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.FeetPosition, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.ChunkPosition, null);
			this.BuildSectionHeader("Chunks");
			this.BuildEntry(DevToolsOverlay.GameInfoKey.ChunksLoaded, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.ChunksDrawable, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.ChunksMax, null);
			this.BuildSectionHeader("AtlasSizes");
			this.BuildEntry(DevToolsOverlay.GameInfoKey.MapAtlasSize, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.EntityAtlasSize, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.IconAtlasSize, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.UIAtlasSize, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.CustomUIAtlasSize, null);
			this.BuildSectionHeader("Assets");
			this.BuildEntry(DevToolsOverlay.GameInfoKey.ActivelyReferencedAssets, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.BuiltInAssets, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.CachedAssets, null);
			this.BuildSectionHeader("UI");
			this.BuildEntry(DevToolsOverlay.GameInfoKey.UIScale, null);
			this.BuildSectionHeader("Particles");
			this.BuildEntry(DevToolsOverlay.GameInfoKey.ParticleSystems, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.ParticleProxies, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.ParticleBlend, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.ParticleDistortion, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.ParticleErosion, null);
			this.BuildSectionHeader("Network");
			this.BuildEntry(DevToolsOverlay.GameInfoKey.NetworkSent, null);
			this.BuildEntry(DevToolsOverlay.GameInfoKey.NetworkReceived, null);
		}

		// Token: 0x060041E6 RID: 16870 RVA: 0x000C5028 File Offset: 0x000C3228
		private void UpdateValues()
		{
			Point size = this._interface.Engine.Window.GetSize();
			this.SetValue(DevToolsOverlay.GameInfoKey.WindowSize, string.Format("{0}x{1}", size.X, size.Y));
			this.SetValue(DevToolsOverlay.GameInfoKey.ViewportSize, string.Format("{0}x{1}", this._interface.Engine.Window.Viewport.Width, this._interface.Engine.Window.Viewport.Height));
			this.SetValue(DevToolsOverlay.GameInfoKey.ActivelyReferencedAssets, this.Desktop.Provider.FormatNumber(AssetManager.ActivelyReferencedAssetsCount));
			this.SetValue(DevToolsOverlay.GameInfoKey.BuiltInAssets, this.Desktop.Provider.FormatNumber(AssetManager.BuiltInAssetsCount));
			this.SetValue(DevToolsOverlay.GameInfoKey.CachedAssets, this.Desktop.Provider.FormatNumber(AssetManager.CachedAssetsCount));
			this.SetValue(DevToolsOverlay.GameInfoKey.UIAtlasSize, string.Format("{0}x{1}", this._interface.TextureAtlasSize.X, this._interface.TextureAtlasSize.X));
			this.SetValue(DevToolsOverlay.GameInfoKey.CustomUIAtlasSize, string.Format("{0}x{1}", this._interface.InGameCustomUIProvider.TextureAtlasSize.X, this._interface.InGameCustomUIProvider.TextureAtlasSize.X));
			this.SetValue(DevToolsOverlay.GameInfoKey.UIScale, this.Desktop.Provider.FormatNumber(this.Desktop.Scale));
			bool flag = this._interface.App.Stage == App.AppStage.InGame;
			if (flag)
			{
				this.UpdateInGameValues();
			}
		}

		// Token: 0x060041E7 RID: 16871 RVA: 0x000C51E8 File Offset: 0x000C33E8
		private void UpdateInGameValues()
		{
			GameInstance instance = this._interface.InGameView.InGame.Instance;
			ProfilingModule profilingModule = instance.ProfilingModule;
			this.SetValue(DevToolsOverlay.GameInfoKey.MapAtlasSize, string.Format("{0}x{1}", instance.MapModule.TextureAtlas.Width, instance.MapModule.TextureAtlas.Height));
			this.SetValue(DevToolsOverlay.GameInfoKey.EntityAtlasSize, string.Format("{0}x{1}", instance.EntityStoreModule.TextureAtlas.Width, instance.EntityStoreModule.TextureAtlas.Height));
			bool flag = this._interface.InGameView.ItemIconAtlasTexture != null;
			if (flag)
			{
				this.SetValue(DevToolsOverlay.GameInfoKey.IconAtlasSize, string.Format("{0}x{1}", this._interface.InGameView.ItemIconAtlasTexture.Width, this._interface.InGameView.ItemIconAtlasTexture.Height));
			}
			else
			{
				this.SetValue(DevToolsOverlay.GameInfoKey.IconAtlasSize, "");
			}
			this.SetValue(DevToolsOverlay.GameInfoKey.ImmersiveViews, this.Desktop.Provider.FormatNumber(instance.ImmersiveScreenModule.GetScreenCount()));
			Vector2 viewportSize = instance.SceneRenderer.Data.ViewportSize;
			this.SetValue(DevToolsOverlay.GameInfoKey.SceneSize, string.Format("{0}x{1}", viewportSize.X, viewportSize.Y));
			this.SetValue(DevToolsOverlay.GameInfoKey.Entities, this.Desktop.Provider.FormatNumber(instance.EntityStoreModule.GetEntitiesCount()));
			this.SetValue(DevToolsOverlay.GameInfoKey.Environment, instance.WeatherModule.CurrentEnvironment.Id);
			this.SetValue(DevToolsOverlay.GameInfoKey.Weather, instance.WeatherModule.CurrentWeather.Id);
			this.SetValue(DevToolsOverlay.GameInfoKey.ViewDistance, string.Format("{0} (Effective: {1:##.0})", instance.App.Settings.ViewDistance, instance.MapModule.EffectiveViewDistance));
			Vector3 position = instance.LocalPlayer.Position;
			WorldMapModule.ClientBiomeData clientBiomeData;
			bool flag2 = instance.WorldMapModule.TryGetBiomeAtPosition(position, out clientBiomeData);
			if (flag2)
			{
				this.SetValue(DevToolsOverlay.GameInfoKey.Biome, clientBiomeData.BiomeName);
				this.SetValue(DevToolsOverlay.GameInfoKey.Zone, clientBiomeData.ZoneName);
			}
			else
			{
				this.SetValue(DevToolsOverlay.GameInfoKey.Biome, "");
				this.SetValue(DevToolsOverlay.GameInfoKey.Zone, "");
			}
			this.SetValue(DevToolsOverlay.GameInfoKey.ParticleSystems, this.Desktop.Provider.FormatNumber(instance.Engine.FXSystem.Particles.ParticleSystemCount));
			this.SetValue(DevToolsOverlay.GameInfoKey.ParticleProxies, this.Desktop.Provider.FormatNumber(instance.Engine.FXSystem.Particles.ParticleSystemProxyCount));
			this.SetValue(DevToolsOverlay.GameInfoKey.ParticleBlend, this.Desktop.Provider.FormatNumber(instance.Engine.FXSystem.Particles.PreviousFrameBlendDrawCount));
			this.SetValue(DevToolsOverlay.GameInfoKey.ParticleDistortion, this.Desktop.Provider.FormatNumber(instance.Engine.FXSystem.Particles.PreviousFrameDistortionDrawCount));
			this.SetValue(DevToolsOverlay.GameInfoKey.ParticleErosion, this.Desktop.Provider.FormatNumber(instance.Engine.FXSystem.Particles.PreviousFrameErosionDrawCount));
			this.SetValue(DevToolsOverlay.GameInfoKey.NetworkSent, string.Format("{0:0.000} KB/s ({1:0.000} KB)", profilingModule.LastAccumulatedSentPacketLength / 1000f, (float)profilingModule.TotalSentPacketLength / 1000f));
			this.SetValue(DevToolsOverlay.GameInfoKey.NetworkReceived, string.Format("{0:0.000} KB/s ({1:0.000} KB)", profilingModule.LastAccumulatedReceivedPacketLength / 1000f, (float)profilingModule.TotalReceivedPacketLength / 1000f));
			bool hasFoundTargetBlock = instance.InteractionModule.HasFoundTargetBlock;
			if (hasFoundTargetBlock)
			{
				HitDetection.RaycastHit targetBlockHit = instance.InteractionModule.TargetBlockHit;
				ClientBlockType clientBlockType = instance.MapModule.ClientBlockTypes[targetBlockHit.BlockId];
				this.SetValue(DevToolsOverlay.GameInfoKey.TargetBlock, string.Format("{0} ({1}, {2}, {3})", new object[]
				{
					clientBlockType.Name,
					targetBlockHit.BlockPosition.X,
					targetBlockHit.BlockPosition.Y,
					targetBlockHit.BlockPosition.Z
				}));
				this.SetValue(DevToolsOverlay.GameInfoKey.HitBox, clientBlockType.HitboxType.ToString());
			}
			else
			{
				this.SetValue(DevToolsOverlay.GameInfoKey.TargetBlock, "");
				this.SetValue(DevToolsOverlay.GameInfoKey.HitBox, "");
			}
			double num = Math.Round((double)position.X, 3);
			double num2 = Math.Round((double)position.Y, 3);
			double num3 = Math.Round((double)position.Z, 3);
			this.SetValue(DevToolsOverlay.GameInfoKey.FeetPosition, string.Format("{0:##.000}, {1:##.000}, {2:##.000}", num, num2, num3));
			double num4 = Math.Round((double)(instance.LocalPlayer.LookOrientation.X * 180f) / 3.141592653589793, 4);
			double num5 = Math.Round((double)(instance.LocalPlayer.LookOrientation.Y * 180f) / 3.141592653589793, 4);
			double num6 = Math.Round((double)(instance.LocalPlayer.LookOrientation.Z * 180f) / 3.141592653589793, 4);
			this.SetValue(DevToolsOverlay.GameInfoKey.Orientation, string.Format("{0:##.0000}, {1:##.0000}, {2:##.0000}", num4, num5, num6));
			int num7 = (int)Math.Floor((double)position.X);
			int num8 = (int)Math.Floor((double)position.Y);
			int num9 = (int)Math.Floor((double)position.Z);
			int num10 = num7 >> 5;
			int num11 = num8 >> 5;
			int num12 = num9 >> 5;
			int num13 = num7 - num10 * 32;
			int num14 = num8 - num11 * 32;
			int num15 = num9 - num12 * 32;
			this.SetValue(DevToolsOverlay.GameInfoKey.ChunkPosition, string.Format("({0}, {1}, {2}) in ({3}, {4}, {5})", new object[]
			{
				num13,
				num14,
				num15,
				num10,
				num11,
				num12
			}));
			ChunkColumn chunkColumn = instance.MapModule.GetChunkColumn(num10, num12);
			bool flag3 = chunkColumn != null;
			if (flag3)
			{
				int num16 = (num15 << 5) + num13;
				uint num17 = chunkColumn.Tints[num16];
				this.SetValue(DevToolsOverlay.GameInfoKey.Heightmap, chunkColumn.Heights[num16].ToString());
				this.SetValue(DevToolsOverlay.GameInfoKey.Tint, string.Format("#{0:X2}{1:X2}{2:X2}", (byte)(num17 >> 16), (byte)(num17 >> 8), (byte)num17));
				Chunk chunk = chunkColumn.GetChunk(num11);
				bool flag4 = chunk != null;
				if (flag4)
				{
					string str = "-";
					string str2 = "-";
					int num18 = ChunkHelper.IndexOfWorldBlockInChunk(num7, num8, num9);
					bool flag5 = chunk.Data.SelfLightAmounts != null;
					if (flag5)
					{
						ushort num19 = chunk.Data.SelfLightAmounts[num18];
						int num20 = (int)(num19 & 15);
						int num21 = num19 >> 4 & 15;
						int num22 = num19 >> 8 & 15;
						int num23 = num19 >> 12 & 15;
						str = string.Format("R: {0}, G: {1}, B: {2}, S: {3}", new object[]
						{
							num20,
							num21,
							num22,
							num23
						});
					}
					bool flag6 = chunk.Data.BorderedLightAmounts != null;
					if (flag6)
					{
						int num24 = ChunkHelper.IndexOfBlockInBorderedChunk(num18, 0, 0, 0);
						ushort num25 = chunk.Data.BorderedLightAmounts[num24];
						int num26 = (int)(num25 & 15);
						int num27 = num25 >> 4 & 15;
						int num28 = num25 >> 8 & 15;
						int num29 = num25 >> 12 & 15;
						str2 = string.Format("R: {0}, G: {1}, B: {2}, S: {3}", new object[]
						{
							num26,
							num27,
							num28,
							num29
						});
					}
					this.SetValue(DevToolsOverlay.GameInfoKey.Light, "Local: " + str + ", Global: " + str2);
				}
				else
				{
					this.SetValue(DevToolsOverlay.GameInfoKey.Light, "");
				}
			}
			else
			{
				this.SetValue(DevToolsOverlay.GameInfoKey.Light, "");
				this.SetValue(DevToolsOverlay.GameInfoKey.Heightmap, "");
				this.SetValue(DevToolsOverlay.GameInfoKey.Tint, "");
			}
			this.SetValue(DevToolsOverlay.GameInfoKey.ChunksLoaded, this.Desktop.Provider.FormatNumber((int)instance.MapModule.LoadedChunksCount));
			this.SetValue(DevToolsOverlay.GameInfoKey.ChunksDrawable, this.Desktop.Provider.FormatNumber((int)instance.MapModule.DrawableChunksCount));
			this.SetValue(DevToolsOverlay.GameInfoKey.ChunksMax, this.Desktop.Provider.FormatNumber(instance.MapModule.ChunkColumnCount()));
		}

		// Token: 0x060041E8 RID: 16872 RVA: 0x000C5AAC File Offset: 0x000C3CAC
		private void SetValue(DevToolsOverlay.GameInfoKey key, string value)
		{
			ref DevToolsOverlay.InfoEntry ptr = ref this._infoEntries[(int)key];
			ptr.Value = value;
			ptr.Label.Text = value;
		}

		// Token: 0x060041E9 RID: 16873 RVA: 0x000C5ADC File Offset: 0x000C3CDC
		private void CopyAllValues()
		{
			this.UpdateValues();
			base.Layout(null, true);
			string text = "";
			foreach (Element element in this._gameInfo.Children)
			{
				Label label = element as Label;
				bool flag = label != null;
				if (flag)
				{
					bool flag2 = text != "";
					if (flag2)
					{
						text += "\n";
					}
					text = text + label.TextSpans[0].Text + "\n";
				}
				Button button = element as Button;
				bool flag3 = button != null;
				if (flag3)
				{
					string[] array = new string[6];
					array[0] = text;
					array[1] = " - ";
					int num = 2;
					Label.LabelSpan labelSpan = Enumerable.FirstOrDefault<Label.LabelSpan>(button.Find<Label>("Name").TextSpans);
					array[num] = ((labelSpan != null) ? labelSpan.Text : null);
					array[3] = ": ";
					int num2 = 4;
					Label.LabelSpan labelSpan2 = Enumerable.FirstOrDefault<Label.LabelSpan>(button.Find<Label>("Value").TextSpans);
					array[num2] = ((labelSpan2 != null) ? labelSpan2.Text : null);
					array[5] = "\n";
					text = string.Concat(array);
				}
			}
			SDL.SDL_SetClipboardText(text);
		}

		// Token: 0x060041EA RID: 16874 RVA: 0x000C5C30 File Offset: 0x000C3E30
		private void CreateDefect()
		{
			OpenUtils.OpenTrustedUrlInDefaultBrowser("https://h-qa.atlassian.net/secure/CreateIssue.jspa?issuetype=10005&pid=10000");
		}

		// Token: 0x060041EB RID: 16875 RVA: 0x000C5C40 File Offset: 0x000C3E40
		private void ScrollDownLog()
		{
			Element consoleLog = this._consoleLog;
			int? y = new int?(this._consoleLog.ScaledScrollSize.Y);
			consoleLog.SetScroll(null, y);
		}

		// Token: 0x060041EC RID: 16876 RVA: 0x000C5C7C File Offset: 0x000C3E7C
		public void AddConsoleMessage(DevToolsOverlay.MessageType type, string text)
		{
			DevToolsOverlay.Message message = Enumerable.LastOrDefault<DevToolsOverlay.Message>(this._consoleMessages);
			bool flag = message.Text != null && message.Text == text && message.MessageType == type;
			if (flag)
			{
				message.Count++;
				bool flag2 = !this._interface.HasMarkupError && this._interface.HasLoaded;
				if (flag2)
				{
					Label label = message.Element.Find<Label>("DuplicateCount");
					label.Visible = true;
					label.Text = this.Desktop.Provider.FormatNumber(message.Count);
				}
				this._consoleMessages[this._consoleMessages.Count - 1] = message;
			}
			else
			{
				DevToolsOverlay.Message message2 = new DevToolsOverlay.Message
				{
					MessageType = type,
					Text = text,
					Count = 1
				};
				bool flag3 = !this._interface.HasMarkupError && this._interface.HasLoaded;
				if (flag3)
				{
					message2.Element = this.BuildConsoleMessage(message2);
				}
				this._consoleMessages.Add(message2);
			}
		}

		// Token: 0x060041ED RID: 16877 RVA: 0x000C5DA4 File Offset: 0x000C3FA4
		public void LayoutLog()
		{
			bool flag = !this._consoleLog.IsMounted;
			if (!flag)
			{
				this._consoleLog.Layout(null, true);
			}
		}

		// Token: 0x060041EE RID: 16878 RVA: 0x000C5DDC File Offset: 0x000C3FDC
		private Button BuildConsoleMessage(DevToolsOverlay.Message message)
		{
			Document document;
			this.Desktop.Provider.TryGetDocument("DevTools/ConsoleEntry.ui", out document);
			PatchStyle background = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "ErrorIcon");
			PatchStyle background2 = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "WarningIcon");
			UInt32Color textColor = document.ResolveNamedValue<UInt32Color>(this.Desktop.Provider, "ErrorLabelColor");
			UInt32Color textColor2 = document.ResolveNamedValue<UInt32Color>(this.Desktop.Provider, "WarningLabelColor");
			Button.ButtonStyle style = document.ResolveNamedValue<Button.ButtonStyle>(this.Desktop.Provider, "ErrorStyle");
			Button.ButtonStyle style2 = document.ResolveNamedValue<Button.ButtonStyle>(this.Desktop.Provider, "WarningStyle");
			UIFragment uifragment = document.Instantiate(this.Desktop, this._consoleLog);
			Button button = uifragment.Get<Button>("Button");
			Group group = uifragment.Get<Group>("Icon");
			Label label = uifragment.Get<Label>("Message");
			Label label2 = uifragment.Get<Label>("DuplicateCount");
			Action <>9__1;
			button.RightClicking = delegate()
			{
				PopupMenuLayer popup = this._popup;
				PopupMenuItem[] array = new PopupMenuItem[1];
				int num = 0;
				string text = this.Desktop.Provider.GetText("ui.devTools.logEntry.popupMenu.copyMessage", null, true);
				Action onActivate;
				if ((onActivate = <>9__1) == null)
				{
					onActivate = (<>9__1 = delegate()
					{
						SDL.SDL_SetClipboardText(message.Text);
					});
				}
				array[num] = new PopupMenuItem(text, onActivate, null, null);
				popup.SetItems(array);
				this._popup.Open();
			};
			label.Text = message.Text;
			bool flag = message.Count > 1;
			if (flag)
			{
				label2.Visible = true;
				label2.Text = this.Desktop.Provider.FormatNumber(message.Count);
			}
			DevToolsOverlay.MessageType messageType = message.MessageType;
			DevToolsOverlay.MessageType messageType2 = messageType;
			if (messageType2 != DevToolsOverlay.MessageType.Warning)
			{
				if (messageType2 == DevToolsOverlay.MessageType.Error)
				{
					group.Visible = true;
					group.Background = background;
					label.Style.TextColor = textColor;
					button.Style = style;
				}
			}
			else
			{
				group.Visible = true;
				group.Background = background2;
				label.Style.TextColor = textColor2;
				button.Style = style2;
			}
			message.Element = button;
			return button;
		}

		// Token: 0x060041EF RID: 16879 RVA: 0x000C5FE4 File Offset: 0x000C41E4
		public void ResetGameInfoState()
		{
			bool hasMarkupError = this._interface.HasMarkupError;
			if (!hasMarkupError)
			{
				this.BuildInfoEntries();
				bool isMounted = base.IsMounted;
				if (isMounted)
				{
					base.Layout(null, true);
				}
			}
		}

		// Token: 0x060041F0 RID: 16880 RVA: 0x000C6025 File Offset: 0x000C4225
		protected internal override void Dismiss()
		{
			this._interface.App.DevTools.Close();
		}

		// Token: 0x0400202B RID: 8235
		private readonly Interface _interface;

		// Token: 0x0400202C RID: 8236
		private readonly PopupMenuLayer _popup;

		// Token: 0x0400202D RID: 8237
		private Document _infoHeaderDoc;

		// Token: 0x0400202E RID: 8238
		private Document _infoEntryDoc;

		// Token: 0x0400202F RID: 8239
		private TabNavigation _tabNavigation;

		// Token: 0x04002030 RID: 8240
		private Group _consoleLog;

		// Token: 0x04002031 RID: 8241
		private Group _gameInfo;

		// Token: 0x04002032 RID: 8242
		private readonly List<DevToolsOverlay.Message> _consoleMessages = new List<DevToolsOverlay.Message>();

		// Token: 0x04002033 RID: 8243
		private readonly DevToolsOverlay.InfoEntry[] _infoEntries;

		// Token: 0x04002034 RID: 8244
		private DevToolsOverlay.Tabs _activeTab = DevToolsOverlay.Tabs.Console;

		// Token: 0x04002035 RID: 8245
		private float _deltaTime;

		// Token: 0x02000D87 RID: 3463
		private enum GameInfoKey
		{
			// Token: 0x0400424F RID: 16975
			Branch,
			// Token: 0x04004250 RID: 16976
			Revision,
			// Token: 0x04004251 RID: 16977
			FrameworkVersion,
			// Token: 0x04004252 RID: 16978
			GPUVendor,
			// Token: 0x04004253 RID: 16979
			GPURenderer,
			// Token: 0x04004254 RID: 16980
			GPUVersion,
			// Token: 0x04004255 RID: 16981
			WindowSize,
			// Token: 0x04004256 RID: 16982
			ViewportSize,
			// Token: 0x04004257 RID: 16983
			SceneSize,
			// Token: 0x04004258 RID: 16984
			Biome,
			// Token: 0x04004259 RID: 16985
			Zone,
			// Token: 0x0400425A RID: 16986
			Environment,
			// Token: 0x0400425B RID: 16987
			Weather,
			// Token: 0x0400425C RID: 16988
			Entities,
			// Token: 0x0400425D RID: 16989
			ViewDistance,
			// Token: 0x0400425E RID: 16990
			Heightmap,
			// Token: 0x0400425F RID: 16991
			Tint,
			// Token: 0x04004260 RID: 16992
			Light,
			// Token: 0x04004261 RID: 16993
			AudioEvents,
			// Token: 0x04004262 RID: 16994
			ImmersiveViews,
			// Token: 0x04004263 RID: 16995
			TargetBlock,
			// Token: 0x04004264 RID: 16996
			HitBox,
			// Token: 0x04004265 RID: 16997
			Orientation,
			// Token: 0x04004266 RID: 16998
			FeetPosition,
			// Token: 0x04004267 RID: 16999
			ChunkPosition,
			// Token: 0x04004268 RID: 17000
			ChunksLoaded,
			// Token: 0x04004269 RID: 17001
			ChunksDrawable,
			// Token: 0x0400426A RID: 17002
			ChunksMax,
			// Token: 0x0400426B RID: 17003
			MapAtlasSize,
			// Token: 0x0400426C RID: 17004
			EntityAtlasSize,
			// Token: 0x0400426D RID: 17005
			IconAtlasSize,
			// Token: 0x0400426E RID: 17006
			UIAtlasSize,
			// Token: 0x0400426F RID: 17007
			CustomUIAtlasSize,
			// Token: 0x04004270 RID: 17008
			ActivelyReferencedAssets,
			// Token: 0x04004271 RID: 17009
			BuiltInAssets,
			// Token: 0x04004272 RID: 17010
			CachedAssets,
			// Token: 0x04004273 RID: 17011
			UIScale,
			// Token: 0x04004274 RID: 17012
			ParticleSystems,
			// Token: 0x04004275 RID: 17013
			ParticleProxies,
			// Token: 0x04004276 RID: 17014
			ParticleBlend,
			// Token: 0x04004277 RID: 17015
			ParticleErosion,
			// Token: 0x04004278 RID: 17016
			ParticleDistortion,
			// Token: 0x04004279 RID: 17017
			NetworkSent,
			// Token: 0x0400427A RID: 17018
			NetworkReceived
		}

		// Token: 0x02000D88 RID: 3464
		private enum Tabs
		{
			// Token: 0x0400427C RID: 17020
			Console,
			// Token: 0x0400427D RID: 17021
			GameInfo
		}

		// Token: 0x02000D89 RID: 3465
		public enum MessageType
		{
			// Token: 0x0400427F RID: 17023
			Info,
			// Token: 0x04004280 RID: 17024
			Warning,
			// Token: 0x04004281 RID: 17025
			Error
		}

		// Token: 0x02000D8A RID: 3466
		private struct Message
		{
			// Token: 0x04004282 RID: 17026
			public string Text;

			// Token: 0x04004283 RID: 17027
			public DevToolsOverlay.MessageType MessageType;

			// Token: 0x04004284 RID: 17028
			public int Count;

			// Token: 0x04004285 RID: 17029
			public Button Element;
		}

		// Token: 0x02000D8B RID: 3467
		private struct InfoEntry
		{
			// Token: 0x04004286 RID: 17030
			public string Value;

			// Token: 0x04004287 RID: 17031
			public Label Label;
		}
	}
}
