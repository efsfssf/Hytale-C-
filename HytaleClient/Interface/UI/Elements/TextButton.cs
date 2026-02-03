using System;
using System.Collections.Generic;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000877 RID: 2167
	[UIMarkupElement]
	public class TextButton : BaseButton<TextButton.TextButtonStyle, TextButton.TextButtonStyleState>
	{
		// Token: 0x17001082 RID: 4226
		// (set) Token: 0x06003D3D RID: 15677 RVA: 0x0009C08C File Offset: 0x0009A28C
		[UIMarkupProperty]
		public string Text
		{
			set
			{
				this._label.Text = value;
			}
		}

		// Token: 0x17001083 RID: 4227
		// (set) Token: 0x06003D3E RID: 15678 RVA: 0x0009C09B File Offset: 0x0009A29B
		[UIMarkupProperty]
		public IList<Label.LabelSpan> TextSpans
		{
			set
			{
				this._label.TextSpans = value;
			}
		}

		// Token: 0x06003D3F RID: 15679 RVA: 0x0009C0AA File Offset: 0x0009A2AA
		public TextButton(Desktop desktop, Element parent) : base(desktop, parent)
		{
			this._label = new Label(this.Desktop, this);
		}

		// Token: 0x06003D40 RID: 15680 RVA: 0x0009C0C8 File Offset: 0x0009A2C8
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			this._stateBackgroundPatch = ((this._styleState.Background != null) ? this.Desktop.MakeTexturePatch(this._styleState.Background) : null);
			this._label.Style = this._styleState.LabelStyle;
			this._label.MaskTexturePath = this._styleState.LabelMaskTexturePath;
		}

		// Token: 0x04001C83 RID: 7299
		private readonly Label _label;

		// Token: 0x02000D48 RID: 3400
		[UIMarkupData]
		public class TextButtonStyle : BaseButtonStyle<TextButton.TextButtonStyleState>
		{
		}

		// Token: 0x02000D49 RID: 3401
		[UIMarkupData]
		public class TextButtonStyleState
		{
			// Token: 0x0400416F RID: 16751
			public PatchStyle Background;

			// Token: 0x04004170 RID: 16752
			public LabelStyle LabelStyle = new LabelStyle();

			// Token: 0x04004171 RID: 16753
			public UIPath LabelMaskTexturePath;
		}
	}
}
