using System;
using System.Collections.Generic;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;

namespace HytaleClient.Interface.Services
{
	// Token: 0x02000813 RID: 2067
	internal class QueueStatus : InterfaceComponent
	{
		// Token: 0x06003958 RID: 14680 RVA: 0x0007AA19 File Offset: 0x00078C19
		public QueueStatus(Interface @interface) : base(@interface, null)
		{
			base.Visible = false;
		}

		// Token: 0x06003959 RID: 14681 RVA: 0x0007AA30 File Offset: 0x00078C30
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("Services/QueueStatus.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._statusTextLabel = uifragment.Get<Label>("StatusText");
			this._queueNameLabel = uifragment.Get<Label>("QueueName");
			uifragment.Get<TextButton>("LeaveButton").Activating = delegate()
			{
				this.Interface.TriggerEvent("services.leaveGameQueue", null, null, null, null, null, null);
			};
		}

		// Token: 0x0600395A RID: 14682 RVA: 0x0007AAA4 File Offset: 0x00078CA4
		public void Update()
		{
			bool hasMarkupError = this.Interface.HasMarkupError;
			if (!hasMarkupError)
			{
				bool flag = this.Interface.QueueTicketName != null;
				if (flag)
				{
					base.Visible = true;
					this._statusTextLabel.Text = (this.Interface.QueueTicketStatus ?? "");
					this._queueNameLabel.Text = this.Interface.GetText("ui.socialMenu.queuedFor", new Dictionary<string, string>
					{
						{
							"game",
							this.Interface.QueueTicketName
						}
					}, true);
					base.Layout(new Rectangle?(this.Parent.ContainerRectangle), true);
				}
				else
				{
					base.Visible = false;
				}
			}
		}

		// Token: 0x0400193E RID: 6462
		private Label _statusTextLabel;

		// Token: 0x0400193F RID: 6463
		private Label _queueNameLabel;
	}
}
