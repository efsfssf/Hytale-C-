using System;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000861 RID: 2145
	[UIMarkupElement(AcceptsChildren = true)]
	public class Group : Element
	{
		// Token: 0x1700105B RID: 4187
		// (get) Token: 0x06003C18 RID: 15384 RVA: 0x00092341 File Offset: 0x00090541
		// (set) Token: 0x06003C19 RID: 15385 RVA: 0x00092349 File Offset: 0x00090549
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

		// Token: 0x1700105C RID: 4188
		// (get) Token: 0x06003C1A RID: 15386 RVA: 0x00092352 File Offset: 0x00090552
		// (set) Token: 0x06003C1B RID: 15387 RVA: 0x0009235A File Offset: 0x0009055A
		[UIMarkupProperty]
		public ScrollbarStyle ScrollbarStyle
		{
			get
			{
				return this._scrollbarStyle;
			}
			set
			{
				this._scrollbarStyle = value;
			}
		}

		// Token: 0x1700105D RID: 4189
		// (set) Token: 0x06003C1C RID: 15388 RVA: 0x00092363 File Offset: 0x00090563
		public Action Scrolled
		{
			set
			{
				this._scrolled = value;
			}
		}

		// Token: 0x06003C1D RID: 15389 RVA: 0x0009236C File Offset: 0x0009056C
		public Group(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06003C1E RID: 15390 RVA: 0x00092378 File Offset: 0x00090578
		protected internal override void Validate()
		{
			bool flag = this.Validating != null;
			if (flag)
			{
				this.Validating();
			}
			else
			{
				base.Validate();
			}
		}

		// Token: 0x06003C1F RID: 15391 RVA: 0x000923A8 File Offset: 0x000905A8
		protected internal override void Dismiss()
		{
			bool flag = this.Dismissing != null;
			if (flag)
			{
				this.Dismissing();
			}
			else
			{
				base.Dismiss();
			}
		}

		// Token: 0x04001BC2 RID: 7106
		public Action Validating;

		// Token: 0x04001BC3 RID: 7107
		public Action Dismissing;
	}
}
