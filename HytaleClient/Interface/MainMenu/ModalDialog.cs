using System;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.MainMenu
{
	// Token: 0x02000819 RID: 2073
	internal class ModalDialog : InterfaceComponent
	{
		// Token: 0x06003979 RID: 14713 RVA: 0x0007C2C5 File Offset: 0x0007A4C5
		public ModalDialog(Interface @interface, ModalDialog.DialogSetup setup = null) : base(@interface, null)
		{
			this._setup = setup;
		}

		// Token: 0x0600397A RID: 14714 RVA: 0x0007C2D8 File Offset: 0x0007A4D8
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("MainMenu/ModalDialog.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._title = uifragment.Get<Label>("TitleLabel");
			this._text = uifragment.Get<Label>("TextLabel");
			this._confirmButton = uifragment.Get<TextButton>("ConfirmButton");
			this._confirmButton.Activating = new Action(this.Confirm);
			this._cancelButton = uifragment.Get<TextButton>("CancelButton");
			this._cancelButton.Activating = new Action(this.Cancel);
			bool flag = this._setup != null;
			if (flag)
			{
				this.Setup(this._setup);
			}
		}

		// Token: 0x0600397B RID: 14715 RVA: 0x0007C39C File Offset: 0x0007A59C
		public void Setup(ModalDialog.DialogSetup setup)
		{
			this._setup = setup;
			this._title.Text = ((setup.Title != null) ? this.Interface.GetText(setup.Title, null, true) : string.Empty);
			this._text.Text = ((setup.Text != null) ? this.Interface.GetText(setup.Text, null, true) : string.Empty);
			this._confirmButton.Text = this.Interface.GetText(setup.ConfirmationText ?? "ui.general.confirm", null, true);
			this._cancelButton.Text = this.Interface.GetText(setup.CancelText ?? "ui.general.cancel", null, true);
			this._cancelButton.Visible = setup.Cancellable;
		}

		// Token: 0x0600397C RID: 14716 RVA: 0x0007C46F File Offset: 0x0007A66F
		protected override void OnUnmounted()
		{
			this._setup = null;
		}

		// Token: 0x0600397D RID: 14717 RVA: 0x0007C47C File Offset: 0x0007A67C
		private void Confirm()
		{
			Action onConfirm = this._setup.OnConfirm;
			this.Desktop.ClearLayer(4);
			if (onConfirm != null)
			{
				onConfirm();
			}
		}

		// Token: 0x0600397E RID: 14718 RVA: 0x0007C4B0 File Offset: 0x0007A6B0
		private void Cancel()
		{
			Action onCancel = this._setup.OnCancel;
			this.Desktop.ClearLayer(4);
			if (onCancel != null)
			{
				onCancel();
			}
		}

		// Token: 0x0600397F RID: 14719 RVA: 0x0007C4E3 File Offset: 0x0007A6E3
		protected internal override void Validate()
		{
			this.Confirm();
		}

		// Token: 0x06003980 RID: 14720 RVA: 0x0007C4EC File Offset: 0x0007A6EC
		protected internal override void Dismiss()
		{
			bool dismissable = this._setup.Dismissable;
			if (dismissable)
			{
				this.Cancel();
			}
		}

		// Token: 0x04001963 RID: 6499
		private Label _title;

		// Token: 0x04001964 RID: 6500
		private Label _text;

		// Token: 0x04001965 RID: 6501
		private TextButton _confirmButton;

		// Token: 0x04001966 RID: 6502
		private TextButton _cancelButton;

		// Token: 0x04001967 RID: 6503
		private ModalDialog.DialogSetup _setup;

		// Token: 0x02000CF0 RID: 3312
		public class DialogSetup
		{
			// Token: 0x0400403E RID: 16446
			public string Title;

			// Token: 0x0400403F RID: 16447
			public string Text;

			// Token: 0x04004040 RID: 16448
			public string ConfirmationText;

			// Token: 0x04004041 RID: 16449
			public string CancelText;

			// Token: 0x04004042 RID: 16450
			public bool Cancellable = true;

			// Token: 0x04004043 RID: 16451
			public bool Dismissable = true;

			// Token: 0x04004044 RID: 16452
			public Action OnConfirm;

			// Token: 0x04004045 RID: 16453
			public Action OnCancel;
		}
	}
}
