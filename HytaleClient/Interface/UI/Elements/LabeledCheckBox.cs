using System;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000867 RID: 2151
	[UIMarkupElement]
	public class LabeledCheckBox : BaseCheckBox<LabeledCheckBox.LabeledCheckBoxStyle, LabeledCheckBoxStyleState>
	{
		// Token: 0x06003C7D RID: 15485 RVA: 0x0009624A File Offset: 0x0009444A
		public LabeledCheckBox(Desktop desktop, Element parent) : base(desktop, parent)
		{
			this._label = new Label(this.Desktop, this);
		}

		// Token: 0x06003C7E RID: 15486 RVA: 0x00096268 File Offset: 0x00094468
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			LabeledCheckBoxStyleState labeledCheckBoxStyleState = this.Value ? this.Style.Checked : this.Style.Unchecked;
			bool disabled = this.Disabled;
			if (disabled)
			{
				Label label = this._label;
				LabelStyle style;
				if ((style = labeledCheckBoxStyleState.DisabledLabelStyle) == null)
				{
					style = (labeledCheckBoxStyleState.DefaultLabelStyle ?? new LabelStyle());
				}
				label.Style = style;
			}
			else
			{
				int? capturedMouseButton = base.CapturedMouseButton;
				long? num = (capturedMouseButton != null) ? new long?((long)capturedMouseButton.GetValueOrDefault()) : null;
				long num2 = (long)((ulong)1);
				bool flag = num.GetValueOrDefault() == num2 & num != null;
				if (flag)
				{
					Label label2 = this._label;
					LabelStyle style2;
					if ((style2 = labeledCheckBoxStyleState.PressedLabelStyle) == null && (style2 = labeledCheckBoxStyleState.HoveredLabelStyle) == null)
					{
						style2 = (labeledCheckBoxStyleState.DefaultLabelStyle ?? new LabelStyle());
					}
					label2.Style = style2;
				}
				else
				{
					bool isHovered = base.IsHovered;
					if (isHovered)
					{
						Label label3 = this._label;
						LabelStyle style3;
						if ((style3 = labeledCheckBoxStyleState.HoveredLabelStyle) == null)
						{
							style3 = (labeledCheckBoxStyleState.DefaultLabelStyle ?? new LabelStyle());
						}
						label3.Style = style3;
					}
					else
					{
						this._label.Style = (labeledCheckBoxStyleState.DefaultLabelStyle ?? new LabelStyle());
					}
				}
			}
			this._label.Text = (labeledCheckBoxStyleState.Text ?? "");
		}

		// Token: 0x04001C07 RID: 7175
		private readonly Label _label;

		// Token: 0x02000D37 RID: 3383
		[UIMarkupData]
		public class LabeledCheckBoxStyle : BaseCheckBoxStyle<LabeledCheckBoxStyleState>
		{
		}
	}
}
