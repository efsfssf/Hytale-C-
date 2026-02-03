using System;
using HytaleClient.Application.Services;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.Services
{
	// Token: 0x02000814 RID: 2068
	internal class SocialBar : InterfaceComponent
	{
		// Token: 0x0600395C RID: 14684 RVA: 0x0007AB78 File Offset: 0x00078D78
		public SocialBar(Interface @interface, Element parent = null) : base(@interface, parent)
		{
		}

		// Token: 0x0600395D RID: 14685 RVA: 0x0007AB84 File Offset: 0x00078D84
		protected override void OnMounted()
		{
			this.UpdateServiceInformation();
		}

		// Token: 0x0600395E RID: 14686 RVA: 0x0007AB90 File Offset: 0x00078D90
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("Services/SocialBar.ui", out document);
			this._statusOfflineColor = document.ResolveNamedValue<UInt32Color>(this.Interface, "StatusOfflineColor");
			this._statusOnlineColor = document.ResolveNamedValue<UInt32Color>(this.Interface, "StatusOnlineColor");
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			uifragment.Get<Label>("Username").Text = this.Interface.App.AuthManager.Settings.Username;
			this._friendsCount = uifragment.Get<Label>("FriendsCount");
			this._newNotificationsCount = uifragment.Get<Label>("NewNotificationsCount");
			this._statusIcon = uifragment.Get<Group>("StatusIcon");
			this._statusLabel = uifragment.Get<Label>("StatusLabel");
		}

		// Token: 0x0600395F RID: 14687 RVA: 0x0007AC64 File Offset: 0x00078E64
		public void SetContainer(Element container)
		{
			bool hasMarkupError = this.Interface.HasMarkupError;
			if (!hasMarkupError)
			{
				Element parent = this.Parent;
				if (parent != null)
				{
					parent.Remove(this);
				}
				container.Add(this, -1);
			}
		}

		// Token: 0x06003960 RID: 14688 RVA: 0x0007ACA0 File Offset: 0x00078EA0
		public void UpdateServiceInformation()
		{
			bool flag = !base.IsMounted || this.Interface.HasMarkupError;
			if (!flag)
			{
				int num = Math.Min(0, 99);
				int num2 = 0;
				foreach (Guid guid in this.Interface.App.HytaleServices.Friends)
				{
					ClientUserState clientUserState;
					bool flag2 = this.Interface.App.HytaleServices.UserStates.TryGetValue(guid, ref clientUserState) && clientUserState.Online;
					if (flag2)
					{
						num2++;
					}
				}
				this._friendsCount.Text = this.Desktop.Provider.FormatNumber(num2);
				bool flag3 = num > 0;
				if (flag3)
				{
					this._newNotificationsCount.Text = this.Desktop.Provider.FormatNumber(num);
					this._newNotificationsCount.Parent.Visible = true;
				}
				else
				{
					this._newNotificationsCount.Parent.Visible = false;
				}
				bool flag4 = this.Interface.ServiceState == HytaleServices.ServiceState.Connected;
				if (flag4)
				{
					this._statusLabel.Text = this.Desktop.Provider.GetText("ui.socialMenu.status.online", null, true);
					this._statusLabel.Style.TextColor = this._statusOnlineColor;
					this._statusIcon.Background = new PatchStyle("Services/StatusOnline.png");
				}
				else
				{
					switch (this.Interface.ServiceState)
					{
					case HytaleServices.ServiceState.Disconnected:
						this._statusLabel.Text = this.Desktop.Provider.GetText("ui.socialMenu.status.disconnected", null, true);
						break;
					case HytaleServices.ServiceState.Connecting:
						this._statusLabel.Text = this.Desktop.Provider.GetText("ui.socialMenu.status.connecting", null, true);
						break;
					case HytaleServices.ServiceState.Authenticating:
						this._statusLabel.Text = this.Desktop.Provider.GetText("ui.socialMenu.status.authenticating", null, true);
						break;
					}
					this._statusLabel.Style.TextColor = this._statusOfflineColor;
					this._statusIcon.Background = new PatchStyle("Services/StatusOffline.png");
				}
				base.Layout(null, true);
			}
		}

		// Token: 0x04001940 RID: 6464
		private Label _friendsCount;

		// Token: 0x04001941 RID: 6465
		private Label _newNotificationsCount;

		// Token: 0x04001942 RID: 6466
		private Group _statusIcon;

		// Token: 0x04001943 RID: 6467
		private Label _statusLabel;

		// Token: 0x04001944 RID: 6468
		private UInt32Color _statusOnlineColor;

		// Token: 0x04001945 RID: 6469
		private UInt32Color _statusOfflineColor;
	}
}
