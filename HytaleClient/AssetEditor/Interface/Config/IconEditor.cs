using System;
using HytaleClient.AssetEditor.Data;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Styles;
using Newtonsoft.Json.Linq;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BC1 RID: 3009
	internal class IconEditor : AssetFileSelectorEditor
	{
		// Token: 0x06005E65 RID: 24165 RVA: 0x001E2E10 File Offset: 0x001E1010
		public IconEditor(Desktop desktop, Element parent, SchemaNode schema, PropertyPath path, PropertyEditor parentPropertyEditor, SchemaNode parentSchema, ConfigEditor configEditor, JToken value) : base(desktop, parent, schema, path, parentPropertyEditor, parentSchema, configEditor, value)
		{
		}

		// Token: 0x06005E66 RID: 24166 RVA: 0x001E2E34 File Offset: 0x001E1034
		protected override void Build()
		{
			Button button = new Button(this.Desktop, this);
			button.Anchor = new Anchor
			{
				Width = new int?(30)
			};
			button.Background = new PatchStyle("AssetEditor/EditIcon.png");
			button.Activating = new Action(this.OpenEditModal);
			Group group = new Group(this.Desktop, this);
			group.Background = new PatchStyle(UInt32Color.FromRGBA(0, 0, 0, 70));
			group.Anchor = new Anchor
			{
				Width = new int?(1)
			};
			base.Build();
			this._layoutMode = LayoutMode.Left;
			this._dropdown.FlexWeight = 1;
		}

		// Token: 0x06005E67 RID: 24167 RVA: 0x001E2EE4 File Offset: 0x001E10E4
		private void OpenEditModal()
		{
			this.ConfigEditor.IconExporterModal.Open();
		}
	}
}
