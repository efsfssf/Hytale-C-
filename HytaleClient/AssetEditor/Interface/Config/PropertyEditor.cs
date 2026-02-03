using System;
using System.Collections.Generic;
using System.Diagnostics;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Interface.Elements;
using HytaleClient.Data.UserSettings;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using HytaleClient.Protocol;
using Newtonsoft.Json.Linq;
using SDL2;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BC9 RID: 3017
	internal class PropertyEditor : Element
	{
		// Token: 0x170013D0 RID: 5072
		// (get) Token: 0x06005ECC RID: 24268 RVA: 0x001E6D64 File Offset: 0x001E4F64
		// (set) Token: 0x06005ECD RID: 24269 RVA: 0x001E6D6C File Offset: 0x001E4F6C
		public string PropertyName { get; private set; }

		// Token: 0x170013D1 RID: 5073
		// (get) Token: 0x06005ECE RID: 24270 RVA: 0x001E6D75 File Offset: 0x001E4F75
		// (set) Token: 0x06005ECF RID: 24271 RVA: 0x001E6D7D File Offset: 0x001E4F7D
		public PropertyPath Path { get; private set; }

		// Token: 0x170013D2 RID: 5074
		// (get) Token: 0x06005ED0 RID: 24272 RVA: 0x001E6D86 File Offset: 0x001E4F86
		// (set) Token: 0x06005ED1 RID: 24273 RVA: 0x001E6D8E File Offset: 0x001E4F8E
		public ValueEditor ValueEditor { get; private set; }

		// Token: 0x170013D3 RID: 5075
		// (get) Token: 0x06005ED2 RID: 24274 RVA: 0x001E6D97 File Offset: 0x001E4F97
		// (set) Token: 0x06005ED3 RID: 24275 RVA: 0x001E6D9F File Offset: 0x001E4F9F
		public bool HasErrors { get; private set; }

		// Token: 0x170013D4 RID: 5076
		// (get) Token: 0x06005ED4 RID: 24276 RVA: 0x001E6DA8 File Offset: 0x001E4FA8
		// (set) Token: 0x06005ED5 RID: 24277 RVA: 0x001E6DB0 File Offset: 0x001E4FB0
		public bool HasWarnings { get; private set; }

		// Token: 0x170013D5 RID: 5077
		// (get) Token: 0x06005ED6 RID: 24278 RVA: 0x001E6DB9 File Offset: 0x001E4FB9
		public bool IsCollapsed
		{
			get
			{
				return !this._valueContainer.Visible;
			}
		}

		// Token: 0x170013D6 RID: 5078
		// (get) Token: 0x06005ED7 RID: 24279 RVA: 0x001E6DC9 File Offset: 0x001E4FC9
		public bool SyncPropertyChanges
		{
			get
			{
				return this._syncCheckBox.Value;
			}
		}

		// Token: 0x06005ED8 RID: 24280 RVA: 0x001E6DD8 File Offset: 0x001E4FD8
		public PropertyEditor(Desktop desktop, Element parent, string propertyName, SchemaNode schema, PropertyPath path, SchemaNode parentSchema, ConfigEditor configEditor, ValueEditor parentValueEditor, string displayName = null, bool isVertical = false) : base(desktop, parent)
		{
			this.PropertyName = propertyName;
			this.Schema = schema;
			this.Path = path;
			this.ParentSchema = parentSchema;
			this.ConfigEditor = configEditor;
			this.ParentValueEditor = parentValueEditor;
			this.DisplayName = displayName;
			this.Padding.Bottom = new int?(1);
			this.Name = "P_" + this.PropertyName;
		}

		// Token: 0x06005ED9 RID: 24281 RVA: 0x001E6E50 File Offset: 0x001E5050
		public void Build(JToken value, bool filterCategory = false, bool isDetached = false, CacheRebuildInfo cacheRebuildInfo = null)
		{
			SchemaNode parentSchema = this.ParentSchema;
			this._drawBottomBorder = (parentSchema == null || !parentSchema.DisplayCompact);
			try
			{
				bool flag = (this.Schema.Type == SchemaNode.NodeType.List || this.Schema.Type == SchemaNode.NodeType.Map || this.Schema.Type == SchemaNode.NodeType.Object || this.Schema.Type == SchemaNode.NodeType.AssetReferenceOrInline || this.Schema.Type == SchemaNode.NodeType.Source || this.Schema.Type == SchemaNode.NodeType.Timeline || this.Schema.Type == SchemaNode.NodeType.WeightedTimeline) && !this.Schema.DisplayCompact;
				if (flag)
				{
					this.BuildContainerProperty(value, filterCategory, isDetached, cacheRebuildInfo);
				}
				else
				{
					this.BuildBasicProperty(value, filterCategory, isDetached, cacheRebuildInfo);
				}
			}
			catch (Exception innerException)
			{
				throw new Exception(string.Format("Failed to build property {0} with value {1}", this.Path, value), innerException);
			}
		}

		// Token: 0x06005EDA RID: 24282 RVA: 0x001E6F48 File Offset: 0x001E5148
		private string GetHeaderText(JToken value)
		{
			SchemaNode.NodeType type = this.Schema.Type;
			SchemaNode.NodeType nodeType = type;
			string result;
			if (nodeType != SchemaNode.NodeType.List)
			{
				if (nodeType != SchemaNode.NodeType.Map)
				{
					result = null;
				}
				else
				{
					IUIProvider provider = this.Desktop.Provider;
					string key = "ui.assetEditor.mapEditor.header";
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					string key2 = "count";
					IUIProvider provider2 = this.Desktop.Provider;
					JObject jobject = (JObject)value;
					dictionary.Add(key2, provider2.FormatNumber((jobject != null) ? jobject.Count : 0));
					result = provider.GetText(key, dictionary, true);
				}
			}
			else
			{
				IUIProvider provider3 = this.Desktop.Provider;
				string key3 = "ui.assetEditor.listEditor.header";
				Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
				string key4 = "count";
				IUIProvider provider4 = this.Desktop.Provider;
				JArray jarray = (JArray)value;
				dictionary2.Add(key4, provider4.FormatNumber((jarray != null) ? jarray.Count : 0));
				result = provider3.GetText(key3, dictionary2, true);
			}
			return result;
		}

		// Token: 0x06005EDB RID: 24283 RVA: 0x001E7014 File Offset: 0x001E5214
		private void BuildContainerProperty(JToken value, bool filterCategory, bool isDetachedEditor, CacheRebuildInfo cacheRebuildInfo = null)
		{
			base.Clear();
			this._layoutMode = LayoutMode.Top;
			PatchStyle background = new PatchStyle(UInt32Color.FromRGBA(59, 59, 59, byte.MaxValue));
			Button parent = new Button(this.Desktop, this)
			{
				LayoutMode = LayoutMode.Left,
				Anchor = new Anchor
				{
					Height = new int?(32),
					Bottom = new int?(0)
				},
				Activating = delegate()
				{
					this.SetCollapseState(!this._valueContainer.Visible, true);
				},
				RightClicking = new Action(this.OpenContextPopup)
			};
			this._nameLabel = new PropertyLabel(this, parent, true)
			{
				Text = (this.DisplayName ?? this.PropertyName),
				Background = background,
				Anchor = new Anchor
				{
					Width = new int?(250)
				}
			};
			ResizerHandle resizerHandle = new ResizerHandle(this.Desktop, parent);
			resizerHandle.Anchor = new Anchor
			{
				Width = new int?(1)
			};
			resizerHandle.Background = background;
			resizerHandle.Resizing = new Action(this.OnResize);
			resizerHandle.MouseButtonReleased = new Action(this.OnResizeComplete);
			ResizerHandle resizerHandle2 = new ResizerHandle(this.Desktop, parent);
			resizerHandle2.Anchor = new Anchor
			{
				Width = new int?(1)
			};
			resizerHandle2.Background = new PatchStyle(PropertyEditor.HeaderBorderColor);
			resizerHandle2.Resizing = new Action(this.OnResize);
			resizerHandle2.MouseButtonReleased = new Action(this.OnResizeComplete);
			ResizerHandle resizerHandle3 = new ResizerHandle(this.Desktop, parent);
			resizerHandle3.Anchor = new Anchor
			{
				Width = new int?(1)
			};
			resizerHandle3.Background = background;
			resizerHandle3.Resizing = new Action(this.OnResize);
			resizerHandle3.MouseButtonReleased = new Action(this.OnResizeComplete);
			this._containerHeader = new Label(this.Desktop, parent)
			{
				Background = background,
				Padding = new Padding
				{
					Left = new int?(8),
					Right = new int?(5),
					Top = new int?(6)
				},
				FlexWeight = 1,
				Style = new LabelStyle
				{
					HorizontalAlignment = LabelStyle.LabelAlignment.Start,
					TextColor = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 80),
					FontSize = 14f,
					RenderBold = false,
					RenderItalics = true
				}
			};
			bool flag = this.Schema.Type == SchemaNode.NodeType.Timeline || this.Schema.Type == SchemaNode.NodeType.WeightedTimeline;
			if (flag)
			{
				Group group = new Group(this.Desktop, parent);
				group.Anchor = new Anchor
				{
					Width = new int?(1)
				};
				group.Background = new PatchStyle(PropertyEditor.HeaderBorderColor);
				this._syncCheckBox = new CheckBox(this.Desktop, parent)
				{
					Background = background,
					Anchor = new Anchor
					{
						Width = new int?(32)
					},
					TooltipText = "Enable/disable synchronization of property changes",
					Style = new CheckBox.CheckBoxStyle
					{
						Checked = new CheckBoxStyleState
						{
							DefaultBackground = new PatchStyle("AssetEditor/SyncPropertiesIcon.png")
						},
						Unchecked = new CheckBoxStyleState
						{
							DefaultBackground = new PatchStyle("AssetEditor/SyncPropertiesIcon.png")
							{
								Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 50)
							}
						}
					}
				};
			}
			bool flag2 = this.Schema.Type == SchemaNode.NodeType.List || this.Schema.Type == SchemaNode.NodeType.Map || this.Schema.Type == SchemaNode.NodeType.WeightedTimeline;
			if (flag2)
			{
				Group group2 = new Group(this.Desktop, parent);
				group2.Anchor = new Anchor
				{
					Width = new int?(1)
				};
				group2.Background = new PatchStyle(PropertyEditor.HeaderBorderColor);
				TextButton textButton = new TextButton(this.Desktop, parent);
				textButton.Text = "+";
				textButton.Background = background;
				textButton.Anchor = new Anchor
				{
					Width = new int?(32)
				};
				textButton.Style = new TextButton.TextButtonStyle
				{
					Default = new TextButton.TextButtonStyleState
					{
						LabelStyle = new LabelStyle
						{
							RenderBold = true,
							FontSize = 28f,
							Alignment = LabelStyle.LabelAlignment.Center
						}
					}
				};
				textButton.Activating = new Action(this.OnActivateInsertButton);
			}
			bool flag3 = this.Schema.Type == SchemaNode.NodeType.Object && this.Schema.AllowEmptyObject;
			if (flag3)
			{
				Group group3 = new Group(this.Desktop, parent);
				group3.Anchor = new Anchor
				{
					Width = new int?(1)
				};
				group3.Background = new PatchStyle(PropertyEditor.HeaderBorderColor);
				this._initializeButton = new Button(this.Desktop, parent)
				{
					Background = background,
					Anchor = new Anchor
					{
						Width = new int?(32)
					},
					Style = new Button.ButtonStyle
					{
						Default = new Button.ButtonStyleState()
					},
					Activating = new Action(this.OnActivateInitializeObjectButton)
				};
			}
			Group group4 = new Group(this.Desktop, this);
			group4.Anchor = new Anchor
			{
				Height = new int?(1)
			};
			group4.Background = new PatchStyle(PropertyEditor.HeaderBorderColor);
			this._valueContainer = new Group(this.Desktop, this)
			{
				FlexWeight = 1,
				Background = new PatchStyle(UInt32Color.FromRGBA(162, 162, 162, 41))
			};
			bool visible;
			bool flag4 = this.ConfigEditor.State.UncollapsedProperties.TryGetValue(this.Path, out visible);
			if (flag4)
			{
				this._valueContainer.Visible = visible;
			}
			else
			{
				this._valueContainer.Visible = !this.Schema.IsCollapsedByDefault;
			}
			this.ValueEditor = ValueEditor.CreateFromSchema(this._valueContainer, this.Schema, this.Path, this, this.ParentSchema, this.ConfigEditor, value);
			this.ValueEditor.FilterCategory = filterCategory;
			this.ValueEditor.IsDetachedEditor = isDetachedEditor;
			this.ValueEditor.CachesToRebuild = cacheRebuildInfo;
			this.ValueEditor.ValidateValue();
			this.ValueEditor.BuildEditor();
			this.UpdateAppearance();
		}

		// Token: 0x06005EDC RID: 24284 RVA: 0x001E7688 File Offset: 0x001E5888
		public override Point ComputeScaledMinSize(int? maxWidth, int? maxHeight)
		{
			Element nameLabel = this._nameLabel;
			Anchor anchor = default(Anchor);
			SchemaNode parentSchema = this.ParentSchema;
			anchor.Width = new int?((parentSchema == null || !parentSchema.DisplayCompact) ? this.ConfigEditor.AssetEditorOverlay.Interface.App.Settings.PaneSizes[AssetEditorSettings.Panes.ConfigEditorPropertyNames] : 70);
			nameLabel.Anchor = anchor;
			return base.ComputeScaledMinSize(maxWidth, maxHeight);
		}

		// Token: 0x06005EDD RID: 24285 RVA: 0x001E7704 File Offset: 0x001E5904
		private void BuildBasicProperty(JToken value, bool filterCategory, bool isDetachedEditor, CacheRebuildInfo cacheRebuildInfo = null)
		{
			base.Clear();
			this._layoutMode = LayoutMode.Left;
			Anchor anchor = new Anchor
			{
				Height = new int?(33)
			};
			this.Anchor = anchor;
			this._nameLabel = new PropertyLabel(this, this, false)
			{
				Text = (this.DisplayName ?? this.PropertyName)
			};
			Element nameLabel = this._nameLabel;
			anchor = default(Anchor);
			SchemaNode parentSchema = this.ParentSchema;
			anchor.Width = new int?((parentSchema == null || !parentSchema.DisplayCompact) ? 250 : 70);
			nameLabel.Anchor = anchor;
			SchemaNode parentSchema2 = this.ParentSchema;
			bool flag = parentSchema2 == null || !parentSchema2.DisplayCompact;
			if (flag)
			{
				ResizerHandle resizerHandle = new ResizerHandle(this.Desktop, this);
				anchor = new Anchor
				{
					Width = new int?(1)
				};
				resizerHandle.Anchor = anchor;
				resizerHandle.Resizing = new Action(this.OnResize);
				resizerHandle.MouseButtonReleased = new Action(this.OnResizeComplete);
				ResizerHandle resizerHandle2 = new ResizerHandle(this.Desktop, this);
				anchor = new Anchor
				{
					Width = new int?(1)
				};
				resizerHandle2.Anchor = anchor;
				resizerHandle2.Background = new PatchStyle(PropertyEditor.BorderColor);
				resizerHandle2.Resizing = new Action(this.OnResize);
				resizerHandle2.MouseButtonReleased = new Action(this.OnResizeComplete);
				ResizerHandle resizerHandle3 = new ResizerHandle(this.Desktop, this);
				anchor = new Anchor
				{
					Width = new int?(1)
				};
				resizerHandle3.Anchor = anchor;
				resizerHandle3.Resizing = new Action(this.OnResize);
				resizerHandle3.MouseButtonReleased = new Action(this.OnResizeComplete);
			}
			Group group = new Group(this.Desktop, this);
			SchemaNode parentSchema3 = this.ParentSchema;
			bool flag2 = parentSchema3 != null && parentSchema3.DisplayCompact;
			if (flag2)
			{
				group.FlexWeight = 0;
				Element element = group;
				anchor = new Anchor
				{
					Vertical = new int?(4),
					Left = new int?(4),
					Width = new int?(64)
				};
				element.Anchor = anchor;
				group.OutlineColor = UInt32Color.FromRGBA(106, 106, 106, byte.MaxValue);
				group.OutlineSize = 1f;
				group.Background = new PatchStyle(UInt32Color.FromRGBA(66, 66, 66, 114));
			}
			else
			{
				group.FlexWeight = 1;
			}
			this.ValueEditor = ValueEditor.CreateFromSchema(group, this.Schema, this.Path, this, this.ParentSchema, this.ConfigEditor, value);
			this.ValueEditor.FilterCategory = filterCategory;
			this.ValueEditor.IsDetachedEditor = isDetachedEditor;
			this.ValueEditor.CachesToRebuild = cacheRebuildInfo;
			this.ValueEditor.ValidateValue();
			this.ValueEditor.BuildEditor();
			this.UpdateAppearance();
		}

		// Token: 0x06005EDE RID: 24286 RVA: 0x001E79CC File Offset: 0x001E5BCC
		private void OnResize()
		{
			int num = this.Desktop.UnscaleRound((float)base.AnchoredRectangle.Width) - 100;
			int num2 = this.Desktop.UnscaleRound((float)(this.Desktop.MousePosition.X - this._rectangleAfterPadding.X));
			bool flag = num2 < 100;
			if (flag)
			{
				num2 = 100;
			}
			else
			{
				bool flag2 = num2 > num;
				if (flag2)
				{
					num2 = num;
				}
			}
			this._nameLabel.Anchor.Width = new int?(num2);
			base.Layout(null, true);
		}

		// Token: 0x06005EDF RID: 24287 RVA: 0x001E7A60 File Offset: 0x001E5C60
		private void OnResizeComplete()
		{
			this.ConfigEditor.AssetEditorOverlay.UpdatePaneSize(AssetEditorSettings.Panes.ConfigEditorPropertyNames, this._nameLabel.Anchor.Width.Value);
			this.ConfigEditor.Layout(null, true);
		}

		// Token: 0x06005EE0 RID: 24288 RVA: 0x001E7AB0 File Offset: 0x001E5CB0
		public override Element HitTest(Point position)
		{
			Debug.Assert(base.IsMounted);
			bool flag = !this._anchoredRectangle.Contains(position);
			Element result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = (base.HitTest(position) ?? this);
			}
			return result;
		}

		// Token: 0x06005EE1 RID: 24289 RVA: 0x001E7AF4 File Offset: 0x001E5CF4
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			bool flag = (long)evt.Button != 3L || !activate;
			if (!flag)
			{
				this.OpenContextPopup();
			}
		}

		// Token: 0x06005EE2 RID: 24290 RVA: 0x001E7B24 File Offset: 0x001E5D24
		private void OnActivateInsertButton()
		{
			ValueEditor valueEditor = this.ValueEditor;
			ValueEditor valueEditor2 = valueEditor;
			ListEditor listEditor = valueEditor2 as ListEditor;
			if (listEditor == null)
			{
				if (!(valueEditor2 is MapEditor))
				{
					WeightedTimelineEditor weightedTimelineEditor = valueEditor2 as WeightedTimelineEditor;
					if (weightedTimelineEditor != null)
					{
						this.ConfigEditor.KeyModal.OpenInsertKey(this.Path, weightedTimelineEditor.IdSchema, this.ConfigEditor);
					}
				}
				else
				{
					this.ConfigEditor.KeyModal.OpenInsertKey(this.Path, this.Schema.Key, this.ConfigEditor);
				}
			}
			else
			{
				listEditor.HandleInsertItem(-1);
			}
		}

		// Token: 0x06005EE3 RID: 24291 RVA: 0x001E7BB8 File Offset: 0x001E5DB8
		private void OnActivateInitializeObjectButton()
		{
			bool flag = this.ValueEditor.Value == null;
			if (flag)
			{
				ConfigEditor configEditor = this.ConfigEditor;
				PropertyPath path = this.Path;
				JToken value = new JObject();
				JToken value2 = this.ValueEditor.Value;
				ValueEditor valueEditor = this.ValueEditor;
				AssetEditorRebuildCaches cachesToRebuild;
				if (valueEditor == null)
				{
					cachesToRebuild = null;
				}
				else
				{
					CacheRebuildInfo cachesToRebuild2 = valueEditor.CachesToRebuild;
					cachesToRebuild = ((cachesToRebuild2 != null) ? cachesToRebuild2.Caches : null);
				}
				configEditor.OnChangeValue(path, value, value2, cachesToRebuild, false, false, false);
				base.Layout(null, true);
			}
			else
			{
				this.HandleRemoveProperty(false);
			}
		}

		// Token: 0x06005EE4 RID: 24292 RVA: 0x001E7C3C File Offset: 0x001E5E3C
		internal void SetCollapseState(bool uncollapsed, bool doDiagnosticsAndLayout = true)
		{
			bool flag = this._containerHeader == null || this._valueContainer.Visible == uncollapsed;
			if (!flag)
			{
				this.ConfigEditor.State.UncollapsedProperties[this.Path] = !this._valueContainer.Visible;
				bool flag2 = this._valueContainer.Visible == uncollapsed;
				if (!flag2)
				{
					this._valueContainer.Visible = uncollapsed;
					if (doDiagnosticsAndLayout)
					{
						this.ConfigEditor.SetupDiagnostics(false, true);
						this.ConfigEditor.Layout(null, true);
					}
				}
			}
		}

		// Token: 0x06005EE5 RID: 24293 RVA: 0x001E7CDF File Offset: 0x001E5EDF
		internal void OpenRenameKeyModal()
		{
			this.ConfigEditor.KeyModal.OpenEditKey(this.PropertyName, this.ParentValueEditor.Path, this.ParentValueEditor.Schema.Key, this.ConfigEditor);
		}

		// Token: 0x06005EE6 RID: 24294 RVA: 0x001E7D1C File Offset: 0x001E5F1C
		internal void OpenContextPopup()
		{
			PopupMenuLayer popup = this.ConfigEditor.AssetEditorOverlay.Popup;
			List<PopupMenuItem> list = new List<PopupMenuItem>();
			bool flag;
			if (!(this.ParentValueEditor is ListEditor) && !(this.ParentValueEditor is MapEditor))
			{
				ValueEditor valueEditor = this.ValueEditor;
				flag = (((valueEditor != null) ? valueEditor.Value : null) != null);
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (flag2)
			{
				list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.property.remove", null, true), delegate()
				{
					this.HandleRemoveProperty(false);
				}, null, null));
			}
			ValueEditor valueEditor2 = this.ParentValueEditor;
			ListEditor listParentValueEditor = valueEditor2 as ListEditor;
			bool flag3 = listParentValueEditor != null;
			if (flag3)
			{
				int index = int.Parse(this.PropertyName);
				bool flag4 = index > 0;
				if (flag4)
				{
					list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.property.moveUp", null, true), delegate()
					{
						listParentValueEditor.HandleMoveItem(index, index - 1, true);
					}, null, null));
				}
				bool flag5 = index < listParentValueEditor.Count() - 1;
				if (flag5)
				{
					list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.property.moveDown", null, true), delegate()
					{
						listParentValueEditor.HandleMoveItem(index, index + 1, true);
					}, null, null));
				}
				list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.property.insertSiblingBefore", null, true), delegate()
				{
					listParentValueEditor.HandleInsertItem(index);
				}, null, null));
				list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.property.insertSiblingAfter", null, true), delegate()
				{
					listParentValueEditor.HandleInsertItem(index + 1);
				}, null, null));
			}
			valueEditor2 = this.ParentValueEditor;
			MapEditor parentMapEditor = valueEditor2 as MapEditor;
			bool flag6 = parentMapEditor != null;
			if (flag6)
			{
				int num = -1;
				for (int i = 0; i < this.Parent.Children.Count; i++)
				{
					bool flag7 = this.Parent.Children[i] == this;
					if (flag7)
					{
						num = i;
						break;
					}
				}
				bool flag8 = num > 0;
				if (flag8)
				{
					list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.property.moveUp", null, true), delegate()
					{
						parentMapEditor.HandleMoveKey(this.PropertyName, true);
					}, null, null));
				}
				bool flag9 = num < ((JObject)parentMapEditor.Value).Count - 1;
				if (flag9)
				{
					list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.property.moveDown", null, true), delegate()
					{
						parentMapEditor.HandleMoveKey(this.PropertyName, false);
					}, null, null));
				}
				list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.property.renameKey", null, true), new Action(this.OpenRenameKeyModal), null, null));
			}
			valueEditor2 = this.ParentValueEditor;
			AssetReferenceOrInlineEditor parentAssetReferenceOrInlineEditor = valueEditor2 as AssetReferenceOrInlineEditor;
			bool flag10 = parentAssetReferenceOrInlineEditor != null;
			if (flag10)
			{
				JToken value = parentAssetReferenceOrInlineEditor.Value;
				bool flag11 = value != null && value.Type == 8;
				if (flag11)
				{
					list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.property.createEmbeddedAsset", null, true), delegate()
					{
						parentAssetReferenceOrInlineEditor.EmbedReference();
					}, null, null));
				}
			}
			valueEditor2 = this.ValueEditor;
			AssetReferenceOrInlineEditor assetReferenceOrInlineEditor = valueEditor2 as AssetReferenceOrInlineEditor;
			bool flag12 = assetReferenceOrInlineEditor != null;
			if (flag12)
			{
				JToken value2 = assetReferenceOrInlineEditor.Value;
				bool flag13 = value2 != null && value2.Type == 8;
				if (flag13)
				{
					list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.property.createEmbeddedAsset", null, true), delegate()
					{
						assetReferenceOrInlineEditor.EmbedReference();
					}, null, null));
				}
				else
				{
					JToken value3 = assetReferenceOrInlineEditor.Value;
					bool flag14 = value3 != null && value3.Type == 1;
					if (flag14)
					{
						list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.property.createDedicatedAsset", null, true), delegate()
						{
							assetReferenceOrInlineEditor.CreateDedicatedAsset();
						}, null, null));
					}
				}
			}
			valueEditor2 = this.ValueEditor;
			ListEditor listEditor = valueEditor2 as ListEditor;
			bool flag15 = listEditor != null;
			if (flag15)
			{
				list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.property.uncollapseAllEntries", null, true), delegate()
				{
					listEditor.SetCollapseStateForAllItems(true);
				}, null, null));
				list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.property.collapseAllEntries", null, true), delegate()
				{
					listEditor.SetCollapseStateForAllItems(false);
				}, null, null));
			}
			valueEditor2 = this.ValueEditor;
			MapEditor mapEditor = valueEditor2 as MapEditor;
			bool flag16 = mapEditor != null;
			if (flag16)
			{
				list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.property.uncollapseAllEntries", null, true), delegate()
				{
					mapEditor.SetCollapseStateForAllItems(true);
				}, null, null));
				list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.property.collapseAllEntries", null, true), delegate()
				{
					mapEditor.SetCollapseStateForAllItems(false);
				}, null, null));
			}
			valueEditor2 = this.ValueEditor;
			AssetDropdownEditor assetDropdownEditor = valueEditor2 as AssetDropdownEditor;
			bool flag17 = assetDropdownEditor != null;
			if (flag17)
			{
				bool flag18 = this.ValueEditor.Value != null;
				if (flag18)
				{
					list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.property.openAssetInNewTab", null, true), delegate()
					{
						assetDropdownEditor.OpenSelectedAssetInNewTab();
					}, null, null));
					list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.property.copyAndReferenceAsset", null, true), delegate()
					{
						assetDropdownEditor.CopyAssetAndReference();
					}, null, null));
				}
				list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.property.createAndReferenceAsset", null, true), delegate()
				{
					assetDropdownEditor.CreateNewAssetAndReference(null, null);
				}, null, null));
			}
			ValueEditor valueEditor3 = this.ValueEditor;
			bool flag19 = ((valueEditor3 != null) ? valueEditor3.Value : null) != null;
			if (flag19)
			{
				list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.property.copyValue", null, true), new Action(this.CopyValue), null, null));
			}
			bool flag20 = SDL.SDL_HasClipboardText() == SDL.SDL_bool.SDL_TRUE;
			if (flag20)
			{
				list.Add(new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.property.pasteValue", null, true), new Action(this.PasteValue), null, null));
			}
			bool flag21 = list.Count == 0;
			if (!flag21)
			{
				popup.SetTitle(this.Desktop.Provider.GetText("ui.assetEditor.property.title", new Dictionary<string, string>
				{
					{
						"name",
						this.Schema.Title ?? this.PropertyName
					}
				}, true));
				popup.SetItems(list);
				popup.Open();
			}
		}

		// Token: 0x06005EE7 RID: 24295 RVA: 0x001E8450 File Offset: 0x001E6650
		private void CopyValue()
		{
			string text = this.ValueEditor.Value.ToString();
			bool flag = this.ValueEditor.Value.Type == 9;
			if (flag)
			{
				text = text.ToLowerInvariant();
			}
			SDL.SDL_SetClipboardText(text);
		}

		// Token: 0x06005EE8 RID: 24296 RVA: 0x001E8498 File Offset: 0x001E6698
		private void PasteValue()
		{
			string text = SDL.SDL_GetClipboardText();
			this.ValueEditor.PasteValue(text);
		}

		// Token: 0x06005EE9 RID: 24297 RVA: 0x001E84BC File Offset: 0x001E66BC
		private void ClearParentProperty()
		{
			PropertyPath parent = this.Path.GetParent();
			JToken value = this.ConfigEditor.GetValue(parent);
			SchemaNode schemaNodeInCurrentContext = this.ConfigEditor.AssetEditorOverlay.GetSchemaNodeInCurrentContext(this.ConfigEditor.Value, parent);
			this.ConfigEditor.OnChangeValue(parent, new JObject(), (value != null) ? value.DeepClone() : null, schemaNodeInCurrentContext.RebuildCaches, false, false, false);
			this.ConfigEditor.Layout(null, true);
			bool flag;
			if (this.IsSchemaTypeField)
			{
				ValueEditor parentValueEditor = this.ParentValueEditor;
				flag = (parentValueEditor != null && parentValueEditor.FilterCategory);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				this.ConfigEditor.UpdateCategories();
			}
		}

		// Token: 0x06005EEA RID: 24298 RVA: 0x001E8570 File Offset: 0x001E6770
		public void HandleRemoveProperty(bool confirmed = false)
		{
			bool flag = this.IsSchemaTypeField && !confirmed;
			if (flag)
			{
				JObject jobject = (JObject)this.ConfigEditor.GetValue(this.Path.GetParent());
				bool flag2 = jobject.Count > 1 || !jobject.ContainsKey(this.ParentSchema.TypePropertyKey);
				if (flag2)
				{
					this.ConfigEditor.AssetEditorOverlay.ConfirmationModal.Open(this.Desktop.Provider.GetText("ui.assetEditor.changeConfirmationModal.title", null, true), this.Desktop.Provider.GetText("ui.assetEditor.changeConfirmationModal.text", null, true), new Action(this.ClearParentProperty), null, null, null, false);
				}
				else
				{
					this.ClearParentProperty();
				}
			}
			else
			{
				ConfigEditor configEditor = this.ConfigEditor;
				PropertyPath path = this.Path;
				ValueEditor valueEditor = this.ValueEditor;
				AssetEditorRebuildCaches cachesToRebuild;
				if (valueEditor == null)
				{
					cachesToRebuild = null;
				}
				else
				{
					CacheRebuildInfo cachesToRebuild2 = valueEditor.CachesToRebuild;
					cachesToRebuild = ((cachesToRebuild2 != null) ? cachesToRebuild2.Caches : null);
				}
				configEditor.OnRemoveProperty(path, cachesToRebuild);
				this.ValueEditor.UpdateDisplayedValue();
				this.ConfigEditor.Layout(null, true);
				SchemaNode parentSchema = this.ParentSchema;
				bool flag3;
				if (parentSchema == null || parentSchema.Type != SchemaNode.NodeType.List)
				{
					SchemaNode parentSchema2 = this.ParentSchema;
					flag3 = (parentSchema2 != null && parentSchema2.Type == SchemaNode.NodeType.Map);
				}
				else
				{
					flag3 = true;
				}
				bool flag4 = flag3;
				if (flag4)
				{
					ValueEditor parentValueEditor = this.ParentValueEditor;
					if (parentValueEditor != null)
					{
						parentValueEditor.ParentPropertyEditor.UpdateAppearance();
					}
				}
			}
		}

		// Token: 0x06005EEB RID: 24299 RVA: 0x001E86E4 File Offset: 0x001E68E4
		public void UpdateAppearance()
		{
			this._nameLabel.ApplyTextColor();
			bool flag = this.Schema.Type == SchemaNode.NodeType.List || this.Schema.Type == SchemaNode.NodeType.Map;
			if (flag)
			{
				this._containerHeader.Text = (this.GetHeaderText(this.ValueEditor.Value) ?? "");
				bool isMounted = base.IsMounted;
				if (isMounted)
				{
					this._containerHeader.Layout(null, true);
				}
			}
			bool flag2 = this._initializeButton != null;
			if (flag2)
			{
				this._initializeButton.TooltipText = this.Desktop.Provider.GetText((this.ValueEditor.Value != null) ? "ui.assetEditor.initializeButton.active" : "ui.assetEditor.initializeButton.inactive", null, true);
				this._initializeButton.Style.Default.Background = new PatchStyle((this.ValueEditor.Value != null) ? "AssetEditor/DeinitializeIcon.png" : "AssetEditor/InitializeIcon.png");
				this._initializeButton.Layout(null, true);
			}
		}

		// Token: 0x06005EEC RID: 24300 RVA: 0x001E8800 File Offset: 0x001E6A00
		public void UpdatePathRecursively(string propertyName, PropertyPath path)
		{
			this.PropertyName = propertyName;
			this.Path = path;
			ValueEditor valueEditor = this.ValueEditor;
			if (valueEditor != null)
			{
				valueEditor.UpdatePathRecursively(path);
			}
			SchemaNode parentSchema = this.ParentSchema;
			SchemaNode.NodeType? nodeType = (parentSchema != null) ? new SchemaNode.NodeType?(parentSchema.Type) : null;
			SchemaNode.NodeType? nodeType2 = nodeType;
			if (nodeType2 != null)
			{
				SchemaNode.NodeType valueOrDefault = nodeType2.GetValueOrDefault();
				if (valueOrDefault - SchemaNode.NodeType.List <= 1)
				{
					this._nameLabel.Text = this.PropertyName;
				}
			}
		}

		// Token: 0x06005EED RID: 24301 RVA: 0x001E8884 File Offset: 0x001E6A84
		public void SetHasError(bool doLayout = true)
		{
			bool hasErrors = this.HasErrors;
			if (!hasErrors)
			{
				this.HasErrors = true;
				this._nameLabel.ApplyTextColor();
				if (doLayout)
				{
					this._nameLabel.Layout(null, true);
				}
			}
		}

		// Token: 0x06005EEE RID: 24302 RVA: 0x001E88D0 File Offset: 0x001E6AD0
		public void SetHasWarning(bool doLayout = true)
		{
			bool hasWarnings = this.HasWarnings;
			if (!hasWarnings)
			{
				this.HasWarnings = true;
				bool hasErrors = this.HasErrors;
				if (!hasErrors)
				{
					this._nameLabel.ApplyTextColor();
					if (doLayout)
					{
						this._nameLabel.Layout(null, true);
					}
				}
			}
		}

		// Token: 0x06005EEF RID: 24303 RVA: 0x001E8928 File Offset: 0x001E6B28
		public void ClearDiagnostics(bool doLayout = true)
		{
			bool flag = !this.HasErrors && !this.HasWarnings;
			if (!flag)
			{
				this.HasErrors = false;
				this.HasWarnings = false;
				this.HasChildErrors = false;
				this.HasChildWarnings = false;
				this._nameLabel.ApplyTextColor();
				if (doLayout)
				{
					this._nameLabel.Layout(null, true);
				}
			}
		}

		// Token: 0x06005EF0 RID: 24304 RVA: 0x001E8998 File Offset: 0x001E6B98
		protected override void PrepareForDrawSelf()
		{
			base.PrepareForDrawSelf();
			bool drawBottomBorder = this._drawBottomBorder;
			if (drawBottomBorder)
			{
				TextureArea whitePixel = this.Desktop.Provider.WhitePixel;
				this.Desktop.Batcher2D.RequestDrawTexture(whitePixel.Texture, whitePixel.Rectangle, new Vector3((float)this._anchoredRectangle.X, (float)(this._anchoredRectangle.Y + this._anchoredRectangle.Height) - 1f, 0f), (float)this._anchoredRectangle.Width, 1f, (this._layoutMode == LayoutMode.Top) ? PropertyEditor.HeaderBorderColor : PropertyEditor.BorderColor);
			}
		}

		// Token: 0x04003B16 RID: 15126
		private static readonly UInt32Color HeaderBorderColor = UInt32Color.FromRGBA(48, 48, 48, byte.MaxValue);

		// Token: 0x04003B17 RID: 15127
		public static readonly UInt32Color BorderColor = UInt32Color.FromRGBA(48, 48, 48, 168);

		// Token: 0x04003B1A RID: 15130
		public readonly SchemaNode Schema;

		// Token: 0x04003B1B RID: 15131
		public readonly SchemaNode ParentSchema;

		// Token: 0x04003B1C RID: 15132
		public readonly ConfigEditor ConfigEditor;

		// Token: 0x04003B1D RID: 15133
		private PropertyLabel _nameLabel;

		// Token: 0x04003B1E RID: 15134
		private Label _containerHeader;

		// Token: 0x04003B1F RID: 15135
		private Group _valueContainer;

		// Token: 0x04003B20 RID: 15136
		private CheckBox _syncCheckBox;

		// Token: 0x04003B21 RID: 15137
		private Button _initializeButton;

		// Token: 0x04003B23 RID: 15139
		public readonly ValueEditor ParentValueEditor;

		// Token: 0x04003B26 RID: 15142
		public bool HasChildErrors;

		// Token: 0x04003B27 RID: 15143
		public bool HasChildWarnings;

		// Token: 0x04003B28 RID: 15144
		public bool IsSchemaTypeField;

		// Token: 0x04003B29 RID: 15145
		public readonly string DisplayName;

		// Token: 0x04003B2A RID: 15146
		private bool _drawBottomBorder;
	}
}
