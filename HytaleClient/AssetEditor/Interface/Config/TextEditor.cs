using System;
using System.Collections.Generic;
using HytaleClient.AssetEditor.Data;
using HytaleClient.Graphics;
using HytaleClient.Interface.Messages;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Styles;
using Newtonsoft.Json.Linq;
using SDL2;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BCD RID: 3021
	internal class TextEditor : ValueEditor
	{
		// Token: 0x06005F0F RID: 24335 RVA: 0x001E9B64 File Offset: 0x001E7D64
		public TextEditor(Desktop desktop, Element parent, SchemaNode schema, PropertyPath path, PropertyEditor parentPropertyEditor, SchemaNode parentSchema, ConfigEditor configEditor, JToken value) : base(desktop, parent, schema, path, parentPropertyEditor, parentSchema, configEditor, value)
		{
		}

		// Token: 0x06005F10 RID: 24336 RVA: 0x001E9B88 File Offset: 0x001E7D88
		protected override void OnUnmounted()
		{
			bool flag = this.ConfigEditor.AssetEditorOverlay.AutoCompleteMenu.TextEditor == this;
			if (flag)
			{
				this.ConfigEditor.AssetEditorOverlay.AutoCompleteMenu.Close();
			}
			base.OnUnmounted();
			base.SubmitUpdateCommand();
		}

		// Token: 0x06005F11 RID: 24337 RVA: 0x001E9BD8 File Offset: 0x001E7DD8
		protected override void Build()
		{
			this._textInput = new TextField(this.Desktop, this)
			{
				Value = (((string)base.Value) ?? ""),
				Padding = new Padding
				{
					Left = new int?(5)
				},
				PlaceholderText = (((string)this.Schema.DefaultValue) ?? ""),
				PlaceholderStyle = new InputFieldStyle
				{
					TextColor = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 100),
					FontSize = 14f
				},
				Style = new InputFieldStyle
				{
					FontSize = 14f
				},
				KeyDown = new Action<SDL.SDL_Keycode>(this.OnTextInputKeyPress),
				Focused = new Action(this.OnTextInputFocus),
				Blurred = new Action(this.OnTextInputBlur),
				Validating = new Action(this.OnTextInputValidate),
				Dismissing = new Action(this.OnTextInputDismissing),
				ValueChanged = new Action(this.OnTextInputChange),
				MaxLength = this.Schema.MaxLength,
				Decoration = new InputFieldDecorationStyle
				{
					Focused = new InputFieldDecorationStyleState
					{
						OutlineSize = new int?(1),
						OutlineColor = new UInt32Color?(UInt32Color.FromRGBA(244, 188, 81, 153))
					}
				}
			};
		}

		// Token: 0x06005F12 RID: 24338 RVA: 0x001E9D58 File Offset: 0x001E7F58
		private void OnTextInputChange()
		{
			bool flag = this.Schema.DataSet != null;
			if (flag)
			{
				bool flag2 = this.ConfigEditor.AssetEditorOverlay.AutoCompleteMenu.TextEditor == null;
				if (flag2)
				{
					this.ConfigEditor.AssetEditorOverlay.AutoCompleteMenu.Open(this, this.Desktop.UnscaleRound((float)this._textInput.AnchoredRectangle.X), this.Desktop.UnscaleRound((float)this._rectangleAfterPadding.Bottom), this.Desktop.UnscaleRound((float)this._textInput.AnchoredRectangle.Width));
				}
				this.UpdateAutoComplete();
			}
			base.HandleChangeValue(this._textInput.Value, true, false, false);
		}

		// Token: 0x06005F13 RID: 24339 RVA: 0x001E9E22 File Offset: 0x001E8022
		private void OnTextInputValidate()
		{
			this.Validate();
			base.SubmitUpdateCommand();
		}

		// Token: 0x06005F14 RID: 24340 RVA: 0x001E9E34 File Offset: 0x001E8034
		private void OnTextInputDismissing()
		{
			bool flag = this.ConfigEditor.AssetEditorOverlay.AutoCompleteMenu.TextEditor == this;
			if (flag)
			{
				this.ConfigEditor.AssetEditorOverlay.AutoCompleteMenu.Close();
			}
			else
			{
				this.Dismiss();
			}
		}

		// Token: 0x06005F15 RID: 24341 RVA: 0x001E9E80 File Offset: 0x001E8080
		private void OnTextInputBlur()
		{
			bool flag = this.ConfigEditor.AssetEditorOverlay.AutoCompleteMenu.TextEditor == this && !(this.Desktop.CapturedElement is AutoCompleteMenu.AutoCompleteMenuButton);
			if (flag)
			{
				this.ConfigEditor.AssetEditorOverlay.AutoCompleteMenu.Close();
			}
			base.SubmitUpdateCommand();
		}

		// Token: 0x06005F16 RID: 24342 RVA: 0x001E9EE4 File Offset: 0x001E80E4
		private void OnTextInputFocus()
		{
			bool flag = this.Schema.DataSet != null;
			if (flag)
			{
				this.ConfigEditor.AssetEditorOverlay.AutoCompleteMenu.Open(this, this.Desktop.UnscaleRound((float)this._textInput.AnchoredRectangle.X), this.Desktop.UnscaleRound((float)this._rectangleAfterPadding.Bottom), this.Desktop.UnscaleRound((float)this._textInput.AnchoredRectangle.Width));
				this.UpdateAutoComplete();
			}
		}

		// Token: 0x06005F17 RID: 24343 RVA: 0x001E9F74 File Offset: 0x001E8174
		private void OnTextInputKeyPress(SDL.SDL_Keycode keycode)
		{
			bool flag = !this.ConfigEditor.AssetEditorOverlay.AutoCompleteMenu.IsMounted;
			if (!flag)
			{
				this.ConfigEditor.AssetEditorOverlay.AutoCompleteMenu.OnKeyDown(keycode, 0);
			}
		}

		// Token: 0x06005F18 RID: 24344 RVA: 0x001E9FB8 File Offset: 0x001E81B8
		internal void OnAutoCompleteSelectedValue(string text)
		{
			this._textInput.Value = text;
			base.HandleChangeValue(this._textInput.Value, false, false, false);
		}

		// Token: 0x06005F19 RID: 24345 RVA: 0x001E9FE4 File Offset: 0x001E81E4
		private void UpdateAutoComplete()
		{
			AutoCompleteMenu autoCompleteMenu = this.ConfigEditor.AssetEditorOverlay.AutoCompleteMenu;
			bool flag = autoCompleteMenu.Parent == null;
			if (!flag)
			{
				this.ConfigEditor.AssetEditorOverlay.Backend.FetchAutoCompleteData(this.Schema.DataSet, this._textInput.Value.Trim(), delegate(HashSet<string> results, FormattedMessage error)
				{
					bool flag2 = autoCompleteMenu.Parent == null;
					if (!flag2)
					{
						autoCompleteMenu.SetupResults(results ?? new HashSet<string>());
					}
				});
			}
		}

		// Token: 0x06005F1A RID: 24346 RVA: 0x001EA060 File Offset: 0x001E8260
		protected override bool IsValueEmptyOrDefault(JToken value)
		{
			bool flag = (string)value == "";
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = this.Schema.DefaultValue != null && (string)this.Schema.DefaultValue == (string)value;
				result = flag2;
			}
			return result;
		}

		// Token: 0x06005F1B RID: 24347 RVA: 0x001EA0BE File Offset: 0x001E82BE
		public override void Focus()
		{
			this.Desktop.FocusElement(this._textInput, true);
		}

		// Token: 0x06005F1C RID: 24348 RVA: 0x001EA0D3 File Offset: 0x001E82D3
		public void SelectAll()
		{
			this._textInput.SelectAll();
		}

		// Token: 0x06005F1D RID: 24349 RVA: 0x001EA0E1 File Offset: 0x001E82E1
		protected internal override void UpdateDisplayedValue()
		{
			this._textInput.Value = (((string)base.Value) ?? "");
		}

		// Token: 0x06005F1E RID: 24350 RVA: 0x001EA103 File Offset: 0x001E8303
		protected override bool ValidateType(JToken value)
		{
			return value.Type == 8;
		}

		// Token: 0x06005F1F RID: 24351 RVA: 0x001EA110 File Offset: 0x001E8310
		protected override JToken SanitizeValue(JToken value)
		{
			string text = (string)value;
			bool flag = this.Schema.MaxLength > 0 && text.Length > this.Schema.MaxLength;
			if (flag)
			{
				value = text.Substring(0, this.Schema.MaxLength);
			}
			return value;
		}

		// Token: 0x06005F20 RID: 24352 RVA: 0x001EA170 File Offset: 0x001E8370
		public override void PasteValue(string text)
		{
			JToken value = this.SanitizeValue(text);
			base.HandleChangeValue(value, false, false, true);
			this.ConfigEditor.Layout(null, true);
		}

		// Token: 0x04003B39 RID: 15161
		private TextField _textInput;
	}
}
