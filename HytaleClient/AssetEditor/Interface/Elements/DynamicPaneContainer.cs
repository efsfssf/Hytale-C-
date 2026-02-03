using System;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.AssetEditor.Interface.Elements
{
	// Token: 0x02000BAA RID: 2986
	[UIMarkupElement(AcceptsChildren = true)]
	public class DynamicPaneContainer : Group
	{
		// Token: 0x06005CA4 RID: 23716 RVA: 0x001D4030 File Offset: 0x001D2230
		public DynamicPaneContainer(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06005CA5 RID: 23717 RVA: 0x001D403C File Offset: 0x001D223C
		protected override void AfterChildrenLayout()
		{
			bool flag = this._layoutMode != LayoutMode.Left && this._layoutMode != LayoutMode.Right && this._layoutMode != LayoutMode.Top && this._layoutMode != LayoutMode.Bottom;
			if (!flag)
			{
				this.EnsureChildrenMinMaxSize();
			}
		}

		// Token: 0x06005CA6 RID: 23718 RVA: 0x001D4084 File Offset: 0x001D2284
		private void EnsureChildrenMinMaxSize()
		{
			bool flag = this._layoutMode == LayoutMode.Top || this._layoutMode == LayoutMode.Bottom;
			int num = flag ? this._anchoredRectangle.Height : this._anchoredRectangle.Width;
			int num2 = 0;
			int num3 = 0;
			foreach (Element element in base.Children)
			{
				bool flag2 = !element.IsMounted;
				if (!flag2)
				{
					DynamicPane dynamicPane = element as DynamicPane;
					bool flag3 = dynamicPane != null;
					if (flag3)
					{
						bool flag4 = element.FlexWeight > 0;
						if (flag4)
						{
							num -= this.Desktop.ScaleRound((float)dynamicPane.MinSize);
						}
						else
						{
							num2 = (flag ? element.AnchoredRectangle.Height : element.AnchoredRectangle.Width);
							num3++;
						}
					}
					else
					{
						bool flag5 = flag && element.Anchor.Height != null;
						if (flag5)
						{
							num -= this.Desktop.ScaleRound((float)element.Anchor.Height.Value);
						}
						else
						{
							bool flag6 = !flag && element.Anchor.Width != null;
							if (flag6)
							{
								num -= this.Desktop.ScaleRound((float)element.Anchor.Width.Value);
							}
							else
							{
								bool flag7 = element.FlexWeight > 0;
								if (flag7)
								{
									num -= this.Desktop.ScaleRound(50f);
								}
							}
						}
					}
				}
			}
			bool flag8 = num3 > 0 && num2 > num;
			if (flag8)
			{
				int num4 = (int)Math.Round((double)((float)(num2 - num) / (float)num3), MidpointRounding.ToEven);
				foreach (Element element2 in base.Children)
				{
					DynamicPane dynamicPane2 = element2 as DynamicPane;
					bool flag9 = dynamicPane2 == null || element2.FlexWeight > 0 || !element2.IsMounted;
					if (!flag9)
					{
						bool flag10 = flag;
						if (flag10)
						{
							Element element3 = element2;
							element3.Anchor.Height = element3.Anchor.Height - num4;
							int? num5 = element2.Anchor.Height;
							int minSize = dynamicPane2.MinSize;
							bool flag11 = num5.GetValueOrDefault() < minSize & num5 != null;
							if (flag11)
							{
								element2.Anchor.Height = new int?(dynamicPane2.MinSize);
							}
						}
						else
						{
							Element element4 = element2;
							element4.Anchor.Width = element4.Anchor.Width - num4;
							int? num5 = element2.Anchor.Width;
							int minSize = dynamicPane2.MinSize;
							bool flag12 = num5.GetValueOrDefault() < minSize & num5 != null;
							if (flag12)
							{
								element2.Anchor.Width = new int?(dynamicPane2.MinSize);
							}
						}
					}
				}
				base.LayoutChildren();
			}
		}

		// Token: 0x04003A09 RID: 14857
		public const int FlexMinSize = 50;
	}
}
