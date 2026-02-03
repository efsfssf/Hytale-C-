using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using HytaleClient.AssetEditor.Backends;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Interface.Config;
using HytaleClient.AssetEditor.Interface.Elements;
using HytaleClient.AssetEditor.Interface.Modals;
using HytaleClient.AssetEditor.Utils;
using HytaleClient.Data;
using HytaleClient.Data.UserSettings;
using HytaleClient.Interface.Messages;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using Newtonsoft.Json.Linq;
using NLog;
using SDL2;

namespace HytaleClient.AssetEditor.Interface.Editor
{
	// Token: 0x02000BB0 RID: 2992
	internal class AssetEditorOverlay : Element
	{
		// Token: 0x170013BD RID: 5053
		// (get) Token: 0x06005D12 RID: 23826 RVA: 0x001D79B1 File Offset: 0x001D5BB1
		// (set) Token: 0x06005D13 RID: 23827 RVA: 0x001D79B9 File Offset: 0x001D5BB9
		public AssetReference CurrentAsset { get; private set; }

		// Token: 0x170013BE RID: 5054
		// (get) Token: 0x06005D14 RID: 23828 RVA: 0x001D79C2 File Offset: 0x001D5BC2
		// (set) Token: 0x06005D15 RID: 23829 RVA: 0x001D79CA File Offset: 0x001D5BCA
		public Dictionary<string, AssetDiagnostics> Diagnostics { get; private set; } = new Dictionary<string, AssetDiagnostics>();

		// Token: 0x06005D16 RID: 23830 RVA: 0x001D79D3 File Offset: 0x001D5BD3
		public void UpdateTextAsset(string path, string text)
		{
			this.TrackedAssets[path].Data = text;
			this.Backend.UpdateAsset(this.CurrentAsset, text, null);
		}

		// Token: 0x06005D17 RID: 23831 RVA: 0x001D79FC File Offset: 0x001D5BFC
		public void UpdateJsonAsset(string path, JObject json, AssetEditorRebuildCaches cachesToRebuild)
		{
			JObject jobject = (JObject)this.TrackedAssets[path].Data;
			JToken previousValue = (jobject != null) ? jobject.DeepClone() : null;
			this.TrackedAssets[path].Data = json;
			this.Backend.UpdateJsonAsset(this.CurrentAsset, new List<ClientJsonUpdateCommand>
			{
				new ClientJsonUpdateCommand
				{
					Type = 0,
					Value = json,
					PreviousValue = previousValue,
					Path = PropertyPath.FromString(""),
					RebuildCaches = cachesToRebuild
				}
			}, null);
			bool flag = base.IsMounted && path == this.CurrentAsset.FilePath;
			if (flag)
			{
				bool isMounted = this.ConfigEditor.IsMounted;
				if (isMounted)
				{
					this.ConfigEditor.UpdateJson(json);
				}
				else
				{
					this.SetupEditorPane();
				}
			}
		}

		// Token: 0x06005D18 RID: 23832 RVA: 0x001D7AD6 File Offset: 0x001D5CD6
		public void UpdateImageAsset(string path, Image image)
		{
			this.TrackedAssets[path].Data = image;
			this.Backend.UpdateAsset(this.CurrentAsset, image, null);
		}

		// Token: 0x06005D19 RID: 23833 RVA: 0x001D7B00 File Offset: 0x001D5D00
		public void SetTrackedAssetData(string path, object asset)
		{
			TrackedAsset trackedAsset;
			bool flag = !this.TrackedAssets.TryGetValue(path, out trackedAsset);
			if (!flag)
			{
				bool flag2 = trackedAsset.IsLoading || trackedAsset.FetchError != null;
				if (!flag2)
				{
					trackedAsset.Data = asset;
					this.OnTrackedAssetUpdated(trackedAsset, false);
				}
			}
		}

		// Token: 0x06005D1A RID: 23834 RVA: 0x001D7B4F File Offset: 0x001D5D4F
		public void CreateAsset(AssetReference assetReference, JObject data, string buttonId, Action<FormattedMessage> callback)
		{
			this.Backend.CreateAsset(assetReference, data, buttonId, true, callback);
		}

		// Token: 0x06005D1B RID: 23835 RVA: 0x001D7B64 File Offset: 0x001D5D64
		private void UpdateAssetTreeForPath(string path, bool doLayout = true)
		{
			AssetTreeFolder assetTreeFolder;
			AssetPathUtils.TryGetAssetTreeFolder(path, out assetTreeFolder, false);
			List<AssetFile> assets = this.Assets.GetAssets(assetTreeFolder);
			AssetTree assetTree = this.GetAssetTree(assetTreeFolder);
			assetTree.UpdateFiles(assets, null);
			if (doLayout)
			{
				assetTree.Layout(null, true);
			}
		}

		// Token: 0x06005D1C RID: 23836 RVA: 0x001D7BB4 File Offset: 0x001D5DB4
		public void OnAssetDeleted(AssetReference assetReference)
		{
			bool flag = this.CurrentAsset.FilePath != null && this.CurrentAsset.Equals(assetReference);
			if (flag)
			{
				this.CloseTab(this.CurrentAsset);
			}
			bool flag2 = !this.Assets.TryRemoveFile(assetReference.FilePath);
			if (!flag2)
			{
				this.UpdateAssetTreeForPath(assetReference.FilePath, true);
				this.ConfigEditor.OnAssetDeleted(assetReference);
			}
		}

		// Token: 0x06005D1D RID: 23837 RVA: 0x001D7C2C File Offset: 0x001D5E2C
		public void OnAssetRenamed(AssetReference oldReference, AssetReference newReference)
		{
			bool flag = !this.Assets.TryMoveFile(oldReference.FilePath, newReference.FilePath);
			if (!flag)
			{
				this.UpdateAssetTreeForPath(oldReference.FilePath, true);
				this.UpdateTab(oldReference, newReference);
				TrackedAsset trackedAsset;
				bool flag2 = this.TrackedAssets.TryGetValue(oldReference.FilePath, out trackedAsset);
				if (flag2)
				{
					this.TrackedAssets.Remove(oldReference.FilePath);
					this.TrackedAssets[newReference.FilePath] = new TrackedAsset(newReference, trackedAsset.Data);
					AssetTypeConfig assetTypeConfig = this.AssetTypeRegistry.AssetTypes[newReference.Type];
					JObject jobject;
					bool flag3;
					if (assetTypeConfig.IsJson && assetTypeConfig.HasIdField)
					{
						jobject = (trackedAsset.Data as JObject);
						flag3 = (jobject != null);
					}
					else
					{
						flag3 = false;
					}
					bool flag4 = flag3;
					if (flag4)
					{
						jobject["Id"] = this.GetAssetIdFromReference(newReference);
					}
				}
				bool flag5 = this.CurrentAsset.FilePath == oldReference.FilePath;
				if (flag5)
				{
					this.CurrentAsset = newReference;
					this.Backend.SetOpenEditorAsset(newReference);
					this.SelectAssetTreeEntry(newReference, false, true);
				}
				this.ConfigEditor.OnAssetRenamed(oldReference, newReference);
			}
		}

		// Token: 0x06005D1E RID: 23838 RVA: 0x001D7D68 File Offset: 0x001D5F68
		public void OnAssetAdded(AssetReference assetReference, bool openInEditor)
		{
			bool flag = !this.Assets.TryInsertFile(assetReference.FilePath);
			if (!flag)
			{
				this.UpdateAssetTreeForPath(assetReference.FilePath, false);
				bool flag2 = assetReference.Equals(this.CurrentAsset);
				if (flag2)
				{
					this.SelectAssetTreeEntry(assetReference, true, false);
				}
				this._assetBrowser.Layout(null, true);
				if (openInEditor)
				{
					this.AddTab(assetReference, true);
					this.SetupEditorPane();
				}
			}
		}

		// Token: 0x06005D1F RID: 23839 RVA: 0x001D7DE8 File Offset: 0x001D5FE8
		public void OpenCreatedAsset(AssetReference assetReference, object data)
		{
			this.ResetTrackedAssets();
			this.SelectAssetTreeEntry(assetReference, true, true);
			this.AddTab(assetReference, true);
			this.CurrentAsset = assetReference;
			this.TrackedAssets[assetReference.FilePath] = new TrackedAsset(assetReference, data);
			this.Interface.App.Editor.ClearPreview(false);
			this.SetupEditorPane();
			this.Backend.SetOpenEditorAsset(assetReference);
		}

		// Token: 0x06005D20 RID: 23840 RVA: 0x001D7E5C File Offset: 0x001D605C
		public void OnDirectoryContentsUpdated(string path, List<AssetFile> newAssetFiles)
		{
			bool flag = !this.Assets.TryReplaceDirectoryContents(path, newAssetFiles);
			if (!flag)
			{
				this.UpdateAssetTreeForPath(path, true);
			}
		}

		// Token: 0x06005D21 RID: 23841 RVA: 0x001D7E8C File Offset: 0x001D608C
		public void OnDirectoryCreated(string path)
		{
			bool flag = !this.Assets.TryInsertDirectory(path);
			if (!flag)
			{
				this.UpdateAssetTreeForPath(path, true);
			}
		}

		// Token: 0x06005D22 RID: 23842 RVA: 0x001D7EB8 File Offset: 0x001D60B8
		public void OnDirectoryRenamed(string oldPath, string newPath)
		{
			Dictionary<string, AssetFile> dictionary;
			bool flag = !this.Assets.TryMoveDirectory(oldPath, newPath, out dictionary);
			if (!flag)
			{
				foreach (KeyValuePair<string, AssetFile> keyValuePair in dictionary)
				{
					bool isDirectory = keyValuePair.Value.IsDirectory;
					if (!isDirectory)
					{
						string type;
						bool flag2 = !this.AssetTypeRegistry.TryGetAssetTypeFromPath(keyValuePair.Key, out type);
						if (!flag2)
						{
							AssetReference assetReference = new AssetReference(type, keyValuePair.Key);
							AssetReference assetReference2 = new AssetReference(keyValuePair.Value.AssetType, keyValuePair.Value.Path);
							this.UpdateTab(assetReference, assetReference2);
							bool flag3 = this.CurrentAsset.Equals(assetReference);
							if (flag3)
							{
								this.CurrentAsset = assetReference2;
								this.SelectAssetTreeEntry(assetReference2, false, true);
							}
							this.ConfigEditor.OnAssetRenamed(assetReference, assetReference2);
						}
					}
				}
				this.UpdateAssetTreeForPath(oldPath, true);
			}
		}

		// Token: 0x06005D23 RID: 23843 RVA: 0x001D7FDC File Offset: 0x001D61DC
		public void OnDirectoryDeleted(string path)
		{
			List<AssetFile> list;
			bool flag = !this.Assets.TryRemoveDirectory(path, out list);
			if (!flag)
			{
				foreach (AssetFile assetFile in list)
				{
					bool isDirectory = assetFile.IsDirectory;
					if (!isDirectory)
					{
						this.CloseTab(new AssetReference(assetFile.AssetType, assetFile.Path));
					}
				}
				this.UpdateAssetTreeForPath(path, true);
			}
		}

		// Token: 0x06005D24 RID: 23844 RVA: 0x001D8070 File Offset: 0x001D6270
		public void OnDiagnosticsUpdated(Dictionary<string, AssetDiagnostics> diagnostics)
		{
			bool flag = false;
			foreach (KeyValuePair<string, AssetDiagnostics> keyValuePair in diagnostics)
			{
				AssetDiagnostics assetDiagnostics;
				bool flag2 = this.Diagnostics.TryGetValue(keyValuePair.Key, out assetDiagnostics);
				if (flag2)
				{
					this._errorCount -= assetDiagnostics.Errors.Length;
					this._warningCount -= assetDiagnostics.Warnings.Length;
				}
				bool flag3 = keyValuePair.Value.Errors != null;
				if (flag3)
				{
					this._errorCount += keyValuePair.Value.Errors.Length;
				}
				bool flag4 = keyValuePair.Value.Warnings != null;
				if (flag4)
				{
					this._warningCount += keyValuePair.Value.Warnings.Length;
				}
				bool flag5 = keyValuePair.Value.Errors != null || keyValuePair.Value.Warnings != null;
				if (flag5)
				{
					this.Diagnostics[keyValuePair.Key] = keyValuePair.Value;
				}
				else
				{
					this.Diagnostics.Remove(keyValuePair.Key);
				}
				bool flag6 = keyValuePair.Key == this.CurrentAsset.FilePath;
				if (flag6)
				{
					flag = true;
				}
			}
			this.UpdateDiagnostics();
			this._errorsInfo.Parent.Layout(null, true);
			bool isMounted = this._diagnosticsPane.IsMounted;
			if (isMounted)
			{
				this._diagnosticsPane.Layout(null, true);
			}
			bool flag7 = flag && this.ConfigEditor.IsMounted;
			if (flag7)
			{
				this.ConfigEditor.SetupDiagnostics(true, true);
			}
		}

		// Token: 0x06005D25 RID: 23845 RVA: 0x001D8260 File Offset: 0x001D6460
		public void SetupAssetTypes(IReadOnlyDictionary<string, SchemaNode> schemas, IReadOnlyDictionary<string, AssetTypeConfig> assetTypes)
		{
			this._schemas = schemas;
			this._areAssetTypesAndSchemasInitialized = true;
			this.AssetTypeRegistry.SetupAssetTypes(assetTypes);
			this.FilterModal.Setup();
		}

		// Token: 0x06005D26 RID: 23846 RVA: 0x001D828C File Offset: 0x001D648C
		public void SetupAssetFiles(List<AssetFile> serverAssetFiles, List<AssetFile> commonAssetFiles, List<AssetFile> cosmeticAssetFiles)
		{
			this.Assets.SetupAssets(serverAssetFiles, commonAssetFiles, cosmeticAssetFiles);
			this._serverAssetTree.UpdateFiles(serverAssetFiles, null);
			this._commonAssetTree.UpdateFiles(commonAssetFiles, null);
			this._cosmeticsAssetTree.UpdateFiles(cosmeticAssetFiles, null);
			this.SetAssetTreeInitializing(false);
			bool flag = this.CurrentAsset.Equals(AssetReference.None);
			if (flag)
			{
				bool flag2 = this._assetToOpenOnceAssetFilesInitialized != null;
				if (flag2)
				{
					bool flag3 = this._assetToOpenOnceAssetFilesInitialized.FilePath != null;
					if (flag3)
					{
						this.OpenExistingAsset(this._assetToOpenOnceAssetFilesInitialized.FilePath, false);
					}
					else
					{
						bool flag4 = this._assetToOpenOnceAssetFilesInitialized.Id.Id != null;
						if (flag4)
						{
							this.OpenExistingAssetById(this._assetToOpenOnceAssetFilesInitialized.Id, false);
						}
					}
					this._assetToOpenOnceAssetFilesInitialized = null;
				}
			}
			else
			{
				this.SelectAssetTreeEntry(this.CurrentAsset, true, true);
			}
		}

