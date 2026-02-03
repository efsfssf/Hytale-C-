using System;

namespace HytaleClient.Application
{
	// Token: 0x02000BE5 RID: 3045
	internal class AppDisconnection
	{
		// Token: 0x170013F3 RID: 5107
		// (get) Token: 0x06006086 RID: 24710 RVA: 0x001F9657 File Offset: 0x001F7857
		// (set) Token: 0x06006087 RID: 24711 RVA: 0x001F965F File Offset: 0x001F785F
		public string ExceptionMessage { get; private set; }

		// Token: 0x170013F4 RID: 5108
		// (get) Token: 0x06006088 RID: 24712 RVA: 0x001F9668 File Offset: 0x001F7868
		// (set) Token: 0x06006089 RID: 24713 RVA: 0x001F9670 File Offset: 0x001F7870
		public string Reason { get; private set; }

		// Token: 0x170013F5 RID: 5109
		// (get) Token: 0x0600608A RID: 24714 RVA: 0x001F9679 File Offset: 0x001F7879
		// (set) Token: 0x0600608B RID: 24715 RVA: 0x001F9681 File Offset: 0x001F7881
		public bool DisconnectedOnLoadingScreen { get; private set; }

		// Token: 0x0600608C RID: 24716 RVA: 0x001F968A File Offset: 0x001F788A
		public void SetReason(string reason)
		{
			this.Reason = reason;
		}

		// Token: 0x0600608D RID: 24717 RVA: 0x001F9694 File Offset: 0x001F7894
		public AppDisconnection(App app)
		{
			this._app = app;
		}

		// Token: 0x0600608E RID: 24718 RVA: 0x001F96A5 File Offset: 0x001F78A5
		public void Open(string exceptionMessage, string hostname = null, int port = 0)
		{
			this.ExceptionMessage = exceptionMessage;
			this.DisconnectedOnLoadingScreen = (this._app.Stage == App.AppStage.GameLoading);
			this._hostname = hostname;
			this._port = port;
			this._app.SetStage(App.AppStage.Disconnection);
		}

		// Token: 0x0600608F RID: 24719 RVA: 0x001F96E0 File Offset: 0x001F78E0
		public void CleanUp()
		{
			this.Reason = null;
			this.ExceptionMessage = null;
			this._hostname = null;
			this._port = 0;
		}

		// Token: 0x06006090 RID: 24720 RVA: 0x001F9704 File Offset: 0x001F7904
		public void Reconnect()
		{
			bool flag = this._app.SingleplayerWorldName != null;
			if (flag)
			{
				this._app.GameLoading.Open(this._app.SingleplayerWorldName);
			}
			else
			{
				this._app.GameLoading.Open(this._hostname, this._port, null);
			}
		}

		// Token: 0x04003C17 RID: 15383
		private readonly App _app;

		// Token: 0x04003C1B RID: 15387
		private string _hostname;

		// Token: 0x04003C1C RID: 15388
		private int _port;
	}
}
