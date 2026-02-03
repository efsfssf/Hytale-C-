using System;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.UI.Styles
{
	// Token: 0x02000839 RID: 2105
	[UIMarkupData]
	public class LabeledCheckBoxStyleState : CheckBoxStyleState
	{
		// Token: 0x04001ACE RID: 6862
		public LabelStyle DefaultLabelStyle;

		// Token: 0x04001ACF RID: 6863
		public LabelStyle HoveredLabelStyle;

		// Token: 0x04001AD0 RID: 6864
		public LabelStyle PressedLabelStyle;

		// Token: 0x04001AD1 RID: 6865
		public LabelStyle DisabledLabelStyle;

		// Token: 0x04001AD2 RID: 6866
		public string Text;
	}
}
