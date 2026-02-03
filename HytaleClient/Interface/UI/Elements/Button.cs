using System;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000853 RID: 2131
	[UIMarkupElement(AcceptsChildren = true)]
	public class Button : BaseButton<Button.ButtonStyle, Button.ButtonStyleState>
	{
		// Token: 0x17001030 RID: 4144
		// (get) Token: 0x06003B26 RID: 15142 RVA: 0x0008AE59 File Offset: 0x00089059
		// (set) Token: 0x06003B27 RID: 15143 RVA: 0x0008AE61 File Offset: 0x00089061
		[UIMarkupProperty]
		public new LayoutMode LayoutMode
		{
			get
			{
				return this._layoutMode;
			}
			set
			{
				this._layoutMode = value;
			}
		}

		// Token: 0x06003B28 RID: 15144 RVA: 0x0008AE6A File Offset: 0x0008906A
		public Button(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06003B29 RID: 15145 RVA: 0x0008AE76 File Offset: 0x00089076
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			this._stateBackgroundPatch = ((this._styleState.Background != null) ? this.Desktop.MakeTexturePatch(this._styleState.Background) : null);
		}

		// Token: 0x02000D23 RID: 3363
		[UIMarkupData]
		public class ButtonStyle : BaseButtonStyle<Button.ButtonStyleState>
		{
		}

		// Token: 0x02000D24 RID: 3364
		[UIMarkupData]
		public class ButtonStyleState
		{
			// Token: 0x040040E9 RID: 16617
			public PatchStyle Background;
		}
	}
}
