using System;
using System.Collections.Generic;
using HytaleClient.Application;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Utils;

namespace HytaleClient.Interface
{
	// Token: 0x02000800 RID: 2048
	internal class DisconnectionView : InterfaceComponent
	{
		// Token: 0x060038AD RID: 14509 RVA: 0x0007528E File Offset: 0x0007348E
		public DisconnectionView(Interface @interface) : base(@interface, null)
		{
			this._appDisconnection = @interface.App.Disconnection;
		}

		// Token: 0x060038AE RID: 14510 RVA: 0x000752AC File Offset: 0x000734AC
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("Disconnection.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._titleLabel = uifragment.Get<Label>("Title");
			this._reasonLabel = uifragment.Get<Label>("Reason");
			this._detailsLabel = uifragment.Get<Label>("Details");
			uifragment.Get<TextButton>("BackToMainMenuButton").Activating = new Action(this.Dismiss);
			this._reconnectButton = uifragment.Get<TextButton>("ReconnectButton");
			this._reconnectButton.Activating = new Action(this.Reconnect);
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.Setup();
			}
		}

		// Token: 0x060038AF RID: 14511 RVA: 0x0007536B File Offset: 0x0007356B
		protected override void OnMounted()
		{
			this.Setup();
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x060038B0 RID: 14512 RVA: 0x0007538D File Offset: 0x0007358D
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x060038B1 RID: 14513 RVA: 0x000753A8 File Offset: 0x000735A8
		private void Animate(float deltaTime)
		{
			bool flag = this._secondsUntilDoReconnect > 0f;
			if (flag)
			{
				this._secondsUntilDoReconnect -= deltaTime;
				bool flag2 = this._secondsUntilDoReconnect <= 0f;
				if (flag2)
				{
					this._appDisconnection.Reconnect();
				}
			}
			else
			{
				bool flag3 = this._reconnectButton.Disabled || this._secondsUntilAutoReconnect == 0f;
				if (!flag3)
				{
					this._secondsUntilAutoReconnect -= deltaTime;
					this._reconnectButton.Text = this.Desktop.Provider.GetText("ui.disconnection.autoConnectingIn", new Dictionary<string, string>
					{
						{
							"seconds",
							((int)this._secondsUntilAutoReconnect).ToString()
						}
					}, true);
					this._reconnectButton.Layout(null, true);
					bool flag4 = this._secondsUntilAutoReconnect <= 0f;
					if (flag4)
					{
						this.Reconnect();
					}
				}
			}
		}

		// Token: 0x060038B2 RID: 14514 RVA: 0x000754A8 File Offset: 0x000736A8
		private void Setup()
		{
			this._secondsUntilAutoReconnect = OptionsHelper.AutoReconnectDelay;
			this._secondsUntilDoReconnect = 0f;
			this._titleLabel.Text = this.Desktop.Provider.GetText(this._appDisconnection.DisconnectedOnLoadingScreen ? "ui.disconnection.titleFailedToConnect" : "ui.disconnection.titleDisconnected", null, true);
			this._reconnectButton.Text = this.Desktop.Provider.GetText(this._appDisconnection.DisconnectedOnLoadingScreen ? "ui.disconnection.tryAgain" : "ui.disconnection.reconnect", null, true);
			this._reconnectButton.Disabled = false;
			this._reasonLabel.Text = (this._appDisconnection.Reason ?? this.Desktop.Provider.GetText("ui.disconnection.errors.unexpectedError", null, true));
			this._detailsLabel.Text = "(" + this._appDisconnection.ExceptionMessage + ")";
			this._detailsLabel.Parent.Visible = (this._appDisconnection.Reason == null);
			this.Interface.SocialBar.SetContainer(base.Find<Group>("SocialBarContainer"));
		}

		// Token: 0x060038B3 RID: 14515 RVA: 0x000755D9 File Offset: 0x000737D9
		protected internal override void Validate()
		{
			this.Dismiss();
		}

		// Token: 0x060038B4 RID: 14516 RVA: 0x000755E2 File Offset: 0x000737E2
		protected internal override void Dismiss()
		{
			this.Interface.App.MainMenu.Open(this.Interface.App.MainMenu.CurrentPage);
		}

		// Token: 0x060038B5 RID: 14517 RVA: 0x00075610 File Offset: 0x00073810
		private void Reconnect()
		{
			bool disabled = this._reconnectButton.Disabled;
			if (!disabled)
			{
				this._secondsUntilDoReconnect = 0.2f;
				this._reconnectButton.Text = this.Desktop.Provider.GetText(this._appDisconnection.DisconnectedOnLoadingScreen ? "ui.disconnection.tryAgain.inProgress" : "ui.disconnection.reconnect.inProgress", null, true);
				this._reconnectButton.Disabled = true;
				this._reconnectButton.Layout(null, true);
			}
		}

		// Token: 0x04001899 RID: 6297
		private AppDisconnection _appDisconnection;

		// Token: 0x0400189A RID: 6298
		private Label _titleLabel;

		// Token: 0x0400189B RID: 6299
		private Label _reasonLabel;

		// Token: 0x0400189C RID: 6300
		private Label _detailsLabel;

		// Token: 0x0400189D RID: 6301
		private TextButton _reconnectButton;

		// Token: 0x0400189E RID: 6302
		private float _secondsUntilAutoReconnect;

		// Token: 0x0400189F RID: 6303
		private float _secondsUntilDoReconnect;
	}
}