		// Token: 0x06005D27 RID: 23847 RVA: 0x001D8374 File Offset: 0x001D6574
		public string GetAssetIdFromReference(AssetReference assetReference)
		{
			return (this.AssetTypeRegistry.AssetTypes[assetReference.Type].AssetTree == AssetTreeFolder.Cosmetics) ? Enumerable.Last<string>(assetReference.FilePath.Split(new char[]
			{
				'#'
			})) : Path.GetFileNameWithoutExtension(assetReference.FilePath);
		}

		// Token: 0x06005D28 RID: 23848 RVA: 0x001D83CC File Offset: 0x001D65CC
		public List<FileSelector.File> GetCommonFileSelectorFiles(string path, string searchQuery, string[] fileExtensionFilter, string[] directoryFilter, int limit)
		{
			List<FileSelector.File> list = new List<FileSelector.File>();
			bool flag = searchQuery != "";
			if (flag)
			{
				string[] array = Enumerable.ToArray<string>(Enumerable.Select<string, string>(searchQuery.ToLower().Split(new char[]
				{
					' '
				}, StringSplitOptions.RemoveEmptyEntries), (string q) => q.Trim()));
				List<AssetFile> assets = this.Assets.GetAssets(AssetTreeFolder.Common);
				foreach (AssetFile assetFile in assets)
				{
					bool flag2 = list.Count >= limit;
					if (flag2)
					{
						break;
					}
					string path2 = assetFile.Path;
					bool flag3 = true;
					string text = path2.ToLowerInvariant();
					foreach (string value in array)
					{
						bool flag4 = !text.Contains(value);
						if (flag4)
						{
							flag3 = false;
							break;
						}
					}
					bool flag5 = !flag3;
					if (!flag5)
					{
						bool flag6 = !assetFile.IsDirectory && fileExtensionFilter != null && !AssetPathUtils.HasAnyFileExtension(path2, fileExtensionFilter);
						if (!flag6)
						{
							string text2 = assetFile.Path.Substring("Common/".Length);
							bool flag7 = directoryFilter != null;
							if (flag7)
							{
								bool flag8 = !AssetPathUtils.IsAnyDirectory("/" + text2, directoryFilter);
								if (flag8)
								{
									continue;
								}
							}
							list.Add(new FileSelector.File
							{
								Name = text2,
								IsDirectory = assetFile.IsDirectory
							});
						}
					}
				}
			}
			else
			{
				string[] array3 = path.Split(new char[]
				{
					'/'
				}, StringSplitOptions.RemoveEmptyEntries);
				List<AssetFile> assets2 = this.Assets.GetAssets(AssetTreeFolder.Common);
				bool flag9 = path == "Common/";
				foreach (AssetFile assetFile2 in assets2)
				{
					bool flag10 = !flag9;
					if (flag10)
					{
						bool flag11 = assetFile2.Path == path;
						if (flag11)
						{
							flag9 = true;
						}
					}
					else
					{
						bool flag12 = assetFile2.PathElements.Length <= array3.Length;
						if (flag12)
						{
							break;
						}
						bool flag13 = array3.Length + 1 != assetFile2.PathElements.Length;
						if (!flag13)
						{
							string text3 = Enumerable.Last<string>(assetFile2.PathElements);
							bool flag14 = !assetFile2.IsDirectory && fileExtensionFilter != null && !AssetPathUtils.HasAnyFileExtension(text3, fileExtensionFilter);
							if (!flag14)
							{
								list.Add(new FileSelector.File
								{
									Name = text3,
									IsDirectory = assetFile2.IsDirectory
								});
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06005D29 RID: 23849 RVA: 0x001D86CC File Offset: 0x001D68CC
		public bool ValidateAssetId(string id, out string errorMessage)
		{
			id = id.Trim();
			bool flag = id == "";
			bool result;
			if (flag)
			{
				errorMessage = this.Desktop.Provider.GetText("ui.assetEditor.idValidation.empty", null, true);
				result = false;
			}
			else
			{
				bool flag2 = id.Length < 3;
				if (flag2)
				{
					errorMessage = this.Desktop.Provider.GetText("ui.assetEditor.idValidation.minLength", new Dictionary<string, string>
					{
						{
							"count",
							"3"
						}
					}, true);
					result = false;
				}
				else
				{
					bool flag3 = id.Length > 64;
					if (flag3)
					{
						errorMessage = this.Desktop.Provider.GetText("ui.assetEditor.idValidation.maxLength", new Dictionary<string, string>
						{
							{
								"count",
								"64"
							}
						}, true);
						result = false;
					}
					else
					{
						string[] array = id.Split(new char[]
						{
							'_'
						});
						foreach (string text in array)
						{
							bool flag4 = text == "";
							if (flag4)
							{
								errorMessage = this.Desktop.Provider.GetText("ui.assetEditor.idValidation.underscoreEmpty", null, true);
								return false;
							}
							bool flag5 = !char.IsLetter(text[0]);
							if (flag5)
							{
								errorMessage = this.Desktop.Provider.GetText("ui.assetEditor.idValidation.firstLetter", null, true);
								return false;
							}
							bool flag6 = !char.IsUpper(text[0]);
							if (flag6)
							{
								errorMessage = this.Desktop.Provider.GetText("ui.assetEditor.idValidation.firstUppercase", null, true);
								return false;
							}
							foreach (char c in text)
							{
								bool flag7 = !char.IsDigit(c) && !char.IsLetter(c);
								if (flag7)
								{
									errorMessage = this.Desktop.Provider.GetText("ui.assetEditor.idValidation.onlyLettersAndDigits", null, true);
									return false;
								}
							}
						}
						errorMessage = null;
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x170013BF RID: 5055
		// (get) Token: 0x06005D2A RID: 23850 RVA: 0x001D88DB File Offset: 0x001D6ADB
		// (set) Token: 0x06005D2B RID: 23851 RVA: 0x001D88E3 File Offset: 0x001D6AE3
		public ToastNotifications ToastNotifications { get; private set; }

		// Token: 0x170013C0 RID: 5056
		// (get) Token: 0x06005D2C RID: 23852 RVA: 0x001D88EC File Offset: 0x001D6AEC
		// (set) Token: 0x06005D2D RID: 23853 RVA: 0x001D88F4 File Offset: 0x001D6AF4
		public AssetEditorOverlay.EditorMode Mode { get; private set; } = AssetEditorOverlay.EditorMode.Editor;

		// Token: 0x170013C1 RID: 5057
		// (get) Token: 0x06005D2E RID: 23854 RVA: 0x001D88FD File Offset: 0x001D6AFD
		// (set) Token: 0x06005D2F RID: 23855 RVA: 0x001D8905 File Offset: 0x001D6B05
		public bool IsBackendInitialized { get; private set; }

		// Token: 0x170013C2 RID: 5058
		// (get) Token: 0x06005D30 RID: 23856 RVA: 0x001D890E File Offset: 0x001D6B0E
		public AssetEditorBackend Backend
		{
			get
			{
				return this.Interface.App.Editor.Backend;
			}
		}

		// Token: 0x06005D31 RID: 23857 RVA: 0x001D8928 File Offset: 0x001D6B28
		public AssetEditorOverlay(AssetEditorInterface @interface, Desktop desktop) : base(desktop, null)
		{
			this.AssetTypeRegistry = new AssetTypeRegistry();
			this.Assets = new AssetList(this.AssetTypeRegistry);
			this.Interface = @interface;
			this.ConfirmationModal = new ConfirmationModal(desktop, null);
			this.Popup = new PopupMenuLayer(this.Desktop, null);
			this.RenameModal = new RenameModal(this);
			this.ConfigEditor = new ConfigEditor(this);
			this.CreateAssetModal = new CreateAssetModal(this);
			this.ChangelogModal = new ChangelogModal(this);
			this.ConfigEditorContextPane = new ConfigEditorContextPane(this);
			this._sourceEditor = new SourceEditor(this);
			this.ExportModal = new ExportModal(this);
			this.FilterModal = new FilterModal(this);
			this.IconMassExportModal = new IconMassExportModal(this);
			this.WeatherDaytimeBar = new WeatherDaytimeBar(this);
			this.AutoCompleteMenu = new AutoCompleteMenu(this.Desktop);
			this.TextTooltipLayer = new TextTooltipLayer(this.Desktop)
			{
				ShowDelay = 0.25f
			};
			this.DropdownAssetTree = new AssetTree(this, "", null)
			{
				PopupMenuEnabled = false,
				ShowVirtualAssets = true
			};
			this.DropdownAssetTree.ScrollbarStyle.Size = 5;
			this._cosmeticsAssetTree = new AssetTree(this, "Cosmetics/CharacterCreator", null)
			{
				FocusSearch = new Action(this.FocusSearch),
				FileEntryActivating = delegate(AssetTree.AssetTreeEntry entry)
				{
					this.OpenExistingAsset(new AssetReference(entry.AssetType, entry.Path), false);
				},
				SelectingDirectoryFilter = new Action<string>(this.SetupSearchFilter),
				CollapseStateChanged = new Action<string, bool>(this.OnAssetTreeCollapseStateChanged)
			};
			this._commonAssetTree = new AssetTree(this, "Common", null)
			{
				FocusSearch = new Action(this.FocusSearch),
				FileEntryActivating = delegate(AssetTree.AssetTreeEntry entry)
				{
					this.OpenExistingAsset(new AssetReference(entry.AssetType, entry.Path), false);
				},
				SelectingDirectoryFilter = new Action<string>(this.SetupSearchFilter),
				CollapseStateChanged = new Action<string, bool>(this.OnAssetTreeCollapseStateChanged),
				PopupMenuEnabled = false
			};
			this._serverAssetTree = new AssetTree(this, "Server", null)
			{
				FocusSearch = new Action(this.FocusSearch),
				FileEntryActivating = delegate(AssetTree.AssetTreeEntry entry)
				{
					this.OpenExistingAsset(new AssetReference(entry.AssetType, entry.Path), false);
				},
				SelectingDirectoryFilter = new Action<string>(this.SetupSearchFilter),
				CollapseStateChanged = new Action<string, bool>(this.OnAssetTreeCollapseStateChanged)
			};
		}

		// Token: 0x06005D32 RID: 23858 RVA: 0x001D8BC8 File Offset: 0x001D6DC8
		public void Build()
		{
			AssetEditorSettings settings = this.Interface.App.Settings;
			this._fileSaveStatus = AssetEditorOverlay.SaveStatus.Disabled;
			Element parent = this.AutoCompleteMenu.Parent;
			if (parent != null)
			{
				parent.Remove(this.AutoCompleteMenu);
			}
			Element parent2 = this.WeatherDaytimeBar.Parent;
			if (parent2 != null)
			{
				parent2.Remove(this.WeatherDaytimeBar);
			}
			base.Clear();
			Group editorPane = this._editorPane;
			if (editorPane != null)
			{
				editorPane.Clear();
			}
			DynamicPane contextPane = this._contextPane;
			if (contextPane != null)
			{
				contextPane.Clear();
			}
			Element parent3 = this._serverAssetTree.Parent;
			if (parent3 != null)
			{
				parent3.Remove(this._serverAssetTree);
			}
			Element parent4 = this._commonAssetTree.Parent;
			if (parent4 != null)
			{
				parent4.Remove(this._commonAssetTree);
			}
			Element parent5 = this._cosmeticsAssetTree.Parent;
			if (parent5 != null)
			{
				parent5.Remove(this._cosmeticsAssetTree);
			}
			Document document;
			this.Desktop.Provider.TryGetDocument("AssetEditor/AssetEditorOverlay.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this.TextTooltipLayer.Style = document.ResolveNamedValue<TextTooltipStyle>(this.Desktop.Provider, "TooltipStyle");
			TextTooltipStyle tooltipStyle = document.ResolveNamedValue<TextTooltipStyle>(this.Desktop.Provider, "AssetTreeTooltipStyle");
			this._serverAssetTree.TooltipStyle = tooltipStyle;
			this._commonAssetTree.TooltipStyle = tooltipStyle;
			this._cosmeticsAssetTree.TooltipStyle = tooltipStyle;
			this._rootContainer = uifragment.Get<Group>("RootContainer");
			this._contentPane = uifragment.Get<DynamicPane>("ContentPane");
			this._editorPane = uifragment.Get<Group>("EditorPane");
			this._contextPane = uifragment.Get<DynamicPane>("ContextPane");
			this._contextPane.Visible = false;
			this._contextPane.Anchor.Width = new int?(settings.PaneSizes[AssetEditorSettings.Panes.ConfigEditorSidebar]);
			this._contextPane.MouseButtonReleased = delegate()
			{
				this.UpdatePaneSize(AssetEditorSettings.Panes.ConfigEditorSidebar, this._contextPane.Anchor.Width.Value);
			};
			TextButton textButton = uifragment.Get<TextButton>("ChangelogButton");
			textButton.Activating = delegate()
			{
				this.Desktop.SetLayer(4, this.ChangelogModal);
			};
			textButton.Text = "v0.1.0";
			uifragment.Get<Button>("OptionsButton").Activating = new Action(this.OnActivateOptionsButton);
			this._assetsSourceButton = uifragment.Get<TextButton>("AssetsSourceButton");
			this._assetsSourceButton.Activating = delegate()
			{
				bool flag4 = !OpenUtils.TryOpenDirectoryInContainingDirectory(this.Interface.App.Settings.AssetsPath, this.Interface.App.Settings.AssetsPath);
				if (flag4)
				{
					AssetEditorOverlay.Logger.Warn("Failed to open folder {0}", this.Interface.App.Settings.AssetsPath);
				}
			};
			this.UpdateAssetsSourceButton();
			uifragment.Get<TextButton>("CloseButton").Activating = new Action(this.TryClose);
			uifragment.Get<TextButton>("NewAssetButton").Activating = new Action(this.OpenCreateAssetModal);
			uifragment.Get<Button>("CollapseAllButton").Activating = new Action(this.CollapseAllDirectoriesInAssetTree);
			uifragment.Get<Button>("FilterButton").Activating = delegate()
			{
				this.FilterModal.Open();
			};
			List<Element> list = (this._tabs != null) ? new List<Element>(this._tabs.Children) : null;
			Group tabs = this._tabs;
			if (tabs != null)
			{
				tabs.Clear();
			}
			this._tabs = uifragment.Get<Group>("Tabs");
			bool flag = list != null;
			if (flag)
			{
				foreach (Element element in list)
				{
					((EditorTabButton)element).Build();
					this._tabs.Add(element, -1);
				}
			}
			this._modifiedAssetsCountLabel = uifragment.Get<Label>("ModifiedAssetsCountLabel");
			this._modifiedAssetsCountLabel.Text = this.Desktop.Provider.FormatNumber(this._modifiedAssetsCount);
			this._modifiedAssetsCountLabel.Visible = (this._modifiedAssetsCount > 0);
			this._modeSelection = uifragment.Get<TabNavigation>("Mode");
			this._modeSelection.SelectedTab = this.Mode.ToString();
			this._modeSelection.SelectedTabChanged = delegate()
			{
				this.OnChangeMode((AssetEditorOverlay.EditorMode)Enum.Parse(typeof(AssetEditorOverlay.EditorMode), this._modeSelection.SelectedTab));
			};
			this._assetTreeSelection = uifragment.Get<TabNavigation>("RootFolderSelection");
			this._assetTreeSelection.SelectedTabChanged = delegate()
			{
				this.OnAssetTreeSelectionChanged((AssetTreeFolder)Enum.Parse(typeof(AssetTreeFolder), this._assetTreeSelection.SelectedTab));
			};
			this._footer = uifragment.Get<Group>("Footer");
			this._fileSaveStatusGroup = uifragment.Get<Group>("FileSaveStatus");
			this._fileSaveStatusGroup.Visible = false;
			this._validatedInfo = uifragment.Get<Button>("ValidatedInfo");
			this._validatedInfo.Activating = new Action(this.ToggleDiagnosticsPane);
			this._errorsInfo = uifragment.Get<Button>("ErrorsInfo");
			this._errorsInfo.Activating = new Action(this.ToggleDiagnosticsPane);
			this._warningsInfo = uifragment.Get<Button>("WarningsInfo");
			this._warningsInfo.Activating = new Action(this.ToggleDiagnosticsPane);
			this._diagnosticsPane = uifragment.Get<DynamicPane>("DiagnosticsPane");
			this._diagnosticsPane.Visible = this._isDiagnosticsPaneOpen;
			this._diagnosticsPane.Anchor.Height = new int?(settings.PaneSizes[AssetEditorSettings.Panes.Diagnostics]);
			this._diagnosticsPane.MouseButtonReleased = delegate()
			{
				this.UpdatePaneSize(AssetEditorSettings.Panes.Diagnostics, this._diagnosticsPane.Anchor.Height.Value);
			};
			this.UpdateDiagnostics();
			this._assetBrowserSearchInput = uifragment.Get<TextField>("SearchInput");
			this._assetBrowserSearchInput.ValueChanged = delegate()
			{
				this.UpdateAssetTreeSearch(true);
			};
			this._assetBrowserSearchInput.KeyDown = delegate(SDL.SDL_Keycode key)
			{
				bool flag4 = key != SDL.SDL_Keycode.SDLK_DOWN && key != SDL.SDL_Keycode.SDLK_UP;
				if (!flag4)
				{
					AssetTree assetTree = this.GetAssetTree(this.Interface.App.Settings.ActiveAssetTree);
					this.Desktop.FocusElement(assetTree, true);
					assetTree.OnKeyDown(key, 0);
				}
			};
			this._assetBrowser = uifragment.Get<DynamicPane>("AssetBrowserPane");
			this._assetBrowser.Anchor.Width = new int?(settings.PaneSizes[AssetEditorSettings.Panes.AssetBrowser]);
			this._assetBrowser.MouseButtonReleased = delegate()
			{
				this.UpdatePaneSize(AssetEditorSettings.Panes.AssetBrowser, this._assetBrowser.Anchor.Width.Value);
			};
			this._assetBrowserSpinner = uifragment.Get<Group>("AssetBrowserSpinner");
			this._exportButton = uifragment.Get<Button>("ExportButton");
			this._exportButton.Activating = delegate()
			{
				this.ExportModal.Open();
			};
			this._assetPathWarning = uifragment.Get<Group>("AssetPathWarning");
			this._assetPathWarning.Visible = settings.DisplayDefaultAssetPathWarning;
			uifragment.Get<TextButton>("AssetPathWarningOpenSettings").Activating = delegate()
			{
				this.Interface.SettingsModal.Open();
			};
			uifragment.Get<TextButton>("AssetPathWarningDismiss").Activating = delegate()
			{
				AssetEditorSettings assetEditorSettings = this.Interface.App.Settings.Clone();
				assetEditorSettings.DisplayDefaultAssetPathWarning = false;
				this.Interface.App.ApplySettings(assetEditorSettings);
			};
			this._assetBrowser.Add(this._serverAssetTree, -1);
			this._assetBrowser.Add(this._commonAssetTree, -1);
			this._assetBrowser.Add(this._cosmeticsAssetTree, -1);
			this.Popup.Style = document.ResolveNamedValue<PopupMenuLayerStyle>(this.Desktop.Provider, "PopupMenuLayerStyle");
			this.ConfigEditor.Build();
			this.ConfirmationModal.Build();
			this.RenameModal.Build();
			this.CreateAssetModal.Build();
			this.ChangelogModal.Build();
			this.AutoCompleteMenu.Build();
			this.ConfigEditorContextPane.Build();
			this.ExportModal.Build();
			this.FilterModal.Build();
			this.IconMassExportModal.Build();
			this._sourceEditor.Build();
			this.WeatherDaytimeBar.Build();
			this.ConfigEditorContextPane.DayTimeControls.Build();
			this.ToastNotifications = new ToastNotifications(this.Desktop, this);
			Document document2;
			this.Interface.TryGetDocument("AssetEditor/AssetLoadingIndicator.ui", out document2);
			this._assetLoadingIndicator = document2.Instantiate(this.Desktop, null).RootElements[0];
			this.UpdateAssetTreeUncollapsedStateFromSettings();
			bool flag2 = this.Backend != null;
			if (flag2)
			{
				this._exportButton.Visible = this.Backend.IsEditingRemotely;
				this.BuildAssetTreeTabs();
			}
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				bool flag3 = !this.IsBackendInitialized;
				if (flag3)
				{
					this.InitializeBackend();
				}
				this.FinishWork();
				this.SetupEditorPane();
			}
		}

		// Token: 0x06005D33 RID: 23859 RVA: 0x001D93D4 File Offset: 0x001D75D4
		public void UpdateAssetPathWarning(bool isDefault)
		{
			this._assetPathWarning.Visible = isDefault;
			base.Layout(null, true);
		}

		// Token: 0x06005D34 RID: 23860 RVA: 0x001D9400 File Offset: 0x001D7600
		private void UpdateAssetsSourceButton()
		{
			AssetEditorBackend backend = this.Backend;
			AssetEditorBackend assetEditorBackend = backend;
			if (!(assetEditorBackend is LocalAssetEditorBackend))
			{
				if (!(assetEditorBackend is ServerAssetEditorBackend))
				{
					this._assetsSourceButton.Text = "???";
				}
				else
				{
					this._assetsSourceButton.Text = this.Desktop.Provider.GetText("ui.assetEditor.backends.server", null, true);
				}
			}
			else
			{
				this._assetsSourceButton.Text = this.Desktop.Provider.GetText("ui.assetEditor.backends.local", null, true);
			}
		}

		// Token: 0x06005D35 RID: 23861 RVA: 0x001D9488 File Offset: 0x001D7688
		private void BuildAssetTreeTabs()
		{
			List<TabNavigation.Tab> list = new List<TabNavigation.Tab>();
			AssetTreeFolder[] supportedAssetTreeFolders = this.Backend.SupportedAssetTreeFolders;
			foreach (AssetTreeFolder assetTreeFolder in supportedAssetTreeFolders)
			{
				list.Add(new TabNavigation.Tab
				{
					Id = assetTreeFolder.ToString(),
					Text = this.Desktop.Provider.GetText("ui.assetEditor.assetBrowser.tabs." + assetTreeFolder.ToString().ToLowerInvariant(), null, true)
				});
			}
			this._assetTreeSelection.Tabs = list.ToArray();
			AssetEditorApp app = this.Interface.App;
			AssetEditorSettings assetEditorSettings = app.Settings;
			bool flag = !Enumerable.Contains<AssetTreeFolder>(supportedAssetTreeFolders, assetEditorSettings.ActiveAssetTree);
			if (flag)
			{
				assetEditorSettings = assetEditorSettings.Clone();
				assetEditorSettings.ActiveAssetTree = supportedAssetTreeFolders[0];
				app.ApplySettings(assetEditorSettings);
				this.UpdateActiveAssetTree(false);
			}
		}

		// Token: 0x06005D36 RID: 23862 RVA: 0x001D9578 File Offset: 0x001D7778
		protected override void OnMounted()
		{
			bool flag = !this._sourceEditor.CodeEditor.IsInitialized;
			if (flag)
			{
				this._sourceEditor.CodeEditor.InitEditor();
			}
			bool flag2 = !this.CreateAssetModal.CodeEditor.IsInitialized;
			if (flag2)
			{
				this.CreateAssetModal.CodeEditor.InitEditor();
			}
			bool hasMarkupError = this.Interface.HasMarkupError;
			if (!hasMarkupError)
			{
				bool flag3 = !this.IsBackendInitialized;
				if (flag3)
				{
					this.InitializeBackend();
				}
				this.Backend.OnEditorOpen(true);
				bool flag4 = !this.CurrentAsset.Equals(AssetReference.None);
				if (flag4)
				{
					this.Backend.SetOpenEditorAsset(this.CurrentAsset);
					this.FetchOpenAsset();
				}
				this.OpenChangelogModalIfNewVersion();
			}
		}

		// Token: 0x06005D37 RID: 23863 RVA: 0x001D9649 File Offset: 0x001D7849
		protected override void OnUnmounted()
		{
			CancellationTokenSource currentAssetCancellationToken = this._currentAssetCancellationToken;
			if (currentAssetCancellationToken != null)
			{
				currentAssetCancellationToken.Cancel();
			}
		}

		// Token: 0x06005D38 RID: 23864 RVA: 0x001D965E File Offset: 0x001D785E
		private void InitializeBackend()
		{
			this.UpdateActiveAssetTree(false);
			this.UpdateAssetTreeUncollapsedStateFromSettings();
			this.SetAssetTreeInitializing(true);
			Debug.Assert(!this.IsBackendInitialized);
			this.IsBackendInitialized = true;
			this.Backend.Initialize();
		}

		// Token: 0x06005D39 RID: 23865 RVA: 0x001D969C File Offset: 0x001D789C
		public void SetupBackend(AssetEditorBackend backend)
		{
			this.IsBackendInitialized = false;
			bool flag = this._rootContainer != null;
			if (flag)
			{
				this._exportButton.Visible = (backend != null && backend.IsEditingRemotely);
				this.BuildAssetTreeTabs();
				this.UpdateAssetsSourceButton();
			}
		}

		// Token: 0x06005D3A RID: 23866 RVA: 0x001D96E8 File Offset: 0x001D78E8
		public void FinishWork()
		{
			bool isMounted = this.ConfigEditor.IsMounted;
			if (isMounted)
			{
				this.ConfigEditor.SubmitPendingUpdateCommands();
			}
		}

		// Token: 0x06005D3B RID: 23867 RVA: 0x001D9714 File Offset: 0x001D7914
		public void Reset()
		{
			CancellationTokenSource currentAssetCancellationToken = this._currentAssetCancellationToken;
			if (currentAssetCancellationToken != null)
			{
				currentAssetCancellationToken.Cancel();
			}
			this._serverAssetTree.UpdateFiles(new List<AssetFile>(), null);
			this._serverAssetTree.DeselectEntry();
			this._serverAssetTree.DirectoriesToDisplay = null;
			this._serverAssetTree.AssetTypesToDisplay = null;
			this._commonAssetTree.UpdateFiles(new List<AssetFile>(), null);
			this._commonAssetTree.DeselectEntry();
			this._commonAssetTree.DirectoriesToDisplay = null;
			this._commonAssetTree.AssetTypesToDisplay = null;
			this._cosmeticsAssetTree.UpdateFiles(new List<AssetFile>(), null);
			this._cosmeticsAssetTree.DeselectEntry();
			this._cosmeticsAssetTree.DirectoriesToDisplay = null;
			this._cosmeticsAssetTree.AssetTypesToDisplay = null;
			this.DisplayedCommonAssetTypes.Clear();
			this.DisplayedServerAssetTypes.Clear();
			this.DisplayedCosmeticAssetTypes.Clear();
			this._modifiedAssetsCount = 0;
			this._editorPane.Clear();
			this._contextPane.Clear();
			this._modifiedAssetsCountLabel.Visible = false;
			this._assetBrowserSearchInput.Value = "";
			this._tabs.Clear();
			this.WeatherDaytimeBar.ResetState();
			this.ConfigEditorContextPane.ResetState();
			this.ExportModal.ResetState();
			this.FilterModal.ResetState();
			this.Mode = AssetEditorOverlay.EditorMode.Editor;
			this._modeSelection.SelectedTab = this.Mode.ToString();
			this.IsBackendInitialized = false;
			this._areAssetFilesInitialized = false;
			this._areAssetTypesAndSchemasInitialized = false;
			this.SetFileSaveStatus(AssetEditorOverlay.SaveStatus.Disabled, false);
			this.CurrentAsset = AssetReference.None;
			this.TrackedAssets.Clear();
			this._assetToOpenOnceAssetFilesInitialized = null;
			this.AssetTypeRegistry.Clear();
			this.Diagnostics = new Dictionary<string, AssetDiagnostics>();
			this._schemas = new Dictionary<string, SchemaNode>();
			this.ConfigEditor.Reset();
		}

		// Token: 0x06005D3C RID: 23868 RVA: 0x001D9908 File Offset: 0x001D7B08
		public void CleanupWebViews()
		{
			bool isInitialized = this._sourceEditor.CodeEditor.IsInitialized;
			if (isInitialized)
			{
				this._sourceEditor.CodeEditor.DisposeEditor();
			}
			bool isInitialized2 = this.CreateAssetModal.CodeEditor.IsInitialized;
			if (isInitialized2)
			{
				this.CreateAssetModal.CodeEditor.DisposeEditor();
			}
		}

		// Token: 0x06005D3D RID: 23869 RVA: 0x001D9960 File Offset: 0x001D7B60
		public void OnWindowFocusChanged()
		{
			bool flag = !this.Interface.Engine.Window.IsFocused;
			if (flag)
			{
				this.SaveAll();
			}
		}

		// Token: 0x06005D3E RID: 23870 RVA: 0x001D9994 File Offset: 0x001D7B94
		private void OnActivateOptionsButton()
		{
			List<PopupMenuItem> items = new List<PopupMenuItem>
			{
				new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.options.settings", null, true), delegate()
				{
					this.Interface.SettingsModal.Open();
				}, null, null),
				new PopupMenuItem(this.Desktop.Provider.GetText(this.ConfigEditor.DisplayUnsetProperties ? "ui.assetEditor.options.displayUnsetProperties.on" : "ui.assetEditor.options.displayUnsetProperties.off", null, true), delegate()
				{
					this.ConfigEditor.ToggleDisplayUnsetProperties();
				}, null, null)
			};
			this.Popup.SetTitle(this.Desktop.Provider.GetText("ui.assetEditor.options.title", null, true));
			this.Popup.SetItems(items);
			this.Popup.Open();
		}

		// Token: 0x06005D3F RID: 23871 RVA: 0x001D9A5C File Offset: 0x001D7C5C
		private bool CanEditCurrentAsset()
		{
			TrackedAsset trackedAsset;
			return this.CurrentAsset.FilePath != null && this.TrackedAssets.TryGetValue(this.CurrentAsset.FilePath, out trackedAsset) && !trackedAsset.IsLoading && trackedAsset.FetchError == null;
		}

		// Token: 0x06005D40 RID: 23872 RVA: 0x001D9AA4 File Offset: 0x001D7CA4
		public void SetModifiedAssetsCount(int count)
		{
			bool flag = this._modifiedAssetsCount == count;
			if (!flag)
			{
				this._modifiedAssetsCount = count;
				this._modifiedAssetsCountLabel.Text = this.Desktop.Provider.FormatNumber(count);
				this._modifiedAssetsCountLabel.Visible = (count > 0);
				bool isMounted = this._modifiedAssetsCountLabel.IsMounted;
				if (isMounted)
				{
					this._modifiedAssetsCountLabel.Parent.Layout(null, true);
				}
			}
		}

		// Token: 0x06005D41 RID: 23873 RVA: 0x001D9B20 File Offset: 0x001D7D20
		public HashSet<string> GetDisplayedAssetTypes()
		{
			HashSet<string> result;
			switch (this.Interface.App.Settings.ActiveAssetTree)
			{
			case AssetTreeFolder.Server:
				result = this.DisplayedServerAssetTypes;
				break;
			case AssetTreeFolder.Common:
				result = this.DisplayedCommonAssetTypes;
				break;
			case AssetTreeFolder.Cosmetics:
				result = this.DisplayedCosmeticAssetTypes;
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		// Token: 0x06005D42 RID: 23874 RVA: 0x001D9B7C File Offset: 0x001D7D7C
		public void OnDisplayedAssetTypesChanged()
		{
			AssetTreeFolder activeAssetTree = this.Interface.App.Settings.ActiveAssetTree;
			AssetTree assetTree = this.GetAssetTree(activeAssetTree);
			HashSet<string> displayedAssetTypes = this.GetDisplayedAssetTypes();
			bool flag = displayedAssetTypes.Count == 0;
			if (flag)
			{
				assetTree.DirectoriesToDisplay = null;
				assetTree.AssetTypesToDisplay = null;
			}
			else
			{
				assetTree.DirectoriesToDisplay = new List<string>();
				assetTree.AssetTypesToDisplay = new HashSet<string>();
				foreach (string text in this.GetDisplayedAssetTypes())
				{
					bool flag2 = activeAssetTree != AssetTreeFolder.Common;
					if (flag2)
					{
						assetTree.DirectoriesToDisplay.Add(this.AssetTypeRegistry.AssetTypes[text].Path);
					}
					assetTree.AssetTypesToDisplay.Add(text);
				}
			}
			assetTree.BuildTree();
			assetTree.Layout(null, true);
		}

		// Token: 0x06005D43 RID: 23875 RVA: 0x001D9C88 File Offset: 0x001D7E88
		private void OnAssetTreeSelectionChanged(AssetTreeFolder assetTree)
		{
			AssetEditorApp app = this.Interface.App;
			AssetEditorSettings assetEditorSettings = app.Settings;
			assetEditorSettings = assetEditorSettings.Clone();
			assetEditorSettings.ActiveAssetTree = assetTree;
			app.ApplySettings(assetEditorSettings);
			this.UpdateActiveAssetTree(true);
		}

		// Token: 0x06005D44 RID: 23876 RVA: 0x001D9CC8 File Offset: 0x001D7EC8
		private void UpdateActiveAssetTree(bool doLayout)
		{
			AssetEditorSettings settings = this.Interface.App.Settings;
			this._assetTreeSelection.SelectedTab = settings.ActiveAssetTree.ToString();
			this._cosmeticsAssetTree.Visible = (settings.ActiveAssetTree == AssetTreeFolder.Cosmetics);
			this._commonAssetTree.Visible = (settings.ActiveAssetTree == AssetTreeFolder.Common);
			this._serverAssetTree.Visible = (settings.ActiveAssetTree == AssetTreeFolder.Server);
			this.UpdateAssetTreeSearch(false);
			if (doLayout)
			{
				this._assetBrowser.Layout(null, true);
			}
		}

		// Token: 0x06005D45 RID: 23877 RVA: 0x001D9D68 File Offset: 0x001D7F68
		private void UpdateAssetTreeSearch(bool doLayout = true)
		{
			AssetTree assetTree = this.GetAssetTree(this.Interface.App.Settings.ActiveAssetTree);
			bool flag = assetTree.SearchQuery == this._assetBrowserSearchInput.Value;
			if (!flag)
			{
				assetTree.SearchQuery = this._assetBrowserSearchInput.Value;
				assetTree.BuildTree();
				if (doLayout)
				{
					assetTree.Layout(null, true);
				}
			}
		}

		// Token: 0x06005D46 RID: 23878 RVA: 0x001D9DDC File Offset: 0x001D7FDC
		private void CollapseAllDirectoriesInAssetTree()
		{
			AssetEditorApp app = this.Interface.App;
			AssetEditorSettings settings = app.Settings;
			settings.UncollapsedDirectories.Clear();
			app.SaveSettings();
			this._commonAssetTree.ClearCollapsedStates();
			this._commonAssetTree.BuildTree();
			this._serverAssetTree.ClearCollapsedStates();
			this._serverAssetTree.BuildTree();
			this._cosmeticsAssetTree.ClearCollapsedStates();
			this._cosmeticsAssetTree.BuildTree();
		}

		// Token: 0x06005D47 RID: 23879 RVA: 0x001D9E58 File Offset: 0x001D8058
		private void UpdateAssetTreeUncollapsedStateFromSettings()
		{
			this._commonAssetTree.ClearCollapsedStates();
			this._serverAssetTree.ClearCollapsedStates();
			this._cosmeticsAssetTree.ClearCollapsedStates();
			foreach (string text in this.Interface.App.Settings.UncollapsedDirectories)
			{
				bool flag = text.StartsWith("Common/");
				if (flag)
				{
					this._commonAssetTree.SetUncollapsedState(text, true, true);
				}
				else
				{
					bool flag2 = text.StartsWith("Server/");
					if (flag2)
					{
						this._serverAssetTree.SetUncollapsedState(text, true, true);
					}
					else
					{
						bool flag3 = text.StartsWith("Cosmetics/");
						if (flag3)
						{
							this._cosmeticsAssetTree.SetUncollapsedState(text, true, true);
						}
					}
				}
			}
		}

		// Token: 0x06005D48 RID: 23880 RVA: 0x001D9F44 File Offset: 0x001D8144
		private void OnChangeMode(AssetEditorOverlay.EditorMode mode)
		{
			bool flag = this.Mode == mode;
			if (!flag)
			{
				bool flag2 = this.CurrentAsset.FilePath != null && mode == AssetEditorOverlay.EditorMode.Editor && this.AssetTypeRegistry.AssetTypes[this.CurrentAsset.Type].Schema == null;
				if (flag2)
				{
					this._modeSelection.SelectedTab = this.Mode.ToString();
					this._modeSelection.Layout(null, true);
					this.ToastNotifications.AddNotification(2, this.Interface.GetText("ui.assetEditor.errors.assetTypeNotSetupForEditor", null, true));
				}
				else
				{
					this.FinishWork();
					if (mode != AssetEditorOverlay.EditorMode.Editor)
					{
						if (mode == AssetEditorOverlay.EditorMode.Source)
						{
							this.Mode = AssetEditorOverlay.EditorMode.Source;
							this.SetupEditorPane();
						}
					}
					else
					{
						bool flag3 = this.CheckHasUnsavedSourceEditorChanges(delegate
						{
							this.OnChangeMode(AssetEditorOverlay.EditorMode.Editor);
						});
						if (flag3)
						{
							this._modeSelection.SelectedTab = this.Mode.ToString();
							this._modeSelection.Layout(null, true);
						}
						else
						{
							this.Mode = AssetEditorOverlay.EditorMode.Editor;
							this.SetupEditorPane();
						}
					}
				}
			}
		}

		// Token: 0x06005D49 RID: 23881 RVA: 0x001DA088 File Offset: 0x001D8288
		public void SetAssetTreeInitializing(bool isInitializing)
		{
			this._areAssetFilesInitialized = !isInitializing;
			AssetEditorSettings settings = this.Interface.App.Settings;
			this._cosmeticsAssetTree.Visible = (settings.ActiveAssetTree == AssetTreeFolder.Cosmetics && !isInitializing);
			this._commonAssetTree.Visible = (settings.ActiveAssetTree == AssetTreeFolder.Common && !isInitializing);
			this._serverAssetTree.Visible = (settings.ActiveAssetTree == AssetTreeFolder.Server && !isInitializing);
			this._assetBrowserSpinner.Visible = isInitializing;
			this._assetBrowser.Layout(null, true);
		}

		// Token: 0x06005D4A RID: 23882 RVA: 0x001DA127 File Offset: 0x001D8327
		private void SetupSearchFilter(string filter)
		{
			this._assetBrowserSearchInput.Value = filter + ": ";
			this._assetBrowserSearchInput.ValueChanged();
			this.Desktop.FocusElement(this._assetBrowserSearchInput, true);
		}

		// Token: 0x06005D4B RID: 23883 RVA: 0x001DA168 File Offset: 0x001D8368
		private void OnAssetTreeCollapseStateChanged(string path, bool uncollapsed)
		{
			AssetEditorApp app = this.Interface.App;
			AssetEditorSettings settings = app.Settings;
			bool flag = !uncollapsed;
			if (flag)
			{
				settings.UncollapsedDirectories.Remove(path);
			}
			else
			{
				settings.UncollapsedDirectories.Add(path);
			}
			app.SaveSettings();
		}

		// Token: 0x06005D4C RID: 23884 RVA: 0x001DA1B4 File Offset: 0x001D83B4
		private void SelectAssetTreeEntry(AssetReference assetReference, bool bringIntoView = false, bool doLayout = true)
		{
			this._commonAssetTree.DeselectEntry();
			this._serverAssetTree.DeselectEntry();
			this._cosmeticsAssetTree.DeselectEntry();
			AssetEditorSettings assetEditorSettings = this.Interface.App.Settings;
			AssetTypeConfig assetTypeConfig;
			bool flag = !this.AssetTypeRegistry.AssetTypes.TryGetValue(assetReference.Type, out assetTypeConfig);
			if (!flag)
			{
				bool flag2 = assetTypeConfig.AssetTree != assetEditorSettings.ActiveAssetTree;
				if (flag2)
				{
					assetEditorSettings = assetEditorSettings.Clone();
					assetEditorSettings.ActiveAssetTree = assetTypeConfig.AssetTree;
					this.Interface.App.ApplySettings(assetEditorSettings);
					this.UpdateActiveAssetTree(doLayout);
				}
				this.GetAssetTree(assetTypeConfig.AssetTree).SelectEntry(assetReference, bringIntoView);
			}
		}

		// Token: 0x06005D4D RID: 23885 RVA: 0x001DA270 File Offset: 0x001D8470
		private void OpenChangelogModalIfNewVersion()
		{
			AssetEditorSettings assetEditorSettings = this.Interface.App.Settings;
			string lastUsedVersion = assetEditorSettings.LastUsedVersion;
			bool flag = false;
			bool flag2 = lastUsedVersion != null;
			if (flag2)
			{
				Version v = new Version("0.1.0");
				Version version = new Version(lastUsedVersion);
				bool flag3 = v > version;
				if (flag3)
				{
					flag = true;
					this.ChangelogModal.PreviouslyUsedVersion = version;
					this.Desktop.SetLayer(4, this.ChangelogModal);
				}
			}
			else
			{
				flag = true;
				this.Desktop.SetLayer(4, this.ChangelogModal);
			}
			bool flag4 = flag;
			if (flag4)
			{
				assetEditorSettings = assetEditorSettings.Clone();
				assetEditorSettings.LastUsedVersion = "0.1.0";
				this.Interface.App.ApplySettings(assetEditorSettings);
			}
		}

		// Token: 0x06005D4E RID: 23886 RVA: 0x001DA330 File Offset: 0x001D8530
		private void OpenCreateAssetModal()
		{
			bool flag = this.CheckHasUnsavedSourceEditorChanges(new Action(this.OpenCreateAssetModal));
			if (!flag)
			{
				string text = this.CurrentAsset.Type ?? Enumerable.FirstOrDefault<string>(this.AssetTypeRegistry.AssetTypes.Keys);
				bool flag2 = text != null && this.AssetTypeRegistry.AssetTypes[text].AssetTree == AssetTreeFolder.Common;
				if (flag2)
				{
					foreach (KeyValuePair<string, AssetTypeConfig> keyValuePair in this.AssetTypeRegistry.AssetTypes)
					{
						bool flag3 = keyValuePair.Value.AssetTree == AssetTreeFolder.Common;
						if (!flag3)
						{
							text = keyValuePair.Key;
							break;
						}
					}
				}
				bool flag4 = text == null;
				if (!flag4)
				{
					this.CreateAssetModal.Open(text, null, null, null, null, null);
				}
			}
		}

		// Token: 0x06005D4F RID: 23887 RVA: 0x001DA424 File Offset: 0x001D8624
		private AssetTree GetAssetTree(AssetTreeFolder assetTreeFolder)
		{
			AssetTree result;
			switch (assetTreeFolder)
			{
			case AssetTreeFolder.Server:
				result = this._serverAssetTree;
				break;
			case AssetTreeFolder.Common:
				result = this._commonAssetTree;
				break;
			case AssetTreeFolder.Cosmetics:
				result = this._cosmeticsAssetTree;
				break;
			default:
				throw new Exception("Invalid type: " + assetTreeFolder.ToString());
			}
			return result;
		}

		// Token: 0x06005D50 RID: 23888 RVA: 0x001DA484 File Offset: 0x001D8684
		public void FocusAssetInTree(AssetReference assetReference)
		{
			AssetTypeConfig assetTypeConfig = this.AssetTypeRegistry.AssetTypes[assetReference.Type];
			AssetEditorSettings settings = this.Interface.App.Settings;
			bool flag = assetTypeConfig.AssetTree != settings.ActiveAssetTree;
			if (flag)
			{
				settings.ActiveAssetTree = assetTypeConfig.AssetTree;
				this.Interface.App.ApplySettings(settings);
				this.UpdateActiveAssetTree(true);
			}
			this._serverAssetTree.BringEntryIntoView(assetReference);
		}

		// Token: 0x06005D51 RID: 23889 RVA: 0x001DA504 File Offset: 0x001D8704
		private void ToggleDiagnosticsPane()
		{
			this._isDiagnosticsPaneOpen = !this._isDiagnosticsPaneOpen;
			this._diagnosticsPane.Visible = this._isDiagnosticsPaneOpen;
			bool isDiagnosticsPaneOpen = this._isDiagnosticsPaneOpen;
			if (isDiagnosticsPaneOpen)
			{
				this.UpdateDiagnostics();
			}
			base.Layout(null, true);
		}

		// Token: 0x06005D52 RID: 23890 RVA: 0x001DA558 File Offset: 0x001D8758
		private void UpdateDiagnostics()
		{
			Document document;
			this.Interface.TryGetDocument("AssetEditor/DiagnosticsPaneEntry.ui", out document);
			bool flag = this._errorCount > 0;
			if (flag)
			{
				this._errorsInfo.Visible = true;
				this._errorsInfo.Find<Label>("Count").Text = this.Desktop.Provider.FormatNumber(this._errorCount);
			}
			else
			{
				this._errorsInfo.Visible = false;
			}
			bool flag2 = this._warningCount > 0;
			if (flag2)
			{
				this._warningsInfo.Visible = true;
				this._warningsInfo.Find<Label>("Count").Text = this.Desktop.Provider.FormatNumber(this._warningCount);
			}
			else
			{
				this._warningsInfo.Visible = false;
			}
			this._validatedInfo.Visible = (!this._warningsInfo.Visible && !this._errorsInfo.Visible);
			bool isDiagnosticsPaneOpen = this._isDiagnosticsPaneOpen;
			if (isDiagnosticsPaneOpen)
			{
				this._diagnosticsPane.Clear();
				using (Dictionary<string, AssetDiagnostics>.Enumerator enumerator = this.Diagnostics.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, AssetDiagnostics> entry = enumerator.Current;
						bool flag3 = entry.Value.Errors != null;
						if (flag3)
						{
							Action <>9__0;
							foreach (AssetDiagnosticMessage assetDiagnosticMessage in entry.Value.Errors)
							{
								UIFragment uifragment = document.Instantiate(this.Desktop, this._diagnosticsPane);
								uifragment.Get<Label>("File").Text = entry.Key;
								uifragment.Get<Label>("Message").Text = assetDiagnosticMessage.Property.ToString() + ": " + assetDiagnosticMessage.Message;
								BaseButton<Button.ButtonStyle, Button.ButtonStyleState> baseButton = uifragment.Get<Button>("Button");
								Action activating;
								if ((activating = <>9__0) == null)
								{
									activating = (<>9__0 = delegate()
									{
										this.OpenExistingAsset(entry.Key, true);
									});
								}
								baseButton.Activating = activating;
								uifragment.Get<Group>("Icon").Background = new PatchStyle("AssetEditor/ErrorIcon.png");
							}
						}
						bool flag4 = entry.Value.Warnings != null;
						if (flag4)
						{
							Action <>9__1;
							foreach (AssetDiagnosticMessage assetDiagnosticMessage2 in entry.Value.Warnings)
							{
								UIFragment uifragment2 = document.Instantiate(this.Desktop, this._diagnosticsPane);
								uifragment2.Get<Label>("File").Text = entry.Key;
								uifragment2.Get<Label>("Message").Text = assetDiagnosticMessage2.Property.ToString() + ": " + assetDiagnosticMessage2.Message;
								BaseButton<Button.ButtonStyle, Button.ButtonStyleState> baseButton2 = uifragment2.Get<Button>("Button");
								Action activating2;
								if ((activating2 = <>9__1) == null)
								{
									activating2 = (<>9__1 = delegate()
									{
										this.OpenExistingAsset(entry.Key, true);
									});
								}
								baseButton2.Activating = activating2;
								uifragment2.Get<Group>("Icon").Background = new PatchStyle("AssetEditor/WarningIcon.png");
							}
						}
					}
				}
			}
		}

		// Token: 0x06005D53 RID: 23891 RVA: 0x001DA904 File Offset: 0x001D8B04
		private void AddTab(AssetReference assetReference, bool setActive = true)
		{
			bool flag = false;
			EditorTabButton editorTabButton = null;
			int num = -1;
			for (int i = 0; i < this._tabs.Children.Count; i++)
			{
				EditorTabButton editorTabButton2 = (EditorTabButton)this._tabs.Children[i];
				bool flag2 = editorTabButton == null || editorTabButton.TimeLastActive > editorTabButton2.TimeLastActive;
				if (flag2)
				{
					editorTabButton = editorTabButton2;
				}
				bool isActive = editorTabButton2.IsActive;
				if (isActive)
				{
					num = i;
				}
				bool flag3 = editorTabButton2.AssetReference.Equals(assetReference);
				if (flag3)
				{
					if (setActive)
					{
						editorTabButton2.SetActive(true);
					}
					flag = true;
					this._tabs.ScrollChildElementIntoView(editorTabButton2);
				}
				else if (setActive)
				{
					editorTabButton2.SetActive(false);
				}
			}
			bool flag4 = !flag;
			if (flag4)
			{
				EditorTabButton editorTabButton3 = new EditorTabButton(this, assetReference);
				bool flag5 = num > -1;
				if (flag5)
				{
					this._tabs.Add(editorTabButton3, num + 1);
				}
				else
				{
					this._tabs.Add(editorTabButton3, -1);
				}
				editorTabButton3.Build();
				if (setActive)
				{
					editorTabButton3.SetActive(true);
				}
				bool flag6 = this._tabs.Children.Count > 25;
				if (flag6)
				{
					this._tabs.Remove(editorTabButton);
				}
				this._tabs.Layout(null, true);
				bool isMounted = base.IsMounted;
				if (isMounted)
				{
					this._tabs.ScrollChildElementIntoView(editorTabButton3);
				}
			}
		}

		// Token: 0x06005D54 RID: 23892 RVA: 0x001DAA88 File Offset: 0x001D8C88
		public void CloseAllTabs()
		{
			bool flag = this.CurrentAsset.FilePath != null;
			if (flag)
			{
				this._serverAssetTree.DeselectEntry();
				this._commonAssetTree.DeselectEntry();
				this._cosmeticsAssetTree.DeselectEntry();
				this.CurrentAsset = AssetReference.None;
				this.Interface.App.Editor.ClearPreview(false);
				this.Backend.SetOpenEditorAsset(this.CurrentAsset);
				this._editorPane.Clear();
				this._editorPane.Layout(null, true);
			}
			this._tabs.Clear();
			this._tabs.Layout(null, true);
		}

		// Token: 0x06005D55 RID: 23893 RVA: 0x001DAB48 File Offset: 0x001D8D48
		public void CloseTab(AssetReference assetReference)
		{
			bool flag = false;
			foreach (Element element in this._tabs.Children)
			{
				EditorTabButton editorTabButton = (EditorTabButton)element;
				bool flag2 = !editorTabButton.AssetReference.Equals(assetReference);
				if (!flag2)
				{
					this._tabs.Remove(editorTabButton);
					flag = true;
					break;
				}
			}
			bool flag3 = flag;
			if (flag3)
			{
				this._tabs.Layout(null, true);
			}
			bool flag4 = assetReference.Equals(this.CurrentAsset);
			if (flag4)
			{
				EditorTabButton editorTabButton2 = null;
				foreach (Element element2 in this._tabs.Children)
				{
					EditorTabButton editorTabButton3 = (EditorTabButton)element2;
					bool flag5 = editorTabButton2 == null || editorTabButton3.TimeLastActive > editorTabButton2.TimeLastActive;
					if (flag5)
					{
						editorTabButton2 = editorTabButton3;
					}
				}
				bool flag6 = editorTabButton2 != null;
				if (flag6)
				{
					this.OpenExistingAsset(editorTabButton2.AssetReference, false);
				}
				else
				{
					this._serverAssetTree.DeselectEntry();
					this._commonAssetTree.DeselectEntry();
					this._cosmeticsAssetTree.DeselectEntry();
					this.CurrentAsset = AssetReference.None;
					this.Interface.App.Editor.ClearPreview(false);
					this.Backend.SetOpenEditorAsset(this.CurrentAsset);
					this.SetupEditorPane();
				}
			}
		}

		// Token: 0x06005D56 RID: 23894 RVA: 0x001DACF4 File Offset: 0x001D8EF4
		public void UpdateTab(AssetReference oldReference, AssetReference newReference)
		{
			bool flag = false;
			foreach (Element element in this._tabs.Children)
			{
				EditorTabButton editorTabButton = (EditorTabButton)element;
				bool flag2 = !editorTabButton.AssetReference.Equals(oldReference);
				if (!flag2)
				{
					editorTabButton.OnAssetRenamed(newReference);
					flag = true;
					break;
				}
			}
			bool flag3 = flag;
			if (flag3)
			{
				this._tabs.Layout(null, true);
			}
		}

		// Token: 0x06005D57 RID: 23895 RVA: 0x001DAD90 File Offset: 0x001D8F90
		public void OpenExistingAsset(AssetReference assetReference, bool bringAssetIntoAssetTreeView = false)
		{
			bool flag = this.CheckHasUnsavedSourceEditorChanges(delegate
			{
				this.OpenExistingAsset(assetReference, bringAssetIntoAssetTreeView);
			});
			if (!flag)
			{
				this.SelectAssetTreeEntry(assetReference, bringAssetIntoAssetTreeView, true);
				bool flag2 = this.CurrentAsset.Equals(assetReference);
				if (!flag2)
				{
					this.CurrentAsset = assetReference;
					this.Interface.App.Editor.ClearPreview(false);
					this.Backend.SetOpenEditorAsset(assetReference);
					AssetEditorOverlay.Logger.Info<string, string>("Opening asset {0}:{1}", assetReference.Type, assetReference.FilePath);
					this.AddTab(assetReference, true);
					this.FetchOpenAsset();
				}
			}
		}

		// Token: 0x06005D58 RID: 23896 RVA: 0x001DAE74 File Offset: 0x001D9074
		public void OpenExistingAsset(string filePath, bool bringAssetIntoAssetTreeView = false)
		{
			bool flag = !this._areAssetTypesAndSchemasInitialized;
			if (flag)
			{
				this._assetToOpenOnceAssetFilesInitialized = new AssetEditorOverlay.AssetToOpen
				{
					FilePath = filePath
				};
			}
			else
			{
				string type;
				bool flag2 = !this.AssetTypeRegistry.TryGetAssetTypeFromPath(filePath, out type);
				if (!flag2)
				{
					bool flag3 = this.CheckHasUnsavedSourceEditorChanges(delegate
					{
						this.OpenExistingAsset(filePath, bringAssetIntoAssetTreeView);
					});
					if (!flag3)
					{
						this.OpenExistingAsset(new AssetReference(type, filePath), bringAssetIntoAssetTreeView);
					}
				}
			}
		}

		// Token: 0x06005D59 RID: 23897 RVA: 0x001DAF14 File Offset: 0x001D9114
		public void OpenExistingAssetById(AssetIdReference assetIdReference, bool bringAssetIntoAssetTreeView = false)
		{
			bool flag = this.CheckHasUnsavedSourceEditorChanges(delegate
			{
				this.OpenExistingAssetById(assetIdReference, bringAssetIntoAssetTreeView);
			});
			if (!flag)
			{
				bool areAssetFilesInitialized = this._areAssetFilesInitialized;
				if (areAssetFilesInitialized)
				{
					string filePath;
					bool flag2 = this.Assets.TryGetPathForAssetId(assetIdReference.Type, assetIdReference.Id, out filePath, false);
					if (flag2)
					{
						this.OpenExistingAsset(new AssetReference(assetIdReference.Type, filePath), bringAssetIntoAssetTreeView);
					}
					else
					{
						AssetEditorOverlay.Logger.Warn<string, string>("Failed to late open asset since a path for this id could not be found: {0} ({1})", assetIdReference.Id, assetIdReference.Type);
					}
				}
				else
				{
					this._assetToOpenOnceAssetFilesInitialized = new AssetEditorOverlay.AssetToOpen
					{
						Id = assetIdReference
					};
				}
			}
		}

		// Token: 0x06005D5A RID: 23898 RVA: 0x001DAFF3 File Offset: 0x001D91F3
		public void FetchOpenAsset()
		{
			this.ResetTrackedAssets();
			this.FetchTrackedAsset(this.CurrentAsset, true);
			this.SetupEditorPane();
		}

		// Token: 0x06005D5B RID: 23899 RVA: 0x001DB012 File Offset: 0x001D9212
		private void ResetTrackedAssets()
		{
			this.FinishWork();
			this._awaitingInitialEditorSetup = true;
			CancellationTokenSource currentAssetCancellationToken = this._currentAssetCancellationToken;
			if (currentAssetCancellationToken != null)
			{
				currentAssetCancellationToken.Cancel();
			}
			this._currentAssetCancellationToken = new CancellationTokenSource();
			this.TrackedAssets.Clear();
		}

		// Token: 0x06005D5C RID: 23900 RVA: 0x001DB04C File Offset: 0x001D924C
		public void FetchTrackedAsset(AssetReference assetReference, bool isFirstRequest = false)
		{
			CancellationToken cancelToken = this._currentAssetCancellationToken.Token;
			TrackedAsset trackedAsset;
			bool flag = !this.TrackedAssets.TryGetValue(assetReference.FilePath, out trackedAsset);
			if (flag)
			{
				trackedAsset = new TrackedAsset(assetReference, null);
				this.TrackedAssets[assetReference.FilePath] = trackedAsset;
			}
			bool isLoading = trackedAsset.IsLoading;
			if (!isLoading)
			{
				trackedAsset.FetchError = null;
				trackedAsset.IsLoading = true;
				AssetTypeConfig assetTypeConfig = this.AssetTypeRegistry.AssetTypes[assetReference.Type];
				bool flag2 = assetTypeConfig.AssetTree != AssetTreeFolder.Cosmetics && assetTypeConfig.IsJson && assetTypeConfig.Schema != null;
				if (flag2)
				{
					this.Backend.FetchJsonAssetWithParents(assetReference, delegate(Dictionary<string, TrackedAsset> results, FormattedMessage fetchParentsError)
					{
						bool isCancellationRequested = cancelToken.IsCancellationRequested;
						if (!isCancellationRequested)
						{
							bool flag3 = !this.TrackedAssets.TryGetValue(assetReference.FilePath, out trackedAsset) || !trackedAsset.IsLoading;
							if (!flag3)
							{
								bool flag4 = fetchParentsError == null;
								if (flag4)
								{
									foreach (KeyValuePair<string, TrackedAsset> keyValuePair in results)
									{
										this.TrackedAssets[keyValuePair.Key] = keyValuePair.Value;
									}
								}
								else
								{
									trackedAsset.IsLoading = false;
									trackedAsset.FetchError = fetchParentsError;
									this.ToastNotifications.AddNotification(2, fetchParentsError);
								}
								this.OnTrackedAssetUpdated(this.TrackedAssets[assetReference.FilePath], isFirstRequest);
							}
						}
					}, true);
				}
				else
				{
					this.Backend.FetchAsset(assetReference, delegate(object data, FormattedMessage error)
					{
						bool isCancellationRequested = cancelToken.IsCancellationRequested;
						if (!isCancellationRequested)
						{
							bool flag3 = !this.TrackedAssets.TryGetValue(assetReference.FilePath, out trackedAsset) || !trackedAsset.IsLoading;
							if (!flag3)
							{
								trackedAsset.IsLoading = false;
								trackedAsset.FetchError = error;
								trackedAsset.Data = data;
								bool flag4 = error != null;
								if (flag4)
								{
									this.ToastNotifications.AddNotification(2, error);
								}
								this.OnTrackedAssetUpdated(trackedAsset, isFirstRequest);
							}
						}
					}, true);
				}
			}
		}

		// Token: 0x06005D5D RID: 23901 RVA: 0x001DB18C File Offset: 0x001D938C
		private void OnTrackedAssetUpdated(TrackedAsset asset, bool isFirstRequest = false)
		{
			bool flag = this._awaitingInitialEditorSetup && !isFirstRequest;
			if (!flag)
			{
				bool awaitingInitialEditorSetup = this._awaitingInitialEditorSetup;
				if (awaitingInitialEditorSetup)
				{
					this._awaitingInitialEditorSetup = false;
					this.SetupEditorPane();
				}
				else
				{
					bool flag2 = !base.IsMounted;
					if (!flag2)
					{
						bool flag3 = asset.Reference.Equals(this.CurrentAsset);
						if (flag3)
						{
							bool isMounted = this.ConfigEditor.IsMounted;
							if (isMounted)
							{
								this.ConfigEditor.UpdateJson((JObject)asset.Data);
							}
							else
							{
								this.SetupEditorPane();
							}
						}
						bool isMounted2 = this.ConfigEditorContextPane.IsMounted;
						if (isMounted2)
						{
							this.ConfigEditorContextPane.OnTrackedAssetChanged(asset);
						}
					}
				}
			}
		}

		// Token: 0x06005D5E RID: 23902 RVA: 0x001DB248 File Offset: 0x001D9448
		public void ReloadInheritanceStack()
		{
			AssetReference currentAsset = this.CurrentAsset;
			AssetTypeConfig assetTypeConfig;
			bool flag = !this.AssetTypeRegistry.AssetTypes.TryGetValue(currentAsset.Type, out assetTypeConfig) || assetTypeConfig.AssetTree == AssetTreeFolder.Cosmetics;
			if (!flag)
			{
				CancellationTokenSource reloadInheritanceStackCancellationTokenSource = this._reloadInheritanceStackCancellationTokenSource;
				if (reloadInheritanceStackCancellationTokenSource != null)
				{
					reloadInheritanceStackCancellationTokenSource.Cancel();
				}
				this._reloadInheritanceStackCancellationTokenSource = new CancellationTokenSource();
				CancellationToken cancellationToken = this._reloadInheritanceStackCancellationTokenSource.Token;
				this.Backend.FetchJsonAssetWithParents(currentAsset, delegate(Dictionary<string, TrackedAsset> results, FormattedMessage error)
				{
					bool flag2 = !currentAsset.Equals(this.CurrentAsset) || cancellationToken.IsCancellationRequested;
					if (!flag2)
					{
						this.TrackedAssets.Clear();
						bool flag3 = error == null;
						if (flag3)
						{
							foreach (KeyValuePair<string, TrackedAsset> keyValuePair in results)
							{
								this.TrackedAssets[keyValuePair.Key] = keyValuePair.Value;
							}
						}
						else
						{
							this.ToastNotifications.AddNotification(2, this.Desktop.Provider.GetText("ui.assetEditor.errors.failedToFetchParent", null, true));
						}
					}
				}, false);
			}
		}

		// Token: 0x06005D5F RID: 23903 RVA: 0x001DB2F0 File Offset: 0x001D94F0
		public void FetchParents(AssetReference assetReference, JObject jObject, Action<List<TrackedAsset>, FormattedMessage> callback)
		{
			AssetEditorOverlay.<>c__DisplayClass149_0 CS$<>8__locals1 = new AssetEditorOverlay.<>c__DisplayClass149_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.callback = callback;
			CS$<>8__locals1.assetReference = assetReference;
			CS$<>8__locals1.list = new List<TrackedAsset>
			{
				new TrackedAsset(CS$<>8__locals1.assetReference, jObject)
			};
			CS$<>8__locals1.<FetchParents>g__FetchNext|0(jObject);
		}

		// Token: 0x06005D60 RID: 23904 RVA: 0x001DB340 File Offset: 0x001D9540
		private void SetupEditorPane()
		{
			Element parent = this.WeatherDaytimeBar.Parent;
			if (parent != null)
			{
				parent.Remove(this.WeatherDaytimeBar);
			}
			this.ConfigEditorContextPane.SetActiveCategory(null, true);
			TrackedAsset trackedAsset;
			bool flag = this.CurrentAsset.Equals(AssetReference.None) || !this.TrackedAssets.TryGetValue(this.CurrentAsset.FilePath, out trackedAsset);
			if (flag)
			{
				this._contextPane.Clear();
				this._editorPane.Clear();
				this._contentPane.Layout(null, true);
			}
			else
			{
				bool isLoading = trackedAsset.IsLoading;
				if (isLoading)
				{
					this._contextPane.Clear();
					this._editorPane.Clear();
					this._editorPane.Add(this._assetLoadingIndicator, -1);
					this._contentPane.Layout(null, true);
				}
				else
				{
					bool flag2 = trackedAsset.Data == null || trackedAsset.FetchError != null;
					if (flag2)
					{
						FormattedMessage formattedMessage = FormattedMessage.FromMessageId("ui.assetEditor.errors.failedToFetchAsset", new Dictionary<string, object>
						{
							{
								"path",
								this.CurrentAsset.FilePath
							}
						});
						bool flag3 = trackedAsset.FetchError != null;
						if (flag3)
						{
							formattedMessage.Children = new List<FormattedMessage>
							{
								new FormattedMessage
								{
									RawText = "\n\n"
								},
								trackedAsset.FetchError
							};
						}
						this.ShowAssetError(formattedMessage);
					}
					else
					{
						AssetTypeConfig assetTypeConfig;
						bool flag4 = !this.AssetTypeRegistry.AssetTypes.TryGetValue(this.CurrentAsset.Type, out assetTypeConfig);
						if (flag4)
						{
							this.ShowAssetError(new FormattedMessage
							{
								MessageId = "ui.assetEditor.errors.unknownAssetType",
								Params = new Dictionary<string, object>
								{
									{
										"assetType",
										this.CurrentAsset.Type
									}
								}
							});
						}
						else
						{
							bool flag5 = assetTypeConfig.EditorType != 3 && assetTypeConfig.EditorType != 2 && assetTypeConfig.EditorType != 1;
							if (flag5)
							{
								this.ShowAssetError(FormattedMessage.FromMessageId("ui.assetEditor.errors.fileFormatNotSupported", null));
							}
							else
							{
								bool flag6 = this.Mode == AssetEditorOverlay.EditorMode.Editor && assetTypeConfig.Schema == null;
								if (flag6)
								{
									this.Mode = AssetEditorOverlay.EditorMode.Source;
									this._modeSelection.SelectedTab = AssetEditorOverlay.EditorMode.Source.ToString();
									this._modeSelection.Layout(null, true);
								}
								bool flag7 = this.Mode == AssetEditorOverlay.EditorMode.Editor;
								Element element;
								if (flag7)
								{
									this.ConfigEditor.Setup(assetTypeConfig, (JObject)trackedAsset.Data, this.CurrentAsset);
									element = this.ConfigEditor;
								}
								else
								{
									this._sourceEditor.Setup(trackedAsset.Data.ToString(), (assetTypeConfig.EditorType == 2 || assetTypeConfig.EditorType == 3) ? WebCodeEditor.EditorLanguage.Json : WebCodeEditor.EditorLanguage.Plaintext, this.CurrentAsset);
									element = this._sourceEditor;
								}
								bool flag8 = !element.IsMounted;
								if (flag8)
								{
									this._editorPane.Clear();
									this._editorPane.Add(element, -1);
								}
								bool flag9 = assetTypeConfig.EditorFeatures != null && assetTypeConfig.EditorFeatures.Contains(AssetTypeConfig.EditorFeature.WeatherDaytimeBar);
								if (flag9)
								{
									this._editorPane.Add(this.WeatherDaytimeBar, 0);
								}
								bool visible = this.ConfigEditor.Categories.Count > 1 || assetTypeConfig.Preview > AssetTypeConfig.PreviewType.None;
								this._contextPane.Visible = visible;
								this.ConfigEditorContextPane.Update();
								bool flag10 = this.ConfigEditorContextPane.Parent == null;
								if (flag10)
								{
									this._contextPane.Clear();
									this._contextPane.Add(this.ConfigEditorContextPane, -1);
								}
								this._contentPane.Layout(null, true);
							}
						}
					}
				}
			}
		}

		// Token: 0x06005D61 RID: 23905 RVA: 0x001DB720 File Offset: 0x001D9920
		private void ShowAssetError(FormattedMessage message)
		{
			this._contextPane.Clear();
			this._editorPane.Clear();
			Label label = new Label(this.Desktop, this._editorPane);
			label.TextSpans = FormattedMessageConverter.GetLabelSpans(message, this.Interface, default(SpanStyle), true);
			label.Style = new LabelStyle
			{
				Alignment = LabelStyle.LabelAlignment.Center
			};
			label.FlexWeight = 1;
			this._contentPane.Layout(null, true);
		}

		// Token: 0x06005D62 RID: 23906 RVA: 0x001DB7A3 File Offset: 0x001D99A3
		private void Reload()
		{
			this.Backend.Initialize();
		}

		// Token: 0x06005D63 RID: 23907 RVA: 0x001DB7B1 File Offset: 0x001D99B1
		private void FocusSearch()
		{
			this.Desktop.FocusElement(this._assetBrowserSearchInput, true);
			this._assetBrowserSearchInput.SelectAll();
		}

		// Token: 0x06005D64 RID: 23908 RVA: 0x001DB7D4 File Offset: 0x001D99D4
		public void UndoChanges()
		{
			bool flag = !this.CanEditCurrentAsset();
			if (!flag)
			{
				this.ConfigEditor.SubmitPendingUpdateCommands();
				this.Backend.UndoChanges(this.CurrentAsset);
			}
		}

		// Token: 0x06005D65 RID: 23909 RVA: 0x001DB810 File Offset: 0x001D9A10
		public void RedoChanges()
		{
			bool flag = !this.CanEditCurrentAsset();
			if (!flag)
			{
				this.ConfigEditor.SubmitPendingUpdateCommands();
				this.Backend.RedoChanges(this.CurrentAsset);
			}
		}

		// Token: 0x06005D66 RID: 23910 RVA: 0x001DB84C File Offset: 0x001D9A4C
		protected internal override void OnKeyDown(SDL.SDL_Keycode keyCode, int repeat)
		{
			base.OnKeyDown(keyCode, repeat);
			bool flag = !this.Desktop.IsShortcutKeyDown;
			if (!flag)
			{
				if (keyCode != SDL.SDL_Keycode.SDLK_e)
				{
					if (keyCode != SDL.SDL_Keycode.SDLK_f)
					{
						switch (keyCode)
						{
						case SDL.SDL_Keycode.SDLK_n:
							this.OpenCreateAssetModal();
							break;
						case SDL.SDL_Keycode.SDLK_o:
							this.ConfigEditor.ToggleDisplayUnsetProperties();
							break;
						case SDL.SDL_Keycode.SDLK_p:
						{
							bool isMounted = this.ConfigEditor.IsMounted;
							if (isMounted)
							{
								this.ConfigEditor.FocusPropertySearch();
							}
							break;
						}
						case SDL.SDL_Keycode.SDLK_r:
							this.Reload();
							break;
						case SDL.SDL_Keycode.SDLK_s:
						{
							bool flag2 = this.Mode == AssetEditorOverlay.EditorMode.Source;
							if (!flag2)
							{
								this.SaveAll();
							}
							break;
						}
						case SDL.SDL_Keycode.SDLK_t:
							this.FilterModal.Open();
							break;
						case SDL.SDL_Keycode.SDLK_y:
						{
							bool flag3 = BuildInfo.Platform != Platform.MacOS;
							if (flag3)
							{
								this.RedoChanges();
							}
							break;
						}
						case SDL.SDL_Keycode.SDLK_z:
						{
							bool flag4 = this.Mode == AssetEditorOverlay.EditorMode.Source;
							if (!flag4)
							{
								bool flag5 = this.Desktop.IsShiftKeyDown && BuildInfo.Platform == Platform.MacOS;
								if (flag5)
								{
									this.RedoChanges();
								}
								else
								{
									this.UndoChanges();
								}
							}
							break;
						}
						}
					}
					else
					{
						this.FocusSearch();
					}
				}
				else
				{
					this.ExportCurrentAsset();
				}
			}
		}

		// Token: 0x06005D67 RID: 23911 RVA: 0x001DB9B0 File Offset: 0x001D9BB0
		public void ExportCurrentAsset()
		{
			bool flag = this.Backend == null || !this.Backend.IsEditingRemotely || this.CurrentAsset.Type == null;
			if (!flag)
			{
				this.Backend.ExportAssets(new List<AssetReference>
				{
					this.CurrentAsset
				}, null);
			}
		}

		// Token: 0x06005D68 RID: 23912 RVA: 0x001DBA0C File Offset: 0x001D9C0C
		public void SaveAll()
		{
			bool flag = this.CurrentAsset.Type != null;
			if (flag)
			{
				AssetEditorOverlay.EditorMode mode = this.Mode;
				AssetEditorOverlay.EditorMode editorMode = mode;
				if (editorMode != AssetEditorOverlay.EditorMode.Editor)
				{
					if (editorMode == AssetEditorOverlay.EditorMode.Source)
					{
						if (!this._sourceEditor.ApplyChanges())
						{
							AssetEditorOverlay.Logger.Info("Source Editor validation failed. Skip saving all...");
							return;
						}
					}
				}
				else
				{
					this.ConfigEditor.SubmitPendingUpdateCommands();
				}
			}
			AssetEditorBackend backend = this.Backend;
			if (backend != null)
			{
				backend.SaveUnsavedChanges();
			}
		}

		// Token: 0x06005D69 RID: 23913 RVA: 0x001DBA84 File Offset: 0x001D9C84
		private bool CheckHasUnsavedSourceEditorChanges(Action onChangesDiscarded)
		{
			AssetEditorOverlay.<>c__DisplayClass159_0 CS$<>8__locals1 = new AssetEditorOverlay.<>c__DisplayClass159_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.onChangesDiscarded = onChangesDiscarded;
			bool flag = this.Mode == AssetEditorOverlay.EditorMode.Source && this._sourceEditor.HasUnsavedChanges;
			bool result;
			if (flag)
			{
				this.ConfirmationModal.Open(this.Interface.GetText("ui.assetEditor.sourceEditor.unsavedChangesModal.title", null, true), this.Interface.GetText("ui.assetEditor.sourceEditor.unsavedChangesModal.text", null, true), new Action(CS$<>8__locals1.<CheckHasUnsavedSourceEditorChanges>g__OnDiscard|0), null, this.Interface.GetText("ui.assetEditor.sourceEditor.unsavedChangesModal.discardChanges", null, true), null, false);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06005D6A RID: 23914 RVA: 0x001DBB20 File Offset: 0x001D9D20
		public bool CheckHasUnexportedChanges(bool quit, Action onConfirm)
		{
			bool flag = this.Backend == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this.FinishWork();
				result = false;
			}
			return result;
		}

		// Token: 0x06005D6B RID: 23915 RVA: 0x001DBB4C File Offset: 0x001D9D4C
		public void RegisterDropdownWithDataset(string dataset, DropdownBox dropdownBox, object extraValue = null)
		{
			List<DropdownBox> list;
			bool flag = !this._dropdownBoxesWithDataset.TryGetValue(dataset, out list);
			if (flag)
			{
				list = (this._dropdownBoxesWithDataset[dataset] = new List<DropdownBox>());
			}
			list.Add(dropdownBox);
			this.UpdateDropdownDataset(dataset, dropdownBox, null);
		}

		// Token: 0x06005D6C RID: 23916 RVA: 0x001DBB9C File Offset: 0x001D9D9C
		public void UnregisterDropdownWithDataset(string dataset, DropdownBox dropdownBox)
		{
			List<DropdownBox> list;
			bool flag = !this._dropdownBoxesWithDataset.TryGetValue(dataset, out list);
			if (!flag)
			{
				list.Remove(dropdownBox);
			}
		}

		// Token: 0x06005D6D RID: 23917 RVA: 0x001DBBCC File Offset: 0x001D9DCC
		public void UpdateDropdownDataset(string dataset, DropdownBox dropdownBox, object extraValue = null)
		{
			List<string> list;
			bool flag = !this.Backend.TryGetDropdownEntriesOrFetch(dataset, out list, extraValue);
			if (!flag)
			{
				List<DropdownBox.DropdownEntryInfo> list2 = new List<DropdownBox.DropdownEntryInfo>();
				foreach (string text in list)
				{
					list2.Add(new DropdownBox.DropdownEntryInfo(text, text, false));
				}
				dropdownBox.Entries = list2;
			}
		}

		// Token: 0x06005D6E RID: 23918 RVA: 0x001DBC50 File Offset: 0x001D9E50
		public void OnDropdownDatasetUpdated(string dataset, List<string> entries)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			List<DropdownBox> list;
			bool flag = this._dropdownBoxesWithDataset.TryGetValue(dataset, out list);
			if (flag)
			{
				List<DropdownBox.DropdownEntryInfo> list2 = new List<DropdownBox.DropdownEntryInfo>();
				foreach (string text in entries)
				{
					list2.Add(new DropdownBox.DropdownEntryInfo(text, text, false));
				}
				foreach (DropdownBox dropdownBox in list)
				{
					dropdownBox.Entries = list2;
					bool isMounted = dropdownBox.IsMounted;
					if (isMounted)
					{
						dropdownBox.Layout(null, true);
					}
				}
			}
		}

		// Token: 0x06005D6F RID: 23919 RVA: 0x001DBD3C File Offset: 0x001D9F3C
		public void AttachNotifications(Element parent)
		{
			base.Remove(this.ToastNotifications);
			parent.Add(this.ToastNotifications, -1);
		}

		// Token: 0x06005D70 RID: 23920 RVA: 0x001DBD5C File Offset: 0x001D9F5C
		public void ReparentNotifications()
		{
			this.ToastNotifications.Parent.Remove(this.ToastNotifications);
			base.Add(this.ToastNotifications, -1);
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.ToastNotifications.Layout(new Rectangle?(this._rectangleAfterPadding), true);
			}
		}

		// Token: 0x06005D71 RID: 23921 RVA: 0x001DBDB4 File Offset: 0x001D9FB4
		public void UpdateModelPreview()
		{
			bool flag = !this.ConfigEditorContextPane.IsMounted;
			if (!flag)
			{
				this.ConfigEditorContextPane.UpdatePreview(true);
			}
		}

		// Token: 0x06005D72 RID: 23922 RVA: 0x001DBDE4 File Offset: 0x001D9FE4
		public void SetFileSaveStatus(AssetEditorOverlay.SaveStatus saveStatus, bool doLayout = true)
		{
			bool flag = saveStatus == this._fileSaveStatus;
			if (!flag)
			{
				this._fileSaveStatus = saveStatus;
				this._fileSaveStatusGroup.Visible = (saveStatus > AssetEditorOverlay.SaveStatus.Disabled);
				this._fileSaveStatusGroup.Find<Label>("Label").Text = this.Desktop.Provider.GetText(string.Format("ui.assetEditor.fileSaveStatus.{0}", saveStatus), null, true);
				bool flag2 = doLayout && base.IsMounted;
				if (flag2)
				{
					this._footer.Layout(null, true);
				}
			}
		}

		// Token: 0x06005D73 RID: 23923 RVA: 0x001DBE78 File Offset: 0x001DA078
		public void SetupAssetPopup(AssetReference assetReference, List<PopupMenuItem> items)
		{
			items.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.assetBrowser.copyPath", null, true), delegate()
			{
				SDL.SDL_SetClipboardText(assetReference.FilePath);
			}, null, null));
			items.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.assetBrowser.duplicate", null, true), delegate()
			{
				this.CreateAssetModal.Open(assetReference.Type, assetReference.FilePath, null, null, null, null);
			}, null, null));
			items.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.assetBrowser.rename", null, true), delegate()
			{
				this.RenameModal.OpenForAsset(assetReference, this.Backend.IsEditingRemotely);
			}, null, null));
			items.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.assetBrowser.delete", null, true), delegate()
			{
				this.OpenDeleteAssetPrompt(assetReference);
			}, null, null));
			bool flag = this.Backend is ServerAssetEditorBackend;
			if (flag)
			{
				items.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.assetBrowser.export", null, true), delegate()
				{
					this.Backend.ExportAssets(new List<AssetReference>
					{
						assetReference
					}, null);
				}, null, null));
				items.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.assetBrowser.exportAndDiscard", null, true), delegate()
				{
					this.Backend.ExportAndDiscardAssets(new List<AssetReference>
					{
						assetReference
					});
				}, null, null));
				items.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.assetBrowser.discard", null, true), delegate()
				{
					this.Backend.DiscardChanges(new TimestampedAssetReference(assetReference.FilePath, null));
				}, null, null));
			}
			items.Add(new PopupMenuItem(this.Desktop.Provider.GetText((BuildInfo.Platform == Platform.MacOS) ? "ui.assetEditor.assetBrowser.showInFinder" : "ui.assetEditor.assetBrowser.showInExplorer", null, true), delegate()
			{
				this.RevealAssetInDirectory(assetReference.FilePath);
			}, null, null));
		}

		// Token: 0x06005D74 RID: 23924 RVA: 0x001DC048 File Offset: 0x001DA248
		public void OpenIpcOpenEditorConfirmationModal(string serverName)
		{
			string text = this.Desktop.Provider.GetText("ui.assetEditor.ipcServerConfirmation.title", null, true);
			string text2 = this.Desktop.Provider.GetText("ui.assetEditor.ipcServerConfirmation.description", new Dictionary<string, string>
			{
				{
					"serverName",
					serverName
				}
			}, true);
			this.ConfirmationModal.Open(text, text2, delegate
			{
				this.Interface.App.MainMenu.Open();
			}, null, null, null, false);
		}

		// Token: 0x06005D75 RID: 23925 RVA: 0x001DC0B8 File Offset: 0x001DA2B8
		private void RevealAssetInDirectory(string assetPath)
		{
			string assetsPath = this.Interface.App.Settings.AssetsPath;
			string text = Path.Combine(assetsPath, assetPath);
			bool flag = !OpenUtils.TryOpenFileInContainingDirectory(text, assetsPath) && !OpenUtils.TryOpenDirectoryInContainingDirectory(text, assetsPath);
			if (flag)
			{
				AssetEditorOverlay.Logger.Warn("Failed to open {0}", text);
			}
		}

		// Token: 0x06005D76 RID: 23926 RVA: 0x001DC114 File Offset: 0x001DA314
		private void OpenDeleteAssetPrompt(AssetReference assetReference)
		{
			string text = this.Desktop.Provider.GetText("ui.assetEditor.deleteAssetModal.text", new Dictionary<string, string>
			{
				{
					"assetId",
					assetReference.FilePath
				}
			}, true);
			this.ConfirmationModal.Open(this.Desktop.Provider.GetText("ui.assetEditor.deleteAssetModal.title", null, true), text, delegate
			{
				this.Backend.DeleteAsset(assetReference, this.ConfirmationModal.ApplyChangesLocally);
			}, null, this.Desktop.Provider.GetText("ui.assetEditor.deleteAssetModal.confirmButton", null, true), null, this.Backend.IsEditingRemotely);
		}

		// Token: 0x06005D77 RID: 23927 RVA: 0x001DC1C0 File Offset: 0x001DA3C0
		public void UpdatePaneSize(AssetEditorSettings.Panes pane, int size)
		{
			AssetEditorSettings settings = this.Interface.App.Settings;
			settings.PaneSizes[pane] = size;
			settings.Save();
		}

		// Token: 0x06005D78 RID: 23928 RVA: 0x001DC1F4 File Offset: 0x001DA3F4
		private void TryClose()
		{
			bool flag = this.CheckHasUnsavedSourceEditorChanges(new Action(this.TryClose));
			if (!flag)
			{
				this.Interface.App.Editor.CloseEditor();
			}
		}

		// Token: 0x06005D79 RID: 23929 RVA: 0x001DC230 File Offset: 0x001DA430
		private SchemaNode GetSchema(string schemaReference, string rootSchemaId)
		{
			bool flag = schemaReference.StartsWith("#");
			if (flag)
			{
				schemaReference = rootSchemaId + schemaReference;
			}
			schemaReference = schemaReference.TrimEnd(new char[]
			{
				'#'
			});
			return this._schemas[schemaReference];
		}

		// Token: 0x06005D7A RID: 23930 RVA: 0x001DC27C File Offset: 0x001DA47C
		public SchemaNode ResolveSchemaInCurrentContext(SchemaNode schema)
		{
			return this.ResolveSchema(schema, this.AssetTypeRegistry.AssetTypes[this.CurrentAsset.Type].Schema);
		}

		// Token: 0x06005D7B RID: 23931 RVA: 0x001DC2B8 File Offset: 0x001DA4B8
		public SchemaNode ResolveSchema(SchemaNode schema, SchemaNode rootSchema)
		{
			bool flag = schema.SchemaReference == null;
			SchemaNode result;
			if (flag)
			{
				result = schema;
			}
			else
			{
				SchemaNode schemaNode = this.GetSchema(schema.SchemaReference, rootSchema.Id).Clone(false);
				bool flag2 = schema.Title != null;
				if (flag2)
				{
					schemaNode.Title = schema.Title;
				}
				bool flag3 = schema.Description != null;
				if (flag3)
				{
					schemaNode.Description = schema.Description;
				}
				bool allowEmptyObject = schema.AllowEmptyObject;
				if (allowEmptyObject)
				{
					schemaNode.AllowEmptyObject = true;
				}
				bool flag4 = schema.RebuildCaches != null;
				if (flag4)
				{
					schemaNode.RebuildCaches = schema.RebuildCaches;
				}
				bool flag5 = !schema.IsCollapsedByDefault;
				if (flag5)
				{
					schemaNode.IsCollapsedByDefault = false;
				}
				schemaNode.RebuildCachesForChildProperties = schema.RebuildCachesForChildProperties;
				schemaNode.DefaultValue = schema.DefaultValue;
				schemaNode.InheritsProperty = schema.InheritsProperty;
				result = schemaNode;
			}
			return result;
		}

		// Token: 0x06005D7C RID: 23932 RVA: 0x001DC394 File Offset: 0x001DA594
		public SchemaNode GetSchemaNodeInCurrentContext(JObject value, PropertyPath path)
		{
			SchemaNode schemaNode = this.ResolveSchemaInCurrentContext(this.AssetTypeRegistry.AssetTypes[this.CurrentAsset.Type].Schema);
			JToken jtoken = value;
			this.TryResolveTypeSchemaInCurrentContext(jtoken, ref schemaNode);
			bool flag = path.Elements.Length == 0;
			SchemaNode result;
			if (flag)
			{
				result = schemaNode;
			}
			else
			{
				string[] elements = path.Elements;
				int i = 0;
				while (i < elements.Length)
				{
					string text = elements[i];
					SchemaNode.NodeType type = schemaNode.Type;
					SchemaNode.NodeType nodeType = type;
					if (nodeType == SchemaNode.NodeType.AssetReferenceOrInline)
					{
						goto IL_FF;
					}
					if (nodeType - SchemaNode.NodeType.List > 1)
					{
						if (nodeType == SchemaNode.NodeType.Object)
						{
							goto IL_FF;
						}
					}
					else
					{
						schemaNode = this.ResolveSchemaInCurrentContext(schemaNode.Value);
						bool flag2 = jtoken == null || jtoken.Type == 10;
						if (flag2)
						{
							jtoken = null;
						}
						else
						{
							bool flag3 = jtoken.Type == 2;
							if (flag3)
							{
								jtoken = jtoken[int.Parse(text)];
							}
							else
							{
								bool flag4 = jtoken.Type == 1;
								if (flag4)
								{
									jtoken = jtoken[text];
								}
								else
								{
									jtoken = null;
								}
							}
						}
					}
					IL_178:
					i++;
					continue;
					IL_FF:
					bool flag5 = schemaNode.Type == SchemaNode.NodeType.AssetReferenceOrInline;
					if (flag5)
					{
						schemaNode = this.ResolveSchemaInCurrentContext(schemaNode.Value);
					}
					schemaNode = this.ResolveSchemaInCurrentContext(schemaNode.Properties[text]);
					bool flag6 = jtoken == null || jtoken.Type == 10;
					if (flag6)
					{
						jtoken = null;
					}
					else
					{
						bool flag7 = jtoken.Type == 1;
						if (flag7)
						{
							jtoken = jtoken[text];
						}
						else
						{
							jtoken = null;
						}
					}
					this.TryResolveTypeSchemaInCurrentContext(jtoken, ref schemaNode);
					goto IL_178;
				}
				result = schemaNode;
			}
			return result;
		}

		// Token: 0x06005D7D RID: 23933 RVA: 0x001DC530 File Offset: 0x001DA730
		public bool TryResolveTypeSchemaInCurrentContext(JToken value, ref SchemaNode schemaNode)
		{
			bool flag = schemaNode.TypePropertyKey != null;
			if (flag)
			{
				bool flag2 = ((value != null) ? value[schemaNode.TypePropertyKey] : null) != null;
				if (flag2)
				{
					JToken jtoken = value[schemaNode.TypePropertyKey];
					return this.TryResolveTypeSchemaInCurrentContext((string)jtoken, ref schemaNode);
				}
				bool flag3 = schemaNode.DefaultTypeSchema != null;
				if (flag3)
				{
					return this.TryResolveTypeSchemaInCurrentContext(schemaNode.DefaultTypeSchema, ref schemaNode);
				}
			}
			return false;
		}

		// Token: 0x06005D7E RID: 23934 RVA: 0x001DC5B0 File Offset: 0x001DA7B0
		public bool TryResolveTypeSchemaInCurrentContext(string type, ref SchemaNode schemaNode)
		{
			return this.TryResolveTypeSchema(type, ref schemaNode, this.AssetTypeRegistry.AssetTypes[this.CurrentAsset.Type].Schema);
		}

		// Token: 0x06005D7F RID: 23935 RVA: 0x001DC5EC File Offset: 0x001DA7EC
		public bool TryResolveTypeSchema(string type, ref SchemaNode schemaNode, SchemaNode rootSchema)
		{
			for (int i = 0; i < schemaNode.TypeSchemas.Length; i++)
			{
				SchemaNode schema = schemaNode.TypeSchemas[i];
				string a = schemaNode.Value.Enum[i];
				bool flag = type != null && a == type;
				if (flag)
				{
					schemaNode = this.ResolveSchema(schema, rootSchema);
					return true;
				}
			}
			schemaNode = null;
			return false;
		}

		// Token: 0x06005D80 RID: 23936 RVA: 0x001DC65C File Offset: 0x001DA85C
		private void MergeParentObject(SchemaNode schema, JObject targetObject, JObject parentObject, SchemaNode rootSchema)
		{
			foreach (KeyValuePair<string, SchemaNode> keyValuePair in schema.Properties)
			{
				string key = keyValuePair.Key;
				bool flag = targetObject.ContainsKey(key);
				if (flag)
				{
					if (!keyValuePair.Value.InheritsProperty)
					{
						goto IL_71;
					}
					JToken jtoken = targetObject[key];
					if (jtoken == null || jtoken.Type != 1)
					{
						goto IL_71;
					}
					JToken jtoken2 = parentObject[key];
					bool flag2 = jtoken2 != null && jtoken2.Type == 1;
					IL_72:
					bool flag3 = flag2;
					if (flag3)
					{
						SchemaNode schemaNode = this.ResolveSchema(keyValuePair.Value, rootSchema);
						bool flag4 = schemaNode.TypePropertyKey != null;
						if (flag4)
						{
							JToken jtoken3 = targetObject[key][schemaNode.TypePropertyKey];
							bool flag5 = jtoken3 != null && jtoken3.Type == 8;
							if (flag5)
							{
								this.TryResolveTypeSchema((string)targetObject[key][schemaNode.TypePropertyKey], ref schemaNode, rootSchema);
							}
							else
							{
								JToken jtoken4 = parentObject[key][schemaNode.TypePropertyKey];
								bool flag6 = jtoken4 != null && jtoken4.Type == 8;
								if (flag6)
								{
									targetObject[key][schemaNode.TypePropertyKey] = parentObject[key][schemaNode.TypePropertyKey];
									this.TryResolveTypeSchema((string)parentObject[key][schemaNode.TypePropertyKey], ref schemaNode, rootSchema);
								}
							}
						}
						bool mergesProperties = schemaNode.MergesProperties;
						if (mergesProperties)
						{
							this.MergeParentObject(schemaNode, (JObject)targetObject[key], (JObject)parentObject[key], rootSchema);
						}
					}
					continue;
					IL_71:
					flag2 = false;
					goto IL_72;
				}
				bool flag7 = keyValuePair.Value.InheritsProperty && parentObject.ContainsKey(key);
				if (flag7)
				{
					targetObject[key] = parentObject[key];
				}
			}
		}

		// Token: 0x06005D81 RID: 23937 RVA: 0x001DC870 File Offset: 0x001DAA70
		public void ApplyAssetInheritance(SchemaNode schema, JObject targetAsset, Dictionary<string, TrackedAsset> jsonAssets, SchemaNode rootSchema)
		{
			bool flag = schema.TypePropertyKey != null;
			if (flag)
			{
				bool flag2;
				if (schema.HasParentProperty)
				{
					JToken jtoken = targetAsset["Parent"];
					flag2 = (jtoken != null && jtoken.Type == 8);
				}
				else
				{
					flag2 = false;
				}
				bool flag3 = flag2;
				if (flag3)
				{
					SchemaNode schemaNode = this.ResolveSchema(schema.TypeSchemas[0], rootSchema);
					string assetType = schemaNode.Properties["Parent"].AssetType;
					AssetTypeConfig assetTypeConfig;
					bool flag4 = !this.AssetTypeRegistry.AssetTypes.TryGetValue(assetType, out assetTypeConfig);
					if (!flag4)
					{
						string key;
						TrackedAsset trackedAsset;
						bool flag5 = !this.Assets.TryGetPathForAssetId(assetTypeConfig.IdProvider ?? assetType, (string)targetAsset["Parent"], out key, false) || !jsonAssets.TryGetValue(key, out trackedAsset);
						if (!flag5)
						{
							JObject jobject = (JObject)((JObject)trackedAsset.Data).DeepClone();
							bool flag6 = assetType == "BlockType";
							if (flag6)
							{
								jobject = (JObject)jobject["BlockType"];
							}
							this.ApplyAssetInheritance(schema, jobject, jsonAssets, rootSchema);
							this.MergeParentObject(schema, targetAsset, jobject, rootSchema);
						}
					}
				}
			}
			else
			{
				bool flag7;
				if (targetAsset.ContainsKey("Parent"))
				{
					SchemaNode schemaNode2 = schema.Properties["Parent"];
					flag7 = (schemaNode2 != null && schemaNode2.IsParentProperty);
				}
				else
				{
					flag7 = false;
				}
				bool flag8 = flag7;
				if (flag8)
				{
					SchemaNode schemaNode3 = schema.Properties["Parent"];
					AssetTypeConfig assetTypeConfig2;
					string key2;
					TrackedAsset trackedAsset2;
					bool flag9 = this.AssetTypeRegistry.AssetTypes.TryGetValue(schemaNode3.AssetType, out assetTypeConfig2) && this.Assets.TryGetPathForAssetId(assetTypeConfig2.IdProvider ?? schemaNode3.AssetType, (string)targetAsset["Parent"], out key2, false) && jsonAssets.TryGetValue(key2, out trackedAsset2);
					if (flag9)
					{
						JObject jobject2 = (JObject)((JObject)trackedAsset2.Data).DeepClone();
						bool flag10 = schemaNode3.AssetType == "BlockType";
						if (flag10)
						{
							jobject2 = (JObject)jobject2["BlockType"];
						}
						this.ApplyAssetInheritance(schema, jobject2, jsonAssets, rootSchema);
						this.MergeParentObject(schema, targetAsset, jobject2, rootSchema);
					}
				}
				foreach (KeyValuePair<string, JToken> keyValuePair in targetAsset)
				{
					SchemaNode schemaNode4;
					bool flag11 = !schema.Properties.TryGetValue(keyValuePair.Key, out schemaNode4);
					if (!flag11)
					{
						schemaNode4 = this.ResolveSchema(schemaNode4, rootSchema);
						JToken value = keyValuePair.Value;
						bool flag12 = value != null && value.Type == 1;
						if (flag12)
						{
							bool flag13 = schemaNode4.Type == SchemaNode.NodeType.AssetReferenceOrInline;
							if (flag13)
							{
								schemaNode4 = this.ResolveSchema(schemaNode4.Value, rootSchema);
							}
							bool flag14 = schemaNode4.Type == SchemaNode.NodeType.Object;
							if (flag14)
							{
								this.ApplyAssetInheritance(schemaNode4, (JObject)keyValuePair.Value, jsonAssets, rootSchema);
							}
						}
					}
				}
			}
		}

		// Token: 0x04003A43 RID: 14915
		public readonly AssetTypeRegistry AssetTypeRegistry;

		// Token: 0x04003A44 RID: 14916
		public readonly AssetList Assets;

		// Token: 0x04003A45 RID: 14917
		private IReadOnlyDictionary<string, SchemaNode> _schemas = new Dictionary<string, SchemaNode>();

		// Token: 0x04003A47 RID: 14919
		public readonly Dictionary<string, TrackedAsset> TrackedAssets = new Dictionary<string, TrackedAsset>();

		// Token: 0x04003A48 RID: 14920
		private int _errorCount;

		// Token: 0x04003A49 RID: 14921
		private int _warningCount;

		// Token: 0x04003A4B RID: 14923
		private const string Version = "0.1.0";

		// Token: 0x04003A4C RID: 14924
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003A4D RID: 14925
		public readonly AssetTree DropdownAssetTree;

		// Token: 0x04003A4E RID: 14926
		private readonly AssetTree _serverAssetTree;

		// Token: 0x04003A4F RID: 14927
		private readonly AssetTree _commonAssetTree;

		// Token: 0x04003A50 RID: 14928
		private readonly AssetTree _cosmeticsAssetTree;

		// Token: 0x04003A51 RID: 14929
		private Group _rootContainer;

		// Token: 0x04003A52 RID: 14930
		private Group _editorPane;

		// Token: 0x04003A53 RID: 14931
		private DynamicPane _contextPane;

		// Token: 0x04003A54 RID: 14932
		private DynamicPane _contentPane;

		// Token: 0x04003A55 RID: 14933
		private TextField _assetBrowserSearchInput;

		// Token: 0x04003A56 RID: 14934
		private Group _assetBrowserSpinner;

		// Token: 0x04003A57 RID: 14935
		private DynamicPane _assetBrowser;

		// Token: 0x04003A58 RID: 14936
		private Element _assetLoadingIndicator;

		// Token: 0x04003A59 RID: 14937
		private Button _errorsInfo;

		// Token: 0x04003A5A RID: 14938
		private Button _warningsInfo;

		// Token: 0x04003A5B RID: 14939
		private Button _validatedInfo;

		// Token: 0x04003A5C RID: 14940
		private Group _footer;

		// Token: 0x04003A5D RID: 14941
		private Group _fileSaveStatusGroup;

		// Token: 0x04003A5E RID: 14942
		private DynamicPane _diagnosticsPane;

		// Token: 0x04003A5F RID: 14943
		private TabNavigation _modeSelection;

		// Token: 0x04003A60 RID: 14944
		private TabNavigation _assetTreeSelection;

		// Token: 0x04003A61 RID: 14945
		private Button _exportButton;

		// Token: 0x04003A62 RID: 14946
		private TextButton _assetsSourceButton;

		// Token: 0x04003A63 RID: 14947
		private Label _modifiedAssetsCountLabel;

		// Token: 0x04003A64 RID: 14948
		private Group _tabs;

		// Token: 0x04003A65 RID: 14949
		private Group _assetPathWarning;

		// Token: 0x04003A66 RID: 14950
		public readonly AssetEditorInterface Interface;

		// Token: 0x04003A67 RID: 14951
		public readonly ConfirmationModal ConfirmationModal;

		// Token: 0x04003A68 RID: 14952
		public readonly PopupMenuLayer Popup;

		// Token: 0x04003A69 RID: 14953
		public readonly RenameModal RenameModal;

		// Token: 0x04003A6A RID: 14954
		public readonly CreateAssetModal CreateAssetModal;

		// Token: 0x04003A6B RID: 14955
		public readonly ChangelogModal ChangelogModal;

		// Token: 0x04003A6C RID: 14956
		public readonly AutoCompleteMenu AutoCompleteMenu;

		// Token: 0x04003A6D RID: 14957
		public readonly TextTooltipLayer TextTooltipLayer;

		// Token: 0x04003A6E RID: 14958
		public readonly WeatherDaytimeBar WeatherDaytimeBar;

		// Token: 0x04003A6F RID: 14959
		public readonly ConfigEditor ConfigEditor;

		// Token: 0x04003A70 RID: 14960
		public readonly ConfigEditorContextPane ConfigEditorContextPane;

		// Token: 0x04003A71 RID: 14961
		public readonly ExportModal ExportModal;

		// Token: 0x04003A72 RID: 14962
		public readonly FilterModal FilterModal;

		// Token: 0x04003A73 RID: 14963
		public readonly IconMassExportModal IconMassExportModal;

		// Token: 0x04003A75 RID: 14965
		private readonly SourceEditor _sourceEditor;

		// Token: 0x04003A76 RID: 14966
		private readonly HashSet<string> DisplayedCosmeticAssetTypes = new HashSet<string>();

		// Token: 0x04003A77 RID: 14967
		private readonly HashSet<string> DisplayedServerAssetTypes = new HashSet<string>();

		// Token: 0x04003A78 RID: 14968
		private readonly HashSet<string> DisplayedCommonAssetTypes = new HashSet<string>();

		// Token: 0x04003A7B RID: 14971
		private bool _isDiagnosticsPaneOpen;

		// Token: 0x04003A7C RID: 14972
		private AssetEditorOverlay.AssetToOpen _assetToOpenOnceAssetFilesInitialized;

		// Token: 0x04003A7D RID: 14973
		private bool _areAssetFilesInitialized;

		// Token: 0x04003A7E RID: 14974
		private bool _areAssetTypesAndSchemasInitialized;

		// Token: 0x04003A7F RID: 14975
		private AssetEditorOverlay.SaveStatus _fileSaveStatus = AssetEditorOverlay.SaveStatus.Disabled;

		// Token: 0x04003A80 RID: 14976
		private int _modifiedAssetsCount;

		// Token: 0x04003A81 RID: 14977
		private Dictionary<string, List<DropdownBox>> _dropdownBoxesWithDataset = new Dictionary<string, List<DropdownBox>>();

		// Token: 0x04003A82 RID: 14978
		private CancellationTokenSource _reloadInheritanceStackCancellationTokenSource;

		// Token: 0x04003A83 RID: 14979
		private CancellationTokenSource _currentAssetCancellationToken;

		// Token: 0x04003A84 RID: 14980
		private bool _awaitingInitialEditorSetup;

		// Token: 0x02000FB9 RID: 4025
		private class AssetToOpen
		{
			// Token: 0x04004BCF RID: 19407
			public AssetIdReference Id;

			// Token: 0x04004BD0 RID: 19408
			public string FilePath;
		}

		// Token: 0x02000FBA RID: 4026
		public enum SaveStatus
		{
			// Token: 0x04004BD2 RID: 19410
			Disabled,
			// Token: 0x04004BD3 RID: 19411
			Saved,
			// Token: 0x04004BD4 RID: 19412
			Unsaved,
			// Token: 0x04004BD5 RID: 19413
			Saving
		}

		// Token: 0x02000FBB RID: 4027
		public enum EditorMode
		{
			// Token: 0x04004BD7 RID: 19415
			Editor,
			// Token: 0x04004BD8 RID: 19416
			Source
		}
	}
}
