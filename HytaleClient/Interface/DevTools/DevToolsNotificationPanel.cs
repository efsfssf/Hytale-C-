using System;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.DevTools
{
	// Token: 0x020008D7 RID: 2263
	internal class DevToolsNotificationPanel : Element
	{
		// Token: 0x060041D8 RID: 16856 RVA: 0x000C465F File Offset: 0x000C285F
		public DevToolsNotificationPanel(Interface @interface) : base(@interface.Desktop, null)
		{
			this._interface = @interface;
		}

		// Token: 0x060041D9 RID: 16857 RVA: 0x000C4678 File Offset: 0x000C2878
		public void Build()
		{
			base.Clear();
			Document document;
			this.Desktop.Provider.TryGetDocument("DevTools/NotificationPanel.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._errorPanel = uifragment.Get<Group>("ErrorPanel");
			this._warningPanel = uifragment.Get<Group>("WarningPanel");
			this._errorLabel = uifragment.Get<Label>("ErrorLabel");
			this._errorLabel.Text = this.Desktop.Provider.FormatNumber(this._unreadErrors);
			this._errorPanel.Visible = (this._unreadErrors > 0);
			this._warningLabel = uifragment.Get<Label>("WarningLabel");
			this._warningLabel.Text = this.Desktop.Provider.FormatNumber(this._unreadWarnings);
			this._warningPanel.Visible = (this._unreadWarnings > 0);
		}

		// Token: 0x060041DA RID: 16858 RVA: 0x000C4768 File Offset: 0x000C2968
		public void AddUnreadError(int count)
		{
			this._unreadErrors += count;
			bool flag = this._interface.HasMarkupError || !this._interface.HasLoaded;
			if (!flag)
			{
				this._errorLabel.Text = this.Desktop.Provider.FormatNumber(this._unreadErrors);
				this._errorPanel.Visible = true;
			}
		}

		// Token: 0x060041DB RID: 16859 RVA: 0x000C47D8 File Offset: 0x000C29D8
		public void ClearUnread()
		{
			bool flag = this._unreadErrors == 0 && this._unreadWarnings == 0;
			if (!flag)
			{
				this._unreadErrors = 0;
				this._unreadWarnings = 0;
				bool flag2 = this._interface.HasMarkupError || !this._interface.HasLoaded;
				if (!flag2)
				{
					this._errorPanel.Visible = false;
					this._warningPanel.Visible = false;
					bool isMounted = base.IsMounted;
					if (isMounted)
					{
						this._warningPanel.Parent.Layout(null, true);
					}
				}
			}
		}

		// Token: 0x060041DC RID: 16860 RVA: 0x000C4874 File Offset: 0x000C2A74
		public void AddUnreadWarning(int count)
		{
			this._unreadWarnings += count;
			bool flag = this._interface.HasMarkupError || !this._interface.HasLoaded;
			if (!flag)
			{
				this._warningLabel.Text = this.Desktop.Provider.FormatNumber(this._unreadWarnings);
				this._warningPanel.Visible = true;
			}
		}

		// Token: 0x04002024 RID: 8228
		private Group _errorPanel;

		// Token: 0x04002025 RID: 8229
		private Group _warningPanel;

		// Token: 0x04002026 RID: 8230
		private Label _errorLabel;

		// Token: 0x04002027 RID: 8231
		private Label _warningLabel;

		// Token: 0x04002028 RID: 8232
		private int _unreadErrors;

		// Token: 0x04002029 RID: 8233
		private int _unreadWarnings;

		// Token: 0x0400202A RID: 8234
		private readonly Interface _interface;
	}
}
