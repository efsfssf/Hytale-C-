using System;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x0200086C RID: 2156
	[UIMarkupElement(AcceptsChildren = true)]
	public class Panel : Group
	{
		// Token: 0x06003CD2 RID: 15570 RVA: 0x00099089 File Offset: 0x00097289
		public Panel(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06003CD3 RID: 15571 RVA: 0x00099098 File Offset: 0x00097298
		public override Element HitTest(Point position)
		{
			bool flag = this._waitingForLayoutAfterMount || !this._anchoredRectangle.Contains(position);
			Element result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = (base.HitTest(position) ?? this);
			}
			return result;
		}
	}
}
