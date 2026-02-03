using System;
using HytaleClient.Data.Items;
using HytaleClient.Interface.Messages;
using HytaleClient.Protocol;
using NLog;
using Utf8Json;

namespace HytaleClient.InGame
{
	// Token: 0x020008E7 RID: 2279
	internal class Notifications
	{
		// Token: 0x0600437A RID: 17274 RVA: 0x000D6752 File Offset: 0x000D4952
		public Notifications(GameInstance gameInstance)
		{
			this._gameInstance = gameInstance;
		}

		// Token: 0x0600437B RID: 17275 RVA: 0x000D6764 File Offset: 0x000D4964
		public void AddNotification(string message, string icon)
		{
			FormattedMessage message2 = new FormattedMessage
			{
				RawText = message
			};
			this._gameInstance.App.Interface.InGameView.NotificationFeedComponent.OnReceiveNotification(new Notifications.ClientNotification(message2, 0, icon));
		}

		// Token: 0x0600437C RID: 17276 RVA: 0x000D67A8 File Offset: 0x000D49A8
		public void AddNotification(Notification notification)
		{
			Notifications.ClientNotification notification2;
			try
			{
				notification2 = new Notifications.ClientNotification(notification);
			}
			catch (Exception value)
			{
				this._gameInstance.Chat.Error("Failed to parse notification!");
				Notifications.Logger.Error<Exception>(value);
				return;
			}
			this._gameInstance.App.Interface.InGameView.NotificationFeedComponent.OnReceiveNotification(notification2);
		}

		// Token: 0x0400213E RID: 8510
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400213F RID: 8511
		private readonly GameInstance _gameInstance;

		// Token: 0x02000DB1 RID: 3505
		public class ClientNotification
		{
			// Token: 0x060065F5 RID: 26101 RVA: 0x00212754 File Offset: 0x00210954
			public ClientNotification(Notification notification)
			{
				bool flag = notification.Message == null;
				if (flag)
				{
					throw new Exception("Message cannot be empty");
				}
				this.Message = JsonSerializer.Deserialize<FormattedMessage>(notification.Message);
				bool flag2 = notification.SecondaryMessage != null;
				if (flag2)
				{
					this.SecondaryMessage = JsonSerializer.Deserialize<FormattedMessage>(notification.SecondaryMessage);
				}
				this.Style = notification.Style;
				this.Icon = notification.Icon;
				bool flag3 = notification.Item_ != null;
				if (flag3)
				{
					this.Item = new ClientItemStack(notification.Item_);
				}
			}

			// Token: 0x060065F6 RID: 26102 RVA: 0x002127E6 File Offset: 0x002109E6
			public ClientNotification(FormattedMessage message, NotificationStyle style, string icon)
			{
				this.Message = message;
				this.Style = style;
				this.Icon = icon;
			}

			// Token: 0x04004379 RID: 17273
			public FormattedMessage Message;

			// Token: 0x0400437A RID: 17274
			public FormattedMessage SecondaryMessage;

			// Token: 0x0400437B RID: 17275
			public NotificationStyle Style;

			// Token: 0x0400437C RID: 17276
			public string Icon;

			// Token: 0x0400437D RID: 17277
			public ClientItemStack Item;
		}
	}
}
