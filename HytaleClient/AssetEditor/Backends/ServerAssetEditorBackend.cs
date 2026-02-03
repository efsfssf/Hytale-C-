using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Interface.Config;
using HytaleClient.AssetEditor.Interface.Editor;
using HytaleClient.AssetEditor.Networking;
using HytaleClient.AssetEditor.Utils;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Data.UserSettings;
using HytaleClient.Graphics;
using HytaleClient.Interface.Messages;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Networking;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using Newtonsoft.Json.Linq;
using NLog;

namespace HytaleClient.AssetEditor.Backends
{
	// Token: 0x02000BE3 RID: 3043
	internal class ServerAssetEditorBackend : AssetEditorBackend
	{
		// Token: 0x170013E6 RID: 5094
		// (get) Token: 0x06006022 RID: 24610 RVA: 0x001F5F12 File Offset: 0x001F4112
		// (set) Token: 0x06006023 RID: 24611 RVA: 0x001F5F1A File Offset: 0x001F411A
		public string LocalAssetsDirectoryPathForCurrentExport { get; private set; }

		// Token: 0x170013E7 RID: 5095
		// (get) Token: 0x06006024 RID: 24612 RVA: 0x001F5F23 File Offset: 0x001F4123
		public string Hostname
		{
			get
			{
				return this._connection.Hostname;
			}
		}

		// Token: 0x170013E8 RID: 5096
		// (get) Token: 0x06006025 RID: 24613 RVA: 0x001F5F30 File Offset: 0x001F4130
		public int Port
		{
			get
			{
				return this._connection.Port;
			}
		}

		// Token: 0x06006026 RID: 24614 RVA: 0x001F5F40 File Offset: 0x001F4140
		public ServerAssetEditorBackend(AssetEditorOverlay assetEditorOverlay, ConnectionToServer connection, AssetEditorPacketHandler packetHandler) : base(assetEditorOverlay)
		{
			this._connection = connection;
			this._connection.OnDisconnected = new Action<Exception>(this.OnDisconnected);
			this._packetHandler = packetHandler;
			this._backendLifetimeCancellationToken = this._backendLifetimeCancellationTokenSource.Token;
			base.SupportedAssetTreeFolders = new AssetTreeFolder[]
			{
				AssetTreeFolder.Server,
				AssetTreeFolder.Common
			};
			base.IsEditingRemotely = true;
		}

		// Token: 0x06006027 RID: 24615 RVA: 0x001F5FF4 File Offset: 0x001F41F4
		protected override void DoDispose()
		{
			this._exportCompleteCallback = null;
			this._backendLifetimeCancellationTokenSource.Cancel();
			foreach (Texture texture in this._iconTextures)
			{
				texture.Dispose();
			}
			this._packetHandler.Dispose();
			this._connection.OnDisconnected = null;
			this._connection.SendPacketImmediate(new Disconnect("User leave", 0));
			this._connection.Close();
		}

		// Token: 0x06006028 RID: 24616 RVA: 0x001F6098 File Offset: 0x001F4298
		public override void Initialize()
		{
			Debug.Assert(!this._isInitializingOrInitialized);
			bool isInitializingOrInitialized = this._isInitializingOrInitialized;
			if (!isInitializingOrInitialized)
			{
				this._isInitializingOrInitialized = true;
				this._isInitializing = true;
				this._localAssetsDirectoryPath = this.AssetEditorOverlay.Interface.App.Settings.AssetsPath;
			}
		}

		// Token: 0x06006029 RID: 24617 RVA: 0x001F60F0 File Offset: 0x001F42F0
		private void OnDisconnected(Exception exception)
		{
			bool isCancellationRequested = this._backendLifetimeCancellationTokenSource.IsCancellationRequested;
			if (!isCancellationRequested)
			{
				ServerAssetEditorBackend.Logger.Info("got disconnected from server!");
				bool flag = exception != null;
				if (flag)
				{
					ServerAssetEditorBackend.Logger.Error<Exception>(exception);
				}
				this.AssetEditorOverlay.Interface.App.MainMenu.OpenWithDisconnectPopup(this._connection.Hostname, this._connection.Port);
			}
		}

		// Token: 0x0600602A RID: 24618 RVA: 0x001F6164 File Offset: 0x001F4364
		public void OnLocalAssetsDirectoryPathChanged()
		{
			this._localAssetsDirectoryPath = this.AssetEditorOverlay.Interface.App.Settings.AssetsPath;
		}

		// Token: 0x0600602B RID: 24619 RVA: 0x001F6188 File Offset: 0x001F4388
		private void ProcessCallback<T>(Action<T, FormattedMessage> action, T value, FormattedMessage error)
		{
			bool isCancellationRequested = this._backendLifetimeCancellationTokenSource.IsCancellationRequested;
			if (!isCancellationRequested)
			{
				this.AssetEditorOverlay.Interface.Engine.RunOnMainThread(this.AssetEditorOverlay.Interface, delegate
				{
					action(value, error);
				}, true, false);
			}
		}

		// Token: 0x0600602C RID: 24620 RVA: 0x001F61F4 File Offset: 0x001F43F4
		private void ProcessErrorCallback<T>(Action<T, FormattedMessage> action, FormattedMessage error)
		{
			this.ProcessCallback<T>(action, default(T), error);
		}

		// Token: 0x0600602D RID: 24621 RVA: 0x001F6214 File Offset: 0x001F4414
		private void ProcessSuccessCallback<T>(Action<T, FormattedMessage> action, T value)
		{
			this.ProcessCallback<T>(action, value, null);
		}

		// Token: 0x0600602E RID: 24622 RVA: 0x001F6221 File Offset: 0x001F4421
		public void SetupSchemas(AssetEditorSetupSchemas.SchemaFile[] schemaFiles)
		{
			this.schemaFilesToProcess = schemaFiles;
		}

