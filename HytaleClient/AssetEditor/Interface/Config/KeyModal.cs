using System;
using System.Diagnostics;
using HytaleClient.AssetEditor.Data;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BC4 RID: 3012
	internal class KeyModal : Element
	{
		// Token: 0x06005E89 RID: 24201 RVA: 0x001E4204 File Offset: 0x001E2404
		public KeyModal(ConfigEditor configEditor) : base(configEditor.Desktop, null)
		{
			this._configEditor = configEditor;
		}

		// Token: 0x06005E8A RID: 24202 RVA: 0x001E421C File Offset: 0x001E241C
		public void Build()
		{
			base.Clear();
			Document document;
			this.Desktop.Provider.TryGetDocument("AssetEditor/KeyModal.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._saveButton = uifragment.Get<TextButton>("SaveButton");
			this._saveButton.Activating = new Action(this.Validate);
			uifragment.Get<TextButton>("CancelButton").Activating = new Action(this.Dismiss);
			this._inputContainer = uifragment.Get<Group>("InputContainer");
			this._container = uifragment.Get<Group>("Container");
			this._errorLabel = uifragment.Get<Label>("ErrorMessage");
			this._titleLabel = uifragment.Get<Label>("Title");
		}

		// Token: 0x06005E8B RID: 24203 RVA: 0x001E42E1 File Offset: 0x001E24E1
		public void OpenEditKey(string key, PropertyPath parentPropertyPath, SchemaNode schema, ConfigEditor editor)
		{
			Debug.Assert(key != null);
			this._key = key;
			this.Open(parentPropertyPath, schema, editor);
		}

		// Token: 0x06005E8C RID: 24204 RVA: 0x001E4300 File Offset: 0x001E2500
		public void OpenInsertKey(PropertyPath parentPropertyPath, SchemaNode schema, ConfigEditor editor)
		{
			this._key = null;
			this.Open(parentPropertyPath, schema, editor);
		}

		// Token: 0x06005E8D RID: 24205 RVA: 0x001E4314 File Offset: 0x001E2514
		private void Open(PropertyPath parentPropertyPath, SchemaNode schema, ConfigEditor editor)
		{
			this._parentPropertyPath = parentPropertyPath;
			this._errorLabel.Visible = false;
			this._input = ValueEditor.CreateFromSchema(this._inputContainer, schema, PropertyPath.Root, null, null, editor, null);
			this._input.IsDetachedEditor = true;
			this._input.BuildEditor();
			bool flag = this._key != null;
			if (flag)
			{
				this._input.SetValue(this._key);
				this._input.UpdateDisplayedValue();
				this._titleLabel.Text = this.Desktop.Provider.GetText("ui.assetEditor.keyModal.renameTitle", null, true);
				this._saveButton.Text = this.Desktop.Provider.GetText("ui.assetEditor.keyModal.renameButton", null, true);
			}
			else
			{
				this._titleLabel.Text = this.Desktop.Provider.GetText("ui.assetEditor.keyModal.insertTitle", null, true);
				this._saveButton.Text = this.Desktop.Provider.GetText("ui.assetEditor.keyModal.insertButton", null, true);
			}
			this.Desktop.SetLayer(4, this);
			this._input.Focus();
			TextEditor textEditor = this._input as TextEditor;
			bool flag2 = textEditor != null;
			if (flag2)
			{
				textEditor.SelectAll();
			}
		}

		// Token: 0x06005E8E RID: 24206 RVA: 0x001E4460 File Offset: 0x001E2660
		protected override void OnUnmounted()
		{
			this._inputContainer.Clear();
			this._input = null;
		}

		// Token: 0x06005E8F RID: 24207 RVA: 0x001E4478 File Offset: 0x001E2678
		protected internal override void Validate()
		{
			string text = (this._input.Value != null) ? ((string)this._input.Value).Trim() : "";
			bool flag = text == "";
			if (flag)
			{
				this.SetError(this.Desktop.Provider.GetText("ui.assetEditor.keyModal.errors.fieldEmpty", null, true));
			}
			else
			{
				PropertyEditor propertyEditor;
				bool flag2 = this._configEditor.TryFindPropertyEditor(this._parentPropertyPath, out propertyEditor);
				if (flag2)
				{
					ValueEditor valueEditor = propertyEditor.ValueEditor;
					ValueEditor valueEditor2 = valueEditor;
					MapEditor mapEditor = valueEditor2 as MapEditor;
					if (mapEditor == null)
					{
						WeightedTimelineEditor weightedTimelineEditor = valueEditor2 as WeightedTimelineEditor;
						if (weightedTimelineEditor == null)
						{
							this.SetError(this.Desktop.Provider.GetText("ui.assetEditor.keyModal.errors.invalidProperty", null, true));
						}
						else if (!weightedTimelineEditor.HasEntryId(text))
						{
							WeightedTimelineEditor weightedTimelineEditor2 = weightedTimelineEditor;
							weightedTimelineEditor2.HandleInsertEntry(text);
							this.Desktop.ClearLayer(4);
						}
						else
						{
							this.SetError(this.Desktop.Provider.GetText("ui.assetEditor.keyModal.errors.existingKey", null, true));
						}
					}
					else if (!mapEditor.HasItemWithKey(text))
					{
						MapEditor mapEditor2 = mapEditor;
						bool flag3 = this._key != null;
						if (flag3)
						{
							mapEditor2.HandleRenameKey(this._key, text);
						}
						else
						{
							mapEditor2.HandleInsertKey(text);
						}
						this.Desktop.ClearLayer(4);
					}
					else
					{
						this.SetError(this.Desktop.Provider.GetText("ui.assetEditor.keyModal.errors.existingKey", null, true));
					}
				}
				else
				{
					this.SetError(this.Desktop.Provider.GetText("ui.assetEditor.keyModal.errors.invalidProperty", null, true));
				}
			}
		}

		// Token: 0x06005E90 RID: 24208 RVA: 0x001E4624 File Offset: 0x001E2824
		private void SetError(string message)
		{
			this._errorLabel.Text = message;
			this._errorLabel.Visible = true;
			base.Layout(null, true);
		}

		// Token: 0x06005E91 RID: 24209 RVA: 0x001E465D File Offset: 0x001E285D
		protected internal override void Dismiss()
		{
			this.Desktop.ClearLayer(4);
		}

		// Token: 0x06005E92 RID: 24210 RVA: 0x001E466D File Offset: 0x001E286D
		public override Element HitTest(Point position)
		{
			return base.HitTest(position) ?? this;
		}

		// Token: 0x06005E93 RID: 24211 RVA: 0x001E467C File Offset: 0x001E287C
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			base.OnMouseButtonUp(evt, activate);
			bool flag = activate && !this._container.AnchoredRectangle.Contains(this.Desktop.MousePosition);
			if (flag)
			{
				this.Dismiss();
			}
		}

		// Token: 0x04003B06 RID: 15110
		private Group _container;

		// Token: 0x04003B07 RID: 15111
		private Group _inputContainer;

		// Token: 0x04003B08 RID: 15112
		private ValueEditor _input;

		// Token: 0x04003B09 RID: 15113
		private Label _errorLabel;

		// Token: 0x04003B0A RID: 15114
		private TextButton _saveButton;

		// Token: 0x04003B0B RID: 15115
		private Label _titleLabel;

		// Token: 0x04003B0C RID: 15116
		private string _key;

		// Token: 0x04003B0D RID: 15117
		private PropertyPath _parentPropertyPath;

		// Token: 0x04003B0E RID: 15118
		private readonly ConfigEditor _configEditor;
	}
}
