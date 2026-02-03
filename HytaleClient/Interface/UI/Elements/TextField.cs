using System;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000878 RID: 2168
	[UIMarkupElement]
	public class TextField : InputField<string>
	{
		// Token: 0x17001084 RID: 4228
		// (get) Token: 0x06003D41 RID: 15681 RVA: 0x0009C135 File Offset: 0x0009A335
		// (set) Token: 0x06003D42 RID: 15682 RVA: 0x0009C140 File Offset: 0x0009A340
		public override string Value
		{
			get
			{
				return this._text;
			}
			set
			{
				bool flag = value != this._text;
				if (flag)
				{
					this._text = value;
					base.CursorIndex = this.Value.Length;
				}
			}
		}

		// Token: 0x17001085 RID: 4229
		// (get) Token: 0x06003D43 RID: 15683 RVA: 0x0009C179 File Offset: 0x0009A379
		// (set) Token: 0x06003D44 RID: 15684 RVA: 0x0009C181 File Offset: 0x0009A381
		[UIMarkupProperty]
		public string PlaceholderText
		{
			get
			{
				return this._placeholderText;
			}
			set
			{
				this._placeholderText = value;
			}
		}

		// Token: 0x06003D45 RID: 15685 RVA: 0x0009C18A File Offset: 0x0009A38A
		public TextField(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}
	}
}
