using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using HytaleClient.Core;
using HytaleClient.Data.Audio;
using HytaleClient.Data.Characters;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.Application
{
	// Token: 0x02000BE9 RID: 3049
	internal class AppStartup
	{
		// Token: 0x0600613C RID: 24892 RVA: 0x001FF72C File Offset: 0x001FD92C
		public AppStartup(App app)
		{
			this._app = app;
		}

		// Token: 0x0600613D RID: 24893 RVA: 0x001FF748 File Offset: 0x001FD948
		public void StartFromMainMenu()
		{
			this.Load(delegate
			{
				this._app.MainMenu.Open(AppMainMenu.MainMenuPage.Home);
				this._app.Interface.OnAppStageChanged();
			});
		}

		// Token: 0x0600613E RID: 24894 RVA: 0x001FF760 File Offset: 0x001FD960
		public void StartWithLocalWorld(string worldName)
		{
			this.Load(delegate
			{
				foreach (AppMainMenu.World world in this._app.MainMenu.Worlds)
				{
					bool flag = world.Options.Name == worldName;
					if (flag)
					{
						this._app.GameLoading.Open(world.Path);
						this._app.Interface.FadeIn(null, false);
						return;
					}
				}
				this._app.MainMenu.Open(AppMainMenu.MainMenuPage.Adventure);
				this._app.Interface.MainMenuView.AdventurePage.OnFailedToJoinUnknownWorld();
				this._app.Interface.FadeIn(null, false);
			});
		}

		// Token: 0x0600613F RID: 24895 RVA: 0x001FF798 File Offset: 0x001FD998
		public void StartWithServerConnection(string address)
		{
			this.Load(delegate
			{
				this._app.MainMenu.Open(AppMainMenu.MainMenuPage.Servers);
				this._app.Interface.MainMenuView.ServersPage.HandleAutoConnectOnStartup(address);
			});
		}

		// Token: 0x06006140 RID: 24896 RVA: 0x001FF7CD File Offset: 0x001FD9CD
		internal void CleanUp()
		{
			this._startupLoadingCancelTokenSource.Cancel();
		}

		// Token: 0x06006141 RID: 24897 RVA: 0x001FF7DC File Offset: 0x001FD9DC
		private void Load(Action onLoaded)
		{
			Debug.Assert(this._app.Stage == App.AppStage.Initial);
			this._app.SetupInterface();
			this._app.SetStage(App.AppStage.Startup);
			ManualResetEventSlim fadeInDoneEvent = new ManualResetEventSlim(false);
			this._app.Interface.FadeIn(delegate
			{
				fadeInDoneEvent.Set();
			}, true);
			this._app.ResetElapsedTime();
			CancellationToken cancelToken = this._startupLoadingCancelTokenSource.Token;
			Action <>9__3;
			ThreadPool.QueueUserWorkItem(delegate(object _)
			{
				Dictionary<string, WwiseResource> upcomingWwiseIds = null;
				byte[][] upcomingHairGradientAtlasPixels = null;
				Task[] array = new Task[3];
				array[0] = Task.Run(delegate()
				{
					try
					{
						WwiseHeaderParser.Parse(Path.Combine(Paths.BuiltInAssets, "Common/SoundBanks/Wwise_IDs.h"), out upcomingWwiseIds);
					}
					catch (Exception exception)
					{
						AppStartup.Logger.Error(exception, "Failed to load wwise header file.");
					}
				});
				int num = 1;
				Action action;
				if ((action = <>9__3) == null)
				{
					action = (<>9__3 = delegate()
					{
						this._app.Fonts.LoadFonts(this._app.Engine.Graphics);
					});
				}
				array[num] = Task.Run(action);
				array[2] = Task.Run(delegate()
				{
					HashSet<string> updatedCommonAssets;
					AssetManager.Initialize(this._app.Engine, cancelToken, out updatedCommonAssets);
					AppStartup.Logger.Info("AssetManager initialized.");
					HashSet<string> updatedCosmeticsAssets = new HashSet<string>();
					bool flag = !AssetManager.IsAssetsDirectoryImmutable;
					if (flag)
					{
						updatedCosmeticsAssets = AssetManager.GetUpdatedAssets(Path.Combine(Paths.BuiltInAssets, "Cosmetics"), Path.Combine(Paths.BuiltInAssets, "CosmeticsAssetsIndex.cache"), cancelToken);
						AppStartup.Logger.Info("Finished getting list of updated cosmetic config files.");
					}
					bool textureAtlasNeedsUpdate = false;
					CharacterPartStore characterPartStore = this._app.CharacterPartStore;
					characterPartStore.LoadAssets(updatedCosmeticsAssets, ref textureAtlasNeedsUpdate, cancelToken);
					characterPartStore.PrepareGradientAtlas(out upcomingHairGradientAtlasPixels);
					characterPartStore.LoadModelData(this._app.Engine, updatedCommonAssets, textureAtlasNeedsUpdate);
					AppStartup.Logger.Info("CharacterPartStore initialized.");
					bool isCancellationRequested = cancelToken.IsCancellationRequested;
					if (!isCancellationRequested)
					{
						string text = Path.Combine(Paths.BuiltInAssets, "CommonAssetsIndex.cache");
						bool flag2 = File.Exists(text + ".tmp");
						if (flag2)
						{
							File.Delete(text);
							File.Move(text + ".tmp", text);
							File.Delete(text + ".tmp");
						}
					}
				});
				Task.WaitAll(array);
				AppStartup.Logger.Info("Background startup loading done.");
				fadeInDoneEvent.Wait();
				this._app.Engine.RunOnMainThread(this._app.Engine, delegate
				{
					this._app.Engine.Audio.ResourceManager.SetupWwiseIds(upcomingWwiseIds);
					this._app.CharacterPartStore.BuildGradientTexture(upcomingHairGradientAtlasPixels);
					this._app.Fonts.BuildFontTextures();
					this._app.Interface.SetLanguageAndLoad(this._app.Settings.Language);
					this._app.Interface.LoadAndBuild();
					onLoaded();
				}, false, false);
			});
		}

		// Token: 0x04003C65 RID: 15461
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003C66 RID: 15462
		private readonly App _app;

		// Token: 0x04003C67 RID: 15463
		private readonly CancellationTokenSource _startupLoadingCancelTokenSource = new CancellationTokenSource();
	}
}
