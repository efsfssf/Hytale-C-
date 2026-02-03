using System;
using System.Collections.Generic;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Interface.Editor;
using HytaleClient.Graphics;
using HytaleClient.Interface.Messages;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BCA RID: 3018
	internal class PropertyLabel : ReorderableListGrip
	{
		// Token: 0x170013D7 RID: 5079
		// (set) Token: 0x06005EF3 RID: 24307 RVA: 0x001E8A88 File Offset: 0x001E6C88
		public string Text
		{
			set
			{
				this._label.Text = value;
			}
		}

		// Token: 0x06005EF4 RID: 24308 RVA: 0x001E8A98 File Offset: 0x001E6C98
		public PropertyLabel(PropertyEditor propertyEditor, Element parent, bool isCollapsable) : base(propertyEditor.Desktop, parent)
		{
			this._propertyEditor = propertyEditor;
			this._layoutMode = LayoutMode.Left;
			SchemaNode parentSchema = propertyEditor.ParentSchema;
			bool isDragEnabled;
			if (parentSchema == null || parentSchema.Type != SchemaNode.NodeType.Map)
			{
				SchemaNode parentSchema2 = propertyEditor.ParentSchema;
				isDragEnabled = (parentSchema2 != null && parentSchema2.Type == SchemaNode.NodeType.List);
			}
			else
			{
				isDragEnabled = true;
			}
			this.IsDragEnabled = isDragEnabled;
			this.Padding.Left = new int?(12 + (propertyEditor.Path.Elements.Length - 1) * 18);
			SchemaNode parentSchema3 = propertyEditor.ParentSchema;
			bool flag;
			if (parentSchema3 == null || parentSchema3.Type != SchemaNode.NodeType.List)
			{
				SchemaNode parentSchema4 = propertyEditor.ParentSchema;
				flag = (parentSchema4 != null && parentSchema4.Type == SchemaNode.NodeType.Map);
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (flag2)
			{
				this.Padding.Left = new int?(3);
				Group group = new Group(this.Desktop, this);
				group.Anchor = new Anchor
				{
					Width = new int?(6),
					Height = new int?(18),
					Right = new int?(3 + (propertyEditor.Path.Elements.Length - 1) * 18)
				};
				group.Background = new PatchStyle("AssetEditor/GripIcon.png");
			}
			if (isCollapsable)
			{
				this._collapseIcon = new Group(this.Desktop, this)
				{
					Anchor = new Anchor
					{
						Right = new int?(6),
						Width = new int?(12),
						Height = new int?(12)
					}
				};
			}
			this._diagnosticsIcon = new Group(this.Desktop, this)
			{
				Anchor = new Anchor
				{
					Right = new int?(6),
					Width = new int?(18),
					Height = new int?(18)
				},
				Visible = false
			};
			this._label = new Label(this.Desktop, this)
			{
				FlexWeight = 1,
				Style = new LabelStyle
				{
					HorizontalAlignment = LabelStyle.LabelAlignment.Start,
					FontSize = 14f,
					VerticalAlignment = LabelStyle.LabelAlignment.Center
				}
			};
			SchemaNode parentSchema5 = propertyEditor.ParentSchema;
			bool flag3;
			if (parentSchema5 == null || parentSchema5.Type != SchemaNode.NodeType.List)
			{
				SchemaNode parentSchema6 = propertyEditor.ParentSchema;
				flag3 = (parentSchema6 != null && parentSchema6.Type == SchemaNode.NodeType.Map);
			}
			else
			{
				flag3 = true;
			}
			bool flag4 = flag3;
			if (flag4)
			{
				this._label.Style.RenderBold = true;
			}
			this._removeButton = new Button(this.Desktop, this)
			{
				Visible = false,
				Anchor = new Anchor
				{
					Right = new int?(5),
					Width = new int?(22),
					Height = new int?(22)
				},
				Style = new Button.ButtonStyle
				{
					Default = new Button.ButtonStyleState
					{
						Background = new PatchStyle(PropertyLabel.IconRemove)
						{
							Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 38)
						}
					},
					Hovered = new Button.ButtonStyleState
					{
						Background = new PatchStyle(PropertyLabel.IconRemove)
						{
							Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 120)
						}
					},
					Pressed = new Button.ButtonStyleState
					{
						Background = new PatchStyle(PropertyLabel.IconRemove)
						{
							Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 100)
						}
					}
				},
				Activating = new Action(this.OnRemoveButtonActivating)
			};
			SchemaNode parentSchema7 = this._propertyEditor.ParentSchema;
			bool flag5 = parentSchema7 != null && parentSchema7.DisplayCompact;
			if (flag5)
			{
				this.Padding.Left = new int?(6);
				this._label.Style.HorizontalAlignment = LabelStyle.LabelAlignment.End;
				base.Reorder(this._label, base.Children.Count - 1);
			}
		}

		// Token: 0x06005EF5 RID: 24309 RVA: 0x001E8E68 File Offset: 0x001E7068
		public void ApplyTextColor()
		{
			bool hasErrors = this._propertyEditor.HasErrors;
			if (hasErrors)
			{
				this._label.Style.TextColor = UInt32Color.FromRGBA(232, 96, 96, (this._propertyEditor.ValueEditor.Value != null) ? 200 : 120);
			}
			else
			{
				bool hasWarnings = this._propertyEditor.HasWarnings;
				if (hasWarnings)
				{
					this._label.Style.TextColor = UInt32Color.FromRGBA(232, 151, 96, (this._propertyEditor.ValueEditor.Value != null) ? 200 : 120);
				}
				else
				{
					this._label.Style.TextColor = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, (this._propertyEditor.ValueEditor.Value != null) ? 200 : 80);
				}
			}
		}

		// Token: 0x06005EF6 RID: 24310 RVA: 0x001E8F54 File Offset: 0x001E7154
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			bool hasErrors = this._propertyEditor.HasErrors;
			if (hasErrors)
			{
				this._diagnosticsIcon.Visible = true;
				this._diagnosticsIcon.Background = PropertyLabel.IconError;
			}
			else
			{
				bool hasWarnings = this._propertyEditor.HasWarnings;
				if (hasWarnings)
				{
					this._diagnosticsIcon.Visible = true;
					this._diagnosticsIcon.Background = PropertyLabel.IconWarning;
				}
				else
				{
					this._diagnosticsIcon.Visible = false;
				}
			}
			bool flag = this._collapseIcon != null;
			if (flag)
			{
				this._collapseIcon.Background = (this._propertyEditor.IsCollapsed ? PropertyLabel.IconCollapsed : PropertyLabel.IconUncollapsed);
			}
		}

		// Token: 0x06005EF7 RID: 24311 RVA: 0x001E900B File Offset: 0x001E720B
		public override Element HitTest(Point position)
		{
			return this._anchoredRectangle.Contains(position) ? (base.HitTest(position) ?? this) : null;
		}

		// Token: 0x06005EF8 RID: 24312 RVA: 0x001E902A File Offset: 0x001E722A
		private void OnRemoveButtonActivating()
		{
			this._removeButton.Visible = false;
			this._propertyEditor.HandleRemoveProperty(false);
		}

		// Token: 0x06005EF9 RID: 24313 RVA: 0x001E9048 File Offset: 0x001E7248
		protected override void OnMouseEnter()
		{
			AssetEditorOverlay assetEditorOverlay = this._propertyEditor.ConfigEditor.AssetEditorOverlay;
			List<Label.LabelSpan> list = new List<Label.LabelSpan>();
			SchemaNode.NodeType type = this._propertyEditor.ParentSchema.Type;
			SchemaNode.NodeType nodeType = type;
			if (nodeType != SchemaNode.NodeType.List)
			{
				if (nodeType != SchemaNode.NodeType.Map)
				{
					list.Add(new Label.LabelSpan
					{
						Text = (this._propertyEditor.DisplayName ?? this._propertyEditor.PropertyName),
						IsBold = true
					});
					list.Add(new Label.LabelSpan
					{
						Text = " (" + this._propertyEditor.PropertyName + ")"
					});
				}
				else
				{
					list.Add(new Label.LabelSpan
					{
						Text = this.Desktop.Provider.GetText("ui.assetEditor.property.tooltip.mapItem", new Dictionary<string, string>
						{
							{
								"key",
								this._propertyEditor.PropertyName
							}
						}, true),
						IsBold = true
					});
				}
			}
			else
			{
				list.Add(new Label.LabelSpan
				{
					Text = this.Desktop.Provider.GetText("ui.assetEditor.property.tooltip.arrayItem", new Dictionary<string, string>
					{
						{
							"index",
							this._propertyEditor.PropertyName
						}
					}, true),
					IsBold = true
				});
			}
			bool flag = this._propertyEditor.Schema.Description != null;
			if (flag)
			{
				list.Add(new Label.LabelSpan
				{
					Text = "\n"
				});
				FormattedMessageConverter.AppendLabelSpansFromMarkup(this._propertyEditor.Schema.Description, list, new SpanStyle
				{
					Color = new UInt32Color?(UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 180)),
					IsItalics = true
				});
			}
			bool hasChildErrors = this._propertyEditor.HasChildErrors;
			if (hasChildErrors)
			{
				list.Add(new Label.LabelSpan
				{
					Text = "\n\n" + this.Desktop.Provider.GetText("ui.assetEditor.diagnosticsTooltip.hasChildErrors", null, true)
				});
			}
			bool hasChildWarnings = this._propertyEditor.HasChildWarnings;
			if (hasChildWarnings)
			{
				list.Add(new Label.LabelSpan
				{
					Text = "\n\n" + this.Desktop.Provider.GetText("ui.assetEditor.diagnosticsTooltip.hasChildWarnings", null, true)
				});
			}
			AssetDiagnostics assetDiagnostics;
			bool flag2 = (this._propertyEditor.HasErrors || this._propertyEditor.HasWarnings) && assetEditorOverlay.Diagnostics.TryGetValue(assetEditorOverlay.CurrentAsset.FilePath, out assetDiagnostics);
			if (flag2)
			{
				bool flag3 = assetDiagnostics.Errors != null && assetDiagnostics.Errors.Length != 0;
				if (flag3)
				{
					bool flag4 = false;
					foreach (AssetDiagnosticMessage assetDiagnosticMessage in assetDiagnostics.Errors)
					{
						bool flag5 = !this._propertyEditor.Path.Equals(assetDiagnosticMessage.Property);
						if (!flag5)
						{
							bool flag6 = !flag4;
							if (flag6)
							{
								flag4 = true;
								list.Add(new Label.LabelSpan
								{
									Text = "\n\n" + this.Desktop.Provider.GetText("ui.assetEditor.diagnosticsTooltip.errors", null, true),
									IsBold = true
								});
							}
							list.Add(new Label.LabelSpan
							{
								Text = "\n- " + assetDiagnosticMessage.Message
							});
						}
					}
				}
				bool flag7 = assetDiagnostics.Warnings != null && assetDiagnostics.Warnings.Length != 0;
				if (flag7)
				{
					bool flag8 = false;
					foreach (AssetDiagnosticMessage assetDiagnosticMessage2 in assetDiagnostics.Warnings)
					{
						bool flag9 = !this._propertyEditor.Path.Equals(assetDiagnosticMessage2.Property);
						if (!flag9)
						{
							bool flag10 = !flag8;
							if (flag10)
							{
								flag8 = true;
								list.Add(new Label.LabelSpan
								{
									Text = "\n\n" + this.Desktop.Provider.GetText("ui.assetEditor.diagnosticsTooltip.warnings", null, true),
									IsBold = true
								});
							}
							list.Add(new Label.LabelSpan
							{
								Text = "\n- " + assetDiagnosticMessage2.Message
							});
						}
					}
				}
			}
			TextTooltipLayer textTooltipLayer = this._propertyEditor.ConfigEditor.AssetEditorOverlay.TextTooltipLayer;
			textTooltipLayer.TextSpans = list;
			textTooltipLayer.Start(false);
			bool flag11 = this._propertyEditor.ParentValueEditor is ListEditor || this._propertyEditor.ParentValueEditor is MapEditor || this._propertyEditor.ValueEditor.Value != null;
			if (flag11)
			{
				this._removeButton.Visible = true;
				base.Layout(null, true);
			}
		}

		// Token: 0x06005EFA RID: 24314 RVA: 0x001E9531 File Offset: 0x001E7731
		protected override void OnMouseLeave()
		{
			this._removeButton.Visible = false;
			this._propertyEditor.ConfigEditor.AssetEditorOverlay.TextTooltipLayer.Stop();
		}

		// Token: 0x06005EFB RID: 24315 RVA: 0x001E955C File Offset: 0x001E775C
		protected override void OnMouseStartDrag()
		{
			this._propertyEditor.ConfigEditor.AssetEditorOverlay.TextTooltipLayer.Stop();
		}

		// Token: 0x06005EFC RID: 24316 RVA: 0x001E957C File Offset: 0x001E777C
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			base.OnMouseButtonUp(evt, activate);
			bool flag = !activate || this._wasDragging;
			if (!flag)
			{
				uint button = (uint)evt.Button;
				uint num = button;
				if (num != 1U)
				{
					if (num == 3U)
					{
						this._propertyEditor.OpenContextPopup();
					}
				}
				else
				{
					bool flag2 = this._collapseIcon != null;
					if (flag2)
					{
						this._propertyEditor.SetCollapseState(this._propertyEditor.IsCollapsed, true);
					}
					int clicks = evt.Clicks;
					int num2 = clicks;
					if (num2 != 1)
					{
						if (num2 == 2)
						{
							bool flag3 = this.Desktop.FocusedElement != this || !(this._propertyEditor.ParentValueEditor is MapEditor);
							if (!flag3)
							{
								this._propertyEditor.OpenRenameKeyModal();
							}
						}
					}
					else
					{
						this.Desktop.FocusElement(this, true);
					}
				}
			}
		}

		// Token: 0x04003B2B RID: 15147
		private static readonly PatchStyle IconUncollapsed = new PatchStyle("Common/CaretUncollapsed.png")
		{
			Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 153)
		};

		// Token: 0x04003B2C RID: 15148
		private static readonly PatchStyle IconCollapsed = new PatchStyle("Common/CaretCollapsed.png")
		{
			Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 153)
		};

		// Token: 0x04003B2D RID: 15149
		private static readonly PatchStyle IconError = new PatchStyle("AssetEditor/ErrorIcon.png");

		// Token: 0x04003B2E RID: 15150
		private static readonly PatchStyle IconWarning = new PatchStyle("AssetEditor/WarningIcon.png");

		// Token: 0x04003B2F RID: 15151
		public static readonly string IconRemove = "AssetEditor/PropertyRemoveIcon.png";

		// Token: 0x04003B30 RID: 15152
		private readonly PropertyEditor _propertyEditor;

		// Token: 0x04003B31 RID: 15153
		private readonly Label _label;

		// Token: 0x04003B32 RID: 15154
		private readonly Group _diagnosticsIcon;

		// Token: 0x04003B33 RID: 15155
		private readonly Group _collapseIcon;

		// Token: 0x04003B34 RID: 15156
		private readonly Button _removeButton;
	}
}
