using System;
using System.Collections.Generic;
using HytaleClient.AssetEditor.Data;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Styles;
using Newtonsoft.Json.Linq;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BC0 RID: 3008
	internal class DropdownEditor : ValueEditor
	{
		// Token: 0x06005E5A RID: 24154 RVA: 0x001E2928 File Offset: 0x001E0B28
		public DropdownEditor(Desktop desktop, Element parent, SchemaNode schema, PropertyPath path, PropertyEditor parentPropertyEditor, SchemaNode parentSchema, ConfigEditor configEditor, JToken value) : base(desktop, parent, schema, path, parentPropertyEditor, parentSchema, configEditor, value)
		{
		}

		// Token: 0x06005E5B RID: 24155 RVA: 0x001E294C File Offset: 0x001E0B4C
		protected override void OnMounted()
		{
			base.OnMounted();
			bool flag;
			if (!this._isRegistered)
			{
				SchemaNode schema = this.Schema;
				if (((schema != null) ? schema.DataSet : null) != null)
				{
					flag = (this._dropdown != null);
					goto IL_30;
				}
			}
			flag = false;
			IL_30:
			bool flag2 = flag;
			if (flag2)
			{
				this._isRegistered = true;
				this.ConfigEditor.AssetEditorOverlay.RegisterDropdownWithDataset(this.Schema.DataSet, this._dropdown, this.GetGradientSetValue());
			}
		}

		// Token: 0x06005E5C RID: 24156 RVA: 0x001E29C0 File Offset: 0x001E0BC0
		protected override void OnUnmounted()
		{
			base.OnUnmounted();
			bool flag = this._isRegistered && this.Schema.DataSet != null;
			if (flag)
			{
				this._isRegistered = false;
				this.ConfigEditor.AssetEditorOverlay.UnregisterDropdownWithDataset(this.Schema.DataSet, this._dropdown);
			}
		}

		// Token: 0x06005E5D RID: 24157 RVA: 0x001E2A20 File Offset: 0x001E0C20
		private string GetGradientSetValue()
		{
			bool flag = this.Schema.DataSet != "GradientIds";
			string result;
			if (flag)
			{
				result = null;
			}
			else
			{
				PropertyEditor parentPropertyEditor = this.ParentPropertyEditor;
				JToken jtoken;
				if (parentPropertyEditor == null)
				{
					jtoken = null;
				}
				else
				{
					ValueEditor parentValueEditor = parentPropertyEditor.ParentValueEditor;
					if (parentValueEditor == null)
					{
						jtoken = null;
					}
					else
					{
						JToken value = parentValueEditor.Value;
						jtoken = ((value != null) ? value["GradientSet"] : null);
					}
				}
				JToken jtoken2 = jtoken;
				bool flag2 = jtoken2 == null;
				if (flag2)
				{
					result = null;
				}
				else
				{
					result = (string)jtoken2;
				}
			}
			return result;
		}

		// Token: 0x06005E5E RID: 24158 RVA: 0x001E2A94 File Offset: 0x001E0C94
		protected override void Build()
		{
			List<DropdownBox.DropdownEntryInfo> list = new List<DropdownBox.DropdownEntryInfo>();
			bool flag = this.Schema.DataSet == null;
			if (flag)
			{
				for (int i = 0; i < this.Schema.Enum.Length; i++)
				{
					string text = this.Schema.Enum[i];
					list.Add(new DropdownBox.DropdownEntryInfo(this.Schema.EnumTitles[i] ?? text, text, false));
				}
			}
			this._dropdown = new DropdownBox(this.Desktop, this)
			{
				ValueChanged = delegate()
				{
					base.HandleChangeValue(this._dropdown.Value, false, false, false);
					this.Validate();
				},
				DropdownToggled = delegate
				{
					bool flag6 = !this._dropdown.IsOpen || this.Schema.DataSet != "GradientIds";
					if (!flag6)
					{
						this.ConfigEditor.AssetEditorOverlay.UpdateDropdownDataset(this.Schema.DataSet, this._dropdown, this.GetGradientSetValue());
					}
				},
				DisplayNonExistingValue = true,
				Entries = list,
				Style = this.ConfigEditor.DropdownBoxStyle,
				ShowSearchInput = true
			};
			this._dropdown.Style.PanelScrollbarStyle = ScrollbarStyle.MakeDefault();
			this._dropdown.Style.PanelScrollbarStyle.Size = 5;
			bool flag2 = base.Value != null;
			if (flag2)
			{
				this._dropdown.Value = (string)base.Value;
			}
			else
			{
				bool flag3 = this.Schema.DefaultValue != null;
				if (flag3)
				{
					this._dropdown.Value = (string)this.Schema.DefaultValue;
				}
				else
				{
					SchemaNode parentSchema = this._parentSchema;
					bool flag4 = ((parentSchema != null) ? parentSchema.DefaultTypeSchema : null) != null;
					if (flag4)
					{
						this._dropdown.Value = this._parentSchema.DefaultTypeSchema;
					}
				}
			}
			bool flag5 = base.IsMounted && this.Schema.DataSet != null && !this._isRegistered;
			if (flag5)
			{
				this._isRegistered = true;
				this.ConfigEditor.AssetEditorOverlay.RegisterDropdownWithDataset(this.Schema.DataSet, this._dropdown, null);
			}
		}

		// Token: 0x06005E5F RID: 24159 RVA: 0x001E2C74 File Offset: 0x001E0E74
		protected override bool IsValueEmptyOrDefault(JToken value)
		{
			return this.Schema.DefaultValue != null && (string)this.Schema.DefaultValue == (string)value;
		}

		// Token: 0x06005E60 RID: 24160 RVA: 0x001E2CB4 File Offset: 0x001E0EB4
		protected internal override void UpdateDisplayedValue()
		{
			bool flag = base.Value != null;
			if (flag)
			{
				this._dropdown.Value = (string)base.Value;
			}
			else
			{
				bool flag2 = this.Schema.DefaultValue != null;
				if (flag2)
				{
					this._dropdown.Value = (string)this.Schema.DefaultValue;
				}
				else
				{
					SchemaNode parentSchema = this._parentSchema;
					bool flag3 = ((parentSchema != null) ? parentSchema.DefaultTypeSchema : null) != null;
					if (flag3)
					{
						this._dropdown.Value = this._parentSchema.DefaultTypeSchema;
					}
					else
					{
						this._dropdown.Value = null;
					}
				}
			}
			this._dropdown.Layout(null, true);
		}

		// Token: 0x06005E61 RID: 24161 RVA: 0x001E2D6D File Offset: 0x001E0F6D
		public override void Focus()
		{
			this._dropdown.Open();
		}

		// Token: 0x06005E62 RID: 24162 RVA: 0x001E2D7B File Offset: 0x001E0F7B
		protected override bool ValidateType(JToken value)
		{
			return value.Type == 8;
		}

		// Token: 0x04003AEE RID: 15086
		private DropdownBox _dropdown;

		// Token: 0x04003AEF RID: 15087
		private bool _isRegistered;
	}
}
