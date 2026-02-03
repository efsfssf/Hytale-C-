using System;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000854 RID: 2132
	[UIMarkupElement]
	public class CheckBox : BaseCheckBox<CheckBox.CheckBoxStyle, CheckBoxStyleState>
	{
		// Token: 0x06003B2A RID: 15146 RVA: 0x0008AEAC File Offset: 0x000890AC
		public CheckBox(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x02000D25 RID: 3365
		[UIMarkupData]
		public class CheckBoxStyle : BaseCheckBoxStyle<CheckBoxStyleState>
		{
		}
	}
}
