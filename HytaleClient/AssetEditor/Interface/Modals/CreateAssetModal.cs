using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Interface.Editor;
using HytaleClient.AssetEditor.Interface.Elements;
using HytaleClient.AssetEditor.Utils;
using HytaleClient.Interface.Messages;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HytaleClient.AssetEditor.Interface.Modals
{
	// Token: 0x02000B9F RID: 2975
	internal class CreateAssetModal : Element
	{
		// Token: 0x06005BF5 RID: 23541 RVA: 0x001CD90C File Offset: 0x001CBB0C
		public CreateAssetModal(AssetEditorOverlay assetEditorOverlay) : base(assetEditorOverlay.Desktop, null)
		{
			this._assetEditorOverlay = assetEditorOverlay;
			this.CodeEditor = new WebCodeEditor(assetEditorOverlay.Interface, this.Desktop, null)
			{
				ValueChanged = new Action(this.OnCodeEditorDidChange)
			};
		}

		// Token: 0x06005BF6 RID: 23542 RVA: 0x001CD96C File Offset: 0x001CBB6C
		public void Build()
		{
			base.Clear();
			Element parent = this.CodeEditor.Parent;
			if (parent != null)
			{
				parent.Remove(this.CodeEditor);
			}
			Document document;
			this.Desktop.Provider.TryGetDocument("AssetEditor/CreateAssetModal.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._tabs = uifragment.Get<TabNavigation>("Tabs");
			this._tabs.SelectedTabChanged = delegate()
			{
				this.OnChangeTab((CreateAssetModal.Tab)Enum.Parse(typeof(CreateAssetModal.Tab), this._tabs.SelectedTab));
			};
			this._tabs.SelectedTab = this._activeTab.ToString();
			this._container = uifragment.Get<Group>("Container");
			this._titleLabel = uifragment.Get<Label>("Title");
			this._errorLabel = uifragment.Get<Label>("ErrorMessage");
			this._sourceErrorLabel = uifragment.Get<Label>("SourceErrorMessage");
			this._propertyList = uifragment.Get<Group>("PropertyList");
			this._propertyList.Visible = (this._activeTab == CreateAssetModal.Tab.Properties);
			this._sourceEditor = uifragment.Get<Group>("SourceEditor");
			this._sourceEditor.Visible = (this._activeTab == CreateAssetModal.Tab.Source);
			this._idTextInput = uifragment.Get<TextField>("AssetIdInput");
			this._idTextInput.Validating = new Action(this.Validate);
			this._assetTypeDropdown = uifragment.Get<DropdownBox>("AssetTypeDropdownBox");
			this._assetTypeDropdown.ValueChanged = delegate()
			{
				this.Open(this._assetTypeDropdown.Value, null, null, null, null, null);
				base.Layout(null, true);
			};
			this._copyAssetDropdown = new AssetSelectorDropdown(this.Desktop, uifragment.Get<Group>("CopyAssetGroup"), this._assetEditorOverlay)
			{
				ValueChanged = delegate()
				{
					bool flag = this._copyAssetDropdown.Value == null;
					if (!flag)
					{
						this.LoadAssetToCopy();
					}
				},
				FlexWeight = 1,
				AssetType = this._assetType,
				Style = document.ResolveNamedValue<FileDropdownBoxStyle>(this.Desktop.Provider, "FileDropdownBoxStyle")
			};
			this._folderSelector = new FileDropdownBox(this.Desktop, uifragment.Get<Group>("FolderGroup"), "AssetEditor/FileSelector.ui", () => this.GetFolders(null))
			{
				FlexWeight = 1,
				SelectedFiles = new HashSet<string>(),
				AllowDirectorySelection = true,
				AllowDirectoryCreation = true,
				DropdownToggled = new Action(this.OnToggleDropdown),
				CreatingDirectory = new Action<string, Action<FormattedMessage>>(this.CreateDirectory),
				Style = this._assetEditorOverlay.ConfigEditor.FileDropdownBoxStyle
			};
			this._saveButton = uifragment.Get<TextButton>("SaveButton");
			this._saveButton.Activating = new Action(this.Validate);
			uifragment.Get<TextButton>("CancelButton").Activating = new Action(this.Dismiss);
			uifragment.Get<TextButton>("CopyAssetClearButton").Activating = delegate()
			{
				this._copyAssetDropdown.Value = null;
				this._json = null;
				this.CodeEditor.Value = "{}";
				this._propertyList.Clear();
				this._propertyList.Layout(null, true);
			};
			this._sourceEditor.Add(this.CodeEditor, -1);
		}

		// Token: 0x06005BF7 RID: 23543 RVA: 0x001CDC50 File Offset: 0x001CBE50
		private void CreateDirectory(string name, Action<FormattedMessage> callback)
		{
			AssetTypeConfig assetTypeConfig = this._assetEditorOverlay.AssetTypeRegistry.AssetTypes[this._assetType];
			string path = assetTypeConfig.Path + this._folderSelector.CurrentPath.TrimEnd(new char[]
			{
				'/'
			}) + "/" + name;
			this._assetEditorOverlay.Backend.CreateDirectory(path, false, delegate(string createdPath, FormattedMessage error)
			{
				callback(error);
				bool flag = error == null;
				if (flag)
				{
					this._folderSelector.Setup(this._folderSelector.CurrentPath, this.GetFolders(null));
				}
			});
		}

		// Token: 0x06005BF8 RID: 23544 RVA: 0x001CDCDA File Offset: 0x001CBEDA
		private void OnToggleDropdown()
		{
			this._folderSelector.Setup("/", this.GetFolders("/"));
		}

		// Token: 0x06005BF9 RID: 23545 RVA: 0x001CDCFC File Offset: 0x001CBEFC
		private List<FileSelector.File> GetFolders(string currentPath = null)
		{
			List<FileSelector.File> list = new List<FileSelector.File>();
			string text = this._folderSelector.SearchQuery.Trim();
			bool flag = text != "" && text.Length < 3;
			List<FileSelector.File> result;
			if (flag)
			{
				result = list;
			}
			else
			{
				AssetTypeConfig assetTypeConfig = this._assetEditorOverlay.AssetTypeRegistry.AssetTypes[this._assetType];
				bool flag2 = assetTypeConfig.AssetTree == AssetTreeFolder.Cosmetics;
				if (flag2)
				{
					throw new Exception("Unsupported");
				}
				string path = assetTypeConfig.Path;
				currentPath = (path + (currentPath ?? this._folderSelector.CurrentPath)).Trim(new char[]
				{
					'/'
				});
				string[] array = currentPath.Trim(new char[]
				{
					'/'
				}).Split(new char[]
				{
					'/'
				});
				string[] array2 = Enumerable.ToArray<string>(Enumerable.Select<string, string>(text.ToLower().Split(new char[]
				{
					' '
				}, StringSplitOptions.RemoveEmptyEntries), (string q) => q.Trim()));
				foreach (AssetFile assetFile in this._assetEditorOverlay.Assets.GetAssets(assetTypeConfig.AssetTree))
				{
					bool flag3 = !assetFile.IsDirectory;
					if (!flag3)
					{
						bool flag4 = array2.Length != 0;
						if (flag4)
						{
							bool flag5 = !assetFile.Path.StartsWith(path + "/") && assetFile.Path != path;
							if (!flag5)
							{
								bool flag6 = true;
								string text2 = Enumerable.Last<string>(assetFile.PathElements).ToLowerInvariant();
								foreach (string value in array2)
								{
									bool flag7 = !text2.Contains(value);
									if (flag7)
									{
										flag6 = false;
										break;
									}
								}
								bool flag8 = !flag6;
								if (!flag8)
								{
									list.Add(new FileSelector.File
									{
										IsDirectory = true,
										Name = assetFile.Path.Replace(path + "/", "")
									});
								}
							}
						}
						else
						{
							bool flag9 = assetFile.PathElements.Length == array.Length + 1 && assetFile.Path.StartsWith(currentPath + "/");
							if (flag9)
							{
								list.Add(new FileSelector.File
								{
									IsDirectory = true,
									Name = Enumerable.Last<string>(assetFile.PathElements)
								});
							}
						}
					}
				}
				result = list;
			}
			return result;
		}

		// Token: 0x06005BFA RID: 23546 RVA: 0x001CDFC8 File Offset: 0x001CC1C8
		private void TryBuildPropertyList()
		{
			CreateAssetModal.<>c__DisplayClass25_0 CS$<>8__locals1 = new CreateAssetModal.<>c__DisplayClass25_0();
			CS$<>8__locals1.<>4__this = this;
			this._propertyList.Clear();
			AssetTypeConfig assetTypeConfig = this._assetEditorOverlay.AssetTypeRegistry.AssetTypes[this._assetType];
			bool flag = assetTypeConfig.AssetTree == AssetTreeFolder.Common || assetTypeConfig.Schema == null;
			if (flag)
			{
				bool isMounted = base.IsMounted;
				if (isMounted)
				{
					this._propertyList.Layout(null, true);
				}
			}
			else
			{
				this.Desktop.Provider.TryGetDocument("AssetEditor/CreateAssetModalPropertyEntry.ui", out CS$<>8__locals1.doc);
				SchemaNode schemaNode = this._assetEditorOverlay.ResolveSchema(assetTypeConfig.Schema, assetTypeConfig.Schema);
				bool flag2 = schemaNode.TypeSchemas != null;
				if (flag2)
				{
					JToken jtoken = this._json[schemaNode.TypePropertyKey];
					bool flag3 = jtoken != null && jtoken.Type == 8;
					if (flag3)
					{
						this._assetEditorOverlay.TryResolveTypeSchema((string)this._json[schemaNode.TypePropertyKey], ref schemaNode, schemaNode);
					}
					else
					{
						bool flag4 = schemaNode.DefaultTypeSchema != null;
						if (flag4)
						{
							this._assetEditorOverlay.TryResolveTypeSchema(schemaNode.DefaultTypeSchema, ref schemaNode, schemaNode);
						}
					}
				}
				bool flag5 = schemaNode != null && schemaNode.Type == SchemaNode.NodeType.Object && schemaNode.Properties != null;
				if (flag5)
				{
					CS$<>8__locals1.<TryBuildPropertyList>g__AddProperties|1(schemaNode, this._json, "");
				}
				bool isMounted2 = base.IsMounted;
				if (isMounted2)
				{
					this._propertyList.Layout(null, true);
				}
			}
		}

		// Token: 0x06005BFB RID: 23547 RVA: 0x001CE160 File Offset: 0x001CC360
		private void OnChangeTab(CreateAssetModal.Tab tab)
		{
			bool flag = tab == this._activeTab;
			if (!flag)
			{
				this._sourceErrorLabel.Visible = false;
				bool flag2 = tab == CreateAssetModal.Tab.Properties;
				if (flag2)
				{
					try
					{
						JsonUtils.ValidateJson(this.CodeEditor.Value);
						JObject json = JObject.Parse(this.CodeEditor.Value);
						this._json = json;
						this.TryBuildPropertyList();
					}
					catch (JsonReaderException)
					{
						this._sourceErrorLabel.Visible = true;
						base.Layout(null, true);
						return;
					}
				}
				this._activeTab = tab;
				this._propertyList.Visible = (this._activeTab == CreateAssetModal.Tab.Properties);
				this._sourceEditor.Visible = (this._activeTab == CreateAssetModal.Tab.Source);
				base.Layout(null, true);
			}
		}

		// Token: 0x06005BFC RID: 23548 RVA: 0x001CE240 File Offset: 0x001CC440
		protected override void OnMounted()
		{
			this.Desktop.FocusElement(this._idTextInput, true);
			this._assetEditorOverlay.AttachNotifications(this);
		}

		// Token: 0x06005BFD RID: 23549 RVA: 0x001CE264 File Offset: 0x001CC464
		protected override void OnUnmounted()
		{
			this._json = null;
			this._idTextInput.Value = "";
			this._assetType = null;
			this._errorLabel.Visible = false;
			this._copyAssetDropdown.Value = null;
			this._sourceErrorLabel.Visible = false;
			this._assetEditorOverlay.ReparentNotifications();
		}

		// Token: 0x06005BFE RID: 23550 RVA: 0x001CE2C4 File Offset: 0x001CC4C4
		private void LoadAssetToCopy()
		{
			string filePath;
			bool flag = !this._assetEditorOverlay.Assets.TryGetPathForAssetId(this._assetType, this._copyAssetDropdown.Value, out filePath, false);
			if (!flag)
			{
				this._assetEditorOverlay.Backend.FetchAsset(new AssetReference(this._assetType, filePath), delegate(object asset, FormattedMessage error)
				{
					bool flag2 = asset == null;
					if (flag2)
					{
						this._assetEditorOverlay.ToastNotifications.AddNotification(2, error ?? FormattedMessage.FromMessageId("ui.assetEditor.errors.errorOccurredFetching", null));
					}
					else
					{
						this._json = (JObject)asset;
						this.CodeEditor.Value = asset.ToString();
						this.TryBuildPropertyList();
					}
				}, false);
			}
		}

		// Token: 0x06005BFF RID: 23551 RVA: 0x001CE32C File Offset: 0x001CC52C
		private void OnCodeEditorDidChange()
		{
			bool visible = this._sourceErrorLabel.Visible;
			if (visible)
			{
				this._sourceErrorLabel.Visible = false;
				base.Layout(null, true);
			}
		}

		// Token: 0x06005C00 RID: 23552 RVA: 0x001CE36C File Offset: 0x001CC56C
		public void Open(string assetType, string assetToCopyPath = null, string path = null, string id = null, JObject json = null, Action<string, FormattedMessage> callback = null)
		{
			AssetTypeConfig assetTypeConfig = this._assetEditorOverlay.AssetTypeRegistry.AssetTypes[assetType];
			this._buttonId = null;
			this._assetCreatedCallback = callback;
			this._assetType = assetType;
			this._copyAssetDropdown.AssetType = assetType;
			this._idTextInput.Value = (id ?? "");
			this._titleLabel.Text = this.Desktop.Provider.GetText("ui.assetEditor.createAssetModal.title", new Dictionary<string, string>
			{
				{
					"assetType",
					assetTypeConfig.Name
				}
			}, true);
			this._folderSelector.Parent.Visible = (assetTypeConfig.AssetTree != AssetTreeFolder.Cosmetics);
			foreach (TextButton textButton in this._customButtons)
			{
				textButton.Parent.Remove(textButton);
			}
			this._customButtons.Clear();
			bool flag = assetTypeConfig.CreateButtons != null && this._assetEditorOverlay.Backend.IsEditingRemotely;
			if (flag)
			{
				Document document;
				this.Desktop.Provider.TryGetDocument("AssetEditor/CustomButton.ui", out document);
				using (List<AssetTypeConfig.Button>.Enumerator enumerator2 = assetTypeConfig.CreateButtons.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						AssetTypeConfig.Button customButton = enumerator2.Current;
						UIFragment uifragment = document.Instantiate(this.Desktop, null);
						TextButton textButton2 = uifragment.Get<TextButton>("Button");
						textButton2.Text = this.Desktop.Provider.GetText(customButton.TextId, null, true);
						textButton2.Activating = delegate()
						{
							this._buttonId = customButton.Action;
							this.Validate();
						};
						this._saveButton.Parent.Add(textButton2, 1);
						this._customButtons.Add(textButton2);
					}
				}
			}
			bool flag2 = path == null;
			if (flag2)
			{
				bool flag3 = assetToCopyPath != null;
				if (flag3)
				{
					path = Path.GetDirectoryName(assetToCopyPath.Replace(assetTypeConfig.Path + "/", "")).Replace(Path.DirectorySeparatorChar, '/');
				}
				else
				{
					path = "";
				}
			}
			FileDropdownBox folderSelector = this._folderSelector;
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add(path);
			folderSelector.SelectedFiles = hashSet;
			List<DropdownBox.DropdownEntryInfo> list = new List<DropdownBox.DropdownEntryInfo>();
			foreach (KeyValuePair<string, AssetTypeConfig> keyValuePair in this._assetEditorOverlay.AssetTypeRegistry.AssetTypes)
			{
				bool flag4 = keyValuePair.Value.AssetTree == AssetTreeFolder.Common || keyValuePair.Value.IsVirtual;
				if (!flag4)
				{
					list.Add(new DropdownBox.DropdownEntryInfo(keyValuePair.Value.Name, keyValuePair.Key, false));
				}
			}
			list.Sort((DropdownBox.DropdownEntryInfo a, DropdownBox.DropdownEntryInfo b) => string.Compare(a.Label, b.Label, StringComparison.InvariantCulture));
			this._assetTypeDropdown.Entries = list;
			this._assetTypeDropdown.Value = assetType;
			this._json = (assetTypeConfig.BaseJsonAsset ?? new JObject());
			this.TryBuildPropertyList();
			this.CodeEditor.Value = this._json.ToString();
			bool flag5 = assetToCopyPath != null;
			if (flag5)
			{
				this._copyAssetDropdown.Value = this._assetEditorOverlay.GetAssetIdFromReference(new AssetReference(assetType, assetToCopyPath));
				this.LoadAssetToCopy();
			}
			else
			{
				bool flag6 = json != null;
				if (flag6)
				{
					this._json = json;
					this.CodeEditor.Value = json.ToString();
					this.TryBuildPropertyList();
				}
			}
			bool flag7 = !base.IsMounted;
			if (flag7)
			{
				this.Desktop.SetLayer(4, this);
			}
		}

		// Token: 0x06005C01 RID: 23553 RVA: 0x001CE784 File Offset: 0x001CC984
		private void SetError(string message)
		{
			this._errorLabel.Text = message;
			this._errorLabel.Visible = true;
			base.Layout(null, true);
		}

		// Token: 0x06005C02 RID: 23554 RVA: 0x001CE7C0 File Offset: 0x001CC9C0
		private void SetError(FormattedMessage formattedMessage)
		{
			this._errorLabel.TextSpans = FormattedMessageConverter.GetLabelSpans(formattedMessage, this._assetEditorOverlay.Interface, default(SpanStyle), true);
			this._errorLabel.Visible = true;
			base.Layout(null, true);
		}

		// Token: 0x06005C03 RID: 23555 RVA: 0x001CE813 File Offset: 0x001CCA13
		public override Element HitTest(Point position)
		{
			return base.HitTest(position) ?? this;
		}

		// Token: 0x06005C04 RID: 23556 RVA: 0x001CE824 File Offset: 0x001CCA24
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			base.OnMouseButtonUp(evt, activate);
			bool flag = activate && !this._container.AnchoredRectangle.Contains(this.Desktop.MousePosition);
			if (flag)
			{
				this.Dismiss();
			}
		}

		// Token: 0x06005C05 RID: 23557 RVA: 0x001CE86D File Offset: 0x001CCA6D
		protected internal override void Dismiss()
		{
			this.Desktop.ClearLayer(4);
		}

		// Token: 0x06005C06 RID: 23558 RVA: 0x001CE87C File Offset: 0x001CCA7C
		protected internal override void Validate()
		{
			this._errorLabel.Visible = false;
			bool flag = this._activeTab == CreateAssetModal.Tab.Source;
			if (flag)
			{
				try
				{
					JsonUtils.ValidateJson(this.CodeEditor.Value);
					JObject json = JObject.Parse(this.CodeEditor.Value);
					this._json = json;
					this.TryBuildPropertyList();
				}
				catch (JsonReaderException)
				{
					this._sourceErrorLabel.Visible = true;
					base.Layout(null, true);
					return;
				}
			}
			string text = this._idTextInput.Value.Trim();
			string error;
			bool flag2 = !this._assetEditorOverlay.ValidateAssetId(text, out error);
			if (flag2)
			{
				this.SetError(error);
			}
			else
			{
				bool flag3 = !this._assetEditorOverlay.AssetTypeRegistry.AssetTypes.ContainsKey(this._assetType);
				if (flag3)
				{
					this.SetError(this.Desktop.Provider.GetText("ui.assetEditor.createAssetModal.errors.invalidAssetType", null, true));
				}
				else
				{
					AssetTypeConfig assetTypeConfig = this._assetEditorOverlay.AssetTypeRegistry.AssetTypes[this._assetType];
					string assetType = this._assetType;
					JObject jobject = this._json ?? new JObject();
					bool hasIdField = assetTypeConfig.HasIdField;
					if (hasIdField)
					{
						jobject["Id"] = text;
					}
					bool flag4 = assetTypeConfig.AssetTree == AssetTreeFolder.Cosmetics;
					string filePath;
					if (flag4)
					{
						filePath = assetTypeConfig.Path + "#" + text;
					}
					else
					{
						filePath = AssetPathUtils.CombinePaths(assetTypeConfig.Path, Enumerable.FirstOrDefault<string>(this._folderSelector.SelectedFiles) ?? "");
						filePath = AssetPathUtils.CombinePaths(filePath, text + assetTypeConfig.FileExtension);
					}
					string path;
					AssetFile assetFile;
					bool flag5 = this._assetEditorOverlay.Assets.TryGetPathForAssetId(assetType, text, out path, true) && this._assetEditorOverlay.Assets.TryGetAsset(path, out assetFile, false);
					if (flag5)
					{
						this.SetError(this.Desktop.Provider.GetText("ui.assetEditor.createAssetModal.errors.existingId", null, true));
					}
					else
					{
						this._assetEditorOverlay.CreateAsset(new AssetReference(assetType, filePath), jobject, this._buttonId, delegate(FormattedMessage err)
						{
							Action<string, FormattedMessage> assetCreatedCallback = this._assetCreatedCallback;
							if (assetCreatedCallback != null)
							{
								assetCreatedCallback(filePath, err);
							}
							bool flag6 = !this.IsMounted;
							if (!flag6)
							{
								bool flag7 = err != null;
								if (flag7)
								{
									this.SetError(err);
								}
								else
								{
									this.Dismiss();
								}
							}
						});
					}
				}
			}
		}

		// Token: 0x04003992 RID: 14738
		private readonly AssetEditorOverlay _assetEditorOverlay;

		// Token: 0x04003993 RID: 14739
		private string _assetType;

		// Token: 0x04003994 RID: 14740
		private Action<string, FormattedMessage> _assetCreatedCallback;

		// Token: 0x04003995 RID: 14741
		private Group _container;

		// Token: 0x04003996 RID: 14742
		private Label _titleLabel;

		// Token: 0x04003997 RID: 14743
		private Label _errorLabel;

		// Token: 0x04003998 RID: 14744
		private Label _sourceErrorLabel;

		// Token: 0x04003999 RID: 14745
		private TextField _idTextInput;

		// Token: 0x0400399A RID: 14746
		private AssetSelectorDropdown _copyAssetDropdown;

		// Token: 0x0400399B RID: 14747
		private DropdownBox _assetTypeDropdown;

		// Token: 0x0400399C RID: 14748
		private TabNavigation _tabs;

		// Token: 0x0400399D RID: 14749
		private FileDropdownBox _folderSelector;

		// Token: 0x0400399E RID: 14750
		private TextButton _saveButton;

		// Token: 0x0400399F RID: 14751
		private Group _propertyList;

		// Token: 0x040039A0 RID: 14752
		private Group _sourceEditor;

		// Token: 0x040039A1 RID: 14753
		private JObject _json;

		// Token: 0x040039A2 RID: 14754
		private CreateAssetModal.Tab _activeTab = CreateAssetModal.Tab.Properties;

		// Token: 0x040039A3 RID: 14755
		private string _buttonId;

		// Token: 0x040039A4 RID: 14756
		public readonly WebCodeEditor CodeEditor;

		// Token: 0x040039A5 RID: 14757
		private List<TextButton> _customButtons = new List<TextButton>();

		// Token: 0x02000F9E RID: 3998
		private enum Tab
		{
			// Token: 0x04004B85 RID: 19333
			Properties,
			// Token: 0x04004B86 RID: 19334
			Source
		}
	}
}
