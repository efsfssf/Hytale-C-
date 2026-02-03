using System;
using System.Diagnostics;
using System.IO;
using HytaleClient.AssetEditor;
using HytaleClient.Audio.Commands;
using HytaleClient.Core;
using HytaleClient.Graphics;
using HytaleClient.Utils;
using NLog;
using Sentry;

namespace HytaleClient.Application
{
	// Token: 0x02000BEC RID: 3052
	internal class Program
	{
		// Token: 0x0600614F RID: 24911 RVA: 0x001FFBEC File Offset: 0x001FDDEC
		private static void Main(string[] args)
		{
			bool flag = !Debugger.IsAttached;
			if (flag)
			{
				Directory.SetCurrentDirectory(Paths.App);
			}
			Language.Initialize();
			BitUtils.UnitTest();
			MemoryPoolHelper.UnitTest();
			FXMemoryPool.UnitTest();
			ClusteredLighting.UnitTest();
			CommandMemoryPool.UnitTest();
			CommandMemoryPool.CommandBufferUnitTest();
			string text;
			CommandMemoryPool.StressTest(false, out text);
			Console.WriteLine(text);
			bool flag2 = !OptionsHelper.Setup(args);
			if (!flag2)
			{
				Paths.Setup();
				LogWriter.Start();
				bool flag3 = BuildInfo.Platform == Platform.Windows;
				if (flag3)
				{
					string str = OptionsHelper.LaunchEditor ? "Editor" : "Game";
					int num = WindowsUtils.SetApplicationUserModelId("HypixelStudios.Hytale." + str);
					bool flag4 = num != 0;
					if (flag4)
					{
						Program.Logger.Warn("Failed to set application user model id, result: {0}", num);
					}
				}
				using (new ApplicationMutex())
				{
					SentrySdk.ConfigureScope(delegate(Scope o)
					{
						o.SetTag("Build.Platform", BuildInfo.Platform.ToString());
						o.SetTag("Build.Architecture", BuildInfo.Architecture);
						o.SetTag("Build.Configuration", BuildInfo.Configuration);
						o.SetTag("Build.Version", BuildInfo.Version);
						o.SetTag("Build.RevisionId", BuildInfo.RevisionId);
						o.SetTag("Build.BranchName", BuildInfo.BranchName);
					});
					bool flag5 = !Debugger.IsAttached;
					if (flag5)
					{
						CrashHandler.Hook();
					}
					BuildInfo.PrintAll();
					GraphicsDevice.TryForceDedicatedNvGraphics();
					ThreadHelper.Initialize();
					AffinityHelper.Setup();
					Engine.Initialize();
					bool launchEditor = OptionsHelper.LaunchEditor;
					if (launchEditor)
					{
						Program.StartAssetEditor();
					}
					else
					{
						Program.StartGame();
					}
				}
			}
		}

		// Token: 0x06006150 RID: 24912 RVA: 0x001FFD5C File Offset: 0x001FDF5C
		private static void StartGame()
		{
			using (App app = new App())
			{
				string username = app.Username;
				SentrySdk.ConfigureScope(delegate(Scope o)
				{
					o.SetTag("Username", username);
				});
				bool flag = OptionsHelper.ServerAddress != null;
				if (flag)
				{
					app.Startup.StartWithServerConnection(OptionsHelper.ServerAddress);
				}
				else
				{
					bool flag2 = OptionsHelper.WorldName != null;
					if (flag2)
					{
						app.Startup.StartWithLocalWorld(OptionsHelper.WorldName);
					}
					else
					{
						app.Startup.StartFromMainMenu();
					}
				}
				app.RunLoop();
			}
		}

		// Token: 0x06006151 RID: 24913 RVA: 0x001FFE04 File Offset: 0x001FE004
		private static void StartAssetEditor()
		{
			using (AssetEditorApp assetEditorApp = new AssetEditorApp())
			{
				string username = assetEditorApp.AuthManager.Settings.Username;
				SentrySdk.ConfigureScope(delegate(Scope o)
				{
					o.SetTag("Username", username);
				});
				bool flag = OptionsHelper.ServerAddress != null;
				if (flag)
				{
					bool flag2 = OptionsHelper.OpenAssetPath != null;
					if (flag2)
					{
						assetEditorApp.Startup.StartFromAssetEditorWithPath(OptionsHelper.ServerAddress, OptionsHelper.OpenAssetPath);
					}
					else
					{
						bool flag3 = OptionsHelper.OpenAssetType != null && OptionsHelper.OpenAssetId != null;
						if (flag3)
						{
							assetEditorApp.Startup.StartFromAssetEditorWithId(OptionsHelper.ServerAddress, OptionsHelper.OpenAssetType, OptionsHelper.OpenAssetId);
						}
						else
						{
							assetEditorApp.Startup.StartFromAssetEditor(OptionsHelper.ServerAddress);
						}
					}
				}
				else
				{
					bool flag4 = OptionsHelper.OpenAssetPath != null;
					if (flag4)
					{
						assetEditorApp.Startup.StartFromCosmeticsEditorWithPath(OptionsHelper.OpenAssetPath);
					}
					else
					{
						bool flag5 = OptionsHelper.OpenAssetType != null && OptionsHelper.OpenAssetId != null;
						if (flag5)
						{
							assetEditorApp.Startup.StartFromCosmeticsEditorWithId(OptionsHelper.OpenAssetType, OptionsHelper.OpenAssetId);
						}
						else
						{
							bool openCosmetics = OptionsHelper.OpenCosmetics;
							if (openCosmetics)
							{
								assetEditorApp.Startup.StartFromCosmeticsEditor();
							}
							else
							{
								assetEditorApp.Startup.StartFromMainMenu();
							}
						}
					}
				}
				assetEditorApp.RunLoop();
			}
		}

		// Token: 0x04003C6E RID: 15470
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
	}
}
