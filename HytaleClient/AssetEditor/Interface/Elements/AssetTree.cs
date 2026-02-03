using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Interface.Editor;
using HytaleClient.AssetEditor.Utils;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Fonts;
using HytaleClient.Interface.Messages;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.AssetEditor.Interface.Elements
{
	// Token: 0x02000BA7 RID: 2983
	internal class AssetTree : Element
	{
		// Token: 0x170013A6 RID: 5030
		// (get) Token: 0x06005C6E RID: 23662 RVA: 0x001D1924 File Offset: 0x001CFB24
		// (set) Token: 0x06005C6F RID: 23663 RVA: 0x001D192C File Offset: 0x001CFB2C
		public ScrollbarStyle ScrollbarStyle
		{
			get
			{
				return this._scrollbarStyle;
			}
			set
			{
				this._scrollbarStyle = value;
			}
		}

		// Token: 0x170013A7 RID: 5031
		// (set) Token: 0x06005C70 RID: 23664 RVA: 0x001D1935 File Offset: 0x001CFB35
		public TextTooltipStyle TooltipStyle
		{
			set
			{
				this._tooltip.Style = value;
			}
		}

		// Token: 0x06005C71 RID: 23665 RVA: 0x001D1944 File Offset: 0x001CFB44
		public AssetTree(AssetEditorOverlay assetEditorOverlay, string rootPath, Element parent) : base(assetEditorOverlay.Desktop, parent)
		{
			this._assetEditorOverlay = assetEditorOverlay;
			this._rootPath = rootPath;
			this._scrollbarStyle.Size = 8;
			this._scrollbarStyle.Spacing = 0;
			this.FlexWeight = 1;
			this._tooltip = new TextTooltipLayer(this.Desktop)
			{
				ShowDelay = 1.5f
			};
		}

		// Token: 0x06005C72 RID: 23666 RVA: 0x001D1A08 File Offset: 0x001CFC08
		protected override void OnUnmounted()
		{
			bool flag = this.Desktop.FocusedElement == this;
			if (flag)
			{
				this.Desktop.FocusElement(null, true);
			}
			this._focusedEntryIndex = -1;
			this._hoveredEntryIndex = -1;
			this._tooltip.Stop();
		}

		// Token: 0x06005C73 RID: 23667 RVA: 0x001D1A50 File Offset: 0x001CFC50
		private void PrepareFilters(out string[] searchKeywords, out HashSet<string> directoryEntriesoDisplay, out HashSet<string> directoriesToDisplay)
		{
			List<string> list = (this.DirectoriesToDisplay != null) ? new List<string>(this.DirectoriesToDisplay) : new List<string>();
			string text = this.SearchQuery.Trim();
			string text2 = null;
			Match match = AssetTree.DirectoryFilterRegex.Match(text);
			bool success = match.Success;
			if (success)
			{
				text = text.Replace(match.Groups[0].Value, "");
				string text3 = match.Groups[1].Value.Trim();
				bool flag = !text3.StartsWith("/");
				if (flag)
				{
					text3 = text3.ToLowerInvariant();
					foreach (AssetTypeConfig assetTypeConfig in this._assetEditorOverlay.AssetTypeRegistry.AssetTypes.Values)
					{
						bool flag2 = assetTypeConfig.Id.ToLowerInvariant() != text3;
						if (!flag2)
						{
							text2 = assetTypeConfig.Path;
							break;
						}
					}
				}
				else
				{
					text2 = text3.TrimStart(new char[]
					{
						'/'
					});
				}
			}
			bool flag3 = text2 != null;
			if (flag3)
			{
				bool flag4 = list.Count == 0 || AssetPathUtils.IsAnyDirectory(text2, list);
				if (flag4)
				{
					list.Add(text2);
				}
			}
			bool flag5 = list.Count > 0;
			if (flag5)
			{
				directoryEntriesoDisplay = new HashSet<string>();
				directoriesToDisplay = new HashSet<string>();
				foreach (string text4 in list)
				{
					directoriesToDisplay.Add(text4);
					string[] array = text4.Split(new char[]
					{
						'/'
					});
					string text5 = "";
					foreach (string str in array)
					{
						bool flag6 = text5 != "";
						if (flag6)
						{
							text5 += "/";
						}
						text5 += str;
						directoryEntriesoDisplay.Add(text5);
					}
				}
			}
			else
			{
				directoryEntriesoDisplay = null;
				directoriesToDisplay = null;
			}
			string[] array3;
			if (!(text != ""))
			{
				array3 = null;
			}
			else
			{
				array3 = Enumerable.ToArray<string>(Enumerable.Select<string, string>(text.ToLowerInvariant().Split(new char[]
				{
					' '
				}), (string k) => k.Trim()));
			}
			searchKeywords = array3;
		}

		// Token: 0x06005C74 RID: 23668 RVA: 0x001D1CF0 File Offset: 0x001CFEF0
		public void BuildTree()
		{
			this._activeEntryIndex = -1;
			string[] array;
			HashSet<string> hashSet;
			HashSet<string> hashSet2;
			this.PrepareFilters(out array, out hashSet, out hashSet2);
			List<AssetTree.AssetTreeEntry> list = new List<AssetTree.AssetTreeEntry>();
			int num = -this._rootPath.Split(new char[]
			{
				'/'
			}).Length;
			bool flag = false;
			string value = "";
			List<AssetTree.AssetTreeEntry> list2 = new List<AssetTree.AssetTreeEntry>();
			int num2 = -1;
			foreach (AssetFile assetFile in this._assetFiles)
			{
				bool flag2 = hashSet != null;
				if (flag2)
				{
					bool flag3 = assetFile.IsDirectory && hashSet.Contains(assetFile.Path);
					if (flag3)
					{
						bool flag4 = hashSet2.Contains(assetFile.Path);
						if (flag4)
						{
							num2 = assetFile.PathElements.Length;
						}
					}
					else
					{
						bool flag5 = num2 == -1;
						if (flag5)
						{
							continue;
						}
						bool flag6 = assetFile.PathElements.Length <= num2;
						if (flag6)
						{
							num2 = -1;
							continue;
						}
					}
				}
				bool flag7 = array != null && !assetFile.IsDirectory;
				if (flag7)
				{
					string text = assetFile.DisplayName.ToLowerInvariant();
					bool flag8 = true;
					foreach (string value2 in array)
					{
						bool flag9 = text.Contains(value2);
						if (!flag9)
						{
							flag8 = false;
							break;
						}
					}
					bool flag10 = !flag8;
					if (flag10)
					{
						continue;
					}
				}
				bool isDirectory = assetFile.IsDirectory;
				AssetTree.AssetTreeEntry assetTreeEntry;
				if (isDirectory)
				{
					bool flag11 = flag;
					if (flag11)
					{
						bool flag12 = assetFile.Path.StartsWith(value);
						if (flag12)
						{
							continue;
						}
					}
					flag = (array == null && !this._uncollapsedEntries.Contains(assetFile.Path));
					AssetTypeConfig assetTypeConfig;
					bool flag13 = assetFile.AssetType != null && this._assetEditorOverlay.AssetTypeRegistry.AssetTypes.TryGetValue(assetFile.AssetType, out assetTypeConfig);
					if (flag13)
					{
						bool flag14 = assetTypeConfig.AssetTree == AssetTreeFolder.Cosmetics;
						if (flag14)
						{
							value = assetFile.Path + "#";
						}
						else
						{
							value = assetFile.Path + "/";
						}
						bool flag15 = !this.ShowVirtualAssets && assetTypeConfig.IsVirtual;
						if (flag15)
						{
							flag = true;
							continue;
						}
					}
					else
					{
						value = assetFile.Path + "/";
					}
					bool flag16 = assetFile.AssetType != null;
					if (flag16)
					{
						assetTreeEntry = new AssetTree.AssetTreeEntry(assetFile.DisplayName, assetFile.Path, num + assetFile.PathElements.Length - 1, AssetTree.AssetTreeEntryType.Type, assetFile.AssetType, flag);
					}
					else
					{
						assetTreeEntry = new AssetTree.AssetTreeEntry(assetFile.DisplayName, assetFile.Path, num + assetFile.PathElements.Length - 1, AssetTree.AssetTreeEntryType.Folder, null, flag);
					}
				}
				else
				{
					bool flag17 = flag;
					if (flag17)
					{
						bool flag18 = assetFile.Path.StartsWith(value);
						if (flag18)
						{
							continue;
						}
						flag = false;
					}
					bool flag19 = this.AssetTypesToDisplay != null && !this.AssetTypesToDisplay.Contains(assetFile.AssetType);
					if (flag19)
					{
						continue;
					}
					assetTreeEntry = new AssetTree.AssetTreeEntry(assetFile.DisplayName, assetFile.Path, num + assetFile.PathElements.Length - 1, AssetTree.AssetTreeEntryType.File, assetFile.AssetType, false);
				}
				bool flag20 = array != null;
				if (flag20)
				{
					bool isDirectory2 = assetFile.IsDirectory;
					if (isDirectory2)
					{
						list2.Add(assetTreeEntry);
					}
					else
					{
						bool flag21 = list2.Count > 0;
						if (flag21)
						{
							foreach (AssetTree.AssetTreeEntry assetTreeEntry2 in list2)
							{
								bool flag22 = assetTreeEntry.Path.StartsWith(assetTreeEntry2.Path);
								if (flag22)
								{
									list.Add(assetTreeEntry2);
								}
							}
							list2.Clear();
						}
						list.Add(assetTreeEntry);
					}
				}
				else
				{
					list.Add(assetTreeEntry);
				}
				bool flag23 = assetTreeEntry.Type == AssetTree.AssetTreeEntryType.File && assetTreeEntry.Path == this._activeEntry.FilePath;
				if (flag23)
				{
					this._activeEntryIndex = list.Count - 1;
				}
			}
			this._entries = list.ToArray();
		}

		// Token: 0x06005C75 RID: 23669 RVA: 0x001D2180 File Offset: 0x001D0380
		public void UpdateFiles(List<AssetFile> files, string rootPath = null)
		{
			bool flag = rootPath != null;
			if (flag)
			{
				this._rootPath = rootPath;
			}
			this._assetFiles = files;
			this.BuildTree();
		}

		// Token: 0x06005C76 RID: 23670 RVA: 0x001D21AC File Offset: 0x001D03AC
		private void ToggleCollapsedState(string path)
		{
			bool flag = this._uncollapsedEntries.Contains(path);
			bool flag2 = !flag;
			if (flag2)
			{
				this._uncollapsedEntries.Add(path);
			}
			else
			{
				this._uncollapsedEntries.Remove(path);
			}
			Action<string, bool> collapseStateChanged = this.CollapseStateChanged;
			if (collapseStateChanged != null)
			{
				collapseStateChanged(path, !flag);
			}
		}

		// Token: 0x06005C77 RID: 23671 RVA: 0x001D2204 File Offset: 0x001D0404
		public void SetUncollapsedState(string path, bool uncollapsed, bool bypassCallback = false)
		{
			if (uncollapsed)
			{
				this._uncollapsedEntries.Add(path);
				bool flag = !bypassCallback;
				if (flag)
				{
					Action<string, bool> collapseStateChanged = this.CollapseStateChanged;
					if (collapseStateChanged != null)
					{
						collapseStateChanged(path, true);
					}
				}
			}
			else
			{
				this._uncollapsedEntries.Remove(path);
				bool flag2 = !bypassCallback;
				if (flag2)
				{
					Action<string, bool> collapseStateChanged2 = this.CollapseStateChanged;
					if (collapseStateChanged2 != null)
					{
						collapseStateChanged2(path, false);
					}
				}
			}
		}

		// Token: 0x06005C78 RID: 23672 RVA: 0x001D226F File Offset: 0x001D046F
		public void ClearCollapsedStates()
		{
			this._uncollapsedEntries.Clear();
		}

		// Token: 0x06005C79 RID: 23673 RVA: 0x001D2280 File Offset: 0x001D0480
		public void SelectEntry(AssetReference assetReference, bool bringIntoView = false)
		{
			this._activeEntry = assetReference;
			this._activeEntryIndex = -1;
			for (int i = 0; i < this._entries.Length; i++)
			{
				AssetTree.AssetTreeEntry assetTreeEntry = this._entries[i];
				bool flag = assetTreeEntry.Path == this._activeEntry.FilePath;
				if (flag)
				{
					this._activeEntryIndex = i;
					break;
				}
			}
			if (bringIntoView)
			{
				bool flag2 = this._activeEntryIndex != -1;
				if (flag2)
				{
					this.ScrollEntryIntoView(this._activeEntryIndex);
				}
				else
				{
					this.BringEntryIntoView(assetReference);
				}
			}
		}

		// Token: 0x06005C7A RID: 23674 RVA: 0x001D2314 File Offset: 0x001D0514
		public void BringEntryIntoView(AssetReference assetReference)
		{
			bool flag = this.SearchQuery != "";
			if (!flag)
			{
				AssetFile assetFile = null;
				foreach (AssetFile assetFile2 in this._assetFiles)
				{
					bool flag2 = assetFile2.Path != assetReference.FilePath;
					if (!flag2)
					{
						assetFile = assetFile2;
						break;
					}
				}
				bool flag3 = assetFile == null;
				if (!flag3)
				{
					string text = "";
					foreach (string str in assetFile.PathElements)
					{
						bool flag4 = text != "";
						if (flag4)
						{
							text += "/";
						}
						text += str;
						this.SetUncollapsedState(text, true, false);
					}
					this.BuildTree();
					base.Layout(null, true);
					this.ScrollEntryIntoView(this.GetEntryIndex(assetFile.Path));
				}
			}
		}

		// Token: 0x06005C7B RID: 23675 RVA: 0x001D2438 File Offset: 0x001D0638
		private int GetEntryIndex(string filePath)
		{
			for (int i = 0; i < this._entries.Length; i++)
			{
				AssetTree.AssetTreeEntry assetTreeEntry = this._entries[i];
				bool flag = assetTreeEntry.Type == AssetTree.AssetTreeEntryType.File && assetTreeEntry.Path == filePath;
				if (flag)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06005C7C RID: 23676 RVA: 0x001D2490 File Offset: 0x001D0690
		public void DeselectEntry()
		{
			this._activeEntry = AssetReference.None;
			this._activeEntryIndex = -1;
		}

		// Token: 0x06005C7D RID: 23677 RVA: 0x001D24A8 File Offset: 0x001D06A8
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			this._fontFamily = this.Desktop.Provider.GetFontFamily("Default");
			this._iconPatches.Clear();
			foreach (AssetTypeConfig assetTypeConfig in this._assetEditorOverlay.AssetTypeRegistry.AssetTypes.Values)
			{
				TexturePatch texturePatch = this.Desktop.MakeTexturePatch(assetTypeConfig.Icon);
				bool flag = !assetTypeConfig.IsColoredIcon;
				if (flag)
				{
					texturePatch.Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 125);
				}
				this._iconPatches.Add(assetTypeConfig.Id, texturePatch);
			}
			this._missingIconPatch = this.Desktop.MakeTexturePatch(new PatchStyle(this.Desktop.Provider.MissingTexture));
			this._folderIconPatch = this.Desktop.MakeTexturePatch(new PatchStyle("AssetEditor/AssetIcons/Folder.png"));
			this._collapseIconPatch = this.Desktop.MakeTexturePatch(new PatchStyle("Common/CaretUncollapsed.png"));
			this._uncollapseIconPatch = this.Desktop.MakeTexturePatch(new PatchStyle("Common/CaretCollapsed.png"));
		}

		// Token: 0x06005C7E RID: 23678 RVA: 0x001D25F8 File Offset: 0x001D07F8
		protected override void LayoutSelf()
		{
			this.ContentHeight = new int?(this.Desktop.UnscaleRound((float)(this._entries.Length * this.Desktop.ScaleRound((float)this._rowHeight))));
		}

		// Token: 0x06005C7F RID: 23679 RVA: 0x001D2630 File Offset: 0x001D0830
		public override Element HitTest(Point position)
		{
			bool flag = !base.Visible || !this._rectangleAfterPadding.Contains(position);
			Element result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = this;
			}
			return result;
		}

		// Token: 0x06005C80 RID: 23680 RVA: 0x001D2668 File Offset: 0x001D0868
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			this.RefreshHoveredEntry();
			bool flag = this._hoveredEntryIndex == -1;
			if (!flag)
			{
				uint button = (uint)evt.Button;
				uint num = button;
				if (num != 1U)
				{
					if (num == 3U)
					{
						bool popupMenuEnabled = this.PopupMenuEnabled;
						if (popupMenuEnabled)
						{
							this.OpenPopupMenu(this._entries[this._hoveredEntryIndex]);
						}
					}
				}
				else
				{
					this._focusedEntryIndex = this._hoveredEntryIndex;
					AssetTree.AssetTreeEntry assetTreeEntry = this._entries[this._hoveredEntryIndex];
					AssetTree.AssetTreeEntryType type = assetTreeEntry.Type;
					AssetTree.AssetTreeEntryType assetTreeEntryType = type;
					if (assetTreeEntryType > AssetTree.AssetTreeEntryType.Folder)
					{
						if (assetTreeEntryType == AssetTree.AssetTreeEntryType.File)
						{
							Action<AssetTree.AssetTreeEntry> fileEntryActivating = this.FileEntryActivating;
							if (fileEntryActivating != null)
							{
								fileEntryActivating(assetTreeEntry);
							}
						}
					}
					else
					{
						bool flag2 = this.SearchQuery != "";
						if (!flag2)
						{
							this.ToggleCollapsedState(assetTreeEntry.Path);
							this.BuildTree();
							base.Layout(null, true);
						}
					}
				}
			}
		}

		// Token: 0x06005C81 RID: 23681 RVA: 0x001D275C File Offset: 0x001D095C
		private void OpenPopupMenu(AssetTree.AssetTreeEntry entry)
		{
			AssetTree.<>c__DisplayClass48_0 CS$<>8__locals1 = new AssetTree.<>c__DisplayClass48_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.entry = entry;
			CS$<>8__locals1.popup = this._assetEditorOverlay.Popup;
			bool isMounted = CS$<>8__locals1.popup.IsMounted;
			if (isMounted)
			{
				CS$<>8__locals1.popup.Close();
			}
			CS$<>8__locals1.popup.SetTitle(null);
			switch (CS$<>8__locals1.entry.Type)
			{
			case AssetTree.AssetTreeEntryType.Type:
				CS$<>8__locals1.popup.SetItems(new List<PopupMenuItem>
				{
					new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.assetBrowser.create", new Dictionary<string, string>
					{
						{
							"assetType",
							CS$<>8__locals1.entry.Name
						}
					}, true), delegate()
					{
						CS$<>8__locals1.<>4__this.OpenCreateAssetModal(CS$<>8__locals1.entry.AssetType, null, null);
					}, null, null),
					new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.assetBrowser.findAsset", null, true), delegate()
					{
						CS$<>8__locals1.popup.Close();
						Action<string> selectingDirectoryFilter = CS$<>8__locals1.<>4__this.SelectingDirectoryFilter;
						if (selectingDirectoryFilter != null)
						{
							selectingDirectoryFilter(CS$<>8__locals1.entry.AssetType);
						}
					}, null, null)
				});
				break;
			case AssetTree.AssetTreeEntryType.Folder:
			{
				List<PopupMenuItem> list = new List<PopupMenuItem>();
				bool flag = false;
				List<string> list2;
				bool flag2 = this._assetEditorOverlay.AssetTypeRegistry.TryGetAssetTypesFromDirectoryPath(CS$<>8__locals1.entry.Path + "/", out list2);
				if (flag2)
				{
					using (List<string>.Enumerator enumerator = list2.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							AssetTree.<>c__DisplayClass48_1 CS$<>8__locals2 = new AssetTree.<>c__DisplayClass48_1();
							CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
							CS$<>8__locals2.assetType = enumerator.Current;
							AssetTypeConfig assetTypeConfig;
							bool flag3 = !this._assetEditorOverlay.AssetTypeRegistry.AssetTypes.TryGetValue(CS$<>8__locals2.assetType, out assetTypeConfig);
							if (!flag3)
							{
								list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.assetBrowser.createInFolder", new Dictionary<string, string>
								{
									{
										"assetType",
										assetTypeConfig.Name
									}
								}, true), delegate()
								{
									CS$<>8__locals2.CS$<>8__locals1.<>4__this.OpenCreateAssetModal(CS$<>8__locals2.assetType, null, CS$<>8__locals2.CS$<>8__locals1.entry.Path.Replace(assetTypeConfig.Path + "/", ""));
								}, null, null));
								bool flag4 = CS$<>8__locals2.CS$<>8__locals1.entry.Path.StartsWith(assetTypeConfig.Path + "/");
								if (flag4)
								{
									flag = true;
								}
							}
						}
					}
				}
				list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.assetBrowser.findInFolder", null, true), delegate()
				{
					CS$<>8__locals1.popup.Close();
					Action<string> selectingDirectoryFilter = CS$<>8__locals1.<>4__this.SelectingDirectoryFilter;
					if (selectingDirectoryFilter != null)
					{
						selectingDirectoryFilter("/" + CS$<>8__locals1.entry.Path);
					}
				}, null, null));
				bool flag5 = flag;
				if (flag5)
				{
					list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.assetBrowser.rename", null, true), delegate()
					{
						CS$<>8__locals1.<>4__this.OpenRenameDirectoryModal(CS$<>8__locals1.entry.Path);
					}, null, null));
					list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.assetBrowser.delete", null, true), delegate()
					{
						CS$<>8__locals1.<>4__this.OpenDeleteDirectoryPrompt(CS$<>8__locals1.entry.Path);
					}, null, null));
				}
				CS$<>8__locals1.popup.SetItems(list);
				break;
			}
			case AssetTree.AssetTreeEntryType.File:
			{
				List<PopupMenuItem> items = new List<PopupMenuItem>();
				this._assetEditorOverlay.SetupAssetPopup(new AssetReference(CS$<>8__locals1.entry.AssetType, CS$<>8__locals1.entry.Path), items);
				CS$<>8__locals1.popup.SetItems(items);
				break;
			}
			}
			CS$<>8__locals1.popup.Open();
		}

		// Token: 0x06005C82 RID: 23682 RVA: 0x001D2AE8 File Offset: 0x001D0CE8
		private void OpenDeleteDirectoryPrompt(string path)
		{
			string text = this.Desktop.Provider.GetText("ui.assetEditor.deleteDirectoryModal.text", new Dictionary<string, string>
			{
				{
					"path",
					path
				}
			}, true);
			this._assetEditorOverlay.ConfirmationModal.Open(this.Desktop.Provider.GetText("ui.assetEditor.deleteDirectoryModal.title", null, true), text, delegate
			{
				this._assetEditorOverlay.Backend.DeleteDirectory(path, this._assetEditorOverlay.ConfirmationModal.ApplyChangesLocally, new Action<string, FormattedMessage>(base.<OpenDeleteDirectoryPrompt>g__OnDeleted|0));
			}, null, this.Desktop.Provider.GetText("ui.assetEditor.deleteDirectoryModal.confirmButton", null, true), null, this._assetEditorOverlay.Backend.IsEditingRemotely);
		}

		// Token: 0x06005C83 RID: 23683 RVA: 0x001D2B97 File Offset: 0x001D0D97
		private void OpenCreateAssetModal(string assetType, string assetToCopyPath = null, string path = null)
		{
			this._assetEditorOverlay.CreateAssetModal.Open(assetType, assetToCopyPath, path, null, null, null);
		}

		// Token: 0x06005C84 RID: 23684 RVA: 0x001D2BB1 File Offset: 0x001D0DB1
		private void OpenRenameDirectoryModal(string path)
		{
			this._assetEditorOverlay.RenameModal.OpenForDirectory(path, this._assetEditorOverlay.Backend.IsEditingRemotely);
		}

		// Token: 0x06005C85 RID: 23685 RVA: 0x001D2BD6 File Offset: 0x001D0DD6
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			this.Desktop.FocusElement(this, true);
		}

		// Token: 0x06005C86 RID: 23686 RVA: 0x001D2BE8 File Offset: 0x001D0DE8
		protected internal override void OnKeyDown(SDL.SDL_Keycode keyCode, int repeat)
		{
			bool flag = this.Desktop.IsShortcutKeyDown && keyCode == SDL.SDL_Keycode.SDLK_f;
			if (flag)
			{
				this.FocusSearch();
			}
			else if (keyCode != SDL.SDL_Keycode.SDLK_RETURN)
			{
				switch (keyCode)
				{
				case SDL.SDL_Keycode.SDLK_RIGHT:
				{
					bool flag2 = this._focusedEntryIndex != -1 && this.SearchQuery == "";
					if (flag2)
					{
						AssetTree.AssetTreeEntry assetTreeEntry = this._entries[this._focusedEntryIndex];
						bool flag3 = assetTreeEntry.Type != AssetTree.AssetTreeEntryType.File;
						if (flag3)
						{
							this.SetUncollapsedState(assetTreeEntry.Path, true, false);
							this.BuildTree();
							base.Layout(null, true);
						}
					}
					break;
				}
				case SDL.SDL_Keycode.SDLK_LEFT:
				{
					bool flag4 = this._focusedEntryIndex != -1 && this.SearchQuery == "";
					if (flag4)
					{
						AssetTree.AssetTreeEntry assetTreeEntry2 = this._entries[this._focusedEntryIndex];
						bool flag5 = assetTreeEntry2.Type != AssetTree.AssetTreeEntryType.File;
						if (flag5)
						{
							this.SetUncollapsedState(assetTreeEntry2.Path, false, false);
							this.BuildTree();
							base.Layout(null, true);
						}
					}
					break;
				}
				case SDL.SDL_Keycode.SDLK_DOWN:
				{
					bool flag6 = this._focusedEntryIndex == -1 && this._entries.Length > 1;
					if (flag6)
					{
						this._focusedEntryIndex = 1;
					}
					else
					{
						this._focusedEntryIndex++;
						bool flag7 = this._focusedEntryIndex >= this._entries.Length;
						if (flag7)
						{
							this._focusedEntryIndex = 0;
						}
					}
					this.ScrollEntryIntoView(this._focusedEntryIndex);
					break;
				}
				case SDL.SDL_Keycode.SDLK_UP:
				{
					this._focusedEntryIndex--;
					bool flag8 = this._focusedEntryIndex < 0;
					if (flag8)
					{
						this._focusedEntryIndex = this._entries.Length - 1;
					}
					this.ScrollEntryIntoView(this._focusedEntryIndex);
					break;
				}
				}
			}
			else
			{
				bool flag9 = this._focusedEntryIndex != -1;
				if (flag9)
				{
					AssetTree.AssetTreeEntry assetTreeEntry3 = this._entries[this._focusedEntryIndex];
					bool flag10 = assetTreeEntry3.Type == AssetTree.AssetTreeEntryType.File;
					if (flag10)
					{
						Action<AssetTree.AssetTreeEntry> fileEntryActivating = this.FileEntryActivating;
						if (fileEntryActivating != null)
						{
							fileEntryActivating(assetTreeEntry3);
						}
					}
				}
			}
		}

		// Token: 0x06005C87 RID: 23687 RVA: 0x001D2E2E File Offset: 0x001D102E
		protected internal override void OnBlur()
		{
			this._focusedEntryIndex = -1;
		}

		// Token: 0x06005C88 RID: 23688 RVA: 0x001D2E37 File Offset: 0x001D1037
		protected override void OnMouseMove()
		{
			this.RefreshHoveredEntry();
		}

		// Token: 0x06005C89 RID: 23689 RVA: 0x001D2E40 File Offset: 0x001D1040
		protected override void OnMouseEnter()
		{
			this.RefreshHoveredEntry();
		}

		// Token: 0x06005C8A RID: 23690 RVA: 0x001D2E49 File Offset: 0x001D1049
		protected override void OnMouseLeave()
		{
			this.RefreshHoveredEntry();
		}

		// Token: 0x06005C8B RID: 23691 RVA: 0x001D2E54 File Offset: 0x001D1054
		private void RefreshHoveredEntry()
		{
			bool flag = !this._viewRectangle.Contains(this.Desktop.MousePosition);
			if (flag)
			{
				this._tooltip.Stop();
				this._hoveredEntryIndex = -1;
			}
			else
			{
				int num = this.Desktop.ScaleRound((float)this._rowHeight);
				int num2 = (int)((float)(this.Desktop.MousePosition.Y - this._rectangleAfterPadding.Y + this._scaledScrollOffset.Y) / (float)num);
				int hoveredEntryIndex = this._hoveredEntryIndex;
				this._hoveredEntryIndex = ((num2 < this._entries.Length) ? num2 : -1);
				bool flag2 = this._hoveredEntryIndex != hoveredEntryIndex;
				if (flag2)
				{
					bool flag3 = this._hoveredEntryIndex > -1;
					if (flag3)
					{
						AssetTree.AssetTreeEntry assetTreeEntry = this._entries[this._hoveredEntryIndex];
						bool flag4 = assetTreeEntry.Type == AssetTree.AssetTreeEntryType.File;
						if (flag4)
						{
							this._tooltip.TextSpans = new List<Label.LabelSpan>
							{
								new Label.LabelSpan
								{
									Text = this._assetEditorOverlay.AssetTypeRegistry.AssetTypes[assetTreeEntry.AssetType].Name + ": ",
									IsBold = true
								},
								new Label.LabelSpan
								{
									Text = assetTreeEntry.Path
								}
							};
						}
						else
						{
							this._tooltip.Text = assetTreeEntry.Path;
						}
						this._tooltip.Start(true);
						this._tooltip.Layout(null, true);
					}
					else
					{
						this._tooltip.Stop();
					}
				}
			}
		}

		// Token: 0x06005C8C RID: 23692 RVA: 0x001D2FFC File Offset: 0x001D11FC
		private void ScrollEntryIntoView(int entryIndex)
		{
			int num = this.Desktop.ScaleRound((float)this._rowHeight);
			bool flag = num * (entryIndex + 1) > this._anchoredRectangle.Height + this._scaledScrollOffset.Y;
			if (flag)
			{
				base.SetScroll(new int?(0), new int?(num * (entryIndex + 1) - this._anchoredRectangle.Height));
			}
			else
			{
				bool flag2 = num * entryIndex < this._scaledScrollOffset.Y;
				if (flag2)
				{
					base.SetScroll(new int?(0), new int?(num * entryIndex));
				}
			}
		}

		// Token: 0x06005C8D RID: 23693 RVA: 0x001D3094 File Offset: 0x001D1294
		protected override void PrepareForDrawSelf()
		{
			base.PrepareForDrawSelf();
			TextureArea whitePixel = this.Desktop.Provider.WhitePixel;
			UInt32Color color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 64);
			UInt32Color color2 = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 140);
			UInt32Color color3 = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 40);
			UInt32Color colorOverride = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 125);
			UInt32Color color4 = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 30);
			int num = this.Desktop.ScaleRound(5f);
			bool flag = this.SearchQuery.Trim() != "";
			int num2 = this.Desktop.ScaleRound((float)this._rowHeight);
			int height = (int)MathHelper.Max((float)this.Desktop.ScaleRound(1f), 1f);
			int num3 = this.Desktop.ScaleRound(11f);
			int num4 = this.Desktop.ScaleRound(7f);
			int num5 = (num2 - num3) / 2;
			int num6 = this.Desktop.ScaleRound(20f);
			int num7 = this.Desktop.ScaleRound(43f);
			float num8 = 13f * this.Desktop.Scale;
			this.Desktop.Batcher2D.PushScissor(this._rectangleAfterPadding);
			for (int i = 0; i < this._entries.Length; i++)
			{
				AssetTree.AssetTreeEntry assetTreeEntry = this._entries[i];
				int num9 = this._rectangleAfterPadding.Y + num2 * i - this._scaledScrollOffset.Y;
				int num10 = this._rectangleAfterPadding.X + this.Desktop.ScaleRound((float)(5 + 10 * (assetTreeEntry.Indention - 1)));
				bool flag2 = num9 > this._rectangleAfterPadding.Bottom;
				if (!flag2)
				{
					int num11 = num9 + num2;
					bool flag3 = num11 < this._rectangleAfterPadding.Top;
					if (!flag3)
					{
						bool flag4 = i > 0 && this._entries[i - 1].Indention > assetTreeEntry.Indention;
						if (flag4)
						{
							this.Desktop.Batcher2D.RequestDrawTexture(whitePixel.Texture, whitePixel.Rectangle, new Rectangle(num10 + num6, num9, this._rectangleAfterPadding.Width, height), color4);
						}
						bool flag5 = i == this._activeEntryIndex;
						if (flag5)
						{
							this.Desktop.Batcher2D.RequestDrawTexture(whitePixel.Texture, whitePixel.Rectangle, new Rectangle(this._rectangleAfterPadding.X, num9, this._rectangleAfterPadding.Width, num2), color2);
						}
						else
						{
							bool flag6 = i == this._hoveredEntryIndex;
							if (flag6)
							{
								this.Desktop.Batcher2D.RequestDrawTexture(whitePixel.Texture, whitePixel.Rectangle, new Rectangle(this._rectangleAfterPadding.X, num9, this._rectangleAfterPadding.Width, num2), color);
							}
						}
						bool flag7 = i == this._focusedEntryIndex;
						if (flag7)
						{
							this.Desktop.Batcher2D.RequestDrawOutline(whitePixel.Texture, whitePixel.Rectangle, new Rectangle(this._rectangleAfterPadding.X - 10, num9, this._rectangleAfterPadding.Width + 20, num2), 2f, color3);
						}
						Rectangle destRect = new Rectangle(num10 + num6, num9 + num, num2 - num * 2, num2 - num * 2);
						bool flag8 = assetTreeEntry.Type == AssetTree.AssetTreeEntryType.Type;
						if (flag8)
						{
							this.Desktop.Batcher2D.RequestDrawPatch(this._folderIconPatch, destRect, this.Desktop.Scale, colorOverride);
						}
						else
						{
							bool flag9 = assetTreeEntry.Type == AssetTree.AssetTreeEntryType.File;
							if (flag9)
							{
								TexturePatch texturePatch;
								bool flag10 = this._iconPatches.TryGetValue(assetTreeEntry.AssetType, out texturePatch);
								if (flag10)
								{
									this.Desktop.Batcher2D.RequestDrawPatch(texturePatch, destRect, this.Desktop.Scale, texturePatch.Color);
								}
								else
								{
									this.Desktop.Batcher2D.RequestDrawPatch(this._missingIconPatch, destRect, this.Desktop.Scale, colorOverride);
								}
							}
							else
							{
								this.Desktop.Batcher2D.RequestDrawPatch(this._folderIconPatch, destRect, this.Desktop.Scale, colorOverride);
							}
						}
						bool flag11 = assetTreeEntry.Type != AssetTree.AssetTreeEntryType.File && !flag;
						if (flag11)
						{
							Rectangle destRect2 = new Rectangle(num10 + num4, num9 + num5, num3, num3);
							this.Desktop.Batcher2D.RequestDrawPatch(assetTreeEntry.IsCollapsed ? this._uncollapseIconPatch : this._collapseIconPatch, destRect2, this.Desktop.Scale, colorOverride);
						}
						Font font = (assetTreeEntry.Type == AssetTree.AssetTreeEntryType.Type) ? this._fontFamily.BoldFont : this._fontFamily.RegularFont;
						float num12 = num8 / (float)font.BaseSize;
						float num13 = (float)num9 + (float)num2 / 2f;
						float y = num13 - (float)((int)((float)font.Height * num12 / 2f));
						this.Desktop.Batcher2D.RequestDrawText(font, num8, assetTreeEntry.Name, new Vector3((float)(num10 + num7), y, 0f), (assetTreeEntry.Type == AssetTree.AssetTreeEntryType.File) ? UInt32Color.White : UInt32Color.FromRGBA(200, 200, 200, byte.MaxValue), false, false, 0f);
					}
				}
			}
			this.Desktop.Batcher2D.PopScissor();
		}

		// Token: 0x040039E1 RID: 14817
		private static readonly Regex DirectoryFilterRegex = new Regex("^(:?.*):");

		// Token: 0x040039E2 RID: 14818
		private readonly AssetEditorOverlay _assetEditorOverlay;

		// Token: 0x040039E3 RID: 14819
		private TextTooltipLayer _tooltip;

		// Token: 0x040039E4 RID: 14820
		private FontFamily _fontFamily;

		// Token: 0x040039E5 RID: 14821
		private int _rowHeight = 30;

		// Token: 0x040039E6 RID: 14822
		private int _hoveredEntryIndex = -1;

		// Token: 0x040039E7 RID: 14823
		private int _focusedEntryIndex = -1;

		// Token: 0x040039E8 RID: 14824
		private int _activeEntryIndex = -1;

		// Token: 0x040039E9 RID: 14825
		private AssetReference _activeEntry;

		// Token: 0x040039EA RID: 14826
		public Action<AssetTree.AssetTreeEntry> FileEntryActivating;

		// Token: 0x040039EB RID: 14827
		public Action<string> SelectingDirectoryFilter;

		// Token: 0x040039EC RID: 14828
		public Action<string, bool> CollapseStateChanged;

		// Token: 0x040039ED RID: 14829
		public Action FocusSearch;

		// Token: 0x040039EE RID: 14830
		private readonly Dictionary<string, TexturePatch> _iconPatches = new Dictionary<string, TexturePatch>();

		// Token: 0x040039EF RID: 14831
		private TexturePatch _missingIconPatch;

		// Token: 0x040039F0 RID: 14832
		private TexturePatch _folderIconPatch;

		// Token: 0x040039F1 RID: 14833
		private TexturePatch _uncollapseIconPatch;

		// Token: 0x040039F2 RID: 14834
		private TexturePatch _collapseIconPatch;

		// Token: 0x040039F3 RID: 14835
		private AssetTree.AssetTreeEntry[] _entries = new AssetTree.AssetTreeEntry[0];

		// Token: 0x040039F4 RID: 14836
		private List<AssetFile> _assetFiles = new List<AssetFile>();

		// Token: 0x040039F5 RID: 14837
		private readonly HashSet<string> _uncollapsedEntries = new HashSet<string>();

		// Token: 0x040039F6 RID: 14838
		public string SearchQuery = "";

		// Token: 0x040039F7 RID: 14839
		public List<string> DirectoriesToDisplay;

		// Token: 0x040039F8 RID: 14840
		public HashSet<string> AssetTypesToDisplay;

		// Token: 0x040039F9 RID: 14841
		public bool PopupMenuEnabled = true;

		// Token: 0x040039FA RID: 14842
		public bool ShowVirtualAssets;

		// Token: 0x040039FB RID: 14843
		private string _rootPath;

		// Token: 0x02000FA8 RID: 4008
		public class AssetTreeEntry
		{
			// Token: 0x06006964 RID: 26980 RVA: 0x0021E144 File Offset: 0x0021C344
			public AssetTreeEntry(string name, string path, int indention, AssetTree.AssetTreeEntryType type, string assetType, bool isCollapsed)
			{
				this.Name = name;
				this.Path = path;
				this.Indention = indention;
				this.Type = type;
				this.AssetType = assetType;
				this.IsCollapsed = isCollapsed;
			}

			// Token: 0x04004BA0 RID: 19360
			public readonly AssetTree.AssetTreeEntryType Type;

			// Token: 0x04004BA1 RID: 19361
			public readonly string Name;

			// Token: 0x04004BA2 RID: 19362
			public readonly string Path;

			// Token: 0x04004BA3 RID: 19363
			public readonly string AssetType;

			// Token: 0x04004BA4 RID: 19364
			public readonly int Indention;

			// Token: 0x04004BA5 RID: 19365
			public readonly bool IsCollapsed;
		}

		// Token: 0x02000FA9 RID: 4009
		public enum AssetTreeEntryType
		{
			// Token: 0x04004BA7 RID: 19367
			Type,
			// Token: 0x04004BA8 RID: 19368
			Folder,
			// Token: 0x04004BA9 RID: 19369
			File
		}
	}
}
