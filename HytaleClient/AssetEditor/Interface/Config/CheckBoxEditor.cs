using System;
using HytaleClient.AssetEditor.Data;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using Newtonsoft.Json.Linq;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BBA RID: 3002
	internal class CheckBoxEditor : ValueEditor
	{
		// Token: 0x06005DF7 RID: 24055 RVA: 0x001DFA6C File Offset: 0x001DDC6C
		public CheckBoxEditor(Desktop desktop, Element parent, SchemaNode schema, PropertyPath path, PropertyEditor parentPropertyEditor, SchemaNode parentSchema, ConfigEditor configEditor, JToken value) : base(desktop, parent, schema, path, parentPropertyEditor, parentSchema, configEditor, value)
		{
		}

		// Token: 0x06005DF8 RID: 24056 RVA: 0x001DFA90 File Offset: 0x001DDC90
		protected override void Build()
		{
			this._checkBox = new CheckBox(this.Desktop, this)
			{
				Anchor = new Anchor
				{
					Width = new int?(24),
					Height = new int?(24),
					Left = new int?(4),
					Top = new int?(4)
				},
				ValueChanged = delegate()
				{
					base.HandleChangeValue(this._checkBox.Value, false, false, false);
					this.Validate();
				},
				Value = (bool)(base.Value ?? ((bool)this.Schema.DefaultValue)),
				Style = this.ConfigEditor.CheckBoxStyle
			};
		}

		// Token: 0x06005DF9 RID: 24057 RVA: 0x001DFB44 File Offset: 0x001DDD44
		protected internal override void UpdateDisplayedValue()
		{
			this._checkBox.Value = (bool)(base.Value ?? ((bool)this.Schema.DefaultValue));
			this._checkBox.Layout(null, true);
		}

		// Token: 0x06005DFA RID: 24058 RVA: 0x001DFB98 File Offset: 0x001DDD98
		protected override bool IsValueEmptyOrDefault(JToken value)
		{
			return (bool)value == (bool)this.Schema.DefaultValue;
		}

		// Token: 0x06005DFB RID: 24059 RVA: 0x001DFBC2 File Offset: 0x001DDDC2
		protected override bool ValidateType(JToken value)
		{
			return value.Type == 9;
		}

		// Token: 0x04003AB9 RID: 15033
		private CheckBox _checkBox;
	}
}
