using System;
using System.Collections.Generic;
using HytaleClient.Interface.Messages;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Protocol;

namespace HytaleClient.AssetEditor.Interface.Editor
{
	// Token: 0x02000BB4 RID: 2996
	internal class ToastNotifications : Element
	{
		// Token: 0x06005DC2 RID: 24002 RVA: 0x001DE20C File Offset: 0x001DC40C
		public ToastNotifications(Desktop desktop, Element parent) : base(desktop, parent)
		{
			this._layoutMode = LayoutMode.Bottom;
			this.Anchor = new Anchor
			{
				Width = new int?(350),
				Right = new int?(10),
				Bottom = new int?(10)
			};
		}

		// Token: 0x06005DC3 RID: 24003 RVA: 0x001DE27D File Offset: 0x001DC47D
		protected override void OnMounted()
		{
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06005DC4 RID: 24004 RVA: 0x001DE298 File Offset: 0x001DC498
		protected override void OnUnmounted()
		{
			base.Clear();
			this._notifications.Clear();
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06005DC5 RID: 24005 RVA: 0x001DE2C8 File Offset: 0x001DC4C8
		private TextButton AddNotification(AssetEditorPopupNotificationType type)
		{
			Document document;
			this.Desktop.Provider.TryGetDocument("AssetEditor/ToastNotification.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			TextButton label = uifragment.Get<TextButton>("Label");
			ToastNotifications.ToastNotification notification = new ToastNotifications.ToastNotification
			{
				Element = label,
				TimeLeft = 5f
			};
			label.Background = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "Background" + type.ToString());
			label.Activating = delegate()
			{
				this._notifications.Remove(notification);
				this.Remove(label);
				this.Layout(null, true);
			};
			this._notifications.Add(notification);
			return label;
		}

		// Token: 0x06005DC6 RID: 24006 RVA: 0x001DE3A4 File Offset: 0x001DC5A4
		public void AddNotification(AssetEditorPopupNotificationType type, string text)
		{
			TextButton textButton = this.AddNotification(type);
			textButton.Text = text;
			base.Layout(null, true);
		}

		// Token: 0x06005DC7 RID: 24007 RVA: 0x001DE3D4 File Offset: 0x001DC5D4
		public void AddNotification(AssetEditorPopupNotificationType type, FormattedMessage message)
		{
			TextButton textButton = this.AddNotification(type);
			textButton.TextSpans = FormattedMessageConverter.GetLabelSpans(message, this.Desktop.Provider, default(SpanStyle), true);
			base.Layout(null, true);
		}

		// Token: 0x06005DC8 RID: 24008 RVA: 0x001DE420 File Offset: 0x001DC620
		private void Animate(float deltaTime)
		{
			bool flag = this._notifications.Count > 0;
			if (flag)
			{
				foreach (ToastNotifications.ToastNotification toastNotification in this._notifications)
				{
					toastNotification.TimeLeft -= deltaTime;
					bool flag2 = toastNotification.TimeLeft <= 0f;
					if (flag2)
					{
						this._removedNotifications.Add(toastNotification);
						base.Remove(toastNotification.Element);
					}
				}
				bool flag3 = this._removedNotifications.Count > 0;
				if (flag3)
				{
					foreach (ToastNotifications.ToastNotification item in this._removedNotifications)
					{
						this._notifications.Remove(item);
					}
					this._removedNotifications.Clear();
					base.Layout(null, true);
				}
			}
		}

		// Token: 0x04003AA8 RID: 15016
		private const float Duration = 5f;

		// Token: 0x04003AA9 RID: 15017
		private readonly List<ToastNotifications.ToastNotification> _notifications = new List<ToastNotifications.ToastNotification>();

		// Token: 0x04003AAA RID: 15018
		private readonly List<ToastNotifications.ToastNotification> _removedNotifications = new List<ToastNotifications.ToastNotification>();

		// Token: 0x02000FC9 RID: 4041
		private class ToastNotification
		{
			// Token: 0x04004C02 RID: 19458
			public Element Element;

			// Token: 0x04004C03 RID: 19459
			public float TimeLeft;
		}
	}
}
