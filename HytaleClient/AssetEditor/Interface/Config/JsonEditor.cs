using System;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Utils;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Styles;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BC3 RID: 3011
	internal class JsonEditor : ValueEditor
	{
		// Token: 0x06005E82 RID: 24194 RVA: 0x001E3E50 File Offset: 0x001E2050
		public JsonEditor(Desktop desktop, Element parent, SchemaNode schema, PropertyPath path, PropertyEditor parentPropertyEditor, SchemaNode parentSchema, ConfigEditor configEditor, JToken value) : base(desktop, parent, schema, path, parentPropertyEditor, parentSchema, configEditor, value)
		{
		}

		// Token: 0x06005E83 RID: 24195 RVA: 0x001E3E74 File Offset: 0x001E2074
		protected override void Build()
		{
			this._layoutMode = LayoutMode.Top;
			this._statusLabel = new Label(this.Desktop, this)
			{
				Text = this.Desktop.Provider.GetText("ui.assetEditor.jsonObjectEditor.json", null, true),
				Style = 
				{
					VerticalAlignment = LabelStyle.LabelAlignment.Center,
					FontSize = 13f
				},
				Anchor = new Anchor
				{
					Height = new int?(30)
				},
				Padding = new Padding
				{
					Horizontal = new int?(7)
				}
			};
			TextField textField = new TextField(this.Desktop, this);
			JToken value = base.Value;
			textField.Value = (((value != null) ? value.ToString(0, Array.Empty<JsonConverter>()) : null) ?? "");
			textField.Anchor = new Anchor
			{
				Height = new int?(40)
			};
			textField.Padding = new Padding
			{
				Left = new int?(5)
			};
			textField.PlaceholderText = (((string)this.Schema.DefaultValue) ?? "");
			textField.PlaceholderStyle = new InputFieldStyle
			{
				TextColor = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 100)
			};
			textField.Style = new InputFieldStyle
			{
				FontSize = 14f
			};
			textField.MaxLength = this.Schema.MaxLength;
			textField.Validating = new Action(this.OnTextInputValidating);
			textField.Blurred = new Action(this.OnTextInputBlurred);
			textField.Decoration = new InputFieldDecorationStyle
			{
				Default = new InputFieldDecorationStyleState
				{
					Background = new PatchStyle(UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 20))
				},
				Focused = new InputFieldDecorationStyleState
				{
					OutlineSize = new int?(1),
					OutlineColor = new UInt32Color?(UInt32Color.FromRGBA(205, 240, 252, 206))
				}
			};
			this._textInput = textField;
		}

		// Token: 0x06005E84 RID: 24196 RVA: 0x001E4088 File Offset: 0x001E2288
		protected internal override void UpdateDisplayedValue()
		{
			this._statusLabel.Text = this.Desktop.Provider.GetText("ui.assetEditor.jsonObjectEditor.json", null, true);
			this._statusLabel.Layout(null, true);
			InputElement<string> textInput = this._textInput;
			JToken value = base.Value;
			textInput.Value = (((value != null) ? value.ToString(0, Array.Empty<JsonConverter>()) : null) ?? "");
		}

		// Token: 0x06005E85 RID: 24197 RVA: 0x001E40FC File Offset: 0x001E22FC
		protected override bool ValidateType(JToken value)
		{
			return value.Type == 1;
		}

		// Token: 0x06005E86 RID: 24198 RVA: 0x001E4108 File Offset: 0x001E2308
		private void OnTextInputBlurred()
		{
			bool flag = !this.TryUpdateValue();
			if (flag)
			{
				this.UpdateDisplayedValue();
			}
		}

		// Token: 0x06005E87 RID: 24199 RVA: 0x001E412C File Offset: 0x001E232C
		private void OnTextInputValidating()
		{
			this.TryUpdateValue();
		}

		// Token: 0x06005E88 RID: 24200 RVA: 0x001E4138 File Offset: 0x001E2338
		private bool TryUpdateValue()
		{
			JObject value;
			try
			{
				JsonUtils.ValidateJson(this._textInput.Value);
				value = JObject.Parse(this._textInput.Value);
			}
			catch (JsonReaderException)
			{
				this._statusLabel.Text = this.Desktop.Provider.GetText("ui.assetEditor.jsonObjectEditor.jsonParseError", null, true);
				this._statusLabel.Layout(null, true);
				return false;
			}
			this._statusLabel.Text = this.Desktop.Provider.GetText("ui.assetEditor.jsonObjectEditor.json", null, true);
			this._statusLabel.Layout(null, true);
			base.HandleChangeValue(value, false, false, false);
			return true;
		}

		// Token: 0x04003B04 RID: 15108
		private TextField _textInput;

		// Token: 0x04003B05 RID: 15109
		private Label _statusLabel;
	}
}
