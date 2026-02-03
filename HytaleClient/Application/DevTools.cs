using System;
using System.Collections.Concurrent;
using HytaleClient.Interface.DevTools;
using NLog;

namespace HytaleClient.Application
{
	// Token: 0x02000BEA RID: 3050
	internal class DevTools
	{
		// Token: 0x17001409 RID: 5129
		// (get) Token: 0x06006144 RID: 24900 RVA: 0x001FF8B6 File Offset: 0x001FDAB6
		// (set) Token: 0x06006145 RID: 24901 RVA: 0x001FF8BE File Offset: 0x001FDABE
		public bool IsOpen { get; private set; }

		// Token: 0x06006146 RID: 24902 RVA: 0x001FF8C7 File Offset: 0x001FDAC7
		public DevTools(App app)
		{
			this._app = app;
			this.IsDiagnosticsModeEnabled = this._app.Settings.DiagnosticMode;
		}

		// Token: 0x06006147 RID: 24903 RVA: 0x001FF8FC File Offset: 0x001FDAFC
		public void Info(string message)
		{
			DevTools.Logger.Info(message);
			bool flag = !this.IsDiagnosticsModeEnabled;
			if (!flag)
			{
				this._messageQueue.Enqueue(Tuple.Create<DevToolsOverlay.MessageType, string>(DevToolsOverlay.MessageType.Info, message));
			}
		}

		// Token: 0x06006148 RID: 24904 RVA: 0x001FF93C File Offset: 0x001FDB3C
		public void Error(string message)
		{
			DevTools.Logger.Error(message);
			bool flag = !this.IsDiagnosticsModeEnabled;
			if (!flag)
			{
				this._messageQueue.Enqueue(Tuple.Create<DevToolsOverlay.MessageType, string>(DevToolsOverlay.MessageType.Error, message));
			}
		}

		// Token: 0x06006149 RID: 24905 RVA: 0x001FF97C File Offset: 0x001FDB7C
		public void Warn(string message)
		{
			DevTools.Logger.Warn(message);
			bool flag = !this.IsDiagnosticsModeEnabled;
			if (!flag)
			{
				this._messageQueue.Enqueue(Tuple.Create<DevToolsOverlay.MessageType, string>(DevToolsOverlay.MessageType.Warning, message));
			}
		}

		// Token: 0x0600614A RID: 24906 RVA: 0x001FF9BC File Offset: 0x001FDBBC
		public void HandleMessageQueue()
		{
			bool isEmpty = this._messageQueue.IsEmpty;
			if (!isEmpty)
			{
				DevToolsOverlay devTools = this._app.Interface.DevToolsLayer.DevTools;
				DevToolsNotificationPanel devToolsNotificationPanel = this._app.Interface.DevToolsNotificationPanel;
				int num = 0;
				int num2 = 0;
				for (;;)
				{
					Tuple<DevToolsOverlay.MessageType, string> tuple;
					bool flag = this._messageQueue.TryDequeue(out tuple);
					if (!flag)
					{
						break;
					}
					bool flag2 = !this.IsDiagnosticsModeEnabled;
					if (!flag2)
					{
						devTools.AddConsoleMessage(tuple.Item1, tuple.Item2);
						DevToolsOverlay.MessageType item = tuple.Item1;
						DevToolsOverlay.MessageType messageType = item;
						if (messageType != DevToolsOverlay.MessageType.Warning)
						{
							if (messageType == DevToolsOverlay.MessageType.Error)
							{
								num2++;
							}
						}
						else
						{
							num++;
						}
					}
				}
				bool flag3 = !this.IsOpen && (this._app.Stage != App.AppStage.InGame || this._app.InGame.IsHudVisible) && (num > 0 || num2 > 0);
				if (flag3)
				{
					bool flag4 = num2 > 0;
					if (flag4)
					{
						devToolsNotificationPanel.AddUnreadError(num2);
					}
					bool flag5 = num > 0;
					if (flag5)
					{
						devToolsNotificationPanel.AddUnreadWarning(num);
					}
					bool isMounted = devToolsNotificationPanel.IsMounted;
					if (isMounted)
					{
						devToolsNotificationPanel.Layout(null, true);
					}
				}
				devTools.LayoutLog();
			}
		}

		// Token: 0x0600614B RID: 24907 RVA: 0x001FFAF8 File Offset: 0x001FDCF8
		public void Open()
		{
			this.IsOpen = true;
			this._app.Interface.Desktop.SetLayer(5, this._app.Interface.DevToolsLayer);
			this._app.Interface.DevToolsNotificationPanel.ClearUnread();
			bool flag = this._app.Stage == App.AppStage.InGame;
			if (flag)
			{
				this._app.InGame.UpdateInputStates(false);
			}
		}

		// Token: 0x0600614C RID: 24908 RVA: 0x001FFB74 File Offset: 0x001FDD74
		public void Close()
		{
			this.IsOpen = false;
			this._app.Interface.Desktop.ClearLayer(5);
			bool flag = this._app.Stage == App.AppStage.InGame;
			if (flag)
			{
				this._app.InGame.UpdateInputStates(false);
			}
		}

		// Token: 0x0600614D RID: 24909 RVA: 0x001FFBC7 File Offset: 0x001FDDC7
		public void ClearNotifications()
		{
			this._app.Interface.DevToolsNotificationPanel.ClearUnread();
		}

		// Token: 0x04003C68 RID: 15464
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003C69 RID: 15465
		private readonly App _app;

		// Token: 0x04003C6A RID: 15466
		public volatile bool IsDiagnosticsModeEnabled;

		// Token: 0x04003C6C RID: 15468
		private readonly ConcurrentQueue<Tuple<DevToolsOverlay.MessageType, string>> _messageQueue = new ConcurrentQueue<Tuple<DevToolsOverlay.MessageType, string>>();
	}
}
