using System;
using System.Collections.Generic;
using HytaleClient.Data.Items;
using HytaleClient.Graphics;
using HytaleClient.InGame;
using HytaleClient.Interface.Messages;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Protocol;

namespace HytaleClient.Interface.InGame.Hud
{
	// Token: 0x020008C0 RID: 2240
	internal class NotificationFeedComponent : InterfaceComponent
	{
		// Token: 0x060040FB RID: 16635 RVA: 0x000BD68C File Offset: 0x000BB88C
		public NotificationFeedComponent(InGameView inGameView) : base(inGameView.Interface, inGameView.HudContainer)
		{
			this._inGameView = inGameView;
		}

		// Token: 0x060040FC RID: 16636 RVA: 0x000BD6B4 File Offset: 0x000BB8B4
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Hud/NotificationFeed.ui", out document);
			this._itemGridStyle = document.ResolveNamedValue<ItemGrid.ItemGridStyle>(this.Interface, "ItemGridStyle");
			this._itemGridStyle.SlotBackground = null;
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._notificationFeed = uifragment.Get<Group>("NotificationFeed");
			this.RebuildFeed();
		}

		// Token: 0x060040FD RID: 16637 RVA: 0x000BD725 File Offset: 0x000BB925
		protected override void OnMounted()
		{
			this._currentTime = 0f;
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x060040FE RID: 16638 RVA: 0x000BD74B File Offset: 0x000BB94B
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x060040FF RID: 16639 RVA: 0x000BD766 File Offset: 0x000BB966
		public void ResetState()
		{
			this._currentTime = 0f;
			this._notifications.Clear();
			this._notificationFeed.Clear();
		}

		// Token: 0x06004100 RID: 16640 RVA: 0x000BD78C File Offset: 0x000BB98C
		public void RebuildFeed()
		{
			this._notificationFeed.Clear();
			Document doc;
			this.Interface.TryGetDocument("InGame/Hud/Notification.ui", out doc);
			foreach (NotificationFeedComponent.Notification notification in this._notifications)
			{
				this.AddNotification(notification, doc);
			}
			this._notificationFeed.Layout(null, true);
		}

		// Token: 0x06004101 RID: 16641 RVA: 0x000BD81C File Offset: 0x000BBA1C
		private void Animate(float deltaTime)
		{
			bool flag = false;
			this._currentTime += deltaTime;
			while (this._notifications.Count > 0)
			{
				NotificationFeedComponent.Notification notification = this._notifications[0];
				bool flag2 = notification.ExpirationTime < this._currentTime;
				if (!flag2)
				{
					break;
				}
				this._notifications.RemoveAt(0);
				this._notificationFeed.Remove(notification.Element);
				notification.Element = null;
				flag = true;
			}
			bool flag3 = flag;
			if (flag3)
			{
				this._notificationFeed.Layout(null, true);
			}
		}

		// Token: 0x06004102 RID: 16642 RVA: 0x000BD8BC File Offset: 0x000BBABC
		private void AddNotification(NotificationFeedComponent.Notification notification, Document doc)
		{
			UIFragment uifragment = doc.Instantiate(this.Desktop, this._notificationFeed);
			UInt32Color value = doc.ResolveNamedValue<UInt32Color>(this.Interface, "MessageColor" + notification.Style.ToString());
			uifragment.Get<Label>("Message").TextSpans = FormattedMessageConverter.GetLabelSpans(notification.Message, this.Interface, new SpanStyle
			{
				Color = new UInt32Color?(value)
			}, true);
			bool flag = notification.SecondaryMessage != null;
			if (flag)
			{
				UInt32Color value2 = doc.ResolveNamedValue<UInt32Color>(this.Interface, "SecondaryMessageColor" + notification.Style.ToString());
				uifragment.Get<Label>("SecondaryMessage").TextSpans = FormattedMessageConverter.GetLabelSpans(notification.SecondaryMessage, this.Interface, new SpanStyle
				{
					Color = new UInt32Color?(value2)
				}, true);
			}
			else
			{
				uifragment.Get<Label>("SecondaryMessage").Visible = false;
			}
			TextureArea textureArea;
			bool flag2 = notification.Icon != null && this._inGameView.TryMountAssetTexture(notification.Icon, out textureArea);
			if (flag2)
			{
				uifragment.Get<Group>("Icon").Background = new PatchStyle(textureArea);
			}
			else
			{
				ClientItemBase clientItemBase;
				bool flag3 = notification.Item != null && this._inGameView.Items.TryGetValue(notification.Item.Id, out clientItemBase);
				if (flag3)
				{
					ItemGrid itemGrid = new ItemGrid(this.Desktop, uifragment.Get<Group>("Icon"))
					{
						SlotsPerRow = 1,
						Slots = new ItemGridSlot[1],
						RenderItemQualityBackground = false,
						Style = this._itemGridStyle
					};
					itemGrid.Slots[0] = new ItemGridSlot(notification.Item);
				}
			}
			notification.Element = uifragment.Get<Group>("Notification");
		}

		// Token: 0x06004103 RID: 16643 RVA: 0x000BDAA4 File Offset: 0x000BBCA4
		public void OnReceiveNotification(Notifications.ClientNotification notification)
		{
			Document doc;
			this.Interface.TryGetDocument("InGame/Hud/Notification.ui", out doc);
			NotificationFeedComponent.Notification notification2 = new NotificationFeedComponent.Notification
			{
				Message = notification.Message,
				SecondaryMessage = notification.SecondaryMessage,
				ExpirationTime = this._currentTime + 5f,
				Icon = notification.Icon,
				Item = notification.Item,
				Style = notification.Style
			};
			this.AddNotification(notification2, doc);
			this._notifications.Add(notification2);
			this._notificationFeed.Layout(null, true);
		}

		// Token: 0x04001F38 RID: 7992
		public const float NotificationDuration = 5f;

		// Token: 0x04001F39 RID: 7993
		private float _currentTime;

		// Token: 0x04001F3A RID: 7994
		private InGameView _inGameView;

		// Token: 0x04001F3B RID: 7995
		private Group _notificationFeed;

		// Token: 0x04001F3C RID: 7996
		private ItemGrid.ItemGridStyle _itemGridStyle;

		// Token: 0x04001F3D RID: 7997
		private List<NotificationFeedComponent.Notification> _notifications = new List<NotificationFeedComponent.Notification>();

		// Token: 0x02000D82 RID: 3458
		private class Notification
		{
			// Token: 0x04004236 RID: 16950
			public FormattedMessage Message;

			// Token: 0x04004237 RID: 16951
			public FormattedMessage SecondaryMessage;

			// Token: 0x04004238 RID: 16952
			public float ExpirationTime;

			// Token: 0x04004239 RID: 16953
			public NotificationStyle Style;

			// Token: 0x0400423A RID: 16954
			public string Icon;

			// Token: 0x0400423B RID: 16955
			public ClientItemStack Item;

			// Token: 0x0400423C RID: 16956
			public Group Element;
		}
	}
}
