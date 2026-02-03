using System;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000864 RID: 2148
	public abstract class InputElement<T> : Element
	{
		// Token: 0x17001061 RID: 4193
		// (get) Token: 0x06003C43 RID: 15427 RVA: 0x00092F72 File Offset: 0x00091172
		// (set) Token: 0x06003C44 RID: 15428 RVA: 0x00092F7A File Offset: 0x0009117A
		[UIMarkupProperty]
		public virtual T Value { get; set; }

		// Token: 0x06003C45 RID: 15429 RVA: 0x00092F83 File Offset: 0x00091183
		public InputElement(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x04001BDC RID: 7132
		public Action ValueChanged;
	}
}