		// Token: 0x0600602F RID: 24623 RVA: 0x001F622C File Offset: 0x001F442C
		private bool TryParseVirtualAssetTypeFromSchema(JObject json, SchemaNode schema, IDictionary<string, string> translationMapping, out AssetTypeConfig assetTypeConfig)
		{
			assetTypeConfig = null;
			JObject jobject = (JObject)json["hytale"];
			bool flag = jobject == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !jobject.ContainsKey("virtualPath") && jobject.ContainsKey("uiEditorIgnore") && (bool)jobject["uiEditorIgnore"];
				if (flag2)
				{
					result = false;
				}
				else
				{
					string text = Path.Combine(Paths.BuiltInAssets, "Server");
					string text2 = (string)jobject["virtualPath"];
					bool flag3 = text2 == null || !Paths.IsSubPathOf(Path.Combine(text, text2), text);
					if (flag3)
					{
						result = false;
					}
					else
					{
						string fileExtension = ".json";
						bool flag4 = jobject.ContainsKey("extension");
						if (flag4)
						{
							fileExtension = (string)jobject["extension"];
						}
						string name;
						bool flag5 = !translationMapping.TryGetValue((string)json["title"] + ".title", out name);
						if (flag5)
						{
							name = (string)json["title"];
						}
						assetTypeConfig = new AssetTypeConfig
						{
							Schema = schema,
							Id = Path.GetFileNameWithoutExtension(schema.Id),
							Name = name,
							Path = AssetPathUtils.CombinePaths("Server", text2),
							AssetTree = AssetTreeFolder.Server,
							EditorType = 3,
							FileExtension = fileExtension,
							IsVirtual = true
						};
						base.ApplySchemaMetadata(assetTypeConfig, jobject);
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06006030 RID: 24624 RVA: 0x001F63B4 File Offset: 0x001F45B4
		public void SetupAssetTypes(AssetEditorSetupAssetTypes.AssetEditorAssetType[] networkAssetTypes)
		{
			Action <>9__3;
			Task.Run(delegate()
			{
				Dictionary<string, JObject> dictionary = new Dictionary<string, JObject>();
				Dictionary<string, SchemaNode> schemas = new Dictionary<string, SchemaNode>();
				Dictionary<string, AssetTypeConfig> assetTypes = new Dictionary<string, AssetTypeConfig>();
				IDictionary<string, string> dictionary2 = Language.LoadServerLanguageFile("assetTypes.lang", this.AssetEditorOverlay.Interface.App.Settings.Language);
				foreach (AssetEditorSetupSchemas.SchemaFile schemaFile in this.schemaFilesToProcess)
				{
					bool isCancellationRequested = this._backendLifetimeCancellationToken.IsCancellationRequested;
					if (isCancellationRequested)
					{
						return;
					}
					JObject jobject = (JObject)BsonHelper.FromBson(schemaFile.Content);
					SchemaNode schemaNode = this.LoadSchema(jobject, schemas);
					dictionary[schemaNode.Id] = jobject;
					AssetTypeConfig assetTypeConfig;
					bool flag = !this.TryParseVirtualAssetTypeFromSchema(jobject, schemaNode, dictionary2, out assetTypeConfig);
					if (!flag)
					{
						assetTypes.Add(assetTypeConfig.Id, assetTypeConfig);
					}
				}
				this.schemaFilesToProcess = null;
				foreach (AssetEditorSetupAssetTypes.AssetEditorAssetType assetEditorAssetType in networkAssetTypes)
				{
					bool flag2 = assetEditorAssetType.Path.StartsWith("Server/");
					AssetTreeFolder assetTree;
					if (flag2)
					{
						assetTree = AssetTreeFolder.Server;
					}
					else
					{
						bool flag3 = assetEditorAssetType.Path.StartsWith("Common/") || assetEditorAssetType.Path == "Common";
						if (!flag3)
						{
							throw new Exception("Path of asset type " + assetEditorAssetType.Path + " must be either in Server/ or Common/ directory: " + assetEditorAssetType.Path);
						}
						assetTree = AssetTreeFolder.Common;
					}
					string id;
					bool flag4 = !dictionary2.TryGetValue(assetEditorAssetType.Id + ".title", out id);
					if (flag4)
					{
						id = assetEditorAssetType.Id;
					}
					AssetTypeConfig assetTypeConfig2 = new AssetTypeConfig
					{
						Name = id,
						Id = assetEditorAssetType.Id,
						AssetTree = assetTree,
						IsColoredIcon = (assetEditorAssetType.Icon != null && assetEditorAssetType.IsColoredIcon),
						Icon = new PatchStyle("AssetEditor/AssetIcons/" + (assetEditorAssetType.Icon ?? "File.png")),
						Path = assetEditorAssetType.Path,
						FileExtension = assetEditorAssetType.FileExtension,
						EditorType = assetEditorAssetType.EditorType
					};
					JObject jobject2;
					bool flag5 = assetTypeConfig2.EditorType == 3 && dictionary.TryGetValue(assetTypeConfig2.Id + ".json", out jobject2);
					if (flag5)
					{
						JObject jobject3 = (JObject)jobject2["hytale"];
						bool flag6 = jobject3 != null;
						if (flag6)
						{
							assetTypeConfig2.Schema = schemas[assetTypeConfig2.Id + ".json"];
							this.ApplySchemaMetadata(assetTypeConfig2, jobject3);
						}
					}
					assetTypes.Add(assetEditorAssetType.Id, assetTypeConfig2);
				}
				this.AssetEditorOverlay.Interface.Engine.RunOnMainThread(this, delegate
				{
					bool isCancellationRequested2 = this._backendLifetimeCancellationToken.IsCancellationRequested;
					if (!isCancellationRequested2)
					{
						foreach (AssetTypeConfig assetTypeConfig3 in assetTypes.Values)
						{
							bool flag7 = assetTypeConfig3.IconImage == null;
							if (!flag7)
							{
								Image iconImage = assetTypeConfig3.IconImage;
								Texture texture = new Texture(Texture.TextureTypes.Texture2D);
								texture.CreateTexture2D(iconImage.Width, iconImage.Height, iconImage.Pixels, 5, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
								this._iconTextures.Add(texture);
								assetTypeConfig3.Icon = new PatchStyle(new TextureArea(texture, 0, 0, iconImage.Width, iconImage.Height, 1));
								assetTypeConfig3.IconImage = null;
							}
						}
						this.AssetEditorOverlay.SetupAssetTypes(schemas, assetTypes);
						this._areAssetTypesInitialized = true;
						bool flag8 = this._serverAssetFileEntries != null;
						if (flag8)
						{
							this.SetupAssetList(0, this._serverAssetFileEntries);
						}
						bool flag9 = this._commonAssetFileEntries != null;
						if (flag9)
						{
							this.SetupAssetList(1, this._commonAssetFileEntries);
						}
					}
				}, false, false);
			}).ContinueWith(delegate(Task task)
			{
				bool flag = !task.IsFaulted;
				if (!flag)
				{
					ServerAssetEditorBackend.Logger.Error(task.Exception, "Failed to initialize asset list");
					Engine engine = this.AssetEditorOverlay.Interface.Engine;
					Disposable <>4__this = this;
					Action action;
					if ((action = <>9__3) == null)
					{
						action = (<>9__3 = delegate()
						{
							bool isCancellationRequested = this._backendLifetimeCancellationToken.IsCancellationRequested;
							if (!isCancellationRequested)
							{
								this.AssetEditorOverlay.ToastNotifications.AddNotification(2, this.AssetEditorOverlay.Interface.GetText("ui.assetEditor.errors.failedToSetupAssetTypes", null, true));
								this.AssetEditorOverlay.SetupAssetFiles(new List<AssetFile>(), new List<AssetFile>(), new List<AssetFile>());
							}
						});
					}
					engine.RunOnMainThread(<>4__this, action, false, false);
				}
			});
		}

		// Token: 0x06006031 RID: 24625 RVA: 0x001F63FC File Offset: 0x001F45FC
		public void UpdateAssetList(AssetEditorFileEntry[] additions, AssetEditorFileEntry[] deletions)
		{
			Debug.Assert(!this._isInitializing);
			bool flag = deletions != null;
			if (flag)
			{
				int i = 0;
				while (i < deletions.Length)
				{
					AssetEditorFileEntry assetEditorFileEntry = deletions[i];
					bool isDirectory = assetEditorFileEntry.IsDirectory;
					if (isDirectory)
					{
						this.AssetEditorOverlay.OnDirectoryDeleted(assetEditorFileEntry.Path);
					}
					else
					{
						string type;
						bool flag2 = !this.AssetEditorOverlay.AssetTypeRegistry.TryGetAssetTypeFromPath(assetEditorFileEntry.Path, out type);
						if (!flag2)
						{
							this.AssetEditorOverlay.OnAssetDeleted(new AssetReference(type, assetEditorFileEntry.Path));
						}
					}
					IL_86:
					i++;
					continue;
					goto IL_86;
				}
			}
			bool flag3 = additions != null;
			if (flag3)
			{
				foreach (AssetEditorFileEntry assetEditorFileEntry2 in additions)
				{
					AssetFile assetFile;
					bool flag4 = this.AssetEditorOverlay.Assets.TryGetAsset(assetEditorFileEntry2.Path, out assetFile, false);
					if (!flag4)
					{
						bool isDirectory2 = assetEditorFileEntry2.IsDirectory;
						if (isDirectory2)
						{
							this.AssetEditorOverlay.OnDirectoryCreated(assetEditorFileEntry2.Path);
						}
						else
						{
							string type2;
							bool flag5 = !this.AssetEditorOverlay.AssetTypeRegistry.TryGetAssetTypeFromPath(assetEditorFileEntry2.Path, out type2);
							if (!flag5)
							{
								this.AssetEditorOverlay.OnAssetAdded(new AssetReference(type2, assetEditorFileEntry2.Path), false);
							}
						}
					}
				}
			}
		}

		// Token: 0x06006032 RID: 24626 RVA: 0x001F6558 File Offset: 0x001F4758
		public void SetupAssetList(AssetEditorFileTree fileTree, AssetEditorFileEntry[] assets)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			AssetEditorFileTree fileTree2 = fileTree;
			AssetEditorFileTree assetEditorFileTree = fileTree2;
			if (assetEditorFileTree != null)
			{
				if (assetEditorFileTree == 1)
				{
					this._commonAssetFileEntries = assets;
				}
			}
			else
			{
				this._serverAssetFileEntries = assets;
			}
			bool flag = !this._areAssetTypesInitialized;
			if (!flag)
			{
				Dictionary<string, Dictionary<string, AssetTypeConfig>> serverDirectoryMapping = new Dictionary<string, Dictionary<string, AssetTypeConfig>>();
				Dictionary<string, AssetTypeConfig> commonExtensionMapping = new Dictionary<string, AssetTypeConfig>();
				foreach (KeyValuePair<string, AssetTypeConfig> keyValuePair in this.AssetEditorOverlay.AssetTypeRegistry.AssetTypes)
				{
					switch (keyValuePair.Value.AssetTree)
					{
					case AssetTreeFolder.Server:
					{
						Dictionary<string, AssetTypeConfig> dictionary;
						bool flag2 = !serverDirectoryMapping.TryGetValue(keyValuePair.Value.Path, out dictionary);
						if (flag2)
						{
							dictionary = (serverDirectoryMapping[keyValuePair.Value.Path] = new Dictionary<string, AssetTypeConfig>());
						}
						dictionary[keyValuePair.Value.FileExtension] = keyValuePair.Value;
						break;
					}
					case AssetTreeFolder.Common:
						commonExtensionMapping[keyValuePair.Value.FileExtension] = keyValuePair.Value;
						break;
					}
				}
				List<AssetFile> targetList = new List<AssetFile>(assets.Length);
				Action <>9__2;
				Action <>9__3;
				Task.Run(delegate()
				{
					bool flag3 = fileTree == 0;
					if (flag3)
					{
						this.SetupServerAssetList(assets, serverDirectoryMapping, targetList);
					}
					else
					{
						this.SetupCommonAssetList(assets, commonExtensionMapping, targetList);
					}
					targetList.Sort(AssetFileComparer.Instance);
					Engine engine = this.AssetEditorOverlay.Interface.Engine;
					Disposable <>4__this = this;
					Action action;
					if ((action = <>9__2) == null)
					{
						action = (<>9__2 = delegate()
						{
							bool isCancellationRequested = this._backendLifetimeCancellationToken.IsCancellationRequested;
							if (!isCancellationRequested)
							{
								AssetEditorFileTree fileTree3 = fileTree;
								AssetEditorFileTree assetEditorFileTree2 = fileTree3;
								if (assetEditorFileTree2 != null)
								{
									if (assetEditorFileTree2 == 1)
									{
										this._commonAssets = targetList;
									}
								}
								else
								{
									this._serverAssets = targetList;
								}
								bool flag4 = this._commonAssets == null || this._serverAssets == null;
								if (!flag4)
								{
									this._isInitializing = false;
									this.AssetEditorOverlay.SetupAssetFiles(this._serverAssets, this._commonAssets, new List<AssetFile>());
									this._commonAssets = null;
									this._serverAssets = null;
								}
							}
						});
					}
					engine.RunOnMainThread(<>4__this, action, false, false);
				}).ContinueWith(delegate(Task task)
				{
					bool flag3 = !task.IsFaulted;
					if (!flag3)
					{
						ServerAssetEditorBackend.Logger.Error(task.Exception, "Failed to initialize asset list");
						Engine engine = this.AssetEditorOverlay.Interface.Engine;
						Disposable <>4__this = this;
						Action action;
						if ((action = <>9__3) == null)
						{
							action = (<>9__3 = delegate()
							{
								bool isCancellationRequested = this._backendLifetimeCancellationToken.IsCancellationRequested;
								if (!isCancellationRequested)
								{
									this.AssetEditorOverlay.ToastNotifications.AddNotification(2, this.AssetEditorOverlay.Interface.GetText("ui.assetEditor.errors.failedToLoadAssetFiles", null, true));
									this.AssetEditorOverlay.SetupAssetFiles(new List<AssetFile>(), new List<AssetFile>(), new List<AssetFile>());
								}
							});
						}
						engine.RunOnMainThread(<>4__this, action, false, false);
					}
				});
			}
		}

		// Token: 0x06006033 RID: 24627 RVA: 0x001F6724 File Offset: 0x001F4924
		private void SetupServerAssetList(AssetEditorFileEntry[] assets, Dictionary<string, Dictionary<string, AssetTypeConfig>> directoryMapping, List<AssetFile> list)
		{
			Dictionary<string, AssetTypeConfig> dictionary = null;
			int num = 0;
			int i = 0;
			while (i < assets.Length)
			{
				AssetEditorFileEntry assetEditorFileEntry = assets[i];
				string path = assetEditorFileEntry.Path;
				string[] array = path.Split(new char[]
				{
					'/'
				});
				bool flag = dictionary != null && array.Length <= num;
				if (flag)
				{
					dictionary = null;
				}
				bool isDirectory = assetEditorFileEntry.IsDirectory;
				if (isDirectory)
				{
					Dictionary<string, AssetTypeConfig> dictionary2;
					bool flag2 = directoryMapping.TryGetValue(path, out dictionary2);
					if (flag2)
					{
						dictionary = dictionary2;
						num = array.Length;
						bool flag3 = dictionary2.Count == 1;
						if (flag3)
						{
							AssetTypeConfig value = Enumerable.First<KeyValuePair<string, AssetTypeConfig>>(dictionary2).Value;
							list.Add(AssetFile.CreateAssetTypeDirectory(value.Name, path, value.Id, array));
						}
						else
						{
							list.Add(AssetFile.CreateDirectory(UnixPathUtil.GetFileName(path), path, array));
						}
					}
					else
					{
						list.Add(AssetFile.CreateDirectory(UnixPathUtil.GetFileName(path), path, array));
					}
				}
				else
				{
					AssetTypeConfig assetTypeConfig;
					bool flag4 = dictionary == null || !dictionary.TryGetValue(UnixPathUtil.GetExtension(path), out assetTypeConfig);
					if (!flag4)
					{
						list.Add(AssetFile.CreateFile(UnixPathUtil.GetFileNameWithoutExtension(path), path, assetTypeConfig.Id, array));
					}
				}
				IL_133:
				i++;
				continue;
				goto IL_133;
			}
		}

		// Token: 0x06006034 RID: 24628 RVA: 0x001F6874 File Offset: 0x001F4A74
		private void SetupCommonAssetList(AssetEditorFileEntry[] assets, Dictionary<string, AssetTypeConfig> extensionMapping, List<AssetFile> list)
		{
			int i = 0;
			while (i < assets.Length)
			{
				AssetEditorFileEntry assetEditorFileEntry = assets[i];
				string path = assetEditorFileEntry.Path;
				string[] pathElements = path.Split(new char[]
				{
					'/'
				});
				bool isDirectory = assetEditorFileEntry.IsDirectory;
				if (isDirectory)
				{
					list.Add(AssetFile.CreateDirectory(Path.GetFileNameWithoutExtension(path), path, pathElements));
				}
				else
				{
					AssetTypeConfig assetTypeConfig;
					bool flag = !extensionMapping.TryGetValue(Path.GetExtension(path), out assetTypeConfig);
					if (!flag)
					{
						list.Add(AssetFile.CreateFile(Path.GetFileNameWithoutExtension(path), path, assetTypeConfig.Id, pathElements));
					}
				}
				IL_88:
				i++;
				continue;
				goto IL_88;
			}
		}

		// Token: 0x06006035 RID: 24629 RVA: 0x001F6918 File Offset: 0x001F4B18
		public override void CreateDirectory(string path, bool applyLocally, Action<string, FormattedMessage> callback)
		{
			string localAssetsPath = this._localAssetsDirectoryPath;
			int token = this._packetHandler.AddPendingCallback<SuccessReply>(this, delegate(FailureReply err, SuccessReply reply)
			{
				this.AssetEditorOverlay.Interface.Engine.RunOnMainThread(this, delegate
				{
					FailureReply err;
					bool flag = err == null;
					if (flag)
					{
						this.AssetEditorOverlay.OnDirectoryCreated(path);
					}
					err = err;
					bool flag2 = ((err != null) ? err.Message : null) != null;
					if (flag2)
					{
						this.ProcessErrorCallback<string>(callback, BsonHelper.ObjectFromBson<FormattedMessage>(err.Message));
					}
					else
					{
						bool applyLocally2 = applyLocally;
						if (applyLocally2)
						{
							this.CreateDirectoryLocally(path, localAssetsPath);
						}
						this.ProcessSuccessCallback<string>(callback, path);
					}
				}, false, false);
			});
			this._connection.SendPacket(new AssetEditorCreateDirectory
			{
				Token = token,
				Path = path
			});
		}

		// Token: 0x06006036 RID: 24630 RVA: 0x001F6994 File Offset: 0x001F4B94
		private void CreateDirectoryLocally(string path, string localAssetsPath)
		{
			try
			{
				string fullPath = Path.GetFullPath(Path.Combine(localAssetsPath, path));
				string fullPath2 = Path.GetFullPath(localAssetsPath);
				bool flag = !Paths.IsSubPathOf(fullPath, fullPath2);
				if (flag)
				{
					throw new Exception("Tried creating directory outside of asset directory at " + fullPath);
				}
				bool flag2 = !Directory.Exists(fullPath);
				if (flag2)
				{
					Directory.CreateDirectory(fullPath);
				}
			}
			catch (Exception exception)
			{
				ServerAssetEditorBackend.Logger.Error(exception, "Failed to delete directory locally {0}", new object[]
				{
					path
				});
			}
		}

		// Token: 0x06006037 RID: 24631 RVA: 0x001F6A24 File Offset: 0x001F4C24
		public override void DeleteDirectory(string path, bool applyLocally, Action<string, FormattedMessage> callback)
		{
			string localAssetsPath = this._localAssetsDirectoryPath;
			int token = this._packetHandler.AddPendingCallback<SuccessReply>(this, delegate(FailureReply err, SuccessReply reply)
			{
				this.AssetEditorOverlay.Interface.Engine.RunOnMainThread(this, delegate
				{
					FailureReply err;
					bool flag = err == null;
					if (flag)
					{
						this.AssetEditorOverlay.OnDirectoryDeleted(path);
					}
					err = err;
					bool flag2 = ((err != null) ? err.Message : null) != null;
					if (flag2)
					{
						this.ProcessErrorCallback<string>(callback, BsonHelper.ObjectFromBson<FormattedMessage>(err.Message));
					}
					else
					{
						bool applyLocally2 = applyLocally;
						if (applyLocally2)
						{
							this.DeleteDirectoryLocally(path, localAssetsPath);
						}
						this.ProcessSuccessCallback<string>(callback, path);
					}
				}, false, false);
			});
			this._connection.SendPacket(new AssetEditorDeleteDirectory
			{
				Token = token,
				Path = path
			});
		}

		// Token: 0x06006038 RID: 24632 RVA: 0x001F6AA0 File Offset: 0x001F4CA0
		private void DeleteDirectoryLocally(string path, string localAssetsPath)
		{
			try
			{
				string fullPath = Path.GetFullPath(Path.Combine(localAssetsPath, path));
				string fullPath2 = Path.GetFullPath(localAssetsPath);
				bool flag = !Paths.IsSubPathOf(fullPath, fullPath2);
				if (flag)
				{
					throw new Exception("Tried removing directory outside of asset directory at " + fullPath);
				}
				bool flag2 = Directory.Exists(fullPath);
				if (flag2)
				{
					Directory.Delete(fullPath, true);
				}
			}
			catch (Exception exception)
			{
				ServerAssetEditorBackend.Logger.Error(exception, "Failed to delete directory locally {0}", new object[]
				{
					path
				});
			}
		}

		// Token: 0x06006039 RID: 24633 RVA: 0x001F6B2C File Offset: 0x001F4D2C
		public override void RenameDirectory(string path, string newPath, bool applyLocally, Action<string, FormattedMessage> callback)
		{
			string localAssetsPath = this._localAssetsDirectoryPath;
			int token = this._packetHandler.AddPendingCallback<SuccessReply>(this, delegate(FailureReply err, SuccessReply reply)
			{
				this.AssetEditorOverlay.Interface.Engine.RunOnMainThread(this, delegate
				{
					FailureReply err;
					bool flag = err == null;
					if (flag)
					{
						this.AssetEditorOverlay.OnDirectoryRenamed(path, newPath);
					}
					err = err;
					bool flag2 = ((err != null) ? err.Message : null) != null;
					if (flag2)
					{
						this.ProcessErrorCallback<string>(callback, BsonHelper.ObjectFromBson<FormattedMessage>(err.Message));
					}
					else
					{
						bool applyLocally2 = applyLocally;
						if (applyLocally2)
						{
							this.RenameDirectoryLocally(path, newPath, localAssetsPath);
						}
						this.ProcessSuccessCallback<string>(callback, newPath);
					}
				}, false, false);
			});
			this._connection.SendPacket(new AssetEditorRenameDirectory
			{
				Token = token,
				Path = path,
				NewPath = newPath
			});
		}

		// Token: 0x0600603A RID: 24634 RVA: 0x001F6BBC File Offset: 0x001F4DBC
		private void RenameDirectoryLocally(string path, string newPath, string localAssetsPath)
		{
			try
			{
				string fullPath = Path.GetFullPath(Path.Combine(localAssetsPath, path));
				string fullPath2 = Path.GetFullPath(Path.Combine(localAssetsPath, newPath));
				string fullPath3 = Path.GetFullPath(localAssetsPath);
				bool flag = !Paths.IsSubPathOf(fullPath, fullPath3);
				if (flag)
				{
					throw new Exception("Tried moving directory outside of asset directory at " + fullPath);
				}
				bool flag2 = !Paths.IsSubPathOf(fullPath2, fullPath3);
				if (flag2)
				{
					throw new Exception("Tried moving directory outside of asset directory at " + fullPath2);
				}
				bool flag3 = Directory.Exists(fullPath) && !Directory.Exists(fullPath2);
				if (flag3)
				{
					Directory.Move(fullPath, fullPath2);
				}
			}
			catch (Exception exception)
			{
				ServerAssetEditorBackend.Logger.Error(exception, "Failed to delete directory locally {0}", new object[]
				{
					path
				});
			}
		}

		// Token: 0x0600603B RID: 24635 RVA: 0x001F6C88 File Offset: 0x001F4E88
		public override void FetchAsset(AssetReference assetReference, Action<object, FormattedMessage> callback, bool fromOpenedTab = false)
		{
			if (fromOpenedTab)
			{
				this._currentAssetCancellationToken.Cancel();
				this._currentAssetCancellationToken = new CancellationTokenSource();
			}
			AssetTypeConfig assetType;
			bool flag = !this.AssetEditorOverlay.AssetTypeRegistry.AssetTypes.TryGetValue(assetReference.Type, out assetType);
			if (flag)
			{
				ServerAssetEditorBackend.Logger.Warn("Tried opening asset with unknown type: {0}", assetReference.Type);
				FormattedMessage message = new FormattedMessage
				{
					MessageId = "ui.assetEditor.errors.unknownAssetType",
					Params = new Dictionary<string, object>
					{
						{
							"assetType",
							assetReference.Type
						}
					}
				};
				this.AssetEditorOverlay.Interface.Engine.RunOnMainThread(this.AssetEditorOverlay.Interface, delegate
				{
					callback(null, message);
				}, true, false);
			}
			else
			{
				int num = this._packetHandler.AddPendingCallback<AssetEditorFetchAssetReply>(this, delegate(FailureReply err, AssetEditorFetchAssetReply reply)
				{
					this.AssetEditorOverlay.Interface.Engine.RunOnMainThread(this, delegate
					{
						bool flag2 = err != null;
						if (flag2)
						{
							FormattedMessage arg = (err.Message != null) ? BsonHelper.ObjectFromBson<FormattedMessage>(err.Message) : null;
							callback(null, arg);
						}
						else
						{
							object arg2;
							FormattedMessage arg3;
							DataConversionUtils.TryDecodeBytes(reply.Contents, assetType.EditorType, out arg2, out arg3);
							callback(arg2, arg3);
						}
					}, false, false);
				});
				this._connection.SendPacket(new AssetEditorFetchAsset(num, assetReference.FilePath, fromOpenedTab));
			}
		}

		// Token: 0x0600603C RID: 24636 RVA: 0x001F6DB4 File Offset: 0x001F4FB4
		public override void FetchJsonAssetWithParents(AssetReference assetReference, Action<Dictionary<string, TrackedAsset>, FormattedMessage> callback, bool fromOpenedTab = false)
		{
			if (fromOpenedTab)
			{
				this._currentAssetCancellationToken.Cancel();
				this._currentAssetCancellationToken = new CancellationTokenSource();
			}
			int num = this._packetHandler.AddPendingCallback<AssetEditorFetchJsonAssetWithParentsReply>(this, delegate(FailureReply err, AssetEditorFetchJsonAssetWithParentsReply reply)
			{
				this.AssetEditorOverlay.Interface.Engine.RunOnMainThread(this, delegate
				{
					bool flag = err != null;
					if (flag)
					{
						FormattedMessage arg = (err.Message != null) ? BsonHelper.ObjectFromBson<FormattedMessage>(err.Message) : null;
						callback(null, arg);
					}
					else
					{
						Dictionary<string, TrackedAsset> dictionary = new Dictionary<string, TrackedAsset>();
						foreach (KeyValuePair<string, string> keyValuePair in reply.Assets)
						{
							JObject data;
							try
							{
								data = JObject.Parse(keyValuePair.Value);
							}
							catch (Exception)
							{
								callback(null, new FormattedMessage
								{
									MessageId = "ui.assetEditor.errors.invalidJson"
								});
								return;
							}
							string type;
							bool flag2 = !this.AssetEditorOverlay.AssetTypeRegistry.TryGetAssetTypeFromPath(keyValuePair.Key, out type);
							if (flag2)
							{
								callback(null, new FormattedMessage
								{
									MessageId = "ui.assetEditor.errors.invalidAssetType"
								});
								return;
							}
							AssetReference assetReference2 = new AssetReference(type, keyValuePair.Key);
							dictionary.Add(keyValuePair.Key, new TrackedAsset(assetReference2, data));
						}
						callback(dictionary, null);
					}
				}, false, false);
			});
			this._connection.SendPacket(new AssetEditorFetchJsonAssetWithParents(num, assetReference.FilePath, fromOpenedTab));
		}

		// Token: 0x0600603D RID: 24637 RVA: 0x001F6E28 File Offset: 0x001F5028
		public override void UpdateJsonAsset(AssetReference assetReference, List<ClientJsonUpdateCommand> jsonUpdateCommands, Action<FormattedMessage> callback = null)
		{
			int token = this._packetHandler.AddPendingCallback<SuccessReply>(this, delegate(FailureReply err, SuccessReply reply)
			{
				this.AssetEditorOverlay.Interface.Engine.RunOnMainThread(this, delegate
				{
					bool flag = err != null;
					if (flag)
					{
						FormattedMessage formattedMessage = (err.Message != null) ? BsonHelper.ObjectFromBson<FormattedMessage>(err.Message) : null;
						this.AssetEditorOverlay.ToastNotifications.AddNotification(2, formattedMessage);
						ServerAssetEditorBackend.Logger.Error("Failed to update json asset: {0}", FormattedMessageConverter.GetString(formattedMessage, this.AssetEditorOverlay.Interface));
						TrackedAsset trackedAsset;
						bool flag2 = this.AssetEditorOverlay.CurrentAsset.Equals(assetReference) && this.AssetEditorOverlay.TrackedAssets.TryGetValue(assetReference.FilePath, out trackedAsset) && !trackedAsset.IsLoading;
						if (flag2)
						{
							bool flag3 = this.AssetEditorOverlay.AssetTypeRegistry.AssetTypes.ContainsKey(assetReference.Type);
							if (flag3)
							{
								this.AssetEditorOverlay.FetchOpenAsset();
							}
							else
							{
								this.AssetEditorOverlay.CloseTab(assetReference);
							}
						}
						Action<FormattedMessage> callback2 = callback;
						if (callback2 != null)
						{
							callback2(formattedMessage);
						}
					}
					else
					{
						ServerAssetEditorBackend.Logger.Info("Updated json asset");
						Action<FormattedMessage> callback3 = callback;
						if (callback3 != null)
						{
							callback3(null);
						}
					}
				}, false, false);
			});
			JsonUpdateCommand[] array = new JsonUpdateCommand[jsonUpdateCommands.Count];
			for (int i = 0; i < jsonUpdateCommands.Count; i++)
			{
				ClientJsonUpdateCommand clientJsonUpdateCommand = jsonUpdateCommands[i];
				JObject jobject = new JObject();
				jobject.Add("value", clientJsonUpdateCommand.Value);
				sbyte[] array2 = BsonHelper.ToBson(jobject);
				JObject jobject2 = new JObject();
				jobject2.Add("value", clientJsonUpdateCommand.PreviousValue);
				sbyte[] array3 = BsonHelper.ToBson(jobject2);
				JsonUpdateCommand[] array4 = array;
				int num = i;
				JsonUpdateType type = clientJsonUpdateCommand.Type;
				string[] elements = clientJsonUpdateCommand.Path.Elements;
				sbyte[] array5 = array2;
				sbyte[] array6 = array3;
				ClientJsonUpdateCommand clientJsonUpdateCommand2 = clientJsonUpdateCommand;
				array4[num] = new JsonUpdateCommand(type, elements, array5, array6, (clientJsonUpdateCommand2.FirstCreatedProperty != null) ? clientJsonUpdateCommand2.FirstCreatedProperty.GetValueOrDefault().Elements : null, clientJsonUpdateCommand.RebuildCaches);
			}
			this._connection.SendPacket(new AssetEditorUpdateJsonAsset
			{
				Token = token,
				AssetType = assetReference.Type,
				Path = assetReference.FilePath,
				Commands = array
			});
		}

		// Token: 0x0600603E RID: 24638 RVA: 0x001F6F60 File Offset: 0x001F5160
		public override void UpdateAsset(AssetReference assetReference, object data, Action<FormattedMessage> callback = null)
		{
			int token = this._packetHandler.AddPendingCallback<SuccessReply>(this, delegate(FailureReply err, SuccessReply reply)
			{
				this.AssetEditorOverlay.Interface.Engine.RunOnMainThread(this, delegate
				{
					bool flag = err != null;
					if (flag)
					{
						FormattedMessage formattedMessage = (err.Message != null) ? BsonHelper.ObjectFromBson<FormattedMessage>(err.Message) : null;
						this.AssetEditorOverlay.ToastNotifications.AddNotification(2, formattedMessage);
						ServerAssetEditorBackend.Logger.Error("Failed to update asset: {0}", FormattedMessageConverter.GetString(formattedMessage, this.AssetEditorOverlay.Interface));
						Action<FormattedMessage> callback2 = callback;
						if (callback2 != null)
						{
							callback2(formattedMessage);
						}
					}
					else
					{
						ServerAssetEditorBackend.Logger.Info("Updated json asset");
						Action<FormattedMessage> callback3 = callback;
						if (callback3 != null)
						{
							callback3(null);
						}
					}
				}, false, false);
			});
			sbyte[] data2 = DataConversionUtils.EncodeObject(data);
			this._connection.SendPacket(new AssetEditorUpdateAsset
			{
				Token = token,
				AssetType = assetReference.Type,
				Path = assetReference.FilePath,
				Data = data2
			});
		}

		// Token: 0x0600603F RID: 24639 RVA: 0x001F6FD9 File Offset: 0x001F51D9
		public override void SetOpenEditorAsset(AssetReference assetReference)
		{
			this._connection.SendPacket(new AssetEditorSelectAsset(assetReference.FilePath));
		}

		// Token: 0x06006040 RID: 24640 RVA: 0x001F6FF3 File Offset: 0x001F51F3
		public override void OnSidebarButtonActivated(string action)
		{
			this._connection.SendPacket(new AssetEditorActivateButton(action));
		}

		// Token: 0x06006041 RID: 24641 RVA: 0x001F7008 File Offset: 0x001F5208
		public override void FetchAutoCompleteData(string dataset, string query, Action<HashSet<string>, FormattedMessage> callback)
		{
			int token = this._packetHandler.AddPendingCallback<AssetEditorFetchAutoCompleteDataReply>(this, delegate(FailureReply err, AssetEditorFetchAutoCompleteDataReply reply)
			{
				bool flag = err != null;
				if (flag)
				{
					FormattedMessage message = BsonHelper.ObjectFromBson<FormattedMessage>(err.Message);
					ServerAssetEditorBackend.Logger.Error("Failed to fetch auto complete data: {0}", FormattedMessageConverter.GetString(message, this.AssetEditorOverlay.Interface));
				}
				else
				{
					this.ProcessSuccessCallback<HashSet<string>>(callback, new HashSet<string>(reply.Results));
				}
			});
			this._connection.SendPacket(new AssetEditorFetchAutoCompleteData
			{
				Token = token,
				Dataset = dataset,
				Query = query
			});
		}

		// Token: 0x06006042 RID: 24642 RVA: 0x001F706C File Offset: 0x001F526C
		public override bool TryGetDropdownEntriesOrFetch(string dataset, out List<string> entries, object extraValue = null)
		{
			bool flag = base.TryGetDropdownEntriesOrFetch(dataset, out entries, extraValue);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = this._dropdownDatasetEntriesCache.TryGetValue(dataset, out entries);
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool flag3 = !this._loadingDropdownDatasets.Contains(dataset);
					if (flag3)
					{
						this._loadingDropdownDatasets.Add(dataset);
						this.LoadDropdownEntries(dataset);
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06006043 RID: 24643 RVA: 0x001F70D0 File Offset: 0x001F52D0
		private void LoadDropdownEntries(string dataset)
		{
			this._connection.SendPacket(new AssetEditorRequestDataset
			{
				Name = dataset
			});
		}

		// Token: 0x06006044 RID: 24644 RVA: 0x001F70EB File Offset: 0x001F52EB
		public void OnDropdownDatasetReceived(string dataset, List<string> entries)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this._loadingDropdownDatasets.Remove(dataset);
			this._dropdownDatasetEntriesCache[dataset] = entries;
			this.AssetEditorOverlay.OnDropdownDatasetUpdated(dataset, entries);
		}

		// Token: 0x06006045 RID: 24645 RVA: 0x001F7124 File Offset: 0x001F5324
		public override void CreateAsset(AssetReference assetReference, object data, string buttonId = null, bool openAssetInTab = false, Action<FormattedMessage> callback = null)
		{
			int token = this._packetHandler.AddPendingCallback<SuccessReply>(this, delegate(FailureReply err, SuccessReply reply)
			{
				this.AssetEditorOverlay.Interface.Engine.RunOnMainThread(this, delegate
				{
					bool flag = err != null;
					if (flag)
					{
						FormattedMessage formattedMessage = (err.Message != null) ? BsonHelper.ObjectFromBson<FormattedMessage>(err.Message) : null;
						Action<FormattedMessage> callback2 = callback;
						if (callback2 != null)
						{
							callback2(formattedMessage);
						}
						this.AssetEditorOverlay.ToastNotifications.AddNotification(2, formattedMessage);
					}
					else
					{
						Action<FormattedMessage> callback3 = callback;
						if (callback3 != null)
						{
							callback3(null);
						}
						this.AssetEditorOverlay.OnAssetAdded(assetReference, false);
						bool openAssetInTab2 = openAssetInTab;
						if (openAssetInTab2)
						{
							this.AssetEditorOverlay.OpenCreatedAsset(assetReference, data);
						}
					}
				}, false, false);
			});
			sbyte[] data2 = DataConversionUtils.EncodeObject(data);
			this._connection.SendPacket(new AssetEditorCreateAsset
			{
				Token = token,
				Path = assetReference.FilePath,
				Data = data2,
				RebuildCaches = this.GetCachesToRebuild(assetReference.Type),
				ButtonId = buttonId
			});
		}

		// Token: 0x06006046 RID: 24646 RVA: 0x001F71D0 File Offset: 0x001F53D0
		public override void DeleteAsset(AssetReference assetReference, bool applyLocally)
		{
			string localAssetsPath = this._localAssetsDirectoryPath;
			int token = this._packetHandler.AddPendingCallback<SuccessReply>(this, delegate(FailureReply err, SuccessReply reply)
			{
				this.AssetEditorOverlay.Interface.Engine.RunOnMainThread(this, delegate
				{
					bool flag = err != null;
					if (flag)
					{
						FormattedMessage message = (err.Message != null) ? BsonHelper.ObjectFromBson<FormattedMessage>(err.Message) : null;
						this.AssetEditorOverlay.ToastNotifications.AddNotification(2, message);
					}
					else
					{
						bool applyLocally2 = applyLocally;
						if (applyLocally2)
						{
							this.DeleteFileLocally(assetReference, localAssetsPath);
						}
						this.AssetEditorOverlay.OnAssetDeleted(assetReference);
					}
				}, false, false);
			});
			this._connection.SendPacket(new AssetEditorDeleteAsset
			{
				Token = token,
				Path = assetReference.FilePath
			});
		}

		// Token: 0x06006047 RID: 24647 RVA: 0x001F7248 File Offset: 0x001F5448
		private void DeleteFileLocally(AssetReference assetReference, string localAssetsPath)
		{
			try
			{
				string fullPath = Path.GetFullPath(Path.Combine(localAssetsPath, assetReference.FilePath));
				string fullPath2 = Path.GetFullPath(localAssetsPath);
				bool flag = !Paths.IsSubPathOf(fullPath, fullPath2);
				if (flag)
				{
					throw new Exception("Tried removing asset file outside of asset directory at " + fullPath);
				}
				bool flag2 = File.Exists(fullPath);
				if (flag2)
				{
					File.Delete(fullPath);
				}
			}
			catch (Exception exception)
			{
				ServerAssetEditorBackend.Logger.Error(exception, "Failed to delete asset locally {0}", new object[]
				{
					assetReference
				});
				this.AssetEditorOverlay.ToastNotifications.AddNotification(2, this.AssetEditorOverlay.Interface.GetText("ui.assetEditor.errors.failedToDeleteAsset", null, true));
			}
		}

		// Token: 0x06006048 RID: 24648 RVA: 0x001F7308 File Offset: 0x001F5508
		public override void RenameAsset(AssetReference assetReference, string newAssetFilePath, bool applyLocally)
		{
			string localAssetsPath = this._localAssetsDirectoryPath;
			int token = this._packetHandler.AddPendingCallback<SuccessReply>(this, delegate(FailureReply err, SuccessReply reply)
			{
				this.AssetEditorOverlay.Interface.Engine.RunOnMainThread(this, delegate
				{
					bool flag = err != null;
					if (flag)
					{
						FormattedMessage message = (err.Message != null) ? BsonHelper.ObjectFromBson<FormattedMessage>(err.Message) : null;
						this.AssetEditorOverlay.ToastNotifications.AddNotification(2, message);
					}
					else
					{
						bool applyLocally2 = applyLocally;
						if (applyLocally2)
						{
							this.RenameAssetLocally(assetReference, newAssetFilePath, localAssetsPath);
						}
						this.AssetEditorOverlay.OnAssetRenamed(assetReference, new AssetReference(assetReference.Type, newAssetFilePath));
					}
				}, false, false);
			});
			this._connection.SendPacket(new AssetEditorRenameAsset
			{
				Token = token,
				Path = assetReference.FilePath,
				NewPath = newAssetFilePath
			});
		}

		// Token: 0x06006049 RID: 24649 RVA: 0x001F7394 File Offset: 0x001F5594
		private void RenameAssetLocally(AssetReference assetReference, string newFilePath, string localAssetsPath)
		{
			string fullPath = Path.GetFullPath(Path.Combine(localAssetsPath, assetReference.FilePath));
			string fullPath2 = Path.GetFullPath(Path.Combine(localAssetsPath, newFilePath));
			string fullPath3 = Path.GetFullPath(localAssetsPath);
			try
			{
				bool flag = !Paths.IsSubPathOf(fullPath, fullPath3);
				if (flag)
				{
					ServerAssetEditorBackend.Logger.Warn("Tried moving asset file from folder outside of asset directory at " + fullPath);
					this.AssetEditorOverlay.ToastNotifications.AddNotification(2, this.AssetEditorOverlay.Interface.GetText("ui.assetEditor.errors.failedToRenameAsset", null, true));
				}
				else
				{
					bool flag2 = !Paths.IsSubPathOf(fullPath2, fullPath3);
					if (flag2)
					{
						ServerAssetEditorBackend.Logger.Warn("Tried moving asset file to folder outside of asset directory at " + fullPath);
						this.AssetEditorOverlay.ToastNotifications.AddNotification(2, this.AssetEditorOverlay.Interface.GetText("ui.assetEditor.errors.failedToRenameAsset", null, true));
					}
					else
					{
						bool flag3 = Directory.Exists(fullPath);
						if (flag3)
						{
							Directory.CreateDirectory(Path.GetDirectoryName(fullPath2));
							File.Move(fullPath, fullPath2);
						}
					}
				}
			}
			catch (Exception exception)
			{
				ServerAssetEditorBackend.Logger.Error(exception, "Failed to move file locally from {0} to {1}", new object[]
				{
					fullPath,
					fullPath2
				});
				this.AssetEditorOverlay.ToastNotifications.AddNotification(2, this.AssetEditorOverlay.Interface.GetText("ui.assetEditor.errors.failedToRenameAsset", null, true));
			}
		}

		// Token: 0x0600604A RID: 24650 RVA: 0x001F74F4 File Offset: 0x001F56F4
		private int CreateUndoRedoCallback(AssetReference assetReference)
		{
			CancellationToken cancellationToken = this._currentAssetCancellationToken.Token;
			this.AssetEditorOverlay.ConfigEditor.SetWaitingForBackend(true);
			return this._packetHandler.AddPendingCallback<AssetEditorUndoRedoReply>(this, delegate(FailureReply err, AssetEditorUndoRedoReply reply)
			{
				this.AssetEditorOverlay.Interface.Engine.RunOnMainThread(this, delegate
				{
					bool isCancellationRequested = cancellationToken.IsCancellationRequested;
					if (!isCancellationRequested)
					{
						this.AssetEditorOverlay.ConfigEditor.SetWaitingForBackend(false);
						bool flag = err != null;
						if (flag)
						{
							FormattedMessage message = (err.Message != null) ? BsonHelper.ObjectFromBson<FormattedMessage>(err.Message) : null;
							this.AssetEditorOverlay.ToastNotifications.AddNotification(2, message);
						}
						else
						{
							JObject jobject = (JObject)this.AssetEditorOverlay.TrackedAssets[assetReference.FilePath].Data;
							JsonUpdateCommand command = reply.Command;
							JToken jtoken = (command.Value != null) ? BsonHelper.FromBson(command.Value)["value"] : null;
							bool flag2 = command.Path.Length == 0 && command.Type == 0;
							if (flag2)
							{
								jobject = (JObject)jtoken;
								this.AssetEditorOverlay.SetTrackedAssetData(assetReference.FilePath, jobject);
							}
							else
							{
								JsonUpdateType type = command.Type;
								JsonUpdateType jsonUpdateType = type;
								if (jsonUpdateType > 1)
								{
									if (jsonUpdateType == 2)
									{
										PropertyPath? propertyPath;
										JToken jtoken2;
										this.AssetEditorOverlay.ConfigEditor.RemoveProperty(jobject, PropertyPath.FromElements(command.Path), out propertyPath, out jtoken2, true, false);
									}
								}
								else
								{
									PropertyPath? propertyPath;
									this.AssetEditorOverlay.ConfigEditor.SetProperty(jobject, PropertyPath.FromElements(command.Path), jtoken, out propertyPath, true, command.Type == 1);
								}
							}
							this.AssetEditorOverlay.Layout(null, true);
						}
					}
				}, false, false);
			});
		}

		// Token: 0x0600604B RID: 24651 RVA: 0x001F7558 File Offset: 0x001F5758
		public override void UndoChanges(AssetReference assetReference)
		{
			int num = this.CreateUndoRedoCallback(assetReference);
			this._connection.SendPacket(new AssetEditorUndoChanges(num, assetReference.FilePath));
		}

		// Token: 0x0600604C RID: 24652 RVA: 0x001F7588 File Offset: 0x001F5788
		public override void RedoChanges(AssetReference assetReference)
		{
			int num = this.CreateUndoRedoCallback(assetReference);
			this._connection.SendPacket(new AssetEditorRedoChanges(num, assetReference.FilePath));
		}

		// Token: 0x0600604D RID: 24653 RVA: 0x001F75B8 File Offset: 0x001F57B8
		public override void DiscardChanges(List<TimestampedAssetReference> assetsToDiscard)
		{
			TimestampedAssetReference[] array = new TimestampedAssetReference[assetsToDiscard.Count];
			for (int i = 0; i < assetsToDiscard.Count; i++)
			{
				array[i] = assetsToDiscard[i].ToPacket();
			}
			this._connection.SendPacket(new AssetEditorDiscardChanges(array));
		}

		// Token: 0x0600604E RID: 24654 RVA: 0x001F760C File Offset: 0x001F580C
		public override void ExportAssets(List<AssetReference> assetReferences, Action<List<TimestampedAssetReference>> callback = null)
		{
			bool isExportingAssets = base.IsExportingAssets;
			if (isExportingAssets)
			{
				this.AssetEditorOverlay.ToastNotifications.AddNotification(2, this.AssetEditorOverlay.Interface.GetText("ui.assetEditor.errors.exportInProgress", null, true));
			}
			else
			{
				AssetEditorExportAssets assetEditorExportAssets = new AssetEditorExportAssets(new string[assetReferences.Count]);
				for (int i = 0; i < assetReferences.Count; i++)
				{
					assetEditorExportAssets.Paths[i] = assetReferences[i].FilePath;
					this.AssetExportStatuses[assetReferences[i].FilePath] = ServerAssetEditorBackend.AssetExportStatus.Pending;
				}
				this.LocalAssetsDirectoryPathForCurrentExport = this._localAssetsDirectoryPath;
				base.IsExportingAssets = true;
				this._exportCompleteCallback = callback;
				this.AssetEditorOverlay.ExportModal.UpdateExportButtonState();
				this._connection.SendPacket(assetEditorExportAssets);
			}
		}

		// Token: 0x0600604F RID: 24655 RVA: 0x001F76E4 File Offset: 0x001F58E4
		public void OnExportProgress()
		{
			bool flag = !base.IsExportingAssets;
			if (!flag)
			{
				int num = 0;
				foreach (ServerAssetEditorBackend.AssetExportStatus assetExportStatus in this.AssetExportStatuses.Values)
				{
					bool flag2 = assetExportStatus > ServerAssetEditorBackend.AssetExportStatus.Pending;
					if (flag2)
					{
						num++;
					}
				}
				ServerAssetEditorBackend.Logger.Info(string.Format("Export progress {0}/{1}", num, this.AssetExportStatuses.Count));
			}
		}

		// Token: 0x06006050 RID: 24656 RVA: 0x001F7780 File Offset: 0x001F5980
		public void OnExportComplete(TimestampedAssetReference[] sentAssets)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			int num = 0;
			int num2 = 0;
			foreach (ServerAssetEditorBackend.AssetExportStatus assetExportStatus in this.AssetExportStatuses.Values)
			{
				bool flag = assetExportStatus > ServerAssetEditorBackend.AssetExportStatus.Pending;
				if (flag)
				{
					num++;
				}
				bool flag2 = assetExportStatus == ServerAssetEditorBackend.AssetExportStatus.Complete;
				if (flag2)
				{
					num2++;
				}
			}
			Debug.Assert(num == this.AssetExportStatuses.Count);
			Debug.Assert(base.IsExportingAssets);
			bool flag3 = !base.IsExportingAssets;
			if (!flag3)
			{
				List<TimestampedAssetReference> list = new List<TimestampedAssetReference>();
				foreach (TimestampedAssetReference timestampedAssetReference in sentAssets)
				{
					ServerAssetEditorBackend.AssetExportStatus assetExportStatus2;
					bool flag4 = !this.AssetExportStatuses.TryGetValue(timestampedAssetReference.Path, ref assetExportStatus2) || assetExportStatus2 != ServerAssetEditorBackend.AssetExportStatus.Complete;
					if (!flag4)
					{
						list.Add(timestampedAssetReference);
					}
				}
				base.IsExportingAssets = false;
				this.AssetExportStatuses.Clear();
				this.AssetEditorOverlay.ExportModal.UpdateExportButtonState();
				ServerAssetEditorBackend.Logger.Info("Export complete");
				this.AssetEditorOverlay.ToastNotifications.AddNotification(1, string.Format("Exported {0}/{1} assets", num2, num));
				bool flag5 = this._exportCompleteCallback != null;
				if (flag5)
				{
					Action<List<TimestampedAssetReference>> exportCompleteCallback = this._exportCompleteCallback;
					this._exportCompleteCallback = null;
					exportCompleteCallback(list);
				}
			}
		}

		// Token: 0x06006051 RID: 24657 RVA: 0x001F7910 File Offset: 0x001F5B10
		public override void FetchLastModifiedAssets()
		{
			this._connection.SendPacket(new AssetEditorFetchLastModifiedAssets());
		}

		// Token: 0x06006052 RID: 24658 RVA: 0x001F7924 File Offset: 0x001F5B24
		public override void UpdateSubscriptionToModifiedAssetsUpdates(bool subscribe)
		{
			this._connection.SendPacket(new AssetEditorSubscribeModifiedAssetsChanges(subscribe));
		}

		// Token: 0x06006053 RID: 24659 RVA: 0x001F7939 File Offset: 0x001F5B39
		public override AssetEditorLastModifiedAssets.AssetInfo[] GetLastModifiedAssets()
		{
			return this._lastModifiedAssets;
		}

		// Token: 0x06006054 RID: 24660 RVA: 0x001F7944 File Offset: 0x001F5B44
		public void SetupLastModifiedAssets(AssetEditorLastModifiedAssets.AssetInfo[] assets)
		{
			bool isCancellationRequested = this._backendLifetimeCancellationToken.IsCancellationRequested;
			if (!isCancellationRequested)
			{
				Array.Sort<AssetEditorLastModifiedAssets.AssetInfo>(assets, (AssetEditorLastModifiedAssets.AssetInfo a, AssetEditorLastModifiedAssets.AssetInfo b) => (int)((float)(b.LastModificationDate - a.LastModificationDate) / 1000f));
				this._lastModifiedAssets = assets;
				this.AssetEditorOverlay.SetModifiedAssetsCount(this._lastModifiedAssets.Length);
				bool isMounted = this.AssetEditorOverlay.ExportModal.IsMounted;
				if (isMounted)
				{
					this.AssetEditorOverlay.ExportModal.Setup();
				}
			}
		}

		// Token: 0x06006055 RID: 24661 RVA: 0x001F79D0 File Offset: 0x001F5BD0
		public void SetupModifiedAssetsCount(int count)
		{
			bool isCancellationRequested = this._backendLifetimeCancellationToken.IsCancellationRequested;
			if (!isCancellationRequested)
			{
				this.AssetEditorOverlay.SetModifiedAssetsCount(count);
			}
		}

		// Token: 0x06006056 RID: 24662 RVA: 0x001F7A00 File Offset: 0x001F5C00
		public void OnAssetUpdated(string filePath, sbyte[] data)
		{
			AssetTypeRegistry assetTypeRegistry = this.AssetEditorOverlay.AssetTypeRegistry;
			string key;
			AssetTypeConfig assetTypeConfig;
			bool flag = !this.AssetEditorOverlay.TrackedAssets.ContainsKey(filePath) || !assetTypeRegistry.TryGetAssetTypeFromPath(filePath, out key) || !assetTypeRegistry.AssetTypes.TryGetValue(key, out assetTypeConfig);
			if (!flag)
			{
				object asset;
				FormattedMessage formattedMessage;
				bool flag2 = !DataConversionUtils.TryDecodeBytes(data, assetTypeConfig.EditorType, out asset, out formattedMessage);
				if (!flag2)
				{
					this.AssetEditorOverlay.SetTrackedAssetData(filePath, asset);
				}
			}
		}

		// Token: 0x06006057 RID: 24663 RVA: 0x001F7A80 File Offset: 0x001F5C80
		public override void SetGameTime(DateTime time, bool paused)
		{
			InstantData instantData = TimeHelper.DateTimeToInstantData(time);
			this._connection.SendPacket(new AssetEditorSetGameTime(instantData, paused));
		}

		// Token: 0x06006058 RID: 24664 RVA: 0x001F7AA8 File Offset: 0x001F5CA8
		public override void SetWeatherAndTimeLock(bool locked)
		{
			this._connection.SendPacket(new AssetEditorUpdateWeatherPreviewLock(locked));
		}

		// Token: 0x06006059 RID: 24665 RVA: 0x001F7AC0 File Offset: 0x001F5CC0
		public void OnJsonAssetUpdated(string assetPath, JsonUpdateCommand[] commands)
		{
			TrackedAsset trackedAsset;
			bool flag = !this.AssetEditorOverlay.TrackedAssets.TryGetValue(assetPath, out trackedAsset);
			if (!flag)
			{
				JObject jobject = (JObject)trackedAsset.Data;
				foreach (JsonUpdateCommand jsonUpdateCommand in commands)
				{
					JToken jtoken = (jsonUpdateCommand.Value != null) ? BsonHelper.FromBson(jsonUpdateCommand.Value)["value"] : null;
					bool flag2 = jsonUpdateCommand.Path.Length == 0 && jsonUpdateCommand.Type == 0;
					if (flag2)
					{
						jobject = (JObject)jtoken;
						this.AssetEditorOverlay.SetTrackedAssetData(assetPath, jobject);
					}
					else
					{
						JsonUpdateType type = jsonUpdateCommand.Type;
						JsonUpdateType jsonUpdateType = type;
						if (jsonUpdateType > 1)
						{
							if (jsonUpdateType == 2)
							{
								PropertyPath? propertyPath;
								JToken jtoken2;
								this.AssetEditorOverlay.ConfigEditor.RemoveProperty(jobject, PropertyPath.FromElements(jsonUpdateCommand.Path), out propertyPath, out jtoken2, true, false);
							}
						}
						else
						{
							PropertyPath? propertyPath;
							this.AssetEditorOverlay.ConfigEditor.SetProperty(jobject, PropertyPath.FromElements(jsonUpdateCommand.Path), jtoken, out propertyPath, true, jsonUpdateCommand.Type == 1);
						}
					}
				}
				this.AssetEditorOverlay.Layout(null, true);
			}
		}

		// Token: 0x0600605A RID: 24666 RVA: 0x001F7C04 File Offset: 0x001F5E04
		public override void OnLanguageChanged()
		{
			AssetEditorSettings settings = this.AssetEditorOverlay.Interface.App.Settings;
			this._connection.SendPacket(new UpdateLanguage(settings.Language ?? Language.SystemLanguage));
		}

		// Token: 0x0600605B RID: 24667 RVA: 0x001F7C48 File Offset: 0x001F5E48
		private AssetEditorRebuildCaches GetCachesToRebuild(string assetType)
		{
			AssetTypeConfig assetTypeConfig = this.AssetEditorOverlay.AssetTypeRegistry.AssetTypes[assetType];
			bool flag = assetTypeConfig.RebuildCaches == null;
			AssetEditorRebuildCaches result;
			if (flag)
			{
				result = new AssetEditorRebuildCaches();
			}
			else
			{
				result = new AssetEditorRebuildCaches
				{
					Models = assetTypeConfig.RebuildCaches.Contains(AssetTypeConfig.RebuildCacheType.Models),
					ModelTextures = assetTypeConfig.RebuildCaches.Contains(AssetTypeConfig.RebuildCacheType.ModelTextures),
					BlockTextures = assetTypeConfig.RebuildCaches.Contains(AssetTypeConfig.RebuildCacheType.BlockTextures),
					ItemIcons = assetTypeConfig.RebuildCaches.Contains(AssetTypeConfig.RebuildCacheType.ItemIcons),
					MapGeometry = assetTypeConfig.RebuildCaches.Contains(AssetTypeConfig.RebuildCacheType.MapGemoetry)
				};
			}
			return result;
		}

		// Token: 0x04003BE3 RID: 15331
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003BE4 RID: 15332
		private bool _isInitializing;

		// Token: 0x04003BE5 RID: 15333
		private bool _isInitializingOrInitialized;

		// Token: 0x04003BE6 RID: 15334
		private bool _areAssetTypesInitialized;

		// Token: 0x04003BE7 RID: 15335
		private readonly CancellationTokenSource _backendLifetimeCancellationTokenSource = new CancellationTokenSource();

		// Token: 0x04003BE8 RID: 15336
		private readonly CancellationToken _backendLifetimeCancellationToken;

		// Token: 0x04003BE9 RID: 15337
		private Action<List<TimestampedAssetReference>> _exportCompleteCallback;

		// Token: 0x04003BEA RID: 15338
		private Dictionary<string, List<string>> _dropdownDatasetEntriesCache = new Dictionary<string, List<string>>();

		// Token: 0x04003BEB RID: 15339
		private List<string> _loadingDropdownDatasets = new List<string>();

		// Token: 0x04003BEC RID: 15340
		private List<Texture> _iconTextures = new List<Texture>();

		// Token: 0x04003BED RID: 15341
		private List<AssetFile> _commonAssets;

		// Token: 0x04003BEE RID: 15342
		private List<AssetFile> _serverAssets;

		// Token: 0x04003BEF RID: 15343
		private AssetEditorFileEntry[] _serverAssetFileEntries;

		// Token: 0x04003BF0 RID: 15344
		private AssetEditorFileEntry[] _commonAssetFileEntries;

		// Token: 0x04003BF1 RID: 15345
		public readonly ConcurrentDictionary<string, ServerAssetEditorBackend.AssetExportStatus> AssetExportStatuses = new ConcurrentDictionary<string, ServerAssetEditorBackend.AssetExportStatus>();

		// Token: 0x04003BF2 RID: 15346
		private AssetEditorLastModifiedAssets.AssetInfo[] _lastModifiedAssets = new AssetEditorLastModifiedAssets.AssetInfo[0];

		// Token: 0x04003BF3 RID: 15347
		private AssetEditorSetupSchemas.SchemaFile[] schemaFilesToProcess;

		// Token: 0x04003BF4 RID: 15348
		private CancellationTokenSource _currentAssetCancellationToken = new CancellationTokenSource();

		// Token: 0x04003BF5 RID: 15349
		private string _localAssetsDirectoryPath;

		// Token: 0x04003BF7 RID: 15351
		private readonly ConnectionToServer _connection;

		// Token: 0x04003BF8 RID: 15352
		private readonly AssetEditorPacketHandler _packetHandler;

		// Token: 0x02001003 RID: 4099
		public enum AssetExportStatus
		{
			// Token: 0x04004CC6 RID: 19654
			Pending,
			// Token: 0x04004CC7 RID: 19655
			Complete,
			// Token: 0x04004CC8 RID: 19656
			Failed
		}
	}
}
