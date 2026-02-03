using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Interface.Modals;
using HytaleClient.Graphics;
using HytaleClient.Interface.Messages;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Styles;
using Newtonsoft.Json.Linq;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BB7 RID: 2999
	internal class AssetReferenceOrInlineEditor : ObjectEditor
	{
		// Token: 0x06005DDF RID: 24031 RVA: 0x001DEFAC File Offset: 0x001DD1AC
		public AssetReferenceOrInlineEditor(Desktop desktop, Element parent, SchemaNode schema, PropertyPath path, PropertyEditor parentPropertyEditor, SchemaNode parentSchema, ConfigEditor configEditor, JToken value) : base(desktop, parent, schema, path, parentPropertyEditor, parentSchema, configEditor, value)
		{
			this._layoutMode = ((value == null) ? LayoutMode.Left : LayoutMode.Top);
		}

		// Token: 0x06005DE0 RID: 24032 RVA: 0x001DEFDC File Offset: 0x001DD1DC
		protected override void Build()
		{
			this._referenceEditor = null;
			bool flag = base.Value == null;
			if (flag)
			{
				this._layoutMode = LayoutMode.Left;
				new Group(this.Desktop, this).Anchor = new Anchor
				{
					Width = new int?(250)
				};
				Group group = new Group(this.Desktop, this);
				group.Anchor = new Anchor
				{
					Width = new int?(1)
				};
				group.Background = new PatchStyle(PropertyEditor.BorderColor);
				AssetReferenceOrInlineEditor.<>c__DisplayClass2_0 CS$<>8__locals1;
				CS$<>8__locals1.group = new Group(this.Desktop, this)
				{
					FlexWeight = 1,
					LayoutMode = LayoutMode.Left
				};
				this.<Build>g__CreateButton|2_0("ui.assetEditor.assetReferenceOrInlineEditor.useExisting", new Action(this.OnUseExisting), ref CS$<>8__locals1);
				this.<Build>g__CreateButton|2_0("ui.assetEditor.assetReferenceOrInlineEditor.createEmbedded", new Action(this.OnCreateEmbedded), ref CS$<>8__locals1);
			}
			else
			{
				bool flag2 = base.Value.Type == 8;
				if (flag2)
				{
					this._layoutMode = LayoutMode.Full;
					string value = this.Schema.AssetType;
					AssetTypeConfig assetTypeConfig;
					bool flag3 = this.ConfigEditor.AssetEditorOverlay.AssetTypeRegistry.AssetTypes.TryGetValue(this.Schema.AssetType, out assetTypeConfig);
					if (flag3)
					{
						value = assetTypeConfig.Name;
					}
					SchemaNode schema = new SchemaNode
					{
						Type = SchemaNode.NodeType.AssetIdDropdown,
						AssetType = this.Schema.AssetType
					};
					string text = this.Desktop.Provider.GetText("ui.assetEditor.assetReferenceOrInlineEditor.reference", new Dictionary<string, string>
					{
						{
							"assetType",
							value
						}
					}, true);
					this._referenceEditor = new PropertyEditor(this.Desktop, this, null, schema, this.Path, this.Schema, this.ConfigEditor, this, text, false);
					this._referenceEditor.Build(base.Value, false, this.IsDetachedEditor, null);
				}
				else
				{
					this._layoutMode = LayoutMode.Top;
					base.Build(this.ConfigEditor.AssetEditorOverlay.ResolveSchemaInCurrentContext(this.Schema.Value));
				}
			}
		}

		// Token: 0x06005DE1 RID: 24033 RVA: 0x001DF1EC File Offset: 0x001DD3EC
		private void OnCreateEmbedded()
		{
			base.HandleChangeValue(new JObject(), false, false, false);
			this.ConfigEditor.Layout(null, true);
		}

		// Token: 0x06005DE2 RID: 24034 RVA: 0x001DF220 File Offset: 0x001DD420
		private void OnUseExisting()
		{
			base.HandleChangeValue("", false, false, false);
			this.ConfigEditor.Layout(null, true);
			this._referenceEditor.ValueEditor.Focus();
		}

		// Token: 0x06005DE3 RID: 24035 RVA: 0x001DF269 File Offset: 0x001DD469
		public override void SetValue(JToken value)
		{
			base.SetValue(value);
			PropertyEditor referenceEditor = this._referenceEditor;
			if (referenceEditor != null)
			{
				referenceEditor.ValueEditor.SetValue(value);
			}
		}

		// Token: 0x06005DE4 RID: 24036 RVA: 0x001DF28C File Offset: 0x001DD48C
		public override void SetValueRecursively(JToken value)
		{
			JToken value2 = base.Value;
			bool flag = value2 != null && value2.Type == 8 && value != null && value.Type == 8;
			this.SetValue(value);
			bool flag2 = !flag;
			if (flag2)
			{
				this._properties.Clear();
				base.Clear();
				this.Build();
				this.ConfigEditor.UpdateCategories();
			}
		}

		// Token: 0x06005DE5 RID: 24037 RVA: 0x001DF2FC File Offset: 0x001DD4FC
		protected internal override void UpdateDisplayedValue()
		{
			PropertyEditor referenceEditor = this._referenceEditor;
			if (referenceEditor != null)
			{
				referenceEditor.ValueEditor.UpdateDisplayedValue();
			}
		}

		// Token: 0x06005DE6 RID: 24038 RVA: 0x001DF318 File Offset: 0x001DD518
		public void CreateDedicatedAsset()
		{
			Debug.Assert(base.Value is JObject);
			string currentAssetId = this.ConfigEditor.GetCurrentAssetId();
			CreateAssetModal createAssetModal = this.ConfigEditor.AssetEditorOverlay.CreateAssetModal;
			string assetType = this.Schema.AssetType;
			string assetToCopyPath = null;
			string path = null;
			JObject json = (JObject)base.Value;
			createAssetModal.Open(assetType, assetToCopyPath, path, currentAssetId, json, delegate(string filePath, FormattedMessage error)
			{
				bool flag = error != null || !base.IsMounted;
				if (!flag)
				{
					base.HandleChangeValue(this.ConfigEditor.AssetEditorOverlay.GetAssetIdFromReference(new AssetReference(this.Schema.AssetType, filePath)), false, false, false);
					this.Validate();
					this.ConfigEditor.Layout(null, true);
				}
			});
		}

		// Token: 0x06005DE7 RID: 24039 RVA: 0x001DF384 File Offset: 0x001DD584
		public void EmbedReference()
		{
			JObject jobject = new JObject();
			jobject.Add("Parent", (string)base.Value);
			base.HandleChangeValue(jobject, false, false, false);
			base.Clear();
			this.Build();
			this.ConfigEditor.Layout(null, true);
		}

		// Token: 0x06005DE8 RID: 24040 RVA: 0x001DF3E1 File Offset: 0x001DD5E1
		protected override bool ValidateType(JToken value)
		{
			return value.Type == 8 || value.Type == 1;
		}

		// Token: 0x06005DE9 RID: 24041 RVA: 0x001DF3F8 File Offset: 0x001DD5F8
		[CompilerGenerated]
		private void <Build>g__CreateButton|2_0(string messageId, Action action, ref AssetReferenceOrInlineEditor.<>c__DisplayClass2_0 A_3)
		{
			TextButton textButton = new TextButton(this.Desktop, A_3.group);
			textButton.Text = this.Desktop.Provider.GetText(messageId, null, true);
			textButton.Padding = new Padding
			{
				Horizontal = new int?(8),
				Vertical = new int?(6)
			};
			textButton.Anchor = new Anchor
			{
				Full = new int?(2),
				Right = new int?(0)
			};
			textButton.Activating = action;
			textButton.Style = new TextButton.TextButtonStyle
			{
				Default = new TextButton.TextButtonStyleState
				{
					Background = new PatchStyle(UInt32Color.FromRGBA(0, 0, 0, 150)),
					LabelStyle = new LabelStyle
					{
						FontSize = 13f
					}
				},
				Hovered = new TextButton.TextButtonStyleState
				{
					Background = new PatchStyle(UInt32Color.FromRGBA(0, 0, 0, 180)),
					LabelStyle = new LabelStyle
					{
						FontSize = 13f
					}
				},
				Pressed = new TextButton.TextButtonStyleState
				{
					Background = new PatchStyle(UInt32Color.FromRGBA(0, 0, 0, 165)),
					LabelStyle = new LabelStyle
					{
						FontSize = 13f
					}
				}
			};
		}

		// Token: 0x04003AAF RID: 15023
		private PropertyEditor _referenceEditor;
	}
}
