using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000855 RID: 2133
	[UIMarkupElement(AcceptsChildren = true)]
	public class CheckBoxContainer : Group
	{
		// Token: 0x06003B2B RID: 15147 RVA: 0x0008AEB8 File Offset: 0x000890B8
		public CheckBoxContainer(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06003B2C RID: 15148 RVA: 0x0008AEC4 File Offset: 0x000890C4
		public override Element HitTest(Point position)
		{
			Debug.Assert(base.IsMounted);
			bool flag = !this._anchoredRectangle.Contains(position);
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

		// Token: 0x06003B2D RID: 15149 RVA: 0x0008AF08 File Offset: 0x00089108
		private InputElement<bool> FindCheckBox()
		{
			return CheckBoxContainer.<FindCheckBox>g__TraverseChildren|2_0(this);
		}

		// Token: 0x06003B2E RID: 15150 RVA: 0x0008AF24 File Offset: 0x00089124
		private bool IsCheckBoxDisabled(Element element)
		{
			CheckBox checkBox = element as CheckBox;
			bool result;
			if (checkBox == null)
			{
				LabeledCheckBox labeledCheckBox = element as LabeledCheckBox;
				result = (labeledCheckBox == null || labeledCheckBox.Disabled);
			}
			else
			{
				result = checkBox.Disabled;
			}
			return result;
		}

		// Token: 0x06003B2F RID: 15151 RVA: 0x0008AF6C File Offset: 0x0008916C
		protected override void OnMouseEnter()
		{
			InputElement<bool> inputElement = this.FindCheckBox();
			bool flag = inputElement == null || this.IsCheckBoxDisabled(inputElement);
			if (!flag)
			{
				SDL.SDL_SetCursor(this.Desktop.Cursors.Hand);
			}
		}

		// Token: 0x06003B30 RID: 15152 RVA: 0x0008AFAA File Offset: 0x000891AA
		protected override void OnMouseLeave()
		{
			SDL.SDL_SetCursor(this.Desktop.Cursors.Arrow);
		}

		// Token: 0x06003B31 RID: 15153 RVA: 0x0008AFC4 File Offset: 0x000891C4
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			InputElement<bool> inputElement = this.FindCheckBox();
			bool flag = inputElement == null || this.IsCheckBoxDisabled(inputElement);
			if (!flag)
			{
				inputElement.Value = !inputElement.Value;
				inputElement.Layout(null, true);
			}
		}

		// Token: 0x06003B32 RID: 15154 RVA: 0x0008B010 File Offset: 0x00089210
		[CompilerGenerated]
		internal static InputElement<bool> <FindCheckBox>g__TraverseChildren|2_0(Element element)
		{
			foreach (Element element2 in element.Children)
			{
				bool flag = element2 is CheckBox || element2 is LabeledCheckBox;
				if (flag)
				{
					return (InputElement<bool>)element2;
				}
				InputElement<bool> inputElement = CheckBoxContainer.<FindCheckBox>g__TraverseChildren|2_0(element2);
				bool flag2 = inputElement != null;
				if (flag2)
				{
					return inputElement;
				}
			}
			return null;
		}
	}
}
