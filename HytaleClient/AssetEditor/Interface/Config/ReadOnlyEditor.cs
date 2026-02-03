using System;
using HytaleClient.AssetEditor.Data;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Styles;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BCC RID: 3020
	internal class ReadOnlyEditor : ValueEditor
	{
		// Token: 0x06005F0C RID: 24332 RVA: 0x001E9A30 File Offset: 0x001E7C30
		public ReadOnlyEditor(Desktop desktop, Element parent, SchemaNode schema, PropertyPath path, PropertyEditor parentPropertyEditor, SchemaNode parentSchema, ConfigEditor configEditor, JToken value) : base(desktop, parent, schema, path, parentPropertyEditor, parentSchema, configEditor, value)
		{
			this.ContentHeight = new int?(32);
		}

		// Token: 0x06005F0D RID: 24333 RVA: 0x001E9A60 File Offset: 0x001E7C60
		protected override void Build()
		{
			Label label = new Label(this.Desktop, this);
			JToken value = base.Value;
			label.Text = (((value != null) ? value.ToString(0, Array.Empty<JsonConverter>()) : null) ?? "");
			label.Padding = new Padding
			{
				Left = new int?(5)
			};
			label.Anchor = new Anchor
			{
				Height = new int?(32)
			};
			label.Style = new LabelStyle
			{
				VerticalAlignment = LabelStyle.LabelAlignment.Center,
				TextColor = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 125)
			};
			this._label = label;
		}

		// Token: 0x06005F0E RID: 24334 RVA: 0x001E9B10 File Offset: 0x001E7D10
		protected internal override void UpdateDisplayedValue()
		{
			Label label = this._label;
			JToken value = base.Value;
			label.Text = (((value != null) ? value.ToString(0, Array.Empty<JsonConverter>()) : null) ?? "");
			this._label.Layout(null, true);
		}

		// Token: 0x04003B38 RID: 15160
		private Label _label;
	}
}
