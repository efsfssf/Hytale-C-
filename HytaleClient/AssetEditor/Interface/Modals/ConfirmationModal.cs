using System;
using System.Diagnostics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.AssetEditor.Interface.Modals
{
	// Token: 0x02000B9E RID: 2974
	internal class ConfirmationModal : Element
	{
		// Token: 0x170013A3 RID: 5027
		// (get) Token: 0x06005BED RID: 23533 RVA: 0x001CD6E2 File Offset: 0x001CB8E2
		public bool ApplyChangesLocally
		{
			get
			{
				return this._applyChangesLocallyContainer.Visible && this._applyChangesLocally.Value;
			}
		}

		// Token: 0x06005BEE RID: 23534 RVA: 0x001CD6FF File Offset: 0x001CB8FF
		public ConfirmationModal(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06005BEF RID: 23535 RVA: 0x001CD70C File Offset: 0x001CB90C
		public void Build()
		{
			base.Clear();
			Document document;
			this.Desktop.Provider.TryGetDocument("AssetEditor/Modal.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._titleLabel = uifragment.Get<Label>("TitleLabel");
			this._textlabel = uifragment.Get<Label>("TextLabel");
			this._confirmationButton = uifragment.Get<TextButton>("ConfirmationButton");
			this._confirmationButton.Activating = new Action(this.Validate);
			this._cancelButton = uifragment.Get<TextButton>("CancelButton");
			this._cancelButton.Activating = new Action(this.Dismiss);
			this._applyChangesLocallyContainer = uifragment.Get<Group>("ApplyChangesLocallyContainer");
			this._applyChangesLocally = uifragment.Get<CheckBox>("ApplyChangesLocally");
		}

		// Token: 0x06005BF0 RID: 23536 RVA: 0x001CD7E0 File Offset: 0x001CB9E0
		protected internal override void Dismiss()
		{
			Action onDismiss = this._onDismiss;
			this.Close();
			if (onDismiss != null)
			{
				onDismiss();
			}
		}

		// Token: 0x06005BF1 RID: 23537 RVA: 0x001CD808 File Offset: 0x001CBA08
		protected internal override void Validate()
		{
			Action onConfirm = this._onConfirm;
			this.Close();
			if (onConfirm != null)
			{
				onConfirm();
			}
		}

		// Token: 0x06005BF2 RID: 23538 RVA: 0x001CD830 File Offset: 0x001CBA30
		protected override void OnUnmounted()
		{
			this._onDismiss = null;
			this._onConfirm = null;
		}

		// Token: 0x06005BF3 RID: 23539 RVA: 0x001CD844 File Offset: 0x001CBA44
		public void Open(string title, string text, Action onConfirm = null, Action onDismiss = null, string confirmText = null, string abortText = null, bool displayApplyLocalChangeCheckBox = false)
		{
			this._confirmationButton.Text = (confirmText ?? this.Desktop.Provider.GetText("ui.general.confirm", null, true));
			this._cancelButton.Text = (abortText ?? this.Desktop.Provider.GetText("ui.general.cancel", null, true));
			this._titleLabel.Text = title;
			this._textlabel.Text = text;
			this._onConfirm = onConfirm;
			this._onDismiss = onDismiss;
			this._applyChangesLocallyContainer.Visible = displayApplyLocalChangeCheckBox;
			this.Desktop.SetTransientLayer(this);
		}

		// Token: 0x06005BF4 RID: 23540 RVA: 0x001CD8E8 File Offset: 0x001CBAE8
		private void Close()
		{
			Debug.Assert(this.Desktop.GetTransientLayer() == this);
			this.Desktop.SetTransientLayer(null);
		}

		// Token: 0x0400398A RID: 14730
		private Label _titleLabel;

		// Token: 0x0400398B RID: 14731
		private Label _textlabel;

		// Token: 0x0400398C RID: 14732
		private TextButton _confirmationButton;

		// Token: 0x0400398D RID: 14733
		private TextButton _cancelButton;

		// Token: 0x0400398E RID: 14734
		private Group _applyChangesLocallyContainer;

		// Token: 0x0400398F RID: 14735
		private CheckBox _applyChangesLocally;

		// Token: 0x04003990 RID: 14736
		private Action _onConfirm;

		// Token: 0x04003991 RID: 14737
		private Action _onDismiss;
	}
}
