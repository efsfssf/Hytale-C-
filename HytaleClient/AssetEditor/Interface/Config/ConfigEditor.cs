using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HytaleClient.AssetEditor.Backends;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Interface.Editor;
using HytaleClient.AssetEditor.Utils;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using HytaleClient.Protocol;
using Newtonsoft.Json.Linq;
using NLog;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BBC RID: 3004
	internal class ConfigEditor : Element
	{
		// Token: 0x170013C7 RID: 5063
		// (get) Token: 0x06005E09 RID: 24073 RVA: 0x001E0286 File Offset: 0x001DE486
		// (set) Token: 0x06005E0A RID: 24074 RVA: 0x001E028E File Offset: 0x001DE48E
		public FileDropdownBoxStyle FileDropdownBoxStyle { get; private set; }

		// Token: 0x170013C8 RID: 5064
		// (get) Token: 0x06005E0B RID: 24075 RVA: 0x001E0297 File Offset: 0x001DE497
		// (set) Token: 0x06005E0C RID: 24076 RVA: 0x001E029F File Offset: 0x001DE49F
		public ColorPickerDropdownBoxStyle ColorPickerDropdownBoxStyle { get; private set; }

		// Token: 0x170013C9 RID: 5065
		// (get) Token: 0x06005E0D RID: 24077 RVA: 0x001E02A8 File Offset: 0x001DE4A8
		// (set) Token: 0x06005E0E RID: 24078 RVA: 0x001E02B0 File Offset: 0x001DE4B0
		public CheckBox.CheckBoxStyle CheckBoxStyle { get; private set; }

		// Token: 0x170013CA RID: 5066
		// (get) Token: 0x06005E0F RID: 24079 RVA: 0x001E02B9 File Offset: 0x001DE4B9
		// (set) Token: 0x06005E10 RID: 24080 RVA: 0x001E02C1 File Offset: 0x001DE4C1
		public DropdownBoxStyle DropdownBoxStyle { get; private set; }

		// Token: 0x170013CB RID: 5067
		// (get) Token: 0x06005E11 RID: 24081 RVA: 0x001E02CA File Offset: 0x001DE4CA
		// (set) Token: 0x06005E12 RID: 24082 RVA: 0x001E02D2 File Offset: 0x001DE4D2
		public SliderStyle SliderStyle { get; private set; }

		// Token: 0x170013CC RID: 5068
		// (get) Token: 0x06005E13 RID: 24083 RVA: 0x001E02DB File Offset: 0x001DE4DB
		public string SearchQuery
		{
			get
			{
				return this._searchInput.Value.Trim();
			}
		}

		// Token: 0x170013CD RID: 5069
		// (get) Token: 0x06005E14 RID: 24084 RVA: 0x001E02ED File Offset: 0x001DE4ED
		// (set) Token: 0x06005E15 RID: 24085 RVA: 0x001E02F5 File Offset: 0x001DE4F5
		public AssetReference CurrentAsset { get; private set; }

		// Token: 0x170013CE RID: 5070
		// (get) Token: 0x06005E16 RID: 24086 RVA: 0x001E02FE File Offset: 0x001DE4FE
		// (set) Token: 0x06005E17 RID: 24087 RVA: 0x001E0306 File Offset: 0x001DE506
		public JObject Value { get; private set; }

		// Token: 0x170013CF RID: 5071
		// (get) Token: 0x06005E18 RID: 24088 RVA: 0x001E030F File Offset: 0x001DE50F
		// (set) Token: 0x06005E19 RID: 24089 RVA: 0x001E0317 File Offset: 0x001DE517
		public bool DisplayUnsetProperties { get; private set; } = true;

		// Token: 0x06005E1A RID: 24090 RVA: 0x001E0320 File Offset: 0x001DE520
		public ConfigEditor(AssetEditorOverlay overlay) : base(overlay.Desktop, null)
		{
			this.FlexWeight = 1;
			this.AssetEditorOverlay = overlay;
			this.IconExporterModal = new IconExporterModal(this);
			this.KeyModal = new KeyModal(this);
		}

		// Token: 0x06005E1B RID: 24091 RVA: 0x001E0398 File Offset: 0x001DE598
		public void Build()
		{
			base.Clear();
			Document document;
			this.Desktop.Provider.TryGetDocument("Common.ui", out document);
			this.FileDropdownBoxStyle = document.ResolveNamedValue<FileDropdownBoxStyle>(this.Desktop.Provider, "FileDropdownBoxStyle");
			this.ColorPickerDropdownBoxStyle = document.ResolveNamedValue<ColorPickerDropdownBoxStyle>(this.Desktop.Provider, "ColorPickerDropdownBoxStyle");
			this.DropdownBoxStyle = document.ResolveNamedValue<DropdownBoxStyle>(this.Desktop.Provider, "DropdownBoxStyle");
			this.CheckBoxStyle = document.ResolveNamedValue<CheckBox.CheckBoxStyle>(this.Desktop.Provider, "CheckBoxStyle");
			this.SliderStyle = document.ResolveNamedValue<SliderStyle>(this.Desktop.Provider, "SliderStyle");
			Document document2;
			this.Desktop.Provider.TryGetDocument("AssetEditor/ConfigEditor.ui", out document2);
			UIFragment uifragment = document2.Instantiate(this.Desktop, this);
			this._currentAssetNameLabel = uifragment.Get<Group>("CurrentAsset").Find<Label>("AssetName");
			this._currentAssetTypeLabel = uifragment.Get<Group>("CurrentAsset").Find<Label>("AssetType");
			uifragment.Get<Button>("CurrentAssetWrapper").RightClicking = new Action(this.OpenContextMenu);
			uifragment.Get<Button>("CurrentAssetWrapper").DoubleClicking = delegate()
			{
				this.AssetEditorOverlay.FocusAssetInTree(this.CurrentAsset);
			};
			this._container = uifragment.Get<Group>("Container");
			this._container.Scrolled = new Action(this.OnContainerScroll);
			this._loadingOverlay = uifragment.Get<Panel>("LoadingOverlay");
			this._loadingOverlay.Visible = this._isWaitingForBackend;
			this._propertiesContainer = uifragment.Get<Group>("Properties");
			this._errorLabel = uifragment.Get<Label>("ErrorLabel");
			this._searchInput = uifragment.Get<TextField>("PropertySearchInput");
			this._searchInput.ValueChanged = delegate()
			{
				this.BuildPropertyEditors();
				base.Layout(null, true);
			};
			this.IconExporterModal.Build();
			this.KeyModal.Build();
		}

		// Token: 0x06005E1C RID: 24092 RVA: 0x001E0598 File Offset: 0x001DE798
		protected override void OnMounted()
		{
			this.SetupDiagnostics(true, true);
		}

		// Token: 0x06005E1D RID: 24093 RVA: 0x001E05A3 File Offset: 0x001DE7A3
		protected override void OnUnmounted()
		{
			this._propertiesContainer.Clear();
		}

		// Token: 0x06005E1E RID: 24094 RVA: 0x001E05B2 File Offset: 0x001DE7B2
		public void Reset()
		{
			this._propertiesContainer.Clear();
			this._valueEditor = null;
			this.SetWaitingForBackend(false);
			this._assetFileStates.Clear();
			this._assetTypeConfig = null;
		}

		// Token: 0x06005E1F RID: 24095 RVA: 0x001E05E4 File Offset: 0x001DE7E4
		protected override void AfterChildrenLayout()
		{
			bool scrollToOffsetFromStateAfterLayout = this._scrollToOffsetFromStateAfterLayout;
			if (scrollToOffsetFromStateAfterLayout)
			{
				this._scrollToOffsetFromStateAfterLayout = false;
				this._container.SetScroll(new int?(this.State.ScrollOffset.X), new int?(this.State.ScrollOffset.Y));
			}
		}

		// Token: 0x06005E20 RID: 24096 RVA: 0x001E063C File Offset: 0x001DE83C
		public void ToggleDisplayUnsetProperties()
		{
			this.DisplayUnsetProperties = !this.DisplayUnsetProperties;
			this.BuildPropertyEditors();
			base.Layout(null, true);
		}

		// Token: 0x06005E21 RID: 24097 RVA: 0x001E0674 File Offset: 0x001DE874
		public void BuildPropertyEditors()
		{
			this._propertiesContainer.Clear();
			this._propertiesContainer.Visible = true;
			this._errorLabel.Visible = false;
			this._valueEditor = null;
			try
			{
				SchemaNode schemaNode = this.AssetEditorOverlay.ResolveSchemaInCurrentContext(this._assetTypeConfig.Schema);
				this._valueEditor = ValueEditor.CreateFromSchema(this._propertiesContainer, schemaNode, PropertyPath.Root, null, null, this, this.Value);
				this._valueEditor.FilterCategory = (this._valueEditor is ObjectEditor && !this.State.ActiveCategory.Equals(PropertyPath.Root));
				bool flag = schemaNode.RebuildCaches != null;
				if (flag)
				{
					this._valueEditor.CachesToRebuild = new CacheRebuildInfo(schemaNode.RebuildCaches, schemaNode.RebuildCachesForChildProperties);
				}
				this._valueEditor.ValidateValue();
				this._valueEditor.BuildEditor();
				bool isMounted = base.IsMounted;
				if (isMounted)
				{
					this.SetupDiagnostics(false, false);
				}
			}
			catch (Exception ex)
			{
				this._propertiesContainer.Visible = false;
				Exception ex2 = ex;
				Exception ex3 = ex2;
				ValueEditor.InvalidValueException ex4 = ex3 as ValueEditor.InvalidValueException;
				if (ex4 == null)
				{
					if (!(ex3 is ValueEditor.InvalidJsonTypeException))
					{
						this._errorLabel.Text = this.Desktop.Provider.GetText("ui.assetEditor.configEditor.errors.failedToLoad", null, true);
					}
					else
					{
						this._errorLabel.Text = ex.Message;
					}
				}
				else
				{
					this._errorLabel.Text = ex4.DisplayMessage;
				}
				this._errorLabel.Visible = true;
				ConfigEditor.Logger.Error(ex, "Failed to mount editor");
			}
		}

		// Token: 0x06005E22 RID: 24098 RVA: 0x001E0830 File Offset: 0x001DEA30
		public void ScrollToTop()
		{
			Element container = this._container;
			int? y = new int?(0);
			container.SetScroll(null, y);
		}

		// Token: 0x06005E23 RID: 24099 RVA: 0x001E085C File Offset: 0x001DEA5C
		private void GatherCategories()
		{
			SchemaNode schemaNode = this.AssetEditorOverlay.ResolveSchemaInCurrentContext(this._assetTypeConfig.Schema);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			this.GatherCategoriesFromObject(schemaNode, dictionary, "", this.Value);
			this.Categories = dictionary;
			bool flag = this.State.ActiveCategory != null && !this.Categories.ContainsKey(this.State.ActiveCategory.ToString());
			if (flag)
			{
				bool flag2 = dictionary.Count == 0;
				if (flag2)
				{
					this.State.ActiveCategory = null;
				}
				else
				{
					this.State.ActiveCategory = new PropertyPath?(PropertyPath.FromString(Enumerable.FirstOrDefault<KeyValuePair<string, string>>(dictionary).Key));
				}
			}
		}

		// Token: 0x06005E24 RID: 24100 RVA: 0x001E092C File Offset: 0x001DEB2C
		private void GatherCategoriesFromObject(SchemaNode schemaNode, Dictionary<string, string> categories, string path, JToken value)
		{
			bool flag = schemaNode.Properties != null;
			if (flag)
			{
				foreach (KeyValuePair<string, SchemaNode> keyValuePair in schemaNode.Properties)
				{
					bool flag2 = keyValuePair.Value.IsHidden || keyValuePair.Value.SectionStart == null;
					if (!flag2)
					{
						string text = path;
						bool flag3 = text != "";
						if (flag3)
						{
							text += ".";
						}
						text += keyValuePair.Key;
						categories.Add(text, keyValuePair.Value.SectionStart);
						SchemaNode schemaNode2 = this.AssetEditorOverlay.ResolveSchemaInCurrentContext(keyValuePair.Value);
						bool flag4 = schemaNode2.Type == SchemaNode.NodeType.Object;
						if (flag4)
						{
							this.GatherCategoriesFromObject(schemaNode2, categories, text, this.Value[keyValuePair.Key]);
						}
						else
						{
							bool flag5 = schemaNode2.Type == SchemaNode.NodeType.AssetReferenceOrInline;
							if (flag5)
							{
								JToken jtoken = this.Value[keyValuePair.Key];
								bool flag6 = jtoken == null || jtoken.Type != 1;
								if (!flag6)
								{
									schemaNode2 = this.AssetEditorOverlay.ResolveSchemaInCurrentContext(schemaNode2.Value);
									this.GatherCategoriesFromObject(schemaNode2, categories, text, jtoken);
								}
							}
						}
					}
				}
			}
			bool flag7 = schemaNode.TypePropertyKey != null;
			if (flag7)
			{
				SchemaNode schemaNode3 = schemaNode;
				bool flag8 = this.AssetEditorOverlay.TryResolveTypeSchemaInCurrentContext(value, ref schemaNode3);
				if (flag8)
				{
					this.GatherCategoriesFromObject(schemaNode3, categories, path, value);
				}
			}
		}

		// Token: 0x06005E25 RID: 24101 RVA: 0x001E0AF0 File Offset: 0x001DECF0
		public void UpdateCategories()
		{
			this.GatherCategories();
			this.AssetEditorOverlay.ConfigEditorContextPane.UpdateCategories();
			this.AssetEditorOverlay.ConfigEditorContextPane.Layout(null, true);
		}

		// Token: 0x06005E26 RID: 24102 RVA: 0x001E0B34 File Offset: 0x001DED34
		public void SetWaitingForBackend(bool isWaiting)
		{
			bool flag = this._isWaitingForBackend == isWaiting;
			if (!flag)
			{
				this._isWaitingForBackend = isWaiting;
				this._loadingOverlay.Visible = isWaiting;
				if (isWaiting)
				{
					this._loadingOverlay.Layout(new Rectangle?(base.RectangleAfterPadding), true);
				}
			}
		}

		// Token: 0x06005E27 RID: 24103 RVA: 0x001E0B84 File Offset: 0x001DED84
		public void OnAssetRenamed(AssetReference oldReference, AssetReference newReference)
		{
			bool flag = oldReference.Equals(this.CurrentAsset);
			if (flag)
			{
				this.CurrentAsset = newReference;
			}
			ConfigEditorState value;
			bool flag2 = this._assetFileStates.TryGetValue(oldReference.FilePath, out value);
			if (flag2)
			{
				this._assetFileStates.Remove(oldReference.FilePath);
				this._assetFileStates[newReference.FilePath] = value;
			}
		}

		// Token: 0x06005E28 RID: 24104 RVA: 0x001E0BE9 File Offset: 0x001DEDE9
		public void OnAssetDeleted(AssetReference assetReference)
		{
			this._assetFileStates.Remove(assetReference.FilePath);
		}

		// Token: 0x06005E29 RID: 24105 RVA: 0x001E0C00 File Offset: 0x001DEE00
		public void Update()
		{
			this.BuildPropertyEditors();
			base.Layout(null, true);
		}

		// Token: 0x06005E2A RID: 24106 RVA: 0x001E0C28 File Offset: 0x001DEE28
		public void UpdateJson(JObject value)
		{
			this.Value = value;
			this._valueEditor.SetValueRecursively(value);
			this.GatherCategories();
			this.AssetEditorOverlay.ConfigEditorContextPane.UpdateCategories();
			this.AssetEditorOverlay.ConfigEditorContextPane.Layout(null, true);
			base.Layout(null, true);
		}

		// Token: 0x06005E2B RID: 24107 RVA: 0x001E0C90 File Offset: 0x001DEE90
		public void Setup(AssetTypeConfig assetTypeConfig, JObject value, AssetReference asset)
		{
			bool flag = !this._assetFileStates.TryGetValue(asset.FilePath, out this.State);
			if (flag)
			{
				this._assetFileStates[asset.FilePath] = (this.State = new ConfigEditorState());
			}
			this.SubmitPendingUpdateCommands();
			this._currentAssetNameLabel.Text = this.AssetEditorOverlay.GetAssetIdFromReference(asset);
			this._currentAssetTypeLabel.Text = assetTypeConfig.Name;
			this._scrollToOffsetFromStateAfterLayout = true;
			this._assetTypeConfig = assetTypeConfig;
			this.CurrentAsset = asset;
			this.Value = value;
			this.SetWaitingForBackend(false);
			this.GatherCategories();
			bool flag2 = this.State.ActiveCategory == null;
			if (flag2)
			{
				this.State.ActiveCategory = new PropertyPath?((this.Categories.Count > 0) ? PropertyPath.FromString(Enumerable.First<KeyValuePair<string, string>>(this.Categories).Key) : PropertyPath.Root);
			}
			this.BuildPropertyEditors();
		}

		// Token: 0x06005E2C RID: 24108 RVA: 0x001E0D9C File Offset: 0x001DEF9C
		private void OnContainerScroll()
		{
			bool flag = !this._scrollToOffsetFromStateAfterLayout;
			if (flag)
			{
				this.State.ScrollOffset = this._container.ScaledScrollOffset;
			}
		}

		// Token: 0x06005E2D RID: 24109 RVA: 0x001E0DD0 File Offset: 0x001DEFD0
		public void OnChangeValue(PropertyPath path, JToken value, JToken previousValue, AssetEditorRebuildCaches cachesToRebuild, bool withheldCommand = false, bool insertItem = false, bool updateDisplayedValue = false)
		{
			PropertyPath? firstCreatedProperty;
			this.SetProperty(this.Value, path, value, out firstCreatedProperty, updateDisplayedValue, insertItem);
			ClientJsonUpdateCommand clientJsonUpdateCommand = new ClientJsonUpdateCommand
			{
				Type = (insertItem ? 1 : 0),
				Path = path,
				Value = ((value != null) ? value.DeepClone() : null),
				PreviousValue = previousValue,
				FirstCreatedProperty = firstCreatedProperty,
				RebuildCaches = cachesToRebuild
			};
			ClientJsonUpdateCommand clientJsonUpdateCommand2 = Enumerable.LastOrDefault<ClientJsonUpdateCommand>(this._pendingUpdateCommands);
			bool flag = clientJsonUpdateCommand2 != null && clientJsonUpdateCommand2.Path.Equals(path) && clientJsonUpdateCommand2.Type == clientJsonUpdateCommand.Type;
			if (flag)
			{
				bool flag2 = clientJsonUpdateCommand2.RebuildCaches != null;
				if (flag2)
				{
					bool flag3 = clientJsonUpdateCommand.RebuildCaches == null;
					if (flag3)
					{
						clientJsonUpdateCommand.RebuildCaches = clientJsonUpdateCommand2.RebuildCaches;
					}
					else
					{
						clientJsonUpdateCommand.RebuildCaches = new AssetEditorRebuildCaches
						{
							Models = (clientJsonUpdateCommand.RebuildCaches.Models || clientJsonUpdateCommand2.RebuildCaches.Models),
							ModelTextures = (clientJsonUpdateCommand.RebuildCaches.ModelTextures || clientJsonUpdateCommand2.RebuildCaches.ModelTextures),
							BlockTextures = (clientJsonUpdateCommand.RebuildCaches.BlockTextures || clientJsonUpdateCommand2.RebuildCaches.BlockTextures),
							ItemIcons = (clientJsonUpdateCommand.RebuildCaches.ItemIcons || clientJsonUpdateCommand2.RebuildCaches.ItemIcons),
							MapGeometry = (clientJsonUpdateCommand.RebuildCaches.MapGeometry || clientJsonUpdateCommand2.RebuildCaches.MapGeometry)
						};
					}
				}
				clientJsonUpdateCommand.PreviousValue = clientJsonUpdateCommand2.PreviousValue;
				clientJsonUpdateCommand.FirstCreatedProperty = clientJsonUpdateCommand2.FirstCreatedProperty;
				this._pendingUpdateCommands[this._pendingUpdateCommands.Count - 1] = clientJsonUpdateCommand;
			}
			else
			{
				this._pendingUpdateCommands.Add(clientJsonUpdateCommand);
			}
			bool flag4 = !withheldCommand;
			if (flag4)
			{
				this.SubmitPendingUpdateCommands();
			}
			this.AssetEditorOverlay.Backend.OnValueChanged(path, value);
		}

		// Token: 0x06005E2E RID: 24110 RVA: 0x001E0FC4 File Offset: 0x001DF1C4
		public void OnRemoveProperty(PropertyPath path, AssetEditorRebuildCaches cachesToRebuild)
		{
			PropertyPath path2;
			JToken jtoken;
			this.RemoveProperty(path, out path2, out jtoken);
			ClientJsonUpdateCommand item = new ClientJsonUpdateCommand
			{
				Type = 2,
				Path = path2,
				PreviousValue = ((jtoken != null) ? jtoken.DeepClone() : null),
				RebuildCaches = cachesToRebuild
			};
			base.Layout(null, true);
			this._pendingUpdateCommands.Add(item);
			this.AssetEditorOverlay.Backend.OnValueChanged(path, null);
			this.SubmitPendingUpdateCommands();
		}

		// Token: 0x06005E2F RID: 24111 RVA: 0x001E1044 File Offset: 0x001DF244
		public void SubmitPendingUpdateCommands()
		{
			bool flag = this._pendingUpdateCommands.Count == 0;
			if (!flag)
			{
				ConfigEditor.Logger.Info("Submitting {0} update commands", this._pendingUpdateCommands.Count);
				this.AssetEditorOverlay.Backend.UpdateJsonAsset(this.CurrentAsset, this._pendingUpdateCommands, null);
				this._pendingUpdateCommands.Clear();
			}
		}

		// Token: 0x06005E30 RID: 24112 RVA: 0x001E10AC File Offset: 0x001DF2AC
		public void SetupDiagnostics(bool doLayout = true, bool clearAlreadySetupDiagnostics = true)
		{
			AssetDiagnostics assetDiagnostics;
			this.AssetEditorOverlay.Diagnostics.TryGetValue(this.CurrentAsset.FilePath, out assetDiagnostics);
			if (clearAlreadySetupDiagnostics)
			{
				this._valueEditor.ClearDiagnosticsOfDescendants();
			}
			bool flag = assetDiagnostics.Errors != null;
			if (flag)
			{
				foreach (AssetDiagnosticMessage assetDiagnosticMessage in assetDiagnostics.Errors)
				{
					bool flag2 = assetDiagnosticMessage.Property.ElementCount == 0;
					if (!flag2)
					{
						PropertyPath path = PropertyPath.Root;
						for (int j = 0; j < assetDiagnosticMessage.Property.ElementCount; j++)
						{
							path = path.GetChild(assetDiagnosticMessage.Property.Elements[j]);
							PropertyEditor propertyEditor;
							bool flag3 = !this.TryFindPropertyEditor(path, out propertyEditor) || !propertyEditor.IsMounted;
							if (flag3)
							{
								break;
							}
							propertyEditor.SetHasError(doLayout);
							bool flag4 = j != assetDiagnosticMessage.Property.ElementCount - 1;
							if (flag4)
							{
								propertyEditor.HasChildErrors = true;
							}
						}
					}
				}
			}
			bool flag5 = assetDiagnostics.Warnings != null;
			if (flag5)
			{
				foreach (AssetDiagnosticMessage assetDiagnosticMessage2 in assetDiagnostics.Warnings)
				{
					bool flag6 = assetDiagnosticMessage2.Property.ElementCount == 0;
					if (!flag6)
					{
						PropertyPath path2 = PropertyPath.Root;
						for (int l = 0; l < assetDiagnosticMessage2.Property.ElementCount; l++)
						{
							path2 = path2.GetChild(assetDiagnosticMessage2.Property.Elements[l]);
							PropertyEditor propertyEditor2;
							bool flag7 = !this.TryFindPropertyEditor(path2, out propertyEditor2) || !propertyEditor2.IsMounted;
							if (flag7)
							{
								break;
							}
							propertyEditor2.SetHasWarning(doLayout);
							bool flag8 = l != assetDiagnosticMessage2.Property.ElementCount - 1;
							if (flag8)
							{
								propertyEditor2.HasChildErrors = true;
							}
						}
					}
				}
			}
		}

		// Token: 0x06005E31 RID: 24113 RVA: 0x001E12DC File Offset: 0x001DF4DC
		public void FocusPropertySearch()
		{
			bool flag = !this._searchInput.IsMounted;
			if (!flag)
			{
				this.Desktop.FocusElement(this._searchInput, true);
			}
		}

		// Token: 0x06005E32 RID: 24114 RVA: 0x001E1314 File Offset: 0x001DF514
		public bool TryFindPropertyEditor(PropertyPath path, out PropertyEditor propertyEditor)
		{
			bool flag = path.Elements.Length == 0;
			bool result;
			if (flag)
			{
				propertyEditor = null;
				result = false;
			}
			else
			{
				result = this._valueEditor.TryFindPropertyEditor(path, out propertyEditor);
			}
			return result;
		}

		// Token: 0x06005E33 RID: 24115 RVA: 0x001E134C File Offset: 0x001DF54C
		public string GetCurrentAssetId()
		{
			bool flag = this.CurrentAsset.Type == null;
			string result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = AssetPathUtils.GetAssetIdFromReference(this.CurrentAsset.FilePath, this._assetTypeConfig.AssetTree == AssetTreeFolder.Cosmetics);
			}
			return result;
		}

		// Token: 0x06005E34 RID: 24116 RVA: 0x001E1394 File Offset: 0x001DF594
		private void OpenContextMenu()
		{
			PopupMenuLayer popup = this.AssetEditorOverlay.Popup;
			List<PopupMenuItem> items = new List<PopupMenuItem>();
			this.AssetEditorOverlay.SetupAssetPopup(this.CurrentAsset, items);
			popup.SetTitle(this.AssetEditorOverlay.GetAssetIdFromReference(this.CurrentAsset));
			popup.SetItems(items);
			popup.Open();
		}

		// Token: 0x06005E35 RID: 24117 RVA: 0x001E13F0 File Offset: 0x001DF5F0
		private JContainer GetContainer(JContainer root, PropertyPath path, bool create, out PropertyPath? firstCreatedProperty)
		{
			firstCreatedProperty = null;
			JContainer jcontainer = root;
			PropertyPath propertyPath = PropertyPath.Root;
			string[] elements = path.Elements;
			for (int i = 0; i < elements.Length - 1; i++)
			{
				string text = elements[i];
				propertyPath = propertyPath.GetChild(text);
				bool flag = jcontainer is JObject;
				if (flag)
				{
					JToken jtoken = jcontainer[text];
					bool flag2 = jtoken == null || jtoken.Type == 10;
					if (flag2)
					{
						bool flag3 = !create;
						if (flag3)
						{
							return null;
						}
						bool flag4 = Enumerable.All<char>(elements[i + 1], new Func<char, bool>(char.IsDigit));
						if (flag4)
						{
							jtoken = new JArray();
						}
						else
						{
							jtoken = new JObject();
						}
						jcontainer[text] = jtoken;
						bool flag5 = firstCreatedProperty == null;
						if (flag5)
						{
							firstCreatedProperty = new PropertyPath?(propertyPath);
						}
						this.SetEditorValue(propertyPath, jtoken);
					}
					jcontainer = (JContainer)jtoken;
				}
				else
				{
					JArray jarray = jcontainer as JArray;
					bool flag6 = jarray != null;
					if (!flag6)
					{
						string str = "Value is of unexpected type: ";
						Type type = jcontainer.GetType();
						throw new Exception(str + ((type != null) ? type.ToString() : null));
					}
					int num;
					bool flag7 = !int.TryParse(text, out num);
					if (flag7)
					{
						throw new Exception("Array index must be number!");
					}
					JToken jtoken2 = (jarray.Count > num) ? jarray[num] : null;
					bool flag8 = jtoken2 == null || jtoken2.Type == 10;
					if (flag8)
					{
						bool flag9 = !create;
						if (flag9)
						{
							return null;
						}
						bool flag10 = Enumerable.All<char>(elements[i + 1], new Func<char, bool>(char.IsDigit));
						if (flag10)
						{
							jtoken2 = new JArray();
						}
						else
						{
							jtoken2 = new JObject();
						}
						bool flag11 = num >= jcontainer.Count;
						if (flag11)
						{
							jcontainer.Add(jtoken2);
						}
						else
						{
							jcontainer[num] = jtoken2;
						}
						bool flag12 = firstCreatedProperty == null;
						if (flag12)
						{
							firstCreatedProperty = new PropertyPath?(propertyPath);
						}
						this.SetEditorValue(propertyPath, jtoken2);
					}
					jcontainer = (JContainer)jtoken2;
				}
			}
			return jcontainer;
		}

		// Token: 0x06005E36 RID: 24118 RVA: 0x001E1630 File Offset: 0x001DF830
		public JToken GetValue(PropertyPath path)
		{
			bool flag = path.Elements.Length == 0;
			JToken result;
			if (flag)
			{
				result = this.Value;
			}
			else
			{
				PropertyPath? propertyPath;
				JContainer container = this.GetContainer(this.Value, path, false, out propertyPath);
				JContainer jcontainer = container;
				JContainer jcontainer2 = jcontainer;
				if (jcontainer2 != null)
				{
					if (!(jcontainer2 is JArray))
					{
						result = container[path.LastElement];
					}
					else
					{
						result = container[int.Parse(path.LastElement)];
					}
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x06005E37 RID: 24119 RVA: 0x001E16AC File Offset: 0x001DF8AC
		public void RemoveProperty(JObject root, PropertyPath path, out PropertyPath? finalContainerRemoved, out JToken finalValueRemoved, bool updateDisplayedValue = false, bool cleanupEmptyContainers = true)
		{
			Debug.Assert(path.Elements.Length != 0);
			finalContainerRemoved = null;
			PropertyPath? propertyPath;
			JContainer container = this.GetContainer(root, path, false, out propertyPath);
			PropertyPath parent = path.GetParent();
			if (cleanupEmptyContainers)
			{
				PropertyPath propertyPath2;
				bool flag = this.TryGetEmptyParentContainerToRemove(container, parent, out propertyPath2);
				if (flag)
				{
					finalContainerRemoved = new PropertyPath?(propertyPath2);
					path = propertyPath2;
					container = this.GetContainer(root, path, false, out propertyPath);
				}
			}
			JContainer jcontainer = container;
			JContainer jcontainer2 = jcontainer;
			JArray jarray = jcontainer2 as JArray;
			if (jarray == null)
			{
				JObject jobject = jcontainer2 as JObject;
				if (jobject == null)
				{
					throw new Exception("Invalid container type: " + container.Type.ToString());
				}
				string lastElement = path.LastElement;
				finalValueRemoved = jobject[lastElement];
				jobject.Remove(lastElement);
			}
			else
			{
				int num = int.Parse(path.LastElement);
				finalValueRemoved = jarray[num];
				jarray.RemoveAt(num);
			}
			this.SetEditorValueRecursively(path, null, updateDisplayedValue);
			PropertyEditor propertyEditor;
			bool flag2 = !this.DisplayUnsetProperties && path.Elements.Length != 0 && this.TryFindPropertyEditor(finalContainerRemoved.GetValueOrDefault(path), out propertyEditor);
			if (flag2)
			{
				this._valueEditor.SetValueRecursively(this.Value);
				base.Layout(null, true);
			}
			else
			{
				PropertyEditor propertyEditor2;
				bool flag3 = path.Elements.Length > 1 && this.TryFindPropertyEditor(path.GetParent(), out propertyEditor2);
				if (flag3)
				{
					ValueEditor valueEditor = propertyEditor2.ValueEditor;
					ValueEditor valueEditor2 = valueEditor;
					ListEditor listEditor = valueEditor2 as ListEditor;
					if (listEditor == null)
					{
						MapEditor mapEditor = valueEditor2 as MapEditor;
						if (mapEditor != null)
						{
							mapEditor.OnItemRemoved(path.LastElement);
						}
					}
					else
					{
						listEditor.OnItemRemoved(int.Parse(path.LastElement));
					}
				}
			}
		}

		// Token: 0x06005E38 RID: 24120 RVA: 0x001E1888 File Offset: 0x001DFA88
		public void SetProperty(JObject root, PropertyPath path, JToken value, out PropertyPath? firstCreatedProperty, bool updateDisplayedValue = false, bool insertItem = false)
		{
			firstCreatedProperty = null;
			bool flag = path.Elements.Length == 0;
			if (flag)
			{
				this.AssetEditorOverlay.SetTrackedAssetData(this.CurrentAsset.FilePath, (JObject)value);
			}
			else
			{
				JContainer container = this.GetContainer(root, PropertyPath.FromElements(path.Elements), true, out firstCreatedProperty);
				JArray jarray = container as JArray;
				bool flag2 = jarray != null;
				if (flag2)
				{
					int num = int.Parse(path.LastElement);
					if (insertItem)
					{
						bool flag3 = firstCreatedProperty == null;
						if (flag3)
						{
							firstCreatedProperty = new PropertyPath?(path);
						}
						jarray.Insert(num, value);
						PropertyEditor propertyEditor;
						bool flag4 = this.TryFindPropertyEditor(path.GetParent(), out propertyEditor);
						if (flag4)
						{
							((ListEditor)propertyEditor.ValueEditor).OnItemInserted(value, num);
						}
						else
						{
							this.SetEditorValueRecursively(path, value, updateDisplayedValue);
						}
					}
					else
					{
						container[num] = value;
						this.SetEditorValueRecursively(path, value, updateDisplayedValue);
					}
				}
				else
				{
					string lastElement = path.LastElement;
					bool flag5 = !((JObject)container).ContainsKey(lastElement);
					if (flag5)
					{
						bool flag6 = firstCreatedProperty == null;
						if (flag6)
						{
							firstCreatedProperty = new PropertyPath?(path);
						}
						PropertyEditor propertyEditor2;
						MapEditor mapEditor;
						bool flag7;
						if (this.TryFindPropertyEditor(path.GetParent(), out propertyEditor2))
						{
							mapEditor = (propertyEditor2.ValueEditor as MapEditor);
							flag7 = (mapEditor != null);
						}
						else
						{
							flag7 = false;
						}
						bool flag8 = flag7;
						if (flag8)
						{
							mapEditor.OnItemInserted(lastElement, value);
						}
						else
						{
							this.SetEditorValueRecursively(path, value, updateDisplayedValue);
						}
					}
					else
					{
						this.SetEditorValueRecursively(path, value, updateDisplayedValue);
					}
					container[lastElement] = value;
				}
				bool flag9 = firstCreatedProperty != null && !this.DisplayUnsetProperties;
				if (flag9)
				{
					this._valueEditor.SetValueRecursively(this.Value);
					base.Layout(null, true);
				}
			}
		}

		// Token: 0x06005E39 RID: 24121 RVA: 0x001E1A7C File Offset: 0x001DFC7C
		private void RemoveProperty(PropertyPath path, out PropertyPath finalPropertyRemoved, out JToken finalValueRemoved)
		{
			PropertyPath? propertyPath;
			this.RemoveProperty(this.Value, path, out propertyPath, out finalValueRemoved, false, true);
			finalPropertyRemoved = propertyPath.GetValueOrDefault(path);
		}

		// Token: 0x06005E3A RID: 24122 RVA: 0x001E1AAC File Offset: 0x001DFCAC
		private void SetEditorValue(PropertyPath path, JToken value)
		{
			PropertyEditor propertyEditor;
			bool flag = !this.TryFindPropertyEditor(path, out propertyEditor);
			if (!flag)
			{
				propertyEditor.ValueEditor.SetValue(value);
			}
		}

		// Token: 0x06005E3B RID: 24123 RVA: 0x001E1ADC File Offset: 0x001DFCDC
		private void SetEditorValueRecursively(PropertyPath path, JToken value, bool updateDisplayedValue)
		{
			PropertyEditor propertyEditor;
			bool flag = !this.TryFindPropertyEditor(path, out propertyEditor);
			if (!flag)
			{
				propertyEditor.ValueEditor.SetValueRecursively(value);
				if (updateDisplayedValue)
				{
					propertyEditor.ValueEditor.UpdateDisplayedValue();
				}
			}
		}

		// Token: 0x06005E3C RID: 24124 RVA: 0x001E1B1C File Offset: 0x001DFD1C
		private bool TryGetEmptyParentContainerToRemove(JContainer container, PropertyPath path, out PropertyPath finalPath)
		{
			JContainer jcontainer = container;
			finalPath = PropertyPath.Root;
			while (jcontainer != null)
			{
				bool flag = jcontainer.Count <= 1 && jcontainer != this.Value;
				if (!flag)
				{
					break;
				}
				JContainer parent = jcontainer.Parent;
				bool flag2 = parent is JProperty;
				if (flag2)
				{
					parent = parent.Parent;
				}
				SchemaNode schemaNodeInCurrentContext = this.AssetEditorOverlay.GetSchemaNodeInCurrentContext(this.Value, path);
				bool flag3 = schemaNodeInCurrentContext.Type == SchemaNode.NodeType.Object && schemaNodeInCurrentContext.AllowEmptyObject;
				if (flag3)
				{
					break;
				}
				PropertyPath parent2 = path.GetParent();
				SchemaNode schemaNodeInCurrentContext2 = this.AssetEditorOverlay.GetSchemaNodeInCurrentContext(this.Value, parent2);
				bool flag4 = schemaNodeInCurrentContext2.Type == SchemaNode.NodeType.List || schemaNodeInCurrentContext2.Type == SchemaNode.NodeType.Map;
				if (flag4)
				{
					break;
				}
				finalPath = path;
				jcontainer = parent;
				bool flag5 = jcontainer == this.Value;
				if (flag5)
				{
					break;
				}
				path = parent2;
			}
			return finalPath.Elements.Length != 0;
		}

		// Token: 0x04003ABC RID: 15036
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003ABD RID: 15037
		public readonly AssetEditorOverlay AssetEditorOverlay;

		// Token: 0x04003ABE RID: 15038
		public readonly IconExporterModal IconExporterModal;

		// Token: 0x04003ABF RID: 15039
		public readonly KeyModal KeyModal;

		// Token: 0x04003AC5 RID: 15045
		private Label _errorLabel;

		// Token: 0x04003AC6 RID: 15046
		private TextField _searchInput;

		// Token: 0x04003AC7 RID: 15047
		private Group _container;

		// Token: 0x04003AC8 RID: 15048
		private Group _propertiesContainer;

		// Token: 0x04003AC9 RID: 15049
		private ValueEditor _valueEditor;

		// Token: 0x04003ACA RID: 15050
		private Panel _loadingOverlay;

		// Token: 0x04003ACB RID: 15051
		private Label _currentAssetNameLabel;

		// Token: 0x04003ACC RID: 15052
		private Label _currentAssetTypeLabel;

		// Token: 0x04003ACD RID: 15053
		public readonly List<TimelineEditor> MountedTimelineEditors = new List<TimelineEditor>();

		// Token: 0x04003ACE RID: 15054
		public string LastOpenedFileSelectorDirectory;

		// Token: 0x04003ACF RID: 15055
		private AssetTypeConfig _assetTypeConfig;

		// Token: 0x04003AD2 RID: 15058
		private readonly List<ClientJsonUpdateCommand> _pendingUpdateCommands = new List<ClientJsonUpdateCommand>();

		// Token: 0x04003AD3 RID: 15059
		private Dictionary<string, ConfigEditorState> _assetFileStates = new Dictionary<string, ConfigEditorState>();

		// Token: 0x04003AD4 RID: 15060
		public ConfigEditorState State;

		// Token: 0x04003AD5 RID: 15061
		private bool _isWaitingForBackend;

		// Token: 0x04003AD6 RID: 15062
		private bool _scrollToOffsetFromStateAfterLayout;

		// Token: 0x04003AD8 RID: 15064
		public Dictionary<string, string> Categories = new Dictionary<string, string>();
	}
}
